// Decompiled with JetBrains decompiler
// Type: SaveConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class SaveConverter
{
  private static Dictionary<string, string[]> _buildings_old_id_dictionary;
  private static long _kingdom_0;

  public static void convert(SavedMap pData)
  {
    if (pData.saveVersion == 15)
      throw new Exception("saveVersion 15 is not supported");
    if (pData.saveVersion < 12)
      SaveConverter.convertOldAges(pData);
    if (pData.saveVersion < 15)
      SaveConverter.checkOldBuildingID(pData);
    if (pData.saveVersion <= 15)
      SaveConverter.convertTo15(pData);
    if (pData.saveVersion <= 16 /*0x10*/)
      SaveConverter.convertTo16(pData);
    if (pData.saveVersion > 17)
      return;
    SaveConverter.convertTo17(pData);
  }

  public static long kingdomIDFixer(SavedMap pData, long pKingdomID)
  {
    if (pKingdomID != 0L)
      return pKingdomID;
    if (SaveConverter._kingdom_0 == 0L)
      SaveConverter._kingdom_0 = pData.mapStats.id_kingdom++;
    Debug.LogWarning((object) ("found kingdom with id 0, changing to " + SaveConverter._kingdom_0.ToString()));
    return SaveConverter._kingdom_0;
  }

  public static string assetIDFixer(string pAssetID)
  {
    if (pAssetID.StartsWith("unit_"))
      pAssetID = pAssetID.Replace("unit_", "");
    if (pAssetID.StartsWith("baby_"))
      pAssetID = pAssetID.Replace("baby_", "");
    if (pAssetID == "chick")
      pAssetID = "chicken";
    if (pAssetID == "skeleton_cursed")
      pAssetID = "skeleton";
    if (pAssetID == "whiteMage")
      pAssetID = "white_mage";
    if (pAssetID == "evilMage")
      pAssetID = "evil_mage";
    if (pAssetID == "godFinger")
      pAssetID = "god_finger";
    if (pAssetID == "livingPlants")
      pAssetID = "living_plants";
    if (pAssetID == "livingHouse")
      pAssetID = "living_house";
    if (pAssetID == "walker")
      pAssetID = "cold_one";
    if (pAssetID == "lemon_man")
      pAssetID = "lemon_snail";
    if (pAssetID == "lemon_boi")
      pAssetID = "lemon_snail";
    if (pAssetID == "enchanted_fairy")
      pAssetID = "fairy";
    if (pAssetID == "crystal_golem")
      pAssetID = "crystal_sword";
    return pAssetID;
  }

  public static void checkMaxValues(SavedMap pData)
  {
    if (pData.mapStats == null)
      return;
    if (pData.mapStats.id_unit <= 1L && pData.actors_data != null)
    {
      bool flag = false;
      foreach (ActorData actorData in pData.actors_data)
      {
        if (actorData.id >= pData.mapStats.id_unit)
        {
          pData.mapStats.id_unit = actorData.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_unit to " + pData.mapStats.id_unit.ToString()));
    }
    if (pData.mapStats.id_building <= 1L && pData.buildings != null)
    {
      bool flag = false;
      foreach (BuildingData building in pData.buildings)
      {
        if (building.id >= pData.mapStats.id_building)
        {
          pData.mapStats.id_building = building.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_building to " + pData.mapStats.id_building.ToString()));
    }
    if (pData.mapStats.id_kingdom <= 1L && pData.kingdoms != null)
    {
      bool flag = false;
      foreach (KingdomData kingdom in pData.kingdoms)
      {
        if (kingdom.id >= pData.mapStats.id_kingdom)
        {
          pData.mapStats.id_kingdom = kingdom.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_kingdom to " + pData.mapStats.id_kingdom.ToString()));
    }
    if (pData.mapStats.id_city <= 1L && pData.cities != null)
    {
      bool flag = false;
      foreach (CityData city in pData.cities)
      {
        if (city.id >= pData.mapStats.id_city)
        {
          pData.mapStats.id_city = city.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_city to " + pData.mapStats.id_city.ToString()));
    }
    if (pData.mapStats.id_culture <= 1L && pData.cultures != null)
    {
      bool flag = false;
      foreach (CultureData culture in pData.cultures)
      {
        if (culture.id >= pData.mapStats.id_culture)
        {
          pData.mapStats.id_culture = culture.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_culture to " + pData.mapStats.id_culture.ToString()));
    }
    if (pData.mapStats.id_clan <= 1L && pData.clans != null)
    {
      bool flag = false;
      foreach (ClanData clan in pData.clans)
      {
        if (clan.id >= pData.mapStats.id_clan)
        {
          pData.mapStats.id_clan = clan.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_clan to " + pData.mapStats.id_clan.ToString()));
    }
    if (pData.mapStats.id_alliance <= 1L && pData.alliances != null)
    {
      bool flag = false;
      foreach (AllianceData alliance in pData.alliances)
      {
        if (alliance.id >= pData.mapStats.id_alliance)
        {
          pData.mapStats.id_alliance = alliance.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_alliance to " + pData.mapStats.id_alliance.ToString()));
    }
    if (pData.mapStats.id_war <= 1L && pData.wars != null)
    {
      bool flag = false;
      foreach (WarData war in pData.wars)
      {
        if (war.id >= pData.mapStats.id_war)
        {
          pData.mapStats.id_war = war.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_war to " + pData.mapStats.id_war.ToString()));
    }
    if (pData.mapStats.id_plot <= 1L && pData.plots != null)
    {
      bool flag = false;
      foreach (PlotData plot in pData.plots)
      {
        if (plot.id >= pData.mapStats.id_plot)
        {
          pData.mapStats.id_plot = plot.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_plot to " + pData.mapStats.id_plot.ToString()));
    }
    if (pData.mapStats.id_book <= 1L && pData.books != null)
    {
      bool flag = false;
      foreach (BookData book in pData.books)
      {
        if (book.id >= pData.mapStats.id_book)
        {
          pData.mapStats.id_book = book.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_book to " + pData.mapStats.id_book.ToString()));
    }
    if (pData.mapStats.id_subspecies <= 1L && pData.subspecies != null)
    {
      bool flag = false;
      foreach (SubspeciesData subspeciesData in pData.subspecies)
      {
        if (subspeciesData.id >= pData.mapStats.id_subspecies)
        {
          pData.mapStats.id_subspecies = subspeciesData.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_subspecies to " + pData.mapStats.id_subspecies.ToString()));
    }
    if (pData.mapStats.id_family <= 1L && pData.families != null)
    {
      bool flag = false;
      foreach (FamilyData family in pData.families)
      {
        if (family.id >= pData.mapStats.id_family)
        {
          pData.mapStats.id_family = family.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_family to " + pData.mapStats.id_family.ToString()));
    }
    if (pData.mapStats.id_army <= 1L && pData.armies != null)
    {
      bool flag = false;
      foreach (ArmyData army in pData.armies)
      {
        if (army.id >= pData.mapStats.id_army)
        {
          pData.mapStats.id_army = army.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_army to " + pData.mapStats.id_army.ToString()));
    }
    if (pData.mapStats.id_language <= 1L && pData.languages != null)
    {
      bool flag = false;
      foreach (LanguageData language in pData.languages)
      {
        if (language.id >= pData.mapStats.id_language)
        {
          pData.mapStats.id_language = language.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_language to " + pData.mapStats.id_language.ToString()));
    }
    if (pData.mapStats.id_religion <= 1L && pData.religions != null)
    {
      bool flag = false;
      foreach (ReligionData religion in pData.religions)
      {
        if (religion.id >= pData.mapStats.id_religion)
        {
          pData.mapStats.id_religion = religion.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_religion to " + pData.mapStats.id_religion.ToString()));
    }
    if (pData.mapStats.id_item <= 1L && pData.items != null)
    {
      bool flag = false;
      foreach (ItemData itemData in pData.items)
      {
        if (itemData.id >= pData.mapStats.id_item)
        {
          pData.mapStats.id_item = itemData.id + 1L;
          flag = true;
        }
      }
      if (flag)
        Debug.LogWarning((object) ("increased id_item to " + pData.mapStats.id_item.ToString()));
    }
    if (pData.mapStats.id_diplomacy > 1L || pData.relations == null)
      return;
    long num = pData.mapStats.id_diplomacy;
    foreach (DiplomacyRelationData relation in pData.relations)
    {
      if (relation.id < 100000000L && relation.id >= num)
        num = relation.id + 1L;
    }
    foreach (DiplomacyRelationData relation in pData.relations)
    {
      if (relation.id >= 100000000L)
        relation.id = num++;
    }
    pData.mapStats.id_diplomacy = num;
  }

  public static void convertTo17(SavedMap pData)
  {
    if (pData.subspecies == null)
      return;
    foreach (SubspeciesData subspeciesData in pData.subspecies)
    {
      for (int index = 0; index < subspeciesData.saved_traits.Count; ++index)
      {
        if (subspeciesData.saved_traits[index] == "water_creature")
          subspeciesData.saved_traits[index] = "aquatic";
        if (subspeciesData.saved_traits[index] == "aquatic_adaptation")
          subspeciesData.saved_traits[index] = "fins";
      }
    }
  }

  public static void convertTo16(SavedMap pData)
  {
    if (pData.buildings != null)
    {
      foreach (BuildingData building in pData.buildings)
      {
        building.asset_id = building.asset_id.Replace("mapple_plant", "maple_plant");
        building.asset_id = building.asset_id.Replace("mapple_tree", "maple_tree");
      }
    }
    if (pData.tileMap != null)
    {
      for (int index = 0; index < pData.tileMap.Count; ++index)
      {
        if (pData.tileMap[index].Contains("mapple_"))
          pData.tileMap[index] = pData.tileMap[index].Replace("mapple_", "maple_");
      }
    }
    if (pData.subspecies == null)
      return;
    foreach (SubspeciesData subspeciesData in pData.subspecies)
    {
      if (subspeciesData.biome_variant.Contains("biome_mapple"))
        subspeciesData.biome_variant = subspeciesData.biome_variant.Replace("biome_mapple", "biome_maple");
    }
  }

  public static void convertTo15(SavedMap pData)
  {
    SaveConverter._kingdom_0 = 0L;
    SaveConverter.checkMaxValues(pData);
    if (pData.kingdoms != null)
    {
      foreach (KingdomData kingdom in pData.kingdoms)
      {
        kingdom.id = SaveConverter.kingdomIDFixer(pData, kingdom.id);
        kingdom.original_actor_asset = SaveConverter.assetIDFixer(kingdom.original_actor_asset);
      }
    }
    if (pData.actors_data != null)
    {
      foreach (ActorData actorData in pData.actors_data)
      {
        actorData.asset_id = SaveConverter.assetIDFixer(actorData.asset_id);
        actorData.civ_kingdom_id = SaveConverter.kingdomIDFixer(pData, actorData.civ_kingdom_id);
        if (actorData.profession == UnitProfession.Baby)
          actorData.profession = UnitProfession.Nothing;
        if (actorData.saved_traits != null)
        {
          for (int index = 0; index < actorData.saved_traits.Count; ++index)
          {
            if (actorData.saved_traits[index] == "mushSpores")
              actorData.saved_traits[index] = "mush_spores";
            if (actorData.saved_traits[index] == "tumorInfection")
              actorData.saved_traits[index] = "tumor_infection";
          }
        }
      }
    }
    if (pData.cities != null)
    {
      foreach (CityData city in pData.cities)
      {
        city.kingdomID = SaveConverter.kingdomIDFixer(pData, city.kingdomID);
        city.original_actor_asset = SaveConverter.assetIDFixer(city.original_actor_asset);
      }
    }
    if (pData.books != null)
    {
      foreach (BookData book in pData.books)
        book.author_kingdom_id = SaveConverter.kingdomIDFixer(pData, book.author_kingdom_id);
    }
    if (pData.religions != null)
    {
      foreach (ReligionData religion in pData.religions)
        religion.creator_kingdom_id = SaveConverter.kingdomIDFixer(pData, religion.creator_kingdom_id);
    }
    if (pData.alliances != null)
    {
      foreach (AllianceData alliance in pData.alliances)
      {
        alliance.founder_kingdom_id = SaveConverter.kingdomIDFixer(pData, alliance.founder_kingdom_id);
        List<long> kingdoms = alliance.kingdoms;
        // ISSUE: explicit non-virtual call
        if ((kingdoms != null ? (__nonvirtual (kingdoms.Contains(0L)) ? 1 : 0) : 0) != 0)
          alliance.kingdoms[alliance.kingdoms.IndexOf(0L)] = SaveConverter.kingdomIDFixer(pData, 0L);
      }
    }
    if (pData.wars != null)
    {
      foreach (WarData war in pData.wars)
      {
        war.started_by_kingdom_id = SaveConverter.kingdomIDFixer(pData, war.started_by_kingdom_id);
        war.main_attacker = SaveConverter.kingdomIDFixer(pData, war.main_attacker);
        war.main_defender = SaveConverter.kingdomIDFixer(pData, war.main_defender);
        List<long> listAttackers = war.list_attackers;
        // ISSUE: explicit non-virtual call
        if ((listAttackers != null ? (__nonvirtual (listAttackers.Contains(0L)) ? 1 : 0) : 0) != 0)
          war.list_attackers[war.list_attackers.IndexOf(0L)] = SaveConverter.kingdomIDFixer(pData, 0L);
        List<long> listDefenders = war.list_defenders;
        // ISSUE: explicit non-virtual call
        if ((listDefenders != null ? (__nonvirtual (listDefenders.Contains(0L)) ? 1 : 0) : 0) != 0)
          war.list_defenders[war.list_defenders.IndexOf(0L)] = SaveConverter.kingdomIDFixer(pData, 0L);
      }
    }
    if (pData.clans != null)
    {
      foreach (ClanData clan in pData.clans)
      {
        clan.founder_kingdom_id = SaveConverter.kingdomIDFixer(pData, clan.founder_kingdom_id);
        clan.original_actor_asset = SaveConverter.assetIDFixer(clan.original_actor_asset);
      }
    }
    if (pData.cultures != null)
    {
      foreach (CultureData culture in pData.cultures)
      {
        culture.creator_kingdom_id = SaveConverter.kingdomIDFixer(pData, culture.creator_kingdom_id);
        culture.original_actor_asset = SaveConverter.assetIDFixer(culture.original_actor_asset);
      }
    }
    if (pData.families != null)
    {
      foreach (FamilyData family in pData.families)
      {
        family.species_id = SaveConverter.assetIDFixer(family.species_id);
        family.founder_kingdom_id = SaveConverter.kingdomIDFixer(pData, family.founder_kingdom_id);
      }
    }
    if (pData.subspecies != null)
    {
      foreach (SubspeciesData subspeciesData in pData.subspecies)
        subspeciesData.species_id = SaveConverter.assetIDFixer(subspeciesData.species_id);
    }
    if (pData.plots != null)
    {
      foreach (PlotData plot in pData.plots)
      {
        if (plot.plot_type_id == "stop_war")
          plot.plot_type_id = "attacker_stop_war";
      }
    }
    if (pData.relations == null)
      return;
    foreach (DiplomacyRelationData relation in pData.relations)
    {
      relation.kingdom1_id = SaveConverter.kingdomIDFixer(pData, relation.kingdom1_id);
      relation.kingdom2_id = SaveConverter.kingdomIDFixer(pData, relation.kingdom2_id);
    }
  }

  public static void convertOldAges(SavedMap data)
  {
    if (data.actors != null)
    {
      foreach (ActorDataObsolete actor in data.actors)
      {
        ActorData newActorData = SaveConverter.getNewActorData(actor);
        if (newActorData != null)
          data.actors_data.Add(newActorData);
      }
      foreach (ActorData actorData in data.actors_data)
      {
        if (data.saveVersion < 11 && actorData.created_time < 0.0)
          actorData.created_time = data.mapStats.world_time + actorData.created_time + (double) Randy.randomFloat(0.0f, 360f);
      }
      data.actors = (List<ActorDataObsolete>) null;
    }
    if (data.kingdoms != null)
    {
      foreach (KingdomData kingdom in data.kingdoms)
      {
        if (kingdom.created_time < 0.0)
          kingdom.created_time = data.mapStats.world_time + kingdom.created_time + (double) Randy.randomFloat(0.0f, 360f);
      }
    }
    if (data.cities != null)
    {
      foreach (CityData city in data.cities)
      {
        if (city.created_time < 0.0)
          city.created_time = data.mapStats.world_time + city.created_time + (double) Randy.randomFloat(0.0f, 360f);
      }
    }
    if (data.cultures == null)
      return;
    foreach (CultureData culture in data.cultures)
    {
      if (culture.created_time == 0.0 && culture.year_obsolete > 0)
        culture.created_time = data.mapStats.world_time - (double) culture.year_obsolete * 60.0 + (double) Randy.randomFloat(0.0f, 360f);
    }
  }

  public static void checkOldCityZones(SavedMap pData)
  {
    if (pData.saveVersion >= 7)
      return;
    for (int index = 0; index < pData.buildings.Count; ++index)
    {
      BuildingData building = pData.buildings[index];
      City city = World.world.cities.get(building.cityID);
      if (city != null)
      {
        WorldTile tile = World.world.GetTile(building.mainX, building.mainY);
        city.addZone(tile.zone);
      }
    }
  }

  public static void checkOldBuildingID(SavedMap pData)
  {
    if (SaveConverter._buildings_old_id_dictionary == null)
    {
      SaveConverter._buildings_old_id_dictionary = new Dictionary<string, string[]>();
      SaveConverter._buildings_old_id_dictionary.Add("geyserAcid", new string[1]
      {
        "geyser_acid"
      });
      SaveConverter._buildings_old_id_dictionary.Add("tree", new string[3]
      {
        "tree_green_1",
        "tree_green_2",
        "tree_green_3"
      });
      SaveConverter._buildings_old_id_dictionary.Add("mushroom", new string[1]
      {
        "mushroom_red"
      });
      SaveConverter._buildings_old_id_dictionary.Add("savanna_tree", new string[2]
      {
        "savanna_tree_1",
        "savanna_tree_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("savanna_tree_big", new string[2]
      {
        "savanna_tree_big_1",
        "savanna_tree_big_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("cacti", new string[1]
      {
        "cacti_tree"
      });
      SaveConverter._buildings_old_id_dictionary.Add("iron", new string[1]
      {
        "mineral_metals"
      });
      SaveConverter._buildings_old_id_dictionary.Add("iron_m", new string[1]
      {
        "mineral_metals"
      });
      SaveConverter._buildings_old_id_dictionary.Add("iron_s", new string[1]
      {
        "mineral_metals"
      });
      SaveConverter._buildings_old_id_dictionary.Add("gold", new string[1]
      {
        "mineral_gold"
      });
      SaveConverter._buildings_old_id_dictionary.Add("gold_m", new string[1]
      {
        "mineral_gold"
      });
      SaveConverter._buildings_old_id_dictionary.Add("gold_s", new string[1]
      {
        "mineral_gold"
      });
      SaveConverter._buildings_old_id_dictionary.Add("ore_deposit", new string[1]
      {
        "mineral_metals"
      });
      SaveConverter._buildings_old_id_dictionary.Add("ore_deposit_m", new string[1]
      {
        "mineral_metals"
      });
      SaveConverter._buildings_old_id_dictionary.Add("ore_deposit_s", new string[1]
      {
        "mineral_metals"
      });
      SaveConverter._buildings_old_id_dictionary.Add("palm", new string[1]
      {
        "palm_tree"
      });
      SaveConverter._buildings_old_id_dictionary.Add("pine", new string[1]
      {
        "pine_tree"
      });
      SaveConverter._buildings_old_id_dictionary.Add("stone", new string[1]
      {
        "mineral_stone"
      });
      SaveConverter._buildings_old_id_dictionary.Add("stone_m", new string[1]
      {
        "mineral_stone"
      });
      SaveConverter._buildings_old_id_dictionary.Add("stone_s", new string[1]
      {
        "mineral_stone"
      });
      SaveConverter._buildings_old_id_dictionary.Add("ruins_small", new string[1]
      {
        "poop"
      });
      SaveConverter._buildings_old_id_dictionary.Add("ruins_medium", new string[1]
      {
        "poop"
      });
      SaveConverter._buildings_old_id_dictionary.Add("house_human", new string[1]
      {
        "house_human_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1house_human", new string[1]
      {
        "house_human_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2house_human", new string[1]
      {
        "house_human_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("3house_human", new string[1]
      {
        "house_human_3"
      });
      SaveConverter._buildings_old_id_dictionary.Add("4house_human", new string[1]
      {
        "house_human_4"
      });
      SaveConverter._buildings_old_id_dictionary.Add("5house_human", new string[1]
      {
        "house_human_5"
      });
      SaveConverter._buildings_old_id_dictionary.Add("hall_human", new string[1]
      {
        "hall_human_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1hall_human", new string[1]
      {
        "hall_human_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2hall_human", new string[1]
      {
        "hall_human_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("windmill_human", new string[1]
      {
        "windmill_human_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1windmill_human", new string[1]
      {
        "windmill_human_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("house_elf", new string[1]
      {
        "house_elf_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1house_elf", new string[1]
      {
        "house_elf_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2house_elf", new string[1]
      {
        "house_elf_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("3house_elf", new string[1]
      {
        "house_elf_3"
      });
      SaveConverter._buildings_old_id_dictionary.Add("4house_elf", new string[1]
      {
        "house_elf_4"
      });
      SaveConverter._buildings_old_id_dictionary.Add("5house_elf", new string[1]
      {
        "house_elf_5"
      });
      SaveConverter._buildings_old_id_dictionary.Add("hall_elf", new string[1]
      {
        "hall_elf_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1hall_elf", new string[1]
      {
        "hall_elf_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2hall_elf", new string[1]
      {
        "hall_elf_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("windmill_elf", new string[1]
      {
        "windmill_elf_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1windmill_elf", new string[1]
      {
        "windmill_elf_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("house_orc", new string[1]
      {
        "house_orc_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1house_orc", new string[1]
      {
        "house_orc_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2house_orc", new string[1]
      {
        "house_orc_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("3house_orc", new string[1]
      {
        "house_orc_3"
      });
      SaveConverter._buildings_old_id_dictionary.Add("4house_orc", new string[1]
      {
        "house_orc_4"
      });
      SaveConverter._buildings_old_id_dictionary.Add("5house_orc", new string[1]
      {
        "house_orc_5"
      });
      SaveConverter._buildings_old_id_dictionary.Add("hall_orc", new string[1]
      {
        "hall_orc_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1hall_orc", new string[1]
      {
        "hall_orc_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2hall_orc", new string[1]
      {
        "hall_orc_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("windmill_orc", new string[1]
      {
        "windmill_orc_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1windmill_orc", new string[1]
      {
        "windmill_orc_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("house_dwarf", new string[1]
      {
        "house_dwarf_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1house_dwarf", new string[1]
      {
        "house_dwarf_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2house_dwarf", new string[1]
      {
        "house_dwarf_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("3house_dwarf", new string[1]
      {
        "house_dwarf_3"
      });
      SaveConverter._buildings_old_id_dictionary.Add("4house_dwarf", new string[1]
      {
        "house_dwarf_4"
      });
      SaveConverter._buildings_old_id_dictionary.Add("5house_dwarf", new string[1]
      {
        "house_dwarf_5"
      });
      SaveConverter._buildings_old_id_dictionary.Add("hall_dwarf", new string[1]
      {
        "hall_dwarf_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1hall_dwarf", new string[1]
      {
        "hall_dwarf_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2hall_dwarf", new string[1]
      {
        "hall_dwarf_2"
      });
      SaveConverter._buildings_old_id_dictionary.Add("windmill_dwarf", new string[1]
      {
        "windmill_dwarf_0"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1windmill_dwarf", new string[1]
      {
        "windmill_dwarf_1"
      });
      SaveConverter._buildings_old_id_dictionary.Add("0wheat", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("1wheat", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("2wheat", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("3wheat", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("4wheat", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("wheat_0", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("wheat_1", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("wheat_2", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("wheat_3", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("wheat_4", new string[1]
      {
        "wheat"
      });
      SaveConverter._buildings_old_id_dictionary.Add("goldenBrain", new string[1]
      {
        "golden_brain"
      });
      SaveConverter._buildings_old_id_dictionary.Add("corruptedBrain", new string[1]
      {
        "corrupted_brain"
      });
      SaveConverter._buildings_old_id_dictionary.Add("flameTower", new string[1]
      {
        "flame_tower"
      });
      SaveConverter._buildings_old_id_dictionary.Add("iceTower", new string[1]
      {
        "ice_tower"
      });
      SaveConverter._buildings_old_id_dictionary.Add("superPumpkin", new string[1]
      {
        "super_pumpkin"
      });
    }
    if (pData.buildings == null)
      return;
    foreach (BuildingData building in pData.buildings)
    {
      if (SaveConverter._buildings_old_id_dictionary.ContainsKey(building.asset_id))
        building.asset_id = SaveConverter._buildings_old_id_dictionary[building.asset_id].GetRandom<string>();
      if (building.state == BuildingState.None)
        building.state = BuildingState.Normal;
      if (building.state == BuildingState.CivKingdom)
        building.state = BuildingState.Normal;
      if (building.state == BuildingState.CivAbandoned)
        building.state = BuildingState.Normal;
    }
  }

  private static ActorData getNewActorData(ActorDataObsolete pOldData)
  {
    ActorData status = pOldData.status;
    if (string.IsNullOrEmpty(status.asset_id))
    {
      Debug.Log((object) "skipping unit because it's missing an asset_id");
      return (ActorData) null;
    }
    status.x = pOldData.x;
    status.y = pOldData.y;
    status.cityID = pOldData.cityID;
    List<long> savedItems = pOldData.saved_items;
    // ISSUE: explicit non-virtual call
    if ((savedItems != null ? (__nonvirtual (savedItems.Count) > 0 ? 1 : 0) : 0) != 0)
      status.saved_items = pOldData.saved_items;
    status.inventory = pOldData.inventory;
    if (status.inventory.isEmpty())
      status.inventory.empty();
    return status;
  }
}
