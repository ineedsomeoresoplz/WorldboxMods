// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehMoveAwayFromBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehMoveAwayFromBlock : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.current_tile.region.island.type != TileLayerType.Block)
      return BehResult.Stop;
    int num1 = int.MaxValue;
    WorldTile worldTile = BehMoveAwayFromBlock.getBestTileToMove(pActor.current_tile.region, pActor);
    if (worldTile == null)
    {
      foreach (MapRegion neighbour in pActor.current_tile.region.neighbours)
      {
        WorldTile bestTileToMove = BehMoveAwayFromBlock.getBestTileToMove(neighbour, pActor);
        if (bestTileToMove != null)
        {
          int num2 = Toolbox.SquaredDistTile(pActor.current_tile, bestTileToMove);
          if (num2 < num1)
          {
            worldTile = bestTileToMove;
            num1 = num2;
          }
        }
      }
    }
    if (worldTile == null)
    {
      foreach (MapRegion insideRegionEdge in pActor.current_tile.region.island.insideRegionEdges)
      {
        WorldTile bestTileToMove = BehMoveAwayFromBlock.getBestTileToMove(insideRegionEdge, pActor);
        if (bestTileToMove != null)
        {
          int num3 = Toolbox.SquaredDistTile(pActor.current_tile, bestTileToMove);
          if (num3 < num1)
          {
            worldTile = bestTileToMove;
            num1 = num3;
          }
        }
      }
    }
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }

  private static WorldTile getBestTileToMove(MapRegion pRegion, Actor pActor)
  {
    List<WorldTile> edgeTiles = pRegion.getEdgeTiles();
    if (edgeTiles.Count == 0)
      return (WorldTile) null;
    WorldTile currentTile = pActor.current_tile;
    WorldTile bestTileToMove = (WorldTile) null;
    int num1 = int.MaxValue;
    TileIsland island = currentTile.region.island;
    foreach (WorldTile pT2 in edgeTiles.LoopRandom<WorldTile>())
    {
      int num2 = Toolbox.SquaredDistTile(currentTile, pT2);
      if (num2 < num1)
      {
        MapRegion region = pT2.region;
        if (BehMoveAwayFromBlock.isGoodTileRegion(region, pActor) && region.island != island && region.island.getTileCount() > 5)
        {
          bestTileToMove = pT2;
          num1 = num2;
        }
      }
    }
    return bestTileToMove;
  }

  private static bool isGoodTileRegion(MapRegion pRegion, Actor pActor)
  {
    return (pRegion.type != TileLayerType.Ocean || !pActor.isDamagedByOcean()) && (pRegion.type != TileLayerType.Lava || !pActor.asset.die_in_lava);
  }
}
