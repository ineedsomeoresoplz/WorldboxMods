// Decompiled with JetBrains decompiler
// Type: BehCheckSoulBorneReproduction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckSoulBorneReproduction : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasStatus("soul_harvested") || pActor.hasStatus("pregnant") || BabyHelper.isMetaLimitsReached(pActor))
      return BehResult.Stop;
    pActor.finishStatusEffect("soul_harvested");
    BabyMaker.startSoulborneBirth(pActor);
    return BehResult.Continue;
  }
}
