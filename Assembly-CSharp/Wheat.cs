// Decompiled with JetBrains decompiler
// Type: Wheat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class Wheat : BaseBuildingComponent
{
  private int _current_level;
  private int _max_level;

  internal override void create(Building pBuilding)
  {
    base.create(pBuilding);
    this._current_level = 0;
    this._max_level = this.building.asset.building_sprites.animation_data.Count - 1;
    this.checkSprite();
  }

  public override void update(float pElapsed)
  {
    this.building.data.grow_time += pElapsed;
    if (this.isMaxLevel() || (double) this.building.data.grow_time < (double) (this.building.asset.growth_time * (float) (this._current_level + 1)))
      return;
    this.grow();
  }

  internal void grow()
  {
    if (this._current_level < this._max_level)
    {
      ++this._current_level;
      this.checkSprite();
    }
    MusicBox.playSound("event:/SFX/DROPS/DropSeedGrass", this.building.current_tile, true, true);
    if (World.world_era.flag_crops_grow || !Randy.randomBool())
      return;
    this.building.startDestroyBuilding();
  }

  public void growFull()
  {
    this._current_level = this._max_level;
    this.checkSprite();
    MusicBox.playSound("event:/SFX/DROPS/DropSeedGrass", this.building.current_tile, true, true);
  }

  private void checkSprite()
  {
    this.building.setAnimData(this._current_level);
    if (this.building.asset.random_flip && !this.building.asset.shadow)
      this.building.flip_x = Randy.randomBool();
    this.building.setScaleTween();
    World.world.setTileDirty(this.building.current_tile);
  }

  public bool isMaxLevel() => this._current_level == this._max_level;

  public int getCurrentLevel() => this._current_level;
}
