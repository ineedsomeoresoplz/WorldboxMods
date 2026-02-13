// Decompiled with JetBrains decompiler
// Type: Poop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Poop : BaseBuildingComponent
{
  public override void update(float pElapsed)
  {
    if (Time.frameCount % 30 != 0 || Date.getYearsSince(this.building.data.created_time) < 1)
      return;
    BuildingActions.tryGrowVegetationRandom(this.building.current_tile, VegetationType.Plants, pCheckLimit: false, pCheckRandom: false);
    this.building.startDestroyBuilding();
  }
}
