// Decompiled with JetBrains decompiler
// Type: ButtonAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class ButtonAnimation : MonoBehaviour
{
  public static float scaleTime = 0.1f;

  private IEnumerator newAnim()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ButtonAnimation buttonAnimation = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) buttonAnimation).gameObject.transform, 1f, ButtonAnimation.scaleTime), (Ease) 28);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ((Component) buttonAnimation).gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) CoroutineHelper.wait_for_0_01_s;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void clickAnimation()
  {
    if (!((Component) this).gameObject.activeSelf)
      return;
    this.StartCoroutine(this.newAnim());
  }
}
