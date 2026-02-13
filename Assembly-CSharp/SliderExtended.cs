// Decompiled with JetBrains decompiler
// Type: SliderExtended
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class SliderExtended : Slider, IEndDragHandler, IEventSystemHandler
{
  private SliderEndedEvent _on_sliding_ended;
  private SliderPointerDownEvent _on_pointer_down;

  public void OnEndDrag(PointerEventData pEventData)
  {
    SliderEndedEvent onSlidingEnded = this._on_sliding_ended;
    if (onSlidingEnded == null)
      return;
    onSlidingEnded();
  }

  public virtual void OnPointerDown(PointerEventData pEventData)
  {
    base.OnPointerDown(pEventData);
    ScrollWindow.getCurrentWindow().scrollRect.StopMovement();
    SliderPointerDownEvent onPointerDown = this._on_pointer_down;
    if (onPointerDown == null)
      return;
    onPointerDown();
  }

  public void addCallbackDragEnd(SliderEndedEvent pCallback) => this._on_sliding_ended += pCallback;

  public void removeCallbackDragEnd(SliderEndedEvent pCallback)
  {
    this._on_sliding_ended -= pCallback;
  }

  public void addCallbackPointerDown(SliderPointerDownEvent pCallback)
  {
    this._on_pointer_down += pCallback;
  }

  public void removeCallbackPointerDown(SliderPointerDownEvent pCallback)
  {
    this._on_pointer_down -= pCallback;
  }
}
