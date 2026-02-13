// Decompiled with JetBrains decompiler
// Type: TooltipSpeciesSpawnTraitsRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class TooltipSpeciesSpawnTraitsRow : TooltipTraitsRow<SubspeciesTrait>
{
  protected override IReadOnlyCollection<SubspeciesTrait> traits_hashset
  {
    get => (IReadOnlyCollection<SubspeciesTrait>) this.loadTraitsFromPowerAsset();
  }

  private HashSet<SubspeciesTrait> loadTraitsFromPowerAsset()
  {
    string pID = this.tooltip_data.power == null ? this.tooltip_data.tip_name : this.tooltip_data.power.getActorAssetID();
    return AssetManager.actor_library.get(pID)?.getDefaultSubspeciesTraits();
  }
}
