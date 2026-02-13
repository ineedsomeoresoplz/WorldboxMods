// Decompiled with JetBrains decompiler
// Type: PathFindingVisualiser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using EpPathFinding.cs;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PathFindingVisualiser : MapLayer
{
  public Color default_color;
  private List<WorldTile> tiles = new List<WorldTile>();

  internal override void create()
  {
    this.colorValues = new Color(1f, 0.46f, 0.19f, 1f);
    this.colorValues = this.default_color;
    base.create();
  }

  protected override void UpdateDirty(float pElapsed)
  {
    if (DebugConfig.isOn(DebugOption.LastPath))
    {
      if (((Component) this).gameObject.activeSelf)
        return;
      ((Component) this).gameObject.SetActive(true);
    }
    else
    {
      if (!((Component) this).gameObject.activeSelf)
        return;
      ((Component) this).gameObject.SetActive(false);
    }
  }

  internal override void clear()
  {
    if (this.tiles.Count == 0)
      return;
    this.tiles.Clear();
    for (int index = 0; index < this.pixels.Length; ++index)
      this.pixels[index] = Color32.op_Implicit(Color.clear);
    this.createTextureNew();
  }

  internal void showPath(StaticGrid pGrid, List<WorldTile> pTilePath)
  {
    if (!DebugConfig.isOn(DebugOption.LastPath))
      return;
    this.clear();
    if (pGrid != null)
    {
      foreach (WorldTile tiles in World.world.tiles_list)
      {
        this.tiles.Add(tiles);
        StaticGrid staticGrid = pGrid;
        Vector2Int pos = tiles.pos;
        int x = ((Vector2Int) ref pos).x;
        pos = tiles.pos;
        int y = ((Vector2Int) ref pos).y;
        Node nodeAt = staticGrid.GetNodeAt(x, y);
        if (nodeAt.isClosed)
          this.pixels[tiles.data.tile_id] = Color32.op_Implicit(Color.red);
        else if (nodeAt.isOpened)
          this.pixels[tiles.data.tile_id] = Color32.op_Implicit(Color.green);
        else
          this.pixels[tiles.data.tile_id] = Color32.op_Implicit(Color.clear);
      }
    }
    foreach (WorldTile worldTile in pTilePath)
    {
      this.pixels[worldTile.data.tile_id] = Color32.op_Implicit(Color.blue);
      this.tiles.Add(worldTile);
    }
    this.updatePixels();
  }
}
