// Decompiled with JetBrains decompiler
// Type: TileZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityPools;

#nullable disable
public class TileZone : IDisposable
{
  private const int MAX_BUILDING_COLLECTIONS = 16 /*0x10*/;
  public int last_drawn_id;
  public int last_drawn_hashcode;
  public int last_color_asset_id;
  private bool? _last_animated;
  public (string, int)[] debug_args;
  public bool debug_show;
  private bool _dirty;
  public bool visible;
  public bool visible_main_centered;
  public int x;
  public int y;
  public int id;
  public readonly WorldTile[] tiles = new WorldTile[64 /*0x40*/];
  public Color debug_zone_color;
  [CanBeNull]
  public City city;
  public TileZone[] neighbours;
  public TileZone[] neighbours_all;
  public bool world_edge;
  public WorldTile centerTile;
  public int tiles_with_liquid;
  public int tiles_with_ground;
  public readonly List<Building> buildings_render_list = new List<Building>();
  public readonly HashSet<Building> buildings_all = new HashSet<Building>();
  private readonly HashSet<Building>[] _building_hashset_types_array = new HashSet<Building>[16 /*0x10*/];
  internal TileZone zone_up;
  internal TileZone zone_down;
  internal TileZone zone_left;
  internal TileZone zone_right;
  private readonly Dictionary<TileTypeBase, HashSet<WorldTile>> _tile_types = new Dictionary<TileTypeBase, HashSet<WorldTile>>();
  private bool _good_for_new_city;
  public static int debug_adapted;
  public static int debug_not_adapted;
  public static int debug_soil;
  public static bool debug_can_settle;

  public bool checkShouldReRender(int pHashCode, int pID, int pColorAssetID, bool pLastAnimated)
  {
    if (this.last_drawn_id == pID && this.last_drawn_hashcode == pHashCode)
    {
      bool? lastAnimated = this._last_animated;
      bool flag = pLastAnimated;
      if (lastAnimated.GetValueOrDefault() == flag & lastAnimated.HasValue && this.last_color_asset_id == pColorAssetID)
        return false;
    }
    this.last_drawn_id = pID;
    this.last_drawn_hashcode = pHashCode;
    this._last_animated = new bool?(pLastAnimated);
    this.last_color_asset_id = pColorAssetID;
    return true;
  }

  public void resetRenderHelpers()
  {
    this.last_color_asset_id = 0;
    this.last_drawn_hashcode = 0;
    this.last_drawn_id = 0;
    this._last_animated = new bool?();
  }

  public void clearDebug()
  {
    this.debug_args = ((string, int)[]) null;
    this.debug_show = false;
  }

  public void showDebug(params (string key, int value)[] pArgs)
  {
    if (!DebugConfig.isOn(DebugOption.DebugZones))
      return;
    this.debug_show = true;
    this.debug_args = pArgs;
  }

  public WorldTile top_left_corner_tile => this.tiles.Last<WorldTile>();

  private CityPlaceFinder _city_place_finder => World.world.city_zone_helper.city_place_finder;

  public bool checkCanSettleInThisBiomes(Subspecies pSubspecies)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    foreach (KeyValuePair<TileTypeBase, HashSet<WorldTile>> tileType in this._tile_types)
    {
      TileTypeBase key = tileType.Key;
      if (key.biome_build_check)
      {
        if (key.soil)
          num3 += tileType.Value.Count;
        else if (!key.is_biome)
        {
          num1 += tileType.Value.Count;
        }
        else
        {
          string allowedToBuildWithTag = key.only_allowed_to_build_with_tag;
          if (string.IsNullOrEmpty(allowedToBuildWithTag))
            num1 += tileType.Value.Count;
          else if (!pSubspecies.hasMetaTag(allowedToBuildWithTag))
            num2 += tileType.Value.Count;
          else
            num1 += tileType.Value.Count;
        }
      }
    }
    int num4 = num3 - num1 - num2;
    bool flag = num4 > num2 || num2 <= num1;
    TileZone.debug_adapted = num1;
    TileZone.debug_not_adapted = num2;
    TileZone.debug_soil = num4;
    TileZone.debug_can_settle = flag;
    return flag;
  }

  public bool hasAnyBuildings() => this.buildings_all.Count > 0;

  public HashSet<Building> getHashset(BuildingList pType)
  {
    return this._building_hashset_types_array[(int) pType];
  }

  private HashSet<Building> getHashsetOrCreate(BuildingList pType)
  {
    int index = (int) pType;
    HashSet<Building> buildingHashsetTypes = this._building_hashset_types_array[index];
    if (buildingHashsetTypes == null)
    {
      buildingHashsetTypes = UnsafeCollectionPool<HashSet<Building>, Building>.Get();
      this._building_hashset_types_array[index] = buildingHashsetTypes;
    }
    return buildingHashsetTypes;
  }

  public bool hasAnyBuildingsInSet(BuildingList pType)
  {
    HashSet<Building> hashset = this.getHashset(pType);
    // ISSUE: explicit non-virtual call
    return hashset != null && __nonvirtual (hashset.Count) > 0;
  }

  public int countBuildingsType(BuildingList pType)
  {
    HashSet<Building> hashset = this.getHashset(pType);
    return hashset == null ? 0 : hashset.Count;
  }

  public bool isGoodForNewCity(Actor pActor)
  {
    return this.checkCanSettleInThisBiomes(pActor.subspecies) && (!this._city_place_finder.isDirty() || !this.hasCity() && (!pActor.hasCity() || !pActor.city.neighbour_zones.Contains(this))) && this.isGoodForNewCity();
  }

  public bool isGoodForNewCity()
  {
    if (this.hasCity())
      return false;
    if (this._city_place_finder.isDirty())
    {
      TileZone[] neighboursAll = this.neighbours_all;
      int length = neighboursAll.Length;
      for (int index = 0; index < length; ++index)
      {
        if (neighboursAll[index].hasCity())
          return false;
      }
      this._city_place_finder.recalc();
    }
    return this._good_for_new_city;
  }

  public void setGoodForNewCity(bool pValue) => this._good_for_new_city = pValue;

  public bool hasCity() => this.city != null;

  public bool isSameCityHere(City pCity) => this.city == pCity;

  public static bool hasZonesConnectedViaRegions(
    TileZone pZone1,
    TileZone pZone2,
    MapRegion pMainRegion1,
    ListPool<MapRegion> pListToFill)
  {
    if (pZone2.tiles_with_ground == 0 || !pMainRegion1.isTypeGround())
      return false;
    MapChunk chunk1 = pZone1.chunk;
    MapChunk chunk2 = pZone2.chunk;
    if (chunk1 == chunk2)
    {
      if (chunk1.regions.Count == 1 && pMainRegion1.isTypeGround())
      {
        pListToFill.Add(pMainRegion1);
        return true;
      }
    }
    else if (chunk1.regions.Count == 1 && chunk2.regions.Count == 1)
    {
      MapRegion region = chunk2.regions[0];
      if (pMainRegion1.isTypeGround() && region.isTypeGround())
      {
        pListToFill.Add(region);
        return true;
      }
    }
    List<MapRegion> regions = chunk2.regions;
    for (int index = 0; index < regions.Count; ++index)
    {
      MapRegion pRegion = regions[index];
      if (pRegion.isTypeGround() && pRegion.zones.Contains(pZone2))
      {
        if (pMainRegion1 == pRegion)
        {
          pListToFill.Add(pMainRegion1);
          return true;
        }
        if (pMainRegion1.island == pRegion.island && pMainRegion1.hasNeighbour(pRegion))
          pListToFill.Add(pRegion);
      }
    }
    return pListToFill.Count > 0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasLiquid() => this.tiles_with_liquid > 0;

  public bool hasBuildingOf(City pCity)
  {
    foreach (Building building in this.getHashsetOrCreate(BuildingList.Civs))
    {
      if (building.isUsable() && building.city == pCity)
        return true;
    }
    return false;
  }

  public void addTileType(TileTypeBase pType, WorldTile pTile)
  {
    HashSet<WorldTile> worldTileSet;
    if (!this._tile_types.TryGetValue(pType, out worldTileSet))
    {
      worldTileSet = UnsafeCollectionPool<HashSet<WorldTile>, WorldTile>.Get();
      this._tile_types.Add(pType, worldTileSet);
    }
    worldTileSet.Add(pTile);
    this.chunk.setTileTypesDirty();
  }

  public void removeTileType(TileTypeBase pType, WorldTile pTile)
  {
    HashSet<WorldTile> worldTileSet;
    if (!this._tile_types.TryGetValue(pType, out worldTileSet))
      return;
    worldTileSet.Remove(pTile);
    this.chunk.setTileTypesDirty();
  }

  public HashSet<WorldTile> getTilesOfType(TileTypeBase pType)
  {
    HashSet<WorldTile> worldTileSet;
    return this._tile_types.TryGetValue(pType, out worldTileSet) ? worldTileSet : (HashSet<WorldTile>) null;
  }

  public bool hasTilesOfType(TileTypeBase pType)
  {
    HashSet<WorldTile> worldTileSet;
    return this._tile_types.TryGetValue(pType, out worldTileSet) && worldTileSet.Count > 0;
  }

  internal void addTile(WorldTile pTile, int pX, int pY)
  {
    this.tiles[this.tiles.FreeIndex<WorldTile>()] = pTile;
    switch (pX)
    {
      case 0:
        pTile.world_tile_zone_border.border_left = true;
        pTile.world_tile_zone_border.border = true;
        break;
      case 7:
        pTile.world_tile_zone_border.border_right = true;
        pTile.world_tile_zone_border.border = true;
        break;
    }
    if (pY == 0)
    {
      pTile.world_tile_zone_border.border_down = true;
      pTile.world_tile_zone_border.border = true;
    }
    else
    {
      if (pY != 7)
        return;
      pTile.world_tile_zone_border.border = true;
      pTile.world_tile_zone_border.border_up = true;
    }
  }

  internal void setCity(City pCity)
  {
    this.setDirty(true);
    if (this.hasCity() && this.city != pCity)
      World.world.cities.setDirtyBuildings(this.city);
    this.city = pCity;
  }

  public bool isDirty() => this._dirty;

  public void setDirty(bool pValue)
  {
    this._dirty = pValue;
    if (pValue)
      BuildingZonesSystem.setDirty();
    if (!this.hasCity())
      return;
    World.world.cities.setDirtyBuildings(this.city);
    this.city.setStatusDirty();
  }

  public void addBuildingMain(Building pBuilding)
  {
    this.buildings_all.Add(pBuilding);
    this.setDirty(true);
  }

  public void removeBuildingMain(Building pBuilding)
  {
    this.buildings_all.Remove(pBuilding);
    this.setDirty(true);
  }

  internal void addBuildingToSet(Building pBuilding)
  {
    HashSet<Building> buildingSet;
    if (pBuilding.isRuin())
      buildingSet = this.getHashsetOrCreate(BuildingList.Ruins);
    else if (pBuilding.asset.city_building)
    {
      buildingSet = !pBuilding.isAbandoned() ? this.getHashsetOrCreate(BuildingList.Civs) : this.getHashsetOrCreate(BuildingList.Abandoned);
    }
    else
    {
      switch (pBuilding.asset.building_type)
      {
        case BuildingType.Building_Tree:
          buildingSet = this.getHashsetOrCreate(BuildingList.Trees);
          break;
        case BuildingType.Building_Fruits:
          buildingSet = this.getHashsetOrCreate(BuildingList.Food);
          break;
        case BuildingType.Building_Hives:
          buildingSet = this.getHashsetOrCreate(BuildingList.Hives);
          break;
        case BuildingType.Building_Poops:
          buildingSet = this.getHashsetOrCreate(BuildingList.Poops);
          break;
        case BuildingType.Building_Wheat:
          buildingSet = this.getHashsetOrCreate(BuildingList.Wheat);
          break;
        case BuildingType.Building_Plant:
          buildingSet = this.getHashsetOrCreate(BuildingList.Flora);
          break;
        case BuildingType.Building_Mineral:
          buildingSet = this.getHashsetOrCreate(BuildingList.Minerals);
          break;
        default:
          buildingSet = this.getHashsetOrCreate(BuildingList.Civs);
          break;
      }
    }
    buildingSet.Add(pBuilding);
  }

  internal void addNeighbour(
    TileZone pNeighbour,
    TileDirection pDirection,
    IList<TileZone> pNeighbours,
    IList<TileZone> pNeighboursAll,
    bool pDiagonal = false)
  {
    if (pNeighbour == null)
    {
      this.world_edge = true;
    }
    else
    {
      if (!pDiagonal)
        pNeighbours.Add(pNeighbour);
      pNeighboursAll.Add(pNeighbour);
      switch (pDirection)
      {
        case TileDirection.Left:
          this.zone_left = pNeighbour;
          break;
        case TileDirection.Right:
          this.zone_right = pNeighbour;
          break;
        case TileDirection.Up:
          this.zone_up = pNeighbour;
          break;
        case TileDirection.Down:
          this.zone_down = pNeighbour;
          break;
      }
    }
  }

  public bool canStartCityHere()
  {
    return !this.hasCity() && this.tiles_with_ground >= 64 /*0x40*/ && !this.hasTilesOfType((TileTypeBase) TileLibrary.hills);
  }

  public bool hasLava()
  {
    foreach (TileTypeBase lavaType in TileLibrary.lava_types)
    {
      if (this.hasTilesOfType(lavaType))
        return true;
    }
    return false;
  }

  public int countLava()
  {
    int num = 0;
    foreach (TileTypeBase lavaType in TileLibrary.lava_types)
    {
      HashSet<WorldTile> tilesOfType = this.getTilesOfType(lavaType);
      if (tilesOfType != null)
        num += tilesOfType.Count;
    }
    return num;
  }

  public IEnumerable<WorldTile> loopLava()
  {
    foreach (TileTypeBase pType in TileLibrary.lava_types.LoopRandom<TileType>())
    {
      HashSet<WorldTile> tilesOfType = this.getTilesOfType(pType);
      if (tilesOfType != null)
      {
        foreach (WorldTile worldTile in tilesOfType)
          yield return worldTile;
      }
    }
  }

  public bool goodForExpansion() => !this.hasLava();

  public bool hasReachedBuildingLimit(WorldTile pTile, BuildingAsset pAsset)
  {
    int num = pAsset.limit_per_zone;
    if (pTile.isTileRank(TileRank.Low) && pAsset.building_type == BuildingType.Building_Tree)
    {
      num = (int) ((double) num * 0.5);
      if (num == 0)
        num = 1;
    }
    if (WorldLawLibrary.world_law_spread_density_high.isEnabled())
      num = (int) ((double) num * 1.5);
    if (pAsset.limit_in_radius > 0)
    {
      foreach (Building buildingFromZone in World.world.buildings.getBuildingFromZones(pTile, (float) pAsset.limit_in_radius))
      {
        if (buildingFromZone.asset == pAsset)
          return true;
      }
    }
    BuildingList? nullable = new BuildingList?();
    switch (pAsset.building_type)
    {
      case BuildingType.Building_Tree:
        nullable = new BuildingList?(BuildingList.Trees);
        break;
      case BuildingType.Building_Fruits:
        nullable = new BuildingList?(BuildingList.Food);
        break;
      case BuildingType.Building_Hives:
        nullable = new BuildingList?(BuildingList.Hives);
        break;
      case BuildingType.Building_Poops:
        nullable = new BuildingList?(BuildingList.Poops);
        break;
      case BuildingType.Building_Plant:
        nullable = new BuildingList?(BuildingList.Flora);
        break;
      case BuildingType.Building_Mineral:
        nullable = new BuildingList?(BuildingList.Minerals);
        break;
    }
    return nullable.HasValue && this.countBuildingsType(nullable.Value) >= num;
  }

  public void clearBuildingLists()
  {
    this.buildings_render_list.Clear();
    for (int index = 0; index < this._building_hashset_types_array.Length; ++index)
    {
      HashSet<Building> buildingHashsetTypes = this._building_hashset_types_array[index];
      if (buildingHashsetTypes != null)
      {
        buildingHashsetTypes.Clear();
        UnsafeCollectionPool<HashSet<Building>, Building>.Release(buildingHashsetTypes);
        this._building_hashset_types_array[index] = (HashSet<Building>) null;
      }
    }
  }

  public Dictionary<TileTypeBase, HashSet<WorldTile>> getTileTypes() => this._tile_types;

  public void clear()
  {
    foreach (HashSet<WorldTile> worldTileSet in this._tile_types.Values)
      worldTileSet.Clear();
    this.clearBuildingLists();
    this.buildings_all.Clear();
    this.city = (City) null;
    this._good_for_new_city = false;
    this.resetRenderHelpers();
  }

  public void Dispose()
  {
    this.clear();
    this.tiles.Clear<WorldTile>();
    this.neighbours.Clear<TileZone>();
    this.neighbours_all.Clear<TileZone>();
    this.neighbours = (TileZone[]) null;
    this.neighbours_all = (TileZone[]) null;
    this.zone_up = (TileZone) null;
    this.zone_down = (TileZone) null;
    this.zone_left = (TileZone) null;
    this.zone_right = (TileZone) null;
    this.centerTile = (WorldTile) null;
    foreach (HashSet<WorldTile> worldTileSet in this._tile_types.Values)
    {
      worldTileSet.Clear();
      UnsafeCollectionPool<HashSet<WorldTile>, WorldTile>.Release(worldTileSet);
    }
    this._tile_types.Clear();
  }

  public bool canBeClaimedByCity(City pCity)
  {
    return (!pCity.hasLeader() || this.checkCanSettleInThisBiomes(pCity.leader.subspecies)) && (!this.hasCity() || !this.isSameCityHere(pCity) && WorldLawLibrary.world_law_border_stealing.isEnabled() && (!pCity.hasKingdom() || !this.city.hasKingdom() || pCity.kingdom.isEnemy(this.city.kingdom)) && pCity.kingdom != this.city.kingdom);
  }

  public bool isZoneOnFire() => WorldBehaviourActionFire.countFires(this) > 0;

  public WorldTile getRandomTile() => this.tiles.GetRandom<WorldTile>();

  public MapChunk chunk => this.centerTile.chunk;

  public int countNotNullTypes()
  {
    int num = 0;
    for (int index = 0; index < this._building_hashset_types_array.Length; ++index)
    {
      if (this._building_hashset_types_array[index] != null)
        ++num;
    }
    return num;
  }

  public IMetaObject getClanOnZone(int pZoneOption)
  {
    IMetaObject clanOnZone;
    switch (pZoneOption)
    {
      case 0:
        City city1 = this.city;
        if (city1.isRekt())
          return (IMetaObject) null;
        clanOnZone = (IMetaObject) city1.kingdom.getKingClan();
        break;
      case 1:
        City city2 = this.city;
        if (city2.isRekt())
          return (IMetaObject) null;
        clanOnZone = (IMetaObject) city2.getRoyalClan();
        break;
      default:
        clanOnZone = this.getDefaultMetaOnZone();
        break;
    }
    return clanOnZone;
  }

  public IMetaObject getFamilyOnZone(int pZoneOption)
  {
    IMetaObject familyOnZone;
    switch (pZoneOption)
    {
      case 0:
        City city1 = this.city;
        if (city1.isRekt())
          return (IMetaObject) null;
        familyOnZone = (IMetaObject) city1.kingdom.king?.family;
        break;
      case 1:
        City city2 = this.city;
        if (city2.isRekt())
          return (IMetaObject) null;
        familyOnZone = (IMetaObject) city2.leader?.family;
        break;
      default:
        familyOnZone = this.getDefaultMetaOnZone();
        break;
    }
    return familyOnZone;
  }

  public IMetaObject getArmyOnZone(int pZoneOption) => this.getDefaultMetaOnZone();

  public IMetaObject getCityOnZone(int pZoneOption)
  {
    IMetaObject cityOnZone;
    if (pZoneOption == 0)
    {
      City city = this.city;
      if (city.isRekt())
        return (IMetaObject) null;
      cityOnZone = (IMetaObject) city;
    }
    else
      cityOnZone = this.getDefaultMetaOnZone();
    return cityOnZone;
  }

  public IMetaObject getKingdomOnZone(int pZoneOption)
  {
    IMetaObject kingdomOnZone;
    if (pZoneOption == 0)
    {
      City city = this.city;
      if (city.isRekt())
        return (IMetaObject) null;
      kingdomOnZone = (IMetaObject) city.kingdom;
    }
    else
      kingdomOnZone = this.getDefaultMetaOnZone();
    return kingdomOnZone;
  }

  public IMetaObject getLanguageOnZone(int pZoneOption)
  {
    IMetaObject languageOnZone;
    switch (pZoneOption)
    {
      case 0:
        City city1 = this.city;
        if (city1.isRekt())
          return (IMetaObject) null;
        languageOnZone = (IMetaObject) city1.kingdom.getLanguage();
        break;
      case 1:
        City city2 = this.city;
        if (city2.isRekt())
          return (IMetaObject) null;
        languageOnZone = (IMetaObject) city2.getLanguage();
        break;
      default:
        languageOnZone = this.getDefaultMetaOnZone();
        break;
    }
    return languageOnZone;
  }

  public IMetaObject getReligionOnZone(int pZoneOption)
  {
    IMetaObject religionOnZone;
    switch (pZoneOption)
    {
      case 0:
        City city1 = this.city;
        if (city1.isRekt())
          return (IMetaObject) null;
        religionOnZone = (IMetaObject) city1.kingdom.getReligion();
        break;
      case 1:
        City city2 = this.city;
        if (city2.isRekt())
          return (IMetaObject) null;
        religionOnZone = (IMetaObject) city2.getReligion();
        break;
      default:
        religionOnZone = this.getDefaultMetaOnZone();
        break;
    }
    return religionOnZone;
  }

  public IMetaObject getSubspeciesOnZone(int pZoneOption)
  {
    IMetaObject subspeciesOnZone;
    switch (pZoneOption)
    {
      case 0:
        City city1 = this.city;
        if (city1.isRekt())
          return (IMetaObject) null;
        subspeciesOnZone = (IMetaObject) city1.kingdom.getMainSubspecies();
        break;
      case 1:
        City city2 = this.city;
        if (city2.isRekt())
          return (IMetaObject) null;
        subspeciesOnZone = (IMetaObject) city2.getMainSubspecies();
        break;
      default:
        subspeciesOnZone = this.getDefaultMetaOnZone();
        break;
    }
    return subspeciesOnZone;
  }

  public IMetaObject getCultureOnZone(int pZoneOption)
  {
    IMetaObject cultureOnZone;
    switch (pZoneOption)
    {
      case 0:
        City city1 = this.city;
        if (city1.isRekt())
          return (IMetaObject) null;
        cultureOnZone = (IMetaObject) city1.kingdom.getCulture();
        break;
      case 1:
        City city2 = this.city;
        if (city2.isRekt())
          return (IMetaObject) null;
        cultureOnZone = (IMetaObject) city2.getCulture();
        break;
      default:
        cultureOnZone = this.getDefaultMetaOnZone();
        break;
    }
    return cultureOnZone;
  }

  public Alliance getAllianceOnZone(int pZoneOption) => this.city?.getAlliance();

  private IMetaObject getDefaultMetaOnZone()
  {
    ZoneMetaData zoneMetaData = ZoneMetaDataVisualizer.getZoneMetaData(this);
    if (zoneMetaData.meta_object == null)
      return (IMetaObject) null;
    return !zoneMetaData.meta_object.isAlive() ? (IMetaObject) null : zoneMetaData.meta_object;
  }
}
