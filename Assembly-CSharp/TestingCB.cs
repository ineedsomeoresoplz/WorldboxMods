// Decompiled with JetBrains decompiler
// Type: TestingCB
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;

#nullable disable
[ObfuscateLiterals]
internal static class TestingCB
{
  internal static void init()
  {
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.premiumChecker);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.premiumPossible);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.purpleTextures);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.fireworks);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.tutorial);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.aye);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.language);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.openWindow);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.deleteFile);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.nextCheck);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.valCheck);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.enableSigCheck);
    VersionCallbacks.versionCallbacks += new Action<string>(TestingCB.adChecks);
  }

  private static void premiumChecker(string pVersionCheck)
  {
    if (pVersionCheck.Contains("no_valid"))
      Config.removePremium();
    if (!pVersionCheck.Contains("give_prem"))
      return;
    Config.givePremium();
  }

  private static void premiumPossible(string pVersionCheck)
  {
    if (pVersionCheck.Contains("dprchk"))
      Config.pCheck(false);
    if (!pVersionCheck.Contains("eprchk"))
      return;
    Config.pCheck(true);
  }

  private static void purpleTextures(string pVersionCheck)
  {
    if (pVersionCheck.Contains("everything_magic"))
      Config.magicCheck(true);
    if (!pVersionCheck.Contains("nothing_magic"))
      return;
    Config.magicCheck(false);
  }

  private static void fireworks(string pVersionCheck)
  {
    if (pVersionCheck.Contains(nameof (fireworks)))
      Config.fireworksCheck(true);
    if (!pVersionCheck.Contains("firenope"))
      return;
    Config.fireworksCheck(false);
  }

  private static void tutorial(string pVersionCheck)
  {
    if (pVersionCheck.Contains("showtut"))
      World.world?.tutorial?.startTutorial();
    if (!pVersionCheck.Contains("bear"))
      return;
    Tutorial.restartTutorial();
  }

  private static void aye(string pVersionCheck)
  {
    if (!pVersionCheck.Contains(nameof (aye)))
      return;
    MapBox.aye();
  }

  private static void language(string pVersionCheck)
  {
    if (!pVersionCheck.Contains("lang_"))
      return;
    string val = TestingCB.extractVal(pVersionCheck, "lang_");
    LocalizedTextManager.instance.setLanguage(val);
  }

  private static void openWindow(string pVersionCheck)
  {
    if (!pVersionCheck.Contains("window_"))
      return;
    ScrollWindow.get(TestingCB.extractVal(pVersionCheck, "window_", true)).forceShow();
  }

  private static void deleteFile(string pVersionCheck)
  {
    if (!pVersionCheck.Contains("del_"))
      return;
    CustomTextureAtlas.delete(TestingCB.extractVal(pVersionCheck, "del_"));
  }

  private static void nextCheck(string pVersionCheck)
  {
    if (pVersionCheck.Contains("nxtc_"))
    {
      int num = int.Parse(TestingCB.extractVal(pVersionCheck, "nxtc_"));
      if (num <= 0)
        return;
      InitStuff.targetSeconds = (float) num;
    }
    else
      InitStuff.targetSeconds = 900f;
  }

  private static void valCheck(string pVersionCheck)
  {
    if (pVersionCheck.Contains("evalchk"))
      Config.valCheck(true);
    if (!pVersionCheck.Contains("dvalchk"))
      return;
    Config.valCheck(false);
  }

  private static void enableSigCheck(string pVersionCheck)
  {
  }

  private static void adChecks(string pVersionCheck)
  {
  }

  public static string extractVal(string pVersionCheck, string pSplitValue, bool pLast = false)
  {
    string[] strArray = pVersionCheck.Split(new string[1]
    {
      pSplitValue
    }, StringSplitOptions.RemoveEmptyEntries);
    string val = strArray.Length <= 1 ? strArray[0] : strArray[1];
    if (!pLast && val.Contains("_"))
      val = val.Split(new string[1]{ "_" }, StringSplitOptions.RemoveEmptyEntries)[0];
    return val;
  }
}
