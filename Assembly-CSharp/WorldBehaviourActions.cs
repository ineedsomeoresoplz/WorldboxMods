// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class WorldBehaviourActions
{
  private static List<WorldTile> _temp_wheat_list = new List<WorldTile>();
  private static List<MapRegion> _spread_fauna_list = new List<MapRegion>();
  private static int _last_used_world_id = 0;
  private static readonly HashSet<IMetaObject> _used_for_grin_reaper = new HashSet<IMetaObject>();

  public static void buildingSparks()
  {
    if (World.world.buildings.sparkles.Count == 0 || !MapBox.isRenderGameplay())
      return;
    for (int index = 0; index < 1; ++index)
    {
      Building random1 = World.world.buildings.sparkles.GetRandom<Building>();
      if (random1.isUsable() && random1.animation_state == BuildingAnimationState.Normal)
      {
        WorldTile random2 = random1.tiles.GetRandom<WorldTile>();
        if (random2.has_tile_up)
          EffectsLibrary.spawnAtTile("fx_building_sparkle", random2.tile_up, 0.25f);
      }
    }
  }

  public static void updateDisasters()
  {
    DisasterAsset randomAssetFromPool = AssetManager.disasters.getRandomAssetFromPool();
    if (randomAssetFromPool == null || !Randy.randomChance(randomAssetFromPool.chance) || randomAssetFromPool == null)
      return;
    randomAssetFromPool.action(randomAssetFromPool);
  }

  public static void updateMigrants()
  {
    if (!WorldLawLibrary.world_law_civ_migrants.isEnabled() || World.world.cities.list.Count == 0)
      return;
    foreach (City pCity in World.world.cities.list.LoopRandom<City>(Randy.randomInt(1, World.world.cities.list.Count)))
    {
      if (!pCity.isNeutral() && pCity.getPopulationPeople() <= 100 && pCity.countFoodTotal() >= 10)
      {
        Building buildingOfType = pCity.getBuildingOfType("type_bonfire");
        if (buildingOfType != null)
        {
          Subspecies mainSubspecies = pCity.getMainSubspecies();
          if (mainSubspecies != null && !mainSubspecies.hasReachedPopulationLimit())
          {
            Kingdom kingdom = pCity.kingdom;
            ActorAsset actorAsset = mainSubspecies.getActorAsset();
            int num = Randy.randomInt(1, 4);
            for (int index = 0; index < num; ++index)
            {
              Actor actor = World.world.units.spawnNewUnit(actorAsset.id, buildingOfType.current_tile, pSpawnHeight: 0.0f, pSubspecies: mainSubspecies, pGiveOwnerlessItems: true);
              actor.data.age_overgrowth = Randy.randomInt(actorAsset.age_spawn, actorAsset.age_spawn * 2);
              actor.addTrait("attractive");
              actor.addTrait("soft_skin");
              actor.setMetasFromCity(pCity);
              kingdom.increaseMigrants();
              actor.setKingdom(kingdom);
              pCity.increaseMigrants();
              actor.setCity(pCity);
              actor.addStatusEffect("handsome_migrant");
            }
          }
        }
      }
    }
  }

  public static void updateRoadDecay()
  {
    TileZone random = World.world.zone_calculator.zones.GetRandom<TileZone>();
    if (random.hasCity() && random.city.hasCulture() && random.city.culture.canUseRoads() || Randy.randomBool())
      return;
    foreach (WorldTile pTile in random.tiles.LoopRandom<WorldTile>())
    {
      if (!Randy.randomBool() && pTile.Type.road)
        MapAction.decreaseTile(pTile, false);
    }
  }

  public static void updateFarmDecay()
  {
    if (MapBox.current_world_seed_id != WorldBehaviourActions._last_used_world_id)
    {
      WorldBehaviourActions._last_used_world_id = MapBox.current_world_seed_id;
      WorldBehaviourActions._temp_wheat_list.Clear();
    }
    if (WorldBehaviourActions._temp_wheat_list.Count == 0)
    {
      foreach (WorldTile worldTile in (HashSet<WorldTile>) TopTileLibrary.field.hashset)
        WorldBehaviourActions._temp_wheat_list.Add(worldTile);
      WorldBehaviourActions._temp_wheat_list.Shuffle<WorldTile>();
    }
    for (int index = 0; index < 10 && WorldBehaviourActions._temp_wheat_list.Count != 0; ++index)
    {
      WorldTile worldTile = WorldBehaviourActions._temp_wheat_list.Pop<WorldTile>();
      if (worldTile.Type.farm_field)
      {
        City city = worldTile.zone.city;
        if (city == null)
          MapAction.decreaseTile(worldTile, false);
        else if (!city.calculated_farm_fields.Contains(worldTile) && !city.calculated_place_for_farms.Contains(worldTile))
        {
          if (worldTile.hasBuilding() && worldTile.building.asset.wheat)
            worldTile.building.startDestroyBuilding();
          MapAction.decreaseTile(worldTile, false);
        }
      }
    }
  }

  public static void clearFarmDecay() => WorldBehaviourActions._temp_wheat_list.Clear();

  public static void updateRuinRatSpawn()
  {
    if (!WorldLawLibrary.world_law_animals_spawn.isEnabled() || World.world.map_chunk_manager.chunks.Length == 0)
      return;
    ActorAsset actorAsset = AssetManager.actor_library.get("rat");
    if (actorAsset == null || actorAsset.units.Count > actorAsset.max_random_amount * 3)
      return;
    Kingdom kingdom = World.world.kingdoms_wild.get("ruins");
    if (kingdom.buildings.Count == 0)
      return;
    Building building1 = (Building) null;
    foreach (Building building2 in kingdom.buildings.LoopRandom<Building>())
    {
      if (building2.isAlive() && building2.asset.city_building && building2.asset.hasHousingSlots())
      {
        building1 = building2;
        break;
      }
    }
    if (building1 == null)
      return;
    WorldTile currentTile = building1.current_tile;
    int num = Randy.randomInt(2, 5);
    for (int index = 0; index < num; ++index)
      World.world.units.spawnNewUnit(actorAsset.id, currentTile);
  }

  public static void updateUnitSpawn()
  {
    if (!WorldLawLibrary.world_law_animals_spawn.isEnabled() || World.world.map_chunk_manager.chunks.Length == 0)
      return;
    TileIsland tileIsland = (TileIsland) null;
    if (World.world.islands_calculator.islands_ground.Count > 0)
      tileIsland = World.world.islands_calculator.getRandomIslandGround();
    if (tileIsland == null)
      return;
    WorldTile randomTile = tileIsland.getRandomTile();
    BiomeAsset biomeAsset = randomTile.Type.biome_asset;
    if (biomeAsset == null || !biomeAsset.pot_spawn_units_auto)
      return;
    string random = biomeAsset.pot_units_spawn.GetRandom<string>();
    ActorAsset actorAsset = AssetManager.actor_library.get(random);
    if (actorAsset == null || actorAsset.units.Count > actorAsset.max_random_amount)
      return;
    int num1 = 0;
    foreach (Actor actor in Finder.getUnitsFromChunk(randomTile, 1))
    {
      if (num1++ > 3)
        return;
    }
    int num2 = Randy.randomInt(2, 5);
    for (int index = 0; index < num2; ++index)
      World.world.units.spawnNewUnit(actorAsset.id, randomTile, pGiveOwnerlessItems: true);
  }

  public static void growMinerals()
  {
    if (!WorldLawLibrary.world_law_grow_minerals.isEnabled())
      return;
    for (int index1 = 0; index1 < World.world.islands_calculator.islands_ground.Count; ++index1)
    {
      TileIsland tileIsland = World.world.islands_calculator.islands_ground[index1];
      if (tileIsland.getTileCount() > 20)
      {
        MapRegion random = tileIsland.regions.GetRandom();
        if (!WorldBehaviourActions.mineralsAroundZone(random.getRandomTile().zone))
        {
          int num = Mathf.Min(tileIsland.regions.Count / 20 + 1, 3);
          for (int index2 = 0; index2 < num; ++index2)
          {
            foreach (WorldTile pTile in random.tiles.LoopRandom<WorldTile>())
            {
              BiomeAsset biome = pTile.getBiome();
              if (biome != null && biome.grow_minerals_auto && !pTile.isOnFire() && !Randy.randomBool())
              {
                BuildingActions.tryGrowMineralRandom(pTile);
                break;
              }
            }
          }
        }
      }
    }
  }

  private static bool mineralsAroundZone(TileZone pZone)
  {
    if (pZone.hasAnyBuildingsInSet(BuildingList.Minerals))
      return true;
    foreach (TileZone tileZone in pZone.neighbours_all)
    {
      if (tileZone.hasAnyBuildingsInSet(BuildingList.Minerals))
        return true;
    }
    return false;
  }

  public static bool tryGrowVegetation(Building pBuilding, bool pCheckLimit = true)
  {
    if (!pBuilding.isUsable())
      return false;
    BuildingAsset asset = pBuilding.asset;
    WorldTile random = pBuilding.tiles.GetRandom<WorldTile>();
    int num = 6;
    for (int index = 0; index < num; ++index)
      random = random.neighboursAll.GetRandom<WorldTile>();
    if (WorldBehaviourActions.tryToGrowOnTile(random, asset, pCheckLimit))
      return true;
    WorldBehaviourActions._spread_fauna_list.Clear();
    WorldBehaviourActions._spread_fauna_list.AddRange((IEnumerable<MapRegion>) pBuilding.current_tile.region.neighbours);
    WorldBehaviourActions._spread_fauna_list.Add(pBuilding.current_tile.region);
    return WorldBehaviourActions.tryToGrowOnTile(WorldBehaviourActions._spread_fauna_list.GetRandom<MapRegion>().getRandomTile(), asset, pCheckLimit);
  }

  private static bool tryToGrowOnTile(WorldTile pTile, BuildingAsset pAsset, bool pCheckLimit = true)
  {
    if (pCheckLimit && pTile.zone.hasReachedBuildingLimit(pTile, pAsset) || !World.world.buildings.canBuildFrom(pTile, pAsset, (City) null))
      return false;
    World.world.buildings.addBuilding(pAsset, pTile);
    ++World.world.game_stats.data.treesGrown;
    return true;
  }

  public static void spawnRandomVegetation()
  {
    if (!WorldLawLibrary.world_law_vegetation_random_seeds.isEnabled() || !World.world_era.grow_vegetation)
      return;
    for (int index = 0; index < 5; ++index)
    {
      MapRegion random = World.world.map_chunk_manager.chunks.GetRandom<MapChunk>().regions.GetRandom<MapRegion>();
      if (random.island.type == TileLayerType.Ground)
        WorldBehaviourActions.growVegetationAt(random, 5);
    }
  }

  public static void growVegetationAt(MapRegion pRegion, int pAmount)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      WorldTile random = pRegion.tiles.GetRandom<WorldTile>();
      if (!random.isOnFire())
      {
        BiomeAsset biomeAsset = random.Type.biome_asset;
        if (biomeAsset != null && biomeAsset.grow_vegetation_auto && !Randy.randomBool())
          ActionLibrary.growRandomVegetation(random, biomeAsset);
      }
    }
  }

  public static bool addForGrinReaper(NanoObject pObject, BaseAugmentationAsset pAugmentationAsset)
  {
    return !pObject.isRekt() && WorldBehaviourActions._used_for_grin_reaper.Add((IMetaObject) pObject);
  }

  public static bool removeUsedForGrinReaper(
    NanoObject pObject,
    BaseAugmentationAsset pAugmentationAsset)
  {
    return pObject != null && WorldBehaviourActions._used_for_grin_reaper.Remove((IMetaObject) pObject);
  }

  public static void clearGrinReaper() => WorldBehaviourActions._used_for_grin_reaper.Clear();

  public static void updateGrinReaper()
  {
    if (WorldBehaviourActions._used_for_grin_reaper.Count == 0 || World.world.isWindowOnScreen())
      return;
    Actor randomActorForReaper = WorldBehaviourActions.getRandomActorForReaper();
    if (randomActorForReaper.isRekt() || randomActorForReaper.isInMagnet())
      return;
    randomActorForReaper.getHitFullHealth(AttackType.Smile);
    EffectsLibrary.spawnAt("fx_grin_reaper", randomActorForReaper.current_position, randomActorForReaper.actor_scale);
  }

  private static Actor getRandomActorForReaper()
  {
    WorldBehaviourActions._used_for_grin_reaper.RemoveWhere((Predicate<IMetaObject>) (pMeta => pMeta == null || !pMeta.isAlive()));
    IMetaObject random = WorldBehaviourActions._used_for_grin_reaper.GetRandom<IMetaObject>();
    if (random == null)
      return (Actor) null;
    return !random.isAlive() ? (Actor) null : random.getRandomActorForReaper();
  }
}
