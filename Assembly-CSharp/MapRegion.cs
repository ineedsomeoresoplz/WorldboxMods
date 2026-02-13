// Decompiled with JetBrains decompiler
// Type: MapRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class MapRegion : IEquatable<MapRegion>
{
  public int id;
  public bool used_by_path_lock;
  public int region_path_id = -1;
  public TileLayerType type;
  public readonly float created;
  private readonly List<WorldTile> _edge_tiles = new List<WorldTile>(4);
  public readonly HashSet<WorldTile> edge_tiles_set = new HashSet<WorldTile>();
  private readonly HashSet<MapRegion> _outside_edge_regions = new HashSet<MapRegion>();
  private readonly HashSet<TileIsland> _outside_islands = new HashSet<TileIsland>();
  public readonly List<WorldTile> tiles = new List<WorldTile>(256 /*0x0100*/);
  private readonly List<RegionLink> _links = new List<RegionLink>(4);
  public TileIsland island;
  public bool is_island_checked;
  public bool is_checked_path;
  public int path_wave_id = -1;
  public bool region_path;
  public MapChunk chunk;
  public readonly HashSet<TileZone> zones = new HashSet<TileZone>();
  public bool center_region;
  public readonly List<MapRegion> neighbours = new List<MapRegion>(4);
  private readonly HashSet<MapRegion> _neighbours_hash = new HashSet<MapRegion>();
  public static float created_time_last;
  public List<WorldTile> debug_blink_edges_left;
  public List<WorldTile> debug_blink_edges_up;
  public List<WorldTile> debug_blink_edges_down;
  public List<WorldTile> debug_blink_edges_right;

  public MapRegion() => this.created = MapRegion.created_time_last;

  public void reset()
  {
    this._edge_tiles.Clear();
    this.edge_tiles_set.Clear();
    this._outside_edge_regions.Clear();
    this._outside_islands.Clear();
    this.tiles.Clear();
    this.zones.Clear();
    this._links.Clear();
    this.island = (TileIsland) null;
    this.is_island_checked = false;
    this.is_checked_path = false;
    this.path_wave_id = -1;
    this.region_path = false;
    this.chunk = (MapChunk) null;
    this.center_region = false;
    this.neighbours.Clear();
    this._neighbours_hash.Clear();
    this.debug_blink_edges_left?.Clear();
    this.debug_blink_edges_up?.Clear();
    this.debug_blink_edges_down?.Clear();
    this.debug_blink_edges_right?.Clear();
  }

  public void checkZones()
  {
    if (this.chunk.regions.Count == 1)
    {
      this.zones.UnionWith((IEnumerable<TileZone>) this.chunk.zones);
    }
    else
    {
      List<WorldTile> tiles = this.tiles;
      int count = tiles.Count;
      for (int index = 0; index < count; ++index)
        this.zones.Add(tiles[index].zone);
    }
  }

  private void checkDebugLists()
  {
    if (!DebugConfig.isOn(DebugOption.Connections) || this.debug_blink_edges_left != null)
      return;
    this.debug_blink_edges_left = new List<WorldTile>();
    this.debug_blink_edges_up = new List<WorldTile>();
    this.debug_blink_edges_down = new List<WorldTile>();
    this.debug_blink_edges_right = new List<WorldTile>();
  }

  internal int newConnection(
    List<WorldTile> pTiles,
    LinkDirection pDirection,
    LinkDirection pDirectionID)
  {
    return this.newHash(pTiles, pTiles.Count, pDirection, pDirectionID);
  }

  private void checkDebugEdges(List<WorldTile> pTiles, LinkDirection pDirection)
  {
    if (!DebugConfig.isOn(DebugOption.Connections))
      return;
    List<WorldTile> worldTileList = (List<WorldTile>) null;
    this.checkDebugLists();
    switch (pDirection)
    {
      case LinkDirection.Up:
        worldTileList = this.debug_blink_edges_up;
        break;
      case LinkDirection.Down:
        worldTileList = this.debug_blink_edges_down;
        break;
      case LinkDirection.Left:
        worldTileList = this.debug_blink_edges_left;
        break;
      case LinkDirection.Right:
        worldTileList = this.debug_blink_edges_right;
        break;
    }
    worldTileList?.AddRange((IEnumerable<WorldTile>) pTiles);
  }

  private int newHash(
    List<WorldTile> pTiles,
    int pLen,
    LinkDirection pDirection,
    LinkDirection pDirectionID)
  {
    WorldTile pTile = pTiles[0];
    int x = pTile.x;
    int y = pTile.y;
    int num = (int) (pTile.Type.layer_type + 1) * 100000 + (x * 11 + y * 3) * 10000 + pLen * 1000 + x * 10 + y;
    if (pDirectionID == LinkDirection.LR)
      num = -num;
    return num;
  }

  public void clear()
  {
    for (int index = 0; index < this._links.Count; ++index)
      RegionLinkHashes.remove(this._links[index], this);
    this._links.Clear();
    this.region_path_id = -1;
  }

  public void addLink(RegionLink pLink) => this._links.Add(pLink);

  public void calculateNeighbours()
  {
    this.neighbours.Clear();
    this._neighbours_hash.Clear();
    for (int index = 0; index < this._links.Count; ++index)
    {
      foreach (MapRegion region in (HashSet<MapRegion>) this._links[index].regions)
      {
        if (region != this)
          this._neighbours_hash.Add(region);
      }
    }
    this.neighbours.AddRange((IEnumerable<MapRegion>) this._neighbours_hash);
  }

  public bool hasNeighbour(MapRegion pRegion) => this._neighbours_hash.Contains(pRegion);

  public void debugLinks(DebugTool pTool)
  {
    pTool.setText("- links:", (object) this._links.Count);
    List<RegionLink> regionLinkList = new List<RegionLink>((IEnumerable<RegionLink>) this._links);
    regionLinkList.Sort(new Comparison<RegionLink>(this.linkSortByID));
    for (int index = 0; index < regionLinkList.Count; ++index)
    {
      RegionLink regionLink = regionLinkList[index];
      pTool.setText($"- hash {index.ToString()}:", (object) regionLinkList[index].id);
    }
  }

  private int linkSortByID(RegionLink o1, RegionLink o2) => o2.id.CompareTo(o1.id);

  public void calculateCenterRegion()
  {
    this.center_region = true;
    if (this.chunk.regions.Count > 1)
    {
      this.center_region = false;
    }
    else
    {
      foreach (MapChunk mapChunk in this.chunk.neighbours_all)
      {
        List<MapRegion> regions = mapChunk.regions;
        if (regions.Count > 1)
        {
          this.center_region = false;
          break;
        }
        if (regions[0].island != this.island)
        {
          this.center_region = false;
          break;
        }
      }
    }
  }

  public void calculateTileEdges()
  {
    if (this.center_region)
      return;
    this._edge_tiles.Clear();
    this._outside_edge_regions.Clear();
    this._outside_islands.Clear();
    foreach (WorldTile edgeTiles in this.edge_tiles_set)
    {
      if (edgeTiles.Type.layer_type != this.type)
      {
        this._edge_tiles.Add(edgeTiles);
        this._outside_edge_regions.Add(edgeTiles.region);
      }
    }
  }

  public WorldTile getRandomTile() => this.tiles.GetRandom<WorldTile>();

  public bool isTypeGround() => this.type == TileLayerType.Ground;

  public List<WorldTile> getEdgeTiles() => this._edge_tiles;

  public HashSet<MapRegion> getEdgeRegions() => this._outside_edge_regions;

  public override int GetHashCode() => this.id;

  public override bool Equals(object obj) => this.Equals(obj as MapRegion);

  public bool Equals(MapRegion pObject) => this.id == pObject.id;
}
