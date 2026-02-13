// Decompiled with JetBrains decompiler
// Type: tools.OceanHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace tools;

public class OceanHelper
{
  private static List<TileIsland> _ocean_pools_with_docks = new List<TileIsland>();

  public static bool goodForNewDock(WorldTile pTile)
  {
    return pTile.Type.ocean && pTile.region?.island != null && pTile.region.island.goodForDocks() && !OceanHelper._ocean_pools_with_docks.Contains(pTile.region.island);
  }

  public static void saveOceanPoolsWithDocks(City pCity)
  {
    List<Building> buildingListOfType = pCity.getBuildingListOfType("type_docks");
    if (buildingListOfType == null || buildingListOfType.Count == 0)
      return;
    for (int index1 = 0; index1 < buildingListOfType.Count; ++index1)
    {
      Building building = buildingListOfType[index1];
      for (int index2 = 0; index2 < building.tiles.Count; ++index2)
        OceanHelper.addOceanPool(building.tiles[index2]);
    }
  }

  public static void clearOceanPools() => OceanHelper._ocean_pools_with_docks.Clear();

  private static void addOceanPool(WorldTile pTile)
  {
    if (pTile.region.island == null || pTile.region.type != TileLayerType.Ocean)
      return;
    OceanHelper._ocean_pools_with_docks.Add(pTile.region.island);
  }

  public static WorldTile findTileForBoat(WorldTile pTileBoat, WorldTile pTileTarget)
  {
    WorldTile pTile = OceanHelper.findWaterTileInRegion(pTileTarget.region, pTileBoat) ?? OceanHelper.findTileInWholeIsland(pTileTarget.region.island, pTileBoat);
    if (pTile == null)
      return (WorldTile) null;
    for (int index = 0; index < pTile.neighboursAll.Length; ++index)
    {
      WorldTile worldTile = pTile.neighboursAll[index];
      if (worldTile.isSameIsland(pTileBoat) && worldTile.isGoodForBoat())
      {
        pTile = worldTile;
        break;
      }
    }
    World.world.flash_effects.flashPixel(pTile);
    return pTile;
  }

  private static WorldTile findTileInWholeIsland(TileIsland pIslandTarget, WorldTile pTileBoat)
  {
    WorldTile tileInWholeIsland = (WorldTile) null;
    int num1 = 10;
    float num2 = float.MaxValue;
    foreach (MapRegion pRegion in pIslandTarget.regions.getSimpleList().LoopRandom<MapRegion>())
    {
      WorldTile waterTileInRegion = OceanHelper.findWaterTileInRegion(pRegion, pTileBoat);
      if (waterTileInRegion != null)
      {
        float num3 = Toolbox.DistTile(pTileBoat, waterTileInRegion);
        if ((double) num3 < (double) num2)
        {
          tileInWholeIsland = waterTileInRegion;
          num2 = num3;
          --num1;
          if (num1 <= 0)
            break;
        }
      }
    }
    return tileInWholeIsland;
  }

  private static WorldTile findWaterTileInRegion(MapRegion pRegion, WorldTile pBoatTile)
  {
    List<WorldTile> edgeTiles = pRegion.getEdgeTiles();
    if (edgeTiles.Count == 0)
      return (WorldTile) null;
    foreach (WorldTile waterTileInRegion in edgeTiles.LoopRandom<WorldTile>())
    {
      if (waterTileInRegion.isSameIsland(pBoatTile))
        return waterTileInRegion;
    }
    return (WorldTile) null;
  }
}
