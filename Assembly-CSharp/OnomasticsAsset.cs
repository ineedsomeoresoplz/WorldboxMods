// Decompiled with JetBrains decompiler
// Type: OnomasticsAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class OnomasticsAsset : Asset, IDescription2Asset, IDescriptionAsset, ILocalizedAsset
{
  public OnomasticsAssetType type;
  public string path_icon;
  public string color_text = "#FF0000";
  [NonSerialized]
  private Sprite _cached_sprite;
  public bool affects_left;
  public bool affects_left_word;
  public bool affects_left_group_only;
  public bool affects_everything;
  public bool is_divider;
  public bool is_upper;
  public bool is_word_divider;
  public bool is_immune;
  [DefaultValue(-1)]
  public int group_id = -1;
  [DefaultValue('?')]
  public char short_id = '?';
  [DefaultValue("")]
  public string forced_locale_subname_id = string.Empty;
  [DefaultValue("")]
  public string forced_locale_description_id = string.Empty;
  [DefaultValue("")]
  public string forced_locale_description_id_2 = string.Empty;
  public OnomasticsNameMakerDelegate namemaker_delegate;
  public OnomasticsCheckDelegate check_delegate;

  public bool isGroupType() => this.type == OnomasticsAssetType.Group;

  public Sprite getSprite()
  {
    if (this._cached_sprite == null)
      this._cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this._cached_sprite;
  }

  public string getLocaleID() => "onomastics_" + this.id;

  public string getIDSubname()
  {
    return this.forced_locale_subname_id != string.Empty ? this.forced_locale_subname_id : $"onomastics_{this.id}_subname";
  }

  public string getDescriptionID()
  {
    return this.forced_locale_description_id != string.Empty ? this.forced_locale_description_id : this.getLocaleID() + "_info";
  }

  public string getDescriptionID2()
  {
    return this.forced_locale_description_id_2 != string.Empty ? this.forced_locale_description_id_2 : this.getLocaleID() + "_info_2";
  }
}
