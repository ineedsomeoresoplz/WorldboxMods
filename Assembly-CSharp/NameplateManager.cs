// Decompiled with JetBrains decompiler
// Type: NameplateManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NameplateManager : MonoBehaviour
{
  private readonly Stack<NameplateText> _pool = new Stack<NameplateText>();
  private readonly List<NameplateText> _active = new List<NameplateText>();
  private int _next_index;
  public NameplateText prefab;
  private Canvas _canvas;
  internal CanvasScaler canvas_scaler;
  internal RectTransform canvas_rect;
  internal Vector2 canvas_size_delta;
  internal float canvas_size_delta_mod_x;
  internal float canvas_size_delta_mod_y;
  private MetaType _last_mode;
  public NameplateText cursor_over_text;
  private int _latest_touch_id;
  private bool _touch_released;
  private float _tween_timer;
  private float _tween_scale;
  internal bool cached_favorites_only;
  internal float cached_canvas_scale;
  private NameplateRenderingType _nameplate_mode;
  private bool _nano_object_set;
  private NanoObject _selected_nano_object;

  private void Awake()
  {
    this._canvas = ((Component) this).GetComponent<Canvas>();
    this.canvas_rect = ((Component) this._canvas).GetComponent<RectTransform>();
    this.canvas_scaler = ((Component) this).GetComponent<CanvasScaler>();
  }

  private void prepare()
  {
    this._next_index = 0;
    this._nameplate_mode = PlayerConfig.getOptionInt("map_names") == 0 ? NameplateRenderingType.Full : NameplateRenderingType.BannerOnly;
    this._nano_object_set = SelectedObjects.isNanoObjectSet();
    this._selected_nano_object = SelectedObjects.getSelectedNanoObject();
    this.cached_favorites_only = PlayerConfig.optionBoolEnabled("only_favorited_meta");
    this.canvas_size_delta = this.canvas_rect.sizeDelta;
    this.cached_canvas_scale = this.canvas_scaler.scaleFactor;
    this.canvas_size_delta_mod_x = this.canvas_size_delta.x * 0.5f;
    this.canvas_size_delta_mod_y = this.canvas_size_delta.y * 0.5f;
  }

  internal void update()
  {
    Bench.bench("nameplates", "nameplates_total");
    Bench.bench("prepare", "nameplates");
    this.prepare();
    Bench.benchEnd("prepare", "nameplates");
    Bench.bench("check_mode", "nameplates");
    MetaType currentMode = this.getCurrentMode();
    this.setMode(currentMode);
    NameplateAsset pAsset = (NameplateAsset) null;
    MetaTypeAsset metaTypeAsset = (MetaTypeAsset) null;
    if (!currentMode.isNone())
    {
      pAsset = AssetManager.nameplates_library.map_modes_nameplates[currentMode];
      metaTypeAsset = currentMode.getAsset();
    }
    Bench.benchEnd("check_mode", "nameplates");
    Bench.bench("set_nameplates", "nameplates");
    if (CanvasMain.isNameplatesAllowed())
    {
      if (currentMode == MetaType.None)
      {
        if (((Component) this).gameObject.activeSelf)
          ((Component) this).gameObject.SetActive(false);
      }
      else
      {
        if (!((Component) this).gameObject.activeSelf)
          ((Component) this).gameObject.SetActive(true);
        pAsset.action_main(this, pAsset);
      }
    }
    Bench.benchEnd("set_nameplates", "nameplates", pCounter: (long) this._active.Count);
    Bench.bench("updateOverlappingPositions", "nameplates");
    bool flag = false;
    if (!currentMode.isNone() && metaTypeAsset != null)
    {
      if (metaTypeAsset.isMetaZoneOptionSelectedFluid())
      {
        if (pAsset.overlap_for_fluid_mode)
          flag = true;
      }
      else
        flag = true;
    }
    if (flag)
      this.updateOverlappingPosition();
    Bench.benchEnd("updateOverlappingPositions", "nameplates");
    Bench.bench("updateTweenScale", "nameplates");
    this.updateTweenScale();
    Bench.benchEnd("updateTweenScale", "nameplates");
    Bench.bench("checkActive", "nameplates");
    this.checkActive();
    Bench.benchEnd("checkActive", "nameplates");
    Bench.bench("findObjectForTooltip", "nameplates");
    NanoObject objectForTooltip = this.findObjectForTooltip();
    Bench.benchEnd("findObjectForTooltip", "nameplates");
    Bench.bench("showTooltip", "nameplates");
    if (objectForTooltip != null)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (objectForTooltip.getMetaTypeAsset()).cursor_tooltip_action(objectForTooltip);
    }
    Bench.benchEnd("showTooltip", "nameplates");
    Bench.bench("check_siblings", "nameplates");
    this.checkSiblingsToFront();
    Bench.benchEnd("check_siblings", "nameplates");
    Bench.bench("finale", "nameplates");
    this.finale();
    Bench.benchEnd("finale", "nameplates");
    Bench.benchEnd("nameplates", "nameplates_total");
  }

  private void checkSiblingsToFront()
  {
    if (Object.op_Inequality((Object) this.cursor_over_text, (Object) null))
      ((Component) this.cursor_over_text).transform.SetAsLastSibling();
    if (!SelectedObjects.isNanoObjectSet())
      return;
    foreach (NameplateText nameplateText in this._active)
    {
      if (nameplateText.nano_object == SelectedObjects.getSelectedNanoObject())
      {
        ((Component) nameplateText).transform.SetAsLastSibling();
        break;
      }
    }
  }

  private void checkActive()
  {
    for (int index = this._next_index - 1; index >= 0; --index)
      this._active[index].checkActive();
  }

  private void updateTweenScale()
  {
    this._tween_timer += Time.deltaTime * 2f;
    this._tween_timer = Mathf.Clamp(this._tween_timer, 0.0f, 1f);
    this._tween_scale = iTween.easeOutBack(0.0f, 1f, this._tween_timer) * 0.5f;
  }

  private NanoObject findObjectForTooltip()
  {
    this.cursor_over_text = (NameplateText) null;
    if (World.world.isBusyWithUI())
      return (NanoObject) null;
    if (ControllableUnit.isControllingUnit())
      return (NanoObject) null;
    if (!InputHelpers.mouseSupported && !this.checkTouch(out Vector2 _))
      return (NanoObject) null;
    Vector2 vector2 = Vector2.op_Implicit(World.world.camera.WorldToScreenPoint(Vector2.op_Implicit(World.world.getMousePos())));
    bool mouseSupported = InputHelpers.mouseSupported;
    NanoObject objectForTooltip = (NanoObject) null;
    float num1 = float.MaxValue;
    NameplateText nameplateText1 = (NameplateText) null;
    for (int index = 0; index < this._active.Count; ++index)
    {
      NameplateText nameplateText2 = this._active[index];
      if (nameplateText2.isShowing())
      {
        Vector2 lastScreenPosition = nameplateText2.getLastScreenPosition();
        float num2 = Toolbox.SquaredDist(lastScreenPosition.x, lastScreenPosition.y, vector2.x, vector2.y);
        if (((Rect) ref nameplateText2.map_text_rect_click).Contains(vector2) && (!Object.op_Inequality((Object) nameplateText1, (Object) null) || (double) num2 <= (double) num1 && (double) num2 <= 625.0))
        {
          nameplateText1 = nameplateText2;
          num1 = num2;
        }
      }
    }
    if (Object.op_Inequality((Object) nameplateText1, (Object) null))
    {
      NanoObject nanoObject = nameplateText1.nano_object;
      if (Input.mousePresent)
        objectForTooltip = nanoObject;
      this.cursor_over_text = nameplateText1;
      this.cursor_over_text.forceScale(Vector3.op_Multiply(((Component) nameplateText1).transform.localScale, 1.1f));
      if (nanoObject is IMetaObject & mouseSupported)
        ((IMetaObject) nanoObject).setCursorOver();
    }
    return objectForTooltip;
  }

  public bool isOverNameplate()
  {
    return Object.op_Inequality((Object) this.cursor_over_text, (Object) null);
  }

  private bool checkTouch(out Vector2 pPosition)
  {
    pPosition = Vector2.op_Implicit(Globals.POINT_IN_VOID);
    if (Input.touchCount == 0)
      return false;
    Touch touch = Input.touches[0];
    if (((Touch) ref touch).phase == null && this._touch_released)
    {
      this._latest_touch_id = ((Touch) ref touch).fingerId;
      this._touch_released = false;
      return false;
    }
    if (((Touch) ref touch).fingerId != this._latest_touch_id || ((Touch) ref touch).phase != 3 || this._touch_released)
      return false;
    this._touch_released = true;
    pPosition = Vector2.op_Implicit(World.world.camera.ScreenToWorldPoint(Vector2.op_Implicit(((Touch) ref touch).position)));
    return true;
  }

  private MetaType getCurrentMode()
  {
    MetaType pType = MetaType.None;
    if (Zones.showMapNames())
    {
      if (!Zones.hasPowerForceMapMode())
      {
        pType = Zones.getCurrentMapBorderMode();
        if (pType.isNone())
          pType = MetaType.City;
      }
      else
        pType = Zones.getForcedMapMode();
    }
    return pType;
  }

  private void setMode(MetaType pMode)
  {
    if (this._last_mode == pMode)
      return;
    this._last_mode = pMode;
    this.clearAll();
  }

  private void updateOverlappingPosition()
  {
    // ISSUE: unable to decompile the method.
  }

  private void OnDrawGizmos()
  {
    Camera main = Camera.main;
    if (Object.op_Equality((Object) main, (Object) null))
      return;
    for (int index = 0; index < this._next_index; ++index)
    {
      NameplateText nameplateText = this._active[index];
      Rect mapTextRectOverlap = nameplateText.map_text_rect_overlap;
      Vector3 vector3_1;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_1).\u002Ector(((Rect) ref mapTextRectOverlap).xMin, ((Rect) ref mapTextRectOverlap).yMin, main.nearClipPlane);
      Vector3 vector3_2;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_2).\u002Ector(((Rect) ref mapTextRectOverlap).xMax, ((Rect) ref mapTextRectOverlap).yMax, main.nearClipPlane);
      Vector3 worldPoint1 = main.ScreenToWorldPoint(vector3_1);
      Vector3 worldPoint2 = main.ScreenToWorldPoint(vector3_2);
      Vector3 vector3_3 = Vector3.op_Multiply(Vector3.op_Addition(worldPoint1, worldPoint2), 0.5f);
      Vector3 vector3_4;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3_4).\u002Ector(worldPoint2.x - worldPoint1.x, worldPoint2.y - worldPoint1.y, 0.1f);
      Gizmos.color = nameplateText.isShowing() ? Color.green : Color.red;
      Vector3 vector3_5 = vector3_4;
      Gizmos.DrawWireCube(vector3_3, vector3_5);
    }
  }

  private int compareNameplates(NameplateText pText1, NameplateText pText2)
  {
    NanoObject selectedNanoObject = SelectedObjects.getSelectedNanoObject();
    bool flag1 = pText1.nano_object == selectedNanoObject;
    bool flag2 = pText2.nano_object == selectedNanoObject;
    if (flag1 != flag2)
      return (flag2 ? 1 : 0) - (flag1 ? 1 : 0);
    if (pText1.favorited != pText2.favorited)
      return (pText2.favorited ? 1 : 0) - (pText1.favorited ? 1 : 0);
    if (pText1.priority_capital != pText2.priority_capital)
      return (pText2.priority_capital ? 1 : 0) - (pText1.priority_capital ? 1 : 0);
    int num = pText2.priority_population.CompareTo(pText1.priority_population);
    return num != 0 ? num : pText1.nano_object.id.CompareTo(pText2.nano_object.id);
  }

  public NameplateText prepareNext(NameplateAsset pAsset, NanoObject pMeta)
  {
    NameplateText nameplateToRender = this.getNameplateToRender();
    nameplateToRender.prepare(pAsset, pMeta, this._tween_scale, this._nameplate_mode, this._nano_object_set, this._selected_nano_object);
    return nameplateToRender;
  }

  private NameplateText getNameplateToRender()
  {
    NameplateText nameplateToRender;
    if (this._active.Count > this._next_index)
    {
      nameplateToRender = this._active[this._next_index];
    }
    else
    {
      nameplateToRender = this._pool.Count != 0 ? this._pool.Pop() : this.createNew();
      this._active.Add(nameplateToRender);
    }
    ++this._next_index;
    return nameplateToRender;
  }

  protected virtual NameplateText createNew()
  {
    NameplateText nameplateText = Object.Instantiate<NameplateText>(this.prefab, ((Component) this).transform);
    nameplateText.newNameplate(this, $"map text {this._pool.Count + this._active.Count}");
    return nameplateText;
  }

  internal void clearAll()
  {
    this._tween_timer = 0.5f;
    this._tween_scale = 0.0f;
    if (this._active.Count == 0)
      return;
    for (int index = 0; index < this._active.Count; ++index)
    {
      NameplateText nameplateText = this._active[index];
      nameplateText.clearFull();
      ((Component) nameplateText).gameObject.SetActive(false);
      this._pool.Push(nameplateText);
    }
    this._active.Clear();
  }

  private void finale() => this.clearLast();

  public void clearCaches()
  {
    foreach (NameplateText nameplateText in this._active)
      nameplateText.clearCaches();
  }

  public void clearLast()
  {
    int num = this._active.Count - this._next_index;
    if (num <= 0)
      return;
    for (; num > 0; --num)
    {
      int index = this._active.Count - 1;
      NameplateText nameplateText = this._active[index];
      nameplateText.clearFull();
      ((Component) nameplateText).gameObject.SetActive(false);
      this._active.RemoveAt(index);
      this._pool.Push(nameplateText);
    }
  }
}
