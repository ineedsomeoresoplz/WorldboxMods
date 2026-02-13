// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindDesireWaypoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehFindDesireWaypoint : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    string buildingAttractorId = pActor.kingdom.asset.building_attractor_id;
    if (string.IsNullOrEmpty(buildingAttractorId))
      return BehResult.Stop;
    BuildingAsset buildingAsset = AssetManager.buildings.get(buildingAttractorId);
    if (buildingAsset == null)
      return BehResult.Stop;
    HashSet<Building> buildings = buildingAsset.buildings;
    if (buildings.Count == 0)
      return BehResult.Stop;
    Building closestBuildingFrom = Finder.getClosestBuildingFrom(pActor, (IReadOnlyCollection<Building>) buildings);
    if (closestBuildingFrom == null || (double) Toolbox.DistTile(pActor.current_tile, closestBuildingFrom.current_tile) < 10.0)
      return BehResult.Stop;
    pActor.beh_building_target = closestBuildingFrom;
    return BehResult.Continue;
  }
}
