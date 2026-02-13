// Decompiled with JetBrains decompiler
// Type: WorldBehaviourTilesTemperatureFreeze
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class WorldBehaviourTilesTemperatureFreeze : WorldBehaviourTilesRunner
{
  private static float timer_freeze_summits = 0.0f;
  private static List<WorldTile> _summit_tiles = new List<WorldTile>();

  public static void update()
  {
    WorldBehaviourTilesTemperatureFreeze.freezeSummits();
    if (!World.world_era.global_freeze_world)
      return;
    WorldBehaviourTilesTemperatureFreeze.updateSingleTiles();
  }

  private static void freezeSummits()
  {
    if (World.world_era.overlay_sun)
      return;
    if ((double) WorldBehaviourTilesTemperatureFreeze.timer_freeze_summits > 0.0)
    {
      WorldBehaviourTilesTemperatureFreeze.timer_freeze_summits -= World.world.elapsed;
    }
    else
    {
      WorldBehaviourTilesTemperatureFreeze.timer_freeze_summits = Randy.randomFloat(10f, 60f);
      if (TileLibrary.summit.hashset.Count == 0)
        return;
      WorldBehaviourTilesTemperatureFreeze._summit_tiles.AddRange((IEnumerable<WorldTile>) TileLibrary.summit.hashset);
      foreach (WorldTile summitTile in WorldBehaviourTilesTemperatureFreeze._summit_tiles)
      {
        if (!Randy.randomChance(0.8f) && summitTile.canBeFrozen())
          summitTile.freeze(5);
      }
      WorldBehaviourTilesTemperatureFreeze._summit_tiles.Clear();
    }
  }

  public static void updateSingleTiles()
  {
    WorldBehaviourTilesRunner.checkTiles();
    WorldTile[] tilesToCheck = WorldBehaviourTilesRunner._tiles_to_check;
    int num = World.world.map_chunk_manager.amount_x * 10;
    if (WorldBehaviourTilesRunner._tile_next_check + num >= tilesToCheck.Length)
      num = tilesToCheck.Length - WorldBehaviourTilesRunner._tile_next_check;
    while (num-- > 0)
    {
      WorldBehaviourTilesRunner._tiles_to_check.ShuffleOne<WorldTile>(WorldBehaviourTilesRunner._tile_next_check);
      WorldTile worldTile = tilesToCheck[WorldBehaviourTilesRunner._tile_next_check++];
      if (worldTile.canBeFrozen())
        worldTile.freeze(5);
    }
  }
}
