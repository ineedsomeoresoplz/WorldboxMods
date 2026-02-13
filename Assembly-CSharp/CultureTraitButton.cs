// Decompiled with JetBrains decompiler
// Type: CultureTraitButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class CultureTraitButton : TraitButton<CultureTrait>
{
  internal override void load(string pTraitID)
  {
    this.load(AssetManager.culture_traits.get(pTraitID));
  }

  protected override void startSignal()
  {
    AchievementLibrary.trait_explorer_culture.checkBySignal();
  }

  protected override string tooltip_type => "culture_trait";

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      culture_trait = this.augmentation_asset
    };
  }
}
