// Decompiled with JetBrains decompiler
// Type: TabTogglesGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TabTogglesGroup : MonoBehaviour
{
  private readonly List<TabToggleContainer> _toggles = new List<TabToggleContainer>();
  private TabToggle _current_toggle;

  public TabToggle getCurrentButton() => this._current_toggle;

  public void clearButtons()
  {
    foreach (Component toggle in this._toggles)
      toggle.gameObject.SetActive(false);
  }

  public void tryAddButton(
    string pIcon,
    string pTooltip,
    TabToggleAction pShowAction,
    TabToggleAction pAction)
  {
    if (this.switchButton(pTooltip, true))
      return;
    this.addButton(pIcon, pTooltip, pShowAction, pAction);
  }

  public bool switchButton(string pTooltip, bool pEnabled)
  {
    foreach (TabToggleContainer toggle in this._toggles)
    {
      if (((Object) ((Component) toggle).gameObject).name == pTooltip)
      {
        ((Component) toggle).gameObject.SetActive(pEnabled);
        return true;
      }
    }
    return false;
  }

  public void addButton(
    string pIcon,
    string pTooltip,
    TabToggleAction pShowAction,
    TabToggleAction pAction)
  {
    TabToggleContainer tabToggleContainer = Object.Instantiate<TabToggleContainer>(Resources.Load<TabToggleContainer>("ui/TabToggleGeneric"), ((Component) this).transform);
    TabToggle componentInChildren = ((Component) tabToggleContainer).GetComponentInChildren<TabToggle>();
    PowerButton component = ((Component) componentInChildren).GetComponent<PowerButton>();
    component.icon.sprite = SpriteTextureLoader.getSprite(pIcon);
    ((Component) component).GetComponent<TipButton>().textOnClick = pTooltip;
    ((Component) component).GetComponent<TipButton>().textOnClickDescription = pTooltip + "_description";
    componentInChildren.icon = component.icon;
    componentInChildren.select_action = new TabToggleClearAction(this.selectAction);
    componentInChildren.action = pAction;
    componentInChildren.post_action = pShowAction;
    ((Object) ((Component) componentInChildren).gameObject).name = pTooltip;
    ((Object) ((Component) tabToggleContainer).gameObject).name = pTooltip;
    this._toggles.Add(tabToggleContainer);
  }

  private void selectAction(TabToggle pToggle)
  {
    foreach (TabToggleContainer toggle in this._toggles)
    {
      if (!Object.op_Equality((Object) toggle.toggle, (Object) pToggle))
        toggle.toggle.unselect();
    }
    this._current_toggle = pToggle;
  }

  internal void enableFirst()
  {
    if (this._toggles.Count == 0)
      return;
    this.selectAction((TabToggle) null);
    foreach (TabToggleContainer toggle in this._toggles)
    {
      if (((Component) toggle).gameObject.activeSelf)
      {
        toggle.toggle.click();
        break;
      }
    }
  }
}
