// Decompiled with JetBrains decompiler
// Type: BuildingMonolith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingMonolith : BaseBuildingComponent
{
  private const float ACTION_INTERVAL = 10f;
  private float _action_timer = 10f;

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (Date.isMonolithMonth() && this.building.is_visible && this.building.isNormal() && Time.frameCount % 30 == 0)
      EffectsLibrary.spawnAt("fx_monolith_glow_2", this.building.current_tile.posV3, this.building.current_scale.y);
    if ((double) this._action_timer > 0.0)
    {
      this._action_timer -= pElapsed;
    }
    else
    {
      if (!Date.isMonolithMonth())
        return;
      this._action_timer = 10f;
      this.doMonolithAction(this.building.current_tile);
    }
  }

  internal void doMonolithAction(WorldTile pFromTile, bool pForce = false)
  {
    if (!WorldLawLibrary.world_law_evolution_events.isEnabled())
      return;
    this.spawnMainEffect();
    World.world.applyForceOnTile(this.building.current_tile, pForceAmount: 3f);
    int num1 = 3;
    int num2 = 0;
    foreach (Actor pActor in Finder.getUnitsFromChunk(pFromTile, 1, pRandom: true))
    {
      if (!pActor.hasStatus("confused") && pActor.hasSubspecies() && (Date.isMonolithMonth() || pForce))
      {
        if (ActionLibrary.tryToEvolveUnitViaMonolith(pActor))
          ++num2;
        if (num2 >= num1)
          break;
      }
    }
  }

  public void spawnMainEffect()
  {
    EffectsLibrary.spawnAt("fx_monolith_launch_bottom", this.building.current_tile.posV3, this.building.current_scale.y);
    EffectsLibrary.spawnAt("fx_monolith_launch", this.building.current_tile.posV3, this.building.current_scale.y);
  }
}
