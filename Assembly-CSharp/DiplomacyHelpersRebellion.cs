// Decompiled with JetBrains decompiler
// Type: DiplomacyHelpersRebellion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class DiplomacyHelpersRebellion
{
  public static void startRebellion(Actor pActor, Plot pPlot, bool pCheckForHappiness)
  {
    City city1 = pActor.city;
    Kingdom kingdom1 = city1.kingdom;
    if (pActor.isCityLeader())
      pActor.city.removeLeader();
    Kingdom kingdom2 = city1.makeOwnKingdom(pActor, true);
    using (ListPool<City> pNewCities = new ListPool<City>())
    {
      pNewCities.Add(city1);
      pActor.joinCity(city1);
      War war1 = (War) null;
      foreach (War war2 in kingdom1.getWars())
      {
        if (war2.isMainAttacker(kingdom1) && war2.getAsset() == WarTypeLibrary.rebellion)
        {
          war1 = war2;
          war1.joinDefenders(kingdom2);
          break;
        }
      }
      if (war1 == null)
      {
        War war3 = World.world.diplomacy.startWar(kingdom1, kingdom2, WarTypeLibrary.rebellion);
        if (kingdom1.hasAlliance())
        {
          foreach (Kingdom pKingdom in kingdom1.getAlliance().kingdoms_hashset)
          {
            if (pKingdom != kingdom1 && pKingdom.isOpinionTowardsKingdomGood(kingdom1))
              war3.joinAttackers(pKingdom);
          }
        }
      }
      foreach (Actor unit in pPlot.units)
      {
        City city2 = unit.city;
        if (city2 != null && city2.kingdom != kingdom2 && city2.kingdom == kingdom1)
          city2.joinAnotherKingdom(kingdom2, pRebellion: true);
      }
      int num1 = kingdom1.countCities();
      int num2 = kingdom2.getMaxCities() - pNewCities.Count;
      if (num2 < 0)
        num2 = 0;
      if (num2 > num1 / 3)
        num2 = (int) ((double) num1 / 3.0);
      int num3 = 0;
      while (num3 < num2 && DiplomacyHelpersRebellion.checkMoreAlignedCities(kingdom2, kingdom1, pNewCities, pCheckForHappiness))
        ++num3;
    }
  }

  public static bool checkMoreAlignedCities(
    Kingdom pNewKingdom,
    Kingdom pOldKingdom,
    ListPool<City> pNewCities,
    bool pCheckForHappiness)
  {
    using (ListPool<City> listPool = new ListPool<City>(World.world.cities.Count))
    {
      DiplomacyHelpersRebellion.addNeighbourCities(listPool, pNewCities);
      if (listPool.Count == 0)
        listPool.AddRange(pOldKingdom.getCities());
      if (listPool.Count == 0)
        return false;
      foreach (City city in listPool.LoopRandom<City>())
      {
        if (city.kingdom == pOldKingdom && !city.isCapitalCity() && (!pCheckForHappiness || !city.isHappy()) && !Randy.randomBool())
        {
          city.joinAnotherKingdom(pNewKingdom, pRebellion: true);
          return true;
        }
      }
      return true;
    }
  }

  private static void addNeighbourCities(
    ListPool<City> pTempCitiesToCheck,
    ListPool<City> pRebelledCities)
  {
    // ISSUE: unable to decompile the method.
  }
}
