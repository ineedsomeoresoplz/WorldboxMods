// Decompiled with JetBrains decompiler
// Type: ListWindowStatistics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ListWindowStatistics : StatisticsRows
{
  public MetaType meta_type;

  protected override void init()
  {
    foreach (StatisticsAsset pAsset in AssetManager.statistics_library.list)
    {
      if (!pAsset.list_window_meta_type.isNone() && this.meta_type == pAsset.list_window_meta_type)
        this.addStatRow(pAsset);
    }
  }
}
