// Decompiled with JetBrains decompiler
// Type: GodPower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class GodPower : Asset, IDescriptionAsset, ILocalizedAsset
{
  internal static List<PowerButton> powers_rank_0 = new List<PowerButton>();
  internal static List<PowerButton> powers_rank_1 = new List<PowerButton>();
  internal static List<PowerButton> powers_rank_2 = new List<PowerButton>();
  internal static List<PowerButton> powers_rank_3 = new List<PowerButton>();
  internal static List<PowerButton> powers_rank_4 = new List<PowerButton>();
  internal static List<PowerButton> premium_buttons = new List<PowerButton>();
  internal static List<GodPower> premium_powers = new List<GodPower>();
  internal static Dictionary<string, GodPower> god_powers_on_canvas = new Dictionary<string, GodPower>();
  public string name = "DEFAULT NAME";
  public bool requires_premium;
  [DefaultValue(PowerRank.Rank0_free)]
  public PowerRank rank;
  public string path_icon;
  public bool multiple_spawn_tip;
  public bool show_unit_stats_overview;
  public bool show_tool_sizes;
  public bool unselect_when_window;
  public bool make_buildings_transparent;
  [DefaultValue(MetaType.None)]
  public MetaType force_map_mode;
  public bool ignore_cursor_icon;
  public bool hold_action;
  public float click_interval;
  public float particle_interval;
  [DefaultValue(0.95f)]
  public float falling_chance = 0.95f;
  public string sound_drawing;
  public string sound_event;
  public string tile_type;
  [NonSerialized]
  internal TileType cached_tile_type_asset;
  public string top_tile_type;
  [NonSerialized]
  internal TopTileType cached_top_tile_type_asset;
  public string drop_id;
  [NonSerialized]
  internal DropAsset cached_drop_asset;
  public string force_brush;
  public bool terraform;
  public bool draw_lines;
  [DefaultValue(PowerActionType.PowerSpecial)]
  public PowerActionType type;
  [DefaultValue(MouseHoldAnimation.Default)]
  public MouseHoldAnimation mouse_hold_animation;
  public bool highlight;
  public PowerActionWithID click_brush_action;
  public PowerActionWithID click_action;
  public PowerActionWithID click_special_action;
  public PowerAction click_power_brush_action;
  public PowerAction click_power_action;
  public PowerButtonClickAction select_button_action;
  public bool disabled_on_mobile;
  public string toggle_name;
  public bool multi_toggle;
  public PowerToggleAction toggle_action;
  public string actor_asset_id;
  public string[] actor_asset_ids;
  [DefaultValue(6f)]
  public float actor_spawn_height = 6f;
  public bool show_spawn_effect;
  public string printers_print;
  public bool ignore_fast_spawn;
  public bool set_used_camera_drag_on_long_move;
  public bool can_drag_map;
  [DefaultValue(true)]
  public bool tester_enabled = true;
  [DefaultValue(true)]
  public bool track_activity = true;
  public bool map_modes_switch;
  public bool allow_unit_selection;
  public bool show_close_actor;
  [DefaultValue(true)]
  public bool activate_on_hotkey_select = true;
  [DefaultValue(true)]
  public bool surprises_units = true;
  [NonSerialized]
  public Sprite sprite_icon;

  [JsonIgnore]
  public bool has_sound_drawing => this.sound_drawing != null;

  [JsonIgnore]
  public bool has_sound_event => this.sound_event != null;

  [JsonIgnore]
  public OptionAsset option_asset => AssetManager.options_library.get(this.toggle_name);

  public bool isSelected() => PowerButtonSelector.instance.selectedButton?.godPower == this;

  public bool isAvailable() => true;

  internal static void addPower(GodPower pPower, PowerButton pButton)
  {
    GodPower.god_powers_on_canvas[pPower.id] = pPower;
    if (pPower.requires_premium)
    {
      GodPower.premium_powers.Add(pPower);
      GodPower.premium_buttons.Add(pButton);
    }
    if (!pPower.requires_premium)
    {
      GodPower.powers_rank_0.Add(pButton);
    }
    else
    {
      switch (pPower.rank)
      {
        case PowerRank.Rank1_common:
          GodPower.powers_rank_1.Add(pButton);
          break;
        case PowerRank.Rank2_normal:
          GodPower.powers_rank_2.Add(pButton);
          break;
        case PowerRank.Rank3_good:
          GodPower.powers_rank_3.Add(pButton);
          break;
        case PowerRank.Rank4_awesome:
          GodPower.powers_rank_4.Add(pButton);
          break;
      }
    }
  }

  public Sprite getIconSprite() => SpriteTextureLoader.getSprite("ui/Icons/" + this.path_icon);

  public static void diagnostic()
  {
    LogText.log("Ranked Powers", "Print");
    GodPower.printRankedPowerButtons("rank0", GodPower.powers_rank_0);
    GodPower.printRankedPowerButtons("rank1", GodPower.powers_rank_1);
    GodPower.printRankedPowerButtons("rank2", GodPower.powers_rank_2);
    GodPower.printRankedPowerButtons("rank3", GodPower.powers_rank_3);
    GodPower.printRankedPowerButtons("rank4", GodPower.powers_rank_4);
    GodPower.printRankedPowers("premium powers", GodPower.premium_powers);
  }

  private static void printRankedPowerButtons(string pID, List<PowerButton> pList)
  {
    string pInfo = "";
    foreach (PowerButton p in pList)
      pInfo = $"{pInfo}{p.godPower.id}, ";
    if (pInfo.Length > 2)
      pInfo = pInfo.Substring(0, pInfo.Length - 2);
    LogText.log(pID, pInfo);
  }

  private static void printRankedPowers(string pID, List<GodPower> pList)
  {
    string str = "";
    foreach (GodPower p in pList)
      str = $"{str}{p.id}, ";
    string pInfo = str.Substring(0, str.Length - 2);
    LogText.log(pID, pInfo);
  }

  public string getActorAssetID()
  {
    if (this.actor_asset_id != null)
      return this.actor_asset_id;
    string[] actorAssetIds = this.actor_asset_ids;
    return (actorAssetIds != null ? (actorAssetIds.Length != 0 ? 1 : 0) : 0) != 0 ? this.actor_asset_ids[0] : (string) null;
  }

  public ActorAsset getActorAsset() => AssetManager.actor_library.get(this.getActorAssetID());

  public string getLocaleID() => this.name.Underscore();

  public string getDescriptionID() => this.getLocaleID() + "_description";

  public string getTranslatedName() => this.getLocaleID().Localize();

  public string getTranslatedDescription() => this.getDescriptionID().Localize();
}
