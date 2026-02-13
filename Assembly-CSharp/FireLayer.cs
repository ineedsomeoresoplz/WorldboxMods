// Decompiled with JetBrains decompiler
// Type: FireLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FireLayer : MapLayer
{
  internal override void create() => base.create();

  public void setTileDirty(WorldTile pTile)
  {
    if (this.pixels_to_update.Contains(pTile))
      return;
    this.pixels_to_update.Add(pTile);
  }

  protected override void checkAutoDisable()
  {
    bool flag = WorldBehaviourActionFire.hasFires();
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

  protected override void UpdateDirty(float pElapsed)
  {
    if (this.pixels_to_update.Count <= 0)
      return;
    Color color = Color32.op_Implicit(Toolbox.color_fire);
    foreach (WorldTile worldTile in (HashSet<WorldTile>) this.pixels_to_update)
    {
      if (worldTile.isOnFire())
      {
        float timeElapsedSince = World.world.getWorldTimeElapsedSince(worldTile.data.fire_timestamp);
        color.a = (float) (0.5 + (1.0 - (double) timeElapsedSince / (double) SimGlobals.m.fire_stop_time));
        this.pixels[worldTile.data.tile_id] = Color32.op_Implicit(color);
      }
      else
        this.pixels[worldTile.data.tile_id] = Toolbox.clear;
    }
    this.pixels_to_update.Clear();
    this.updatePixels();
  }
}
