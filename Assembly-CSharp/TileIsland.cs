// Decompiled with JetBrains decompiler
// Type: TileIsland
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public sealed class TileIsland
{
  public readonly RegionsContainer regions = new RegionsContainer();
  public readonly HashSet<MapRegion> insideRegionEdges = new HashSet<MapRegion>();
  public readonly HashSet<MapRegion> outsideRegionEdges = new HashSet<MapRegion>();
  public readonly float created;
  public TileLayerType type = TileLayerType.Ocean;
  public bool dirty;
  private bool _dirty_neighbours = true;
  public readonly int id;
  public readonly int debug_hash_code;
  public readonly HashSetWorldTile tiles_roads = new HashSetWorldTile();
  internal bool removed;
  public readonly List<Actor> actors = new List<Actor>();
  public ListPool<Docks> docks;
  private int _tile_count;
  private readonly Dictionary<TileIsland, bool> _cached_reachable_islands_check = new Dictionary<TileIsland, bool>();
  private readonly HashSet<TileIsland> _connected_islands = new HashSet<TileIsland>();

  public TileIsland(int pID)
  {
    this.id = pID;
    this.debug_hash_code = this.GetHashCode();
    this.created = Time.time;
  }

  public void reset()
  {
    this.removed = false;
    this._tile_count = 0;
    this.regions.Clear();
    this.insideRegionEdges.Clear();
    this.outsideRegionEdges.Clear();
    this.tiles_roads.Clear();
    this.actors.Clear();
    this.docks?.Dispose();
    this.docks = (ListPool<Docks>) null;
    this._cached_reachable_islands_check.Clear();
    this._connected_islands.Clear();
    this.dirty = false;
    this._dirty_neighbours = true;
  }

  public void addDock(Building pBuilding)
  {
    if (this.docks == null)
      this.docks = new ListPool<Docks>();
    this.docks.Add(pBuilding.component_docks);
  }

  public void clearCache() => this._cached_reachable_islands_check.Clear();

  public void addRegion(MapRegion pRegion) => this.regions.Add(pRegion);

  public void removeRegion(MapRegion pRegion)
  {
    this.regions.Remove(pRegion);
    pRegion.island = (TileIsland) null;
    this.dirty = true;
  }

  public void clearRegionsFromIsland()
  {
    if (this.removed)
      return;
    this.removed = true;
    List<MapRegion> simpleList = this.regions.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
      simpleList[index].island = (TileIsland) null;
    this.regions.Clear();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int getTileCount() => this._tile_count;

  internal void countTiles()
  {
    this._tile_count = 0;
    List<MapRegion> simpleList = this.regions.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
      this._tile_count += simpleList[index].tiles.Count;
  }

  internal WorldTile getRandomTile() => this.regions.GetRandom().tiles.GetRandom<WorldTile>();

  internal HashSet<TileIsland> getConnectedIslands() => this._connected_islands;

  public void setDirtyIslandNeighbours() => this._dirty_neighbours = true;

  public bool isDirtyNeighbours() => this._dirty_neighbours;

  public void calcNeighbourIslands()
  {
    if (!this._dirty_neighbours)
      return;
    this._dirty_neighbours = false;
    HashSet<TileIsland> connectedIslands = this._connected_islands;
    HashSet<MapRegion> outsideRegionEdges = this.outsideRegionEdges;
    HashSet<MapRegion> insideRegionEdges = this.insideRegionEdges;
    connectedIslands.Clear();
    outsideRegionEdges.Clear();
    foreach (MapRegion mapRegion1 in insideRegionEdges)
    {
      HashSet<MapRegion> edgeRegions = mapRegion1.getEdgeRegions();
      if (edgeRegions.Count != 0)
      {
        foreach (MapRegion mapRegion2 in edgeRegions)
        {
          if (outsideRegionEdges.Add(mapRegion2))
            connectedIslands.Add(mapRegion2.island);
        }
      }
    }
  }

  public bool goodForDocks() => this.getTileCount() >= 2500;

  public bool reachableByCityFrom(TileIsland pIsland)
  {
    if (this._cached_reachable_islands_check.ContainsKey(pIsland))
      return this._cached_reachable_islands_check[pIsland];
    if (pIsland == this)
      return false;
    HashSet<TileIsland> connectedIslands1 = this.getConnectedIslands();
    HashSet<TileIsland> connectedIslands2 = pIsland.getConnectedIslands();
    foreach (TileIsland tileIsland1 in connectedIslands1)
    {
      foreach (TileIsland tileIsland2 in connectedIslands2)
      {
        if (tileIsland1 == tileIsland2 && tileIsland1.type == TileLayerType.Ocean && tileIsland1.goodForDocks())
        {
          this._cached_reachable_islands_check[pIsland] = true;
          return true;
        }
      }
    }
    this._cached_reachable_islands_check[pIsland] = false;
    return false;
  }

  public bool isConnectedWith(TileIsland pIsland) => this._connected_islands.Contains(pIsland);

  internal bool isGoodIslandForActor(Actor pActor)
  {
    return this.type != TileLayerType.Block && (this.type != TileLayerType.Ocean || pActor.isWaterCreature()) && (this.type != TileLayerType.Ground || !pActor.isWaterCreature()) && (this.type != TileLayerType.Lava || !pActor.asset.die_in_lava) && this._tile_count > 5 && (this._tile_count >= 100 || this._tile_count >= this.actors.Count);
  }
}
