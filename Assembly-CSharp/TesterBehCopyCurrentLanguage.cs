// Decompiled with JetBrains decompiler
// Type: TesterBehCopyCurrentLanguage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using System.IO;
using UnityEngine;

#nullable disable
public class TesterBehCopyCurrentLanguage : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    string language = LocalizedTextManager.instance.language;
    string screenshotFolder = TesterBehScreenshotFolder.getScreenshotFolder(language);
    string str = "locales/" + language;
    string text = (Resources.Load(str) as TextAsset).text;
    Console.WriteLine($"[{Date.TimeNow()}] Copying language: {str} to {screenshotFolder}/{language}.json");
    File.WriteAllText($"{screenshotFolder}/{language}.json", text);
    return BehResult.Continue;
  }
}
