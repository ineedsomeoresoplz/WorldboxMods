// Decompiled with JetBrains decompiler
// Type: LanguageTraitGroupLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class LanguageTraitGroupLibrary : BaseCategoryLibrary<LanguageTraitGroupAsset>
{
  public override void init()
  {
    base.init();
    LanguageTraitGroupAsset pAsset1 = new LanguageTraitGroupAsset();
    pAsset1.id = "knowledge";
    pAsset1.name = "trait_group_knowledge";
    pAsset1.color = "#BAF0F4";
    this.add(pAsset1);
    LanguageTraitGroupAsset pAsset2 = new LanguageTraitGroupAsset();
    pAsset2.id = "spirit";
    pAsset2.name = "trait_group_spirit";
    pAsset2.color = "#BAFFC2";
    this.add(pAsset2);
    LanguageTraitGroupAsset pAsset3 = new LanguageTraitGroupAsset();
    pAsset3.id = "harmony";
    pAsset3.name = "trait_group_harmony";
    pAsset3.color = "#FFFAA3";
    this.add(pAsset3);
    LanguageTraitGroupAsset pAsset4 = new LanguageTraitGroupAsset();
    pAsset4.id = "chaos";
    pAsset4.name = "trait_group_chaos";
    pAsset4.color = "#FF6B86";
    this.add(pAsset4);
    LanguageTraitGroupAsset pAsset5 = new LanguageTraitGroupAsset();
    pAsset5.id = "miscellaneous";
    pAsset5.name = "trait_group_miscellaneous";
    pAsset5.color = "#D8D8D8";
    this.add(pAsset5);
    LanguageTraitGroupAsset pAsset6 = new LanguageTraitGroupAsset();
    pAsset6.id = "fate";
    pAsset6.name = "trait_group_fate";
    pAsset6.color = "#ffd82f";
    this.add(pAsset6);
    LanguageTraitGroupAsset pAsset7 = new LanguageTraitGroupAsset();
    pAsset7.id = "special";
    pAsset7.name = "trait_group_special";
    pAsset7.color = "#FF8F44";
    this.add(pAsset7);
  }
}
