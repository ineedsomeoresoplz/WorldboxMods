// Decompiled with JetBrains decompiler
// Type: HorizontalToolbarArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HorizontalToolbarArrow : ToolbarArrow
{
  public bool is_left = true;

  protected override void Update()
  {
    base.Update();
    float num = iTween.easeInOutCirc(0.0f, this.hide_position.x, this.timer);
    if ((double) this.arrow_transform.localPosition.x == (double) num)
      return;
    this.arrow_transform.localPosition = new Vector3(num, 0.0f);
  }

  protected override float getScrollPosition() => this.scroll_rect.horizontalNormalizedPosition;

  protected override void setScrollPosition(float pValue)
  {
    this.scroll_rect.horizontalNormalizedPosition = pValue;
  }

  protected override float getEndPosition()
  {
    float scrollPosition = this.getScrollPosition();
    double num1 = (double) Screen.width / (double) CanvasMain.instance.canvas_ui.scaleFactor;
    Rect rect = this.scroll_rect.content.rect;
    double width = (double) ((Rect) ref rect).width;
    float num2 = (float) (num1 / width);
    return !this.is_left ? scrollPosition + Mathf.Min(num2, 0.5f) : scrollPosition - Mathf.Min(num2, 0.5f);
  }

  protected override void onScroll(Vector2 pVal)
  {
    float num = (float) Screen.width / CanvasMain.instance.canvas_ui.scaleFactor;
    this.should_show = true;
    Rect rect = this.scroll_rect.content.rect;
    if ((double) ((Rect) ref rect).width < (double) num)
      this.should_show = false;
    else if (this.is_left)
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
