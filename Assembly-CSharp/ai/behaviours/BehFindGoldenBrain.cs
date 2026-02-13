// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindGoldenBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehFindGoldenBrain : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (WorldLawLibrary.world_law_peaceful_monsters.isEnabled())
      return BehResult.Stop;
    List<Building> buildings = BehaviourActionBase<Actor>.world.kingdoms_wild.get("golden_brain").buildings;
    if (buildings.Count == 0)
      return BehResult.Stop;
    Building closestBuildingFrom = Finder.getClosestBuildingFrom(pActor, (IReadOnlyCollection<Building>) buildings);
    if (closestBuildingFrom == null)
      return BehResult.Stop;
    pActor.beh_building_target = closestBuildingFrom;
    return BehResult.Continue;
  }
}
