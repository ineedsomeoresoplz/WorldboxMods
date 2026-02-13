// Decompiled with JetBrains decompiler
// Type: PhenotypeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class PhenotypeAsset : BaseAugmentationAsset, ISkipLocaleAsset
{
  public string shades_from;
  public string shades_to;
  public string color_eyes;
  public string color_details_1;
  public string color_details_2;
  public int phenotype_index;
  [NonSerialized]
  public Color32[] colors = new Color32[4];
  public string subspecies_trait_id;

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.subspecies_trait_groups.get(this.group_id);
  }

  public PhenotypeAsset() => this.has_locales = false;
}
