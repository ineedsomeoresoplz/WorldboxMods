// Decompiled with JetBrains decompiler
// Type: BehDoTalk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class BehDoTalk : BehaviourActionActor
{
  public BehDoTalk() => this.socialize = true;

  public override BehResult execute(Actor pActor)
  {
    Actor a = pActor.beh_actor_target?.a;
    if (a == null || !this.stillCanTalk(a) || (!pActor.hasTelepathicLink() || !a.hasTelepathicLink()) && (double) Toolbox.SquaredDistTile(a.current_tile, pActor.current_tile) > 16.0)
      return BehResult.Stop;
    int pResult;
    pActor.data.get("socialize", out pResult);
    int num = Randy.randomInt(5, 10);
    if (pResult > num)
      return BehResult.Continue;
    this.continueTalk(pActor, a);
    return BehResult.RepeatStep;
  }

  private bool stillCanTalk(Actor pTarget) => pTarget.isAlive() && !pTarget.isLying();

  private void continueTalk(Actor pActor, Actor pTarget)
  {
    int pResult;
    pActor.data.get("socialize", out pResult);
    int num1;
    pActor.data.set("socialize", num1 = pResult + 1);
    bool flag = false;
    if (Randy.randomChance(0.4f))
    {
      pActor.clearLastTopicSprite();
      flag = true;
    }
    else if (Randy.randomChance(0.4f))
    {
      pTarget.clearLastTopicSprite();
      flag = true;
    }
    if (!flag && Object.op_Inequality((Object) pTarget.getTopicSpriteTrait(), (Object) null) && Randy.randomChance(0.45f))
      pActor.cloneTopicSprite(pTarget.getSocializeTopic());
    pActor.lookTowardsPosition(pTarget.current_position);
    pTarget.lookTowardsPosition(pActor.current_position);
    pTarget.setTask("socialize_receiving", pForceAction: true);
    float pMaxExclusive = 10f;
    if (Randy.randomBool())
      pActor.playIdleSound();
    else
      pTarget.playIdleSound();
    pActor.setTargetAngleZ(Randy.randomFloat(-pMaxExclusive, pMaxExclusive));
    pTarget.setTargetAngleZ(Randy.randomFloat(-pMaxExclusive, pMaxExclusive));
    float num2 = Randy.randomFloat(1.1f, 3.3f);
    pActor.timer_action = num2;
    pTarget.timer_action = num2;
    if (pActor.timestamp_tween_session_social != 0.0)
      return;
    pActor.timestamp_tween_session_social = BehaviourActionBase<Actor>.world.getCurSessionTime();
    pTarget.timestamp_tween_session_social = BehaviourActionBase<Actor>.world.getCurSessionTime();
  }
}
