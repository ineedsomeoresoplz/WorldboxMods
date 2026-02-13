// Decompiled with JetBrains decompiler
// Type: SelectedObjectsGraph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SelectedObjectsGraph : IEnumerable<NanoObject>, IEnumerable
{
  private NanoObject[] _selected_objects = new NanoObject[3];
  private int _selected_count;
  private bool _dirty;

  public int Count
  {
    get
    {
      if (this._dirty)
      {
        this._selected_count = 0;
        for (int index = 0; index < this._selected_objects.Length; ++index)
        {
          if (this._selected_objects[index] != null)
            ++this._selected_count;
        }
        this._dirty = false;
      }
      return this._selected_count;
    }
  }

  public void Clear()
  {
    Array.Clear((Array) this._selected_objects, 0, this._selected_objects.Length);
    this._dirty = true;
  }

  public void Add(NanoObject pObject)
  {
    if (pObject == null)
      return;
    for (int index = 0; index < this._selected_objects.Length; ++index)
    {
      if (this._selected_objects[index] == null)
      {
        this._selected_objects[index] = pObject;
        this._dirty = true;
        return;
      }
    }
    Debug.LogWarning((object) "SelectedObjects is full, cannot add more objects.");
  }

  public void Remove(NanoObject pObject)
  {
    if (pObject == null)
      return;
    for (int index = 0; index < this._selected_objects.Length; ++index)
    {
      if (this._selected_objects[index] == pObject)
      {
        this._selected_objects[index] = (NanoObject) null;
        this._dirty = true;
        return;
      }
    }
    Debug.LogWarning((object) "SelectedObjects does not contain the object to remove.");
  }

  public IEnumerator<NanoObject> GetEnumerator()
  {
    for (int i = 0; i < this._selected_objects.Length; ++i)
      yield return this._selected_objects[i];
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public NanoObject First()
  {
    foreach (NanoObject nanoObject in this)
    {
      if (nanoObject != null)
        return nanoObject;
    }
    return (NanoObject) null;
  }

  public bool Contains(NanoObject pObject)
  {
    foreach (NanoObject nanoObject in this)
    {
      if (nanoObject == pObject)
        return true;
    }
    return false;
  }

  public NanoObject this[int index] => this._selected_objects[index];

  public void RemoveWhere(Func<NanoObject, bool> predicate)
  {
    for (int index = 0; index < this._selected_objects.Length; ++index)
    {
      if (this._selected_objects[index] != null && predicate(this._selected_objects[index]))
        this._selected_objects[index] = (NanoObject) null;
    }
    this._dirty = true;
  }
}
