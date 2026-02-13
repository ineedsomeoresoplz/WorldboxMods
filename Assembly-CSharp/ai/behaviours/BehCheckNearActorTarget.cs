// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckNearActorTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckNearActorTarget : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_actor_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Actor a = pActor.beh_actor_target.a;
    if (!pActor.canTalkWith(a))
      return BehResult.Stop;
    return (double) Toolbox.SquaredDistVec2Float(pActor.current_position, a.current_position) < 4.0 ? BehResult.Continue : BehResult.RestartTask;
  }
}
