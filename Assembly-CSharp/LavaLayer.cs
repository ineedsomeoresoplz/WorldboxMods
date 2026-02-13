// Decompiled with JetBrains decompiler
// Type: LavaLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LavaLayer : MapLayer
{
  private List<WorldTile> _to_clear = new List<WorldTile>();

  internal override void create() => base.create();

  protected override void checkAutoDisable()
  {
    bool flag = WorldBehaviourActionLava.hasLava();
    if (!MapBox.isRenderMiniMap())
      flag = false;
    if (flag)
    {
      if (((Renderer) this.sprRnd).enabled)
        return;
      ((Renderer) this.sprRnd).enabled = true;
    }
    else
    {
      if (!((Renderer) this.sprRnd).enabled)
        return;
      ((Renderer) this.sprRnd).enabled = false;
    }
  }

  public override void update(float pElapsed)
  {
    this.checkAutoDisable();
    this.updateLava();
  }

  public override void draw(float pElapsed)
  {
  }

  private void updateLava()
  {
    bool flag = false;
    if (this._to_clear.Count > 0)
    {
      foreach (WorldTile worldTile in this._to_clear)
        this.pixels[worldTile.data.tile_id] = Toolbox.clear;
      this._to_clear.Clear();
      flag = true;
    }
    if (WorldBehaviourActionLava.hasLava())
    {
      flag = true;
      this.drawLavaPixel(TileLibrary.lava0);
      this.drawLavaPixel(TileLibrary.lava1);
      this.drawLavaPixel(TileLibrary.lava2);
      this.drawLavaPixel(TileLibrary.lava3);
    }
    if (!flag)
      return;
    this.updatePixels();
  }

  internal override void clear()
  {
    base.clear();
    this._to_clear.Clear();
  }

  private void drawLavaPixel(TileType pType)
  {
    if (pType.hashset.Count == 0)
      return;
    foreach (WorldTile worldTile in (HashSet<WorldTile>) pType.hashset)
    {
      this.pixels[worldTile.data.tile_id] = pType.color;
      this._to_clear.Add(worldTile);
    }
  }
}
