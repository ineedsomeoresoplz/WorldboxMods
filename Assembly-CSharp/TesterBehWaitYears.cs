// Decompiled with JetBrains decompiler
// Type: TesterBehWaitYears
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehWaitYears : BehaviourActionTester
{
  private int wait_years;

  public TesterBehWaitYears(int pWaitYears) => this.wait_years = pWaitYears;

  public override BehResult execute(AutoTesterBot pObject)
  {
    pObject.beh_year_target = Date.getCurrentYear() + this.wait_years;
    return BehResult.Continue;
  }
}
