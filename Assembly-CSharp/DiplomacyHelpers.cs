// Decompiled with JetBrains decompiler
// Type: DiplomacyHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class DiplomacyHelpers
{
  public static bool isWarNeeded(Kingdom pKingdom)
  {
    if (!pKingdom.hasCities() || !pKingdom.hasCapital() || pKingdom.data.timestamp_last_war != -1.0 && Date.getYearsSince(pKingdom.data.timestamp_last_war) <= SimGlobals.m.diplomacy_years_war_timeout || DiplomacyHelpers.wars.hasWars(pKingdom) || pKingdom.countTotalWarriors() <= SimGlobals.m.diplomacy_years_war_min_warriors)
      return false;
    float populationPeople = (float) pKingdom.getPopulationPeople();
    float populationTotalPossible = (float) pKingdom.getPopulationTotalPossible();
    return pKingdom.countCities() >= 4 || (double) populationPeople >= (double) populationTotalPossible * 0.60000002384185791;
  }

  public static Kingdom getWarTarget(Kingdom pInitiatorKingdom)
  {
    // ISSUE: unable to decompile the method.
  }

  public static Kingdom getAllianceTarget(Kingdom pKingdomStarter)
  {
    if (pKingdomStarter.isSupreme())
      return (Kingdom) null;
    using (ListPool<Kingdom> neutralKingdoms = World.world.wars.getNeutralKingdoms(pKingdomStarter, true, true))
    {
      if (neutralKingdoms.Count == 0)
        return (Kingdom) null;
      foreach (Kingdom allianceTarget in neutralKingdoms.LoopRandom<Kingdom>())
      {
        if (allianceTarget.hasKing() && !allianceTarget.isSupreme() && !allianceTarget.king.hasPlot() && pKingdomStarter.isOpinionTowardsKingdomGood(allianceTarget) && allianceTarget.getRenown() >= PlotsLibrary.alliance_create.min_renown_kingdom)
        {
          bool flag = false;
          if (pKingdomStarter.countCities() <= 2 && allianceTarget.countCities() <= 2 && !pKingdomStarter.hasNearbyKingdoms() && !allianceTarget.hasNearbyKingdoms())
            flag = true;
          if (!flag && DiplomacyHelpers.areKingdomsClose(allianceTarget, pKingdomStarter))
            flag = true;
          if (flag)
            return allianceTarget;
        }
      }
      return (Kingdom) null;
    }
  }

  public static bool areKingdomsClose(Kingdom pMain, Kingdom pTarget)
  {
    foreach (City city1 in pMain.getCities())
    {
      foreach (City city2 in pTarget.getCities())
      {
        if (City.nearbyBorders(city1, city2))
          return true;
      }
    }
    return false;
  }

  public static bool isThereActiveCityConquest(Kingdom pKingdom, Kingdom pTargetKingdom)
  {
    foreach (City city in pKingdom.getCities())
    {
      if (city.isGettingCapturedBy(pTargetKingdom))
        return true;
    }
    foreach (City city in pTargetKingdom.getCities())
    {
      if (city.isGettingCapturedBy(pKingdom))
        return true;
    }
    return false;
  }

  public static bool isThereFightBetween(Kingdom pKingdom1, Kingdom pKingdom2)
  {
    return DiplomacyHelpers.isThereActiveCityFight(pKingdom1, pKingdom2) || DiplomacyHelpers.isThereActiveCityFight(pKingdom2, pKingdom1);
  }

  private static bool isThereActiveCityFight(Kingdom pDefenderKingdom, Kingdom pAttackerKingdom)
  {
    foreach (City city in pDefenderKingdom.getCities())
    {
      if (city.hasArmy())
      {
        Army army = city.army;
        if (army.hasCaptain())
        {
          Actor captain = army.getCaptain();
          if (captain.current_tile.hasCity() && captain.current_tile.zone_city.kingdom == pAttackerKingdom)
            return true;
        }
      }
    }
    return false;
  }

  public static bool areDefendersGettingCaptured(this War pWar)
  {
    foreach (Kingdom defender in pWar.getDefenders())
    {
      if (defender.isGettingCaptured())
        return true;
    }
    return false;
  }

  public static bool areAttackersGettingCaptured(this War pWar)
  {
    foreach (Kingdom attacker in pWar.getAttackers())
    {
      if (attacker.isGettingCaptured())
        return true;
    }
    return false;
  }

  public static bool areAttackersAttackingAnotherCity(this War pWar)
  {
    foreach (Kingdom attacker in pWar.getAttackers())
    {
      if (attacker.isAttackingAnotherCity())
        return true;
    }
    return false;
  }

  public static bool areDefendersAttackingAnotherCity(this War pWar)
  {
    foreach (Kingdom defender in pWar.getDefenders())
    {
      if (defender.isAttackingAnotherCity())
        return true;
    }
    return false;
  }

  public static bool isAttackingAnotherCity(this Kingdom pAttackerKingdom)
  {
    foreach (City city in pAttackerKingdom.getCities())
    {
      if (city.hasArmy())
      {
        Army army = city.army;
        if (army.hasCaptain())
        {
          Actor captain = army.getCaptain();
          if (captain.current_tile.hasCity() && captain.current_tile.zone_city.kingdom.isEnemy(pAttackerKingdom))
            return true;
        }
      }
    }
    return false;
  }

  public static WarManager wars => World.world.wars;

  public static DiplomacyManager diplomacy => World.world.diplomacy;
}
