// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehStartShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehStartShake : BehaviourActionActor
{
  private float _timer_shake;
  private float _wait_action;

  public BehStartShake(float pTimerShake = 1f, float pTimeWaitAction = 0.0f)
  {
    this._timer_shake = pTimerShake;
    this._wait_action = pTimeWaitAction;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.startShake(this._timer_shake);
    pActor.timer_action = this._wait_action;
    return BehResult.Continue;
  }
}
