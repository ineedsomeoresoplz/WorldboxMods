// Decompiled with JetBrains decompiler
// Type: ToggleButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ToggleButton : MonoBehaviour
{
  [SerializeField]
  private Image _background;
  private ToggleButtonSelectAction _action;
  private ToggleButtonAction _post_action;
  private static Sprite _sprite_on;
  private static Sprite _sprite_off;
  public bool is_on;

  private void Awake()
  {
    if (Object.op_Equality((Object) ToggleButton._sprite_on, (Object) null))
    {
      ToggleButton._sprite_on = SpriteTextureLoader.getSprite("ui/tab_button_sort_selected");
      ToggleButton._sprite_off = SpriteTextureLoader.getSprite("ui/tab_button_sort");
    }
    this._background.sprite = ToggleButton._sprite_off;
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
  }

  public void init(
    string pIcon,
    string pTooltip,
    ToggleButtonSelectAction pAction,
    ToggleButtonAction pShowAction)
  {
    PowerButton component = ((Component) this).GetComponent<PowerButton>();
    component.icon.sprite = SpriteTextureLoader.getSprite(pIcon);
    ((Component) component).GetComponent<TipButton>().textOnClick = pTooltip;
    this._action = pAction;
    this._post_action = pShowAction;
    ((Object) ((Component) this).gameObject).name = pTooltip;
  }

  public void click()
  {
    this.is_on = !this.is_on;
    this.checkSprite();
    ToggleButtonSelectAction action = this._action;
    if (action != null)
      action(this);
    ToggleButtonAction postAction = this._post_action;
    if (postAction == null)
      return;
    postAction();
  }

  private void checkSprite()
  {
    this._background.sprite = this.is_on ? ToggleButton._sprite_on : ToggleButton._sprite_off;
  }
}
