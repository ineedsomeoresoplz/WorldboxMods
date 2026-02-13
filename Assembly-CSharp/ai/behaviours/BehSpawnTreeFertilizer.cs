// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSpawnTreeFertilizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehSpawnTreeFertilizer : BehaviourActionActor
{
  private static List<WorldTile> _tiles = new List<WorldTile>();

  public override BehResult execute(Actor pActor)
  {
    if (!Randy.randomChance(0.3f) || !pActor.current_tile.Type.ground)
      return BehResult.Stop;
    BiomeAsset biomeAsset1 = pActor.current_tile.Type.biome_asset;
    if (biomeAsset1 == null || biomeAsset1.grow_vegetation_auto)
      return BehResult.Stop;
    SpellAsset spellAsset = AssetManager.spells.get("spawn_vegetation");
    BehSpawnTreeFertilizer._tiles.Clear();
    foreach (WorldTile tile in pActor.current_tile.region.tiles)
    {
      if (!(tile.Type.biome_id == "biome_grass"))
      {
        BiomeAsset biomeAsset2 = tile.Type.biome_asset;
        if (biomeAsset2 != null && biomeAsset2.grow_vegetation_auto)
          BehSpawnTreeFertilizer._tiles.Add(tile);
      }
    }
    if (BehSpawnTreeFertilizer._tiles.Count == 0)
      return BehResult.Stop;
    AttackAction action = spellAsset.action;
    if (action != null)
    {
      int num = action((BaseSimObject) pActor, (BaseSimObject) pActor, BehSpawnTreeFertilizer._tiles.GetRandom<WorldTile>()) ? 1 : 0;
    }
    pActor.doCastAnimation();
    return BehResult.Continue;
  }
}
