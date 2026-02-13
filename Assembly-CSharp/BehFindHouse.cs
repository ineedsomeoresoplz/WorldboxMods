// Decompiled with JetBrains decompiler
// Type: BehFindHouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehFindHouse : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.hasHouse())
      return BehResult.Stop;
    Building pBuilding = (Building) null;
    foreach (Building building in pActor.city.buildings)
    {
      if (!building.isUnderConstruction() && building.hasResidentSlots())
      {
        pBuilding = building;
        break;
      }
    }
    if (pBuilding == null)
      pBuilding = BehFindHouse.tryToFindFamilyHouse(pActor);
    if (pBuilding == null)
      return BehResult.Stop;
    pActor.setHomeBuilding(pBuilding);
    pActor.changeHappiness("just_found_house", pBuilding.asset.housing_happiness);
    return BehResult.Continue;
  }

  private static Building tryToFindFamilyHouse(Actor pActor)
  {
    if (!pActor.hasFamily())
      return (Building) null;
    int num = 0;
    Family family = pActor.family;
    foreach (Actor actor in pActor.family.units.LoopRandom<Actor>())
    {
      if (actor != pActor)
      {
        if (++num <= 5)
        {
          if (actor.hasHouse() && actor.city == pActor.city)
          {
            Building findFamilyHouse = BehFindHouse.checkBuilding(actor.home_building, family);
            if (findFamilyHouse != null)
              return findFamilyHouse;
          }
        }
        else
          break;
      }
    }
    return (Building) null;
  }

  private static Building checkBuilding(Building pGetHomeBuilding, Family pFamily)
  {
    foreach (long resident in pGetHomeBuilding.residents)
    {
      Actor actor = BehaviourActionBase<Actor>.world.units.get(resident);
      if (actor != null && actor.isAlive() && actor.family == pFamily)
      {
        actor.clearHomeBuilding();
        return pGetHomeBuilding;
      }
    }
    return (Building) null;
  }
}
