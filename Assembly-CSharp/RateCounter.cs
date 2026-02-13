// Decompiled with JetBrains decompiler
// Type: RateCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class RateCounter
{
  private readonly List<RateCounterData> _timestamps = new List<RateCounterData>();
  private string _id;
  private double _ticks;
  private int _total;

  public RateCounter(string pID, int pTicks = 60)
  {
    this._id = pID;
    this._ticks = (double) pTicks;
  }

  public void reset()
  {
    this._timestamps.Clear();
    this._total = 0;
  }

  public void registerEvent()
  {
  }

  public void registerEvent(double pValue)
  {
  }

  private double getTime() => World.world.getCurWorldTime();

  public double getValuesAll()
  {
    double valuesAll = 0.0;
    foreach (RateCounterData timestamp in this._timestamps)
      valuesAll += timestamp.value;
    return valuesAll;
  }

  public int getEventsPerTick()
  {
    this.cleanupOldEvents(this.getTime());
    return this._timestamps.Count;
  }

  private void cleanupOldEvents(double tNow)
  {
    if (this._timestamps.Count == 0)
      return;
    this._timestamps.RemoveAll((Predicate<RateCounterData>) (t => tNow - t.timestamp > this._ticks));
  }

  public string getInfo() => $"{this.getEventsPerTick()} | tot: {this._total}";

  public int getTotal() => this._total;

  public string id => this._id;

  public int getEventsPerMinute() => this.getEventsPerTick();
}
