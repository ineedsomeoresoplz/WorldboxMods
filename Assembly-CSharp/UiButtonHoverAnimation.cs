// Decompiled with JetBrains decompiler
// Type: UiButtonHoverAnimation
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
[DisallowMultipleComponent]
public class UiButtonHoverAnimation : MonoBehaviour
{
  private Button button;
  public Vector3 default_scale;
  public float scale_size = 1.1f;
  public static float scaleTime = 0.1f;

  private void Awake()
  {
    this.button = ((Component) this).GetComponent<Button>();
    this.default_scale = ((Component) this).gameObject.transform.localScale;
  }

  private void Start()
  {
    // ISSUE: method pointer
    this.button.OnHover(new UnityAction((object) this, __methodptr(startAnim)));
  }

  private void startAnim()
  {
    float num = this.default_scale.x * this.scale_size;
    ((Component) this).transform.localScale = new Vector3(num, num, num);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, this.default_scale, UiButtonHoverAnimation.scaleTime), (Ease) 26);
  }

  private void OnDestroy()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
  }
}
