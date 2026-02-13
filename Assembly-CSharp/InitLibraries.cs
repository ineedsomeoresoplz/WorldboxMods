// Decompiled with JetBrains decompiler
// Type: InitLibraries
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class InitLibraries : MonoBehaviour
{
  public static bool initiated;
  public static bool initiated_main;

  private void Awake() => this.initLibs();

  public static void initMainLibs()
  {
    if (InitLibraries.initiated_main)
      return;
    InitLibraries.initiated_main = true;
    try
    {
      Config.gv = Application.version;
      Config.iname = Application.installerName;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    try
    {
      if (Application.genuineCheckAvailable)
        Config.gen = new bool?(Application.genuine);
    }
    catch (Exception ex)
    {
      Config.gen = new bool?();
    }
    LogHandler.init();
    AssetManager.initMain();
    GameProgress.init();
    PlayerConfig.init();
    LocalizedTextManager.init();
  }

  private void initLibs()
  {
    if (InitLibraries.initiated)
      return;
    InitLibraries.initiated = true;
    LogText.log("InitLibraries " + Config.gv, nameof (initLibs), "st");
    DebugConfig.init();
    InitLibraries.initMainLibs();
    AssetManager.init();
    DebugConfig.checkSonicTimeScales();
    NameGenerator.init();
    LogText.log(nameof (InitLibraries), nameof (initLibs), "en");
  }
}
