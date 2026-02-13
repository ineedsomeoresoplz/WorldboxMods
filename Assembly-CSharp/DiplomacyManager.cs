// Decompiled with JetBrains decompiler
// Type: DiplomacyManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class DiplomacyManager : CoreSystemManager<DiplomacyRelation, DiplomacyRelationData>
{
  public static Kingdom kingdom_supreme;
  public static Kingdom kingdom_second;
  public static List<Kingdom> superpowers = new List<Kingdom>();
  private float diplomacyTick;
  private static List<Kingdom> _kingdom_sorter = new List<Kingdom>();
  private static List<DiplomacyRelation> _relations_remover = new List<DiplomacyRelation>();
  protected readonly Dictionary<string, DiplomacyRelation> _dict = new Dictionary<string, DiplomacyRelation>();

  public DiplomacyManager() => this.type_id = "diplomacy";

  public override List<DiplomacyRelationData> save(List<DiplomacyRelation> pList = null)
  {
    List<DiplomacyRelationData> diplomacyRelationDataList = new List<DiplomacyRelationData>();
    foreach (DiplomacyRelation diplomacyRelation in (CoreSystemManager<DiplomacyRelation, DiplomacyRelationData>) this)
    {
      diplomacyRelation.kingdom1 = World.world.kingdoms.get(diplomacyRelation.data.kingdom1_id);
      diplomacyRelation.kingdom2 = World.world.kingdoms.get(diplomacyRelation.data.kingdom2_id);
      if (diplomacyRelation.kingdom1 != null && diplomacyRelation.kingdom2 != null)
        diplomacyRelationDataList.Add(diplomacyRelation.data);
    }
    return diplomacyRelationDataList;
  }

  public override void loadFromSave(List<DiplomacyRelationData> pList)
  {
    for (int index = 0; index < pList.Count; ++index)
    {
      DiplomacyRelationData p = pList[index];
      Kingdom kingdom1 = World.world.kingdoms.get(p.kingdom1_id);
      Kingdom kingdom2 = World.world.kingdoms.get(p.kingdom2_id);
      if (kingdom1 != null && kingdom2 != null)
      {
        if (p.id == -1L)
          p.id = World.world.map_stats.getNextId(this.type_id);
        this.loadObject(p);
      }
    }
  }

  public override DiplomacyRelation loadObject(DiplomacyRelationData pData)
  {
    Kingdom kingdom1 = World.world.kingdoms.get(pData.kingdom1_id);
    Kingdom kingdom2 = World.world.kingdoms.get(pData.kingdom2_id);
    pData.rel_id = $"{pData.kingdom1_id.ToString()}_{pData.kingdom2_id.ToString()}";
    DiplomacyRelation diplomacyRelation = base.loadObject(pData);
    this._dict.Add(pData.rel_id, diplomacyRelation);
    diplomacyRelation.kingdom1 = kingdom1;
    diplomacyRelation.kingdom2 = kingdom2;
    return diplomacyRelation;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (World.world.isPaused())
      return;
    if ((double) this.diplomacyTick > 0.0)
    {
      this.diplomacyTick -= pElapsed;
    }
    else
    {
      if (World.world.cities.isLocked())
        return;
      this.diplomacyTick = 2f;
      this.newDiplomacyTick();
    }
  }

  public void newDiplomacyTick()
  {
    this.findSupremeKingdom();
    this.checkAchievements();
  }

  private void checkAchievements() => AchievementLibrary.world_war.check();

  private void findSupremeKingdom()
  {
    DiplomacyManager.kingdom_supreme = (Kingdom) null;
    DiplomacyManager.kingdom_second = (Kingdom) null;
    if (World.world.kingdoms.Count == 0)
      return;
    List<Kingdom> kingdomSorter = DiplomacyManager._kingdom_sorter;
    kingdomSorter.AddRange((IEnumerable<Kingdom>) World.world.kingdoms);
    for (int index = 0; index < kingdomSorter.Count; ++index)
    {
      Kingdom kingdom = kingdomSorter[index];
      kingdom.power = kingdom.countTotalWarriors() * 2 + kingdom.countCities() * 5 + 1;
    }
    kingdomSorter.Sort(new Comparison<Kingdom>(this.sortByPower));
    DiplomacyManager.kingdom_supreme = kingdomSorter[0];
    DiplomacyManager.kingdom_second = kingdomSorter.Count <= 1 ? (Kingdom) null : kingdomSorter[1];
    kingdomSorter.Clear();
  }

  public int sortByPower(Kingdom o1, Kingdom o2) => o2.power.CompareTo(o1.power);

  private War startTotalWar(Kingdom pAttacker, WarTypeAsset pType)
  {
    // ISSUE: unable to decompile the method.
  }

  internal War startWar(Kingdom pAttacker, Kingdom pDefender, WarTypeAsset pAsset, bool pLog = true)
  {
    if (pAsset.total_war)
      return this.startTotalWar(pAttacker, pAsset);
    if (pAttacker == pDefender)
      return (War) null;
    if (World.world.wars.getWar(pAttacker, pDefender) != null)
      return (War) null;
    if (pLog)
      WorldLog.logNewWar(pAttacker, pDefender);
    War war = World.world.wars.newWar(pAttacker, pDefender, pAsset);
    if (pAsset.alliance_join)
    {
      Alliance alliance1 = pAttacker.getAlliance();
      Alliance alliance2 = pDefender.getAlliance();
      if (alliance1 != null)
      {
        foreach (Kingdom pKingdom in alliance1.kingdoms_hashset)
          war.joinAttackers(pKingdom);
      }
      if (alliance2 != null)
      {
        foreach (Kingdom pKingdom in alliance2.kingdoms_hashset)
          war.joinDefenders(pKingdom);
      }
    }
    return war;
  }

  public void eventSpite(Kingdom pKingdom)
  {
    // ISSUE: unable to decompile the method.
  }

  public void eventFriendship(Kingdom pKingdom)
  {
    // ISSUE: unable to decompile the method.
  }

  public KingdomOpinion getOpinion(Kingdom k1, Kingdom k2)
  {
    return this.getRelation(k1, k2).getOpinion(k1, k2);
  }

  public int sortID(Kingdom o1, Kingdom o2) => o1.id.CompareTo(o2.id);

  public DiplomacyRelation getRelation(Kingdom pK1, Kingdom pK2)
  {
    Kingdom kingdom1;
    Kingdom kingdom2;
    if (pK1.id.CompareTo(pK2.id) > 0)
    {
      kingdom1 = pK1;
      kingdom2 = pK2;
    }
    else
    {
      kingdom1 = pK2;
      kingdom2 = pK1;
    }
    long id = kingdom1.id;
    string str1 = id.ToString();
    id = kingdom2.id;
    string str2 = id.ToString();
    string str3 = $"{str1}_{str2}";
    DiplomacyRelation pObject;
    if (this.tryGet(str3, out pObject))
      return pObject;
    pObject = this.newObject();
    pObject.data.rel_id = str3;
    this._dict.Add(str3, pObject);
    pObject.data.kingdom1_id = kingdom1.id;
    pObject.data.kingdom2_id = kingdom2.id;
    pObject.kingdom1 = kingdom1;
    pObject.kingdom2 = kingdom2;
    return pObject;
  }

  public void removeRelationsFor(Kingdom pKingdom)
  {
    foreach (DiplomacyRelation diplomacyRelation in (CoreSystemManager<DiplomacyRelation, DiplomacyRelationData>) this)
    {
      if (diplomacyRelation.kingdom1 == pKingdom || diplomacyRelation.kingdom2 == pKingdom)
        DiplomacyManager._relations_remover.Add(diplomacyRelation);
    }
    foreach (DiplomacyRelation pObject in DiplomacyManager._relations_remover)
      this.removeObject(pObject);
    DiplomacyManager._relations_remover.Clear();
  }

  public bool tryGet(string pID, out DiplomacyRelation pObject)
  {
    return this._dict.TryGetValue(pID, out pObject);
  }

  public DiplomacyRelation get(string pID)
  {
    if (string.IsNullOrEmpty(pID))
      return (DiplomacyRelation) null;
    DiplomacyRelation pObject;
    this.tryGet(pID, out pObject);
    return pObject;
  }

  public override void removeObject(DiplomacyRelation pObject)
  {
    this._dict.Remove(pObject.data.rel_id);
    base.removeObject(pObject);
  }

  public override void clear()
  {
    this.diplomacyTick = 0.0f;
    DiplomacyManager.kingdom_supreme = (Kingdom) null;
    DiplomacyManager.kingdom_second = (Kingdom) null;
    this._dict.Clear();
    base.clear();
  }
}
