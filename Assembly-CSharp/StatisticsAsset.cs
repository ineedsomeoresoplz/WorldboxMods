// Decompiled with JetBrains decompiler
// Type: StatisticsAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class StatisticsAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  public string localized_key;
  public string localized_key_description;
  public LocaleGetter locale_getter;
  public int rarity;
  [DefaultValue("#Status_stat")]
  public string steam_activity = "#Status_stat";
  public StatisticsStringAction string_action = (StatisticsStringAction) (pAsset => pAsset.long_action != null ? pAsset.long_action(pAsset).ToText() : (string) null);
  public StatisticsLongAction long_action;
  public MetaIdGetter get_meta_id;
  [NonSerialized]
  public string last_value = string.Empty;
  public bool is_world_statistics;
  public bool is_game_statistics;
  public WorldStatsTabs world_stats_tabs;
  [DefaultValue(MetaType.None)]
  public MetaType world_stats_meta_type;
  [DefaultValue(MetaType.None)]
  public MetaType list_window_meta_type;
  public string path_icon;
  private Sprite _icon;

  public Sprite getIcon()
  {
    if (Object.op_Equality((Object) this._icon, (Object) null) && !string.IsNullOrEmpty(this.path_icon))
      this._icon = SpriteTextureLoader.getSprite(this.path_icon);
    return this._icon;
  }

  public string getLocaleID() => this.localized_key.Underscore() ?? this.id;

  public string getDescriptionID()
  {
    string pKey = this.getLocaleID() + "_description";
    if (!string.IsNullOrEmpty(this.localized_key_description))
      pKey = this.localized_key_description;
    return LocalizedTextManager.stringExists(pKey) ? pKey : this.getLocaleID() + "_description";
  }
}
