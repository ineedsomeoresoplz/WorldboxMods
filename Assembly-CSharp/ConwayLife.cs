// Decompiled with JetBrains decompiler
// Type: ConwayLife
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ConwayLife : MapLayer
{
  public static Color32 colorEater = Color32.op_Implicit(new Color(1f, 0.2f, 1f));
  public static Color32 colorCreator;
  public bool makeFlash = true;
  private HashSetWorldTile newList;
  private float nextTickTimer;
  private float nextTickInterval = 0.05f;
  private int decreaseTick;
  private List<WorldTile> toRemove = new List<WorldTile>();

  internal override void create()
  {
    base.create();
    ConwayLife.colorCreator = Color32.op_Implicit(Toolbox.makeColor("#3BCC55"));
    this.hashsetTiles = new HashSetWorldTile();
    this.newList = new HashSetWorldTile();
  }

  protected override void UpdateDirty(float pElapsed)
  {
    this.UpdateVisual();
    if (World.world.isPaused())
      return;
    if ((double) this.nextTickTimer > 0.0)
    {
      this.nextTickTimer -= pElapsed;
    }
    else
    {
      this.nextTickTimer = this.nextTickInterval;
      for (int index = 0; index < Config.time_scale_asset.conway_ticks; ++index)
        this.updateTick();
    }
  }

  private void UpdateVisual()
  {
    if (this.pixels_to_update.Count == 0)
      return;
    foreach (WorldTile worldTile in (HashSet<WorldTile>) this.pixels_to_update)
    {
      if (this.hashsetTiles.Contains(worldTile))
      {
        if (worldTile.data.conwayType == ConwayType.Eater)
          this.pixels[worldTile.data.tile_id] = ConwayLife.colorEater;
        else if (worldTile.data.conwayType == ConwayType.Creator)
          this.pixels[worldTile.data.tile_id] = ConwayLife.colorCreator;
        else
          this.pixels[worldTile.data.tile_id] = Toolbox.clear;
      }
      else
      {
        worldTile.data.conwayType = ConwayType.None;
        this.pixels[worldTile.data.tile_id] = Toolbox.clear;
      }
    }
    this.pixels_to_update.Clear();
    this.updatePixels();
  }

  public void remove(WorldTile pTile)
  {
    if (this.hashsetTiles.Count == 0)
      return;
    this.hashsetTiles.Remove(pTile);
    this.pixels_to_update.Add(pTile);
    pTile.data.conwayType = ConwayType.None;
  }

  public void add(WorldTile pTile, string pType)
  {
    pTile.data.conwayType = !(pType == "conway") ? ConwayType.Creator : ConwayType.Eater;
    this.hashsetTiles.Add(pTile);
    this.pixels_to_update.Add(pTile);
  }

  private void updateTick()
  {
    if (this.decreaseTick-- <= 0)
      this.decreaseTick = 5;
    if (this.hashsetTiles.Count <= 0 && this.newList.Count <= 0)
      return;
    this.newList.Clear();
    foreach (WorldTile hashsetTile in (HashSet<WorldTile>) this.hashsetTiles)
    {
      this.checkCell(hashsetTile);
      foreach (WorldTile pCell in hashsetTile.neighboursAll)
        this.checkCell(pCell);
    }
    HashSetWorldTile hashsetTiles = this.hashsetTiles;
    this.hashsetTiles = this.newList;
    this.newList = hashsetTiles;
    this.UpdateVisual();
  }

  private void makeAlive(WorldTile pCell)
  {
    if (this.decreaseTick == 5)
    {
      MusicBox.playSound("event:/SFX/UNIQUE/ConwayMove", pCell);
      if (pCell.data.conwayType == ConwayType.Eater)
        MapAction.decreaseTile(pCell, true, "destroy_no_flash");
      else
        MapAction.increaseTile(pCell, true, "destroy_no_flash");
    }
    this.newList.Add(pCell);
    if (!this.makeFlash)
      return;
    this.makeFlashh(pCell, 25);
  }

  internal void makeFlashh(WorldTile pCell, int pAmount)
  {
    if (pCell.data.conwayType == ConwayType.None)
      return;
    int conwayType = (int) pCell.data.conwayType;
  }

  internal override void clear()
  {
    base.clear();
    this.newList.Clear();
    this.hashsetTiles.Clear();
  }

  private void checkCell(WorldTile pCell)
  {
    if (this.pixels_to_update.Contains(pCell))
      return;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    this.pixels_to_update.Add(pCell);
    if (pCell.data.conwayType == ConwayType.Eater)
      ++num2;
    if (pCell.data.conwayType == ConwayType.Creator)
      ++num3;
    if (this.hashsetTiles.Contains(pCell))
    {
      foreach (WorldTile worldTile in pCell.neighboursAll)
      {
        if (this.hashsetTiles.Contains(worldTile))
        {
          ++num1;
          if (worldTile.data.conwayType == ConwayType.Creator)
            ++num3;
          else if (worldTile.data.conwayType == ConwayType.Eater)
            ++num2;
        }
        if (num1 >= 4)
        {
          if (this.makeFlash)
            this.makeFlashh(pCell, 15);
          pCell.data.conwayType = ConwayType.None;
          return;
        }
      }
      if (num1 == 2 || num1 == 3)
      {
        if (pCell.data.conwayType == ConwayType.None && (num2 != 0 || num3 != 0))
          pCell.data.conwayType = num2 < num3 ? ConwayType.Creator : ConwayType.Eater;
        this.makeAlive(pCell);
      }
      else
        pCell.data.conwayType = ConwayType.None;
    }
    else
    {
      foreach (WorldTile worldTile in pCell.neighboursAll)
      {
        if (this.hashsetTiles.Contains(worldTile))
          ++num1;
        if (worldTile.data.conwayType == ConwayType.Eater)
          ++num2;
        if (worldTile.data.conwayType == ConwayType.Creator)
          ++num3;
      }
      if (num1 != 3)
        return;
      if (pCell.data.conwayType == ConwayType.None && (num2 != 0 || num3 != 0))
        pCell.data.conwayType = num2 < num3 ? ConwayType.Creator : ConwayType.Eater;
      this.makeAlive(pCell);
    }
  }

  internal void checkKillRange(Vector2Int pPos, int pRad)
  {
    if (this.hashsetTiles.Count == 0)
      return;
    this.toRemove.Clear();
    foreach (WorldTile hashsetTile in (HashSet<WorldTile>) this.hashsetTiles)
    {
      if ((double) Toolbox.DistVec2(hashsetTile.pos, pPos) <= (double) pRad)
      {
        hashsetTile.data.conwayType = ConwayType.None;
        this.toRemove.Add(hashsetTile);
      }
    }
    foreach (WorldTile pTile in this.toRemove)
      this.remove(pTile);
  }
}
