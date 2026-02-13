// Decompiled with JetBrains decompiler
// Type: GreyGooLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class GreyGooLayer : BaseModule
{
  private const float SPREAD_CHANCE = 0.05f;
  private const float REMOVE_CHANCE = 0.09f;
  private const float INTERVAL = 0.08f;
  private List<WorldTile> _to_remove = new List<WorldTile>();
  private List<WorldTile> _to_add = new List<WorldTile>();
  private bool _initiated;

  internal override void create()
  {
    base.create();
    this.hashset = new HashSet<WorldTile>();
  }

  internal override void clear()
  {
    base.clear();
    this.hashset.Clear();
    this._to_remove.Clear();
    this._to_add.Clear();
    this._initiated = false;
  }

  public bool isActive() => this.hashset.Count > 0;

  private void init()
  {
    this._initiated = true;
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.Type.grey_goo)
        this.add(tiles);
    }
  }

  public override void update(float pElapsed)
  {
    if (!this._initiated)
      this.init();
    base.update(pElapsed);
    if (!this.isActive() || World.world.isPaused())
      return;
    if ((double) this.timer > 0.0)
    {
      this.timer -= pElapsed;
    }
    else
    {
      this.timer = 0.08f;
      this._to_remove.Clear();
      this._to_add.Clear();
      this.updateGooTiles();
      this.removeFromHashset();
      this.addToHashset();
    }
  }

  private void updateGooTiles()
  {
    if (this.hashset.Count == 0)
      return;
    foreach (WorldTile pTile in this.hashset)
    {
      if (!pTile.Type.grey_goo)
      {
        this._to_remove.Add(pTile);
      }
      else
      {
        if (pTile.hasBuilding())
          pTile.building.startDestroyBuilding();
        if (Randy.randomChance(0.05f))
        {
          this.terraform(pTile);
          this.checkAroundTiles(pTile);
          this._to_remove.Add(pTile);
        }
        else if (Randy.randomChance(0.05f))
          this.checkAroundTiles(pTile);
        else if (Randy.randomChance(0.09f) && this.areAroundTilesEmpty(pTile))
        {
          this._to_remove.Add(pTile);
          if (pTile.Type.grey_goo)
            this.terraform(pTile);
        }
      }
    }
  }

  private void removeFromHashset()
  {
    if (this._to_remove.Count == 0)
      return;
    for (int index = 0; index < this._to_remove.Count; ++index)
      this.remove(this._to_remove[index]);
  }

  private void addToHashset()
  {
    if (this._to_add.Count == 0)
      return;
    for (int index = 0; index < this._to_add.Count; ++index)
      this.add(this._to_add[index]);
  }

  private void checkAroundTiles(WorldTile pTile)
  {
    if (WorldLawLibrary.world_law_gaias_covenant.isEnabled())
      return;
    foreach (WorldTile neighbour in pTile.neighbours)
    {
      TileTypeBase type = neighbour.Type;
      if (!type.grey_goo && !type.IsType("pit_deep_ocean") && (!type.IsType("deep_ocean") || neighbour.hasBuilding()))
        this._to_add.Add(neighbour);
    }
  }

  private bool areAroundTilesEmpty(WorldTile pTile)
  {
    foreach (WorldTile neighbour in pTile.neighbours)
    {
      TileTypeBase type = neighbour.Type;
      if (neighbour.hasBuilding() || !type.grey_goo && !type.considered_empty_tile)
        return false;
    }
    return true;
  }

  private void makeGoo(WorldTile pTile)
  {
    pTile.unfreeze(99);
    MapAction.terraformMain(pTile, TileLibrary.grey_goo);
  }

  private void terraform(WorldTile pTile)
  {
    MapAction.terraformMain(pTile, TileLibrary.pit_deep_ocean, TerraformLibrary.grey_goo);
    MusicBox.playSound("event:/SFX/DESTRUCTION/GreyGooEat", pTile, pVisibleOnly: true);
  }

  public void remove(WorldTile pTile) => this.hashset.Remove(pTile);

  public void add(WorldTile pTile)
  {
    if (pTile.Type.considered_empty_tile || !this.hashset.Add(pTile))
      return;
    this.makeGoo(pTile);
  }
}
