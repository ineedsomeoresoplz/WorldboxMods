// Decompiled with JetBrains decompiler
// Type: MetaRepresentationAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class MetaRepresentationAsset : Asset, ILocalizedAsset
{
  public MetaType meta_type;
  public string title_locale;
  public IconPathGetter icon_getter;
  public CheckActorHasMeta check_has_meta;
  public GetMetaFromActor meta_getter;
  public GetMetaTotalFromActor meta_getter_total;
  public GetWorldUnits world_units_getter;
  public string general_icon_path;
  public bool show_none_percent;
  [DefaultValue(true)]
  public bool show_none_percent_for_total = true;
  [DefaultValue(true)]
  public bool show_species_icon = true;

  public string getLocaleID() => this.title_locale;
}
