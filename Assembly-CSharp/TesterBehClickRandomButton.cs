// Decompiled with JetBrains decompiler
// Type: TesterBehClickRandomButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class TesterBehClickRandomButton : BehaviourActionTester
{
  private Type _type;

  public TesterBehClickRandomButton(Type pButtonType = null) => this._type = pButtonType;

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (ScrollWindow.isAnimationActive())
      return BehResult.RepeatStep;
    if (!ScrollWindow.isWindowActive())
      return BehResult.Stop;
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    if (Object.op_Equality((Object) currentWindow, (Object) null))
      return BehResult.Stop;
    Component[] componentsInChildren = ((Component) currentWindow).GetComponentsInChildren(this._type);
    if (componentsInChildren.Length == 0)
      return BehResult.Stop;
    Component random = Randy.getRandom<Component>(componentsInChildren);
    Button button;
    if (Object.op_Equality((Object) random, (Object) null) || !random.TryGetComponent<Button>(ref button))
      return BehResult.Stop;
    pObject.wait = 0.5f;
    ((UnityEvent) button.onClick)?.Invoke();
    return BehResult.Continue;
  }
}
