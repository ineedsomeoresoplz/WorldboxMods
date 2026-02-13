// Decompiled with JetBrains decompiler
// Type: HotkeyLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public class HotkeyLibrary : AssetLibrary<HotkeyAsset>
{
  public static HotkeyAsset cancel;
  public static HotkeyAsset console;
  public static HotkeyAsset remove;
  public static HotkeyAsset pause;
  public static HotkeyAsset hide_ui;
  public static HotkeyAsset action_jump;
  public static HotkeyAsset action_dash;
  public static HotkeyAsset action_backstep;
  public static HotkeyAsset action_talk;
  public static HotkeyAsset action_steal;
  public static HotkeyAsset action_swear;
  public static HotkeyAsset left;
  public static HotkeyAsset right;
  public static HotkeyAsset up;
  public static HotkeyAsset down;
  public static HotkeyAsset next_unit_in_multi_selection;
  public static HotkeyAsset next_tab;
  public static HotkeyAsset prev_tab;
  public static HotkeyAsset zoom_in;
  public static HotkeyAsset zoom_out;
  public static HotkeyAsset zoom;
  public static HotkeyAsset world_speed;
  public static HotkeyAsset brush;
  public static HotkeyAsset follow_unit;
  public static HotkeyAsset control_unit;
  public static HotkeyAsset fullscreen_switch;
  public static HotkeyAsset many_mod;
  public static HotkeyAsset fast_civ_mod;
  public static KeyCode[] mod_keys = new KeyCode[0];
  private HotkeyAsset[] action_hotkeys = new HotkeyAsset[0];
  private Dictionary<string, float> holding_times = new Dictionary<string, float>();
  private bool holdingAnyModKey;
  private bool runModKeyCheck = true;
  private bool _last_input_active;
  private MetaType[] _meta_zones = new MetaType[10]
  {
    MetaType.Army,
    MetaType.Alliance,
    MetaType.Kingdom,
    MetaType.City,
    MetaType.Clan,
    MetaType.Religion,
    MetaType.Culture,
    MetaType.Language,
    MetaType.Family,
    MetaType.Subspecies
  };

  public override void init()
  {
    base.init();
    this.addHotkeysForUnitControlLayer();
    HotkeyAsset pAsset1 = new HotkeyAsset();
    pAsset1.id = "fullscreen_switch";
    pAsset1.default_key_1 = (KeyCode) 13;
    pAsset1.default_key_mod_1 = (KeyCode) 308;
    pAsset1.just_pressed_action = (HotkeyAction) (_ => PlayerConfig.toggleFullScreen());
    HotkeyLibrary.fullscreen_switch = this.add(pAsset1);
    HotkeyAsset pAsset2 = new HotkeyAsset();
    pAsset2.id = "console";
    pAsset2.default_key_1 = (KeyCode) 126;
    pAsset2.default_key_2 = (KeyCode) 96 /*0x60*/;
    pAsset2.check_controls_locked = true;
    pAsset2.just_pressed_action = (HotkeyAction) (_ =>
    {
      if (!Object.op_Equality((Object) EventSystem.current.currentSelectedGameObject, (Object) null))
        return;
      World.world.console.Toggle();
    });
    HotkeyLibrary.console = this.add(pAsset2);
    HotkeyAsset pAsset3 = new HotkeyAsset();
    pAsset3.id = "cancel";
    pAsset3.default_key_1 = (KeyCode) 27;
    pAsset3.just_pressed_action = new HotkeyAction(this.escapeAction);
    HotkeyLibrary.cancel = this.add(pAsset3);
    HotkeyAsset pAsset4 = new HotkeyAsset();
    pAsset4.id = "back";
    pAsset4.default_key_1 = (KeyCode) 326;
    pAsset4.just_pressed_action = new HotkeyAction(this.backAction);
    this.add(pAsset4);
    HotkeyAsset pAsset5 = new HotkeyAsset();
    pAsset5.id = "pause";
    pAsset5.default_key_1 = (KeyCode) 32 /*0x20*/;
    pAsset5.check_window_not_active = true;
    pAsset5.check_controls_locked = true;
    pAsset5.just_pressed_action = (HotkeyAction) (_ => Config.paused = !Config.paused);
    HotkeyLibrary.pause = this.add(pAsset5);
    HotkeyAsset pAsset6 = new HotkeyAsset();
    pAsset6.id = "hide_ui";
    pAsset6.default_key_1 = (KeyCode) 104;
    pAsset6.check_window_not_active = true;
    pAsset6.check_controls_locked = true;
    pAsset6.just_pressed_action = (HotkeyAction) (_ => Config.ui_main_hidden = !Config.ui_main_hidden);
    HotkeyLibrary.hide_ui = this.add(pAsset6);
    HotkeyAsset pAsset7 = new HotkeyAsset();
    pAsset7.id = "remove";
    pAsset7.default_key_1 = (KeyCode) (int) sbyte.MaxValue;
    pAsset7.default_key_2 = (KeyCode) 8;
    pAsset7.check_window_not_active = true;
    pAsset7.check_controls_locked = true;
    pAsset7.just_pressed_action = (HotkeyAction) (_ =>
    {
      if (SelectedUnit.isSet())
      {
        SelectedUnit.killSelected();
      }
      else
      {
        string pID = "life_eraser";
        if (World.world.isSelectedPower("life_eraser"))
          pID = "demolish";
        World.world.selected_buttons.clickPowerButton(PowerButton.get(pID));
      }
    });
    HotkeyLibrary.remove = this.add(pAsset7);
    HotkeyAsset pAsset8 = new HotkeyAsset();
    pAsset8.id = "zoom";
    pAsset8.use_mouse_wheel = true;
    pAsset8.holding_cooldown = 0.0f;
    pAsset8.check_window_not_active = true;
    pAsset8.check_controls_locked = true;
    pAsset8.allow_unit_control = true;
    pAsset8.holding_action = (HotkeyAction) (pAsset =>
    {
      if (!World.world.isPointerInGame() || World.world.isOverUI() && !MoveCamera.inSpectatorMode())
        return;
      float y = Input.mouseScrollDelta.y;
      if ((double) y < 0.0)
      {
        MoveCamera.zoomOutWheel(pAsset);
      }
      else
      {
        if ((double) y <= 0.0)
          return;
        MoveCamera.zoomInWheel(pAsset);
      }
    });
    HotkeyLibrary.zoom = this.add(pAsset8);
    HotkeyAsset pAsset9 = new HotkeyAsset();
    pAsset9.id = "world_speed";
    pAsset9.default_key_mod_1 = (KeyCode) 306;
    pAsset9.default_key_mod_2 = (KeyCode) 305;
    pAsset9.default_key_mod_3 = (KeyCode) 310;
    pAsset9.check_window_not_active = true;
    pAsset9.check_controls_locked = true;
    pAsset9.use_mouse_wheel = true;
    pAsset9.holding_cooldown = 0.0f;
    pAsset9.holding_action = (HotkeyAction) (_ =>
    {
      float y = Input.mouseScrollDelta.y;
      WorldTimeScaleAsset timeScaleAsset = Config.time_scale_asset;
      if ((double) y < 0.0)
        Config.prevWorldSpeed();
      else if ((double) y > 0.0)
        Config.nextWorldSpeed();
      if (timeScaleAsset == Config.time_scale_asset)
        return;
      string pText = LocalizedTextManager.getText("changed_worldspeed").Replace("$speed$", Config.time_scale_asset.getLocaleID() == null ? Toolbox.coloredText(Config.time_scale_asset.id, "#95DD5D") : Toolbox.coloredText(Config.time_scale_asset.getLocaleID(), "#95DD5D", true));
      WorldTip.instance.showToolbarText(pText);
    });
    HotkeyLibrary.world_speed = this.add(pAsset9);
    HotkeyAsset pAsset10 = new HotkeyAsset();
    pAsset10.id = "brush";
    pAsset10.default_key_mod_1 = (KeyCode) 308;
    pAsset10.default_key_mod_2 = (KeyCode) 307;
    pAsset10.check_window_not_active = true;
    pAsset10.check_controls_locked = true;
    pAsset10.use_mouse_wheel = true;
    pAsset10.holding_cooldown = 0.0f;
    pAsset10.holding_action = (HotkeyAction) (_ =>
    {
      float y = Input.mouseScrollDelta.y;
      string currentBrush = Config.current_brush;
      if ((double) y < 0.0)
        BrushLibrary.nextBrush();
      else if ((double) y > 0.0)
        BrushLibrary.previousBrush();
      if (!(currentBrush != Config.current_brush))
        return;
      BrushData brushData = Brush.get(Config.current_brush);
      string localeId = brushData.getLocaleID();
      string pText = LocalizedTextManager.getText("changed_brush").Replace("$brush$", $"{Toolbox.coloredText(localeId, "#95DD5D", true)} ({Toolbox.coloredText(brushData.size.ToString(), "#95DD5D")})");
      WorldTip.instance.showToolbarText(pText);
    });
    HotkeyLibrary.brush = this.add(pAsset10);
    HotkeyAsset pAsset11 = new HotkeyAsset();
    pAsset11.id = "many_mod";
    pAsset11.default_key_mod_1 = (KeyCode) 303;
    pAsset11.default_key_mod_2 = (KeyCode) 304;
    pAsset11.disable_for_controlled_unit = true;
    pAsset11.check_only_not_controllable_unit = true;
    HotkeyLibrary.many_mod = this.add(pAsset11);
    HotkeyAsset pAsset12 = new HotkeyAsset();
    pAsset12.id = "fast_civ_mod";
    pAsset12.default_key_mod_1 = (KeyCode) 305;
    pAsset12.default_key_mod_2 = (KeyCode) 306;
    HotkeyLibrary.fast_civ_mod = this.add(pAsset12);
    HotkeyAsset pAsset13 = new HotkeyAsset();
    pAsset13.id = "left";
    pAsset13.default_key_1 = (KeyCode) 97;
    pAsset13.default_key_2 = (KeyCode) 276;
    pAsset13.holding_action = new HotkeyAction(MoveCamera.move);
    pAsset13.holding_cooldown = 0.0f;
    pAsset13.check_window_not_active = true;
    pAsset13.check_controls_locked = true;
    pAsset13.allow_unit_control = true;
    HotkeyLibrary.left = this.add(pAsset13);
    HotkeyLibrary.right = this.clone("right", "left");
    this.t.default_key_1 = (KeyCode) 100;
    this.t.default_key_2 = (KeyCode) 275;
    HotkeyLibrary.up = this.clone("up", "left");
    this.t.default_key_1 = (KeyCode) 119;
    this.t.default_key_2 = (KeyCode) 273;
    HotkeyLibrary.down = this.clone("down", "left");
    this.t.default_key_1 = (KeyCode) 115;
    this.t.default_key_2 = (KeyCode) 274;
    this.clone("fast_left", "left");
    this.t.default_key_mod_1 = (KeyCode) 303;
    this.t.default_key_mod_2 = (KeyCode) 304;
    this.clone("fast_right", "right");
    this.t.default_key_mod_1 = (KeyCode) 303;
    this.t.default_key_mod_2 = (KeyCode) 304;
    this.clone("fast_up", "up");
    this.t.default_key_mod_1 = (KeyCode) 303;
    this.t.default_key_mod_2 = (KeyCode) 304;
    this.clone("fast_down", "down");
    this.t.default_key_mod_1 = (KeyCode) 303;
    this.t.default_key_mod_2 = (KeyCode) 304;
    HotkeyAsset pAsset14 = new HotkeyAsset();
    pAsset14.id = "zoom_in";
    pAsset14.default_key_1 = (KeyCode) 113;
    pAsset14.default_key_2 = (KeyCode) 43;
    pAsset14.default_key_3 = (KeyCode) 270;
    pAsset14.check_window_not_active = true;
    pAsset14.check_controls_locked = true;
    pAsset14.holding_action = new HotkeyAction(MoveCamera.zoomIn);
    pAsset14.holding_cooldown = 0.0f;
    HotkeyLibrary.zoom_in = this.add(pAsset14);
    HotkeyAsset pAsset15 = new HotkeyAsset();
    pAsset15.id = "zoom_out";
    pAsset15.default_key_1 = (KeyCode) 101;
    pAsset15.default_key_2 = (KeyCode) 45;
    pAsset15.default_key_3 = (KeyCode) 269;
    pAsset15.check_window_not_active = true;
    pAsset15.check_controls_locked = true;
    pAsset15.holding_action = new HotkeyAction(MoveCamera.zoomOut);
    pAsset15.holding_cooldown = 0.0f;
    HotkeyLibrary.zoom_out = this.add(pAsset15);
    HotkeyAsset pAsset16 = new HotkeyAsset();
    pAsset16.id = "power_left";
    pAsset16.default_key_1 = (KeyCode) 276;
    pAsset16.default_key_2 = (KeyCode) 97;
    pAsset16.default_key_mod_1 = (KeyCode) 306;
    pAsset16.default_key_mod_2 = (KeyCode) 310;
    pAsset16.default_key_mod_3 = (KeyCode) 305;
    pAsset16.check_window_not_active = true;
    pAsset16.check_controls_locked = true;
    pAsset16.just_pressed_action = new HotkeyAction(this.powerMove);
    pAsset16.holding_action = new HotkeyAction(this.powerMove);
    this.add(pAsset16);
    this.clone("power_right", "power_left");
    this.t.default_key_1 = (KeyCode) 275;
    this.t.default_key_2 = (KeyCode) 100;
    this.clone("power_up", "power_left");
    this.t.default_key_1 = (KeyCode) 273;
    this.t.default_key_2 = (KeyCode) 119;
    this.clone("power_down", "power_left");
    this.t.default_key_1 = (KeyCode) 274;
    this.t.default_key_2 = (KeyCode) 115;
    HotkeyAsset pAsset17 = new HotkeyAsset();
    pAsset17.id = "toggle_power";
    pAsset17.default_key_1 = (KeyCode) 13;
    pAsset17.default_key_2 = (KeyCode) 271;
    pAsset17.check_controls_locked = true;
    pAsset17.just_pressed_action = (HotkeyAction) (_ =>
    {
      PowerButton activeButton = PowersTab.getActiveTab().getActiveButton();
      if (Object.op_Equality((Object) activeButton, (Object) null))
        return;
      if (activeButton.godPower != null)
      {
        switch (activeButton.godPower.id)
        {
          case "clock":
            Config.nextWorldSpeed(true);
            break;
          case "pause":
            activeButton.clickSpecial();
            break;
          default:
            PowerButtonClickAction selectButtonAction = activeButton.godPower.select_button_action;
            if (selectButtonAction != null)
            {
              int num = selectButtonAction(activeButton.godPower.id) ? 1 : 0;
            }
            if (activeButton.godPower.toggle_action == null)
              break;
            PowerToggleAction toggleAction = activeButton.godPower.toggle_action;
            if (toggleAction != null)
              toggleAction(activeButton.godPower.id);
            PowerButtonSelector.instance.checkToggleIcons();
            break;
        }
      }
      else if (activeButton.type == PowerButtonType.Options)
        ((UnityEvent) ((Component) activeButton).gameObject.GetComponent<Button>().onClick).Invoke();
      else
        activeButton.clickButton();
    });
    this.add(pAsset17);
    this.clone("toggle_power2", "toggle_power");
    this.t.default_key_mod_1 = (KeyCode) 306;
    this.t.default_key_mod_2 = (KeyCode) 310;
    HotkeyAsset pAsset18 = new HotkeyAsset();
    pAsset18.id = "next_tab";
    pAsset18.default_key_1 = (KeyCode) 9;
    pAsset18.check_window_not_active = true;
    pAsset18.check_controls_locked = true;
    pAsset18.check_no_multi_unit_selection = true;
    pAsset18.just_pressed_action = (HotkeyAction) (_ =>
    {
      Button next = PowerTabController.instance.getNext(((Object) PowersTab.getActiveTab()).name);
      PowersTab.showTabFromButton(next);
      TipButton component = ((Component) next).gameObject.GetComponent<TipButton>();
      string pText = $"{LocalizedTextManager.getText(component.textOnClick)}\n{LocalizedTextManager.getText(component.textOnClickDescription)}";
      WorldTip.instance.showToolbarText(pText);
    });
    HotkeyLibrary.next_tab = this.add(pAsset18);
    HotkeyAsset pAsset19 = new HotkeyAsset();
    pAsset19.id = "prev_tab";
    pAsset19.default_key_1 = (KeyCode) 9;
    pAsset19.default_key_mod_1 = (KeyCode) 304;
    pAsset19.default_key_mod_2 = (KeyCode) 303;
    pAsset19.check_window_not_active = true;
    pAsset19.check_controls_locked = true;
    pAsset19.check_no_multi_unit_selection = true;
    pAsset19.just_pressed_action = (HotkeyAction) (_ => PowersTab.showTabFromButton(PowerTabController.instance.getPrev(((Object) PowersTab.getActiveTab()).name)));
    HotkeyLibrary.prev_tab = this.add(pAsset19);
    HotkeyAsset pAsset20 = new HotkeyAsset();
    pAsset20.id = "hotkey_1";
    pAsset20.default_key_1 = (KeyCode) 49;
    pAsset20.default_key_2 = (KeyCode) 257;
    pAsset20.check_window_not_active = true;
    pAsset20.check_controls_locked = true;
    pAsset20.just_pressed_action = (HotkeyAction) (pAsset =>
    {
      string id = pAsset.id;
      string pSelectNano = this.getHotkeyFromData(id);
      if (!string.IsNullOrEmpty(pSelectNano))
      {
        this.hotkeySelectNano(pAsset, pSelectNano);
      }
      else
      {
        string stringVal = PlayerConfig.dict[id].stringVal;
        this.hotkeySelectPower(pAsset, stringVal);
      }
    });
    this.add(pAsset20);
    this.clone("hotkey_2", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 50;
    this.t.default_key_2 = (KeyCode) 258;
    this.clone("hotkey_3", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 51;
    this.t.default_key_2 = (KeyCode) 259;
    this.clone("hotkey_4", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 52;
    this.t.default_key_2 = (KeyCode) 260;
    this.clone("hotkey_5", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 53;
    this.t.default_key_2 = (KeyCode) 261;
    this.clone("hotkey_6", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 54;
    this.t.default_key_2 = (KeyCode) 262;
    this.clone("hotkey_7", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 55;
    this.t.default_key_2 = (KeyCode) 263;
    this.clone("hotkey_8", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 56;
    this.t.default_key_2 = (KeyCode) 264;
    this.clone("hotkey_9", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 57;
    this.t.default_key_2 = (KeyCode) 265;
    this.clone("hotkey_0", "hotkey_1");
    this.t.default_key_1 = (KeyCode) 48 /*0x30*/;
    this.t.default_key_2 = (KeyCode) 256 /*0x0100*/;
    HotkeyAsset pAsset21 = new HotkeyAsset();
    pAsset21.id = "save_hotkey_1";
    pAsset21.default_key_1 = (KeyCode) 49;
    pAsset21.default_key_2 = (KeyCode) 257;
    pAsset21.default_key_mod_1 = (KeyCode) 306;
    pAsset21.default_key_mod_2 = (KeyCode) 310;
    pAsset21.check_window_not_active = true;
    pAsset21.check_controls_locked = true;
    pAsset21.just_pressed_action = (HotkeyAction) (pAsset =>
    {
      if (SelectedObjects.isNanoObjectSet())
        this.hotkeySaveTab(pAsset);
      else
        this.hotkeySavePower(pAsset);
    });
    this.add(pAsset21);
    this.clone("save_hotkey_2", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 50;
    this.t.default_key_2 = (KeyCode) 258;
    this.clone("save_hotkey_3", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 51;
    this.t.default_key_2 = (KeyCode) 259;
    this.clone("save_hotkey_4", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 52;
    this.t.default_key_2 = (KeyCode) 260;
    this.clone("save_hotkey_5", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 53;
    this.t.default_key_2 = (KeyCode) 261;
    this.clone("save_hotkey_6", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 54;
    this.t.default_key_2 = (KeyCode) 262;
    this.clone("save_hotkey_7", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 55;
    this.t.default_key_2 = (KeyCode) 263;
    this.clone("save_hotkey_8", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 56;
    this.t.default_key_2 = (KeyCode) 264;
    this.clone("save_hotkey_9", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 57;
    this.t.default_key_2 = (KeyCode) 265;
    this.clone("save_hotkey_0", "save_hotkey_1");
    this.t.default_key_1 = (KeyCode) 48 /*0x30*/;
    this.t.default_key_2 = (KeyCode) 256 /*0x0100*/;
    HotkeyAsset pAsset22 = new HotkeyAsset();
    pAsset22.id = "zone_type_previous";
    pAsset22.default_key_1 = (KeyCode) 122;
    pAsset22.check_window_not_active = true;
    pAsset22.check_controls_locked = true;
    pAsset22.just_pressed_action = (HotkeyAction) (_ => this.switchZones(-1));
    this.add(pAsset22);
    this.clone("zone_type_next", "zone_type_previous");
    this.t.just_pressed_action = (HotkeyAction) (_ => this.switchZones(1));
    this.t.default_key_1 = (KeyCode) 120;
    HotkeyAsset pAsset23 = new HotkeyAsset();
    pAsset23.id = "zone_type_state_next";
    pAsset23.default_key_1 = (KeyCode) 99;
    pAsset23.check_window_not_active = true;
    pAsset23.check_controls_locked = true;
    pAsset23.just_pressed_action = (HotkeyAction) (_ => this.toggleZones(1));
    this.add(pAsset23);
    this.clone("zone_type_state_previous", "zone_type_state_next");
    this.t.just_pressed_action = (HotkeyAction) (_ => this.toggleZones(-1));
    this.t.default_key_mod_1 = (KeyCode) 306;
    this.t.default_key_mod_2 = (KeyCode) 310;
    HotkeyAsset pAsset24 = new HotkeyAsset();
    pAsset24.id = "follow_unit";
    pAsset24.default_key_1 = (KeyCode) 102;
    pAsset24.check_window_not_active = false;
    pAsset24.check_controls_locked = true;
    pAsset24.just_pressed_action = (HotkeyAction) (_ =>
    {
      Actor unit = SelectedUnit.unit;
      if (ScrollWindow.isWindowActive())
      {
        ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
        if (currentWindow.screen_id != "unit" || ((Component) currentWindow).GetComponent<UnitWindow>().name_input.inputField.isFocused || !SelectedUnit.isSet())
          return;
        World.world.followUnit(unit);
        ScrollWindow.hideAllEvent();
      }
      else
      {
        if (!MapBox.isRenderGameplay())
          return;
        Actor actorNearCursor = World.world.getActorNearCursor();
        if (actorNearCursor == null)
        {
          if (MoveCamera.hasFocusUnit())
          {
            MoveCamera.clearFocusUnitOnly();
          }
          else
          {
            if (!SelectedUnit.isSet())
              return;
            World.world.followUnit(unit);
          }
        }
        else if (actorNearCursor.isCameraFollowingUnit())
          MoveCamera.clearFocusUnitOnly();
        else
          World.world.followUnit(actorNearCursor);
      }
    });
    HotkeyLibrary.follow_unit = this.add(pAsset24);
    HotkeyAsset pAsset25 = new HotkeyAsset();
    pAsset25.id = "control_unit";
    pAsset25.default_key_1 = (KeyCode) 103;
    pAsset25.check_window_not_active = false;
    pAsset25.just_pressed_action = (HotkeyAction) (_ =>
    {
      if (MoveCamera.hasFocusUnit())
        World.world.move_camera.clearFocusUnitAndUnselect();
      Actor unit = SelectedUnit.unit;
      if (ScrollWindow.isWindowActive())
      {
        ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
        if (currentWindow.screen_id != "unit" || ((Component) currentWindow).GetComponent<UnitWindow>().name_input.inputField.isFocused || !SelectedUnit.isSet())
          return;
        ControllableUnit.setControllableCreature(unit);
        ScrollWindow.hideAllEvent();
      }
      else
      {
        if (!MapBox.isRenderGameplay())
          return;
        Actor actorNearCursor = World.world.getActorNearCursor();
        if (ControllableUnit.isControllingUnit())
        {
          if (ControllableUnit.isControllingUnit(actorNearCursor))
          {
            ControllableUnit.clear();
            return;
          }
          if (actorNearCursor != null)
          {
            ControllableUnit.clear();
            ControllableUnit.setControllableCreature(actorNearCursor);
            return;
          }
          if (actorNearCursor == null)
          {
            ControllableUnit.clear();
            return;
          }
        }
        if (actorNearCursor == null)
        {
          if (!SelectedUnit.isSet())
            return;
          ControllableUnit.setControllableCreatureAndSelected(unit);
        }
        else
          ControllableUnit.setControllableCreatureAndSelected(actorNearCursor);
      }
    });
    HotkeyLibrary.control_unit = this.add(pAsset25);
    HotkeyAsset pAsset26 = new HotkeyAsset();
    pAsset26.id = "meta_window_previous";
    pAsset26.default_key_1 = (KeyCode) 276;
    pAsset26.default_key_2 = (KeyCode) 113;
    pAsset26.default_key_3 = (KeyCode) 97;
    pAsset26.just_pressed_action = (HotkeyAction) (_ => MetaSwitchManager.switchWindows(MetaSwitchManager.Direction.Left));
    pAsset26.check_controls_locked = true;
    pAsset26.check_window_active = true;
    this.add(pAsset26);
    this.clone("meta_window_next", "meta_window_previous");
    this.t.default_key_1 = (KeyCode) 275;
    this.t.default_key_2 = (KeyCode) 101;
    this.t.default_key_3 = (KeyCode) 100;
    this.t.just_pressed_action = (HotkeyAction) (_ => MetaSwitchManager.switchWindows(MetaSwitchManager.Direction.Right));
    HotkeyAsset pAsset27 = new HotkeyAsset();
    pAsset27.id = "window_tab_next";
    pAsset27.default_key_1 = (KeyCode) 9;
    pAsset27.default_key_2 = (KeyCode) 115;
    pAsset27.default_key_3 = (KeyCode) 274;
    pAsset27.just_pressed_action = new HotkeyAction(this.windowTabsSwitch);
    pAsset27.check_controls_locked = true;
    pAsset27.check_window_active = true;
    this.add(pAsset27);
    this.clone("window_tab_previous", "window_tab_next");
    this.t.default_key_mod_1 = (KeyCode) 304;
    this.t.default_key_mod_2 = (KeyCode) 303;
    this.clone("window_tab_previous_2", "window_tab_next");
    this.t.default_key_1 = (KeyCode) 119;
    this.t.default_key_2 = (KeyCode) 273;
    this.t.default_key_3 = (KeyCode) 0;
  }

  private void addHotkeysForUnitControlLayer()
  {
    HotkeyAsset pAsset1 = new HotkeyAsset();
    pAsset1.id = "next_unit_in_multi_selection";
    pAsset1.default_key_1 = (KeyCode) 9;
    pAsset1.check_window_not_active = true;
    pAsset1.check_controls_locked = true;
    pAsset1.check_multi_unit_selection = true;
    pAsset1.ignore_same_key_diagnostic = true;
    pAsset1.just_pressed_action = (HotkeyAction) (_ => SelectedUnit.nextMainUnit());
    HotkeyLibrary.next_unit_in_multi_selection = this.add(pAsset1);
    HotkeyAsset pAsset2 = new HotkeyAsset();
    pAsset2.id = "action_jump";
    pAsset2.default_key_1 = (KeyCode) 32 /*0x20*/;
    pAsset2.ignore_same_key_diagnostic = true;
    pAsset2.check_window_not_active = true;
    pAsset2.check_controls_locked = true;
    pAsset2.check_only_controllable_unit = true;
    HotkeyLibrary.action_jump = this.add(pAsset2);
    HotkeyAsset pAsset3 = new HotkeyAsset();
    pAsset3.id = "action_dash";
    pAsset3.default_key_1 = (KeyCode) 304;
    pAsset3.default_key_2 = (KeyCode) 303;
    pAsset3.ignore_same_key_diagnostic = true;
    pAsset3.check_window_not_active = true;
    pAsset3.check_controls_locked = true;
    pAsset3.ignore_mod_keys = true;
    pAsset3.check_only_controllable_unit = true;
    HotkeyLibrary.action_dash = this.add(pAsset3);
    HotkeyAsset pAsset4 = new HotkeyAsset();
    pAsset4.id = "action_backstep";
    pAsset4.default_key_1 = (KeyCode) 306;
    pAsset4.default_key_2 = (KeyCode) 305;
    pAsset4.ignore_same_key_diagnostic = true;
    pAsset4.check_window_not_active = true;
    pAsset4.check_controls_locked = true;
    pAsset4.ignore_mod_keys = true;
    pAsset4.check_only_controllable_unit = true;
    HotkeyLibrary.action_backstep = this.add(pAsset4);
    HotkeyAsset pAsset5 = new HotkeyAsset();
    pAsset5.id = "action_swear";
    pAsset5.default_key_1 = (KeyCode) 102;
    pAsset5.ignore_same_key_diagnostic = true;
    pAsset5.check_window_not_active = true;
    pAsset5.check_controls_locked = true;
    pAsset5.check_only_controllable_unit = true;
    HotkeyLibrary.action_swear = this.add(pAsset5);
    HotkeyAsset pAsset6 = new HotkeyAsset();
    pAsset6.id = "action_steal";
    pAsset6.default_key_1 = (KeyCode) 113;
    pAsset6.ignore_same_key_diagnostic = true;
    pAsset6.check_window_not_active = true;
    pAsset6.check_controls_locked = true;
    pAsset6.check_only_controllable_unit = true;
    HotkeyLibrary.action_steal = this.add(pAsset6);
    HotkeyAsset pAsset7 = new HotkeyAsset();
    pAsset7.id = "action_talk";
    pAsset7.default_key_1 = (KeyCode) 116;
    pAsset7.ignore_same_key_diagnostic = true;
    pAsset7.check_window_not_active = true;
    pAsset7.check_controls_locked = true;
    pAsset7.check_only_controllable_unit = true;
    HotkeyLibrary.action_talk = this.add(pAsset7);
  }

  private void switchZones(int pIndexChange)
  {
    MetaType metaZone = this._meta_zones[Toolbox.loopIndex(Array.IndexOf<MetaType>(this._meta_zones, Zones.getCurrentMapBorderMode(true)) + pIndexChange, this._meta_zones.Length)];
    MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(metaZone);
    AssetManager.powers.get(asset.power_option_zone_id).toggle_action(asset.power_option_zone_id);
    PowerButtonSelector.instance.checkToggleIcons();
    GodPower pPower = AssetManager.powers.get(asset.power_option_zone_id);
    WorldTip.instance.showToolbarText(pPower);
  }

  private void toggleZones(int pIndexChange)
  {
    MetaType currentMapBorderMode = Zones.getCurrentMapBorderMode(true);
    if (currentMapBorderMode == MetaType.None)
      return;
    MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(currentMapBorderMode);
    GodPower pPower = AssetManager.powers.get(asset.power_option_zone_id);
    if (!pPower.multi_toggle)
      return;
    asset.toggleOptionZone(pPower, pIndexChange, false);
    PowerButtonSelector.instance.checkToggleIcons();
  }

  private void windowTabsSwitch(HotkeyAsset pAsset)
  {
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    List<WindowMetaTab> contentTabs = currentWindow.tabs.getContentTabs();
    if (contentTabs.Count < 2)
      return;
    WindowMetaTab activeTab = currentWindow.tabs.getActiveTab();
    int pIndex = contentTabs.IndexOf(activeTab);
    switch (pAsset.id)
    {
      case "window_tab_next":
        ++pIndex;
        break;
      case "window_tab_previous":
      case "window_tab_previous_2":
        --pIndex;
        break;
    }
    int index = Toolbox.loopIndex(pIndex, contentTabs.Count);
    WindowMetaTab windowMetaTab = contentTabs[index];
    windowMetaTab.doAction();
    WorldTip.showNowTop(windowMetaTab.getWorldTipText(), false);
  }

  private bool navigateWindowBack(HotkeyAsset pAsset)
  {
    if (!ScrollWindow.isWindowActive())
      return false;
    if (ScrollWindow.isAnimationActive())
      ScrollWindow.finishAnimations();
    WindowHistory.clickBack();
    return true;
  }

  private bool navigateTabBack(HotkeyAsset pAsset)
  {
    return !ScrollWindow.isWindowActive() && SelectedTabsHistory.showPreviousTab();
  }

  private void backAction(HotkeyAsset pAsset)
  {
    if (this.navigateWindowBack(pAsset) || this.navigateTabBack(pAsset) || PowersTab.getActiveTab().getAsset().tab_type_main)
      return;
    PowerTabController.showMainTab();
  }

  private void escapeAction(HotkeyAsset pAsset)
  {
    if (World.world.console.isActive())
      World.world.console.Hide();
    else if (ControllableUnit.isControllingUnit())
      ControllableUnit.clear();
    else if (World.world.tutorial.isActive())
    {
      World.world.tutorial.endTutorial();
    }
    else
    {
      if (MapBox.controlsLocked() || MapBox.isControllingUnit())
        return;
      if (MoveCamera.hasFocusUnit())
      {
        MoveCamera.clearFocusUnitOnly();
      }
      else
      {
        if (this.navigateWindowBack(pAsset))
          return;
        if (Config.ui_main_hidden)
        {
          Config.ui_main_hidden = false;
        }
        else
        {
          if (this.navigateTabBack(pAsset))
            return;
          if (Object.op_Inequality((Object) World.world.selected_buttons.selectedButton, (Object) null))
            World.world.selected_buttons.unselectAll();
          else if (SelectedUnit.isSet())
            SelectedUnit.clear();
          else if (PowersTab.isTabSelected())
          {
            World.world.selected_buttons.unselectTabs();
            SelectedObjects.unselectNanoObject();
          }
          else
            ScrollWindow.showWindow("quit_game");
        }
      }
    }
  }

  private void powerMove(HotkeyAsset pAsset)
  {
    PowersTab activeTab = PowersTab.getActiveTab();
    switch (pAsset.id)
    {
      case "power_left":
        activeTab.leftButton();
        break;
      case "power_right":
        activeTab.rightButton();
        break;
      case "power_up":
        activeTab.upButton();
        break;
      case "power_down":
        activeTab.downButton();
        break;
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    HashSet<KeyCode> pHashSet1 = new HashSet<KeyCode>();
    HashSet<HotkeyAsset> pHashSet2 = new HashSet<HotkeyAsset>();
    foreach (HotkeyAsset hotkeyAsset in this.list)
    {
      hotkeyAsset.overridden_key_1 = hotkeyAsset.default_key_1;
      hotkeyAsset.overridden_key_2 = hotkeyAsset.default_key_2;
      hotkeyAsset.overridden_key_3 = hotkeyAsset.default_key_3;
      hotkeyAsset.overridden_key_mod_1 = hotkeyAsset.default_key_mod_1;
      hotkeyAsset.overridden_key_mod_2 = hotkeyAsset.default_key_mod_2;
      hotkeyAsset.overridden_key_mod_3 = hotkeyAsset.default_key_mod_3;
      if (hotkeyAsset.default_key_mod_1 != null)
        pHashSet1.Add(hotkeyAsset.default_key_mod_1);
      if (hotkeyAsset.default_key_mod_2 != null)
        pHashSet1.Add(hotkeyAsset.default_key_mod_2);
      if (hotkeyAsset.default_key_mod_3 != null)
        pHashSet1.Add(hotkeyAsset.default_key_mod_3);
      if (hotkeyAsset.just_pressed_action != null)
        pHashSet2.Add(hotkeyAsset);
      else if (hotkeyAsset.holding_action != null)
        pHashSet2.Add(hotkeyAsset);
    }
    HotkeyLibrary.mod_keys = pHashSet1.ToArray<KeyCode>();
    this.action_hotkeys = pHashSet2.ToArray<HotkeyAsset>();
  }

  public override void editorDiagnostic()
  {
    // ISSUE: unable to decompile the method.
  }

  public static bool isHoldingControlForSelection()
  {
    return Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305);
  }

  public static bool isHoldingAlt() => Input.GetKey((KeyCode) 308) || Input.GetKey((KeyCode) 307);

  public static bool isHoldingAnyMod()
  {
    return AssetManager.hotkey_library != null && AssetManager.hotkey_library.isHoldingAnyModKey();
  }

  public void reset()
  {
    foreach (HotkeyAsset hotkeyAsset in this.list)
    {
      hotkeyAsset.overridden_key_1 = hotkeyAsset.default_key_1;
      hotkeyAsset.overridden_key_2 = hotkeyAsset.default_key_2;
      hotkeyAsset.overridden_key_3 = hotkeyAsset.default_key_3;
      hotkeyAsset.overridden_key_mod_1 = hotkeyAsset.default_key_mod_1;
      hotkeyAsset.overridden_key_mod_2 = hotkeyAsset.default_key_mod_2;
      hotkeyAsset.overridden_key_mod_3 = hotkeyAsset.default_key_mod_3;
    }
  }

  public string replaceSpecialTextKeys(string pText)
  {
    if (!pText.Contains("$"))
      return pText;
    foreach (HotkeyAsset hotkeyAsset in this.list)
    {
      if (pText.Contains(hotkeyAsset.id))
      {
        string oldValue = $"${hotkeyAsset.id}$";
        string localizedKeys = hotkeyAsset.getLocalizedKeys();
        pText = pText.Replace(oldValue, localizedKeys);
        if (pText.Contains("$mouse_wheel$"))
        {
          string newValue = Toolbox.coloredText("mouse_wheel", "#95DD5D", true);
          pText = pText.Replace("$mouse_wheel$", newValue);
        }
        if (!pText.Contains("$"))
          return pText;
      }
    }
    return pText;
  }

  public bool isHoldingAnyModKey()
  {
    if (!Input.anyKey)
      return false;
    if (this.runModKeyCheck)
    {
      this.runModKeyCheck = false;
      this.holdingAnyModKey = false;
      foreach (int modKey in HotkeyLibrary.mod_keys)
      {
        if (Input.GetKey((KeyCode) modKey))
        {
          this.holdingAnyModKey = true;
          break;
        }
      }
    }
    return this.holdingAnyModKey;
  }

  public void checkHotKeyActions()
  {
    this.runModKeyCheck = true;
    bool flag1 = (double) Input.mouseScrollDelta.y != 0.0;
    if (!World.world.has_focus || !Input.anyKey && !flag1)
      return;
    bool flag2 = this.isInputActive();
    bool flag3 = this._last_input_active && !flag2;
    this._last_input_active = flag2;
    if (flag2 | flag3)
      return;
    bool flag4 = MapBox.controlsLocked();
    bool flag5 = MapBox.isControllingUnit();
    foreach (HotkeyAsset actionHotkey in this.action_hotkeys)
    {
      if ((!actionHotkey.use_mouse_wheel || flag1) && (!actionHotkey.check_controls_locked || !flag4 && (!flag5 || actionHotkey.allow_unit_control)) && actionHotkey.checkIsPossible())
      {
        if (actionHotkey.just_pressed_action != null && actionHotkey.isJustPressed())
        {
          actionHotkey.just_pressed_action(actionHotkey);
          if (actionHotkey.holding_action != null)
            this.holding_times[actionHotkey.id] = actionHotkey.holding_cooldown_first_action;
        }
        else if (actionHotkey.holding_action != null && actionHotkey.isHolding())
        {
          float num;
          this.holding_times.TryGetValue(actionHotkey.id, out num);
          num -= Time.deltaTime;
          if ((double) num > 0.0)
          {
            this.holding_times[actionHotkey.id] = num;
          }
          else
          {
            actionHotkey.holding_action(actionHotkey);
            this.holding_times[actionHotkey.id] = actionHotkey.holding_cooldown;
          }
        }
      }
    }
  }

  private bool isInputActive()
  {
    if (!EventSystem.current.isFocused)
      return false;
    GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
    if (Object.op_Equality((Object) selectedGameObject, (Object) null))
      return false;
    InputField component = selectedGameObject.GetComponent<InputField>();
    return !Object.op_Equality((Object) component, (Object) null) && component.isFocused;
  }

  public static bool allowedToUsePowers() => !ScrollWindow.isWindowActive();

  public void changeKey(HotkeyAsset pAsset, KeyCode pCode)
  {
  }

  public void load()
  {
  }

  public void hotkeySelectPower(HotkeyAsset pAsset, string pSelectPower)
  {
    if (!string.IsNullOrEmpty(pSelectPower) && AssetManager.powers.get(pSelectPower) == null)
      return;
    if (string.IsNullOrEmpty(pSelectPower))
    {
      this.showTipNothing(pAsset);
    }
    else
    {
      PowerButton tPowerButton = PowerButton.get(pSelectPower);
      if (Object.op_Equality((Object) tPowerButton, (Object) null))
        return;
      if (tPowerButton.isSelected())
        tPowerButton.cancelSelection();
      else
        tPowerButton.selectPowerTab((TweenCallback) (() =>
        {
          World.world.selected_buttons.clickPowerButton(tPowerButton);
          if (!tPowerButton.isSelected())
            return;
          WorldTip.instance.showToolbarText(tPowerButton.godPower);
        }));
    }
  }

  public void hotkeySelectNano(HotkeyAsset pAsset, string pSelectNano)
  {
    if (string.IsNullOrEmpty(pSelectNano))
    {
      this.showTipNothing(pAsset);
    }
    else
    {
      string[] strArray = pSelectNano.Split("|", StringSplitOptions.None);
      string pID = strArray[0];
      long pId = long.Parse(strArray[1]);
      MetaTypeAsset metaTypeAsset = AssetManager.meta_type_library.get(pID);
      NanoObject nanoObject = metaTypeAsset.get(pId);
      if (nanoObject.isRekt() && strArray.Length < 3)
      {
        this.showTipNothing(pAsset);
      }
      else
      {
        NanoObject selectedNanoObject = SelectedObjects.getSelectedNanoObject();
        if (SelectedObjects.isNanoObjectSet() && SelectedObjects.getSelectedNanoObject() == nanoObject)
        {
          if (selectedNanoObject == SelectedUnit.unit)
          {
            World.world.locatePosition(Vector2.op_Implicit(SelectedUnit.unit.current_position));
          }
          else
          {
            if (!(nanoObject is IMetaObject))
              return;
            Actor randomUnit = (nanoObject as IMetaObject).getRandomUnit();
            if (randomUnit == null)
              return;
            World.world.locatePosition(Vector2.op_Implicit(randomUnit.current_position));
          }
        }
        else
        {
          if (World.world.isAnyPowerSelected())
            PowerButtonSelector.instance.unselectAll();
          SelectedObjects.unselectNanoObject();
          SelectedUnit.clear();
          if (pID == "unit")
          {
            if (strArray.Length >= 3)
            {
              using (ListPool<Actor> pActors = new ListPool<Actor>(strArray.Length))
              {
                for (int index = 1; index < strArray.Length; ++index)
                {
                  Actor pObject = World.world.units.get(long.Parse(strArray[index]));
                  if (!pObject.isRekt())
                    pActors.Add(pObject);
                }
                if (pActors.Count > 0)
                {
                  SelectedUnit.selectMultiple(pActors);
                  SelectedObjects.setNanoObject((NanoObject) SelectedUnit.unit);
                  if (selectedNanoObject == SelectedUnit.unit)
                    World.world.locatePosition(Vector2.op_Implicit(SelectedUnit.unit.current_position));
                }
                if (pActors.Count == 0)
                  this.showTipNothing(pAsset);
                else if (pActors.Count == 1)
                  PowerTabController.showTabSelectedUnit();
                else
                  PowerTabController.showTabMultipleUnits();
              }
            }
            else
            {
              SelectedUnit.select(nanoObject as Actor);
              SelectedObjects.setNanoObject((NanoObject) SelectedUnit.unit);
              PowerTabController.showTabSelectedUnit();
            }
          }
          else
            metaTypeAsset.selectAndInspect(nanoObject, pCheckNameplate: false);
        }
      }
    }
  }

  public void showTipNothing(HotkeyAsset pAsset)
  {
    string pText1 = LocalizedTextManager.getText("hotkey_tip_empty_tip").Replace("$save_hotkey$", $"$save_{pAsset.id}$");
    string pText2 = AssetManager.hotkey_library.replaceSpecialTextKeys(pText1);
    WorldTip.instance.showToolbarText(pText2);
  }

  public void hotkeySavePower(HotkeyAsset pAsset)
  {
    string str1 = World.world.getSelectedPowerID();
    string str2 = pAsset.id.Replace("save_", "");
    string text;
    if (string.IsNullOrEmpty(str1))
    {
      str1 = string.Empty;
      text = LocalizedTextManager.getText("hotkey_tip_cleared");
    }
    else
      text = LocalizedTextManager.getText("hotkey_tip_saved_power");
    string pText1 = text.Replace("$save_hotkey$", $"${str2}$");
    string pText2 = AssetManager.hotkey_library.replaceSpecialTextKeys(pText1);
    WorldTip.instance.showToolbarText(pText2);
    PlayerConfig.dict[str2].stringVal = str1;
    PlayerConfig.saveData();
    this.getHotkeyFromData(str2) = string.Empty;
  }

  public void hotkeySaveTab(HotkeyAsset pAsset)
  {
    string pHotkeyId = pAsset.id.Replace("save_", "");
    string text;
    string str;
    if (!SelectedObjects.isNanoObjectSet())
    {
      text = LocalizedTextManager.getText("hotkey_tip_cleared");
      str = string.Empty;
    }
    else
    {
      text = LocalizedTextManager.getText("hotkey_tip_saved_nano");
      NanoObject selectedNanoObject = SelectedObjects.getSelectedNanoObject();
      str = selectedNanoObject.getMetaTypeAsset().id ?? "";
      if (SelectedUnit.isSet())
      {
        foreach (Actor allSelected in SelectedUnit.getAllSelectedList())
          str += $"|{allSelected.id}";
      }
      else
        str += $"|{selectedNanoObject.id}";
    }
    string pText1 = text.Replace("$save_hotkey$", $"${pHotkeyId}$");
    string pText2 = AssetManager.hotkey_library.replaceSpecialTextKeys(pText1);
    this.getHotkeyFromData(pHotkeyId) = str;
    WorldTip.instance.showToolbarText(pText2);
  }

  public ref string getHotkeyFromData(string pHotkeyId)
  {
    switch (pHotkeyId)
    {
      case "hotkey_0":
        return ref World.world.hotkey_tabs_data.hotkey_data_0;
      case "hotkey_1":
        return ref World.world.hotkey_tabs_data.hotkey_data_1;
      case "hotkey_2":
        return ref World.world.hotkey_tabs_data.hotkey_data_2;
      case "hotkey_3":
        return ref World.world.hotkey_tabs_data.hotkey_data_3;
      case "hotkey_4":
        return ref World.world.hotkey_tabs_data.hotkey_data_4;
      case "hotkey_5":
        return ref World.world.hotkey_tabs_data.hotkey_data_5;
      case "hotkey_6":
        return ref World.world.hotkey_tabs_data.hotkey_data_6;
      case "hotkey_7":
        return ref World.world.hotkey_tabs_data.hotkey_data_7;
      case "hotkey_8":
        return ref World.world.hotkey_tabs_data.hotkey_data_8;
      case "hotkey_9":
        return ref World.world.hotkey_tabs_data.hotkey_data_9;
      default:
        return ref World.world.hotkey_tabs_data.hotkey_data_1;
    }
  }

  public void initDebugHotkeys()
  {
    this.initDebugHotkeysBase();
    this.initUnitDebugHotkeys();
    this.initDebugWindowHotkeys();
    HotkeyAsset pAsset1 = new HotkeyAsset();
    pAsset1.id = "debug_autosave";
    pAsset1.default_key_1 = (KeyCode) 115;
    pAsset1.default_key_mod_1 = (KeyCode) 308;
    pAsset1.just_pressed_action = new HotkeyAction(this.debugAutosave);
    this.add(pAsset1);
    HotkeyAsset pAsset2 = new HotkeyAsset();
    pAsset2.id = "debug_next_test_map";
    pAsset2.default_key_1 = (KeyCode) 280;
    pAsset2.just_pressed_action = (HotkeyAction) (_ =>
    {
      if (SmoothLoader.isLoading())
        return;
      World.world.transition_screen.startTransition(new LoadingScreen.TransitionAction(TestMaps.loadNextMap));
    });
    this.add(pAsset2);
    HotkeyAsset pAsset3 = new HotkeyAsset();
    pAsset3.id = "debug_prev_test_map";
    pAsset3.default_key_1 = (KeyCode) 281;
    pAsset3.just_pressed_action = (HotkeyAction) (_ =>
    {
      if (SmoothLoader.isLoading())
        return;
      World.world.transition_screen.startTransition(new LoadingScreen.TransitionAction(TestMaps.loadPrevMap));
    });
    this.add(pAsset3);
  }

  private void initDebugHotkeysBase()
  {
    HotkeyAsset pAsset1 = new HotkeyAsset();
    pAsset1.id = "export_unit_sprites";
    pAsset1.default_key_1 = (KeyCode) 121;
    pAsset1.check_window_not_active = true;
    pAsset1.check_controls_locked = true;
    pAsset1.just_pressed_action = (HotkeyAction) (_ =>
    {
      WorldTip.instance.showToolbarText("Exporting unit sprites");
      AssetManager.dynamic_sprites_library.export();
    });
    this.add(pAsset1);
    HotkeyAsset pAsset2 = new HotkeyAsset();
    pAsset2.id = "autotester";
    pAsset2.default_key_1 = (KeyCode) 117;
    pAsset2.check_window_not_active = true;
    pAsset2.check_controls_locked = true;
    pAsset2.just_pressed_action = (HotkeyAction) (_ => World.world.auto_tester.toggleAutoTester());
    this.add(pAsset2);
    HotkeyAsset pAsset3 = new HotkeyAsset();
    pAsset3.id = "test_zones_border_growth";
    pAsset3.default_key_1 = (KeyCode) 111;
    pAsset3.check_window_not_active = true;
    pAsset3.check_controls_locked = true;
    pAsset3.just_pressed_action = (HotkeyAction) (_ => DebugZonesTool.actionGrowBorder());
    this.add(pAsset3);
    HotkeyAsset pAsset4 = new HotkeyAsset();
    pAsset4.id = "test_zones_abandon_zones";
    pAsset4.default_key_1 = (KeyCode) 112 /*0x70*/;
    pAsset4.check_window_not_active = true;
    pAsset4.check_controls_locked = true;
    pAsset4.just_pressed_action = (HotkeyAction) (_ =>
    {
      foreach (WorldTile tiles in World.world.tiles_list)
        World.world.buildings.addBuilding("poop", tiles);
    });
    this.add(pAsset4);
    HotkeyAsset pAsset5 = new HotkeyAsset();
    pAsset5.id = "test_colors";
    pAsset5.default_key_1 = (KeyCode) 114;
    pAsset5.check_window_not_active = true;
    pAsset5.check_controls_locked = true;
    pAsset5.just_pressed_action = (HotkeyAction) (_ =>
    {
      foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      {
        kingdom.generateBanner();
        ColorAsset random = AssetManager.kingdom_colors_library.list.GetRandom<ColorAsset>();
        kingdom.data.setColorID(AssetManager.kingdom_colors_library.list.IndexOf(random));
        if (kingdom.updateColor(random))
          World.world.zone_calculator.dirtyAndClear();
      }
    });
    this.add(pAsset5);
  }

  private void initDebugWindowHotkeys()
  {
    HotkeyAsset pAsset = new HotkeyAsset();
    pAsset.id = "debug_building_shadow_x_increase";
    pAsset.default_key_1 = (KeyCode) 120;
    pAsset.default_key_mod_1 = (KeyCode) 306;
    pAsset.just_pressed_action = new HotkeyAction(this.debugShadow);
    pAsset.check_controls_locked = true;
    pAsset.check_window_active = true;
    pAsset.check_debug_active = true;
    this.add(pAsset);
    this.clone("debug_building_shadow_x_reduce", "debug_building_shadow_x_increase");
    this.t.default_key_mod_1 = (KeyCode) 304;
    this.clone("debug_building_shadow_y_increase", "debug_building_shadow_x_increase");
    this.t.default_key_1 = (KeyCode) 121;
    this.clone("debug_building_shadow_y_reduce", "debug_building_shadow_y_increase");
    this.t.default_key_mod_1 = (KeyCode) 304;
    this.clone("debug_building_shadow_distortion_increase", "debug_building_shadow_x_increase");
    this.t.default_key_1 = (KeyCode) 100;
    this.clone("debug_building_shadow_distortion_reduce", "debug_building_shadow_distortion_increase");
    this.t.default_key_mod_1 = (KeyCode) 304;
  }

  private void initUnitDebugHotkeys()
  {
    // ISSUE: unable to decompile the method.
  }

  private void debugAutosave(HotkeyAsset pAsset)
  {
    if (!Config.isEditor)
      return;
    AutoSaveManager.autoSave(true, true);
  }

  private void debugShadow(HotkeyAsset pAsset)
  {
    if (!DebugConfig.isOn(DebugOption.DebugWindowHotkeys) || ((Object) ScrollWindow.getCurrentWindow()).name != "building_asset")
      return;
    BuildingAsset asset = BaseDebugAssetWindow<BuildingAsset, BuildingDebugAssetElement>.current_element.asset;
    if (!asset.shadow)
      return;
    switch (pAsset.id)
    {
      case "debug_building_shadow_x_increase":
        asset.shadow_bound.x += 0.05f;
        break;
      case "debug_building_shadow_x_reduce":
        asset.shadow_bound.x -= 0.05f;
        break;
      case "debug_building_shadow_y_increase":
        asset.shadow_bound.y += 0.05f;
        break;
      case "debug_building_shadow_y_reduce":
        asset.shadow_bound.y -= 0.05f;
        break;
      case "debug_building_shadow_distortion_increase":
        asset.shadow_distortion += 0.05f;
        break;
      case "debug_building_shadow_distortion_reduce":
        asset.shadow_distortion -= 0.05f;
        break;
    }
    Debug.Log((object) $"t.setShadow({asset.shadow_bound.x.ToString((IFormatProvider) CultureInfo.InvariantCulture)}f, {asset.shadow_bound.y.ToString((IFormatProvider) CultureInfo.InvariantCulture)}f, {asset.shadow_distortion.ToString((IFormatProvider) CultureInfo.InvariantCulture)}f);");
    BuildingAssetWindow.reloadSprites();
  }

  public void debug(DebugTool pTool)
  {
    foreach (HotkeyAsset hotkeyAsset in this.list)
    {
      if (hotkeyAsset.just_pressed_action == null && hotkeyAsset.holding_action == null)
      {
        if (hotkeyAsset.isJustPressed())
          pTool.setText(hotkeyAsset.id, (object) "just_pressed");
        if (hotkeyAsset.isHolding())
          pTool.setText(hotkeyAsset.id, (object) "holding");
      }
    }
  }
}
