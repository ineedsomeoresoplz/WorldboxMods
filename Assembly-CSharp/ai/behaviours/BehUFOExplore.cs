// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehUFOExplore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehUFOExplore : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    pActor.data.set("exploringTicks", Randy.randomInt(3, 7));
    return BehResult.Continue;
  }
}
