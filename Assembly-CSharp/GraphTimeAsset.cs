// Decompiled with JetBrains decompiler
// Type: GraphTimeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;

#nullable disable
[Serializable]
public class GraphTimeAsset : Asset
{
  public GraphTimeScale scale_id;
  public HistoryInterval interval;
  public int max_time_frame;
}
