// Decompiled with JetBrains decompiler
// Type: Finder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public static class Finder
{
  private static readonly List<BaseSimObject> _list_objects = new List<BaseSimObject>(4096 /*0x1000*/);
  private static MapChunk[] _chunks;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<Building> getBuildingsFromChunk(
    WorldTile pTile,
    int pChunkRadius,
    int pTileRadius = 0,
    bool pRandom = false)
  {
    int num1 = pTile.chunk.x - pChunkRadius;
    int num2 = pTile.chunk.y - pChunkRadius;
    int num3 = pChunkRadius * 2 + 1;
    int num4 = pChunkRadius * 2 + 1;
    int num5 = num3 * num4;
    MapChunk[] array = Toolbox.checkArraySize<MapChunk>(Finder._chunks, num5);
    Finder._chunks = array;
    MapChunkManager mapChunkManager = World.world.map_chunk_manager;
    int tTileRadius = pTileRadius * pTileRadius;
    int num6 = 0;
    for (int index1 = 0; index1 < num3; ++index1)
    {
      for (int index2 = 0; index2 < num4; ++index2)
      {
        MapChunk mapChunk = mapChunkManager.get(num1 + index1, num2 + index2);
        if (mapChunk == null)
          --num5;
        else
          array[num6++] = mapChunk;
      }
    }
    if (pRandom)
    {
      foreach (MapChunk mapChunk in array.LoopRandom<MapChunk>(num5))
      {
        if (mapChunk != null)
        {
          foreach (Building building in mapChunk.objects.buildings_all.LoopRandom<Building>())
          {
            if (building.isAlive() && (tTileRadius == 0 || Toolbox.SquaredDistTile(building.current_tile, pTile) <= tTileRadius))
              yield return building;
          }
        }
      }
    }
    else
    {
      foreach (MapChunk mapChunk in array.LoopRandom<MapChunk>(num5))
      {
        if (mapChunk != null)
        {
          List<Building> tBuildings = mapChunk.objects.buildings_all;
          int i = 0;
          for (int tLen = tBuildings.Count; i < tLen; ++i)
          {
            Building building = tBuildings[i];
            if (building.isAlive() && (tTileRadius == 0 || Toolbox.SquaredDistTile(building.current_tile, pTile) <= tTileRadius))
              yield return building;
          }
          tBuildings = (List<Building>) null;
        }
      }
    }
  }

  public static bool isEnemyNearOnSameIsland(Actor pActor, int pChunkRadius = 1)
  {
    foreach (Actor pActor1 in Finder.getUnitsFromChunk(pActor.current_tile, pChunkRadius))
    {
      if (pActor.isOnSameIsland(pActor1) && pActor1.kingdom.isEnemy(pActor.kingdom))
        return true;
    }
    return false;
  }

  public static bool isEnemyNearOnSameIslandAndCarnivore(Actor pActor, int pChunkRadius = 1)
  {
    foreach (Actor pActor1 in Finder.getUnitsFromChunk(pActor.current_tile, pChunkRadius))
    {
      if (pActor.isOnSameIsland(pActor1) && (pActor1.isCarnivore() || pActor1.kingdom.isEnemy(pActor.kingdom)))
        return true;
    }
    return false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<Actor> getUnitsFromChunk(
    WorldTile pTile,
    int pChunkRadius,
    float pTileRadius = 0.0f,
    bool pRandom = false)
  {
    int num1 = pTile.chunk.x - pChunkRadius;
    int num2 = pTile.chunk.y - pChunkRadius;
    int num3 = pChunkRadius * 2 + 1;
    int num4 = pChunkRadius * 2 + 1;
    int num5 = num3 * num4;
    MapChunk[] array = Toolbox.checkArraySize<MapChunk>(Finder._chunks, num5);
    Finder._chunks = array;
    MapChunkManager mapChunkManager = World.world.map_chunk_manager;
    float tTileRadius = pTileRadius * pTileRadius;
    int num6 = 0;
    for (int index1 = 0; index1 < num3; ++index1)
    {
      for (int index2 = 0; index2 < num4; ++index2)
      {
        MapChunk mapChunk = mapChunkManager.get(num1 + index1, num2 + index2);
        if (mapChunk == null)
          --num5;
        else
          array[num6++] = mapChunk;
      }
    }
    if (pRandom)
    {
      foreach (MapChunk mapChunk in array.LoopRandom<MapChunk>(num5))
      {
        if (mapChunk != null)
        {
          foreach (Actor actor in mapChunk.objects.units_all.LoopRandom<Actor>())
          {
            if (actor.isAlive() && ((double) tTileRadius == 0.0 || (double) Toolbox.SquaredDistTile(actor.current_tile, pTile) <= (double) tTileRadius))
              yield return actor;
          }
        }
      }
    }
    else
    {
      foreach (MapChunk mapChunk in array.LoopRandom<MapChunk>(num5))
      {
        if (mapChunk != null)
        {
          List<Actor> tUnits = mapChunk.objects.units_all;
          int i = 0;
          for (int tLen = tUnits.Count; i < tLen; ++i)
          {
            Actor actor = tUnits[i];
            if (actor.isAlive() && ((double) tTileRadius == 0.0 || (double) Toolbox.SquaredDistTile(actor.current_tile, pTile) <= (double) tTileRadius))
              yield return actor;
          }
          tUnits = (List<Actor>) null;
        }
      }
    }
  }

  public static List<BaseSimObject> getAllObjectsInChunks(WorldTile pTile, int pTileRadius = 3)
  {
    List<BaseSimObject> listObjects = Finder._list_objects;
    listObjects.Clear();
    Finder.fillAllObjectsFromChunk(pTile.chunk, pTile, pTileRadius, listObjects);
    foreach (MapChunk neighbour in pTile.chunk.neighbours)
      Finder.fillAllObjectsFromChunk(neighbour, pTile, pTileRadius, listObjects);
    return listObjects;
  }

  private static void fillAllObjectsFromChunk(
    MapChunk pChunk,
    WorldTile pTile,
    int pTileRadius,
    List<BaseSimObject> pListObjects)
  {
    int num = pTileRadius * pTileRadius;
    List<long> kingdoms = pChunk.objects.kingdoms;
    for (int index1 = 0; index1 < kingdoms.Count; ++index1)
    {
      long pKingdom = kingdoms[index1];
      List<Actor> units = pChunk.objects.getUnits(pKingdom);
      for (int index2 = 0; index2 < units.Count; ++index2)
      {
        BaseSimObject baseSimObject = (BaseSimObject) units[index2];
        if (baseSimObject.isAlive() && (pTileRadius == 0 || Toolbox.SquaredDistTile(baseSimObject.current_tile, pTile) <= num))
          pListObjects.Add(baseSimObject);
      }
      List<Building> buildings = pChunk.objects.getBuildings(pKingdom);
      for (int index3 = 0; index3 < buildings.Count; ++index3)
      {
        BaseSimObject baseSimObject = (BaseSimObject) buildings[index3];
        if (baseSimObject.isAlive() && (pTileRadius == 0 || Toolbox.SquaredDistTile(baseSimObject.current_tile, pTile) <= num))
          pListObjects.Add(baseSimObject);
      }
    }
  }

  internal static IEnumerable<Actor> findSpeciesAroundTileChunk(WorldTile pTile, string pUnitID)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1))
    {
      if (!(actor.a.asset.id != pUnitID))
        yield return actor;
    }
  }

  public static Building getClosestBuildingFrom(
    Actor pActor,
    IReadOnlyCollection<Building> pBuildingList)
  {
    return Finder.getClosestBuildingFrom(pActor.current_tile, pBuildingList);
  }

  public static Building getClosestBuildingFrom(
    WorldTile pTile,
    IReadOnlyCollection<Building> pBuildingList)
  {
    Building closestBuildingFrom = (Building) null;
    float num1 = float.MaxValue;
    foreach (Building pBuilding in (IEnumerable<Building>) pBuildingList)
    {
      if (!pBuilding.isRekt() && pBuilding.current_tile.isSameIsland(pTile))
      {
        float num2 = (float) Toolbox.SquaredDistTile(pBuilding.current_tile, pTile);
        if ((double) num2 < (double) num1)
        {
          closestBuildingFrom = pBuilding;
          num1 = num2;
        }
      }
    }
    return closestBuildingFrom;
  }

  public static void clear() => Finder._list_objects.Clear();

  public static WorldTile findTileInChunk(WorldTile pTile, TileFinderType pTileType)
  {
    (MapChunk[], int) allChunksFromTile = Toolbox.getAllChunksFromTile(pTile);
    foreach (MapChunk mapChunk in allChunksFromTile.Item1.LoopRandom<MapChunk>(allChunksFromTile.Item2))
    {
      foreach (MapRegion mapRegion in mapChunk.regions.LoopRandom<MapRegion>())
      {
        foreach (WorldTile tileInChunk in mapRegion.tiles.LoopRandom<WorldTile>())
        {
          switch (pTileType)
          {
            case TileFinderType.FreeTile:
              if (!tileInChunk.isSameIsland(pTile) || tileInChunk.hasBuilding() || !tileInChunk.Type.ground)
                continue;
              break;
            case TileFinderType.Sand:
              if (tileInChunk.Type.sand)
                break;
              continue;
            case TileFinderType.Water:
              if (tileInChunk.isTargeted() || !tileInChunk.Type.ocean)
                continue;
              break;
            case TileFinderType.Grass:
              if (!tileInChunk.isSameIsland(pTile) || tileInChunk.isTargeted() || !tileInChunk.Type.grass || tileInChunk.hasBuilding())
                continue;
              break;
            case TileFinderType.Biome:
              if (!tileInChunk.isSameIsland(pTile) || tileInChunk.isTargeted() || !tileInChunk.Type.is_biome || tileInChunk.hasBuilding())
                continue;
              break;
            case TileFinderType.Dirt:
              if (!tileInChunk.isSameIsland(pTile) || tileInChunk.isTargeted() || !tileInChunk.Type.can_be_farm || tileInChunk.hasBuilding())
                continue;
              break;
            default:
              if (!tileInChunk.isSameIsland(pTile))
                continue;
              break;
          }
          return tileInChunk;
        }
      }
    }
    return (WorldTile) null;
  }
}
