// Decompiled with JetBrains decompiler
// Type: SimSystemManager`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public abstract class SimSystemManager<TObject, TData> : 
  SystemManager<TObject, TData>,
  IEnumerable<TObject>,
  IEnumerable
  where TObject : BaseSimObject, ILoadable<TData>, new()
  where TData : BaseObjectData, new()
{
  private readonly ObjectContainer<TObject> _container = new ObjectContainer<TObject>();
  private HashSet<TObject> _to_destroy_objects = new HashSet<TObject>();
  public bool event_destroy;
  public bool event_houses;

  public void prepareArray() => this._container.prepareArray(this.Count);

  public override void loadFromSave(List<TData> pList)
  {
    base.loadFromSave(pList);
    this._container.checkAddRemove();
  }

  protected override void addObject(TObject pObject)
  {
    base.addObject(pObject);
    this._container.Add(pObject);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override void removeObject(TObject pObject)
  {
    base.removeObject(pObject);
    this._container.Remove(pObject);
  }

  public override void clear()
  {
    base.clear();
    this._container.Clear();
  }

  public void checkContainer() => this._container.checkAddRemove();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IEnumerator<TObject> GetEnumerator() => this._container.GetEnumerator();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public string debugContainer() => this._container.debug();

  public TObject GetRandom() => this._container.GetRandom();

  public List<TObject> getSimpleList() => this._container.getSimpleList();

  public TObject[] getSimpleArray() => this._container.getSimpleArray();

  internal virtual void scheduleDestroyOnPlay(TObject pObject)
  {
    this._to_destroy_objects.Add(pObject);
  }

  internal void scheduleDestroyAllOnWorldClear()
  {
    foreach (TObject @object in this)
      this._to_destroy_objects.Add(@object);
  }

  internal bool checkObjectsToDestroy()
  {
    if (this._to_destroy_objects.Count <= 0)
      return false;
    foreach (TObject toDestroyObject in this._to_destroy_objects)
      this.destroyObject(toDestroyObject);
    this._to_destroy_objects.Clear();
    this.checkContainer();
    this.event_destroy = true;
    return true;
  }

  protected virtual void destroyObject(TObject pObject)
  {
  }
}
