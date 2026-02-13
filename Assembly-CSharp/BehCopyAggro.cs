// Decompiled with JetBrains decompiler
// Type: BehCopyAggro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCopyAggro : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    Actor a = pActor.beh_actor_target?.a;
    if (a == null)
      return BehResult.Continue;
    pActor.copyAggroFrom(a);
    this.copyEnemiesOf(pActor, a);
    return BehResult.Continue;
  }

  private void copyEnemiesOf(Actor pCopyTo, Actor pTarget)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTarget.current_tile, 1, pRandom: true))
    {
      if (actor != pCopyTo && actor.isInAggroList(pTarget) && pCopyTo.isSameIslandAs((BaseSimObject) actor))
        pCopyTo.addAggro(actor);
    }
  }
}
