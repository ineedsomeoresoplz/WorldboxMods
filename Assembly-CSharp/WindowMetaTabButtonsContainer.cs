// Decompiled with JetBrains decompiler
// Type: WindowMetaTabButtonsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WindowMetaTabButtonsContainer : MonoBehaviour
{
  private static Sprite _tab_button_on;
  private static Sprite _tab_button_off;
  public WindowMetaTab tab_default;
  private ScrollWindow _scroll_window;
  private List<WindowMetaTab> _tabs = new List<WindowMetaTab>();
  private readonly List<WindowMetaTab> _tabs_with_content = new List<WindowMetaTab>();
  private WindowMetaTab _tab_last;
  private TabShowAction _on_tab_show;
  private TabHideAction _on_tab_hide;
  private bool _initialized;

  private void Awake() => this.init();

  public void init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    if (Object.op_Equality((Object) WindowMetaTabButtonsContainer._tab_button_on, (Object) null))
    {
      WindowMetaTabButtonsContainer._tab_button_on = SpriteTextureLoader.getSprite("ui/tab_button_vertical_selected");
      WindowMetaTabButtonsContainer._tab_button_off = SpriteTextureLoader.getSprite("ui/tab_button_vertical");
    }
    if (this._tabs.Count > 0 && Object.op_Equality((Object) this.tab_default, (Object) null))
      this.tab_default = this._tabs[0];
    this._scroll_window = ((Component) ((Component) this).transform).GetComponentInParent<ScrollWindow>();
    WindowMetaTab[] componentsInChildren = ((Component) ((Component) this).transform).GetComponentsInChildren<WindowMetaTab>(true);
    if (componentsInChildren.Length == 0)
      return;
    foreach (WindowMetaTab windowMetaTab in componentsInChildren)
    {
      this._tabs.Add(windowMetaTab);
      windowMetaTab.container = this;
    }
    this.refillTabsWithContent();
  }

  private void OnEnable()
  {
    this.refillTabsWithContent();
    this.initialTabAction();
  }

  private void OnDisable() => this.hideAllTabContent();

  private void refillTabsWithContent()
  {
    this._tabs_with_content.Clear();
    foreach (WindowMetaTab tab in this._tabs)
    {
      if (((Component) tab).gameObject.activeSelf && tab.getState() && tab.tab_elements.Count != 0)
        this._tabs_with_content.Add(tab);
    }
  }

  public Transform addTabContent(WindowMetaTab pTabButton, Transform pContent)
  {
    string name = ((Object) pTabButton).name;
    if (!this._tabs.Contains(pTabButton))
    {
      Debug.LogError((object) $"[addTabContent] Tab {name} not found in window {((Object) this._scroll_window).name}");
      return (Transform) null;
    }
    if (Object.op_Equality((Object) pContent, (Object) null))
      return (Transform) null;
    pTabButton.tab_elements.Add(pContent);
    return pContent;
  }

  public Transform addTabContent(string pTabId, Transform pContent)
  {
    WindowMetaTab pTabButton = this._tabs.Find((Predicate<WindowMetaTab>) (c => ((Object) c).name == pTabId));
    if (!Object.op_Equality((Object) pTabButton, (Object) null))
      return this.addTabContent(pTabButton, pContent);
    Debug.LogError((object) $"[addTabContent] Tab {pTabId} not found in window {((Object) this._scroll_window).name}");
    return (Transform) null;
  }

  internal void removeTab(WindowMetaTab pTab)
  {
    this._tabs.Remove(pTab);
    if (!Object.op_Equality((Object) this._tab_last, (Object) pTab))
      return;
    this.startTabAction(this.tab_default);
  }

  public void initialTabAction()
  {
    if (Object.op_Equality((Object) this.tab_default, (Object) null))
      return;
    if (Object.op_Equality((Object) this._tab_last, (Object) null))
      this.tab_default.doAction();
    else
      this._tab_last.doAction();
  }

  public void showTab(WindowMetaTab pTabButton, bool pSkipActionIfSame = false)
  {
    this.startTabAction(pTabButton, pSkipActionIfSame);
  }

  public void showTab(WindowMetaTab pTabButton) => this.startTabAction(pTabButton);

  public void showTab(string pId)
  {
    foreach (WindowMetaTab tab in this._tabs)
    {
      if (!(((Object) tab).name != pId))
      {
        this.showTab(tab);
        tab.checkShowWorldTip();
        break;
      }
    }
  }

  public void startTabAction(WindowMetaTab pTab, bool pSkipIfSame = false)
  {
    if (pTab.destroyed || pSkipIfSame && this.isActiveTab(pTab))
      return;
    this._tab_last = pTab;
    this.hideAllTabContent();
    this.enableTab(this._tab_last);
    foreach (Transform tabElement in pTab.tab_elements)
    {
      if (!Object.op_Equality((Object) tabElement, (Object) null))
        ((Component) tabElement).gameObject.SetActive(true);
    }
    TabShowAction onTabShow = this._on_tab_show;
    if (onTabShow != null)
      onTabShow(this._tab_last);
    this._scroll_window.resetScroll();
  }

  public bool isActiveTab(WindowMetaTab pTab)
  {
    return Object.op_Equality((Object) this._tab_last, (Object) pTab);
  }

  protected void enableTab(WindowMetaTab pTabButton)
  {
    ((Component) pTabButton).GetComponent<Image>().sprite = WindowMetaTabButtonsContainer._tab_button_on;
  }

  public void hideAllTabContent()
  {
    this.disableTabs();
    foreach (WindowMetaTab tab in this._tabs)
    {
      foreach (Transform tabElement in tab.tab_elements)
      {
        if (!Object.op_Equality((Object) tabElement, (Object) null))
          ((Component) tabElement).gameObject.SetActive(false);
      }
    }
    TabHideAction onTabHide = this._on_tab_hide;
    if (onTabHide != null)
      onTabHide();
    Tooltip.hideTooltipNow();
  }

  protected void disableTabs()
  {
    foreach (Component tab in this._tabs)
      tab.GetComponent<Image>().sprite = WindowMetaTabButtonsContainer._tab_button_off;
  }

  public List<WindowMetaTab> getContentTabs()
  {
    this._tabs_with_content.Sort((Comparison<WindowMetaTab>) ((p1, p2) => ((Component) p1).transform.GetSiblingIndex().CompareTo(((Component) p2).transform.GetSiblingIndex())));
    return this._tabs_with_content;
  }

  public WindowMetaTab getActiveTab()
  {
    return Object.op_Inequality((Object) this._tab_last, (Object) null) ? this._tab_last : this.tab_default;
  }

  public void reloadActiveTab() => this.getActiveTab().doAction();

  public void addTabShowCallback(TabShowAction pCallback) => this._on_tab_show += pCallback;

  public void removeTabShowCallback(TabShowAction pCallback) => this._on_tab_show -= pCallback;

  public void addTabHideCallback(TabHideAction pCallback) => this._on_tab_hide += pCallback;

  public void removeTabHideCallback(TabHideAction pCallback) => this._on_tab_hide -= pCallback;
}
