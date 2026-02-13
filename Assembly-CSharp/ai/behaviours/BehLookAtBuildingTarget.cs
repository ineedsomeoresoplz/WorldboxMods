// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehLookAtBuildingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehLookAtBuildingTarget : BehActorBuildingTarget
{
  private float _timer;

  public BehLookAtBuildingTarget(float pTimer = 0.3f) => this._timer = pTimer;

  public override BehResult execute(Actor pActor)
  {
    pActor.lookTowardsPosition(pActor.beh_building_target.current_position);
    pActor.timer_action = this._timer;
    return BehResult.Continue;
  }
}
