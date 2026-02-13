// Decompiled with JetBrains decompiler
// Type: IconRotationAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class IconRotationAnimation : MonoBehaviour
{
  public float delay = 5f;
  public bool randomDelay;
  private Vector3 initScale;
  private Vector3 scaleTo;
  internal Tweener curTween;

  private void Awake()
  {
    this.initScale = ((Component) this).transform.localScale;
    this.scaleTo = Vector3.op_Multiply(this.initScale, 1.1f);
    if (!this.randomDelay)
      return;
    this.delay = Randy.randomFloat(1f, 10f);
  }

  private void checkDestroyTween()
  {
    if (this.curTween == null || !((Tween) this.curTween).active)
      return;
    TweenExtensions.Complete((Tween) this.curTween, false);
    TweenExtensions.Kill((Tween) this.curTween, false);
    this.curTween = (Tweener) null;
  }

  private void rotate1()
  {
    if (Object.op_Equality((Object) ((Component) this).transform, (Object) null))
      return;
    this.curTween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, this.scaleTo, 0.3f), this.delay), (Ease) 28), new TweenCallback(this.rotate2));
  }

  private void rotate2()
  {
    if (Object.op_Equality((Object) ((Component) this).transform, (Object) null))
      return;
    if (this.randomDelay)
      this.delay = Randy.randomFloat(1f, 10f);
    this.curTween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, this.initScale, 0.3f), 0.0f), (Ease) 28), new TweenCallback(this.rotate1));
  }

  private void OnEnable()
  {
    this.checkDestroyTween();
    this.rotate1();
  }

  private void OnDisable() => this.checkDestroyTween();

  private void OnDestroy() => this.checkDestroyTween();
}
