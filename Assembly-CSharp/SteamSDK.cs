// Decompiled with JetBrains decompiler
// Type: SteamSDK
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Proyecto26;
using RSG;
using Steamworks;
using System;
using UnityEngine;

#nullable disable
public class SteamSDK : MonoBehaviour
{
  public const uint STEAM_APP_ID = 1206560;
  internal static Promise steamInitialized = new Promise();
  private bool _initiated;
  private static SteamSDK _instance;
  private static bool _should_quit = false;
  private static readonly string[] _supported_steam_languages = new string[28]
  {
    "ar",
    "cz",
    "ch",
    "cs",
    "da",
    "nl",
    "en",
    "fn",
    "fr",
    "de",
    "gr",
    "hu",
    "it",
    "ja",
    "ko",
    "no",
    "pl",
    "pt",
    "br",
    "ro",
    "ru",
    "es",
    "es",
    "sv",
    "th",
    "tr",
    "ua",
    "vn"
  };

  private void Start()
  {
    if (this._initiated)
      return;
    this._initiated = true;
    bool flag = false;
    try
    {
      SteamSDK._instance = this;
      SteamClient.Init(1206560U, true);
      RestClient.DefaultRequestHeaders["wb-stmc"] = "true";
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Disabling Steam Integration");
      Debug.LogWarning((object) ex);
      RestClient.DefaultRequestHeaders["wb-stmc"] = "na";
      flag = true;
      SteamSDK._should_quit = true;
    }
    try
    {
      string str = SteamClient.SteamId.ToString();
      if (!string.IsNullOrEmpty(str))
      {
        Config.steam_id = str;
        RestClient.DefaultRequestHeaders["wb-stm"] = str;
        Debug.Log((object) ("S:" + Config.steam_id));
      }
      else
        Debug.Log((object) "S:nf");
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (Config.steam_language_allow_detect)
      {
        Debug.Log((object) "s:Detect - Steam detecting language");
        string steamLanguage = SteamSDK.getSteamLanguage();
        if (!string.IsNullOrEmpty(steamLanguage))
        {
          string language = LocalizedTextManager.instance.language;
          if (steamLanguage == "en" && language != "en")
            Debug.Log((object) "s:Detect - Already have a language, not falling back to english");
          else
            LocalizedTextManager.instance.setLanguage(steamLanguage);
        }
        Debug.Log((object) ("s:Detect - language " + steamLanguage));
      }
    }
    catch (Exception ex)
    {
    }
    try
    {
      string name = SteamClient.Name;
      if (!string.IsNullOrEmpty(name))
        Config.steam_name = name;
    }
    catch (Exception ex)
    {
    }
    try
    {
      if (SteamClient.RestartAppIfNecessary(1206560U))
      {
        Debug.Log((object) "Restart App from Steam launcher");
        SteamSDK._should_quit = true;
        flag = true;
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
    if (SteamSDK._should_quit && !Config.disable_steam)
      Application.Quit();
    if (flag)
    {
      Debug.Log((object) "Steam is not available");
      SteamSDK.steamInitialized.Reject(new Exception("Steam is not available"));
      Object.Destroy((Object) SteamSDK._instance);
    }
    else
      SteamSDK.steamInitialized.Resolve();
  }

  private static string getSteamLanguage()
  {
    switch (SteamApps.GameLanguage)
    {
      case "arabic":
        return "ar";
      case "brazilian":
        return "br";
      case "czech":
        return "cs";
      case "danish":
        return "da";
      case "dutch":
        return "nl";
      case "english":
        return "en";
      case "finnish":
        return "fn";
      case "french":
        return "fr";
      case "german":
        return "de";
      case "greek":
        return "gr";
      case "hungarian":
        return "hu";
      case "indonesian":
        return "id";
      case "italian":
        return "it";
      case "japanese":
        return "ja";
      case "korean":
      case "koreana":
        return "ko";
      case "latam":
        return "es";
      case "norwegian":
        return "no";
      case "polish":
        return "pl";
      case "portuguese":
        return "pt";
      case "romanian":
        return "ro";
      case "russian":
        return "ru";
      case "schinese":
        return "cz";
      case "spanish":
        return "es";
      case "swedish":
        return "sv";
      case "tchinese":
        return "ch";
      case "thai":
        return "th";
      case "turkish":
        return "tr";
      case "ukrainian":
        return "ua";
      case "vietnamese":
        return "vn";
      default:
        return string.Empty;
    }
  }

  private void OnDisable()
  {
    try
    {
      SteamClient.Shutdown();
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ex);
      Object.Destroy((Object) SteamSDK._instance);
    }
  }

  private void OnDestroy() => SteamSDK._instance = (SteamSDK) null;
}
