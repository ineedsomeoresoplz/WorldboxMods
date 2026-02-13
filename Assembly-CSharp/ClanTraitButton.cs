// Decompiled with JetBrains decompiler
// Type: ClanTraitButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ClanTraitButton : TraitButton<ClanTrait>
{
  internal override void load(string pTraitID) => this.load(AssetManager.clan_traits.get(pTraitID));

  protected override void startSignal() => AchievementLibrary.trait_explorer_clan.checkBySignal();

  protected override string tooltip_type => "clan_trait";

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      clan_trait = this.augmentation_asset
    };
  }
}
