// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSandspiderCheckDie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehSandspiderCheckDie : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int pResult;
    pActor.data.get("ant_steps", out pResult);
    if (pActor.beh_tile_target != null && pResult <= 20)
      return BehResult.Continue;
    pActor.dieSimpleNone();
    return BehResult.Stop;
  }
}
