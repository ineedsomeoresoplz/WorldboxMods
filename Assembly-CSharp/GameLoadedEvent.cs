// Decompiled with JetBrains decompiler
// Type: GameLoadedEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Proyecto26;
using System;
using UnityEngine;

#nullable disable
public class GameLoadedEvent : BaseMapObject
{
  private void Awake()
  {
    LogText.log(nameof (GameLoadedEvent), nameof (Awake), "st");
    LogText.log(nameof (GameLoadedEvent), nameof (Awake), "en");
    this.setVersionData();
  }

  private void setVersionData()
  {
    TextAsset textAsset1 = Resources.Load("texts/build_info") as TextAsset;
    try
    {
      Config.versionCodeText = textAsset1.text.Split('$', StringSplitOptions.None)[0];
      Config.versionCodeDate = textAsset1.text.Split('$', StringSplitOptions.None)[1];
    }
    catch (Exception ex)
    {
      if (Object.op_Inequality((Object) textAsset1, (Object) null))
      {
        Config.versionCodeText = textAsset1.text;
        Config.versionCodeDate = "";
      }
    }
    try
    {
      RestClient.DefaultRequestHeaders["wb-build"] = Config.versionCodeText ?? "na";
    }
    catch (Exception ex)
    {
    }
    try
    {
      TextAsset textAsset2 = Resources.Load("texts/git_info") as TextAsset;
      if (Object.op_Inequality((Object) textAsset2, (Object) null))
        Config.gitCodeText = textAsset2.text;
      try
      {
        RestClient.DefaultRequestHeaders["wb-git"] = Config.gitCodeText ?? "na";
      }
      catch (Exception ex)
      {
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  internal override void create()
  {
    base.create();
    Config.LOAD_TIME_INIT = Time.realtimeSinceStartup;
    LogText.log(nameof (GameLoadedEvent), nameof (create));
    LocalizedTextManager.instance.setLanguage(PlayerConfig.dict["language"].stringVal);
    if (Globals.TRAILER_MODE)
      TrailerModeSettings.startEvent();
    World.world.startTheGame();
    GodPower.diagnostic();
    Config.updateCrashMetadata();
  }
}
