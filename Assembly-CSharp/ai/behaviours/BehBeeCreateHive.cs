// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBeeCreateHive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBeeCreateHive : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (BehBeeCreateHive.isAnotherBeehiveNearby(pActor))
      return BehResult.Stop;
    Building building = BehaviourActionBase<Actor>.world.buildings.addBuilding("beehive", pActor.beh_tile_target, true);
    pActor.beh_building_target = building;
    return BehResult.Continue;
  }

  public static bool isAnotherBeehiveNearby(Actor pActor)
  {
    foreach (Building building in Finder.getBuildingsFromChunk(pActor.current_tile, 2))
    {
      if (building.asset.id == "beehive")
        return true;
    }
    return false;
  }
}
