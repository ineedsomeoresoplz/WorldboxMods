// Decompiled with JetBrains decompiler
// Type: BehAffectDreams
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehAffectDreams : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    Actor randomDreamingActor = this.getRandomDreamingActor(pActor);
    if (randomDreamingActor == null)
      return BehResult.Stop;
    randomDreamingActor.tryToConvertActorToMetaFromActor(pActor);
    return BehResult.Continue;
  }

  private Actor getRandomDreamingActor(Actor pActor)
  {
    BehaviourActionBase<Actor>.world.units.checkSleepingUnits();
    if (BehaviourActionBase<Actor>.world.units.cached_sleeping_units.Count == 0)
      return (Actor) null;
    foreach (Actor randomDreamingActor in BehaviourActionBase<Actor>.world.units.cached_sleeping_units.LoopRandom<Actor>())
    {
      if (randomDreamingActor.isAlive() && randomDreamingActor.hasSubspecies() && randomDreamingActor.hasStatus("sleeping") && (randomDreamingActor.subspecies.has_advanced_memory || randomDreamingActor.subspecies.has_advanced_communication))
        return randomDreamingActor;
    }
    return (Actor) null;
  }
}
