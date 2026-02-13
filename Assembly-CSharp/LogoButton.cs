// Decompiled with JetBrains decompiler
// Type: LogoButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LogoButton : MonoBehaviour
{
  private List<UiCreature> listLetters;
  private float initScale = 1f;
  private Tweener tweener;

  private void Awake()
  {
    this.initScale = ((Component) this).transform.localScale.x;
    this.loadLetters();
  }

  private void loadLetters()
  {
    this.listLetters = new List<UiCreature>();
    Transform transform = ((Component) ((Component) this).transform.FindRecursive("Letters")).transform;
    int childCount = transform.childCount;
    for (int index = 0; index < childCount; ++index)
    {
      UiCreature component = ((Component) transform.GetChild(index)).GetComponent<UiCreature>();
      if (component.dropped)
        component.resetPosition();
      this.listLetters.Add(component);
    }
  }

  private void letterFall()
  {
    if (this.listLetters.Count == 0)
    {
      this.loadLetters();
      AchievementLibrary.destroy_worldbox.check();
    }
    else
    {
      this.listLetters.ShuffleOne<UiCreature>();
      UiCreature listLetter = this.listLetters[0];
      this.listLetters.RemoveAt(0);
      listLetter.click();
    }
  }

  public void clickLogo()
  {
    MusicBox.playSound("event:/SFX/EXPLOSIONS/ExplosionHuge");
    if (this.tweener != null && ((Tween) this.tweener).active)
      TweenExtensions.Kill((Tween) this.tweener, false);
    float num1 = this.initScale * 1.2f;
    if (this.listLetters.Count == 0)
    {
      float num2 = 1.6f;
      ((Component) this).transform.localScale = new Vector3(num2, num2, num2);
      this.tweener = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, new Vector3(this.initScale, this.initScale, this.initScale), 0.3f), (Ease) 27);
    }
    else
    {
      ((Component) this).transform.localScale = new Vector3(num1, num1, num1);
      this.tweener = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, new Vector3(this.initScale, this.initScale, this.initScale), 0.3f), (Ease) 27);
    }
    this.letterFall();
  }
}
