// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTileForFarm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindTileForFarm : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    Building buildingOfType = pActor.city.getBuildingOfType("type_windmill");
    if (buildingOfType == null)
      return BehResult.Stop;
    int num1 = int.MaxValue;
    WorldTile worldTile = (WorldTile) null;
    foreach (WorldTile calculatedPlaceForFarm in (ObjectContainer<WorldTile>) pActor.city.calculated_place_for_farms)
    {
      int num2 = Toolbox.SquaredDistTile(buildingOfType.current_tile, calculatedPlaceForFarm);
      if (num2 < num1 && (!calculatedPlaceForFarm.hasBuilding() || calculatedPlaceForFarm.building.canRemoveForFarms()) && !calculatedPlaceForFarm.isTargeted() && pActor.current_tile.isSameIsland(calculatedPlaceForFarm) && calculatedPlaceForFarm.IsTypeAround((TileTypeBase) TopTileLibrary.field))
      {
        num1 = num2;
        worldTile = calculatedPlaceForFarm;
      }
    }
    if (worldTile == null)
    {
      foreach (WorldTile calculatedPlaceForFarm in (ObjectContainer<WorldTile>) pActor.city.calculated_place_for_farms)
      {
        int num3 = Toolbox.SquaredDistTile(buildingOfType.current_tile, calculatedPlaceForFarm);
        if (num3 < num1 && (!calculatedPlaceForFarm.hasBuilding() || calculatedPlaceForFarm.building.canRemoveForFarms()) && !calculatedPlaceForFarm.isTargeted() && pActor.current_tile.isSameIsland(calculatedPlaceForFarm))
        {
          num1 = num3;
          worldTile = calculatedPlaceForFarm;
        }
      }
    }
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }
}
