// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionBiomes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class WorldBehaviourActionBiomes : WorldBehaviourTilesRunner
{
  private static List<WorldTile> _bonus_tiles = new List<WorldTile>();

  public static void clear()
  {
    WorldBehaviourTilesRunner.clearTilesToCheck();
    WorldBehaviourActionBiomes._bonus_tiles.Clear();
  }

  public static void update()
  {
    AchievementLibrary.the_hell.check();
    if (!WorldLawLibrary.world_law_grow_grass.isEnabled() || World.world.era_manager.isWinter())
      return;
    WorldBehaviourActionBiomes.updateSingleTiles();
  }

  public static void updateSingleTiles()
  {
    WorldBehaviourTilesRunner.checkTiles();
    int num = World.world.map_chunk_manager.amount_x * (SimGlobals.m.biomes_growth_speed + World.world_era.bonus_biomes_growth);
    if (num <= 0)
      return;
    WorldTile[] tilesToCheck = WorldBehaviourTilesRunner._tiles_to_check;
    if (WorldBehaviourTilesRunner._tile_next_check + num >= tilesToCheck.Length)
      num = tilesToCheck.Length - WorldBehaviourTilesRunner._tile_next_check;
    while (num-- > 0)
    {
      WorldBehaviourTilesRunner._tiles_to_check.ShuffleOne<WorldTile>(WorldBehaviourTilesRunner._tile_next_check);
      WorldTile worldTile = tilesToCheck[WorldBehaviourTilesRunner._tile_next_check++];
      if (worldTile.Type.is_biome && worldTile.burned_stages <= 0)
        WorldBehaviourActionBiomes.trySpreadBiomeAround(worldTile, worldTile, true, true);
    }
    WorldBehaviourActionBiomes.checkBonusTilesGrowth();
  }

  private static void checkBonusTilesGrowth()
  {
    if (WorldBehaviourActionBiomes._bonus_tiles.Count == 0)
      return;
    foreach (WorldTile bonusTile in WorldBehaviourActionBiomes._bonus_tiles)
      WorldBehaviourActionBiomes.trySpreadBiomeAround(bonusTile, bonusTile);
    WorldBehaviourActionBiomes._bonus_tiles.Clear();
  }

  public static void trySpreadBiomeAround(
    WorldTile pAroundTile,
    WorldTile pSpreader,
    bool pCheckRoad = false,
    bool pCheckBonuses = false,
    bool pForce = false)
  {
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pAroundTile, pSpreader.Type, pCheckRoad, pCheckBonuses, pForce);
  }

  public static void trySpreadBiomeAround(
    WorldTile pAroundTile,
    TileTypeBase pSpreadType,
    bool pCheckRoad = false,
    bool pCheckBonuses = false,
    bool pForce = false,
    bool pSkipEraCheck = false)
  {
    if (!pSpreadType.is_biome)
      return;
    BiomeAsset biomeAsset1 = pSpreadType.biome_asset;
    bool ignoreBurnedStages = biomeAsset1.spread_ignore_burned_stages;
    if (!biomeAsset1.spread_biome || !pSkipEraCheck && !string.IsNullOrEmpty(biomeAsset1.spread_only_in_era) && World.world_era.id != biomeAsset1.spread_only_in_era)
      return;
    for (int index = 0; index < pAroundTile.neighbours.Length; ++index)
    {
      WorldTile neighbour = pAroundTile.neighbours[index];
      if (ignoreBurnedStages || neighbour.burned_stages <= 0)
      {
        if (neighbour.Type.road & pCheckRoad)
          WorldBehaviourActionBiomes.trySpreadBiomeAround(neighbour, pSpreadType);
        if (neighbour.Type.can_be_biome)
        {
          BiomeAsset biomeAsset2 = neighbour.Type.biome_asset;
          if (biomeAsset2 != biomeAsset1)
          {
            bool flag = false;
            if (neighbour.Type.soil | pForce)
              flag = true;
            else if (WorldLawLibrary.world_law_biome_overgrowth.isEnabled())
            {
              if (biomeAsset2.grow_strength == biomeAsset1.grow_strength)
              {
                if (Randy.randomChance(0.05f))
                  flag = true;
              }
              else if (Randy.randomInt(0, biomeAsset1.grow_strength) > Randy.randomInt(0, biomeAsset2.grow_strength))
                flag = true;
            }
            if (flag)
            {
              MusicBox.playSound("event:/SFX/NATURE/GrassSpawn", neighbour, true, true);
              TopTileType tile = biomeAsset1.getTile(neighbour);
              MapAction.growGreens(neighbour, tile);
              if (pCheckBonuses)
              {
                HashSet<string> biomes = World.world_era.biomes;
                // ISSUE: explicit non-virtual call
                if ((biomes != null ? (__nonvirtual (biomes.Contains(neighbour.Type.biome_id)) ? 1 : 0) : 0) != 0)
                  WorldBehaviourActionBiomes._bonus_tiles.Add(neighbour);
              }
            }
          }
        }
      }
    }
  }
}
