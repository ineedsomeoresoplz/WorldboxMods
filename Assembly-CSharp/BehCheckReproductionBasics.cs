// Decompiled with JetBrains decompiler
// Type: BehCheckReproductionBasics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckReproductionBasics : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.isSapient())
    {
      if (!WorldLawLibrary.world_law_civ_babies.isEnabled())
        return BehResult.Stop;
    }
    else if (!WorldLawLibrary.world_law_animals_babies.isEnabled())
      return BehResult.Stop;
    pActor.subspecies.counter_reproduction_basics_1?.registerEvent();
    if (!pActor.canBreed())
      return BehResult.Stop;
    pActor.subspecies.counter_reproduction_basics_2?.registerEvent();
    if (!BabyHelper.canMakeBabies(pActor) || BabyHelper.isMetaLimitsReached(pActor))
      return BehResult.Stop;
    pActor.subspecies.counter_reproduction_basics_3?.registerEvent();
    if (!pActor.isImportantPerson() && !pActor.isPlacePrivateForBreeding())
      return BehResult.Stop;
    pActor.subspecies.counter_reproduction_basics_4?.registerEvent();
    return BehResult.Continue;
  }
}
