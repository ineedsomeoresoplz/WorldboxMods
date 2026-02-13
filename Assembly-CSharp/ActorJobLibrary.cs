// Decompiled with JetBrains decompiler
// Type: ActorJobLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours.conditions;

#nullable disable
public class ActorJobLibrary : AssetLibrary<ActorJob>
{
  public override void init()
  {
    base.init();
    this.initJobsCivs();
    this.initJobsMobs();
  }

  private void initJobsCivs()
  {
    ActorJob pAsset1 = new ActorJob();
    pAsset1.id = "unit_citizen";
    this.add(pAsset1);
    this.t.addTask("make_decision");
    this.t.addTask("check_city_destroyed");
    ActorJob pAsset2 = new ActorJob();
    pAsset2.id = "hunter";
    this.add(pAsset2);
    this.t.addTask("random_move");
    this.t.addTask("do_hunting");
    this.t.addTask("end_job");
    ActorJob pAsset3 = new ActorJob();
    pAsset3.id = "builder";
    this.add(pAsset3);
    this.t.addTask("try_build_building");
    this.t.addTask("end_job");
    ActorJob pAsset4 = new ActorJob();
    pAsset4.id = "cleaner";
    this.add(pAsset4);
    this.t.addTask("cleaning");
    this.t.addTask("end_job");
    ActorJob pAsset5 = new ActorJob();
    pAsset5.id = "manure_cleaner";
    this.add(pAsset5);
    this.t.addTask("manure_cleaning");
    this.t.addTask("end_job");
    ActorJob pAsset6 = new ActorJob();
    pAsset6.id = "road_builder";
    this.add(pAsset6);
    this.t.addTask("build_road");
    this.t.addTask("end_job");
    ActorJob pAsset7 = new ActorJob();
    pAsset7.id = "woodcutter";
    this.add(pAsset7);
    this.t.addTask("chop_trees");
    this.t.addTask("end_job");
    ActorJob pAsset8 = new ActorJob();
    pAsset8.id = "miner";
    this.add(pAsset8);
    this.t.addTask("mine");
    this.t.addTask("end_job");
    ActorJob pAsset9 = new ActorJob();
    pAsset9.id = "miner_deposit";
    this.add(pAsset9);
    this.t.addTask("mine_deposit");
    this.t.addTask("end_job");
    ActorJob pAsset10 = new ActorJob();
    pAsset10.id = "gatherer_bushes";
    this.add(pAsset10);
    this.t.addTask("collect_fruits");
    this.t.addTask("end_job");
    ActorJob pAsset11 = new ActorJob();
    pAsset11.id = "gatherer_herbs";
    this.add(pAsset11);
    this.t.addTask("collect_herbs");
    this.t.addTask("end_job");
    ActorJob pAsset12 = new ActorJob();
    pAsset12.id = "gatherer_honey";
    this.add(pAsset12);
    this.t.addTask("collect_honey");
    this.t.addTask("end_job");
    ActorJob pAsset13 = new ActorJob();
    pAsset13.id = "farmer";
    this.add(pAsset13);
    this.t.addTask("farmer_make_field");
    this.t.addTask("farmer_plant_crops");
    this.t.addTask("farmer_harvest");
    this.t.addTask("farmer_fertilize_crops");
    this.t.addTask("farmer_random_move");
    this.t.addTask("check_end_job");
    this.t.addTask("check_city_destroyed");
    ActorJob pAsset14 = new ActorJob();
    pAsset14.id = "attacker";
    this.add(pAsset14);
    this.t.addTask("make_decision");
  }

  private void initJobsMobs()
  {
    ActorJob pAsset1 = new ActorJob();
    pAsset1.id = "crab";
    this.add(pAsset1);
    this.t.addTask("swim_to_island");
    this.t.addTask("crab_danger_check");
    this.t.addTask("random_move");
    this.t.addTask("crab_danger_check");
    this.t.addTask("crab_eat");
    this.t.addTask("crab_danger_check");
    ActorJob pAsset2 = new ActorJob();
    pAsset2.id = "crab_burrow";
    this.add(pAsset2);
    this.t.addTask("crab_burrow");
    ActorJob pAsset3 = new ActorJob();
    pAsset3.id = "decision";
    this.add(pAsset3);
    this.t.addTask("make_decision");
    ActorJob pAsset4 = new ActorJob();
    pAsset4.id = "egg";
    this.add(pAsset4);
    this.t.addTask("wait10");
    this.t.addTask("end_job");
    ActorJob pAsset5 = new ActorJob();
    pAsset5.id = "random_move";
    this.add(pAsset5);
    this.t.addTask("random_move");
    ActorJob pAsset6 = new ActorJob();
    pAsset6.id = "random_swim";
    this.add(pAsset6);
    this.t.addTask("random_swim");
    ActorJob pAsset7 = new ActorJob();
    pAsset7.id = "printer_job";
    this.add(pAsset7);
    this.t.addTask("print_start");
    this.t.addTask("print_step");
    ActorJob pAsset8 = new ActorJob();
    pAsset8.id = "godfinger_job";
    pAsset8.random = true;
    this.add(pAsset8);
    this.t.addTask("godfinger_move");
    this.t.addTask("godfinger_move");
    this.t.addTask("godfinger_find_target");
    this.t.addTask("godfinger_random_fun_move");
    this.t.addTask("godfinger_random_fun_move");
    this.t.addTask("godfinger_circle_move");
    this.t.addTask("godfinger_circle_move");
    this.t.addTask("godfinger_circle_move_big");
    this.t.addTask("godfinger_circle_move_big");
    this.t.addTask("godfinger_circle_move_small");
    this.t.addTask("godfinger_circle_move_small");
    ActorJob pAsset9 = new ActorJob();
    pAsset9.id = "dragon_job";
    pAsset9.random = true;
    this.add(pAsset9);
    this.t.addTask("dragon_slide");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying());
    this.t.addCondition((BehaviourActorCondition) new CondNoPeace());
    this.t.addCondition((BehaviourActorCondition) new CondDragonHasTargets());
    this.t.addCondition((BehaviourActorCondition) new CondDragonCanSlide());
    this.t.addTask("dragon_fly");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying());
    this.t.addTask("dragon_land");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying());
    this.t.addCondition((BehaviourActorCondition) new CondDragonCanLand());
    this.t.addTask("dragon_sleep");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying(), false);
    this.t.addCondition((BehaviourActorCondition) new CondDragonSleepy());
    this.t.addCondition((BehaviourActorCondition) new CondDragonHasTargets(), false);
    this.t.addCondition((BehaviourActorCondition) new CondDragonHasCityTarget(), false);
    this.t.addCondition((BehaviourActorCondition) new CondCurrentTileNoOtherUnits());
    this.t.addTask("dragon_wakeup");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying(), false);
    this.t.addCondition((BehaviourActorCondition) new CondDragonSleeping());
    this.t.addTask("dragon_land_attack");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying(), false);
    this.t.addCondition((BehaviourActorCondition) new CondNoPeace());
    this.t.addCondition((BehaviourActorCondition) new CondDragonCanLandAttack());
    this.t.addTask("dragon_idle");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying(), false);
    this.t.addCondition((BehaviourActorCondition) new CondDragonHasTargets(), false);
    this.t.addCondition((BehaviourActorCondition) new CondDragonHasCityTarget(), false);
    this.t.addTask("dragon_up");
    this.t.addCondition((BehaviourActorCondition) new CondActorFlying(), false);
    this.t.addCondition((BehaviourActorCondition) new CondActorNotJustLanded());
    this.t.addCondition((BehaviourActorCondition) new CondDragonCanLandAttack(), false);
    ActorJob pAsset10 = new ActorJob();
    pAsset10.id = "ufo_job";
    this.add(pAsset10);
    this.t.addTask("ufo_idle");
    this.t.addTask("ufo_fly");
    this.t.addTask("ufo_explore");
    ActorJob pAsset11 = new ActorJob();
    pAsset11.id = "worm_job";
    this.add(pAsset11);
    this.t.addTask("worm_move");
    ActorJob pAsset12 = new ActorJob();
    pAsset12.id = "sandspider_job";
    this.add(pAsset12);
    this.t.addTask("sandspider_move");
    ActorJob pAsset13 = new ActorJob();
    pAsset13.id = "ant_black";
    this.add(pAsset13);
    this.t.addTask("ant_black_island");
    this.t.addTask("ant_black_sand");
    ActorJob pAsset14 = new ActorJob();
    pAsset14.id = "ant_red";
    this.add(pAsset14);
    this.t.addTask("ant_red_move");
    ActorJob pAsset15 = new ActorJob();
    pAsset15.id = "ant_blue";
    this.add(pAsset15);
    this.t.addTask("ant_blue_move");
    ActorJob pAsset16 = new ActorJob();
    pAsset16.id = "ant_green";
    this.add(pAsset16);
    this.t.addTask("ant_green_move");
    ActorJob pAsset17 = new ActorJob();
    pAsset17.id = "skeleton_job";
    this.add(pAsset17);
    this.t.addTask("skeleton_move");
    this.t.addTask("make_decision");
  }
}
