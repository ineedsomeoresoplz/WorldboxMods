// Decompiled with JetBrains decompiler
// Type: DebugUiDrag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class DebugUiDrag : EventTrigger
{
  private bool dragging;
  private Transform mainTransform;
  private Transform canvasContainer;
  private DebugTool _tool;
  private Canvas _canvas;

  private void Start()
  {
    this._tool = ((Component) ((Component) this).transform).GetComponentInParent<DebugTool>();
    this._canvas = ((Component) ((Component) this).transform).GetComponentInParent<Canvas>();
    this.mainTransform = ((Component) this._tool).transform;
    this.canvasContainer = this.mainTransform.parent;
  }

  public void Update()
  {
    if (!this.dragging)
      return;
    Vector3 vector3 = Vector2.op_Implicit(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    this.mainTransform.SetParent((Transform) null, true);
    this.mainTransform.SetParent(this.canvasContainer, true);
    Vector2 sizeDelta = ((Component) this._tool).GetComponent<RectTransform>().sizeDelta;
    vector3.x += (float) ((double) sizeDelta.x / 2.0 - 75.0);
    vector3.y += 20f;
    this.mainTransform.position = vector3;
  }

  public virtual void OnPointerDown(PointerEventData eventData) => this.dragging = true;

  public virtual void OnPointerUp(PointerEventData eventData) => this.dragging = false;
}
