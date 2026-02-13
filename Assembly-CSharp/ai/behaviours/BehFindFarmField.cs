// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindFarmField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindFarmField : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.city.calculated_grown_wheat.Count > 0)
      return BehResult.Stop;
    int num1 = int.MaxValue;
    WorldTile worldTile1 = (WorldTile) null;
    WorldTile currentTile = pActor.current_tile;
    WorldTileContainer calculatedFarmFields = pActor.city.calculated_farm_fields;
    calculatedFarmFields.checkAddRemove();
    foreach (WorldTile worldTile2 in (ObjectContainer<WorldTile>) calculatedFarmFields)
    {
      int num2 = Toolbox.SquaredDistTile(currentTile, worldTile2);
      if (num2 < num1 && worldTile2.Type.farm_field && !worldTile2.isTargeted() && currentTile.isSameIsland(worldTile2) && (!worldTile2.hasBuilding() || worldTile2.building.canRemoveForFarms() && !worldTile2.building.asset.wheat))
      {
        num1 = num2;
        worldTile1 = worldTile2;
      }
    }
    if (worldTile1 == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile1;
    return BehResult.Continue;
  }
}
