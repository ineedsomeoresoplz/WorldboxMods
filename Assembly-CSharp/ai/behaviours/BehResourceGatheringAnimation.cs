// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehResourceGatheringAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehResourceGatheringAnimation : BehaviourActionActor
{
  private float _timer_action;
  private string _sound_event_id;

  public BehResourceGatheringAnimation(float pTimerAction, string pSound = "", bool pLandIfHovering = true)
  {
    this._sound_event_id = pSound;
    this._timer_action = pTimerAction;
    this.land_if_hovering = pLandIfHovering;
  }

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_building_target = true;
    this.check_building_target_non_usable = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_building_target.asset.gatherable && !pActor.beh_building_target.hasResourcesToCollect())
      return BehResult.Stop;
    pActor.punchTargetAnimation(pActor.beh_building_target.current_tile.posV3);
    pActor.beh_building_target.resourceGathering(BehaviourActionBase<Actor>.world.elapsed);
    pActor.beh_building_target.startShake(0.01f);
    if (!string.IsNullOrEmpty(this._sound_event_id))
      MusicBox.playSound(this._sound_event_id, pActor.beh_building_target.current_tile, true, true);
    pActor.timer_action = this._timer_action;
    return BehResult.Continue;
  }
}
