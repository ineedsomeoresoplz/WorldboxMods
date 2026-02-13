// Decompiled with JetBrains decompiler
// Type: IslandsCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class IslandsCalculator
{
  private float _timer_update_actors;
  public ListPool<TileIsland> islands = new ListPool<TileIsland>();
  public readonly List<TileIsland> islands_ground = new List<TileIsland>();
  private readonly List<MapRegion> _temp_regions = new List<MapRegion>();
  private readonly List<MapRegion> _temp_regions_cur_wave = new List<MapRegion>();
  private readonly List<MapRegion> _temp_regions_next_wave = new List<MapRegion>();
  private int _last_island_id;
  private readonly HashSet<TileIsland> _dirty_islands = new HashSet<TileIsland>();
  private readonly Queue<MapRegion> _wave = new Queue<MapRegion>();
  private readonly Stack<TileIsland> _island_pool = new Stack<TileIsland>();

  public void prepareCalc() => this._dirty_islands.Clear();

  public void makeDirty(TileIsland pIsland) => this._dirty_islands.Add(pIsland);

  public void clearDirty()
  {
    using (ListPool<TileIsland> islands = this.islands)
    {
      this.islands = new ListPool<TileIsland>(islands.Count);
      foreach (TileIsland dirtyIsland in this._dirty_islands)
      {
        dirtyIsland.clearRegionsFromIsland();
        dirtyIsland.insideRegionEdges.Clear();
        foreach (TileIsland connectedIsland in dirtyIsland.getConnectedIslands())
        {
          connectedIsland.setDirtyIslandNeighbours();
          ++MapChunkManager.m_dirtyIslands;
        }
      }
      for (int index = 0; index < islands.Count; ++index)
      {
        TileIsland tileIsland = islands[index];
        if (tileIsland.removed)
        {
          tileIsland.reset();
          this._island_pool.Push(tileIsland);
        }
        else
          this.islands.Add(tileIsland);
      }
    }
  }

  public void clear()
  {
    this._last_island_id = 0;
    ListPool<TileIsland> islands = this.islands;
    for (int index = 0; index < islands.Count; ++index)
    {
      TileIsland tileIsland = islands[index];
      tileIsland.reset();
      this._island_pool.Push(tileIsland);
    }
    this._dirty_islands.Clear();
    this.islands.Clear();
    this.islands_ground.Clear();
    this._wave.Clear();
    this._temp_regions.Clear();
    this._temp_regions_cur_wave.Clear();
    this._temp_regions_next_wave.Clear();
  }

  public WorldTile tryGetRandomGround()
  {
    WorldTile randomGround = (WorldTile) null;
    if (this.islands.Count > 0)
    {
      TileIsland randomIslandGround = this.getRandomIslandGround();
      if (randomIslandGround != null && randomIslandGround.regions.Count > 0)
        randomGround = randomIslandGround.getRandomTile();
    }
    if (randomGround == null)
      randomGround = World.world.tiles_list.GetRandom<WorldTile>();
    return randomGround;
  }

  internal bool hasGround() => this.islands_ground.Count > 0;

  internal bool hasNonGround() => this.islands.Count > this.islands_ground.Count;

  internal float groundIslandRatio()
  {
    return this.islands.Count == 0 ? 0.0f : (float) this.islands_ground.Count / (float) this.islands.Count;
  }

  internal float realGroundRatio()
  {
    // ISSUE: unable to decompile the method.
  }

  internal TileIsland getRandomIslandGroundWeighted(bool pMinRegions = true)
  {
    if (this.islands_ground.Count == 0)
      return (TileIsland) null;
    int capacity = 0;
    for (int index = 0; index < this.islands_ground.Count; ++index)
    {
      TileIsland tileIsland = this.islands_ground[index];
      if (!pMinRegions || tileIsland.regions.Count >= 4)
        capacity += tileIsland.regions.Count;
    }
    if (capacity == 0)
      return (TileIsland) null;
    using (ListPool<TileIsland> listPool = new ListPool<TileIsland>(capacity))
    {
      for (int index = 0; index < this.islands_ground.Count; ++index)
      {
        TileIsland pObject = this.islands_ground[index];
        if (!pMinRegions || pObject.regions.Count >= 4)
          listPool.AddTimes<TileIsland>(pObject.regions.Count, pObject);
      }
      return listPool.GetRandom<TileIsland>();
    }
  }

  internal TileIsland getRandomIslandGround(bool pMinRegions = true)
  {
    if (this.islands_ground.Count == 0)
      return (TileIsland) null;
    if (!pMinRegions)
      return this.islands_ground.GetRandom<TileIsland>();
    foreach (TileIsland randomIslandGround in this.islands_ground.LoopRandom<TileIsland>())
    {
      if (randomIslandGround.regions.Count >= 4)
        return randomIslandGround;
    }
    return (TileIsland) null;
  }

  internal TileIsland getRandomIslandNonGroundWeighted(bool pMinRegions = true)
  {
    if (this.islands.Count == 0)
      return (TileIsland) null;
    if (this.islands_ground.Count == this.islands.Count)
      return (TileIsland) null;
    int capacity = 0;
    for (int index = 0; index < this.islands.Count; ++index)
    {
      TileIsland island = this.islands[index];
      if (island.type != TileLayerType.Ground && (!pMinRegions || island.regions.Count >= 4))
        capacity += island.regions.Count;
    }
    if (capacity == 0)
      return (TileIsland) null;
    using (ListPool<TileIsland> listPool = new ListPool<TileIsland>(capacity))
    {
      for (int index = 0; index < this.islands.Count; ++index)
      {
        TileIsland island = this.islands[index];
        if (island.type != TileLayerType.Ground && (!pMinRegions || island.regions.Count >= 4))
          listPool.AddTimes<TileIsland>(island.regions.Count, island);
      }
      return listPool.GetRandom<TileIsland>();
    }
  }

  internal TileIsland getRandomIslandNonGround(bool pMinRegions = true)
  {
    if (this.islands.Count == 0)
      return (TileIsland) null;
    if (this.islands_ground.Count == this.islands.Count)
      return (TileIsland) null;
    foreach (TileIsland randomIslandNonGround in this.islands.LoopRandom<TileIsland>())
    {
      if (randomIslandNonGround.type != TileLayerType.Ground && (!pMinRegions || randomIslandNonGround.regions.Count >= 4))
        return randomIslandNonGround;
    }
    return (TileIsland) null;
  }

  public int countLandIslands()
  {
    int num = 0;
    for (int index = 0; index < this.islands.Count; ++index)
    {
      TileIsland island = this.islands[index];
      if (island.type == TileLayerType.Ground && island.regions.Count >= 4)
        ++num;
    }
    return num;
  }

  internal void recalcActors()
  {
    ListPool<TileIsland> islands = this.islands;
    for (int index = 0; index < islands.Count; ++index)
      islands[index].actors.Clear();
    List<Actor> simpleList = World.world.units.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
    {
      Actor actor = simpleList[index];
      if (actor.isAlive())
        actor.current_tile.region.island.actors.Add(actor);
    }
  }

  private void clearCaches()
  {
    for (int index = 0; index < this.islands.Count; ++index)
      this.islands[index].clearCache();
  }

  public void findIslands(ListPool<TileIsland> pNewIslands)
  {
    Bench.bench("find_islands_prepare", "chunks");
    this._temp_regions.Clear();
    this.islands_ground.Clear();
    this.clearCaches();
    Bench.benchEnd("find_islands_prepare", "chunks");
    Bench.bench("find_islands_temp_regions", "chunks");
    MapChunk[] chunks = World.world.map_chunk_manager.chunks;
    int length = chunks.Length;
    for (int index1 = 0; index1 < length; ++index1)
    {
      MapChunk mapChunk = chunks[index1];
      for (int index2 = 0; index2 < mapChunk.regions.Count; ++index2)
      {
        MapRegion region = mapChunk.regions[index2];
        if (region.island == null)
        {
          region.is_island_checked = false;
          this._temp_regions.Add(region);
        }
      }
    }
    Bench.benchEnd("find_islands_temp_regions", "chunks", true, (long) this._temp_regions.Count);
    Bench.bench("find_islands_new_islands", "chunks");
    for (int index = 0; index < this._temp_regions.Count; ++index)
    {
      MapRegion tempRegion = this._temp_regions[index];
      if (tempRegion.island == null)
      {
        TileIsland tileIsland = this.newIslandFrom(tempRegion);
        pNewIslands.Add(tileIsland);
        ++MapChunkManager.m_newIslands;
      }
    }
    Bench.benchEnd("find_islands_new_islands", "chunks", true, (long) MapChunkManager.m_newIslands);
    Bench.bench("find_islands_fin", "chunks");
    for (int index = 0; index < this.islands.Count; ++index)
    {
      TileIsland island = this.islands[index];
      island.countTiles();
      if (island.type == TileLayerType.Ground)
        this.islands_ground.Add(island);
    }
    Bench.benchEnd("find_islands_fin", "chunks");
  }

  private TileIsland newIslandFrom(MapRegion pRegion)
  {
    this._temp_regions_cur_wave.Clear();
    this._temp_regions_next_wave.Clear();
    TileIsland pIsland;
    if (this._island_pool.Count > 0)
    {
      pIsland = this._island_pool.Pop();
    }
    else
    {
      pIsland = new TileIsland(this._last_island_id);
      ++this._last_island_id;
    }
    pIsland.reset();
    pIsland.type = pRegion.type;
    this.islands.Add(pIsland);
    this._temp_regions_next_wave.Add(pRegion);
    this._wave.Clear();
    this._wave.Enqueue(pRegion);
    this.startFill(pIsland);
    return pIsland;
  }

  private void startFill(TileIsland pIsland)
  {
    while (this._wave.Count > 0)
    {
      MapRegion pRegion = this._wave.Dequeue();
      if (!pRegion.is_island_checked)
      {
        pRegion.island = pIsland;
        pIsland.addRegion(pRegion);
      }
      pRegion.is_island_checked = true;
      for (int index = 0; index < pRegion.neighbours.Count; ++index)
      {
        MapRegion neighbour = pRegion.neighbours[index];
        if (!neighbour.is_island_checked)
        {
          neighbour.is_island_checked = true;
          neighbour.island = pIsland;
          pIsland.addRegion(neighbour);
          this._wave.Enqueue(neighbour);
        }
      }
    }
    pIsland.regions.checkAddRemove();
  }
}
