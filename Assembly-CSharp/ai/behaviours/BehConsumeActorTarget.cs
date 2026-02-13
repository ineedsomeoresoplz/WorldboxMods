// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehConsumeActorTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehConsumeActorTarget : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_actor_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Actor a = pActor.beh_actor_target.a;
    if ((double) Toolbox.DistTile(pActor.current_tile, a.current_tile) > 1.0)
      return BehResult.StepBack;
    this.consume(pActor, a);
    return a.hasHealth() ? BehResult.RepeatStep : BehResult.Continue;
  }

  private void consume(Actor pMain, Actor pTarget)
  {
    pMain.timer_action = 0.3f;
    if ((double) pMain.current_position.x > (double) pTarget.current_position.x)
    {
      if ((double) pTarget.target_angle.z > -45.0)
      {
        pTarget.target_angle.z -= BehaviourActionBase<Actor>.world.elapsed * 100f;
        if ((double) pTarget.target_angle.z < -90.0)
          pTarget.target_angle.z = -90f;
        pTarget.rotation_cooldown = 1f;
      }
    }
    else if ((double) pTarget.target_angle.z < 45.0)
    {
      pTarget.target_angle.z += BehaviourActionBase<Actor>.world.elapsed * 100f;
      pTarget.rotation_cooldown = 1f;
    }
    if (pTarget.hasTrait("poisonous"))
      pMain.addStatusEffect("poisoned");
    if ((double) pMain.target_angle.z == 0.0)
    {
      pMain.punchTargetAnimation(Vector2.op_Implicit(pTarget.current_position), false);
      int num = (int) ((double) pTarget.getMaxHealth() * 0.15000000596046448) + 1;
      Clan clan = pTarget.clan;
      pTarget.getHit((float) num, pAttackType: AttackType.Eaten, pAttacker: (BaseSimObject) pMain, pSkipIfShake: false);
      pTarget.startShake(0.2f);
      if (pTarget.hasHealth())
      {
        pMain.addNutritionFromEating(num);
      }
      else
      {
        pMain.addNutritionFromEating(pSetMaxNutrition: true, pSetJustAte: true);
        pMain.countConsumed();
        AchievementLibrary.clannibals.checkBySignal((object) (pMain, clan));
      }
    }
    pTarget.cancelAllBeh();
  }
}
