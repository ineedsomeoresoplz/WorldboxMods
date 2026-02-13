// Decompiled with JetBrains decompiler
// Type: Batch`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Batch<T>
{
  internal ObjectContainer<T> main;
  internal bool free_slots;
  private List<ObjectContainer<T>> containers = new List<ObjectContainer<T>>();
  internal List<Job<T>> jobs_pre = new List<Job<T>>();
  internal List<Job<T>> jobs_post = new List<Job<T>>();
  internal List<Job<T>> jobs_parallel = new List<Job<T>>();
  protected List<T> _list;
  protected T[] _array;
  protected int _count;
  protected float _elapsed;
  protected ObjectContainer<T> _cur_container;
  internal JobUpdater clearParallelResults;
  internal JobUpdater applyParallelResults;
  public int batch_id;

  public Batch()
  {
    this.createJobs();
    this.createHelpers();
  }

  public void updateJobsPre(float pElapsed)
  {
    this._elapsed = pElapsed;
    List<Job<T>> jobsPre = this.jobs_pre;
    int count = jobsPre.Count;
    for (int index = 0; index < count; ++index)
    {
      Job<T> pObj = jobsPre[index];
      this._cur_container = pObj.container;
      if (pObj.current_skips > 0)
        --pObj.current_skips;
      else
        this.runUpdater(pObj);
    }
  }

  public void updateJobsParallel(float pElapsed)
  {
    List<Job<T>> jobsParallel = this.jobs_parallel;
    int count = jobsParallel.Count;
    for (int index = 0; index < count; ++index)
    {
      Job<T> job = jobsParallel[index];
      this._cur_container = job.container;
      job.job_updater();
    }
  }

  public void updateJobsPost(float pElapsed)
  {
    this._elapsed = pElapsed;
    List<Job<T>> jobsPost = this.jobs_post;
    int count = jobsPost.Count;
    for (int index = 0; index < count; ++index)
    {
      Job<T> pObj = jobsPost[index];
      this._cur_container = pObj.container;
      if (pObj.current_skips > 0)
        --pObj.current_skips;
      else
        this.runUpdater(pObj);
    }
  }

  private void runUpdater(Job<T> pObj)
  {
    double sinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
    pObj.job_updater();
    if (pObj.random_tick_skips > 0)
      pObj.current_skips = Randy.randomInt(0, pObj.random_tick_skips);
    double num = Time.realtimeSinceStartupAsDouble - sinceStartupAsDouble;
    pObj.time_benchmark += num;
    pObj.counter += this._cur_container.Count;
  }

  internal virtual void prepare()
  {
    for (int index = 0; index < this.containers.Count; ++index)
      this.containers[index].doChecks();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected void createJob(
    out ObjectContainer<T> pContainer,
    JobUpdater pJobUpdater,
    JobType pType,
    string pID,
    int pRandomTickSkips = 0)
  {
    pContainer = new ObjectContainer<T>();
    pContainer.prepareArray(JobConst.MAX_ELEMENTS);
    this.containers.Add(pContainer);
    if (pJobUpdater == null)
      return;
    this.addJob(pContainer, pJobUpdater, pType, pID, pRandomTickSkips);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected void addJob(
    ObjectContainer<T> pContainer,
    JobUpdater pJobUpdater,
    JobType pType,
    string pID,
    int pRandomTickSkips = 0)
  {
    switch (pType)
    {
      case JobType.Pre:
        this.putJob(pContainer, pJobUpdater, this.jobs_pre, pID, pRandomTickSkips);
        break;
      case JobType.Post:
        this.putJob(pContainer, pJobUpdater, this.jobs_post, pID, pRandomTickSkips);
        break;
      case JobType.Parallel:
        this.putJob(pContainer, pJobUpdater, this.jobs_parallel, pID, pRandomTickSkips);
        break;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void putJob(
    ObjectContainer<T> pContainer,
    JobUpdater pJobUpdater,
    List<Job<T>> pListJobs,
    string pID,
    int pRandomTickSkips = 0)
  {
    pListJobs.Add(new Job<T>()
    {
      container = pContainer,
      job_updater = pJobUpdater,
      id = pID,
      random_tick_skips = pRandomTickSkips
    });
  }

  internal virtual void clear()
  {
    for (int index = 0; index < this.containers.Count; ++index)
      this.containers[index].Clear();
    if (this._array == null)
      return;
    Array.Clear((Array) this._array, 0, this._array.Length);
  }

  internal virtual void remove(T pObject)
  {
    for (int index = 0; index < this.containers.Count; ++index)
      this.containers[index].Remove(pObject);
  }

  internal virtual void add(T pObject) => this.main.Add(pObject);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected bool check(ObjectContainer<T> pContainer)
  {
    if (pContainer.Count <= 0 && !pContainer.isDirtyContainer())
      return false;
    pContainer.checkAddRemove();
    this._array = pContainer.getFastSimpleArray();
    this._count = pContainer.Count;
    return true;
  }

  protected virtual void createJobs()
  {
  }

  protected virtual void createHelpers()
  {
  }

  public virtual void clearHelperLists()
  {
  }

  public void debug(DebugTool pTool) => pTool.setText("total", (object) this.main.Count);
}
