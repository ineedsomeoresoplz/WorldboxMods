// Decompiled with JetBrains decompiler
// Type: DecisionsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class DecisionsLibrary : AssetLibrary<DecisionAsset>
{
  public DecisionAsset[] list_only_civ;
  public DecisionAsset[] list_only_children;
  public DecisionAsset[] list_only_city;
  public DecisionAsset[] list_only_animal;
  public DecisionAsset[] list_others;

  public override void init()
  {
    base.init();
    this.initDecisionsGeneral();
    this.initDecisionsTraits();
    this.initDecisionsChildren();
    this.initDecisionsAnimals();
    this.initDecisionDiets();
    this.initDecisionSleep();
    this.initDecisionsHerd();
    this.initDecisionsCivs();
    this.initDecisionsKings();
    this.initDecisionsWarriors();
    this.initDecisionsLeaders();
    this.initDecisionsBoats();
    this.initDecisionsBees();
    this.initDecisionsOther();
    this.initDecisionsUnique();
    this.initDecisionsSocialize();
    this.initDecisionsReproduction();
    this.initDecisionsClans();
    this.initDecisionsNomads();
    this.initDecisionsStatusRelated();
  }

  private void initDecisionsWarriors()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "warrior_try_join_army_group";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.path_icon = "ui/Icons/iconSoldier";
    pAsset1.cooldown = 5;
    pAsset1.unique = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && !pActor.hasArmy());
    pAsset1.weight = 3f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "check_warrior_limit";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconArmyList";
    pAsset2.cooldown = 60;
    pAsset2.unique = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.inOwnCityBorders());
    pAsset2.weight = 0.7f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "city_walking_to_danger_zone";
    pAsset3.priority = NeuroLayer.Layer_3_High;
    pAsset3.path_icon = "ui/Icons/iconArrowAttackTarget";
    pAsset3.cooldown = 5;
    pAsset3.unique = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.city.isInDanger() && pActor.inOwnCityBorders());
    pAsset3.weight = 2.7f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "warrior_army_captain_idle_walking_city";
    pAsset4.priority = NeuroLayer.Layer_2_Moderate;
    pAsset4.path_icon = "ui/Icons/iconArmyList";
    pAsset4.cooldown = 20;
    pAsset4.unique = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.isArmyGroupLeader() && !pActor.city.hasAttackZoneOrder());
    pAsset4.weight = 1.3f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "warrior_army_captain_waiting";
    pAsset5.priority = NeuroLayer.Layer_2_Moderate;
    pAsset5.path_icon = "ui/Icons/iconClock";
    pAsset5.cooldown = 20;
    pAsset5.unique = true;
    pAsset5.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.isArmyGroupLeader() && !pActor.city.hasAttackZoneOrder());
    pAsset5.weight = 1.5f;
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "warrior_army_leader_move_random";
    pAsset6.priority = NeuroLayer.Layer_2_Moderate;
    pAsset6.path_icon = "ui/Icons/iconArrowDestination";
    pAsset6.cooldown = 1;
    pAsset6.unique = true;
    pAsset6.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.isArmyGroupLeader());
    pAsset6.weight = 1.5f;
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "warrior_army_leader_move_to_attack_target";
    pAsset7.priority = NeuroLayer.Layer_3_High;
    pAsset7.path_icon = "ui/Icons/iconArrowAttackTarget";
    pAsset7.cooldown = 1;
    pAsset7.unique = true;
    pAsset7.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.isArmyGroupLeader() && pActor.city.hasAttackZoneOrder());
    pAsset7.weight = 2f;
    this.add(pAsset7);
    DecisionAsset pAsset8 = new DecisionAsset();
    pAsset8.id = "warrior_army_follow_leader";
    pAsset8.priority = NeuroLayer.Layer_3_High;
    pAsset8.path_icon = "ui/Icons/iconLoyalty";
    pAsset8.cooldown = 1;
    pAsset8.unique = true;
    pAsset8.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.hasCity() || !pActor.isArmyGroupWarrior() || !pActor.army.hasCaptain())
        return false;
      WorldTile currentTile = pActor.army.getCaptain().current_tile;
      return pActor.current_tile.isSameIsland(currentTile) && pActor.city.hasAttackZoneOrder();
    });
    pAsset8.weight = 5f;
    this.add(pAsset8);
    DecisionAsset pAsset9 = new DecisionAsset();
    pAsset9.id = "warrior_random_move";
    pAsset9.priority = NeuroLayer.Layer_1_Low;
    pAsset9.path_icon = "ui/Icons/iconArrowDestination";
    pAsset9.cooldown = 4;
    pAsset9.unique = true;
    pAsset9.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.hasCity() || !pActor.isArmyGroupWarrior())
        return false;
      if (!pActor.army.hasCaptain())
        return true;
      WorldTile currentTile = pActor.army.getCaptain().current_tile;
      return !pActor.current_tile.isSameIsland(currentTile);
    });
    pAsset9.weight = 1.6f;
    this.add(pAsset9);
    DecisionAsset pAsset10 = new DecisionAsset();
    pAsset10.id = "check_warrior_transport";
    pAsset10.priority = NeuroLayer.Layer_2_Moderate;
    pAsset10.path_icon = "ui/Icons/iconBoat";
    pAsset10.cooldown = 6;
    pAsset10.unique = true;
    pAsset10.weight = 2f;
    this.add(pAsset10);
    DecisionAsset pAsset11 = new DecisionAsset();
    pAsset11.id = "warrior_train_with_dummy";
    pAsset11.priority = NeuroLayer.Layer_2_Moderate;
    pAsset11.path_icon = "ui/Icons/iconWarfare";
    pAsset11.cooldown = 100;
    pAsset11.unique = true;
    pAsset11.city_must_be_safe = true;
    pAsset11.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.inOwnCityBorders() && pActor.city.hasBuildingType("type_training_dummies", pLimitIsland: pActor.current_island));
    pAsset11.weight = 1.1f;
    this.add(pAsset11);
  }

  private void initDecisionsStatusRelated()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "check_swearing";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.task_id = "swearing";
    pAsset1.path_icon = "ui/Icons/iconSwearing";
    pAsset1.cooldown_on_launch_failure = true;
    pAsset1.cooldown = 60;
    pAsset1.unique = true;
    pAsset1.action_check_launch = (DecisionAction) (_ => Randy.randomChance(0.1f));
    pAsset1.weight = 0.1f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "do_tantrum";
    pAsset2.priority = NeuroLayer.Layer_4_Critical;
    pAsset2.path_icon = "ui/Icons/iconTantrum";
    pAsset2.cooldown = 1;
    pAsset2.unique = true;
    pAsset2.weight = 3f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "possessed_following";
    pAsset3.priority = NeuroLayer.Layer_4_Critical;
    pAsset3.path_icon = "ui/Icons/iconPossessed";
    pAsset3.cooldown = 1;
    pAsset3.unique = true;
    pAsset3.weight = 3f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "status_confused";
    pAsset4.priority = NeuroLayer.Layer_4_Critical;
    pAsset4.path_icon = "ui/Icons/iconConfused";
    pAsset4.cooldown = 1;
    pAsset4.unique = true;
    pAsset4.weight = 3.5f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "run_to_water_when_on_fire";
    pAsset5.priority = NeuroLayer.Layer_4_Critical;
    pAsset5.path_icon = "ui/Icons/iconFire";
    pAsset5.cooldown = 1;
    pAsset5.unique = true;
    pAsset5.action_check_launch = (DecisionAction) (pActor => pActor.asset.run_to_water_when_on_fire);
    pAsset5.weight = 5f;
    this.add(pAsset5);
  }

  private void initDecisionsNomads()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "try_to_start_new_civilization";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.path_icon = "ui/Icons/iconKingdom";
    pAsset1.cooldown_on_launch_failure = true;
    pAsset1.cooldown = 30;
    pAsset1.unique = true;
    pAsset1.only_sapient = true;
    pAsset1.only_safe = true;
    pAsset1.only_adult = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor =>
    {
      if (pActor.isKing() || pActor.hasCity())
        return false;
      if (pActor.current_zone.hasCity())
      {
        if (!pActor.current_zone.city.isNeutral())
          return false;
      }
      else if (!pActor.canBuildNewCity())
        return false;
      return true;
    });
    pAsset1.weight = 1f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "check_join_city";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconCity";
    pAsset2.cooldown = 10;
    pAsset2.unique = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor => !pActor.hasCity() && !pActor.kingdom.asset.is_forced_by_trait && pActor.current_zone.hasCity());
    pAsset2.weight = 1f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "check_join_empty_nearby_city";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconCity";
    pAsset3.cooldown = 10;
    pAsset3.unique = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => !pActor.hasCity() && !pActor.kingdom.asset.is_forced_by_trait);
    pAsset3.weight = 1f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "build_civ_city_here";
    pAsset4.priority = NeuroLayer.Layer_2_Moderate;
    pAsset4.path_icon = "ui/Icons/iconCity";
    pAsset4.cooldown = 60;
    pAsset4.only_adult = true;
    pAsset4.unique = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor => pActor.isKingdomCiv() && !pActor.hasCity() && !Finder.isEnemyNearOnSameIsland(pActor));
    pAsset4.weight = 1f;
    this.add(pAsset4);
  }

  private void initDecisionsHerd()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "family_check_existence";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.path_icon = "ui/Icons/iconFamily";
    pAsset1.cooldown = 60;
    pAsset1.unique = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor => pActor.family.countUnits() <= 1);
    pAsset1.weight = 2f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "family_alpha_move";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconFamily";
    pAsset2.cooldown = 200;
    pAsset2.unique = true;
    pAsset2.only_herd = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor => !pActor.isSapient() && pActor.family.isAlpha(pActor));
    pAsset2.weight = 0.8f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "family_group_follow";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconFamily";
    pAsset3.cooldown = 5;
    pAsset3.unique = true;
    pAsset3.only_herd = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => !pActor.isSapient() && !pActor.family.isAlpha(pActor));
    pAsset3.weight = 0.7f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "family_group_leave";
    pAsset4.priority = NeuroLayer.Layer_1_Low;
    pAsset4.path_icon = "ui/Icons/iconFamily";
    pAsset4.cooldown = 20;
    pAsset4.unique = true;
    pAsset4.only_herd = true;
    pAsset4.only_adult = true;
    pAsset4.only_safe = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor => !pActor.isSapient() && pActor.family.isFull() && !pActor.family.isAlpha(pActor));
    pAsset4.weight = 0.5f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "family_group_join_or_new_herd";
    pAsset5.priority = NeuroLayer.Layer_3_High;
    pAsset5.path_icon = "ui/Icons/iconFamily";
    pAsset5.cooldown = 60;
    pAsset5.only_herd = true;
    pAsset5.action_check_launch = (DecisionAction) (pActor => !pActor.isSapient() && !pActor.hasFamily());
    pAsset5.weight = 0.3f;
    this.add(pAsset5);
  }

  private void initDecisionSleep()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "bored_sleep";
    pAsset1.priority = NeuroLayer.Layer_0_Minimal;
    pAsset1.task_id = "decide_where_to_sleep";
    pAsset1.path_icon = "ui/Icons/iconSleep";
    pAsset1.cooldown = 90;
    pAsset1.only_safe = true;
    pAsset1.city_must_be_safe = true;
    pAsset1.weight = 0.05f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "polyphasic_sleep";
    pAsset2.priority = NeuroLayer.Layer_1_Low;
    pAsset2.task_id = "decide_where_to_sleep";
    pAsset2.path_icon = "ui/Icons/iconSleep";
    pAsset2.unique = true;
    pAsset2.cooldown = 90;
    pAsset2.only_safe = true;
    pAsset2.city_must_be_safe = true;
    pAsset2.weight = 0.8f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "monophasic_sleep";
    pAsset3.priority = NeuroLayer.Layer_1_Low;
    pAsset3.task_id = "decide_where_to_sleep";
    pAsset3.path_icon = "ui/Icons/iconSleep";
    pAsset3.unique = true;
    pAsset3.cooldown = 200;
    pAsset3.only_safe = true;
    pAsset3.city_must_be_safe = true;
    pAsset3.weight = 0.75f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "sleep_at_winter_age";
    pAsset4.priority = NeuroLayer.Layer_2_Moderate;
    pAsset4.task_id = "decide_where_to_sleep";
    pAsset4.path_icon = "ui/Icons/iconSleep";
    pAsset4.unique = true;
    pAsset4.cooldown = 30;
    pAsset4.only_safe = true;
    pAsset4.city_must_be_safe = true;
    pAsset4.action_check_launch = (DecisionAction) (_ => World.world.era_manager.isWinter());
    pAsset4.weight = 1f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "sleep_at_dark_age";
    pAsset5.priority = NeuroLayer.Layer_2_Moderate;
    pAsset5.task_id = "decide_where_to_sleep";
    pAsset5.path_icon = "ui/Icons/iconSleep";
    pAsset5.unique = true;
    pAsset5.cooldown = 30;
    pAsset5.only_safe = true;
    pAsset5.city_must_be_safe = true;
    pAsset5.action_check_launch = (DecisionAction) (_ => World.world.era_manager.isNight());
    pAsset5.weight = 1f;
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "sleep_when_not_chaos_age";
    pAsset6.priority = NeuroLayer.Layer_2_Moderate;
    pAsset6.task_id = "decide_where_to_sleep";
    pAsset6.path_icon = "ui/Icons/iconSleep";
    pAsset6.unique = true;
    pAsset6.cooldown = 30;
    pAsset6.only_safe = true;
    pAsset6.city_must_be_safe = true;
    pAsset6.action_check_launch = (DecisionAction) (_ => !World.world.era_manager.isChaosAge());
    pAsset6.weight = 1f;
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "sleep_at_light_age";
    pAsset7.priority = NeuroLayer.Layer_2_Moderate;
    pAsset7.task_id = "decide_where_to_sleep";
    pAsset7.path_icon = "ui/Icons/iconSleep";
    pAsset7.unique = true;
    pAsset7.cooldown = 30;
    pAsset7.only_safe = true;
    pAsset7.city_must_be_safe = true;
    pAsset7.action_check_launch = (DecisionAction) (_ => World.world.era_manager.isLightAge());
    pAsset7.weight = 1f;
    this.add(pAsset7);
  }

  private void initDecisionDiets()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "diet_wood";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_xylophagy";
    pAsset1.unique = true;
    pAsset1.cooldown = 30;
    pAsset1.only_safe = true;
    pAsset1.only_hungry = true;
    pAsset1.weight = 1f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "diet_tiles";
    pAsset2.priority = NeuroLayer.Layer_3_High;
    pAsset2.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_geophagy";
    pAsset2.unique = true;
    pAsset2.cooldown = 10;
    pAsset2.only_safe = true;
    pAsset2.only_hungry = true;
    pAsset2.weight = 1f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "diet_minerals";
    pAsset3.priority = NeuroLayer.Layer_3_High;
    pAsset3.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_lithotroph";
    pAsset3.unique = true;
    pAsset3.cooldown = 10;
    pAsset3.only_safe = true;
    pAsset3.only_hungry = true;
    pAsset3.weight = 1f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "diet_algae";
    pAsset4.priority = NeuroLayer.Layer_3_High;
    pAsset4.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_algivore";
    pAsset4.unique = true;
    pAsset4.cooldown = 10;
    pAsset4.only_safe = true;
    pAsset4.only_hungry = true;
    pAsset4.weight = 1f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "diet_fish";
    pAsset5.priority = NeuroLayer.Layer_3_High;
    pAsset5.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_piscivore";
    pAsset5.unique = true;
    pAsset5.cooldown = 10;
    pAsset5.only_safe = true;
    pAsset5.only_hungry = true;
    pAsset5.weight = 1f;
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "diet_fruits";
    pAsset6.priority = NeuroLayer.Layer_3_High;
    pAsset6.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_frugivore";
    pAsset6.unique = true;
    pAsset6.cooldown = 10;
    pAsset6.only_safe = true;
    pAsset6.only_hungry = true;
    pAsset6.weight = 1f;
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "diet_flowers";
    pAsset7.priority = NeuroLayer.Layer_3_High;
    pAsset7.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_florivore";
    pAsset7.unique = true;
    pAsset7.cooldown = 10;
    pAsset7.only_safe = true;
    pAsset7.only_hungry = true;
    pAsset7.weight = 1f;
    this.add(pAsset7);
    DecisionAsset pAsset8 = new DecisionAsset();
    pAsset8.id = "diet_nectar";
    pAsset8.priority = NeuroLayer.Layer_3_High;
    pAsset8.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_nectarivore";
    pAsset8.unique = true;
    pAsset8.cooldown = 10;
    pAsset8.only_safe = true;
    pAsset8.only_hungry = true;
    pAsset8.weight = 1f;
    this.add(pAsset8);
    DecisionAsset pAsset9 = new DecisionAsset();
    pAsset9.id = "diet_crops";
    pAsset9.priority = NeuroLayer.Layer_3_High;
    pAsset9.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_granivore";
    pAsset9.unique = true;
    pAsset9.cooldown = 10;
    pAsset9.only_safe = true;
    pAsset9.only_hungry = true;
    pAsset9.weight = 1f;
    this.add(pAsset9);
    DecisionAsset pAsset10 = new DecisionAsset();
    pAsset10.id = "diet_vegetation";
    pAsset10.priority = NeuroLayer.Layer_3_High;
    pAsset10.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_folivore";
    pAsset10.unique = true;
    pAsset10.cooldown = 10;
    pAsset10.only_safe = true;
    pAsset10.only_hungry = true;
    pAsset10.weight = 1f;
    this.add(pAsset10);
    DecisionAsset pAsset11 = new DecisionAsset();
    pAsset11.id = "diet_grass";
    pAsset11.priority = NeuroLayer.Layer_3_High;
    pAsset11.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_graminivore";
    pAsset11.unique = true;
    pAsset11.cooldown = 20;
    pAsset11.only_safe = true;
    pAsset11.only_hungry = true;
    pAsset11.weight = 1f;
    this.add(pAsset11);
    DecisionAsset pAsset12 = new DecisionAsset();
    pAsset12.id = "diet_meat";
    pAsset12.priority = NeuroLayer.Layer_3_High;
    pAsset12.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_carnivore";
    pAsset12.unique = true;
    pAsset12.cooldown = 55;
    pAsset12.only_safe = true;
    pAsset12.only_hungry = true;
    pAsset12.cooldown_on_launch_failure = true;
    pAsset12.weight = 0.96f;
    this.add(pAsset12);
    DecisionAsset pAsset13 = new DecisionAsset();
    pAsset13.id = "diet_blood";
    pAsset13.priority = NeuroLayer.Layer_3_High;
    pAsset13.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_hematophagy";
    pAsset13.unique = true;
    pAsset13.cooldown = 55;
    pAsset13.only_safe = true;
    pAsset13.only_hungry = true;
    pAsset13.cooldown_on_launch_failure = true;
    pAsset13.weight = 0.96f;
    this.add(pAsset13);
    DecisionAsset pAsset14 = new DecisionAsset();
    pAsset14.id = "diet_meat_insect";
    pAsset14.priority = NeuroLayer.Layer_3_High;
    pAsset14.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_insectivore";
    pAsset14.unique = true;
    pAsset14.cooldown = 55;
    pAsset14.only_safe = true;
    pAsset14.only_hungry = true;
    pAsset14.cooldown_on_launch_failure = true;
    pAsset14.weight = 0.96f;
    this.add(pAsset14);
    DecisionAsset pAsset15 = new DecisionAsset();
    pAsset15.id = "diet_same_species";
    pAsset15.priority = NeuroLayer.Layer_3_High;
    pAsset15.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_diet_cannibalism";
    pAsset15.unique = true;
    pAsset15.cooldown = 60;
    pAsset15.only_safe = true;
    pAsset15.only_hungry = true;
    pAsset15.cooldown_on_launch_failure = true;
    pAsset15.action_check_launch = (DecisionAction) (pActor => (double) pActor.getNutritionRatio() < 0.10000000149011612 && pActor.hasStatus("starving"));
    pAsset15.weight = 0.3f;
    this.add(pAsset15);
  }

  private void initDecisionsAnimals()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "run_away_from_carnivore";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.path_icon = "ui/Icons/actor_traits/iconAgile";
    pAsset1.cooldown = 10;
    pAsset1.unique = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.isAnimal())
        return false;
      bool flag = false;
      if (!pActor.isCarnivore())
        flag = Finder.isEnemyNearOnSameIslandAndCarnivore(pActor);
      return flag;
    });
    pAsset1.weight = 2f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "check_if_stuck_on_small_land";
    pAsset2.priority = NeuroLayer.Layer_3_High;
    pAsset2.path_icon = "ui/Icons/iconTileSoil";
    pAsset2.cooldown = 90;
    pAsset2.action_check_launch = (DecisionAction) (pActor => !pActor.current_tile.region.island.isGoodIslandForActor(pActor));
    pAsset2.weight = 3f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "run_away";
    pAsset3.priority = NeuroLayer.Layer_3_High;
    pAsset3.path_icon = "ui/Icons/actor_traits/iconAgile";
    pAsset3.cooldown = 10;
    pAsset3.unique = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => pActor.isFighting() && (double) pActor.getHealthRatio() > 0.20000000298023224);
    pAsset3.weight = 3.1f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "run_away_being_sus";
    pAsset4.priority = NeuroLayer.Layer_3_High;
    pAsset4.task_id = "run_away";
    pAsset4.path_icon = "ui/Icons/actor_traits/iconAgile";
    pAsset4.cooldown = 10;
    pAsset4.unique = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor => true);
    pAsset4.weight = 5f;
    this.add(pAsset4);
  }

  private void initDecisionsSocialize()
  {
    DecisionAsset pAsset = new DecisionAsset();
    pAsset.id = "socialize_initial_check";
    pAsset.priority = NeuroLayer.Layer_2_Moderate;
    pAsset.path_icon = "ui/Icons/culture_traits/culture_trait_gossip_lovers";
    pAsset.cooldown = 30;
    pAsset.unique = true;
    pAsset.cooldown_on_launch_failure = true;
    pAsset.action_check_launch = (DecisionAction) (pActor => pActor.canSocialize());
    pAsset.weight = 0.5f;
    this.add(pAsset);
  }

  private void initDecisionsReproduction()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "sexual_reproduction_try";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_sexual";
    pAsset1.unique = true;
    pAsset1.cooldown_on_launch_failure = true;
    pAsset1.cooldown = 20;
    pAsset1.only_adult = true;
    pAsset1.only_safe = true;
    pAsset1.city_must_be_safe = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.canBreed() || !pActor.hasLover() || pActor.isHungry())
        return false;
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset1.weight_calculate_custom = (DecisionActionWeight) (pActor => !pActor.hasReachedOffspringLimit() ? 2f : 0.1f);
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "asexual_reproduction_divine";
    pAsset2.priority = NeuroLayer.Layer_3_High;
    pAsset2.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_divine";
    pAsset2.unique = true;
    pAsset2.cooldown_on_launch_failure = true;
    pAsset2.cooldown = 60;
    pAsset2.only_adult = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.canBreed() || pActor.hasLover() || pActor.hasTrait("miracle_bearer") || (double) pActor.getAgeRatio() < 0.60000002384185791)
        return false;
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset2.weight_calculate_custom = (DecisionActionWeight) (pActor => !pActor.hasReachedOffspringLimit() ? 2f : 0.1f);
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "status_soul_harvested";
    pAsset3.priority = NeuroLayer.Layer_3_High;
    pAsset3.path_icon = "ui/Icons/iconSoulHarvested";
    pAsset3.unique = true;
    pAsset3.cooldown_on_launch_failure = true;
    pAsset3.cooldown = 40;
    pAsset3.only_safe = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor =>
    {
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset3.weight_calculate_custom = (DecisionActionWeight) (pActor => !pActor.hasReachedOffspringLimit() ? 2f : 0.1f);
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "asexual_reproduction_fission";
    pAsset4.priority = NeuroLayer.Layer_3_High;
    pAsset4.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_fission";
    pAsset4.unique = true;
    pAsset4.cooldown_on_launch_failure = true;
    pAsset4.cooldown = 20;
    pAsset4.only_adult = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.canBreed() || (double) pActor.getHealthRatio() < 0.89999997615814209)
        return false;
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset4.weight_calculate_custom = (DecisionActionWeight) (pActor => !pActor.hasReachedOffspringLimit() ? 2f : 0.1f);
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "asexual_reproduction_budding";
    pAsset5.priority = NeuroLayer.Layer_3_High;
    pAsset5.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_budding";
    pAsset5.unique = true;
    pAsset5.cooldown_on_launch_failure = true;
    pAsset5.cooldown = 50;
    pAsset5.only_adult = true;
    pAsset5.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.canBreed() || (double) pActor.getHealthRatio() < 0.89999997615814209)
        return false;
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset5.weight_calculate_custom = (DecisionActionWeight) (pActor => !pActor.hasReachedOffspringLimit() ? 2f : 0.1f);
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "asexual_reproduction_parthenogenesis";
    pAsset6.priority = NeuroLayer.Layer_3_High;
    pAsset6.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_parthenogenesis";
    pAsset6.unique = true;
    pAsset6.cooldown_on_launch_failure = true;
    pAsset6.cooldown = 40;
    pAsset6.only_adult = true;
    pAsset6.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.isSexFemale() || !pActor.canBreed())
        return false;
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset6.weight_calculate_custom = (DecisionActionWeight) (pActor => !pActor.hasReachedOffspringLimit() ? 2f : 0.1f);
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "asexual_reproduction_spores";
    pAsset7.priority = NeuroLayer.Layer_3_High;
    pAsset7.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_spores";
    pAsset7.unique = true;
    pAsset7.cooldown_on_launch_failure = true;
    pAsset7.cooldown = 60;
    pAsset7.only_adult = true;
    pAsset7.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.canBreed() || !pActor.hasStatus("just_ate"))
        return false;
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset7.weight_calculate_custom = (DecisionActionWeight) (pActor => !pActor.hasReachedOffspringLimit() ? 2f : 0.1f);
    this.add(pAsset7);
    DecisionAsset pAsset8 = new DecisionAsset();
    pAsset8.id = "asexual_reproduction_vegetative";
    pAsset8.priority = NeuroLayer.Layer_3_High;
    pAsset8.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_reproduction_vegetative";
    pAsset8.unique = true;
    pAsset8.cooldown_on_launch_failure = true;
    pAsset8.cooldown = 30;
    pAsset8.only_adult = true;
    pAsset8.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.canBreed() || pActor.hasStatus("taking_roots"))
        return false;
      pActor.subspecies.countReproductionNeuron();
      return true;
    });
    pAsset8.weight = 2.5f;
    this.add(pAsset8);
  }

  private void initDecisionsClans()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "try_new_plot";
    pAsset1.priority = NeuroLayer.Layer_2_Moderate;
    pAsset1.path_icon = "ui/Icons/iconPlot";
    pAsset1.cooldown = 40;
    pAsset1.list_civ = true;
    pAsset1.only_adult = true;
    pAsset1.only_safe = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor => !pActor.hasPlot() && pActor.hasClan());
    pAsset1.weight = 1.5f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "check_plot";
    pAsset2.priority = NeuroLayer.Layer_3_High;
    pAsset2.path_icon = "ui/Icons/iconPlot";
    pAsset2.cooldown = 3;
    pAsset2.only_safe = true;
    pAsset2.unique = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor => pActor.hasPlot());
    pAsset2.weight = 5f;
    this.add(pAsset2);
  }

  private void initDecisionsKings()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "king_check_new_city_foundation";
    pAsset1.priority = NeuroLayer.Layer_2_Moderate;
    pAsset1.path_icon = "ui/Icons/iconCity";
    pAsset1.cooldown = 60;
    pAsset1.unique = true;
    pAsset1.only_adult = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor => WorldLawLibrary.world_law_kingdom_expansion.isEnabled() && !pActor.kingdom.hasEnemies());
    pAsset1.weight = 3f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "king_change_kingdom_language";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconLanguage";
    pAsset2.cooldown = 10;
    pAsset2.unique = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor => pActor.hasLanguage() && pActor.kingdom.getLanguage() != pActor.language);
    pAsset2.weight = 1f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "king_change_kingdom_culture";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconCulture";
    pAsset3.cooldown = 10;
    pAsset3.unique = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => pActor.hasCulture() && pActor.kingdom.getCulture() != pActor.culture);
    pAsset3.weight = 1f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "king_change_kingdom_religion";
    pAsset4.priority = NeuroLayer.Layer_2_Moderate;
    pAsset4.path_icon = "ui/Icons/iconReligion";
    pAsset4.cooldown = 10;
    pAsset4.unique = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor => pActor.hasReligion() && pActor.kingdom.getReligion() != pActor.religion);
    pAsset4.weight = 1f;
    this.add(pAsset4);
  }

  private void initDecisionsLeaders()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "leader_change_city_language";
    pAsset1.priority = NeuroLayer.Layer_2_Moderate;
    pAsset1.path_icon = "ui/Icons/iconLanguage";
    pAsset1.cooldown = 10;
    pAsset1.unique = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.hasLanguage() && pActor.city.getLanguage() != pActor.language);
    pAsset1.weight = 1f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "leader_change_city_culture";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconCulture";
    pAsset2.cooldown = 10;
    pAsset2.unique = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.hasCulture() && pActor.city.getCulture() != pActor.culture);
    pAsset2.weight = 1f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "leader_change_city_religion";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconReligion";
    pAsset3.cooldown = 10;
    pAsset3.unique = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.hasReligion() && pActor.city.getReligion() != pActor.religion);
    pAsset3.weight = 1f;
    this.add(pAsset3);
  }

  private void initDecisionsCivs()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "try_to_return_to_home_city";
    pAsset1.priority = NeuroLayer.Layer_2_Moderate;
    pAsset1.path_icon = "ui/Icons/iconHoused";
    pAsset1.cooldown = 15;
    pAsset1.unique = true;
    pAsset1.only_adult = true;
    pAsset1.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && !pActor.inOwnCityBorders() && !pActor.inOwnCityIsland());
    pAsset1.weight = 3f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "random_move_near_house";
    pAsset2.priority = NeuroLayer.Layer_0_Minimal;
    pAsset2.path_icon = "ui/Icons/iconLivingHouse";
    pAsset2.cooldown = 30;
    pAsset2.action_check_launch = (DecisionAction) (pActor => pActor.hasHouse());
    pAsset2.weight = 0.3f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "try_to_read";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconBooks";
    pAsset3.cooldown = 120;
    pAsset3.only_adult = true;
    pAsset3.unique = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && (double) pActor.stats["intelligence"] > 5.0 && pActor.city.hasBooksToRead(pActor));
    pAsset3.weight = 1f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "try_affect_dreams";
    pAsset4.priority = NeuroLayer.Layer_2_Moderate;
    pAsset4.path_icon = "ui/Icons/subspecies_traits/subspecies_trait_dreamweavers";
    pAsset4.cooldown = 120;
    pAsset4.only_adult = true;
    pAsset4.unique = true;
    pAsset4.weight = 1.5f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "try_to_take_city_item";
    pAsset5.priority = NeuroLayer.Layer_2_Moderate;
    pAsset5.path_icon = "ui/Icons/iconEquipmentEditor2";
    pAsset5.cooldown = 180;
    pAsset5.unique = true;
    pAsset5.only_adult = true;
    pAsset5.action_check_launch = (DecisionAction) (pActor => pActor.inOwnCityBorders() && pActor.city.data.equipment.hasAnyItem());
    pAsset5.weight = 1.5f;
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "make_items";
    pAsset6.priority = NeuroLayer.Layer_2_Moderate;
    pAsset6.path_icon = "ui/Icons/iconReforge";
    pAsset6.cooldown = 90;
    pAsset6.unique = true;
    pAsset6.only_adult = true;
    pAsset6.action_check_launch = (DecisionAction) (pActor => pActor.hasHouse() && pActor.inOwnCityBorders() && pActor.city.hasResourcesForNewItems());
    pAsset6.weight = 0.4f;
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "city_idle_walking";
    pAsset7.priority = NeuroLayer.Layer_1_Low;
    pAsset7.path_icon = "ui/Icons/iconCity";
    pAsset7.cooldown = 5;
    pAsset7.unique = true;
    pAsset7.action_check_launch = (DecisionAction) (pActor => pActor.city.hasZones());
    pAsset7.weight = 0.5f;
    this.add(pAsset7);
    DecisionAsset pAsset8 = new DecisionAsset();
    pAsset8.id = "store_resources";
    pAsset8.priority = NeuroLayer.Layer_3_High;
    pAsset8.path_icon = "ui/Icons/iconCityInventory";
    pAsset8.cooldown = 5;
    pAsset8.unique = true;
    pAsset8.action_check_launch = (DecisionAction) (pActor => pActor.isCarryingResources() && pActor.city.hasStorageBuilding() && (!pActor.isWarrior() || pActor.inOwnCityBorders()));
    pAsset8.weight = 3.1f;
    this.add(pAsset8);
    DecisionAsset pAsset9 = new DecisionAsset();
    pAsset9.id = "stay_in_own_home";
    pAsset9.priority = NeuroLayer.Layer_1_Low;
    pAsset9.path_icon = "ui/Icons/iconHoused";
    pAsset9.cooldown = 15;
    pAsset9.list_civ = true;
    pAsset9.action_check_launch = (DecisionAction) (pActor => pActor.hasHouse() && (double) pActor.getHappinessRatio() <= 0.5 && Randy.randomChance(0.2f));
    pAsset9.weight = 0.5f;
    this.add(pAsset9);
    DecisionAsset pAsset10 = new DecisionAsset();
    pAsset10.id = "generate_loot";
    pAsset10.priority = NeuroLayer.Layer_2_Moderate;
    pAsset10.path_icon = "ui/Icons/iconLoot";
    pAsset10.cooldown = 60;
    pAsset10.list_civ = true;
    pAsset10.action_check_launch = (DecisionAction) (pActor => pActor.hasHouse());
    pAsset10.weight = 1.5f;
    this.add(pAsset10);
    DecisionAsset pAsset11 = new DecisionAsset();
    pAsset11.id = "try_to_eat_city_food";
    pAsset11.priority = NeuroLayer.Layer_3_High;
    pAsset11.path_icon = "ui/Icons/iconHunger";
    pAsset11.cooldown = 10;
    pAsset11.unique = true;
    pAsset11.only_hungry = true;
    pAsset11.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.city.hasSuitableFood(pActor.subspecies));
    pAsset11.weight = 2.6f;
    this.add(pAsset11);
    DecisionAsset pAsset12 = new DecisionAsset();
    pAsset12.id = "find_city_job";
    pAsset12.priority = NeuroLayer.Layer_3_High;
    pAsset12.path_icon = "ui/Icons/iconShowTasks";
    pAsset12.cooldown = 50;
    pAsset12.unique = true;
    pAsset12.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.isInCityIsland() && pActor.city.jobs.hasAnyTask() && pActor.canWork() && (pActor.city.getPopulationPeople() <= 30 || !pActor.isWarrior()));
    pAsset12.weight_calculate_custom = (DecisionActionWeight) (pActor =>
    {
      float num = 2f;
      if (pActor.isStarving())
        num = 0.3f;
      else if (pActor.isHungry())
        num = 1f;
      return num;
    });
    this.add(pAsset12);
    DecisionAsset pAsset13 = new DecisionAsset();
    pAsset13.id = "claim_land";
    pAsset13.priority = NeuroLayer.Layer_2_Moderate;
    pAsset13.path_icon = "ui/Icons/citizen_jobs/iconCitizenLandClaimer";
    pAsset13.cooldown = 60;
    pAsset13.unique = true;
    pAsset13.cooldown_on_launch_failure = true;
    pAsset13.action_check_launch = (DecisionAction) (pActor => pActor.hasCity() && pActor.city.canGrowZones());
    pAsset13.weight_calculate_custom = (DecisionActionWeight) (pActor => (float) (2.0 + (double) pActor.stats["stewardship"] / 5.0 * 0.10000000149011612));
    this.add(pAsset13);
    DecisionAsset pAsset14 = new DecisionAsset();
    pAsset14.id = "put_out_fire";
    pAsset14.priority = NeuroLayer.Layer_3_High;
    pAsset14.path_icon = "ui/Icons/iconMoney";
    pAsset14.cooldown = 1;
    pAsset14.cooldown_on_launch_failure = true;
    pAsset14.unique = true;
    pAsset14.action_check_launch = (DecisionAction) (pActor => pActor.inOwnCityBorders() && pActor.city.isCityUnderDangerFire() && !pActor.hasStatus("burning"));
    pAsset14.weight = 4f;
    this.add(pAsset14);
    DecisionAsset pAsset15 = new DecisionAsset();
    pAsset15.id = "give_tax";
    pAsset15.priority = NeuroLayer.Layer_3_High;
    pAsset15.path_icon = "ui/Icons/iconMoney";
    pAsset15.cooldown = 90;
    pAsset15.cooldown_on_launch_failure = true;
    pAsset15.unique = true;
    pAsset15.action_check_launch = (DecisionAction) (pActor => pActor.inOwnCityBorders() && pActor.data.loot >= pActor.kingdom.getLootMin());
    pAsset15.weight = 2.55f;
    this.add(pAsset15);
    DecisionAsset pAsset16 = new DecisionAsset();
    pAsset16.id = "check_city_destroyed";
    pAsset16.priority = NeuroLayer.Layer_2_Moderate;
    pAsset16.path_icon = "ui/Icons/iconWar";
    pAsset16.cooldown = 10;
    pAsset16.list_civ = true;
    pAsset16.action_check_launch = (DecisionAction) (pActor => !pActor.hasCity() && pActor.profession_asset != null && pActor.profession_asset.cancel_when_no_city);
    pAsset16.weight = 0.5f;
    this.add(pAsset16);
    DecisionAsset pAsset17 = new DecisionAsset();
    pAsset17.id = "check_lover_city";
    pAsset17.priority = NeuroLayer.Layer_3_High;
    pAsset17.path_icon = "ui/Icons/iconCity";
    pAsset17.cooldown = 30;
    pAsset17.only_adult = true;
    pAsset17.unique = true;
    pAsset17.action_check_launch = (DecisionAction) (pActor => !pActor.isKing() && !pActor.isCityLeader() && !pActor.isSexMale() && pActor.hasLover() && pActor.lover.hasCity() && pActor.lover.hasHouse() && !pActor.hasSameCity(pActor.lover));
    pAsset17.weight = 1.5f;
    this.add(pAsset17);
    DecisionAsset pAsset18 = new DecisionAsset();
    pAsset18.id = "find_lover";
    pAsset18.priority = NeuroLayer.Layer_3_High;
    pAsset18.path_icon = "ui/Icons/iconArrowLover";
    pAsset18.cooldown = 50;
    pAsset18.only_adult = true;
    pAsset18.unique = true;
    pAsset18.action_check_launch = (DecisionAction) (pActor => !pActor.hasLover() && pActor.isBreedingAge());
    pAsset18.weight = 1.5f;
    this.add(pAsset18);
    DecisionAsset pAsset19 = new DecisionAsset();
    pAsset19.id = "find_house";
    pAsset19.priority = NeuroLayer.Layer_2_Moderate;
    pAsset19.path_icon = "ui/Icons/iconBuildings";
    pAsset19.cooldown = 10;
    pAsset19.unique = true;
    pAsset19.action_check_launch = (DecisionAction) (pActor => !pActor.hasHouse());
    pAsset19.weight = 0.5f;
    this.add(pAsset19);
    DecisionAsset pAsset20 = new DecisionAsset();
    pAsset20.id = "replenish_energy";
    pAsset20.priority = NeuroLayer.Layer_2_Moderate;
    pAsset20.path_icon = "ui/Icons/iconStamina";
    pAsset20.cooldown = 30;
    pAsset20.unique = true;
    pAsset20.action_check_launch = (DecisionAction) (pActor => (!pActor.isStaminaFull() || !pActor.isManaFull()) && pActor.getCity().hasBuildingType("type_well", pLimitIsland: pActor.current_island));
    pAsset20.weight = 0.3f;
    this.add(pAsset20);
    DecisionAsset pAsset21 = new DecisionAsset();
    pAsset21.id = "repair_equipment";
    pAsset21.priority = NeuroLayer.Layer_2_Moderate;
    pAsset21.path_icon = "ui/Icons/iconReforge";
    pAsset21.cooldown = 300;
    pAsset21.unique = true;
    pAsset21.action_check_launch = (DecisionAction) (pActor => pActor.hasEquipment() && pActor.getCity().hasBuildingType("type_barracks", pLimitIsland: pActor.current_island));
    pAsset21.weight = 0.3f;
    this.add(pAsset21);
  }

  private void initDecisionsBoats()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "boat_check_existence";
    pAsset1.priority = NeuroLayer.Layer_1_Low;
    pAsset1.path_icon = "ui/Icons/iconBoat";
    pAsset1.cooldown = 5;
    pAsset1.unique = true;
    pAsset1.weight = 1f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "boat_danger_check";
    pAsset2.priority = NeuroLayer.Layer_1_Low;
    pAsset2.path_icon = "ui/Icons/iconBoat";
    pAsset2.cooldown = 5;
    pAsset2.unique = true;
    pAsset2.weight = 1.25f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "boat_idle";
    pAsset3.priority = NeuroLayer.Layer_1_Low;
    pAsset3.path_icon = "ui/Icons/iconBoat";
    pAsset3.cooldown = 5;
    pAsset3.unique = true;
    pAsset3.weight = 0.75f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "boat_check_limits";
    pAsset4.priority = NeuroLayer.Layer_1_Low;
    pAsset4.path_icon = "ui/Icons/iconBoat";
    pAsset4.cooldown = 15;
    pAsset4.unique = true;
    pAsset4.weight = 1f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "boat_fishing";
    pAsset5.priority = NeuroLayer.Layer_1_Low;
    pAsset5.path_icon = "ui/Icons/iconResFish";
    pAsset5.cooldown = 1;
    pAsset5.unique = true;
    pAsset5.weight = 1.3f;
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "boat_trading";
    pAsset6.priority = NeuroLayer.Layer_1_Low;
    pAsset6.path_icon = "ui/Icons/iconCityInventory";
    pAsset6.cooldown = 1;
    pAsset6.unique = true;
    pAsset6.weight = 1.3f;
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "boat_transport_check";
    pAsset7.priority = NeuroLayer.Layer_2_Moderate;
    pAsset7.path_icon = "ui/Icons/iconBoat";
    pAsset7.cooldown = 1;
    pAsset7.unique = true;
    pAsset7.weight = 1.3f;
    this.add(pAsset7);
  }

  private void initDecisionsBees()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "pollinate";
    pAsset1.priority = NeuroLayer.Layer_1_Low;
    pAsset1.path_icon = "ui/Icons/iconBee";
    pAsset1.cooldown = 10;
    pAsset1.unique = true;
    pAsset1.weight = 1.3f;
    pAsset1.action_check_launch = (DecisionAction) (pActor => !(pActor.asset.id == "bee") || pActor.hasHomeBuilding());
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "bee_find_hive";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconBeehive";
    pAsset2.cooldown = 20;
    pAsset2.only_mob = true;
    pAsset2.unique = true;
    pAsset2.weight = 1.5f;
    pAsset2.action_check_launch = (DecisionAction) (pActor => !pActor.isKingdomCiv() && !pActor.hasHomeBuilding());
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "bee_create_hive";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconBeehive";
    pAsset3.cooldown = 200;
    pAsset3.unique = true;
    pAsset3.only_mob = true;
    pAsset3.cooldown_on_launch_failure = true;
    pAsset3.weight = 1.5f;
    pAsset3.action_check_launch = (DecisionAction) (pActor => !pActor.hasHomeBuilding() && pActor.isSexFemale());
    this.add(pAsset3);
  }

  private void initDecisionsGeneral()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "swim_to_island";
    pAsset1.priority = NeuroLayer.Layer_4_Critical;
    pAsset1.path_icon = "ui/Icons/iconTileShallowWater";
    pAsset1.cooldown = 1;
    pAsset1.action_check_launch = (DecisionAction) (pActor => !pActor.isInStablePlace());
    pAsset1.weight = 4f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "move_to_water";
    pAsset2.priority = NeuroLayer.Layer_4_Critical;
    pAsset2.path_icon = "ui/Icons/iconTileShallowWater";
    pAsset2.cooldown = 1;
    pAsset2.action_check_launch = (DecisionAction) (pActor => !pActor.isInStablePlace());
    pAsset2.weight = 4f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "random_move";
    pAsset3.priority = NeuroLayer.Layer_0_Minimal;
    pAsset3.path_icon = "ui/Icons/iconArrowDestination";
    pAsset3.cooldown = 1;
    pAsset3.weight = 0.2f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "random_fun_move";
    pAsset4.priority = NeuroLayer.Layer_1_Low;
    pAsset4.path_icon = "ui/Icons/iconArrowDestination";
    pAsset4.cooldown = 10;
    pAsset4.list_animal = true;
    pAsset4.weight = 0.2f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "wait5";
    pAsset5.priority = NeuroLayer.Layer_0_Minimal;
    pAsset5.path_icon = "ui/Icons/iconClock";
    pAsset5.cooldown = 10;
    pAsset5.weight = 0.1f;
    this.add(pAsset5);
  }

  private void initDecisionsTraits()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "follow_desire_target";
    pAsset1.priority = NeuroLayer.Layer_3_High;
    pAsset1.path_icon = "ui/Icons/iconGoldBrain";
    pAsset1.cooldown = 5;
    pAsset1.unique = true;
    pAsset1.weight = 4f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "try_to_poop";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconPoop";
    pAsset2.cooldown = 60;
    pAsset2.unique = true;
    pAsset2.action_check_launch = (DecisionAction) (pActor => pActor.asset.actor_size != ActorSize.S0_Bug && !pActor.hasSubspeciesTrait("reproduction_spores"));
    pAsset2.weight = 1.5f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "try_to_launch_fireworks";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconFireworks";
    pAsset3.cooldown = 60;
    pAsset3.unique = true;
    pAsset3.action_check_launch = (DecisionAction) (pActor => pActor.hasEnoughMoney(SimGlobals.m.festive_fireworks_cost));
    pAsset3.weight = 1.5f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "reflection";
    pAsset4.priority = NeuroLayer.Layer_2_Moderate;
    pAsset4.path_icon = "ui/Icons/iconBre";
    pAsset4.cooldown = 100;
    pAsset4.unique = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor => pActor.subspecies.can_process_emotions);
    pAsset4.weight = 2.5f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "madness_random_emotion";
    pAsset5.priority = NeuroLayer.Layer_3_High;
    pAsset5.path_icon = "ui/Icons/actor_traits/iconMadness";
    pAsset5.cooldown = 60;
    pAsset5.unique = true;
    pAsset5.action_check_launch = (DecisionAction) (pActor => pActor.hasSubspecies() && pActor.subspecies.can_process_emotions);
    pAsset5.weight = 2.5f;
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "try_to_steal_money";
    pAsset6.priority = NeuroLayer.Layer_2_Moderate;
    pAsset6.path_icon = "ui/Icons/actor_traits/iconThief";
    pAsset6.cooldown = 60;
    pAsset6.unique = true;
    pAsset6.action_check_launch = (DecisionAction) (pActor => pActor.isHungry() && pActor.canGetFoodFromCity());
    pAsset6.weight = 1f;
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "kill_unruly_clan_members";
    pAsset7.priority = NeuroLayer.Layer_3_High;
    pAsset7.path_icon = "ui/Icons/clan_traits/clan_trait_deathbound";
    pAsset7.cooldown = 200;
    pAsset7.unique = true;
    pAsset7.action_check_launch = (DecisionAction) (pActor => pActor.clan.getChief() == pActor && pActor.kingdom.hasEnemies());
    pAsset7.weight = 3f;
    this.add(pAsset7);
    DecisionAsset pAsset8 = new DecisionAsset();
    pAsset8.id = "banish_unruly_clan_members";
    pAsset8.priority = NeuroLayer.Layer_3_High;
    pAsset8.path_icon = "ui/Icons/clan_traits/clan_trait_blood_pact";
    pAsset8.cooldown = 200;
    pAsset8.unique = true;
    pAsset8.action_check_launch = (DecisionAction) (pActor => pActor.clan.getChief() == pActor && pActor.kingdom.hasEnemies());
    pAsset8.weight = 3f;
    this.add(pAsset8);
  }

  private void initDecisionsUnique()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "check_heal";
    pAsset1.priority = NeuroLayer.Layer_2_Moderate;
    pAsset1.path_icon = "ui/Icons/iconHealth";
    pAsset1.cooldown = 5;
    pAsset1.unique = true;
    pAsset1.weight = 0.5f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "make_skeleton";
    pAsset2.priority = NeuroLayer.Layer_2_Moderate;
    pAsset2.path_icon = "ui/Icons/iconSkeleton";
    pAsset2.cooldown = 10;
    pAsset2.unique = true;
    pAsset2.weight = 0.5f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "spawn_fertilizer";
    pAsset3.priority = NeuroLayer.Layer_2_Moderate;
    pAsset3.path_icon = "ui/Icons/iconFertilizerPlants";
    pAsset3.cooldown = 10;
    pAsset3.unique = true;
    pAsset3.weight = 0.5f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "random_teleport";
    pAsset4.priority = NeuroLayer.Layer_1_Low;
    pAsset4.path_icon = "ui/Icons/iconArrowDestination";
    pAsset4.cooldown = 20;
    pAsset4.unique = true;
    pAsset4.weight = 0.5f;
    this.add(pAsset4);
    DecisionAsset pAsset5 = new DecisionAsset();
    pAsset5.id = "teleport_back_home";
    pAsset5.priority = NeuroLayer.Layer_1_Low;
    pAsset5.path_icon = "ui/Icons/iconArrowDestination";
    pAsset5.cooldown = 60;
    pAsset5.action_check_launch = (DecisionAction) (pActor =>
    {
      if (!pActor.hasCity() || !Randy.randomChance(0.3f))
        return false;
      WorldTile tile = pActor.city.getTile();
      return tile != null && !tile.isSameIsland(pActor.current_tile);
    });
    pAsset5.unique = true;
    pAsset5.weight = 1f;
    this.add(pAsset5);
    DecisionAsset pAsset6 = new DecisionAsset();
    pAsset6.id = "check_cure";
    pAsset6.priority = NeuroLayer.Layer_2_Moderate;
    pAsset6.path_icon = "ui/Icons/iconHealth";
    pAsset6.cooldown = 15;
    pAsset6.unique = true;
    pAsset6.action_check_launch = (DecisionAction) (_ => Randy.randomChance(0.3f));
    pAsset6.weight = 0.5f;
    this.add(pAsset6);
    DecisionAsset pAsset7 = new DecisionAsset();
    pAsset7.id = "burn_tumors";
    pAsset7.priority = NeuroLayer.Layer_3_High;
    pAsset7.path_icon = "ui/Icons/iconFire";
    pAsset7.cooldown = 10;
    pAsset7.unique = true;
    pAsset7.action_check_launch = (DecisionAction) (_ => Randy.randomChance(0.5f));
    pAsset7.weight = 0.5f;
    this.add(pAsset7);
    DecisionAsset pAsset8 = new DecisionAsset();
    pAsset8.id = "random_move_towards_civ_building";
    pAsset8.priority = NeuroLayer.Layer_1_Low;
    pAsset8.path_icon = "ui/Icons/iconArrowDestination";
    pAsset8.cooldown = 10;
    pAsset8.unique = true;
    pAsset8.weight = 0.5f;
    this.add(pAsset8);
    DecisionAsset pAsset9 = new DecisionAsset();
    pAsset9.id = "random_swim";
    pAsset9.priority = NeuroLayer.Layer_1_Low;
    pAsset9.path_icon = "ui/Icons/iconTileShallowWater";
    pAsset9.cooldown = 10;
    pAsset9.unique = true;
    pAsset9.weight = 2f;
    this.add(pAsset9);
  }

  private void initDecisionsOther()
  {
    DecisionAsset pAsset = new DecisionAsset();
    pAsset.id = "attack_golden_brain";
    pAsset.priority = NeuroLayer.Layer_3_High;
    pAsset.path_icon = "ui/Icons/iconGoldBrain";
    pAsset.cooldown = 60;
    pAsset.only_mob = true;
    pAsset.unique = true;
    pAsset.weight = 1f;
    this.add(pAsset);
  }

  private void initDecisionsChildren()
  {
    DecisionAsset pAsset1 = new DecisionAsset();
    pAsset1.id = "child_random_flips";
    pAsset1.priority = NeuroLayer.Layer_1_Low;
    pAsset1.path_icon = "ui/Icons/iconChildren";
    pAsset1.cooldown = 5;
    pAsset1.list_baby = true;
    pAsset1.weight = 0.1f;
    this.add(pAsset1);
    DecisionAsset pAsset2 = new DecisionAsset();
    pAsset2.id = "child_play_at_one_spot";
    pAsset2.priority = NeuroLayer.Layer_1_Low;
    pAsset2.path_icon = "ui/Icons/iconChildren";
    pAsset2.cooldown = 5;
    pAsset2.list_baby = true;
    pAsset2.weight = 0.1f;
    this.add(pAsset2);
    DecisionAsset pAsset3 = new DecisionAsset();
    pAsset3.id = "child_random_jump";
    pAsset3.priority = NeuroLayer.Layer_1_Low;
    pAsset3.path_icon = "ui/Icons/iconChildren";
    pAsset3.cooldown = 5;
    pAsset3.list_baby = true;
    pAsset3.weight = 0.1f;
    this.add(pAsset3);
    DecisionAsset pAsset4 = new DecisionAsset();
    pAsset4.id = "child_follow_parent";
    pAsset4.priority = NeuroLayer.Layer_2_Moderate;
    pAsset4.path_icon = "ui/Icons/iconAdults";
    pAsset4.cooldown = 10;
    pAsset4.unique = true;
    pAsset4.action_check_launch = (DecisionAction) (pActor => pActor.family.hasFounders() && pActor.isBaby());
    pAsset4.weight = 0.2f;
    this.add(pAsset4);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (DecisionAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }

  public override void linkAssets()
  {
    using (ListPool<DecisionAsset> list1 = new ListPool<DecisionAsset>())
    {
      using (ListPool<DecisionAsset> list2 = new ListPool<DecisionAsset>())
      {
        using (ListPool<DecisionAsset> list3 = new ListPool<DecisionAsset>())
        {
          using (ListPool<DecisionAsset> list4 = new ListPool<DecisionAsset>())
          {
            using (ListPool<DecisionAsset> list5 = new ListPool<DecisionAsset>())
            {
              int num = 0;
              foreach (DecisionAsset decisionAsset in this.list)
              {
                decisionAsset.decision_index = num++;
                decisionAsset.priority_int_cached = (int) decisionAsset.priority;
                decisionAsset.has_weight_custom = decisionAsset.weight_calculate_custom != null;
                if (!decisionAsset.unique)
                {
                  if (decisionAsset.list_baby)
                    list2.Add(decisionAsset);
                  else if (decisionAsset.list_animal)
                    list4.Add(decisionAsset);
                  else if (decisionAsset.list_civ)
                    list1.Add(decisionAsset);
                  else
                    list5.Add(decisionAsset);
                }
              }
              this.list_only_civ = list1.ToArray<DecisionAsset>();
              this.list_only_children = list2.ToArray<DecisionAsset>();
              this.list_only_city = list3.ToArray<DecisionAsset>();
              this.list_only_animal = list4.ToArray<DecisionAsset>();
              this.list_others = list5.ToArray<DecisionAsset>();
              base.linkAssets();
            }
          }
        }
      }
    }
  }

  public override void editorDiagnostic()
  {
    foreach (DecisionAsset pAsset in this.list)
      this.checkSpriteExists("path_icon", pAsset.path_icon, (Asset) pAsset);
    foreach (DecisionAsset pValue in this.list)
    {
      int num = 0;
      if (pValue.unique && (pValue.list_civ || pValue.list_baby || pValue.list_animal))
        BaseAssetLibrary.logAssetError($"<e>{pValue.id}</e>: Unique but also has list setting?");
      if (this.list_only_civ.Contains<DecisionAsset>(pValue))
        ++num;
      if (this.list_only_children.Contains<DecisionAsset>(pValue))
        ++num;
      if (this.list_only_city.Contains<DecisionAsset>(pValue))
        ++num;
      if (this.list_only_animal.Contains<DecisionAsset>(pValue))
        ++num;
      if (this.list_others.Contains<DecisionAsset>(pValue))
        ++num;
      if (pValue.unique && num > 0)
        BaseAssetLibrary.logAssetError($"<e>{pValue.id}</e>: Unique but also in a list?");
      if (!pValue.unique && num == 0)
        BaseAssetLibrary.logAssetError($"<e>{pValue.id}</e>: Not unique but not in any list?");
      if (num > 1)
        BaseAssetLibrary.logAssetError($"<e>{pValue.id}</e>: In multiple lists?");
    }
    base.editorDiagnostic();
  }
}
