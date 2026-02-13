// Decompiled with JetBrains decompiler
// Type: DragOrderContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DragOrderContainer : MonoBehaviour
{
  internal static float drag_delay = 0.25f;
  public MonoBehaviour scroll_rect;
  public DragOrderContainer.SnapAxis snapping_axis = DragOrderContainer.SnapAxis.No;
  public bool limit_moving;
  public bool delay_before_drag = true;
  public bool debug;
  public Action on_order_changed;
  internal DragOrderElement dragging_element;
  internal bool is_anything_dragging;
  internal RectTransform rect_transform;
  internal LayoutGroup grid_layout;
  internal LayoutElement layout_element;
  private List<DragOrderElement> _elements = new List<DragOrderElement>();
  private Dictionary<int, DragOrderElement> _elements_dict = new Dictionary<int, DragOrderElement>();
  private Dictionary<int, Vector2> _children_positions = new Dictionary<int, Vector2>();
  private Dictionary<int, Rect> _children_rects = new Dictionary<int, Rect>();
  private Transform _to_ignore_in_intersection;
  private int _previous_elements_count;
  private bool _marked_for_update;
  private int _marked_for_update_on_frame;
  private bool _initialized;

  private void Awake()
  {
    if (Object.op_Equality((Object) this.scroll_rect, (Object) null))
      this.scroll_rect = (MonoBehaviour) ((Component) this).GetComponentInParent<ScrollRectExtended>();
    if (Object.op_Equality((Object) this.scroll_rect, (Object) null))
      this.scroll_rect = (MonoBehaviour) ((Component) this).GetComponentInParent<ScrollRect>();
    this.rect_transform = ((Component) this).GetComponent<RectTransform>();
    this.grid_layout = ((Component) this).GetComponent<LayoutGroup>();
    this.layout_element = ((Component) this).gameObject.AddOrGetComponent<LayoutElement>();
    ((Behaviour) this.layout_element).enabled = false;
  }

  private void markForUpdate()
  {
    this._marked_for_update = true;
    this._marked_for_update_on_frame = Time.frameCount;
  }

  private void OnApplicationFocus(bool pHasFocus)
  {
    if (pHasFocus)
      return;
    this.disable();
  }

  private void OnEnable()
  {
    this.markForUpdate();
    ScrollWindow.addCallbackShow(new ScrollWindowNameAction(this.onWindowClose));
    ScrollWindow.addCallbackHide(new ScrollWindowNameAction(this.onWindowClose));
  }

  private void OnDisable()
  {
    this.disable();
    ScrollWindow.removeCallbackShow(new ScrollWindowNameAction(this.onWindowClose));
    ScrollWindow.removeCallbackHide(new ScrollWindowNameAction(this.onWindowClose));
  }

  private void onWindowClose(string pId) => this.disable();

  private void disable()
  {
    ((Behaviour) this.grid_layout).enabled = true;
    LayoutRebuilder.MarkLayoutForRebuild(this.rect_transform);
    if (Object.op_Inequality((Object) this.dragging_element, (Object) null))
      this.dragging_element.stopDrag();
    foreach (DragOrderElement element in this._elements)
    {
      if (!element.is_target_reached)
      {
        element.is_target_reached = true;
        element.unsetOnTop();
      }
    }
  }

  private void Update()
  {
    if (this._marked_for_update && this._marked_for_update_on_frame != Time.frameCount)
    {
      this._marked_for_update = false;
      this.updateChildrenData();
    }
    this.checkIntersections();
    this.updatePositions();
  }

  private void OnDrawGizmos()
  {
    if (!this.debug)
      return;
    foreach (Rect pRect in this._children_rects.Values)
    {
      ((Rect) ref pRect).min = Vector2.op_Implicit(((Transform) this.rect_transform).TransformPoint(Vector2.op_Implicit(((Rect) ref pRect).min)));
      ((Rect) ref pRect).max = Vector2.op_Implicit(((Transform) this.rect_transform).TransformPoint(Vector2.op_Implicit(((Rect) ref pRect).max)));
      DragOrderContainer.drawRect(pRect, Color.green);
    }
  }

  private void checkIntersections()
  {
    if (!this.is_anything_dragging)
      return;
    DragOrderElement intersectedWith = this.getIntersectedWith();
    if (Object.op_Equality((Object) intersectedWith, (Object) null))
    {
      this._to_ignore_in_intersection = (Transform) null;
    }
    else
    {
      if (Object.op_Equality((Object) intersectedWith.main_transform, (Object) this._to_ignore_in_intersection))
        return;
      this._to_ignore_in_intersection = (Transform) intersectedWith.main_transform;
      this.switchElements(this.dragging_element, intersectedWith);
      Action onOrderChanged = this.on_order_changed;
      if (onOrderChanged == null)
        return;
      onOrderChanged();
    }
  }

  private DragOrderElement getIntersectedWith()
  {
    int orderIndex = this.dragging_element.order_index;
    Vector2 vector2 = Vector2.op_Implicit(((Transform) this.dragging_element.main_transform).localPosition);
    RectTransform rectTransform = this.rect_transform;
    Rect childrenRect1 = this._children_rects[orderIndex];
    Vector3 vector3 = Vector2.op_Implicit(((Rect) ref childrenRect1).center);
    Debug.DrawLine(((Transform) rectTransform).TransformPoint(vector3), ((Transform) this.rect_transform).TransformPoint(Vector2.op_Implicit(vector2)));
    if (this.snapping_axis != DragOrderContainer.SnapAxis.No)
    {
      int key1 = 0;
      int key2 = this._elements.Count - 1;
      Rect childrenRect2 = this._children_rects[key1];
      Rect childrenRect3 = this._children_rects[key2];
      if (this.snapping_axis == DragOrderContainer.SnapAxis.Horizontal)
      {
        if ((double) vector2.x <= (double) ((Rect) ref childrenRect2).xMax)
          return this._elements_dict[key1];
        if ((double) vector2.x >= (double) ((Rect) ref childrenRect3).xMin)
          return this._elements_dict[key2];
      }
      if (this.snapping_axis == DragOrderContainer.SnapAxis.Vertical)
      {
        if ((double) vector2.y >= (double) ((Rect) ref childrenRect2).yMax)
          return this._elements_dict[key1];
        if ((double) vector2.y <= (double) ((Rect) ref childrenRect3).yMin)
          return this._elements_dict[key2];
      }
    }
    for (int key = 0; key < this._elements.Count; ++key)
    {
      if (key != orderIndex)
      {
        Rect childrenRect4 = this._children_rects[key];
        if (((Rect) ref childrenRect4).Contains(vector2))
          return this._elements_dict[key];
      }
    }
    return (DragOrderElement) null;
  }

  private void updatePositions()
  {
    if (((Behaviour) this.grid_layout).enabled)
      return;
    bool flag = false;
    foreach (DragOrderElement element in this._elements)
    {
      if (!Object.op_Equality((Object) element, (Object) this.dragging_element))
      {
        element.updatePosition();
        if (!element.is_target_reached)
          flag = true;
      }
    }
    if (flag || this.is_anything_dragging)
      return;
    ((Behaviour) this.grid_layout).enabled = true;
  }

  public void updateChildrenData()
  {
    LayoutElement layoutElement1 = this.layout_element;
    Rect rect1 = this.rect_transform.rect;
    double height = (double) ((Rect) ref rect1).height;
    layoutElement1.minHeight = (float) height;
    LayoutElement layoutElement2 = this.layout_element;
    Rect rect2 = this.rect_transform.rect;
    double width = (double) ((Rect) ref rect2).width;
    layoutElement2.minWidth = (float) width;
    this._elements.Clear();
    this._elements_dict.Clear();
    this._children_positions.Clear();
    this._children_rects.Clear();
    DragOrderElement[] componentsInChildren = ((Component) this.rect_transform).GetComponentsInChildren<DragOrderElement>();
    int key = 0;
    foreach (DragOrderElement dragOrderElement in componentsInChildren)
    {
      Vector2 vector2 = dragOrderElement.is_target_reached || this._previous_elements_count != componentsInChildren.Length ? Vector2.op_Implicit(((Transform) dragOrderElement.main_transform).localPosition) : dragOrderElement.current_destination;
      dragOrderElement.order_index = key;
      this._elements.Add(dragOrderElement);
      this._elements_dict.Add(key, dragOrderElement);
      this._children_positions.Add(key, vector2);
      Rect rect3 = dragOrderElement.getRect();
      this._children_rects.Add(key, rect3);
      dragOrderElement.current_destination = vector2;
      dragOrderElement.unsetOnTop();
      ++key;
    }
    this._previous_elements_count = componentsInChildren.Length;
  }

  private void switchElements(DragOrderElement pFirst, DragOrderElement pSecond)
  {
    ((Transform) pFirst.main_transform).SetSiblingIndex(((Transform) pSecond.main_transform).GetSiblingIndex());
    int orderIndex1 = pFirst.order_index;
    int orderIndex2 = pSecond.order_index;
    bool tIsAscending = orderIndex1 > orderIndex2;
    pFirst.order_index = orderIndex2;
    this._elements.Sort((Comparison<DragOrderElement>) ((e1, e2) => this.sort(e1, e2, tIsAscending)));
    int orderIndex3 = pFirst.order_index;
    foreach (DragOrderElement element in this._elements)
    {
      if (!Object.op_Equality((Object) element, (Object) pFirst) && (!tIsAscending || element.order_index >= orderIndex3) && (tIsAscending || element.order_index <= orderIndex3) && element.order_index == orderIndex3)
      {
        element.order_index += tIsAscending ? 1 : -1;
        orderIndex3 = element.order_index;
      }
    }
    foreach (DragOrderElement element in this._elements)
      this._elements_dict[element.order_index] = element;
  }

  public Vector3 getChildPosition(int pIndex)
  {
    return Vector2.op_Implicit(this._children_positions[pIndex]);
  }

  private int sort(DragOrderElement pFirst, DragOrderElement pSecond, bool pIsAscending)
  {
    return pFirst.order_index.CompareTo(pSecond.order_index) * (pIsAscending ? 1 : -1);
  }

  private static void drawRect(Rect pRect, Color pColor)
  {
    Vector3 vector3_1 = Vector2.op_Implicit(((Rect) ref pRect).min);
    Vector3 vector3_2 = Vector2.op_Implicit(((Rect) ref pRect).max);
    Debug.DrawLine(vector3_1, new Vector3(vector3_1.x, vector3_2.y), pColor);
    Debug.DrawLine(new Vector3(vector3_1.x, vector3_2.y), vector3_2, pColor);
    Debug.DrawLine(vector3_2, new Vector3(vector3_2.x, vector3_1.y), pColor);
    Debug.DrawLine(vector3_1, new Vector3(vector3_2.x, vector3_1.y), pColor);
  }

  public enum SnapAxis
  {
    Horizontal,
    Vertical,
    No,
  }
}
