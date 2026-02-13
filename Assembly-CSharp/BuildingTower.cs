// Decompiled with JetBrains decompiler
// Type: BuildingTower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingTower : BaseBuildingComponent
{
  protected float _check_targets_timeout = 1f;
  private bool _test_shooting;
  protected int _shooting_amount;
  private bool _shooting_active;
  protected BaseSimObject _shooting_target;

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (this.building.isUnderConstruction())
      return;
    this.updateTestShooting();
    this.updateTower(pElapsed);
  }

  protected void updateTestShooting()
  {
    if (!this._test_shooting || !Input.GetMouseButtonDown(2))
      return;
    Vector3 pLaunchPosition;
    // ISSUE: explicit constructor call
    ((Vector3) ref pLaunchPosition).\u002Ector(this.building.current_tile.posV3.x, this.building.current_tile.posV3.y);
    pLaunchPosition.y += this.building.asset.tower_projectile_offset;
    World.world.projectiles.spawn((BaseSimObject) this.building, (BaseSimObject) null, this.building.asset.tower_projectile, pLaunchPosition, World.world.getMouseTilePos().posV3);
  }

  protected virtual void updateTower(float pElapsed)
  {
    if (this._shooting_active)
      this.shootAtTarget();
    else
      this.updateCheckTargets(pElapsed);
  }

  protected virtual void updateCheckTargets(float pElapsed)
  {
    if ((double) this._check_targets_timeout > 0.0)
      this._check_targets_timeout -= pElapsed;
    else
      this.checkTargets();
  }

  protected virtual void resetTimeout()
  {
    this._check_targets_timeout = this.building.asset.tower_projectile_reload + Randy.randomFloat(0.0f, this.building.asset.tower_projectile_reload);
  }

  protected virtual void shootAtTarget()
  {
    if (this._shooting_target == null || !this._shooting_target.isAlive())
    {
      this._shooting_active = false;
    }
    else
    {
      --this._shooting_amount;
      if (this._shooting_amount <= 0)
        this._shooting_active = false;
      Vector3 pLaunchPosition;
      // ISSUE: explicit constructor call
      ((Vector3) ref pLaunchPosition).\u002Ector(this.building.current_tile.posV3.x, this.building.current_tile.posV3.y);
      pLaunchPosition.y += this.building.asset.tower_projectile_offset;
      Vector3 posV3 = this._shooting_target.current_tile.posV3;
      posV3.x += Randy.randomFloat((float) -((double) this._shooting_target.stats["size"] + 1.0), this._shooting_target.stats["size"] + 1f);
      posV3.y += Randy.randomFloat(-this._shooting_target.stats["size"], this._shooting_target.stats["size"]);
      float pTargetZ = 0.0f;
      if (this._shooting_target.isInAir())
        pTargetZ = this._shooting_target.getHeight();
      this.projectileStarted();
      World.world.projectiles.spawn((BaseSimObject) this.building, this._shooting_target, this.building.asset.tower_projectile, pLaunchPosition, posV3, pTargetZ);
    }
  }

  protected virtual void projectileStarted()
  {
  }

  protected virtual void checkTargets()
  {
    this.resetTimeout();
    this._shooting_target = (BaseSimObject) null;
    this._shooting_active = false;
    this._shooting_amount = 0;
    BaseSimObject target = this.findTarget();
    if (target == null)
      return;
    this._shooting_active = true;
    this._shooting_target = target;
    this._shooting_amount = this.building.asset.tower_projectile_amount;
  }

  protected virtual BaseSimObject findTarget()
  {
    return this.building.findEnemyObjectTarget(this.building.asset.tower_attack_buildings);
  }

  public override void Dispose()
  {
    this._shooting_target = (BaseSimObject) null;
    base.Dispose();
  }
}
