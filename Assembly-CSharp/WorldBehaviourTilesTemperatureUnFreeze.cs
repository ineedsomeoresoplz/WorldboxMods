// Decompiled with JetBrains decompiler
// Type: WorldBehaviourTilesTemperatureUnFreeze
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldBehaviourTilesTemperatureUnFreeze : WorldBehaviourTilesRunner
{
  public static void update()
  {
    if (!World.world_era.global_unfreeze_world || WorldLawLibrary.world_law_forever_cold.isEnabled())
      return;
    WorldBehaviourTilesTemperatureUnFreeze.updateSingleTiles();
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
      WorldTile pTile = tilesToCheck[WorldBehaviourTilesRunner._tile_next_check++];
      if (pTile.isTemporaryFrozen() && (!pTile.Type.mountains || WorldBehaviourTilesTemperatureUnFreeze.checkMountainUnfreeze(pTile)))
        pTile.unfreeze(5 + World.world_era.temperature_damage_bonus);
    }
  }

  private static bool checkMountainUnfreeze(WorldTile pTile)
  {
    if (World.world_era.global_unfreeze_world_mountains)
      return true;
    if (pTile.Type.summit && !World.world_era.overlay_sun || Randy.randomChance(0.9f))
      return false;
    bool flag = false;
    for (int index = 0; index < pTile.neighboursAll.Length; ++index)
    {
      if (!pTile.neighboursAll[index].isFrozen())
      {
        flag = true;
        break;
      }
    }
    return flag;
  }
}
