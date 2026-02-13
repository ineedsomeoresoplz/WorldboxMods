// Decompiled with JetBrains decompiler
// Type: ActorMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using EpPathFinding.cs;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ActorMove
{
  public static ExecuteEvent goTo(
    Actor pActor,
    WorldTile pTileTarget,
    bool pPathOnLiquid = false,
    bool pWalkOnBlocks = false,
    bool pPathOnLava = false,
    int pLimitPathfindingRegions = 0)
  {
    pActor.clearOldPath();
    if (!DebugConfig.isOn(DebugOption.SystemUnitPathfinding))
    {
      pActor.current_path.Add(pTileTarget);
      return ExecuteEvent.True;
    }
    if (pActor.asset.is_boat && !pTileTarget.isGoodForBoat())
      return ExecuteEvent.False;
    if (pActor.isFlying())
    {
      pActor.current_path.Add(pTileTarget);
      return ExecuteEvent.True;
    }
    bool flag1 = pActor.current_tile.isSameIsland(pTileTarget);
    if (flag1 && !pActor.isInLiquid())
      pPathOnLiquid = false;
    AStarParam pathfindingParam = World.world.pathfinding_param;
    bool fire = pActor.isImmuneToFire();
    pathfindingParam.resetParam();
    pathfindingParam.block = pWalkOnBlocks;
    pathfindingParam.lava = fire;
    pathfindingParam.fire = fire;
    if (pActor.hasStatus("burning") || pActor.current_tile.isOnFire())
      pathfindingParam.fire = true;
    if (pActor.current_tile.Type.lava)
      pathfindingParam.lava = true;
    if (pPathOnLava)
      pathfindingParam.lava = true;
    pathfindingParam.ocean = pActor.isWaterCreature();
    if (pPathOnLiquid && !pActor.isDamagedByOcean())
      pathfindingParam.ocean = true;
    else if (pActor.isInLiquid())
      pathfindingParam.ocean = true;
    pathfindingParam.ground = !pActor.isWaterCreature() || pActor.asset.force_land_creature;
    if (pActor.isWaterCreature() && !pActor.isInWater())
      pathfindingParam.ground = true;
    pathfindingParam.boat = pActor.asset.is_boat && pActor.current_tile.isGoodForBoat();
    pathfindingParam.limit = true;
    if (PathfinderTools.tryToGetSimplePath(pActor.current_tile, pTileTarget, pActor.current_path, pActor.asset, pathfindingParam))
    {
      int index = (int) ((double) pLimitPathfindingRegions * 8.0 * 0.5);
      List<WorldTile> lastRaycastResult = PathfinderTools.getLastRaycastResult();
      if (pLimitPathfindingRegions > 0 && lastRaycastResult.Count > index)
      {
        lastRaycastResult.RemoveRange(index, lastRaycastResult.Count - index);
        pActor.current_path.Add(lastRaycastResult.Last<WorldTile>());
      }
      else
        pActor.current_path.Add(pTileTarget);
    }
    World.world.path_finding_visualiser.showPath((StaticGrid) null, pActor.current_path);
    if (pActor.isUsingPath())
    {
      pActor.setTileTarget(pTileTarget);
      return ExecuteEvent.True;
    }
    if (!flag1)
    {
      if ((!pTileTarget.Type.ground || !pathfindingParam.ground) && (!pTileTarget.Type.ocean || !pathfindingParam.ocean) && (!pTileTarget.Type.lava || !pathfindingParam.lava) && (!pTileTarget.Type.block || !pathfindingParam.block))
        return ExecuteEvent.False;
      if (pTileTarget.region.island.getTileCount() < pActor.current_tile.region.island.getTileCount())
      {
        if (!pTileTarget.region.island.isConnectedWith(pActor.current_tile.region.island))
          return ExecuteEvent.False;
        pathfindingParam.end_to_start_path = true;
      }
      else if (!pActor.current_tile.region.island.isConnectedWith(pTileTarget.region.island))
        return ExecuteEvent.False;
    }
    bool flag2 = DebugConfig.isOn(DebugOption.UseGlobalPathLock);
    if (flag2)
    {
      if (pActor.asset.is_boat)
        flag2 = true;
      else if (!flag1)
        flag2 = false;
    }
    if (flag2)
    {
      switch (World.world.region_path_finder.getGlobalPath(pActor.current_tile, pTileTarget, pActor.asset.is_boat))
      {
        case PathFinderResult.NotFound:
          return ExecuteEvent.True;
        case PathFinderResult.SamePlace:
          pActor.current_path.Add(pTileTarget);
          return ExecuteEvent.True;
        case PathFinderResult.DifferentIslands:
          return ExecuteEvent.True;
        default:
          if (World.world.region_path_finder.last_globalPath != null)
          {
            pActor.current_path_global = World.world.region_path_finder.last_globalPath;
            ActorMove.lockRegionsForAStarSearch(pActor, pLimitPathfindingRegions);
            break;
          }
          pActor.current_tile.region.used_by_path_lock = true;
          pActor.current_tile.region.region_path_id = 0;
          break;
      }
    }
    pathfindingParam.use_global_path_lock = flag2;
    WorldTile pTargetTile = pTileTarget;
    if (flag2 && pActor.current_path_global != null && pActor.current_path_global.Count > 4)
    {
      int index = 4;
      pTargetTile = pActor.current_path_global[index].getRandomTile();
    }
    World.world.calcPath(pActor, pActor.current_tile, pTargetTile, pActor.current_path);
    if (AStarFinder.result_split_path)
      pActor.split_path = SplitPathStatus.Prepare;
    if (flag2)
      ActorMove.cleanRegionSearch(pActor);
    if (pLimitPathfindingRegions > 0 && pActor.current_path_global != null)
      pTileTarget = pActor.current_path_global.Last<MapRegion>().getRandomTile();
    pActor.setTileTarget(pTileTarget);
    return ExecuteEvent.True;
  }

  private static void lockRegionsForAStarSearch(Actor pActor, int pLimitPathfindingRegions)
  {
    int num = 0;
    List<MapRegion> currentPathGlobal = pActor.current_path_global;
    int pathfindingRegions = ActorMove.getLimitedPathfindingRegions(pActor, pLimitPathfindingRegions);
    for (int index = 0; index < pathfindingRegions; ++index)
    {
      MapRegion mapRegion = currentPathGlobal[index];
      mapRegion.used_by_path_lock = true;
      mapRegion.region_path_id = num++;
    }
    if (pathfindingRegions >= currentPathGlobal.Count)
      return;
    currentPathGlobal.RemoveRange(pathfindingRegions, currentPathGlobal.Count - pathfindingRegions);
  }

  private static int getLimitedPathfindingRegions(Actor pActor, int pLimitPathfindingRegions)
  {
    return pLimitPathfindingRegions <= 0 ? pActor.current_path_global.Count : Mathf.Min(pActor.current_path_global.Count, pLimitPathfindingRegions);
  }

  private static void cleanRegionSearch(Actor pActor)
  {
    if (pActor.current_path_global != null)
    {
      List<MapRegion> currentPathGlobal = pActor.current_path_global;
      for (int index = 0; index < currentPathGlobal.Count; ++index)
      {
        MapRegion mapRegion = currentPathGlobal[index];
        mapRegion.used_by_path_lock = false;
        mapRegion.region_path_id = -1;
      }
    }
    pActor.current_tile.region.used_by_path_lock = false;
    pActor.current_tile.region.region_path_id = -1;
  }

  public static ExecuteEvent goToCurved(Actor pActor, params WorldTile[] pTargets)
  {
    Vector3[] vector3Array = new Vector3[pTargets.Length];
    for (int index = 0; index < pTargets.Length; ++index)
      vector3Array[index] = pTargets[index].posV3;
    pActor.clearOldPath();
    float num1 = 0.0f;
    for (int index = 1; index < pTargets.Length; ++index)
    {
      WorldTile pTarget1 = pTargets[index - 1];
      WorldTile pTarget2 = pTargets[index];
      num1 += Toolbox.DistTile(pTarget1, pTarget2);
    }
    float num2 = Mathf.Clamp((float) (int) ((double) num1 / 4.0), 5f, 100f);
    for (int index = 0; (double) index < (double) num2; ++index)
    {
      Vector2 pPos = Toolbox.cubeBezierN(Toolbox.easeInOutQuart((float) (index + 1) / num2), vector3Array);
      Toolbox.clampToMap(ref pPos);
      WorldTile tileSimple = World.world.GetTileSimple((int) pPos.x, (int) pPos.y);
      if (pActor.current_path.Count <= 0 || pActor.current_path.Last<WorldTile>() != tileSimple)
        pActor.current_path.Add(tileSimple);
    }
    WorldTile pTile = pTargets.Last<WorldTile>();
    pActor.current_path.Add(pTile);
    World.world.path_finding_visualiser.showPath((StaticGrid) null, pActor.current_path);
    pActor.setTileTarget(pTile);
    return ExecuteEvent.True;
  }
}
