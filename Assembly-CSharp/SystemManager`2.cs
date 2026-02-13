// Decompiled with JetBrains decompiler
// Type: SystemManager`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class SystemManager<TObject, TData> : BaseSystemManager
  where TObject : NanoObject, ILoadable<TData>, new()
  where TData : BaseSystemData, new()
{
  protected string type_id;
  private int _version;
  protected readonly Dictionary<long, TObject> dict = new Dictionary<long, TObject>();
  private HashSet<TObject> _to_dispose = new HashSet<TObject>();
  protected readonly Stack<TObject> _dead_objects = new Stack<TObject>();
  protected long _counter_recycled;

  public int version => this._version;

  public override int Count => this.dict.Count;

  public override void clear()
  {
    ++this._version;
    this.dict.Clear();
    base.clear();
  }

  public virtual void update(float pElapsed)
  {
  }

  [CanBeNull]
  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public TObject get(long pID)
  {
    if (!pID.hasValue())
      return default (TObject);
    TObject @object;
    this.dict.TryGetValue(pID, out @object);
    return @object;
  }

  public sealed override void ClearAllDisposed()
  {
    foreach (TObject pObject in this._to_dispose)
    {
      pObject.Dispose();
      this.putForRecycle(pObject);
    }
    this._to_dispose.Clear();
  }

  public void scheduleToDisposeObject(TObject pObject) => this._to_dispose.Add(pObject);

  public void scheduleToDisposeHashSet(HashSet<TObject> pHashSet)
  {
    this._to_dispose.UnionWith((IEnumerable<TObject>) pHashSet);
  }

  public void scheduleToDisposeList(List<TObject> pList)
  {
    this._to_dispose.UnionWith((IEnumerable<TObject>) pList);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected void putForRecycle(TObject pObject)
  {
    pObject.exists = false;
    this._dead_objects.Push(pObject);
  }

  [MustUseReturnValue]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected TObject newObject(long pSpecialID) => this.newObjectFromID(pSpecialID);

  [MustUseReturnValue]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected TObject newObject()
  {
    return this.newObjectFromID(World.world.map_stats.getNextId(this.type_id));
  }

  [MustUseReturnValue]
  private TObject newObjectFromID(long pID)
  {
    TData data = new TData();
    data.id = pID;
    data.created_time = World.world.getCurWorldTime();
    TData pData = data;
    TObject nextObject = this.getNextObject();
    nextObject.setData(pData);
    nextObject.created_time_unscaled = (double) Time.time;
    this.addObject(nextObject);
    return nextObject;
  }

  [MustUseReturnValue]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected TObject getNextObject()
  {
    TObject pObject;
    if (this._dead_objects.Count > 0)
    {
      pObject = this._dead_objects.Pop();
      this.revive(pObject);
    }
    else
    {
      pObject = new TObject();
      pObject.setHash(BaseSystemManager._latest_hash++);
    }
    return pObject;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void loadFromSave(List<TData> pList)
  {
    for (int index = 0; index < pList.Count; ++index)
    {
      TData p = pList[index];
      if (p.id == -1L)
        p.id = World.world.map_stats.getNextId(this.type_id);
      this.loadObject(p);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual TObject loadObject(TData pData)
  {
    TObject nextObject = this.getNextObject();
    nextObject.loadData(pData);
    this.addObject(nextObject);
    return nextObject;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected virtual void addObject(TObject pObject)
  {
    this.dict.Add(pObject.getID(), pObject);
    this.somethingChanged();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void removeObject(TObject pObject)
  {
    pObject.setAlive(false);
    this.dict.Remove(pObject.getID());
    this.scheduleToDisposeObject(pObject);
    this.somethingChanged();
  }

  public void somethingChanged()
  {
    BaseSystemManager.anything_changed = true;
    ++this._version;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected void revive(TObject pObject)
  {
    ++this._counter_recycled;
    pObject.revive();
  }

  public override void showDebugTool(DebugTool pTool)
  {
    string str1 = this.GetType().ToString();
    string str2 = str1.Substring(str1.LastIndexOf('.') + 1);
    int num = 0;
    foreach (TObject deadObject in this._dead_objects)
    {
      if (deadObject.isAlive())
        ++num;
    }
    DebugTool debugTool = pTool;
    string pT1 = str2;
    string[] strArray = new string[8];
    strArray[0] = "a: ";
    int count = this.Count;
    strArray[1] = count.ToString();
    strArray[2] = " | d: ";
    count = this._dead_objects.Count;
    strArray[3] = count.ToString();
    strArray[4] = " | r : ";
    strArray[5] = this._counter_recycled.ToString();
    strArray[6] = " | ad: ";
    strArray[7] = num.ToString();
    string pT2 = string.Concat(strArray);
    debugTool.setText(pT1, (object) pT2);
  }

  public override string debugShort()
  {
    if (this.Count == 0 && this._dead_objects.Count == 0)
      return "";
    return this._dead_objects.Count > 0 ? $"{this.type_id} [a:{this.Count}|d:{this._dead_objects.Count}]" : $"{this.type_id} [{this.Count}]";
  }
}
