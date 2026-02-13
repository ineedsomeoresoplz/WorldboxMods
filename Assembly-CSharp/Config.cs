// Decompiled with JetBrains decompiler
// Type: Config
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
public class Config
{
  public static bool parallel_jobs_updater = true;
  public static bool parallel_chunk_manager = true;
  public static string versionCodeText = string.Empty;
  public static string gitCodeText = string.Empty;
  public static string versionCodeDate = string.Empty;
  public static string iname = string.Empty;
  internal static bool? gen = new bool?();
  public static string testStreamingAssets = "test";
  public static bool ui_main_hidden = false;
  public static int WORLD_SAVE_VERSION = 17;
  public static string current_map_template = "continent";
  public static string customMapSize = "standard";
  public static int customZoneX = 0;
  public static int customZoneY = 0;
  public static int customPerlinScale = 10;
  public static int customRandomShapes = 10;
  public static int customWaterLevel = 10;
  public static int ZONE_AMOUNT_X = 4;
  public static int ZONE_AMOUNT_Y = 4;
  public static string customMapSizeDefault = "standard";
  public static string maxMapSize = "iceberg";
  public static int ZONE_AMOUNT_X_DEFAULT = 3;
  public static int ZONE_AMOUNT_Y_DEFAULT = 4;
  public const int MAP_BLOCK_SIZE = 64 /*0x40*/;
  public const int CHUNK_SIZE = 16 /*0x10*/;
  public const int TILES_IN_CHUNK = 256 /*0x0100*/;
  public const int CITY_ZONE_SIZE = 8;
  public const int CITY_ZONE_TILES = 64 /*0x40*/;
  public const int TILES_IN_REGION = 256 /*0x0100*/;
  public const int PREVIEW_MAP_SIZE = 512 /*0x0200*/;
  public const float FOCUS_SCROLL_DELAY_PC = 0.4f;
  public const float FOCUS_SCROLL_DELAY_PHONE = 0.55f;
  public static WorldTimeScaleAsset time_scale_asset = (WorldTimeScaleAsset) null;
  public static bool fps_lock_30 = false;
  public static bool MODDED = false;
  public static bool EVERYTHING_MAGIC_COLOR = false;
  public static bool EVERYTHING_FIREWORKS = false;
  private static bool _paused = false;
  public static bool lockGameControls = false;
  internal static string steam_name;
  internal static string steam_id;
  internal static bool steam_language_allow_detect = true;
  internal static string discordId;
  internal static string discordName;
  internal static string discordDiscriminator;
  public static bool testAds = false;
  public static bool firebaseInitiating = false;
  public static bool firebaseChecked = false;
  public static bool firebaseEnabled = true;
  public static bool authEnabled = false;
  public const string firebaseDatabaseURL = "https://worldbox-g.firebaseio.com/";
  public const string baseURL = "https://versions.superworldbox.com";
  public const string currencyURL = "https://currency.superworldbox.com";
  public static bool adsInitialized = false;
  public static bool disable_dispose_logs = true;
  public static bool disable_loading_logs = false;
  public static bool disable_discord = false;
  public static bool disable_steam = false;
  public static bool disable_db = false;
  public static bool disable_startup_window = false;
  public static bool disable_tutorial = false;
  public static bool debug_log_meta_ranks = false;
  public static string debug_last_selected_power_button;
  public static string debug_last_window;
  public static int debug_worlds_loaded;
  public static WindowStats debug_window_stats;
  public static bool load_random_test_map = false;
  public static bool load_new_map = false;
  public static bool load_dragon = false;
  public static bool load_save_on_start = false;
  public static bool load_save_from_path = false;
  public static string load_test_save_path = "";
  public static bool load_test_map = false;
  public static int load_save_on_start_slot = 1;
  public static string auto_test_on_start = (string) null;
  public static float LOAD_TIME_INIT = 0.0f;
  public static float LOAD_TIME_CREATE = 0.0f;
  public static float LOAD_TIME_GENERATE = 0.0f;
  public static float LAST_LOAD_TIME = 0.0f;
  public static bool editor_test_rewards_from_ads = false;
  private static bool _hpr = false;
  public static bool sprite_animations_on = true;
  public static bool shadows_active = false;
  public static bool tooltips_active = true;
  public static bool preload_windows = true;
  public static bool preload_quantum_sprites = true;
  public static bool preload_buildings = true;
  public static bool preload_units = true;
  public static bool autosaves = true;
  public static bool graphs = true;
  public static bool experimental_mode = false;
  public static bool wbb_confirmed = false;
  public static bool full_screen = true;
  public static bool firebase_available = false;
  public static bool upload_available = false;
  public static bool game_loaded = false;
  public static bool show_console_on_start = false;
  public static bool show_console_on_error = true;
  public static bool editor_maxim = false;
  public static bool editor_mastef = false;
  public static bool editor_nikon = false;
  public static bool editor_devs = false;
  public static bool fmod_test_build = false;
  public static bool isEditor = false;
  public static bool isMobile = false;
  public static bool isIos = false;
  public static bool isAndroid = false;
  public static bool isComputer = true;
  public static bool grey_goo_damaged = false;
  public static GodPower power_to_unlock;
  private static string _current_brush = "circ_5";
  public static string selected_trait_editor = string.Empty;
  public static readonly SelectedObjectsGraph selected_objects_graph = new SelectedObjectsGraph();
  private static bool _dragging_item = false;
  public static IDraggable dragging_item_object = (IDraggable) null;
  public static Kingdom whisper_A;
  public static Kingdom whisper_B;
  public static Kingdom unity_A;
  public static Kingdom unity_B;
  private static float timer = 30f;
  private static bool skip = false;
  private static List<string> _loggedSelectedPowers = new List<string>();
  private static bool _scheduledGC = false;
  private static bool _scheduledGCUnload = false;
  public static string gv;

  public static bool worldLoading => SmoothLoader.isLoading();

  public static bool paused
  {
    get => Config._paused;
    set => Config._paused = value;
  }

  public static bool hasPremium
  {
    set => Config._hpr = value;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return !Config.editor_test_rewards_from_ads && Config._hpr;
    }
  }

  public static string current_brush
  {
    get => Config._current_brush;
    set
    {
      Config._current_brush = value;
      Config.current_brush_data = Brush.get(value);
    }
  }

  public static BrushData current_brush_data { get; private set; }

  public static void setDraggingObject(IDraggable pGameObject)
  {
    Config.dragging_item_object = pGameObject;
    Config._dragging_item = true;
  }

  public static bool isDraggingObject(IDraggable pGameObject)
  {
    return Config.dragging_item_object != null && Config.dragging_item_object == pGameObject;
  }

  public static IDraggable getDraggingObject() => Config.dragging_item_object;

  public static void clearDraggingObject()
  {
    Config._dragging_item = false;
    Config.dragging_item_object = (IDraggable) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isDraggingItem() => Config._dragging_item;

  public static bool joyControls => false;

  public static void setWorldSpeed(WorldTimeScaleAsset pAsset, bool pUpdateDebug = true)
  {
    if (!pAsset.sonic & pUpdateDebug)
      DebugConfig.setOption(DebugOption.SonicSpeed, false, false);
    Config.time_scale_asset = pAsset;
  }

  public static void setWorldSpeed(string pID, bool pUpdateDebug = true)
  {
    Config.setWorldSpeed(AssetManager.time_scales.get(pID), pUpdateDebug);
  }

  public static void nextWorldSpeed(bool pCycle = false)
  {
    Config.setWorldSpeed(Config.time_scale_asset.getNext(pCycle));
  }

  public static void prevWorldSpeed()
  {
    Config.setWorldSpeed(Config.time_scale_asset.getPrevious());
  }

  public static void setPortrait(bool pValue)
  {
    if (pValue)
    {
      Screen.autorotateToPortrait = true;
      Screen.autorotateToPortraitUpsideDown = true;
      Screen.autorotateToLandscapeLeft = false;
      Screen.autorotateToLandscapeRight = false;
      Screen.orientation = Input.deviceOrientation != 2 ? (ScreenOrientation) 1 : (ScreenOrientation) 2;
      Screen.orientation = (ScreenOrientation) 5;
      World.world.camera.ResetAspect();
    }
    else
    {
      Screen.autorotateToPortrait = false;
      Screen.autorotateToPortraitUpsideDown = false;
      Screen.autorotateToLandscapeLeft = true;
      Screen.autorotateToLandscapeRight = true;
      Screen.orientation = Input.deviceOrientation != 4 ? (ScreenOrientation) 3 : (ScreenOrientation) 4;
      Screen.orientation = (ScreenOrientation) 5;
      World.world.camera.ResetAspect();
    }
  }

  public static void setAutorotation(bool pValue)
  {
    OptionAsset optionAsset = AssetManager.options_library.get("portrait");
    if (pValue)
    {
      Screen.autorotateToPortrait = true;
      Screen.autorotateToPortraitUpsideDown = true;
      Screen.autorotateToLandscapeLeft = true;
      Screen.autorotateToLandscapeRight = true;
      ScreenOrientation screenOrientation;
      switch (Input.deviceOrientation - 1)
      {
        case 0:
          screenOrientation = (ScreenOrientation) 1;
          break;
        case 1:
          screenOrientation = (ScreenOrientation) 2;
          break;
        case 2:
          screenOrientation = (ScreenOrientation) 3;
          break;
        case 3:
          screenOrientation = (ScreenOrientation) 4;
          break;
        default:
          screenOrientation = (ScreenOrientation) 1;
          break;
      }
      Screen.orientation = screenOrientation;
      Screen.orientation = (ScreenOrientation) 5;
      optionAsset.interactable = false;
      World.world.camera.ResetAspect();
    }
    else
    {
      optionAsset.interactable = true;
      Config.setPortrait(PlayerConfig.optionBoolEnabled("portrait"));
    }
  }

  public static void enableAutoRotation(bool pValue)
  {
  }

  public static bool skipCrashMetadata()
  {
    if (Config.MODDED || Config.experimental_mode)
      return true;
    bool? gen = Config.gen;
    return gen.HasValue && !gen.GetValueOrDefault();
  }

  [Skip]
  [SkipRename]
  [DoNotFake]
  public static void updateCrashMetadata()
  {
    if (!Config.game_loaded || SmoothLoader.isLoading() || Config.skip)
      return;
    if ((double) Config.timer > 0.0)
    {
      Config.timer -= Time.fixedDeltaTime;
    }
    else
    {
      Config.timer = 30f;
      if (Config.skipCrashMetadata())
      {
        Config.skip = true;
        UnityEngine.CrashReportHandler.CrashReportHandler.enableCaptureExceptions = false;
      }
      else
      {
        UnityEngine.CrashReportHandler.CrashReportHandler.enableCaptureExceptions = false;
        try
        {
          if (!string.IsNullOrEmpty(Config.versionCodeText))
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("u_versionCodeText", Config.versionCodeText);
          if (!string.IsNullOrEmpty(Config.gitCodeText))
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("u_gitCodeText", Config.gitCodeText);
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("c_MODDED", Config.MODDED.ToString());
          bool flag = Config.hasPremium;
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("c_HAVE_PREMIUM", flag.ToString());
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("c_game_speed", Config.time_scale_asset.id);
          flag = DebugConfig.isOn(DebugOption.SonicSpeed);
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("c_sonic_speed", flag.ToString());
          flag = Zones.showMapNames();
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("c_show_map_names", flag.ToString());
          if (DebugConfig.instance.debugButton.gameObject.activeSelf)
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("c_debug_button", "visible");
          flag = World.world.quality_changer.isLowRes();
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("o_camera_lowRes", flag.ToString());
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("o_selected_power", World.world.getSelectedPowerID());
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("c_map_mode", World.world.zone_calculator.getCurrentModeDebug().ToString());
          if (ScrollWindow.isWindowActive())
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("o_window_open", ScrollWindow.getCurrentWindow().screen_id);
          else
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("o_window_open", "false");
          string str = "";
          for (int index = 0; index < Config._loggedSelectedPowers.Count; ++index)
            str = $"{str}{Config._loggedSelectedPowers[index]},";
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("o_power_history", str);
          int num = World.world.units.Count;
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_units", num.ToString());
          num = World.world.buildings.Count;
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_buildings", num.ToString());
          num = World.world.kingdoms.Count;
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_civ_kingdoms", num.ToString());
          num = World.world.cultures.Count;
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_cultures", num.ToString());
          num = World.world.zone_calculator.zones.Count;
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_layers", num.ToString());
          num = World.world.map_chunk_manager.chunks.Length;
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_chunks", num.ToString());
          num = World.world.drop_manager.getActiveIndex();
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_drops_active", num.ToString());
          num = World.world.stack_effects.countActive();
          UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("map_stackEffects_active", num.ToString());
          try
          {
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("u_installMode", Application.installMode.ToString());
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("u_sandboxType", Application.sandboxType.ToString());
            UnityEngine.CrashReportHandler.CrashReportHandler.SetUserMetadata("g_activeTier", Graphics.activeTier.ToString());
          }
          catch (Exception ex)
          {
          }
        }
        catch (Exception ex)
        {
          Config.skip = true;
          Debug.LogError((object) ex);
          throw;
        }
        UnityEngine.CrashReportHandler.CrashReportHandler.enableCaptureExceptions = true;
      }
    }
  }

  public static void logSelectedPower(GodPower pPower)
  {
    if (Config._loggedSelectedPowers.Count > 5)
      Config._loggedSelectedPowers.RemoveAt(0);
    Config._loggedSelectedPowers.Add(pPower.id);
  }

  public static void scheduleGC(string pWhere, bool pUnloadResources = false)
  {
    Config._scheduledGC = true;
    if (!pUnloadResources)
      return;
    Config._scheduledGCUnload = true;
  }

  public static void checkGC()
  {
    if (!Config._scheduledGC)
      return;
    Config.forceGC("scheduled", Config._scheduledGCUnload);
  }

  public static void forceGC(string pWhere, bool pUnloadResources = false)
  {
    if (pUnloadResources)
    {
      Resources.UnloadUnusedAssets();
      GC.Collect(1, GCCollectionMode.Optimized, false);
    }
    else
      GC.Collect(0, GCCollectionMode.Optimized, false);
    Config._scheduledGC = false;
    Config._scheduledGCUnload = false;
  }

  public static float getScrollToGroupDelay() => !Config.isMobile ? 0.4f : 0.55f;

  public static string gs { get; } = "";

  public static void fireworksCheck(bool pEnabled)
  {
    Config.EVERYTHING_FIREWORKS = pEnabled;
    PlayerConfig.instance.data.fireworksCheck2025 = pEnabled;
    PlayerConfig.saveData();
  }

  public static void valCheck(bool pEnabled)
  {
    PlayerConfig.instance.data.valCheck2025 = pEnabled;
    PlayerConfig.saveData();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void pCheck(bool value)
  {
    PlayerConfig.instance.data.pPossible0507 = value;
    PlayerConfig.saveData();
  }

  public static void magicCheck(bool pEnabled)
  {
    Config.EVERYTHING_MAGIC_COLOR = pEnabled;
    PlayerConfig.instance.data.magicCheck2025 = pEnabled;
    PlayerConfig.saveData();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void givePremium() => InAppManager.activatePrem();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void removePremium()
  {
    Config.hasPremium = false;
    PlayerConfig.instance.data.premium = false;
    PlayerConfig.saveData();
    PremiumElementsChecker.checkElements();
  }
}
