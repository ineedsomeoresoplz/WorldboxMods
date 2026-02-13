// Decompiled with JetBrains decompiler
// Type: WindowListBase`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WindowListBase<TListElement, TMetaObject, TData> : MonoBehaviour, IShouldRefreshWindow
  where TListElement : WindowListElementBase<TMetaObject, TData>
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
{
  public GameObject noItems;
  protected ObjectPoolGenericMono<TListElement> pool_elements;
  protected ObjectPoolGenericMono<BaseEmptyListMono> pool_empty_elements;
  public Transform transformContent;
  public TListElement element_prefab;
  public SortingTab sorting_tab;
  private bool _created;
  protected Comparison<TMetaObject> current_sort;
  public readonly List<TMetaObject> meta_list = new List<TMetaObject>();
  private ScrollWindow _scroll_window;
  private float _element_width;
  private float _element_height;
  private ScrollRect _scroll_rect;
  private bool autolayout_done;
  private const int PADDING_ELEMENTS = 3;
  private static readonly bool _debug;

  protected virtual MetaType meta_type
  {
    get => throw new NotImplementedException(((object) this).GetType().Name);
  }

  private MetaTypeAsset _meta_type_asset => AssetManager.meta_type_library.getAsset(this.meta_type);

  private void checkCreate()
  {
    if (this._created)
      return;
    this._created = true;
    this.create();
  }

  protected virtual void create()
  {
    this.pool_elements = new ObjectPoolGenericMono<TListElement>(this.element_prefab, this.transformContent);
    this._scroll_window = ((Component) this).gameObject.GetComponent<ScrollWindow>();
    this._element_width = ((Component) ((Component) (object) this.element_prefab).transform).GetComponent<RectTransform>().sizeDelta.x;
    this._element_height = ((Component) ((Component) (object) this.element_prefab).transform).GetComponent<RectTransform>().sizeDelta.y;
    this._scroll_rect = ((Component) this).gameObject.GetComponentInChildren<ScrollRect>();
    this.addEmptyPoolSystem();
  }

  private void OnRenderObject() => this.autolayout_done = true;

  private void LateUpdate()
  {
    if (!this.autolayout_done)
      return;
    IReadOnlyList<BaseEmptyListMono> listTotal = this.pool_empty_elements.getListTotal();
    int num1 = int.MaxValue;
    int num2 = int.MinValue;
    float y = ((Transform) this._scroll_rect.content).localPosition.y;
    double num3 = (double) y;
    Rect rect = this._scroll_rect.viewport.rect;
    double height = (double) ((Rect) ref rect).height;
    float pScrollRectTop = (float) (num3 + height);
    for (int index = 0; index < listTotal.Count; ++index)
    {
      BaseEmptyListMono baseEmptyListMono = listTotal[index];
      if (((Component) baseEmptyListMono).gameObject.activeSelf)
      {
        if (this.IsVisibleInScrollRect(baseEmptyListMono.rect_transform, this._scroll_rect, pScrollRectTop, y))
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
    if (!WindowListBase<TListElement, TMetaObject, TData>._debug)
      return;
    this.debugUpdateElementNames(listTotal, pScrollRectTop, y);
  }

  private void makeElementVisible(BaseEmptyListMono pEmptyMono)
  {
    TListElement next = this.pool_elements.getNext();
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
    return (double) vector2.y <= (double) pScrollRectTop + (double) num && (double) vector2.y >= (double) pScrollRectBottom - (double) num;
  }

  private void addEmptyPoolSystem()
  {
    this.pool_empty_elements = new ObjectPoolGenericMono<BaseEmptyListMono>(Resources.Load<BaseEmptyListMono>("ui/list_element_empty"), this.transformContent);
  }

  private void showElement(TMetaObject pObject)
  {
    this.pool_empty_elements.getNext().assignObject((NanoObject) pObject);
  }

  protected virtual IEnumerable<TMetaObject> getObjects()
  {
    return this._meta_type_asset.get_list().Cast<TMetaObject>();
  }

  private void OnEnable()
  {
    this.checkCreate();
    this.show();
  }

  private void show()
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
    this.pool_empty_elements.disableInactive();
    ScrollWindow.checkElements();
  }

  private ListPool<TMetaObject> getElements()
  {
    this.meta_list.Clear();
    this.meta_list.AddRange(this.getObjects());
    this.meta_list.Sort(this.current_sort);
    SortButton currentButton = this.sorting_tab.getCurrentButton();
    if ((currentButton != null ? (currentButton.getState() == SortButtonState.Down ? 1 : 0) : 0) != 0)
      this.meta_list.Reverse();
    return new ListPool<TMetaObject>((ICollection<TMetaObject>) this.meta_list);
  }

  private void showElements()
  {
    using (ListPool<TMetaObject> elements = this.getElements())
    {
      for (int index = 0; index < elements.Count; ++index)
        this.showElement(elements[index]);
    }
  }

  private bool isEmpty()
  {
    IEnumerable<TMetaObject> objects = this.getObjects();
    return objects == null || objects.Count<TMetaObject>() == 0;
  }

  private void clear()
  {
    IReadOnlyList<BaseEmptyListMono> listTotal = this.pool_empty_elements.getListTotal();
    for (int index = 0; index < listTotal.Count; ++index)
    {
      BaseEmptyListMono pEmptyMono = listTotal[index];
      this.releaseElement(pEmptyMono);
      pEmptyMono.clearObject();
    }
    this.pool_empty_elements.clear();
    this.pool_elements.resetParent();
    this.meta_list.Clear();
  }

  private void releaseElement(BaseEmptyListMono pEmptyMono)
  {
    if (!pEmptyMono.hasElement())
      return;
    TListElement element = (TListElement) pEmptyMono.element;
    pEmptyMono.clearElement();
    this.pool_elements.release(element);
  }

  private void debugUpdateElementNames(
    IReadOnlyList<BaseEmptyListMono> pList,
    float pScrollRectTop,
    float pScrollRectBottom)
  {
    for (int index = 0; index < pList.Count; ++index)
    {
      BaseEmptyListMono p = pList[index];
      bool tVisible = this.IsVisibleInScrollRect(p.rect_transform, this._scroll_rect, pScrollRectTop, pScrollRectBottom);
      p.debugUpdateName(tVisible);
    }
  }

  private void OnDisable() => this.clear();

  public virtual bool checkRefreshWindow()
  {
    foreach (TMetaObject meta in this.meta_list)
    {
      if (meta.isRekt())
        return true;
    }
    return false;
  }
}
