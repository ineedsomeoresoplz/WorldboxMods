// Decompiled with JetBrains decompiler
// Type: SimObjectsZones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SimObjectsZones
{
  private float _timer;
  private const float INTERVAL = 0.1f;
  private readonly List<WorldTile> _to_clear_tiles = new List<WorldTile>();
  private readonly HashSet<MapChunk> _dirty_building_chunks = new HashSet<MapChunk>();
  private bool _buildings_dirty;

  public void setBuildingsDirty(MapChunk pChunk)
  {
    this._buildings_dirty = true;
    pChunk.setBuildingsDirty();
    this._dirty_building_chunks.Add(pChunk);
  }

  internal void update()
  {
    Bench.bench("sim_zones", "game_total");
    if ((double) this._timer > 0.0)
    {
      this._timer -= World.world.delta_time;
    }
    else
    {
      this._timer = 0.1f;
      this.recalc();
    }
    Bench.benchEnd("sim_zones", "game_total");
  }

  private void recalc()
  {
    this.reset();
    Bench.bench("islands.recalcActors", "sim_zones");
    World.world.islands_calculator.recalcActors();
    Bench.benchEnd("islands.recalcActors", "sim_zones");
    Bench.bench("checkUnits", "sim_zones");
    this.checkUnits();
    Bench.benchEnd("checkUnits", "sim_zones");
    Bench.bench("checkBuildings", "sim_zones");
    if (this._buildings_dirty)
    {
      this.checkBuildings();
      this._buildings_dirty = false;
      foreach (MapChunk dirtyBuildingChunk in this._dirty_building_chunks)
        dirtyBuildingChunk.finishBuildingsCheck();
      this._dirty_building_chunks.Clear();
    }
    Bench.benchEnd("checkBuildings", "sim_zones");
  }

  private void checkUnits()
  {
    List<Actor> simpleList = World.world.units.getSimpleList();
    int index = 0;
    for (int count = simpleList.Count; index < count; ++index)
    {
      Actor pActor = simpleList[index];
      if (pActor.isAlive())
      {
        WorldTile currentTile = pActor.current_tile;
        this.addUnit(pActor, currentTile);
        currentTile.chunk.objects.addActor(pActor);
      }
    }
  }

  private void checkBuildings()
  {
    List<Building> simpleList = World.world.buildings.getSimpleList();
    int index = 0;
    for (int count = simpleList.Count; index < count; ++index)
    {
      Building pBuilding = simpleList[index];
      if (pBuilding.isUsable())
      {
        MapChunk chunk = pBuilding.chunk;
        if (chunk.buildings_dirty)
        {
          if (pBuilding.isCiv() && pBuilding.asset.docks && pBuilding.component_docks.hasOceanTiles())
            pBuilding.component_docks.tiles_ocean[0].region.island.addDock(pBuilding);
          chunk.objects.addBuilding(pBuilding);
        }
      }
    }
  }

  private void addUnit(Actor pActor, WorldTile pTile)
  {
    if (!pTile.hasUnits())
      this._to_clear_tiles.Add(pTile);
    pTile.addUnit(pActor);
    TileZone zone = pTile.zone;
    City zoneCity = pTile.zone_city;
    if (zoneCity == null || pActor.isInsideSomething())
      return;
    Kingdom kingdom = pActor.kingdom;
    if (pActor.profession_asset.can_capture)
      zoneCity.updateConquest(pActor);
    else if (kingdom.isCiv())
      return;
    if (zoneCity.danger_zones.Contains(zone) || kingdom.isMobs() && WorldLawLibrary.world_law_peaceful_monsters.isEnabled() || kingdom == zoneCity.kingdom || !kingdom.asset.count_as_danger || !kingdom.isEnemy(zoneCity.kingdom))
      return;
    zoneCity.danger_zones.Add(zone);
  }

  private void clearTileUnits()
  {
    List<WorldTile> toClearTiles = this._to_clear_tiles;
    int index = 0;
    for (int count = toClearTiles.Count; index < count; ++index)
      toClearTiles[index].clearUnits();
    toClearTiles.Clear();
  }

  private void clearChunkObjects(bool pForceClearBuildings)
  {
    MapChunk[] chunks = World.world.map_chunk_manager.chunks;
    int index = 0;
    for (int length = chunks.Length; index < length; ++index)
    {
      MapChunk mapChunk = chunks[index];
      if (!mapChunk.objects.isEmpty())
        mapChunk.clearObjects(pForceClearBuildings);
    }
  }

  private void clearIslandsDocks()
  {
    if (!this._buildings_dirty)
      return;
    ListPool<TileIsland> islands = World.world.islands_calculator.islands;
    int index = 0;
    for (int count = islands.Count; index < count; ++index)
    {
      TileIsland tileIsland = islands[index];
      tileIsland.docks?.Dispose();
      tileIsland.docks = (ListPool<Docks>) null;
    }
  }

  private void clearCaptureAndDangerZones()
  {
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      city.clearCurrentCaptureAmounts();
      city.clearDangerZones();
    }
  }

  private void clearAllDisposed()
  {
    foreach (BaseSystemManager listAllSimManager in World.world.list_all_sim_managers)
      listAllSimManager.ClearAllDisposed();
  }

  private void reset(bool pForceClearBuildings = false)
  {
    if (pForceClearBuildings)
      this._buildings_dirty = true;
    Bench.bench("clear_tiles", "sim_zones");
    this.clearTileUnits();
    Bench.benchEnd("clear_tiles", "sim_zones");
    Bench.bench("clear_chunks", "sim_zones");
    this.clearChunkObjects(pForceClearBuildings);
    Bench.benchEnd("clear_chunks", "sim_zones");
    Bench.bench("clear_islands_docks", "sim_zones");
    this.clearIslandsDocks();
    Bench.benchEnd("clear_islands_docks", "sim_zones");
    Bench.bench("clear_capture_and_danger_zones", "sim_zones");
    this.clearCaptureAndDangerZones();
    Bench.benchEnd("clear_capture_and_danger_zones", "sim_zones");
    Bench.bench("clear_all_disposed", "sim_zones");
    this.clearAllDisposed();
    Bench.benchEnd("clear_all_disposed", "sim_zones");
  }

  public void fullClear() => this.reset(true);
}
