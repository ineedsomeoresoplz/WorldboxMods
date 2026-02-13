// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindBuildingWithFood
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindBuildingWithFood : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    Building storageWithFoodNear = pActor.city.getStorageWithFoodNear(pActor.current_tile);
    if (storageWithFoodNear == null)
      return BehResult.Stop;
    pActor.beh_building_target = storageWithFoodNear;
    return BehResult.Continue;
  }
}
