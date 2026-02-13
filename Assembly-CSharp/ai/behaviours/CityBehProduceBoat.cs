// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehProduceBoat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class CityBehProduceBoat : BehaviourActionCity
{
  public override BehResult execute(City pCity)
  {
    if ((double) pCity.timer_build_boat > 0.0)
      return BehResult.Stop;
    Building buildingOfType = pCity.getBuildingOfType("type_docks", pRandom: true);
    if (buildingOfType == null)
      return BehResult.Stop;
    Actor actor = buildingOfType.component_docks.buildBoatFromHere(pCity);
    if (actor == null)
      return BehResult.Stop;
    actor.joinKingdom(pCity.kingdom);
    actor.joinCity(pCity);
    pCity.timer_build_boat = 10f;
    return BehResult.Continue;
  }
}
