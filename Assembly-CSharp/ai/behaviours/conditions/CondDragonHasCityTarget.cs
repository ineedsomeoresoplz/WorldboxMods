// Decompiled with JetBrains decompiler
// Type: ai.behaviours.conditions.CondDragonHasCityTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours.conditions;

public class CondDragonHasCityTarget : BehaviourActorCondition
{
  public override bool check(Actor pActor)
  {
    int pResult1;
    pActor.data.get("attacksForCity", out pResult1);
    if (pResult1 == 0)
      return false;
    long pResult2;
    pActor.data.get("cityToAttack", out pResult2, -1L);
    return pResult2.hasValue();
  }
}
