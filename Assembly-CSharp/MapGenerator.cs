// Decompiled with JetBrains decompiler
// Type: MapGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class MapGenerator
{
  public static MapGenTemplate template;
  private static int _width = 0;
  private static int _height = 0;
  private static WorldTile[,] _tilesMap;
  private static List<GeneratedRoom> _rooms = new List<GeneratedRoom>();
  private static bool reported_tryMakeBiomeSteps = false;

  private static MapGenValues gen_values => MapGenerator.template.values;

  public static void clear()
  {
    MapGenerator._tilesMap = (WorldTile[,]) null;
    MapGenerator._rooms.Clear();
  }

  public static void prepare()
  {
    MapGenerator.template = AssetManager.map_gen_templates.get(Config.current_map_template);
    MapGenerator._width = MapBox.width;
    MapGenerator._height = MapBox.height;
    MapGenerator._tilesMap = World.world.tiles_map;
    MapGenerator.schedulePerlinNoiseMap();
    MapGenerator.scheduleUpdateTileTypes();
    if (MapGenerator.gen_values.forbidden_knowledge_start)
    {
      World.world.world_laws.enable("world_law_cursed_world");
      CursedSacrifice.loadAlreadyCursedState();
    }
    if (MapGenerator.gen_values.remove_mountains)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.removeMountains()), "Normalize Ground");
    if (MapGenerator.template.special_anthill)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.specialAnthill()), "Anthill");
    if (MapGenerator.template.special_checkerboard)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.specialCheckerBoard()), "Checkerboard");
    if (MapGenerator.template.special_cubicles)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.specialCubicles()), "Cubicles");
    MapGenerator.scheduleRandomShapes(Randy.randomBool());
    if (MapGenerator.template.perlin_replace.Count > 0)
    {
      foreach (PerlinReplaceContainer replaceContainer in MapGenerator.template.perlin_replace)
      {
        PerlinReplaceContainer tOption = replaceContainer;
        SmoothLoader.add((MapLoaderAction) (() => GeneratorTool.ApplyPerlinReplace(tOption)), "Perlin Replace");
      }
    }
    SmoothLoader.add((MapLoaderAction) (() => World.world.map_chunk_manager.allDirty()), "Map Chunk Manager (1/2)", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() => World.world.map_chunk_manager.update(0.0f, true)), "Map Chunk Manager (2/2)", pNewWaitTimerValue: 0.1f);
    if (MapGenerator.gen_values.random_biomes)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.generateBiomes()), "Add Random Biome");
    if (MapGenerator.gen_values.add_mountain_edges)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.addMountainEdges()), "Add Mountain Edges");
    if (MapGenerator.template.freeze_mountains)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.freezeMountainTops()), "Freeze Mountain Tops");
    if (MapGenerator.gen_values.add_vegetation)
    {
      int num = 12;
      int vegSpread = World.world.tiles_list.Length / 20;
      for (int index = 0; index < num; ++index)
        SmoothLoader.add((MapLoaderAction) (() =>
        {
          AssetManager.tiles.setListTo(DepthGeneratorType.Gameplay);
          MapGenerator.spawnVegetation(vegSpread);
        }), $"Add Vegetation ({(index + 1).ToString()}/{num.ToString()})");
    }
    if (!MapGenerator.gen_values.add_resources)
      return;
    SmoothLoader.add((MapLoaderAction) (() => MapGenerator.spawnResources()), "Add Resources");
  }

  private static void scheduleUpdateTileTypes()
  {
    int num1 = 4;
    int num2 = World.world.tiles_list.Length / num1;
    int num3 = 0;
    for (int index = 0; index < num1; ++index)
    {
      int tileAmount = Mathf.Min(World.world.tiles_list.Length - num3, num2);
      int startIndex = num3;
      num3 += tileAmount;
      SmoothLoader.add((MapLoaderAction) (() => GeneratorTool.UpdateTileTypes(true, startIndex, tileAmount)), $"Generate Tiles ({num3.ToString()}/{World.world.tiles_list.Length.ToString()})", true);
    }
  }

  private static void scheduleRandomShapes(bool pSubstract)
  {
    if (MapGenerator.gen_values.random_shapes_amount == 0)
      return;
    SmoothLoader.add((MapLoaderAction) (() => GeneratorTool.Init()), "Perlin Random Shapes (Init)");
    for (int index = 0; index < MapGenerator.gen_values.random_shapes_amount; ++index)
      SmoothLoader.add((MapLoaderAction) (() => GeneratorTool.ApplyRandomShape(pSubtract: pSubstract)), $"Perlin Random Shapes ({(index + 1).ToString()}/{MapGenerator.gen_values.random_shapes_amount.ToString()})");
  }

  private static void specialCubicles()
  {
    HashSet<MapChunk> pChunksUsed = new HashSet<MapChunk>();
    List<MapChunk> mapChunkList = new List<MapChunk>();
    foreach (MapChunk chunk in World.world.map_chunk_manager.chunks)
      mapChunkList.Add(chunk);
    MapGenerator._rooms.Clear();
    while (mapChunkList.Count > 0)
    {
      MapChunk random = mapChunkList.GetRandom<MapChunk>();
      MapGenerator.startCubicle(pChunksUsed, mapChunkList, random);
    }
    MapGenerator.createDoors();
  }

  private static void startCubicle(
    HashSet<MapChunk> pChunksUsed,
    List<MapChunk> pListLeft,
    MapChunk pStartRoomChunk)
  {
    MapChunk pChunk1 = pStartRoomChunk;
    List<MapChunk> mapChunkList1 = new List<MapChunk>();
    MapGenerator.rememberChunk(pChunk1, pChunksUsed, pListLeft, mapChunkList1);
    int pMinInclusive = 2;
    int pMaxExclusive = MapGenerator.gen_values.cubicle_size + 2;
    int num1 = Randy.randomInt(pMinInclusive, pMaxExclusive);
    for (int index = 0; index < num1 && pChunk1 != null; ++index)
    {
      pChunk1 = pChunk1.chunk_right;
      if (pChunk1 != null && !pChunksUsed.Contains(pChunk1))
        MapGenerator.rememberChunk(pChunk1, pChunksUsed, pListLeft, mapChunkList1);
    }
    MapChunk pChunk2 = pStartRoomChunk;
    int num2 = Randy.randomInt(pMinInclusive, pMaxExclusive);
    for (int index = 0; index < num2 && pChunk2 != null; ++index)
    {
      pChunk2 = pChunk2.chunk_left;
      if (pChunk2 != null && !pChunksUsed.Contains(pChunk2))
        MapGenerator.rememberChunk(pChunk2, pChunksUsed, pListLeft, mapChunkList1);
    }
    List<MapChunk> collection = new List<MapChunk>();
    collection.AddRange((IEnumerable<MapChunk>) mapChunkList1);
    List<MapChunk> mapChunkList2 = new List<MapChunk>();
    mapChunkList2.AddRange((IEnumerable<MapChunk>) collection);
    int num3 = Randy.randomInt(pMinInclusive, pMaxExclusive);
    for (int index = 0; index < num3; ++index)
    {
      List<MapChunk> mapChunkList3 = new List<MapChunk>();
      bool flag = true;
      foreach (MapChunk mapChunk in mapChunkList2)
      {
        if (mapChunk.chunk_down == null)
        {
          flag = false;
          index = num3;
          break;
        }
        if (pChunksUsed.Contains(mapChunk.chunk_down))
        {
          flag = false;
          index = num3;
          break;
        }
        mapChunkList3.Add(mapChunk);
      }
      if (flag)
      {
        mapChunkList2.Clear();
        foreach (MapChunk mapChunk in mapChunkList3)
        {
          MapGenerator.rememberChunk(mapChunk.chunk_down, pChunksUsed, pListLeft, mapChunkList1);
          mapChunkList2.Add(mapChunk.chunk_down);
        }
      }
    }
    mapChunkList2.Clear();
    mapChunkList2.AddRange((IEnumerable<MapChunk>) collection);
    int num4 = Randy.randomInt(pMinInclusive, pMaxExclusive);
    for (int index = 0; index < num4; ++index)
    {
      List<MapChunk> mapChunkList4 = new List<MapChunk>();
      bool flag = true;
      foreach (MapChunk mapChunk in mapChunkList2)
      {
        if (mapChunk.chunk_up == null)
        {
          flag = false;
          index = num4;
          break;
        }
        if (pChunksUsed.Contains(mapChunk.chunk_up))
        {
          flag = false;
          index = num4;
          break;
        }
        mapChunkList4.Add(mapChunk);
      }
      if (flag)
      {
        mapChunkList2.Clear();
        foreach (MapChunk mapChunk in mapChunkList4)
        {
          MapGenerator.rememberChunk(mapChunk.chunk_up, pChunksUsed, pListLeft, mapChunkList1);
          mapChunkList2.Add(mapChunk.chunk_up);
        }
      }
    }
    MapGenerator.finishRoom(mapChunkList1);
  }

  private static void rememberChunk(
    MapChunk pChunk,
    HashSet<MapChunk> pChunksUsed,
    List<MapChunk> pListLeft,
    List<MapChunk> pNewRoom)
  {
    pChunksUsed.Add(pChunk);
    pListLeft.Remove(pChunk);
    pNewRoom.Add(pChunk);
  }

  private static void finishRoom(List<MapChunk> pChunks)
  {
    BiomeAsset random = BiomeLibrary.pool_biomes.GetRandom<BiomeAsset>();
    TileType pNewTypeMain = TileLibrary.soil_high;
    if (Randy.randomBool())
      pNewTypeMain = TileLibrary.soil_low;
    WorldTile tileSimple1 = World.world.GetTileSimple(0, 0);
    WorldTile tileSimple2 = World.world.GetTileSimple(MapBox.width - 1, 0);
    WorldTile tileSimple3 = World.world.GetTileSimple(0, MapBox.height - 1);
    WorldTile tileSimple4 = World.world.GetTileSimple(MapBox.width - 1, MapBox.height - 1);
    WorldTile pTile1 = (WorldTile) null;
    WorldTile pTile2 = (WorldTile) null;
    WorldTile worldTile1 = (WorldTile) null;
    WorldTile worldTile2 = (WorldTile) null;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 0.0f;
    for (int index1 = 0; index1 < pChunks.Count; ++index1)
    {
      WorldTile[] tiles = pChunks[index1].tiles;
      int length = tiles.Length;
      for (int index2 = 0; index2 < length; ++index2)
      {
        WorldTile worldTile3 = tiles[index2];
        MapAction.terraformTile(worldTile3, pNewTypeMain, (TopTileType) null);
        if (MapGenerator.gen_values.random_biomes)
          DropsLibrary.useSeedOn(worldTile3, random.getTileLow(), random.getTileHigh());
        float num5 = Toolbox.DistTile(tileSimple1, worldTile3);
        float num6 = Toolbox.DistTile(tileSimple2, worldTile3);
        float num7 = Toolbox.DistTile(tileSimple3, worldTile3);
        float num8 = Toolbox.DistTile(tileSimple4, worldTile3);
        if (pTile1 == null || (double) num5 < (double) num1)
        {
          pTile1 = worldTile3;
          num1 = num5;
        }
        if (pTile2 == null || (double) num6 < (double) num2)
        {
          pTile2 = worldTile3;
          num2 = num6;
        }
        if (worldTile1 == null || (double) num7 < (double) num3)
        {
          worldTile1 = worldTile3;
          num3 = num7;
        }
        if (worldTile2 == null || (double) num8 < (double) num4)
        {
          worldTile2 = worldTile3;
          num4 = num8;
        }
      }
    }
    GeneratedRoom generatedRoom = new GeneratedRoom();
    generatedRoom.id_debug = MapGenerator._rooms.Count;
    MapGenerator._rooms.Add(generatedRoom);
    generatedRoom.edges_up = MapGenerator.fillTiles(pTile1, pTile2, TileLibrary.mountains);
    generatedRoom.edges_down = MapGenerator.fillTiles(worldTile1, worldTile2, TileLibrary.mountains);
    generatedRoom.edges_left = MapGenerator.fillTiles(pTile1, worldTile1, TileLibrary.mountains);
    generatedRoom.edges_right = MapGenerator.fillTiles(worldTile2, pTile2, TileLibrary.mountains);
  }

  private static void createDoors()
  {
    foreach (GeneratedRoom room in MapGenerator._rooms)
    {
      MapGenerator.makeDoor(room.edges_down);
      MapGenerator.makeDoor(room.edges_left);
      MapGenerator.makeDoor(room.edges_right);
      MapGenerator.makeDoor(room.edges_up);
    }
  }

  private static void makeDoor(List<WorldTile> pTiles)
  {
    foreach (WorldTile pTile in pTiles)
    {
      if (pTile.main_type != TileLibrary.mountains)
        return;
    }
    WorldTile pTile1;
    if (pTiles.Count > 3)
    {
      int index = Randy.randomInt(3, pTiles.Count - 3);
      pTile1 = pTiles[index];
    }
    else
    {
      int index = pTiles.Count / 2;
      pTile1 = pTiles[index];
    }
    MapAction.terraformTile(pTile1, TileLibrary.hills, (TopTileType) null);
    foreach (WorldTile pTile2 in pTile1.neighboursAll)
    {
      if (pTile2.main_type == TileLibrary.mountains)
        MapAction.terraformTile(pTile2, TileLibrary.hills, (TopTileType) null);
    }
  }

  private static void specialCheckerBoard()
  {
    BiomeAsset random1 = BiomeLibrary.pool_biomes.GetRandom<BiomeAsset>();
    BiomeAsset random2 = BiomeLibrary.pool_biomes.GetRandom<BiomeAsset>();
    foreach (MapChunk chunk in World.world.map_chunk_manager.chunks)
    {
      WorldTile[] tiles = chunk.tiles;
      if ((chunk.x + chunk.y) % 2 == 0)
      {
        int length = tiles.Length;
        for (int index = 0; index < length; ++index)
        {
          WorldTile pTile = tiles[index];
          MapAction.terraformTile(pTile, TileLibrary.soil_high, (TopTileType) null);
          if (MapGenerator.gen_values.random_biomes)
            DropsLibrary.useSeedOn(pTile, random1.getTileLow(), random1.getTileHigh());
        }
      }
      else
      {
        int length = tiles.Length;
        for (int index = 0; index < length; ++index)
        {
          WorldTile pTile = tiles[index];
          MapAction.terraformTile(pTile, TileLibrary.soil_low, (TopTileType) null);
          if (MapGenerator.gen_values.random_biomes)
            DropsLibrary.useSeedOn(pTile, random2.getTileLow(), random2.getTileHigh());
        }
      }
    }
  }

  private static void specialAnthill()
  {
    foreach (WorldTile tiles in World.world.tiles_list)
      MapAction.terraformTile(tiles, TileLibrary.mountains, (TopTileType) null);
    List<TileZone> pZones = new List<TileZone>();
    List<WorldTile> pTunnels = new List<WorldTile>();
    ZoneCalculator zoneCalculator = World.world.zone_calculator;
    int index1 = zoneCalculator.zones_total_x / 10 + 1;
    int index2 = zoneCalculator.zones_total_y / 10 + 1;
    TileZone tileZone1 = zoneCalculator.map[index1, index2];
    TileZone tileZone2 = zoneCalculator.map[zoneCalculator.zones_total_x - index1, index2];
    TileZone tileZone3 = zoneCalculator.map[zoneCalculator.zones_total_x - index1, zoneCalculator.zones_total_y - index2];
    TileZone tileZone4 = zoneCalculator.map[index1, zoneCalculator.zones_total_y - index2];
    MapGenerator.makeJailRoom(pZones, zoneCalculator.map[zoneCalculator.zones_total_x / 2, zoneCalculator.zones_total_y / 2]);
    MapGenerator.makeJailRoom(pZones, zoneCalculator.map[zoneCalculator.zones_total_x / 2, zoneCalculator.zones_total_y / 2]);
    MapGenerator.makeJailRoom(pZones, tileZone1);
    MapGenerator.makeJailRoom(pZones, tileZone1);
    MapGenerator.makeJailRoom(pZones, tileZone2);
    MapGenerator.makeJailRoom(pZones, tileZone2);
    MapGenerator.makeJailRoom(pZones, tileZone3);
    MapGenerator.makeJailRoom(pZones, tileZone3);
    MapGenerator.makeJailRoom(pZones, tileZone4);
    MapGenerator.makeJailRoom(pZones, tileZone4);
    MapGenerator.makeWay(tileZone1, tileZone2, pTunnels);
    MapGenerator.makeWay(tileZone4, tileZone3, pTunnels);
    MapGenerator.makeWay(tileZone1, tileZone4, pTunnels);
    MapGenerator.makeWay(tileZone3, tileZone2, pTunnels);
    MapGenerator.makeWay(tileZone3, tileZone1, pTunnels);
    foreach (TileZone pZone in pZones)
      MapGenerator.carveZone(pZone);
    foreach (WorldTile pTile in pTunnels)
      MapGenerator.carveTunnel(pTile);
  }

  private static List<WorldTile> fillTiles(WorldTile pTile1, WorldTile pTile2, TileType pType)
  {
    List<WorldTile> collection = PathfinderTools.raycast(pTile1, pTile2, 1f);
    List<WorldTile> worldTileList = new List<WorldTile>((IEnumerable<WorldTile>) collection);
    collection.Clear();
    foreach (WorldTile pTile in worldTileList)
      MapAction.terraformTile(pTile, pType, (TopTileType) null);
    return worldTileList;
  }

  private static void makeWay(TileZone tZone1, TileZone tZone2, List<WorldTile> pTunnels)
  {
    List<WorldTile> worldTileList = PathfinderTools.raycast(tZone1.centerTile, tZone2.centerTile);
    foreach (WorldTile worldTile in worldTileList)
    {
      foreach (WorldTile pTile in worldTile.neighboursAll)
      {
        MapAction.terraformTile(pTile, TileLibrary.soil_high, (TopTileType) null);
        pTunnels.Add(pTile);
      }
    }
    worldTileList.Clear();
  }

  private static void carveTunnel(WorldTile pTile)
  {
    for (int index1 = 0; index1 < 10; ++index1)
    {
      WorldTile pTile1 = pTile.neighbours.GetRandom<WorldTile>();
      for (int index2 = 10; index2 > 0; --index2)
      {
        WorldTile random = pTile1.neighbours.GetRandom<WorldTile>();
        if (random.Type.rocks)
        {
          MapAction.terraformTile(pTile1, TileLibrary.soil_high, (TopTileType) null);
          pTile1 = random;
        }
      }
    }
  }

  private static void carveZone(TileZone pZone)
  {
    for (int index1 = 0; index1 < 20; ++index1)
    {
      WorldTile pTile = pZone.tiles.GetRandom<WorldTile>();
      for (int index2 = 15; index2 > 0; --index2)
      {
        WorldTile random = pTile.neighbours.GetRandom<WorldTile>();
        if (random.Type.rocks)
        {
          MapAction.terraformTile(pTile, TileLibrary.soil_high, (TopTileType) null);
          pTile = random;
        }
      }
    }
  }

  private static void makeJailRoom(List<TileZone> pZones, TileZone pStartZone)
  {
    int num = World.world.zone_calculator.zones.Count / 10;
    TileZone tileZone = pStartZone;
    if (tileZone.world_edge)
      return;
    for (int index1 = 0; index1 < num; ++index1)
    {
      if (tileZone.world_edge)
      {
        tileZone = tileZone.neighbours.GetRandom<TileZone>();
      }
      else
      {
        WorldTile[] tiles = tileZone.tiles;
        int length = tiles.Length;
        for (int index2 = 0; index2 < length; ++index2)
          MapAction.terraformTile(tiles[index2], TileLibrary.soil_high, (TopTileType) null);
        pZones.Add(tileZone);
        tileZone = tileZone.neighbours.GetRandom<TileZone>();
      }
    }
  }

  private static void schedulePerlinNoiseMap()
  {
    MapGenerator.scheduleRandomShapes(true);
    if (MapGenerator.gen_values.main_perlin_noise_stage && MapGenerator.gen_values.perlin_scale_stage_1 > 0)
      SmoothLoader.add((MapLoaderAction) (() =>
      {
        int pPosX = Randy.randomInt(0, 1000000);
        int pPosY = Randy.randomInt(0, 1000000);
        GeneratorTool.ApplyPerlinNoise(MapGenerator._tilesMap, MapGenerator._width, MapGenerator._height, (float) pPosX, (float) pPosY, 1f, 1f * (float) MapGenerator.gen_values.perlin_scale_stage_1);
      }), "Perlin Noise", true);
    if (MapGenerator.template.force_height_to > 0)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.forceHeight()), "Add Height");
    if (MapGenerator.gen_values.add_center_gradient_land)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.addCenterGradient()), "Center Gradient");
    if (MapGenerator.gen_values.center_gradient_mountains)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.addCenterMountains()), "Center Mountains");
    if (MapGenerator.gen_values.add_center_lake)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.addCenterLake()), "Center Lake");
    if (MapGenerator.gen_values.perlin_noise_stage_2 && MapGenerator.gen_values.perlin_scale_stage_2 > 0)
      SmoothLoader.add((MapLoaderAction) (() =>
      {
        float perlinScaleStage2 = (float) MapGenerator.gen_values.perlin_scale_stage_2;
        int pPosX = Randy.randomInt(0, 1000000);
        int pPosY = Randy.randomInt(0, 1000000);
        GeneratorTool.ApplyPerlinNoise(MapGenerator._tilesMap, MapGenerator._width, MapGenerator._height, (float) pPosX, (float) pPosY, 0.2f, 4f * perlinScaleStage2, true);
      }), "Perlin Noise (1)");
    if (MapGenerator.gen_values.perlin_noise_stage_3 && MapGenerator.gen_values.perlin_scale_stage_3 > 0)
      SmoothLoader.add((MapLoaderAction) (() =>
      {
        float perlinScaleStage3 = (float) MapGenerator.gen_values.perlin_scale_stage_3;
        int pPosX = Randy.randomInt(0, 1000000);
        int pPosY = Randy.randomInt(0, 1000000);
        GeneratorTool.ApplyPerlinNoise(MapGenerator._tilesMap, MapGenerator._width, MapGenerator._height, (float) pPosX, (float) pPosY, 0.1f, perlinScaleStage3 * 10f, true);
      }), "Perlin Noise (2)");
    if (MapGenerator.gen_values.low_ground)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.lowGround()), "Lower Ground");
    if (MapGenerator.gen_values.high_ground)
      SmoothLoader.add((MapLoaderAction) (() => MapGenerator.highGround()), "High Ground");
    MapGenerator.scheduleRandomShapes(true);
    if (MapGenerator.gen_values.ring_effect)
      SmoothLoader.add((MapLoaderAction) (() => GeneratorTool.ApplyRingEffect()), "Perlin Ring", true);
    if (MapGenerator.gen_values.gradient_round_edges)
      SmoothLoader.add((MapLoaderAction) (() => MapEdges.AddEdgeGradientCircle(World.world.tiles_map, "height")), "Gradient Circle Edges", true);
    if (!MapGenerator.gen_values.square_edges)
      return;
    SmoothLoader.add((MapLoaderAction) (() => MapEdges.AddEdgeSquare(World.world.tiles_map, "height")), "Gradient Circle Edges", true);
  }

  private static void forceHeight()
  {
    foreach (WorldTile tiles in World.world.tiles_list)
      tiles.Height = MapGenerator.template.force_height_to;
  }

  private static void removeMountains()
  {
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.Type.rocks)
        MapAction.decreaseTile(tiles, false);
      if (tiles.Type.rocks)
        MapAction.decreaseTile(tiles, false);
    }
  }

  private static void lowGround()
  {
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.Height > 150)
        tiles.Height -= 50;
      if (tiles.Height > 130)
        tiles.Height -= 20;
    }
  }

  private static void highGround()
  {
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.Height < 20)
        tiles.Height += 80 /*0x50*/;
      else if (tiles.Height < 120)
        tiles.Height += 40;
    }
  }

  private static void addCenterGradient()
  {
    WorldTile tiles1 = World.world.tiles_map[MapBox.width / 2, MapBox.height / 2];
    float num1 = 0.9f;
    float num2 = 0.6f;
    float num3 = (float) (MapBox.width / 2) * num1;
    float num4 = (float) (MapBox.width / 2) * num2;
    float num5 = num3 - num4;
    foreach (WorldTile tiles2 in World.world.tiles_list)
    {
      float num6 = Toolbox.DistTile(tiles2, tiles1);
      if ((double) num6 <= (double) num3)
      {
        int num7 = (int) (45.0 * (double) ((num3 - num6) / num5));
        tiles2.Height += num7;
      }
    }
  }

  private static void addCenterLake()
  {
    WorldTile tiles1 = World.world.tiles_map[MapBox.width / 2, MapBox.height / 2];
    float num1 = 0.6f;
    float num2 = 0.2f;
    float num3 = (float) (MapBox.width / 2) * num1;
    float num4 = (float) (MapBox.width / 2) * num2;
    float num5 = num3 - num4;
    foreach (WorldTile tiles2 in World.world.tiles_list)
    {
      float num6 = Toolbox.DistTile(tiles2, tiles1);
      if ((double) num6 <= (double) num3)
      {
        float num7 = (float) (1.0 - (double) (num3 - num6) / (double) num5);
        int num8 = (int) ((double) tiles2.Height * (double) num7);
        tiles2.Height = num8;
      }
    }
  }

  private static void addCenterMountains()
  {
    WorldTile tiles1 = World.world.tiles_map[MapBox.width / 2, MapBox.height / 2];
    float num1 = 0.3f;
    float num2 = 0.0f;
    float num3 = (float) (MapBox.width / 2) * num1;
    float num4 = (float) (MapBox.width / 2) * num2;
    float num5 = num3 - num4;
    foreach (WorldTile tiles2 in World.world.tiles_list)
    {
      float num6 = Toolbox.DistTile(tiles2, tiles1);
      tiles2.Height -= (int) (75.0 * (double) ((num3 - num6) / num5));
    }
  }

  private static void generateBiomes()
  {
    HashSetWorldTile hashSetWorldTile = new HashSetWorldTile();
    for (int index = 0; index < World.world.tiles_list.Length; ++index)
    {
      WorldTile tiles = World.world.tiles_list[index];
      if (tiles.Type.soil)
        hashSetWorldTile.Add(tiles);
    }
    using (ListPool<WorldTile> listPool = new ListPool<WorldTile>(hashSetWorldTile.Count))
    {
      bool flag = true;
      while (hashSetWorldTile.Count > 0)
      {
        if (flag)
        {
          flag = false;
          MapGenerator.recreateSoilList(hashSetWorldTile, listPool);
        }
        WorldTile pStartTile = listPool.Last<WorldTile>();
        BiomeAsset random = BiomeLibrary.pool_biomes.GetRandom<BiomeAsset>();
        int pMaxSteps = MapGenerator.tryMakeBiomeSteps(pStartTile, random);
        if (pMaxSteps == 0)
        {
          listPool.Pop<WorldTile>();
        }
        else
        {
          MapGenerator.tryMakeBiome(pStartTile, hashSetWorldTile, pMaxSteps, random);
          flag = true;
        }
      }
    }
  }

  private static void recreateSoilList(
    HashSetWorldTile pHashSet,
    ListPool<WorldTile> pTempSoilTiles)
  {
    pTempSoilTiles.Clear();
    pTempSoilTiles.AddRange((IEnumerable<WorldTile>) pHashSet);
    pTempSoilTiles.Shuffle<WorldTile>();
  }

  private static int tryMakeBiomeSteps(WorldTile pStartTile, BiomeAsset pBiome)
  {
    if (!MapGenerator.reported_tryMakeBiomeSteps)
    {
      if (pBiome == null)
      {
        Debug.Log((object) "pBiome is null");
        MapGenerator.reported_tryMakeBiomeSteps = true;
      }
      if (pStartTile == null)
      {
        Debug.Log((object) "pStartTile is null");
        MapGenerator.reported_tryMakeBiomeSteps = true;
      }
      if (pStartTile.region == null)
      {
        Debug.Log((object) "pStartTile.region is null");
        MapGenerator.reported_tryMakeBiomeSteps = true;
      }
      if (pStartTile.region.island == null)
      {
        Debug.Log((object) "pStartTile.region.island is null");
        MapGenerator.reported_tryMakeBiomeSteps = true;
      }
    }
    int tileCount = pStartTile.region.island.getTileCount();
    int num = tileCount >= 400 ? (tileCount >= 600 ? tileCount / 3 : tileCount / 2) : tileCount;
    if (num > pBiome.generator_max_size && pBiome.generator_max_size != 0)
      num = pBiome.generator_max_size;
    return num;
  }

  private static void tryMakeBiome(
    WorldTile pStartTile,
    HashSetWorldTile pSoilTiles,
    int pMaxSteps,
    BiomeAsset pBiome)
  {
    int num = 0;
    using (ListPool<WorldTile> list = new ListPool<WorldTile>(pMaxSteps))
    {
      HashSetWorldTile pTiles = new HashSetWorldTile();
      list.Add(pStartTile);
      pTiles.Add(pStartTile);
      while (list.Count > 0 && num < pMaxSteps)
      {
        list.ShuffleLast<WorldTile>();
        WorldTile worldTile1 = list.Pop<WorldTile>();
        if (worldTile1.isTileRank(TileRank.Low))
          worldTile1.setTopTileType(pBiome.getTileLow());
        else
          worldTile1.setTopTileType(pBiome.getTileHigh());
        pSoilTiles.Remove(worldTile1);
        ++num;
        for (int index = 0; index < worldTile1.neighboursAll.Length; ++index)
        {
          WorldTile worldTile2 = worldTile1.neighboursAll[index];
          if (!pTiles.Contains(worldTile2) && worldTile2.Type.soil)
          {
            list.Add(worldTile2);
            pTiles.Add(worldTile2);
          }
        }
      }
      if (num > 10)
        return;
      MapGenerator.removeSmallBiomePatches(pTiles, pStartTile);
    }
  }

  private static void removeSmallBiomePatches(HashSetWorldTile pTiles, WorldTile pStartTile)
  {
    WorldTile worldTile1 = (WorldTile) null;
    foreach (WorldTile worldTile2 in pStartTile.neighboursAll)
    {
      if (worldTile2.Type.is_biome && !worldTile2.Type.biome_asset.special_biome && worldTile2.Type.biome_asset != pStartTile.Type.biome_asset)
      {
        worldTile1 = worldTile2;
        break;
      }
    }
    if (worldTile1 == null)
      return;
    BiomeAsset biomeAsset = worldTile1.top_type.biome_asset;
    foreach (WorldTile pTile in (HashSet<WorldTile>) pTiles)
    {
      TopTileType tile = biomeAsset.getTile(pTile);
      pTile.setTopTileType(tile);
    }
  }

  private static void addMountainEdges()
  {
    int pX = 0;
    int pY = 0;
    WorldTile tileSimple1 = World.world.GetTileSimple(pX, pY);
    WorldTile tileSimple2 = World.world.GetTileSimple(MapBox.width - pX - 1, pY);
    WorldTile tileSimple3 = World.world.GetTileSimple(MapBox.width - pX - 1, MapBox.height - pY - 1);
    WorldTile tileSimple4 = World.world.GetTileSimple(pX, MapBox.height - pY - 1);
    MapGenerator.fillTiles(tileSimple1, tileSimple2, TileLibrary.mountains);
    MapGenerator.fillTiles(tileSimple4, tileSimple3, TileLibrary.mountains);
    MapGenerator.fillTiles(tileSimple1, tileSimple4, TileLibrary.mountains);
    MapGenerator.fillTiles(tileSimple3, tileSimple2, TileLibrary.mountains);
  }

  private static void freezeMountainTops()
  {
    for (int index = 0; index < World.world.tiles_list.Length; ++index)
    {
      WorldTile tiles = World.world.tiles_list[index];
      if (tiles.Type.IsType("mountains") && tiles.Height > 220)
        tiles.freeze();
    }
  }

  private static void spawnVegetation(int pAmount)
  {
    for (int index = 0; index < pAmount; ++index)
    {
      WorldTile random = World.world.tiles_list.GetRandom<WorldTile>();
      if (random.Type.ground && random.zone.countBuildingsType(BuildingList.Trees) < 3)
      {
        BiomeAsset biomeAsset = random.Type.biome_asset;
        if (biomeAsset != null && biomeAsset.grow_vegetation_auto)
        {
          switch (Randy.randomInt(0, 3))
          {
            case 0:
              BuildingActions.tryGrowVegetationRandom(random, VegetationType.Plants, true);
              continue;
            case 1:
              BuildingActions.tryGrowVegetationRandom(random, VegetationType.Trees, true);
              continue;
            case 2:
              BuildingActions.tryGrowVegetationRandom(random, VegetationType.Bushes, true);
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  private static void spawnResources()
  {
    BuildingActions.spawnResource(World.world.tiles_list.Length / 1000 / 2 / 2, "fruit_bush", false);
  }
}
