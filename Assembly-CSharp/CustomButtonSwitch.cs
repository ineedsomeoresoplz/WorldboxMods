// Decompiled with JetBrains decompiler
// Type: CustomButtonSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class CustomButtonSwitch : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
  public Action click_increase;
  public Action click_decrease;
  private Animator anim;
  private Vector3 defaultScale;
  private Vector3 clickedScale;

  private void Start() => this.anim = ((Component) this).gameObject.GetComponent<Animator>();

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button == 1)
    {
      Action clickDecrease = this.click_decrease;
      if (clickDecrease != null)
        clickDecrease();
      SoundBox.click();
      this.newClickAnimation();
    }
    else
    {
      Action clickIncrease = this.click_increase;
      if (clickIncrease != null)
        clickIncrease();
      SoundBox.click();
      this.newClickAnimation();
    }
  }

  private void Awake()
  {
    this.defaultScale = ((Component) this).transform.localScale;
    this.clickedScale = Vector3.op_Multiply(this.defaultScale, 1.1f);
  }

  public void newClickAnimation()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    ((Component) this).transform.localScale = this.clickedScale;
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, this.defaultScale, 0.3f), (Ease) 28);
  }

  private void OnDestroy()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
  }
}
