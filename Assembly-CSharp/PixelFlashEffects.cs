// Decompiled with JetBrains decompiler
// Type: PixelFlashEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PixelFlashEffects : MapLayer
{
  private List<WorldTile> toRemove = new List<WorldTile>();
  private ColorArray colorWhite;
  private ColorArray colorPurple;
  private ColorArray colorBlue;
  private float _timer;

  internal override void create()
  {
    this.colors_amount = 30;
    this.colorValues = new Color(1f, 1f, 1f);
    this.colorWhite = new ColorArray(1f, 1f, 1f, 1f, (float) this.colors_amount, 0.5f);
    this.colorPurple = new ColorArray(ConwayLife.colorEater, this.colors_amount);
    this.colorBlue = new ColorArray(Color32.op_Implicit(Toolbox.makeColor("#3BCC55")), this.colors_amount);
    base.create();
    ((Behaviour) this).enabled = true;
  }

  public void flashPixel(WorldTile pTile, int pVal = -1, ColorType pColorType = ColorType.White)
  {
    if (SmoothLoader.isLoading())
      return;
    if (pVal == -1 || pVal >= this.colors_amount)
      pVal = this.colors_amount - 1;
    if (!((Behaviour) this).enabled)
      return;
    switch (pColorType)
    {
      case ColorType.White:
        pTile.color_array = this.colorWhite;
        break;
      case ColorType.Purple:
        pTile.color_array = this.colorPurple;
        break;
      case ColorType.Blue:
        pTile.color_array = this.colorBlue;
        break;
    }
    if (pTile.flash_state <= 0)
      this.pixels_to_update.Add(pTile);
    if (pTile.flash_state >= pVal)
      return;
    pTile.flash_state = pVal;
  }

  internal override void clear()
  {
    base.clear();
    this.pixels_to_update.Clear();
  }

  protected override void UpdateDirty(float pElapsed)
  {
    if ((double) this._timer > 0.0)
    {
      this._timer -= World.world.delta_time;
    }
    else
    {
      this._timer = 0.01f;
      if (this.pixels_to_update.Count <= 0)
        return;
      this.toRemove.Clear();
      foreach (WorldTile worldTile in (HashSet<WorldTile>) this.pixels_to_update)
      {
        if (worldTile.flash_state < 0)
        {
          this.toRemove.Add(worldTile);
        }
        else
        {
          this.pixels[worldTile.data.tile_id] = worldTile.color_array.colors[worldTile.flash_state];
          --worldTile.flash_state;
        }
      }
      for (int index = 0; index < this.toRemove.Count; ++index)
        this.pixels_to_update.Remove(this.toRemove[index]);
      this.updatePixels();
    }
  }
}
