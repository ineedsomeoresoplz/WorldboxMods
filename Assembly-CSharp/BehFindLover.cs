// Decompiled with JetBrains decompiler
// Type: BehFindLover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehFindLover : BehaviourActionActor
{
  public override bool shouldRetry(Actor pActor)
  {
    return pActor.hasCity() && BehaviourActionBase<Actor>.world.cities.isLocked() || base.shouldRetry(pActor);
  }

  public override BehResult execute(Actor pActor)
  {
    if (pActor.hasLover())
      return BehResult.Stop;
    Actor pTarget = this.findLoverAround(pActor) ?? this.checkCityLovers(pActor);
    if (pTarget != null)
      pActor.becomeLoversWith(pTarget);
    return BehResult.Continue;
  }

  private Actor findLoverAround(Actor pActor)
  {
    Actor loverAround = (Actor) null;
    foreach (Actor pTarget in Finder.getUnitsFromChunk(pActor.current_tile, 1))
    {
      if (this.checkIfPossibleLover(pActor, pTarget))
      {
        loverAround = pTarget;
        break;
      }
    }
    return loverAround;
  }

  private bool checkIfPossibleLover(Actor pActor, Actor pTarget)
  {
    return pTarget != pActor && pTarget.hasSubspecies() && pTarget.isAlive() && pTarget.canFallInLoveWith(pActor);
  }

  private Actor checkCityLovers(Actor pActor)
  {
    if (!pActor.hasCity())
      return (Actor) null;
    Actor actor = (Actor) null;
    foreach (Actor pTarget in pActor.city.getUnits().LoopRandom<Actor>())
    {
      if (this.checkIfPossibleLover(pActor, pTarget) && pTarget.inOwnCityBorders())
      {
        actor = pTarget;
        break;
      }
    }
    return actor;
  }
}
