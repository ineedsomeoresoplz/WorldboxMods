// Decompiled with JetBrains decompiler
// Type: PlayerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Firebase.Analytics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class PlayerConfig
{
  public static PlayerConfig instance;
  public static Dictionary<string, PlayerOptionData> dict = new Dictionary<string, PlayerOptionData>();
  private string dataPath;
  internal PlayerConfigData data;
  private float rewardCheckTimer = 10f;
  private float rewardCheckTimerInterval = 60f;
  private static bool _memory_check_done = false;

  public static void init()
  {
    if (PlayerConfig.instance != null)
      return;
    Debug.Log((object) "INIT PlayerConfig");
    PlayerConfig.instance = new PlayerConfig();
    PlayerConfig.instance.create();
  }

  public void create()
  {
    this.rewardCheckTimer = this.rewardCheckTimerInterval;
    this.setNewDataPath();
    Debug.Log((object) "Init PlayerConfig");
    if (File.Exists(this.dataPath))
    {
      try
      {
        this.loadData();
      }
      catch (Exception ex)
      {
        this.initNewSave();
      }
    }
    else
      this.initNewSave();
  }

  internal void start() => AdButtonTimer.setAdTimer();

  internal void update()
  {
  }

  private void setNewDataPath() => this.dataPath = Application.persistentDataPath + "/worldboxData";

  private void initNewSave()
  {
    this.data = new PlayerConfigData();
    this.data.initData();
    PlayerConfig.dict["language"].stringVal = PlayerConfig.detectLanguage();
    Config.steam_language_allow_detect = true;
    if (Globals.specialAbstudio)
      PlayerConfig.dict["language"].stringVal = "fa";
    PlayerConfig.saveData();
  }

  public static void setFirebaseProp(string pName, string pProp)
  {
    if (!Config.isMobile)
      return;
    if (!Config.firebase_available)
      return;
    try
    {
      FirebaseAnalytics.SetUserProperty(pName, pProp);
    }
    catch (Exception ex)
    {
    }
  }

  public static void toggleFullScreen()
  {
    PlayerConfig.setFullScreen(!PlayerConfig.dict["fullscreen"].boolVal);
  }

  public static void setFullScreen(bool pFullScreen, bool pSwitchScreen = true)
  {
    PlayerConfig.dict["fullscreen"].boolVal = pFullScreen;
    PlayerConfig.saveData();
    OptionAsset pAsset = AssetManager.options_library.get("fullscreen");
    pAsset.action(pAsset);
  }

  public static string detectLanguage()
  {
    string str;
    switch (Application.systemLanguage - 1)
    {
      case 0:
        str = "ar";
        break;
      case 6:
        str = "cs";
        break;
      case 7:
        str = "da";
        break;
      case 8:
        str = "nl";
        break;
      case 9:
        str = "en";
        break;
      case 12:
        str = "fn";
        break;
      case 13:
        str = "fr";
        break;
      case 14:
        str = "de";
        break;
      case 15:
        str = "gr";
        break;
      case 16 /*0x10*/:
        str = "he";
        break;
      case 17:
        str = "hu";
        break;
      case 19:
        str = "id";
        break;
      case 20:
        str = "it";
        break;
      case 21:
        str = "ja";
        break;
      case 22:
        str = "ko";
        break;
      case 24:
        str = "lt";
        break;
      case 25:
        str = "no";
        break;
      case 26:
        str = "pl";
        break;
      case 27:
        str = "pt";
        break;
      case 28:
        str = "ro";
        break;
      case 29:
        str = "ru";
        break;
      case 30:
        str = "hr";
        break;
      case 31 /*0x1F*/:
        str = "sk";
        break;
      case 33:
        str = "es";
        break;
      case 34:
        str = "sv";
        break;
      case 35:
        str = "th";
        break;
      case 36:
        str = "tr";
        break;
      case 37:
        str = "ua";
        break;
      case 38:
        str = "vn";
        break;
      case 39:
        str = "cz";
        break;
      case 40:
        str = "ch";
        break;
      default:
        str = "en";
        break;
    }
    return str;
  }

  public static void saveData()
  {
    string pStringData = Toolbox.encode(PlayerConfig.instance.data.toJson());
    Toolbox.WriteSafely("Player Config", PlayerConfig.instance.dataPath, ref pStringData);
    foreach (PlayerOptionData playerOptionData in PlayerConfig.dict.Values)
    {
      if (playerOptionData.boolVal)
        PlayerConfig.setFirebaseProp("option_" + playerOptionData.name, playerOptionData.boolVal ? "on" : "off");
    }
    PlayerConfig.setFirebaseProp("option_language", PlayerConfig.dict["language"].stringVal);
  }

  private void loadData()
  {
    if (!File.Exists(this.dataPath))
      return;
    string pString = File.ReadAllText(this.dataPath);
    string str;
    try
    {
      str = Toolbox.decode(pString);
    }
    catch (Exception ex)
    {
      str = "";
    }
    if (string.IsNullOrEmpty(str))
    {
      try
      {
        str = Toolbox.decodeMobile(pString);
      }
      catch (Exception ex)
      {
        str = "";
      }
    }
    if (!string.IsNullOrEmpty(str))
      pString = str;
    if (pString.Contains("list"))
    {
      this.data = JsonConvert.DeserializeObject<PlayerConfigData>(pString);
      this.data.initData();
      string stringVal = this.data.get("language").stringVal;
      if (stringVal == "boat" || stringVal == "keys")
        this.data.get("language").stringVal = PlayerConfig.detectLanguage();
    }
    else
      this.initNewSave();
    if (this.data.fireworksCheck2025)
      Config.EVERYTHING_FIREWORKS = true;
    if (this.data.magicCheck2025)
      Config.EVERYTHING_MAGIC_COLOR = true;
    if (Config.isEditor && Config.editor_test_rewards_from_ads)
      this.data.rewardedPowers.Clear();
    if (this.data.premium)
      Config.hasPremium = true;
    bool flag = false;
    if (this.moveTraitsAndAchievements())
      flag = true;
    if (this.handleDebugOptions())
      flag = true;
    if (!flag)
      return;
    PlayerConfig.saveData();
  }

  internal static bool optionEnabled(string gameOptionName, OptionType pType)
  {
    foreach (PlayerOptionData playerOptionData in PlayerConfig.instance.data.list)
    {
      if (!(playerOptionData.name != gameOptionName) && pType == OptionType.Bool)
        return playerOptionData.boolVal;
    }
    return false;
  }

  public static int getIntValue(string pID)
  {
    PlayerOptionData playerOptionData = PlayerConfig.dict[pID];
    OptionAsset optionAsset = AssetManager.options_library.get(pID);
    return playerOptionData.intVal != Mathf.Clamp(playerOptionData.intVal, optionAsset.min_value, optionAsset.max_value) ? optionAsset.default_int : playerOptionData.intVal;
  }

  public static bool optionBoolEnabled(string pName) => PlayerConfig.dict[pName].boolVal;

  public static int getOptionInt(string pName) => PlayerConfig.dict[pName].intVal;

  public static string getOptionString(string pName) => PlayerConfig.dict[pName].stringVal;

  public static void setOptionBool(string pName, bool pVal)
  {
    PlayerConfig.dict[pName].boolVal = pVal;
  }

  public static void setOptionInt(string pName, int pVal) => PlayerConfig.dict[pName].intVal = pVal;

  public static void setOptionString(string pName, string pVal)
  {
    PlayerConfig.dict[pName].stringVal = pVal;
  }

  [Obsolete]
  internal static void switchOption(string gameOptionName, OptionType pType)
  {
    foreach (PlayerOptionData playerOptionData in PlayerConfig.instance.data.list)
    {
      if (!(playerOptionData.name != gameOptionName) && pType == OptionType.Bool)
        playerOptionData.boolVal = !playerOptionData.boolVal;
    }
    PlayerConfig.checkSettings();
  }

  public static void setVsync(bool vsyncEnabled)
  {
    if (vsyncEnabled)
    {
      Resolution currentResolution1 = Screen.currentResolution;
      RefreshRate refreshRateRatio1 = ((Resolution) ref currentResolution1).refreshRateRatio;
      if (((RefreshRate) ref refreshRateRatio1).value < 61.0)
      {
        QualitySettings.vSyncCount = 1;
      }
      else
      {
        Resolution currentResolution2 = Screen.currentResolution;
        RefreshRate refreshRateRatio2 = ((Resolution) ref currentResolution2).refreshRateRatio;
        if (((RefreshRate) ref refreshRateRatio2).value < 121.0)
        {
          QualitySettings.vSyncCount = 2;
        }
        else
        {
          Resolution currentResolution3 = Screen.currentResolution;
          RefreshRate refreshRateRatio3 = ((Resolution) ref currentResolution3).refreshRateRatio;
          if (((RefreshRate) ref refreshRateRatio3).value < 181.0)
            QualitySettings.vSyncCount = 3;
          else
            QualitySettings.vSyncCount = 4;
        }
      }
    }
    else
    {
      QualitySettings.vSyncCount = 0;
      if (Config.fps_lock_30)
      {
        if (Application.targetFrameRate == 30)
          return;
        Application.targetFrameRate = 30;
      }
      else
      {
        if (Application.targetFrameRate == 60)
          return;
        Application.targetFrameRate = 60;
      }
    }
  }

  public static void turnOffAssetsPreloading()
  {
    PlayerConfig.setOptionBool("preload_units", false);
    PlayerConfig.setOptionBool("preload_buildings", false);
    PlayerConfig.setOptionBool("preload_quantum_sprites", false);
    PlayerConfig.setOptionBool("preload_windows", false);
  }

  internal static void checkSettings()
  {
    if (SystemInfo.systemMemorySize < 3000 && !PlayerConfig._memory_check_done)
    {
      PlayerConfig._memory_check_done = true;
      Debug.Log((object) ("[RAM is MEH] SystemInfo.systemMemorySize: " + SystemInfo.systemMemorySize.ToString()));
      PlayerConfig.turnOffAssetsPreloading();
    }
    foreach (OptionAsset pAsset in AssetManager.options_library.list)
    {
      if (!pAsset.computer_only || Config.isComputer)
      {
        if (pAsset.reset_to_default_on_launch)
        {
          PlayerConfig.setOptionBool(pAsset.id, pAsset.default_bool);
          PlayerConfig.setOptionInt(pAsset.id, pAsset.default_int);
          PlayerConfig.setOptionString(pAsset.id, pAsset.default_string);
        }
        ActionOptionAsset action = pAsset.action;
        if (action != null)
          action(pAsset);
      }
    }
  }

  public static int countRewards()
  {
    return PlayerConfig.instance?.data?.rewardedPowers != null ? PlayerConfig.instance.data.rewardedPowers.Count : 0;
  }

  public static void clearRewards() => PlayerConfig.instance?.data?.rewardedPowers?.Clear();

  private bool moveTraitsAndAchievements()
  {
    bool flag = false;
    List<string> achievements = this.data.achievements;
    // ISSUE: explicit non-virtual call
    if ((achievements != null ? (__nonvirtual (achievements.Count) > 0 ? 1 : 0) : 0) != 0)
    {
      foreach (string achievement in this.data.achievements)
        GameProgress.unlockAchievement(achievement);
      this.data.achievements.Clear();
      flag = true;
    }
    List<string> unlockedTraits = this.data.unlocked_traits;
    // ISSUE: explicit non-virtual call
    if ((unlockedTraits != null ? (__nonvirtual (unlockedTraits.Count) > 0 ? 1 : 0) : 0) != 0)
    {
      foreach (string unlockedTrait in this.data.unlocked_traits)
        AssetManager.traits.get(unlockedTrait)?.unlock();
      this.data.unlocked_traits.Clear();
      flag = true;
    }
    return flag;
  }

  private bool handleDebugOptions()
  {
    bool flag = false;
    if (this.data.clearDebugOnStart)
    {
      DebugConfig.setOption(DebugOption.DisablePremium, false);
      DebugConfig.setOption(DebugOption.TestAds, false);
      this.data.clearDebugOnStart = false;
      this.data.premiumDisabled = false;
      this.data.testAds = false;
      flag = true;
    }
    else
    {
      if (this.data.premiumDisabled)
      {
        DebugConfig.setOption(DebugOption.DisablePremium, true);
        this.data.clearDebugOnStart = true;
        flag = true;
      }
      if (this.data.testAds)
      {
        DebugConfig.setOption(DebugOption.TestAds, true);
        this.data.clearDebugOnStart = true;
        flag = true;
      }
    }
    return flag;
  }
}
