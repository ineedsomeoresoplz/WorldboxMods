// Decompiled with JetBrains decompiler
// Type: FamilyManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FamilyManager : MetaSystemManager<Family, FamilyData>
{
  public FamilyManager() => this.type_id = "family";

  public Family newFamily(Actor pActor, WorldTile pTile, Actor pActor2)
  {
    ++World.world.game_stats.data.familiesCreated;
    ++World.world.map_stats.familiesCreated;
    Family pObject = this.newObject();
    pObject.newFamily(pActor, pActor2, pTile);
    if (pActor.hasFamily())
      pObject.saveOriginFamily1(pActor.family.id);
    pActor.setFamily(pObject);
    if (pActor2 != null)
    {
      if (pActor2.hasFamily())
        pObject.saveOriginFamily2(pActor2.family.id);
      pActor2.setFamily(pObject);
    }
    return pObject;
  }

  public override void removeObject(Family pObject)
  {
    ++World.world.game_stats.data.familiesDestroyed;
    ++World.world.map_stats.familiesDestroyed;
    base.removeObject(pObject);
  }

  public Family getNearbyFamily(ActorAsset pUnitAsset, WorldTile pTile)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 4, (float) pUnitAsset.family_spawn_radius, true))
    {
      if (actor.isAlive() && actor.hasFamily() && !actor.family.isFull() && actor.family.isSameSpecies(pUnitAsset.id) && actor.current_tile.isSameIsland(pTile))
        return actor.family;
    }
    return (Family) null;
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Family family = pActor.family;
      if (family != null && family.isDirtyUnits())
        family.listUnit(pActor);
    }
  }
}
