// Decompiled with JetBrains decompiler
// Type: TesterBehScreenshotWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using UnityEngine;

#nullable disable
public class TesterBehScreenshotWindow : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    string screenId = currentWindow.screen_id;
    string str = ((int) ((Transform) ((Component) ((Component) currentWindow).transform.FindRecursive("Content")).gameObject.GetComponent<RectTransform>()).localPosition.y).ToString("D4");
    string screenshotFolder = TesterBehScreenshotFolder.getScreenshotFolder(LocalizedTextManager.instance.language);
    Console.WriteLine($"[{Date.TimeNow()}] Screenshotting window: {screenId} to {screenshotFolder}/{screenId}_{str}.png");
    ScreenCapture.CaptureScreenshot($"{screenshotFolder}/{screenId}_{str}_000.png");
    return BehResult.Continue;
  }
}
