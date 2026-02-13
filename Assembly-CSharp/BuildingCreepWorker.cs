// Decompiled with JetBrains decompiler
// Type: BuildingCreepWorker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BuildingCreepWorker : IDisposable
{
  private int steps_max;
  private WorldTile cur_tile;
  private ActorDirection cur_direction;
  private BuildingCreepHUB _parent;
  private int _total_step_counter;
  private BiomeAsset _this_creep_biome;
  private int _direction_step_amount;

  public BuildingCreepWorker(BuildingCreepHUB pParent)
  {
    this._parent = pParent;
    this._this_creep_biome = AssetManager.biome_library.get(this._parent.building.asset.grow_creep_type);
    this.steps_max = this._parent.building.asset.grow_creep_steps_max;
  }

  public void update()
  {
    if (this.cur_tile == null)
    {
      this._total_step_counter = 0;
      this.cur_tile = this._parent.building.current_tile;
      this.cur_direction = Randy.getRandom<ActorDirection>(Toolbox.directions);
    }
    this.checkRandomDirectionChange();
    this.updateMovement(this.cur_tile);
    if (this._total_step_counter <= this.steps_max)
      return;
    this.cur_tile = (WorldTile) null;
  }

  private void checkRandomDirectionChange()
  {
    if (!this._parent.building.asset.grow_creep_random_new_direction)
      return;
    if (this._direction_step_amount >= this._parent.building.asset.grow_creep_steps_before_new_direction)
    {
      this.cur_direction = Randy.getRandom<ActorDirection>(Toolbox.directions);
      this._direction_step_amount = 0;
    }
    ++this._direction_step_amount;
  }

  private void creepFlash(int pVal = 15)
  {
    if (this._parent.building.asset.grow_creep_flash)
      World.world.flash_effects.flashPixel(this.cur_tile, pVal);
    if (!this._parent.building.asset.grow_creep_redraw_tile)
      return;
    World.world.redrawRenderedTile(this.cur_tile);
  }

  private void updateMovement(WorldTile pNextTile)
  {
    this.cur_tile = pNextTile;
    if (this.canPlaceWorkerOn(this.cur_tile))
    {
      this.makeCreep(this.cur_tile);
      this.creepFlash();
      ++this._total_step_counter;
    }
    else if (this.cur_tile.Type.biome_asset == this._this_creep_biome)
    {
      this.creepFlash();
      pNextTile = this.getNextRandomTile(this.cur_tile);
      if (pNextTile == null)
        this.cur_tile = (WorldTile) null;
      else if (this.canPlaceWorkerOn(pNextTile))
        this.cur_tile = pNextTile;
      else if (pNextTile.Type.biome_asset == this._this_creep_biome)
        this.cur_tile = pNextTile;
      else if (pNextTile.Type.biome_asset != this._this_creep_biome)
      {
        this.creepFlash(30);
        this.cur_tile = pNextTile;
      }
      else
      {
        if (pNextTile.getCreepTileRank() != TileRank.Nothing)
          return;
        pNextTile = this.cur_tile;
        this.cur_direction = Randy.getRandom<ActorDirection>(Toolbox.directions);
      }
    }
    else
      this.cur_tile = (WorldTile) null;
  }

  private bool canPlaceWorkerOn(WorldTile pTile)
  {
    return pTile.getCreepTileRank() != TileRank.Nothing && (!pTile.Type.creep || pTile.Type.biome_asset != this._this_creep_biome);
  }

  private void makeCreep(WorldTile pTile)
  {
    TopTileType tile = AssetManager.biome_library.get(this._parent.building.asset.grow_creep_type).getTile(pTile);
    if (tile == null)
      return;
    MapAction.terraformTop(pTile, tile, TerraformLibrary.flash);
  }

  private WorldTile getNextRandomTile(WorldTile pTile)
  {
    switch (this._parent.building.asset.grow_creep_movement_type)
    {
      case CreepWorkerMovementType.RandomNeighbourAll:
        return pTile.neighboursAll.GetRandom<WorldTile>();
      case CreepWorkerMovementType.RandomNeighbour:
        return pTile.neighbours.GetRandom<WorldTile>();
      case CreepWorkerMovementType.Direction:
        return this.getDirectionTile(pTile, this._parent.building.asset.grow_creep_direction_random_position);
      default:
        return pTile.neighboursAll.GetRandom<WorldTile>();
    }
  }

  private WorldTile getDirectionTile(WorldTile pTile, bool pAddRandom = true)
  {
    Vector2Int pos1 = pTile.pos;
    int x = ((Vector2Int) ref pos1).x;
    Vector2Int pos2 = pTile.pos;
    int y = ((Vector2Int) ref pos2).y;
    switch (this.cur_direction)
    {
      case ActorDirection.Up:
        if (pAddRandom)
          x += Randy.randomInt(-1, 2);
        ++y;
        break;
      case ActorDirection.Right:
        ++x;
        if (pAddRandom)
        {
          y += Randy.randomInt(-1, 2);
          break;
        }
        break;
      case ActorDirection.Down:
        if (pAddRandom)
          x += Randy.randomInt(-1, 2);
        --y;
        break;
      case ActorDirection.Left:
        --x;
        if (pAddRandom)
        {
          y += Randy.randomInt(-1, 2);
          break;
        }
        break;
    }
    return World.world.GetTile(x, y);
  }

  public void Dispose()
  {
    this._parent = (BuildingCreepHUB) null;
    this.cur_tile = (WorldTile) null;
    this._this_creep_biome = (BiomeAsset) null;
  }
}
