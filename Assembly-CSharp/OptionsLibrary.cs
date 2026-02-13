// Decompiled with JetBrains decompiler
// Type: OptionsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using SleekRender;
using UnityEngine;

#nullable disable
public class OptionsLibrary : AssetLibrary<OptionAsset>
{
  public override void init()
  {
    base.init();
    this.initGameplayOptions();
    this.initLayerOptions();
    this.initAppOptions();
    this.initQualityOptions();
    this.initOtherOptions();
    this.initHotkeyOptions();
  }

  private void initLayerOptions()
  {
    OptionAsset pAsset1 = new OptionAsset();
    pAsset1.id = "map_layers";
    pAsset1.default_bool = true;
    pAsset1.type = OptionType.Bool;
    this.add(pAsset1);
    OptionAsset pAsset2 = new OptionAsset();
    pAsset2.id = "map_species_families";
    pAsset2.default_bool = true;
    pAsset2.type = OptionType.Bool;
    this.add(pAsset2);
    OptionAsset pAsset3 = new OptionAsset();
    pAsset3.id = "map_kingdom_layer";
    pAsset3.default_int = 0;
    pAsset3.max_value = 1;
    pAsset3.multi_toggle = true;
    pAsset3.type = OptionType.Bool;
    pAsset3.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_borders", "ui_zone_mode_units");
    this.add(pAsset3);
    OptionAsset pAsset4 = new OptionAsset();
    pAsset4.id = "map_city_layer";
    pAsset4.default_int = 1;
    pAsset4.max_value = 1;
    pAsset4.multi_toggle = true;
    pAsset4.type = OptionType.Bool;
    pAsset4.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_borders", "ui_zone_mode_units");
    this.add(pAsset4);
    OptionAsset pAsset5 = new OptionAsset();
    pAsset5.id = "map_clan_layer";
    pAsset5.default_int = 0;
    pAsset5.max_value = 2;
    pAsset5.multi_toggle = true;
    pAsset5.type = OptionType.Bool;
    pAsset5.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_kingdoms", "ui_zone_mode_cities", "ui_zone_mode_units");
    this.add(pAsset5);
    OptionAsset pAsset6 = new OptionAsset();
    pAsset6.id = "map_religion_layer";
    pAsset6.default_int = 2;
    pAsset6.max_value = 2;
    pAsset6.multi_toggle = true;
    pAsset6.type = OptionType.Bool;
    pAsset6.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_kingdoms", "ui_zone_mode_cities", "ui_zone_mode_units");
    this.add(pAsset6);
    OptionAsset pAsset7 = new OptionAsset();
    pAsset7.id = "map_culture_layer";
    pAsset7.default_int = 2;
    pAsset7.max_value = 2;
    pAsset7.multi_toggle = true;
    pAsset7.type = OptionType.Bool;
    pAsset7.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_kingdoms", "ui_zone_mode_cities", "ui_zone_mode_units");
    this.add(pAsset7);
    OptionAsset pAsset8 = new OptionAsset();
    pAsset8.id = "map_subspecies_layer";
    pAsset8.default_int = 2;
    pAsset8.max_value = 2;
    pAsset8.multi_toggle = true;
    pAsset8.type = OptionType.Bool;
    pAsset8.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_kingdoms", "ui_zone_mode_cities", "ui_zone_mode_units");
    this.add(pAsset8);
    OptionAsset pAsset9 = new OptionAsset();
    pAsset9.id = "map_family_layer";
    pAsset9.default_int = 2;
    pAsset9.max_value = 2;
    pAsset9.multi_toggle = true;
    pAsset9.type = OptionType.Bool;
    pAsset9.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_kingdoms", "ui_zone_mode_cities", "ui_zone_mode_units");
    this.add(pAsset9);
    OptionAsset pAsset10 = new OptionAsset();
    pAsset10.id = "map_language_layer";
    pAsset10.default_int = 2;
    pAsset10.max_value = 2;
    pAsset10.multi_toggle = true;
    pAsset10.type = OptionType.Bool;
    pAsset10.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_zone_mode_kingdoms", "ui_zone_mode_cities", "ui_zone_mode_units");
    this.add(pAsset10);
    OptionAsset pAsset11 = new OptionAsset();
    pAsset11.id = "map_alliance_layer";
    pAsset11.type = OptionType.Bool;
    this.add(pAsset11);
    OptionAsset pAsset12 = new OptionAsset();
    pAsset12.id = "map_army_layer";
    pAsset12.default_bool = false;
    pAsset12.type = OptionType.Bool;
    this.add(pAsset12);
  }

  private void initGameplayOptions()
  {
    OptionAsset pAsset1 = new OptionAsset();
    pAsset1.id = "map_names";
    pAsset1.default_bool = true;
    pAsset1.default_int = 0;
    pAsset1.max_value = 1;
    pAsset1.multi_toggle = true;
    pAsset1.type = OptionType.Bool;
    pAsset1.locale_options_ids = AssetLibrary<OptionAsset>.a<string>("ui_map_names_normal", "ui_map_names_banners_only");
    this.add(pAsset1);
    OptionAsset pAsset2 = new OptionAsset();
    pAsset2.id = "map_kings_leaders";
    pAsset2.default_bool = true;
    pAsset2.type = OptionType.Bool;
    this.add(pAsset2);
    OptionAsset pAsset3 = new OptionAsset();
    pAsset3.id = "marks_favorites";
    pAsset3.default_bool = true;
    pAsset3.type = OptionType.Bool;
    this.add(pAsset3);
    OptionAsset pAsset4 = new OptionAsset();
    pAsset4.id = "marks_favorite_items";
    pAsset4.default_bool = true;
    pAsset4.type = OptionType.Bool;
    this.add(pAsset4);
    OptionAsset pAsset5 = new OptionAsset();
    pAsset5.id = "marks_plots";
    pAsset5.default_bool = true;
    pAsset5.type = OptionType.Bool;
    this.add(pAsset5);
    OptionAsset pAsset6 = new OptionAsset();
    pAsset6.id = "marks_wars";
    pAsset6.default_bool = false;
    pAsset6.type = OptionType.Bool;
    this.add(pAsset6);
    OptionAsset pAsset7 = new OptionAsset();
    pAsset7.id = "marks_armies";
    pAsset7.default_bool = true;
    pAsset7.type = OptionType.Bool;
    this.add(pAsset7);
    OptionAsset pAsset8 = new OptionAsset();
    pAsset8.id = "only_favorited_meta";
    pAsset8.default_bool = false;
    pAsset8.reset_to_default_on_launch = true;
    pAsset8.type = OptionType.Bool;
    this.add(pAsset8);
    OptionAsset pAsset9 = new OptionAsset();
    pAsset9.id = "money_flow";
    pAsset9.default_bool = false;
    pAsset9.type = OptionType.Bool;
    this.add(pAsset9);
    OptionAsset pAsset10 = new OptionAsset();
    pAsset10.id = "icons_happiness";
    pAsset10.default_bool = false;
    pAsset10.type = OptionType.Bool;
    this.add(pAsset10);
    OptionAsset pAsset11 = new OptionAsset();
    pAsset11.id = "icons_tasks";
    pAsset11.default_bool = false;
    pAsset11.type = OptionType.Bool;
    this.add(pAsset11);
    OptionAsset pAsset12 = new OptionAsset();
    pAsset12.id = "talk_bubbles";
    pAsset12.default_bool = true;
    pAsset12.type = OptionType.Bool;
    this.add(pAsset12);
    OptionAsset pAsset13 = new OptionAsset();
    pAsset13.id = "meta_conversions";
    pAsset13.default_bool = true;
    pAsset13.type = OptionType.Bool;
    this.add(pAsset13);
    OptionAsset pAsset14 = new OptionAsset();
    pAsset14.id = "unit_metas";
    pAsset14.default_bool = false;
    pAsset14.type = OptionType.Bool;
    this.add(pAsset14);
    OptionAsset pAsset15 = new OptionAsset();
    pAsset15.id = "marks_battles";
    pAsset15.default_bool = true;
    pAsset15.type = OptionType.Bool;
    this.add(pAsset15);
    OptionAsset pAsset16 = new OptionAsset();
    pAsset16.id = "history_log";
    pAsset16.default_bool = true;
    pAsset16.type = OptionType.Bool;
    this.add(pAsset16);
    OptionAsset pAsset17 = new OptionAsset();
    pAsset17.id = "marks_boats";
    pAsset17.default_bool = true;
    pAsset17.type = OptionType.Bool;
    this.add(pAsset17);
    OptionAsset pAsset18 = new OptionAsset();
    pAsset18.id = "army_targets";
    pAsset18.default_bool = true;
    pAsset18.type = OptionType.Bool;
    this.add(pAsset18);
    OptionAsset pAsset19 = new OptionAsset();
    pAsset19.id = "tooltip_zones";
    pAsset19.default_bool = true;
    pAsset19.default_bool_mobile = false;
    pAsset19.type = OptionType.Bool;
    this.add(pAsset19);
    OptionAsset pAsset20 = new OptionAsset();
    pAsset20.id = "tooltip_units";
    pAsset20.default_bool = true;
    pAsset20.default_bool_mobile = false;
    pAsset20.type = OptionType.Bool;
    this.add(pAsset20);
    OptionAsset pAsset21 = new OptionAsset();
    pAsset21.id = "cursor_arrow_destination";
    pAsset21.default_bool = false;
    pAsset21.type = OptionType.Bool;
    this.add(pAsset21);
    OptionAsset pAsset22 = new OptionAsset();
    pAsset22.id = "cursor_arrow_house";
    pAsset22.default_bool = false;
    pAsset22.type = OptionType.Bool;
    this.add(pAsset22);
    OptionAsset pAsset23 = new OptionAsset();
    pAsset23.id = "cursor_arrow_lover";
    pAsset23.default_bool = false;
    pAsset23.type = OptionType.Bool;
    this.add(pAsset23);
    OptionAsset pAsset24 = new OptionAsset();
    pAsset24.id = "cursor_arrow_family";
    pAsset24.default_bool = false;
    pAsset24.type = OptionType.Bool;
    this.add(pAsset24);
    OptionAsset pAsset25 = new OptionAsset();
    pAsset25.id = "cursor_arrow_parents";
    pAsset25.default_bool = false;
    pAsset25.type = OptionType.Bool;
    this.add(pAsset25);
    OptionAsset pAsset26 = new OptionAsset();
    pAsset26.id = "cursor_arrow_kids";
    pAsset26.default_bool = false;
    pAsset26.type = OptionType.Bool;
    this.add(pAsset26);
    OptionAsset pAsset27 = new OptionAsset();
    pAsset27.id = "cursor_arrow_attack_target";
    pAsset27.default_bool = false;
    pAsset27.type = OptionType.Bool;
    this.add(pAsset27);
    OptionAsset pAsset28 = new OptionAsset();
    pAsset28.id = "highlight_kingdom_enemies";
    pAsset28.default_bool = true;
    pAsset28.type = OptionType.Bool;
    pAsset28.override_bool_mobile = true;
    pAsset28.default_bool_mobile = false;
    this.add(pAsset28);
  }

  private void initAppOptions()
  {
    OptionAsset pAsset1 = new OptionAsset();
    pAsset1.id = "autorotation";
    pAsset1.translation_key = "autorotation";
    pAsset1.translation_key_description = "option_description_autorotation";
    pAsset1.update_all_elements_after_click = true;
    pAsset1.default_bool = false;
    pAsset1.type = OptionType.Bool;
    pAsset1.action = (ActionOptionAsset) (pAsset => Config.setAutorotation(this.getSavedBool(pAsset.id)));
    this.add(pAsset1);
    OptionAsset pAsset2 = new OptionAsset();
    pAsset2.id = "portrait";
    pAsset2.translation_key = "portrait_mode";
    pAsset2.translation_key_description = "option_description_portrait";
    pAsset2.default_bool = true;
    pAsset2.type = OptionType.Bool;
    pAsset2.action = (ActionOptionAsset) (pAsset => Config.setPortrait(this.getSavedBool(pAsset.id)));
    this.add(pAsset2);
    OptionAsset pAsset3 = new OptionAsset();
    pAsset3.id = "fps_lock_30";
    pAsset3.translation_key_description = "option_description_fps_lock_30";
    pAsset3.default_bool = false;
    pAsset3.type = OptionType.Bool;
    pAsset3.update_all_elements_after_click = true;
    pAsset3.action = (ActionOptionAsset) (pAsset =>
    {
      int num;
      Config.fps_lock_30 = (num = this.getSavedBool(pAsset.id) ? 1 : 0) != 0;
      Application.targetFrameRate = !Config.fps_lock_30 ? 60 : 30;
      if (num == 0)
        return;
      this.forceDisableOption("vsync");
    });
    this.add(pAsset3);
    OptionAsset pAsset4 = new OptionAsset();
    pAsset4.id = "sound";
    pAsset4.translation_key = "sound_effects";
    pAsset4.translation_key_description = "option_description_sound_effects";
    pAsset4.default_bool = true;
    pAsset4.type = OptionType.Bool;
    this.add(pAsset4);
    OptionAsset pAsset5 = new OptionAsset();
    pAsset5.id = "volume_master_sound";
    pAsset5.translation_key_description = "option_description_volume_master_sound";
    pAsset5.default_int = 100;
    pAsset5.type = OptionType.Int;
    pAsset5.max_value = 100;
    pAsset5.min_value = 0;
    pAsset5.counter_percent = true;
    this.add(pAsset5);
    OptionAsset pAsset6 = new OptionAsset();
    pAsset6.id = "volume_music";
    pAsset6.translation_key_description = "option_description_volume_music";
    pAsset6.default_int = 100;
    pAsset6.type = OptionType.Int;
    pAsset6.max_value = 100;
    pAsset6.min_value = 0;
    pAsset6.counter_percent = true;
    this.add(pAsset6);
    OptionAsset pAsset7 = new OptionAsset();
    pAsset7.id = "volume_sound_effects";
    pAsset7.translation_key_description = "option_description_volume_sound_effects";
    pAsset7.default_int = 100;
    pAsset7.type = OptionType.Int;
    pAsset7.max_value = 100;
    pAsset7.min_value = 0;
    pAsset7.counter_percent = true;
    this.add(pAsset7);
    OptionAsset pAsset8 = new OptionAsset();
    pAsset8.id = "volume_ui";
    pAsset8.translation_key_description = "option_description_volume_ui";
    pAsset8.default_int = 100;
    pAsset8.type = OptionType.Int;
    pAsset8.max_value = 100;
    pAsset8.min_value = 0;
    pAsset8.counter_percent = true;
    this.add(pAsset8);
    OptionAsset pAsset9 = new OptionAsset();
    pAsset9.id = "music";
    pAsset9.translation_key_description = "option_description_music";
    pAsset9.translation_key = "music";
    pAsset9.default_bool = true;
    pAsset9.type = OptionType.Bool;
    this.add(pAsset9);
    OptionAsset pAsset10 = new OptionAsset();
    pAsset10.id = "vsync";
    pAsset10.translation_key = "vsync";
    pAsset10.translation_key_description = "option_description_vsync";
    pAsset10.default_bool = false;
    pAsset10.type = OptionType.Bool;
    pAsset10.update_all_elements_after_click = true;
    pAsset10.computer_only = true;
    pAsset10.action = (ActionOptionAsset) (pAsset =>
    {
      int num = this.getSavedBool(pAsset.id) ? 1 : 0;
      PlayerConfig.setVsync(num != 0);
      if (num == 0)
        return;
      this.forceDisableOption("fps_lock_30");
    });
    this.add(pAsset10);
    OptionAsset pAsset11 = new OptionAsset();
    pAsset11.id = "fullscreen";
    pAsset11.default_bool = true;
    pAsset11.translation_key_description = "option_description_fullscreen";
    pAsset11.type = OptionType.Bool;
    pAsset11.computer_only = true;
    pAsset11.action = (ActionOptionAsset) (pAsset =>
    {
      Config.full_screen = this.getSavedBool(pAsset.id);
      if (Config.full_screen)
      {
        Resolution currentResolution1 = Screen.currentResolution;
        int width = ((Resolution) ref currentResolution1).width;
        Resolution currentResolution2 = Screen.currentResolution;
        int height = ((Resolution) ref currentResolution2).height;
        currentResolution2 = Screen.currentResolution;
        RefreshRate refreshRateRatio = ((Resolution) ref currentResolution2).refreshRateRatio;
        int num = height;
        RefreshRate refreshRate = refreshRateRatio;
        Screen.SetResolution(width, num, (FullScreenMode) 1, refreshRate);
      }
      else
        Screen.fullScreen = false;
    });
    this.add(pAsset11);
    OptionAsset pAsset12 = new OptionAsset();
    pAsset12.id = "language";
    pAsset12.type = OptionType.String;
    pAsset12.default_string = "en";
    this.add(pAsset12);
    OptionAsset pAsset13 = new OptionAsset();
    pAsset13.id = "username";
    pAsset13.type = OptionType.String;
    pAsset13.default_string = "";
    pAsset13.has_locales = false;
    this.add(pAsset13);
    OptionAsset pAsset14 = new OptionAsset();
    pAsset14.id = "wbb_confirmed";
    pAsset14.type = OptionType.Bool;
    pAsset14.default_bool = false;
    pAsset14.has_locales = false;
    pAsset14.action = (ActionOptionAsset) (pAsset => Config.wbb_confirmed = this.getSavedBool(pAsset.id));
    this.add(pAsset14);
    OptionAsset pAsset15 = new OptionAsset();
    pAsset15.id = "ui_size_main";
    pAsset15.translation_key = "power_bar_size";
    pAsset15.translation_key_description = "option_description_power_bar_size";
    pAsset15.type = OptionType.Int;
    pAsset15.default_int = 100;
    pAsset15.max_value = 150;
    pAsset15.min_value = 50;
    pAsset15.action = (ActionOptionAsset) (_ =>
    {
      CanvasMain.instance.resizeMainUI();
      PowerButtonSelector.instance.showBarTemporary();
    });
    pAsset15.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset15);
    OptionAsset pAsset16 = new OptionAsset();
    pAsset16.id = "ui_size_windows";
    pAsset16.translation_key_description = "option_description_ui_windows_size";
    pAsset16.type = OptionType.Int;
    pAsset16.default_int = 100;
    pAsset16.max_value = 115;
    pAsset16.min_value = 30;
    pAsset16.action = (ActionOptionAsset) (_ =>
    {
      CanvasMain.instance.resizeWindowsUI();
      MetaSwitchManager.checkAndRefresh();
      HoveringBgIconManager.dropAll();
    });
    pAsset16.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset16);
    OptionAsset pAsset17 = new OptionAsset();
    pAsset17.id = "ui_size_tooltips";
    pAsset17.translation_key_description = "option_description_ui_tooltips_size";
    pAsset17.type = OptionType.Int;
    pAsset17.default_int = 100;
    pAsset17.max_value = 150;
    pAsset17.min_value = 30;
    pAsset17.action = (ActionOptionAsset) (_ => CanvasMain.instance.resizeTooltipUI());
    pAsset17.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset17);
    OptionAsset pAsset18 = new OptionAsset();
    pAsset18.id = "ui_size_map_names";
    pAsset18.translation_key_description = "option_description_ui_map_names_size";
    pAsset18.type = OptionType.Int;
    pAsset18.default_int = 70;
    pAsset18.max_value = 160 /*0xA0*/;
    pAsset18.min_value = 30;
    pAsset18.action = (ActionOptionAsset) (_ =>
    {
      CanvasMain.instance.resizeMapNames();
      World.world.nameplate_manager.clearCaches();
    });
    pAsset18.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset18);
    OptionAsset pAsset19 = new OptionAsset();
    pAsset19.id = "ui_map_border_opacity";
    pAsset19.translation_key_description = "option_description_map_border_opacity";
    pAsset19.type = OptionType.Int;
    pAsset19.default_int = 88;
    pAsset19.max_value = 100;
    pAsset19.min_value = 30;
    pAsset19.action = (ActionOptionAsset) (pAsset => World.world.zone_calculator.minimap_opacity = (float) this.getSavedInt(pAsset.id) / 100f);
    pAsset19.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset19);
    OptionAsset pAsset20 = new OptionAsset();
    pAsset20.id = "ui_map_border_brightness";
    pAsset20.translation_key_description = "option_description_map_border_brightness";
    pAsset20.type = OptionType.Int;
    pAsset20.default_int = 100;
    pAsset20.max_value = 100;
    pAsset20.min_value = 60;
    pAsset20.action = (ActionOptionAsset) (pAsset => World.world.zone_calculator.border_brightness = (float) this.getSavedInt(pAsset.id) / 100f);
    pAsset20.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset20);
  }

  private void initQualityOptions()
  {
    OptionAsset pAsset1 = new OptionAsset();
    pAsset1.id = "vignette";
    pAsset1.translation_key_description = "option_description_vignette";
    pAsset1.default_bool = true;
    pAsset1.type = OptionType.Bool;
    pAsset1.action = (ActionOptionAsset) (pAsset => ((Component) Camera.main).GetComponent<SleekRenderPostProcess>().settings.vignetteEnabled = this.getSavedBool(pAsset.id));
    this.add(pAsset1);
    OptionAsset pAsset2 = new OptionAsset();
    pAsset2.id = "bloom";
    pAsset2.translation_key_description = "option_description_bloom";
    pAsset2.default_bool = true;
    pAsset2.type = OptionType.Bool;
    pAsset2.action = (ActionOptionAsset) (pAsset => ((Component) Camera.main).GetComponent<SleekRenderPostProcess>().settings.bloomEnabled = this.getSavedBool(pAsset.id));
    this.add(pAsset2);
    OptionAsset pAsset3 = new OptionAsset();
    pAsset3.id = "fire";
    pAsset3.translation_key_description = "option_description_fire";
    pAsset3.default_bool = true;
    pAsset3.type = OptionType.Bool;
    pAsset3.action = (ActionOptionAsset) (pAsset => ((Behaviour) World.world.particles_fire).enabled = this.getSavedBool(pAsset.id));
    this.add(pAsset3);
    OptionAsset pAsset4 = new OptionAsset();
    pAsset4.id = "smoke";
    pAsset4.translation_key_description = "option_description_smoke";
    pAsset4.default_bool = true;
    pAsset4.type = OptionType.Bool;
    pAsset4.action = (ActionOptionAsset) (pAsset => ((Behaviour) World.world.particles_smoke).enabled = this.getSavedBool(pAsset.id));
    this.add(pAsset4);
    OptionAsset pAsset5 = new OptionAsset();
    pAsset5.id = "shadows";
    pAsset5.translation_key_description = "option_description_shadows";
    pAsset5.type = OptionType.Bool;
    pAsset5.default_bool = true;
    pAsset5.action = (ActionOptionAsset) (pAsset => Config.shadows_active = this.getSavedBool(pAsset.id));
    this.add(pAsset5);
    OptionAsset pAsset6 = new OptionAsset();
    pAsset6.id = "tree_wind";
    pAsset6.translation_key_description = "option_description_tree_wind";
    pAsset6.type = OptionType.Bool;
    pAsset6.default_bool = true;
    pAsset6.action = (ActionOptionAsset) (pAsset =>
    {
      BuildingRendererSettings.wobbly_material_enabled = this.getSavedBool(pAsset.id);
      World.world.buildings.checkWobblySetting();
    });
    this.add(pAsset6);
    OptionAsset pAsset7 = new OptionAsset();
    pAsset7.id = "minimap_transition_animation";
    pAsset7.translation_key_description = "option_description_minimap_transition_animation";
    pAsset7.type = OptionType.Bool;
    pAsset7.default_bool = true;
    this.add(pAsset7);
    OptionAsset pAsset8 = new OptionAsset();
    pAsset8.id = "night_lights";
    pAsset8.translation_key_description = "option_description_night_lights";
    pAsset8.type = OptionType.Bool;
    pAsset8.default_bool = true;
    this.add(pAsset8);
    OptionAsset pAsset9 = new OptionAsset();
    pAsset9.id = "cursor_lights";
    pAsset9.translation_key_description = "option_description_cursor_lights";
    pAsset9.type = OptionType.Bool;
    pAsset9.default_bool = true;
    pAsset9.default_bool_mobile = false;
    this.add(pAsset9);
    OptionAsset pAsset10 = new OptionAsset();
    pAsset10.id = "age_particles";
    pAsset10.translation_key_description = "option_description_age_particles";
    pAsset10.type = OptionType.Bool;
    pAsset10.default_bool = true;
    pAsset10.action = (ActionOptionAsset) (pAsset => WorldAgesParticles.effects_enabled = this.getSavedBool(pAsset.id));
    this.add(pAsset10);
    OptionAsset pAsset11 = new OptionAsset();
    pAsset11.id = "age_overlay_effect";
    pAsset11.translation_key_description = "option_description_age_overlay_effect";
    pAsset11.type = OptionType.Int;
    pAsset11.default_int = 100;
    pAsset11.max_value = 100;
    pAsset11.min_value = 0;
    pAsset11.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset11);
    OptionAsset pAsset12 = new OptionAsset();
    pAsset12.id = "age_night_effect";
    pAsset12.translation_key_description = "option_description_age_night_effect";
    pAsset12.type = OptionType.Int;
    pAsset12.default_int = 100;
    pAsset12.max_value = 100;
    pAsset12.min_value = 50;
    pAsset12.counter_format = new ActionFormatCounterOptionAsset(this.getCounterFormat100Percent);
    this.add(pAsset12);
  }

  private string getCounterFormat100Percent(OptionAsset pAsset)
  {
    return PlayerConfig.getIntValue(pAsset.id).ToString() + "%";
  }

  private void initOtherOptions()
  {
    OptionAsset pAsset1 = new OptionAsset();
    pAsset1.id = "preload_windows";
    pAsset1.translation_key_description = "option_description_preload_windows";
    pAsset1.default_bool = true;
    pAsset1.type = OptionType.Bool;
    pAsset1.override_bool_mobile = true;
    pAsset1.default_bool_mobile = false;
    pAsset1.action = (ActionOptionAsset) (pAsset => Config.preload_windows = this.getSavedBool(pAsset.id));
    this.add(pAsset1);
    OptionAsset pAsset2 = new OptionAsset();
    pAsset2.id = "preload_quantum_sprites";
    pAsset2.translation_key_description = "option_description_preload_quantum_sprites";
    pAsset2.default_bool = true;
    pAsset2.type = OptionType.Bool;
    pAsset2.override_bool_mobile = true;
    pAsset2.default_bool_mobile = false;
    pAsset2.action = (ActionOptionAsset) (pAsset => Config.preload_quantum_sprites = this.getSavedBool(pAsset.id));
    this.add(pAsset2);
    OptionAsset pAsset3 = new OptionAsset();
    pAsset3.id = "preload_buildings";
    pAsset3.translation_key_description = "option_description_preload_buildings";
    pAsset3.default_bool = true;
    pAsset3.type = OptionType.Bool;
    pAsset3.override_bool_mobile = true;
    pAsset3.default_bool_mobile = false;
    pAsset3.action = (ActionOptionAsset) (pAsset => Config.preload_buildings = this.getSavedBool(pAsset.id));
    this.add(pAsset3);
    OptionAsset pAsset4 = new OptionAsset();
    pAsset4.id = "preload_units";
    pAsset4.translation_key_description = "option_description_preload_units";
    pAsset4.default_bool = true;
    pAsset4.type = OptionType.Bool;
    pAsset4.override_bool_mobile = true;
    pAsset4.default_bool_mobile = false;
    pAsset4.action = (ActionOptionAsset) (pAsset => Config.preload_units = this.getSavedBool(pAsset.id));
    this.add(pAsset4);
    OptionAsset pAsset5 = new OptionAsset();
    pAsset5.id = "autosaves";
    pAsset5.translation_key_description = "option_description_autosaves";
    pAsset5.default_bool = true;
    pAsset5.type = OptionType.Bool;
    pAsset5.action = (ActionOptionAsset) (pAsset => Config.autosaves = this.getSavedBool(pAsset.id));
    this.add(pAsset5);
    OptionAsset pAsset6 = new OptionAsset();
    pAsset6.id = "graphs";
    pAsset6.translation_key_description = "option_description_graphs";
    pAsset6.default_bool = true;
    pAsset6.type = OptionType.Bool;
    pAsset6.action = (ActionOptionAsset) (pAsset => Config.graphs = this.getSavedBool(pAsset.id));
    this.add(pAsset6);
    OptionAsset pAsset7 = new OptionAsset();
    pAsset7.id = "tooltips";
    pAsset7.translation_key_description = "option_description_tooltips";
    pAsset7.default_bool = true;
    pAsset7.type = OptionType.Bool;
    pAsset7.override_bool_mobile = true;
    pAsset7.default_bool_mobile = false;
    pAsset7.action = (ActionOptionAsset) (pAsset => Config.tooltips_active = this.getSavedBool(pAsset.id));
    this.add(pAsset7);
    OptionAsset pAsset8 = new OptionAsset();
    pAsset8.id = "experimental";
    pAsset8.translation_key_description = "option_description_experimental";
    pAsset8.translation_key_description_2 = "option_description_experimental_warning";
    pAsset8.default_bool = false;
    pAsset8.type = OptionType.Bool;
    pAsset8.action = (ActionOptionAsset) (pAsset =>
    {
      bool pValue = this.getSavedBool(pAsset.id);
      if (pValue)
      {
        string savedString = this.getSavedString("last_used_version");
        if (savedString != Application.version)
        {
          pValue = false;
          Debug.LogWarning((object) $"Last version and current version differ, disabling experimental mode {savedString} {Application.version}");
          this.setSavedBool(pAsset.id, pValue);
        }
        else
        {
          Debug.Log((object) "Experimental mode is enabled");
          WorldTip.instance.showToolbarText("Experimental mode is enabled");
        }
      }
      Config.experimental_mode = pValue;
    });
    this.add(pAsset8);
    OptionAsset pAsset9 = new OptionAsset();
    pAsset9.id = "last_used_version";
    pAsset9.default_string = "";
    pAsset9.type = OptionType.String;
    pAsset9.has_locales = false;
    pAsset9.action = (ActionOptionAsset) (pAsset => PlayerConfig.setOptionString(pAsset.id, Application.version));
    this.add(pAsset9);
  }

  private void initHotkeyOptions()
  {
    OptionAsset pAsset = new OptionAsset();
    pAsset.id = "hotkey_1";
    pAsset.default_string = string.Empty;
    pAsset.type = OptionType.String;
    pAsset.has_locales = false;
    this.add(pAsset);
    this.clone("hotkey_2", "hotkey_1");
    this.clone("hotkey_3", "hotkey_1");
    this.clone("hotkey_4", "hotkey_1");
    this.clone("hotkey_5", "hotkey_1");
    this.clone("hotkey_6", "hotkey_1");
    this.clone("hotkey_7", "hotkey_1");
    this.clone("hotkey_8", "hotkey_1");
    this.clone("hotkey_9", "hotkey_1");
    this.clone("hotkey_0", "hotkey_1");
  }

  public void forceDisableOption(string pID)
  {
    OptionAsset pAsset = this.get(pID);
    if (pAsset.computer_only && Config.isMobile)
      return;
    PlayerConfig.setOptionBool(pAsset.id, false);
    ActionOptionAsset action = pAsset.action;
    if (action == null)
      return;
    action(pAsset);
  }

  private bool getSavedBool(string pAssetID) => PlayerConfig.optionBoolEnabled(pAssetID);

  private void setSavedBool(string pAssetID, bool pValue)
  {
    PlayerConfig.setOptionBool(pAssetID, pValue);
  }

  private int getSavedInt(string pAssetID) => PlayerConfig.getOptionInt(pAssetID);

  private string getSavedString(string pAssetID) => PlayerConfig.getOptionString(pAssetID);

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (OptionAsset optionAsset in this.list)
    {
      if (optionAsset.has_locales && optionAsset.type == OptionType.Bool && string.IsNullOrEmpty(optionAsset.translation_key))
      {
        foreach (GodPower godPower in AssetManager.powers.list)
        {
          if (!(godPower.toggle_name != optionAsset.id))
          {
            optionAsset.translation_key = godPower.getLocaleID();
            if (string.IsNullOrEmpty(optionAsset.translation_key_description))
              optionAsset.translation_key_description = godPower.getDescriptionID();
          }
        }
      }
    }
  }

  public override void editorDiagnosticLocales()
  {
    foreach (OptionAsset pAsset in this.list)
    {
      if (pAsset.has_locales)
      {
        this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
        this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
        this.checkLocale((Asset) pAsset, pAsset.getDescriptionID2());
      }
    }
    base.editorDiagnosticLocales();
  }
}
