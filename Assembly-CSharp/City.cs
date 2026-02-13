// Decompiled with JetBrains decompiler
// Type: City
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class City : MetaObject<CityData>
{
  private static readonly HashSet<City> _connected_checked = new HashSet<City>();
  private static readonly HashSet<City> _connected_next_wave = new HashSet<City>();
  private static readonly HashSet<City> _connected_current_wave = new HashSet<City>();
  private readonly Dictionary<string, CityStorageSlot> _total_resource_slots = new Dictionary<string, CityStorageSlot>();
  private readonly Dictionary<UnitProfession, List<Actor>> _professions_dict = new Dictionary<UnitProfession, List<Actor>>();
  private readonly List<Actor> _boats = new List<Actor>();
  private readonly Dictionary<string, long> _species = new Dictionary<string, long>();
  public readonly List<Building> buildings = new List<Building>();
  public readonly Dictionary<string, List<Building>> buildings_dict_type = new Dictionary<string, List<Building>>();
  public readonly Dictionary<string, List<Building>> buildings_dict_id = new Dictionary<string, List<Building>>();
  public readonly CityTasksData tasks = new CityTasksData();
  public readonly CitizenJobs jobs = new CitizenJobs();
  public readonly CityStatus status = new CityStatus();
  public float mark_scale_effect;
  [NonSerialized]
  internal Kingdom kingdom;
  public Culture culture;
  public Language language;
  public Religion religion;
  public Actor leader;
  public Army army;
  internal readonly List<TileZone> zones = new List<TileZone>();
  internal readonly HashSet<TileZone> neighbour_zones = new HashSet<TileZone>();
  internal readonly HashSet<TileZone> border_zones = new HashSet<TileZone>();
  internal readonly HashSet<City> neighbours_cities = new HashSet<City>();
  internal readonly HashSet<City> neighbours_cities_kingdom = new HashSet<City>();
  internal readonly HashSet<Kingdom> neighbours_kingdoms = new HashSet<Kingdom>();
  internal Building under_construction_building;
  internal readonly List<Building> stockpiles = new List<Building>();
  internal readonly List<Building> storages = new List<Building>();
  internal float timer_build_boat;
  internal float timer_build;
  public float timer_action;
  private float _timer_capture;
  private float _timer_warrior;
  internal readonly List<WorldTile> road_tiles_to_build = new List<WorldTile>();
  private readonly List<WorldTile> tiles_to_remove = new List<WorldTile>();
  internal TileZone target_attack_zone;
  internal City target_attack_city;
  internal WorldTile _city_tile;
  internal string _debug_last_possible_build_orders;
  internal string _debug_last_possible_build_orders_no_resources;
  internal string _debug_last_build_order_try;
  internal Kingdom being_captured_by;
  private float _capture_ticks;
  public int last_visual_capture_ticks;
  private bool _dirty_citizens;
  private bool _dirty_city_status;
  private bool _dirty_abandoned_zones;
  internal Vector2 city_center;
  internal Vector2 last_city_center;
  public readonly WorldTileContainer calculated_place_for_farms = new WorldTileContainer();
  public readonly WorldTileContainer calculated_farm_fields = new WorldTileContainer();
  public readonly WorldTileContainer calculated_crops = new WorldTileContainer();
  public readonly WorldTileContainer calculated_grown_wheat = new WorldTileContainer();
  private readonly Dictionary<Kingdom, int> _capturing_units = new Dictionary<Kingdom, int>();
  internal readonly HashSet<TileZone> danger_zones = new HashSet<TileZone>();
  public AiSystemCity ai;
  private int _current_total_food;
  private int _last_checked_job_id;
  private double _loyalty_last_time;
  private int _loyalty_cached;
  private readonly List<long> _cached_book_ids = new List<long>();
  private readonly List<Building> _cached_buildings_with_book_slots = new List<Building>();
  public double timestamp_shrink;
  private int _storage_version;

  protected override MetaType meta_type => MetaType.City;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.cities;

  protected override bool track_death_types => true;

  public int getStorageVersion() => this._storage_version;

  public override void increaseBirths()
  {
    base.increaseBirths();
    this.addRenown(1);
  }

  public void increaseLeft()
  {
    if (!this.isAlive())
      return;
    ++this.data.left;
  }

  public void increaseJoined()
  {
    if (!this.isAlive())
      return;
    ++this.data.joined;
    this.addRenown(1);
  }

  public void increaseMoved()
  {
    if (!this.isAlive())
      return;
    ++this.data.moved;
    this.addRenown(2);
  }

  public void increaseMigrants()
  {
    if (!this.isAlive())
      return;
    ++this.data.migrated;
  }

  public long getTotalLeft() => this.data.left;

  public long getTotalJoined() => this.data.joined;

  public long getTotalMoved() => this.data.moved;

  public long getTotalMigrated() => this.data.migrated;

  public bool isZoneToClaimStillGood(Actor pActor, TileZone pZone, WorldTile pCityTile)
  {
    if (!pZone.canBeClaimedByCity(this) || !pZone.checkCanSettleInThisBiomes(pActor.subspecies))
      return false;
    foreach (TileZone neighbour in pZone.neighbours)
    {
      if (neighbour.hasCity() && neighbour.city == this)
        return true;
    }
    return false;
  }

  internal override void clearListUnits()
  {
    base.clearListUnits();
    this._boats.Clear();
    this._species.Clear();
  }

  public override ActorAsset getActorAsset()
  {
    return this.hasLeader() ? this.leader.getActorAsset() : this.getFounderSpecies();
  }

  public ActorAsset getFounderSpecies()
  {
    return AssetManager.actor_library.get(this.data.original_actor_asset);
  }

  public CityLayoutTilePlacement getTilePlacementFromZone()
  {
    if (this.hasCulture())
    {
      if (this.culture.hasTrait("city_layout_the_grand_arrangement"))
        return CityLayoutTilePlacement.CenterTile;
      if (this.culture.hasTrait("city_layout_tile_wobbly_pattern"))
        return CityLayoutTilePlacement.CenterTileDrunk;
      if (this.culture.hasTrait("city_layout_tile_moonsteps"))
        return CityLayoutTilePlacement.Moonsteps;
    }
    return CityLayoutTilePlacement.Random;
  }

  public string getSpecies() => this.getActorAsset().id;

  public override bool isReadyForRemoval() => this.zones.Count == 0;

  public void clearBuildingList()
  {
    this.buildings.Clear();
    foreach (List<Building> buildingList in this.buildings_dict_type.Values)
      buildingList.Clear();
    foreach (List<Building> buildingList in this.buildings_dict_id.Values)
      buildingList.Clear();
    this.stockpiles.Clear();
    this.storages.Clear();
    this._cached_book_ids.Clear();
    this._cached_buildings_with_book_slots.Clear();
  }

  public override void listUnit(Actor pActor)
  {
    if (pActor.asset.is_boat)
    {
      this._boats.Add(pActor);
    }
    else
    {
      this.units.Add(pActor);
      if (!pActor.hasSubspecies())
        return;
      this._species[pActor.asset.id] = pActor.subspecies.id;
    }
  }

  public Subspecies getSubspecies(string pSpeciesId)
  {
    return World.world.subspecies.get(this.getSubspeciesId(pSpeciesId));
  }

  public long getSubspeciesId(string pSpeciesId)
  {
    long num;
    return this._species.TryGetValue(pSpeciesId, out num) ? num : -1L;
  }

  public bool hasFreeHouseSlots() => this.status.housing_free != 0;

  public bool hasReachedWorldLawLimit()
  {
    return WorldLawLibrary.world_law_civ_limit_population_100.isEnabled() && this.getPopulationPeople() >= 100;
  }

  public void listBuilding(Building pBuilding)
  {
    this.buildings.Add(pBuilding);
    BuildingAsset asset = pBuilding.asset;
    if (asset.type == "type_stockpile")
      this.stockpiles.Add(pBuilding);
    if (asset.storage)
      this.storages.Add(pBuilding);
    if (asset.book_slots > 0)
    {
      this._cached_buildings_with_book_slots.Add(pBuilding);
      if (pBuilding.data.books != null)
        this._cached_book_ids.AddRange((IEnumerable<long>) pBuilding.data.books.list_books);
    }
    this.setBuildingDictType(pBuilding);
    this.setBuildingDictID(pBuilding);
  }

  [CanBeNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public WorldTile getTile(bool pForceRecalc = false)
  {
    if (this._city_tile == null | pForceRecalc)
      this.recalculateCityTile();
    return this._city_tile;
  }

  internal void recalculateCityTile()
  {
    this._city_tile = (WorldTile) null;
    Building building1 = this.getBuildingOfType("type_bonfire");
    if (building1 != null)
    {
      this._city_tile = building1.current_tile;
    }
    else
    {
      foreach (Building building2 in this.buildings.LoopRandom<Building>())
      {
        if (!building2.asset.docks && !building2.current_tile.Type.ocean)
        {
          if (building1 == null)
            building1 = building2;
          else if (building2.asset.priority > building1.asset.priority)
            building1 = building2;
        }
      }
      if (building1 != null)
      {
        this._city_tile = building1.current_tile;
      }
      else
      {
        List<TileZone> zones = this.zones;
        if (zones.Count == 0)
          return;
        for (int index = 0; index < zones.Count; ++index)
        {
          TileZone tileZone = zones[index];
          if (!tileZone.centerTile.Type.ocean)
          {
            this._city_tile = tileZone.centerTile;
            break;
          }
        }
      }
    }
  }

  internal int countInHouses()
  {
    int num = 0;
    List<Actor> units = this.units;
    for (int index = 0; index < units.Count; ++index)
    {
      if (units[index].is_inside_building)
        ++num;
    }
    return num;
  }

  public int countBookSlots()
  {
    int num = 0;
    for (int index = 0; index < this._cached_buildings_with_book_slots.Count; ++index)
    {
      Building buildingsWithBookSlot = this._cached_buildings_with_book_slots[index];
      num += buildingsWithBookSlot.asset.book_slots;
    }
    return num;
  }

  public bool hasBookSlots()
  {
    int num = this.countBookSlots();
    return this.countBooks() < num;
  }

  public Building getBuildingWithBookSlot()
  {
    foreach (Building buildingsWithBookSlot in this._cached_buildings_with_book_slots)
    {
      if (buildingsWithBookSlot.hasFreeBookSlot())
        return buildingsWithBookSlot;
    }
    return (Building) null;
  }

  public int countBooks() => this._cached_book_ids.Count;

  private void setKingdomTimestamp() => this.data.timestamp_kingdom = World.world.getCurWorldTime();

  public override ColorAsset getColor() => this.kingdom.getColor();

  internal void setKingdom(Kingdom pKingdom, bool pFromLoad = false)
  {
    World.world.kingdoms.setDirtyCities();
    if (this.isCapitalCity())
      this.kingdom.clearCapital();
    this.kingdom = pKingdom;
    if (this.kingdom != null && this.kingdom != WildKingdomsManager.neutral)
      this.data.last_kingdom_id = this.kingdom.id;
    if (pFromLoad)
      return;
    this.checkArmyExistence();
    if (!this.hasArmy())
      return;
    this.army.checkCity();
  }

  internal void newForceKingdomEvent(
    List<Actor> pUnits,
    List<Actor> pBoats,
    Kingdom pKingdom,
    string pHappinessEvent)
  {
    this.setKingdomTimestamp();
    this.forceUnitsIntoThisKingdom(pUnits, pKingdom, false, pHappinessEvent);
    this.forceUnitsIntoThisKingdom(pBoats, pKingdom, true);
  }

  internal void forceBuildingsToKingdom(List<Building> pBuildings, Kingdom pKingdom)
  {
    for (int index = 0; index < pBuildings.Count; ++index)
      pBuildings[index].setKingdom(pKingdom);
  }

  internal void forceUnitsIntoThisKingdom(
    List<Actor> pActors,
    Kingdom pKingdom,
    bool pBoats,
    string pHappinessEvent = null)
  {
    if (pBoats)
    {
      for (int index = 0; index < pActors.Count; ++index)
      {
        Actor pActor = pActors[index];
        if (!pActor.isRekt())
          pActor.joinKingdom(pKingdom);
      }
    }
    else
    {
      for (int index = 0; index < pActors.Count; ++index)
      {
        Actor pActor = pActors[index];
        if (!pActor.isRekt())
        {
          if (pActor.isKing())
          {
            if (pActor.city == this && pActor.kingdom != pKingdom)
              pActor.kingdom.kingLeftEvent();
            else
              continue;
          }
          pActor.joinKingdom(pKingdom);
          if (pHappinessEvent != null)
            pActor.changeHappiness(pHappinessEvent);
        }
      }
    }
  }

  internal Building getStorageNear(WorldTile pTile, bool pOnlyFood = false)
  {
    Building storageNear = (Building) null;
    int num1 = int.MaxValue;
    List<Building> storages = this.storages;
    for (int index = 0; index < storages.Count; ++index)
    {
      Building building = storages[index];
      if (building.isUsable() && building.current_tile.isSameIsland(pTile))
      {
        if (pOnlyFood && building.asset.storage_only_food)
        {
          storageNear = building;
        }
        else
        {
          int num2 = Toolbox.SquaredDistVec2(building.current_tile.pos, pTile.pos);
          if (num2 < num1)
          {
            num1 = num2;
            storageNear = building;
          }
        }
      }
    }
    return storageNear;
  }

  internal Building getStorageWithFoodNear(WorldTile pTile)
  {
    Building storageWithFoodNear = (Building) null;
    int num1 = int.MaxValue;
    List<Building> storages = this.storages;
    for (int index = 0; index < storages.Count; ++index)
    {
      Building building = storages[index];
      if (building.isUsable() && building.current_tile.isSameIsland(pTile) && building.countFood() != 0)
      {
        int num2 = Toolbox.SquaredDistVec2(building.current_tile.pos, pTile.pos);
        if (num2 < num1)
        {
          num1 = num2;
          storageWithFoodNear = building;
        }
      }
    }
    return storageWithFoodNear;
  }

  internal bool hasStorageBuilding()
  {
    List<Building> storages = this.storages;
    for (int index = 0; index < storages.Count; ++index)
    {
      if (!storages[index].isUnderConstruction())
        return true;
    }
    return false;
  }

  public WorldTile getRoadTileToBuild(Actor pBuilder)
  {
    this.tiles_to_remove.Clear();
    for (int index = 0; index < this.road_tiles_to_build.Count; ++index)
    {
      WorldTile worldTile = this.road_tiles_to_build[index];
      if (worldTile.Type.road)
        this.tiles_to_remove.Add(worldTile);
    }
    for (int index = 0; index < this.tiles_to_remove.Count; ++index)
      this.road_tiles_to_build.Remove(this.tiles_to_remove[index]);
    this.tiles_to_remove.Clear();
    return this.road_tiles_to_build.Count > 0 ? this.road_tiles_to_build[0] : (WorldTile) null;
  }

  internal void init()
  {
    this.createAI();
    this.setStatusDirty();
  }

  private void createAI()
  {
    if (!Globals.AI_TEST_ACTIVE)
      return;
    if (this.ai == null)
      this.ai = new AiSystemCity(this);
    this.ai.next_job_delegate = new GetNextJobID(this.getNextJob);
    this.ai.jobs_library = (AssetLibrary<JobCityAsset>) AssetManager.job_city;
    this.ai.task_library = (AssetLibrary<BehaviourTaskCity>) AssetManager.tasks_city;
    this.ai.addSingleTask("build");
    this.ai.addSingleTask("check_loyalty");
    this.ai.addSingleTask("check_destruction");
  }

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this.mark_scale_effect = 1f;
    this.timer_build_boat = 10f;
    this.timer_build = 0.0f;
    this.timer_action = 0.0f;
    this._timer_capture = 0.0f;
    this._timer_warrior = 0.0f;
    this._capture_ticks = 0.0f;
    this.last_visual_capture_ticks = 0;
    this._dirty_citizens = true;
    this._dirty_city_status = false;
    this._dirty_abandoned_zones = false;
    this._current_total_food = 0;
    this._last_checked_job_id = 0;
    this._loyalty_last_time = -1.0;
    this._loyalty_cached = -1;
  }

  private string getNextJob() => "city";

  public bool isValidTargetForWar() => this.hasZones();

  public bool hasZones() => this.zones.Count > 0;

  public bool needSettlers()
  {
    int populationPeople = this.getPopulationPeople();
    return this.getAge() < 5 || populationPeople < 22 && (populationPeople >= 22 || this.status.housing_free != 0 || this.getAge() <= 10 || this.getHouseCurrent() <= 2);
  }

  internal void generateName(Actor pActor)
  {
    this.setName(pActor.generateName(MetaType.City, this.getID()));
    CityData data = this.data;
    Culture culture = this.culture;
    long num = culture != null ? culture.id : -1L;
    data.name_culture_id = num;
  }

  public void loadLeader()
  {
    if (!this.data.leaderID.hasValue())
      return;
    this.setLeader(World.world.units.get(this.data.leaderID), false);
  }

  public void newCityEvent(Actor pActor)
  {
    this.recalculateCityTile();
    this.generateName(pActor);
  }

  private void loadCityZones(List<ZoneData> pZoneData)
  {
    if (pZoneData == null)
      return;
    for (int index = 0; index < pZoneData.Count; ++index)
    {
      ZoneData zoneData = pZoneData[index];
      TileZone zone = World.world.zone_calculator.getZone(zoneData.x, zoneData.y);
      if (zone != null)
        this.addZone(zone);
    }
  }

  public void loadCity(CityData pData)
  {
    this.loadCityZones(pData.zones);
    this.setData(pData);
    if (this.data.id_culture.hasValue())
      this.setCulture(World.world.cultures.get(this.data.id_culture));
    if (this.data.id_language.hasValue())
      this.setLanguage(World.world.languages.get(this.data.id_language));
    if (this.data.id_religion.hasValue())
      this.setReligion(World.world.religions.get(this.data.id_religion));
    if (this.data.equipment == null)
      this.data.equipment = new CityEquipment();
    else
      this.data.equipment.loadFromSave(this);
    this.setKingdom(!pData.kingdomID.hasValue() || pData.kingdomID == 0L ? WildKingdomsManager.neutral : World.world.kingdoms.get(pData.kingdomID), true);
  }

  public void forceDoChecks()
  {
    this.updateTotalFood();
    this.updateCitizens();
    this.updateCityStatus();
  }

  public void executeAllActionsForCity()
  {
    AssetManager.tasks_city.get("do_initial_load_check").executeAllActionsForCity(this);
  }

  public void eventUnitAdded(Actor pActor)
  {
    if (!pActor.asset.is_boat)
      this.setCitizensDirty();
    this.setStatusDirty();
  }

  public void eventUnitRemoved(Actor pActor)
  {
    this.setStatusDirty();
    this.setCitizensDirty();
    if (!pActor.isCityLeader())
      return;
    this.removeLeader();
  }

  public void setAbandonedZonesDirty() => this._dirty_abandoned_zones = true;

  public void setCitizensDirty() => this._dirty_citizens = true;

  public void setStatusDirty() => this._dirty_city_status = true;

  private void sortZonesByDistanceToCenter()
  {
    WorldTile tile = this.getTile();
    if (tile == null)
      return;
    Vector2Int tCenterPos = tile.pos;
    this.zones.Sort((Comparison<TileZone>) ((a, b) => Toolbox.SquaredDistVec2(a.centerTile.pos, tCenterPos).CompareTo(Toolbox.SquaredDistVec2(b.centerTile.pos, tCenterPos))));
  }

  private void updateCityStatus()
  {
    this._dirty_city_status = false;
    this.status.clear();
    this.recalculateCityTile();
    this.sortZonesByDistanceToCenter();
    this.recalculateNeighbourZones();
    this.recalculateNeighbourCities();
    List<Building> buildings = this.buildings;
    int num = this.countPopulationChildren();
    this.status.population = this.getPopulationPeople();
    this.status.population_adults = this.status.population - num;
    this.status.population_children = num;
    MetaObject<CityData>._family_counter.Clear();
    List<Actor> units = this.units;
    for (int index = 0; index < units.Count; ++index)
    {
      Actor actor = units[index];
      if (actor.isHungry())
        ++this.status.hungry;
      if (actor.isSexMale())
        ++this.status.males;
      else
        ++this.status.females;
      if (actor.hasFamily())
        MetaObject<CityData>._family_counter.Add(actor.family);
      if (actor.isSick())
        ++this.status.sick;
      if (actor.hasHouse())
        ++this.status.housed;
      else
        ++this.status.homeless;
    }
    this.status.families = MetaObject<CityData>._family_counter.Count;
    MetaObject<CityData>._family_counter.Clear();
    for (int index = 0; index < buildings.Count; ++index)
    {
      Building building = buildings[index];
      if (!building.isUnderConstruction() && building.asset.hasHousingSlots())
        this.status.housing_total += building.asset.housing_slots;
    }
    this.status.housing_occupied = this.status.population <= this.status.housing_total ? this.status.population : this.status.housing_total;
    this.status.housing_free = this.status.housing_total - this.status.housing_occupied;
    this.status.maximum_items = 15;
    this.recalculateMaxHouses();
    this.status.warrior_slots = this.jobs.countCurrentJobs(CitizenJobLibrary.attacker);
    this.status.warriors_current = this.countProfession(UnitProfession.Warrior);
    CityBehCheckFarms.check(this);
  }

  private void recalculateMaxHouses()
  {
    if (DebugConfig.isOn(DebugOption.CityUnlimitedHouses))
    {
      this.status.houses_max = 9999;
    }
    else
    {
      float num = (float) this.zones.Count;
      if (this.hasCulture())
      {
        if (this.culture.hasTrait("dense_dwellings"))
          num = (float) (this.zones.Count * 2);
        if (this.culture.hasTrait("solitude_seekers"))
          num = (float) this.zones.Count / 3f;
        if (this.culture.hasTrait("hive_society"))
          num = (float) this.zones.Count * 3f;
      }
      foreach (Building building in this.buildings)
        num += (float) building.asset.max_houses;
      this.status.houses_max = (int) num;
    }
  }

  public bool hasBooksToRead(Actor pActor)
  {
    if (pActor.hasTag("can_read_any_book"))
      return this.countBooks() > 0;
    return pActor.hasLanguage() && this.hasBooksOfLanguage(pActor.language);
  }

  public bool hasBooksOfLanguage(Language pLanguage)
  {
    int index1 = 0;
    for (int index2 = this.countBooks(); index1 < index2; ++index1)
    {
      Book book = World.world.books.get(this._cached_book_ids[index1]);
      if (book.isAlive() && book.isReadyToBeRead())
      {
        Language language = book.getLanguage();
        if (language.id == pLanguage.id || language.hasTrait("magic_words"))
          return true;
      }
    }
    return false;
  }

  public Book getRandomBookOfLanguage(Language pLanguage)
  {
    using (ListPool<Book> list = new ListPool<Book>())
    {
      int index1 = 0;
      for (int index2 = this.countBooks(); index1 < index2; ++index1)
      {
        Book book = World.world.books.get(this._cached_book_ids[index1]);
        if (book.isReadyToBeRead())
        {
          Language language = book.getLanguage();
          if (language.id == pLanguage.id || language.hasTrait("magic_words"))
            list.Add(book);
        }
      }
      return list.Count == 0 ? (Book) null : list.GetRandom<Book>();
    }
  }

  public Book getRandomBook()
  {
    using (ListPool<Book> list = new ListPool<Book>())
    {
      int index1 = 0;
      for (int index2 = this.countBooks(); index1 < index2; ++index1)
      {
        Book book = World.world.books.get(this._cached_book_ids[index1]);
        if (book.isReadyToBeRead())
          list.Add(book);
      }
      return list.Count == 0 ? (Book) null : list.GetRandom<Book>();
    }
  }

  public List<long> getBooks() => this._cached_book_ids;

  public int getHouseCurrent() => this.countBuildingsType("type_house", false);

  public int getHouseLimit() => this.status.houses_max;

  public bool isConnectedToCapital()
  {
    if (!this.kingdom.hasCapital())
      return false;
    this.recalculateNeighbourCities();
    if (this.neighbours_cities_kingdom.Contains(this))
      return true;
    this.kingdom.calculateNeighbourCities();
    City._connected_checked.Clear();
    City._connected_next_wave.Clear();
    City._connected_current_wave.Clear();
    City._connected_next_wave.UnionWith((IEnumerable<City>) this.kingdom.capital.neighbours_cities_kingdom);
    int num = 0;
    while (City._connected_next_wave.Count > 0)
    {
      City._connected_current_wave.UnionWith((IEnumerable<City>) City._connected_next_wave);
      City._connected_next_wave.Clear();
      ++num;
      foreach (City city1 in City._connected_current_wave)
      {
        if (city1 == this)
          return true;
        City._connected_checked.Add(city1);
        foreach (City city2 in city1.neighbours_cities_kingdom)
        {
          if (!City._connected_checked.Contains(city2))
            City._connected_next_wave.Add(city2);
        }
      }
      if (num > 30)
        break;
    }
    return false;
  }

  public void recalculateNeighbourCities()
  {
    this.neighbours_cities.Clear();
    this.neighbours_cities_kingdom.Clear();
    this.neighbours_kingdoms.Clear();
    foreach (TileZone neighbourZone in this.neighbour_zones)
    {
      City city = neighbourZone.city;
      if (city != this && city != null)
      {
        this.neighbours_cities.Add(city);
        if (city.kingdom == this.kingdom)
          this.neighbours_cities_kingdom.Add(city);
        else
          this.neighbours_kingdoms.Add(city.kingdom);
      }
    }
  }

  public void recalculateNeighbourZones()
  {
    this.border_zones.Clear();
    this.neighbour_zones.Clear();
    List<TileZone> zones = this.zones;
    for (int index = 0; index < zones.Count; ++index)
    {
      TileZone tileZone1 = zones[index];
      foreach (TileZone tileZone2 in tileZone1.neighbours_all)
      {
        if (tileZone2.city != this)
        {
          this.border_zones.Add(tileZone1);
          this.neighbour_zones.Add(tileZone2);
        }
      }
    }
  }

  internal void setCulture(Culture pCulture)
  {
    if (this.culture == pCulture)
      return;
    this.culture = pCulture;
    World.world.cultures.setDirtyCities();
  }

  public Culture getCulture() => this.culture;

  public Language getLanguage() => this.language;

  public Religion getReligion() => this.religion;

  public void checkAbandon()
  {
    if (!this._dirty_abandoned_zones)
      return;
    this._dirty_abandoned_zones = false;
    World.world.city_zone_helper.city_abandon.check(this);
  }

  public void update(float pElapsed)
  {
    if ((double) this.timer_build > 0.0)
      this.timer_build -= pElapsed;
    this.updateTotalFood();
    if ((double) this.data.timer_supply > 0.0)
      this.data.timer_supply -= pElapsed;
    if ((double) this.data.timer_trade > 0.0)
      this.data.timer_trade -= pElapsed;
    if ((double) this._timer_warrior > 0.0)
      this._timer_warrior -= pElapsed;
    if (this.isDirtyUnits())
      return;
    if (!this.kingdom.wild && !this.hasUnits())
    {
      this.turnCityToNeutral();
    }
    else
    {
      if (this._dirty_city_status)
        this.updateCityStatus();
      if (this._dirty_citizens)
        this.updateCitizens();
      if (World.world.isPaused())
        return;
      if ((double) this.timer_build_boat > 0.0)
        this.timer_build_boat -= pElapsed;
      if (this.ai != null)
      {
        if ((double) this.timer_action > 0.0)
          this.timer_action -= pElapsed;
        else
          this.ai.update();
        this.ai.updateSingleTasks(pElapsed);
      }
      this.updateCapture(pElapsed);
    }
  }

  private void turnCityToNeutral()
  {
    this.makeBoatsAbandonCity();
    this.setKingdom(WildKingdomsManager.neutral);
    this.forceBuildingsToKingdom(this.buildings, WildKingdomsManager.neutral);
  }

  private void makeBoatsAbandonCity()
  {
    if (this.countBoats() == 0)
      return;
    foreach (Actor boat in this._boats)
    {
      if (!boat.isRekt())
        boat.setCity((City) null);
    }
  }

  private void updateTotalFood() => this._current_total_food = this.countFoodTotal();

  private void updateCapture(float pElapsed)
  {
    if (this.last_visual_capture_ticks == 0 && !this.isGettingCaptured())
      return;
    if ((int) this._capture_ticks != this.last_visual_capture_ticks)
    {
      if ((int) this._capture_ticks > this.last_visual_capture_ticks)
        ++this.last_visual_capture_ticks;
      else
        --this.last_visual_capture_ticks;
    }
    this.last_visual_capture_ticks = Mathf.Clamp(this.last_visual_capture_ticks, 0, 100);
    if ((double) this._timer_capture > 0.0)
    {
      this._timer_capture -= pElapsed;
    }
    else
    {
      this._timer_capture = 0.1f;
      int num = this.countBuildingsType("type_watch_tower");
      if (num > 0)
        this.addCapturePoints(this.kingdom, 10 * num);
      Kingdom kingdom = (Kingdom) null;
      foreach (Kingdom key in this._capturing_units.Keys)
      {
        if (kingdom == null)
          kingdom = key;
        else if (this._capturing_units[key] > this._capturing_units[kingdom])
          kingdom = key;
      }
      if (kingdom == null)
      {
        this._capture_ticks -= 0.5f;
        if ((double) this._capture_ticks > 0.0)
          return;
        this.clearCapture();
      }
      else
      {
        bool flag1 = false;
        if (this._capturing_units.ContainsKey(this.kingdom) && this._capturing_units[this.kingdom] > 0 && this.countWarriors() > 0)
          flag1 = true;
        if (this.being_captured_by != null && !this.being_captured_by.isAlive())
          this.being_captured_by = (Kingdom) null;
        bool flag2 = false;
        if (this.kingdom == kingdom)
          flag2 = true;
        if (flag1 && this._capturing_units.Count == 1)
          flag2 = true;
        if (flag2)
        {
          --this._capture_ticks;
          if ((double) this._capture_ticks > 0.0)
            return;
          this.clearCapture();
        }
        else
        {
          if (!kingdom.isEnemy(this.kingdom) || flag1 && (double) this._capture_ticks >= 5.0)
            return;
          if (this.being_captured_by == null || this.being_captured_by == kingdom)
          {
            this._capture_ticks += (float) (1.0 + 1.0 * (double) pElapsed);
            this.being_captured_by = kingdom;
            if ((double) this._capture_ticks < 100.0)
              return;
            this.finishCapture(kingdom);
          }
          else if (kingdom.isEnemy(this.being_captured_by))
          {
            this._capture_ticks -= 0.5f;
            if ((double) this._capture_ticks > 0.0)
              return;
            this.clearCapture();
          }
          else
          {
            this._capture_ticks += (float) (1.0 + 1.0 * (double) pElapsed);
            if ((double) this._capture_ticks < 100.0)
              return;
            this.finishCapture(this.being_captured_by);
          }
        }
      }
    }
  }

  public bool isGettingCaptured()
  {
    return this._capturing_units.Count != 0 && (this._capturing_units.Count != 1 || !this._capturing_units.ContainsKey(this.kingdom));
  }

  public bool isGettingCapturedBy(Kingdom pKingdom)
  {
    int num;
    return this._capturing_units.TryGetValue(pKingdom, out num) && num > 0;
  }

  public Kingdom getCapturingKingdom() => this.being_captured_by;

  private void clearCapture()
  {
    this._capture_ticks = 0.0f;
    this.being_captured_by = (Kingdom) null;
  }

  public float getCaptureTicks() => this._capture_ticks;

  private void prepareProfessionDicts()
  {
    if (this._professions_dict.Count != 0)
      return;
    for (int index = 0; index < ProfessionLibrary.list_enum_profession_ids.Length; ++index)
      this._professions_dict.Add(ProfessionLibrary.list_enum_profession_ids[index], new List<Actor>());
  }

  private void updateCitizens()
  {
    this._dirty_citizens = false;
    this.prepareProfessionDicts();
    foreach (List<Actor> actorList in this._professions_dict.Values)
      actorList.Clear();
    List<Actor> units = this.units;
    for (int index = 0; index < units.Count; ++index)
    {
      Actor actor = units[index];
      if (actor != null && actor.isAlive())
        this._professions_dict[actor.getProfession()].Add(actor);
    }
  }

  public bool canGrowZones()
  {
    return DebugConfig.isOn(DebugOption.SystemZoneGrowth) && !this._dirty_abandoned_zones && this.getPopulationPeople() != 0;
  }

  internal int countProfession(UnitProfession pType)
  {
    List<Actor> actorList;
    return this._professions_dict.TryGetValue(pType, out actorList) ? actorList.Count : 0;
  }

  public void destroyCity()
  {
    this.removeLeader();
    this.disbandArmy();
    foreach (TileZone zone in this.zones)
      zone.setCity((City) null);
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.city == this)
        unit.setCity((City) null);
    }
    this.data.equipment.clearItems();
    this.units.Clear();
    this._boats.Clear();
    this.zones.Clear();
    if (!this.hasKingdom())
      return;
    this.removeFromCurrentKingdom();
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "city");
    City._connected_checked.Clear();
    City._connected_next_wave.Clear();
    City._connected_current_wave.Clear();
    this.stockpiles.Clear();
    this.storages.Clear();
    this._cached_book_ids.Clear();
    this._cached_buildings_with_book_slots.Clear();
    this.units.Clear();
    this._boats.Clear();
    this.buildings.Clear();
    this.buildings_dict_id.Clear();
    this.buildings_dict_type.Clear();
    this.zones.Clear();
    this.road_tiles_to_build.Clear();
    this.calculated_place_for_farms.Clear();
    this.calculated_farm_fields.Clear();
    this.calculated_crops.Clear();
    this.calculated_grown_wheat.Clear();
    this._professions_dict.Clear();
    this.neighbour_zones.Clear();
    this.border_zones.Clear();
    this.neighbours_cities.Clear();
    this.neighbours_cities_kingdom.Clear();
    this.neighbours_kingdoms.Clear();
    this.tiles_to_remove.Clear();
    this.danger_zones.Clear();
    this._capturing_units.Clear();
    this._city_tile = (WorldTile) null;
    this.target_attack_zone = (TileZone) null;
    this.target_attack_city = (City) null;
    this.army = (Army) null;
    this.tasks.clear();
    this.jobs.clear();
    this.status.clear();
    this.under_construction_building = (Building) null;
    this.culture = (Culture) null;
    this.language = (Language) null;
    this.religion = (Religion) null;
    this.kingdom = (Kingdom) null;
    this.leader = (Actor) null;
    this.being_captured_by = (Kingdom) null;
    this._debug_last_possible_build_orders = (string) null;
    this._debug_last_possible_build_orders_no_resources = (string) null;
    this._debug_last_build_order_try = (string) null;
    this.timestamp_shrink = 0.0;
    this.ai.reset();
    base.Dispose();
  }

  public bool hasAttackZoneOrder() => this.target_attack_zone != null;

  internal void spendResourcesForBuildingAsset(ConstructionCost pCost)
  {
    this.takeResource("wood", pCost.wood);
    this.takeResource("gold", pCost.gold);
    this.takeResource("stone", pCost.stone);
    this.takeResource("common_metals", pCost.common_metals);
  }

  internal bool hasEnoughResourcesFor(ConstructionCost pCost)
  {
    return DebugConfig.isOn(DebugOption.CityInfiniteResources) || this.amount_wood >= pCost.wood && this.amount_common_metals >= pCost.common_metals && this.amount_stone >= pCost.stone && this.amount_gold >= pCost.gold;
  }

  public int amount_wood => this.getResourcesAmount("wood");

  public int amount_gold => this.getResourcesAmount("gold");

  public int amount_stone => this.getResourcesAmount("stone");

  public int amount_common_metals => this.getResourcesAmount("common_metals");

  internal Building getBuildingToBuild()
  {
    if (this.under_construction_building != null && (!this.under_construction_building.isAlive() || !this.under_construction_building.isUnderConstruction()))
      this.under_construction_building = (Building) null;
    return this.under_construction_building;
  }

  internal bool hasBuildingToBuild()
  {
    if (this.under_construction_building == null)
      return false;
    if (this.under_construction_building.isAlive() && this.under_construction_building.isUnderConstruction())
      return true;
    this.under_construction_building = (Building) null;
    return false;
  }

  internal void setBuildingDictType(Building pBuilding)
  {
    List<Building> buildingList = this.getBuildingListOfType(pBuilding.asset.type);
    if (buildingList == null)
    {
      buildingList = new List<Building>();
      this.buildings_dict_type.Add(pBuilding.asset.type, buildingList);
    }
    buildingList.Add(pBuilding);
  }

  internal List<Building> getBuildingListOfID(string pBuildingID)
  {
    List<Building> buildingListOfId;
    this.buildings_dict_id.TryGetValue(pBuildingID, out buildingListOfId);
    return buildingListOfId;
  }

  public int countZones() => this.zones.Count;

  public int countBuildings() => this.buildings.Count;

  public int countBuildingsOfID(string pBuildingID)
  {
    List<Building> buildingListOfId = this.getBuildingListOfID(pBuildingID);
    return buildingListOfId == null ? 0 : buildingListOfId.Count;
  }

  internal void setBuildingDictID(Building pBuilding)
  {
    List<Building> buildingList;
    if (!this.buildings_dict_id.TryGetValue(pBuilding.asset.id, out buildingList))
      this.buildings_dict_id.Add(pBuilding.asset.id, buildingList = new List<Building>());
    buildingList.Add(pBuilding);
  }

  public int countBuildingsType(string pBuildingTypeID, bool pCountOnlyFinished = true)
  {
    List<Building> buildingListOfType = this.getBuildingListOfType(pBuildingTypeID);
    if (buildingListOfType == null)
      return 0;
    if (!pCountOnlyFinished)
      return buildingListOfType.Count;
    int num = 0;
    foreach (Building building in buildingListOfType)
    {
      if (!building.isUnderConstruction())
        ++num;
    }
    return num;
  }

  internal bool hasBuildingType(
    string pBuildingTypeID,
    bool pCountOnlyFinished = true,
    TileIsland pLimitIsland = null)
  {
    List<Building> buildingListOfType = this.getBuildingListOfType(pBuildingTypeID);
    if (buildingListOfType == null || buildingListOfType.Count == 0)
      return false;
    bool flag = pLimitIsland != null;
    foreach (Building building in buildingListOfType)
    {
      if ((!pCountOnlyFinished || !building.isUnderConstruction() && building.isUsable()) && (!flag || building.current_island == pLimitIsland))
        return true;
    }
    return false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal List<Building> getBuildingListOfType(string pType)
  {
    List<Building> buildingListOfType;
    this.buildings_dict_type.TryGetValue(pType, out buildingListOfType);
    return buildingListOfType;
  }

  internal Building getBuildingOfType(
    string pBuildingTypeID,
    bool pCountOnlyFinished = true,
    bool pRandom = false,
    bool pOnlyFreeTile = false,
    TileIsland pLimitIsland = null)
  {
    List<Building> buildingListOfType = this.getBuildingListOfType(pBuildingTypeID);
    if (buildingListOfType == null)
      return (Building) null;
    if (buildingListOfType.Count == 0)
      return (Building) null;
    bool flag = pLimitIsland != null;
    foreach (Building buildingOfType in pRandom ? buildingListOfType.LoopRandom<Building>() : (IEnumerable<Building>) buildingListOfType)
    {
      if ((!pCountOnlyFinished || !buildingOfType.isUnderConstruction() && buildingOfType.isUsable()) && (!pOnlyFreeTile || !buildingOfType.current_tile.isTargeted()) && (!flag || buildingOfType.current_island == pLimitIsland))
        return buildingOfType;
    }
    return (Building) null;
  }

  public void addRoads(List<WorldTile> pTiles)
  {
    for (int index = 0; index < pTiles.Count; ++index)
    {
      WorldTile pTile = pTiles[index];
      if (!pTile.Type.road && !this.road_tiles_to_build.Contains(pTile))
        this.road_tiles_to_build.Add(pTile);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private bool isArmyFull() => this.status.warriors_current >= this.status.warrior_slots;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private bool isArmyOverLimit() => this.status.warriors_current > this.status.warrior_slots;

  private bool tryToMakeWarrior(Actor pActor)
  {
    if (!this.checkCanMakeWarrior(pActor))
      return false;
    this.makeWarrior(pActor);
    this._timer_warrior = 15f;
    if (this.hasLeader())
    {
      this._timer_warrior -= this.leader.stats["warfare"] / 2f;
      if ((double) this._timer_warrior < 1.0)
        this._timer_warrior = 1f;
    }
    if (this.hasBuildingType("type_barracks"))
      this._timer_warrior /= 2f;
    return true;
  }

  public bool checkCanMakeWarrior(Actor pActor)
  {
    return !this.isArmyFull() && !pActor.isBaby() && (!this.hasCulture() || (!pActor.isSexFemale() || !this.culture.hasTrait("conscription_male_only")) && (!pActor.isSexMale() || !this.culture.hasTrait("conscription_female_only")));
  }

  public void makeWarrior(Actor pActor)
  {
    pActor.setProfession(UnitProfession.Warrior);
    if (pActor.equipment.weapon.isEmpty())
      City.giveItem(pActor, this.getEquipmentList(EquipmentType.Weapon), this);
    ++this.status.warriors_current;
  }

  public bool checkIfWarriorStillOk(Actor pActor)
  {
    bool flag = true;
    if (this.isArmyOverLimit())
      flag = false;
    else if (!this.hasEnoughFoodForArmy())
      flag = false;
    if (!flag)
    {
      pActor.stopBeingWarrior();
      this._timer_warrior = 30f;
    }
    return flag;
  }

  public void setCitizenJob(Actor pActor)
  {
    if (!this.isGettingCaptured() && (double) this._timer_warrior <= 0.0 && pActor.isProfession(UnitProfession.Unit) && this.getResourcesAmount("gold") > 10 && this.hasEnoughFoodForArmy() && this.tryToMakeWarrior(pActor) || this.checkCitizenJobList(AssetManager.citizen_job_library.list_priority_high, pActor) || !this.hasAnyFood() && this.checkCitizenJobList(AssetManager.citizen_job_library.list_priority_high_food, pActor))
      return;
    List<CitizenJobAsset> listPriorityNormal = AssetManager.citizen_job_library.list_priority_normal;
    for (int index = 0; index < listPriorityNormal.Count; ++index)
    {
      ++this._last_checked_job_id;
      if (this._last_checked_job_id > listPriorityNormal.Count - 1)
        this._last_checked_job_id = 0;
      CitizenJobAsset pJobAsset = listPriorityNormal[this._last_checked_job_id];
      if ((pJobAsset.ok_for_king || !pActor.isKing()) && (pJobAsset.ok_for_leader || !pActor.isCityLeader()) && this.checkCitizenJob(pJobAsset, this, pActor))
        break;
    }
  }

  private bool checkCitizenJobList(List<CitizenJobAsset> pList, Actor pActor)
  {
    for (int index = 0; index < pList.Count; ++index)
    {
      if (this.checkCitizenJob(pList[index], this, pActor))
        return true;
    }
    return false;
  }

  private bool checkCitizenJob(CitizenJobAsset pJobAsset, City pCity, Actor pActor)
  {
    if (pJobAsset.only_leaders && !pActor.isKing() && !pActor.isCityLeader() || pJobAsset.should_be_assigned != null && !pJobAsset.should_be_assigned(pActor) || !this.jobs.hasJob(pJobAsset))
      return false;
    this.jobs.takeJob(pJobAsset);
    pActor.setCitizenJob(pJobAsset);
    return true;
  }

  public bool hasSuitableFood(Subspecies pSubspecies)
  {
    HashSet<string> allowedFoodByDiet = pSubspecies.getAllowedFoodByDiet();
    foreach (Building storage in this.storages)
    {
      if (storage.isUsable())
      {
        foreach (string pResourceID in allowedFoodByDiet)
        {
          if (storage.getResourcesAmount(pResourceID) != 0)
            return true;
        }
      }
    }
    return false;
  }

  internal ResourceAsset getFoodItem(Subspecies pSubspecies, string pFavoriteFood = null)
  {
    return !string.IsNullOrEmpty(pFavoriteFood) && this.getResourcesAmount(pFavoriteFood) > 0 ? AssetManager.resources.get(pFavoriteFood) : this.getRandomSuitableFood(pSubspecies);
  }

  internal void eatFoodItem(string pItem)
  {
    if (pItem == null)
      return;
    this.takeResource(pItem, 1);
    ++this.data.total_food_consumed;
  }

  internal void removeZone(TileZone pZone)
  {
    this.setAbandonedZonesDirty();
    if (this.zones.Remove(pZone))
    {
      pZone.setCity((City) null);
      World.world.city_zone_helper.city_place_finder.setDirty();
    }
    this.updateCityCenter();
    this.setStatusDirty();
  }

  internal void addZone(TileZone pZone)
  {
    if (this.zones.Contains(pZone))
      return;
    if (pZone.city != null)
      pZone.city.removeZone(pZone);
    this.zones.Add(pZone);
    pZone.setCity(this);
    this.updateCityCenter();
    if (World.world.city_zone_helper.city_place_finder.hasPossibleZones())
      World.world.city_zone_helper.city_place_finder.setDirty();
    this.setStatusDirty();
  }

  public int getLoyalty(bool pForceRecalc = false)
  {
    if (this.kingdom.isNeutral())
      this._loyalty_cached = 0;
    else if ((double) World.world.getWorldTimeElapsedSince(this._loyalty_last_time) > 3.0 | pForceRecalc)
    {
      this._loyalty_cached = LoyaltyCalculator.calculate(this);
      this._loyalty_last_time = World.world.getCurWorldTime();
    }
    return this._loyalty_cached;
  }

  public int getCachedLoyalty() => this._loyalty_cached;

  public bool isCapitalCity() => this.kingdom != null && this == this.kingdom.capital;

  internal void updateAge()
  {
    if (!this.hasLeader() || !this.leader.hasClan())
      return;
    this.leader.addRenown(1);
  }

  private void updateCityCenter()
  {
    if (!this.hasZones())
    {
      this.city_center = Globals.POINT_IN_VOID_2;
    }
    else
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = float.MaxValue;
      TileZone tileZone = (TileZone) null;
      for (int index = 0; index < this.zones.Count; ++index)
      {
        TileZone zone = this.zones[index];
        num1 += zone.centerTile.posV3.x;
        num2 += zone.centerTile.posV3.y;
      }
      this.city_center.x = num1 / (float) this.zones.Count;
      this.city_center.y = num2 / (float) this.zones.Count;
      for (int index = 0; index < this.zones.Count; ++index)
      {
        TileZone zone = this.zones[index];
        float num4 = Toolbox.SquaredDist((float) zone.centerTile.x, (float) zone.centerTile.y, this.city_center.x, this.city_center.y);
        if ((double) num4 < (double) num3)
        {
          tileZone = zone;
          num3 = num4;
        }
      }
      this.city_center.x = tileZone.centerTile.posV3.x;
      this.city_center.y = tileZone.centerTile.posV3.y + 2f;
      this.last_city_center = this.city_center;
    }
  }

  internal void removeFromCurrentKingdom() => this.kingdom.checkClearCapital(this);

  internal void switchedKingdom()
  {
    List<Building> buildings = this.buildings;
    for (int index = 0; index < buildings.Count; ++index)
    {
      Building building = buildings[index];
      if (!building.isRemoved())
        building.setKingdomCiv(this.kingdom);
    }
    World.world.zone_calculator.setDrawnZonesDirty();
  }

  internal void useInspire(Actor pActor)
  {
    Kingdom kingdom = this.kingdom;
    this.makeOwnKingdom(pActor, true);
    World.world.diplomacy.startWar(kingdom, this.kingdom, WarTypeLibrary.inspire, false);
  }

  internal void clearCurrentCaptureAmounts() => this._capturing_units.Clear();

  internal void clearDangerZones() => this.danger_zones.Clear();

  public bool isInDanger() => this.danger_zones.Count > 0;

  internal void updateConquest(Actor pActor)
  {
    if (!pActor.isKingdomCiv() || pActor.kingdom != this.kingdom && !pActor.kingdom.isEnemy(this.kingdom))
      return;
    this.addCapturePoints((BaseSimObject) pActor, 1);
  }

  public void addCapturePoints(BaseSimObject pObject, int pValue)
  {
    this.addCapturePoints(pObject.kingdom, pValue);
  }

  public void addCapturePoints(Kingdom pKingdom, int pValue)
  {
    int num;
    this._capturing_units.TryGetValue(pKingdom, out num);
    this._capturing_units[pKingdom] = num + pValue;
  }

  public void debugCaptureUnits(DebugTool pTool)
  {
    pTool.setText("capture units:", (object) this._capturing_units.Count);
    pTool.setText("isGettingCaptured()", (object) this.isGettingCaptured());
    foreach (Kingdom key in this._capturing_units.Keys)
      pTool.setText("-" + key.name, (object) this._capturing_units[key]);
  }

  internal void finishCapture(Kingdom pNewKingdom)
  {
    if (this.kingdom.hasKing() && this.kingdom.king.city == this)
      this.kingdom.kingFledCity();
    if (World.world.cities.isLocked())
      return;
    this.clearCapture();
    this.recalculateNeighbourCities();
    pNewKingdom.increaseHappinessFromNewCityCapture();
    this.kingdom.decreaseHappinessFromLostCityCapture(this);
    using (ListPool<War> pWars = new ListPool<War>(pNewKingdom.getWars()))
    {
      Kingdom joinAfterCapture = this.findKingdomToJoinAfterCapture(pNewKingdom, pWars);
      if (!this.checkRebelWar(joinAfterCapture, pWars))
        joinAfterCapture.data.timestamp_new_conquest = World.world.getCurWorldTime();
      this.removeSoldiers();
      this.joinAnotherKingdom(joinAfterCapture, true);
    }
  }

  private Kingdom findKingdomToJoinAfterCapture(Kingdom pKingdom, ListPool<War> pWars)
  {
    Kingdom joinAfterCapture = (Kingdom) null;
    for (int index = 0; index < pWars.Count; ++index)
    {
      War pWar = pWars[index];
      if (!pWar.isTotalWar() && pWar.hasKingdom(this.kingdom) && pWar.isInWarWith(pKingdom, this.kingdom))
      {
        if (!pWar.isMainAttacker(pKingdom) && !pWar.isMainDefender(pKingdom))
        {
          if (pWar.isAttacker(this.kingdom))
          {
            Kingdom mainDefender = pWar.main_defender;
            if (!mainDefender.isRekt())
            {
              joinAfterCapture = !this.neighbours_kingdoms.Contains(mainDefender) ? (!this.neighbours_kingdoms.Contains(pKingdom) ? mainDefender : pKingdom) : mainDefender;
              break;
            }
          }
          if (pWar.isDefender(this.kingdom))
          {
            Kingdom mainAttacker = pWar.main_attacker;
            if (!mainAttacker.isRekt())
            {
              joinAfterCapture = !this.neighbours_kingdoms.Contains(mainAttacker) ? (!this.neighbours_kingdoms.Contains(pKingdom) ? mainAttacker : pKingdom) : mainAttacker;
              break;
            }
          }
        }
        else
          break;
      }
    }
    if (joinAfterCapture == null)
      joinAfterCapture = pKingdom;
    else if (joinAfterCapture.getSpecies() != this.kingdom.getSpecies())
      joinAfterCapture = pKingdom;
    return joinAfterCapture;
  }

  private bool checkRebelWar(Kingdom pKingdomToJoin, ListPool<War> pWars)
  {
    // ISSUE: unable to decompile the method.
  }

  private void removeSoldiers()
  {
    foreach (Actor actor in this._professions_dict[UnitProfession.Warrior])
      actor.setProfession(UnitProfession.Unit);
    this.disbandArmy();
  }

  public void disbandArmy()
  {
    this.checkArmyExistence();
    if (!this.hasArmy())
      return;
    this.army.disband();
    this.checkArmyExistence();
  }

  public void checkArmyExistence()
  {
    if (!this.hasArmy() || this.army.isAlive() && this.army.countUnits() > 0)
      return;
    this.setArmy((Army) null);
  }

  public bool hasArmy() => this.army != null;

  public Army getArmy() => this.army;

  public void setArmy(Army pArmy)
  {
    if (this.army != null && this.army != pArmy)
      this.army.clearCity();
    this.army = pArmy;
  }

  public Actor getRandomWarrior()
  {
    return this._professions_dict[UnitProfession.Warrior].GetRandom<Actor>();
  }

  internal Kingdom makeOwnKingdom(Actor pActor, bool pRebellion = false, bool pFellApart = false)
  {
    string pHappinessEvent = (string) null;
    if (pRebellion)
    {
      ++World.world.game_stats.data.citiesRebelled;
      ++World.world.map_stats.citiesRebelled;
      pHappinessEvent = "just_rebelled";
    }
    if (pFellApart)
      pHappinessEvent = "kingdom_fell_apart";
    Kingdom kingdom = this.kingdom;
    this.removeFromCurrentKingdom();
    this.removeLeader();
    Kingdom pKingdom = World.world.kingdoms.makeNewCivKingdom(pActor);
    this.setKingdom(pKingdom);
    this.newForceKingdomEvent(this.units, this._boats, pKingdom, pHappinessEvent);
    this.switchedKingdom();
    pKingdom.copyMetasFromOtherKingdom(kingdom);
    pKingdom.setCityMetas(this);
    return pKingdom;
  }

  public override int getPopulationPeople() => this.countUnits();

  public int getPopulationMaximum()
  {
    return WorldLawLibrary.world_law_civ_limit_population_100.isEnabled() && this.status.housing_total >= 100 ? 100 : this.status.housing_total;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int getUnitsTotal() => this.countUnits() + this.countBoats();

  public int countPopulationChildren()
  {
    int num = 0;
    foreach (Actor unit in this.units)
    {
      if (unit.isAlive() && unit.isBaby())
        ++num;
    }
    return num;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int countBoats() => this._boats.Count;

  public void joinAnotherKingdom(Kingdom pNewSetKingdom, bool pCaptured = false, bool pRebellion = false)
  {
    string pHappinessEvent = (string) null;
    if (pCaptured)
    {
      ++World.world.game_stats.data.citiesConquered;
      ++World.world.map_stats.citiesConquered;
      pHappinessEvent = "was_conquered";
    }
    if (pRebellion)
    {
      ++World.world.game_stats.data.citiesRebelled;
      ++World.world.map_stats.citiesRebelled;
      pHappinessEvent = "just_rebelled";
    }
    Kingdom kingdom = this.kingdom;
    this.removeFromCurrentKingdom();
    this.setKingdom(pNewSetKingdom);
    this.newForceKingdomEvent(this.units, this._boats, pNewSetKingdom, pHappinessEvent);
    this.switchedKingdom();
    pNewSetKingdom.capturedFrom(kingdom);
  }

  public int countWeapons() => this.getEquipmentList(EquipmentType.Weapon).Count;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int countFoodTotal() => this.countFood();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasEnoughFoodForArmy() => true;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int getTotalFood() => this._current_total_food;

  public bool hasAnyFood() => this._current_total_food > 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int countWarriors() => this.countProfession(UnitProfession.Warrior);

  public bool hasAnyWarriors() => this.countWarriors() > 0;

  public bool isHappy() => this.getCachedLoyalty() >= 0;

  public float getArmyMaxMultiplier()
  {
    return 0.0f + this.getActorAsset().civ_base_army_multiplier + this.getArmyMaxLeaderMultiplier();
  }

  public float getArmyMaxLeaderMultiplier()
  {
    float leaderMultiplier = 0.0f;
    if (this.hasLeader())
      leaderMultiplier = leaderMultiplier + this.leader.stats["army"] + (float) ((double) this.leader.stats["warfare"] * 2.0 / 100.0);
    return leaderMultiplier;
  }

  public int getMaxWarriors() => this.status.warrior_slots;

  public void removeLeader()
  {
    this.leader = (Actor) null;
    this.data.leaderID = -1L;
    this.rulerLeft();
  }

  public void setLeader(Actor pActor, bool pNew)
  {
    if (pActor == null || this.kingdom.king == pActor)
      return;
    this.leader = pActor;
    this.leader.setProfession(UnitProfession.Leader);
    this.data.leaderID = this.data.last_leader_id = pActor.data.id;
    if (!pNew)
      return;
    ++this.data.total_leaders;
    this.leader.changeHappiness("become_leader");
    this.addRuler(pActor);
  }

  public void updateRulers()
  {
    if (this.data.past_rulers == null || this.data.past_rulers.Count == 0)
      return;
    foreach (LeaderEntry pastRuler in this.data.past_rulers)
    {
      Actor pObject = World.world.units.get(pastRuler.id);
      if (!pObject.isRekt())
        pastRuler.name = pObject.name;
    }
  }

  public void addRuler(Actor pActor)
  {
    CityData data = this.data;
    if (data.past_rulers == null)
      data.past_rulers = new List<LeaderEntry>();
    this.rulerLeft();
    List<LeaderEntry> pastRulers = this.data.past_rulers;
    LeaderEntry leaderEntry = new LeaderEntry();
    leaderEntry.id = pActor.getID();
    leaderEntry.name = pActor.name;
    Kingdom kingdom = pActor.kingdom;
    leaderEntry.color_id = kingdom != null ? kingdom.data.color_id : -1;
    leaderEntry.timestamp_ago = World.world.getCurWorldTime();
    pastRulers.Add(leaderEntry);
    if (this.data.past_rulers.Count <= 30)
      return;
    this.data.past_rulers.Shift<LeaderEntry>();
  }

  public void rulerLeft()
  {
    if (this.data.past_rulers == null || this.data.past_rulers.Count == 0)
      return;
    LeaderEntry leaderEntry = this.data.past_rulers.Last<LeaderEntry>();
    if (leaderEntry.timestamp_end >= leaderEntry.timestamp_ago)
      return;
    leaderEntry.timestamp_end = World.world.getCurWorldTime();
    this.updateRulers();
  }

  public static bool nearbyBorders(City pA, City pB)
  {
    City city1;
    City city2;
    if (pA.zones.Count > pB.zones.Count)
    {
      city1 = pB;
      city2 = pA;
    }
    else
    {
      city1 = pA;
      city2 = pB;
    }
    for (int index = 0; index < city1.zones.Count; ++index)
    {
      foreach (TileZone tileZone in city1.zones[index].neighbours_all)
      {
        if (tileZone.city == city2)
          return true;
      }
    }
    return false;
  }

  public static bool giveItem(Actor pActor, List<long> pItems, City pCity)
  {
    if (pItems.Count == 0 || !pActor.understandsHowToUseItems())
      return false;
    long random = pItems.GetRandom<long>();
    Item pItem1 = World.world.items.get(random);
    EquipmentAsset asset = pItem1.getAsset();
    ActorEquipmentSlot slot = pActor.equipment.getSlot(asset.equipment_type);
    if (!slot.isEmpty())
    {
      int num = slot.getItem().getValue();
      if (pItem1.getValue() <= num)
        return false;
    }
    Item pItem2 = (Item) null;
    if (!slot.isEmpty())
    {
      pItem2 = slot.getItem();
      slot.takeAwayItem();
    }
    pItems.Remove(random);
    slot.setItem(pItem1, pActor);
    pActor.setStatsDirty();
    if (pItem2 != null)
      pCity.data.equipment.addItem(pCity, pItem2, pItems);
    ++pCity._storage_version;
    return true;
  }

  public int getLimitOfBuildingsType(BuildOrder pElement)
  {
    int limitType = pElement.limit_type;
    if (this.hasCulture())
    {
      switch (pElement.getBuildingAsset(this).type)
      {
        case "type_statue":
          if (this.culture.hasTrait("statue_lovers"))
          {
            limitType += CultureTraitLibrary.getValue("statue_lovers");
            break;
          }
          break;
        case "type_watch_tower":
          if (this.culture.hasTrait("tower_lovers"))
            limitType += CultureTraitLibrary.getValue("tower_lovers");
          if (this.hasLeader())
          {
            limitType += (int) this.leader.stats["bonus_towers"];
            break;
          }
          break;
      }
    }
    return limitType;
  }

  public Alliance getAlliance() => this.kingdom.getAlliance();

  public Clan getRoyalClan()
  {
    Clan royalClan = (Clan) null;
    if (royalClan == null && this.hasLeader())
      royalClan = this.leader.clan;
    if (royalClan == null && this.kingdom.hasKing())
      royalClan = this.kingdom.king.clan;
    return royalClan;
  }

  public bool isOkToSendArmy()
  {
    if (!this.hasArmy())
      return false;
    float maxWarriors = (float) this.getMaxWarriors();
    return (double) this.army.countUnits() / (double) maxWarriors >= 0.699999988079071;
  }

  public void tryToPutItem(Item pItem)
  {
    List<long> equipmentList = this.data.equipment.getEquipmentList(pItem.getAsset().equipment_type);
    if (equipmentList.Count >= this.status.maximum_items)
    {
      this.tryToPutItemInStorage(pItem);
    }
    else
    {
      this.data.equipment.addItem(this, pItem, equipmentList);
      ++this._storage_version;
    }
  }

  public void tryToPutItems(IEnumerable<Item> pItems)
  {
    foreach (Item pItem in pItems)
      this.tryToPutItem(pItem);
  }

  private void tryToPutItemInStorage(Item pNewItem)
  {
    float num1 = (float) pNewItem.getValue();
    List<long> equipmentList = this.data.equipment.getEquipmentList(pNewItem.getAsset().equipment_type);
    for (int index = 0; index < equipmentList.Count; ++index)
    {
      Item obj = World.world.items.get(equipmentList[index]);
      float num2 = (float) obj.getValue();
      if ((double) num1 > (double) num2)
      {
        obj.clearCity();
        equipmentList[index] = pNewItem.id;
        pNewItem.setInCityStorage(this);
        ++this._storage_version;
        break;
      }
    }
  }

  public int getZoneRange(bool pAllowCheat = true)
  {
    return pAllowCheat && DebugConfig.isOn(DebugOption.CityUnlimitedZoneRange) ? 999 : 13;
  }

  public bool reachableFrom(City pCity)
  {
    WorldTile tile1 = this.getTile();
    if (tile1 == null)
      return false;
    WorldTile tile2 = pCity.getTile();
    return tile2 != null && tile1.reachableFrom(tile2);
  }

  public bool hasLeader()
  {
    if (this.leader == null)
      return false;
    if (this.leader.isAlive())
      return true;
    this.removeLeader();
    return false;
  }

  public override void convertSameSpeciesAroundUnit(Actor pActorMain, bool pOverride = false)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pActorMain.current_tile, 2))
    {
      if (!actor.hasCity() && !actor.isKingdomCiv() && actor.isSameSpecies(pActorMain) && actor.isSapient())
        actor.joinCity(this);
    }
  }

  public override void forceConvertSameSpeciesAroundUnit(Actor pActorMain)
  {
    this.convertSameSpeciesAroundUnit(pActorMain, true);
  }

  public void setUnitMetas(Actor pActor)
  {
    if (pActor.hasCulture())
      this.setCulture(pActor.culture);
    if (pActor.hasLanguage())
      this.setLanguage(pActor.language);
    if (!pActor.hasReligion())
      return;
    this.setReligion(pActor.religion);
  }

  public override void save()
  {
    base.save();
    if (this.hasCulture())
      this.data.id_culture = this.culture.id;
    if (this.hasReligion())
      this.data.id_religion = this.religion.id;
    if (this.hasLanguage())
      this.data.id_language = this.language.id;
    if (this.kingdom == null)
      this.data.kingdomID = -1L;
    else
      this.data.kingdomID = this.kingdom.id;
    this.data.zones.Clear();
    foreach (TileZone zone in this.zones)
      this.data.zones.Add(new ZoneData()
      {
        x = zone.x,
        y = zone.y
      });
  }

  public bool hasCulture()
  {
    if (this.culture != null && !this.culture.isAlive())
      this.setCulture((Culture) null);
    return this.culture != null;
  }

  public bool hasLanguage()
  {
    if (this.language != null && !this.language.isAlive())
      this.setLanguage((Language) null);
    return this.language != null;
  }

  internal void setLanguage(Language pLanguage)
  {
    if (this.language == pLanguage)
      return;
    this.language = pLanguage;
    World.world.languages.setDirtyCities();
  }

  internal void setReligion(Religion pReligion)
  {
    if (this.religion == pReligion)
      return;
    this.religion = pReligion;
    World.world.religions.setDirtyCities();
  }

  public Subspecies getMainSubspecies()
  {
    if (this.hasLeader())
      return this.leader.subspecies;
    return this.getPopulationPeople() == 0 ? (Subspecies) null : this.units[0].subspecies;
  }

  public bool hasReligion()
  {
    if (this.religion != null && !this.religion.isAlive())
      this.setReligion((Religion) null);
    return this.religion != null;
  }

  public bool hasStockpiles() => this.stockpiles.Count > 0;

  public bool hasStorages() => this.storages.Count > 0;

  public Building getRandomStockpile()
  {
    if (!this.hasStockpiles())
      return (Building) null;
    foreach (Building randomStockpile in this.stockpiles.LoopRandom<Building>())
    {
      if (randomStockpile.isUsable())
        return randomStockpile;
    }
    return (Building) null;
  }

  public void takeResource(string pResourceID, int pAmount)
  {
    if (!this.hasStorages())
      return;
    int num = pAmount;
    foreach (Building storage in this.storages)
    {
      if (storage.isUsable())
      {
        int pAmount1 = storage.getResourcesAmount(pResourceID) < num ? storage.getResourcesAmount(pResourceID) : num;
        storage.takeResource(pResourceID, pAmount1);
        num -= pAmount1;
        if (num == 0)
          break;
      }
    }
    ++this._storage_version;
  }

  public int getResourcesAmount(string pResourceID)
  {
    if (!this.hasStorages())
      return 0;
    int resourcesAmount = 0;
    foreach (Building storage in this.storages)
    {
      if (storage.isUsable())
        resourcesAmount += storage.getResourcesAmount(pResourceID);
    }
    return resourcesAmount;
  }

  public int addResourcesToRandomStockpile(string pResourceID, int pAmount = 1)
  {
    Building randomStockpile = this.getRandomStockpile();
    if (randomStockpile == null)
      return 0;
    ++this._storage_version;
    return randomStockpile.addResources(pResourceID, pAmount);
  }

  public bool hasSpaceForResourceInStockpile(ResourceAsset pResourceAsset)
  {
    if (!this.hasStockpiles())
      return false;
    foreach (Building stockpile in this.stockpiles)
    {
      if (stockpile.isUsable() && stockpile.hasSpaceForResource(pResourceAsset))
        return true;
    }
    return false;
  }

  public bool hasResourcesForNewItems()
  {
    if (!this.hasStorages())
      return false;
    foreach (Building storage in this.storages)
    {
      if (storage.isUsable() && storage.hasResourcesForNewItems())
        return true;
    }
    return false;
  }

  public ResourceAsset getRandomSuitableFood(Subspecies pSubspecies)
  {
    if (!this.hasStorages())
      return (ResourceAsset) null;
    foreach (Building storage in this.storages)
    {
      if (storage.isUsable())
      {
        ResourceAsset randomSuitableFood = storage.getRandomSuitableFood(pSubspecies);
        if (randomSuitableFood != null)
          return randomSuitableFood;
      }
    }
    return (ResourceAsset) null;
  }

  public int countFood()
  {
    if (!this.hasStorages())
      return 0;
    int num = 0;
    foreach (Building storage in this.storages)
    {
      if (storage.isUsable())
        num += storage.countFood();
    }
    return num;
  }

  public ListPool<CityStorageSlot> getTotalResourceSlots(ResType[] pResTypes)
  {
    foreach (CityStorageSlot cityStorageSlot in this._total_resource_slots.Values)
    {
      ResourceAsset asset = cityStorageSlot.asset;
      if (pResTypes.IndexOf<ResType>(asset.type) != -1)
        cityStorageSlot.amount = 0;
    }
    foreach (Building storage in this.storages)
    {
      if (storage.isUsable())
      {
        foreach (CityStorageSlot slot in storage.resources.getSlots())
        {
          CityStorageSlot cityStorageSlot;
          this._total_resource_slots.TryGetValue(slot.id, out cityStorageSlot);
          if (cityStorageSlot == null)
          {
            cityStorageSlot = new CityStorageSlot(slot.id);
            this._total_resource_slots[slot.id] = cityStorageSlot;
          }
          cityStorageSlot.amount += slot.amount;
        }
      }
    }
    ListPool<CityStorageSlot> totalResourceSlots = new ListPool<CityStorageSlot>(this._total_resource_slots.Count);
    foreach (CityStorageSlot cityStorageSlot in this._total_resource_slots.Values)
    {
      ResourceAsset asset = cityStorageSlot.asset;
      if (pResTypes.IndexOf<ResType>(asset.type) != -1 && cityStorageSlot.amount != 0)
        totalResourceSlots.Add(cityStorageSlot);
    }
    totalResourceSlots.Sort((Comparison<CityStorageSlot>) ((a, b) => a.asset.order.CompareTo(b.asset.order)));
    return totalResourceSlots;
  }

  public bool hasKingdom() => this.kingdom != null;

  public float getTimerForNewWarrior() => this._timer_warrior;

  public List<long> getEquipmentList(EquipmentType pType)
  {
    return this.data.equipment.getEquipmentList(pType);
  }

  public bool planAllowsToPlaceBuildingInZone(TileZone pZone, TileZone pCenterZone)
  {
    return this.status.housing_total < 10 && this.zones.Count < 20 || this.culture.planAllowsToPlaceBuildingInZone(pZone, pCenterZone);
  }

  public bool hasSpecialTownPlans() => this.hasCulture() && this.culture.hasSpecialTownPlans();

  public bool isNeutral() => this.kingdom.isNeutral();

  public bool isWelcomedToJoin(Actor pActor)
  {
    return pActor.kingdom == this.kingdom || pActor.isSameSubspecies(this.getMainSubspecies()) || this.hasCulture() && !this.culture.hasTrait("xenophobic") && !pActor.hasCultureTrait("xenophobic") && (this.culture.hasTrait("xenophiles") && (!pActor.hasCulture() || pActor.hasCultureTrait("xenophiles")) || this.isSameSpeciesAsActor(pActor));
  }

  public bool isSameSpeciesAsActor(Actor pActor) => pActor.isSameSpecies(this.getCurrentSpecies());

  public string getCurrentSpecies()
  {
    Subspecies mainSubspecies = this.getMainSubspecies();
    return mainSubspecies != null ? mainSubspecies.getActorAsset().id : this.getActorAsset().id;
  }

  public Sprite getCurrentSpeciesIcon()
  {
    Subspecies mainSubspecies = this.getMainSubspecies();
    return mainSubspecies != null ? mainSubspecies.getSpriteIcon() : this.getActorAsset().getSpriteIcon();
  }

  public bool hasTransportBoats()
  {
    foreach (Actor boat in this._boats)
    {
      if (boat.asset.is_boat_transport)
        return true;
    }
    return false;
  }

  public bool isCityUnderDangerFire() => this.tasks.fire > 0;

  public bool isPossibleToJoin(Actor pActor)
  {
    return this != pActor.city && (this.isNeutral() || this.isWelcomedToJoin(pActor) && (pActor.city == null || !pActor.isKing() && !pActor.isCityLeader() && pActor.city.getPopulationPeople() >= this.getPopulationPeople()));
  }

  public override string ToString()
  {
    if (this.data == null)
      return "[City is null]";
    using (StringBuilderPool stringBuilderPool1 = new StringBuilderPool())
    {
      stringBuilderPool1.Append($"[City:{this.id} ");
      if (!this.isAlive())
        stringBuilderPool1.Append("[DEAD] ");
      stringBuilderPool1.Append($"\"{this.name}\" ");
      StringBuilderPool stringBuilderPool2 = stringBuilderPool1;
      Kingdom kingdom = this.kingdom;
      string str = $"Kingdom:{(kingdom != null ? kingdom.id : -1L)} ";
      stringBuilderPool2.Append(str);
      if (this.hasArmy())
        stringBuilderPool1.Append($"Army:{this.army.id} ");
      stringBuilderPool1.Append($"Units:{this.units.Count} ");
      if (this.isDirtyUnits())
        stringBuilderPool1.Append("[Dirty] ");
      if (!this.leader.isRekt())
        stringBuilderPool1.Append($"Leader:{this.leader.id} ");
      if (this.kingdom?.king?.city == this)
        stringBuilderPool1.Append($"King:{this.kingdom.king.id} ");
      return stringBuilderPool1.ToString().Trim() + "]";
    }
  }
}
