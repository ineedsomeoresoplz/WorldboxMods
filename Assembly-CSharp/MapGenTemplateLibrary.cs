// Decompiled with JetBrains decompiler
// Type: MapGenTemplateLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MapGenTemplateLibrary : AssetLibrary<MapGenTemplate>
{
  private Dictionary<string, MapGenValues> default_values = new Dictionary<string, MapGenValues>();

  public override void init()
  {
    base.init();
    MapGenTemplate pAsset1 = new MapGenTemplate();
    pAsset1.id = "continent";
    pAsset1.freeze_mountains = true;
    pAsset1.path_icon = "ui/new_world_templates_icons/template_continent";
    this.add(pAsset1);
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.perlin_noise_stage_3 = true;
    this.t.values.gradient_round_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.ring_effect = true;
    this.t.values.random_shapes_amount = 5;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset2 = new MapGenTemplate();
    pAsset2.id = "box_world";
    pAsset2.freeze_mountains = true;
    pAsset2.path_icon = "ui/new_world_templates_icons/template_box_world";
    this.add(pAsset2);
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.perlin_noise_stage_3 = true;
    this.t.values.add_mountain_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.ring_effect = true;
    this.t.values.random_shapes_amount = 5;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset3 = new MapGenTemplate();
    pAsset3.id = "islands";
    pAsset3.freeze_mountains = true;
    pAsset3.path_icon = "ui/new_world_templates_icons/template_islands";
    this.add(pAsset3);
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.perlin_noise_stage_3 = true;
    this.t.values.gradient_round_edges = true;
    this.t.values.ring_effect = true;
    this.t.values.random_shapes_amount = 5;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset4 = new MapGenTemplate();
    pAsset4.id = "toast";
    pAsset4.path_icon = "ui/new_world_templates_icons/template_toast";
    this.add(pAsset4);
    this.t.values.square_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.perlin_noise_stage_3 = true;
    this.t.values.remove_mountains = true;
    this.t.values.random_shapes_amount = 5;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset5 = new MapGenTemplate();
    pAsset5.id = "pancake";
    pAsset5.path_icon = "ui/new_world_templates_icons/template_pancake";
    this.add(pAsset5);
    this.t.values.gradient_round_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.perlin_noise_stage_3 = true;
    this.t.values.remove_mountains = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset6 = new MapGenTemplate();
    pAsset6.id = "boring_plains";
    pAsset6.path_icon = "ui/new_world_templates_icons/template_boring_plains";
    this.add(pAsset6);
    this.t.values.add_center_gradient_land = true;
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.perlin_noise_stage_3 = true;
    this.t.values.add_mountain_edges = true;
    this.t.values.remove_mountains = true;
    this.t.allow_edit_low_ground = false;
    this.t.allow_edit_high_ground = false;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    this.addReplaceOption(0, "shallow_waters", "soil_low");
    this.addReplaceOption(0, "close_ocean", "soil_low");
    this.addReplaceOption(0, "deep_ocean", "soil_high");
    MapGenTemplate pAsset7 = new MapGenTemplate();
    pAsset7.id = "checkerboard";
    pAsset7.special_checkerboard = true;
    pAsset7.path_icon = "ui/new_world_templates_icons/template_checkerboard";
    this.add(pAsset7);
    this.t.values.random_biomes = true;
    this.t.values.add_mountain_edges = true;
    this.t.values.remove_mountains = true;
    this.disableNormalSettings(this.t);
    this.t.allow_edit_random_biomes = true;
    this.t.allow_edit_random_resources = true;
    this.t.allow_edit_random_vegetation = true;
    MapGenTemplate pAsset8 = new MapGenTemplate();
    pAsset8.id = "cubicles";
    pAsset8.special_cubicles = true;
    pAsset8.allow_edit_cubicles = true;
    pAsset8.path_icon = "ui/new_world_templates_icons/template_cubicles";
    this.add(pAsset8);
    this.t.values.random_biomes = true;
    this.t.values.add_mountain_edges = true;
    this.t.values.remove_mountains = true;
    this.t.values.cubicle_size = 2;
    this.disableNormalSettings(this.t);
    this.t.allow_edit_cubicles = true;
    this.t.allow_edit_random_biomes = true;
    this.t.allow_edit_random_resources = true;
    this.t.allow_edit_random_vegetation = true;
    MapGenTemplate pAsset9 = new MapGenTemplate();
    pAsset9.id = "dormant_volcano";
    pAsset9.force_height_to = 125;
    pAsset9.path_icon = "ui/new_world_templates_icons/template_dormant_volcano";
    this.add(pAsset9);
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.random_biomes = false;
    this.t.values.gradient_round_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset10 = new MapGenTemplate();
    pAsset10.id = "cheese";
    pAsset10.force_height_to = 120;
    pAsset10.path_icon = "ui/new_world_templates_icons/template_cheese";
    this.add(pAsset10);
    this.t.values.square_edges = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.t.values.remove_mountains = true;
    this.t.values.add_center_gradient_land = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "shallow_waters");
    this.addReplaceOption(170, "sand", "shallow_waters");
    this.addReplaceOption(150, "soil_high", "sand");
    this.addReplaceOption(130, "soil_low", "sand");
    this.addReplaceOption(80 /*0x50*/, "soil_high", "soil_low");
    MapGenTemplate pAsset11 = new MapGenTemplate();
    pAsset11.id = "bad_apple";
    pAsset11.path_icon = "ui/new_world_templates_icons/template_bad_apple";
    this.add(pAsset11);
    this.t.values.gradient_round_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.center_gradient_mountains = true;
    this.t.values.main_perlin_noise_stage = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset12 = new MapGenTemplate();
    pAsset12.id = "donut";
    pAsset12.force_height_to = 125;
    pAsset12.path_icon = "ui/new_world_templates_icons/template_donut";
    this.add(pAsset12);
    this.t.values.gradient_round_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.add_center_lake = true;
    this.t.values.perlin_noise_stage_2 = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset13 = new MapGenTemplate();
    pAsset13.id = "lasagna";
    pAsset13.path_icon = "ui/new_world_templates_icons/template_lasagna";
    this.add(pAsset13);
    this.t.values.square_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.random_shapes_amount = 5;
    this.t.values.low_ground = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset14 = new MapGenTemplate();
    pAsset14.id = "chaos_pearl";
    pAsset14.path_icon = "ui/new_world_templates_icons/template_chaos_pearl";
    this.add(pAsset14);
    this.t.values.gradient_round_edges = true;
    this.t.values.add_center_gradient_land = true;
    this.t.values.main_perlin_noise_stage = true;
    this.t.values.low_ground = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset15 = new MapGenTemplate();
    pAsset15.id = "anthill";
    pAsset15.special_anthill = true;
    pAsset15.path_icon = "ui/new_world_templates_icons/template_anthill";
    this.add(pAsset15);
    this.disableNormalSettings(this.t);
    this.t.allow_edit_random_biomes = true;
    this.t.allow_edit_random_resources = true;
    this.t.allow_edit_random_vegetation = true;
    this.newReplaceContainer(15);
    this.addReplaceOption(170, "soil_high", "soil_low");
    MapGenTemplate pAsset16 = new MapGenTemplate();
    pAsset16.id = "empty";
    pAsset16.show_reset_button = false;
    pAsset16.path_icon = "ui/new_world_templates_icons/template_empty";
    this.add(pAsset16);
    this.disableNormalSettings(this.t);
    this.createDefaultBackupValues();
  }

  public void createDefaultBackupValues()
  {
    foreach (MapGenTemplate mapGenTemplate in this.list)
    {
      MapGenValues mapGenValues = JsonUtility.FromJson<MapGenValues>(JsonUtility.ToJson((object) mapGenTemplate.values));
      this.default_values.Add(mapGenTemplate.id, mapGenValues);
    }
  }

  public void resetTemplateValues(MapGenTemplate pAsset)
  {
    MapGenValues mapGenValues = JsonUtility.FromJson<MapGenValues>(JsonUtility.ToJson((object) this.default_values[pAsset.id]));
    pAsset.values = mapGenValues;
  }

  public void disableNormalSettings(MapGenTemplate pAsset)
  {
    pAsset.allow_edit_size = false;
    pAsset.allow_edit_random_shapes = false;
    pAsset.allow_edit_random_biomes = false;
    pAsset.allow_edit_perlin_scale_stage_1 = false;
    pAsset.allow_edit_perlin_scale_stage_2 = false;
    pAsset.allow_edit_perlin_scale_stage_3 = false;
    pAsset.allow_edit_mountain_edges = false;
    pAsset.allow_edit_random_vegetation = false;
    pAsset.allow_edit_random_resources = false;
    pAsset.allow_edit_center_lake = false;
    pAsset.allow_edit_round_edges = false;
    pAsset.allow_edit_square_edges = false;
    pAsset.allow_edit_ring_effect = false;
    pAsset.allow_edit_center_land = false;
    pAsset.allow_edit_low_ground = false;
    pAsset.allow_edit_high_ground = false;
    pAsset.allow_edit_remove_mountains = false;
  }

  private void newReplaceContainer(int pScale = 1)
  {
    this.t.perlin_replace.Add(new PerlinReplaceContainer()
    {
      scale = pScale
    });
  }

  private void addReplaceOption(int pHeight, string pFrom, string pTo)
  {
    this.t.perlin_replace[this.t.perlin_replace.Count - 1].options.Add(new PerlinReplaceOption()
    {
      replace_height_value = pHeight,
      from = pFrom,
      to = pTo
    });
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (MapGenTemplate pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }
}
