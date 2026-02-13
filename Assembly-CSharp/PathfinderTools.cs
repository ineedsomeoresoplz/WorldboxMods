// Decompiled with JetBrains decompiler
// Type: PathfinderTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using EpPathFinding.cs;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PathfinderTools
{
  private static readonly List<WorldTile> _raycast_result = new List<WorldTile>();

  public static List<WorldTile> raycast(WorldTile pFrom, WorldTile pTarget, float pMod = 0.99f)
  {
    List<WorldTile> raycastResult = PathfinderTools._raycast_result;
    raycastResult.Clear();
    float num = Toolbox.DistTile(pFrom, pTarget) * pMod;
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(pFrom.posV3.x, pFrom.posV3.y);
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(pTarget.posV3.x, pTarget.posV3.y);
    WorldTile worldTile = (WorldTile) null;
    for (int index = 0; (double) index <= (double) num; ++index)
    {
      Vector2 vector2_3 = Vector2.Lerp(vector2_1, vector2_2, (float) index / num);
      WorldTile tile = World.world.GetTile(Mathf.FloorToInt(vector2_3.x), Mathf.FloorToInt(vector2_3.y));
      if (tile != null && tile != worldTile)
      {
        raycastResult.Add(tile);
        worldTile = tile;
      }
    }
    if (worldTile != pTarget)
      raycastResult.Add(pTarget);
    return raycastResult;
  }

  public static List<WorldTile> raycast(Vector2 pFrom, Vector2 pTarget, float pMod = 0.99f)
  {
    List<WorldTile> raycastResult = PathfinderTools._raycast_result;
    raycastResult.Clear();
    float num = Toolbox.DistVec2Float(pFrom, pTarget) * pMod;
    WorldTile worldTile = (WorldTile) null;
    for (int index = 0; (double) index <= (double) num; ++index)
    {
      Vector2 vector2 = Vector2.Lerp(pFrom, pTarget, (float) index / num);
      WorldTile tile = World.world.GetTile(Mathf.FloorToInt(vector2.x), Mathf.FloorToInt(vector2.y));
      if (tile != null && tile != worldTile)
      {
        raycastResult.Add(tile);
        worldTile = tile;
      }
    }
    if (Vector2.op_Inequality(Vector2Int.op_Implicit(worldTile.pos), pTarget))
    {
      WorldTile tile = World.world.GetTile((int) pTarget.x, (int) pTarget.y);
      if (tile != null)
        raycastResult.Add(tile);
    }
    return raycastResult;
  }

  public static bool tryToGetSimplePath(
    WorldTile pFrom,
    WorldTile pTargetTile,
    List<WorldTile> pPathToFill,
    ActorAsset pAsset,
    AStarParam pParam,
    int pTileLimit = 0)
  {
    PathfinderTools.raycast(pFrom, pTargetTile);
    List<WorldTile> raycastResult = PathfinderTools._raycast_result;
    int index = 0;
    for (int count = raycastResult.Count; index < count; ++index)
    {
      WorldTile worldTile = raycastResult[index];
      if (!pParam.block && worldTile.Type.block || !pParam.lava && worldTile.Type.lava || !pParam.fire && worldTile.isOnFire() || !pParam.ocean && worldTile.Type.ocean || !pParam.ground && worldTile.Type.ground || pParam.boat && !worldTile.isGoodForBoat() || worldTile.hasWallsAround())
        return false;
    }
    return true;
  }

  public static WorldTile raycastTileForUnitToEmbark(WorldTile pFromGround, WorldTile pTargetOcean)
  {
    List<WorldTile> worldTileList = PathfinderTools.raycast(pTargetOcean, pFromGround);
    WorldTile embark = (WorldTile) null;
    TileIsland island = pFromGround.region.island;
    for (int index = 0; index < worldTileList.Count; ++index)
    {
      WorldTile worldTile = worldTileList[index];
      if (worldTile.region.island == island)
      {
        embark = worldTile;
        break;
      }
    }
    worldTileList.Clear();
    return embark;
  }

  public static List<WorldTile> getLastRaycastResult() => PathfinderTools._raycast_result;

  public static WorldTile raycastTileForUnitLandingFromOcean(
    WorldTile pFromOcean,
    WorldTile pTargetGround)
  {
    List<WorldTile> worldTileList = PathfinderTools.raycast(pFromOcean, pTargetGround);
    WorldTile worldTile1 = (WorldTile) null;
    TileIsland island = pTargetGround.region.island;
    for (int index = 0; index < worldTileList.Count; ++index)
    {
      WorldTile worldTile2 = worldTileList[index];
      if (worldTile2.region.island == island)
      {
        worldTile1 = worldTile2;
        break;
      }
    }
    worldTileList.Clear();
    return worldTile1;
  }
}
