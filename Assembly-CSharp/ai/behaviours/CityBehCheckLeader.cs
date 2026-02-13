// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehCheckLeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace ai.behaviours;

public class CityBehCheckLeader : BehaviourActionCity
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.uses_clans = true;
  }

  public override BehResult execute(City pCity)
  {
    this.checkLeaderClan(pCity);
    this.checkFindLeader(pCity);
    return BehResult.Continue;
  }

  private void checkLeaderClan(City pCity)
  {
    if (!pCity.hasLeader())
      return;
    Actor leader = pCity.leader;
    if (leader.hasClan())
      return;
    BehaviourActionBase<City>.world.clans.newClan(leader, true);
  }

  private void checkFindLeader(City pCity)
  {
    if (pCity.hasLeader() || pCity.isGettingCaptured())
      return;
    Actor pActor = this.tryGetClanLeader(pCity);
    if (pActor != null)
    {
      if (pActor.city != pCity)
        pActor.stopBeingWarrior();
      pActor.joinCity(pCity);
      pCity.setLeader(pActor, true);
    }
    else
    {
      int num1 = 0;
      foreach (Actor unit in pCity.units)
      {
        if (!unit.isKing() && !unit.isCityLeader())
        {
          int pAmount = 1;
          if (unit.is_profession_citizen)
          {
            if (unit.isFavorite())
              pAmount += 2;
            int num2 = ActorTool.attributeDice(unit, pAmount);
            if (pActor == null || num2 > num1)
            {
              pActor = unit;
              num1 = num2;
            }
          }
        }
      }
      if (pActor == null)
        return;
      pCity.setLeader(pActor, true);
    }
  }

  private Actor tryGetClanLeader(City pCity)
  {
    Kingdom kingdom = pCity.kingdom;
    Clan clan = (Clan) null;
    if (kingdom.data.royal_clan_id.hasValue())
      clan = BehaviourActionBase<City>.world.clans.get(kingdom.data.royal_clan_id);
    using (ListPool<Actor> pUnits1 = new ListPool<Actor>())
    {
      using (ListPool<Actor> pUnits2 = new ListPool<Actor>())
      {
        foreach (MetaObject<CityData> city in kingdom.getCities())
        {
          foreach (Actor unit in city.getUnits())
          {
            if (unit.isUnitFitToRule() && !unit.isKing() && !unit.isCityLeader() && unit.hasClan())
            {
              if (clan != null && unit.clan == clan)
                pUnits1.Add(unit);
              else
                pUnits2.Add(unit);
            }
          }
        }
        Actor clanLeader = (Actor) null;
        if (pUnits1.Count > 0)
        {
          if (pCity.hasCulture())
            return ListSorters.getUnitSortedByAgeAndTraits(pUnits1, pCity.culture);
          pUnits1.Sort(new Comparison<Actor>(ListSorters.sortUnitByAgeOldFirst));
          return pUnits1[0];
        }
        if (pUnits2.Count <= 0)
          return clanLeader;
        if (pCity.hasCulture())
          return ListSorters.getUnitSortedByAgeAndTraits(pUnits2, pCity.culture);
        pUnits2.Sort(new Comparison<Actor>(ListSorters.sortUnitByAgeOldFirst));
        return pUnits2[0];
      }
    }
  }
}
