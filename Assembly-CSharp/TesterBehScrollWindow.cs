// Decompiled with JetBrains decompiler
// Type: TesterBehScrollWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TesterBehScrollWindow : BehaviourActionTester
{
  private static string[] skipWindows = new string[2]
  {
    "saves_list",
    "patch_log"
  };

  public override BehResult execute(AutoTesterBot pObject)
  {
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    string screenId = currentWindow.screen_id;
    if (TesterBehScrollWindow.skipWindows.IndexOf<string>(screenId) > -1)
      return BehResult.Continue;
    Transform recursive = ((Component) currentWindow).transform.FindRecursive("Scrollbar Vertical");
    if (!((Component) recursive).gameObject.activeInHierarchy)
      return BehResult.Continue;
    Scrollbar component = ((Component) recursive).gameObject.GetComponent<Scrollbar>();
    float num1 = component.value;
    float size = component.size;
    if ((double) size < 0.05000000074505806 || (double) size > 0.949999988079071 || (double) num1 <= 0.10000000149011612)
      return BehResult.Continue;
    float num2 = num1 - size;
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    component.value = num2;
    return BehResult.RestartTask;
  }
}
