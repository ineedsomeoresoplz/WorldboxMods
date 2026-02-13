// Decompiled with JetBrains decompiler
// Type: ScrollRectExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[Skip]
[SelectionBase]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof (RectTransform))]
public class ScrollRectExtended : 
  UIBehaviour,
  IInitializePotentialDragHandler,
  IEventSystemHandler,
  IBeginDragHandler,
  IEndDragHandler,
  IDragHandler,
  IScrollHandler,
  ICanvasElement,
  ILayoutElement,
  ILayoutGroup,
  ILayoutController
{
  [SerializeField]
  private RectTransform m_Content;
  [SerializeField]
  private bool m_Horizontal = true;
  [SerializeField]
  private bool m_Vertical = true;
  [SerializeField]
  private ScrollRectExtended.MovementType m_MovementType = ScrollRectExtended.MovementType.Elastic;
  [SerializeField]
  private float m_Elasticity = 0.1f;
  [SerializeField]
  private bool m_Inertia = true;
  [SerializeField]
  private float m_DecelerationRate = 0.135f;
  [SerializeField]
  private float m_ScrollSensitivity = 1f;
  [SerializeField]
  private RectTransform m_Viewport;
  [SerializeField]
  private float m_ScrollFactor = 1f;
  public float velocityMod = 1f;
  private Scrollbar m_HorizontalScrollbar;
  [SerializeField]
  private Scrollbar m_VerticalScrollbar;
  [SerializeField]
  private ScrollRectExtended.ScrollbarVisibility m_HorizontalScrollbarVisibility;
  [SerializeField]
  private ScrollRectExtended.ScrollbarVisibility m_VerticalScrollbarVisibility;
  [SerializeField]
  private float m_HorizontalScrollbarSpacing;
  [SerializeField]
  private float m_VerticalScrollbarSpacing;
  [SerializeField]
  private ScrollRectExtended.ScrollRectEvent m_OnValueChanged = new ScrollRectExtended.ScrollRectEvent();
  private Vector2 m_PointerStartLocalCursor = Vector2.zero;
  private Vector2 m_ContentStartPosition = Vector2.zero;
  private RectTransform m_ViewRect;
  private Bounds m_ContentBounds;
  private Bounds m_ViewBounds;
  private Vector2 m_Velocity;
  private bool m_Dragging;
  private Vector2 m_PrevPosition = Vector2.zero;
  private Bounds m_PrevContentBounds;
  private Bounds m_PrevViewBounds;
  [NonSerialized]
  private bool m_HasRebuiltLayout;
  private bool m_HSliderExpand;
  private bool m_VSliderExpand;
  private float m_HSliderHeight;
  private float m_VSliderWidth;
  [NonSerialized]
  private RectTransform m_Rect;
  private RectTransform m_HorizontalScrollbarRect;
  private RectTransform m_VerticalScrollbarRect;
  private DrivenRectTransformTracker m_Tracker;
  public static List<ScrollRectExtended> instances = new List<ScrollRectExtended>();
  private float _check_timer = 0.05f;
  private float _check_timer_interval = 0.05f;
  private readonly Vector3[] m_Corners = new Vector3[4];

  public RectTransform content
  {
    get => this.m_Content;
    set => this.m_Content = value;
  }

  public bool horizontal
  {
    get => this.m_Horizontal;
    set => this.m_Horizontal = value;
  }

  public bool vertical
  {
    get => this.m_Vertical;
    set => this.m_Vertical = value;
  }

  public ScrollRectExtended.MovementType movementType
  {
    get => this.m_MovementType;
    set => this.m_MovementType = value;
  }

  public float elasticity
  {
    get => this.m_Elasticity;
    set => this.m_Elasticity = value;
  }

  public bool inertia
  {
    get => this.m_Inertia;
    set => this.m_Inertia = value;
  }

  public float decelerationRate
  {
    get => this.m_DecelerationRate;
    set => this.m_DecelerationRate = value;
  }

  public float scrollSensitivity
  {
    get => this.m_ScrollSensitivity;
    set => this.m_ScrollSensitivity = value;
  }

  public RectTransform viewport
  {
    get => this.m_Viewport;
    set
    {
      this.m_Viewport = value;
      this.SetDirtyCaching();
    }
  }

  public float scrollFactor
  {
    get => this.m_ScrollFactor;
    set => this.m_ScrollFactor = value;
  }

  public Scrollbar horizontalScrollbar
  {
    get => this.m_HorizontalScrollbar;
    set
    {
      if (Object.op_Implicit((Object) this.m_HorizontalScrollbar))
        ((UnityEvent<float>) this.m_HorizontalScrollbar.onValueChanged).RemoveListener(new UnityAction<float>((object) this, __methodptr(SetHorizontalNormalizedPosition)));
      this.m_HorizontalScrollbar = value;
      if (Object.op_Implicit((Object) this.m_HorizontalScrollbar))
        ((UnityEvent<float>) this.m_HorizontalScrollbar.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(SetHorizontalNormalizedPosition)));
      this.SetDirtyCaching();
    }
  }

  public Scrollbar verticalScrollbar
  {
    get => this.m_VerticalScrollbar;
    set
    {
      if (Object.op_Implicit((Object) this.m_VerticalScrollbar))
        ((UnityEvent<float>) this.m_VerticalScrollbar.onValueChanged).RemoveListener(new UnityAction<float>((object) this, __methodptr(SetVerticalNormalizedPosition)));
      this.m_VerticalScrollbar = value;
      if (Object.op_Implicit((Object) this.m_VerticalScrollbar))
        ((UnityEvent<float>) this.m_VerticalScrollbar.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(SetVerticalNormalizedPosition)));
      this.SetDirtyCaching();
    }
  }

  public ScrollRectExtended.ScrollbarVisibility horizontalScrollbarVisibility
  {
    get => this.m_HorizontalScrollbarVisibility;
    set
    {
      this.m_HorizontalScrollbarVisibility = value;
      this.SetDirtyCaching();
    }
  }

  public ScrollRectExtended.ScrollbarVisibility verticalScrollbarVisibility
  {
    get => this.m_VerticalScrollbarVisibility;
    set
    {
      this.m_VerticalScrollbarVisibility = value;
      this.SetDirtyCaching();
    }
  }

  public float horizontalScrollbarSpacing
  {
    get => this.m_HorizontalScrollbarSpacing;
    set
    {
      this.m_HorizontalScrollbarSpacing = value;
      this.SetDirty();
    }
  }

  public float verticalScrollbarSpacing
  {
    get => this.m_VerticalScrollbarSpacing;
    set
    {
      this.m_VerticalScrollbarSpacing = value;
      this.SetDirty();
    }
  }

  public ScrollRectExtended.ScrollRectEvent onValueChanged
  {
    get => this.m_OnValueChanged;
    set => this.m_OnValueChanged = value;
  }

  protected RectTransform viewRect
  {
    get
    {
      if (Object.op_Equality((Object) this.m_ViewRect, (Object) null))
        this.m_ViewRect = this.m_Viewport;
      if (Object.op_Equality((Object) this.m_ViewRect, (Object) null))
        this.m_ViewRect = (RectTransform) ((Component) this).transform;
      return this.m_ViewRect;
    }
  }

  public Vector2 velocity
  {
    get => this.m_Velocity;
    set => this.m_Velocity = value;
  }

  internal RectTransform rectTransform
  {
    get
    {
      if (Object.op_Equality((Object) this.m_Rect, (Object) null))
        this.m_Rect = ((Component) this).GetComponent<RectTransform>();
      return this.m_Rect;
    }
  }

  protected ScrollRectExtended() => this.flexibleWidth = -1f;

  public virtual void Rebuild(CanvasUpdate executing)
  {
    if (executing == null)
      this.UpdateCachedData();
    if (executing != 2)
      return;
    this.UpdateBounds();
    this.UpdateScrollbars(Vector2.zero);
    this.UpdatePrevData();
    this.m_HasRebuiltLayout = true;
  }

  public virtual void LayoutComplete()
  {
  }

  public virtual void GraphicUpdateComplete()
  {
  }

  private void UpdateCachedData()
  {
    Transform transform = ((Component) this).transform;
    this.m_HorizontalScrollbarRect = Object.op_Equality((Object) this.m_HorizontalScrollbar, (Object) null) ? (RectTransform) null : ((Component) this.m_HorizontalScrollbar).transform as RectTransform;
    this.m_VerticalScrollbarRect = Object.op_Equality((Object) this.m_VerticalScrollbar, (Object) null) ? (RectTransform) null : ((Component) this.m_VerticalScrollbar).transform as RectTransform;
    int num1 = Object.op_Equality((Object) ((Transform) this.viewRect).parent, (Object) transform) ? 1 : 0;
    bool flag1 = !Object.op_Implicit((Object) this.m_HorizontalScrollbarRect) || Object.op_Equality((Object) ((Transform) this.m_HorizontalScrollbarRect).parent, (Object) transform);
    bool flag2 = !Object.op_Implicit((Object) this.m_VerticalScrollbarRect) || Object.op_Equality((Object) ((Transform) this.m_VerticalScrollbarRect).parent, (Object) transform);
    int num2 = flag1 ? 1 : 0;
    bool flag3 = (num1 & num2 & (flag2 ? 1 : 0)) != 0;
    this.m_HSliderExpand = flag3 && Object.op_Implicit((Object) this.m_HorizontalScrollbarRect) && this.horizontalScrollbarVisibility == ScrollRectExtended.ScrollbarVisibility.AutoHideAndExpandViewport;
    this.m_VSliderExpand = flag3 && Object.op_Implicit((Object) this.m_VerticalScrollbarRect) && this.verticalScrollbarVisibility == ScrollRectExtended.ScrollbarVisibility.AutoHideAndExpandViewport;
    Rect rect;
    double num3;
    if (!Object.op_Equality((Object) this.m_HorizontalScrollbarRect, (Object) null))
    {
      rect = this.m_HorizontalScrollbarRect.rect;
      num3 = (double) ((Rect) ref rect).height;
    }
    else
      num3 = 0.0;
    this.m_HSliderHeight = (float) num3;
    double num4;
    if (!Object.op_Equality((Object) this.m_VerticalScrollbarRect, (Object) null))
    {
      rect = this.m_VerticalScrollbarRect.rect;
      num4 = (double) ((Rect) ref rect).width;
    }
    else
      num4 = 0.0;
    this.m_VSliderWidth = (float) num4;
  }

  protected virtual void OnEnable()
  {
    base.OnEnable();
    ScrollRectExtended.instances.Add(this);
    if (Object.op_Implicit((Object) this.m_HorizontalScrollbar))
    {
      // ISSUE: method pointer
      ((UnityEvent<float>) this.m_HorizontalScrollbar.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(SetHorizontalNormalizedPosition)));
    }
    if (Object.op_Implicit((Object) this.m_VerticalScrollbar))
    {
      // ISSUE: method pointer
      ((UnityEvent<float>) this.m_VerticalScrollbar.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(SetVerticalNormalizedPosition)));
    }
    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild((ICanvasElement) this);
  }

  protected virtual void OnDisable()
  {
    CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild((ICanvasElement) this);
    ScrollRectExtended.instances.Remove(this);
    if (Object.op_Implicit((Object) this.m_HorizontalScrollbar))
    {
      // ISSUE: method pointer
      ((UnityEvent<float>) this.m_HorizontalScrollbar.onValueChanged).RemoveListener(new UnityAction<float>((object) this, __methodptr(SetHorizontalNormalizedPosition)));
    }
    if (Object.op_Implicit((Object) this.m_VerticalScrollbar))
    {
      // ISSUE: method pointer
      ((UnityEvent<float>) this.m_VerticalScrollbar.onValueChanged).RemoveListener(new UnityAction<float>((object) this, __methodptr(SetVerticalNormalizedPosition)));
    }
    this.m_HasRebuiltLayout = false;
    ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
    this.m_Velocity = Vector2.zero;
    LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
    base.OnDisable();
  }

  public virtual bool IsActive()
  {
    return base.IsActive() && Object.op_Inequality((Object) this.m_Content, (Object) null);
  }

  private void EnsureLayoutHasRebuilt()
  {
    if (this.m_HasRebuiltLayout || CanvasUpdateRegistry.IsRebuildingLayout())
      return;
    Canvas.ForceUpdateCanvases();
  }

  public virtual void StopMovement() => this.m_Velocity = Vector2.zero;

  public virtual void OnScroll(PointerEventData data)
  {
    if (!base.IsActive())
      return;
    CanvasMain.addTooltipShowTimeout(0.06f);
    if (AssetManager.hotkey_library != null && AssetManager.hotkey_library.isHoldingAnyModKey())
      return;
    this.EnsureLayoutHasRebuilt();
    this.UpdateBounds();
    Vector2 scrollDelta = data.scrollDelta;
    scrollDelta.y *= -1f;
    if (this.vertical && !this.horizontal)
    {
      if ((double) Mathf.Abs(scrollDelta.x) > (double) Mathf.Abs(scrollDelta.y))
        scrollDelta.y = scrollDelta.x;
      scrollDelta.x = 0.0f;
    }
    if (this.horizontal && !this.vertical)
    {
      if ((double) Mathf.Abs(scrollDelta.y) > (double) Mathf.Abs(scrollDelta.x))
        scrollDelta.x = scrollDelta.y;
      scrollDelta.y = 0.0f;
    }
    Vector2 position = Vector2.op_Addition(this.m_Content.anchoredPosition, Vector2.op_Multiply(scrollDelta, this.m_ScrollSensitivity));
    if (this.m_MovementType == ScrollRectExtended.MovementType.Clamped)
      position = Vector2.op_Addition(position, this.CalculateOffset(Vector2.op_Subtraction(position, this.m_Content.anchoredPosition)));
    this.SetContentAnchoredPosition(position);
    this.UpdateBounds();
  }

  public virtual void OnInitializePotentialDrag(PointerEventData eventData)
  {
    if (eventData.button != null)
      return;
    this.m_Velocity = Vector2.zero;
  }

  public virtual void OnBeginDrag(PointerEventData eventData)
  {
    if (eventData.button != null || !base.IsActive())
      return;
    this.UpdateBounds();
    this.m_PointerStartLocalCursor = Vector2.zero;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, ref this.m_PointerStartLocalCursor);
    this.m_ContentStartPosition = this.m_Content.anchoredPosition;
    this.m_Dragging = true;
    this._check_timer = this._check_timer_interval;
    ((AbstractEventData) eventData).Use();
  }

  public virtual void OnEndDrag(PointerEventData eventData)
  {
    if (eventData.button != null)
      return;
    this.m_Dragging = false;
    ((AbstractEventData) eventData).Use();
  }

  public virtual void OnDrag(PointerEventData eventData)
  {
    Vector2 vector2_1;
    if (eventData.button != null || !base.IsActive() || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, ref vector2_1))
      return;
    this.UpdateBounds();
    Vector2 vector2_2 = Vector2.op_Addition(this.m_ContentStartPosition, Vector2.op_Multiply(Vector2.op_Subtraction(vector2_1, this.m_PointerStartLocalCursor), this.m_ScrollFactor));
    Vector2 offset = this.CalculateOffset(Vector2.op_Subtraction(vector2_2, this.m_Content.anchoredPosition));
    Vector2 position = Vector2.op_Addition(vector2_2, offset);
    if (this.m_MovementType == ScrollRectExtended.MovementType.Elastic)
    {
      if ((double) offset.x != 0.0)
        position.x -= ScrollRectExtended.RubberDelta(offset.x, ((Bounds) ref this.m_ViewBounds).size.x);
      if ((double) offset.y != 0.0)
        position.y -= ScrollRectExtended.RubberDelta(offset.y, ((Bounds) ref this.m_ViewBounds).size.y);
    }
    this.SetContentAnchoredPosition(position);
    ((AbstractEventData) eventData).Use();
  }

  protected virtual void SetContentAnchoredPosition(Vector2 position)
  {
    if (!this.m_Horizontal)
      position.x = this.m_Content.anchoredPosition.x;
    if (!this.m_Vertical)
      position.y = this.m_Content.anchoredPosition.y;
    if (!Vector2.op_Inequality(position, this.m_Content.anchoredPosition))
      return;
    this.m_Content.anchoredPosition = position;
    this.UpdateBounds();
  }

  private void fixDragHack()
  {
    if (!InputHelpers.mouseSupported || !this.m_Dragging || Input.GetMouseButton(0))
      return;
    if ((double) this._check_timer > 0.0)
      this._check_timer -= Time.fixedDeltaTime;
    else
      this.m_Dragging = false;
  }

  protected virtual void LateUpdate()
  {
    if (!Object.op_Implicit((Object) this.m_Content))
      return;
    this.fixDragHack();
    this.EnsureLayoutHasRebuilt();
    this.UpdateScrollbarVisibility();
    this.UpdateBounds();
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    Vector2 offset = this.CalculateOffset(Vector2.zero);
    if (!this.m_Dragging && (Vector2.op_Inequality(offset, Vector2.zero) || Vector2.op_Inequality(this.m_Velocity, Vector2.zero)))
    {
      Vector2 position = this.m_Content.anchoredPosition;
      for (int index = 0; index < 2; ++index)
      {
        if (this.m_MovementType == ScrollRectExtended.MovementType.Elastic && (double) ((Vector2) ref offset)[index] != 0.0)
        {
          float num1 = ((Vector2) ref this.m_Velocity)[index];
          ref Vector2 local1 = ref position;
          int num2 = index;
          Vector2 anchoredPosition = this.m_Content.anchoredPosition;
          double num3 = (double) ((Vector2) ref anchoredPosition)[index];
          anchoredPosition = this.m_Content.anchoredPosition;
          double num4 = (double) ((Vector2) ref anchoredPosition)[index] + (double) ((Vector2) ref offset)[index];
          ref float local2 = ref num1;
          double elasticity = (double) this.m_Elasticity;
          double num5 = (double) unscaledDeltaTime;
          double num6 = (double) Mathf.SmoothDamp((float) num3, (float) num4, ref local2, (float) elasticity, float.PositiveInfinity, (float) num5);
          ((Vector2) ref local1)[num2] = (float) num6;
          ((Vector2) ref this.m_Velocity)[index] = num1;
        }
        else if (this.m_Inertia)
        {
          ref Vector2 local3 = ref this.m_Velocity;
          int num7 = index;
          ((Vector2) ref local3)[num7] = ((Vector2) ref local3)[num7] * Mathf.Pow(this.m_DecelerationRate, unscaledDeltaTime);
          if ((double) Mathf.Abs(((Vector2) ref this.m_Velocity)[index]) < 1.0)
            ((Vector2) ref this.m_Velocity)[index] = 0.0f;
          ref Vector2 local4 = ref position;
          int num8 = index;
          ((Vector2) ref local4)[num8] = ((Vector2) ref local4)[num8] + ((Vector2) ref this.m_Velocity)[index] * unscaledDeltaTime;
        }
        else
          ((Vector2) ref this.m_Velocity)[index] = 0.0f;
      }
      if (Vector2.op_Inequality(this.m_Velocity, Vector2.zero))
      {
        if (this.m_MovementType == ScrollRectExtended.MovementType.Clamped)
        {
          offset = this.CalculateOffset(Vector2.op_Subtraction(position, this.m_Content.anchoredPosition));
          position = Vector2.op_Addition(position, offset);
        }
        this.SetContentAnchoredPosition(position);
      }
    }
    if (this.m_Dragging && this.m_Inertia)
    {
      Vector3 vector3 = Vector3.op_Multiply(Vector2.op_Implicit(Vector2.op_Division(Vector2.op_Subtraction(this.m_Content.anchoredPosition, this.m_PrevPosition), unscaledDeltaTime)), this.velocityMod);
      this.m_Velocity = Vector2.op_Implicit(Vector3.Lerp(Vector2.op_Implicit(this.m_Velocity), vector3, unscaledDeltaTime * 10f));
    }
    if (!Bounds.op_Inequality(this.m_ViewBounds, this.m_PrevViewBounds) && !Bounds.op_Inequality(this.m_ContentBounds, this.m_PrevContentBounds) && !Vector2.op_Inequality(this.m_Content.anchoredPosition, this.m_PrevPosition))
      return;
    this.UpdateScrollbars(offset);
    this.m_OnValueChanged.Invoke(this.normalizedPosition);
    this.UpdatePrevData();
  }

  private void UpdatePrevData()
  {
    this.m_PrevPosition = !Object.op_Equality((Object) this.m_Content, (Object) null) ? this.m_Content.anchoredPosition : Vector2.zero;
    this.m_PrevViewBounds = this.m_ViewBounds;
    this.m_PrevContentBounds = this.m_ContentBounds;
  }

  private void UpdateScrollbars(Vector2 offset)
  {
    if (Object.op_Implicit((Object) this.m_HorizontalScrollbar))
    {
      this.m_HorizontalScrollbar.size = (double) ((Bounds) ref this.m_ContentBounds).size.x <= 0.0 ? 1f : Mathf.Clamp01((((Bounds) ref this.m_ViewBounds).size.x - Mathf.Abs(offset.x)) / ((Bounds) ref this.m_ContentBounds).size.x);
      this.m_HorizontalScrollbar.value = this.horizontalNormalizedPosition;
    }
    if (!Object.op_Implicit((Object) this.m_VerticalScrollbar))
      return;
    this.m_VerticalScrollbar.size = (double) ((Bounds) ref this.m_ContentBounds).size.y <= 0.0 ? 1f : Mathf.Clamp01((((Bounds) ref this.m_ViewBounds).size.y - Mathf.Abs(offset.y)) / ((Bounds) ref this.m_ContentBounds).size.y);
    this.m_VerticalScrollbar.value = this.verticalNormalizedPosition;
  }

  public Vector2 normalizedPosition
  {
    get => new Vector2(this.horizontalNormalizedPosition, this.verticalNormalizedPosition);
    set
    {
      this.SetNormalizedPosition(value.x, 0);
      this.SetNormalizedPosition(value.y, 1);
    }
  }

  public float horizontalNormalizedPosition
  {
    get
    {
      this.UpdateBounds();
      return (double) ((Bounds) ref this.m_ContentBounds).size.x <= (double) ((Bounds) ref this.m_ViewBounds).size.x ? ((double) ((Bounds) ref this.m_ViewBounds).min.x > (double) ((Bounds) ref this.m_ContentBounds).min.x ? 1f : 0.0f) : (float) (((double) ((Bounds) ref this.m_ViewBounds).min.x - (double) ((Bounds) ref this.m_ContentBounds).min.x) / ((double) ((Bounds) ref this.m_ContentBounds).size.x - (double) ((Bounds) ref this.m_ViewBounds).size.x));
    }
    set => this.SetNormalizedPosition(value, 0);
  }

  public float verticalNormalizedPosition
  {
    get
    {
      this.UpdateBounds();
      return (double) ((Bounds) ref this.m_ContentBounds).size.y <= (double) ((Bounds) ref this.m_ViewBounds).size.y ? ((double) ((Bounds) ref this.m_ViewBounds).min.y > (double) ((Bounds) ref this.m_ContentBounds).min.y ? 1f : 0.0f) : (float) (((double) ((Bounds) ref this.m_ViewBounds).min.y - (double) ((Bounds) ref this.m_ContentBounds).min.y) / ((double) ((Bounds) ref this.m_ContentBounds).size.y - (double) ((Bounds) ref this.m_ViewBounds).size.y));
    }
    set => this.SetNormalizedPosition(value, 1);
  }

  private void SetHorizontalNormalizedPosition(float value) => this.SetNormalizedPosition(value, 0);

  private void SetVerticalNormalizedPosition(float value) => this.SetNormalizedPosition(value, 1);

  private void SetNormalizedPosition(float value, int axis)
  {
    this.EnsureLayoutHasRebuilt();
    this.UpdateBounds();
    Vector3 size = ((Bounds) ref this.m_ContentBounds).size;
    double num1 = (double) ((Vector3) ref size)[axis];
    size = ((Bounds) ref this.m_ViewBounds).size;
    double num2 = (double) ((Vector3) ref size)[axis];
    float num3 = (float) (num1 - num2);
    Vector3 min = ((Bounds) ref this.m_ViewBounds).min;
    float num4 = ((Vector3) ref min)[axis] - value * num3;
    Vector3 vector3 = ((Transform) this.m_Content).localPosition;
    double num5 = (double) ((Vector3) ref vector3)[axis] + (double) num4;
    vector3 = ((Bounds) ref this.m_ContentBounds).min;
    double num6 = (double) ((Vector3) ref vector3)[axis];
    float num7 = (float) (num5 - num6);
    Vector3 localPosition = ((Transform) this.m_Content).localPosition;
    if ((double) Mathf.Abs(((Vector3) ref localPosition)[axis] - num7) <= 0.0099999997764825821)
      return;
    ((Vector3) ref localPosition)[axis] = num7;
    ((Transform) this.m_Content).localPosition = localPosition;
    ((Vector2) ref this.m_Velocity)[axis] = 0.0f;
    this.UpdateBounds();
  }

  private static float RubberDelta(float overStretching, float viewSize)
  {
    return (float) (1.0 - 1.0 / ((double) Mathf.Abs(overStretching) * 0.550000011920929 / (double) viewSize + 1.0)) * viewSize * Mathf.Sign(overStretching);
  }

  protected virtual void OnRectTransformDimensionsChange() => this.SetDirty();

  private bool hScrollingNeeded
  {
    get
    {
      return !Application.isPlaying || (double) ((Bounds) ref this.m_ContentBounds).size.x > (double) ((Bounds) ref this.m_ViewBounds).size.x + 0.0099999997764825821;
    }
  }

  private bool vScrollingNeeded
  {
    get
    {
      return !Application.isPlaying || (double) ((Bounds) ref this.m_ContentBounds).size.y > (double) ((Bounds) ref this.m_ViewBounds).size.y + 0.0099999997764825821;
    }
  }

  public virtual void CalculateLayoutInputHorizontal()
  {
  }

  public virtual void CalculateLayoutInputVertical()
  {
  }

  public virtual float minWidth => -1f;

  public virtual float preferredWidth => -1f;

  public virtual float flexibleWidth { get; private set; }

  public virtual float minHeight => -1f;

  public virtual float preferredHeight => -1f;

  public virtual float flexibleHeight => -1f;

  public virtual int layoutPriority => -1;

  public virtual void SetLayoutHorizontal()
  {
    ((DrivenRectTransformTracker) ref this.m_Tracker).Clear();
    Rect rect;
    if (this.m_HSliderExpand || this.m_VSliderExpand)
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.viewRect, (DrivenTransformProperties) 16134);
      this.viewRect.anchorMin = Vector2.zero;
      this.viewRect.anchorMax = Vector2.one;
      this.viewRect.sizeDelta = Vector2.zero;
      this.viewRect.anchoredPosition = Vector2.zero;
      LayoutRebuilder.ForceRebuildLayoutImmediate(this.content);
      rect = this.viewRect.rect;
      Vector3 vector3_1 = Vector2.op_Implicit(((Rect) ref rect).center);
      rect = this.viewRect.rect;
      Vector3 vector3_2 = Vector2.op_Implicit(((Rect) ref rect).size);
      this.m_ViewBounds = new Bounds(vector3_1, vector3_2);
      this.m_ContentBounds = this.GetBounds();
    }
    if (this.m_VSliderExpand && this.vScrollingNeeded)
    {
      this.viewRect.sizeDelta = new Vector2((float) -((double) this.m_VSliderWidth + (double) this.m_VerticalScrollbarSpacing), this.viewRect.sizeDelta.y);
      LayoutRebuilder.ForceRebuildLayoutImmediate(this.content);
      rect = this.viewRect.rect;
      Vector3 vector3_3 = Vector2.op_Implicit(((Rect) ref rect).center);
      rect = this.viewRect.rect;
      Vector3 vector3_4 = Vector2.op_Implicit(((Rect) ref rect).size);
      this.m_ViewBounds = new Bounds(vector3_3, vector3_4);
      this.m_ContentBounds = this.GetBounds();
    }
    if (this.m_HSliderExpand && this.hScrollingNeeded)
    {
      this.viewRect.sizeDelta = new Vector2(this.viewRect.sizeDelta.x, (float) -((double) this.m_HSliderHeight + (double) this.m_HorizontalScrollbarSpacing));
      rect = this.viewRect.rect;
      Vector3 vector3_5 = Vector2.op_Implicit(((Rect) ref rect).center);
      rect = this.viewRect.rect;
      Vector3 vector3_6 = Vector2.op_Implicit(((Rect) ref rect).size);
      this.m_ViewBounds = new Bounds(vector3_5, vector3_6);
      this.m_ContentBounds = this.GetBounds();
    }
    if (!this.m_VSliderExpand || !this.vScrollingNeeded || (double) this.viewRect.sizeDelta.x != 0.0 || (double) this.viewRect.sizeDelta.y >= 0.0)
      return;
    this.viewRect.sizeDelta = new Vector2((float) -((double) this.m_VSliderWidth + (double) this.m_VerticalScrollbarSpacing), this.viewRect.sizeDelta.y);
  }

  public virtual void SetLayoutVertical()
  {
    this.UpdateScrollbarLayout();
    Rect rect = this.viewRect.rect;
    Vector3 vector3_1 = Vector2.op_Implicit(((Rect) ref rect).center);
    rect = this.viewRect.rect;
    Vector3 vector3_2 = Vector2.op_Implicit(((Rect) ref rect).size);
    this.m_ViewBounds = new Bounds(vector3_1, vector3_2);
    this.m_ContentBounds = this.GetBounds();
  }

  private void UpdateScrollbarVisibility()
  {
    if (Object.op_Implicit((Object) this.m_VerticalScrollbar) && this.m_VerticalScrollbarVisibility != ScrollRectExtended.ScrollbarVisibility.Permanent && ((Component) this.m_VerticalScrollbar).gameObject.activeSelf != this.vScrollingNeeded)
      ((Component) this.m_VerticalScrollbar).gameObject.SetActive(this.vScrollingNeeded);
    if (!Object.op_Implicit((Object) this.m_HorizontalScrollbar) || this.m_HorizontalScrollbarVisibility == ScrollRectExtended.ScrollbarVisibility.Permanent || ((Component) this.m_HorizontalScrollbar).gameObject.activeSelf == this.hScrollingNeeded)
      return;
    ((Component) this.m_HorizontalScrollbar).gameObject.SetActive(this.hScrollingNeeded);
  }

  private void UpdateScrollbarLayout()
  {
    if (this.m_VSliderExpand && Object.op_Implicit((Object) this.m_HorizontalScrollbar))
    {
      ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.m_HorizontalScrollbarRect, (DrivenTransformProperties) 5378);
      this.m_HorizontalScrollbarRect.anchorMin = new Vector2(0.0f, this.m_HorizontalScrollbarRect.anchorMin.y);
      this.m_HorizontalScrollbarRect.anchorMax = new Vector2(1f, this.m_HorizontalScrollbarRect.anchorMax.y);
      this.m_HorizontalScrollbarRect.anchoredPosition = new Vector2(0.0f, this.m_HorizontalScrollbarRect.anchoredPosition.y);
      this.m_HorizontalScrollbarRect.sizeDelta = !this.vScrollingNeeded ? new Vector2(0.0f, this.m_HorizontalScrollbarRect.sizeDelta.y) : new Vector2((float) -((double) this.m_VSliderWidth + (double) this.m_VerticalScrollbarSpacing), this.m_HorizontalScrollbarRect.sizeDelta.y);
    }
    if (!this.m_HSliderExpand || !Object.op_Implicit((Object) this.m_VerticalScrollbar))
      return;
    ((DrivenRectTransformTracker) ref this.m_Tracker).Add((Object) this, this.m_VerticalScrollbarRect, (DrivenTransformProperties) 10756);
    this.m_VerticalScrollbarRect.anchorMin = new Vector2(this.m_VerticalScrollbarRect.anchorMin.x, 0.0f);
    this.m_VerticalScrollbarRect.anchorMax = new Vector2(this.m_VerticalScrollbarRect.anchorMax.x, 1f);
    this.m_VerticalScrollbarRect.anchoredPosition = new Vector2(this.m_VerticalScrollbarRect.anchoredPosition.x, 0.0f);
    if (this.hScrollingNeeded)
      this.m_VerticalScrollbarRect.sizeDelta = new Vector2(this.m_VerticalScrollbarRect.sizeDelta.x, (float) -((double) this.m_HSliderHeight + (double) this.m_HorizontalScrollbarSpacing));
    else
      this.m_VerticalScrollbarRect.sizeDelta = new Vector2(this.m_VerticalScrollbarRect.sizeDelta.x, 0.0f);
  }

  private void UpdateBounds()
  {
    Rect rect = this.viewRect.rect;
    Vector3 vector3_1 = Vector2.op_Implicit(((Rect) ref rect).center);
    rect = this.viewRect.rect;
    Vector3 vector3_2 = Vector2.op_Implicit(((Rect) ref rect).size);
    this.m_ViewBounds = new Bounds(vector3_1, vector3_2);
    this.m_ContentBounds = this.GetBounds();
    if (Object.op_Equality((Object) this.m_Content, (Object) null))
      return;
    Vector3 size = ((Bounds) ref this.m_ContentBounds).size;
    Vector3 center = ((Bounds) ref this.m_ContentBounds).center;
    Vector3 vector3_3 = Vector3.op_Subtraction(((Bounds) ref this.m_ViewBounds).size, size);
    if ((double) vector3_3.x > 0.0)
    {
      center.x -= vector3_3.x * (this.m_Content.pivot.x - 0.5f);
      size.x = ((Bounds) ref this.m_ViewBounds).size.x;
    }
    if ((double) vector3_3.y > 0.0)
    {
      center.y -= vector3_3.y * (this.m_Content.pivot.y - 0.5f);
      size.y = ((Bounds) ref this.m_ViewBounds).size.y;
    }
    ((Bounds) ref this.m_ContentBounds).size = size;
    ((Bounds) ref this.m_ContentBounds).center = center;
  }

  private Bounds GetBounds()
  {
    if (Object.op_Equality((Object) this.m_Content, (Object) null))
      return new Bounds();
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(float.MaxValue, float.MaxValue, float.MaxValue);
    Vector3 vector3_2;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_2).\u002Ector(float.MinValue, float.MinValue, float.MinValue);
    Matrix4x4 worldToLocalMatrix = ((Transform) this.viewRect).worldToLocalMatrix;
    this.m_Content.GetWorldCorners(this.m_Corners);
    for (int index = 0; index < 4; ++index)
    {
      Vector3 vector3_3 = ((Matrix4x4) ref worldToLocalMatrix).MultiplyPoint3x4(this.m_Corners[index]);
      vector3_1 = Vector3.Min(vector3_3, vector3_1);
      vector3_2 = Vector3.Max(vector3_3, vector3_2);
    }
    Bounds bounds;
    // ISSUE: explicit constructor call
    ((Bounds) ref bounds).\u002Ector(vector3_1, Vector3.zero);
    ((Bounds) ref bounds).Encapsulate(vector3_2);
    return bounds;
  }

  private Vector2 CalculateOffset(Vector2 delta)
  {
    Vector2 zero = Vector2.zero;
    if (this.m_MovementType == ScrollRectExtended.MovementType.Unrestricted)
      return zero;
    Vector2 vector2_1 = Vector2.op_Implicit(((Bounds) ref this.m_ContentBounds).min);
    Vector2 vector2_2 = Vector2.op_Implicit(((Bounds) ref this.m_ContentBounds).max);
    if (this.m_Horizontal)
    {
      vector2_1.x += delta.x;
      vector2_2.x += delta.x;
      if ((double) vector2_1.x > (double) ((Bounds) ref this.m_ViewBounds).min.x)
        zero.x = ((Bounds) ref this.m_ViewBounds).min.x - vector2_1.x;
      else if ((double) vector2_2.x < (double) ((Bounds) ref this.m_ViewBounds).max.x)
        zero.x = ((Bounds) ref this.m_ViewBounds).max.x - vector2_2.x;
    }
    if (this.m_Vertical)
    {
      vector2_1.y += delta.y;
      vector2_2.y += delta.y;
      if ((double) vector2_2.y < (double) ((Bounds) ref this.m_ViewBounds).max.y)
        zero.y = ((Bounds) ref this.m_ViewBounds).max.y - vector2_2.y;
      else if ((double) vector2_1.y > (double) ((Bounds) ref this.m_ViewBounds).min.y)
        zero.y = ((Bounds) ref this.m_ViewBounds).min.y - vector2_1.y;
    }
    return zero;
  }

  protected void SetDirty()
  {
    if (!base.IsActive())
      return;
    LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
  }

  protected void SetDirtyCaching()
  {
    if (!base.IsActive())
      return;
    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild((ICanvasElement) this);
    LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
  }

  public bool isHorizontalScrollAvailable()
  {
    Rect rect = this.content.rect;
    double width1 = (double) ((Rect) ref rect).width;
    rect = this.viewport.rect;
    double width2 = (double) ((Rect) ref rect).width;
    return width1 > width2;
  }

  public bool isVerticalScrollAvailable()
  {
    Rect rect = this.content.rect;
    double height1 = (double) ((Rect) ref rect).height;
    rect = this.viewport.rect;
    double height2 = (double) ((Rect) ref rect).height;
    return height1 > height2;
  }

  public bool isDragged() => this.m_Dragging;

  public static bool isAnyDragged()
  {
    for (int index = 0; index < ScrollRectExtended.instances.Count; ++index)
    {
      if (ScrollRectExtended.instances[index].isDragged())
        return true;
    }
    return false;
  }

  public static void SendMessageToAll(string pMessage, PointerEventData pEventData)
  {
    for (int index = 0; index < ScrollRectExtended.instances.Count; ++index)
      ((Component) ScrollRectExtended.instances[index]).SendMessage(pMessage, (object) pEventData);
  }

  Transform ICanvasElement.get_transform() => ((Component) this).transform;

  public enum MovementType
  {
    Unrestricted,
    Elastic,
    Clamped,
  }

  public enum ScrollbarVisibility
  {
    Permanent,
    AutoHide,
    AutoHideAndExpandViewport,
  }

  [Serializable]
  public class ScrollRectEvent : UnityEvent<Vector2>
  {
  }
}
