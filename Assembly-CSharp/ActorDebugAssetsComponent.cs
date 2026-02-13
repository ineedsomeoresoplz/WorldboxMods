// Decompiled with JetBrains decompiler
// Type: ActorDebugAssetsComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class ActorDebugAssetsComponent : 
  BaseDebugAssetsComponent<ActorAsset, ActorDebugAssetElement, ActorAssetElementPlace>
{
  protected override List<ActorAsset> getAssetsList() => AssetManager.actor_library.list;

  protected override void init()
  {
    this.sorting_tab.addButton("ui/Icons/iconHealth", "sort_by_health", new SortButtonAction(((BaseDebugAssetsComponent<ActorAsset, ActorDebugAssetElement, ActorAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<ActorAsset>(this.sortByHealth));
      this.checkReverseSort();
    }));
    this.sorting_tab.addButton("ui/Icons/iconDamage", "sort_by_damage", new SortButtonAction(((BaseDebugAssetsComponent<ActorAsset, ActorDebugAssetElement, ActorAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<ActorAsset>(this.sortByDamage));
      this.checkReverseSort();
    }));
    this.sorting_tab.addButton("ui/Icons/iconSpeed", "sort_by_speed", new SortButtonAction(((BaseDebugAssetsComponent<ActorAsset, ActorDebugAssetElement, ActorAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<ActorAsset>(this.sortBySpeed));
      this.checkReverseSort();
    }));
    this.sorting_tab.addButton("ui/Icons/iconAge", "sort_by_lifespan", new SortButtonAction(((BaseDebugAssetsComponent<ActorAsset, ActorDebugAssetElement, ActorAssetElementPlace>) this).setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting;
      this.list_assets_sorted.Sort(new Comparison<ActorAsset>(this.sortByLifespan));
      this.checkReverseSort();
    }));
    base.init();
  }

  private int sortByHealth(ActorAsset pObject1, ActorAsset pObject2)
  {
    return -pObject1.getStatsForOverview()["health"].CompareTo(pObject2.getStatsForOverview()["health"]);
  }

  private int sortByDamage(ActorAsset pObject1, ActorAsset pObject2)
  {
    return -pObject1.getStatsForOverview()["damage"].CompareTo(pObject2.getStatsForOverview()["damage"]);
  }

  private int sortBySpeed(ActorAsset pObject1, ActorAsset pObject2)
  {
    return -pObject1.getStatsForOverview()["speed"].CompareTo(pObject2.getStatsForOverview()["speed"]);
  }

  private int sortByLifespan(ActorAsset pObject1, ActorAsset pObject2)
  {
    return -pObject1.getStatsForOverview()["lifespan"].CompareTo(pObject2.getStatsForOverview()["lifespan"]);
  }

  protected override List<ActorAsset> getListCivsSort()
  {
    bool flag = this.sorting_tab.getCurrentButton().getState() == SortButtonState.Up;
    List<ActorAsset> listCivsSort = new List<ActorAsset>();
    foreach (ActorAsset assets in this.getAssetsList())
    {
      if (assets.civ == flag)
        listCivsSort.Add(assets);
    }
    return listCivsSort;
  }
}
