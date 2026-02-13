// Decompiled with JetBrains decompiler
// Type: RunningIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class RunningIcon : 
  MonoBehaviour,
  IDragHandler,
  IEventSystemHandler,
  IBeginDragHandler,
  IEndDragHandler,
  IScrollHandler,
  IPointerClickHandler,
  IDraggable
{
  [SerializeField]
  private Image _icon;
  private RunningIcons _parent;
  private Vector2 _last_position;

  public bool spawn_particles_on_drag => false;

  public void Awake() => this._parent = ((Component) this).GetComponentInParent<RunningIcons>();

  public Image getIconImage() => this._icon;

  public void setIcon(Sprite pIcon) => this._icon.sprite = pIcon;

  public void setIconColor(Color pColor) => ((Graphic) this._icon).color = pColor;

  public void OnBeginDrag(PointerEventData pEventData)
  {
    if (Config.isDraggingItem())
      return;
    Config.setDraggingObject((IDraggable) this);
    this._last_position = pEventData.position;
    this._parent.toggle(false);
  }

  public void OnDrag(PointerEventData pEventData)
  {
    if (!Config.isDraggingObject((IDraggable) this))
      return;
    this._parent.toggle(false);
    Vector2 vector2 = Vector2.op_Subtraction(pEventData.position, this._last_position);
    this._last_position = pEventData.position;
    if ((double) vector2.x == 0.0)
      return;
    float num = vector2.x / CanvasMain.instance.canvas_ui.scaleFactor;
    if ((double) num < 0.0)
      this._parent.moveBy(Mathf.Abs(num), RunningIcons.Direction.Left);
    else
      this._parent.moveBy(Mathf.Abs(num), RunningIcons.Direction.Right);
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    if (!Config.isDraggingItem() || !Config.isDraggingObject((IDraggable) this))
      return;
    Config.clearDraggingObject();
    this._parent.toggle(true);
  }

  public void OnScroll(PointerEventData pEventData)
  {
    if ((double) pEventData.scrollDelta.y < 0.0)
      this._parent.moveBy(Mathf.Abs(pEventData.scrollDelta.y * 20f), RunningIcons.Direction.Left);
    else
      this._parent.moveBy(Mathf.Abs(pEventData.scrollDelta.y * 20f), RunningIcons.Direction.Right);
  }

  public void OnPointerClick(PointerEventData pEventData)
  {
    if (InputHelpers.mouseSupported)
      return;
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).Invoke();
    if (Object.op_Equality((Object) EventSystem.current.currentSelectedGameObject, (Object) ((Component) this._parent).gameObject))
      this._parent.toggle(false);
    EventSystem.current.SetSelectedGameObject(((Component) this._parent).gameObject);
  }

  private void OnDisable() => this.KillDrag();

  public void KillDrag() => this.OnEndDrag(new PointerEventData(EventSystem.current));

  Transform IDraggable.get_transform() => ((Component) this).transform;
}
