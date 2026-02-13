// Decompiled with JetBrains decompiler
// Type: CityBehCheckFarms
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class CityBehCheckFarms : BehaviourActionCity
{
  public override bool shouldRetry(City pCity) => false;

  public override BehResult execute(City pCity)
  {
    CityBehCheckFarms.check(pCity);
    return BehResult.Continue;
  }

  public static void check(City pCity)
  {
    pCity.calculated_place_for_farms.Clear();
    pCity.calculated_grown_wheat.Clear();
    pCity.calculated_farm_fields.Clear();
    pCity.calculated_crops.Clear();
    CityBehCheckFarms.behFindTileForFarm(pCity);
    pCity.calculated_place_for_farms.checkAddRemove();
    pCity.calculated_farm_fields.checkAddRemove();
    pCity.calculated_crops.checkAddRemove();
    CityBehCheckFarms.behCheckWheat(pCity);
    pCity.calculated_grown_wheat.checkAddRemove();
  }

  private static void behCheckWheat(City pCity)
  {
    foreach (WorldTile calculatedCrop in (ObjectContainer<WorldTile>) pCity.calculated_crops)
    {
      if (calculatedCrop.hasBuilding() && calculatedCrop.building.asset.wheat && calculatedCrop.building.component_wheat.isMaxLevel())
        pCity.calculated_grown_wheat.Add(calculatedCrop);
    }
  }

  private static void behFindTileForFarm(City pCity)
  {
    Building buildingOfType = pCity.getBuildingOfType("type_windmill");
    if (buildingOfType == null)
      return;
    CityBehCheckFarms.checkRegion(buildingOfType.current_tile.region, buildingOfType, pCity);
    for (int index = 0; index < buildingOfType.current_tile.region.neighbours.Count; ++index)
      CityBehCheckFarms.checkRegion(buildingOfType.current_tile.region.neighbours[index], buildingOfType, pCity);
  }

  private static void checkRegion(MapRegion pRegion, Building pBuilding, City pCity)
  {
    MapChunk chunk = pRegion.chunk;
    for (int index = 0; index < chunk.zones.Count; ++index)
      CityBehCheckFarms.checkZone(chunk.zones[index], pBuilding, pCity);
  }

  private static void checkZone(TileZone pZone, Building pBuilding, City pCity)
  {
    if (!pZone.isSameCityHere(pCity))
      return;
    WorldTile[] tiles = pZone.tiles;
    int length = tiles.Length;
    for (int index = 0; index < length; ++index)
    {
      WorldTile worldTile = tiles[index];
      if ((double) Toolbox.SquaredDistTile(pBuilding.current_tile, worldTile) <= 81.0)
      {
        if (worldTile.Type.can_be_farm)
          pCity.calculated_place_for_farms.Add(worldTile);
        if (worldTile.Type.farm_field)
        {
          pCity.calculated_farm_fields.Add(worldTile);
          if (worldTile.hasBuilding() && worldTile.building.asset.wheat)
            pCity.calculated_crops.Add(worldTile);
        }
      }
    }
  }
}
