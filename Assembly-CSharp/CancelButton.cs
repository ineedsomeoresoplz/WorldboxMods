// Decompiled with JetBrains decompiler
// Type: CancelButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CancelButton : MonoBehaviour
{
  public Image powerIcon;
  public bool goUp;
  public bool goDown;
  private bool _going_down;
  private bool _going_up;
  private RectTransform _rect;
  private float _timer;
  private const float Y_TOP_TARGET = 90f;

  private void Awake() => this._rect = ((Component) this).GetComponent<RectTransform>();

  public void setIconFrom(PowerButton pButton)
  {
    if (pButton.godPower == null || Object.op_Equality((Object) pButton.icon, (Object) null))
      return;
    this.powerIcon.sprite = pButton.icon.sprite;
  }

  private void Update()
  {
    if (this.goDown != this._going_down)
    {
      this._going_down = this.goDown;
      this._timer = 0.0f;
      if (this.goDown)
        this._timer = 0.95f;
    }
    if (this.goUp != this._going_up)
    {
      this._going_up = this.goUp;
      this._timer = -1f;
    }
    if ((double) this._timer >= 1.0)
      return;
    this._timer += Time.deltaTime / 2f;
    this._timer = Mathf.Clamp(this._timer, 0.0f, 1f);
    this._rect.anchoredPosition = Vector2.op_Implicit(new Vector3(this._rect.anchoredPosition.x, !this._going_down ? (!this._going_up ? iTween.easeInOutCirc(this._rect.anchoredPosition.y, 0.0f, this._timer) : iTween.easeInQuart(0.0f, 90f, this._timer)) : iTween.easeInOutCirc(0.0f, -90f, this._timer)));
  }
}
