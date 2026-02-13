// Decompiled with JetBrains decompiler
// Type: Zones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class Zones
{
  internal static bool isPowerForceMapMode(MetaType pMode = MetaType.None)
  {
    return World.world.isAnyPowerSelected() && Zones._selected_power.force_map_mode == pMode;
  }

  public static bool isPowerForcedMapModeEnabled()
  {
    MetaType pType = MetaType.None;
    if (World.world.isAnyPowerSelected() && !Zones._selected_power.force_map_mode.isNone())
      pType = Zones._selected_power.force_map_mode;
    return !pType.isNone();
  }

  internal static MetaType getForcedMapMode()
  {
    MetaType forcedMapMode = MetaType.None;
    if (Zones.isPowerForcedMapModeEnabled())
      forcedMapMode = Zones._selected_power.force_map_mode;
    else if (SelectedObjects.isNanoObjectSet())
    {
      MetaTypeAsset metaTypeAsset = SelectedObjects.getSelectedNanoObject().getMetaTypeAsset();
      if (metaTypeAsset.force_zone_when_selected)
        forcedMapMode = metaTypeAsset.map_mode;
    }
    return forcedMapMode;
  }

  public static bool hasPowerForceMapMode() => !Zones.getForcedMapMode().isNone();

  public static MetaTypeAsset getMapMetaAsset()
  {
    if (Zones.hasPowerForceMapMode())
      return Zones.getForcedMapMode().getAsset();
    for (int index = 0; index < AssetManager.meta_type_library.list.Count; ++index)
    {
      MetaTypeAsset mapMetaAsset = AssetManager.meta_type_library.list[index];
      if (!string.IsNullOrEmpty(mapMetaAsset.option_id) && (AssetManager.options_library.get(mapMetaAsset.option_id).isActive() || Zones.isPowerForceMapMode(mapMetaAsset.map_mode)))
        return mapMetaAsset;
    }
    return (MetaTypeAsset) null;
  }

  public static bool showMapNames()
  {
    return PlayerConfig.optionBoolEnabled("map_names") || Zones.hasPowerForceMapMode();
  }

  public static bool showMapBorders() => Zones.isBordersEnabled() || Zones.hasPowerForceMapMode();

  public static bool isBordersEnabled() => PlayerConfig.optionBoolEnabled("map_layers");

  public static MetaType getCurrentMapBorderMode(bool pCheckOnlyOption = false)
  {
    return !Zones.showCultureZones(pCheckOnlyOption) ? (!Zones.showKingdomZones(pCheckOnlyOption) ? (!Zones.showClanZones(pCheckOnlyOption) ? (!Zones.showAllianceZones(pCheckOnlyOption) ? (!Zones.showCityZones(pCheckOnlyOption) ? (!Zones.showSpeciesZones(pCheckOnlyOption) ? (!Zones.showFamiliesZones(pCheckOnlyOption) ? (!Zones.showLanguagesZones(pCheckOnlyOption) ? (!Zones.showReligionZones(pCheckOnlyOption) ? (!Zones.showArmyZones(pCheckOnlyOption) ? MetaType.None : MetaType.Army) : MetaType.Religion) : MetaType.Language) : MetaType.Family) : MetaType.Subspecies) : MetaType.City) : MetaType.Alliance) : MetaType.Clan) : MetaType.Kingdom) : MetaType.Culture;
  }

  public static bool showCityZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.city.isActive(pCheckOnlyOption);
  }

  public static bool showKingdomZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.kingdom.isActive(pCheckOnlyOption);
  }

  public static bool showClanZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.clan.isActive(pCheckOnlyOption);
  }

  public static bool showAllianceZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.alliance.isActive(pCheckOnlyOption);
  }

  public static bool showCultureZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.culture.isActive(pCheckOnlyOption);
  }

  public static bool showSpeciesZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.subspecies.isActive(pCheckOnlyOption);
  }

  public static bool showFamiliesZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.family.isActive(pCheckOnlyOption);
  }

  public static bool showLanguagesZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.language.isActive(pCheckOnlyOption);
  }

  public static bool showReligionZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.religion.isActive(pCheckOnlyOption);
  }

  public static bool showArmyZones(bool pCheckOnlyOption = false)
  {
    return MetaTypeLibrary.army.isActive(pCheckOnlyOption);
  }

  private static GodPower _selected_power => World.world.selected_power;
}
