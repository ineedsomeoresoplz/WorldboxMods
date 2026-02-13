// Decompiled with JetBrains decompiler
// Type: Ant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
internal static class Ant
{
  private static List<WorldTile> _axis_neighbours = new List<WorldTile>(4);

  public static WorldTile getNextTile(WorldTile pTile, ActorDirection pDirection)
  {
    switch (pDirection)
    {
      case ActorDirection.Up:
        return pTile.tile_up;
      case ActorDirection.UpRight:
        return pTile?.tile_up?.tile_right;
      case ActorDirection.Right:
        return pTile.tile_right;
      case ActorDirection.UpLeft:
        return pTile?.tile_up?.tile_left;
      case ActorDirection.Down:
        return pTile.tile_down;
      case ActorDirection.DownRight:
        return pTile?.tile_down?.tile_right;
      case ActorDirection.DownLeft:
        return pTile?.tile_down?.tile_left;
      case ActorDirection.Left:
        return pTile.tile_left;
      default:
        return (WorldTile) null;
    }
  }

  public static WorldTile randomNeighbour(WorldTile pTile)
  {
    try
    {
      Ant._axis_neighbours.Add(pTile.tile_up);
      Ant._axis_neighbours.Add(pTile.tile_right);
      Ant._axis_neighbours.Add(pTile.tile_left);
      Ant._axis_neighbours.Add(pTile.tile_down);
      foreach (WorldTile worldTile in Ant._axis_neighbours.LoopRandom<WorldTile>())
      {
        if (worldTile != null)
          return worldTile;
      }
      return (WorldTile) null;
    }
    finally
    {
      Ant._axis_neighbours.Clear();
    }
  }

  internal static void antUseOnTile(WorldTile pTile, string pType)
  {
    MapAction.terraformMain(pTile, AssetManager.tiles.get(pType), TerraformLibrary.destroy);
    MusicBox.playSound("event:/SFX/UNIQUE/langton/ant_step", pTile);
  }
}
