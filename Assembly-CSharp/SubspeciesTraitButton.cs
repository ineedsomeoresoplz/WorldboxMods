// Decompiled with JetBrains decompiler
// Type: SubspeciesTraitButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SubspeciesTraitButton : TraitButton<SubspeciesTrait>
{
  internal override void load(string pTraitID)
  {
    this.load(AssetManager.subspecies_traits.get(pTraitID));
  }

  protected override void startSignal()
  {
    AchievementLibrary.trait_explorer_subspecies.checkBySignal();
  }

  protected override string tooltip_type => "subspecies_trait";

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      subspecies_trait = this.augmentation_asset
    };
  }
}
