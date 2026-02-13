// Decompiled with JetBrains decompiler
// Type: ProfessionLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ProfessionLibrary : AssetLibrary<ProfessionAsset>
{
  public static readonly UnitProfession[] list_enum_profession_ids = (UnitProfession[]) Enum.GetValues(typeof (UnitProfession));
  private Dictionary<UnitProfession, ProfessionAsset> _dict_profession_id = new Dictionary<UnitProfession, ProfessionAsset>();

  public override void init()
  {
    base.init();
    ProfessionAsset pAsset1 = new ProfessionAsset();
    pAsset1.id = "nothing";
    pAsset1.profession_id = UnitProfession.Nothing;
    this.add(pAsset1);
    ProfessionAsset pAsset2 = new ProfessionAsset();
    pAsset2.id = "unit";
    pAsset2.profession_id = UnitProfession.Unit;
    pAsset2.is_civilian = true;
    this.add(pAsset2);
    ProfessionAsset pAsset3 = new ProfessionAsset();
    pAsset3.id = "warrior";
    pAsset3.profession_id = UnitProfession.Warrior;
    pAsset3.can_capture = true;
    pAsset3.cancel_when_no_city = true;
    this.add(pAsset3);
    this.t.addDecision("warrior_try_join_army_group");
    this.t.addDecision("check_warrior_limit");
    this.t.addDecision("city_walking_to_danger_zone");
    this.t.addDecision("warrior_army_captain_idle_walking_city");
    this.t.addDecision("warrior_army_captain_waiting");
    this.t.addDecision("warrior_army_leader_move_random");
    this.t.addDecision("warrior_army_leader_move_to_attack_target");
    this.t.addDecision("warrior_army_follow_leader");
    this.t.addDecision("warrior_random_move");
    this.t.addDecision("check_warrior_transport");
    this.t.addDecision("warrior_train_with_dummy");
    ProfessionAsset pAsset4 = new ProfessionAsset();
    pAsset4.id = "king";
    pAsset4.profession_id = UnitProfession.King;
    pAsset4.can_capture = true;
    this.add(pAsset4);
    this.t.addDecision("king_check_new_city_foundation");
    this.t.addDecision("king_change_kingdom_language");
    this.t.addDecision("king_change_kingdom_culture");
    this.t.addDecision("king_change_kingdom_religion");
    this.t.addDecision("claim_land");
    ProfessionAsset pAsset5 = new ProfessionAsset();
    pAsset5.id = "leader";
    pAsset5.profession_id = UnitProfession.Leader;
    pAsset5.can_capture = true;
    this.add(pAsset5);
    this.t.addDecision("leader_change_city_language");
    this.t.addDecision("leader_change_city_culture");
    this.t.addDecision("leader_change_city_religion");
    this.t.addDecision("claim_land");
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this.linkDecisions();
  }

  private void linkDecisions()
  {
    foreach (ProfessionAsset professionAsset in this.list)
      professionAsset.linkDecisions();
  }

  public override ProfessionAsset add(ProfessionAsset pAsset)
  {
    this._dict_profession_id.Add(pAsset.profession_id, pAsset);
    return base.add(pAsset);
  }

  public virtual ProfessionAsset get(UnitProfession pID) => this._dict_profession_id[pID];
}
