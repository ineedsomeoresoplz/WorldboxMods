// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSkeletonFindTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehSkeletonFindTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    using (IEnumerator<Actor> enumerator = Finder.findSpeciesAroundTileChunk(pActor.current_tile, "necromancer").GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        Actor current = enumerator.Current;
        pActor.beh_tile_target = current.current_tile.region.tiles.GetRandom<WorldTile>();
        return BehResult.Continue;
      }
    }
    MapRegion mapRegion = pActor.current_tile.region;
    if (mapRegion.neighbours.Count > 0 && Randy.randomBool())
      mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
    if (mapRegion.tiles.Count <= 0)
      return BehResult.Stop;
    pActor.beh_tile_target = mapRegion.tiles.GetRandom<WorldTile>();
    return BehResult.Continue;
  }
}
