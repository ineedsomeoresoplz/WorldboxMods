// Decompiled with JetBrains decompiler
// Type: BuildingFruitGrowth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BuildingFruitGrowth : BaseBuildingComponent
{
  private float _resource_reset_time;

  public override void update(float pElapsed)
  {
    if (!this.building.isNormal() || this.building.hasResourcesToCollect())
      return;
    if ((double) this._resource_reset_time > 0.0)
    {
      this._resource_reset_time -= pElapsed;
    }
    else
    {
      this.building.setHaveResourcesToCollect(true);
      this.building.setScaleTween(0.75f);
    }
  }

  public void reset() => this._resource_reset_time = 90f;
}
