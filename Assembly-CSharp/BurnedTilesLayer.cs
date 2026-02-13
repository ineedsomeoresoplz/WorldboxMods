// Decompiled with JetBrains decompiler
// Type: BurnedTilesLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BurnedTilesLayer : MapLayer
{
  public Color color;
  private WorldBehaviour worldBehaviour;

  internal override void create()
  {
    this.colorValues = new Color(this.color.r, this.color.g, this.color.b, 0.5f);
    this.colors_amount = 15;
    this.autoDisable = false;
    base.create();
    ((Behaviour) this).enabled = true;
  }

  public void setTileDirty(WorldTile pTile)
  {
    if (this.pixels_to_update.Contains(pTile))
      return;
    this.pixels_to_update.Add(pTile);
  }

  protected override void UpdateDirty(float pElapsed)
  {
    if (this.pixels_to_update.Count <= 0)
      return;
    foreach (WorldTile worldTile in (HashSet<WorldTile>) this.pixels_to_update)
    {
      if (worldTile.burned_stages > 0)
        this.pixels[worldTile.data.tile_id] = this.colors[worldTile.burned_stages - 1];
      else
        this.pixels[worldTile.data.tile_id] = Toolbox.clear;
    }
    this.pixels_to_update.Clear();
    this.updatePixels();
  }
}
