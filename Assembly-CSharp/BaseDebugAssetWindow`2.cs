// Decompiled with JetBrains decompiler
// Type: BaseDebugAssetWindow`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
public class BaseDebugAssetWindow<TAsset, TAssetElement> : TabbedWindow
  where TAsset : Asset
  where TAssetElement : BaseDebugAssetElement<TAsset>
{
  public static TAssetElement current_element;
  public SpriteElement sprite_element_prefab;
  public TAssetElement asset_debug_element;
  public Transform sprite_elements_parent;
  public SortingTab sorting_tab;
  public FieldInfoList field_infos;
  public PowerButton show_sprites_button;
  public GameObject hidden_sprites_placeholder;
  private List<FieldInfo> _sorted_fields;
  private List<FieldInfo> _sorting_fields;
  private List<FieldInfo> _default_sorting_fields;
  private bool _default_reversed;
  protected TAsset asset;
  private SortButton _default_sort_button;
  private bool _initialized;

  protected override void create()
  {
    base.create();
    this.asset = BaseDebugAssetElement<TAsset>.selected_asset;
    this.sorting_tab.addButton("ui/Icons/onomastics/onomastics_vowel_separator", "sort_by_alphabet", new SortButtonAction(this.setDataResorted), (SortButtonAction) (() =>
    {
      this._sorted_fields = this._sorting_fields;
      this._sorted_fields.Sort(new Comparison<FieldInfo>(this.sortByName));
      this.checkReverseSort();
    }));
    this.sorting_tab.addButton("ui/Icons/onomastics/onomastics_consonant_separator", "sort_by_type", new SortButtonAction(this.setDataResorted), (SortButtonAction) (() =>
    {
      this._sorted_fields = this._sorting_fields;
      this._sorted_fields.Sort(new Comparison<FieldInfo>(this.sortByType));
      this.checkReverseSort();
    }));
    this._default_sort_button = this.sorting_tab.addButton("ui/Icons/actor_traits/iconClumsy", "default_sort", new SortButtonAction(this.setDataResorted), (SortButtonAction) (() =>
    {
      this._sorted_fields = this._default_sorting_fields;
      if (this.sorting_tab.getCurrentButton().getState() != SortButtonState.Down && !this._default_reversed)
        return;
      this._default_reversed = !this._default_reversed;
      this._sorted_fields.Reverse();
    }));
  }

  private void OnEnable()
  {
    this.asset = BaseDebugAssetElement<TAsset>.selected_asset;
    BaseDebugAssetWindow<TAsset, TAssetElement>.current_element = this.asset_debug_element;
    this._initialized = false;
  }

  private void Update()
  {
    this.load();
    this.asset_debug_element.update();
  }

  private void load()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this.scroll_window.titleText.text = this.asset.id;
    this.asset_debug_element.setData(this.asset);
    this.initSprites();
    this.field_infos.init<TAsset>();
    this.field_infos.setData((object) this.asset);
    this._sorted_fields = new List<FieldInfo>((IEnumerable<FieldInfo>) this.field_infos.field_infos);
    this._sorting_fields = new List<FieldInfo>((IEnumerable<FieldInfo>) this.field_infos.field_infos);
    this._default_sorting_fields = new List<FieldInfo>((IEnumerable<FieldInfo>) this.field_infos.field_infos);
    this._default_sort_button.click();
  }

  protected virtual void initSprites()
  {
    foreach (Component component in this.sprite_elements_parent)
      Object.Destroy((Object) component.gameObject);
  }

  public void clickShowAllSprites()
  {
    GameObject gameObject = ((Component) this.sprite_elements_parent).gameObject;
    bool flag = !gameObject.activeSelf;
    gameObject.SetActive(flag);
    this.hidden_sprites_placeholder.SetActive(!flag);
    if (flag)
      this.show_sprites_button.icon.sprite = SpriteTextureLoader.getSprite("ui/icons/IconOn");
    else
      this.show_sprites_button.icon.sprite = SpriteTextureLoader.getSprite("ui/icons/IconOff");
  }

  private void setDataResorted()
  {
    this.field_infos.clear();
    Dictionary<string, FieldInfoListItem> fieldsCollectionData = this.field_infos.fields_collection_data;
    fieldsCollectionData.Clear();
    for (int index = 0; index < this._sorted_fields.Count; ++index)
    {
      FieldInfoListItem fieldData = this.field_infos.getFieldData(this._sorted_fields[index], (object) this.asset);
      fieldsCollectionData.Add(fieldData.field_name, fieldData);
      this.field_infos.addRow(fieldData.field_name, fieldData.field_value);
    }
    this.field_infos.setDataSearched(this.field_infos.search_input_field.text);
  }

  private void checkReverseSort()
  {
    if (this.sorting_tab.getCurrentButton().getState() != SortButtonState.Down)
      return;
    this._sorted_fields.Reverse();
  }

  private int sortByName(FieldInfo pObject1, FieldInfo pObject2)
  {
    return string.Compare(pObject1.Name, pObject2.Name, StringComparison.InvariantCulture);
  }

  private int sortByType(FieldInfo pObject1, FieldInfo pObject2)
  {
    return string.Compare(pObject1.FieldType.Name, pObject2.FieldType.Name, StringComparison.InvariantCulture);
  }
}
