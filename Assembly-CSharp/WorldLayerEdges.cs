// Decompiled with JetBrains decompiler
// Type: WorldLayerEdges
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class WorldLayerEdges : MapLayer
{
  private HashSet<MapChunk> _dirty_chunks = new HashSet<MapChunk>();
  private HashSet<MapChunk> _chunks_to_redraw = new HashSet<MapChunk>();

  public override void update(float pElapsed)
  {
  }

  public override void draw(float pElapsed) => this.UpdateDirty(pElapsed);

  public void addDirtyChunk(MapChunk pChunk) => this._dirty_chunks.Add(pChunk);

  internal override void clear()
  {
    this._dirty_chunks.Clear();
    base.clear();
  }

  public void redraw()
  {
    this._chunks_to_redraw.UnionWith((IEnumerable<MapChunk>) this._dirty_chunks);
    foreach (MapChunk dirtyChunk in this._dirty_chunks)
      this._chunks_to_redraw.UnionWith((IEnumerable<MapChunk>) dirtyChunk.neighbours_all);
    this._dirty_chunks.Clear();
    foreach (MapChunk mapChunk in this._chunks_to_redraw)
    {
      WorldTile[] tiles = mapChunk.tiles;
      int length = tiles.Length;
      for (int index = 0; index < length; ++index)
      {
        WorldTile pTile = tiles[index];
        this.pixels[pTile.data.tile_id] = Toolbox.clear;
        this.redrawTile(pTile);
      }
    }
    this._chunks_to_redraw.Clear();
    this.updatePixels();
  }

  private void redrawTile(WorldTile pTile)
  {
    WorldTile tileDown = pTile.tile_down;
    WorldTile tileUp = pTile.tile_up;
    if (!pTile.Type.check_edge || pTile.Type.wall)
      return;
    if (tileDown != null && !tileDown.Type.wall)
    {
      if (pTile.Type.edge_hills && !tileDown.Type.rocks)
      {
        this.pixels[pTile.data.tile_id] = TileTypeBase.edge_color_hills;
        return;
      }
      if (pTile.Type.edge_mountains && tileDown.Type.edge_hills)
      {
        this.pixels[pTile.data.tile_id] = TileTypeBase.edge_color_mountain;
        return;
      }
      if (pTile.Type.rocks && !tileDown.Type.rocks)
      {
        this.pixels[pTile.data.tile_id] = TileTypeBase.edge_color_mountain;
        return;
      }
      if (!tileDown.Type.ocean && !pTile.Type.ocean && tileDown.Type != pTile.Type && tileDown.Type.height_min < pTile.Type.height_min)
      {
        this.pixels[pTile.data.tile_id] = Toolbox.edge_alpha;
        return;
      }
    }
    if (tileUp == null)
      return;
    if (tileUp.Type.wall)
      this.pixels[pTile.data.tile_id] = tileUp.Type.edge_color;
    else if (pTile.Type.ocean && tileUp.Type.ocean && tileUp.Type.height_min > pTile.Type.height_min)
    {
      this.pixels[pTile.data.tile_id] = Toolbox.edge_alpha;
    }
    else
    {
      if (!tileUp.Type.ground || tileUp.Type.can_be_filled_with_ocean)
        return;
      if (pTile.Type.layer_type == TileLayerType.Ocean)
      {
        this.pixels[pTile.data.tile_id] = TileTypeBase.edge_color_ocean;
      }
      else
      {
        if (!pTile.Type.can_be_filled_with_ocean || pTile.Type.explodable)
          return;
        this.pixels[pTile.data.tile_id] = TileTypeBase.edge_color_no_ocean;
      }
    }
  }
}
