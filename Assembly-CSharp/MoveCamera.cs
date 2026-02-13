// Decompiled with JetBrains decompiler
// Type: MoveCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class MoveCamera : BaseMapObject
{
  private Vector3 _origin;
  private bool _is_zooming;
  internal const float ORTHOGRAPHIC_SIZE_MIN = 10f;
  internal float orthographic_size_max = 130f;
  private float _target_zoom;
  private Vector3 _first_touch;
  internal Camera main_camera;
  internal static MoveCamera instance;
  private WhooshState _whoosh_state;
  private Action _focus_reached_callback;
  private Action _focus_cancel_callback;
  private float _focus_zoom = -1000000f;
  private float _focus_timer;
  private static Actor _focus_unit;
  private static bool _spectator_mode;
  private static float _touch_dist;
  public static bool camera_drag_activated;
  public static int camera_drag_activated_frame;
  public static bool camera_drag_run;
  private float _last_width;
  private float _last_height;
  private bool _first_touch_on_ui;
  internal float camera_zoom_speed = 5f;
  internal float camera_move_speed = 0.01f;
  internal float camera_move_max = 0.06f;
  private Vector2 _move_velocity;
  private readonly Vector2?[] _old_touch_positions = new Vector2?[2];
  private Vector2 _old_touch_vector;
  private float _old_touch_distance;
  private Rect _visible_bounds;
  private Rect _visible_bounds_without_power_bar;
  public float power_bar_position_y;
  private bool _skip_reset_zoom;
  private bool _mouse_controls_used_last;

  private void Awake()
  {
    MoveCamera.instance = this;
    this.main_camera = Camera.main;
  }

  internal override void create()
  {
    base.create();
    this.resetZoom();
    this._target_zoom = this.main_camera.orthographicSize;
  }

  public static Actor getFocusUnit() => MoveCamera._focus_unit;

  public static void setFocusUnit(Actor pActor) => MoveCamera._focus_unit = pActor;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool hasFocusUnit() => MoveCamera._focus_unit != null;

  public static bool isCameraFollowingUnit(Actor pActor) => MoveCamera._focus_unit == pActor;

  internal void focusOn(Vector3 pPos)
  {
    this.clearFocusUnitAndUnselect();
    this._target_zoom = 15f;
    this._focus_zoom = this._target_zoom;
    pPos.z = ((Component) this).transform.position.z;
    ((Component) this).transform.position = pPos;
  }

  internal void focusOn(Vector3 pPos, Action pFocusReachedCallback, Action pFocusCancelCallback)
  {
    this.clearFocusUnitAndUnselect();
    this._target_zoom = 15f;
    this._focus_zoom = this._target_zoom;
    this._focus_reached_callback = pFocusReachedCallback;
    this._focus_cancel_callback = pFocusCancelCallback;
    pPos.z = ((Component) this).transform.position.z;
    ((Component) this).transform.position = pPos;
  }

  internal void focusOnAndFollow(
    Actor pActor,
    Action pFocusReachedCallback,
    Action pFocusCancelCallback)
  {
    this.clearFocusUnitAndUnselect();
    Config.ui_main_hidden = false;
    this._target_zoom = 15f;
    this._focus_zoom = this._target_zoom;
    this._focus_reached_callback = pFocusReachedCallback;
    this._focus_cancel_callback = pFocusCancelCallback;
    MoveCamera._focus_unit = pActor;
    this._focus_timer = 0.0f;
    WorldTip.addWordReplacement("$name$", MoveCamera._focus_unit.coloredName);
    WorldTip.showNowTop("tip_following_unit");
    PowerTracker.spectatingUnit(MoveCamera._focus_unit.getName());
    PowerButtonSelector.instance.setPower(PowerButtonSelector.instance.followUnit);
  }

  internal void resetZoom()
  {
    int num = Screen.width >= Screen.height ? Screen.height / 4 : Screen.width / 4;
    this.orthographic_size_max = MapBox.width <= MapBox.height ? (float) (int) ((double) MapBox.height * 1.1000000238418579) : (float) (int) ((double) MapBox.width * 1.1000000238418579);
    if ((double) num > (double) this.orthographic_size_max)
      num = (int) this.orthographic_size_max;
    this._target_zoom = (float) num;
    this.main_camera.orthographicSize = Mathf.Clamp(this._target_zoom, 10f, this.orthographic_size_max);
    World.world.setZoomOrthographic(this.main_camera.orthographicSize);
    this._mouse_controls_used_last = false;
    this.main_camera.farClipPlane = (float) MapBox.height * 1.1f;
  }

  public void forceZoom(float pZoom)
  {
    this._target_zoom = pZoom;
    this.zoomToBounds(true);
  }

  public void setTargetZoom(float pValue) => this._target_zoom = pValue;

  public float getTargetZoom() => this._target_zoom;

  private void updateZoomControls()
  {
    if (InputHelpers.touchSupported)
    {
      bool flag = false;
      if (UltimateJoystick.getJoyCount() == 2)
        flag = UltimateJoystick.GetJoystickState("JoyRight") || UltimateJoystick.GetJoystickState("JoyLeft");
      if (flag)
        return;
      if (InputHelpers.touchCount == 2 & (!World.world.player_control.already_used_power || ControllableUnit.isControllingUnit()))
      {
        World.world.player_control.already_used_zoom = true;
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);
        Vector2 vector2_1 = Vector2.op_Subtraction(Vector2.op_Subtraction(((Touch) ref touch1).position, ((Touch) ref touch1).deltaPosition), Vector2.op_Subtraction(((Touch) ref touch2).position, ((Touch) ref touch2).deltaPosition));
        double magnitude1 = (double) ((Vector2) ref vector2_1).magnitude;
        Vector2 vector2_2 = Vector2.op_Subtraction(((Touch) ref touch1).position, ((Touch) ref touch2).position);
        double magnitude2 = (double) ((Vector2) ref vector2_2).magnitude;
        this._target_zoom += (float) ((magnitude1 - magnitude2) * 0.20000000298023224 * ((double) this.main_camera.orthographicSize * 0.014999999664723873));
      }
    }
    if (!MoveCamera.inSpectatorMode())
      return;
    this.followFocusUnit();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool inSpectatorMode()
  {
    if (MoveCamera._spectator_mode && !MoveCamera.hasFocusUnit())
      MoveCamera.instance.clearFocusUnitAndUnselect();
    MoveCamera._spectator_mode = MoveCamera.hasFocusUnit();
    return MoveCamera._spectator_mode;
  }

  private void checkFocusReached()
  {
    if ((double) this.main_camera.orthographicSize == (double) this._focus_zoom)
    {
      if (this._focus_reached_callback != null)
        this._focus_reached_callback();
      this.clearFocus();
    }
    if ((double) this._target_zoom == (double) this._focus_zoom)
      return;
    if (this._focus_cancel_callback != null)
      this._focus_cancel_callback();
    this.clearFocus();
  }

  private void followFocusUnit()
  {
    if (!MoveCamera.hasFocusUnit())
      return;
    Actor focusUnit = MoveCamera._focus_unit;
    if (!focusUnit.isAlive())
    {
      Actor a1 = focusUnit.attackedBy?.a;
      if (a1 != null && a1.isAlive())
      {
        WorldTip.addWordReplacement("$name$", focusUnit.coloredName);
        WorldTip.addWordReplacement("$killer$", a1.coloredName);
        WorldTip.showNowTop("tip_followed_unit_killed");
        Actor a2 = a1.a;
        focusUnit.attackedBy = (BaseSimObject) null;
        MoveCamera.setFocusUnit(a2);
        this._focus_timer = 0.0f;
      }
      else
      {
        WorldTip.addWordReplacement("$name$", focusUnit.coloredName);
        WorldTip.showNowTop("tip_followed_unit_died");
        this.clearFocusUnitAndUnselect();
      }
    }
    else if (MoveCamera.camera_drag_run || InputHelpers.touchCount > 0)
    {
      this._focus_timer = 0.0f;
    }
    else
    {
      Vector3 vector3 = Vector2.op_Implicit(focusUnit.current_position);
      vector3.z = ((Component) this).transform.position.z;
      if ((double) this._focus_timer <= 1.0)
      {
        this._focus_timer += Time.deltaTime;
        this._focus_timer = Mathf.Clamp(this._focus_timer, 0.0f, 1f);
        vector3.x = iTween.easeOutCubic(((Component) this).transform.position.x, vector3.x, this._focus_timer);
        vector3.y = iTween.easeOutCubic(((Component) this).transform.position.y, vector3.y, this._focus_timer);
      }
      ((Component) this).transform.position = vector3;
    }
  }

  private void clearFocus()
  {
    this._focus_reached_callback = (Action) null;
    this._focus_cancel_callback = (Action) null;
    this._focus_zoom = -1000000f;
  }

  public static void clearFocusUnitOnly() => MoveCamera._focus_unit = (Actor) null;

  internal void clearFocusUnitAndUnselect()
  {
    MoveCamera.clearFocusUnitOnly();
    this._focus_timer = 0.0f;
    if (!World.world.isSelectedPower("follow_unit"))
      return;
    PowerButtonSelector.instance.unselectAll();
  }

  private void zoomToBounds(bool pForce = false)
  {
    this._target_zoom = Mathf.Clamp(this._target_zoom, 10f, World.world.player_control.isSelectionHappens() ? World.world.quality_changer.getZoomRateBoundLow() : this.orthographic_size_max);
    if ((double) this.main_camera.orthographicSize == (double) this._target_zoom)
      return;
    if ((double) this._target_zoom > (double) this.main_camera.orthographicSize)
    {
      this.main_camera.orthographicSize += (float) ((double) Time.deltaTime * (double) this.camera_zoom_speed * ((double) Mathf.Abs(this.main_camera.orthographicSize - this._target_zoom) + 5.0));
      if ((double) this.main_camera.orthographicSize > (double) this._target_zoom)
        this.main_camera.orthographicSize = Mathf.Clamp(this._target_zoom, 10f, this.orthographic_size_max);
    }
    else if ((double) this._target_zoom < (double) this.main_camera.orthographicSize)
    {
      this.main_camera.orthographicSize -= (float) ((double) Time.deltaTime * (double) this.camera_zoom_speed * ((double) Mathf.Abs(this.main_camera.orthographicSize - this._target_zoom) + 5.0));
      if ((double) this.main_camera.orthographicSize < (double) this._target_zoom)
        this.main_camera.orthographicSize = Mathf.Clamp(this._target_zoom, 10f, this.orthographic_size_max);
    }
    if (pForce)
      this.main_camera.orthographicSize = this._target_zoom;
    World.world.setZoomOrthographic(this.main_camera.orthographicSize);
  }

  private void updateMouseCameraDrag()
  {
    if (ControllableUnit.isControllingUnit())
      return;
    MoveCamera.camera_drag_run = false;
    bool flag1 = false;
    bool flag2 = false;
    if (InputHelpers.mouseSupported)
    {
      flag1 = this.checkMouseInputDown();
      flag2 = this.checkMouseInput();
    }
    if (!flag2)
      this.clearTouches();
    else if (flag1 && World.world.isOverUI())
    {
      this.clearTouches();
    }
    else
    {
      if (flag1 && (double) this._origin.x == -1.0 && (double) this._origin.z == -1.0)
        this._origin = this.getMousePos();
      if ((double) this._origin.x == -1.0 && (double) this._origin.y == -1.0 && (double) this._origin.z == -1.0 || !flag2)
        return;
      MoveCamera.camera_drag_run = true;
      Vector3 position = ((Component) this).transform.position;
      position.z = 0.0f;
      Vector3 vector3_1 = Vector3.op_Subtraction(this.getMousePos(), position);
      if ((double) Toolbox.DistVec3(this._origin, this.getMousePos()) > 0.10000000149011612)
      {
        MoveCamera.camera_drag_activated = true;
        MoveCamera.camera_drag_activated_frame = Time.frameCount;
      }
      Vector3 vector3_2 = Vector3.op_Subtraction(this._origin, vector3_1);
      vector3_2.z = 0.0f;
      if (InputHelpers.touchSupported)
      {
        MoveCamera._touch_dist = Toolbox.DistVec3(this._first_touch, this.getTouchPos(true));
        if (World.world.player_control.touch_ticks_skip > 5)
        {
          if ((double) MoveCamera._touch_dist >= 20.0 || (double) World.world.player_control.touch_ticks_skip > 0.30000001192092896)
          {
            World.world.player_control.already_used_zoom = true;
            World.world.player_control.already_used_power = false;
          }
        }
        else if (InputHelpers.touchCount == 1)
          return;
      }
      if (!InputHelpers.mouseSupported)
        return;
      Vector3 pOldPosition = position;
      ((Component) this).transform.position = vector3_2;
      Vector2 vector2_1 = Vector2.op_Implicit(Vector3.op_Subtraction(vector3_2, pOldPosition));
      if ((double) ((Vector2) ref vector2_1).magnitude > 0.0099999997764825821)
      {
        Vector2 vector2_2 = Vector2.op_Multiply(vector2_1, 0.2f);
        this.addVelocity(vector2_2.x, vector2_2.y);
        this._mouse_controls_used_last = true;
      }
      else
        this._move_velocity = Vector2.zero;
      this.checkDistanceMoved(pOldPosition);
      this.cameraToBounds();
    }
  }

  private void updateVelocity()
  {
    Vector2 moveVelocity = this._move_velocity;
    if ((double) moveVelocity.x == 0.0 && (double) moveVelocity.y == 0.0)
      return;
    this._move_velocity = Vector2.op_Multiply(this._move_velocity, this.getDecayFactor());
    if ((double) Mathf.Abs(this._move_velocity.x) < 0.0099999997764825821)
      this._move_velocity.x = 0.0f;
    if ((double) Mathf.Abs(this._move_velocity.y) < 0.0099999997764825821)
      this._move_velocity.y = 0.0f;
    if ((!InputHelpers.mouseSupported ? 0 : (InputHelpers.GetMouseButton(1) ? 1 : 0)) != 0)
      return;
    Vector3 vector3 = Vector2.op_Implicit(this._move_velocity);
    Transform transform = ((Component) this).transform;
    transform.position = Vector3.op_Addition(transform.position, vector3);
    this.setWhooshState(WhooshState.NeedWhoosh);
    this.cameraToBounds();
  }

  private float getDecayFactor()
  {
    return this._mouse_controls_used_last ? Mathf.Pow(0.8f, Time.deltaTime / 0.0166666675f) : 0.8f;
  }

  private void checkDistanceMoved(Vector3 pOldPosition)
  {
    double num1 = (double) Toolbox.DistVec3(((Component) this).transform.position, pOldPosition);
    Vector3 worldPoint = this.main_camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, this.main_camera.nearClipPlane));
    Vector3 vector3 = Vector3.op_Subtraction(this.main_camera.ScreenToWorldPoint(new Vector3((float) Screen.width, (float) Screen.height, this.main_camera.nearClipPlane)), worldPoint);
    float num2 = ((Vector3) ref vector3).magnitude * 0.007f;
    if (num1 > (double) num2)
    {
      GodPower selectedPower = World.world.selected_power;
      if ((selectedPower != null ? (selectedPower.set_used_camera_drag_on_long_move ? 1 : 0) : 0) != 0)
        World.world.player_control.already_used_camera_drag = true;
    }
    if (num1 <= (double) num2 * 1.2000000476837158)
      return;
    this.setWhooshState(WhooshState.NeedWhoosh);
  }

  private bool checkMouseInputDown()
  {
    return InputHelpers.GetMouseButtonDown(1) || InputHelpers.GetMouseButtonDown(2) || InputHelpers.GetMouseButtonDown(0) && (!Input.mousePresent || MapBox.isRenderMiniMap());
  }

  private bool checkMouseInput()
  {
    return InputHelpers.GetMouseButton(1) || InputHelpers.GetMouseButton(2) || InputHelpers.GetMouseButton(0) && !Input.mousePresent;
  }

  private void clearTouches()
  {
    ((Vector3) ref this._first_touch).Set(-1f, -1f, -1f);
    ((Vector3) ref this._origin).Set(-1f, -1f, -1f);
    if (!MoveCamera.camera_drag_activated || Time.frameCount <= MoveCamera.camera_drag_activated_frame + 2)
      return;
    MoveCamera.camera_drag_activated = false;
  }

  private void cameraToBounds()
  {
    ((Component) this).transform.position = new Vector3()
    {
      x = Mathf.Clamp(((Component) this).transform.position.x, 0.0f, (float) MapBox.width),
      y = Mathf.Clamp(((Component) this).transform.position.y, 0.0f, (float) MapBox.height),
      z = -0.5f
    };
    World.world.nameplate_manager.update();
  }

  private Vector3 getTouchPos(bool pScreenCoords = false)
  {
    Vector2 vector2 = new Vector2();
    int num = 0;
    int touchCount = InputHelpers.touchCount;
    for (int index = 0; index < touchCount; ++index)
    {
      Touch touch = Input.GetTouch(index);
      if (((Touch) ref touch).phase != 4 && ((Touch) ref touch).phase != 3)
      {
        vector2 = Vector2.op_Addition(vector2, ((Touch) ref touch).position);
        ++num;
      }
    }
    Vector3 vector3 = Vector2.op_Implicit(Vector2.op_Division(vector2, (float) num));
    return pScreenCoords ? vector3 : this.main_camera.ScreenToWorldPoint(vector3);
  }

  private Vector3 getMousePos()
  {
    return InputHelpers.mouseSupported ? Vector2.op_Implicit(World.world.getMousePos()) : Vector3.one;
  }

  private void setWhooshState(WhooshState pState)
  {
    if (pState == WhooshState.NeedWhoosh && this._whoosh_state == WhooshState.WhooshPlayed)
      return;
    this._whoosh_state = pState;
  }

  private bool isNoInputDetected()
  {
    return (double) this._move_velocity.x == 0.0 && (double) this._move_velocity.y == 0.0 && InputHelpers.touchCount == 0 && !InputHelpers.GetMouseButton(0) && !InputHelpers.GetMouseButton(1) && !InputHelpers.GetMouseButton(2);
  }

  private void LateUpdate()
  {
    this.updateVisibleBounds();
    if (World.world.tutorial.isActive())
      return;
    if (this._whoosh_state == WhooshState.NeedWhoosh)
      this.setWhooshState(WhooshState.WhooshPlayed);
    if (!this.isNoInputDetected())
      return;
    this.setWhooshState(WhooshState.Idle);
  }

  private void updateVisibleBounds()
  {
    this.power_bar_position_y = Vector2.op_Implicit(World.world.camera.ScreenToWorldPoint(ToolbarButtons.instance.getPowerBarLeftCornerViewportPos())).y;
    if ((double) this.power_bar_position_y < 0.0)
      this.power_bar_position_y = 0.0f;
    Camera mainCamera = this.main_camera;
    float nearClipPlane = mainCamera.nearClipPlane;
    Vector3 worldPoint1 = mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, nearClipPlane));
    Vector3 worldPoint2 = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, nearClipPlane));
    ((Rect) ref this._visible_bounds).x = worldPoint1.x;
    ((Rect) ref this._visible_bounds).y = worldPoint1.y;
    ((Rect) ref this._visible_bounds).width = worldPoint2.x - ((Rect) ref this._visible_bounds).x;
    ((Rect) ref this._visible_bounds).height = worldPoint2.y - ((Rect) ref this._visible_bounds).y;
    ((Rect) ref this._visible_bounds_without_power_bar).x = worldPoint1.x;
    ((Rect) ref this._visible_bounds_without_power_bar).y = this.power_bar_position_y;
    ((Rect) ref this._visible_bounds_without_power_bar).width = worldPoint2.x - ((Rect) ref this._visible_bounds_without_power_bar).x;
    ((Rect) ref this._visible_bounds_without_power_bar).height = worldPoint2.y - ((Rect) ref this._visible_bounds_without_power_bar).y;
  }

  public bool isWithinCameraView(Vector2 pPos)
  {
    Rect visibleBounds = this._visible_bounds;
    return this.checkBounds(pPos, visibleBounds);
  }

  public bool isWithinCameraViewNotPowerBar(Vector2 pPos)
  {
    Rect boundsWithoutPowerBar = this._visible_bounds_without_power_bar;
    return this.checkBounds(pPos, boundsWithoutPowerBar);
  }

  private bool checkBounds(Vector2 pPos, Rect pBounds) => ((Rect) ref pBounds).Contains(pPos);

  public void update()
  {
    if (World.world.tutorial.isActive())
      return;
    int pixelWidth = this.main_camera.pixelWidth;
    int pixelHeight = this.main_camera.pixelHeight;
    if ((double) this._last_width != (double) pixelWidth || (double) this._last_height != (double) pixelHeight)
    {
      this._last_width = (float) pixelWidth;
      this._last_height = (float) pixelHeight;
      if (this._skip_reset_zoom)
        this._skip_reset_zoom = false;
      else
        this.resetZoom();
    }
    else
    {
      if (Globals.TRAILER_MODE)
        this.updateTrailerMode();
      if (InputHelpers.touchCount > 0)
      {
        Touch touch = Input.GetTouch(0);
        if (((Touch) ref touch).phase == null && World.world.isOverUI())
          this._first_touch_on_ui = true;
      }
      else
        this._first_touch_on_ui = false;
      if (!ScrollWindow.isWindowActive() && (!World.world.isOverUI() || MoveCamera.inSpectatorMode()))
        this.updateZoomControls();
      if ((double) this._target_zoom != (double) this.main_camera.orthographicSize)
        this.zoomToBounds();
      if ((double) this._focus_zoom > -1000000.0)
        this.checkFocusReached();
      if (World.world.isGameplayControlsLocked() || ScrollWindow.isAnimationActive() || this._first_touch_on_ui)
      {
        this.clearTouches();
        this._old_touch_positions[0] = new Vector2?();
        this._old_touch_positions[1] = new Vector2?();
      }
      else
      {
        if (InputHelpers.touchSupported)
          this.updateMobileCamera();
        if (!InputHelpers.mouseSupported || InputHelpers.touchSupported && InputHelpers.touchCount > 0)
          return;
        this.updateMouseCameraDrag();
        if (ScrollWindow.isWindowActive() || ControllableUnit.isControllingUnit())
          return;
        this.updateVelocity();
      }
    }
  }

  public Vector2 getVelocity() => this._move_velocity;

  private bool ignoreTouchControls()
  {
    return World.world.isOverUI() || ScrollWindow.isWindowActive() || ScrollWindow.isAnimationActive();
  }

  private void updateMobileCamera()
  {
    if (InputHelpers.touchCount == 0)
    {
      this._old_touch_positions[0] = new Vector2?();
      this._old_touch_positions[1] = new Vector2?();
    }
    else
    {
      if (World.world.isAnyPowerSelected() && World.world.selected_power.hold_action && InputHelpers.touchCount == 1 || World.world.player_control.already_used_power || ControllableUnit.isControllingUnit())
        return;
      Vector3 position1 = ((Component) this).transform.position;
      if (InputHelpers.touchCount == 1)
      {
        if (!this._old_touch_positions[0].HasValue || this._old_touch_positions[1].HasValue)
        {
          Vector2?[] oldTouchPositions = this._old_touch_positions;
          Touch touch = Input.GetTouch(0);
          Vector2? nullable = new Vector2?(((Touch) ref touch).position);
          oldTouchPositions[0] = nullable;
          this._old_touch_positions[1] = new Vector2?();
        }
        else
        {
          Touch touch = Input.GetTouch(0);
          Vector2 position2 = ((Touch) ref touch).position;
          Vector3 position3 = ((Component) this).transform.position;
          Transform transform = ((Component) this).transform;
          Vector2? oldTouchPosition = this._old_touch_positions[0];
          Vector2 vector2 = position2;
          Vector2? nullable1 = oldTouchPosition.HasValue ? new Vector2?(Vector2.op_Subtraction(oldTouchPosition.GetValueOrDefault(), vector2)) : new Vector2?();
          float orthographicSize = this.main_camera.orthographicSize;
          Vector2? nullable2 = nullable1.HasValue ? new Vector2?(Vector2.op_Multiply(nullable1.GetValueOrDefault(), orthographicSize)) : new Vector2?();
          float pixelHeight = (float) this.main_camera.pixelHeight;
          Vector3 vector3_1 = Vector2.op_Implicit((nullable2.HasValue ? new Vector2?(Vector2.op_Multiply(Vector2.op_Division(nullable2.GetValueOrDefault(), pixelHeight), 2f)) : new Vector2?()).Value);
          Vector3 vector3_2 = transform.TransformDirection(vector3_1);
          ((Component) this).transform.position = Vector3.op_Addition(position3, vector3_2);
          this._old_touch_positions[0] = new Vector2?(position2);
          this.cameraToBounds();
        }
      }
      else if (!this._old_touch_positions[1].HasValue)
      {
        Vector2?[] oldTouchPositions1 = this._old_touch_positions;
        Touch touch = Input.GetTouch(0);
        Vector2? nullable3 = new Vector2?(((Touch) ref touch).position);
        oldTouchPositions1[0] = nullable3;
        Vector2?[] oldTouchPositions2 = this._old_touch_positions;
        touch = Input.GetTouch(1);
        Vector2? nullable4 = new Vector2?(((Touch) ref touch).position);
        oldTouchPositions2[1] = nullable4;
        Vector2? oldTouchPosition1 = this._old_touch_positions[0];
        Vector2? oldTouchPosition2 = this._old_touch_positions[1];
        this._old_touch_vector = (oldTouchPosition1.HasValue & oldTouchPosition2.HasValue ? new Vector2?(Vector2.op_Subtraction(oldTouchPosition1.GetValueOrDefault(), oldTouchPosition2.GetValueOrDefault())) : new Vector2?()).Value;
        this._old_touch_distance = ((Vector2) ref this._old_touch_vector).magnitude;
      }
      else
      {
        Vector2 vector2_1;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_1).\u002Ector((float) this.main_camera.pixelWidth, (float) this.main_camera.pixelHeight);
        Vector2[] vector2Array1 = new Vector2[2];
        Touch touch1 = Input.GetTouch(0);
        vector2Array1[0] = ((Touch) ref touch1).position;
        Touch touch2 = Input.GetTouch(1);
        vector2Array1[1] = ((Touch) ref touch2).position;
        Vector2[] vector2Array2 = vector2Array1;
        Vector2 vector2_2 = Vector2.op_Subtraction(vector2Array2[0], vector2Array2[1]);
        float magnitude = ((Vector2) ref vector2_2).magnitude;
        Transform transform1 = ((Component) this).transform;
        Vector3 position4 = transform1.position;
        Transform transform2 = ((Component) this).transform;
        Vector2? oldTouchPosition = this._old_touch_positions[0];
        Vector2? nullable5 = this._old_touch_positions[1];
        Vector2? nullable6 = oldTouchPosition.HasValue & nullable5.HasValue ? new Vector2?(Vector2.op_Addition(oldTouchPosition.GetValueOrDefault(), nullable5.GetValueOrDefault())) : new Vector2?();
        Vector2 vector2_3 = vector2_1;
        Vector2? nullable7;
        if (!nullable6.HasValue)
        {
          nullable5 = new Vector2?();
          nullable7 = nullable5;
        }
        else
          nullable7 = new Vector2?(Vector2.op_Subtraction(nullable6.GetValueOrDefault(), vector2_3));
        Vector2? nullable8 = nullable7;
        float orthographicSize = this.main_camera.orthographicSize;
        Vector2? nullable9;
        if (!nullable8.HasValue)
        {
          nullable6 = new Vector2?();
          nullable9 = nullable6;
        }
        else
          nullable9 = new Vector2?(Vector2.op_Multiply(nullable8.GetValueOrDefault(), orthographicSize));
        Vector2? nullable10 = nullable9;
        float y = vector2_1.y;
        Vector3 vector3_3 = Vector2.op_Implicit((nullable10.HasValue ? new Vector2?(Vector2.op_Division(nullable10.GetValueOrDefault(), y)) : new Vector2?()).Value);
        Vector3 vector3_4 = transform2.TransformDirection(vector3_3);
        transform1.position = Vector3.op_Addition(position4, vector3_4);
        if ((double) magnitude != 0.0 && (double) this._old_touch_distance != (double) magnitude)
          this.main_camera.orthographicSize = Mathf.Clamp(this.main_camera.orthographicSize * (this._old_touch_distance / magnitude), 10f, this.orthographic_size_max);
        World.world.setZoomOrthographic(this.main_camera.orthographicSize);
        Transform transform3 = ((Component) this).transform;
        transform3.position = Vector3.op_Subtraction(transform3.position, ((Component) this).transform.TransformDirection(Vector2.op_Implicit(Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Subtraction(Vector2.op_Addition(vector2Array2[0], vector2Array2[1]), vector2_1), this.main_camera.orthographicSize), vector2_1.y))));
        this.cameraToBounds();
        this._old_touch_positions[0] = new Vector2?(vector2Array2[0]);
        this._old_touch_positions[1] = new Vector2?(vector2Array2[1]);
        this._old_touch_vector = vector2_2;
        this._old_touch_distance = magnitude;
        World.world.player_control.already_used_zoom = true;
      }
      this.checkDistanceMoved(position1);
    }
  }

  private static float getMoveDistance(bool pFast = false)
  {
    float num = Time.deltaTime * 55f;
    if (pFast)
      num *= 2.5f;
    return num * MoveCamera.instance._target_zoom * MoveCamera.instance.camera_move_speed;
  }

  public static void move(HotkeyAsset pAsset)
  {
    float moveDistance = MoveCamera.getMoveDistance(pAsset.id.StartsWith("fast_"));
    switch (pAsset.id)
    {
      case "down":
      case "fast_down":
        MoveCamera.instance.addVelocity(0.0f, -moveDistance);
        break;
      case "fast_left":
      case "left":
        MoveCamera.instance.addVelocity(-moveDistance, 0.0f);
        break;
      case "fast_right":
      case "right":
        MoveCamera.instance.addVelocity(moveDistance, 0.0f);
        break;
      case "fast_up":
      case "up":
        MoveCamera.instance.addVelocity(0.0f, moveDistance);
        break;
    }
    MoveCamera.instance.clampVelocity();
    MoveCamera.instance._mouse_controls_used_last = false;
  }

  private void addVelocity(float pX, float pY)
  {
    this._move_velocity.x += pX;
    this._move_velocity.y += pY;
  }

  private void clampVelocity()
  {
    float num1 = -this._target_zoom * this.camera_move_max;
    float num2 = this._target_zoom * this.camera_move_max;
    this._move_velocity.y = Mathf.Clamp(this._move_velocity.y, num1, num2);
    this._move_velocity.x = Mathf.Clamp(this._move_velocity.x, num1, num2);
  }

  public static void zoomIn(HotkeyAsset pAsset)
  {
    MoveCamera.instance._target_zoom -= MoveCamera.instance.main_camera.orthographicSize * 0.05f;
  }

  public static void zoomOut(HotkeyAsset pAsset)
  {
    MoveCamera.instance._target_zoom += MoveCamera.instance.main_camera.orthographicSize * 0.05f;
  }

  public static void zoomInWheel(HotkeyAsset pAsset)
  {
    MoveCamera.instance._target_zoom -= MoveCamera.instance.main_camera.orthographicSize * 0.2f;
  }

  public static void zoomOutWheel(HotkeyAsset pAsset)
  {
    MoveCamera.instance._target_zoom += MoveCamera.instance.main_camera.orthographicSize * 0.2f;
  }

  private void updateTrailerMode()
  {
    if (Input.GetKeyUp((KeyCode) 291))
    {
      this.camera_zoom_speed -= 0.2f;
      if ((double) this.camera_zoom_speed < 0.0)
        this.camera_zoom_speed = 0.2f;
    }
    if (Input.GetKeyUp((KeyCode) 292))
      this.camera_zoom_speed += 0.2f;
    if (Input.GetKeyUp((KeyCode) 111))
    {
      this.camera_move_max -= 0.1f;
      if ((double) this.camera_move_max < 0.0099999997764825821)
        this.camera_move_max = 0.01f;
    }
    if (Input.GetKeyUp((KeyCode) 112 /*0x70*/))
      this.camera_move_max += 0.1f;
    if (Input.GetKeyUp((KeyCode) 107))
    {
      this.camera_move_speed -= 0.01f;
      if ((double) this.camera_move_speed < 0.0099999997764825821)
        this.camera_move_speed = 0.01f;
    }
    if (Input.GetKeyUp((KeyCode) 108))
      this.camera_move_speed += 0.01f;
    if (!Input.GetKeyDown((KeyCode) 114) || (double) this._target_zoom == (double) this.main_camera.orthographicSize)
      return;
    if ((double) this._target_zoom > (double) this.main_camera.orthographicSize)
      this._target_zoom = this.main_camera.orthographicSize + this._target_zoom * 0.1f;
    else
      this._target_zoom = this.main_camera.orthographicSize - this._target_zoom * 0.1f;
  }

  public void debug(DebugTool pTool)
  {
    pTool.setText("bounds_normal:", (object) this._visible_bounds);
    pTool.setText("bounds_wth_power_bar:", (object) this._visible_bounds_without_power_bar);
    pTool.setText("is_no_input_detected:", (object) this.isNoInputDetected());
    pTool.setText("_whooshState:", (object) this._whoosh_state);
    pTool.setText("InputHelpers.touchCount:", (object) InputHelpers.touchCount);
    pTool.setText("world.isGameplayControlsLocked():", (object) World.world.isGameplayControlsLocked());
    pTool.setText("ScrollWindow.animationActive:", (object) ScrollWindow.isAnimationActive());
    pTool.setText("firstTouchOnUI", (object) this._first_touch_on_ui);
    pTool.setText("world.alreadyUsedZoom", (object) World.world.player_control.already_used_zoom);
    pTool.setText("world.alreadyUsedPower", (object) World.world.player_control.already_used_power);
    pTool.setText("world.already_used_camera_drag", (object) World.world.player_control.already_used_camera_drag);
    pTool.setText("_touch_dist", (object) MoveCamera._touch_dist);
    pTool.setText("cameraDragRun", (object) MoveCamera.camera_drag_run);
    pTool.setText("camera_drag_activated", (object) MoveCamera.camera_drag_activated);
    if (UltimateJoystick.getJoyCount() != 2)
      return;
    pTool.setText("JoyRight", (object) UltimateJoystick.GetJoystickState("JoyRight"));
    pTool.setText("JoyLeft", (object) UltimateJoystick.GetJoystickState("JoyLeft"));
  }

  public void skipResetZoom() => this._skip_reset_zoom = true;
}
