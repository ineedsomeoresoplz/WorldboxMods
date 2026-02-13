// Decompiled with JetBrains decompiler
// Type: StatusLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
public class StatusLibrary : AssetLibrary<StatusAsset>
{
  public const float DURATION_STRANGE_URGE = 100f;
  private string[] _pot_dreams = new string[23]
  {
    "had_bad_dream",
    "had_good_dream",
    "had_nightmare",
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null,
    null
  };

  public override void init()
  {
    base.init();
    StatusAsset pAsset1 = new StatusAsset();
    pAsset1.id = "handsome_migrant";
    pAsset1.locale_id = "status_title_handsome_migrant";
    pAsset1.locale_description = "status_description_handsome_migrant";
    pAsset1.duration = 360f;
    pAsset1.path_icon = "ui/Icons/iconStatisticsGoodLookingMigrants";
    this.add(pAsset1);
    StatusAsset pAsset2 = new StatusAsset();
    pAsset2.id = "recovery_plot";
    pAsset2.locale_id = "status_title_recovery_plot";
    pAsset2.locale_description = "status_description_recovery_plot";
    pAsset2.duration = 60f;
    pAsset2.path_icon = "ui/Icons/iconRecoveryPlot";
    this.add(pAsset2);
    StatusAsset pAsset3 = new StatusAsset();
    pAsset3.id = "voices_in_my_head";
    pAsset3.locale_id = "status_title_voices_in_my_head";
    pAsset3.locale_description = "status_description_voices_in_my_head";
    pAsset3.duration = 180f;
    pAsset3.path_icon = "ui/Icons/iconVoicesInMyHead";
    this.add(pAsset3);
    this.t.base_stats["diplomacy"] = -5f;
    this.t.base_stats["personality_rationality"] = -0.3f;
    this.t.base_stats["opinion"] = -5f;
    StatusAsset pAsset4 = new StatusAsset();
    pAsset4.id = "recovery_spell";
    pAsset4.locale_id = "status_title_recovery_spell";
    pAsset4.locale_description = "status_description_recovery_spell";
    pAsset4.duration = 5f;
    pAsset4.path_icon = "ui/Icons/iconRecoverySpell";
    this.add(pAsset4);
    StatusAsset pAsset5 = new StatusAsset();
    pAsset5.id = "recovery_social";
    pAsset5.locale_id = "status_title_recovery_social";
    pAsset5.locale_description = "status_description_recovery_social";
    pAsset5.duration = 60f;
    pAsset5.path_icon = "ui/Icons/iconRecoverySocial";
    this.add(pAsset5);
    StatusAsset pAsset6 = new StatusAsset();
    pAsset6.id = "recovery_combat_action";
    pAsset6.locale_id = "status_title_recovery_combat_action";
    pAsset6.locale_description = "status_description_recovery_combat_action";
    pAsset6.duration = 1f;
    pAsset6.path_icon = "ui/Icons/iconRecoveryCombatAction";
    this.add(pAsset6);
    StatusAsset pAsset7 = new StatusAsset();
    pAsset7.id = "starving";
    pAsset7.locale_id = "status_title_starvation";
    pAsset7.locale_description = "status_description_starvation";
    pAsset7.duration = 60f;
    pAsset7.path_icon = "ui/Icons/iconHungry";
    pAsset7.action_on_receive = (WorldAction) ((pActor, _) => pActor.a.changeHappiness("starving"));
    pAsset7.action_interval = 5f;
    pAsset7.action = (WorldAction) ((pObject, pTile) =>
    {
      Actor a = pObject.a;
      if (!a.isAlive() || !a.hasCity() || a.isFighting())
        return false;
      City city = a.city;
      if (a.hasTask() && a.ai.task.diet)
        return false;
      if (!city.hasSuitableFood(a.subspecies))
      {
        a.cancelAllBeh();
        return false;
      }
      a.setTask("try_to_eat_city_food");
      return true;
    });
    this.add(pAsset7);
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset8 = new StatusAsset();
    pAsset8.id = "drowning";
    pAsset8.locale_id = "status_title_drowning";
    pAsset8.locale_description = "status_description_drowning";
    pAsset8.duration = 1f;
    pAsset8.path_icon = "ui/Icons/iconDrowning";
    pAsset8.action_interval = 0.5f;
    pAsset8.action = (WorldAction) ((pObject, pTile) =>
    {
      Actor a = pObject.a;
      if (!a.isAlive())
        return false;
      a.getHit(1f, pAttackType: AttackType.Drowning);
      EffectsLibrary.spawnAt("fx_drowning", a.current_position, 0.1f);
      return true;
    });
    pAsset8.action_death = (WorldAction) ((pObject, pTile) =>
    {
      EffectsLibrary.spawnAt("fx_drowning", pObject.a.current_position, 0.1f);
      return true;
    });
    this.add(pAsset8);
    this.t.tier = StatusTier.Advanced;
    this.t.base_stats.addTag("ignore_fights");
    StatusAsset pAsset9 = new StatusAsset();
    pAsset9.id = "sleeping";
    pAsset9.render_priority = 8;
    pAsset9.locale_id = "status_title_sleeping";
    pAsset9.locale_description = "status_description_sleeping";
    pAsset9.duration = 60f;
    pAsset9.animated = true;
    pAsset9.is_animated_in_pause = true;
    pAsset9.can_be_flipped = false;
    pAsset9.use_parent_rotation = false;
    pAsset9.removed_on_damage = true;
    pAsset9.texture = "fx_status_sleeping_t";
    pAsset9.path_icon = "ui/Icons/iconSleep";
    pAsset9.action_finish = (WorldAction) ((pActor, _) =>
    {
      Actor a = pActor.a;
      string random = this._pot_dreams.GetRandom<string>();
      if (!string.IsNullOrEmpty(random))
        pActor.a.addStatusEffect(random);
      a.restoreStaminaPercent(0.2f);
      return pActor.a.changeHappiness("just_slept");
    });
    this.add(pAsset9);
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("tantrum", "surprised");
    this.t.tier = StatusTier.Advanced;
    this.t.base_stats.addTag("unconscious");
    StatusAsset pAsset10 = new StatusAsset();
    pAsset10.id = "laughing";
    pAsset10.render_priority = 4;
    pAsset10.locale_id = "status_title_laughing";
    pAsset10.locale_description = "status_description_laughing";
    pAsset10.duration = 60f;
    pAsset10.animated = true;
    pAsset10.is_animated_in_pause = true;
    pAsset10.can_be_flipped = false;
    pAsset10.use_parent_rotation = false;
    pAsset10.removed_on_damage = true;
    pAsset10.texture = "fx_status_laughing_t";
    pAsset10.path_icon = "ui/Icons/iconLaughing";
    pAsset10.action_on_receive = (WorldAction) ((pActor, _) =>
    {
      pActor.a.playIdleSound();
      return true;
    });
    this.add(pAsset10);
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset11 = new StatusAsset();
    pAsset11.id = "singing";
    pAsset11.render_priority = 4;
    pAsset11.locale_id = "status_title_singing";
    pAsset11.locale_description = "status_description_singing";
    pAsset11.duration = 60f;
    pAsset11.animated = true;
    pAsset11.is_animated_in_pause = true;
    pAsset11.can_be_flipped = false;
    pAsset11.use_parent_rotation = false;
    pAsset11.removed_on_damage = true;
    pAsset11.texture = "fx_status_singing_t";
    pAsset11.path_icon = "ui/Icons/iconSinging";
    pAsset11.action_on_receive = (WorldAction) ((pActor, _) =>
    {
      pActor.a.playIdleSound();
      return true;
    });
    this.add(pAsset11);
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset12 = new StatusAsset();
    pAsset12.id = "swearing";
    pAsset12.render_priority = 4;
    pAsset12.locale_id = "status_title_swearing";
    pAsset12.locale_description = "status_description_swearing";
    pAsset12.duration = 60f;
    pAsset12.animated = true;
    pAsset12.is_animated_in_pause = true;
    pAsset12.can_be_flipped = false;
    pAsset12.use_parent_rotation = false;
    pAsset12.removed_on_damage = true;
    pAsset12.texture = "fx_status_swearing_t";
    pAsset12.path_icon = "ui/Icons/iconSwearing";
    pAsset12.action_on_receive = (WorldAction) ((pActor, _) =>
    {
      pActor.a.playIdleSound();
      return true;
    });
    this.add(pAsset12);
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("peaceful");
    this.t.tier = StatusTier.Advanced;
    this.t.base_stats.addTag("moody");
    StatusAsset pAsset13 = new StatusAsset();
    pAsset13.id = "crying";
    pAsset13.render_priority = 4;
    pAsset13.locale_id = "status_title_crying";
    pAsset13.locale_description = "status_description_crying";
    pAsset13.duration = 60f;
    pAsset13.animated = true;
    pAsset13.is_animated_in_pause = true;
    pAsset13.can_be_flipped = false;
    pAsset13.use_parent_rotation = false;
    pAsset13.removed_on_damage = true;
    pAsset13.texture = "fx_status_crying_t";
    pAsset13.path_icon = "ui/Icons/iconCrying";
    pAsset13.action_on_receive = (WorldAction) ((pActor, _) =>
    {
      pActor.a.playIdleSound();
      return true;
    });
    this.add(pAsset13);
    this.t.tier = StatusTier.Advanced;
    this.t.base_stats.addTag("moody");
    StatusAsset pAsset14 = new StatusAsset();
    pAsset14.id = "possessed";
    pAsset14.locale_id = "status_title_possessed";
    pAsset14.locale_description = "status_description_possessed";
    pAsset14.duration = 10f;
    pAsset14.animated = true;
    pAsset14.path_icon = "ui/Icons/iconPossessed";
    pAsset14.action_on_receive = (WorldAction) ((pActor, _) => pActor.a.changeHappiness("just_possessed"));
    pAsset14.action_interval = 0.0f;
    pAsset14.action = new WorldAction(this.possessedAction);
    this.add(pAsset14);
    StatusAsset pAsset15 = new StatusAsset();
    pAsset15.id = "possessed_follower";
    pAsset15.locale_id = "status_title_possessed_follower";
    pAsset15.locale_description = "status_description_possessed_follower";
    pAsset15.duration = 200f;
    pAsset15.affects_mind = true;
    pAsset15.animated = true;
    pAsset15.is_animated_in_pause = true;
    pAsset15.texture = "fx_status_possessed_follower_t";
    pAsset15.path_icon = "ui/Icons/iconPossessed";
    pAsset15.decision_id = "possessed_following";
    this.add(pAsset15);
    StatusAsset pAsset16 = new StatusAsset();
    pAsset16.id = "strange_urge";
    pAsset16.locale_id = "status_title_strange_urge";
    pAsset16.locale_description = "status_description_strange_urge";
    pAsset16.duration = 100f;
    pAsset16.animated = true;
    pAsset16.path_icon = "ui/Icons/iconStrangeUrge";
    pAsset16.action_on_receive = (WorldAction) ((pActor, _) => !Randy.randomChance(0.7f) && pActor.a.changeHappiness("strange_urge"));
    this.add(pAsset16);
    StatusAsset pAsset17 = new StatusAsset();
    pAsset17.id = "tantrum";
    pAsset17.locale_id = "status_title_tantrum";
    pAsset17.locale_description = "status_description_tantrum";
    pAsset17.duration = 120f;
    pAsset17.affects_mind = true;
    pAsset17.path_icon = "ui/Icons/iconTantrum";
    pAsset17.action_finish = (WorldAction) ((pActor, _) => pActor.a.changeHappiness("just_had_tantrum"));
    pAsset17.decision_id = "do_tantrum";
    this.add(pAsset17);
    StatusAsset pAsset18 = new StatusAsset();
    pAsset18.id = "egg";
    pAsset18.locale_id = "status_egg";
    pAsset18.locale_description = "status_description_egg";
    pAsset18.duration = 5f;
    pAsset18.animated = false;
    pAsset18.path_icon = "ui/Icons/iconEgg";
    pAsset18.action_finish = (WorldAction) ((pActor, _) =>
    {
      pActor.a.makeStunned();
      if (pActor.a.isRendered())
        EffectsLibrary.spawn("fx_spawn", pActor.current_tile, pX: pActor.current_position.x, pY: pActor.current_position.y);
      return true;
    });
    this.add(pAsset18);
    this.t.base_stats.addTag("immovable");
    this.t.base_stats.addTag("frozen_ai");
    this.t.base_stats["armor"] = 10f;
    StatusAsset pAsset19 = new StatusAsset();
    pAsset19.id = "cursed";
    pAsset19.locale_id = "status_title_cursed";
    pAsset19.locale_description = "status_description_cursed";
    pAsset19.duration = 300f;
    pAsset19.animated = false;
    pAsset19.path_icon = "ui/Icons/iconCursed";
    pAsset19.can_be_cured = true;
    pAsset19.action_on_receive = (WorldAction) ((pActor, _) => pActor.a.changeHappiness("just_cursed"));
    this.add(pAsset19);
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("evil");
    this.t.action_death += new WorldAction(ActionLibrary.turnIntoSkeleton);
    this.t.base_stats["loyalty_traits"] = -100f;
    this.t.base_stats["multiplier_damage"] = -0.5f;
    this.t.base_stats["multiplier_health"] = -0.5f;
    this.t.base_stats["multiplier_speed"] = -0.2f;
    this.t.base_stats["multiplier_diplomacy"] = -0.9f;
    this.t.base_stats["lifespan"] = -10f;
    this.t.base_stats["multiplier_offspring"] = -2f;
    StatusAsset pAsset20 = new StatusAsset();
    pAsset20.id = "spell_silence";
    pAsset20.locale_id = "status_spell_silence";
    pAsset20.locale_description = "status_description_spell_silence";
    pAsset20.duration = 60f;
    pAsset20.animated = false;
    pAsset20.path_icon = "ui/Icons/iconSpellSilence";
    this.add(pAsset20);
    StatusAsset pAsset21 = new StatusAsset();
    pAsset21.id = "spell_boost";
    pAsset21.locale_id = "status_spell_boost";
    pAsset21.locale_description = "status_description_spell_boost";
    pAsset21.duration = 180f;
    pAsset21.animated = false;
    pAsset21.path_icon = "ui/Icons/iconSpellBoost";
    this.add(pAsset21);
    this.t.base_stats["mana"] = 100f;
    this.t.base_stats["skill_spell"] = 0.2f;
    StatusAsset pAsset22 = new StatusAsset();
    pAsset22.id = "inspired";
    pAsset22.locale_id = "status_title_inspired";
    pAsset22.locale_description = "status_description_inspired";
    pAsset22.duration = 60f;
    pAsset22.animated = false;
    pAsset22.path_icon = "ui/Icons/iconInspired";
    pAsset22.action_finish = (WorldAction) ((pActor, _) => pActor.a.changeHappiness("just_inspired"));
    this.add(pAsset22);
    this.t.base_stats["multiplier_speed"] = 0.3f;
    this.t.base_stats["multiplier_crit"] = 0.3f;
    this.t.base_stats["multiplier_attack_speed"] = 0.3f;
    StatusAsset pAsset23 = new StatusAsset();
    pAsset23.id = "confused";
    pAsset23.locale_id = "status_title_confused";
    pAsset23.locale_description = "status_description_confused";
    pAsset23.duration = 10f;
    pAsset23.animated = false;
    pAsset23.affects_mind = true;
    pAsset23.path_icon = "ui/Icons/iconConfused";
    pAsset23.decision_id = "status_confused";
    this.add(pAsset23);
    this.t.opposite_status = AssetLibrary<StatusAsset>.a<string>("egg");
    StatusAsset pAsset24 = new StatusAsset();
    pAsset24.id = "soul_harvested";
    pAsset24.locale_id = "status_title_soul_harvested";
    pAsset24.locale_description = "status_description_soul_harvested";
    pAsset24.duration = 666f;
    pAsset24.animated = false;
    pAsset24.path_icon = "ui/Icons/iconSoulHarvested";
    pAsset24.decision_id = "status_soul_harvested";
    this.add(pAsset24);
    StatusAsset pAsset25 = new StatusAsset();
    pAsset25.id = "magnetized";
    pAsset25.locale_id = "status_title_magnetized";
    pAsset25.locale_description = "status_description_magnetized";
    pAsset25.duration = 4f;
    pAsset25.animated = false;
    pAsset25.path_icon = "ui/Icons/iconMagnetized";
    pAsset25.action_on_receive = (WorldAction) ((pSelf, _) => pSelf.a.changeHappiness("just_magnetised"));
    this.add(pAsset25);
    StatusAsset pAsset26 = new StatusAsset();
    pAsset26.id = "rage";
    pAsset26.locale_id = "status_title_rage";
    pAsset26.locale_description = "status_description_rage";
    pAsset26.duration = 300f;
    pAsset26.animated = false;
    pAsset26.affects_mind = true;
    pAsset26.path_icon = "ui/Icons/iconRage";
    this.add(pAsset26);
    this.t.base_stats["multiplier_damage"] = 1f;
    StatusAsset pAsset27 = new StatusAsset();
    pAsset27.id = "surprised";
    pAsset27.render_priority = 9;
    pAsset27.locale_id = "status_title_surprised";
    pAsset27.locale_description = "status_description_surprised";
    pAsset27.duration = 2f;
    pAsset27.texture = "fx_status_surprised_t";
    pAsset27.animated = true;
    pAsset27.is_animated_in_pause = true;
    pAsset27.loop = false;
    pAsset27.use_parent_rotation = false;
    pAsset27.path_icon = "ui/Icons/iconSurprised";
    pAsset27.offset_y = 1f;
    pAsset27.offset_y_ui = 0.65f;
    pAsset27.action_on_receive = (WorldAction) ((pSelf, _) => pSelf.a.changeHappiness("just_surprised"));
    this.add(pAsset27);
    StatusAsset pAsset28 = new StatusAsset();
    pAsset28.id = "on_guard";
    pAsset28.render_priority = 8;
    pAsset28.locale_id = "status_title_on_guard";
    pAsset28.locale_description = "status_description_on_guard";
    pAsset28.duration = 60f;
    pAsset28.use_parent_rotation = false;
    pAsset28.path_icon = "ui/Icons/iconOnGuard";
    this.add(pAsset28);
    StatusAsset pAsset29 = new StatusAsset();
    pAsset29.id = "angry";
    pAsset29.locale_id = "status_title_angry";
    pAsset29.locale_description = "status_description_angry";
    pAsset29.duration = 60f;
    pAsset29.use_parent_rotation = false;
    pAsset29.path_icon = "ui/Icons/iconAngry";
    pAsset29.animated = true;
    pAsset29.is_animated_in_pause = true;
    pAsset29.texture = "fx_status_angry_t";
    pAsset29.action_finish = (WorldAction) ((pActor, _) =>
    {
      pActor.a.finishAngryStatus();
      return true;
    });
    this.add(pAsset29);
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("peaceful");
    StatusAsset pAsset30 = new StatusAsset();
    pAsset30.id = "just_ate";
    pAsset30.locale_id = "status_title_just_ate";
    pAsset30.locale_description = "status_description_just_ate";
    pAsset30.duration = 200f;
    pAsset30.path_icon = "ui/Icons/iconHunger";
    pAsset30.decision_id = "try_to_poop";
    this.add(pAsset30);
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("starving");
    StatusAsset pAsset31 = new StatusAsset();
    pAsset31.id = "festive_spirit";
    pAsset31.locale_id = "status_title_festive_spirit";
    pAsset31.locale_description = "status_description_festive_spirit";
    pAsset31.duration = 100f;
    pAsset31.path_icon = "ui/Icons/iconFireworks";
    pAsset31.decision_id = "try_to_launch_fireworks";
    this.add(pAsset31);
    StatusAsset pAsset32 = new StatusAsset();
    pAsset32.id = "being_suspicious";
    pAsset32.locale_id = "status_title_being_suspicious";
    pAsset32.locale_description = "status_description_being_suspicious";
    pAsset32.duration = 20f;
    pAsset32.path_icon = "ui/Icons/iconSuspicious";
    pAsset32.decision_id = "run_away_being_sus";
    this.add(pAsset32);
    StatusAsset pAsset33 = new StatusAsset();
    pAsset33.id = "had_good_dream";
    pAsset33.locale_id = "status_title_had_good_dream";
    pAsset33.locale_description = "status_description_had_good_dream";
    pAsset33.duration = 90f;
    pAsset33.path_icon = "ui/Icons/iconDreamGood";
    pAsset33.action_on_receive = (WorldAction) ((pSelf, _) => pSelf.a.changeHappiness("had_good_dream"));
    this.add(pAsset33);
    StatusAsset pAsset34 = new StatusAsset();
    pAsset34.id = "had_bad_dream";
    pAsset34.locale_id = "status_title_had_bad_dream";
    pAsset34.locale_description = "status_description_had_bad_dream";
    pAsset34.duration = 90f;
    pAsset34.path_icon = "ui/Icons/iconDreamBad";
    pAsset34.action_on_receive = (WorldAction) ((pSelf, _) => pSelf.a.changeHappiness("had_bad_dream"));
    this.add(pAsset34);
    StatusAsset pAsset35 = new StatusAsset();
    pAsset35.id = "had_nightmare";
    pAsset35.locale_id = "status_title_had_nightmare";
    pAsset35.locale_description = "status_description_had_nightmare";
    pAsset35.duration = 90f;
    pAsset35.path_icon = "ui/Icons/iconDreamNightmare";
    pAsset35.action_on_receive = (WorldAction) ((pSelf, _) => pSelf.a.changeHappiness("had_nightmare"));
    this.add(pAsset35);
    StatusAsset pAsset36 = new StatusAsset();
    pAsset36.id = "stunned";
    pAsset36.render_priority = 10;
    pAsset36.locale_id = "status_title_stunned";
    pAsset36.locale_description = "status_description_stunned";
    pAsset36.duration = 4f;
    pAsset36.texture = "fx_status_stunned_t";
    pAsset36.animated = true;
    pAsset36.is_animated_in_pause = true;
    pAsset36.use_parent_rotation = false;
    pAsset36.path_icon = "ui/Icons/iconStunned";
    this.add(pAsset36);
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("sleeping", "tantrum", "surprised", "singing", "laughing", "being_suspicious");
    this.t.tier = StatusTier.Advanced;
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("tough");
    this.t.opposite_status = AssetLibrary<StatusAsset>.a<string>("egg");
    this.t.base_stats.addTag("unconscious");
    StatusAsset pAsset37 = new StatusAsset();
    pAsset37.id = "afterglow";
    pAsset37.locale_id = "status_title_afterglow";
    pAsset37.locale_description = "status_description_afterglow";
    pAsset37.duration = 90f;
    pAsset37.animated = true;
    pAsset37.is_animated_in_pause = true;
    pAsset37.use_parent_rotation = false;
    pAsset37.path_icon = "ui/Icons/iconAfterglow";
    pAsset37.offset_y = 1f;
    this.add(pAsset37);
    StatusAsset pAsset38 = new StatusAsset();
    pAsset38.id = "fell_in_love";
    pAsset38.locale_id = "status_title_fell_in_love";
    pAsset38.locale_description = "status_description_fell_in_love";
    pAsset38.duration = 180f;
    pAsset38.use_parent_rotation = false;
    pAsset38.path_icon = "ui/Icons/iconLovers";
    pAsset38.offset_y = 1f;
    pAsset38.action_interval = 2f;
    pAsset38.action_on_receive = (WorldAction) ((pSelf, _) => pSelf.a.changeHappiness("fallen_in_love"));
    pAsset38.action = (WorldAction) ((pTarget, _) =>
    {
      EffectsLibrary.spawnAt("fx_hearts", pTarget.current_position, pTarget.current_scale.y);
      return true;
    });
    this.add(pAsset38);
    StatusAsset pAsset39 = new StatusAsset();
    pAsset39.id = "pregnant";
    pAsset39.locale_id = "status_title_pregnant";
    pAsset39.locale_description = "status_description_pregnant";
    pAsset39.duration = 45f;
    pAsset39.path_icon = "ui/Icons/iconPregnant";
    pAsset39.animated = true;
    pAsset39.is_animated_in_pause = true;
    pAsset39.use_parent_rotation = false;
    pAsset39.texture = "fx_status_pregnant_t";
    pAsset39.action_finish = new WorldAction(this.actionPregnancyFinish);
    this.add(pAsset39);
    this.t.base_stats["multiplier_speed"] = -0.2f;
    StatusAsset pAsset40 = new StatusAsset();
    pAsset40.id = "pregnant_parthenogenesis";
    pAsset40.locale_id = "status_title_pregnant";
    pAsset40.locale_description = "status_description_pregnant";
    pAsset40.duration = 45f;
    pAsset40.path_icon = "ui/Icons/iconPregnant";
    pAsset40.animated = true;
    pAsset40.is_animated_in_pause = true;
    pAsset40.use_parent_rotation = false;
    pAsset40.texture = "fx_status_pregnant_t";
    pAsset40.action_finish = new WorldAction(this.actionPregnancyParthenogenesisFinish);
    this.add(pAsset40);
    this.t.base_stats["multiplier_speed"] = -0.2f;
    StatusAsset pAsset41 = new StatusAsset();
    pAsset41.id = "powerup";
    pAsset41.locale_id = "status_title_powerup";
    pAsset41.locale_description = "status_description_powerup";
    pAsset41.duration = 300f;
    pAsset41.animated = true;
    pAsset41.path_icon = "ui/Icons/iconPowerup";
    this.add(pAsset41);
    this.t.draw_light_area = true;
    this.t.draw_light_size = 0.05f;
    this.t.base_stats["armor"] = 1f;
    this.t.base_stats["damage"] = 5f;
    this.t.base_stats["multiplier_damage"] = 0.5f;
    this.t.base_stats["multiplier_crit"] = 0.5f;
    this.t.base_stats["scale"] = 0.1f;
    StatusAsset pAsset42 = new StatusAsset();
    pAsset42.id = "enchanted";
    pAsset42.locale_id = "status_title_enchanted";
    pAsset42.locale_description = "status_description_enchanted";
    pAsset42.duration = 120f;
    pAsset42.path_icon = "ui/Icons/iconEnchanted";
    pAsset42.action_on_receive = (WorldAction) ((pSelf, _) => pSelf.a.changeHappiness("just_enchanted"));
    this.add(pAsset42);
    this.t.draw_light_area = true;
    this.t.draw_light_size = 0.05f;
    this.t.base_stats["multiplier_damage"] = 0.77f;
    this.t.base_stats["multiplier_speed"] = 0.1f;
    this.t.base_stats["multiplier_diplomacy"] = 0.1f;
    this.t.base_stats["multiplier_crit"] = 0.1f;
    this.t.base_stats["lifespan"] = 10f;
    StatusAsset pAsset43 = new StatusAsset();
    pAsset43.id = "slowness";
    pAsset43.locale_id = "status_title_slowness";
    pAsset43.locale_description = "status_description_slowness";
    pAsset43.texture = "fx_status_slowness_t";
    pAsset43.duration = 30f;
    pAsset43.animated = true;
    pAsset43.path_icon = "ui/Icons/iconSlowness";
    this.add(pAsset43);
    this.t.base_stats["speed"] = -100f;
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("caffeinated");
    StatusAsset pAsset44 = new StatusAsset();
    pAsset44.id = "motivated";
    pAsset44.locale_id = "status_title_motivated";
    pAsset44.locale_description = "status_description_motivated";
    pAsset44.texture = "fx_status_motivated_t";
    pAsset44.duration = 120f;
    pAsset44.animated = true;
    pAsset44.is_animated_in_pause = true;
    pAsset44.path_icon = "ui/Icons/iconMotivated";
    this.add(pAsset44);
    this.t.base_stats["speed"] = 10f;
    this.t.base_stats["attack_speed"] = 2f;
    StatusAsset pAsset45 = new StatusAsset();
    pAsset45.id = "cough";
    pAsset45.locale_id = "status_title_cough";
    pAsset45.locale_description = "status_description_cough";
    pAsset45.duration = 200f;
    pAsset45.path_icon = "ui/Icons/iconCough";
    pAsset45.can_be_cured = true;
    this.add(pAsset45);
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("immune");
    this.t.base_stats["lifespan"] = -15f;
    this.t.base_stats["multiplier_speed"] = -0.1f;
    this.t.base_stats["multiplier_health"] = -0.1f;
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset46 = new StatusAsset();
    pAsset46.id = "ash_fever";
    pAsset46.locale_id = "status_title_ash_fever";
    pAsset46.locale_description = "status_description_ash_fever";
    pAsset46.duration = 500f;
    pAsset46.path_icon = "ui/Icons/iconAshFever";
    pAsset46.action = new WorldAction(StatusLibrary.ashFeverEffect);
    pAsset46.action_interval = 10f;
    pAsset46.can_be_cured = true;
    this.add(pAsset46);
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("immune");
    this.t.tier = StatusTier.Advanced;
    this.t.base_stats["diplomacy"] = -5f;
    this.t.base_stats["intelligence"] = -5f;
    this.t.base_stats["stewardship"] = -5f;
    this.t.base_stats["warfare"] = 5f;
    this.t.base_stats["multiplier_speed"] = -0.1f;
    this.t.base_stats["multiplier_health"] = -0.6f;
    this.t.base_stats["lifespan"] = -45f;
    StatusAsset pAsset47 = new StatusAsset();
    pAsset47.id = "caffeinated";
    pAsset47.locale_id = "status_title_caffeinated";
    pAsset47.locale_description = "status_description_caffeinated";
    pAsset47.texture = "fx_status_caffeinated_t";
    pAsset47.duration = 60f;
    pAsset47.animated = true;
    pAsset47.is_animated_in_pause = true;
    pAsset47.path_icon = "ui/Icons/iconCoffee";
    this.add(pAsset47);
    this.t.base_stats["intelligence"] = 222f;
    this.t.base_stats["speed"] = 200f;
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("frozen");
    StatusAsset pAsset48 = new StatusAsset();
    pAsset48.id = "frozen";
    pAsset48.locale_id = "status_title_frozen";
    pAsset48.locale_description = "status_description_frozen";
    pAsset48.texture = "fx_status_frozen_t";
    pAsset48.duration = 15f;
    pAsset48.allow_timer_reset = false;
    pAsset48.random_frame = true;
    pAsset48.sound_idle = "event:/SFX/STATUS/StatusFrozen";
    pAsset48.path_icon = "ui/Icons/iconFrozen";
    this.add(pAsset48);
    this.t.base_stats["mass"] = 100f;
    this.t.base_stats["armor"] = -20f;
    this.t.base_stats["speed"] = -10000f;
    this.t.base_stats.addTag("immovable");
    this.t.base_stats.addTag("frozen_ai");
    this.t.base_stats.addTag("stop_idle_animation");
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("burning", "tantrum");
    this.t.opposite_status = AssetLibrary<StatusAsset>.a<string>("shield");
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("freeze_proof");
    this.t.opposite_tags = AssetLibrary<StatusAsset>.a<string>("immunity_cold");
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset49 = new StatusAsset();
    pAsset49.id = "shield";
    pAsset49.locale_id = "status_title_shield";
    pAsset49.locale_description = "status_description_shield";
    pAsset49.texture = "fx_status_shield_t";
    pAsset49.duration = 60f;
    pAsset49.animated = true;
    pAsset49.is_animated_in_pause = true;
    pAsset49.sound_idle = "event:/SFX/STATUS/StatusShield";
    pAsset49.path_icon = "ui/Icons/iconBubbleShield";
    this.add(pAsset49);
    this.t.draw_light_area = true;
    this.t.draw_light_size = 0.1f;
    this.t.base_stats["mass"] = 100f;
    this.t.base_stats["armor"] = 90f;
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("burning");
    this.t.opposite_status = AssetLibrary<StatusAsset>.a<string>("frozen");
    this.t.action_get_hit += new GetHitAction(StatusLibrary.spawnShieldHitEffect);
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset50 = new StatusAsset();
    pAsset50.id = "burning";
    pAsset50.locale_id = "status_title_burning";
    pAsset50.locale_description = "status_description_burning";
    pAsset50.texture = "fx_status_burning_t";
    pAsset50.duration = 30f;
    pAsset50.render_priority = 100;
    pAsset50.allow_timer_reset = false;
    pAsset50.action = new WorldAction(StatusLibrary.burningEffect);
    pAsset50.action_interval = 2f;
    pAsset50.animated = true;
    pAsset50.animation_speed = 0.1f;
    pAsset50.animation_speed_random = 0.08f;
    pAsset50.random_frame = true;
    pAsset50.random_flip = true;
    pAsset50.cancel_actor_job = true;
    pAsset50.material_id = "mat_world_object_lit";
    pAsset50.draw_light_area = true;
    pAsset50.draw_light_size = 0.2f;
    pAsset50.sound_idle = "event:/SFX/STATUS/StatusBurningBuilding";
    pAsset50.path_icon = "ui/Icons/iconFire";
    pAsset50.decision_id = "run_to_water_when_on_fire";
    this.add(pAsset50);
    this.t.opposite_status = AssetLibrary<StatusAsset>.a<string>("shield", "tantrum");
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("fire_proof");
    this.t.opposite_tags = AssetLibrary<StatusAsset>.a<string>("immunity_fire");
    this.t.remove_status = AssetLibrary<StatusAsset>.a<string>("frozen");
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset51 = new StatusAsset();
    pAsset51.id = "poisoned";
    pAsset51.locale_id = "status_title_poisoned";
    pAsset51.locale_description = "status_description_poisoned";
    pAsset51.duration = 90f;
    pAsset51.allow_timer_reset = false;
    pAsset51.action = new WorldAction(StatusLibrary.poisonedEffect);
    pAsset51.action_interval = 1f;
    pAsset51.path_icon = "ui/Icons/iconPoisoned";
    this.add(pAsset51);
    this.t.tier = StatusTier.Advanced;
    this.t.opposite_traits = AssetLibrary<StatusAsset>.a<string>("poison_immune");
    StatusAsset pAsset52 = new StatusAsset();
    pAsset52.id = "invincible";
    pAsset52.locale_id = "status_title_invincible";
    pAsset52.locale_description = "status_description_invincible";
    pAsset52.duration = 5f;
    pAsset52.path_icon = "ui/Icons/iconInvincible";
    this.add(pAsset52);
    StatusAsset pAsset53 = new StatusAsset();
    pAsset53.id = "dodge";
    pAsset53.locale_id = "status_title_dodge";
    pAsset53.locale_description = "status_description_dodge";
    pAsset53.duration = 1f;
    pAsset53.path_icon = "ui/Icons/skills/iconSkillDodge";
    this.add(pAsset53);
    StatusAsset pAsset54 = new StatusAsset();
    pAsset54.id = "dash";
    pAsset54.locale_id = "status_title_dash";
    pAsset54.locale_description = "status_description_dash";
    pAsset54.duration = 2f;
    pAsset54.path_icon = "ui/Icons/skills/iconSkillDash";
    this.add(pAsset54);
    this.t.base_stats["multiplier_speed"] = 1.55f;
    StatusAsset pAsset55 = new StatusAsset();
    pAsset55.id = "taking_roots";
    pAsset55.locale_id = "status_title_taking_roots";
    pAsset55.locale_description = "status_description_taking_roots";
    pAsset55.sound_idle = "event:/SFX/NATURE/TreeFall";
    pAsset55.duration = 120f;
    pAsset55.path_icon = "ui/Icons/iconTakingRoots";
    pAsset55.texture = "fx_status_taking_roots_t";
    pAsset55.animated = true;
    pAsset55.use_parent_rotation = false;
    this.add(pAsset55);
    this.t.base_stats["mass"] = 100f;
    this.t.base_stats["armor"] = 10f;
    this.t.base_stats["speed"] = -10000f;
    this.t.base_stats["multiplier_speed"] = -0.3f;
    this.t.base_stats["multiplier_damage"] = -0.3f;
    this.t.base_stats["multiplier_health"] = -0.3f;
    this.t.base_stats.addTag("immovable");
    this.t.base_stats.addTag("stop_idle_animation");
    this.t.base_stats.addTag("frozen_ai");
    this.t.tier = StatusTier.Advanced;
    this.t.action_finish += new WorldAction(this.actionTakingRootsFinish);
    StatusAsset pAsset56 = new StatusAsset();
    pAsset56.id = "uprooting";
    pAsset56.locale_id = "status_title_uprooting";
    pAsset56.locale_description = "status_description_uprooting";
    pAsset56.sound_idle = "event:/SFX/CIVILIZATIONS/CropsGrow";
    pAsset56.duration = 120f;
    pAsset56.path_icon = "ui/Icons/iconUprooting";
    pAsset56.texture = "fx_status_uprooting_t";
    pAsset56.animated = true;
    pAsset56.use_parent_rotation = false;
    pAsset56.offset_y_ui = 0.1f;
    this.add(pAsset56);
    this.t.base_stats["armor"] = 20f;
    this.t.base_stats["speed"] = -10000f;
    this.t.base_stats["multiplier_speed"] = -0.3f;
    this.t.base_stats["multiplier_damage"] = -0.3f;
    this.t.base_stats["multiplier_health"] = -0.3f;
    this.t.base_stats.addTag("immovable");
    this.t.base_stats.addTag("frozen_ai");
    this.t.base_stats.addTag("stop_idle_animation");
    this.t.tier = StatusTier.Advanced;
    StatusAsset pAsset57 = new StatusAsset();
    pAsset57.id = "budding";
    pAsset57.locale_id = "status_title_budding";
    pAsset57.locale_description = "status_description_budding";
    pAsset57.duration = 60f;
    pAsset57.path_icon = "ui/Icons/iconStatusBudding";
    pAsset57.animated = true;
    pAsset57.animation_speed = 0.0f;
    pAsset57.scale = 0.7f;
    pAsset57.position_z = -0.01f;
    pAsset57.rotation_z = -30f;
    pAsset57.use_parent_rotation = true;
    pAsset57.get_override_sprite = (GetEffectSprite) ((pActor, pIndex) =>
    {
      Sprite frame = pActor.a.animation_container.walking.frames[0];
      return pActor.a.calculateColoredSprite(frame, false);
    });
    pAsset57.get_override_sprite_position = (GetEffectSpritePosition) ((pActor, pIndex) =>
    {
      AnimationFrameData animationFrameData = pActor.a.getAnimationFrameData();
      ref Vector3 local1 = ref pActor.a.current_scale;
      Vector3 vector3;
      if (animationFrameData.show_head)
      {
        ref Vector2 local2 = ref animationFrameData.pos_head_new;
        float scaleMod = pActor.a.getScaleMod();
        vector3 = Vector2.op_Implicit(new Vector2((float) (-0.40000000596046448 * (double) scaleMod + (double) local2.x * (double) local1.x), (float) (-0.25 * (double) scaleMod + (double) local2.y * (double) local1.y)));
      }
      else
      {
        float num = Mathf.Max((float) pActor.a.asset.actor_size, 3f) / 13f;
        vector3 = Vector2.op_Implicit(new Vector2(0.0f, (!pActor.a.isInLiquid() ? 5f : 1f) * num * local1.y));
      }
      return vector3;
    });
    pAsset57.get_override_sprite_rotation_z = (GetEffectSpriteRotationZ) ((pActor, pIndex) => !pActor.a.has_rendered_sprite_head ? 0.0f : -30f);
    pAsset57.get_override_sprite_ui = (GetEffectSpriteUI) ((pEffect, pIndex) =>
    {
      UnitAvatarLoader avatar = pEffect.getAvatar();
      ActorAvatarData data = avatar.getData();
      AnimationContainerUnit animationContainer = avatar.getAnimationContainer();
      Sprite frame = animationContainer.walking.frames[0];
      return data.getColoredSprite(frame, animationContainer);
    });
    pAsset57.get_override_sprite_position_ui = (GetEffectSpritePositionUI) ((pEffect, pIndex) =>
    {
      UnitAvatarLoader avatar = pEffect.getAvatar();
      ActorAvatarData data = avatar.getData();
      Vector2 vector2 = new Vector2();
      int actualSpriteIndex = avatar.getActualSpriteIndex();
      if (data.hasRenderedSpriteHead())
      {
        AnimationContainerUnit animationContainer = avatar.getAnimationContainer();
        string key = !data.is_touching_liquid || !animationContainer.has_swimming ? ((Object) animationContainer.walking.frames[actualSpriteIndex]).name : ((Object) animationContainer.swimming.frames[actualSpriteIndex]).name;
        ref Vector2 local3 = ref animationContainer.dict_frame_data[key].pos_head;
        ref float local4 = ref pEffect.getAsset().scale;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector((float) ((double) local3.x * (double) local4 - 0.40000000596046448), (float) ((double) local3.y * (double) local4 - 0.25));
      }
      else
      {
        float num1 = Mathf.Max((float) data.asset.actor_size, 3f) / 13f;
        float num2 = !data.is_touching_liquid ? 5f : 1f;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(0.0f, num2 * num1);
      }
      return Vector2.op_Implicit(vector2);
    });
    pAsset57.get_override_sprite_rotation_z_ui = (GetEffectSpriteRotationZUI) ((pEffect, pIndex) => !pEffect.getAvatar().getData().hasRenderedSpriteHead() ? 0.0f : -30f);
    pAsset57.get_sprites_count = (GetEffectSpriteCount) ((_1, _2) => 1);
    this.add(pAsset57);
    this.t.render_check = (RenderEffectCheck) (pAsset => pAsset.render_budding);
    this.t.action_finish += new WorldAction(this.actionBuddingFinish);
    StatusAsset pAsset58 = new StatusAsset();
    pAsset58.id = "flicked";
    pAsset58.locale_id = "status_title_flicked";
    pAsset58.locale_description = "status_description_flicked";
    pAsset58.duration = 3f;
    pAsset58.path_icon = "ui/Icons/iconFingerFlick";
    pAsset58.action_death = (WorldAction) ((pActor, pTile) =>
    {
      if (pActor.a.getActorAsset().id != "beetle" || !pActor.current_tile.Type.lava)
        return false;
      AchievementLibrary.flick_it.check();
      return true;
    });
    this.add(pAsset58);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this.setupBools();
    this.setupCachedSprites();
  }

  private void setupCachedSprites()
  {
    foreach (StatusAsset statusAsset in this.list)
    {
      if (statusAsset.texture != null && statusAsset.sprite_list == null)
        statusAsset.sprite_list = SpriteTextureLoader.getSpriteList("effects/" + statusAsset.texture);
    }
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    foreach (StatusAsset pAsset in this.list)
    {
      if (!string.IsNullOrEmpty(pAsset.texture))
        this.checkSpriteExists("texture", "effects/" + pAsset.texture, (Asset) pAsset);
      this.checkSpriteExists("path_icon", pAsset.path_icon, (Asset) pAsset);
    }
  }

  public override void editorDiagnosticLocales()
  {
    foreach (StatusAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
    base.editorDiagnosticLocales();
  }

  private void setupBools()
  {
    foreach (StatusAsset statusAsset in this.list)
    {
      if (statusAsset.get_override_sprite != null)
      {
        statusAsset.has_override_sprite = true;
        statusAsset.need_visual_render = true;
      }
      if (statusAsset.get_override_sprite_position != null)
        statusAsset.has_override_sprite_position = true;
      if (statusAsset.get_override_sprite_rotation_z != null)
        statusAsset.has_override_sprite_rotation_z = true;
      if (statusAsset.texture != null)
        statusAsset.need_visual_render = true;
    }
  }

  private bool actionPregnancyFinish(BaseSimObject pTarget, WorldTile pTile)
  {
    if (!pTarget.isAlive())
      return false;
    BabyMaker.makeBabyFromPregnancy(pTarget.a);
    return true;
  }

  private bool actionPregnancyParthenogenesisFinish(BaseSimObject pTarget, WorldTile pTile)
  {
    if (!pTarget.isAlive())
      return false;
    BabyMaker.makeBabyViaParthenogenesis(pTarget.a);
    return true;
  }

  private bool actionTakingRootsFinish(BaseSimObject pTarget, WorldTile pTile)
  {
    if (!pTarget.isAlive())
      return false;
    BabyMaker.makeBabyViaVegetative(pTarget.a);
    return true;
  }

  private bool actionBuddingFinish(BaseSimObject pTarget, WorldTile pTile)
  {
    if (!pTarget.isAlive())
      return false;
    Actor a = pTarget.a;
    BabyMaker.makeBabyViaBudding(pTarget as Actor).applyRandomForce();
    a.applyRandomForce();
    return true;
  }

  public static bool ashFeverEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    int maxHealthPercent = pTarget.getMaxHealthPercent(0.01f);
    pTarget.getHit((float) maxHealthPercent, pAttackType: AttackType.AshFever);
    return true;
  }

  public static bool burningEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget.isActor() && pTarget.a.asset.has_skin && Randy.randomBool())
      pTarget.a.addInjuryTrait("skin_burns");
    int pDamage = pTarget.getMaxHealthPercent(0.1f);
    if (pTarget.isBuilding() && pTarget.b.isRuin())
      pDamage = (int) ((double) pDamage * 0.25 + 1.0);
    pTarget.getHit((float) pDamage, pAttackType: AttackType.Fire);
    if (MapBox.isRenderGameplay() && Randy.randomChance(0.1f))
      World.world.particles_fire.spawn(pTarget.current_position.x, pTarget.current_position.y, true);
    return true;
  }

  public static bool spawnShieldHitEffect(
    BaseSimObject pSelf,
    BaseSimObject pAttackedBy,
    WorldTile pTile = null)
  {
    if (!pSelf.isAlive())
      return false;
    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_shield_hit", pSelf.current_position, 1f);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return false;
    baseEffect.attachTo(pSelf.a);
    return true;
  }

  public static bool poisonedEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    float pDamage = Mathf.Max((float) pTarget.getHealth() * 0.01f, 1f);
    if (Randy.randomBool() && pTarget.getHealth() > 1)
      pTarget.getHit(pDamage, pAttackType: AttackType.Poison);
    pTarget.a.spawnParticle(Toolbox.color_infected);
    pTarget.a.startShake(0.4f, 0.2f, pVertical: false);
    return true;
  }

  public void linkMaterials()
  {
    foreach (StatusAsset statusAsset in this.list)
    {
      Material material = LibraryMaterials.instance.dict[statusAsset.material_id];
      statusAsset.material = material;
    }
  }

  public string addToGameplayReport(string pWhat)
  {
    string str1 = $"{string.Empty}{pWhat}\n";
    foreach (StatusAsset statusAsset in this.list)
    {
      string str2 = statusAsset.getLocaleID().Localize();
      string str3 = statusAsset.getDescriptionID().Localize();
      string str4 = "\n" + str2 + "\n";
      if (!string.IsNullOrEmpty(str3))
        str4 = $"{str4}1: {str3}";
      str1 += str4;
    }
    return str1 + "\n\n";
  }

  private bool possessedAction(BaseSimObject pTarget, WorldTile _)
  {
    Actor a = pTarget.a;
    if (a.asset.id == "crabzilla" || a.is_unconscious)
      return false;
    this.checkPossessedMovement(a);
    this.checkPossessedAttackLeft(a);
    this.checkPossessedAttackRight(a);
    this.checkPossessedFlip(a);
    this.checkBonusActions(a);
    return true;
  }

  private void checkBonusActions(Actor pActor)
  {
    if (pActor.is_immovable)
      return;
    if (ControllableUnit.isActionPressedJump())
      this.checkJump(pActor);
    if (ControllableUnit.isActionPressedTalk())
      this.checkTalk(pActor);
    if (ControllableUnit.isActionPressedDash())
      this.checkDash(pActor);
    if (ControllableUnit.isActionPressedBackstep())
      this.checkBackstep(pActor);
    if (ControllableUnit.isActionPressedSteal())
      this.checkSteal(pActor);
    if (!ControllableUnit.isActionPressedSwear())
      return;
    this.checkSwear(pActor);
  }

  private void checkTalk(Actor pActor)
  {
    if (pActor.under_forces || !pActor.asset.control_can_talk || pActor.hasTrait("mute") || !pActor.isAttackReady())
      return;
    Vector2 controlTargetPosition = pActor.getPossessionControlTargetPosition();
    pActor.spawnSlashTalk(controlTargetPosition);
    pActor.punchTargetAnimation(Vector2.op_Implicit(controlTargetPosition), false, pAngle: -20f);
    pActor.lookTowardsPosition(controlTargetPosition);
    pActor.setActionTimeout(1f);
    Actor pUnit = this.getActorTargetRaycast(pActor) ?? this.getActorTargetNearCursor(pActor);
    if (pUnit == null || pUnit.is_unconscious || !pUnit.asset.can_talk_with || pUnit.hasStatus("possessed") || ControllableUnit.isControllingUnit(pUnit))
      return;
    if (Randy.randomChance(0.3f))
      pUnit.getSurprised(pActor.current_tile, true);
    pUnit.cancelAllBeh();
    pUnit.tryToConvertActorToMetaFromActor(pActor, false);
    pUnit.addStatusEffect("possessed_follower");
    pUnit.lookTowardsPosition(pActor.current_position);
    pUnit.setTask("socialize_receiving", pForceAction: true);
    pActor.clearLastTopicSprite();
    pUnit.clearLastTopicSprite();
    float pValue = 2f + Randy.randomFloat(0.0f, 3f);
    pUnit.makeWait(pValue);
    pActor.setTask("socialize_receiving", pForceAction: true);
    pActor.makeWait(pValue);
  }

  private void checkSteal(Actor pActor)
  {
    if (pActor.under_forces || !pActor.asset.control_can_steal || !pActor.isAttackReady())
      return;
    Vector2 controlTargetPosition = pActor.getPossessionControlTargetPosition();
    pActor.spawnSlashSteal(controlTargetPosition);
    pActor.punchTargetAnimation(Vector2.op_Implicit(controlTargetPosition), false);
    pActor.lookTowardsPosition(controlTargetPosition);
    pActor.setActionTimeout(1f);
    Actor actorTargetRaycast = this.getActorTargetRaycast(pActor);
    if (actorTargetRaycast == null || (double) Vector2.Distance(pActor.current_position, actorTargetRaycast.current_position) >= 2.0)
      return;
    pActor.stealActionFrom(actorTargetRaycast, pPossessedSteal: true);
  }

  private void checkSwear(Actor pActor)
  {
    if (!pActor.asset.control_can_swear || pActor.hasTrait("mute") || !pActor.isAttackReady())
      return;
    Vector2 controlTargetPosition = pActor.getPossessionControlTargetPosition();
    pActor.addStatusEffect("swearing", 2f, false);
    pActor.punchTargetAnimation(Vector2.op_Implicit(controlTargetPosition), false, pAngle: -40f);
    pActor.spawnSlashYell(controlTargetPosition);
    pActor.lookTowardsPosition(controlTargetPosition);
    pActor.setActionTimeout(1f);
    Actor actor = this.getActorTargetRaycast(pActor) ?? this.getActorTargetNearCursor(pActor);
    if (actor == null)
      return;
    actor.tryToGetSurprised(pActor.current_tile, true);
    actor.addAggro(pActor);
  }

  private void checkBackstep(Actor pActor)
  {
    if (!pActor.asset.control_can_backstep)
      return;
    CombatActionAsset combatActionBackstep = CombatActionLibrary.combat_action_backstep;
    Vector2 positionMovementVector = pActor.getPossessionControlTargetPositionMovementVector();
    int num = combatActionBackstep.action_actor_target_position(pActor, positionMovementVector) ? 1 : 0;
  }

  private void checkDash(Actor pActor)
  {
    if (!pActor.asset.control_can_dash)
      return;
    CombatActionAsset combatActionDash = CombatActionLibrary.combat_action_dash;
    Vector2 positionMovementVector = pActor.getPossessionControlTargetPositionMovementVector();
    int num = combatActionDash.action_actor_target_position(pActor, positionMovementVector) ? 1 : 0;
  }

  private void checkJump(Actor pActor)
  {
    if (pActor.under_forces || !pActor.asset.control_can_jump)
      return;
    float pForceAmountDirection = Mathf.Clamp(SimGlobals.m.gravity * 0.5f / (pActor.stats["mass_2"] * pActor.actor_scale), 1.5f, 2.5f);
    if (pActor.hasTrait("weightless"))
      pForceAmountDirection += pForceAmountDirection * 0.5f;
    float pForceHeight = pForceAmountDirection;
    Vector2 vector2 = Vector2.op_Subtraction(pActor.current_position, ControllableUnit.getMovementVector());
    Vector2 currentPosition = pActor.current_position;
    EffectsLibrary.spawnAt("fx_jump", currentPosition, 0.1f);
    pActor.calculateForce(currentPosition.x, currentPosition.y, vector2.x, vector2.y, pForceAmountDirection, pForceHeight, true);
  }

  private void checkPossessedAttackLeft(Actor pActor)
  {
    if (!ControllableUnit.isAttackPressedLeft() || Config.joyControls && !TouchPossessionController.isSelectedActionAttack() || pActor.asset.skip_fight_logic || !pActor.isAttackReady())
      return;
    Actor actorTargetAttack = this.getActorTargetAttack(pActor);
    float attackRange = pActor.getAttackRange();
    Vector3 pAttackPosition = Vector2.op_Implicit(this.getAttackPos(pActor));
    if (actorTargetAttack != null && pActor.hasMeleeAttack())
    {
      Vector3 vector3 = Vector2.op_Implicit(actorTargetAttack.current_position);
      float pDist = Vector2.Distance(pActor.current_position, Vector2.op_Implicit(vector3));
      if ((double) pDist > (double) attackRange)
        pDist = attackRange;
      pAttackPosition = Vector2.op_Implicit(Toolbox.getNewPointVec2(pActor.current_position, Vector2.op_Implicit(pAttackPosition), pDist));
    }
    Kingdom pForceKingdom = World.world.kingdoms_wild.get("possessed");
    pActor.tryToAttack((BaseSimObject) null, false, pAttackPosition: pAttackPosition, pForceKingdom: pForceKingdom, pTileTarget: actorTargetAttack?.current_tile, pBonusAreOfEffect: attackRange * 0.2f);
    pActor.setPossessionAttackHappened();
  }

  private void checkPossessedAttackRight(Actor pActor)
  {
    if (!ControllableUnit.isAttackJustPressedRight() || !pActor.asset.control_can_kick || pActor.asset.skip_fight_logic || !pActor.isAttackReady())
      return;
    Vector2 controlTargetPosition = pActor.getPossessionControlTargetPosition();
    pActor.punchTargetAnimation(Vector2.op_Implicit(controlTargetPosition), false, pAngle: -20f);
    pActor.setActionTimeout(1f);
    pActor.spawnSlashKick(controlTargetPosition);
    pActor.lookTowardsPosition(controlTargetPosition);
    pActor.setPossessionAttackHappened();
    Actor actorTargetAttack = this.getActorTargetAttack(pActor, 2f);
    if (actorTargetAttack == null || (double) Vector2.Distance(pActor.current_position, actorTargetAttack.current_position) >= 2.0)
      return;
    actorTargetAttack.makeStunned();
    Vector2 currentPosition = pActor.current_position;
    currentPosition.y += pActor.current_scale.y;
    Vector2 vector2 = controlTargetPosition;
    float pForceAmountDirection = Randy.randomFloat(1.5f, 3f);
    float pForceHeight = Randy.randomFloat(1.5f, 3f);
    if (pActor.under_forces || pActor.hasStatus("dash"))
    {
      pForceAmountDirection *= 2f;
      pForceHeight *= 1.5f;
    }
    actorTargetAttack.calculateForce(vector2.x, vector2.y, currentPosition.x, currentPosition.y, pForceAmountDirection, pForceHeight, true);
    actorTargetAttack.addAggro(pActor);
  }

  private Vector2 getAttackPos(Actor pActorFor, float pRange = 0.0f)
  {
    Vector2 currentPosition = pActorFor.current_position;
    Vector2 mousePos = this.mouse_pos;
    Vector2 attackPos = mousePos;
    float pDist = (double) pRange != 0.0 ? pRange : pActorFor.getAttackRange();
    if (pActorFor.hasMeleeAttack())
      attackPos = Toolbox.getNewPointVec2(currentPosition, mousePos, pDist);
    return attackPos;
  }

  private Actor getActorTargetAttack(Actor pActorFor, float pRange = 0.0f)
  {
    float num1 = (double) pRange != 0.0 ? pRange : pActorFor.getAttackRange();
    Vector2 attackPos = this.getAttackPos(pActorFor, num1);
    WorldTile pTile = World.world.GetTile((int) attackPos.x, (int) attackPos.y) ?? pActorFor.current_tile;
    float num2 = float.MaxValue;
    Actor actorTargetAttack = (Actor) null;
    float num3 = num1 * num1;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 0, num1, true))
    {
      if (pActorFor != actor)
      {
        float num4 = Toolbox.SquaredDistVec2Float(attackPos, actor.current_position);
        if ((double) num4 <= (double) num3 && (double) num4 < (double) num2)
        {
          actorTargetAttack = actor;
          num2 = num4;
        }
      }
    }
    return actorTargetAttack;
  }

  private Actor getActorTargetRaycast(Actor pActorFor)
  {
    Vector2 tActorPos = pActorFor.current_position;
    Vector2 mousePos = this.mouse_pos;
    List<WorldTile> worldTileList = PathfinderTools.raycast(tActorPos, mousePos);
    float tDistance = float.MaxValue;
    Actor tActorTarget = (Actor) null;
    foreach (WorldTile worldTile in worldTileList)
    {
      if (worldTile.hasUnits())
      {
        worldTile.doUnits((Action<Actor>) (tActor =>
        {
          if (tActor == pActorFor)
            return;
          float num = Toolbox.SquaredDistVec2Float(tActorPos, tActor.current_position);
          if ((double) num >= (double) tDistance)
            return;
          tDistance = num;
          tActorTarget = tActor;
        }));
        if (tActorTarget != null)
          break;
      }
    }
    return tActorTarget == null || tActorTarget == pActorFor ? (Actor) null : tActorTarget;
  }

  private Actor getActorTargetNearCursor(Actor pActorFor)
  {
    Actor actorNearCursor = World.world.getActorNearCursor();
    return actorNearCursor == null || actorNearCursor == pActorFor ? (Actor) null : actorNearCursor;
  }

  private void checkPossessedMovement(Actor pActor)
  {
    if (!ControllableUnit.isMovementActionActive())
    {
      pActor.setPossessedMovement(false);
    }
    else
    {
      if (pActor.is_immovable)
        return;
      Vector2 movementVector = ControllableUnit.getMovementVector();
      if (Vector2.op_Equality(movementVector, Vector2.zero))
        return;
      pActor.setPossessedMovement(true);
      Vector2 currentPosition = pActor.current_position;
      pActor.next_step_position_possession = Vector2.op_Addition(currentPosition, Vector2.op_Multiply(movementVector, 2f));
      Vector2 pMovementPoint = Vector2.op_Addition(movementVector, currentPosition);
      float num = pActor.updatePossessedMovementTowards(World.world.elapsed, pMovementPoint);
      if (pActor.isInAir() || pActor.under_forces || (double) num < 0.029999999329447746)
        return;
      BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_walk", currentPosition, 0.1f);
      if (!Object.op_Inequality((Object) baseEffect, (Object) null))
        return;
      ((Component) baseEffect).GetComponent<SpriteRenderer>().flipX = !pActor.flip;
    }
  }

  private void checkPossessedFlip(Actor pActor) => pActor.updateMovementPossessedFlip();

  public Vector2 mouse_pos => ControllableUnit.getClickVector();
}
