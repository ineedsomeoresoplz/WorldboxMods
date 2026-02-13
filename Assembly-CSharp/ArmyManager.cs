// Decompiled with JetBrains decompiler
// Type: ArmyManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ArmyManager : MetaSystemManager<Army, ArmyData>
{
  public ArmyManager() => this.type_id = "army";

  public Army newArmy(Actor pActor, City pCity)
  {
    ++World.world.game_stats.data.armiesCreated;
    ++World.world.map_stats.armiesCreated;
    Army army = this.newObject();
    army.createArmy(pActor, pCity);
    pActor.setArmy(army);
    pCity.setArmy(army);
    return army;
  }

  public override void removeObject(Army pObject)
  {
    ++World.world.game_stats.data.armiesDestroyed;
    ++World.world.map_stats.armiesDestroyed;
    base.removeObject(pObject);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    foreach (Army army in (CoreSystemManager<Army, ArmyData>) this)
      army.checkCaptainExistence();
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Army army = pActor.army;
      if (army != null && army.isDirtyUnits())
        army.listUnit(pActor);
    }
  }
}
