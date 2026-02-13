// Decompiled with JetBrains decompiler
// Type: UiWindowStretch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class UiWindowStretch : EventTrigger
{
  public RectTransform stretchTarget;
  private bool dragging;
  private Transform mainTransform;
  private Transform canvasContainer;
  public Vector3 posClicked;
  public Vector3 newSize;
  public Vector2 originSizeDelta;

  private void Start()
  {
  }

  public void Update()
  {
    if (!this.dragging)
      return;
    this.newSize = Vector3.op_Subtraction(this.posClicked, Vector2.op_Implicit(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
    this.stretchTarget.sizeDelta = new Vector2(this.originSizeDelta.x - this.newSize.x, this.originSizeDelta.y + this.newSize.y);
  }

  public virtual void OnPointerDown(PointerEventData eventData)
  {
    if (!this.dragging)
    {
      this.posClicked = Vector2.op_Implicit(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
      this.originSizeDelta = this.stretchTarget.sizeDelta;
    }
    this.dragging = true;
  }

  public virtual void OnPointerUp(PointerEventData eventData) => this.dragging = false;
}
