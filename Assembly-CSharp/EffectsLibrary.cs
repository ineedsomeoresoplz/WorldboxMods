// Decompiled with JetBrains decompiler
// Type: EffectsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectsLibrary : AssetLibrary<EffectAsset>
{
  public override void init()
  {
    base.init();
    EffectAsset pAsset1 = new EffectAsset();
    pAsset1.id = "fx_spores";
    pAsset1.prefab_id = "effects/prefabs/PrefabSpores";
    pAsset1.show_on_mini_map = true;
    pAsset1.limit = 200;
    this.add(pAsset1);
    EffectAsset pAsset2 = new EffectAsset();
    pAsset2.id = "fx_fireball_explosion";
    pAsset2.sprite_path = "effects/fx_fireball_explosion";
    pAsset2.sorting_layer_id = "EffectsTop";
    pAsset2.use_basic_prefab = true;
    pAsset2.draw_light_area = true;
    pAsset2.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionSmall";
    this.add(pAsset2);
    EffectAsset pAsset3 = new EffectAsset();
    pAsset3.id = "fx_firebomb_explosion";
    pAsset3.sprite_path = "effects/fx_firebomb_explosion";
    pAsset3.sorting_layer_id = "EffectsTop";
    pAsset3.use_basic_prefab = true;
    pAsset3.draw_light_area = true;
    pAsset3.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionSmall";
    this.add(pAsset3);
    EffectAsset pAsset4 = new EffectAsset();
    pAsset4.id = "fx_plasma_ball_explosion";
    pAsset4.sprite_path = "effects/fx_plasma_ball_explosion";
    pAsset4.sorting_layer_id = "EffectsTop";
    pAsset4.use_basic_prefab = true;
    pAsset4.draw_light_area = true;
    this.add(pAsset4);
    EffectAsset pAsset5 = new EffectAsset();
    pAsset5.id = "fx_cast_ground_blue";
    pAsset5.use_basic_prefab = true;
    pAsset5.sorting_layer_id = "EffectsBack";
    pAsset5.sprite_path = "effects/fx_cast_ground_blue_t";
    pAsset5.draw_light_area = true;
    pAsset5.draw_light_size = 0.2f;
    pAsset5.limit = 60;
    this.add(pAsset5);
    EffectAsset pAsset6 = new EffectAsset();
    pAsset6.id = "fx_cast_top_blue";
    pAsset6.use_basic_prefab = true;
    pAsset6.sorting_layer_id = "EffectsTop";
    pAsset6.sprite_path = "effects/fx_cast_top_blue_t";
    pAsset6.draw_light_area = true;
    pAsset6.draw_light_size = 0.2f;
    pAsset6.limit = 60;
    this.add(pAsset6);
    EffectAsset pAsset7 = new EffectAsset();
    pAsset7.id = "fx_cast_ground_red";
    pAsset7.use_basic_prefab = true;
    pAsset7.sorting_layer_id = "EffectsBack";
    pAsset7.sprite_path = "effects/fx_cast_ground_red_t";
    pAsset7.draw_light_area = true;
    pAsset7.draw_light_size = 0.2f;
    pAsset7.limit = 60;
    this.add(pAsset7);
    EffectAsset pAsset8 = new EffectAsset();
    pAsset8.id = "fx_cast_top_red";
    pAsset8.use_basic_prefab = true;
    pAsset8.sorting_layer_id = "EffectsTop";
    pAsset8.sprite_path = "effects/fx_cast_top_red_t";
    pAsset8.draw_light_area = true;
    pAsset8.draw_light_size = 0.2f;
    pAsset8.limit = 60;
    this.add(pAsset8);
    EffectAsset pAsset9 = new EffectAsset();
    pAsset9.id = "fx_cast_ground_green";
    pAsset9.use_basic_prefab = true;
    pAsset9.sorting_layer_id = "EffectsBack";
    pAsset9.sprite_path = "effects/fx_cast_ground_green_t";
    pAsset9.draw_light_area = true;
    pAsset9.draw_light_size = 0.2f;
    pAsset9.limit = 60;
    this.add(pAsset9);
    EffectAsset pAsset10 = new EffectAsset();
    pAsset10.id = "fx_cast_ground_purple";
    pAsset10.use_basic_prefab = true;
    pAsset10.sorting_layer_id = "EffectsBack";
    pAsset10.sprite_path = "effects/fx_cast_ground_purple_t";
    pAsset10.draw_light_area = true;
    pAsset10.draw_light_size = 0.2f;
    pAsset10.limit = 60;
    this.add(pAsset10);
    EffectAsset pAsset11 = new EffectAsset();
    pAsset11.id = "fx_cast_top_green";
    pAsset11.use_basic_prefab = true;
    pAsset11.sorting_layer_id = "EffectsTop";
    pAsset11.sprite_path = "effects/fx_cast_top_green_t";
    pAsset11.draw_light_area = true;
    pAsset11.draw_light_size = 0.2f;
    pAsset11.limit = 60;
    this.add(pAsset11);
    EffectAsset pAsset12 = new EffectAsset();
    pAsset12.id = "fx_create_skeleton";
    pAsset12.use_basic_prefab = true;
    pAsset12.sorting_layer_id = "EffectsTop";
    pAsset12.sprite_path = "effects/fx_create_skeleton_t";
    pAsset12.draw_light_area = true;
    pAsset12.show_on_mini_map = true;
    pAsset12.limit = 0;
    this.add(pAsset12);
    EffectAsset pAsset13 = new EffectAsset();
    pAsset13.id = "fx_teleport_blue";
    pAsset13.use_basic_prefab = true;
    pAsset13.sorting_layer_id = "EffectsTop";
    pAsset13.sprite_path = "effects/fx_teleport_blue_t";
    pAsset13.draw_light_area = true;
    pAsset13.limit = 100;
    this.add(pAsset13);
    EffectAsset pAsset14 = new EffectAsset();
    pAsset14.id = "fx_teleport_red";
    pAsset14.use_basic_prefab = true;
    pAsset14.sorting_layer_id = "EffectsTop";
    pAsset14.sprite_path = "effects/fx_teleport_red_t";
    pAsset14.draw_light_area = true;
    pAsset14.limit = 100;
    this.add(pAsset14);
    EffectAsset pAsset15 = new EffectAsset();
    pAsset15.id = "fx_shield_hit";
    pAsset15.use_basic_prefab = true;
    pAsset15.sorting_layer_id = "EffectsTop";
    pAsset15.sprite_path = "effects/fx_shield_hit_t";
    pAsset15.draw_light_area = true;
    pAsset15.limit = 200;
    this.add(pAsset15);
    EffectAsset pAsset16 = new EffectAsset();
    pAsset16.id = "fx_dodge";
    pAsset16.use_basic_prefab = true;
    pAsset16.sorting_layer_id = "EffectsBack";
    pAsset16.sprite_path = "effects/combat_actions/fx_action_dodge_t";
    pAsset16.limit = 100;
    this.add(pAsset16);
    EffectAsset pAsset17 = new EffectAsset();
    pAsset17.id = "fx_block";
    pAsset17.use_basic_prefab = true;
    pAsset17.sorting_layer_id = "EffectsTop";
    pAsset17.sprite_path = "effects/combat_actions/fx_action_block_t";
    pAsset17.limit = 10;
    this.add(pAsset17);
    EffectAsset pAsset18 = new EffectAsset();
    pAsset18.id = "fx_drowning";
    pAsset18.use_basic_prefab = true;
    pAsset18.sorting_layer_id = "EffectsTop";
    pAsset18.sprite_path = "effects/fx_status_drowning_t";
    pAsset18.limit = 50;
    this.add(pAsset18);
    EffectAsset pAsset19 = new EffectAsset();
    pAsset19.id = "fx_water_splash";
    pAsset19.use_basic_prefab = true;
    pAsset19.sorting_layer_id = "EffectsTop";
    pAsset19.sprite_path = "effects/fx_status_drowning_t";
    pAsset19.limit_unload = true;
    pAsset19.limit = 50;
    this.add(pAsset19);
    EffectAsset pAsset20 = new EffectAsset();
    pAsset20.id = "fx_grin_reaper";
    pAsset20.use_basic_prefab = true;
    pAsset20.sorting_layer_id = "EffectsTop";
    pAsset20.sprite_path = "effects/fx_grin_reaper_animation";
    pAsset20.show_on_mini_map = false;
    pAsset20.time_between_frames = 1f / 1000f;
    pAsset20.limit = 20;
    this.add(pAsset20);
    EffectAsset pAsset21 = new EffectAsset();
    pAsset21.id = "fx_monolith_launch";
    pAsset21.use_basic_prefab = true;
    pAsset21.sorting_layer_id = "EffectsTop";
    pAsset21.sprite_path = "effects/fx_monolith_launch";
    pAsset21.limit = 10;
    this.add(pAsset21);
    EffectAsset pAsset22 = new EffectAsset();
    pAsset22.id = "fx_monolith_launch_bottom";
    pAsset22.use_basic_prefab = true;
    pAsset22.sorting_layer_id = "EffectsBack";
    pAsset22.sprite_path = "effects/fx_monolith_launch_bottom";
    pAsset22.limit = 10;
    this.add(pAsset22);
    EffectAsset pAsset23 = new EffectAsset();
    pAsset23.id = "fx_monolith_glow_1";
    pAsset23.use_basic_prefab = true;
    pAsset23.sorting_layer_id = "Objects";
    pAsset23.sprite_path = "effects/fx_monolith_glow_1";
    pAsset23.limit = 10;
    this.add(pAsset23);
    EffectAsset pAsset24 = new EffectAsset();
    pAsset24.id = "fx_monolith_glow_2";
    pAsset24.use_basic_prefab = true;
    pAsset24.sorting_layer_id = "Objects";
    pAsset24.sprite_path = "effects/fx_monolith_glow_2";
    pAsset24.limit = 10;
    this.add(pAsset24);
    EffectAsset pAsset25 = new EffectAsset();
    pAsset25.id = "fx_waypoint_alien_mold_launch_bottom";
    pAsset25.use_basic_prefab = true;
    pAsset25.sorting_layer_id = "EffectsBack";
    pAsset25.sprite_path = "effects/fx_waypoint_alien_mold_launch_bottom";
    pAsset25.limit = 10;
    this.add(pAsset25);
    EffectAsset pAsset26 = new EffectAsset();
    pAsset26.id = "fx_waypoint_computer_launch_bottom";
    pAsset26.use_basic_prefab = true;
    pAsset26.sorting_layer_id = "EffectsTop";
    pAsset26.sprite_path = "effects/fx_waypoint_computer_launch_bottom";
    pAsset26.limit = 10;
    this.add(pAsset26);
    EffectAsset pAsset27 = new EffectAsset();
    pAsset27.id = "fx_waypoint_golden_egg_launch_bottom";
    pAsset27.use_basic_prefab = true;
    pAsset27.sorting_layer_id = "EffectsTop";
    pAsset27.sprite_path = "effects/fx_waypoint_golden_egg_launch_bottom";
    pAsset27.limit = 10;
    this.add(pAsset27);
    EffectAsset pAsset28 = new EffectAsset();
    pAsset28.id = "fx_waypoint_harp_launch_bottom";
    pAsset28.use_basic_prefab = true;
    pAsset28.sorting_layer_id = "EffectsBack";
    pAsset28.sprite_path = "effects/fx_waypoint_harp_launch_bottom";
    pAsset28.limit = 10;
    this.add(pAsset28);
    EffectAsset pAsset29 = new EffectAsset();
    pAsset29.id = "fx_bad_place";
    pAsset29.use_basic_prefab = true;
    pAsset29.sorting_layer_id = "EffectsBack";
    pAsset29.sprite_path = "effects/fx_bad_place_t";
    pAsset29.draw_light_area = true;
    pAsset29.limit = 10;
    pAsset29.show_on_mini_map = true;
    this.add(pAsset29);
    EffectAsset pAsset30 = new EffectAsset();
    pAsset30.id = "fx_debug_tile";
    pAsset30.use_basic_prefab = true;
    pAsset30.sorting_layer_id = "EffectsTop";
    pAsset30.sprite_path = "effects/fx_debug_tile";
    pAsset30.draw_light_area = true;
    pAsset30.time_between_frames = 3f;
    pAsset30.show_on_mini_map = true;
    this.add(pAsset30);
    EffectAsset pAsset31 = new EffectAsset();
    pAsset31.id = "fx_move";
    pAsset31.use_basic_prefab = true;
    pAsset31.sorting_layer_id = "EffectsBack";
    pAsset31.sprite_path = "effects/fx_move_t";
    pAsset31.draw_light_area = true;
    pAsset31.limit = 30;
    pAsset31.show_on_mini_map = true;
    this.add(pAsset31);
    EffectAsset pAsset32 = new EffectAsset();
    pAsset32.id = "fx_plasma_trail";
    pAsset32.use_basic_prefab = true;
    pAsset32.sorting_layer_id = "EffectsTop";
    pAsset32.sprite_path = "effects/fx_plasma_trail_t";
    pAsset32.draw_light_area = true;
    pAsset32.show_on_mini_map = true;
    pAsset32.limit = 15;
    this.add(pAsset32);
    EffectAsset pAsset33 = new EffectAsset();
    pAsset33.id = "fx_building_sparkle";
    pAsset33.use_basic_prefab = true;
    pAsset33.sorting_layer_id = "EffectsTop";
    pAsset33.sprite_path = "effects/fx_building_sparkle_t";
    pAsset33.limit = 15;
    this.add(pAsset33);
    EffectAsset pAsset34 = new EffectAsset();
    pAsset34.id = "fx_fire_smoke";
    pAsset34.prefab_id = "Prefabs/PrefabFireSmoke";
    pAsset34.show_on_mini_map = true;
    this.add(pAsset34);
    EffectAsset pAsset35 = new EffectAsset();
    pAsset35.id = "fx_boulder_charge";
    pAsset35.prefab_id = "Prefabs/PrefabBoulderCharge";
    pAsset35.show_on_mini_map = true;
    this.add(pAsset35);
    EffectAsset pAsset36 = new EffectAsset();
    pAsset36.id = "fx_spark";
    pAsset36.prefab_id = "Prefabs/PrefabSpark";
    pAsset36.show_on_mini_map = true;
    this.add(pAsset36);
    EffectAsset pAsset37 = new EffectAsset();
    pAsset37.id = "fx_lightning_big";
    pAsset37.prefab_id = "effects/prefabs/PrefabLightning";
    pAsset37.show_on_mini_map = true;
    pAsset37.limit = 100;
    pAsset37.draw_light_area = true;
    pAsset37.draw_light_size = 2f;
    pAsset37.draw_light_area_offset_y = 5f;
    pAsset37.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionLightningStrike";
    this.add(pAsset37);
    EffectAsset pAsset38 = new EffectAsset();
    pAsset38.id = "fx_lightning_medium";
    pAsset38.prefab_id = "effects/prefabs/PrefabLightningMedium";
    pAsset38.show_on_mini_map = true;
    pAsset38.limit = 100;
    pAsset38.draw_light_area = true;
    pAsset38.draw_light_size = 2f;
    pAsset38.draw_light_area_offset_y = 5f;
    pAsset38.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionLightningStrike";
    this.add(pAsset38);
    EffectAsset pAsset39 = new EffectAsset();
    pAsset39.id = "fx_lightning_small";
    pAsset39.prefab_id = "effects/prefabs/PrefabLightningSmall";
    pAsset39.show_on_mini_map = true;
    pAsset39.limit = 100;
    pAsset39.draw_light_area = true;
    pAsset39.draw_light_size = 2f;
    pAsset39.draw_light_area_offset_y = 5f;
    pAsset39.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionLightningStrike";
    this.add(pAsset39);
    EffectAsset pAsset40 = new EffectAsset();
    pAsset40.id = "fx_spawn";
    pAsset40.prefab_id = "effects/prefabs/PrefabSpawnSmall";
    pAsset40.show_on_mini_map = false;
    pAsset40.draw_light_area = true;
    pAsset40.spawn_action = new EffectAction(this.showSpawnEffect);
    pAsset40.limit = 100;
    this.add(pAsset40);
    EffectAsset pAsset41 = new EffectAsset();
    pAsset41.id = "fx_teleport_singularity";
    pAsset41.use_basic_prefab = true;
    pAsset41.sorting_layer_id = "EffectsTop";
    pAsset41.sprite_path = "effects/fx_teleport_singularity";
    pAsset41.draw_light_area = true;
    pAsset41.limit = 0;
    this.add(pAsset41);
    EffectAsset pAsset42 = new EffectAsset();
    pAsset42.id = "fx_spawn_big";
    pAsset42.prefab_id = "effects/prefabs/PrefabSpawnBig";
    pAsset42.show_on_mini_map = true;
    pAsset42.spawn_action = new EffectAction(this.spawnSimpleTile);
    pAsset42.draw_light_area = true;
    pAsset42.draw_light_size = 2f;
    pAsset42.sound_launch = "event:/SFX/UNIQUE/Crabzilla/CrabzillaSpawn";
    this.add(pAsset42);
    EffectAsset pAsset43 = new EffectAsset();
    pAsset43.id = "fx_land_explosion_old";
    pAsset43.prefab_id = "effects/prefabs/PrefabFireballExplosion";
    pAsset43.show_on_mini_map = true;
    pAsset43.draw_light_area = true;
    pAsset43.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionSmall";
    this.add(pAsset43);
    EffectAsset pAsset44 = new EffectAsset();
    pAsset44.id = "fx_explosion_crab_bomb";
    pAsset44.prefab_id = "effects/prefabs/PrefabFireballExplosion";
    pAsset44.show_on_mini_map = true;
    pAsset44.draw_light_area = true;
    pAsset44.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionCrabBomb";
    this.add(pAsset44);
    EffectAsset pAsset45 = new EffectAsset();
    pAsset45.id = "fx_explosion_tiny";
    pAsset45.prefab_id = "effects/prefabs/PrefabExplosionSmall";
    pAsset45.show_on_mini_map = true;
    pAsset45.draw_light_area = true;
    pAsset45.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionTiny";
    this.add(pAsset45);
    EffectAsset pAsset46 = new EffectAsset();
    pAsset46.id = "fx_explosion_small";
    pAsset46.prefab_id = "effects/prefabs/PrefabExplosionSmall";
    pAsset46.show_on_mini_map = true;
    pAsset46.draw_light_area = true;
    pAsset46.draw_light_size = 1f;
    pAsset46.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionSmall";
    this.add(pAsset46);
    EffectAsset pAsset47 = new EffectAsset();
    pAsset47.id = "fx_explosion_ufo";
    pAsset47.prefab_id = "effects/prefabs/PrefabExplosionSmall";
    pAsset47.show_on_mini_map = true;
    pAsset47.draw_light_area = true;
    pAsset47.draw_light_size = 1f;
    pAsset47.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionUFO";
    this.add(pAsset47);
    EffectAsset pAsset48 = new EffectAsset();
    pAsset48.id = "fx_explosion_meteorite";
    pAsset48.prefab_id = "effects/prefabs/PrefabExplosionSmall";
    pAsset48.show_on_mini_map = true;
    pAsset48.draw_light_area = true;
    pAsset48.draw_light_size = 2f;
    pAsset48.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionMeteorite";
    this.add(pAsset48);
    EffectAsset pAsset49 = new EffectAsset();
    pAsset49.id = "fx_explosion_middle";
    pAsset49.prefab_id = "effects/prefabs/PrefabExplosionSmall";
    pAsset49.show_on_mini_map = true;
    pAsset49.draw_light_area = true;
    pAsset49.draw_light_size = 2f;
    pAsset49.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionSmall";
    this.add(pAsset49);
    EffectAsset pAsset50 = new EffectAsset();
    pAsset50.id = "fx_explosion_nuke_atomic";
    pAsset50.show_on_mini_map = true;
    pAsset50.prefab_id = "effects/prefabs/PrefabExplosionBig";
    pAsset50.draw_light_area = true;
    pAsset50.draw_light_size = 5f;
    pAsset50.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionBig";
    this.add(pAsset50);
    EffectAsset pAsset51 = new EffectAsset();
    pAsset51.id = "fx_explosion_huge";
    pAsset51.show_on_mini_map = true;
    pAsset51.prefab_id = "effects/prefabs/PrefabExplosionBig";
    pAsset51.draw_light_area = true;
    pAsset51.draw_light_size = 5f;
    pAsset51.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionHuge";
    this.add(pAsset51);
    EffectAsset pAsset52 = new EffectAsset();
    pAsset52.id = "fx_explosion_wave";
    pAsset52.prefab_id = "effects/prefabs/PrefabExplosionWave";
    pAsset52.show_on_mini_map = true;
    this.add(pAsset52);
    EffectAsset pAsset53 = new EffectAsset();
    pAsset53.id = "fx_fireworks";
    pAsset53.prefab_id = "effects/prefabs/PrefabFireworks";
    pAsset53.show_on_mini_map = true;
    pAsset53.spawn_action = new EffectAction(this.spawnFireworks);
    pAsset53.cooldown_interval = 0.20000000298023224;
    pAsset53.draw_light_area = true;
    pAsset53.draw_light_size = 4f;
    pAsset53.draw_light_area_offset_y = 40f;
    pAsset53.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionFireworks";
    this.add(pAsset53);
    EffectAsset pAsset54 = new EffectAsset();
    pAsset54.id = "fx_hearts";
    pAsset54.prefab_id = "effects/prefabs/PrefabHearts";
    pAsset54.sorting_layer_id = "EffectsTop";
    pAsset54.limit = 10;
    this.add(pAsset54);
    EffectAsset pAsset55 = new EffectAsset();
    pAsset55.id = "fx_new_border";
    pAsset55.prefab_id = "effects/prefabs/PrefabNewBorder";
    pAsset55.sorting_layer_id = "EffectsTop";
    pAsset55.limit = 10;
    this.add(pAsset55);
    EffectAsset pAsset56 = new EffectAsset();
    pAsset56.id = "fx_money_got_loot";
    pAsset56.prefab_id = "effects/prefabs/PrefabMoneyGotLoot";
    pAsset56.sorting_layer_id = "EffectsTop";
    pAsset56.limit = 10;
    this.add(pAsset56);
    EffectAsset pAsset57 = new EffectAsset();
    pAsset57.id = "fx_money_got_money";
    pAsset57.prefab_id = "effects/prefabs/PrefabMoneyGotMoney";
    pAsset57.sorting_layer_id = "EffectsTop";
    pAsset57.limit = 10;
    this.add(pAsset57);
    EffectAsset pAsset58 = new EffectAsset();
    pAsset58.id = "fx_money_paid_tax";
    pAsset58.prefab_id = "effects/prefabs/PrefabMoneyPaidTax";
    pAsset58.sorting_layer_id = "EffectsTop";
    pAsset58.limit = 10;
    this.add(pAsset58);
    EffectAsset pAsset59 = new EffectAsset();
    pAsset59.id = "fx_money_paid_tribute";
    pAsset59.prefab_id = "effects/prefabs/PrefabMoneyPaidTribute";
    pAsset59.sorting_layer_id = "EffectsTop";
    pAsset59.limit = 10;
    this.add(pAsset59);
    EffectAsset pAsset60 = new EffectAsset();
    pAsset60.id = "fx_money_spend_money";
    pAsset60.prefab_id = "effects/prefabs/PrefabMoneySpendMoney";
    pAsset60.sorting_layer_id = "EffectsTop";
    pAsset60.limit = 10;
    this.add(pAsset60);
    EffectAsset pAsset61 = new EffectAsset();
    pAsset61.id = "fx_conversion_religion";
    pAsset61.load_texture = true;
    pAsset61.prefab_id = "effects/prefabs/PrefabMetaEvent";
    pAsset61.sprite_path = "effects/fx_conversion_religion";
    pAsset61.sorting_layer_id = "EffectsTop";
    pAsset61.limit = 10;
    this.add(pAsset61);
    EffectAsset pAsset62 = new EffectAsset();
    pAsset62.id = "fx_conversion_culture";
    pAsset62.load_texture = true;
    pAsset62.prefab_id = "effects/prefabs/PrefabMetaEvent";
    pAsset62.sprite_path = "effects/fx_conversion_culture";
    pAsset62.sorting_layer_id = "EffectsTop";
    pAsset62.limit = 10;
    this.add(pAsset62);
    EffectAsset pAsset63 = new EffectAsset();
    pAsset63.id = "fx_conversion_language";
    pAsset63.load_texture = true;
    pAsset63.prefab_id = "effects/prefabs/PrefabMetaEvent";
    pAsset63.sprite_path = "effects/fx_conversion_language";
    pAsset63.sorting_layer_id = "EffectsTop";
    pAsset63.limit = 10;
    this.add(pAsset63);
    EffectAsset pAsset64 = new EffectAsset();
    pAsset64.id = "fx_experience_gain";
    pAsset64.load_texture = true;
    pAsset64.prefab_id = "effects/prefabs/PrefabMetaEvent";
    pAsset64.sprite_path = "effects/fx_experience_gain";
    pAsset64.sorting_layer_id = "EffectsTop";
    pAsset64.limit = 10;
    this.add(pAsset64);
    EffectAsset pAsset65 = new EffectAsset();
    pAsset65.id = "fx_change_happiness_positive";
    pAsset65.load_texture = true;
    pAsset65.prefab_id = "effects/prefabs/PrefabMetaEvent";
    pAsset65.sprite_path = "effects/fx_change_happiness_positive";
    pAsset65.sorting_layer_id = "EffectsTop";
    pAsset65.limit = 10;
    this.add(pAsset65);
    EffectAsset pAsset66 = new EffectAsset();
    pAsset66.id = "fx_change_happiness_negative";
    pAsset66.load_texture = true;
    pAsset66.prefab_id = "effects/prefabs/PrefabMetaEvent";
    pAsset66.sprite_path = "effects/fx_change_happiness_negative";
    pAsset66.sorting_layer_id = "EffectsTop";
    pAsset66.limit = 10;
    this.add(pAsset66);
    EffectAsset pAsset67 = new EffectAsset();
    pAsset67.id = "fx_hmm";
    pAsset67.prefab_id = "effects/prefabs/PrefabHmm";
    pAsset67.sorting_layer_id = "EffectsTop";
    pAsset67.limit = 10;
    this.add(pAsset67);
    EffectAsset pAsset68 = new EffectAsset();
    pAsset68.id = "fx_plot_progress";
    pAsset68.prefab_id = "effects/prefabs/PrefabPlotProgress";
    pAsset68.sorting_layer_id = "EffectsTop";
    pAsset68.limit = 10;
    this.add(pAsset68);
    EffectAsset pAsset69 = new EffectAsset();
    pAsset69.id = "fx_nuke_flash";
    pAsset69.prefab_id = "effects/prefabs/PrefabNukeFlash";
    pAsset69.show_on_mini_map = true;
    pAsset69.draw_light_area = true;
    pAsset69.draw_light_size = 3f;
    pAsset69.spawn_action = new EffectAction(this.spawnNukeFlash);
    this.add(pAsset69);
    EffectAsset pAsset70 = new EffectAsset();
    pAsset70.id = "fx_napalm_flash";
    pAsset70.show_on_mini_map = true;
    pAsset70.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionMiddle";
    pAsset70.prefab_id = "effects/prefabs/PrefabNapalmFlash";
    pAsset70.draw_light_area = true;
    pAsset70.draw_light_size = 2f;
    pAsset70.spawn_action = new EffectAction(this.spawnNapalmFlash);
    this.add(pAsset70);
    EffectAsset pAsset71 = new EffectAsset();
    pAsset71.id = "fx_thunder_flash";
    pAsset71.prefab_id = "effects/prefabs/PrefabThunderFlash";
    pAsset71.limit = 3;
    pAsset71.show_on_mini_map = true;
    pAsset71.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionLightningStrike";
    pAsset71.spawn_action = new EffectAction(this.spawnThunderFlash);
    this.add(pAsset71);
    EffectAsset pAsset72 = new EffectAsset();
    pAsset72.id = "fx_boulder_impact";
    pAsset72.prefab_id = "effects/prefabs/PrefabBoulderImpact";
    pAsset72.show_on_mini_map = true;
    pAsset72.sound_launch = "event:/SFX/DESTRUCTION/DropSimpleImpact";
    this.add(pAsset72);
    EffectAsset pAsset73 = new EffectAsset();
    pAsset73.id = "fx_boulder_impact_water";
    pAsset73.prefab_id = "effects/prefabs/PrefabBoulderImpactWater";
    pAsset73.show_on_mini_map = true;
    pAsset73.sound_launch = "event:/SFX/DESTRUCTION/DropSimpleImpact";
    this.add(pAsset73);
    EffectAsset pAsset74 = new EffectAsset();
    pAsset74.id = "fx_antimatter_effect";
    pAsset74.prefab_id = "effects/prefabs/PrefabAntimatterEffect";
    pAsset74.show_on_mini_map = true;
    pAsset74.sound_launch = "event:/SFX/EXPLOSIONS/ExplosionAntimatterBomb";
    pAsset74.spawn_action = new EffectAction(this.spawnSimpleTile);
    this.add(pAsset74);
    EffectAsset pAsset75 = new EffectAsset();
    pAsset75.id = "fx_infinity_coin";
    pAsset75.show_on_mini_map = true;
    pAsset75.prefab_id = "effects/prefabs/PrefabInfinityCoin";
    pAsset75.spawn_action = new EffectAction(this.spawnSimpleTile);
    pAsset75.draw_light_area = true;
    pAsset75.draw_light_size = 1f;
    pAsset75.sound_launch = "event:/SFX/DESTRUCTION/InfinityCoin";
    this.add(pAsset75);
    EffectAsset pAsset76 = new EffectAsset();
    pAsset76.id = "fx_status_particle";
    pAsset76.prefab_id = "effects/prefabs/PrefabStatusParticle";
    pAsset76.limit = 10;
    this.add(pAsset76);
    EffectAsset pAsset77 = new EffectAsset();
    pAsset77.id = "fx_weapon_particle";
    pAsset77.prefab_id = "effects/prefabs/PrefabStatusParticle";
    pAsset77.limit = 50;
    this.add(pAsset77);
    EffectAsset pAsset78 = new EffectAsset();
    pAsset78.id = "fx_slash";
    pAsset78.prefab_id = "effects/prefabs/PrefabSlash";
    pAsset78.limit = 40;
    this.add(pAsset78);
    EffectAsset pAsset79 = new EffectAsset();
    pAsset79.id = "fx_hit";
    pAsset79.prefab_id = "effects/prefabs/PrefabHit";
    pAsset79.limit = 20;
    this.add(pAsset79);
    EffectAsset pAsset80 = new EffectAsset();
    pAsset80.id = "fx_miss";
    pAsset80.prefab_id = "effects/prefabs/PrefabMiss";
    pAsset80.limit = 10;
    this.add(pAsset80);
    EffectAsset pAsset81 = new EffectAsset();
    pAsset81.id = "fx_jump";
    pAsset81.sorting_layer_id = "EffectsBack";
    pAsset81.load_texture = true;
    pAsset81.sprite_path = "effects/jump";
    pAsset81.use_basic_prefab = true;
    pAsset81.limit = 10;
    this.add(pAsset81);
    EffectAsset pAsset82 = new EffectAsset();
    pAsset82.id = "fx_walk";
    pAsset82.sorting_layer_id = "EffectsBack";
    pAsset82.load_texture = true;
    pAsset82.sprite_path = "effects/walk";
    pAsset82.limit = 15;
    pAsset82.use_basic_prefab = true;
    pAsset82.cooldown_interval = 0.15000000596046448;
    this.add(pAsset82);
    EffectAsset pAsset83 = new EffectAsset();
    pAsset83.id = "fx_hit_critical";
    pAsset83.prefab_id = "effects/prefabs/PrefabHitCritical";
    pAsset83.limit = 10;
    this.add(pAsset83);
    EffectAsset pAsset84 = new EffectAsset();
    pAsset84.id = "fx_boat_explosion";
    pAsset84.prefab_id = "effects/prefabs/PrefabBoatExplosion";
    pAsset84.draw_light_area = true;
    pAsset84.limit = 20;
    this.add(pAsset84);
    EffectAsset pAsset85 = new EffectAsset();
    pAsset85.id = "fx_fishnet";
    pAsset85.prefab_id = "effects/prefabs/PrefabFishnet";
    pAsset85.limit = 20;
    pAsset85.sound_launch = "event:/SFX/CIVILIZATIONS/SpawnFishnet";
    this.add(pAsset85);
    EffectAsset pAsset86 = new EffectAsset();
    pAsset86.id = "fx_tile_effect";
    pAsset86.prefab_id = "effects/prefabs/PrefabTileEffect";
    pAsset86.limit = 20;
    pAsset86.show_on_mini_map = false;
    pAsset86.spawn_action = new EffectAction(this.spawnSimpleTile);
    this.add(pAsset86);
    EffectAsset pAsset87 = new EffectAsset();
    pAsset87.id = "fx_cloud";
    pAsset87.prefab_id = "effects/prefabs/PrefabCloud";
    pAsset87.show_on_mini_map = true;
    pAsset87.limit = 200;
    pAsset87.limit_unload = true;
    pAsset87.spawn_action = new EffectAction(this.spawnCloud);
    this.add(pAsset87);
    EffectAsset pAsset88 = new EffectAsset();
    pAsset88.id = "fx_meteorite";
    pAsset88.prefab_id = "effects/prefabs/PrefabMeteorite";
    pAsset88.show_on_mini_map = true;
    pAsset88.spawn_action = new EffectAction(this.spawnMeteorite);
    pAsset88.sound_launch = "event:/SFX/DESTRUCTION/FallMeteorite";
    this.add(pAsset88);
    EffectAsset pAsset89 = new EffectAsset();
    pAsset89.id = "fx_boulder";
    pAsset89.prefab_id = "effects/prefabs/PrefabBoulder";
    pAsset89.show_on_mini_map = true;
    pAsset89.spawn_action = new EffectAction(this.spawnBoulder);
    this.add(pAsset89);
    EffectAsset pAsset90 = new EffectAsset();
    pAsset90.id = "fx_santa";
    pAsset90.prefab_id = "effects/prefabs/PrefabSanta";
    pAsset90.show_on_mini_map = true;
    pAsset90.spawn_action = new EffectAction(this.spawnSanta);
    pAsset90.sound_loop_idle = "event:/SFX/OTHER/RoboSanta/RoboSantaIdleLoop";
    pAsset90.limit = 100;
    this.add(pAsset90);
    EffectAsset pAsset91 = new EffectAsset();
    pAsset91.id = "fx_zone_highlight";
    pAsset91.prefab_id = "effects/prefabs/PrefabZoneFlash";
    pAsset91.show_on_mini_map = true;
    pAsset91.spawn_action = new EffectAction(this.spawnZoneFlash);
    this.add(pAsset91);
    EffectAsset pAsset92 = new EffectAsset();
    pAsset92.id = "fx_tornado";
    pAsset92.prefab_id = "effects/prefabs/PrefabTornado";
    pAsset92.show_on_mini_map = true;
    pAsset92.sound_loop_idle = "event:/SFX/NATURE/TornadoIdleLoop";
    this.add(pAsset92);
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    foreach (EffectAsset effectAsset in this.list)
    {
      if (effectAsset.use_basic_prefab || effectAsset.load_texture)
      {
        if (effectAsset.sorting_layer_id == null)
          BaseAssetLibrary.logAssetError("EffectsLibrary: sorting_layer_id is missing", effectAsset.id);
        if (effectAsset.sprite_path == null)
          BaseAssetLibrary.logAssetError("EffectsLibrary: sprite_path is missing", effectAsset.id);
      }
      if (!effectAsset.use_basic_prefab && effectAsset.prefab_id == null)
        BaseAssetLibrary.logAssetError("EffectsLibrary: prefab_id is missing", effectAsset.id);
    }
  }

  private static BaseEffect check(string pID)
  {
    EffectAsset effectAsset = AssetManager.effects_library.get(pID);
    if (effectAsset == null)
      return (BaseEffect) null;
    if (effectAsset.cooldown_interval > 0.0 && effectAsset.checkIsUnderCooldown())
      return (BaseEffect) null;
    return !effectAsset.show_on_mini_map && MapBox.isRenderMiniMap() ? (BaseEffect) null : World.world.stack_effects.get(pID).spawnNew();
  }

  public static BaseEffect spawnAtTileRandomScale(
    string pID,
    WorldTile pTile,
    float pScaleMin,
    float pScaleMax)
  {
    float pScale = Randy.randomFloat(pScaleMin, pScaleMax);
    return EffectsLibrary.spawnAtTile(pID, pTile, pScale);
  }

  public static void spawnDebugTile(WorldTile pTile, Color pColor)
  {
    if (pTile == null)
      return;
    BaseEffect baseEffect = EffectsLibrary.spawnAtTile("fx_debug_tile", pTile, 0.75f);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return;
    pColor.a = 0.7f;
    baseEffect.sprite_renderer.color = pColor;
  }

  public static BaseEffect spawnAtTile(string pID, WorldTile pTile, float pScale)
  {
    BaseEffect baseEffect = EffectsLibrary.spawn(pID, pTile);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return (BaseEffect) null;
    baseEffect.prepare(pTile, pScale);
    return baseEffect;
  }

  public static BaseEffect spawnAt(string pID, Vector2 pPos, float pScale)
  {
    BaseEffect baseEffect = EffectsLibrary.spawn(pID, pX: pPos.x, pY: pPos.y);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return (BaseEffect) null;
    baseEffect.prepare(pPos, pScale);
    return baseEffect;
  }

  public static BaseEffect spawnAt(string pID, Vector3 pPos, float pScale)
  {
    BaseEffect baseEffect = EffectsLibrary.spawn(pID, pX: pPos.x, pY: pPos.y);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return (BaseEffect) null;
    baseEffect.prepare(Vector2.op_Implicit(pPos), pScale);
    return baseEffect;
  }

  public static BaseEffect spawn(
    string pID,
    WorldTile pTile = null,
    string pParam1 = null,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    float pX = -1f,
    float pY = -1f,
    Actor pActor = null)
  {
    BaseEffect pEffect = EffectsLibrary.check(pID);
    if (Object.op_Equality((Object) pEffect, (Object) null))
      return (BaseEffect) null;
    EffectAsset effectAsset = AssetManager.effects_library.get(pID);
    if (effectAsset.spawn_action != null)
    {
      BaseEffect baseEffect = effectAsset.spawn_action(pEffect, pTile, pParam1, pParam2, pFloatParam1, pActor);
    }
    if (effectAsset.has_sound_launch)
    {
      float pX1 = pX;
      float pY1 = pY;
      if (pTile != null && (double) pX1 == -1.0 && (double) pY1 == -1.0)
      {
        pX1 = (float) pTile.x;
        pY1 = (float) pTile.y;
      }
      MusicBox.playSound(effectAsset.sound_launch, pX1, pY1);
    }
    if ((double) pX != -1.0 && (double) pY != -1.0)
      ((Component) pEffect).transform.position = new Vector3(pX, pY, 0.0f);
    if (effectAsset.has_sound_loop_idle)
      pEffect.fmod_instance = MusicBox.attachToObject(effectAsset.sound_loop_idle, ((Component) pEffect).gameObject, Object.op_Implicit((Object) pEffect));
    return pEffect;
  }

  public static void spawnExplosionWave(Vector3 pVec, float pRadius, float pSpeed = 1f)
  {
    BaseEffect baseEffect = EffectsLibrary.spawn("fx_explosion_wave");
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return;
    ((ExplosionFlash) baseEffect).start(pVec, pRadius, pSpeed);
  }

  public static bool canShowSlashEffect()
  {
    return !World.world.stack_effects.controller_slash_effects.isLimitReached();
  }

  public static void spawnSlash(Vector2 pVec, string pPathSprites, float pAngle, float pScaleMod = 0.1f)
  {
    BaseEffect baseEffect = EffectsLibrary.spawn("fx_slash");
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return;
    baseEffect.prepare(pVec, pScaleMod);
    ((Component) baseEffect).GetComponent<SpriteAnimation>().setFrames(SpriteTextureLoader.getSpriteList(pPathSprites));
    ((Component) baseEffect).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, pAngle));
  }

  public BaseEffect spawnMeteorite(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    ((Meteorite) pEffect).spawnOn(pTile, pParam1, pActor);
    return pEffect;
  }

  public BaseEffect spawnSanta(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    ((Santa) pEffect).spawnOn(pTile);
    return pEffect;
  }

  public BaseEffect spawnBoulder(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    ((Boulder) pEffect).spawnOn(World.world.getMousePos());
    return pEffect;
  }

  public BaseEffect spawnNapalmFlash(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    ((NapalmFlash) pEffect).spawnFlash(pTile);
    return pEffect;
  }

  public BaseEffect spawnNukeFlash(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    ((NukeFlash) pEffect).spawnFlash(pTile, pParam1);
    return pEffect;
  }

  public BaseEffect spawnThunderFlash(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    ((ThunderFlash) pEffect).spawnFlash();
    return pEffect;
  }

  public BaseEffect spawnSimpleTile(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1 = null,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    pEffect.spawnOnTile(pTile);
    return pEffect;
  }

  public BaseEffect spawnZoneFlash(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1 = null,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    pEffect.spawnOnTile(pTile);
    return pEffect;
  }

  public BaseEffect spawnCloud(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    ((Cloud) pEffect).spawn(pTile, pParam1);
    return pEffect;
  }

  public BaseEffect spawnFireworks(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    pEffect.spawnOnTile(pTile);
    return pEffect;
  }

  public BaseEffect showSpawnEffect(
    BaseEffect pEffect,
    WorldTile pTile,
    string pParam1,
    string pParam2 = null,
    float pFloatParam1 = 0.0f,
    Actor pActor = null)
  {
    pEffect.prepare(pTile, 0.2f);
    return pEffect;
  }

  public BaseEffect spawnStatusParticle(BaseEffect pEffect, Vector3 pPos) => pEffect;

  public static void highlightKingdomZones(Kingdom pKingdom, Color pColor, float pAlpha = 0.3f)
  {
    foreach (City city in pKingdom.getCities())
    {
      foreach (TileZone zone in city.zones)
        ((ZoneFlash) EffectsLibrary.spawn("fx_zone_highlight", zone.centerTile, pFloatParam1: pAlpha)).start(pColor, pAlpha);
    }
  }

  public static void showMoneyEffect(string pID, Vector2 pPosition, TileZone pZone, float pScale)
  {
    if (!pZone.visible_main_centered || !PlayerConfig.optionBoolEnabled("money_flow"))
      return;
    float num = pPosition.x + Randy.randomFloat(-0.3f, 0.3f);
    pPosition.x = num;
    EffectsLibrary.spawnAt(pID, pPosition, pScale);
  }

  public static void showMetaEventEffectConversion(string pID, Actor pActor)
  {
    if (!PlayerConfig.optionBoolEnabled("meta_conversions"))
      return;
    EffectsLibrary.showMetaEventEffect(pID, pActor.current_position, pActor.current_zone, pActor.actor_scale);
  }

  public static void showMetaEventEffect(string pID, Actor pActor)
  {
    EffectsLibrary.showMetaEventEffect(pID, pActor.current_position, pActor.current_zone, pActor.actor_scale);
  }

  public static void showMetaEventEffect(
    string pID,
    Vector2 pPosition,
    TileZone pZone,
    float pScale)
  {
    if (!pZone.visible_main_centered)
      return;
    float num = pPosition.x + Randy.randomFloat(-0.3f, 0.3f);
    pPosition.x = num;
    EffectsLibrary.spawnAt(pID, pPosition, pScale);
  }
}
