// Decompiled with JetBrains decompiler
// Type: SelectedUnitRefresher`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SelectedUnitRefresher<T>
{
  private HashSet<T> _temp_set_1 = new HashSet<T>();
  private HashSet<T> _temp_set_2 = new HashSet<T>();

  public bool needsToRefresh(IReadOnlyCollection<T> pCurrent, IReadOnlyCollection<T> pNew)
  {
    foreach (T obj in (IEnumerable<T>) pCurrent)
      this._temp_set_1.Add(obj);
    foreach (T obj in (IEnumerable<T>) pNew)
      this._temp_set_2.Add(obj);
    return !this._temp_set_1.SetEquals((IEnumerable<T>) this._temp_set_2);
  }

  public void addRendered(T pPrev) => this._temp_set_1.Add(pPrev);

  public void addCurrent(T pCurrent) => this._temp_set_2.Add(pCurrent);

  public bool needsToRefresh() => !this._temp_set_1.SetEquals((IEnumerable<T>) this._temp_set_2);

  public void clear()
  {
    this._temp_set_1.Clear();
    this._temp_set_2.Clear();
  }
}
