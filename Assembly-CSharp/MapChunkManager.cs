// Decompiled with JetBrains decompiler
// Type: MapChunkManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public class MapChunkManager
{
  private readonly Color _color_1_gray = Color.gray;
  private readonly Color _color_2_white = Color.white;
  private MapChunk[,] _map;
  public MapChunk[] chunks = new MapChunk[0];
  private readonly List<MapChunk> _dirty_chunks_regions = new List<MapChunk>();
  private readonly List<MapChunk> _dirty_chunks_links = new List<MapChunk>();
  private int m_dirtyChunks;
  private static int m_dirtyCorners;
  private static int m_newRegions;
  public static int m_newLinks;
  internal static int m_newIslands;
  internal static int m_dirtyIslands;
  private float _timer = 0.4f;
  private int _get_amount_x;
  private int _amount_y;
  private readonly List<MapChunk> _last_dirty_chunks = new List<MapChunk>();
  private readonly List<MapChunk> _last_dirty_links = new List<MapChunk>();
  private readonly HashSet<MapRegion> _region_set = new HashSet<MapRegion>();
  private readonly List<TileIsland> _temp_dirty_neighbours_islands = new List<TileIsland>();

  public int amount_x => this._get_amount_x;

  public void checkDiagnosticRegions() => this.diagnosticRegions();

  public void update(float pElapsed, bool pForce = false)
  {
    if ((double) this._timer > 0.0 && !pForce)
      this._timer -= pElapsed;
    else
      this.updateDirty();
  }

  private void diagnosticRegions()
  {
    HashSet<MapRegion> mapRegionSet1 = new HashSet<MapRegion>();
    HashSet<MapRegion> mapRegionSet2 = new HashSet<MapRegion>();
    MapChunk[] chunks = this.chunks;
    int length = chunks.Length;
    for (int index = 0; index < length; ++index)
    {
      foreach (MapRegion region in chunks[index].regions)
      {
        if (region.tiles.Count == 0 || region.chunk == null)
          mapRegionSet1.Add(region);
        foreach (MapRegion neighbour in region.neighbours)
        {
          if (neighbour.tiles.Count == 0 || neighbour.chunk == null)
            mapRegionSet2.Add(region);
        }
      }
    }
    if (mapRegionSet1.Count <= 0 && mapRegionSet2.Count <= 0)
      return;
    Debug.LogError((object) "Something is wrong with regions");
    int count = mapRegionSet1.Count;
    Debug.LogError((object) ("tRegionMain: " + count.ToString()));
    count = mapRegionSet2.Count;
    Debug.LogError((object) ("tRegionNeighbour: " + count.ToString()));
  }

  public void prepare()
  {
    int num1 = 4;
    this._get_amount_x = Config.ZONE_AMOUNT_X * num1;
    this._amount_y = Config.ZONE_AMOUNT_Y * num1;
    this._map = new MapChunk[this._get_amount_x, this._amount_y];
    int length = this._get_amount_x * this._amount_y;
    if (length != this.chunks.Length)
      this.chunks = new MapChunk[length];
    else
      Array.Clear((Array) this.chunks, 0, this.chunks.Length);
    int num2 = 0;
    int index1 = 0;
    for (int index2 = 0; index2 < this._amount_y; ++index2)
    {
      for (int index3 = 0; index3 < this._get_amount_x; ++index3)
      {
        MapChunk pChunk = new MapChunk();
        pChunk.id = num2++;
        pChunk.x = index3;
        pChunk.y = index2;
        this._map[index3, index2] = pChunk;
        pChunk.color = (index3 + index2) % 2 != 0 ? this._color_2_white : this._color_1_gray;
        this.chunks[index1] = pChunk;
        this.fill(pChunk);
        ++index1;
      }
    }
    this.fillAndLinkTileZones();
    this.generateNeighbours();
    this.generateEdgeConnections();
  }

  private void generateEdgeConnections()
  {
    MapChunk[] chunks = this.chunks;
    int length = chunks.Length;
    for (int index = 0; index < length; ++index)
      chunks[index].generateEdgeConnections();
  }

  private void fillAndLinkTileZones()
  {
    for (int index = 0; index < World.world.zone_calculator.zones.Count; ++index)
    {
      TileZone zone = World.world.zone_calculator.zones[index];
      zone.chunk.zones.Add(zone);
    }
  }

  private void fill(MapChunk pChunk)
  {
    int num = 16 /*0x10*/;
    int pX1 = pChunk.x * num;
    int pY1 = pChunk.y * num;
    for (int pX2 = 0; pX2 < num; ++pX2)
    {
      for (int pY2 = 0; pY2 < num; ++pY2)
      {
        WorldTile tileSimple = World.world.GetTileSimple(pX2 + pX1, pY2 + pY1);
        tileSimple.chunk = pChunk;
        pChunk.addTile(tileSimple, pX2, pY2);
      }
    }
    pChunk.world_center_x = (float) (pX1 + 8);
    pChunk.world_center_y = (float) (pY1 + 8);
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      WorldTile tileSimple = World.world.GetTileSimple(index + pX1, pY1);
      pChunk.bounds_down.Add(tileSimple);
    }
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      WorldTile tileSimple = World.world.GetTileSimple(pX1, pY1 + index);
      pChunk.bounds_left.Add(tileSimple);
    }
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      WorldTile tileSimple = World.world.GetTileSimple(pX1 + 16 /*0x10*/ - 1, pY1 + index);
      pChunk.bounds_right.Add(tileSimple);
    }
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      WorldTile tileSimple = World.world.GetTileSimple(index + pX1, pY1 + 16 /*0x10*/ - 1);
      pChunk.bounds_up.Add(tileSimple);
    }
    pChunk.edge_up_left = pChunk.bounds_up[0];
    pChunk.edge_up_right = pChunk.bounds_up[15];
    pChunk.edge_down_left = pChunk.bounds_down[0];
    pChunk.edge_down_right = pChunk.bounds_down[15];
    pChunk.combineEdges();
  }

  private void generateNeighbours()
  {
    MapChunk[] chunks = this.chunks;
    int length = chunks.Length;
    using (ListPool<MapChunk> listPool1 = new ListPool<MapChunk>(4))
    {
      using (ListPool<MapChunk> listPool2 = new ListPool<MapChunk>(8))
      {
        for (int index = 0; index < length; ++index)
        {
          MapChunk mapChunk = chunks[index];
          MapChunk pNeighbour1 = this.get(mapChunk.x - 1, mapChunk.y);
          mapChunk.addNeighbour(pNeighbour1, TileDirection.Left, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2);
          MapChunk pNeighbour2 = this.get(mapChunk.x + 1, mapChunk.y);
          mapChunk.addNeighbour(pNeighbour2, TileDirection.Right, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2);
          MapChunk pNeighbour3 = this.get(mapChunk.x, mapChunk.y - 1);
          mapChunk.addNeighbour(pNeighbour3, TileDirection.Down, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2);
          MapChunk pNeighbour4 = this.get(mapChunk.x, mapChunk.y + 1);
          mapChunk.addNeighbour(pNeighbour4, TileDirection.Up, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2);
          MapChunk pNeighbour5 = this.get(mapChunk.x - 1, mapChunk.y - 1);
          mapChunk.addNeighbour(pNeighbour5, TileDirection.Null, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2, true);
          MapChunk pNeighbour6 = this.get(mapChunk.x - 1, mapChunk.y + 1);
          mapChunk.addNeighbour(pNeighbour6, TileDirection.Null, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2, true);
          MapChunk pNeighbour7 = this.get(mapChunk.x + 1, mapChunk.y - 1);
          mapChunk.addNeighbour(pNeighbour7, TileDirection.Null, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2, true);
          MapChunk pNeighbour8 = this.get(mapChunk.x + 1, mapChunk.y + 1);
          mapChunk.addNeighbour(pNeighbour8, TileDirection.Null, (IList<MapChunk>) listPool1, (IList<MapChunk>) listPool2, true);
          mapChunk.neighbours = listPool1.ToArray<MapChunk>();
          mapChunk.neighbours_all = listPool2.ToArray<MapChunk>();
          listPool1.Clear();
          listPool2.Clear();
        }
      }
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public MapChunk get(int pX, int pY)
  {
    return pX < 0 || pX >= this._get_amount_x || pY < 0 || pY >= this._amount_y ? (MapChunk) null : this._map[pX, pY];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public MapChunk get(ref Vector2Int pPos)
  {
    return ((Vector2Int) ref pPos).x < 0 || ((Vector2Int) ref pPos).x >= this._get_amount_x || ((Vector2Int) ref pPos).y < 0 || ((Vector2Int) ref pPos).y >= this._amount_y ? (MapChunk) null : this._map[((Vector2Int) ref pPos).x, ((Vector2Int) ref pPos).y];
  }

  public MapChunk getByID(int pID) => this.chunks[pID];

  public void clearAll()
  {
    World.world.islands_calculator.clear();
    this._dirty_chunks_links.Clear();
    this._dirty_chunks_regions.Clear();
    int length = this.chunks.Length;
    for (int index = 0; index < length; ++index)
      this.chunks[index].clearRegions();
  }

  public void clean()
  {
    this.clearAll();
    MapChunk[] chunks = this.chunks;
    int length = chunks.Length;
    for (int index = 0; index < length; ++index)
      chunks[index].Dispose();
    Array.Clear((Array) this.chunks, 0, this.chunks.Length);
  }

  public void setAllLinksDirty()
  {
    foreach (MapChunk chunk in this.chunks)
      this.setDirty(chunk, false);
  }

  public void setDirty(MapChunk pChunk, bool pRegions = true, bool pLinks = true)
  {
    if (pRegions && !pChunk.dirty_regions)
    {
      pChunk.dirty_regions = true;
      this._dirty_chunks_regions.Add(pChunk);
    }
    if (!pLinks || pChunk.dirty_links)
      return;
    pChunk.dirty_links = true;
    this._dirty_chunks_links.Add(pChunk);
  }

  public void allDirty()
  {
    this._dirty_chunks_links.Clear();
    this._dirty_chunks_regions.Clear();
    MapChunk[] chunks = this.chunks;
    int length = chunks.Length;
    for (int index = 0; index < length; ++index)
    {
      MapChunk mapChunk = chunks[index];
      mapChunk.dirty_links = true;
      mapChunk.dirty_regions = true;
    }
    this._dirty_chunks_links.AddRange((IEnumerable<MapChunk>) this.chunks);
    this._dirty_chunks_regions.AddRange((IEnumerable<MapChunk>) this.chunks);
  }

  private bool isAllChunksDirty() => this.chunks.Length == this._dirty_chunks_regions.Count;

  private void updateDirty()
  {
    if (!DebugConfig.isOn(DebugOption.SystemUpdateDirtyChunks) || !this.isAllChunksDirty() && World.world.isActionHappening() || this._dirty_chunks_links.Count == 0 && this._dirty_chunks_regions.Count == 0)
      return;
    Bench.bench("chunks", "chunks_total");
    this._timer = 0.4f;
    this.m_dirtyChunks = this._dirty_chunks_regions.Count;
    MapChunkManager.m_newRegions = 0;
    MapChunkManager.m_newLinks = 0;
    MapChunkManager.m_newIslands = 0;
    MapChunkManager.m_dirtyIslands = 0;
    MapChunkManager.m_dirtyCorners = 0;
    MapRegion.created_time_last = Time.time;
    World.world.islands_calculator.prepareCalc();
    Bench.bench("clear_regions", "chunks");
    this.calc_clearRegions();
    Bench.benchEnd("clear_regions", "chunks", true, (long) this._dirty_chunks_links.Count);
    Bench.bench("clear_dirty_islands", "chunks");
    World.world.islands_calculator.clearDirty();
    Bench.benchEnd("clear_dirty_islands", "chunks", true, (long) this._dirty_chunks_links.Count);
    Bench.bench("P calc_regions", "chunks");
    this.calc_regions();
    Bench.benchEnd("P calc_regions", "chunks", true, (long) this._dirty_chunks_regions.Count);
    Bench.bench("shuffle_region_tiles", "chunks");
    this.calc_shuffleRegionTiles();
    Bench.benchEnd("shuffle_region_tiles", "chunks", true, (long) MapChunkManager.m_newRegions);
    Bench.bench("P calc_links", "chunks");
    this.calc_links();
    Bench.benchEnd("P calc_links", "chunks", true, (long) this._dirty_chunks_links.Count);
    Bench.bench("create_links", "chunks");
    this.calc_checkLinkResults();
    Bench.benchEnd("create_links", "chunks", true, (long) this._dirty_chunks_links.Count);
    Bench.bench("P calc_linked_regions", "chunks");
    this.calc_linkedRegions();
    Bench.benchEnd("P calc_linked_regions", "chunks", true, (long) this._dirty_chunks_links.Count);
    using (ListPool<TileIsland> pNewIslands = new ListPool<TileIsland>())
    {
      World.world.islands_calculator.findIslands(pNewIslands);
      Bench.bench("tile_corners_prepare", "chunks");
      this.calc_tileCornersPrepare(this._region_set);
      MapChunkManager.m_dirtyCorners = this._region_set.Count;
      Bench.benchEnd("tile_corners_prepare", "chunks", true, (long) this._dirty_chunks_links.Count);
      Bench.bench("center_regions", "chunks");
      this.calc_centerRegions();
      Bench.benchEnd("center_regions", "chunks", true, (long) this._region_set.Count);
      Bench.bench("island_region_edges", "chunks");
      Bench.benchEnd("island_region_edges", "chunks", true, (long) this.calc_islandRegionEdges(pNewIslands));
      Bench.bench("PH tile_edges", "chunks");
      this.calc_tileEdges();
      Bench.benchEnd("PH tile_edges", "chunks", true, (long) this._region_set.Count);
      Bench.bench("prepare_d_neighbour_islands", "chunks");
      this.prepareDirtyNeighbourIslands();
      Bench.benchEnd("prepare_d_neighbour_islands", "chunks", true, (long) this._temp_dirty_neighbours_islands.Count);
      Bench.bench("P neighbour_islands", "chunks");
      this.calc_neighbourIslands();
      Bench.benchEnd("P neighbour_islands", "chunks", true, (long) this._temp_dirty_neighbours_islands.Count);
      Bench.bench("clear_end", "chunks");
      this._region_set.Clear();
      this._dirty_chunks_links.Clear();
      this._dirty_chunks_regions.Clear();
      World.world.city_zone_helper.city_place_finder.setDirty();
      World.world.region_path_finder.clearCache();
      Bench.benchEnd("clear_end", "chunks");
      Bench.benchSetValue("m_dirtyChunks", this.m_dirtyChunks, "chunks");
      Bench.benchSetValue("m_newRegions", MapChunkManager.m_newRegions, "chunks");
      Bench.benchSetValue("m_newLinks", MapChunkManager.m_newLinks, "chunks");
      Bench.benchSetValue("m_newIslands", MapChunkManager.m_newIslands, "chunks");
      Bench.benchSetValue("m_dirtyIslands", MapChunkManager.m_dirtyIslands, "chunks");
      Bench.benchSetValue("m_dirtyCorners", MapChunkManager.m_dirtyCorners, "chunks");
      Bench.benchSetValue("m_dirtyIslandNeighb", this._temp_dirty_neighbours_islands.Count, "chunks");
      Bench.benchEnd("chunks", "chunks_total");
    }
  }

  private void calc_clearRegions()
  {
    List<MapChunk> dirtyChunksRegions = this._dirty_chunks_regions;
    int count1 = dirtyChunksRegions.Count;
    for (int index = 0; index < count1; ++index)
      dirtyChunksRegions[index].clearRegions();
    List<MapChunk> dirtyChunksLinks = this._dirty_chunks_links;
    int count2 = dirtyChunksLinks.Count;
    for (int index = 0; index < count2; ++index)
      dirtyChunksLinks[index].clearIsland();
  }

  private void calc_regions()
  {
    if (this._is_parallel_enabled)
    {
      List<MapChunk> tDirtyChunks = this._dirty_chunks_regions;
      int tCount = tDirtyChunks.Count;
      if (!this._batches_enabled)
      {
        Parallel.For(0, tCount, World.world.parallel_options, (Action<int>) (pIndex => tDirtyChunks[pIndex].calculateRegions()));
      }
      else
      {
        int tDynamicBatchSize = ParallelHelper.getDynamicBatchSize(tCount);
        Parallel.For(0, ParallelHelper.calcTotalBatches(tCount, tDynamicBatchSize), World.world.parallel_options, (Action<int>) (pBatchIndex =>
        {
          int batchBeg = ParallelHelper.calculateBatchBeg(pBatchIndex, tDynamicBatchSize);
          int batchEnd = ParallelHelper.calculateBatchEnd(batchBeg, tDynamicBatchSize, tCount);
          for (int index = batchBeg; index < batchEnd; ++index)
            tDirtyChunks[index].calculateRegions();
        }));
      }
    }
    else
    {
      List<MapChunk> dirtyChunksRegions = this._dirty_chunks_regions;
      int count = dirtyChunksRegions.Count;
      for (int index = 0; index < count; ++index)
        dirtyChunksRegions[index].calculateRegions();
    }
  }

  private void calc_shuffleRegionTiles()
  {
    List<MapChunk> dirtyChunksRegions = this._dirty_chunks_regions;
    int count = dirtyChunksRegions.Count;
    for (int index = 0; index < count; ++index)
    {
      MapChunk mapChunk = dirtyChunksRegions[index];
      MapChunkManager.m_newRegions += mapChunk.regions.Count;
      mapChunk.shuffleRegionTiles();
    }
  }

  private void calc_links()
  {
    if (this._is_parallel_enabled)
    {
      List<MapChunk> tDirtyChunks = this._dirty_chunks_links;
      int tCount = tDirtyChunks.Count;
      if (!this._batches_enabled)
      {
        Parallel.For(0, tCount, World.world.parallel_options, (Action<int>) (pIndex => tDirtyChunks[pIndex].calculateLinks()));
      }
      else
      {
        int tDynamicBatchSize = ParallelHelper.getDynamicBatchSize(tCount);
        Parallel.For(0, ParallelHelper.calcTotalBatches(tCount, tDynamicBatchSize), World.world.parallel_options, (Action<int>) (pBatchIndex =>
        {
          int batchBeg = ParallelHelper.calculateBatchBeg(pBatchIndex, tDynamicBatchSize);
          int batchEnd = ParallelHelper.calculateBatchEnd(batchBeg, tDynamicBatchSize, tCount);
          for (int index = batchBeg; index < batchEnd; ++index)
            tDirtyChunks[index].calculateLinks();
        }));
      }
    }
    else
    {
      List<MapChunk> dirtyChunksLinks = this._dirty_chunks_links;
      int count = dirtyChunksLinks.Count;
      for (int index = 0; index < count; ++index)
        dirtyChunksLinks[index].calculateLinks();
    }
  }

  private void calc_checkLinkResults()
  {
    List<MapChunk> dirtyChunksLinks = this._dirty_chunks_links;
    int count = dirtyChunksLinks.Count;
    for (int index = 0; index < count; ++index)
      dirtyChunksLinks[index].checkLinksResults();
  }

  private void calc_linkedRegions()
  {
    if (this._is_parallel_enabled)
    {
      List<MapChunk> tDirtyChunks = this._dirty_chunks_links;
      int tCount = tDirtyChunks.Count;
      if (!this._batches_enabled)
      {
        Parallel.For(0, tCount, World.world.parallel_options, (Action<int>) (pIndex => this.calculateRegionNeighbours(tDirtyChunks[pIndex])));
      }
      else
      {
        int tDynamicBatchSize = ParallelHelper.getDynamicBatchSize(tCount);
        Parallel.For(0, ParallelHelper.calcTotalBatches(tCount, tDynamicBatchSize), World.world.parallel_options, (Action<int>) (pBatchIndex =>
        {
          int batchBeg = ParallelHelper.calculateBatchBeg(pBatchIndex, tDynamicBatchSize);
          int batchEnd = ParallelHelper.calculateBatchEnd(batchBeg, tDynamicBatchSize, tCount);
          for (int index = batchBeg; index < batchEnd; ++index)
            this.calculateRegionNeighbours(tDirtyChunks[index]);
        }));
      }
    }
    else
    {
      List<MapChunk> dirtyChunksLinks = this._dirty_chunks_links;
      int count = dirtyChunksLinks.Count;
      for (int index = 0; index < count; ++index)
        this.calculateRegionNeighbours(dirtyChunksLinks[index]);
    }
  }

  private void calc_tileCornersPrepare(HashSet<MapRegion> pSetRegionsResult)
  {
    List<MapChunk> dirtyChunksLinks = this._dirty_chunks_links;
    int count1 = dirtyChunksLinks.Count;
    for (int index1 = 0; index1 < count1; ++index1)
    {
      List<MapRegion> regions = dirtyChunksLinks[index1].regions;
      int count2 = regions.Count;
      for (int index2 = 0; index2 < count2; ++index2)
      {
        MapRegion mapRegion = regions[index2];
        pSetRegionsResult.Add(mapRegion);
      }
    }
  }

  private void calc_centerRegions()
  {
    foreach (MapRegion region in this._region_set)
      region.calculateCenterRegion();
  }

  private int calc_islandRegionEdges(ListPool<TileIsland> pNewIslands)
  {
    int num = 0;
    for (int index1 = 0; index1 < pNewIslands.Count; ++index1)
    {
      TileIsland pNewIsland = pNewIslands[index1];
      List<MapRegion> simpleList = pNewIsland.regions.getSimpleList();
      for (int index2 = 0; index2 < simpleList.Count; ++index2)
      {
        MapRegion mapRegion = simpleList[index2];
        ++num;
        if (!mapRegion.center_region)
          pNewIsland.insideRegionEdges.Add(mapRegion);
      }
    }
    return num;
  }

  private void calc_tileEdges()
  {
    HashSet<MapRegion> regionSet = this._region_set;
    if (this._is_parallel_enabled)
    {
      Parallel.ForEach<MapRegion>((IEnumerable<MapRegion>) regionSet, World.world.parallel_options, (Action<MapRegion>) (tReg => tReg.calculateTileEdges()));
    }
    else
    {
      foreach (MapRegion mapRegion in regionSet)
        mapRegion.calculateTileEdges();
    }
  }

  private void prepareDirtyNeighbourIslands()
  {
    // ISSUE: unable to decompile the method.
  }

  private void calc_neighbourIslands()
  {
    List<TileIsland> tIslandsWithDirtyNeighbours = this._temp_dirty_neighbours_islands;
    if (this._is_parallel_enabled)
    {
      int tCount = tIslandsWithDirtyNeighbours.Count;
      if (!this._batches_enabled)
      {
        Parallel.For(0, tCount, World.world.parallel_options, (Action<int>) (pIndex => tIslandsWithDirtyNeighbours[pIndex].calcNeighbourIslands()));
      }
      else
      {
        int tDynamicBatchSize = ParallelHelper.DEBUG_BATCH_SIZE;
        Parallel.For(0, ParallelHelper.calcTotalBatches(tCount, tDynamicBatchSize), World.world.parallel_options, (Action<int>) (pBatchIndex =>
        {
          int batchBeg = ParallelHelper.calculateBatchBeg(pBatchIndex, tDynamicBatchSize);
          int batchEnd = ParallelHelper.calculateBatchEnd(batchBeg, tDynamicBatchSize, tCount);
          for (int index = batchBeg; index < batchEnd; ++index)
            tIslandsWithDirtyNeighbours[index].calcNeighbourIslands();
        }));
      }
    }
    else
    {
      for (int index = 0; index < tIslandsWithDirtyNeighbours.Count; ++index)
        tIslandsWithDirtyNeighbours[index].calcNeighbourIslands();
    }
  }

  private void checkWrongIslands()
  {
    MapChunk[] chunks = this.chunks;
    int length = chunks.Length;
    for (int index = 0; index < length; ++index)
    {
      MapChunk mapChunk = chunks[index];
      foreach (MapRegion region in mapChunk.regions)
      {
        foreach (WorldTile tile in region.tiles)
        {
          if (tile.Type.layer_type != region.island.type)
          {
            bool flag1 = this._last_dirty_chunks.Contains(mapChunk);
            bool flag2 = this._last_dirty_links.Contains(mapChunk);
            Debug.LogError((object) $"Wrong island type: {tile.Type.layer_type.ToString()} != {region.island.type.ToString()} {tile.chunk.id.ToString()} - was dirty: {flag1.ToString()} | {flag2.ToString()}");
            break;
          }
        }
      }
    }
  }

  private void calculateRegionNeighbours(MapChunk pChunk)
  {
    for (int index = 0; index < pChunk.regions.Count; ++index)
      pChunk.regions[index].calculateNeighbours();
  }

  public int countRegions()
  {
    int num = 0;
    MapChunk[] chunks = this.chunks;
    int length = chunks.Length;
    for (int index = 0; index < length; ++index)
    {
      MapChunk mapChunk = chunks[index];
      num += mapChunk.regions.Count;
    }
    return num;
  }

  private bool _is_parallel_enabled => DebugConfig.isOn(DebugOption.ParallelChunks);

  private bool _batches_enabled => DebugConfig.isOn(DebugOption.ChunkBatches);
}
