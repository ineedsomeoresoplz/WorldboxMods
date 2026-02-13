// Decompiled with JetBrains decompiler
// Type: DebugConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DebugConfig : MonoBehaviour
{
  public GameObject debugButton;
  public static DebugConfig instance;
  private static Dictionary<DebugOption, bool> _dictionary;
  private static int _pos_x = 20;
  public static List<string> default_debug_tools = new List<string>();

  private void Start()
  {
    DebugConfig.instance = this;
    if (!DebugConfig.isOn(DebugOption.DisablePremium) && !DebugConfig.isOn(DebugOption.TestAds))
      return;
    this.debugButton.SetActive(true);
    if (DebugConfig.isOn(DebugOption.DisablePremium))
    {
      Debug.LogError((object) "[DEBUG] Premium is disabled via debug menu!");
      Debug.Log((object) "This option is only for beta testers. If you purchased premium, you won't have it until you disable it.");
      Debug.Log((object) "Disable this option in the debug menu and restart the game.");
    }
    if (!DebugConfig.isOn(DebugOption.TestAds))
      return;
    Debug.LogError((object) "[DEBUG] Test Ads are enabled via debug menu!");
    Debug.Log((object) "This option is only for beta testers. You most likely don't want to play with it.");
    Debug.Log((object) "Disable this option in the debug menu and restart the game.");
  }

  private static void enableDefaultOptions()
  {
    DebugConfig.setOption(DebugOption.GenerateNewMapOnMapLoadingError, true);
    DebugConfig.setOption(DebugOption.LavaGlow, true);
    DebugConfig.setOption(DebugOption.UseCameraAspect, true);
    DebugConfig.setOption(DebugOption.UseGlobalPathLock, true);
    DebugConfig.setOption(DebugOption.SystemBuildTick, true);
    DebugConfig.setOption(DebugOption.SystemCityPlaceFinder, true);
    DebugConfig.setOption(DebugOption.SystemProduceNewCitizens, true);
    DebugConfig.setOption(DebugOption.SystemUnitPathfinding, true);
    DebugConfig.setOption(DebugOption.SystemWorldBehaviours, true);
    DebugConfig.setOption(DebugOption.SystemZoneGrowth, true);
    DebugConfig.setOption(DebugOption.SystemCheckUnitAction, true);
    DebugConfig.setOption(DebugOption.SystemUpdateUnitAnimation, true);
    DebugConfig.setOption(DebugOption.SystemUpdateBuildings, true);
    DebugConfig.setOption(DebugOption.SystemUpdateUnits, true);
    DebugConfig.setOption(DebugOption.SystemUpdateCities, true);
    DebugConfig.setOption(DebugOption.SystemRedrawMap, true);
    DebugConfig.setOption(DebugOption.SystemUpdateDirtyChunks, true);
    DebugConfig.setOption(DebugOption.SystemCheckGoodForBoat, false);
    DebugConfig.setOption(DebugOption.SystemCityTasks, true);
    DebugConfig.setOption(DebugOption.InspectObjectsOnClick, true);
    DebugConfig.setOption(DebugOption.SystemMusic, false);
    DebugConfig.setOption(DebugOption.Greg, false);
    DebugConfig.setOption(DebugOption.MakeUnitsFollowCursor, false);
    DebugConfig.setOption(DebugOption.SystemSplitAstar, true);
    DebugConfig.setOption(DebugOption.UseCacheForRegionPath, true);
    DebugConfig.setOption(DebugOption.ParallelJobsUpdater, true);
    DebugConfig.setOption(DebugOption.ParallelChunks, true);
    DebugConfig.setOption(DebugOption.ScaleEffectEnabled, true);
    if (Config.editor_devs && !Config.editor_nikon)
    {
      DebugConfig.setOption(DebugOption.ExportAssetLibraries, true);
      DebugConfig.setOption(DebugOption.GenerateGameplayReport, true);
      DebugConfig.setOption(DebugOption.TesterLibs, true);
    }
    if (!Config.isEditor)
      return;
    DebugConfig.setOption(DebugOption.DebugButton, true);
  }

  public static void initDict()
  {
    DebugConfig._dictionary = new Dictionary<DebugOption, bool>();
    foreach (DebugOption pOption in Enum.GetValues(typeof (DebugOption)))
      DebugConfig.setOption(pOption, false);
  }

  public static void init()
  {
    if (DebugConfig._dictionary != null)
      return;
    DebugConfig.initDict();
    DebugConfig.enableDefaultOptions();
    if (PlayerConfig.instance != null)
    {
      if (PlayerConfig.instance.data.premiumDisabled)
        DebugConfig.setOption(DebugOption.DisablePremium, true);
      if (PlayerConfig.instance.data.testAds)
        DebugConfig.setOption(DebugOption.TestAds, true);
    }
    if (Config.fmod_test_build)
    {
      Config.disable_discord = true;
      Config.disable_steam = true;
    }
    if (Config.isEditor)
    {
      Config.show_console_on_error = false;
      Config.customMapSizeDefault = "standard";
      DebugConfig.setOption(DebugOption.Graphy, true);
    }
    if (Config.editor_maxim)
      DebugConfig.editorMaximOptions();
    if (Config.editor_mastef)
      DebugConfig.editorMastefOptions();
    if (!Config.editor_nikon)
      return;
    DebugConfig.editorNikonOptions();
  }

  private static void editorNikonOptions()
  {
    Config.show_console_on_error = false;
    Config.disable_discord = true;
    Config.disable_steam = true;
    Config.load_save_on_start = false;
    Config.load_save_on_start_slot = 8;
    Config.disable_startup_window = false;
    Config.disable_tutorial = true;
    MusicBox.debug_sounds = false;
    DebugConfig.setOption(DebugOption.DebugButton, true);
    DebugConfig.setOption(DebugOption.Graphy, false);
    DebugConfig.setOption(DebugOption.PauseOnStart, true);
    DebugConfig.setOption(DebugOption.ShowHiddenStats, true);
    DebugConfig.setOption(DebugOption.DebugWindowHotkeys, true);
    DebugConfig.setOption(DebugOption.DebugUnitHotkeys, true);
  }

  internal static void debugToolMastefDefaults(DebugTool pTool)
  {
    pTool.sort_order_reversed = false;
    pTool.sort_by_values = true;
    pTool.show_averages = false;
    pTool.hide_zeroes = false;
    pTool.show_counter = true;
    pTool.show_max = false;
    pTool.paused = false;
    pTool.hide_zeroes = true;
    pTool.state = DebugToolState.Values;
  }

  private static void editorMastefOptions()
  {
    Plot.DEBUG_PLOTS = false;
    Config.show_console_on_error = false;
    Config.disable_loading_logs = true;
    Config.disable_discord = true;
    Config.disable_steam = true;
    MusicBox.debug_sounds = false;
    Config.disable_startup_window = true;
    Config.disable_dispose_logs = true;
    DebugConfig.setOption(DebugOption.OverlaySoundsActive, false);
    DebugConfig.setOption(DebugOption.ShowLayoutGroupGrid, false);
    DebugConfig.setOption(DebugOption.BenchAiEnabled, false);
    DebugConfig.setOption(DebugOption.DebugTooltipUI, false);
    DebugConfig.setOption(DebugOption.ParallelJobsUpdater, false);
    DebugConfig.setOption(DebugOption.ParallelChunks, false);
    DebugConfig.setOption(DebugOption.SonicSpeed, false);
    DebugConfig.setOption(DebugOption.Graphy, false);
    MapBox.on_world_loaded += (Action) (() => { });
  }

  private static void debugBoats()
  {
    DebugConfig.setOption(DebugOption.OverlayBoatTransport, true);
    DebugConfig.setOption(DebugOption.BoatPassengerLines, true);
    DebugConfig.setOption(DebugOption.ActorGizmosBoatTaxiRequestTargets, true);
    DebugConfig.setOption(DebugOption.ActorGizmosBoatTaxiTarget, true);
    DebugConfig.setOption(DebugOption.RenderIslandsTileCorners, true);
    DebugConfig.setOption(DebugOption.RegionNeighbours, true);
    DebugConfig.setOption(DebugOption.Region, true);
    DebugConfig.addDebugTool("boat");
    DebugConfig.addDebugTool("taxi");
    DebugConfig.addDebugTool("tile_info");
  }

  private static void debugReproduction()
  {
    DebugConfig.addDebugTool("reproduction_diagnostic_cursor");
    DebugConfig.addDebugTool("reproduction_diagnostic_total");
    DebugConfig.addDebugTool("city_jobs");
    DebugConfig.addDebugTool("city_storage");
  }

  private static void debugMapChunks()
  {
    DebugConfig.addDebugTool("map_chunks");
    DebugConfig.addDebugTool("benchmark_chunks");
  }

  private static void editorMaximOptions()
  {
    DebugConfig.setOption(DebugOption.GenerateNewMapOnMapLoadingError, false);
    Config.disable_discord = true;
    Config.disable_steam = true;
    Config.disable_startup_window = true;
    Config.show_console_on_error = false;
    Config.disable_tutorial = true;
    DebugConfig.setOption(DebugOption.ControlledUnitsAttackRaycast, true);
    DebugConfig.setOption(DebugOption.DebugWindowHotkeys, true);
    DebugConfig.setOption(DebugOption.CivDrawCityClaimZone, true);
    DebugConfig.setOption(DebugOption.PauseOnStart, true);
    DebugConfig.setOption(DebugOption.ShowHiddenStats, true);
    DebugConfig.setOption(DebugOption.DebugUnitHotkeys, true);
    DebugConfig.setOption(DebugOption.ShowWarriorsCityText, true);
    DebugConfig.setOption(DebugOption.ShowCityWeaponsText, true);
    DebugConfig.setOption(DebugOption.ShowFoodCityText, true);
    DebugConfig.setOption(DebugOption.CursorUnitAttackRange, true);
    DebugConfig.setOption(DebugOption.ShowAmountNearArmy, true);
    for (int index = 0; index < 100; ++index)
      Debug.Log((object) "remember the cant");
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isOn(DebugOption pOption) => DebugConfig._dictionary[pOption];

  public static void switchOption(DebugOption pOption)
  {
    DebugConfig.setOption(pOption, !DebugConfig.isOn(pOption));
    if (pOption == DebugOption.DisablePremium && PlayerConfig.instance != null)
    {
      PlayerConfig.instance.data.premiumDisabled = DebugConfig.isOn(pOption);
      PlayerConfig.instance.data.clearDebugOnStart = false;
      PlayerConfig.saveData();
      PremiumElementsChecker.checkElements();
      DebugConfig.disablePremiumNotify();
    }
    if (pOption != DebugOption.TestAds || PlayerConfig.instance == null)
      return;
    PlayerConfig.instance.data.testAds = DebugConfig.isOn(pOption);
    PlayerConfig.instance.data.clearDebugOnStart = false;
    PlayerConfig.saveData();
    PremiumElementsChecker.checkElements();
    DebugConfig.testAdsNotify();
  }

  public static void disablePremiumNotify()
  {
    if (PlayerConfig.instance == null)
      return;
    if (DebugConfig.isOn(DebugOption.DisablePremium))
      WorldTip.showNow("Premium is blocked! Even after restart!", false, "top", 10f);
    else
      WorldTip.showNow("Premium is unblocked", false, "top");
  }

  public static void testAdsNotify()
  {
    if (PlayerConfig.instance == null)
      return;
    if (DebugConfig.isOn(DebugOption.TestAds))
      WorldTip.showNow("Test Ads are enabled! Even after restart!", false, "top", 10f);
    else
      WorldTip.showNow("Test Ads disabled", false, "top");
  }

  public static void setOption(DebugOption pOption, bool pVal, bool pUpdateSpecialSettings = true)
  {
    if (DebugConfig._dictionary == null)
      DebugConfig.init();
    DebugConfig._dictionary[pOption] = pVal;
    if (!pUpdateSpecialSettings)
      return;
    if (pOption == DebugOption.Graphy)
      DebugConfig.checkGraphy();
    if (pOption != DebugOption.SonicSpeed || !Config.game_loaded)
      return;
    if (pVal)
      Config.setWorldSpeed("x40", false);
    else
      Config.setWorldSpeed("x1", false);
  }

  public static void checkSonicTimeScales()
  {
    if (DebugConfig.isOn(DebugOption.SonicSpeed))
      Config.setWorldSpeed("x40", false);
    else
      Config.setWorldSpeed("x1", false);
  }

  public static void checkGraphy()
  {
    if (!Object.op_Inequality((Object) PrefabLibrary.instance, (Object) null))
      return;
    PrefabLibrary.instance.graphy.gameObject.SetActive(DebugConfig.isOn(DebugOption.Graphy));
  }

  public static void createTool(string pID, int pX = 80 /*0x50*/, int pY = -10, int pWidth = -1)
  {
    Bench.bench_enabled = true;
    DebugToolAsset pAsset = AssetManager.debug_tool_library.get(pID);
    DebugTool debugTool = Object.Instantiate<DebugTool>(PrefabLibrary.instance.debugTool, ((Component) DebugConfig.instance).transform);
    debugTool.setAsset(pAsset);
    debugTool.populateOptions();
    for (int index = 0; index < debugTool.dropdown.options.Count; ++index)
    {
      if (debugTool.dropdown.options[index].text == pID)
      {
        debugTool.dropdown.value = index;
        debugTool.dropdown.captionText.text = pID;
        break;
      }
    }
    ((Component) debugTool).transform.localPosition = new Vector3((float) pX, (float) pY);
    DebugConfig._pos_x += 128 /*0x80*/;
  }

  public static bool debug_enabled => DebugConfig.instance.debugButton.gameObject.activeSelf;

  private static void addDebugTool(string pID) => DebugConfig.default_debug_tools.Add(pID);
}
