// Decompiled with JetBrains decompiler
// Type: ai.ActorTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ai;

public static class ActorTool
{
  public static int countContagiousNearby(Actor pActor)
  {
    int num = 0;
    foreach (Actor actor in Finder.getUnitsFromChunk(pActor.current_tile, 1, 10f))
    {
      if (actor.hasTrait("contagious"))
        ++num;
    }
    return num;
  }

  public static Building findNewBuildingTarget(Actor pActor, string pType, bool pOnlyFreeTile = true)
  {
    using (ListPool<Building> listPool = new ListPool<Building>(64 /*0x40*/))
    {
      switch (pType)
      {
        case "new_building":
          Building buildingToBuild = pActor.city.getBuildingToBuild();
          if (buildingToBuild != null && buildingToBuild.tiles.Count != 0)
          {
            WorldTile constructionTile = buildingToBuild.getConstructionTile();
            if (constructionTile != null && constructionTile.isSameIsland(pActor.current_tile))
              return buildingToBuild;
            break;
          }
          break;
        case "random_house_building":
          using (IEnumerator<Building> enumerator = pActor.city.buildings.LoopRandom<Building>().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Building current = enumerator.Current;
              if (current.isSameIslandAs((BaseSimObject) pActor) && !current.isUnderConstruction() && current.isUsable() && current.asset.hasHousingSlots())
                return current;
            }
            break;
          }
        case "ruins":
          using (IEnumerator<TileZone> enumerator = pActor.city.zones.LoopRandom<TileZone>().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              TileZone current = enumerator.Current;
              HashSet<Building> hashset1 = current.getHashset(BuildingList.Ruins);
              if (hashset1 != null)
                listPool.AddRange((IEnumerable<Building>) hashset1);
              HashSet<Building> hashset2 = current.getHashset(BuildingList.Abandoned);
              if (hashset2 != null)
                listPool.AddRange((IEnumerable<Building>) hashset2);
              if (listPool.Count > 0)
                break;
            }
            break;
          }
        case "type_mine":
        case "type_bonfire":
        case "type_training_dummies":
          Building buildingOfType = pActor.city.getBuildingOfType(pType, pOnlyFreeTile: pOnlyFreeTile, pLimitIsland: pActor.current_island);
          if (buildingOfType != null)
            return buildingOfType;
          break;
        default:
          return ActorTool.findNewTargetInZones(pActor, pType, listPool);
      }
      return listPool.Count == 0 ? (Building) null : listPool.GetRandom<Building>();
    }
  }

  public static Building findNewTargetInZones(
    Actor pActor,
    string pType,
    ListPool<Building> pPossibleBuildings)
  {
    foreach (TileZone zone in pActor.city.zones)
    {
      HashSet<Building> buildingSet = (HashSet<Building>) null;
      switch (pType)
      {
        case "type_flower":
        case "type_vegetation":
          buildingSet = zone.getHashset(BuildingList.Flora);
          break;
        case "type_fruits":
          buildingSet = zone.getHashset(BuildingList.Food);
          break;
        case "type_hive":
          buildingSet = zone.getHashset(BuildingList.Hives);
          break;
        case "type_mineral":
          buildingSet = zone.getHashset(BuildingList.Minerals);
          break;
        case "type_poop":
          buildingSet = zone.getHashset(BuildingList.Poops);
          break;
        case "type_tree":
          buildingSet = zone.getHashset(BuildingList.Trees);
          break;
      }
      if (buildingSet != null && buildingSet.Count != 0)
      {
        foreach (Building building in buildingSet)
        {
          BuildingAsset asset = building.asset;
          if (!building.current_tile.isTargeted() && building.isSameIslandAs((BaseSimObject) pActor) && (asset.building_type != BuildingType.Building_Fruits && asset.building_type != BuildingType.Building_Tree || building.hasResourcesToCollect()) && (asset.building_type != BuildingType.Building_Tree || asset.can_be_chopped_down))
            pPossibleBuildings.Add(building);
        }
        if (pPossibleBuildings.Count > 0)
          break;
      }
    }
    return pPossibleBuildings.Count == 0 ? (Building) null : pPossibleBuildings.GetRandom<Building>();
  }

  public static WorldTile getTileNearby(ActorTileTarget pTarget, MapChunk pChunk)
  {
    using (ListPool<WorldTile> tPossibleMoves = new ListPool<WorldTile>(64 /*0x40*/))
    {
      (MapChunk[] mapChunkArray, int num) = Toolbox.getAllChunksFromChunk(pChunk);
      for (int index1 = 0; index1 < num; ++index1)
      {
        MapChunk mapChunk = mapChunkArray[index1];
        if (tPossibleMoves.Count <= 20)
        {
          WorldTile[] tiles = mapChunk.tiles;
          int length = tiles.Length;
          for (int index2 = 0; index2 < length; ++index2)
          {
            WorldTile tTile = tiles[index2];
            if (tPossibleMoves.Count <= 20)
            {
              switch (pTarget)
              {
                case ActorTileTarget.RandomTNT:
                  if (tTile.Type.explodable)
                  {
                    tPossibleMoves.Add(tTile);
                    continue;
                  }
                  continue;
                case ActorTileTarget.RandomBurnableTile:
                  if (tTile.Type.burnable)
                  {
                    tPossibleMoves.Add(tTile);
                    continue;
                  }
                  continue;
                case ActorTileTarget.RandomTileWithUnits:
                  tTile.doUnits((Action<Actor>) (_ => tPossibleMoves.Add(tTile)));
                  continue;
                case ActorTileTarget.RandomTileWithCivStructures:
                  if (tTile.hasBuilding() && tTile.building.hasCity())
                    tPossibleMoves.Add(tTile);
                  if (tTile.Type.burnable && tTile.zone.city != null)
                  {
                    tPossibleMoves.Add(tTile);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      return tPossibleMoves.Count == 0 ? (WorldTile) null : tPossibleMoves.GetRandom<WorldTile>();
    }
  }

  public static Docks getDockTradeTarget(Actor pActor)
  {
    return ActorTool.getDockTradeTarget(pActor.current_tile, pActor);
  }

  private static Docks getDockTradeTarget(WorldTile pTile, Actor pActor)
  {
    return ActorTool.getDockTradeTarget(pTile.region, pActor);
  }

  private static Docks getDockTradeTarget(MapRegion pRegion, Actor pActor)
  {
    return ActorTool.getDockTradeTarget(pRegion.island, pActor);
  }

  private static Docks getDockTradeTarget(TileIsland pIsland, Actor pActor)
  {
    return ActorTool.getDockTradeTarget(pIsland.docks, pActor);
  }

  private static Docks getDockTradeTarget(ListPool<Docks> pList, Actor pActor)
  {
    if (pList == null || pList.Count == 0)
      return (Docks) null;
    foreach (Docks dockTradeTarget in pList.LoopRandom<Docks>())
    {
      if (dockTradeTarget.building.hasCity() && pActor.getHomeBuilding() != dockTradeTarget.building && dockTradeTarget.building.isUsable() && !dockTradeTarget.building.isAbandoned() && !dockTradeTarget.building.city.kingdom.isEnemy(pActor.kingdom) && (dockTradeTarget.isDockGood() || dockTradeTarget.hasOceanTiles()))
        return dockTradeTarget;
    }
    return (Docks) null;
  }

  public static WorldTile getRandomTileForBoat(Actor pActor)
  {
    MapRegion mapRegion = pActor.current_tile.region;
    if (mapRegion.neighbours.Count > 0 && Randy.randomBool())
      mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
    return mapRegion.tiles.Count > 0 ? mapRegion.tiles.GetRandom<WorldTile>() : (WorldTile) null;
  }

  public static int attributeDice(Actor pActor, int pAmount = 2)
  {
    if (pActor == null)
      return int.MinValue;
    int num1 = 0;
    int pMaxExclusive = (int) ((double) pActor.stats["diplomacy"] + (double) pActor.stats["warfare"] + (double) pActor.stats["stewardship"]);
    if (pActor.hasCulture())
    {
      Culture culture = pActor.culture;
      bool flag = culture.hasTrait("patriarchy");
      int num2 = culture.hasTrait("matriarchy") ? 1 : 0;
      if (flag && pActor.isSexMale())
        pMaxExclusive += 999;
      if (num2 != 0 && pActor.isSexFemale())
        pMaxExclusive += 999;
    }
    for (int index = 0; index < pAmount; ++index)
      num1 += Randy.randomInt(0, pMaxExclusive);
    return num1;
  }

  public static void checkHomeDocks(Actor pActor)
  {
    Building homeBuilding = pActor.getHomeBuilding();
    if (homeBuilding == null)
    {
      ListPool<Docks> docks1 = pActor.current_tile.region.island.docks;
      if (docks1 != null && docks1.Count > 0)
      {
        for (int index = 0; index < docks1.Count; ++index)
        {
          Docks docks2 = docks1[index];
          if (docks2.building.isUsable() && !docks2.building.isAbandoned() && !docks2.building.isUnderConstruction() && docks2.building.hasCity() && docks2.building.city.kingdom == pActor.kingdom && !docks2.building.city.kingdom.isEnemy(pActor.kingdom) && !docks2.isFull(pActor.asset.boat_type))
          {
            docks2.addBoatToDock(pActor);
            return;
          }
        }
      }
    }
    if (homeBuilding == null)
      return;
    if (!homeBuilding.isSameIslandAs((BaseSimObject) pActor))
    {
      pActor.clearHomeBuilding();
    }
    else
    {
      Docks componentDocks = homeBuilding.component_docks;
      if (componentDocks.isDockGood() || componentDocks.hasOceanTiles())
        return;
      pActor.clearHomeBuilding();
    }
  }

  public static void copyImportantData(ActorData pFrom, ActorData pCloneTo, bool pCopyAge)
  {
    pCloneTo.name = pFrom.name;
    pCloneTo.custom_name = pFrom.custom_name;
    if (pCopyAge)
    {
      pCloneTo.created_time = pFrom.created_time;
      pCloneTo.age_overgrowth = pFrom.age_overgrowth;
    }
    pCloneTo.asset_id = pFrom.asset_id;
    pCloneTo.kills = pFrom.kills;
    pCloneTo.births = pFrom.births;
    pCloneTo.favorite = pFrom.favorite;
    pCloneTo.food_consumed = pFrom.food_consumed;
    pCloneTo.favorite_food = pFrom.favorite_food;
    pCloneTo.head = pFrom.head;
    pCloneTo.generation = pFrom.generation;
    pCloneTo.parent_id_1 = pFrom.parent_id_1;
    pCloneTo.parent_id_2 = pFrom.parent_id_2;
    pCloneTo.ancestor_family = pFrom.ancestor_family;
    pCloneTo.best_friend_id = pFrom.best_friend_id;
    pCloneTo.lover = pFrom.lover;
    pCloneTo.experience = pFrom.experience;
    pCloneTo.renown = pFrom.renown;
    pCloneTo.loot = pFrom.loot;
    pCloneTo.money = pFrom.money;
    pCloneTo.level = pFrom.level;
    pCloneTo.sex = pFrom.sex;
    pCloneTo.phenotype_shade = pFrom.phenotype_shade;
    pCloneTo.phenotype_index = pFrom.phenotype_index;
    pCloneTo["diplomacy"] = pFrom["diplomacy"];
    pCloneTo["intelligence"] = pFrom["intelligence"];
    pCloneTo["stewardship"] = pFrom["stewardship"];
    pCloneTo["warfare"] = pFrom["warfare"];
  }

  public static void copyUnitToOtherUnit(Actor pParent, Actor pCloneTarget, bool pCopyAge = true)
  {
    pCloneTarget.current_position = pParent.current_position;
    pCloneTarget.current_rotation = pParent.current_rotation;
    ActorTool.copyImportantData(pParent.data, pCloneTarget.data, pCopyAge);
    pCloneTarget.takeItems(pParent);
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pParent.getTraits())
      pCloneTarget.addTrait(trait);
    pCloneTarget.setStatsDirty();
    if (!MoveCamera.inSpectatorMode() || !pParent.isCameraFollowingUnit())
      return;
    MoveCamera.setFocusUnit(pCloneTarget);
  }

  public static bool canBeCuredFromTraitsOrStatus(Actor pActor)
  {
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pActor.getTraits())
    {
      if (trait.can_be_cured)
        return true;
    }
    if (pActor.hasAnyStatusEffect())
    {
      foreach (Status statuse in pActor.getStatuses())
      {
        if (statuse.asset.can_be_cured)
          return true;
      }
    }
    return false;
  }

  public static void applyForceToUnit(
    AttackData pData,
    BaseSimObject pTargetToCheck,
    float pMod = 1f,
    bool pCheckCancelJobOnLand = false)
  {
    float pForceAmountDirection = pData.knockback * pMod;
    if ((double) pForceAmountDirection <= 0.0 || !pTargetToCheck.isActor())
      return;
    Vector2 vector2_1 = Vector2.op_Implicit(pTargetToCheck.cur_transform_position);
    Vector2 vector2_2 = Vector2.op_Implicit(pData.hit_position);
    pTargetToCheck.a.calculateForce(vector2_1.x, vector2_1.y, vector2_2.x, vector2_2.y, pForceAmountDirection, pCheckCancelJobOnLand: pCheckCancelJobOnLand);
  }

  public static int countUnitsFrom(string pActorID)
  {
    return AssetManager.actor_library.get(pActorID).units.Count;
  }

  public static void checkFallInLove(Actor pActor, Actor pTarget)
  {
    if (!pActor.canFallInLoveWith(pTarget) || !pTarget.canFallInLoveWith(pActor))
      return;
    pActor.becomeLoversWith(pTarget);
  }

  public static void checkBecomingBestFriends(Actor pActor, Actor pTarget)
  {
    if (pActor.hasBestFriend() || pTarget.hasBestFriend() || pActor.isBaby() && pTarget.isAdult() || pActor.isAdult() && pTarget.isBaby())
      return;
    float num1 = 0.0f;
    if (pActor.hasEmotions() && pTarget.hasEmotions())
    {
      float num2 = 1f - Mathf.Abs(pActor.getHappinessRatio() - pTarget.getHappinessRatio());
      num1 += num2 * 0.25f;
    }
    if (pActor.family == pTarget.family)
      num1 += 0.1f;
    float pVal = num1 + ActorTool.calcLikeability(pActor, pTarget);
    if ((double) pVal <= 0.0 || !Randy.randomChance(pVal))
      return;
    pActor.setBestFriend(pTarget, true);
    pTarget.setBestFriend(pActor, true);
  }

  private static float calcLikeability(Actor pActor, Actor pTarget)
  {
    float num1 = 0.0f;
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pActor.getTraits())
    {
      float num2 = 1f;
      if (trait.same_trait_mod != 0 && pTarget.hasTrait(trait))
        num2 += (float) trait.same_trait_mod / 100f;
      if (trait.opposite_trait_mod != 0)
      {
        foreach (ActorTrait oppositeTrait in trait.opposite_traits)
        {
          if (pTarget.hasTrait(oppositeTrait))
            num2 += (float) trait.opposite_trait_mod / 100f;
        }
      }
      if ((double) trait.likeability == 0.0)
        num1 += num2 * 0.1f;
      else
        num1 += trait.likeability * num2;
    }
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pTarget.getTraits())
    {
      if (trait.same_trait_mod == 0 && trait.opposite_trait_mod == 0)
        num1 += trait.likeability;
    }
    float num3 = num1 * 0.5f;
    if (pActor.areFoes((BaseSimObject) pTarget))
      num3 -= 0.5f;
    float num4 = pActor.religion != pTarget.religion ? num3 - 0.25f : num3 + 0.1f;
    if (pActor.clan == pTarget.clan)
      num4 += 0.1f;
    if (pActor.culture == pTarget.culture)
      num4 += 0.1f;
    return num4;
  }
}
