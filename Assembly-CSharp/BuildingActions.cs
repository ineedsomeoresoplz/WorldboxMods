// Decompiled with JetBrains decompiler
// Type: BuildingActions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class BuildingActions
{
  public static void tryGrowVegetationRandom(
    WorldTile pTile,
    VegetationType pType,
    bool pOnStart = false,
    bool pCheckLimit = true,
    bool pCheckRandom = true)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    if (biomeAsset == null || !biomeAsset.grow_vegetation_auto)
      return;
    BuildingAsset buildingAsset = (BuildingAsset) null;
    switch (pType)
    {
      case VegetationType.Trees:
        if (biomeAsset.grow_type_selector_trees != null)
        {
          buildingAsset = biomeAsset.grow_type_selector_trees(pTile);
          break;
        }
        break;
      case VegetationType.Plants:
        if (biomeAsset.grow_type_selector_plants != null)
        {
          buildingAsset = biomeAsset.grow_type_selector_plants(pTile);
          break;
        }
        break;
      case VegetationType.Bushes:
        if (biomeAsset.grow_type_selector_bushes != null)
        {
          buildingAsset = biomeAsset.grow_type_selector_bushes(pTile);
          break;
        }
        break;
    }
    if (buildingAsset == null)
      return;
    if (buildingAsset.limit_in_radius > 0)
      pCheckLimit = true;
    if (pCheckLimit && pTile.zone.hasReachedBuildingLimit(pTile, buildingAsset) || pCheckRandom && (double) buildingAsset.vegetation_random_chance < (double) Randy.random() || !World.world.buildings.canBuildFrom(pTile, buildingAsset, (City) null))
      return;
    World.world.buildings.addBuilding(buildingAsset, pTile);
    if (buildingAsset.flora_type == FloraType.Tree)
      ++World.world.game_stats.data.treesGrown;
    else if (buildingAsset.flora_type == FloraType.Plant || buildingAsset.flora_type == FloraType.Fungi)
      ++World.world.game_stats.data.floraGrown;
    if (!buildingAsset.has_sound_spawn)
      return;
    MusicBox.playSound(buildingAsset.sound_spawn, pTile, true, true);
  }

  public static void tryGrowMineralRandom(WorldTile pTile, bool pOnStart = false, bool pCheckLimit = true)
  {
    BiomeAsset biome = pTile.getBiome();
    if (biome == null || !biome.grow_minerals_auto || pTile.hasBuilding() && pTile.building.isUsable())
      return;
    BuildingAsset buildingAsset = biome.grow_type_selector_minerals(pTile);
    if (buildingAsset == null || pCheckLimit && pTile.zone.hasReachedBuildingLimit(pTile, buildingAsset) || !World.world.buildings.canBuildFrom(pTile, buildingAsset, (City) null))
      return;
    World.world.buildings.addBuilding(buildingAsset, pTile);
  }

  public static Building tryGrowVegetation(
    WorldTile pTile,
    string pTemplateID,
    bool pSfx = false,
    bool pCheckLimit = true)
  {
    BuildingAsset buildingAsset = AssetManager.buildings.get(pTemplateID);
    if (pTile.hasBuilding() && pTile.building.isUsable())
      return (Building) null;
    if (buildingAsset == null)
      return (Building) null;
    if (pCheckLimit && pTile.zone.hasReachedBuildingLimit(pTile, buildingAsset))
      return (Building) null;
    if (!World.world.buildings.canBuildFrom(pTile, buildingAsset, (City) null))
      return (Building) null;
    Building building = World.world.buildings.addBuilding(buildingAsset, pTile, pSfx: pSfx);
    ++World.world.game_stats.data.floraGrown;
    return building;
  }

  public static void spawnBeehives(int pAmount)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      WorldTile random = World.world.tiles_list.GetRandom<WorldTile>();
      if (random.Type.grass)
        World.world.buildings.addBuilding("beehive", random, true);
    }
  }

  public static void spawnResource(int pAmount, string pType, bool pRandomSize = true)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      WorldTile random = World.world.tiles_list.GetRandom<WorldTile>();
      if (random.Type.ground)
        World.world.buildings.addBuilding(pType, random, true);
    }
  }
}
