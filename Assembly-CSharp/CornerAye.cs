// Decompiled with JetBrains decompiler
// Type: CornerAye
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class CornerAye : MonoBehaviour
{
  public static CornerAye instance;
  public Transform sprite;
  private RectTransform _rect;

  private void Awake()
  {
    this._rect = ((Component) this.sprite).GetComponent<RectTransform>();
    this.reset();
  }

  private void reset()
  {
    this._rect.anchoredPosition = new Vector2(100f, 0.0f);
    ShortcutExtensions.DOKill((Component) ((Component) this.sprite).transform, false);
  }

  private void Start() => CornerAye.instance = this;

  public void startAye()
  {
    this.reset();
    ((Tween) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(((Component) this.sprite).transform, new Vector3(), 0.3f, false), (Ease) 27)).onComplete = new TweenCallback(this.moveBack);
  }

  private void moveBack()
  {
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(100f, 0.0f);
    float num = 0.3f;
    TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(((Component) this.sprite).transform, vector3, num, false), (Ease) 7), 0.1f);
  }
}
