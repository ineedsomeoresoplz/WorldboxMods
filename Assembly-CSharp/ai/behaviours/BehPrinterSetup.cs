// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehPrinterSetup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehPrinterSetup : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int pResult1;
    pActor.data.get("step", out pResult1, -1);
    if (pResult1 < 0)
    {
      ActorData data1 = pActor.data;
      Vector2Int pos1 = pActor.current_tile.pos;
      int x = ((Vector2Int) ref pos1).x;
      data1.set("origin_x", x);
      ActorData data2 = pActor.data;
      Vector2Int pos2 = pActor.current_tile.pos;
      int y = ((Vector2Int) ref pos2).y;
      data2.set("origin_y", y);
      string pResult2;
      pActor.data.get("template", out pResult2);
      PrintTemplate template = PrintLibrary.getTemplate(pResult2);
      pActor.data.set("steps", template.steps.Length);
      pActor.data.set("step", 0);
    }
    int pResult3;
    pActor.data.get("steps", out pResult3, -1);
    if (pResult1 < pResult3)
      return BehResult.Continue;
    pActor.dieSimpleNone();
    return BehResult.Stop;
  }
}
