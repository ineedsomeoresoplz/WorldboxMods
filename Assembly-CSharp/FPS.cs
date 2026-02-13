// Decompiled with JetBrains decompiler
// Type: FPS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class FPS
{
  private static float _delta_time;
  private static float _frame_count;
  private static float _update_rate = 3f;
  private static int _fps;
  private const int DEBUG_FPS_THRESHOLD = 25;
  private static List<int> _last_fps = new List<int>(5);

  public static int fps => FPS._fps;

  public static string getFPS()
  {
    if (FPS._fps >= 60)
      return $"<color=#75D53A>{FPS._fps.ToString()}</color>";
    return FPS._fps >= 30 ? $"<color=#F4E700>{FPS._fps.ToString()}</color>" : $"<color=#DB2920>{FPS._fps.ToString()}</color>";
  }

  public static void update()
  {
    FPS._delta_time += Time.unscaledDeltaTime;
    ++FPS._frame_count;
    if ((double) FPS._delta_time <= 1.0 / (double) FPS._update_rate)
      return;
    FPS._fps = (int) ((double) FPS._frame_count / (double) FPS._delta_time);
    FPS._delta_time = 0.0f;
    FPS._frame_count = 0.0f;
  }

  public static void debug_update()
  {
    if (!Config.game_loaded || SmoothLoader.isLoading() || FPS._last_fps.Count >= 5)
      return;
    if (FPS._fps >= 25)
      FPS._last_fps.Clear();
    FPS._last_fps.Add(FPS._fps);
    if (!Config.editor_mastef || FPS._last_fps.Count < 5)
      return;
    DebugConfig.createTool(AssetManager.debug_tool_library.get("Benchmark All").id, 30);
    DebugConfig.createTool(AssetManager.debug_tool_library.get("Benchmark Actors").id, 200);
  }
}
