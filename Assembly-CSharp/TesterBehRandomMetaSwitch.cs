// Decompiled with JetBrains decompiler
// Type: TesterBehRandomMetaSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TesterBehRandomMetaSwitch : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    if (ScrollWindow.isAnimationActive())
    {
      pObject.wait = 0.1f;
      return BehResult.RepeatStep;
    }
    if (!ScrollWindow.isWindowActive() || !MetaSwitchManager.isSwitcherEnabled())
      return BehResult.Continue;
    using (ListPool<MetaSwitchButton> list = new ListPool<MetaSwitchButton>(2))
    {
      list.Add(MetaSwitchManager.getLeftbutton());
      list.Add(MetaSwitchManager.getRightButton());
      list.RemoveAll((Predicate<MetaSwitchButton>) (pButton => !((Component) pButton).gameObject.activeSelf));
      if (list.Count == 0)
        return BehResult.Continue;
      MetaSwitchButton random = list.GetRandom<MetaSwitchButton>();
      pObject.wait = 0.2f;
      ((UnityEvent) random.button.onClick)?.Invoke();
      return BehResult.Continue;
    }
  }
}
