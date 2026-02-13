// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehBuild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using tools;
using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class CityBehBuild : BehaviourActionCity
{
  private static readonly List<BuildOrder> _possible_buildings = new List<BuildOrder>();
  private static readonly List<BuildOrder> _possible_buildings_no_resources = new List<BuildOrder>();
  private static readonly List<TileZone> _possible_zones = new List<TileZone>();

  public override bool shouldRetry(City pCity) => false;

  public override BehResult execute(City pCity)
  {
    if ((double) pCity.timer_build > 0.0 || !DebugConfig.isOn(DebugOption.SystemBuildTick) || pCity.isGettingCaptured() || pCity.isInDanger())
      return BehResult.Continue;
    pCity.timer_build = 5f;
    CityBehBuild.buildTick(pCity);
    if (DebugConfig.isOn(DebugOption.CityFastUpgrades))
      pCity.timer_build = 0.2f;
    return BehResult.Continue;
  }

  public static bool buildTick(City pCity)
  {
    if (pCity.buildings.Count > 2 && pCity.hasCulture() && pCity.culture.canUseRoads())
    {
      Building random = pCity.buildings.GetRandom<Building>();
      if (random != null)
        CityBehBuild.makeRoadsBuildings(pCity, random);
    }
    if (pCity.isCityUnderDangerFire() && pCity.hasLeader() && !pCity.leader.isImmuneToFire())
      return false;
    if (pCity.under_construction_building == null)
    {
      foreach (Building building in pCity.buildings)
      {
        if (building.isUnderConstruction())
        {
          pCity.under_construction_building = building;
          break;
        }
      }
    }
    if (pCity.under_construction_building != null)
      return false;
    CityBehBuild.calcPossibleBuildings(pCity);
    if (DebugConfig.isOn(DebugOption.OverlayCity))
    {
      pCity._debug_last_possible_build_orders = string.Empty;
      pCity._debug_last_possible_build_orders_no_resources = string.Empty;
      pCity._debug_last_build_order_try = string.Empty;
    }
    if (CityBehBuild._possible_buildings.Count == 0)
      return false;
    BuildOrder random1 = CityBehBuild._possible_buildings.GetRandom<BuildOrder>();
    if (DebugConfig.isOn(DebugOption.OverlayCity))
    {
      foreach (BuildOrder possibleBuilding in CityBehBuild._possible_buildings)
      {
        City city = pCity;
        city._debug_last_possible_build_orders = $"{city._debug_last_possible_build_orders}{(possibleBuilding.upgrade ? "U-" : "")}{possibleBuilding.id}; ";
      }
      foreach (BuildOrder buildingsNoResource in CityBehBuild._possible_buildings_no_resources)
      {
        City city = pCity;
        city._debug_last_possible_build_orders_no_resources = $"{city._debug_last_possible_build_orders_no_resources}{(buildingsNoResource.upgrade ? "U-" : "")}{buildingsNoResource.id}; ";
      }
      pCity._debug_last_build_order_try = (random1.upgrade ? "U-" : "") + random1.id;
    }
    CityBehBuild._possible_buildings_no_resources.Clear();
    CityBehBuild._possible_buildings.Clear();
    if (random1.upgrade)
    {
      List<Building> buildingListOfId = pCity.getBuildingListOfID(random1.getBuildingAsset(pCity).id);
      if (buildingListOfId == null)
        return false;
      Building random2 = buildingListOfId.GetRandom<Building>();
      return random2 != null && CityBehBuild.upgradeBuilding(random2, pCity);
    }
    Building build = CityBehBuild.tryToBuild(pCity, random1.getBuildingAsset(pCity));
    if (build == null)
      return false;
    if (DebugConfig.isOn(DebugOption.CityFastConstruction))
    {
      build?.updateBuild(1000);
      pCity.under_construction_building = (Building) null;
    }
    if (pCity.hasCulture())
      pCity.culture.canUseRoads();
    return true;
  }

  private void upgradeRandomBuilding(City pCity)
  {
    if (pCity.buildings.Count == 0)
      return;
    foreach (Building pBuilding in pCity.buildings.LoopRandom<Building>())
    {
      if (pBuilding.canBeUpgraded())
      {
        CityBehBuild.upgradeBuilding(pBuilding, pCity);
        break;
      }
    }
  }

  public static bool upgradeBuilding(Building pBuilding, City pCity)
  {
    string upgradeTo = pBuilding.asset.upgrade_to;
    BuildingAsset buildingAsset = AssetManager.buildings.get(upgradeTo);
    if (!pCity.hasEnoughResourcesFor(buildingAsset.cost))
      return false;
    int num = pBuilding.upgradeBuilding() ? 1 : 0;
    if (num == 0)
      return num != 0;
    pCity.spendResourcesForBuildingAsset(buildingAsset.cost);
    return num != 0;
  }

  public static void calcPossibleBuildings(City pCity)
  {
    ActorAsset actorAsset = pCity.getActorAsset();
    CityBuildOrderAsset cityBuildOrderAsset = AssetManager.city_build_orders.get(actorAsset.build_order_template_id);
    bool flag = DebugConfig.isOn(DebugOption.OverlayCity);
    foreach (BuildOrder pBuildAsset in cityBuildOrderAsset.list)
    {
      if (CityBehBuild.canUseBuildAsset(pBuildAsset, pCity))
      {
        if (!CityBehBuild.hasResourcesForBuildAsset(pBuildAsset, pCity))
        {
          if (flag)
            CityBehBuild._possible_buildings_no_resources.Add(pBuildAsset);
        }
        else
          CityBehBuild._possible_buildings.Add(pBuildAsset);
      }
    }
  }

  public static bool hasResourcesForBuildAsset(BuildOrder pBuildAsset, City pCity)
  {
    BuildingAsset buildingAsset = pBuildAsset.getBuildingAsset(pCity);
    return pCity.hasEnoughResourcesFor(buildingAsset.cost);
  }

  public static bool canUseBuildAsset(BuildOrder pBuildAsset, City pCity)
  {
    BuildingAsset buildingAsset = pBuildAsset.getBuildingAsset(pCity);
    if (pBuildAsset.min_zones != 0 && pCity.zones.Count < pBuildAsset.min_zones)
      return false;
    int num = pCity.countBuildingsType(buildingAsset.type, false);
    if (pBuildAsset.check_house_limit)
    {
      if (pCity.status.housing_free > 10)
        return false;
      int houseLimit = pCity.getHouseLimit();
      if (num >= houseLimit)
        return false;
    }
    int limitOfBuildingsType = pCity.getLimitOfBuildingsType(pBuildAsset);
    if (limitOfBuildingsType != 0 && num >= limitOfBuildingsType || pBuildAsset.check_full_village && pCity.status.housing_free != 0 || pCity.status.population < pBuildAsset.required_pop || pCity.buildings.Count < pBuildAsset.required_buildings || !CityBehBuild.haveRequiredBuildings(pBuildAsset, pCity) || !CityBehBuild.haveRequiredBuildingTypes(pBuildAsset.requirements_types, pCity))
      return false;
    if (pBuildAsset.upgrade)
    {
      List<Building> buildingListOfId = pCity.getBuildingListOfID(buildingAsset.id);
      if (buildingListOfId == null || buildingListOfId.Count == 0)
        return false;
    }
    else if (buildingAsset.docks && CityBehBuild.getDockTile(pCity) == null)
      return false;
    return true;
  }

  private static bool haveRequiredBuildings(BuildOrder pOrder, City pCity)
  {
    if (pOrder.requirements_orders == null)
      return true;
    for (int index = 0; index < pOrder.requirements_orders.Length; ++index)
    {
      string requirementsOrder = pOrder.requirements_orders[index];
      BuildingAsset buildingAsset = pOrder.getBuildingAsset(pCity, requirementsOrder);
      if (buildingAsset.id == buildingAsset.upgrade_to)
      {
        Debug.LogError((object) ("(!) Building is set to be upgraded to self: " + buildingAsset.id));
      }
      else
      {
        for (; pCity.countBuildingsOfID(buildingAsset.id) == 0; buildingAsset = AssetManager.buildings.get(buildingAsset.upgrade_to))
        {
          if (!buildingAsset.can_be_upgraded || string.IsNullOrEmpty(buildingAsset.upgrade_to))
            return false;
        }
      }
    }
    return true;
  }

  private static bool haveRequiredBuildingTypes(string[] pRequiredBuildingTypes, City pCity)
  {
    if (pRequiredBuildingTypes == null)
      return true;
    for (int index = 0; index < pRequiredBuildingTypes.Length; ++index)
    {
      string requiredBuildingType = pRequiredBuildingTypes[index];
      if (!pCity.hasBuildingType(requiredBuildingType))
        return false;
    }
    return true;
  }

  public static Building tryToBuild(City pCity, BuildingAsset pBuildingAsset)
  {
    if (!pCity.hasEnoughResourcesFor(pBuildingAsset.cost))
      return (Building) null;
    WorldTile pTile = (WorldTile) null;
    List<TileZone> possibleZones = CityBehBuild._possible_zones;
    if (pBuildingAsset.type == "type_training_dummies")
      pTile = CityBehBuild.getTileTrainingDummy(pBuildingAsset, pCity);
    else if (pBuildingAsset.docks)
    {
      pTile = CityBehBuild.getDockTile(pCity);
    }
    else
    {
      if (pBuildingAsset.build_prefer_replace_house)
        pTile = CityBehBuild.getOnHouseTile(pCity, pBuildingAsset);
      if (pTile == null)
        CityBehBuild.fillPossibleZones(pBuildingAsset, pCity, possibleZones);
    }
    if (pTile == null && possibleZones.Count > 0)
    {
      if (pBuildingAsset.build_place_center)
        pTile = CityBehBuild.tryToBuildInZones(possibleZones, pBuildingAsset, pCity, true);
      if (pTile == null)
        pTile = CityBehBuild.tryToBuildInZones(possibleZones, pBuildingAsset, pCity);
      if (pTile != null && pBuildingAsset.needs_farms_ground && !CityBehBuild.checkFarmGround(pTile, pBuildingAsset, pCity))
        pTile = (WorldTile) null;
    }
    possibleZones.Clear();
    if (pTile == null)
      return (Building) null;
    Building build = BehaviourActionBase<City>.world.buildings.addBuilding(pBuildingAsset, pTile);
    pCity.under_construction_building = build;
    build.setUnderConstruction();
    pCity.spendResourcesForBuildingAsset(pBuildingAsset.cost);
    return build;
  }

  private static void fillPossibleZones(
    BuildingAsset pBuildingAsset,
    City pCity,
    List<TileZone> pPossibleZones)
  {
    for (int index = 0; index < pCity.zones.Count; ++index)
    {
      TileZone zone = pCity.zones[index];
      if ((!pBuildingAsset.build_place_single || CityBehBuild.isZonesClear(zone, pBuildingAsset, pCity)) && (!pBuildingAsset.build_place_batch || !CityBehBuild.isNearbySingleBuilding(zone, pBuildingAsset, pCity)) && (!pBuildingAsset.build_place_borders || CityBehBuild.isZoneNearbyBorder(zone, pBuildingAsset, pCity)))
        pPossibleZones.Add(zone);
    }
  }

  public static WorldTile getOnHouseTile(City pCity, BuildingAsset pAsset)
  {
    foreach (Building building in pCity.buildings.LoopRandom<Building>())
    {
      if (building.asset.priority <= pAsset.priority && building.asset.hasHousingSlots() && CityBehBuild.isGoodTileForBuilding(building.current_tile, pAsset, pCity))
        return building.current_tile;
    }
    return (WorldTile) null;
  }

  public static WorldTile tryToBuildInZones(
    List<TileZone> pList,
    BuildingAsset pBuildingAsset,
    City pCity,
    bool pForceCenterZone = false)
  {
    CityLayoutTilePlacement placementFromZone = pCity.getTilePlacementFromZone();
    int num1 = !pCity.hasCulture() ? 0 : (pCity.culture.hasTrait("buildings_spread") ? 1 : 0);
    bool flag1 = pCity.hasSpecialTownPlans();
    WorldTile tile = pCity.getTile();
    TileZone zone = tile?.zone;
    WorldTile buildInZones = (WorldTile) null;
    int num2 = int.MaxValue;
    bool flag2 = num1 == 0 && tile != null;
    foreach (TileZone tileZone in pList.LoopRandom<TileZone>())
    {
      WorldTile pT2 = (WorldTile) null;
      if (pForceCenterZone)
      {
        if (CityBehBuild.isGoodTileForBuilding(tileZone.centerTile, pBuildingAsset, pCity))
          pT2 = tileZone.centerTile;
      }
      else if (pBuildingAsset.docks)
        pT2 = CityBehBuild.tryToBuildInZoneRandomly(tileZone, pBuildingAsset, pCity);
      else if (!flag1 || pCity.planAllowsToPlaceBuildingInZone(tileZone, zone))
        pT2 = CityBehBuild.getTileBasedOnLayout(placementFromZone, tileZone, pBuildingAsset, pCity);
      else
        continue;
      if (pT2 != null)
      {
        if (flag2)
        {
          int num3 = Toolbox.SquaredDistTile(tile, pT2);
          if (num3 < num2)
          {
            buildInZones = pT2;
            num2 = num3;
          }
        }
        else
        {
          buildInZones = pT2;
          break;
        }
      }
    }
    return buildInZones;
  }

  private static WorldTile getTileBasedOnLayout(
    CityLayoutTilePlacement pCityLayoutTilePlacement,
    TileZone pTileZone,
    BuildingAsset pBuildingAsset,
    City pCity)
  {
    switch (pCityLayoutTilePlacement)
    {
      case CityLayoutTilePlacement.Random:
        WorldTile buildInZoneRandomly = CityBehBuild.tryToBuildInZoneRandomly(pTileZone, pBuildingAsset, pCity);
        if (buildInZoneRandomly != null)
          return buildInZoneRandomly;
        break;
      case CityLayoutTilePlacement.CenterTile:
        WorldTile buildInZoneCenter = CityBehBuild.tryToBuildInZoneCenter(pTileZone, pBuildingAsset, pCity);
        if (buildInZoneCenter != null)
          return buildInZoneCenter;
        break;
      case CityLayoutTilePlacement.CenterTileDrunk:
        WorldTile buildInZoneDrunk = CityBehBuild.tryToBuildInZoneDrunk(pTileZone, pBuildingAsset, pCity);
        if (buildInZoneDrunk != null)
          return buildInZoneDrunk;
        break;
      case CityLayoutTilePlacement.Moonsteps:
        WorldTile buildInZoneMoonsteps = CityBehBuild.tryToBuildInZoneMoonsteps(pTileZone, pBuildingAsset, pCity);
        if (buildInZoneMoonsteps != null)
          return buildInZoneMoonsteps;
        break;
    }
    return (WorldTile) null;
  }

  private static WorldTile tryToBuildInZoneMoonsteps(
    TileZone pZone,
    BuildingAsset pBuildingAsset,
    City pCity)
  {
    WorldTile pTile = (pZone.x + pZone.y) % 2 == 0 ? pZone.centerTile.tile_up.tile_up.tile_up : pZone.centerTile.tile_down.tile_down.tile_down;
    return CityBehBuild.isGoodTileForBuilding(pTile, pBuildingAsset, pCity) ? pTile : (WorldTile) null;
  }

  private static WorldTile tryToBuildInZoneDrunk(
    TileZone pZone,
    BuildingAsset pBuildingAsset,
    City pCity)
  {
    WorldTile buildInZoneDrunk = CityBehBuild.tryToBuildInZoneCenter(pZone, pBuildingAsset, pCity);
    if (Randy.randomChance(0.6f) && buildInZoneDrunk != null)
    {
      WorldTile random = buildInZoneDrunk.neighboursAll.GetRandom<WorldTile>();
      if (CityBehBuild.isGoodTileForBuilding(random, pBuildingAsset, pCity))
        buildInZoneDrunk = random;
    }
    return buildInZoneDrunk;
  }

  private static WorldTile tryToBuildInZoneRandomly(
    TileZone pZone,
    BuildingAsset pBuildingAsset,
    City pCity)
  {
    foreach (WorldTile pTile in pZone.tiles.LoopRandom<WorldTile>())
    {
      if (CityBehBuild.isGoodTileForBuilding(pTile, pBuildingAsset, pCity))
        return pTile;
    }
    return (WorldTile) null;
  }

  private static WorldTile tryToBuildInZoneCenter(
    TileZone pZone,
    BuildingAsset pBuildingAsset,
    City pCity)
  {
    return !CityBehBuild.isGoodTileForBuilding(pZone.centerTile, pBuildingAsset, pCity) ? (WorldTile) null : pZone.centerTile;
  }

  internal static bool isZoneNearbyBorder(TileZone pParentZone, BuildingAsset pAsset, City pCity)
  {
    foreach (TileZone tileZone in pParentZone.neighbours_all)
    {
      if (tileZone.city != pCity)
        return true;
    }
    return false;
  }

  internal static bool isNearbySingleBuilding(
    TileZone pParentZone,
    BuildingAsset pAsset,
    City pCity)
  {
    if (CityBehBuild.checkZoneNearbySignleBuilding(pParentZone, pAsset, pCity))
      return true;
    foreach (TileZone pZone in pParentZone.neighbours_all)
    {
      if (CityBehBuild.checkZoneNearbySignleBuilding(pZone, pAsset, pCity))
        return true;
    }
    return false;
  }

  internal static bool checkZoneNearbySignleBuilding(
    TileZone pZone,
    BuildingAsset pAsset,
    City pCity)
  {
    if (!pZone.hasBuildingOf(pCity))
      return false;
    HashSet<Building> hashset = pZone.getHashset(BuildingList.Civs);
    if (hashset != null)
    {
      foreach (Building building in hashset)
      {
        if (building.asset.build_place_single)
          return true;
      }
    }
    return false;
  }

  internal static bool isZonesNearbyBuilding(
    TileZone pParentZone,
    BuildingAsset pAsset,
    City pCity)
  {
    if (CityBehBuild.checkZoneNearbyBuilding(pParentZone, pAsset, pCity))
      return true;
    foreach (TileZone pZone in pParentZone.neighbours_all)
    {
      if (CityBehBuild.checkZoneNearbyBuilding(pZone, pAsset, pCity))
        return true;
    }
    return false;
  }

  internal static bool checkZoneNearbyBuilding(TileZone pZone, BuildingAsset pAsset, City pCity)
  {
    return pZone.isSameCityHere(pCity) && pZone.hasBuildingOf(pCity);
  }

  internal static bool isZonesClear(TileZone pParentZone, BuildingAsset pAsset, City pCity)
  {
    if (!CityBehBuild.checkZoneClear(pParentZone, pAsset, pCity))
      return false;
    foreach (TileZone pZone in pParentZone.neighbours_all)
    {
      if (!CityBehBuild.checkZoneClear(pZone, pAsset, pCity))
        return false;
    }
    return true;
  }

  internal static bool checkFarmGround(WorldTile pTile, BuildingAsset pAsset, City pCity)
  {
    int num = 0 + CityBehBuild.countGoodForFarms(pTile.region, pCity);
    for (int index1 = 0; index1 < pTile.region.neighbours.Count; ++index1)
    {
      MapRegion neighbour = pTile.region.neighbours[index1];
      List<TileZone> zones = neighbour.chunk.zones;
      for (int index2 = 0; index2 < zones.Count; ++index2)
      {
        if (zones[index2].city == pCity)
          num += CityBehBuild.countGoodForFarms(neighbour, pCity);
      }
    }
    return num > 30;
  }

  internal static int countGoodForFarms(MapRegion pRegion, City pCity)
  {
    int num = 0;
    List<WorldTile> tiles = pRegion.tiles;
    for (int index = 0; index < tiles.Count; ++index)
    {
      if (tiles[index].Type.can_be_farm)
        ++num;
    }
    return num;
  }

  internal static bool checkZoneClear(TileZone pZone, BuildingAsset pAsset, City pCity)
  {
    return !pZone.hasAnyBuildingsInSet(BuildingList.Civs);
  }

  public static bool isGoodTileForBuilding(WorldTile pTile, BuildingAsset pAsset, City pCity)
  {
    return pTile.canBuildOn(pAsset) && BehaviourActionBase<City>.world.buildings.canBuildFrom(pTile, pAsset, pCity);
  }

  public static void debugRoards(City pCity, Building pBuilding)
  {
    CityBehBuild.makeRoadsBuildings(pCity, pBuilding);
  }

  public static void makeRoadsBuildings(City pCity, Building pBuilding)
  {
    if (pCity.road_tiles_to_build.Count > 0 || !pBuilding.asset.build_road_to)
      return;
    WorldTile currentTile = pBuilding.current_tile;
    if (currentTile.Type.liquid)
      return;
    using (ListPool<WorldTile> pArray = new ListPool<WorldTile>(pCity.buildings.Count))
    {
      foreach (Building building in pCity.buildings)
      {
        if (building != pBuilding && building.asset.build_road_to && !building.current_tile.Type.liquid && building.current_tile.isSameIsland(pBuilding.current_tile))
          pArray.Add(building.current_tile);
      }
      if (pArray.Count == 0)
        return;
      bool pForceFinished = false;
      if (DebugConfig.isOn(DebugOption.CityFastConstruction))
        pForceFinished = true;
      WorldTile closestTile1 = Toolbox.getClosestTile(pArray, currentTile);
      if (closestTile1 != null)
      {
        pArray.Remove(closestTile1);
        MapAction.makeRoadBetween(closestTile1, currentTile, pCity, pForceFinished);
      }
      WorldTile closestTile2 = Toolbox.getClosestTile(pArray, currentTile);
      if (closestTile2 == null)
        return;
      MapAction.makeRoadBetween(closestTile2, currentTile, pCity, pForceFinished);
    }
  }

  public static WorldTile getTileTrainingDummy(BuildingAsset pBuildingAsset, City pCity)
  {
    // ISSUE: unable to decompile the method.
  }

  public static WorldTile getDockTile(City pCity)
  {
    BuildingAsset buildingDockAsset = pCity.getActorAsset().getBuildingDockAsset();
    if (buildingDockAsset == null)
      return (WorldTile) null;
    OceanHelper.clearOceanPools();
    OceanHelper.saveOceanPoolsWithDocks(pCity);
    if (pCity.getTile() == null)
      return (WorldTile) null;
    foreach (TileZone tileZone in pCity.zones.LoopRandom<TileZone>())
    {
      if (tileZone.tiles_with_liquid != 0)
      {
        MapChunk chunk = tileZone.chunk;
        if (chunk.regions.Count > 1)
        {
          bool flag = false;
          for (int index = 0; index < chunk.regions.Count; ++index)
          {
            MapRegion region = chunk.regions[index];
            if (region.type == TileLayerType.Ocean && OceanHelper.goodForNewDock(region.tiles[0]))
              flag = true;
          }
          if (flag)
          {
            foreach (WorldTile pTile in tileZone.tiles.LoopRandom<WorldTile>())
            {
              if (pTile.Type.ocean && BehaviourActionBase<City>.world.buildings.canBuildFrom(pTile, buildingDockAsset, pCity))
                return pTile;
            }
          }
        }
      }
    }
    return (WorldTile) null;
  }
}
