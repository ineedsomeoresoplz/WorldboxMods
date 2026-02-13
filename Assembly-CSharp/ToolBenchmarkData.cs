// Decompiled with JetBrains decompiler
// Type: ToolBenchmarkData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ToolBenchmarkData
{
  public string id;
  private const int MAXIMUM_VALUES = 64 /*0x40*/;
  private Queue<double> results = new Queue<double>(64 /*0x40*/);
  private Queue<long> results_counters = new Queue<long>(64 /*0x40*/);
  public double latest_time;
  public double latest_result;
  public double calculated_percentage;
  public long call_count;
  public long debug_value;
  public double last_max_value;
  public int max_value_ticks;

  public void newValue(int pValue) => this.debug_value = (long) pValue;

  public void newCount(long pValue) => this.call_count += pValue;

  public void saveLastMaxValue(double pValue)
  {
    if (pValue > this.last_max_value || this.max_value_ticks <= 0)
    {
      this.last_max_value = pValue;
      this.max_value_ticks = 200;
    }
    if (this.max_value_ticks <= 0)
      return;
    --this.max_value_ticks;
  }

  public long getAverageCount()
  {
    if (this.results_counters.Count == 0)
      return 0;
    long num = 0;
    foreach (long resultsCounter in this.results_counters)
      num += resultsCounter;
    return num / (long) this.results_counters.Count;
  }

  public long getLastCount()
  {
    return this.results_counters.Count == 0 ? 0L : this.results_counters.Peek();
  }

  public void saveAverageCounter()
  {
    if (this.results_counters.Count > 64 /*0x40*/)
      this.results_counters.Dequeue();
    this.results_counters.Enqueue(this.call_count);
    this.call_count = 0L;
  }

  public void start(double pTime) => this.latest_time = pTime;

  public void end(double pTime)
  {
    this.latest_result = pTime;
    if (this.results.Count > 64 /*0x40*/)
      this.results.Dequeue();
    this.results.Enqueue(pTime);
  }

  public double getAverage()
  {
    double num = 0.0;
    foreach (double result in this.results)
      num += result;
    return num / (double) this.results.Count;
  }

  public void setResult(double pTime)
  {
    this.latest_time = pTime;
    this.latest_result = pTime;
  }
}
