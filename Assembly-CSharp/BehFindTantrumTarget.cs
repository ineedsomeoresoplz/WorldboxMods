// Decompiled with JetBrains decompiler
// Type: BehFindTantrumTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehFindTantrumTarget : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_actor_target != null && pActor.isTargetOkToAttack(pActor.beh_actor_target.a))
      return BehResult.Continue;
    Actor closestActor = this.getClosestActor(pActor);
    if (closestActor == null)
      return this.forceTask(pActor, "random_move");
    pActor.beh_actor_target = (BaseSimObject) closestActor;
    return BehResult.Continue;
  }

  private Actor getClosestActor(Actor pActor)
  {
    bool pRandom = Randy.randomBool();
    WorldTile currentTile = pActor.current_tile;
    float num1 = (float) int.MaxValue;
    Actor closestActor = (Actor) null;
    foreach (Actor pTarget in Finder.getUnitsFromChunk(currentTile, 1, pRandom: pRandom))
    {
      float num2 = (float) Toolbox.SquaredDistTile(pTarget.current_tile, currentTile);
      if ((double) num2 < (double) num1 && pActor.isTargetOkToAttack(pTarget) && (!pTarget.hasStatusStunned() || pActor.areFoes((BaseSimObject) pTarget)))
      {
        num1 = num2;
        closestActor = pTarget;
        if (Randy.randomBool())
          break;
      }
    }
    return closestActor;
  }
}
