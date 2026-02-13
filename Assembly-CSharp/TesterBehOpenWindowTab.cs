// Decompiled with JetBrains decompiler
// Type: TesterBehOpenWindowTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class TesterBehOpenWindowTab : BehaviourActionTester
{
  private string _tab;

  public TesterBehOpenWindowTab(string pTab = null) => this._tab = pTab;

  public override BehResult execute(AutoTesterBot pObject)
  {
    pObject.wait = 0.5f;
    if (ScrollWindow.isAnimationActive())
      return BehResult.RepeatStep;
    if (!ScrollWindow.isWindowActive())
      return BehResult.Stop;
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    if (Object.op_Equality((Object) currentWindow, (Object) null))
      return BehResult.Stop;
    currentWindow.tabs.showTab(this._tab);
    return BehResult.Continue;
  }
}
