// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehCheckCitizenTasks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class CityBehCheckCitizenTasks : BehaviourActionCity
{
  private int _citizens_left;

  public override BehResult execute(City pCity)
  {
    CityTasksData tasks = pCity.tasks;
    CitizenJobs jobs = pCity.jobs;
    CityStatus status = pCity.status;
    jobs.clearJobs();
    this.checkOccupied(pCity);
    this._citizens_left = pCity.status.population_adults;
    tasks.clear();
    if (!DebugConfig.isOn(DebugOption.SystemCityTasks))
      return BehResult.Continue;
    this.countFires(pCity, tasks);
    this.countResources(pCity, tasks);
    this.countRoads(pCity, tasks);
    int num1 = 0;
    bool flag1 = false;
    int pTaskAmountMax = 0;
    bool flag2 = pCity.hasStorageBuilding();
    int totalFood = pCity.getTotalFood();
    if (WorldLawLibrary.world_law_civ_army.isEnabled() && status.population_adults > 15 && pCity.hasEnoughFoodForArmy())
    {
      flag1 = true;
      pTaskAmountMax = this.getPossibleWarriors(pCity);
    }
    int pJobMax1 = totalFood <= 100 ? (totalFood <= 60 ? (totalFood <= 40 ? (totalFood <= 20 ? 10 : 7) : 5) : 4) : 1;
    int pJobMax2 = pCity.getResourcesAmount("wood") <= 15 ? 3 : 1;
    bool flag3 = pCity.hasBuildingType("type_windmill");
    bool flag4 = pCity.hasBuildingType("type_mine");
    bool build = pCity.hasBuildingToBuild();
    int num2 = this._citizens_left * 2;
    while (this._citizens_left >= 0)
    {
      if (build)
        this.addToJob(CitizenJobLibrary.builder, jobs, 1, 3);
      if (flag2)
      {
        this.addToJob(CitizenJobLibrary.gatherer_bushes, jobs, 1, tasks.bushes, pJobMax1);
        this.addToJob(CitizenJobLibrary.gatherer_herbs, jobs, 1, tasks.plants, pJobMax1);
        this.addToJob(CitizenJobLibrary.gatherer_honey, jobs, 1, tasks.hives, pJobMax1);
      }
      if (flag3)
        this.addToJob(CitizenJobLibrary.farmer, jobs, 1, pCity.calculated_place_for_farms.Count);
      if (World.world_era.flag_crops_grow && flag3)
        this.addToJob(CitizenJobLibrary.farmer, jobs, 1, tasks.farms_total);
      if (flag2)
      {
        this.addToJob(CitizenJobLibrary.miner_deposit, jobs, 1, tasks.minerals);
        this.addToJob(CitizenJobLibrary.woodcutter, jobs, 1, tasks.trees, pJobMax2);
      }
      if (flag4)
        this.addToJob(CitizenJobLibrary.miner, jobs, 1, 5);
      if (flag1)
        this.addToJob(CitizenJobLibrary.attacker, jobs, 2, pTaskAmountMax);
      this.addToJob(CitizenJobLibrary.road_builder, jobs, 1, tasks.roads, 1);
      this.addToJob(CitizenJobLibrary.cleaner, jobs, 1, tasks.ruins, 1);
      if (flag2)
      {
        this.addToJob(CitizenJobLibrary.hunter, jobs, 1, 1);
        this.addToJob(CitizenJobLibrary.manure_cleaner, jobs, 1, tasks.poops, 3);
      }
      ++num1;
      if (num1 > num2)
        break;
    }
    return BehResult.Continue;
  }

  private void addToJob(
    CitizenJobAsset pJobAsset,
    CitizenJobs pJobsContainer,
    int pAdd,
    int pTaskAmountMax,
    int pJobMax = 0)
  {
    if (pAdd == 0 || pTaskAmountMax == 0)
      return;
    int num = pJobsContainer.countCurrentJobs(pJobAsset);
    if (num >= pTaskAmountMax || pJobMax != 0 && num >= pJobMax)
      return;
    int pValue = pAdd;
    if (this._citizens_left <= pValue)
      pValue = this._citizens_left;
    this._citizens_left -= pValue;
    pJobsContainer.addToJob(pJobAsset, pValue);
  }

  private int getPossibleWarriors(City pCity)
  {
    float armyMaxMultiplier = pCity.getArmyMaxMultiplier();
    return (int) ((double) pCity.status.population_adults * (double) armyMaxMultiplier);
  }

  private void countRoads(City pCity, CityTasksData pTasks)
  {
    if (pCity.road_tiles_to_build.Count <= 0)
      return;
    pTasks.roads = 1;
  }

  private void countResources(City pCity, CityTasksData pTasks)
  {
    bool flag1 = false;
    if (pCity.hasSpaceForResourceInStockpile(ResourceLibrary.berries))
      flag1 = true;
    bool flag2 = false;
    if (pCity.hasSpaceForResourceInStockpile(ResourceLibrary.herbs))
      flag2 = true;
    bool flag3 = false;
    if (pCity.hasSpaceForResourceInStockpile(ResourceLibrary.honey))
      flag3 = true;
    bool flag4 = false;
    if (pCity.hasSpaceForResourceInStockpile(ResourceLibrary.wood))
      flag4 = true;
    bool flag5 = false;
    if (pCity.hasSpaceForResourceInStockpile(ResourceLibrary.fertilizer))
      flag5 = true;
    for (int index = 0; index < pCity.zones.Count; ++index)
    {
      TileZone zone = pCity.zones[index];
      if (flag4)
        pTasks.trees += zone.countBuildingsType(BuildingList.Trees);
      pTasks.minerals += zone.countBuildingsType(BuildingList.Minerals);
      pTasks.ruins += zone.countBuildingsType(BuildingList.Ruins);
      if (flag1)
      {
        HashSet<Building> hashset = zone.getHashset(BuildingList.Food);
        if (hashset != null)
        {
          foreach (Building building in hashset)
          {
            if (building.hasResourcesToCollect())
              ++pTasks.bushes;
          }
        }
      }
      if (flag2)
        pTasks.plants += zone.countBuildingsType(BuildingList.Flora);
      if (flag5)
        pTasks.poops += zone.countBuildingsType(BuildingList.Poops);
      if (flag3)
      {
        HashSet<Building> hashset = zone.getHashset(BuildingList.Food);
        if (hashset != null)
        {
          foreach (Building building in hashset)
          {
            if (building.hasResourcesToCollect())
              ++pTasks.hives;
          }
        }
      }
      HashSet<WorldTile> tilesOfType = zone.getTilesOfType((TileTypeBase) TopTileLibrary.field);
      if (tilesOfType != null)
      {
        foreach (WorldTile worldTile in tilesOfType)
        {
          ++pTasks.farms_total;
          if (!worldTile.hasBuilding())
            ++pTasks.farm_fields;
          else if (worldTile.building.asset.wheat && worldTile.building.component_wheat.isMaxLevel())
            ++pTasks.wheats;
        }
      }
    }
  }

  private void countFires(City pCity, CityTasksData pTasks)
  {
    foreach (TileZone neighbourZone in pCity.neighbour_zones)
    {
      if (neighbourZone.city == null)
        pTasks.fire += WorldBehaviourActionFire.countFires(neighbourZone);
    }
    for (int index = 0; index < pCity.zones.Count; ++index)
    {
      TileZone zone = pCity.zones[index];
      pTasks.fire += WorldBehaviourActionFire.countFires(zone);
    }
  }

  private void checkOccupied(City pCity)
  {
    Dictionary<CitizenJobAsset, int> occupied = pCity.jobs.occupied;
    occupied.Clear();
    for (int index = 0; index < pCity.units.Count; ++index)
    {
      Actor unit = pCity.units[index];
      if (unit.citizen_job != null)
      {
        int num;
        occupied.TryGetValue(unit.citizen_job, out num);
        occupied[unit.citizen_job] = num + 1;
      }
    }
  }
}
