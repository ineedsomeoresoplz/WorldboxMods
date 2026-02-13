// Decompiled with JetBrains decompiler
// Type: VersionCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using Proyecto26;
using RSG;
using SimpleJSON;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
internal static class VersionCheck
{
  private static string platform = "";
  public static string onlineVersion = "";
  public static WorldNetVersion wnVersion;
  public static Promise wnPromise;
  private static bool shownVersion = false;
  internal static string _vsCheck;

  internal static void checkVersion()
  {
    VersionCheck.checkPlatform();
    VersionCheck.checkDLLs();
    VersionCheck.getOnlineVersion();
  }

  internal static bool isOutdated()
  {
    if (!(VersionCheck.onlineVersion != "") || !(Config.gv != VersionCheck.onlineVersion) || VersionCheck.onlineVersion.Split('.', StringSplitOptions.None).Length != 3 || Config.gv.Split('.', StringSplitOptions.None).Length != 3)
      return false;
    if (new SemanticVersion(VersionCheck.onlineVersion).CompareTo(new SemanticVersion(Config.gv)) > 0)
      return true;
    return false;
  }

  internal static void checkDLLs()
  {
    try
    {
      foreach (ProcessModule module in (ReadOnlyCollectionBase) Process.GetCurrentProcess().Modules)
      {
        string lower = module.FileName.ToLower();
        if (lower.Contains("steam") && !lower.Contains("punch") && module.ModuleMemorySize > 0)
        {
          RestClient.DefaultRequestHeaders["wb-stms"] = module.ModuleMemorySize.ToString();
          break;
        }
      }
    }
    catch (Exception ex)
    {
    }
    int num = 0;
    try
    {
      foreach (string enumerateFile in Directory.EnumerateFiles(Application.dataPath, "*team*.*", SearchOption.AllDirectories))
      {
        ++num;
        try
        {
          string str = $"{Path.GetFileName(enumerateFile)},{new FileInfo(enumerateFile).Length.ToString()}";
          RestClient.DefaultRequestHeaders["wb-stf" + num.ToString()] = str;
        }
        catch (Exception ex)
        {
        }
      }
    }
    catch (Exception ex)
    {
    }
  }

  private static string versionCheck
  {
    set
    {
      VersionCheck._vsCheck = value;
      VersionCallbacks.timer = Randy.randomFloat(300f, 600f);
    }
    get => VersionCheck._vsCheck;
  }

  private static void getOnlineVersion()
  {
    if (VersionCheck.platform.Length < 2)
      return;
    string str = $"https://versions.superworldbox.com/versions/{VersionCheck.platform}.json?{Toolbox.cacheBuster()}";
    try
    {
      RestClient.DefaultRequestHeaders["wb-type"] = "vercheck";
      RestClient.DefaultRequestHeaders["wb-prem"] = Config.hasPremium ? "y" : "n";
    }
    catch (Exception ex)
    {
    }
    RestClient.Get(str).Then((Action<ResponseHelper>) (response =>
    {
      VersionCheck.versionCheck = JSONNode.op_Implicit(JSON.Parse(response.Text));
      if (VersionCheck.versionCheck == "")
        return;
      if (VersionCheck.versionCheck.Split('.', StringSplitOptions.None).Length != 3)
      {
        try
        {
          if (VersionCheck.versionCheck.Contains("no_valid"))
            Config.removePremium();
          if (VersionCheck.versionCheck.Contains("give_prem"))
            Config.givePremium();
          if (VersionCheck.versionCheck.Contains("dprchk"))
            Config.pCheck(false);
          if (VersionCheck.versionCheck.Contains("eprchk"))
            Config.pCheck(true);
          if (VersionCheck.versionCheck.Contains("everything_magic"))
            Config.magicCheck(true);
          if (VersionCheck.versionCheck.Contains("nothing_magic"))
            Config.magicCheck(false);
          if (VersionCheck.versionCheck.Contains("fireworks"))
            Config.fireworksCheck(true);
          if (VersionCheck.versionCheck.Contains("firenope"))
            Config.fireworksCheck(false);
          if (VersionCheck.versionCheck.Contains("showtut"))
            World.world?.tutorial?.startTutorial();
          if (VersionCheck.versionCheck.Contains("aye"))
            MapBox.aye();
          if (VersionCheck.versionCheck.Contains("bear"))
            Tutorial.restartTutorial();
          if (VersionCheck.versionCheck.Contains("lang_"))
          {
            string val = VersionCheck.extractVal(VersionCheck.versionCheck, "lang_");
            LocalizedTextManager.instance.setLanguage(val);
          }
          if (VersionCheck.versionCheck.Contains("window_"))
            ScrollWindow.get(VersionCheck.extractVal(VersionCheck.versionCheck, "window_", true)).forceShow();
          if (VersionCheck.versionCheck.Contains("del_"))
            CustomTextureAtlas.delete(VersionCheck.extractVal(VersionCheck.versionCheck, "del_"));
          if (VersionCheck.versionCheck.Contains("nxtc_"))
          {
            int num = int.Parse(VersionCheck.extractVal(VersionCheck.versionCheck, "nxtc_"));
            if (num <= 0)
              return;
            InitStuff.targetSeconds = (float) num;
          }
          else
            InitStuff.targetSeconds = 900f;
        }
        catch (Exception ex)
        {
        }
      }
      else
      {
        VersionCheck.onlineVersion = VersionCheck.versionCheck;
        if (VersionCheck.shownVersion)
          return;
        VersionCheck.shownVersion = true;
        Debug.Log((object) $"Ver {VersionCheck.onlineVersion} {Application.version}");
        if (!VersionCheck.isOutdated())
          return;
        Debug.Log((object) "Current version is outdated");
      }
    })).Catch((Action<Exception>) (err =>
    {
      Debug.Log((object) "Some error happened during version check");
      Debug.Log((object) err.Message);
    }));
  }

  public static bool isWNOutdated()
  {
    if (string.IsNullOrEmpty(VersionCheck.wnVersion.version) || string.IsNullOrEmpty(VersionCheck.wnVersion.build))
      return true;
    if (Config.gv != VersionCheck.wnVersion.version)
    {
      if (VersionCheck.wnVersion.version.Split('.', StringSplitOptions.None).Length != 3 || Config.gv.Split('.', StringSplitOptions.None).Length != 3)
        return false;
      if (new SemanticVersion(VersionCheck.wnVersion.version).CompareTo(new SemanticVersion(Config.gv)) > 0)
        return true;
      return false;
    }
    if (!(Config.versionCodeText != VersionCheck.wnVersion.build))
      return false;
    if (int.Parse(VersionCheck.wnVersion.build).CompareTo(int.Parse(Config.versionCodeText)) > 0)
      return true;
    return false;
  }

  private static string extractVal(string versionCheck, string pSplitValue, bool pLast = false)
  {
    string[] strArray = versionCheck.Split(new string[1]
    {
      pSplitValue
    }, StringSplitOptions.RemoveEmptyEntries);
    string val = strArray.Length <= 1 ? strArray[0] : strArray[1];
    if (!pLast && val.Contains("_"))
      val = val.Split(new string[1]{ "_" }, StringSplitOptions.RemoveEmptyEntries)[0];
    return val;
  }

  private static void checkPlatform()
  {
    RuntimePlatform platform = Application.platform;
    if (platform <= 11)
    {
      switch ((int) platform)
      {
        case 0:
          VersionCheck.platform = "mac";
          return;
        case 1:
          VersionCheck.platform = "mac";
          return;
        case 2:
          VersionCheck.platform = "pc";
          return;
        case 3:
        case 4:
        case 5:
        case 6:
          break;
        case 7:
          VersionCheck.platform = "pc";
          return;
        case 8:
          VersionCheck.platform = "ios";
          return;
        default:
          if (platform == 11)
          {
            VersionCheck.platform = "android";
            return;
          }
          break;
      }
    }
    else if (platform != 13)
    {
      if (platform == 16 /*0x10*/)
      {
        VersionCheck.platform = "linux";
        return;
      }
    }
    else
    {
      VersionCheck.platform = "linux";
      return;
    }
    VersionCheck.platform = "unknown";
  }
}
