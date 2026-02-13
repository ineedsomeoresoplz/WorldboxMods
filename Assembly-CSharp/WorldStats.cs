// Decompiled with JetBrains decompiler
// Type: WorldStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorldStats : StatisticsRows
{
  [SerializeField]
  private WorldStatsTabs _tab_type;

  protected override void init()
  {
    bool flag = this._tab_type != 0;
    foreach (StatisticsAsset pAsset in AssetManager.statistics_library.list)
    {
      if (pAsset.is_world_statistics && (!flag || pAsset.world_stats_tabs.HasFlag((Enum) this._tab_type)))
        this.addStatRow(pAsset);
    }
  }
}
