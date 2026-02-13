// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehShakeBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehShakeBuilding : BehaviourActionActor
{
  private float _shake_time;
  private float _shake_intensity_x;
  private float _shake_intensity_y;

  public BehShakeBuilding(float pShakeParam = 0.7f, float pIntensityX = 0.04f, float pIntensityY = 0.04f)
  {
    this._shake_time = pShakeParam;
    this._shake_intensity_x = pIntensityX;
    this._shake_intensity_y = pIntensityY;
  }

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.beh_building_target.startShake(this._shake_time, this._shake_intensity_x, this._shake_intensity_y);
    return BehResult.Continue;
  }
}
