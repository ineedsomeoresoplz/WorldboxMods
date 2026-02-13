// Decompiled with JetBrains decompiler
// Type: BuildingEffectSpawnDrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BuildingEffectSpawnDrop : BaseBuildingComponent
{
  private float _timer;

  public override void update(float pElapsed)
  {
    if (this.building.data.hasFlag("stop_spawn_drops"))
      return;
    if ((double) this._timer >= 0.0)
    {
      this._timer -= pElapsed;
    }
    else
    {
      int pAmount = Mathf.CeilToInt((float) -((double) this._timer / (double) this.building.asset.spawn_drop_interval));
      if (pAmount < 1)
        pAmount = 1;
      this._timer = this.building.asset.spawn_drop_interval;
      this.building.spawnBurstSpecial(pAmount);
    }
  }
}
