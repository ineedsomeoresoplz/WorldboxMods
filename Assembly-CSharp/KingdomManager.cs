// Decompiled with JetBrains decompiler
// Type: KingdomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using JetBrains.Annotations;
using SQLite;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KingdomManager : MetaSystemManager<Kingdom, KingdomData>
{
  private bool _dirty_cities = true;
  private bool _dirty_buildings = true;
  protected readonly Dictionary<long, DeadKingdom> _dead_kingdoms = new Dictionary<long, DeadKingdom>();

  public KingdomManager() => this.type_id = "kingdom";

  public Kingdom makeNewCivKingdom(Actor pActor, string pID = null, bool pLog = true)
  {
    ++World.world.game_stats.data.kingdomsCreated;
    ++World.world.map_stats.kingdomsCreated;
    Kingdom pKingdom = this.newObject();
    pKingdom.newCivKingdom(pActor);
    pActor.stopBeingWarrior();
    pActor.joinKingdom(pKingdom);
    pKingdom.setKing(pActor);
    pKingdom.location = Vector2.op_Implicit(pActor.current_position);
    if (pLog)
      WorldLog.logNewKingdom(pKingdom);
    return pKingdom;
  }

  protected override void addObject(Kingdom pObject)
  {
    base.addObject(pObject);
    World.world.zone_calculator?.setDrawnZonesDirty();
    pObject.createAI();
  }

  public override void removeObject(Kingdom pKingdom)
  {
    // ISSUE: unable to decompile the method.
  }

  public Kingdom getCivOrWildViaID(long pID)
  {
    return pID < 0L ? World.world.kingdoms_wild.get(pID) : World.world.kingdoms.get(pID);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    foreach (MetaObject<KingdomData> metaObject in (CoreSystemManager<Kingdom, KingdomData>) this)
      metaObject.clearCursorOver();
    if (World.world.isPaused())
      return;
    this.updateCivKingdoms(pElapsed);
  }

  private void updateCivKingdoms(float pElapsed)
  {
    int index = 0;
    for (int count = this.list.Count; index < count; ++index)
      this.list[index].updateCiv(pElapsed);
  }

  public void updateAge()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) this)
      kingdom.updateAge();
  }

  [CanBeNull]
  public DeadKingdom db_get(long pID)
  {
    if (Config.disable_db)
      return (DeadKingdom) null;
    DeadKingdom deadKingdom;
    if (this._dead_kingdoms.TryGetValue(pID, out deadKingdom))
      return deadKingdom;
    SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
    using (syncConnection.Lock())
    {
      KingdomData pData = ((SQLiteConnection) syncConnection).Find<KingdomData>((object) pID);
      if (pData == null)
        return (DeadKingdom) null;
      pData.from_db = true;
      deadKingdom = new DeadKingdom();
      deadKingdom.loadData(pData);
      this._dead_kingdoms[pID] = deadKingdom;
      return deadKingdom;
    }
  }

  public override void clear()
  {
    foreach (NanoObject nanoObject in this._dead_kingdoms.Values)
      nanoObject.Dispose();
    this._dead_kingdoms.Clear();
    base.clear();
  }

  public override bool isLocked() => this.isUnitsDirty() || this._dirty_cities;

  protected override void updateDirtyUnits()
  {
    for (int index = 0; index < World.world.units.units_only_dying.Count; ++index)
      World.world.units.units_only_dying[index].kingdom.preserveAlive();
    List<Actor> unitsOnlyCiv = World.world.units.units_only_civ;
    for (int index = 0; index < unitsOnlyCiv.Count; ++index)
    {
      Actor pActor = unitsOnlyCiv[index];
      if (pActor.kingdom.isDirtyUnits())
        pActor.kingdom.listUnit(pActor);
    }
  }

  public void beginChecksCities()
  {
    if (this._dirty_cities)
      this.updateDirtyCities();
    this._dirty_cities = false;
  }

  public void updateDirtyCities()
  {
    this.clearAllCitiesLists();
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
      city.kingdom.listCity(city);
  }

  public void clearAllCitiesLists()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) this)
      kingdom.clearListCities();
    WildKingdomsManager.neutral.clearListCities();
  }

  public bool hasDirtyCities() => this._dirty_cities;

  public void setDirtyCities() => this._dirty_cities = true;

  public void beginChecksBuildings()
  {
    if (this._dirty_buildings)
      this.updateDirtyBuildings();
    this._dirty_buildings = false;
  }

  private void updateDirtyBuildings()
  {
    this.clearAllBuildingLists();
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (!city.kingdom.wild)
        city.kingdom.addBuildings(city.buildings);
    }
  }

  public void setDirtyBuildings() => this._dirty_buildings = true;

  private void clearAllBuildingLists()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) this)
      kingdom.clearBuildingList();
  }
}
