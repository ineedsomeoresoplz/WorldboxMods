// Decompiled with JetBrains decompiler
// Type: MetaSystemManager`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using db.tables;
using System;
using System.Collections.Generic;

#nullable disable
public abstract class MetaSystemManager<TObject, TData> : CoreSystemManager<TObject, TData>
  where TObject : MetaObject<TData>, new()
  where TData : MetaObjectData, new()
{
  private bool _dirty_units;
  protected Dictionary<TObject, MetaObjectCounter<TObject, TData>> _counters_dict = new Dictionary<TObject, MetaObjectCounter<TObject, TData>>();
  protected List<MetaObjectCounter<TObject, TData>> _counters_list = new List<MetaObjectCounter<TObject, TData>>();
  private List<TObject> _to_remove = new List<TObject>();

  protected void countMetaObject(TObject pMetaObject)
  {
    if (!this._counters_dict.TryGetValue(pMetaObject, out MetaObjectCounter<TObject, TData> _))
    {
      MetaObjectCounter<TObject, TData> metaObjectCounter = new MetaObjectCounter<TObject, TData>(pMetaObject);
      this._counters_dict.Add(pMetaObject, metaObjectCounter);
      this._counters_list.Add(metaObjectCounter);
    }
    ++this._counters_dict[pMetaObject].amount;
  }

  protected TObject getMostUsedMetaObject()
  {
    if (!this._counters_list.Any<MetaObjectCounter<TObject, TData>>())
      return default (TObject);
    this._counters_list.Sort(new Comparison<MetaObjectCounter<TObject, TData>>(this.sortByAmount));
    TObject metaObject = this._counters_list[0].meta_object;
    this.clearCounters();
    return metaObject;
  }

  private int sortByAmount(
    MetaObjectCounter<TObject, TData> pO1,
    MetaObjectCounter<TObject, TData> pO2)
  {
    return pO2.amount.CompareTo(pO1.amount);
  }

  private void clearCounters()
  {
    this._counters_dict.Clear();
    this._counters_list.Clear();
  }

  public override void parallelDirtyUnitsCheck() => this.beginChecksUnits();

  public void beginChecksUnits()
  {
    if (!this._dirty_units)
      return;
    this.clearAllUnitLists();
    this.updateDirtyUnits();
    this.finishDirtyUnits();
  }

  public override void startCollectHistoryData()
  {
    HistoryMetaDataAsset historyMetaDataAsset = AssetManager.history_meta_data_library.get(this.type_id);
    if (historyMetaDataAsset.collector == null)
      return;
    foreach (TObject pNanoObject in (CoreSystemManager<TObject, TData>) this)
    {
      foreach (HistoryDataCollector invocation in historyMetaDataAsset.collector.GetInvocationList())
      {
        HistoryTable pObject = invocation((NanoObject) pNanoObject);
        pObject.timestamp = (long) World.world.map_stats.history_current_year;
        DBInserter.insertData(pObject, this.type_id);
      }
    }
  }

  public override void clearLastYearStats()
  {
    foreach (TObject @object in (CoreSystemManager<TObject, TData>) this)
      @object.clearLastYearStats();
  }

  public override bool isUnitsDirty() => this._dirty_units;

  protected virtual void finishDirtyUnits()
  {
    this._dirty_units = false;
    foreach (TObject @object in (CoreSystemManager<TObject, TData>) this)
    {
      if (@object.isDirtyUnits())
        @object.unDirty();
    }
  }

  public override void checkDeadObjects()
  {
    base.checkDeadObjects();
    foreach (TObject @object in (CoreSystemManager<TObject, TData>) this)
    {
      if (@object.isReadyForRemoval())
        this._to_remove.Add(@object);
    }
    foreach (TObject pObject in this._to_remove)
    {
      pObject.triggerOnRemoveObject();
      this.removeObject(pObject);
    }
    this._to_remove.Clear();
  }

  protected abstract void updateDirtyUnits();

  public virtual void unitDied(TObject pObject) => this.setDirtyUnits(pObject);

  public virtual void unitAdded(TObject pObject) => this.setDirtyUnits(pObject);

  protected override void addObject(TObject pObject)
  {
    base.addObject(pObject);
    this.setDirtyUnits(pObject);
  }

  public void setDirtyUnits(TObject pObject)
  {
    pObject?.setDirty();
    this._dirty_units = true;
  }

  private void clearAllUnitLists()
  {
    foreach (TObject @object in (CoreSystemManager<TObject, TData>) this)
    {
      if (@object.isDirtyUnits())
        @object.clearListUnits();
    }
  }
}
