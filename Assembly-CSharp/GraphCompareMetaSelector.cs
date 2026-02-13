// Decompiled with JetBrains decompiler
// Type: GraphCompareMetaSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (ScrollableButton))]
public class GraphCompareMetaSelector : 
  MonoBehaviour,
  IInitializePotentialDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IBeginDragHandler,
  IEndDragHandler,
  IDraggable
{
  [SerializeField]
  private bool _spawn_particles_on_drag = true;
  private Vector3 _start_local_position;
  private Transform _start_parent;
  private ScrollableButton _scrollable_button;
  private readonly List<Graphic> _raycastables = new List<Graphic>();
  private Vector2 _first_position = Vector2.zero;
  private bool _dragging;
  private readonly List<RectTransform> _dropzones = new List<RectTransform>();
  private GraphCompareWindow _window;

  public bool spawn_particles_on_drag => this._spawn_particles_on_drag;

  private Transform _attach_parent => World.world.drag_parent;

  private void Awake()
  {
    this._scrollable_button = ((Component) this).GetComponent<ScrollableButton>();
    this._start_parent = ((Component) this).transform.parent;
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(showTooltip)));
  }

  private void showTooltip()
  {
    IBanner component = ((Component) this).GetComponent<IBanner>();
    if (InputHelpers.mouseSupported || Tooltip.isShowingFor((object) component))
      return;
    component.showTooltip();
  }

  public void addWindow(GraphCompareWindow pWindow) => this._window = pWindow;

  public void addDropzones(params RectTransform[] pDropzones)
  {
    this._dropzones.Clear();
    this._dropzones.AddRange((IEnumerable<RectTransform>) pDropzones);
  }

  public bool isBeingDragged() => this._dragging;

  public void OnInitializePotentialDrag(PointerEventData pEventData)
  {
    this._dragging = false;
    this._first_position = pEventData.position;
    this._start_parent = ((Component) this).transform.parent;
    this._start_local_position = ((Component) this).transform.localPosition;
  }

  public bool checkIfDragging(PointerEventData pEventData)
  {
    if (this._window.countNoosItems() < 5)
      return true;
    Vector2 p1;
    // ISSUE: explicit constructor call
    ((Vector2) ref p1).\u002Ector(float.MaxValue, 0.0f);
    Vector2 p2;
    // ISSUE: explicit constructor call
    ((Vector2) ref p2).\u002Ector(float.MinValue, 0.0f);
    foreach (RectTransform dropzone in this._dropzones)
    {
      Vector2 vector2_1 = Vector2.op_Implicit(((Transform) dropzone).position);
      ref float local1 = ref vector2_1.x;
      double num1 = (double) local1;
      Rect rect = dropzone.rect;
      double num2 = (double) ((Rect) ref rect).width * (double) ((Transform) dropzone).lossyScale.x / 2.0;
      local1 = (float) (num1 - num2);
      ref float local2 = ref vector2_1.y;
      double num3 = (double) local2;
      rect = dropzone.rect;
      double num4 = (double) ((Rect) ref rect).height * (double) ((Transform) dropzone).lossyScale.y / 2.0;
      local2 = (float) (num3 - num4);
      Vector2 vector2_2 = Vector2.op_Implicit(((Transform) dropzone).position);
      ref float local3 = ref vector2_2.x;
      double num5 = (double) local3;
      rect = dropzone.rect;
      double num6 = (double) ((Rect) ref rect).width * (double) ((Transform) dropzone).lossyScale.x / 2.0;
      local3 = (float) (num5 + num6);
      ref float local4 = ref vector2_2.y;
      double num7 = (double) local4;
      rect = dropzone.rect;
      double num8 = (double) ((Rect) ref rect).height * (double) ((Transform) dropzone).lossyScale.y / 2.0;
      local4 = (float) (num7 - num8);
      if ((double) vector2_1.x < (double) p1.x)
        p1 = vector2_1;
      if ((double) vector2_2.x > (double) p2.x)
        p2 = vector2_2;
    }
    if (!Toolbox.isInTriangle(pEventData.position, this._first_position, p1, p2))
    {
      Vector2 vector2 = Vector2.op_Subtraction(pEventData.position, this._first_position);
      if ((double) Mathf.Abs(vector2.x) > (double) Mathf.Abs(vector2.y))
        return false;
    }
    return true;
  }

  public void OnBeginDrag(PointerEventData pEventData)
  {
    if (Config.isDraggingItem() || this._dragging)
      return;
    this._dragging = this.checkIfDragging(pEventData);
    if (!this._dragging)
      return;
    Config.setDraggingObject((IDraggable) this);
    ((AbstractEventData) pEventData).Use();
    ((Behaviour) this._scrollable_button).enabled = false;
    GraphCompareMetaObject.disable_raycasts = true;
    ((Component) this).transform.SetParent(this._attach_parent);
    ((Component) this).transform.position = Vector2.op_Implicit(pEventData.position);
    this.disableRaycast();
  }

  public void OnDrag(PointerEventData pEventData)
  {
    if (!this._dragging || !Config.isDraggingObject((IDraggable) this))
      return;
    ((AbstractEventData) pEventData).Use();
    ((Component) this).transform.position = Vector2.op_Implicit(pEventData.position);
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    this._scrollable_button.OnEndDrag(pEventData);
    if (!this._dragging || !Config.isDraggingObject((IDraggable) this))
      return;
    ((AbstractEventData) pEventData).Use();
    ((Component) this).transform.SetParent(this._start_parent);
    ((Component) this).transform.localPosition = this._start_local_position;
    this.resetDrag();
  }

  public void resetDrag()
  {
    if (!this._dragging)
      return;
    Config.clearDraggingObject();
    this._dragging = false;
    ((Behaviour) this._scrollable_button).enabled = true;
    GraphCompareMetaObject.disable_raycasts = false;
    foreach (Graphic raycastable in this._raycastables)
      raycastable.raycastTarget = true;
  }

  private void disableRaycast()
  {
    this._raycastables.Clear();
    foreach (Graphic componentsInChild in ((Component) this).GetComponentsInChildren<Graphic>())
    {
      if (componentsInChild.raycastTarget)
        this._raycastables.Add(componentsInChild);
    }
    foreach (Graphic raycastable in this._raycastables)
      raycastable.raycastTarget = false;
  }

  private void OnDisable() => this.resetDrag();

  public void KillDrag() => this.OnDisable();

  Transform IDraggable.get_transform() => ((Component) this).transform;
}
