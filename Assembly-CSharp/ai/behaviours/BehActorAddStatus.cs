// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehActorAddStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehActorAddStatus : BehaviourActionActor
{
  private string _status_id;
  private float _override_timer;
  private bool _effect_on;
  private bool _add_action_timer;

  public BehActorAddStatus(
    string pStatusID,
    float pOverrideTimer = -1f,
    bool pEffectOn = true,
    bool pAddActionTimer = false)
  {
    this._status_id = pStatusID;
    this._override_timer = pOverrideTimer;
    this._effect_on = pEffectOn;
    this._add_action_timer = pAddActionTimer;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.addStatusEffect(this._status_id, this._override_timer, this._effect_on);
    if (this._add_action_timer)
      pActor.makeWait(this._override_timer);
    return BehResult.Continue;
  }
}
