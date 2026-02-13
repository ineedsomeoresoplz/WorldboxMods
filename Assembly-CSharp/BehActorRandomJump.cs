// Decompiled with JetBrains decompiler
// Type: BehActorRandomJump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class BehActorRandomJump : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    float pForceAmountDirection = Randy.randomFloat(1f, 5f);
    float pForceHeight = Randy.randomFloat(1f, 2f);
    Vector2 currentPosition = pActor.current_position;
    float degrees = Randy.randomFloat(-180f, 180f);
    Vector2 pVec2 = Vector2.op_Addition(currentPosition, Vector2.op_Multiply(Toolbox.rotateVector(currentPosition, degrees), pForceAmountDirection));
    pActor.calculateForce(currentPosition.x, currentPosition.y, pVec2.x, pVec2.y, pForceAmountDirection, pForceHeight);
    pActor.punchTargetAnimation(Vector2.op_Implicit(currentPosition), false, pAngle: -60f);
    if (pActor.is_visible)
    {
      BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_dodge", pActor.current_position, pActor.actor_scale);
      if (Object.op_Inequality((Object) baseEffect, (Object) null))
        ((Component) baseEffect).transform.rotation = Toolbox.getEulerAngle(currentPosition, pVec2);
    }
    return BehResult.Continue;
  }
}
