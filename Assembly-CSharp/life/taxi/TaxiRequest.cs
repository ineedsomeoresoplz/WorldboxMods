// Decompiled with JetBrains decompiler
// Type: life.taxi.TaxiRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace life.taxi;

public class TaxiRequest
{
  private WorldTile _tile_target;
  private WorldTile _tile_start;
  private readonly HashSet<Actor> _actors = new HashSet<Actor>();
  private Boat _boat;
  public TaxiRequestState state;
  private Kingdom _kingdom;

  public TaxiRequest(Actor pActor, Kingdom pKingdom, WorldTile pStartTile, WorldTile pTargetTile)
  {
    this._kingdom = pKingdom;
    this.addActor(pActor);
    this._tile_start = pStartTile;
    this._tile_target = pTargetTile;
    this.setState(TaxiRequestState.Pending);
  }

  public WorldTile getTileTarget() => this._tile_target;

  public WorldTile getTileStart() => this._tile_start;

  public bool isSameKingdom(Kingdom pKingdom) => this._kingdom == pKingdom;

  public HashSet<Actor> getActors() => this._actors;

  public int countActors() => this._actors.Count;

  public bool hasActor(Actor pActor) => this._actors.Contains(pActor);

  public bool addActor(Actor pActor) => this._actors.Add(pActor);

  public void cancel()
  {
    this.setState(TaxiRequestState.Finished);
    this.clear();
  }

  public void finish()
  {
    this.setState(TaxiRequestState.Finished);
    this.clear();
  }

  private void checkList()
  {
    using (ListPool<Actor> other = new ListPool<Actor>(this._actors.Count))
    {
      foreach (Actor actor in this._actors)
      {
        bool flag = false;
        if (actor.isRekt())
          flag = true;
        else if (!actor.current_tile.isSameIsland(this._tile_start))
          flag = true;
        else if (actor.current_tile.isSameIsland(this._tile_target))
          flag = true;
        if (flag)
          other.Add(actor);
      }
      this._actors.ExceptWith((IEnumerable<Actor>) other);
    }
  }

  public void removeDeadUnits()
  {
    using (ListPool<Actor> other = new ListPool<Actor>(this._actors.Count))
    {
      foreach (Actor actor in this._actors)
      {
        if (actor.isRekt())
          other.Add(actor);
      }
      if (other.Count == 0)
        return;
      this._actors.ExceptWith((IEnumerable<Actor>) other);
    }
  }

  public bool isAlreadyUsedByBoat(Actor pBoatActor)
  {
    return this.isState(TaxiRequestState.Assigned) && this.isStillLegit() && this.isAssignedToBoat(pBoatActor);
  }

  public bool isAssignedToBoat(Actor pBoatActor) => this._boat.actor == pBoatActor;

  public bool isStillLegit()
  {
    if (!this.isState(TaxiRequestState.Pending) && !this.hasAssignedBoat() || this.isState(TaxiRequestState.Finished))
      return false;
    if ((this.isState(TaxiRequestState.Assigned) || this.isState(TaxiRequestState.Loading) || this.isState(TaxiRequestState.Transporting)) && this.hasAssignedBoat() && this._boat.hasPassengers())
      return true;
    this.checkList();
    return this._actors.Count != 0;
  }

  public bool hasAssignedBoat() => this._boat != null && !this._boat.actor.isRekt();

  public void assign(Boat pTaxi)
  {
    this._boat = pTaxi;
    this.setState(TaxiRequestState.Assigned);
  }

  public Boat getBoat() => this._boat;

  public bool isBoatNearDock() => this._boat.isNearDock();

  public void setState(TaxiRequestState pState) => this.state = pState;

  public void cancelForLatePassengers()
  {
    using (ListPool<Actor> other = new ListPool<Actor>(this._actors.Count))
    {
      foreach (Actor actor in this._actors)
      {
        if (!actor.is_inside_boat || actor.inside_boat != this._boat)
          other.Add(actor);
      }
      this._actors.ExceptWith((IEnumerable<Actor>) other);
    }
  }

  public bool embarkToBoat(Actor pActor) => this._actors.Remove(pActor);

  public bool everyoneEmbarked()
  {
    this.checkList();
    foreach (Actor actor in this._actors)
    {
      if (!this._boat.hasPassenger(actor))
        return false;
    }
    return true;
  }

  public bool isState(TaxiRequestState pState) => this.state == pState;

  public void clear()
  {
    this._actors.Clear();
    this._boat = (Boat) null;
    this._tile_target = (WorldTile) null;
    this._tile_start = (WorldTile) null;
    this._kingdom = (Kingdom) null;
  }
}
