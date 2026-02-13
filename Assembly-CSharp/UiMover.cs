// Decompiled with JetBrains decompiler
// Type: UiMover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class UiMover : MonoBehaviour
{
  public bool onVisible;
  public Vector3 initPos;
  public Vector3 hidePos;
  public bool visible;
  public bool initInitPos = true;
  private Tweener _tweener;

  private void Awake()
  {
    if (!this.initInitPos)
      return;
    this.initPos = ((Component) this).gameObject.transform.localPosition;
  }

  public void setVisible(bool pVisible, bool pNow = false, TweenCallback pCompleteCallback = null)
  {
    this.visible = pVisible;
    if (pNow)
    {
      if (pVisible)
        ((Component) this).gameObject.transform.localPosition = this.initPos;
      else
        ((Component) this).gameObject.transform.localPosition = this.hidePos;
    }
    else if (this.visible)
    {
      if (this.onVisible)
        return;
      this.onVisible = true;
      this.moveTween(this.initPos, pCompleteCallback);
    }
    else
    {
      if (!this.onVisible)
        return;
      this.onVisible = false;
      this.moveTween(this.hidePos, pCompleteCallback);
    }
  }

  protected void moveTween(Vector3 pVecPos, TweenCallback pCompleteCallback = null)
  {
    float num = 0.35f;
    TweenExtensions.Kill((Tween) this._tweener, true);
    this._tweener = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(((Component) this).transform, pVecPos, num, false), 0.02f), (Ease) 10), pCompleteCallback);
  }
}
