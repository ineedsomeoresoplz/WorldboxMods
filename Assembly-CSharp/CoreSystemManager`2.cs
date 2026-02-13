// Decompiled with JetBrains decompiler
// Type: CoreSystemManager`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public abstract class CoreSystemManager<TObject, TData> : 
  SystemManager<TObject, TData>,
  IEnumerable<TObject>,
  IEnumerable
  where TObject : CoreSystemObject<TData>, new()
  where TData : BaseSystemData, new()
{
  public readonly List<TObject> list = new List<TObject>(512 /*0x0200*/);
  private readonly HashSet<TObject> _hashset = new HashSet<TObject>(512 /*0x0200*/);
  private bool _dirty_list;

  private void setListDirty() => this._dirty_list = true;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IEnumerator<TObject> GetEnumerator()
  {
    return (IEnumerator<TObject>) this._hashset.GetEnumerator();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public override int Count => this._hashset.Count;

  public override void checkLists()
  {
    if (!this._dirty_list)
      return;
    this._dirty_list = false;
    this.list.Clear();
    this.list.AddRange((IEnumerable<TObject>) this._hashset);
  }

  public override void loadFromSave(List<TData> pList)
  {
    base.loadFromSave(pList);
    this.setListDirty();
  }

  protected override void addObject(TObject pObject)
  {
    base.addObject(pObject);
    this.setListDirty();
    this._hashset.Add(pObject);
  }

  public virtual List<TData> save(List<TObject> pList = null)
  {
    if (pList == null)
      pList = this.list;
    List<TData> dataList = new List<TData>();
    foreach (TObject p in pList)
    {
      if (p.isAlive())
      {
        p.save();
        dataList.Add(p.data);
      }
    }
    return dataList;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override void removeObject(TObject pObject)
  {
    base.removeObject(pObject);
    this._hashset.Remove(pObject);
    this.setListDirty();
  }

  public TObject getRandom()
  {
    foreach (TObject random in this.list.LoopRandom<TObject>())
    {
      if (this._hashset.Contains(random))
        return random;
    }
    return default (TObject);
  }

  public override void clear()
  {
    foreach (TObject @object in this._hashset)
      @object.setAlive(false);
    this.scheduleToDisposeHashSet(this._hashset);
    this._hashset.Clear();
    this.list.Clear();
    this.setListDirty();
    base.clear();
  }
}
