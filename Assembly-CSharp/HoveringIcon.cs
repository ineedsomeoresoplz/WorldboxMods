// Decompiled with JetBrains decompiler
// Type: HoveringIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HoveringIcon : MonoBehaviour
{
  private Vector3 _original_pos;
  private float _random_timer;
  public Image image;
  public float min = -2f;
  public float max = 2f;
  public float timer_mod = 1f;
  private Tweener _tweener;
  internal RectTransform rect;

  private void Awake()
  {
    this.rect = ((Component) this).GetComponent<RectTransform>();
    this.image = ((Component) this).GetComponent<Image>();
  }

  internal void clear() => TweenExtensions.Kill((Tween) this._tweener, false);

  internal void init()
  {
    this._original_pos = ((Component) this).transform.localPosition;
    this._random_timer = Randy.randomFloat(1f * this.timer_mod, 1.5f * this.timer_mod);
    this.startAnimation();
  }

  private void OnDisable() => this.clear();

  private void startAnimation()
  {
    TweenExtensions.Kill((Tween) this._tweener, false);
    ((Component) this).transform.localPosition = new Vector3(this._original_pos.x, this._original_pos.y += Randy.randomFloat(this.min, this.max));
    if (Randy.randomBool())
      this.moveStageOne();
    else
      this.moveStageTwo();
  }

  private void moveStageTwo()
  {
    this._tweener = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(((Component) this).transform, this._original_pos, this._random_timer, false), (Ease) 7), new TweenCallback(this.moveStageOne));
  }

  private void moveStageOne()
  {
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(this._original_pos.x, this._original_pos.y, 1f);
    vector3.y += 3f;
    this._tweener = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(((Component) this).transform, vector3, this._random_timer, false), (Ease) 7), new TweenCallback(this.moveStageTwo));
  }
}
