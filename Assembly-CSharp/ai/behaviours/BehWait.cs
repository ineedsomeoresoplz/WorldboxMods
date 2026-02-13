// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehWait : BehaviourActionActor
{
  private float _wait_interval;

  public BehWait(float pWaitInterval = 1f) => this._wait_interval = pWaitInterval;

  public override BehResult execute(Actor pActor)
  {
    pActor.timer_action = this._wait_interval;
    return BehResult.Continue;
  }
}
