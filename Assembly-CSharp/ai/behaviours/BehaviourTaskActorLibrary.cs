// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehaviourTaskActorLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehaviourTaskActorLibrary : AssetLibrary<BehaviourTaskActor>
{
  public override void init()
  {
    base.init();
    this.initTasksMobs();
    this.initTasksSocializing();
    this.initTasksSubspeciesTraits();
    this.initTasksReproductionSexual();
    this.initTasksReproductionAsexual();
    this.initTasksChildren();
    this.initTasksStatusRelated();
    this.initTasksKings();
    this.initTasksLeaders();
    this.initTasksWarriors();
    this.initTasksSleep();
    this.initTasksPoop();
    this.initTasksThinkingReflectionHappiness();
    this.initTasksClanLeader();
    this.initTasksBoats();
    this.initTasksDragons();
    this.initTasksFingers();
    this.initTasksUFOs();
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "nothing";
    behaviourTaskActor1.cancellable_by_socialize = true;
    behaviourTaskActor1.cancellable_by_reproduction = true;
    behaviourTaskActor1.locale_key = "task_unit_nothing";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconClock");
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "fighting";
    behaviourTaskActor2.in_combat = true;
    behaviourTaskActor2.locale_key = "task_unit_fight";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconWar");
    this.t.addBeh((BehaviourActionActor) new BehFightCheckEnemyIsOk());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(pPathOnWater: true, pCheckCanAttackTarget: true, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "move_from_block";
    behaviourTaskActor3.move_from_block = true;
    behaviourTaskActor3.ignore_fight_check = true;
    behaviourTaskActor3.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehMoveAwayFromBlock());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "swim_to_island";
    behaviourTaskActor4.ignore_fight_check = true;
    behaviourTaskActor4.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconTileShallowWater");
    this.t.addBeh((BehaviourActionActor) new BehGoToStablePlace());
    this.t.addBeh((BehaviourActionActor) new BehGoOrSwimToTileTarget());
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "move_to_water";
    behaviourTaskActor5.ignore_fight_check = true;
    behaviourTaskActor5.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconTileShallowWater");
    this.t.addBeh((BehaviourActionActor) new BehGoToStablePlace());
    this.t.addBeh((BehaviourActionActor) new BehGoOrSwimToTileTarget());
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "check_if_stuck_on_small_land";
    behaviourTaskActor6.ignore_fight_check = true;
    behaviourTaskActor6.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconTileSoil");
    this.t.addBeh((BehaviourActionActor) new BehCheckIfOnSmallLand());
    this.t.addBeh((BehaviourActionActor) new BehWalkIntoWaterCorner());
    this.t.addBeh((BehaviourActionActor) new BehGoOrSwimToTileTarget());
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "pollinate";
    behaviourTaskActor7.locale_key = "task_unit_pollinate";
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconBee");
    this.t.addBeh((BehaviourActionActor) new BehBeeCheckHome());
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_flower", true, false));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f, true));
    this.t.addBeh((BehaviourActionActor) new BehPollinate());
    this.t.addBeh((BehaviourActionActor) new BehBeeCheckReturnHome());
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehBeeReturnHome());
    BehaviourTaskActor behaviourTaskActor8 = new BehaviourTaskActor();
    behaviourTaskActor8.id = "bee_find_hive";
    behaviourTaskActor8.locale_key = "task_unit_bee_find_hive";
    BehaviourTaskActor pAsset8 = behaviourTaskActor8;
    this.t = behaviourTaskActor8;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/iconBeehive");
    this.t.addBeh((BehaviourActionActor) new BehBeeCheckNoHome());
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_hive", false, false));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehBeeJoinHive());
    this.t.addBeh((BehaviourActionActor) new BehBeeReturnHome());
    BehaviourTaskActor behaviourTaskActor9 = new BehaviourTaskActor();
    behaviourTaskActor9.id = "bee_create_hive";
    behaviourTaskActor9.locale_key = "task_unit_bee_create_hive";
    BehaviourTaskActor pAsset9 = behaviourTaskActor9;
    this.t = behaviourTaskActor9;
    this.add(pAsset9);
    this.t.setIcon("ui/Icons/iconBeehive");
    this.t.addBeh((BehaviourActionActor) new BehBeeCheckNoHome());
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.Biome));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 5; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, pTimerAction: 0.5f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehBeeCreateHive());
    this.t.addBeh((BehaviourActionActor) new BehBeeJoinHive());
    this.t.addBeh((BehaviourActionActor) new BehBeeReturnHome());
    BehaviourTaskActor behaviourTaskActor10 = new BehaviourTaskActor();
    behaviourTaskActor10.id = "random_move";
    behaviourTaskActor10.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset10 = behaviourTaskActor10;
    this.t = behaviourTaskActor10;
    this.add(pAsset10);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    BehaviourTaskActor behaviourTaskActor11 = new BehaviourTaskActor();
    behaviourTaskActor11.id = "random_fun_move";
    behaviourTaskActor11.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset11 = behaviourTaskActor11;
    this.t = behaviourTaskActor11;
    this.add(pAsset11);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    for (int index = 0; index < 6; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehFindRandomNeighbourTile());
      this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
      this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 0.01f));
    }
    BehaviourTaskActor behaviourTaskActor12 = new BehaviourTaskActor();
    behaviourTaskActor12.id = "run_away";
    behaviourTaskActor12.speed_multiplier = 2f;
    behaviourTaskActor12.ignore_fight_check = true;
    behaviourTaskActor12.locale_key = "task_unit_flee";
    BehaviourTaskActor pAsset12 = behaviourTaskActor12;
    this.t = behaviourTaskActor12;
    this.add(pAsset12);
    this.t.setIcon("ui/Icons/actor_traits/iconAgile");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomFarTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    BehaviourTaskActor behaviourTaskActor13 = new BehaviourTaskActor();
    behaviourTaskActor13.id = "run_away_from_carnivore";
    behaviourTaskActor13.speed_multiplier = 2f;
    behaviourTaskActor13.ignore_fight_check = true;
    behaviourTaskActor13.locale_key = "task_unit_flee";
    BehaviourTaskActor pAsset13 = behaviourTaskActor13;
    this.t = behaviourTaskActor13;
    this.add(pAsset13);
    this.t.setIcon("ui/Icons/actor_traits/iconAgile");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomFarTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    BehaviourTaskActor behaviourTaskActor14 = new BehaviourTaskActor();
    behaviourTaskActor14.id = "print_start";
    BehaviourTaskActor pAsset14 = behaviourTaskActor14;
    this.t = behaviourTaskActor14;
    this.add(pAsset14);
    this.t.setIcon("ui/Icons/iconPrinterStar");
    this.t.addBeh((BehaviourActionActor) new BehPrinterSetup());
    BehaviourTaskActor behaviourTaskActor15 = new BehaviourTaskActor();
    behaviourTaskActor15.id = "print_step";
    BehaviourTaskActor pAsset15 = behaviourTaskActor15;
    this.t = behaviourTaskActor15;
    this.add(pAsset15);
    this.t.setIcon("ui/Icons/iconPrinterStar");
    this.t.addBeh((BehaviourActionActor) new BehPrinterStep());
    BehaviourTaskActor behaviourTaskActor16 = new BehaviourTaskActor();
    behaviourTaskActor16.id = "worm_move";
    BehaviourTaskActor pAsset16 = behaviourTaskActor16;
    this.t = behaviourTaskActor16;
    this.add(pAsset16);
    this.t.setIcon("ui/Icons/iconWorm");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile8Directions());
    this.t.addBeh((BehaviourActionActor) new BehWormDive());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    this.t.addBeh((BehaviourActionActor) new BehWormDig());
    BehaviourTaskActor behaviourTaskActor17 = new BehaviourTaskActor();
    behaviourTaskActor17.id = "sandspider_move";
    BehaviourTaskActor pAsset17 = behaviourTaskActor17;
    this.t = behaviourTaskActor17;
    this.add(pAsset17);
    this.t.setIcon("ui/Icons/iconSandSpider");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile4Directions());
    this.t.addBeh((BehaviourActionActor) new BehSandspiderCheckSand());
    this.t.addBeh((BehaviourActionActor) new BehSandspiderCheckDie());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    this.t.addBeh((BehaviourActionActor) new BehSandspiderBuildSand());
    BehaviourTaskActor behaviourTaskActor18 = new BehaviourTaskActor();
    behaviourTaskActor18.id = "ant_black_island";
    BehaviourTaskActor pAsset18 = behaviourTaskActor18;
    this.t = behaviourTaskActor18;
    this.add(pAsset18);
    this.t.setIcon("ui/Icons/iconAntBlack");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile4Directions());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    this.t.addBeh((BehaviourActionActor) new BehBlackAntBuildIsland());
    BehaviourTaskActor behaviourTaskActor19 = new BehaviourTaskActor();
    behaviourTaskActor19.id = "ant_black_sand";
    BehaviourTaskActor pAsset19 = behaviourTaskActor19;
    this.t = behaviourTaskActor19;
    this.add(pAsset19);
    this.t.setIcon("ui/Icons/iconAntBlack");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile4Directions());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    this.t.addBeh((BehaviourActionActor) new BehBlackAntBuildSand());
    BehaviourTaskActor behaviourTaskActor20 = new BehaviourTaskActor();
    behaviourTaskActor20.id = "ant_red_move";
    BehaviourTaskActor pAsset20 = behaviourTaskActor20;
    this.t = behaviourTaskActor20;
    this.add(pAsset20);
    this.t.setIcon("ui/Icons/iconAntRed");
    this.t.addBeh((BehaviourActionActor) new BehAntSetup());
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile4Directions());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    this.t.addBeh((BehaviourActionActor) new BehAntSwitchGround());
    BehaviourTaskActor behaviourTaskActor21 = new BehaviourTaskActor();
    behaviourTaskActor21.id = "ant_blue_move";
    BehaviourTaskActor pAsset21 = behaviourTaskActor21;
    this.t = behaviourTaskActor21;
    this.add(pAsset21);
    this.t.setIcon("ui/Icons/iconAntBlue");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile4Directions());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    this.t.addBeh((BehaviourActionActor) new BehBlueAntSwitchGround());
    BehaviourTaskActor behaviourTaskActor22 = new BehaviourTaskActor();
    behaviourTaskActor22.id = "ant_green_move";
    BehaviourTaskActor pAsset22 = behaviourTaskActor22;
    this.t = behaviourTaskActor22;
    this.add(pAsset22);
    this.t.setIcon("ui/Icons/iconAntGreen");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile4Directions());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    this.t.addBeh((BehaviourActionActor) new BehGreenAntSwitchGround());
    BehaviourTaskActor behaviourTaskActor23 = new BehaviourTaskActor();
    behaviourTaskActor23.id = "random_wait_short_1";
    behaviourTaskActor23.cancellable_by_socialize = true;
    behaviourTaskActor23.cancellable_by_reproduction = true;
    behaviourTaskActor23.locale_key = "task_unit_wait";
    BehaviourTaskActor pAsset23 = behaviourTaskActor23;
    this.t = behaviourTaskActor23;
    this.add(pAsset23);
    this.t.setIcon("ui/Icons/iconClock");
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.1f));
    BehaviourTaskActor behaviourTaskActor24 = new BehaviourTaskActor();
    behaviourTaskActor24.id = "investigate_curiosity";
    behaviourTaskActor24.cancellable_by_socialize = true;
    behaviourTaskActor24.cancellable_by_reproduction = true;
    behaviourTaskActor24.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset24 = behaviourTaskActor24;
    this.t = behaviourTaskActor24;
    this.add(pAsset24);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_inquisitive_nature");
    this.t.addBeh((BehaviourActionActor) new BehCheckCuriosityTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    this.t.addBeh((BehaviourActionActor) new BehFindRandomNeighbourTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor25 = new BehaviourTaskActor();
    behaviourTaskActor25.id = "random_animal_move";
    behaviourTaskActor25.cancellable_by_socialize = true;
    behaviourTaskActor25.cancellable_by_reproduction = true;
    behaviourTaskActor25.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset25 = behaviourTaskActor25;
    this.t = behaviourTaskActor25;
    this.add(pAsset25);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehAnimalFindTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 6f));
    BehaviourTaskActor behaviourTaskActor26 = new BehaviourTaskActor();
    behaviourTaskActor26.id = "random_move_towards_civ_building";
    behaviourTaskActor26.cancellable_by_socialize = true;
    behaviourTaskActor26.cancellable_by_reproduction = true;
    behaviourTaskActor26.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset26 = behaviourTaskActor26;
    this.t = behaviourTaskActor26;
    this.add(pAsset26);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomCivBuildingTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 4f));
    BehaviourTaskActor behaviourTaskActor27 = new BehaviourTaskActor();
    behaviourTaskActor27.id = "stay_in_random_house";
    behaviourTaskActor27.cancellable_by_socialize = true;
    behaviourTaskActor27.cancellable_by_reproduction = true;
    behaviourTaskActor27.locale_key = "task_unit_nothing";
    BehaviourTaskActor pAsset27 = behaviourTaskActor27;
    this.t = behaviourTaskActor27;
    this.add(pAsset27);
    this.t.setIcon("ui/Icons/iconBuildings");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("random_house_building"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(10f, 60f));
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    BehaviourTaskActor behaviourTaskActor28 = new BehaviourTaskActor();
    behaviourTaskActor28.id = "stay_in_own_home";
    behaviourTaskActor28.cancellable_by_socialize = true;
    behaviourTaskActor28.cancellable_by_reproduction = true;
    behaviourTaskActor28.locale_key = "task_unit_nothing";
    BehaviourTaskActor pAsset28 = behaviourTaskActor28;
    this.t = behaviourTaskActor28;
    this.add(pAsset28);
    this.t.setIcon("ui/Icons/iconHoused");
    this.t.addBeh((BehaviourActionActor) new BehBuildingTargetHome());
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(10f, 60f));
    this.t.addBeh((BehaviourActionActor) new BehRestoreStats(0.1f, 0.2f));
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    BehaviourTaskActor behaviourTaskActor29 = new BehaviourTaskActor();
    behaviourTaskActor29.id = "generate_loot";
    behaviourTaskActor29.cancellable_by_socialize = true;
    behaviourTaskActor29.cancellable_by_reproduction = true;
    behaviourTaskActor29.locale_key = "task_unit_nothing";
    BehaviourTaskActor pAsset29 = behaviourTaskActor29;
    this.t = behaviourTaskActor29;
    this.add(pAsset29);
    this.t.setIcon("ui/Icons/iconMoney");
    this.t.addBeh((BehaviourActionActor) new BehBuildingTargetHome());
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(2f, 6f));
    this.t.addBeh((BehaviourActionActor) new BehGenerateLootFromHouse());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    BehaviourTaskActor behaviourTaskActor30 = new BehaviourTaskActor();
    behaviourTaskActor30.id = "random_swim";
    behaviourTaskActor30.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset30 = behaviourTaskActor30;
    this.t = behaviourTaskActor30;
    this.add(pAsset30);
    this.t.setIcon("ui/Icons/iconTileShallowWater");
    this.t.addBeh((BehaviourActionActor) new BehRandomSwim());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 2f));
    BehaviourTaskActor behaviourTaskActor31 = new BehaviourTaskActor();
    behaviourTaskActor31.id = "make_decision";
    BehaviourTaskActor pAsset31 = behaviourTaskActor31;
    this.t = behaviourTaskActor31;
    this.add(pAsset31);
    this.t.setIcon("ui/Icons/actor_traits/iconStupid");
    this.t.addBeh((BehaviourActionActor) new BehMakeDecision());
    BehaviourTaskActor behaviourTaskActor32 = new BehaviourTaskActor();
    behaviourTaskActor32.id = "try_new_plot";
    behaviourTaskActor32.locale_key = "task_unit_plot";
    BehaviourTaskActor pAsset32 = behaviourTaskActor32;
    this.t = behaviourTaskActor32;
    this.add(pAsset32);
    this.t.setIcon("ui/Icons/iconPlot");
    this.t.addBeh((BehaviourActionActor) new BehTryNewPlot());
    BehaviourTaskActor behaviourTaskActor33 = new BehaviourTaskActor();
    behaviourTaskActor33.id = "check_plot";
    behaviourTaskActor33.locale_key = "task_unit_plot";
    BehaviourTaskActor pAsset33 = behaviourTaskActor33;
    this.t = behaviourTaskActor33;
    this.add(pAsset33);
    this.t.setIcon("ui/Icons/iconPlot");
    this.t.addBeh((BehaviourActionActor) new BehCheckPlot());
    BehaviourTaskActor behaviourTaskActor34 = new BehaviourTaskActor();
    behaviourTaskActor34.id = "progress_plot";
    behaviourTaskActor34.force_hand_tool = "coffee_cup";
    behaviourTaskActor34.locale_key = "task_unit_plot";
    BehaviourTaskActor pAsset34 = behaviourTaskActor34;
    this.t = behaviourTaskActor34;
    this.add(pAsset34);
    this.t.setIcon("ui/Icons/iconPlot");
    this.t.addBeh((BehaviourActionActor) new BehCheckPlotIsOk());
    this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    this.t.addBeh((BehaviourActionActor) new BehWait(2f));
    this.t.addBeh((BehaviourActionActor) new BehCheckPlotProgress());
    this.t.addBeh((BehaviourActionActor) new BehSpawnPlotProgressEffect());
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(0));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor35 = new BehaviourTaskActor();
    behaviourTaskActor35.id = "try_to_eat_city_food";
    behaviourTaskActor35.locale_key = "task_unit_eat";
    behaviourTaskActor35.diet = true;
    BehaviourTaskActor pAsset35 = behaviourTaskActor35;
    this.t = behaviourTaskActor35;
    this.add(pAsset35);
    this.t.setIcon("ui/Icons/iconHunger");
    this.t.addBeh((BehaviourActionActor) new BehCheckHasMoneyForCityFood());
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.1f));
    for (int index = 0; index < 3; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, string.Empty, 0.2f));
    this.t.addBeh((BehaviourActionActor) new BehTryToEatCityFood());
    for (int index = 0; index < 3; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, string.Empty, 0.2f));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 5f));
    BehaviourTaskActor behaviourTaskActor36 = new BehaviourTaskActor();
    behaviourTaskActor36.id = "find_house";
    BehaviourTaskActor pAsset36 = behaviourTaskActor36;
    this.t = behaviourTaskActor36;
    this.add(pAsset36);
    this.t.setIcon("ui/Icons/iconBuildings");
    this.t.addBeh((BehaviourActionActor) new BehFindHouse());
    BehaviourTaskActor behaviourTaskActor37 = new BehaviourTaskActor();
    behaviourTaskActor37.id = "random_move_near_house";
    behaviourTaskActor37.cancellable_by_socialize = true;
    behaviourTaskActor37.cancellable_by_reproduction = true;
    behaviourTaskActor37.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset37 = behaviourTaskActor37;
    this.t = behaviourTaskActor37;
    this.add(pAsset37);
    this.t.setIcon("ui/Icons/iconLivingHouse");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomFrontTileNearHouse());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 6f));
    BehaviourTaskActor behaviourTaskActor38 = new BehaviourTaskActor();
    behaviourTaskActor38.id = "end_job";
    BehaviourTaskActor pAsset38 = behaviourTaskActor38;
    this.t = behaviourTaskActor38;
    this.add(pAsset38);
    this.t.setIcon("ui/Icons/iconClose");
    this.t.addBeh((BehaviourActionActor) new BehEndJob());
    BehaviourTaskActor behaviourTaskActor39 = new BehaviourTaskActor();
    behaviourTaskActor39.id = "check_end_job";
    BehaviourTaskActor pAsset39 = behaviourTaskActor39;
    this.t = behaviourTaskActor39;
    this.add(pAsset39);
    this.t.setIcon("ui/Icons/iconClose");
    this.t.addBeh((BehaviourActionActor) new BehCheckEndCityActorJob());
    BehaviourTaskActor behaviourTaskActor40 = new BehaviourTaskActor();
    behaviourTaskActor40.id = "check_city_destroyed";
    BehaviourTaskActor pAsset40 = behaviourTaskActor40;
    this.t = behaviourTaskActor40;
    this.add(pAsset40);
    this.t.setIcon("ui/Icons/iconWar");
    this.t.addBeh((BehaviourActionActor) new BehCheckCityDestroyed());
    BehaviourTaskActor behaviourTaskActor41 = new BehaviourTaskActor();
    behaviourTaskActor41.id = "build_civ_city_here";
    BehaviourTaskActor pAsset41 = behaviourTaskActor41;
    this.t = behaviourTaskActor41;
    this.add(pAsset41);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehCheckBuildCity());
    this.t.addBeh((BehaviourActionActor) new BehEndJob());
    BehaviourTaskActor behaviourTaskActor42 = new BehaviourTaskActor();
    behaviourTaskActor42.id = "try_to_start_new_civilization";
    BehaviourTaskActor pAsset42 = behaviourTaskActor42;
    this.t = behaviourTaskActor42;
    this.add(pAsset42);
    this.t.setIcon("ui/Icons/iconKingdom");
    this.t.addBeh((BehaviourActionActor) new BehCheckEnemyNotNear());
    this.t.addBeh((BehaviourActionActor) new BehCheckStartCivilization());
    BehaviourTaskActor behaviourTaskActor43 = new BehaviourTaskActor();
    behaviourTaskActor43.id = "check_join_city";
    BehaviourTaskActor pAsset43 = behaviourTaskActor43;
    this.t = behaviourTaskActor43;
    this.add(pAsset43);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehJoinCity());
    this.t.addBeh((BehaviourActionActor) new BehEndJob());
    BehaviourTaskActor behaviourTaskActor44 = new BehaviourTaskActor();
    behaviourTaskActor44.id = "check_join_empty_nearby_city";
    BehaviourTaskActor pAsset44 = behaviourTaskActor44;
    this.t = behaviourTaskActor44;
    this.add(pAsset44);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehFindNearbyPotentialCivCityToJoin());
    BehaviourTaskActor behaviourTaskActor45 = new BehaviourTaskActor();
    behaviourTaskActor45.id = "try_to_read";
    behaviourTaskActor45.force_hand_tool = "book";
    behaviourTaskActor45.cancellable_by_reproduction = true;
    behaviourTaskActor45.cancellable_by_socialize = true;
    behaviourTaskActor45.locale_key = "task_unit_read";
    BehaviourTaskActor pAsset45 = behaviourTaskActor45;
    this.t = behaviourTaskActor45;
    this.add(pAsset45);
    this.t.setIcon("ui/Icons/iconBooks");
    this.t.addBeh((BehaviourActionActor) new BehTryToRead());
    for (int index1 = 0; index1 < 7; ++index1)
    {
      this.t.addBeh((BehaviourActionActor) new BehFindRandomNeighbourTile());
      this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
      this.t.addBeh((BehaviourActionActor) new BehSpawnHmmEffect());
      for (int index2 = 0; index2 < 5; ++index2)
        this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pTimerAction: 1f, pAngle: 6f));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      for (int index3 = 0; index3 < 5; ++index3)
        this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pTimerAction: 1f, pAngle: 6f));
    }
    this.t.addBeh((BehaviourActionActor) new BehFinishReading());
    BehaviourTaskActor behaviourTaskActor46 = new BehaviourTaskActor();
    behaviourTaskActor46.id = "citizen";
    BehaviourTaskActor pAsset46 = behaviourTaskActor46;
    this.t = behaviourTaskActor46;
    this.add(pAsset46);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 5f));
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    BehaviourTaskActor behaviourTaskActor47 = new BehaviourTaskActor();
    behaviourTaskActor47.id = "try_to_take_city_item";
    BehaviourTaskActor pAsset47 = behaviourTaskActor47;
    this.t = behaviourTaskActor47;
    this.add(pAsset47);
    this.t.setIcon("ui/Icons/items/icon_sword_wood");
    this.t.addBeh((BehaviourActionActor) new BehActorTryToTakeItemFromCity());
    BehaviourTaskActor behaviourTaskActor48 = new BehaviourTaskActor();
    behaviourTaskActor48.id = "city_idle_walking";
    behaviourTaskActor48.cancellable_by_socialize = true;
    behaviourTaskActor48.cancellable_by_reproduction = true;
    behaviourTaskActor48.locale_key = "task_unit_walk";
    BehaviourTaskActor pAsset48 = behaviourTaskActor48;
    this.t = behaviourTaskActor48;
    this.add(pAsset48);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehCityActorGetRandomIdleTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 6f));
    BehaviourTaskActor behaviourTaskActor49 = new BehaviourTaskActor();
    behaviourTaskActor49.id = "city_walking_to_danger_zone";
    behaviourTaskActor49.locale_key = "task_unit_investigate";
    BehaviourTaskActor pAsset49 = behaviourTaskActor49;
    this.t = behaviourTaskActor49;
    this.add(pAsset49);
    this.t.setIcon("ui/Icons/iconWar");
    this.t.addBeh((BehaviourActionActor) new BehCityActorGetRandomDangerZone());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 6f));
    BehaviourTaskActor behaviourTaskActor50 = new BehaviourTaskActor();
    behaviourTaskActor50.id = "find_city_job";
    BehaviourTaskActor pAsset50 = behaviourTaskActor50;
    this.t = behaviourTaskActor50;
    this.add(pAsset50);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindNewJob());
    BehaviourTaskActor behaviourTaskActor51 = new BehaviourTaskActor();
    behaviourTaskActor51.id = "give_tax";
    BehaviourTaskActor pAsset51 = behaviourTaskActor51;
    this.t = behaviourTaskActor51;
    this.add(pAsset51);
    this.t.setIcon("ui/Icons/iconTax");
    this.t.addBeh((BehaviourActionActor) new BehActorGiveTax());
    BehaviourTaskActor behaviourTaskActor52 = new BehaviourTaskActor();
    behaviourTaskActor52.id = "do_hunting";
    behaviourTaskActor52.cancellable_by_reproduction = true;
    behaviourTaskActor52.in_combat = true;
    behaviourTaskActor52.ignore_fight_check = true;
    behaviourTaskActor52.locale_key = "task_unit_hunt";
    BehaviourTaskActor pAsset52 = behaviourTaskActor52;
    this.t = behaviourTaskActor52;
    this.add(pAsset52);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobHunter");
    this.t.addBeh((BehaviourActionActor) new BehFindTargetForHunter());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(pCheckCanAttackTarget: true, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehAttackActorHuntingTarget());
    this.addActionsForDeliverResources(this.t);
    BehaviourTaskActor behaviourTaskActor53 = new BehaviourTaskActor();
    behaviourTaskActor53.id = "make_items";
    BehaviourTaskActor pAsset53 = behaviourTaskActor53;
    this.t = behaviourTaskActor53;
    this.add(pAsset53);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobBlacksmith");
    this.t.addBeh((BehaviourActionActor) new BehBuildingTargetHome());
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(10f, 15f));
    this.t.addBeh((BehaviourActionActor) new BehMakeItem());
    this.t.addBeh((BehaviourActionActor) new BehActorTryToTakeItemFromCity());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    BehaviourTaskActor behaviourTaskActor54 = new BehaviourTaskActor();
    behaviourTaskActor54.id = "cleaning";
    behaviourTaskActor54.force_hand_tool = "hammer";
    behaviourTaskActor54.cancellable_by_socialize = true;
    behaviourTaskActor54.cancellable_by_reproduction = true;
    behaviourTaskActor54.locale_key = "task_unit_cleaning_ruins";
    BehaviourTaskActor pAsset54 = behaviourTaskActor54;
    this.t = behaviourTaskActor54;
    this.add(pAsset54);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobCleaner");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("ruins"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.3f, 0.3f));
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 3; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Ruin, "event:/SFX/CIVILIZATIONS/CleanRuins", 1f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Ruin, "event:/SFX/CIVILIZATIONS/CleanRuins", pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehRemoveRuins());
    this.addActionsForDeliverResources(this.t);
    BehaviourTaskActor behaviourTaskActor55 = new BehaviourTaskActor();
    behaviourTaskActor55.id = "manure_cleaning";
    behaviourTaskActor55.force_hand_tool = "basket";
    behaviourTaskActor55.cancellable_by_reproduction = true;
    behaviourTaskActor55.locale_key = "task_unit_cleaning_poop";
    BehaviourTaskActor pAsset55 = behaviourTaskActor55;
    this.t = behaviourTaskActor55;
    this.add(pAsset55);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobCleaner");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_poop"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 5; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/CIVILIZATIONS/CollectHerbs"));
    this.t.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    this.addActionsForDeliverResources(this.t);
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(6));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor56 = new BehaviourTaskActor();
    behaviourTaskActor56.id = "put_out_fire";
    behaviourTaskActor56.force_hand_tool = "bucket";
    behaviourTaskActor56.locale_key = "task_unit_extinguish";
    behaviourTaskActor56.is_fireman = true;
    BehaviourTaskActor pAsset56 = behaviourTaskActor56;
    this.t = behaviourTaskActor56;
    this.add(pAsset56);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobFireman");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindFireZone());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindClosestFire());
    this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pAngle: -20f, pCheckFlip: false));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.1f, 0.5f));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile));
    this.t.addBeh((BehaviourActionActor) new BehCityActorRemoveFire());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.4f, 1.2f));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor57 = new BehaviourTaskActor();
    behaviourTaskActor57.id = "try_build_building";
    behaviourTaskActor57.force_hand_tool = "hammer";
    behaviourTaskActor57.locale_key = "task_unit_build";
    BehaviourTaskActor pAsset57 = behaviourTaskActor57;
    this.t = behaviourTaskActor57;
    this.add(pAsset57);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobBuilder");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("new_building"));
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("build_building", false, true));
    BehaviourTaskActor behaviourTaskActor58 = new BehaviourTaskActor();
    behaviourTaskActor58.id = "build_building";
    behaviourTaskActor58.force_hand_tool = "hammer";
    behaviourTaskActor58.locale_key = "task_unit_build";
    BehaviourTaskActor pAsset58 = behaviourTaskActor58;
    this.t = behaviourTaskActor58;
    this.add(pAsset58);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobBuilder");
    this.t.addBeh((BehaviourActionActor) new BehCheckStillUnderConstruction());
    this.t.addBeh((BehaviourActionActor) new BehFindConstructionTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 5; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehCheckStillUnderConstruction());
      this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Building, "event:/SFX/BUILDINGS/СonstructionBuildingGeneric", pLandIfHovering: true));
      this.t.addBeh((BehaviourActionActor) new BehBuildTarget());
    }
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor59 = new BehaviourTaskActor();
    behaviourTaskActor59.id = "build_road";
    behaviourTaskActor59.force_hand_tool = "hammer";
    behaviourTaskActor59.locale_key = "task_unit_build";
    BehaviourTaskActor pAsset59 = behaviourTaskActor59;
    this.t = behaviourTaskActor59;
    this.add(pAsset59);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobRoadBuilder");
    BehaviourTaskActor t = this.t;
    BehFindTile pAction = new BehFindTile(TileFinderType.NewRoad);
    pAction.null_check_city = true;
    t.addBeh((BehaviourActionActor) pAction);
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.2f));
    for (int index = 0; index < 3; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, "event:/SFX/CIVILIZATIONS/BuildRoad", 1f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, "event:/SFX/CIVILIZATIONS/BuildRoad", pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehCityActorCreateRoad());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(10));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor60 = new BehaviourTaskActor();
    behaviourTaskActor60.id = "collect_fruits";
    behaviourTaskActor60.force_hand_tool = "basket";
    behaviourTaskActor60.cancellable_by_reproduction = true;
    behaviourTaskActor60.locale_key = "task_unit_collect_food";
    BehaviourTaskActor pAsset60 = behaviourTaskActor60;
    this.t = behaviourTaskActor60;
    this.add(pAsset60);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobGathererBushes");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_fruits"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/CIVILIZATIONS/CollectFruits"));
    this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(0.0f, "event:/SFX/CIVILIZATIONS/CollectFruits"));
    this.t.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    this.addActionsForDeliverResources(this.t);
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(5));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor61 = new BehaviourTaskActor();
    behaviourTaskActor61.id = "collect_honey";
    behaviourTaskActor61.force_hand_tool = "basket";
    behaviourTaskActor61.cancellable_by_reproduction = true;
    behaviourTaskActor61.locale_key = "task_unit_collect_honey";
    BehaviourTaskActor pAsset61 = behaviourTaskActor61;
    this.t = behaviourTaskActor61;
    this.add(pAsset61);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobGathererHoney");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_hive"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, (string) null));
    this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(0.0f, (string) null));
    this.t.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    this.addActionsForDeliverResources(this.t);
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(3));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor62 = new BehaviourTaskActor();
    behaviourTaskActor62.id = "claim_land";
    behaviourTaskActor62.force_hand_tool = "flag";
    behaviourTaskActor62.cancellable_by_reproduction = true;
    behaviourTaskActor62.locale_key = "task_unit_claim_land";
    BehaviourTaskActor pAsset62 = behaviourTaskActor62;
    this.t = behaviourTaskActor62;
    this.add(pAsset62);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenLandClaimer");
    this.t.addBeh((BehaviourActionActor) new BehActorCheckZoneTarget());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 21; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.5f, 0.5f));
      this.t.addBeh((BehaviourActionActor) new BehSpawnCityBorderEffect());
    }
    this.t.addBeh((BehaviourActionActor) new BehClaimZoneForCityActorBorder());
    this.t.addBeh((BehaviourActionActor) new BehSpawnCityBorderEffect(5));
    this.t.addBeh((BehaviourActionActor) new BehEndJob());
    BehaviourTaskActor behaviourTaskActor63 = new BehaviourTaskActor();
    behaviourTaskActor63.id = "collect_herbs";
    behaviourTaskActor63.force_hand_tool = "basket";
    behaviourTaskActor63.cancellable_by_reproduction = true;
    behaviourTaskActor63.locale_key = "task_unit_collect_herbs";
    BehaviourTaskActor pAsset63 = behaviourTaskActor63;
    this.t = behaviourTaskActor63;
    this.add(pAsset63);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobGathererHerbs");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_vegetation"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/CIVILIZATIONS/CollectHerbs"));
    this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(0.0f, "event:/SFX/CIVILIZATIONS/CollectHerbs"));
    this.t.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    this.addActionsForDeliverResources(this.t);
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(4));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor64 = new BehaviourTaskActor();
    behaviourTaskActor64.id = "chop_trees";
    behaviourTaskActor64.force_hand_tool = "axe";
    behaviourTaskActor64.cancellable_by_reproduction = true;
    behaviourTaskActor64.locale_key = "task_unit_chop";
    BehaviourTaskActor pAsset64 = behaviourTaskActor64;
    this.t = behaviourTaskActor64;
    this.add(pAsset64);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobWoodcutter");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_tree"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 7; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/CIVILIZATIONS/ChopTree"));
    this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(0.0f, "event:/SFX/CIVILIZATIONS/ChopTree"));
    this.t.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    this.addActionsForDeliverResources(this.t);
    BehaviourTaskActor behaviourTaskActor65 = new BehaviourTaskActor();
    behaviourTaskActor65.id = "mine_deposit";
    behaviourTaskActor65.force_hand_tool = "pickaxe";
    behaviourTaskActor65.cancellable_by_reproduction = true;
    behaviourTaskActor65.locale_key = "task_unit_mine";
    BehaviourTaskActor pAsset65 = behaviourTaskActor65;
    this.t = behaviourTaskActor65;
    this.add(pAsset65);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobMinerDeposit");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_mineral"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 6; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/CIVILIZATIONS/MiningMineral"));
    this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(0.0f, "event:/SFX/CIVILIZATIONS/MiningMineral"));
    this.t.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    this.addActionsForDeliverResources(this.t);
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(3));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor66 = new BehaviourTaskActor();
    behaviourTaskActor66.id = "farmer_make_field";
    behaviourTaskActor66.force_hand_tool = "hoe";
    behaviourTaskActor66.cancellable_by_reproduction = true;
    behaviourTaskActor66.locale_key = "task_unit_farm";
    BehaviourTaskActor pAsset66 = behaviourTaskActor66;
    this.t = behaviourTaskActor66;
    this.add(pAsset66);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobFarmer");
    this.t.addBeh((BehaviourActionActor) new BehFindTileForFarm());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 6; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, "event:/SFX/CIVILIZATIONS/MakeFarmField", 1f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehMakeFarm());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor67 = new BehaviourTaskActor();
    behaviourTaskActor67.id = "farmer_plant_crops";
    behaviourTaskActor67.force_hand_tool = "hoe";
    behaviourTaskActor67.cancellable_by_reproduction = true;
    behaviourTaskActor67.locale_key = "task_unit_farm";
    BehaviourTaskActor pAsset67 = behaviourTaskActor67;
    this.t = behaviourTaskActor67;
    this.add(pAsset67);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobFarmer");
    this.t.addBeh((BehaviourActionActor) new BehFindFarmField());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, "event:/SFX/CIVILIZATIONS/PlantCrops", 1f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehPlantCrops());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(6));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor68 = new BehaviourTaskActor();
    behaviourTaskActor68.id = "farmer_harvest";
    behaviourTaskActor68.force_hand_tool = "hoe";
    behaviourTaskActor68.locale_key = "task_unit_farm";
    BehaviourTaskActor pAsset68 = behaviourTaskActor68;
    this.t = behaviourTaskActor68;
    this.add(pAsset68);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobFarmer");
    this.t.addBeh((BehaviourActionActor) new BehFindWheat());
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 3; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/CIVILIZATIONS/HarvestCrops"));
    this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(0.0f, "event:/SFX/CIVILIZATIONS/HarvestCrops"));
    this.t.addBeh((BehaviourActionActor) new BehExtractResourcesFromBuilding());
    this.addActionsForDeliverResources(this.t, true);
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(6));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor69 = new BehaviourTaskActor();
    behaviourTaskActor69.id = "farmer_fertilize_crops";
    behaviourTaskActor69.force_hand_tool = "bucket";
    behaviourTaskActor69.cancellable_by_reproduction = true;
    behaviourTaskActor69.locale_key = "task_unit_farm";
    BehaviourTaskActor pAsset69 = behaviourTaskActor69;
    this.t = behaviourTaskActor69;
    this.add(pAsset69);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobFarmer");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindStorage());
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Building, pTimerAction: 1f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehCityActorGetResourceFromStorage("fertilizer", 5));
    for (int index = 0; index < 5; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehCityActorFindUngrownCrop());
      this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Building, pLandIfHovering: true));
      this.t.addBeh((BehaviourActionActor) new BehThrowResourceAnimation("fertilizer"));
      this.t.addBeh((BehaviourActionActor) new BehWait());
      this.t.addBeh((BehaviourActionActor) new BehCityActorFertilizeCrop());
    }
    this.addActionsForDeliverResources(this.t);
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(6));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor70 = new BehaviourTaskActor();
    behaviourTaskActor70.id = "farmer_random_move";
    behaviourTaskActor70.force_hand_tool = "hoe";
    behaviourTaskActor70.locale_key = "task_unit_farm";
    BehaviourTaskActor pAsset70 = behaviourTaskActor70;
    this.t = behaviourTaskActor70;
    this.add(pAsset70);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobFarmer");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomFarmTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    BehaviourTaskActor behaviourTaskActor71 = new BehaviourTaskActor();
    behaviourTaskActor71.id = "store_resources";
    behaviourTaskActor71.locale_key = "task_unit_store_resources";
    BehaviourTaskActor pAsset71 = behaviourTaskActor71;
    this.t = behaviourTaskActor71;
    this.add(pAsset71);
    this.t.setIcon("ui/Icons/iconCityInventory");
    this.addActionsForDeliverResources(this.t);
    BehaviourTaskActor behaviourTaskActor72 = new BehaviourTaskActor();
    behaviourTaskActor72.id = "mine";
    behaviourTaskActor72.force_hand_tool = "pickaxe";
    behaviourTaskActor72.locale_key = "task_unit_mine";
    BehaviourTaskActor pAsset72 = behaviourTaskActor72;
    this.t = behaviourTaskActor72;
    this.add(pAsset72);
    this.t.setIcon("ui/Icons/citizen_jobs/iconCitizenJobMiner");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_mine"));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(10f, 15f));
    this.t.addBeh((BehaviourActionActor) new BehGetResourcesFromMine());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    this.t.addBeh((BehaviourActionActor) new BehCheckNeeds(3));
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor73 = new BehaviourTaskActor();
    behaviourTaskActor73.id = "try_to_return_to_home_city";
    behaviourTaskActor73.flag_boat_related = true;
    BehaviourTaskActor pAsset73 = behaviourTaskActor73;
    this.t = behaviourTaskActor73;
    this.add(pAsset73);
    this.t.setIcon("ui/Icons/iconHoused");
    this.t.addBeh((BehaviourActionActor) new BehTaxiCheck());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f));
    this.t.addBeh((BehaviourActionActor) new BehTaxiFindShipTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehTaxiEmbark());
    BehaviourTaskActor behaviourTaskActor74 = new BehaviourTaskActor();
    behaviourTaskActor74.id = "force_into_a_boat";
    behaviourTaskActor74.flag_boat_related = true;
    BehaviourTaskActor pAsset74 = behaviourTaskActor74;
    this.t = behaviourTaskActor74;
    this.add(pAsset74);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehTaxiFindShipTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehTaxiEmbark());
    BehaviourTaskActor behaviourTaskActor75 = new BehaviourTaskActor();
    behaviourTaskActor75.id = "embark_into_boat";
    behaviourTaskActor75.flag_boat_related = true;
    BehaviourTaskActor pAsset75 = behaviourTaskActor75;
    this.t = behaviourTaskActor75;
    this.add(pAsset75);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehTaxiEmbark());
    BehaviourTaskActor behaviourTaskActor76 = new BehaviourTaskActor();
    behaviourTaskActor76.id = "sit_inside_boat";
    behaviourTaskActor76.flag_boat_related = true;
    BehaviourTaskActor pAsset76 = behaviourTaskActor76;
    this.t = behaviourTaskActor76;
    this.add(pAsset76);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehTaxiSitInside());
    this.t.addBeh((BehaviourActionActor) new BehTaxiSitInside());
    this.t.addBeh((BehaviourActionActor) new BehTaxiSitInside());
    BehaviourTaskActor behaviourTaskActor77 = new BehaviourTaskActor();
    behaviourTaskActor77.id = "wait";
    behaviourTaskActor77.cancellable_by_socialize = true;
    behaviourTaskActor77.cancellable_by_reproduction = true;
    behaviourTaskActor77.locale_key = "task_unit_wait";
    BehaviourTaskActor pAsset77 = behaviourTaskActor77;
    this.t = behaviourTaskActor77;
    this.add(pAsset77);
    this.t.setIcon("ui/Icons/iconClock");
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f, 1.3f));
    BehaviourTaskActor behaviourTaskActor78 = new BehaviourTaskActor();
    behaviourTaskActor78.id = "wait5";
    behaviourTaskActor78.cancellable_by_socialize = true;
    behaviourTaskActor78.cancellable_by_reproduction = true;
    behaviourTaskActor78.locale_key = "task_unit_wait";
    BehaviourTaskActor pAsset78 = behaviourTaskActor78;
    this.t = behaviourTaskActor78;
    this.add(pAsset78);
    this.t.setIcon("ui/Icons/iconClock");
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 5f));
    BehaviourTaskActor behaviourTaskActor79 = new BehaviourTaskActor();
    behaviourTaskActor79.id = "wait10";
    behaviourTaskActor79.cancellable_by_socialize = true;
    behaviourTaskActor79.cancellable_by_reproduction = true;
    behaviourTaskActor79.locale_key = "task_unit_wait";
    BehaviourTaskActor pAsset79 = behaviourTaskActor79;
    this.t = behaviourTaskActor79;
    this.add(pAsset79);
    this.t.setIcon("ui/Icons/iconClock");
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 10f));
    BehaviourTaskActor behaviourTaskActor80 = new BehaviourTaskActor();
    behaviourTaskActor80.id = "replenish_energy";
    behaviourTaskActor80.cancellable_by_socialize = false;
    behaviourTaskActor80.cancellable_by_reproduction = true;
    behaviourTaskActor80.locale_key = "task_unit_replenish_energy";
    BehaviourTaskActor pAsset80 = behaviourTaskActor80;
    this.t = behaviourTaskActor80;
    this.add(pAsset80);
    this.t.setIcon("ui/Icons/iconStamina");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_well", false, false));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 5; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Building, pTimerAction: 1f, pAngle: 20f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehReplenishEnergy());
    BehaviourTaskActor behaviourTaskActor81 = new BehaviourTaskActor();
    behaviourTaskActor81.id = "repair_equipment";
    behaviourTaskActor81.cancellable_by_socialize = false;
    behaviourTaskActor81.cancellable_by_reproduction = true;
    behaviourTaskActor81.locale_key = "task_unit_repair_equipment";
    BehaviourTaskActor pAsset81 = behaviourTaskActor81;
    this.t = behaviourTaskActor81;
    this.add(pAsset81);
    this.t.setIcon("ui/Icons/iconReforge");
    this.t.addBeh((BehaviourActionActor) new BehCheckCanRepairEquipment());
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_barracks", false, false));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 5; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Building, pTimerAction: 1f, pAngle: 20f));
    this.t.addBeh((BehaviourActionActor) new BehRepairEquipment());
  }

  private void initTasksThinkingReflectionHappiness()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "try_to_steal_money";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/actor_traits/iconThief");
    this.t.addBeh((BehaviourActionActor) new BehFindTargetToStealFrom());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(GoToActorTargetType.NearbyTileClosest, pCalibrateTargetPosition: true));
    for (int index = 0; index < 2; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Actor, pTimerAction: 0.3f, pAngle: 30f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehStealFromTarget());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("run_away"));
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "reflection";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconBre");
    this.t.addBeh((BehaviourActionActor) new BehReflection());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "madness_random_emotion";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/actor_traits/iconMadness");
    this.t.addBeh((BehaviourActionActor) new BehMadnessRandomEmotion());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "happy_laughing";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconLaughing");
    this.t.addBeh((BehaviourActionActor) new BehTryFindTargetWithStatusNearby(new string[4]
    {
      "laughing",
      "crying",
      "swearing",
      "singing"
    }));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index1 = 0; index1 < 4; ++index1)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("laughing", 2f, false));
      for (int index2 = 0; index2 < 3; ++index2)
        this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pTimerAction: 0.2f, pAngle: -20f, pCheckFlip: false));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehWait(0.3f));
    }
    this.t.addBeh((BehaviourActionActor) new BehAddHappiness("just_laughed"));
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "singing";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconSinging");
    this.t.addBeh((BehaviourActionActor) new BehTryFindTargetWithStatusNearby(new string[4]
    {
      "laughing",
      "crying",
      "swearing",
      "singing"
    }));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index3 = 0; index3 < 5; ++index3)
    {
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.0f, 5f));
      this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("singing", 3f, false));
      for (int index4 = 0; index4 < 3; ++index4)
      {
        this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pTimerAction: 0.4f, pAngle: -30f, pCheckFlip: false));
        this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
        this.t.addBeh((BehaviourActionActor) new BehWait(0.1f));
      }
    }
    this.t.addBeh((BehaviourActionActor) new BehAddHappiness("just_sang"));
    this.t.addBeh((BehaviourActionActor) new BehFinishSinging());
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "swearing";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconSwearing");
    this.t.addBeh((BehaviourActionActor) new BehTryFindTargetWithStatusNearby(new string[4]
    {
      "laughing",
      "crying",
      "swearing",
      "singing"
    }));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index5 = 0; index5 < 4; ++index5)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("swearing", 2f, false));
      for (int index6 = 0; index6 < 3; ++index6)
        this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pTimerAction: 0.2f, pAngle: -20f, pCheckFlip: false));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehWait(0.3f));
    }
    this.t.addBeh((BehaviourActionActor) new BehAddHappiness("just_swore"));
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "crying";
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconCrying");
    this.t.addBeh((BehaviourActionActor) new BehTryFindTargetWithStatusNearby(new string[4]
    {
      "laughing",
      "crying",
      "swearing",
      "singing"
    }));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index7 = 0; index7 < 4; ++index7)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("crying", 2f, false));
      for (int index8 = 0; index8 < 3; ++index8)
        this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pTimerAction: 0.2f, pAngle: -20f, pCheckFlip: false));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehWait(0.3f));
    }
    this.t.addBeh((BehaviourActionActor) new BehAddHappiness("just_cried"));
    BehaviourTaskActor behaviourTaskActor8 = new BehaviourTaskActor();
    behaviourTaskActor8.id = "possessed_following";
    BehaviourTaskActor pAsset8 = behaviourTaskActor8;
    this.t = behaviourTaskActor8;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/iconPossessed");
    this.t.addBeh((BehaviourActionActor) new BehTryFindTargetWithStatusNearby(new string[1]
    {
      "possessed"
    }));
    this.t.addBeh((BehaviourActionActor) new BehCopyAggro());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehFindRandomNeighbourTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f, 2f));
    this.t.addBeh((BehaviourActionActor) new BehRandomSocializeTopic(1.5f, 3f, 0.1f));
    BehaviourTaskActor behaviourTaskActor9 = new BehaviourTaskActor();
    behaviourTaskActor9.id = "start_tantrum";
    BehaviourTaskActor pAsset9 = behaviourTaskActor9;
    this.t = behaviourTaskActor9;
    this.add(pAsset9);
    this.t.setIcon("ui/Icons/iconTantrum");
    for (int index = 0; index < 6; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.1f, 0.1f));
    }
    this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("tantrum", 60f));
    BehaviourTaskActor behaviourTaskActor10 = new BehaviourTaskActor();
    behaviourTaskActor10.id = "do_tantrum";
    BehaviourTaskActor pAsset10 = behaviourTaskActor10;
    this.t = behaviourTaskActor10;
    this.add(pAsset10);
    this.t.setIcon("ui/Icons/iconTantrum");
    this.t.addBeh((BehaviourActionActor) new BehFindTantrumTarget());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(GoToActorTargetType.RaycastWithAttackRange, pCheckCanAttackTarget: true, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehAddAggroForBehTarget());
    BehaviourTaskActor behaviourTaskActor11 = new BehaviourTaskActor();
    behaviourTaskActor11.id = "punch_a_tree";
    BehaviourTaskActor pAsset11 = behaviourTaskActor11;
    this.t = behaviourTaskActor11;
    this.add(pAsset11);
    this.t.setIcon("ui/Icons/iconRage");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_tree", true, false));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 2; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/HIT/HitWood"));
    this.t.addBeh((BehaviourActionActor) new BehGetDamaged(1, AttackType.Gravity));
    this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("crying", 5f, false, true));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor12 = new BehaviourTaskActor();
    behaviourTaskActor12.id = "punch_a_building";
    BehaviourTaskActor pAsset12 = behaviourTaskActor12;
    this.t = behaviourTaskActor12;
    this.add(pAsset12);
    this.t.setIcon("ui/Icons/iconRage");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_house", false, false));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 2; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/HIT/HitGeneric"));
    this.t.addBeh((BehaviourActionActor) new BehGetDamaged(1, AttackType.Gravity));
    this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("crying", 5f, false, true));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor13 = new BehaviourTaskActor();
    behaviourTaskActor13.id = "start_fire";
    BehaviourTaskActor pAsset13 = behaviourTaskActor13;
    this.t = behaviourTaskActor13;
    this.add(pAsset13);
    this.t.setIcon("ui/Icons/iconFire");
    this.t.addBeh((BehaviourActionActor) new BehWait());
  }

  private void initTasksClanLeader()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "kill_unruly_clan_members";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/clan_traits/clan_trait_deathbound");
    this.t.addBeh((BehaviourActionActor) new BehClanChiefCheckMembersToKill());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "banish_unruly_clan_members";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/clan_traits/clan_trait_blood_pact");
    this.t.addBeh((BehaviourActionActor) new BehClanChiefCheckMembersToBanish());
  }

  private void initTasksPoop()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "try_to_poop";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconPoop");
    this.t.addBeh((BehaviourActionActor) new BehDecideWhereToPoop());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "try_to_launch_fireworks";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconFireworks");
    this.t.addBeh((BehaviourActionActor) new BehTryFindTargetWithStatusNearby(new string[4]
    {
      "laughing",
      "crying",
      "swearing",
      "singing"
    }));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index1 = 0; index1 < 1; ++index1)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("laughing", 2f, false));
      for (int index2 = 0; index2 < 3; ++index2)
        this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Nothing, pTimerAction: 0.2f, pAngle: -20f, pCheckFlip: false, pLandIfHovering: true));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehWait(0.3f));
    }
    this.t.addBeh((BehaviourActionActor) new BehLaunchFireworks());
    this.t.addBeh((BehaviourActionActor) new BehAddHappiness("just_laughed"));
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "poop_inside";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconPoop");
    this.t.addBeh((BehaviourActionActor) new BehBuildingTargetHome());
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(10f, 60f));
    this.t.addBeh((BehaviourActionActor) new BehPoopInside());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "poop_outside";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconPoop");
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.FreeTile));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.5f, 0.5f));
    }
    this.t.addBeh((BehaviourActionActor) new BehPoopOutside());
  }

  private void initTasksSleep()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "decide_where_to_sleep";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconSleep");
    this.t.addBeh((BehaviourActionActor) new BehDecideWhereToSleep());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "sleep_inside";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconHoused");
    this.t.addBeh((BehaviourActionActor) new BehBuildingTargetHome());
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget());
    this.t.addBeh((BehaviourActionActor) new BehTrySleep());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "sleep_outside";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconHomeless");
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.FreeTile));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 2f));
    this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 2f));
    this.t.addBeh((BehaviourActionActor) new BehFindRandomNeighbourTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 2f));
    this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 2f));
    this.t.addBeh((BehaviourActionActor) new BehTrySleep(true));
  }

  private void initTasksKings()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "king_check_new_city_foundation";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehKingCheckNewCityFoundation());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "king_change_kingdom_language";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconLanguage");
    this.t.addBeh((BehaviourActionActor) new BehChangeKingdomLanguage());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "king_change_kingdom_culture";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconCulture");
    this.t.addBeh((BehaviourActionActor) new BehChangeKingdomCulture());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "king_change_kingdom_religion";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconReligion");
    this.t.addBeh((BehaviourActionActor) new BehChangeKingdomReligion());
  }

  private void initTasksWarriors()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "warrior_random_move";
    behaviourTaskActor1.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "warrior_army_captain_idle_walking_city";
    behaviourTaskActor2.cancellable_by_socialize = true;
    behaviourTaskActor2.cancellable_by_reproduction = true;
    behaviourTaskActor2.locale_key = "task_unit_walk";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehCityActorGetRandomBorderTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 6f));
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "warrior_army_captain_waiting";
    behaviourTaskActor3.cancellable_by_socialize = true;
    behaviourTaskActor3.cancellable_by_reproduction = true;
    behaviourTaskActor3.locale_key = "task_unit_walk";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehCityActorGetRandomBorderTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(10f, 20f));
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "check_warrior_limit";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconWar");
    this.t.addBeh((BehaviourActionActor) new BehCheckCityActorWarriorLimit());
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "warrior_try_join_army_group";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconWar");
    this.t.addBeh((BehaviourActionActor) new BehCheckCityActorArmyGroup());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "warrior_army_leader_move_random";
    behaviourTaskActor6.speed_multiplier = 0.8f;
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(10f, 20f));
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "warrior_army_leader_move_to_attack_target";
    behaviourTaskActor7.speed_multiplier = 0.8f;
    behaviourTaskActor7.cancellable_by_socialize = false;
    behaviourTaskActor7.cancellable_by_reproduction = false;
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconArrowAttackTarget");
    this.t.addTaskVerifier((BehaviourActionActor) new BehVerifierAttackZone());
    this.t.addBeh((BehaviourActionActor) new BehCityActorCheckAttack());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      limit_pathfinding_regions = 6
    });
    this.t.addBeh((BehaviourActionActor) new BehWarriorCaptainWait());
    this.t.addBeh((BehaviourActionActor) new BehRestartTask());
    BehaviourTaskActor behaviourTaskActor8 = new BehaviourTaskActor();
    behaviourTaskActor8.id = "warrior_army_follow_leader";
    behaviourTaskActor8.speed_multiplier = 1.3f;
    behaviourTaskActor8.cancellable_by_socialize = false;
    behaviourTaskActor8.cancellable_by_reproduction = false;
    BehaviourTaskActor pAsset8 = behaviourTaskActor8;
    this.t = behaviourTaskActor8;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/iconLoyalty");
    this.t.addBeh((BehaviourActionActor) new BehFindTileNearbyGroupLeader());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor9 = new BehaviourTaskActor();
    behaviourTaskActor9.id = "check_warrior_transport";
    behaviourTaskActor9.flag_boat_related = true;
    behaviourTaskActor9.cancellable_by_socialize = false;
    behaviourTaskActor9.cancellable_by_reproduction = false;
    BehaviourTaskActor pAsset9 = behaviourTaskActor9;
    this.t = behaviourTaskActor9;
    this.add(pAsset9);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehCityActorWarriorTaxiCheck());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 5f));
    this.t.addBeh((BehaviourActionActor) new BehTaxiFindShipTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehTaxiEmbark());
    BehaviourTaskActor behaviourTaskActor10 = new BehaviourTaskActor();
    behaviourTaskActor10.id = "warrior_train_with_dummy";
    BehaviourTaskActor pAsset10 = behaviourTaskActor10;
    this.t = behaviourTaskActor10;
    this.add(pAsset10);
    this.t.setIcon("ui/Icons/iconWarfare");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_training_dummies"));
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    for (int index = 0; index < 10; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Building, "event:/SFX/BUILDINGS/DestroyBuildingWood", pLandIfHovering: true));
      this.t.addBeh((BehaviourActionActor) new BehActorAddExperience(0, 2));
      this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 2f));
      this.t.addBeh((BehaviourActionActor) new BehDealDamageToTargetBuilding(0.05f, 0.15f));
    }
    this.t.addBeh((BehaviourActionActor) new BehActorTryToAddRandomCombatSkill());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 6f));
  }

  private void initTasksLeaders()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "leader_change_city_language";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconLanguage");
    this.t.addBeh((BehaviourActionActor) new BehChangeCityActorLanguage());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "leader_change_city_culture";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconCulture");
    this.t.addBeh((BehaviourActionActor) new BehChangeCityActorCulture());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "leader_change_city_religion";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconReligion");
    this.t.addBeh((BehaviourActionActor) new BehChangeCityActorReligion());
  }

  private void initTasksStatusRelated()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "strange_urge_finish";
    behaviourTaskActor1.locale_key = "task_strange_urge_finish";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconStrangeUrge");
    for (int index = 0; index < 6; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehFindRandomNeighbourTile());
      this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
      this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f, 5f));
    }
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "status_confused";
    behaviourTaskActor2.locale_key = "task_unit_confused";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconConfused");
    for (int index = 0; index < 6; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehFindRandomNeighbourTile());
      this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
      this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 0.01f));
    }
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "status_soul_harvested";
    behaviourTaskActor3.locale_key = "status_title_soul_harvested";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconSoulHarvested");
    this.t.addBeh((BehaviourActionActor) new BehStartShake());
    for (int index = 0; index < 12; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.1f, 0.1f));
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckSoulBorneReproduction());
  }

  private void initTasksMobs()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "diet_tiles";
    behaviourTaskActor1.cancellable_by_socialize = true;
    behaviourTaskActor1.cancellable_by_reproduction = true;
    behaviourTaskActor1.locale_key = "task_unit_eat";
    behaviourTaskActor1.diet = true;
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_geophagy");
    this.t.addBeh((BehaviourActionActor) new BehFindTileForEating());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true,
      walk_on_blocks = true
    });
    for (int index = 0; index < 10; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, string.Empty, 1f, 20f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehConsumeTargetTile());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "diet_wood";
    behaviourTaskActor2.cancellable_by_socialize = true;
    behaviourTaskActor2.cancellable_by_reproduction = true;
    behaviourTaskActor2.locale_key = "task_unit_eat";
    behaviourTaskActor2.diet = true;
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_xylophagy");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_tree", false, true));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/NATURE/AnimalEatPlant"));
    this.t.addBeh((BehaviourActionActor) new BehConsumeTargetBuilding());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "diet_minerals";
    behaviourTaskActor3.cancellable_by_socialize = true;
    behaviourTaskActor3.cancellable_by_reproduction = true;
    behaviourTaskActor3.locale_key = "task_unit_eat";
    behaviourTaskActor3.diet = true;
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_lithotroph");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_mineral", false, true));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/NATURE/AnimalEatPlant"));
    this.t.addBeh((BehaviourActionActor) new BehConsumeTargetBuilding());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "diet_fruits";
    behaviourTaskActor4.cancellable_by_socialize = true;
    behaviourTaskActor4.cancellable_by_reproduction = true;
    behaviourTaskActor4.locale_key = "task_unit_eat";
    behaviourTaskActor4.diet = true;
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_frugivore");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_fruits", false, true));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/NATURE/AnimalEatPlant"));
    this.t.addBeh((BehaviourActionActor) new BehConsumeTargetBuilding());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "diet_vegetation";
    behaviourTaskActor5.cancellable_by_socialize = true;
    behaviourTaskActor5.cancellable_by_reproduction = true;
    behaviourTaskActor5.locale_key = "task_unit_eat";
    behaviourTaskActor5.diet = true;
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_folivore");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_vegetation", false, true));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/NATURE/AnimalEatPlant"));
    this.t.addBeh((BehaviourActionActor) new BehConsumeTargetBuilding());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "diet_flowers";
    behaviourTaskActor6.cancellable_by_socialize = true;
    behaviourTaskActor6.cancellable_by_reproduction = true;
    behaviourTaskActor6.locale_key = "task_unit_eat";
    behaviourTaskActor6.diet = true;
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_florivore");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_flower", false, true));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/NATURE/AnimalEatPlant"));
    this.t.addBeh((BehaviourActionActor) new BehConsumeTargetBuilding());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "diet_nectar";
    behaviourTaskActor7.cancellable_by_socialize = true;
    behaviourTaskActor7.cancellable_by_reproduction = true;
    behaviourTaskActor7.locale_key = "task_unit_eat";
    behaviourTaskActor7.diet = true;
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_florivore");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_flower", false, true));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/NATURE/AnimalEatPlant"));
    this.t.addBeh((BehaviourActionActor) new BehNectarNectarFromFlower());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor8 = new BehaviourTaskActor();
    behaviourTaskActor8.id = "diet_crops";
    behaviourTaskActor8.cancellable_by_socialize = true;
    behaviourTaskActor8.cancellable_by_reproduction = true;
    behaviourTaskActor8.locale_key = "task_unit_eat";
    behaviourTaskActor8.diet = true;
    BehaviourTaskActor pAsset8 = behaviourTaskActor8;
    this.t = behaviourTaskActor8;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_granivore");
    this.t.addBeh((BehaviourActionActor) new BehFindBuilding("type_crops", false, true));
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehResourceGatheringAnimation(1f, "event:/SFX/NATURE/AnimalEatPlant"));
    this.t.addBeh((BehaviourActionActor) new BehConsumeTargetBuilding());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor9 = new BehaviourTaskActor();
    behaviourTaskActor9.id = "diet_grass";
    behaviourTaskActor9.cancellable_by_socialize = true;
    behaviourTaskActor9.cancellable_by_reproduction = true;
    behaviourTaskActor9.locale_key = "task_unit_eat";
    behaviourTaskActor9.diet = true;
    BehaviourTaskActor pAsset9 = behaviourTaskActor9;
    this.t = behaviourTaskActor9;
    this.add(pAsset9);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_graminivore");
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.Grass));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, string.Empty, 1f, 20f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehConsumeGrass());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor10 = new BehaviourTaskActor();
    behaviourTaskActor10.id = "diet_meat";
    behaviourTaskActor10.cancellable_by_reproduction = true;
    behaviourTaskActor10.ignore_fight_check = true;
    behaviourTaskActor10.locale_key = "task_unit_eat";
    behaviourTaskActor10.diet = true;
    BehaviourTaskActor pAsset10 = behaviourTaskActor10;
    this.t = behaviourTaskActor10;
    this.add(pAsset10);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_carnivore");
    this.t.addBeh((BehaviourActionActor) new BehFindMeatSource());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(pCheckCanAttackTarget: true, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehConsumeActorTarget());
    BehaviourTaskActor behaviourTaskActor11 = new BehaviourTaskActor();
    behaviourTaskActor11.id = "diet_blood";
    behaviourTaskActor11.cancellable_by_reproduction = true;
    behaviourTaskActor11.ignore_fight_check = true;
    behaviourTaskActor11.locale_key = "task_unit_eat";
    behaviourTaskActor11.diet = true;
    BehaviourTaskActor pAsset11 = behaviourTaskActor11;
    this.t = behaviourTaskActor11;
    this.add(pAsset11);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_hematophagy");
    this.t.addBeh((BehaviourActionActor) new BehFindMeatSource(pCheckForFactions: false));
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(pCheckCanAttackTarget: true, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehConsumeActorsBloodTarget());
    BehaviourTaskActor behaviourTaskActor12 = new BehaviourTaskActor();
    behaviourTaskActor12.id = "diet_meat_insect";
    behaviourTaskActor12.locale_key = "task_unit_eat";
    behaviourTaskActor12.diet = true;
    BehaviourTaskActor pAsset12 = behaviourTaskActor12;
    this.t = behaviourTaskActor12;
    this.add(pAsset12);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_insectivore");
    this.t.addBeh((BehaviourActionActor) new BehFindMeatInsectSource());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(pCheckCanAttackTarget: true, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehConsumeActorTarget());
    BehaviourTaskActor behaviourTaskActor13 = new BehaviourTaskActor();
    behaviourTaskActor13.id = "diet_same_species";
    behaviourTaskActor13.locale_key = "task_unit_eat";
    behaviourTaskActor13.diet = true;
    BehaviourTaskActor pAsset13 = behaviourTaskActor13;
    this.t = behaviourTaskActor13;
    this.add(pAsset13);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_cannibalism");
    this.t.addBeh((BehaviourActionActor) new BehFindMeatSameSpeciesSource(false));
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(pCheckCanAttackTarget: true, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehConsumeActorTarget());
    BehaviourTaskActor behaviourTaskActor14 = new BehaviourTaskActor();
    behaviourTaskActor14.id = "diet_algae";
    behaviourTaskActor14.locale_key = "task_unit_eat";
    behaviourTaskActor14.diet = true;
    BehaviourTaskActor pAsset14 = behaviourTaskActor14;
    this.t = behaviourTaskActor14;
    this.add(pAsset14);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_algivore");
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.Water));
    this.t.addBeh((BehaviourActionActor) new BehGoOrSwimToTileTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, string.Empty, 1f, 20f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehReplenishNutrition());
    BehaviourTaskActor behaviourTaskActor15 = new BehaviourTaskActor();
    behaviourTaskActor15.id = "diet_fish";
    behaviourTaskActor15.locale_key = "task_unit_eat";
    behaviourTaskActor15.diet = true;
    BehaviourTaskActor pAsset15 = behaviourTaskActor15;
    this.t = behaviourTaskActor15;
    this.add(pAsset15);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_diet_piscivore");
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.Water));
    this.t.addBeh((BehaviourActionActor) new BehGoOrSwimToTileTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, string.Empty, 1f, 20f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehReplenishNutrition());
    BehaviourTaskActor behaviourTaskActor16 = new BehaviourTaskActor();
    behaviourTaskActor16.id = "family_alpha_move";
    behaviourTaskActor16.cancellable_by_socialize = true;
    behaviourTaskActor16.cancellable_by_reproduction = true;
    behaviourTaskActor16.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset16 = behaviourTaskActor16;
    this.t = behaviourTaskActor16;
    this.add(pAsset16);
    this.t.setIcon("ui/Icons/actor_traits/iconStrong");
    this.t.addBeh((BehaviourActionActor) new BehFamilyAlphaMove());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    BehaviourTaskActor behaviourTaskActor17 = new BehaviourTaskActor();
    behaviourTaskActor17.id = "family_group_follow";
    behaviourTaskActor17.cancellable_by_socialize = true;
    behaviourTaskActor17.cancellable_by_reproduction = true;
    behaviourTaskActor17.locale_key = "task_unit_follow_family";
    BehaviourTaskActor pAsset17 = behaviourTaskActor17;
    this.t = behaviourTaskActor17;
    this.add(pAsset17);
    this.t.setIcon("ui/Icons/iconFamiliesZones");
    this.t.addBeh((BehaviourActionActor) new BehFamilyFollowAlpha());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(GoToActorTargetType.SameRegion));
    BehaviourTaskActor behaviourTaskActor18 = new BehaviourTaskActor();
    behaviourTaskActor18.id = "family_check_existence";
    BehaviourTaskActor pAsset18 = behaviourTaskActor18;
    this.t = behaviourTaskActor18;
    this.add(pAsset18);
    this.t.setIcon("ui/Icons/iconFamiliesZones");
    this.t.addBeh((BehaviourActionActor) new BehFamilyCheckMembers());
    BehaviourTaskActor behaviourTaskActor19 = new BehaviourTaskActor();
    behaviourTaskActor19.id = "family_group_leave";
    BehaviourTaskActor pAsset19 = behaviourTaskActor19;
    this.t = behaviourTaskActor19;
    this.add(pAsset19);
    this.t.setIcon("ui/Icons/iconFamiliesZones");
    this.t.addBeh((BehaviourActionActor) new BehFamilyGroupLeave());
    BehaviourTaskActor behaviourTaskActor20 = new BehaviourTaskActor();
    behaviourTaskActor20.id = "family_group_join_or_new_herd";
    BehaviourTaskActor pAsset20 = behaviourTaskActor20;
    this.t = behaviourTaskActor20;
    this.add(pAsset20);
    this.t.setIcon("ui/Icons/iconFamiliesZones");
    this.t.addBeh((BehaviourActionActor) new BehFamilyGroupJoin());
    this.t.addBeh((BehaviourActionActor) new BehFamilyGroupNew());
    BehaviourTaskActor behaviourTaskActor21 = new BehaviourTaskActor();
    behaviourTaskActor21.id = "attack_golden_brain";
    BehaviourTaskActor pAsset21 = behaviourTaskActor21;
    this.t = behaviourTaskActor21;
    this.add(pAsset21);
    this.t.setIcon("ui/Icons/iconGoldBrain");
    this.t.addBeh((BehaviourActionActor) new BehFindGoldenBrain());
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    BehaviourTaskActor behaviourTaskActor22 = new BehaviourTaskActor();
    behaviourTaskActor22.id = "follow_desire_target";
    behaviourTaskActor22.locale_key = "task_unit_weird_desire";
    BehaviourTaskActor pAsset22 = behaviourTaskActor22;
    this.t = behaviourTaskActor22;
    this.add(pAsset22);
    this.t.setIcon("ui/Icons/iconGoldBrain");
    this.t.addBeh((BehaviourActionActor) new BehFindDesireWaypoint());
    this.t.addBeh((BehaviourActionActor) new BehGoToBuildingTarget());
    BehaviourTaskActor behaviourTaskActor23 = new BehaviourTaskActor();
    behaviourTaskActor23.id = "crab_eat";
    behaviourTaskActor23.diet = true;
    BehaviourTaskActor pAsset23 = behaviourTaskActor23;
    this.t = behaviourTaskActor23;
    this.add(pAsset23);
    this.t.setIcon("ui/Icons/iconCrab");
    this.t.addBeh((BehaviourActionActor) new BehAnimalCheckHungry());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(2f, 5f));
    this.t.addBeh((BehaviourActionActor) new BehFindTileBeach());
    this.t.addBeh((BehaviourActionActor) new BehGoOrSwimToTileTarget());
    for (int index = 0; index < 4; ++index)
      this.t.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Tile, string.Empty, 1f, 20f, pLandIfHovering: true));
    this.t.addBeh((BehaviourActionActor) new BehReplenishNutrition());
    BehaviourTaskActor behaviourTaskActor24 = new BehaviourTaskActor();
    behaviourTaskActor24.id = "crab_danger_check";
    BehaviourTaskActor pAsset24 = behaviourTaskActor24;
    this.t = behaviourTaskActor24;
    this.add(pAsset24);
    this.t.setIcon("ui/Icons/iconBloodRain");
    this.t.addBeh((BehaviourActionActor) new BehCheckIfOnGround());
    this.t.addBeh((BehaviourActionActor) new BehActiveCrabDangerCheck());
    BehaviourTaskActor behaviourTaskActor25 = new BehaviourTaskActor();
    behaviourTaskActor25.id = "crab_burrow";
    BehaviourTaskActor pAsset25 = behaviourTaskActor25;
    this.t = behaviourTaskActor25;
    this.add(pAsset25);
    this.t.setIcon("ui/Icons/iconCrab");
    this.t.addBeh((BehaviourActionActor) new BehCheckIfOnGround());
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehCrabBurrow());
    BehaviourTaskActor behaviourTaskActor26 = new BehaviourTaskActor();
    behaviourTaskActor26.id = "make_skeleton";
    BehaviourTaskActor pAsset26 = behaviourTaskActor26;
    this.t = behaviourTaskActor26;
    this.add(pAsset26);
    this.t.setIcon("ui/Icons/iconSkeleton");
    this.t.addBeh((BehaviourActionActor) new BehMagicMakeSkeleton());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 3f));
    BehaviourTaskActor behaviourTaskActor27 = new BehaviourTaskActor();
    behaviourTaskActor27.id = "skeleton_move";
    behaviourTaskActor27.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset27 = behaviourTaskActor27;
    this.t = behaviourTaskActor27;
    this.add(pAsset27);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehSkeletonFindTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 6f));
    BehaviourTaskActor behaviourTaskActor28 = new BehaviourTaskActor();
    behaviourTaskActor28.id = "spawn_fertilizer";
    BehaviourTaskActor pAsset28 = behaviourTaskActor28;
    this.t = behaviourTaskActor28;
    this.add(pAsset28);
    this.t.setIcon("ui/Icons/iconFertilizerPlants");
    this.t.addBeh((BehaviourActionActor) new BehSpawnTreeFertilizer());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    BehaviourTaskActor behaviourTaskActor29 = new BehaviourTaskActor();
    behaviourTaskActor29.id = "check_cure";
    BehaviourTaskActor pAsset29 = behaviourTaskActor29;
    this.t = behaviourTaskActor29;
    this.add(pAsset29);
    this.t.setIcon("ui/Icons/iconHealth");
    this.t.addBeh((BehaviourActionActor) new BehCheckCure());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    BehaviourTaskActor behaviourTaskActor30 = new BehaviourTaskActor();
    behaviourTaskActor30.id = "burn_tumors";
    BehaviourTaskActor pAsset30 = behaviourTaskActor30;
    this.t = behaviourTaskActor30;
    this.add(pAsset30);
    this.t.setIcon("ui/Icons/iconFire");
    this.t.addBeh((BehaviourActionActor) new BehBurnTumorTiles());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    BehaviourTaskActor behaviourTaskActor31 = new BehaviourTaskActor();
    behaviourTaskActor31.id = "check_heal";
    BehaviourTaskActor pAsset31 = behaviourTaskActor31;
    this.t = behaviourTaskActor31;
    this.add(pAsset31);
    this.t.setIcon("ui/Icons/iconHealth");
    this.t.addBeh((BehaviourActionActor) new BehHeal());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    BehaviourTaskActor behaviourTaskActor32 = new BehaviourTaskActor();
    behaviourTaskActor32.id = "random_teleport";
    BehaviourTaskActor pAsset32 = behaviourTaskActor32;
    this.t = behaviourTaskActor32;
    this.add(pAsset32);
    this.t.setIcon("ui/Icons/actor_traits/iconMageSlayer");
    this.t.addBeh((BehaviourActionActor) new BehRandomTeleport());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f));
    BehaviourTaskActor behaviourTaskActor33 = new BehaviourTaskActor();
    behaviourTaskActor33.id = "teleport_back_home";
    BehaviourTaskActor pAsset33 = behaviourTaskActor33;
    this.t = behaviourTaskActor33;
    this.add(pAsset33);
    this.t.setIcon("ui/Icons/actor_traits/iconMageSlayer");
    this.t.addBeh((BehaviourActionActor) new BehTeleportHome());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f));
    BehaviourTaskActor behaviourTaskActor34 = new BehaviourTaskActor();
    behaviourTaskActor34.id = "run_to_water_when_on_fire";
    behaviourTaskActor34.locale_key = "task_unit_run_to_water";
    BehaviourTaskActor pAsset34 = behaviourTaskActor34;
    this.t = behaviourTaskActor34;
    this.add(pAsset34);
    this.t.setIcon("ui/Icons/iconFire");
    this.t.addBeh((BehaviourActionActor) new BehShortRandomMove());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehShortRandomMove());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehShortRandomMove());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehFindTileWhenOnFire());
    this.t.addBeh((BehaviourActionActor) new BehGoOrSwimToTileTarget());
    BehaviourTaskActor behaviourTaskActor35 = new BehaviourTaskActor();
    behaviourTaskActor35.id = "short_move";
    behaviourTaskActor35.cancellable_by_socialize = true;
    behaviourTaskActor35.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset35 = behaviourTaskActor35;
    this.t = behaviourTaskActor35;
    this.add(pAsset35);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehShortRandomMove());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
  }

  private void initTasksFingers()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "godfinger_find_target";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconGodFinger");
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehFingerFindTarget());
    this.t.addBeh((BehaviourActionActor) new BehFingerGoTowardsTileTarget(5));
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(true, 2f));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("godfinger_draw"));
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "godfinger_draw";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconGodFinger");
    this.t.addBeh((BehaviourActionActor) new BehFingerCheckCanDraw());
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(false));
    this.t.addBeh((BehaviourActionActor) new BehFingerFindCloseTile());
    this.t.addBeh((BehaviourActionActor) new BehFingerWaitForFlying());
    this.t.addBeh((BehaviourActionActor) new BehFingerDrawToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("godfinger_find_target"));
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "godfinger_move";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconGodFinger");
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.1f, 0.4f));
    this.t.addBeh((BehaviourActionActor) new BehFingerFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehFingerGoTowardsTileTarget());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "godfinger_random_fun_move";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconGodFinger");
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(true));
    for (int index = 0; index < 6; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehFingerFindRandomTile(5));
      this.t.addBeh((BehaviourActionActor) new BehFingerGoTowardsTileTarget(5));
      this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 0.01f));
    }
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "godfinger_circle_move";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconGodFinger");
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.1f, 0.4f));
    this.t.addBeh((BehaviourActionActor) new BehFingerGoToCircleTarget(25, 75));
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "godfinger_circle_move_small";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconGodFinger");
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.1f, 0.4f));
    this.t.addBeh((BehaviourActionActor) new BehFingerGoToCircleTarget(5, 15));
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "godfinger_circle_move_big";
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconGodFinger");
    this.t.addBeh((BehaviourActionActor) new BehFingerSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.1f, 0.4f));
    this.t.addBeh((BehaviourActionActor) new BehFingerGoToCircleTarget(75, 150));
  }

  private void initTasksDragons()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "dragon_fly";
    behaviourTaskActor1.locale_key = "task_unit_dragon_fly";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconDragon");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.Fly, pForceRestart: false));
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehDragonSleepy());
    this.t.addBeh((BehaviourActionActor) new BehDragonCheckAttackTargetAlive());
    this.t.addBeh((BehaviourActionActor) new BehDragonZombieFindGoldenBrain());
    this.t.addBeh((BehaviourActionActor) new BehDragonCheckAttackTile());
    this.t.addBeh((BehaviourActionActor) new BehDragonCheckAttackCity());
    this.t.addBeh((BehaviourActionActor) new BehDragonFindRandomTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRepeatTaskChance(0.7f));
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "dragon_sleep";
    behaviourTaskActor2.locale_key = "task_unit_dragon_sleep";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconSleep");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.SleepStart, false));
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlip(false));
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlying(false));
    this.t.addBeh((BehaviourActionActor) new BehDragonFinishAnimation());
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.SleepLoop));
    this.t.addBeh((BehaviourActionActor) new BehDragonSleep());
    this.t.addBeh((BehaviourActionActor) new BehDragonFinishAnimation());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("dragon_wakeup"));
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "dragon_wakeup";
    behaviourTaskActor3.locale_key = "task_unit_dragon_sleep";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconDragon");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.SleepUp, false));
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlip(false));
    this.t.addBeh((BehaviourActionActor) new BehDragonFinishAnimation());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "dragon_slide";
    behaviourTaskActor4.locale_key = "task_unit_dragon_attack";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconFire");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.Slide, false));
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehDragonSleepy());
    this.t.addBeh((BehaviourActionActor) new BehActorAddStatus("invincible", 2f));
    this.t.addBeh((BehaviourActionActor) new BehActorSetBool("justSlid", true));
    this.t.addBeh((BehaviourActionActor) new BehDragonSlide());
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "dragon_land";
    behaviourTaskActor5.locale_key = "task_unit_dragon_land";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconDragon");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.Landing, false));
    this.t.addBeh((BehaviourActionActor) new BehDragonSleepy());
    this.t.addBeh((BehaviourActionActor) new BehDragonCheckAttackTargetAlive());
    this.t.addBeh((BehaviourActionActor) new BehDragonCheckOverTargetCity());
    this.t.addBeh((BehaviourActionActor) new BehDragonFinishAnimation());
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlying(false));
    this.t.addBeh((BehaviourActionActor) new BehDragonCantLand("dragon_up"));
    this.t.addBeh((BehaviourActionActor) new BehDragonLanded());
    this.t.addBeh((BehaviourActionActor) new BehDragonCheckOverTargetActor());
    this.t.addBeh((BehaviourActionActor) new BehDragonCheckOverTargetCity());
    this.t.addBeh((BehaviourActionActor) new BehActorSetInt("justLanded", 2));
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "dragon_land_attack";
    behaviourTaskActor6.locale_key = "task_unit_dragon_attack";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconFire");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.LandAttack, false));
    this.t.addBeh((BehaviourActionActor) new BehDragonSleepy());
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlying(false));
    this.t.addBeh((BehaviourActionActor) new BehDragonLandAttack());
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "dragon_up";
    behaviourTaskActor7.locale_key = "task_unit_dragon_fly";
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconDragon");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.Up, false));
    this.t.addBeh((BehaviourActionActor) new BehDragonFlyUp());
    this.t.addBeh((BehaviourActionActor) new BehDragonSleepy());
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlying(true));
    this.t.addBeh((BehaviourActionActor) new BehDragonFinishAnimation());
    this.t.addBeh((BehaviourActionActor) new BehActorSetBool("justUp", true));
    this.t.addBeh((BehaviourActionActor) new BehActorCheckBool("justGotHit", "dragon_fly"));
    BehaviourTaskActor behaviourTaskActor8 = new BehaviourTaskActor();
    behaviourTaskActor8.id = "dragon_idle";
    behaviourTaskActor8.locale_key = "task_unit_dragon_normal";
    BehaviourTaskActor pAsset8 = behaviourTaskActor8;
    this.t = behaviourTaskActor8;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/iconDragon");
    this.t.addBeh((BehaviourActionActor) new BehDragonSetAnimation(DragonState.Idle));
    this.t.addBeh((BehaviourActionActor) new BehActorSetFlying(false));
    this.t.addBeh((BehaviourActionActor) new BehDragonIdle());
    this.t.addBeh((BehaviourActionActor) new BehDragonFinishAnimation());
  }

  private void initTasksUFOs()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "ufo_idle";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconUFO");
    this.t.addBeh((BehaviourActionActor) new BehSetActorSpeed(20f));
    this.t.addBeh((BehaviourActionActor) new BehUFOBeam());
    this.t.addBeh((BehaviourActionActor) new BehUFOCheckExplore());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "ufo_hit";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconUFO");
    this.t.addBeh((BehaviourActionActor) new BehUFOSelectTarget());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "ufo_flee";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/actor_traits/iconAgile");
    this.t.addBeh((BehaviourActionActor) new BehSetActorSpeed(100f));
    this.t.addBeh((BehaviourActionActor) new BehUFOBeam());
    this.t.addBeh((BehaviourActionActor) new BehGetRandomZoneTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "ufo_fly";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconUFO");
    this.t.addBeh((BehaviourActionActor) new BehSetActorSpeed(20f));
    this.t.addBeh((BehaviourActionActor) new BehUFOBeam());
    this.t.addBeh((BehaviourActionActor) new BehUFOFindTarget());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehUFOCheckAttackCity());
    this.t.addBeh((BehaviourActionActor) new BehUFOExplore());
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "ufo_explore";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconInspectZoneToggle");
    this.t.addBeh((BehaviourActionActor) new BehSetActorSpeed(10f));
    this.t.addBeh((BehaviourActionActor) new BehUFOBeam());
    this.t.addBeh((BehaviourActionActor) new BehGetRandomZoneTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehUFOCheckExplore());
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "ufo_chase";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconUFO");
    this.t.addBeh((BehaviourActionActor) new BehSetActorSpeed(50f));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("ufo_attack"));
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "ufo_attack";
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconArrowAttackTarget");
    this.t.addBeh((BehaviourActionActor) new BehUFOBeam(true));
  }

  private void initTasksBoats()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "boat_check_existence";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehBoatCheckExistence());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "boat_check_limits";
    behaviourTaskActor2.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehBoatCheckLimit());
    this.t.addBeh((BehaviourActionActor) new BehBoatCheckHomeDocks());
    this.t.addBeh((BehaviourActionActor) new BehBoatSetHomeDockTarget());
    this.t.addBeh((BehaviourActionActor) new BehBoatFindTileInDock());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 3f));
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(2f, 4f));
    this.t.addBeh((BehaviourActionActor) new BehBoatRemoveIfLimit());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "boat_idle";
    behaviourTaskActor3.locale_key = "task_unit_nothing";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconClock");
    this.t.addBeh((BehaviourActionActor) new BehBoatDamageCheck());
    this.t.addBeh((BehaviourActionActor) new BehBoatFindOceanNeutralTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 10f));
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "boat_danger_check";
    behaviourTaskActor4.locale_key = "task_unit_flee";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehCheckIfInLiquid());
    this.t.addBeh((BehaviourActionActor) new BehBoatDangerCheck());
    this.t.addBeh((BehaviourActionActor) new BehBoatFindWaterTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 10f));
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "boat_transport_check";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    this.t.addBeh((BehaviourActionActor) new BehBoatTransportCheck());
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "boat_transport_check_taxi";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconBoat");
    this.t.addBeh((BehaviourActionActor) new BehBoatFindRequest());
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "boat_transport_go_load";
    behaviourTaskActor7.locale_key = "task_unit_boat_load_units";
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconCityInventory");
    this.t.addBeh((BehaviourActionActor) new BehBoatTransportFindTilePickUp());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    this.t.addBeh((BehaviourActionActor) new BehBoatTransportDoLoading());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("boat_transport_go_unload", false));
    BehaviourTaskActor behaviourTaskActor8 = new BehaviourTaskActor();
    behaviourTaskActor8.id = "boat_transport_go_unload";
    behaviourTaskActor8.locale_key = "task_unit_boat_unload_units";
    BehaviourTaskActor pAsset8 = behaviourTaskActor8;
    this.t = behaviourTaskActor8;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/iconCityInventory");
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    this.t.addBeh((BehaviourActionActor) new BehBoatTransportFindTileUnload());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget()
    {
      walk_on_water = true
    });
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    this.t.addBeh((BehaviourActionActor) new BehBoatTransportUnloadUnits());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(3f, 10f));
    this.t.addBeh((BehaviourActionActor) new BehEndJob());
    BehaviourTaskActor behaviourTaskActor9 = new BehaviourTaskActor();
    behaviourTaskActor9.id = "boat_trading";
    behaviourTaskActor9.locale_key = "task_unit_boat_trade";
    BehaviourTaskActor pAsset9 = behaviourTaskActor9;
    this.t = behaviourTaskActor9;
    this.add(pAsset9);
    this.t.setIcon("ui/Icons/iconMoney");
    this.t.addBeh((BehaviourActionActor) new BehBoatCheckHomeDocks());
    this.t.addBeh((BehaviourActionActor) new BehBoatFindTargetForTrade());
    this.t.addBeh((BehaviourActionActor) new BehBoatFindTileInDock());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(2f, 5f));
    this.t.addBeh((BehaviourActionActor) new BehBoatMakeTrade());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("boat_return_to_dock"));
    BehaviourTaskActor behaviourTaskActor10 = new BehaviourTaskActor();
    behaviourTaskActor10.id = "boat_fishing";
    behaviourTaskActor10.locale_key = "task_unit_boat_catch_fish";
    BehaviourTaskActor pAsset10 = behaviourTaskActor10;
    this.t = behaviourTaskActor10;
    this.add(pAsset10);
    this.t.setIcon("ui/Icons/iconResFish");
    this.t.addBeh((BehaviourActionActor) new BehBoatCheckHomeDocks());
    this.t.addBeh((BehaviourActionActor) new BehBoatFindWaterTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait());
    this.t.addBeh((BehaviourActionActor) new BehBoatFishing());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(5f, 10f));
    this.t.addBeh((BehaviourActionActor) new BehBoatCollectFish());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    this.t.addBeh((BehaviourActionActor) new BehBoatCheckFishingRepeat());
    BehaviourTaskActor behaviourTaskActor11 = new BehaviourTaskActor();
    behaviourTaskActor11.id = "boat_return_to_dock";
    behaviourTaskActor11.locale_key = "task_unit_return_to_dock";
    BehaviourTaskActor pAsset11 = behaviourTaskActor11;
    this.t = behaviourTaskActor11;
    this.add(pAsset11);
    this.t.setIcon("ui/Icons/iconArrowDestination");
    this.t.addBeh((BehaviourActionActor) new BehBoatCheckHomeDocks());
    this.t.addBeh((BehaviourActionActor) new BehBoatSetHomeDockTarget());
    this.t.addBeh((BehaviourActionActor) new BehBoatFindTileInDock());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 3f));
    this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(2f, 4f));
    this.t.addBeh((BehaviourActionActor) new BehUnloadResources());
    this.t.addBeh((BehaviourActionActor) new BehRepairInDock());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    this.t.addBeh((BehaviourActionActor) new BehEndJob());
  }

  private void initTasksReproductionAsexual()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "asexual_reproduction_budding";
    behaviourTaskActor1.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_reproduction_budding");
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehStartShake(0.1f, 0.1f));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckReproductionBasics());
    this.t.addBeh((BehaviourActionActor) new BehCheckBuddingReproduction());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "asexual_reproduction_divine";
    behaviourTaskActor2.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_reproduction_divine");
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehStartShake(0.1f, 0.1f));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckReproductionBasics());
    this.t.addBeh((BehaviourActionActor) new BehCheckDivineReproduction());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "asexual_reproduction_spores";
    behaviourTaskActor3.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_reproduction_spores");
    this.t.addBeh((BehaviourActionActor) new BehStartShake());
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.1f, 0.1f));
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckReproductionBasics());
    this.t.addBeh((BehaviourActionActor) new BehCheckSporeReproduction());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "asexual_reproduction_fission";
    behaviourTaskActor4.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_reproduction_fission");
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehStartShake(0.1f, 0.1f));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckReproductionBasics());
    this.t.addBeh((BehaviourActionActor) new BehCheckFissionReproduction());
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "asexual_reproduction_vegetative";
    behaviourTaskActor5.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_reproduction_vegetative");
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehStartShake(0.1f, 0.1f));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckReproductionBasics());
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.Dirt));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehCheckVegetativeReproduction());
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "asexual_reproduction_parthenogenesis";
    behaviourTaskActor6.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_reproduction_parthenogenesis");
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehStartShake(0.1f, 0.1f));
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckReproductionBasics());
    this.t.addBeh((BehaviourActionActor) new BehCheckParthenogenesisReproduction());
  }

  private void initTasksReproductionSexual()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "check_lover_city";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconCity");
    this.t.addBeh((BehaviourActionActor) new BehCheckSameCityActorLover());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "find_lover";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconArrowLover");
    this.t.addBeh((BehaviourActionActor) new BehFindLover());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "sexual_reproduction_try";
    behaviourTaskActor3.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconLovers");
    this.t.addBeh((BehaviourActionActor) new BehCheckReproductionBasics());
    this.t.addBeh((BehaviourActionActor) new BehSexualReproductionTry());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "sexual_reproduction_check_outside";
    behaviourTaskActor4.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconLovers");
    this.t.addBeh((BehaviourActionActor) new BehCheckSexualReproductionOutside());
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "sexual_reproduction_inside";
    behaviourTaskActor5.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/iconLovers");
    this.t.addBeh((BehaviourActionActor) new BehCheckSexualReproductionCiv());
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "sexual_reproduction_outside";
    behaviourTaskActor6.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/iconLovers");
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f));
    this.t.addBeh((BehaviourActionActor) new BehAnimalBreedingTime());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f));
    this.t.addBeh((BehaviourActionActor) new BehAnimalBreedingTime());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f));
    this.t.addBeh((BehaviourActionActor) new BehAnimalBreedingTime());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(0.5f));
    this.t.addBeh((BehaviourActionActor) new BehCheckForBabiesFromSexualReproduction());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    BehaviourTaskActor behaviourTaskActor7 = new BehaviourTaskActor();
    behaviourTaskActor7.id = "sexual_reproduction_civ_go";
    behaviourTaskActor7.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset7 = behaviourTaskActor7;
    this.t = behaviourTaskActor7;
    this.add(pAsset7);
    this.t.setIcon("ui/Icons/iconLovers");
    this.t.addBeh((BehaviourActionActor) new BehBuildingTargetLoverHome());
    this.t.addBeh((BehaviourActionActor) new BehGetTargetBuildingMainTile());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 6; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
      this.t.addBeh((BehaviourActionActor) new BehCheckForLover());
      this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    }
    BehaviourTaskActor behaviourTaskActor8 = new BehaviourTaskActor();
    behaviourTaskActor8.id = "sexual_reproduction_civ_action";
    behaviourTaskActor8.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset8 = behaviourTaskActor8;
    this.t = behaviourTaskActor8;
    this.add(pAsset8);
    this.t.setIcon("ui/Icons/iconLovers");
    for (int index = 0; index < 5; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(1f, 2f));
      this.t.addBeh((BehaviourActionActor) new BehShakeBuilding());
      this.t.addBeh((BehaviourActionActor) new BehSpawnHeartsFromBuilding());
    }
    this.t.addBeh((BehaviourActionActor) new BehCheckForBabiesFromSexualReproduction());
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
    BehaviourTaskActor behaviourTaskActor9 = new BehaviourTaskActor();
    behaviourTaskActor9.id = "sexual_reproduction_civ_wait";
    behaviourTaskActor9.locale_key = "task_unit_reproduce";
    BehaviourTaskActor pAsset9 = behaviourTaskActor9;
    this.t = behaviourTaskActor9;
    this.add(pAsset9);
    this.t.setIcon("ui/Icons/iconLovers");
    for (int index = 0; index < 5; ++index)
      this.t.addBeh((BehaviourActionActor) new BehStayInBuildingTarget(1f, 2f));
    this.t.addBeh((BehaviourActionActor) new BehExitBuilding());
  }

  private void initTasksChildren()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "child_random_flips";
    behaviourTaskActor1.cancellable_by_socialize = true;
    behaviourTaskActor1.locale_key = "task_unit_play";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/iconChildren");
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.2f, 0.2f));
    }
    this.t.addBeh((BehaviourActionActor) new BehActorChangeHappiness("just_played"));
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "child_play_at_one_spot";
    behaviourTaskActor2.cancellable_by_socialize = true;
    behaviourTaskActor2.locale_key = "task_unit_play";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/iconChildren");
    this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(1.5f, 1.5f));
    this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
    this.t.addBeh((BehaviourActionActor) new BehActorChangeHappiness("just_played"));
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "child_random_jump";
    behaviourTaskActor3.cancellable_by_socialize = true;
    behaviourTaskActor3.locale_key = "task_unit_play";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/iconChildren");
    for (int index = 0; index < 2; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(1.5f, 1.5f));
    }
    this.t.addBeh((BehaviourActionActor) new BehActorRandomJump());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 3f));
    this.t.addBeh((BehaviourActionActor) new BehActorChangeHappiness("just_played"));
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "child_follow_parent";
    behaviourTaskActor4.cancellable_by_socialize = true;
    behaviourTaskActor4.locale_key = "task_unit_move";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/iconAdults");
    this.t.addBeh((BehaviourActionActor) new BehChildFindRandomFamilyParent());
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(GoToActorTargetType.SameRegion));
    this.t.addBeh((BehaviourActionActor) new BehRepeatTaskChance(0.1f));
  }

  private void initTasksSubspeciesTraits()
  {
    BehaviourTaskActor behaviourTaskActor = new BehaviourTaskActor();
    behaviourTaskActor.id = "try_affect_dreams";
    BehaviourTaskActor pAsset = behaviourTaskActor;
    this.t = behaviourTaskActor;
    this.add(pAsset);
    this.t.setIcon("ui/Icons/subspecies_traits/subspecies_trait_dreamweavers");
    this.t.addBeh((BehaviourActionActor) new BehFindTile(TileFinderType.FreeTile));
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    for (int index = 0; index < 4; ++index)
    {
      this.t.addBeh((BehaviourActionActor) new BehActorReverseFlip());
      this.t.addBeh((BehaviourActionActor) new BehJumpingAnimation(0.5f, 0.5f));
    }
    this.t.addBeh((BehaviourActionActor) new BehAffectDreams());
  }

  private void initTasksSocializing()
  {
    BehaviourTaskActor behaviourTaskActor1 = new BehaviourTaskActor();
    behaviourTaskActor1.id = "socialize_initial_check";
    behaviourTaskActor1.locale_key = "task_unit_socialize";
    BehaviourTaskActor pAsset1 = behaviourTaskActor1;
    this.t = behaviourTaskActor1;
    this.add(pAsset1);
    this.t.setIcon("ui/Icons/culture_traits/culture_trait_gossip_lovers");
    this.t.addBeh((BehaviourActionActor) new BehSocializeStartCheck());
    BehaviourTaskActor behaviourTaskActor2 = new BehaviourTaskActor();
    behaviourTaskActor2.id = "socialize_try_to_start_near_bonfire";
    behaviourTaskActor2.locale_key = "task_unit_socialize";
    BehaviourTaskActor pAsset2 = behaviourTaskActor2;
    this.t = behaviourTaskActor2;
    this.add(pAsset2);
    this.t.setIcon("ui/Icons/culture_traits/culture_trait_gossip_lovers");
    this.t.addBeh((BehaviourActionActor) new BehCityActorFindBuilding("type_bonfire"));
    this.t.addBeh((BehaviourActionActor) new BehFindRandomTileNearBuildingTarget());
    this.t.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    this.t.addBeh((BehaviourActionActor) new BehTryToSocialize());
    BehaviourTaskActor behaviourTaskActor3 = new BehaviourTaskActor();
    behaviourTaskActor3.id = "socialize_try_to_start_immediate";
    behaviourTaskActor3.locale_key = "task_unit_socialize";
    BehaviourTaskActor pAsset3 = behaviourTaskActor3;
    this.t = behaviourTaskActor3;
    this.add(pAsset3);
    this.t.setIcon("ui/Icons/culture_traits/culture_trait_gossip_lovers");
    this.t.addBeh((BehaviourActionActor) new BehTryToSocialize());
    BehaviourTaskActor behaviourTaskActor4 = new BehaviourTaskActor();
    behaviourTaskActor4.id = "socialize_go_to_target";
    behaviourTaskActor4.locale_key = "task_unit_socialize";
    BehaviourTaskActor pAsset4 = behaviourTaskActor4;
    this.t = behaviourTaskActor4;
    this.add(pAsset4);
    this.t.setIcon("ui/Icons/culture_traits/culture_trait_gossip_lovers");
    this.t.addBeh((BehaviourActionActor) new BehGoToActorTarget(GoToActorTargetType.NearbyTileClosest, pCalibrateTargetPosition: true));
    this.t.addBeh((BehaviourActionActor) new BehCheckNearActorTarget());
    this.t.addBeh((BehaviourActionActor) new BehSetNextTask("socialize_do_talk", false));
    BehaviourTaskActor behaviourTaskActor5 = new BehaviourTaskActor();
    behaviourTaskActor5.id = "socialize_do_talk";
    behaviourTaskActor5.locale_key = "task_unit_socialize";
    BehaviourTaskActor pAsset5 = behaviourTaskActor5;
    this.t = behaviourTaskActor5;
    this.add(pAsset5);
    this.t.setIcon("ui/Icons/culture_traits/culture_trait_gossip_lovers");
    this.t.addBeh((BehaviourActionActor) new BehDoTalk());
    this.t.addBeh((BehaviourActionActor) new BehFinishTalk());
    this.t.addBeh((BehaviourActionActor) new BehRandomWait(1f, 2f));
    BehaviourTaskActor behaviourTaskActor6 = new BehaviourTaskActor();
    behaviourTaskActor6.id = "socialize_receiving";
    behaviourTaskActor6.locale_key = "task_unit_socialize";
    BehaviourTaskActor pAsset6 = behaviourTaskActor6;
    this.t = behaviourTaskActor6;
    this.add(pAsset6);
    this.t.setIcon("ui/Icons/culture_traits/culture_trait_gossip_lovers");
    this.t.addBeh((BehaviourActionActor) new BehSocializeTalk());
  }

  private void addActionsForDeliverResources(BehaviourTaskActor pTask, bool pWheatStorage = false)
  {
    pTask.addBeh((BehaviourActionActor) new BehCheckHasResources());
    pTask.addBeh((BehaviourActionActor) new BehRandomWait(0.7f, 1.2f));
    if (pWheatStorage)
      pTask.addBeh((BehaviourActionActor) new BehCityActorFindStorageWheat());
    else
      pTask.addBeh((BehaviourActionActor) new BehCityActorFindStorage());
    pTask.addBeh((BehaviourActionActor) new BehFindRaycastTileForBuildingTarget());
    pTask.addBeh((BehaviourActionActor) new BehGoToTileTarget());
    pTask.addBeh((BehaviourActionActor) new BehLookAtBuildingTarget());
    pTask.addBeh((BehaviourActionActor) new BehAngleAnimation(AngleAnimationTarget.Building, string.Empty, 0.1f, 20f));
    pTask.addBeh((BehaviourActionActor) new BehThrowResources());
    pTask.addBeh((BehaviourActionActor) new BehRandomWait(pMax: 0.2f));
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (BehaviourTaskActor behaviourTaskActor in this.list)
    {
      string forceHandTool = behaviourTaskActor.force_hand_tool;
      if (!string.IsNullOrEmpty(forceHandTool))
        behaviourTaskActor.cached_hand_tool_asset = AssetManager.unit_hand_tools.get(forceHandTool);
    }
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (BehaviourTaskActor pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}
