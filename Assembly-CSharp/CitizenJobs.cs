// Decompiled with JetBrains decompiler
// Type: CitizenJobs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
[Serializable]
public class CitizenJobs
{
  public Dictionary<CitizenJobAsset, int> jobs = new Dictionary<CitizenJobAsset, int>();
  public Dictionary<CitizenJobAsset, int> occupied = new Dictionary<CitizenJobAsset, int>();
  private int _total_tasks;

  public void clear()
  {
    this.jobs.Clear();
    this.occupied.Clear();
  }

  public int getTotalTasks() => this._total_tasks;

  public bool hasAnyTask() => this._total_tasks > 0;

  public void clearJobs()
  {
    this._total_tasks = 0;
    this.jobs.Clear();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void addToJob(CitizenJobAsset pJobAsset, int pValue)
  {
    this._total_tasks += pValue;
    int num;
    if (this.jobs.TryGetValue(pJobAsset, out num))
      this.jobs[pJobAsset] = num + pValue;
    else
      this.jobs.Add(pJobAsset, pValue);
  }

  public bool continueJob(CitizenJobAsset pJobAsset) => this.jobs.ContainsKey(pJobAsset);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int countOccupied(CitizenJobAsset pJobAsset)
  {
    int num;
    return this.occupied.TryGetValue(pJobAsset, out num) ? num : 0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int countCurrentJobs(CitizenJobAsset pJobAsset)
  {
    int num;
    return this.jobs.TryGetValue(pJobAsset, out num) ? num : 0;
  }

  public bool hasJob(CitizenJobAsset pJobAsset)
  {
    int num;
    return this.jobs.TryGetValue(pJobAsset, out num) && num != 0 && (!this.occupied.ContainsKey(pJobAsset) || this.occupied[pJobAsset] < num);
  }

  public void takeJob(CitizenJobAsset pJobAsset)
  {
    if (!this.occupied.ContainsKey(pJobAsset))
      this.occupied.Add(pJobAsset, 1);
    else
      ++this.occupied[pJobAsset];
  }

  public void freeJob(CitizenJobAsset pJobAsset)
  {
    int num;
    if (this.occupied.TryGetValue(pJobAsset, out num))
    {
      if (num <= 0)
        return;
      --this.occupied[pJobAsset];
    }
    else
      this.occupied.Add(pJobAsset, 0);
  }
}
