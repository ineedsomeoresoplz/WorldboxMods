// Decompiled with JetBrains decompiler
// Type: ClanTraitLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ClanTraitLibrary : BaseTraitLibrary<ClanTrait>
{
  protected override List<string> getDefaultTraitsForMeta(ActorAsset pAsset)
  {
    return pAsset.default_clan_traits;
  }

  public override void init()
  {
    base.init();
    ClanTrait pAsset1 = new ClanTrait();
    pAsset1.id = "mark_of_becoming";
    pAsset1.group_id = "special";
    pAsset1.can_be_given = false;
    pAsset1.can_be_removed = false;
    pAsset1.spawn_random_trait_allowed = false;
    this.add(pAsset1);
    ClanTrait pAsset2 = new ClanTrait();
    pAsset2.id = "blood_pact";
    pAsset2.group_id = "spirit";
    this.add(pAsset2);
    this.t.base_stats["warfare"] = 1f;
    this.t.addDecision("banish_unruly_clan_members");
    this.t.addOpposite("deathbound");
    ClanTrait pAsset3 = new ClanTrait();
    pAsset3.id = "deathbound";
    pAsset3.group_id = "spirit";
    this.add(pAsset3);
    this.t.base_stats["warfare"] = 5f;
    this.t.addDecision("kill_unruly_clan_members");
    this.t.addOpposite("blood_pact");
    ClanTrait pAsset4 = new ClanTrait();
    pAsset4.id = "bonebreakers";
    pAsset4.group_id = "body";
    this.add(pAsset4);
    this.t.setUnlockedWithAchievement("achievementSegregator");
    this.t.base_stats["damage"] = 5f;
    ClanTrait t1 = this.t;
    t1.action_attack_target = t1.action_attack_target + new AttackAction(ActionLibrary.breakBones);
    ClanTrait pAsset5 = new ClanTrait();
    pAsset5.id = "stonefists";
    pAsset5.group_id = "body";
    this.add(pAsset5);
    this.t.base_stats["damage"] = 30f;
    ClanTrait pAsset6 = new ClanTrait();
    pAsset6.id = "blood_of_sea";
    pAsset6.group_id = "body";
    this.add(pAsset6);
    this.t.base_stats["stamina"] = 20f;
    this.t.base_stats.addTag("fast_swimming");
    ClanTrait pAsset7 = new ClanTrait();
    pAsset7.id = "gaia_shield";
    pAsset7.group_id = "body";
    this.add(pAsset7);
    this.t.base_stats["armor"] = 10f;
    this.t.base_stats["multiplier_health"] = 0.1f;
    this.t.base_stats.addTag("immunity_fire");
    this.t.base_stats.addTag("immunity_cold");
    ClanTrait pAsset8 = new ClanTrait();
    pAsset8.id = "iron_will";
    pAsset8.group_id = "mind";
    this.add(pAsset8);
    this.t.base_stats["intelligence"] = 5f;
    this.t.base_stats.addTag("strong_mind");
    ClanTrait pAsset9 = new ClanTrait();
    pAsset9.id = "flesh_weavers";
    pAsset9.group_id = "body";
    pAsset9.special_effect_interval = 2f;
    this.add(pAsset9);
    this.t.base_stats["multiplier_health"] = 0.2f;
    ClanTrait t2 = this.t;
    t2.action_special_effect = t2.action_special_effect + new WorldAction(ActionLibrary.regenerationEffectClan);
    ClanTrait pAsset10 = new ClanTrait();
    pAsset10.id = "endurance_of_titans";
    pAsset10.group_id = "body";
    this.add(pAsset10);
    this.t.base_stats["multiplier_stamina"] = 3f;
    ClanTrait pAsset11 = new ClanTrait();
    pAsset11.id = "combat_instincts";
    pAsset11.group_id = "mind";
    this.add(pAsset11);
    this.t.setUnlockedWithAchievement("achievementMasterOfCombat");
    this.t.base_stats["warfare"] = 10f;
    this.t.addCombatAction("combat_dash");
    this.t.addCombatAction("combat_block");
    this.t.addCombatAction("combat_dodge");
    this.t.addCombatAction("combat_backstep");
    this.t.addCombatAction("combat_deflect_projectile");
    ClanTrait pAsset12 = new ClanTrait();
    pAsset12.id = "void_ban";
    pAsset12.group_id = "chaos";
    pAsset12.spawn_random_trait_allowed = false;
    this.add(pAsset12);
    this.t.base_stats["multiplier_mana"] = -10f;
    ClanTrait pAsset13 = new ClanTrait();
    pAsset13.id = "warlocks_vein";
    pAsset13.group_id = "spirit";
    this.add(pAsset13);
    this.t.base_stats_male["multiplier_mana"] = 2f;
    ClanTrait pAsset14 = new ClanTrait();
    pAsset14.id = "witchs_vein";
    pAsset14.group_id = "spirit";
    this.add(pAsset14);
    this.t.base_stats_female["multiplier_mana"] = 2f;
    ClanTrait pAsset15 = new ClanTrait();
    pAsset15.id = "magic_blood";
    pAsset15.group_id = "spirit";
    this.add(pAsset15);
    this.t.setUnlockedWithAchievement("achievementTheAccomplished");
    this.t.base_stats["multiplier_mana"] = 3f;
    ClanTrait pAsset16 = new ClanTrait();
    pAsset16.id = "blood_of_eons";
    pAsset16.group_id = "body";
    pAsset16.spawn_random_trait_allowed = false;
    this.add(pAsset16);
    this.t.addOpposite("cursed_blood");
    this.t.base_stats["lifespan"] = 1E+09f;
    ClanTrait pAsset17 = new ClanTrait();
    pAsset17.id = "blood_of_giants";
    pAsset17.group_id = "body";
    this.add(pAsset17);
    this.t.base_stats["scale"] = 0.05f;
    ClanTrait pAsset18 = new ClanTrait();
    pAsset18.id = "silver_tongues";
    pAsset18.group_id = "mind";
    this.add(pAsset18);
    this.t.base_stats["opinion"] = 20f;
    this.t.base_stats["diplomacy"] = 5f;
    ClanTrait pAsset19 = new ClanTrait();
    pAsset19.id = "masters_of_propaganda";
    pAsset19.group_id = "mind";
    this.add(pAsset19);
    this.t.base_stats["loyalty_traits"] = 20f;
    ClanTrait pAsset20 = new ClanTrait();
    pAsset20.id = "gods_chosen";
    pAsset20.group_id = "spirit";
    this.add(pAsset20);
    this.t.base_stats["stewardship"] = 10f;
    this.t.base_stats["diplomacy"] = 5f;
    this.t.base_stats["armor"] = 20f;
    ClanTrait pAsset21 = new ClanTrait();
    pAsset21.id = "cursed_blood";
    pAsset21.group_id = "chaos";
    pAsset21.spawn_random_trait_allowed = false;
    this.add(pAsset21);
    this.t.setUnlockedWithAchievement("achievementTheBroken");
    this.t.base_stats["lifespan"] = -666f;
    this.t.addOpposite("blood_of_eons");
    ClanTrait pAsset22 = new ClanTrait();
    pAsset22.id = "divine_dozen";
    pAsset22.group_id = "harmony";
    this.add(pAsset22);
    this.t.addOpposite("we_are_legion");
    this.t.addOpposite("best_five");
    this.t.base_stats_meta["limit_clan_members"] = 12f;
    ClanTrait pAsset23 = new ClanTrait();
    pAsset23.id = "best_five";
    pAsset23.group_id = "harmony";
    this.add(pAsset23);
    this.t.addOpposite("we_are_legion");
    this.t.addOpposite("divine_dozen");
    this.t.base_stats_meta["limit_clan_members"] = 5f;
    ClanTrait pAsset24 = new ClanTrait();
    pAsset24.id = "we_are_legion";
    pAsset24.group_id = "harmony";
    this.add(pAsset24);
    this.t.setUnlockedWithAchievement("achievementMegapolis");
    this.t.addOpposite("best_five");
    this.t.addOpposite("divine_dozen");
    this.t.base_stats_meta["limit_clan_members"] = 1000f;
    ClanTrait pAsset25 = new ClanTrait();
    pAsset25.id = "nitroglycerin_blood";
    pAsset25.group_id = "chaos";
    pAsset25.action_death = (WorldAction) ((_, pTile) =>
    {
      DropsLibrary.action_grenade(pTile);
      return true;
    });
    this.add(pAsset25);
    this.t.setUnlockedWithAchievement("achievementMinefield");
    this.t.base_stats["health"] = -1f;
    ClanTrait pAsset26 = new ClanTrait();
    pAsset26.id = "antimatter_blood";
    pAsset26.group_id = "chaos";
    pAsset26.spawn_random_trait_allowed = false;
    pAsset26.action_death = (WorldAction) ((_, pTile) =>
    {
      DropsLibrary.action_antimatter_bomb(pTile);
      return true;
    });
    this.add(pAsset26);
    this.t.setUnlockedWithAchievement("achievementTraitExplorerClan");
    this.t.base_stats["damage"] = 1f;
    ClanTrait pAsset27 = new ClanTrait();
    pAsset27.id = "gaia_blood";
    pAsset27.group_id = "spirit";
    pAsset27.action_death = (WorldAction) ((_, pTile) =>
    {
      if (!WorldLawLibrary.world_law_clouds.isEnabled())
        return false;
      if (Randy.randomChance(0.3f))
        EffectsLibrary.spawn("fx_cloud", pTile, "cloud_normal");
      return true;
    });
    this.add(pAsset27);
    this.t.setUnlockedWithAchievement("achievementThePrincess");
    this.t.base_stats["multiplier_health"] = 0.05f;
    ClanTrait pAsset28 = new ClanTrait();
    pAsset28.id = "grin_mark";
    pAsset28.group_id = "fate";
    pAsset28.spawn_random_trait_allowed = false;
    pAsset28.priority = -100;
    this.add(pAsset28);
    this.t.setTraitInfoToGrinMark();
    this.t.setUnlockedWithAchievement("achievementCreaturesExplorer");
    ClanTrait pAsset29 = new ClanTrait();
    pAsset29.id = "geb";
    pAsset29.group_id = "special";
    pAsset29.can_be_given = false;
    pAsset29.can_be_removed = false;
    pAsset29.spawn_random_trait_allowed = false;
    this.add(pAsset29);
  }

  protected override string icon_path => "ui/Icons/clan_traits/";
}
