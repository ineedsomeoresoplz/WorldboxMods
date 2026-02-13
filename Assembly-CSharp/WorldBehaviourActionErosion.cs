// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionErosion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class WorldBehaviourActionErosion
{
  private const int MAX_TILES_IN_LIST = 5;
  private static List<WorldTile> list = new List<WorldTile>();

  public static void updateErosion()
  {
    if (!WorldLawLibrary.world_law_erosion.isEnabled())
      return;
    WorldBehaviourActionErosion.list.Clear();
    foreach (TileIsland tileIsland in World.world.islands_calculator.islands.LoopRandom<TileIsland>())
    {
      if (tileIsland.type == TileLayerType.Ground)
      {
        for (int index = 0; index < 5; ++index)
        {
          WorldTile randomTile = tileIsland.getRandomTile();
          if (randomTile != null && randomTile.Type.can_errode_to_sand && (randomTile.Type.can_be_biome || randomTile.Type.grass) && randomTile.IsOceanAround() && !WorldBehaviourActionErosion.list.Contains(randomTile))
          {
            WorldBehaviourActionErosion.list.Add(randomTile);
            if (WorldBehaviourActionErosion.list.Count >= 5)
              break;
          }
        }
        if (WorldBehaviourActionErosion.list.Count >= 5)
          break;
      }
    }
    if (WorldBehaviourActionErosion.list.Count == 0)
      return;
    for (int index = 0; index < WorldBehaviourActionErosion.list.Count; ++index)
      MapAction.terraformMain(WorldBehaviourActionErosion.list[index], TileLibrary.sand, AssetManager.terraform.get("flash"));
  }

  public static void clear() => WorldBehaviourActionErosion.list.Clear();
}
