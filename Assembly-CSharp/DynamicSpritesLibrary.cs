// Decompiled with JetBrains decompiler
// Type: DynamicSpritesLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class DynamicSpritesLibrary : AssetLibrary<DynamicSpritesAsset>
{
  public static DynamicSpritesAsset units;
  public static DynamicSpritesAsset boats;
  public static DynamicSpritesAsset units_shadows;
  public static DynamicSpritesAsset building_lights;
  public static DynamicSpritesAsset building_shadows;
  public static DynamicSpritesAsset icons;
  public static DynamicSpritesAsset items;
  public static DynamicSpritesAsset zombies;
  private bool _dirty;
  public static long _debug_id;
  private static long _debug_kingdom_color_id;
  private static long _debug_head_id;
  private static long _debug_main_body_sprite;
  private static long _debug_phenotype_index;
  private static long _debug_shade_id;

  public override void init()
  {
    base.init();
    DynamicSpritesAsset pAsset1 = new DynamicSpritesAsset();
    pAsset1.id = "units";
    pAsset1.atlas_id = UnitTextureAtlasID.Units;
    DynamicSpritesLibrary.units = this.add(pAsset1);
    DynamicSpritesAsset pAsset2 = new DynamicSpritesAsset();
    pAsset2.id = "units_shadows";
    pAsset2.atlas_id = UnitTextureAtlasID.UnitsShadows;
    DynamicSpritesLibrary.units_shadows = this.add(pAsset2);
    DynamicSpritesAsset pAsset3 = new DynamicSpritesAsset();
    pAsset3.id = "boats";
    pAsset3.atlas_id = UnitTextureAtlasID.Boats;
    DynamicSpritesLibrary.boats = this.add(pAsset3);
    DynamicSpritesAsset pAsset4 = new DynamicSpritesAsset();
    pAsset4.id = "buildings";
    pAsset4.export_folder_path = "buildings";
    pAsset4.atlas_id = UnitTextureAtlasID.Buildings;
    pAsset4.buildings = true;
    this.add(pAsset4);
    DynamicSpritesAsset pAsset5 = new DynamicSpritesAsset();
    pAsset5.id = "buildings_trees";
    pAsset5.export_folder_path = "buildings_trees";
    pAsset5.check_wobbly_setting = true;
    pAsset5.atlas_id = UnitTextureAtlasID.BuildingsTrees;
    pAsset5.buildings = true;
    this.add(pAsset5);
    DynamicSpritesAsset pAsset6 = new DynamicSpritesAsset();
    pAsset6.id = "buildings_wobbly";
    pAsset6.export_folder_path = "buildings_wobbly";
    pAsset6.check_wobbly_setting = true;
    pAsset6.atlas_id = UnitTextureAtlasID.BuildingsWobbly;
    pAsset6.buildings = true;
    this.add(pAsset6);
    DynamicSpritesAsset pAsset7 = new DynamicSpritesAsset();
    pAsset7.id = "buildings_trees_big";
    pAsset7.export_folder_path = "buildings_trees_big";
    pAsset7.check_wobbly_setting = true;
    pAsset7.atlas_id = UnitTextureAtlasID.BuildingsTreesBig;
    pAsset7.buildings = true;
    this.add(pAsset7);
    DynamicSpritesAsset pAsset8 = new DynamicSpritesAsset();
    pAsset8.id = "building_lights";
    pAsset8.atlas_id = UnitTextureAtlasID.BuildingsLights;
    DynamicSpritesLibrary.building_lights = this.add(pAsset8);
    DynamicSpritesAsset pAsset9 = new DynamicSpritesAsset();
    pAsset9.id = "building_shadows";
    pAsset9.atlas_id = UnitTextureAtlasID.BuildingsShadows;
    DynamicSpritesLibrary.building_shadows = this.add(pAsset9);
    DynamicSpritesAsset pAsset10 = new DynamicSpritesAsset();
    pAsset10.id = "icons";
    pAsset10.big_atlas = false;
    pAsset10.atlas_id = UnitTextureAtlasID.Icons;
    DynamicSpritesLibrary.icons = this.add(pAsset10);
    DynamicSpritesAsset pAsset11 = new DynamicSpritesAsset();
    pAsset11.id = "items";
    pAsset11.atlas_id = UnitTextureAtlasID.Items;
    DynamicSpritesLibrary.items = this.add(pAsset11);
    DynamicSpritesAsset pAsset12 = new DynamicSpritesAsset();
    pAsset12.id = "zombies";
    pAsset12.atlas_id = UnitTextureAtlasID.Zombies;
    DynamicSpritesLibrary.zombies = this.add(pAsset12);
  }

  public override DynamicSpritesAsset add(DynamicSpritesAsset pAsset)
  {
    base.add(pAsset);
    return pAsset;
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (Asset asset in this.list)
      asset.create();
  }

  public void clear()
  {
    foreach (DynamicSpritesAsset dynamicSpritesAsset in this.list)
      dynamicSpritesAsset.clear();
  }

  public void debug(DebugTool pTool, Actor pActor)
  {
    DynamicSpriteCreator.debug_actor = pActor;
    pTool.setText("preloaded_sprites_units:", (object) ActorTextureSubAsset.all_preloaded_sprites_units.Count);
    pTool.setText("pixel_bags:", (object) PixelBagManager.total);
    pTool.setText("total_sprite_textures:", (object) SpriteTextureLoader.total_sprites);
    pTool.setText("total_sprites_lists:", (object) SpriteTextureLoader.total_sprites_list);
    pTool.setText("total_sprites_lists_single_sprites:", (object) SpriteTextureLoader.total_sprites_list_single_sprites);
    pTool.setText("total_texture_sub_assets:", (object) ActorTextureSubAsset.getTotal());
    pTool.setText("hand_renderer_textures:", (object) HandRendererTexturePreloader.getTotal());
    pTool.setSeparator();
    pTool.setText("[ActorAnimationLoader] units:", (object) ActorAnimationLoader.count_units);
    pTool.setText("[ActorAnimationLoader] heads:", (object) ActorAnimationLoader.count_heads);
    pTool.setText("[ActorAnimationLoader] boats:", (object) ActorAnimationLoader.count_boats);
    pTool.setSeparator();
    foreach (DynamicSpritesAsset dynamicSpritesAsset in AssetManager.dynamic_sprites_library.list)
    {
      pTool.setText($"sprites {dynamicSpritesAsset.id}:", (object) dynamicSpritesAsset.countSprites());
      pTool.setText($"textures {dynamicSpritesAsset.id}:", (object) dynamicSpritesAsset.countTextures());
    }
    pTool.setText("units:", (object) DynamicSpritesLibrary.units.getAtlas().debug());
    pTool.setText("boats:", (object) DynamicSpritesLibrary.boats.getAtlas().debug());
    pTool.setSeparator();
    pTool.setText("_debug_id:", (object) DynamicSpritesLibrary._debug_id);
    pTool.setText("_debug_head_id:", (object) DynamicSpritesLibrary._debug_head_id);
    pTool.setText("_debug_main_body_sprite:", (object) DynamicSpritesLibrary._debug_main_body_sprite);
    pTool.setText("_debug_phenotype_index:", (object) DynamicSpritesLibrary._debug_phenotype_index);
    pTool.setText("_debug_kingdom_color_id:", (object) DynamicSpritesLibrary._debug_kingdom_color_id);
  }

  public void setDirty() => this._dirty = true;

  public void checkDirty()
  {
    if (!this._dirty)
      return;
    this._dirty = false;
    foreach (DynamicSpritesAsset dynamicSpritesAsset in AssetManager.dynamic_sprites_library.list)
      dynamicSpritesAsset.checkAtlasDirty();
  }

  public void setDebugActor(
    long pID,
    long pKingdomColorID,
    long pHeadID,
    long pMainBodySpriteID,
    long pPhenotypeIndex,
    long pShadeID)
  {
    DynamicSpritesLibrary._debug_id = pID;
    DynamicSpritesLibrary._debug_kingdom_color_id = pKingdomColorID;
    DynamicSpritesLibrary._debug_head_id = pHeadID;
    DynamicSpritesLibrary._debug_main_body_sprite = pMainBodySpriteID;
    DynamicSpritesLibrary._debug_phenotype_index = pPhenotypeIndex;
    DynamicSpritesLibrary._debug_shade_id = pShadeID;
  }

  public void export()
  {
  }
}
