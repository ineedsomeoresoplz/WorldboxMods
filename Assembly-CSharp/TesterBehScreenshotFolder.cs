// Decompiled with JetBrains decompiler
// Type: TesterBehScreenshotFolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.IO;

#nullable disable
public class TesterBehScreenshotFolder : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    string screenshotFolder = TesterBehScreenshotFolder.getScreenshotFolder(LocalizedTextManager.instance.language);
    if (!Directory.Exists(screenshotFolder))
      Directory.CreateDirectory(screenshotFolder);
    return BehResult.Continue;
  }

  internal static string getScreenshotFolder(string pLanguage) => "GenAssets/Windows/" + pLanguage;
}
