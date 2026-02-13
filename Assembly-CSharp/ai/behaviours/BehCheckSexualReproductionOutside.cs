// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckSexualReproductionOutside
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckSexualReproductionOutside : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.canBreed() || !pActor.hasLover())
      return BehResult.Stop;
    Actor lover = pActor.lover;
    if (!lover.canBreed() || !lover.canCurrentTaskBeCancelledByReproduction())
      return BehResult.Stop;
    if (this.tryStartBreeding(pActor, lover))
      return BehResult.RepeatStep;
    pActor.addAfterglowStatus();
    return BehResult.Stop;
  }

  internal bool tryStartBreeding(Actor pActor, Actor pLover)
  {
    pActor.setTask("sexual_reproduction_outside", pCleanJob: true);
    pActor.beh_actor_target = (BaseSimObject) pLover;
    pLover.makeWait();
    return true;
  }
}
