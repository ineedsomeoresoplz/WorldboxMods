// Decompiled with JetBrains decompiler
// Type: Alliance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Alliance : MetaObject<AllianceData>
{
  public List<Kingdom> kingdoms_list = new List<Kingdom>();
  public HashSet<Kingdom> kingdoms_hashset = new HashSet<Kingdom>();
  public int power;

  protected override MetaType meta_type => MetaType.Alliance;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.alliances;

  public void createNewAlliance()
  {
    this.setName(NameGenerator.getName("alliance_name"));
    this.generateNewMetaObject();
  }

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this.power = 0;
  }

  public override int countTotalMoney()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countTotalMoney();
    }
    return num;
  }

  public override int countHappyUnits()
  {
    if (this.kingdoms_list.Count == 0)
      return 0;
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countHappyUnits();
    }
    return num;
  }

  public override int countSick()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countSick();
    }
    return num;
  }

  public override int countHungry()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countHungry();
    }
    return num;
  }

  public override int countStarving()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countStarving();
    }
    return num;
  }

  public override int countChildren()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countChildren();
    }
    return num;
  }

  public override int countAdults()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countAdults();
    }
    return num;
  }

  public override int countHomeless()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countHomeless();
    }
    return num;
  }

  public override IEnumerable<Family> getFamilies()
  {
    List<Kingdom> tKingdoms = this.kingdoms_list;
    for (int i = 0; i < tKingdoms.Count; ++i)
    {
      foreach (Family family in tKingdoms[i].getFamilies())
        yield return family;
    }
  }

  public override bool hasFamilies()
  {
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      if (kingdomsList[index].hasFamilies())
        return true;
    }
    return false;
  }

  public override int countMales()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countMales();
    }
    return num;
  }

  public override int countFemales()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countFemales();
    }
    return num;
  }

  public override int countHoused()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countHoused();
    }
    return num;
  }

  public void setType(AllianceType pType) => this.data.alliance_type = pType;

  public bool isForcedType() => this.data.alliance_type == AllianceType.Forced;

  public bool isNormalType() => this.data.alliance_type == AllianceType.Normal;

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.kingdom_colors_library;
  }

  public override void generateBanner()
  {
    this.data.banner_background_id = Randy.randomInt(0, World.world.alliances.getBackgroundsList().Length);
    this.data.banner_icon_id = Randy.randomInt(0, World.world.alliances.getIconsList().Length);
  }

  public void addFounders(Kingdom pKingdom1, Kingdom pKingdom2)
  {
    this.data.founder_kingdom_name = pKingdom1.data.name;
    this.data.founder_kingdom_id = pKingdom1.getID();
    this.data.founder_actor_name = pKingdom1.king?.getName();
    AllianceData data = this.data;
    Actor king = pKingdom1.king;
    long num = king != null ? king.getID() : -1L;
    data.founder_actor_id = num;
    this.join(pKingdom1, pForce: true);
    this.join(pKingdom2, pForce: true);
  }

  public void update()
  {
    this.power = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
      this.power += kingdomsList[index].power;
  }

  public bool checkActive()
  {
    bool flag = false;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = kingdomsList.Count - 1; index >= 0; --index)
    {
      Kingdom pKingdom = kingdomsList[index];
      if (!pKingdom.isAlive())
      {
        this.leave(pKingdom, false);
        this.kingdoms_list.RemoveAt(index);
        flag = true;
      }
    }
    if (flag)
      this.recalculate();
    return this.kingdoms_list.Count >= 2;
  }

  public void dissolve()
  {
    foreach (Kingdom kingdom in this.kingdoms_hashset)
      kingdom.allianceLeave(this);
    this.kingdoms_hashset.Clear();
  }

  public void recalculate()
  {
    this.kingdoms_list.Clear();
    this.kingdoms_list.AddRange((IEnumerable<Kingdom>) this.kingdoms_hashset);
    this.mergeWars();
  }

  public bool canJoin(Kingdom pKingdom)
  {
    foreach (Kingdom pKingdom1 in this.kingdoms_hashset)
    {
      if (!pKingdom.isOpinionTowardsKingdomGood(pKingdom1))
        return false;
    }
    return true;
  }

  public bool join(Kingdom pKingdom, bool pRecalc = true, bool pForce = false)
  {
    if (this.hasKingdom(pKingdom) || !pForce && !this.canJoin(pKingdom))
      return false;
    this.kingdoms_hashset.Add(pKingdom);
    if (this.hasWars())
    {
      if (this.hasWarsWith(pKingdom))
      {
        foreach (War attackerWar in this.getAttackerWars())
        {
          if (attackerWar.isDefender(pKingdom))
            attackerWar.leaveWar(pKingdom);
        }
        foreach (War defenderWar in this.getDefenderWars())
        {
          if (defenderWar.isAttacker(pKingdom))
            defenderWar.leaveWar(pKingdom);
        }
      }
      foreach (War attackerWar in this.getAttackerWars())
        attackerWar.joinAttackers(pKingdom);
      foreach (War defenderWar in this.getDefenderWars())
      {
        if (!defenderWar.isTotalWar())
          defenderWar.joinDefenders(pKingdom);
      }
    }
    if (pKingdom.hasEnemies())
    {
      foreach (War war in pKingdom.getWars())
      {
        if (!war.isTotalWar())
        {
          if (war.isMainAttacker(pKingdom))
          {
            foreach (Kingdom kingdoms in this.kingdoms_list)
              war.joinAttackers(kingdoms);
          }
          if (war.isMainDefender(pKingdom))
          {
            foreach (Kingdom kingdoms in this.kingdoms_list)
              war.joinDefenders(kingdoms);
          }
        }
      }
    }
    pKingdom.allianceJoin(this);
    if (pRecalc)
      this.recalculate();
    this.data.timestamp_member_joined = World.world.getCurWorldTime();
    return true;
  }

  public void leave(Kingdom pKingdom, bool pRecalc = true)
  {
    this.kingdoms_hashset.Remove(pKingdom);
    if (this.hasWars())
    {
      foreach (War attackerWar in this.getAttackerWars())
      {
        if (!attackerWar.isMainAttacker(pKingdom))
        {
          attackerWar.leaveWar(pKingdom);
        }
        else
        {
          foreach (Kingdom pKingdom1 in this.kingdoms_hashset)
            attackerWar.leaveWar(pKingdom1);
        }
      }
      foreach (War defenderWar in this.getDefenderWars())
      {
        if (!defenderWar.isMainDefender(pKingdom))
        {
          defenderWar.leaveWar(pKingdom);
        }
        else
        {
          foreach (Kingdom pKingdom2 in this.kingdoms_hashset)
            defenderWar.leaveWar(pKingdom2);
        }
      }
    }
    pKingdom.allianceLeave(this);
    if (!pRecalc)
      return;
    this.recalculate();
  }

  public override void save()
  {
    base.save();
    this.data.kingdoms = new List<long>();
    foreach (NanoObject nanoObject in this.kingdoms_hashset)
      this.data.kingdoms.Add(nanoObject.id);
  }

  public override void loadData(AllianceData pData)
  {
    base.loadData(pData);
    foreach (long kingdom1 in this.data.kingdoms)
    {
      Kingdom kingdom2 = World.world.kingdoms.get(kingdom1);
      if (kingdom2 != null)
        this.kingdoms_hashset.Add(kingdom2);
    }
    this.recalculate();
  }

  public int countBuildings()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countBuildings();
    }
    return num;
  }

  public int countZones()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countZones();
    }
    return num;
  }

  public override int countUnits() => this.countPopulation();

  public int countPopulation()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.getPopulationPeople();
    }
    return num;
  }

  public int countCities()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countCities();
    }
    return num;
  }

  public int countKingdoms() => this.kingdoms_hashset.Count;

  public string getMotto()
  {
    if (string.IsNullOrEmpty(this.data.motto))
      this.data.motto = NameGenerator.getName("alliance_mottos");
    return this.data.motto;
  }

  public int countWarriors()
  {
    int num = 0;
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom kingdom = kingdomsList[index];
      num += kingdom.countTotalWarriors();
    }
    return num;
  }

  public static bool isSame(Alliance pAlliance1, Alliance pAlliance2)
  {
    return pAlliance1 != null && pAlliance2 != null && pAlliance1 == pAlliance2;
  }

  public bool hasWarsWith(Kingdom pKingdom)
  {
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom pKingdom1 = kingdomsList[index];
      if (pKingdom.isInWarWith(pKingdom1))
        return true;
    }
    return false;
  }

  public bool hasSupremeKingdom()
  {
    return DiplomacyManager.kingdom_supreme != null && this.hasKingdom(DiplomacyManager.kingdom_supreme);
  }

  public bool hasKingdom(Kingdom pKingdom) => this.kingdoms_hashset.Contains(pKingdom);

  public bool hasSharedBordersWithKingdom(Kingdom pKingdom)
  {
    List<Kingdom> kingdomsList = this.kingdoms_list;
    for (int index = 0; index < kingdomsList.Count; ++index)
    {
      Kingdom pTarget = kingdomsList[index];
      if (DiplomacyHelpers.areKingdomsClose(pKingdom, pTarget))
        return true;
    }
    return false;
  }

  public bool hasWars() => World.world.wars.hasWars(this);

  public IEnumerable<War> getWars(bool pRandom = false) => World.world.wars.getWars(this, pRandom);

  public void mergeWars()
  {
    if (!this.hasWars())
      return;
    using (ListPool<War> listPool = new ListPool<War>(this.getWars()))
    {
      for (int index1 = 0; index1 < listPool.Count; ++index1)
      {
        War pWar1 = listPool[index1];
        if (!pWar1.hasEnded())
        {
          for (int index2 = index1 + 1; index2 < listPool.Count; ++index2)
          {
            War pWar2 = listPool[index2];
            if (!pWar2.hasEnded() && pWar1.isSameAs(pWar2))
            {
              if (pWar1.data.created_time < pWar2.data.created_time)
                World.world.wars.endWar(pWar2, WarWinner.Merged);
              else
                World.world.wars.endWar(pWar1, WarWinner.Merged);
              this.mergeWars();
              return;
            }
          }
        }
      }
    }
  }

  public IEnumerable<War> getAttackerWars()
  {
    foreach (War war in this.getWars())
    {
      foreach (Kingdom kingdoms in this.kingdoms_list)
      {
        if (war.isAttacker(kingdoms))
        {
          yield return war;
          break;
        }
      }
    }
  }

  public IEnumerable<War> getDefenderWars()
  {
    foreach (War war in this.getWars())
    {
      foreach (Kingdom kingdoms in this.kingdoms_list)
      {
        if (war.isDefender(kingdoms))
        {
          yield return war;
          break;
        }
      }
    }
  }

  public override IEnumerable<Actor> getUnits()
  {
    List<Kingdom> tKingdoms = this.kingdoms_list;
    for (int i = 0; i < tKingdoms.Count; ++i)
    {
      foreach (Actor unit in tKingdoms[i].getUnits())
        yield return unit;
    }
  }

  public override bool isReadyForRemoval() => false;

  public override Actor getRandomUnit() => this.kingdoms_list.GetRandom<Kingdom>().getRandomUnit();

  public Sprite getBackgroundSprite()
  {
    return World.world.alliances.getBackgroundsList()[this.data.banner_background_id];
  }

  public Sprite getIconSprite() => World.world.alliances.getIconsList()[this.data.banner_icon_id];

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "alliance");
    this.kingdoms_list.Clear();
    this.kingdoms_hashset.Clear();
    base.Dispose();
  }
}
