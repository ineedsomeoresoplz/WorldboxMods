// Decompiled with JetBrains decompiler
// Type: BehFindTargetToStealFrom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehFindTargetToStealFrom : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    Actor closestActorWithMoneys = this.getClosestActorWithMoneys(pActor);
    if (closestActorWithMoneys == null)
      return BehResult.Stop;
    pActor.beh_actor_target = (BaseSimObject) closestActorWithMoneys;
    return BehResult.Continue;
  }

  private Actor getClosestActorWithMoneys(Actor pActor)
  {
    using (ListPool<Actor> pCollection = new ListPool<Actor>(4))
    {
      bool pRandom = Randy.randomBool();
      int pChunkRadius = Randy.randomInt(1, 4);
      int num = Randy.randomInt(1, 4);
      foreach (Actor pTarget in Finder.getUnitsFromChunk(pActor.current_tile, pChunkRadius, pRandom: pRandom))
      {
        if (pTarget != pActor && pActor.isSameIslandAs((BaseSimObject) pTarget) && pTarget.hasAnyCash())
        {
          pCollection.Add(pTarget);
          if (pCollection.Count >= num)
            break;
        }
      }
      return Toolbox.getClosestActor(pCollection, pActor.current_tile);
    }
  }
}
