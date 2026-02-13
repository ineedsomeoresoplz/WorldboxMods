// Decompiled with JetBrains decompiler
// Type: PowerTabAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class PowerTabAsset : Asset, IDescriptionAsset, ILocalizedAsset
{
  [NonSerialized]
  public PowersTab gameplay_tab;
  public bool tab_type_selected;
  public bool tab_type_main;
  public PowerTabGetter get_power_tab;
  public string window_id;
  public MetaType meta_type;
  public PowerTabAction on_main_tab_select;
  public PowerTabAction on_main_info_click;
  public PowerTabActionCheck on_update_check_active;
  public PowerTabWorldtipAction get_localized_worldtip;
  public string icon_path;
  public string locale_key;
  [NonSerialized]
  public float last_scroll_position;

  public void tryToShowPowerTab()
  {
    PowerTabGetter getPowerTab = this.get_power_tab;
    PowersTab powersTab = getPowerTab != null ? getPowerTab() : (PowersTab) null;
    if (Object.op_Equality((Object) powersTab, (Object) null))
      Debug.LogError((object) ("PowerTabAsset: tryToShowPowerTab: get_power_tab returned null for " + this.meta_type.ToString()));
    else
      powersTab.tryToShowTab();
  }

  public string getLocaleID() => this.locale_key;

  public string getDescriptionID()
  {
    return this.locale_key == null ? (string) null : this.locale_key + "_info";
  }

  public Sprite getIcon() => SpriteTextureLoader.getSprite(this.icon_path);
}
