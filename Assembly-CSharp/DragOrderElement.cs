// Decompiled with JetBrains decompiler
// Type: DragOrderElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using LayoutGroupExt;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class DragOrderElement : MonoBehaviour, IDraggable, IEndDragHandler, IEventSystemHandler
{
  [SerializeField]
  private bool _spawn_particles_on_drag = true;
  public RectTransform main_transform;
  public bool can_be_dragged = true;
  private DragOrderContainer _container;
  private int _parent_canvas_sorting_order;
  private Canvas _canvas;
  private GraphicRaycaster _raycaster;
  private Button _button;
  private Transform _current_parent;
  internal Vector2 current_destination;
  internal bool is_target_reached;
  internal int order_index;
  private bool _drag_initialized;
  private float _drag_started_at;
  private int _mouse_button = -1;
  private Vector3 _prev_mouse_position;

  public bool spawn_particles_on_drag => this._spawn_particles_on_drag;

  private void Start()
  {
    if (Object.op_Equality((Object) this.main_transform, (Object) null))
      this.main_transform = ((Component) this).GetComponent<RectTransform>();
    this._parent_canvas_sorting_order = ((Component) this.main_transform).gameObject.GetComponentInParent<Canvas>().sortingOrder;
    this._canvas = ((Component) this.main_transform).gameObject.AddComponent<Canvas>();
    this._canvas.sortingOrder = this._parent_canvas_sorting_order;
    this._canvas.overrideSorting = false;
    this._raycaster = ((Component) this.main_transform).gameObject.AddComponent<GraphicRaycaster>();
    this._raycaster.blockingObjects = (GraphicRaycaster.BlockingObjects) 3;
    this._raycaster.blockingMask = LayerMask.op_Implicit(-1);
    this._raycaster.ignoreReversedGraphics = true;
    this._button = ((Component) this).GetComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Eb__18_0)));
    this.is_target_reached = true;
    this.checkContainer();
  }

  private void checkContainer()
  {
    if (Object.op_Inequality((Object) this._container, (Object) null))
      return;
    this._container = ((Component) this.main_transform).GetComponentInParent<DragOrderContainer>();
  }

  private void checkParent()
  {
    Transform parent = ((Transform) this.main_transform).parent;
    if (Object.op_Equality((Object) this._current_parent, (Object) parent))
      return;
    this._current_parent = parent;
    this.checkContainer();
  }

  private void Update()
  {
    if (!((Behaviour) this).enabled)
      return;
    this.checkParent();
    if (Object.op_Equality((Object) this._container, (Object) null))
      return;
    this.checkDrag();
    if (((Behaviour) this._container.grid_layout).enabled)
      return;
    if (Object.op_Equality((Object) this._container.dragging_element, (Object) this))
    {
      this.moveDraggingTab();
    }
    else
    {
      if (this.is_target_reached)
        return;
      if ((double) Vector3.Distance(((Transform) this.main_transform).localPosition, Vector2.op_Implicit(this.current_destination)) < 0.10000000149011612)
      {
        this.is_target_reached = true;
        this.unsetOnTop();
      }
      else
        ((Transform) this.main_transform).localPosition = Vector3.Lerp(((Transform) this.main_transform).localPosition, Vector2.op_Implicit(this.current_destination), Time.deltaTime * 10f);
    }
  }

  private void setOnTop()
  {
    this._canvas.overrideSorting = true;
    this._canvas.sortingOrder = 24;
  }

  internal void unsetOnTop()
  {
    if (!this._canvas.overrideSorting)
      return;
    this._canvas.sortingOrder = this._parent_canvas_sorting_order;
    this._canvas.overrideSorting = false;
  }

  public void updatePosition()
  {
    Vector2 vector2 = Vector2.op_Implicit(this.getChildPositionInContainer());
    if (Vector2.op_Equality(Vector2.op_Implicit(((Transform) this.main_transform).localPosition), vector2) || Vector2.op_Equality(this.current_destination, vector2))
      return;
    this.current_destination = vector2;
    this.is_target_reached = false;
  }

  private void moveDraggingTab()
  {
    if (!this._container.is_anything_dragging)
      this.endDrag();
    else if (!InputHelpers.GetMouseButton(this._mouse_button))
    {
      this.endDrag();
    }
    else
    {
      Vector3 mousePosition = Input.mousePosition;
      switch (this._container.snapping_axis)
      {
        case DragOrderContainer.SnapAxis.Horizontal:
          mousePosition.y = ((Transform) this.main_transform).position.y;
          break;
        case DragOrderContainer.SnapAxis.Vertical:
          mousePosition.x = ((Transform) this.main_transform).position.x;
          break;
      }
      if (!this._container.limit_moving)
      {
        ((Transform) this.main_transform).position = mousePosition;
      }
      else
      {
        Rect worldRect = this._container.rect_transform.GetWorldRect();
        Vector2 pCellSize;
        this.getGridValues(this._container.grid_layout, out pCellSize, out Vector2 _);
        Vector2 vector2 = Vector2.op_Multiply(pCellSize, 0.5f);
        mousePosition.x = Mathf.Min(mousePosition.x, ((Rect) ref worldRect).xMax - vector2.x);
        mousePosition.x = Mathf.Max(mousePosition.x, ((Rect) ref worldRect).xMin + vector2.x);
        mousePosition.y = Mathf.Min(mousePosition.y, ((Rect) ref worldRect).yMax - vector2.y);
        mousePosition.y = Mathf.Max(mousePosition.y, ((Rect) ref worldRect).yMin + vector2.y);
        ((Transform) this.main_transform).position = mousePosition;
      }
    }
  }

  private void checkDrag()
  {
    this.checkDragBegin();
    this.checkDragEnd();
  }

  private void checkDragBegin()
  {
    if (Config.isDraggingItem() || this._container.is_anything_dragging || !this.can_be_dragged)
      return;
    if (this._container.delay_before_drag && !this.isMouseOver())
    {
      this._drag_initialized = false;
    }
    else
    {
      if (InputHelpers.GetAnyMouseButtonDown() && this.isMouseOver())
      {
        this._mouse_button = InputHelpers.GetAnyMouseButtonDownIndex();
        this._drag_started_at = Time.time;
        if (!this._container.delay_before_drag)
        {
          this._drag_started_at -= DragOrderContainer.drag_delay;
          this._prev_mouse_position = Input.mousePosition;
        }
        this._drag_initialized = true;
      }
      if (!this._drag_initialized)
        return;
      if (!InputHelpers.GetMouseButton(this._mouse_button))
      {
        this._drag_initialized = false;
      }
      else
      {
        if (!this._container.delay_before_drag && !DragOrderElement.shouldStartDrag(Vector2.op_Implicit(Input.mousePosition), Vector2.op_Implicit(this._prev_mouse_position)) || (double) Time.time - (double) this._drag_started_at < (double) DragOrderContainer.drag_delay)
          return;
        this.startDrag();
      }
    }
  }

  private void checkDragEnd()
  {
    if (!InputHelpers.GetMouseButtonUp(this._mouse_button) || !this._container.is_anything_dragging)
      return;
    this._drag_initialized = false;
    this.endDrag();
  }

  public void OnEndDrag(PointerEventData pData)
  {
    if (!this._container.is_anything_dragging)
      return;
    this._drag_initialized = false;
    this.endDrag();
  }

  private void startDrag()
  {
    this._drag_started_at = Time.realtimeSinceStartup;
    this._container.dragging_element = this;
    Config.setDraggingObject((IDraggable) this);
    this._container.is_anything_dragging = true;
    ((Behaviour) this._container.grid_layout).enabled = false;
    ((Behaviour) this._container.layout_element).enabled = true;
    ((Selectable) this._button).interactable = false;
    if (Object.op_Inequality((Object) this._container.scroll_rect, (Object) null))
      ((Behaviour) this._container.scroll_rect).enabled = false;
    this._container.updateChildrenData();
    this.setOnTop();
  }

  public void stopDrag() => this.endDrag();

  private void endDrag()
  {
    if (!Config.isDraggingObject((IDraggable) this))
      return;
    ((Selectable) this._button).interactable = true;
    this._mouse_button = -1;
    if (Object.op_Inequality((Object) this._container.scroll_rect, (Object) null))
      ((Behaviour) this._container.scroll_rect).enabled = true;
    this.current_destination = Vector2.op_Implicit(this.getChildPositionInContainer());
    this.is_target_reached = false;
    if (Object.op_Inequality((Object) this._container.dragging_element, (Object) this))
      return;
    ((Behaviour) this._container.layout_element).enabled = false;
    this._drag_initialized = false;
    this._container.dragging_element = (DragOrderElement) null;
    Config.clearDraggingObject();
    this._container.is_anything_dragging = false;
  }

  private Vector3 getChildPositionInContainer()
  {
    return this._container.getChildPosition(this.order_index);
  }

  private bool isMouseOver()
  {
    Vector2 vector2 = Vector2.op_Implicit(((Transform) this._container.rect_transform).InverseTransformPoint(Input.mousePosition));
    Rect rect = this.getRect();
    return ((Rect) ref rect).Contains(vector2);
  }

  public Rect getRect()
  {
    Vector2 pCellSize;
    Vector2 pSpacing;
    this.getGridValues(this._container.grid_layout, out pCellSize, out pSpacing);
    return new Rect(Vector2.op_Subtraction(Vector2.op_Subtraction(Vector2.op_Implicit(((Transform) this.main_transform).localPosition), Vector2.op_Multiply(pCellSize, this.main_transform.pivot)), Vector2.op_Division(pSpacing, 2f)), Vector2.op_Addition(pCellSize, pSpacing));
  }

  private void getGridValues(LayoutGroup pLayoutGroup, out Vector2 pCellSize, out Vector2 pSpacing)
  {
    switch (pLayoutGroup)
    {
      case GridLayoutGroup gridLayoutGroup:
        pCellSize = gridLayoutGroup.cellSize;
        pSpacing = gridLayoutGroup.spacing;
        break;
      case GridLayoutGroupExtended layoutGroupExtended:
        pCellSize = layoutGroupExtended.cellSize;
        pSpacing = layoutGroupExtended.spacing;
        break;
      default:
        pCellSize = Vector2.zero;
        pSpacing = Vector2.zero;
        break;
    }
  }

  private static bool shouldStartDrag(Vector2 pPressPos, Vector2 pCurrentPos)
  {
    float pixelDragThreshold = (float) EventSystem.current.pixelDragThreshold;
    Vector2 vector2 = Vector2.op_Subtraction(pPressPos, pCurrentPos);
    return (double) ((Vector2) ref vector2).sqrMagnitude >= (double) pixelDragThreshold * (double) pixelDragThreshold;
  }

  private void OnDisable()
  {
    DragOrderContainer container = this._container;
    if ((container != null ? (!container.is_anything_dragging ? 1 : 0) : 1) != 0)
      return;
    this.OnEndDrag(new PointerEventData(EventSystem.current));
  }

  public void KillDrag() => this.OnDisable();

  Transform IDraggable.get_transform() => ((Component) this).transform;
}
