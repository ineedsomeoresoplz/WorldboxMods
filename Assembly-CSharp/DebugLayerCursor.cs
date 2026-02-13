// Decompiled with JetBrains decompiler
// Type: DebugLayerCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DebugLayerCursor : MapLayer
{
  private Color color_highlight_white;
  private Color color_main;
  private Color color_neighbour;
  private Color color_neighbour_2;
  private Color color_region;
  private Color color_edges;
  private Color color_chunk_bounds;
  private Color color_edges_blink;
  private List<WorldTile> _tiles = new List<WorldTile>();
  private bool blink = true;
  private float timerBlink = 0.2f;
  private float timerRecalc = 0.1f;
  private MapChunk lastChunk;

  internal override void create()
  {
    base.create();
    this.color_highlight_white = Toolbox.makeColor("#FFFFFF77");
    this.color_main = new Color(0.0f, 1f, 0.0f, 0.1f);
    this.color_neighbour = new Color(1f, 0.0f, 1f, 0.8f);
    this.color_neighbour_2 = new Color(1f, 0.0f, 1f, 0.3f);
    this.color_edges = new Color(1f, 0.0f, 0.0f, 0.5f);
    this.color_chunk_bounds = new Color(0.0f, 1f, 1f, 0.5f);
    this.color_edges_blink = new Color(0.1f, 0.1f, 1f, 1f);
    this.color_region = new Color(0.0f, 0.0f, 1f, 0.8f);
  }

  protected override void UpdateDirty(float pElapsed)
  {
    if (ScrollWindow.isWindowActive())
      return;
    if (!Config.isEditor && !DebugConfig.instance.debugButton.gameObject.activeSelf)
    {
      this.clear();
    }
    else
    {
      if ((double) this.timerBlink > 0.0)
      {
        this.timerBlink -= Time.deltaTime;
      }
      else
      {
        this.timerBlink = 0.2f;
        this.blink = !this.blink;
      }
      if ((double) this.timerRecalc > 0.0)
      {
        this.timerRecalc -= pElapsed;
        this.clear();
        WorldTile mouseTilePos = World.world.getMouseTilePos();
        if (mouseTilePos == null)
          return;
        this.lastChunk = mouseTilePos.chunk;
        MapChunk chunk = mouseTilePos.chunk;
        MapChunk lastChunk = this.lastChunk;
        if (DebugConfig.isOn(DebugOption.RenderIslands) && mouseTilePos?.region?.island != null)
          this.drawIsland(mouseTilePos.region.island);
        if (DebugConfig.isOn(DebugOption.CursorChunk))
          this.fill(this.lastChunk.tiles, this.color_highlight_white);
        if (DebugConfig.isOn(DebugOption.RenderConnectedIslands) && mouseTilePos?.region?.island != null)
        {
          foreach (TileIsland connectedIsland in mouseTilePos.region.island.getConnectedIslands())
          {
            foreach (MapRegion region in (ObjectContainer<MapRegion>) connectedIsland.regions)
              this.fill(region.tiles, Color.blue);
          }
        }
        if (DebugConfig.isOn(DebugOption.PossibleCityReach))
          this.renderPossibleCityReach();
        if (DebugConfig.isOn(DebugOption.RenderIslandsInsideRegionCorners) && mouseTilePos?.region?.island != null)
        {
          foreach (MapRegion insideRegionEdge in mouseTilePos.region.island.insideRegionEdges)
            this.fill(insideRegionEdge.tiles, Color.magenta);
        }
        if (DebugConfig.isOn(DebugOption.RenderIslandsTileCorners) && mouseTilePos?.region?.island != null)
        {
          foreach (MapRegion insideRegionEdge in mouseTilePos.region.island.insideRegionEdges)
            this.fill(insideRegionEdge.getEdgeTiles(), Color.red);
        }
        if (DebugConfig.isOn(DebugOption.RenderIslandCenterRegions) && mouseTilePos?.region?.island != null)
        {
          foreach (MapRegion region in (ObjectContainer<MapRegion>) mouseTilePos.region.island.regions)
          {
            if (!region.center_region)
              this.fill(region.tiles, Color.red);
          }
        }
        if (DebugConfig.isOn(DebugOption.RenderRegionOutsideRegionCorners) && mouseTilePos?.region != null)
        {
          foreach (MapRegion edgeRegion in mouseTilePos.region.getEdgeRegions())
            this.fill(edgeRegion.tiles, Color.yellow);
        }
        if (DebugConfig.isOn(DebugOption.RenderMapRegionEdges) && mouseTilePos.region != null)
          this.fill(mouseTilePos.region.getEdgeTiles(), Color.red);
        if (DebugConfig.isOn(DebugOption.RegionNeighbours) && mouseTilePos.region != null)
        {
          HashSet<MapRegion> mapRegionSet1 = new HashSet<MapRegion>();
          HashSet<MapRegion> mapRegionSet2 = new HashSet<MapRegion>();
          mapRegionSet1.Add(mouseTilePos.region);
          foreach (MapRegion neighbour in mouseTilePos.region.neighbours)
            mapRegionSet1.Add(neighbour);
          foreach (MapRegion mapRegion in mapRegionSet1)
          {
            foreach (MapRegion neighbour in mapRegion.neighbours)
            {
              if (!mapRegionSet1.Contains(neighbour))
                mapRegionSet2.Add(neighbour);
            }
          }
          foreach (MapRegion mapRegion in mapRegionSet1)
            this.fill(mapRegion.tiles, this.color_neighbour);
          foreach (MapRegion mapRegion in mapRegionSet2)
            this.fill(mapRegion.tiles, this.color_neighbour_2);
        }
        if (DebugConfig.isOn(DebugOption.Region) && mouseTilePos.region != null)
          this.fill(mouseTilePos.region.tiles, this.color_region);
        if (DebugConfig.isOn(DebugOption.ConnectedZones) && mouseTilePos.zone != null)
        {
          TileZone zone = mouseTilePos.zone;
          MapRegion region = mouseTilePos.region;
          this.fill(zone.tiles, this.color_region);
          using (ListPool<MapRegion> pListToFill = new ListPool<MapRegion>())
          {
            foreach (TileZone neighbour in zone.neighbours)
            {
              pListToFill.Clear();
              if (TileZone.hasZonesConnectedViaRegions(zone, neighbour, region, pListToFill))
                this.fill(neighbour.tiles, this.color_neighbour);
            }
          }
        }
        if (DebugConfig.isOn(DebugOption.ChunkEdges) && mouseTilePos.chunk != null)
          this.fill(mouseTilePos.chunk.edges_all, this.color_edges);
        if (DebugConfig.isOn(DebugOption.ChunkBounds) && mouseTilePos.chunk != null)
          this.fill(mouseTilePos.chunk.chunk_bounds, this.color_chunk_bounds);
        if (DebugConfig.isOn(DebugOption.Connections) && mouseTilePos.region != null)
          this.drawConnections(mouseTilePos);
        this.updatePixels();
      }
      else
        this.timerRecalc = 0.1f;
    }
  }

  private void renderPossibleCityReach()
  {
    // ISSUE: unable to decompile the method.
  }

  private void drawIsland(TileIsland pIsland)
  {
    Color32 color32 = Color32.op_Implicit(Color.red);
    foreach (MapRegion region in (ObjectContainer<MapRegion>) pIsland.regions)
    {
      this._tiles.AddRange((IEnumerable<WorldTile>) region.tiles);
      foreach (WorldTile tile in region.tiles)
        this.pixels[tile.data.tile_id] = color32;
    }
  }

  private void drawConnections(WorldTile pTile)
  {
    if (!this.blink || pTile.region.debug_blink_edges_up == null)
      return;
    this.fill(pTile.region.debug_blink_edges_up, this.color_edges_blink, true);
    this.fill(pTile.region.debug_blink_edges_down, this.color_edges_blink, true);
    this.fill(pTile.region.debug_blink_edges_left, this.color_edges_blink, true);
    this.fill(pTile.region.debug_blink_edges_right, this.color_edges_blink, true);
  }

  private void fill(List<WorldTile> pTiles, Color pColor, bool pEdge = false)
  {
    this.createTextureNew();
    for (int index = 0; index < pTiles.Count; ++index)
    {
      WorldTile pTile = pTiles[index];
      if (!pEdge || pTile.region != null)
      {
        this._tiles.Add(pTile);
        this.pixels[pTile.data.tile_id] = Color32.op_Implicit(pColor);
      }
    }
  }

  private void fill(WorldTile[] pTiles, Color pColor, bool pEdge = false)
  {
    this.createTextureNew();
    for (int index = 0; index < pTiles.Length; ++index)
    {
      WorldTile pTile = pTiles[index];
      if (!pEdge || pTile.region != null)
      {
        this._tiles.Add(pTile);
        this.pixels[pTile.data.tile_id] = Color32.op_Implicit(pColor);
      }
    }
  }

  internal override void clear()
  {
    if (this._tiles.Count == 0)
      return;
    this._tiles.Clear();
    for (int index = 0; index < this.pixels.Length; ++index)
      this.pixels[index] = Color32.op_Implicit(Color.clear);
    this.createTextureNew();
  }
}
