// Decompiled with JetBrains decompiler
// Type: PopulationPyramidItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class PopulationPyramidItem : MonoBehaviour
{
  [SerializeField]
  private RectTransform _mask;
  [SerializeField]
  private RectTransform _bar;
  [SerializeField]
  private Image _bar_image;
  [SerializeField]
  private Text _count_text;
  [SerializeField]
  private float _bar_width = 80f;
  [SerializeField]
  private int _count;
  [SerializeField]
  private int _max_count;
  [SerializeField]
  private float _percent;
  [SerializeField]
  private float _calc_percent;
  private Tweener _cur_tween;

  private void Awake() => this.resetBar();

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).gameObject.AddOrGetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(animateBar)));
  }

  internal void setCount(int pCount, int pMax)
  {
    this._count_text.text = pCount.ToString();
    Color color = ((Graphic) this._count_text).color;
    color.a = pCount != 0 ? 1f : 0.5f;
    ((Graphic) this._count_text).color = color;
    this._count = pCount;
    this._max_count = pMax;
    this.animateBar();
  }

  internal int getCount() => this._count;

  private void resetBar()
  {
    this.checkDestroyTween();
    this._bar.sizeDelta = new Vector2(0.1f, this._bar.sizeDelta.y);
  }

  internal void setOpacity(float pOpacity)
  {
    Color color = ((Graphic) this._bar_image).color;
    color.a = pOpacity;
    ((Graphic) this._bar_image).color = color;
  }

  internal void animateBar()
  {
    this.resetBar();
    this._percent = (float) this._count / (float) this._max_count;
    this._calc_percent = this._count <= 0 ? 0.0f : 4f + Mathf.Floor(this._percent * this._bar_width);
    this._cur_tween = (Tweener) DOTweenModuleUI.DOSizeDelta(this._bar, new Vector2(this._calc_percent, this._bar.sizeDelta.y), 0.3f, false);
  }

  private void OnDisable() => this.checkDestroyTween();

  private void checkDestroyTween()
  {
    if (TweenExtensions.IsActive((Tween) this._cur_tween))
    {
      TweenExtensions.Complete((Tween) this._cur_tween, false);
      TweenExtensions.Kill((Tween) this._cur_tween, false);
    }
    this._cur_tween = (Tweener) null;
  }
}
