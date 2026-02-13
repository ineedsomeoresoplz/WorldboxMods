// Decompiled with JetBrains decompiler
// Type: CategoryData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityPools;

#nullable disable
public class CategoryData : IDisposable
{
  private LinkedList<Dictionary<string, long>> _data = new LinkedList<Dictionary<string, long>>();
  internal ListPool<object> db_list;

  public LinkedListNode<Dictionary<string, long>> AddLast(Dictionary<string, long> pDict)
  {
    return this._data.AddLast(pDict);
  }

  public LinkedListNode<Dictionary<string, long>> Last => this._data.Last;

  public int Count => this._data.Count;

  public void Clear()
  {
    foreach (Dictionary<string, long> dictionary in this._data)
      UnsafeCollectionPool<Dictionary<string, long>, KeyValuePair<string, long>>.Release(dictionary);
    this._data.Clear();
    this.db_list?.Dispose();
    this.db_list = (ListPool<object>) null;
  }

  public void Dispose()
  {
    this.Clear();
    this._data = (LinkedList<Dictionary<string, long>>) null;
  }
}
