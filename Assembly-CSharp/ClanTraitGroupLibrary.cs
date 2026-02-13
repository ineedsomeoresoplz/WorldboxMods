// Decompiled with JetBrains decompiler
// Type: ClanTraitGroupLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ClanTraitGroupLibrary : BaseCategoryLibrary<ClanTraitGroupAsset>
{
  public override void init()
  {
    base.init();
    ClanTraitGroupAsset pAsset1 = new ClanTraitGroupAsset();
    pAsset1.id = "spirit";
    pAsset1.name = "trait_group_spirit";
    pAsset1.color = "#BAFFC2";
    this.add(pAsset1);
    ClanTraitGroupAsset pAsset2 = new ClanTraitGroupAsset();
    pAsset2.id = "mind";
    pAsset2.name = "trait_group_mind";
    pAsset2.color = "#BAF0F4";
    this.add(pAsset2);
    ClanTraitGroupAsset pAsset3 = new ClanTraitGroupAsset();
    pAsset3.id = "body";
    pAsset3.name = "trait_group_body";
    pAsset3.color = "#FF6B86";
    this.add(pAsset3);
    ClanTraitGroupAsset pAsset4 = new ClanTraitGroupAsset();
    pAsset4.id = "chaos";
    pAsset4.name = "trait_group_chaos";
    pAsset4.color = "#FF6A00";
    this.add(pAsset4);
    ClanTraitGroupAsset pAsset5 = new ClanTraitGroupAsset();
    pAsset5.id = "harmony";
    pAsset5.name = "trait_group_harmony";
    pAsset5.color = "#DD96FF";
    this.add(pAsset5);
    ClanTraitGroupAsset pAsset6 = new ClanTraitGroupAsset();
    pAsset6.id = "fate";
    pAsset6.name = "trait_group_fate";
    pAsset6.color = "#ffd82f";
    this.add(pAsset6);
    ClanTraitGroupAsset pAsset7 = new ClanTraitGroupAsset();
    pAsset7.id = "special";
    pAsset7.name = "trait_group_special";
    pAsset7.color = "#FF8F44";
    this.add(pAsset7);
  }
}
