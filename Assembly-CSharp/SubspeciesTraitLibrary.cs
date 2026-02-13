// Decompiled with JetBrains decompiler
// Type: SubspeciesTraitLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using strings;
using System.Collections.Generic;

#nullable disable
public class SubspeciesTraitLibrary : BaseTraitLibrary<SubspeciesTrait>
{
  private const string TEMPLATE_EGG = "$egg$";
  private const string TEMPLATE_MAGIC_BLOOD = "$magic_blood$";
  private const string TEMPLATE_SKIN_MUTATION = "$skin_mutation$";
  private const string TEMPLATE_ADAPTATION = "$adaptation$";
  private List<SubspeciesTrait> _pot_mutation_traits_add = new List<SubspeciesTrait>();
  private List<SubspeciesTrait> _pot_mutation_traits_remove = new List<SubspeciesTrait>();
  private static List<string> _bad_genes = AssetLibrary<SubspeciesTrait>.l<string>("fragile_health", "weak", "slow", "fat", "ugly");

  protected override string icon_path => "ui/Icons/subspecies_traits/";

  protected override List<string> getDefaultTraitsForMeta(ActorAsset pAsset)
  {
    return pAsset.default_subspecies_traits;
  }

  public override void init()
  {
    base.init();
    this.addMetamorphosis();
    this.addSpawnSomething();
    this.addLimits();
    this.addMaturation();
    this.addStats();
    this.addGenetic();
    this.addDiet();
    this.addReproduction();
    this.addReproductionModes();
    this.addOther();
    this.addSleepCycles();
    this.addMagic();
    this.addChaos();
    this.addPhenotypes();
    this.addAdaptations();
    this.addMutations();
    this.addEggs();
  }

  private void addMagic()
  {
    SubspeciesTrait pAsset = new SubspeciesTrait();
    pAsset.id = "$magic_blood$";
    pAsset.group_id = "talents";
    this.add(pAsset);
    this.t.base_stats_meta.addTag("magic");
    SubspeciesTrait t = this.t;
    t.action_death = t.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    this.clone("gift_of_fire", "$magic_blood$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.addSpell("cast_fire");
    this.clone("gift_of_thunder", "$magic_blood$");
    this.t.addSpell("summon_lightning");
    this.clone("gift_of_void", "$magic_blood$");
    this.t.addSpell("teleport");
    this.clone("gift_of_air", "$magic_blood$");
    this.t.addSpell("summon_tornado");
    this.clone("gift_of_blood", "$magic_blood$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.addSpell("cast_blood_rain");
    this.clone("gift_of_harmony", "$magic_blood$");
    this.t.addSpell("cast_blood_rain");
    this.t.addSpell("cast_cure");
    this.clone("gift_of_water", "$magic_blood$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.addSpell("cast_shield");
    this.clone("gift_of_life", "$magic_blood$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.addSpell("cast_grass_seeds");
    this.t.addSpell("spawn_vegetation");
    this.clone("gift_of_death", "$magic_blood$");
    this.t.addSpell("spawn_skeleton");
    this.t.addSpell("cast_curse");
  }

  private void addChaos()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "grin_mark";
    pAsset1.group_id = "fate";
    pAsset1.spawn_random_trait_allowed = false;
    pAsset1.priority = -100;
    this.add(pAsset1);
    this.t.setTraitInfoToGrinMark();
    this.t.show_for_unlockables_ui = true;
    this.t.setUnlockedWithAchievement("achievementCreaturesExplorer");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "annoying_fireworks";
    pAsset2.group_id = "chaos";
    pAsset2.rarity = Rarity.R0_Normal;
    pAsset2.action_death = (WorldAction) ((_, pTile) =>
    {
      EffectsLibrary.spawn("fx_fireworks", pTile);
      return true;
    });
    this.add(pAsset2);
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "spicy_kids";
    pAsset3.group_id = "chaos";
    pAsset3.action_birth = new WorldAction(ActionLibrary.fireDropsSpawn);
    this.add(pAsset3);
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "nimble";
    pAsset4.group_id = "chaos";
    pAsset4.in_mutation_pot_add = true;
    pAsset4.in_mutation_pot_remove = true;
    pAsset4.action_attack_target = (AttackAction) ((pSelf, pTarget, pTile) => pTarget.isActor() && pSelf.a.tryToStealItems(pTarget.a));
    this.add(pAsset4);
    this.t.setUnlockedWithAchievement("achievementNotOnMyWatch");
    this.t.base_stats_meta.addTag("steal_items");
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "antimatter_essence";
    pAsset5.group_id = "chaos";
    pAsset5.spawn_random_trait_allowed = false;
    pAsset5.action_death = (WorldAction) ((_, pTile) =>
    {
      DropsLibrary.action_antimatter_bomb(pTile);
      return true;
    });
    this.add(pAsset5);
    this.t.setUnlockedWithAchievement("achievementTntAndHeat");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "gaia_roots";
    pAsset6.group_id = "growth";
    pAsset6.rarity = Rarity.R0_Normal;
    pAsset6.action_death = (WorldAction) ((_, pTile) =>
    {
      if (!WorldLawLibrary.world_law_clouds.isEnabled())
        return false;
      if (Randy.randomChance(0.3f))
        EffectsLibrary.spawn("fx_cloud", pTile, "cloud_normal");
      return true;
    });
    pAsset6.in_mutation_pot_add = true;
    pAsset6.in_mutation_pot_remove = true;
    this.add(pAsset6);
    this.t.setUnlockedWithAchievement("achievementZoo");
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "parental_care";
    pAsset7.group_id = "growth";
    this.add(pAsset7);
  }

  private void addMetamorphosis()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "fire_elemental_form";
    pAsset1.group_id = "chaos";
    pAsset1.rarity = Rarity.R2_Epic;
    pAsset1.action_death = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      if (!a.isPrettyOld())
        return false;
      ActionLibrary.metamorphInto(a, SA.fire_elementals.GetRandom<string>());
      return true;
    });
    pAsset1.in_mutation_pot_add = true;
    pAsset1.in_mutation_pot_remove = true;
    this.add(pAsset1);
    this.t.setUnlockedWithAchievement("achievementEternalChaos");
    this.t.addOpposite("fenix_born");
    this.t.addOpposite("metamorphosis_butterfly");
    this.t.addOpposite("metamorphosis_chicken");
    this.t.addOpposite("metamorphosis_crab");
    this.t.addOpposite("metamorphosis_sword");
    this.t.addOpposite("metamorphosis_wolf");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "fenix_born";
    pAsset2.group_id = "rebirth";
    pAsset2.action_death = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      if (!a.isPrettyOld())
        return false;
      ActionLibrary.metamorphInto(a, a.asset.id, true, true);
      return true;
    });
    pAsset2.in_mutation_pot_add = true;
    pAsset2.in_mutation_pot_remove = true;
    this.add(pAsset2);
    this.t.setUnlockedWithAchievement("achievementLongLiving");
    this.t.addOpposite("fire_elemental_form");
    this.t.addOpposite("metamorphosis_butterfly");
    this.t.addOpposite("metamorphosis_chicken");
    this.t.addOpposite("metamorphosis_crab");
    this.t.addOpposite("metamorphosis_sword");
    this.t.addOpposite("metamorphosis_wolf");
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "metamorphosis_crab";
    pAsset3.group_id = "rebirth";
    pAsset3.rarity = Rarity.R1_Rare;
    pAsset3.action_death = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      if (!a.isPrettyOld())
        return false;
      ActionLibrary.metamorphInto(a, "crab");
      return true;
    });
    pAsset3.in_mutation_pot_add = true;
    pAsset3.in_mutation_pot_remove = true;
    this.add(pAsset3);
    this.t.setUnlockedWithAchievement("achievementEngineeredEvolution");
    this.t.addOpposite("fire_elemental_form");
    this.t.addOpposite("fenix_born");
    this.t.addOpposite("metamorphosis_butterfly");
    this.t.addOpposite("metamorphosis_chicken");
    this.t.addOpposite("metamorphosis_sword");
    this.t.addOpposite("metamorphosis_wolf");
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "metamorphosis_chicken";
    pAsset4.group_id = "rebirth";
    pAsset4.rarity = Rarity.R0_Normal;
    pAsset4.action_death = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      if (!a.isPrettyOld())
        return false;
      ActionLibrary.metamorphInto(a, "chicken");
      return true;
    });
    pAsset4.in_mutation_pot_add = true;
    pAsset4.in_mutation_pot_remove = true;
    this.add(pAsset4);
    this.t.addOpposite("fire_elemental_form");
    this.t.addOpposite("fenix_born");
    this.t.addOpposite("metamorphosis_butterfly");
    this.t.addOpposite("metamorphosis_crab");
    this.t.addOpposite("metamorphosis_sword");
    this.t.addOpposite("metamorphosis_wolf");
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "metamorphosis_wolf";
    pAsset5.group_id = "rebirth";
    pAsset5.rarity = Rarity.R0_Normal;
    pAsset5.action_death = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      if (!a.isPrettyOld())
        return false;
      ActionLibrary.metamorphInto(a, "wolf");
      return true;
    });
    pAsset5.in_mutation_pot_add = true;
    pAsset5.in_mutation_pot_remove = true;
    this.add(pAsset5);
    this.t.addOpposite("fire_elemental_form");
    this.t.addOpposite("fenix_born");
    this.t.addOpposite("metamorphosis_butterfly");
    this.t.addOpposite("metamorphosis_chicken");
    this.t.addOpposite("metamorphosis_crab");
    this.t.addOpposite("metamorphosis_sword");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "metamorphosis_butterfly";
    pAsset6.group_id = "rebirth";
    pAsset6.rarity = Rarity.R0_Normal;
    pAsset6.action_death = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      if (!a.isPrettyOld())
        return false;
      ActionLibrary.metamorphInto(a, "butterfly");
      return true;
    });
    pAsset6.in_mutation_pot_add = true;
    pAsset6.in_mutation_pot_remove = true;
    this.add(pAsset6);
    this.t.setUnlockedWithAchievement("achievementMasterWeaver");
    this.t.addOpposite("fire_elemental_form");
    this.t.addOpposite("fenix_born");
    this.t.addOpposite("metamorphosis_chicken");
    this.t.addOpposite("metamorphosis_crab");
    this.t.addOpposite("metamorphosis_sword");
    this.t.addOpposite("metamorphosis_wolf");
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "metamorphosis_sword";
    pAsset7.group_id = "rebirth";
    pAsset7.rarity = Rarity.R1_Rare;
    pAsset7.action_death = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      if (!a.isPrettyOld())
        return false;
      ActionLibrary.metamorphInto(a, "crystal_sword");
      return true;
    });
    pAsset7.in_mutation_pot_add = true;
    pAsset7.in_mutation_pot_remove = true;
    this.add(pAsset7);
    this.t.addOpposite("fire_elemental_form");
    this.t.addOpposite("fenix_born");
    this.t.addOpposite("metamorphosis_butterfly");
    this.t.addOpposite("metamorphosis_chicken");
    this.t.addOpposite("metamorphosis_crab");
    this.t.addOpposite("metamorphosis_wolf");
  }

  private void addSpawnSomething()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "bioproduct_gold";
    pAsset1.group_id = "bioproducts";
    pAsset1.rarity = Rarity.R0_Normal;
    pAsset1.priority = 100;
    pAsset1.is_diet_related = true;
    pAsset1.in_mutation_pot_add = true;
    pAsset1.in_mutation_pot_remove = true;
    this.add(pAsset1);
    this.t.setUnlockedWithAchievement("achievementSmellyCity");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "bioproduct_gems";
    pAsset2.group_id = "bioproducts";
    pAsset2.rarity = Rarity.R0_Normal;
    pAsset2.priority = 100;
    pAsset2.is_diet_related = true;
    pAsset2.in_mutation_pot_add = true;
    pAsset2.in_mutation_pot_remove = true;
    this.add(pAsset2);
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "bioproduct_stone";
    pAsset3.group_id = "bioproducts";
    pAsset3.rarity = Rarity.R0_Normal;
    pAsset3.priority = 99;
    pAsset3.is_diet_related = true;
    pAsset3.in_mutation_pot_add = true;
    pAsset3.in_mutation_pot_remove = true;
    this.add(pAsset3);
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "bioproduct_mushrooms";
    pAsset4.group_id = "bioproducts";
    pAsset4.rarity = Rarity.R0_Normal;
    pAsset4.priority = 98;
    pAsset4.is_diet_related = true;
    pAsset4.in_mutation_pot_add = true;
    pAsset4.in_mutation_pot_remove = true;
    this.add(pAsset4);
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "death_grow_mythril";
    pAsset5.group_id = "growth";
    pAsset5.rarity = Rarity.R1_Rare;
    pAsset5.priority = 97;
    pAsset5.action_death = (WorldAction) ((pSimObject, pTile) =>
    {
      if (pSimObject.a.isAdult())
        World.world.buildings.addBuilding("mineral_mythril", pTile, true);
      return true;
    });
    pAsset5.in_mutation_pot_add = true;
    pAsset5.in_mutation_pot_remove = true;
    this.add(pAsset5);
    this.t.setUnlockedWithAchievement("achievementGen5Worlds");
    this.t.addOpposite("death_grow_tree");
    this.t.addOpposite("death_grow_plant");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "death_grow_tree";
    pAsset6.group_id = "growth";
    pAsset6.rarity = Rarity.R0_Normal;
    pAsset6.priority = 95;
    pAsset6.action_death = new WorldAction(ActionLibrary.tryToGrowTree);
    pAsset6.in_mutation_pot_add = true;
    pAsset6.in_mutation_pot_remove = true;
    this.add(pAsset6);
    this.t.setUnlockedWithAchievement("achievementGen50Worlds");
    this.t.addOpposite("death_grow_plant");
    this.t.addOpposite("death_grow_mythril");
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "death_grow_plant";
    pAsset7.group_id = "growth";
    pAsset7.rarity = Rarity.R0_Normal;
    pAsset7.priority = 96 /*0x60*/;
    pAsset7.action_death = new WorldAction(ActionLibrary.tryToCreatePlants);
    pAsset7.in_mutation_pot_add = true;
    pAsset7.in_mutation_pot_remove = true;
    this.add(pAsset7);
    this.t.setUnlockedWithAchievement("achievementGen100Worlds");
    this.t.addOpposite("death_grow_tree");
    this.t.addOpposite("death_grow_mythril");
  }

  private void addSleepCycles()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "energy_preserver";
    pAsset1.group_id = "sleep_cycles";
    pAsset1.rarity = Rarity.R1_Rare;
    pAsset1.priority = 100;
    this.add(pAsset1);
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "polyphasic_sleep";
    pAsset2.group_id = "sleep_cycles";
    pAsset2.rarity = Rarity.R1_Rare;
    pAsset2.priority = 99;
    this.add(pAsset2);
    this.t.addDecision("polyphasic_sleep");
    this.t.addOpposite("monophasic_sleep");
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "monophasic_sleep";
    pAsset3.group_id = "sleep_cycles";
    pAsset3.rarity = Rarity.R1_Rare;
    pAsset3.priority = 98;
    this.add(pAsset3);
    this.t.addDecision("monophasic_sleep");
    this.t.addOpposite("polyphasic_sleep");
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "prolonged_rest";
    pAsset4.group_id = "sleep_cycles";
    pAsset4.rarity = Rarity.R1_Rare;
    pAsset4.priority = 97;
    this.add(pAsset4);
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "nocturnal_dormancy";
    pAsset5.group_id = "hibernation";
    pAsset5.rarity = Rarity.R2_Epic;
    pAsset5.priority = 100;
    this.add(pAsset5);
    this.t.addDecision("sleep_at_dark_age");
    this.t.addOpposite("chaos_driven");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "circadian_drift";
    pAsset6.group_id = "hibernation";
    pAsset6.priority = 99;
    this.add(pAsset6);
    this.t.addDecision("sleep_at_light_age");
    this.t.addOpposite("chaos_driven");
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "winter_slumberers";
    pAsset7.group_id = "hibernation";
    pAsset7.rarity = Rarity.R2_Epic;
    pAsset7.priority = 98;
    this.add(pAsset7);
    this.t.addDecision("sleep_at_winter_age");
    this.t.addOpposite("chaos_driven");
    SubspeciesTrait pAsset8 = new SubspeciesTrait();
    pAsset8.id = "chaos_driven";
    pAsset8.group_id = "hibernation";
    pAsset8.priority = 97;
    this.add(pAsset8);
    this.t.addDecision("sleep_when_not_chaos_age");
    this.t.addOpposite("nocturnal_dormancy");
    this.t.addOpposite("winter_slumberers");
    this.t.addOpposite("circadian_drift");
  }

  private void addOther()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "shiny_love";
    pAsset1.group_id = "chaos";
    this.add(pAsset1);
    this.t.setUnlockedWithAchievement("achievementPlanetOfApes");
    this.t.addDecision("try_to_steal_money");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "aggressive";
    pAsset2.group_id = "chaos";
    this.add(pAsset2);
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "genetic_mirror";
    pAsset3.group_id = "chaos";
    this.add(pAsset3);
    this.t.setUnlockedWithAchievement("achievementTraitExplorerSubspecies");
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "unstable_genome";
    pAsset4.group_id = "chaos";
    this.add(pAsset4);
    this.t.setUnlockedWithAchievement("achievementGenesExplorer");
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "pure";
    pAsset5.group_id = "mind";
    pAsset5.rarity = Rarity.R2_Epic;
    pAsset5.remove_for_zombies = true;
    this.add(pAsset5);
    this.t.setUnlockedWithAchievement("achievementCantBeTooMuch");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "super_positivity";
    pAsset6.group_id = "mind";
    pAsset6.rarity = Rarity.R0_Normal;
    pAsset6.in_mutation_pot_add = true;
    pAsset6.in_mutation_pot_remove = true;
    pAsset6.remove_for_zombies = true;
    this.add(pAsset6);
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "dreamweavers";
    pAsset7.group_id = "mind";
    pAsset7.in_mutation_pot_add = true;
    pAsset7.in_mutation_pot_remove = true;
    pAsset7.remove_for_zombies = true;
    this.add(pAsset7);
    this.t.setUnlockedWithAchievement("achievementMindlessHusk");
    this.t.addDecision("try_affect_dreams");
    SubspeciesTrait pAsset8 = new SubspeciesTrait();
    pAsset8.id = "telepathic_link";
    pAsset8.group_id = "mind";
    pAsset8.in_mutation_pot_add = true;
    pAsset8.in_mutation_pot_remove = true;
    pAsset8.remove_for_zombies = true;
    this.add(pAsset8);
    SubspeciesTrait pAsset9 = new SubspeciesTrait();
    pAsset9.id = "inquisitive_nature";
    pAsset9.group_id = "mind";
    pAsset9.rarity = Rarity.R0_Normal;
    pAsset9.in_mutation_pot_add = true;
    pAsset9.in_mutation_pot_remove = true;
    this.add(pAsset9);
    SubspeciesTrait pAsset10 = new SubspeciesTrait();
    pAsset10.id = "cautious_instincts";
    pAsset10.group_id = "mind";
    pAsset10.rarity = Rarity.R0_Normal;
    pAsset10.in_mutation_pot_add = true;
    pAsset10.in_mutation_pot_remove = true;
    this.add(pAsset10);
    SubspeciesTrait pAsset11 = new SubspeciesTrait();
    pAsset11.id = "aquatic";
    pAsset11.group_id = "body";
    pAsset11.rarity = Rarity.R0_Normal;
    pAsset11.in_mutation_pot_add = false;
    pAsset11.in_mutation_pot_remove = false;
    this.add(pAsset11);
    this.t.base_stats.addTag("water_creature");
    this.t.setUnlockedWithAchievement("achievementBoatsDisposal");
    this.t.addDecision("random_swim");
    SubspeciesTrait pAsset12 = new SubspeciesTrait();
    pAsset12.id = "hovering";
    pAsset12.group_id = "body";
    pAsset12.rarity = Rarity.R0_Normal;
    pAsset12.in_mutation_pot_add = true;
    pAsset12.in_mutation_pot_remove = true;
    this.add(pAsset12);
    SubspeciesTrait pAsset13 = new SubspeciesTrait();
    pAsset13.id = "pollinating";
    pAsset13.group_id = "body";
    pAsset13.rarity = Rarity.R0_Normal;
    pAsset13.in_mutation_pot_add = false;
    pAsset13.in_mutation_pot_remove = false;
    this.add(pAsset13);
    this.t.addDecision("pollinate");
    SubspeciesTrait pAsset14 = new SubspeciesTrait();
    pAsset14.id = "hydrophobia";
    pAsset14.group_id = "body";
    pAsset14.rarity = Rarity.R0_Normal;
    this.add(pAsset14);
    this.t.base_stats_meta.addTag("damaged_by_water");
  }

  private void addReproductionModes()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "reproduction_strategy_oviparity";
    pAsset1.group_id = "reproduction_strategy";
    pAsset1.rarity = Rarity.R0_Normal;
    pAsset1.priority = 100;
    pAsset1.in_mutation_pot_add = true;
    pAsset1.remove_for_zombies = true;
    this.add(pAsset1);
    this.t.action_on_augmentation_remove = (WorldActionTrait) ((pNanoObject, _) =>
    {
      if (pNanoObject.isRekt())
        return false;
      Subspecies subspecies = (Subspecies) pNanoObject;
      using (ListPool<string> pTraits = new ListPool<string>())
      {
        foreach (SubspeciesTrait trait in (IEnumerable<SubspeciesTrait>) subspecies.getTraits())
        {
          if (trait.phenotype_egg)
            pTraits.Add(trait.id);
        }
        if (pTraits.Count > 0)
          subspecies.removeTraits(pTraits);
        foreach (Actor unit in subspecies.getUnits())
        {
          if (unit.isEgg())
            unit.finishStatusEffect("egg");
        }
        return true;
      }
    });
    this.t.base_stats_meta["maturation"] = 1f;
    this.t.addOpposite("reproduction_strategy_viviparity");
    this.t.addOpposite("reproduction_budding");
    this.t.addOpposite("reproduction_vegetative");
    this.t.base_stats_meta.addTag("oviparity");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "reproduction_strategy_viviparity";
    pAsset2.group_id = "reproduction_strategy";
    pAsset2.rarity = Rarity.R0_Normal;
    pAsset2.priority = 99;
    pAsset2.in_mutation_pot_add = true;
    pAsset2.remove_for_zombies = true;
    this.add(pAsset2);
    this.t.base_stats_meta["maturation"] = 1f;
    this.t.addOpposite("reproduction_fission");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_strategy_oviparity");
    this.t.addOpposite("reproduction_budding");
    this.t.addOpposite("reproduction_vegetative");
    this.t.base_stats_meta.addTag("viviparity");
  }

  private void addReproduction()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "reproduction_sexual";
    pAsset1.group_id = "reproductive_methods";
    pAsset1.rarity = Rarity.R0_Normal;
    pAsset1.priority = 100;
    pAsset1.in_mutation_pot_add = true;
    pAsset1.remove_for_zombies = true;
    this.add(pAsset1);
    this.t.base_stats["birth_rate"] = 3f;
    this.t.addDecision("sexual_reproduction_try");
    this.t.addDecision("find_lover");
    this.t.base_stats_meta.addTag("reproduction_sexual");
    this.t.base_stats_meta.addTag("needs_mate");
    this.t.addOpposite("reproduction_hermaphroditic");
    this.t.addOpposite("reproduction_fission");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_vegetative");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "reproduction_spores";
    pAsset2.group_id = "reproductive_methods";
    pAsset2.priority = 99;
    pAsset2.in_mutation_pot_add = true;
    pAsset2.remove_for_zombies = true;
    this.add(pAsset2);
    this.t.addDecision("asexual_reproduction_spores");
    this.t.base_stats_meta.addTag("reproduction_asexual");
    this.t.addOpposite("reproduction_strategy_viviparity");
    this.t.addOpposite("reproduction_sexual");
    this.t.addOpposite("reproduction_hermaphroditic");
    this.t.addOpposite("reproduction_parthenogenesis");
    this.t.addOpposite("reproduction_fission");
    this.t.addOpposite("reproduction_vegetative");
    this.t.addOpposite("reproduction_divine");
    this.t.addOpposite("reproduction_budding");
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "reproduction_fission";
    pAsset3.group_id = "reproductive_methods";
    pAsset3.rarity = Rarity.R2_Epic;
    pAsset3.priority = 98;
    pAsset3.in_mutation_pot_add = true;
    pAsset3.remove_for_zombies = true;
    this.add(pAsset3);
    this.t.addDecision("asexual_reproduction_fission");
    this.t.base_stats_meta.addTag("reproduction_asexual");
    this.t.addOpposite("reproduction_strategy_viviparity");
    this.t.addOpposite("reproduction_sexual");
    this.t.addOpposite("reproduction_hermaphroditic");
    this.t.addOpposite("reproduction_parthenogenesis");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_vegetative");
    this.t.addOpposite("reproduction_divine");
    this.t.addOpposite("reproduction_budding");
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "reproduction_budding";
    pAsset4.group_id = "reproductive_methods";
    pAsset4.rarity = Rarity.R2_Epic;
    pAsset4.priority = 98;
    pAsset4.in_mutation_pot_add = true;
    pAsset4.remove_for_zombies = true;
    this.add(pAsset4);
    this.t.addDecision("asexual_reproduction_budding");
    this.t.base_stats_meta.addTag("reproduction_asexual");
    this.t.addOpposite("reproduction_strategy_viviparity");
    this.t.addOpposite("reproduction_strategy_oviparity");
    this.t.addOpposite("reproduction_parthenogenesis");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_vegetative");
    this.t.addOpposite("reproduction_divine");
    this.t.addOpposite("reproduction_fission");
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "reproduction_hermaphroditic";
    pAsset5.group_id = "reproductive_methods";
    pAsset5.rarity = Rarity.R0_Normal;
    pAsset5.priority = 97;
    pAsset5.in_mutation_pot_add = true;
    pAsset5.remove_for_zombies = true;
    this.add(pAsset5);
    this.t.addDecision("sexual_reproduction_try");
    this.t.addDecision("find_lover");
    this.t.base_stats_meta.addTag("reproduction_sexual");
    this.t.base_stats_meta.addTag("needs_mate");
    this.t.addOpposite("reproduction_sexual");
    this.t.addOpposite("reproduction_parthenogenesis");
    this.t.addOpposite("reproduction_fission");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_vegetative");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "reproduction_parthenogenesis";
    pAsset6.group_id = "reproductive_methods";
    pAsset6.rarity = Rarity.R1_Rare;
    pAsset6.priority = 96 /*0x60*/;
    pAsset6.in_mutation_pot_add = true;
    pAsset6.remove_for_zombies = true;
    this.add(pAsset6);
    this.t.addDecision("asexual_reproduction_parthenogenesis");
    this.t.base_stats_meta.addTag("reproduction_asexual");
    this.t.addOpposite("reproduction_hermaphroditic");
    this.t.addOpposite("reproduction_fission");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_vegetative");
    this.t.addOpposite("reproduction_divine");
    this.t.addOpposite("reproduction_budding");
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "reproduction_vegetative";
    pAsset7.group_id = "reproductive_methods";
    pAsset7.rarity = Rarity.R0_Normal;
    pAsset7.priority = 95;
    pAsset7.in_mutation_pot_add = true;
    pAsset7.remove_for_zombies = true;
    this.add(pAsset7);
    this.t.base_stats_meta["maturation"] = 12f;
    this.t.addDecision("asexual_reproduction_vegetative");
    this.t.base_stats_meta.addTag("reproduction_asexual");
    this.t.addOpposite("reproduction_strategy_oviparity");
    this.t.addOpposite("reproduction_strategy_viviparity");
    this.t.addOpposite("reproduction_sexual");
    this.t.addOpposite("reproduction_hermaphroditic");
    this.t.addOpposite("reproduction_parthenogenesis");
    this.t.addOpposite("reproduction_fission");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_divine");
    this.t.addOpposite("reproduction_budding");
    SubspeciesTrait pAsset8 = new SubspeciesTrait();
    pAsset8.id = "reproduction_divine";
    pAsset8.group_id = "reproductive_methods";
    pAsset8.rarity = Rarity.R2_Epic;
    pAsset8.priority = 94;
    pAsset8.remove_for_zombies = true;
    this.add(pAsset8);
    this.t.addDecision("asexual_reproduction_divine");
    this.t.addOpposite("reproduction_parthenogenesis");
    this.t.addOpposite("reproduction_fission");
    this.t.addOpposite("reproduction_spores");
    this.t.addOpposite("reproduction_vegetative");
    this.t.addOpposite("reproduction_budding");
    SubspeciesTrait pAsset9 = new SubspeciesTrait();
    pAsset9.id = "reproduction_soulborne";
    pAsset9.group_id = "reproductive_methods";
    pAsset9.priority = 93;
    pAsset9.remove_for_zombies = true;
    pAsset9.action_attack_target = (AttackAction) ((pSelf, pTarget, pTile) =>
    {
      if (!pTarget.isActor() || !pTarget.a.asset.has_soul)
        return false;
      pSelf.addStatusEffect("soul_harvested");
      return true;
    });
    this.add(pAsset9);
    SubspeciesTrait pAsset10 = new SubspeciesTrait();
    pAsset10.id = "reproduction_metamorph";
    pAsset10.group_id = "reproductive_methods";
    pAsset10.priority = 92;
    pAsset10.remove_for_zombies = true;
    pAsset10.action_attack_target = (AttackAction) ((pSelf, pTarget, pTile) =>
    {
      if (!pTarget.isActor() || !pTarget.a.canTurnIntoColdOne() || pTarget.a.subspecies == pSelf.a.subspecies)
        return false;
      Actor pBaby = ActionLibrary.turnIntoMetamorph((BaseSimObject) pTarget.a, pSelf.a.asset.id);
      if (pBaby != null)
      {
        pBaby.setParent1(pSelf.a);
        BabyHelper.applyParentsMeta(pSelf.a, (Actor) null, pBaby);
      }
      return true;
    });
    this.add(pAsset10);
  }

  private void addDiet()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "stomach";
    pAsset1.group_id = "body";
    pAsset1.rarity = Rarity.R0_Normal;
    pAsset1.priority = 100;
    pAsset1.remove_for_zombies = true;
    this.add(pAsset1);
    this.t.addDecision("try_to_eat_city_food");
    this.t.action_on_augmentation_add = (WorldActionTrait) ((pNanoObject, _) =>
    {
      ((MetaObjectWithTraits<SubspeciesData, SubspeciesTrait>) pNanoObject).addTrait("diet_omnivore");
      return true;
    });
    this.t.action_on_augmentation_remove = (WorldActionTrait) ((pNanoObject, _) =>
    {
      if (pNanoObject.isRekt())
        return false;
      Subspecies subspecies = (Subspecies) pNanoObject;
      using (ListPool<string> pTraits = new ListPool<string>())
      {
        foreach (SubspeciesTrait trait in (IEnumerable<SubspeciesTrait>) subspecies.getTraits())
        {
          if (trait.is_diet_related)
            pTraits.Add(trait.id);
        }
        if (pTraits.Count > 0)
          subspecies.removeTraits(pTraits);
        return true;
      }
    });
    this.t.base_stats_meta.addTag("needs_food");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "big_stomach";
    pAsset2.group_id = "body";
    pAsset2.rarity = Rarity.R1_Rare;
    pAsset2.priority = 99;
    pAsset2.is_diet_related = true;
    pAsset2.in_mutation_pot_add = true;
    pAsset2.remove_for_zombies = true;
    this.add(pAsset2);
    this.t.base_stats_meta["max_nutrition"] = 100f;
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "voracious";
    pAsset3.group_id = "body";
    pAsset3.rarity = Rarity.R0_Normal;
    pAsset3.priority = 98;
    this.add(pAsset3);
    this.t.base_stats_meta["metabolic_rate"] = 10f;
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "diet_frugivore";
    pAsset4.group_id = "diet";
    pAsset4.rarity = Rarity.R0_Normal;
    pAsset4.is_diet_related = true;
    pAsset4.in_mutation_pot_add = true;
    pAsset4.remove_for_zombies = true;
    this.add(pAsset4);
    this.t.addDecision("diet_fruits");
    this.t.addOpposite("diet_herbivore");
    this.t.addOpposite("diet_omnivore");
    this.t.base_stats_meta.addTag("diet_fruits");
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "diet_granivore";
    pAsset5.group_id = "diet";
    pAsset5.rarity = Rarity.R0_Normal;
    pAsset5.is_diet_related = true;
    pAsset5.in_mutation_pot_add = true;
    pAsset5.remove_for_zombies = true;
    this.add(pAsset5);
    this.t.addDecision("diet_crops");
    this.t.addOpposite("diet_herbivore");
    this.t.addOpposite("diet_omnivore");
    this.t.base_stats_meta.addTag("diet_crops");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "diet_florivore";
    pAsset6.group_id = "diet";
    pAsset6.rarity = Rarity.R0_Normal;
    pAsset6.is_diet_related = true;
    pAsset6.in_mutation_pot_add = true;
    pAsset6.remove_for_zombies = true;
    this.add(pAsset6);
    this.t.addDecision("diet_flowers");
    this.t.addOpposite("diet_herbivore");
    this.t.addOpposite("diet_omnivore");
    this.t.base_stats_meta.addTag("diet_flowers");
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "diet_graminivore";
    pAsset7.group_id = "diet";
    pAsset7.rarity = Rarity.R1_Rare;
    pAsset7.is_diet_related = true;
    pAsset7.in_mutation_pot_add = false;
    pAsset7.remove_for_zombies = true;
    this.add(pAsset7);
    this.t.addDecision("diet_grass");
    this.t.base_stats_meta.addTag("diet_grass");
    SubspeciesTrait pAsset8 = new SubspeciesTrait();
    pAsset8.id = "diet_xylophagy";
    pAsset8.group_id = "diet";
    pAsset8.rarity = Rarity.R2_Epic;
    pAsset8.is_diet_related = true;
    pAsset8.in_mutation_pot_add = true;
    pAsset8.remove_for_zombies = true;
    this.add(pAsset8);
    this.t.addDecision("diet_wood");
    this.t.base_stats_meta.addTag("diet_wood");
    SubspeciesTrait pAsset9 = new SubspeciesTrait();
    pAsset9.id = "diet_geophagy";
    pAsset9.group_id = "diet";
    pAsset9.rarity = Rarity.R2_Epic;
    pAsset9.is_diet_related = true;
    pAsset9.remove_for_zombies = true;
    pAsset9.spawn_random_trait_allowed = false;
    this.add(pAsset9);
    this.t.addDecision("diet_tiles");
    this.t.base_stats_meta.addTag("diet_tiles");
    SubspeciesTrait pAsset10 = new SubspeciesTrait();
    pAsset10.id = "diet_folivore";
    pAsset10.group_id = "diet";
    pAsset10.rarity = Rarity.R0_Normal;
    pAsset10.is_diet_related = true;
    pAsset10.in_mutation_pot_add = true;
    pAsset10.remove_for_zombies = true;
    this.add(pAsset10);
    this.t.addDecision("diet_vegetation");
    this.t.addOpposite("diet_herbivore");
    this.t.addOpposite("diet_omnivore");
    this.t.base_stats_meta.addTag("diet_vegetation");
    SubspeciesTrait pAsset11 = new SubspeciesTrait();
    pAsset11.id = "diet_carnivore";
    pAsset11.group_id = "diet";
    pAsset11.rarity = Rarity.R0_Normal;
    pAsset11.priority = 98;
    pAsset11.is_diet_related = true;
    pAsset11.in_mutation_pot_add = true;
    pAsset11.remove_for_zombies = true;
    this.add(pAsset11);
    this.t.addDecision("diet_meat");
    this.t.addOpposite("diet_omnivore");
    this.t.base_stats_meta.addTag("diet_meat");
    SubspeciesTrait pAsset12 = new SubspeciesTrait();
    pAsset12.id = "diet_piscivore";
    pAsset12.group_id = "diet";
    pAsset12.rarity = Rarity.R0_Normal;
    pAsset12.is_diet_related = true;
    pAsset12.in_mutation_pot_add = true;
    pAsset12.remove_for_zombies = true;
    this.add(pAsset12);
    this.t.addDecision("diet_fish");
    this.t.addOpposite("diet_omnivore");
    this.t.addOpposite("diet_herbivore");
    this.t.base_stats_meta.addTag("diet_fish");
    SubspeciesTrait pAsset13 = new SubspeciesTrait();
    pAsset13.id = "diet_lithotroph";
    pAsset13.group_id = "diet";
    pAsset13.rarity = Rarity.R1_Rare;
    pAsset13.is_diet_related = true;
    pAsset13.in_mutation_pot_add = true;
    pAsset13.remove_for_zombies = true;
    this.add(pAsset13);
    this.t.addDecision("diet_minerals");
    this.t.base_stats_meta.addTag("diet_minerals");
    SubspeciesTrait pAsset14 = new SubspeciesTrait();
    pAsset14.id = "diet_insectivore";
    pAsset14.group_id = "diet";
    pAsset14.rarity = Rarity.R0_Normal;
    pAsset14.is_diet_related = true;
    pAsset14.in_mutation_pot_add = true;
    pAsset14.remove_for_zombies = true;
    this.add(pAsset14);
    this.t.addDecision("diet_meat_insect");
    this.t.base_stats_meta.addTag("diet_meat_insect");
    SubspeciesTrait pAsset15 = new SubspeciesTrait();
    pAsset15.id = "diet_algivore";
    pAsset15.group_id = "diet";
    pAsset15.rarity = Rarity.R0_Normal;
    pAsset15.is_diet_related = true;
    pAsset15.in_mutation_pot_add = true;
    pAsset15.remove_for_zombies = true;
    this.add(pAsset15);
    this.t.addDecision("diet_algae");
    this.t.base_stats_meta.addTag("diet_algae");
    SubspeciesTrait pAsset16 = new SubspeciesTrait();
    pAsset16.id = "diet_cannibalism";
    pAsset16.group_id = "diet";
    pAsset16.priority = 1;
    pAsset16.is_diet_related = true;
    pAsset16.rarity = Rarity.R1_Rare;
    pAsset16.in_mutation_pot_add = true;
    pAsset16.remove_for_zombies = true;
    this.add(pAsset16);
    this.t.setUnlockedWithAchievement("achievementClannibals");
    this.t.addDecision("diet_same_species");
    this.t.base_stats_meta.addTag("diet_same_species");
    SubspeciesTrait pAsset17 = new SubspeciesTrait();
    pAsset17.id = "diet_nectarivore";
    pAsset17.group_id = "diet";
    pAsset17.is_diet_related = true;
    pAsset17.rarity = Rarity.R0_Normal;
    pAsset17.in_mutation_pot_add = true;
    pAsset17.remove_for_zombies = true;
    this.add(pAsset17);
    this.t.addDecision("diet_nectar");
    this.t.base_stats_meta.addTag("diet_nectar");
    SubspeciesTrait pAsset18 = new SubspeciesTrait();
    pAsset18.id = "diet_hematophagy";
    pAsset18.group_id = "diet";
    pAsset18.is_diet_related = true;
    pAsset18.in_mutation_pot_add = true;
    pAsset18.remove_for_zombies = true;
    this.add(pAsset18);
    this.t.addDecision("diet_blood");
    this.t.base_stats_meta.addTag("diet_blood");
    SubspeciesTrait pAsset19 = new SubspeciesTrait();
    pAsset19.id = "diet_herbivore";
    pAsset19.group_id = "diet";
    pAsset19.rarity = Rarity.R1_Rare;
    pAsset19.priority = 99;
    pAsset19.is_diet_related = true;
    pAsset19.in_mutation_pot_add = true;
    pAsset19.remove_for_zombies = true;
    this.add(pAsset19);
    this.t.addDecision("diet_fruits");
    this.t.addDecision("diet_vegetation");
    this.t.addDecision("diet_flowers");
    this.t.addDecision("diet_grass");
    this.t.addDecision("diet_crops");
    this.t.addOpposite("diet_frugivore");
    this.t.addOpposite("diet_granivore");
    this.t.addOpposite("diet_florivore");
    this.t.addOpposite("diet_folivore");
    this.t.addOpposite("diet_piscivore");
    this.t.addOpposite("diet_omnivore");
    this.t.base_stats_meta.addTag("diet_flowers");
    this.t.base_stats_meta.addTag("diet_fruits");
    this.t.base_stats_meta.addTag("diet_crops");
    this.t.base_stats_meta.addTag("diet_vegetation");
    this.t.base_stats_meta.addTag("diet_grass");
    SubspeciesTrait pAsset20 = new SubspeciesTrait();
    pAsset20.id = "diet_omnivore";
    pAsset20.group_id = "diet";
    pAsset20.rarity = Rarity.R1_Rare;
    pAsset20.priority = 100;
    pAsset20.is_diet_related = true;
    pAsset20.in_mutation_pot_add = true;
    pAsset20.remove_for_zombies = true;
    this.add(pAsset20);
    this.t.addDecision("diet_fruits");
    this.t.addDecision("diet_vegetation");
    this.t.addDecision("diet_meat");
    this.t.addOpposite("diet_frugivore");
    this.t.addOpposite("diet_granivore");
    this.t.addOpposite("diet_florivore");
    this.t.addOpposite("diet_folivore");
    this.t.addOpposite("diet_carnivore");
    this.t.addOpposite("diet_piscivore");
    this.t.addOpposite("diet_herbivore");
    this.t.base_stats_meta.addTag("diet_flowers");
    this.t.base_stats_meta.addTag("diet_fruits");
    this.t.base_stats_meta.addTag("diet_crops");
    this.t.base_stats_meta.addTag("diet_vegetation");
    this.t.base_stats_meta.addTag("diet_meat");
    this.t.base_stats_meta.addTag("diet_fish");
  }

  private void addGenetic()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "advanced_hippocampus";
    pAsset1.group_id = "advanced_brain";
    pAsset1.rarity = Rarity.R1_Rare;
    pAsset1.in_mutation_pot_add = true;
    pAsset1.remove_for_zombies = true;
    this.add(pAsset1);
    this.t.addDecision("try_to_read");
    this.t.base_stats_meta.addTag("has_advanced_memory");
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "wernicke_area";
    pAsset2.group_id = "advanced_brain";
    pAsset2.in_mutation_pot_add = true;
    pAsset2.remove_for_zombies = true;
    this.add(pAsset2);
    this.t.addDecision("socialize_initial_check");
    this.t.base_stats_meta.addTag("has_advanced_communication");
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "amygdala";
    pAsset3.group_id = "advanced_brain";
    pAsset3.rarity = Rarity.R2_Epic;
    pAsset3.in_mutation_pot_add = true;
    pAsset3.remove_for_zombies = true;
    this.add(pAsset3);
    this.t.addDecision("run_away_from_carnivore");
    this.t.addDecision("run_away");
    this.t.addDecision("reflection");
    this.t.base_stats_meta.addTag("has_emotions");
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "prefrontal_cortex";
    pAsset4.group_id = "advanced_brain";
    pAsset4.in_mutation_pot_add = true;
    pAsset4.remove_for_zombies = true;
    pAsset4.priority = 100;
    this.add(pAsset4);
    this.t.addDecision("check_lover_city");
    this.t.addDecision("find_city_job");
    this.t.addDecision("build_civ_city_here");
    this.t.addDecision("try_to_return_to_home_city");
    this.t.addDecision("try_to_start_new_civilization");
    this.t.addDecision("check_join_city");
    this.t.addDecision("check_join_empty_nearby_city");
    this.t.base_stats_meta.addTag("has_sapience");
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "bad_genes";
    pAsset5.group_id = "body";
    pAsset5.rarity = Rarity.R0_Normal;
    pAsset5.in_mutation_pot_add = true;
    pAsset5.in_mutation_pot_remove = true;
    pAsset5.remove_for_zombies = true;
    pAsset5.action_growth = (WorldAction) ((pSimObject, _) =>
    {
      if (Randy.randomChance(0.01f))
      {
        string random = SubspeciesTraitLibrary._bad_genes.GetRandom<string>();
        pSimObject.a.addTrait(random);
      }
      return true;
    });
    this.add(pAsset5);
    this.t.setUnlockedWithAchievement("achievementFastLiving");
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "photosynthetic_skin";
    pAsset6.group_id = "diet";
    pAsset6.rarity = Rarity.R2_Epic;
    pAsset6.in_mutation_pot_add = true;
    pAsset6.remove_for_zombies = true;
    pAsset6.special_effect_interval = 10f;
    pAsset6.action_special_effect = (WorldAction) ((pSimObject, _) =>
    {
      if (World.world.era_manager.getCurrentAge().flag_night)
        return false;
      int pVal = Randy.randomInt(2, 10);
      pSimObject.a.addNutritionFromEating(pVal);
      return true;
    });
    this.add(pAsset6);
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "genetic_psychosis";
    pAsset7.group_id = "mind";
    pAsset7.rarity = Rarity.R2_Epic;
    pAsset7.in_mutation_pot_add = true;
    pAsset7.in_mutation_pot_remove = true;
    pAsset7.remove_for_zombies = true;
    pAsset7.action_growth = (WorldAction) ((pSimObject, _) =>
    {
      if (pSimObject.a.isPrettyOld() && Randy.randomChance(0.01f))
        pSimObject.a.addTrait("madness");
      return true;
    });
    this.add(pAsset7);
    SubspeciesTrait pAsset8 = new SubspeciesTrait();
    pAsset8.id = "bioluminescence";
    pAsset8.group_id = "body";
    pAsset8.in_mutation_pot_add = true;
    pAsset8.in_mutation_pot_remove = true;
    this.add(pAsset8);
    this.t.base_stats.addTag("generate_light");
    SubspeciesTrait pAsset9 = new SubspeciesTrait();
    pAsset9.id = "accelerated_healing";
    pAsset9.group_id = "body";
    pAsset9.rarity = Rarity.R1_Rare;
    pAsset9.in_mutation_pot_add = true;
    pAsset9.in_mutation_pot_remove = true;
    pAsset9.remove_for_zombies = true;
    pAsset9.action_growth = (WorldAction) ((pSimObject, _) =>
    {
      Actor a = pSimObject.a;
      IReadOnlyCollection<ActorTrait> traits = pSimObject.a.getTraits();
      using (ListPool<ActorTrait> pTraits = new ListPool<ActorTrait>())
      {
        foreach (ActorTrait actorTrait in (IEnumerable<ActorTrait>) traits)
        {
          if (actorTrait.can_be_removed_by_accelerated_healing)
            pTraits.Add(actorTrait);
        }
        if (pTraits.Count > 0)
        {
          a.removeTraits((ICollection<ActorTrait>) pTraits);
          a.setStatsDirty();
        }
        return true;
      }
    });
    this.add(pAsset9);
    SubspeciesTrait pAsset10 = new SubspeciesTrait();
    pAsset10.id = "rapid_aging";
    pAsset10.group_id = "growth";
    pAsset10.rarity = Rarity.R1_Rare;
    pAsset10.in_mutation_pot_add = true;
    pAsset10.in_mutation_pot_remove = true;
    pAsset10.remove_for_zombies = true;
    pAsset10.action_growth = (WorldAction) ((pSimObject, _) =>
    {
      if (Randy.randomChance(0.5f))
        ++pSimObject.a.data.age_overgrowth;
      if (Randy.randomChance(0.5f))
        ++pSimObject.a.data.age_overgrowth;
      return true;
    });
    this.add(pAsset10);
    SubspeciesTrait pAsset11 = new SubspeciesTrait();
    pAsset11.id = "good_throwers";
    pAsset11.group_id = "body";
    pAsset11.rarity = Rarity.R1_Rare;
    pAsset11.in_mutation_pot_add = true;
    pAsset11.in_mutation_pot_remove = true;
    this.add(pAsset11);
    this.t.setUnlockedWithAchievement("achievementBallToBall");
    this.t.base_stats["throwing_range"] = 6f;
    SubspeciesTrait pAsset12 = new SubspeciesTrait();
    pAsset12.id = "fast_builders";
    pAsset12.group_id = "mind";
    pAsset12.rarity = Rarity.R1_Rare;
    pAsset12.in_mutation_pot_add = true;
    pAsset12.in_mutation_pot_remove = true;
    this.add(pAsset12);
    this.t.setUnlockedWithAchievement("achievementCustomWorld");
    this.t.addOpposite("slow_builders");
    this.t.base_stats_meta["construction_speed"] = 2f;
    SubspeciesTrait pAsset13 = new SubspeciesTrait();
    pAsset13.id = "slow_builders";
    pAsset13.group_id = "mind";
    pAsset13.rarity = Rarity.R1_Rare;
    pAsset13.in_mutation_pot_add = true;
    pAsset13.in_mutation_pot_remove = true;
    this.add(pAsset13);
    this.t.addOpposite("fast_builders");
    this.t.base_stats_meta["construction_speed"] = -1f;
    SubspeciesTrait pAsset14 = new SubspeciesTrait();
    pAsset14.id = "fins";
    pAsset14.group_id = "body";
    pAsset14.rarity = Rarity.R1_Rare;
    pAsset14.in_mutation_pot_add = true;
    pAsset14.in_mutation_pot_remove = true;
    this.add(pAsset14);
    this.t.setUnlockedWithAchievement("achievementPiranhaLand");
    this.t.base_stats.addTag("fast_swimming");
    SubspeciesTrait pAsset15 = new SubspeciesTrait();
    pAsset15.id = "heat_resistance";
    pAsset15.group_id = "body";
    pAsset15.rarity = Rarity.R1_Rare;
    pAsset15.in_mutation_pot_add = true;
    pAsset15.in_mutation_pot_remove = true;
    this.add(pAsset15);
    this.t.setUnlockedWithAchievement("achievementFlickIt");
    this.t.base_stats.addTag("immunity_fire");
    this.t.base_stats_meta.addTag("can_build_in_biome_infernal");
    SubspeciesTrait pAsset16 = new SubspeciesTrait();
    pAsset16.id = "cold_resistance";
    pAsset16.group_id = "body";
    pAsset16.rarity = Rarity.R1_Rare;
    pAsset16.in_mutation_pot_add = true;
    pAsset16.in_mutation_pot_remove = true;
    this.add(pAsset16);
    this.t.base_stats.addTag("immunity_cold");
    this.t.base_stats_meta.addTag("can_build_in_biome_permafrost");
  }

  private void addStats()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "exoskeleton";
    pAsset1.group_id = "body";
    pAsset1.rarity = Rarity.R0_Normal;
    pAsset1.in_mutation_pot_add = true;
    pAsset1.in_mutation_pot_remove = true;
    this.add(pAsset1);
    this.t.base_stats["armor"] = 10f;
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "long_lifespan";
    pAsset2.group_id = "body";
    pAsset2.rarity = Rarity.R0_Normal;
    pAsset2.in_mutation_pot_add = true;
    pAsset2.in_mutation_pot_remove = true;
    pAsset2.remove_for_zombies = true;
    this.add(pAsset2);
    this.t.base_stats["lifespan"] = 100f;
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "hyper_intelligence";
    pAsset3.group_id = "mind";
    pAsset3.rarity = Rarity.R0_Normal;
    pAsset3.in_mutation_pot_add = true;
    pAsset3.in_mutation_pot_remove = true;
    pAsset3.remove_for_zombies = true;
    this.add(pAsset3);
    this.t.base_stats["intelligence"] = 30f;
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "enhanced_strength";
    pAsset4.group_id = "body";
    pAsset4.rarity = Rarity.R0_Normal;
    pAsset4.in_mutation_pot_add = true;
    pAsset4.in_mutation_pot_remove = true;
    this.add(pAsset4);
    this.t.setUnlockedWithAchievement("achievementSuperMushroom");
    this.t.base_stats["damage"] = 50f;
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "high_fecundity";
    pAsset5.group_id = "body";
    pAsset5.rarity = Rarity.R0_Normal;
    pAsset5.in_mutation_pot_add = true;
    pAsset5.in_mutation_pot_remove = true;
    pAsset5.remove_for_zombies = true;
    this.add(pAsset5);
    this.t.setUnlockedWithAchievement("achievement10000Creatures");
    this.t.base_stats["birth_rate"] = 5f;
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "unmoving";
    pAsset6.group_id = "body";
    pAsset6.rarity = Rarity.R0_Normal;
    pAsset6.in_mutation_pot_add = false;
    pAsset6.remove_for_zombies = false;
    pAsset6.spawn_random_trait_allowed = false;
    this.add(pAsset6);
    this.t.setUnlockedWithAchievement("achievementSimpleStupidGenetics");
    this.t.base_stats.addTag("immovable");
  }

  private void addLimits()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "population_minimal";
    pAsset1.group_id = "harmony";
    pAsset1.rarity = Rarity.R0_Normal;
    pAsset1.in_mutation_pot_remove = false;
    pAsset1.in_mutation_pot_add = false;
    pAsset1.spawn_random_trait_allowed = false;
    pAsset1.priority = 100;
    this.add(pAsset1);
    this.t.addOpposite("population_small");
    this.t.addOpposite("population_moderate");
    this.t.addOpposite("population_large");
    this.t.addOpposite("population_expansive");
    this.t.base_stats_meta["limit_population"] = 50f;
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "population_small";
    pAsset2.group_id = "harmony";
    pAsset2.rarity = Rarity.R0_Normal;
    pAsset2.in_mutation_pot_remove = false;
    pAsset2.in_mutation_pot_add = false;
    pAsset2.spawn_random_trait_allowed = false;
    pAsset2.priority = 99;
    this.add(pAsset2);
    this.t.addOpposite("population_minimal");
    this.t.addOpposite("population_moderate");
    this.t.addOpposite("population_large");
    this.t.addOpposite("population_expansive");
    this.t.base_stats_meta["limit_population"] = 100f;
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "population_moderate";
    pAsset3.group_id = "harmony";
    pAsset3.rarity = Rarity.R0_Normal;
    pAsset3.in_mutation_pot_remove = false;
    pAsset3.in_mutation_pot_add = false;
    pAsset3.spawn_random_trait_allowed = false;
    pAsset3.priority = 98;
    this.add(pAsset3);
    this.t.addOpposite("population_small");
    this.t.addOpposite("population_minimal");
    this.t.addOpposite("population_large");
    this.t.addOpposite("population_expansive");
    this.t.base_stats_meta["limit_population"] = 500f;
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "population_large";
    pAsset4.group_id = "harmony";
    pAsset4.rarity = Rarity.R0_Normal;
    pAsset4.in_mutation_pot_remove = false;
    pAsset4.in_mutation_pot_add = false;
    pAsset4.spawn_random_trait_allowed = false;
    pAsset4.priority = 97;
    this.add(pAsset4);
    this.t.addOpposite("population_small");
    this.t.addOpposite("population_minimal");
    this.t.addOpposite("population_moderate");
    this.t.addOpposite("population_expansive");
    this.t.base_stats_meta["limit_population"] = 1000f;
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "population_expansive";
    pAsset5.group_id = "harmony";
    pAsset5.rarity = Rarity.R0_Normal;
    pAsset5.in_mutation_pot_remove = false;
    pAsset5.in_mutation_pot_add = false;
    pAsset5.spawn_random_trait_allowed = false;
    pAsset5.priority = 96 /*0x60*/;
    this.add(pAsset5);
    this.t.addOpposite("population_small");
    this.t.addOpposite("population_minimal");
    this.t.addOpposite("population_moderate");
    this.t.addOpposite("population_large");
    this.t.base_stats_meta["limit_population"] = 3000f;
  }

  private void addMaturation()
  {
    SubspeciesTrait pAsset1 = new SubspeciesTrait();
    pAsset1.id = "gestation_short";
    pAsset1.group_id = "gestation";
    pAsset1.rarity = Rarity.R0_Normal;
    pAsset1.priority = 100;
    pAsset1.in_mutation_pot_add = true;
    pAsset1.remove_for_zombies = true;
    this.add(pAsset1);
    this.t.addOpposite("gestation_moderate");
    this.t.addOpposite("gestation_long");
    this.t.addOpposite("gestation_very_long");
    this.t.addOpposite("gestation_extremely_long");
    this.t.base_stats_meta["maturation"] = 2f;
    SubspeciesTrait pAsset2 = new SubspeciesTrait();
    pAsset2.id = "gestation_moderate";
    pAsset2.group_id = "gestation";
    pAsset2.rarity = Rarity.R0_Normal;
    pAsset2.priority = 98;
    pAsset2.in_mutation_pot_add = true;
    pAsset2.remove_for_zombies = true;
    this.add(pAsset2);
    this.t.addOpposite("gestation_short");
    this.t.addOpposite("gestation_long");
    this.t.addOpposite("gestation_very_long");
    this.t.addOpposite("gestation_extremely_long");
    this.t.base_stats_meta["maturation"] = 4f;
    SubspeciesTrait pAsset3 = new SubspeciesTrait();
    pAsset3.id = "gestation_long";
    pAsset3.group_id = "gestation";
    pAsset3.rarity = Rarity.R0_Normal;
    pAsset3.priority = 97;
    pAsset3.in_mutation_pot_add = true;
    pAsset3.remove_for_zombies = true;
    this.add(pAsset3);
    this.t.addOpposite("gestation_short");
    this.t.addOpposite("gestation_moderate");
    this.t.addOpposite("gestation_very_long");
    this.t.addOpposite("gestation_extremely_long");
    this.t.base_stats_meta["maturation"] = 9f;
    SubspeciesTrait pAsset4 = new SubspeciesTrait();
    pAsset4.id = "gestation_very_long";
    pAsset4.group_id = "gestation";
    pAsset4.rarity = Rarity.R0_Normal;
    pAsset4.priority = 96 /*0x60*/;
    pAsset4.in_mutation_pot_add = true;
    pAsset4.remove_for_zombies = true;
    this.add(pAsset4);
    this.t.addOpposite("gestation_short");
    this.t.addOpposite("gestation_moderate");
    this.t.addOpposite("gestation_long");
    this.t.addOpposite("gestation_extremely_long");
    this.t.base_stats_meta["maturation"] = 20f;
    SubspeciesTrait pAsset5 = new SubspeciesTrait();
    pAsset5.id = "gestation_extremely_long";
    pAsset5.group_id = "gestation";
    pAsset5.rarity = Rarity.R1_Rare;
    pAsset5.priority = 95;
    pAsset5.in_mutation_pot_add = true;
    pAsset5.remove_for_zombies = true;
    this.add(pAsset5);
    this.t.addOpposite("gestation_short");
    this.t.addOpposite("gestation_moderate");
    this.t.addOpposite("gestation_long");
    this.t.addOpposite("gestation_very_long");
    this.t.base_stats_meta["maturation"] = 50f;
    SubspeciesTrait pAsset6 = new SubspeciesTrait();
    pAsset6.id = "gmo";
    pAsset6.group_id = "special";
    pAsset6.priority = 94;
    pAsset6.can_be_removed = false;
    pAsset6.can_be_given = false;
    pAsset6.spawn_random_trait_allowed = false;
    this.add(pAsset6);
    SubspeciesTrait pAsset7 = new SubspeciesTrait();
    pAsset7.id = "uplifted";
    pAsset7.group_id = "special";
    pAsset7.priority = 93;
    pAsset7.can_be_removed = false;
    pAsset7.can_be_given = false;
    pAsset7.spawn_random_trait_allowed = false;
    this.add(pAsset7);
  }

  private void addAdaptations()
  {
    SubspeciesTrait pAsset = new SubspeciesTrait();
    pAsset.id = "$adaptation$";
    pAsset.group_id = "adaptations";
    pAsset.remove_for_zombies = true;
    this.add(pAsset);
    this.clone("adaptation_desert", "$adaptation$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.base_stats_meta.addTag("can_build_in_biome_desert");
    this.t.base_stats.addTag("walk_adaptation_sand");
    this.clone("adaptation_swamp", "$adaptation$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.base_stats_meta.addTag("can_build_in_biome_swamp");
    this.t.base_stats.addTag("walk_adaptation_swamp");
    this.clone("adaptation_wasteland", "$adaptation$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.base_stats_meta.addTag("can_build_in_biome_wasteland");
    this.clone("adaptation_corruption", "$adaptation$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.base_stats_meta.addTag("can_build_in_biome_corruption");
    this.clone("adaptation_permafrost", "$adaptation$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.base_stats_meta.addTag("can_build_in_biome_permafrost");
    this.t.base_stats.addTag("walk_adaptation_snow");
    this.clone("adaptation_infernal", "$adaptation$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.base_stats_meta.addTag("can_build_in_biome_infernal");
  }

  private void addMutations()
  {
    SubspeciesTrait pAsset = new SubspeciesTrait();
    pAsset.id = "$skin_mutation$";
    pAsset.group_id = "mutations";
    pAsset.remove_for_zombies = true;
    pAsset.is_mutation_skin = true;
    pAsset.animation_walk = ActorAnimationSequences.walk_0_3;
    pAsset.animation_swim = ActorAnimationSequences.swim_0_3;
    pAsset.skin_citizen_male = AssetLibrary<SubspeciesTrait>.l<string>("male_1");
    pAsset.skin_citizen_female = AssetLibrary<SubspeciesTrait>.l<string>("female_1");
    pAsset.skin_warrior = AssetLibrary<SubspeciesTrait>.l<string>("warrior_1");
    pAsset.render_heads_for_children = true;
    this.add(pAsset);
    this.clone("mutation_skin_burger", "$skin_mutation$");
    this.t.setUnlockedWithAchievement("achievementBurger");
    this.t.priority = 92;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_burger";
    this.t.render_heads_for_children = false;
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_light_orb", "$skin_mutation$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.priority = 93;
    this.t.animation_idle = ActorAnimationSequences.walk_0_3;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_light_orb";
    this.t.prevent_unconscious_rotation = true;
    this.t.base_stats_meta.addTag("always_idle_animation");
    this.t.shadow = false;
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_living_rock", "$skin_mutation$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.priority = 92;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_living_rock";
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_tentacle_horror", "$skin_mutation$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.priority = 92;
    this.t.animation_idle = ActorAnimationSequences.walk_0_3;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_tentacle_horror";
    this.t.prevent_unconscious_rotation = true;
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_abomination", "$skin_mutation$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.priority = 92;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_abomination";
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_fractal", "$skin_mutation$");
    this.t.priority = 92;
    this.t.animation_walk = ActorAnimationSequences.walk_0_5;
    this.t.animation_idle = ActorAnimationSequences.walk_0_5;
    this.t.animation_swim = ActorAnimationSequences.swim_0_5;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_fractal";
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_void", "$skin_mutation$");
    this.t.priority = 92;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_void";
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_metalic_orb", "$skin_mutation$");
    this.t.setUnlockedWithAchievement("achievementBackToBetaTesting");
    this.t.rarity = Rarity.R2_Epic;
    this.t.priority = 92;
    this.t.animation_idle = ActorAnimationSequences.walk_0_3;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_metalic_orb";
    this.t.prevent_unconscious_rotation = true;
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_blood_vortex", "$skin_mutation$");
    this.t.priority = 92;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_blood_vortex";
    this.t.shadow_texture = "unitShadow_6";
    this.t.shadow_texture_baby = "unitShadow_5";
    this.loadSpritesPaths(this.t);
    this.clone("mutation_skin_energy", "$skin_mutation$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.priority = 92;
    this.t.animation_idle = ActorAnimationSequences.walk_0_3;
    this.t.sprite_path = "actors/species/mutations/mutation_skin_energy";
    this.t.prevent_unconscious_rotation = true;
    this.t.base_stats_meta.addTag("always_idle_animation");
    this.t.shadow = false;
    this.loadSpritesPaths(this.t);
    this.addMutationOpposites();
  }

  private void addEggs()
  {
    SubspeciesTrait pAsset = new SubspeciesTrait();
    pAsset.id = "$egg$";
    pAsset.group_id = "eggs";
    pAsset.phenotype_egg = true;
    this.add(pAsset);
    this.t.action_on_augmentation_add = (WorldActionTrait) ((pNanoObject, _) =>
    {
      Subspecies subspecies = (Subspecies) pNanoObject;
      if (!subspecies.hasTrait("reproduction_strategy_oviparity"))
        subspecies.addTrait("reproduction_strategy_oviparity", true);
      return true;
    });
    this.clone("egg_shell_plain", "$egg$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.base_stats_meta["maturation"] = 3f;
    this.clone("egg_shell_spotted", "$egg$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.base_stats_meta["maturation"] = 3f;
    this.clone("egg_colored", "$egg$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.base_stats_meta["maturation"] = 3f;
    this.clone("egg_roe", "$egg$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.base_stats_meta["maturation"] = 3f;
    this.clone("egg_face", "$egg$");
    this.t.base_stats_meta["maturation"] = 5f;
    this.clone("egg_orb", "$egg$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.base_stats_meta["maturation"] = 6f;
    this.clone("egg_eyeball", "$egg$");
    this.t.setUnlockedWithAchievement("achievementGodMode");
    this.t.rarity = Rarity.R1_Rare;
    this.t.animation_idle = ActorAnimationSequences.walk_0_3;
    this.t.base_stats_meta["maturation"] = 4f;
    this.clone("egg_alien", "$egg$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.base_stats_meta["maturation"] = 7f;
    this.clone("egg_cocoon", "$egg$");
    this.t.rarity = Rarity.R0_Normal;
    this.t.base_stats_meta["maturation"] = 6f;
    this.clone("egg_metal_box", "$egg$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.base_stats_meta["maturation"] = 15f;
    this.clone("egg_crystal", "$egg$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.base_stats_meta["maturation"] = 10f;
    this.clone("egg_ice", "$egg$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.after_hatch_from_egg_action = (AfterHatchFromEggAction) (pActor => ActionLibrary.snowDropsSpawn((BaseSimObject) pActor));
    this.t.base_stats_meta["maturation"] = 8f;
    this.clone("egg_blob", "$egg$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.base_stats_meta["maturation"] = 2f;
    this.clone("egg_candy", "$egg$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.base_stats_meta["maturation"] = 3f;
    this.clone("egg_bubble", "$egg$");
    this.t.rarity = Rarity.R1_Rare;
    this.t.base_stats_meta["maturation"] = 1f;
    this.clone("egg_rainbow", "$egg$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.base_stats_meta["maturation"] = 3f;
    this.clone("egg_pumpkin", "$egg$");
    this.t.setUnlockedWithAchievement("achievementSocialNetwork");
    this.t.base_stats_meta["maturation"] = 5f;
    this.clone("egg_flames", "$egg$");
    this.t.rarity = Rarity.R2_Epic;
    this.t.after_hatch_from_egg_action = (AfterHatchFromEggAction) (pActor => ActionLibrary.fireDropsSpawn((BaseSimObject) pActor));
    this.t.base_stats_meta["maturation"] = 6f;
    this.addEggOpposites();
  }

  public override void post_init()
  {
    base.post_init();
    foreach (SubspeciesTrait subspeciesTrait in this.list)
    {
      if (subspeciesTrait.phenotype_egg)
      {
        if (string.IsNullOrEmpty(subspeciesTrait.id_egg))
          subspeciesTrait.id_egg = subspeciesTrait.id;
        subspeciesTrait.sprite_path = "eggs/" + subspeciesTrait.id_egg;
      }
      if (subspeciesTrait.shadow && subspeciesTrait.is_mutation_skin)
        subspeciesTrait.texture_asset.loadShadow();
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (SubspeciesTrait pObject in this.list)
    {
      if (pObject.spawn_random_trait_allowed)
        this._pot_allowed_to_be_given_randomly.Add(pObject);
      if (pObject.in_mutation_pot_add)
        this._pot_mutation_traits_add.AddTimes<SubspeciesTrait>(pObject.rarity.GetRate(), pObject);
      if (pObject.in_mutation_pot_remove)
        this._pot_mutation_traits_remove.AddTimes<SubspeciesTrait>(pObject.rarity.GetRate(), pObject);
      if (pObject.phenotype_egg && pObject.after_hatch_from_egg_action != null)
        pObject.has_after_hatch_from_egg_action = true;
    }
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    foreach (SubspeciesTrait pAsset in this.list)
      this.checkSpriteExists("sprite_path", pAsset.sprite_path, (Asset) pAsset);
  }

  public SubspeciesTrait getRandomMutationTraitToAdd()
  {
    return this._pot_mutation_traits_add.GetRandom<SubspeciesTrait>();
  }

  public SubspeciesTrait getRandomMutationTraitToRemove()
  {
    return this._pot_mutation_traits_remove.GetRandom<SubspeciesTrait>();
  }

  private void addPhenotypes()
  {
    string str1 = "phenotype_skin";
    for (int index = 0; index < AssetManager.phenotype_library.list.Count; ++index)
    {
      PhenotypeAsset phenotypeAsset = AssetManager.phenotype_library.list[index];
      string str2 = $"{str1}_{phenotypeAsset.id}";
      phenotypeAsset.subspecies_trait_id = str2;
    }
    foreach (PhenotypeAsset phenotypeAsset in AssetManager.phenotype_library.list)
    {
      SubspeciesTrait pAsset = new SubspeciesTrait();
      pAsset.id = $"{str1}_{phenotypeAsset.id}";
      pAsset.group_id = "phenotypes";
      pAsset.id_phenotype = phenotypeAsset.id;
      pAsset.phenotype_skin = true;
      pAsset.priority = phenotypeAsset.priority;
      pAsset.special_icon_logic = true;
      pAsset.special_locale_id = "subspecies_trait_phenotype";
      pAsset.special_locale_description = "subspecies_trait_phenotype_info";
      pAsset.has_description_2 = false;
      pAsset.path_icon = "ui/Icons/iconPhenotype";
      pAsset.spawn_random_trait_allowed = false;
      this.add(pAsset);
    }
  }

  private void addMutationOpposites()
  {
    using (ListPool<string> pListIDS = new ListPool<string>())
    {
      foreach (SubspeciesTrait subspeciesTrait in this.list)
      {
        if (subspeciesTrait.is_mutation_skin)
          pListIDS.Add(subspeciesTrait.id);
      }
      foreach (SubspeciesTrait subspeciesTrait in this.list)
      {
        if (subspeciesTrait.is_mutation_skin)
        {
          subspeciesTrait.addOpposites((IEnumerable<string>) pListIDS);
          subspeciesTrait.removeOpposite(subspeciesTrait.id);
        }
      }
    }
  }

  private void addEggOpposites()
  {
    using (ListPool<string> pListIDS = new ListPool<string>())
    {
      foreach (SubspeciesTrait subspeciesTrait in this.list)
      {
        if (subspeciesTrait.phenotype_egg)
          pListIDS.Add(subspeciesTrait.id);
      }
      foreach (SubspeciesTrait subspeciesTrait in this.list)
      {
        if (subspeciesTrait.phenotype_egg)
        {
          subspeciesTrait.addOpposites((IEnumerable<string>) pListIDS);
          subspeciesTrait.removeOpposite(subspeciesTrait.id);
        }
      }
    }
  }

  private void loadSpritesPaths(SubspeciesTrait pAsset)
  {
    if (!pAsset.is_mutation_skin)
      return;
    string pBasePath = pAsset.sprite_path + "/";
    pAsset.texture_asset = new ActorTextureSubAsset(pBasePath, true);
    pAsset.texture_asset.prevent_unconscious_rotation = pAsset.prevent_unconscious_rotation;
    pAsset.texture_asset.render_heads_for_children = pAsset.render_heads_for_children;
    pAsset.texture_asset.shadow = pAsset.shadow;
    pAsset.texture_asset.shadow_texture = pAsset.shadow_texture;
    pAsset.texture_asset.shadow_texture_egg = pAsset.shadow_texture_egg;
    pAsset.texture_asset.shadow_texture_baby = pAsset.shadow_texture_baby;
  }

  public void preloadMainUnitSprites()
  {
    foreach (SubspeciesTrait pAnimationAsset in this.list)
    {
      if (pAnimationAsset.is_mutation_skin)
        pAnimationAsset.texture_asset.preloadSprites(true, true, (IAnimationFrames) pAnimationAsset);
    }
  }
}
