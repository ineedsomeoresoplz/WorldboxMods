// Decompiled with JetBrains decompiler
// Type: CityManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CityManager : MetaSystemManager<City, CityData>
{
  private bool _dirty_buildings;

  public CityManager() => this.type_id = "city";

  public City newCity(Kingdom pKingdom, TileZone pZone, Actor pOriginalActor)
  {
    ++World.world.game_stats.data.citiesCreated;
    ++World.world.map_stats.citiesCreated;
    City city = this.newObject();
    city.data.founder_id = pOriginalActor.getID();
    city.data.founder_name = pOriginalActor.name;
    city.data.original_actor_asset = pOriginalActor.asset.id;
    city.data.equipment = new CityEquipment();
    city.setKingdom(pKingdom);
    city.addZone(pZone);
    foreach (TileZone pZone1 in pZone.neighbours_all)
    {
      if (pZone1.city == null)
        city.addZone(pZone1);
    }
    World.world.city_zone_helper.city_place_finder.setDirty();
    return city;
  }

  public City buildNewCity(Actor pActor, TileZone pZone)
  {
    City pCity = World.world.cities.newCity(pActor.kingdom, pZone, pActor);
    pCity.setUnitMetas(pActor);
    pCity.newCityEvent(pActor);
    WorldLog.logNewCity(pCity);
    return pCity;
  }

  public bool tryToCreateCity(Actor pActor, ListPool<Building> pBuildingList)
  {
    return !pActor.current_tile.zone.hasCity();
  }

  public bool canStartNewCityCivilizationHere(Actor pActor)
  {
    if (pActor.kingdom.asset.is_forced_by_trait || !pActor.canBuildNewCity())
      return false;
    KingdomAsset kingdomAsset = AssetManager.kingdoms.get(pActor.asset.kingdom_id_civilization);
    if (kingdomAsset == null || !kingdomAsset.civ)
      return false;
    WorldTile currentTile = pActor.current_tile;
    TileZone zone = currentTile.zone;
    foreach (TileZone neighbour in zone.neighbours)
    {
      if (neighbour.hasCity())
      {
        WorldTile tile = neighbour.city.getTile();
        if (tile != null && tile.isSameIsland(currentTile))
        {
          neighbour.city.addZone(zone);
          return false;
        }
      }
    }
    return true;
  }

  public City buildFirstCivilizationCity(Actor pActor)
  {
    City pCity = this.buildNewCity(pActor, pActor.current_zone);
    pActor.joinCity(pCity);
    pCity.setUnitMetas(pActor);
    pCity.convertSameSpeciesAroundUnit(pActor, false);
    return pCity;
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      City city = pActor.city;
      if (city != null && city.isDirtyUnits())
        city.listUnit(pActor);
    }
  }

  public void beginChecksBuildings()
  {
    if (this._dirty_buildings)
      this.updateDirtyBuildings();
    this._dirty_buildings = false;
  }

  private void updateDirtyBuildings()
  {
    this.clearAllBuildingLists();
    foreach (City city in (CoreSystemManager<City, CityData>) this)
    {
      Kingdom kingdom = city.kingdom;
      for (int index = 0; index < city.zones.Count; ++index)
      {
        foreach (Building pBuilding in city.zones[index].buildings_all)
        {
          if (pBuilding.asset.city_building && pBuilding.isUsable())
          {
            pBuilding.setKingdomCiv(kingdom);
            city.listBuilding(pBuilding);
          }
        }
      }
    }
  }

  public void setDirtyBuildings(City pCity)
  {
    this._dirty_buildings = true;
    World.world.kingdoms.setDirtyBuildings();
  }

  private void clearAllBuildingLists()
  {
    foreach (City city in (CoreSystemManager<City, CityData>) this)
      city.clearBuildingList();
  }

  protected override void addObject(City pObject)
  {
    pObject.init();
    base.addObject(pObject);
  }

  public override City loadObject(CityData pData)
  {
    City city = base.loadObject(pData);
    city.loadCity(pData);
    return city;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    foreach (City city in (CoreSystemManager<City, CityData>) this)
    {
      city.update(pElapsed);
      city.clearCursorOver();
    }
  }

  public void updateAge()
  {
    foreach (City city in (CoreSystemManager<City, CityData>) this)
      city.updateAge();
  }

  public override List<CityData> save(List<City> pList = null)
  {
    List<CityData> cityDataList = new List<CityData>();
    foreach (City city in (CoreSystemManager<City, CityData>) this)
    {
      city.save();
      cityDataList.Add(city.data);
    }
    return cityDataList;
  }

  private void checkForCityErrors(SavedMap pSaveData)
  {
    List<CityData> cityDataList = new List<CityData>();
    for (int index = 0; index < pSaveData.cities.Count; ++index)
    {
      CityData city = pSaveData.cities[index];
      if (city.zones.Count != 0)
      {
        TileZone tileZone = World.world.zone_calculator.getZone(city.zones[0].x, city.zones[0].y);
        if (pSaveData.saveVersion < 7)
          tileZone = this.findZoneViaBuilding(city.id, pSaveData.buildings);
        if (tileZone != null)
          cityDataList.Add(city);
      }
    }
    pSaveData.cities = cityDataList;
  }

  public void loadCities(SavedMap pSaveData)
  {
    this.checkForCityErrors(pSaveData);
    for (int index1 = 0; index1 < pSaveData.cities.Count; ++index1)
    {
      CityData city1 = pSaveData.cities[index1];
      City city2 = this.loadObject(city1);
      if (city2 != null && pSaveData.saveVersion >= 7)
      {
        for (int index2 = 0; index2 < city1.zones.Count; ++index2)
        {
          ZoneData zone1 = city1.zones[index2];
          TileZone zone2 = World.world.zone_calculator.getZone(zone1.x, zone1.y);
          if (zone2 != null)
            city2.addZone(zone2);
        }
      }
    }
  }

  public override void removeObject(City pObject)
  {
    ++World.world.game_stats.data.citiesDestroyed;
    ++World.world.map_stats.citiesDestroyed;
    WorldLog.logCityDestroyed(pObject);
    pObject.destroyCity();
    base.removeObject(pObject);
    World.world.city_zone_helper.city_place_finder.setDirty();
    World.world.cultures.setDirtyCities();
    World.world.kingdoms.setDirtyCities();
    World.world.languages.setDirtyCities();
    World.world.religions.setDirtyCities();
  }

  private TileZone findZoneViaBuilding(long pID, List<BuildingData> pList)
  {
    for (int index = 0; index < pList.Count; ++index)
    {
      BuildingData p = pList[index];
      if (p.cityID == pID)
        return World.world.GetTileSimple(p.mainX, p.mainY).zone;
    }
    return (TileZone) null;
  }

  public override bool isLocked() => this.isUnitsDirty() || World.world.kingdoms.hasDirtyCities();
}
