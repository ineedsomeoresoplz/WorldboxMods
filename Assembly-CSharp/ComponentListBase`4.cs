// Decompiled with JetBrains decompiler
// Type: ComponentListBase`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ComponentListBase<TListElement, TMetaObject, TData, TComponent> : 
  MonoBehaviour,
  IComponentList,
  IShouldRefreshWindow
  where TListElement : WindowListElementBase<TMetaObject, TData>
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
  where TComponent : ComponentListBase<TListElement, TMetaObject, TData, TComponent>
{
  public GameObject no_items;
  public SortingTab sorting_tab;
  public TListElement element_prefab;
  public Transform list_transform;
  public ScrollRect scroll_rect;
  [SerializeField]
  private Text _title_counter;
  [SerializeField]
  private Text _favorites_counter;
  [SerializeField]
  private Text _dead_counter;
  private ListItemsFilter _show_items;
  public GetListOfObjectsFunc<TListElement, TMetaObject, TData, TComponent> get_objects_delegate = new GetListOfObjectsFunc<TListElement, TMetaObject, TData, TComponent>(ComponentListBase<TListElement, TMetaObject, TData, TComponent>.getObjects);
  private ObjectPoolGenericMono<TListElement> _pool_elements;
  private ObjectPoolGenericMono<BaseEmptyListMono> _pool_empty_elements;
  protected Comparison<TMetaObject> current_sort;
  public readonly List<NanoObject> meta_list = new List<NanoObject>();
  private bool autolayout_done;
  private const int PADDING_ELEMENTS = 3;
  private static readonly bool _debug;
  private bool _created;
  protected int latest_counted;
  private float _element_height;

  protected virtual MetaType meta_type => throw new NotImplementedException();

  private MetaTypeAsset _meta_type_asset => AssetManager.meta_type_library.getAsset(this.meta_type);

  protected virtual IEnumerable<TMetaObject> getObjectsList()
  {
    return this.get_objects_delegate((TComponent) this);
  }

  protected virtual bool change_asset_sort_order => true;

  protected ObjectPoolGenericMono<BaseEmptyListMono> getPoolEmpty() => this._pool_empty_elements;

  private void checkCreate()
  {
    if (this._created)
      return;
    this._created = true;
    this.create();
  }

  protected virtual void create()
  {
    this._pool_elements = new ObjectPoolGenericMono<TListElement>(this.element_prefab, this.list_transform);
    this._element_height = ((Component) ((Component) (object) this.element_prefab).transform).GetComponent<RectTransform>().sizeDelta.y;
    this.addEmptyPoolSystem();
    this.showSortingTabs();
  }

  protected virtual void setupSortingTabs()
  {
  }

  protected virtual void showSortingTabs()
  {
    this.sorting_tab.clearButtons();
    this.setupSortingTabs();
    this.sorting_tab.enableFirstIfNone();
  }

  private void OnRenderObject() => this.autolayout_done = true;

  private void LateUpdate()
  {
    if (!this.autolayout_done)
      return;
    IReadOnlyList<BaseEmptyListMono> listTotal = this._pool_empty_elements.getListTotal();
    int num1 = int.MaxValue;
    int num2 = int.MinValue;
    float y = ((Transform) this.scroll_rect.content).localPosition.y;
    double num3 = (double) y;
    Rect rect = this.scroll_rect.viewport.rect;
    double height = (double) ((Rect) ref rect).height;
    float pScrollRectTop = (float) (num3 + height);
    for (int index = 0; index < listTotal.Count; ++index)
    {
      BaseEmptyListMono baseEmptyListMono = listTotal[index];
      if (((Component) baseEmptyListMono).gameObject.activeSelf)
      {
        if (this.IsVisibleInScrollRect(baseEmptyListMono.rect_transform, this.scroll_rect, pScrollRectTop, y))
        {
          if (num1 == int.MaxValue)
            num1 = index;
          num2 = index;
        }
        else if (num2 > int.MinValue)
          break;
      }
    }
    if (num2 == int.MaxValue || num1 == int.MinValue)
      return;
    int num4 = Math.Max(0, num1 - 3);
    int num5 = Math.Min(listTotal.Count - 1, num2 + 3);
    for (int index = 0; index < listTotal.Count; ++index)
    {
      if (index < num4 || index > num5)
        this.releaseElement(listTotal[index]);
    }
    for (int index = num4; index <= num5; ++index)
    {
      BaseEmptyListMono pEmptyMono = listTotal[index];
      if (((Component) pEmptyMono).gameObject.activeSelf && !pEmptyMono.hasElement())
        this.makeElementVisible(pEmptyMono);
    }
    if (!ComponentListBase<TListElement, TMetaObject, TData, TComponent>._debug)
      return;
    this.debugUpdateElementNames(listTotal, pScrollRectTop, y);
  }

  private void makeElementVisible(BaseEmptyListMono pEmptyMono)
  {
    TListElement next = this._pool_elements.getNext();
    next.show((TMetaObject) pEmptyMono.meta_object);
    ((Component) (object) next).transform.SetParent(((Component) pEmptyMono).transform);
    ((Component) (object) next).transform.localPosition = Vector3.zero;
    pEmptyMono.assignElement((MonoBehaviour) next);
  }

  private bool IsVisibleInScrollRect(
    RectTransform pRectTransform,
    ScrollRect pScrollRect,
    float pScrollRectTop,
    float pScrollRectBottom)
  {
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Implicit(((Transform) pRectTransform).localPosition), -1f);
    float num = pRectTransform.sizeDelta.y * 0.6f;
    return (double) vector2.y <= (double) pScrollRectTop + (double) num + (double) ((Component) this).transform.localPosition.y && (double) vector2.y >= (double) pScrollRectBottom - (double) num + (double) ((Component) this).transform.localPosition.y;
  }

  private void addEmptyPoolSystem()
  {
    BaseEmptyListMono pPrefab = Object.Instantiate<BaseEmptyListMono>(Resources.Load<BaseEmptyListMono>("ui/list_element_empty"), this.list_transform);
    ((Component) pPrefab).gameObject.SetActive(false);
    LayoutElement layoutElement;
    if ((double) this._element_height > 0.0 && ((Component) pPrefab).TryGetComponent<LayoutElement>(ref layoutElement))
      layoutElement.minHeight = this._element_height;
    this._pool_empty_elements = new ObjectPoolGenericMono<BaseEmptyListMono>(pPrefab, this.list_transform);
  }

  private void showElement(TMetaObject pObject)
  {
    this._pool_empty_elements.getNext().assignObject((NanoObject) pObject);
  }

  protected static IEnumerable<TMetaObject> getObjects(
    ComponentListBase<TListElement, TMetaObject, TData, TComponent> pComponentList)
  {
    foreach (TMetaObject metaObject in pComponentList.getFiltered(pComponentList._meta_type_asset.get_list().Cast<TMetaObject>()))
      yield return metaObject;
  }

  protected virtual IEnumerable<TMetaObject> getFiltered(IEnumerable<TMetaObject> pList)
  {
    switch (this.getCurrentFilter())
    {
      case ListItemsFilter.Favorites:
        foreach (TMetaObject p in pList)
        {
          if (p.isFavorite())
            yield return p;
        }
        break;
      case ListItemsFilter.Dead:
        foreach (TMetaObject p in pList)
        {
          if (p.hasDied())
            yield return p;
        }
        break;
      case ListItemsFilter.OnlyAlive:
        foreach (TMetaObject p in pList)
        {
          if (!p.hasDied())
            yield return p;
        }
        break;
      default:
        foreach (TMetaObject p in pList)
          yield return p;
        break;
    }
  }

  private void OnEnable()
  {
    this.checkCreate();
    this.showSortingTabs();
    this.show();
  }

  protected virtual void show()
  {
    if (!Config.game_loaded)
      return;
    this.clear();
    this.latest_counted = 0;
    if (this.isEmpty())
    {
      if (Object.op_Inequality((Object) this.no_items, (Object) null))
        this.no_items.SetActive(true);
    }
    else
    {
      if (Object.op_Inequality((Object) this.no_items, (Object) null))
        this.no_items.SetActive(false);
      this.showElements();
      this.latest_counted = this._pool_empty_elements.countActive();
    }
    if (Object.op_Inequality((Object) this._title_counter, (Object) null))
      this._title_counter.text = this.latest_counted.ToString();
    if (Object.op_Inequality((Object) this._favorites_counter, (Object) null))
      this._favorites_counter.text = this.latest_counted.ToString();
    if (Object.op_Inequality((Object) this._dead_counter, (Object) null))
      this._dead_counter.text = this.latest_counted.ToString();
    this._pool_empty_elements.disableInactive();
    ScrollWindow.checkElements();
  }

  public ListPool<NanoObject> getElements()
  {
    this.meta_list.Clear();
    this.meta_list.AddRange((IEnumerable<NanoObject>) this.getObjectsList());
    this.meta_list.Sort((Comparison<NanoObject>) ((a, b) => this.current_sort(a as TMetaObject, b as TMetaObject)));
    SortButton currentButton = this.sorting_tab.getCurrentButton();
    if ((currentButton != null ? (currentButton.getState() == SortButtonState.Down ? 1 : 0) : 0) != 0)
      this.meta_list.Reverse();
    return new ListPool<NanoObject>((ICollection<NanoObject>) this.meta_list);
  }

  protected void showElements()
  {
    using (ListPool<NanoObject> elements = this.getElements())
    {
      for (int index = 0; index < elements.Count; ++index)
        this.showElement(elements[index] as TMetaObject);
      if (!this.change_asset_sort_order)
        return;
      this._meta_type_asset.setListGetter(new MetaTypeListPoolAction(this.getElements));
    }
  }

  public virtual bool isEmpty()
  {
    IEnumerable<TMetaObject> objectsList = this.getObjectsList();
    return objectsList == null || !objectsList.Any<TMetaObject>();
  }

  public virtual void clear()
  {
    IReadOnlyList<BaseEmptyListMono> listTotal = this._pool_empty_elements.getListTotal();
    for (int index = 0; index < listTotal.Count; ++index)
    {
      BaseEmptyListMono pEmptyMono = listTotal[index];
      this.releaseElement(pEmptyMono);
      pEmptyMono.clearObject();
    }
    this._pool_empty_elements.clear();
    this._pool_elements.resetParent();
    this.meta_list.Clear();
    this._meta_type_asset.setListGetter((MetaTypeListPoolAction) null);
  }

  private void releaseElement(BaseEmptyListMono pEmptyMono)
  {
    if (!pEmptyMono.hasElement())
      return;
    TListElement element = (TListElement) pEmptyMono.element;
    pEmptyMono.clearElement();
    this._pool_elements.release(element);
  }

  private void debugUpdateElementNames(
    IReadOnlyList<BaseEmptyListMono> pList,
    float pScrollRectTop,
    float pScrollRectBottom)
  {
    for (int index = 0; index < pList.Count; ++index)
    {
      BaseEmptyListMono p = pList[index];
      bool tVisible = this.IsVisibleInScrollRect(p.rect_transform, this.scroll_rect, pScrollRectTop, pScrollRectBottom);
      p.debugUpdateName(tVisible);
    }
  }

  private void OnDisable() => this.clear();

  public void setShowFavoritesOnly() => this._show_items = ListItemsFilter.Favorites;

  public void setShowAll() => this._show_items = ListItemsFilter.All;

  public void setShowDeadOnly() => this._show_items = ListItemsFilter.Dead;

  public void setShowAliveOnly() => this._show_items = ListItemsFilter.OnlyAlive;

  public virtual void setDefault()
  {
  }

  public ListItemsFilter getCurrentFilter() => this._show_items;

  public void init(
    GameObject pNoItems,
    SortingTab pSortingTab,
    GameObject pListElementPrefab,
    Transform pListTransform,
    ScrollRect pScrollRect,
    Text pTitleCounter,
    Text pFavoritesCounter,
    Text pDeadCounter)
  {
    this.no_items = pNoItems;
    this.sorting_tab = pSortingTab;
    this.element_prefab = pListElementPrefab.GetComponent<TListElement>();
    this.list_transform = pListTransform;
    this.scroll_rect = pScrollRect;
    this._title_counter = pTitleCounter;
    this._favorites_counter = pFavoritesCounter;
    this._dead_counter = pDeadCounter;
  }

  public virtual bool checkRefreshWindow()
  {
    foreach (NanoObject meta in this.meta_list)
    {
      if (meta.isRekt())
        return true;
    }
    return false;
  }

  protected void genericMetaSortByAge(Comparison<TMetaObject> pAction)
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconAge", "sort_by_age", new SortButtonAction(this.show), (SortButtonAction) (() => this.current_sort = pAction));
  }

  protected void genericMetaSortByRenown(Comparison<TMetaObject> pAction)
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconRenown", "sort_by_renown", new SortButtonAction(this.show), (SortButtonAction) (() => this.current_sort = pAction));
  }

  protected void genericMetaSortByPopulation(Comparison<TMetaObject> pAction)
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconPopulation", "sort_by_members", new SortButtonAction(this.show), (SortButtonAction) (() => this.current_sort = pAction));
  }

  protected void genericMetaSortByKills(Comparison<TMetaObject> pAction)
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconKills", "sort_by_kills", new SortButtonAction(this.show), (SortButtonAction) (() => this.current_sort = pAction));
  }

  protected void genericMetaSortByDeath(Comparison<TMetaObject> pAction)
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconDead", "sort_by_dead", new SortButtonAction(this.show), (SortButtonAction) (() => this.current_sort = pAction));
  }

  protected int sortByRenown(IMetaObject p1, IMetaObject p2)
  {
    return p2.getRenown().CompareTo(p1.getRenown());
  }

  protected int sortByAge(IMetaObject p1, IMetaObject p2)
  {
    return -p2.getMetaData().created_time.CompareTo(p1.getMetaData().created_time);
  }

  public static int sortByPopulation(IMetaObject p1, IMetaObject p2)
  {
    return p2.getPopulationPeople().CompareTo(p1.getPopulationPeople());
  }

  public static int sortByKills(IMetaObject p1, IMetaObject p2)
  {
    return p2.getTotalKills().CompareTo(p1.getTotalKills());
  }

  public static int sortByDeaths(IMetaObject p1, IMetaObject p2)
  {
    return p2.getTotalDeaths().CompareTo(p1.getTotalDeaths());
  }
}
