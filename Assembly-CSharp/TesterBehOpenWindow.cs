// Decompiled with JetBrains decompiler
// Type: TesterBehOpenWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehOpenWindow : BehaviourActionTester
{
  private string _type;

  public TesterBehOpenWindow(string pType) => this._type = pType;

  public override BehResult execute(AutoTesterBot pObject)
  {
    pObject.wait = 0.5f;
    if (ScrollWindow.isAnimationActive())
      return BehResult.RepeatStep;
    string pWindowID = this._type;
    if (this._type == "random")
      pWindowID = AssetManager.window_library.getTestableWindows().GetRandom<WindowAsset>().id;
    ScrollWindow.showWindow(pWindowID, true);
    return BehResult.Continue;
  }
}
