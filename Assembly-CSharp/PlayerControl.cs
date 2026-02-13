// Decompiled with JetBrains decompiler
// Type: PlayerControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class PlayerControl
{
  public const int TOUCH_POWER_ACTIVATION_FRAMES = 5;
  internal float timer_spawn_pixels;
  private float _over_ui_timeout;
  private GameObject _gui_check_game_object;
  private static bool? _is_pointer_over_ui_object;
  private Vector2Int _last_click;
  private Vector2 _origin_touch;
  private Vector2 _current_touch;
  internal WorldTile first_pressed_tile;
  internal TileType first_pressed_type;
  internal TopTileType first_pressed_top_type;
  internal bool first_click = true;
  public double click_started_at;
  private float _click_timer;
  private int _last_check = -1;
  internal float inspect_timer_click;
  internal int touch_ticks_skip;
  private double _last_time_touched_ui;
  internal bool already_used_zoom;
  internal bool already_used_camera_drag;
  internal bool already_used_power;
  internal float controls_lock_timer;
  private PointerEventData _event_data_current_position = new PointerEventData(EventSystem.current);
  private readonly List<RaycastResult> _results = new List<RaycastResult>();
  public Vector2 square_selection_position_current;
  private Vector2 square_selection_position_start_last;
  public bool square_selection_started;
  private bool _ignore_square_selection;
  private int _square_selection_ended_frame;
  private WorldTile _cached_mouse_tile_pos;
  public const float DRAG_DETECTION_THRESHOLD_PERCENT = 0.007f;

  public bool isSelectionHappens() => this.square_selection_started;

  public bool isAnyInputHappening()
  {
    return InputHelpers.mouseSupported ? InputHelpers.GetMouseButton(0) : InputHelpers.touchCount > 0;
  }

  internal void updateControls()
  {
    this._cached_mouse_tile_pos = this.getMouseTilePos();
    if ((double) this.timer_spawn_pixels > 0.0)
      this.timer_spawn_pixels -= World.world.delta_time;
    AssetManager.hotkey_library.checkHotKeyActions();
    if (PlayerControl.controlsLocked())
      return;
    if (this.isOverUI(false))
      this._over_ui_timeout = 0.05f;
    else if ((double) this._over_ui_timeout > 0.0)
      this._over_ui_timeout -= Time.deltaTime;
    if (PlayerControl.isControllingUnit())
    {
      this.updateTouch();
    }
    else
    {
      if (DebugConfig.isOn(DebugOption.MakeUnitsFollowCursor))
      {
        WorldTile tilePosCachedFrame = this.getMouseTilePosCachedFrame();
        if (tilePosCachedFrame != null && !this.isOverUI() && InputHelpers.GetMouseButtonDown(0))
        {
          Bench.bench("test_follow");
          foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
          {
            if (unit.isFavorite() && unit.current_tile.region.island == tilePosCachedFrame.region.island)
            {
              unit.stopMovement();
              int num = (int) unit.goTo(this.getMouseTilePosCachedFrame());
            }
          }
          Bench.benchEnd("test_follow");
        }
      }
      this.checkTrailerModeButtons();
      if (!Globals.TRAILER_MODE && !((Component) World.world.canvas).gameObject.activeSelf)
        return;
      World.world.magnet.magnetAction(true);
      Boulder.checkRelease();
      if (InputHelpers.GetMouseButtonUp(0))
        this.timer_spawn_pixels = 0.0f;
      if (!this.isAnyInputHappening())
      {
        this.already_used_zoom = false;
        this.already_used_camera_drag = false;
        this.touch_ticks_skip = 0;
        this.already_used_power = false;
      }
      this.updateTouch();
      if ((double) this.controls_lock_timer > 0.0)
      {
        this.controls_lock_timer -= Time.deltaTime;
      }
      else
      {
        if (World.world.isGameplayControlsLocked())
          return;
        if ((InputHelpers.GetMouseButton(1) || InputHelpers.GetMouseButton(0)) && this.isOverUI())
          this._last_time_touched_ui = World.world.getCurSessionTime();
        if (!this.checkSquareMultiSelection())
          this.finishSquareSelection();
        if (this.isOverUI() || this.tryToMoveSelectedUnits() || this.checkClickTouchInspectSelect())
          return;
        if (!this.already_used_zoom && MoveCamera.inSpectatorMode() && !MoveCamera.camera_drag_activated && InputHelpers.GetMouseButtonUp(0) && (double) this.getDistanceBetweenOriginAndCurrentTouch() < 20.0)
        {
          Actor actorFromTile = ActionLibrary.getActorFromTile(this.getMouseTilePosCachedFrame());
          if (actorFromTile != null)
          {
            World.world.locateAndFollow(actorFromTile, (Action) null, (Action) null);
            return;
          }
        }
        if (InputHelpers.GetMouseButton(1) || InputHelpers.GetMouseButton(0))
          this.inspect_timer_click += Time.deltaTime;
        else
          this.inspect_timer_click = 0.0f;
        if (!World.world.isAnyPowerSelected())
        {
          this.checkEmptyClick();
        }
        else
        {
          if (World.world.selected_buttons.selectedButton.godPower == null || this.already_used_zoom || this.already_used_camera_drag)
            return;
          GodPower godPower = World.world.selected_buttons.selectedButton.godPower;
          if (!Globals.TRAILER_MODE)
            this.highlightCursor(godPower);
          if (InputHelpers.touchCount > 1)
          {
            this.already_used_zoom = true;
            this.already_used_camera_drag = true;
            this.already_used_power = false;
          }
          else
          {
            if (InputHelpers.touchCount > 0 && this.touch_ticks_skip < 5 && godPower.hold_action)
              return;
            bool flag1 = HotkeyLibrary.many_mod.isHolding() || godPower.hold_action;
            if (!flag1 && !godPower.ignore_fast_spawn && godPower.type == PowerActionType.PowerSpawnActor && DebugConfig.isOn(DebugOption.FastSpawn))
              flag1 = true;
            if (flag1)
            {
              if (InputHelpers.GetMouseButton(0))
              {
                if (!this.first_click && DebugConfig.isOn(DebugOption.UltraFastSpawn))
                {
                  for (int index = 0; index < 10; ++index)
                    this.clickedStart();
                }
                else
                  this.clickedStart();
              }
              else
              {
                ((Vector2Int) ref this._last_click).Set(-1, -1);
                this.first_pressed_tile = (WorldTile) null;
                this.first_pressed_type = (TileType) null;
                this.first_pressed_top_type = (TopTileType) null;
                this.first_click = true;
                this.click_started_at = (double) Time.time;
              }
            }
            else
            {
              bool flag2 = false;
              if (InputHelpers.touchSupported && InputHelpers.touchCount > 0 && InputHelpers.GetMouseButtonUp(0))
                flag2 = true;
              else if (Input.mousePresent && InputHelpers.GetMouseButtonDown(0))
                flag2 = true;
              if (flag2)
              {
                this.clickedStart();
              }
              else
              {
                this.first_pressed_tile = (WorldTile) null;
                this.first_pressed_type = (TileType) null;
                this.first_pressed_top_type = (TopTileType) null;
                this.first_click = true;
              }
            }
          }
        }
      }
    }
  }

  private void updateTouch()
  {
    if (!InputHelpers.touchSupported)
      return;
    if (InputHelpers.touchCount > 0 && InputHelpers.GetMouseButtonDown(0) && this.isOverUI())
      this.already_used_power = true;
    else if (InputHelpers.touchCount == 0)
    {
      this.touch_ticks_skip = 0;
      this.already_used_zoom = false;
      this.already_used_camera_drag = false;
      this.already_used_power = false;
      this._origin_touch = Vector2.zero;
      this._current_touch = Vector2.zero;
    }
    else
    {
      if (InputHelpers.touchCount > 0)
        ++this.touch_ticks_skip;
      if (InputHelpers.touchCount == 1)
      {
        Touch touch = Input.GetTouch(0);
        Vector2 position = ((Touch) ref touch).position;
        if (Vector2.op_Equality(this._origin_touch, Vector2.zero))
          this._origin_touch = position;
        else
          this._current_touch = position;
      }
      else
        this._origin_touch = Vector2.zero;
      if (this.already_used_power || InputHelpers.touchCount <= 1 || !this.isTouchMoreThanDragThreshold())
        return;
      this.already_used_camera_drag = true;
    }
  }

  public bool isTouchMoreThanDragThreshold()
  {
    return !Vector2.op_Equality(this._origin_touch, Vector2.zero) && !Vector2.op_Equality(this._current_touch, Vector2.zero) && (double) this.getCurrentDragDistance() > 0.0070000002160668373;
  }

  public float getCurrentDragDistance()
  {
    return Vector2.op_Equality(this._origin_touch, Vector2.zero) || Vector2.op_Equality(this._current_touch, Vector2.zero) ? 0.0f : this.getDistanceBetweenOriginAndCurrentTouch() / Mathf.Sqrt((float) (Screen.width * Screen.width + Screen.height * Screen.height));
  }

  public float getDistanceBetweenOriginAndCurrentTouch()
  {
    return Vector2.op_Equality(this._origin_touch, Vector2.zero) || Vector2.op_Equality(this._current_touch, Vector2.zero) ? 0.0f : Toolbox.DistVec2Float(this._origin_touch, this._current_touch);
  }

  private bool checkSquareMultiSelection()
  {
    if (ControllableUnit.isControllingUnit() || World.world.isAnyPowerSelected() || ScrollWindow.isWindowActive() || ScrollWindow.isAnimationActive())
      return false;
    if (MapBox.isRenderMiniMap())
    {
      this.square_selection_started = false;
      this._ignore_square_selection = true;
      return true;
    }
    if (!InputHelpers.mouseSupported || !InputHelpers.GetMouseButton(0) || InputHelpers.GetMouseButton(1) || InputHelpers.GetMouseButton(2))
      return false;
    if (!this.square_selection_started && !this._ignore_square_selection)
    {
      if (this.isOverUI())
      {
        this._ignore_square_selection = true;
      }
      else
      {
        this.square_selection_started = true;
        this.square_selection_position_current = this.getMousePos();
      }
    }
    return true;
  }

  private void finishSquareSelection()
  {
    if (!this._ignore_square_selection && this.square_selection_started)
    {
      this.checkSelectedUnits();
      ((Vector2) ref this.square_selection_position_start_last).Set(this.square_selection_position_current.x, this.square_selection_position_current.y);
      this._square_selection_ended_frame = Time.frameCount;
    }
    this._ignore_square_selection = false;
    this.square_selection_started = false;
    ((Vector2) ref this.square_selection_position_current).Set(-1f, -1f);
  }

  private void checkSelectedUnits()
  {
    // ISSUE: unable to decompile the method.
  }

  public bool isSquareSelectionMinSizeAchievedLast()
  {
    Vector2 positionStartLast = this.square_selection_position_start_last;
    Vector2 mousePos = this.getMousePos();
    int num1 = Mathf.FloorToInt(positionStartLast.x);
    int num2 = Mathf.FloorToInt(positionStartLast.y);
    int num3 = Mathf.FloorToInt(mousePos.x);
    int num4 = Mathf.FloorToInt(mousePos.y);
    if (num1 == -1 || num2 == -1 || num3 == -1 || num4 == -1)
      return false;
    int num5 = Mathf.Min(num1, num3);
    int num6 = Mathf.Max(num1, num3);
    int num7 = Mathf.Min(num2, num4);
    int num8 = Mathf.Max(num2, num4);
    int num9 = num5;
    return num6 - num9 >= 2 || num8 - num7 >= 2;
  }

  public ListPool<Actor> getUnitsToBeSelected()
  {
    Vector2 selectionPositionCurrent = this.square_selection_position_current;
    Vector2 mousePos = this.getMousePos();
    int num1 = Mathf.FloorToInt(selectionPositionCurrent.x);
    int num2 = Mathf.FloorToInt(selectionPositionCurrent.y);
    int num3 = Mathf.FloorToInt(mousePos.x);
    int num4 = Mathf.FloorToInt(mousePos.y);
    if (num1 == -1 || num2 == -1 || num3 == -1 || num4 == -1)
      return (ListPool<Actor>) null;
    int num5 = Mathf.Min(num1, num3);
    int num6 = Mathf.Max(num1, num3);
    int num7 = Mathf.Min(num2, num4);
    int num8 = Mathf.Max(num2, num4);
    if (num6 - num5 < 2 && num8 - num7 < 2)
      return (ListPool<Actor>) null;
    int num9 = Mathf.Clamp(num5, 0, MapBox.width - 1);
    int num10 = Mathf.Clamp(num6, 0, MapBox.width - 1);
    int num11 = Mathf.Clamp(num7, 0, MapBox.height - 1);
    int num12 = Mathf.Clamp(num8, 0, MapBox.height - 1);
    ListPool<Actor> tList = new ListPool<Actor>();
    for (int pX = num9; pX <= num10; ++pX)
    {
      for (int pY = num11; pY <= num12; ++pY)
        World.world.GetTile(pX, pY)?.doUnits((Action<Actor>) (tActor =>
        {
          if (tActor.isInsideSomething() || !tActor.asset.can_be_inspected)
            return;
          tList.Add(tActor);
        }));
    }
    return tList;
  }

  internal void clickedFinal(Vector2Int pPos, GodPower pPower = null, bool pTrack = true)
  {
    if (pPower == null)
      pPower = World.world.selected_buttons.selectedButton.godPower;
    if (pPower.requires_premium && !Config.hasPremium)
    {
      ScrollWindow.showWindow("steam");
    }
    else
    {
      WorldTile tile = World.world.GetTile(((Vector2Int) ref pPos).x, ((Vector2Int) ref pPos).y);
      string[] strArray = new string[5]
      {
        pPower.id,
        " ",
        null,
        null,
        null
      };
      Vector2Int pos = tile.pos;
      strArray[2] = ((Vector2Int) ref pos).x.ToString();
      strArray[3] = ":";
      pos = tile.pos;
      strArray[4] = ((Vector2Int) ref pos).y.ToString();
      LogText.log("Clicked", string.Concat(strArray));
      if (pPower.click_special_action != null)
      {
        int num1 = pPower.click_special_action(tile, pPower.id) ? 1 : 0;
      }
      else
      {
        if (this.first_pressed_type == null)
        {
          this.first_pressed_tile = tile;
          this.first_pressed_type = tile.main_type;
          this.first_pressed_top_type = tile.top_type;
        }
        if ((double) pPower.click_interval > 0.0)
        {
          if ((double) this._click_timer >= 0.0)
          {
            this._click_timer -= World.world.delta_time;
            return;
          }
          this._click_timer = pPower.click_interval;
        }
        if (pPower.click_power_action != null || pPower.click_power_brush_action != null)
        {
          if (pPower.click_power_brush_action != null)
          {
            int num2 = pPower.click_power_brush_action(tile, pPower) ? 1 : 0;
          }
          else if (pPower.click_power_action != null)
          {
            int num3 = pPower.click_power_action(tile, pPower) ? 1 : 0;
          }
          if (!pTrack)
            return;
          PowerTracker.PlusOne(pPower);
        }
        else
        {
          if (pPower.click_action == null && pPower.click_brush_action == null)
            return;
          if (pPower.click_brush_action != null)
          {
            int num4 = pPower.click_brush_action(tile, pPower.id) ? 1 : 0;
          }
          else if (pPower.click_action != null)
          {
            int num5 = pPower.click_action(tile, pPower.id) ? 1 : 0;
          }
          if (!pTrack)
            return;
          PowerTracker.PlusOne(pPower);
        }
      }
    }
  }

  private void checkEmptyClick()
  {
    if (!InputHelpers.GetMouseButtonUp(0))
      return;
    Vector2Int pVector;
    if (!PixelDetector.GetSpritePixelColorUnderMousePointer((MonoBehaviour) World.world, out pVector) || ((Vector2Int) ref pVector).x == -1)
    {
      ((Vector2Int) ref this._last_click).Set(-1, -1);
    }
    else
    {
      if (World.world.GetTile(((Vector2Int) ref pVector).x, ((Vector2Int) ref pVector).y) == null)
        return;
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        if ((double) Toolbox.Dist(unit.current_tile.posV3.x, unit.current_tile.posV3.y, (float) ((Vector2Int) ref pVector).x, (float) ((Vector2Int) ref pVector).y) <= 10.0)
        {
          WorldAction actionClick = unit.asset.action_click;
          if (actionClick != null)
          {
            int num = actionClick((BaseSimObject) unit, unit.current_tile) ? 1 : 0;
          }
        }
      }
    }
  }

  internal void clickedStart()
  {
    this.already_used_power = true;
    Vector2Int pVector;
    if (!PixelDetector.GetSpritePixelColorUnderMousePointer((MonoBehaviour) World.world, out pVector) || ((Vector2Int) ref pVector).x == -1)
    {
      ((Vector2Int) ref this._last_click).Set(-1, -1);
    }
    else
    {
      GodPower godPower = World.world.selected_buttons.selectedButton.godPower;
      string pID = Config.current_brush;
      if (godPower != null && !string.IsNullOrEmpty(godPower.force_brush))
        pID = godPower.force_brush;
      BrushData brushData = Brush.get(pID);
      if (brushData.continuous && godPower.draw_lines && ((Vector2Int) ref this._last_click).x != -1 && (((Vector2Int) ref pVector).x != ((Vector2Int) ref this._last_click).x || ((Vector2Int) ref pVector).y != ((Vector2Int) ref this._last_click).y))
      {
        int num1 = (int) ((double) Toolbox.Dist(((Vector2Int) ref pVector).x, ((Vector2Int) ref pVector).y, ((Vector2Int) ref this._last_click).x, ((Vector2Int) ref this._last_click).y) / (double) (brushData.size + 1)) + 1;
        for (int index = 0; index < num1; ++index)
        {
          Vector2 vector2_1 = new Vector2((float) ((Vector2Int) ref pVector).x, (float) ((Vector2Int) ref pVector).y);
          Vector2 vector2_2;
          // ISSUE: explicit constructor call
          ((Vector2) ref vector2_2).\u002Ector((float) ((Vector2Int) ref this._last_click).x, (float) ((Vector2Int) ref this._last_click).y);
          Vector2 vector2_3 = vector2_2;
          double num2 = (double) index / (double) num1;
          Vector2 vector2_4 = Vector2.Lerp(vector2_1, vector2_3, (float) num2);
          Vector2Int pPos;
          // ISSUE: explicit constructor call
          ((Vector2Int) ref pPos).\u002Ector((int) vector2_4.x, (int) vector2_4.y);
          if (((Vector2Int) ref pPos).x >= 0 && ((Vector2Int) ref pPos).x < MapBox.width && ((Vector2Int) ref pPos).y >= 0 && ((Vector2Int) ref pPos).y < MapBox.height)
            this.clickedFinal(pPos, godPower, false);
        }
      }
      this.clickedFinal(pVector, godPower);
      this.first_click = false;
      ((Vector2Int) ref this._last_click).Set(((Vector2Int) ref pVector).x, ((Vector2Int) ref pVector).y);
    }
  }

  private bool tryToMoveSelectedUnits()
  {
    // ISSUE: unable to decompile the method.
  }

  private Actor getActorTargetNearCursor()
  {
    Actor actorNearCursor = World.world.getActorNearCursor();
    return actorNearCursor == null || !actorNearCursor.isAlive() ? (Actor) null : actorNearCursor;
  }

  private bool tryToMoveSelectedUnit(Actor pActor, WorldTile pTile, bool pCancelBeh = true)
  {
    if (!pActor.asset.allow_strange_urge_movement || pTile.Type.block || pActor.asset.id == "dragon" && !pActor.isFlying())
      return false;
    pActor.stopSleeping();
    bool pPathOnWater = pTile.Type.liquid;
    if (pActor.isWaterCreature() || pActor.asset.is_boat)
      pPathOnWater = true;
    if (!pPathOnWater && !pTile.isSameIsland(pActor.current_tile))
      pPathOnWater = true;
    bool lava = pTile.Type.lava;
    if (pCancelBeh)
    {
      pActor.cancelAllBeh();
      pActor.stopMovement();
    }
    int num = (int) pActor.goTo(pTile, pPathOnWater, pWalkOnLava: lava);
    pActor.addStatusEffect("strange_urge", 100f, false);
    pActor.clearWait();
    return true;
  }

  private bool checkClickTouchInspectSelect()
  {
    if (!this.canInspectWithMainTouch() && !this.canInspectWithRightClick() || !DebugConfig.isOn(DebugOption.InspectObjectsOnClick) || MoveCamera.camera_drag_activated || (double) this._over_ui_timeout > 0.0 || this.already_used_zoom || this.already_used_camera_drag || this.isActionHappening() || (double) this.inspect_timer_click > 0.15000000596046448 || MoveCamera.inSpectatorMode() || World.world.getCurSessionTime() - this._last_time_touched_ui < 0.20000000298023224 || (double) Toolbox.DistVec2Float(this._origin_touch, this._current_touch) >= 20.0)
      return false;
    NameplateText cursorOverText = World.world.nameplate_manager.cursor_over_text;
    if (Object.op_Inequality((Object) cursorOverText, (Object) null))
    {
      NanoObject nanoObject = cursorOverText.nano_object;
      nanoObject.getMetaType().getAsset().selectAndInspect(nanoObject, true);
      return true;
    }
    WorldTile tilePosCachedFrame = this.getMouseTilePosCachedFrame();
    if (tilePosCachedFrame == null)
      return true;
    MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
    if (MapBox.isRenderMiniMap() && cachedMapMetaAsset != null && Zones.showMapBorders())
    {
      int num = cachedMapMetaAsset.click_action_zone(tilePosCachedFrame) ? 1 : 0;
    }
    else
    {
      bool flag = false;
      if (HotkeyLibrary.many_mod.isHolding())
      {
        if (SelectedUnit.isSet())
          flag = true;
        if (ActionLibrary.inspectUnitSelectedMeta())
          return true;
      }
      if (this.isAllowedToSelectUnits())
      {
        Actor actorNearCursor = World.world.getActorNearCursor();
        if (actorNearCursor != null)
        {
          if (SelectedUnit.isSelected(actorNearCursor))
          {
            if (SelectedUnit.isMainSelected(actorNearCursor))
            {
              if (this.isSquareSelectionMinSizeAchievedLast())
              {
                if (Time.frameCount != this._square_selection_ended_frame)
                  ActionLibrary.inspectUnit();
              }
              else
                ActionLibrary.inspectUnit();
            }
            else
              SelectedUnit.makeMainSelected(actorNearCursor);
          }
          else
          {
            if (!flag)
              SelectedUnit.clear();
            SelectedUnit.select(actorNearCursor);
            SelectedObjects.setNanoObject((NanoObject) actorNearCursor);
            PowerTabController.showTabSelectedUnit();
          }
        }
      }
      else if (!ScrollWindow.isWindowActive())
        ActionLibrary.inspectUnit();
    }
    return true;
  }

  public void updateCurrentPosition()
  {
    if (this._last_check == Time.frameCount)
      return;
    this._last_check = Time.frameCount;
    bool flag = false;
    if (InputHelpers.touchSupported && InputHelpers.touchCount != 0)
    {
      if (this._event_data_current_position == null)
        this._event_data_current_position = new PointerEventData(EventSystem.current);
      PointerEventData dataCurrentPosition = this._event_data_current_position;
      Touch touch1 = Input.GetTouch(0);
      double x = (double) ((Touch) ref touch1).position.x;
      Touch touch2 = Input.GetTouch(0);
      double y = (double) ((Touch) ref touch2).position.y;
      Vector2 vector2 = new Vector2((float) x, (float) y);
      dataCurrentPosition.position = vector2;
      flag = true;
    }
    if (!flag && InputHelpers.mouseSupported && (double) Input.mousePosition.x >= 0.0 && (double) Input.mousePosition.y >= 0.0 && (double) Input.mousePosition.x <= (double) Screen.width && (double) Input.mousePosition.y <= (double) Screen.height)
    {
      if (this._event_data_current_position == null)
        this._event_data_current_position = new PointerEventData(EventSystem.current);
      this._event_data_current_position.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      flag = true;
    }
    if (flag)
      return;
    this._event_data_current_position = (PointerEventData) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isBusyWithUI()
  {
    return ScrollWindow.isWindowActive() || ScrollWindow.isAnimationActive() || this.isOverUI();
  }

  public bool isOverUI(bool pCheckTimeout = true)
  {
    return pCheckTimeout && (double) this._over_ui_timeout > 0.0 || this.isPointerOverUIObject();
  }

  private bool isAllowedToSelectUnits() => true;

  private bool canInspectUnitWithCurrentPower()
  {
    return !World.world.isAnyPowerSelected() || World.world.selected_buttons.selectedButton.godPower.allow_unit_selection;
  }

  private bool canInspectWithRightClick()
  {
    return InputHelpers.mouseSupported && this.canInspectUnitWithCurrentPower() && Input.GetMouseButtonUp(1);
  }

  private bool canInspectWithMainTouch()
  {
    return this.canInspectUnitWithCurrentPower() && Input.GetMouseButtonUp(0);
  }

  public bool isPointerInGame()
  {
    this.updateCurrentPosition();
    return this._event_data_current_position != null;
  }

  public bool isPointerOverUIObject()
  {
    if (PlayerControl._is_pointer_over_ui_object.HasValue)
      return PlayerControl._is_pointer_over_ui_object.Value;
    this.updateCurrentPosition();
    if (this._event_data_current_position == null)
      return false;
    List<RaycastResult> results = this._results;
    EventSystem.current.RaycastAll(this._event_data_current_position, results);
    PlayerControl._is_pointer_over_ui_object = new bool?(results.Count > 0);
    return PlayerControl._is_pointer_over_ui_object.Value;
  }

  public bool isPointerOverUIButton()
  {
    this.updateCurrentPosition();
    if (this._event_data_current_position == null)
      return false;
    List<RaycastResult> results = this._results;
    EventSystem.current.RaycastAll(this._event_data_current_position, results);
    for (int index = 0; index < results.Count; ++index)
    {
      RaycastResult raycastResult = results[index];
      if (((RaycastResult) ref raycastResult).isValid)
      {
        GameObject gameObject = ((RaycastResult) ref raycastResult).gameObject;
        if (gameObject.HasComponent<Button>() || gameObject.HasComponent<EventTrigger>())
          return true;
      }
    }
    return false;
  }

  public bool isTouchOverUI(Touch pTouch)
  {
    return ((Touch) ref pTouch).phase != 3 && ((Touch) ref pTouch).phase != 4 && EventSystem.current.IsPointerOverGameObject(((Touch) ref pTouch).fingerId);
  }

  public bool isPointerOverUIScroll()
  {
    this.updateCurrentPosition();
    if (this._event_data_current_position == null)
      return false;
    List<RaycastResult> results = this._results;
    if (results.Count == 0)
      EventSystem.current.RaycastAll(this._event_data_current_position, results);
    for (int index = 0; index < results.Count; ++index)
    {
      RaycastResult raycastResult = results[index];
      if (!((RaycastResult) ref raycastResult).isValid)
        return false;
      this._gui_check_game_object = ((RaycastResult) ref raycastResult).gameObject;
      if (Object.op_Equality((Object) this._gui_check_game_object, (Object) null))
        return false;
      if (this._gui_check_game_object.HasComponent<ScrollRectExtended>())
        return true;
      if (((Object) this._gui_check_game_object).name == "Scroll View")
      {
        Transform transform = this._gui_check_game_object.transform.Find("Viewport/Content");
        if (Object.op_Inequality((Object) transform, (Object) null))
        {
          Vector2 sizeDelta = this._gui_check_game_object.GetComponent<RectTransform>().sizeDelta;
          if ((double) ((Component) transform).GetComponent<RectTransform>().sizeDelta.y > (double) sizeDelta.y)
            return true;
        }
      }
    }
    return false;
  }

  public bool isActionHappening()
  {
    if (Earthquake.isQuakeActive())
      return true;
    AutoTesterBot autoTester = World.world.auto_tester;
    return (autoTester != null ? (autoTester.active ? 1 : 0) : 0) == 0 && (Input.touchSupported && Input.touchCount > 1 || Input.mousePresent && (Input.GetMouseButton(0) || Input.GetMouseButton(2)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool controlsLocked()
  {
    return MapBox.instance.tutorial.isActive() || Config.lockGameControls || Config.isDraggingItem();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isControllingUnit()
  {
    return ControllableUnit.isControllingUnit() || MapBox.instance.stack_effects.isLocked();
  }

  private void highlightCursor(GodPower pPower)
  {
    if (!Config.isComputer && !Config.isEditor)
      return;
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    if (mouseTilePos == null)
      return;
    World.world.flash_effects.flashPixel(mouseTilePos, 10);
    if (pPower == null)
      return;
    if (!string.IsNullOrEmpty(pPower.force_brush))
    {
      this.highlightFrom(mouseTilePos, Brush.get(pPower.force_brush));
    }
    else
    {
      if (!pPower.highlight && !pPower.show_tool_sizes)
        return;
      this.highlightFrom(mouseTilePos, Config.current_brush_data);
    }
  }

  private void highlightFrom(WorldTile pTile, BrushData pBrushData)
  {
    for (int index = 0; index < pBrushData.pos.Length; ++index)
    {
      WorldTile tile = World.world.GetTile(pBrushData.pos[index].x + pTile.x, pBrushData.pos[index].y + pTile.y);
      if (tile != null)
        World.world.flash_effects.flashPixel(tile, 20);
    }
  }

  public void clearLateUpdate()
  {
    PlayerControl._is_pointer_over_ui_object = new bool?();
    this._results.Clear();
  }

  public void clear()
  {
    this._results.Clear();
    this.first_click = true;
    this._cached_mouse_tile_pos = (WorldTile) null;
  }

  public Vector2 getMousePos()
  {
    return Vector2.op_Implicit(World.world.camera.ScreenToWorldPoint(Vector2.op_Implicit(Vector2.op_Implicit(Input.mousePosition))));
  }

  public WorldTile getMouseTilePosCachedFrame() => this._cached_mouse_tile_pos;

  public WorldTile getMouseTilePos()
  {
    Vector2Int pVector;
    return !PixelDetector.GetSpritePixelColorUnderMousePointer((MonoBehaviour) World.world, out pVector) ? (WorldTile) null : World.world.GetTile(((Vector2Int) ref pVector).x, ((Vector2Int) ref pVector).y);
  }

  public bool getTouchPos(out Touch pTouch, bool pOnlyGameplay = false)
  {
    pTouch = new Touch();
    bool touchPos = false;
    foreach (Touch touch in Input.touches)
    {
      if (!pOnlyGameplay || !World.world.isTouchOverUI(touch))
      {
        pTouch = touch;
        touchPos = true;
        break;
      }
    }
    return touchPos;
  }

  internal void checkTrailerModeButtons()
  {
    if (!Globals.TRAILER_MODE)
      return;
    if (!ScrollWindow.isWindowActive() && Input.GetKeyDown((KeyCode) 290))
      ((Component) World.world.canvas).gameObject.SetActive(!((Component) World.world.canvas).gameObject.activeSelf);
    if (Input.GetKeyDown((KeyCode) 96 /*0x60*/))
      TrailerModeSettings.startEvent();
    if (Input.GetKeyDown((KeyCode) 289))
      DebugConfig.switchOption(DebugOption.FastSpawn);
    if (Input.GetKeyDown((KeyCode) 288))
      DebugConfig.switchOption(DebugOption.SonicSpeed);
    if (Input.GetKeyDown((KeyCode) 32 /*0x20*/))
      Config.paused = !Config.paused;
    if (Input.GetKeyDown((KeyCode) 257) || Input.GetKeyDown((KeyCode) 49))
      World.world.selected_buttons.clickPowerButton(PowerButton.get("seeds"));
    else if (Input.GetKeyDown((KeyCode) 258) || Input.GetKeyDown((KeyCode) 50))
      World.world.selected_buttons.clickPowerButton(PowerButton.get("tile_shallow_waters"));
    else if (Input.GetKeyDown((KeyCode) 259) || Input.GetKeyDown((KeyCode) 51))
      World.world.selected_buttons.clickPowerButton(PowerButton.get("fruit_bush"));
    else if (Input.GetKeyDown((KeyCode) 260) || Input.GetKeyDown((KeyCode) 52))
      World.world.selected_buttons.clickPowerButton(PowerButton.get("cat"));
    else if (Input.GetKeyDown((KeyCode) 261) || Input.GetKeyDown((KeyCode) 53))
    {
      World.world.selected_buttons.clickPowerButton(PowerButton.get("atomic_bomb"));
    }
    else
    {
      if (!Input.GetKeyDown((KeyCode) 262) && !Input.GetKeyDown((KeyCode) 54))
        return;
      World.world.selected_buttons.clickPowerButton(PowerButton.get("czar_bomba"));
    }
  }
}
