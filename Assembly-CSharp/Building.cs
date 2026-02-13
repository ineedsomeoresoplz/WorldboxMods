// Decompiled with JetBrains decompiler
// Type: Building
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
public class Building : 
  BaseSimObject,
  IEquatable<Building>,
  IComparable<Building>,
  ILoadable<BuildingData>
{
  public BatchBuildings batch;
  internal bool positionDirty;
  internal bool sprite_dirty;
  internal bool tiles_dirty;
  private Sprite _last_colored_sprite;
  private ColorAsset _last_color_asset;
  internal Sprite last_main_sprite;
  internal BuildingData data;
  internal BuildingAsset asset;
  public bool flip_x;
  internal readonly List<WorldTile> tiles = new List<WorldTile>();
  public BuildingAnimationData animData;
  public int animData_index;
  private float _shake_timer;
  private float _shake_intensity_x;
  private float _shake_intensity_y;
  internal float lastAngle;
  private Vector2 _shake_offset;
  internal readonly List<TileZone> zones = new List<TileZone>();
  internal BuildingAnimationState animation_state;
  internal BuildingOwnershipState state_ownership;
  internal ListPool<BaseBuildingComponent> components_list;
  internal Docks component_docks;
  internal Wheat component_wheat;
  internal BuildingFruitGrowth component_fruit_growth;
  internal UnitSpawner component_unit_spawner;
  internal BuildingSpreadBiome component_biome_spreader;
  internal BuildingMonolith component_monolith;
  internal BuildingWaypoint component_waypoint;
  internal BuildingBiomeFoodProducer component_food_producer;
  internal Beehive component_beehive;
  internal readonly BuildingTweenScaleHelper scale_helper = new BuildingTweenScaleHelper();
  internal bool chopped;
  internal bool is_visible;
  internal bool check_spawn_animation;
  private float _timer_shake_resource;
  private float _auto_remove_timer;
  public HashSet<long> residents = new HashSet<long>();
  private Vector3 _last_scale = Vector3.zero;
  public Material material;

  protected override MetaType meta_type => MetaType.Building;

  internal WorldTile door_tile
  {
    get => !this.current_tile.has_tile_down ? this.current_tile : this.current_tile.tile_down;
  }

  internal bool isBurnable()
  {
    if (!this.hasHealth())
      return false;
    if (this.hasCity())
    {
      City city = this.getCity();
      if (city.hasReligion() && city.getReligion().hasMetaTag("building_immunity_fire"))
        return false;
    }
    return this.asset.burnable;
  }

  public float getExistenceTime() => World.world.getWorldTimeElapsedSince(this.data.created_time);

  public float getExistenceMonths() => this.getExistenceTime() / 5f;

  public void setAnimData(int pIndex)
  {
    if (pIndex >= this.asset.building_sprites.animation_data.Count || pIndex < 0)
      pIndex = 0;
    this.animData = this.asset.building_sprites.animation_data[pIndex];
    this.animData_index = pIndex;
  }

  internal void stopFire() => this.finishStatusEffect("burning");

  internal override void create()
  {
    base.create();
    this.setObjectType(MapObjectType.Building);
    this.startShake(0.3f);
  }

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this.flip_x = false;
    this.positionDirty = false;
    this.sprite_dirty = false;
    this.tiles_dirty = false;
    this._last_colored_sprite = (Sprite) null;
    this._last_color_asset = (ColorAsset) null;
    this._shake_timer = 0.0f;
    this.lastAngle = 0.0f;
    this.residents.Clear();
    this._shake_offset = Vector2.zero;
    this.animation_state = BuildingAnimationState.Normal;
    this.state_ownership = BuildingOwnershipState.None;
    this.chopped = false;
    this.is_visible = false;
    this.check_spawn_animation = false;
  }

  private T addComponent<T>() where T : BaseBuildingComponent, new()
  {
    T obj = World.world.buildings.component_pool.get<T>();
    if (this.components_list == null)
      this.components_list = new ListPool<BaseBuildingComponent>();
    this.components_list.Add((BaseBuildingComponent) obj);
    obj.create(this);
    this.batch.c_components.Add(this);
    return obj;
  }

  public bool hasBooks() => this.data.books != null && this.data.books.hasAny();

  public bool hasFreeBookSlot()
  {
    return this.asset.book_slots != 0 && this.asset.book_slots > this.data.books.totalBooks();
  }

  public void addBook(Book pBook) => this.data.books.addBook(pBook);

  public bool isState(BuildingState pState) => this.data.state == pState;

  internal void setBuilding(WorldTile pTile, BuildingAsset pAsset, BuildingData pData)
  {
    this.current_tile = pTile;
    this.current_tile.zone.addBuildingMain(this);
    if (pData == null)
    {
      this.setTemplate(pAsset);
      BuildingData data1 = this.data;
      Vector2Int pos1 = pTile.pos;
      int x = ((Vector2Int) ref pos1).x;
      data1.mainX = x;
      BuildingData data2 = this.data;
      Vector2Int pos2 = pTile.pos;
      int y = ((Vector2Int) ref pos2).y;
      data2.mainY = y;
      this.setState(BuildingState.Normal);
      this.updateStats();
      this.setMaxHealth();
      if (this.asset.has_resources_grown_to_collect)
        this.setHaveResourcesToCollect(this.asset.has_resources_grown_to_collect_on_spawn);
    }
    else
    {
      this.setData(pData);
      this.setTemplate(pAsset);
    }
    this.setStatsDirty();
    this.current_position = Vector2Int.op_Implicit(this.current_tile.pos);
    this.current_scale.x = this.asset.scale_base.x;
    this.current_scale.y = this.asset.scale_base.y;
    this.fillTiles();
    if (!string.IsNullOrEmpty(this.asset.kingdom))
      this.setKingdom(World.world.kingdoms_wild.get(this.asset.kingdom));
    if (!this.isUnderConstruction())
    {
      int pIndex = -1;
      if (pData != null)
        pIndex = pData.frameID;
      this.initAnimationData();
      if (pIndex != -1)
        this.setAnimData(pIndex);
    }
    this.checkMaterial();
    this.setPositionDirty();
    this.updatePosition();
    if (pAsset.storage && this.data.resources == null)
      this.data.resources = new CityResources();
    if (pAsset.book_slots > 0 && this.data.books == null)
      this.data.books = new StorageBooks();
    if (pAsset.smoke)
      this.addComponent<BuildingSmokeEffect>();
    if (pAsset.building_type == BuildingType.Building_Poops)
      this.batch.c_poop.Add(this);
    if (pAsset.spread)
    {
      switch (pAsset.flora_type)
      {
        case FloraType.Tree:
          this.batch.c_spread_trees.Add(this);
          break;
        case FloraType.Fungi:
          this.batch.c_spread_fungi.Add(this);
          break;
        case FloraType.Plant:
          this.batch.c_spread_plants.Add(this);
          break;
      }
    }
    if (pAsset.produce_biome_food)
      this.component_food_producer = this.addComponent<BuildingBiomeFoodProducer>();
    if (pAsset.spawn_drops)
      this.addComponent<BuildingEffectSpawnDrop>();
    if (pAsset.id == "monolith")
      this.component_monolith = this.addComponent<BuildingMonolith>();
    if (pAsset.waypoint)
    {
      BuildingWaypoint buildingWaypoint;
      switch (pAsset.kingdom)
      {
        case "alien_mold":
          buildingWaypoint = (BuildingWaypoint) this.addComponent<BuildingWaypointAlienMold>();
          break;
        case "computer":
          buildingWaypoint = (BuildingWaypoint) this.addComponent<BuildingWaypointComputer>();
          break;
        case "golden_egg":
          buildingWaypoint = (BuildingWaypoint) this.addComponent<BuildingWaypointGoldenEgg>();
          break;
        case "harp":
          buildingWaypoint = (BuildingWaypoint) this.addComponent<BuildingWaypointHarp>();
          break;
        default:
          throw new ArgumentOutOfRangeException(pAsset.kingdom + " is not a valid kingdom for a waypoint");
      }
      this.component_waypoint = buildingWaypoint;
    }
    if (pAsset.grow_creep)
      this.addComponent<BuildingCreepHUB>();
    if (pAsset.wheat)
      this.component_wheat = this.addComponent<Wheat>();
    if (pAsset.building_type == BuildingType.Building_Fruits)
      this.component_fruit_growth = this.addComponent<BuildingFruitGrowth>();
    if (pAsset.ice_tower)
      this.addComponent<IceTower>();
    if (pAsset.id == "poop")
      this.addComponent<Poop>();
    if (pAsset.spawn_units)
      this.component_unit_spawner = this.addComponent<UnitSpawner>();
    if (pAsset.spread_biome)
      this.component_biome_spreader = this.addComponent<BuildingSpreadBiome>();
    if (pAsset.beehive)
      this.component_beehive = this.addComponent<Beehive>();
    if (pAsset.docks)
      this.component_docks = this.addComponent<Docks>();
    if (pAsset.tower)
      this.addComponent<BuildingTower>();
    if (pData == null && !pAsset.city_building)
    {
      this.setAnimationState(BuildingAnimationState.Normal);
      this.setScaleTween();
    }
    if (this.isRuin())
      this.makeRuins();
    else if (this.asset.city_building && this.hasCity())
    {
      this.setKingdom(this.current_tile.zone_city.kingdom);
    }
    else
    {
      if (!this.asset.city_building || this.hasCity() || !this.isAbandoned())
        return;
      this.makeAbandoned();
    }
  }

  private void debugCheckResourcesOnSpawn(BuildingAsset pAsset)
  {
  }

  public override void setStatsDirty()
  {
    base.setStatsDirty();
    if (!this.isAlive())
      return;
    this.batch.c_stats_dirty.Add(this);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void setPositionDirty()
  {
    this.positionDirty = true;
    this.batch.c_position_dirty.Add(this);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override BaseObjectData getData() => (BaseObjectData) this.data;

  public void setData(BuildingData pData) => this.data = pData;

  public void loadData(BuildingData pData)
  {
    this.setData(pData);
    pData.load();
  }

  public void loadBuilding(BuildingData pData)
  {
    if (!this.isUnderConstruction())
      this.setAnimData(pData.frameID);
    if (this.data.resources == null)
      return;
    this.resources.loadFromSave();
  }

  internal void setHaveResourcesToCollect(bool pValue)
  {
    if (pValue)
      this.data.addFlag("has_resources");
    else
      this.data.removeFlag("has_resources");
  }

  public bool hasResourcesToCollect()
  {
    if (this.asset.has_resources_grown_to_collect)
      return this.data.hasFlag("has_resources");
    return !this.chopped && this.asset.has_resources_to_collect;
  }

  internal bool canBeUpgraded()
  {
    return !this.isUnderConstruction() && (!this.asset.city_building || this.isCiv()) && this.asset.can_be_upgraded;
  }

  internal bool upgradeBuilding()
  {
    if (!this.canBeUpgraded())
      return false;
    BuildingAsset pTemplate = AssetManager.buildings.get(this.asset.upgrade_to);
    if ((pTemplate.fundament.left != this.asset.fundament.left || pTemplate.fundament.right != this.asset.fundament.right || pTemplate.fundament.top != this.asset.fundament.top || pTemplate.fundament.bottom != this.asset.fundament.bottom) && !this.checkTilesForUpgrade(this.current_tile, pTemplate))
      return false;
    this.makeZoneDirty();
    this.setTemplate(pTemplate);
    this.initAnimationData();
    this.updateStats();
    this.setMaxHealth();
    this.fillTiles();
    return true;
  }

  private void setTemplate(BuildingAsset pTemplate)
  {
    this.asset = pTemplate;
    this.data.asset_id = this.asset.id;
    this.asset.buildings.Add(this);
    if (this.asset.canBeOccupied())
      World.world.buildings.occupied_buildings.Add(this);
    this.asset.checkSpritesAreLoaded();
  }

  internal void setMaterial(string pMaterialID)
  {
    this.material = LibraryMaterials.instance.dict[pMaterialID];
  }

  internal void setKingdomCiv(Kingdom pKingdom)
  {
    if (this.kingdom == pKingdom && this.hasKingdom())
      return;
    this.setKingdom(pKingdom);
  }

  internal void makeRuins()
  {
    this.setKingdom(World.world.kingdoms_wild.get("ruins"));
    this.setState(BuildingState.Ruins);
  }

  public void makeAbandoned()
  {
    this.setKingdom(WildKingdomsManager.abandoned);
    if (this.isUnderConstruction())
    {
      this.startDestroyBuilding();
    }
    else
    {
      if (this.asset.can_be_abandoned)
        return;
      if (this.asset.has_ruin_state)
        this.startMakingRuins();
      else
        this.startDestroyBuilding();
    }
  }

  public void setKingdom(Kingdom pKingdom)
  {
    if (this.kingdom == pKingdom)
      return;
    if (this.kingdom != pKingdom)
      this.makeZoneDirty();
    this.checkKingdom();
    this.kingdom = pKingdom;
    this.checkKingdom();
    if (this.isKingdomCiv())
      this.setOwnershipState(BuildingOwnershipState.Civilization);
    else
      this.setOwnershipState(BuildingOwnershipState.World);
    this.setTilesDirty();
    World.world.sim_object_zones.setBuildingsDirty(this.chunk);
  }

  private void checkKingdom()
  {
    if (!this.hasKingdom())
      return;
    if (this.kingdom.wild)
      World.world.kingdoms_wild.setDirtyBuildings();
    else
      World.world.kingdoms.setDirtyBuildings();
  }

  public bool hasHousingLogic() => this.asset.canBeOccupied();

  private void setState(BuildingState pState)
  {
    if (this.hasHousingLogic())
      World.world.buildings.event_houses = true;
    if (this.isRemoved())
      return;
    if (pState == BuildingState.Ruins && !this.isRuin())
    {
      bool flag = false;
      if (flag)
      {
        foreach (WorldTile tile in this.tiles)
        {
          if (tile.Type.lava)
          {
            flag = true;
            break;
          }
        }
      }
      if (flag)
        this.setHealth(this.getMaxHealthPercent(0.5f));
      else
        this.setMaxHealth();
      this.stats["health"] = (float) this.getHealth();
    }
    this.data.state = pState;
    this.checkAutoRemove();
    this.checkMaterial();
    this.clearZones();
    if (!this.isRemoved())
      this.fillTiles();
    this.setTilesDirty();
    World.world.sim_object_zones.setBuildingsDirty(this.chunk);
  }

  public void checkMaterial()
  {
    if (this.data.state == BuildingState.Ruins)
      this.setMaterial(BuildingRendererSettings.cur_default_material);
    else if (BuildingRendererSettings.wobbly_material_enabled)
      this.setMaterial(this.asset.material);
    else
      this.setMaterial(BuildingRendererSettings.cur_default_material);
  }

  internal void updateKingdomColors() => this.setTilesDirty();

  internal bool checkTilesForUpgrade(WorldTile pTile, BuildingAsset pTemplate)
  {
    Vector2Int pos1 = pTile.pos;
    int num1 = ((Vector2Int) ref pos1).x - pTemplate.fundament.left;
    Vector2Int pos2 = pTile.pos;
    int num2 = ((Vector2Int) ref pos2).y - pTemplate.fundament.bottom;
    int num3 = pTemplate.fundament.right + pTemplate.fundament.left + 1;
    int num4 = pTemplate.fundament.top + pTemplate.fundament.bottom + 1;
    for (int index1 = 0; index1 < num3; ++index1)
    {
      for (int index2 = 0; index2 < num4; ++index2)
      {
        WorldTile tile = World.world.GetTile(num1 + index1, num2 + index2);
        if (tile == null || !tile.Type.can_build_on || tile.zone.city != this.city)
          return false;
        Building building = tile.building;
        if (building != null && building != this && (building.asset.priority >= this.asset.priority || building.asset.upgrade_level >= this.asset.upgrade_level))
          return false;
      }
    }
    return true;
  }

  internal void debugConstructions()
  {
    if (Object.op_Equality((Object) this.asset.building_sprites.construction, (Object) null))
      return;
    this.setUnderConstruction();
  }

  private void initAnimationData()
  {
    this.asset.checkSpritesAreLoaded();
    this.setAnimData(Randy.randomInt(0, this.asset.building_sprites.animation_data.Count));
    if (this.asset.random_flip && !this.asset.shadow)
      this.flip_x = Randy.randomBool();
    this.setScaleTween();
  }

  private void fillTiles()
  {
    if (this.tiles.Count != 0)
      this.clearTiles();
    Vector2Int pos1 = this.current_tile.pos;
    int num1 = ((Vector2Int) ref pos1).x - this.asset.fundament.left;
    Vector2Int pos2 = this.current_tile.pos;
    int num2 = ((Vector2Int) ref pos2).y - this.asset.fundament.bottom;
    int num3 = this.asset.fundament.right + this.asset.fundament.left + 1;
    int num4 = this.asset.fundament.top + this.asset.fundament.bottom + 1;
    int num5 = 0;
    for (int pX = 0; pX < num3; ++pX)
    {
      for (int pY = num5; pY < num4; ++pY)
      {
        WorldTile tile = World.world.GetTile(num1 + pX, num2 + pY);
        if (tile != null)
          this.setBuildingTile(tile, pX, pY);
      }
    }
    this.setTilesDirty();
  }

  internal void checkDirtyTiles()
  {
    if (!this.tiles_dirty)
      return;
    this.tiles_dirty = false;
    for (int index = 0; index < this.tiles.Count; ++index)
      World.world.setTileDirty(this.tiles[index]);
    this.batch?.c_tiles_dirty.Remove(this);
  }

  private void setTilesDirty()
  {
    this.tiles_dirty = true;
    this.batch?.c_tiles_dirty.Add(this);
  }

  private void forceUpdateTilesDirty()
  {
    this.setTilesDirty();
    this.checkDirtyTiles();
  }

  private void setBuildingTile(WorldTile pTile, int pX, int pY)
  {
    if (pTile.hasBuilding() && pTile.building != this)
      pTile.building.startDestroyBuilding();
    pTile.building = this;
    pTile.minimap_building_x = pX;
    pTile.minimap_building_y = pY;
    if (!this.tiles.Contains(pTile))
    {
      this.tiles.Add(pTile);
      if (!this.zones.Contains(pTile.zone))
        this.zones.Add(pTile.zone);
    }
    TileType pNewTypeMain = (TileType) null;
    TopTileType pTopType = (TopTileType) null;
    if (this.asset.transform_tiles_to_tile_type != null)
      pNewTypeMain = AssetManager.tiles.get(this.asset.transform_tiles_to_tile_type);
    if (this.asset.transform_tiles_to_top_tiles != null)
      pTopType = AssetManager.top_tiles.get(this.asset.transform_tiles_to_top_tiles);
    if (pNewTypeMain == null && pTopType == null)
      return;
    if (pNewTypeMain == null)
      pNewTypeMain = pTile.main_type;
    if (!pNewTypeMain.can_be_biome)
      return;
    MapAction.terraformTile(pTile, pNewTypeMain, pTopType, TerraformLibrary.nothing);
  }

  public void setOwnershipState(BuildingOwnershipState pState)
  {
    if (this.state_ownership != pState)
      this.makeZoneDirty();
    this.state_ownership = pState;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isRuin() => this.data.state == BuildingState.Ruins;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isRemoved() => this.data.state == BuildingState.Removed;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isNormal() => this.data.state == BuildingState.Normal;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isAbandoned()
  {
    return this.state_ownership == BuildingOwnershipState.World && this.asset.city_building;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isCiv() => this.state_ownership == BuildingOwnershipState.Civilization;

  public void prepareForSave()
  {
    this.data.cityID = !this.hasCity() ? -1L : this.city.data.id;
    this.resources?.save();
    this.data.frameID = this.animData_index;
    this.data.save();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isUsable()
  {
    return this.isAlive() && !this.isRuin() && !this.isOnRemove() && !this.isRemoved();
  }

  internal void startDestroyBuilding()
  {
    if (this.isOnRemove())
      return;
    if (this.asset.has_ruins_graphics && !this.isUnderConstruction())
      this.setState(BuildingState.Ruins);
    this.startRemove();
  }

  private void clearZones() => this.zones.Clear();

  internal void kill()
  {
    if (!this.isAlive())
      return;
    this.clearZones();
    this.setAlive(false);
    if (this.asset.city_building)
      ++World.world.map_stats.housesDestroyed;
    if (!this.hasBooks())
      return;
    foreach (long listBook in this.data.books.list_books)
      World.world.books.burnBook(World.world.books.get(listBook));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override City getCity() => this.city;

  public City city => this.current_tile.zone.city;

  internal override void updateStats()
  {
    base.updateStats();
    this.stats.clear();
    this.stats.mergeStats(this.asset.base_stats);
    if (this.getHealth() > this.getMaxHealth())
      this.setMaxHealth();
    this.batch.c_stats_dirty.Remove(this);
  }

  internal void chopTree()
  {
    if (this.chopped || (this.asset.become_alive_when_chopped || WorldLawLibrary.world_law_bark_bites_back.isEnabled()) && Randy.randomChance(0.2f) && ActionLibrary.tryToMakeFloraAlive(this))
      return;
    this.finishAllStatusEffects();
    MusicBox.playSound("event:/SFX/NATURE/TreeFall", this.current_tile, true, true);
    this.chopped = true;
    this.setHaveResourcesToCollect(false);
    this.scale_helper.doRotateTween(Randy.randomBool() ? 90f : -90f, 1f, new Action(this.finishChop));
    this.batch.c_angle.Add(this);
  }

  private void finishChop() => this.startRemove();

  private void startRemove()
  {
    if (this.isOnRemove())
      return;
    if (!this.isUnderConstruction() && this.asset.has_sound_destroyed)
      MusicBox.playSound(this.asset.sound_destroyed, this.current_tile, true, true);
    this.setAnimationState(BuildingAnimationState.OnRemove);
    this.clearTiles();
    this.clearComponents();
    this.setHaveResourcesToCollect(false);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isAnimationState(BuildingAnimationState pState) => this.animation_state == pState;

  internal void startMakingRuins()
  {
    if (!this.asset.has_ruin_state)
    {
      this.startRemove();
    }
    else
    {
      if (this.isAnimationState(BuildingAnimationState.OnRuin) || this.data.state == BuildingState.Ruins)
        return;
      this.setAnimationState(BuildingAnimationState.OnRuin);
      this.makeRuins();
    }
  }

  internal void removeBuildingFinal()
  {
    this.setState(BuildingState.Removed);
    this.clearZones();
    this.clearTiles();
    this.kill();
    this.current_tile.zone.removeBuildingMain(this);
    World.world.buildings.scheduleDestroyOnPlay(this);
  }

  internal void clearTiles()
  {
    this.forceUpdateTilesDirty();
    for (int index = 0; index < this.tiles.Count; ++index)
      this.tiles[index].building = (Building) null;
    this.tiles.Clear();
  }

  private void clearComponents()
  {
    if (this.asset.flora_type == FloraType.Tree)
      this.batch.c_spread_trees.Remove(this);
    if (this.asset.flora_type == FloraType.Fungi)
      this.batch.c_spread_fungi.Remove(this);
    if (this.asset.flora_type == FloraType.Plant)
      this.batch.c_spread_plants.Remove(this);
    if (this.asset.building_type == BuildingType.Building_Poops)
      this.batch.c_poop.Remove(this);
    if (this.components_list == null)
      return;
    this.batch.c_components.Remove(this);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isOnRemove() => this.animation_state == BuildingAnimationState.OnRemove;

  internal void setAnimationState(BuildingAnimationState pState)
  {
    if (this.isOnRemove())
      return;
    this.animation_state = pState;
    this.checkTweens();
  }

  internal void completeMakingRuin()
  {
    this.setState(BuildingState.Ruins);
    this.setAnimationState(BuildingAnimationState.Normal);
    this.setScaleTween();
  }

  private void checkAutoRemove()
  {
    if (this.batch == null)
      return;
    if (this.asset.auto_remove_ruin && this.isRuin() && !this.isCiv())
      this.batch.c_auto_remove.Add(this);
    else
      this.batch.c_auto_remove.Remove(this);
  }

  internal void updateAutoRemove(float pElapsed)
  {
    if ((double) this._auto_remove_timer < 300.0)
    {
      this._auto_remove_timer += pElapsed;
    }
    else
    {
      this._auto_remove_timer = 0.0f;
      this.batch.c_auto_remove.Remove(this);
      this.startDestroyBuilding();
    }
  }

  internal void updateTimerShakeResources(float pElapsed)
  {
    if ((double) this._timer_shake_resource <= 0.0)
      return;
    this._timer_shake_resource -= pElapsed;
    if ((double) this._timer_shake_resource > 0.0)
      return;
    this.batch.c_resource_shaker.Remove(this);
  }

  internal void updateComponents(float pElapsed)
  {
    for (int index = 0; index < this.components_list.Count; ++index)
      this.components_list[index].update(pElapsed);
  }

  public void updatePosition()
  {
    if (!this.positionDirty)
      return;
    this.positionDirty = false;
    this.batch.c_position_dirty.Remove(this);
    this.cur_transform_position = this.current_tile.posV3;
    if ((double) this.cur_transform_position.z < 0.0)
      this.cur_transform_position.z = 0.0f;
    this.cur_transform_position.x += this._shake_offset.x;
    this.cur_transform_position.y += this._shake_offset.y;
    this.cur_transform_position.z = this.asset.bonus_z - 0.2f;
  }

  internal void spawnBurstSpecial(int pAmount = 1)
  {
    MapBox world = World.world;
    Vector2Int pos = this.current_tile.pos;
    int x = ((Vector2Int) ref pos).x;
    pos = this.current_tile.pos;
    int y = ((Vector2Int) ref pos).y;
    WorldTile pTile = world.GetTile(x, y) ?? this.current_tile;
    for (int index = 0; index < pAmount; ++index)
      World.world.drop_manager.spawnParabolicDrop(pTile, this.asset.spawn_drop_id, this.asset.spawn_drop_start_height, this.asset.spawn_drop_min_height, this.asset.spawn_drop_max_height, this.asset.spawn_drop_min_radius, this.asset.spawn_drop_max_radius);
  }

  internal bool updateBuild(int pProgress = 1)
  {
    this.data.change("construction_progress", pProgress);
    this.startShake(0.3f);
    bool flag = false;
    if (this.getConstructionProgress() > this.asset.construction_progress_needed)
    {
      flag = true;
      this.completeConstruction();
      if (this.asset.has_sound_built)
        MusicBox.playSound(this.asset.sound_built, this.current_tile, true, true);
      this.initAnimationData();
      this.setScaleTween(0.25f);
    }
    else
      this.setScaleTween(0.75f);
    return flag;
  }

  private void makeZoneDirty()
  {
    this.current_tile.zone.setDirty(true);
    if (!this.hasHousingLogic())
      return;
    World.world.buildings.event_houses = true;
  }

  public bool hasResidentSlots()
  {
    return this.asset.hasHousingSlots() && this.asset.housing_slots > this.countResidents();
  }

  public int countResidents() => this.residents.Count;

  public bool hasResidents() => this.countResidents() > 0;

  public void startShake(float pDuration, float pIntensityX = 0.1f, float pIntensityY = 0.1f)
  {
    this._shake_timer = pDuration;
    this._shake_intensity_x = pIntensityX;
    this._shake_intensity_y = pIntensityY;
    this.batch?.c_shake.Add(this);
  }

  internal void resourceGathering(float pElapsed)
  {
    if ((double) this._timer_shake_resource > 0.0)
      return;
    this.batch.c_resource_shaker.Add(this);
    this.startShake(0.3f);
    this._timer_shake_resource = 1f;
  }

  public void updateShake(float pElapsed)
  {
    if ((double) this._shake_timer <= 0.0)
      return;
    this._shake_timer -= pElapsed;
    if ((double) this._shake_timer < 0.0)
    {
      this._shake_offset = Vector2.zero;
      this.batch.c_shake.Remove(this);
    }
    else
    {
      this._shake_offset.x = ((Random) ref this.batch.rnd).NextFloat(-this._shake_intensity_x, this._shake_intensity_x);
      this._shake_offset.y = ((Random) ref this.batch.rnd).NextFloat(-this._shake_intensity_y, this._shake_intensity_y);
    }
    this.setPositionDirty();
  }

  internal override void getHitFullHealth(AttackType pAttackType)
  {
    this.getHit((float) this.getHealth(), false, pAttackType, (BaseSimObject) null, false, false, false);
  }

  internal override void getHit(
    float pDamage,
    bool pFlash = true,
    AttackType pAttackType = AttackType.Other,
    BaseSimObject pAttacker = null,
    bool pSkipIfShake = true,
    bool pMetallicWeapon = false,
    bool pCheckDamageReduction = true)
  {
    if (!this.isAnimationState(BuildingAnimationState.Normal))
      return;
    this.changeHealth((int) -(double) pDamage);
    if (pAttackType == AttackType.Weapon && this.asset.has_sound_hit)
      MusicBox.playSound(this.asset.sound_hit, this.current_tile, true, true);
    this.startShake(0.3f);
    if (!this.hasHealth())
    {
      if (this.data.state == BuildingState.Ruins)
        this.startDestroyBuilding();
      else
        this.startMakingRuins();
    }
    else
      this.setScaleTween(0.75f);
  }

  internal void extractResources(Actor pBy)
  {
    this.setScaleTween(0.75f);
    switch (this.asset.building_type)
    {
      case BuildingType.Building_Tree:
        this.chopTree();
        break;
      case BuildingType.Building_Fruits:
        this.component_fruit_growth.reset();
        this.setHaveResourcesToCollect(false);
        if (!Randy.randomChance(0.2f))
          break;
        this.startDestroyBuilding();
        break;
      case BuildingType.Building_Hives:
        this.component_beehive.honey = 0;
        this.setHaveResourcesToCollect(false);
        break;
      case BuildingType.Building_Poops:
      case BuildingType.Building_Mineral:
        this.startRemove();
        break;
      case BuildingType.Building_Wheat:
      case BuildingType.Building_Plant:
        this.startDestroyBuilding();
        break;
    }
  }

  internal Color32 getColorForMinimap(WorldTile pTile)
  {
    return Config.EVERYTHING_MAGIC_COLOR ? Toolbox.EVERYTHING_MAGIC_COLOR32 : this.asset.building_sprites.map_icon.getColor(pTile.minimap_building_x, pTile.minimap_building_y, this);
  }

  public WorldTile getConstructionTile()
  {
    if (this.asset.docks)
    {
      (TileZone[], int) allZonesFromTile = Toolbox.getAllZonesFromTile(this.current_tile);
      foreach (TileZone pZone in allZonesFromTile.Item1.LoopRandom<TileZone>(allZonesFromTile.Item2))
      {
        using (IEnumerator<WorldTile> enumerator = this.checkZoneForDockConstruction(pZone).GetEnumerator())
        {
          if (enumerator.MoveNext())
            return enumerator.Current;
        }
      }
    }
    return Randy.getRandom<WorldTile>(this.tiles);
  }

  public int getConstructionProgress()
  {
    int pResult;
    this.data.get("construction_progress", out pResult);
    return pResult;
  }

  public void completeConstruction()
  {
    this.data.removeInt("construction_progress");
    this.data.removeFlag("under_construction");
    this.makeZoneDirty();
  }

  public bool isUnderConstruction()
  {
    return this.asset.has_sprite_construction && this.data.hasFlag("under_construction");
  }

  public void setUnderConstruction()
  {
    if (!this.asset.has_sprite_construction)
      return;
    this.data.addFlag("under_construction");
  }

  public bool canRemoveForFarms() => this.asset.flora;

  internal IEnumerable<WorldTile> checkZoneForDockConstruction(TileZone pZone)
  {
    Building building = this;
    if (pZone.city != null && pZone.city == building.city)
    {
      foreach (WorldTile pT2 in pZone.tiles.LoopRandom<WorldTile>())
      {
        if (pT2.Type.ground && Toolbox.SquaredDistTile(building.current_tile, pT2) <= 49)
          yield return pT2;
      }
    }
  }

  internal void checkStartSpawnAnimation()
  {
    Sprite[] spawn = this.animData.spawn;
    if ((spawn != null ? (spawn.Length != 0 ? 1 : 0) : 0) == 0)
      return;
    this.check_spawn_animation = true;
  }

  public Sprite calculateMainSprite()
  {
    bool flag1 = true;
    bool flag2 = this.isRuin();
    if (flag2)
      flag1 = false;
    if (this.isUnderConstruction())
    {
      this.last_main_sprite = this.asset.building_sprites.construction;
      return this.last_main_sprite;
    }
    Sprite[] pFrames;
    if (this.asset.has_special_animation_state)
      pFrames = !this.hasResourcesToCollect() ? this.animData.special : this.animData.main;
    else if (flag2 && this.asset.has_ruins_graphics)
    {
      flag1 = false;
      pFrames = this.animData.ruins;
    }
    else if (this.asset.spawn_drops && this.data.hasFlag("stop_spawn_drops"))
      pFrames = this.animData.main_disabled;
    else if (this.asset.can_be_abandoned && this.isAbandoned())
    {
      Sprite[] mainDisabled = this.animData.main_disabled;
      pFrames = (mainDisabled != null ? (mainDisabled.Length != 0 ? 1 : 0) : 0) == 0 ? this.animData.main : this.animData.main_disabled;
      flag1 = false;
    }
    else
    {
      pFrames = this.animData.main;
      if (this.asset.get_override_sprites_main != null)
      {
        Sprite[] spriteArray = this.asset.get_override_sprites_main(this);
        if (spriteArray != null)
          pFrames = spriteArray;
      }
    }
    return !this.check_spawn_animation ? (!flag1 || pFrames.Length == 1 ? pFrames[0] : AnimationHelper.getSpriteFromList(this.GetHashCode(), (IList<Sprite>) pFrames, this.asset.animation_speed)) : this.getSpawnFrameSprite();
  }

  public bool isColoredSpriteNeedsCheck(Sprite pMainSprite)
  {
    return this.last_main_sprite == null || this.last_main_sprite.GetHashCode() != pMainSprite.GetHashCode() || this._last_color_asset != this.kingdom.getColor();
  }

  public Sprite calculateColoredSprite(Sprite pMainSprite)
  {
    if (this.isColoredSpriteNeedsCheck(pMainSprite))
    {
      this._last_colored_sprite = DynamicSprites.getRecoloredBuilding(pMainSprite, this.kingdom.getColor(), this.asset.atlas_asset);
      this.last_main_sprite = pMainSprite;
      this._last_color_asset = this.kingdom.getColor();
    }
    return this._last_colored_sprite;
  }

  public Sprite getLastColoredSprite() => this._last_colored_sprite;

  public void clearSprites()
  {
    this.last_main_sprite = (Sprite) null;
    this._last_colored_sprite = (Sprite) null;
    this._last_color_asset = (ColorAsset) null;
  }

  public Sprite checkSpriteToRender() => this.calculateColoredSprite(this.calculateMainSprite());

  public Vector3 getCurrentScale()
  {
    float tweenBuildingsValue = World.world.quality_changer.getTweenBuildingsValue();
    float num1 = this.current_scale.y * tweenBuildingsValue;
    float num2 = this.current_scale.x * tweenBuildingsValue;
    if ((double) this._last_scale.y != (double) num1 || (double) this._last_scale.x != (double) num2)
      ((Vector3) ref this._last_scale).Set(num2, num1, 1f);
    return this._last_scale;
  }

  public bool isFullyGrown()
  {
    if (!this.asset.can_be_grown)
      return true;
    return this.asset.wheat && this.component_wheat.isMaxLevel();
  }

  private Sprite getSpawnFrameSprite()
  {
    Sprite[] spawn = this.animData.spawn;
    float timeElapsedSince = World.world.getWorldTimeElapsedSince(this.data.created_time);
    float num = (float) ((double) spawn.Length * (double) this.asset.animation_speed / 60.0);
    Sprite spawnFrameSprite;
    if ((double) num > (double) timeElapsedSince)
    {
      int index = (int) ((double) timeElapsedSince / (double) num * (double) spawn.Length);
      spawnFrameSprite = spawn[index];
    }
    else
    {
      spawnFrameSprite = spawn.Last<Sprite>();
      this.check_spawn_animation = false;
    }
    return spawnFrameSprite;
  }

  public CityResources resources => this.data.resources;

  public int takeResource(string pResourceID, int pAmount)
  {
    return this.resources.change(pResourceID, -pAmount);
  }

  public int getResourcesAmount(string pResourceID) => this.resources.get(pResourceID);

  public int addResources(string pResourceID, int pAmount)
  {
    return this.resources.change(pResourceID, pAmount);
  }

  public bool hasSpaceForResource(ResourceAsset pResourceAsset)
  {
    return this.resources.hasSpaceForResource(pResourceAsset);
  }

  public bool hasResourcesForNewItems() => this.resources.hasResourcesForNewItems();

  public int countFood() => this.resources.countFood();

  public ResourceAsset getRandomSuitableFood(Subspecies pSubspecies, string pFavoriteFood = null)
  {
    return this.resources.getRandomSuitableFood(pSubspecies, pFavoriteFood);
  }

  public override void Dispose()
  {
    this.kingdom = (Kingdom) null;
    this._last_colored_sprite = (Sprite) null;
    this._last_color_asset = (ColorAsset) null;
    this.last_main_sprite = (Sprite) null;
    this.batch = (BatchBuildings) null;
    this.data = (BuildingData) null;
    this.asset = (BuildingAsset) null;
    this.tiles.Clear();
    this.animData = (BuildingAnimationData) null;
    this.zones.Clear();
    if (this.components_list != null)
    {
      for (int index = 0; index < this.components_list.Count; ++index)
      {
        BaseBuildingComponent components = this.components_list[index];
        components.Dispose();
        World.world.buildings.component_pool.release(components);
      }
      this.components_list.Clear();
      this.components_list.Dispose();
      this.components_list = (ListPool<BaseBuildingComponent>) null;
    }
    this.component_docks = (Docks) null;
    this.component_wheat = (Wheat) null;
    this.component_fruit_growth = (BuildingFruitGrowth) null;
    this.component_unit_spawner = (UnitSpawner) null;
    this.component_biome_spreader = (BuildingSpreadBiome) null;
    this.component_monolith = (BuildingMonolith) null;
    this.component_waypoint = (BuildingWaypoint) null;
    this.component_food_producer = (BuildingBiomeFoodProducer) null;
    this.component_beehive = (Beehive) null;
    this.scale_helper.reset();
    base.Dispose();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(Building pObject) => this.GetHashCode() == pObject.GetHashCode();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int CompareTo(Building pTarget) => this.GetHashCode().CompareTo(pTarget.GetHashCode());

  public void checkVegetationSpread(float pElapsed)
  {
    BuildingAsset asset = this.asset;
    if (!Randy.randomChance(asset.spread_chance))
      return;
    WorldTile random1 = this.current_tile.neighboursAll.GetRandom<WorldTile>();
    for (int index = 0; (double) index < (double) asset.spread_steps; ++index)
      random1 = random1.neighboursAll.GetRandom<WorldTile>();
    string random2 = asset.spread_ids.GetRandom<string>();
    BuildingAsset pAsset = AssetManager.buildings.get(random2);
    this.tryToGrowOnTile(random1, pAsset);
  }

  private bool tryToGrowOnTile(WorldTile pTile, BuildingAsset pAsset, bool pCheckLimit = true)
  {
    if (pCheckLimit && pTile.zone.hasReachedBuildingLimit(pTile, pAsset) || !World.world.buildings.canBuildFrom(pTile, pAsset, (City) null, pFloraGrowth: true))
      return false;
    World.world.buildings.addBuilding(pAsset, pTile);
    if (pAsset.flora_type == FloraType.Tree)
      ++World.world.game_stats.data.treesGrown;
    else if (pAsset.flora_type == FloraType.Plant || pAsset.flora_type == FloraType.Fungi)
      ++World.world.game_stats.data.floraGrown;
    return true;
  }
}
