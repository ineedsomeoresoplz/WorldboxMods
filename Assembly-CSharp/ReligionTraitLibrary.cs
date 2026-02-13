// Decompiled with JetBrains decompiler
// Type: ReligionTraitLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ReligionTraitLibrary : BaseTraitLibrary<ReligionTrait>
{
  private const string TEMPLATE_MAGIC_SPELL = "$magic_spell$";
  private const string TEMPLATE_TRANSFORMATION = "$transformation$";

  protected override List<string> getDefaultTraitsForMeta(ActorAsset pAsset)
  {
    return pAsset.default_religion_traits;
  }

  public override void init()
  {
    base.init();
    this.addMagicSpells();
    this.addTransformations();
    this.addHarmony();
    this.addRites();
    this.addSpecial();
    ReligionTrait pAsset = new ReligionTrait();
    pAsset.id = "grin_mark";
    pAsset.group_id = "fate";
    pAsset.spawn_random_trait_allowed = false;
    pAsset.priority = -100;
    this.add(pAsset);
    this.t.setTraitInfoToGrinMark();
    this.t.setUnlockedWithAchievement("achievementCreaturesExplorer");
  }

  private void addTransformations()
  {
    ReligionTrait pAsset = new ReligionTrait();
    pAsset.id = "$transformation$";
    pAsset.group_id = "transformation";
    pAsset.spawn_random_trait_allowed = false;
    this.add(pAsset);
    this.clone("sands_of_ruin", "$transformation$");
    this.t.transformation_biome_id = "biome_desert";
    this.clone("shadowroot", "$transformation$");
    this.t.transformation_biome_id = "biome_corrupted";
    this.clone("echo_of_the_void", "$transformation$");
    this.t.transformation_biome_id = "biome_singularity";
    this.clone("infernal_rot", "$transformation$");
    this.t.transformation_biome_id = "biome_infernal";
    this.clone("cosmic_radiation", "$transformation$");
    this.t.transformation_biome_id = "biome_wasteland";
  }

  private void addSpecial()
  {
    ReligionTrait pAsset1 = new ReligionTrait();
    pAsset1.id = "divine_insight";
    pAsset1.group_id = "special";
    pAsset1.can_be_given = false;
    pAsset1.can_be_removed = false;
    pAsset1.can_be_in_book = false;
    pAsset1.spawn_random_trait_allowed = false;
    this.add(pAsset1);
    ReligionTrait pAsset2 = new ReligionTrait();
    pAsset2.id = "bloodline_bond";
    pAsset2.group_id = "the_void";
    this.add(pAsset2);
  }

  private void addRites()
  {
    ReligionTrait pAsset1 = new ReligionTrait();
    pAsset1.id = "rite_of_change";
    pAsset1.group_id = "the_void";
    pAsset1.plot_id = "clan_ascension";
    pAsset1.priority = -1;
    this.add(pAsset1);
    ReligionTrait t1 = this.t;
    t1.action_death = t1.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset2 = new ReligionTrait();
    pAsset2.id = "rite_of_shattered_earth";
    pAsset2.group_id = "destruction";
    pAsset2.plot_id = "summon_earthquake";
    pAsset2.priority = -1;
    this.add(pAsset2);
    this.t.setUnlockedWithAchievement("achievementWatchYourMouth");
    ReligionTrait t2 = this.t;
    t2.action_death = t2.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset3 = new ReligionTrait();
    pAsset3.id = "rite_of_falling_stars";
    pAsset3.group_id = "destruction";
    pAsset3.plot_id = "summon_meteor_rain";
    pAsset3.priority = -1;
    this.add(pAsset3);
    this.t.setUnlockedWithAchievement("achievementMayday");
    ReligionTrait t3 = this.t;
    t3.action_death = t3.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset4 = new ReligionTrait();
    pAsset4.id = "rite_of_roaring_skies";
    pAsset4.group_id = "destruction";
    pAsset4.plot_id = "summon_thunderstorm";
    pAsset4.priority = -1;
    this.add(pAsset4);
    this.t.setUnlockedWithAchievement("achievementMayIInterrupt");
    ReligionTrait t4 = this.t;
    t4.action_death = t4.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset5 = new ReligionTrait();
    pAsset5.id = "rite_of_tempest_call";
    pAsset5.group_id = "destruction";
    pAsset5.plot_id = "summon_stormfront";
    pAsset5.priority = -1;
    this.add(pAsset5);
    ReligionTrait t5 = this.t;
    t5.action_death = t5.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset6 = new ReligionTrait();
    pAsset6.id = "rite_of_infernal_wrath";
    pAsset6.group_id = "destruction";
    pAsset6.plot_id = "summon_hellstorm";
    pAsset6.priority = -1;
    this.add(pAsset6);
    ReligionTrait t6 = this.t;
    t6.action_death = t6.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset7 = new ReligionTrait();
    pAsset7.id = "rite_of_the_abyss";
    pAsset7.group_id = "destruction";
    pAsset7.plot_id = "summon_demons";
    pAsset7.priority = -1;
    this.add(pAsset7);
    ReligionTrait t7 = this.t;
    t7.action_death = t7.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset8 = new ReligionTrait();
    pAsset8.id = "rite_of_infinite_edges";
    pAsset8.group_id = "protection";
    pAsset8.plot_id = "summon_angles";
    pAsset8.priority = -1;
    this.add(pAsset8);
    ReligionTrait t8 = this.t;
    t8.action_death = t8.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset9 = new ReligionTrait();
    pAsset9.id = "rite_of_restless_dead";
    pAsset9.group_id = "necromancy";
    pAsset9.plot_id = "summon_skeletons";
    pAsset9.priority = -1;
    this.add(pAsset9);
    ReligionTrait t9 = this.t;
    t9.action_death = t9.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset10 = new ReligionTrait();
    pAsset10.id = "rite_of_fractured_minds";
    pAsset10.group_id = "the_void";
    pAsset10.plot_id = "big_cast_madness";
    pAsset10.priority = -1;
    this.add(pAsset10);
    this.t.setUnlockedWithAchievement("achievementGreg");
    ReligionTrait t10 = this.t;
    t10.action_death = t10.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset11 = new ReligionTrait();
    pAsset11.id = "rite_of_dissent";
    pAsset11.group_id = "the_void";
    pAsset11.plot_id = "cause_rebellion";
    pAsset11.priority = -1;
    this.add(pAsset11);
    this.t.setUnlockedWithAchievement("achievementNotJustACult");
    ReligionTrait t11 = this.t;
    t11.action_death = t11.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset12 = new ReligionTrait();
    pAsset12.id = "rite_of_unbroken_shield";
    pAsset12.group_id = "protection";
    pAsset12.plot_id = "big_cast_bubble_shield";
    pAsset12.priority = -1;
    this.add(pAsset12);
    ReligionTrait t12 = this.t;
    t12.action_death = t12.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset13 = new ReligionTrait();
    pAsset13.id = "rite_of_eternal_brew";
    pAsset13.group_id = "necromancy";
    pAsset13.plot_id = "big_cast_coffee";
    pAsset13.priority = -1;
    this.add(pAsset13);
    this.t.setUnlockedWithAchievement("achievementGodFingerLightning");
    ReligionTrait t13 = this.t;
    t13.action_death = t13.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset14 = new ReligionTrait();
    pAsset14.id = "rite_of_entanglement";
    pAsset14.group_id = "creation";
    pAsset14.plot_id = "big_cast_slowness";
    pAsset14.priority = -1;
    this.add(pAsset14);
    ReligionTrait t14 = this.t;
    t14.action_death = t14.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    ReligionTrait pAsset15 = new ReligionTrait();
    pAsset15.id = "rite_of_living_harvest";
    pAsset15.group_id = "creation";
    pAsset15.plot_id = "summon_living_plants";
    pAsset15.priority = -1;
    this.add(pAsset15);
    ReligionTrait t15 = this.t;
    t15.action_death = t15.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
  }

  private void addHarmony()
  {
    ReligionTrait pAsset1 = new ReligionTrait();
    pAsset1.id = "minds_awakening";
    pAsset1.group_id = "harmony";
    this.add(pAsset1);
    this.t.base_stats["intelligence"] = 10f;
    ReligionTrait pAsset2 = new ReligionTrait();
    pAsset2.id = "zeal_of_conquest";
    pAsset2.group_id = "harmony";
    this.add(pAsset2);
    this.t.base_stats["warfare"] = 10f;
    ReligionTrait pAsset3 = new ReligionTrait();
    pAsset3.id = "path_of_unity";
    pAsset3.group_id = "harmony";
    this.add(pAsset3);
    this.t.base_stats["diplomacy"] = 10f;
    ReligionTrait pAsset4 = new ReligionTrait();
    pAsset4.id = "hand_of_order";
    pAsset4.group_id = "harmony";
    this.add(pAsset4);
    this.t.base_stats["stewardship"] = 10f;
  }

  private void addMagicSpells()
  {
    ReligionTrait pAsset1 = new ReligionTrait();
    pAsset1.id = "$magic_spell$";
    pAsset1.group_id = "creation";
    this.add(pAsset1);
    ReligionTrait t = this.t;
    t.action_death = t.action_death + new WorldAction(ActionLibrary.mageSlayerCheck);
    this.clone("teleport", "$magic_spell$");
    this.t.setUnlockedWithAchievement("achievementTraitExplorerReligion");
    this.t.group_id = "the_void";
    this.t.addSpell("teleport");
    this.clone("cast_silence", "$magic_spell$");
    this.t.group_id = "the_void";
    this.t.addSpell("cast_silence");
    this.clone("summon_lightning", "$magic_spell$");
    this.t.group_id = "destruction";
    this.t.addSpell("summon_lightning");
    this.clone("summon_tornado", "$magic_spell$");
    this.t.group_id = "destruction";
    this.t.addSpell("summon_tornado");
    this.clone("cast_curse", "$magic_spell$");
    this.t.addSpell("cast_curse");
    this.clone("cast_fire", "$magic_spell$");
    this.t.group_id = "destruction";
    this.t.addSpell("cast_fire");
    this.clone("cast_blood_rain", "$magic_spell$");
    this.t.group_id = "restoration";
    this.t.addSpell("cast_blood_rain");
    this.clone("cast_grass_seeds", "$magic_spell$");
    this.t.group_id = "creation";
    this.t.addSpell("cast_grass_seeds");
    this.clone("spawn_vegetation", "$magic_spell$");
    this.t.group_id = "creation";
    this.t.addSpell("spawn_vegetation");
    this.clone("spawn_skeleton", "$magic_spell$");
    this.t.setUnlockedWithAchievement("achievementTheCorruptedTrees");
    this.t.group_id = "necromancy";
    this.t.addSpell("spawn_skeleton");
    this.clone("cast_shield", "$magic_spell$");
    this.t.group_id = "protection";
    this.t.addSpell("cast_shield");
    this.clone("cast_cure", "$magic_spell$");
    this.t.group_id = "restoration";
    this.t.addSpell("cast_cure");
    ReligionTrait pAsset2 = new ReligionTrait();
    pAsset2.id = "blessed_by_ashes";
    pAsset2.group_id = "protection";
    this.add(pAsset2);
    this.t.setUnlockedWithAchievement("achievementSacrifice");
    this.t.base_stats_meta.addTag("building_immunity_fire");
  }

  protected override string icon_path => "ui/Icons/religion_traits/";
}
