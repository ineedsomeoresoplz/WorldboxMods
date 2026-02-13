// Decompiled with JetBrains decompiler
// Type: TesterBehChangeWorldLaw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehChangeWorldLaw : BehaviourActionTester
{
  private string world_law;
  private bool value;

  public TesterBehChangeWorldLaw(string pWorldLaw, bool pValue)
  {
    this.world_law = pWorldLaw;
    this.value = pValue;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    BehaviourActionBase<AutoTesterBot>.world.world_laws.dict[this.world_law].boolVal = this.value;
    return BehResult.Continue;
  }
}
