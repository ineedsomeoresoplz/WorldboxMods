// Decompiled with JetBrains decompiler
// Type: TooltipLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public class TooltipLibrary : AssetLibrary<TooltipAsset>
{
  private BaseStats _base_stats_temp = new BaseStats();
  private const string ARROW_UP_1 = " <size=4>↗</size>";
  private const string ARROW_UP_2 = " <size=4>↗↗</size>";
  private const string ARROW_UP_3 = " <size=4>↗↗↗</size>";
  private const string ARROW_DOWN_1 = " <size=4>↘</size>";
  private const string ARROW_DOWN_2 = " <size=4>↘↘</size>";
  private const string ARROW_DOWN_3 = " <size=4>↘↘↘</size>";

  public override void init()
  {
    base.init();
    TooltipAsset pAsset1 = new TooltipAsset();
    pAsset1.id = "normal";
    pAsset1.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset1);
    TooltipAsset pAsset2 = new TooltipAsset();
    pAsset2.id = "neuron";
    pAsset2.callback = new TooltipShowAction(this.showNeuron);
    pAsset2.callback_text_animated = new TooltipShowAction(this.showNeuron);
    this.add(pAsset2);
    TooltipAsset pAsset3 = new TooltipAsset();
    pAsset3.id = "biome_seed";
    pAsset3.prefab_id = "tooltips/tooltip_biome_seed";
    pAsset3.callback = new TooltipShowAction(this.showBiomeSeed);
    this.add(pAsset3);
    TooltipAsset pAsset4 = new TooltipAsset();
    pAsset4.id = "unit_spawn";
    pAsset4.prefab_id = "tooltips/tooltip_species_spawn";
    pAsset4.callback = new TooltipShowAction(this.showUnitSpawn);
    this.add(pAsset4);
    TooltipAsset pAsset5 = new TooltipAsset();
    pAsset5.id = "unit_species";
    pAsset5.prefab_id = "tooltips/tooltip_species_spawn";
    pAsset5.callback = new TooltipShowAction(this.showUnitSpecies);
    this.add(pAsset5);
    TooltipAsset pAsset6 = new TooltipAsset();
    pAsset6.id = "unit_button";
    pAsset6.prefab_id = "tooltips/tooltip_species_spawn";
    pAsset6.callback = new TooltipShowAction(this.showUnitButton);
    this.add(pAsset6);
    TooltipAsset pAsset7 = new TooltipAsset();
    pAsset7.id = "onomastics_asset";
    pAsset7.callback = new TooltipShowAction(this.showOnomastics);
    this.add(pAsset7);
    TooltipAsset pAsset8 = new TooltipAsset();
    pAsset8.id = "trait";
    pAsset8.prefab_id = "tooltips/tooltip_trait";
    pAsset8.callback = new TooltipShowAction(this.showTrait);
    this.add(pAsset8);
    TooltipAsset pAsset9 = new TooltipAsset();
    pAsset9.id = "culture_trait";
    pAsset9.prefab_id = "tooltips/tooltip_trait";
    pAsset9.callback = new TooltipShowAction(this.showCultureTrait);
    this.add(pAsset9);
    TooltipAsset pAsset10 = new TooltipAsset();
    pAsset10.id = "language_trait";
    pAsset10.prefab_id = "tooltips/tooltip_trait";
    pAsset10.callback = new TooltipShowAction(this.showLanguageTrait);
    this.add(pAsset10);
    TooltipAsset pAsset11 = new TooltipAsset();
    pAsset11.id = "subspecies_trait";
    pAsset11.prefab_id = "tooltips/tooltip_trait";
    pAsset11.callback = new TooltipShowAction(this.showSubspeciesTrait);
    this.add(pAsset11);
    TooltipAsset pAsset12 = new TooltipAsset();
    pAsset12.id = "clan_trait";
    pAsset12.prefab_id = "tooltips/tooltip_trait";
    pAsset12.callback = new TooltipShowAction(this.showClanTrait);
    this.add(pAsset12);
    TooltipAsset pAsset13 = new TooltipAsset();
    pAsset13.id = "religion_trait";
    pAsset13.prefab_id = "tooltips/tooltip_trait";
    pAsset13.callback = new TooltipShowAction(this.showReligionTrait);
    this.add(pAsset13);
    TooltipAsset pAsset14 = new TooltipAsset();
    pAsset14.id = "kingdom_trait";
    pAsset14.prefab_id = "tooltips/tooltip_trait";
    pAsset14.callback = new TooltipShowAction(this.showKingdomTrait);
    this.add(pAsset14);
    TooltipAsset pAsset15 = new TooltipAsset();
    pAsset15.id = "chromosome";
    pAsset15.callback = new TooltipShowAction(this.showChromosome);
    this.add(pAsset15);
    TooltipAsset pAsset16 = new TooltipAsset();
    pAsset16.id = "gene";
    pAsset16.callback = new TooltipShowAction(this.showGene);
    pAsset16.callback_text_animated = new TooltipShowAction(this.showGeneDNASequence);
    this.add(pAsset16);
    TooltipAsset pAsset17 = new TooltipAsset();
    pAsset17.id = "status";
    pAsset17.prefab_id = "tooltips/tooltip_status";
    pAsset17.callback = new TooltipShowAction(this.showStatus);
    this.add(pAsset17);
    TooltipAsset pAsset18 = new TooltipAsset();
    pAsset18.id = "status_updatable";
    pAsset18.prefab_id = "tooltips/tooltip_status";
    pAsset18.callback = new TooltipShowAction(this.showStatus);
    pAsset18.callback_text_animated = new TooltipShowAction(this.showStatus);
    this.add(pAsset18);
    TooltipAsset pAsset19 = new TooltipAsset();
    pAsset19.id = "culture";
    pAsset19.prefab_id = "tooltips/tooltip_culture";
    pAsset19.callback = new TooltipShowAction(this.showCulture);
    this.add(pAsset19);
    TooltipAsset pAsset20 = new TooltipAsset();
    pAsset20.id = "subspecies";
    pAsset20.prefab_id = "tooltips/tooltip_subspecies";
    pAsset20.callback = new TooltipShowAction(this.showSubspecies);
    this.add(pAsset20);
    TooltipAsset pAsset21 = new TooltipAsset();
    pAsset21.id = "family";
    pAsset21.prefab_id = "tooltips/tooltip_family";
    pAsset21.callback = new TooltipShowAction(this.showFamily);
    this.add(pAsset21);
    TooltipAsset pAsset22 = new TooltipAsset();
    pAsset22.id = "language";
    pAsset22.prefab_id = "tooltips/tooltip_language";
    pAsset22.callback = new TooltipShowAction(this.showLanguage);
    this.add(pAsset22);
    TooltipAsset pAsset23 = new TooltipAsset();
    pAsset23.id = "religion";
    pAsset23.prefab_id = "tooltips/tooltip_religion";
    pAsset23.callback = new TooltipShowAction(this.showReligion);
    this.add(pAsset23);
    TooltipAsset pAsset24 = new TooltipAsset();
    pAsset24.id = "book";
    pAsset24.prefab_id = "tooltips/tooltip_book";
    pAsset24.callback = new TooltipShowAction(this.showBook);
    this.add(pAsset24);
    TooltipAsset pAsset25 = new TooltipAsset();
    pAsset25.id = "clan";
    pAsset25.prefab_id = "tooltips/tooltip_clan";
    pAsset25.callback = new TooltipShowAction(this.showClan);
    this.add(pAsset25);
    TooltipAsset pAsset26 = new TooltipAsset();
    pAsset26.id = "army";
    pAsset26.prefab_id = "tooltips/tooltip_army";
    pAsset26.callback = new TooltipShowAction(this.showArmy);
    this.add(pAsset26);
    TooltipAsset pAsset27 = new TooltipAsset();
    pAsset27.id = "alliance";
    pAsset27.prefab_id = "tooltips/tooltip_alliance";
    pAsset27.callback = new TooltipShowAction(this.showAlliance);
    this.add(pAsset27);
    TooltipAsset pAsset28 = new TooltipAsset();
    pAsset28.id = "kingdom";
    pAsset28.prefab_id = "tooltips/tooltip_kingdom";
    pAsset28.callback = new TooltipShowAction(this.showKingdom);
    this.add(pAsset28);
    TooltipAsset pAsset29 = new TooltipAsset();
    pAsset29.id = "kingdom_dead";
    pAsset29.prefab_id = "tooltips/tooltip_kingdom_dead";
    pAsset29.callback = new TooltipShowAction(this.showDeadKingdom);
    this.add(pAsset29);
    TooltipAsset pAsset30 = new TooltipAsset();
    pAsset30.id = "kingdom_diplo";
    pAsset30.callback = new TooltipShowAction(this.showKingdom);
    pAsset30.prefab_id = "tooltips/tooltip_kingdom_opinion";
    this.add(pAsset30);
    this.t.callback += new TooltipShowAction(this.opinionListToStatsDiplomacy);
    TooltipAsset pAsset31 = new TooltipAsset();
    pAsset31.id = "city";
    pAsset31.prefab_id = "tooltips/tooltip_city";
    pAsset31.callback = new TooltipShowAction(this.showCity);
    this.add(pAsset31);
    TooltipAsset pAsset32 = new TooltipAsset();
    pAsset32.id = "plot";
    pAsset32.prefab_id = "tooltips/tooltip_plot";
    pAsset32.callback = new TooltipShowAction(this.showPlot);
    this.add(pAsset32);
    TooltipAsset pAsset33 = new TooltipAsset();
    pAsset33.id = "plot_in_editor";
    pAsset33.prefab_id = "tooltips/tooltip_plot_editor";
    pAsset33.callback = new TooltipShowAction(this.showPlotInEditor);
    this.add(pAsset33);
    TooltipAsset pAsset34 = new TooltipAsset();
    pAsset34.id = "happiness";
    pAsset34.prefab_id = "tooltips/tooltip_happiness";
    pAsset34.callback = new TooltipShowAction(this.showHappiness);
    this.add(pAsset34);
    TooltipAsset pAsset35 = new TooltipAsset();
    pAsset35.id = "city_capital";
    pAsset35.prefab_id = "tooltips/tooltip_city";
    pAsset35.callback = new TooltipShowAction(this.showCityCapital);
    this.add(pAsset35);
    TooltipAsset pAsset36 = new TooltipAsset();
    pAsset36.id = "city_home";
    pAsset36.prefab_id = "tooltips/tooltip_city";
    pAsset36.callback = new TooltipShowAction(this.showCityHome);
    this.add(pAsset36);
    TooltipAsset pAsset37 = new TooltipAsset();
    pAsset37.id = "actor_king";
    pAsset37.prefab_id = "tooltips/tooltip_actor";
    pAsset37.callback = new TooltipShowAction(this.showKing);
    this.add(pAsset37);
    TooltipAsset pAsset38 = new TooltipAsset();
    pAsset38.id = "actor";
    pAsset38.prefab_id = "tooltips/tooltip_actor";
    pAsset38.callback = new TooltipShowAction(this.showActorNormal);
    this.add(pAsset38);
    TooltipAsset pAsset39 = new TooltipAsset();
    pAsset39.id = "actor_leader";
    pAsset39.prefab_id = "tooltips/tooltip_actor";
    pAsset39.callback = new TooltipShowAction(this.showLeader);
    this.add(pAsset39);
    TooltipAsset pAsset40 = new TooltipAsset();
    pAsset40.id = "map_meta";
    pAsset40.callback = new TooltipShowAction(this.showMapMeta);
    this.add(pAsset40);
    TooltipAsset pAsset41 = new TooltipAsset();
    pAsset41.id = "equipment";
    pAsset41.prefab_id = "tooltips/tooltip_equipment";
    pAsset41.callback = new TooltipShowAction(this.showEquipment);
    this.add(pAsset41);
    TooltipAsset pAsset42 = new TooltipAsset();
    pAsset42.id = "equipment_in_editor";
    pAsset42.prefab_id = "tooltips/tooltip_equipment_in_editor";
    pAsset42.callback = new TooltipShowAction(this.showEquipmentInEditor);
    this.add(pAsset42);
    TooltipAsset pAsset43 = new TooltipAsset();
    pAsset43.id = "city_resource";
    pAsset43.callback = new TooltipShowAction(this.showCityResource);
    pAsset43.callback_text_animated = new TooltipShowAction(this.showCityResource);
    this.add(pAsset43);
    TooltipAsset pAsset44 = new TooltipAsset();
    pAsset44.id = "city_resource_food";
    pAsset44.callback = new TooltipShowAction(this.showCityResourceFood);
    pAsset44.callback_text_animated = new TooltipShowAction(this.showCityResourceFood);
    this.add(pAsset44);
    TooltipAsset pAsset45 = new TooltipAsset();
    pAsset45.id = "graph_resource";
    pAsset45.callback = new TooltipShowAction(this.showGraphResource);
    this.add(pAsset45);
    TooltipAsset pAsset46 = new TooltipAsset();
    pAsset46.id = "graph_multi_resource";
    pAsset46.callback = new TooltipShowAction(this.showGraphMultiResource);
    this.add(pAsset46);
    TooltipAsset pAsset47 = new TooltipAsset();
    pAsset47.id = "gender_data";
    pAsset47.callback = new TooltipShowAction(this.showGenderData);
    this.add(pAsset47);
    TooltipAsset pAsset48 = new TooltipAsset();
    pAsset48.id = "war";
    pAsset48.prefab_id = "tooltips/tooltip_war";
    pAsset48.callback = new TooltipShowAction(this.showWar);
    this.add(pAsset48);
    this.t.callback += new TooltipShowAction(this.showWarSides);
    TooltipAsset pAsset49 = new TooltipAsset();
    pAsset49.id = "world_law";
    pAsset49.callback = new TooltipShowAction(this.showWorldLaw);
    this.add(pAsset49);
    TooltipAsset pAsset50 = new TooltipAsset();
    pAsset50.id = "world_age";
    pAsset50.prefab_id = "tooltips/tooltip_world_age";
    pAsset50.callback = new TooltipShowAction(this.showWorldAge);
    this.add(pAsset50);
    TooltipAsset pAsset51 = new TooltipAsset();
    pAsset51.id = "tip";
    pAsset51.callback = new TooltipShowAction(this.showTip);
    this.add(pAsset51);
    TooltipAsset pAsset52 = new TooltipAsset();
    pAsset52.id = "tip_zone_mode";
    pAsset52.callback = new TooltipShowAction(this.showTipZoneMode);
    this.add(pAsset52);
    TooltipAsset pAsset53 = new TooltipAsset();
    pAsset53.id = "stats_icon";
    pAsset53.callback = new TooltipShowAction(this.showTip);
    this.add(pAsset53);
    this.t.callback += new TooltipShowAction(this.showStatsData);
    TooltipAsset pAsset54 = new TooltipAsset();
    pAsset54.id = "debug_kingdom_assets";
    pAsset54.callback = new TooltipShowAction(this.showKingdomAsset);
    this.add(pAsset54);
    TooltipAsset pAsset55 = new TooltipAsset();
    pAsset55.id = "mass";
    pAsset55.callback = new TooltipShowAction(this.showMass);
    this.add(pAsset55);
    TooltipAsset pAsset56 = new TooltipAsset();
    pAsset56.id = "past_rulers";
    pAsset56.prefab_id = "tooltips/tooltip_past_rulers";
    pAsset56.callback = new TooltipShowAction(this.showPastRulers);
    this.add(pAsset56);
    TooltipAsset pAsset57 = new TooltipAsset();
    pAsset57.id = "past_names";
    pAsset57.prefab_id = "tooltips/tooltip_past_rulers";
    pAsset57.callback = new TooltipShowAction(this.showPastNames);
    this.add(pAsset57);
    TooltipAsset pAsset58 = new TooltipAsset();
    pAsset58.id = "carrying_resources";
    pAsset58.callback = new TooltipShowAction(this.showCarryingResources);
    this.add(pAsset58);
    TooltipAsset pAsset59 = new TooltipAsset();
    pAsset59.id = "passengers";
    pAsset59.prefab_id = "tooltips/tooltip_passengers";
    pAsset59.callback = new TooltipShowAction(this.showPassengers);
    this.add(pAsset59);
    TooltipAsset pAsset60 = new TooltipAsset();
    pAsset60.id = "loyalty";
    pAsset60.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset60);
    this.t.callback += new TooltipShowAction(this.showLoyalty);
    this.t.callback += new TooltipShowAction(this.opinionListToStatsLoyalty);
    TooltipAsset pAsset61 = new TooltipAsset();
    pAsset61.id = "taxonomy";
    pAsset61.prefab_id = "tooltips/tooltip_taxonomy";
    pAsset61.callback = new TooltipShowAction(this.showTaxonomy);
    this.add(pAsset61);
    TooltipAsset pAsset62 = new TooltipAsset();
    pAsset62.id = "achievement";
    pAsset62.prefab_id = "tooltips/tooltip_achievement";
    pAsset62.callback = new TooltipShowAction(this.showAchievement);
    this.add(pAsset62);
    TooltipAsset pAsset63 = new TooltipAsset();
    pAsset63.id = "color_counter";
    pAsset63.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset63);
    this.t.callback += new TooltipShowAction(this.showColorCounter);
    TooltipAsset pAsset64 = new TooltipAsset();
    pAsset64.id = "game_language";
    pAsset64.callback = new TooltipShowAction(this.showGameLanguage);
    this.add(pAsset64);
    this.addMetaListButtonTooltips();
    this.initDebug();
  }

  private void showMetaInfo(Tooltip pTooltip, string pAssetId, string pStatisticID)
  {
    MetaTypeAsset metaTypeAsset = AssetManager.meta_type_library.get(pAssetId);
    int pMainVal = 0;
    foreach (NanoObject pObject in metaTypeAsset.get_list())
    {
      if (!pObject.isRekt() && !pObject.hasDied())
        ++pMainVal;
    }
    this.setIconValue(pTooltip, "i_total", (float) pMainVal);
    this.setIconValue(pTooltip, "i_destroyed", (float) StatsHelper.getStat(pStatisticID));
    this.setIconSprite(pTooltip, "i_total", metaTypeAsset.icon_list);
  }

  private void addMetaListButtonTooltips()
  {
    TooltipAsset pAsset1 = new TooltipAsset();
    pAsset1.id = "tooltip_meta_list_subspecies";
    pAsset1.prefab_id = "tooltips/tooltip_meta_list";
    pAsset1.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset1);
    this.t.callback += (TooltipShowAction) ((pTooltip, _1, _2) => this.showMetaInfo(pTooltip, "subspecies", "world_statistics_subspecies_extinct"));
    TooltipAsset pAsset2 = new TooltipAsset();
    pAsset2.id = "tooltip_meta_list_languages";
    pAsset2.prefab_id = "tooltips/tooltip_meta_list";
    pAsset2.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset2);
    this.t.callback += (TooltipShowAction) ((pTooltip, _3, _4) => this.showMetaInfo(pTooltip, "language", "world_statistics_languages_forgotten"));
    TooltipAsset pAsset3 = new TooltipAsset();
    pAsset3.id = "tooltip_meta_list_families";
    pAsset3.prefab_id = "tooltips/tooltip_meta_list";
    pAsset3.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset3);
    this.t.callback += (TooltipShowAction) ((pTooltip, _5, _6) => this.showMetaInfo(pTooltip, "family", "world_statistics_families_destroyed"));
    TooltipAsset pAsset4 = new TooltipAsset();
    pAsset4.id = "tooltip_meta_list_cultures";
    pAsset4.prefab_id = "tooltips/tooltip_meta_list";
    pAsset4.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset4);
    this.t.callback += (TooltipShowAction) ((pTooltip, _7, _8) => this.showMetaInfo(pTooltip, "culture", "world_statistics_cultures_forgotten"));
    TooltipAsset pAsset5 = new TooltipAsset();
    pAsset5.id = "tooltip_meta_list_religions";
    pAsset5.prefab_id = "tooltips/tooltip_meta_list";
    pAsset5.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset5);
    this.t.callback += (TooltipShowAction) ((pTooltip, _9, _10) => this.showMetaInfo(pTooltip, "religion", "world_statistics_religions_forgotten"));
    TooltipAsset pAsset6 = new TooltipAsset();
    pAsset6.id = "tooltip_meta_list_clans";
    pAsset6.prefab_id = "tooltips/tooltip_meta_list";
    pAsset6.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset6);
    this.t.callback += (TooltipShowAction) ((pTooltip, _11, _12) => this.showMetaInfo(pTooltip, "clan", "world_statistics_clans_destroyed"));
    TooltipAsset pAsset7 = new TooltipAsset();
    pAsset7.id = "tooltip_meta_list_cities";
    pAsset7.prefab_id = "tooltips/tooltip_meta_list";
    pAsset7.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset7);
    this.t.callback += (TooltipShowAction) ((pTooltip, _13, _14) => this.showMetaInfo(pTooltip, "city", "world_statistics_cities_destroyed"));
    TooltipAsset pAsset8 = new TooltipAsset();
    pAsset8.id = "tooltip_meta_list_kingdoms";
    pAsset8.prefab_id = "tooltips/tooltip_meta_list";
    pAsset8.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset8);
    this.t.callback += (TooltipShowAction) ((pTooltip, _15, _16) => this.showMetaInfo(pTooltip, "kingdom", "world_statistics_kingdoms_destroyed"));
    TooltipAsset pAsset9 = new TooltipAsset();
    pAsset9.id = "tooltip_meta_list_armies";
    pAsset9.prefab_id = "tooltips/tooltip_meta_list";
    pAsset9.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset9);
    this.t.callback += (TooltipShowAction) ((pTooltip, _17, _18) => this.showMetaInfo(pTooltip, "army", "world_statistics_armies_destroyed"));
    TooltipAsset pAsset10 = new TooltipAsset();
    pAsset10.id = "tooltip_meta_list_alliances";
    pAsset10.prefab_id = "tooltips/tooltip_meta_list";
    pAsset10.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset10);
    this.t.callback += (TooltipShowAction) ((pTooltip, _19, _20) => this.showMetaInfo(pTooltip, "alliance", "world_statistics_alliances_made"));
    TooltipAsset pAsset11 = new TooltipAsset();
    pAsset11.id = "tooltip_meta_list_wars";
    pAsset11.prefab_id = "tooltips/tooltip_meta_list";
    pAsset11.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset11);
    this.t.callback += (TooltipShowAction) ((pTooltip, _21, _22) => this.showMetaInfo(pTooltip, "war", "world_statistics_peaces_made"));
    TooltipAsset pAsset12 = new TooltipAsset();
    pAsset12.id = "tooltip_meta_list_plots";
    pAsset12.prefab_id = "tooltips/tooltip_meta_list";
    pAsset12.callback = new TooltipShowAction(this.showNormal);
    this.add(pAsset12);
    this.t.callback += (TooltipShowAction) ((pTooltip, _23, _24) => this.showMetaInfo(pTooltip, "plot", "world_statistics_plots_succeeded"));
  }

  private void showNormal(Tooltip pTooltip, string pType, TooltipData pData)
  {
    if (!string.IsNullOrEmpty(pData.tip_name))
      pTooltip.name.text = pData.tip_name.Localize();
    if (!string.IsNullOrEmpty(pData.tip_description))
    {
      string pDescription = pData.tip_description.Localize();
      pTooltip.setDescription(pDescription);
    }
    if (!Config.isComputer && !Config.isEditor)
      return;
    string str = pData.tip_description_2;
    if (string.IsNullOrEmpty(str))
      str = pData.tip_description + "_2";
    if (string.IsNullOrEmpty(str) || !LocalizedTextManager.stringExists(str))
      return;
    string pText = str.Localize();
    string pDescription1 = AssetManager.hotkey_library.replaceSpecialTextKeys(pText);
    pTooltip.setBottomDescription(pDescription1);
  }

  private void showNeuron(Tooltip pTooltip, string pType, TooltipData pData)
  {
    NeuronElement neuron = pData.neuron;
    DecisionAsset decision = neuron.decision;
    Actor actor = neuron.actor;
    NeuralLayerAsset asset = decision.priority.GetAsset();
    pTooltip.clearTextRows();
    pTooltip.setTitle(decision.getLocalizedText(), "neuron", asset.color_hex);
    if (decision.unique)
      ((Graphic) pTooltip.name).color = RarityLibrary.legendary.color_container.color;
    else
      ((Graphic) pTooltip.name).color = RarityLibrary.rare.color_container.color;
    pTooltip.setDescription("neuron_description".Localize());
    bool flag = actor.isDecisionEnabled(decision.decision_index);
    pTooltip.addLineText("neuron_state", flag ? LocalizedTextManager.getText("neuron_active") : LocalizedTextManager.getText("neuron_silenced"), flag ? "#43FF43" : "#FB2C21");
    pTooltip.addLineBreak();
    pTooltip.addLineText("neuro_layer", asset.getLocaleID().Localize(), asset.color_hex);
    pTooltip.addLineText("neuro_layer_priority", asset.getDescriptionID().Localize(), asset.color_hex);
    pTooltip.addLineText("neuron_firing_rate", decision.getFiringRate());
    pTooltip.addLineText("neuron_cooldown", neuron.getSimulatedTimer().ToText() + "s");
    if (!actor.isDecisionOnCooldown(decision.decision_index, (double) decision.cooldown))
      return;
    pTooltip.resetBottomDescription();
    pTooltip.addBottomDescription("neuron_on_refractory_period".Localize());
  }

  private void showBiomeSeed(Tooltip pTooltip, string pType, TooltipData pData)
  {
    GodPower power = pData.power;
    string biomeId = AssetManager.drops.get(power.drop_id).cached_drop_type_low.biome_id;
    BiomeAsset biomeAsset = AssetManager.biome_library.get(biomeId);
    using (ListPool<string> listPool = new ListPool<string>())
    {
      TooltipIconsRow component = ((Component) ((Component) pTooltip).transform.FindRecursive("Traits")).GetComponent<TooltipIconsRow>();
      bool flag = false | this.showBiomeTraits<ActorTrait>(biomeAsset.spawn_trait_actor, (BaseTraitLibrary<ActorTrait>) AssetManager.traits, component, pTooltip, pData) | this.showBiomeTraits<SubspeciesTrait>(biomeAsset.spawn_trait_subspecies, (BaseTraitLibrary<SubspeciesTrait>) AssetManager.subspecies_traits, component, pTooltip, pData) | this.showBiomeTraits<SubspeciesTrait>(biomeAsset.evolution_trait_subspecies, (BaseTraitLibrary<SubspeciesTrait>) AssetManager.subspecies_traits, component, pTooltip, pData) | this.showBiomeTraits<SubspeciesTrait>(biomeAsset.spawn_trait_subspecies_always, (BaseTraitLibrary<SubspeciesTrait>) AssetManager.subspecies_traits, component, pTooltip, pData) | this.showBiomeTraits<CultureTrait>(biomeAsset.spawn_trait_culture, (BaseTraitLibrary<CultureTrait>) AssetManager.culture_traits, component, pTooltip, pData) | this.showBiomeTraits<LanguageTrait>(biomeAsset.spawn_trait_language, (BaseTraitLibrary<LanguageTrait>) AssetManager.language_traits, component, pTooltip, pData) | this.showBiomeTraits<ReligionTrait>(biomeAsset.spawn_trait_religion, (BaseTraitLibrary<ReligionTrait>) AssetManager.religion_traits, component, pTooltip, pData) | this.showBiomeTraits<ClanTrait>(biomeAsset.spawn_trait_clan, (BaseTraitLibrary<ClanTrait>) AssetManager.clan_traits, component, pTooltip, pData);
      ((Component) component).gameObject.SetActive(flag);
      if (flag)
        component.init(pTooltip, pData);
      if (pTooltip.pool_icons == null)
      {
        Transform recursive = ((Component) pTooltip).transform.FindRecursive("Species");
        StatsIcon pPrefab = Resources.Load<StatsIcon>("ui/PrefabTextIconTooltipBig");
        pTooltip.pool_icons = new ObjectPoolGenericMono<StatsIcon>(pPrefab, recursive);
      }
      listPool.Clear();
      if (biomeAsset.pot_units_spawn != null)
      {
        foreach (string pId in biomeAsset.pot_units_spawn)
        {
          if (!listPool.Contains(pId))
          {
            listPool.Add(pId);
            this.showBiomeSeedUnit(pId, pTooltip);
          }
        }
      }
      if (WorldLawLibrary.world_law_drop_of_thoughts.isEnabled() && biomeAsset.pot_sapient_units_spawn != null)
      {
        foreach (string pId in biomeAsset.pot_sapient_units_spawn)
        {
          if (!listPool.Contains(pId))
          {
            listPool.Add(pId);
            this.showBiomeSeedUnit(pId, pTooltip);
          }
        }
      }
      this.showNormal(pTooltip, pType, pData);
    }
  }

  private void showBiomeSeedUnit(string pId, Tooltip pTooltip)
  {
    StatsIcon next = pTooltip.pool_icons.getNext();
    Image icon = next.getIcon();
    ActorAsset actorAsset = AssetManager.actor_library.get(pId);
    icon.sprite = actorAsset.getSpriteIcon();
    next.text.text = actorAsset.getTranslatedName();
    if (actorAsset.isAvailable())
      ((Graphic) icon).color = Toolbox.color_white;
    else
      ((Graphic) icon).color = Toolbox.color_black;
  }

  private bool showBiomeTraits<T>(
    List<string> pTraits,
    BaseTraitLibrary<T> pLibrary,
    TooltipIconsRow pRow,
    Tooltip pTooltip,
    TooltipData pData)
    where T : BaseTrait<T>
  {
    int num;
    if (pTraits == null)
    {
      num = 1;
    }
    else
    {
      // ISSUE: explicit non-virtual call
      int count = __nonvirtual (pTraits.Count);
      num = 0;
    }
    if (num != 0)
      return false;
    foreach (string pTrait in pTraits)
    {
      T obj = pLibrary.get(pTrait);
      string pColor = obj.isAvailable() ? "#FFFFFF" : "#000000";
      Sprite sprite = obj.getSprite();
      pRow.addIcon(sprite, pColor);
    }
    return true;
  }

  private void showUnitSpawn(Tooltip pTooltip, string pType, TooltipData pData)
  {
    GodPower power = pData.power;
    string actorAssetId = power.getActorAssetID();
    bool unitStatsOverview = power.show_unit_stats_overview;
    this.showUnitGeneric(pTooltip, pData, actorAssetId, unitStatsOverview);
    this.checkDebugSpeciesRows(pTooltip, pData, actorAssetId);
  }

  private void checkDebugSpeciesRows(Tooltip pTooltip, TooltipData pData, string pActorAssetID)
  {
    ActorAsset actorAsset = AssetManager.actor_library.get(pActorAssetID);
    this.showDebugRowsIcons<ActorTrait>(pTooltip, pData, "IconsRowActor", actorAsset.traits, (BaseTraitLibrary<ActorTrait>) AssetManager.traits);
    this.showDebugRowsIcons<SubspeciesTrait>(pTooltip, pData, "IconsRowSubspecies", actorAsset.default_subspecies_traits, (BaseTraitLibrary<SubspeciesTrait>) AssetManager.subspecies_traits);
    this.showDebugRowsIcons<ClanTrait>(pTooltip, pData, "IconsRowClan", actorAsset.default_clan_traits, (BaseTraitLibrary<ClanTrait>) AssetManager.clan_traits);
    this.showDebugRowsIcons<LanguageTrait>(pTooltip, pData, "IconsRowLanguage", actorAsset.default_language_traits, (BaseTraitLibrary<LanguageTrait>) AssetManager.language_traits);
    this.showDebugRowsIcons<CultureTrait>(pTooltip, pData, "IconsRowCulture", actorAsset.default_culture_traits, (BaseTraitLibrary<CultureTrait>) AssetManager.culture_traits);
    this.showDebugRowsIcons<ReligionTrait>(pTooltip, pData, "IconsRowReligion", actorAsset.default_religion_traits, (BaseTraitLibrary<ReligionTrait>) AssetManager.religion_traits);
  }

  private void showDebugRowsIcons<TTraitType>(
    Tooltip pTooltip,
    TooltipData pData,
    string pRowName,
    List<string> pTraitsList,
    BaseTraitLibrary<TTraitType> pTraitLibrary)
    where TTraitType : BaseTrait<TTraitType>
  {
    TooltipIconsRow component = ((Component) ((Component) pTooltip).transform.FindRecursive(pRowName)).GetComponent<TooltipIconsRow>();
    bool flag = DebugConfig.isOn(DebugOption.DebugPowerBarTooltipSpeciesTraits);
    if (pTraitsList != null & flag)
    {
      foreach (string pTraits in pTraitsList)
      {
        TTraitType traitType = pTraitLibrary.get(pTraits);
        component.addIcon(traitType.getSprite());
      }
    }
    component.init(pTooltip, pData);
  }

  private void showUnitSpecies(Tooltip pTooltip, string pType, TooltipData pData)
  {
    string actorAssetId = pData.power.getActorAssetID();
    this.showUnitGeneric(pTooltip, pData, actorAssetId, true, false);
    this.checkDebugSpeciesRows(pTooltip, pData, actorAssetId);
  }

  private void showUnitButton(Tooltip pTooltip, string pType, TooltipData pData)
  {
    string id = pData.actor_asset.id;
    this.showUnitGeneric(pTooltip, pData, id, true);
    this.checkDebugSpeciesRows(pTooltip, pData, id);
  }

  private void showUnitGeneric(
    Tooltip pTooltip,
    TooltipData pData,
    string pActorAssetId,
    bool pShowStatsOverview,
    bool pShowStats = true)
  {
    Transform recursive = ((Component) pTooltip).transform.FindRecursive("Stats");
    bool flag = false;
    if (pShowStatsOverview && !string.IsNullOrEmpty(pActorAssetId))
    {
      ActorAsset pAsset = AssetManager.actor_library.get(pActorAssetId);
      if (pAsset != null)
      {
        pTooltip.name.text = pAsset.getLocalizedName();
        pTooltip.setDescription(pAsset.getLocalizedDescription());
        if (!pAsset.isAvailable())
        {
          ((Component) recursive).gameObject.SetActive(false);
          return;
        }
        if (pShowStats && DebugConfig.isOn(DebugOption.DebugPowerBarTooltipSpeciesTraits))
          BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, pAsset.getStatsForOverview(), false);
        if (pAsset.can_have_subspecies)
        {
          flag = true;
          this.setIconValue(pTooltip, "i_population", (float) pAsset.countPopulation());
          this.setIconValue(pTooltip, "i_subspecies", (float) pAsset.countSubspecies());
          this.setIconValue(pTooltip, "i_families", (float) pAsset.countFamilies());
        }
        if (DebugConfig.isOn(DebugOption.DebugPowerBarTooltipTaxonomy))
          this.showDebugTaxonomy(pTooltip, pAsset);
        if (DebugConfig.isOn(DebugOption.DebugPowerBarTooltipStartingCivMetas))
          this.showDebugTraits(pTooltip, pAsset);
      }
    }
    ((Component) recursive).gameObject.SetActive(flag);
    if (!string.IsNullOrEmpty(pData.tip_description))
    {
      string pDescription = LocalizedTextManager.getText(pData.tip_description).Replace("$lifeissimhours$", 24f.ToText());
      pTooltip.setDescription(pDescription);
    }
    if (!Config.isComputer && !Config.isEditor || string.IsNullOrEmpty(pData.tip_description_2))
      return;
    string text = LocalizedTextManager.getText(pData.tip_description_2);
    string pDescription1 = AssetManager.hotkey_library.replaceSpecialTextKeys(text);
    pTooltip.setBottomDescription(pDescription1);
  }

  private void showDebugTraits(Tooltip pTooltip, ActorAsset pAsset)
  {
    pTooltip.addLineBreak();
    if (pAsset.default_language_traits != null)
    {
      pTooltip.addLineIntText("language_traits", pAsset.default_language_traits.Count, "#4CCFFF", false);
      foreach (string defaultLanguageTrait in pAsset.default_language_traits)
        pTooltip.addLineText("trait", defaultLanguageTrait, "#4CCFFF", pLocalize: false);
    }
    else
      pTooltip.addLineText("language_traits", "-----", "#4CCFFF", pLocalize: false);
    if (pAsset.default_clan_traits != null)
    {
      pTooltip.addLineIntText("clan_traits", pAsset.default_clan_traits.Count, "#FF637D", false);
      foreach (string defaultClanTrait in pAsset.default_clan_traits)
        pTooltip.addLineText("trait", defaultClanTrait, "#4CCFFF", pLocalize: false);
    }
    else
      pTooltip.addLineText("clan_traits", "-----", "#FF637D", pLocalize: false);
    if (pAsset.default_culture_traits != null)
    {
      pTooltip.addLineIntText("culture_traits", pAsset.default_culture_traits.Count, "#35CC6E", false);
      foreach (string defaultCultureTrait in pAsset.default_culture_traits)
        pTooltip.addLineText("trait", defaultCultureTrait, "#35CC6E", pLocalize: false);
    }
    else
      pTooltip.addLineText("culture_traits", "-----", "#35CC6E", pLocalize: false);
    if (pAsset.default_religion_traits != null)
    {
      pTooltip.addLineIntText("religions_traits", pAsset.default_religion_traits.Count, "#8CFF99", false);
      foreach (string defaultReligionTrait in pAsset.default_religion_traits)
        pTooltip.addLineText("trait", defaultReligionTrait, "#8CFF99", pLocalize: false);
    }
    else
      pTooltip.addLineText("religions_traits", "-----", "#8CFF99", pLocalize: false);
  }

  private void showDebugTaxonomy(Tooltip pTooltip, ActorAsset pAsset)
  {
    pTooltip.addLineBreak();
    pTooltip.addLineText("kingdom", pAsset.getTaxonomyRank("taxonomy_kingdom"), ColorStyleLibrary.m.taxonomy_kingdom, pLocalize: false);
    pTooltip.addLineText("phylum", pAsset.getTaxonomyRank("taxonomy_phylum"), ColorStyleLibrary.m.taxonomy_phylum, pLocalize: false);
    pTooltip.addLineText("class", pAsset.getTaxonomyRank("taxonomy_class"), ColorStyleLibrary.m.taxonomy_class, pLocalize: false);
    pTooltip.addLineText("order", pAsset.getTaxonomyRank("taxonomy_order"), ColorStyleLibrary.m.taxonomy_order, pLocalize: false);
    pTooltip.addLineText("family", pAsset.getTaxonomyRank("taxonomy_family"), ColorStyleLibrary.m.taxonomy_family, pLocalize: false);
    pTooltip.addLineText("genus", pAsset.getTaxonomyRank("taxonomy_genus"), ColorStyleLibrary.m.taxonomy_genus, pLocalize: false);
    pTooltip.addLineText("species", pAsset.getTaxonomyRank("taxonomy_species"), ColorStyleLibrary.m.taxonomy_genus, pLocalize: false);
  }

  private void showDeadKingdom(Tooltip pTooltip, string pType, TooltipData pData)
  {
    DeadKingdom kingdom = (DeadKingdom) pData.kingdom;
    pTooltip.setSpeciesIcon(kingdom.getSpeciesIcon());
    pTooltip.setTitle(kingdom.name, "kingdom", kingdom.getColor().color_text);
    this.setIconValue(pTooltip, "i_age", (float) kingdom.getAge(), pColor: "#FF637D");
    this.setIconValue(pTooltip, "i_population", (float) kingdom.getPopulationPeople(), pColor: "#FF637D");
    this.setIconValue(pTooltip, "i_army", (float) kingdom.countTotalWarriors(), pColor: "#FF637D");
    pTooltip.setDescription(kingdom.getMotto());
    pTooltip.addLineText("founded", kingdom.getFoundedYear());
    pTooltip.addLineText("kingdom_died_at", kingdom.getDiedYear(), "#FF637D");
    pTooltip.addLineIntText("age", kingdom.getAge());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("births", kingdom.getTotalBirths());
    pTooltip.addLineIntText("deaths", kingdom.getTotalDeaths());
    pTooltip.addLineIntText("kills", kingdom.getTotalKills());
    pTooltip.addLineBreak();
    pTooltip.addLineText("species", kingdom.getActorAsset().getTranslatedName());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<KingdomBanner>())
      bannerBase.load((NanoObject) kingdom);
  }

  private void showKingdom(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Kingdom kingdom = pData.kingdom;
    pTooltip.setSpeciesIcon(kingdom.getSpeciesIcon());
    string colorText = kingdom.getColor().color_text;
    pTooltip.setTitle(kingdom.name, "kingdom", kingdom.getColor().color_text);
    ((Component) ((Component) pTooltip).transform.FindRecursive("Stats")).gameObject.SetActive(true);
    this.setIconValue(pTooltip, "i_age", (float) kingdom.getAge());
    this.setIconValue(pTooltip, "i_population", (float) kingdom.getPopulationPeople());
    this.setIconValue(pTooltip, "i_army", (float) kingdom.countTotalWarriors());
    pTooltip.setDescription(kingdom.getMotto());
    string pValue1 = "-";
    if (kingdom.hasKing())
      pValue1 = kingdom.king.getName();
    pTooltip.addLineText("village_statistics_king", pValue1, colorText);
    if (kingdom.hasKing())
      pTooltip.addLineIntText("ruler_money", kingdom.king.money);
    pTooltip.addLineBreak();
    pTooltip.addLineText("villages", $"{kingdom.cities.Count.ToText()}/{kingdom.getMaxCities().ToText()}");
    pTooltip.addLineIntText("adults", kingdom.countAdults());
    pTooltip.addLineIntText("children", kingdom.countChildren());
    pTooltip.addLineIntText("families", kingdom.countFamilies());
    pTooltip.addLineIntText("happy", kingdom.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("food", kingdom.countTotalFood());
    pTooltip.addLineBreak();
    string pValue2 = "-";
    if (kingdom.hasCapital())
      pValue2 = kingdom.capital.name;
    pTooltip.addLineText("kingdom_statistics_capital", pValue2, colorText);
    if (kingdom.hasKing() && kingdom.king.hasClan())
      pTooltip.addLineText("clan", kingdom.king.clan.data.name, kingdom.king.clan.getColor().color_text);
    if (kingdom.hasCulture())
      pTooltip.addLineText("culture", kingdom.culture.data.name, kingdom.culture.getColor().color_text);
    if (kingdom.hasLanguage())
      pTooltip.addLineText("language", kingdom.language.data.name, kingdom.language.getColor().color_text);
    if (kingdom.hasReligion())
      pTooltip.addLineText("religion", kingdom.religion.data.name, kingdom.religion.getColor().color_text);
    Alliance alliance = kingdom.getAlliance();
    if (alliance != null)
    {
      int yearsSince = Date.getYearsSince(kingdom.data.timestamp_alliance);
      pTooltip.addLineText("alliance", alliance.data.name, alliance.getColor().color_text);
      pTooltip.addLineIntText("kingdom_time_in_alliance", yearsSince, alliance.getColor().color_text);
    }
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("births", kingdom.getTotalBirths());
    pTooltip.addLineIntText("deaths", kingdom.getTotalDeaths());
    pTooltip.addLineIntText("kills", kingdom.getTotalKills());
    pTooltip.addLineBreak();
    pTooltip.addLineText("species", kingdom.getActorAsset().getTranslatedName());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<KingdomBanner>())
      bannerBase.load((NanoObject) kingdom);
    TooltipKingdomTraitsRow componentInChildren = ((Component) pTooltip).GetComponentInChildren<TooltipKingdomTraitsRow>(true);
    if (Object.op_Inequality((Object) componentInChildren, (Object) null))
      componentInChildren.init(pTooltip, pData);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showStatus(Tooltip pTooltip, string pType, TooltipData pData)
  {
    StatBar component = ((Component) ((Component) pTooltip).transform.FindRecursive("TimeBar")).GetComponent<StatBar>();
    StatusAsset asset = pData.status.asset;
    if (!string.IsNullOrEmpty(pData.tip_name))
      pTooltip.name.text = LocalizedTextManager.getText(pData.tip_name);
    if (!string.IsNullOrEmpty(pData.tip_description))
      pTooltip.setDescription(LocalizedTextManager.getText(pData.tip_description));
    if (asset == null)
      return;
    Status status = pData.status;
    component.setBar((float) (int) status.getRemainingTime(), status.duration, "s", false);
    pTooltip.clearTextRows();
    BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, asset.base_stats);
  }

  private void showOnomastics(Tooltip pTooltip, string pType, TooltipData pData)
  {
    OnomasticsAsset onomasticsAsset = pData.onomastics_asset;
    string localeId = onomasticsAsset.getLocaleID();
    string descriptionId = onomasticsAsset.getDescriptionID();
    string idSubname = onomasticsAsset.getIDSubname();
    string text1 = LocalizedTextManager.getText(localeId);
    pTooltip.setTitle(text1, idSubname, onomasticsAsset.color_text);
    string str = "";
    if (onomasticsAsset.isGroupType() && !pData.onomastics_data.isGroupEmpty(onomasticsAsset.id))
    {
      string lower = pData.onomastics_data.getGroup(onomasticsAsset.id).characters_string.ToLower();
      str = $"{str}[ {Toolbox.coloredText(lower, onomasticsAsset.color_text)} ]\n\n";
    }
    string pDescription = str + LocalizedTextManager.getText(descriptionId);
    pTooltip.setDescription(pDescription);
    string descriptionId2 = onomasticsAsset.getDescriptionID2();
    if (string.IsNullOrEmpty(descriptionId2))
      return;
    string text2 = LocalizedTextManager.getText(descriptionId2);
    pTooltip.setBottomDescription(text2);
  }

  private void showTrait(Tooltip pTooltip, string pType, TooltipData pData)
  {
    ActorTrait trait = pData.trait;
    this.showGenericInfoForTrait<ActorTrait>(pTooltip, pData, trait);
  }

  private void showKingdomTrait(Tooltip pTooltip, string pType, TooltipData pData)
  {
    KingdomTrait kingdomTrait = pData.kingdom_trait;
    this.showGenericInfoForTrait<KingdomTrait>(pTooltip, pData, kingdomTrait);
  }

  private void showCultureTrait(Tooltip pTooltip, string pType, TooltipData pData)
  {
    CultureTrait cultureTrait = pData.culture_trait;
    this.showGenericInfoForTrait<CultureTrait>(pTooltip, pData, cultureTrait);
  }

  private void showLanguageTrait(Tooltip pTooltip, string pType, TooltipData pData)
  {
    LanguageTrait languageTrait = pData.language_trait;
    this.showGenericInfoForTrait<LanguageTrait>(pTooltip, pData, languageTrait);
  }

  private void showSubspeciesTrait(Tooltip pTooltip, string pType, TooltipData pData)
  {
    SubspeciesTrait subspeciesTrait = pData.subspecies_trait;
    this.showGenericInfoForTrait<SubspeciesTrait>(pTooltip, pData, subspeciesTrait);
  }

  private void showClanTrait(Tooltip pTooltip, string pType, TooltipData pData)
  {
    ClanTrait clanTrait = pData.clan_trait;
    this.showGenericInfoForTrait<ClanTrait>(pTooltip, pData, clanTrait, clanTrait.base_stats_male, clanTrait.base_stats_female);
  }

  private void showReligionTrait(Tooltip pTooltip, string pType, TooltipData pData)
  {
    ReligionTrait religionTrait = pData.religion_trait;
    this.showGenericInfoForTrait<ReligionTrait>(pTooltip, pData, religionTrait);
  }

  private void showGenericInfoForTrait<T>(
    Tooltip pTooltip,
    TooltipData pData,
    T pTrait,
    params BaseStats[] pAdditionalBaseStats)
    where T : BaseTrait<T>
  {
    this.showTraitOwners<T>(pTooltip, pTrait);
    bool flag = !pData.is_editor_augmentation_button || pTrait.isAvailable();
    Rarity rarity = pTrait.rarity;
    string str1 = rarity.getAsset().getLocaleID().Localize();
    string text = LocalizedTextManager.getText(flag ? pTrait.getLocaleID() : "achievement_tip_hidden");
    pTooltip.name.text = text;
    ((Graphic) pTooltip.name).color = rarity.getRarityColor();
    Text component1 = ((Component) ((Component) pTooltip).transform.Find("Icon and Info/Background/Rarity Type/Rarity Text")).GetComponent<Text>();
    component1.text = str1;
    ((Graphic) component1).color = rarity.getRarityColor();
    Image component2 = ((Component) ((Component) pTooltip).transform.Find("Icon and Info/IconBG/Icon")).GetComponent<Image>();
    component2.sprite = pTrait.getSprite();
    ((Graphic) component2).color = flag ? Toolbox.color_white : Toolbox.color_black;
    ((Component) ((Component) pTooltip).transform.Find("Icon and Info/IconBG/LegendaryBG")).gameObject.SetActive(rarity == Rarity.R3_Legendary);
    ((Component) ((Component) pTooltip).transform.Find("Icon and Info/Background/IconedText")).GetComponent<Text>().text = pTrait.getCountRows();
    GameObject gameObject = ((Component) ((Component) pTooltip).transform.Find("Icon and Info/Background/Rarity Type/Rarity Stars")).gameObject;
    int num = (int) rarity;
    for (int index = 0; index < gameObject.transform.childCount; ++index)
    {
      Image component3 = ((Component) gameObject.transform.GetChild(index)).gameObject.GetComponent<Image>();
      if (index <= num)
        ((Graphic) component3).color = Toolbox.makeColor("#313131");
      else
        ((Graphic) component3).color = Color.black;
    }
    string translatedDescription = pTrait.getTranslatedDescription();
    if (!string.IsNullOrEmpty(translatedDescription))
    {
      string pDescription = translatedDescription;
      if (!pTrait.isAvailable() && pTrait.show_for_unlockables_ui)
      {
        if (pTrait.unlocked_with_achievement)
        {
          string str2 = LocalizedTextManager.getText("trait_locked_tooltip_text_achievement").ColorHex(ColorStyleLibrary.m.color_text_grey).Replace("$achievement_id$", $"<color=#00ffffff>{pTrait.getAchievementLocaleID().Localize()}</color>");
          pDescription = pData.is_editor_augmentation_button ? str2 : $"{pDescription}\n\n{str2}";
        }
        else
          pDescription = LocalizedTextManager.getText(pTrait.typed_id + "_locked_tooltip_text_exploration");
      }
      pTooltip.setDescription(pDescription);
    }
    else
      pTooltip.resetDescription();
    if (flag)
    {
      string translatedDescription2 = pTrait.getTranslatedDescription2();
      pTooltip.setBottomDescription(translatedDescription2);
    }
    if (!flag)
      return;
    BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, pTrait.base_stats);
    BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, pTrait.base_stats_meta, false);
    if (pAdditionalBaseStats == null)
      return;
    foreach (BaseStats additionalBaseStat in pAdditionalBaseStats)
      BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, additionalBaseStat, false);
  }

  private void showTraitOwners<T>(Tooltip pTooltip, T pTrait) where T : BaseTrait<T>
  {
    Transform recursive = ((Component) pTooltip).transform.FindRecursive("Species");
    if (pTrait.default_for_actor_assets == null)
    {
      ((Component) recursive).gameObject.SetActive(false);
    }
    else
    {
      ((Component) recursive).gameObject.SetActive(true);
      if (pTooltip.pool_icons == null)
      {
        StatsIcon pPrefab = Resources.Load<StatsIcon>("ui/PrefabTooltipTraitSpecies");
        pTooltip.pool_icons = new ObjectPoolGenericMono<StatsIcon>(pPrefab, recursive);
      }
      foreach (ActorAsset defaultForActorAsset in pTrait.default_for_actor_assets)
      {
        if (!defaultForActorAsset.unit_zombie && defaultForActorAsset.show_in_taxonomy_tooltip)
        {
          Image icon = pTooltip.pool_icons.getNext().getIcon();
          icon.sprite = defaultForActorAsset.getSpriteIcon();
          if (defaultForActorAsset.isAvailable())
            ((Graphic) icon).color = Toolbox.color_white;
          else
            ((Graphic) icon).color = Toolbox.color_black;
        }
      }
    }
  }

  private void showChromosome(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Chromosome chromosome = pData.chromosome;
    ChromosomeTypeAsset asset = chromosome.getAsset();
    string localeId = asset.getLocaleID();
    ((Component) pTooltip.name).GetComponent<LocalizedText>().setKeyAndUpdate(localeId);
    string text = LocalizedTextManager.getText(asset.getDescriptionID());
    pTooltip.setDescription(text);
    Tooltip tooltip = pTooltip;
    int num = chromosome.countNonEmpty();
    string str1 = num.ToString();
    num = chromosome.genes.Count;
    string str2 = num.ToString();
    string pValue = $"{str1}/{str2}";
    tooltip.addLineText("genes", pValue);
    pTooltip.addLineBreak();
    BaseStats totalStatsFrom = BaseStatsHelper.getTotalStatsFrom(chromosome.getStats(), chromosome.getStatsMeta());
    BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, totalStatsFrom);
  }

  private void showGene(Tooltip pTooltip, string pType, TooltipData pData)
  {
    GeneAsset gene = pData.gene;
    LocusElement locus = pData.locus;
    Chromosome chromosome = pData.chromosome;
    bool flag = Object.op_Inequality((Object) locus, (Object) null) && locus.isAmplifier();
    int num1 = gene.isAvailable() ? 1 : 0;
    int num2 = chromosome == null ? 0 : (chromosome.isVoidLocus(locus.locus_index) ? 1 : 0);
    string text1;
    if (num1 == 0)
    {
      text1 = LocalizedTextManager.getText("achievement_tip_hidden");
      if (gene.unlocked_with_achievement)
      {
        string pDescription = LocalizedTextManager.getText("gene_locked_tooltip_text_achievement").Replace("$achievement_id$", $"<color=#00ffffff>{gene.getAchievementLocaleID().Localize()}</color>");
        pTooltip.setDescription(pDescription);
      }
      else
        pTooltip.setDescription(LocalizedTextManager.getText("gene_locked_tooltip_text_exploration"));
      ((Component) ((Component) pTooltip).transform.FindRecursive("Stats")).gameObject.SetActive(false);
    }
    else
    {
      text1 = LocalizedTextManager.getText(gene.getLocaleID());
      string pDescription = "";
      string descriptionId = gene.getDescriptionID();
      if (LocalizedTextManager.stringExists(descriptionId))
      {
        string text2 = LocalizedTextManager.getText(descriptionId);
        pDescription = $"{pDescription}{text2}\n";
      }
      pTooltip.setDescription(pDescription);
    }
    if (Object.op_Inequality((Object) locus, (Object) null))
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string pMainText;
      if (locus.isAmplifierBad())
      {
        pMainText = LocalizedTextManager.getText("amplifier_bad");
        string text3 = LocalizedTextManager.getText("amplifier_bad_description");
        pTooltip.setDescription(text3);
      }
      else if (locus.isAmplifier())
      {
        pMainText = LocalizedTextManager.getText("amplifier");
        string text4 = LocalizedTextManager.getText("amplifier_description");
        pTooltip.setDescription(text4);
      }
      else
        pMainText = text1;
      string pSubText = "locus";
      pTooltip.setTitle(pMainText, pSubText);
    }
    else
      pTooltip.setTitle(text1, "gene");
    string pDescription1 = "";
    if (num1 != 0 && !gene.is_empty)
    {
      this._base_stats_temp.clear();
      if (Object.op_Inequality((Object) locus, (Object) null) && chromosome != null)
        chromosome.fillStatsForTooltip(locus, this._base_stats_temp);
      else
        this._base_stats_temp.mergeStats(gene.base_stats);
      if (chromosome != null && !flag)
        pDescription1 += chromosome.getSynergyTooltipText(locus.locus_index);
      BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, this._base_stats_temp);
    }
    if (!flag)
      pDescription1 = $"{pDescription1}{LocalizedTextManager.getText("dna_sequence")}\n{gene.getSequence()}";
    pTooltip.setBottomDescription(pDescription1);
  }

  private void showGeneDNASequence(Tooltip pTooltip, string pType, TooltipData pData)
  {
    GeneAsset gene = pData.gene;
    Chromosome chromosome = pData.chromosome;
    LocusElement locus = pData.locus;
    bool flag = false;
    if (Object.op_Inequality((Object) locus, (Object) null))
      flag = locus.isAmplifier();
    int num = gene.isAvailable() ? 1 : 0;
    string pDescription = "";
    if (num != 0 && !flag && Object.op_Inequality((Object) locus, (Object) null) && chromosome != null)
      pDescription += chromosome.getSynergyTooltipText(locus.locus_index);
    if (!flag)
      pDescription = $"{pDescription}{LocalizedTextManager.getText("dna_sequence")}\n{gene.getSequence()}";
    pTooltip.setBottomDescription(pDescription);
  }

  private void showKingdomAsset(Tooltip pTooltip, string pType, TooltipData pData)
  {
    KingdomAsset kingdomAsset = pData.kingdom_asset;
    pTooltip.name.text = kingdomAsset.id;
    string pDescription;
    string pDescription2;
    DebugKingdomButton.getTooltipDescription(kingdomAsset, out pDescription, out pDescription2);
    pTooltip.setDescription(pDescription);
    if (!string.IsNullOrEmpty(pDescription2))
      pTooltip.setBottomDescription(pDescription2);
    pTooltip.tryShowBoolDebug("civ", kingdomAsset.civ);
    pTooltip.tryShowBoolDebug("nomads", kingdomAsset.nomads);
    pTooltip.tryShowBoolDebug("nature", kingdomAsset.nature);
    pTooltip.tryShowBoolDebug("mobs", kingdomAsset.mobs);
    pTooltip.tryShowBoolDebug("miniciv", kingdomAsset.group_miniciv);
    pTooltip.tryShowBoolDebug("neutral", kingdomAsset.neutral);
    pTooltip.tryShowBoolDebug("brain", kingdomAsset.brain);
    pTooltip.tryShowBoolDebug("always_attack_each_other", kingdomAsset.always_attack_each_other);
    pTooltip.tryShowBoolDebug("units_always_aggressive", kingdomAsset.units_always_looking_for_enemies);
  }

  private void showTip(Tooltip pTooltip, string pType, TooltipData pData)
  {
    pTooltip.name.text = !LocalizedTextManager.stringExists(pData.tip_name) ? pData.tip_name : LocalizedTextManager.getText(pData.tip_name);
    if (!string.IsNullOrEmpty(pData.tip_description))
    {
      string pDescription = LocalizedTextManager.getText(pData.tip_description);
      if (pDescription.Contains("$favorite_food$"))
      {
        string newValue = "??";
        if (SelectedUnit.unit.hasFavoriteFood())
          newValue = LocalizedTextManager.getText(SelectedUnit.unit.data.favorite_food);
        pDescription = $"{pDescription.Replace("$favorite_food$", newValue) + "\n" + "\n"}{LocalizedTextManager.getText("food_consumed")}: {SelectedUnit.unit.data.food_consumed.ToString()}";
      }
      pTooltip.setDescription(pDescription);
    }
    if (!Config.isComputer && !Config.isEditor || string.IsNullOrEmpty(pData.tip_description_2))
      return;
    string text = LocalizedTextManager.getText(pData.tip_description_2);
    string pDescription1 = AssetManager.hotkey_library.replaceSpecialTextKeys(text);
    pTooltip.setBottomDescription(pDescription1);
  }

  private void showTipZoneMode(Tooltip pTooltip, string pType, TooltipData pData)
  {
    OptionAsset optionAsset = AssetManager.meta_type_library.getFromPower(pData.tip_name).option_asset;
    string pMainText = pData.tip_name.Localize();
    string pSubText = "";
    if (optionAsset.multi_toggle)
      pSubText = optionAsset.getOptionLocaleID();
    pTooltip.setTitle(pMainText, pSubText);
    if (!string.IsNullOrEmpty(pData.tip_description))
    {
      string pDescription = $"{LocalizedTextManager.getText(pData.tip_description)}\n\n{this.getStateText("borders_state_tip", Zones.isBordersEnabled())}, {this.getStateText("map_names_state_tip", Zones.showMapNames())}";
      pTooltip.setDescription(pDescription);
    }
    if (!Config.isComputer && !Config.isEditor || string.IsNullOrEmpty(pData.tip_description_2))
      return;
    string text = LocalizedTextManager.getText(pData.tip_description_2);
    string pDescription1 = AssetManager.hotkey_library.replaceSpecialTextKeys(text);
    pTooltip.setBottomDescription(pDescription1);
  }

  private string getStateText(string pLocale, bool pState)
  {
    string newValue = (pState ? "short_on" : "short_off").ColorHex(pState ? "#95DD5D" : "#FF8686", true);
    return LocalizedTextManager.getText(pLocale).Replace("$state$", newValue);
  }

  private void showCarryingResources(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Actor actor = pData.actor;
    pTooltip.name.text = pData.tip_name.Localize();
    foreach (KeyValuePair<string, ResourceContainer> resource in actor.inventory.getResources())
    {
      ResourceAsset asset = resource.Value.asset;
      int amount = resource.Value.amount;
      pTooltip.addLineIntText(asset.getLocaleID(), amount, "#43FF43");
    }
  }

  private void showPastNames(Tooltip pTooltip, string pType, TooltipData pData)
  {
    // ISSUE: unable to decompile the method.
  }

  private void showPastRulers(Tooltip pTooltip, string pType, TooltipData pData)
  {
    ListPool<LeaderEntry> pastRulers = pData.past_rulers;
    pTooltip.name.text = pData.tip_name.Localize();
    if (pastRulers == null || pastRulers.Count == 0)
      return;
    MetaCustomizationColorLibrary colorLibrary1 = AssetManager.meta_customization_library.getAsset(pData.meta_type).color_library;
    ColorLibrary colorLibrary2 = colorLibrary1 != null ? colorLibrary1() : (ColorLibrary) null;
    int num1 = Date.getCurrentYear();
    for (int index = pastRulers.Count - 1; index >= 0; --index)
    {
      LeaderEntry leaderEntry = pastRulers[index];
      string str = leaderEntry.name;
      bool flag = false;
      Actor pObject = World.world.units.get(leaderEntry.id);
      if (!pObject.isRekt())
        str = pObject.name;
      else
        flag = true;
      if (string.IsNullOrEmpty(str))
        str = LocalizedTextManager.getText("unknown");
      if (flag)
        str = "† " + str;
      if (leaderEntry.color_id > -1 && colorLibrary2 != null)
      {
        string hex = Toolbox.colorToHex(Color32.op_Implicit(colorLibrary2.list[leaderEntry.color_id].getColorText()), false);
        str = Toolbox.coloredText(str, hex);
      }
      int year = Date.getYear(leaderEntry.timestamp_ago);
      int num2 = Date.getYear(leaderEntry.timestamp_end);
      if (leaderEntry.timestamp_end < leaderEntry.timestamp_ago)
        num2 = num1;
      num1 = year;
      int num3 = num2 - year;
      string pValue = $"{year}–{num2} ({num3} {"y"})";
      pTooltip.addLineText(str, pValue, pLocalize: false);
    }
  }

  private void showMass(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Actor actor = pData.actor;
    pTooltip.name.text = pData.tip_name.Localize();
    foreach (ResourceContainer resourceContainer in actor.getResourcesFromActor())
    {
      ResourceAsset resourceAsset = AssetManager.resources.get(resourceContainer.id);
      pTooltip.addLineIntText(resourceAsset.getLocaleID(), resourceContainer.amount, "#43FF43");
    }
  }

  private void showPassengers(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Actor actor = pData.actor;
    pTooltip.name.text = LocalizedTextManager.getText("passengers");
    TooltipIconsRow component = ((Component) ((Component) pTooltip).transform.FindRecursive("Passengers")).GetComponent<TooltipIconsRow>();
    this.showBoatPassengers(actor.getSimpleComponent<Boat>(), component, pTooltip, pData);
  }

  private void showLoyalty(Tooltip pTooltip, string pType, TooltipData pData)
  {
    pTooltip.name.text = LocalizedTextManager.getText("loyalty");
    int loyalty = pData.city.getLoyalty(true);
    if (loyalty > 0)
      pTooltip.addLineIntText("opinion_total", loyalty, "#43FF43");
    else
      pTooltip.addLineIntText("opinion_total", loyalty, "#FB2C21");
    foreach (LoyaltyAsset key in LoyaltyCalculator.results.Keys)
    {
      int result = LoyaltyCalculator.results[key];
      string translationKey = key.getTranslationKey(result);
      pTooltip.addOpinion(new TooltipOpinionInfo(translationKey, result));
    }
    pTooltip.stats_description.text += "\n------------";
    pTooltip.stats_values.text += "\n------------";
    pTooltip.addLineBreak();
    pTooltip.addLineBreak();
  }

  private void showArmy(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Army army = pData.army;
    Kingdom kingdom = army.getKingdom();
    City city = army.getCity();
    pTooltip.setTitle(army.name, "army", army.getColor().color_text);
    pTooltip.setSpeciesIcon(army.getActorAsset().getSpriteIcon());
    this.setIconValue(pTooltip, "i_age", (float) army.getAge());
    this.setIconValue(pTooltip, "i_population", (float) army.countUnits());
    if (!kingdom.isRekt())
      pTooltip.addLineText("kingdom", kingdom.name, kingdom.getColor().color_text);
    if (!city.isRekt())
      pTooltip.addLineText("villages", city.name, city.getColor().color_text);
    if (army.hasCaptain())
      pTooltip.addLineText("captain", army.getCaptain().getName(), army.getColor().color_text);
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("males", army.countMales());
    pTooltip.addLineIntText("females", army.countFemales());
    pTooltip.addLineIntText("happy", army.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("kills", army.getTotalKills());
    pTooltip.addLineIntText("deaths", army.getTotalDeaths());
    pTooltip.addLineIntText("renown", army.getRenown());
    foreach (KingdomBanner kingdomBanner in ((Component) pTooltip).transform.FindAllRecursive<KingdomBanner>())
    {
      if (((Component) kingdomBanner).gameObject.activeSelf)
        kingdomBanner.load((NanoObject) army.getKingdom());
    }
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showSubspecies(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Subspecies subspecies = pData.subspecies;
    pTooltip.setTitle(subspecies.name, "subspecies_singular", subspecies.getColor().color_text);
    pTooltip.setSpeciesIcon(subspecies.getActorAsset().getSpriteIcon());
    this.setIconValue(pTooltip, "i_age", (float) subspecies.getAge());
    this.setIconValue(pTooltip, "i_population", (float) subspecies.countUnits());
    ((Component) pTooltip).GetComponentInChildren<TooltipSubspeciesTraitsRow>(true).init(pTooltip, pData);
    pTooltip.addLineIntText("adults", subspecies.countAdults());
    pTooltip.addLineIntText("children", subspecies.countChildren());
    pTooltip.addLineIntText("happy", subspecies.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("kings", subspecies.countKings());
    pTooltip.addLineIntText("leaders", subspecies.countLeaders());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("births", subspecies.getTotalBirths());
    pTooltip.addLineIntText("deaths", subspecies.getTotalDeaths());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("families", subspecies.countCurrentFamilies());
    foreach (SubspeciesBanner subspeciesBanner in ((Component) pTooltip).transform.FindAllRecursive<SubspeciesBanner>())
    {
      if (((Component) subspeciesBanner).gameObject.activeSelf)
        subspeciesBanner.load((NanoObject) subspecies);
    }
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showFamily(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Family family = pData.family;
    ActorAsset actorAsset = family.getActorAsset();
    pTooltip.setSpeciesIcon(actorAsset.getSpriteIcon());
    pTooltip.setTitle(family.name, "family", family.getColor().color_text);
    int age = family.getAge();
    this.setIconValue(pTooltip, "i_age", (float) age);
    this.setIconValue(pTooltip, "i_population", (float) family.countUnits());
    pTooltip.addLineIntText("adults", family.countAdults());
    pTooltip.addLineIntText("children", family.countChildren());
    pTooltip.addLineIntText("happy", family.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("births", family.getTotalBirths());
    pTooltip.addLineIntText("deaths", family.getTotalDeaths());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<FamilyBanner>())
      bannerBase.load((NanoObject) family);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showLanguage(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Language language = pData.language;
    pTooltip.setSpeciesIcon(language.getActorAsset().getSpriteIcon());
    pTooltip.setTitle(language.name, "language", language.getColor().color_text);
    this.setIconValue(pTooltip, "i_age", (float) language.getAge());
    this.setIconValue(pTooltip, "i_population", (float) language.countUnits());
    ((Component) pTooltip).GetComponentInChildren<TooltipLanguageTraitsRow>(true).init(pTooltip, pData);
    if (!string.IsNullOrEmpty(language.data.creator_city_name))
    {
      pTooltip.addLineText("founded_in", language.data.creator_city_name);
      pTooltip.addLineBreak();
    }
    pTooltip.addLineIntText("kingdoms", language.countKingdoms());
    pTooltip.addLineIntText("villages", language.countCities());
    pTooltip.addLineIntText("books", language.books.count());
    pTooltip.addLineIntText("books_written", language.countWrittenBooks());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("adults", language.countAdults());
    pTooltip.addLineIntText("children", language.countChildren());
    pTooltip.addLineIntText("happy", language.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("kings", language.countKings());
    pTooltip.addLineIntText("leaders", language.countLeaders());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("deaths", language.getTotalDeaths());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<LanguageBanner>())
      bannerBase.load((NanoObject) language);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showReligion(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Religion religion = pData.religion;
    pTooltip.setSpeciesIcon(religion.getActorAsset().getSpriteIcon());
    pTooltip.setTitle(religion.name, "religion", religion.getColor().color_text);
    int age = religion.getAge();
    this.setIconValue(pTooltip, "i_age", (float) age);
    this.setIconValue(pTooltip, "i_population", (float) religion.countUnits());
    if (!string.IsNullOrEmpty(religion.data.creator_city_name))
    {
      pTooltip.addLineText("founded_in", religion.data.creator_city_name);
      pTooltip.addLineBreak();
    }
    pTooltip.addLineIntText("kingdoms", religion.countKingdoms());
    pTooltip.addLineIntText("villages", religion.countCities());
    pTooltip.addLineIntText("books", religion.books.count());
    pTooltip.addLineIntText("happy", religion.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("adults", religion.countAdults());
    pTooltip.addLineIntText("children", religion.countChildren());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("kings", religion.countKings());
    pTooltip.addLineIntText("leaders", religion.countLeaders());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("deaths", religion.getTotalDeaths());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<ReligionBanner>())
      bannerBase.load((NanoObject) religion);
    ((Component) pTooltip).GetComponentInChildren<TooltipReligionTraitsRow>(true).init(pTooltip, pData);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showCulture(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Culture culture = pData.culture;
    pTooltip.setSpeciesIcon(culture.getActorAsset().getSpriteIcon());
    pTooltip.setTitle(culture.name, "culture", culture.getColor().color_text);
    this.setIconValue(pTooltip, "i_age", (float) culture.getAge());
    this.setIconValue(pTooltip, "i_population", (float) culture.countUnits());
    ((Component) pTooltip).GetComponentInChildren<TooltipCultureTraitsRow>(true).init(pTooltip, pData);
    if (!string.IsNullOrEmpty(culture.data.creator_city_name))
    {
      pTooltip.addLineText("founded_in", culture.data.creator_city_name);
      pTooltip.addLineBreak();
    }
    pTooltip.addLineIntText("kingdoms", culture.countKingdoms());
    pTooltip.addLineIntText("villages", culture.countCities());
    pTooltip.addLineIntText("books", culture.books.count());
    pTooltip.addLineIntText("happy", culture.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("adults", culture.countAdults());
    pTooltip.addLineIntText("children", culture.countChildren());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("kings", culture.countKings());
    pTooltip.addLineIntText("leaders", culture.countLeaders());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("deaths", culture.getTotalDeaths());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<CultureBanner>())
      bannerBase.load((NanoObject) culture);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showPlot(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Plot plot = pData.plot;
    string pColorHex = plot.getColor()?.color_text;
    if (string.IsNullOrEmpty(pColorHex))
      pColorHex = "#F3961F";
    pTooltip.setTitle(plot.name, "plot", pColorHex);
    int progressPercentage = plot.getProgressPercentage();
    int age = plot.getAge();
    string pValue1 = progressPercentage.ToText() + "%";
    string pValue2 = $"{plot.getSupporters().ToText()}/{plot.getMaxSupporters().ToText()}";
    pTooltip.addDescription(plot.getAsset().get_formatted_description(plot));
    pTooltip.addLineText("started_at", plot.getFoundedDate());
    string pColor = pColorHex;
    Actor author = plot.getAuthor();
    if (author != null)
      pColor = author.kingdom.getColor().color_text;
    pTooltip.addLineText("started_by", plot.data.founder_name, pColor);
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("tip_plot_age", age);
    pTooltip.addLineText("tip_plot_progress", pValue1);
    pTooltip.addLineText("tip_plot_members", pValue2);
    foreach (BannerBase componentsInChild in ((Component) ((Component) pTooltip).transform).GetComponentsInChildren<PlotBanner>(true))
      componentsInChild.load((NanoObject) plot);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showPlotInEditor(Tooltip pTooltip, string pType, TooltipData pData)
  {
    PlotAsset plotAsset = pData.plot_asset;
    string pKey1;
    string pKey2;
    if (plotAsset.isAvailable())
    {
      pKey1 = plotAsset.getDescriptionID2();
      pKey2 = plotAsset.getLocaleID();
    }
    else
    {
      pKey1 = "plot_locked_tooltip_text_exploration";
      pKey2 = "achievement_tip_hidden";
    }
    string text1 = LocalizedTextManager.getText(pKey2);
    pTooltip.setTitle(text1);
    string text2 = LocalizedTextManager.getText(pKey1);
    pTooltip.addDescription(text2);
    Sprite sprite = plotAsset.getSprite();
    foreach (Image componentsInChild in ((Component) ((Component) pTooltip).transform.Find("Headline/icons")).GetComponentsInChildren<Image>(true))
    {
      componentsInChild.sprite = sprite;
      if (plotAsset.isAvailable())
        ((Graphic) componentsInChild).color = Toolbox.color_white;
      else
        ((Graphic) componentsInChild).color = Toolbox.color_black;
    }
  }

  private void showCityHome(Tooltip pTooltip, string pType, TooltipData pData)
  {
    this.showCity("creature_statistics_home_village", pTooltip, pData);
  }

  private void showCityCapital(Tooltip pTooltip, string pType, TooltipData pData)
  {
    this.showCity("kingdom_statistics_capital", pTooltip, pData);
  }

  private void showCity(Tooltip pTooltip, string pType, TooltipData pData)
  {
    this.showCity("village", pTooltip, pData);
  }

  private void showHappiness(Tooltip pTooltip, string pType, TooltipData pData)
  {
    pData.actor = SelectedUnit.unit;
    ((Component) pTooltip.name).GetComponent<LocalizedText>().setKeyAndUpdate("happiness");
    if (!pData.actor.hasHappinessHistory())
      return;
    using (ListPool<HappinessHistory> listPool = new ListPool<HappinessHistory>((IEnumerable<HappinessHistory>) pData.actor.happiness_change_history))
    {
      listPool.Reverse();
      pTooltip.addLineText("happiness_current", pData.actor.getHappiness().ToText() + $" ({pData.actor.getHappinessPercent()}%)");
      pTooltip.addLineBreak();
      for (int index = 0; index < listPool.Count; ++index)
      {
        int bonus = listPool[index].bonus;
        HappinessHistory happinessHistory = listPool[index];
        HappinessAsset asset = happinessHistory.asset;
        int num1 = asset.value;
        int num2 = bonus + num1;
        string text = LocalizedTextManager.getText(asset.getLocaleID());
        happinessHistory = listPool[index];
        string pID = $"{Toolbox.coloredString(happinessHistory.getAgoString(), ColorStyleLibrary.m.color_text_grey_dark)}: {text}";
        if (num2 >= 0)
          pTooltip.addLineText(pID, num2.ToString("+##,#;-##,#;0"), "#43FF43", pLocalize: false);
        else
          pTooltip.addLineText(pID, num2.ToString("+##,#;-##,#;0"), "#FB2C21", pLocalize: false);
      }
    }
  }

  private void showCity(string pTitleID, Tooltip pTooltip, TooltipData pData)
  {
    City city = pData.city;
    pTooltip.setSpeciesIcon(city.getCurrentSpeciesIcon());
    Kingdom kingdom = city.kingdom;
    string colorText = kingdom.getColor().color_text;
    int age = city.getAge();
    this.setIconValue(pTooltip, "i_age", (float) age);
    this.setIconValue(pTooltip, "i_population", (float) city.getPopulationPeople());
    this.setIconValue(pTooltip, "i_army", (float) city.countWarriors());
    Tooltip tooltip = pTooltip;
    int num = city.countBooks();
    string str1 = num.ToString();
    num = city.countBookSlots();
    string str2 = num.ToString();
    string pValue1 = $"{str1}/{str2}";
    tooltip.addLineText("books", pValue1);
    pTooltip.setTitle(city.name, pTitleID, colorText);
    string pValue2 = "-";
    if (kingdom.hasKing())
      pValue2 = kingdom.king.getName();
    string pValue3 = "-";
    if (city.hasLeader())
      pValue3 = city.leader.getName();
    pTooltip.addLineText("village_statistics_leader", pValue3, colorText);
    if (city.hasLeader())
      pTooltip.addLineIntText("ruler_money", city.leader.money);
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("adults", city.countAdults());
    pTooltip.addLineIntText("children", city.countChildren());
    pTooltip.addLineIntText("families", city.countFamilies());
    pTooltip.addLineIntText("happy", city.countHappyUnits());
    pTooltip.addLineBreak();
    if (!city.kingdom.isNeutral())
      pTooltip.addLineText("kingdom", city.kingdom.name, colorText);
    pTooltip.addLineText("village_statistics_king", pValue2, colorText);
    if (city.hasLeader() && city.leader.hasClan())
      pTooltip.addLineText("clan", city.leader.clan.name, city.leader.clan.getColor().color_text);
    if (city.hasCulture())
      pTooltip.addLineText("culture", city.culture.name, city.culture.getColor().color_text);
    if (city.hasReligion())
      pTooltip.addLineText("religion", city.religion.name, city.religion.getColor().color_text);
    if (city.hasLanguage())
      pTooltip.addLineText("language", city.language.name, city.language.getColor().color_text);
    Alliance alliance = kingdom.getAlliance();
    if (alliance != null)
    {
      int yearsSince = Date.getYearsSince(kingdom.data.timestamp_alliance);
      pTooltip.addLineText("alliance", alliance.data.name, alliance.getColor().color_text);
      pTooltip.addLineIntText("kingdom_time_in_alliance", yearsSince, alliance.getColor().color_text);
    }
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("houses", city.getHouseCurrent());
    pTooltip.addLineIntText("area", city.zones.Count);
    pTooltip.addLineIntText("loyalty", city.getCachedLoyalty());
    pTooltip.addLineIntText("books", city.countBooks());
    ((Component) pTooltip).GetComponentInChildren<CityBanner>().load((NanoObject) city);
    if (DebugConfig.isOn(DebugOption.DebugCityReproduction))
    {
      pTooltip.addLineBreak();
      pTooltip.addLineIntText("males_single", city.countSingleMales(), "#4CCFFF");
      pTooltip.addLineIntText("females_single", city.countSingleFemales(), "#FF637D");
      pTooltip.addLineIntText("couples", city.countCouples());
      pTooltip.addLineText("male/female", $"{city.countMales().ToText()}/{city.countFemales().ToText()}", "#FF637D", pLocalize: false);
      pTooltip.addLineText("adults/kids", $"{city.countAdults().ToText()} | {city.countChildren().ToText()}", pLocalize: false);
      pTooltip.addLineIntText("pot_par_males", city.countPotentialParents(ActorSex.Male), pLocalize: false);
      pTooltip.addLineIntText("pot_par_females", city.countPotentialParents(ActorSex.Female), pLocalize: false);
      pTooltip.addLineBreak();
      pTooltip.addLineIntText("hungry", city.countHungry(), "#FF637D", false);
      pTooltip.addLineIntText("starving", city.countStarving(), "#FF637D", false);
      pTooltip.addLineIntText("food", city.countFoodTotal());
      pTooltip.addLineIntText("afteglows", city.countUnitsWithStatus("afterglow"), pLocalize: false);
      pTooltip.addLineIntText("pregnant", city.countUnitsWithStatus("pregnant"), pLocalize: false);
      pTooltip.addLineIntText("births", city.getTotalBirths(), pLocalize: false);
    }
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showActorNormal(Tooltip pTooltip, string pType, TooltipData pData)
  {
    this.showActor("", pTooltip, pData);
  }

  private void showLeader(Tooltip pTooltip, string pType, TooltipData pData)
  {
    this.showActor("village_statistics_leader", pTooltip, pData);
  }

  private void showKing(Tooltip pTooltip, string pType, TooltipData pData)
  {
    this.showActor("village_statistics_king", pTooltip, pData);
  }

  private void showActor(string pSubTitle, Tooltip pTooltip, TooltipData pData)
  {
    Actor actor = pData.actor;
    ((Component) ((Component) pTooltip).transform.FindRecursive("IconSpecial")).GetComponent<Image>().sprite = !actor.asset.is_boat ? (!actor.isSexMale() ? SpriteTextureLoader.getSprite("ui/icons/IconFemale") : SpriteTextureLoader.getSprite("ui/icons/IconMale")) : actor.asset.getSpriteIcon();
    this.setIconValue(pTooltip, "i_age", (float) actor.getAge());
    this.setIconValue(pTooltip, "i_level", (float) actor.data.level);
    this.setIconValue(pTooltip, "i_kills", (float) actor.data.kills);
    ((Component) pTooltip).GetComponentInChildren<TooltipActorTraitsRow>(true).init(pTooltip, pData);
    ((Component) pTooltip).GetComponentInChildren<TooltipActorEquipmentsRow>(true).init(pTooltip, pData);
    StatBar component1 = ((Component) ((Component) pTooltip).transform.FindRecursive("HealthBar")).GetComponent<StatBar>();
    float health = (float) actor.getHealth();
    float maxHealth = (float) actor.getMaxHealth();
    double pVal = (double) health;
    double pMax = (double) maxHealth;
    string pEnding = "/" + ((int) maxHealth).ToText(4);
    component1.setBar((float) pVal, (float) pMax, pEnding, false, pSpeed: 0.25f);
    this.showActorBars(pTooltip, actor);
    string colorText1 = actor.kingdom.getColor().color_text;
    pTooltip.setTitle(actor.name, pSubTitle, colorText1);
    if (DebugConfig.isOn(DebugOption.DebugTooltipActorAI))
    {
      pTooltip.addLineText("wait_timer", actor.hasTask() ? actor.timer_action.ToText() : "-", "#43FF43", pLocalize: false);
      pTooltip.addLineText("task", actor.hasTask() ? actor.ai.task.id : "-", "#43FF43", pLocalize: false);
      pTooltip.addLineText("action", actor.ai.action != null ? actor.ai.action.id : "-", "#23F3FF", pLocalize: false);
      pTooltip.addLineText("job", actor.ai.job != null ? actor.ai.job.id : "-", "#FB2C21", pLocalize: false);
      pTooltip.addLineText("citizen_job", actor.citizen_job != null ? actor.citizen_job.id : "-", "#8CFF99", pLocalize: false);
      pTooltip.addLineIntText("id", actor.data.id);
      pTooltip.addLineIntText("hashset", actor.GetHashCode(), pLocalize: false);
      pTooltip.addLineIntText("kingdom_hash", actor.kingdom.GetHashCode(), pLocalize: false);
      pTooltip.addLineIntText("kingdom_id", actor.kingdom.data.id, pLocalize: false);
      pTooltip.addLineText("profession", actor.profession_asset.id, pLocalize: false);
    }
    if (actor.isSapient() && actor.isKingdomCiv())
      pTooltip.addLineText("kingdom", actor.kingdom.name, colorText1);
    if (actor.hasLover())
      pTooltip.addLineText("lover", actor.lover.name, actor.lover.kingdom.getColor().color_text);
    if (actor.asset.inspect_home)
    {
      string pValue = "??";
      if (actor.city != null)
        pValue = actor.city.name;
      pTooltip.addLineText("creature_statistics_home_village", pValue, colorText1);
      if (actor.hasClan())
      {
        string colorText2 = actor.clan.getColor().color_text;
        pTooltip.addLineText("clan", actor.clan.data.name, colorText2);
      }
    }
    if (actor.hasFamily())
      pTooltip.addLineText("family", actor.family.name, actor.family.getColor().color_text);
    if (actor.hasCulture())
      pTooltip.addLineText("culture", actor.culture.name, actor.culture.getColor().color_text);
    if (actor.hasLanguage())
      pTooltip.addLineText("language", actor.language.name, actor.language.getColor().color_text);
    if (actor.hasArmy())
      pTooltip.addLineText("army", actor.army.name, actor.army.getColor().color_text);
    pTooltip.addLineBreak();
    if (actor.money > 0)
      pTooltip.addLineIntText("money", actor.money);
    if (actor.loot > 0)
      pTooltip.addLineIntText("loot", actor.loot);
    if (actor.asset.inspect_kills)
      pTooltip.addLineIntText("creature_statistics_kills", actor.data.kills);
    if (actor.asset.inspect_children)
      pTooltip.addLineIntText("creature_statistics_children", actor.current_children_count);
    if (actor.isSapient() && actor.s_personality != null)
      pTooltip.addLineText("creature_statistics_personality", LocalizedTextManager.getText("personality_" + actor.s_personality.id));
    pTooltip.addLineText("task", actor.hasTask() ? actor.ai.task.getLocalizedText() : "-", "#43FF43");
    if (actor.hasSubspecies())
    {
      pTooltip.addLineBreak();
      pTooltip.addLineText("subspecies", actor.subspecies.name, actor.subspecies.getColor().color_text, pLimitValue: 15);
    }
    TooltipIconsRow component2 = ((Component) ((Component) pTooltip).transform.FindRecursive("Resources")).GetComponent<TooltipIconsRow>();
    bool flag = actor.isCarryingResources();
    ((Component) component2).gameObject.SetActive(flag);
    if (flag)
    {
      foreach (ResourceContainer resourceContainer in actor.inventory.getResources().Values)
      {
        Sprite spriteIcon = AssetManager.resources.get(resourceContainer.id).getSpriteIcon();
        int amount = resourceContainer.amount;
        int num = 5;
        for (int index = 0; index < amount; ++index)
        {
          component2.addIcon(spriteIcon);
          --num;
          if (num <= 0)
            break;
        }
      }
      component2.init(pTooltip, pData);
    }
    TooltipIconsRow component3 = ((Component) ((Component) pTooltip).transform.FindRecursive("Passengers")).GetComponent<TooltipIconsRow>();
    if (actor.asset.is_boat)
    {
      Boat simpleComponent = actor.getSimpleComponent<Boat>();
      pTooltip.addLineBreak();
      pTooltip.addLineIntText("passengers", simpleComponent.countPassengers());
      this.showBoatPassengers(simpleComponent, component3, pTooltip, pData);
    }
    else
      ((Component) component3).gameObject.SetActive(false);
    Sprite sprite = !actor.asset.is_boat || !actor.hasCity() ? actor.asset.getSpriteIcon() : actor.city.getSpriteIcon();
    Image speciesIcon = pTooltip.getSpeciesIcon();
    if (Object.op_Inequality((Object) sprite, (Object) null))
    {
      speciesIcon.sprite = sprite;
      ((Component) speciesIcon).gameObject.SetActive(true);
    }
    else
      ((Component) speciesIcon).gameObject.SetActive(false);
  }

  private void showBoatPassengers(
    Boat pBoat,
    TooltipIconsRow pPassengersIcons,
    Tooltip pTooltip,
    TooltipData pData)
  {
    if (!pBoat.hasPassengers())
    {
      ((Component) pPassengersIcons).gameObject.SetActive(false);
    }
    else
    {
      ((Component) pPassengersIcons).gameObject.SetActive(true);
      int num = 60;
      foreach (Actor passenger in (IEnumerable<Actor>) pBoat.getPassengers())
      {
        Sprite spriteIcon = passenger.asset.getSpriteIcon();
        pPassengersIcons.addIcon(spriteIcon);
        --num;
        if (num <= 0)
          break;
      }
      pPassengersIcons.init(pTooltip, pData);
    }
  }

  private void showActorBars(Tooltip pTooltip, Actor pActor)
  {
    bool pShow1 = pActor.hasEmotions();
    if (pShow1)
      ((Component) pTooltip).GetComponentInChildren<HappinessBarIcon>(true).load(pActor);
    this.checkShowProgressBar(pTooltip, "HappinessBarFitter", "%", (float) pActor.getHappinessPercent(), 100f, pShow1);
    bool pShow2 = pActor.needsFood();
    float pCurrentValue1 = (float) ((double) pActor.getNutrition() / (double) pActor.getMaxNutrition() * 100.0);
    this.checkShowProgressBar(pTooltip, "HungerBarFitter", "%", pCurrentValue1, 100f, pShow2);
    bool pShow3 = !pActor.asset.force_hide_stamina;
    int maxStamina = pActor.getMaxStamina();
    float pCurrentValue2 = (float) Mathf.Clamp(pActor.getStamina(), 0, maxStamina);
    this.checkShowProgressBar(pTooltip, "StaminaBarFitter", $"/{maxStamina}", pCurrentValue2, (float) maxStamina, pShow3);
    bool pShow4 = !pActor.asset.force_hide_mana;
    int maxMana = pActor.getMaxMana();
    float pCurrentValue3 = (float) Mathf.Clamp(pActor.getMana(), 0, maxMana);
    this.checkShowProgressBar(pTooltip, "ManaBarFitter", $"/{maxMana}", pCurrentValue3, (float) maxMana, pShow4);
    Transform recursive = ((Component) pTooltip).transform.FindRecursive("Bars");
    if (!pShow1 && !pShow2 && !pShow3 && !pShow4)
      ((Component) recursive).gameObject.SetActive(false);
    else
      ((Component) recursive).gameObject.SetActive(true);
  }

  private void checkShowProgressBar(
    Tooltip pTooltip,
    string pBarName,
    string pEnding,
    float pCurrentValue,
    float pMax,
    bool pShow)
  {
    Transform recursive = ((Component) pTooltip).transform.FindRecursive(pBarName);
    ((Component) recursive).gameObject.SetActive(pShow);
    if (!pShow)
      return;
    ((Component) recursive).GetComponentInChildren<StatBar>(true).setBar(pCurrentValue, pMax, pEnding, false, pSpeed: 0.25f);
  }

  private void showClan(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Clan clan = pData.clan;
    pTooltip.setSpeciesIcon(clan.getActorAsset().getSpriteIcon());
    pTooltip.setDescription(clan.getMotto());
    string colorText = clan.getColor().color_text;
    pTooltip.setTitle(clan.name, "clan", colorText);
    this.setIconValue(pTooltip, "i_age", (float) clan.getAge());
    this.setIconValue(pTooltip, "i_population", (float) clan.countUnits());
    ((Component) pTooltip).GetComponentInChildren<TooltipClanTraitsRow>(true).init(pTooltip, pData);
    pTooltip.addLineText("clan_members_title", clan.getTextMaxMembers());
    if (clan.getChief() != null)
    {
      if (clan.getChief().hasKingdom())
        colorText = clan.getChief().kingdom.getColor().color_text;
      pTooltip.addLineText("clan_chief_title", clan.getChief().getName(), colorText);
      pTooltip.addLineText("species", clan.getChief().asset.getTranslatedName(), colorText);
      pTooltip.addLineBreak();
    }
    pTooltip.addLineIntText("adults", clan.countAdults());
    pTooltip.addLineIntText("children", clan.countChildren());
    pTooltip.addLineIntText("happy", clan.countHappyUnits());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("kings", clan.countKings());
    pTooltip.addLineIntText("leaders", clan.countLeaders());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("deaths", clan.getTotalDeaths());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<ClanBanner>())
      bannerBase.load((NanoObject) clan);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showBook(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Book book = pData.book;
    BookTypeAsset asset = book.getAsset();
    int age = book.getAge();
    string pDescription1 = LocalizedTextManager.getText("book_author_description").Replace("$author_name$", book.data.author_name).Replace("$author_kingdom$", book.data.author_kingdom_name).Replace("$author_city$", book.data.author_city_name);
    string pDescription2 = asset.getDescriptionTranslated();
    pTooltip.setTitle(book.name, asset.getLocaleID(), asset.color_text);
    pTooltip.addLineIntText("age", age);
    pTooltip.addLineText("book_written_in", book.getBirthday());
    pTooltip.addLineIntText("book_times_read", book.data.times_read);
    pTooltip.addLineBreak();
    this.showMetaLineActor(pTooltip, "book_author", book.data.author_id, book.data.author_name);
    this.showMetaLineClan(pTooltip, "clan", book.data.author_clan_id, book.data.author_clan_name);
    this.showMetaLineCulture(pTooltip, "culture", book.data.culture_id, book.data.culture_name);
    this.showMetaLineLanguage(pTooltip, "language", book.data.language_id, book.data.language_name);
    this.showMetaLineVillage(pTooltip, "village", book.data.author_city_id, book.data.author_city_name);
    this.showMetaLineVillage(pTooltip, "religion", book.data.religion_id, book.data.religion_name);
    pTooltip.addLineBreak();
    string pID = Toolbox.coloredText(LocalizedTextManager.getText("book_action_on_read"), "#FFFFFF");
    pTooltip.addLineText(pID, "", pLocalize: false);
    TooltipIconsRow componentInChildren = ((Component) pTooltip).GetComponentInChildren<TooltipIconsRow>(true);
    BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, book.getBaseStats());
    if (book.getBookTraitActor() != null)
      componentInChildren.addIcon(book.getBookTraitActor().getSprite());
    if (book.getBookTraitCulture() != null)
      componentInChildren.addIcon(book.getBookTraitCulture().getSprite());
    if (book.getBookTraitLanguage() != null)
      componentInChildren.addIcon(book.getBookTraitLanguage().getSprite());
    if (book.getBookTraitReligion() != null)
      componentInChildren.addIcon(book.getBookTraitReligion().getSprite());
    componentInChildren.init(pTooltip, pData);
    if (Config.editor_maxim)
      pDescription2 = pDescription2 + "\n\n" + StoryLibrary.getTestText(book.getLanguage());
    pTooltip.setDescription(pDescription2);
    pTooltip.setBottomDescription(pDescription1);
  }

  private void showMetaLineActor(Tooltip pTooltip, string pTitle, long pID, string pDefaultName)
  {
    Actor pObject = pID.hasValue() ? World.world.units.get(pID) : (Actor) null;
    string pValue = "† " + pDefaultName;
    string pColor = (string) null;
    if (!pObject.isRekt())
    {
      pColor = pObject.kingdom?.getColor().color_text;
      pValue = pObject.name;
    }
    pTooltip.addLineText(pTitle, pValue, pColor);
  }

  private void showMetaLineClan(Tooltip pTooltip, string pTitle, long pID, string pDefaultName)
  {
    if (!pID.hasValue())
      return;
    Clan pObject = World.world.clans.get(pID);
    string pValue = "† " + pDefaultName;
    string pColor = (string) null;
    if (!pObject.isRekt())
    {
      pColor = pObject.getColor().color_text;
      pValue = pObject.name;
    }
    pTooltip.addLineText(pTitle, pValue, pColor);
  }

  private void showMetaLineCulture(Tooltip pTooltip, string pTitle, long pID, string pDefaultName)
  {
    if (!pID.hasValue())
      return;
    Culture pObject = World.world.cultures.get(pID);
    string pValue = "† " + pDefaultName;
    string pColor = (string) null;
    if (!pObject.isRekt())
    {
      pColor = pObject.getColor().color_text;
      pValue = pObject.name;
    }
    pTooltip.addLineText(pTitle, pValue, pColor);
  }

  private void showMetaLineLanguage(
    Tooltip pTooltip,
    string pTitle,
    long pID,
    string pDefaultName)
  {
    if (!pID.hasValue())
      return;
    Language pObject = World.world.languages.get(pID);
    string pValue = "† " + pDefaultName;
    string pColor = (string) null;
    if (!pObject.isRekt())
    {
      pColor = pObject.getColor().color_text;
      pValue = pObject.name;
    }
    pTooltip.addLineText(pTitle, pValue, pColor);
  }

  private void showMetaLineVillage(Tooltip pTooltip, string pTitle, long pID, string pDefaultName)
  {
    if (!pID.hasValue())
      return;
    City pObject = World.world.cities.get(pID);
    string pValue = "† " + pDefaultName;
    string pColor = (string) null;
    if (!pObject.isRekt())
    {
      pColor = pObject.getColor().color_text;
      pValue = pObject.name;
    }
    pTooltip.addLineText(pTitle, pValue, pColor);
  }

  private void showMetaLineSubspecies(
    Tooltip pTooltip,
    string pTitle,
    long pID,
    string pDefaultName)
  {
    if (!pID.hasValue())
      return;
    Subspecies pObject = World.world.subspecies.get(pID);
    string pValue = "† " + pDefaultName;
    string pColor = (string) null;
    if (!pObject.isRekt())
    {
      pColor = pObject.getColor().color_text;
      pValue = pObject.name;
    }
    pTooltip.addLineText(pTitle, pValue, pColor);
  }

  private void showMetaLineReligion(
    Tooltip pTooltip,
    string pTitle,
    long pID,
    string pDefaultName)
  {
    if (!pID.hasValue())
      return;
    Religion pObject = World.world.religions.get(pID);
    string pValue = "† " + pDefaultName;
    string pColor = (string) null;
    if (!pObject.isRekt())
    {
      pColor = pObject.getColor().color_text;
      pValue = pObject.name;
    }
    pTooltip.addLineText(pTitle, pValue, pColor);
  }

  private void showWar(Tooltip pTooltip, string pType, TooltipData pData)
  {
    War war = pData.war;
    ((Component) pTooltip).GetComponentInChildren<WarTooltipBannersContainer>().load(war);
    pTooltip.setTitle(war.name, war.getAsset().localized_war_name, war.getAttackersColorTextString());
    pTooltip.addLineIntText("started_at", war.getYearStarted());
    if (war.hasEnded())
      pTooltip.addLineIntText("war_ended_at", war.getYearEnded());
    pTooltip.addLineIntText("war_duration", war.getDuration());
    string pValue1 = war.data.winner.getLocaleID().Localize();
    switch (war.data.winner)
    {
      case WarWinner.Attackers:
        pTooltip.addLineText("war_winner", pValue1, war.getAttackersColorTextString());
        break;
      case WarWinner.Defenders:
        pTooltip.addLineText("war_winner", pValue1, war.getDefendersColorTextString());
        break;
      case WarWinner.Peace:
        pTooltip.addLineText("war_outcome", pValue1);
        break;
      case WarWinner.Merged:
        pTooltip.addLineText("war_outcome", pValue1);
        break;
    }
    pTooltip.addLineBreak();
    Actor actor = World.world.units.get(war.data.started_by_actor_id);
    string pValue2 = actor != null ? actor.getName() : war.data.started_by_actor_name;
    pTooltip.addLineText("instigator", pValue2);
    long startedByKingdomId = war.data.started_by_kingdom_id;
    Kingdom kingdom = World.world.kingdoms.get(startedByKingdomId) ?? (Kingdom) World.world.kingdoms.db_get(startedByKingdomId);
    if (kingdom != null)
      pTooltip.addLineText("instigator_from", kingdom.name, kingdom.getColor().color_text);
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("kingdoms", war.countKingdoms());
    pTooltip.addLineIntText("villages", war.countCities());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("deaths", war.getTotalDeaths());
    this.setIconValue(pTooltip, "a_army", (float) war.countAttackersWarriors());
    this.setIconValue(pTooltip, "a_population", (float) war.countAttackersPopulation());
    this.setIconValue(pTooltip, "a_deaths", (float) war.getDeadAttackers());
    this.setIconValue(pTooltip, "a_cities", (float) war.countAttackersCities());
    this.setIconValue(pTooltip, "d_army", (float) war.countDefendersWarriors());
    this.setIconValue(pTooltip, "d_population", (float) war.countDefendersPopulation());
    this.setIconValue(pTooltip, "d_deaths", (float) war.getDeadDefenders());
    this.setIconValue(pTooltip, "d_cities", (float) war.countDefendersCities());
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showWarSides(Tooltip pTooltip, string pType, TooltipData pData)
  {
    War war = pData.war;
    Text component1 = ((Component) ((Component) pTooltip).transform.Find("Sides/Attackers/List")).GetComponent<Text>();
    Text component2 = ((Component) ((Component) pTooltip).transform.Find("Sides/Defenders/List")).GetComponent<Text>();
    component1.text = "";
    component2.text = "";
    switch (war.data.winner)
    {
      case WarWinner.Attackers:
        Text text1 = component1;
        text1.text = $"{text1.text}{Toolbox.coloredText("war_winner_won", war.getAttackersColorTextString(), true)}\n\n";
        Text text2 = component2;
        text2.text = $"{text2.text}{Toolbox.coloredText("war_winner_lost", war.getDefendersColorTextString(), true)}\n\n";
        break;
      case WarWinner.Defenders:
        Text text3 = component2;
        text3.text = $"{text3.text}{Toolbox.coloredText("war_winner_won", war.getDefendersColorTextString(), true)}\n\n";
        Text text4 = component1;
        text4.text = $"{text4.text}{Toolbox.coloredText("war_winner_lost", war.getAttackersColorTextString(), true)}\n\n";
        break;
    }
    using (ListPool<string> listPool1 = new ListPool<string>())
    {
      using (ListPool<string> listPool2 = new ListPool<string>())
      {
        foreach (Kingdom attacker in war.getAttackers())
          TooltipLibrary.addParty(attacker, listPool1);
        foreach (Kingdom diedAttacker in war.getDiedAttackers())
          TooltipLibrary.addParty(diedAttacker, listPool1, pDied: true);
        foreach (Kingdom pastAttacker in war.getPastAttackers())
          TooltipLibrary.addParty(pastAttacker, listPool1, true);
        foreach (Kingdom defender in war.getDefenders())
          TooltipLibrary.addParty(defender, listPool2);
        foreach (Kingdom diedDefender in war.getDiedDefenders())
          TooltipLibrary.addParty(diedDefender, listPool2, pDied: true);
        foreach (Kingdom pastDefender in war.getPastDefenders())
          TooltipLibrary.addParty(pastDefender, listPool2, true);
        if (listPool1.Count > 13)
        {
          int num = listPool1.Count - 12;
          while (listPool1.Count > 12)
            listPool1.Pop<string>();
          listPool1.Add($"... and {num.ToString()} more");
        }
        if (listPool2.Count > 13)
        {
          int num = listPool2.Count - 12;
          while (listPool2.Count > 12)
            listPool2.Pop<string>();
          listPool2.Add($"... and {num.ToString()} more");
        }
        component1.text += string.Join("\n", (IEnumerable<string>) listPool1);
        component2.text += string.Join("\n", (IEnumerable<string>) listPool2);
        this.showTabBannerTip(pTooltip, pData);
      }
    }
  }

  private static void addParty(
    Kingdom pKingdom,
    ListPool<string> pPartyList,
    bool pLeft = false,
    bool pDied = false)
  {
    string name = pKingdom.name;
    string colorText = pKingdom.getColor().color_text;
    string str = "";
    if (pLeft)
      str = Toolbox.coloredText(" (left)", ColorStyleLibrary.m.color_text_grey_dark);
    else if (pDied)
      str = Toolbox.coloredText(" (died)", ColorStyleLibrary.m.color_text_grey);
    else
      pKingdom.hasDied();
    pPartyList.Add(Toolbox.coloredText(name, colorText) + str);
  }

  private void showAlliance(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Alliance alliance = pData.alliance;
    pTooltip.setDescription(alliance.getMotto());
    string colorText = alliance.getColor().color_text;
    pTooltip.setTitle(alliance.name, "alliance", colorText);
    int age = alliance.getAge();
    this.setIconValue(pTooltip, "i_age", (float) age);
    this.setIconValue(pTooltip, "i_population", (float) alliance.countPopulation());
    this.setIconValue(pTooltip, "i_army", (float) alliance.countWarriors());
    pTooltip.addLineIntText("adults", alliance.countAdults());
    pTooltip.addLineIntText("children", alliance.countChildren());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("tip_alliance_kingdoms", alliance.countKingdoms());
    pTooltip.addLineIntText("tip_alliance_buildings", alliance.countBuildings());
    pTooltip.addLineIntText("territory", alliance.countZones());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("housed", alliance.countHoused());
    pTooltip.addLineIntText("homeless", alliance.countHomeless());
    pTooltip.addLineBreak();
    pTooltip.addLineIntText("deaths", alliance.getTotalDeaths());
    foreach (BannerBase bannerBase in ((Component) pTooltip).transform.FindAllRecursive<AllianceBanner>())
      bannerBase.load((NanoObject) alliance);
    this.showTabBannerTip(pTooltip, pData);
  }

  private KingdomOpinion showKingdomOpinion(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Kingdom kingdom = pData.kingdom;
    pTooltip.name.text = Toolbox.coloredText(kingdom.name, kingdom.getColor().color_text);
    KingdomOpinion opinion = World.world.diplomacy.getRelation(kingdom, SelectedMetas.selected_kingdom).getOpinion(SelectedMetas.selected_kingdom, kingdom);
    foreach (OpinionAsset key in opinion.results.Keys)
    {
      int result = opinion.results[key];
      string translationKey = key.getTranslationKey(result);
      pTooltip.addOpinion(new TooltipOpinionInfo(translationKey, result));
    }
    return opinion;
  }

  private string getArrowUp(long pValue)
  {
    if (pValue < 10L)
      return " <size=4>↗</size>";
    return pValue < 100L ? " <size=4>↗↗</size>" : " <size=4>↗↗↗</size>";
  }

  private string getArrowDown(long pValue)
  {
    pValue = (long) Mathf.Abs((float) pValue);
    if (pValue < 10L)
      return " <size=4>↘</size>";
    return pValue < 100L ? " <size=4>↘↘</size>" : " <size=4>↘↘↘</size>";
  }

  private void showGraphResource(Tooltip pTooltip, string pType, TooltipData pData)
  {
    // ISSUE: unable to decompile the method.
  }

  private void showGraphMultiResource(Tooltip pTooltip, string pType, TooltipData pData)
  {
    // ISSUE: unable to decompile the method.
  }

  private void showGenderData(Tooltip pTooltip, string pType, TooltipData pData)
  {
    string pValue = pData.custom_data_string["age_range"];
    pTooltip.name.text = pValue;
    HistoryDataAsset historyDataAsset1 = AssetManager.history_data_library.get("males");
    HistoryDataAsset historyDataAsset2 = AssetManager.history_data_library.get("females");
    pTooltip.addLineText("age_range", pValue);
    pTooltip.addLineBreak();
    pTooltip.addLineIntText(historyDataAsset1.getLocaleID(), pData.custom_data_int["males"], historyDataAsset1.tooltip_color_hex);
    pTooltip.addLineIntText(historyDataAsset2.getLocaleID(), pData.custom_data_int["females"], historyDataAsset2.tooltip_color_hex);
  }

  private void showCityResource(Tooltip pTooltip, string pType, TooltipData pData)
  {
    if (SelectedMetas.selected_city.isRekt())
      return;
    ResourceAsset resource = pData.resource;
    pTooltip.name.text = resource.getTranslatedName();
    pTooltip.clearTextRows();
    pTooltip.addLineIntText("amount", SelectedMetas.selected_city.getResourcesAmount(resource.id));
  }

  private void showCityResourceFood(Tooltip pTooltip, string pType, TooltipData pData)
  {
    if (SelectedMetas.selected_city.isRekt())
      return;
    ResourceAsset resource = pData.resource;
    pTooltip.name.text = resource.getTranslatedName();
    pTooltip.clearTextRows();
    pTooltip.addLineIntText("amount", SelectedMetas.selected_city.getResourcesAmount(resource.id));
    pTooltip.addLineBreak();
    if ((double) resource.restore_health != 0.0)
      pTooltip.addLineText("health", resource.restore_health.ToText());
    if (resource.restore_mana != 0)
      pTooltip.addLineIntText("mana", resource.restore_mana);
    if (resource.restore_stamina != 0)
      pTooltip.addLineIntText("stamina", resource.restore_stamina);
    pTooltip.addLineBreak();
    if (resource.restore_nutrition != 0)
      pTooltip.addLineIntText("nutrition", resource.restore_nutrition);
    if (resource.restore_happiness == 0)
      return;
    pTooltip.addLineIntText("happiness", resource.restore_happiness);
  }

  private void showMapMeta(Tooltip pTooltip, string pType, TooltipData pData)
  {
    MapMetaData mapMeta = pData.map_meta;
    string pColor1 = (string) null;
    if (mapMeta.saveVersion > Config.WORLD_SAVE_VERSION)
    {
      pTooltip.setBottomDescription(LocalizedTextManager.getText("future_save_version"), "#FB2C21");
      pColor1 = "#FB2C21";
    }
    pTooltip.name.text = mapMeta.mapStats.name;
    ((Graphic) pTooltip.name).color = mapMeta.mapStats.getArchitectMood().getColorText();
    if (mapMeta.modded)
    {
      if (pColor1 != null)
        pTooltip.addBottomDescription("\n\n");
      if (!Config.MODDED)
      {
        pTooltip.addBottomDescription(LocalizedTextManager.getText("modded_world_no_mod_active"), "#FB2C21");
        pColor1 = "#FB2C21";
      }
      else
      {
        pTooltip.addBottomDescription(LocalizedTextManager.getText("modded_world"), "#45FFFE");
        if (pColor1 == null)
          pColor1 = "#45FFFE";
      }
    }
    if (pColor1 != null)
      pTooltip.name.text = Toolbox.coloredText(mapMeta.mapStats.name, pColor1);
    if (mapMeta.mapStats.description != "")
      pTooltip.addDescription(mapMeta.mapStats.description);
    else
      pTooltip.addDescription("WORLDBOX, HO!");
    string pColor2 = "#95DD5D";
    pTooltip.addLineIntText("world_age", Date.getYear(mapMeta.mapStats.world_time), pColor2);
    pTooltip.addLineIntText("kingdoms", mapMeta.kingdoms, pColor2);
    pTooltip.addLineIntText("cultures", mapMeta.cultures, pColor2);
    pTooltip.addLineIntText("villages", mapMeta.cities, pColor2);
    pTooltip.addLineIntText("mobs", mapMeta.mobs, pColor2);
    pTooltip.addLineIntText("population", mapMeta.population, pColor2);
    if (pTooltip.stats_description.text.Length > 0)
      pTooltip.addLineBreak();
    pTooltip.addLineText("created", mapMeta.temp_date_string);
  }

  private void showEquipment(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Item pItem = pData.item;
    Transform transform = ((Component) pTooltip).transform.Find("Description/IconBG/LegendaryBG");
    Image component1 = ((Component) ((Component) pTooltip).transform.Find("Description/IconBG/ItemIcon")).GetComponent<Image>();
    Text component2 = ((Component) ((Component) pTooltip).transform.Find("Equipment Type/EquipmentText")).GetComponent<Text>();
    EquipmentAsset asset = pItem.getAsset();
    Sprite sprite = pItem.getSprite();
    component1.sprite = sprite;
    Text component3 = ((Component) ((Component) pTooltip).transform.Find("Item Description/item_description_text")).GetComponent<Text>();
    string qualityColor = pItem.getQualityColor();
    Transform recursive = ((Component) pTooltip).transform.FindRecursive("Stats");
    bool flag1 = asset.isAvailable();
    string name = pItem.getName();
    pTooltip.name.text = Toolbox.coloredText(name, qualityColor);
    if (!flag1)
    {
      if (asset.unlocked_with_achievement)
      {
        string str = LocalizedTextManager.getText("item_locked_tooltip_text_achievement").Replace("$achievement_id$", $"<color=#00ffffff>{asset.getAchievementLocaleID().Localize()}</color>");
        component3.text = str;
      }
      else
        component3.text = LocalizedTextManager.getText("item_locked_tooltip_text_exploration");
    }
    else
    {
      BaseStatsHelper.showItemMods(pTooltip.stats_description, pTooltip.stats_values, pItem);
      BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, pItem.getFullStats());
      pTooltip.addLineBreak();
      pTooltip.addLineText("durability", pItem.getDurabilityString());
      if (pItem.data.kills > 0)
      {
        pTooltip.addLineBreak();
        pTooltip.addItemText("creature_statistics_kills", (float) pItem.data.kills, pAddPlus: false, pMainColor: "#FF9B1C");
      }
      string pKey = asset.id + "_description";
      bool flag2 = LocalizedTextManager.stringExists(pKey);
      ((Component) ((Component) component3).transform.parent).gameObject.SetActive(flag2);
      if (flag2)
        component3.text = LocalizedTextManager.getText(pKey);
    }
    ((Component) recursive).gameObject.SetActive(flag1);
    Rarity quality = pItem.getQuality();
    ((Component) transform).gameObject.SetActive(quality == Rarity.R3_Legendary);
    pTooltip.description.alignment = (TextAnchor) 3;
    ((Component) component2).GetComponent<LocalizedText>().setKeyAndUpdate(pItem.getItemKeyType());
    ((Graphic) component2).color = Toolbox.makeColor(qualityColor);
    string pDescription = Toolbox.coloredText(pItem.getItemDescription(), "#FFFFFF");
    pTooltip.setDescription(pDescription);
    this.showTabBannerTip(pTooltip, pData);
  }

  private void showEquipmentInEditor(Tooltip pTooltip, string pType, TooltipData pData)
  {
    EquipmentAsset itemAsset = pData.item_asset;
    string text1 = LocalizedTextManager.getText("achievement_tip_hidden");
    if (!itemAsset.isAvailable())
    {
      pTooltip.name.text = text1;
      if (itemAsset.unlocked_with_achievement)
      {
        string pDescription = LocalizedTextManager.getText("item_locked_tooltip_text_achievement").Replace("$achievement_id$", $"<color=#00ffffff>{itemAsset.getAchievementLocaleID().Localize()}</color>");
        pTooltip.setDescription(pDescription);
      }
      else
        pTooltip.setDescription(LocalizedTextManager.getText("item_locked_tooltip_text_exploration"));
      ((Component) ((Component) pTooltip).transform.FindRecursive("Stats")).gameObject.SetActive(false);
    }
    else
    {
      string pName;
      string pMaterial;
      ItemTools.getTooltipTitle(itemAsset, out pName, out pMaterial);
      pTooltip.name.text = pMaterial + pName;
      string descriptionId = itemAsset.getDescriptionID();
      string pDescription = descriptionId != null ? descriptionId.Localize() : (string) null;
      if (!string.IsNullOrEmpty(pDescription))
        pTooltip.setDescription(pDescription);
      else
        pTooltip.resetDescription();
      if (!string.IsNullOrEmpty(pData.tip_description_2))
      {
        string text2 = LocalizedTextManager.getText(pData.tip_description_2);
        pTooltip.setBottomDescription(text2);
      }
      BaseStatsHelper.showBaseStats(pTooltip.stats_description, pTooltip.stats_values, itemAsset.base_stats);
    }
  }

  private void showWorldLaw(Tooltip pTooltip, string pType, TooltipData pData)
  {
    WorldLawAsset worldLaw = pData.world_law;
    pTooltip.name.text = LocalizedTextManager.getText(worldLaw.getLocaleID());
    string pDescription = LocalizedTextManager.getText(worldLaw.getDescriptionID());
    if (!InputHelpers.mouseSupported)
    {
      if (worldLaw.id != "world_law_cursed_world")
        pDescription = pDescription + "\n\n" + Toolbox.coloredText(LocalizedTextManager.getText("world_laws_tip_mobile_tap"), "#999999");
      else if (!worldLaw.isEnabled())
        pDescription = pDescription + "\n\n" + Toolbox.coloredText(LocalizedTextManager.getText("world_laws_tip_mobile_tap_cursed"), "#999999");
    }
    pTooltip.setDescription(pDescription);
    string descriptionId2 = worldLaw.getDescriptionID2();
    if (!LocalizedTextManager.stringExists(descriptionId2))
      return;
    string text = LocalizedTextManager.getText(descriptionId2);
    pTooltip.setBottomDescription(text);
  }

  private void showWorldAge(Tooltip pTooltip, string pType, TooltipData pData)
  {
    string tipName = pData.tip_name;
    WorldAgeAsset worldAgeAsset = AssetManager.era_library.get(tipName);
    string localeId = worldAgeAsset.getLocaleID();
    string descriptionId = worldAgeAsset.getDescriptionID();
    pTooltip.name.text = Toolbox.coloredText(localeId, Toolbox.colorToHex(Color32.op_Implicit(worldAgeAsset.title_color)), true);
    string pDescription = LocalizedTextManager.getText(descriptionId);
    Sprite sprite = worldAgeAsset.getSprite();
    ((Component) ((Component) pTooltip).transform.Find("Headline/IconLeft")).GetComponent<Image>().sprite = sprite;
    ((Component) ((Component) pTooltip).transform.Find("Headline/IconRight")).GetComponent<Image>().sprite = sprite;
    if (Config.isMobile)
      pDescription = pDescription + "\n\n" + Toolbox.coloredText(LocalizedTextManager.getText("world_age_tip_mobile_tap"), "#999999");
    pTooltip.setDescription(pDescription);
  }

  private void showStatsData(Tooltip pTooltip, string pType, TooltipData pData)
  {
    CustomDataContainer<string> customDataString = pData.custom_data_string;
    string pValue1;
    if (customDataString.TryGetValue("value", out pValue1))
      pTooltip.addLineText(pData.tip_name, pValue1);
    string pValue2;
    if (!customDataString.TryGetValue("max_value", out pValue2))
      return;
    pTooltip.addLineText("max", pValue2);
  }

  private void opinionListToStatsLoyalty(Tooltip pTooltip, string pType, TooltipData pData)
  {
    if (pTooltip.opinion_list.Count == 0)
      return;
    pTooltip.opinion_list.Sort(new Comparison<TooltipOpinionInfo>(this.sorter));
    string pStats = "";
    string pValues = "";
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 0; index < pTooltip.opinion_list.Count; ++index)
    {
      TooltipOpinionInfo opinion = pTooltip.opinion_list[index];
      if (opinion.value > 0)
        flag1 = true;
      if (((opinion.value >= 0 || flag2 ? 0 : (index > 0 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0)
      {
        flag2 = true;
        pStats += "\n---";
        pValues += "\n---";
      }
      if (index > 0)
      {
        pStats += "\n";
        pValues += "\n";
      }
      if (opinion.value > 0)
      {
        pValues += Toolbox.coloredText(opinion.value.ToString("+##,#;-##,#;0"), "#43FF43");
        pStats += Toolbox.coloredText(LocalizedTextManager.getText(opinion.translation_key), "#43FF43");
      }
      else
      {
        pValues += Toolbox.coloredText(opinion.value.ToString("+##,#;-##,#;0"), "#FB2C21");
        pStats += Toolbox.coloredText(LocalizedTextManager.getText(opinion.translation_key), "#FB2C21");
      }
    }
    pTooltip.addStatValues(pStats, pValues);
  }

  private void opinionListToStatsDiplomacy(Tooltip pTooltip, string pType, TooltipData pData)
  {
    KingdomOpinion kingdomOpinion = this.showKingdomOpinion(pTooltip, pType, pData);
    if (pTooltip.opinion_list.Count == 0)
    {
      pTooltip.stats_container.SetActive(false);
    }
    else
    {
      pTooltip.opinion_list.Sort(new Comparison<TooltipOpinionInfo>(this.sorter));
      string str1 = "";
      string str2 = "";
      int total = kingdomOpinion.total;
      string str3;
      string str4;
      if (total >= 0)
      {
        str3 = str2 + Toolbox.coloredText(total.ToText(), "#43FF43");
        str4 = str1 + Toolbox.coloredText(LocalizedTextManager.getText("opinion_total"), "#43FF43");
      }
      else
      {
        str3 = str2 + Toolbox.coloredText(total.ToText(), "#FB2C21");
        str4 = str1 + Toolbox.coloredText(LocalizedTextManager.getText("opinion_total"), "#FB2C21");
      }
      string str5 = str4 + "\n------------";
      string str6 = str3 + "\n------------";
      string pDescriptionString = str5 + "\n";
      string pValuesString = str6 + "\n";
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < pTooltip.opinion_list.Count; ++index)
      {
        TooltipOpinionInfo opinion = pTooltip.opinion_list[index];
        if (opinion.value > 0)
          flag1 = true;
        if (((opinion.value >= 0 || flag2 ? 0 : (index > 0 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0)
        {
          flag2 = true;
          pDescriptionString += "\n---";
          pValuesString += "\n---";
        }
        if (index > 0)
        {
          pDescriptionString += "\n";
          pValuesString += "\n";
        }
        if (opinion.value > 0)
        {
          pValuesString += Toolbox.coloredText(opinion.value.ToString("+##,#;-##,#;0"), "#43FF43");
          pDescriptionString += Toolbox.coloredText(LocalizedTextManager.getText(opinion.translation_key), "#43FF43");
        }
        else
        {
          pValuesString += Toolbox.coloredText(opinion.value.ToString("+##,#;-##,#;0"), "#FB2C21");
          pDescriptionString += Toolbox.coloredText(LocalizedTextManager.getText(opinion.translation_key), "#FB2C21");
        }
      }
      Transform transform = ((Component) pTooltip).transform.Find("StatsOpinion");
      Text component1 = ((Component) transform.Find("StatsDescription")).GetComponent<Text>();
      Text component2 = ((Component) transform.Find("StatsValues")).GetComponent<Text>();
      component1.text = string.Empty;
      component2.text = string.Empty;
      pTooltip.showOpinion(pDescriptionString, pValuesString, component1, component2);
      ((Component) component2).GetComponent<LocalizedText>().checkSpecialLanguages();
      ((Component) component1).GetComponent<LocalizedText>().checkSpecialLanguages();
      pTooltip.stats_container.SetActive(true);
    }
  }

  private void showTaxonomy(Tooltip pTooltip, string pType, TooltipData pData)
  {
    ActorAsset actorAsset1 = AssetManager.actor_library.get(pData.subspecies.data.species_id);
    string tipName = pData.tip_name;
    string pType1 = tipName;
    string taxonomyRank = actorAsset1.getTaxonomyRank(pType1);
    string colorForTaxonomy = ColorStyleLibrary.m.getColorForTaxonomy(tipName);
    ((Component) pTooltip.name).GetComponent<LocalizedText>().setKeyAndUpdate(tipName);
    ((Graphic) pTooltip.name).color = Toolbox.makeColor(colorForTaxonomy);
    Text name = pTooltip.name;
    name.text = $"{name.text}\n{Toolbox.firstLetterToUpper(taxonomyRank)}";
    pTooltip.setDescription(LocalizedTextManager.getText("taxonomy_description_tooltip"));
    if (pTooltip.pool_icons == null)
    {
      Transform recursive = ((Component) pTooltip).transform.FindRecursive("Assets");
      StatsIcon pPrefab = Resources.Load<StatsIcon>("ui/PrefabTextIconTooltipBig");
      pTooltip.pool_icons = new ObjectPoolGenericMono<StatsIcon>(pPrefab, recursive);
    }
    foreach (ActorAsset actorAsset2 in AssetManager.actor_library.list)
    {
      if (!actorAsset2.unit_zombie && actorAsset2.show_in_taxonomy_tooltip && actorAsset2.isTaxonomyRank(tipName, taxonomyRank))
      {
        StatsIcon next = pTooltip.pool_icons.getNext();
        Image icon = next.getIcon();
        icon.sprite = actorAsset2.getSpriteIcon();
        next.text.text = actorAsset2.getTranslatedName();
        if (actorAsset2.isAvailable())
          ((Graphic) icon).color = Toolbox.color_white;
        else
          ((Graphic) icon).color = Toolbox.color_black;
      }
    }
  }

  private void showColorCounter(Tooltip pTooltip, string pType, TooltipData pData)
  {
    int num1 = pData.custom_data_int["color_count"];
    int num2 = pData.custom_data_int["color_current"];
    pTooltip.setDescription($"{num2.ToString()} / {num1.ToString()}");
  }

  private void showGameLanguage(Tooltip pTooltip, string pType, TooltipData pData)
  {
    GameLanguageAsset gameLanguageAsset = pData.game_language_asset;
    pTooltip.name.text = gameLanguageAsset.name;
    if (!gameLanguageAsset.export || !gameLanguageAsset.show_translators)
      return;
    GameLanguageData languageData = gameLanguageAsset.getLanguageData();
    if (languageData == null)
      return;
    string[] active = languageData.active;
    if ((active != null ? (active.Length != 0 ? 1 : 0) : 0) != 0)
    {
      pTooltip.resetDescription();
      pTooltip.addDescription("translators_current_translators".Localize() + ":");
      pTooltip.description.text = $"<b>{pTooltip.description.text}</b>";
      foreach (string str in languageData.active)
        pTooltip.addDescription("\n" + str);
    }
    string[] inactive = languageData.inactive;
    if ((inactive != null ? (inactive.Length != 0 ? 1 : 0) : 0) == 0)
      return;
    pTooltip.resetBottomDescription();
    pTooltip.addBottomDescription("translators_past_translators".Localize() + ":");
    pTooltip.description_2.text = $"<b>{pTooltip.description_2.text}</b>";
    foreach (string str in languageData.inactive)
      pTooltip.addBottomDescription("\n" + str);
  }

  private void showAchievement(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Achievement achievement = pData.achievement;
    Image component1 = ((Component) ((Component) pTooltip).transform.FindRecursive("IconLeft")).GetComponent<Image>();
    Image component2 = ((Component) ((Component) pTooltip).transform.FindRecursive("IconRight")).GetComponent<Image>();
    if (achievement.isUnlocked())
    {
      ((Graphic) component1).color = Toolbox.color_white;
      ((Graphic) component2).color = Toolbox.color_white;
    }
    else
    {
      ((Graphic) component1).color = Toolbox.color_black;
      ((Graphic) component2).color = Toolbox.color_black;
    }
    Sprite icon = achievement.getIcon();
    if (Object.op_Inequality((Object) icon, (Object) null))
    {
      component1.sprite = icon;
      component2.sprite = icon;
    }
    string localeId = achievement.getLocaleID();
    ((Component) pTooltip.name).GetComponent<LocalizedText>().setKeyAndUpdate(localeId);
    string pDescription = (!achievement.hidden || achievement.isUnlocked() ? achievement.getDescriptionID() : "achievement_tip_hidden").Localize().Replace("$lifeissimhours$", 24f.ToText());
    pTooltip.setDescription(pDescription);
    bool pUnlocked = achievement.isUnlocked();
    string pName1 = pUnlocked ? "unlocked" : "locked";
    string pName2 = !pUnlocked ? "unlocked" : "locked";
    Transform recursive = ((Component) pTooltip).transform.FindRecursive(pName1);
    ((Component) recursive.parent).gameObject.SetActive(achievement.unlocks_something);
    ((Component) ((Component) pTooltip).transform.FindRecursive(pName2)).gameObject.SetActive(false);
    if (!achievement.unlocks_something)
      return;
    ((Component) recursive).gameObject.SetActive(true);
    string pString = pData.achievement.unlock_assets.Count > 1 ? "unlocks_goodies" : "unlocks_goodie";
    pTooltip.setBottomDescription(pString.Localize());
    ObjectPoolGenericMono<StatsIcon> objectPoolGenericMono;
    if (!pUnlocked)
    {
      if (pTooltip.pool_icons == null)
      {
        StatsIcon pPrefab = Resources.Load<StatsIcon>("ui/AchievementGoodieTooltipLocked");
        pTooltip.pool_icons = new ObjectPoolGenericMono<StatsIcon>(pPrefab, recursive);
      }
      objectPoolGenericMono = pTooltip.pool_icons;
    }
    else
    {
      if (pTooltip.pool_icons_2 == null)
      {
        StatsIcon pPrefab = Resources.Load<StatsIcon>("ui/AchievementGoodieTooltipUnlocked");
        pTooltip.pool_icons_2 = new ObjectPoolGenericMono<StatsIcon>(pPrefab, recursive);
      }
      objectPoolGenericMono = pTooltip.pool_icons_2;
    }
    foreach (BaseUnlockableAsset unlockAsset in achievement.unlock_assets)
      ((Component) objectPoolGenericMono.getNext()).GetComponent<AchievementGoodie>().load(unlockAsset, pUnlocked);
  }

  public int sorter(TooltipOpinionInfo p1, TooltipOpinionInfo p2) => p2.value.CompareTo(p1.value);

  protected void setIconSprite(Tooltip pTooltip, string pName, string pIconName)
  {
    Transform recursive = ((Component) pTooltip).transform.FindRecursive(pName);
    if (Object.op_Equality((Object) recursive, (Object) null))
      Debug.LogError((object) ("No icon with this name! " + pName));
    else
      ((Component) recursive).GetComponent<StatsIcon>().getIcon().sprite = SpriteTextureLoader.getSprite("ui/Icons/" + pIconName);
  }

  protected void setIconValue(
    Tooltip pTooltip,
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    Transform recursive = ((Component) pTooltip).transform.FindRecursive(pName);
    if (Object.op_Equality((Object) recursive, (Object) null))
    {
      Debug.LogError((object) ("No icon with this name! " + pName));
    }
    else
    {
      StatsIcon component = ((Component) recursive).GetComponent<StatsIcon>();
      component.enable_animation = false;
      component.setValue(pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
    }
  }

  private void showTabBannerTip(Tooltip pTooltip, TooltipData pData)
  {
    if (!Config.isComputer && !Config.isEditor)
      return;
    CustomDataContainer<bool> customDataBool = pData.custom_data_bool;
    if ((customDataBool != null ? (customDataBool["tab_banner"] ? 1 : 0) : 0) == 0)
      return;
    string text = LocalizedTextManager.getText("tab_banner_show_window");
    string pDescription = AssetManager.hotkey_library.replaceSpecialTextKeys(text);
    pTooltip.setBottomDescription(pDescription);
  }

  private void initDebug()
  {
    TooltipAsset pAsset1 = new TooltipAsset();
    pAsset1.id = "debug_asset";
    pAsset1.prefab_id = "tooltips/tooltip_asset_debug";
    pAsset1.callback = new TooltipShowAction(this.showAssetDebug);
    this.add(pAsset1);
    TooltipAsset pAsset2 = new TooltipAsset();
    pAsset2.id = "debug_collection";
    pAsset2.prefab_id = "tooltips/tooltip_collection_data";
    pAsset2.callback = new TooltipShowAction(this.showCollectionData);
    this.add(pAsset2);
  }

  private void showAssetDebug(Tooltip pTooltip, string pType, TooltipData pData)
  {
    if (pData.tip_name == "actor")
      this.showActorAssetDebug(pTooltip, pType, pData);
    if (!(pData.tip_name == "building"))
      return;
    this.showBuildingAssetDebug(pTooltip, pType, pData);
  }

  private void showActorAssetDebug(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Sprite spriteIcon = BaseDebugAssetElement<ActorAsset>.selected_asset.getSpriteIcon();
    ((Component) ((Component) pTooltip).transform.FindRecursive("IconSpecial")).GetComponent<Image>().sprite = spriteIcon;
    ((Component) ((Component) pTooltip).transform.FindRecursive("IconRace")).GetComponent<Image>().sprite = spriteIcon;
    using (ListPool<string> pFields = new ListPool<string>()
    {
      "id",
      "icon",
      "has_skin",
      "banner_id",
      "skin_civ_default_male",
      "skin_civ_default_female"
    })
      this.showAssetDebug<ActorAsset>(pTooltip, pFields);
  }

  private void showBuildingAssetDebug(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Sprite sprite = SpriteTextureLoader.getSprite("ui/Icons/iconHouseTier0");
    ((Component) ((Component) pTooltip).transform.FindRecursive("IconSpecial")).GetComponent<Image>().sprite = sprite;
    ((Component) ((Component) pTooltip).transform.FindRecursive("IconRace")).GetComponent<Image>().sprite = sprite;
    using (ListPool<string> pFields = new ListPool<string>()
    {
      "id",
      "civ_kingdom",
      "can_be_upgraded",
      "upgrade_to",
      "housing_slots",
      "spawn_units_asset",
      "spawn_drop_id"
    })
      this.showAssetDebug<BuildingAsset>(pTooltip, pFields);
  }

  private void showAssetDebug<TAsset>(Tooltip pTooltip, ListPool<string> pFields) where TAsset : Asset
  {
    TAsset selectedAsset = BaseDebugAssetElement<TAsset>.selected_asset;
    pTooltip.name.text = selectedAsset.id;
    FieldInfoList componentInChildren = ((Component) pTooltip).GetComponentInChildren<FieldInfoList>();
    componentInChildren.init<TAsset>(pFields);
    componentInChildren.setData((object) selectedAsset);
    pTooltip.setDescription("Need description to fix rounded tooltip");
  }

  private void showCollectionData(Tooltip pTooltip, string pType, TooltipData pData)
  {
    Dictionary<string, string> selectedFieldData = FieldInfoList.selected_field_data;
    if (selectedFieldData == null)
    {
      pTooltip.setDescription("Nothing to show");
    }
    else
    {
      FieldInfoList componentInChildren = ((Component) pTooltip).GetComponentInChildren<FieldInfoList>();
      componentInChildren.checkInitPool();
      foreach (KeyValuePair<string, string> keyValuePair in selectedFieldData)
        componentInChildren.addRow(keyValuePair.Key, keyValuePair.Value);
      pTooltip.setDescription("need description to fix rounded tooltip");
    }
  }
}
