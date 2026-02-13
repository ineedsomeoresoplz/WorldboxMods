// Decompiled with JetBrains decompiler
// Type: MouseCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MouseCursor : MonoBehaviour
{
  private const int CURSOR_DEFAULT_0 = 0;
  private const int CURSOR_DOWN_1 = 1;
  private const int CURSOR_UP_2 = 2;
  private const int CURSOR_HOLD_6 = 6;
  private const int CURSOR_DRAG_7 = 7;
  private const int CURSOR_PINKIE_8 = 8;
  private const int CURSOR_SPRINKLE_13 = 13;
  private const int CURSOR_DRAW_17 = 17;
  private const int CURSOR_ATTACK = 22;
  private const int CURSOR_INSPECT = 26;
  private const int CURSOR_OVER_UI = 27;
  private const int CLICK_FRAMES = 6;
  private const int UP_FRAMES = 4;
  private const int PINKIE_FRAMES = 5;
  private const int SPRINKLE_FRAMES = 4;
  private const int DRAW_FRAMES = 5;
  private const int ATTACK_FRAMES = 4;
  private const int ANIM_SPEED = 5;
  public Texture2D mouseCursorDefault;
  public Texture2D mouseCursorDown;
  public Texture2D mouseCursorUp1;
  public Texture2D mouseCursorUp2;
  public Texture2D mouseCursorUp3;
  public Texture2D mouseCursorUp4;
  public Texture2D mouseCursorHold;
  public Texture2D mouseCursorDrag;
  public Texture2D mouseCursorPinkie1;
  public Texture2D mouseCursorPinkie2;
  public Texture2D mouseCursorPinkie3;
  public Texture2D mouseCursorPinkie4;
  public Texture2D mouseCursorPinkie5;
  public Texture2D mouseCursorSprinkle1;
  public Texture2D mouseCursorSprinkle2;
  public Texture2D mouseCursorSprinkle3;
  public Texture2D mouseCursorSprinkle4;
  public Texture2D mouseCursorDraw1;
  public Texture2D mouseCursorDraw2;
  public Texture2D mouseCursorDraw3;
  public Texture2D mouseCursorDraw4;
  public Texture2D mouseCursorDraw5;
  public Texture2D mouseCursorAttack1;
  public Texture2D mouseCursorAttack2;
  public Texture2D mouseCursorAttack3;
  public Texture2D mouseCursorAttack4;
  public Texture2D mouseCursorInspect;
  public Texture2D mouseCursorOverUI;
  private static int _counter = 0;
  private static bool _pressed = false;
  private static int _pressing = 0;
  private static int _dragged = 0;
  private static bool _right = false;
  private static bool _middle = false;
  private static bool _anim_done = true;
  private static int _cur_cursor = -1;
  private static Texture2D[] _cursors;
  private static int _last_texture_id = -1;
  private static bool _can_drag = false;
  private static bool _selected_unit_attack = false;
  private static MouseHoldAnimation _mouse_hold_animation = MouseHoldAnimation.Default;
  private static Texture2D _selected_cursor_texture;

  private void Awake()
  {
    if (!Input.mousePresent)
      Object.Destroy((Object) this);
    else
      MouseCursor._cursors = new Texture2D[28]
      {
        this.mouseCursorDefault,
        this.mouseCursorDown,
        this.mouseCursorUp1,
        this.mouseCursorUp2,
        this.mouseCursorUp3,
        this.mouseCursorUp4,
        this.mouseCursorHold,
        this.mouseCursorDrag,
        this.mouseCursorPinkie1,
        this.mouseCursorPinkie2,
        this.mouseCursorPinkie3,
        this.mouseCursorPinkie4,
        this.mouseCursorPinkie5,
        this.mouseCursorSprinkle1,
        this.mouseCursorSprinkle2,
        this.mouseCursorSprinkle3,
        this.mouseCursorSprinkle4,
        this.mouseCursorDraw1,
        this.mouseCursorDraw2,
        this.mouseCursorDraw3,
        this.mouseCursorDraw4,
        this.mouseCursorDraw5,
        this.mouseCursorAttack1,
        this.mouseCursorAttack2,
        this.mouseCursorAttack3,
        this.mouseCursorAttack4,
        this.mouseCursorInspect,
        this.mouseCursorOverUI
      };
  }

  private void OnEnable() => this.setCursorDefault();

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
    {
      if (!MouseCursor._pressed)
      {
        MouseCursor._pressing = 0;
        MouseCursor._dragged = 0;
        MouseCursor._counter = 0;
      }
      MouseCursor._pressed = true;
      MouseCursor._anim_done = false;
      MouseCursor._right = Input.GetMouseButtonDown(1);
      MouseCursor._middle = Input.GetMouseButtonDown(2);
    }
    else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
    {
      if (!Input.anyKey)
      {
        MouseCursor._pressed = false;
        MouseCursor._pressing = 0;
        MouseCursor._dragged = 0;
        MouseCursor._right = Input.GetMouseButtonUp(1);
        MouseCursor._middle = Input.GetMouseButtonUp(2);
      }
      else
      {
        MouseCursor._right = Input.GetMouseButton(1);
        MouseCursor._middle = Input.GetMouseButton(2);
      }
    }
    else if (MouseCursor._pressed)
    {
      if (!Input.anyKey)
      {
        MouseCursor._pressed = false;
        MouseCursor._pressing = 0;
        MouseCursor._dragged = 0;
        MouseCursor._right = false;
        MouseCursor._middle = false;
      }
      else
      {
        MouseCursor._right = Input.GetMouseButton(1);
        MouseCursor._middle = Input.GetMouseButton(2);
        ++MouseCursor._pressing;
        if (Config.isDraggingItem())
          ++MouseCursor._dragged;
      }
    }
    MouseCursor._can_drag = World.world.isOverUI() || World.world.canDragMap();
    MouseCursor._selected_unit_attack = ControllableUnit.isControllingUnit();
    MouseCursor._mouse_hold_animation = World.world.getSelectedPowerHoldAnimation();
    if (MouseCursor._selected_unit_attack)
      this.setCursor(22);
    else if (MouseCursor._can_drag && Config.isDraggingItem() && MouseCursor._pressed && MouseCursor._dragged > 3)
    {
      if (MouseCursor._dragged < 10)
        this.setCursor(6);
      else
        this.setCursor(7);
    }
    else if (!MouseCursor._pressed && MouseCursor._anim_done)
    {
      if (this.shouldShowInspectCursor())
        this.setCursorInspect();
      else if (MouseCursor._right)
        this.setCursor(8);
      else
        this.setCursorDefault();
      MouseCursor._counter = 0;
    }
    else if (MouseCursor._can_drag && MouseCursor._pressed && MouseCursor._pressing > 3)
    {
      if (!World.world.isOverUI() || World.world.player_control.isPointerOverUIScroll())
      {
        if (MouseCursor._pressing < 10)
          this.setCursor(6);
        else
          this.setCursor(7);
      }
      else if (MouseCursor._right)
        this.setCursor(8);
      else
        this.setCursor(1);
    }
    else if (!MouseCursor._pressed && this.shouldShowInspectCursor())
      this.setCursorInspect();
    else if (MouseCursor._right)
    {
      int num = Mathf.CeilToInt((float) (MouseCursor._counter / 5));
      if (num > 0 && num < 5)
        this.setCursor(num + 8);
      else
        this.setCursor(8);
      ++MouseCursor._counter;
      if (MouseCursor._counter > 40)
      {
        MouseCursor._counter = 0;
        if (!MouseCursor._pressed)
          MouseCursor._anim_done = true;
      }
    }
    else
    {
      int num;
      switch (MouseCursor._mouse_hold_animation)
      {
        case MouseHoldAnimation.Sprinkle:
          num = 4;
          this.setCursor(13 + Mathf.CeilToInt((float) (MouseCursor._counter / 5)) % 4);
          break;
        case MouseHoldAnimation.Draw:
          num = 5;
          this.setCursor(17 + Mathf.CeilToInt((float) (MouseCursor._counter / 5)) % 5);
          break;
        default:
          num = 6;
          int pCursor = Mathf.CeilToInt((float) (MouseCursor._counter / 5));
          if (pCursor < 6)
          {
            this.setCursor(pCursor);
            break;
          }
          this.setCursorDefault();
          break;
      }
      ++MouseCursor._counter;
      if (MouseCursor._counter >= num * 5 && !MouseCursor._pressed)
        MouseCursor._anim_done = true;
    }
    this.renderCursor();
  }

  private void renderCursor()
  {
    int num = -1;
    if (Object.op_Inequality((Object) MouseCursor._selected_cursor_texture, (Object) null))
      num = MouseCursor._selected_cursor_texture.GetHashCode();
    if (MouseCursor._last_texture_id == num)
      return;
    Cursor.SetCursor(MouseCursor._selected_cursor_texture, Vector2.zero, (CursorMode) 0);
    MouseCursor._last_texture_id = num;
  }

  private void setCursor(int pCursor = -1)
  {
    MouseCursor._cur_cursor = pCursor;
    MouseCursor._selected_cursor_texture = MouseCursor._cursors[pCursor];
    if (!Object.op_Equality((Object) MouseCursor._selected_cursor_texture, (Object) null))
      return;
    MouseCursor._selected_cursor_texture = this.mouseCursorDefault;
  }

  private bool shouldShowInspectCursor()
  {
    if (UnitSelectionEffect.last_actor != null)
      return true;
    if (World.world.isOverUI() || ScrollWindow.isWindowActive())
      return false;
    if (World.world.quality_changer.isLowRes())
    {
      WorldTile tilePosCachedFrame = World.world.getMouseTilePosCachedFrame();
      if (tilePosCachedFrame == null)
        return false;
      if (Zones.showMapBorders())
      {
        MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
        if (cachedMapMetaAsset == null)
          return false;
        if (cachedMapMetaAsset.check_tile_has_meta(tilePosCachedFrame.zone, cachedMapMetaAsset, cachedMapMetaAsset.getZoneOptionState()))
          return true;
      }
    }
    return World.world.nameplate_manager.isOverNameplate();
  }

  private void setCursorDefault()
  {
    MapBox world = World.world;
    if ((world != null ? (world.isOverUiButton() ? 1 : 0) : 0) != 0)
      this.setCursor(27);
    else
      this.setCursor(0);
  }

  private void setCursorInspect() => this.setCursor(26);

  public static void debug(DebugTool pTool)
  {
    pTool.setText("_counter:", (object) MouseCursor._counter);
    pTool.setText("_cur_cursor:", (object) MouseCursor._cur_cursor);
    string pT2;
    switch (MouseCursor._cur_cursor)
    {
      case 0:
        pT2 = "mouseCursorDefault";
        break;
      case 1:
        pT2 = "mouseCursorDown";
        break;
      case 2:
        pT2 = "mouseCursorUp1";
        break;
      case 3:
        pT2 = "mouseCursorUp2";
        break;
      case 4:
        pT2 = "mouseCursorUp3";
        break;
      case 5:
        pT2 = "mouseCursorUp4";
        break;
      case 6:
        pT2 = "mouseCursorHold";
        break;
      case 7:
        pT2 = "mouseCursorDrag";
        break;
      case 8:
        pT2 = "mouseCursorPinkie1";
        break;
      case 9:
        pT2 = "mouseCursorPinkie2";
        break;
      case 10:
        pT2 = "mouseCursorPinkie3";
        break;
      case 11:
        pT2 = "mouseCursorPinkie4";
        break;
      case 12:
        pT2 = "mouseCursorPinkie5";
        break;
      case 13:
        pT2 = "mouseCursorSprinkle1";
        break;
      case 14:
        pT2 = "mouseCursorSprinkle2";
        break;
      case 15:
        pT2 = "mouseCursorSprinkle3";
        break;
      case 16 /*0x10*/:
        pT2 = "mouseCursorSprinkle4";
        break;
      case 17:
        pT2 = "mouseCursorDraw1";
        break;
      case 18:
        pT2 = "mouseCursorDraw2";
        break;
      case 19:
        pT2 = "mouseCursorDraw3";
        break;
      case 20:
        pT2 = "mouseCursorDraw4";
        break;
      case 21:
        pT2 = "mouseCursorDraw5";
        break;
      default:
        pT2 = MouseCursor._cur_cursor.ToString();
        break;
    }
    pTool.setText("_cur_cursor:", (object) pT2);
    pTool.setText("_pressed:", (object) MouseCursor._pressed);
    pTool.setText("_pressing:", (object) MouseCursor._pressing);
    pTool.setText("_dragged:", (object) MouseCursor._dragged);
    pTool.setText("_right:", (object) MouseCursor._right);
    pTool.setText("_middle:", (object) MouseCursor._middle);
    pTool.setText("_anim_done:", (object) MouseCursor._anim_done);
    pTool.setSeparator();
    pTool.setText("_can_drag:", (object) MouseCursor._can_drag);
    pTool.setText("_hold_animation:", (object) MouseCursor._mouse_hold_animation);
    pTool.setSeparator();
    pTool.setText("_last_texture_id:", (object) MouseCursor._last_texture_id);
    pTool.setText("_selected_cursor_texture:", (object) MouseCursor._selected_cursor_texture.GetHashCode().ToString());
  }
}
