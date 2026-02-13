// Decompiled with JetBrains decompiler
// Type: NameGeneratorReplacers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class NameGeneratorReplacers
{
  public static void replaceKingdom(ref string pName, Kingdom pKingdom)
  {
    if (!pName.Contains("$kingdom$"))
      return;
    if (pKingdom == null)
      pName = "";
    else
      pName = pName.Replace("$kingdom$", pKingdom.name);
  }

  public static void replaceEnemyKing(ref string pName, Actor pActor)
  {
    using (ListPool<Kingdom> enemiesKingdoms = pActor.kingdom.getEnemiesKingdoms())
    {
      foreach (Kingdom kingdom in enemiesKingdoms.LoopRandom<Kingdom>())
      {
        if (kingdom.hasKing() && Toolbox.isFirstLatin(kingdom.king.getName()))
        {
          pName = pName.Replace("$king$", "King " + kingdom.king.getName());
          return;
        }
      }
      pName = "";
    }
  }

  public static void replaceOwnKingdom(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$kingdom$"))
      return;
    if (!pActor.hasKingdom())
    {
      pName = "";
    }
    else
    {
      Kingdom kingdom = pActor.kingdom;
      pName = pName.Replace("$kingdom$", kingdom.name);
    }
  }

  public static void replaceEnemyKingdom(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$kingdom$"))
      return;
    using (ListPool<Kingdom> enemiesKingdoms = pActor.kingdom.getEnemiesKingdoms())
    {
      foreach (Kingdom kingdom in enemiesKingdoms.LoopRandom<Kingdom>())
      {
        if (Toolbox.isFirstLatin(kingdom.name))
        {
          pName = pName.Replace("$kingdom$", kingdom.name);
          return;
        }
      }
      pName = "";
    }
  }

  public static void replaceFavoriteFood(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$food$"))
      return;
    Kingdom kingdom = pActor.kingdom;
    bool? nullable;
    int num1;
    if (kingdom == null)
    {
      num1 = 0;
    }
    else
    {
      nullable = kingdom.king?.hasFavoriteFood();
      bool flag = true;
      num1 = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
    }
    string newValue;
    if (num1 != 0)
    {
      newValue = pActor.kingdom.king.favorite_food_asset.getTranslatedName();
    }
    else
    {
      City city = pActor.city;
      int num2;
      if (city == null)
      {
        num2 = 0;
      }
      else
      {
        nullable = city.leader?.hasFavoriteFood();
        bool flag = true;
        num2 = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
      }
      newValue = num2 == 0 ? (!pActor.hasFavoriteFood() ? AssetManager.resources.list.GetRandom<ResourceAsset>().getTranslatedName() : pActor.favorite_food_asset.getTranslatedName()) : pActor.city.leader.favorite_food_asset.getTranslatedName();
    }
    pName = pName.Replace("$food$", newValue);
  }

  public static void replaceOwnName(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$unit$"))
      return;
    pName = pName.Replace("$unit$", pActor.getName());
  }

  public static void replaceOwnCity(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$city$"))
      return;
    if (!pActor.hasCity())
    {
      pName = "";
    }
    else
    {
      City city = pActor.city;
      pName = pName.Replace("$city$", city.name);
    }
  }

  public static void replaceOwnSubspecies(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$subspecies$"))
      return;
    if (!pActor.hasSubspecies())
    {
      pName = "";
    }
    else
    {
      Subspecies subspecies = pActor.subspecies;
      pName = pName.Replace("$subspecies$", subspecies.name);
    }
  }

  public static void replaceOwnAlliance(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$alliance$"))
      return;
    if (!pActor.hasKingdom())
    {
      pName = "";
    }
    else
    {
      Kingdom kingdom = pActor.kingdom;
      if (!kingdom.hasAlliance())
      {
        pName = "";
      }
      else
      {
        Alliance alliance = kingdom.getAlliance();
        pName = pName.Replace("$alliance$", alliance.name);
      }
    }
  }

  public static void replaceOwnKingClan(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$clan$"))
      return;
    Kingdom kingdom = pActor.kingdom;
    if (!kingdom.hasKing())
    {
      pName = "";
    }
    else
    {
      Actor king = kingdom.king;
      if (!king.hasClan())
        pName = "";
      else
        pName = pName.Replace("$clan$", king.clan.name);
    }
  }

  public static void replaceOwnLeader(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$leader$"))
      return;
    if (!pActor.hasCity())
    {
      pName = "";
    }
    else
    {
      City city = pActor.city;
      if (!city.hasLeader())
      {
        pName = "";
      }
      else
      {
        Actor leader = city.leader;
        pName = pName.Replace("$leader$", leader.getName());
      }
    }
  }

  public static void replaceFigure(ref string pName, Actor pActor)
  {
    NameGeneratorReplacers.replaceOwnLeader(ref pName, pActor);
    NameGeneratorReplacers.replaceOwnKing(ref pName, pActor);
    NameGeneratorReplacers.replaceOwnKingClan(ref pName, pActor);
  }

  public static void replaceAnyCity(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$city_random$"))
      return;
    if (!World.world.cities.hasAny())
    {
      pName = "";
    }
    else
    {
      City random = World.world.cities.getRandom();
      pName = pName.Replace("$city_random$", random.name);
    }
  }

  public static void replaceAnyKingdom(ref string pName, Actor _)
  {
    if (!pName.Contains("$kingdom_random$"))
      return;
    if (!World.world.kingdoms.hasAny())
    {
      pName = "";
    }
    else
    {
      Kingdom random = World.world.kingdoms.getRandom();
      pName = pName.Replace("$kingdom_random$", random.name);
    }
  }

  public static void replaceAnyCulture(ref string pName, Actor _)
  {
    if (!pName.Contains("$culture_random$"))
      return;
    if (!World.world.cultures.hasAny())
    {
      pName = "";
    }
    else
    {
      Culture random = World.world.cultures.getRandom();
      pName = pName.Replace("$culture_random$", random.name);
    }
  }

  public static void replaceAnyFamily(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$family_random$"))
      return;
    if (!World.world.families.hasAny())
    {
      pName = "";
    }
    else
    {
      int count = World.world.families.Count;
      do
      {
        Family random = World.world.families.getRandom();
        if (random.isSameSpecies(pActor.asset.id))
        {
          Family family = random;
          pName = pName.Replace("$family_random$", family.name);
          return;
        }
      }
      while (count-- > 0);
      pName = "";
    }
  }

  public static void replaceAnySubspecies(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$random_subspecies$"))
      return;
    if (!World.world.subspecies.hasAny())
    {
      pName = "";
    }
    else
    {
      Subspecies random = World.world.subspecies.getRandom();
      pName = pName.Replace("$random_subspecies$", random.name);
    }
  }

  public static void replaceAnyClan(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$clan_random$"))
      return;
    if (!World.world.clans.hasAny())
    {
      pName = "";
    }
    else
    {
      Clan random = World.world.clans.getRandom();
      pName = pName.Replace("$clan_random$", random.name);
    }
  }

  public static void replaceAnyKing(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$king_random$"))
      return;
    if (!World.world.kingdoms.hasAny())
    {
      pName = "";
    }
    else
    {
      int num = 0;
      Kingdom kingdom;
      for (kingdom = (Kingdom) null; kingdom == null || !kingdom.hasKing(); kingdom = World.world.kingdoms.getRandom())
      {
        if (num++ > 10)
        {
          pName = "";
          return;
        }
      }
      Actor king = kingdom.king;
      pName = pName.Replace("$king_random$", king.getName());
    }
  }

  public static void replaceAnyLeader(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$leader_random$"))
      return;
    if (!World.world.cities.hasAny())
    {
      pName = "";
    }
    else
    {
      int num = 0;
      City city;
      for (city = (City) null; city == null || !city.hasLeader(); city = World.world.cities.getRandom())
      {
        if (num++ > 10)
        {
          pName = "";
          return;
        }
      }
      Actor leader = city.leader;
      pName = pName.Replace("$leader_random$", leader.getName());
    }
  }

  public static void replaceOwnKing(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$king$"))
      return;
    if (!pActor.hasKingdom())
    {
      pName = "";
    }
    else
    {
      Kingdom kingdom = pActor.kingdom;
      if (!kingdom.hasKing())
      {
        pName = "";
      }
      else
      {
        Actor king = kingdom.king;
        pName = pName.Replace("$king$", king.getName());
      }
    }
  }

  public static void replaceOwnKingLover(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$king_lover$"))
      return;
    if (!pActor.hasKingdom())
    {
      pName = "";
    }
    else
    {
      Kingdom kingdom = pActor.kingdom;
      if (!kingdom.hasKing())
      {
        pName = "";
      }
      else
      {
        Actor king = kingdom.king;
        if (!king.hasLover())
        {
          pName = "";
        }
        else
        {
          Actor lover = king.lover;
          pName = pName.Replace("$king$", king.getName());
          pName = pName.Replace("$king_lover$", lover.getName());
        }
      }
    }
  }

  public static void replaceOwnCulture(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$culture$"))
      return;
    if (!pActor.hasCulture())
    {
      pName = "";
    }
    else
    {
      Culture culture = pActor.culture;
      pName = pName.Replace("$culture$", culture.name);
    }
  }

  public static void replaceOwnLanguage(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$language$"))
      return;
    if (!pActor.hasLanguage())
    {
      pName = "";
    }
    else
    {
      Language language = pActor.language;
      pName = pName.Replace("$language$", language.name);
    }
  }

  public static void replaceOwnReligion(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$religion$"))
      return;
    if (!pActor.hasReligion())
    {
      pName = "";
    }
    else
    {
      Religion religion = pActor.religion;
      pName = pName.Replace("$religion$", religion.name);
    }
  }

  public static void replaceOwnFamily(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$family$"))
      return;
    if (!pActor.hasFamily())
    {
      pName = "";
    }
    else
    {
      Family family = pActor.family;
      pName = pName.Replace("$family$", family.name);
    }
  }

  public static void replaceAnyFamilyFounders(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$family_founder_1$") && !pName.Contains("$family_founder_2$"))
      return;
    if (!World.world.families.hasAny())
    {
      pName = "";
    }
    else
    {
      int count = World.world.families.list.Count;
      do
      {
        Family random = World.world.families.getRandom();
        if (random.isSameSpecies(pActor.asset.id) && random.hasFounders())
        {
          Family family = random;
          NameGeneratorReplacers.replaceFamilyFounder1(ref pName, family.units[0]);
          NameGeneratorReplacers.replaceFamilyFounder2(ref pName, family.units[0]);
          return;
        }
      }
      while (count-- > 0);
      pName = "";
    }
  }

  public static void replaceOwnFamilyFounders(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$family_founder_1$") && !pName.Contains("$family_founder_2$"))
      return;
    if (!pActor.hasFamily())
    {
      pName = "";
    }
    else
    {
      NameGeneratorReplacers.replaceFamilyFounder1(ref pName, pActor);
      NameGeneratorReplacers.replaceFamilyFounder2(ref pName, pActor);
    }
  }

  public static void replaceFamilyFounder1(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$family_founder_1$"))
      return;
    if (!pActor.hasFamily())
    {
      pName = "";
    }
    else
    {
      string founderActorName1 = pActor.family.data.founder_actor_name_1;
      if (string.IsNullOrEmpty(founderActorName1))
        pName = "";
      else
        pName = pName.Replace("$family_founder_1$", founderActorName1);
    }
  }

  public static void replaceFamilyFounder2(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$family_founder_2$"))
      return;
    if (!pActor.hasFamily())
    {
      pName = "";
    }
    else
    {
      string founderActorName2 = pActor.family.data.founder_actor_name_2;
      if (string.IsNullOrEmpty(founderActorName2))
        pName = "";
      else
        pName = pName.Replace("$family_founder_2$", founderActorName2);
    }
  }

  public static void replaceWorldName(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$world_name$"))
      return;
    pName = pName.Replace("$world_name$", World.world.map_stats.name);
  }

  public static void replaceArchitectName(ref string pName, Actor pActor)
  {
    if (!pName.Contains("$architect_name$"))
      return;
    pName = pName.Replace("$architect_name$", World.world.map_stats.player_name);
  }

  public static void replacer_debug(ref string pName)
  {
    pName = pName.Replace("$alliance$", "Pact of Gregs");
    pName = pName.Replace("$food$", "Tea");
    pName = pName.Replace("$family$", "Gregovich");
    pName = pName.Replace("$family_random$", "Urg Zurg");
    pName = pName.Replace("$family_founder_1$", "Greg");
    pName = pName.Replace("$family_founder_2$", "Gregia");
    pName = pName.Replace("$king$", "Gregor");
    pName = pName.Replace("$king_lover$", "Gregoria");
    pName = pName.Replace("$king_random$", "Zurg Gurg");
    pName = pName.Replace("$kingdom$", "Kingdom of Greg");
    pName = pName.Replace("$kingdom_random$", "Brothers of Wargh");
    pName = pName.Replace("$clan$", "Greg Clan");
    pName = pName.Replace("$clan_random$", "Deze Zaz");
    pName = pName.Replace("$leader$", "Gregoryl");
    pName = pName.Replace("$leader_random$", "Orcaryl");
    pName = pName.Replace("$culture$", "Gragian Culture");
    pName = pName.Replace("$culture_random$", "Orkian Kult");
    pName = pName.Replace("$city$", "Gregopolis");
    pName = pName.Replace("$city_random$", "Orcville");
    pName = pName.Replace("$unit$", "Greg the Great");
    pName = pName.Replace("$warrior$", "Greg the Warrior");
    pName = pName.Replace("$language$", "Gregian Language");
    pName = pName.Replace("$religion$", "Gregianity");
    pName = pName.Replace("$subspecies$", "Gregian Sapient");
    pName = pName.Replace("$random_subspecies$", "Weird Dudes");
    pName = pName.Replace("$world_name$", "The Bad Place");
    pName = pName.Replace("$architect_name$", "Your Mom");
    pName = pName.Replace("$item$", "Legendary Greg Axe");
    if (!pName.Contains('$'))
      return;
    Debug.LogWarning((object) ("replacer_debug missing variable " + pName));
  }
}
