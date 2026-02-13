// Decompiled with JetBrains decompiler
// Type: MetaCustomizationLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MetaCustomizationLibrary : AssetLibrary<MetaCustomizationAsset>
{
  public override void init()
  {
    base.init();
    MetaCustomizationAsset pAsset1 = new MetaCustomizationAsset();
    pAsset1.id = "religion";
    pAsset1.meta_type = MetaType.Religion;
    pAsset1.banner_prefab_id = "ui/PrefabBannerReligion";
    pAsset1.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      ReligionBanner religionBanner = Object.Instantiate<ReligionBanner>(Resources.Load<ReligionBanner>(pAsset.banner_prefab_id), pParent);
      religionBanner.enable_default_click = false;
      religionBanner.load((NanoObject) (pNanoObject as Religion));
      return (IBanner) religionBanner;
    });
    pAsset1.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<ReligionCustomizeWindow>());
    pAsset1.customize_window_id = "religion_customize";
    pAsset1.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_religion.data.banner_background_id);
    pAsset1.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_religion.data.banner_background_id = pValue);
    pAsset1.option_2_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_religion.data.banner_icon_id);
    pAsset1.option_2_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_religion.data.banner_icon_id = pValue);
    pAsset1.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_religion.data.color_id);
    pAsset1.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_religion.data.setColorID(pValue));
    pAsset1.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.religion_colors_library);
    pAsset1.option_1_count = (MetaCustomizationCounter) (() => AssetManager.religion_banners_library.getCurrentAsset().backgrounds.Count);
    pAsset1.option_2_count = (MetaCustomizationCounter) (() => AssetManager.religion_banners_library.getCurrentAsset().icons.Count);
    pAsset1.title_locale = "customize_religion";
    pAsset1.option_1_locale = "religion_background";
    pAsset1.option_2_locale = "religion_element";
    pAsset1.color_locale = "religion_color";
    pAsset1.icon_banner = "iconReligion";
    pAsset1.icon_creature = "iconLivingPlants";
    this.add(pAsset1);
    MetaCustomizationAsset pAsset2 = new MetaCustomizationAsset();
    pAsset2.id = "culture";
    pAsset2.meta_type = MetaType.Culture;
    pAsset2.banner_prefab_id = "ui/PrefabBannerCulture";
    pAsset2.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      CultureBanner cultureBanner = Object.Instantiate<CultureBanner>(Resources.Load<CultureBanner>(pAsset.banner_prefab_id), pParent);
      cultureBanner.enable_default_click = false;
      cultureBanner.load((NanoObject) (pNanoObject as Culture));
      return (IBanner) cultureBanner;
    });
    pAsset2.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<CultureCustomizeWindow>());
    pAsset2.customize_window_id = "culture_customize";
    pAsset2.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_culture.data.banner_decor_id);
    pAsset2.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_culture.data.banner_decor_id = pValue);
    pAsset2.option_2_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_culture.data.banner_element_id);
    pAsset2.option_2_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_culture.data.banner_element_id = pValue);
    pAsset2.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_culture.data.color_id);
    pAsset2.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_culture.data.setColorID(pValue));
    pAsset2.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.culture_colors_library);
    pAsset2.option_1_count = (MetaCustomizationCounter) (() => AssetManager.culture_banners_library.getCurrentAsset().backgrounds.Count);
    pAsset2.option_2_count = (MetaCustomizationCounter) (() => AssetManager.culture_banners_library.getCurrentAsset().icons.Count);
    pAsset2.title_locale = "customize_culture";
    pAsset2.option_1_locale = "culture_decor";
    pAsset2.option_2_locale = "culture_element";
    pAsset2.color_locale = "culture_color";
    pAsset2.icon_banner = "iconCulture";
    pAsset2.icon_creature = "iconSuperPumpkin";
    this.add(pAsset2);
    MetaCustomizationAsset pAsset3 = new MetaCustomizationAsset();
    pAsset3.id = "family";
    pAsset3.meta_type = MetaType.Family;
    pAsset3.banner_prefab_id = "ui/PrefabBannerFamily";
    pAsset3.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      FamilyBanner familyBanner = Object.Instantiate<FamilyBanner>(Resources.Load<FamilyBanner>(pAsset.banner_prefab_id), pParent);
      familyBanner.enable_default_click = false;
      familyBanner.load((NanoObject) (pNanoObject as Family));
      return (IBanner) familyBanner;
    });
    pAsset3.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<FamilyCustomizeWindow>());
    pAsset3.customize_window_id = "family_customize";
    pAsset3.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_family.data.banner_background_id);
    pAsset3.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_family.data.banner_background_id = pValue);
    pAsset3.option_2_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_family.data.banner_frame_id);
    pAsset3.option_2_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_family.data.banner_frame_id = pValue);
    pAsset3.option_2_color_editable = false;
    pAsset3.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_family.data.color_id);
    pAsset3.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_family.data.setColorID(pValue));
    pAsset3.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.families_colors_library);
    pAsset3.option_1_count = (MetaCustomizationCounter) (() => AssetManager.family_banners_library.getCurrentAsset().backgrounds.Count);
    pAsset3.option_2_count = (MetaCustomizationCounter) (() => AssetManager.family_banners_library.getCurrentAsset().frames.Count);
    pAsset3.title_locale = "customize_family";
    pAsset3.option_1_locale = "family_background";
    pAsset3.option_2_locale = "family_frame";
    pAsset3.color_locale = "family_color";
    pAsset3.icon_banner = "iconFamily";
    pAsset3.icon_creature = "iconLivingPlants";
    this.add(pAsset3);
    MetaCustomizationAsset pAsset4 = new MetaCustomizationAsset();
    pAsset4.id = "language";
    pAsset4.meta_type = MetaType.Language;
    pAsset4.banner_prefab_id = "ui/PrefabBannerLanguage";
    pAsset4.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      LanguageBanner languageBanner = Object.Instantiate<LanguageBanner>(Resources.Load<LanguageBanner>(pAsset.banner_prefab_id), pParent);
      languageBanner.enable_default_click = false;
      languageBanner.load((NanoObject) (pNanoObject as Language));
      return (IBanner) languageBanner;
    });
    pAsset4.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<LanguageCustomizeWindow>());
    pAsset4.customize_window_id = "language_customize";
    pAsset4.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_language.data.banner_background_id);
    pAsset4.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_language.data.banner_background_id = pValue);
    pAsset4.option_2_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_language.data.banner_icon_id);
    pAsset4.option_2_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_language.data.banner_icon_id = pValue);
    pAsset4.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_language.data.color_id);
    pAsset4.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_language.data.setColorID(pValue));
    pAsset4.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.languages_colors_library);
    pAsset4.option_1_count = (MetaCustomizationCounter) (() => AssetManager.language_banners_library.getCurrentAsset().backgrounds.Count);
    pAsset4.option_2_count = (MetaCustomizationCounter) (() => AssetManager.language_banners_library.getCurrentAsset().icons.Count);
    pAsset4.title_locale = "customize_language";
    pAsset4.option_1_locale = "language_background";
    pAsset4.option_2_locale = "language_element";
    pAsset4.color_locale = "language_color";
    pAsset4.icon_banner = "iconLanguage";
    pAsset4.icon_creature = "iconLivingPlants";
    this.add(pAsset4);
    MetaCustomizationAsset pAsset5 = new MetaCustomizationAsset();
    pAsset5.id = "subspecies";
    pAsset5.meta_type = MetaType.Subspecies;
    pAsset5.banner_prefab_id = "ui/PrefabBannerSubspecies";
    pAsset5.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      SubspeciesBanner subspeciesBanner = Object.Instantiate<SubspeciesBanner>(Resources.Load<SubspeciesBanner>(pAsset.banner_prefab_id), pParent);
      subspeciesBanner.enable_default_click = false;
      subspeciesBanner.load((NanoObject) (pNanoObject as Subspecies));
      return (IBanner) subspeciesBanner;
    });
    pAsset5.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<SubspeciesCustomizeWindow>());
    pAsset5.customize_window_id = "subspecies_customize";
    pAsset5.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_subspecies.data.banner_background_id);
    pAsset5.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_subspecies.data.banner_background_id = pValue);
    pAsset5.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_subspecies.data.color_id);
    pAsset5.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_subspecies.data.setColorID(pValue));
    pAsset5.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.subspecies_colors_library);
    pAsset5.option_1_count = (MetaCustomizationCounter) (() => AssetManager.subspecies_banners_library.getCurrentAsset().backgrounds.Count);
    pAsset5.option_2_editable = false;
    pAsset5.title_locale = "customize_subspecies";
    pAsset5.option_1_locale = "subspecies_background";
    pAsset5.color_locale = "subspecies_color";
    pAsset5.icon_banner = "iconSubspeciesList";
    pAsset5.icon_creature = "iconLivingPlants";
    this.add(pAsset5);
    MetaCustomizationAsset pAsset6 = new MetaCustomizationAsset();
    pAsset6.id = "kingdom";
    pAsset6.meta_type = MetaType.Kingdom;
    pAsset6.banner_prefab_id = "ui/PrefabBannerKingdom";
    pAsset6.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      KingdomBanner kingdomBanner = Object.Instantiate<KingdomBanner>(Resources.Load<KingdomBanner>(pAsset.banner_prefab_id), pParent);
      kingdomBanner.enable_default_click = false;
      kingdomBanner.load((NanoObject) (pNanoObject as Kingdom));
      return (IBanner) kingdomBanner;
    });
    pAsset6.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<KingdomCustomizeWindow>());
    pAsset6.customize_window_id = "kingdom_customize";
    pAsset6.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_kingdom.data.banner_background_id);
    pAsset6.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_kingdom.data.banner_background_id = pValue);
    pAsset6.option_2_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_kingdom.data.banner_icon_id);
    pAsset6.option_2_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_kingdom.data.banner_icon_id = pValue);
    pAsset6.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_kingdom.data.color_id);
    pAsset6.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_kingdom.data.setColorID(pValue));
    pAsset6.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.kingdom_colors_library);
    pAsset6.option_1_count = (MetaCustomizationCounter) (() => AssetManager.kingdom_banners_library.get(SelectedMetas.selected_kingdom.getActorAsset().banner_id).backgrounds.Count);
    pAsset6.option_2_count = (MetaCustomizationCounter) (() => AssetManager.kingdom_banners_library.get(SelectedMetas.selected_kingdom.getActorAsset().banner_id).icons.Count);
    pAsset6.title_locale = "customize_kingdom";
    pAsset6.option_1_locale = "banner_design";
    pAsset6.option_2_locale = "banner_emblem";
    pAsset6.color_locale = "kingdom_color";
    pAsset6.icon_banner = "iconCrown";
    pAsset6.icon_creature = "iconBiomass";
    this.add(pAsset6);
    MetaCustomizationAsset pAsset7 = new MetaCustomizationAsset();
    pAsset7.id = "city";
    pAsset7.meta_type = MetaType.City;
    pAsset7.localization_title = "village";
    pAsset7.option_1_editable = false;
    pAsset7.option_2_editable = false;
    pAsset7.banner_prefab_id = "ui/PrefabBannerCity";
    pAsset7.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      CityBanner cityBanner = Object.Instantiate<CityBanner>(Resources.Load<CityBanner>(pAsset.banner_prefab_id), pParent);
      cityBanner.enable_default_click = false;
      cityBanner.load((NanoObject) (pNanoObject as City));
      return (IBanner) cityBanner;
    });
    pAsset7.customize_window_id = "kingdom_customize";
    this.add(pAsset7);
    MetaCustomizationAsset pAsset8 = new MetaCustomizationAsset();
    pAsset8.id = "army";
    pAsset8.meta_type = MetaType.Army;
    pAsset8.localization_title = "army";
    pAsset8.option_1_editable = false;
    pAsset8.option_2_editable = false;
    pAsset8.banner_prefab_id = "ui/PrefabBannerArmy";
    pAsset8.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      ArmyBanner armyBanner = Object.Instantiate<ArmyBanner>(Resources.Load<ArmyBanner>(pAsset.banner_prefab_id), pParent);
      armyBanner.enable_default_click = false;
      armyBanner.load((NanoObject) (pNanoObject as Army));
      return (IBanner) armyBanner;
    });
    pAsset8.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.kingdom_colors_library);
    pAsset8.customize_window_id = "kingdom_customize";
    this.add(pAsset8);
    MetaCustomizationAsset pAsset9 = new MetaCustomizationAsset();
    pAsset9.id = "clan";
    pAsset9.meta_type = MetaType.Clan;
    pAsset9.banner_prefab_id = "ui/PrefabBannerClan";
    pAsset9.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      ClanBanner clanBanner = Object.Instantiate<ClanBanner>(Resources.Load<ClanBanner>(pAsset.banner_prefab_id), pParent);
      clanBanner.enable_default_click = false;
      clanBanner.load((NanoObject) (pNanoObject as Clan));
      return (IBanner) clanBanner;
    });
    pAsset9.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<ClanCustomizeWindow>());
    pAsset9.customize_window_id = "clan_customize";
    pAsset9.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_clan.data.banner_background_id);
    pAsset9.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_clan.data.banner_background_id = pValue);
    pAsset9.option_2_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_clan.data.banner_icon_id);
    pAsset9.option_2_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_clan.data.banner_icon_id = pValue);
    pAsset9.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_clan.data.color_id);
    pAsset9.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_clan.data.setColorID(pValue));
    pAsset9.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.clan_colors_library);
    pAsset9.option_1_count = (MetaCustomizationCounter) (() => AssetManager.clan_banners_library.getCurrentAsset().backgrounds.Count);
    pAsset9.option_2_count = (MetaCustomizationCounter) (() => AssetManager.clan_banners_library.getCurrentAsset().icons.Count);
    pAsset9.title_locale = "customize_clan";
    pAsset9.option_1_locale = "clan_background";
    pAsset9.option_2_locale = "clan_icon";
    pAsset9.color_locale = "clan_color";
    pAsset9.icon_banner = "iconClan";
    pAsset9.icon_creature = "iconSuperPumpkin";
    this.add(pAsset9);
    MetaCustomizationAsset pAsset10 = new MetaCustomizationAsset();
    pAsset10.id = "alliance";
    pAsset10.meta_type = MetaType.Alliance;
    pAsset10.banner_prefab_id = "ui/PrefabBannerAlliance";
    pAsset10.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      AllianceBanner allianceBanner = Object.Instantiate<AllianceBanner>(Resources.Load<AllianceBanner>(pAsset.banner_prefab_id), pParent);
      allianceBanner.enable_default_click = false;
      allianceBanner.load((NanoObject) (pNanoObject as Alliance));
      return (IBanner) allianceBanner;
    });
    pAsset10.customize_component = (MetaCustomizationComponent) (pGameObject => pGameObject.AddComponent<AllianceCustomizeWindow>());
    pAsset10.customize_window_id = "alliance_customize";
    pAsset10.option_1_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_alliance.data.banner_background_id);
    pAsset10.option_1_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_alliance.data.banner_background_id = pValue);
    pAsset10.option_2_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_alliance.data.banner_icon_id);
    pAsset10.option_2_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_alliance.data.banner_icon_id = pValue);
    pAsset10.color_get = (MetaCustomizationOptionGet) (() => SelectedMetas.selected_alliance.data.color_id);
    pAsset10.color_set = (MetaCustomizationOptionSet) (pValue => SelectedMetas.selected_alliance.data.setColorID(pValue));
    pAsset10.color_library = (MetaCustomizationColorLibrary) (() => (ColorLibrary) AssetManager.kingdom_colors_library);
    pAsset10.option_1_count = (MetaCustomizationCounter) (() => World.world.alliances.getBackgroundsList().Length);
    pAsset10.option_2_count = (MetaCustomizationCounter) (() => World.world.alliances.getIconsList().Length);
    pAsset10.title_locale = "customize_alliance";
    pAsset10.option_1_locale = "alliance_background";
    pAsset10.option_2_locale = "alliance_icon";
    pAsset10.color_locale = "alliance_color";
    pAsset10.icon_banner = "iconAlliance";
    pAsset10.icon_creature = "iconSuperPumpkin";
    this.add(pAsset10);
    MetaCustomizationAsset pAsset11 = new MetaCustomizationAsset();
    pAsset11.id = "plot";
    pAsset11.meta_type = MetaType.Plot;
    pAsset11.editable = false;
    pAsset11.banner_prefab_id = "ui/PrefabBannerPlot";
    pAsset11.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      PlotBanner plotBanner = Object.Instantiate<PlotBanner>(Resources.Load<PlotBanner>(pAsset.banner_prefab_id), pParent);
      plotBanner.load(pNanoObject);
      return (IBanner) plotBanner;
    });
    this.add(pAsset11);
    MetaCustomizationAsset pAsset12 = new MetaCustomizationAsset();
    pAsset12.id = "war";
    pAsset12.meta_type = MetaType.War;
    pAsset12.editable = false;
    pAsset12.banner_prefab_id = "ui/PrefabBannerWar";
    pAsset12.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      WarBanner warBanner = Object.Instantiate<WarBanner>(Resources.Load<WarBanner>(pAsset.banner_prefab_id), pParent);
      warBanner.load(pNanoObject);
      return (IBanner) warBanner;
    });
    this.add(pAsset12);
    MetaCustomizationAsset pAsset13 = new MetaCustomizationAsset();
    pAsset13.id = "unit";
    pAsset13.meta_type = MetaType.Unit;
    pAsset13.editable = false;
    pAsset13.banner_prefab_id = "ui/UnitAvatarElement";
    pAsset13.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      UiUnitAvatarElement unitAvatarElement = Object.Instantiate<UiUnitAvatarElement>(Resources.Load<UiUnitAvatarElement>(pAsset.banner_prefab_id), pParent);
      unitAvatarElement.load(pNanoObject);
      ((Component) unitAvatarElement).transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
      return (IBanner) unitAvatarElement;
    });
    this.add(pAsset13);
    MetaCustomizationAsset pAsset14 = new MetaCustomizationAsset();
    pAsset14.id = "item";
    pAsset14.meta_type = MetaType.Item;
    pAsset14.editable = false;
    pAsset14.banner_prefab_id = "ui/EquipmentButton";
    pAsset14.get_banner = (MetaBanner) ((pAsset, pNanoObject, pParent) =>
    {
      EquipmentButton equipmentButton = Object.Instantiate<EquipmentButton>(Resources.Load<EquipmentButton>(pAsset.banner_prefab_id), pParent);
      equipmentButton.load(pNanoObject);
      return (IBanner) equipmentButton;
    });
    this.add(pAsset14);
    MetaCustomizationAsset pAsset15 = new MetaCustomizationAsset();
    pAsset15.id = "world";
    pAsset15.meta_type = MetaType.World;
    pAsset15.editable = false;
    pAsset15.option_1_editable = false;
    pAsset15.option_2_editable = false;
    pAsset15.color_editable = false;
    this.add(pAsset15);
  }

  public MetaCustomizationAsset getAsset(MetaType pType) => this.get(pType.AsString());

  public override void post_init()
  {
    base.post_init();
    foreach (MetaCustomizationAsset customizationAsset in this.list)
    {
      if (customizationAsset.localization_title == null)
        customizationAsset.localization_title = customizationAsset.id;
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (MetaCustomizationAsset customizationAsset in this.list)
    {
      MetaCustomizationAsset tAsset = customizationAsset;
      if (tAsset.color_count == null)
        tAsset.color_count = (MetaCustomizationCounter) (() => tAsset.color_library().list.Count);
    }
  }

  public override void editorDiagnostic()
  {
    foreach (MetaCustomizationAsset customizationAsset in this.list)
    {
      if (customizationAsset.editable)
      {
        if (customizationAsset.color_count == null)
          BaseAssetLibrary.logAssetError("Missing <e>color_count</e>", customizationAsset.id);
        if (customizationAsset.option_1_editable && customizationAsset.option_1_count == null)
          BaseAssetLibrary.logAssetError("Missing <e>option_1_count</e>", customizationAsset.id);
        if (customizationAsset.option_2_editable && customizationAsset.option_2_count == null)
          BaseAssetLibrary.logAssetError("Missing <e>option_2_count</e>", customizationAsset.id);
      }
    }
    base.editorDiagnostic();
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (MetaCustomizationAsset pAsset in this.list)
    {
      if (pAsset.editable)
      {
        foreach (string localeId in pAsset.getLocaleIDs())
          this.checkLocale((Asset) pAsset, localeId);
      }
    }
  }
}
