// Decompiled with JetBrains decompiler
// Type: QuantumSpriteLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using life.taxi;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class QuantumSpriteLibrary : AssetLibrary<QuantumSpriteAsset>
{
  public static QuantumSpriteAsset light_areas;
  private static readonly Sprite _sprite_pixel = SpriteTextureLoader.getSprite("effects/pixel_corner");
  private static readonly Sprite _sprite_attack_reload = SpriteTextureLoader.getSprite("ui/Icons/iconAttackReload");
  private static readonly Sprite _boat_sprite_small = SpriteTextureLoader.getSprite("civ/icons/minimap_boat_small");
  private static readonly Sprite _boat_sprite_big = SpriteTextureLoader.getSprite("civ/icons/minimap_boat_big");
  private static readonly Sprite[] _unexplored_sprites = SpriteTextureLoader.getSpriteList("effects/fx_unexplored");
  private static readonly Sprite[] _unit_selection_effect = SpriteTextureLoader.getSpriteList("effects/unit_selected_effect");
  private static readonly Sprite[] _unit_selection_effect_main = SpriteTextureLoader.getSpriteList("effects/unit_selected_effect_main");
  private static readonly Sprite[] _fire_sprites_1 = SpriteTextureLoader.getSpriteList("effects/fx_status_burning_t");
  private static readonly Sprite[] _fire_sprites_2 = SpriteTextureLoader.getSpriteList("effects/fx_status_burning_t_2");
  private static readonly Sprite[] _fire_sprites_3 = SpriteTextureLoader.getSpriteList("effects/fx_status_burning_t_3");
  private static readonly Sprite[][] _fire_sprites_sets = new Sprite[3][]
  {
    QuantumSpriteLibrary._fire_sprites_1,
    QuantumSpriteLibrary._fire_sprites_2,
    QuantumSpriteLibrary._fire_sprites_3
  };
  private static readonly Sprite _king_sprite_normal = SpriteTextureLoader.getSprite("civ/icons/minimap_king_normal");
  private static readonly Sprite _king_sprite_angry = SpriteTextureLoader.getSprite("civ/icons/minimap_king_angry");
  private static readonly Sprite _king_sprite_surprised = SpriteTextureLoader.getSprite("civ/icons/minimap_king_surprised");
  private static readonly Sprite _king_sprite_happy = SpriteTextureLoader.getSprite("civ/icons/minimap_king_happy");
  private static readonly Sprite _king_sprite_sad = SpriteTextureLoader.getSprite("civ/icons/minimap_king_sad");
  private static readonly Sprite _leader_sprite_normal = SpriteTextureLoader.getSprite("civ/icons/minimap_leader_normal");
  private static readonly Sprite _leader_sprite_angry = SpriteTextureLoader.getSprite("civ/icons/minimap_leader_angry");
  private static readonly Sprite _leader_sprite_surprised = SpriteTextureLoader.getSprite("civ/icons/minimap_leader_surprised");
  private static readonly Sprite _leader_sprite_happy = SpriteTextureLoader.getSprite("civ/icons/minimap_leader_happy");
  private static readonly Sprite _leader_sprite_sad = SpriteTextureLoader.getSprite("civ/icons/minimap_leader_sad");
  private static readonly Sprite _flag_sprite = SpriteTextureLoader.getSprite("civ/icons/minimap_flag");
  public static double last_order_timestamp;
  private static int[] _q_render_indexes_units = new int[8192 /*0x2000*/];
  private static int[] _q_render_indexes_shadows_units = new int[8192 /*0x2000*/];
  private static int[] _q_render_indexes_shadows_buildings = new int[8192 /*0x2000*/];
  private static int[] _q_render_indexes_sprites_fire = new int[4096 /*0x1000*/];
  private static int[] _q_render_indexes_unit_items = new int[8192 /*0x2000*/];
  private static readonly List<Vector3> _wars_pos_sword_main = new List<Vector3>();
  private static readonly List<Vector3> _wars_pos_shields_main = new List<Vector3>();
  private float _metas_fall_offset_timer;
  private MetaType _last_meta_type_metas;
  private const float STOCKPILE_ITEM_OFFSET = 0.4f;
  private const int STOCKPILE_MAX_STACKS = 7;
  private const int STOCKPILE_MAX_ROWS = 5;
  private const int STOCKPILE_MAX_COLUMNS = 7;
  private const float STOCKPILE_ROW_OFFSET = 0.58f;
  private const float STOCKPILE_COLUMN_OFFSET = 0.5f;
  private const float STOCKPILE_OFFSET_Z = 0.5f;
  private static Vector2[] _array_stockpile_slots;
  private const int MAX_SLOTS = 35;

  public override void init()
  {
    base.init();
    this.initDebugQuantumSpriteAssets();
    QuantumSpriteAsset pAsset1 = new QuantumSpriteAsset();
    pAsset1.id = "square_selection";
    pAsset1.id_prefab = "p_gameSprite";
    pAsset1.base_scale = 0.1f;
    pAsset1.arrow_animation = true;
    pAsset1.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawSquareSelection);
    pAsset1.render_gameplay = true;
    pAsset1.turn_off_renderer = true;
    pAsset1.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("MapOverlay"));
    pAsset1.add_camera_zoom_multiplier_min = 0;
    pAsset1.add_camera_zoom_multiplier_max = 100;
    pAsset1.color = new Color(1f, 1f, 1f, 0.95f);
    this.add(pAsset1);
    QuantumSpriteAsset pAsset2 = new QuantumSpriteAsset();
    pAsset2.id = "arrows_unit_cursor_destination";
    pAsset2.id_prefab = "p_mapArrow_stroke";
    pAsset2.base_scale = 0.1f;
    pAsset2.arrow_animation = true;
    pAsset2.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursor);
    pAsset2.render_gameplay = true;
    pAsset2.color = new Color(1f, 1f, 1f, 0.95f);
    this.add(pAsset2);
    QuantumSpriteAsset pAsset3 = new QuantumSpriteAsset();
    pAsset3.id = "arrows_unit_cursor_destination_selected";
    pAsset3.id_prefab = "p_mapArrow_stroke";
    pAsset3.base_scale = 0.1f;
    pAsset3.arrow_animation = true;
    pAsset3.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorSelected);
    pAsset3.render_gameplay = true;
    pAsset3.color = new Color(0.3f, 1f, 1f, 0.7f);
    this.add(pAsset3);
    QuantumSpriteAsset pAsset4 = new QuantumSpriteAsset();
    pAsset4.id = "debug_raycasts_controlled";
    pAsset4.id_prefab = "p_mapSprite";
    pAsset4.base_scale = 0.3f;
    pAsset4.render_gameplay = true;
    pAsset4.debug_option = DebugOption.ControlledUnitsAttackRaycast;
    pAsset4.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorSelectedRaycasts);
    pAsset4.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSharedMat(LibraryMaterials.instance.mat_minis));
    this.add(pAsset4);
    QuantumSpriteAsset pAsset5 = new QuantumSpriteAsset();
    pAsset5.id = "arrows_unit_cursor_lover";
    pAsset5.id_prefab = "p_mapArrow_stroke";
    pAsset5.base_scale = 0.1f;
    pAsset5.arrow_animation = true;
    pAsset5.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorLover);
    pAsset5.render_gameplay = true;
    pAsset5.color = new Color(1f, 0.4f, 0.77f, 0.95f);
    this.add(pAsset5);
    QuantumSpriteAsset pAsset6 = new QuantumSpriteAsset();
    pAsset6.id = "arrows_unit_cursor_family";
    pAsset6.id_prefab = "p_mapArrow_stroke";
    pAsset6.base_scale = 0.05f;
    pAsset6.arrow_animation = true;
    pAsset6.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorFamily);
    pAsset6.render_gameplay = true;
    pAsset6.color = new Color(1f, 1f, 0.28f, 0.7f);
    this.add(pAsset6);
    QuantumSpriteAsset pAsset7 = new QuantumSpriteAsset();
    pAsset7.id = "arrows_unit_cursor_house";
    pAsset7.id_prefab = "p_mapArrow_stroke";
    pAsset7.base_scale = 0.05f;
    pAsset7.arrow_animation = true;
    pAsset7.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorHouse);
    pAsset7.render_gameplay = true;
    pAsset7.color = new Color(0.2f, 0.72f, 0.0f, 0.95f);
    this.add(pAsset7);
    QuantumSpriteAsset pAsset8 = new QuantumSpriteAsset();
    pAsset8.id = "cursor_arrow_parents";
    pAsset8.id_prefab = "p_mapArrow_stroke";
    pAsset8.base_scale = 0.05f;
    pAsset8.arrow_animation = true;
    pAsset8.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorParents);
    pAsset8.render_gameplay = true;
    pAsset8.color = new Color(0.5f, 0.83f, 1f, 0.95f);
    this.add(pAsset8);
    QuantumSpriteAsset pAsset9 = new QuantumSpriteAsset();
    pAsset9.id = "cursor_arrow_kids";
    pAsset9.id_prefab = "p_mapArrow_stroke";
    pAsset9.base_scale = 0.05f;
    pAsset9.arrow_animation = true;
    pAsset9.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorKids);
    pAsset9.render_gameplay = true;
    pAsset9.color = new Color(0.63f, 0.16f, 0.92f, 0.95f);
    this.add(pAsset9);
    QuantumSpriteAsset pAsset10 = new QuantumSpriteAsset();
    pAsset10.id = "cursor_arrow_attack_target";
    pAsset10.id_prefab = "p_mapArrow_stroke";
    pAsset10.base_scale = 0.05f;
    pAsset10.arrow_animation = true;
    pAsset10.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsUnitCursorAttackTarget);
    pAsset10.render_gameplay = true;
    pAsset10.color = new Color(1f, 0.0f, 0.0f, 0.95f);
    this.add(pAsset10);
    QuantumSpriteAsset pAsset11 = new QuantumSpriteAsset();
    pAsset11.id = "draw_walls";
    pAsset11.id_prefab = "p_mapSprite";
    pAsset11.add_camera_zoom_multiplier = false;
    pAsset11.turn_off_renderer = true;
    pAsset11.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawWalls);
    pAsset11.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_world_object);
    });
    pAsset11.render_gameplay = true;
    pAsset11.default_amount = 500;
    this.add(pAsset11);
    QuantumSpriteAsset pAsset12 = new QuantumSpriteAsset();
    pAsset12.id = "draw_light_walls_light_blobs";
    pAsset12.id_prefab = "p_mapSprite";
    pAsset12.add_camera_zoom_multiplier = false;
    pAsset12.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawWallLightBlobs);
    pAsset12.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_world_object);
    });
    pAsset12.render_gameplay = true;
    pAsset12.render_map = true;
    this.add(pAsset12);
    QuantumSpriteAsset pAsset13 = new QuantumSpriteAsset();
    pAsset13.id = "draw_lava_light_blobs";
    pAsset13.id_prefab = "p_mapSprite";
    pAsset13.add_camera_zoom_multiplier = false;
    pAsset13.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawLavaLightBlobs);
    pAsset13.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_world_object);
    });
    pAsset13.render_gameplay = true;
    pAsset13.render_map = true;
    this.add(pAsset13);
    QuantumSpriteAsset pAsset14 = new QuantumSpriteAsset();
    pAsset14.id = "draw_units";
    pAsset14.id_prefab = "p_mapSprite";
    pAsset14.add_camera_zoom_multiplier = false;
    pAsset14.turn_off_renderer = true;
    pAsset14.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnits);
    pAsset14.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_world_object);
    });
    pAsset14.render_gameplay = true;
    pAsset14.default_amount = 1000;
    this.add(pAsset14);
    QuantumSpriteAsset pAsset15 = new QuantumSpriteAsset();
    pAsset15.id = "draw_healthbars";
    pAsset15.id_prefab = "p_mapSprite";
    pAsset15.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawHealthbars);
    pAsset15.turn_off_renderer = true;
    pAsset15.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("MapOverlay"));
    pAsset15.render_gameplay = true;
    pAsset15.add_camera_zoom_multiplier_min = 0;
    pAsset15.add_camera_zoom_multiplier_max = 100;
    pAsset15.default_amount = 100;
    this.add(pAsset15);
    QuantumSpriteAsset pAsset16 = new QuantumSpriteAsset();
    pAsset16.id = "draw_units_avatars";
    pAsset16.id_prefab = "p_mapSprite";
    pAsset16.add_camera_zoom_multiplier = false;
    pAsset16.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitsAvatars);
    pAsset16.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_world_object);
    });
    pAsset16.render_gameplay = true;
    pAsset16.render_map = true;
    this.add(pAsset16);
    QuantumSpriteAsset pAsset17 = new QuantumSpriteAsset();
    pAsset17.id = "unit_items";
    pAsset17.id_prefab = "p_mapSprite";
    pAsset17.base_scale = 0.15f;
    pAsset17.add_camera_zoom_multiplier = false;
    pAsset17.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitItems);
    pAsset17.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_world_object);
    });
    pAsset17.render_gameplay = true;
    pAsset17.default_amount = 200;
    this.add(pAsset17);
    QuantumSpriteAsset pAsset18 = new QuantumSpriteAsset();
    pAsset18.id = "draw_unit_hit_effect";
    pAsset18.id_prefab = "p_mapSprite";
    pAsset18.add_camera_zoom_multiplier = false;
    pAsset18.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitsEffectDamage);
    pAsset18.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_damaged);
    });
    pAsset18.render_gameplay = true;
    pAsset18.render_map = true;
    this.add(pAsset18);
    QuantumSpriteAsset pAsset19 = new QuantumSpriteAsset();
    pAsset19.id = "draw_parabolic_unload";
    pAsset19.id_prefab = "p_mapSprite";
    pAsset19.add_camera_zoom_multiplier = false;
    pAsset19.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawParabolicUnload);
    pAsset19.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects"));
    pAsset19.render_gameplay = true;
    pAsset19.render_map = true;
    this.add(pAsset19);
    QuantumSpriteAsset pAsset20 = new QuantumSpriteAsset();
    pAsset20.id = "draw_unit_highlight_effect";
    pAsset20.id_prefab = "p_mapSprite";
    pAsset20.add_camera_zoom_multiplier = false;
    pAsset20.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitsEffectHighlight);
    pAsset20.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_highlighted);
    });
    pAsset20.render_gameplay = true;
    pAsset20.render_map = true;
    this.add(pAsset20);
    QuantumSpriteAsset pAsset21 = new QuantumSpriteAsset();
    pAsset21.id = "draw_buildings";
    pAsset21.id_prefab = "p_mapSprite";
    pAsset21.add_camera_zoom_multiplier = false;
    pAsset21.turn_off_renderer = true;
    pAsset21.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawBuildings);
    pAsset21.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects"));
    pAsset21.render_gameplay = true;
    pAsset21.default_amount = 2000;
    this.add(pAsset21);
    QuantumSpriteAsset pAsset22 = new QuantumSpriteAsset();
    pAsset22.id = "draw_building_stockpiles";
    pAsset22.id_prefab = "p_mapSprite";
    pAsset22.add_camera_zoom_multiplier = false;
    pAsset22.turn_off_renderer = true;
    pAsset22.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawStockpileResources);
    pAsset22.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects"));
    pAsset22.render_gameplay = true;
    pAsset22.default_amount = 100;
    this.add(pAsset22);
    QuantumSpriteAsset pAsset23 = new QuantumSpriteAsset();
    pAsset23.id = "projectiles";
    pAsset23.id_prefab = "p_mapSprite";
    pAsset23.render_gameplay = true;
    pAsset23.turn_off_renderer = true;
    pAsset23.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects"));
    pAsset23.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawProjectiles);
    pAsset23.default_amount = 100;
    this.add(pAsset23);
    QuantumSpriteAsset pAsset24 = new QuantumSpriteAsset();
    pAsset24.id = "projectile_shadows";
    pAsset24.id_prefab = "p_shadow";
    pAsset24.turn_off_renderer = true;
    pAsset24.render_gameplay = true;
    pAsset24.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawProjectileShadows);
    pAsset24.default_amount = 100;
    this.add(pAsset24);
    QuantumSpriteAsset pAsset25 = new QuantumSpriteAsset();
    pAsset25.id = "throwing_items_shadows";
    pAsset25.id_prefab = "p_shadow";
    pAsset25.turn_off_renderer = true;
    pAsset25.render_gameplay = true;
    pAsset25.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawThrowingItemsShadows);
    pAsset25.default_amount = 100;
    this.add(pAsset25);
    QuantumSpriteAsset pAsset26 = new QuantumSpriteAsset();
    pAsset26.id = "shadows_buildings";
    pAsset26.id_prefab = "p_shadow";
    pAsset26.turn_off_renderer = true;
    pAsset26.render_gameplay = true;
    pAsset26.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawShadowsBuildings);
    pAsset26.default_amount = 500;
    this.add(pAsset26);
    QuantumSpriteAsset pAsset27 = new QuantumSpriteAsset();
    pAsset27.id = "shadows_unit";
    pAsset27.id_prefab = "p_shadow";
    pAsset27.turn_off_renderer = true;
    pAsset27.render_gameplay = true;
    pAsset27.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawShadowsUnit);
    this.add(pAsset27);
    QuantumSpriteAsset pAsset28 = new QuantumSpriteAsset();
    pAsset28.id = "unit_banners";
    pAsset28.id_prefab = "p_unitBanner";
    pAsset28.turn_off_renderer = true;
    pAsset28.render_gameplay = true;
    pAsset28.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitBanners);
    this.add(pAsset28);
    QuantumSpriteAsset pAsset29 = new QuantumSpriteAsset();
    pAsset29.id = "selected_units";
    pAsset29.id_prefab = "p_gameSprite";
    pAsset29.render_gameplay = true;
    pAsset29.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack"));
    pAsset29.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawSelectedUnits);
    this.add(pAsset29);
    QuantumSpriteAsset pAsset30 = new QuantumSpriteAsset();
    pAsset30.id = "square_selection_to_select";
    pAsset30.id_prefab = "p_gameSprite";
    pAsset30.render_gameplay = true;
    pAsset30.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack"));
    pAsset30.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitsToBeSelectedBySquareTool);
    this.add(pAsset30);
    QuantumSpriteAsset pAsset31 = new QuantumSpriteAsset();
    pAsset31.id = "favorites_game";
    pAsset31.id_prefab = "p_gameSprite";
    pAsset31.render_gameplay = true;
    pAsset31.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      pQSprite.setSprite(SpriteTextureLoader.getSprite("ui/Icons/iconFavoriteStar"));
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 1;
    });
    pAsset31.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawFavoritesGame);
    this.add(pAsset31);
    QuantumSpriteAsset pAsset32 = new QuantumSpriteAsset();
    pAsset32.id = "favorites_items";
    pAsset32.id_prefab = "p_gameSprite";
    pAsset32.base_scale = 0.3f;
    pAsset32.render_map = true;
    pAsset32.selected_city_scale = true;
    pAsset32.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSprite(SpriteTextureLoader.getSprite("ui/Icons/iconFavoriteWeapon")));
    pAsset32.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawFavoriteItemsMap);
    this.add(pAsset32);
    QuantumSpriteAsset pAsset33 = new QuantumSpriteAsset();
    pAsset33.id = "unit_metas";
    pAsset33.id_prefab = "p_gameSprite";
    pAsset33.turn_off_renderer = true;
    pAsset33.base_scale = 0.3f;
    pAsset33.render_map = false;
    pAsset33.render_gameplay = true;
    pAsset33.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      pQSprite.setSprite(SpriteTextureLoader.getSprite("effects/unit_meta"));
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
    });
    pAsset33.draw_call = new QuantumSpriteUpdater(this.drawUnitMetas);
    this.add(pAsset33);
    QuantumSpriteAsset pAsset34 = new QuantumSpriteAsset();
    pAsset34.id = "happiness_icons";
    pAsset34.id_prefab = "p_gameSprite";
    pAsset34.turn_off_renderer = true;
    pAsset34.base_scale = 0.03f;
    pAsset34.render_gameplay = true;
    pAsset34.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects"));
    pAsset34.draw_call = new QuantumSpriteUpdater(this.drawUnitHappinessIcons);
    this.add(pAsset34);
    QuantumSpriteAsset pAsset35 = new QuantumSpriteAsset();
    pAsset35.id = "task_icons";
    pAsset35.id_prefab = "p_gameSprite";
    pAsset35.turn_off_renderer = true;
    pAsset35.base_scale = 0.04f;
    pAsset35.render_gameplay = true;
    pAsset35.create_object = (QuantumSpriteCreate) ((_, pQSprite) => ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects"));
    pAsset35.draw_call = new QuantumSpriteUpdater(this.drawUnitTaskIcons);
    this.add(pAsset35);
    QuantumSpriteAsset pAsset36 = new QuantumSpriteAsset();
    pAsset36.id = "family_species_icons";
    pAsset36.id_prefab = "p_mapSprite";
    pAsset36.base_scale = 0.3f;
    pAsset36.add_camera_zoom_multiplier = false;
    pAsset36.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawFamilySpeciesIcons);
    pAsset36.color = new Color(1f, 1f, 1f, 0.8f);
    pAsset36.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) =>
    {
      pQSprite.setColor(ref pAsset.color);
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 1;
    });
    pAsset36.render_map = true;
    this.add(pAsset36);
    QuantumSpriteAsset pAsset37 = new QuantumSpriteAsset();
    pAsset37.id = "favorites_map";
    pAsset37.id_prefab = "p_gameSprite";
    pAsset37.base_scale = 0.3f;
    pAsset37.render_map = true;
    pAsset37.selected_city_scale = true;
    pAsset37.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSprite(SpriteTextureLoader.getSprite("ui/Icons/iconFavoriteStar_Map")));
    pAsset37.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawFavoritesMap);
    this.add(pAsset37);
    QuantumSpriteAsset pAsset38 = new QuantumSpriteAsset();
    pAsset38.id = "status_effects";
    pAsset38.id_prefab = "p_gameSprite";
    pAsset38.render_gameplay = true;
    pAsset38.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawStatusEffects);
    pAsset38.default_amount = 10;
    this.add(pAsset38);
    QuantumSpriteAsset pAsset39 = new QuantumSpriteAsset();
    pAsset39.id = "wars_lines";
    pAsset39.id_prefab = "p_mapArrow_arrows";
    pAsset39.line_width = 5;
    pAsset39.line_height = 7;
    pAsset39.arrow_animation = true;
    pAsset39.render_map = true;
    pAsset39.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawWars);
    this.add(pAsset39);
    QuantumSpriteAsset pAsset40 = new QuantumSpriteAsset();
    pAsset40.id = "wars_icons";
    pAsset40.id_prefab = "p_mapSprite";
    pAsset40.render_map = true;
    pAsset40.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawWarsIcons);
    this.add(pAsset40);
    QuantumSpriteAsset pAsset41 = new QuantumSpriteAsset();
    pAsset41.id = "plots";
    pAsset41.id_prefab = "p_plot";
    pAsset41.base_scale = 0.3f;
    pAsset41.render_map = true;
    pAsset41.render_gameplay = true;
    pAsset41.selected_city_scale = true;
    pAsset41.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawPlots);
    pAsset41.default_amount = 10;
    this.add(pAsset41);
    QuantumSpriteAsset pAsset42 = new QuantumSpriteAsset();
    pAsset42.id = "plot_removals";
    pAsset42.id_prefab = "p_plot";
    pAsset42.base_scale = 0.3f;
    pAsset42.render_map = true;
    pAsset42.render_gameplay = true;
    pAsset42.selected_city_scale = true;
    pAsset42.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawPlotRemovals);
    pAsset42.default_amount = 10;
    this.add(pAsset42);
    QuantumSpriteAsset pAsset43 = new QuantumSpriteAsset();
    pAsset43.id = "kings";
    pAsset43.id_prefab = "p_mapSprite";
    pAsset43.base_scale = 0.3f;
    pAsset43.render_map = true;
    pAsset43.selected_city_scale = true;
    pAsset43.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawKings);
    pAsset43.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSharedMat(LibraryMaterials.instance.mat_minis));
    pAsset43.default_amount = 10;
    this.add(pAsset43);
    QuantumSpriteAsset pAsset44 = new QuantumSpriteAsset();
    pAsset44.id = "leaders";
    pAsset44.id_prefab = "p_mapSprite";
    pAsset44.render_map = true;
    pAsset44.selected_city_scale = true;
    pAsset44.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawLeaders);
    pAsset44.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSharedMat(LibraryMaterials.instance.mat_minis));
    pAsset44.default_amount = 10;
    this.add(pAsset44);
    QuantumSpriteAsset pAsset45 = new QuantumSpriteAsset();
    pAsset45.id = "armies";
    pAsset45.id_prefab = "p_mapArmy";
    pAsset45.base_scale = 0.3f;
    pAsset45.render_map = true;
    pAsset45.selected_city_scale = true;
    pAsset45.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((QuantumSpriteWithText) pQSprite).initText();
      pQSprite.setSharedMat(LibraryMaterials.instance.mat_minis);
    });
    pAsset45.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArmies);
    pAsset45.default_amount = 10;
    this.add(pAsset45);
    QuantumSpriteAsset pAsset46 = new QuantumSpriteAsset();
    pAsset46.id = "magnet_units";
    pAsset46.id_prefab = "p_mapSprite";
    pAsset46.render_map = true;
    pAsset46.render_gameplay = true;
    pAsset46.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawMagnetUnits);
    pAsset46.default_amount = 10;
    this.add(pAsset46);
    QuantumSpriteAsset pAsset47 = new QuantumSpriteAsset();
    pAsset47.id = "boats_big";
    pAsset47.id_prefab = "p_mapSprite";
    pAsset47.base_scale = 0.3f;
    pAsset47.render_map = true;
    pAsset47.selected_city_scale = true;
    pAsset47.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawBoatIcons);
    pAsset47.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSharedMat(LibraryMaterials.instance.mat_minis));
    pAsset47.default_amount = 10;
    this.add(pAsset47);
    QuantumSpriteAsset pAsset48 = new QuantumSpriteAsset();
    pAsset48.id = "boats_small";
    pAsset48.id_prefab = "p_mapSprite";
    pAsset48.render_map = true;
    pAsset48.selected_city_scale = true;
    pAsset48.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawBoatIcons);
    pAsset48.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSharedMat(LibraryMaterials.instance.mat_minis));
    pAsset48.default_amount = 10;
    this.add(pAsset48);
    QuantumSpriteAsset pAsset49 = new QuantumSpriteAsset();
    pAsset49.id = "battles";
    pAsset49.id_prefab = "p_mapBattle";
    pAsset49.base_scale = 0.6f;
    pAsset49.flag_battle = true;
    pAsset49.path_icon = "civ/map_mark_battle_animation";
    pAsset49.render_map = true;
    pAsset49.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawBattles);
    pAsset49.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSharedMat(LibraryMaterials.instance.mat_minis));
    pAsset49.default_amount = 10;
    this.add(pAsset49);
    QuantumSpriteAsset pAsset50 = new QuantumSpriteAsset();
    pAsset50.id = "arrows_army_targets";
    pAsset50.id_prefab = "p_mapArrow_stroke";
    pAsset50.render_map = true;
    pAsset50.arrow_animation = true;
    pAsset50.base_scale = 0.3f;
    pAsset50.selected_city_scale = true;
    pAsset50.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawArrowsArmyAttackTargets);
    this.add(pAsset50);
    QuantumSpriteAsset pAsset51 = new QuantumSpriteAsset();
    pAsset51.id = "highlight_cursor_zones";
    pAsset51.id_prefab = "p_mapZone";
    pAsset51.base_scale = 1f;
    pAsset51.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawCursorZones);
    pAsset51.render_map = true;
    pAsset51.add_camera_zoom_multiplier = false;
    pAsset51.color = new Color(1f, 1f, 1f, 0.2f);
    pAsset51.color_2 = new Color(1f, 0.1f, 0.1f, 0.2f);
    this.add(pAsset51);
    QuantumSpriteAsset pAsset52 = new QuantumSpriteAsset();
    pAsset52.id = "selected_kingdom";
    pAsset52.id_prefab = "p_mapZone";
    pAsset52.base_scale = 1f;
    pAsset52.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawSelectedKingdomZones);
    pAsset52.render_map = true;
    pAsset52.add_camera_zoom_multiplier = false;
    pAsset52.color = new Color(1f, 1f, 1f, 0.4f);
    pAsset52.color_2 = new Color(1f, 0.1f, 0.1f, 0.2f);
    this.add(pAsset52);
    QuantumSpriteAsset pAsset53 = new QuantumSpriteAsset();
    pAsset53.id = "whisper_of_war";
    pAsset53.id_prefab = "p_mapZone";
    pAsset53.base_scale = 1f;
    pAsset53.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawWhisperOfWar);
    pAsset53.render_map = true;
    pAsset53.add_camera_zoom_multiplier = false;
    pAsset53.color = new Color(1f, 1f, 1f, 0.4f);
    pAsset53.color_2 = new Color(1f, 0.1f, 0.1f, 0.2f);
    this.add(pAsset53);
    QuantumSpriteAsset pAsset54 = new QuantumSpriteAsset();
    pAsset54.id = "whisper_of_war_line";
    pAsset54.id_prefab = "p_mapArrow_line";
    pAsset54.base_scale = 0.5f;
    pAsset54.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawWhisperOfWarLine);
    pAsset54.render_map = true;
    pAsset54.render_gameplay = true;
    pAsset54.color = new Color(0.4f, 0.4f, 1f, 0.9f);
    this.add(pAsset54);
    QuantumSpriteAsset pAsset55 = new QuantumSpriteAsset();
    pAsset55.id = "unity_line";
    pAsset55.id_prefab = "p_mapArrow_line";
    pAsset55.base_scale = 0.5f;
    pAsset55.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnityLine);
    pAsset55.render_map = true;
    pAsset55.render_gameplay = true;
    pAsset55.color = new Color(0.4f, 0.4f, 1f, 0.9f);
    this.add(pAsset55);
    QuantumSpriteAsset pAsset56 = new QuantumSpriteAsset();
    pAsset56.id = "capturing_zones";
    pAsset56.id_prefab = "p_mapZone_lines";
    pAsset56.base_scale = 1f;
    pAsset56.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawCapturingZones);
    pAsset56.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 0;
    });
    pAsset56.render_map = true;
    pAsset56.add_camera_zoom_multiplier = false;
    this.add(pAsset56);
    QuantumSpriteAsset pAsset57 = new QuantumSpriteAsset();
    pAsset57.id = "ate_item";
    pAsset57.id_prefab = "p_mapSprite";
    pAsset57.base_scale = 0.15f;
    pAsset57.add_camera_zoom_multiplier = false;
    pAsset57.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawJustAte);
    pAsset57.render_gameplay = true;
    this.add(pAsset57);
    QuantumSpriteAsset pAsset58 = new QuantumSpriteAsset();
    pAsset58.id = "socialize";
    pAsset58.id_prefab = "p_mapSprite";
    pAsset58.base_scale = 0.15f;
    pAsset58.add_camera_zoom_multiplier = false;
    pAsset58.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawSocialize);
    pAsset58.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSharedMat(LibraryMaterials.instance.mat_socialize));
    pAsset58.render_gameplay = true;
    this.add(pAsset58);
    QuantumSpriteAsset pAsset59 = new QuantumSpriteAsset();
    pAsset59.id = "cursor_power";
    pAsset59.id_prefab = "p_mapSprite";
    pAsset59.base_scale = 0.1f;
    pAsset59.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawCursorSprite);
    pAsset59.add_camera_zoom_multiplier_min = 0;
    pAsset59.add_camera_zoom_multiplier_max = 100;
    pAsset59.render_gameplay = true;
    pAsset59.render_map = true;
    this.add(pAsset59);
    QuantumSpriteAsset pAsset60 = new QuantumSpriteAsset();
    pAsset60.id = "controlled_unit_attack_recharge";
    pAsset60.id_prefab = "p_attack_recharge";
    pAsset60.base_scale = 0.03f;
    pAsset60.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawCursorAttackRecharge);
    pAsset60.add_camera_zoom_multiplier_min = 0;
    pAsset60.add_camera_zoom_multiplier_max = 100;
    pAsset60.render_gameplay = true;
    pAsset60.render_map = true;
    this.add(pAsset60);
    QuantumSpriteAsset pAsset61 = new QuantumSpriteAsset();
    pAsset61.id = "cursor_target_subspecies";
    pAsset61.id_prefab = "p_mapArrow_dna";
    pAsset61.base_scale = 0.1f;
    pAsset61.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawCursorTargetSubspecies);
    pAsset61.arrow_animation = false;
    pAsset61.add_camera_zoom_multiplier_min = 0;
    pAsset61.add_camera_zoom_multiplier_max = 100;
    pAsset61.line_height = 6;
    pAsset61.line_width = 45;
    pAsset61.render_gameplay = true;
    pAsset61.render_map = true;
    this.add(pAsset61);
    QuantumSpriteAsset pAsset62 = new QuantumSpriteAsset();
    pAsset62.id = "buildings_light_windows";
    pAsset62.id_prefab = "p_windowLight";
    pAsset62.add_camera_zoom_multiplier = false;
    pAsset62.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawBuildingsLightWindows);
    pAsset62.render_gameplay = true;
    pAsset62.render_map = true;
    this.add(pAsset62);
    QuantumSpriteAsset pAsset63 = new QuantumSpriteAsset();
    pAsset63.id = "light_areas";
    pAsset63.id_prefab = "p_lightArea";
    pAsset63.add_camera_zoom_multiplier = false;
    pAsset63.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawLightAreas);
    pAsset63.render_gameplay = true;
    pAsset63.render_map = true;
    QuantumSpriteLibrary.light_areas = this.add(pAsset63);
    QuantumSpriteAsset pAsset64 = new QuantumSpriteAsset();
    pAsset64.id = "fire_sprites";
    pAsset64.id_prefab = "p_mapSprite";
    pAsset64.base_scale = 0.15f;
    pAsset64.add_camera_zoom_multiplier = false;
    pAsset64.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawFires);
    pAsset64.sound_idle = "event:/SFX/STATUS/StatusBurningBuilding";
    pAsset64.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      pQSprite.setScale(pAsset.base_scale);
      pQSprite.sprite_renderer.sprite = QuantumSpriteLibrary._fire_sprites_1.GetRandom<Sprite>();
    });
    pAsset64.render_gameplay = true;
    this.add(pAsset64);
    QuantumSpriteAsset pAsset65 = new QuantumSpriteAsset();
    pAsset65.id = "unexplored_augmentations";
    pAsset65.id_prefab = "p_gameSprite";
    pAsset65.base_scale = 0.1f;
    pAsset65.add_camera_zoom_multiplier = false;
    pAsset65.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("Objects");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 1;
    });
    pAsset65.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnexploredAugmentationSprite);
    pAsset65.color = new Color(1f, 1f, 1f, 0.8f);
    pAsset65.render_gameplay = true;
    this.add(pAsset65);
  }

  private void drawUnitHappinessIcons(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("icons_happiness"))
      return;
    float num = 18f;
    if (PlayerConfig.optionBoolEnabled("icons_tasks"))
      num += 11f;
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.hasEmotions() && !actor.isInsideSomething())
      {
        float pForceScaleTo = actor.current_scale.y * 0.5f;
        Vector3 pPos = Vector2.op_Implicit(actor.current_position);
        pPos.z = num;
        pPos.y += num * actor.current_scale.y;
        Sprite onHappinessValue = HappinessHelper.getSpriteBasedOnHappinessValue(actor.getHappiness());
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pForceScaleTo: pForceScaleTo).setSprite(onHappinessValue);
      }
    }
  }

  private void drawUnitTaskIcons(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("icons_tasks"))
      return;
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    float num = 17.5f;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (!actor.isInsideSomething() && actor.asset.show_task_icon && actor.ai != null)
      {
        BehaviourTaskActor task = actor.ai.task;
        if (task != null && task.show_icon)
        {
          float pForceScaleTo = actor.current_scale.y * 0.5f;
          Vector3 pPos = Vector2.op_Implicit(actor.current_position);
          pPos.z = num;
          pPos.y += num * actor.current_scale.y;
          QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pForceScaleTo: pForceScaleTo).setSprite(task.getSprite());
        }
      }
    }
  }

  private void drawUnitMetas(QuantumSpriteAsset pAsset)
  {
    bool flag1 = PlayerConfig.optionBoolEnabled("unit_metas");
    bool flag2 = SelectedObjects.isNanoObjectSet();
    if (flag2)
      flag1 = true;
    if (!flag1)
    {
      this._last_meta_type_metas = MetaType.None;
    }
    else
    {
      this._metas_fall_offset_timer += World.world.delta_time * 1f;
      if ((double) this._metas_fall_offset_timer > 1.0)
        this._metas_fall_offset_timer = 1f;
      Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
      int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
      MetaType pType = Zones.getCurrentMapBorderMode();
      if (pType.isNone())
        return;
      bool flag3 = PlayerConfig.optionBoolEnabled("only_favorited_meta");
      NanoObject selectedNanoObject = SelectedObjects.getSelectedNanoObject();
      if (flag2)
        pType = selectedNanoObject.getMetaType();
      if (this._last_meta_type_metas != pType)
        this._metas_fall_offset_timer = 0.0f;
      this._last_meta_type_metas = pType;
      float num = (float) ((1.0 - (double) iTween.easeOutBounce(0.0f, 1f, this._metas_fall_offset_timer)) * 5.0);
      for (int index = 0; index < visibleUnitsAliveCount; ++index)
      {
        Actor actor = visibleUnitsAlive[index];
        if (actor.getMetaObjectOfType(pType) is IMetaObject metaObjectOfType && (!flag2 || selectedNanoObject == metaObjectOfType) && (!flag3 || metaObjectOfType.isFavorite()))
        {
          ColorAsset color = metaObjectOfType.getColor();
          if (color == null)
          {
            Debug.LogError((object) ("[drawUnitMetas] Forgot to set color asset for ? " + pType.ToString()));
          }
          else
          {
            ref Color local = ref color.getColorTextRef();
            QuantumSprite next = pAsset.group_system.getNext();
            Vector3 currentScale = actor.current_scale;
            Vector3 pPosition = Vector2.op_Implicit(actor.current_position);
            pPosition.y += num;
            pPosition.z = -0.02f;
            next.setPosOnly(ref pPosition);
            next.setScale(ref currentScale);
            next.setColor(ref local);
          }
        }
      }
    }
  }

  private static void showLightAt(Vector2 pPos, Color pColor, float pScale = 1f)
  {
    QuantumSprite next = QuantumSpriteLibrary.light_areas.group_system.getNext();
    next.set(ref pPos, pScale);
    next.setColor(ref QuantumSpriteLibrary.light_areas.color);
  }

  private static Color getColorForLight()
  {
    Color white = Color.white;
    if (MapBox.isRenderMiniMap())
    {
      white.a = World.world_era.era_effect_light_alpha_minimap;
      if (!World.world.zone_calculator.isModeNone())
        white.a = 0.4f;
    }
    else
      white.a = World.world_era.era_effect_light_alpha_game;
    return white;
  }

  private static void drawLightAreas(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("night_lights") || !World.world.era_manager.shouldShowLights())
      return;
    Color white = Color.white;
    Color colorForLight = QuantumSpriteLibrary.getColorForLight();
    if (World.world.heat_ray_fx.isReady())
      QuantumSpriteLibrary.showLightAt(World.world.heat_ray_fx.getPosForLight(), white, 1.5f);
    List<LightBlobData> lightBlobs = World.world.stack_effects.light_blobs;
    if (lightBlobs.Count > 0)
    {
      for (int index = 0; index < lightBlobs.Count; ++index)
      {
        LightBlobData lightBlobData = lightBlobs[index];
        QuantumSpriteLibrary.showLightAt(lightBlobData.position, white, lightBlobData.radius);
      }
    }
    if (MapBox.isRenderGameplay())
    {
      Actor[] visibleUnits = QuantumSpriteLibrary.visible_units;
      int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
      for (int index = 0; index < visibleUnitsCount; ++index)
        QuantumSpriteLibrary.checkUnitLight(visibleUnits[index], colorForLight);
    }
    else
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
        QuantumSpriteLibrary.checkUnitLight(simpleList[index], colorForLight);
    }
    if (World.world.quality_changer.shouldRenderBuildings())
    {
      int num = World.world.buildings.countVisibleBuildings();
      Building[] visibleBuildings = World.world.buildings.getVisibleBuildings();
      for (int index = 0; index < num; ++index)
        QuantumSpriteLibrary.checkBuildingLights(visibleBuildings[index], colorForLight);
    }
    else
    {
      List<Building> simpleList = World.world.buildings.getSimpleList();
      for (int index = 0; index < simpleList.Count; ++index)
        QuantumSpriteLibrary.checkBuildingLights(simpleList[index], colorForLight);
    }
    if (MapBox.isRenderGameplay())
    {
      if (WorldBehaviourActionFire.hasFires())
      {
        List<TileZone> visibleZones = World.world.zone_camera.getVisibleZones();
        for (int index1 = 0; index1 < visibleZones.Count; ++index1)
        {
          TileZone pZone = visibleZones[index1];
          if (WorldBehaviourActionFire.hasFires(pZone))
          {
            WorldTile[] tiles = pZone.tiles;
            int length = tiles.Length;
            for (int index2 = 0; index2 < length; ++index2)
            {
              WorldTile worldTile = tiles[index2];
              if (worldTile.isOnFire())
                QuantumSpriteLibrary.showLightAt(Vector2Int.op_Implicit(worldTile.pos), colorForLight, 0.2f);
            }
          }
        }
      }
    }
    else if (WorldBehaviourActionFire.hasFires())
    {
      foreach (TileZone zone in World.world.zone_calculator.zones)
      {
        if (WorldBehaviourActionFire.hasFires(zone))
        {
          WorldTile[] tiles = zone.tiles;
          int length = tiles.Length;
          for (int index = 0; index < length; ++index)
          {
            WorldTile worldTile = tiles[index];
            if (worldTile.isOnFire())
              QuantumSpriteLibrary.showLightAt(Vector2Int.op_Implicit(worldTile.pos), colorForLight, 0.2f);
          }
        }
      }
    }
    if (!Config.isComputer && !Config.isEditor || !PlayerConfig.optionBoolEnabled("cursor_lights"))
      return;
    QuantumSpriteLibrary.showLightAt(Vector2.op_Implicit(Vector2.op_Implicit(World.world.getMousePos())), white, 0.4f);
  }

  private static void checkBuildingLights(Building pBuilding, Color pColor)
  {
    if (pBuilding.hasAnyStatusEffect())
    {
      foreach (Status statuse in pBuilding.getStatuses())
      {
        if (statuse.asset.draw_light_area)
          QuantumSpriteLibrary.showLightAt(pBuilding.current_position, pColor, statuse.asset.draw_light_size);
      }
    }
    if (!pBuilding.asset.draw_light_area || !pBuilding.isUsable() || pBuilding.isAbandoned() || pBuilding.asset.hasHousingSlots() && !pBuilding.hasResidents())
      return;
    Vector3 vector3 = Vector2.op_Implicit(pBuilding.current_position);
    vector3.x += pBuilding.asset.draw_light_area_offset_x;
    vector3.y += pBuilding.asset.draw_light_area_offset_y;
    QuantumSpriteLibrary.showLightAt(Vector2.op_Implicit(vector3), pColor, pBuilding.asset.draw_light_size);
  }

  private static void checkUnitLight(Actor pActor, Color pColor)
  {
    if (pActor.a.has_tag_generate_light)
    {
      Vector2 currentPosition = pActor.current_position;
      currentPosition.y += pActor.getHeight();
      QuantumSpriteLibrary.showLightAt(currentPosition, pColor, 0.3f);
    }
    else
    {
      if (!pActor.hasAnyStatusEffect())
        return;
      foreach (Status statuse in pActor.getStatuses())
      {
        if (statuse.asset.draw_light_area)
          QuantumSpriteLibrary.showLightAt(pActor.current_position, pColor, statuse.asset.draw_light_size);
      }
    }
  }

  private static void drawBuildingsLightWindows(QuantumSpriteAsset pAsset)
  {
    if (!World.world.quality_changer.shouldRenderBuildings() || !World.world.era_manager.shouldShowLights() || !PlayerConfig.optionBoolEnabled("night_lights"))
      return;
    Color.white.a = !Randy.randomBool() ? 1f : 0.95f;
    int num = World.world.buildings.countVisibleBuildings();
    Building[] visibleBuildings = World.world.buildings.getVisibleBuildings();
    for (int index = 0; index < num; ++index)
    {
      Building pBuilding = visibleBuildings[index];
      if (pBuilding.asset.city_building && pBuilding.isUsable() && !pBuilding.isAbandoned() && (!pBuilding.asset.hasHousingSlots() || pBuilding.hasResidents()))
      {
        Sprite buildingLight = DynamicSprites.getBuildingLight(pBuilding);
        if (!Object.op_Equality((Object) buildingLight, (Object) null))
        {
          Vector3 transformPosition = pBuilding.cur_transform_position;
          transformPosition.z = -0.19f;
          QuantumSpriteLibrary.drawQuantumSprite(pAsset, transformPosition, pForceScaleTo: pBuilding.getCurrentScale().y).setSprite(buildingLight);
        }
      }
    }
  }

  private static void drawFamilySpeciesIcons(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("map_species_families"))
      return;
    foreach (Family family in (CoreSystemManager<Family, FamilyData>) World.world.families)
    {
      if (family.isAlive())
      {
        ActorAsset actorAsset = family.getActorAsset();
        if (family.units.Count != 0)
        {
          Actor unit = family.units[0];
          if (!unit.isRekt() && unit.current_zone.visible)
          {
            Sprite spriteIcon = actorAsset.getSpriteIcon();
            QuantumSpriteLibrary.drawQuantumSprite(pAsset, unit.current_tile.zone.centerTile.posV3).setSprite(spriteIcon);
          }
        }
      }
    }
  }

  private static void drawCursorTargetSubspecies(QuantumSpriteAsset pAsset)
  {
    if (!MapBox.isRenderGameplay() || !InputHelpers.mouseSupported || Object.op_Equality((Object) World.world.selected_buttons.selectedButton, (Object) null) || World.world.isBusyWithUI() || ControllableUnit.isControllingUnit() || MoveCamera.inSpectatorMode() || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
      return;
    WorldTile tilePosCachedFrame = World.world.getMouseTilePosCachedFrame();
    if (tilePosCachedFrame == null)
      return;
    GodPower selectedPowerAsset = World.world.getSelectedPowerAsset();
    if (selectedPowerAsset.type != PowerActionType.PowerSpawnActor)
      return;
    ActorAsset actorAsset = selectedPowerAsset.getActorAsset();
    Actor pSubspeciesActor;
    if (actorAsset == null || !actorAsset.can_have_subspecies || World.world.subspecies.getNearbySpecies(actorAsset, tilePosCachedFrame, out pSubspeciesActor) == null || !pSubspeciesActor.is_visible)
      return;
    Vector3 positionForFunRendering = pSubspeciesActor.getHeadOffsetPositionForFunRendering();
    QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(World.world.getMousePos()), positionForFunRendering, ref Toolbox.color_white);
  }

  private static void drawCursorSprite(QuantumSpriteAsset pAsset)
  {
    if (!InputHelpers.mouseSupported || Object.op_Equality((Object) World.world.selected_buttons.selectedButton, (Object) null) || World.world.isBusyWithUI() || ControllableUnit.isControllingUnit() || MoveCamera.inSpectatorMode() || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || World.world.getSelectedPowerAsset().ignore_cursor_icon)
      return;
    float scaleZoomMultiplier = QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset);
    Vector2 mousePos = World.world.getMousePos();
    mousePos.x += -0.3f * scaleZoomMultiplier;
    mousePos.y += -0.3f * scaleZoomMultiplier;
    QuantumSprite quantumSprite1 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(mousePos));
    quantumSprite1.setSprite(World.world.selected_buttons.selectedButton.icon.sprite);
    mousePos.x += 0.3f * scaleZoomMultiplier;
    mousePos.y += 0.3f * scaleZoomMultiplier;
    Color colorBlack = Toolbox.color_black;
    quantumSprite1.setSprite(World.world.selected_buttons.selectedButton.icon.sprite);
    colorBlack.a = 0.3f;
    quantumSprite1.setColor(ref colorBlack);
    ((Renderer) quantumSprite1.sprite_renderer).sortingOrder = 9;
    QuantumSprite quantumSprite2 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(mousePos));
    quantumSprite2.setSprite(World.world.selected_buttons.selectedButton.icon.sprite);
    ((Renderer) quantumSprite2.sprite_renderer).sortingOrder = 10;
  }

  private static void drawCursorAttackRecharge(QuantumSpriteAsset pAsset)
  {
    if (!InputHelpers.mouseSupported || World.world.isBusyWithUI() || !ControllableUnit.isControllingUnit())
      return;
    Actor controllableUnit = ControllableUnit.getControllableUnit();
    if (controllableUnit.asset.id == "crabzilla" || controllableUnit.isAttackReady())
      return;
    float attackCooldownRatio = controllableUnit.getAttackCooldownRatio();
    float scaleZoomMultiplier = QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset);
    Vector2 mousePos = World.world.getMousePos();
    mousePos.x += 2.5f * scaleZoomMultiplier;
    mousePos.y -= 2.5f * scaleZoomMultiplier;
    CircleIconShaderMod component = ((Component) QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(mousePos))).GetComponent<CircleIconShaderMod>();
    component.sprite_renderer_with_mat.sprite = QuantumSpriteLibrary._sprite_attack_reload;
    component.setShaderVal(attackCooldownRatio);
  }

  private static void drawUnexploredAugmentationSprite(QuantumSpriteAsset pQAsset)
  {
    if (!PowerLibrary.inspect_unit.isSelected() || WorldLawLibrary.world_law_cursed_world.isEnabled())
      return;
    Sprite fromListSessionTime = AnimationHelper.getSpriteFromListSessionTime(0, (IList<Sprite>) QuantumSpriteLibrary._unexplored_sprites, SimGlobals.m.unexplored_sprite_animation_speed);
    for (int index = 0; index < QuantumSpriteLibrary.visible_units_alive_count; ++index)
    {
      Actor pActor = QuantumSpriteLibrary.visible_units_alive[index];
      if (QuantumSpriteLibrary.checkShouldDrawUnexploredSpriteFor(pActor))
      {
        Vector3 positionForFunRendering = pActor.getHeadOffsetPositionForFunRendering();
        QuantumSpriteLibrary.drawQuantumSprite(pQAsset, positionForFunRendering, pForceScaleTo: pActor.current_scale.y).setSprite(fromListSessionTime);
      }
    }
  }

  private static bool checkShouldDrawUnexploredSpriteFor(Actor pActor)
  {
    if (pActor.asset.is_boat)
      return false;
    ActorAsset asset1 = pActor.asset;
    if (!asset1.isAvailable() && asset1.needs_to_be_explored)
      return true;
    if (pActor.hasEquipment())
    {
      foreach (Item obj in pActor.equipment.getItems())
      {
        EquipmentAsset asset2 = obj.getAsset();
        if (!asset2.isAvailable() && !asset2.unlocked_with_achievement && asset2.needs_to_be_explored)
          return true;
      }
    }
    return QuantumSpriteLibrary.checkAssetsForUnexplored((IReadOnlyCollection<BaseUnlockableAsset>) pActor.traits) || pActor.hasClan() && QuantumSpriteLibrary.checkAssetsForUnexplored((IReadOnlyCollection<BaseUnlockableAsset>) pActor.clan.getTraits()) || pActor.hasCulture() && QuantumSpriteLibrary.checkAssetsForUnexplored((IReadOnlyCollection<BaseUnlockableAsset>) pActor.culture.getTraits()) || pActor.hasLanguage() && QuantumSpriteLibrary.checkAssetsForUnexplored((IReadOnlyCollection<BaseUnlockableAsset>) pActor.language.getTraits()) || pActor.hasReligion() && QuantumSpriteLibrary.checkAssetsForUnexplored((IReadOnlyCollection<BaseUnlockableAsset>) pActor.religion.getTraits()) || pActor.hasSubspecies() && QuantumSpriteLibrary.checkAssetsForUnexplored((IReadOnlyCollection<BaseUnlockableAsset>) pActor.subspecies.getTraits()) || pActor.hasKingdom() && QuantumSpriteLibrary.checkAssetsForUnexplored((IReadOnlyCollection<BaseUnlockableAsset>) pActor.kingdom.getTraits());
  }

  private static bool checkAssetsForUnexplored(IReadOnlyCollection<BaseUnlockableAsset> pAssets)
  {
    foreach (BaseUnlockableAsset pAsset in (IEnumerable<BaseUnlockableAsset>) pAssets)
    {
      if (!pAsset.isAvailable() && !pAsset.unlocked_with_achievement && pAsset.needs_to_be_explored)
        return true;
    }
    return false;
  }

  private static void drawBuildingsOld(QuantumSpriteAsset pAsset)
  {
    int pPlannedSize = World.world.buildings.countVisibleBuildings();
    Building[] visibleBuildings = World.world.buildings.getVisibleBuildings();
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(pPlannedSize);
    for (int index = 0; index < pPlannedSize; ++index)
    {
      Building building = visibleBuildings[index];
      QuantumSprite quantumSprite = fastActiveList[index];
      Sprite render = building.checkSpriteToRender();
      Vector3 currentScale = building.getCurrentScale();
      Vector3 transformPosition = building.cur_transform_position;
      Vector3 currentRotation = building.current_rotation;
      Material material = building.material;
      bool flipX = building.flip_x;
      Color colorBuilding = building.kingdom.asset.color_building;
      quantumSprite.setSprite(render);
      quantumSprite.setScale(ref currentScale);
      quantumSprite.setSharedMat(material);
      quantumSprite.setPosOnly(ref transformPosition);
      quantumSprite.setRotation(ref currentRotation);
      quantumSprite.setFlipX(flipX);
      quantumSprite.setColor(ref colorBuilding);
    }
  }

  private static void drawBuildingsCache(QuantumSpriteAsset pAsset)
  {
    BuildingRenderData renderData = World.world.buildings.render_data;
    int num = World.world.buildings.countVisibleBuildings();
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(num);
    QuantumSpriteCacheData cacheData = pAsset.group_system.getCacheData(num);
    Sprite[] coloredSprites = renderData.colored_sprites;
    Material[] materials1 = renderData.materials;
    Vector3[] scales1 = renderData.scales;
    Vector3[] positions1 = renderData.positions;
    Vector3[] rotations1 = renderData.rotations;
    bool[] flipXStates1 = renderData.flip_x_states;
    Color[] colors1 = renderData.colors;
    Sprite[] sprites = cacheData.sprites;
    Material[] materials2 = cacheData.materials;
    Vector3[] scales2 = cacheData.scales;
    Vector3[] positions2 = cacheData.positions;
    Vector3[] rotations2 = cacheData.rotations;
    bool[] flipXStates2 = cacheData.flip_x_states;
    Color[] colors2 = cacheData.colors;
    for (int index = 0; index < num; ++index)
    {
      Sprite sprite = coloredSprites[index];
      if (sprites[index] != sprite)
      {
        sprites[index] = sprite;
        fastActiveList[index].sprite_renderer.sprite = sprite;
      }
    }
    for (int index = 0; index < num; ++index)
    {
      Material material = materials1[index];
      if (materials2[index] != material)
      {
        materials2[index] = material;
        ((Renderer) fastActiveList[index].sprite_renderer).sharedMaterial = material;
      }
    }
    for (int index = 0; index < num; ++index)
    {
      ref Vector3 local1 = ref scales1[index];
      ref Vector3 local2 = ref scales2[index];
      if ((double) local1.x != (double) local2.x || (double) local1.y != (double) local2.y || (double) local1.z != (double) local2.z)
      {
        local2 = local1;
        fastActiveList[index].m_transform.localScale = local1;
      }
    }
    for (int index = 0; index < num; ++index)
    {
      ref Vector3 local3 = ref positions1[index];
      ref Vector3 local4 = ref positions2[index];
      if ((double) local3.x != (double) local4.x || (double) local3.y != (double) local4.y || (double) local3.z != (double) local4.z)
      {
        local4 = local3;
        fastActiveList[index].m_transform.position = local3;
      }
    }
    for (int index = 0; index < num; ++index)
    {
      ref Vector3 local5 = ref rotations1[index];
      ref Vector3 local6 = ref rotations2[index];
      if ((double) local5.x != (double) local6.x || (double) local5.y != (double) local6.y || (double) local5.z != (double) local6.z)
      {
        local6 = local5;
        fastActiveList[index].m_transform.eulerAngles = local5;
      }
    }
    for (int index = 0; index < num; ++index)
    {
      ref bool local7 = ref flipXStates1[index];
      ref bool local8 = ref flipXStates2[index];
      if (local7 != local8)
      {
        local8 = local7;
        fastActiveList[index].sprite_renderer.flipX = local7;
      }
    }
    for (int index = 0; index < num; ++index)
    {
      ref Color local9 = ref colors1[index];
      ref Color local10 = ref colors2[index];
      if ((double) local9.r != (double) local10.r || (double) local9.g != (double) local10.g || (double) local9.b != (double) local10.b || (double) local9.a != (double) local10.a)
      {
        local10 = local9;
        fastActiveList[index].sprite_renderer.color = local9;
      }
    }
  }

  private static void drawBuildings(QuantumSpriteAsset pAsset)
  {
    BuildingRenderData renderData = World.world.buildings.render_data;
    int pPlannedSize = World.world.buildings.countVisibleBuildings();
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(pPlannedSize);
    Sprite[] coloredSprites = renderData.colored_sprites;
    Material[] materials = renderData.materials;
    Vector3[] scales = renderData.scales;
    Vector3[] positions = renderData.positions;
    Vector3[] rotations = renderData.rotations;
    bool[] flipXStates = renderData.flip_x_states;
    Color[] colors = renderData.colors;
    for (int index = 0; index < pPlannedSize; ++index)
    {
      QuantumSprite quantumSprite = fastActiveList[index];
      quantumSprite.setSprite(coloredSprites[index]);
      quantumSprite.setSharedMat(materials[index]);
      quantumSprite.setScale(ref scales[index]);
      quantumSprite.setPosOnly(ref positions[index]);
      quantumSprite.setRotation(ref rotations[index]);
      quantumSprite.setFlipX(flipXStates[index]);
      quantumSprite.setColor(ref colors[index]);
    }
  }

  private static void drawParabolicUnload(QuantumSpriteAsset pAsset)
  {
    List<ResourceThrowData> list = World.world.resource_throw_manager.getList();
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(list.Count);
    for (int index = list.Count - 1; index >= 0; --index)
    {
      ResourceThrowData resourceThrowData = list[index];
      QuantumSprite quantumSprite = fastActiveList[index];
      float ratio = resourceThrowData.getRatio();
      Vector3 pPosition = Vector2.op_Implicit(Toolbox.Parabola(resourceThrowData.position_start, resourceThrowData.position_end, resourceThrowData.height, ratio));
      pPosition.z = 4f;
      float pScale = 0.1f;
      quantumSprite.setSprite(AssetManager.resources.get(resourceThrowData.resource_asset_id).getGameplaySprite());
      quantumSprite.setPosOnly(ref pPosition);
      quantumSprite.setScale(pScale);
      ((Component) quantumSprite).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, ratio * 360f));
    }
  }

  private static void drawUnitsEffectDamage(QuantumSpriteAsset pAsset)
  {
    List<ActorDamageEffectData> actorEffectHit = World.world.stack_effects.actor_effect_hit;
    for (int index = actorEffectHit.Count - 1; index >= 0; --index)
    {
      ActorDamageEffectData damageEffectData = actorEffectHit[index];
      float timeElapsedSince = World.world.getRealTimeElapsedSince(damageEffectData.timestamp);
      Actor actor = damageEffectData.actor;
      if ((double) timeElapsedSince > 0.30000001192092896 || !actor.isAlive() || !actor.is_visible)
      {
        actorEffectHit.RemoveAt(index);
      }
      else
      {
        QuantumSprite next = pAsset.group_system.getNext();
        Vector3 pVec = actor.updateRotation();
        Vector3 currentScale = actor.current_scale;
        Vector3 transformPosition = actor.cur_transform_position;
        Sprite render = actor.checkSpriteToRender();
        Color white = Color.white;
        white.a = (float) (1.0 - (double) timeElapsedSince / 0.30000001192092896);
        next.setSprite(render);
        next.setPosOnly(ref transformPosition);
        next.setScale(ref currentScale);
        next.setRotation(ref pVec);
        next.setColor(ref white);
      }
    }
  }

  private static void drawUnitsEffectHighlight(QuantumSpriteAsset pAsset)
  {
    List<ActorHighlightEffectData> actorEffectHighlight = World.world.stack_effects.actor_effect_highlight;
    for (int index = actorEffectHighlight.Count - 1; index >= 0; --index)
    {
      ActorHighlightEffectData highlightEffectData = actorEffectHighlight[index];
      float timeElapsedSince = World.world.getRealTimeElapsedSince(highlightEffectData.timestamp);
      Actor actor = highlightEffectData.actor;
      if ((double) timeElapsedSince > 0.30000001192092896 || !actor.isAlive() || !actor.is_visible)
      {
        actorEffectHighlight.RemoveAt(index);
      }
      else
      {
        QuantumSprite next = pAsset.group_system.getNext();
        Vector3 pVec = actor.updateRotation();
        Vector3 currentScale = actor.current_scale;
        Vector3 transformPosition = actor.cur_transform_position;
        Sprite render = actor.checkSpriteToRender();
        Color white = Color.white;
        white.a = (float) (1.0 - (double) timeElapsedSince / 0.30000001192092896);
        next.setSprite(render);
        next.setPosOnly(ref transformPosition);
        next.setScale(ref currentScale);
        next.setRotation(ref pVec);
        next.setColor(ref white);
      }
    }
  }

  private static void drawSquareSelection(QuantumSpriteAsset pAsset)
  {
    if (!World.world.player_control.square_selection_started)
      return;
    float scaleZoomMultiplier = QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset);
    Color architectColor = World.world.getArchitectColor();
    Vector2 selectionPositionCurrent = World.world.player_control.square_selection_position_current;
    Vector2 mousePos = World.world.getMousePos();
    float num1 = mousePos.x - selectionPositionCurrent.x;
    float num2 = mousePos.y - selectionPositionCurrent.y;
    float num3 = 0.1f * scaleZoomMultiplier;
    Color pColor = architectColor;
    pColor.a = 0.3f;
    QuantumSprite quantumSprite1 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(selectionPositionCurrent));
    quantumSprite1.setSprite(QuantumSpriteLibrary._sprite_pixel);
    ((Component) quantumSprite1).transform.localScale = new Vector3(num1, num2);
    quantumSprite1.setColor(ref pColor);
    QuantumSprite quantumSprite2 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(selectionPositionCurrent));
    quantumSprite2.setSprite(QuantumSpriteLibrary._sprite_pixel);
    ((Component) quantumSprite2).transform.localScale = new Vector3(num1, num3);
    quantumSprite2.setColor(ref architectColor);
    QuantumSprite quantumSprite3 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(selectionPositionCurrent));
    quantumSprite3.setSprite(QuantumSpriteLibrary._sprite_pixel);
    ((Component) quantumSprite3).transform.localScale = new Vector3(num3, num2);
    quantumSprite3.setColor(ref architectColor);
    QuantumSprite quantumSprite4 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(mousePos));
    quantumSprite4.setSprite(QuantumSpriteLibrary._sprite_pixel);
    ((Component) quantumSprite4).transform.localScale = new Vector3(-num1, num3);
    quantumSprite4.setColor(ref architectColor);
    QuantumSprite quantumSprite5 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(mousePos));
    quantumSprite5.setSprite(QuantumSpriteLibrary._sprite_pixel);
    ((Component) quantumSprite5).transform.localScale = new Vector3(num3, -num2);
    quantumSprite5.setColor(ref architectColor);
  }

  private static void drawArrowsUnitCursor(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_destination") || ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor.isRekt() || lastActor.isInMagnet())
      return;
    WorldTile currentTile = lastActor.current_tile;
    WorldTile tileTarget = lastActor.tile_target;
    if (currentTile != null && tileTarget != null)
      QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(lastActor.current_position), tileTarget.posV3, ref pAsset.color);
    if (!lastActor.has_attack_target || !lastActor.isEnemyTargetAlive())
      return;
    QuantumSpriteAsset pAsset1 = AssetManager.quantum_sprites.get("debug_arrows_units_attack_targets");
    QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset1, Vector2.op_Implicit(lastActor.current_position), Vector2.op_Implicit(lastActor.attack_target.current_position), ref pAsset1.color);
  }

  private static void drawArrowsUnitCursorSelectedRaycasts(QuantumSpriteAsset pAsset)
  {
    if (!ControllableUnit.isControllingUnit() || !Input.GetKey((KeyCode) 304))
      return;
    foreach (Actor cotrolledUnit in ControllableUnit.getCotrolledUnits())
    {
      if (cotrolledUnit.isRekt() || cotrolledUnit.isInMagnet())
        break;
      Vector2 currentPosition = cotrolledUnit.current_position;
      Vector2 mousePos = World.world.getMousePos();
      Color red = Color.red;
      Color white = Color.white;
      Color black = Color.black;
      Vector2 pTarget = mousePos;
      List<WorldTile> worldTileList = PathfinderTools.raycast(currentPosition, pTarget);
      bool flag1 = false;
      for (int index = 0; index < worldTileList.Count; ++index)
      {
        WorldTile worldTile = worldTileList[index];
        float pForceScaleTo = (float) (0.05000000074505806 + (double) index * 0.10000000149011612 * 0.05000000074505806);
        bool flag2 = false;
        if (index > 0 && worldTile.countUnits() > 0)
        {
          flag1 = true;
          flag2 = true;
        }
        float num = Toolbox.getAngleDegrees((float) worldTile.x, (float) worldTile.y, mousePos.x, mousePos.y) - 45f;
        QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, worldTile.posV3, pForceScaleTo: pForceScaleTo);
        quantumSprite.setSprite(SpriteTextureLoader.getSprite("ui/Icons/iconAttack"));
        ((Component) quantumSprite).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, num));
        if (flag2)
          quantumSprite.setColor(ref red);
        else if (flag1)
          quantumSprite.setColor(ref black);
        else
          quantumSprite.setColor(ref white);
      }
    }
  }

  private static void drawArrowsUnitCursorSelected(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_destination") || ControllableUnit.isControllingUnit())
      return;
    float timeElapsedSince = World.world.getRealTimeElapsedSince(QuantumSpriteLibrary.last_order_timestamp);
    if ((double) timeElapsedSince > 2.0)
      return;
    float num1 = (float) (1.0 - (double) timeElapsedSince / 2.0);
    Color architectColor = World.world.getArchitectColor();
    int num2 = 0;
    foreach (Actor allSelected in SelectedUnit.getAllSelectedList())
    {
      if (allSelected.isRekt() || allSelected.isInMagnet())
        break;
      architectColor.a = !SelectedUnit.isMainSelected(allSelected) ? num1 * 0.4f : num1;
      WorldTile currentTile = allSelected.current_tile;
      WorldTile tileTarget = allSelected.tile_target;
      if (currentTile != null && tileTarget != null)
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(allSelected.current_position), tileTarget.posV3, ref architectColor);
      ++num2;
      if (num2 > 20)
        break;
    }
  }

  private static void drawArrowsUnitCursorLover(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_lover") || ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor.isRekt() || lastActor.isInMagnet() || !lastActor.hasLover())
      return;
    Vector3 pStart = Vector2.op_Implicit(lastActor.current_position);
    Vector3 pEnd = Vector2.op_Implicit(lastActor.lover.current_position);
    QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref pAsset.color);
  }

  private static void drawArrowsUnitCursorHouse(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_house") || ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor.isRekt() || lastActor.isInMagnet() || !lastActor.hasHouse())
      return;
    Vector3 pStart = Vector2.op_Implicit(lastActor.current_position);
    Vector3 pEnd = Vector2.op_Implicit(lastActor.getHomeBuilding().current_position);
    QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref pAsset.color);
  }

  private static void drawArrowsUnitCursorFamily(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_family") || ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor.isRekt() || lastActor.isInMagnet() || !lastActor.hasFamily())
      return;
    Vector3 pStart = Vector2.op_Implicit(lastActor.current_position);
    foreach (Actor unit in lastActor.family.units)
    {
      if (unit != lastActor && !unit.isRekt())
      {
        Vector3 pEnd = Vector2.op_Implicit(unit.current_position);
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref pAsset.color);
      }
    }
  }

  private static void drawArrowsUnitCursorParents(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_parents") || ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor.isRekt() || lastActor.isInMagnet())
      return;
    Vector3 pStart = Vector2.op_Implicit(lastActor.current_position);
    foreach (BaseSimObject parent in lastActor.getParents())
    {
      Vector3 pEnd = Vector2.op_Implicit(parent.current_position);
      QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref pAsset.color);
    }
  }

  private static void drawArrowsUnitCursorKids(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_kids") || ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor.isRekt() || lastActor.isInMagnet())
      return;
    Vector3 pStart = Vector2.op_Implicit(lastActor.current_position);
    foreach (BaseSimObject child in lastActor.getChildren(false))
    {
      Vector3 pEnd = Vector2.op_Implicit(child.current_position);
      QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref pAsset.color);
    }
  }

  private static void drawArrowsUnitCursorAttackTarget(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("cursor_arrow_attack_target") || ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor.isRekt() || lastActor.isInMagnet() || !lastActor.has_attack_target || lastActor.attack_target.isRekt())
      return;
    BaseSimObject attackTarget = lastActor.attack_target;
    Vector3 pStart = Vector2.op_Implicit(lastActor.current_position);
    Vector3 pEnd = Vector2.op_Implicit(attackTarget.current_position);
    QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref pAsset.color);
  }

  private static void drawWalls(QuantumSpriteAsset pAsset)
  {
    GodPower selectedPowerAsset = World.world.getSelectedPowerAsset();
    bool pTransparentBuildings = selectedPowerAsset != null && selectedPowerAsset.make_buildings_transparent;
    Material matWorldObject = LibraryMaterials.instance.mat_world_object;
    QuantumSpriteLibrary.drawWallType(TopTileLibrary.wall_order, pAsset, pTransparentBuildings, matWorldObject);
    QuantumSpriteLibrary.drawWallType(TopTileLibrary.wall_evil, pAsset, pTransparentBuildings, matWorldObject);
    QuantumSpriteLibrary.drawWallType(TopTileLibrary.wall_ancient, pAsset, pTransparentBuildings, matWorldObject);
    QuantumSpriteLibrary.drawWallType(TopTileLibrary.wall_wild, pAsset, pTransparentBuildings, matWorldObject);
    QuantumSpriteLibrary.drawWallType(TopTileLibrary.wall_iron, pAsset, pTransparentBuildings, matWorldObject);
    QuantumSpriteLibrary.drawWallType(TopTileLibrary.wall_green, pAsset, pTransparentBuildings, matWorldObject);
    QuantumSpriteLibrary.drawWallType(TopTileLibrary.wall_light, pAsset, pTransparentBuildings, World.world.library_materials.mat_world_object_lit);
  }

  private static void drawWallLightBlobs(QuantumSpriteAsset pAsset)
  {
    if (!World.world.era_manager.shouldShowLights())
      return;
    List<WorldTile> currentTiles = TopTileLibrary.wall_light.getCurrentTiles();
    if (currentTiles.Count == 0)
      return;
    for (int index = 0; index < currentTiles.Count; ++index)
    {
      WorldTile worldTile = currentTiles[index];
      if (worldTile.zone.visible)
        World.world.stack_effects.light_blobs.Add(new LightBlobData()
        {
          position = Vector2.op_Implicit(worldTile.posV3),
          radius = 0.3f
        });
    }
  }

  private static void drawLavaLightBlobs(QuantumSpriteAsset pAsset)
  {
    if (!World.world.era_manager.shouldShowLights())
      return;
    List<TileZone> visibleZones = World.world.zone_camera.getVisibleZones();
    for (int index = 0; index < visibleZones.Count; ++index)
    {
      TileZone tileZone = visibleZones[index];
      LightBlobData lightBlobData1;
      if (tileZone.hasLava())
      {
        if (tileZone.countLava() < 5)
        {
          foreach (WorldTile worldTile in tileZone.loopLava())
          {
            List<LightBlobData> lightBlobs = World.world.stack_effects.light_blobs;
            lightBlobData1 = new LightBlobData();
            lightBlobData1.position = Vector2.op_Implicit(worldTile.posV3);
            lightBlobData1.radius = 0.2f;
            LightBlobData lightBlobData2 = lightBlobData1;
            lightBlobs.Add(lightBlobData2);
          }
        }
        else
        {
          List<LightBlobData> lightBlobs = World.world.stack_effects.light_blobs;
          lightBlobData1 = new LightBlobData();
          lightBlobData1.position = Vector2.op_Implicit(tileZone.centerTile.posV3);
          lightBlobData1.radius = 1f;
          LightBlobData lightBlobData3 = lightBlobData1;
          lightBlobs.Add(lightBlobData3);
        }
      }
    }
  }

  private static void drawWallType(
    TopTileType pTileTypeAsset,
    QuantumSpriteAsset pAsset,
    bool pTransparentBuildings,
    Material pMaterial)
  {
    List<WorldTile> currentTiles = pTileTypeAsset.getCurrentTiles();
    if (currentTiles.Count == 0)
      return;
    float pScaleX = World.world.quality_changer.getTweenBuildingsValue() * 0.25f;
    float pScaleY = pScaleX;
    float num1 = 0.1f;
    int num2 = pTransparentBuildings ? 1 : 0;
    for (int index = 0; index < currentTiles.Count; ++index)
    {
      WorldTile pTile = currentTiles[index];
      if (pTile.zone.visible)
      {
        Sprite sprite = WallHelper.getSprite(pTile, pTileTypeAsset);
        QuantumSprite next = pAsset.group_system.getNext();
        next.setSprite(sprite);
        Vector3 posV3 = pTile.posV3;
        posV3.z = Mathf.Repeat(posV3.x * 0.0001f, num1);
        next.setPosOnly(ref posV3);
        next.setScale(pScaleX, pScaleY);
        next.setSharedMat(pMaterial);
      }
    }
  }

  private static void drawUnitsAvatars(QuantumSpriteAsset pAsset)
  {
    Actor[] array = World.world.units.visible_units_avatars.array;
    int count = World.world.units.visible_units_avatars.count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      if (!actor.asset.ignore_generic_render)
      {
        Transform transform = actor.avatar.transform;
        if (!actor.is_visible)
        {
          transform.position = Globals.POINT_IN_VOID;
        }
        else
        {
          Vector3 vector3_1 = actor.updateRotation();
          Vector3 currentScale = actor.current_scale;
          Vector3 vector3_2 = actor.updatePos();
          transform.position = vector3_2;
          transform.localScale = currentScale;
          transform.eulerAngles = vector3_1;
        }
      }
    }
  }

  private static void drawHealthbars(QuantumSpriteAsset pAsset)
  {
    bool flag1 = SelectedUnit.isSet();
    bool flag2 = HotkeyLibrary.isHoldingAlt();
    if (!flag2 && !flag1)
      return;
    if (flag2)
      flag1 = false;
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    if (Zones.getCurrentMapBorderMode().isNone())
      return;
    ref Color local1 = ref ColorStyleLibrary.m.health_bar_background;
    ref Color local2 = ref ColorStyleLibrary.m.health_bar_main_green;
    ref Color local3 = ref ColorStyleLibrary.m.health_bar_main_red;
    float num1 = QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset) * 1.6f;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor pActor = visibleUnitsAlive[index];
      if (!flag1 || SelectedUnit.isSelected(pActor))
      {
        float healthRatio = pActor.getHealthRatio();
        if ((double) healthRatio < 1.0)
        {
          float num2 = 0.1f;
          float num3 = 9f * num2 * num1;
          float num4 = 1.5f * num2 * num1;
          Vector3 pPos = new Vector3();
          pPos.x = pActor.cur_transform_position.x - num3 / 2f;
          pPos.y = pActor.cur_transform_position.y + 13f * num2;
          if ((double) healthRatio < 1.0)
          {
            QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos);
            quantumSprite.setSprite(QuantumSpriteLibrary._sprite_pixel);
            ((Component) quantumSprite).transform.localScale = new Vector3(num3, num4);
            quantumSprite.setColor(ref local1);
          }
          ref Color local4 = ref local2;
          if ((double) pActor.getHealthRatio() < 0.40000000596046448)
            local4 = ref local3;
          pPos.z += 0.01f;
          QuantumSprite quantumSprite1 = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos);
          quantumSprite1.setSprite(QuantumSpriteLibrary._sprite_pixel);
          ((Component) quantumSprite1).transform.localScale = new Vector3(num3 * healthRatio, num4);
          quantumSprite1.setColor(ref local4);
        }
      }
    }
  }

  private static void drawUnits(QuantumSpriteAsset pAsset)
  {
    ActorRenderData renderData = World.world.units.render_data;
    int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
    if (visibleUnitsCount == 0)
      return;
    bool[] hasNormalRender = renderData.has_normal_render;
    Sprite[] mainSpriteColored = renderData.main_sprite_colored;
    Vector3[] positions1 = renderData.positions;
    Vector3[] scales1 = renderData.scales;
    Vector3[] rotations1 = renderData.rotations;
    Color[] colors1 = renderData.colors;
    if (QuantumSpriteLibrary._q_render_indexes_units.Length < visibleUnitsCount)
      QuantumSpriteLibrary._q_render_indexes_units = Toolbox.checkArraySize<int>(QuantumSpriteLibrary._q_render_indexes_units, visibleUnitsCount);
    int[] renderIndexesUnits = QuantumSpriteLibrary._q_render_indexes_units;
    int num = 0;
    for (int index = 0; index < visibleUnitsCount; ++index)
    {
      if (hasNormalRender[index])
        renderIndexesUnits[num++] = index;
    }
    if (num == 0)
      return;
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(num);
    QuantumSpriteCacheData cacheData = pAsset.group_system.getCacheData(num);
    Sprite[] sprites = cacheData.sprites;
    Vector3[] positions2 = cacheData.positions;
    Vector3[] scales2 = cacheData.scales;
    Vector3[] rotations2 = cacheData.rotations;
    Color[] colors2 = cacheData.colors;
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = renderIndexesUnits[index1];
      Sprite sprite = mainSpriteColored[index2];
      if (sprites[index1] != sprite)
      {
        sprites[index1] = sprite;
        fastActiveList[index1].sprite_renderer.sprite = sprite;
      }
    }
    for (int index3 = 0; index3 < num; ++index3)
    {
      int index4 = renderIndexesUnits[index3];
      Transform transform = fastActiveList[index3].m_transform;
      ref Vector3 local1 = ref positions1[index4];
      ref Vector3 local2 = ref positions2[index3];
      if ((double) local1.x != (double) local2.x || (double) local1.y != (double) local2.y || (double) local1.z != (double) local2.z)
      {
        local2 = local1;
        transform.position = local1;
      }
      ref Vector3 local3 = ref scales1[index4];
      ref Vector3 local4 = ref scales2[index3];
      if ((double) local3.x != (double) local4.x || (double) local3.y != (double) local4.y || (double) local3.z != (double) local4.z)
      {
        local4 = local3;
        transform.localScale = local3;
      }
      ref Vector3 local5 = ref rotations1[index4];
      ref Vector3 local6 = ref rotations2[index3];
      if ((double) local5.x != (double) local6.x || (double) local5.y != (double) local6.y || (double) local5.z != (double) local6.z)
      {
        local6 = local5;
        transform.eulerAngles = local5;
      }
      ref Color local7 = ref colors1[index4];
      ref Color local8 = ref colors2[index3];
      if ((double) local7.r != (double) local8.r || (double) local7.g != (double) local8.g || (double) local7.b != (double) local8.b)
      {
        local8 = local7;
        fastActiveList[index3].sprite_renderer.color = local7;
      }
    }
  }

  private static void drawUnitItems(QuantumSpriteAsset pAsset)
  {
    ActorRenderData renderData = World.world.units.render_data;
    int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
    if (visibleUnitsCount == 0)
      return;
    bool[] hasItem = renderData.has_item;
    Vector3[] itemScale = renderData.item_scale;
    Vector3[] itemPos = renderData.item_pos;
    Vector3[] rotations1 = renderData.rotations;
    Sprite[] itemSprites = renderData.item_sprites;
    if (QuantumSpriteLibrary._q_render_indexes_unit_items.Length < visibleUnitsCount)
      QuantumSpriteLibrary._q_render_indexes_unit_items = Toolbox.checkArraySize<int>(QuantumSpriteLibrary._q_render_indexes_unit_items, visibleUnitsCount);
    int[] indexesUnitItems = QuantumSpriteLibrary._q_render_indexes_unit_items;
    int num = 0;
    for (int index = 0; index < visibleUnitsCount; ++index)
    {
      if (hasItem[index])
        indexesUnitItems[num++] = index;
    }
    if (num == 0)
      return;
    QuantumSpriteCacheData cacheData = pAsset.group_system.getCacheData(num);
    Vector3[] scales = cacheData.scales;
    Vector3[] positions = cacheData.positions;
    Vector3[] rotations2 = cacheData.rotations;
    Sprite[] sprites = cacheData.sprites;
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(num);
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = indexesUnitItems[index1];
      ref Vector3 local1 = ref itemScale[index2];
      ref Vector3 local2 = ref scales[index1];
      if ((double) local1.x != (double) local2.x || (double) local1.y != (double) local2.y || (double) local1.z != (double) local2.z)
      {
        local2 = local1;
        fastActiveList[index1].m_transform.localScale = local1;
      }
      ref Vector3 local3 = ref itemPos[index2];
      ref Vector3 local4 = ref positions[index1];
      if ((double) local3.x != (double) local4.x || (double) local3.y != (double) local4.y || (double) local3.z != (double) local4.z)
      {
        local4 = local3;
        fastActiveList[index1].m_transform.position = local3;
      }
      ref Vector3 local5 = ref rotations1[index2];
      ref Vector3 local6 = ref rotations2[index1];
      if ((double) local5.x != (double) local6.x || (double) local5.y != (double) local6.y || (double) local5.z != (double) local6.z)
      {
        local6 = local5;
        fastActiveList[index1].m_transform.eulerAngles = local5;
      }
    }
    for (int index3 = 0; index3 < num; ++index3)
    {
      int index4 = indexesUnitItems[index3];
      Sprite sprite = itemSprites[index4];
      if (sprites[index3] != sprite)
      {
        sprites[index3] = sprite;
        fastActiveList[index3].sprite_renderer.sprite = sprite;
      }
    }
  }

  private static void drawFires(QuantumSpriteAsset pAsset)
  {
    if (!WorldBehaviourActionFire.hasFires())
      return;
    int num = 0;
    if (QuantumSpriteLibrary._q_render_indexes_sprites_fire.Length < World.world.tile_manager.tiles_count)
      QuantumSpriteLibrary._q_render_indexes_sprites_fire = new int[World.world.tile_manager.tiles_count];
    int[] indexesSpritesFire = QuantumSpriteLibrary._q_render_indexes_sprites_fire;
    float animationGlobalTime = AnimationHelper.getAnimationGlobalTime(10f);
    Sprite[][] fireSpritesSets = QuantumSpriteLibrary._fire_sprites_sets;
    int[] fires1 = WorldBehaviourActionFire.getFires();
    int[] randomSeeds = World.world.tile_manager.random_seeds;
    int[] fireAnimationSet = World.world.tile_manager.fire_animation_set;
    List<TileZone> visibleZones = World.world.zone_camera.getVisibleZones();
    Vector3[] positionsVector3 = World.world.tile_manager.positions_vector3;
    bool[] fires2 = World.world.tile_manager.fires;
    for (int index1 = 0; index1 < visibleZones.Count; ++index1)
    {
      TileZone tileZone = visibleZones[index1];
      if (fires1[tileZone.id] != 0)
      {
        WorldTile[] tiles = tileZone.tiles;
        int length = tiles.Length;
        for (int index2 = 0; index2 < length; ++index2)
        {
          int tileId = tiles[index2].tile_id;
          if (fires2[tileId])
            indexesSpritesFire[num++] = tileId;
        }
      }
    }
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(num);
    QuantumSpriteCacheData cacheData = pAsset.group_system.getCacheData(num);
    Vector3[] positions = cacheData.positions;
    int[] indexes = cacheData.indexes;
    int[] indexes2 = cacheData.indexes_2;
    for (int index3 = 0; index3 < num; ++index3)
    {
      int index4 = indexesSpritesFire[index3];
      int index5 = fireAnimationSet[index4];
      Sprite[] spriteArray = fireSpritesSets[index5];
      Vector3 vector3 = positionsVector3[index4];
      ref Vector3 local = ref positions[index3];
      if ((double) vector3.x != (double) local.x || (double) vector3.y != (double) local.y || (double) vector3.z != (double) local.z)
      {
        local = vector3;
        fastActiveList[index3].m_transform.position = vector3;
      }
      int index6 = (int) ((double) animationGlobalTime + (double) (randomSeeds[index4] * 100)) % spriteArray.Length;
      if (indexes[index3] != index6 || indexes2[index3] != index5)
      {
        indexes[index3] = index6;
        indexes2[index3] = index5;
        Sprite sprite = spriteArray[index6];
        fastActiveList[index3].sprite_renderer.sprite = sprite;
      }
    }
  }

  private static void drawSocialize(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("talk_bubbles"))
      return;
    float num1 = 1f;
    double curSessionTime = World.world.getCurSessionTime();
    Actor[] array = World.world.units.visible_units_socialize.array;
    int num2 = Math.Min(World.world.units.visible_units_socialize.count, 1000);
    for (int index = 0; index < num2; ++index)
    {
      Actor actor = array[index];
      if (!actor.hasTrait("mute"))
      {
        CommunicationAsset normal = CommunicationLibrary.normal;
        float num3 = (float) (curSessionTime - actor.timestamp_tween_session_social);
        if ((double) num3 > (double) num1)
          num3 = 1f;
        Vector3 positionForFunRendering = actor.getHeadOffsetPositionForFunRendering();
        float num4 = iTween.easeOutCubic(0.0f, 1f, num3);
        float num5 = Randy.randomFloat(-0.03f, 0.03f);
        float num6 = Randy.randomFloat(-0.03f, 0.03f);
        Vector2 vector2 = Vector2.op_Implicit(actor.current_scale);
        float num7 = positionForFunRendering.x + num5 * vector2.x;
        float num8 = positionForFunRendering.y + num6 * vector2.y;
        Vector2 pPosition1;
        // ISSUE: explicit constructor call
        ((Vector2) ref pPosition1).\u002Ector(num7, num8);
        vector2.y *= num4;
        QuantumSprite next1 = pAsset.group_system.getNext();
        next1.set(ref pPosition1, vector2.y);
        Sprite spriteBubble = normal.getSpriteBubble();
        next1.setSprite(spriteBubble);
        if (normal.show_topic)
        {
          Vector3 pPosition2 = Vector2.op_Implicit(pPosition1);
          pPosition2.x += -1.65f * actor.current_scale.x;
          pPosition2.y += 10.04f * actor.current_scale.y;
          pPosition2.z = pPosition1.y + 3f * actor.current_scale.y;
          QuantumSprite next2 = pAsset.group_system.getNext();
          next2.set(ref pPosition2, vector2.y * 0.35f);
          next2.setSprite(actor.getSocializeTopic());
        }
      }
    }
  }

  private static void drawJustAte(QuantumSpriteAsset pAsset)
  {
    float num1 = 1f;
    double curSessionTime = World.world.getCurSessionTime();
    Actor[] array = World.world.units.visible_units_just_ate.array;
    int count = World.world.units.visible_units_just_ate.count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      float num2 = (float) (curSessionTime - actor.timestamp_session_ate_food);
      if ((double) num2 > (double) num1)
      {
        actor.timestamp_session_ate_food = 0.0;
      }
      else
      {
        float num3 = num2 / num1;
        float num4 = iTween.easeOutCubic(0.0f, 1f, num3);
        Vector3 pPos = Vector2.op_Implicit(actor.current_position);
        pPos.y += (float) (1.0 + (double) num4 * 2.0);
        float pModScale = num4;
        if ((double) pModScale > 0.5)
          pModScale = 0.5f;
        QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pModScale: pModScale);
        quantumSprite.setSprite(AssetManager.resources.get(actor.ate_last_item_id).getSpriteIcon());
        ((Component) quantumSprite).transform.eulerAngles = new Vector3(0.0f, 0.0f, num4 * 360f);
        float num5 = 1f;
        if ((double) num3 > 0.6)
          num5 = (float) ((1.0 - (double) num3) / 0.40000000596046448);
        Color pColor;
        // ISSUE: explicit constructor call
        ((Color) ref pColor).\u002Ector(num5, num5, num5, num5);
        quantumSprite.setColor(ref pColor);
      }
    }
  }

  private static void drawCapturingZones(QuantumSpriteAsset pAsset)
  {
    if (!Zones.showKingdomZones() && !Zones.showCityZones() && !Zones.showAllianceZones())
      return;
    using (ListPool<TileZone> pResults = new ListPool<TileZone>())
    {
      foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
      {
        if (!city.being_captured_by.isRekt() && city.hasZones())
        {
          float pTicks = (float) city.last_visual_capture_ticks / 100f * (float) city.zones.Count;
          if ((double) pTicks > (double) city.zones.Count)
            pTicks = (float) city.zones.Count;
          CapturingZonesCalculator.getListToDraw(city, (int) pTicks, pResults);
          for (int index = 0; index < pResults.Count; ++index)
          {
            TileZone tileZone = pResults[index];
            QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, tileZone.centerTile, (WorldTile) null);
            Color borderOutCapture = city.being_captured_by.getColor().getColorBorderOut_capture();
            ref Color local = ref borderOutCapture;
            quantumSprite.setColor(ref local);
          }
        }
      }
    }
  }

  private static void drawUnityLine(QuantumSpriteAsset pAsset)
  {
    if (!InputHelpers.mouseSupported || World.world.isBusyWithUI() || !World.world.isSelectedPower("unity"))
      return;
    Kingdom unityA = Config.unity_A;
    if (unityA == null)
      return;
    Vector2 mousePos = World.world.getMousePos();
    foreach (City city in unityA.getCities())
    {
      Color colorMainSecond = unityA.getColor().getColorMainSecond();
      QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, city.getTile().posV, Vector2.op_Implicit(mousePos), ref colorMainSecond);
    }
  }

  private static void drawWhisperOfWarLine(QuantumSpriteAsset pAsset)
  {
    if (!InputHelpers.mouseSupported || World.world.isBusyWithUI() || !World.world.isSelectedPower("whisper_of_war"))
      return;
    Kingdom whisperA = Config.whisper_A;
    if (whisperA == null)
      return;
    Vector2 mousePos = World.world.getMousePos();
    foreach (City city in whisperA.getCities())
    {
      Color colorMainSecond = whisperA.getColor().getColorMainSecond();
      QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, city.getTile().posV, Vector2.op_Implicit(mousePos), ref colorMainSecond);
    }
  }

  private static void drawWhisperOfWar(QuantumSpriteAsset pAsset)
  {
    if (World.world.isBusyWithUI() || !World.world.isSelectedPower("whisper_of_war"))
      return;
    City city1 = World.world.getMouseTilePosCachedFrame()?.zone.city;
    Kingdom pKingdom;
    if (Config.whisper_A == null)
    {
      if (city1 == null)
        return;
      pKingdom = city1.kingdom;
    }
    else
      pKingdom = Config.whisper_A;
    foreach (City city2 in pKingdom.getCities())
      QuantumSpriteLibrary.colorZones(pAsset, city2.zones, pAsset.color);
    QuantumSpriteLibrary.colorEnemies(pAsset, pKingdom);
  }

  private static void drawSelectedKingdomZones(QuantumSpriteAsset pAsset)
  {
    if (!World.world.isSelectedPower("relations") || SelectedMetas.selected_kingdom == null)
      return;
    foreach (City city in SelectedMetas.selected_kingdom.getCities())
      QuantumSpriteLibrary.colorZones(pAsset, city.zones, pAsset.color);
    QuantumSpriteLibrary.colorEnemies(pAsset, SelectedMetas.selected_kingdom);
  }

  private static void drawCursorZones(QuantumSpriteAsset pAsset)
  {
    if (World.world.isBusyWithUI() || !InputHelpers.mouseSupported || !Zones.showMapBorders())
      return;
    WorldTile tilePosCachedFrame = World.world.getMouseTilePosCachedFrame();
    if (tilePosCachedFrame == null)
      return;
    MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
    if (cachedMapMetaAsset == null)
      return;
    cachedMapMetaAsset.check_cursor_highlight(cachedMapMetaAsset, tilePosCachedFrame, pAsset);
  }

  public static void colorEnemies(QuantumSpriteAsset pAsset, Kingdom pKingdom)
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.isEnemy(pKingdom))
      {
        foreach (City city in kingdom.getCities())
        {
          Color color2 = pAsset.color_2;
          color2.a = (float) (0.10000000149011612 + (double) QuantumSpriteManager.highlight_animation / 30.0);
          QuantumSpriteLibrary.colorZones(pAsset, city.zones, color2);
        }
      }
    }
  }

  public static void colorZones(QuantumSpriteAsset pAsset, List<TileZone> pZones, Color pColor)
  {
    for (int index = 0; index < pZones.Count; ++index)
    {
      TileZone pZone = pZones[index];
      if (pZone.visible)
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pZone.centerTile.posV).setColor(ref pColor);
    }
  }

  public static void colorZones(QuantumSpriteAsset pAsset, ListPool<TileZone> pZones, Color pColor)
  {
    for (int index = 0; index < pZones.Count; ++index)
    {
      TileZone pZone = pZones[index];
      if (pZone.visible)
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pZone.centerTile.posV).setColor(ref pColor);
    }
  }

  private static void drawArrowsArmyAttackTargets(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_armies") || !PlayerConfig.optionBoolEnabled("army_targets"))
      return;
    WorldTile tilePosCachedFrame = World.world.getMouseTilePosCachedFrame();
    City city1 = (City) null;
    if (tilePosCachedFrame != null && DebugConfig.isOn(DebugOption.ArrowsOnlyForCursorCities))
      city1 = tilePosCachedFrame.zone.city;
    foreach (City city2 in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city2.target_attack_city != null && (!Zones.showCityZones() || city1 == null || city2 == city1) && city2.hasArmy() && city2.army.hasCaptain())
      {
        Actor captain = city2.army.getCaptain();
        WorldTile currentTile = captain.current_tile;
        WorldTile behTileTarget = captain.beh_tile_target;
        if (currentTile != null && behTileTarget != null)
        {
          Color colorMainSecond = city2.kingdom.getColor().getColorMainSecond();
          QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, currentTile.posV3, behTileTarget.posV3, ref colorMainSecond, city2);
        }
      }
    }
  }

  private static void drawWarsIcons(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_wars"))
      return;
    QuantumSpriteLibrary.drawWarIconInList(QuantumSpriteLibrary._wars_pos_sword_main, "ui/Icons/iconAttack", pAsset, 0.2f);
    QuantumSpriteLibrary.drawWarIconInList(QuantumSpriteLibrary._wars_pos_shields_main, "ui/Icons/iconShield", pAsset, 0.2f);
  }

  private static void drawWarIconInList(
    List<Vector3> pList,
    string pPath,
    QuantumSpriteAsset pAsset,
    float pSize)
  {
    if (pList.Count == 0)
      return;
    foreach (Vector3 p in pList)
    {
      float num = (float) ((double) pSize * (double) p.z * 1.5);
      pAsset.base_scale = num;
      QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, p);
      quantumSprite.setSprite(SpriteTextureLoader.getSprite(pPath));
      ((Renderer) quantumSprite.sprite_renderer).sortingOrder = 1;
    }
  }

  private static void drawProjectileShadows(QuantumSpriteAsset pAsset)
  {
    if (!Config.shadows_active)
      return;
    foreach (Projectile projectile in World.world.projectiles.list)
    {
      ProjectileAsset asset = projectile.asset;
      if (!string.IsNullOrEmpty(asset.texture_shadow))
      {
        Vector3 pPosition = Vector2.op_Implicit(projectile.getCurrentPosition());
        float angleForShadow = projectile.getAngleForShadow();
        QuantumSprite next = pAsset.group_system.getNext();
        next.setSprite(SpriteTextureLoader.getSprite(asset.texture_shadow));
        next.set(ref pPosition, projectile.getCurrentScale());
        ((Component) next).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angleForShadow));
      }
    }
  }

  private static void drawProjectiles(QuantumSpriteAsset pAsset)
  {
    foreach (Projectile projectile in World.world.projectiles.list)
    {
      ProjectileAsset asset = projectile.asset;
      Color pColor;
      // ISSUE: explicit constructor call
      ((Color) ref pColor).\u002Ector(1f, 1f, 1f, projectile.getAlpha());
      Vector3 pPosition = Vector2.op_Implicit(projectile.getTransformedPositionWithHeight());
      pPosition.z = projectile.getCurrentHeight();
      QuantumSprite next = pAsset.group_system.getNext();
      if (asset.animated)
      {
        Sprite spriteFromList = AnimationHelper.getSpriteFromList(projectile.GetHashCode(), (IList<Sprite>) asset.frames, asset.animation_speed);
        next.setSprite(spriteFromList);
      }
      else
      {
        Sprite frame = asset.frames[0];
        next.setSprite(frame);
      }
      next.set(ref pPosition, projectile.getCurrentScale());
      ((Component) next).transform.rotation = projectile.rotation;
      next.setColor(ref pColor);
    }
  }

  private static void drawThrowingItemsShadows(QuantumSpriteAsset pAsset)
  {
    if (!Config.shadows_active)
      return;
    List<ResourceThrowData> list = World.world.resource_throw_manager.getList();
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(list.Count);
    for (int index = 0; index < list.Count; ++index)
    {
      ResourceThrowData resourceThrowData = list[index];
      QuantumSprite quantumSprite = fastActiveList[index];
      float ratio = resourceThrowData.getRatio();
      Vector3 pPosition = Vector2.op_Implicit(Vector2.Lerp(resourceThrowData.position_start, resourceThrowData.position_end, ratio));
      pPosition.z = 4f;
      float pScale = 0.1f;
      quantumSprite.setSprite(AssetManager.resources.get(resourceThrowData.resource_asset_id).getGameplaySprite());
      quantumSprite.set(ref pPosition, pScale);
      ((Component) quantumSprite).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, ratio * 360f));
    }
  }

  private static void drawShadowsBuildings(QuantumSpriteAsset pAsset)
  {
    if (!World.world.quality_changer.shouldRenderBuildingShadows())
      return;
    int pTargetSize = World.world.buildings.countVisibleBuildings();
    if (pTargetSize == 0)
      return;
    BuildingRenderData renderData = World.world.buildings.render_data;
    bool[] shadows = renderData.shadows;
    Vector3[] positions1 = renderData.positions;
    Vector3[] scales1 = renderData.scales;
    Sprite[] shadowSprites = renderData.shadow_sprites;
    if (QuantumSpriteLibrary._q_render_indexes_shadows_buildings.Length < pTargetSize)
      QuantumSpriteLibrary._q_render_indexes_shadows_buildings = Toolbox.checkArraySize<int>(QuantumSpriteLibrary._q_render_indexes_shadows_buildings, pTargetSize);
    int[] shadowsBuildings = QuantumSpriteLibrary._q_render_indexes_shadows_buildings;
    int num = 0;
    for (int index = 0; index < pTargetSize; ++index)
    {
      if (shadows[index])
        shadowsBuildings[num++] = index;
    }
    if (num == 0)
      return;
    QuantumSpriteCacheData cacheData = pAsset.group_system.getCacheData(num);
    Vector3[] positions2 = cacheData.positions;
    Vector3[] scales2 = cacheData.scales;
    Sprite[] sprites = cacheData.sprites;
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(num);
    for (int index1 = 0; index1 < num; ++index1)
    {
      QuantumSprite quantumSprite = fastActiveList[index1];
      int index2 = shadowsBuildings[index1];
      ref Vector3 local1 = ref positions1[index2];
      ref Vector3 local2 = ref positions2[index1];
      if ((double) local1.x != (double) local2.x || (double) local1.y != (double) local2.y || (double) local1.z != (double) local2.z)
      {
        local2 = local1;
        quantumSprite.m_transform.position = local1;
      }
      ref Vector3 local3 = ref scales1[index2];
      ref Vector3 local4 = ref scales2[index1];
      if ((double) local3.x != (double) local4.x || (double) local3.y != (double) local4.y || (double) local3.z != (double) local4.z)
      {
        local4 = local3;
        quantumSprite.m_transform.localScale = local3;
      }
      Sprite sprite = shadowSprites[index2];
      if (sprites[index1] != sprite)
      {
        sprites[index1] = sprite;
        quantumSprite.sprite_renderer.sprite = sprite;
      }
    }
  }

  private static void drawShadowsUnit(QuantumSpriteAsset pAsset)
  {
    if (!World.world.quality_changer.shouldRenderUnitShadows())
      return;
    ActorRenderData renderData = World.world.units.render_data;
    int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
    if (visibleUnitsCount == 0)
      return;
    bool[] shadows = renderData.shadows;
    Vector3[] shadowPosition = renderData.shadow_position;
    Vector3[] shadowScales1 = renderData.shadow_scales;
    Sprite[] shadowSprites = renderData.shadow_sprites;
    if (QuantumSpriteLibrary._q_render_indexes_shadows_units.Length < visibleUnitsCount)
      QuantumSpriteLibrary._q_render_indexes_shadows_units = Toolbox.checkArraySize<int>(QuantumSpriteLibrary._q_render_indexes_shadows_units, visibleUnitsCount);
    int[] indexesShadowsUnits = QuantumSpriteLibrary._q_render_indexes_shadows_units;
    int num = 0;
    for (int index = 0; index < visibleUnitsCount; ++index)
    {
      if (shadows[index])
        indexesShadowsUnits[num++] = index;
    }
    if (num == 0)
      return;
    QuantumSprite[] fastActiveList = pAsset.group_system.getFastActiveList(num);
    QuantumSpriteCacheData cacheData = pAsset.group_system.getCacheData(num);
    Vector3[] positions = cacheData.positions;
    Vector3[] shadowScales2 = cacheData.shadow_scales;
    Sprite[] sprites = cacheData.sprites;
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = indexesShadowsUnits[index1];
      ref Vector3 local1 = ref shadowPosition[index2];
      ref Vector3 local2 = ref positions[index1];
      if ((double) local1.x != (double) local2.x || (double) local1.y != (double) local2.y || (double) local1.z != (double) local2.z)
      {
        local2 = local1;
        fastActiveList[index1].m_transform.position = local1;
      }
      ref Vector3 local3 = ref shadowScales1[index2];
      ref Vector3 local4 = ref shadowScales2[index1];
      if ((double) local3.x != (double) local4.x || (double) local3.y != (double) local4.y || (double) local3.z != (double) local4.z)
      {
        local4 = local3;
        fastActiveList[index1].m_transform.localScale = local3;
      }
      Sprite sprite = shadowSprites[index2];
      if (sprites[index1] != sprite)
      {
        sprites[index1] = sprite;
        fastActiveList[index1].sprite_renderer.sprite = sprite;
      }
    }
  }

  private static void drawUnitBanners(QuantumSpriteAsset pAsset)
  {
    Actor[] array = World.world.units.visible_units_with_banner.array;
    int count = World.world.units.visible_units_with_banner.count;
    for (int index = 0; index < count; ++index)
    {
      Actor pSimObject = array[index];
      Vector3 positionForFunRendering = pSimObject.getHeadOffsetPositionForFunRendering();
      QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, positionForFunRendering, pForceScaleTo: pSimObject.current_scale.y);
      Color colorText = pSimObject.kingdom.getColor().getColorText();
      quantumSprite.setColor(ref colorText);
      quantumSprite.checkRotation(positionForFunRendering, (BaseSimObject) pSimObject, -0.01f);
    }
  }

  private static void drawFavoriteItemsMap(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_favorite_items"))
      return;
    foreach (Item obj in (CoreSystemManager<Item, ItemData>) World.world.items)
    {
      if (obj.isFavorite())
      {
        Actor actor = obj.getActor();
        if (!actor.isRekt() && actor.current_zone.visible)
        {
          Vector3 pPos = Vector2.op_Implicit(actor.current_position);
          ++pPos.y;
          QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pKingdom: actor.kingdom, pCity: actor.city).setSprite(obj.getSprite());
        }
      }
    }
  }

  private static void drawFavoritesMap(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_favorites"))
      return;
    Actor[] array = World.world.units.visible_units_with_favorite.array;
    int count = World.world.units.visible_units_with_favorite.count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      Vector3 pPos = Vector2.op_Implicit(actor.current_position);
      pPos.y -= 3f;
      QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pKingdom: actor.kingdom, pCity: actor.city);
    }
  }

  private static void drawUnitsToBeSelectedBySquareTool(QuantumSpriteAsset pAsset)
  {
    // ISSUE: unable to decompile the method.
  }

  private static void drawSelectedUnits(QuantumSpriteAsset pAsset)
  {
    if (!SelectedUnit.isSet())
      return;
    Sprite fromListSessionTime1 = AnimationHelper.getSpriteFromListSessionTime(0, (IList<Sprite>) QuantumSpriteLibrary._unit_selection_effect, 10f);
    Sprite fromListSessionTime2 = AnimationHelper.getSpriteFromListSessionTime(0, (IList<Sprite>) QuantumSpriteLibrary._unit_selection_effect_main, 10f);
    Color architectColor1 = World.world.getArchitectColor();
    architectColor1.a = 0.8f;
    Color architectColor2 = World.world.getArchitectColor();
    foreach (Actor pActor in SelectedUnit.getAllSelected())
    {
      Vector3 pPos = Vector2.op_Implicit(pActor.current_position);
      float y = pActor.current_scale.y;
      if (SelectedUnit.isMainSelected(pActor))
      {
        QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pForceScaleTo: y * 1.1f);
        quantumSprite.setSprite(fromListSessionTime2);
        quantumSprite.setColor(ref architectColor2);
      }
      else
      {
        QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pForceScaleTo: y);
        quantumSprite.setSprite(fromListSessionTime1);
        quantumSprite.setColor(ref architectColor1);
      }
    }
  }

  private static void drawFavoritesGame(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_favorites"))
      return;
    float num1 = 20f;
    if (PlayerConfig.optionBoolEnabled("icons_tasks"))
      num1 += 11.5f;
    if (PlayerConfig.optionBoolEnabled("icons_happiness"))
      num1 += 11.5f;
    Actor[] array = World.world.units.visible_units_with_favorite.array;
    int count = World.world.units.visible_units_with_favorite.count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      if (!actor.isInMagnet())
      {
        actor.updatePos();
        float x = actor.cur_transform_position.x;
        float num2 = actor.cur_transform_position.y + num1 * actor.current_scale.y;
        Vector3 pPos;
        // ISSUE: explicit constructor call
        ((Vector3) ref pPos).\u002Ector(x, num2);
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pForceScaleTo: actor.current_scale.y);
      }
    }
  }

  private static void drawStatusEffects(QuantumSpriteAsset pAsset)
  {
    Actor[] array = World.world.units.visible_units_with_status.array;
    int count = World.world.units.visible_units_with_status.count;
    for (int index = 0; index < count; ++index)
      QuantumSpriteLibrary.drawStatusEffectFor((BaseSimObject) array[index], pAsset);
    int num = World.world.buildings.countVisibleBuildings();
    Building[] visibleBuildings = World.world.buildings.getVisibleBuildings();
    for (int index = 0; index < num; ++index)
    {
      Building pSimObject = visibleBuildings[index];
      if (pSimObject.hasAnyStatusEffectToRender())
        QuantumSpriteLibrary.drawStatusEffectFor((BaseSimObject) pSimObject, pAsset);
    }
  }

  private static void drawStatusEffectFor(BaseSimObject pSimObject, QuantumSpriteAsset pAsset)
  {
    foreach (Status statuse in pSimObject.getStatuses())
    {
      StatusAsset asset = statuse.asset;
      if (asset.need_visual_render)
      {
        Vector3 pPosition = pSimObject.cur_transform_position;
        if (pSimObject.isActor())
        {
          pPosition.x += asset.offset_x * pSimObject.a.getScaleMod();
          pPosition.y += asset.offset_y * pSimObject.a.getScaleMod();
        }
        if (asset.has_override_sprite_position)
        {
          Vector3 vector3 = asset.get_override_sprite_position(pSimObject, statuse.anim_frame);
          pPosition = Vector3.op_Addition(pPosition, vector3);
        }
        if (!pSimObject.isActor() || statuse.asset.render_check(pSimObject.a.asset))
        {
          QuantumSprite next = pAsset.group_system.getNext();
          next.setScale(pSimObject.current_scale.y * asset.scale);
          Sprite pSprite = !asset.has_override_sprite ? asset.sprite_list[statuse.anim_frame] : asset.get_override_sprite(pSimObject, statuse.anim_frame);
          next.setSprite(pSprite);
          next.setPosOnly(ref pPosition);
          if (asset.use_parent_rotation)
          {
            next.setFlipX(false);
            next.checkRotation(pPosition, pSimObject, asset.position_z);
          }
          else
          {
            if (pSimObject.isActor() && asset.can_be_flipped)
              next.setFlipX(pSimObject.a.flip);
            else
              next.setFlipX(false);
            Vector3 pVec;
            // ISSUE: explicit constructor call
            ((Vector3) ref pVec).\u002Ector(0.0f, 0.0f, 0.0f);
            next.setRotation(ref pVec);
          }
          if ((double) asset.rotation_z != 0.0)
          {
            Vector3 currentRotation = pSimObject.current_rotation;
            if (asset.has_override_sprite_rotation_z)
              currentRotation.z += asset.get_override_sprite_rotation_z(pSimObject, statuse.anim_frame);
            else
              currentRotation.z += asset.rotation_z;
            next.setRotation(ref currentRotation);
          }
          next.setSharedMat(asset.material);
        }
      }
    }
  }

  private static void drawWars(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_wars"))
      return;
    QuantumSpriteLibrary._wars_pos_sword_main.Clear();
    QuantumSpriteLibrary._wars_pos_shields_main.Clear();
    if (World.world.wars.Count == 0)
      return;
    Kingdom pKingdom = (Kingdom) null;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.isCursorOver())
      {
        pKingdom = kingdom;
        break;
      }
    }
    float num = 1f;
    foreach (War war in (CoreSystemManager<War, WarData>) World.world.wars)
    {
      bool flag = false;
      if (!war.hasEnded() && !war.isTotalWar())
      {
        if (pKingdom != null && war.hasKingdom(pKingdom))
          flag = true;
        if (pKingdom != null)
        {
          if (flag)
          {
            pAsset.base_scale = 1f;
            num = 1f;
          }
          else
          {
            pAsset.base_scale = 0.2f;
            num = 0.1f;
          }
        }
        else
          pAsset.base_scale = 0.5f;
        Kingdom mainAttacker = war.main_attacker;
        Kingdom mainDefender = war.main_defender;
        if (!mainAttacker.isRekt() && !mainDefender.isRekt() && mainAttacker.hasCapital() && mainDefender.hasCapital() && mainAttacker.capital.isValidTargetForWar() && mainDefender.capital.isValidTargetForWar())
        {
          Vector3 pStart = Vector2.op_Implicit(mainAttacker.capital.city_center);
          Vector3 pEnd = Vector2.op_Implicit(mainDefender.capital.city_center);
          pStart.y -= 20f;
          pEnd.y -= 20f;
          pStart.z = pAsset.base_scale;
          pEnd.z = pAsset.base_scale;
          QuantumSpriteLibrary._wars_pos_sword_main.Add(pStart);
          QuantumSpriteLibrary._wars_pos_shields_main.Add(pEnd);
          pAsset.base_scale *= 0.6f;
          QuantumSpriteArrows quantumSpriteArrows = QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref Toolbox.color_white);
          Color colorMainSecond = mainAttacker.getColor().getColorMainSecond();
          colorMainSecond.a = num;
          if (Object.op_Inequality((Object) quantumSpriteArrows, (Object) null))
          {
            quantumSpriteArrows.spriteArrowMiddle.color = colorMainSecond;
            ((Renderer) quantumSpriteArrows.spriteArrowMiddle).sortingOrder = -1;
          }
        }
      }
    }
  }

  private static void drawPlots(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_plots"))
      return;
    foreach (Plot plot in (CoreSystemManager<Plot, PlotData>) World.world.plots)
    {
      if (plot.isActive())
        QuantumSpriteLibrary.drawPlotIcon(pAsset, plot);
    }
  }

  private static void drawPlotIcon(QuantumSpriteAsset pAsset, Plot pPlot)
  {
    foreach (Actor unit in pPlot.units)
    {
      if (!unit.isRekt() && unit.current_zone.visible)
      {
        Vector3 pPos = Vector2.op_Implicit(unit.current_position);
        City city = unit.city;
        float num = 5.5f * QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset);
        if (city != null)
          num *= city.mark_scale_effect;
        pPos.y += num;
        QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pCity: city, pModScale: pPlot.transition_animation);
        Sprite sprite = pPlot.getSprite();
        quantumSprite.setSprite(sprite);
        CircleIconShaderMod component = ((Component) quantumSprite).GetComponent<CircleIconShaderMod>();
        component.sprite_renderer_with_mat.sprite = sprite;
        component.setShaderVal(pPlot.getProgressMod());
      }
    }
  }

  private static void drawPlotRemovals(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_plots"))
      return;
    List<PlotIconData> plotRemovals = World.world.stack_effects.plot_removals;
    if (plotRemovals.Count <= 0)
      return;
    for (int index = plotRemovals.Count - 1; index >= 0; --index)
    {
      PlotIconData plotIconData = plotRemovals[index];
      Actor actor = plotIconData.actor;
      float timeElapsedSince = World.world.getRealTimeElapsedSince(plotIconData.timestamp);
      if ((double) timeElapsedSince > 1.0 || !actor.isAlive())
      {
        plotRemovals.RemoveAt(index);
      }
      else
      {
        Vector3 pPos = Vector2.op_Implicit(actor.current_position);
        City city = actor.city;
        float num = 5.5f * QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset);
        if (city != null)
          num *= city.mark_scale_effect;
        pPos.y += num;
        float pModScale = Mathf.Lerp(1.3f, 0.0f, timeElapsedSince / 1f);
        QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pCity: city, pModScale: pModScale);
        Sprite sprite = SpriteTextureLoader.getSprite(plotIconData.sprite);
        quantumSprite.setSprite(sprite);
        CircleIconShaderMod component = ((Component) quantumSprite).GetComponent<CircleIconShaderMod>();
        component.sprite_renderer_with_mat.sprite = sprite;
        component.setShaderVal(1f);
      }
    }
  }

  private static void drawKings(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("map_kings_leaders"))
      return;
    int num = 0;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (num > 2)
        break;
      Actor king = kingdom.king;
      if (!king.isRekt() && !king.isInMagnet() && king.current_zone.visible)
      {
        Vector3 pPos = Vector2.op_Implicit(king.current_position);
        pPos.y -= 3f;
        Sprite pSprite = !king.has_attack_target ? (!king.hasPlot() ? (kingdom.hasEnemies() ? QuantumSpriteLibrary._king_sprite_normal : QuantumSpriteLibrary._king_sprite_happy) : QuantumSpriteLibrary._king_sprite_surprised) : QuantumSpriteLibrary._king_sprite_angry;
        if (!pAsset.group_system.is_within_active_index)
          ++num;
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pKingdom: kingdom, pCity: king.city).setSprite(DynamicSprites.getIcon(pSprite, kingdom.getColor()));
      }
    }
  }

  private static void drawLeaders(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("map_kings_leaders"))
      return;
    int num = 0;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (num > 2)
        break;
      foreach (City city in kingdom.getCities())
      {
        Actor leader = city.leader;
        if (!leader.isRekt() && !leader.isInMagnet() && !leader.isKing() && leader.current_zone.visible)
        {
          Vector3 pPos = Vector2.op_Implicit(leader.current_position);
          pPos.y -= 3f;
          Sprite pSprite = !leader.has_attack_target ? (!leader.hasPlot() ? (!kingdom.hasEnemies() ? (!leader.isHappy() ? QuantumSpriteLibrary._leader_sprite_sad : QuantumSpriteLibrary._leader_sprite_happy) : QuantumSpriteLibrary._leader_sprite_normal) : QuantumSpriteLibrary._leader_sprite_surprised) : QuantumSpriteLibrary._leader_sprite_angry;
          if (!pAsset.group_system.is_within_active_index)
            ++num;
          QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos, pKingdom: kingdom, pCity: city).setSprite(DynamicSprites.getIcon(pSprite, kingdom.getColor()));
        }
      }
    }
  }

  private static void drawBattles(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_battles"))
      return;
    HashSet<BattleContainer> battleContainerSet = BattleKeeperManager.get();
    if (battleContainerSet.Count == 0)
      return;
    foreach (BattleContainer pBattle in battleContainerSet)
    {
      if (pBattle.isRendered())
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pBattle.tile, (WorldTile) null, pBattle: pBattle).setSprite(SpriteTextureLoader.getSpriteList(pAsset.path_icon)[pBattle.frame]);
    }
  }

  private static void drawBoatIcons(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_boats"))
      return;
    foreach (ActorAsset listOnlyBoatAsset in AssetManager.actor_library.list_only_boat_assets)
      QuantumSpriteLibrary.drawBoatIcons(pAsset, listOnlyBoatAsset.id);
  }

  private static void drawBoatIcons(QuantumSpriteAsset pAsset, string pActorAssetID)
  {
    HashSet<Actor> units = AssetManager.actor_library.get(pActorAssetID).units;
    if (units.Count == 0)
      return;
    int num = 0;
    foreach (Actor pObject in units)
    {
      if (num > 2)
        break;
      if (!pObject.isRekt() && pObject.current_zone.visible && pObject.asset.draw_boat_mark && pObject.isKingdomCiv() && (!(pAsset.id == "boats_big") || pObject.asset.draw_boat_mark_big) && (!(pAsset.id == "boats_small") || !pObject.asset.draw_boat_mark_big) && !pObject.isInMagnet())
      {
        ColorAsset color = pObject.kingdom.getColor();
        if (!pAsset.group_system.is_within_active_index)
          ++num;
        QuantumSprite quantumSprite;
        if (color != null)
        {
          QuantumSpriteAsset pAsset1 = pAsset;
          Vector3 pPos = Vector2.op_Implicit(pObject.current_position);
          City city = pObject.city;
          Kingdom kingdom = pObject.kingdom;
          City pCity = city;
          quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset1, pPos, pKingdom: kingdom, pCity: pCity);
        }
        else
        {
          QuantumSpriteAsset pAsset2 = pAsset;
          Vector3 pPos = Vector2.op_Implicit(pObject.current_position);
          City city = pObject.city;
          Kingdom kingdom = pObject.kingdom;
          City pCity = city;
          quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset2, pPos, pKingdom: kingdom, pCity: pCity);
        }
        Sprite pSprite = !pObject.asset.draw_boat_mark_big ? DynamicSprites.getIcon(QuantumSpriteLibrary._boat_sprite_small, pObject.kingdom.getColor()) : DynamicSprites.getIcon(QuantumSpriteLibrary._boat_sprite_big, pObject.kingdom.getColor());
        quantumSprite.setSprite(pSprite);
      }
    }
  }

  private static void drawMagnetUnits(QuantumSpriteAsset pAsset)
  {
    if (!World.world.magnet.hasUnits())
      return;
    List<Actor> magnetUnits = World.world.magnet.magnet_units;
    for (int index = 0; index < magnetUnits.Count; ++index)
    {
      Actor pObject = magnetUnits[index];
      if (!pObject.isRekt())
      {
        QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(pObject.current_position), pKingdom: pObject.kingdom);
        quantumSprite.setScale(pObject.current_scale.y);
        ((Component) quantumSprite).transform.rotation = Quaternion.Euler(0.0f, 0.0f, World.world.magnet.moving_angle);
        quantumSprite.setSprite(pObject.getSpriteToRender());
      }
    }
  }

  private static void drawArmies(QuantumSpriteAsset pAsset)
  {
    if (!PlayerConfig.optionBoolEnabled("marks_armies"))
      return;
    int num = 0;
    if ((!Zones.showArmyZones() ? 0 : (Zones.showMapNames() ? 1 : 0)) != 0)
      return;
    for (int index = 0; index < World.world.armies.list.Count && num <= 2; ++index)
    {
      Army army = World.world.armies.list[index];
      if (army.hasCaptain())
      {
        Actor captain = army.getCaptain();
        if (!captain.isInMagnet() && captain.current_zone.visible && captain.isKingdomCiv())
        {
          Kingdom kingdom = captain.kingdom;
          QuantumSpriteWithText quantumSpriteWithText = (QuantumSpriteWithText) QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(captain.current_position), pKingdom: kingdom, pCity: captain.city);
          if (DebugConfig.isOn(DebugOption.ShowAmountNearArmy))
          {
            ((Component) quantumSpriteWithText.text).gameObject.SetActive(true);
            quantumSpriteWithText.text.text = army.countUnits().ToString() ?? "";
            ((Component) quantumSpriteWithText.text).GetComponent<Renderer>().sortingLayerID = ((Renderer) quantumSpriteWithText.sprite_renderer).sortingLayerID;
            ((Component) quantumSpriteWithText.text).GetComponent<Renderer>().sortingOrder = ((Renderer) quantumSpriteWithText.sprite_renderer).sortingOrder;
          }
          else
            ((Component) quantumSpriteWithText.text).gameObject.SetActive(false);
          if (!pAsset.group_system.is_within_active_index)
            ++num;
          Sprite icon = DynamicSprites.getIcon(QuantumSpriteLibrary._flag_sprite, kingdom.getColor());
          quantumSpriteWithText.setSprite(icon);
        }
      }
    }
  }

  private static QuantumSpriteArrows drawArrowQuantumSprite(
    QuantumSpriteAsset pAsset,
    Vector3 pStart,
    Vector3 pEnd,
    ref Color pColor,
    City pCity = null)
  {
    if ((double) pStart.x == (double) pEnd.x && (double) pStart.y == (double) pEnd.y)
      return (QuantumSpriteArrows) null;
    float num1 = Toolbox.Dist(pStart.x, pStart.y, pEnd.x, pEnd.y);
    float num2 = pAsset.base_scale * QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset);
    if (pCity != null)
      num2 *= pCity.mark_scale_effect;
    float num3 = num1 / num2;
    if ((double) num3 < (double) pAsset.line_width)
      return (QuantumSpriteArrows) null;
    float num4 = QuantumSpriteManager.arrow_middle_current;
    if (!pAsset.arrow_animation)
      num4 = 0.0f;
    QuantumSpriteArrows next = (QuantumSpriteArrows) pAsset.group_system.getNext();
    ((Renderer) next.spriteArrowEnd).enabled = pAsset.render_arrow_end;
    ((Renderer) next.spriteArrowStart).enabled = pAsset.render_arrow_start;
    if ((double) num3 < (double) (pAsset.line_width + 2))
      ((Renderer) next.spriteArrowEnd).enabled = false;
    if (((Renderer) next.spriteArrowEnd).enabled)
    {
      next.spriteArrowEnd.color = pColor;
      ((Component) next.spriteArrowEnd).transform.localPosition = new Vector3(num3, 0.0f, 0.0f);
    }
    if (((Renderer) next.spriteArrowStart).enabled)
      next.spriteArrowStart.color = pColor;
    next.spriteArrowMiddle.color = pColor;
    Vector3 vector3 = pStart;
    vector3.z = (float) pAsset.group_system.countActive() * (1f / 1000f);
    ((Component) next).transform.position = vector3;
    float angleDegrees = Toolbox.getAngleDegrees(pStart.x, pStart.y, pEnd.x, pEnd.y);
    ((Component) next).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angleDegrees));
    float num5 = num3 - num4;
    if (((Renderer) next.spriteArrowEnd).enabled)
      num5 -= 5f;
    next.spriteArrowMiddle.size = new Vector2(num5, (float) pAsset.line_height);
    ((Component) next.spriteArrowMiddle).transform.localPosition = new Vector3(num4, 0.0f, 0.0f);
    ((Component) next).transform.localScale = new Vector3(num2, num2, 1f);
    return next;
  }

  private static QuantumSprite drawQuantumSprite(
    QuantumSpriteAsset pAsset,
    Vector3 pPos,
    WorldTile pTileTarget = null,
    Kingdom pKingdom = null,
    City pCity = null,
    BattleContainer pBattle = null,
    float pModScale = 1f,
    bool pSetColor = false,
    float pForceScaleTo = -1f)
  {
    QuantumSprite next = pAsset.group_system.getNext();
    if (pSetColor)
      next.setColor(ref Toolbox.color_white);
    float pScale;
    if ((double) pForceScaleTo == -1.0)
    {
      pScale = pAsset.base_scale * pModScale;
      if (pAsset.flag_battle)
        pScale = (float) ((double) pScale * (double) pBattle.timer * 0.20000000298023224);
      if (pAsset.add_camera_zoom_multiplier)
        pScale *= QuantumSpriteLibrary.getCameraScaleZoomMultiplier(pAsset);
      if (pAsset.selected_city_scale)
      {
        if (pCity != null)
          pScale *= pCity.mark_scale_effect;
        else
          pScale *= 0.5f;
      }
    }
    else
      pScale = pForceScaleTo;
    next.set(ref pPos, pScale);
    return next;
  }

  private static QuantumSprite drawQuantumSprite(
    QuantumSpriteAsset pAsset,
    WorldTile pTile,
    WorldTile pTileTarget,
    Kingdom pKingdom = null,
    City pCity = null,
    BattleContainer pBattle = null)
  {
    return pTile == null ? (QuantumSprite) null : QuantumSpriteLibrary.drawQuantumSprite(pAsset, pTile.posV3, pTileTarget, pKingdom, pCity, pBattle);
  }

  private static float getCameraScaleZoomMultiplier(QuantumSpriteAsset pAsset)
  {
    return Mathf.Clamp(MoveCamera.instance.main_camera.orthographicSize / 30f, (float) pAsset.add_camera_zoom_multiplier_min, (float) pAsset.add_camera_zoom_multiplier_max);
  }

  private static Actor[] visible_units => World.world.units.visible_units.array;

  private static int visible_units_count => World.world.units.visible_units.count;

  private static Actor[] visible_units_alive => World.world.units.visible_units_alive.array;

  private static int visible_units_alive_count => World.world.units.visible_units_alive.count;

  public void initDebugQuantumSpriteAssets()
  {
    QuantumSpriteAsset pAsset1 = new QuantumSpriteAsset();
    pAsset1.id = "draw_money";
    pAsset1.id_prefab = "p_mapSprite";
    pAsset1.add_camera_zoom_multiplier = false;
    pAsset1.debug_option = DebugOption.ShowMoneyIcons;
    pAsset1.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawMoney);
    pAsset1.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsTop");
      pQSprite.sprite_renderer.sprite = SpriteTextureLoader.getSprite("ui/Icons/iconResGold");
    });
    pAsset1.render_gameplay = true;
    pAsset1.default_amount = 10;
    this.add(pAsset1);
    QuantumSpriteAsset pAsset2 = new QuantumSpriteAsset();
    pAsset2.id = "debug_arrows_settlers";
    pAsset2.id_prefab = "p_mapArrow_stroke";
    pAsset2.render_map = true;
    pAsset2.arrow_animation = true;
    pAsset2.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsSettlers);
    pAsset2.debug_option = DebugOption.CivDrawSettleTarget;
    this.add(pAsset2);
    QuantumSpriteAsset pAsset3 = new QuantumSpriteAsset();
    pAsset3.id = "debug_arrows_land_claim";
    pAsset3.id_prefab = "p_mapArrow_stroke";
    pAsset3.render_map = true;
    pAsset3.arrow_animation = true;
    pAsset3.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawClaimZone);
    pAsset3.debug_option = DebugOption.CivDrawCityClaimZone;
    this.add(pAsset3);
    QuantumSpriteAsset pAsset4 = new QuantumSpriteAsset();
    pAsset4.base_scale = 0.35f;
    pAsset4.id = "debug_kingdom_attack_targets";
    pAsset4.id_prefab = "p_mapArrow_stroke";
    pAsset4.render_arrow_end = true;
    pAsset4.render_arrow_start = true;
    pAsset4.arrow_animation = true;
    pAsset4.render_map = true;
    pAsset4.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsKingdomAttackTarget);
    pAsset4.debug_option = DebugOption.KingdomDrawAttackTarget;
    this.add(pAsset4);
    QuantumSpriteAsset pAsset5 = new QuantumSpriteAsset();
    pAsset5.id = "debug_unit_attack_range";
    pAsset5.id_prefab = "p_mapSprite";
    pAsset5.base_scale = 0.1f;
    pAsset5.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitAttackRange);
    pAsset5.debug_option = DebugOption.CursorUnitAttackRange;
    pAsset5.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) =>
    {
      Sprite sprite = SpriteTextureLoader.getSprite("ui/Icons/iconWhiteCircle");
      pQSprite.setSprite(sprite);
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 10;
      pQSprite.setColor(ref pAsset.color);
    });
    pAsset5.render_gameplay = true;
    pAsset5.color = new Color(1f, 1f, 1f, 0.3f);
    this.add(pAsset5);
    QuantumSpriteAsset pAsset6 = new QuantumSpriteAsset();
    pAsset6.id = "debug_unit_attack_size";
    pAsset6.id_prefab = "p_mapSprite";
    pAsset6.base_scale = 0.1f;
    pAsset6.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawUnitSize);
    pAsset6.debug_option = DebugOption.CursorUnitSize;
    pAsset6.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) =>
    {
      Sprite sprite = SpriteTextureLoader.getSprite("ui/Icons/iconWhiteCircle");
      pQSprite.setSprite(sprite);
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 10;
      pQSprite.setColor(ref pAsset.color);
    });
    pAsset6.render_gameplay = true;
    pAsset6.color = new Color(0.2f, 0.2f, 1f, 0.4f);
    this.add(pAsset6);
    QuantumSpriteAsset pAsset7 = new QuantumSpriteAsset();
    pAsset7.id = "debug_arrows_units_attack_targets";
    pAsset7.id_prefab = "p_mapArrow_stroke";
    pAsset7.base_scale = 0.1f;
    pAsset7.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitAttackTargets);
    pAsset7.debug_option = DebugOption.ArrowsUnitsAttackTargets;
    pAsset7.arrow_animation = true;
    pAsset7.render_gameplay = true;
    pAsset7.color = new Color(1f, 0.0f, 0.0f, 0.7f);
    this.add(pAsset7);
    QuantumSpriteAsset pAsset8 = new QuantumSpriteAsset();
    pAsset8.id = "debug_arrows_units_actor_targets";
    pAsset8.id_prefab = "p_mapArrow_stroke";
    pAsset8.base_scale = 0.1f;
    pAsset8.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitBehTarget);
    pAsset8.debug_option = DebugOption.ArrowUnitsBehActorTarget;
    pAsset8.arrow_animation = true;
    pAsset8.render_gameplay = true;
    pAsset8.color = new Color(1f, 1f, 0.0f, 0.7f);
    this.add(pAsset8);
    QuantumSpriteAsset pAsset9 = new QuantumSpriteAsset();
    pAsset9.id = "debug_arrows_units_navigation_targets";
    pAsset9.id_prefab = "p_mapArrow_stroke";
    pAsset9.base_scale = 0.1f;
    pAsset9.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitNavigationTargets);
    pAsset9.debug_option = DebugOption.ArrowsUnitsNavigationTargets;
    pAsset9.arrow_animation = true;
    pAsset9.render_gameplay = true;
    pAsset9.color = new Color(0.9f, 0.9f, 0.9f, 0.5f);
    this.add(pAsset9);
    QuantumSpriteAsset pAsset10 = new QuantumSpriteAsset();
    pAsset10.id = "debug_arrows_units_height";
    pAsset10.id_prefab = "p_mapArrow_line";
    pAsset10.base_scale = 0.1f;
    pAsset10.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitHeight);
    pAsset10.debug_option = DebugOption.ArrowsUnitsHeight;
    pAsset10.render_gameplay = true;
    pAsset10.color = new Color(0.0f, 1f, 0.0f, 0.5f);
    this.add(pAsset10);
    QuantumSpriteAsset pAsset11 = new QuantumSpriteAsset();
    pAsset11.id = "debug_arrows_units_navigation_path";
    pAsset11.id_prefab = "p_mapArrow_line";
    pAsset11.base_scale = 0.08f;
    pAsset11.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitNavigationPath);
    pAsset11.debug_option = DebugOption.ArrowsUnitsPaths;
    pAsset11.render_gameplay = true;
    pAsset11.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    this.add(pAsset11);
    QuantumSpriteAsset pAsset12 = new QuantumSpriteAsset();
    pAsset12.id = "debug_arrows_units_next_step_tile";
    pAsset12.id_prefab = "p_mapArrow_line";
    pAsset12.base_scale = 0.08f;
    pAsset12.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitNextStepTile);
    pAsset12.debug_option = DebugOption.ArrowsUnitsNextStepTile;
    pAsset12.render_gameplay = true;
    pAsset12.color = new Color(0.4f, 1f, 1f, 0.9f);
    this.add(pAsset12);
    QuantumSpriteAsset pAsset13 = new QuantumSpriteAsset();
    pAsset13.id = "debug_arrows_units_next_position";
    pAsset13.id_prefab = "p_mapArrow_line";
    pAsset13.base_scale = 0.08f;
    pAsset13.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitNextStepPosition);
    pAsset13.debug_option = DebugOption.ArrowsUnitsNextStepPosition;
    pAsset13.render_gameplay = true;
    pAsset13.color = new Color(0.4f, 0.4f, 1f, 0.9f);
    this.add(pAsset13);
    QuantumSpriteAsset pAsset14 = new QuantumSpriteAsset();
    pAsset14.id = "debug_arrows_units_current_position";
    pAsset14.id_prefab = "p_mapArrow_line";
    pAsset14.base_scale = 0.08f;
    pAsset14.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsUnitCurrentPosition);
    pAsset14.debug_option = DebugOption.ArrowsUnitsCurrentPosition;
    pAsset14.render_gameplay = true;
    pAsset14.color = new Color(0.0f, 1f, 0.0f, 0.9f);
    this.add(pAsset14);
    QuantumSpriteAsset pAsset15 = new QuantumSpriteAsset();
    pAsset15.id = "debug_boat_passenger_lines";
    pAsset15.id_prefab = "p_mapArrow_line";
    pAsset15.base_scale = 0.08f;
    pAsset15.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsBoatPassengers);
    pAsset15.debug_option = DebugOption.BoatPassengerLines;
    pAsset15.render_gameplay = true;
    pAsset15.color = new Color(1f, 1f, 0.0f, 0.9f);
    this.add(pAsset15);
    QuantumSpriteAsset pAsset16 = new QuantumSpriteAsset();
    pAsset16.id = "debug_boat_taxi_request";
    pAsset16.id_prefab = "p_mapArrow_line";
    pAsset16.base_scale = 0.08f;
    pAsset16.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsPassengerTaxiRequestTargets);
    pAsset16.debug_option = DebugOption.ActorGizmosBoatTaxiRequestTargets;
    pAsset16.render_gameplay = true;
    pAsset16.color = new Color(0.0f, 1f, 0.0f, 0.9f);
    this.add(pAsset16);
    QuantumSpriteAsset pAsset17 = new QuantumSpriteAsset();
    pAsset17.id = "debug_building_residents";
    pAsset17.id_prefab = "p_mapArrow_line";
    pAsset17.base_scale = 0.08f;
    pAsset17.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsBuildingResidents);
    pAsset17.debug_option = DebugOption.BuildingResidents;
    pAsset17.render_gameplay = true;
    pAsset17.color = new Color(1f, 1f, 0.0f, 0.3f);
    this.add(pAsset17);
    QuantumSpriteAsset pAsset18 = new QuantumSpriteAsset();
    pAsset18.id = "debug_lovers";
    pAsset18.id_prefab = "p_mapArrow_line";
    pAsset18.base_scale = 0.08f;
    pAsset18.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawArrowsLovers);
    pAsset18.debug_option = DebugOption.Lovers;
    pAsset18.render_gameplay = true;
    pAsset18.color = new Color(1f, 0.0f, 0.0f, 0.5f);
    this.add(pAsset18);
    QuantumSpriteAsset pAsset19 = new QuantumSpriteAsset();
    pAsset19.id = "debug_favorite_foods";
    pAsset19.id_prefab = "p_mapSprite";
    pAsset19.base_scale = 0.2f;
    pAsset19.add_camera_zoom_multiplier = false;
    pAsset19.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawFavoriteFoods);
    pAsset19.debug_option = DebugOption.RenderFavoriteFoods;
    pAsset19.render_gameplay = true;
    this.add(pAsset19);
    QuantumSpriteAsset pAsset20 = new QuantumSpriteAsset();
    pAsset20.id = "debug_show_kingdom_icons";
    pAsset20.id_prefab = "p_mapSprite";
    pAsset20.base_scale = 0.1f;
    pAsset20.add_camera_zoom_multiplier = false;
    pAsset20.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawKingdomIcons);
    pAsset20.debug_option = DebugOption.ShowKingdomIcons;
    pAsset20.render_gameplay = true;
    this.add(pAsset20);
    QuantumSpriteAsset pAsset21 = new QuantumSpriteAsset();
    pAsset21.id = "debug_holding_items";
    pAsset21.id_prefab = "p_mapSprite";
    pAsset21.base_scale = 0.1f;
    pAsset21.add_camera_zoom_multiplier = false;
    pAsset21.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawHoldingFoods);
    pAsset21.debug_option = DebugOption.RenderHoldingResources;
    pAsset21.render_gameplay = true;
    this.add(pAsset21);
    QuantumSpriteAsset pAsset22 = new QuantumSpriteAsset();
    pAsset22.id = "debug_show_zones_mush";
    pAsset22.id_prefab = "p_mapZone";
    pAsset22.base_scale = 1f;
    pAsset22.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawMushInfection);
    pAsset22.debug_option = DebugOption.ShowMushInfection;
    pAsset22.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) => pQSprite.setColor(ref pAsset.color));
    pAsset22.render_map = true;
    pAsset22.add_camera_zoom_multiplier = false;
    pAsset22.color = Toolbox.makeColor("#FF5E6A", 0.2f);
    this.add(pAsset22);
    QuantumSpriteAsset pAsset23 = new QuantumSpriteAsset();
    pAsset23.id = "debug_show_highlighted_zones";
    pAsset23.id_prefab = "p_mapZone";
    pAsset23.base_scale = 1f;
    pAsset23.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawDebugHighlightedZones);
    pAsset23.render_map = true;
    pAsset23.render_gameplay = true;
    pAsset23.add_camera_zoom_multiplier = false;
    this.add(pAsset23);
    QuantumSpriteAsset pAsset24 = new QuantumSpriteAsset();
    pAsset24.id = "debug_show_godfinger_tiles";
    pAsset24.id_prefab = "p_mapZone";
    pAsset24.base_scale = 0.15f;
    pAsset24.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawGodFingerTiles);
    pAsset24.debug_option = DebugOption.ShowGodFingerTargetting;
    pAsset24.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 0;
    });
    pAsset24.render_map = true;
    pAsset24.render_gameplay = true;
    pAsset24.add_camera_zoom_multiplier = false;
    this.add(pAsset24);
    QuantumSpriteAsset pAsset25 = new QuantumSpriteAsset();
    pAsset25.id = "debug_show_dragon_attack_tiles";
    pAsset25.id_prefab = "p_mapZone";
    pAsset25.base_scale = 0.15f;
    pAsset25.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawDragonAttackTiles);
    pAsset25.debug_option = DebugOption.ShowDragonTargetting;
    pAsset25.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 0;
    });
    pAsset25.render_map = true;
    pAsset25.render_gameplay = true;
    pAsset25.add_camera_zoom_multiplier = false;
    this.add(pAsset25);
    QuantumSpriteAsset pAsset26 = new QuantumSpriteAsset();
    pAsset26.id = "debug_show_swim_targets";
    pAsset26.id_prefab = "p_mapZone";
    pAsset26.base_scale = 0.15f;
    pAsset26.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.drawSwimTargets);
    pAsset26.debug_option = DebugOption.ShowSwimToIslandLogic;
    pAsset26.create_object = (QuantumSpriteCreate) ((_, pQSprite) =>
    {
      ((Renderer) pQSprite.sprite_renderer).sortingLayerID = SortingLayer.NameToID("EffectsBack");
      ((Renderer) pQSprite.sprite_renderer).sortingOrder = 0;
    });
    pAsset26.render_map = true;
    pAsset26.render_gameplay = true;
    pAsset26.add_camera_zoom_multiplier = false;
    this.add(pAsset26);
    QuantumSpriteAsset pAsset27 = new QuantumSpriteAsset();
    pAsset27.id = "debug_show_zones_zombie_infection";
    pAsset27.id_prefab = "p_mapZone";
    pAsset27.base_scale = 1f;
    pAsset27.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawZombieInfection);
    pAsset27.debug_option = DebugOption.ShowZombieInfection;
    pAsset27.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) => pQSprite.setColor(ref pAsset.color));
    pAsset27.render_map = true;
    pAsset27.add_camera_zoom_multiplier = false;
    pAsset27.color = Toolbox.makeColor("#3FC668", 0.2f);
    this.add(pAsset27);
    QuantumSpriteAsset pAsset28 = new QuantumSpriteAsset();
    pAsset28.id = "debug_show_zones_plague";
    pAsset28.id_prefab = "p_mapZone";
    pAsset28.base_scale = 1f;
    pAsset28.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawPlagueInfection);
    pAsset28.debug_option = DebugOption.ShowPlagueInfection;
    pAsset28.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) => pQSprite.setColor(ref pAsset.color));
    pAsset28.render_map = true;
    pAsset28.add_camera_zoom_multiplier = false;
    pAsset28.color = Toolbox.makeColor("#C444FF", 0.2f);
    this.add(pAsset28);
    QuantumSpriteAsset pAsset29 = new QuantumSpriteAsset();
    pAsset29.id = "debug_show_zones_curse";
    pAsset29.id_prefab = "p_mapZone";
    pAsset29.base_scale = 1f;
    pAsset29.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawCurseInfection);
    pAsset29.debug_option = DebugOption.ShowCursed;
    pAsset29.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) => pQSprite.setColor(ref pAsset.color));
    pAsset29.render_map = true;
    pAsset29.add_camera_zoom_multiplier = false;
    pAsset29.color = Toolbox.makeColor("#852EAD", 0.2f);
    this.add(pAsset29);
    QuantumSpriteAsset pAsset30 = new QuantumSpriteAsset();
    pAsset30.id = "debug_dead_units";
    pAsset30.id_prefab = "p_mapZone";
    pAsset30.base_scale = 0.2f;
    pAsset30.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawDeadUnits);
    pAsset30.debug_option = DebugOption.DeadUnits;
    pAsset30.create_object = (QuantumSpriteCreate) ((_, pQSprite) => pQSprite.setSprite(SpriteTextureLoader.getSprite("ui/Icons/iconSkulls")));
    pAsset30.render_map = true;
    pAsset30.render_gameplay = true;
    pAsset30.color = Toolbox.makeColor("#FFFFFF", 0.1f);
    this.add(pAsset30);
    QuantumSpriteAsset pAsset31 = new QuantumSpriteAsset();
    pAsset31.id = "debug_draw_bad_links";
    pAsset31.id_prefab = "p_mapArrow_line";
    pAsset31.base_scale = 0.4f;
    pAsset31.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawBadLinks);
    pAsset31.debug_option = DebugOption.DrawBadLinksDiag;
    pAsset31.render_arrow_end = true;
    pAsset31.render_arrow_start = true;
    pAsset31.render_map = true;
    pAsset31.render_gameplay = true;
    pAsset31.color = Toolbox.makeColor("#D300B0", 0.8f);
    this.add(pAsset31);
    QuantumSpriteAsset pAsset32 = new QuantumSpriteAsset();
    pAsset32.id = "debug_cursor_city_zone_range";
    pAsset32.id_prefab = "p_mapZone";
    pAsset32.base_scale = 1f;
    pAsset32.add_camera_zoom_multiplier = false;
    pAsset32.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugCityZoneRange);
    pAsset32.debug_option = DebugOption.CursorCityZoneRange;
    pAsset32.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) => pQSprite.setColor(ref pAsset.color));
    pAsset32.render_map = true;
    pAsset32.render_gameplay = true;
    pAsset32.color = Toolbox.makeColor("#00FF00", 0.5f);
    this.add(pAsset32);
    QuantumSpriteAsset pAsset33 = new QuantumSpriteAsset();
    pAsset33.id = "debug_enemy_finder";
    pAsset33.id_prefab = "p_mapSprite";
    pAsset33.base_scale = 0.2f;
    pAsset33.debug_option = DebugOption.CursorEnemyFinderChunks;
    pAsset33.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugEnemyFinder);
    pAsset33.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) =>
    {
      Color.white.a = 0.8f;
      pQSprite.setSprite(SpriteTextureLoader.getSprite("ui/Icons/iconAccuracy"));
      pQSprite.setColor(ref pAsset.color);
    });
    pAsset33.render_map = true;
    pAsset33.render_gameplay = true;
    this.add(pAsset33);
    QuantumSpriteAsset pAsset34 = new QuantumSpriteAsset();
    pAsset34.id = "debug_show_population";
    pAsset34.id_prefab = "p_mapZone";
    pAsset34.base_scale = 1f;
    pAsset34.draw_call = new QuantumSpriteUpdater(QuantumSpriteLibrary.debugDrawPopulation);
    pAsset34.debug_option = DebugOption.ShowPopulationTotal;
    pAsset34.create_object = (QuantumSpriteCreate) ((pAsset, pQSprite) => pQSprite.setColor(ref pAsset.color));
    pAsset34.render_map = true;
    pAsset34.add_camera_zoom_multiplier = false;
    pAsset34.color = Toolbox.makeColor("#FFFFFF", 0.1f);
    this.add(pAsset34);
  }

  private static void drawMoney(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.isAlive() && (unit.data.money != 0 || unit.data.loot != 0))
      {
        Vector3 pPos = Vector2.op_Implicit(unit.current_position);
        ++pPos.y;
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos);
      }
    }
  }

  private static void debugDrawArrowsSettlers(QuantumSpriteAsset pAsset)
  {
  }

  private static void debugDrawClaimZone(QuantumSpriteAsset pAsset)
  {
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    City city1 = (City) null;
    if (mouseTilePos != null && DebugConfig.isOn(DebugOption.ArrowsOnlyForCursorCities))
      city1 = mouseTilePos.zone.city;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.hasKing() && kingdom.king.isTask("claim_land"))
        QuantumSpriteLibrary.checkDrawClaimLand(pAsset, kingdom.king);
      foreach (City city2 in kingdom.getCities())
      {
        if ((city1 == null || city2 == city1) && city2.hasLeader() && city2.leader.isTask("claim_land"))
          QuantumSpriteLibrary.checkDrawClaimLand(pAsset, city2.leader);
      }
    }
  }

  private static void checkDrawClaimLand(QuantumSpriteAsset pAsset, Actor pActor)
  {
    if (pActor.city.isRekt())
      return;
    WorldTile currentTile = pActor.current_tile;
    WorldTile behTileTarget = pActor.beh_tile_target;
    if (currentTile == null || behTileTarget == null)
      return;
    QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, currentTile.posV3, behTileTarget.posV3, ref Toolbox.color_yellow);
  }

  private static void debugDrawArrowsKingdomAttackTarget(QuantumSpriteAsset pAsset)
  {
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    City city1 = (City) null;
    if (mouseTilePos != null && DebugConfig.isOn(DebugOption.ArrowsOnlyForCursorCities))
      city1 = mouseTilePos.zone.city;
    foreach (MetaObject<KingdomData> kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      foreach (City city2 in kingdom.getCities())
      {
        if (city2.target_attack_city != null && (!Zones.showCityZones() || city1 == null || city2 == city1))
        {
          WorldTile tile = city2.getTile();
          WorldTile centerTile = city2.target_attack_zone.centerTile;
          if (tile != null && centerTile != null)
            QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, tile.posV3, centerTile.posV3, ref Toolbox.color_red);
        }
      }
    }
  }

  private static void drawUnitAttackRange(QuantumSpriteAsset pAsset)
  {
    if (ControllableUnit.isControllingUnit())
      return;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor == null || lastActor.isInMagnet())
      return;
    float pForceScaleTo = lastActor.getAttackRange() / 13f;
    ((Component) QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(lastActor.current_position), pForceScaleTo: pForceScaleTo)).transform.position = Vector2.op_Implicit(lastActor.current_position);
  }

  private static void drawUnitSize(QuantumSpriteAsset pAsset)
  {
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor == null || lastActor.isInMagnet())
      return;
    float pForceScaleTo = lastActor.stats["size"] / 13f;
    ((Component) QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(lastActor.current_position), pForceScaleTo: pForceScaleTo)).transform.position = Vector2.op_Implicit(lastActor.current_position);
  }

  private static void debugDrawArrowsUnitAttackTargets(QuantumSpriteAsset pAsset)
  {
    bool flag = DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly);
    Actor[] visibleUnits = QuantumSpriteLibrary.visible_units;
    int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
    for (int index = 0; index < visibleUnitsCount; ++index)
    {
      Actor actor = visibleUnits[index];
      if (actor.has_attack_target && (!flag || actor.isFavorite()) && actor.isEnemyTargetAlive())
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position), Vector2.op_Implicit(actor.attack_target.current_position), ref pAsset.color);
    }
  }

  private static void debugDrawArrowsUnitBehTarget(QuantumSpriteAsset pAsset)
  {
    bool flag = DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly);
    Actor[] visibleUnits = QuantumSpriteLibrary.visible_units;
    int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
    for (int index = 0; index < visibleUnitsCount; ++index)
    {
      Actor actor = visibleUnits[index];
      if (actor.beh_actor_target != null && (!flag || actor.isFavorite()) && actor.beh_actor_target != null)
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position), Vector2.op_Implicit(actor.beh_actor_target.current_position), ref pAsset.color);
    }
  }

  private static void debugDrawArrowsUnitNavigationTargets(QuantumSpriteAsset pAsset)
  {
    bool flag = DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly);
    Actor[] visibleUnits = QuantumSpriteLibrary.visible_units;
    int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
    for (int index = 0; index < visibleUnitsCount; ++index)
    {
      Actor actor = visibleUnits[index];
      if (actor.tile_target != null && (!flag || actor.isFavorite()))
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position), actor.tile_target.posV3, ref pAsset.color);
    }
  }

  private static void debugDrawArrowsUnitHeight(QuantumSpriteAsset pAsset)
  {
    bool flag = DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly);
    Actor[] visibleUnits = QuantumSpriteLibrary.visible_units;
    int visibleUnitsCount = QuantumSpriteLibrary.visible_units_count;
    for (int index = 0; index < visibleUnitsCount; ++index)
    {
      Actor actor = visibleUnits[index];
      if (!flag || actor.isFavorite())
      {
        Vector3 pStart = Vector2.op_Implicit(actor.current_position);
        Vector3 pEnd = Vector2.op_Implicit(actor.current_position);
        pEnd.y += actor.getHeight();
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, pEnd, ref pAsset.color);
      }
    }
  }

  private static void debugDrawArrowsUnitNavigationPath(QuantumSpriteAsset pAsset)
  {
    bool flag = DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly);
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.current_path.Count != 0 && (!flag || actor.isFavorite()))
      {
        WorldTile worldTile1 = (WorldTile) null;
        foreach (WorldTile worldTile2 in actor.current_path)
        {
          if (worldTile1 == null)
          {
            worldTile1 = worldTile2;
            QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, actor.current_tile.posV3, worldTile1.posV3, ref pAsset.color);
          }
          else
          {
            QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, worldTile1.posV3, worldTile2.posV3, ref pAsset.color);
            worldTile1 = worldTile2;
          }
        }
      }
    }
  }

  private static void debugDrawArrowsUnitNextStepTile(QuantumSpriteAsset pAsset)
  {
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.debug_next_step_tile != null && (!DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly) || actor.isFavorite()))
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position), actor.debug_next_step_tile.posV3, ref pAsset.color);
    }
  }

  private static void debugDrawArrowsUnitNextStepPosition(QuantumSpriteAsset pAsset)
  {
    bool flag = DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly);
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.is_moving && (!flag || actor.isFavorite()))
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position), Vector2.op_Implicit(actor.next_step_position), ref pAsset.color);
    }
  }

  private static void debugDrawArrowsUnitCurrentPosition(QuantumSpriteAsset pAsset)
  {
    bool flag = DebugConfig.isOn(DebugOption.ArrowsUnitsFavoritesOnly);
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (!flag || actor.isFavorite())
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position), actor.current_tile.posV3, ref pAsset.color);
    }
  }

  private static void debugDrawArrowsBoatPassengers(QuantumSpriteAsset pAsset)
  {
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor1 = visibleUnitsAlive[index];
      if (actor1.asset.is_boat && actor1.asset.is_boat_transport)
      {
        TaxiRequest taxiRequest = actor1.getSimpleComponent<Boat>().taxi_request;
        if (taxiRequest != null)
        {
          foreach (Actor actor2 in taxiRequest.getActors())
            QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor2.current_position), actor1.current_tile.posV3, ref pAsset.color);
        }
      }
    }
  }

  private static void debugDrawArrowsPassengerTaxiRequestTargets(QuantumSpriteAsset pAsset)
  {
    Color cyan = Color.cyan;
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor1 = visibleUnitsAlive[index];
      if (actor1.asset.is_boat && actor1.asset.is_boat_transport)
      {
        TaxiRequest taxiRequest = actor1.getSimpleComponent<Boat>().taxi_request;
        if (taxiRequest != null)
        {
          foreach (Actor actor2 in taxiRequest.getActors())
          {
            QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor2.current_position), taxiRequest.getTileStart().posV3, ref pAsset.color);
            QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor2.current_position), taxiRequest.getTileTarget().posV3, ref cyan);
          }
        }
      }
    }
  }

  private static void debugDrawArrowsBuildingResidents(QuantumSpriteAsset pAsset)
  {
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      Building homeBuilding = actor.getHomeBuilding();
      if (homeBuilding != null)
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position), homeBuilding.current_tile.posV3, ref pAsset.color);
    }
  }

  private static void debugDrawArrowsLovers(QuantumSpriteAsset pAsset)
  {
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.hasLover() && actor.data.created_time >= actor.lover.data.created_time)
      {
        Actor lover = actor.lover;
        Vector3 pStart = Vector2.op_Implicit(actor.current_position);
        pStart.y += 0.5f;
        Color color = pAsset.color;
        color.a = actor.kingdom == lover.kingdom ? (actor.city == lover.city ? 0.5f : 0.2f) : 0.1f;
        if (actor.isKingdomCiv())
        {
          color.r = 1f;
          color.g = 0.0f;
          color.b = 0.0f;
        }
        else
        {
          color.r = 1f;
          color.g = 1f;
          color.b = 0.0f;
        }
        QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, pStart, Vector2.op_Implicit(lover.current_position), ref color);
      }
    }
  }

  private static void debugDrawFavoriteFoods(QuantumSpriteAsset pAsset)
  {
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.hasFavoriteFood())
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(actor.current_position)).setSprite(actor.favorite_food_asset.getSpriteIcon());
    }
  }

  private static void debugDrawKingdomIcons(QuantumSpriteAsset pAsset)
  {
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.kingdom.asset.show_icon)
      {
        Vector3 pPos = Vector2.op_Implicit(actor.current_position);
        ++pPos.y;
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos).setSprite(actor.kingdom.asset.getSprite());
      }
    }
  }

  private static void debugDrawHoldingFoods(QuantumSpriteAsset pAsset)
  {
    Actor[] visibleUnitsAlive = QuantumSpriteLibrary.visible_units_alive;
    int visibleUnitsAliveCount = QuantumSpriteLibrary.visible_units_alive_count;
    for (int index = 0; index < visibleUnitsAliveCount; ++index)
    {
      Actor actor = visibleUnitsAlive[index];
      if (actor.isCarryingResources())
      {
        string itemIdToRender = actor.inventory.getItemIDToRender();
        if (!string.IsNullOrEmpty(itemIdToRender))
        {
          Vector3 pPos = Vector2.op_Implicit(actor.current_position);
          pPos.y += 2f;
          QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos).setSprite(AssetManager.resources.get(itemIdToRender).getSpriteIcon());
        }
      }
    }
  }

  private static void debugDrawMushInfection(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.hasTrait("mush_spores"))
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, unit.current_tile.zone.centerTile.posV);
    }
  }

  private static void drawDebugHighlightedZones(QuantumSpriteAsset pAsset)
  {
    if (DebugHighlight.hashset.Count == 0)
      return;
    foreach (DebugHighlightContainer highlightContainer in DebugHighlight.hashset)
    {
      QuantumSprite quantumSprite = (QuantumSprite) null;
      if (highlightContainer.zone != null)
        quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, highlightContainer.zone.centerTile.posV);
      else if (highlightContainer.chunk != null)
        quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, highlightContainer.chunk.tiles[0].zone.centerTile.posV);
      Color color = highlightContainer.color;
      color.a = highlightContainer.timer / highlightContainer.interval * highlightContainer.color.a;
      quantumSprite.setColor(ref color);
    }
  }

  private static void debugDrawGodFingerTiles(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in World.world.kingdoms_wild.get("godfinger").units)
    {
      if (unit.isAlive())
      {
        GodFinger actorComponent = unit.getActorComponent<GodFinger>();
        Color debugColor = actorComponent.debug_color;
        debugColor.a = 0.9f;
        foreach (WorldTile targetTile in actorComponent.target_tiles)
          QuantumSpriteLibrary.drawQuantumSprite(pAsset, targetTile.posV).setColor(ref debugColor);
        GodFinger.debug_trail(actorComponent);
      }
    }
  }

  private static void debugDrawDragonAttackTiles(QuantumSpriteAsset pAsset)
  {
    Kingdom kingdom1 = World.world.kingdoms_wild.get("dragons");
    Kingdom kingdom2 = World.world.kingdoms_wild.get("undead");
    if (kingdom1 == null && kingdom2 == null)
      return;
    if (kingdom1 != null && kingdom1.units.Count > 0)
      QuantumSpriteLibrary.debugDrawDragonAttackTiles(pAsset, kingdom1.units);
    if (kingdom2 != null && kingdom2.units.Count > 0)
      QuantumSpriteLibrary.debugDrawDragonAttackTiles(pAsset, kingdom2.units);
    foreach (WorldTile tempListTile in Toolbox.temp_list_tiles)
    {
      QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, tempListTile.posV);
      Color colorMushSpores = Toolbox.color_mushSpores;
      colorMushSpores.a = 0.4f;
      ref Color local = ref colorMushSpores;
      quantumSprite.setColor(ref local);
    }
  }

  private static void debugDrawDragonAttackTiles(QuantumSpriteAsset pAsset, List<Actor> pUnits)
  {
    foreach (Actor pUnit in pUnits)
    {
      if (pUnit.isAlive())
      {
        Dragon actorComponent = pUnit.getActorComponent<Dragon>();
        if (!Object.op_Equality((Object) actorComponent, (Object) null))
        {
          Color pColor1 = Toolbox.color_infected;
          float num1 = (float) (0.10000000149011612 + (double) actorComponent._landAttackCache * 0.10000000149011612);
          pColor1.a = Mathf.Min(num1, 0.8f);
          foreach (WorldTile landAttackTile in actorComponent.getLandAttackTiles())
            QuantumSpriteLibrary.drawQuantumSprite(pAsset, landAttackTile.posV).setColor(ref pColor1);
          Color pColor2 = Color32.op_Implicit(Toolbox.color_phenotype_green_0);
          float num2 = (float) (0.10000000149011612 + (double) actorComponent._slideAttackTilesFlipCache * 0.10000000149011612);
          pColor2.a = Mathf.Min(num2, 0.8f);
          foreach (WorldTile worldTile in actorComponent._slideAttackTilesFlip)
            QuantumSpriteLibrary.drawQuantumSprite(pAsset, worldTile.posV).setColor(ref pColor2);
          Color pColor3 = Color32.op_Implicit(Toolbox.color_magenta_1);
          float num3 = (float) (0.10000000149011612 + (double) actorComponent._slideAttackTilesNoFlipCache * 0.10000000149011612);
          pColor3.a = Mathf.Min(num3, 0.8f);
          foreach (WorldTile worldTile in actorComponent._slideAttackTilesNoFlip)
            QuantumSpriteLibrary.drawQuantumSprite(pAsset, worldTile.posV).setColor(ref pColor3);
          pColor1 = Toolbox.color_red;
          if (pUnit.tile_target != null)
            QuantumSpriteLibrary.drawQuantumSprite(pAsset, pUnit.tile_target.posV).setColor(ref pColor1);
          pColor1 = Toolbox.color_heal;
          if (pUnit.beh_tile_target != null)
            QuantumSpriteLibrary.drawQuantumSprite(pAsset, pUnit.beh_tile_target.posV).setColor(ref pColor1);
        }
      }
    }
  }

  private static void drawSwimTargets(QuantumSpriteAsset pAsset)
  {
    Color colorInfected = Toolbox.color_infected;
    colorInfected.a = 0.8f;
    foreach (KeyValuePair<int, MapRegion> bestRegion in BehGoToStablePlace.bestRegions)
    {
      List<WorldTile> tiles = bestRegion.Value.tiles;
      for (int index = 0; index < tiles.Count; ++index)
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, tiles[index].posV).setColor(ref colorInfected);
    }
    if (BehGoToStablePlace.best_tile == null)
      return;
    Color colorRed = Toolbox.color_red;
    QuantumSpriteLibrary.drawQuantumSprite(pAsset, BehGoToStablePlace.best_tile.posV).setColor(ref colorRed);
  }

  private static void debugDrawZombieInfection(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.hasTrait("infected") || unit.hasTrait("zombie"))
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, unit.current_tile.zone.centerTile.posV);
    }
  }

  private static void debugDrawPlagueInfection(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.hasTrait("plague"))
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, unit.current_tile.zone.centerTile.posV);
    }
  }

  private static void debugDrawCurseInfection(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.hasStatus("cursed"))
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, unit.current_tile.zone.centerTile.posV);
    }
  }

  private static void debugDrawDeadUnits(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (!unit.isAlive())
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(unit.current_position));
    }
  }

  private static void debugDrawCitizenJobs(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.citizen_job != null && (DebugConfig.isOn(DebugOption.DrawCitizenJobIconsAll) || DebugConfig.isOn(unit.citizen_job.debug_option)))
        QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(unit.current_position)).setSprite(SpriteTextureLoader.getSprite(unit.citizen_job.path_icon));
    }
  }

  private static void debugDrawBadLinks(QuantumSpriteAsset pAsset)
  {
    foreach (MapChunk chunk in World.world.map_chunk_manager.chunks)
    {
      foreach (MapRegion region in chunk.regions)
      {
        foreach (MapRegion neighbour in region.neighbours)
        {
          if ((double) Toolbox.Dist(neighbour.chunk.x, neighbour.chunk.y, region.chunk.x, region.chunk.y) >= 1.5)
          {
            Vector3 posV1 = region.tiles[0].posV;
            Vector3 posV2 = neighbour.tiles[0].posV;
            QuantumSpriteLibrary.drawArrowQuantumSprite(pAsset, posV1, posV2, ref pAsset.color);
            break;
          }
        }
      }
    }
  }

  private static void debugCityZoneRange(QuantumSpriteAsset pAsset)
  {
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    if (mouseTilePos == null)
      return;
    City city = mouseTilePos.zone.city;
    if (city.isRekt())
      return;
    HashSet<TileZone> pSetToFill = new HashSet<TileZone>();
    Bench.bench(nameof (debugCityZoneRange), "meh");
    World.world.city_zone_helper.city_growth.getZoneToClaim((Actor) null, city, true, pSetToFill);
    Debug.Log((object) ("bench city growth: " + Bench.benchEnd(nameof (debugCityZoneRange), "meh").ToString()));
    foreach (TileZone tileZone in pSetToFill)
      QuantumSpriteLibrary.drawQuantumSprite(pAsset, tileZone.centerTile.posV);
  }

  private static void debugEnemyFinder(QuantumSpriteAsset pAsset)
  {
    if (World.world.getMouseTilePos() == null)
      return;
    Actor actorNearCursor = World.world.getActorNearCursor();
    if (actorNearCursor == null || actorNearCursor.isInMagnet())
      return;
    EnemyFinderData enemiesFrom = EnemiesFinder.findEnemiesFrom(actorNearCursor.current_tile, actorNearCursor.kingdom);
    if (enemiesFrom.isEmpty())
      return;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(0.0f, 1f);
    foreach (BaseSimObject baseSimObject in enemiesFrom.list)
      QuantumSpriteLibrary.drawQuantumSprite(pAsset, Vector2.op_Implicit(Vector2.op_Addition(baseSimObject.current_position, vector2)), pModScale: 0.2f);
  }

  private static void debugDrawPopulation(QuantumSpriteAsset pAsset)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      QuantumSpriteLibrary.drawQuantumSprite(pAsset, unit.current_tile.zone.centerTile.posV);
  }

  private static void drawStockpileResources(QuantumSpriteAsset pAsset)
  {
    List<Building> visibleStockpiles = World.world.buildings.visible_stockpiles;
    if (visibleStockpiles.Count == 0)
      return;
    if (QuantumSpriteLibrary._array_stockpile_slots == null)
    {
      QuantumSpriteLibrary._array_stockpile_slots = new Vector2[35];
      for (int index1 = 0; index1 < 5; ++index1)
      {
        for (int index2 = 0; index2 < 7; ++index2)
        {
          int index3 = index1 * 7 + index2;
          QuantumSpriteLibrary._array_stockpile_slots[index3] = new Vector2((float) index2, (float) index1);
        }
      }
      QuantumSpriteLibrary._array_stockpile_slots.Shuffle<Vector2>();
    }
    foreach (Building pBuilding in visibleStockpiles)
    {
      if (pBuilding.is_visible && pBuilding.isUsable() && !pBuilding.isUnderConstruction())
        QuantumSpriteLibrary.drawStockpileResourcesForBuilding(pAsset, pBuilding);
    }
  }

  private static void drawStockpileResourcesForBuilding(
    QuantumSpriteAsset pAsset,
    Building pBuilding)
  {
    float tweenBuildingsValue = World.world.quality_changer.getTweenBuildingsValue();
    Color pColor = Toolbox.color_white;
    if (!pBuilding.hasCity())
      pColor = Toolbox.color_abandoned_building;
    Vector3 transformPosition = pBuilding.cur_transform_position;
    transformPosition.x += pBuilding.asset.stockpile_top_left_offset.x * tweenBuildingsValue;
    transformPosition.y += pBuilding.asset.stockpile_top_left_offset.y * tweenBuildingsValue;
    transformPosition.z = 0.0f;
    using (ListPool<SlotDrawAmount> listPool = new ListPool<SlotDrawAmount>())
    {
      foreach (CityStorageSlot slot in pBuilding.resources.getSlots())
      {
        if (slot.amount != 0)
          listPool.Add(new SlotDrawAmount()
          {
            resource_id = slot.id,
            amount = slot.amount / slot.asset.stack_size + 1
          });
      }
      int index1 = 0;
      int index2 = 0;
      while (index1 < 35 && listPool.Count > 0)
      {
        SlotDrawAmount slotDrawAmount = listPool[index2];
        if (slotDrawAmount.amount <= 0)
        {
          listPool.RemoveAt(index2);
          if (index2 >= listPool.Count)
            index2 = 0;
        }
        else
        {
          int amount = slotDrawAmount.amount;
          if (amount <= 0)
            break;
          int x = (int) QuantumSpriteLibrary._array_stockpile_slots[index1].x;
          int y = (int) QuantumSpriteLibrary._array_stockpile_slots[index1].y;
          ResourceAsset asset = slotDrawAmount.asset;
          int num1 = amount;
          Sprite gameplaySprite = asset.getGameplaySprite();
          int num2 = Mathf.Clamp(num1, 1, 7);
          if (y % 2 != 0)
            --num2;
          slotDrawAmount.amount -= num2;
          listPool[index2] = slotDrawAmount;
          for (int pIndex = 0; pIndex < num2; ++pIndex)
            QuantumSpriteLibrary.drawResourceIconOnStockpile(pAsset, transformPosition, gameplaySprite, pIndex, x, y, ref pColor);
          ++index1;
          ++index2;
          if (index2 >= listPool.Count)
            index2 = 0;
          if (index1 >= 35)
            break;
        }
      }
    }
  }

  private static void drawResourceIconOnStockpile(
    QuantumSpriteAsset pAsset,
    Vector3 pMainPosition,
    Sprite pSprite,
    int pIndex,
    int pRow,
    int pColumn,
    ref Color pColor)
  {
    Vector3 pPos = pMainPosition;
    pPos.x += 0.58f * (float) pRow;
    pPos.y -= 0.5f * (float) pColumn;
    if (pColumn % 2 != 0)
      pPos.x += 0.29f;
    pPos.y += 0.4f * (float) pIndex;
    pPos.z += 0.5f * (float) pIndex;
    QuantumSprite quantumSprite = QuantumSpriteLibrary.drawQuantumSprite(pAsset, pPos);
    quantumSprite.setSprite(pSprite);
    quantumSprite.setColor(ref pColor);
  }
}
