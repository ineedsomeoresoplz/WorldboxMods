// Decompiled with JetBrains decompiler
// Type: EffectAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class EffectAsset : Asset
{
  public string prefab_id;
  public bool use_basic_prefab;
  public bool load_texture;
  public string sprite_path;
  public string sorting_layer_id;
  [DefaultValue(0.1f)]
  public float time_between_frames = 0.1f;
  public int limit;
  public bool limit_unload;
  public string sound_launch;
  public string sound_loop_idle;
  public bool show_on_mini_map;
  public EffectAction spawn_action;
  public double cooldown_interval;
  private double _cooldown;
  public bool draw_light_area;
  public float draw_light_area_offset_x;
  public float draw_light_area_offset_y;
  [DefaultValue(0.5f)]
  public float draw_light_size = 0.5f;

  [JsonIgnore]
  public bool has_sound_launch => this.sound_launch != null;

  [JsonIgnore]
  public bool has_sound_loop_idle => this.sound_loop_idle != null;

  public bool checkIsUnderCooldown()
  {
    double curSessionTime = World.world.getCurSessionTime();
    if (curSessionTime - this._cooldown < this.cooldown_interval)
      return true;
    this._cooldown = curSessionTime;
    return false;
  }
}
