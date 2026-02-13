// Decompiled with JetBrains decompiler
// Type: ai.behaviours.conditions.CondDragonNotSleepy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours.conditions;

public class CondDragonNotSleepy : BehaviourActorCondition
{
  public override bool check(Actor pActor)
  {
    int pResult;
    pActor.data.get("sleepy", out pResult);
    return pResult < 10;
  }
}
