// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerFindTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehFingerFindTarget : BehFinger
{
  private const float RANDOM_CHANCE_ADD_TILE = 0.65f;
  private const float RANDOM_CHANCE_FIND_PITS = 0.95f;
  private const float RANDOM_CHANCE_FIND_WATER = 0.95f;
  private const float RANDOM_CHANCE_FIND_GROUND = 0.95f;
  private const float RANDOM_CHANCE_USE_CURRENT_ISLAND = 0.6f;

  public override BehResult execute(Actor pActor)
  {
    pActor.findCurrentTile(false);
    BehFingerFindTarget.clearTargets(this.finger);
    if (this.finger.target_tiles.Count == 0)
      this.finger.finger_target = this.fillRandomTiles(pActor.current_tile, this.finger.target_tiles);
    pActor.beh_tile_target = this.finger.target_tiles.GetRandom<WorldTile>();
    return BehResult.Continue;
  }

  private FingerTarget fillRandomTiles(WorldTile pTile, HashSet<WorldTile> pTargetTiles)
  {
    float num1 = BehaviourActionBase<Actor>.world.islands_calculator.groundIslandRatio() * 4f;
    int capacity = TileLibrary.pit_deep_ocean.hashset.Count + TileLibrary.pit_close_ocean.hashset.Count + TileLibrary.pit_shallow_waters.hashset.Count;
    if (capacity > 20 && Randy.randomChance(0.95f))
    {
      using (ListPool<WorldTile> listPool = new ListPool<WorldTile>(capacity))
      {
        listPool.AddRange((IEnumerable<WorldTile>) TileLibrary.pit_deep_ocean.hashset);
        listPool.AddRange((IEnumerable<WorldTile>) TileLibrary.pit_close_ocean.hashset);
        listPool.AddRange((IEnumerable<WorldTile>) TileLibrary.pit_shallow_waters.hashset);
        Toolbox.sortTilesByDistance(pTile, listPool);
        listPool.Clear(10);
        WorldTile random = listPool.GetRandom<WorldTile>();
        (MapChunk[] mapChunkArray, int num2) = Toolbox.getAllChunksFromTile(random);
        bool flag = false;
        for (int index = 0; index < num2; ++index)
        {
          foreach (WorldTile tile in mapChunkArray[index].tiles)
          {
            if (tile.Type.IsType(random.Type) && Randy.randomChance(0.65f))
            {
              pTargetTiles.Add(tile);
              if (pTargetTiles.Count >= 1200)
              {
                flag = true;
                break;
              }
            }
          }
          if (flag)
            break;
        }
        return FingerTarget.Water;
      }
    }
    if (BehaviourActionBase<Actor>.world.islands_calculator.hasNonGround() && Randy.randomChance(0.95f * num1))
    {
      TileIsland tileIsland;
      if (pTile.region.island.type == TileLayerType.Ocean && Randy.randomChance(0.6f))
      {
        tileIsland = pTile.region.island;
      }
      else
      {
        tileIsland = BehaviourActionBase<Actor>.world.islands_calculator.getRandomIslandNonGroundWeighted() ?? BehaviourActionBase<Actor>.world.islands_calculator.getRandomIslandNonGround(false);
        pTile = tileIsland.getRandomTile();
      }
      foreach (MapRegion pRegion in tileIsland.regions.getSimpleList().LoopRandom<MapRegion>())
      {
        if (pTile.region == pRegion || pTile.region.hasNeighbour(pRegion))
        {
          foreach (WorldTile worldTile in pRegion.tiles.LoopRandom<WorldTile>())
          {
            if (Randy.randomChance(0.65f))
              pTargetTiles.Add(worldTile);
          }
          if (pTargetTiles.Count >= 1200)
            break;
        }
      }
      return FingerTarget.Water;
    }
    if (BehaviourActionBase<Actor>.world.islands_calculator.hasGround() && Randy.randomChance(0.95f))
    {
      TileIsland tileIsland;
      if (pTile.region.island.type == TileLayerType.Ground && Randy.randomChance(0.6f))
      {
        tileIsland = pTile.region.island;
      }
      else
      {
        tileIsland = BehaviourActionBase<Actor>.world.islands_calculator.getRandomIslandGroundWeighted() ?? BehaviourActionBase<Actor>.world.islands_calculator.getRandomIslandGround(false);
        pTile = tileIsland.getRandomTile();
      }
      foreach (MapRegion pRegion in tileIsland.regions.getSimpleList().LoopRandom<MapRegion>())
      {
        if (pTile.region == pRegion || pTile.region.hasNeighbour(pRegion))
        {
          foreach (WorldTile worldTile in pRegion.tiles.LoopRandom<WorldTile>())
          {
            if (Randy.randomChance(0.65f))
              pTargetTiles.Add(worldTile);
          }
          if (pTargetTiles.Count >= 1200)
            break;
        }
      }
      return FingerTarget.Ground;
    }
    WorldTile tileWithinDistance = Toolbox.getRandomTileWithinDistance(pTile, 75);
    foreach (MapRegion pRegion in tileWithinDistance.region.island.regions.getSimpleList().LoopRandom<MapRegion>())
    {
      if (tileWithinDistance.region == pRegion || tileWithinDistance.region.hasNeighbour(pRegion))
      {
        foreach (WorldTile worldTile in pRegion.tiles.LoopRandom<WorldTile>())
        {
          if (Randy.randomChance(0.65f))
            pTargetTiles.Add(worldTile);
        }
        if (pTargetTiles.Count >= 1200)
          break;
      }
    }
    return BehFingerFindTarget.getFingerTarget(tileWithinDistance);
  }

  private static FingerTarget getFingerTarget(WorldTile pTile)
  {
    return pTile.Type.layer_type == TileLayerType.Ocean || pTile.Type.can_be_filled_with_ocean ? FingerTarget.Water : FingerTarget.Ground;
  }

  private static void clearTargets(GodFinger pFinger)
  {
    if (pFinger.finger_target == FingerTarget.None)
      return;
    if (pFinger.drawing_over_water)
      pFinger.target_tiles.RemoveWhere((Predicate<WorldTile>) (x => x.Type.layer_type != TileLayerType.Ocean && !x.Type.can_be_filled_with_ocean));
    if (!pFinger.drawing_over_ground)
      return;
    pFinger.target_tiles.RemoveWhere((Predicate<WorldTile>) (x => x.Type.layer_type != TileLayerType.Ground));
  }
}
