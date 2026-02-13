// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehJumpingAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehJumpingAnimation : BehaviourActionActor
{
  private float _timer_action;
  private float _timer_jumping;

  public BehJumpingAnimation(float pTimerAction, float pTimerJumpAnimation)
  {
    this._timer_action = pTimerAction;
    this._timer_jumping = pTimerJumpAnimation;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.timer_jump_animation = this._timer_jumping;
    pActor.timer_action = this._timer_action;
    return BehResult.Continue;
  }
}
