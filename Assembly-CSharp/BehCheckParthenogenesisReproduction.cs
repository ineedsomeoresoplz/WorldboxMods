// Decompiled with JetBrains decompiler
// Type: BehCheckParthenogenesisReproduction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckParthenogenesisReproduction : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    switch (pActor.subspecies.getReproductionStrategy())
    {
      case ReproductiveStrategy.Egg:
      case ReproductiveStrategy.SpawnUnitImmediate:
        BabyMaker.makeBabyViaParthenogenesis(pActor);
        break;
      case ReproductiveStrategy.Pregnancy:
        BabyHelper.babyMakingStart(pActor);
        float maturationTimeSeconds = pActor.getMaturationTimeSeconds();
        pActor.addStatusEffect("pregnant_parthenogenesis", maturationTimeSeconds);
        pActor.subspecies.counterReproduction();
        break;
    }
    return BehResult.Continue;
  }
}
