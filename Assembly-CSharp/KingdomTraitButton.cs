// Decompiled with JetBrains decompiler
// Type: KingdomTraitButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class KingdomTraitButton : TraitButton<KingdomTrait>
{
  internal override void load(string pTraitID)
  {
    this.load(AssetManager.kingdoms_traits.get(pTraitID));
  }

  protected override string tooltip_type => "kingdom_trait";

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      kingdom_trait = this.augmentation_asset
    };
  }
}
