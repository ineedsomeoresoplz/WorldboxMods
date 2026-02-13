// Decompiled with JetBrains decompiler
// Type: db.HistoryMetaDataLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db.tables;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace db;

public class HistoryMetaDataLibrary : AssetLibrary<HistoryMetaDataAsset>
{
  public static readonly Dictionary<MetaType, HistoryMetaDataAsset[]> _meta_data = new Dictionary<MetaType, HistoryMetaDataAsset[]>();
  public static readonly Dictionary<string, HistoryMetaDataAsset[]> _meta_dict = new Dictionary<string, HistoryMetaDataAsset[]>();

  public override void init()
  {
    base.init();
    HistoryMetaDataAsset pAsset1 = new HistoryMetaDataAsset();
    pAsset1.id = "world";
    pAsset1.meta_type = MetaType.World;
    pAsset1.table_type = typeof (WorldTable);
    pAsset1.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (WorldYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (WorldYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (WorldYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (WorldYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (WorldYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (WorldYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (WorldYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (WorldYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (WorldYearly10000)
      }
    };
    this.add(pAsset1);
    this.t.collector = (HistoryDataCollector) (_ =>
    {
      WorldYearly1 worldYearly1 = new WorldYearly1();
      ((HistoryTable) worldYearly1).id = 1L;
      ((WorldTable) worldYearly1).alliances = new long?(StatsHelper.getStat("alliances"));
      ((WorldTable) worldYearly1).alliances_dissolved = new long?(StatsHelper.getStat("world_statistics_alliances_dissolved"));
      ((WorldTable) worldYearly1).alliances_made = new long?(StatsHelper.getStat("world_statistics_alliances_made"));
      ((WorldTable) worldYearly1).books = new long?(StatsHelper.getStat("books"));
      ((WorldTable) worldYearly1).books_burnt = new long?(StatsHelper.getStat("world_statistics_books_burnt"));
      ((WorldTable) worldYearly1).books_read = new long?(StatsHelper.getStat("world_statistics_books_read"));
      ((WorldTable) worldYearly1).books_written = new long?(StatsHelper.getStat("world_statistics_books_written"));
      ((WorldTable) worldYearly1).cities = new long?(StatsHelper.getStat("villages"));
      ((WorldTable) worldYearly1).cities_rebelled = new long?(StatsHelper.getStat("world_statistics_cities_rebelled"));
      ((WorldTable) worldYearly1).cities_conquered = new long?(StatsHelper.getStat("world_statistics_cities_conquered"));
      ((WorldTable) worldYearly1).cities_created = new long?(StatsHelper.getStat("world_statistics_cities_created"));
      ((WorldTable) worldYearly1).cities_destroyed = new long?(StatsHelper.getStat("world_statistics_cities_destroyed"));
      ((WorldTable) worldYearly1).clans = new long?(StatsHelper.getStat("clans"));
      ((WorldTable) worldYearly1).clans_created = new long?(StatsHelper.getStat("world_statistics_clans_created"));
      ((WorldTable) worldYearly1).clans_destroyed = new long?(StatsHelper.getStat("world_statistics_clans_destroyed"));
      ((WorldTable) worldYearly1).creatures_born = new long?(StatsHelper.getStat("world_statistics_creatures_born"));
      ((WorldTable) worldYearly1).creatures_created = new long?(StatsHelper.getStat("world_statistics_creatures_created"));
      ((WorldTable) worldYearly1).cultures = new long?(StatsHelper.getStat("cultures"));
      ((WorldTable) worldYearly1).cultures_created = new long?(StatsHelper.getStat("world_statistics_cultures_created"));
      ((WorldTable) worldYearly1).cultures_forgotten = new long?(StatsHelper.getStat("world_statistics_cultures_forgotten"));
      ((WorldTable) worldYearly1).deaths_eaten = new long?(StatsHelper.getStat("world_statistics_deaths_eaten"));
      ((WorldTable) worldYearly1).deaths_hunger = new long?(StatsHelper.getStat("world_statistics_deaths_hunger"));
      ((WorldTable) worldYearly1).deaths_natural = new long?(StatsHelper.getStat("world_statistics_deaths_natural"));
      ((WorldTable) worldYearly1).deaths_poison = new long?(StatsHelper.getStat("world_statistics_deaths_poison"));
      ((WorldTable) worldYearly1).deaths_infection = new long?(StatsHelper.getStat("world_statistics_deaths_infection"));
      ((WorldTable) worldYearly1).deaths_tumor = new long?(StatsHelper.getStat("world_statistics_deaths_tumor"));
      ((WorldTable) worldYearly1).deaths_acid = new long?(StatsHelper.getStat("world_statistics_deaths_acid"));
      ((WorldTable) worldYearly1).deaths_fire = new long?(StatsHelper.getStat("world_statistics_deaths_fire"));
      ((WorldTable) worldYearly1).deaths_divine = new long?(StatsHelper.getStat("world_statistics_deaths_divine"));
      ((WorldTable) worldYearly1).deaths_weapon = new long?(StatsHelper.getStat("world_statistics_deaths_weapon"));
      ((WorldTable) worldYearly1).deaths_gravity = new long?(StatsHelper.getStat("world_statistics_deaths_gravity"));
      ((WorldTable) worldYearly1).deaths_drowning = new long?(StatsHelper.getStat("world_statistics_deaths_drowning"));
      ((WorldTable) worldYearly1).deaths_water = new long?(StatsHelper.getStat("world_statistics_deaths_water"));
      ((WorldTable) worldYearly1).deaths_explosion = new long?(StatsHelper.getStat("world_statistics_deaths_explosion"));
      ((WorldTable) worldYearly1).metamorphosis = new long?(StatsHelper.getStat("world_statistics_metamorphosis"));
      ((WorldTable) worldYearly1).evolutions = new long?(StatsHelper.getStat("world_statistics_evolutions"));
      ((WorldTable) worldYearly1).deaths_other = new long?(StatsHelper.getStat("world_statistics_deaths_other"));
      ((WorldTable) worldYearly1).deaths_plague = new long?(StatsHelper.getStat("world_statistics_deaths_plague"));
      ((WorldTable) worldYearly1).deaths_total = new long?(StatsHelper.getStat("world_statistics_deaths_total"));
      ((WorldTable) worldYearly1).families = new long?(StatsHelper.getStat("families"));
      ((WorldTable) worldYearly1).families_created = new long?(StatsHelper.getStat("world_statistics_families_created"));
      ((WorldTable) worldYearly1).families_destroyed = new long?(StatsHelper.getStat("world_statistics_families_destroyed"));
      ((WorldTable) worldYearly1).houses = new long?(StatsHelper.getStat("world_statistics_houses"));
      ((WorldTable) worldYearly1).houses_built = new long?(StatsHelper.getStat("world_statistics_houses_built"));
      ((WorldTable) worldYearly1).houses_destroyed = new long?(StatsHelper.getStat("world_statistics_houses_destroyed"));
      ((WorldTable) worldYearly1).infected = new long?(StatsHelper.getStat("world_statistics_infected"));
      ((WorldTable) worldYearly1).islands = new long?(StatsHelper.getStat("world_statistics_islands"));
      ((WorldTable) worldYearly1).kingdoms = new long?(StatsHelper.getStat("kingdoms"));
      ((WorldTable) worldYearly1).kingdoms_created = new long?(StatsHelper.getStat("world_statistics_kingdoms_created"));
      ((WorldTable) worldYearly1).kingdoms_destroyed = new long?(StatsHelper.getStat("world_statistics_kingdoms_destroyed"));
      ((WorldTable) worldYearly1).languages = new long?(StatsHelper.getStat("languages"));
      ((WorldTable) worldYearly1).languages_created = new long?(StatsHelper.getStat("world_statistics_languages_created"));
      ((WorldTable) worldYearly1).languages_forgotten = new long?(StatsHelper.getStat("world_statistics_languages_forgotten"));
      ((WorldTable) worldYearly1).peaces_made = new long?(StatsHelper.getStat("world_statistics_peaces_made"));
      ((WorldTable) worldYearly1).plots = new long?(StatsHelper.getStat("plots"));
      ((WorldTable) worldYearly1).plots_forgotten = new long?(StatsHelper.getStat("world_statistics_plots_forgotten"));
      ((WorldTable) worldYearly1).plots_started = new long?(StatsHelper.getStat("world_statistics_plots_started"));
      ((WorldTable) worldYearly1).plots_succeeded = new long?(StatsHelper.getStat("world_statistics_plots_succeeded"));
      ((WorldTable) worldYearly1).population_beasts = new long?(StatsHelper.getStat("world_statistics_beasts"));
      ((WorldTable) worldYearly1).population_civ = new long?(StatsHelper.getStat("world_statistics_population"));
      ((WorldTable) worldYearly1).religions = new long?(StatsHelper.getStat("religions"));
      ((WorldTable) worldYearly1).religions_created = new long?(StatsHelper.getStat("world_statistics_religions_created"));
      ((WorldTable) worldYearly1).religions_forgotten = new long?(StatsHelper.getStat("world_statistics_religions_forgotten"));
      ((WorldTable) worldYearly1).subspecies = new long?(StatsHelper.getStat("subspecies"));
      ((WorldTable) worldYearly1).subspecies_created = new long?(StatsHelper.getStat("world_statistics_subspecies_created"));
      ((WorldTable) worldYearly1).subspecies_extinct = new long?(StatsHelper.getStat("world_statistics_subspecies_extinct"));
      ((WorldTable) worldYearly1).trees = new long?(StatsHelper.getStat("world_statistics_trees"));
      ((WorldTable) worldYearly1).vegetation = new long?(StatsHelper.getStat("world_statistics_vegetation"));
      ((WorldTable) worldYearly1).wars = new long?(StatsHelper.getStat("wars"));
      ((WorldTable) worldYearly1).wars_started = new long?(StatsHelper.getStat("world_statistics_wars_started"));
      ((WorldTable) worldYearly1).grass = new long?(StatsHelper.getStat("world_statistics_grass"));
      ((WorldTable) worldYearly1).savanna = new long?(StatsHelper.getStat("world_statistics_savanna"));
      ((WorldTable) worldYearly1).jungle = new long?(StatsHelper.getStat("world_statistics_jungle"));
      ((WorldTable) worldYearly1).desert = new long?(StatsHelper.getStat("world_statistics_desert"));
      ((WorldTable) worldYearly1).lemon = new long?(StatsHelper.getStat("world_statistics_lemon"));
      ((WorldTable) worldYearly1).permafrost = new long?(StatsHelper.getStat("world_statistics_permafrost"));
      ((WorldTable) worldYearly1).swamp = new long?(StatsHelper.getStat("world_statistics_swamp"));
      ((WorldTable) worldYearly1).crystal = new long?(StatsHelper.getStat("world_statistics_crystal"));
      ((WorldTable) worldYearly1).enchanted = new long?(StatsHelper.getStat("world_statistics_enchanted"));
      ((WorldTable) worldYearly1).corruption = new long?(StatsHelper.getStat("world_statistics_corruption"));
      ((WorldTable) worldYearly1).infernal = new long?(StatsHelper.getStat("world_statistics_infernal"));
      ((WorldTable) worldYearly1).candy = new long?(StatsHelper.getStat("world_statistics_candy"));
      ((WorldTable) worldYearly1).mushroom = new long?(StatsHelper.getStat("world_statistics_mushroom"));
      ((WorldTable) worldYearly1).wasteland = new long?(StatsHelper.getStat("world_statistics_wasteland"));
      ((WorldTable) worldYearly1).birch = new long?(StatsHelper.getStat("world_statistics_birch"));
      ((WorldTable) worldYearly1).maple = new long?(StatsHelper.getStat("world_statistics_maple"));
      ((WorldTable) worldYearly1).rocklands = new long?(StatsHelper.getStat("world_statistics_rocklands"));
      ((WorldTable) worldYearly1).garlic = new long?(StatsHelper.getStat("world_statistics_garlic"));
      ((WorldTable) worldYearly1).flower = new long?(StatsHelper.getStat("world_statistics_flower"));
      ((WorldTable) worldYearly1).celestial = new long?(StatsHelper.getStat("world_statistics_celestial"));
      ((WorldTable) worldYearly1).clover = new long?(StatsHelper.getStat("world_statistics_clover"));
      ((WorldTable) worldYearly1).singularity = new long?(StatsHelper.getStat("world_statistics_singularity"));
      ((WorldTable) worldYearly1).paradox = new long?(StatsHelper.getStat("world_statistics_paradox"));
      ((WorldTable) worldYearly1).sand = new long?(StatsHelper.getStat("world_statistics_sand"));
      ((WorldTable) worldYearly1).biomass = new long?(StatsHelper.getStat("world_statistics_biomass"));
      ((WorldTable) worldYearly1).cybertile = new long?(StatsHelper.getStat("world_statistics_cybertile"));
      ((WorldTable) worldYearly1).pumpkin = new long?(StatsHelper.getStat("world_statistics_pumpkin"));
      ((WorldTable) worldYearly1).tumor = new long?(StatsHelper.getStat("world_statistics_tumor"));
      ((WorldTable) worldYearly1).water = new long?(StatsHelper.getStat("world_statistics_water"));
      ((WorldTable) worldYearly1).soil = new long?(StatsHelper.getStat("world_statistics_soil"));
      ((WorldTable) worldYearly1).summit = new long?(StatsHelper.getStat("world_statistics_summit"));
      ((WorldTable) worldYearly1).mountains = new long?(StatsHelper.getStat("world_statistics_mountains"));
      ((WorldTable) worldYearly1).hills = new long?(StatsHelper.getStat("world_statistics_hills"));
      ((WorldTable) worldYearly1).lava = new long?(StatsHelper.getStat("world_statistics_lava"));
      ((WorldTable) worldYearly1).pit = new long?(StatsHelper.getStat("world_statistics_pit"));
      ((WorldTable) worldYearly1).field = new long?(StatsHelper.getStat("world_statistics_field"));
      ((WorldTable) worldYearly1).fireworks = new long?(StatsHelper.getStat("world_statistics_fireworks"));
      ((WorldTable) worldYearly1).frozen = new long?(StatsHelper.getStat("world_statistics_frozen"));
      ((WorldTable) worldYearly1).fuse = new long?(StatsHelper.getStat("world_statistics_fuse"));
      ((WorldTable) worldYearly1).ice = new long?(StatsHelper.getStat("world_statistics_ice"));
      ((WorldTable) worldYearly1).landmine = new long?(StatsHelper.getStat("world_statistics_landmine"));
      ((WorldTable) worldYearly1).road = new long?(StatsHelper.getStat("world_statistics_road"));
      ((WorldTable) worldYearly1).snow = new long?(StatsHelper.getStat("world_statistics_snow"));
      ((WorldTable) worldYearly1).tnt = new long?(StatsHelper.getStat("world_statistics_tnt"));
      ((WorldTable) worldYearly1).wall = new long?(StatsHelper.getStat("world_statistics_wall"));
      ((WorldTable) worldYearly1).water_bomb = new long?(StatsHelper.getStat("world_statistics_water_bomb"));
      ((WorldTable) worldYearly1).grey_goo = new long?(StatsHelper.getStat("world_statistics_grey_goo"));
      return (HistoryTable) worldYearly1;
    });
    HistoryMetaDataAsset pAsset2 = new HistoryMetaDataAsset();
    pAsset2.id = "alliance";
    pAsset2.meta_type = MetaType.Alliance;
    pAsset2.table_type = typeof (AllianceTable);
    pAsset2.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (AllianceYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (AllianceYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (AllianceYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (AllianceYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (AllianceYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (AllianceYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (AllianceYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (AllianceYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (AllianceYearly10000)
      }
    };
    this.add(pAsset2);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Alliance alliance = (Alliance) pNanoObject;
      AllianceYearly1 allianceYearly1 = new AllianceYearly1();
      ((HistoryTable) allianceYearly1).id = alliance.getID();
      ((AllianceTable) allianceYearly1).population = new long?((long) alliance.countPopulation());
      ((AllianceTable) allianceYearly1).adults = new long?((long) alliance.countAdults());
      ((AllianceTable) allianceYearly1).children = new long?((long) alliance.countChildren());
      ((AllianceTable) allianceYearly1).army = new long?((long) alliance.countWarriors());
      ((AllianceTable) allianceYearly1).sick = new long?((long) alliance.countSick());
      ((AllianceTable) allianceYearly1).hungry = new long?((long) alliance.countHungry());
      ((AllianceTable) allianceYearly1).starving = new long?((long) alliance.countStarving());
      ((AllianceTable) allianceYearly1).happy = new long?((long) alliance.countHappyUnits());
      ((AllianceTable) allianceYearly1).deaths = new long?(alliance.getTotalDeaths());
      ((AllianceTable) allianceYearly1).kills = new long?(alliance.getTotalKills());
      ((AllianceTable) allianceYearly1).births = new long?(alliance.getTotalBirths());
      ((AllianceTable) allianceYearly1).territory = new long?((long) alliance.countZones());
      ((AllianceTable) allianceYearly1).buildings = new long?((long) alliance.countBuildings());
      ((AllianceTable) allianceYearly1).homeless = new long?((long) alliance.countHomeless());
      ((AllianceTable) allianceYearly1).housed = new long?((long) alliance.countHoused());
      ((AllianceTable) allianceYearly1).families = new long?((long) alliance.countFamilies());
      ((AllianceTable) allianceYearly1).males = new long?((long) alliance.countMales());
      ((AllianceTable) allianceYearly1).females = new long?((long) alliance.countFemales());
      ((AllianceTable) allianceYearly1).kingdoms = new long?((long) alliance.countKingdoms());
      ((AllianceTable) allianceYearly1).cities = new long?((long) alliance.countCities());
      ((AllianceTable) allianceYearly1).renown = new long?((long) alliance.getRenown());
      ((AllianceTable) allianceYearly1).money = new long?((long) alliance.countTotalMoney());
      return (HistoryTable) allianceYearly1;
    });
    HistoryMetaDataAsset pAsset3 = new HistoryMetaDataAsset();
    pAsset3.id = "clan";
    pAsset3.meta_type = MetaType.Clan;
    pAsset3.table_type = typeof (ClanTable);
    pAsset3.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (ClanYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (ClanYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (ClanYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (ClanYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (ClanYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (ClanYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (ClanYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (ClanYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (ClanYearly10000)
      }
    };
    this.add(pAsset3);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Clan clan = (Clan) pNanoObject;
      ClanYearly1 clanYearly1 = new ClanYearly1();
      ((HistoryTable) clanYearly1).id = clan.getID();
      ((ClanTable) clanYearly1).population = new long?((long) clan.countUnits());
      ((ClanTable) clanYearly1).adults = new long?((long) clan.countAdults());
      ((ClanTable) clanYearly1).children = new long?((long) clan.countChildren());
      ((ClanTable) clanYearly1).births = new long?(clan.getTotalBirths());
      ((ClanTable) clanYearly1).deaths = new long?(clan.getTotalDeaths());
      ((ClanTable) clanYearly1).kills = new long?(clan.getTotalKills());
      ((ClanTable) clanYearly1).kings = new long?((long) clan.countKings());
      ((ClanTable) clanYearly1).leaders = new long?((long) clan.countLeaders());
      ((ClanTable) clanYearly1).renown = new long?((long) clan.getRenown());
      ((ClanTable) clanYearly1).money = new long?((long) clan.countTotalMoney());
      ((ClanTable) clanYearly1).deaths_eaten = new long?(clan.getDeaths(AttackType.Eaten));
      ((ClanTable) clanYearly1).deaths_hunger = new long?(clan.getDeaths(AttackType.Starvation));
      ((ClanTable) clanYearly1).deaths_natural = new long?(clan.getDeaths(AttackType.Age));
      ((ClanTable) clanYearly1).deaths_plague = new long?(clan.getDeaths(AttackType.Plague));
      ((ClanTable) clanYearly1).deaths_poison = new long?(clan.getDeaths(AttackType.Poison));
      ((ClanTable) clanYearly1).deaths_infection = new long?(clan.getDeaths(AttackType.Infection));
      ((ClanTable) clanYearly1).deaths_tumor = new long?(clan.getDeaths(AttackType.Tumor));
      ((ClanTable) clanYearly1).deaths_acid = new long?(clan.getDeaths(AttackType.Acid));
      ((ClanTable) clanYearly1).deaths_fire = new long?(clan.getDeaths(AttackType.Fire));
      ((ClanTable) clanYearly1).deaths_divine = new long?(clan.getDeaths(AttackType.Divine));
      ((ClanTable) clanYearly1).deaths_weapon = new long?(clan.getDeaths(AttackType.Weapon));
      ((ClanTable) clanYearly1).deaths_gravity = new long?(clan.getDeaths(AttackType.Gravity));
      ((ClanTable) clanYearly1).deaths_drowning = new long?(clan.getDeaths(AttackType.Drowning));
      ((ClanTable) clanYearly1).deaths_water = new long?(clan.getDeaths(AttackType.Water));
      ((ClanTable) clanYearly1).deaths_explosion = new long?(clan.getDeaths(AttackType.Explosion));
      ((ClanTable) clanYearly1).deaths_other = new long?(clan.getDeaths(AttackType.Other));
      ((ClanTable) clanYearly1).metamorphosis = new long?(clan.getDeaths(AttackType.Metamorphosis));
      ((ClanTable) clanYearly1).evolutions = new long?(clan.getEvolutions());
      return (HistoryTable) clanYearly1;
    });
    HistoryMetaDataAsset pAsset4 = new HistoryMetaDataAsset();
    pAsset4.id = "city";
    pAsset4.meta_type = MetaType.City;
    pAsset4.table_type = typeof (CityTable);
    pAsset4.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (CityYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (CityYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (CityYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (CityYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (CityYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (CityYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (CityYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (CityYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (CityYearly10000)
      }
    };
    this.add(pAsset4);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      City city = (City) pNanoObject;
      CityYearly1 cityYearly1 = new CityYearly1();
      ((HistoryTable) cityYearly1).id = city.getID();
      ((CityTable) cityYearly1).population = new long?((long) city.countUnits());
      ((CityTable) cityYearly1).adults = new long?((long) city.countAdults());
      ((CityTable) cityYearly1).children = new long?((long) city.countChildren());
      ((CityTable) cityYearly1).boats = new long?((long) city.countBoats());
      ((CityTable) cityYearly1).army = new long?((long) city.countWarriors());
      ((CityTable) cityYearly1).families = new long?((long) city.countFamilies());
      ((CityTable) cityYearly1).males = new long?((long) city.countMales());
      ((CityTable) cityYearly1).females = new long?((long) city.countFemales());
      ((CityTable) cityYearly1).sick = new long?((long) city.countSick());
      ((CityTable) cityYearly1).loyalty = new long?((long) city.getCachedLoyalty());
      ((CityTable) cityYearly1).hungry = new long?((long) city.countHungry());
      ((CityTable) cityYearly1).starving = new long?((long) city.countStarving());
      ((CityTable) cityYearly1).happy = new long?((long) city.countHappyUnits());
      ((CityTable) cityYearly1).deaths = new long?(city.getTotalDeaths());
      ((CityTable) cityYearly1).births = new long?(city.getTotalBirths());
      ((CityTable) cityYearly1).joined = new long?(city.getTotalJoined());
      ((CityTable) cityYearly1).left = new long?(city.getTotalLeft());
      ((CityTable) cityYearly1).moved = new long?(city.getTotalMoved());
      ((CityTable) cityYearly1).migrated = new long?(city.getTotalMigrated());
      ((CityTable) cityYearly1).territory = new long?((long) city.countZones());
      ((CityTable) cityYearly1).buildings = new long?((long) city.countBuildings());
      ((CityTable) cityYearly1).homeless = new long?((long) city.countHomeless());
      ((CityTable) cityYearly1).housed = new long?((long) city.countHoused());
      ((CityTable) cityYearly1).renown = new long?((long) city.getRenown());
      ((CityTable) cityYearly1).money = new long?((long) city.countTotalMoney());
      ((CityTable) cityYearly1).food = new long?((long) city.getTotalFood());
      ((CityTable) cityYearly1).gold = new long?((long) city.getResourcesAmount("gold"));
      ((CityTable) cityYearly1).wood = new long?((long) city.getResourcesAmount("wood"));
      ((CityTable) cityYearly1).stone = new long?((long) city.getResourcesAmount("stone"));
      ((CityTable) cityYearly1).common_metals = new long?((long) city.getResourcesAmount("common_metals"));
      ((CityTable) cityYearly1).items = new long?((long) city.data.equipment.countItems());
      ((CityTable) cityYearly1).deaths_eaten = new long?(city.getDeaths(AttackType.Eaten));
      ((CityTable) cityYearly1).deaths_hunger = new long?(city.getDeaths(AttackType.Starvation));
      ((CityTable) cityYearly1).deaths_natural = new long?(city.getDeaths(AttackType.Age));
      ((CityTable) cityYearly1).deaths_plague = new long?(city.getDeaths(AttackType.Plague));
      ((CityTable) cityYearly1).deaths_poison = new long?(city.getDeaths(AttackType.Poison));
      ((CityTable) cityYearly1).deaths_infection = new long?(city.getDeaths(AttackType.Infection));
      ((CityTable) cityYearly1).deaths_tumor = new long?(city.getDeaths(AttackType.Tumor));
      ((CityTable) cityYearly1).deaths_acid = new long?(city.getDeaths(AttackType.Acid));
      ((CityTable) cityYearly1).deaths_fire = new long?(city.getDeaths(AttackType.Fire));
      ((CityTable) cityYearly1).deaths_divine = new long?(city.getDeaths(AttackType.Divine));
      ((CityTable) cityYearly1).deaths_weapon = new long?(city.getDeaths(AttackType.Weapon));
      ((CityTable) cityYearly1).deaths_gravity = new long?(city.getDeaths(AttackType.Gravity));
      ((CityTable) cityYearly1).deaths_drowning = new long?(city.getDeaths(AttackType.Drowning));
      ((CityTable) cityYearly1).deaths_water = new long?(city.getDeaths(AttackType.Water));
      ((CityTable) cityYearly1).deaths_explosion = new long?(city.getDeaths(AttackType.Explosion));
      ((CityTable) cityYearly1).deaths_other = new long?(city.getDeaths(AttackType.Other));
      ((CityTable) cityYearly1).metamorphosis = new long?(city.getDeaths(AttackType.Metamorphosis));
      ((CityTable) cityYearly1).evolutions = new long?(city.getEvolutions());
      return (HistoryTable) cityYearly1;
    });
    HistoryMetaDataAsset pAsset5 = new HistoryMetaDataAsset();
    pAsset5.id = "culture";
    pAsset5.meta_type = MetaType.Culture;
    pAsset5.table_type = typeof (CultureTable);
    pAsset5.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (CultureYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (CultureYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (CultureYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (CultureYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (CultureYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (CultureYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (CultureYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (CultureYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (CultureYearly10000)
      }
    };
    this.add(pAsset5);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Culture culture = (Culture) pNanoObject;
      CultureYearly1 cultureYearly1 = new CultureYearly1();
      ((HistoryTable) cultureYearly1).id = culture.getID();
      ((CultureTable) cultureYearly1).population = new long?((long) culture.countUnits());
      ((CultureTable) cultureYearly1).cities = new long?((long) culture.countCities());
      ((CultureTable) cultureYearly1).kingdoms = new long?((long) culture.countKingdoms());
      ((CultureTable) cultureYearly1).births = new long?(culture.getTotalBirths());
      ((CultureTable) cultureYearly1).deaths = new long?(culture.getTotalDeaths());
      ((CultureTable) cultureYearly1).kills = new long?(culture.getTotalKills());
      ((CultureTable) cultureYearly1).adults = new long?((long) culture.countAdults());
      ((CultureTable) cultureYearly1).children = new long?((long) culture.countChildren());
      ((CultureTable) cultureYearly1).kings = new long?((long) culture.countKings());
      ((CultureTable) cultureYearly1).leaders = new long?((long) culture.countLeaders());
      ((CultureTable) cultureYearly1).renown = new long?((long) culture.getRenown());
      ((CultureTable) cultureYearly1).money = new long?((long) culture.countTotalMoney());
      return (HistoryTable) cultureYearly1;
    });
    HistoryMetaDataAsset pAsset6 = new HistoryMetaDataAsset();
    pAsset6.id = "family";
    pAsset6.meta_type = MetaType.Family;
    pAsset6.table_type = typeof (FamilyTable);
    pAsset6.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (FamilyYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (FamilyYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (FamilyYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (FamilyYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (FamilyYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (FamilyYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (FamilyYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (FamilyYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (FamilyYearly10000)
      }
    };
    this.add(pAsset6);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Family family = (Family) pNanoObject;
      FamilyYearly1 familyYearly1 = new FamilyYearly1();
      ((HistoryTable) familyYearly1).id = family.getID();
      ((FamilyTable) familyYearly1).population = new long?((long) family.countUnits());
      ((FamilyTable) familyYearly1).adults = new long?((long) family.countAdults());
      ((FamilyTable) familyYearly1).children = new long?((long) family.countChildren());
      ((FamilyTable) familyYearly1).births = new long?(family.getTotalBirths());
      ((FamilyTable) familyYearly1).deaths = new long?(family.getTotalDeaths());
      ((FamilyTable) familyYearly1).kills = new long?(family.getTotalKills());
      ((FamilyTable) familyYearly1).money = new long?((long) family.countTotalMoney());
      return (HistoryTable) familyYearly1;
    });
    HistoryMetaDataAsset pAsset7 = new HistoryMetaDataAsset();
    pAsset7.id = "army";
    pAsset7.meta_type = MetaType.Army;
    pAsset7.table_type = typeof (ArmyTable);
    pAsset7.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (ArmyYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (ArmyYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (ArmyYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (ArmyYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (ArmyYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (ArmyYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (ArmyYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (ArmyYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (ArmyYearly10000)
      }
    };
    this.add(pAsset7);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Army army = (Army) pNanoObject;
      ArmyYearly1 armyYearly1 = new ArmyYearly1();
      ((HistoryTable) armyYearly1).id = army.getID();
      ((ArmyTable) armyYearly1).population = new long?((long) army.countUnits());
      ((ArmyTable) armyYearly1).deaths = new long?(army.getTotalDeaths());
      ((ArmyTable) armyYearly1).kills = new long?(army.getTotalKills());
      return (HistoryTable) armyYearly1;
    });
    HistoryMetaDataAsset pAsset8 = new HistoryMetaDataAsset();
    pAsset8.id = "kingdom";
    pAsset8.meta_type = MetaType.Kingdom;
    pAsset8.table_type = typeof (KingdomTable);
    pAsset8.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (KingdomYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (KingdomYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (KingdomYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (KingdomYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (KingdomYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (KingdomYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (KingdomYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (KingdomYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (KingdomYearly10000)
      }
    };
    this.add(pAsset8);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Kingdom kingdom = (Kingdom) pNanoObject;
      KingdomYearly1 kingdomYearly1 = new KingdomYearly1();
      ((HistoryTable) kingdomYearly1).id = kingdom.getID();
      ((KingdomTable) kingdomYearly1).population = new long?((long) kingdom.countUnits());
      ((KingdomTable) kingdomYearly1).adults = new long?((long) kingdom.countAdults());
      ((KingdomTable) kingdomYearly1).children = new long?((long) kingdom.countChildren());
      ((KingdomTable) kingdomYearly1).boats = new long?((long) kingdom.countBoats());
      ((KingdomTable) kingdomYearly1).army = new long?((long) kingdom.countTotalWarriors());
      ((KingdomTable) kingdomYearly1).sick = new long?((long) kingdom.countSick());
      ((KingdomTable) kingdomYearly1).hungry = new long?((long) kingdom.countHungry());
      ((KingdomTable) kingdomYearly1).starving = new long?((long) kingdom.countStarving());
      ((KingdomTable) kingdomYearly1).happy = new long?((long) kingdom.countHappyUnits());
      ((KingdomTable) kingdomYearly1).deaths = new long?(kingdom.getTotalDeaths());
      ((KingdomTable) kingdomYearly1).births = new long?(kingdom.getTotalBirths());
      ((KingdomTable) kingdomYearly1).kills = new long?(kingdom.getTotalKills());
      ((KingdomTable) kingdomYearly1).joined = new long?(kingdom.getTotalJoined());
      ((KingdomTable) kingdomYearly1).left = new long?(kingdom.getTotalLeft());
      ((KingdomTable) kingdomYearly1).moved = new long?(kingdom.getTotalMoved());
      ((KingdomTable) kingdomYearly1).migrated = new long?(kingdom.getTotalMigrated());
      ((KingdomTable) kingdomYearly1).territory = new long?((long) kingdom.countZones());
      ((KingdomTable) kingdomYearly1).buildings = new long?((long) kingdom.countBuildings());
      ((KingdomTable) kingdomYearly1).homeless = new long?((long) kingdom.countHomeless());
      ((KingdomTable) kingdomYearly1).housed = new long?((long) kingdom.countHoused());
      ((KingdomTable) kingdomYearly1).food = new long?((long) kingdom.countTotalFood());
      ((KingdomTable) kingdomYearly1).families = new long?((long) kingdom.countFamilies());
      ((KingdomTable) kingdomYearly1).males = new long?((long) kingdom.countMales());
      ((KingdomTable) kingdomYearly1).females = new long?((long) kingdom.countFemales());
      ((KingdomTable) kingdomYearly1).cities = new long?((long) kingdom.countCities());
      ((KingdomTable) kingdomYearly1).renown = new long?((long) kingdom.getRenown());
      ((KingdomTable) kingdomYearly1).money = new long?((long) kingdom.countTotalMoney());
      ((KingdomTable) kingdomYearly1).deaths_eaten = new long?(kingdom.getDeaths(AttackType.Eaten));
      ((KingdomTable) kingdomYearly1).deaths_hunger = new long?(kingdom.getDeaths(AttackType.Starvation));
      ((KingdomTable) kingdomYearly1).deaths_natural = new long?(kingdom.getDeaths(AttackType.Age));
      ((KingdomTable) kingdomYearly1).deaths_plague = new long?(kingdom.getDeaths(AttackType.Plague));
      ((KingdomTable) kingdomYearly1).deaths_poison = new long?(kingdom.getDeaths(AttackType.Poison));
      ((KingdomTable) kingdomYearly1).deaths_infection = new long?(kingdom.getDeaths(AttackType.Infection));
      ((KingdomTable) kingdomYearly1).deaths_tumor = new long?(kingdom.getDeaths(AttackType.Tumor));
      ((KingdomTable) kingdomYearly1).deaths_acid = new long?(kingdom.getDeaths(AttackType.Acid));
      ((KingdomTable) kingdomYearly1).deaths_fire = new long?(kingdom.getDeaths(AttackType.Fire));
      ((KingdomTable) kingdomYearly1).deaths_divine = new long?(kingdom.getDeaths(AttackType.Divine));
      ((KingdomTable) kingdomYearly1).deaths_weapon = new long?(kingdom.getDeaths(AttackType.Weapon));
      ((KingdomTable) kingdomYearly1).deaths_gravity = new long?(kingdom.getDeaths(AttackType.Gravity));
      ((KingdomTable) kingdomYearly1).deaths_drowning = new long?(kingdom.getDeaths(AttackType.Drowning));
      ((KingdomTable) kingdomYearly1).deaths_water = new long?(kingdom.getDeaths(AttackType.Water));
      ((KingdomTable) kingdomYearly1).deaths_explosion = new long?(kingdom.getDeaths(AttackType.Explosion));
      ((KingdomTable) kingdomYearly1).deaths_other = new long?(kingdom.getDeaths(AttackType.Other));
      ((KingdomTable) kingdomYearly1).metamorphosis = new long?(kingdom.getDeaths(AttackType.Metamorphosis));
      ((KingdomTable) kingdomYearly1).evolutions = new long?(kingdom.getEvolutions());
      return (HistoryTable) kingdomYearly1;
    });
    HistoryMetaDataAsset pAsset9 = new HistoryMetaDataAsset();
    pAsset9.id = "language";
    pAsset9.meta_type = MetaType.Language;
    pAsset9.table_type = typeof (LanguageTable);
    pAsset9.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (LanguageYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (LanguageYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (LanguageYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (LanguageYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (LanguageYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (LanguageYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (LanguageYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (LanguageYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (LanguageYearly10000)
      }
    };
    this.add(pAsset9);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Language language = (Language) pNanoObject;
      LanguageYearly1 languageYearly1 = new LanguageYearly1();
      ((HistoryTable) languageYearly1).id = language.getID();
      ((LanguageTable) languageYearly1).population = new long?((long) language.countUnits());
      ((LanguageTable) languageYearly1).adults = new long?((long) language.countAdults());
      ((LanguageTable) languageYearly1).children = new long?((long) language.countChildren());
      ((LanguageTable) languageYearly1).kingdoms = new long?((long) language.countKingdoms());
      ((LanguageTable) languageYearly1).cities = new long?((long) language.countCities());
      ((LanguageTable) languageYearly1).books = new long?((long) language.books.count());
      ((LanguageTable) languageYearly1).books_written = new long?((long) language.countWrittenBooks());
      ((LanguageTable) languageYearly1).speakers_new = new long?((long) language.getSpeakersNew());
      ((LanguageTable) languageYearly1).speakers_lost = new long?((long) language.getSpeakersLost());
      ((LanguageTable) languageYearly1).speakers_converted = new long?((long) language.getSpeakersConverted());
      ((LanguageTable) languageYearly1).deaths = new long?(language.getTotalDeaths());
      ((LanguageTable) languageYearly1).kills = new long?(language.getTotalKills());
      ((LanguageTable) languageYearly1).renown = new long?((long) language.getRenown());
      ((LanguageTable) languageYearly1).money = new long?((long) language.countTotalMoney());
      return (HistoryTable) languageYearly1;
    });
    HistoryMetaDataAsset pAsset10 = new HistoryMetaDataAsset();
    pAsset10.id = "religion";
    pAsset10.meta_type = MetaType.Religion;
    pAsset10.table_type = typeof (ReligionTable);
    pAsset10.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (ReligionYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (ReligionYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (ReligionYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (ReligionYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (ReligionYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (ReligionYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (ReligionYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (ReligionYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (ReligionYearly10000)
      }
    };
    this.add(pAsset10);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Religion religion = (Religion) pNanoObject;
      ReligionYearly1 religionYearly1 = new ReligionYearly1();
      ((HistoryTable) religionYearly1).id = religion.getID();
      ((ReligionTable) religionYearly1).population = new long?((long) religion.countUnits());
      ((ReligionTable) religionYearly1).kingdoms = new long?((long) religion.countKingdoms());
      ((ReligionTable) religionYearly1).cities = new long?((long) religion.countCities());
      ((ReligionTable) religionYearly1).sick = new long?((long) religion.countSick());
      ((ReligionTable) religionYearly1).happy = new long?((long) religion.countHappyUnits());
      ((ReligionTable) religionYearly1).hungry = new long?((long) religion.countHungry());
      ((ReligionTable) religionYearly1).starving = new long?((long) religion.countStarving());
      ((ReligionTable) religionYearly1).deaths = new long?(religion.getTotalDeaths());
      ((ReligionTable) religionYearly1).births = new long?(religion.getTotalBirths());
      ((ReligionTable) religionYearly1).kills = new long?(religion.getTotalKills());
      ((ReligionTable) religionYearly1).adults = new long?((long) religion.countAdults());
      ((ReligionTable) religionYearly1).children = new long?((long) religion.countChildren());
      ((ReligionTable) religionYearly1).males = new long?((long) religion.countMales());
      ((ReligionTable) religionYearly1).females = new long?((long) religion.countFemales());
      ((ReligionTable) religionYearly1).homeless = new long?((long) religion.countHomeless());
      ((ReligionTable) religionYearly1).housed = new long?((long) religion.countHoused());
      ((ReligionTable) religionYearly1).kings = new long?((long) religion.countKings());
      ((ReligionTable) religionYearly1).leaders = new long?((long) religion.countLeaders());
      ((ReligionTable) religionYearly1).renown = new long?((long) religion.getRenown());
      ((ReligionTable) religionYearly1).money = new long?((long) religion.countTotalMoney());
      ((ReligionTable) religionYearly1).evolutions = new long?(religion.getEvolutions());
      return (HistoryTable) religionYearly1;
    });
    HistoryMetaDataAsset pAsset11 = new HistoryMetaDataAsset();
    pAsset11.id = "subspecies";
    pAsset11.meta_type = MetaType.Subspecies;
    pAsset11.table_type = typeof (SubspeciesTable);
    pAsset11.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (SubspeciesYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (SubspeciesYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (SubspeciesYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (SubspeciesYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (SubspeciesYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (SubspeciesYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (SubspeciesYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (SubspeciesYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (SubspeciesYearly10000)
      }
    };
    this.add(pAsset11);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      Subspecies subspecies = (Subspecies) pNanoObject;
      SubspeciesYearly1 subspeciesYearly1 = new SubspeciesYearly1();
      ((HistoryTable) subspeciesYearly1).id = subspecies.getID();
      ((SubspeciesTable) subspeciesYearly1).population = new long?((long) subspecies.countUnits());
      ((SubspeciesTable) subspeciesYearly1).adults = new long?((long) subspecies.countAdults());
      ((SubspeciesTable) subspeciesYearly1).children = new long?((long) subspecies.countChildren());
      ((SubspeciesTable) subspeciesYearly1).deaths = new long?(subspecies.getTotalDeaths());
      ((SubspeciesTable) subspeciesYearly1).births = new long?(subspecies.getTotalBirths());
      ((SubspeciesTable) subspeciesYearly1).kills = new long?(subspecies.getTotalKills());
      ((SubspeciesTable) subspeciesYearly1).renown = new long?((long) subspecies.getRenown());
      ((SubspeciesTable) subspeciesYearly1).money = new long?((long) subspecies.countTotalMoney());
      ((SubspeciesTable) subspeciesYearly1).deaths_eaten = new long?(subspecies.getDeaths(AttackType.Eaten));
      ((SubspeciesTable) subspeciesYearly1).deaths_hunger = new long?(subspecies.getDeaths(AttackType.Starvation));
      ((SubspeciesTable) subspeciesYearly1).deaths_natural = new long?(subspecies.getDeaths(AttackType.Age));
      ((SubspeciesTable) subspeciesYearly1).deaths_plague = new long?(subspecies.getDeaths(AttackType.Plague));
      ((SubspeciesTable) subspeciesYearly1).deaths_poison = new long?(subspecies.getDeaths(AttackType.Poison));
      ((SubspeciesTable) subspeciesYearly1).deaths_infection = new long?(subspecies.getDeaths(AttackType.Infection));
      ((SubspeciesTable) subspeciesYearly1).deaths_tumor = new long?(subspecies.getDeaths(AttackType.Tumor));
      ((SubspeciesTable) subspeciesYearly1).deaths_acid = new long?(subspecies.getDeaths(AttackType.Acid));
      ((SubspeciesTable) subspeciesYearly1).deaths_fire = new long?(subspecies.getDeaths(AttackType.Fire));
      ((SubspeciesTable) subspeciesYearly1).deaths_divine = new long?(subspecies.getDeaths(AttackType.Divine));
      ((SubspeciesTable) subspeciesYearly1).deaths_weapon = new long?(subspecies.getDeaths(AttackType.Weapon));
      ((SubspeciesTable) subspeciesYearly1).deaths_gravity = new long?(subspecies.getDeaths(AttackType.Gravity));
      ((SubspeciesTable) subspeciesYearly1).deaths_drowning = new long?(subspecies.getDeaths(AttackType.Drowning));
      ((SubspeciesTable) subspeciesYearly1).deaths_water = new long?(subspecies.getDeaths(AttackType.Water));
      ((SubspeciesTable) subspeciesYearly1).deaths_explosion = new long?(subspecies.getDeaths(AttackType.Explosion));
      ((SubspeciesTable) subspeciesYearly1).deaths_other = new long?(subspecies.getDeaths(AttackType.Other));
      ((SubspeciesTable) subspeciesYearly1).metamorphosis = new long?(subspecies.getDeaths(AttackType.Metamorphosis));
      ((SubspeciesTable) subspeciesYearly1).evolutions = new long?(subspecies.getEvolutions());
      return (HistoryTable) subspeciesYearly1;
    });
    HistoryMetaDataAsset pAsset12 = new HistoryMetaDataAsset();
    pAsset12.id = "war";
    pAsset12.meta_type = MetaType.War;
    pAsset12.table_type = typeof (WarTable);
    pAsset12.table_types = new Dictionary<HistoryInterval, Type>()
    {
      {
        HistoryInterval.Yearly1,
        typeof (WarYearly1)
      },
      {
        HistoryInterval.Yearly5,
        typeof (WarYearly5)
      },
      {
        HistoryInterval.Yearly10,
        typeof (WarYearly10)
      },
      {
        HistoryInterval.Yearly50,
        typeof (WarYearly50)
      },
      {
        HistoryInterval.Yearly100,
        typeof (WarYearly100)
      },
      {
        HistoryInterval.Yearly500,
        typeof (WarYearly500)
      },
      {
        HistoryInterval.Yearly1000,
        typeof (WarYearly1000)
      },
      {
        HistoryInterval.Yearly5000,
        typeof (WarYearly5000)
      },
      {
        HistoryInterval.Yearly10000,
        typeof (WarYearly10000)
      }
    };
    this.add(pAsset12);
    this.t.collector = (HistoryDataCollector) (pNanoObject =>
    {
      War war = (War) pNanoObject;
      WarYearly1 warYearly1 = new WarYearly1();
      ((HistoryTable) warYearly1).id = war.getID();
      ((WarTable) warYearly1).population = new long?((long) war.countTotalPopulation());
      ((WarTable) warYearly1).army = new long?((long) war.countTotalArmy());
      ((WarTable) warYearly1).renown = new long?((long) war.getRenown());
      ((WarTable) warYearly1).kingdoms = new long?((long) war.countKingdoms());
      ((WarTable) warYearly1).cities = new long?((long) war.countCities());
      ((WarTable) warYearly1).deaths = new long?(war.getTotalDeaths());
      ((WarTable) warYearly1).population_attackers = new long?((long) war.countAttackersPopulation());
      ((WarTable) warYearly1).population_defenders = new long?((long) war.countDefendersPopulation());
      ((WarTable) warYearly1).army_attackers = new long?((long) war.countAttackersWarriors());
      ((WarTable) warYearly1).army_defenders = new long?((long) war.countDefendersWarriors());
      ((WarTable) warYearly1).deaths_attackers = new long?((long) war.getDeadAttackers());
      ((WarTable) warYearly1).deaths_defenders = new long?((long) war.getDeadDefenders());
      ((WarTable) warYearly1).money_attackers = new long?((long) war.countAttackersMoney());
      ((WarTable) warYearly1).money_defenders = new long?((long) war.countDefendersMoney());
      return (HistoryTable) warYearly1;
    });
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    HashSet<Type> typeSet = new HashSet<Type>();
    foreach (HistoryMetaDataAsset historyMetaDataAsset in this.list)
    {
      Dictionary<HistoryInterval, Type> tableTypes = historyMetaDataAsset.table_types;
      foreach (HistoryInterval key in Enum.GetValues(typeof (HistoryInterval)))
      {
        if (key != HistoryInterval.None)
        {
          if (!tableTypes.ContainsKey(key))
            BaseAssetLibrary.logAssetError($"<e>HistoryMetaDataLibrary</e>: Missing a table type for <b>{key}</b>", historyMetaDataAsset.id);
          else if (!typeSet.Add(tableTypes[key]))
            BaseAssetLibrary.logAssetError($"<e>HistoryMetaDataLibrary</e>: Duplicate table type <b>{tableTypes[key]}</b> for <b>{key}</b>", historyMetaDataAsset.id);
        }
      }
    }
  }

  public HistoryMetaDataAsset[] getAssets(MetaType pMetaType)
  {
    return HistoryMetaDataLibrary._meta_data[pMetaType];
  }

  public HistoryMetaDataAsset[] getAssets(string pMetaType)
  {
    return HistoryMetaDataLibrary._meta_dict[pMetaType];
  }

  public override void linkAssets()
  {
    base.linkAssets();
    Dictionary<MetaType, ListPool<HistoryMetaDataAsset>> dictionary = new Dictionary<MetaType, ListPool<HistoryMetaDataAsset>>();
    foreach (HistoryMetaDataAsset historyMetaDataAsset in this.list)
    {
      TypeInfo typeInfo = historyMetaDataAsset.table_type.GetTypeInfo();
      List<string> stringList = new List<string>();
      foreach (PropertyInfo declaredProperty in typeInfo.DeclaredProperties)
      {
        if (declaredProperty.CanRead && declaredProperty.CanWrite && declaredProperty.GetMethod != (MethodInfo) null && declaredProperty.SetMethod != (MethodInfo) null && declaredProperty.GetMethod.IsPublic && declaredProperty.SetMethod.IsPublic && !declaredProperty.GetMethod.IsStatic && !declaredProperty.SetMethod.IsStatic)
          stringList.Add(declaredProperty.Name);
      }
      foreach (string pID in stringList)
      {
        HistoryDataAsset historyDataAsset = AssetManager.history_data_library.get(pID);
        historyMetaDataAsset.categories.Add(historyDataAsset);
      }
      if (!dictionary.ContainsKey(historyMetaDataAsset.meta_type))
        dictionary.Add(historyMetaDataAsset.meta_type, new ListPool<HistoryMetaDataAsset>());
      dictionary[historyMetaDataAsset.meta_type].Add(historyMetaDataAsset);
    }
    foreach (KeyValuePair<MetaType, ListPool<HistoryMetaDataAsset>> keyValuePair in dictionary)
    {
      MetaType metaType1;
      ListPool<HistoryMetaDataAsset> listPool;
      keyValuePair.Deconstruct(ref metaType1, ref listPool);
      MetaType metaType2 = metaType1;
      ListPool<HistoryMetaDataAsset> list = listPool;
      HistoryMetaDataLibrary._meta_data.Add(metaType2, list.ToArray<HistoryMetaDataAsset>());
      HistoryMetaDataLibrary._meta_dict.Add(metaType2.AsString(), HistoryMetaDataLibrary._meta_data[metaType2]);
    }
  }
}
