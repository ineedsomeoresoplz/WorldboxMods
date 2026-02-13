// Decompiled with JetBrains decompiler
// Type: BaseStatAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class BaseStatAsset : Asset, ILocalizedAsset
{
  public bool hidden;
  public string icon;
  public bool normalize;
  public float normalize_min;
  [DefaultValue(2.14748365E+09f)]
  public float normalize_max = (float) int.MaxValue;
  public bool used_only_for_civs;
  public bool actor_data_attribute;
  public bool show_as_percents;
  [DefaultValue(1f)]
  public float tooltip_multiply_for_visual_number = 1f;
  public bool multiplier;
  public string main_stat_to_multiply;
  public int sort_rank;
  public bool ignore;
  public string translation_key;

  public string getLocaleID() => this.translation_key ?? this.id;
}
