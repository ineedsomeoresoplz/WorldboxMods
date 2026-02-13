// Decompiled with JetBrains decompiler
// Type: WorldBehaviourTilesTemperatureFreezeWaves
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class WorldBehaviourTilesTemperatureFreezeWaves
{
  private static List<WorldTile> _nextFreezeWave = new List<WorldTile>();
  private static List<WorldTile> _currentWave = new List<WorldTile>();
  private static int _waveNumber = 0;
  private const int MAX_WAVES = 60;
  private const int MAX_TILES_PER_WAVE = 20;

  public static void clear()
  {
    WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave.Clear();
    WorldBehaviourTilesTemperatureFreezeWaves._currentWave.Clear();
    WorldBehaviourTilesTemperatureFreezeWaves._waveNumber = 0;
  }

  public static void update()
  {
    if (!World.world_era.global_freeze_world)
      return;
    WorldBehaviourTilesTemperatureFreezeWaves.updateTileFreezeWaves();
  }

  public static void updateTileFreezeWaves()
  {
    if (WorldBehaviourTilesTemperatureFreezeWaves._waveNumber == 60)
    {
      WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave.Clear();
      for (int index = 0; index < WorldBehaviourTilesTemperatureFreezeWaves._currentWave.Count; ++index)
      {
        WorldTile worldTile = WorldBehaviourTilesTemperatureFreezeWaves._currentWave[index];
        if (worldTile.canBeFrozen() && worldTile.heat <= 0)
        {
          WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave.Add(worldTile);
          if (WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave.Count > 20)
            break;
        }
      }
      WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave.Shuffle<WorldTile>();
      WorldBehaviourTilesTemperatureFreezeWaves._waveNumber = 0;
    }
    WorldBehaviourTilesTemperatureFreezeWaves._currentWave.Clear();
    if (WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave.Count == 0)
    {
      int num1 = 3 + Randy.randomInt(0, 3);
      while (num1-- > 0)
      {
        MapChunk random = World.world.map_chunk_manager.chunks.GetRandom<MapChunk>();
        int num2 = 0;
        foreach (WorldTile worldTile in random.tiles.LoopRandom<WorldTile>())
        {
          if (worldTile.canBeFrozen() && worldTile.heat <= 0)
          {
            WorldBehaviourTilesTemperatureFreezeWaves._currentWave.Add(worldTile);
            ++num2;
            if (num2 > 5)
              break;
          }
        }
      }
    }
    else
    {
      WorldBehaviourTilesTemperatureFreezeWaves._currentWave = WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave;
      WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave = new List<WorldTile>();
    }
    for (int index = 0; index < WorldBehaviourTilesTemperatureFreezeWaves._currentWave.Count; ++index)
    {
      WorldTile worldTile = WorldBehaviourTilesTemperatureFreezeWaves._currentWave[index];
      if (worldTile.canBeFrozen() && (WorldBehaviourTilesTemperatureFreezeWaves._waveNumber <= 3 || !Randy.randomChance(0.7f)) && worldTile.freeze(5))
        WorldBehaviourTilesTemperatureFreezeWaves._nextFreezeWave.AddRange((IEnumerable<WorldTile>) worldTile.neighboursAll);
    }
    ++WorldBehaviourTilesTemperatureFreezeWaves._waveNumber;
  }
}
