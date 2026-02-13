// Decompiled with JetBrains decompiler
// Type: AutoCivilization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AutoCivilization
{
  private const int KIDS_AGE = 5;
  private const float KIDS_PERCENT = 0.5f;
  private const int EGGS_AGE = 0;
  private const float EGGS_PERCENT_OF_KIDS = 0.5f;
  private const float WARRIORS_PERCENT_OF_ADULTS = 0.1f;
  private const float ITEMS_HOLDER_PERCENT = 0.6f;
  private const int TICKS_WITHOUT_BUILDING_TO_STOP = 100;
  private const int MAXIMUM_TICKS = 5000;
  private const int UNITS_AMOUNT = 100;
  private const float ELAPSED_PER_TICK = 1.5f;
  private const int ITEM_PRODUCTION_PER_UNIT = 5;
  private const int MAXIMUM_TASK_ACTIONS = 500;
  private const int FRAMES_PER_ROUTINE_UPDATE = 4;
  private List<BehaviourTaskActor> _tasks_building = new List<BehaviourTaskActor>();
  private List<BehaviourTaskActor> _tasks_gathering = new List<BehaviourTaskActor>();
  private List<BehaviourTaskActor> _tasks_farming = new List<BehaviourTaskActor>();
  private BehaviourTaskActor _task_take_items;
  private City _city;
  private Actor _unit;
  private List<Actor> _units_list = new List<Actor>(100);
  private BuildingBiomeFoodProducer _food_producer_bonfire;
  private BuildingBiomeFoodProducer _food_producer_hall;
  private Coroutine _routine;
  private int _action_index;

  public AutoCivilization()
  {
    this._tasks_building.Add(AssetManager.tasks_actor.get("try_build_building"));
    this._tasks_building.Add(AssetManager.tasks_actor.get("build_building"));
    this._tasks_building.Add(AssetManager.tasks_actor.get("build_road"));
    this._tasks_building.Add(AssetManager.tasks_actor.get("cleaning"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("chop_trees"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("mine_deposit"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("mine"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("collect_fruits"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("collect_herbs"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("collect_honey"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("manure_cleaning"));
    this._tasks_gathering.Add(AssetManager.tasks_actor.get("store_resources"));
    this._tasks_farming.Add(AssetManager.tasks_actor.get("farmer_make_field"));
    this._tasks_farming.Add(AssetManager.tasks_actor.get("farmer_plant_crops"));
    this._tasks_farming.Add(AssetManager.tasks_actor.get("farmer_harvest"));
    this._tasks_farming.Add(AssetManager.tasks_actor.get("farmer_fertilize_crops"));
    this._task_take_items = AssetManager.tasks_actor.get("try_to_take_city_item");
  }

  public void makeCivilization(Actor pUnit)
  {
    if (!pUnit.isSapient())
      return;
    this._unit = pUnit;
    this.clear();
    if (!this._unit.buildCityAndStartCivilization())
      return;
    this._unit.setReligion(World.world.religions.newReligion(this._unit, true));
    this._city = this._unit.current_tile.zone.city;
    this._unit.setCity(this._city);
    this._unit.createNewWeapon("flame_sword");
    this._unit.kingdom.setCapital(this._city);
    this._units_list.Add(this._unit);
    this._city.setLeader(this.newUnit(this._unit.culture, this._unit.language), true);
    this.spawnUnits(this._unit.culture, this._unit.language);
    for (int index = 0; index < 50; ++index)
      this.makeBooks(this._unit);
    this._routine = World.world.StartCoroutine(this.civilizationMakingRoutine());
  }

  private void makeBooks(Actor pActor)
  {
    if (!pActor.hasLanguage() || !pActor.hasCulture())
      return;
    World.world.books.generateNewBook(pActor);
  }

  private Actor newUnit(Culture pCulture, Language pLanguage)
  {
    Actor actor = World.world.units.spawnNewUnit(this._unit.asset.id, this._city.zones.GetRandom<TileZone>().centerTile, true, true, 3f);
    actor.setCity(this._city);
    actor.setSubspecies(this._unit.subspecies);
    actor.setCulture(pCulture);
    actor.joinLanguage(pLanguage);
    this._units_list.Add(actor);
    return actor;
  }

  private void spawnUnits(Culture pCulture, Language pLanguage)
  {
    for (int index = 0; index < 99; ++index)
      this.newUnit(pCulture, pLanguage).data.age_overgrowth = Randy.randomInt(0, 100);
  }

  private IEnumerator civilizationMakingRoutine()
  {
    int tNoBuiltTicks = 0;
    for (int i = 0; i < 5000; ++i)
    {
      Actor random = this._units_list.GetRandom<Actor>();
      this.claimZone(random);
      this.gatherResources(random);
      bool flag = this.buildBuilding(random);
      this.doFarming(random);
      this.makeFood(random);
      this.refertilizeTiles(random.current_zone);
      if (!flag)
      {
        this.randomTeleport(random);
        ++tNoBuiltTicks;
      }
      else
        tNoBuiltTicks = 0;
      if (tNoBuiltTicks <= 100)
      {
        if (i % 4 == 0)
          yield return (object) new WaitForEndOfFrame();
      }
      else
        break;
    }
    foreach (Actor units in this._units_list)
    {
      this.craftAndTakeItems(units);
      units.setTask("citizen");
      units.setStatsDirty();
    }
  }

  private void claimZone(Actor pUnit)
  {
    TileZone zoneToClaim = MapBox.instance.city_zone_helper.city_growth.getZoneToClaim((Actor) null, this._city);
    if (zoneToClaim == null)
      return;
    pUnit.setCurrentTilePosition(zoneToClaim.centerTile);
    int num = (int) BehClaimZoneForCityActorBorder.tryClaimZone(pUnit);
  }

  private void gatherResources(Actor pUnit)
  {
    foreach (BehaviourTaskActor pTask in this._tasks_gathering)
      this.doTask(pTask, pUnit);
    this._city.addResourcesToRandomStockpile("wood", 8);
    this._city.addResourcesToRandomStockpile("stone", 8);
    this._city.addResourcesToRandomStockpile("common_metals", 4);
    this._city.addResourcesToRandomStockpile("gold", 2);
    this._city.addResourcesToRandomStockpile("fertilizer");
  }

  private bool buildBuilding(Actor pActor)
  {
    bool flag = CityBehBuild.buildTick(this._city);
    foreach (BehaviourTaskActor pTask in this._tasks_building)
      this.doTask(pTask, pActor);
    World.world.cities.setDirtyBuildings(this._city);
    World.world.cities.beginChecksBuildings();
    BuildingZonesSystem.setDirty();
    BuildingZonesSystem.update();
    return flag;
  }

  private void doFarming(Actor pUnit)
  {
    foreach (BehaviourTaskActor pTask in this._tasks_farming)
      this.doTask(pTask, pUnit);
    foreach (WorldTile calculatedFarmField in (ObjectContainer<WorldTile>) this._city.calculated_farm_fields)
    {
      Building building = calculatedFarmField.building;
      if (building != null && building.asset.wheat)
        building.component_wheat.update(1.5f);
    }
  }

  private void makeFood(Actor pUnit)
  {
    if (this._food_producer_bonfire == null)
      this._food_producer_bonfire = this._city.getBuildingOfType("type_bonfire")?.component_food_producer;
    if (this._food_producer_hall == null)
      this._food_producer_hall = this._city.getBuildingOfType("type_hall")?.component_food_producer;
    this._food_producer_bonfire?.update(1.5f);
    this._food_producer_hall?.update(1.5f);
  }

  private void refertilizeTiles(TileZone pTileZone)
  {
    foreach (WorldTile tile in pTileZone.tiles)
    {
      if (Randy.randomChance(0.05f))
        DropsLibrary.action_fertilizer_trees(tile);
      if (Randy.randomChance(0.05f))
        DropsLibrary.action_fertilizer_plants(tile);
    }
  }

  private void randomTeleport(Actor pUnit)
  {
    pUnit.setCurrentTilePosition(this._city.zones.GetRandom<TileZone>().centerTile);
  }

  private void craftAndTakeItems(Actor pUnit)
  {
    if (!Randy.randomChance(0.6f))
      return;
    for (int index = 0; index < 5; ++index)
    {
      ItemCrafting.tryToCraftRandomArmor(pUnit, this._city);
      ItemCrafting.tryToCraftRandomWeapon(pUnit, this._city);
    }
    this.doTask(this._task_take_items, pUnit);
  }

  private void doTask(BehaviourTaskActor pTask, Actor pActor)
  {
    this._action_index = 0;
    int num = 0;
    while (num < 500 && this.updateTaskIndex(pTask.list[this._action_index].startExecute(pActor)) && this._action_index <= pTask.list.Count - 1)
      ++num;
  }

  private bool updateTaskIndex(BehResult pResult)
  {
    switch (pResult)
    {
      case BehResult.Continue:
        ++this._action_index;
        break;
      case BehResult.Stop:
      case BehResult.Skip:
      case BehResult.RestartTask:
      case BehResult.ActiveTaskReturn:
      case BehResult.ImmediateRun:
        return false;
      case BehResult.StepBack:
        --this._action_index;
        if (this._action_index < 0)
        {
          this._action_index = 0;
          break;
        }
        break;
    }
    return true;
  }

  private void clear()
  {
    if (this._routine != null)
      World.world.StopCoroutine(this._routine);
    this._action_index = 0;
    this._food_producer_bonfire = (BuildingBiomeFoodProducer) null;
    this._food_producer_hall = (BuildingBiomeFoodProducer) null;
    this._units_list.Clear();
  }
}
