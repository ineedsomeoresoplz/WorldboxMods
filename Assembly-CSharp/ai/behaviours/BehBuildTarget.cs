// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBuildTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBuildTarget : BehActorUsableBuildingTarget
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
    this.null_check_city = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.beh_building_target.isUnderConstruction())
      return BehResult.Stop;
    if (pActor.beh_building_target.updateBuild(pActor.getConstructionSpeed()))
      pActor.addLoot(SimGlobals.m.coins_for_building);
    return BehResult.Continue;
  }
}
