// Decompiled with JetBrains decompiler
// Type: ActorTraitButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ActorTraitButton : TraitButton<ActorTrait>
{
  internal override void load(string pTraitID) => this.load(AssetManager.traits.get(pTraitID));

  protected override string tooltip_type => "trait";

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      trait = this.augmentation_asset
    };
  }
}
