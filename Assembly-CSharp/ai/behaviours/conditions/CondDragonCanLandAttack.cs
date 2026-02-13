// Decompiled with JetBrains decompiler
// Type: ai.behaviours.conditions.CondDragonCanLandAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours.conditions;

public class CondDragonCanLandAttack : BehaviourActorCondition
{
  public override bool check(Actor pActor)
  {
    int pResult1;
    pActor.data.get("landAttacks", out pResult1);
    if (pResult1 >= 2)
      return false;
    int pResult2;
    pActor.data.get("attacksForCity", out pResult2);
    if (pResult2 > 0 && pActor.current_tile.zone.city != null)
    {
      long pResult3;
      pActor.data.get("cityToAttack", out pResult3, -1L);
      if (pResult3 == pActor.current_tile.zone.city.data.id)
        return true;
    }
    Dragon actorComponent = pActor.getActorComponent<Dragon>();
    if (actorComponent.targetsWithinLandAttackRange())
      return true;
    if (pActor.hasTrait("zombie") && World.world.kingdoms_wild.get("golden_brain").hasBuildings())
    {
      foreach (Building building in World.world.kingdoms_wild.get("golden_brain").buildings)
      {
        if (actorComponent.landAttackRange(building.current_tile))
          return true;
      }
    }
    return false;
  }
}
