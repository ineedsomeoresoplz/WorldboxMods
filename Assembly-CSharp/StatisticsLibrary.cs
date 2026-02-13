// Decompiled with JetBrains decompiler
// Type: StatisticsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public class StatisticsLibrary : AssetLibrary<StatisticsAsset>
{
  internal static readonly List<StatisticsAsset> power_tracker_pool = new List<StatisticsAsset>();
  private static readonly string _unknown_text = Toolbox.coloredString("???", ColorStyleLibrary.m.color_dead_text);

  public override void init()
  {
    base.init();
    this.addStatsGeneralMain();
    this.addStats();
    this.addStatsNoos();
    this.addStatsDeaths();
    this.addStatsTiles();
    this.addStatsBiomes();
  }

  private void addStats()
  {
    StatisticsAsset pAsset1 = new StatisticsAsset();
    pAsset1.id = "world_name";
    pAsset1.rarity = 1;
    pAsset1.string_action = (StatisticsStringAction) (_ => World.world.map_stats.name ?? "");
    this.add(pAsset1);
    StatisticsAsset pAsset2 = new StatisticsAsset();
    pAsset2.id = "world_statistics_infected";
    pAsset2.list_window_meta_type = MetaType.Unit;
    pAsset2.localized_key = "world_statistics_infected";
    pAsset2.steam_activity = "#Status_stat_value";
    pAsset2.rarity = 1;
    pAsset2.path_icon = "ui/Icons/actor_traits/iconInfected";
    pAsset2.long_action = (StatisticsLongAction) (_ =>
    {
      long num = 0;
      List<Actor> simpleList = World.world.units.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
      {
        if (simpleList[index].isSick())
          ++num;
      }
      return num;
    });
    pAsset2.is_world_statistics = true;
    pAsset2.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset2);
    StatisticsAsset pAsset3 = new StatisticsAsset();
    pAsset3.id = "world_statistics_houses";
    pAsset3.localized_key_description = "houses".Description();
    pAsset3.list_window_meta_type = MetaType.City;
    pAsset3.path_icon = "ui/Icons/iconBuildings";
    pAsset3.long_action = (StatisticsLongAction) (_ =>
    {
      long num = 0;
      List<Building> simpleList = World.world.buildings.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
      {
        if (simpleList[index].asset.city_building)
          ++num;
      }
      return num;
    });
    pAsset3.is_world_statistics = true;
    pAsset3.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset3);
    StatisticsAsset pAsset4 = new StatisticsAsset();
    pAsset4.id = "world_statistics_houses_built";
    pAsset4.list_window_meta_type = MetaType.World;
    pAsset4.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobBuilder";
    pAsset4.long_action = (StatisticsLongAction) (_ => World.world.map_stats.housesBuilt);
    this.add(pAsset4);
    StatisticsAsset pAsset5 = new StatisticsAsset();
    pAsset5.id = "houses";
    pAsset5.rarity = 4;
    pAsset5.path_icon = "ui/Icons/iconBuildings";
    pAsset5.string_action = (StatisticsStringAction) (_ =>
    {
      string str = "";
      long pLong1 = this.get("world_statistics_houses").long_action((StatisticsAsset) null);
      long pLong2 = this.get("world_statistics_houses_destroyed").long_action((StatisticsAsset) null);
      return pLong1 < 1L && pLong2 < 1L ? "" : (str + LocalizedTextManager.getText("world_statistics_houses_all")).Replace("$houses$", pLong1.ToText()).Replace("$destroyed$", pLong2.ToText());
    });
    this.add(pAsset5);
    StatisticsAsset pAsset6 = new StatisticsAsset();
    pAsset6.id = "alliances";
    pAsset6.localized_key = "statistics_alliances";
    pAsset6.list_window_meta_type = MetaType.Alliance;
    pAsset6.path_icon = "ui/Icons/iconAllianceList";
    pAsset6.long_action = (StatisticsLongAction) (_ => (long) World.world.alliances.Count);
    this.add(pAsset6);
    StatisticsAsset pAsset7 = new StatisticsAsset();
    pAsset7.id = "books";
    pAsset7.localized_key = "books";
    pAsset7.list_window_meta_type = MetaType.Language;
    pAsset7.path_icon = "ui/Icons/iconBooks";
    pAsset7.long_action = (StatisticsLongAction) (_ => (long) World.world.books.Count);
    this.add(pAsset7);
    StatisticsAsset pAsset8 = new StatisticsAsset();
    pAsset8.id = "clans";
    pAsset8.localized_key = "statistics_clans";
    pAsset8.list_window_meta_type = MetaType.Clan;
    pAsset8.path_icon = "ui/Icons/iconClanList";
    pAsset8.long_action = (StatisticsLongAction) (_ => (long) World.world.clans.Count);
    this.add(pAsset8);
    StatisticsAsset pAsset9 = new StatisticsAsset();
    pAsset9.id = "cultures";
    pAsset9.localized_key = "statistics_cultures";
    pAsset9.list_window_meta_type = MetaType.Culture;
    pAsset9.path_icon = "ui/Icons/iconCultureList";
    pAsset9.long_action = (StatisticsLongAction) (_ => (long) World.world.cultures.Count);
    this.add(pAsset9);
    StatisticsAsset pAsset10 = new StatisticsAsset();
    pAsset10.id = "families";
    pAsset10.localized_key = "statistics_families";
    pAsset10.list_window_meta_type = MetaType.Family;
    pAsset10.path_icon = "ui/Icons/iconFamilyList";
    pAsset10.long_action = (StatisticsLongAction) (_ => (long) World.world.families.Count);
    this.add(pAsset10);
    StatisticsAsset pAsset11 = new StatisticsAsset();
    pAsset11.id = "plots";
    pAsset11.localized_key = "statistics_plots";
    pAsset11.list_window_meta_type = MetaType.Plot;
    pAsset11.path_icon = "ui/Icons/iconPlotList";
    pAsset11.long_action = (StatisticsLongAction) (_ => (long) World.world.plots.Count);
    this.add(pAsset11);
    StatisticsAsset pAsset12 = new StatisticsAsset();
    pAsset12.id = "languages";
    pAsset12.localized_key = "statistics_languages";
    pAsset12.list_window_meta_type = MetaType.Language;
    pAsset12.path_icon = "ui/Icons/iconLanguageList";
    pAsset12.long_action = (StatisticsLongAction) (_ => (long) World.world.languages.Count);
    this.add(pAsset12);
    StatisticsAsset pAsset13 = new StatisticsAsset();
    pAsset13.id = "religions";
    pAsset13.localized_key = "statistics_religions";
    pAsset13.list_window_meta_type = MetaType.Religion;
    pAsset13.path_icon = "ui/Icons/iconReligionList";
    pAsset13.long_action = (StatisticsLongAction) (_ => (long) World.world.religions.Count);
    this.add(pAsset13);
    StatisticsAsset pAsset14 = new StatisticsAsset();
    pAsset14.id = "subspecies";
    pAsset14.localized_key = "statistics_subspecies";
    pAsset14.list_window_meta_type = MetaType.Subspecies;
    pAsset14.path_icon = "ui/Icons/iconSpecies";
    pAsset14.long_action = (StatisticsLongAction) (_ => (long) World.world.subspecies.Count);
    this.add(pAsset14);
    StatisticsAsset pAsset15 = new StatisticsAsset();
    pAsset15.id = "wars";
    pAsset15.localized_key = "statistics_wars";
    pAsset15.list_window_meta_type = MetaType.War;
    pAsset15.path_icon = "ui/Icons/iconWar";
    pAsset15.long_action = (StatisticsLongAction) (_ => World.world.wars.countActiveWars());
    this.add(pAsset15);
    StatisticsAsset pAsset16 = new StatisticsAsset();
    pAsset16.id = "kingdoms";
    pAsset16.localized_key = "statistics_kingdoms";
    pAsset16.list_window_meta_type = MetaType.Kingdom;
    pAsset16.path_icon = "ui/Icons/iconKingdomList";
    pAsset16.long_action = (StatisticsLongAction) (_ => (long) World.world.kingdoms.Count);
    this.add(pAsset16);
    StatisticsAsset pAsset17 = new StatisticsAsset();
    pAsset17.id = "villages";
    pAsset17.localized_key = "statistics_villages";
    pAsset17.list_window_meta_type = MetaType.City;
    pAsset17.path_icon = "ui/Icons/iconCitySelect";
    pAsset17.long_action = (StatisticsLongAction) (_ => (long) World.world.cities.Count);
    this.add(pAsset17);
    StatisticsAsset pAsset18 = new StatisticsAsset();
    pAsset18.id = "world_statistics_population_total";
    pAsset18.localized_key = "world_statistics_population_total";
    pAsset18.steam_activity = "#Status_stat_value";
    pAsset18.rarity = 2;
    pAsset18.list_window_meta_type = MetaType.Unit;
    pAsset18.path_icon = "ui/Icons/iconPopulation";
    pAsset18.long_action = (StatisticsLongAction) (_ => (long) World.world.units.Count);
    this.add(pAsset18);
    StatisticsAsset pAsset19 = new StatisticsAsset();
    pAsset19.id = "world_statistics_beasts";
    pAsset19.list_window_meta_type = MetaType.Unit;
    pAsset19.path_icon = "ui/Icons/worldrules/icon_animalspawn";
    pAsset19.long_action = (StatisticsLongAction) (_ =>
    {
      long num = 0;
      List<Actor> simpleList = World.world.units.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
      {
        if (!simpleList[index].isSapient())
          ++num;
      }
      return num;
    });
    pAsset19.is_world_statistics = true;
    pAsset19.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset19);
    StatisticsAsset pAsset20 = new StatisticsAsset();
    pAsset20.id = "world_statistics_trees";
    pAsset20.path_icon = "ui/Icons/iconFertilizerTrees";
    pAsset20.long_action = (StatisticsLongAction) (_ =>
    {
      long num = 0;
      List<Building> simpleList = World.world.buildings.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
      {
        if (simpleList[index].asset.building_type == BuildingType.Building_Tree)
          ++num;
      }
      return num;
    });
    pAsset20.is_world_statistics = true;
    pAsset20.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset20);
    StatisticsAsset pAsset21 = new StatisticsAsset();
    pAsset21.id = "world_statistics_vegetation";
    pAsset21.path_icon = "ui/Icons/iconFertilizerPlants";
    pAsset21.long_action = (StatisticsLongAction) (_ =>
    {
      long num = 0;
      List<Building> simpleList = World.world.buildings.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
      {
        Building building = simpleList[index];
        if (building.asset.building_type == BuildingType.Building_Tree || building.asset.building_type == BuildingType.Building_Plant)
          ++num;
      }
      return num;
    });
    this.add(pAsset21);
    StatisticsAsset pAsset22 = new StatisticsAsset();
    pAsset22.id = "world_statistics_islands";
    pAsset22.path_icon = "ui/Icons/iconZones";
    pAsset22.long_action = (StatisticsLongAction) (_ => (long) World.world.islands_calculator.countLandIslands());
    pAsset22.is_world_statistics = true;
    pAsset22.world_stats_tabs = WorldStatsTabs.Everything;
    this.add(pAsset22);
    StatisticsAsset pAsset23 = new StatisticsAsset();
    pAsset23.id = "world_statistics_creatures_born";
    pAsset23.list_window_meta_type = MetaType.Unit;
    pAsset23.path_icon = "ui/Icons/iconBirths";
    pAsset23.long_action = (StatisticsLongAction) (_ => World.world.map_stats.creaturesBorn);
    pAsset23.is_world_statistics = true;
    pAsset23.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset23);
    StatisticsAsset pAsset24 = new StatisticsAsset();
    pAsset24.id = "world_statistics_creatures_created";
    pAsset24.list_window_meta_type = MetaType.Unit;
    pAsset24.path_icon = "ui/Icons/actor_traits/iconMiracleBorn";
    pAsset24.long_action = (StatisticsLongAction) (_ => World.world.map_stats.creaturesCreated);
    pAsset24.is_world_statistics = true;
    pAsset24.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset24);
    StatisticsAsset pAsset25 = new StatisticsAsset();
    pAsset25.id = "world_statistics_subspecies_created";
    pAsset25.localized_key_description = "statistics_subspecies".Description();
    pAsset25.list_window_meta_type = MetaType.Subspecies;
    pAsset25.path_icon = "ui/Icons/iconSpecies";
    pAsset25.long_action = (StatisticsLongAction) (_ => World.world.map_stats.subspeciesCreated);
    pAsset25.is_world_statistics = true;
    pAsset25.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset25);
    StatisticsAsset pAsset26 = new StatisticsAsset();
    pAsset26.id = "statistics_total_playtime";
    pAsset26.is_game_statistics = true;
    pAsset26.path_icon = "ui/Icons/iconClock";
    pAsset26.string_action = (StatisticsStringAction) (_ => Toolbox.formatTime((float) World.world.game_stats.data.gameTime));
    this.add(pAsset26);
    StatisticsAsset pAsset27 = new StatisticsAsset();
    pAsset27.id = "statistics_trees_grown";
    pAsset27.path_icon = "ui/Icons/worldrules/icon_grow_trees";
    pAsset27.is_game_statistics = true;
    pAsset27.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.treesGrown);
    this.add(pAsset27);
    StatisticsAsset pAsset28 = new StatisticsAsset();
    pAsset28.id = "statistics_flora_grown";
    pAsset28.path_icon = "ui/Icons/worldrules/icon_flora_density_high";
    pAsset28.is_game_statistics = true;
    pAsset28.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.floraGrown);
    this.add(pAsset28);
    StatisticsAsset pAsset29 = new StatisticsAsset();
    pAsset29.id = "statistics_meteorites_launched";
    pAsset29.is_game_statistics = true;
    pAsset29.path_icon = "ui/Icons/iconMeteorite";
    pAsset29.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.meteoritesLaunched);
    this.add(pAsset29);
    StatisticsAsset pAsset30 = new StatisticsAsset();
    pAsset30.id = "statistics_pixels_exploded";
    pAsset30.is_game_statistics = true;
    pAsset30.path_icon = "ui/Icons/worldrules/icon_exploding_mushrooms";
    pAsset30.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.pixelsExploded);
    this.add(pAsset30);
    StatisticsAsset pAsset31 = new StatisticsAsset();
    pAsset31.id = "statistics_creatures_created";
    pAsset31.is_game_statistics = true;
    pAsset31.path_icon = "ui/Icons/actor_traits/iconMiracleBorn";
    pAsset31.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.creaturesCreated);
    this.add(pAsset31);
    StatisticsAsset pAsset32 = new StatisticsAsset();
    pAsset32.id = "statistics_creatures_born";
    pAsset32.is_game_statistics = true;
    pAsset32.path_icon = "ui/Icons/iconBirths";
    pAsset32.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.creaturesBorn);
    this.add(pAsset32);
    StatisticsAsset pAsset33 = new StatisticsAsset();
    pAsset33.id = "statistics_creatures_died";
    pAsset33.is_game_statistics = true;
    pAsset33.path_icon = "ui/Icons/iconDead";
    pAsset33.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.creaturesDied);
    this.add(pAsset33);
    StatisticsAsset pAsset34 = new StatisticsAsset();
    pAsset34.id = "statistics_bombs_dropped";
    pAsset34.is_game_statistics = true;
    pAsset34.path_icon = "ui/Icons/iconBomb";
    pAsset34.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.bombsDropped);
    this.add(pAsset34);
    StatisticsAsset pAsset35 = new StatisticsAsset();
    pAsset35.id = "statistics_subspecies_created";
    pAsset35.localized_key_description = "statistics_subspecies".Description();
    pAsset35.is_game_statistics = true;
    pAsset35.path_icon = "ui/Icons/iconSpecies";
    pAsset35.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.subspeciesCreated);
    this.add(pAsset35);
    StatisticsAsset pAsset36 = new StatisticsAsset();
    pAsset36.id = "statistics_subspecies_extinct";
    pAsset36.is_game_statistics = true;
    pAsset36.path_icon = "ui/Icons/iconSpeciesExtinct";
    pAsset36.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.subspeciesExtinct);
    this.add(pAsset36);
    StatisticsAsset pAsset37 = new StatisticsAsset();
    pAsset37.id = "statistics_languages_created";
    pAsset37.localized_key_description = "statistics_languages".Description();
    pAsset37.is_game_statistics = true;
    pAsset37.path_icon = "ui/Icons/iconLanguage";
    pAsset37.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.languagesCreated);
    this.add(pAsset37);
    StatisticsAsset pAsset38 = new StatisticsAsset();
    pAsset38.id = "statistics_languages_forgotten";
    pAsset38.is_game_statistics = true;
    pAsset38.path_icon = "ui/Icons/iconLanguageForgotten";
    pAsset38.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.languagesForgotten);
    this.add(pAsset38);
    StatisticsAsset pAsset39 = new StatisticsAsset();
    pAsset39.id = "statistics_cultures_created";
    pAsset39.localized_key_description = "statistics_cultures".Description();
    pAsset39.is_game_statistics = true;
    pAsset39.path_icon = "ui/Icons/iconCulture";
    pAsset39.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.culturesCreated);
    this.add(pAsset39);
    StatisticsAsset pAsset40 = new StatisticsAsset();
    pAsset40.id = "statistics_cultures_forgotten";
    pAsset40.is_game_statistics = true;
    pAsset40.path_icon = "ui/Icons/iconCultureForgotten";
    pAsset40.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.culturesForgotten);
    this.add(pAsset40);
    StatisticsAsset pAsset41 = new StatisticsAsset();
    pAsset41.id = "statistics_families_created";
    pAsset41.localized_key_description = "statistics_families".Description();
    pAsset41.is_game_statistics = true;
    pAsset41.path_icon = "ui/Icons/iconNewFamily";
    pAsset41.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.familiesCreated);
    this.add(pAsset41);
    StatisticsAsset pAsset42 = new StatisticsAsset();
    pAsset42.id = "statistics_families_destroyed";
    pAsset42.is_game_statistics = true;
    pAsset42.path_icon = "ui/Icons/iconFamilyDestroyed";
    pAsset42.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.familiesDestroyed);
    this.add(pAsset42);
    StatisticsAsset pAsset43 = new StatisticsAsset();
    pAsset43.id = "statistics_clans_created";
    pAsset43.localized_key_description = "statistics_clans".Description();
    pAsset43.is_game_statistics = true;
    pAsset43.path_icon = "ui/Icons/iconClan";
    pAsset43.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.clansCreated);
    this.add(pAsset43);
    StatisticsAsset pAsset44 = new StatisticsAsset();
    pAsset44.id = "statistics_clans_destroyed";
    pAsset44.is_game_statistics = true;
    pAsset44.path_icon = "ui/Icons/iconClanDestroyed";
    pAsset44.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.clansDestroyed);
    this.add(pAsset44);
    StatisticsAsset pAsset45 = new StatisticsAsset();
    pAsset45.id = "statistics_books_written";
    pAsset45.localized_key_description = "books".Description();
    pAsset45.is_game_statistics = true;
    pAsset45.path_icon = "ui/Icons/iconBooksWritten";
    pAsset45.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.booksWritten);
    this.add(pAsset45);
    StatisticsAsset pAsset46 = new StatisticsAsset();
    pAsset46.id = "statistics_books_read";
    pAsset46.is_game_statistics = true;
    pAsset46.path_icon = "ui/Icons/iconBooksRead";
    pAsset46.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.booksRead);
    this.add(pAsset46);
    StatisticsAsset pAsset47 = new StatisticsAsset();
    pAsset47.id = "statistics_books_burnt";
    pAsset47.is_game_statistics = true;
    pAsset47.path_icon = "ui/Icons/iconBooksDestroyed";
    pAsset47.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.booksBurnt);
    this.add(pAsset47);
    StatisticsAsset pAsset48 = new StatisticsAsset();
    pAsset48.id = "statistics_religions_created";
    pAsset48.localized_key_description = "statistics_religions".Description();
    pAsset48.is_game_statistics = true;
    pAsset48.path_icon = "ui/Icons/iconReligion";
    pAsset48.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.religionsCreated);
    this.add(pAsset48);
    StatisticsAsset pAsset49 = new StatisticsAsset();
    pAsset49.id = "statistics_religions_forgotten";
    pAsset49.is_game_statistics = true;
    pAsset49.path_icon = "ui/Icons/iconReligionForgotten";
    pAsset49.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.religionsForgotten);
    this.add(pAsset49);
    StatisticsAsset pAsset50 = new StatisticsAsset();
    pAsset50.id = "statistics_kingdoms_created";
    pAsset50.localized_key_description = "statistics_kingdoms".Description();
    pAsset50.is_game_statistics = true;
    pAsset50.path_icon = "ui/Icons/iconKingdom";
    pAsset50.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.kingdomsCreated);
    this.add(pAsset50);
    StatisticsAsset pAsset51 = new StatisticsAsset();
    pAsset51.id = "statistics_kingdoms_destroyed";
    pAsset51.is_game_statistics = true;
    pAsset51.path_icon = "ui/Icons/iconKingdomDestroyed";
    pAsset51.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.kingdomsDestroyed);
    this.add(pAsset51);
    StatisticsAsset pAsset52 = new StatisticsAsset();
    pAsset52.id = "statistics_cities_created";
    pAsset52.localized_key_description = "statistics_villages".Description();
    pAsset52.is_game_statistics = true;
    pAsset52.path_icon = "ui/Icons/iconCity";
    pAsset52.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.citiesCreated);
    this.add(pAsset52);
    StatisticsAsset pAsset53 = new StatisticsAsset();
    pAsset53.id = "statistics_cities_destroyed";
    pAsset53.is_game_statistics = true;
    pAsset53.path_icon = "ui/Icons/iconCityDestroyed";
    pAsset53.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.citiesDestroyed);
    this.add(pAsset53);
    StatisticsAsset pAsset54 = new StatisticsAsset();
    pAsset54.id = "statistics_wars_started";
    pAsset54.localized_key_description = "statistics_wars".Description();
    pAsset54.is_game_statistics = true;
    pAsset54.path_icon = "ui/Icons/iconWhisperOfWar";
    pAsset54.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.warsStarted);
    this.add(pAsset54);
    StatisticsAsset pAsset55 = new StatisticsAsset();
    pAsset55.id = "statistics_peaces_made";
    pAsset55.is_game_statistics = true;
    pAsset55.path_icon = "ui/Icons/actor_traits/iconPacifist";
    pAsset55.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.peacesMade);
    this.add(pAsset55);
    StatisticsAsset pAsset56 = new StatisticsAsset();
    pAsset56.id = "statistics_plots_started";
    pAsset56.localized_key_description = "statistics_plots".Description();
    pAsset56.is_game_statistics = true;
    pAsset56.path_icon = "ui/Icons/iconPlot";
    pAsset56.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.plotsStarted);
    this.add(pAsset56);
    StatisticsAsset pAsset57 = new StatisticsAsset();
    pAsset57.id = "statistics_plots_succeeded";
    pAsset57.is_game_statistics = true;
    pAsset57.path_icon = "ui/Icons/iconPlotSucceeded";
    pAsset57.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.plotsSucceeded);
    this.add(pAsset57);
    StatisticsAsset pAsset58 = new StatisticsAsset();
    pAsset58.id = "statistics_plots_forgotten";
    pAsset58.is_game_statistics = true;
    pAsset58.path_icon = "ui/Icons/iconPlotForgotten";
    pAsset58.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.plotsForgotten);
    this.add(pAsset58);
    StatisticsAsset pAsset59 = new StatisticsAsset();
    pAsset59.id = "statistics_creatures_sacrificed";
    pAsset59.is_game_statistics = true;
    pAsset59.path_icon = "ui/Icons/iconVolcano";
    pAsset59.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.creaturesSacrificed);
    this.add(pAsset59);
    StatisticsAsset pAsset60 = new StatisticsAsset();
    pAsset60.id = "statistics_elves_sacrificed";
    pAsset60.is_game_statistics = true;
    pAsset60.path_icon = "ui/Icons/iconHateElf";
    pAsset60.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.elvesSacrificed);
    this.add(pAsset60);
    StatisticsAsset pAsset61 = new StatisticsAsset();
    pAsset61.id = "statistics_boats_destroyed_by_magnet";
    pAsset61.is_game_statistics = true;
    pAsset61.path_icon = "ui/Icons/iconBoat";
    pAsset61.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.boatsDestroyedByMagnet);
    this.add(pAsset61);
  }

  private void addStatsGeneralMain()
  {
    StatisticsAsset pAsset1 = new StatisticsAsset();
    pAsset1.id = "world_statistics_time";
    pAsset1.localized_key = "world_statistics_time";
    pAsset1.steam_activity = "#Status_stat_value";
    pAsset1.rarity = 2;
    pAsset1.path_icon = "ui/Icons/iconClock";
    pAsset1.string_action = (StatisticsStringAction) (_ => Date.getUIStringYearMonthShort());
    pAsset1.is_world_statistics = true;
    pAsset1.world_stats_tabs = WorldStatsTabs.Everything;
    this.add(pAsset1);
    StatisticsAsset pAsset2 = new StatisticsAsset();
    pAsset2.id = "world_statistics_population";
    pAsset2.localized_key = "world_statistics_population";
    pAsset2.steam_activity = "#Status_stat_value";
    pAsset2.rarity = 2;
    pAsset2.list_window_meta_type = MetaType.Unit;
    pAsset2.path_icon = "ui/Icons/iconPopulationCiv";
    pAsset2.long_action = (StatisticsLongAction) (_ => (long) World.world.getCivWorldPopulation());
    pAsset2.is_world_statistics = true;
    pAsset2.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset2);
  }

  public override void post_init()
  {
    base.post_init();
    foreach (StatisticsAsset statisticsAsset in this.list)
    {
      if (statisticsAsset.locale_getter != null)
        statisticsAsset.localized_key = statisticsAsset.locale_getter();
    }
  }

  public override void editorDiagnosticLocales()
  {
    foreach (StatisticsAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
    foreach (StatisticsAsset statisticsAsset1 in this.list)
    {
      if (statisticsAsset1.is_world_statistics)
      {
        string pID = statisticsAsset1.id.Replace("world_", "");
        if (this.has(pID))
        {
          StatisticsAsset statisticsAsset2 = this.get(pID);
          if (statisticsAsset2.path_icon != statisticsAsset1.path_icon)
            BaseAssetLibrary.logAssetError($"<e>StatisticsLibrary</e>: World Stat <b>{statisticsAsset1.path_icon}</b> has different icon than Game Stat <b>{statisticsAsset2.path_icon}</b>", statisticsAsset1.id);
        }
      }
    }
    foreach (StatisticsAsset statisticsAsset3 in this.list)
    {
      if (statisticsAsset3.is_game_statistics)
      {
        string pID = "world_" + statisticsAsset3.id;
        if (this.has(pID))
        {
          StatisticsAsset statisticsAsset4 = this.get(pID);
          if (statisticsAsset4.path_icon != statisticsAsset3.path_icon)
            BaseAssetLibrary.logAssetError($"<e>StatisticsLibrary</e>: Game Stat <b>{statisticsAsset3.path_icon}</b> has different icon than World Stat <b>{statisticsAsset4.path_icon}</b>", statisticsAsset3.id);
        }
      }
    }
    base.editorDiagnosticLocales();
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (StatisticsAsset statisticsAsset in this.list)
    {
      for (int index = 0; index < statisticsAsset.rarity; ++index)
        StatisticsLibrary.power_tracker_pool.Add(statisticsAsset);
    }
  }

  private string getDominatingMetaRow(MetaType pType)
  {
    MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(pType);
    long dominatingMetaId = this.getDominatingMetaId(asset);
    if (!(asset.get(dominatingMetaId) is IMetaObject metaObject) || metaObject.countUnits() == 0)
      return StatisticsLibrary._unknown_text;
    string colorText = metaObject.getColor().color_text;
    return Toolbox.coloredText(metaObject.name + Toolbox.coloredGreyPart((object) metaObject.countUnits(), colorText), colorText);
  }

  private long getDominatingMetaId(MetaType pType)
  {
    return this.getDominatingMetaId(AssetManager.meta_type_library.getAsset(pType));
  }

  private long getDominatingMetaId(MetaTypeAsset pMetaAsset)
  {
    IMetaObject metaObject1 = (IMetaObject) null;
    foreach (IMetaObject metaObject2 in pMetaAsset.get_list())
    {
      if (metaObject1 == null || metaObject2.countUnits() > metaObject1.countUnits())
        metaObject1 = metaObject2;
    }
    return metaObject1 == null ? -1L : metaObject1.getID();
  }

  private string getOldestMetaRow(MetaType pType)
  {
    MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(pType);
    long oldestMetaId = this.getOldestMetaId(asset);
    if (!(asset.get(oldestMetaId) is IMetaObject metaObject))
      return StatisticsLibrary._unknown_text;
    string colorText = metaObject.getColor().color_text;
    return Toolbox.coloredText(metaObject.name + Toolbox.coloredGreyPart((object) metaObject.getAge(), colorText), colorText);
  }

  private long getOldestMetaId(MetaType pType)
  {
    return this.getOldestMetaId(AssetManager.meta_type_library.getAsset(pType));
  }

  private long getOldestMetaId(MetaTypeAsset pMetaAsset)
  {
    IMetaObject metaObject1 = (IMetaObject) null;
    foreach (IMetaObject metaObject2 in pMetaAsset.get_list())
    {
      if (metaObject1 == null || metaObject2.getAge() > metaObject1.getAge())
        metaObject1 = metaObject2;
    }
    return metaObject1 == null ? -1L : metaObject1.getID();
  }

  public string addToGameplayReport(string pWhatFor)
  {
    string str1 = $"{string.Empty}{pWhatFor}\n";
    foreach (StatisticsAsset statisticsAsset in this.list)
    {
      string str2 = statisticsAsset.getLocaleID().Localize();
      string str3 = statisticsAsset.getDescriptionID().Localize();
      string str4 = "\n" + str2 + "\n";
      if (!string.IsNullOrEmpty(str3))
        str4 = $"{str4}1: {str3}";
      str1 += str4;
    }
    return str1 + "\n\n";
  }

  public void addStatsBiomes()
  {
    this.addNormalBiomes();
    this.addCreepBiomes();
    this.addSpecialBiomes();
  }

  public void addNormalBiomes()
  {
    StatisticsAsset pAsset1 = new StatisticsAsset();
    pAsset1.id = "world_statistics_grass";
    pAsset1.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_grass"));
    pAsset1.path_icon = "ui/Icons/iconSeedGrass";
    pAsset1.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.grass_high.hashset.Count + TopTileLibrary.grass_low.hashset.Count));
    pAsset1.is_world_statistics = true;
    pAsset1.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset1);
    StatisticsAsset pAsset2 = new StatisticsAsset();
    pAsset2.id = "world_statistics_savanna";
    pAsset2.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_savanna"));
    pAsset2.path_icon = "ui/Icons/iconSeedSavanna";
    pAsset2.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.savanna_high.hashset.Count + TopTileLibrary.savanna_low.hashset.Count));
    pAsset2.is_world_statistics = true;
    pAsset2.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset2);
    StatisticsAsset pAsset3 = new StatisticsAsset();
    pAsset3.id = "world_statistics_jungle";
    pAsset3.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_jungle"));
    pAsset3.path_icon = "ui/Icons/iconSeedJungle";
    pAsset3.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.jungle_high.hashset.Count + TopTileLibrary.jungle_low.hashset.Count));
    pAsset3.is_world_statistics = true;
    pAsset3.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset3);
    StatisticsAsset pAsset4 = new StatisticsAsset();
    pAsset4.id = "world_statistics_desert";
    pAsset4.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_desert"));
    pAsset4.path_icon = "ui/Icons/iconSeedDesert";
    pAsset4.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.desert_high.hashset.Count + TopTileLibrary.desert_low.hashset.Count));
    pAsset4.is_world_statistics = true;
    pAsset4.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset4);
    StatisticsAsset pAsset5 = new StatisticsAsset();
    pAsset5.id = "world_statistics_lemon";
    pAsset5.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_lemon"));
    pAsset5.path_icon = "ui/Icons/iconSeedLemon";
    pAsset5.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.lemon_high.hashset.Count + TopTileLibrary.lemon_low.hashset.Count));
    pAsset5.is_world_statistics = true;
    pAsset5.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset5);
    StatisticsAsset pAsset6 = new StatisticsAsset();
    pAsset6.id = "world_statistics_permafrost";
    pAsset6.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_permafrost"));
    pAsset6.path_icon = "ui/Icons/iconSeedPermafrost";
    pAsset6.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.permafrost_high.hashset.Count + TopTileLibrary.permafrost_low.hashset.Count));
    pAsset6.is_world_statistics = true;
    pAsset6.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset6);
    StatisticsAsset pAsset7 = new StatisticsAsset();
    pAsset7.id = "world_statistics_swamp";
    pAsset7.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_swamp"));
    pAsset7.path_icon = "ui/Icons/iconSeedSwamp";
    pAsset7.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.swamp_high.hashset.Count + TopTileLibrary.swamp_low.hashset.Count));
    pAsset7.is_world_statistics = true;
    pAsset7.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset7);
    StatisticsAsset pAsset8 = new StatisticsAsset();
    pAsset8.id = "world_statistics_crystal";
    pAsset8.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_crystal"));
    pAsset8.path_icon = "ui/Icons/iconSeedCrystal";
    pAsset8.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.crystal_high.hashset.Count + TopTileLibrary.crystal_low.hashset.Count));
    pAsset8.is_world_statistics = true;
    pAsset8.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset8);
    StatisticsAsset pAsset9 = new StatisticsAsset();
    pAsset9.id = "world_statistics_enchanted";
    pAsset9.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_enchanted"));
    pAsset9.path_icon = "ui/Icons/iconSeedEnchanted";
    pAsset9.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.enchanted_high.hashset.Count + TopTileLibrary.enchanted_low.hashset.Count));
    pAsset9.is_world_statistics = true;
    pAsset9.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset9);
    StatisticsAsset pAsset10 = new StatisticsAsset();
    pAsset10.id = "world_statistics_corruption";
    pAsset10.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_corrupted"));
    pAsset10.path_icon = "ui/Icons/iconSeedCorrupted";
    pAsset10.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.corruption_high.hashset.Count + TopTileLibrary.corruption_low.hashset.Count));
    pAsset10.is_world_statistics = true;
    pAsset10.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset10);
    StatisticsAsset pAsset11 = new StatisticsAsset();
    pAsset11.id = "world_statistics_infernal";
    pAsset11.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_infernal"));
    pAsset11.path_icon = "ui/Icons/iconSeedInfernal";
    pAsset11.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.infernal_high.hashset.Count + TopTileLibrary.infernal_low.hashset.Count));
    pAsset11.is_world_statistics = true;
    pAsset11.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset11);
    StatisticsAsset pAsset12 = new StatisticsAsset();
    pAsset12.id = "world_statistics_candy";
    pAsset12.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_candy"));
    pAsset12.path_icon = "ui/Icons/iconSeedCandy";
    pAsset12.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.candy_high.hashset.Count + TopTileLibrary.candy_low.hashset.Count));
    pAsset12.is_world_statistics = true;
    pAsset12.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset12);
    StatisticsAsset pAsset13 = new StatisticsAsset();
    pAsset13.id = "world_statistics_mushroom";
    pAsset13.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_mushroom"));
    pAsset13.path_icon = "ui/Icons/iconSeedMushroom";
    pAsset13.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.mushroom_high.hashset.Count + TopTileLibrary.mushroom_low.hashset.Count));
    pAsset13.is_world_statistics = true;
    pAsset13.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset13);
    StatisticsAsset pAsset14 = new StatisticsAsset();
    pAsset14.id = "world_statistics_wasteland";
    pAsset14.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_wasteland"));
    pAsset14.path_icon = "ui/Icons/achievements/achievements_wastelandbiome";
    pAsset14.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.wasteland_high.hashset.Count + TopTileLibrary.wasteland_low.hashset.Count));
    pAsset14.is_world_statistics = true;
    pAsset14.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset14);
    StatisticsAsset pAsset15 = new StatisticsAsset();
    pAsset15.id = "world_statistics_birch";
    pAsset15.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_birch"));
    pAsset15.path_icon = "ui/Icons/iconSeedBirch";
    pAsset15.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.birch_high.hashset.Count + TopTileLibrary.birch_low.hashset.Count));
    pAsset15.is_world_statistics = true;
    pAsset15.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset15);
    StatisticsAsset pAsset16 = new StatisticsAsset();
    pAsset16.id = "world_statistics_maple";
    pAsset16.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_maple"));
    pAsset16.path_icon = "ui/Icons/iconSeedMaple";
    pAsset16.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.maple_high.hashset.Count + TopTileLibrary.maple_low.hashset.Count));
    pAsset16.is_world_statistics = true;
    pAsset16.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset16);
    StatisticsAsset pAsset17 = new StatisticsAsset();
    pAsset17.id = "world_statistics_rocklands";
    pAsset17.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_rocklands"));
    pAsset17.path_icon = "ui/Icons/iconSeedRocklands";
    pAsset17.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.rocklands_high.hashset.Count + TopTileLibrary.rocklands_low.hashset.Count));
    pAsset17.is_world_statistics = true;
    pAsset17.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset17);
    StatisticsAsset pAsset18 = new StatisticsAsset();
    pAsset18.id = "world_statistics_garlic";
    pAsset18.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_garlic"));
    pAsset18.path_icon = "ui/Icons/iconSeedGarlic";
    pAsset18.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.garlic_high.hashset.Count + TopTileLibrary.garlic_low.hashset.Count));
    pAsset18.is_world_statistics = true;
    pAsset18.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset18);
    StatisticsAsset pAsset19 = new StatisticsAsset();
    pAsset19.id = "world_statistics_flower";
    pAsset19.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_flower"));
    pAsset19.path_icon = "ui/Icons/iconSeedFlower";
    pAsset19.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.flower_high.hashset.Count + TopTileLibrary.flower_low.hashset.Count));
    pAsset19.is_world_statistics = true;
    pAsset19.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset19);
    StatisticsAsset pAsset20 = new StatisticsAsset();
    pAsset20.id = "world_statistics_celestial";
    pAsset20.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_celestial"));
    pAsset20.path_icon = "ui/Icons/iconSeedCelestial";
    pAsset20.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.celestial_high.hashset.Count + TopTileLibrary.celestial_low.hashset.Count));
    pAsset20.is_world_statistics = true;
    pAsset20.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset20);
    StatisticsAsset pAsset21 = new StatisticsAsset();
    pAsset21.id = "world_statistics_clover";
    pAsset21.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_clover"));
    pAsset21.path_icon = "ui/Icons/iconSeedClover";
    pAsset21.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.clover_high.hashset.Count + TopTileLibrary.clover_low.hashset.Count));
    pAsset21.is_world_statistics = true;
    pAsset21.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset21);
    StatisticsAsset pAsset22 = new StatisticsAsset();
    pAsset22.id = "world_statistics_singularity";
    pAsset22.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_singularity"));
    pAsset22.path_icon = "ui/Icons/iconSeedSingularity";
    pAsset22.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.singularity_high.hashset.Count + TopTileLibrary.singularity_low.hashset.Count));
    pAsset22.is_world_statistics = true;
    pAsset22.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset22);
    StatisticsAsset pAsset23 = new StatisticsAsset();
    pAsset23.id = "world_statistics_paradox";
    pAsset23.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_paradox"));
    pAsset23.path_icon = "ui/Icons/iconSeedParadox";
    pAsset23.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.paradox_high.hashset.Count + TopTileLibrary.paradox_low.hashset.Count));
    pAsset23.is_world_statistics = true;
    pAsset23.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset23);
  }

  public void addSpecialBiomes()
  {
    StatisticsAsset pAsset = new StatisticsAsset();
    pAsset.id = "world_statistics_sand";
    pAsset.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("tile_sand"));
    pAsset.path_icon = "ui/Icons/iconTileSand";
    pAsset.long_action = (StatisticsLongAction) (_ => (long) TileLibrary.sand.hashset.Count);
    pAsset.is_world_statistics = true;
    pAsset.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset);
  }

  public void addCreepBiomes()
  {
    StatisticsAsset pAsset1 = new StatisticsAsset();
    pAsset1.id = "world_statistics_biomass";
    pAsset1.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_biomass"));
    pAsset1.path_icon = "ui/Icons/iconBiomass";
    pAsset1.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.biomass_high.hashset.Count + TopTileLibrary.biomass_low.hashset.Count));
    pAsset1.is_world_statistics = true;
    pAsset1.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset1);
    StatisticsAsset pAsset2 = new StatisticsAsset();
    pAsset2.id = "world_statistics_cybertile";
    pAsset2.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_cybertile"));
    pAsset2.path_icon = "ui/Icons/iconCybercore";
    pAsset2.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.cybertile_high.hashset.Count + TopTileLibrary.cybertile_low.hashset.Count));
    pAsset2.is_world_statistics = true;
    pAsset2.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset2);
    StatisticsAsset pAsset3 = new StatisticsAsset();
    pAsset3.id = "world_statistics_pumpkin";
    pAsset3.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_pumpkin"));
    pAsset3.path_icon = "ui/Icons/iconSuperPumpkin";
    pAsset3.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.pumpkin_high.hashset.Count + TopTileLibrary.pumpkin_low.hashset.Count));
    pAsset3.is_world_statistics = true;
    pAsset3.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset3);
    StatisticsAsset pAsset4 = new StatisticsAsset();
    pAsset4.id = "world_statistics_tumor";
    pAsset4.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getBiomeLocale("biome_tumor"));
    pAsset4.path_icon = "ui/Icons/iconTumor";
    pAsset4.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.tumor_high.hashset.Count + TopTileLibrary.tumor_low.hashset.Count));
    pAsset4.is_world_statistics = true;
    pAsset4.world_stats_tabs = WorldStatsTabs.Biomes;
    this.add(pAsset4);
  }

  public static string getBiomeLocale(string pBiomeID)
  {
    return AssetManager.biome_library.get(pBiomeID).getLocaleID();
  }

  public static string getPowerLocale(string pPowerID)
  {
    return AssetManager.powers.get(pPowerID).getLocaleID();
  }

  public void addStatsDeaths()
  {
    StatisticsAsset pAsset1 = new StatisticsAsset();
    pAsset1.id = "world_statistics_deaths_total";
    pAsset1.localized_key = "world_statistics_deaths_total";
    pAsset1.steam_activity = "#Status_stat_value";
    pAsset1.rarity = 3;
    pAsset1.list_window_meta_type = MetaType.Unit;
    pAsset1.path_icon = "ui/Icons/iconDead";
    pAsset1.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths);
    pAsset1.is_world_statistics = true;
    pAsset1.world_stats_tabs = WorldStatsTabs.Deaths;
    this.add(pAsset1);
    StatisticsAsset pAsset2 = new StatisticsAsset();
    pAsset2.id = "world_statistics_deaths_natural";
    pAsset2.list_window_meta_type = MetaType.Unit;
    pAsset2.path_icon = "ui/Icons/iconClock";
    pAsset2.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_age);
    pAsset2.is_world_statistics = true;
    pAsset2.world_stats_tabs = WorldStatsTabs.Deaths;
    this.add(pAsset2);
    this.clone("world_statistics_deaths_hunger", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_hunger);
    this.t.path_icon = "ui/Icons/iconDeathsHunger";
    this.clone("world_statistics_deaths_eaten", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_eaten);
    this.t.path_icon = "ui/Icons/iconDeathsEaten";
    this.clone("world_statistics_deaths_plague", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_plague);
    this.t.path_icon = "ui/Icons/actor_traits/iconPlague";
    this.clone("world_statistics_deaths_poison", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_poison);
    this.t.path_icon = "ui/Icons/iconPoisoned";
    this.clone("world_statistics_deaths_infection", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_infection);
    this.t.path_icon = "ui/Icons/actor_traits/iconInfected";
    this.clone("world_statistics_deaths_tumor", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_tumor);
    this.t.path_icon = "ui/Icons/iconTumor";
    this.clone("world_statistics_deaths_acid", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_acid);
    this.t.path_icon = "ui/Icons/iconAcid";
    this.clone("world_statistics_deaths_fire", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_fire);
    this.t.path_icon = "ui/Icons/iconFire";
    this.clone("world_statistics_deaths_divine", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_divine);
    this.t.path_icon = "ui/Icons/iconDivineLight";
    this.clone("world_statistics_deaths_weapon", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_weapon);
    this.t.path_icon = "ui/Icons/actor_traits/iconBloodlust";
    this.clone("world_statistics_deaths_gravity", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_gravity);
    this.t.path_icon = "ui/Icons/worldrules/icon_grow_trees";
    this.clone("world_statistics_deaths_drowning", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_drowning);
    this.t.path_icon = "ui/Icons/iconTileDeepOcean";
    this.clone("world_statistics_deaths_water", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_water);
    this.t.path_icon = "ui/Icons/iconRain";
    this.clone("world_statistics_deaths_explosion", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_explosion);
    this.t.path_icon = "ui/Icons/worldrules/icon_exploding_mushrooms";
    this.clone("world_statistics_deaths_other", "world_statistics_deaths_natural");
    this.t.long_action = (StatisticsLongAction) (_ => World.world.map_stats.deaths_other);
    this.t.path_icon = "ui/Icons/iconDead";
    StatisticsAsset pAsset3 = new StatisticsAsset();
    pAsset3.id = "world_statistics_metamorphosis";
    pAsset3.list_window_meta_type = MetaType.Unit;
    pAsset3.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_metamorph";
    pAsset3.long_action = (StatisticsLongAction) (_ => World.world.map_stats.metamorphosis);
    pAsset3.is_world_statistics = true;
    pAsset3.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset3);
    StatisticsAsset pAsset4 = new StatisticsAsset();
    pAsset4.id = "world_statistics_evolutions";
    pAsset4.list_window_meta_type = MetaType.Unit;
    pAsset4.path_icon = "ui/Icons/iconMonolith";
    pAsset4.long_action = (StatisticsLongAction) (_ => World.world.map_stats.evolutions);
    pAsset4.is_world_statistics = true;
    pAsset4.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset4);
    StatisticsAsset pAsset5 = new StatisticsAsset();
    pAsset5.id = "world_statistics_houses_destroyed";
    pAsset5.list_window_meta_type = MetaType.City;
    pAsset5.path_icon = "ui/Icons/actor_traits/iconPyromaniac";
    pAsset5.long_action = (StatisticsLongAction) (_ => World.world.map_stats.housesDestroyed);
    pAsset5.is_world_statistics = true;
    pAsset5.world_stats_tabs = WorldStatsTabs.General | WorldStatsTabs.Deaths;
    this.add(pAsset5);
    StatisticsAsset pAsset6 = new StatisticsAsset();
    pAsset6.id = "world_statistics_subspecies_extinct";
    pAsset6.list_window_meta_type = MetaType.Subspecies;
    pAsset6.path_icon = "ui/Icons/iconSpeciesExtinct";
    pAsset6.long_action = (StatisticsLongAction) (_ => World.world.map_stats.subspeciesExtinct);
    pAsset6.is_world_statistics = true;
    pAsset6.world_stats_tabs = WorldStatsTabs.Noosphere | WorldStatsTabs.Deaths;
    this.add(pAsset6);
    StatisticsAsset pAsset7 = new StatisticsAsset();
    pAsset7.id = "world_statistics_kingdoms_destroyed";
    pAsset7.list_window_meta_type = MetaType.Kingdom;
    pAsset7.path_icon = "ui/Icons/iconKingdomDestroyed";
    pAsset7.long_action = (StatisticsLongAction) (_ => World.world.map_stats.kingdomsDestroyed);
    pAsset7.is_world_statistics = true;
    pAsset7.world_stats_tabs = WorldStatsTabs.Noosphere | WorldStatsTabs.Deaths;
    this.add(pAsset7);
    StatisticsAsset pAsset8 = new StatisticsAsset();
    pAsset8.id = "world_statistics_armies_destroyed";
    pAsset8.list_window_meta_type = MetaType.Army;
    pAsset8.path_icon = "ui/Icons/iconArmiesDestroyed";
    pAsset8.long_action = (StatisticsLongAction) (_ => World.world.map_stats.armiesDestroyed);
    pAsset8.is_world_statistics = true;
    pAsset8.world_stats_tabs = WorldStatsTabs.Noosphere | WorldStatsTabs.Deaths;
    this.add(pAsset8);
    StatisticsAsset pAsset9 = new StatisticsAsset();
    pAsset9.id = "world_statistics_cities_destroyed";
    pAsset9.list_window_meta_type = MetaType.City;
    pAsset9.path_icon = "ui/Icons/iconCityDestroyed";
    pAsset9.long_action = (StatisticsLongAction) (_ => World.world.map_stats.citiesDestroyed);
    pAsset9.is_world_statistics = true;
    pAsset9.world_stats_tabs = WorldStatsTabs.Noosphere | WorldStatsTabs.Deaths;
    this.add(pAsset9);
  }

  public void addStatsNoos()
  {
    StatisticsAsset pAsset1 = new StatisticsAsset();
    pAsset1.id = "world_statistics_languages_created";
    pAsset1.localized_key_description = "statistics_languages".Description();
    pAsset1.list_window_meta_type = MetaType.Language;
    pAsset1.path_icon = "ui/Icons/iconLanguage";
    pAsset1.long_action = (StatisticsLongAction) (_ => World.world.map_stats.languagesCreated);
    pAsset1.is_world_statistics = true;
    pAsset1.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset1);
    StatisticsAsset pAsset2 = new StatisticsAsset();
    pAsset2.id = "world_statistics_languages_forgotten";
    pAsset2.list_window_meta_type = MetaType.Language;
    pAsset2.path_icon = "ui/Icons/iconLanguageForgotten";
    pAsset2.long_action = (StatisticsLongAction) (_ => World.world.map_stats.languagesForgotten);
    pAsset2.is_world_statistics = true;
    pAsset2.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset2);
    StatisticsAsset pAsset3 = new StatisticsAsset();
    pAsset3.id = "world_statistics_families_created";
    pAsset3.list_window_meta_type = MetaType.Family;
    pAsset3.path_icon = "ui/Icons/iconNewFamily";
    pAsset3.long_action = (StatisticsLongAction) (_ => World.world.map_stats.familiesCreated);
    pAsset3.is_world_statistics = true;
    pAsset3.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset3);
    StatisticsAsset pAsset4 = new StatisticsAsset();
    pAsset4.id = "world_statistics_families_destroyed";
    pAsset4.list_window_meta_type = MetaType.Family;
    pAsset4.path_icon = "ui/Icons/iconFamilyDestroyed";
    pAsset4.long_action = (StatisticsLongAction) (_ => World.world.map_stats.familiesDestroyed);
    pAsset4.is_world_statistics = true;
    pAsset4.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset4);
    StatisticsAsset pAsset5 = new StatisticsAsset();
    pAsset5.id = "world_statistics_clans_created";
    pAsset5.localized_key_description = "statistics_clans".Description();
    pAsset5.list_window_meta_type = MetaType.Clan;
    pAsset5.path_icon = "ui/Icons/iconClan";
    pAsset5.long_action = (StatisticsLongAction) (_ => World.world.map_stats.clansCreated);
    pAsset5.is_world_statistics = true;
    pAsset5.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset5);
    StatisticsAsset pAsset6 = new StatisticsAsset();
    pAsset6.id = "world_statistics_clans_destroyed";
    pAsset6.list_window_meta_type = MetaType.Clan;
    pAsset6.path_icon = "ui/Icons/iconClanDestroyed";
    pAsset6.long_action = (StatisticsLongAction) (_ => World.world.map_stats.clansDestroyed);
    pAsset6.is_world_statistics = true;
    pAsset6.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset6);
    StatisticsAsset pAsset7 = new StatisticsAsset();
    pAsset7.id = "world_statistics_cultures_created";
    pAsset7.localized_key_description = "statistics_cultures".Description();
    pAsset7.list_window_meta_type = MetaType.Culture;
    pAsset7.path_icon = "ui/Icons/iconCulture";
    pAsset7.long_action = (StatisticsLongAction) (_ => World.world.map_stats.culturesCreated);
    pAsset7.is_world_statistics = true;
    pAsset7.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset7);
    StatisticsAsset pAsset8 = new StatisticsAsset();
    pAsset8.id = "world_statistics_cultures_forgotten";
    pAsset8.list_window_meta_type = MetaType.Culture;
    pAsset8.path_icon = "ui/Icons/iconCultureForgotten";
    pAsset8.long_action = (StatisticsLongAction) (_ => World.world.map_stats.culturesForgotten);
    pAsset8.is_world_statistics = true;
    pAsset8.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset8);
    StatisticsAsset pAsset9 = new StatisticsAsset();
    pAsset9.id = "world_statistics_books_written";
    pAsset9.localized_key_description = "books".Description();
    pAsset9.list_window_meta_type = MetaType.Language;
    pAsset9.path_icon = "ui/Icons/iconBooksWritten";
    pAsset9.long_action = (StatisticsLongAction) (_ => World.world.map_stats.booksWritten);
    pAsset9.is_world_statistics = true;
    pAsset9.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset9);
    StatisticsAsset pAsset10 = new StatisticsAsset();
    pAsset10.id = "world_statistics_books_read";
    pAsset10.list_window_meta_type = MetaType.Language;
    pAsset10.path_icon = "ui/Icons/iconBooksRead";
    pAsset10.long_action = (StatisticsLongAction) (_ => World.world.map_stats.booksRead);
    pAsset10.is_world_statistics = true;
    pAsset10.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset10);
    StatisticsAsset pAsset11 = new StatisticsAsset();
    pAsset11.id = "world_statistics_books_burnt";
    pAsset11.list_window_meta_type = MetaType.Language;
    pAsset11.path_icon = "ui/Icons/iconBooksDestroyed";
    pAsset11.long_action = (StatisticsLongAction) (_ => World.world.map_stats.booksBurnt);
    pAsset11.is_world_statistics = true;
    pAsset11.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset11);
    StatisticsAsset pAsset12 = new StatisticsAsset();
    pAsset12.id = "world_statistics_religions_created";
    pAsset12.localized_key_description = "statistics_religions".Description();
    pAsset12.list_window_meta_type = MetaType.Religion;
    pAsset12.path_icon = "ui/Icons/iconReligion";
    pAsset12.long_action = (StatisticsLongAction) (_ => World.world.map_stats.religionsCreated);
    pAsset12.is_world_statistics = true;
    pAsset12.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset12);
    StatisticsAsset pAsset13 = new StatisticsAsset();
    pAsset13.id = "world_statistics_religions_forgotten";
    pAsset13.list_window_meta_type = MetaType.Religion;
    pAsset13.path_icon = "ui/Icons/iconReligionForgotten";
    pAsset13.long_action = (StatisticsLongAction) (_ => World.world.map_stats.religionsForgotten);
    pAsset13.is_world_statistics = true;
    pAsset13.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset13);
    StatisticsAsset pAsset14 = new StatisticsAsset();
    pAsset14.id = "world_statistics_kingdoms_created";
    pAsset14.list_window_meta_type = MetaType.Kingdom;
    pAsset14.path_icon = "ui/Icons/iconKingdom";
    pAsset14.long_action = (StatisticsLongAction) (_ => World.world.map_stats.kingdomsCreated);
    pAsset14.is_world_statistics = true;
    pAsset14.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset14);
    StatisticsAsset pAsset15 = new StatisticsAsset();
    pAsset15.id = "world_statistics_cities_created";
    pAsset15.localized_key_description = "statistics_villages".Description();
    pAsset15.list_window_meta_type = MetaType.City;
    pAsset15.path_icon = "ui/Icons/iconCity";
    pAsset15.long_action = (StatisticsLongAction) (_ => World.world.map_stats.citiesCreated);
    pAsset15.is_world_statistics = true;
    pAsset15.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset15);
    StatisticsAsset pAsset16 = new StatisticsAsset();
    pAsset16.id = "world_statistics_cities_conquered";
    pAsset16.list_window_meta_type = MetaType.City;
    pAsset16.path_icon = "ui/Icons/iconCityConquered";
    pAsset16.long_action = (StatisticsLongAction) (_ => World.world.map_stats.citiesConquered);
    pAsset16.is_world_statistics = true;
    pAsset16.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset16);
    StatisticsAsset pAsset17 = new StatisticsAsset();
    pAsset17.id = "statistics_cities_conquered";
    pAsset17.path_icon = "ui/Icons/iconCityConquered";
    pAsset17.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.citiesConquered);
    pAsset17.is_game_statistics = true;
    pAsset17.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset17);
    StatisticsAsset pAsset18 = new StatisticsAsset();
    pAsset18.id = "world_statistics_cities_rebelled";
    pAsset18.list_window_meta_type = MetaType.City;
    pAsset18.path_icon = "ui/Icons/worldrules/icon_rebellion";
    pAsset18.long_action = (StatisticsLongAction) (_ => World.world.map_stats.citiesRebelled);
    pAsset18.is_world_statistics = true;
    pAsset18.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset18);
    StatisticsAsset pAsset19 = new StatisticsAsset();
    pAsset19.id = "statistics_cities_rebelled";
    pAsset19.path_icon = "ui/Icons/worldrules/icon_rebellion";
    pAsset19.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.citiesRebelled);
    pAsset19.is_game_statistics = true;
    pAsset19.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset19);
    StatisticsAsset pAsset20 = new StatisticsAsset();
    pAsset20.id = "world_statistics_alliances_made";
    pAsset20.localized_key_description = "statistics_alliances".Description();
    pAsset20.list_window_meta_type = MetaType.Alliance;
    pAsset20.path_icon = "ui/Icons/iconAlliance";
    pAsset20.long_action = (StatisticsLongAction) (_ => World.world.map_stats.alliancesMade);
    pAsset20.is_world_statistics = true;
    pAsset20.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset20);
    StatisticsAsset pAsset21 = new StatisticsAsset();
    pAsset21.id = "world_statistics_alliances_dissolved";
    pAsset21.list_window_meta_type = MetaType.Alliance;
    pAsset21.path_icon = "ui/Icons/iconAllianceDissolved";
    pAsset21.long_action = (StatisticsLongAction) (_ => World.world.map_stats.alliancesDissolved);
    pAsset21.is_world_statistics = true;
    pAsset21.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset21);
    StatisticsAsset pAsset22 = new StatisticsAsset();
    pAsset22.id = "statistics_alliances_made";
    pAsset22.path_icon = "ui/Icons/iconAlliance";
    pAsset22.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.alliancesMade);
    pAsset22.is_game_statistics = true;
    pAsset22.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset22);
    StatisticsAsset pAsset23 = new StatisticsAsset();
    pAsset23.id = "statistics_alliances_dissolved";
    pAsset23.path_icon = "ui/Icons/iconAllianceDissolved";
    pAsset23.long_action = (StatisticsLongAction) (_ => World.world.game_stats.data.alliancesDissolved);
    pAsset23.is_game_statistics = true;
    pAsset23.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset23);
    StatisticsAsset pAsset24 = new StatisticsAsset();
    pAsset24.id = "world_statistics_wars_started";
    pAsset24.localized_key_description = "statistics_wars".Description();
    pAsset24.list_window_meta_type = MetaType.War;
    pAsset24.path_icon = "ui/Icons/iconWhisperOfWar";
    pAsset24.long_action = (StatisticsLongAction) (_ => World.world.map_stats.warsStarted);
    pAsset24.is_world_statistics = true;
    pAsset24.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset24);
    StatisticsAsset pAsset25 = new StatisticsAsset();
    pAsset25.id = "world_statistics_peaces_made";
    pAsset25.list_window_meta_type = MetaType.War;
    pAsset25.path_icon = "ui/Icons/actor_traits/iconPacifist";
    pAsset25.long_action = (StatisticsLongAction) (_ => World.world.map_stats.peacesMade);
    pAsset25.is_world_statistics = true;
    pAsset25.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset25);
    StatisticsAsset pAsset26 = new StatisticsAsset();
    pAsset26.id = "world_statistics_plots_started";
    pAsset26.localized_key_description = "statistics_plots".Description();
    pAsset26.list_window_meta_type = MetaType.Plot;
    pAsset26.path_icon = "ui/Icons/iconPlot";
    pAsset26.long_action = (StatisticsLongAction) (_ => World.world.map_stats.plotsStarted);
    pAsset26.is_world_statistics = true;
    pAsset26.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset26);
    StatisticsAsset pAsset27 = new StatisticsAsset();
    pAsset27.id = "world_statistics_plots_succeeded";
    pAsset27.list_window_meta_type = MetaType.Plot;
    pAsset27.path_icon = "ui/Icons/iconPlotSucceeded";
    pAsset27.long_action = (StatisticsLongAction) (_ => World.world.map_stats.plotsSucceeded);
    pAsset27.is_world_statistics = true;
    pAsset27.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset27);
    StatisticsAsset pAsset28 = new StatisticsAsset();
    pAsset28.id = "world_statistics_plots_forgotten";
    pAsset28.list_window_meta_type = MetaType.Plot;
    pAsset28.path_icon = "ui/Icons/iconPlotForgotten";
    pAsset28.long_action = (StatisticsLongAction) (_ => World.world.map_stats.plotsForgotten);
    pAsset28.is_world_statistics = true;
    pAsset28.world_stats_tabs = WorldStatsTabs.Noosphere;
    this.add(pAsset28);
    StatisticsAsset pAsset29 = new StatisticsAsset();
    pAsset29.id = "world_statistics_most_populated_village";
    pAsset29.path_icon = "ui/Icons/iconCity";
    pAsset29.is_world_statistics = true;
    pAsset29.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset29.world_stats_meta_type = MetaType.City;
    pAsset29.list_window_meta_type = MetaType.City;
    pAsset29.string_action = (StatisticsStringAction) (_ => this.getDominatingMetaRow(MetaType.City));
    pAsset29.get_meta_id = (MetaIdGetter) (_ => this.getDominatingMetaId(MetaType.City));
    this.add(pAsset29);
    StatisticsAsset pAsset30 = new StatisticsAsset();
    pAsset30.id = "world_statistics_biggest_village";
    pAsset30.path_icon = "ui/Icons/iconCity";
    pAsset30.is_world_statistics = true;
    pAsset30.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset30.world_stats_meta_type = MetaType.City;
    pAsset30.list_window_meta_type = MetaType.City;
    pAsset30.string_action = (StatisticsStringAction) (pAsset =>
    {
      City city = World.world.cities.get(pAsset.get_meta_id(pAsset));
      if (city == null)
        return StatisticsLibrary._unknown_text;
      string colorText = city.getColor().color_text;
      return Toolbox.coloredText(city.name + Toolbox.coloredGreyPart((object) city.zones.Count, colorText), colorText);
    });
    pAsset30.get_meta_id = (MetaIdGetter) (_ =>
    {
      City city1 = (City) null;
      foreach (City city2 in (CoreSystemManager<City, CityData>) World.world.cities)
      {
        if (city1 == null || city2.zones.Count > city1.zones.Count)
          city1 = city2;
      }
      return city1 == null ? -1L : city1.id;
    });
    this.add(pAsset30);
    StatisticsAsset pAsset31 = new StatisticsAsset();
    pAsset31.id = "world_statistics_most_populated_kingdom";
    pAsset31.path_icon = "ui/Icons/iconKingdom";
    pAsset31.is_world_statistics = true;
    pAsset31.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset31.world_stats_meta_type = MetaType.Kingdom;
    pAsset31.list_window_meta_type = MetaType.Kingdom;
    pAsset31.string_action = (StatisticsStringAction) (_ => this.getDominatingMetaRow(MetaType.Kingdom));
    pAsset31.get_meta_id = (MetaIdGetter) (_ => this.getDominatingMetaId(MetaType.Kingdom));
    this.add(pAsset31);
    StatisticsAsset pAsset32 = new StatisticsAsset();
    pAsset32.id = "world_statistics_dominating_culture";
    pAsset32.path_icon = "ui/Icons/iconCulture";
    pAsset32.is_world_statistics = true;
    pAsset32.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset32.world_stats_meta_type = MetaType.Culture;
    pAsset32.list_window_meta_type = MetaType.Culture;
    pAsset32.string_action = (StatisticsStringAction) (_ => this.getDominatingMetaRow(MetaType.Culture));
    pAsset32.get_meta_id = (MetaIdGetter) (_ => this.getDominatingMetaId(MetaType.Culture));
    this.add(pAsset32);
    StatisticsAsset pAsset33 = new StatisticsAsset();
    pAsset33.id = "world_statistics_dominating_language";
    pAsset33.path_icon = "ui/Icons/iconLanguage";
    pAsset33.is_world_statistics = true;
    pAsset33.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset33.world_stats_meta_type = MetaType.Language;
    pAsset33.list_window_meta_type = MetaType.Language;
    pAsset33.string_action = (StatisticsStringAction) (_ => this.getDominatingMetaRow(MetaType.Language));
    pAsset33.get_meta_id = (MetaIdGetter) (_ => this.getDominatingMetaId(MetaType.Language));
    this.add(pAsset33);
    StatisticsAsset pAsset34 = new StatisticsAsset();
    pAsset34.id = "world_statistics_dominating_religion";
    pAsset34.path_icon = "ui/Icons/iconReligion";
    pAsset34.is_world_statistics = true;
    pAsset34.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset34.world_stats_meta_type = MetaType.Religion;
    pAsset34.list_window_meta_type = MetaType.Religion;
    pAsset34.string_action = (StatisticsStringAction) (_ => this.getDominatingMetaRow(MetaType.Religion));
    pAsset34.get_meta_id = (MetaIdGetter) (_ => this.getDominatingMetaId(MetaType.Religion));
    this.add(pAsset34);
    StatisticsAsset pAsset35 = new StatisticsAsset();
    pAsset35.id = "world_statistics_dominating_subspecies";
    pAsset35.path_icon = "ui/Icons/iconSpecies";
    pAsset35.is_world_statistics = true;
    pAsset35.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset35.world_stats_meta_type = MetaType.Subspecies;
    pAsset35.list_window_meta_type = MetaType.Subspecies;
    pAsset35.string_action = (StatisticsStringAction) (_ => this.getDominatingMetaRow(MetaType.Subspecies));
    pAsset35.get_meta_id = (MetaIdGetter) (_ => this.getDominatingMetaId(MetaType.Subspecies));
    this.add(pAsset35);
    StatisticsAsset pAsset36 = new StatisticsAsset();
    pAsset36.id = "world_statistics_oldest_clan";
    pAsset36.path_icon = "ui/Icons/iconClan";
    pAsset36.is_world_statistics = true;
    pAsset36.world_stats_tabs = WorldStatsTabs.Noosphere;
    pAsset36.world_stats_meta_type = MetaType.Clan;
    pAsset36.list_window_meta_type = MetaType.Clan;
    pAsset36.string_action = (StatisticsStringAction) (_ => this.getOldestMetaRow(MetaType.Clan));
    pAsset36.get_meta_id = (MetaIdGetter) (_ => this.getOldestMetaId(MetaType.Clan));
    this.add(pAsset36);
  }

  public void addStatsTiles()
  {
    StatisticsAsset pAsset1 = new StatisticsAsset();
    pAsset1.id = "world_statistics_water";
    pAsset1.localized_key = "Water";
    pAsset1.path_icon = "ui/Icons/iconTileDeepOcean";
    pAsset1.long_action = (StatisticsLongAction) (_ => (long) (TileLibrary.deep_ocean.hashset.Count + TileLibrary.close_ocean.hashset.Count + TileLibrary.shallow_waters.hashset.Count));
    pAsset1.is_world_statistics = true;
    pAsset1.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset1);
    StatisticsAsset pAsset2 = new StatisticsAsset();
    pAsset2.id = "world_statistics_soil";
    pAsset2.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("tile_soil"));
    pAsset2.path_icon = "ui/Icons/iconTileSoil";
    pAsset2.long_action = (StatisticsLongAction) (_ => (long) (TileLibrary.soil_low.hashset.Count + TileLibrary.soil_high.hashset.Count));
    pAsset2.is_world_statistics = true;
    pAsset2.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset2);
    StatisticsAsset pAsset3 = new StatisticsAsset();
    pAsset3.id = "world_statistics_summit";
    pAsset3.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("tile_summit"));
    pAsset3.path_icon = "ui/Icons/iconTileSummit";
    pAsset3.long_action = (StatisticsLongAction) (_ => (long) TileLibrary.summit.hashset.Count);
    pAsset3.is_world_statistics = true;
    pAsset3.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset3);
    StatisticsAsset pAsset4 = new StatisticsAsset();
    pAsset4.id = "world_statistics_mountains";
    pAsset4.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("tile_mountains"));
    pAsset4.path_icon = "ui/Icons/iconTileMountains";
    pAsset4.long_action = (StatisticsLongAction) (_ => (long) TileLibrary.mountains.hashset.Count);
    pAsset4.is_world_statistics = true;
    pAsset4.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset4);
    StatisticsAsset pAsset5 = new StatisticsAsset();
    pAsset5.id = "world_statistics_hills";
    pAsset5.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("tile_hills"));
    pAsset5.path_icon = "ui/Icons/iconTileHills";
    pAsset5.long_action = (StatisticsLongAction) (_ => (long) TileLibrary.hills.hashset.Count);
    pAsset5.is_world_statistics = true;
    pAsset5.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset5);
    StatisticsAsset pAsset6 = new StatisticsAsset();
    pAsset6.id = "world_statistics_lava";
    pAsset6.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("lava"));
    pAsset6.path_icon = "ui/Icons/iconLava";
    pAsset6.long_action = (StatisticsLongAction) (_ => (long) (TileLibrary.lava0.hashset.Count + TileLibrary.lava1.hashset.Count + TileLibrary.lava2.hashset.Count + TileLibrary.lava3.hashset.Count));
    pAsset6.is_world_statistics = true;
    pAsset6.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset6);
    StatisticsAsset pAsset7 = new StatisticsAsset();
    pAsset7.id = "world_statistics_pit";
    pAsset7.localized_key = "Pit";
    pAsset7.path_icon = "ui/Icons/iconTileShallowWater";
    pAsset7.long_action = (StatisticsLongAction) (_ => (long) (TileLibrary.pit_deep_ocean.hashset.Count + TileLibrary.pit_close_ocean.hashset.Count + TileLibrary.pit_shallow_waters.hashset.Count));
    pAsset7.is_world_statistics = true;
    pAsset7.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset7);
    StatisticsAsset pAsset8 = new StatisticsAsset();
    pAsset8.id = "world_statistics_field";
    pAsset8.localized_key = "fields";
    pAsset8.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobFarmer";
    pAsset8.long_action = (StatisticsLongAction) (_ => (long) TopTileLibrary.field.hashset.Count);
    pAsset8.is_world_statistics = true;
    pAsset8.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset8);
    StatisticsAsset pAsset9 = new StatisticsAsset();
    pAsset9.id = "world_statistics_fireworks";
    pAsset9.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("fireworks"));
    pAsset9.path_icon = "ui/Icons/iconFireworks";
    pAsset9.long_action = (StatisticsLongAction) (_ => (long) TopTileLibrary.fireworks.hashset.Count);
    pAsset9.is_world_statistics = true;
    pAsset9.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset9);
    StatisticsAsset pAsset10 = new StatisticsAsset();
    pAsset10.id = "world_statistics_frozen";
    pAsset10.localized_key = "Frozen";
    pAsset10.path_icon = "ui/Icons/iconFrozen";
    pAsset10.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.frozen_high.hashset.Count + TopTileLibrary.frozen_low.hashset.Count));
    pAsset10.is_world_statistics = true;
    pAsset10.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset10);
    StatisticsAsset pAsset11 = new StatisticsAsset();
    pAsset11.id = "world_statistics_fuse";
    pAsset11.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("fuse"));
    pAsset11.path_icon = "ui/Icons/iconFuse";
    pAsset11.long_action = (StatisticsLongAction) (_ => (long) TopTileLibrary.fuse.hashset.Count);
    pAsset11.is_world_statistics = true;
    pAsset11.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset11);
    StatisticsAsset pAsset12 = new StatisticsAsset();
    pAsset12.id = "world_statistics_ice";
    pAsset12.localized_key = "Ice";
    pAsset12.path_icon = "ui/Icons/iconIceberg";
    pAsset12.long_action = (StatisticsLongAction) (_ => (long) TopTileLibrary.ice.hashset.Count);
    pAsset12.is_world_statistics = true;
    pAsset12.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset12);
    StatisticsAsset pAsset13 = new StatisticsAsset();
    pAsset13.id = "world_statistics_landmine";
    pAsset13.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("landmine"));
    pAsset13.path_icon = "ui/Icons/iconLandmine";
    pAsset13.long_action = (StatisticsLongAction) (_ => (long) TopTileLibrary.landmine.hashset.Count);
    pAsset13.is_world_statistics = true;
    pAsset13.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset13);
    StatisticsAsset pAsset14 = new StatisticsAsset();
    pAsset14.id = "world_statistics_road";
    pAsset14.localized_key = "Roads";
    pAsset14.path_icon = "ui/Icons/citizen_jobs/iconCitizenJobRoadBuilder";
    pAsset14.long_action = (StatisticsLongAction) (_ => (long) TopTileLibrary.road.hashset.Count);
    pAsset14.is_world_statistics = true;
    pAsset14.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset14);
    StatisticsAsset pAsset15 = new StatisticsAsset();
    pAsset15.id = "world_statistics_snow";
    pAsset15.localized_key = "Snow";
    pAsset15.path_icon = "ui/Icons/iconSnow";
    pAsset15.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.snow_hills.hashset.Count + TopTileLibrary.snow_block.hashset.Count + TopTileLibrary.snow_summit.hashset.Count + TopTileLibrary.snow_sand.hashset.Count));
    pAsset15.is_world_statistics = true;
    pAsset15.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset15);
    StatisticsAsset pAsset16 = new StatisticsAsset();
    pAsset16.id = "world_statistics_tnt";
    pAsset16.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("tnt"));
    pAsset16.path_icon = "ui/Icons/iconTnt";
    pAsset16.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.tnt.hashset.Count + TopTileLibrary.tnt_timed.hashset.Count));
    pAsset16.is_world_statistics = true;
    pAsset16.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset16);
    StatisticsAsset pAsset17 = new StatisticsAsset();
    pAsset17.id = "world_statistics_wall";
    pAsset17.localized_key = "Walls";
    pAsset17.path_icon = "ui/Icons/iconWallIron";
    pAsset17.long_action = (StatisticsLongAction) (_ => (long) (TopTileLibrary.wall_evil.hashset.Count + TopTileLibrary.wall_order.hashset.Count + TopTileLibrary.wall_ancient.hashset.Count + TopTileLibrary.wall_wild.hashset.Count + TopTileLibrary.wall_green.hashset.Count + TopTileLibrary.wall_iron.hashset.Count + TopTileLibrary.wall_light.hashset.Count));
    pAsset17.is_world_statistics = true;
    pAsset17.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset17);
    StatisticsAsset pAsset18 = new StatisticsAsset();
    pAsset18.id = "world_statistics_water_bomb";
    pAsset18.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("water_bomb"));
    pAsset18.path_icon = "ui/Icons/iconWaterBomb";
    pAsset18.long_action = (StatisticsLongAction) (_ => (long) TopTileLibrary.water_bomb.hashset.Count);
    pAsset18.is_world_statistics = true;
    pAsset18.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset18);
    StatisticsAsset pAsset19 = new StatisticsAsset();
    pAsset19.id = "world_statistics_grey_goo";
    pAsset19.locale_getter = (LocaleGetter) (() => StatisticsLibrary.getPowerLocale("grey_goo"));
    pAsset19.path_icon = "ui/Icons/iconGreygoo";
    pAsset19.long_action = (StatisticsLongAction) (_ => (long) TileLibrary.grey_goo.hashset.Count);
    pAsset19.is_world_statistics = true;
    pAsset19.world_stats_tabs = WorldStatsTabs.General;
    this.add(pAsset19);
  }
}
