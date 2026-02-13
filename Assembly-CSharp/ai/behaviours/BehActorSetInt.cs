// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehActorSetInt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehActorSetInt : BehaviourActionActor
{
  private string intName;
  private int intValue;

  public BehActorSetInt(string pIntName, int pIntValue)
  {
    this.intName = pIntName;
    this.intValue = pIntValue;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.data.set(this.intName, this.intValue);
    return BehResult.Continue;
  }
}
