// Decompiled with JetBrains decompiler
// Type: AchievementLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public class AchievementLibrary : AssetLibrary<Achievement>
{
  private const int ZOO_SPECIES_NEED = 33;
  private const int SPECIES_EXPLORER_ASSETS = 52;
  private const int TRAITS_EXPLORER_AMOUNT_FIRST = 40;
  private const int TRAITS_EXPLORER_AMOUNT_SECOND = 60;
  private const int TRAITS_EXPLORER_AMOUNT_THIRD = 90;
  private const int SUBSPECIES_TRAITS_EXPLORER_AMOUNT = 190;
  private const int CULTURE_TRAITS_EXPLORER_AMOUNT = 70;
  private const int LANGUAGE_TRAITS_EXPLORER_AMOUNT = 20;
  private const int CLAN_TRAITS_EXPLORER_AMOUNT = 25;
  private const int RELIGION_TRAITS_EXPLORER_AMOUNT = 33;
  private const int EQUIPMENT_EXPLORER_AMOUNT = 80 /*0x50*/;
  private const int GENES_EXPLORER_AMOUNT = 35;
  private const int PLOTS_EXPLORER_AMOUNT = 20;
  private const string ONOMASTICS_NAME_FOR_ACHIEVEMENT = "Mako Mako";
  private const int NOT_JUST_A_CULT_UNITS = 7777;
  private const int MULTIPLY_SPOKEN_UNITS = 5555;
  public static Achievement lava_strike;
  public static Achievement baby_tornado;
  public static Achievement rain_tornado;
  public static Achievement many_bombs;
  public static Achievement megapolis;
  public static Achievement wilhelm_scream;
  public static Achievement burger;
  public static Achievement mayday;
  public static Achievement destroy_worldbox;
  public static Achievement custom_world;
  public static Achievement four_race_cities;
  public static Achievement piranha_land;
  public static Achievement print_heart;
  public static Achievement sacrifice;
  public static Achievement final_resolution;
  public static Achievement tnt_and_heat;
  public static Achievement god_finger_lightning;
  public static Achievement ten_thousands_creatures;
  public static Achievement ant_world;
  public static Achievement traits_explorer_40;
  public static Achievement traits_explorer_60;
  public static Achievement traits_explorer_90;
  public static Achievement trait_explorer_subspecies;
  public static Achievement trait_explorer_culture;
  public static Achievement trait_explorer_language;
  public static Achievement trait_explorer_clan;
  public static Achievement trait_explorer_religion;
  public static Achievement equipment_explorer;
  public static Achievement genes_explorer;
  public static Achievement creatures_explorer;
  public static Achievement plots_explorer;
  public static Achievement the_builder;
  public static Achievement the_dwarf;
  public static Achievement the_creator;
  public static Achievement the_light;
  public static Achievement the_sky;
  public static Achievement the_land;
  public static Achievement the_sun;
  public static Achievement the_moon;
  public static Achievement the_living;
  public static Achievement the_rest_day;
  public static Achievement life_is_a_sim;
  public static Achievement gen_5_worlds;
  public static Achievement gen_50_worlds;
  public static Achievement gen_100_worlds;
  public static Achievement the_corrupted_trees;
  public static Achievement the_hell;
  public static Achievement lets_not;
  public static Achievement world_war;
  public static Achievement planet_of_apes;
  public static Achievement super_mushroom;
  public static Achievement the_princess;
  public static Achievement oh_my_crab;
  public static Achievement tornado;
  public static Achievement god_mode;
  public static Achievement greg;
  public static Achievement ninja_turtle;
  public static Achievement great_plague;
  public static Achievement no_hope_only_mush;
  public static Achievement touch_the_grass;
  public static Achievement the_broken;
  public static Achievement the_king;
  public static Achievement the_demon;
  public static Achievement the_accomplished;
  public static Achievement cursed_world;
  public static Achievement boats_disposal;
  public static Achievement engineered_evolution;
  public static Achievement simple_stupid_genetics;
  public static Achievement fast_living;
  public static Achievement long_living;
  public static Achievement ancient_war_of_geometry_and_evil;
  public static Achievement cant_be_too_much;
  public static Achievement zoo;
  public static Achievement mindless_husk;
  public static Achievement master_weaver;
  public static Achievement not_just_a_cult;
  public static Achievement succession;
  public static Achievement multiply_spoken;
  public static Achievement child_named_toto;
  public static Achievement flick_it;
  public static Achievement segregator;
  public static Achievement swarm;
  public static Achievement eternal_chaos;
  public static Achievement minefield;
  public static Achievement godly_smithing;
  public static Achievement master_of_combat;
  public static Achievement clannibals;
  public static Achievement social_network;
  public static Achievement not_on_my_watch;
  public static Achievement may_i_interrupt;
  public static Achievement watch_your_mouth;
  public static Achievement smelly_city;
  public static Achievement ball_to_ball;
  public static Achievement back_to_beta_testing;
  public static Achievement clone_wars;
  public static Achievement sword_with_shotgun;
  public static Achievement tldr;
  public const float LIFE_IS_SIM_HOURS = 24f;

  public override void init()
  {
    base.init();
    Debug.Log((object) "Init Achievements");
    Achievement pAsset1 = new Achievement();
    pAsset1.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset1.steam_id = "1_20";
    pAsset1.id = "achievementTheAccomplished";
    pAsset1.action = new AchievementCheck(AchievementLibrary.checkTheAccomplished);
    pAsset1.icon = "ui/Icons/achievements/achievements_theAccomplished";
    pAsset1.group = "creatures";
    AchievementLibrary.the_accomplished = this.add(pAsset1);
    Achievement pAsset2 = new Achievement();
    pAsset2.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset2.steam_id = "1_17";
    pAsset2.id = "achievementTheKing";
    pAsset2.action = new AchievementCheck(AchievementLibrary.checkTheKing);
    pAsset2.icon = "ui/Icons/achievements/achievements_theKing";
    pAsset2.group = "creatures";
    AchievementLibrary.the_king = this.add(pAsset2);
    Achievement pAsset3 = new Achievement();
    pAsset3.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset3.steam_id = "1_21";
    pAsset3.id = "achievementTheDemon";
    pAsset3.action = new AchievementCheck(AchievementLibrary.checkTheDemon);
    pAsset3.icon = "ui/Icons/achievements/achievements_theDemon";
    pAsset3.group = "creatures";
    AchievementLibrary.the_demon = this.add(pAsset3);
    Achievement pAsset4 = new Achievement();
    pAsset4.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset4.steam_id = "1_22";
    pAsset4.id = "achievementTheBroken";
    pAsset4.action = new AchievementCheck(AchievementLibrary.checkTheBroken);
    pAsset4.icon = "ui/Icons/achievements/achievements_theBroken";
    pAsset4.group = "creatures";
    AchievementLibrary.the_broken = this.add(pAsset4);
    Achievement pAsset5 = new Achievement();
    pAsset5.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset5.steam_id = "2_10";
    pAsset5.id = "achievementTouchTheGrass";
    pAsset5.icon = "ui/Icons/achievements/achievements_touchTheGrass";
    pAsset5.group = "nature";
    pAsset5.hidden = true;
    AchievementLibrary.touch_the_grass = this.add(pAsset5);
    Achievement pAsset6 = new Achievement();
    pAsset6.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset6.steam_id = "1_0";
    pAsset6.id = "achievementGen5Worlds";
    pAsset6.action = new AchievementCheck(AchievementLibrary.checkMapCreations5);
    pAsset6.icon = "ui/Icons/achievements/achievements_generate5";
    pAsset6.group = "worlds";
    AchievementLibrary.gen_5_worlds = this.add(pAsset6);
    Achievement pAsset7 = new Achievement();
    pAsset7.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset7.steam_id = "1_1";
    pAsset7.id = "achievementGen50Worlds";
    pAsset7.action = new AchievementCheck(AchievementLibrary.checkMapCreations50);
    pAsset7.icon = "ui/Icons/achievements/achievements_generate50";
    pAsset7.group = "worlds";
    AchievementLibrary.gen_50_worlds = this.add(pAsset7);
    Achievement pAsset8 = new Achievement();
    pAsset8.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset8.steam_id = "1_2";
    pAsset8.id = "achievementGen100Worlds";
    pAsset8.action = new AchievementCheck(AchievementLibrary.checkMapCreations100);
    pAsset8.icon = "ui/Icons/achievements/achievements_generate100";
    pAsset8.group = "worlds";
    AchievementLibrary.gen_100_worlds = this.add(pAsset8);
    Achievement pAsset9 = new Achievement();
    pAsset9.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset9.steam_id = "1_3";
    pAsset9.id = "achievementLifeIsASim";
    pAsset9.hidden = true;
    pAsset9.action = new AchievementCheck(AchievementLibrary.checkLifeIsASim);
    pAsset9.icon = "ui/Icons/achievements/achievements_lifeissimulation";
    pAsset9.group = "miscellaneous";
    AchievementLibrary.life_is_a_sim = this.add(pAsset9);
    Achievement pAsset10 = new Achievement();
    pAsset10.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset10.steam_id = "1_31";
    pAsset10.id = "achievementTheCorruptedTrees";
    pAsset10.icon = "ui/Icons/achievements/achievements_corruptedbiome";
    pAsset10.group = "exploration";
    AchievementLibrary.the_corrupted_trees = this.add(pAsset10);
    Achievement pAsset11 = new Achievement();
    pAsset11.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset11.steam_id = "2_0";
    pAsset11.id = "achievementTheHell";
    pAsset11.hidden = true;
    pAsset11.action = new AchievementCheck(AchievementLibrary.checkTheHell);
    pAsset11.icon = "ui/Icons/achievements/achievements_infernalbiome";
    pAsset11.group = "destruction";
    AchievementLibrary.the_hell = this.add(pAsset11);
    Achievement pAsset12 = new Achievement();
    pAsset12.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset12.steam_id = "2_1";
    pAsset12.id = "achievementLetsNot";
    pAsset12.hidden = true;
    pAsset12.action = new AchievementCheck(AchievementLibrary.checkLetsNot);
    pAsset12.icon = "ui/Icons/achievements/achievements_wastelandbiome";
    pAsset12.group = "destruction";
    AchievementLibrary.lets_not = this.add(pAsset12);
    Achievement pAsset13 = new Achievement();
    pAsset13.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset13.steam_id = "2_2";
    pAsset13.id = "achievementWorldWar";
    pAsset13.hidden = true;
    pAsset13.action = new AchievementCheck(AchievementLibrary.checkWorldWar);
    pAsset13.icon = "ui/Icons/achievements/achievements_worldwar";
    pAsset13.group = "civilizations";
    AchievementLibrary.world_war = this.add(pAsset13);
    Achievement pAsset14 = new Achievement();
    pAsset14.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset14.steam_id = "1_23";
    pAsset14.id = "achievementPlanetOfApes";
    pAsset14.hidden = true;
    pAsset14.action = new AchievementCheck(AchievementLibrary.checkPlanetOfTheApes);
    pAsset14.icon = "ui/Icons/achievements/achievements_planetofapes";
    pAsset14.group = "creatures";
    AchievementLibrary.planet_of_apes = this.add(pAsset14);
    Achievement pAsset15 = new Achievement();
    pAsset15.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset15.steam_id = "1_24";
    pAsset15.id = "achievementSuperMushroom";
    pAsset15.hidden = true;
    pAsset15.icon = "ui/Icons/achievements/achievements_supermush";
    pAsset15.group = "creatures";
    AchievementLibrary.super_mushroom = this.add(pAsset15);
    Achievement pAsset16 = new Achievement();
    pAsset16.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset16.steam_id = "1_25";
    pAsset16.id = "achievementThePrincess";
    pAsset16.hidden = true;
    pAsset16.icon = "ui/Icons/achievements/achievements_princess";
    pAsset16.group = "creatures";
    AchievementLibrary.the_princess = this.add(pAsset16);
    Achievement pAsset17 = new Achievement();
    pAsset17.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset17.steam_id = "2_11";
    pAsset17.id = "achievementTORNADO";
    pAsset17.locale_key = "achievement_tornado";
    pAsset17.hidden = true;
    pAsset17.icon = "ui/Icons/achievements/achievements_cursedtornado";
    pAsset17.group = "nature";
    AchievementLibrary.tornado = this.add(pAsset17);
    Achievement pAsset18 = new Achievement();
    pAsset18.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset18.steam_id = "2_13";
    pAsset18.id = "achievementGodMode";
    pAsset18.hidden = true;
    pAsset18.icon = "ui/Icons/achievements/achievements_godmode";
    pAsset18.group = "miscellaneous";
    AchievementLibrary.god_mode = this.add(pAsset18);
    Achievement pAsset19 = new Achievement();
    pAsset19.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset19.steam_id = "1_26";
    pAsset19.id = "achievementGreg";
    pAsset19.hidden = true;
    pAsset19.icon = "ui/Icons/achievements/achievements_greg";
    pAsset19.group = "forbidden";
    AchievementLibrary.greg = this.add(pAsset19);
    Achievement pAsset20 = new Achievement();
    pAsset20.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset20.steam_id = "1_27";
    pAsset20.id = "achievementNinjaTurtle";
    pAsset20.icon = "ui/Icons/achievements/achievements_ninjaturtle";
    pAsset20.action = (AchievementCheck) (pActor =>
    {
      Actor actor = pActor as Actor;
      return actor.asset.flag_turtle && actor.level >= 10;
    });
    pAsset20.group = "creatures";
    AchievementLibrary.ninja_turtle = this.add(pAsset20);
    Achievement pAsset21 = new Achievement();
    pAsset21.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset21.steam_id = "2_3";
    pAsset21.id = "achievementGreatPlague";
    pAsset21.hidden = true;
    pAsset21.action = new AchievementCheck(AchievementLibrary.checkGreatPlague);
    pAsset21.icon = "ui/Icons/achievements/achievements_plagueworld";
    pAsset21.group = "experiments";
    AchievementLibrary.great_plague = this.add(pAsset21);
    Achievement pAsset22 = new Achievement();
    pAsset22.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset22.steam_id = "2_4";
    pAsset22.id = "achievementLavaStrike";
    pAsset22.hidden = true;
    pAsset22.icon = "ui/Icons/actor_traits/iconLightning";
    pAsset22.group = "destruction";
    AchievementLibrary.lava_strike = this.add(pAsset22);
    Achievement pAsset23 = new Achievement();
    pAsset23.play_store_id = "CgkIia6M98wfEAIQAw";
    pAsset23.steam_id = "2_12";
    pAsset23.id = "achievementBabyTornado";
    pAsset23.hidden = true;
    pAsset23.icon = "ui/Icons/achievements/achievements_babytornado";
    pAsset23.group = "nature";
    AchievementLibrary.baby_tornado = this.add(pAsset23);
    Achievement pAsset24 = new Achievement();
    pAsset24.id = "achievementRainTornado";
    pAsset24.play_store_id = "CgkIia6M98wfEAIQAw";
    pAsset24.steam_id = "2_21";
    pAsset24.icon = "ui/Icons/achievements/achievements_raintornado";
    pAsset24.group = "nature";
    pAsset24.hidden = true;
    pAsset24.action = new AchievementCheck(AchievementLibrary.checkRainTornado);
    AchievementLibrary.rain_tornado = this.add(pAsset24);
    Achievement pAsset25 = new Achievement();
    pAsset25.play_store_id = "CgkIia6M98wfEAIQBA";
    pAsset25.steam_id = "1_4";
    pAsset25.id = "achievement10000Creatures";
    pAsset25.action = new AchievementCheck(AchievementLibrary.check10000Creatures);
    pAsset25.icon = "ui/Icons/achievements/achievements_1000Creatures";
    pAsset25.group = "creation";
    AchievementLibrary.ten_thousands_creatures = this.add(pAsset25);
    Achievement pAsset26 = new Achievement();
    pAsset26.play_store_id = "";
    pAsset26.steam_id = "2_5";
    pAsset26.id = "achievementManyBombs";
    pAsset26.action = new AchievementCheck(AchievementLibrary.checkManyBombs);
    pAsset26.icon = "ui/Icons/iconBomb";
    pAsset26.group = "destruction";
    AchievementLibrary.many_bombs = this.add(pAsset26);
    Achievement pAsset27 = new Achievement();
    pAsset27.play_store_id = "CgkIia6M98wfEAIQBg";
    pAsset27.steam_id = "1_18";
    pAsset27.hidden = true;
    pAsset27.id = "achievementMegapolis";
    pAsset27.action = new AchievementCheck(AchievementLibrary.checkMegapolis);
    pAsset27.icon = "ui/Icons/achievements/achievements_megapolis";
    pAsset27.group = "civilizations";
    AchievementLibrary.megapolis = this.add(pAsset27);
    Achievement pAsset28 = new Achievement();
    pAsset28.play_store_id = "CgkIia6M98wfEAIQBw";
    pAsset28.steam_id = "2_14";
    pAsset28.id = "achievementMakeWilhelmScream";
    pAsset28.hidden = true;
    pAsset28.icon = "ui/Icons/iconHumans";
    pAsset28.group = "exploration";
    AchievementLibrary.wilhelm_scream = this.add(pAsset28);
    Achievement pAsset29 = new Achievement();
    pAsset29.play_store_id = "?";
    pAsset29.steam_id = "2_15";
    pAsset29.id = "achievementBurger";
    pAsset29.hidden = true;
    pAsset29.icon = "ui/Icons/iconBurger";
    pAsset29.group = "exploration";
    AchievementLibrary.burger = this.add(pAsset29);
    Achievement pAsset30 = new Achievement();
    pAsset30.play_store_id = "?";
    pAsset30.steam_id = "2_16";
    pAsset30.id = "achievementPie";
    pAsset30.hidden = true;
    pAsset30.icon = "ui/Icons/iconResPie";
    pAsset30.group = "exploration";
    AchievementLibrary.burger = this.add(pAsset30);
    Achievement pAsset31 = new Achievement();
    pAsset31.play_store_id = "?";
    pAsset31.steam_id = "2_6";
    pAsset31.id = "achievementMayday";
    pAsset31.hidden = true;
    pAsset31.icon = "ui/Icons/iconSanta";
    pAsset31.group = "destruction";
    AchievementLibrary.mayday = this.add(pAsset31);
    Achievement pAsset32 = new Achievement();
    pAsset32.play_store_id = "?";
    pAsset32.steam_id = "2_17";
    pAsset32.id = "achievementDestroyWorldBox";
    pAsset32.hidden = true;
    pAsset32.icon = "ui/Icons/iconBrowse2";
    pAsset32.group = "exploration";
    AchievementLibrary.destroy_worldbox = this.add(pAsset32);
    Achievement pAsset33 = new Achievement();
    pAsset33.play_store_id = "?";
    pAsset33.steam_id = "1_5";
    pAsset33.id = "achievementCustomWorld";
    pAsset33.hidden = true;
    pAsset33.icon = "ui/Icons/iconTileSoil";
    pAsset33.group = "creation";
    AchievementLibrary.custom_world = this.add(pAsset33);
    Achievement pAsset34 = new Achievement();
    pAsset34.play_store_id = "?";
    pAsset34.steam_id = "1_19";
    pAsset34.id = "achievement4RaceCities";
    pAsset34.hidden = true;
    pAsset34.icon = "ui/Icons/achievements/achievements_4Races";
    pAsset34.action = new AchievementCheck(AchievementLibrary.check4RaceCities);
    pAsset34.group = "civilizations";
    AchievementLibrary.four_race_cities = this.add(pAsset34);
    Achievement pAsset35 = new Achievement();
    pAsset35.play_store_id = "?";
    pAsset35.steam_id = "1_28";
    pAsset35.id = "achievementPiranhaLand";
    pAsset35.hidden = true;
    pAsset35.icon = "ui/Icons/iconPiranha";
    pAsset35.action = new AchievementCheck(AchievementLibrary.checkPiranhaLand);
    pAsset35.group = "experiments";
    AchievementLibrary.piranha_land = this.add(pAsset35);
    Achievement pAsset36 = new Achievement();
    pAsset36.play_store_id = "?";
    pAsset36.steam_id = "1_6";
    pAsset36.id = "achievementPrintHeart";
    pAsset36.hidden = true;
    pAsset36.action = new AchievementCheck(AchievementLibrary.checkPrintHeart);
    pAsset36.icon = "ui/Icons/achievements/achievements_printHeart";
    pAsset36.group = "creation";
    AchievementLibrary.print_heart = this.add(pAsset36);
    Achievement pAsset37 = new Achievement();
    pAsset37.play_store_id = "?";
    pAsset37.steam_id = "1_29";
    pAsset37.id = "achievementSacrifice";
    pAsset37.hidden = true;
    pAsset37.icon = "ui/Icons/iconSheep";
    pAsset37.group = "experiments";
    AchievementLibrary.sacrifice = this.add(pAsset37);
    Achievement pAsset38 = new Achievement();
    pAsset38.play_store_id = "?";
    pAsset38.steam_id = "1_30";
    pAsset38.id = "achievementAntWorld";
    pAsset38.hidden = true;
    pAsset38.icon = "ui/Icons/iconAntBlack";
    pAsset38.action = new AchievementCheck(AchievementLibrary.checkAntWorld);
    pAsset38.group = "creatures";
    AchievementLibrary.ant_world = this.add(pAsset38);
    Achievement pAsset39 = new Achievement();
    pAsset39.play_store_id = "?";
    pAsset39.steam_id = "2_7";
    pAsset39.id = "achievementFinalResolution";
    pAsset39.hidden = true;
    pAsset39.icon = "ui/Icons/iconGreygoo";
    pAsset39.group = "destruction";
    AchievementLibrary.final_resolution = this.add(pAsset39);
    Achievement pAsset40 = new Achievement();
    pAsset40.play_store_id = "?";
    pAsset40.steam_id = "2_8";
    pAsset40.id = "achievementTntAndHeat";
    pAsset40.hidden = true;
    pAsset40.icon = "ui/Icons/iconTnt";
    pAsset40.group = "destruction";
    AchievementLibrary.tnt_and_heat = this.add(pAsset40);
    Achievement pAsset41 = new Achievement();
    pAsset41.play_store_id = "?";
    pAsset41.steam_id = "2_9";
    pAsset41.id = "achievementGodFingerLightning";
    pAsset41.hidden = true;
    pAsset41.icon = "ui/Icons/iconGodFinger";
    pAsset41.group = "destruction";
    AchievementLibrary.god_finger_lightning = this.add(pAsset41);
    Achievement pAsset42 = new Achievement();
    pAsset42.play_store_id = "?";
    pAsset42.steam_id = "2_18";
    pAsset42.id = "achievementTraitsExplorer40";
    pAsset42.hidden = false;
    pAsset42.icon = "ui/Icons/achievements/achievements_traitexplorer1";
    pAsset42.group = "collection";
    pAsset42.action = (AchievementCheck) (_ => AchievementLibrary.checkTraitsExplorer(40));
    AchievementLibrary.traits_explorer_40 = this.add(pAsset42);
    Achievement pAsset43 = new Achievement();
    pAsset43.play_store_id = "?";
    pAsset43.steam_id = "2_19";
    pAsset43.id = "achievementTraitsExplorer60";
    pAsset43.hidden = false;
    pAsset43.icon = "ui/Icons/achievements/achievements_traitexplorer2";
    pAsset43.group = "collection";
    pAsset43.action = (AchievementCheck) (_ => AchievementLibrary.checkTraitsExplorer(60));
    AchievementLibrary.traits_explorer_60 = this.add(pAsset43);
    Achievement pAsset44 = new Achievement();
    pAsset44.play_store_id = "?";
    pAsset44.steam_id = "2_20";
    pAsset44.id = "achievementTraitsExplorer90";
    pAsset44.hidden = false;
    pAsset44.icon = "ui/Icons/achievements/achievements_traitexplorer3";
    pAsset44.group = "collection";
    pAsset44.action = (AchievementCheck) (_ => AchievementLibrary.checkTraitsExplorer(90));
    AchievementLibrary.traits_explorer_90 = this.add(pAsset44);
    Achievement pAsset45 = new Achievement();
    pAsset45.play_store_id = "?";
    pAsset45.steam_id = "?";
    pAsset45.id = "achievementTraitExplorerSubspecies";
    pAsset45.hidden = false;
    pAsset45.icon = "ui/Icons/achievements/achievements_traits_explorer_subspecies";
    pAsset45.group = "collection";
    pAsset45.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(190, (ILibraryWithUnlockables) AssetManager.subspecies_traits));
    AchievementLibrary.trait_explorer_subspecies = this.add(pAsset45);
    Achievement pAsset46 = new Achievement();
    pAsset46.play_store_id = "?";
    pAsset46.steam_id = "?";
    pAsset46.id = "achievementTraitExplorerCulture";
    pAsset46.hidden = false;
    pAsset46.icon = "ui/Icons/achievements/achievements_traits_explorer_culture";
    pAsset46.group = "collection";
    pAsset46.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(70, (ILibraryWithUnlockables) AssetManager.culture_traits));
    AchievementLibrary.trait_explorer_culture = this.add(pAsset46);
    Achievement pAsset47 = new Achievement();
    pAsset47.play_store_id = "?";
    pAsset47.steam_id = "?";
    pAsset47.id = "achievementTraitExplorerLanguage";
    pAsset47.hidden = false;
    pAsset47.icon = "ui/Icons/achievements/achievements_traits_explorer_language";
    pAsset47.group = "collection";
    pAsset47.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(20, (ILibraryWithUnlockables) AssetManager.language_traits));
    AchievementLibrary.trait_explorer_language = this.add(pAsset47);
    Achievement pAsset48 = new Achievement();
    pAsset48.play_store_id = "?";
    pAsset48.steam_id = "?";
    pAsset48.id = "achievementTraitExplorerClan";
    pAsset48.hidden = false;
    pAsset48.icon = "ui/Icons/achievements/achievements_traits_explorer_clan";
    pAsset48.group = "collection";
    pAsset48.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(25, (ILibraryWithUnlockables) AssetManager.clan_traits));
    AchievementLibrary.trait_explorer_clan = this.add(pAsset48);
    Achievement pAsset49 = new Achievement();
    pAsset49.play_store_id = "?";
    pAsset49.steam_id = "?";
    pAsset49.id = "achievementTraitExplorerReligion";
    pAsset49.hidden = false;
    pAsset49.icon = "ui/Icons/achievements/achievements_traits_explorer_religion";
    pAsset49.group = "collection";
    pAsset49.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(33, (ILibraryWithUnlockables) AssetManager.religion_traits));
    AchievementLibrary.trait_explorer_religion = this.add(pAsset49);
    Achievement pAsset50 = new Achievement();
    pAsset50.play_store_id = "?";
    pAsset50.steam_id = "?";
    pAsset50.id = "achievementEquipmentExplorer";
    pAsset50.hidden = false;
    pAsset50.icon = "ui/Icons/achievements/achievements_equipment_explorer";
    pAsset50.group = "collection";
    pAsset50.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(80 /*0x50*/, (ILibraryWithUnlockables) AssetManager.items));
    AchievementLibrary.equipment_explorer = this.add(pAsset50);
    Achievement pAsset51 = new Achievement();
    pAsset51.play_store_id = "?";
    pAsset51.steam_id = "?";
    pAsset51.id = "achievementGenesExplorer";
    pAsset51.hidden = false;
    pAsset51.icon = "ui/Icons/achievements/achievements_genes_explorer";
    pAsset51.group = "collection";
    pAsset51.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(35, (ILibraryWithUnlockables) AssetManager.gene_library));
    AchievementLibrary.genes_explorer = this.add(pAsset51);
    Achievement pAsset52 = new Achievement();
    pAsset52.play_store_id = "?";
    pAsset52.steam_id = "?";
    pAsset52.id = "achievementCreaturesExplorer";
    pAsset52.hidden = true;
    pAsset52.icon = "ui/Icons/achievements/achievements_creatures_explorer";
    pAsset52.group = "collection";
    pAsset52.action = new AchievementCheck(AchievementLibrary.checkCreaturesExplorer);
    AchievementLibrary.creatures_explorer = this.add(pAsset52);
    Achievement pAsset53 = new Achievement();
    pAsset53.play_store_id = "?";
    pAsset53.steam_id = "?";
    pAsset53.id = "achievementPlotsExplorer";
    pAsset53.hidden = false;
    pAsset53.icon = "ui/Icons/achievements/achievements_plots_explorer";
    pAsset53.group = "collection";
    pAsset53.action = (AchievementCheck) (_ => AchievementLibrary.checkUnlockAugmentations(20, (ILibraryWithUnlockables) AssetManager.plots_library));
    AchievementLibrary.plots_explorer = this.add(pAsset53);
    Achievement pAsset54 = new Achievement();
    pAsset54.play_store_id = "?";
    pAsset54.steam_id = "?";
    pAsset54.id = "achievementCursedWorld";
    pAsset54.hidden = true;
    pAsset54.icon = "ui/Icons/achievements/achievement_cursed_world";
    pAsset54.action = new AchievementCheck(AchievementLibrary.checkCursedWorld);
    pAsset54.group = "forbidden";
    AchievementLibrary.cursed_world = this.add(pAsset54);
    Achievement pAsset55 = new Achievement();
    pAsset55.play_store_id = "?";
    pAsset55.steam_id = "?";
    pAsset55.id = "achievementBoatsDisposal";
    pAsset55.hidden = false;
    pAsset55.icon = "ui/Icons/achievements/achievement_boats_disposal";
    pAsset55.group = "destruction";
    pAsset55.action = new AchievementCheck(AchievementLibrary.checkBoatDisposal);
    AchievementLibrary.boats_disposal = this.add(pAsset55);
    Achievement pAsset56 = new Achievement();
    pAsset56.play_store_id = "?";
    pAsset56.steam_id = "?";
    pAsset56.id = "achievementEngineeredEvolution";
    pAsset56.hidden = false;
    pAsset56.icon = "ui/Icons/achievements/achievement_engineered_evolution";
    pAsset56.group = "experiments";
    AchievementLibrary.engineered_evolution = this.add(pAsset56);
    Achievement pAsset57 = new Achievement();
    pAsset57.play_store_id = "?";
    pAsset57.steam_id = "?";
    pAsset57.id = "achievementSimpleStupidGenetics";
    pAsset57.hidden = false;
    pAsset57.icon = "ui/Icons/achievements/achievement_simple_stupid_genetics";
    pAsset57.group = "experiments";
    pAsset57.action = new AchievementCheck(AchievementLibrary.checkSimpleStupidGenetics);
    AchievementLibrary.simple_stupid_genetics = this.add(pAsset57);
    Achievement pAsset58 = new Achievement();
    pAsset58.play_store_id = "?";
    pAsset58.steam_id = "?";
    pAsset58.id = "achievementFastLiving";
    pAsset58.hidden = false;
    pAsset58.icon = "ui/Icons/achievements/achievement_fast_living";
    pAsset58.group = "experiments";
    pAsset58.action = new AchievementCheck(AchievementLibrary.checkFastLiving);
    AchievementLibrary.fast_living = this.add(pAsset58);
    Achievement pAsset59 = new Achievement();
    pAsset59.play_store_id = "?";
    pAsset59.steam_id = "?";
    pAsset59.id = "achievementLongLiving";
    pAsset59.hidden = false;
    pAsset59.icon = "ui/Icons/achievements/achievement_long_living";
    pAsset59.group = "experiments";
    pAsset59.action = new AchievementCheck(AchievementLibrary.checkLongLiving);
    AchievementLibrary.long_living = this.add(pAsset59);
    Achievement pAsset60 = new Achievement();
    pAsset60.play_store_id = "?";
    pAsset60.steam_id = "?";
    pAsset60.id = "achievementAncientWarOfGeometryAndEvil";
    pAsset60.hidden = false;
    pAsset60.icon = "ui/Icons/achievements/achievement_ancient_war_of_geometry_and_evil";
    pAsset60.group = "civilizations";
    pAsset60.action = new AchievementCheck(AchievementLibrary.checkAncientWarOfGeometryAndEvil);
    AchievementLibrary.ancient_war_of_geometry_and_evil = this.add(pAsset60);
    Achievement pAsset61 = new Achievement();
    pAsset61.play_store_id = "?";
    pAsset61.steam_id = "?";
    pAsset61.id = "achievementCantBeTooMuch";
    pAsset61.hidden = false;
    pAsset61.icon = "ui/Icons/achievements/achievement_cant_be_too_much";
    pAsset61.group = "creation";
    pAsset61.action = new AchievementCheck(AchievementLibrary.checkCantBeTooMuch);
    AchievementLibrary.cant_be_too_much = this.add(pAsset61);
    Achievement pAsset62 = new Achievement();
    pAsset62.play_store_id = "?";
    pAsset62.steam_id = "?";
    pAsset62.id = "achievementZoo";
    pAsset62.hidden = true;
    pAsset62.icon = "ui/Icons/achievements/achievement_zoo";
    pAsset62.group = "civilizations";
    pAsset62.action = new AchievementCheck(AchievementLibrary.checkZoo);
    AchievementLibrary.zoo = this.add(pAsset62);
    Achievement pAsset63 = new Achievement();
    pAsset63.play_store_id = "?";
    pAsset63.steam_id = "?";
    pAsset63.id = "achievementMindlessHusk";
    pAsset63.hidden = false;
    pAsset63.icon = "ui/Icons/achievements/achievement_mindless_husk";
    pAsset63.group = "experiments";
    pAsset63.action = new AchievementCheck(AchievementLibrary.checkMindlessHusk);
    AchievementLibrary.mindless_husk = this.add(pAsset63);
    Achievement pAsset64 = new Achievement();
    pAsset64.play_store_id = "?";
    pAsset64.steam_id = "?";
    pAsset64.id = "achievementMasterWeaver";
    pAsset64.hidden = false;
    pAsset64.icon = "ui/Icons/achievements/achievement_master_weaver";
    pAsset64.group = "experiments";
    pAsset64.action = new AchievementCheck(AchievementLibrary.checkMasterWeaver);
    AchievementLibrary.master_weaver = this.add(pAsset64);
    Achievement pAsset65 = new Achievement();
    pAsset65.play_store_id = "?";
    pAsset65.steam_id = "?";
    pAsset65.id = "achievementNotJustACult";
    pAsset65.hidden = false;
    pAsset65.icon = "ui/Icons/achievements/achievement_not_just_a_cult";
    pAsset65.group = "civilizations";
    pAsset65.action = new AchievementCheck(AchievementLibrary.checkNotJustACult);
    AchievementLibrary.not_just_a_cult = this.add(pAsset65);
    Achievement pAsset66 = new Achievement();
    pAsset66.play_store_id = "?";
    pAsset66.steam_id = "?";
    pAsset66.id = "achievementSuccession";
    pAsset66.hidden = false;
    pAsset66.icon = "ui/Icons/achievements/achievement_succession";
    pAsset66.group = "experiments";
    AchievementLibrary.succession = this.add(pAsset66);
    Achievement pAsset67 = new Achievement();
    pAsset67.play_store_id = "?";
    pAsset67.steam_id = "?";
    pAsset67.id = "achievementMultiplySpoken";
    pAsset67.hidden = false;
    pAsset67.icon = "ui/Icons/achievements/achievement_multiply_spoken";
    pAsset67.group = "civilizations";
    pAsset67.action = new AchievementCheck(AchievementLibrary.checkMultiplySpoken);
    AchievementLibrary.multiply_spoken = this.add(pAsset67);
    Achievement pAsset68 = new Achievement();
    pAsset68.play_store_id = "?";
    pAsset68.steam_id = "?";
    pAsset68.id = "achievementChildNamedToto";
    pAsset68.hidden = false;
    pAsset68.icon = "ui/Icons/achievements/achievement_child_named_toto";
    pAsset68.group = "experiments";
    pAsset68.action = new AchievementCheck(AchievementLibrary.checkChildNamedMakoMako);
    AchievementLibrary.child_named_toto = this.add(pAsset68);
    Achievement pAsset69 = new Achievement();
    pAsset69.play_store_id = "?";
    pAsset69.steam_id = "?";
    pAsset69.id = "achievementFlickIt";
    pAsset69.hidden = false;
    pAsset69.icon = "ui/Icons/achievements/achievement_flick_it";
    pAsset69.group = "experiments";
    AchievementLibrary.flick_it = this.add(pAsset69);
    Achievement pAsset70 = new Achievement();
    pAsset70.play_store_id = "?";
    pAsset70.steam_id = "?";
    pAsset70.id = "achievementSegregator";
    pAsset70.hidden = false;
    pAsset70.icon = "ui/Icons/achievements/achievement_segregator";
    pAsset70.group = "creation";
    pAsset70.action = new AchievementCheck(AchievementLibrary.checkSegregator);
    AchievementLibrary.segregator = this.add(pAsset70);
    Achievement pAsset71 = new Achievement();
    pAsset71.play_store_id = "?";
    pAsset71.steam_id = "?";
    pAsset71.id = "achievementEternalChaos";
    pAsset71.hidden = false;
    pAsset71.icon = "ui/Icons/achievements/achievement_eternal_chaos";
    pAsset71.group = "nature";
    pAsset71.action = new AchievementCheck(AchievementLibrary.checkEternalChaos);
    AchievementLibrary.eternal_chaos = this.add(pAsset71);
    Achievement pAsset72 = new Achievement();
    pAsset72.play_store_id = "?";
    pAsset72.steam_id = "?";
    pAsset72.id = "achievementMinefield";
    pAsset72.hidden = false;
    pAsset72.icon = "ui/Icons/achievements/achievement_minefield";
    pAsset72.group = "nature";
    pAsset72.action = new AchievementCheck(AchievementLibrary.checkMinefield);
    AchievementLibrary.minefield = this.add(pAsset72);
    Achievement pAsset73 = new Achievement();
    pAsset73.play_store_id = "?";
    pAsset73.steam_id = "?";
    pAsset73.id = "achievementGodlySmithing";
    pAsset73.hidden = false;
    pAsset73.icon = "ui/Icons/achievements/achievement_godly_smithing";
    pAsset73.group = "experiments";
    AchievementLibrary.godly_smithing = this.add(pAsset73);
    Achievement pAsset74 = new Achievement();
    pAsset74.play_store_id = "?";
    pAsset74.steam_id = "?";
    pAsset74.id = "achievementMasterOfCombat";
    pAsset74.hidden = false;
    pAsset74.icon = "ui/Icons/achievements/achievement_master_of_combat";
    pAsset74.group = "creatures";
    pAsset74.action = new AchievementCheck(AchievementLibrary.checkMasterOfCombat);
    AchievementLibrary.master_of_combat = this.add(pAsset74);
    Achievement pAsset75 = new Achievement();
    pAsset75.play_store_id = "?";
    pAsset75.steam_id = "?";
    pAsset75.id = "achievementClannibals";
    pAsset75.hidden = false;
    pAsset75.icon = "ui/Icons/achievements/achievement_clannibals";
    pAsset75.group = "creatures";
    pAsset75.action = new AchievementCheck(AchievementLibrary.checkClannibals);
    AchievementLibrary.clannibals = this.add(pAsset75);
    Achievement pAsset76 = new Achievement();
    pAsset76.play_store_id = "?";
    pAsset76.steam_id = "?";
    pAsset76.id = "achievementSocialNetwork";
    pAsset76.hidden = false;
    pAsset76.icon = "ui/Icons/achievements/achievement_social_network";
    pAsset76.group = "miscellaneous";
    AchievementLibrary.social_network = this.add(pAsset76);
    Achievement pAsset77 = new Achievement();
    pAsset77.play_store_id = "?";
    pAsset77.steam_id = "?";
    pAsset77.id = "achievementWatchYourMouth";
    pAsset77.hidden = false;
    pAsset77.icon = "ui/Icons/achievements/achievement_watch_your_mouth";
    pAsset77.group = "creatures";
    pAsset77.action = new AchievementCheck(AchievementLibrary.checkWatchYourMouth);
    AchievementLibrary.watch_your_mouth = this.add(pAsset77);
    Achievement pAsset78 = new Achievement();
    pAsset78.play_store_id = "?";
    pAsset78.steam_id = "?";
    pAsset78.id = "achievementCloneWars";
    pAsset78.hidden = false;
    pAsset78.icon = "ui/Icons/achievements/achievement_clone_wars";
    pAsset78.group = "experiments";
    pAsset78.action = new AchievementCheck(AchievementLibrary.checkCloneWars);
    AchievementLibrary.clone_wars = this.add(pAsset78);
    Achievement pAsset79 = new Achievement();
    pAsset79.play_store_id = "?";
    pAsset79.steam_id = "?";
    pAsset79.id = "achievementSmellyCity";
    pAsset79.hidden = false;
    pAsset79.icon = "ui/Icons/achievements/achievement_smelly_city";
    pAsset79.group = "civilizations";
    pAsset79.action = new AchievementCheck(AchievementLibrary.checkSmellyCity);
    AchievementLibrary.smelly_city = this.add(pAsset79);
    Achievement pAsset80 = new Achievement();
    pAsset80.play_store_id = "?";
    pAsset80.steam_id = "?";
    pAsset80.id = "achievementTLDR";
    pAsset80.locale_key = "achievement_tldr";
    pAsset80.hidden = true;
    pAsset80.icon = "ui/Icons/achievements/achievement_tldr";
    pAsset80.group = "exploration";
    AchievementLibrary.tldr = this.add(pAsset80);
    Achievement pAsset81 = new Achievement();
    pAsset81.play_store_id = "?";
    pAsset81.steam_id = "?";
    pAsset81.id = "achievementNotOnMyWatch";
    pAsset81.hidden = false;
    pAsset81.icon = "ui/Icons/achievements/achievement_not_on_my_watch";
    pAsset81.group = "civilizations";
    pAsset81.action = new AchievementCheck(AchievementLibrary.checkNotOnMyWatch);
    AchievementLibrary.not_on_my_watch = this.add(pAsset81);
    Achievement pAsset82 = new Achievement();
    pAsset82.play_store_id = "?";
    pAsset82.steam_id = "?";
    pAsset82.id = "achievementMayIInterrupt";
    pAsset82.hidden = false;
    pAsset82.icon = "ui/Icons/achievements/achievement_may_i_interrupt";
    pAsset82.group = "civilizations";
    pAsset82.action = new AchievementCheck(AchievementLibrary.checkMayIInterrupt);
    AchievementLibrary.may_i_interrupt = this.add(pAsset82);
    Achievement pAsset83 = new Achievement();
    pAsset83.play_store_id = "?";
    pAsset83.steam_id = "?";
    pAsset83.id = "achievementBallToBall";
    pAsset83.hidden = false;
    pAsset83.icon = "ui/Icons/achievements/achievement_ball_to_ball";
    pAsset83.group = "destruction";
    pAsset83.action = new AchievementCheck(AchievementLibrary.checkBallToBall);
    AchievementLibrary.ball_to_ball = this.add(pAsset83);
    Achievement pAsset84 = new Achievement();
    pAsset84.play_store_id = "?";
    pAsset84.steam_id = "?";
    pAsset84.id = "achievementSwordWithShotgun";
    pAsset84.hidden = false;
    pAsset84.icon = "ui/Icons/achievements/achievement_sword_with_shotgun";
    pAsset84.group = "creatures";
    pAsset84.action = new AchievementCheck(AchievementLibrary.checkSwordWithShotgun);
    AchievementLibrary.sword_with_shotgun = this.add(pAsset84);
    Achievement pAsset85 = new Achievement();
    pAsset85.play_store_id = "?";
    pAsset85.steam_id = "?";
    pAsset85.id = "achievementBackToBetaTesting";
    pAsset85.hidden = false;
    pAsset85.icon = "ui/Icons/achievements/achievement_back_to_beta_testing";
    pAsset85.group = "creatures";
    pAsset85.action = new AchievementCheck(AchievementLibrary.checkBackToBetaTesting);
    AchievementLibrary.back_to_beta_testing = this.add(pAsset85);
    Achievement pAsset86 = new Achievement();
    pAsset86.play_store_id = "?";
    pAsset86.steam_id = "?";
    pAsset86.id = "achievementSwarm";
    pAsset86.hidden = false;
    pAsset86.icon = "ui/Icons/achievements/achievement_swarm";
    pAsset86.group = "creatures";
    pAsset86.action = new AchievementCheck(AchievementLibrary.checkSwarm);
    AchievementLibrary.swarm = this.add(pAsset86);
    this.standaloneAchievements();
  }

  private void standaloneAchievements()
  {
    Achievement pAsset1 = new Achievement();
    pAsset1.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset1.steam_id = "1_7";
    pAsset1.id = "achievementTheBuilder";
    pAsset1.icon = "ui/Icons/achievements/achievements_thebuilder";
    pAsset1.group = "worlds";
    AchievementLibrary.the_builder = this.add(pAsset1);
    Achievement pAsset2 = new Achievement();
    pAsset2.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset2.steam_id = "1_8";
    pAsset2.id = "achievementTheDwarf";
    pAsset2.icon = "ui/Icons/achievements/achievements_thedwarf";
    pAsset2.group = "worlds";
    AchievementLibrary.the_dwarf = this.add(pAsset2);
    Achievement pAsset3 = new Achievement();
    pAsset3.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset3.steam_id = "1_9";
    pAsset3.id = "achievementTheCreator";
    pAsset3.icon = "ui/Icons/achievements/achievements_thecreator";
    pAsset3.group = "worlds";
    AchievementLibrary.the_creator = this.add(pAsset3);
    Achievement pAsset4 = new Achievement();
    pAsset4.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset4.steam_id = "1_10";
    pAsset4.id = "achievementTheLight";
    pAsset4.icon = "ui/Icons/achievements/achievements_thelight";
    pAsset4.group = "worlds";
    AchievementLibrary.the_light = this.add(pAsset4);
    Achievement pAsset5 = new Achievement();
    pAsset5.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset5.steam_id = "1_11";
    pAsset5.id = "achievementTheSky";
    pAsset5.icon = "ui/Icons/achievements/achievements_thesky";
    pAsset5.group = "worlds";
    AchievementLibrary.the_sky = this.add(pAsset5);
    Achievement pAsset6 = new Achievement();
    pAsset6.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset6.steam_id = "1_12";
    pAsset6.id = "achievementTheLand";
    pAsset6.icon = "ui/Icons/achievements/achievements_theland";
    pAsset6.group = "worlds";
    AchievementLibrary.the_land = this.add(pAsset6);
    Achievement pAsset7 = new Achievement();
    pAsset7.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset7.steam_id = "1_13";
    pAsset7.id = "achievementTheSun";
    pAsset7.icon = "ui/Icons/achievements/achievements_thesun";
    pAsset7.group = "worlds";
    AchievementLibrary.the_sun = this.add(pAsset7);
    Achievement pAsset8 = new Achievement();
    pAsset8.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset8.steam_id = "1_14";
    pAsset8.id = "achievementTheMoon";
    pAsset8.icon = "ui/Icons/achievements/achievements_themoon";
    pAsset8.group = "worlds";
    AchievementLibrary.the_moon = this.add(pAsset8);
    Achievement pAsset9 = new Achievement();
    pAsset9.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset9.steam_id = "1_15";
    pAsset9.id = "achievementTheLiving";
    pAsset9.icon = "ui/Icons/achievements/achievements_theliving";
    pAsset9.group = "worlds";
    AchievementLibrary.the_living = this.add(pAsset9);
    Achievement pAsset10 = new Achievement();
    pAsset10.play_store_id = "CgkIia6M98wfEAIQAg";
    pAsset10.steam_id = "1_16";
    pAsset10.id = "achievementTheRestDay";
    pAsset10.icon = "ui/Icons/achievements/achievements_restday";
    pAsset10.group = "worlds";
    AchievementLibrary.the_rest_day = this.add(pAsset10);
    WorkshopAchievements.checkAchievements();
  }

  public override void post_init()
  {
    base.post_init();
    foreach (Achievement achievement1 in this.list)
    {
      Achievement achievement2 = achievement1;
      if (achievement2.locale_key == null)
        achievement2.locale_key = achievement1.id.Underscore();
      if (achievement1.getSignal() != null)
        achievement1.has_signal = true;
    }
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (Achievement pAchievement in this.list)
    {
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.traits);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.subspecies_traits);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.culture_traits);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.language_traits);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.clan_traits);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.religion_traits);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.kingdoms_traits);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.gene_library);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.items);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.actor_library);
      this.addAsUnlockAssets(pAchievement, (ILibraryWithUnlockables) AssetManager.plots_library);
    }
  }

  private void addAsUnlockAssets(Achievement pAchievement, ILibraryWithUnlockables pLibrary)
  {
    foreach (BaseUnlockableAsset elements in pLibrary.elements_list)
    {
      if (elements.unlocked_with_achievement && !(elements.achievement_id != pAchievement.id))
      {
        if (pAchievement.unlock_assets == null)
        {
          pAchievement.unlock_assets = new List<BaseUnlockableAsset>();
          pAchievement.unlocks_something = true;
        }
        pAchievement.unlock_assets.Add(elements);
      }
    }
  }

  public static void unlock(string pID)
  {
    Achievement pAchievement = AssetManager.achievements.get(pID);
    if (pAchievement == null)
      return;
    AchievementLibrary.unlock(pAchievement);
  }

  public static void unlock(Achievement pAchievement)
  {
    if (WorldLawLibrary.world_law_cursed_world.isEnabled())
      return;
    SteamAchievements.TriggerAchievement(pAchievement.id);
    if (AchievementLibrary.isUnlocked(pAchievement))
      return;
    if (GameProgress.unlockAchievement(pAchievement.id))
      AchievementPopup.show(pAchievement);
    Analytics.LogEvent("Achievement", "id", pAchievement.id);
    MapBox.aye();
  }

  public static bool isUnlocked(Achievement pAchievement)
  {
    return GameProgress.isAchievementUnlocked(pAchievement.id);
  }

  public static bool isUnlocked(string pID) => GameProgress.isAchievementUnlocked(pID);

  private static bool checkTraitsExplorer(int pAmount)
  {
    int num = 0;
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.traits.list)
    {
      if (baseUnlockableAsset.isAvailable())
        ++num;
    }
    return num >= pAmount;
  }

  private static bool checkUnlockAugmentations(int pAmount, ILibraryWithUnlockables pLibrary)
  {
    int num = 0;
    foreach (BaseUnlockableAsset elements in pLibrary.elements_list)
    {
      if (elements.isAvailable())
        ++num;
    }
    return num >= pAmount;
  }

  private static bool checkAntWorld(object pCheckData = null)
  {
    List<Actor> units = World.world.kingdoms_wild.get("ants").units;
    if (units.Count < 40)
      return false;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    for (int index = 0; index < units.Count; ++index)
    {
      switch (units[index].asset.id)
      {
        case "ant_black":
          ++num1;
          break;
        case "ant_blue":
          ++num4;
          break;
        case "ant_red":
          ++num3;
          break;
        case "ant_green":
          ++num2;
          break;
      }
    }
    return num4 >= 10 && num2 >= 10 && num3 >= 10 && num1 >= 10;
  }

  private static bool checkCursedWorld(object pCheckData = null)
  {
    return WorldLawLibrary.world_law_cursed_world.isEnabledRaw();
  }

  private static bool checkBoatDisposal(object pCheckData = null)
  {
    return StatsHelper.getStat("statistics_boats_destroyed_by_magnet") >= 10L;
  }

  private static bool check10000Creatures(object pCheckData = null)
  {
    return World.world.game_stats.data.creaturesCreated >= 10000L;
  }

  private static bool checkManyBombs(object pCheckData = null)
  {
    return World.world.game_stats.data.bombsDropped >= 1000L;
  }

  private static bool checkMegapolis(object pCheckData = null)
  {
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (!(city.getSpecies() != "human") && city.getPopulationPeople() >= 200)
        return true;
    }
    return false;
  }

  private static bool check4RaceCities(object pCheckData = null)
  {
    if (World.world.cities.Count < 4)
      return false;
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      switch (city.getSpecies())
      {
        case "human":
          flag1 = true;
          continue;
        case "orc":
          flag2 = true;
          continue;
        case "elf":
          flag3 = true;
          continue;
        case "dwarf":
          flag4 = true;
          continue;
        default:
          continue;
      }
    }
    return flag1 & flag2 & flag3 & flag4;
  }

  private static bool checkPiranhaLand(object pCheckData = null)
  {
    Actor actor = (Actor) pCheckData;
    return !(actor.asset.id != "piranha") && actor.mustAvoidGround() && !actor.current_tile.Type.liquid;
  }

  private static bool checkPrintHeart(object pCheckData = null)
  {
    return ((GodPower) pCheckData).printers_print == "heart";
  }

  internal static void checkSteamMapUploads()
  {
    long workshopUploads = World.world.game_stats.data.workshopUploads;
    if (workshopUploads >= 1L)
      AchievementLibrary.the_builder.check();
    if (workshopUploads >= 3L)
      AchievementLibrary.the_dwarf.check();
    if (workshopUploads < 5L)
      return;
    AchievementLibrary.the_creator.check();
  }

  internal static void checkSteamMapDownloads(int pDownloads)
  {
    if (pDownloads >= 1)
      AchievementLibrary.the_light.check();
    if (pDownloads >= 2)
      AchievementLibrary.the_sky.check();
    if (pDownloads >= 3)
      AchievementLibrary.the_land.check();
    if (pDownloads >= 4)
      AchievementLibrary.the_sun.check();
    if (pDownloads >= 5)
      AchievementLibrary.the_moon.check();
    if (pDownloads >= 6)
      AchievementLibrary.the_living.check();
    if (pDownloads < 7)
      return;
    AchievementLibrary.the_rest_day.check();
  }

  private static bool checkLifeIsASim(object pCheckData = null)
  {
    return (double) Mathf.Ceil(Time.realtimeSinceStartup) / 3600.0 > 24.0;
  }

  private static bool checkTheDemon(object pCheckData = null)
  {
    return SelectedUnit.isSet() && !SelectedUnit.unit.hasDivineScar() && !(SelectedUnit.unit.asset.id != "demon") && SelectedUnit.unit.countTraits() >= 10;
  }

  private static bool checkTheKing(object pCheckData = null)
  {
    return SelectedUnit.isSet() && !SelectedUnit.unit.hasDivineScar() && SelectedUnit.unit.isKing() && SelectedUnit.unit.countTraits() >= 20;
  }

  private static bool checkTheAccomplished(object pCheckData = null)
  {
    return SelectedUnit.isSet() && !SelectedUnit.unit.hasDivineScar() && SelectedUnit.unit.hasTrait("veteran") && SelectedUnit.unit.hasTrait("mageslayer") && SelectedUnit.unit.hasTrait("dragonslayer") && SelectedUnit.unit.hasTrait("kingslayer");
  }

  private static bool checkTheBroken(object pCheckData = null)
  {
    return SelectedUnit.isSet() && !SelectedUnit.unit.hasDivineScar() && SelectedUnit.unit.hasTrait("crippled") && SelectedUnit.unit.hasTrait("eyepatch") && SelectedUnit.unit.hasTrait("skin_burns");
  }

  private static bool checkMapCreations100(object pCheckData = null)
  {
    return World.world.game_stats.data.mapsCreated >= 100L;
  }

  private static bool checkMapCreations50(object pCheckData = null)
  {
    return World.world.game_stats.data.mapsCreated >= 50L;
  }

  private static bool checkMapCreations5(object pCheckData = null)
  {
    return World.world.game_stats.data.mapsCreated >= 5L;
  }

  private static bool checkTheHell(object pCheckData = null)
  {
    float num = (float) (TopTileLibrary.infernal_high.hashset.Count + TopTileLibrary.infernal_low.hashset.Count);
    if ((double) num == 0.0)
      return false;
    float length = (float) World.world.tiles_list.Length;
    return (double) num / (double) length >= 0.66600000858306885 && World.world.kingdoms_wild.get("demon").units.Count >= 66;
  }

  private static bool checkLetsNot(object pCheckData = null)
  {
    return (double) (TopTileLibrary.wasteland_high.hashset.Count + TopTileLibrary.wasteland_low.hashset.Count) / (double) World.world.tiles_list.Length >= 0.89999997615814209;
  }

  private static bool checkWorldWar(object pCheckData = null)
  {
    int num = 0;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.hasEnemies())
      {
        ++num;
        if (num >= 10)
          return true;
      }
    }
    return false;
  }

  private static bool checkPlanetOfTheApes(object pCheckData = null)
  {
    double num1 = (double) (TopTileLibrary.wasteland_high.hashset.Count + TopTileLibrary.wasteland_low.hashset.Count);
    float num2 = (float) (TopTileLibrary.jungle_high.hashset.Count + TopTileLibrary.jungle_low.hashset.Count);
    float length = (float) World.world.tiles_list.Length;
    double num3 = (double) length;
    float num4 = (float) (num1 / num3) + num2 / length;
    return World.world.kingdoms_wild.get("monkey").units.Count >= 100 && (double) num4 > 0.800000011920929;
  }

  private static bool checkGreatPlague(object pCheckData = null)
  {
    return World.world.map_stats.current_infected_plague >= 1000L;
  }

  private static bool checkRainTornado(object pCheckData = null)
  {
    return !(World.world.map_stats.world_age_id != "age_tears") && World.world.stack_effects.get("fx_tornado").getList().Count >= 100;
  }

  private static bool checkSimpleStupidGenetics(object pCheckData = null)
  {
    Subspecies selectedSubspecies = SelectedMetas.selected_subspecies;
    if (selectedSubspecies == null)
      return false;
    List<Chromosome> chromosomes = selectedSubspecies.nucleus.chromosomes;
    GeneAsset geneAsset = (GeneAsset) null;
    foreach (Chromosome chromosome in chromosomes)
    {
      foreach (GeneAsset gene in chromosome.genes)
      {
        if (!gene.is_empty)
        {
          if (geneAsset == null)
            geneAsset = gene;
          if (geneAsset != gene)
            return false;
        }
      }
    }
    return true;
  }

  private static bool checkFastLiving(object pCheckData = null)
  {
    Subspecies selectedSubspecies = SelectedMetas.selected_subspecies;
    if (selectedSubspecies == null)
      return false;
    int num1 = 2;
    float baseStat = selectedSubspecies.base_stats["lifespan"];
    float num2 = selectedSubspecies.base_stats["lifespan"] + baseStat;
    float num3 = selectedSubspecies.base_stats_female["lifespan"] + baseStat;
    return (double) num2 <= (double) num1 && (double) num3 <= (double) num1;
  }

  private static bool checkLongLiving(object pCheckData = null)
  {
    Subspecies selectedSubspecies = SelectedMetas.selected_subspecies;
    if (selectedSubspecies == null)
      return false;
    int num1 = 3000;
    float baseStat = selectedSubspecies.base_stats["lifespan"];
    float num2 = selectedSubspecies.base_stats_male["lifespan"] + baseStat;
    float num3 = selectedSubspecies.base_stats_female["lifespan"] + baseStat;
    return (double) num2 >= (double) num1 && (double) num3 >= (double) num1;
  }

  private static bool checkAncientWarOfGeometryAndEvil(object pCheckData = null)
  {
    foreach (War war in (CoreSystemManager<War, WarData>) World.world.wars)
    {
      if (!war.hasEnded())
      {
        bool flag1 = false;
        bool flag2 = false;
        foreach (Kingdom attacker in war.getAttackers())
        {
          if (attacker.getSpecies() == "angle")
            flag1 = true;
          if (attacker.getSpecies() == "demon")
            flag2 = true;
        }
        foreach (Kingdom defender in war.getDefenders())
        {
          if (defender.getSpecies() == "angle")
            flag1 = true;
          if (defender.getSpecies() == "demon")
            flag2 = true;
        }
        if (flag1 & flag2)
          return true;
      }
    }
    return false;
  }

  private static bool checkCantBeTooMuch(object pCheckData = null)
  {
    int num1 = 10;
    int num2 = 0;
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (!(building.asset.id != "monolith"))
        ++num2;
    }
    return num2 >= num1;
  }

  private static bool checkZoo(object pCheckData = null)
  {
    using (ListPool<string> listPool = new ListPool<string>())
    {
      using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          City current = enumerator.Current;
          listPool.Clear();
          foreach (Actor unit in current.units)
          {
            string id = unit.asset.id;
            if (!listPool.Contains(id))
              listPool.Add(id);
          }
          return listPool.Count >= 33;
        }
      }
      return false;
    }
  }

  private static bool checkMindlessHusk(object pCheckData = null)
  {
    Actor actor = (Actor) pCheckData;
    UtilityBasedDecisionSystem decisionSystem = DecisionHelper.decision_system;
    int counter = decisionSystem.getCounter();
    DecisionAsset[] actions = decisionSystem.getActions();
    for (int index = 0; index < counter; ++index)
    {
      DecisionAsset decisionAsset = actions[index];
      if (actor.isDecisionEnabled(decisionAsset.decision_index))
        return false;
    }
    return true;
  }

  private static bool checkMasterWeaver(object pCheckData = null)
  {
    Subspecies selectedSubspecies = SelectedMetas.selected_subspecies;
    if (selectedSubspecies == null || selectedSubspecies.getActorAsset().id != "butterfly")
      return false;
    foreach (Chromosome chromosome in selectedSubspecies.nucleus.chromosomes)
    {
      if (!chromosome.isAllLociSynergy())
        return false;
    }
    return true;
  }

  private static bool checkNotJustACult(object pCheckData = null)
  {
    return ((MetaObject<ReligionData>) pCheckData).countUnits() >= 7777;
  }

  private static bool checkMultiplySpoken(object pCheckData = null)
  {
    return ((MetaObject<LanguageData>) pCheckData).countUnits() >= 5555;
  }

  private static bool checkChildNamedMakoMako(object pCheckData = null)
  {
    return !((string) pCheckData != "Mako Mako");
  }

  private static bool checkSegregator(object pCheckData = null)
  {
    return World.world.game_stats.data.wallsPlaced >= 10000L;
  }

  private static bool checkEternalChaos(object pCheckData = null)
  {
    return !(World.world_era.id != "age_chaos") && (double) Date.getYearsSince(World.world.map_stats.same_world_age_started_at) >= 1000.0;
  }

  private static bool checkMinefield(object pCheckData = null)
  {
    return WorldLawLibrary.world_law_exploding_mushrooms.isEnabled() && (double) Date.getYearsSince(World.world.map_stats.exploding_mushrooms_enabled_at) >= 1000.0;
  }

  private static bool checkWatchYourMouth(object pCheckData = null)
  {
    Actor actor = (Actor) pCheckData;
    return actor.isBaby() && actor.hasStatus("swearing");
  }

  private static bool checkCloneWars(object pCheckData = null)
  {
    (Actor pActor1, Actor pActor2) = ((Actor, Actor)) pCheckData;
    return pActor1.isAlive() && pActor2.isAlive() && (pActor1.isSameClones(pActor2) || pActor1.isClonedFrom(pActor2) ? 1 : (pActor2.isClonedFrom(pActor1) ? 1 : 0)) != 0;
  }

  private static bool checkCreaturesExplorer(object pCheckData = null)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (ActorAsset actorAsset in AssetManager.actor_library.list)
    {
      if (actorAsset.needs_to_be_explored && !actorAsset.isTemplateAsset())
      {
        ++num1;
        if (actorAsset.isAvailable())
          ++num2;
      }
    }
    return num2 >= 52;
  }

  private static bool checkMasterOfCombat(object pCheckData = null)
  {
    Actor actor = (Actor) pCheckData;
    if (actor == null || !actor.isAlive() || actor.hasDivineScar())
      return false;
    int num = 0;
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) actor.getTraits())
    {
      if (trait.in_training_dummy_combat_pot)
        ++num;
    }
    return num >= 5;
  }

  private static bool checkClannibals(object pCheckData = null)
  {
    (Actor actor, Clan clan) = ((Actor, Clan)) pCheckData;
    return actor.hasClan() && clan != null && actor.clan.id == clan.id;
  }

  private static bool checkSmellyCity(object pCheckData = null)
  {
    return ((City) pCheckData).getResourcesAmount("fertilizer") >= 999;
  }

  private static bool checkNotOnMyWatch(object pCheckData = null)
  {
    return ((BaseSimObject) pCheckData).hasStatus("being_suspicious");
  }

  private static bool checkMayIInterrupt(object pCheckData = null)
  {
    return !((string) pCheckData != "socialize_do_talk");
  }

  private static bool checkBallToBall(object pCheckData = null)
  {
    return !(((Actor) pCheckData).asset.id != "armadillo");
  }

  private static bool checkSwordWithShotgun(object pCheckData = null)
  {
    Actor actor = (Actor) pCheckData;
    if (actor.asset.id != "crystal_sword" || !actor.hasWeapon())
      return false;
    Item weapon = actor.getWeapon();
    return !(weapon.getAsset().id != "shotgun") && !weapon.hasMod("divine_rune");
  }

  private static bool checkBackToBetaTesting(object pCheckData = null)
  {
    Actor actor = (Actor) pCheckData;
    return !(actor.asset.id != "beetle") && !(actor.current_tile.Type.biome_id != "biome_singularity");
  }

  private static bool checkSwarm(object pCheckData = null)
  {
    Subspecies subspecies = (Subspecies) pCheckData;
    return !subspecies.hasPopulationLimit() && subspecies.hasTrait("high_fecundity") && subspecies.hasTrait("reproduction_spores") && subspecies.hasTrait("gestation_short") && subspecies.hasTrait("rapid_aging");
  }

  public static void checkCityAchievements(City pCity)
  {
    AchievementLibrary.zoo.checkBySignal();
    AchievementLibrary.four_race_cities.check((object) pCity);
    AchievementLibrary.smelly_city.check((object) pCity);
    AchievementLibrary.megapolis.checkBySignal();
  }

  public static void checkUnitAchievements(Actor pActor)
  {
    AchievementLibrary.creatures_explorer.check();
    AchievementLibrary.the_broken.check();
    AchievementLibrary.the_demon.check();
    AchievementLibrary.the_king.check();
    AchievementLibrary.the_accomplished.check();
    AchievementLibrary.watch_your_mouth.check((object) pActor);
    AchievementLibrary.master_of_combat.check((object) pActor);
    AchievementLibrary.sword_with_shotgun.check((object) pActor);
    AchievementLibrary.ninja_turtle.check((object) pActor);
  }

  public static void checkSubspeciesAchievements(Subspecies pSubspecies)
  {
    AchievementLibrary.creatures_explorer.check();
    AchievementLibrary.swarm.checkBySignal((object) pSubspecies);
  }

  public static void login()
  {
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (Achievement pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }

  public static int countUnlocked()
  {
    int num = 0;
    foreach (Asset asset in AssetManager.achievements.list)
    {
      if (AchievementLibrary.isUnlocked(asset.id))
        ++num;
    }
    return num;
  }

  public static bool isAllUnlocked()
  {
    return AchievementLibrary.countUnlocked() >= AssetManager.achievements.list.Count;
  }
}
