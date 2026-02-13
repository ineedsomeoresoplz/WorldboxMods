// Decompiled with JetBrains decompiler
// Type: WorldBehaviourOcean
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class WorldBehaviourOcean
{
  private static List<WorldTile> tiles_to_update = new List<WorldTile>();
  public static HashSetWorldTile tiles = new HashSetWorldTile();

  public static void clear()
  {
    WorldBehaviourOcean.tiles_to_update.Clear();
    WorldBehaviourOcean.tiles.Clear();
  }

  public static void updateOcean()
  {
    if (WorldBehaviourOcean.tiles.Count == 0)
      return;
    WorldBehaviourOcean.tiles_to_update.Clear();
    foreach (WorldTile tile in (HashSet<WorldTile>) WorldBehaviourOcean.tiles)
    {
      if (tile.world_edge)
      {
        if ((double) Randy.randomInt(0, 100) >= 30.0)
          WorldBehaviourOcean.tiles_to_update.Add(tile);
      }
      else if (tile.IsOceanAround() && (double) Randy.randomInt(0, 100) >= 30.0)
        WorldBehaviourOcean.tiles_to_update.Add(tile);
    }
    for (int index = 0; index < WorldBehaviourOcean.tiles_to_update.Count; ++index)
    {
      WorldTile worldTile = WorldBehaviourOcean.tiles_to_update[index];
      if (worldTile.Type.can_be_filled_with_ocean)
      {
        if (worldTile.Type.explodable_by_ocean)
          World.world.explosion_layer.explodeBomb(worldTile);
        else
          MapAction.setOcean(worldTile);
      }
    }
  }
}
