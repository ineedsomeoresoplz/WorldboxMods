// Decompiled with JetBrains decompiler
// Type: db.HistoryMetaDataAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace db;

[Serializable]
public class HistoryMetaDataAsset : Asset
{
  [NonSerialized]
  public List<HistoryDataAsset> categories = new List<HistoryDataAsset>();
  public HistoryDataCollector collector;
  public MetaType meta_type;
  public Type table_type;
  public Dictionary<HistoryInterval, Type> table_types;

  public Type getTableType(HistoryInterval pInterval) => this.table_types[pInterval];
}
