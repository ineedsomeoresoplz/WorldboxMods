// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFightCheckEnemyIsOk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFightCheckEnemyIsOk : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.has_attack_target || !pActor.isEnemyTargetAlive())
      return BehResult.Stop;
    if (!pActor.shouldContinueToAttackTarget())
    {
      pActor.clearAttackTarget();
      return BehResult.Stop;
    }
    if (!pActor.canAttackTarget(pActor.attack_target))
    {
      pActor.ignoreTarget(pActor.attack_target);
      pActor.clearAttackTarget();
      return BehResult.Stop;
    }
    if (!pActor.isInAttackRange(pActor.attack_target))
    {
      if (pActor.isWaterCreature())
      {
        if (!pActor.attack_target.isInLiquid() && !pActor.asset.force_land_creature || pActor.attack_target.isFlying())
        {
          pActor.ignoreTarget(pActor.attack_target);
          pActor.clearAttackTarget();
          return BehResult.Stop;
        }
      }
      else if (pActor.attack_target.isInLiquid() && !pActor.isWaterCreature() || pActor.attack_target.isFlying())
      {
        pActor.ignoreTarget(pActor.attack_target);
        pActor.clearAttackTarget();
        return BehResult.Stop;
      }
    }
    int x1 = pActor.chunk.x;
    int y1 = pActor.chunk.y;
    int x2 = pActor.attack_target.chunk.x;
    int y2 = pActor.attack_target.chunk.y;
    float num = 1f;
    int y1_1 = y1;
    int x2_1 = x2;
    int y2_1 = y2;
    if ((double) Toolbox.Dist(x1, y1_1, x2_1, y2_1) >= (double) SimGlobals.m.unit_chunk_sight_range + (double) num)
    {
      pActor.clearAttackTarget();
      return BehResult.Stop;
    }
    pActor.beh_actor_target = pActor.attack_target;
    return BehResult.Continue;
  }
}
