// Decompiled with JetBrains decompiler
// Type: BehChildFindRandomFamilyParent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehChildFindRandomFamilyParent : BehaviourActionActor
{
  public override BehResult execute(Actor pBabyActor)
  {
    if (!pBabyActor.family.hasFounders())
      return BehResult.Stop;
    Actor randomFounder = pBabyActor.family.getRandomFounder();
    if (pBabyActor.inOwnCityBorders() && !randomFounder.inOwnCityBorders())
      return BehResult.Stop;
    pBabyActor.beh_actor_target = (BaseSimObject) randomFounder;
    return BehResult.Continue;
  }
}
