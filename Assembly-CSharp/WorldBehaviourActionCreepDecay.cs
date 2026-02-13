// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionCreepDecay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class WorldBehaviourActionCreepDecay
{
  private const int MAX_CREEP_TO_DESTROY_IN_ONE_STEP = 3;
  private static List<WorldTile> next_wave = new List<WorldTile>();
  private static List<WorldTile> cur_wave = new List<WorldTile>();
  private static HashSetWorldTile checked_tiles = new HashSetWorldTile();
  private static HashSetWorldTile not_checked_tiles = new HashSetWorldTile();
  private static List<WorldTile> _list_of_disconnected_tiles = new List<WorldTile>();

  public static void checkCreep()
  {
    if (WorldLawLibrary.world_law_forever_tumor_creep.isEnabled())
      return;
    WorldBehaviourActionCreepDecay.checkBiome("tumor", "biome_tumor");
    WorldBehaviourActionCreepDecay.checkBiome("biomass", "biome_biomass");
    WorldBehaviourActionCreepDecay.checkBiome("super_pumpkin", "biome_pumpkin");
    WorldBehaviourActionCreepDecay.checkBiome("cybercore", "biome_cybertile");
  }

  private static void checkBiome(string pCreepHubID, string pBiomeID)
  {
    Kingdom kingdom = World.world.kingdoms_wild.get(AssetManager.buildings.get(pCreepHubID).kingdom);
    BiomeAsset biomeAsset = AssetManager.biome_library.get(pBiomeID);
    WorldBehaviourActionCreepDecay.clear();
    WorldBehaviourActionCreepDecay.addToNotChecked(biomeAsset.getTileLow());
    WorldBehaviourActionCreepDecay.addToNotChecked(biomeAsset.getTileHigh());
    if (WorldBehaviourActionCreepDecay.not_checked_tiles.Count == 0)
      return;
    if (kingdom.buildings.Count > 0)
    {
      List<Building> buildings = kingdom.buildings;
      for (int index = 0; index < buildings.Count; ++index)
      {
        Building building = buildings[index];
        if (building.isUsable())
        {
          WorldBehaviourActionCreepDecay.checkTile(building.current_tile);
          WorldBehaviourActionCreepDecay.next_wave.Add(building.current_tile);
        }
      }
    }
    WorldBehaviourActionCreepDecay.startWave(pBiomeID);
    if (WorldBehaviourActionCreepDecay.not_checked_tiles.Count <= 0)
      return;
    WorldBehaviourActionCreepDecay.destroyNonCheckedCreep();
  }

  private static void startWave(string pBiomeID)
  {
    if (WorldBehaviourActionCreepDecay.next_wave.Count == 0)
      return;
    WorldBehaviourActionCreepDecay.cur_wave.AddRange((IEnumerable<WorldTile>) WorldBehaviourActionCreepDecay.next_wave);
    WorldBehaviourActionCreepDecay.next_wave.Clear();
    while (WorldBehaviourActionCreepDecay.cur_wave.Count > 0)
    {
      WorldTile worldTile = WorldBehaviourActionCreepDecay.cur_wave[WorldBehaviourActionCreepDecay.cur_wave.Count - 1];
      WorldBehaviourActionCreepDecay.cur_wave.RemoveAt(WorldBehaviourActionCreepDecay.cur_wave.Count - 1);
      for (int index = 0; index < worldTile.neighboursAll.Length; ++index)
      {
        WorldTile pTile = worldTile.neighboursAll[index];
        if (!(pTile.Type.biome_id != pBiomeID) && !WorldBehaviourActionCreepDecay.checked_tiles.Contains(pTile))
        {
          WorldBehaviourActionCreepDecay.checkTile(pTile);
          WorldBehaviourActionCreepDecay.next_wave.Add(pTile);
        }
      }
    }
    if (WorldBehaviourActionCreepDecay.next_wave.Count <= 0)
      return;
    WorldBehaviourActionCreepDecay.startWave(pBiomeID);
  }

  private static void destroyNonCheckedCreep()
  {
    foreach (WorldTile notCheckedTile in (HashSet<WorldTile>) WorldBehaviourActionCreepDecay.not_checked_tiles)
      WorldBehaviourActionCreepDecay._list_of_disconnected_tiles.Add(notCheckedTile);
    foreach (WorldTile pTile in WorldBehaviourActionCreepDecay._list_of_disconnected_tiles.LoopRandom<WorldTile>(3))
      MapAction.decreaseTile(pTile, false);
  }

  private static void checkTile(WorldTile pTile)
  {
    WorldBehaviourActionCreepDecay.checked_tiles.Add(pTile);
    WorldBehaviourActionCreepDecay.not_checked_tiles.Remove(pTile);
  }

  private static void addToNotChecked(TopTileType pTileType)
  {
    if (pTileType.hashset.Count == 0)
      return;
    WorldBehaviourActionCreepDecay.not_checked_tiles.UnionWith((IEnumerable<WorldTile>) pTileType.hashset);
  }

  public static void clear()
  {
    WorldBehaviourActionCreepDecay.checked_tiles.Clear();
    WorldBehaviourActionCreepDecay.not_checked_tiles.Clear();
    WorldBehaviourActionCreepDecay.next_wave.Clear();
    WorldBehaviourActionCreepDecay.cur_wave.Clear();
    WorldBehaviourActionCreepDecay._list_of_disconnected_tiles.Clear();
  }
}
