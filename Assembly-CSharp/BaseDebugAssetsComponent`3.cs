// Decompiled with JetBrains decompiler
// Type: BaseDebugAssetsComponent`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class BaseDebugAssetsComponent<TAsset, TAssetElement, TAssetElementPlace> : MonoBehaviour
  where TAsset : Asset
  where TAssetElement : BaseDebugAssetElement<TAsset>
  where TAssetElementPlace : BaseAssetElementPlace<TAsset, TAssetElement>
{
  public TAssetElementPlace place_prefab;
  public TAssetElement element_prefab;
  public ScrollRect scroll_rect;
  private RectTransform _scroll_rect_transform;
  private Rect _scroll_world_rect;
  public InputField search_input_field;
  public SortingTab sorting_tab;
  protected List<TAsset> list_assets_sorted;
  protected List<TAsset> list_assets_sorting;
  protected List<TAsset> list_assets_sorting_default;
  protected bool default_sort_reversed;
  protected List<TAssetElementPlace> list_places;
  private bool _initialized;

  protected virtual List<TAsset> getAssetsList() => throw new NotImplementedException();

  protected virtual List<TAsset> getListCivsSort() => throw new NotImplementedException();

  private void OnEnable() => this.refresh();

  private void Start()
  {
    this._scroll_rect_transform = ((Component) this.scroll_rect).GetComponent<RectTransform>();
    // ISSUE: method pointer
    ((UnityEvent<string>) this.search_input_field.onValueChanged).AddListener(new UnityAction<string>((object) this, __methodptr(setDataSearched)));
    this.init();
  }

  protected virtual void init()
  {
    this.list_assets_sorted = new List<TAsset>((IEnumerable<TAsset>) this.getAssetsList());
    this.list_assets_sorting = new List<TAsset>((IEnumerable<TAsset>) this.getAssetsList());
    this.list_assets_sorting_default = new List<TAsset>((IEnumerable<TAsset>) this.getAssetsList());
    foreach (Component component in ((Component) this).transform)
      Object.Destroy((Object) component.gameObject);
    this.list_places = new List<TAssetElementPlace>();
    foreach (TAsset assets in this.getAssetsList())
    {
      TAssetElementPlace assetElementPlace = Object.Instantiate<TAssetElementPlace>(this.place_prefab, ((Component) this).transform);
      this.list_places.Add(assetElementPlace);
      assetElementPlace.setData(assets, this.element_prefab);
    }
    this.sorting_tab.addButton("ui/Icons/iconHumans", "sort_by_civs", new SortButtonAction(this.setDataResorted), (SortButtonAction) (() => this.list_assets_sorted = this.getListCivsSort()));
    this.sorting_tab.addButton("ui/Icons/actor_traits/iconClumsy", "default_sort", new SortButtonAction(this.setDataResorted), (SortButtonAction) (() =>
    {
      this.list_assets_sorted = this.list_assets_sorting_default;
      if (this.sorting_tab.getCurrentButton().getState() != SortButtonState.Down && !this.default_sort_reversed)
        return;
      this.default_sort_reversed = !this.default_sort_reversed;
      this.list_assets_sorted.Reverse();
    })).click();
    this._initialized = true;
  }

  private void Update()
  {
    if (!this._initialized)
      return;
    this._scroll_world_rect = this._scroll_rect_transform.GetWorldRect();
    foreach (TAssetElementPlace listPlace in this.list_places)
    {
      if (listPlace.game_object_cache.activeSelf)
      {
        if (Object.op_Inequality((Object) (object) listPlace.element, (Object) null))
          listPlace.element.update();
        this.checkVisible(listPlace);
      }
    }
  }

  private void checkVisible(TAssetElementPlace pPlace)
  {
    if (!((Component) (object) pPlace).gameObject.activeSelf)
      return;
    bool flag = this.isElementVisible(pPlace);
    if (!flag && pPlace.has_element)
    {
      pPlace.clear();
    }
    else
    {
      if (!flag || pPlace.has_element)
        return;
      TAsset pAsset = this.list_assets_sorted[((Transform) pPlace.rect_transform).GetSiblingIndex()];
      pPlace.setData(pAsset, this.element_prefab);
    }
  }

  public void refresh()
  {
    if (!this._initialized)
      return;
    this.setDataResorted();
  }

  public bool isElementVisible(TAssetElementPlace pPlace)
  {
    return ((Rect) ref this._scroll_world_rect).Overlaps(pPlace.rect_transform.GetWorldRect());
  }

  protected void setDataResorted()
  {
    int num = this.list_assets_sorted.Count - 1;
    for (int index = 0; index < this.list_places.Count; ++index)
    {
      TAssetElementPlace listPlace = this.list_places[index];
      if (index > num)
      {
        listPlace.game_object_cache.SetActive(false);
        listPlace.allowed_for_search = false;
      }
      else
      {
        listPlace.game_object_cache.SetActive(true);
        listPlace.allowed_for_search = true;
        if (this.isElementVisible(listPlace) && listPlace.has_element)
        {
          TAsset pAsset = this.list_assets_sorted[index];
          listPlace.element.setData(pAsset);
        }
      }
    }
    this.setDataSearched(this.search_input_field.text);
  }

  protected void checkReverseSort()
  {
    if (this.sorting_tab.getCurrentButton().getState() != SortButtonState.Down)
      return;
    this.list_assets_sorted.Reverse();
  }

  private void setDataSearched(string pValue)
  {
    if (!((Component) this).gameObject.activeSelf)
      return;
    pValue = pValue.ToLower();
    if (string.IsNullOrEmpty(pValue))
    {
      foreach (TAssetElementPlace listPlace in this.list_places)
      {
        if (listPlace.allowed_for_search)
          listPlace.game_object_cache.SetActive(true);
      }
    }
    else
    {
      for (int index = 0; index < this.list_assets_sorted.Count; ++index)
      {
        TAssetElementPlace listPlace = this.list_places[index];
        if (listPlace.allowed_for_search)
        {
          bool flag = this.list_assets_sorted[index].id.ToLower().Contains(pValue);
          listPlace.game_object_cache.SetActive(flag);
        }
      }
    }
  }
}
