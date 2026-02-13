// Decompiled with JetBrains decompiler
// Type: KingdomTraitGroupLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class KingdomTraitGroupLibrary : BaseCategoryLibrary<KingdomTraitGroupAsset>
{
  public override void init()
  {
    base.init();
    KingdomTraitGroupAsset pAsset1 = new KingdomTraitGroupAsset();
    pAsset1.id = "tribute";
    pAsset1.name = "trait_group_tribute";
    pAsset1.color = "#BAFFC2";
    this.add(pAsset1);
    KingdomTraitGroupAsset pAsset2 = new KingdomTraitGroupAsset();
    pAsset2.id = "local_tax";
    pAsset2.name = "trait_group_local_tax";
    pAsset2.color = "#BAFFC2";
    this.add(pAsset2);
    KingdomTraitGroupAsset pAsset3 = new KingdomTraitGroupAsset();
    pAsset3.id = "miscellaneous";
    pAsset3.name = "trait_group_miscellaneous";
    pAsset3.color = "#D8D8D8";
    this.add(pAsset3);
    KingdomTraitGroupAsset pAsset4 = new KingdomTraitGroupAsset();
    pAsset4.id = "fate";
    pAsset4.name = "trait_group_fate";
    pAsset4.color = "#ffd82f";
    this.add(pAsset4);
  }
}
