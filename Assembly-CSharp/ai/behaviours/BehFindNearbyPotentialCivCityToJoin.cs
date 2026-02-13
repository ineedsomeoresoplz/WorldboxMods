// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindNearbyPotentialCivCityToJoin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehFindNearbyPotentialCivCityToJoin : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    City cityToJoin = BehFindNearbyPotentialCivCityToJoin.getCityToJoin(pActor);
    if (cityToJoin == null)
      return BehResult.Stop;
    if (cityToJoin.kingdom != pActor.kingdom)
      pActor.removeFromPreviousFaction();
    pActor.stopBeingWarrior();
    pActor.joinCity(cityToJoin);
    return BehResult.Continue;
  }

  private static City getCityToJoin(Actor pActor)
  {
    City zoneCity = pActor.current_tile.zone_city;
    return zoneCity != null && !zoneCity.isNeutral() && zoneCity.isWelcomedToJoin(pActor) ? zoneCity : BehFindNearbyPotentialCivCityToJoin.getPotentialCityNearby(pActor.current_tile, pActor);
  }

  public static City getPotentialCityNearby(WorldTile pFromTile, Actor pActor)
  {
    City potentialCityNearby = (City) null;
    foreach (City city in BehaviourActionBase<Actor>.world.cities.list.LoopRandom<City>())
    {
      WorldTile tile = city.getTile();
      if (tile != null && !city.isNeutral())
      {
        float checkFarCityRange = SimGlobals.m.nomad_check_far_city_range;
        if (!pActor.isNomad() && city.kingdom != pActor.kingdom)
          checkFarCityRange *= 3f;
        if ((double) Toolbox.DistVec3(pFromTile.posV, Vector2.op_Implicit(city.city_center)) <= (double) checkFarCityRange && city.isWelcomedToJoin(pActor) && tile.isSameIsland(pFromTile))
        {
          if (potentialCityNearby != null)
          {
            if (city.kingdom == pActor.kingdom && potentialCityNearby.kingdom != pActor.kingdom)
              potentialCityNearby = city;
          }
          else
            potentialCityNearby = city;
        }
      }
    }
    return potentialCityNearby;
  }
}
