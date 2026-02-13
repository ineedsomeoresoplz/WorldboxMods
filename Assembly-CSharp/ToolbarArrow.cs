// Decompiled with JetBrains decompiler
// Type: ToolbarArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ToolbarArrow : MonoBehaviour
{
  protected const float POSITION_SCROLL_BOUND_BEGIN = 0.1f;
  protected const float POSITION_SCROLL_BOUND_END = 0.98f;
  protected const float POSITION_SCROLL_END = 1f;
  protected const float POSITION_SCROLL_MIN = 0.5f;
  private const float POSITION_UPDATE_SPEED = 2f;
  private const float TWEEN_DURATION = 0.3f;
  [SerializeField]
  private Image arrow;
  [SerializeField]
  protected Vector3 hide_position;
  [SerializeField]
  protected ScrollRectExtended scroll_rect;
  protected float timer;
  protected bool should_show = true;
  protected Transform arrow_transform;
  private Button _button;
  private Tweener _tweener;

  private void Awake()
  {
    this.arrow_transform = ((Component) this.arrow).transform;
    ScrollRectExtended.ScrollRectEvent onValueChanged = this.scroll_rect.onValueChanged;
    ToolbarArrow toolbarArrow = this;
    // ISSUE: virtual method pointer
    UnityAction<Vector2> unityAction = new UnityAction<Vector2>((object) toolbarArrow, __vmethodptr(toolbarArrow, onScroll));
    onValueChanged.AddListener(unityAction);
    this._button = ((Component) this.arrow).GetComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(scrollTab)));
  }

  protected virtual void onScroll(Vector2 pVal) => throw new NotImplementedException();

  protected virtual float getEndPosition() => throw new NotImplementedException();

  protected virtual float getScrollPosition() => throw new NotImplementedException();

  protected virtual void setScrollPosition(float pValue) => throw new NotImplementedException();

  protected virtual void Update()
  {
    if (!this.should_show)
      this.timer += Time.deltaTime * 2f;
    else
      this.timer -= Time.deltaTime * 2f;
    this.timer = Mathf.Clamp(this.timer, 0.0f, 1f);
  }

  private void scrollTab()
  {
    float endPosition = this.getEndPosition();
    TweenExtensions.Kill((Tween) this._tweener, false);
    // ISSUE: method pointer
    // ISSUE: method pointer
    this._tweener = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>((object) this, __methodptr(\u003CscrollTab\u003Eb__20_0)), new DOSetter<float>((object) this, __methodptr(\u003CscrollTab\u003Eb__20_1)), endPosition, 0.3f), (Ease) 22);
  }

  private void OnDisable() => TweenExtensions.Kill((Tween) this._tweener, true);
}
