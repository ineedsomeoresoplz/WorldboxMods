// Decompiled with JetBrains decompiler
// Type: ListWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ListWindow : MonoBehaviour
{
  [SerializeField]
  private Transform _content_list;
  [SerializeField]
  private WindowMetaTab _tab_list;
  [SerializeField]
  private WindowMetaTab _tab_favorite;
  [SerializeField]
  private WindowMetaTab _tab_dead;
  [SerializeField]
  private WindowMetaTabButtonsContainer _tabs_container;
  [SerializeField]
  private Image _art;
  [SerializeField]
  private Image _tab_list_icon;
  [SerializeField]
  private Image _title_icon_left;
  [SerializeField]
  private Image _title_icon_right;
  [SerializeField]
  private Image _no_items_icon_left;
  [SerializeField]
  private Image _no_items_icon_right;
  [SerializeField]
  private TipButton _tip_button_favorite;
  [SerializeField]
  private LocalizedText _no_items_description;
  [SerializeField]
  private ListWindowStatistics _statistics;
  [SerializeField]
  private MetaRepresentationTotal _breakdown;
  [SerializeField]
  private GameObject _no_items;
  [SerializeField]
  private SortingTab _sorting_tab;
  [SerializeField]
  private Transform _list_transform;
  [SerializeField]
  private ScrollRect _scroll_rect;
  [SerializeField]
  private Text _title_counter;
  [SerializeField]
  private Text _favorites_counter;
  [SerializeField]
  private Text _dead_counter;
  [SerializeField]
  private GameObject _list_element_prefab;
  [SerializeField]
  private MetaType _meta_type;
  private ListWindowAsset _asset;

  private void Awake()
  {
    this._asset = AssetManager.list_window_library.getByMetaType(this._meta_type);
    ((Component) this._list_transform).gameObject.SetActive(false);
    IComponentList pComponent = this._asset.set_list_component(this._list_transform);
    this.initComponent(pComponent);
    this.initTabsCallbacks(pComponent);
    // ISSUE: method pointer
    this._tab_list.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) this, __methodptr(\u003CAwake\u003Eb__25_0)));
    this._no_items_description.setKeyAndUpdate(this._asset.no_items_locale);
    if (Object.op_Inequality((Object) this._statistics, (Object) null))
      this._statistics.meta_type = this._meta_type;
    ((Component) this._list_transform).gameObject.SetActive(true);
    this._art.sprite = SpriteTextureLoader.getSprite(this._asset.art_path);
    Sprite sprite = SpriteTextureLoader.getSprite(this._asset.icon_path);
    this._tab_list_icon.sprite = sprite;
    this._title_icon_left.sprite = sprite;
    this._title_icon_right.sprite = sprite;
    this._no_items_icon_left.sprite = sprite;
    this._no_items_icon_right.sprite = sprite;
    if (!Object.op_Inequality((Object) this._breakdown, (Object) null))
      return;
    this._breakdown.setMetaType(this._meta_type);
  }

  protected virtual void initComponent(IComponentList pComponent)
  {
    pComponent.init(this._no_items, this._sorting_tab, this._list_element_prefab, this._list_transform, this._scroll_rect, this._title_counter, this._favorites_counter, this._dead_counter);
  }

  protected virtual void initTabsCallbacks(IComponentList pComponent)
  {
    bool flag1 = Object.op_Inequality((Object) this._tab_favorite, (Object) null) && ((Component) this._tab_favorite).gameObject.activeSelf;
    bool flag2 = Object.op_Inequality((Object) this._tab_dead, (Object) null) && ((Component) this._tab_dead).gameObject.activeSelf;
    if (flag1 | flag2)
    {
      this.setTabCallbacks(this._tab_list, new Action(pComponent.setShowAll), new Action(pComponent.setDefault));
      if (flag1)
      {
        this.setTabCallbacks(this._tab_favorite, new Action(pComponent.setShowFavoritesOnly), new Action(pComponent.setDefault));
        // ISSUE: method pointer
        this._tab_favorite.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) this, __methodptr(\u003CinitTabsCallbacks\u003Eb__27_0)));
      }
      if (!flag2)
        return;
      this.setTabCallbacks(this._tab_list, new Action(pComponent.setShowAliveOnly), new Action(pComponent.setDefault));
      this.setTabCallbacks(this._tab_dead, new Action(pComponent.setShowDeadOnly), new Action(pComponent.setDefault));
      // ISSUE: method pointer
      this._tab_dead.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) this, __methodptr(\u003CinitTabsCallbacks\u003Eb__27_1)));
    }
    else
    {
      // ISSUE: method pointer
      this._tab_list.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) this, __methodptr(\u003CinitTabsCallbacks\u003Eb__27_2)));
    }
  }

  protected void setTabCallbacks(WindowMetaTab pTab, Action pCallback, Action pDefaultCallback = null)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ListWindow.\u003C\u003Ec__DisplayClass28_0 cDisplayClass280 = new ListWindow.\u003C\u003Ec__DisplayClass28_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass280.pDefaultCallback = pDefaultCallback;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass280.pCallback = pCallback;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass280.\u003C\u003E4__this = this;
    ((UnityEventBase) pTab.tab_action).RemoveAllListeners();
    // ISSUE: reference to a compiler-generated field
    if (cDisplayClass280.pDefaultCallback != null)
    {
      // ISSUE: method pointer
      pTab.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) cDisplayClass280, __methodptr(\u003CsetTabCallbacks\u003Eb__0)));
    }
    // ISSUE: method pointer
    pTab.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) cDisplayClass280, __methodptr(\u003CsetTabCallbacks\u003Eb__1)));
    // ISSUE: method pointer
    pTab.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) cDisplayClass280, __methodptr(\u003CsetTabCallbacks\u003Eb__2)));
  }

  protected LocalizedText getNoItems() => this._no_items_description;
}
