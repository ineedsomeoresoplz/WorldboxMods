// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using strings;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public static class WorldBehaviourActionFire
{
  private static readonly HashSetWorldTile _tiles = new HashSetWorldTile();
  private static int[] _fires;
  private static readonly List<WorldTile> _list_updater = new List<WorldTile>();

  public static void prepare()
  {
    if (WorldBehaviourActionFire._fires == null || WorldBehaviourActionFire._fires.Length != World.world.zone_calculator.zones.Count)
      WorldBehaviourActionFire._fires = new int[World.world.zone_calculator.zones.Count];
    else
      WorldBehaviourActionFire.clearFires();
  }

  public static void clearFires()
  {
    if (WorldBehaviourActionFire._fires == null)
      return;
    for (int index = 0; index < WorldBehaviourActionFire._fires.Length; ++index)
      WorldBehaviourActionFire._fires[index] = 0;
  }

  public static void addFire(WorldTile pTile)
  {
    if (!WorldBehaviourActionFire._tiles.Add(pTile))
      return;
    World.world.fire_layer.setTileDirty(pTile);
    ++WorldBehaviourActionFire._fires[pTile.zone.id];
  }

  public static void removeFire(WorldTile pTile)
  {
    if (!WorldBehaviourActionFire._tiles.Remove(pTile))
      return;
    World.world.fire_layer.setTileDirty(pTile);
    if (WorldBehaviourActionFire._fires[pTile.zone.id] == 0)
      Debug.Log((object) "FIRE ERROR");
    else
      --WorldBehaviourActionFire._fires[pTile.zone.id];
  }

  public static void updateFire()
  {
    if (WorldBehaviourActionFire._tiles.Count == 0)
      return;
    WorldBehaviourActionFire._list_updater.Clear();
    WorldBehaviourActionFire._list_updater.AddRange((IEnumerable<WorldTile>) WorldBehaviourActionFire._tiles);
    foreach (WorldTile pTile in WorldBehaviourActionFire._list_updater)
    {
      float timeElapsedSince = World.world.getWorldTimeElapsedSince(pTile.data.fire_timestamp);
      bool flag1 = true;
      if (World.world.era_manager.getCurrentAge().particles_rain)
        flag1 = false;
      if (WorldLawLibrary.world_law_gaias_covenant.isEnabled())
        flag1 = false;
      if ((double) timeElapsedSince >= (double) SimGlobals.m.fire_spread_time & flag1)
      {
        for (int index = 0; index < pTile.neighbours.Length; ++index)
        {
          WorldTile neighbour = pTile.neighbours[index];
          if (Randy.randomChance(neighbour.Type.fire_chance * World.world_era.fire_spread_rate_bonus) && neighbour.startFire())
            World.world.flash_effects.flashPixel(neighbour, 10);
        }
      }
      if (Randy.randomChance(0.1f))
        World.world.particles_fire.spawn(pTile.posV3);
      bool flag2 = false;
      float pVal = 0.0f;
      if ((double) timeElapsedSince > (double) SimGlobals.m.fire_stop_time)
        pVal = (float) (0.10000000149011612 + (double) timeElapsedSince / (double) SimGlobals.m.fire_time * 0.30000001192092896);
      if (pTile.Type.ocean)
        flag2 = true;
      else if ((double) timeElapsedSince >= (double) SimGlobals.m.fire_time)
        flag2 = true;
      else if (Randy.randomChance(pVal))
        flag2 = true;
      if (flag2)
      {
        pTile.stopFire();
        WorldBehaviourActionFire.checkFireElementalSpawn(pTile);
      }
    }
  }

  private static void checkFireElementalSpawn(WorldTile pTile)
  {
    if (!WorldLawLibrary.world_law_disasters_other.isEnabled() || !World.world_era.era_disaster_fire_elemental_spawn_on_fire || !Randy.randomChance(World.world_era.fire_elemental_spawn_chance) || ActorTool.countUnitsFrom("fire_elemental") > 100)
      return;
    World.world.units.spawnNewUnit(SA.fire_elementals.GetRandom<string>(), pTile, pAdultAge: true);
  }

  public static void clear()
  {
    WorldBehaviourActionFire._tiles.Clear();
    WorldBehaviourActionFire._list_updater.Clear();
  }

  public static bool hasFires() => WorldBehaviourActionFire._tiles.Count > 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int countFires(TileZone pZone) => WorldBehaviourActionFire._fires[pZone.id];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool hasFires(TileZone pZone) => WorldBehaviourActionFire._fires[pZone.id] > 0;

  public static int[] getFires() => WorldBehaviourActionFire._fires;
}
