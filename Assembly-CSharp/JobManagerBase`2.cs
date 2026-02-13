// Decompiled with JetBrains decompiler
// Type: JobManagerBase`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
public class JobManagerBase<TBatch, T> where TBatch : Batch<T>, new()
{
  protected readonly List<TBatch> _batches_active = new List<TBatch>();
  private readonly Stack<TBatch> _batches_free = new Stack<TBatch>();
  public string id;
  public string benchmark_id;
  private Dictionary<string, double> _dict_benchmark_time = new Dictionary<string, double>();
  private Dictionary<string, int> _dict_benchmark_counter = new Dictionary<string, int>();

  public JobManagerBase(string pID) => this.id = pID;

  public void updateBase(float pElapsed)
  {
    this.clearJobBenchmarks();
    this.updateBaseJobsPre(pElapsed);
    this.updateBaseJobsParallel(pElapsed);
    this.updateBaseJobsPost(pElapsed);
    this.saveJobBenchmarks();
  }

  private void clearJobBenchmarks()
  {
    if (!Bench.bench_enabled)
      return;
    for (int index1 = 0; index1 < this._batches_active.Count; ++index1)
    {
      TBatch batch = this._batches_active[index1];
      for (int index2 = 0; index2 < batch.jobs_pre.Count; ++index2)
      {
        Job<T> job = batch.jobs_pre[index2];
        job.time_benchmark = 0.0;
        job.counter = 0;
      }
      for (int index3 = 0; index3 < batch.jobs_post.Count; ++index3)
      {
        Job<T> job = batch.jobs_post[index3];
        job.time_benchmark = 0.0;
        job.counter = 0;
      }
    }
  }

  private void saveJobBenchmarks()
  {
    if (!Bench.bench_enabled)
      return;
    this._dict_benchmark_time.Clear();
    this._dict_benchmark_counter.Clear();
    for (int index = 0; index < this._batches_active.Count; ++index)
    {
      TBatch batch = this._batches_active[index];
      this.checkListForBenchmark(batch.jobs_pre);
      this.checkListForBenchmark(batch.jobs_post);
    }
    foreach (KeyValuePair<string, double> keyValuePair in this._dict_benchmark_time)
    {
      string str1;
      double num;
      keyValuePair.Deconstruct(ref str1, ref num);
      string str2 = str1;
      double pValue = num;
      int pCounter = this._dict_benchmark_counter[str2];
      Bench.benchSave(str2, pValue, pCounter, this.benchmark_id);
      Bench.saveAverageCounter(str2, this.benchmark_id);
    }
  }

  private void checkListForBenchmark(List<Job<T>> pList)
  {
    for (int index = 0; index < pList.Count; ++index)
    {
      Job<T> p = pList[index];
      if (!this._dict_benchmark_time.ContainsKey(p.id))
      {
        this._dict_benchmark_time.Add(p.id, 0.0);
        this._dict_benchmark_counter.Add(p.id, 0);
      }
      this._dict_benchmark_time[p.id] += p.time_benchmark;
      this._dict_benchmark_counter[p.id] += p.counter;
    }
  }

  internal void updateBaseJobsPre(float pElapsed)
  {
    for (int index = 0; index < this._batches_active.Count; ++index)
      this._batches_active[index].updateJobsPre(pElapsed);
  }

  internal void updateBaseJobsPost(float pElapsed)
  {
    for (int index = 0; index < this._batches_active.Count; ++index)
      this._batches_active[index].updateJobsPost(pElapsed);
  }

  internal void updateBaseJobsParallel(float pElapsed)
  {
    this.clearParallelResults();
    Bench.bench("update_jobs_parallel", this.benchmark_id);
    if (Config.parallel_jobs_updater)
    {
      Parallel.ForEach<TBatch>((IEnumerable<TBatch>) this._batches_active, World.world.parallel_options, (Action<TBatch>) (pBatch => pBatch.updateJobsParallel(pElapsed)));
    }
    else
    {
      List<TBatch> batchesActive = this._batches_active;
      int count = batchesActive.Count;
      for (int index = 0; index < count; ++index)
        batchesActive[index].updateJobsParallel(pElapsed);
    }
    Bench.benchEnd("update_jobs_parallel", this.benchmark_id);
    this.applyParallelResults();
  }

  internal void clearParallelResults()
  {
    Bench.bench("clear_parallel_results", this.benchmark_id);
    for (int index = 0; index < this._batches_active.Count; ++index)
    {
      JobUpdater clearParallelResults = this._batches_active[index].clearParallelResults;
      if (clearParallelResults != null)
        clearParallelResults();
    }
    Bench.benchEnd("clear_parallel_results", this.benchmark_id);
  }

  internal void applyParallelResults()
  {
    Bench.bench("apply_parallel_results", this.benchmark_id);
    for (int index = 0; index < this._batches_active.Count; ++index)
    {
      JobUpdater applyParallelResults = this._batches_active[index].applyParallelResults;
      if (applyParallelResults != null)
        applyParallelResults();
    }
    Bench.benchEnd("apply_parallel_results", this.benchmark_id);
  }

  internal void removeObject(T pObject, TBatch pBatch)
  {
    pBatch.remove(pObject);
    this.checkFree(pBatch);
  }

  protected TBatch newBatch()
  {
    TBatch batch = new TBatch();
    this._batches_active.Add(batch);
    return batch;
  }

  internal virtual void addNewObject(T pObject)
  {
    TBatch batch = this.getBatch();
    batch.add(pObject);
    batch.main.checkAddRemove();
    if (batch.main.Count < JobConst.MAX_ELEMENTS)
      return;
    batch.free_slots = false;
    this._batches_free.Pop();
  }

  internal TBatch getBatch()
  {
    if (this._batches_free.Count == 0)
    {
      TBatch pBatch = this.newBatch();
      pBatch.batch_id = this._batches_active.Count;
      this.makeFree(pBatch);
      return pBatch;
    }
    TBatch batch = this._batches_free.Peek();
    if (batch.main.Count == 0)
      this._batches_active.Add(batch);
    return batch;
  }

  protected void checkFree(TBatch pBatch)
  {
    pBatch.main.checkAddRemove();
    if (pBatch.main.Count < JobConst.MAX_ELEMENTS)
      this.makeFree(pBatch);
    if (pBatch.main.Count != 0)
      return;
    this._batches_active.Remove(pBatch);
  }

  protected virtual void makeFree(TBatch pBatch)
  {
    if (pBatch.free_slots)
      return;
    pBatch.free_slots = true;
    this._batches_free.Push(pBatch);
  }

  internal void clear()
  {
    this._batches_free.Clear();
    for (int index = 0; index < this._batches_active.Count; ++index)
    {
      TBatch pBatch = this._batches_active[index];
      pBatch.clear();
      pBatch.free_slots = false;
      this.makeFree(pBatch);
    }
  }

  internal void clearHelperLists()
  {
    for (int index = 0; index < this._batches_active.Count; ++index)
      this._batches_active[index].clearHelperLists();
  }

  public void debug(DebugTool pTool)
  {
    int pT2 = 0;
    for (int index = 0; index < this._batches_active.Count; ++index)
    {
      TBatch batch = this._batches_active[index];
      pT2 += batch.main.Count;
    }
    pTool.setText("batches all", (object) this._batches_active.Count);
    pTool.setText("objects", (object) pT2);
    pTool.setSeparator();
    pTool.setText("parallel_jobs_updater_on", (object) Config.parallel_jobs_updater);
  }

  public string debugBatchCount()
  {
    int count = this._batches_active.Count;
    string str1 = count.ToString();
    count = this._batches_free.Count;
    string str2 = count.ToString();
    return $"{str1} / {str2}";
  }

  public string debugJobCount()
  {
    int num = 0;
    foreach (TBatch batch in this._batches_active)
    {
      num += batch.jobs_post.Count;
      num += batch.jobs_pre.Count;
      num += batch.jobs_parallel.Count;
    }
    return num.ToString();
  }
}
