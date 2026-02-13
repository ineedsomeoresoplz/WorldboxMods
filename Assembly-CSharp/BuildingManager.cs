// Decompiled with JetBrains decompiler
// Type: BuildingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tools;
using UnityEngine;

#nullable disable
public class BuildingManager : SimSystemManager<Building, BuildingData>
{
  private List<WorldTile> _temp_list_tiles = new List<WorldTile>();
  private JobManagerBuildings _job_manager;
  private Building[] _array_visible_buildings = new Building[0];
  private int _visible_buildings_count;
  public BuildingRenderData render_data = new BuildingRenderData(4096 /*0x1000*/);
  public HashSet<Building> occupied_buildings = new HashSet<Building>();
  public List<Building> visible_stockpiles = new List<Building>();
  public List<Building> sparkles = new List<Building>();
  public MultiStackPool<BaseBuildingComponent> component_pool = new MultiStackPool<BaseBuildingComponent>();
  private bool _need_normal_check;

  public BuildingManager()
  {
    this.type_id = "building";
    this._job_manager = new JobManagerBuildings("buildings");
  }

  public override void clear()
  {
    this._job_manager.clear();
    Array.Clear((Array) this._array_visible_buildings, 0, this._array_visible_buildings.Length);
    this._temp_list_tiles.Clear();
    this.occupied_buildings.Clear();
    this.checkContainer();
    this.scheduleDestroyAllOnWorldClear();
    this.checkObjectsToDestroy();
    base.clear();
  }

  protected override void destroyObject(Building pBuilding)
  {
    base.destroyObject(pBuilding);
    if (pBuilding.hasHousingLogic())
      this.event_houses = true;
    pBuilding.setAlive(false);
    pBuilding.asset.buildings.Remove(pBuilding);
    this.occupied_buildings.Remove(pBuilding);
    this.removeObject(pBuilding);
    this._job_manager.removeObject(pBuilding, pBuilding.batch);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    Bench.bench("buildings", "game_total");
    this.checkContainer();
    this._job_manager.updateBase(pElapsed);
    this.checkContainer();
    Bench.benchEnd("buildings", "game_total");
  }

  public override void loadFromSave(List<BuildingData> pList)
  {
    base.loadFromSave(pList);
    this.checkContainer();
  }

  internal Building addBuilding(
    string pID,
    WorldTile pTile,
    bool pCheckForBuild = false,
    bool pSfx = false,
    BuildPlacingType pType = BuildPlacingType.New)
  {
    return this.addBuilding(AssetManager.buildings.get(pID), pTile, pCheckForBuild, pSfx, pType);
  }

  internal Building addBuilding(
    BuildingAsset pAsset,
    WorldTile pTile,
    bool pCheckForBuild = false,
    bool pSfx = false,
    BuildPlacingType pType = BuildPlacingType.New)
  {
    if (pCheckForBuild && !this.canBuildFrom(pTile, pAsset, (City) null, pType))
      return (Building) null;
    Building building = this.newObject();
    building.create();
    building.setBuilding(pTile, pAsset, (BuildingData) null);
    building.checkStartSpawnAnimation();
    if (!building.asset.city_building)
      return building;
    ++World.world.map_stats.housesBuilt;
    return building;
  }

  protected override void addObject(Building pObject)
  {
    base.addObject(pObject);
    this._job_manager.addNewObject(pObject);
  }

  public override Building loadObject(BuildingData pData)
  {
    if (pData.state == BuildingState.Removed)
      return (Building) null;
    BuildingAsset buildingAsset = AssetManager.buildings.get(pData.asset_id);
    if (buildingAsset == null)
      return (Building) null;
    WorldTile tileSimple = World.world.GetTileSimple(pData.mainX, pData.mainY);
    if (!this.canBuildFrom(tileSimple, buildingAsset, (City) null, BuildPlacingType.Load))
      return (Building) null;
    Building building = base.loadObject(pData);
    building.create();
    building.setBuilding(tileSimple, buildingAsset, pData);
    building.loadBuilding(pData);
    return building;
  }

  internal bool canBuildFrom(
    WorldTile pTile,
    BuildingAsset pNewBuildingAsset,
    City pCity,
    BuildPlacingType pType = BuildPlacingType.New,
    bool pFloraGrowth = false)
  {
    Subspecies mainSubspecies = pCity?.getMainSubspecies();
    bool flag1 = mainSubspecies != null && pNewBuildingAsset.city_building && pNewBuildingAsset.check_for_adaptation_tags;
    if (flag1 && pTile.Type.is_biome)
    {
      string allowedToBuildWithTag = pTile.Type.only_allowed_to_build_with_tag;
      if (allowedToBuildWithTag != null && !mainSubspecies.hasMetaTag(allowedToBuildWithTag))
        return false;
    }
    BuildingFundament fundament = pNewBuildingAsset.fundament;
    int num1 = pTile.x - fundament.left;
    int num2 = pTile.y - fundament.bottom;
    int width = fundament.width;
    int height = fundament.height;
    bool flag2 = false;
    bool flag3 = false;
    bool docks = pNewBuildingAsset.docks;
    List<WorldTile> tempListTiles = this._temp_list_tiles;
    tempListTiles.Clear();
    bool flag4 = !WorldLawLibrary.world_law_roots_without_borders.isEnabled();
    WorldTile tile1 = pCity?.getTile();
    if (pCity != null && tile1 == null)
      return false;
    bool flag5 = pType == BuildPlacingType.New && Randy.randomChance(0.1f);
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
      {
        WorldTile tile2 = World.world.GetTile(num1 + index1, num2 + index2);
        if (tile2 == null)
          return false;
        if (flag1)
        {
          string allowedToBuildWithTag = tile2.Type.only_allowed_to_build_with_tag;
          if (allowedToBuildWithTag != null && !mainSubspecies.hasMetaTag(allowedToBuildWithTag))
            return false;
        }
        tempListTiles.Add(tile2);
        Building building = tile2.building;
        TileTypeBase type = tile2.Type;
        if (docks)
        {
          if (type.ocean && OceanHelper.goodForNewDock(tile2))
            flag3 = true;
          if (type.ground)
            flag2 = true;
        }
        if (pCity != null && (!docks && !tile2.isSameIsland(tile1) || !tile2.isSameCityHere(pCity) || pNewBuildingAsset.only_build_tiles && !type.can_build_on) || (pType != BuildPlacingType.Load ? 0 : (type.id == "frozen_low" ? 1 : (type.id == "frozen_high" ? 1 : 0))) == 0 & flag4 && !pNewBuildingAsset.isOverlaysBiomeTags(type) && (!pFloraGrowth || !pNewBuildingAsset.isOverlaysBiomeSpreadTags(type)))
          return false;
        if (pNewBuildingAsset.flora && building != null)
        {
          if (!building.asset.flora)
            return false;
          if (pNewBuildingAsset.flora_size <= building.asset.flora_size)
          {
            if (flag5 && building.asset.flora_size == FloraSize.Tiny && building.asset.flora_size == pNewBuildingAsset.flora_size)
            {
              if (building.asset == pNewBuildingAsset)
                return false;
            }
            else if (!building.isRuin())
              return false;
          }
          if (!tile2.canGrow())
            return false;
        }
        if (type.liquid && !pNewBuildingAsset.can_be_placed_on_liquid || pNewBuildingAsset.destroy_on_liquid && type.ocean || !tile2.canBuildOn(pNewBuildingAsset))
          return false;
        if (pNewBuildingAsset.check_for_close_building && pType == BuildPlacingType.New)
        {
          if (index1 == 0)
          {
            if (this.isBuildingNearby(tile2.tile_left))
              return false;
          }
          else if (index1 == width - 1 && this.isBuildingNearby(tile2.tile_right))
            return false;
          if (index2 == 0)
          {
            if (this.isBuildingNearby(tile2.tile_down) || tile2.has_tile_down && this.isBuildingNearby(tile2.tile_down.tile_down))
              return false;
          }
          else if (index2 == height - 1 && (this.isBuildingNearby(tile2.tile_up) || tile2.has_tile_up && this.isBuildingNearby(tile2.tile_up.tile_up)))
            return false;
        }
      }
    }
    if (!docks || pType != BuildPlacingType.New)
      return true;
    if (flag3 && !flag2)
    {
      for (int index3 = 0; index3 < tempListTiles.Count; ++index3)
      {
        WorldTile worldTile = tempListTiles[index3];
        for (int index4 = 0; index4 < worldTile.neighbours.Length; ++index4)
        {
          WorldTile neighbour = worldTile.neighbours[index4];
          if (neighbour.Type.ground && neighbour.region.island == tile1?.region.island)
            return true;
        }
      }
    }
    return false;
  }

  private bool isBuildingNearby(WorldTile pTile)
  {
    if (pTile == null)
      return true;
    Building building = pTile.building;
    return building != null && building.isUsable() && building.asset.city_building;
  }

  public Building getNearbyBuildingToLive(Actor pActor, bool pOnlyBuilt)
  {
    foreach (Building buildingFromZone in this.getBuildingFromZones(pActor.current_tile, 10f))
    {
      if (buildingFromZone.asset.hasHousingSlots() && buildingFromZone.current_tile.isSameIsland(pActor.current_tile) && buildingFromZone.hasResidentSlots())
      {
        if (pOnlyBuilt)
        {
          if (buildingFromZone.isUnderConstruction())
            continue;
        }
        else if (!buildingFromZone.isUnderConstruction())
          continue;
        if (buildingFromZone.kingdom == pActor.kingdom)
          return buildingFromZone;
      }
    }
    return (Building) null;
  }

  public IEnumerable<Building> getBuildingFromZones(WorldTile pTile, float pRadius)
  {
    foreach (Building buildingFromZone in this.checkZoneForBuilding(pTile, pTile.zone, pRadius))
      yield return buildingFromZone;
    int tSize = (int) (pRadius / 8f) + 1;
    int startX = pTile.zone.x - tSize;
    int startY = pTile.zone.y - tSize;
    for (int iX = 0; iX < tSize * 2; ++iX)
    {
      for (int iY = 0; iY < tSize * 2; ++iY)
      {
        TileZone zone = World.world.zone_calculator.getZone(iX + startX, iY + startY);
        if (zone != null)
        {
          foreach (Building buildingFromZone in this.checkZoneForBuilding(pTile, zone, pRadius))
            yield return buildingFromZone;
        }
      }
    }
  }

  private IEnumerable<Building> checkZoneForBuilding(
    WorldTile pTile,
    TileZone pZone,
    float pRadius)
  {
    if (pZone.buildings_all.Any<Building>())
    {
      float tRadius = pRadius * pRadius;
      foreach (Building building in pZone.buildings_all)
      {
        if (((double) tRadius == 0.0 || (double) Toolbox.SquaredDistTile(building.current_tile, pTile) <= (double) tRadius) && !building.isRuin() && building.current_tile.isSameIsland(pTile))
          yield return building;
      }
    }
  }

  public void debugJobManager(DebugTool pTool) => this._job_manager.debug(pTool);

  private void prepareLists()
  {
    this._array_visible_buildings = Toolbox.checkArraySize<Building>(this._array_visible_buildings, this.Count);
    this.render_data.checkSize(this.Count);
    this.visible_stockpiles.Clear();
    this.sparkles.Clear();
    this.checkContainer();
  }

  internal void calculateVisibleBuildings()
  {
    Bench.bench("buildings_prepare", "game_total");
    this.prepareLists();
    this._visible_buildings_count = 0;
    Bench.benchEnd("buildings_prepare", "game_total");
    if (!World.world.quality_changer.shouldRenderBuildings())
    {
      Bench.clearBenchmarkEntrySkipMultiple("game_total", "buildings_render_data_parallel_256", "buildings_fill_visible", "buildings_render_data_normal");
    }
    else
    {
      Bench.bench("buildings_fill_visible", "game_total");
      this.fillVisibleObjects();
      Bench.benchEnd("buildings_fill_visible", "game_total");
      Bench.bench("buildings_render_data_parallel_256", "game_total");
      this.precalculateRenderDataParallel();
      Bench.benchEnd("buildings_render_data_parallel_256", "game_total");
      Bench.bench("buildings_render_data_normal", "game_total");
      this.precalculateRenderDataNormal();
      Bench.benchEnd("buildings_render_data_normal", "game_total");
    }
  }

  private void fillVisibleObjects()
  {
    Building[] visibleBuildings = this._array_visible_buildings;
    List<TileZone> visibleZones = World.world.zone_camera.getVisibleZones();
    int count1 = visibleZones.Count;
    int arrayIndex = 0;
    for (int index = 0; index < count1; ++index)
    {
      List<Building> buildingsRenderList = visibleZones[index].buildings_render_list;
      int count2 = buildingsRenderList.Count;
      buildingsRenderList.CopyTo(visibleBuildings, arrayIndex);
      arrayIndex += count2;
    }
    this._visible_buildings_count = arrayIndex;
  }

  private void precalculateRenderDataParallel()
  {
    Building[] tArrayVisibleBuildings = this._array_visible_buildings;
    bool tNeedShadows = World.world.quality_changer.shouldRenderBuildingShadows();
    int tTotalVisibleObjects = this._visible_buildings_count;
    Vector3[] tRenderScales = this.render_data.scales;
    Vector3[] tRenderPositions = this.render_data.positions;
    Vector3[] tRenderRotations = this.render_data.rotations;
    Material[] tRenderMaterials = this.render_data.materials;
    bool[] tRenderFlipXStates = this.render_data.flip_x_states;
    Color[] tRenderColors = this.render_data.colors;
    Sprite[] tRenderMainSprites = this.render_data.main_sprites;
    Sprite[] tRenderColoredSprites = this.render_data.colored_sprites;
    bool[] tRenderShadows = this.render_data.shadows;
    Sprite[] tRenderShadowSprites = this.render_data.shadow_sprites;
    int tDynamicBatchSize = 256 /*0x0100*/;
    int toExclusive = ParallelHelper.calcTotalBatches(tTotalVisibleObjects, tDynamicBatchSize);
    bool tNeedNormalCheck = false;
    Parallel.For(0, toExclusive, World.world.parallel_options, (Action<int>) (pBatchIndex =>
    {
      int batchBeg = ParallelHelper.calculateBatchBeg(pBatchIndex, tDynamicBatchSize);
      int batchEnd = ParallelHelper.calculateBatchEnd(batchBeg, tDynamicBatchSize, tTotalVisibleObjects);
      for (int index = batchBeg; index < batchEnd; ++index)
      {
        Building building = tArrayVisibleBuildings[index];
        BuildingAsset asset = building.asset;
        tRenderScales[index] = building.getCurrentScale();
        tRenderPositions[index] = building.cur_transform_position;
        tRenderRotations[index] = building.current_rotation;
        tRenderMaterials[index] = building.material;
        tRenderFlipXStates[index] = building.flip_x;
        tRenderColors[index] = building.kingdom.asset.color_building;
        Sprite mainSprite = building.calculateMainSprite();
        tRenderMainSprites[index] = mainSprite;
        if (building.isColoredSpriteNeedsCheck(mainSprite))
        {
          tRenderColoredSprites[index] = (Sprite) null;
          tNeedNormalCheck = true;
        }
        else
          tRenderColoredSprites[index] = building.getLastColoredSprite();
        if (tNeedShadows)
        {
          tRenderShadows[index] = asset.shadow && !building.chopped;
          tRenderShadowSprites[index] = DynamicSprites.getShadowBuilding(building.asset, tRenderMainSprites[index]);
        }
        if (asset.is_stockpile)
          tNeedNormalCheck = true;
        if (asset.sparkle_effect)
          tNeedNormalCheck = true;
      }
    }));
    this._need_normal_check = tNeedNormalCheck;
  }

  private void precalculateRenderDataNormal()
  {
    if (!this._need_normal_check)
      return;
    BuildingRenderData renderData = this.render_data;
    int visibleBuildingsCount = this._visible_buildings_count;
    Sprite[] coloredSprites = renderData.colored_sprites;
    Sprite[] mainSprites = renderData.main_sprites;
    for (int index = 0; index < visibleBuildingsCount; ++index)
    {
      Building arrayVisibleBuilding = this._array_visible_buildings[index];
      if (arrayVisibleBuilding.asset.is_stockpile)
        this.visible_stockpiles.Add(arrayVisibleBuilding);
      if (arrayVisibleBuilding.asset.sparkle_effect)
        this.sparkles.Add(arrayVisibleBuilding);
      if (coloredSprites[index] == null)
        coloredSprites[index] = arrayVisibleBuilding.calculateColoredSprite(mainSprites[index]);
    }
  }

  public Building[] getVisibleBuildings() => this._array_visible_buildings;

  public int countVisibleBuildings() => this._visible_buildings_count;

  public void checkWobblySetting()
  {
    bool pWobbleTreesSettingIsActive = PlayerConfig.optionEnabled("tree_wind", OptionType.Bool);
    foreach (DynamicSpritesAsset dynamicSpritesAsset in AssetManager.dynamic_sprites_library.list)
    {
      if (dynamicSpritesAsset.check_wobbly_setting)
        dynamicSpritesAsset.big_atlas = !pWobbleTreesSettingIsActive;
    }
    foreach (DynamicSpritesAsset dynamicSpritesAsset in AssetManager.dynamic_sprites_library.list)
    {
      if (dynamicSpritesAsset.buildings)
        dynamicSpritesAsset.resetAtlas();
    }
    AssetManager.buildings.checkAtlasLink(pWobbleTreesSettingIsActive);
    foreach (Building building in (SimSystemManager<Building, BuildingData>) this)
    {
      building.checkMaterial();
      building.clearSprites();
    }
  }

  public JobManagerBuildings getJobManager() => this._job_manager;
}
