// Decompiled with JetBrains decompiler
// Type: TesterBehChangeWorldSpeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehChangeWorldSpeed : BehaviourActionTester
{
  private string _time_scale_id;

  public TesterBehChangeWorldSpeed(string pTimeScaleID) => this._time_scale_id = pTimeScaleID;

  public override BehResult execute(AutoTesterBot pObject)
  {
    Config.setWorldSpeed(this._time_scale_id);
    return BehResult.Continue;
  }
}
