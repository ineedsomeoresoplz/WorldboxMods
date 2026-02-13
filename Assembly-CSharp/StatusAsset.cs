// Decompiled with JetBrains decompiler
// Type: StatusAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class StatusAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  public WorldAction action_finish;
  public WorldAction action_death;
  public WorldAction action;
  public GetHitAction action_get_hit;
  public WorldAction action_on_receive;
  public float action_interval;
  [DefaultValue(StatusTier.Basic)]
  public StatusTier tier = StatusTier.Basic;
  public bool can_be_cured;
  [DefaultValue(10f)]
  public float duration = 10f;
  [DefaultValue(true)]
  public bool allow_timer_reset = true;
  public string texture;
  public bool random_frame;
  [DefaultValue(true)]
  public bool can_be_flipped = true;
  public bool animated;
  public bool is_animated_in_pause;
  [DefaultValue(true)]
  public bool loop = true;
  [DefaultValue(0.1f)]
  public float animation_speed = 0.1f;
  public float animation_speed_random;
  [DefaultValue(1f)]
  public float scale = 1f;
  public float offset_x;
  public float offset_x_ui;
  public float offset_y;
  public float offset_y_ui;
  public float rotation_z;
  [DefaultValue(true)]
  public bool use_parent_rotation = true;
  public bool removed_on_damage;
  [DefaultValue(0.01f)]
  public float position_z = 0.01f;
  public bool random_flip;
  public bool cancel_actor_job;
  public bool affects_mind;
  [DefaultValue("mat_world_object")]
  public string material_id = "mat_world_object";
  [NonSerialized]
  public Material material;
  public bool draw_light_area;
  [DefaultValue(0.2f)]
  public float draw_light_size = 0.2f;
  public BaseStats base_stats = new BaseStats();
  public string[] opposite_traits;
  public string[] opposite_tags;
  public string[] opposite_status;
  public string[] remove_status;
  [NonSerialized]
  public Sprite[] sprite_list;
  public string path_icon;
  public Sprite cached_sprite;
  public int render_priority;
  public string sound_idle;
  public string locale_id;
  public string locale_description;
  public GetEffectSprite get_override_sprite;
  public GetEffectSpriteUI get_override_sprite_ui;
  [NonSerialized]
  public bool has_override_sprite;
  public GetEffectSpritePosition get_override_sprite_position;
  public GetEffectSpritePositionUI get_override_sprite_position_ui;
  [NonSerialized]
  public bool has_override_sprite_position;
  public GetEffectSpriteRotationZ get_override_sprite_rotation_z;
  public GetEffectSpriteRotationZUI get_override_sprite_rotation_z_ui;
  [NonSerialized]
  public bool has_override_sprite_rotation_z;
  [NonSerialized]
  public GetEffectSpriteCount get_sprites_count = (GetEffectSpriteCount) ((_, pEffect) => pEffect == null ? 0 : pEffect.sprite_list.Length);
  public RenderEffectCheck render_check = new RenderEffectCheck(StatusAsset.defaultRenderCheck);
  public string decision_id;
  [NonSerialized]
  public bool need_visual_render;

  [JsonIgnore]
  public bool has_sound_idle => this.sound_idle != null;

  public Sprite getSprite()
  {
    if (Object.op_Equality((Object) this.cached_sprite, (Object) null))
      this.cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this.cached_sprite;
  }

  private static bool defaultRenderCheck(ActorAsset pAsset) => true;

  public Material getMaterial() => LibraryMaterials.instance.dict[this.material_id];

  public DecisionAsset getDecisionAsset() => AssetManager.decisions_library.get(this.decision_id);

  public string getLocaleID() => this.locale_id;

  public string getDescriptionID() => this.locale_description;
}
