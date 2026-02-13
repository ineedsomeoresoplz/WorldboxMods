// Decompiled with JetBrains decompiler
// Type: CityZoneAbandon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class CityZoneAbandon : CityZoneWorkerBase
{
  private List<ListPool<TileZone>> _split_areas = new List<ListPool<TileZone>>();
  private HashSetTileZone _zones_to_check = new HashSetTileZone();

  public void checkCities()
  {
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
      city.checkAbandon();
  }

  public void check(City pCity, bool pDebug = false, HashSet<TileZone> pSetToFill = null)
  {
    if (pCity.buildings.Count == 0)
      return;
    this.clearAll();
    this.prepareCityZones(pCity);
    this.startCheckingFromBuildings(pCity);
    this._split_areas.Sort(new Comparison<ListPool<TileZone>>(CityZoneAbandon.sorter));
    if (pDebug)
      return;
    this.abandonLeftoverZones(pCity);
    if (this._split_areas.Count < 2)
      return;
    this._split_areas[0].Dispose();
    this._split_areas.RemoveAt(0);
    if (this._split_areas.Count <= 0)
      return;
    this.abandonSmallAreas(pCity);
  }

  private void startCheckingFromBuildings(City pCity)
  {
    for (int index = 0; index < pCity.buildings.Count; ++index)
    {
      Building building = pCity.buildings[index];
      WorldTile currentTile = building.current_tile;
      if (!building.asset.docks)
        this.startWaveFromTile(currentTile, pCity);
    }
  }

  private void startWaveFromTile(WorldTile pTile, City pCity)
  {
    if (!this._zones_to_check.Contains(pTile.zone))
      return;
    this.prepareWave();
    Queue<ZoneConnection> pWave1 = this._wave;
    Queue<ZoneConnection> pWave2 = this._next_wave;
    ListPool<TileZone> listPool = new ListPool<TileZone>(this._wave.Count + this._next_wave.Count);
    this._split_areas.Add(listPool);
    this.queueConnection(new ZoneConnection(pTile.zone, pTile.region), pWave1, true);
    using (ListPool<MapRegion> pListToFill = new ListPool<MapRegion>())
    {
      int num = 0;
      while (pWave2.Count > 0 || pWave1.Count > 0)
      {
        if (pWave1.Count == 0)
        {
          Queue<ZoneConnection> zoneConnectionQueue = pWave1;
          pWave1 = pWave2;
          pWave2 = zoneConnectionQueue;
          ++num;
        }
        ZoneConnection zoneConnection = pWave1.Dequeue();
        TileZone zone = zoneConnection.zone;
        MapRegion region = zoneConnection.region;
        if (zone.isSameCityHere(pCity))
          listPool.Add(zone);
        foreach (TileZone neighbour in zone.neighbours)
        {
          if (neighbour.isSameCityHere(pCity) && neighbour.tiles_with_ground != 0)
          {
            pListToFill.Clear();
            if (TileZone.hasZonesConnectedViaRegions(zone, neighbour, region, pListToFill))
            {
              for (int index = 0; index < pListToFill.Count; ++index)
              {
                MapRegion pRegion = pListToFill[index];
                ZoneConnection pConnection = new ZoneConnection(neighbour, pRegion);
                if (this._zones_checked.Add(pConnection))
                  this.queueConnection(pConnection, pWave2, false);
              }
            }
          }
        }
      }
    }
  }

  private void abandonLeftoverZones(City pCity)
  {
    if (this._zones_to_check.Count == 0)
      return;
    foreach (TileZone pZone in (HashSet<TileZone>) this._zones_to_check)
      pCity.removeZone(pZone);
  }

  private void abandonSmallAreas(City pCity)
  {
    for (int index1 = 0; index1 < this._split_areas.Count; ++index1)
    {
      ListPool<TileZone> splitArea = this._split_areas[index1];
      for (int index2 = 0; index2 < splitArea.Count; ++index2)
      {
        TileZone pZone = splitArea[index2];
        pCity.removeZone(pZone);
      }
    }
  }

  private void prepareCityZones(City pCity)
  {
    this._zones_to_check.UnionWith((IEnumerable<TileZone>) pCity.zones);
  }

  internal override void clearAll()
  {
    base.clearAll();
    foreach (ListPool<TileZone> splitArea in this._split_areas)
      splitArea.Dispose();
    this._split_areas.Clear();
    this._zones_to_check.Clear();
  }

  private static int sorter(ListPool<TileZone> pList1, ListPool<TileZone> pList2)
  {
    return pList2.Count.CompareTo(pList1.Count);
  }

  protected override void queueConnection(
    ZoneConnection pConnection,
    Queue<ZoneConnection> pWave,
    bool pCheck = false)
  {
    base.queueConnection(pConnection, pWave, pCheck);
    this._zones_to_check.Remove(pConnection.zone);
  }
}
