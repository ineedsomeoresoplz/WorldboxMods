// Decompiled with JetBrains decompiler
// Type: ReligionTraitButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ReligionTraitButton : TraitButton<ReligionTrait>
{
  internal override void load(string pTraitID)
  {
    this.load(AssetManager.religion_traits.get(pTraitID));
  }

  protected override void startSignal()
  {
    AchievementLibrary.trait_explorer_religion.checkBySignal();
  }

  protected override string tooltip_type => "religion_trait";

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      religion_trait = this.augmentation_asset
    };
  }
}
