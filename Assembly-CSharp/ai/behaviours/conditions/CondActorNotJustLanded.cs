// Decompiled with JetBrains decompiler
// Type: ai.behaviours.conditions.CondActorNotJustLanded
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours.conditions;

public class CondActorNotJustLanded : BehaviourActorCondition
{
  public override bool check(Actor pActor)
  {
    int pResult;
    pActor.data.get("justLanded", out pResult);
    int num1 = pResult <= 0 ? 1 : 0;
    int num2 = pResult - 1;
    pActor.data.set("justLanded", Mathf.Max(num2, 0));
    return num1 != 0;
  }
}
