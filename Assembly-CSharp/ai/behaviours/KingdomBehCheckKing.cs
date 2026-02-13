// Decompiled with JetBrains decompiler
// Type: ai.behaviours.KingdomBehCheckKing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class KingdomBehCheckKing : BehaviourActionKingdom
{
  public override BehResult execute(Kingdom pKingdom)
  {
    if ((double) pKingdom.data.timer_new_king > 0.0)
      return BehResult.Continue;
    if (pKingdom.hasKing())
    {
      Actor king = pKingdom.king;
      if (king.isAlive())
      {
        this.tryToGiveGoldenTooth(king);
        this.checkClanCreation(king);
        return BehResult.Continue;
      }
    }
    pKingdom.clearKingData();
    if (pKingdom.data.royal_clan_id != -1L)
    {
      Clan clan = BehaviourActionBase<Kingdom>.world.clans.get(pKingdom.data.royal_clan_id);
      bool flag = !clan.isRekt();
      Actor kingFromRoyalClan = this.findKingFromRoyalClan(pKingdom);
      if (kingFromRoyalClan == null)
      {
        if (pKingdom.countCities() == 1)
        {
          if (pKingdom.capital != null && pKingdom.capital.hasLeader())
          {
            Actor leader = pKingdom.capital.leader;
            pKingdom.capital.removeLeader();
            leader.stopBeingWarrior();
            pKingdom.setKing(leader);
          }
        }
        else
          this.checkKingdomChaos(pKingdom);
      }
      else if (((!pKingdom.hasCulture() ? 0 : (pKingdom.culture.hasTrait("shattered_crown") ? 1 : 0)) & (flag ? 1 : 0)) != 0)
        this.checkShatteredCrownEvent(pKingdom, kingFromRoyalClan, clan);
      if (!flag)
        pKingdom.data.royal_clan_id = -1L;
    }
    else
    {
      Actor kingFromLeaders = SuccessionTool.getKingFromLeaders(pKingdom);
      if (kingFromLeaders != null)
        this.makeKingAndMoveToCapital(pKingdom, kingFromLeaders);
      else
        this.checkKingdomChaos(pKingdom);
    }
    return BehResult.Continue;
  }

  private void checkKingdomChaos(Kingdom pMainKingdom)
  {
    bool flag = false;
    using (ListPool<City> list = new ListPool<City>(pMainKingdom.countCities()))
    {
      foreach (City city in pMainKingdom.getCities())
      {
        if (city != pMainKingdom.capital && city.hasLeader())
          list.Add(city);
      }
      if (list.Count == 0)
        return;
      foreach (City city in list.LoopRandom<City>())
      {
        Actor leader = city.leader;
        if (leader != null && leader.isAlive())
        {
          city.makeOwnKingdom(leader, pFellApart: true);
          flag = true;
        }
      }
      if (!flag)
        return;
      if (pMainKingdom.hasAlliance())
        pMainKingdom.getAlliance().leave(pMainKingdom);
      WorldLog.logFracturedKingdom(pMainKingdom);
    }
  }

  private void checkShatteredCrownEvent(Kingdom pMainKingdom, Actor pMainKing, Clan pRoyalClan)
  {
    // ISSUE: unable to decompile the method.
  }

  private void checkClanCreation(Actor pActor)
  {
    if (pActor.hasClan())
      return;
    BehaviourActionBase<Kingdom>.world.clans.newClan(pActor, true);
  }

  private void tryToGiveGoldenTooth(Actor pActor)
  {
    if (pActor.getAge() <= 45 || !Randy.randomChance(0.05f))
      return;
    pActor.addTrait("golden_tooth");
  }

  private bool isRebellionsEnabled() => WorldLawLibrary.world_law_rebellions.isEnabled();

  private Actor findKingFromRoyalClan(Kingdom pKingdom)
  {
    Actor pNewKing = SuccessionTool.getKingFromRoyalClan(pKingdom);
    if (pNewKing == null && pKingdom.hasCulture() && (pKingdom.culture.hasTrait("unbroken_chain") || !this.isRebellionsEnabled()))
      pNewKing = SuccessionTool.getKingFromLeaders(pKingdom);
    if (pNewKing == null)
      return (Actor) null;
    this.makeKingAndMoveToCapital(pKingdom, pNewKing);
    return pNewKing;
  }

  private void makeKingAndMoveToCapital(Kingdom pKingdom, Actor pNewKing)
  {
    if (pNewKing.hasCity())
    {
      pNewKing.stopBeingWarrior();
      if (pNewKing.isCityLeader())
        pNewKing.city.removeLeader();
    }
    if (pKingdom.hasCapital() && pNewKing.city != pKingdom.capital)
      pNewKing.joinCity(pKingdom.capital);
    pKingdom.setKing(pNewKing);
    WorldLog.logNewKing(pKingdom);
  }
}
