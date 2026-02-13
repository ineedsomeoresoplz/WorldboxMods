// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehWormDive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehWormDive : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int pResult1;
    pActor.data.get("dive_steps", out pResult1);
    int pData1;
    if ((pData1 = pResult1 - 1) < 1)
    {
      pData1 = Randy.randomInt(Randy.randomInt(1, 6), Randy.randomInt(10, 60));
      int pResult2;
      pActor.data.get("size", out pResult2);
      int num1;
      int num2;
      if (!Randy.randomBool())
        num2 = num1 = pResult2 - 1;
      else
        num1 = num2 = pResult2 + 1;
      int pData2 = Mathf.Clamp(num2, 0, 2);
      pActor.data.set("size", pData2);
    }
    pActor.data.set("dive_steps", pData1);
    return BehResult.Continue;
  }
}
