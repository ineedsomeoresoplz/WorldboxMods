// Decompiled with JetBrains decompiler
// Type: NotchMover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NotchMover : MonoBehaviour
{
  private float originalTopPosition;
  private RectTransform rectTransform;
  private Canvas _canvas;

  private void Start()
  {
    this.rectTransform = ((Component) this).GetComponent<RectTransform>();
    this.originalTopPosition = this.rectTransform.anchoredPosition.y;
    this._canvas = ((Component) ((Component) this).gameObject.transform).GetComponentInParent<Canvas>();
  }

  private void Update()
  {
    double height1 = (double) Screen.height;
    Rect safeArea1 = Screen.safeArea;
    double height2 = (double) ((Rect) ref safeArea1).height;
    if (height1 == height2 || Object.op_Equality((Object) this._canvas, (Object) null))
      return;
    double height3 = (double) Screen.height;
    Rect safeArea2 = Screen.safeArea;
    double height4 = (double) ((Rect) ref safeArea2).height;
    this.rectTransform.anchoredPosition = Vector2.op_Implicit(new Vector3(this.rectTransform.anchoredPosition.x, this.originalTopPosition - (float) (height3 - height4) / this._canvas.scaleFactor));
  }
}
