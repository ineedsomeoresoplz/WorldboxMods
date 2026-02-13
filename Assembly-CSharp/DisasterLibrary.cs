// Decompiled with JetBrains decompiler
// Type: DisasterLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;

#nullable disable
[ObfuscateLiterals]
public class DisasterLibrary : AssetLibrary<DisasterAsset>
{
  public override void init()
  {
    base.init();
    DisasterAsset pAsset1 = new DisasterAsset();
    pAsset1.id = "tornado";
    pAsset1.rate = 3;
    pAsset1.chance = 0.5f;
    pAsset1.min_world_cities = 3;
    pAsset1.world_log = "disaster_tornado";
    pAsset1.min_world_population = 100;
    pAsset1.type = DisasterType.Nature;
    this.add(pAsset1);
    this.t.ages_allow.Add("age_tears");
    this.t.ages_allow.Add("age_ash");
    this.t.ages_allow.Add("age_chaos");
    this.t.ages_allow.Add("age_wonders");
    this.t.ages_allow.Add("age_moon");
    this.t.action = new DisasterAction(this.spawnTornado);
    DisasterAsset pAsset2 = new DisasterAsset();
    pAsset2.id = "heatwave";
    pAsset2.rate = 4;
    pAsset2.chance = 0.5f;
    pAsset2.world_log = "disaster_heatwave";
    pAsset2.premium_only = false;
    pAsset2.type = DisasterType.Nature;
    this.add(pAsset2);
    this.t.ages_allow.Add("age_sun");
    this.t.action = new DisasterAction(this.spawnHeatwave);
    DisasterAsset pAsset3 = new DisasterAsset();
    pAsset3.id = "small_meteorite";
    pAsset3.rate = 5;
    pAsset3.chance = 0.5f;
    pAsset3.world_log = "disaster_meteorite";
    pAsset3.min_world_population = 400;
    pAsset3.min_world_cities = 3;
    pAsset3.premium_only = true;
    pAsset3.type = DisasterType.Nature;
    this.add(pAsset3);
    this.t.ages_forbid.Add("age_hope");
    this.t.ages_forbid.Add("age_sun");
    this.t.action = new DisasterAction(this.spawnMeteorite);
    DisasterAsset pAsset4 = new DisasterAsset();
    pAsset4.id = "small_earthquake";
    pAsset4.rate = 3;
    pAsset4.chance = 0.4f;
    pAsset4.world_log = "disaster_earthquake";
    pAsset4.min_world_population = 400;
    pAsset4.min_world_cities = 5;
    pAsset4.type = DisasterType.Nature;
    this.add(pAsset4);
    this.t.ages_forbid.Add("age_hope");
    this.t.ages_forbid.Add("age_sun");
    this.t.action = new DisasterAction(this.spawnSmallEarthquake);
    DisasterAsset pAsset5 = new DisasterAsset();
    pAsset5.id = "hellspawn";
    pAsset5.rate = 2;
    pAsset5.chance = 0.9f;
    pAsset5.min_world_cities = 5;
    pAsset5.world_log = "disaster_hellspawn";
    pAsset5.min_world_population = 300;
    pAsset5.premium_only = true;
    pAsset5.spawn_asset_unit = "demon";
    pAsset5.max_existing_units = 5;
    pAsset5.units_min = 2;
    pAsset5.units_max = 5;
    this.add(pAsset5);
    this.t.ages_allow.Add("age_chaos");
    this.t.action = new DisasterAction(this.simpleUnitAssetSpawnUsingIslands);
    DisasterAsset pAsset6 = new DisasterAsset();
    pAsset6.id = "ice_ones_awoken";
    pAsset6.rate = 2;
    pAsset6.chance = 0.9f;
    pAsset6.min_world_cities = 5;
    pAsset6.world_log = "disaster_ice_ones";
    pAsset6.min_world_population = 300;
    pAsset6.premium_only = true;
    pAsset6.spawn_asset_unit = "cold_one";
    pAsset6.max_existing_units = 5;
    pAsset6.units_min = 10;
    pAsset6.units_max = 20;
    this.add(pAsset6);
    this.t.ages_allow.Add("age_despair");
    this.t.ages_allow.Add("age_ice");
    this.t.action = new DisasterAction(this.simpleUnitAssetSpawnUsingIslands);
    DisasterAsset pAsset7 = new DisasterAsset();
    pAsset7.id = "sudden_snowman";
    pAsset7.rate = 3;
    pAsset7.chance = 0.9f;
    pAsset7.min_world_cities = 5;
    pAsset7.world_log = "disaster_sudden_snowman";
    pAsset7.min_world_population = 100;
    pAsset7.spawn_asset_unit = "snowman";
    pAsset7.max_existing_units = 5;
    pAsset7.units_min = 20;
    pAsset7.units_max = 40;
    this.add(pAsset7);
    this.t.ages_allow.Add("age_ice");
    this.t.action = new DisasterAction(this.simpleUnitAssetSpawnUsingIslands);
    DisasterAsset pAsset8 = new DisasterAsset();
    pAsset8.id = "garden_surprise";
    pAsset8.rate = 1;
    pAsset8.chance = 0.9f;
    pAsset8.min_world_cities = 5;
    pAsset8.world_log = "disaster_garden_surprise";
    pAsset8.min_world_population = 800;
    pAsset8.spawn_asset_building = "super_pumpkin";
    pAsset8.spawn_asset_unit = "lil_pumpkin";
    pAsset8.max_existing_units = 5;
    pAsset8.units_min = 50;
    pAsset8.units_max = 100;
    this.add(pAsset8);
    this.t.ages_allow.Add("age_sun");
    this.t.ages_allow.Add("age_wonders");
    this.t.action = new DisasterAction(this.gardenSurprise);
    DisasterAsset pAsset9 = new DisasterAsset();
    pAsset9.id = "dragon_from_farlands";
    pAsset9.rate = 1;
    pAsset9.chance = 0.9f;
    pAsset9.min_world_cities = 10;
    pAsset9.world_log = "disaster_dragon_from_farlands";
    pAsset9.min_world_population = 3000;
    pAsset9.spawn_asset_unit = "dragon";
    pAsset9.max_existing_units = 1;
    pAsset9.units_min = 1;
    pAsset9.units_max = 1;
    this.add(pAsset9);
    this.t.ages_allow.Add("age_chaos");
    this.t.ages_allow.Add("age_dark");
    this.t.ages_allow.Add("age_despair");
    this.t.action = new DisasterAction(this.spawnDragon);
    DisasterAsset pAsset10 = new DisasterAsset();
    pAsset10.id = "ash_bandits";
    pAsset10.rate = 1;
    pAsset10.chance = 0.9f;
    pAsset10.min_world_cities = 10;
    pAsset10.world_log = "disaster_bandits";
    pAsset10.min_world_population = 700;
    pAsset10.spawn_asset_unit = "bandit";
    pAsset10.max_existing_units = 10;
    pAsset10.units_min = 15;
    pAsset10.units_max = 30;
    this.add(pAsset10);
    this.t.ages_allow.Add("age_ash");
    this.t.action = new DisasterAction(this.simpleUnitAssetSpawnUsingIslands);
    DisasterAsset pAsset11 = new DisasterAsset();
    pAsset11.id = "alien_invasion";
    pAsset11.rate = 1;
    pAsset11.chance = 0.9f;
    pAsset11.min_world_cities = 10;
    pAsset11.world_log = "disaster_alien_invasion";
    pAsset11.min_world_population = 1500;
    pAsset11.spawn_asset_unit = "UFO";
    pAsset11.max_existing_units = 1;
    pAsset11.units_min = 5;
    pAsset11.units_max = 10;
    this.add(pAsset11);
    this.t.ages_allow.Add("age_moon");
    this.t.action = new DisasterAction(this.startAlienInvasion);
    DisasterAsset pAsset12 = new DisasterAsset();
    pAsset12.id = "biomass";
    pAsset12.rate = 1;
    pAsset12.chance = 0.9f;
    pAsset12.min_world_cities = 10;
    pAsset12.world_log = "disaster_biomass";
    pAsset12.min_world_population = 700;
    pAsset12.spawn_asset_building = "biomass";
    pAsset12.spawn_asset_unit = "bioblob";
    pAsset12.max_existing_units = 10;
    pAsset12.units_min = 20;
    pAsset12.units_max = 30;
    this.add(pAsset12);
    this.t.ages_allow.Add("age_ash");
    this.t.action = new DisasterAction(this.spawnBiomass);
    DisasterAsset pAsset13 = new DisasterAsset();
    pAsset13.id = "tumor";
    pAsset13.rate = 1;
    pAsset13.chance = 0.9f;
    pAsset13.min_world_cities = 10;
    pAsset13.world_log = "disaster_tumor";
    pAsset13.min_world_population = 700;
    pAsset13.spawn_asset_building = "tumor";
    pAsset13.spawn_asset_unit = "tumor_monster_unit";
    pAsset13.max_existing_units = 10;
    pAsset13.units_min = 20;
    pAsset13.units_max = 30;
    this.add(pAsset13);
    this.t.ages_allow.Add("age_moon");
    this.t.action = new DisasterAction(this.spawnTumor);
    DisasterAsset pAsset14 = new DisasterAsset();
    pAsset14.id = "wild_mage";
    pAsset14.rate = 1;
    pAsset14.chance = 0.8f;
    pAsset14.world_log = "disaster_evil_mage";
    pAsset14.min_world_population = 400;
    pAsset14.min_world_cities = 5;
    pAsset14.premium_only = true;
    pAsset14.max_existing_units = 1;
    pAsset14.spawn_asset_unit = "evil_mage";
    pAsset14.units_min = 1;
    pAsset14.units_max = 1;
    this.add(pAsset14);
    this.t.ages_forbid.Add("age_hope");
    this.t.action = new DisasterAction(this.spawnEvilMage);
    DisasterAsset pAsset15 = new DisasterAsset();
    pAsset15.id = "underground_necromancer";
    pAsset15.rate = 2;
    pAsset15.chance = 0.9f;
    pAsset15.world_log = "disaster_underground_necromancer";
    pAsset15.min_world_population = 200;
    pAsset15.min_world_cities = 4;
    pAsset15.premium_only = true;
    pAsset15.spawn_asset_unit = "necromancer";
    pAsset15.max_existing_units = 1;
    pAsset15.units_min = 1;
    pAsset15.units_max = 1;
    this.add(pAsset15);
    this.t.ages_allow.Add("age_dark");
    this.t.ages_allow.Add("age_despair");
    this.t.action = new DisasterAction(this.spawnNecromancer);
    DisasterAsset pAsset16 = new DisasterAsset();
    pAsset16.id = "mad_thoughts";
    pAsset16.rate = 2;
    pAsset16.chance = 0.7f;
    pAsset16.world_log = "disaster_mad_thoughts";
    pAsset16.min_world_cities = 5;
    pAsset16.min_world_population = 150;
    this.add(pAsset16);
    this.t.ages_forbid.Add("age_hope");
    this.t.ages_forbid.Add("age_wonders");
    this.t.action = new DisasterAction(this.spawnMadThought);
    DisasterAsset pAsset17 = new DisasterAsset();
    pAsset17.id = "greg_abominations";
    pAsset17.rate = 1;
    pAsset17.chance = 0.5f;
    pAsset17.world_log = "disaster_greg_abominations";
    pAsset17.min_world_population = 1000;
    pAsset17.min_world_cities = 3;
    pAsset17.spawn_asset_unit = "greg";
    pAsset17.max_existing_units = 1;
    pAsset17.units_min = 30;
    pAsset17.units_max = 55;
    this.add(pAsset17);
    this.t.ages_allow.Add("age_despair");
    this.t.action = new DisasterAction(this.spawnGreg);
  }

  public DisasterAsset getRandomAssetFromPool()
  {
    using (ListPool<DisasterAsset> list = new ListPool<DisasterAsset>())
    {
      for (int index1 = 0; index1 < this.list.Count; ++index1)
      {
        DisasterAsset disasterAsset = this.list[index1];
        if ((disasterAsset.ages_allow.Count <= 0 || disasterAsset.ages_allow.Contains(World.world_era.id)) && (disasterAsset.ages_forbid.Count <= 0 || !disasterAsset.ages_forbid.Contains(World.world_era.id)))
        {
          for (int index2 = 0; index2 < disasterAsset.rate; ++index2)
            list.Add(disasterAsset);
        }
      }
      if (list.Count == 0)
        return (DisasterAsset) null;
      DisasterAsset random = list.GetRandom<DisasterAsset>();
      if (random.type == DisasterType.Nature)
      {
        if (!WorldLawLibrary.world_law_disasters_nature.isEnabled())
          return (DisasterAsset) null;
      }
      else if (random.type == DisasterType.Other && !WorldLawLibrary.world_law_disasters_other.isEnabled())
        return (DisasterAsset) null;
      return random.min_world_cities > World.world.cities.Count || random.min_world_population > World.world.units.Count ? (DisasterAsset) null : random;
    }
  }

  public void spawnMadThought(DisasterAsset pAsset)
  {
    City random = World.world.cities.getRandom();
    if (random == null || random.getPopulationPeople() < 50 || random.getTile() == null)
      return;
    using (ListPool<Actor> listPool = new ListPool<Actor>(random.countUnits()))
    {
      foreach (Actor actor in random.units.LoopRandom<Actor>())
      {
        if (actor.city == random && Randy.randomChance(0.2f))
          listPool.Add(actor);
      }
      for (int index = 0; index < listPool.Count; ++index)
        listPool[index].addTrait("madness");
      WorldLog.logDisaster(pAsset, random.getTile(), pCity: random);
    }
  }

  public void spawnGreg(DisasterAsset pAsset)
  {
    if (!DebugConfig.isOn(DebugOption.Greg) || !this.checkUnitSpawnLimits(pAsset))
      return;
    City random = World.world.cities.getRandom();
    if (random == null)
      return;
    Building buildingOfType = random.getBuildingOfType("type_mine");
    if (buildingOfType == null)
      return;
    WorldTile currentTile = buildingOfType.current_tile;
    Actor pUnit = this.spawnDisasterUnit(pAsset, currentTile);
    WorldLog.logDisaster(pAsset, currentTile, pUnit.getName(), random, pUnit);
    this.spawnDisasterUnit(pAsset, currentTile.region.tiles.GetRandom<WorldTile>());
    AchievementLibrary.greg.check();
  }

  public void spawnNecromancer(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    City random = World.world.cities.getRandom();
    if (random == null)
      return;
    Building buildingOfType = random.getBuildingOfType("type_mine");
    if (buildingOfType == null)
      return;
    WorldTile currentTile = buildingOfType.current_tile;
    Actor pUnit = this.spawnDisasterUnit(pAsset, currentTile);
    WorldLog.logDisaster(pAsset, currentTile, pUnit.getName(), random, pUnit);
    int num = Randy.randomInt(5, 25);
    for (int index = 0; index < num; ++index)
      World.world.units.createNewUnit("skeleton", currentTile.region.tiles.GetRandom<WorldTile>(), pAdultAge: true);
  }

  public void spawnEvilMage(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    TileIsland randomIslandGround = World.world.islands_calculator.getRandomIslandGround();
    if (randomIslandGround == null)
      return;
    WorldTile randomTile = randomIslandGround.getRandomTile();
    Actor pUnit = this.spawnDisasterUnit(pAsset, randomTile);
    WorldLog.logDisaster(pAsset, randomTile, pUnit.getName(), pUnit: pUnit);
  }

  public void spawnHeatwave(DisasterAsset pAsset)
  {
    if (!WorldLawLibrary.world_law_disasters_nature.isEnabled())
      return;
    int num = Randy.randomInt(1, 3);
    WorldTile pTile = (WorldTile) null;
    bool flag = false;
    for (int index = 0; index < num; ++index)
    {
      TileIsland randomIslandGround = World.world.islands_calculator.getRandomIslandGround();
      if (randomIslandGround != null)
      {
        pTile = randomIslandGround.getRandomTile();
        flag = true;
        foreach (WorldTile worldTile in pTile.neighboursAll)
          worldTile.startFire();
      }
    }
    if (!flag)
      return;
    WorldLog.logDisaster(pAsset, pTile);
  }

  public void spawnSmallEarthquake(DisasterAsset pAsset)
  {
    WorldTile random = World.world.tiles_list.GetRandom<WorldTile>();
    Earthquake.startQuake(random, EarthquakeType.SmallDisaster);
    WorldLog.logDisaster(pAsset, random);
  }

  public void spawnDragon(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    WorldTile centerTile = (!Randy.randomBool() ? World.world.zone_calculator.getZone(World.world.zone_calculator.zones_total_x - 1, Randy.randomInt(0, World.world.zone_calculator.zones_total_y)) : World.world.zone_calculator.getZone(0, Randy.randomInt(0, World.world.zone_calculator.zones_total_y))).centerTile;
    this.spawnDisasterUnit(pAsset, centerTile);
    WorldLog.logDisaster(pAsset, centerTile);
  }

  public void startAlienInvasion(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    WorldTile centerTile = (!Randy.randomBool() ? World.world.zone_calculator.getZone(World.world.zone_calculator.zones_total_x - 1, Randy.randomInt(0, World.world.zone_calculator.zones_total_y)) : World.world.zone_calculator.getZone(0, Randy.randomInt(0, World.world.zone_calculator.zones_total_y))).centerTile;
    this.spawnDisasterUnit(pAsset, centerTile);
    WorldLog.logDisaster(pAsset, centerTile);
  }

  public void spawnBiomass(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    Building building = (Building) null;
    foreach (City city in World.world.cities.list.LoopRandom<City>())
    {
      Building random = Randy.getRandom<Building>(city.buildings);
      if (random != null && random.isUsable())
      {
        building = random;
        break;
      }
    }
    if (building == null)
      return;
    WorldTile random1 = building.tiles.GetRandom<WorldTile>();
    if (this.spawnDisasterBuilding(pAsset, random1) == null)
      return;
    this.spawnDisasterUnit(pAsset, random1);
    WorldLog.logDisaster(pAsset, random1);
  }

  public void spawnTumor(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    Building building = (Building) null;
    foreach (City city in World.world.cities.list.LoopRandom<City>())
    {
      Building random = Randy.getRandom<Building>(city.buildings);
      if (random != null && random.isUsable())
      {
        building = random;
        break;
      }
    }
    if (building == null)
      return;
    WorldTile random1 = building.tiles.GetRandom<WorldTile>();
    if (this.spawnDisasterBuilding(pAsset, random1) == null)
      return;
    this.spawnDisasterUnit(pAsset, random1);
    WorldLog.logDisaster(pAsset, random1);
  }

  public void gardenSurprise(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    Building building = (Building) null;
    foreach (City city in World.world.cities.list.LoopRandom<City>())
    {
      Building buildingOfType = city.getBuildingOfType("type_windmill");
      if (buildingOfType != null && buildingOfType.isAlive())
      {
        building = buildingOfType;
        break;
      }
    }
    if (building == null)
      return;
    WorldTile random = building.tiles.GetRandom<WorldTile>();
    if (this.spawnDisasterBuilding(pAsset, random) == null)
      return;
    this.spawnDisasterUnit(pAsset, random);
    WorldLog.logDisaster(pAsset, random);
  }

  public void spawnTornado(DisasterAsset pAsset)
  {
    WorldTile random = World.world.tiles_list.GetRandom<WorldTile>();
    EffectsLibrary.spawnAtTile("fx_tornado", random, 0.5f);
    WorldLog.logDisaster(pAsset, random);
  }

  public void spawnMeteorite(DisasterAsset pAsset)
  {
    WorldTile random = World.world.tiles_list.GetRandom<WorldTile>();
    Meteorite.spawnMeteoriteDisaster(random);
    WorldLog.logDisaster(pAsset, random);
  }

  public void simpleUnitAssetSpawnUsingIslands(DisasterAsset pAsset)
  {
    if (!this.checkUnitSpawnLimits(pAsset))
      return;
    TileIsland randomIslandGround = World.world.islands_calculator.getRandomIslandGround();
    if (randomIslandGround == null)
      return;
    WorldTile randomTile = randomIslandGround.getRandomTile();
    this.spawnDisasterUnit(pAsset, randomTile);
    WorldLog.logDisaster(pAsset, randomTile);
  }

  private bool checkUnitSpawnLimits(DisasterAsset pAsset)
  {
    return !string.IsNullOrEmpty(pAsset.spawn_asset_unit) && AssetManager.actor_library.get(pAsset.spawn_asset_unit).units.Count < pAsset.max_existing_units;
  }

  private Actor spawnDisasterUnit(DisasterAsset pAsset, WorldTile pTile)
  {
    EffectsLibrary.spawn("fx_spawn", pTile);
    Actor actor = (Actor) null;
    int num = Randy.randomInt(pAsset.units_min, pAsset.units_max);
    for (int index = 0; index < num; ++index)
      actor = World.world.units.createNewUnit(pAsset.spawn_asset_unit, pTile, pAdultAge: true, pGiveOwnerlessItems: true);
    return actor;
  }

  private Building spawnDisasterBuilding(DisasterAsset pAsset, WorldTile pTile)
  {
    return string.IsNullOrEmpty(pAsset.spawn_asset_building) ? (Building) null : World.world.buildings.addBuilding(pAsset.spawn_asset_building, pTile, true);
  }
}
