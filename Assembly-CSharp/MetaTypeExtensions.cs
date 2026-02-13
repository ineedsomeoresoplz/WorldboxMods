// Decompiled with JetBrains decompiler
// Type: MetaTypeExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class MetaTypeExtensions
{
  public static MetaTypeAsset getAsset(this MetaType pType)
  {
    return AssetManager.meta_type_library.getAsset(pType);
  }

  public static bool isNone(this MetaType pType) => pType == MetaType.None;

  public static int getZoneState(this MetaType pType)
  {
    return AssetManager.meta_type_library.getAsset(pType).getZoneOptionState();
  }

  public static string AsString(this MetaType pType)
  {
    switch (pType)
    {
      case MetaType.None:
        return "none";
      case MetaType.Subspecies:
        return "subspecies";
      case MetaType.Family:
        return "family";
      case MetaType.Language:
        return "language";
      case MetaType.Culture:
        return "culture";
      case MetaType.Religion:
        return "religion";
      case MetaType.Clan:
        return "clan";
      case MetaType.City:
        return "city";
      case MetaType.Kingdom:
        return "kingdom";
      case MetaType.Alliance:
        return "alliance";
      case MetaType.War:
        return "war";
      case MetaType.Plot:
        return "plot";
      case MetaType.Unit:
        return "unit";
      case MetaType.Building:
        return "building";
      case MetaType.Item:
        return "item";
      case MetaType.World:
        return "world";
      case MetaType.Special:
        return "special";
      case MetaType.Army:
        return "army";
      default:
        Debug.LogError((object) ("MetaTypeExtensions.AsString missing option for : " + pType.ToString()));
        return pType.ToString().ToLower();
    }
  }
}
