// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehGetResourcesFromMine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehGetResourcesFromMine : BehActorUsableBuildingTarget
{
  private const string ASSET_FOR_NO_BIOME = "mineral_stone";
  private static List<string> pool_mineral_assets_default = new List<string>();

  public override void create()
  {
    base.create();
    if (BehGetResourcesFromMine.pool_mineral_assets_default.Count != 0)
      return;
    BehGetResourcesFromMine.initPool();
  }

  private static void initPool()
  {
    BehGetResourcesFromMine.addToPool("mineral_gems", 1, BehGetResourcesFromMine.pool_mineral_assets_default);
    BehGetResourcesFromMine.addToPool("mineral_gems", 4, BehGetResourcesFromMine.pool_mineral_assets_default);
    BehGetResourcesFromMine.addToPool("mineral_stone", 20, BehGetResourcesFromMine.pool_mineral_assets_default);
    BehGetResourcesFromMine.addToPool("mineral_metals", 10, BehGetResourcesFromMine.pool_mineral_assets_default);
  }

  private static void addToPool(string pID, int pAmount, List<string> pPool)
  {
    for (int index = 0; index < pAmount; ++index)
      pPool.Add(pID);
  }

  public override BehResult execute(Actor pActor)
  {
    if (Randy.randomChance(0.4f))
      return BehResult.Continue;
    BuildingAsset randomAssetFromPool = BehGetResourcesFromMine.getRandomAssetFromPool(pActor);
    if (randomAssetFromPool == null)
      return BehResult.Continue;
    BuildingHelper.tryToBuildNear(pActor.beh_building_target.current_tile, randomAssetFromPool);
    pActor.addLoot(SimGlobals.m.coins_for_mine);
    return BehResult.Continue;
  }

  private static BuildingAsset getRandomAssetFromPool(Actor pActor)
  {
    Building behBuildingTarget = pActor.beh_building_target;
    WorldTile worldTile1 = behBuildingTarget.current_tile;
    if (!worldTile1.Type.is_biome)
    {
      bool flag = false;
      foreach (WorldTile tile in behBuildingTarget.tiles)
      {
        if (tile.Type.is_biome)
        {
          flag = true;
          worldTile1 = tile;
          break;
        }
      }
      if (!flag)
      {
        for (int index = 0; index < worldTile1.neighboursAll.Length; ++index)
        {
          WorldTile worldTile2 = worldTile1.neighboursAll[index];
          if (worldTile2.Type.is_biome)
          {
            flag = true;
            worldTile1 = worldTile2;
            break;
          }
        }
      }
      if (!flag)
        return AssetManager.buildings.get("mineral_stone");
    }
    string random = (worldTile1.Type.biome_asset?.pot_minerals_spawn ?? BehGetResourcesFromMine.pool_mineral_assets_default).GetRandom<string>();
    return AssetManager.buildings.get(random);
  }
}
