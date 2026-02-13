// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehAttackActorHuntingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehAttackActorHuntingTarget : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    BaseSimObject behActorTarget = pActor.beh_actor_target;
    if (pActor.isInWaterAndCantAttack())
      return BehResult.Stop;
    if (behActorTarget == null || !behActorTarget.isAlive())
    {
      pActor.makeWait(0.5f);
      return BehResult.Continue;
    }
    if (!pActor.isInAttackRange(behActorTarget))
      return BehResult.StepBack;
    int num = pActor.tryToAttack(behActorTarget, pAttackPosition: new Vector3()) ? 1 : 0;
    if (num != 0 && pActor.hasRangeAttack())
      pActor.makeWait(0.5f);
    return num != 0 && !behActorTarget.isAlive() || !behActorTarget.isAlive() ? BehResult.Continue : BehResult.RepeatStep;
  }
}
