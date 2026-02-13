// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorFindUngrownCrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorFindUngrownCrop : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    City city = pActor.city;
    using (ListPool<Building> list = new ListPool<Building>())
    {
      foreach (WorldTile calculatedCrop in (ObjectContainer<WorldTile>) city.calculated_crops)
      {
        Building building = calculatedCrop.building;
        if (!building.isRekt() && building.asset.wheat && !building.component_wheat.isMaxLevel())
          list.Add(building);
      }
      if (list.Count == 0)
        return BehResult.Stop;
      pActor.beh_building_target = list.GetRandom<Building>();
      return BehResult.Continue;
    }
  }
}
