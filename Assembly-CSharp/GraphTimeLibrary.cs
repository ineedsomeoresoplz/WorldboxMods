// Decompiled with JetBrains decompiler
// Type: GraphTimeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;

#nullable disable
public class GraphTimeLibrary : AssetLibrary<GraphTimeAsset>
{
  public override void init()
  {
    base.init();
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_10,
      max_time_frame = 10,
      interval = HistoryInterval.Yearly1
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_25,
      max_time_frame = 25,
      interval = HistoryInterval.Yearly5
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_100,
      max_time_frame = 100,
      interval = HistoryInterval.Yearly10
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_250,
      max_time_frame = 250,
      interval = HistoryInterval.Yearly50
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_500,
      max_time_frame = 500,
      interval = HistoryInterval.Yearly50
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_1000,
      max_time_frame = 1000,
      interval = HistoryInterval.Yearly100
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_2500,
      max_time_frame = 2500,
      interval = HistoryInterval.Yearly500
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_5000,
      max_time_frame = 5000,
      interval = HistoryInterval.Yearly500
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_10000,
      max_time_frame = 10000,
      interval = HistoryInterval.Yearly1000
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_50000,
      max_time_frame = 50000,
      interval = HistoryInterval.Yearly5000
    });
    this.add(new GraphTimeAsset()
    {
      scale_id = GraphTimeScale.year_100000,
      max_time_frame = 100000,
      interval = HistoryInterval.Yearly10000
    });
  }

  public static long getMinTime(GraphTimeAsset pAsset)
  {
    return (long) Date.getYear((double) Date.getYearsSince(0.0) * 60.0 - 60.0 * (double) pAsset.max_time_frame);
  }

  public static long getMaxTime(GraphTimeAsset pAsset)
  {
    return (long) Date.getYear((double) Date.getYearsSince(0.0) * 60.0);
  }

  public override GraphTimeAsset add(GraphTimeAsset pAsset)
  {
    pAsset.id = pAsset.scale_id.ToString();
    return base.add(pAsset);
  }
}
