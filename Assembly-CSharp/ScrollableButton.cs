// Decompiled with JetBrains decompiler
// Type: ScrollableButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[DisallowMultipleComponent]
public class ScrollableButton : 
  MonoBehaviour,
  IBeginDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IEndDragHandler,
  IInitializePotentialDragHandler,
  IScrollHandler
{
  private ScrollRect _scroll_rect;
  private ScrollRectExtended _scroll_rect_extended;
  private Button _button;
  private bool _has_button;
  [SerializeField]
  private bool _scroll_wheel_only;

  protected void Start()
  {
    this._scroll_rect_extended = ((Component) this).gameObject.GetComponentInParent<ScrollRectExtended>();
    if (Object.op_Equality((Object) this._scroll_rect_extended, (Object) null))
      this._scroll_rect = ((Component) this).gameObject.GetComponentInParent<ScrollRect>();
    if (Object.op_Equality((Object) this._scroll_rect, (Object) null) && Object.op_Equality((Object) this._scroll_rect_extended, (Object) null))
      ((Behaviour) this).enabled = false;
    this._has_button = ((Component) this).gameObject.TryGetComponent<Button>(ref this._button);
  }

  public void OnBeginDrag(PointerEventData pEventData)
  {
    if (this._scroll_wheel_only)
      return;
    this.sendMessage(nameof (OnBeginDrag), pEventData);
    if (!this._has_button)
      return;
    ((Selectable) this._button).interactable = false;
  }

  public void OnDrag(PointerEventData pEventData)
  {
    if (this._scroll_wheel_only)
      return;
    this.sendMessage(nameof (OnDrag), pEventData);
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    if (this._scroll_wheel_only)
      return;
    this.sendMessage(nameof (OnEndDrag), pEventData);
    if (!this._has_button)
      return;
    ((Selectable) this._button).interactable = true;
  }

  public void OnInitializePotentialDrag(PointerEventData pEventData)
  {
    if (this._scroll_wheel_only)
      return;
    this.sendMessage(nameof (OnInitializePotentialDrag), pEventData);
  }

  public void OnScroll(PointerEventData pEventData)
  {
    this.sendMessage(nameof (OnScroll), pEventData);
  }

  private void sendMessage(string pMethodName, PointerEventData pEventData)
  {
    ((Component) this._scroll_rect)?.SendMessage(pMethodName, (object) pEventData);
    ((Component) this._scroll_rect_extended)?.SendMessage(pMethodName, (object) pEventData);
  }
}
