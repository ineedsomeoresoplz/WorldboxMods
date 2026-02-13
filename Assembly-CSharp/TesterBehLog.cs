// Decompiled with JetBrains decompiler
// Type: TesterBehLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehLog : BehaviourActionTester
{
  private string _msg;

  public TesterBehLog(string pMessage) => this._msg = pMessage;

  public override BehResult execute(AutoTesterBot pObject)
  {
    new WorldLogMessage(WorldLogLibrary.auto_tester, this._msg).add();
    return BehResult.Continue;
  }
}
