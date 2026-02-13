// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehStayInBuildingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehStayInBuildingTarget : BehCityActor
{
  private float min;
  private float max;

  public BehStayInBuildingTarget(float pMin = 0.0f, float pMax = 1f)
  {
    this.min = pMin;
    this.max = pMax;
  }

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.timer_action = Randy.randomFloat(this.min, this.max);
    pActor.stayInBuilding(pActor.beh_building_target);
    pActor.beh_tile_target = (WorldTile) null;
    pActor.beh_building_target.startShake(0.5f);
    return BehResult.Continue;
  }
}
