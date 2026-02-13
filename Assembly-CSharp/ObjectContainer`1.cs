// Decompiled with JetBrains decompiler
// Type: ObjectContainer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public class ObjectContainer<T> : IEnumerable<T>, IEnumerable
{
  private HashSet<T> _to_remove = new HashSet<T>();
  private HashSet<T> _to_add = new HashSet<T>();
  private HashSet<T> _hashSet = new HashSet<T>();
  private T[] _array;
  private int _array_count;
  private bool _dirty_list;
  private bool _dirty_array;
  private bool _dirty_container;
  private readonly List<T> _simple_list = new List<T>();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._hashSet.GetEnumerator();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Contains(T pObject, bool pCheckToAddRemove = true)
  {
    if (pCheckToAddRemove)
    {
      if (this._to_add.Contains(pObject))
        return true;
      if (this._to_remove.Contains(pObject))
        return false;
    }
    return this._hashSet.Contains(pObject);
  }

  public void Clear()
  {
    this._hashSet.Clear();
    this._to_add.Clear();
    this._to_remove.Clear();
    this._simple_list.Clear();
    this._dirty_list = false;
    this._dirty_array = false;
    this._dirty_container = false;
    if (this._array_count <= 0)
      return;
    Array.Clear((Array) this._array, 0, this._array.Length);
    this._array_count = 0;
  }

  public void doChecks()
  {
    this.checkAddRemove();
    this.checkSimpleListDirty();
  }

  public bool isDirtyContainer() => this._dirty_container;

  public void checkAddRemove()
  {
    if (this._to_add.Count > 0)
    {
      if (this._hashSet.Count == 0)
      {
        HashSet<T> hashSet = this._hashSet;
        this._hashSet = this._to_add;
        this._to_add = hashSet;
      }
      else
      {
        this._hashSet.UnionWith((IEnumerable<T>) this._to_add);
        this._to_add.Clear();
      }
      this._dirty_list = true;
      this._dirty_array = true;
    }
    if (this._to_remove.Count > 0)
    {
      this._hashSet.ExceptWith((IEnumerable<T>) this._to_remove);
      this._to_remove.Clear();
      this._dirty_list = true;
      this._dirty_array = true;
    }
    this._dirty_container = false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public List<T> getSimpleList()
  {
    this.checkSimpleListDirty();
    return this._simple_list;
  }

  public T[] getFastSimpleArray()
  {
    if (this._dirty_array)
    {
      this._dirty_array = false;
      this._hashSet.CopyTo(this._array, 0);
      this._array_count = this._hashSet.Count;
    }
    return this._array;
  }

  public T[] getSimpleArray()
  {
    if (this._dirty_array)
    {
      this._dirty_array = false;
      this.prepareArray(this._hashSet.Count);
      this._hashSet.CopyTo(this._array, 0);
      this._array_count = this._hashSet.Count;
    }
    return this._array;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void checkSimpleListDirty()
  {
    if (!this._dirty_list)
      return;
    List<T> simpleList = this._simple_list;
    simpleList.Clear();
    simpleList.AddRange((IEnumerable<T>) this._hashSet);
    this._dirty_list = false;
  }

  public void prepareArray(int pSize)
  {
    this._array = Toolbox.checkArraySize<T>(this._array, pSize);
  }

  [CanBeNull]
  public T GetRandom()
  {
    this.checkAddRemove();
    List<T> simpleList = this.getSimpleList();
    return simpleList.Count == 0 ? default (T) : simpleList.GetRandom<T>();
  }

  public int Count
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._hashSet.Count;
  }

  public string debug()
  {
    return $"{this._hashSet.Count.ToString()}/{this._to_add.Count.ToString()}/{this._to_remove.Count.ToString()}";
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Add(T pObject)
  {
    this._to_add.Add(pObject);
    this._to_remove.Remove(pObject);
    this._dirty_container = true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Remove(T pObject)
  {
    this._to_remove.Add(pObject);
    this._to_add.Remove(pObject);
    this._dirty_container = true;
  }
}
