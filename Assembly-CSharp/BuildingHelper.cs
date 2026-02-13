// Decompiled with JetBrains decompiler
// Type: BuildingHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class BuildingHelper
{
  private static List<WorldTile> _list_tiles = new List<WorldTile>();

  public static void tryToBuildNear(WorldTile pTile, string pAssetID)
  {
    BuildingAsset buildingAsset = AssetManager.buildings.get(pAssetID);
    if (buildingAsset == null)
      return;
    if (World.world.buildings.canBuildFrom(pTile, buildingAsset, (City) null))
      World.world.buildings.addBuilding(buildingAsset, pTile);
    else
      BuildingHelper.tryToBuildNear(pTile, buildingAsset);
  }

  public static bool tryToBuildNear(WorldTile pTile, BuildingAsset pAsset)
  {
    List<WorldTile> listTiles = BuildingHelper._list_tiles;
    BuildingHelper.fillEmptyTilesAroundMine(pTile, listTiles);
    int num = BuildingHelper.tryToPlaceBuilding(pAsset, listTiles) ? 1 : 0;
    listTiles.Clear();
    return num != 0;
  }

  private static void fillEmptyTilesAroundMine(WorldTile pTile, List<WorldTile> pList)
  {
    pList.Clear();
    int num1 = 4;
    int num2 = pTile.x - num1;
    int num3 = pTile.y - num1;
    for (int index1 = 0; index1 < num1 * 2; ++index1)
    {
      for (int index2 = 0; index2 < num1 * 2; ++index2)
      {
        WorldTile tile = World.world.GetTile(index1 + num2, index2 + num3);
        if (tile != null && (!tile.hasBuilding() || !tile.building.isUsable() || !tile.building.asset.city_building))
          pList.Add(tile);
      }
    }
  }

  private static bool tryToPlaceBuilding(BuildingAsset pAsset, List<WorldTile> pList)
  {
    foreach (WorldTile pTile in pList.LoopRandom<WorldTile>())
    {
      if (World.world.buildings.canBuildFrom(pTile, pAsset, (City) null))
      {
        if (World.world.buildings.addBuilding(pAsset, pTile) != null)
          return true;
        break;
      }
    }
    return false;
  }
}
