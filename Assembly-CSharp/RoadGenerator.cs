// Decompiled with JetBrains decompiler
// Type: RoadGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class RoadGenerator
{
  internal static List<WorldTile> checkedTiles = new List<WorldTile>();
  private static List<WorldTile> nextWave = new List<WorldTile>();
  private static List<WorldTile> curWave = new List<WorldTile>();
  private static List<Building> targetBuildings = new List<Building>();
  private static List<WorldTile> path = new List<WorldTile>();

  internal static void generateRoadFor(Building pBuilding)
  {
    City city = pBuilding.city;
    if (city == null || city.buildings.Count <= 2 || !pBuilding.door_tile.Type.ground)
      return;
    RoadGenerator.calcFlow(pBuilding.door_tile, pBuilding, 12);
  }

  private static void calcFlow(WorldTile pStartTile, Building pBuilding, int pMaxWave)
  {
    RoadGenerator.targetBuildings.Clear();
    RoadGenerator.checkedTiles.Clear();
    RoadGenerator.curWave.Clear();
    RoadGenerator.nextWave.Clear();
    RoadGenerator.nextWave.Add(pStartTile);
    RoadGenerator.checkedTiles.Add(pStartTile);
    int num = 0;
    while (RoadGenerator.nextWave.Count > 0 || RoadGenerator.curWave.Count > 0)
    {
      if (RoadGenerator.curWave.Count == 0)
      {
        if (num <= pMaxWave)
        {
          RoadGenerator.curWave.AddRange((IEnumerable<WorldTile>) RoadGenerator.nextWave);
          RoadGenerator.nextWave.Clear();
          ++num;
        }
        else
          break;
      }
      WorldTile pTile = RoadGenerator.curWave[RoadGenerator.curWave.Count - 1];
      RoadGenerator.curWave.RemoveAt(RoadGenerator.curWave.Count - 1);
      pTile.is_checked_tile = true;
      pTile.score = num;
      World.world.flash_effects.flashPixel(pTile);
      if (pTile.hasBuilding() && pTile.building.city == pBuilding.city && pTile.building != pBuilding && pTile.building.current_tile == pTile)
        RoadGenerator.targetBuildings.Add(pTile.building);
      for (int index = 0; index < pTile.neighboursAll.Length; ++index)
      {
        WorldTile worldTile = pTile.neighboursAll[index];
        if (!worldTile.is_checked_tile)
        {
          worldTile.is_checked_tile = true;
          RoadGenerator.checkedTiles.Add(worldTile);
          if (!worldTile.Type.liquid && worldTile.Type.layer_type != TileLayerType.Block)
            RoadGenerator.nextWave.Add(worldTile);
        }
      }
    }
    for (int index = 0; index < RoadGenerator.targetBuildings.Count; ++index)
    {
      Building targetBuilding = RoadGenerator.targetBuildings[index];
      RoadGenerator.path.Clear();
      if (targetBuilding.asset.build_road_to)
      {
        RoadGenerator.findPath(targetBuilding.door_tile);
        RoadGenerator.fillPath();
      }
    }
    for (int index = 0; index < RoadGenerator.checkedTiles.Count; ++index)
    {
      WorldTile checkedTile = RoadGenerator.checkedTiles[index];
      checkedTile.is_checked_tile = false;
      checkedTile.score = -1;
    }
  }

  private static void fillPath()
  {
    for (int index = 0; index < RoadGenerator.path.Count; ++index)
      MapAction.createRoadTile(RoadGenerator.path[index]);
  }

  private static void findPath(WorldTile pTile)
  {
    RoadGenerator.path.Add(pTile);
    if (pTile.score <= 1)
      return;
    WorldTile pTile1 = (WorldTile) null;
    for (int index = 0; index < pTile.neighboursAll.Length; ++index)
    {
      WorldTile pTile2 = pTile.neighboursAll[index];
      if (pTile2.score == pTile.score - 1)
      {
        if (pTile1 == null)
          pTile1 = pTile2;
        else if (pTile2.Type.road)
        {
          RoadGenerator.findPath(pTile2);
          return;
        }
      }
    }
    RoadGenerator.findPath(pTile1);
  }
}
