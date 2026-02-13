// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehGoToStablePlace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehGoToStablePlace : BehaviourActionActor
{
  private static MapRegion best_region;
  private static int best_fast_dist = int.MaxValue;
  internal static List<KeyValuePair<int, MapRegion>> bestRegions = new List<KeyValuePair<int, MapRegion>>(4);
  internal static WorldTile best_tile = (WorldTile) null;
  private const int MAX_DISTANCE = 15;

  public override BehResult execute(Actor pActor)
  {
    TileIsland island1 = pActor.current_tile.region.island;
    if (island1.isGoodIslandForActor(pActor))
      return BehResult.Stop;
    BehGoToStablePlace.best_region = (MapRegion) null;
    BehGoToStablePlace.best_fast_dist = int.MaxValue;
    BehGoToStablePlace.best_tile = (WorldTile) null;
    BehGoToStablePlace.best_region = BehGoToStablePlace.findIslandNearby(pActor);
    if (BehGoToStablePlace.best_region != null)
    {
      pActor.beh_tile_target = BehGoToStablePlace.best_region.tiles.GetRandom<WorldTile>();
      BehGoToStablePlace.best_tile = pActor.beh_tile_target;
      return BehResult.Continue;
    }
    BehGoToStablePlace.bestRegions.Clear();
    Vector2Int pos1 = pActor.current_tile.pos;
    KeyValuePair<int, MapRegion> keyValuePair;
    for (int index = 0; index < BehaviourActionBase<Actor>.world.islands_calculator.islands.Count; ++index)
    {
      TileIsland island2 = BehaviourActionBase<Actor>.world.islands_calculator.islands[index];
      if (BehGoToStablePlace.checkIsland(island1, island2, pActor))
      {
        HashSet<MapRegion> pOut1;
        HashSet<MapRegion> pOut2;
        BehGoToStablePlace.selectBorderRegionsForComparison(island2, island1, out pOut1, out pOut2);
        MapRegion mapRegion1 = (MapRegion) null;
        int num1 = int.MaxValue;
        foreach (MapRegion mapRegion2 in pOut1)
        {
          if (pOut2.Contains(mapRegion2))
          {
            int x1 = ((Vector2Int) ref pos1).x;
            int y1 = ((Vector2Int) ref pos1).y;
            Vector2Int pos2 = mapRegion2.tiles[0].pos;
            int x2 = ((Vector2Int) ref pos2).x;
            pos2 = mapRegion2.tiles[0].pos;
            int y2 = ((Vector2Int) ref pos2).y;
            int num2 = Toolbox.SquaredDist(x1, y1, x2, y2);
            if (num2 < num1)
            {
              num1 = num2;
              mapRegion1 = mapRegion2;
            }
          }
        }
        if (mapRegion1 != null)
        {
          MapRegion mapRegion3 = mapRegion1;
          List<WorldTile> edgeTiles = mapRegion3.getEdgeTiles();
          if (edgeTiles.Count != 0)
          {
            float key = Toolbox.DistTile(pActor.current_tile, edgeTiles.GetRandom<WorldTile>());
            if (BehGoToStablePlace.bestRegions.Count > 0)
            {
              keyValuePair = BehGoToStablePlace.bestRegions[0];
              if ((double) (keyValuePair.Key + 15) < (double) key)
                continue;
            }
            if (BehGoToStablePlace.bestRegions.Count < 4)
            {
              BehGoToStablePlace.bestRegions.Add(new KeyValuePair<int, MapRegion>((int) key, mapRegion3));
            }
            else
            {
              BehGoToStablePlace.bestRegions.Sort((Comparison<KeyValuePair<int, MapRegion>>) ((x, y) => x.Key.CompareTo(y.Key)));
              if ((double) BehGoToStablePlace.bestRegions[3].Key > (double) key)
                BehGoToStablePlace.bestRegions[3] = new KeyValuePair<int, MapRegion>((int) key, mapRegion3);
            }
          }
        }
      }
    }
    BehGoToStablePlace.bestRegions.RemoveAll((Predicate<KeyValuePair<int, MapRegion>>) (x => x.Key - 15 > BehGoToStablePlace.bestRegions[0].Key));
    if (Randy.randomChance(0.8f) && BehGoToStablePlace.bestRegions.Count > 0)
    {
      Actor actor = pActor;
      keyValuePair = BehGoToStablePlace.bestRegions.GetRandom<KeyValuePair<int, MapRegion>>();
      WorldTile random = keyValuePair.Value.tiles.GetRandom<WorldTile>();
      actor.beh_tile_target = random;
    }
    else
    {
      MapRegion mapRegion = !Randy.randomChance(0.5f) ? Randy.getRandom<MapRegion>(pActor.current_tile.region.neighbours) : pActor.current_tile.region;
      if (mapRegion != null)
        pActor.beh_tile_target = Randy.getRandom<WorldTile>(mapRegion.tiles);
    }
    if (!DebugConfig.isOn(DebugOption.ShowSwimToIslandLogic))
      BehGoToStablePlace.bestRegions.Clear();
    else
      BehGoToStablePlace.best_tile = pActor.beh_tile_target;
    return BehResult.Continue;
  }

  private static MapRegion findIslandNearby(Actor pActor)
  {
    (MapChunk[] mapChunkArray, int num1) = Toolbox.getAllChunksFromTile(pActor.current_tile);
    TileIsland island = pActor.current_tile.region.island;
    for (int index1 = 0; index1 < num1; ++index1)
    {
      MapChunk mapChunk = mapChunkArray[index1];
      for (int index2 = 0; index2 < mapChunk.regions.Count; ++index2)
      {
        MapRegion region = mapChunk.regions[index2];
        if (BehGoToStablePlace.checkIsland(island, region.island, pActor))
        {
          List<WorldTile> edgeTiles = region.getEdgeTiles();
          if (edgeTiles.Count != 0)
          {
            WorldTile closestTile = Toolbox.getClosestTile(edgeTiles, pActor.current_tile);
            int num2 = Toolbox.SquaredDistTile(pActor.current_tile, closestTile);
            if (num2 < BehGoToStablePlace.best_fast_dist)
            {
              BehGoToStablePlace.best_region = region;
              BehGoToStablePlace.best_fast_dist = num2;
            }
          }
        }
      }
    }
    return BehGoToStablePlace.best_region;
  }

  private static bool checkIsland(TileIsland pCurrentIsland, TileIsland pIsland, Actor pActor)
  {
    return pCurrentIsland != pIsland && pIsland.isGoodIslandForActor(pActor) && (pCurrentIsland.getTileCount() <= pIsland.getTileCount() ? pCurrentIsland.isConnectedWith(pIsland) : pIsland.isConnectedWith(pCurrentIsland)) && pIsland.insideRegionEdges.Count != 0;
  }

  private static void selectBorderRegionsForComparison(
    TileIsland pIsland1,
    TileIsland pIsland2,
    out HashSet<MapRegion> pOut1,
    out HashSet<MapRegion> pOut2)
  {
    if (pIsland1.outsideRegionEdges.Count + pIsland2.insideRegionEdges.Count < pIsland1.insideRegionEdges.Count + pIsland2.outsideRegionEdges.Count)
    {
      if (pIsland1.outsideRegionEdges.Count > pIsland2.insideRegionEdges.Count)
      {
        pOut1 = pIsland2.insideRegionEdges;
        pOut2 = pIsland1.outsideRegionEdges;
      }
      else
      {
        pOut2 = pIsland2.insideRegionEdges;
        pOut1 = pIsland1.outsideRegionEdges;
      }
    }
    else if (pIsland1.insideRegionEdges.Count > pIsland2.outsideRegionEdges.Count)
    {
      pOut1 = pIsland2.outsideRegionEdges;
      pOut2 = pIsland1.insideRegionEdges;
    }
    else
    {
      pOut2 = pIsland2.outsideRegionEdges;
      pOut1 = pIsland1.insideRegionEdges;
    }
  }
}
