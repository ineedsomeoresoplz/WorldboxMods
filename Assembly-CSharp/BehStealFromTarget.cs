// Decompiled with JetBrains decompiler
// Type: BehStealFromTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class BehStealFromTarget : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_actor_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Actor a = pActor.beh_actor_target.a;
    if (a == null || !a.isAlive() || a.isInsideSomething() || (double) pActor.distanceToActorTile(a) > 2.0)
      return BehResult.Stop;
    bool flag = false;
    float pWaitTimerForThief = 0.5f;
    float pTargetStunnedTimer = 1f;
    bool pAddAggro = false;
    if (a.canSeeTileBasedOnDirection(pActor.current_tile))
    {
      if (Randy.randomChance(0.4f))
      {
        flag = true;
        pTargetStunnedTimer = 1f;
        pWaitTimerForThief = 0.9f;
        pAddAggro = true;
      }
    }
    else if (Randy.randomChance(0.7f))
    {
      flag = true;
      pTargetStunnedTimer = 5f;
      pWaitTimerForThief = 1f;
    }
    else
      pActor.makeWait(1f);
    if (!flag)
      return BehResult.Stop;
    pActor.spawnSlashTalk(a.current_position);
    pActor.punchTargetAnimation(Vector2.op_Implicit(a.current_position), false, pAngle: -20f);
    pActor.stealActionFrom(a, pTargetStunnedTimer, pWaitTimerForThief, pAddAggro);
    return BehResult.Continue;
  }
}
