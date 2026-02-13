// Decompiled with JetBrains decompiler
// Type: Projectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Projectile : CoreSystemObject<ProjectileData>
{
  internal readonly BaseStats stats = new BaseStats();
  private BaseSimObject by_who;
  private BaseSimObject _main_target;
  private long _main_target_id;
  internal Kingdom kingdom;
  private Action _kill_action;
  public ProjectileAsset asset;
  private Vector2 _vector_start;
  private Vector2 _vector_target;
  private float _current_scale;
  private float _target_scale;
  private bool _is_target_reached;
  private Vector3 _current_position_3d;
  private float _angle_horizontal;
  private float _angle_vertical;
  private float _timer_smoke;
  private float _dead_alpha;
  private ProjectileState _state;
  private float _collision_timeout;
  private Vector3 _velocity;
  public Quaternion rotation;
  private float _speed;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.projectiles;

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this._current_scale = 0.0f;
    this._target_scale = 0.0f;
    this._is_target_reached = false;
    this._angle_horizontal = 0.0f;
    this._angle_vertical = 0.0f;
    this._timer_smoke = 0.0f;
    this._dead_alpha = 1f;
    this._state = ProjectileState.Active;
    this._collision_timeout = 0.0f;
    this._speed = 0.0f;
  }

  public Vector2 getStartVector() => this._vector_start;

  public Vector2 getTargetVector() => this._vector_target;

  public void start(
    BaseSimObject pInitiator,
    BaseSimObject pTargetObject,
    Vector3 pLaunchPosition,
    Vector3 pTargetPosition,
    string pAssetID,
    float pTargetPosZ = 0.0f,
    float pStartZ = 0.25f,
    float pForce = 0.0f,
    Action pKillAction = null,
    Kingdom pForcedKingdom = null)
  {
    this._kill_action = pKillAction;
    this.by_who = pInitiator;
    this._main_target = pTargetObject;
    if (this._main_target != null)
      this._main_target_id = this._main_target.id;
    if (this.by_who != null)
    {
      this.setStats(pInitiator.stats);
      this.kingdom = this.by_who.kingdom;
    }
    if (pForcedKingdom != null)
      this.kingdom = pForcedKingdom;
    this.asset = AssetManager.projectiles.get(pAssetID);
    this._speed = this.asset.speed + Randy.randomFloat(0.0f, this.asset.speed_random);
    if ((double) pForce != 0.0)
      this._speed = pForce;
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(pLaunchPosition.x, pLaunchPosition.y, 0.0f);
    vector3.z = pStartZ;
    this._current_position_3d = vector3;
    this.calculateAngles(pLaunchPosition, pTargetPosition, vector3.z, pTargetPosZ);
    this.calculateVelocities();
    if (this.asset.frames == null || this.asset.frames.Length == 0)
      this.asset.frames = SpriteTextureLoader.getSpriteList("effects/projectiles/" + this.asset.texture);
    if (this.asset.sound_launch != string.Empty)
      MusicBox.playSound(this.asset.sound_launch, pLaunchPosition.x, pLaunchPosition.y, true);
    this._current_scale = this.asset.scale_start;
    this._target_scale = this.asset.scale_target;
    this._is_target_reached = false;
    this._dead_alpha = 1f;
    this.setState(ProjectileState.Active);
  }

  private void calculateAngles(Vector3 pStart, Vector3 pTarget, float pStartZ, float pTargetZ)
  {
    this._vector_start = Vector2.op_Implicit(pStart);
    this._vector_target = Vector2.op_Implicit(pTarget);
    this._angle_horizontal = this.getHorizontalLaunchAngle(pStart, pTarget);
    this._angle_vertical = this.getVerticalLaunchAngle(pStart, pTarget, this._speed, pStartZ, pTargetZ);
  }

  private void calculateVelocities()
  {
    float num = this._speed / this.asset.mass;
    this._velocity.x = num * Mathf.Cos(this._angle_vertical) * Mathf.Cos(this._angle_horizontal);
    this._velocity.y = num * Mathf.Cos(this._angle_vertical) * Mathf.Sin(this._angle_horizontal);
    this._velocity.z = num * Mathf.Sin(this._angle_vertical);
  }

  private float getVerticalLaunchAngle(
    Vector3 pStart,
    Vector3 pTarget,
    float pForce,
    float pStartHeight,
    float pTargetHeight)
  {
    float gravity = SimGlobals.m.gravity;
    float num1 = Toolbox.DistVec3(pStart, pTarget);
    float num2 = pTargetHeight - pStartHeight;
    float num3 = pForce / this.asset.mass;
    float num4 = num3 * num3;
    float num5 = Mathf.Pow(num3, 4f);
    float num6 = gravity * (float) ((double) gravity * (double) num1 * (double) num1 + 2.0 * (double) num2 * (double) num4);
    if ((double) num6 > (double) num5)
      return 0.7853982f;
    float num7 = Mathf.Sqrt(num5 - num6);
    float num8 = Mathf.Atan((float) (((double) num4 + (double) num7) / ((double) SimGlobals.m.gravity * (double) num1)));
    float num9 = Mathf.Atan((float) (((double) num4 - (double) num7) / ((double) SimGlobals.m.gravity * (double) num1)));
    float num10 = 2f * num3 * Mathf.Sin(num8) / gravity;
    float num11 = 2f * num3 * Mathf.Sin(num9) / gravity;
    return !this.asset.use_min_angle_height ? ((double) num8 <= (double) num9 ? num9 : num8) : ((double) num10 < (double) num11 ? num8 : num9);
  }

  private float getHorizontalLaunchAngle(Vector3 pStart, Vector3 pTarget)
  {
    return Mathf.Atan2(pTarget.y - pStart.y, pTarget.x - pStart.x);
  }

  private void updateVelocity(float pElapsed)
  {
    this._velocity.z -= SimGlobals.m.gravity * pElapsed;
    this._current_position_3d = Vector3.op_Addition(this._current_position_3d, Vector3.op_Multiply(this._velocity, pElapsed));
    this.rotation = Quaternion.AngleAxis(Mathf.Atan2(this._velocity.y + this._velocity.z, this._velocity.x) * 57.29578f, Vector3.forward);
    if ((double) this._current_position_3d.z <= 0.0)
    {
      ((Vector3) ref this._velocity).Set(0.0f, 0.0f, 0.0f);
      this._current_position_3d.z = 0.0f;
    }
    if ((double) this._collision_timeout != 0.0 && (double) this._current_position_3d.z != 0.0)
      return;
    AttackDataResult pDataResult = this.checkHitOnNearbyUnits();
    switch (pDataResult.state)
    {
      case ApplyAttackState.Hit:
        this.setState(ProjectileState.ToRemove);
        Action killAction = this._kill_action;
        if (killAction != null)
          killAction();
        this.targetReached();
        break;
      case ApplyAttackState.Block:
        this.getCollided(Vector2.op_Implicit(this._vector_target));
        break;
      case ApplyAttackState.Deflect:
        this.getDeflected(Vector2.op_Implicit(this._vector_start), pDataResult);
        break;
      case ApplyAttackState.Continue:
        break;
      default:
        EffectsLibrary.spawnAt("fx_miss", this._current_position_3d, 0.1f);
        if (this.asset.can_be_left_on_ground)
          this.setState(ProjectileState.AlphaAnimation);
        else
          this.setState(ProjectileState.ToRemove);
        this.targetReached();
        break;
    }
  }

  private bool isOnGround() => (double) this._current_position_3d.z <= 0.0;

  private AttackDataResult checkHitOnNearbyUnits()
  {
    if (this._is_target_reached)
      return AttackDataResult.Continue;
    WorldTile tile = World.world.GetTile((int) this._current_position_3d.x, (int) this._current_position_3d.y);
    if (tile == null)
      return this.isOnGround() ? AttackDataResult.Miss : AttackDataResult.Continue;
    if (this.by_who.isRekt())
      return AttackDataResult.Miss;
    EnemyFinderData enemiesFrom = EnemiesFinder.findEnemiesFrom(tile, this.kingdom);
    int num = this.isMainTargetStillValid() ? 1 : 0;
    if (num == 0)
    {
      this._main_target = (BaseSimObject) null;
      this._main_target_id = -1L;
    }
    if (num != 0 && !enemiesFrom.list.Contains(this._main_target))
      enemiesFrom.list.Add(this._main_target);
    if (enemiesFrom.isEmpty())
      return this.isOnGround() ? AttackDataResult.Miss : AttackDataResult.Continue;
    Vector3 vector3_1 = Vector2.op_Implicit(this.by_who.current_position);
    BaseSimObject byWho = this.by_who;
    Kingdom kingdom = this.kingdom;
    Vector3 vector3_2 = vector3_1;
    WorldTile pHitTile = tile;
    Vector3 currentPosition3d = this._current_position_3d;
    Vector3 pInitiatorPosition = vector3_2;
    Kingdom pKingdom = kingdom;
    string id = this.asset.id;
    AttackData pData = new AttackData(byWho, pHitTile, currentPosition3d, pInitiatorPosition, (BaseSimObject) null, pKingdom, AttackType.Weapon, pSkipShake: false, pProjectile: true, pProjectileID: id);
    if (this.isOnGround())
    {
      AttackDataResult attackDataResult = MapBox.newAttack(pData);
      if (attackDataResult.state == ApplyAttackState.Continue)
        attackDataResult.state = ApplyAttackState.Miss;
      return attackDataResult;
    }
    foreach (BaseSimObject pObject in enemiesFrom.list.LoopRandom<BaseSimObject>())
    {
      AttackDataResult attackDataResult = this.checkHitForUnit(pObject, pData);
      switch (attackDataResult.state)
      {
        case ApplyAttackState.Hit:
        case ApplyAttackState.Block:
        case ApplyAttackState.Deflect:
          return attackDataResult;
        default:
          continue;
      }
    }
    return AttackDataResult.Continue;
  }

  private bool isMainTargetStillValid()
  {
    return !this._main_target.isRekt() && this._main_target.id == this._main_target_id;
  }

  private AttackDataResult checkHitForUnit(BaseSimObject pObject, AttackData pData)
  {
    if (pObject == null || !pObject.isAlive() || (double) Mathf.Abs(this._current_position_3d.z - pObject.getHeight()) > 3.0)
      return AttackDataResult.Continue;
    Vector3 vector3 = Vector2.op_Implicit(pObject.current_position);
    Vector3 currentPosition3d = this._current_position_3d;
    return (double) Toolbox.Dist(currentPosition3d.x, currentPosition3d.y + currentPosition3d.z, vector3.x, vector3.y + pObject.getHeight()) > (double) (this.asset.size + pObject.stats["size"]) ? AttackDataResult.Continue : MapBox.checkAttackFor(pData, pObject);
  }

  public void setStats(BaseStats pStats)
  {
    this.stats.clear();
    this.stats.mergeStats(pStats);
  }

  public float getLaunchAngle()
  {
    return Mathf.Atan2(this._velocity.y + this._velocity.z, this._velocity.x) * 57.29578f;
  }

  private void updateScaleEffect(float pElapsed)
  {
    if ((double) this._current_scale >= (double) this._target_scale)
      return;
    this._current_scale += pElapsed * 0.2f;
    if ((double) this._current_scale <= (double) this._target_scale)
      return;
    this._current_scale = this._target_scale;
  }

  private void updateTrailEffect(float pElapsed)
  {
    if (!this.asset.trail_effect_enabled)
      return;
    if ((double) this._timer_smoke > 0.0)
    {
      this._timer_smoke -= pElapsed;
    }
    else
    {
      Vector3 pPos;
      // ISSUE: explicit constructor call
      ((Vector3) ref pPos).\u002Ector(this._current_position_3d.x, this._current_position_3d.y + this._current_position_3d.z, 0.0f);
      BaseEffect baseEffect = EffectsLibrary.spawnAt(this.asset.trail_effect_id, pPos, this.asset.trail_effect_scale);
      if (this.asset.look_at_target && Object.op_Inequality((Object) baseEffect, (Object) null))
        ((Component) baseEffect).transform.rotation = this.rotation;
      this._timer_smoke = this.asset.trail_effect_timer;
    }
  }

  public void update(float pElapsed)
  {
    if ((double) this._collision_timeout > 0.0)
    {
      this._collision_timeout -= pElapsed;
      if ((double) this._collision_timeout < 0.0)
        this._collision_timeout = 0.0f;
    }
    switch (this._state)
    {
      case ProjectileState.Active:
        this.updateVelocity(pElapsed);
        break;
      case ProjectileState.AlphaAnimation:
        this.updateDeadAnimation(pElapsed);
        break;
    }
    this.updateScaleEffect(pElapsed);
    this.updateTrailEffect(pElapsed);
    this.updateLightEffect(pElapsed);
  }

  private void updateLightEffect(float pElapsed)
  {
    if (!this.asset.draw_light_area)
      return;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(this._current_position_3d.x, this._current_position_3d.y + this._current_position_3d.z);
    vector2.x += this.asset.draw_light_area_offset_x;
    vector2.y += this.asset.draw_light_area_offset_y;
    World.world.stack_effects.light_blobs.Add(new LightBlobData()
    {
      position = vector2,
      radius = this.asset.draw_light_size
    });
  }

  private void updateDeadAnimation(float pElapsed)
  {
    this._dead_alpha -= pElapsed * 0.5f;
    if ((double) this._dead_alpha >= 0.0)
      return;
    this.setState(ProjectileState.ToRemove);
  }

  private void targetReached()
  {
    this._is_target_reached = true;
    WorldTile pTile = this.getCurrentTilePosition() ?? World.world.GetTile((int) this._vector_target.x, (int) this._vector_target.y);
    if (this.asset.impact_actions != null && pTile != null && !this.asset.impact_actions(this.by_who, (BaseSimObject) null, pTile))
    {
      this.reset();
    }
    else
    {
      Vector2 positionWithHeight = this.getTransformedPositionWithHeight();
      if (!string.IsNullOrEmpty(this.asset.end_effect))
        EffectsLibrary.spawnAt(this.asset.end_effect, positionWithHeight, this.asset.end_effect_scale);
      if (this.asset.sound_impact != string.Empty)
        MusicBox.playSound(this.asset.sound_impact, positionWithHeight.x, positionWithHeight.y, true);
      if (pTile != null)
      {
        if (this.asset.world_actions != null)
        {
          int num = this.asset.world_actions(this.by_who, (BaseSimObject) null, pTile) ? 1 : 0;
        }
        if (this.asset.hit_freeze)
        {
          pTile.freeze();
          for (int index = 0; index < pTile.neighbours.Length; ++index)
          {
            WorldTile neighbour = pTile.neighbours[index];
            if (Randy.randomBool())
              neighbour.freeze();
          }
        }
        if (this.asset.hit_shake && pTile.zone.visible && MapBox.isRenderGameplay())
          World.world.startShake(this.asset.shake_duration, this.asset.shake_interval, this.asset.shake_intensity, this.asset.shake_x, this.asset.shake_y);
        if (this.asset.terraform_option != string.Empty)
          MapAction.damageWorld(pTile, this.asset.terraform_range, AssetManager.terraform.get(this.asset.terraform_option), this.by_who);
      }
      this.reset();
    }
  }

  public void getDeflected(Vector3 pPos, AttackDataResult pDataResult)
  {
    this.start((BaseSimObject) World.world.units.get(pDataResult.deflected_by_who_id), (BaseSimObject) null, Vector2.op_Implicit(this.getCurrentPosition()), Vector2.op_Implicit(this._vector_start), this.asset.id, this.getCurrentHeight(), this.getCurrentHeight(), this._speed);
  }

  public void getCollided(Vector3 pPos)
  {
    if (this.asset.trigger_on_collision)
    {
      this.targetReached();
      this.setState(ProjectileState.ToRemove);
    }
    else
    {
      this._collision_timeout = 1f;
      Vector3 pLaunchPosition = Vector2.op_Implicit(this.getCurrentPosition());
      Vector3 newPoint = Toolbox.getNewPoint(pLaunchPosition.x, pLaunchPosition.y, pPos.x, pPos.y, 6f);
      this.start(this.by_who, (BaseSimObject) null, pLaunchPosition, newPoint, this.asset.id, this.getCurrentHeight() + 5f, this.getCurrentHeight(), this._speed * 0.2f);
    }
  }

  private void setState(ProjectileState pState) => this._state = pState;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isTargetReached() => this._is_target_reached;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float getCurrentHeight() => this._current_position_3d.z;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Vector2 getCurrentPosition() => Vector2.op_Implicit(this._current_position_3d);

  private WorldTile getCurrentTilePosition()
  {
    return World.world.GetTile((int) this._current_position_3d.x, (int) this._current_position_3d.y);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Vector2 getTransformedPositionWithHeight()
  {
    Vector2 positionWithHeight = Vector2.op_Implicit(this._current_position_3d);
    positionWithHeight.y += this.getCurrentHeight();
    return positionWithHeight;
  }

  public float getCurrentScale() => this._current_scale;

  public float getAngleForShadow()
  {
    return Toolbox.getAngleDegrees(this._current_position_3d.x, this._current_position_3d.y, this._vector_target.x, this._vector_target.y);
  }

  public override void setAlive(bool pValue) => this._alive = pValue;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isFinished() => this._state == ProjectileState.ToRemove;

  public bool isDeadAnimation() => this._state == ProjectileState.AlphaAnimation;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool canBeCollided()
  {
    return this.asset.can_be_collided && !this.isFinished() && !this.isTargetReached() && (double) this._collision_timeout <= 0.0;
  }

  public float getAlpha() => this._dead_alpha;

  private void reset()
  {
    this._kill_action = (Action) null;
    this._collision_timeout = 0.0f;
    this.by_who = (BaseSimObject) null;
    this._main_target = (BaseSimObject) null;
    this._main_target_id = -1L;
    this.kingdom = (Kingdom) null;
    this.stats.clear();
  }

  public override void Dispose()
  {
    this.reset();
    this.asset = (ProjectileAsset) null;
    this.stats.reset();
    base.Dispose();
  }
}
