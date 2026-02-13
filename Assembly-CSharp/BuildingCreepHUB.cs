// Decompiled with JetBrains decompiler
// Type: BuildingCreepHUB
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BuildingCreepHUB : BaseBuildingComponent
{
  private float _interval = 0.1f;
  private float _timer;
  private ListPool<BuildingCreepWorker> _workers;

  internal override void create(Building pBuilding)
  {
    this._workers = new ListPool<BuildingCreepWorker>();
    base.create(pBuilding);
    for (int index = 0; index < this.building.asset.grow_creep_workers; ++index)
      this._workers.Add(new BuildingCreepWorker(this));
    this._interval = this.building.asset.grow_creep_step_interval;
    this._timer = this._interval;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if ((double) this._timer > 0.0)
    {
      this._timer -= pElapsed;
    }
    else
    {
      this._timer = this._interval;
      ListPool<BuildingCreepWorker> workers = this._workers;
      for (int index = 0; index < workers.Count; ++index)
        workers[index].update();
    }
  }

  public override void Dispose()
  {
    for (int index = 0; index < this._workers.Count; ++index)
      this._workers[index].Dispose();
    this._workers.Dispose();
    this._workers = (ListPool<BuildingCreepWorker>) null;
    base.Dispose();
  }
}
