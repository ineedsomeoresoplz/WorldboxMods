// Decompiled with JetBrains decompiler
// Type: War
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class War : MetaObject<WarData>
{
  private readonly List<Kingdom> _list_attackers = new List<Kingdom>();
  private readonly List<Kingdom> _list_defenders = new List<Kingdom>();
  private readonly HashSet<Kingdom> _hashset_attackers = new HashSet<Kingdom>();
  private readonly HashSet<Kingdom> _hashset_defenders = new HashSet<Kingdom>();
  private WarTypeAsset _asset;

  protected override MetaType meta_type => MetaType.War;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.wars;

  [CanBeNull]
  public Kingdom main_attacker => this.getMainAttacker();

  [CanBeNull]
  public Kingdom main_defender => this.getMainDefender();

  [CanBeNull]
  public Kingdom getMainAttacker()
  {
    return World.world.kingdoms.get(this.data.main_attacker) ?? (Kingdom) World.world.kingdoms.db_get(this.data.main_attacker);
  }

  [CanBeNull]
  public Kingdom getMainDefender()
  {
    return World.world.kingdoms.get(this.data.main_defender) ?? (Kingdom) World.world.kingdoms.db_get(this.data.main_defender);
  }

  public bool isMainAttacker(Kingdom pKingdom) => pKingdom.getID() == this.data.main_attacker;

  public bool isMainDefender(Kingdom pKingdom) => pKingdom.getID() == this.data.main_defender;

  protected sealed override void setDefaultValues() => base.setDefaultValues();

  public override ColorAsset getColor()
  {
    Kingdom mainAttacker = this.getMainAttacker();
    if (!mainAttacker.isRekt())
      return mainAttacker.getColor();
    using (IEnumerator<Kingdom> enumerator = (this.hasEnded() ? this.getAllAttackers() : this.getAttackers()).GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current.getColor();
    }
    Kingdom mainDefender = this.getMainDefender();
    if (!mainDefender.isRekt())
      return mainDefender.getColor();
    using (IEnumerator<Kingdom> enumerator = (this.hasEnded() ? this.getAllDefenders() : this.getDefenders()).GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current.getColor();
    }
    return (ColorAsset) null;
  }

  public WarTypeAsset getAsset()
  {
    if (this._asset == null)
      this._asset = AssetManager.war_types_library.get(this.data.war_type);
    return this._asset;
  }

  public void newWar(Kingdom pAttacker, Kingdom pDefender, WarTypeAsset pAsset)
  {
    this.data.main_attacker = pAttacker.id;
    if (pDefender != null)
      this.data.main_defender = pDefender.id;
    this._asset = pAsset;
    this.data.war_type = pAsset.id;
    this.joinAttackers(pAttacker);
    if (pDefender != null)
      this.joinDefenders(pDefender);
    this.prepare();
  }

  public override void clearLastYearStats() => this.addRenown(1);

  public override void increaseBirths() => throw new NotImplementedException(this.GetType().Name);

  public override void increaseKills() => throw new NotImplementedException(this.GetType().Name);

  public void increaseDeathsDefenders(AttackType pAttackType)
  {
    ++this.data.dead_defenders;
    this.increaseDeaths(pAttackType);
    this.addRenown(1);
  }

  public void increaseDeathsAttackers(AttackType pAttackType)
  {
    ++this.data.dead_attackers;
    this.increaseDeaths(pAttackType);
    this.addRenown(1);
  }

  public void leaveWar(Kingdom pKingdom)
  {
    this.addRenown(25);
    this.removeFromWar(pKingdom, true);
  }

  public void lostWar(Kingdom pKingdom)
  {
    this.addRenown(50);
    this.removeFromWar(pKingdom, false);
    this.update();
  }

  internal void removeFromWar(Kingdom pKingdom, bool pInPeace)
  {
    if (this.isAttacker(pKingdom))
    {
      foreach (Kingdom hashsetDefender in this._hashset_defenders)
        World.world.diplomacy.getRelation(pKingdom, hashsetDefender).data.timestamp_last_war_ended = World.world.getCurWorldTime();
    }
    else
    {
      foreach (Kingdom hashsetAttacker in this._hashset_attackers)
        World.world.diplomacy.getRelation(pKingdom, hashsetAttacker).data.timestamp_last_war_ended = World.world.getCurWorldTime();
    }
    this.removeAttacker(pKingdom, pInPeace);
    this.removeDefender(pKingdom, pInPeace);
    if (this.isMainAttacker(pKingdom) && !this.trySelectNewMainAttacker())
      World.world.wars.endWar(this, pInPeace ? WarWinner.Peace : WarWinner.Defenders);
    else if (this.isMainDefender(pKingdom) && !this.trySelectNewMainDefender())
    {
      World.world.wars.endWar(this, pInPeace ? WarWinner.Peace : WarWinner.Attackers);
    }
    else
    {
      pKingdom.checkEndWar();
      if (pInPeace)
        pKingdom.madePeace(this);
      else
        pKingdom.lostWar(this);
    }
    this.prepare();
  }

  public int countAttackers() => this._list_attackers.Count;

  public bool trySelectNewMainAttacker()
  {
    if (this.countAttackers() <= 1)
      return false;
    this._list_attackers.Sort(new Comparison<Kingdom>(KingdomListComponent.sortByArmy));
    foreach (Kingdom listAttacker in this._list_attackers)
    {
      if (listAttacker.id != this.data.main_attacker)
      {
        this.data.main_attacker = listAttacker.id;
        return true;
      }
    }
    return false;
  }

  public bool trySelectNewMainDefender()
  {
    if (this.countDefenders() <= 1)
      return false;
    this._list_defenders.Sort(new Comparison<Kingdom>(KingdomListComponent.sortByArmy));
    foreach (Kingdom listDefender in this._list_defenders)
    {
      if (listDefender.id != this.data.main_defender)
      {
        this.data.main_defender = listDefender.id;
        return true;
      }
    }
    return false;
  }

  public IEnumerable<Kingdom> getAttackers()
  {
    return this.hasEnded() ? this.getHistoricAttackers() : (IEnumerable<Kingdom>) this._list_attackers;
  }

  public IEnumerable<Kingdom> getHistoricAttackers()
  {
    foreach (long listAttacker in this.data.list_attackers)
    {
      Kingdom historicAttacker1 = World.world.kingdoms.get(listAttacker);
      if (historicAttacker1 != null)
      {
        yield return historicAttacker1;
      }
      else
      {
        DeadKingdom historicAttacker2 = World.world.kingdoms.db_get(listAttacker);
        if (historicAttacker2 != null)
          yield return (Kingdom) historicAttacker2;
      }
    }
  }

  public IEnumerable<Kingdom> getAllAttackers()
  {
    foreach (Kingdom attacker in this.getAttackers())
      yield return attacker;
    foreach (Kingdom pastAttacker in this.getPastAttackers())
      yield return pastAttacker;
    foreach (Kingdom diedAttacker in this.getDiedAttackers())
      yield return diedAttacker;
  }

  public IEnumerable<Kingdom> getAllDefenders()
  {
    foreach (Kingdom defender in this.getDefenders())
      yield return defender;
    foreach (Kingdom pastDefender in this.getPastDefenders())
      yield return pastDefender;
    foreach (Kingdom diedDefender in this.getDiedDefenders())
      yield return diedDefender;
  }

  public IEnumerable<Kingdom> getPastAttackers()
  {
    foreach (long pastAttacker1 in this.data.past_attackers)
    {
      Kingdom pastAttacker2 = World.world.kingdoms.get(pastAttacker1);
      if (pastAttacker2 != null)
      {
        yield return pastAttacker2;
      }
      else
      {
        DeadKingdom pastAttacker3 = World.world.kingdoms.db_get(pastAttacker1);
        if (pastAttacker3 != null)
          yield return (Kingdom) pastAttacker3;
      }
    }
  }

  public IEnumerable<Kingdom> getDiedAttackers()
  {
    foreach (long diedAttacker1 in this.data.died_attackers)
    {
      Kingdom diedAttacker2 = World.world.kingdoms.get(diedAttacker1);
      if (diedAttacker2 != null)
      {
        yield return diedAttacker2;
      }
      else
      {
        DeadKingdom diedAttacker3 = World.world.kingdoms.db_get(diedAttacker1);
        if (diedAttacker3 != null)
          yield return (Kingdom) diedAttacker3;
      }
    }
  }

  public IEnumerable<Kingdom> getActiveParties()
  {
    bool tAttackersFirst = Randy.randomBool();
    foreach (Kingdom activeParty in tAttackersFirst ? this.getAttackers() : this.getDefenders())
    {
      if (activeParty.isAlive())
        yield return activeParty;
    }
    foreach (Kingdom activeParty in tAttackersFirst ? this.getDefenders() : this.getAttackers())
    {
      if (activeParty.isAlive())
        yield return activeParty;
    }
  }

  public string getAttackersColorTextString()
  {
    Kingdom mainAttacker = this.getMainAttacker();
    if (mainAttacker != null)
      return mainAttacker.getColor().color_text;
    using (IEnumerator<Kingdom> enumerator = (this.hasEnded() ? this.getAllAttackers() : this.getAttackers()).GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current.getColor().color_text;
    }
    return "#F3961F";
  }

  public string getDefendersColorTextString()
  {
    if (this.isTotalWar())
      return "#F3961F";
    Kingdom mainDefender = this.getMainDefender();
    return mainDefender != null ? mainDefender.getColor().color_text : "#F3961F";
  }

  public int countDefenders()
  {
    return !this.isTotalWar() ? this._list_defenders.Count : World.world.kingdoms.Count - 1;
  }

  public IEnumerable<Kingdom> getDefenders()
  {
    if (this.hasEnded())
    {
      foreach (Kingdom historicDefender in this.getHistoricDefenders())
        yield return historicDefender;
    }
    else if (!this.isTotalWar())
    {
      foreach (Kingdom listDefender in this._list_defenders)
        yield return listDefender;
    }
    else
    {
      foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      {
        if (!this.isMainAttacker(kingdom))
          yield return kingdom;
      }
    }
  }

  public IEnumerable<Kingdom> getHistoricDefenders()
  {
    foreach (long listDefender in this.data.list_defenders)
    {
      Kingdom historicDefender1 = World.world.kingdoms.get(listDefender);
      if (historicDefender1 != null)
      {
        yield return historicDefender1;
      }
      else
      {
        DeadKingdom historicDefender2 = World.world.kingdoms.db_get(listDefender);
        if (historicDefender2 != null)
          yield return (Kingdom) historicDefender2;
      }
    }
  }

  public IEnumerable<Kingdom> getPastDefenders()
  {
    foreach (long pastDefender1 in this.data.past_defenders)
    {
      Kingdom pastDefender2 = World.world.kingdoms.get(pastDefender1);
      if (pastDefender2 != null)
      {
        yield return pastDefender2;
      }
      else
      {
        DeadKingdom pastDefender3 = World.world.kingdoms.db_get(pastDefender1);
        if (pastDefender3 != null)
          yield return (Kingdom) pastDefender3;
      }
    }
  }

  public IEnumerable<Kingdom> getDiedDefenders()
  {
    foreach (long diedDefender1 in this.data.died_defenders)
    {
      Kingdom diedDefender2 = World.world.kingdoms.get(diedDefender1);
      if (diedDefender2 != null)
      {
        yield return diedDefender2;
      }
      else
      {
        DeadKingdom diedDefender3 = World.world.kingdoms.db_get(diedDefender1);
        if (diedDefender3 != null)
          yield return (Kingdom) diedDefender3;
      }
    }
  }

  public void update()
  {
    if (this.hasEnded())
      return;
    if (!this.main_attacker.isAlive())
    {
      this.lostWar(this.main_attacker);
    }
    else
    {
      if (this.isTotalWar())
      {
        if (World.world.kingdoms.Count <= 1)
        {
          World.world.wars.endWar(this, WarWinner.Attackers);
          return;
        }
      }
      else if (!this.main_defender.isAlive())
      {
        this.lostWar(this.main_defender);
        return;
      }
      if (this.getAge() > 10 && !this.isTotalWar())
      {
        if (this.main_attacker.countCities() == 0)
        {
          this.lostWar(this.main_attacker);
          return;
        }
        if (this.main_defender.countCities() == 0)
        {
          this.lostWar(this.main_defender);
          return;
        }
      }
      for (int index = 0; index < this._list_attackers.Count; ++index)
      {
        Kingdom listAttacker = this._list_attackers[index];
        if (!listAttacker.isAlive())
        {
          this.lostWar(listAttacker);
          return;
        }
      }
      if (!this.isTotalWar())
      {
        for (int index = 0; index < this._list_defenders.Count; ++index)
        {
          Kingdom listDefender = this._list_defenders[index];
          if (!listDefender.isAlive())
          {
            this.lostWar(listDefender);
            return;
          }
        }
      }
      if (this.isTotalWar())
      {
        if (this._list_attackers.Count != 0 && World.world.kingdoms.Count != 1)
          return;
        Debug.LogError((object) "[1] should never happen here");
      }
      else
      {
        if (this._list_attackers.Count != 0 && this._list_defenders.Count != 0)
          return;
        Debug.LogError((object) "[2] should never happen here");
      }
    }
  }

  public bool isAttacker(Kingdom pKingdom) => this._hashset_attackers.Contains(pKingdom);

  public bool isDefender(Kingdom pKingdom)
  {
    return this.isTotalWar() && !this.isMainAttacker(pKingdom) || this._hashset_defenders.Contains(pKingdom);
  }

  public List<Kingdom> getOppositeSideKingdom(Kingdom pKingdom)
  {
    if (this.isAttacker(pKingdom))
      return this._list_defenders;
    return this.isDefender(pKingdom) ? this._list_attackers : (List<Kingdom>) null;
  }

  public bool isTotalWar() => this.getAsset().total_war;

  public bool isInWarWith(Kingdom pKingdom, Kingdom pTarget)
  {
    return this.isTotalWar() ? this.isMainAttacker(pKingdom) || this.isMainAttacker(pTarget) : this.isAttacker(pKingdom) && this.isDefender(pTarget) || this.isDefender(pKingdom) && this.isAttacker(pTarget);
  }

  public bool onTheSameSide(Kingdom pKingdom1, Kingdom pKingdom2)
  {
    return this.isAttacker(pKingdom1) && this.isAttacker(pKingdom2) || this.isDefender(pKingdom1) && this.isDefender(pKingdom2);
  }

  public bool hasKingdom(Kingdom pKingdom)
  {
    return this.isTotalWar() || this.isAttacker(pKingdom) || this.isDefender(pKingdom);
  }

  public void joinAttackers(Kingdom pKingdom)
  {
    if (this.data.list_attackers.Contains(pKingdom.id))
      return;
    this.addRenown(5);
    this.data.past_attackers.Remove(pKingdom.id);
    this.data.list_attackers.Add(pKingdom.id);
    this.prepare();
  }

  public void joinDefenders(Kingdom pKingdom)
  {
    if (this.isTotalWar() || this.data.list_defenders.Contains(pKingdom.id))
      return;
    this.addRenown(5);
    this.data.past_defenders.Remove(pKingdom.id);
    this.data.list_defenders.Add(pKingdom.id);
    this.prepare();
  }

  public override void loadData(WarData pData)
  {
    base.loadData(pData);
    this.prepare();
  }

  public void prepare()
  {
    this._list_attackers.Clear();
    this._list_defenders.Clear();
    this._hashset_attackers.Clear();
    this._hashset_defenders.Clear();
    if (this.data.died_attackers == null)
      this.data.died_attackers = new List<long>();
    if (this.data.died_defenders == null)
      this.data.died_defenders = new List<long>();
    if (this.data.past_attackers == null)
      this.data.past_attackers = new List<long>();
    if (this.data.past_defenders == null)
      this.data.past_defenders = new List<long>();
    foreach (long listAttacker in this.data.list_attackers)
    {
      Kingdom kingdom = World.world.kingdoms.get(listAttacker);
      if (kingdom != null)
      {
        this._list_attackers.Add(kingdom);
        this._hashset_attackers.Add(kingdom);
      }
    }
    foreach (long listDefender in this.data.list_defenders)
    {
      Kingdom kingdom = World.world.kingdoms.get(listDefender);
      if (kingdom != null)
      {
        this._list_defenders.Add(kingdom);
        this._hashset_defenders.Add(kingdom);
      }
    }
    World.world.wars.warStateChanged();
  }

  public int getDeadAttackers() => this.data.dead_attackers;

  public int getDeadDefenders() => this.data.dead_defenders;

  public void endForSides(WarWinner pWinner)
  {
    foreach (Kingdom hashsetAttacker in this._hashset_attackers)
    {
      hashsetAttacker.checkEndWar();
      switch (pWinner)
      {
        case WarWinner.Attackers:
          hashsetAttacker.wonWar(this);
          continue;
        case WarWinner.Defenders:
          hashsetAttacker.lostWar(this);
          continue;
        case WarWinner.Peace:
          hashsetAttacker.madePeace(this);
          continue;
        default:
          continue;
      }
    }
    foreach (Kingdom hashsetDefender in this._hashset_defenders)
    {
      hashsetDefender.checkEndWar();
      switch (pWinner)
      {
        case WarWinner.Attackers:
          hashsetDefender.lostWar(this);
          continue;
        case WarWinner.Defenders:
          hashsetDefender.wonWar(this);
          continue;
        case WarWinner.Peace:
          hashsetDefender.madePeace(this);
          continue;
        default:
          continue;
      }
    }
    if (pWinner == WarWinner.Merged)
      return;
    foreach (Kingdom hashsetAttacker in this._hashset_attackers)
    {
      foreach (Kingdom hashsetDefender in this._hashset_defenders)
        World.world.diplomacy.getRelation(hashsetAttacker, hashsetDefender).data.timestamp_last_war_ended = World.world.getCurWorldTime();
    }
  }

  public int countKingdoms()
  {
    return this.isTotalWar() ? World.world.kingdoms.Count : 0 + this.countAttackers() + this.countDefenders();
  }

  public int countCities() => this.countAttackersCities() + this.countDefendersCities();

  public int countAttackersCities()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllAttackers() : this.getAttackers())
      num += kingdom.countCities();
    return num;
  }

  public int countDefendersCities()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllDefenders() : this.getDefenders())
      num += kingdom.countCities();
    return num;
  }

  public int countDefendersPopulation()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllDefenders() : this.getDefenders())
      num += kingdom.getPopulationPeople();
    return num;
  }

  public int countDefendersWarriors()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllDefenders() : this.getDefenders())
      num += kingdom.countTotalWarriors();
    return num;
  }

  public int countDefendersMoney()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllDefenders() : this.getDefenders())
      num += kingdom.countTotalMoney();
    return num;
  }

  public int countAttackersPopulation()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllAttackers() : this.getAttackers())
      num += kingdom.getPopulationPeople();
    return num;
  }

  public int countAttackersWarriors()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllAttackers() : this.getAttackers())
      num += kingdom.countTotalWarriors();
    return num;
  }

  public int countAttackersMoney()
  {
    int num = 0;
    foreach (Kingdom kingdom in this.hasEnded() ? this.getAllAttackers() : this.getAttackers())
      num += kingdom.countTotalMoney();
    return num;
  }

  public int countTotalPopulation()
  {
    return this.countAttackersPopulation() + this.countDefendersPopulation();
  }

  public int countTotalArmy() => this.countAttackersWarriors() + this.countDefendersWarriors();

  public override int countUnits() => this.countTotalPopulation();

  public override IEnumerable<Actor> getUnits()
  {
    foreach (MetaObject<KingdomData> attacker in this.getAttackers())
    {
      foreach (Actor unit in attacker.getUnits())
        yield return unit;
    }
    foreach (MetaObject<KingdomData> defender in this.getDefenders())
    {
      foreach (Actor unit in defender.getUnits())
        yield return unit;
    }
  }

  public override Actor getRandomUnit()
  {
    using (ListPool<Kingdom> list = new ListPool<Kingdom>(this.getActiveParties()))
    {
      foreach (MetaObject<KingdomData> metaObject in list.LoopRandom<Kingdom>())
      {
        Actor randomUnit = metaObject.getRandomUnit();
        if (randomUnit != null)
          return randomUnit;
      }
      return (Actor) null;
    }
  }

  public override bool isReadyForRemoval() => false;

  public bool hasEnded() => !this.isAlive() || this.hasDied();

  public bool isSameAs(War pWar)
  {
    return !this.hasEnded() && pWar != null && !pWar.hasEnded() && (this._hashset_attackers.SetEquals((IEnumerable<Kingdom>) pWar._hashset_attackers) || this._hashset_defenders.SetEquals((IEnumerable<Kingdom>) pWar._hashset_attackers)) && (this._hashset_defenders.SetEquals((IEnumerable<Kingdom>) pWar._hashset_defenders) || this._hashset_attackers.SetEquals((IEnumerable<Kingdom>) pWar._hashset_defenders));
  }

  public int getYearEnded() => Date.getYear(this.data.died_time);

  public int getYearStarted() => Date.getYear(this.data.created_time);

  public int getDuration()
  {
    return this.hasEnded() ? this.getYearEnded() - this.getYearStarted() : Date.getYearsSince(this.data.created_time);
  }

  public void setWinner(WarWinner pWinner)
  {
    if (pWinner == WarWinner.Nobody)
      return;
    this.data.winner = pWinner;
  }

  public void removeAttacker(Kingdom pKingdom, bool pInPeace)
  {
    if (!this.data.list_attackers.Contains(pKingdom.id))
      return;
    this.data.list_attackers.Remove(pKingdom.id);
    if (!pInPeace || !pKingdom.isAlive())
      this.data.died_attackers.Add(pKingdom.id);
    else
      this.data.past_attackers.Add(pKingdom.id);
  }

  public void removeDefender(Kingdom pKingdom, bool pInPeace)
  {
    if (!this.data.list_defenders.Contains(pKingdom.id))
      return;
    this.data.list_defenders.Remove(pKingdom.id);
    if (!pInPeace || !pKingdom.isAlive())
      this.data.died_defenders.Add(pKingdom.id);
    else
      this.data.past_defenders.Add(pKingdom.id);
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "war");
    this._list_attackers.Clear();
    this._list_defenders.Clear();
    this._hashset_attackers.Clear();
    this._hashset_defenders.Clear();
    this._asset = (WarTypeAsset) null;
    base.Dispose();
  }

  public override string ToString()
  {
    string str = "War: " + (this.isAlive() ? "alive " : "dead ");
    if (this.isAlive())
      str = $"{$"{$"{$"{str}{this.id.ToString()} " + " a:" + string.Join<long>(",", this.getAttackers().Select<Kingdom, long>((Func<Kingdom, long>) (tKingdom => tKingdom.id))) + " d:" + string.Join<long>(",", this.getDefenders().Select<Kingdom, long>((Func<Kingdom, long>) (tKingdom => tKingdom.id)))} t:{this.data.war_type}"} w:{this.data.winner.ToString()}"} e:{this.hasEnded().ToString()}";
    return str;
  }
}
