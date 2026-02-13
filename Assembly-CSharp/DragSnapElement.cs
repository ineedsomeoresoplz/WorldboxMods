// Decompiled with JetBrains decompiler
// Type: DragSnapElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (LayoutElement))]
public class DragSnapElement : 
  MonoBehaviour,
  IInitializePotentialDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IBeginDragHandler,
  IEndDragHandler,
  IScrollHandler,
  IDraggable,
  IPointerEnterHandler,
  IPointerExitHandler
{
  [SerializeField]
  private bool _spawn_particles_on_drag = true;
  [SerializeField]
  private bool _touch_drag_delay;
  private float _drag_timer_started_at;
  private Tweener _tweener;
  private LayoutElement _layout_element;
  private Button _button;
  private Vector3 _start_local_position;
  private Transform _start_parent;
  public float limit_max_drag_distance = 77f;
  public float snapback_max_distance = 77f;
  public float snapback_speed_max_distance = 0.35f;
  public float snapback_min_distance = 22f;
  public float snapback_speed_min_distance = 0.9f;
  public Transform attach_parent;
  public Transform fly_back_parent;
  public Ease ease = (Ease) 24;
  public float speed = 0.4f;
  private ScrollRect _scroll_rect;
  private ScrollRectExtended _scroll_rect_extended;
  private bool _initial_ignore_layout;
  private bool _hovered;
  private bool _is_dragging;

  public bool spawn_particles_on_drag => this._spawn_particles_on_drag;

  private void Start()
  {
    this._layout_element = ((Component) this).GetComponent<LayoutElement>();
    this._button = ((Component) this).GetComponent<Button>();
    this._start_parent = ((Component) this).transform.parent;
    this._start_local_position = ((Component) this).transform.localPosition;
    this._initial_ignore_layout = this._layout_element.ignoreLayout;
    if (Object.op_Equality((Object) this.attach_parent, (Object) null))
      this.attach_parent = World.world.drag_parent;
    if (Object.op_Equality((Object) this.fly_back_parent, (Object) null))
      this.fly_back_parent = ((Component) this).transform.FindParentWithName("Content", "Viewport") ?? this.attach_parent;
    ScrollableButton scrollableButton;
    if (!((Component) this).gameObject.TryGetComponent<ScrollableButton>(ref scrollableButton))
      return;
    ((Behaviour) scrollableButton).enabled = false;
    this._scroll_rect_extended = ((Component) this).gameObject.GetComponentInParent<ScrollRectExtended>();
    if (!Object.op_Equality((Object) this._scroll_rect_extended, (Object) null))
      return;
    this._scroll_rect = ((Component) this).gameObject.GetComponentInParent<ScrollRect>();
  }

  public void OnInitializePotentialDrag(PointerEventData pEventData)
  {
    if (!this._touch_drag_delay)
      return;
    this._drag_timer_started_at = Time.time;
  }

  public void OnBeginDrag(PointerEventData pEventData)
  {
    if (this._is_dragging || Config.isDraggingItem() || this.isTouchDragDelayed())
      return;
    TweenExtensions.Kill((Tween) this._tweener, false);
    Config.setDraggingObject((IDraggable) this);
    this._is_dragging = true;
    ((Component) this).transform.SetParent(this.attach_parent);
    ((Behaviour) this._button).enabled = false;
    ((Behaviour) this._layout_element).enabled = true;
    this.updatePosition(Vector2.op_Implicit(pEventData.position));
  }

  public void OnDrag(PointerEventData pEventData)
  {
    if (!Config.isDraggingObject((IDraggable) this) || this.isTouchDragDelayed())
      return;
    this.updatePosition(Vector2.op_Implicit(pEventData.position));
  }

  public float getDragMod()
  {
    if (!this._is_dragging)
      return 0.0f;
    Vector3 vector3_1 = this._start_parent.TransformPoint(this._start_local_position);
    Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, ((Component) this).transform.position);
    float dragMod = Mathf.Clamp01(((Vector3) ref vector3_2).magnitude / this.limit_max_drag_distance);
    if ((double) vector3_1.y > (double) ((Component) this).transform.position.y)
      dragMod = -dragMod;
    return dragMod;
  }

  private void updatePosition(Vector3 pTargetPosition)
  {
    Vector3 vector3_1 = this._start_parent.TransformPoint(this._start_local_position);
    Vector3 vector3_2 = Vector3.op_Subtraction(pTargetPosition, vector3_1);
    if ((double) ((Vector3) ref vector3_2).magnitude > (double) this.limit_max_drag_distance)
      ((Component) this).transform.position = Vector3.op_Addition(vector3_1, Vector3.op_Multiply(((Vector3) ref vector3_2).normalized, this.limit_max_drag_distance));
    else
      ((Component) this).transform.position = pTargetPosition;
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    if (!Config.isDraggingItem() || !Config.isDraggingObject((IDraggable) this))
      return;
    Config.clearDraggingObject();
    this._is_dragging = false;
    this._drag_timer_started_at = 0.0f;
    this._layout_element.ignoreLayout = true;
    ((Component) this).transform.SetParent(this.fly_back_parent);
    Tweener tweener = this._tweener;
    if (tweener != null)
      TweenExtensions.Kill((Tween) tweener, false);
    Vector3 vector3_1 = this._start_parent.TransformPoint(this._start_local_position);
    Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, ((Component) this).transform.position);
    float num = this.dragSpeed(((Vector3) ref vector3_2).magnitude);
    // ISSUE: method pointer
    // ISSUE: method pointer
    this._tweener = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(DOTween.To(new DOGetter<Vector3>((object) this, __methodptr(\u003COnEndDrag\u003Eb__30_0)), new DOSetter<Vector3>((object) this, __methodptr(\u003COnEndDrag\u003Eb__30_1)), vector3_1, num), this.ease), new TweenCallback(this.resetElement));
    Tooltip.blockTooltips(num * 0.7f);
  }

  public void resetElement()
  {
    if (this._hovered)
      this._button.TriggerHover();
    if (Object.op_Equality((Object) this._start_parent, (Object) null) || Object.op_Inequality((Object) this.fly_back_parent, (Object) this._start_parent) && Object.op_Equality((Object) this._start_parent, (Object) ((Component) this).transform.parent))
      return;
    ((Component) this).transform.SetParent(this._start_parent);
    ((Component) this).transform.localPosition = this._start_local_position;
    ((Behaviour) this._button).enabled = true;
    this._layout_element.ignoreLayout = this._initial_ignore_layout;
    ((Behaviour) this._layout_element).enabled = false;
  }

  public float dragSpeed(float pDistance)
  {
    return Mathf.Lerp(this.snapback_speed_min_distance, this.snapback_speed_max_distance, (float) (((double) Mathf.Clamp(pDistance, this.snapback_min_distance, this.snapback_max_distance) - (double) this.snapback_min_distance) / ((double) this.snapback_max_distance - (double) this.snapback_min_distance)));
  }

  public void onWindowClose(string pId) => TweenExtensions.Kill((Tween) this._tweener, true);

  public void OnScroll(PointerEventData pEventData)
  {
    this.sendMessage(nameof (OnScroll), pEventData);
  }

  public void OnPointerEnter(PointerEventData eventData) => this._hovered = true;

  public void OnPointerExit(PointerEventData eventData) => this._hovered = false;

  private void sendMessage(string pMethodName, PointerEventData pEventData)
  {
    ((Component) this._scroll_rect)?.SendMessage(pMethodName, (object) pEventData);
    ((Component) this._scroll_rect_extended)?.SendMessage(pMethodName, (object) pEventData);
  }

  public void OnEnable()
  {
    ScrollWindow.addCallbackHide(new ScrollWindowNameAction(this.onWindowClose));
  }

  public void OnDisable()
  {
    ScrollWindow.removeCallbackHide(new ScrollWindowNameAction(this.onWindowClose));
    this.KillDrag();
    if (!TweenExtensions.IsActive((Tween) this._tweener))
      return;
    Debug.LogError((object) "OnDisable kill called, shouldn't happen", (Object) this);
    TweenExtensions.Kill((Tween) this._tweener, false);
  }

  public void KillDrag()
  {
    if (!this._is_dragging)
      return;
    this.OnEndDrag(new PointerEventData(EventSystem.current));
    TweenExtensions.Kill((Tween) this._tweener, true);
  }

  private bool isTouchDragDelayed()
  {
    return this._touch_drag_delay && !InputHelpers.mouseSupported && (double) Time.time - (double) this._drag_timer_started_at < 0.20000000298023224;
  }

  Transform IDraggable.get_transform() => ((Component) this).transform;
}
