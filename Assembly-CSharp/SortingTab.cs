// Decompiled with JetBrains decompiler
// Type: SortingTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SortingTab : MonoBehaviour
{
  public bool scrollable;
  private readonly List<SortButtonContainer> _buttons = new List<SortButtonContainer>();
  private SortButton _current_sort_button;

  public SortButton getCurrentButton() => this._current_sort_button;

  public void clearButtons()
  {
    foreach (Component button in this._buttons)
      button.gameObject.SetActive(false);
  }

  public SortButton tryAddButton(
    string pIcon,
    string pTooltip,
    SortButtonAction pShowAction,
    SortButtonAction pAction)
  {
    return this.switchButton(pTooltip, true) ? (SortButton) null : this.addButton(pIcon, pTooltip, pShowAction, pAction);
  }

  public bool switchButton(string pTooltip, bool pEnabled)
  {
    foreach (SortButtonContainer button in this._buttons)
    {
      if (((Object) ((Component) button).gameObject).name == pTooltip)
      {
        ((Component) button).gameObject.SetActive(pEnabled);
        return true;
      }
    }
    return false;
  }

  public SortButton addButton(
    string pIcon,
    string pTooltip,
    SortButtonAction pShowAction,
    SortButtonAction pAction)
  {
    SortButtonContainer sortButtonContainer = Object.Instantiate<SortButtonContainer>(Resources.Load<SortButtonContainer>("ui/SortButtonGeneric"), ((Component) this).transform);
    SortButton componentInChildren = ((Component) sortButtonContainer).GetComponentInChildren<SortButton>();
    PowerButton component = ((Component) componentInChildren).GetComponent<PowerButton>();
    component.icon.sprite = SpriteTextureLoader.getSprite(pIcon);
    ((Component) component).GetComponent<TipButton>().textOnClick = pTooltip;
    componentInChildren.icon = component.icon;
    componentInChildren.select_action = new SortButtonClearAction(this.selectAction);
    componentInChildren.action = pAction;
    componentInChildren.post_action = pShowAction;
    ((Object) ((Component) componentInChildren).gameObject).name = pTooltip;
    ((Object) ((Component) sortButtonContainer).gameObject).name = pTooltip;
    this._buttons.Add(sortButtonContainer);
    if (this.scrollable)
      ((Component) componentInChildren).gameObject.AddComponent<ScrollableButton>();
    return componentInChildren;
  }

  private void selectAction(SortButton pButton)
  {
    foreach (SortButtonContainer button in this._buttons)
    {
      if (!Object.op_Equality((Object) button.sort_button, (Object) pButton))
        button.sort_button.turnOff();
    }
    this._current_sort_button = pButton;
  }

  internal void enableFirstIfNone()
  {
    if (this._buttons.Count == 0)
      return;
    foreach (SortButtonContainer button in this._buttons)
    {
      if (((Component) button).gameObject.activeSelf && Object.op_Equality((Object) button.sort_button, (Object) this._current_sort_button))
        return;
    }
    this.selectAction((SortButton) null);
    foreach (SortButtonContainer button in this._buttons)
    {
      if (((Component) button).gameObject.activeSelf)
      {
        button.sort_button.click();
        break;
      }
    }
  }
}
