// Decompiled with JetBrains decompiler
// Type: CityPlaceFinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CityPlaceFinder : CityZoneWorkerBase
{
  private bool _dirty;
  internal List<TileZone> zones = new List<TileZone>();

  internal bool isDirty() => DebugConfig.isOn(DebugOption.SystemCityPlaceFinder) && this._dirty;

  internal void recalc()
  {
    if (!this.isDirty())
      return;
    this._dirty = false;
    this.clearAll();
    this.prepareBasicZones();
    this.prepareQueueFromCities();
    this.startWave();
    this.createFinalList();
  }

  internal override void clearAll()
  {
    base.clearAll();
    this.zones.Clear();
    List<TileZone> zones = World.world.zone_calculator.zones;
    for (int index = 0; index < zones.Count; ++index)
      zones[index].setGoodForNewCity(true);
  }

  private void prepareBasicZones()
  {
    List<TileZone> zones = World.world.zone_calculator.zones;
    for (int index = 0; index < zones.Count; ++index)
    {
      TileZone tileZone = zones[index];
      if (!tileZone.canStartCityHere())
        tileZone.setGoodForNewCity(false);
      else if (tileZone.centerTile.region.island.getTileCount() < 300)
        tileZone.setGoodForNewCity(false);
    }
  }

  private void prepareQueueFromCities()
  {
    this.prepareWave();
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
      this.checkCity(city, this._wave);
  }

  private void checkCity(City pCity, Queue<ZoneConnection> pWaveQ)
  {
    WorldTile tile = pCity.getTile();
    if (tile == null)
      return;
    TileIsland island = tile.region.island;
    foreach (TileZone borderZone in pCity.border_zones)
    {
      for (int index = 0; index < borderZone.centerTile.chunk.regions.Count; ++index)
      {
        MapRegion region = borderZone.chunk.regions[index];
        if (region.isTypeGround() && region.zones.Contains(borderZone) && region.island == island)
          this.queueConnection(new ZoneConnection(borderZone, region), pWaveQ, true);
      }
    }
  }

  private void startWave()
  {
    Queue<ZoneConnection> zoneConnectionQueue1 = this._wave;
    Queue<ZoneConnection> pWave = this._next_wave;
    using (ListPool<MapRegion> pListToFill = new ListPool<MapRegion>())
    {
      int num1 = 3;
      int num2 = 0;
      while (pWave.Count > 0 || zoneConnectionQueue1.Count > 0)
      {
        if (zoneConnectionQueue1.Count == 0)
        {
          Queue<ZoneConnection> zoneConnectionQueue2 = zoneConnectionQueue1;
          zoneConnectionQueue1 = pWave;
          pWave = zoneConnectionQueue2;
          ++num2;
        }
        if (num2 > num1)
          break;
        ZoneConnection zoneConnection = zoneConnectionQueue1.Dequeue();
        TileZone zone = zoneConnection.zone;
        MapRegion region = zoneConnection.region;
        foreach (TileZone neighbour in zone.neighbours)
        {
          if (!neighbour.hasCity())
          {
            pListToFill.Clear();
            if (TileZone.hasZonesConnectedViaRegions(zone, neighbour, region, pListToFill))
            {
              for (int index = 0; index < pListToFill.Count; ++index)
              {
                MapRegion pRegion = pListToFill[index];
                ZoneConnection pConnection = new ZoneConnection(neighbour, pRegion);
                if (this._zones_checked.Add(pConnection))
                {
                  neighbour.setGoodForNewCity(false);
                  this.queueConnection(pConnection, pWave);
                }
              }
            }
          }
        }
      }
    }
  }

  private void createFinalList()
  {
    for (int index = 0; index < World.world.zone_calculator.zones.Count; ++index)
    {
      TileZone zone = World.world.zone_calculator.zones[index];
      if (zone.isGoodForNewCity())
        this.zones.Add(zone);
    }
  }

  public bool hasPossibleZones() => !this._dirty && this.zones.Count > 0;

  internal void setDirty()
  {
    this._dirty = true;
    this.clearCurrentZones();
  }

  private void clearCurrentZones()
  {
    if (this.zones.Count == 0)
      return;
    for (int index = 0; index < this.zones.Count; ++index)
      this.zones[index].setGoodForNewCity(false);
    this.zones.Clear();
  }
}
