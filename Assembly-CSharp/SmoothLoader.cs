// Decompiled with JetBrains decompiler
// Type: SmoothLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class SmoothLoader
{
  public const float DEFAULT_TIMER_VALUE = 0.001f;
  public const int MAX_LOG_LENGTH = 40;
  private static int _index = 0;
  private static float _current_timer = 0.0f;
  private static bool _skip_frame = false;
  private static int _added = 0;
  private static bool _started = false;
  private static bool _has_actions = false;
  private static List<MapLoaderContainer> _actions = new List<MapLoaderContainer>();
  public static string latest_called_id = string.Empty;
  public static string latest_time = string.Empty;
  private static MapLoaderContainer _last_action;
  private static bool _toggled_console = false;
  private static int _last_i = -1;

  public static void prepare()
  {
    SmoothLoader._actions.Clear();
    SmoothLoader._index = 0;
    SmoothLoader._started = false;
    SmoothLoader.finish();
    SmoothLoader.latest_called_id = string.Empty;
    SmoothLoader.latest_time = string.Empty;
    PlayerConfig.checkSettings();
    PreloadHelpers.init();
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      ScrollWindow.hideAllEvent(false);
      CanvasMain.instance.setMainUiEnabled(false);
    }), "Close windows", true, 0.01f);
    SmoothLoader.add((MapLoaderAction) (() => QuantumSpriteManager.hideAll()), "Quantum Clean", true, 0.01f);
  }

  public static void add(
    MapLoaderAction pAction,
    string pId,
    bool pSkipFrame = false,
    float pNewWaitTimerValue = 0.001f,
    bool pToEnd = false)
  {
    MapLoaderContainer mapLoaderContainer = new MapLoaderContainer(pAction, pId, pNewWaitTimerValue: pNewWaitTimerValue);
    SmoothLoader._has_actions = true;
    if (SmoothLoader._started && !pToEnd)
    {
      if (pSkipFrame)
        SmoothLoader._actions.Insert(SmoothLoader._index + 1 + SmoothLoader._added++, new MapLoaderContainer(new MapLoaderAction(SmoothLoader.skipFrame), "skipFrame", false));
      SmoothLoader._actions.Insert(SmoothLoader._index + 1 + SmoothLoader._added++, mapLoaderContainer);
    }
    else
    {
      if (pSkipFrame)
        SmoothLoader._actions.Add(new MapLoaderContainer(new MapLoaderAction(SmoothLoader.skipFrame), "skipFrame", false));
      SmoothLoader._actions.Add(mapLoaderContainer);
    }
  }

  public static void setWaitTimer()
  {
    float num = 1f / 1000f;
    if (SmoothLoader._last_action != null)
      num = SmoothLoader._last_action.new_timer_value;
    if ((double) num <= (double) SmoothLoader._current_timer)
      return;
    SmoothLoader._current_timer = num;
  }

  public static void skipFrame() => SmoothLoader._skip_frame = true;

  public static bool isLoading() => SmoothLoader._has_actions;

  private static void finish()
  {
    SmoothLoader._last_action = (MapLoaderContainer) null;
    SmoothLoader._has_actions = false;
  }

  public static void update(float pElapsed)
  {
    if (!SmoothLoader._has_actions)
      return;
    if ((double) SmoothLoader._current_timer > 0.0)
      SmoothLoader._current_timer -= pElapsed;
    else if (SmoothLoader._actions.Count == 0)
      SmoothLoader.finish();
    else if (SmoothLoader._last_i == SmoothLoader._index)
    {
      SmoothLoader.openConsole();
      SmoothLoader.finish();
      MapBox.instance.startTheGame(true);
    }
    else
    {
      SmoothLoader._last_i = SmoothLoader._index;
      SmoothLoader._skip_frame = false;
      float realtimeSinceStartup = Time.realtimeSinceStartup;
      SmoothLoader.doActions();
      while ((double) Time.realtimeSinceStartup - (double) realtimeSinceStartup < 0.10000000149011612 && !SmoothLoader._skip_frame && SmoothLoader._actions.Count != 0)
        SmoothLoader.doActions();
      SmoothLoader.setWaitTimer();
    }
  }

  private static void openConsole()
  {
    if (!Object.op_Inequality((Object) World.world, (Object) null) || !Object.op_Inequality((Object) World.world.console, (Object) null) || SmoothLoader._toggled_console)
      return;
    SmoothLoader._toggled_console = true;
    World.world.console.Show();
  }

  private static void doActions()
  {
    if (!SmoothLoader._started)
      Bench.bench(nameof (SmoothLoader), "loading", true);
    SmoothLoader._started = true;
    SmoothLoader._added = 0;
    MapLoaderContainer action1 = SmoothLoader._actions[SmoothLoader._index];
    MapLoaderAction action2 = action1.action;
    SmoothLoader._last_action = action1;
    Bench.bench(action1.id, "loading", true);
    action2();
    Bench.benchEnd(action1.id, "loading", pForce: true);
    SmoothLoader.setWaitTimer();
    SmoothLoader.checkDebugText();
    ++SmoothLoader._index;
    if (SmoothLoader._index <= SmoothLoader._actions.Count - 1)
      return;
    SmoothLoader._actions.Clear();
    double num = Bench.benchEnd(nameof (SmoothLoader), "loading", pForce: true);
    if (!SmoothLoader.logsEnabled)
      return;
    Debug.Log((object) $"{SmoothLoader.fillRight(SmoothLoader.fillLeft((++SmoothLoader._index).ToString(), 3, '0') + ": Loading finished", 40)} = {num.ToString("F4")}");
  }

  private static void checkDebugText()
  {
    if (SmoothLoader._last_action == null || !SmoothLoader._last_action.debug_log)
      return;
    SmoothLoader.latest_time = Bench.getBenchResultAsDouble(SmoothLoader._last_action.id, "loading", false).ToString("F4");
    SmoothLoader.latest_called_id = $"{SmoothLoader.fillLeft(SmoothLoader._index.ToString(), 3, '0')}: {SmoothLoader._last_action.id}";
    if (!SmoothLoader.logsEnabled)
      return;
    string str = $"{SmoothLoader.fillRight(SmoothLoader.latest_called_id, 40)} = {SmoothLoader.latest_time}";
    double benchResultAsDouble = Bench.getBenchResultAsDouble(SmoothLoader._last_action.id, "loading", false);
    if (benchResultAsDouble > 1.0)
      str = $"<color=red>{str}</color>";
    else if (benchResultAsDouble > 0.5)
      str = $"<color=yellow>{str}</color>";
    Debug.Log((object) str);
  }

  private static bool logsEnabled => !Config.disable_loading_logs;

  private static string fillLeft(string pString, int pSize = 1, char pFill = ' ')
  {
    return Toolbox.fillLeft(pString, pSize, pFill);
  }

  private static string fillRight(string pString, int pSize = 1, char pFill = ' ')
  {
    return Toolbox.fillRight(pString, pSize, pFill);
  }
}
