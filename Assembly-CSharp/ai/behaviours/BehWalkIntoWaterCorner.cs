// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehWalkIntoWaterCorner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehWalkIntoWaterCorner : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    TileIsland island = pActor.current_tile.region.island;
    if (island.isGoodIslandForActor(pActor))
      return BehResult.Stop;
    WorldTile worldTile = (WorldTile) null;
    int num1 = int.MaxValue;
    foreach (MapRegion insideRegionEdge in island.insideRegionEdges)
    {
      List<WorldTile> edgeTiles = insideRegionEdge.getEdgeTiles();
      for (int index = 0; index < edgeTiles.Count; ++index)
      {
        WorldTile pT2 = edgeTiles[index];
        if (pT2.Type.ocean)
        {
          int num2 = Toolbox.SquaredDistTile(pActor.current_tile, pT2);
          if (num2 < num1)
          {
            worldTile = pT2;
            num1 = num2;
          }
        }
      }
    }
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }
}
