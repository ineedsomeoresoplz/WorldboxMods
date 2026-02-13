// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehAngleAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehAngleAnimation : BehaviourActionActor
{
  private AngleAnimationTarget _target;
  private float _timer_action;
  private float _angle;
  private string _sound_event_id;
  private bool _check_flip;

  public BehAngleAnimation(
    AngleAnimationTarget pTarget,
    string pSound = null,
    float pTimerAction = 0.0f,
    float pAngle = 40f,
    bool pCheckFlip = true,
    bool pLandIfHovering = false)
  {
    this._sound_event_id = pSound;
    this._angle = pAngle;
    this._target = pTarget;
    this._timer_action = pTimerAction;
    this._check_flip = pCheckFlip;
    this.land_if_hovering = pLandIfHovering;
  }

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    switch (this._target)
    {
      case AngleAnimationTarget.Tile:
        this.null_check_tile_target = true;
        break;
      case AngleAnimationTarget.Building:
        this.null_check_building_target = true;
        this.check_building_target_non_usable = true;
        break;
      case AngleAnimationTarget.Actor:
        this.null_check_actor_target = true;
        break;
      case AngleAnimationTarget.Ruin:
        this.null_check_building_target = true;
        break;
    }
  }

  public override BehResult execute(Actor pActor)
  {
    WorldTile pTile = pActor.current_tile;
    switch (this._target)
    {
      case AngleAnimationTarget.Tile:
        pTile = pActor.beh_tile_target;
        break;
      case AngleAnimationTarget.Building:
        pTile = pActor.beh_building_target.current_tile;
        pActor.beh_building_target.startShake(0.3f);
        break;
      case AngleAnimationTarget.Actor:
        if (pActor.beh_actor_target.a.isInsideSomething())
          return BehResult.Stop;
        pTile = pActor.beh_actor_target.current_tile;
        break;
    }
    pActor.punchTargetAnimation(pTile.posV3, this._check_flip, pAngle: this._angle);
    if (!string.IsNullOrEmpty(this._sound_event_id))
      MusicBox.playSound(this._sound_event_id, pTile, true);
    pActor.timer_action = this._timer_action;
    return BehResult.Continue;
  }
}
