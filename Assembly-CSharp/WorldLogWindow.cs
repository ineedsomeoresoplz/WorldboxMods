// Decompiled with JetBrains decompiler
// Type: WorldLogWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldLogWindow : MonoBehaviour
{
  private const int PADDING_ELEMENTS = 3;
  [SerializeField]
  private WorldLogElement _element_prefab_log;
  [SerializeField]
  private EmptyLogElement _element_prefab_empty;
  [SerializeField]
  private Transform _transform_content;
  [SerializeField]
  private GameObject _no_items;
  [SerializeField]
  private GridLayoutGroup _grid;
  [SerializeField]
  private ScrollRect _scroll_rect;
  [SerializeField]
  private ToggleButton _prefab;
  private ObjectPoolGenericMono<WorldLogElement> _pool;
  private ObjectPoolGenericMono<EmptyLogElement> _pool_empty;
  private HashSet<string> _selected_groups = new HashSet<string>();
  private ListPool<WorldLogMessage> _messages;
  private bool _autolayout_done;

  private void Awake()
  {
    this._pool = new ObjectPoolGenericMono<WorldLogElement>(this._element_prefab_log, this._transform_content);
    this._pool_empty = new ObjectPoolGenericMono<EmptyLogElement>(this._element_prefab_empty, this._transform_content);
    foreach (HistoryGroupAsset historyGroupAsset in AssetManager.history_groups.list)
    {
      HistoryGroupAsset tAsset = historyGroupAsset;
      Object.Instantiate<ToggleButton>(this._prefab, ((Component) this._grid).transform).init(tAsset.icon_path, tAsset.getLocaleID(), (ToggleButtonSelectAction) (pButton =>
      {
        if (pButton.is_on)
          this._selected_groups.Add(tAsset.id);
        else
          this._selected_groups.Remove(tAsset.id);
      }), new ToggleButtonAction(this.showSorted));
    }
    this._grid.cellSize = new Vector2((float) (198 / AssetManager.history_groups.list.Count), this._grid.cellSize.y);
  }

  private void OnEnable()
  {
    this.clear();
    this._messages = DBGetter.getWorldLogMessages();
    bool flag = this._messages.Any<WorldLogMessage>();
    this._no_items.SetActive(!flag);
    ((Component) this._grid).gameObject.SetActive(flag);
    if (!flag)
      return;
    this.showSorted();
  }

  private void OnRenderObject() => this._autolayout_done = true;

  private void LateUpdate()
  {
    if (!this._autolayout_done)
      return;
    IReadOnlyList<EmptyLogElement> listTotal = this._pool_empty.getListTotal();
    int num1 = int.MaxValue;
    int num2 = int.MinValue;
    float y = ((Transform) this._scroll_rect.content).localPosition.y;
    double num3 = (double) y;
    Rect rect = this._scroll_rect.viewport.rect;
    double height = (double) ((Rect) ref rect).height;
    float pScrollRectTop = (float) (num3 + height);
    for (int index = 0; index < listTotal.Count; ++index)
    {
      EmptyLogElement emptyLogElement = listTotal[index];
      if (((Component) emptyLogElement).gameObject.activeSelf)
      {
        if (this.IsVisibleInScrollRect(emptyLogElement.rect_transform, this._scroll_rect, pScrollRectTop, y))
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
      {
        EmptyLogElement emptyLogElement = listTotal[index];
        WorldLogElement element = emptyLogElement.getElement();
        if (!Object.op_Equality((Object) element, (Object) null))
        {
          this._pool.release(element);
          emptyLogElement.setElement((WorldLogElement) null);
        }
      }
    }
    for (int index = num4; index <= num5; ++index)
    {
      EmptyLogElement emptyLogElement = listTotal[index];
      if (((Component) emptyLogElement).gameObject.activeSelf && !Object.op_Inequality((Object) emptyLogElement.getElement(), (Object) null))
      {
        WorldLogElement next = this._pool.getNext();
        emptyLogElement.setElement(next);
      }
    }
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

  private void showSorted() => this.StartCoroutine(this.showSortedRoutine());

  private IEnumerator showSortedRoutine()
  {
    // ISSUE: unable to decompile the method.
  }

  private int sortByTime(WorldLogMessage p1, WorldLogMessage p2)
  {
    return p2.timestamp.CompareTo(p1.timestamp);
  }

  private void OnDisable()
  {
    this._messages?.Dispose();
    this._messages = (ListPool<WorldLogMessage>) null;
  }

  private void clear()
  {
    this._pool.clear();
    this._pool_empty.clear();
  }
}
