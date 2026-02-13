// Decompiled with JetBrains decompiler
// Type: BuildingDebugAssetsComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BuildingDebugAssetsComponent : 
  BaseDebugAssetsComponent<BuildingAsset, BuildingDebugAssetElement, BuildingAssetElementPlace>
{
  protected override List<BuildingAsset> getAssetsList() => AssetManager.buildings.list;

  protected override void init()
  {
    this.sorting_tab.addButton("ui/Icons/iconHealth", "sort_by_health", new SortButtonAction(((BaseDebugAssetsComponent<BuildingAsset, BuildingDebugAssetElement, BuildingAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<BuildingAsset>(this.sortByHealth));
      this.checkReverseSort();
    }));
    this.sorting_tab.addButton("ui/Icons/iconDamage", "sort_by_damage", new SortButtonAction(((BaseDebugAssetsComponent<BuildingAsset, BuildingDebugAssetElement, BuildingAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<BuildingAsset>(this.sortByDamage));
      this.checkReverseSort();
    }));
    this.sorting_tab.addButton("ui/Icons/iconPopulationAttackers", "sort_by_targets", new SortButtonAction(((BaseDebugAssetsComponent<BuildingAsset, BuildingDebugAssetElement, BuildingAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<BuildingAsset>(this.sortByTargets));
      this.checkReverseSort();
    }));
    this.sorting_tab.addButton("effects/circle132", "sort_by_area_of_effect", new SortButtonAction(((BaseDebugAssetsComponent<BuildingAsset, BuildingDebugAssetElement, BuildingAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<BuildingAsset>(this.sortByAreaOfEffect));
      this.checkReverseSort();
    }));
    base.init();
  }

  private int sortByHealth(BuildingAsset pObject1, BuildingAsset pObject2)
  {
    return -pObject1.base_stats["health"].CompareTo(pObject2.base_stats["health"]);
  }

  private int sortByDamage(BuildingAsset pObject1, BuildingAsset pObject2)
  {
    return -pObject1.base_stats["damage"].CompareTo(pObject2.base_stats["damage"]);
  }

  private int sortByTargets(BuildingAsset pObject1, BuildingAsset pObject2)
  {
    return -pObject1.base_stats["targets"].CompareTo(pObject2.base_stats["targets"]);
  }

  private int sortByAreaOfEffect(BuildingAsset pObject1, BuildingAsset pObject2)
  {
    return -pObject1.base_stats["area_of_effect"].CompareTo(pObject2.base_stats["area_of_effect"]);
  }

  protected override List<BuildingAsset> getListCivsSort()
  {
    bool flag1 = this.sorting_tab.getCurrentButton().getState() == SortButtonState.Up;
    List<BuildingAsset> listCivsSort = new List<BuildingAsset>();
    foreach (BuildingAsset assets in this.getAssetsList())
    {
      bool flag2 = string.IsNullOrEmpty(assets.civ_kingdom);
      if (!(flag2 & flag1) && (flag2 || flag1))
        listCivsSort.Add(assets);
    }
    return listCivsSort;
  }
}
