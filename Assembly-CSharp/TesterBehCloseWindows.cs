// Decompiled with JetBrains decompiler
// Type: TesterBehCloseWindows
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehCloseWindows : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    if (ScrollWindow.isAnimationActive())
    {
      pObject.wait = 0.1f;
      return BehResult.RepeatStep;
    }
    if (!ScrollWindow.isWindowActive())
      return BehResult.Continue;
    ScrollWindow.hideAllEvent(false);
    pObject.wait = 0.25f;
    return BehResult.RepeatStep;
  }
}
