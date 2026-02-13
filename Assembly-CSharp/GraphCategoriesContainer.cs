// Decompiled with JetBrains decompiler
// Type: GraphCategoriesContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class GraphCategoriesContainer : MonoBehaviour
{
  public GraphController graph_controller;
  public GraphCategoryGroup category_group = GraphCategoryGroup.General;
  private GraphCategoryGroup _last_category_group;
  private GraphCategoryGroup _last_category_groups;
  private List<HistoryDataAsset> _current_list = new List<HistoryDataAsset>();
  private Dictionary<string, ButtonGraphCategory> _category_buttons = new Dictionary<string, ButtonGraphCategory>();
  private ButtonGraphCategory _prefab_button;
  private bool _is_initialized;
  [SerializeField]
  private TabTogglesGroup _category_groups;

  private void init()
  {
    if (this._is_initialized)
      return;
    this._is_initialized = true;
    foreach (Component componentsInChild in ((Component) this).GetComponentsInChildren<ButtonGraphCategory>())
      Object.Destroy((Object) componentsInChild.gameObject);
    this._prefab_button = Resources.Load<ButtonGraphCategory>("ui/graphs/GraphCategoryButton");
  }

  public void apply()
  {
    this.init();
    List<HistoryDataAsset> categories = this.graph_controller.getCategories();
    if ((this._last_category_group != this.category_group || this._current_list.Count <= 0 || this._current_list.Count != categories.Count || !this._current_list.All<HistoryDataAsset>(new Func<HistoryDataAsset, bool>(categories.Contains)) ? 0 : (categories.All<HistoryDataAsset>(new Func<HistoryDataAsset, bool>(this._current_list.Contains)) ? 1 : 0)) != 0)
    {
      foreach (ButtonGraphCategory buttonGraphCategory in this._category_buttons.Values)
        this.graph_controller.setCategoryEnabled(((Object) ((Component) buttonGraphCategory).gameObject).name, buttonGraphCategory.is_on, false);
    }
    else
    {
      foreach (Component component in this._category_buttons.Values)
        component.gameObject.SetActive(false);
      GraphCategoryGroup pGroups = GraphCategoryGroup.None;
      this._current_list = new List<HistoryDataAsset>((IEnumerable<HistoryDataAsset>) categories);
      foreach (HistoryDataAsset current in this._current_list)
      {
        pGroups |= current.category_group;
        if (current.category_group.HasFlag((Enum) this.category_group))
        {
          ButtonGraphCategory buttonGraphCategory;
          if (!this._category_buttons.TryGetValue(current.id, out buttonGraphCategory))
          {
            buttonGraphCategory = Object.Instantiate<ButtonGraphCategory>(this._prefab_button, ((Component) this).transform);
            ((Object) ((Component) buttonGraphCategory).gameObject).name = current.id;
            ((Component) buttonGraphCategory).transform.SetParent(((Component) this).transform);
            buttonGraphCategory.init();
            buttonGraphCategory.setAsset(current);
            buttonGraphCategory.is_on = this.graph_controller.isCategoryEnabled(current.id);
            this._category_buttons.Add(current.id, buttonGraphCategory);
          }
          ((Component) buttonGraphCategory).gameObject.SetActive(true);
        }
      }
      this._last_category_group = this.category_group;
      this.showCategoryGroups(pGroups);
    }
  }

  private void showCategoryGroups(GraphCategoryGroup pGroups)
  {
    ((Component) this._category_groups).gameObject.SetActive(pGroups.Count<GraphCategoryGroup>() > 1);
    if (this._last_category_groups == pGroups)
      return;
    this._category_groups.clearButtons();
    if (pGroups.HasFlag((Enum) GraphCategoryGroup.General))
      this._category_groups.tryAddButton("ui/Icons/iconRenown", "tab_general_stats", new TabToggleAction(this.apply), (TabToggleAction) (() => this.category_group = GraphCategoryGroup.General));
    if (pGroups.HasFlag((Enum) GraphCategoryGroup.Noosphere))
      this._category_groups.tryAddButton("ui/Icons/iconKnowledge", "tab_noosphere", new TabToggleAction(this.apply), (TabToggleAction) (() => this.category_group = GraphCategoryGroup.Noosphere));
    if (pGroups.HasFlag((Enum) GraphCategoryGroup.Deaths))
      this._category_groups.tryAddButton("civ/map_mark_death", "tab_deaths", new TabToggleAction(this.apply), (TabToggleAction) (() => this.category_group = GraphCategoryGroup.Deaths));
    if (pGroups.HasFlag((Enum) GraphCategoryGroup.Biomes))
      this._category_groups.tryAddButton("ui/Icons/iconSeedClover", "tab_biomes", new TabToggleAction(this.apply), (TabToggleAction) (() => this.category_group = GraphCategoryGroup.Biomes));
    if (pGroups.HasFlag((Enum) GraphCategoryGroup.Tiles))
      this._category_groups.tryAddButton("ui/Icons/iconZones", "tab_tiles", new TabToggleAction(this.apply), (TabToggleAction) (() => this.category_group = GraphCategoryGroup.Tiles));
    this._last_category_groups = pGroups;
    this._category_groups.enableFirst();
  }

  public void setCategoryEnabled(string pId, bool pEnabled)
  {
    if (this.graph_controller.multi_chart)
    {
      foreach (ButtonGraphCategory buttonGraphCategory in this._category_buttons.Values)
        buttonGraphCategory.is_on = ((Object) ((Component) buttonGraphCategory).gameObject).name == pId;
      this.graph_controller.disableAllCategories(pId);
    }
    this.graph_controller.setCategoryEnabled(pId, pEnabled, false);
    this.graph_controller.adjustCharts();
  }
}
