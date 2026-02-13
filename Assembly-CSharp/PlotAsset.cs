// Decompiled with JetBrains decompiler
// Type: PlotAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class PlotAsset : 
  BaseAugmentationAsset,
  IDescription2Asset,
  IDescriptionAsset,
  ILocalizedAsset
{
  public int limit_members;
  [DefaultValue(1)]
  public int pot_rate = 1;
  public int min_level;
  public int min_renown_kingdom;
  public int min_renown_actor;
  [DefaultValue(2)]
  public int min_intelligence = 2;
  [DefaultValue(2)]
  public int min_diplomacy = 2;
  [DefaultValue(2)]
  public int min_warfare = 2;
  [DefaultValue(2)]
  public int min_stewardship = 2;
  public bool can_be_done_by_king;
  public bool can_be_done_by_leader;
  public bool can_be_done_by_clan_member;
  public bool requires_diplomacy;
  public bool requires_rebellion;
  [DefaultValue(60f)]
  public float progress_needed = 60f;
  [DefaultValue(5)]
  public int money_cost = 5;
  public bool is_basic_plot;
  public Rarity rarity;
  public PlotDescription get_formatted_description;
  public PlotCheckerDelegate check_is_possible;
  public PlotCheckerDelegate check_can_be_forced;
  public PlotCheckerDelegate check_should_continue;
  public PlotActorPlotDelegate check_other_plots;
  public PlotTryToStart try_to_start_advanced;
  public PlotStart start;
  public PlotAction action;
  public PlotAction post_action;
  public PlotActorPlotDelegate check_supporters;
  public bool check_target_actor;
  public bool check_target_city;
  public bool check_target_kingdom;
  public bool check_target_alliance;
  public bool check_target_war;

  protected override HashSet<string> progress_elements => this._progress_data?.unlocked_plots;

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.plot_category_library.get(this.group_id);
  }

  public PlotCategoryAsset getPlotGroup() => AssetManager.plot_category_library.get(this.group_id);

  public bool checkIsPossible(Actor pActor, bool pWorldLawsCheck = true)
  {
    return pActor.hasEnoughMoney(this.money_cost) && pActor.level >= this.min_level && pActor.renown >= this.min_renown_actor && pActor.kingdom.getRenown() >= this.min_renown_kingdom && pActor.intelligence >= this.min_intelligence && pActor.diplomacy >= this.min_diplomacy && pActor.warfare >= this.min_warfare && pActor.stewardship >= this.min_stewardship && this.canBeDoneByRole(pActor) && (!pWorldLawsCheck || this.isAllowedByWorldLaws()) && this.check_is_possible(pActor);
  }

  public bool canBeDoneByRole(Actor pActor)
  {
    return this.can_be_done_by_king && pActor.isKing() || this.can_be_done_by_leader && pActor.isCityLeader() || this.can_be_done_by_clan_member && pActor.hasClan();
  }

  public bool isAllowedByWorldLaws()
  {
    return (!this.requires_diplomacy || PlotAsset.isDiplomacyON()) && (!this.requires_rebellion || PlotAsset.isRebellionON());
  }

  private static bool isDiplomacyON() => WorldLawLibrary.world_law_diplomacy.isEnabled();

  private static bool isRebellionON() => WorldLawLibrary.world_law_rebellions.isEnabled();

  protected override bool isDebugUnlockedAll() => DebugConfig.isOn(DebugOption.UnlockAllPlots);

  public override string getLocaleID() => "plot_" + this.id;

  public string getDescriptionID() => $"plot_{this.id}_info";

  public string getDescriptionID2() => $"plot_{this.id}_info_base";
}
