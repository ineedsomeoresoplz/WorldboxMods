// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBuildingTargetLoverHome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBuildingTargetLoverHome : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasLover())
      return BehResult.Stop;
    Building loverHomeBuilding = this.getLoverHomeBuilding(pActor, pActor.lover);
    if (loverHomeBuilding == null)
      return BehResult.Stop;
    pActor.beh_building_target = loverHomeBuilding;
    return BehResult.Continue;
  }

  private Building getLoverHomeBuilding(Actor pActor1, Actor pActor2)
  {
    if (pActor1.hasHouse() && pActor2.hasHouse())
      return pActor1.isSexMale() ? pActor1.getHomeBuilding() : pActor2.getHomeBuilding();
    if (pActor1.hasHouse())
      return pActor1.getHomeBuilding();
    return pActor2.hasHouse() ? pActor2.getHomeBuilding() : (Building) null;
  }
}
