// Decompiled with JetBrains decompiler
// Type: DraggableLayoutElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using LayoutGroupExt;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (CanvasGroup))]
public class DraggableLayoutElement : 
  MonoBehaviour,
  IInitializePotentialDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IBeginDragHandler,
  IEndDragHandler,
  ILayoutIgnorer,
  IDraggable
{
  public const float TOUCH_DELAY = 0.2f;
  [SerializeField]
  private bool _spawn_particles_on_drag = true;
  private RectTransform _rect;
  private CanvasGroup _canvas_group;
  private LayoutGroupExtended _parent_layout;
  private RectTransform _parent;
  private Rect _cached_parent_rect;
  private Vector3 _cached_parent_position;
  [SerializeField]
  private Transform _attach_parent;
  [SerializeField]
  private bool _touch_drag_delay;
  private DraggableLayoutElement _drag_object;
  private int _target_index = -1;
  private List<MonoBehaviour> _toggle_elements = new List<MonoBehaviour>(3);
  private static bool _any_dragging;
  private bool? _dragging_cache;
  [SerializeField]
  private bool _drag_only_over_parent = true;
  internal Action<DraggableLayoutElement> start_being_dragged;
  private float _drag_timer_started_at;

  public bool spawn_particles_on_drag => this._spawn_particles_on_drag;

  public bool ignoreLayout { get; set; }

  private List<RectTransform> _siblings => this._parent_layout.m_Children;

  private Vector2[] _sibling_positions => this._parent_layout.m_Positions;

  private void Start()
  {
    this._rect = ((Component) this).GetComponent<RectTransform>();
    this._canvas_group = ((Component) this).GetComponent<CanvasGroup>();
    this._parent = ((Component) ((Component) this).transform.parent).GetComponent<RectTransform>();
    this._parent_layout = ((Component) this._parent).GetComponent<LayoutGroupExtended>();
    if (Object.op_Equality((Object) this._parent_layout, (Object) null))
      return;
    this.addToggleComponent<ScrollableButton>();
    this.addToggleComponent<Button>();
    this.addToggleComponent<TipButton>();
    if (!Object.op_Equality((Object) this._attach_parent, (Object) null))
      return;
    this._attach_parent = World.world.drag_parent;
  }

  private void OnEnable()
  {
    this._cached_parent_position = new Vector3(-1000f, -1000f, -1000f);
    this._target_index = -1;
  }

  public void KillDrag() => this.OnDisable();

  private void OnDisable()
  {
    if (!DraggableLayoutElement._any_dragging || Object.op_Equality((Object) this._drag_object, (Object) null))
      return;
    this.OnEndDrag(new PointerEventData(EventSystem.current));
  }

  public void OnInitializePotentialDrag(PointerEventData pEventData)
  {
    if (!this._touch_drag_delay)
      return;
    this._drag_timer_started_at = Time.time;
  }

  public void OnBeginDrag(PointerEventData pEventData)
  {
    if (Config.isDraggingItem() || this.isTouchDragDelayed() || DraggableLayoutElement._any_dragging)
      return;
    DraggableLayoutElement._any_dragging = true;
    this._drag_object = Object.Instantiate<DraggableLayoutElement>(this, this._attach_parent, true);
    ((Component) this._drag_object).transform.position = Vector2.op_Implicit(pEventData.position);
    this._drag_object.ignoreLayout = true;
    Action<DraggableLayoutElement> startBeingDragged = this._drag_object.start_being_dragged;
    if (startBeingDragged != null)
      startBeingDragged(this);
    this._canvas_group.alpha = 0.2f;
    Config.setDraggingObject((IDraggable) this._drag_object);
  }

  public void OnDrag(PointerEventData pEventData)
  {
    if (this.isTouchDragDelayed() || !DraggableLayoutElement._any_dragging || !Config.isDraggingObject((IDraggable) this._drag_object))
      return;
    ((Component) this._drag_object).transform.position = Vector2.op_Implicit(pEventData.position);
    if (!this.isOverParent(pEventData.position))
      return;
    this.findTarget();
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    ScrollRectExtended.SendMessageToAll(nameof (OnEndDrag), pEventData);
    if (!DraggableLayoutElement._any_dragging || !Config.isDraggingObject((IDraggable) this._drag_object))
      return;
    Config.clearDraggingObject();
    DraggableLayoutElement._any_dragging = false;
    this._drag_timer_started_at = 0.0f;
    Object.Destroy((Object) ((Component) this._drag_object).gameObject);
    this._canvas_group.alpha = 1f;
  }

  public void Update()
  {
    bool? draggingCache = this._dragging_cache;
    bool anyDragging = DraggableLayoutElement._any_dragging;
    if (!(draggingCache.GetValueOrDefault() == anyDragging & draggingCache.HasValue))
    {
      this._dragging_cache = new bool?(DraggableLayoutElement._any_dragging);
      this._canvas_group.interactable = !DraggableLayoutElement._any_dragging;
      this._canvas_group.blocksRaycasts = !DraggableLayoutElement._any_dragging;
      foreach (MonoBehaviour toggleElement in this._toggle_elements)
      {
        if (toggleElement is Selectable)
          (toggleElement as Selectable).interactable = !DraggableLayoutElement._any_dragging;
        else
          ((Behaviour) toggleElement).enabled = !DraggableLayoutElement._any_dragging;
      }
    }
    this.moveToTarget();
  }

  internal void lockToParent(bool pLock = true) => this._drag_only_over_parent = pLock;

  internal void setDragParent(Transform pParent) => this._attach_parent = pParent;

  private void moveToTarget()
  {
    if (this._target_index < 0)
      return;
    int num1 = this._siblings.IndexOf(this._rect);
    int num2 = this._target_index;
    using (ListPool<int> neighbours = this.getNeighbours(num1))
    {
      if (!neighbours.Contains(num2))
        num2 = this.findClosestNeighbour(num2, neighbours);
      this.swapSiblings(num1, num2);
      if (num2 != this._target_index)
        return;
      this._target_index = -1;
    }
  }

  private void recalcParent()
  {
    if (Vector3.op_Equality(this._cached_parent_position, ((Transform) this._parent).position))
      return;
    this._cached_parent_position = ((Transform) this._parent).position;
    this._cached_parent_rect = this._parent.GetWorldRect();
    Rect rect1 = this._rect.rect;
    float num1 = ((Rect) ref rect1).width * 10f;
    Rect rect2 = this._rect.rect;
    float num2 = ((Rect) ref rect2).height * 10f;
    ref Rect local1 = ref this._cached_parent_rect;
    ((Rect) ref local1).x = ((Rect) ref local1).x - num1;
    ref Rect local2 = ref this._cached_parent_rect;
    ((Rect) ref local2).y = ((Rect) ref local2).y - num2;
    ref Rect local3 = ref this._cached_parent_rect;
    ((Rect) ref local3).width = ((Rect) ref local3).width + num1 * 2f;
    ref Rect local4 = ref this._cached_parent_rect;
    ((Rect) ref local4).height = ((Rect) ref local4).height + num2 * 2f;
  }

  private void findTarget()
  {
    if (Object.op_Equality((Object) this._drag_object, (Object) null))
      return;
    Vector3 position = ((Component) this._drag_object).transform.position;
    float num1 = float.MaxValue;
    float num2 = float.MaxValue;
    int num3 = -1;
    int num4 = -1;
    for (int index = 0; index < this._sibling_positions.Length; ++index)
    {
      float num5 = Vector2.Distance(this._sibling_positions[index], Vector2.op_Implicit(position));
      if (Object.op_Equality((Object) this._siblings[index], (Object) this._rect))
      {
        num4 = index;
        num2 = num5;
      }
      if ((double) num5 < (double) num1)
      {
        num1 = num5;
        num3 = index;
      }
    }
    if (num3 == num4 || Mathf.Approximately(num2, num1))
      return;
    this._target_index = num3;
  }

  private bool isOverParent(Vector2 pPosition)
  {
    this.recalcParent();
    return ((Rect) ref this._cached_parent_rect).Contains(pPosition);
  }

  private void swapSiblings(int pStartIndex, int pTargetIndex)
  {
    if (pStartIndex >= this._siblings.Count || pTargetIndex >= this._siblings.Count)
      return;
    int siblingIndex1 = ((Component) this._siblings[pStartIndex]).transform.GetSiblingIndex();
    int siblingIndex2 = ((Component) this._siblings[pTargetIndex]).transform.GetSiblingIndex();
    if (siblingIndex1 > siblingIndex2)
    {
      ((Component) this._siblings[pTargetIndex]).transform.SetSiblingIndex(siblingIndex1);
      ((Component) this).transform.SetSiblingIndex(siblingIndex2);
    }
    else
    {
      ((Component) this).transform.SetSiblingIndex(siblingIndex2);
      ((Component) this._siblings[pTargetIndex]).transform.SetSiblingIndex(siblingIndex1);
    }
    this._siblings.Swap<RectTransform>(pStartIndex, pTargetIndex);
  }

  private int findClosestNeighbour(int pIndex, ListPool<int> pNeighbours)
  {
    // ISSUE: unable to decompile the method.
  }

  private ListPool<int> getNeighbours(int pIndex)
  {
    ListPool<int> neighbours = new ListPool<int>(8);
    if (this._sibling_positions.Length < 2)
      return neighbours;
    Vector2 siblingPosition = this._sibling_positions[pIndex];
    float num = Vector2.Distance(this._sibling_positions[0], this._sibling_positions[1]) * 1.5f;
    for (int index = 0; index < this._sibling_positions.Length; ++index)
    {
      if (index != pIndex && (double) Vector2.Distance(siblingPosition, this._sibling_positions[index]) <= (double) num)
        neighbours.Add(index);
    }
    return neighbours;
  }

  private void addToggleComponent<T>() where T : MonoBehaviour
  {
    if (!((Component) this).HasComponent<T>())
      return;
    this._toggle_elements.Add((MonoBehaviour) ((Component) this).GetComponent<T>());
  }

  private bool isTouchDragDelayed()
  {
    return this._touch_drag_delay && !InputHelpers.mouseSupported && (double) Time.time - (double) this._drag_timer_started_at < 0.20000000298023224;
  }

  Transform IDraggable.get_transform() => ((Component) this).transform;
}
