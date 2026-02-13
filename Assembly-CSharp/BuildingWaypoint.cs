// Decompiled with JetBrains decompiler
// Type: BuildingWaypoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public abstract class BuildingWaypoint : BaseBuildingComponent
{
  private const int UNITS_AFFECTED_PER_ACTION = 5;
  private const float ACTION_INTERVAL = 20f;
  private float _action_timer = 20f;

  protected abstract string effect_id { get; }

  protected abstract string trait_id { get; }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if ((double) this._action_timer > 0.0)
    {
      this._action_timer -= pElapsed;
    }
    else
    {
      this._action_timer = 20f;
      this.doAction(this.building.current_tile);
    }
  }

  internal void doAction(WorldTile pFromTile)
  {
    this.spawnMainEffect();
    World.world.applyForceOnTile(this.building.current_tile, pForceAmount: 3f);
    int num = 0;
    foreach (Actor actor in Finder.getUnitsFromChunk(pFromTile, 1, pRandom: true))
    {
      if (!actor.hasTrait(this.trait_id))
      {
        if (actor.addTrait(this.trait_id))
          ++num;
        if (num >= 5)
          break;
      }
    }
  }

  public void spawnMainEffect()
  {
    EffectsLibrary.spawnAt(this.effect_id, this.building.current_tile.posV3, this.building.current_scale.y);
  }
}
