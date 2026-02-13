// Decompiled with JetBrains decompiler
// Type: BehFindRaycastTileForBuildingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class BehFindRaycastTileForBuildingTarget : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    Building behBuildingTarget = pActor.beh_building_target;
    if (behBuildingTarget == null)
      return BehResult.Stop;
    WorldTile currentTile1 = behBuildingTarget.current_tile;
    WorldTile currentTile2 = pActor.current_tile;
    if (!currentTile1.isSameIsland(currentTile2))
      return BehResult.Stop;
    List<WorldTile> worldTileList = PathfinderTools.raycast(currentTile2, currentTile1);
    WorldTile worldTile = (WorldTile) null;
    float resourceThrowDistance = pActor.getResourceThrowDistance();
    for (int index = 0; index < worldTileList.Count; ++index)
    {
      WorldTile pT1 = worldTileList[index];
      if (pT1.isSameIsland(currentTile2) && (double) Toolbox.DistTile(pT1, currentTile1) < (double) resourceThrowDistance)
      {
        worldTile = pT1;
        break;
      }
    }
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }
}
