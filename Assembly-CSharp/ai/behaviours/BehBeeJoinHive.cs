// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBeeJoinHive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBeeJoinHive : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_building_target = true;
    this.check_building_target_non_usable = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Building behBuildingTarget = pActor.beh_building_target;
    pActor.setHomeBuilding(behBuildingTarget);
    return BehResult.Continue;
  }
}
