// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionBurnedTiles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class WorldBehaviourActionBurnedTiles
{
  private static readonly HashSet<WorldTile> _burned_tiles = new HashSet<WorldTile>();
  private static readonly List<WorldTile> _tiles_to_remove = new List<WorldTile>();

  public static void addTile(WorldTile pTile)
  {
    WorldBehaviourActionBurnedTiles._burned_tiles.Add(pTile);
  }

  public static void clear()
  {
    WorldBehaviourActionBurnedTiles._burned_tiles.Clear();
    WorldBehaviourActionBurnedTiles._tiles_to_remove.Clear();
  }

  public static int countBurnedTiles() => WorldBehaviourActionBurnedTiles._burned_tiles.Count;

  public static void updateBurnedTiles()
  {
    if (WorldBehaviourActionBurnedTiles._burned_tiles.Count == 0)
      return;
    WorldBehaviourActionBurnedTiles._tiles_to_remove.Clear();
    foreach (WorldTile burnedTile in WorldBehaviourActionBurnedTiles._burned_tiles)
    {
      if (!burnedTile.isOnFire())
      {
        --burnedTile.burned_stages;
        World.world.burned_layer.setTileDirty(burnedTile);
        if (burnedTile.burned_stages <= 0)
        {
          burnedTile.burned_stages = 0;
          WorldBehaviourActionBurnedTiles._tiles_to_remove.Add(burnedTile);
        }
      }
    }
    foreach (WorldTile worldTile in WorldBehaviourActionBurnedTiles._tiles_to_remove)
      WorldBehaviourActionBurnedTiles._burned_tiles.Remove(worldTile);
  }
}
