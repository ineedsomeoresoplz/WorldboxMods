// Decompiled with JetBrains decompiler
// Type: MapChunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MapChunk : IDisposable
{
  private readonly Queue<WorldTile> _wave = new Queue<WorldTile>(256 /*0x0100*/);
  public readonly ChunkObjectContainer objects = new ChunkObjectContainer();
  public MapChunk[] neighbours;
  public MapChunk[] neighbours_all;
  public readonly WorldTile[] tiles = new WorldTile[256 /*0x0100*/];
  public readonly List<MapRegion> regions = new List<MapRegion>(4);
  public List<WorldTile> edges_all;
  public List<WorldTile> chunk_bounds;
  public readonly List<WorldTile> bounds_left = new List<WorldTile>(16 /*0x10*/);
  public readonly List<WorldTile> bounds_up = new List<WorldTile>(16 /*0x10*/);
  public readonly List<WorldTile> bounds_down = new List<WorldTile>(16 /*0x10*/);
  public readonly List<WorldTile> bounds_right = new List<WorldTile>(16 /*0x10*/);
  public float world_center_x;
  public float world_center_y;
  public WorldTile edge_up_left;
  public WorldTile edge_up_right;
  public WorldTile edge_down_left;
  public WorldTile edge_down_right;
  private WorldTile _edge_up_left_connection;
  private WorldTile _edge_up_right_connection;
  private WorldTile _edge_down_left_connection;
  private WorldTile _edge_down_right_connection;
  public bool world_edge;
  public int x;
  public int y;
  public int id;
  public Color color;
  public bool dirty_regions;
  public bool dirty_links;
  private readonly List<WorldTile> _temp_tiles = new List<WorldTile>(16 /*0x10*/);
  private readonly List<TempLinkStruct> _new_hashes = new List<TempLinkStruct>();
  internal MapChunk chunk_up;
  internal MapChunk chunk_down;
  internal MapChunk chunk_left;
  internal MapChunk chunk_right;
  private readonly StackPool<MapRegion> _region_pool = new StackPool<MapRegion>();
  public readonly List<TileZone> zones = new List<TileZone>();
  private bool _buildings_dirty;
  private bool _tile_types_dirty;
  private const int MAX_TILE_TYPES = 256 /*0x0100*/;
  private readonly Dictionary<TileTypeBase, int> _tile_types_count = new Dictionary<TileTypeBase, int>(256 /*0x0100*/);
  private readonly List<MusicBoxTileData> _simple_data = new List<MusicBoxTileData>();

  internal void addTile(WorldTile pTile, int pX, int pY)
  {
    this.tiles[this.tiles.FreeIndex<WorldTile>()] = pTile;
  }

  internal void addNeighbour(
    MapChunk pNeighbour,
    TileDirection pDirection,
    IList<MapChunk> pNeighbours,
    IList<MapChunk> pNeighboursAll,
    bool pDiagonal = false)
  {
    if (pNeighbour == null)
    {
      this.world_edge = true;
    }
    else
    {
      if (!pDiagonal)
        pNeighbours.Add(pNeighbour);
      pNeighboursAll.Add(pNeighbour);
      switch (pDirection)
      {
        case TileDirection.Left:
          this.chunk_left = pNeighbour;
          break;
        case TileDirection.Right:
          this.chunk_right = pNeighbour;
          break;
        case TileDirection.Up:
          this.chunk_up = pNeighbour;
          break;
        case TileDirection.Down:
          this.chunk_down = pNeighbour;
          break;
      }
    }
  }

  public void calculateRegions()
  {
    this.dirty_regions = false;
    WorldTile[] tiles = this.tiles;
    List<MapRegion> regions = this.regions;
    StackPool<MapRegion> regionPool = this._region_pool;
    this.clearTiles();
    int length = tiles.Length;
    for (int index = 0; index < length; ++index)
    {
      WorldTile pTile = tiles[index];
      if (pTile.region == null)
      {
        MapRegion pTargetRegion = regionPool.get();
        pTargetRegion.reset();
        pTargetRegion.type = pTile.Type.layer_type;
        pTargetRegion.chunk = this;
        this.fillRegion(pTile, pTargetRegion);
        pTargetRegion.id = pTile.zone.id * 1000 + regions.Count;
        regions.Add(pTargetRegion);
        if (pTargetRegion.tiles.Count == tiles.Length)
          break;
      }
    }
    this.clearTiles(false);
    int count = regions.Count;
    for (int index = 0; index < count; ++index)
      regions[index].checkZones();
  }

  private void clearTiles(bool pClearRegion = true)
  {
    WorldTile[] tiles = this.tiles;
    if (pClearRegion)
    {
      int length = tiles.Length;
      for (int index = 0; index < length; ++index)
      {
        WorldTile worldTile = tiles[index];
        worldTile.is_checked_tile = false;
        worldTile.region = (MapRegion) null;
      }
    }
    else
    {
      int length = tiles.Length;
      for (int index = 0; index < length; ++index)
        tiles[index].is_checked_tile = false;
    }
  }

  internal void shuffleRegionTiles()
  {
    for (int index = 0; index < this.regions.Count; ++index)
    {
      MapRegion region = this.regions[index];
      region.center_region = this.regions.Count <= 1;
      region.tiles.Shuffle<WorldTile>();
    }
  }

  private void fillRegion(WorldTile pTile, MapRegion pTargetRegion)
  {
    Queue<WorldTile> wave = this._wave;
    wave.Enqueue(pTile);
    while (wave.Count > 0)
    {
      WorldTile pTileMain = wave.Dequeue();
      pTileMain.is_checked_tile = true;
      pTileMain.region = pTargetRegion;
      pTargetRegion.tiles.Add(pTileMain);
      this.processTileNeighbours(pTileMain, pTargetRegion, wave);
    }
    wave.Clear();
  }

  private void processTileNeighbours(
    WorldTile pTileMain,
    MapRegion pTargetRegion,
    Queue<WorldTile> pWave)
  {
    foreach (WorldTile pTileTo in pTileMain.neighboursAll)
    {
      TileTypeBase type = pTileTo.Type;
      if (!pTileTo.is_checked_tile)
      {
        if (type.layer_type != pTileMain.region.type || pTileTo.chunk != this)
          pTargetRegion.edge_tiles_set.Add(pTileTo);
        else if (!this.isDiagonalBlockedByCorners(pTileMain, pTileTo))
        {
          pTileTo.is_checked_tile = true;
          pTileTo.region = pTargetRegion;
          pWave.Enqueue(pTileTo);
        }
      }
    }
  }

  private bool isDiagonalBlockedByCorners(WorldTile pTileFrom, WorldTile pTileTo)
  {
    int num1 = pTileTo.x - pTileFrom.x;
    int num2 = pTileTo.y - pTileFrom.y;
    if (Math.Abs(num1) != 1 || Math.Abs(num2) != 1)
      return false;
    WorldTile tile1 = World.world.GetTile(pTileFrom.x + num1, pTileFrom.y);
    WorldTile tile2 = World.world.GetTile(pTileFrom.x, pTileFrom.y + num2);
    return ((tile1 == null ? 1 : (tile1.Type.block ? 1 : 0)) | (tile2 == null ? (true ? 1 : 0) : (tile2.Type.block ? 1 : 0))) != 0;
  }

  public void clearObjects(bool pForceClearBuildings = false)
  {
    if (pForceClearBuildings)
      this.setBuildingsDirty();
    this.objects.reset(this.buildings_dirty);
    this._temp_tiles.Clear();
    this._new_hashes.Clear();
  }

  public void clearRegions()
  {
    this.clearIsland();
    for (int index = 0; index < this.regions.Count; ++index)
    {
      MapRegion region = this.regions[index];
      region.reset();
      this._region_pool.release(region);
    }
    this.regions.Clear();
  }

  public void Dispose()
  {
    this.clearRegions();
    this.clearObjects(true);
    this.setBuildingsDirty();
    this.objects.Dispose();
    this.neighbours.Clear<MapChunk>();
    this.neighbours_all.Clear<MapChunk>();
    this.neighbours = (MapChunk[]) null;
    this.neighbours_all = (MapChunk[]) null;
    this.tiles.Clear<WorldTile>();
    this._wave.Clear();
    this.edges_all?.Clear();
    this.chunk_bounds?.Clear();
    this.bounds_left.Clear();
    this.bounds_up.Clear();
    this.bounds_down.Clear();
    this.bounds_right.Clear();
    this.chunk_up = (MapChunk) null;
    this.chunk_down = (MapChunk) null;
    this.chunk_left = (MapChunk) null;
    this.chunk_right = (MapChunk) null;
    this.edge_down_left = (WorldTile) null;
    this.edge_down_right = (WorldTile) null;
    this.edge_up_left = (WorldTile) null;
    this.edge_up_right = (WorldTile) null;
    this._edge_down_left_connection = (WorldTile) null;
    this._edge_down_right_connection = (WorldTile) null;
    this._edge_up_left_connection = (WorldTile) null;
    this._edge_up_right_connection = (WorldTile) null;
    this._region_pool.clear();
    this.zones.Clear();
  }

  public void clearIsland()
  {
    for (int index = 0; index < this.regions.Count; ++index)
    {
      MapRegion region = this.regions[index];
      region.clear();
      if (region.island != null)
        World.world.islands_calculator.makeDirty(region.island);
    }
  }

  internal void combineEdges()
  {
    HashSet<WorldTile> collection = new HashSet<WorldTile>();
    this.edges_all = new List<WorldTile>((IEnumerable<WorldTile>) collection);
    collection.Clear();
    collection.UnionWith((IEnumerable<WorldTile>) this.bounds_down);
    collection.UnionWith((IEnumerable<WorldTile>) this.bounds_left);
    collection.UnionWith((IEnumerable<WorldTile>) this.bounds_right);
    collection.UnionWith((IEnumerable<WorldTile>) this.bounds_up);
    this.chunk_bounds = new List<WorldTile>((IEnumerable<WorldTile>) collection);
  }

  public void generateEdgeConnections()
  {
    this._edge_up_left_connection = this.chunk_left?.chunk_up?.edge_down_right;
    this._edge_up_right_connection = this.chunk_right?.chunk_up?.edge_down_left;
    this._edge_down_left_connection = this.chunk_left?.chunk_down?.edge_up_right;
    this._edge_down_right_connection = this.chunk_right?.chunk_down?.edge_up_left;
  }

  public void checkLinksResults()
  {
    for (int index = 0; index < this._new_hashes.Count; ++index)
    {
      TempLinkStruct newHash = this._new_hashes[index];
      RegionLinkHashes.addHash(newHash.hash, newHash.region);
    }
    MapChunkManager.m_newLinks += this._new_hashes.Count;
    this._new_hashes.Clear();
  }

  internal void calculateLinks()
  {
    this.dirty_links = false;
    this.calculateLink(this.bounds_right, this.chunk_right?.bounds_left, LinkDirection.Right, LinkDirection.LR, true);
    this.calculateLink(this.bounds_left, this.chunk_left?.bounds_right, LinkDirection.Left, LinkDirection.LR);
    this.calculateLink(this.bounds_up, this.chunk_up?.bounds_down, LinkDirection.Up, LinkDirection.UD, true);
    this.calculateLink(this.bounds_down, this.chunk_down?.bounds_up, LinkDirection.Down, LinkDirection.UD);
    this.checkSpecialDiagonalConnection(this.edge_up_left, this._edge_up_left_connection, LinkDirection.Up, LinkDirection.UD);
    this.checkSpecialDiagonalConnection(this.edge_up_right, this._edge_up_right_connection, LinkDirection.Up, LinkDirection.UD, true);
    this.checkSpecialDiagonalConnection(this.edge_down_left, this._edge_down_left_connection, LinkDirection.Down, LinkDirection.UD);
    this.checkSpecialDiagonalConnection(this.edge_down_right, this._edge_down_right_connection, LinkDirection.Down, LinkDirection.UD, true);
  }

  private void checkSpecialDiagonalConnection(
    WorldTile pTileMain,
    WorldTile pTileTarget,
    LinkDirection pDirection,
    LinkDirection pGroupID,
    bool pUseTargetList = false)
  {
    if (pTileTarget == null || !WorldTile.isSameLayer(pTileMain, pTileTarget) || this.isDiagonalBlockedByCorners(pTileMain, pTileTarget))
      return;
    if (pUseTargetList)
      this._temp_tiles.Add(pTileTarget);
    else
      this._temp_tiles.Add(pTileMain);
    this.makeLink(this._temp_tiles, pDirection, pGroupID, pTileMain.region);
  }

  private void calculateLink(
    List<WorldTile> pOurBounds,
    List<WorldTile> pTargetEdgeTiles,
    LinkDirection pDirection,
    LinkDirection pGroupID,
    bool pUseTargetList = false)
  {
    if (pTargetEdgeTiles == null)
      return;
    int count = pOurBounds.Count;
    List<WorldTile> tempTiles = this._temp_tiles;
    tempTiles.Clear();
    bool flag1 = false;
    MapRegion pRegionMain = (MapRegion) null;
    for (int index = 0; index < count; ++index)
    {
      bool flag2 = index == count - 1;
      WorldTile pOurBound1 = pOurBounds[index];
      WorldTile pTargetEdgeTile1 = pTargetEdgeTiles[index];
      bool flag3 = WorldTile.isSameLayer(pOurBound1, pTargetEdgeTile1);
      if (flag3 && !flag1)
      {
        flag1 = true;
        pRegionMain = pOurBound1.region;
      }
      if (!flag2)
      {
        if (flag3)
        {
          WorldTile pOurBound2 = pOurBounds[index + 1];
          flag3 = WorldTile.isSameLayer(pOurBound1, pOurBound2);
        }
        if (flag3)
        {
          WorldTile pTargetEdgeTile2 = pTargetEdgeTiles[index + 1];
          flag3 = WorldTile.isSameLayer(pOurBound1, pTargetEdgeTile2);
        }
      }
      if (!flag3 && !flag1 && this.tryDiagonal(pOurBound1, pTargetEdgeTile1, pDirection, pGroupID, pUseTargetList, tempTiles))
      {
        flag1 = false;
        pRegionMain = (MapRegion) null;
      }
      else
      {
        if (flag2)
          flag3 = false;
        if (flag1 || flag3)
        {
          if (!flag1 & flag3)
          {
            flag1 = true;
            pRegionMain = pOurBound1.region;
            this.saveToConnection(tempTiles, pOurBound1, pTargetEdgeTile1, pUseTargetList);
          }
          else if (flag1 && !flag3)
          {
            this.saveToConnection(tempTiles, pOurBound1, pTargetEdgeTile1, pUseTargetList);
            this.makeLink(tempTiles, pDirection, pGroupID, pRegionMain);
            flag1 = false;
            pRegionMain = (MapRegion) null;
          }
          else
            this.saveToConnection(tempTiles, pOurBound1, pTargetEdgeTile1, pUseTargetList);
        }
      }
    }
  }

  private bool tryDiagonal(
    WorldTile pMainTile,
    WorldTile pTargetTile,
    LinkDirection pDirection,
    LinkDirection pGroupID,
    bool pUseTargetList,
    List<WorldTile> pListConnections)
  {
    bool flag = false;
    WorldTile diagonalConnection1 = this.getDiagonalConnection(pMainTile, pTargetTile, pDirection, true);
    if (diagonalConnection1 != null)
    {
      this.saveToConnection(pListConnections, pMainTile, diagonalConnection1, pUseTargetList);
      this.makeLink(pListConnections, pDirection, pGroupID, pMainTile.region);
      flag = true;
    }
    WorldTile diagonalConnection2 = this.getDiagonalConnection(pMainTile, pTargetTile, pDirection, false);
    if (diagonalConnection2 != null)
    {
      this.saveToConnection(pListConnections, pMainTile, diagonalConnection2, pUseTargetList);
      this.makeLink(pListConnections, pDirection, pGroupID, pMainTile.region);
      flag = true;
    }
    return flag;
  }

  private void saveToConnection(
    List<WorldTile> pList,
    WorldTile pOurTile,
    WorldTile pTargetTile,
    bool pUseTargetList)
  {
    if (pUseTargetList)
      pList.Add(pTargetTile);
    else
      pList.Add(pOurTile);
  }

  private WorldTile getDiagonalConnection(
    WorldTile pOurTile,
    WorldTile pTargetTile,
    LinkDirection pDirection,
    bool pFirst)
  {
    TileLayerType layerType = pOurTile.Type.layer_type;
    WorldTile pTileTo = (WorldTile) null;
    switch (pDirection)
    {
      case LinkDirection.Up:
      case LinkDirection.Down:
        pTileTo = pFirst ? pTargetTile.tile_right : pTargetTile.tile_left;
        break;
      case LinkDirection.Left:
      case LinkDirection.Right:
        pTileTo = pFirst ? pTargetTile.tile_up : pTargetTile.tile_down;
        break;
    }
    if (pTileTo == null)
      return (WorldTile) null;
    if (this.isDiagonalBlockedByCorners(pOurTile, pTileTo))
      return (WorldTile) null;
    return pTileTo.Type.layer_type != layerType ? (WorldTile) null : pTileTo;
  }

  private void makeLink(
    List<WorldTile> pConnectionList,
    LinkDirection pDirection,
    LinkDirection pGroupID,
    MapRegion pRegionMain)
  {
    int num = pRegionMain.newConnection(pConnectionList, pDirection, pGroupID);
    this._new_hashes.Add(new TempLinkStruct()
    {
      region = pRegionMain,
      hash = num
    });
    pConnectionList.Clear();
  }

  public void setBuildingsDirty() => this._buildings_dirty = true;

  public bool buildings_dirty => this._buildings_dirty;

  public void finishBuildingsCheck() => this._buildings_dirty = false;

  public List<MusicBoxTileData> getSimpleData()
  {
    if (this._tile_types_dirty)
      this.musicBoxCheckCount();
    return this._simple_data;
  }

  private void musicBoxCheckCount()
  {
    this._tile_types_dirty = false;
    this._tile_types_count.Clear();
    this._simple_data.Clear();
    int index = 0;
    TileTypeBase key;
    for (int count1 = this.zones.Count; index < count1; ++index)
    {
      foreach (KeyValuePair<TileTypeBase, HashSet<WorldTile>> tileType in this.zones[index].getTileTypes())
      {
        HashSet<WorldTile> worldTileSet1;
        tileType.Deconstruct(ref key, ref worldTileSet1);
        TileTypeBase tileTypeBase = key;
        HashSet<WorldTile> worldTileSet2 = worldTileSet1;
        if (tileTypeBase.music_assets != null)
        {
          int count2 = worldTileSet2.Count;
          if (!this._tile_types_count.TryAdd(tileTypeBase, count2))
          {
            Dictionary<TileTypeBase, int> tileTypesCount = this._tile_types_count;
            key = tileTypeBase;
            tileTypesCount[key] += count2;
          }
        }
      }
    }
    foreach (KeyValuePair<TileTypeBase, int> keyValuePair in this._tile_types_count)
    {
      int num1;
      keyValuePair.Deconstruct(ref key, ref num1);
      TileTypeBase tileTypeBase = key;
      int num2 = num1;
      this._simple_data.Add(new MusicBoxTileData()
      {
        tile_type_id = tileTypeBase.index_id,
        amount = num2
      });
    }
  }

  public Dictionary<TileTypeBase, int> getTileTypesCount()
  {
    if (this._tile_types_dirty)
      this.musicBoxCheckCount();
    return this._tile_types_count;
  }

  public int countTilesOfType(TileTypeBase pType)
  {
    if (this._tile_types_dirty)
      this.musicBoxCheckCount();
    int num;
    this._tile_types_count.TryGetValue(pType, out num);
    return num;
  }

  public void setTileTypesDirty() => this._tile_types_dirty = true;
}
