// Decompiled with JetBrains decompiler
// Type: TrailerModeSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

#nullable disable
[Serializable]
public class TrailerModeSettings
{
  public bool cityUseResources = true;
  public bool sonicSpeed = true;
  public bool fastSpawn = true;
  public float cameraMoveSpeed = 1f / 1000f;
  public float cameraMoveMax = 0.02f;
  public float cameraZoomSpeed = 3.8f;
  public bool superOrcs = true;

  public static void startEvent()
  {
    string path = Application.persistentDataPath + "/trailer_settings";
    TrailerModeSettings trailerModeSettings;
    if (!File.Exists(path))
    {
      trailerModeSettings = new TrailerModeSettings();
      string contents = JsonUtility.ToJson((object) trailerModeSettings).Replace(",", ",\n").Replace("{", "{\n").Replace("}", "\n}");
      File.WriteAllText(path, contents);
    }
    else
      trailerModeSettings = JsonUtility.FromJson<TrailerModeSettings>(File.ReadAllText(path));
    trailerModeSettings.applyTrailerSettings();
  }

  public void applyTrailerSettings()
  {
    if (this.superOrcs)
      AssetManager.actor_library.get("unit_orc").base_stats["damage"] = 10000f;
    else
      AssetManager.actor_library.get("unit_orc").base_stats["damage"] = 18f;
    DebugConfig.setOption(DebugOption.FastSpawn, this.fastSpawn);
    DebugConfig.setOption(DebugOption.SonicSpeed, this.sonicSpeed);
    World.world.move_camera.camera_move_speed = this.cameraMoveSpeed;
    World.world.move_camera.camera_move_max = this.cameraMoveMax;
    World.world.move_camera.camera_zoom_speed = this.cameraZoomSpeed;
    Globals.TRAILER_MODE_USE_RESOURCES = this.cityUseResources;
  }
}
