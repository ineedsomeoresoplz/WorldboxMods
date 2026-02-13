// Decompiled with JetBrains decompiler
// Type: KingdomAsset
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
public class KingdomAsset : Asset
{
  public bool civ;
  [DefaultValue(-1)]
  public int default_civ_color_index = -1;
  public bool nomads;
  public bool nature;
  public bool abandoned;
  public bool concept;
  public bool always_attack_each_other;
  public bool units_always_looking_for_enemies;
  [NonSerialized]
  public HashSet<string> assets_discrepancies;
  [NonSerialized]
  public HashSet<string> assets_discrepancies_bad;
  public bool force_look_all_chunks;
  public bool mobs;
  public bool neutral;
  public bool brain;
  public bool group_miniciv;
  public bool group_minicivs_cool;
  public bool group_creeps;
  public bool group_main;
  public bool is_forced_by_trait;
  [DefaultValue("")]
  public string forced_by_trait_kingdom_id = string.Empty;
  [DefaultValue("")]
  public string building_attractor_id = string.Empty;
  public bool count_as_danger = true;
  public HashSet<string> friendly_tags = new HashSet<string>();
  public HashSet<string> enemy_tags = new HashSet<string>();
  public HashSet<string> list_tags = new HashSet<string>();
  private Dictionary<int, int> _cached_enemies = new Dictionary<int, int>();
  public ColorAsset default_kingdom_color;
  public Color color_building = Color.white;
  private Sprite _cached_sprite;
  public string path_icon = "ui/Icons/iconWarning";
  public bool show_icon;
  public bool friendship_for_everyone;
  private ColorAsset _debug_color_asset;

  [JsonIgnore]
  public ColorAsset debug_color_asset
  {
    get
    {
      if (this._debug_color_asset == null)
      {
        KingdomColorsLibrary kingdomColorsLibrary = AssetManager.kingdom_colors_library;
        ColorAsset colorAsset;
        if (kingdomColorsLibrary == null)
        {
          colorAsset = (ColorAsset) null;
        }
        else
        {
          List<ColorAsset> list = kingdomColorsLibrary.list;
          colorAsset = list != null ? list.GetRandom<ColorAsset>() : (ColorAsset) null;
        }
        this._debug_color_asset = colorAsset;
      }
      return this._debug_color_asset;
    }
    set => this._debug_color_asset = value;
  }

  public Sprite getSprite()
  {
    if (Object.op_Equality((Object) this._cached_sprite, (Object) null))
      this._cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this._cached_sprite;
  }

  public void setIcon(string pPath)
  {
    this.path_icon = pPath;
    this.show_icon = true;
  }

  public void addTag(string pTag) => this.list_tags.Add(pTag);

  public void addFriendlyTag(string pTag) => this.friendly_tags.Add(pTag);

  public void addEnemyTag(string pTag) => this.enemy_tags.Add(pTag);

  public bool isFoe(KingdomAsset pTarget)
  {
    int num = 0;
    int hashCode = pTarget.GetHashCode();
    this._cached_enemies.TryGetValue(hashCode, out num);
    if (num != 0)
      return num == 1;
    if (this.nature || pTarget.nature)
    {
      this._cached_enemies.Add(hashCode, -1);
      return false;
    }
    if (this == pTarget)
    {
      this._cached_enemies.Add(hashCode, this.always_attack_each_other ? 1 : -1);
      return this.always_attack_each_other;
    }
    if (this.enemy_tags.Count > 0 && this.enemy_tags.Overlaps((IEnumerable<string>) pTarget.list_tags))
    {
      this._cached_enemies.Add(hashCode, 1);
      return true;
    }
    pTarget.list_tags.Add(pTarget.id);
    this.list_tags.Add(this.id);
    if (this.friendly_tags.Count > 0 && this.friendly_tags.Overlaps((IEnumerable<string>) pTarget.list_tags))
    {
      this._cached_enemies.Add(hashCode, -1);
      return false;
    }
    this._cached_enemies.Add(hashCode, 1);
    return true;
  }

  public void clearKingdomColor() => this.default_kingdom_color = (ColorAsset) null;
}
