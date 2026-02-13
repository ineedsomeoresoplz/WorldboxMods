// Decompiled with JetBrains decompiler
// Type: StatsHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
internal static class StatsHelper
{
  public static string getStatistic(string statName)
  {
    StatisticsAsset pAsset = AssetManager.statistics_library.get(statName);
    return pAsset != null && pAsset.string_action != null ? pAsset.string_action(pAsset) : StatsHelper.getStat(statName).ToString() ?? "";
  }

  public static long getStat(string statName)
  {
    StatisticsAsset pAsset = AssetManager.statistics_library.get(statName);
    return pAsset != null && pAsset.long_action != null ? pAsset.long_action(pAsset) : 0L;
  }
}
