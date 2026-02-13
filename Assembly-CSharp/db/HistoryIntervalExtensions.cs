// Decompiled with JetBrains decompiler
// Type: db.HistoryIntervalExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace db;

public static class HistoryIntervalExtensions
{
  public static int getIntervalStep(this HistoryInterval pInterval)
  {
    switch (pInterval)
    {
      case HistoryInterval.Yearly1:
        return 1;
      case HistoryInterval.Yearly5:
        return 5;
      case HistoryInterval.Yearly10:
        return 10;
      case HistoryInterval.Yearly50:
        return 50;
      case HistoryInterval.Yearly100:
        return 100;
      case HistoryInterval.Yearly500:
        return 500;
      case HistoryInterval.Yearly1000:
        return 1000;
      case HistoryInterval.Yearly5000:
        return 5000;
      case HistoryInterval.Yearly10000:
        return 10000;
      default:
        throw new NotImplementedException("interval step not defined");
    }
  }

  public static int getMaxTimeFrame(this HistoryInterval pInterval)
  {
    int maxTimeFrame = 0;
    foreach (GraphTimeAsset graphTimeAsset in AssetManager.graph_time_library.list)
    {
      if (graphTimeAsset.interval == pInterval && maxTimeFrame < graphTimeAsset.max_time_frame)
        maxTimeFrame = graphTimeAsset.max_time_frame;
    }
    return maxTimeFrame;
  }

  public static (int tEveryYears, HistoryInterval tFromInterval) fillFrom(
    this HistoryInterval pInterval)
  {
    switch (pInterval)
    {
      case HistoryInterval.Yearly1:
        return (0, HistoryInterval.None);
      case HistoryInterval.Yearly5:
        return (5, HistoryInterval.Yearly1);
      case HistoryInterval.Yearly10:
        return (10, HistoryInterval.Yearly1);
      case HistoryInterval.Yearly50:
        return (50, HistoryInterval.Yearly10);
      case HistoryInterval.Yearly100:
        return (100, HistoryInterval.Yearly10);
      case HistoryInterval.Yearly500:
        return (500, HistoryInterval.Yearly50);
      case HistoryInterval.Yearly1000:
        return (1000, HistoryInterval.Yearly100);
      case HistoryInterval.Yearly5000:
        return (5000, HistoryInterval.Yearly500);
      case HistoryInterval.Yearly10000:
        return (10000, HistoryInterval.Yearly1000);
      default:
        throw new NotImplementedException("interval step not defined");
    }
  }
}
