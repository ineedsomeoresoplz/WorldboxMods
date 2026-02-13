// Decompiled with JetBrains decompiler
// Type: MapAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class MapAction
{
  private static List<WorldTile> temp_list_tiles_road_path = new List<WorldTile>();
  private static List<WorldTile> temp_list_tiles_road_tiles_to_build = new List<WorldTile>();

  public static void checkAcidTerraform(WorldTile pTile)
  {
    if (pTile.isTemporaryFrozen())
      pTile.unfreeze(99);
    if (pTile.top_type != null && pTile.top_type.wasteland)
      return;
    if (pTile.top_type != null)
    {
      MapAction.decreaseTile(pTile, true);
    }
    else
    {
      if (!pTile.Type.ground)
        return;
      if (pTile.isTileRank(TileRank.Low))
        MapAction.terraformTop(pTile, TopTileLibrary.wasteland_low);
      else if (pTile.isTileRank(TileRank.High))
        MapAction.terraformTop(pTile, TopTileLibrary.wasteland_high);
      AchievementLibrary.lets_not.check();
    }
  }

  public static void terraformMain(WorldTile pTile, TileType pType, bool pSkipTerraform = false)
  {
    MapAction.terraformTile(pTile, pType, (TopTileType) null, TerraformLibrary.flash, pSkipTerraform);
  }

  public static void terraformTop(WorldTile pTile, TopTileType pTopType, bool pSkipTerraform = false)
  {
    MapAction.terraformTile(pTile, pTile.main_type, pTopType, TerraformLibrary.flash, pSkipTerraform);
  }

  public static void terraformMain(
    WorldTile pTile,
    TileType pType,
    TerraformOptions pOptions,
    bool pSkipTerraform = false)
  {
    MapAction.terraformTile(pTile, pType, (TopTileType) null, pOptions, pSkipTerraform);
  }

  public static void terraformTop(
    WorldTile pTile,
    TopTileType pTopType,
    TerraformOptions pOptions,
    bool pSkipTerraform = false)
  {
    MapAction.terraformTile(pTile, pTile.main_type, pTopType, pOptions, pSkipTerraform);
  }

  public static void terraformTile(
    WorldTile pTile,
    TileType pNewTypeMain,
    TopTileType pTopType,
    TerraformOptions pOptions = null,
    bool pSkipTerraform = false)
  {
    if (pOptions == null)
      pOptions = TerraformLibrary.flash;
    TileTypeBase type1 = pTile.Type;
    TileTypeBase type2 = pTile.Type;
    if (pOptions.remove_fire)
      pTile.stopFire();
    if (!pSkipTerraform)
    {
      if (pOptions.remove_water && pTile.Type.ocean)
        pNewTypeMain = pTile.Type.decrease_to;
      if (pOptions.remove_top_tile && pTile.top_type != null)
        pNewTypeMain = pTile.Type.decrease_to;
      if (pOptions.remove_roads && pTile.Type.road)
        pNewTypeMain = pTile.Type.decrease_to;
      if (pOptions.remove_frozen && pTile.isTemporaryFrozen())
        pTile.unfreeze(99);
      if (pNewTypeMain != null)
        pTile.setTileTypes(pNewTypeMain, pTopType);
      else
        pTile.setTopTileType(pTopType);
    }
    if (type2.can_be_farm != pTile.Type.can_be_farm && !pTile.zone.hasCity())
      World.world.city_zone_helper.city_place_finder.setDirty();
    if (pTile.burned_stages > 0 && !pTile.Type.can_be_set_on_fire || pOptions.remove_burned)
      pTile.removeBurn();
    World.world.resetRedrawTimer();
    if (pOptions.remove_borders)
      World.world.checkCityZone(pTile);
    if (pOptions.flash)
      World.world.flash_effects.flashPixel(pTile, 20);
    if (pTile.hasBuilding() && !pTile.building.isRuin() && !pTile.building.asset.isOverlaysBiomeTags(pTile.Type))
    {
      if (pTile.building.asset.has_ruins_graphics)
        pTile.building.startMakingRuins();
      else
        pTile.building.startDestroyBuilding();
    }
    if (pOptions.make_ruins && pTile.hasBuilding())
    {
      Building building = pTile.building;
      if (building.asset.has_ruins_graphics)
        building.startMakingRuins();
      else
        building.startDestroyBuilding();
      if (!building.asset.can_be_placed_on_blocks && pTile.Type.rocks)
        building.startDestroyBuilding();
      if (!building.asset.can_be_placed_on_liquid && pTile.Type.liquid)
        building.startDestroyBuilding();
    }
    if (pOptions.destroy_buildings && pTile.hasBuilding())
    {
      bool flag;
      if (pOptions.ignore_kingdoms != null)
      {
        flag = true;
        for (int index = 0; index < pOptions.ignore_kingdoms.Length; ++index)
        {
          if (!(pOptions.ignore_kingdoms[index] != pTile.building.kingdom?.name))
          {
            flag = false;
            break;
          }
        }
      }
      else if (pOptions.destroy_only != null)
      {
        flag = false;
        for (int index = 0; index < pOptions.destroy_only.Count; ++index)
        {
          if (!(pOptions.destroy_only[index] != pTile.building.asset.group))
          {
            flag = true;
            break;
          }
        }
      }
      else
        flag = pOptions.ignore_buildings == null || !pOptions.ignore_buildings.Contains(pTile.building.asset.id);
      if (flag)
        pTile.building.startDestroyBuilding();
    }
    MapAction.checkTileState(pTile, type1);
  }

  public static void checkTileState(WorldTile pTile, TileTypeBase pOldType, bool pForceMapChunk = false)
  {
    if (pOldType.layer_type != pTile.Type.layer_type | pForceMapChunk)
    {
      World.world.map_chunk_manager.setDirty(pTile.chunk);
      foreach (MapChunk pChunk in pTile.chunk.neighbours_all)
        World.world.map_chunk_manager.setDirty(pChunk, false);
    }
    if (pTile.Type.layer_type != TileLayerType.Ground)
      World.world.checkCityZone(pTile);
    if (!pTile.hasBuilding() || pTile.building.asset.can_be_placed_on_liquid || !pTile.Type.ocean)
      return;
    pTile.building.startDestroyBuilding();
  }

  public static void setOcean(WorldTile pTile)
  {
    if (pTile.Type.fill_to_ocean == null)
      return;
    TileType pType = AssetManager.tiles.get(pTile.Type.fill_to_ocean);
    if (pTile.Type.water_fill_sound != string.Empty)
      MusicBox.playSound(pTile.Type.water_fill_sound, pTile);
    MapAction.terraformMain(pTile, pType, TerraformLibrary.water_fill);
  }

  public static void decreaseTile(WorldTile pTile, bool pDamage, TerraformOptions pTerraformOption)
  {
    if (!MapAction.checkTileDamageGaiaCovenant(pTile, pDamage))
      return;
    if (pTile.isTemporaryFrozen())
      pTile.unfreeze(100);
    else if (pTile.top_type != null)
    {
      MapAction.terraformTile(pTile, pTile.main_type, (TopTileType) null, pTerraformOption);
    }
    else
    {
      if (pTile.Type.decrease_to == null)
        return;
      MapAction.terraformMain(pTile, pTile.Type.decrease_to, pTerraformOption);
    }
  }

  public static bool checkTileDamageGaiaCovenant(WorldTile pTile, bool pDamage)
  {
    bool flag = pTile.Type.life || pTile.Type.explodable;
    return !pDamage || !WorldLawLibrary.world_law_gaias_covenant.isEnabled() || flag;
  }

  public static void decreaseTile(WorldTile pTile, bool pDamage, string pTerraformOption = "flash")
  {
    if (!MapAction.checkTileDamageGaiaCovenant(pTile, pDamage))
      return;
    MapAction.decreaseTile(pTile, pDamage, AssetManager.terraform.get(pTerraformOption));
  }

  public static void increaseTile(WorldTile pTile, bool pDamage, string pTerraformOption = "flash")
  {
    if (!MapAction.checkTileDamageGaiaCovenant(pTile, pDamage))
      return;
    if (pTile.top_type != null)
    {
      MapAction.terraformTile(pTile, pTile.main_type, (TopTileType) null, AssetManager.terraform.get(pTerraformOption));
    }
    else
    {
      if (pTile.Type.increase_to == null)
        return;
      MapAction.terraformMain(pTile, pTile.Type.increase_to, AssetManager.terraform.get(pTerraformOption));
    }
  }

  public static void removeLiquid(WorldTile pTile)
  {
    if (!pTile.Type.liquid)
      return;
    MapAction.decreaseTile(pTile, false);
  }

  public static void growGreens(WorldTile pTile, TopTileType pTopType)
  {
    MapAction.terraformTop(pTile, pTopType, TerraformLibrary.flash);
  }

  public static void removeGreens(WorldTile pTile) => MapAction.decreaseTile(pTile, false);

  private static void applyLightningEffect(WorldTile pTile)
  {
    if (pTile.Type.lava && pTile.heat > 20)
    {
      MapAction.decreaseTile(pTile, true);
      if (Randy.randomChance(0.9f))
      {
        int num = pTile.heat / 10;
        World.world.drop_manager.spawnParabolicDrop(pTile, "lava", pMinHeight: 0.15f, pMaxHeight: 33f + (float) (num * 2), pMinRadius: 1f, pMaxRadius: 40f + (float) num);
      }
      AchievementLibrary.lava_strike.check();
    }
    if (pTile.Type.layer_type == TileLayerType.Ocean)
    {
      MapAction.removeLiquid(pTile);
      if (Randy.randomChance(0.8f))
        World.world.drop_manager.spawnParabolicDrop(pTile, "rain", pMinHeight: 1f, pMaxHeight: 66f, pMinRadius: 1f, pMaxRadius: 45f);
    }
    if (!pTile.hasBuilding() || !pTile.building.asset.spawn_drops)
      return;
    if (!pTile.building.data.hasFlag("stop_spawn_drops"))
      pTile.building.spawnBurstSpecial(10);
    if (pTile.building.data.hasFlag("stop_spawn_drops"))
      pTile.building.data.removeFlag("stop_spawn_drops");
    else
      pTile.building.data.addFlag("stop_spawn_drops");
  }

  public static void applyTileDamage(WorldTile pTargetTile, float pRad, TerraformOptions pOptions)
  {
    World.world.resetRedrawTimer();
    BrushData brushData = Brush.get((int) pRad);
    World.world.conway_layer.checkKillRange(pTargetTile.pos, brushData.size);
    if (pOptions.remove_tornado)
      MapAction.tryRemoveTornadoFromTile(pTargetTile);
    WorldBehaviourTileEffects.checkTileForEffectKill(pTargetTile, brushData.size);
    for (int index1 = 0; index1 < brushData.pos.Length; ++index1)
    {
      BrushPixelData po = brushData.pos[index1];
      Vector2Int pos1 = pTargetTile.pos;
      int pX = ((Vector2Int) ref pos1).x + po.x;
      Vector2Int pos2 = pTargetTile.pos;
      int pY = ((Vector2Int) ref pos2).y + po.y;
      if (pX >= 0 && pX < MapBox.width && pY >= 0 && pY < MapBox.height)
      {
        WorldTile tileSimple = World.world.GetTileSimple(pX, pY);
        if (tileSimple.Type.grey_goo)
          Config.grey_goo_damaged = true;
        if (pOptions.add_burned && !tileSimple.Type.liquid)
          tileSimple.setBurned();
        if (pOptions.lightning_effect)
          MapAction.applyLightningEffect(tileSimple);
        if (pOptions.add_heat != 0)
          World.world.heat.addTile(tileSimple, pOptions.add_heat);
        if (tileSimple.hasBuilding() && pOptions.damage_buildings)
        {
          bool flag = true;
          if (pOptions.ignore_kingdoms != null && tileSimple.building.isAlive() && !tileSimple.building.kingdom.isNature())
          {
            for (int index2 = 0; index2 < pOptions.ignore_kingdoms.Length; ++index2)
            {
              Kingdom kingdom = World.world.kingdoms_wild.get(pOptions.ignore_kingdoms[index2]);
              if (tileSimple.building.kingdom == kingdom)
                flag = false;
            }
          }
          if (flag)
            tileSimple.building.getHit((float) pOptions.damage, true, AttackType.Other, (BaseSimObject) null, true, false, true);
        }
        if (pOptions.set_fire)
          tileSimple.startFire(true);
        bool flag1 = false;
        if (pOptions.explode_tile)
          flag1 = MapAction.explodeTile(tileSimple, (float) po.dist, pRad, pTargetTile, pOptions);
        if (pOptions.transform_to_wasteland && !flag1)
          MapAction.checkAcidTerraform(tileSimple);
        if (tileSimple.hasUnits() && !string.IsNullOrEmpty(pOptions.add_trait))
          tileSimple.doUnits((Action<Actor>) (tActor => tActor.addTrait(pOptions.add_trait)));
      }
    }
  }

  public static bool explodeTile(
    WorldTile pTile,
    float pDist,
    float pRadius,
    WorldTile pExplosionCenter,
    TerraformOptions pOptions)
  {
    if (pOptions.damage > 0)
      pTile.doUnits((Action<Actor>) (tActor =>
      {
        if (tActor.asset.very_high_flyer && !pOptions.applies_to_high_flyers)
          return;
        tActor.getHit((float) pOptions.damage, pAttackType: AttackType.Explosion);
      }));
    if (pTile.isTemporaryFrozen())
      pTile.unfreeze();
    float num1 = pDist / pRadius;
    float num2 = 1f - num1;
    int num3 = (int) (30.0 * (double) num2);
    if (num3 <= 0)
      return false;
    bool liquid = pTile.Type.liquid;
    if (!pTile.Type.explodable && (double) Randy.random() > (double) num2)
      return false;
    ++World.world.game_stats.data.pixelsExploded;
    if (pOptions.explosion_pixel_effect)
      World.world.explosion_layer.setDirty(pTile, pDist, pRadius);
    int num4 = num3 - (int) ((double) num3 * 0.5 * (double) Randy.random() * (double) num1);
    if (pTile.Type.explodable && pTile.explosion_wave == 0)
      World.world.explosion_layer.explodeBomb(pTile);
    if (pTile.Type.explodable_delayed)
      World.world.explosion_layer.activateDelayedBomb(pTile);
    if (pTile.Type.strength <= pOptions.explode_strength)
      MapAction.decreaseTile(pTile, true, TerraformLibrary.flash);
    if (pTile.hasBuilding() && pTile.Type.liquid && !pTile.building.asset.can_be_placed_on_liquid)
      pTile.building.startDestroyBuilding();
    if (!liquid)
    {
      pTile.setBurned();
      if (pOptions.explode_and_set_random_fire)
      {
        if ((double) Randy.random() > 0.8)
          pTile.startFire(true);
        else
          pTile.startFire();
      }
    }
    return true;
  }

  public static void damageWorld(
    WorldTile pTile,
    int pRad,
    TerraformOptions pOptions,
    BaseSimObject pByWho = null)
  {
    if (pOptions.shake)
      World.world.startShake(pOptions.shake_duration, pOptions.shake_interval, pOptions.shake_intensity);
    if (pOptions.apply_force)
      World.world.applyForceOnTile(pTile, pRad, pOptions.force_power, pDamage: pOptions.damage, pIgnoreKingdoms: pOptions.ignore_kingdoms, pByWho: pByWho, pOptions: pOptions);
    MapAction.applyTileDamage(pTile, (float) pRad, pOptions);
  }

  public static void makeTileChanged(WorldTile pTile) => World.world.resetRedrawTimer();

  public static void removeLifeFromTile(WorldTile pTile)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void createRoadTile(WorldTile pTile)
  {
    MapAction.terraformTop(pTile, TopTileLibrary.road, AssetManager.terraform.get("road"));
  }

  public static void createRoadTilesToBuild(
    List<WorldTile> pPath,
    WorldTile pFrom,
    WorldTile pTarget,
    bool pForceFinished = false)
  {
    if (pPath.Count > 20 || pTarget.road_island != null && pTarget.road_island == pFrom.road_island)
      return;
    for (int index = 0; index < pPath.Count; ++index)
    {
      WorldTile pTile = pPath[index];
      if (!pTile.Type.road)
      {
        if (pFrom != pTile && pFrom.road_island != null && pTile.road_island == pTarget.road_island)
          return;
        MapAction.temp_list_tiles_road_tiles_to_build.Add(pTile);
        if (pForceFinished)
          MapAction.createRoadTile(pTile);
      }
    }
    World.world.resetRedrawTimer();
  }

  public static void makeRoadBetween(
    WorldTile pTile1,
    WorldTile pTile2,
    City pCity = null,
    bool pForceFinished = false)
  {
    if (pTile1.road_island != null && pTile1.road_island == pTile2.road_island)
      return;
    MapAction.temp_list_tiles_road_path.Clear();
    MapAction.temp_list_tiles_road_tiles_to_build.Clear();
    World.world.pathfinding_param.resetParam();
    World.world.pathfinding_param.roads = true;
    World.world.calcPath(pTile1, pTile2, MapAction.temp_list_tiles_road_path);
    MapAction.createRoadTilesToBuild(MapAction.temp_list_tiles_road_path, pTile1, pTile2, pForceFinished);
    pCity?.addRoads(MapAction.temp_list_tiles_road_tiles_to_build);
  }

  public static void tryRemoveTornadoFromTile(WorldTile pTile)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void checkSantaHit(Vector2Int pPos, int pRad)
  {
    List<BaseEffect> list = World.world.stack_effects.get("fx_santa").getList();
    for (int index = 0; index < list.Count; ++index)
    {
      Santa component = ((Component) list[index]).GetComponent<Santa>();
      if (component.active && component.alive)
      {
        Vector3 localPosition = ((Component) component).transform.localPosition;
        if ((double) Toolbox.Dist((float) ((Vector2Int) ref pPos).x, 0.0f, localPosition.x, 0.0f) <= (double) pRad && (double) localPosition.y >= (double) ((Vector2Int) ref pPos).y && (double) localPosition.y - 20.0 <= (double) ((Vector2Int) ref pPos).y)
        {
          component.alive = false;
          AchievementLibrary.mayday.check();
        }
      }
    }
  }

  public static void checkUFOHit(Vector2Int pPos, int pRad, Actor pActor)
  {
    Kingdom kingdom = World.world.kingdoms_wild.get("aliens");
    if (kingdom.units.Count == 0)
      return;
    List<Actor> units = kingdom.units;
    for (int index = 0; index < units.Count; ++index)
    {
      Actor actor = units[index];
      if (actor.isAlive())
      {
        Vector3 vector3 = Vector2.op_Implicit(actor.current_position);
        if ((double) Toolbox.Dist((float) ((Vector2Int) ref pPos).x, 0.0f, vector3.x, 0.0f) <= (double) pRad && (double) vector3.y >= (double) ((Vector2Int) ref pPos).y && (double) vector3.y - 10.0 <= (double) ((Vector2Int) ref pPos).y && actor.asset.flag_ufo)
          actor.getHit((float) actor.getHealth(), pAttacker: (BaseSimObject) pActor);
      }
    }
  }

  public static void checkTornadoHit(Vector2Int pPos, int pRad)
  {
    if (!World.world.stack_effects.get("fx_tornado").isAnyActive())
      return;
    using (ListPool<BaseEffect> listPool = new ListPool<BaseEffect>((ICollection<BaseEffect>) World.world.stack_effects.get("fx_tornado").getList()))
    {
      for (int index = 0; index < listPool.Count; ++index)
      {
        if (listPool[index].active)
        {
          TornadoEffect tornadoEffect = (TornadoEffect) listPool[index];
          if (!tornadoEffect.isKilled() && (double) Toolbox.DistVec2Float(Vector2.op_Implicit(((Component) tornadoEffect).transform.localPosition), Vector2Int.op_Implicit(pPos)) <= (double) pRad)
            tornadoEffect.split();
        }
      }
    }
  }

  public static void checkLightningAction(
    Vector2Int pPos,
    int pRad,
    Actor pActor = null,
    bool pCheckForImmortal = false,
    bool pCheckMayIInterrupt = false)
  {
    bool flag = false;
    int num = pRad * pRad;
    List<Actor> simpleList = World.world.units.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
    {
      Actor actor = simpleList[index];
      if (Toolbox.SquaredDistVec2(actor.current_tile.pos, pPos) <= num)
      {
        if (actor.asset.flag_finger)
        {
          actor.getActorComponent<GodFinger>().lightAction();
          actor.getHit(1f, pAttacker: (BaseSimObject) pActor);
        }
        else
        {
          if (pCheckForImmortal && !flag && !actor.hasTrait("immortal") && Randy.randomChance(0.2f))
          {
            actor.addTrait("immortal");
            actor.addTrait("energized");
            flag = true;
          }
          if (pCheckMayIInterrupt)
            AchievementLibrary.may_i_interrupt.checkBySignal((object) actor.ai.task?.id);
        }
      }
    }
  }
}
