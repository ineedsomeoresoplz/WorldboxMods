// Decompiled with JetBrains decompiler
// Type: Boat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Boat : ActorSimpleComponent
{
  private readonly HashSet<Actor> _passengers = new HashSet<Actor>();
  internal TaxiRequest taxi_request;
  internal int passengerWaitCounter;
  public WorldTile taxi_target;
  public bool pickup_near_dock;
  internal int last_movement_angle;
  private Vector2 _last_step = Vector2.zero;

  internal override void create(Actor pActor)
  {
    base.create(pActor);
    this.actor.callbacks_on_death += new BaseActionActor(this.deathAction);
    this.actor.callbacks_on_death += new BaseActionActor(this.spawnBoatExplosion);
    this.actor.callbacks_landed += new BaseActionActor(this.cancelWork);
    this.actor.callbacks_cancel_path_movement += new BaseActionActor(this.cancelPathfinderMovement);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (!this.actor.is_moving)
      return;
    this.calculateMovementAngle();
  }

  public void calculateMovementAngle()
  {
    Vector2 currentPosition = this.actor.current_position;
    Vector2 nextStepPosition = this.actor.next_step_position;
    if (Vector2.op_Equality(this._last_step, nextStepPosition))
      return;
    this._last_step = nextStepPosition;
    this.last_movement_angle = (int) Toolbox.getAngleDegrees(currentPosition.x, currentPosition.y, nextStepPosition.x, nextStepPosition.y);
  }

  public bool isNearDock()
  {
    Building homeBuilding = this.actor.getHomeBuilding();
    return homeBuilding != null && homeBuilding.component_docks.tiles_ocean.Contains(this.actor.current_tile);
  }

  private void cancelPathfinderMovement(Actor pActor) => this.cancelWork(pActor);

  internal void cancelWork(Actor pActor)
  {
    this.actor.cancelAllBeh();
    if (this.taxi_request == null)
      return;
    TaxiManager.cancelRequest(this.taxi_request);
    this.taxi_request = (TaxiRequest) null;
    this.taxi_target = (WorldTile) null;
  }

  private string _boat_texture_id => this.actor.asset.boat_texture_id;

  public AnimationDataBoat getAnimationDataBoat()
  {
    return ActorAnimationLoader.loadAnimationBoat(this._boat_texture_id);
  }

  public void spawnBoatExplosion(Actor pActor)
  {
    EffectsLibrary.spawnAt("fx_boat_explosion", this.actor.current_position, this.actor.asset.base_stats["scale"]);
  }

  private void deathAction(Actor pActor)
  {
    if (this.taxi_request != null)
    {
      TaxiManager.finish(this.taxi_request);
      this.taxi_request = (TaxiRequest) null;
    }
    this.unloadPassengers(this.actor.current_tile, true);
  }

  internal void unloadPassengers(WorldTile pTile, bool pRandomForce = false)
  {
    foreach (Actor passenger in this._passengers)
    {
      if (passenger.isAlive())
      {
        passenger.disembarkTo(this, pTile);
        if (pRandomForce)
          passenger.applyRandomForce();
      }
    }
    this._passengers.Clear();
    this.taxi_target = (WorldTile) null;
  }

  internal bool hasPassengers() => this._passengers.Count > 0;

  internal int countPassengers() => this._passengers.Count;

  internal bool hasPassenger(Actor pActor) => this._passengers.Contains(pActor);

  public IReadOnlyCollection<Actor> getPassengers()
  {
    return (IReadOnlyCollection<Actor>) this._passengers;
  }

  internal void removePassenger(Actor pActor) => this._passengers.Remove(pActor);

  internal void addPassenger(Actor pActor)
  {
    if (!this._passengers.Add(pActor))
      return;
    this.passengerWaitCounter = 0;
    if (this.taxi_request == null)
      return;
    this.taxi_request.embarkToBoat(pActor);
  }

  public bool isHomeDockFull()
  {
    Building homeBuilding = this.actor.getHomeBuilding();
    return homeBuilding == null || homeBuilding.component_docks.isFull(this.actor.asset.boat_type);
  }

  public bool isHomeDockOverfilled()
  {
    Building homeBuilding = this.actor.getHomeBuilding();
    return homeBuilding == null || homeBuilding.component_docks.isOverfilled(this.actor.asset.boat_type);
  }

  public void destroyBecauseOverfilled()
  {
    if (!this.isHomeDockOverfilled())
      return;
    this.actor.getHitFullHealth(AttackType.Explosion);
  }

  public override void Dispose()
  {
    this._passengers.Clear();
    this.taxi_target = (WorldTile) null;
    this.taxi_request = (TaxiRequest) null;
    base.Dispose();
  }
}
