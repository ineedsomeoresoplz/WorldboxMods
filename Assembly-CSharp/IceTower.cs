// Decompiled with JetBrains decompiler
// Type: IceTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class IceTower : BaseBuildingComponent
{
  private float _action_interval = 0.3f;
  private float _action_timer;

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if ((double) this._action_timer > 0.0)
    {
      this._action_timer -= pElapsed;
    }
    else
    {
      this._action_timer = this._action_interval;
      this.freezeRandomTile();
    }
  }

  private void freezeRandomTile()
  {
    WorldTile currentTile = this.building.current_tile;
    TileIsland island = currentTile.region?.island;
    if (island == null)
      return;
    WorldTile random = island.regions.GetRandom().tiles.GetRandom<WorldTile>();
    this.freeze(currentTile, random);
  }

  private void freeze(WorldTile pCenter, WorldTile pTile)
  {
    if ((double) Toolbox.DistTile(pCenter, pTile) > 50.0)
      return;
    pTile.freeze();
  }
}
