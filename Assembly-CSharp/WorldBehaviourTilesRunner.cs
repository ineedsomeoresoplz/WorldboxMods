// Decompiled with JetBrains decompiler
// Type: WorldBehaviourTilesRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class WorldBehaviourTilesRunner
{
  internal static WorldTile[] _tiles_to_check;
  internal static int _tile_next_check;

  public static void clearTilesToCheck()
  {
    WorldBehaviourTilesRunner._tiles_to_check = (WorldTile[]) null;
    WorldBehaviourTilesRunner._tile_next_check = 0;
  }

  public static void checkTiles()
  {
    if (WorldBehaviourTilesRunner._tiles_to_check != null && WorldBehaviourTilesRunner._tile_next_check < WorldBehaviourTilesRunner._tiles_to_check.Length - 1)
      return;
    if (WorldBehaviourTilesRunner._tiles_to_check == null)
    {
      WorldBehaviourTilesRunner._tiles_to_check = new WorldTile[World.world.tiles_list.Length];
      Array.Copy((Array) World.world.tiles_list, (Array) WorldBehaviourTilesRunner._tiles_to_check, World.world.tiles_list.Length);
    }
    WorldBehaviourTilesRunner._tile_next_check = 0;
  }
}
