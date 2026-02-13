// Decompiled with JetBrains decompiler
// Type: WorldAgeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class WorldAgeAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  [DefaultValue(35)]
  public int years_min = 35;
  [DefaultValue(55)]
  public int years_max = 55;
  [DefaultValue(0.2f)]
  public float era_effect_overlay_alpha = 0.2f;
  [DefaultValue(1f)]
  public float era_effect_light_alpha_game = 1f;
  [DefaultValue(1f)]
  public float era_effect_light_alpha_minimap = 1f;
  public bool overlay_darkness;
  public bool particles_snow;
  public bool particles_rain;
  public bool particles_magic;
  public bool particles_ash;
  public bool particles_sun;
  public bool global_freeze_world;
  public bool global_unfreeze_world;
  public bool global_unfreeze_world_mountains;
  public bool overlay_magic;
  public bool overlay_rain_darkness;
  public bool overlay_winter;
  public bool overlay_chaos;
  public bool overlay_moon;
  public bool overlay_sun;
  public bool overlay_ash;
  public bool overlay_night;
  public bool overlay_rain;
  public List<string> clouds;
  public HashSet<string> biomes;
  [DefaultValue(15f)]
  public float cloud_interval = 15f;
  [DefaultValue(1f)]
  public float range_weapons_multiplier = 1f;
  [DefaultValue(1)]
  public int temperature_damage_bonus = 1;
  public string[] conditions;
  [DefaultValue("")]
  public string force_next = string.Empty;
  [DefaultValue(true)]
  public bool flag_crops_grow = true;
  public bool era_disaster_snow_turns_babies_into_ice_ones;
  public bool era_disaster_fire_elemental_spawn_on_fire;
  public float fire_elemental_spawn_chance;
  public bool era_disaster_rage_brings_demons;
  public bool flag_light_age;
  public bool flag_chaos;
  public bool flag_winter;
  public bool flag_moon;
  public bool flag_night;
  public bool flag_light_damage;
  public string path_icon;
  public string path_background;
  [NonSerialized]
  private Sprite _cached_sprite;
  [NonSerialized]
  private Sprite _cached_background;
  public Color light_color = Toolbox.makeColor("#FFCE61");
  public Color title_color = Toolbox.makeColor("#FFFFFF");
  public int bonus_loyalty;
  public int bonus_opinion;
  public int bonus_biomes_growth;
  [DefaultValue(true)]
  public bool grow_vegetation = true;
  [DefaultValue(1f)]
  public float fire_spread_rate_bonus = 1f;
  [DefaultValue(1)]
  public int rate = 1;
  [DefaultValue(10f)]
  public float special_effect_interval = 10f;
  public List<int> default_slots = new List<int>();
  public bool link_default_slots;
  public WorldAgeAction special_effect_action;

  internal Color pie_selection_color => this.title_color;

  public Sprite getSprite()
  {
    if (Object.op_Equality((Object) this._cached_sprite, (Object) null))
      this._cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this._cached_sprite;
  }

  public Sprite getBackground()
  {
    if (Object.op_Equality((Object) this._cached_background, (Object) null))
      this._cached_background = SpriteTextureLoader.getSprite(this.path_background);
    return this._cached_background;
  }

  public string getLocaleID() => this.id + "_title";

  public string getDescriptionID() => this.id + "_description";
}
