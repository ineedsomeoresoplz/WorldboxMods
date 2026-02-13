// Decompiled with JetBrains decompiler
// Type: TileLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TileLibrary : TileLibraryMain<TileType>
{
  private TileType[] _depth_list_generator = new TileType[256 /*0x0100*/];
  private TileType[] _depth_list_gameplay = new TileType[256 /*0x0100*/];
  private TileType[] _depth_list;
  public static List<TileType> lava_types = new List<TileType>();
  public static TileType summit;
  public static TileType mountains;
  public static TileType hills;
  public static TileType grey_goo;
  public static TileType deep_ocean;
  public static TileType close_ocean;
  public static TileType shallow_waters;
  public static TileType sand;
  public static TileType soil_low;
  public static TileType soil_high;
  public static TileType lava0;
  public static TileType lava1;
  public static TileType lava2;
  public static TileType lava3;
  public static TileType pit_deep_ocean;
  public static TileType pit_close_ocean;
  public static TileType pit_shallow_waters;
  public static TileTypeBase[] array_tiles = new TileTypeBase[256 /*0x0100*/];

  public override void init()
  {
    base.init();
    TileType pAsset1 = new TileType();
    pAsset1.id = "deep_ocean";
    pAsset1.color_hex = "#3370CC";
    pAsset1.liquid = true;
    pAsset1.ocean = true;
    pAsset1.height_min = 0;
    pAsset1.decrease_to_id = "pit_deep_ocean";
    pAsset1.increase_to_id = "pit_close_ocean";
    pAsset1.walk_multiplier = 0.1f;
    pAsset1.strength = 0;
    pAsset1.layer_type = TileLayerType.Ocean;
    pAsset1.can_be_frozen = false;
    pAsset1.can_errode_to_sand = false;
    TileLibrary.deep_ocean = this.add(pAsset1);
    this.t.considered_empty_tile = true;
    this.t.used_in_generator = true;
    this.t.setDrawLayer(TileZIndexes.deep_ocean);
    this.t.render_z = 0;
    TileLibrary.close_ocean = this.clone("close_ocean", "deep_ocean");
    this.t.considered_empty_tile = false;
    this.t.can_be_frozen = false;
    this.t.used_in_generator = true;
    this.t.setDrawLayer(TileZIndexes.close_ocean);
    this.t.drawPixel = true;
    this.t.color_hex = "#4084E2";
    this.t.height_min = 30;
    this.t.decrease_to_id = "pit_close_ocean";
    this.t.increase_to_id = "pit_shallow_waters";
    this.t.strength = 0;
    this.t.layer_type = TileLayerType.Ocean;
    this.t.can_errode_to_sand = false;
    TileType pAsset2 = new TileType();
    pAsset2.id = "shallow_waters";
    pAsset2.drawPixel = true;
    pAsset2.can_be_frozen = true;
    pAsset2.color_hex = "#55AEF0";
    pAsset2.edge_color_hex = "#3F90EA";
    pAsset2.liquid = true;
    pAsset2.ocean = true;
    pAsset2.height_min = 70;
    pAsset2.freeze_to_id = "ice";
    pAsset2.decrease_to_id = "pit_shallow_waters";
    pAsset2.increase_to_id = "sand";
    pAsset2.walk_multiplier = 0.1f;
    pAsset2.strength = 0;
    pAsset2.layer_type = TileLayerType.Ocean;
    pAsset2.can_errode_to_sand = false;
    pAsset2.fast_freeze = true;
    TileLibrary.shallow_waters = this.add(pAsset2);
    this.t.used_in_generator = true;
    this.t.setDrawLayer(TileZIndexes.shallow_waters);
    TileLibrary.pit_deep_ocean = this.clone("pit_deep_ocean", "deep_ocean");
    this.t.can_be_frozen = false;
    this.t.setDrawLayer(TileZIndexes.pit_deep_ocean);
    this.t.drawPixel = true;
    this.t.color_hex = "#898989";
    this.t.liquid = false;
    this.t.ocean = false;
    this.t.walk_multiplier = 1f;
    this.t.can_be_filled_with_ocean = true;
    this.t.fill_to_ocean = "deep_ocean";
    this.t.water_fill_sound = "event:/SFX/NATURE/FillWaterTile";
    this.t.ground = true;
    this.t.decrease_to_id = string.Empty;
    this.t.increase_to_id = "pit_close_ocean";
    this.t.can_be_set_on_fire = true;
    this.t.layer_type = TileLayerType.Ground;
    this.t.strength = 2;
    this.t.considered_empty_tile = true;
    TileLibrary.pit_close_ocean = this.clone("pit_close_ocean", "close_ocean");
    this.t.can_be_frozen = false;
    this.t.setDrawLayer(TileZIndexes.pit_close_ocean);
    this.t.drawPixel = true;
    this.t.color_hex = "#A0A0A0";
    this.t.liquid = false;
    this.t.ocean = false;
    this.t.walk_multiplier = 1f;
    this.t.can_be_filled_with_ocean = true;
    this.t.fill_to_ocean = "close_ocean";
    this.t.water_fill_sound = "event:/SFX/NATURE/FillWaterTile";
    this.t.decrease_to_id = "pit_deep_ocean";
    this.t.increase_to_id = "pit_shallow_waters";
    this.t.can_be_set_on_fire = true;
    this.t.layer_type = TileLayerType.Ground;
    this.t.strength = 2;
    this.t.ground = true;
    TileLibrary.pit_shallow_waters = this.clone("pit_shallow_waters", "shallow_waters");
    this.t.can_be_frozen = false;
    this.t.setDrawLayer(TileZIndexes.pit_shallow_waters);
    this.t.drawPixel = true;
    this.t.color_hex = "#C1C1C1";
    this.t.liquid = false;
    this.t.ocean = false;
    this.t.walk_multiplier = 1f;
    this.t.can_be_filled_with_ocean = true;
    this.t.fill_to_ocean = "shallow_waters";
    this.t.water_fill_sound = "event:/SFX/NATURE/FillWaterTile";
    this.t.decrease_to_id = "pit_close_ocean";
    this.t.increase_to_id = "sand";
    this.t.freeze_to_id = string.Empty;
    this.t.can_be_set_on_fire = true;
    this.t.layer_type = TileLayerType.Ground;
    this.t.ground = true;
    this.t.strength = 2;
    TileType pAsset3 = new TileType();
    pAsset3.id = "border_pit";
    pAsset3.layer_type = TileLayerType.Ground;
    pAsset3.can_be_autotested = false;
    this.add(pAsset3);
    this.t.setDrawLayer(TileZIndexes.border_pit);
    TileType pAsset4 = new TileType();
    pAsset4.id = "border_water";
    pAsset4.layer_type = TileLayerType.Ground;
    pAsset4.can_be_autotested = false;
    this.add(pAsset4);
    this.t.setDrawLayer(TileZIndexes.border_water);
    TileType pAsset5 = new TileType();
    pAsset5.id = "border_water_runup";
    pAsset5.layer_type = TileLayerType.Ground;
    pAsset5.can_be_autotested = false;
    this.add(pAsset5);
    this.t.setDrawLayer(TileZIndexes.border_water_runup);
    TileType pAsset6 = new TileType();
    pAsset6.cost = 116;
    pAsset6.biome_build_check = true;
    pAsset6.id = "sand";
    pAsset6.sand = true;
    pAsset6.drawPixel = true;
    pAsset6.color_hex = "#F7E898";
    pAsset6.edge_color_hex = "#D8C08C";
    pAsset6.height_min = 98;
    pAsset6.decrease_to_id = "pit_shallow_waters";
    pAsset6.increase_to_id = "soil_low";
    pAsset6.ground = true;
    pAsset6.walk_multiplier = 0.5f;
    pAsset6.freeze_to_id = "snow_sand";
    pAsset6.creep_rank_type = TileRank.Low;
    pAsset6.can_be_set_on_fire = true;
    pAsset6.can_build_on = true;
    pAsset6.can_be_farm = true;
    TileLibrary.sand = this.add(pAsset6);
    this.t.ignore_walk_multiplier_if_tag = "walk_adaptation_sand";
    this.t.setBiome("biome_sand");
    this.t.used_in_generator = true;
    this.t.setDrawLayer(TileZIndexes.sand);
    this.t.biome_tags = AssetLibrary<TileType>.h<BiomeTag>(BiomeTag.Sand);
    TileType pAsset7 = new TileType();
    pAsset7.cost = 115;
    pAsset7.drawPixel = true;
    pAsset7.id = "soil_low";
    pAsset7.color_hex = "#E2934B";
    pAsset7.height_min = 108;
    pAsset7.decrease_to_id = "sand";
    pAsset7.increase_to_id = "soil_high";
    pAsset7.ground = true;
    pAsset7.can_be_biome = true;
    pAsset7.soil = true;
    pAsset7.freeze_to_id = "frozen_low";
    pAsset7.rank_type = TileRank.Low;
    pAsset7.creep_rank_type = TileRank.Low;
    pAsset7.can_be_farm = true;
    pAsset7.can_build_on = true;
    pAsset7.can_be_set_on_fire = true;
    pAsset7.used_in_generator = true;
    pAsset7.food_resource = "worms";
    pAsset7.biome_build_check = true;
    TileLibrary.soil_low = this.add(pAsset7);
    this.t.setDrawLayer(TileZIndexes.soil_low);
    this.t.biome_tags = AssetLibrary<TileType>.h<BiomeTag>(BiomeTag.Soil);
    TileType pAsset8 = new TileType();
    pAsset8.cost = 120;
    pAsset8.drawPixel = true;
    pAsset8.id = "soil_high";
    pAsset8.color_hex = "#B66F3A";
    pAsset8.height_min = 128 /*0x80*/;
    pAsset8.additional_height = new int[8]
    {
      15,
      16 /*0x10*/,
      17,
      14,
      13,
      12,
      11,
      10
    };
    pAsset8.decrease_to_id = "soil_low";
    pAsset8.increase_to_id = "hills";
    pAsset8.ground = true;
    pAsset8.rank_type = TileRank.High;
    pAsset8.creep_rank_type = TileRank.High;
    pAsset8.can_be_biome = true;
    pAsset8.soil = true;
    pAsset8.freeze_to_id = "frozen_high";
    pAsset8.can_be_farm = true;
    pAsset8.can_build_on = true;
    pAsset8.can_be_set_on_fire = true;
    pAsset8.used_in_generator = true;
    pAsset8.food_resource = "worms";
    pAsset8.biome_build_check = true;
    TileLibrary.soil_high = this.add(pAsset8);
    this.t.setDrawLayer(TileZIndexes.soil_high);
    this.t.biome_tags = AssetLibrary<TileType>.h<BiomeTag>(BiomeTag.Soil);
    TileType pAsset9 = new TileType();
    pAsset9.cost = 100;
    pAsset9.drawPixel = true;
    pAsset9.id = "lava0";
    pAsset9.color_hex = "#F62D14";
    pAsset9.decrease_to_id = "sand";
    pAsset9.increase_to_id = "hills";
    pAsset9.liquid = true;
    pAsset9.walk_multiplier = 0.2f;
    pAsset9.damage_units = true;
    pAsset9.damage = 150;
    pAsset9.lava = true;
    pAsset9.lava_level = 0;
    pAsset9.strength = 0;
    pAsset9.layer_type = TileLayerType.Lava;
    pAsset9.can_be_frozen = false;
    pAsset9.material = "mat_world_object_lit";
    TileLibrary.lava0 = this.add(pAsset9);
    this.t.lava_increase = "lava1";
    this.t.lava_change_state_after = 30;
    this.t.step_action = new TileStepAction(TileActionLibrary.setUnitOnFire);
    this.t.step_action_chance = 0.9f;
    this.t.setDrawLayer(TileZIndexes.lava0);
    TileLibrary.lava1 = this.clone("lava1", "lava0");
    this.t.setDrawLayer(TileZIndexes.lava1);
    this.t.color_hex = "#FF6700";
    this.t.step_action = new TileStepAction(TileActionLibrary.setUnitOnFire);
    this.t.step_action_chance = 0.9f;
    this.t.lava_level = 1;
    this.t.lava_decrease = "lava0";
    this.t.lava_increase = "lava2";
    this.t.lava_change_state_after = 10;
    TileLibrary.lava2 = this.clone("lava2", "lava0");
    this.t.setDrawLayer(TileZIndexes.lava2);
    this.t.color_hex = "#FFAC00";
    this.t.step_action = new TileStepAction(TileActionLibrary.setUnitOnFire);
    this.t.step_action_chance = 0.9f;
    this.t.lava_level = 2;
    this.t.lava_decrease = "lava1";
    this.t.lava_increase = "lava3";
    this.t.lava_change_state_after = 10;
    TileLibrary.lava3 = this.clone("lava3", "lava0");
    this.t.setDrawLayer(TileZIndexes.lava3);
    this.t.color_hex = "#FFDE00";
    this.t.step_action = new TileStepAction(TileActionLibrary.setUnitOnFire);
    this.t.step_action_chance = 0.9f;
    this.t.lava_level = 3;
    this.t.lava_decrease = "lava2";
    this.t.lava_increase = string.Empty;
    this.t.lava_change_state_after = 3;
    TileType pAsset10 = new TileType();
    pAsset10.cost = 140;
    pAsset10.drawPixel = true;
    pAsset10.id = "hills";
    pAsset10.color_hex = "#5B5E5C";
    pAsset10.height_min = 199;
    pAsset10.rocks = true;
    pAsset10.ground = true;
    pAsset10.edge_hills = true;
    pAsset10.additional_height = new int[2]{ 2, -6 };
    pAsset10.decrease_to_id = "soil_high";
    pAsset10.increase_to_id = "mountains";
    pAsset10.freeze_to_id = "snow_hills";
    pAsset10.can_be_set_on_fire = true;
    TileLibrary.hills = this.add(pAsset10);
    this.t.setBiome("biome_hill");
    this.t.biome_tags = AssetLibrary<TileType>.h<BiomeTag>(BiomeTag.Hills);
    this.t.hold_lava = true;
    this.t.used_in_generator = true;
    this.t.setDrawLayer(TileZIndexes.hills);
    TileType pAsset11 = new TileType();
    pAsset11.cost = 160 /*0xA0*/;
    pAsset11.drawPixel = true;
    pAsset11.id = "mountains";
    pAsset11.color_hex = "#414545";
    pAsset11.height_min = 210;
    pAsset11.rocks = true;
    pAsset11.mountains = true;
    pAsset11.edge_mountains = true;
    pAsset11.additional_height = new int[2]{ 2, 4 };
    pAsset11.decrease_to_id = "hills";
    pAsset11.increase_to_id = "summit";
    pAsset11.walk_multiplier = 0.5f;
    pAsset11.freeze_to_id = "snow_block";
    pAsset11.can_be_set_on_fire = true;
    pAsset11.layer_type = TileLayerType.Block;
    pAsset11.block = true;
    pAsset11.block_height = 3f;
    pAsset11.force_edge_variation = true;
    pAsset11.force_edge_variation_frame = 2;
    TileLibrary.mountains = this.add(pAsset11);
    this.t.hold_lava = true;
    this.t.used_in_generator = true;
    this.t.setDrawLayer(TileZIndexes.mountains);
    TileType pAsset12 = new TileType();
    pAsset12.cost = 160 /*0xA0*/;
    pAsset12.drawPixel = true;
    pAsset12.id = "summit";
    pAsset12.color_hex = "#333333";
    pAsset12.height_min = 230;
    pAsset12.rocks = true;
    pAsset12.mountains = true;
    pAsset12.edge_mountains = true;
    pAsset12.additional_height = new int[2]{ 2, 4 };
    pAsset12.decrease_to_id = "mountains";
    pAsset12.walk_multiplier = 0.5f;
    pAsset12.freeze_to_id = "snow_summit";
    pAsset12.can_be_set_on_fire = true;
    pAsset12.layer_type = TileLayerType.Block;
    pAsset12.block = true;
    pAsset12.block_height = 5f;
    pAsset12.force_edge_variation = true;
    pAsset12.force_edge_variation_frame = 2;
    TileLibrary.summit = this.add(pAsset12);
    this.t.summit = true;
    this.t.hold_lava = true;
    this.t.used_in_generator = true;
    this.t.setDrawLayer(TileZIndexes.summit);
    TileType pAsset13 = new TileType();
    pAsset13.cost = 10;
    pAsset13.drawPixel = true;
    pAsset13.grey_goo = true;
    pAsset13.id = "grey_goo";
    pAsset13.color_hex = "#5D6191";
    pAsset13.decrease_to_id = "pit_deep_ocean";
    pAsset13.burnable = true;
    pAsset13.ground = false;
    pAsset13.walk_multiplier = 0.1f;
    pAsset13.damage_units = true;
    pAsset13.damage = 200;
    pAsset13.strength = 0;
    pAsset13.life = true;
    pAsset13.can_be_frozen = false;
    pAsset13.layer_type = TileLayerType.Goo;
    TileLibrary.grey_goo = this.add(pAsset13);
    this.t.setDrawLayer(TileZIndexes.grey_goo);
    TileLibrary.lava_types = new List<TileType>()
    {
      TileLibrary.lava0,
      TileLibrary.lava1,
      TileLibrary.lava2,
      TileLibrary.lava3
    };
  }

  private void loadTileSprites()
  {
    foreach (TileType pType in this.list)
      this.loadSpritesForTile(pType);
  }

  private void loadSpritesForTile(TileType pType)
  {
    Sprite[] spriteList = SpriteTextureLoader.getSpriteList("tiles/" + pType.id);
    if (spriteList == null || spriteList.Length == 0)
      return;
    pType.sprites = new TileSprites();
    foreach (Sprite pSprite in spriteList)
      pType.sprites.addVariation(pSprite, pType.id);
  }

  public TileType getGen(string pID)
  {
    return !this.dict.ContainsKey(pID) ? (TileType) null : this.dict[pID];
  }

  public override TileType add(TileType pAsset)
  {
    pAsset.index_id = TileTypeBase.last_index_id++;
    TileLibrary.array_tiles[pAsset.index_id] = (TileTypeBase) pAsset;
    return base.add(pAsset);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    using (ListPool<TileType> pList = new ListPool<TileType>())
    {
      foreach (TileType tileType in this.list)
      {
        if (tileType.used_in_generator)
          pList.Add(tileType);
      }
      this.setListTo(DepthGeneratorType.Generator);
      for (int pHeight = 0; pHeight < this._depth_list_generator.Length; ++pHeight)
        this._depth_list_generator[pHeight] = this.getTypeByDepth(pHeight, (IReadOnlyList<TileType>) pList);
      this.setListTo(DepthGeneratorType.Gameplay);
      for (int pHeight = 0; pHeight < this._depth_list_gameplay.Length; ++pHeight)
        this._depth_list_gameplay[pHeight] = this.getTypeByDepth(pHeight, (IReadOnlyList<TileType>) this.list);
      this.setListTo(DepthGeneratorType.Generator);
      foreach (TileType tileType in this.list)
      {
        tileType.decrease_to = this.getGen(tileType.decrease_to_id);
        tileType.increase_to = this.getGen(tileType.increase_to_id);
      }
      this.loadTileSprites();
      foreach (TileTypeBase tileTypeBase in this.list)
      {
        if (!string.IsNullOrEmpty(tileTypeBase.biome_id))
          tileTypeBase.biome_asset = AssetManager.biome_library.get(tileTypeBase.biome_id);
      }
    }
  }

  public void setListTo(DepthGeneratorType pVal)
  {
    if (pVal != DepthGeneratorType.Generator)
    {
      if (pVal != DepthGeneratorType.Gameplay)
        return;
      this._depth_list = this._depth_list_gameplay;
    }
    else
      this._depth_list = this._depth_list_generator;
  }

  public TileType getTypeByDepth(int pHeight, IReadOnlyList<TileType> pList)
  {
    TileType typeByDepth = (TileType) null;
    for (int index = 0; index < pList.Count; ++index)
    {
      TileType p = pList[index];
      if (p.height_min != -1 && (typeByDepth == null || pHeight >= p.height_min))
        typeByDepth = p;
    }
    return typeByDepth;
  }

  public override TileType clone(string pNew, string pFrom)
  {
    TileType tileType = base.clone(pNew, pFrom);
    tileType.can_be_farm = false;
    tileType.used_in_generator = false;
    return tileType;
  }

  public TileType getTypeByDepth(WorldTile pWorldTile) => this._depth_list[pWorldTile.Height];
}
