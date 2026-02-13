// Decompiled with JetBrains decompiler
// Type: TweenedColoredText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (Text))]
public class TweenedColoredText : MonoBehaviour
{
  public Color color1 = Color.blue;
  public Color color2 = Color.red;
  public float duration = 1f;
  private Text _text;

  private void Awake() => this._text = ((Component) this).GetComponent<Text>();

  private void OnEnable()
  {
    ShortcutExtensions.DOKill((Component) this._text, true);
    ((Graphic) this._text).color = this.color1;
    TweenSettingsExtensions.SetEase<TweenerCore<Color, Color, ColorOptions>>(TweenSettingsExtensions.SetLoops<TweenerCore<Color, Color, ColorOptions>>(DOTweenModuleUI.DOColor(this._text, this.color2, this.duration), -1, (LoopType) 1), (Ease) 4);
  }
}
