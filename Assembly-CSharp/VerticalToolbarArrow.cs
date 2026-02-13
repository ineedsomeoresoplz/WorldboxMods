// Decompiled with JetBrains decompiler
// Type: VerticalToolbarArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class VerticalToolbarArrow : ToolbarArrow
{
  public bool is_bottom = true;

  protected override void Update()
  {
    base.Update();
    float num = iTween.easeInOutCirc(0.0f, this.hide_position.y, this.timer);
    if ((double) this.arrow_transform.localPosition.y == (double) num)
      return;
    this.arrow_transform.localPosition = new Vector3(0.0f, num);
  }

  protected override float getScrollPosition() => this.scroll_rect.verticalNormalizedPosition;

  protected override void setScrollPosition(float pValue)
  {
    this.scroll_rect.verticalNormalizedPosition = pValue;
  }

  protected override float getEndPosition()
  {
    float scrollPosition = this.getScrollPosition();
    double num1 = (double) Screen.height / (double) CanvasMain.instance.canvas_ui.scaleFactor;
    Rect rect = this.scroll_rect.content.rect;
    double height = (double) ((Rect) ref rect).height;
    float num2 = (float) (num1 / height);
    return !this.is_bottom ? scrollPosition + Mathf.Min(num2, 0.5f) : scrollPosition - Mathf.Min(num2, 0.5f);
  }

  protected override void onScroll(Vector2 pVal)
  {
    float num = (float) Screen.height / CanvasMain.instance.canvas_ui.scaleFactor;
    this.should_show = true;
    Rect rect = this.scroll_rect.content.rect;
    if ((double) ((Rect) ref rect).height < (double) num)
      this.should_show = false;
    else if (this.is_bottom)
    {
      if ((double) this.getScrollPosition() > 0.10000000149011612)
        this.should_show = true;
      else
        this.should_show = false;
    }
    else if ((double) this.getScrollPosition() == 1.0)
      this.should_show = false;
    else if ((double) this.getScrollPosition() < 0.98000001907348633)
      this.should_show = true;
    else
      this.should_show = false;
  }
}
