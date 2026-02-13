// Decompiled with JetBrains decompiler
// Type: ColorStyleAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ColorStyleAsset : Asset
{
  public string taxonomy_kingdom = "#76FFF8";
  public string taxonomy_phylum = "#74FFA3";
  public string taxonomy_subphylum = "#54FF8D";
  public string taxonomy_class = "#76FF4A";
  public string taxonomy_order = "#B9FF48";
  public string taxonomy_family = "#FEFD46";
  public string taxonomy_genus = "#F8AB4F";
  public string taxonomy_common_name = "#DC8D4E";
  public string color_text_grey = "#ADADAD";
  public string color_text_grey_dark = "#7D7D7D";
  public string color_text_selector = "#7FFF75AA";
  public string color_text_selector_remove = "#FF182AAA";
  public string color_text_pumpkin = "#FFA94C";
  public string color_text_pumpkin_light = "#FFBC66";
  public Color favorite_selected = Color.white;
  public Color favorite_not_selected = new Color(0.7f, 0.7f, 0.7f, 0.3f);
  public Color health_bar_main_green = Toolbox.makeColor("#00C21F");
  public Color health_bar_main_red = Toolbox.makeColor("#FF4300");
  public Color health_bar_background = Toolbox.makeColor("#303030");

  public string color_dead_text => this.color_text_grey_dark;

  public Color getSelectorColor() => Toolbox.makeColor(this.color_text_selector);

  public Color getSelectorRemoveColor() => Toolbox.makeColor(this.color_text_selector_remove);

  public string getColorForTaxonomy(string pID)
  {
    switch (pID)
    {
      case "taxonomy_class":
        return this.taxonomy_class;
      case "taxonomy_common_name":
        return this.taxonomy_common_name;
      case "taxonomy_family":
        return this.taxonomy_family;
      case "taxonomy_genus":
        return this.taxonomy_genus;
      case "taxonomy_kingdom":
        return this.taxonomy_kingdom;
      case "taxonomy_order":
        return this.taxonomy_order;
      case "taxonomy_phylum":
        return this.taxonomy_phylum;
      case "taxonomy_subphylum":
        return this.taxonomy_subphylum;
      default:
        return "0xFFFFFF";
    }
  }
}
