// Decompiled with JetBrains decompiler
// Type: VertArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class VertArrow : MonoBehaviour
{
  public Image arrow;
  private Transform _arrow_transform;
  public Vector3 hidPos;
  public bool isLeft = true;
  public ScrollRectExtended scrollRect;
  public RectTransform contentContainer;
  private float timer;
  private bool shouldShow = true;
  private Button button;
  private Tweener _tweener;

  private void Awake()
  {
    this._arrow_transform = ((Component) this.arrow).transform;
    // ISSUE: method pointer
    this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>((object) this, __methodptr(onScroll)));
    this.button = ((Component) this.arrow).GetComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(scrollTab)));
  }

  private void onScroll(Vector2 pVal)
  {
    this.shouldShow = true;
    Rect rect = this.contentContainer.rect;
    double width1 = (double) ((Rect) ref rect).width;
    rect = this.scrollRect.rectTransform.rect;
    double width2 = (double) ((Rect) ref rect).width;
    if (width1 < width2)
      this.shouldShow = false;
    else if (this.isLeft)
    {
      if ((double) this.scrollRect.horizontalNormalizedPosition > 0.10000000149011612)
        this.shouldShow = true;
      else
        this.shouldShow = false;
    }
    else if ((double) this.scrollRect.horizontalNormalizedPosition == 1.0)
      this.shouldShow = false;
    else if ((double) this.scrollRect.horizontalNormalizedPosition < 0.98000001907348633)
      this.shouldShow = true;
    else
      this.shouldShow = false;
  }

  private void Update()
  {
    if (!this.shouldShow)
      this.timer += Time.deltaTime * 2f;
    else
      this.timer -= Time.deltaTime * 2f;
    this.timer = Mathf.Clamp(this.timer, 0.0f, 1f);
    float num = iTween.easeInOutCirc(0.0f, this.hidPos.x, this.timer);
    if ((double) this._arrow_transform.localPosition.x == (double) num)
      return;
    this._arrow_transform.localPosition = new Vector3(num, 0.0f);
  }

  private void scrollTab()
  {
    float normalizedPosition = this.scrollRect.horizontalNormalizedPosition;
    Rect rect = this.scrollRect.rectTransform.rect;
    double width1 = (double) ((Rect) ref rect).width;
    rect = this.scrollRect.content.rect;
    double width2 = (double) ((Rect) ref rect).width;
    float num1 = (float) (width1 / width2);
    float num2 = !this.isLeft ? normalizedPosition + Mathf.Min(num1, 0.5f) : normalizedPosition - Mathf.Min(num1, 0.5f);
    TweenExtensions.Kill((Tween) this._tweener, false);
    // ISSUE: method pointer
    // ISSUE: method pointer
    this._tweener = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object) this, __methodptr(\u003CscrollTab\u003Eb__13_0)), new DOSetter<float>((object) this, __methodptr(\u003CscrollTab\u003Eb__13_1)), num2, 0.3f), (Ease) 22);
  }

  private void OnDisable() => TweenExtensions.Kill((Tween) this._tweener, true);
}
