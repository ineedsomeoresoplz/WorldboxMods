// Decompiled with JetBrains decompiler
// Type: WindowLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
public class WindowLibrary : AssetLibrary<WindowAsset>
{
  [NonSerialized]
  private List<WindowAsset> _testable_windows = new List<WindowAsset>();

  public override void init()
  {
    base.init();
    WindowAsset pAsset1 = new WindowAsset();
    pAsset1.id = "achievements";
    pAsset1.icon_path = "iconAchievements2";
    pAsset1.preload = true;
    this.add(pAsset1);
    WindowAsset pAsset2 = new WindowAsset();
    pAsset2.id = "actor_asset";
    pAsset2.icon_path = "iconDebug";
    pAsset2.is_testable = false;
    this.add(pAsset2);
    WindowAsset pAsset3 = new WindowAsset();
    pAsset3.id = "ad_loading_error";
    pAsset3.icon_path = "iconDeleteWorld";
    this.add(pAsset3);
    WindowAsset pAsset4 = new WindowAsset();
    pAsset4.id = "alliance";
    pAsset4.related_parent_window = "list_alliances";
    pAsset4.icon_path = "iconAlliance";
    pAsset4.preload = true;
    this.add(pAsset4);
    WindowAsset pAsset5 = new WindowAsset();
    pAsset5.id = "alliance_customize";
    pAsset5.related_parent_window = "list_alliances";
    pAsset5.icon_path = "iconColorCustomization";
    this.add(pAsset5);
    WindowAsset pAsset6 = new WindowAsset();
    pAsset6.id = "auto_saves_browse";
    pAsset6.icon_path = "actor_traits/iconBlessing";
    this.add(pAsset6);
    WindowAsset pAsset7 = new WindowAsset();
    pAsset7.id = "brushes";
    pAsset7.icon_path = "iconColorCirlce2";
    pAsset7.preload = true;
    pAsset7.is_testable = false;
    this.add(pAsset7);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("brushes/brush_circ_1", "brushes/brush_circ_10", "brushes/brush_circ_15", "brushes/brush_circ_2", "brushes/brush_circ_5", "brushes/brush_sqr_1", "brushes/brush_sqr_10", "brushes/brush_sqr_15", "brushes/brush_sqr_2", "brushes/brush_sqr_5"));
    WindowAsset pAsset8 = new WindowAsset();
    pAsset8.id = "building_asset";
    pAsset8.icon_path = "iconBuildings";
    pAsset8.is_testable = false;
    this.add(pAsset8);
    WindowAsset pAsset9 = new WindowAsset();
    pAsset9.id = "chart_comparer";
    pAsset9.icon_path = "iconCompareStatistics";
    this.add(pAsset9);
    WindowAsset pAsset10 = new WindowAsset();
    pAsset10.id = "city";
    pAsset10.related_parent_window = "list_cities";
    pAsset10.icon_path = "iconCity";
    pAsset10.preload = true;
    this.add(pAsset10);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconCityInspect", SelectedMetas.selected_city.getActorAsset().icon));
    WindowAsset pAsset11 = new WindowAsset();
    pAsset11.id = "army";
    pAsset11.related_parent_window = "list_armies";
    pAsset11.icon_path = "iconArmy";
    pAsset11.preload = true;
    this.add(pAsset11);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons(SelectedMetas.selected_army.getActorAsset().icon));
    WindowAsset pAsset12 = new WindowAsset();
    pAsset12.id = "clan";
    pAsset12.related_parent_window = "list_clans";
    pAsset12.icon_path = "iconClan";
    pAsset12.preload = true;
    this.add(pAsset12);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons(SelectedMetas.selected_clan.getActorAsset().icon));
    WindowAsset pAsset13 = new WindowAsset();
    pAsset13.id = "clan_customize";
    pAsset13.related_parent_window = "list_clans";
    pAsset13.icon_path = "iconColorCustomization";
    this.add(pAsset13);
    WindowAsset pAsset14 = new WindowAsset();
    pAsset14.id = "community_links";
    pAsset14.icon_path = "actor_traits/iconCommunity";
    this.add(pAsset14);
    WindowAsset pAsset15 = new WindowAsset();
    pAsset15.id = "credits";
    pAsset15.icon_path = "iconCoffee";
    pAsset15.window_toolbar_enabled = false;
    this.add(pAsset15);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("clan_traits/clan_trait_gods_chosen"));
    WindowAsset pAsset16 = new WindowAsset();
    pAsset16.id = "credits_community";
    pAsset16.icon_path = "actor_traits/iconStrong";
    this.add(pAsset16);
    WindowAsset pAsset17 = new WindowAsset();
    pAsset17.id = "culture";
    pAsset17.related_parent_window = "list_cultures";
    pAsset17.icon_path = "iconCulture";
    pAsset17.preload = true;
    this.add(pAsset17);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIconsUnits(SelectedMetas.selected_culture.units));
    WindowAsset pAsset18 = new WindowAsset();
    pAsset18.id = "culture_customize";
    pAsset18.related_parent_window = "list_cultures";
    pAsset18.icon_path = "iconColorCustomization";
    this.add(pAsset18);
    WindowAsset pAsset19 = new WindowAsset();
    pAsset19.id = "debug";
    pAsset19.icon_path = "iconDebug";
    pAsset19.is_testable = false;
    this.add(pAsset19);
    WindowAsset pAsset20 = new WindowAsset();
    pAsset20.id = "debug_avatars";
    pAsset20.icon_path = "iconDebug";
    pAsset20.is_testable = false;
    this.add(pAsset20);
    WindowAsset pAsset21 = new WindowAsset();
    pAsset21.id = "empty";
    pAsset21.icon_path = "iconEmptyLocus";
    this.add(pAsset21);
    WindowAsset pAsset22 = new WindowAsset();
    pAsset22.id = "equipment_rain_editor";
    pAsset22.icon_path = "iconCraftAdamantine";
    pAsset22.preload = true;
    this.add(pAsset22);
    WindowAsset pAsset23 = new WindowAsset();
    pAsset23.id = "error_happened";
    pAsset23.icon_path = "iconDeleteWorld";
    this.add(pAsset23);
    WindowAsset pAsset24 = new WindowAsset();
    pAsset24.id = "error_with_reason";
    pAsset24.icon_path = "iconDeleteWorld";
    this.add(pAsset24);
    WindowAsset pAsset25 = new WindowAsset();
    pAsset25.id = "family";
    pAsset25.related_parent_window = "list_families";
    pAsset25.icon_path = "iconFamily";
    pAsset25.preload = true;
    this.add(pAsset25);
    WindowAsset pAsset26 = new WindowAsset();
    pAsset26.id = "family_customize";
    pAsset26.related_parent_window = "list_families";
    pAsset26.icon_path = "iconColorCustomization";
    this.add(pAsset26);
    WindowAsset pAsset27 = new WindowAsset();
    pAsset27.id = "item";
    pAsset27.related_parent_window = "list_favorite_items";
    pAsset27.icon_path = "iconFavoriteWeapon";
    pAsset27.preload = true;
    this.add(pAsset27);
    WindowAsset pAsset28 = new WindowAsset();
    pAsset28.id = "kingdom";
    pAsset28.related_parent_window = "list_kingdoms";
    pAsset28.icon_path = "iconCrown";
    pAsset28.preload = true;
    this.add(pAsset28);
    WindowAsset pAsset29 = new WindowAsset();
    pAsset29.id = "kingdom_customize";
    pAsset29.related_parent_window = "list_kingdoms";
    pAsset29.icon_path = "iconColorCustomization";
    this.add(pAsset29);
    WindowAsset pAsset30 = new WindowAsset();
    pAsset30.id = "language";
    pAsset30.related_parent_window = "list_languages";
    pAsset30.icon_path = "iconLanguage";
    pAsset30.preload = true;
    this.add(pAsset30);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIconsUnits(SelectedMetas.selected_language.units));
    WindowAsset pAsset31 = new WindowAsset();
    pAsset31.id = "language_customize";
    pAsset31.related_parent_window = "list_languages";
    pAsset31.icon_path = "iconColorCustomization";
    this.add(pAsset31);
    WindowAsset pAsset32 = new WindowAsset();
    pAsset32.id = "list_alliances";
    pAsset32.icon_path = "iconAllianceList";
    pAsset32.preload = true;
    this.add(pAsset32);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconAlliance"));
    WindowAsset pAsset33 = new WindowAsset();
    pAsset33.id = "list_cities";
    pAsset33.icon_path = "iconCityList";
    pAsset33.preload = true;
    this.add(pAsset33);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconCityInspect"));
    WindowAsset pAsset34 = new WindowAsset();
    pAsset34.id = "list_clans";
    pAsset34.icon_path = "iconClanList";
    pAsset34.preload = true;
    this.add(pAsset34);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconClan"));
    WindowAsset pAsset35 = new WindowAsset();
    pAsset35.id = "list_armies";
    pAsset35.icon_path = "iconArmy";
    pAsset35.preload = true;
    this.add(pAsset35);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconArmy"));
    WindowAsset pAsset36 = new WindowAsset();
    pAsset36.id = "list_cultures";
    pAsset36.icon_path = "iconCultureList";
    pAsset36.preload = true;
    this.add(pAsset36);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconCulture"));
    WindowAsset pAsset37 = new WindowAsset();
    pAsset37.id = "list_families";
    pAsset37.icon_path = "iconFamilyList";
    pAsset37.preload = true;
    this.add(pAsset37);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconFamily"));
    WindowAsset pAsset38 = new WindowAsset();
    pAsset38.id = "list_favorite_items";
    pAsset38.icon_path = "iconFavoriteItemsList";
    pAsset38.preload = true;
    this.add(pAsset38);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconFavoriteStar"));
    WindowAsset pAsset39 = new WindowAsset();
    pAsset39.id = "list_favorite_units";
    pAsset39.icon_path = "iconFavoritesList";
    pAsset39.preload = true;
    this.add(pAsset39);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconFavoriteStar"));
    WindowAsset pAsset40 = new WindowAsset();
    pAsset40.id = "list_kingdoms";
    pAsset40.icon_path = "iconKingdomList";
    pAsset40.preload = true;
    this.add(pAsset40);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconCrown"));
    WindowAsset pAsset41 = new WindowAsset();
    pAsset41.id = "list_languages";
    pAsset41.icon_path = "iconLanguageList";
    pAsset41.preload = true;
    this.add(pAsset41);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconLanguage"));
    WindowAsset pAsset42 = new WindowAsset();
    pAsset42.id = "list_knowledge";
    pAsset42.icon_path = "iconKnowledge";
    pAsset42.preload = true;
    this.add(pAsset42);
    WindowAsset pAsset43 = new WindowAsset();
    pAsset43.id = "list_plots";
    pAsset43.icon_path = "iconPlotList";
    pAsset43.preload = true;
    this.add(pAsset43);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconPlot"));
    WindowAsset pAsset44 = new WindowAsset();
    pAsset44.id = "list_religions";
    pAsset44.icon_path = "iconReligionList";
    pAsset44.preload = true;
    this.add(pAsset44);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconReligion"));
    WindowAsset pAsset45 = new WindowAsset();
    pAsset45.id = "list_subspecies";
    pAsset45.icon_path = "iconSubspeciesList";
    pAsset45.preload = true;
    this.add(pAsset45);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconSpecies"));
    WindowAsset pAsset46 = new WindowAsset();
    pAsset46.id = "list_wars";
    pAsset46.icon_path = "iconWarList";
    pAsset46.preload = true;
    this.add(pAsset46);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconWar"));
    WindowAsset pAsset47 = new WindowAsset();
    pAsset47.id = "load_world";
    pAsset47.icon_path = "iconSaveLocal";
    pAsset47.window_toolbar_enabled = false;
    pAsset47.preload = true;
    this.add(pAsset47);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconBox"));
    WindowAsset pAsset48 = new WindowAsset();
    pAsset48.id = "moonbox_promo";
    pAsset48.icon_path = "iconMoonBox";
    pAsset48.window_toolbar_enabled = false;
    this.add(pAsset48);
    WindowAsset pAsset49 = new WindowAsset();
    pAsset49.id = "new_world_templates";
    pAsset49.icon_path = "iconBrowse";
    pAsset49.preload = true;
    this.add(pAsset49);
    WindowAsset pAsset50 = new WindowAsset();
    pAsset50.id = "new_world_templates_2";
    pAsset50.related_parent_window = "new_world_templates";
    pAsset50.icon_path = "iconBrowse";
    this.add(pAsset50);
    WindowAsset pAsset51 = new WindowAsset();
    pAsset51.id = "news";
    pAsset51.icon_path = "iconDocument";
    pAsset51.window_toolbar_enabled = false;
    this.add(pAsset51);
    WindowAsset pAsset52 = new WindowAsset();
    pAsset52.id = "not_found";
    pAsset52.icon_path = "iconDebug";
    pAsset52.window_toolbar_enabled = false;
    this.add(pAsset52);
    WindowAsset pAsset53 = new WindowAsset();
    pAsset53.id = "other";
    pAsset53.icon_path = "iconOptions";
    this.add(pAsset53);
    WindowAsset pAsset54 = new WindowAsset();
    pAsset54.id = "patch_log";
    pAsset54.icon_path = "iconDocument";
    pAsset54.window_toolbar_enabled = false;
    this.add(pAsset54);
    WindowAsset pAsset55 = new WindowAsset();
    pAsset55.id = "plot";
    pAsset55.related_parent_window = "list_plots";
    pAsset55.icon_path = "iconPlot";
    this.add(pAsset55);
    WindowAsset pAsset56 = new WindowAsset();
    pAsset56.id = "premium_menu";
    pAsset56.icon_path = "iconPremium";
    pAsset56.window_toolbar_enabled = false;
    this.add(pAsset56);
    WindowAsset pAsset57 = new WindowAsset();
    pAsset57.id = "premium_help";
    pAsset57.icon_path = "iconPremium";
    pAsset57.window_toolbar_enabled = false;
    this.add(pAsset57);
    WindowAsset pAsset58 = new WindowAsset();
    pAsset58.id = "premium_purchase_error";
    pAsset58.icon_path = "iconDeleteWorld";
    pAsset58.window_toolbar_enabled = false;
    this.add(pAsset58);
    WindowAsset pAsset59 = new WindowAsset();
    pAsset59.id = "premium_unlocked";
    pAsset59.icon_path = "iconPremium";
    pAsset59.window_toolbar_enabled = false;
    this.add(pAsset59);
    WindowAsset pAsset60 = new WindowAsset();
    pAsset60.id = "quit_game";
    pAsset60.icon_path = "iconClose";
    pAsset60.window_toolbar_enabled = false;
    this.add(pAsset60);
    WindowAsset pAsset61 = new WindowAsset();
    pAsset61.id = "rate_us";
    pAsset61.icon_path = "iconHealth";
    pAsset61.window_toolbar_enabled = false;
    this.add(pAsset61);
    WindowAsset pAsset62 = new WindowAsset();
    pAsset62.id = "rate_us_no";
    pAsset62.icon_path = "iconCloudRain";
    pAsset62.window_toolbar_enabled = false;
    this.add(pAsset62);
    WindowAsset pAsset63 = new WindowAsset();
    pAsset63.id = "rate_us_yes";
    pAsset63.icon_path = "iconHealth";
    pAsset63.window_toolbar_enabled = false;
    this.add(pAsset63);
    WindowAsset pAsset64 = new WindowAsset();
    pAsset64.id = "religion";
    pAsset64.related_parent_window = "list_religions";
    pAsset64.icon_path = "iconReligion";
    pAsset64.preload = true;
    this.add(pAsset64);
    WindowAsset pAsset65 = new WindowAsset();
    pAsset65.id = "religion_customize";
    pAsset65.related_parent_window = "list_religions";
    pAsset65.icon_path = "iconColorCustomization";
    this.add(pAsset65);
    WindowAsset pAsset66 = new WindowAsset();
    pAsset66.id = "reward_ads";
    pAsset66.icon_path = "iconAdReward";
    pAsset66.window_toolbar_enabled = false;
    this.add(pAsset66);
    WindowAsset pAsset67 = new WindowAsset();
    pAsset67.id = "reward_ads_power";
    pAsset67.icon_path = "iconAdReward";
    pAsset67.window_toolbar_enabled = false;
    this.add(pAsset67);
    WindowAsset pAsset68 = new WindowAsset();
    pAsset68.id = "reward_ads_received";
    pAsset68.icon_path = "iconAdReward";
    pAsset68.window_toolbar_enabled = false;
    this.add(pAsset68);
    WindowAsset pAsset69 = new WindowAsset();
    pAsset69.id = "reward_ads_saveslot";
    pAsset69.icon_path = "iconAdReward";
    pAsset69.window_toolbar_enabled = false;
    this.add(pAsset69);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconSaveLocal"));
    WindowAsset pAsset70 = new WindowAsset();
    pAsset70.id = "save_delete_confirm";
    pAsset70.related_parent_window = "saves_list";
    pAsset70.window_toolbar_enabled = false;
    pAsset70.icon_path = "iconDeleteWorld";
    this.add(pAsset70);
    WindowAsset pAsset71 = new WindowAsset();
    pAsset71.id = "save_load_confirm";
    pAsset71.related_parent_window = "saves_list";
    pAsset71.window_toolbar_enabled = false;
    pAsset71.icon_path = "iconBox";
    this.add(pAsset71);
    WindowAsset pAsset72 = new WindowAsset();
    pAsset72.id = "save_slot";
    pAsset72.related_parent_window = "saves_list";
    pAsset72.icon_path = "iconSaveLocal";
    this.add(pAsset72);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconBox"));
    WindowAsset pAsset73 = new WindowAsset();
    pAsset73.id = "save_slot_new";
    pAsset73.related_parent_window = "saves_list";
    pAsset73.icon_path = "iconSaveLocal";
    this.add(pAsset73);
    WindowAsset pAsset74 = new WindowAsset();
    pAsset74.id = "save_world_confirm";
    pAsset74.related_parent_window = "saves_list";
    pAsset74.window_toolbar_enabled = false;
    pAsset74.icon_path = "iconSaveLocal";
    this.add(pAsset74);
    WindowAsset pAsset75 = new WindowAsset();
    pAsset75.id = "saves_list";
    pAsset75.icon_path = "iconBrowse";
    pAsset75.preload = true;
    this.add(pAsset75);
    WindowAsset pAsset76 = new WindowAsset();
    pAsset76.id = "settings";
    pAsset76.related_parent_window = "other";
    pAsset76.icon_path = "iconOptions";
    this.add(pAsset76);
    WindowAsset pAsset77 = new WindowAsset();
    pAsset77.id = "settings_old";
    pAsset77.icon_path = "iconOptions";
    pAsset77.is_testable = false;
    this.add(pAsset77);
    WindowAsset pAsset78 = new WindowAsset();
    pAsset78.id = "statistics";
    pAsset78.icon_path = "iconStatistics";
    this.add(pAsset78);
    WindowAsset pAsset79 = new WindowAsset();
    pAsset79.id = "steam";
    pAsset79.icon_path = "iconSteam";
    pAsset79.window_toolbar_enabled = false;
    this.add(pAsset79);
    WindowAsset pAsset80 = new WindowAsset();
    pAsset80.id = "steam_workshop_browse";
    pAsset80.related_parent_window = "steam";
    pAsset80.icon_path = "iconSteam";
    pAsset80.window_toolbar_enabled = false;
    this.add(pAsset80);
    WindowAsset pAsset81 = new WindowAsset();
    pAsset81.id = "steam_workshop_empty";
    pAsset81.related_parent_window = "steam";
    pAsset81.icon_path = "iconSteam";
    pAsset81.window_toolbar_enabled = false;
    this.add(pAsset81);
    WindowAsset pAsset82 = new WindowAsset();
    pAsset82.id = "steam_workshop_main";
    pAsset82.related_parent_window = "steam";
    pAsset82.icon_path = "iconSteam";
    pAsset82.window_toolbar_enabled = false;
    this.add(pAsset82);
    WindowAsset pAsset83 = new WindowAsset();
    pAsset83.id = "steam_workshop_play_world";
    pAsset83.related_parent_window = "steam";
    pAsset83.icon_path = "iconSteam";
    pAsset83.window_toolbar_enabled = false;
    this.add(pAsset83);
    WindowAsset pAsset84 = new WindowAsset();
    pAsset84.id = "steam_workshop_upload_world";
    pAsset84.related_parent_window = "steam";
    pAsset84.icon_path = "iconSteam";
    pAsset84.window_toolbar_enabled = false;
    this.add(pAsset84);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconSaveCloud"));
    WindowAsset pAsset85 = new WindowAsset();
    pAsset85.id = "steam_workshop_uploading";
    pAsset85.related_parent_window = "steam";
    pAsset85.icon_path = "iconSteam";
    pAsset85.window_toolbar_enabled = false;
    this.add(pAsset85);
    WindowAsset pAsset86 = new WindowAsset();
    pAsset86.id = "subspecies";
    pAsset86.related_parent_window = "list_subspecies";
    pAsset86.icon_path = "iconSpecies";
    pAsset86.preload = true;
    this.add(pAsset86);
    WindowAsset pAsset87 = new WindowAsset();
    pAsset87.id = "subspecies_customize";
    pAsset87.related_parent_window = "list_subspecies";
    pAsset87.icon_path = "iconColorCustomization";
    this.add(pAsset87);
    WindowAsset pAsset88 = new WindowAsset();
    pAsset88.id = "thanks_for_testing";
    pAsset88.icon_path = "actor_traits/iconEyePatch";
    pAsset88.window_toolbar_enabled = false;
    this.add(pAsset88);
    WindowAsset pAsset89 = new WindowAsset();
    pAsset89.id = "trait_rain_editor";
    pAsset89.icon_path = "actor_traits/iconDivineScar";
    this.add(pAsset89);
    WindowAsset pAsset90 = new WindowAsset();
    pAsset90.id = "under_development";
    pAsset90.icon_path = "iconDebug";
    this.add(pAsset90);
    WindowAsset pAsset91 = new WindowAsset();
    pAsset91.id = "unit";
    pAsset91.related_parent_window = "list_favorite_units";
    pAsset91.icon_path = "iconInspect";
    pAsset91.preload = true;
    this.add(pAsset91);
    this.t.get_hovering_icons += (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons(SelectedUnit.unit.asset.icon));
    WindowAsset pAsset92 = new WindowAsset();
    pAsset92.id = "update_available";
    pAsset92.icon_path = "iconCrit";
    pAsset92.window_toolbar_enabled = false;
    this.add(pAsset92);
    WindowAsset pAsset93 = new WindowAsset();
    pAsset93.id = "war";
    pAsset93.related_parent_window = "list_wars";
    pAsset93.icon_path = "iconWar";
    pAsset93.preload = true;
    this.add(pAsset93);
    WindowAsset pAsset94 = new WindowAsset();
    pAsset94.id = "welcome";
    pAsset94.window_toolbar_enabled = false;
    pAsset94.icon_path = "iconAye";
    pAsset94.preload = true;
    this.add(pAsset94);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("language_traits/", "culture_traits/", "clan_traits/", "subspecies_traits/", "religion_traits/", "kingdom_traits/"));
    WindowAsset pAsset95 = new WindowAsset();
    pAsset95.id = "world_ages";
    pAsset95.icon_path = "iconAges";
    pAsset95.preload = true;
    this.add(pAsset95);
    WindowAsset pAsset96 = new WindowAsset();
    pAsset96.id = "world_history";
    pAsset96.icon_path = "iconWorldLog";
    pAsset96.preload = true;
    this.add(pAsset96);
    WindowAsset pAsset97 = new WindowAsset();
    pAsset97.id = "world_info";
    pAsset97.icon_path = "iconWorldInfo";
    pAsset97.preload = true;
    this.add(pAsset97);
    WindowAsset pAsset98 = new WindowAsset();
    pAsset98.id = "world_languages";
    pAsset98.related_parent_window = "other";
    pAsset98.icon_path = "iconLanguage";
    this.add(pAsset98);
    this.t.get_hovering_icons = (HoveringBGIconsGetter) (_ => WindowLibrary.getHoveringIcons("iconLanguage"));
    WindowAsset pAsset99 = new WindowAsset();
    pAsset99.id = "world_laws";
    pAsset99.icon_path = "iconWorldLaws";
    pAsset99.preload = true;
    this.add(pAsset99);
  }

  public override void post_init()
  {
    base.post_init();
    ScrollWindow.addCallbackOpen((ScrollWindowNameAction) (_ =>
    {
      ++Config.debug_window_stats.opens;
      HoveringBgIconManager.show();
    }));
    ScrollWindow.addCallbackClose((ScrollWindowAction) (() =>
    {
      ++Config.debug_window_stats.closes;
      HoveringBgIconManager.hide();
    }));
    ScrollWindow.addCallbackShowStarted((ScrollWindowNameAction) (pWindowId =>
    {
      ++Config.debug_window_stats.shows;
      Config.debug_window_stats.setCurrent(pWindowId);
    }));
    ScrollWindow.addCallbackShow((ScrollWindowNameAction) (pWindowId => HoveringBgIconManager.showWindow(this.get(pWindowId))));
    ScrollWindow.addCallbackShowFinished((ScrollWindowNameAction) (_ => ScrollWindow.checkElements()));
    ScrollWindow.addCallbackHide((ScrollWindowNameAction) (_ => ++Config.debug_window_stats.hides));
    foreach (WindowAsset pWindowAsset in this.list)
    {
      if (pWindowAsset.is_testable)
        pWindowAsset.is_testable = this.isTestable(pWindowAsset);
      if (pWindowAsset.is_testable)
        this._testable_windows.Add(pWindowAsset);
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (MetaTypeAsset metaTypeAsset in AssetManager.meta_type_library.list)
    {
      if (!string.IsNullOrEmpty(metaTypeAsset.window_name))
      {
        WindowAsset windowAsset1 = this.get(metaTypeAsset.window_name);
        if (windowAsset1 == null)
        {
          BaseAssetLibrary.logAssetError("WindowAsset not found for MetaTypeAsset ", metaTypeAsset.id);
        }
        else
        {
          windowAsset1.meta_type_asset = metaTypeAsset;
          if (this.has(metaTypeAsset.window_name + "_customize"))
            this.get(metaTypeAsset.window_name + "_customize").meta_type_asset = metaTypeAsset;
          if (windowAsset1.related_parent_window != null)
          {
            WindowAsset windowAsset2 = this.get(windowAsset1.related_parent_window);
            if (windowAsset2 != null)
              windowAsset2.meta_type_asset = metaTypeAsset;
          }
        }
      }
    }
  }

  internal List<WindowAsset> getTestableWindows() => this._testable_windows;

  private bool isTestable(WindowAsset pWindowAsset)
  {
    string id = pWindowAsset.id;
    if (id.Contains("upload") || id.Contains("_testing_") || id.StartsWith("worldnet"))
      return false;
    switch (id)
    {
      case "brushes":
      case "create_custom_world":
      case "create_predefined_world":
      case "debug":
      case "empty":
      case "kingdom_technology":
      case "lsflw2_promo":
      case "moonbox_promo":
      case "more_games":
      case "not_found":
      case "register":
      case "settings_old":
        return false;
      default:
        return true;
    }
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    string[] files = Directory.GetFiles("Assets/Resources/windows", "*.prefab", SearchOption.TopDirectoryOnly);
    using (ListPool<string> listPool = new ListPool<string>())
    {
      foreach (string path in files)
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(path);
        if (this.dict.ContainsKey(withoutExtension))
          listPool.Add(withoutExtension);
        else if (!(withoutExtension == "list_window"))
          BaseAssetLibrary.logAssetError("No associated WindowAsset found for window ", withoutExtension);
      }
      foreach (WindowAsset windowAsset in this.list)
      {
        if (!listPool.Contains(windowAsset.id))
          BaseAssetLibrary.logAssetError("Window prefab not found for WindowAsset ", windowAsset.id);
      }
    }
  }

  private static IEnumerable<string> getHoveringIconsUnits(List<Actor> pUnits)
  {
    HashSet<string> tIcons = new HashSet<string>();
    foreach (Actor pUnit in pUnits)
    {
      string icon = pUnit.asset.icon;
      if (tIcons.Add(icon))
        yield return icon;
    }
  }

  private static IEnumerable<string> getHoveringIcons(params string[] pPaths)
  {
    string[] strArray = pPaths;
    for (int index = 0; index < strArray.Length; ++index)
      yield return strArray[index];
    strArray = (string[]) null;
  }
}
