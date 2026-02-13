// Decompiled with JetBrains decompiler
// Type: WindowListBaseActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WindowListBaseActor : MonoBehaviour, IComponentList, IShouldRefreshWindow
{
  public GameObject noItems;
  protected ObjectPoolGenericMono<PrefabUnitElement> pool_elements;
  public Transform transformContent;
  public PrefabUnitElement element_prefab;
  public SortingTab sorting_tab;
  [SerializeField]
  protected Text _title_counter;
  private bool _created;
  protected Comparison<Actor> current_sort;
  internal ScrollWindow _scrollWindow;
  public readonly List<NanoObject> meta_list = new List<NanoObject>();

  private void checkCreate()
  {
    if (this._created)
      return;
    this._created = true;
    this.create();
  }

  protected virtual void create()
  {
    this.pool_elements = new ObjectPoolGenericMono<PrefabUnitElement>(this.element_prefab, this.transformContent);
    this._scrollWindow = ((Component) this).gameObject.GetComponent<ScrollWindow>();
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
    this.noItems = pNoItems;
    this.sorting_tab = pSortingTab;
    this.element_prefab = pListElementPrefab.GetComponent<PrefabUnitElement>();
    this.transformContent = pListTransform;
    this._title_counter = pTitleCounter;
  }

  private void showElement(Actor pObject) => this.pool_elements.getNext().show(pObject);

  protected virtual List<Actor> getObjects() => (List<Actor>) null;

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
    if (this.isEmpty())
    {
      this.noItems.SetActive(true);
    }
    else
    {
      this.noItems.SetActive(false);
      this.showElements();
    }
    this.pool_elements.disableInactive();
    ScrollWindow.checkElements();
  }

  public ListPool<NanoObject> getElements()
  {
    this.meta_list.Clear();
    this.meta_list.AddRange((IEnumerable<NanoObject>) this.getObjects());
    this.meta_list.Sort((Comparison<NanoObject>) ((a, b) => this.current_sort(a as Actor, b as Actor)));
    SortButton currentButton = this.sorting_tab.getCurrentButton();
    if ((currentButton != null ? (currentButton.getState() == SortButtonState.Down ? 1 : 0) : 0) != 0)
      this.meta_list.Reverse();
    return new ListPool<NanoObject>((ICollection<NanoObject>) this.meta_list);
  }

  private void showElements()
  {
    using (ListPool<NanoObject> elements = this.getElements())
    {
      for (int index = 0; index < elements.Count; ++index)
        this.showElement(elements[index] as Actor);
      AssetManager.meta_type_library.getAsset(MetaType.Unit).setListGetter(new MetaTypeListPoolAction(this.getElements));
    }
  }

  private bool isEmpty()
  {
    List<Actor> objects = this.getObjects();
    return objects == null || objects.Count == 0;
  }

  private void clear()
  {
    this.pool_elements.clear(false);
    this.meta_list.Clear();
    AssetManager.meta_type_library.getAsset(MetaType.Unit).setListGetter((MetaTypeListPoolAction) null);
  }

  private void OnDisable() => this.clear();

  public void setShowFavoritesOnly() => throw new NotImplementedException();

  public void setShowDeadOnly() => throw new NotImplementedException();

  public void setShowAliveOnly() => throw new NotImplementedException();

  public void setShowAll() => throw new NotImplementedException();

  public void setDefault() => throw new NotImplementedException();

  public virtual bool checkRefreshWindow()
  {
    foreach (NanoObject meta in this.meta_list)
    {
      if (meta.isRekt())
        return true;
    }
    return false;
  }
}
