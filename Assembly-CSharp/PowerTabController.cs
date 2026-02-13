// Decompiled with JetBrains decompiler
// Type: PowerTabController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class PowerTabController : MonoBehaviour
{
  public PowersTab tab_main;
  public PowersTab tab_selected_unit;
  public PowersTab tab_multiple_units;
  public PowersTab tab_selected_kingdom;
  public PowersTab tab_selected_subspecies;
  public PowersTab tab_selected_alliance;
  public PowersTab tab_selected_army;
  public PowersTab tab_selected_family;
  public PowersTab tab_selected_language;
  public PowersTab tab_selected_culture;
  public PowersTab tab_selected_religion;
  public PowersTab tab_selected_clan;
  public PowersTab tab_selected_city;
  [Space]
  public Button t_main;
  public Button t_drawing;
  public Button t_kingdoms;
  public Button t_creatures;
  public Button t_bombs;
  public Button t_nature;
  public Button t_other;
  public Transform copyTarget;
  [Space]
  internal static PowerTabController instance;
  public ToolbarArrow arrowLeft;
  public ToolbarArrow arrowRight;
  public RectTransform rect;
  public ScrollRectExtended scrollRect;
  private List<Button> _buttons;
  public static string prev_selected_meta_id;

  private void Awake()
  {
    PowerTabController.instance = this;
    this._buttons = new List<Button>()
    {
      this.t_drawing,
      this.t_kingdoms,
      this.t_creatures,
      this.t_nature,
      this.t_bombs,
      this.t_other
    };
  }

  private void Update()
  {
    PowersTab activeTab = PowersTab.getActiveTab();
    PowerTabAsset asset = activeTab.getAsset();
    if (asset.on_update_check_active != null && !asset.on_update_check_active(asset))
    {
      activeTab.hideTab();
      PowerTabController.showMainTab();
      World.world.selected_buttons.unselectTabs();
    }
    else
      activeTab.update();
  }

  internal static float currentScrollPosition()
  {
    return PowerTabController.instance.scrollRect.horizontalNormalizedPosition;
  }

  public static void loadScrollPosition(float pPosition)
  {
  }

  internal void resetToStartScrollPosition() => this.scrollRect.horizontalNormalizedPosition = 0.0f;

  public Button getTabForTabGroup(string pGroupName)
  {
    foreach (Button button in this._buttons)
    {
      if (((UnityEventBase) button.onClick).GetPersistentTarget(0).name == pGroupName)
        return button;
    }
    return (Button) null;
  }

  public Button getNext(string pActiveTab)
  {
    this._buttons.Sort((Comparison<Button>) ((a, b) => ((Component) a).transform.GetSiblingIndex().CompareTo(((Component) b).transform.GetSiblingIndex())));
    for (int index = 0; index < this._buttons.Count; ++index)
    {
      if (((UnityEventBase) this._buttons[index].onClick).GetPersistentTarget(0).name == pActiveTab && index < this._buttons.Count - 1)
        return this._buttons[index + 1];
    }
    return this._buttons.First<Button>();
  }

  public Button getPrev(string pActiveTab)
  {
    this._buttons.Sort((Comparison<Button>) ((a, b) => ((Component) a).transform.GetSiblingIndex().CompareTo(((Component) b).transform.GetSiblingIndex())));
    for (int index = 0; index < this._buttons.Count; ++index)
    {
      if (((UnityEventBase) this._buttons[index].onClick).GetPersistentTarget(0).name == pActiveTab && index > 0)
        return this._buttons[index - 1];
    }
    return this._buttons.Last<Button>();
  }

  public static void showMainTab() => PowerTabController.instance.tab_main.showTab((Button) null);

  public static void showTabSelectedUnit()
  {
    SelectedTabsHistory.addToHistory((NanoObject) SelectedUnit.unit);
    PowerTabAsset asset = PowerTabController.getAsset("selected_unit");
    asset.tryToShowPowerTab();
    PowerTabController.showWorldTipSelected(asset.id);
  }

  public static void showTabMultipleUnits()
  {
    PowerTabAsset asset = PowerTabController.getAsset("multiple_units");
    asset.tryToShowPowerTab();
    PowerTabController.showWorldTipSelected(asset.id);
  }

  public static void showTabSelectedMeta(MetaTypeAsset pMetaTypeAsset)
  {
    PowerTabAsset asset = PowerTabController.getAsset(pMetaTypeAsset.power_tab_id);
    asset.tryToShowPowerTab();
    MetaTypeAsset.last_meta_type = pMetaTypeAsset.id;
    PowerTabController.showWorldTipSelected(asset.id);
  }

  public static void showWorldTipSelected(string pPowerTabId, bool pBar = true)
  {
    string typeId = SelectedObjects.getSelectedNanoObject().getTypeID();
    if (PowerTabController.prev_selected_meta_id == typeId)
      return;
    PowerTabController.prev_selected_meta_id = typeId;
    PowerTabAsset pPowerTabAsset = AssetManager.power_tab_library.get(pPowerTabId);
    string pText = pPowerTabAsset.get_localized_worldtip(pPowerTabAsset);
    if (pBar)
      WorldTip.instance.showToolbarText(pText);
    else
      WorldTip.showNowTop(pText, false);
  }

  public static PowerTabAsset getAsset(string pID) => AssetManager.power_tab_library.get(pID);
}
