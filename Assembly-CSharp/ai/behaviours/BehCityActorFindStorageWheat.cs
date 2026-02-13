// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorFindStorageWheat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorFindStorageWheat : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    Building buildingOfType = pActor.city.getBuildingOfType("type_windmill", pLimitIsland: pActor.current_island);
    if (buildingOfType != null)
    {
      pActor.beh_building_target = buildingOfType;
      return BehResult.Continue;
    }
    Building storageNear = pActor.city.getStorageNear(pActor.current_tile, true);
    if (storageNear == null)
      return BehResult.Stop;
    pActor.beh_building_target = storageNear;
    return BehResult.Continue;
  }
}
