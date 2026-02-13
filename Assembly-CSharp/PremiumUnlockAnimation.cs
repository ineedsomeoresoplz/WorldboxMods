// Decompiled with JetBrains decompiler
// Type: PremiumUnlockAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
public class PremiumUnlockAnimation : MonoBehaviour
{
  public float time;
  public GameObject circleFX;
  public GameObject shineFX;
  public GameObject aye;
  private CanvasGroup canvasGroup;
  public float fadeDelay;
  private int index;
  public Vector3 scaleAdd;
  public static float scaleTime = 1f;
  public static float delayTime = 0.5f;

  private void Awake() => this.aye.transform.localScale = new Vector3(1f, 0.0f, 1f);

  private void Start()
  {
    this.canvasGroup = this.shineFX.GetComponent<CanvasGroup>();
    this.circleFX.SetActive(true);
    TweenSettingsExtensions.SetLoops<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this.circleFX.transform, Vector3.one, PremiumUnlockAnimation.scaleTime), -1, (LoopType) 1);
    TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(this.aye.transform, Vector3.one, PremiumUnlockAnimation.scaleTime), (Ease) 24), PremiumUnlockAnimation.delayTime);
  }

  private void Update()
  {
    this.canvasGroup.alpha += Time.deltaTime / this.fadeDelay;
    this.shineFX.transform.Rotate(new Vector3(0.0f, 0.0f, 1f));
  }

  public void clickClose()
  {
    this.circleFX.gameObject.SetActive(false);
    this.shineFX.gameObject.SetActive(false);
  }
}
