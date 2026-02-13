// Decompiled with JetBrains decompiler
// Type: StatBar
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
public class StatBar : MonoBehaviour
{
  public Text textField;
  public RectTransform mask;
  public RectTransform bar;
  private Tweener _bar_tween;
  private Tweener _text_tween;
  private float _val;
  private float _max = 100f;
  private string _ending;
  private bool _float;
  public StatBarUpdated _bar_updated_action;

  private void OnEnable() => this.restartBar();

  private void Start()
  {
    this.restartBar();
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).gameObject.AddOrGetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(restartBar)));
  }

  private void restartBar() => this.setBar(this._val, this._max, this._ending, pFloat: this._float);

  private void resetSize()
  {
    Rect rect = this.mask.rect;
    this.bar.sizeDelta = new Vector2(Mathf.Floor((float) (0.0099999997764825821 * (double) ((Rect) ref rect).width)), this.bar.sizeDelta.y);
    this.textField.text = "0";
  }

  public void resetTween()
  {
    this.checkDestroyTween(false);
    this._val = 0.0f;
    this.resetSize();
  }

  private void OnRectTransformDimensionsChange() => this.restartBar();

  public void setBar(
    float pVal,
    float pMax,
    string pEnding,
    bool pReset = true,
    bool pFloat = false,
    bool pUpdateText = true,
    float pSpeed = 0.3f)
  {
    if ((double) pMax == 0.0)
    {
      this.resetTween();
      this.textField.text = "-";
    }
    else
    {
      if (!pReset && (double) pVal == (double) this._val && (double) pMax == (double) this._max && pEnding == this._ending)
        return;
      if (pReset)
        this.resetTween();
      this._max = pMax;
      this._ending = pEnding;
      this._float = pFloat;
      this.checkDestroyTween(false);
      if (pReset)
      {
        this._bar_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector2, Vector2, VectorOptions>>(DOTweenModuleUI.DOSizeDelta(this.bar, new Vector2(0.0f, this.bar.sizeDelta.y), 0.005f, false), (TweenCallback) (() =>
        {
          double num = (double) pVal / (double) pMax;
          Rect rect = this.mask.rect;
          double width = (double) ((Rect) ref rect).width;
          this._bar_tween = (Tweener) DOTweenModuleUI.DOSizeDelta(this.bar, new Vector2(Mathf.Floor((float) (num * width)), this.bar.sizeDelta.y), pSpeed, false);
        }));
      }
      else
      {
        double num = (double) pVal / (double) pMax;
        Rect rect = this.mask.rect;
        double width = (double) ((Rect) ref rect).width;
        this._bar_tween = (Tweener) DOTweenModuleUI.DOSizeDelta(this.bar, new Vector2(Mathf.Floor((float) (num * width)), this.bar.sizeDelta.y), pSpeed, false);
      }
      if (pUpdateText)
        this._text_tween = !pFloat ? (Tweener) this.textField.DOUpCounter((int) this._val, (int) pVal, pSpeed, pEnding) : (Tweener) this.textField.DOUpCounter(this._val, pVal, pSpeed, pEnding);
      this._val = pVal;
      StatBarUpdated barUpdatedAction = this._bar_updated_action;
      if (barUpdatedAction == null)
        return;
      barUpdatedAction(pVal, pMax);
    }
  }

  private void OnDisable() => this.checkDestroyTween();

  private void checkDestroyTween(bool pComplete = true)
  {
    if (TweenExtensions.IsActive((Tween) this._bar_tween))
    {
      TweenExtensions.Complete((Tween) this._bar_tween, pComplete);
      TweenExtensions.Kill((Tween) this._bar_tween, pComplete);
      this._bar_tween = (Tweener) null;
    }
    if (!TweenExtensions.IsActive((Tween) this._text_tween))
      return;
    TweenExtensions.Complete((Tween) this._text_tween, pComplete);
    TweenExtensions.Kill((Tween) this._text_tween, pComplete);
    this._text_tween = (Tweener) null;
  }

  public void addCallback(StatBarUpdated pAction) => this._bar_updated_action += pAction;

  public void removeCallback(StatBarUpdated pAction) => this._bar_updated_action -= pAction;
}
