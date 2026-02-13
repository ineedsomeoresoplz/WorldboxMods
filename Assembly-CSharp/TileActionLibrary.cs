// Decompiled with JetBrains decompiler
// Type: TileActionLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public static class TileActionLibrary
{
  public static bool damage(WorldTile pTile, Actor pActor)
  {
    throw new NotImplementedException("damage logic per tile not implemented yet");
  }

  public static bool landmine(WorldTile pTile, Actor pActor)
  {
    if (pActor.isFlying() || pActor.isInAir() || pActor.isHovering() || pActor.hasTrait("weightless"))
      return false;
    World.world.explosion_layer.explodeBomb(pTile);
    return true;
  }

  public static bool setUnitOnFire(WorldTile pTile, Actor pActor)
  {
    if (pActor.hasTrait("burning_feet"))
      return false;
    pActor.addStatusEffect("burning");
    pTile.startFire(true);
    return true;
  }

  public static bool giveTumorTrait(WorldTile pTile, Actor pActor)
  {
    if (pActor.asset.immune_to_tumor || !pActor.asset.can_turn_into_tumor)
      return false;
    pActor.addTrait("tumor_infection");
    return true;
  }

  public static bool giveSlownessStatus(WorldTile pTile, Actor pActor)
  {
    if (pActor.asset.immune_to_slowness)
      return false;
    pActor.addStatusEffect("slowness");
    return true;
  }

  public static bool giveMadnessTrait(WorldTile pTile, Actor pActor)
  {
    if (pActor.asset.immune_to_tumor || !pActor.asset.can_turn_into_tumor)
      return false;
    pActor.addTrait("madness");
    return true;
  }

  public static BuildingAsset getGrowTypeRandomMineral(WorldTile pTile)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    if (biomeAsset == null)
      return (BuildingAsset) null;
    if (biomeAsset.pot_minerals_spawn == null)
      return (BuildingAsset) null;
    string random = biomeAsset.pot_minerals_spawn.GetRandom<string>();
    return AssetManager.buildings.get(random);
  }

  public static BuildingAsset getGrowTypeRandomTrees(WorldTile pTile)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    return biomeAsset == null ? (BuildingAsset) null : TileActionLibrary.getRandomAssetFromPot(biomeAsset.pot_trees_spawn);
  }

  public static BuildingAsset getGrowTypeRandomPlants(WorldTile pTile)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    return biomeAsset == null ? (BuildingAsset) null : TileActionLibrary.getRandomAssetFromPot(biomeAsset.pot_plants_spawn);
  }

  public static BuildingAsset getGrowTypeRandomBushes(WorldTile pTile)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    return biomeAsset == null ? (BuildingAsset) null : TileActionLibrary.getRandomAssetFromPot(biomeAsset.pot_bushes_spawn);
  }

  private static BuildingAsset getRandomAssetFromPot(List<string> pPotList)
  {
    if (pPotList == null)
      return (BuildingAsset) null;
    string random = pPotList.GetRandom<string>();
    return AssetManager.buildings.get(random);
  }

  public static BuildingAsset getGrowTypeSand(WorldTile pTile)
  {
    bool flag = pTile.zone.hasLiquid();
    if (!flag)
    {
      foreach (TileZone tileZone in pTile.zone.neighbours_all)
      {
        flag = tileZone.hasLiquid();
        if (flag)
          break;
      }
    }
    return flag ? AssetManager.buildings.get("palm_tree") : AssetManager.buildings.get("cacti_tree");
  }
}
