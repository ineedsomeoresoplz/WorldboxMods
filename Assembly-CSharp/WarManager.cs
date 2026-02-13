// Decompiled with JetBrains decompiler
// Type: WarManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WarManager : MetaSystemManager<War, WarData>
{
  public KingdomCheckCache cache_war_check = new KingdomCheckCache();
  private List<War> _end_wars = new List<War>();

  public WarManager() => this.type_id = "war";

  protected override void updateDirtyUnits()
  {
  }

  public override void clear()
  {
    base.clear();
    this.warStateChanged();
  }

  public override void checkDeadObjects()
  {
    if (this.Count <= 20)
      return;
    foreach (War war in (CoreSystemManager<War, WarData>) this)
    {
      if (!war.isFavorite() && war.hasEnded())
        this._end_wars.Add(war);
    }
    if (this._end_wars.Count == 0)
      return;
    int num1 = this.Count - 20;
    this._end_wars.Sort((Comparison<War>) ((a, b) => a.data.died_time.CompareTo(b.data.died_time)));
    int num2 = Mathf.Min(this._end_wars.Count, num1);
    for (int index = 0; index < num2; ++index)
      this.removeObject(this._end_wars[index]);
    this._end_wars.Clear();
  }

  public void warStateChanged()
  {
    this.cache_war_check.clear();
    Kingdom.cache_enemy_check.clear();
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    int count = this.list.Count;
    while (count-- > 0)
      this.list[count].update();
  }

  public void stopAllWars()
  {
    this._end_wars.AddRange((IEnumerable<War>) this);
    this.endWars(WarWinner.Peace);
  }

  private void endWars(WarWinner pWinner = WarWinner.Nobody)
  {
    if (this._end_wars.Count == 0)
      return;
    foreach (War endWar in this._end_wars)
      this.endWar(endWar, pWinner);
    this._end_wars.Clear();
  }

  public bool isInWarWith(Kingdom pKingdom, Kingdom pKingdomTarget)
  {
    long hash = this.cache_war_check.getHash(pKingdom, pKingdomTarget);
    bool flag;
    if (this.cache_war_check.dict.TryGetValue(hash, out flag))
      return flag;
    foreach (War war in pKingdom.getWars())
    {
      if (war.isInWarWith(pKingdom, pKingdomTarget))
      {
        this.cache_war_check.dict.Add(hash, true);
        return true;
      }
    }
    this.cache_war_check.dict.Add(hash, false);
    return false;
  }

  public War getWar(Kingdom pAttacker, Kingdom pDefender, bool pOnlyMain = true)
  {
    foreach (War war in (CoreSystemManager<War, WarData>) this)
    {
      if (!war.hasEnded() && (war.isMainAttacker(pAttacker) && war.isMainDefender(pDefender) || war.isMainAttacker(pDefender) && war.isMainDefender(pAttacker)))
        return war;
    }
    if (!pOnlyMain)
    {
      foreach (War war in (CoreSystemManager<War, WarData>) this)
      {
        if (!war.hasEnded() && war.isInWarWith(pAttacker, pDefender))
          return war;
      }
    }
    return (War) null;
  }

  public War getForcedWar(Kingdom pKingdom)
  {
    foreach (War forcedWar in (CoreSystemManager<War, WarData>) this)
    {
      if (!forcedWar.hasEnded() && forcedWar.getAsset().forced_war && forcedWar.isMainDefender(pKingdom))
        return forcedWar;
    }
    return (War) null;
  }

  public IEnumerable<War> getWars(Kingdom pKingdom, bool pRandom = false)
  {
    WarManager warManager = this;
    foreach (War war in pRandom ? warManager.list.LoopRandom<War>() : (IEnumerable<War>) warManager)
    {
      if (!war.hasEnded() && war.hasKingdom(pKingdom))
        yield return war;
    }
  }

  public IEnumerable<War> getWars(Alliance pAlliance, bool pRandom = false)
  {
    WarManager warManager = this;
    foreach (War war in pRandom ? warManager.list.LoopRandom<War>() : (IEnumerable<War>) warManager)
    {
      if (!war.hasEnded())
      {
        foreach (Kingdom kingdoms in pAlliance.kingdoms_list)
        {
          if (war.hasKingdom(kingdoms))
          {
            yield return war;
            break;
          }
        }
      }
    }
  }

  public bool hasWars(Kingdom pKingdom)
  {
    using (IEnumerator<War> enumerator = this.getWars(pKingdom).GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        War current = enumerator.Current;
        return true;
      }
    }
    return false;
  }

  public bool hasWars(Alliance pAlliance)
  {
    using (IEnumerator<War> enumerator = this.getWars(pAlliance).GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        War current = enumerator.Current;
        return true;
      }
    }
    return false;
  }

  public bool haveCommonEnemy(Kingdom pKingdom1, Kingdom pKingdom2)
  {
    // ISSUE: unable to decompile the method.
  }

  public War getRandomWarFor(Kingdom pKingdom)
  {
    using (IEnumerator<War> enumerator = this.getWars(pKingdom, true).GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current;
    }
    return (War) null;
  }

  public void getWarCities(Kingdom pKingdom, ListPool<City> pCities)
  {
    using (ListPool<Kingdom> enemiesOf = this.getEnemiesOf(pKingdom))
    {
      for (int index = 0; index < enemiesOf.Count; ++index)
      {
        Kingdom kingdom = enemiesOf[index];
        pCities.AddRange(kingdom.getCities());
      }
    }
  }

  public ListPool<Kingdom> getNeutralKingdoms(
    Kingdom pKingdom,
    bool pOnlyWithoutWars = false,
    bool pOnlyWithoutAlliances = false)
  {
    ListPool<Kingdom> neutralKingdoms = new ListPool<Kingdom>(World.world.kingdoms.Count);
    Alliance alliance1 = pKingdom.getAlliance();
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom != pKingdom && !this.isInWarWith(pKingdom, kingdom) && (!pOnlyWithoutWars || !kingdom.hasEnemies()))
      {
        Alliance alliance2 = kingdom.getAlliance();
        if ((!pOnlyWithoutAlliances || alliance2 == null) && !Alliance.isSame(alliance1, alliance2))
          neutralKingdoms.Add(kingdom);
      }
    }
    return neutralKingdoms;
  }

  public ListPool<Kingdom> getEnemiesOf(Kingdom pKingdom)
  {
    ListPool<Kingdom> enemiesOf = new ListPool<Kingdom>(World.world.kingdoms.Count);
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom != pKingdom && pKingdom.isEnemy(kingdom))
        enemiesOf.Add(kingdom);
    }
    return enemiesOf;
  }

  public void endWar(War pWar, WarWinner pWinner = WarWinner.Nobody)
  {
    if (!pWar.isAlive() || pWar.hasEnded())
      return;
    ++World.world.game_stats.data.peacesMade;
    ++World.world.map_stats.peacesMade;
    pWar.setWinner(pWinner);
    this.warStateChanged();
    WorldLog.logWarEnded(pWar);
    pWar.endForSides(pWinner);
    pWar.data.died_time = World.world.getCurWorldTime();
  }

  public War newWar(Kingdom pAttacker, Kingdom pDefender, WarTypeAsset pType)
  {
    ++World.world.game_stats.data.warsStarted;
    ++World.world.map_stats.warsStarted;
    this.warStateChanged();
    War war = this.newObject();
    war.newWar(pAttacker, pDefender, pType);
    war.data.started_by_actor_name = pAttacker.king?.getName();
    WarData data = war.data;
    Actor king = pAttacker.king;
    long num = king != null ? king.getID() : -1L;
    data.started_by_actor_id = num;
    war.data.started_by_kingdom_name = pAttacker.data.name;
    war.data.started_by_kingdom_id = pAttacker.data.id;
    war.setName(NameGenerator.generateNameFromTemplate(pType.name_template, pKingdom: !pType.kingdom_for_name_attacker ? pDefender : pAttacker));
    WarManager.checkHappinessFromStartOfWar(pAttacker);
    return war;
  }

  public static void checkHappinessFromStartOfWar(Kingdom pKingdom)
  {
    if (!pKingdom.hasCulture() || !pKingdom.culture.hasTrait("happiness_from_war"))
      return;
    for (int index = 0; index < pKingdom.units.Count; ++index)
    {
      Actor unit = pKingdom.units[index];
      if (unit.hasCulture() && unit.culture.hasTrait("happiness_from_war"))
        unit.changeHappiness("just_started_war");
    }
  }

  public long countActiveWars()
  {
    long num = 0;
    foreach (War activeWar in this.getActiveWars())
      ++num;
    return num;
  }

  public IEnumerable<War> getActiveWars()
  {
    foreach (War activeWar in (CoreSystemManager<War, WarData>) this)
    {
      if (!activeWar.hasEnded())
        yield return activeWar;
    }
  }

  public override bool hasAny()
  {
    if (this.Count == 0)
      return false;
    using (IEnumerator<War> enumerator = this.getActiveWars().GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        War current = enumerator.Current;
        return true;
      }
    }
    return false;
  }
}
