// Decompiled with JetBrains decompiler
// Type: DropsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityPools;

#nullable disable
public class DropsLibrary : AssetLibrary<DropAsset>
{
  private const string TEMPLATE_BIOME_SEEDS = "$biome_seeds$";
  private const string TEMPLATE_SPAWN_BUILDING = "$spawn_building$";
  private const string TEMPLATE_SPAWN_MINERAL = "$spawn_mineral$";
  private const string TEMPLATE_SPAWN_CREEP = "$spawn_creep$";
  private static HashSet<TileZone> _paint_zones_hashset = new HashSet<TileZone>();

  public override void init()
  {
    base.init();
    DropAsset pAsset1 = new DropAsset();
    pAsset1.id = "paint";
    pAsset1.path_texture = "drops/drop_paint";
    pAsset1.animated = true;
    pAsset1.animation_speed = 0.03f;
    pAsset1.default_scale = 0.1f;
    pAsset1.action_landed = new DropsAction(DropsLibrary.action_paint);
    pAsset1.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset1.type = DropType.DropMagic;
    this.add(pAsset1);
    DropAsset pAsset2 = new DropAsset();
    pAsset2.id = "dust_black";
    pAsset2.path_texture = "drops/drop_dust_black";
    pAsset2.animated = true;
    pAsset2.animation_speed = 0.03f;
    pAsset2.default_scale = 0.1f;
    pAsset2.material = "mat_world_object_lit";
    pAsset2.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset2.type = DropType.DropMagic;
    this.add(pAsset2);
    this.t.action_landed += new DropsAction(DropsLibrary.action_dust_white);
    this.t.action_landed += new DropsAction(DropsLibrary.action_dust_red);
    this.t.action_landed += new DropsAction(DropsLibrary.action_dust_blue);
    this.t.action_landed += new DropsAction(DropsLibrary.action_dust_gold);
    this.t.action_landed += new DropsAction(DropsLibrary.action_dust_purple);
    this.t.action_landed += new DropsAction(DropsLibrary.action_dust_black);
    DropAsset pAsset3 = new DropAsset();
    pAsset3.id = "dust_white";
    pAsset3.path_texture = "drops/drop_dust_white";
    pAsset3.animated = true;
    pAsset3.animation_speed = 0.03f;
    pAsset3.default_scale = 0.1f;
    pAsset3.action_landed = new DropsAction(DropsLibrary.action_dust_white);
    pAsset3.material = "mat_world_object_lit";
    pAsset3.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset3.type = DropType.DropMagic;
    this.add(pAsset3);
    DropAsset pAsset4 = new DropAsset();
    pAsset4.id = "dust_red";
    pAsset4.path_texture = "drops/drop_dust_red";
    pAsset4.animated = true;
    pAsset4.animation_speed = 0.03f;
    pAsset4.default_scale = 0.1f;
    pAsset4.action_landed = new DropsAction(DropsLibrary.action_dust_red);
    pAsset4.material = "mat_world_object_lit";
    pAsset4.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset4.type = DropType.DropMagic;
    this.add(pAsset4);
    DropAsset pAsset5 = new DropAsset();
    pAsset5.id = "dust_blue";
    pAsset5.path_texture = "drops/drop_dust_blue";
    pAsset5.animated = true;
    pAsset5.animation_speed = 0.03f;
    pAsset5.default_scale = 0.1f;
    pAsset5.action_landed = new DropsAction(DropsLibrary.action_dust_blue);
    pAsset5.material = "mat_world_object_lit";
    pAsset5.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset5.type = DropType.DropMagic;
    this.add(pAsset5);
    DropAsset pAsset6 = new DropAsset();
    pAsset6.id = "dust_gold";
    pAsset6.path_texture = "drops/drop_dust_gold";
    pAsset6.animated = true;
    pAsset6.animation_speed = 0.03f;
    pAsset6.default_scale = 0.1f;
    pAsset6.action_landed = new DropsAction(DropsLibrary.action_dust_gold);
    pAsset6.material = "mat_world_object_lit";
    pAsset6.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset6.type = DropType.DropMagic;
    this.add(pAsset6);
    DropAsset pAsset7 = new DropAsset();
    pAsset7.id = "dust_purple";
    pAsset7.path_texture = "drops/drop_dust_purple";
    pAsset7.animated = true;
    pAsset7.animation_speed = 0.03f;
    pAsset7.default_scale = 0.1f;
    pAsset7.action_landed = new DropsAction(DropsLibrary.action_dust_purple);
    pAsset7.material = "mat_world_object_lit";
    pAsset7.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset7.type = DropType.DropMagic;
    this.add(pAsset7);
    DropAsset pAsset8 = new DropAsset();
    pAsset8.id = "gamma_rain";
    pAsset8.path_texture = "drops/drop_gamma_rain";
    pAsset8.random_frame = true;
    pAsset8.default_scale = 0.1f;
    pAsset8.action_landed = new DropsAction(DropsLibrary.action_gamma_rain);
    pAsset8.material = "mat_world_object_lit";
    pAsset8.sound_drop = "event:/SFX/DROPS/DropRainGamma";
    pAsset8.type = DropType.DropTraitRain;
    this.add(pAsset8);
    DropAsset pAsset9 = new DropAsset();
    pAsset9.id = "delta_rain";
    pAsset9.path_texture = "drops/drop_delta_rain";
    pAsset9.random_frame = true;
    pAsset9.default_scale = 0.1f;
    pAsset9.action_landed = new DropsAction(DropsLibrary.action_delta_rain);
    pAsset9.material = "mat_world_object_lit";
    pAsset9.sound_drop = "event:/SFX/DROPS/DropRainDelta";
    pAsset9.type = DropType.DropTraitRain;
    this.add(pAsset9);
    DropAsset pAsset10 = new DropAsset();
    pAsset10.id = "omega_rain";
    pAsset10.path_texture = "drops/drop_omega_rain";
    pAsset10.random_frame = true;
    pAsset10.default_scale = 0.1f;
    pAsset10.action_landed = new DropsAction(DropsLibrary.action_omega_rain);
    pAsset10.material = "mat_world_object_lit";
    pAsset10.sound_drop = "event:/SFX/DROPS/DropRainOmeaga";
    pAsset10.type = DropType.DropTraitRain;
    this.add(pAsset10);
    DropAsset pAsset11 = new DropAsset();
    pAsset11.id = "loot_rain";
    pAsset11.path_texture = "drops/drop_loot_rain";
    pAsset11.random_frame = true;
    pAsset11.default_scale = 0.1f;
    pAsset11.action_landed = new DropsAction(DropsLibrary.action_equipment_rain);
    pAsset11.material = "mat_world_object_lit";
    pAsset11.sound_drop = "event:/SFX/DROPS/DropRainGamma";
    pAsset11.type = DropType.DropEquipmentRain;
    this.add(pAsset11);
    DropAsset pAsset12 = new DropAsset();
    pAsset12.id = "tnt";
    pAsset12.animated = true;
    pAsset12.path_texture = "drops/drop_tnt";
    pAsset12.animation_speed = 0.03f;
    pAsset12.default_scale = 0.2f;
    pAsset12.action_landed = new DropsAction(DropsLibrary.action_tnt);
    pAsset12.sound_drop = "event:/SFX/DROPS/DropTnt";
    pAsset12.type = DropType.DropTile;
    this.add(pAsset12);
    DropAsset pAsset13 = new DropAsset();
    pAsset13.id = "tnt_timed";
    pAsset13.path_texture = "drops/drop_tnttimed";
    pAsset13.default_scale = 0.2f;
    pAsset13.action_landed = new DropsAction(DropsLibrary.action_tnt_timed);
    pAsset13.sound_drop = "event:/SFX/DROPS/DropTnt";
    pAsset13.type = DropType.DropTile;
    this.add(pAsset13);
    DropAsset pAsset14 = new DropAsset();
    pAsset14.id = "water_bomb";
    pAsset14.path_texture = "drops/drop_waterbomb";
    pAsset14.default_scale = 0.2f;
    pAsset14.action_landed = new DropsAction(DropsLibrary.action_water_bomb);
    pAsset14.sound_drop = "event:/SFX/DROPS/DropWaterBomb";
    pAsset14.type = DropType.DropTile;
    this.add(pAsset14);
    DropAsset pAsset15 = new DropAsset();
    pAsset15.id = "landmine";
    pAsset15.path_texture = "drops/drop_landmine";
    pAsset15.default_scale = 0.2f;
    pAsset15.action_landed = new DropsAction(DropsLibrary.action_landmine);
    pAsset15.sound_drop = "event:/SFX/DROPS/DropLandmine";
    pAsset15.type = DropType.DropTile;
    this.add(pAsset15);
    DropAsset pAsset16 = new DropAsset();
    pAsset16.id = "fireworks";
    pAsset16.path_texture = "drops/drop_fireworks";
    pAsset16.random_frame = true;
    pAsset16.default_scale = 0.1f;
    pAsset16.action_landed = new DropsAction(DropsLibrary.action_fireworks);
    pAsset16.sound_drop = "event:/SFX/DROPS/DropFireworks";
    pAsset16.type = DropType.DropTile;
    this.add(pAsset16);
    DropAsset pAsset17 = new DropAsset();
    pAsset17.id = "inspiration";
    pAsset17.path_texture = "drops/drop_inspiration";
    pAsset17.default_scale = 0.2f;
    pAsset17.action_landed = new DropsAction(DropsLibrary.action_inspiration);
    pAsset17.material = "mat_world_object_lit";
    pAsset17.sound_drop = "event:/SFX/DROPS/DropInspiration";
    pAsset17.type = DropType.DropMagic;
    this.add(pAsset17);
    DropAsset pAsset18 = new DropAsset();
    pAsset18.id = "discord";
    pAsset18.path_texture = "drops/drop_discord";
    pAsset18.default_scale = 0.2f;
    pAsset18.action_landed = new DropsAction(DropsLibrary.action_discord);
    pAsset18.material = "mat_world_object_lit";
    pAsset18.sound_drop = "event:/SFX/DROPS/DropInspiration";
    pAsset18.type = DropType.DropMagic;
    this.add(pAsset18);
    DropAsset pAsset19 = new DropAsset();
    pAsset19.id = "friendship";
    pAsset19.path_texture = "drops/drop_friendship";
    pAsset19.random_frame = true;
    pAsset19.default_scale = 0.1f;
    pAsset19.action_landed = new DropsAction(DropsLibrary.action_friendship);
    pAsset19.material = "mat_world_object_lit";
    pAsset19.sound_drop = "event:/SFX/DROPS/DropFriendship";
    pAsset19.type = DropType.DropMagic;
    this.add(pAsset19);
    DropAsset pAsset20 = new DropAsset();
    pAsset20.id = "spite";
    pAsset20.path_texture = "drops/drop_spite";
    pAsset20.random_frame = true;
    pAsset20.default_scale = 0.1f;
    pAsset20.action_landed = new DropsAction(DropsLibrary.action_spite);
    pAsset20.material = "mat_world_object_lit";
    pAsset20.sound_drop = "event:/SFX/DROPS/DropSpite";
    pAsset20.type = DropType.DropMagic;
    this.add(pAsset20);
    DropAsset pAsset21 = new DropAsset();
    pAsset21.id = "madness";
    pAsset21.path_texture = "drops/drop_madness";
    pAsset21.random_frame = true;
    pAsset21.default_scale = 0.1f;
    pAsset21.action_landed = new DropsAction(DropsLibrary.action_madness);
    pAsset21.material = "mat_world_object_lit";
    pAsset21.sound_drop = "event:/SFX/DROPS/DropMadness";
    pAsset21.type = DropType.DropTrait;
    this.add(pAsset21);
    DropAsset pAsset22 = new DropAsset();
    pAsset22.id = "blessing";
    pAsset22.path_texture = "drops/drop_blessing";
    pAsset22.animated = true;
    pAsset22.animation_speed = 0.03f;
    pAsset22.default_scale = 0.1f;
    pAsset22.action_landed = new DropsAction(DropsLibrary.action_blessing);
    pAsset22.material = "mat_world_object_lit";
    pAsset22.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset22.type = DropType.DropMagic;
    this.add(pAsset22);
    this.t.action_landed += new DropsAction(ActionLibrary.action_shrinkTornadoes);
    DropAsset pAsset23 = new DropAsset();
    pAsset23.id = "shield";
    pAsset23.path_texture = "drops/drop_shield";
    pAsset23.random_frame = true;
    pAsset23.default_scale = 0.1f;
    pAsset23.action_landed = new DropsAction(DropsLibrary.action_shield);
    pAsset23.material = "mat_world_object_lit";
    pAsset23.sound_drop = "event:/SFX/DROPS/DropShield";
    pAsset23.type = DropType.DropStatus;
    this.add(pAsset23);
    DropAsset pAsset24 = new DropAsset();
    pAsset24.id = "coffee";
    pAsset24.path_texture = "drops/drop_coffee";
    pAsset24.random_frame = true;
    pAsset24.default_scale = 0.1f;
    pAsset24.action_landed = new DropsAction(DropsLibrary.action_coffee);
    pAsset24.sound_drop = "event:/SFX/DROPS/DropCoffee";
    pAsset24.type = DropType.DropStatus;
    this.add(pAsset24);
    DropAsset pAsset25 = new DropAsset();
    pAsset25.id = "powerup";
    pAsset25.path_texture = "drops/drop_mushroom_powerup";
    pAsset25.random_frame = true;
    pAsset25.default_scale = 0.1f;
    pAsset25.action_landed = new DropsAction(DropsLibrary.action_powerup);
    pAsset25.sound_drop = "event:/SFX/DROPS/DropPowerup";
    pAsset25.type = DropType.DropMagic;
    this.add(pAsset25);
    DropAsset pAsset26 = new DropAsset();
    pAsset26.id = "curse";
    pAsset26.path_texture = "drops/drop_curse";
    pAsset26.random_frame = true;
    pAsset26.default_scale = 0.1f;
    pAsset26.action_landed = new DropsAction(DropsLibrary.action_curse);
    pAsset26.material = "mat_world_object_lit";
    pAsset26.sound_drop = "event:/SFX/DROPS/DropCurse";
    pAsset26.type = DropType.DropTrait;
    this.add(pAsset26);
    this.t.action_landed += new DropsAction(ActionLibrary.action_growTornadoes);
    DropAsset pAsset27 = new DropAsset();
    pAsset27.id = "spell_silence";
    pAsset27.path_texture = "drops/drop_spell_silence";
    pAsset27.random_frame = true;
    pAsset27.default_scale = 0.1f;
    pAsset27.action_landed = new DropsAction(DropsLibrary.action_spell_silence);
    pAsset27.material = "mat_world_object_lit";
    pAsset27.sound_drop = "event:/SFX/DROPS/DropCurse";
    pAsset27.type = DropType.DropTrait;
    this.add(pAsset27);
    DropAsset pAsset28 = new DropAsset();
    pAsset28.id = "zombie_infection";
    pAsset28.path_texture = "drops/drop_zombieinfection";
    pAsset28.random_frame = true;
    pAsset28.default_scale = 0.1f;
    pAsset28.action_landed = new DropsAction(DropsLibrary.action_zombie_infection);
    pAsset28.sound_drop = "event:/SFX/DROPS/DropZombieInfection";
    pAsset28.type = DropType.DropTrait;
    this.add(pAsset28);
    DropAsset pAsset29 = new DropAsset();
    pAsset29.id = "mush_spores";
    pAsset29.path_texture = "drops/drop_mushSpores";
    pAsset29.random_frame = true;
    pAsset29.default_scale = 0.1f;
    pAsset29.action_landed = new DropsAction(DropsLibrary.action_mush_spore);
    pAsset29.sound_drop = "event:/SFX/DROPS/DropMushSpores";
    pAsset29.type = DropType.DropTrait;
    this.add(pAsset29);
    DropAsset pAsset30 = new DropAsset();
    pAsset30.id = "plague";
    pAsset30.path_texture = "drops/drop_plague";
    pAsset30.random_frame = true;
    pAsset30.default_scale = 0.1f;
    pAsset30.action_landed = new DropsAction(DropsLibrary.action_plague);
    pAsset30.material = "mat_world_object_lit";
    pAsset30.sound_drop = "event:/SFX/DROPS/DropPlague";
    pAsset30.type = DropType.DropTrait;
    this.add(pAsset30);
    DropAsset pAsset31 = new DropAsset();
    pAsset31.id = "living_plants";
    pAsset31.path_texture = "drops/drop_blessing";
    pAsset31.animated = true;
    pAsset31.default_scale = 0.1f;
    pAsset31.action_landed = new DropsAction(DropsLibrary.action_living_plants);
    pAsset31.sound_drop = "event:/SFX/DROPS/DropLivingPlants";
    pAsset31.type = DropType.DropMagic;
    this.add(pAsset31);
    DropAsset pAsset32 = new DropAsset();
    pAsset32.id = "living_house";
    pAsset32.path_texture = "drops/drop_blessing";
    pAsset32.animated = true;
    pAsset32.default_scale = 0.1f;
    pAsset32.action_landed = new DropsAction(DropsLibrary.action_living_house);
    pAsset32.sound_drop = "event:/SFX/DROPS/DropLivingHouse";
    pAsset32.type = DropType.DropMagic;
    this.add(pAsset32);
    DropAsset pAsset33 = new DropAsset();
    pAsset33.id = "bomb";
    pAsset33.path_texture = "drops/drop_bomb";
    pAsset33.default_scale = 0.2f;
    pAsset33.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset33.sound_launch = "event:/SFX/DROPS/DropLaunchBombSmall";
    pAsset33.action_landed = new DropsAction(DropsLibrary.action_bomb);
    pAsset33.type = DropType.DropBomb;
    this.add(pAsset33);
    this.t.action_launch += new DropsAction(ActionLibrary.increaseDroppedBombsCounter);
    DropAsset pAsset34 = new DropAsset();
    pAsset34.id = "grenade";
    pAsset34.path_texture = "drops/drop_grenade";
    pAsset34.animated = true;
    pAsset34.default_scale = 0.2f;
    pAsset34.animation_speed = 0.03f;
    pAsset34.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset34.action_landed = new DropsAction(DropsLibrary.action_grenade);
    pAsset34.random_flip = true;
    pAsset34.sound_launch = "event:/SFX/DROPS/DropLaunchGrenade";
    pAsset34.type = DropType.DropBomb;
    this.add(pAsset34);
    DropAsset pAsset35 = new DropAsset();
    pAsset35.id = "crab_bomb";
    pAsset35.path_texture = "drops/drop_crab_bomb_parachute";
    pAsset35.animated = true;
    pAsset35.default_scale = 0.1f;
    pAsset35.animation_speed = 0.05f;
    pAsset35.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset35.action_landed = new DropsAction(DropsLibrary.action_crab_bomb_impact);
    pAsset35.random_flip = true;
    pAsset35.sound_launch = "event:/SFX/DROPS/DropLaunchCrabBomb";
    pAsset35.type = DropType.DropBomb;
    this.add(pAsset35);
    DropAsset pAsset36 = new DropAsset();
    pAsset36.id = "crab_bomb_shrapnel";
    pAsset36.path_texture = "drops/drop_crab_bomb_shrapnel";
    pAsset36.animated = true;
    pAsset36.animation_rotation = true;
    pAsset36.animation_rotation_speed_min = 50f;
    pAsset36.animation_rotation_speed_max = 200f;
    pAsset36.default_scale = 0.175f;
    pAsset36.animation_speed = 0.05f;
    pAsset36.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset36.action_landed = new DropsAction(DropsLibrary.action_crab_bomb_shrapnel);
    pAsset36.random_flip = true;
    pAsset36.sound_launch = "event:/SFX/DROPS/DropLaunchCrabBomb";
    pAsset36.type = DropType.DropBomb;
    pAsset36.surprises_units = true;
    this.add(pAsset36);
    DropAsset pAsset37 = new DropAsset();
    pAsset37.id = "napalm_bomb";
    pAsset37.path_texture = "drops/drop_napalmbomb";
    pAsset37.default_scale = 0.2f;
    pAsset37.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset37.action_landed = new DropsAction(DropsLibrary.action_napalm_bomb);
    pAsset37.random_flip = true;
    pAsset37.type = DropType.DropBomb;
    this.add(pAsset37);
    this.t.action_launch += new DropsAction(ActionLibrary.increaseDroppedBombsCounter);
    DropAsset pAsset38 = new DropAsset();
    pAsset38.id = "atomic_bomb";
    pAsset38.path_texture = "drops/drop_atomicbomb";
    pAsset38.default_scale = 0.2f;
    pAsset38.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset38.action_landed = new DropsAction(DropsLibrary.action_atomic_bomb);
    pAsset38.random_flip = true;
    pAsset38.sound_launch = "event:/SFX/DROPS/DropLaunchGrenadeHuge";
    pAsset38.type = DropType.DropBomb;
    this.add(pAsset38);
    this.t.action_launch += new DropsAction(ActionLibrary.increaseDroppedBombsCounter);
    DropAsset pAsset39 = new DropAsset();
    pAsset39.id = "antimatter_bomb";
    pAsset39.path_texture = "drops/drop_antimatterbomb";
    pAsset39.default_scale = 0.2f;
    pAsset39.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset39.action_landed = new DropsAction(DropsLibrary.action_antimatter_bomb);
    pAsset39.sound_launch = "event:/SFX/DROPS/DropLaunchGrenadeHuge";
    pAsset39.type = DropType.DropBomb;
    this.add(pAsset39);
    this.t.action_launch += new DropsAction(ActionLibrary.increaseDroppedBombsCounter);
    DropAsset pAsset40 = new DropAsset();
    pAsset40.id = "czar_bomba";
    pAsset40.path_texture = "drops/drop_czarbomba";
    pAsset40.default_scale = 0.2f;
    pAsset40.falling_height = Vector2.op_Implicit(new Vector2(60f, 70f));
    pAsset40.action_landed = new DropsAction(DropsLibrary.action_czar_bomba);
    pAsset40.sound_launch = "event:/SFX/DROPS/DropLaunchGrenadeHuge";
    pAsset40.type = DropType.DropBomb;
    this.add(pAsset40);
    this.t.action_launch += new DropsAction(ActionLibrary.increaseDroppedBombsCounter);
    DropAsset pAsset41 = new DropAsset();
    pAsset41.id = "rain";
    pAsset41.path_texture = "drops/drop_rain";
    pAsset41.random_frame = true;
    pAsset41.default_scale = 0.2f;
    pAsset41.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset41.action_landed = new DropsAction(DropsLibrary.action_rain);
    pAsset41.sound_drop = "event:/SFX/DROPS/DropRain";
    pAsset41.type = DropType.DropGeneric;
    pAsset41.surprises_units = false;
    this.add(pAsset41);
    DropAsset pAsset42 = new DropAsset();
    pAsset42.id = "blood_rain";
    pAsset42.path_texture = "drops/drop_blood";
    pAsset42.random_frame = true;
    pAsset42.default_scale = 0.1f;
    pAsset42.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset42.action_landed_drop = new DropsLandedAction(DropsLibrary.action_blood_rain);
    pAsset42.material = "mat_world_object_lit";
    pAsset42.sound_drop = "event:/SFX/DROPS/DropBloodRain";
    pAsset42.type = DropType.DropMagic;
    this.add(pAsset42);
    DropAsset pAsset43 = new DropAsset();
    pAsset43.id = "clone_rain";
    pAsset43.path_texture = "drops/drop_clone";
    pAsset43.random_frame = true;
    pAsset43.default_scale = 0.1f;
    pAsset43.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset43.action_landed = new DropsAction(DropsLibrary.action_clone_rain);
    pAsset43.material = "mat_world_object_lit";
    pAsset43.sound_drop = "event:/SFX/DROPS/DropBloodRain";
    pAsset43.type = DropType.DropMagic;
    this.add(pAsset43);
    DropAsset pAsset44 = new DropAsset();
    pAsset44.id = "jazz";
    pAsset44.path_texture = "drops/drop_jazz";
    pAsset44.random_frame = true;
    pAsset44.default_scale = 0.1f;
    pAsset44.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset44.action_landed = new DropsAction(this.action_jazz_rain);
    pAsset44.material = "mat_world_object_lit";
    pAsset44.sound_drop = "event:/SFX/DROPS/DropBloodRain";
    pAsset44.type = DropType.DropMagic;
    this.add(pAsset44);
    DropAsset pAsset45 = new DropAsset();
    pAsset45.id = "dispel";
    pAsset45.path_texture = "drops/drop_dispel";
    pAsset45.random_frame = true;
    pAsset45.default_scale = 0.1f;
    pAsset45.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset45.action_landed = new DropsAction(this.action_dispel_rain);
    pAsset45.material = "mat_world_object_lit";
    pAsset45.sound_drop = "event:/SFX/DROPS/DropBloodRain";
    pAsset45.type = DropType.DropMagic;
    this.add(pAsset45);
    DropAsset pAsset46 = new DropAsset();
    pAsset46.id = "sleep";
    pAsset46.path_texture = "drops/drop_sleep";
    pAsset46.random_frame = true;
    pAsset46.default_scale = 0.1f;
    pAsset46.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset46.action_landed = new DropsAction(this.action_sleep_rain);
    pAsset46.material = "mat_world_object_lit";
    pAsset46.sound_drop = "event:/SFX/DROPS/DropBloodRain";
    pAsset46.type = DropType.DropMagic;
    this.add(pAsset46);
    DropAsset pAsset47 = new DropAsset();
    pAsset47.id = "cure";
    pAsset47.path_texture = "drops/drop_cure";
    pAsset47.random_frame = true;
    pAsset47.default_scale = 0.1f;
    pAsset47.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset47.action_landed = new DropsAction(DropsLibrary.action_cure);
    pAsset47.material = "mat_world_object_lit";
    pAsset47.sound_drop = "event:/SFX/DROPS/DropCure";
    pAsset47.type = DropType.DropMagic;
    this.add(pAsset47);
    DropAsset pAsset48 = new DropAsset();
    pAsset48.id = "fire";
    pAsset48.path_texture = "drops/drop_fire";
    pAsset48.animated = true;
    pAsset48.animation_speed = 0.03f;
    pAsset48.default_scale = 0.2f;
    pAsset48.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset48.falling_random_x_move = true;
    pAsset48.particle_interval = 0.3f;
    pAsset48.action_landed = new DropsAction(DropsLibrary.action_fire);
    pAsset48.animation_speed_random = 0.08f;
    pAsset48.random_frame = true;
    pAsset48.random_flip = true;
    pAsset48.sound_drop = "event:/SFX/DROPS/DropFire";
    pAsset48.material = "mat_world_object_lit";
    pAsset48.type = DropType.DropGeneric;
    this.add(pAsset48);
    DropAsset pAsset49 = new DropAsset();
    pAsset49.id = "snow";
    pAsset49.path_texture = "drops/drop_snow";
    pAsset49.random_frame = true;
    pAsset49.default_scale = 0.2f;
    pAsset49.falling_speed = 0.3f;
    pAsset49.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset49.falling_random_x_move = true;
    pAsset49.particle_interval = 0.15f;
    pAsset49.sound_drop = "event:/SFX/DROPS/DropSnow";
    pAsset49.action_landed = new DropsAction(DropsLibrary.action_snow);
    pAsset49.type = DropType.DropGeneric;
    this.add(pAsset49);
    DropAsset pAsset50 = new DropAsset();
    pAsset50.id = "life_seed";
    pAsset50.path_texture = "drops/drop_life_seed";
    pAsset50.random_frame = true;
    pAsset50.default_scale = 0.2f;
    pAsset50.falling_speed = 0.3f;
    pAsset50.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset50.falling_random_x_move = true;
    pAsset50.particle_interval = 0.15f;
    pAsset50.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    pAsset50.action_landed = new DropsAction(DropsLibrary.action_life_seed);
    pAsset50.type = DropType.DropGeneric;
    this.add(pAsset50);
    DropAsset pAsset51 = new DropAsset();
    pAsset51.id = "ash";
    pAsset51.path_texture = "drops/drop_ash";
    pAsset51.random_frame = true;
    pAsset51.default_scale = 0.2f;
    pAsset51.falling_speed = 0.3f;
    pAsset51.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset51.falling_random_x_move = true;
    pAsset51.particle_interval = 0.15f;
    pAsset51.sound_drop = "event:/SFX/DROPS/DropAsh";
    pAsset51.action_landed = new DropsAction(DropsLibrary.action_ash);
    pAsset51.type = DropType.DropMagic;
    this.add(pAsset51);
    DropAsset pAsset52 = new DropAsset();
    pAsset52.id = "magic_rain";
    pAsset52.path_texture = "drops/drop_magic_rain";
    pAsset52.random_frame = true;
    pAsset52.default_scale = 0.2f;
    pAsset52.falling_speed = 0.3f;
    pAsset52.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset52.falling_random_x_move = true;
    pAsset52.particle_interval = 0.15f;
    pAsset52.sound_drop = "event:/SFX/DROPS/DropMagicRain";
    pAsset52.action_landed = new DropsAction(DropsLibrary.action_magic_rain);
    pAsset52.type = DropType.DropMagic;
    this.add(pAsset52);
    DropAsset pAsset53 = new DropAsset();
    pAsset53.id = "rage";
    pAsset53.path_texture = "drops/drop_rage";
    pAsset53.random_frame = true;
    pAsset53.default_scale = 0.2f;
    pAsset53.falling_speed = 0.3f;
    pAsset53.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset53.falling_random_x_move = true;
    pAsset53.particle_interval = 0.15f;
    pAsset53.sound_drop = "event:/SFX/DROPS/DropRage";
    pAsset53.action_landed = new DropsAction(DropsLibrary.action_rage);
    pAsset53.type = DropType.DropStatus;
    this.add(pAsset53);
    DropAsset pAsset54 = new DropAsset();
    pAsset54.id = "acid";
    pAsset54.path_texture = "drops/drop_acid";
    pAsset54.random_frame = true;
    pAsset54.default_scale = 0.2f;
    pAsset54.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset54.action_landed = new DropsAction(DropsLibrary.action_acid);
    pAsset54.material = "mat_world_object_lit";
    pAsset54.sound_drop = "event:/SFX/DROPS/DropAcid";
    pAsset54.type = DropType.DropMagic;
    this.add(pAsset54);
    DropAsset pAsset55 = new DropAsset();
    pAsset55.id = "lava";
    pAsset55.path_texture = "drops/drop_lava";
    pAsset55.animated = true;
    pAsset55.animation_speed = 0.03f;
    pAsset55.default_scale = 0.2f;
    pAsset55.falling_height = Vector2.op_Implicit(new Vector2(30f, 45f));
    pAsset55.action_landed = new DropsAction(DropsLibrary.action_lava);
    pAsset55.material = "mat_world_object_lit";
    pAsset55.sound_drop = "event:/SFX/DROPS/DropLava";
    pAsset55.type = DropType.DropGeneric;
    this.add(pAsset55);
    DropAsset pAsset56 = new DropAsset();
    pAsset56.id = "santa_bomb";
    pAsset56.path_texture = "drops/drop_santabomb";
    pAsset56.random_frame = true;
    pAsset56.default_scale = 0.2f;
    pAsset56.sound_launch = "event:/SFX/DROPS/DropLaunchSantaBomb";
    pAsset56.action_landed = new DropsAction(DropsLibrary.action_santa_bomb);
    pAsset56.type = DropType.DropBomb;
    pAsset56.surprises_units = true;
    this.add(pAsset56);
    DropAsset pAsset57 = new DropAsset();
    pAsset57.id = "$spawn_building$";
    pAsset57.path_texture = "drops/drop_stone";
    pAsset57.random_frame = true;
    pAsset57.default_scale = 0.2f;
    pAsset57.falling_height = Vector2.op_Implicit(new Vector2(10f, 15f));
    pAsset57.falling_speed = 5f;
    pAsset57.type = DropType.DropBuilding;
    this.add(pAsset57);
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.clone("$biome_seeds$", "$spawn_building$");
    this.t.type = DropType.DropSeed;
    this.t.action_landed = (DropsAction) null;
    this.clone("seeds_grass", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_grass";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "grass_low";
    this.t.drop_type_high = "grass_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_enchanted", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_enchanted";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "enchanted_low";
    this.t.drop_type_high = "enchanted_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedEnchanted";
    this.clone("seeds_savanna", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_savanna";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "savanna_low";
    this.t.drop_type_high = "savanna_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedSavanna";
    this.clone("seeds_corrupted", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_corrupted";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "corrupted_low";
    this.t.drop_type_high = "corrupted_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedCorrupted";
    this.clone("seeds_mushroom", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_mushroom";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "mushroom_low";
    this.t.drop_type_high = "mushroom_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedMushroom";
    this.clone("seeds_jungle", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_jungle";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "jungle_low";
    this.t.drop_type_high = "jungle_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedJungle";
    this.clone("seeds_desert", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_desert";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "desert_low";
    this.t.drop_type_high = "desert_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedDesert";
    this.clone("seeds_lemon", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_lemon";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "lemon_low";
    this.t.drop_type_high = "lemon_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedLemon";
    this.clone("seeds_permafrost", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_permafrost";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "permafrost_low";
    this.t.drop_type_high = "permafrost_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedPermafrost";
    this.clone("seeds_candy", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_candy";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "candy_low";
    this.t.drop_type_high = "candy_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedCandy";
    this.clone("seeds_crystal", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_crystal";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "crystal_low";
    this.t.drop_type_high = "crystal_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedCrystal";
    this.clone("seeds_swamp", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_swamp";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "swamp_low";
    this.t.drop_type_high = "swamp_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedSwamp";
    this.clone("seeds_infernal", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_infernal";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "infernal_low";
    this.t.drop_type_high = "infernal_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedInfernal";
    this.clone("seeds_birch", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_birch";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "birch_low";
    this.t.drop_type_high = "birch_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_maple", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_maple";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "maple_low";
    this.t.drop_type_high = "maple_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_rocklands", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_rocklands";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "rocklands_low";
    this.t.drop_type_high = "rocklands_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_garlic", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_garlic";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "garlic_low";
    this.t.drop_type_high = "garlic_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_flower", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_flower";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "flower_low";
    this.t.drop_type_high = "flower_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_celestial", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_celestial";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "celestial_low";
    this.t.drop_type_high = "celestial_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_singularity", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_singularity";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "singularity_low";
    this.t.drop_type_high = "singularity_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_clover", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_clover";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "clover_low";
    this.t.drop_type_high = "clover_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("seeds_paradox", "$biome_seeds$");
    this.t.path_texture = "drops/drop_seed_paradox";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_seeds);
    this.t.drop_type_low = "paradox_low";
    this.t.drop_type_high = "paradox_high";
    this.t.sound_drop = "event:/SFX/DROPS/DropSeedGrass";
    this.clone("fruit_bush", "$spawn_building$");
    this.t.path_texture = "drops/drop_seed";
    this.t.falling_speed = 3f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_fruit_bush);
    this.t.sound_drop = "event:/SFX/DROPS/DropBush";
    this.clone("fertilizer_plants", "$biome_seeds$");
    this.t.surprises_units = false;
    this.t.path_texture = "drops/drop_fertilizer";
    this.t.falling_speed = 5f;
    this.t.action_landed += new DropsAction(DropsLibrary.action_fertilizer_plants);
    this.t.action_landed += new DropsAction(DropsLibrary.tryToGrowWheat);
    this.t.action_landed += new DropsAction(DropsLibrary.flash);
    this.t.sound_drop = "event:/SFX/DROPS/DropFertilizerPlants";
    this.clone("fertilizer_trees", "$biome_seeds$");
    this.t.path_texture = "drops/drop_fertilizer";
    this.t.falling_speed = 5f;
    this.t.action_landed = new DropsAction(DropsLibrary.action_fertilizer_trees);
    this.t.action_landed += new DropsAction(DropsLibrary.flash);
    this.t.sound_drop = "event:/SFX/DROPS/DropFertilizerPlants";
    this.clone("$spawn_mineral$", "$spawn_building$");
    this.t.falling_speed = 6f;
    this.t.type = DropType.DropMineral;
    this.clone("stone", "$spawn_mineral$");
    this.t.path_texture = "drops/drop_stone";
    this.t.default_scale = 0.2f;
    this.t.building_asset = "mineral_stone";
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropStone";
    this.clone("metals", "$spawn_mineral$");
    this.t.path_texture = "drops/drop_metal";
    this.t.default_scale = 0.2f;
    this.t.building_asset = "mineral_metals";
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropMineral";
    this.clone("gold", "$spawn_mineral$");
    this.t.path_texture = "drops/drop_gold";
    this.t.default_scale = 0.2f;
    this.t.building_asset = "mineral_gold";
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropGold";
    this.clone("silver", "$spawn_mineral$");
    this.t.path_texture = "drops/drop_stone";
    this.t.default_scale = 0.2f;
    this.t.building_asset = "mineral_silver";
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropMineral";
    this.clone("mythril", "$spawn_mineral$");
    this.t.path_texture = "drops/drop_stone";
    this.t.default_scale = 0.2f;
    this.t.building_asset = "mineral_mythril";
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropMineral";
    this.clone("adamantine", "$spawn_mineral$");
    this.t.path_texture = "drops/drop_stone";
    this.t.default_scale = 0.2f;
    this.t.building_asset = "mineral_adamantine";
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropMineral";
    this.clone("$spawn_creep$", "$spawn_building$");
    this.t.type = DropType.DropCreep;
    this.clone("tumor", "$spawn_creep$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropTumor";
    this.clone("biomass", "$spawn_creep$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropBiomass";
    this.clone("cybercore", "$spawn_creep$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropCybercore";
    this.clone("super_pumpkin", "$spawn_creep$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropSuperPumpkin";
    this.clone("geyser", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropGeyser";
    this.clone("geyser_acid", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropGeyser";
    this.clone("volcano", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropVolcano";
    this.clone("golden_brain", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropGoldenBrain";
    this.clone("monolith", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.action_landed += (DropsAction) ((_1, _2) => AchievementLibrary.cant_be_too_much.checkBySignal());
    this.t.sound_drop = "event:/SFX/DROPS/DropMonolith";
    this.clone("corrupted_brain", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropCorruptedBrain";
    this.clone("ice_tower", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropIceTower";
    this.clone("angle_tower", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropIceTower";
    this.clone("beehive", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropBeehive";
    this.clone("flame_tower", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropFlameTower";
    this.addWaypointDrops();
  }

  public override void linkAssets()
  {
    foreach (DropAsset dropAsset in this.list)
    {
      if (!string.IsNullOrEmpty(dropAsset.drop_type_high))
        dropAsset.cached_drop_type_high = AssetManager.top_tiles.get(dropAsset.drop_type_high);
      if (!string.IsNullOrEmpty(dropAsset.drop_type_low))
        dropAsset.cached_drop_type_low = AssetManager.top_tiles.get(dropAsset.drop_type_low);
    }
    base.linkAssets();
  }

  private void addWaypointDrops()
  {
    DropAsset pAsset = new DropAsset();
    pAsset.id = "desire_alien_mold";
    pAsset.path_texture = "drops/drop_alien_mold";
    pAsset.animated = false;
    pAsset.default_scale = 0.1f;
    pAsset.material = "mat_world_object_lit";
    pAsset.sound_drop = "event:/SFX/DROPS/DropBlessing";
    pAsset.type = DropType.DropMagic;
    this.add(pAsset);
    this.t.action_landed = new DropsAction(DropsLibrary.action_alien_mold);
    this.t.surprises_units = true;
    this.clone("desire_computer", "desire_alien_mold");
    this.t.path_texture = "drops/drop_computer";
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_computer);
    this.clone("desire_golden_egg", "desire_alien_mold");
    this.t.path_texture = "drops/drop_golden_egg";
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_golden_egg);
    this.clone("desire_harp", "desire_alien_mold");
    this.t.path_texture = "drops/drop_harp";
    this.t.action_landed = new DropsAction(DropsLibrary.action_drop_harp);
    this.clone("waypoint_alien_mold", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropCorruptedBrain";
    this.clone("waypoint_computer", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropCorruptedBrain";
    this.clone("waypoint_golden_egg", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropCorruptedBrain";
    this.clone("waypoint_harp", "$spawn_building$");
    this.t.building_asset = this.t.id;
    this.t.action_landed = new DropsAction(DropsLibrary.action_spawn_building);
    this.t.sound_drop = "event:/SFX/DROPS/DropCorruptedBrain";
  }

  public static void action_drop_seeds(WorldTile pTile = null, string pDropID = null)
  {
    DropAsset dropAsset = AssetManager.drops.get(pDropID);
    DropsLibrary.useDropSeedOn(pTile, dropAsset.cached_drop_type_low, dropAsset.cached_drop_type_high);
  }

  public static void useDropSeedOn(WorldTile pTile, TopTileType pTypeLow, TopTileType pHigh)
  {
    DropsLibrary.useSeedOn(pTile, pTypeLow, pHigh);
    for (int index = 0; index < pTile.neighbours.Length; ++index)
      DropsLibrary.useSeedOn(pTile.neighbours[index], pTypeLow, pHigh);
  }

  public static void tryToGrowWheat(WorldTile pTile = null, string pDropID = null)
  {
    if (!pTile.Type.farm_field || pTile.hasBuilding())
      return;
    World.world.buildings.addBuilding("wheat", pTile);
  }

  public static void useSeedOn(WorldTile pTile, TopTileType pTypeLow, TopTileType pHigh)
  {
    pTile.unfreeze();
    if (!pTile.Type.can_be_biome)
      return;
    if (pTile.isTileRank(TileRank.Low))
      MapAction.growGreens(pTile, pTypeLow);
    else if (pTile.isTileRank(TileRank.High))
      MapAction.growGreens(pTile, pHigh);
    BiomeAsset tBiome = pTile.getBiome();
    if (tBiome == null)
      return;
    pTile.doUnits((Action<Actor>) (tActor =>
    {
      if (!tActor.hasSubspecies() || tBiome.spawn_trait_subspecies_always == null)
        return;
      foreach (string traitSubspeciesAlway in tBiome.spawn_trait_subspecies_always)
        tActor.subspecies.addTrait(traitSubspeciesAlway);
    }));
  }

  public static void action_rain(WorldTile pTile = null, string pDropID = null)
  {
    DropsLibrary.useRainOn(pTile);
    for (int index = 0; index < pTile.neighbours.Length; ++index)
      DropsLibrary.useRainOn(pTile.neighbours[index]);
    for (int index = 0; index < pTile.neighbours.Length; ++index)
    {
      WorldTile neighbour = pTile.neighbours[index];
      if (neighbour.isOnFire())
        neighbour.stopFire();
    }
  }

  private static void useRainOn(WorldTile pTile)
  {
    pTile.stopFire();
    pTile.doUnits((Action<Actor>) (tActor =>
    {
      tActor.finishStatusEffect("burning");
      tActor.finishAngryStatus();
      if (tActor.isDamagedByRain())
        tActor.getHit((float) tActor.getWaterDamage(), pAttackType: AttackType.Water);
      else
        tActor.addStamina((int) ((double) tActor.getMaxStamina() * 0.10000000149011612));
    }));
    if (pTile.hasBuilding())
    {
      pTile.building.stopFire();
      if (pTile.building.asset.wheat)
        pTile.building.component_wheat.grow();
    }
    if (pTile.hasBuilding() && pTile.building.asset.damaged_by_rain)
      pTile.building.getHit(20f, true, AttackType.Other, (BaseSimObject) null, true, false, true);
    pTile.removeBurn();
    if (pTile.Type.can_be_filled_with_ocean)
      MapAction.setOcean(pTile);
    if (pTile.Type.lava)
      LavaHelper.putOut(pTile);
    if (pTile.Type.explodable_by_ocean)
      World.world.explosion_layer.explodeBomb(pTile);
    BiomeAsset biome = pTile.getBiome();
    if ((biome != null ? (biome.spread_by_drops_water ? 1 : 0) : 0) == 0)
      return;
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pTile, pTile);
  }

  public static void action_gamma_rain(WorldTile pTile = null, string pDropID = null)
  {
    List<string> traitEditorGamma = PlayerConfig.instance.data.trait_editor_gamma;
    DropsLibrary.useTraitRain(pTile, traitEditorGamma, PlayerConfig.instance.data.trait_editor_gamma_state);
  }

  public static void action_delta_rain(WorldTile pTile = null, string pDropID = null)
  {
    List<string> traitEditorDelta = PlayerConfig.instance.data.trait_editor_delta;
    DropsLibrary.useTraitRain(pTile, traitEditorDelta, PlayerConfig.instance.data.trait_editor_delta_state);
  }

  public static void action_omega_rain(WorldTile pTile = null, string pDropID = null)
  {
    List<string> traitEditorOmega = PlayerConfig.instance.data.trait_editor_omega;
    DropsLibrary.useTraitRain(pTile, traitEditorOmega, PlayerConfig.instance.data.trait_editor_omega_state);
  }

  private static void useTraitRain(WorldTile pTile, List<string> pList, RainState pRainState)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void action_equipment_rain(WorldTile pTile = null, string pDropID = null)
  {
    List<string> equipmentEditor = PlayerConfig.instance.data.equipment_editor;
    DropsLibrary.useEquipmentRain(pTile, equipmentEditor, PlayerConfig.instance.data.equipment_editor_state);
  }

  private static void useEquipmentRain(WorldTile pTile, List<string> pItems, RainState pRainState)
  {
    if (pItems.Count == 0)
      return;
    pItems.Shuffle<string>();
    using (ListPool<EquipmentAsset> listPool = new ListPool<EquipmentAsset>(pItems.Count))
    {
      HashSet<EquipmentType> equipmentTypeSet = UnsafeCollectionPool<HashSet<EquipmentType>, EquipmentType>.Get();
      for (int index = 0; index < pItems.Count; ++index)
      {
        string pItem = pItems[index];
        EquipmentAsset equipmentAsset = AssetManager.items.get(pItem);
        if (equipmentAsset != null && (pRainState != RainState.Add || !equipmentTypeSet.Contains(equipmentAsset.equipment_type)) && equipmentAsset.isAvailable() && (pRainState != RainState.Add || equipmentAsset.can_be_given) && (pRainState != RainState.Remove || equipmentAsset.can_be_removed))
        {
          equipmentTypeSet.Add(equipmentAsset.equipment_type);
          listPool.Add(equipmentAsset);
        }
      }
      UnsafeCollectionPool<HashSet<EquipmentType>, EquipmentType>.Release(equipmentTypeSet);
      foreach (Actor pActor in Finder.getUnitsFromChunk(pTile, 1, 3f))
      {
        if (pActor.canEditEquipment())
        {
          for (int index = 0; index < listPool.Count; ++index)
          {
            EquipmentAsset equipmentAsset = listPool[index];
            if (pActor.asset.canEditItem(equipmentAsset))
            {
              ActorEquipmentSlot slot = pActor.equipment.getSlot(equipmentAsset.equipment_type);
              Item obj = slot.getItem();
              if (pRainState == RainState.Remove)
              {
                if (slot.isEmpty() || obj.asset.id != equipmentAsset.id)
                  continue;
              }
              else if (!slot.isEmpty() && (obj.asset.id == equipmentAsset.id || obj.isFavorite() || obj.isCursed()))
                continue;
              if (pRainState == RainState.Remove)
              {
                obj.data.favorite = false;
                obj.removeMod("eternal");
                slot.takeAwayItem();
              }
              else
              {
                Item pItem = World.world.items.generateItem(equipmentAsset, pActor.kingdom, World.world.map_stats.player_name, pActor: pActor, pByPlayer: true);
                pItem.addMod("divine_rune");
                pActor.equipment.setItem(pItem, pActor);
              }
            }
          }
          pActor.startShake();
          pActor.makeConfused(pColorEffect: true);
        }
      }
    }
  }

  public static void action_acid(WorldTile pTile = null, string pDropID = null)
  {
    MapAction.checkAcidTerraform(pTile);
    if (Randy.randomChance(0.2f))
      World.world.particles_smoke.spawn(pTile.posV3);
    if (pTile.hasBuilding() && pTile.building.asset.affected_by_acid && pTile.building.isAlive())
      pTile.building.getHit(20f, true, AttackType.Other, (BaseSimObject) null, true, false, true);
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (!Randy.randomChance(0.6f) && !actor.hasTrait("acid_proof") && !actor.hasTrait("acid_blood"))
        actor.getHit(20f, pAttackType: AttackType.Acid);
    }
    World.world.conway_layer.checkKillRange(pTile.pos, 2);
    BiomeAsset biome = pTile.getBiome();
    if ((biome != null ? (biome.spread_by_drops_acid ? 1 : 0) : 0) == 0)
      return;
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pTile, pTile, pForce: true);
  }

  public static void action_fire(WorldTile pTile = null, string pDropID = null)
  {
    ActionLibrary.burnTile((BaseSimObject) null, pTile: pTile);
    ActionLibrary.startBurningObjects((BaseSimObject) null, pTile: pTile);
    BiomeAsset biome = pTile.getBiome();
    if ((biome != null ? (biome.spread_by_drops_fire ? 1 : 0) : 0) == 0)
      return;
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pTile, pTile, pForce: true);
  }

  public static void action_fireworks(WorldTile pTile = null, string pDropID = null)
  {
    MapAction.terraformTop(pTile, TopTileLibrary.fireworks, TerraformLibrary.remove);
  }

  public static void action_tnt(WorldTile pTile = null, string pDropID = null)
  {
    if (pTile.Type.lava || pTile.isOnFire())
    {
      MapAction.terraformTop(pTile, TopTileLibrary.tnt, TerraformLibrary.remove);
      World.world.explosion_layer.explodeBomb(pTile);
    }
    else
      MapAction.terraformTop(pTile, TopTileLibrary.tnt, TerraformLibrary.remove);
  }

  public static void action_tnt_timed(WorldTile pTile = null, string pDropID = null)
  {
    if (pTile.Type.lava || pTile.isOnFire())
    {
      MapAction.terraformTop(pTile, TopTileLibrary.tnt_timed, TerraformLibrary.remove);
      World.world.explosion_layer.explodeBomb(pTile);
    }
    else
      MapAction.terraformTop(pTile, TopTileLibrary.tnt_timed, TerraformLibrary.remove);
  }

  public static void action_czar_bomba(WorldTile pTile = null, string pDropID = null)
  {
    EffectsLibrary.spawn("fx_nuke_flash", pTile, "czar_bomba");
    World.world.startShake(pIntensity: 2.5f, pShakeX: true);
  }

  public static void action_atomic_bomb(WorldTile pTile = null, string pDropID = null)
  {
    World.world.startShake(pShakeX: true);
    EffectsLibrary.spawn("fx_nuke_flash", pTile, "atomic_bomb");
  }

  public static void action_antimatter_bomb(WorldTile pTile = null, string pDropID = null)
  {
    World.world.startShake(pIntensity: 0.03f);
    EffectsLibrary.spawn("fx_antimatter_effect", pTile);
  }

  public static void action_napalm_bomb(WorldTile pTile = null, string pDropID = null)
  {
    World.world.startShake(pIntensity: 0.5f, pShakeX: true);
    EffectsLibrary.spawn("fx_napalm_flash", pTile);
    EffectsLibrary.spawnAtTileRandomScale("fx_explosion_tiny", pTile, 0.15f, 0.3f);
  }

  public static void action_crab_bomb_impact(WorldTile pTile = null, string pDropID = null)
  {
    MusicBox.playSound("event:/SFX/DESTRUCTION/CrabBombImpact", pTile);
    int num = Randy.randomInt(1, 4);
    for (int index = 0; index < num; ++index)
      World.world.drop_manager.spawnParabolicDrop(pTile, "crab_bomb_shrapnel", 1f, 15f, 40f, 4f, 16f);
  }

  public static void action_crab_bomb_shrapnel(WorldTile pTile = null, string pDropID = null)
  {
    EffectsLibrary.spawnAt("fx_explosion_crab_bomb", pTile.posV, 0.25f);
    World.world.startShake(pIntensity: 0.5f, pShakeX: true);
    MapAction.damageWorld(pTile, 2, AssetManager.terraform.get("crab_bomb"));
    if (!Randy.randomChance(0.05f))
      return;
    DropsLibrary.action_crab_bomb_impact(pTile, "crab_bomb_shrapnel");
  }

  public static void action_grenade(WorldTile pTile = null, string pDropID = null)
  {
    MapAction.damageWorld(pTile, 5, AssetManager.terraform.get("grenade"));
    EffectsLibrary.spawnAtTileRandomScale("fx_explosion_small", pTile, 0.1f, 0.15f);
  }

  public static void action_bomb(WorldTile pTile = null, string pDropID = null)
  {
    EffectsLibrary.spawnAtTileRandomScale("fx_explosion_middle", pTile, 0.45f, 0.6f);
    if (World.world.explosion_checker.checkNearby(pTile, 10))
      return;
    MapAction.damageWorld(pTile, 10, AssetManager.terraform.get("bomb"));
  }

  public static void action_santa_bomb(WorldTile pTile = null, string pDropID = null)
  {
    MapAction.damageWorld(pTile, 10, AssetManager.terraform.get("santa_bomb"));
    EffectsLibrary.spawnAtTileRandomScale("fx_explosion_small", pTile, 0.45f, 0.6f);
  }

  public static void action_water_bomb(WorldTile pTile = null, string pDropID = null)
  {
    if (pTile.Type.liquid || pTile.Type.lava || pTile.isOnFire())
    {
      MapAction.terraformTop(pTile, TopTileLibrary.water_bomb, TerraformLibrary.remove);
      World.world.explosion_layer.explodeBomb(pTile);
    }
    else
      MapAction.terraformTop(pTile, TopTileLibrary.water_bomb, TerraformLibrary.remove);
  }

  public static void action_lava(WorldTile pTile = null, string pDropID = null)
  {
    LavaHelper.addLava(pTile);
  }

  public static void action_rage(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (Randy.randomChance(0.2f))
        actor.addStatusEffect("rage");
    }
  }

  public static void action_magic_rain(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (Randy.randomChance(0.2f))
        actor.addStatusEffect("powerup");
      if (Randy.randomChance(0.2f))
        actor.addStatusEffect("spell_boost");
      if (Randy.randomChance(0.2f))
        actor.addStatusEffect("shield");
      if (Randy.randomChance(0.2f))
        actor.addStatusEffect("caffeinated");
      actor.addMana((int) ((double) actor.getMaxMana() * 0.10000000149011612));
    }
  }

  public static void action_ash(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (Randy.randomChance(0.3f))
        actor.addStatusEffect("cough");
      if (Randy.randomChance(0.1f))
        actor.addStatusEffect("ash_fever");
    }
  }

  public static void action_life_seed(WorldTile pTile = null, string pDropID = null)
  {
    if (WorldLawLibrary.world_law_animals_spawn.isEnabled())
      DropsLibrary.trySpawnUnit(pTile);
    if (!WorldLawLibrary.world_law_vegetation_random_seeds.isEnabled())
      return;
    DropsLibrary.trySpawnVegetation(pTile);
  }

  private void action_jazz_rain(WorldTile pTile, string pDropID)
  {
    Actor pActor = (Actor) null;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f, true))
    {
      if (actor.hasSubspecies() && actor.isBreedingAge())
      {
        pActor = actor;
        break;
      }
    }
    if (pActor == null)
      return;
    BabyMaker.makeBabyFromMiracle(pActor, pAddToFamily: true);
  }

  private static void trySpawnUnit(WorldTile pTile)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    if (biomeAsset == null || !biomeAsset.pot_spawn_units_auto)
      return;
    string pID1 = biomeAsset.pot_units_spawn.GetRandom<string>();
    bool flag = false;
    if (WorldLawLibrary.world_law_drop_of_thoughts.isEnabled() && Randy.randomBool() && biomeAsset.pot_sapient_units_spawn != null)
    {
      foreach (string pID2 in biomeAsset.pot_sapient_units_spawn.LoopRandom<string>())
      {
        ActorAsset actorAsset = AssetManager.actor_library.get(pID2);
        if (actorAsset.isAvailable())
        {
          GodPower godPower = actorAsset.getGodPower();
          if (godPower == null || godPower.isAvailable())
          {
            pID1 = pID2;
            flag = true;
            break;
          }
        }
      }
    }
    ActorAsset actorAsset1 = AssetManager.actor_library.get(pID1);
    if (actorAsset1 == null || actorAsset1.units.Count > actorAsset1.max_random_amount)
      return;
    int num = 0;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1))
    {
      if (num++ > 3)
        return;
    }
    Actor actor1 = World.world.units.spawnNewUnit(actorAsset1.id, pTile);
    if (!flag || actor1 == null || !actor1.subspecies.isJustCreated())
      return;
    actor1.subspecies.makeSapient();
  }

  private static void trySpawnVegetation(WorldTile pTile)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    if (biomeAsset == null || !biomeAsset.grow_vegetation_auto)
      return;
    ActionLibrary.growRandomVegetation(pTile, biomeAsset);
  }

  public static void action_snow(WorldTile pTile = null, string pDropID = null)
  {
    if (pTile.canBeFrozen())
      pTile.freeze();
    for (int index = 0; index < 10; ++index)
    {
      WorldTile random = pTile.chunk.tiles.GetRandom<WorldTile>();
      if (random.canBeFrozen())
      {
        if ((double) Toolbox.DistTile(pTile, random) >= 11.0)
          random.freeze();
        else
          break;
      }
    }
    if (pTile.Type.lava)
      return;
    if (Randy.randomBool())
    {
      foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
        ActionLibrary.addFrozenEffectOnTarget((BaseSimObject) actor, (BaseSimObject) actor);
    }
    DropsLibrary.checkColdOneBabies(pTile);
  }

  public static void checkColdOneBabies(WorldTile pTile)
  {
    if (!WorldLawLibrary.world_law_disasters_other.isEnabled() || !World.world_era.era_disaster_snow_turns_babies_into_ice_ones)
      return;
    foreach (Actor pTarget in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (pTarget.canTurnIntoColdOne())
        ActionLibrary.turnIntoIceOne((BaseSimObject) pTarget);
    }
  }

  private static void action_cure(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 4f))
    {
      actor.removeTrait("plague");
      actor.removeTrait("tumor_infection");
      actor.removeTrait("mush_spores");
      actor.removeTrait("infected");
      actor.finishStatusEffect("ash_fever");
      actor.finishStatusEffect("cursed");
      actor.startShake();
      actor.startColorEffect();
    }
  }

  private static void action_clone_rain(WorldTile pTile, string pDropID = null)
  {
    foreach (Actor pCloneFrom in Finder.getUnitsFromChunk(pTile, 1, 1f, true))
    {
      WorldTile pTileTarget = (WorldTile) null;
      foreach (WorldTile worldTile in pCloneFrom.current_tile.neighboursAll.LoopRandom<WorldTile>())
      {
        if (!worldTile.hasUnits())
        {
          pTileTarget = worldTile;
          break;
        }
      }
      if (pTileTarget != null && World.world.units.cloneUnit(pCloneFrom, pTileTarget))
        break;
    }
  }

  private void action_sleep_rain(WorldTile pTile, string pDropID)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.makeSleep(60f) && !actor.isLying())
        actor.applyRandomForce();
    }
  }

  private void action_dispel_rain(WorldTile pTile, string pDropID)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      actor.finishStatusEffect("powerup");
      actor.finishStatusEffect("enchanted");
      actor.finishStatusEffect("slowness");
      actor.finishStatusEffect("shield");
      actor.finishStatusEffect("invincible");
      actor.finishStatusEffect("spell_boost");
      if (actor.asset.die_from_dispel)
        actor.getHit((float) actor.getMaxHealthPercent(0.5f));
    }
  }

  public static void action_blood_rain(Drop pDrop, WorldTile pTile = null, string pDropID = null)
  {
    long casterId = pDrop.getCasterId();
    Actor pObject = World.world.units.get(casterId);
    bool flag = !pObject.isRekt();
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (!flag || actor.id == casterId || !pObject.kingdom.isEnemy(actor.kingdom))
      {
        actor.finishStatusEffect("burning");
        actor.restoreHealth(actor.getMaxHealthPercent(0.2f));
        actor.startShake();
        actor.startColorEffect();
      }
    }
  }

  public static void action_plague(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 4f))
    {
      if (actor.hasTrait("plague"))
      {
        actor.startShake();
        actor.startColorEffect();
      }
      else
        actor.addTrait("plague");
    }
  }

  public static void action_zombie_infection(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.can_turn_into_zombie && !actor.hasTrait("zombie"))
      {
        actor.addTrait("infected");
        actor.startShake();
        actor.startColorEffect();
      }
    }
  }

  public static void action_mush_spore(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.can_turn_into_mush && !actor.hasTrait("mush_spores"))
      {
        actor.addTrait("mush_spores");
        actor.startShake();
        actor.startColorEffect();
      }
    }
  }

  private static void action_curse(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.addStatusEffect("cursed"))
      {
        actor.setStatsDirty();
        actor.removeTrait("blessed");
        actor.startShake();
        actor.startColorEffect();
      }
    }
    BiomeAsset biome = pTile.getBiome();
    if ((biome != null ? (biome.spread_by_drops_curse ? 1 : 0) : 0) == 0)
      return;
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pTile, pTile, pForce: true);
  }

  private static void action_spell_silence(WorldTile pTile = null, string pDropID = null)
  {
    foreach (BaseSimObject baseSimObject in Finder.getUnitsFromChunk(pTile, 1, 3f))
      baseSimObject.addStatusEffect("spell_silence");
  }

  private static void action_shield(WorldTile pTile = null, string pDropID = null)
  {
    foreach (BaseSimObject baseSimObject in Finder.getUnitsFromChunk(pTile, 1, 3f))
      baseSimObject.addStatusEffect("shield");
  }

  private static void action_powerup(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      actor.addStatusEffect("powerup");
      if (actor.isSameSpecies("mush_unit") || actor.isSameSpecies("mush_animal"))
        AchievementLibrary.super_mushroom.check();
    }
    BiomeAsset biome = pTile.getBiome();
    if ((biome != null ? (biome.spread_by_drops_powerup ? 1 : 0) : 0) == 0)
      return;
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pTile, pTile, pForce: true);
  }

  private static void action_paint(WorldTile pTile = null, string pDropID = null)
  {
    TileZone zone = pTile.zone;
    if (!zone.hasCity())
      return;
    City city = zone.city;
    World.world.city_zone_helper.city_growth.getZoneToClaim((Actor) null, city, true, DropsLibrary._paint_zones_hashset, 1);
    using (ListPool<TileZone> list = new ListPool<TileZone>())
    {
      foreach (TileZone tileZone in DropsLibrary._paint_zones_hashset)
      {
        if (!tileZone.hasCity())
        {
          foreach (TileZone neighbour in tileZone.neighbours)
          {
            if (neighbour.city == city)
              list.Add(tileZone);
          }
        }
      }
      if (list.Count > 0)
      {
        TileZone random = list.GetRandom<TileZone>();
        city.addZone(random);
        city.setAbandonedZonesDirty();
      }
      DropsLibrary._paint_zones_hashset.Clear();
    }
  }

  public static void action_dust_black(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.affected_by_dust)
        actor.makeConfused(pColorEffect: true);
    }
  }

  public static void action_dust_white(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.affected_by_dust)
        actor.forgetLanguage();
    }
  }

  public static void action_dust_red(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.affected_by_dust)
      {
        actor.makeConfused(pColorEffect: true);
        if (actor.hasFamily())
          actor.setFamily((Family) null);
        if (actor.hasClan())
          actor.forgetClan();
        if (actor.hasLover())
        {
          Actor lover = actor.lover;
          actor.setLover((Actor) null);
          lover.setLover((Actor) null);
        }
      }
    }
  }

  public static void action_dust_blue(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.affected_by_dust)
        actor.forgetCulture();
    }
  }

  public static void action_dust_gold(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.affected_by_dust)
        actor.forgetKingdomAndCity();
    }
  }

  public static void action_dust_purple(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.asset.affected_by_dust)
        actor.forgetReligion();
    }
  }

  public static void action_coffee(WorldTile pTile = null, string pDropID = null)
  {
    foreach (BaseSimObject baseSimObject in Finder.getUnitsFromChunk(pTile, 1, 3f))
      baseSimObject.addStatusEffect("caffeinated");
    BiomeAsset biome = pTile.getBiome();
    if ((biome != null ? (biome.spread_by_drops_coffee ? 1 : 0) : 0) == 0)
      return;
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pTile, pTile, pForce: true);
  }

  public static void action_blessing(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
    {
      if (actor.addTrait("blessed"))
      {
        actor.setStatsDirty();
        actor.event_full_stats = true;
      }
      actor.finishStatusEffect("cursed");
      actor.startShake();
      if (actor.isSameSpecies("frog"))
        AchievementLibrary.the_princess.check();
      actor.startColorEffect();
    }
    BiomeAsset biome = pTile.getBiome();
    if ((biome != null ? (biome.spread_by_drops_blessing ? 1 : 0) : 0) == 0)
      return;
    WorldBehaviourActionBiomes.trySpreadBiomeAround(pTile, pTile, pForce: true);
  }

  public static void action_alien_mold(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
      actor.addTrait("desire_alien_mold");
  }

  public static void action_drop_computer(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
      actor.addTrait("desire_computer");
  }

  public static void action_drop_golden_egg(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
      actor.addTrait("desire_golden_egg");
  }

  public static void action_drop_harp(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
      actor.addTrait("desire_harp");
  }

  public static void action_madness(WorldTile pTile = null, string pDropID = null)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f))
      actor.addTrait("madness");
  }

  public static void action_inspiration(WorldTile pTile, string pDropID = null)
  {
    if (!pTile.zone.hasCity() || World.world.cities.isLocked())
      return;
    City zoneCity = pTile.zone_city;
    if (zoneCity.isNeutral() || zoneCity.kingdom.countCities() == 1 || zoneCity.isCapitalCity() || !zoneCity.hasLeader())
      return;
    zoneCity.leader.addStatusEffect("voices_in_my_head");
    zoneCity.useInspire(zoneCity.leader);
  }

  public static void action_discord(WorldTile pTile, string pDropID = null)
  {
    if (!pTile.zone.hasCity())
      return;
    City zoneCity = pTile.zone_city;
    if (zoneCity == null || zoneCity.isNeutral())
      return;
    Alliance alliance = zoneCity.kingdom.getAlliance();
    if (alliance == null)
      return;
    World.world.alliances.useDiscordPower(alliance, zoneCity);
  }

  public static void action_spite(WorldTile pTile, string pDropID = null)
  {
    if (!pTile.zone.hasCity())
      return;
    Kingdom kingdom = pTile.zone.city.kingdom;
    if (kingdom.isNeutral())
      return;
    World.world.diplomacy.eventSpite(kingdom);
  }

  public static void action_friendship(WorldTile pTile, string pDropID = null)
  {
    if (!pTile.zone.hasCity())
      return;
    Kingdom kingdom = pTile.zone.city.kingdom;
    if (kingdom.isNeutral())
      return;
    World.world.diplomacy.eventFriendship(kingdom);
  }

  public static void action_spawn_building(WorldTile pTile = null, string pDropID = null)
  {
    string randomBuildingAsset = AssetManager.drops.get(pDropID).getRandomBuildingAsset();
    BuildingAsset buildingAsset = AssetManager.buildings.get(randomBuildingAsset);
    Building pBuildingToIgnore = World.world.buildings.addBuilding(randomBuildingAsset, pTile, true);
    if (pBuildingToIgnore == null)
      EffectsLibrary.spawnAtTile("fx_bad_place", pTile, 0.25f);
    else
      buildingAsset.checkLimits(pBuildingToIgnore);
  }

  public static void flash(WorldTile pTile, string pDropID)
  {
    World.world.flash_effects.flashPixel(pTile, 20);
  }

  public static void action_fertilizer_plants(WorldTile pTile = null, string pDropID = null)
  {
    BuildingActions.tryGrowVegetationRandom(pTile, VegetationType.Plants, pCheckLimit: false, pCheckRandom: false);
    if (pTile.Type.biome_asset == null || pTile.Type.biome_asset.grow_type_selector_plants != null)
      return;
    EffectsLibrary.spawnAtTile("fx_bad_place", pTile, 0.25f);
  }

  public static void action_fertilizer_trees(WorldTile pTile = null, string pDropID = null)
  {
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    BuildingActions.tryGrowVegetationRandom(pTile, VegetationType.Trees, pCheckLimit: false, pCheckRandom: false);
    if (biomeAsset == null || biomeAsset.grow_type_selector_trees != null)
      return;
    EffectsLibrary.spawnAtTile("fx_bad_place", pTile, 0.25f);
  }

  public static void action_fruit_bush(WorldTile pTile = null, string pDropID = null)
  {
    BuildingAsset buildingAsset = AssetManager.buildings.get("fruit_bush");
    BuildingActions.tryGrowVegetation(pTile, buildingAsset.id, true, false);
    if (buildingAsset.isOverlaysBiomeTags(pTile.Type))
      return;
    EffectsLibrary.spawnAtTile("fx_bad_place", pTile, 0.25f);
  }

  public static void action_landmine(WorldTile pTile = null, string pDropID = null)
  {
    if (pTile.Type.lava)
      World.world.explosion_layer.explodeBomb(pTile);
    else
      MapAction.terraformTop(pTile, TopTileLibrary.landmine, TerraformLibrary.remove);
  }

  public static void action_living_house(WorldTile pTile = null, string pDropID = null)
  {
    TileZone zone = pTile.zone;
    if (!zone.hasAnyBuildings())
      return;
    using (ListPool<Building> listPool = new ListPool<Building>())
    {
      if (zone.hasAnyBuildingsInSet(BuildingList.Civs))
        listPool.AddRange((IEnumerable<Building>) zone.getHashset(BuildingList.Civs));
      if (zone.hasAnyBuildingsInSet(BuildingList.Ruins))
        listPool.AddRange((IEnumerable<Building>) zone.getHashset(BuildingList.Ruins));
      if (zone.hasAnyBuildingsInSet(BuildingList.Abandoned))
        listPool.AddRange((IEnumerable<Building>) zone.getHashset(BuildingList.Abandoned));
      for (int index = 0; index < listPool.Count; ++index)
        ActionLibrary.tryToMakeBuildingAlive(listPool[index]);
    }
  }

  public static void action_living_plants(WorldTile pTile = null, string pDropID = null)
  {
    TileZone zone = pTile.zone;
    if (!zone.hasAnyBuildings())
      return;
    using (ListPool<Building> listPool = new ListPool<Building>())
    {
      if (zone.hasAnyBuildingsInSet(BuildingList.Food))
        listPool.AddRange((IEnumerable<Building>) zone.getHashset(BuildingList.Food));
      if (zone.hasAnyBuildingsInSet(BuildingList.Trees))
        listPool.AddRange((IEnumerable<Building>) zone.getHashset(BuildingList.Trees));
      if (zone.hasAnyBuildingsInSet(BuildingList.Wheat))
        listPool.AddRange((IEnumerable<Building>) zone.getHashset(BuildingList.Wheat));
      for (int index = 0; index < listPool.Count; ++index)
        ActionLibrary.tryToMakeFloraAlive(listPool[index]);
    }
  }
}
