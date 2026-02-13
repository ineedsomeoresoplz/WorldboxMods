// Decompiled with JetBrains decompiler
// Type: MetaCustomizationAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class MetaCustomizationAsset : Asset, IMultiLocalesAsset
{
  public string localization_title;
  public MetaType meta_type;
  public string banner_prefab_id;
  public MetaBanner get_banner;
  public bool editable = true;
  public bool option_1_editable = true;
  public bool option_2_editable = true;
  public bool option_2_color_editable = true;
  public bool color_editable = true;
  public MetaCustomizationComponent customize_component;
  public string customize_window_id;
  public MetaCustomizationOptionGet option_1_get;
  public MetaCustomizationOptionSet option_1_set;
  public MetaCustomizationOptionGet option_2_get;
  public MetaCustomizationOptionSet option_2_set;
  public MetaCustomizationOptionGet color_get;
  public MetaCustomizationOptionSet color_set;
  public MetaCustomizationCounter option_1_count;
  public MetaCustomizationCounter option_2_count;
  public MetaCustomizationCounter color_count;
  public MetaCustomizationColorLibrary color_library;
  public MetaCustomization on_new_color = (MetaCustomization) (() => World.world.zone_calculator.dirtyAndClear());
  public string title_locale;
  public string option_1_locale;
  public string option_2_locale;
  public string color_locale;
  public string icon_banner;
  public string icon_creature;

  public IEnumerable<string> getLocaleIDs()
  {
    if (this.editable)
    {
      yield return this.localization_title;
      yield return this.title_locale;
      yield return this.option_1_locale;
      yield return this.option_2_locale;
      yield return this.color_locale;
    }
  }
}
