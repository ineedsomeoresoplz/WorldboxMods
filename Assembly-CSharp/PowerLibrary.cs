// Decompiled with JetBrains decompiler
// Type: PowerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using strings;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
public class PowerLibrary : AssetLibrary<GodPower>
{
  private const string TEMPLATE_EXPLOSIVE_TILES = "$template_explosive_tiles$";
  private const string TEMPLATE_BOMBS = "$template_bombs$";
  private const string TEMPLATE_DROPS = "$template_drops$";
  private const string TEMPLATE_SEEDS = "$template_seeds$";
  private const string TEMPLATE_PLANTS = "$template_plants$";
  private const string TEMPLATE_DROP_MINERALS = "$template_minerals$";
  private const string TEMPLATE_DROP_BUILDING = "$template_drop_building$";
  private const string TEMPLATE_PRINTER = "$template_printer$";
  private const string TEMPLATE_SPAWN_SPECIAL = "$template_spawn_special$";
  private const string TEMPLATE_SPAWN_ACTOR = "$template_spawn_actor$";
  private const string TEMPLATE_TERRAFORM_TILES = "$template_terraform_tiles$";
  private const string TEMPLATE_WALL = "$template_wall$";
  private const string TEMPLATE_DRAW = "$template_draw$";
  private const string TEMPLATE_ERASER = "$template_eraser$";
  public static GodPower traits_gamma_rain_edit;
  public static GodPower traits_delta_rain_edit;
  public static GodPower traits_omega_rain_edit;
  public static GodPower equipment_rain_edit;
  public static GodPower inspect_unit;

  public override void init()
  {
    base.init();
    this.addCivsClassic();
    this.addCivsAnimals();
    this.addMobs();
    this.addSpecial();
    this.addTerraformTiles();
    this.addDestruction();
    this.addClouds();
    this.addPrinters();
    this.addDrops();
    this.addWaypoints();
  }

  private void addWaypoints()
  {
    this.clone("desire_alien_mold", "$template_drops$");
    this.t.name = "Alien Mold Desire";
    this.t.drop_id = this.t.id;
    this.clone("desire_computer", "$template_drops$");
    this.t.name = "Evil Computer Desire";
    this.t.drop_id = this.t.id;
    this.clone("desire_golden_egg", "$template_drops$");
    this.t.name = "Golden Egg Desire";
    this.t.drop_id = this.t.id;
    this.clone("desire_harp", "$template_drops$");
    this.t.name = "Ethereal Harp Desire";
    this.t.drop_id = this.t.id;
    this.clone("waypoint_alien_mold", "$template_drop_building$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Alien Mold";
    this.t.drop_id = this.t.id;
    this.clone("waypoint_computer", "$template_drop_building$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Evil Computer";
    this.t.drop_id = this.t.id;
    this.clone("waypoint_golden_egg", "$template_drop_building$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Golden Egg";
    this.t.drop_id = this.t.id;
    this.clone("waypoint_harp", "$template_drop_building$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Ethereal Harp";
    this.t.drop_id = this.t.id;
  }

  private void addTerraformTiles()
  {
    GodPower pAsset = new GodPower();
    pAsset.id = "$template_terraform_tiles$";
    pAsset.draw_lines = true;
    pAsset.terraform = true;
    pAsset.type = PowerActionType.PowerDrawTile;
    pAsset.mouse_hold_animation = MouseHoldAnimation.Draw;
    pAsset.rank = PowerRank.Rank0_free;
    pAsset.show_tool_sizes = true;
    pAsset.unselect_when_window = true;
    pAsset.hold_action = true;
    pAsset.click_interval = 0.0f;
    this.add(pAsset);
    this.t.click_action = new PowerActionWithID(this.cleanBurnedTile);
    this.t.click_action += new PowerActionWithID(this.stopFire);
    this.t.click_brush_action += new PowerActionWithID(this.fmodDrawingSound);
    this.t.click_brush_action += new PowerActionWithID(this.loopWithCurrentBrush);
    this.t.click_brush_action += new PowerActionWithID(this.drawingCursorEffect);
    this.clone("$template_draw$", "$template_terraform_tiles$");
    this.t.click_action += new PowerActionWithID(this.drawTiles);
    this.clone("$template_wall$", "$template_draw$");
    this.t.make_buildings_transparent = true;
    this.t.force_brush = "sqr_0";
    this.t.show_tool_sizes = false;
    this.t.click_action = new PowerActionWithID(this.cleanBurnedTile);
    this.t.click_action += new PowerActionWithID(this.stopFire);
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.t.click_action += new PowerActionWithID(this.drawLifeEraser);
    this.t.click_action += new PowerActionWithID(this.drawTiles);
    this.t.sound_drawing = "event:/SFX/POWERS/Mountains";
    this.clone("$template_eraser$", "$template_terraform_tiles$");
    this.t.click_action = new PowerActionWithID(this.flashPixel);
    this.clone("fuse", "$template_draw$");
    this.t.name = "Fuse";
    this.t.top_tile_type = "fuse";
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.t.click_action += new PowerActionWithID(this.flashPixel);
    this.t.sound_drawing = "event:/SFX/POWERS/Fuse";
    this.clone("tile_deep_ocean", "$template_draw$");
    this.t.name = "Deep Ocean";
    this.t.tile_type = "pit_deep_ocean";
    this.t.path_icon = "iconTileDeepOcean";
    this.t.sound_drawing = "event:/SFX/POWERS/Pit";
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.clone("tile_close_ocean", "$template_draw$");
    this.t.name = "Close Ocean";
    this.t.tile_type = "pit_close_ocean";
    this.t.path_icon = "iconTileCloseOcean";
    this.t.sound_drawing = "event:/SFX/POWERS/Pit";
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.clone("tile_shallow_waters", "$template_draw$");
    this.t.name = "Shallow Waters";
    this.t.tile_type = "pit_shallow_waters";
    this.t.path_icon = "iconTileShallowWater";
    this.t.sound_drawing = "event:/SFX/POWERS/Pit";
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.clone("tile_sand", "$template_draw$");
    this.t.name = "Sand";
    this.t.tile_type = "sand";
    this.t.path_icon = "iconTileSand";
    this.t.sound_drawing = "event:/SFX/POWERS/Sand";
    this.clone("tile_soil", "$template_draw$");
    this.t.name = "Soil";
    this.t.tile_type = "soil_low";
    this.t.path_icon = "iconTileSoil";
    this.t.sound_drawing = "event:/SFX/POWERS/SoilLow";
    this.clone("tile_high_soil", "$template_draw$");
    this.t.name = "Soil High";
    this.t.tile_type = "soil_high";
    this.t.path_icon = "iconTileHighSoil";
    this.t.sound_drawing = "event:/SFX/POWERS/SoilHigh";
    this.clone("tile_hills", "$template_draw$");
    this.t.name = "Hills";
    this.t.tile_type = "hills";
    this.t.path_icon = "iconTileHills";
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.t.sound_drawing = "event:/SFX/POWERS/Hills";
    this.clone("tile_mountains", "$template_draw$");
    this.t.name = "Mountains";
    this.t.tile_type = "mountains";
    this.t.path_icon = "iconTileMountains";
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.t.sound_drawing = "event:/SFX/POWERS/Mountains";
    this.clone("tile_summit", "$template_draw$");
    this.t.name = "Summit";
    this.t.tile_type = "summit";
    this.t.path_icon = "iconTileSummit";
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.t.sound_drawing = "event:/SFX/POWERS/Mountains";
    this.clone("wall_order", "$template_wall$");
    this.t.name = "Stone Wall";
    this.t.top_tile_type = "wall_order";
    this.t.path_icon = "iconWallOrder";
    this.clone("wall_evil", "$template_wall$");
    this.t.name = "Wall of Evil";
    this.t.top_tile_type = "wall_evil";
    this.t.path_icon = "iconWallEvil";
    this.clone("wall_ancient", "$template_wall$");
    this.t.name = "Ancient Wall";
    this.t.top_tile_type = "wall_ancient";
    this.t.path_icon = "iconWallAncient";
    this.clone("wall_wild", "$template_wall$");
    this.t.name = "Wooden Wall";
    this.t.top_tile_type = "wall_wild";
    this.t.path_icon = "iconWallWild";
    this.clone("wall_green", "$template_wall$");
    this.t.name = "Green Wall";
    this.t.top_tile_type = "wall_green";
    this.t.path_icon = "iconWallGreen";
    this.clone("wall_iron", "$template_wall$");
    this.t.name = "Iron Wall";
    this.t.top_tile_type = "wall_iron";
    this.t.path_icon = "iconWallIron";
    this.clone("wall_light", "$template_wall$");
    this.t.name = "Wall of Light";
    this.t.top_tile_type = "wall_light";
    this.t.path_icon = "iconWallLight";
    this.clone("shovel_plus", "$template_terraform_tiles$");
    this.t.name = "Shovel Plus";
    this.t.path_icon = "iconShovelPlus";
    this.t.click_action += new PowerActionWithID(this.drawShovelPlus);
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.t.click_action += new PowerActionWithID(this.flashPixel);
    this.t.sound_drawing = "event:/SFX/POWERS/ShovelPlus";
    this.clone("shovel_minus", "$template_terraform_tiles$");
    this.t.name = "Shovel Minus";
    this.t.path_icon = "iconShovelMinus";
    this.t.click_action += new PowerActionWithID(this.drawShovelMinus);
    this.t.click_action += new PowerActionWithID(this.destroyBuildings);
    this.t.click_action += new PowerActionWithID(this.flashPixel);
    this.t.sound_drawing = "event:/SFX/POWERS/ShovelMinus";
    this.clone("vortex", "$template_terraform_tiles$");
    this.t.name = "Vortex";
    this.t.path_icon = "iconVertex2";
    this.t.click_action = new PowerActionWithID(this.stopFire);
    this.t.sound_drawing = "event:/SFX/POWERS/Vortex";
    this.t.click_brush_action += new PowerActionWithID(this.useVortex);
    this.clone("grey_goo", "$template_eraser$");
    this.t.name = "Grey Goo";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.click_action += new PowerActionWithID(this.drawGreyGoo);
    this.t.click_action += new PowerActionWithID(this.stopFire);
    this.t.sound_drawing = "event:/SFX/POWERS/GreyGoo";
    this.t.tester_enabled = false;
    this.clone("conway", "$template_eraser$");
    this.t.name = "Conway game of Life1";
    this.t.click_action += new PowerActionWithID(this.drawConway);
    this.t.click_action += new PowerActionWithID(this.stopFire);
    this.t.sound_drawing = "event:/SFX/POWERS/Conway";
    this.clone("conway_inverse", "$template_eraser$");
    this.t.name = "Conway game of Life2";
    this.t.click_action += new PowerActionWithID(this.drawConwayInverse);
    this.t.click_action += new PowerActionWithID(this.stopFire);
    this.t.sound_drawing = "event:/SFX/POWERS/Conway";
    this.clone("finger", "$template_eraser$");
    this.t.name = "Finger";
    this.t.path_icon = "iconTileFinger";
    this.t.click_action += new PowerActionWithID(this.drawFinger);
    this.t.click_action += new PowerActionWithID(this.stopFire);
    this.t.click_action += new PowerActionWithID(this.cleanBurnedTile);
    this.t.sound_drawing = "event:/SFX/POWERS/Finger";
    this.clone("life_eraser", "$template_eraser$");
    this.t.click_action += new PowerActionWithID(this.drawLifeEraser);
    this.t.name = "Life Eraser";
    this.t.sound_drawing = "event:/SFX/POWERS/LifeEraser";
    this.clone("demolish", "$template_eraser$");
    this.t.click_action += new PowerActionWithID(this.drawDemolish);
    this.t.name = "Demolish";
    this.t.sound_drawing = "event:/SFX/POWERS/Demolish";
    this.clone("scissors", "$template_eraser$");
    this.t.path_icon = "iconScissors";
    this.t.force_map_mode = MetaType.City;
    this.t.click_action += new PowerActionWithID(this.drawScissors);
    this.t.name = "Scissors";
    this.t.sound_drawing = "event:/SFX/POWERS/Demolish";
    this.clone("border_brush", "$template_eraser$");
    this.t.path_icon = "iconBorderBrush";
    this.t.force_map_mode = MetaType.City;
    this.t.click_action += new PowerActionWithID(this.drawBorderBrush);
    this.t.name = "Border Brush";
    this.t.sound_drawing = "event:/SFX/POWERS/Demolish";
    this.clone("sponge", "$template_eraser$");
    this.t.path_icon = "iconSponge";
    this.t.click_brush_action += new PowerActionWithID(this.removeClouds);
    this.t.click_brush_action += new PowerActionWithID(this.removeTornadoes);
    this.t.click_action += new PowerActionWithID(this.removeBuildingsBySponge);
    this.t.click_action += new PowerActionWithID(this.removeGoo);
    this.t.click_action += new PowerActionWithID(this.cleanBurnedTile);
    this.t.click_action += new PowerActionWithID(this.stopFire);
    this.t.name = "Sponge";
    this.t.sound_drawing = "event:/SFX/POWERS/Sponge";
    this.clone("sickle", "$template_eraser$");
    this.t.path_icon = "iconSickle";
    this.t.click_action += new PowerActionWithID(this.drawSickle);
    this.t.name = "Sickle";
    this.t.sound_event = "event:/SFX/POWERS/Sickle";
    this.t.sound_drawing = "event:/SFX/POWERS/Sickle";
    this.clone("spade", "$template_eraser$");
    this.t.path_icon = "iconSpade";
    this.t.click_action += new PowerActionWithID(this.drawSpade);
    this.t.name = "Spade";
    this.t.sound_drawing = "event:/SFX/POWERS/Spade";
    this.clone("axe", "$template_eraser$");
    this.t.path_icon = "iconAxe";
    this.t.click_action += new PowerActionWithID(this.drawAxe);
    this.t.name = "Axe";
    this.t.sound_drawing = "event:/SFX/POWERS/Axe";
    this.clone("bucket", "$template_eraser$");
    this.t.path_icon = "iconBucket";
    this.t.click_action += new PowerActionWithID(this.drawBucket);
    this.t.name = "Bucket";
    this.t.sound_drawing = "event:/SFX/POWERS/Bucket";
    this.clone("pickaxe", "$template_eraser$");
    this.t.path_icon = "iconPickaxe";
    this.t.click_action += new PowerActionWithID(this.drawPickaxe);
    this.t.name = "Pickaxe";
    this.t.sound_drawing = "event:/SFX/POWERS/Pickaxe";
    this.clone("divine_light", "$template_eraser$");
    this.t.path_icon = "iconDivineLight";
    this.t.click_brush_action += new PowerActionWithID(this.divineLightFX);
    this.t.click_action = new PowerActionWithID(this.drawDivineLight);
    this.t.name = "Divine Light";
    this.t.show_tool_sizes = true;
    this.t.sound_drawing = "event:/SFX/POWERS/DivineLight";
  }

  private void addDrops()
  {
    GodPower pAsset1 = new GodPower();
    pAsset1.id = "$template_drops$";
    pAsset1.hold_action = true;
    pAsset1.show_tool_sizes = true;
    pAsset1.unselect_when_window = true;
    pAsset1.falling_chance = 0.05f;
    pAsset1.type = PowerActionType.PowerSpawnDrops;
    pAsset1.mouse_hold_animation = MouseHoldAnimation.Sprinkle;
    this.add(pAsset1);
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    this.t.surprises_units = false;
    this.clone("paint", "$template_drops$");
    this.t.name = "Paint";
    this.t.force_map_mode = MetaType.City;
    this.t.drop_id = this.t.id;
    this.clone("dust_white", "$template_drops$");
    this.t.name = "White Dust";
    this.t.drop_id = this.t.id;
    this.clone("dust_black", "$template_drops$");
    this.t.name = "Black Dust";
    this.t.drop_id = this.t.id;
    this.clone("dust_red", "$template_drops$");
    this.t.name = "Red Dust";
    this.t.drop_id = this.t.id;
    this.clone("dust_blue", "$template_drops$");
    this.t.name = "Blue Dust";
    this.t.drop_id = this.t.id;
    this.clone("dust_gold", "$template_drops$");
    this.t.name = "Gold Dust";
    this.t.drop_id = this.t.id;
    this.clone("dust_purple", "$template_drops$");
    this.t.name = "Purple Dust";
    this.t.drop_id = this.t.id;
    this.clone("$template_explosive_tiles$", "$template_drops$");
    this.t.falling_chance = 1f;
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsRandom);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.clone("tnt", "$template_explosive_tiles$");
    this.t.name = "tnt";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/Tnt";
    this.clone("tnt_timed", "$template_explosive_tiles$");
    this.t.name = "tnt_timed";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/TntTimed";
    this.clone("water_bomb", "$template_explosive_tiles$");
    this.t.name = "Water Bomb";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/WaterBomb";
    this.clone("landmine", "$template_explosive_tiles$");
    this.t.name = "Landmine";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/LandMine";
    this.clone("fireworks", "$template_explosive_tiles$");
    this.t.name = "Fireworks";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/Fireworks";
    this.clone("inspiration", "$template_drops$");
    this.t.force_map_mode = MetaType.City;
    this.t.name = "Inspiration";
    this.t.drop_id = this.t.id;
    this.t.path_icon = "iconInspiration";
    this.t.falling_chance = 0.01f;
    this.t.sound_drawing = "event:/SFX/POWERS/Inspiration";
    this.clone("discord", "$template_drops$");
    this.t.force_map_mode = MetaType.Alliance;
    this.t.name = "Discord";
    this.t.drop_id = this.t.id;
    this.t.path_icon = "iconDiscord";
    this.t.falling_chance = 0.01f;
    this.t.sound_drawing = "event:/SFX/POWERS/Inspiration";
    this.clone("friendship", "$template_drops$");
    this.t.force_map_mode = MetaType.Kingdom;
    this.t.name = "Friendship";
    this.t.path_icon = "iconFriendship";
    this.t.drop_id = this.t.id;
    this.t.falling_chance = 0.01f;
    this.t.sound_drawing = "event:/SFX/POWERS/Friendship";
    this.clone("spite", "$template_drops$");
    this.t.force_map_mode = MetaType.Kingdom;
    this.t.name = "Spite";
    this.t.path_icon = "iconSprite";
    this.t.drop_id = this.t.id;
    this.t.falling_chance = 0.01f;
    this.t.sound_drawing = "event:/SFX/POWERS/Spite";
    this.clone("madness", "$template_drops$");
    this.t.name = "Madness";
    this.t.falling_chance = 0.01f;
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/Madness";
    this.clone("blessing", "$template_drops$");
    this.t.name = "Blessing";
    this.t.drop_id = this.t.id;
    this.t.falling_chance = 0.01f;
    this.t.sound_drawing = "event:/SFX/POWERS/Blessing";
    this.clone("shield", "$template_drops$");
    this.t.name = "Shield";
    this.t.drop_id = this.t.id;
    this.t.falling_chance = 0.01f;
    this.t.sound_drawing = "event:/SFX/POWERS/Shield";
    this.clone("curse", "$template_drops$");
    this.t.name = "Curse";
    this.t.rank = PowerRank.Rank0_free;
    this.t.drop_id = this.t.id;
    this.t.falling_chance = 0.01f;
    this.t.sound_drawing = "event:/SFX/POWERS/Curse";
    this.clone("zombie_infection", "$template_drops$");
    this.t.name = "Zombie Infection";
    this.t.falling_chance = 0.01f;
    this.t.rank = PowerRank.Rank3_good;
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.sound_drawing = "event:/SFX/POWERS/ZombieInfection";
    this.clone("mush_spores", "$template_drops$");
    this.t.name = "Mush Spores";
    this.t.falling_chance = 0.01f;
    this.t.rank = PowerRank.Rank2_normal;
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.sound_drawing = "event:/SFX/POWERS/MushSpores";
    this.clone("coffee", "$template_drops$");
    this.t.name = "Coffee";
    this.t.falling_chance = 0.01f;
    this.t.rank = PowerRank.Rank1_common;
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.sound_drawing = "event:/SFX/POWERS/Coffee";
    this.clone("powerup", "$template_drops$");
    this.t.name = "Powerup";
    this.t.falling_chance = 0.01f;
    this.t.rank = PowerRank.Rank1_common;
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.sound_drawing = "event:/SFX/POWERS/Powerup";
    this.clone("plague", "$template_drops$");
    this.t.name = "Plague";
    this.t.drop_id = this.t.id;
    this.t.falling_chance = 0.01f;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.sound_drawing = "event:/SFX/POWERS/Plague";
    this.clone("living_plants", "$template_drops$");
    this.t.name = "Living Plants";
    this.t.drop_id = this.t.id;
    this.t.actor_asset_id = "living_plants";
    this.t.falling_chance = 0.01f;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.t.sound_drawing = "event:/SFX/POWERS/LivingPlants";
    this.t.surprises_units = true;
    this.clone("living_house", "$template_drops$");
    this.t.name = "Living Houses";
    this.t.drop_id = this.t.id;
    this.t.actor_asset_id = "living_house";
    this.t.falling_chance = 0.01f;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.t.sound_drawing = "event:/SFX/POWERS/LivingHouses";
    this.t.surprises_units = true;
    this.clone("$template_bombs$", "$template_drops$");
    this.t.falling_chance = 1f;
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsRandom);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.clone("bomb", "$template_bombs$");
    this.t.name = "Bomb";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/Bomb";
    this.t.surprises_units = true;
    this.clone("grenade", "bomb");
    this.t.name = "Grenade";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/Grenade";
    this.clone("napalm_bomb", "bomb");
    this.t.name = "Napalm Bomb";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/NapalmBomb";
    this.clone("atomic_bomb", "$template_bombs$");
    this.t.name = "Atomic Bomb";
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.sound_drawing = "event:/SFX/POWERS/AtomicBomb";
    this.t.surprises_units = true;
    this.clone("antimatter_bomb", "$template_bombs$");
    this.t.name = "Antimatter Bomb";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/AntimatterBomb";
    this.t.surprises_units = true;
    this.clone("czar_bomba", "$template_bombs$");
    this.t.name = "Tsar Bomba";
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.sound_drawing = "event:/SFX/POWERS/TsarBomb";
    this.t.surprises_units = true;
    this.clone("crab_bomb", "bomb");
    this.t.name = "Crab Bomb";
    this.t.drop_id = this.t.id;
    this.t.sound_drawing = "event:/SFX/POWERS/CrabBomb";
    this.clone("rain", "$template_drops$");
    this.t.drop_id = this.t.id;
    this.t.name = "Rain";
    this.t.falling_chance = 0.02f;
    this.t.sound_drawing = "event:/SFX/POWERS/Rain";
    this.clone("blood_rain", "$template_drops$");
    this.t.drop_id = this.t.id;
    this.t.name = "Blood Rain";
    this.t.falling_chance = 0.02f;
    this.t.sound_drawing = "event:/SFX/POWERS/BloodRain";
    this.t.surprises_units = true;
    this.clone("clone_rain", "$template_drops$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.drop_id = this.t.id;
    this.t.name = "Clone Rain";
    this.t.falling_chance = 0.02f;
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/BloodRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    this.clone("dispel", "$template_drops$");
    this.t.drop_id = this.t.id;
    this.t.name = "Dispel";
    this.t.falling_chance = 0.02f;
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/BloodRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    this.clone("sleep", "$template_drops$");
    this.t.drop_id = this.t.id;
    this.t.name = "Sleep";
    this.t.falling_chance = 0.02f;
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/BloodRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    this.clone("jazz", "$template_drops$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.t.drop_id = this.t.id;
    this.t.name = "Smooth Jazz";
    this.t.falling_chance = 0.02f;
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/BloodRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    this.clone("fire", "$template_drops$");
    this.t.drop_id = this.t.id;
    this.t.name = "Fire";
    this.t.falling_chance = 0.01f;
    this.t.particle_interval = 0.3f;
    this.t.sound_drawing = "event:/SFX/POWERS/Fire";
    this.t.surprises_units = true;
    this.clone("acid", "$template_drops$");
    this.t.drop_id = this.t.id;
    this.t.name = "Acid";
    this.t.falling_chance = 0.02f;
    this.t.sound_drawing = "event:/SFX/POWERS/Acid";
    this.t.surprises_units = true;
    this.clone("lava", "$template_drops$");
    this.t.drop_id = this.t.id;
    this.t.name = "Lava";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.t.falling_chance = 0.03f;
    this.t.sound_drawing = "event:/SFX/POWERS/Lava";
    this.t.surprises_units = true;
    GodPower pAsset2 = new GodPower();
    pAsset2.id = "$template_seeds$";
    pAsset2.hold_action = true;
    pAsset2.show_tool_sizes = true;
    pAsset2.unselect_when_window = true;
    pAsset2.falling_chance = 0.05f;
    pAsset2.type = PowerActionType.PowerSpawnSeeds;
    pAsset2.mouse_hold_animation = MouseHoldAnimation.Sprinkle;
    this.add(pAsset2);
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    this.t.surprises_units = false;
    this.clone("seeds_grass", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Grass Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_savanna", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Savanna Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsSavanna";
    this.clone("seeds_enchanted", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Enchanted Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsEnchanted";
    this.clone("seeds_corrupted", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Corrupted Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsCorrupted";
    this.clone("seeds_mushroom", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Mushroom Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsMushroom";
    this.clone("seeds_swamp", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Swamp Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsSwamp";
    this.clone("seeds_infernal", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Infernal Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsInfernal";
    this.clone("seeds_jungle", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Jungle Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsJungle";
    this.clone("seeds_desert", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Desert Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsDesert";
    this.clone("seeds_lemon", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Lemon Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsLemon";
    this.clone("seeds_permafrost", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Permafrost Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsPermafrost";
    this.clone("seeds_candy", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Candy Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsCandy";
    this.clone("seeds_crystal", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Crystal Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsCrystal";
    this.clone("seeds_birch", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Birch Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_maple", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Maple Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_rocklands", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Rocklands Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_garlic", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Garlic Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_flower", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Flower Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_celestial", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Celestial Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_singularity", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Singularity Swamp Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_clover", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Clover Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("seeds_paradox", "$template_seeds$");
    this.t.drop_id = this.t.id;
    this.t.name = "Paradox Seeds";
    this.t.sound_drawing = "event:/SFX/POWERS/SeedsGrass";
    this.clone("$template_plants$", "$template_seeds$");
    this.t.type = PowerActionType.PowerSpawnDrops;
    this.clone("fruit_bush", "$template_plants$");
    this.t.type = PowerActionType.PowerSpawnDrops;
    this.t.falling_chance = 1f;
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsRandom);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.drop_id = this.t.id;
    this.t.name = "Fruit Bush";
    this.t.sound_drawing = "event:/SFX/POWERS/FruitBush";
    this.clone("fertilizer_plants", "$template_plants$");
    this.t.falling_chance = 1f;
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsRandom);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.drop_id = this.t.id;
    this.t.name = "Plants Fertilizer";
    this.t.sound_drawing = "event:/SFX/POWERS/FertilizerPlants";
    this.clone("fertilizer_trees", "$template_plants$");
    this.t.falling_chance = 1f;
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsRandom);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.drop_id = this.t.id;
    this.t.name = "Trees Fertilizer";
    this.t.sound_drawing = "event:/SFX/POWERS/FertilizerTrees";
    GodPower pAsset3 = new GodPower();
    pAsset3.id = "$template_drop_building$";
    pAsset3.unselect_when_window = true;
    pAsset3.type = PowerActionType.PowerSpawnBuilding;
    pAsset3.mouse_hold_animation = MouseHoldAnimation.Sprinkle;
    pAsset3.force_brush = "circ_1";
    pAsset3.set_used_camera_drag_on_long_move = true;
    this.add(pAsset3);
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_action += new PowerAction(this.flashPixel);
    this.clone("$template_minerals$", "$template_drops$");
    this.t.falling_chance = 1f;
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsRandom);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.clone("stone", "$template_minerals$");
    this.t.drop_id = this.t.id;
    this.t.name = "Stone";
    this.t.sound_drawing = "event:/SFX/POWERS/Minerals";
    this.clone("metals", "$template_minerals$");
    this.t.drop_id = this.t.id;
    this.t.name = "Ore Deposit";
    this.t.sound_drawing = "event:/SFX/POWERS/Minerals";
    this.clone("gold", "$template_minerals$");
    this.t.drop_id = this.t.id;
    this.t.name = "Gold";
    this.t.sound_drawing = "event:/SFX/POWERS/Minerals";
    this.clone("silver", "$template_minerals$");
    this.t.drop_id = this.t.id;
    this.t.name = "Silver";
    this.t.sound_drawing = "event:/SFX/POWERS/Minerals";
    this.clone("mythril", "$template_minerals$");
    this.t.drop_id = this.t.id;
    this.t.name = "Mythril";
    this.t.sound_drawing = "event:/SFX/POWERS/Minerals";
    this.clone("adamantine", "$template_minerals$");
    this.t.drop_id = this.t.id;
    this.t.name = "Adamantine";
    this.t.sound_drawing = "event:/SFX/POWERS/Minerals";
    this.clone("tumor", "$template_drop_building$");
    this.t.name = "Tumor";
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("biomass", "$template_drop_building$");
    this.t.name = "Biomass";
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("super_pumpkin", "$template_drop_building$");
    this.t.name = "Super Pumpkin";
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("cybercore", "$template_drop_building$");
    this.t.name = "Cybercore";
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("geyser", "$template_drop_building$");
    this.t.name = "Geyser";
    this.t.drop_id = this.t.id;
    this.clone("geyser_acid", "$template_drop_building$");
    this.t.name = "Acid Geyser";
    this.t.drop_id = this.t.id;
    this.clone("volcano", "$template_drop_building$");
    this.t.name = "Volcano";
    this.t.drop_id = this.t.id;
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.clone("golden_brain", "$template_drop_building$");
    this.t.name = "Golden Brain";
    this.t.drop_id = this.t.id;
    this.clone("monolith", "$template_drop_building$");
    this.t.name = "Monolith";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank5_noAwards;
    this.t.drop_id = this.t.id;
    this.clone("corrupted_brain", "$template_drop_building$");
    this.t.name = "Corrupted Brain";
    this.t.drop_id = this.t.id;
    this.clone("ice_tower", "$template_drop_building$");
    this.t.name = "Ice Tower";
    this.t.requires_premium = true;
    this.t.drop_id = this.t.id;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("beehive", "$template_drop_building$");
    this.t.name = "Beehive";
    this.t.drop_id = this.t.id;
    this.t.rank = PowerRank.Rank1_common;
    this.clone("flame_tower", "$template_drop_building$");
    this.t.name = "Flame Tower";
    this.t.requires_premium = true;
    this.t.drop_id = this.t.id;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("angle_tower", "$template_drop_building$");
    this.t.name = "Angle Tower";
    this.t.requires_premium = true;
    this.t.drop_id = this.t.id;
    this.t.rank = PowerRank.Rank5_noAwards;
  }

  private void addPrinters()
  {
    GodPower pAsset = new GodPower();
    pAsset.id = "$template_printer$";
    pAsset.name = "Printer";
    pAsset.unselect_when_window = true;
    pAsset.actor_spawn_height = 3f;
    pAsset.show_spawn_effect = true;
    pAsset.actor_asset_id = "printer";
    this.add(pAsset);
    this.t.click_action = new PowerActionWithID(this.spawnPrinter);
    this.clone("printer_hexagon", "$template_printer$");
    this.t.printers_print = "hexagon";
    this.clone("printer_skull", "$template_printer$");
    this.t.printers_print = "skull";
    this.clone("printer_squares", "$template_printer$");
    this.t.printers_print = "squares";
    this.clone("printer_yinyang", "$template_printer$");
    this.t.printers_print = "yinyang";
    this.clone("printer_island1", "$template_printer$");
    this.t.printers_print = "island1";
    this.clone("printer_star", "$template_printer$");
    this.t.printers_print = "star";
    this.clone("printer_heart", "$template_printer$");
    this.t.printers_print = "heart";
    this.clone("printer_diamond", "$template_printer$");
    this.t.printers_print = "diamond";
    this.clone("printer_alien_drawing", "$template_printer$");
    this.t.printers_print = "aliendrawing";
    this.clone("printer_crater", "$template_printer$");
    this.t.printers_print = "crater";
    this.clone("printer_labyrinth", "$template_printer$");
    this.t.printers_print = "labyrinth";
    this.clone("printer_spiral", "$template_printer$");
    this.t.printers_print = "spiral";
    this.clone("printer_star_fort", "$template_printer$");
    this.t.printers_print = "starfort";
    this.clone("printer_code", "$template_printer$");
    this.t.printers_print = "code";
  }

  private void addClouds()
  {
    this.clone("cloud", "$template_spawn_special$");
    this.t.name = "Cloud of Life";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudOfLife);
    this.clone("cloud_rain", "$template_spawn_special$");
    this.t.name = "Rain Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudRain);
    this.clone("cloud_fire", "$template_spawn_special$");
    this.t.name = "Fire Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudFire);
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.clone("cloud_lightning", "$template_spawn_special$");
    this.t.name = "Thunder Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudLightning);
    this.clone("cloud_ash", "$template_spawn_special$");
    this.t.name = "Ash Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudAsh);
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.clone("cloud_magic", "$template_spawn_special$");
    this.t.name = "Magic Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudMagic);
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.clone("cloud_rage", "$template_spawn_special$");
    this.t.name = "Rage Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudRage);
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.clone("cloud_acid", "$template_spawn_special$");
    this.t.name = "Acid Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudAcid);
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.clone("cloud_lava", "$template_spawn_special$");
    this.t.name = "Lava Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudLava);
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.clone("cloud_snow", "$template_spawn_special$");
    this.t.name = "Snow Cloud";
    this.t.multiple_spawn_tip = true;
    this.t.click_action = new PowerActionWithID(this.spawnCloudSnow);
  }

  private void addDestruction()
  {
    GodPower pAsset1 = new GodPower();
    pAsset1.id = "$template_spawn_special$";
    pAsset1.name = "$template_spawn_special$";
    pAsset1.unselect_when_window = true;
    pAsset1.set_used_camera_drag_on_long_move = true;
    this.add(pAsset1);
    this.clone("force", "$template_spawn_special$");
    this.t.name = "Force";
    this.t.click_action += new PowerActionWithID(this.spawnForce);
    this.clone("finger_flick", "$template_spawn_special$");
    this.t.show_close_actor = true;
    this.t.name = "finger_flick";
    this.t.click_action += new PowerActionWithID(this.fingerFlick);
    GodPower pAsset2 = new GodPower();
    pAsset2.id = "infinity_coin";
    pAsset2.name = "Infinity Coin";
    pAsset2.multiple_spawn_tip = true;
    pAsset2.set_used_camera_drag_on_long_move = true;
    this.add(pAsset2);
    this.t.click_action += new PowerActionWithID(this.spawnInfinityCoin);
    GodPower pAsset3 = new GodPower();
    pAsset3.id = "heatray";
    pAsset3.name = "Heatray";
    pAsset3.requires_premium = true;
    pAsset3.rank = PowerRank.Rank2_normal;
    pAsset3.force_brush = "circ_10";
    pAsset3.show_tool_sizes = false;
    pAsset3.unselect_when_window = true;
    pAsset3.hold_action = true;
    this.add(pAsset3);
    this.t.click_brush_action += new PowerActionWithID(this.heatrayFX);
    this.t.click_action += new PowerActionWithID(this.drawHeatray);
    this.t.click_action += new PowerActionWithID(this.flashPixel);
    GodPower pAsset4 = new GodPower();
    pAsset4.id = "meteorite";
    pAsset4.name = "Meteorite";
    pAsset4.requires_premium = true;
    pAsset4.rank = PowerRank.Rank3_good;
    pAsset4.unselect_when_window = true;
    pAsset4.set_used_camera_drag_on_long_move = true;
    pAsset4.show_spawn_effect = true;
    pAsset4.multiple_spawn_tip = true;
    this.add(pAsset4);
    this.t.click_action += new PowerActionWithID(this.spawnMeteorite);
    GodPower pAsset5 = new GodPower();
    pAsset5.id = "bowling_ball";
    pAsset5.name = "Bowling Ball";
    pAsset5.requires_premium = true;
    pAsset5.rank = PowerRank.Rank2_normal;
    pAsset5.unselect_when_window = true;
    pAsset5.show_spawn_effect = true;
    pAsset5.hold_action = true;
    pAsset5.sound_drawing = "event:/SFX/POWERS/DivineMagnet";
    pAsset5.multiple_spawn_tip = true;
    this.add(pAsset5);
    this.t.click_brush_action += new PowerActionWithID(this.prepareBoulder);
    this.t.click_brush_action += new PowerActionWithID(this.fmodDrawingSound);
    GodPower pAsset6 = new GodPower();
    pAsset6.id = "robot_santa";
    pAsset6.name = "Robot Santa";
    pAsset6.requires_premium = false;
    pAsset6.rank = PowerRank.Rank0_free;
    pAsset6.unselect_when_window = true;
    pAsset6.set_used_camera_drag_on_long_move = true;
    pAsset6.show_spawn_effect = true;
    pAsset6.multiple_spawn_tip = true;
    this.add(pAsset6);
    this.t.click_action += new PowerActionWithID(this.spawnSanta);
    GodPower pAsset7 = new GodPower();
    pAsset7.id = "lightning";
    pAsset7.name = "Lightning";
    pAsset7.unselect_when_window = true;
    pAsset7.set_used_camera_drag_on_long_move = true;
    this.add(pAsset7);
    this.t.click_action += new PowerActionWithID(this.spawnLightning);
    GodPower pAsset8 = new GodPower();
    pAsset8.id = "earthquake";
    pAsset8.name = "Earthquake";
    pAsset8.unselect_when_window = true;
    pAsset8.set_used_camera_drag_on_long_move = true;
    this.add(pAsset8);
    this.t.click_action += new PowerActionWithID(this.spawnEarthquake);
    GodPower pAsset9 = new GodPower();
    pAsset9.id = "tornado";
    pAsset9.name = "Tornado";
    pAsset9.requires_premium = true;
    pAsset9.rank = PowerRank.Rank3_good;
    pAsset9.unselect_when_window = true;
    pAsset9.set_used_camera_drag_on_long_move = true;
    pAsset9.show_spawn_effect = true;
    pAsset9.multiple_spawn_tip = true;
    this.add(pAsset9);
    this.t.click_action += new PowerActionWithID(this.spawnTornado);
  }

  private void addSpecial()
  {
    GodPower pAsset1 = new GodPower();
    pAsset1.id = "temperature_plus";
    pAsset1.name = "Temperature";
    pAsset1.hold_action = true;
    pAsset1.show_tool_sizes = true;
    pAsset1.unselect_when_window = true;
    this.add(pAsset1);
    this.t.click_action = new PowerActionWithID(this.drawTemperaturePlus);
    this.t.click_action += new PowerActionWithID(this.flashPixel);
    this.t.click_brush_action = new PowerActionWithID(this.loopWithCurrentBrush);
    this.t.click_brush_action += new PowerActionWithID(this.fmodDrawingSound);
    this.t.sound_drawing = "event:/SFX/POWERS/IncreaseTemperature";
    this.clone("temperature_minus", "temperature_plus");
    this.t.click_action = new PowerActionWithID(PowerLibrary.drawTemperatureMinus);
    this.t.click_action += new PowerActionWithID(this.flashPixel);
    this.t.sound_drawing = "event:/SFX/POWERS/DecreaseTemperature";
    GodPower pAsset2 = new GodPower();
    pAsset2.id = "magnet";
    pAsset2.name = "Magnet";
    pAsset2.show_tool_sizes = true;
    pAsset2.hold_action = true;
    pAsset2.highlight = true;
    pAsset2.sound_drawing = "event:/SFX/POWERS/DivineMagnet";
    pAsset2.unselect_when_window = true;
    this.add(pAsset2);
    this.t.click_brush_action = new PowerActionWithID(this.useMagnet);
    this.t.click_brush_action += new PowerActionWithID(this.flashBrushPixelsDuringClick);
    this.t.click_brush_action += new PowerActionWithID(this.fmodDrawingSound);
    GodPower pAsset3 = new GodPower();
    pAsset3.id = "hide_ui";
    pAsset3.name = "hide_ui";
    pAsset3.path_icon = "iconHideUI";
    pAsset3.rank = PowerRank.Rank0_free;
    this.add(pAsset3);
    this.t.select_button_action = new PowerButtonClickAction(this.clickHideUI);
    this.t.tester_enabled = false;
    this.t.activate_on_hotkey_select = false;
    GodPower pAsset4 = new GodPower();
    pAsset4.id = "traits_gamma_rain_edit";
    pAsset4.name = "Gamma Rain";
    pAsset4.path_icon = "iconRainGammaEdit";
    pAsset4.requires_premium = true;
    pAsset4.rank = PowerRank.Rank5_noAwards;
    PowerLibrary.traits_gamma_rain_edit = this.add(pAsset4);
    this.t.select_button_action = new PowerButtonClickAction(this.clickTraitEditorRainButton);
    this.t.tester_enabled = false;
    this.t.activate_on_hotkey_select = false;
    GodPower pAsset5 = new GodPower();
    pAsset5.id = "traits_delta_rain_edit";
    pAsset5.name = "Delta Rain";
    pAsset5.path_icon = "iconRainDeltaEdit";
    pAsset5.requires_premium = true;
    pAsset5.rank = PowerRank.Rank5_noAwards;
    PowerLibrary.traits_delta_rain_edit = this.add(pAsset5);
    this.t.select_button_action = new PowerButtonClickAction(this.clickTraitEditorRainButton);
    this.t.tester_enabled = false;
    this.t.activate_on_hotkey_select = false;
    GodPower pAsset6 = new GodPower();
    pAsset6.id = "traits_omega_rain_edit";
    pAsset6.name = "Omega Rain";
    pAsset6.path_icon = "iconRainOmegaEdit";
    pAsset6.requires_premium = true;
    pAsset6.rank = PowerRank.Rank5_noAwards;
    PowerLibrary.traits_omega_rain_edit = this.add(pAsset6);
    this.t.select_button_action = new PowerButtonClickAction(this.clickTraitEditorRainButton);
    this.t.tester_enabled = false;
    this.t.activate_on_hotkey_select = false;
    GodPower pAsset7 = new GodPower();
    pAsset7.id = "traits_gamma_rain";
    pAsset7.name = "Gamma Rain";
    pAsset7.path_icon = "iconRainGammaEdit";
    pAsset7.requires_premium = true;
    pAsset7.hold_action = true;
    pAsset7.show_tool_sizes = true;
    pAsset7.unselect_when_window = true;
    pAsset7.falling_chance = 0.05f;
    pAsset7.rank = PowerRank.Rank5_noAwards;
    this.add(pAsset7);
    this.t.drop_id = "gamma_rain";
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/GammaRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    GodPower pAsset8 = new GodPower();
    pAsset8.id = "traits_delta_rain";
    pAsset8.name = "Delta Rain";
    pAsset8.path_icon = "iconRainDeltaEdit";
    pAsset8.requires_premium = true;
    pAsset8.hold_action = true;
    pAsset8.show_tool_sizes = true;
    pAsset8.unselect_when_window = true;
    pAsset8.falling_chance = 0.05f;
    pAsset8.rank = PowerRank.Rank5_noAwards;
    this.add(pAsset8);
    this.t.drop_id = "delta_rain";
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/DeltaRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    GodPower pAsset9 = new GodPower();
    pAsset9.id = "traits_omega_rain";
    pAsset9.name = "Omega Rain";
    pAsset9.path_icon = "iconRainOmegaEdit";
    pAsset9.requires_premium = true;
    pAsset9.hold_action = true;
    pAsset9.show_tool_sizes = true;
    pAsset9.unselect_when_window = true;
    pAsset9.falling_chance = 0.05f;
    pAsset9.rank = PowerRank.Rank5_noAwards;
    this.add(pAsset9);
    this.t.drop_id = "omega_rain";
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/OmegaRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    GodPower pAsset10 = new GodPower();
    pAsset10.id = "equipment_rain_edit";
    pAsset10.name = "Loot Rain";
    pAsset10.path_icon = "iconRainGammaEdit";
    pAsset10.requires_premium = true;
    pAsset10.rank = PowerRank.Rank5_noAwards;
    PowerLibrary.equipment_rain_edit = this.add(pAsset10);
    this.t.select_button_action = new PowerButtonClickAction(this.clickEquipmentEditorRainButton);
    this.t.tester_enabled = false;
    this.t.activate_on_hotkey_select = false;
    GodPower pAsset11 = new GodPower();
    pAsset11.id = "equipment_rain";
    pAsset11.name = "Loot Rain";
    pAsset11.path_icon = "iconRainLootEdit";
    pAsset11.requires_premium = true;
    pAsset11.hold_action = true;
    pAsset11.show_tool_sizes = true;
    pAsset11.unselect_when_window = true;
    pAsset11.falling_chance = 0.05f;
    pAsset11.rank = PowerRank.Rank5_noAwards;
    this.add(pAsset11);
    this.t.drop_id = "loot_rain";
    this.t.click_power_action = new PowerAction(this.spawnDrops);
    this.t.click_power_brush_action = new PowerAction(this.loopWithCurrentBrushPowerForDropsFull);
    this.t.click_power_brush_action += new PowerAction(this.flashBrushPixelsDuringClick);
    this.t.sound_drawing = "event:/SFX/POWERS/GammaRain";
    this.t.click_power_action += new PowerAction(this.fmodDrawingSound);
    GodPower pAsset12 = new GodPower();
    pAsset12.id = "city_select";
    pAsset12.name = "Select City";
    pAsset12.force_map_mode = MetaType.City;
    pAsset12.path_icon = "iconCityInspect";
    pAsset12.can_drag_map = true;
    this.add(pAsset12);
    this.t.tester_enabled = false;
    this.t.track_activity = false;
    this.t.click_action += new PowerActionWithID(ActionLibrary.inspectCity);
    GodPower pAsset13 = new GodPower();
    pAsset13.id = "relations";
    pAsset13.name = "Relations";
    pAsset13.force_map_mode = MetaType.Kingdom;
    pAsset13.path_icon = "iconDiplomacy";
    pAsset13.can_drag_map = true;
    this.add(pAsset13);
    this.t.select_button_action += new PowerButtonClickAction(ActionLibrary.selectRelations);
    this.t.click_special_action += new PowerActionWithID(ActionLibrary.clickRelations);
    this.t.tester_enabled = false;
    GodPower pAsset14 = new GodPower();
    pAsset14.id = "whisper_of_war";
    pAsset14.name = "Whisper of War";
    pAsset14.force_map_mode = MetaType.Kingdom;
    pAsset14.path_icon = "iconWhisperOfWar";
    pAsset14.can_drag_map = true;
    this.add(pAsset14);
    this.t.select_button_action += new PowerButtonClickAction(ActionLibrary.selectWhisperOfWar);
    this.t.click_special_action += new PowerActionWithID(ActionLibrary.clickWhisperOfWar);
    this.t.tester_enabled = false;
    GodPower pAsset15 = new GodPower();
    pAsset15.id = "unity";
    pAsset15.name = "unity";
    pAsset15.force_map_mode = MetaType.Alliance;
    pAsset15.path_icon = "iconUnity";
    pAsset15.can_drag_map = true;
    this.add(pAsset15);
    this.t.select_button_action += new PowerButtonClickAction(ActionLibrary.selectUnity);
    this.t.click_special_action += new PowerActionWithID(ActionLibrary.clickUnity);
    this.t.tester_enabled = false;
    GodPower pAsset16 = new GodPower();
    pAsset16.id = "inspect";
    pAsset16.name = "inspect";
    pAsset16.can_drag_map = true;
    pAsset16.set_used_camera_drag_on_long_move = true;
    PowerLibrary.inspect_unit = this.add(pAsset16);
    this.t.tester_enabled = false;
    this.t.click_action += new PowerActionWithID(ActionLibrary.inspectUnit);
    this.t.allow_unit_selection = true;
    GodPower pAsset17 = new GodPower();
    pAsset17.id = "map_names";
    pAsset17.name = "Map Names";
    pAsset17.unselect_when_window = true;
    pAsset17.multi_toggle = true;
    this.add(pAsset17);
    this.t.tester_enabled = false;
    this.t.toggle_name = "map_names";
    this.t.toggle_action += new PowerToggleAction(this.toggleMultiOption);
    GodPower pAsset18 = new GodPower();
    pAsset18.id = "map_layers";
    pAsset18.name = "map_layers";
    pAsset18.unselect_when_window = true;
    this.add(pAsset18);
    this.t.tester_enabled = false;
    this.t.toggle_name = "map_layers";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset19 = new GodPower();
    pAsset19.id = "map_species_families";
    pAsset19.name = "map_species_families";
    pAsset19.unselect_when_window = true;
    this.add(pAsset19);
    this.t.tester_enabled = false;
    this.t.toggle_name = "map_species_families";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset20 = new GodPower();
    pAsset20.id = "city_layer";
    pAsset20.name = "city_layer";
    pAsset20.unselect_when_window = true;
    pAsset20.multi_toggle = true;
    this.add(pAsset20);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_city_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset21 = new GodPower();
    pAsset21.id = "culture_layer";
    pAsset21.name = "culture_layer";
    pAsset21.unselect_when_window = true;
    pAsset21.multi_toggle = true;
    this.add(pAsset21);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_culture_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset22 = new GodPower();
    pAsset22.id = "subspecies_layer";
    pAsset22.name = "subspecies_layer";
    pAsset22.unselect_when_window = true;
    pAsset22.multi_toggle = true;
    this.add(pAsset22);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_subspecies_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset23 = new GodPower();
    pAsset23.id = "family_layer";
    pAsset23.name = "family_layer";
    pAsset23.unselect_when_window = true;
    pAsset23.multi_toggle = true;
    this.add(pAsset23);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_family_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset24 = new GodPower();
    pAsset24.id = "language_layer";
    pAsset24.name = "language_layer";
    pAsset24.unselect_when_window = true;
    pAsset24.multi_toggle = true;
    this.add(pAsset24);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_language_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset25 = new GodPower();
    pAsset25.id = "religion_layer";
    pAsset25.name = "religion_layer";
    pAsset25.unselect_when_window = true;
    pAsset25.multi_toggle = true;
    this.add(pAsset25);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_religion_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset26 = new GodPower();
    pAsset26.id = "clan_layer";
    pAsset26.name = "clan_layer";
    pAsset26.unselect_when_window = true;
    pAsset26.multi_toggle = true;
    this.add(pAsset26);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_clan_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset27 = new GodPower();
    pAsset27.id = "kingdom_layer";
    pAsset27.name = "kingdom_layer";
    pAsset27.unselect_when_window = true;
    pAsset27.multi_toggle = true;
    this.add(pAsset27);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_kingdom_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOptionZone);
    GodPower pAsset28 = new GodPower();
    pAsset28.id = "alliance_layer";
    pAsset28.name = "alliance_layer";
    pAsset28.unselect_when_window = true;
    this.add(pAsset28);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_alliance_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset29 = new GodPower();
    pAsset29.id = "army_layer";
    pAsset29.name = "army_layer";
    pAsset29.unselect_when_window = true;
    this.add(pAsset29);
    this.t.tester_enabled = false;
    this.t.map_modes_switch = true;
    this.t.toggle_name = "map_army_layer";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset30 = new GodPower();
    pAsset30.id = "map_kings_leaders";
    pAsset30.name = "map_kings_leaders";
    pAsset30.unselect_when_window = true;
    this.add(pAsset30);
    this.t.tester_enabled = false;
    this.t.toggle_name = "map_kings_leaders";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset31 = new GodPower();
    pAsset31.id = "marks_favorites";
    pAsset31.name = "marks_favorites";
    pAsset31.unselect_when_window = true;
    this.add(pAsset31);
    this.t.tester_enabled = false;
    this.t.toggle_name = "marks_favorites";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset32 = new GodPower();
    pAsset32.id = "marks_favorite_items";
    pAsset32.name = "marks_favorite_items";
    pAsset32.unselect_when_window = true;
    this.add(pAsset32);
    this.t.tester_enabled = false;
    this.t.toggle_name = "marks_favorite_items";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset33 = new GodPower();
    pAsset33.id = "marks_armies";
    pAsset33.name = "Show Armies";
    pAsset33.unselect_when_window = true;
    this.add(pAsset33);
    this.t.tester_enabled = false;
    this.t.toggle_name = "marks_armies";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset34 = new GodPower();
    pAsset34.id = "marks_battles";
    pAsset34.name = "Show Battles";
    pAsset34.unselect_when_window = true;
    this.add(pAsset34);
    this.t.tester_enabled = false;
    this.t.toggle_name = "marks_battles";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset35 = new GodPower();
    pAsset35.id = "marks_plots";
    pAsset35.name = "Plot Icons";
    pAsset35.unselect_when_window = true;
    this.add(pAsset35);
    this.t.tester_enabled = false;
    this.t.toggle_name = "marks_plots";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset36 = new GodPower();
    pAsset36.id = "marks_wars";
    pAsset36.name = "War Icons";
    pAsset36.unselect_when_window = true;
    this.add(pAsset36);
    this.t.tester_enabled = false;
    this.t.toggle_name = "marks_wars";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset37 = new GodPower();
    pAsset37.id = "highlight_kingdom_enemies";
    pAsset37.name = "Highlight Kingdom Enemies";
    pAsset37.unselect_when_window = true;
    this.add(pAsset37);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "highlight_kingdom_enemies";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset38 = new GodPower();
    pAsset38.id = "only_favorited_meta";
    pAsset38.name = "only_favorited_meta";
    pAsset38.unselect_when_window = true;
    this.add(pAsset38);
    this.t.tester_enabled = false;
    this.t.toggle_name = "only_favorited_meta";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset39 = new GodPower();
    pAsset39.id = "unit_metas";
    pAsset39.name = "unit_metas";
    pAsset39.unselect_when_window = true;
    this.add(pAsset39);
    this.t.tester_enabled = false;
    this.t.toggle_name = "unit_metas";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset40 = new GodPower();
    pAsset40.id = "money_flow";
    pAsset40.name = "money_flow";
    pAsset40.unselect_when_window = true;
    this.add(pAsset40);
    this.t.tester_enabled = false;
    this.t.toggle_name = "money_flow";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset41 = new GodPower();
    pAsset41.id = "meta_conversions";
    pAsset41.name = "meta_conversions";
    pAsset41.unselect_when_window = true;
    this.add(pAsset41);
    this.t.tester_enabled = false;
    this.t.toggle_name = "meta_conversions";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset42 = new GodPower();
    pAsset42.id = "talk_bubbles";
    pAsset42.name = "talk_bubbles";
    pAsset42.unselect_when_window = true;
    this.add(pAsset42);
    this.t.tester_enabled = false;
    this.t.toggle_name = "talk_bubbles";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset43 = new GodPower();
    pAsset43.id = "icons_happiness";
    pAsset43.name = "icons_happiness";
    pAsset43.unselect_when_window = true;
    this.add(pAsset43);
    this.t.tester_enabled = false;
    this.t.toggle_name = "icons_happiness";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset44 = new GodPower();
    pAsset44.id = "icons_tasks";
    pAsset44.name = "icons_tasks";
    pAsset44.unselect_when_window = true;
    this.add(pAsset44);
    this.t.tester_enabled = false;
    this.t.toggle_name = "icons_tasks";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset45 = new GodPower();
    pAsset45.id = "army_targets";
    pAsset45.name = "Army Targets";
    pAsset45.unselect_when_window = true;
    this.add(pAsset45);
    this.t.tester_enabled = false;
    this.t.toggle_name = "army_targets";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset46 = new GodPower();
    pAsset46.id = "tooltip_zones";
    pAsset46.name = "Tooltip Zones";
    pAsset46.unselect_when_window = true;
    this.add(pAsset46);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "tooltip_zones";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset47 = new GodPower();
    pAsset47.id = "tooltip_units";
    pAsset47.name = "Tooltip Units";
    pAsset47.unselect_when_window = true;
    this.add(pAsset47);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "tooltip_units";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset48 = new GodPower();
    pAsset48.id = "cursor_arrow_destination";
    pAsset48.name = "cursor_arrow_destination";
    pAsset48.unselect_when_window = true;
    this.add(pAsset48);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "cursor_arrow_destination";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset49 = new GodPower();
    pAsset49.id = "cursor_arrow_lover";
    pAsset49.name = "cursor_arrow_lover";
    pAsset49.unselect_when_window = true;
    this.add(pAsset49);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "cursor_arrow_lover";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset50 = new GodPower();
    pAsset50.id = "cursor_arrow_house";
    pAsset50.name = "cursor_arrow_house";
    pAsset50.unselect_when_window = true;
    this.add(pAsset50);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "cursor_arrow_house";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset51 = new GodPower();
    pAsset51.id = "cursor_arrow_family";
    pAsset51.name = "cursor_arrow_family";
    pAsset51.unselect_when_window = true;
    this.add(pAsset51);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "cursor_arrow_family";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset52 = new GodPower();
    pAsset52.id = "cursor_arrow_parents";
    pAsset52.name = "cursor_arrow_parents";
    pAsset52.unselect_when_window = true;
    this.add(pAsset52);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "cursor_arrow_parents";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset53 = new GodPower();
    pAsset53.id = "cursor_arrow_kids";
    pAsset53.name = "cursor_arrow_kids";
    pAsset53.unselect_when_window = true;
    this.add(pAsset53);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "cursor_arrow_kids";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset54 = new GodPower();
    pAsset54.id = "cursor_arrow_attack_target";
    pAsset54.name = "cursor_arrow_attack_target";
    pAsset54.unselect_when_window = true;
    this.add(pAsset54);
    this.t.disabled_on_mobile = true;
    this.t.tester_enabled = false;
    this.t.toggle_name = "cursor_arrow_attack_target";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset55 = new GodPower();
    pAsset55.id = "marks_boats";
    pAsset55.name = "marks_boats";
    pAsset55.unselect_when_window = true;
    this.add(pAsset55);
    this.t.tester_enabled = false;
    this.t.toggle_name = "marks_boats";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset56 = new GodPower();
    pAsset56.id = "history_log";
    pAsset56.name = "History Log";
    pAsset56.unselect_when_window = true;
    this.add(pAsset56);
    this.t.tester_enabled = false;
    this.t.toggle_name = "history_log";
    this.t.toggle_action += new PowerToggleAction(this.toggleOption);
    GodPower pAsset57 = new GodPower();
    pAsset57.id = "pause";
    pAsset57.name = "Pause";
    pAsset57.unselect_when_window = true;
    pAsset57.can_drag_map = true;
    this.add(pAsset57);
    this.t.tester_enabled = false;
    this.t.activate_on_hotkey_select = false;
    GodPower pAsset58 = new GodPower();
    pAsset58.id = "clock";
    pAsset58.name = "Clock";
    pAsset58.unselect_when_window = true;
    pAsset58.requires_premium = true;
    pAsset58.ignore_cursor_icon = true;
    pAsset58.rank = PowerRank.Rank0_free;
    pAsset58.can_drag_map = true;
    this.add(pAsset58);
    this.t.tester_enabled = false;
    this.t.allow_unit_selection = true;
    GodPower pAsset59 = new GodPower();
    pAsset59.id = "follow_unit";
    pAsset59.name = "follow_unit";
    pAsset59.unselect_when_window = true;
    pAsset59.can_drag_map = true;
    this.add(pAsset59);
    this.t.tester_enabled = false;
    this.t.allow_unit_selection = true;
  }

  private void addCivsAnimals()
  {
    this.clone("civ_cat", "$template_spawn_actor$");
    this.t.name = "civ_cat";
    this.t.actor_asset_id = "civ_cat";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_dog", "$template_spawn_actor$");
    this.t.name = "civ_dog";
    this.t.actor_asset_id = "civ_dog";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_chicken", "$template_spawn_actor$");
    this.t.name = "civ_chicken";
    this.t.actor_asset_id = "civ_chicken";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_rabbit", "$template_spawn_actor$");
    this.t.name = "civ_rabbit";
    this.t.actor_asset_id = "civ_rabbit";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_monkey", "$template_spawn_actor$");
    this.t.name = "civ_monkey";
    this.t.actor_asset_id = "civ_monkey";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_fox", "$template_spawn_actor$");
    this.t.name = "civ_fox";
    this.t.actor_asset_id = "civ_fox";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_sheep", "$template_spawn_actor$");
    this.t.name = "civ_sheep";
    this.t.actor_asset_id = "civ_sheep";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_cow", "$template_spawn_actor$");
    this.t.name = "civ_cow";
    this.t.actor_asset_id = "civ_cow";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_armadillo", "$template_spawn_actor$");
    this.t.name = "civ_armadillo";
    this.t.actor_asset_id = "civ_armadillo";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_wolf", "$template_spawn_actor$");
    this.t.name = "civ_wolf";
    this.t.actor_asset_id = "civ_wolf";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_bear", "$template_spawn_actor$");
    this.t.name = "civ_bear";
    this.t.actor_asset_id = "civ_bear";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_rhino", "$template_spawn_actor$");
    this.t.name = "civ_rhino";
    this.t.actor_asset_id = "civ_rhino";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_buffalo", "$template_spawn_actor$");
    this.t.name = "civ_buffalo";
    this.t.actor_asset_id = "civ_buffalo";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_hyena", "$template_spawn_actor$");
    this.t.name = "civ_hyena";
    this.t.actor_asset_id = "civ_hyena";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_rat", "$template_spawn_actor$");
    this.t.name = "civ_rat";
    this.t.actor_asset_id = "civ_rat";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_alpaca", "$template_spawn_actor$");
    this.t.name = "civ_alpaca";
    this.t.actor_asset_id = "civ_alpaca";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_capybara", "$template_spawn_actor$");
    this.t.name = "civ_capybara";
    this.t.actor_asset_id = "civ_capybara";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_goat", "$template_spawn_actor$");
    this.t.name = "civ_goat";
    this.t.actor_asset_id = "civ_goat";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_crab", "$template_spawn_actor$");
    this.t.name = "civ_crab";
    this.t.actor_asset_id = "civ_crab";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_penguin", "$template_spawn_actor$");
    this.t.name = "civ_penguin";
    this.t.actor_asset_id = "civ_penguin";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_turtle", "$template_spawn_actor$");
    this.t.name = "civ_turtle";
    this.t.actor_asset_id = "civ_turtle";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_crocodile", "$template_spawn_actor$");
    this.t.name = "civ_crocodile";
    this.t.actor_asset_id = "civ_crocodile";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_snake", "$template_spawn_actor$");
    this.t.name = "civ_snake";
    this.t.actor_asset_id = "civ_snake";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_frog", "$template_spawn_actor$");
    this.t.name = "civ_frog";
    this.t.actor_asset_id = "civ_frog";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_piranha", "$template_spawn_actor$");
    this.t.name = "civ_piranha";
    this.t.actor_asset_id = "civ_piranha";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_scorpion", "$template_spawn_actor$");
    this.t.name = "civ_scorpion";
    this.t.actor_asset_id = "civ_scorpion";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_candy_man", "$template_spawn_actor$");
    this.t.name = "civ_candy_man";
    this.t.actor_asset_id = "civ_candy_man";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_crystal_golem", "$template_spawn_actor$");
    this.t.name = "civ_crystal_golem";
    this.t.actor_asset_id = "civ_crystal_golem";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_liliar", "$template_spawn_actor$");
    this.t.name = "civ_liliar";
    this.t.actor_asset_id = "civ_liliar";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_garlic_man", "$template_spawn_actor$");
    this.t.name = "civ_garlic_man";
    this.t.actor_asset_id = "civ_garlic_man";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_lemon_man", "$template_spawn_actor$");
    this.t.name = "civ_lemon_man";
    this.t.actor_asset_id = "civ_lemon_man";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_acid_gentleman", "$template_spawn_actor$");
    this.t.name = "civ_acid_gentleman";
    this.t.actor_asset_id = "civ_acid_gentleman";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_beetle", "$template_spawn_actor$");
    this.t.name = "civ_beetle";
    this.t.actor_asset_id = "civ_beetle";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_seal", "$template_spawn_actor$");
    this.t.name = "civ_seal";
    this.t.actor_asset_id = "civ_seal";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.clone("civ_unicorn", "$template_spawn_actor$");
    this.t.name = "civ_unicorn";
    this.t.actor_asset_id = "civ_unicorn";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
  }

  private void addMobs()
  {
    this.clone("cold_one", "$template_spawn_actor$");
    this.t.name = "Cold Ones";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "cold_one";
    this.clone("demon", "$template_spawn_actor$");
    this.t.name = "Demon";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "demon";
    this.clone("angle", "$template_spawn_actor$");
    this.t.name = "Angle";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "angle";
    this.clone("tumor_monster_unit", "$template_spawn_actor$");
    this.t.name = "Tumor Monster";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_ids = AssetLibrary<GodPower>.a<string>("tumor_monster_unit", "tumor_monster_animal");
    this.clone("mush_unit", "$template_spawn_actor$");
    this.t.name = "Mush";
    this.t.requires_premium = false;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_ids = AssetLibrary<GodPower>.a<string>("mush_unit", "mush_animal");
    this.clone("bioblob", "$template_spawn_actor$");
    this.t.name = "Bioblob";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "bioblob";
    this.clone("lil_pumpkin", "$template_spawn_actor$");
    this.t.name = "Lil Pumpkin";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "lil_pumpkin";
    this.clone("assimilator", "$template_spawn_actor$");
    this.t.name = "Assimilator";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "assimilator";
    this.clone("necromancer", "$template_spawn_actor$");
    this.t.name = "Necromancer";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "necromancer";
    this.clone("druid", "$template_spawn_actor$");
    this.t.name = "Druid";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "druid";
    this.clone("plague_doctor", "$template_spawn_actor$");
    this.t.name = "Plague Doctor";
    this.t.actor_asset_id = "plague_doctor";
    this.clone("evil_mage", "$template_spawn_actor$");
    this.t.name = "Evil Mage";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "evil_mage";
    this.clone("white_mage", "$template_spawn_actor$");
    this.t.name = "White Mage";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_id = "white_mage";
    this.clone("bandit", "$template_spawn_actor$");
    this.t.name = "Bandits";
    this.t.actor_asset_id = "bandit";
    this.clone("snowman", "$template_spawn_actor$");
    this.t.name = "Snowman";
    this.t.actor_asset_id = "snowman";
    this.clone("zombie", "$template_spawn_actor$");
    this.t.name = "Zombie";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.t.actor_asset_ids = AssetLibrary<GodPower>.a<string>("zombie_human", "zombie_orc", "zombie_dwarf", "zombie_elf");
    this.clone("skeleton", "$template_spawn_actor$");
    this.t.name = "Skeleton";
    this.t.rank = PowerRank.Rank0_free;
    this.t.actor_asset_id = "skeleton";
    this.clone("sheep", "$template_spawn_actor$");
    this.t.name = "Sheeps";
    this.t.actor_asset_id = "sheep";
    this.clone("rhino", "$template_spawn_actor$");
    this.t.name = "Rhino";
    this.t.actor_asset_id = "rhino";
    this.clone("monkey", "$template_spawn_actor$");
    this.t.name = "Monkey";
    this.t.actor_asset_id = "monkey";
    this.clone("buffalo", "$template_spawn_actor$");
    this.t.name = "Buffalo";
    this.t.actor_asset_id = "buffalo";
    this.clone("fox", "$template_spawn_actor$");
    this.t.name = "Fox";
    this.t.actor_asset_id = "fox";
    this.clone("hyena", "$template_spawn_actor$");
    this.t.name = "Hyena";
    this.t.actor_asset_id = "hyena";
    this.clone("dog", "$template_spawn_actor$");
    this.t.name = "Dog";
    this.t.actor_asset_id = "dog";
    this.clone("cow", "$template_spawn_actor$");
    this.t.name = "Cow";
    this.t.actor_asset_id = "cow";
    this.clone("frog", "$template_spawn_actor$");
    this.t.name = "Frog";
    this.t.actor_asset_id = "frog";
    this.clone("crocodile", "$template_spawn_actor$");
    this.t.name = "Crocodile";
    this.t.actor_asset_id = "crocodile";
    this.clone("snake", "$template_spawn_actor$");
    this.t.name = "Snake";
    this.t.actor_asset_id = "snake";
    this.clone("turtle", "$template_spawn_actor$");
    this.t.name = "Turtle";
    this.t.actor_asset_id = "turtle";
    this.clone("penguin", "$template_spawn_actor$");
    this.t.name = "Penguin";
    this.t.actor_asset_id = "penguin";
    this.clone("crab", "$template_spawn_actor$");
    this.t.name = "Crab";
    this.t.actor_asset_id = "crab";
    this.clone("rabbit", "$template_spawn_actor$");
    this.t.name = "Rabbit";
    this.t.actor_asset_id = "rabbit";
    this.clone("cat", "$template_spawn_actor$");
    this.t.name = "Cat";
    this.t.actor_asset_id = "cat";
    this.clone("chicken", "$template_spawn_actor$");
    this.t.name = "Chicken";
    this.t.actor_asset_id = "chicken";
    this.clone("wolf", "$template_spawn_actor$");
    this.t.name = "Wolfs";
    this.t.actor_asset_id = "wolf";
    this.clone("armadillo", "$template_spawn_actor$");
    this.t.name = "Armadillo";
    this.t.actor_asset_id = "armadillo";
    this.clone("raccoon", "$template_spawn_actor$");
    this.t.name = "Raccoon";
    this.t.actor_asset_id = "raccoon";
    this.clone("seal", "$template_spawn_actor$");
    this.t.name = "Seal";
    this.t.actor_asset_id = "seal";
    this.clone("ostrich", "$template_spawn_actor$");
    this.t.name = "Ostrich";
    this.t.actor_asset_id = "ostrich";
    this.clone("unicorn", "$template_spawn_actor$");
    this.t.name = "Unicorn";
    this.t.actor_asset_id = "unicorn";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("alpaca", "$template_spawn_actor$");
    this.t.name = "Alpaca";
    this.t.actor_asset_id = "alpaca";
    this.clone("capybara", "$template_spawn_actor$");
    this.t.name = "Capybara";
    this.t.actor_asset_id = "capybara";
    this.clone("scorpion", "$template_spawn_actor$");
    this.t.name = "Scorpion";
    this.t.actor_asset_id = "scorpion";
    this.clone("flower_bud", "$template_spawn_actor$");
    this.t.name = "Flower Bud";
    this.t.actor_asset_id = "flower_bud";
    this.clone("lemon_snail", "$template_spawn_actor$");
    this.t.name = "Bitba";
    this.t.actor_asset_id = "lemon_snail";
    this.clone("garl", "$template_spawn_actor$");
    this.t.name = "Garl";
    this.t.actor_asset_id = "garl";
    this.clone("bear", "$template_spawn_actor$");
    this.t.name = "Bear";
    this.t.actor_asset_id = "bear";
    this.clone("piranha", "$template_spawn_actor$");
    this.t.name = "Piranha";
    this.t.actor_asset_id = "piranha";
    this.clone("worm", "$template_spawn_actor$");
    this.t.name = "Worm";
    this.t.actor_asset_id = "worm";
    this.clone("crystal_sword", "$template_spawn_actor$");
    this.t.name = "Crystal Sword";
    this.t.actor_asset_id = "crystal_sword";
    this.clone("jumpy_skull", "$template_spawn_actor$");
    this.t.name = "Rude Skull";
    this.t.actor_asset_id = "jumpy_skull";
    this.clone("fire_skull", "$template_spawn_actor$");
    this.t.name = "Fire Skull";
    this.t.actor_asset_id = "fire_skull";
    this.clone("fire_elemental", "$template_spawn_actor$");
    this.t.name = "Fire Elemental";
    this.t.actor_asset_ids = SA.fire_elementals;
    this.clone("ghost", "$template_spawn_actor$");
    this.t.name = "Ghost";
    this.t.actor_asset_id = "ghost";
    this.clone("alien", "$template_spawn_actor$");
    this.t.name = "Alien";
    this.t.actor_asset_id = "alien";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank3_good;
    this.clone("greg", "$template_spawn_actor$");
    this.t.name = "Greg";
    this.t.actor_asset_id = "greg";
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank5_noAwards;
    this.clone("smore", "$template_spawn_actor$");
    this.t.name = "Smore";
    this.t.actor_asset_id = "smore";
    this.clone("sand_spider", "$template_spawn_actor$");
    this.t.name = "Sand Spider";
    this.t.actor_asset_id = "sand_spider";
    this.t.hold_action = true;
    this.clone("goat", "$template_spawn_actor$");
    this.t.name = "Goat";
    this.t.actor_asset_id = "goat";
    this.clone("acid_blob", "$template_spawn_actor$");
    this.t.name = "Acid Blob";
    this.t.actor_asset_id = "acid_blob";
    this.clone("god_finger", "$template_spawn_actor$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank2_normal;
    this.t.name = "God Finger";
    this.t.actor_asset_id = "god_finger";
    this.clone("UFO", "$template_spawn_actor$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.name = "UFO";
    this.t.actor_asset_id = "UFO";
    this.clone("dragon", "$template_spawn_actor$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.name = "Dragon";
    this.t.actor_asset_id = "dragon";
    this.clone("fairy", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank2_normal;
    this.t.name = "Fairy";
    this.t.actor_asset_id = "fairy";
    this.t.requires_premium = true;
    this.clone("butterfly", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Butterfly";
    this.t.actor_asset_id = "butterfly";
    this.clone("bee", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Bee";
    this.t.actor_asset_id = "bee";
    this.t.click_action = new PowerActionWithID(this.spawnUnit);
    this.clone("grasshopper", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Grasshopper";
    this.t.actor_asset_id = "grasshopper";
    this.clone("fly", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Fly";
    this.t.actor_asset_id = "fly";
    this.clone("beetle", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Beetle";
    this.t.actor_asset_id = "beetle";
    this.clone("rat", "$template_spawn_actor$");
    this.t.name = "Rat";
    this.t.actor_asset_id = "rat";
    this.clone("ant_blue", "$template_spawn_actor$");
    this.t.name = "Blue Ant";
    this.t.actor_asset_id = "ant_blue";
    this.clone("ant_green", "$template_spawn_actor$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Green Ant";
    this.t.actor_asset_id = "ant_green";
    this.clone("ant_black", "$template_spawn_actor$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Black Ant";
    this.t.actor_asset_id = "ant_black";
    this.clone("ant_red", "$template_spawn_actor$");
    this.t.requires_premium = true;
    this.t.rank = PowerRank.Rank1_common;
    this.t.name = "Red Ant";
    this.t.actor_asset_id = "ant_red";
    this.clone("crabzilla", "$template_spawn_actor$");
    this.t.name = "Crabzilla";
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.requires_premium = true;
    this.t.actor_asset_id = "crabzilla";
    this.t.actor_spawn_height = 0.0f;
    this.t.ignore_fast_spawn = true;
    this.t.tester_enabled = false;
    this.t.multiple_spawn_tip = false;
    this.t.click_action = new PowerActionWithID(this.spawnCrabzilla);
  }

  private void addCivsClassic()
  {
    GodPower pAsset = new GodPower();
    pAsset.id = "$template_spawn_actor$";
    pAsset.type = PowerActionType.PowerSpawnActor;
    pAsset.rank = PowerRank.Rank0_free;
    pAsset.unselect_when_window = true;
    pAsset.show_spawn_effect = true;
    pAsset.actor_spawn_height = 3f;
    pAsset.multiple_spawn_tip = true;
    pAsset.show_unit_stats_overview = true;
    pAsset.set_used_camera_drag_on_long_move = true;
    this.add(pAsset);
    this.t.click_action = new PowerActionWithID(this.spawnUnit);
    this.clone("human", "$template_spawn_actor$");
    this.t.name = "Human";
    this.t.actor_asset_id = "human";
    this.clone("orc", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.requires_premium = true;
    this.t.name = "Orc";
    this.t.actor_asset_id = "orc";
    this.clone("elf", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.requires_premium = true;
    this.t.name = "Elf";
    this.t.actor_asset_id = "elf";
    this.clone("dwarf", "$template_spawn_actor$");
    this.t.rank = PowerRank.Rank4_awesome;
    this.t.requires_premium = true;
    this.t.name = "Dwarf";
    this.t.actor_asset_id = "dwarf";
  }

  public override void linkAssets()
  {
    foreach (GodPower godPower in this.list)
    {
      if (!string.IsNullOrEmpty(godPower.drop_id))
        godPower.cached_drop_asset = AssetManager.drops.get(godPower.drop_id);
      if (!string.IsNullOrEmpty(godPower.tile_type))
        godPower.cached_tile_type_asset = AssetManager.tiles.get(godPower.tile_type);
      if (!string.IsNullOrEmpty(godPower.top_tile_type))
        godPower.cached_top_tile_type_asset = AssetManager.top_tiles.get(godPower.top_tile_type);
      if (godPower.actor_asset_id != null)
      {
        ActorAsset actorAsset = AssetManager.actor_library.get(godPower.actor_asset_id);
        if (actorAsset.power_id == null)
          actorAsset.power_id = godPower.id;
      }
      string[] actorAssetIds = godPower.actor_asset_ids;
      if ((actorAssetIds != null ? (actorAssetIds.Length != 0 ? 1 : 0) : 0) != 0)
      {
        foreach (string actorAssetId in godPower.actor_asset_ids)
        {
          ActorAsset actorAsset = AssetManager.actor_library.get(actorAssetId);
          if (actorAsset.power_id == null)
            actorAsset.power_id = godPower.id;
        }
      }
    }
  }

  private void traceRanks(PowerButton pTarget)
  {
    string str1 = "";
    string str2 = "";
    string str3 = "";
    string str4 = "";
    string str5 = "";
    for (int index = 0; index < this.list.Count; ++index)
    {
      GodPower godPower = this.list[index];
      switch (godPower.rank)
      {
        case PowerRank.Rank0_free:
          str1 = $"{str1}{godPower.name}, ";
          break;
        case PowerRank.Rank1_common:
          str2 = $"{str2}{godPower.name}, ";
          break;
        case PowerRank.Rank2_normal:
          str3 = $"{str3}{godPower.name}, ";
          break;
        case PowerRank.Rank3_good:
          str4 = $"{str4}{godPower.name}, ";
          break;
        case PowerRank.Rank4_awesome:
          str5 = $"{str5}{godPower.name}, ";
          break;
      }
    }
    Debug.Log((object) ("rank 0: " + str1));
    Debug.Log((object) ("rank 1: " + str2));
    Debug.Log((object) ("rank 2: " + str3));
    Debug.Log((object) ("rank 3: " + str4));
    Debug.Log((object) ("rank 4: " + str5));
  }

  private bool spawnDrops(WorldTile tTile, GodPower pPower)
  {
    BrushData currentBrushData = Config.current_brush_data;
    bool flag = false;
    if (currentBrushData.size == 0 && currentBrushData.fast_spawn)
    {
      if ((double) World.world.player_control.timer_spawn_pixels <= 0.0)
      {
        World.world.player_control.timer_spawn_pixels = 0.5f;
        flag = true;
      }
    }
    else if (currentBrushData.fast_spawn && Randy.randomBool())
    {
      if ((double) World.world.player_control.timer_spawn_pixels <= 0.0)
      {
        World.world.player_control.timer_spawn_pixels = 0.3f;
        flag = true;
      }
    }
    else
      flag = Randy.randomChance(pPower.falling_chance);
    if (World.world.player_control.first_click)
    {
      World.world.player_control.first_click = false;
      flag = true;
      World.world.player_control.timer_spawn_pixels = 0.3f;
    }
    if (flag)
      World.world.drop_manager.spawn(tTile, pPower.cached_drop_asset, pForceSurprise: true).soundOn = true;
    return true;
  }

  private bool spawnPrinter(WorldTile pTile, string pPower)
  {
    GodPower pCheckData = this.get(pPower);
    EffectsLibrary.spawn("fx_spawn", pTile);
    World.world.units.spawnNewUnitByPlayer("printer", pTile, true).data.set("template", pCheckData.printers_print);
    AchievementLibrary.print_heart.check((object) pCheckData);
    return true;
  }

  private bool useMagnet(WorldTile pTile, string pPower)
  {
    World.world.magnet.magnetAction(false, pTile);
    return true;
  }

  private bool spawnCloudSnow(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_snow");
    return true;
  }

  private bool spawnCloudLava(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_lava");
    return true;
  }

  private bool spawnCloudAcid(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_acid");
    return true;
  }

  private bool spawnCloudOfLife(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_normal");
    return true;
  }

  private bool spawnCloudRain(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_rain");
    return true;
  }

  private bool spawnCloudFire(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_fire");
    return true;
  }

  private bool spawnCloudLightning(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_lightning");
    return true;
  }

  private bool spawnCloudMagic(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_magic");
    return true;
  }

  private bool spawnCloudRage(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_rage");
    return true;
  }

  private bool spawnCloudAsh(WorldTile pTile, string pPower)
  {
    this.spawnCloud(pTile, "cloud_ash");
    return true;
  }

  private void spawnCloud(WorldTile pTile, string pCloudID)
  {
    EffectsLibrary.spawn("fx_cloud", pTile, pCloudID);
    Vector2Int pos = pTile.pos;
    double x = (double) ((Vector2Int) ref pos).x;
    pos = pTile.pos;
    double y = (double) ((Vector2Int) ref pos).y;
    MusicBox.playSound("event:/SFX/UNIQUE/SpawnCloud", (float) x, (float) y);
  }

  private bool spawnCrabzilla(WorldTile pTile, string pPower)
  {
    World.world.player_control.already_used_power = false;
    World.world.selected_buttons.unselectAll();
    ((SpawnEffect) EffectsLibrary.spawn("fx_spawn_big", pTile)).setEvent("crabzilla", pTile);
    return true;
  }

  private bool spawnLightning(WorldTile pTile, string pPower)
  {
    MapBox.spawnLightningBig(pTile);
    return true;
  }

  private bool spawnForce(WorldTile pTile, string pPower)
  {
    MusicBox.playSound("event:/SFX/EXPLOSIONS/ExplosionForce", pTile);
    World.world.applyForceOnTile(pTile, pForceAmount: 3f, pChangeHappiness: true);
    EffectsLibrary.spawnExplosionWave(pTile.posV3, 10f);
    return true;
  }

  private bool fingerFlick(WorldTile pTile, string pPower)
  {
    Actor actorNearCursor = World.world.getActorNearCursor();
    if (actorNearCursor == null)
      return false;
    Vector2 mousePos = World.world.getMousePos();
    Vector2 currentPosition = actorNearCursor.current_position;
    float pForceAmountDirection = Randy.randomFloat(2.5f, 5f);
    float pForceHeight = Randy.randomFloat(2.5f, 3f);
    actorNearCursor.calculateForce(currentPosition.x, currentPosition.y, mousePos.x, mousePos.y, pForceAmountDirection, pForceHeight, true);
    actorNearCursor.addStatusEffect("flicked");
    actorNearCursor.makeStunned();
    return true;
  }

  private bool spawnInfinityCoin(WorldTile pTile, string pPower)
  {
    EffectsLibrary.spawn("fx_infinity_coin", pTile);
    return true;
  }

  private bool spawnEarthquake(WorldTile pTile, string pPower)
  {
    Earthquake.startQuake(pTile);
    return true;
  }

  private bool spawnMeteorite(WorldTile pTile, string pPower)
  {
    Meteorite.spawnMeteorite(pTile);
    return true;
  }

  private bool spawnTornado(WorldTile pTile, string pPower)
  {
    EffectsLibrary.spawnAtTile("fx_tornado", pTile, 0.5f);
    return true;
  }

  private bool prepareBoulder(WorldTile pTile, string pPower)
  {
    Touch pTouch = new Touch();
    Vector2 pPosition;
    if (InputHelpers.mouseSupported)
    {
      pPosition = World.world.getMousePos();
    }
    else
    {
      if (!World.world.player_control.getTouchPos(out pTouch, true))
        return false;
      pPosition = Vector2.op_Implicit(World.world.camera.ScreenToWorldPoint(Vector2.op_Implicit(((Touch) ref pTouch).position)));
    }
    Boulder.chargeBoulder(pPosition, pTouch);
    return true;
  }

  private bool spawnSanta(WorldTile pTile, string pPower)
  {
    EffectsLibrary.spawn("fx_santa", pTile, "santa");
    return true;
  }

  private void toggleOptionZone(string pPower)
  {
    GodPower pPower1 = AssetManager.powers.get(pPower);
    MetaTypeAsset fromPower = AssetManager.meta_type_library.getFromPower(pPower1);
    if (InputHelpers.GetMouseButtonUp(1))
      fromPower.toggleOptionZone(pPower1, -1);
    else
      fromPower.toggleOptionZone(pPower1);
  }

  internal void toggleMultiOption(string pPower)
  {
    GodPower godPower = AssetManager.powers.get(pPower);
    string toggleName = godPower.toggle_name;
    OptionAsset optionAsset = AssetManager.options_library.get(toggleName);
    int num = !InputHelpers.GetMouseButtonUp(1) ? 1 : -1;
    PlayerOptionData data = optionAsset.data;
    if (data.boolVal)
    {
      data.intVal += num;
      if (data.intVal > optionAsset.max_value)
      {
        data.intVal = 0;
        data.boolVal = false;
      }
      if (data.intVal < 0)
        data.intVal = optionAsset.max_value;
    }
    else
      data.boolVal = true;
    PlayerConfig.saveData();
    string translatedName = godPower.getTranslatedName();
    string translatedDescription = godPower.getTranslatedDescription();
    string translatedOption = optionAsset.getTranslatedOption();
    if (!data.boolVal)
      return;
    WorldTip.instance.showToolbarText($"{translatedName} - {translatedOption}", translatedDescription);
  }

  private void toggleOption(string pPower)
  {
    GodPower pPower1 = AssetManager.powers.get(pPower);
    WorldTip.instance.showToolbarText(pPower1);
    PlayerOptionData playerOptionData = PlayerConfig.dict[pPower1.toggle_name];
    playerOptionData.boolVal = !playerOptionData.boolVal;
    if (pPower1.map_modes_switch)
    {
      if (playerOptionData.boolVal)
        PowerLibrary.disableAllOtherMapModes(pPower);
      else
        WorldTip.instance.startHide();
    }
    PlayerConfig.saveData();
  }

  internal static void disableAllOtherMapModes(string pMainPower)
  {
    for (int index = 0; index < AssetManager.powers.list.Count; ++index)
    {
      GodPower godPower = AssetManager.powers.list[index];
      if (godPower.map_modes_switch && !(godPower.id == pMainPower))
        PlayerConfig.dict[godPower.toggle_name].boolVal = false;
    }
  }

  private bool useVortex(WorldTile pTile, string pPower)
  {
    if (pTile.isTemporaryFrozen())
      pTile.unfreeze(99);
    VortexAction.moveTiles(pTile, Config.current_brush_data);
    return true;
  }

  private bool drawTiles(WorldTile pTile, string pPowerID)
  {
    GodPower godPower = this.get(pPowerID);
    TileType cachedTileTypeAsset = godPower.cached_tile_type_asset;
    TopTileType topTileTypeAsset = godPower.cached_top_tile_type_asset;
    World.world.flash_effects.flashPixel(pTile, 25);
    if (topTileTypeAsset != null && topTileTypeAsset.wall && pTile.Type.id != topTileTypeAsset.id)
    {
      ++World.world.game_stats.data.wallsPlaced;
      AchievementLibrary.segregator.check();
    }
    MapAction.terraformTile(pTile, cachedTileTypeAsset, topTileTypeAsset, TerraformLibrary.draw);
    return true;
  }

  private bool flashPixel(WorldTile pTile, string pPowerID = null)
  {
    World.world.flash_effects.flashPixel(pTile, 10);
    return true;
  }

  private bool flashPixel(WorldTile pTile, GodPower pPower)
  {
    World.world.flash_effects.flashPixel(pTile, 10);
    return true;
  }

  private bool drawTemperaturePlus(WorldTile pTile, string pPower)
  {
    if (pTile.isTemporaryFrozen() && Randy.randomBool())
      pTile.unfreeze();
    WorldBehaviourUnitTemperatures.checkTile(pTile, 5);
    if (pTile.Type.lava)
      LavaHelper.heatUpLava(pTile);
    if (pTile.hasBuilding() && pTile.building.asset.spawn_drops)
      pTile.building.data.removeFlag("stop_spawn_drops");
    return true;
  }

  public bool clickHideUI(string pPowerId)
  {
    if (ScrollWindow.isWindowActive())
      return true;
    Config.ui_main_hidden = true;
    return true;
  }

  public bool clickTraitEditorRainButton(string pPowerId)
  {
    Config.selected_trait_editor = pPowerId;
    ScrollWindow.showWindow("trait_rain_editor");
    return true;
  }

  public bool clickEquipmentEditorRainButton(string pPowerId)
  {
    ScrollWindow.showWindow("equipment_rain_editor");
    return true;
  }

  public static bool drawTemperatureMinus(WorldTile pTile, string pPower)
  {
    if (pTile.Type.lava)
      LavaHelper.coolDownLava(pTile);
    if (pTile.isOnFire())
      pTile.stopFire();
    if (pTile.canBeFrozen() && Randy.randomBool())
    {
      if (pTile.health > 0)
        --pTile.health;
      else
        pTile.freeze();
    }
    WorldBehaviourUnitTemperatures.checkTile(pTile, -5);
    if (pTile.hasBuilding())
      ActionLibrary.addFrozenEffectOnTarget((BaseSimObject) null, (BaseSimObject) pTile.building);
    if (pTile.hasBuilding() && pTile.building.asset.spawn_drops)
      pTile.building.data.addFlag("stop_spawn_drops");
    return true;
  }

  private bool drawShovelPlus(WorldTile pTile, string pPower)
  {
    if (pTile.health > 0)
      --pTile.health;
    else
      MapAction.increaseTile(pTile, false, "destroy");
    return false;
  }

  private bool drawShovelMinus(WorldTile pTile, string pPower)
  {
    if (pTile.health > 0)
      --pTile.health;
    else
      MapAction.decreaseTile(pTile, false, "destroy");
    return false;
  }

  private bool drawGreyGoo(WorldTile pTile, string pPower)
  {
    World.world.grey_goo_layer.add(pTile);
    return false;
  }

  private bool drawConway(WorldTile pTile, string pPower)
  {
    if (Randy.randomBool())
      World.world.conway_layer.add(pTile, "conway");
    return false;
  }

  private bool drawConwayInverse(WorldTile pTile, string pPower)
  {
    if (Randy.randomBool())
      World.world.conway_layer.add(pTile, "conway_inverse");
    return false;
  }

  private bool drawFinger(WorldTile pTile, string pPower)
  {
    TileType firstPressedType = World.world.player_control.first_pressed_type;
    TopTileType pTopType = World.world.player_control.first_pressed_top_type;
    if (pTopType != null && !pTopType.allowed_to_be_finger_copied)
      pTopType = (TopTileType) null;
    if (firstPressedType.ground && (pTopType == null || pTopType.ground))
    {
      MapAction.terraformTile(pTile, firstPressedType, pTopType, TerraformLibrary.draw);
    }
    else
    {
      this.destroyBuildings(pTile, pPower);
      MapAction.terraformTile(pTile, firstPressedType, pTopType, TerraformLibrary.destroy_no_flash);
    }
    if (pTile.Type.grey_goo)
      World.world.grey_goo_layer.add(pTile);
    if (pTopType != null && pTopType.biome_id == "biome_grass")
      AchievementLibrary.touch_the_grass.check();
    return false;
  }

  private bool drawBorderBrush(WorldTile pTile, string pPower)
  {
    WorldTile firstPressedTile = World.world.player_control.first_pressed_tile;
    if (firstPressedTile == null)
      return false;
    City zoneCity = firstPressedTile.zone_city;
    if (zoneCity == null)
      return false;
    zoneCity.addZone(pTile.zone);
    World.world.city_zone_helper.city_place_finder.setDirty();
    zoneCity.setAbandonedZonesDirty();
    return false;
  }

  private bool spawnUnit(WorldTile pTile, string pPowerID)
  {
    GodPower godPower = this.get(pPowerID);
    Vector2Int pos = pTile.pos;
    double x = (double) ((Vector2Int) ref pos).x;
    pos = pTile.pos;
    double y = (double) ((Vector2Int) ref pos).y;
    MusicBox.playSound("event:/SFX/UNIQUE/SpawnWhoosh", (float) x, (float) y);
    if (godPower.id == "sheep" && pTile.Type.lava)
      AchievementLibrary.sacrifice.check();
    EffectsLibrary.spawn("fx_spawn", pTile);
    string[] actorAssetIds = godPower.actor_asset_ids;
    Actor pCheckData = World.world.units.spawnNewUnitByPlayer((actorAssetIds != null ? (actorAssetIds.Length != 0 ? 1 : 0) : 0) == 0 ? godPower.actor_asset_id : godPower.actor_asset_ids.GetRandom<string>(), pTile, true, true, godPower.actor_spawn_height);
    AchievementLibrary.back_to_beta_testing.check((object) pCheckData);
    return true;
  }

  private bool divineLightFX(WorldTile pCenterTile, string pPowerID)
  {
    World.world.fx_divine_light.playOn(pCenterTile);
    return true;
  }

  private bool drawDivineLight(WorldTile pCenterTile, string pPowerID)
  {
    pCenterTile.doUnits((Action<Actor>) (pActor =>
    {
      this.clearBadTraitsFrom(pActor);
      if (pActor.asset.can_be_killed_by_divine_light)
        pActor.getHit((float) pActor.getMaxHealthPercent(0.4f), pAttackType: AttackType.Divine);
      else
        pActor.startColorEffect();
      pActor.finishStatusEffect("ash_fever");
      pActor.finishAngryStatus();
      if (!pActor.isInLiquid())
        pActor.cancelAllBeh();
      if (!pActor.hasPlot())
        return;
      World.world.plots.cancelPlot(pActor.plot);
    }));
    return true;
  }

  private void clearBadTraitsFrom(Actor pActor)
  {
    using (ListPool<ActorTrait> pTraits = new ListPool<ActorTrait>())
    {
      foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pActor.getTraits())
      {
        if (trait.can_be_removed_by_divine_light)
          pTraits.Add(trait);
      }
      if (pTraits.Count <= 0)
        return;
      pActor.removeTraits((ICollection<ActorTrait>) pTraits);
      pActor.setStatsDirty();
      pActor.changeHappiness("just_felt_the_divine");
    }
  }

  private bool cleanBurnedTile(WorldTile pTile, string pPowerID)
  {
    pTile.removeBurn();
    return true;
  }

  private bool removeTornadoes(WorldTile pTile, string pPowerID)
  {
    // ISSUE: unable to decompile the method.
  }

  private bool drawPickaxe(WorldTile pTile, string pPowerID)
  {
    if (pTile.hasBuilding() && pTile.building.asset.building_type == BuildingType.Building_Mineral)
      pTile.building.startDestroyBuilding();
    if (pTile.Type.can_be_removed_with_pickaxe)
      MapAction.decreaseTile(pTile, false, "remove");
    return true;
  }

  private bool drawBucket(WorldTile pTile, string pPowerID)
  {
    MapAction.removeLiquid(pTile);
    if (pTile.Type.lava)
      MapAction.decreaseTile(pTile, false);
    if (pTile.Type.can_be_removed_with_bucket)
      MapAction.decreaseTile(pTile, false);
    return true;
  }

  private bool drawAxe(WorldTile pTile, string pPowerID)
  {
    if (pTile.hasBuilding())
    {
      Building building = pTile.building;
      BuildingAsset asset = building.asset;
      if (asset.building_type == BuildingType.Building_Tree && !building.chopped)
      {
        if (asset.resources_given != null && pTile.hasCity())
        {
          foreach (ResourceContainer resourceContainer in asset.resources_given)
            pTile.zone_city.addResourcesToRandomStockpile(resourceContainer.id, resourceContainer.amount);
        }
        building.chopTree();
      }
    }
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 0))
    {
      if (!(actor.kingdom.name != "living_plants"))
        actor.a.getHitFullHealth(AttackType.Divine);
    }
    if (pTile.Type.can_be_removed_with_axe)
      MapAction.decreaseTile(pTile, false, "remove");
    return true;
  }

  private bool drawSpade(WorldTile pTile, string pPowerID)
  {
    if (pTile.Type.can_be_removed_with_spade)
      MapAction.removeGreens(pTile);
    return true;
  }

  private bool drawSickle(WorldTile pTile, string pPowerID)
  {
    if (pTile.hasBuilding())
    {
      switch (pTile.building.asset.building_type)
      {
        case BuildingType.Building_Fruits:
        case BuildingType.Building_Wheat:
        case BuildingType.Building_Plant:
          pTile.building.startDestroyBuilding();
          break;
      }
    }
    if (pTile.Type.can_be_removed_with_sickle)
      MapAction.decreaseTile(pTile, false, "remove");
    return true;
  }

  private bool drawDemolish(WorldTile pTile, string pPowerID)
  {
    if (pTile.hasBuilding() && pTile.building.asset.can_be_demolished)
      pTile.building.startDestroyBuilding();
    if (pTile.Type.can_be_removed_with_demolish)
      MapAction.decreaseTile(pTile, false);
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 0))
    {
      if (!(actor.kingdom.name != "living_houses"))
        actor.a.getHitFullHealth(AttackType.Divine);
    }
    return true;
  }

  private bool drawScissors(WorldTile pTile, string pPowerID)
  {
    if (pTile.zone.hasCity())
      pTile.zone.city.removeZone(pTile.zone);
    return true;
  }

  private bool drawLifeEraser(WorldTile pTile, string pPowerID)
  {
    MapAction.removeLifeFromTile(pTile);
    return true;
  }

  private bool drawHeatray(WorldTile pTile, string pPowerID)
  {
    if (World.world.heat_ray_fx.isReady())
      World.world.heat.addTile(pTile, Randy.randomInt(1, 3));
    return true;
  }

  [ClickActionCaller]
  private bool heatrayFX(WorldTile pTile, string pPowerID)
  {
    if (World.world.heat_ray_fx.isReady())
      MusicBox.inst.playDrawingSound("event:/SFX/POWERS/HeatRayMelts", (float) pTile.x, (float) pTile.y);
    World.world.heat_ray_fx.play(Vector2Int.op_Implicit(pTile.pos), 10);
    this.loopWithBrush(pTile, pPowerID);
    return true;
  }

  [ClickActionCaller]
  private bool loopWithCurrentBrush(WorldTile pCenterTile, string pPowerID)
  {
    GodPower pPower = this.get(pPowerID);
    this.loopWithBrush(pCenterTile, pPower);
    if (pPower.surprises_units)
      ActionLibrary.suprisedByArchitector((BaseSimObject) null, pCenterTile);
    return true;
  }

  [ClickActionCaller]
  private bool drawingCursorEffect(WorldTile pTile, string pPowerID)
  {
    EffectsLibrary.spawnAt("fx_spark", pTile.posV3, 0.2f);
    return true;
  }

  private bool flashBrushPixelsDuringClick(WorldTile pCenterTile, string pPower)
  {
    BrushData currentBrushData = Config.current_brush_data;
    World.world.highlightTilesBrush(pCenterTile, currentBrushData, new PowerAction(this.flashPixel));
    return true;
  }

  private bool flashBrushPixelsDuringClick(WorldTile pCenterTile, GodPower pPower)
  {
    BrushData currentBrushData = Config.current_brush_data;
    World.world.highlightTilesBrush(pCenterTile, currentBrushData, new PowerAction(this.flashPixel), pPower);
    return true;
  }

  [ClickPowerActionCaller]
  private bool loopWithCurrentBrushPowerForDropsFull(WorldTile pCenterTile, GodPower pPower)
  {
    BrushData currentBrushData = Config.current_brush_data;
    WorldBehaviourTileEffects.checkTileForEffectKill(pCenterTile, currentBrushData.size);
    World.world.loopWithBrushPowerForDropsFull(pCenterTile, currentBrushData, pPower.click_power_action, pPower);
    return true;
  }

  [ClickPowerActionCaller]
  private bool loopWithCurrentBrushPowerForDropsRandom(WorldTile pCenterTile, GodPower pPower)
  {
    BrushData currentBrushData = Config.current_brush_data;
    WorldBehaviourTileEffects.checkTileForEffectKill(pCenterTile, currentBrushData.size);
    World.world.loopWithBrushPowerForDropsRandom(pCenterTile, currentBrushData, pPower.click_power_action, pPower);
    return true;
  }

  [ClickActionCaller]
  private bool loopWithBrush(WorldTile pCenterTile, string pPowerID)
  {
    GodPower pPower = this.get(pPowerID);
    return this.loopWithBrush(pCenterTile, pPower);
  }

  [ClickActionCaller]
  private bool loopWithBrush(WorldTile pCenterTile, GodPower pPower)
  {
    string pID = Config.current_brush;
    if (!string.IsNullOrEmpty(pPower.force_brush))
      pID = pPower.force_brush;
    BrushData pBrush = Brush.get(pID);
    WorldBehaviourTileEffects.checkTileForEffectKill(pCenterTile, pBrush.size);
    World.world.loopWithBrush(pCenterTile, pBrush, pPower.click_action, pPower.id);
    return true;
  }

  private bool stopFire(WorldTile pTile, string pPowerID)
  {
    pTile.stopFire();
    if (pTile.hasBuilding() && pTile.building.hasStatus("burning"))
      pTile.building.stopFire();
    return true;
  }

  private bool fmodDrawingSound(WorldTile pTile, GodPower pPower)
  {
    if (pPower.has_sound_drawing)
      MusicBox.inst.playDrawingSound(pPower.sound_drawing, (float) pTile.x, (float) pTile.y);
    return true;
  }

  private bool fmodDrawingSound(WorldTile pTile, string pPowerID)
  {
    GodPower pPower = this.get(pPowerID);
    this.fmodDrawingSound(pTile, pPower);
    return true;
  }

  private bool destroyBuildings(WorldTile pTile, string pPowerID)
  {
    if (!pTile.hasBuilding())
      return false;
    pTile.building.startDestroyBuilding();
    return true;
  }

  private bool removeClouds(WorldTile pTile, string pPowerID)
  {
    List<BaseEffect> list = World.world.stack_effects.get("fx_cloud").getList();
    float num1 = (float) (10 * (Config.current_brush_data.size + 1));
    float num2 = num1 * num1;
    for (int index = 0; index < list.Count; ++index)
    {
      BaseEffect baseEffect = list[index];
      if (baseEffect.active)
      {
        Vector3 localPosition = ((Component) baseEffect).transform.localPosition;
        if ((double) Toolbox.SquaredDist(localPosition.x, localPosition.y, (float) pTile.x, (float) pTile.y) <= (double) num2)
          baseEffect.startToDie();
      }
    }
    return true;
  }

  private bool removeGoo(WorldTile pTile, string pPowerID)
  {
    if (pTile.Type.grey_goo)
      MapAction.decreaseTile(pTile, false);
    return true;
  }

  private bool removeBuildingsBySponge(WorldTile pTile, string pPowerID)
  {
    if (!pTile.hasBuilding())
      return false;
    bool flag = false;
    if (pTile.building.isRuin() || pTile.building.asset.removed_by_sponge)
      flag = true;
    if (flag)
      pTile.building.startDestroyBuilding();
    return true;
  }

  public override void editorDiagnosticLocales()
  {
    foreach (GodPower godPower in this.list)
    {
      if (godPower.show_tool_sizes && !string.IsNullOrEmpty(godPower.force_brush))
        BaseAssetLibrary.logAssetError($"<e>PowerLibrary</e>: <b>show_tool_sizes</b> is enabled - but <b>force_brush</b> is set to <b>{godPower.force_brush}</b> - making the tool sizes useless", godPower.id);
      if (godPower.show_tool_sizes && godPower.click_brush_action == null && godPower.click_power_brush_action == null)
        BaseAssetLibrary.logAssetError("<e>PowerLibrary</e>: <b>show_tool_sizes</b> is enabled - but <b>click_brush_action</b> and <b>click_power_brush_action</b> are not set - making the tool sizes useless", godPower.id);
    }
    this.localeChecks();
    this.callbackChecks();
    base.editorDiagnosticLocales();
  }

  private void localeChecks()
  {
    foreach (GodPower pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }

  private void callbackChecks()
  {
    foreach (GodPower godPower in this.list)
    {
      if (godPower.click_action != null)
      {
        if (godPower.click_brush_action != null)
        {
          bool flag = false;
          foreach (Delegate invocation in godPower.click_brush_action.GetInvocationList())
          {
            if (invocation.Method.GetCustomAttributes(typeof (ClickActionCallerAttribute), true).Length != 0)
              flag = true;
          }
          if (!flag)
          {
            string str = godPower.click_action.AsString<PowerActionWithID>();
            BaseAssetLibrary.logAssetError($"<e>PowerLibrary</e>: <b>click_brush_action</b> ({godPower.click_brush_action.AsString<PowerActionWithID>()}) overrides <b>click_action</b> ({str}) - either add <b>loopWithBrush</b> which will call them - or mark a similar caller method with [ClickActionCaller] attribute", godPower.id);
          }
        }
        foreach (Delegate invocation in godPower.click_action.GetInvocationList())
        {
          if (invocation.Method.GetCustomAttributes(typeof (ClickActionCallerAttribute), true).Length != 0)
            BaseAssetLibrary.logAssetError($"<e>PowerLibrary</e>: <b>click_action</b> ({godPower.click_action.AsString<PowerActionWithID>()}) has [ClickActionCaller] attribute - it should be used only in <b>click_brush_action</b>", godPower.id);
        }
      }
      if (godPower.click_power_action != null)
      {
        if (godPower.click_power_brush_action != null)
        {
          bool flag = false;
          foreach (Delegate invocation in godPower.click_power_brush_action.GetInvocationList())
          {
            if (invocation.Method.GetCustomAttributes(typeof (ClickPowerActionCallerAttribute), true).Length != 0)
              flag = true;
          }
          if (!flag)
          {
            string str = godPower.click_power_action.AsString<PowerAction>();
            BaseAssetLibrary.logAssetError($"<e>PowerLibrary</e>: <b>click_power_brush_action</b> ({godPower.click_power_brush_action.AsString<PowerAction>()}) overrides <b>click_power_action</b> ({str}) - either add <b>loopWithCurrentBrushPower</b> which will call them - or mark a similar caller method with [ClickPowerActionCaller] attribute", godPower.id);
          }
        }
        foreach (Delegate invocation in godPower.click_power_action.GetInvocationList())
        {
          if (invocation.Method.GetCustomAttributes(typeof (ClickPowerActionCallerAttribute), true).Length != 0)
            BaseAssetLibrary.logAssetError($"<e>PowerLibrary</e>: <b>click_power_action</b> ({godPower.click_power_action.AsString<PowerAction>()}) has [ClickPowerActionCaller] attribute - it should be used only in <b>click_power_brush_action</b>", godPower.id);
        }
      }
    }
  }

  public string addToGameplayReport(string pWhat)
  {
    string str1 = $"{string.Empty}{pWhat}\n";
    foreach (GodPower godPower in this.list)
    {
      string translatedName = godPower.getTranslatedName();
      string translatedDescription = godPower.getTranslatedDescription();
      string str2 = "\n" + translatedName + "\n";
      if (!string.IsNullOrEmpty(translatedDescription))
        str2 = $"{str2}1: {translatedDescription}";
      str1 += str2;
    }
    return str1 + "\n\n";
  }
}
