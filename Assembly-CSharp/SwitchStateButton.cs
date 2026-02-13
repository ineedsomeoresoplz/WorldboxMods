// Decompiled with JetBrains decompiler
// Type: SwitchStateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SwitchStateButton : MonoBehaviour
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _background;
  [SerializeField]
  private Button _button;
  [SerializeField]
  private Sprite _sprite_enabled;
  [SerializeField]
  private Sprite _sprite_disabled;
  private bool _state = true;
  private PowerButton _power_button;

  private void Awake() => this._power_button = ((Component) this).GetComponent<PowerButton>();

  public void setState(bool pState)
  {
    this._state = pState;
    if (this._state)
    {
      this._background.sprite = this._sprite_enabled;
      ((Graphic) this._icon).color = Color.white;
    }
    else
    {
      this._background.sprite = this._sprite_disabled;
      ((Graphic) this._icon).color = Color32.op_Implicit(Toolbox.color_grey_dark);
    }
    ((Behaviour) this._button).enabled = this._state;
    if (!Object.op_Inequality((Object) this._power_button, (Object) null))
      return;
    this._power_button.is_selectable = this._state;
  }
}
