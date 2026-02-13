// Decompiled with JetBrains decompiler
// Type: NameSetAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class NameSetAsset : Asset
{
  [DefaultValue("")]
  public string city = string.Empty;
  [DefaultValue("")]
  public string clan = string.Empty;
  [DefaultValue("")]
  public string culture = string.Empty;
  [DefaultValue("")]
  public string family = string.Empty;
  [DefaultValue("")]
  public string kingdom = string.Empty;
  [DefaultValue("")]
  public string language = string.Empty;
  [DefaultValue("")]
  public string unit = string.Empty;
  [DefaultValue("")]
  public string religion = string.Empty;

  public string get(MetaType pType)
  {
    switch (pType)
    {
      case MetaType.Family:
        return this.family;
      case MetaType.Language:
        return this.language;
      case MetaType.Culture:
        return this.culture;
      case MetaType.Religion:
        return this.religion;
      case MetaType.Clan:
        return this.clan;
      case MetaType.City:
        return this.city;
      case MetaType.Kingdom:
        return this.kingdom;
      case MetaType.Unit:
        return this.unit;
      default:
        return (string) null;
    }
  }

  public static IEnumerable<MetaType> getTypes()
  {
    yield return MetaType.City;
    yield return MetaType.Clan;
    yield return MetaType.Culture;
    yield return MetaType.Family;
    yield return MetaType.Kingdom;
    yield return MetaType.Language;
    yield return MetaType.Unit;
    yield return MetaType.Religion;
  }
}
