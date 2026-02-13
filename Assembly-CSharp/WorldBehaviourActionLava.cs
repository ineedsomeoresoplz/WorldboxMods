// Decompiled with JetBrains decompiler
// Type: WorldBehaviourActionLava
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldBehaviourActionLava : WorldBehaviourTilesRunner
{
  public const int CHUNK_MOD_AMOUNT = 15;

  public static void update()
  {
    if (!WorldBehaviourActionLava.hasLava())
      return;
    AchievementLibrary.the_hell.check();
    WorldBehaviourActionLava.updateSingleTiles();
  }

  public static bool hasLava()
  {
    return TileLibrary.lava0.hashset.Count > 0 || TileLibrary.lava1.hashset.Count > 0 || TileLibrary.lava2.hashset.Count > 0 || TileLibrary.lava3.hashset.Count > 0;
  }

  private static void updateSingleTiles()
  {
    WorldBehaviourTilesRunner.checkTiles();
    WorldTile[] tilesToCheck = WorldBehaviourTilesRunner._tiles_to_check;
    int num = World.world.map_chunk_manager.amount_x * 15;
    if (WorldBehaviourTilesRunner._tile_next_check + num >= tilesToCheck.Length)
      num = tilesToCheck.Length - WorldBehaviourTilesRunner._tile_next_check;
    while (num-- > 0)
    {
      WorldBehaviourTilesRunner._tiles_to_check.ShuffleOne<WorldTile>(WorldBehaviourTilesRunner._tile_next_check);
      WorldTile pTile = tilesToCheck[WorldBehaviourTilesRunner._tile_next_check++];
      if (pTile.Type.lava)
        WorldBehaviourActionLava.tryToMoveLava(pTile);
    }
  }

  private static void damageBuildingOnTile(WorldTile pTile)
  {
    if (!pTile.hasBuilding() || !pTile.building.asset.affected_by_lava)
      return;
    float maxHealthPercent = (float) pTile.building.getMaxHealthPercent(0.4f);
    pTile.building.getHit(maxHealthPercent, true, AttackType.Other, (BaseSimObject) null, true, false, true);
  }

  private static bool tryToMoveLava(WorldTile pTile)
  {
    bool moveLava = false;
    WorldBehaviourActionLava.damageBuildingOnTile(pTile);
    if (WorldLawLibrary.world_law_forever_lava.isEnabled() || (double) World.world.getWorldTimeElapsedSince(pTile.timestamp_type_changed) < (double) pTile.Type.lava_change_state_after)
      return false;
    if (pTile.Type.lava_level == 0)
    {
      if (WorldLawLibrary.world_law_forever_lava.isEnabled())
        return false;
      bool flag = false;
      foreach (WorldTile neighbour in pTile.neighbours)
      {
        if (neighbour.Type.lava && neighbour.Type.lava_level > 0)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        WorldBehaviourActionLava.changeLavaTile(pTile, TileLibrary.hills);
        moveLava = true;
      }
    }
    else if (Randy.randomBool())
    {
      moveLava = WorldBehaviourActionLava.moveLava(pTile);
      if (pTile.flash_state <= 0 && Randy.randomChance(0.995f))
        pTile.startFire(true);
    }
    return moveLava;
  }

  private static bool moveLava(WorldTile pLavaTile)
  {
    if (Randy.randomChance(0.1f))
      World.world.particles_fire.spawn(pLavaTile.posV3);
    WorldTile pTile1 = (WorldTile) null;
    foreach (WorldTile pTile2 in pLavaTile.neighbours.LoopRandom<WorldTile>())
    {
      if (!pTile2.Type.lava)
      {
        if (pTile2.isTemporaryFrozen())
        {
          World.world.heat.addTile(pTile2, 15);
          pTile2.unfreeze(99);
        }
        else
          pTile2.startFire();
      }
      if (!pTile2.Type.hold_lava && pTile2.Type.lava_level != pLavaTile.Type.lava_level && (!pTile2.Type.lava || !string.IsNullOrEmpty(pTile2.Type.lava_increase)))
      {
        if (pTile1 == null)
          pTile1 = pTile2;
        else if (pTile1.Type.render_z >= pTile2.Type.render_z)
        {
          if (pTile1.Type.lava)
          {
            if (pTile2.Type.lava_level < pTile1.Type.lava_level)
              pTile1 = pTile2;
          }
          else
            pTile1 = pTile2;
        }
      }
    }
    if (pTile1 == null)
      return false;
    WorldBehaviourActionLava.changeLavaTile(pLavaTile, pLavaTile.Type.lava_decrease);
    if (!pTile1.Type.lava)
      WorldBehaviourActionLava.changeLavaTile(pTile1, TileLibrary.lava0);
    else
      WorldBehaviourActionLava.changeLavaTile(pTile1, pLavaTile.Type.lava_increase);
    World.world.flash_effects.flashPixel(pTile1, 10);
    return true;
  }

  private static void changeLavaTile(WorldTile pTile, TileType pType)
  {
    if (pTile.Type == pType)
      return;
    MapAction.terraformMain(pTile, pType, TerraformLibrary.lava_damage);
    WorldBehaviourActionLava.damageBuildingOnTile(pTile);
  }

  private static void changeLavaTile(WorldTile pTile, string pType)
  {
    WorldBehaviourActionLava.changeLavaTile(pTile, AssetManager.tiles.get(pType));
  }
}
