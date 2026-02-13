// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.AStarFinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using C5;
using System;
using System.Collections.Generic;

#nullable disable
namespace EpPathFinding.cs;

public static class AStarFinder
{
  private static int _last_global_region_id;
  public static bool result_split_path;
  private static IntervalHeap<Node> _open_list = new IntervalHeap<Node>();

  public static void backTracePath(Node pNode, List<WorldTile> pSavePath, bool pEndToStartPath = false)
  {
    pSavePath.Clear();
    pSavePath.Add(pNode.tile);
    while ((object) pNode.parent != null)
    {
      pNode = pNode.parent;
      pSavePath.Add(pNode.tile);
    }
    if (pEndToStartPath)
      return;
    pSavePath.Reverse();
  }

  public static void FindPath(AStarParam pParam, List<WorldTile> pSavePath)
  {
    AStarFinder._open_list.Clear();
    Node node;
    Node pNode1;
    if (pParam.end_to_start_path)
    {
      node = pParam.EndNode;
      pNode1 = pParam.StartNode;
    }
    else
    {
      node = pParam.StartNode;
      pNode1 = pParam.EndNode;
    }
    StaticGrid searchGrid = (StaticGrid) pParam.SearchGrid;
    DiagonalMovement diagonalMovement = pParam.DiagonalMovement;
    float weight = pParam.weight;
    bool boat = pParam.boat;
    node.startToCurNodeLen = 0.0f;
    node.heuristicStartToEndLen = 0.0f;
    AStarFinder._open_list.Add(node);
    node.isOpened = true;
    AStarFinder.result_split_path = false;
    AStarFinder._last_global_region_id = 0;
    while (((CollectionValueBase<Node>) AStarFinder._open_list).Count != 0 && (pParam.max_open_list == -1 || ((CollectionValueBase<Node>) AStarFinder._open_list).Count <= pParam.max_open_list))
    {
      Node pNode2 = AStarFinder._open_list.DeleteMin();
      pNode2.isClosed = true;
      searchGrid.addToClosed(pNode2);
      if (pNode2 == pNode1)
      {
        AStarFinder.backTracePath(pNode1, pSavePath, pParam.end_to_start_path);
        break;
      }
      WorldTile tile = pNode2.tile;
      if (pParam.use_global_path_lock)
      {
        if (tile.region.region_path_id < AStarFinder._last_global_region_id && tile.region.region_path_id != -1)
        {
          pNode2.isClosed = true;
          searchGrid.addToClosed(pNode2);
          continue;
        }
        if (tile.region.region_path_id > AStarFinder._last_global_region_id)
          AStarFinder._last_global_region_id = tile.region.region_path_id;
      }
      foreach (WorldTile worldTile in diagonalMovement != DiagonalMovement.Never ? (!AStarFinder.isObstaclesAround(tile) ? tile.neighboursAll : tile.neighbours) : tile.neighbours)
      {
        Node pNode3 = searchGrid.m_nodes[worldTile.x][worldTile.y];
        if (!pNode3.isClosed)
        {
          TileTypeBase type = worldTile.Type;
          if (pParam.block || !type.block)
          {
            if (pParam.use_global_path_lock)
            {
              if (worldTile.region.region_path_id < AStarFinder._last_global_region_id && worldTile.region.region_path_id != -1)
              {
                pNode2.isClosed = true;
                searchGrid.addToClosed(pNode2);
                continue;
              }
              if (worldTile.region.region_path_id > AStarFinder._last_global_region_id)
                AStarFinder._last_global_region_id = worldTile.region.region_path_id;
              if (!worldTile.region.used_by_path_lock)
              {
                pNode2.isClosed = true;
                searchGrid.addToClosed(pNode2);
                continue;
              }
            }
            if (boat)
            {
              if (!worldTile.isGoodForBoat())
                continue;
            }
            else if (pParam.lava || !type.lava)
            {
              if ((!pParam.block || !type.block) && (!pParam.lava || !type.lava))
              {
                if (pParam.ground && pParam.ocean)
                {
                  if (!type.ground && !type.ocean)
                    continue;
                }
                else if (pParam.ground && !type.ground || pParam.ocean && !type.ocean)
                  continue;
              }
            }
            else
              continue;
            float num1 = 1f;
            if (pParam.roads && type.road)
              num1 = 0.01f;
            if (AStarFinder._last_global_region_id >= 4 && searchGrid.closed_list_count > 10)
            {
              AStarFinder.result_split_path = true;
              AStarFinder.backTracePath(pNode2, pSavePath, pParam.end_to_start_path);
              return;
            }
            float num2 = pNode2.startToCurNodeLen + num1 * (pNode3.x - pNode2.x == 0 || pNode3.y - pNode2.y == 0 ? 1f : 1.414f);
            if (!pNode3.isOpened || (double) num2 < (double) pNode3.startToCurNodeLen)
            {
              pNode3.startToCurNodeLen = num2;
              if (!pNode3.heuristicCurNodeToEndLen.HasValue)
                pNode3.heuristicCurNodeToEndLen = !pParam.roads ? new float?((float) (Math.Abs(pNode3.x - pNode1.x) + Math.Abs(pNode3.y - pNode1.y)) * weight) : new float?(Heuristic.Euclidean(Math.Abs(pNode3.x - pNode1.x), Math.Abs(pNode3.y - pNode1.y)) * weight);
              pNode3.heuristicStartToEndLen = pNode3.startToCurNodeLen + pNode3.heuristicCurNodeToEndLen.Value;
              pNode3.parent = pNode2;
              if (!pNode3.isOpened)
              {
                AStarFinder._open_list.Add(pNode3);
                pNode3.isOpened = true;
                searchGrid.addToClosed(pNode3);
              }
            }
          }
        }
      }
    }
  }

  private static bool isObstaclesAround(WorldTile pTile) => pTile.hasWallsAround();
}
