// Decompiled with JetBrains decompiler
// Type: KingdomTraitLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class KingdomTraitLibrary : BaseTraitLibrary<KingdomTrait>
{
  protected override List<string> getDefaultTraitsForMeta(ActorAsset pAsset)
  {
    return pAsset.default_kingdom_traits;
  }

  public override void init()
  {
    base.init();
    KingdomTrait pAsset1 = new KingdomTrait();
    pAsset1.id = "tax_rate_local_high";
    pAsset1.group_id = "local_tax";
    pAsset1.is_local_tax_trait = true;
    pAsset1.tax_rate = 0.7f;
    this.add(pAsset1);
    this.t.addOpposite("tax_rate_local_low");
    KingdomTrait pAsset2 = new KingdomTrait();
    pAsset2.id = "tax_rate_local_low";
    pAsset2.group_id = "local_tax";
    pAsset2.is_local_tax_trait = true;
    pAsset2.tax_rate = 0.2f;
    this.add(pAsset2);
    this.t.addOpposite("tax_rate_local_high");
    KingdomTrait pAsset3 = new KingdomTrait();
    pAsset3.id = "tax_rate_tribute_high";
    pAsset3.group_id = "tribute";
    pAsset3.is_tribute_tax_trait = true;
    pAsset3.tax_rate = 0.7f;
    this.add(pAsset3);
    this.t.addOpposite("tax_rate_tribute_low");
    KingdomTrait pAsset4 = new KingdomTrait();
    pAsset4.id = "tax_rate_tribute_low";
    pAsset4.group_id = "tribute";
    pAsset4.is_tribute_tax_trait = true;
    pAsset4.tax_rate = 0.2f;
    this.add(pAsset4);
    this.t.addOpposite("tax_rate_tribute_high");
    KingdomTrait pAsset5 = new KingdomTrait();
    pAsset5.id = "grin_mark";
    pAsset5.group_id = "fate";
    pAsset5.spawn_random_trait_allowed = false;
    pAsset5.priority = -100;
    this.add(pAsset5);
    this.t.setTraitInfoToGrinMark();
    this.t.setUnlockedWithAchievement("achievementCreaturesExplorer");
  }

  protected override string icon_path => "ui/Icons/kingdom_traits/";
}
