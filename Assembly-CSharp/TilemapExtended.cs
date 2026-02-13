// Decompiled with JetBrains decompiler
// Type: TilemapExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#nullable disable
public class TilemapExtended : MonoBehaviour
{
  public int z;
  private Tilemap _tilemap;
  private readonly List<Vector3Int> _vec = new List<Vector3Int>();
  private readonly ListPool<TileBase> _tiles = new ListPool<TileBase>();

  public void create(TileTypeBase pTileBase)
  {
    this.z = pTileBase.render_z;
    ((Object) ((Component) this).gameObject).name = pTileBase.draw_layer_name;
    TilemapRenderer component = ((Component) this).GetComponent<TilemapRenderer>();
    ((Renderer) component).sortingOrder = pTileBase.render_z;
    ((Renderer) component).sharedMaterial = LibraryMaterials.instance.dict[pTileBase.material];
    if (pTileBase.id == "deep_ocean")
      ((Component) this).gameObject.SetActive(false);
    this._tilemap = ((Component) this).GetComponent<Tilemap>();
  }

  internal void prepareDraw()
  {
    this._vec.Clear();
    this._tiles.Clear();
  }

  internal void addToQueueToRedraw(
    WorldTile pWorldTile,
    Vector3Int pPosition,
    TileBase pTileGraphics,
    bool pSkipCheck = false)
  {
    ((Vector3Int) ref pPosition).z = 0;
    if (!pSkipCheck)
    {
      if (Object.op_Equality((Object) pWorldTile.current_rendered_tile_graphics, (Object) pTileGraphics) && pTileGraphics != null)
        return;
      pWorldTile.current_rendered_tile_graphics = pTileGraphics;
    }
    this._vec.Add(pPosition);
    this._tiles.Add(pTileGraphics);
  }

  internal void clear() => this._tilemap.ClearAllTiles();

  internal void redraw()
  {
    if (this._vec.Count == 0)
      return;
    this._tilemap.SetTiles(this._vec.ToArray(), this._tiles.GetRawBuffer());
  }
}
