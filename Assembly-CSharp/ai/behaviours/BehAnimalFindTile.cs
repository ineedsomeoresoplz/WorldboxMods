// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehAnimalFindTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehAnimalFindTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (Randy.randomChance(0.8f))
    {
      using (IEnumerator<Actor> enumerator = Finder.findSpeciesAroundTileChunk(pActor.current_tile, "druid").GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          Actor current = enumerator.Current;
          pActor.beh_tile_target = current.current_tile.region.getRandomTile();
          return BehResult.Continue;
        }
      }
    }
    MapRegion mapRegion = pActor.current_tile.region;
    if (mapRegion.neighbours.Count > 0 && Randy.randomBool())
      mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
    if (mapRegion.tiles.Count <= 0)
      return BehResult.Stop;
    pActor.beh_tile_target = mapRegion.getRandomTile();
    return BehResult.Continue;
  }
}
