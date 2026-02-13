// Decompiled with JetBrains decompiler
// Type: UnitSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class UnitSpawner : BaseBuildingComponent
{
  private const float SPAWN_INTERVAL = 10f;
  private float _spawn_timer = 1f;

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if ((double) this._spawn_timer > 0.0)
    {
      this._spawn_timer -= pElapsed;
    }
    else
    {
      this._spawn_timer = 10f;
      this.trySpawnUnit();
    }
  }

  private void trySpawnUnit()
  {
    int num = this.building.countResidents();
    if (num >= this.building.asset.housing_slots)
      return;
    Subspecies subspecies = (Subspecies) null;
    if (num > 0)
    {
      foreach (long resident in this.building.residents)
      {
        Actor pObject = World.world.units.get(resident);
        if (!pObject.isRekt())
        {
          subspecies = pObject.subspecies;
          break;
        }
      }
    }
    if (!subspecies.isRekt() && subspecies.hasReachedPopulationLimit())
      return;
    this.spawnUnit(subspecies);
  }

  private void spawnUnit(Subspecies pSubspecies)
  {
    Actor newUnit = World.world.units.createNewUnit(this.building.asset.spawn_units_asset, this.building.current_tile, pSubspecies: pSubspecies, pAdultAge: true);
    newUnit.applyRandomForce();
    this.setUnitFromHere(newUnit);
  }

  public void setUnitFromHere(Actor pActor) => pActor.setHomeBuilding(this.building);
}
