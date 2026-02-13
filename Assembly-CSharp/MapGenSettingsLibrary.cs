// Decompiled with JetBrains decompiler
// Type: MapGenSettingsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class MapGenSettingsLibrary : AssetLibrary<MapGenSettingsAsset>
{
  public override void init()
  {
    base.init();
    MapGenSettingsAsset pAsset1 = new MapGenSettingsAsset();
    pAsset1.id = "gen_perlin_scale_stage_1";
    pAsset1.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_perlin_scale_stage_1);
    pAsset1.max_value = 30;
    pAsset1.action_get = (MapGenSettingsDelegateGet) (() => this.gen_values.perlin_scale_stage_1);
    pAsset1.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.perlin_scale_stage_1 = pValue);
    this.add(pAsset1);
    MapGenSettingsAsset pAsset2 = new MapGenSettingsAsset();
    pAsset2.id = "gen_perlin_scale_stage_2";
    pAsset2.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_perlin_scale_stage_1);
    pAsset2.max_value = 30;
    pAsset2.action_get = (MapGenSettingsDelegateGet) (() => this.gen_values.perlin_scale_stage_2);
    pAsset2.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.perlin_scale_stage_2 = pValue);
    this.add(pAsset2);
    MapGenSettingsAsset pAsset3 = new MapGenSettingsAsset();
    pAsset3.id = "gen_perlin_scale_stage_3";
    pAsset3.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_perlin_scale_stage_1);
    pAsset3.max_value = 30;
    pAsset3.action_get = (MapGenSettingsDelegateGet) (() => this.gen_values.perlin_scale_stage_3);
    pAsset3.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.perlin_scale_stage_3 = pValue);
    this.add(pAsset3);
    MapGenSettingsAsset pAsset4 = new MapGenSettingsAsset();
    pAsset4.id = "gen_random_shapes";
    pAsset4.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_random_shapes);
    pAsset4.max_value = 40;
    pAsset4.action_get = (MapGenSettingsDelegateGet) (() => this.gen_values.random_shapes_amount);
    pAsset4.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.random_shapes_amount = pValue);
    this.add(pAsset4);
    MapGenSettingsAsset pAsset5 = new MapGenSettingsAsset();
    pAsset5.id = "gen_cubicles_sizes";
    pAsset5.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_cubicles);
    pAsset5.min_value = 2;
    pAsset5.max_value = 15;
    pAsset5.action_get = (MapGenSettingsDelegateGet) (() => this.gen_values.cubicle_size);
    pAsset5.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.cubicle_size = pValue);
    this.add(pAsset5);
    MapGenSettingsAsset pAsset6 = new MapGenSettingsAsset();
    pAsset6.id = "gen_random_biomes";
    pAsset6.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_random_biomes);
    pAsset6.is_switch = true;
    pAsset6.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.random_biomes ? 0 : 1);
    pAsset6.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.random_biomes = pValue == 1);
    this.add(pAsset6);
    MapGenSettingsAsset pAsset7 = new MapGenSettingsAsset();
    pAsset7.id = "gen_mountain_edges";
    pAsset7.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_mountain_edges);
    pAsset7.is_switch = true;
    pAsset7.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.add_mountain_edges ? 0 : 1);
    pAsset7.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.add_mountain_edges = pValue == 1);
    this.add(pAsset7);
    MapGenSettingsAsset pAsset8 = new MapGenSettingsAsset();
    pAsset8.id = "gen_add_vegetation";
    pAsset8.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_random_vegetation);
    pAsset8.is_switch = true;
    pAsset8.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.add_vegetation ? 0 : 1);
    pAsset8.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.add_vegetation = pValue == 1);
    this.add(pAsset8);
    MapGenSettingsAsset pAsset9 = new MapGenSettingsAsset();
    pAsset9.id = "gen_add_resources";
    pAsset9.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_random_resources);
    pAsset9.is_switch = true;
    pAsset9.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.add_resources ? 0 : 1);
    pAsset9.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.add_resources = pValue == 1);
    this.add(pAsset9);
    MapGenSettingsAsset pAsset10 = new MapGenSettingsAsset();
    pAsset10.id = "gen_add_center_lake";
    pAsset10.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_center_lake);
    pAsset10.is_switch = true;
    pAsset10.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.add_center_lake ? 0 : 1);
    pAsset10.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.add_center_lake = pValue == 1);
    this.add(pAsset10);
    MapGenSettingsAsset pAsset11 = new MapGenSettingsAsset();
    pAsset11.id = "gen_add_center_land";
    pAsset11.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_center_land);
    pAsset11.is_switch = true;
    pAsset11.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.add_center_gradient_land ? 0 : 1);
    pAsset11.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.add_center_gradient_land = pValue == 1);
    this.add(pAsset11);
    MapGenSettingsAsset pAsset12 = new MapGenSettingsAsset();
    pAsset12.id = "gen_round_edges";
    pAsset12.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_round_edges);
    pAsset12.is_switch = true;
    pAsset12.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.gradient_round_edges ? 0 : 1);
    pAsset12.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.gradient_round_edges = pValue == 1);
    this.add(pAsset12);
    MapGenSettingsAsset pAsset13 = new MapGenSettingsAsset();
    pAsset13.id = "gen_square_edges";
    pAsset13.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_square_edges);
    pAsset13.is_switch = true;
    pAsset13.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.square_edges ? 0 : 1);
    pAsset13.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.square_edges = pValue == 1);
    this.add(pAsset13);
    MapGenSettingsAsset pAsset14 = new MapGenSettingsAsset();
    pAsset14.id = "gen_ring_effect";
    pAsset14.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_ring_effect);
    pAsset14.is_switch = true;
    pAsset14.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.ring_effect ? 0 : 1);
    pAsset14.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.ring_effect = pValue == 1);
    this.add(pAsset14);
    MapGenSettingsAsset pAsset15 = new MapGenSettingsAsset();
    pAsset15.id = "gen_low_ground";
    pAsset15.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_low_ground);
    pAsset15.is_switch = true;
    pAsset15.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.low_ground = pValue == 1);
    pAsset15.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.low_ground ? 0 : 1);
    this.add(pAsset15);
    MapGenSettingsAsset pAsset16 = new MapGenSettingsAsset();
    pAsset16.id = "gen_high_ground";
    pAsset16.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_high_ground);
    pAsset16.is_switch = true;
    pAsset16.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.high_ground = pValue == 1);
    pAsset16.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.high_ground ? 0 : 1);
    this.add(pAsset16);
    MapGenSettingsAsset pAsset17 = new MapGenSettingsAsset();
    pAsset17.id = "gen_remove_mountains";
    pAsset17.allowed_check = (MapGenSettingsDelegateBool) (pAsset => pAsset.allow_edit_remove_mountains);
    pAsset17.is_switch = true;
    pAsset17.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.remove_mountains = pValue == 1);
    pAsset17.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.remove_mountains ? 0 : 1);
    this.add(pAsset17);
    MapGenSettingsAsset pAsset18 = new MapGenSettingsAsset();
    pAsset18.id = "gen_forbidden_knowledge";
    pAsset18.allowed_check = (MapGenSettingsDelegateBool) (_ => Config.hasPremium && AchievementLibrary.cursed_world.isUnlocked());
    pAsset18.is_switch = true;
    pAsset18.action_get = (MapGenSettingsDelegateGet) (() => !this.gen_values.forbidden_knowledge_start ? 0 : 1);
    pAsset18.action_set = (MapGenSettingsDelegateSet) (pValue => this.gen_values.forbidden_knowledge_start = pValue == 1);
    this.add(pAsset18);
  }

  public override MapGenSettingsAsset add(MapGenSettingsAsset pAsset1)
  {
    pAsset1.increase = (MapGenSettingsDelegate) (pAsset2 =>
    {
      int num1 = pAsset2.action_get();
      int num2 = 1;
      if (HotkeyLibrary.many_mod.isHolding())
        num2 = 5;
      int pValue = num1 + num2;
      if (pValue > pAsset2.max_value)
        pValue = pAsset2.min_value;
      pAsset2.action_set(pValue);
    });
    pAsset1.decrease = (MapGenSettingsDelegate) (pAsset3 =>
    {
      int num3 = pAsset3.action_get();
      int num4 = 1;
      if (HotkeyLibrary.many_mod.isHolding())
        num4 = 5;
      int pValue = num3 - num4;
      if (pValue < pAsset3.min_value)
        pValue = pAsset3.max_value;
      pAsset3.action_set(pValue);
    });
    pAsset1.action_switch = (MapGenSettingsDelegateSwitch) (pAsset4 =>
    {
      int pValue = pAsset4.action_get() != 0 ? 0 : 1;
      pAsset4.action_set(pValue);
    });
    return base.add(pAsset1);
  }

  private MapGenValues gen_values
  {
    get => AssetManager.map_gen_templates.get(Config.current_map_template).values;
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (MapGenSettingsAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}
