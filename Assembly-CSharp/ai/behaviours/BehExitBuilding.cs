// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehExitBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehExitBuilding : BehCityActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_building_target = true;
    this.check_building_target_non_usable = true;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.exitBuilding();
    pActor.beh_building_target.startShake(0.01f);
    return BehResult.Continue;
  }
}
