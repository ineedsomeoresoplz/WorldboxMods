// Decompiled with JetBrains decompiler
// Type: RainSwitcherButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RainSwitcherButton : MonoBehaviour
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _background;
  [SerializeField]
  private Button _button;
  [SerializeField]
  private Sprite _enabled;
  [SerializeField]
  private Sprite _disabled;

  public void toggleState(bool pState)
  {
    if (pState)
    {
      this._background.sprite = this._enabled;
      ((Graphic) this._icon).color = ColorStyleLibrary.m.favorite_selected;
    }
    else
    {
      this._background.sprite = this._disabled;
      ((Graphic) this._icon).color = ColorStyleLibrary.m.favorite_not_selected;
    }
  }

  public Button getButton() => this._button;
}
