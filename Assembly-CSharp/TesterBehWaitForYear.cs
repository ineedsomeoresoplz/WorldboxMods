// Decompiled with JetBrains decompiler
// Type: TesterBehWaitForYear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehWaitForYear : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    if ((double) pObject.beh_year_target - (double) Date.getCurrentYear() <= 0.0)
      return BehResult.Continue;
    pObject.wait = 1f;
    return BehResult.RepeatStep;
  }
}
