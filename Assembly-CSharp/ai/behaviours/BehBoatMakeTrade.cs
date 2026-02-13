// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatMakeTrade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatMakeTrade : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    pActor.addToInventory("gold", 5);
    return BehResult.Continue;
  }
}
