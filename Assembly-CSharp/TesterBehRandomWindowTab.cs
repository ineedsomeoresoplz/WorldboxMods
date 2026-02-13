// Decompiled with JetBrains decompiler
// Type: TesterBehRandomWindowTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class TesterBehRandomWindowTab : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    if (ScrollWindow.isAnimationActive())
    {
      pObject.wait = 0.1f;
      return BehResult.RepeatStep;
    }
    if (!ScrollWindow.isWindowActive() || Object.op_Equality((Object) ScrollWindow.getCurrentWindow(), (Object) null))
      return BehResult.Stop;
    string pID = Randy.randomBool() ? "window_tab_previous" : "window_tab_next";
    HotkeyAsset pAsset = AssetManager.hotkey_library.get(pID);
    if (pAsset == null)
      return BehResult.Stop;
    pObject.wait = 0.1f;
    HotkeyAction justPressedAction = pAsset.just_pressed_action;
    if (justPressedAction != null)
      justPressedAction(pAsset);
    return BehResult.Continue;
  }
}
