// Decompiled with JetBrains decompiler
// Type: ScrollableSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[DisallowMultipleComponent]
[RequireComponent(typeof (Slider))]
[RequireComponent(typeof (SliderExtended))]
public class ScrollableSlider : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
  private ScrollRect _scroll_rect;
  private ScrollRectExtended _scroll_rect_extended;

  protected void Start()
  {
    this._scroll_rect_extended = ((Component) this).gameObject.GetComponentInParent<ScrollRectExtended>();
    if (Object.op_Equality((Object) this._scroll_rect_extended, (Object) null))
      this._scroll_rect = ((Component) this).gameObject.GetComponentInParent<ScrollRect>();
    if (!Object.op_Equality((Object) this._scroll_rect, (Object) null) || !Object.op_Equality((Object) this._scroll_rect_extended, (Object) null))
      return;
    ((Behaviour) this).enabled = false;
  }

  public void OnScroll(PointerEventData pEventData)
  {
    ((Component) this._scroll_rect)?.SendMessage(nameof (OnScroll), (object) pEventData);
    ((Component) this._scroll_rect_extended)?.SendMessage(nameof (OnScroll), (object) pEventData);
  }
}
