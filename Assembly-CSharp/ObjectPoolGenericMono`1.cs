// Decompiled with JetBrains decompiler
// Type: ObjectPoolGenericMono`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ObjectPoolGenericMono<T> where T : Component
{
  private readonly List<T> _elements_total = new List<T>();
  private readonly Queue<T> _elements_inactive = new Queue<T>();
  private readonly T _prefab;
  private readonly Transform _parent_transform;

  public ObjectPoolGenericMono(T pPrefab, Transform pParentTransform)
  {
    this._prefab = pPrefab;
    this._parent_transform = pParentTransform;
  }

  public void clear(bool pDisable = true)
  {
    this._elements_inactive.Clear();
    this.sortElements();
    foreach (T pElement in this._elements_total)
      this.release(pElement, pDisable);
  }

  private void sortElements()
  {
    this._elements_total.Sort((Comparison<T>) ((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex())));
  }

  public T getFirstActive() => this._elements_total[0];

  public IReadOnlyList<T> getListTotal() => (IReadOnlyList<T>) this._elements_total;

  public void disableInactive()
  {
    foreach (T obj in this._elements_inactive)
    {
      if (obj.gameObject.activeSelf)
        obj.gameObject.SetActive(false);
    }
  }

  public T getNext()
  {
    T newOrActivate = this.getNewOrActivate();
    this.checkActive(newOrActivate);
    return newOrActivate;
  }

  private T getNewOrActivate()
  {
    T newOrActivate;
    if (this._elements_inactive.Count > 0)
    {
      newOrActivate = this._elements_inactive.Dequeue();
    }
    else
    {
      newOrActivate = Object.Instantiate<T>(this._prefab, this._parent_transform);
      this._elements_total.Add(newOrActivate);
      ((Object) (object) newOrActivate).name = $"{typeof (T)?.ToString()} {this._elements_total.Count.ToString()} {newOrActivate.transform.GetSiblingIndex().ToString()}";
    }
    return newOrActivate;
  }

  public void release(T pElement, bool pDisable = true)
  {
    if (((Component) this._parent_transform).gameObject.activeInHierarchy)
      pElement.transform.SetAsLastSibling();
    if (!this._elements_inactive.Contains(pElement))
      this._elements_inactive.Enqueue(pElement);
    if (!(pElement.gameObject.activeSelf & pDisable))
      return;
    pElement.gameObject.SetActive(false);
  }

  private void checkActive(T pElement)
  {
    if (pElement.gameObject.activeSelf)
      return;
    pElement.gameObject.SetActive(true);
  }

  public int countTotal() => this._elements_total.Count;

  public int countInactive() => this._elements_inactive.Count;

  public int countActive() => this._elements_total.Count - this._elements_inactive.Count;

  public void resetParent()
  {
    foreach (T pElement in this._elements_total)
      this.resetParent(pElement);
  }

  public void resetParent(T pElement)
  {
    if (!((Component) this._parent_transform).gameObject.activeInHierarchy)
      return;
    pElement.transform.SetParent(this._parent_transform);
  }
}
