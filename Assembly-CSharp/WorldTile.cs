// Decompiled with JetBrains decompiler
// Type: WorldTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityPools;

#nullable disable
public class WorldTile : IEquatable<WorldTile>, IDisposable
{
  [CanBeNull]
  public TopTileType top_type;
  [CanBeNull]
  public TileType main_type;
  private TileTypeBase cur_tile_type;
  public bool obstacle_is_around;
  internal TileBase current_rendered_tile_graphics;
  public int burned_stages;
  internal WorldTileZoneBorder world_tile_zone_border = new WorldTileZoneBorder();
  public const int DEFAULT_HEALTH = 10;
  public int health = 10;
  public Vector3Int last_rendered_border_pos_ocean;
  public Vector3Int last_rendered_pos_tile;
  public TileTypeBase last_rendered_tile_type;
  public float delayed_timer_bomb;
  public string delayed_bomb_type = "";
  public double timestamp_type_changed;
  public WorldTileData data;
  public int heat;
  internal int explosion_wave;
  internal int explosion_power;
  private Actor _targeted_by;
  public bool world_edge;
  public WorldTile tile_up;
  public WorldTile tile_down;
  public WorldTile tile_left;
  public WorldTile tile_right;
  public WorldTile[] neighbours;
  public WorldTile[] neighboursAll;
  public TileIsland road_island;
  public int pollinated;
  public readonly int x;
  public readonly int y;
  public readonly Vector2Int pos;
  public readonly Vector3 posV3;
  public readonly Vector3 posV;
  internal int minimap_building_x;
  internal int minimap_building_y;
  internal int flash_state;
  internal ColorArray color_array;
  public MapRegion region;
  public TileZone zone;
  public MapChunk chunk;
  public Building building;
  private List<Actor> _units;
  internal int explosion_fx_stage;
  internal bool is_checked_tile;
  internal int score = -1;
  public bool wall_check_dirty;
  private bool _has_walls_around;

  public WorldTile(int pX, int pY, int pTileID)
  {
    this.last_rendered_pos_tile = WorldTilemap.EMPTY_TILE_POS;
    this._units = UnsafeCollectionPool<List<Actor>, Actor>.Get();
    this.data = new WorldTileData(pTileID);
    this.pos = new Vector2Int(pX, pY);
    this.posV3 = new Vector3((float) pX, (float) pY);
    this.posV = new Vector3((float) pX, (float) pY);
    this.posV3.x += Actor.sprite_offset.x;
    this.posV3.y += Actor.sprite_offset.y;
    this.x = pX;
    this.y = pY;
  }

  public bool hasWallsAround()
  {
    if (this.wall_check_dirty)
    {
      this.wall_check_dirty = false;
      this._has_walls_around = false;
      int index = 0;
      for (int length = this.neighboursAll.Length; index < length; ++index)
      {
        if (this.neighboursAll[index].Type.wall)
        {
          this._has_walls_around = true;
          break;
        }
      }
    }
    return this._has_walls_around;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isTargeted() => this._targeted_by != null;

  public bool isTargetedBy(Actor pActor) => this._targeted_by == pActor;

  public void cleanTargetedBy() => this._targeted_by = (Actor) null;

  public void setTargetedBy(Actor pActor) => this._targeted_by = pActor;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void doUnits(Action<Actor> pAction)
  {
    List<Actor> units = this._units;
    if (units.Count == 0)
      return;
    for (int index = 0; index < units.Count; ++index)
    {
      Actor actor = units[index];
      if (actor.isAlive())
        pAction(actor);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void doUnits(Func<Actor, bool> pAction)
  {
    List<Actor> units = this._units;
    if (units.Count == 0)
      return;
    for (int index = 0; index < units.Count; ++index)
    {
      Actor actor = units[index];
      if (actor.isAlive() && !pAction(actor))
        break;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int countUnits() => this._units.Count;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasUnits() => this._units.Count > 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void addUnit(Actor pActor) => this._units.Add(pActor);

  public void resetNeighbourLists()
  {
    this.neighbours = (WorldTile[]) null;
    this.neighboursAll = (WorldTile[]) null;
  }

  public void pollinate()
  {
    ++this.pollinated;
    if (this.pollinated <= 5)
      return;
    this.growFlowers();
    this.pollinated = 0;
  }

  private void growFlowers()
  {
    WorldTile random = Toolbox.getRandomChunkFromTile(this).tiles.GetRandom<WorldTile>();
    BiomeAsset biomeAsset = random.Type.biome_asset;
    if (biomeAsset == null || biomeAsset.grow_type_selector_plants == null)
      return;
    BuildingActions.tryGrowVegetationRandom(random, VegetationType.Plants);
  }

  public bool canBuildOn(BuildingAsset pNewTemplate)
  {
    if (pNewTemplate.needs_farms_ground && !this.main_type.can_be_farm || this.Type.liquid && !pNewTemplate.can_be_placed_on_liquid || pNewTemplate.burnable && this.isOnFire() || pNewTemplate.affected_by_lava && this.Type.lava || !pNewTemplate.can_be_placed_on_blocks && this.Type.block || this.building != null && !this.building.isUsable() && !this.building.asset.flora && !pNewTemplate.remove_ruins || this.building != null && this.building.isUsable() && pNewTemplate.ignore_same_building_id && this.building.asset == pNewTemplate || !pNewTemplate.ignore_buildings && this.building != null && this.building.isUsable() && !this.building.asset.ignored_by_cities)
      return false;
    if (pNewTemplate.remove_buildings_when_dropped && this.building != null)
    {
      if (!this.building.isUsable() && pNewTemplate.remove_ruins)
        return true;
      if (!pNewTemplate.remove_civ_buildings && this.building.asset.city_building)
        return false;
    }
    return pNewTemplate.ignore_buildings || this.building == null || !this.building.asset.city_building || !this.building.isUsable() || this.building.asset.priority < pNewTemplate.priority;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasBuilding() => this.building != null;

  public void setRoad() => World.world.roads_calculator.setDirty(this);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isSameIsland(WorldTile pTile) => pTile.region.island == this.region.island;

  public Color32 getColor() => this.Type.color;

  internal void addNeighbour(
    WorldTile pNeighbour,
    TileDirection pDirection,
    List<WorldTile> pNeighbours,
    List<WorldTile> pNeighboursAll,
    bool pDiagonal = false)
  {
    if (pNeighbour == null)
    {
      this.world_edge = true;
    }
    else
    {
      pNeighboursAll.Add(pNeighbour);
      if (pDiagonal)
        return;
      pNeighbours.Add(pNeighbour);
      switch (pDirection)
      {
        case TileDirection.Left:
          this.tile_left = pNeighbour;
          break;
        case TileDirection.Right:
          this.tile_right = pNeighbour;
          break;
        case TileDirection.Up:
          this.tile_up = pNeighbour;
          break;
        case TileDirection.Down:
          this.tile_down = pNeighbour;
          break;
      }
    }
  }

  public BiomeAsset getBiome() => this.Type.is_biome ? this.Type.biome_asset : (BiomeAsset) null;

  public bool is_liquid => this.Type.liquid;

  public TileTypeBase Type
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.cur_tile_type;
  }

  public int Height
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.data.height;
    set
    {
      this.data.height = value;
      if (this.data.height < 0)
      {
        this.data.height = 0;
      }
      else
      {
        if (this.data.height <= (int) byte.MaxValue)
          return;
        this.data.height = (int) byte.MaxValue;
      }
    }
  }

  internal bool IsOceanAround()
  {
    for (int index = 0; index < this.neighbours.Length; ++index)
    {
      if (this.neighbours[index].Type.layer_type == TileLayerType.Ocean)
        return true;
    }
    return false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isGoodForBoat() => this.Type.layer_type == TileLayerType.Ocean;

  internal bool IsTypeAround(TileTypeBase pType)
  {
    for (int index = 0; index < this.neighbours.Length; ++index)
    {
      if (this.neighbours[index].Type == pType)
        return true;
    }
    return false;
  }

  internal bool startFire(bool pForce = false)
  {
    if (this.Type.explodable)
      World.world.explosion_layer.explodeBomb(this);
    if ((pForce ? 1 : (!this.Type.burnable ? 0 : (!this.isOnFire() ? 1 : 0))) == 0 || this.Type.liquid)
      return false;
    this.unfreeze(99);
    bool flag = false;
    if (this.building != null && this.building.isBurnable())
    {
      ActionLibrary.addBurningEffectOnTarget((BaseSimObject) null, (BaseSimObject) this.building);
      flag = true;
    }
    if (this.Type.burnable | flag | pForce)
    {
      flag = true;
      if (this.Type.IsType("fireworks"))
        EffectsLibrary.spawn("fx_fireworks", this);
      this.data.fire_timestamp = World.world.getCurWorldTime();
      if (this.Type.burnable)
      {
        this.health -= this.Type.burn_rate;
        this.setBurned();
        World.world.flash_effects.flashPixel(this, 10);
        if (this.health <= 0)
          MapAction.decreaseTile(this, true);
      }
      this.setFireData(true);
    }
    return flag;
  }

  public void setFireData(bool pVal)
  {
    World.world.tile_manager.fires[this.data.tile_id] = pVal;
    if (this.isOnFire())
      WorldBehaviourActionFire.addFire(this);
    else
      WorldBehaviourActionFire.removeFire(this);
  }

  public void updateStats()
  {
    this.cur_tile_type = this.top_type == null ? (TileTypeBase) this.main_type : (TileTypeBase) this.top_type;
    foreach (WorldTile worldTile in this.neighboursAll)
      worldTile.wall_check_dirty = true;
    if (!this.isTemporaryFrozen())
      return;
    if (!this.cur_tile_type.can_be_frozen)
    {
      this.data.frozen = false;
    }
    else
    {
      TopTileType topTileType = AssetManager.top_tiles.get(this.main_type.freeze_to_id);
      if (topTileType == null)
      {
        if (!this.main_type.can_be_frozen && this.cur_tile_type.can_be_frozen)
          Debug.LogError((object) "TILE SETTINGS CONFILICT! SET TOP TILE TO can_be_frozen FALSE!");
        Debug.LogError((object) $"TILE 1 f:{this.cur_tile_type.freeze_to_id} m: {this.cur_tile_type.id}");
        Debug.LogError((object) $"TILE 2 f:{this.main_type.freeze_to_id} m: {this.main_type.id}");
      }
      else
        this.cur_tile_type = (TileTypeBase) topTileType;
    }
  }

  public void setTopTileType(TopTileType pTopTile, bool pUpdateStats = true)
  {
    if (this.top_type != pTopTile)
    {
      if (this.top_type != null)
        this.zone.removeTileType((TileTypeBase) this.top_type, this);
      if (pTopTile != null)
        this.zone.addTileType((TileTypeBase) pTopTile, this);
    }
    if (this.top_type != null)
      this.top_type.hashsetRemove(this);
    this.top_type = pTopTile;
    if (this.top_type != null)
      this.top_type.hashsetAdd(this);
    if (!pUpdateStats)
      return;
    World.world.setTileDirty(this);
    this.updateStats();
  }

  public void setTileTypes(TileType pType, TopTileType pTopTile, bool pSetDirty = true)
  {
    this.setTopTileType(pTopTile, false);
    this.setTileType(pType, pSetDirty);
  }

  public void setTileTypes(string pType, TopTileType pTopTile)
  {
    this.setTopTileType(pTopTile, false);
    this.setTileType(pType);
  }

  public void setTileType(TileType pType, bool pSetDirty = true)
  {
    this.health = 10;
    if (this.zone != null)
    {
      if (this.main_type != pType)
      {
        if (this.main_type != null)
          this.zone.removeTileType((TileTypeBase) this.main_type, this);
        this.zone.addTileType((TileTypeBase) pType, this);
      }
      if (this.main_type == null)
      {
        if (pType.liquid)
          ++this.zone.tiles_with_liquid;
        if (pType.ground)
          ++this.zone.tiles_with_ground;
      }
      else
      {
        if (!this.main_type.liquid && pType.liquid)
          ++this.zone.tiles_with_liquid;
        else if (this.main_type.liquid && !pType.liquid)
          --this.zone.tiles_with_liquid;
        if (!this.main_type.ground && pType.ground)
          ++this.zone.tiles_with_ground;
        else if (this.main_type.ground && !pType.ground)
          --this.zone.tiles_with_ground;
      }
    }
    if (this.main_type != null)
      this.main_type.hashsetRemove(this);
    this.main_type = pType;
    this.main_type.hashsetAdd(this);
    this.updateStats();
    if (pSetDirty)
      World.world.setTileDirty(this);
    this.timestamp_type_changed = World.world.getCurWorldTime();
  }

  public void setTileType(string pType)
  {
    this.setTileType(AssetManager.tiles.get(pType) ?? TileLibrary.soil_low);
  }

  public void setBurned(int pForceVal = -1)
  {
    if (!this.Type.can_be_set_on_fire)
      return;
    if (pForceVal == -1)
      this.setBurnedStage(15 - Randy.randomInt(0, 10));
    else
      this.setBurnedStage(this.burned_stages);
    World.world.burned_layer.setTileDirty(this);
  }

  public void setBurnedStage(int pValue)
  {
    if (this.burned_stages == 0 && pValue == 0)
      return;
    this.burned_stages = pValue;
    WorldBehaviourActionBurnedTiles.addTile(this);
  }

  public void removeBurn()
  {
    if (this.burned_stages == 0)
      return;
    this.setBurnedStage(0);
    World.world.burned_layer.setTileDirty(this);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isOnFire() => World.world.tile_manager.fires[this.data.tile_id];

  internal void stopFire()
  {
    if (!this.isOnFire())
      return;
    this.setFireData(false);
    this.data.fire_timestamp = -1.0;
    this.setBurned();
  }

  internal bool canGrow() => !this.isOnFire() && this.burned_stages == 0;

  public void removeTrees(bool pFlash = true)
  {
    if (pFlash)
      World.world.flash_effects.flashPixel(this, 20);
    World.world.setTileDirty(this);
  }

  public void removeGrass(bool pFlash = true)
  {
    if (pFlash)
      World.world.flash_effects.flashPixel(this, 20);
    MapAction.removeGreens(this);
  }

  public void topTileEaten(int pTicks = 5) => this.removeGrass();

  public bool isTileRank(TileRank pRank) => this.main_type.rank_type == pRank;

  internal void clearUnits() => this._units.Clear();

  internal void clear()
  {
    this.last_rendered_tile_type = (TileTypeBase) null;
    this.health = 10;
    this.minimap_building_x = 0;
    this.minimap_building_y = 0;
    this.clearUnits();
    this.cleanTargetedBy();
    this.explosion_wave = 0;
    this.explosion_power = 0;
    this.pollinated = 0;
    this.setTileTypes(TileLibrary.deep_ocean, (TopTileType) null, false);
    this.delayed_timer_bomb = 0.0f;
    this.Height = 0;
    this.current_rendered_tile_graphics = (TileBase) null;
    this.heat = 0;
    this.flash_state = 0;
    this.burned_stages = 0;
    this.building = (Building) null;
    this.data.clear();
    this.explosion_fx_stage = 0;
    this.region = (MapRegion) null;
    this.last_rendered_pos_tile = WorldTilemap.EMPTY_TILE_POS;
    this.world_tile_zone_border.reset();
  }

  public void Dispose()
  {
    this.clear();
    this.wall_check_dirty = false;
    this._has_walls_around = false;
    if (this.main_type != null)
      this.main_type.hashsetRemove(this);
    this.main_type = (TileType) null;
    if (this.top_type != null)
      this.top_type.hashsetRemove(this);
    this.top_type = (TopTileType) null;
    this.cur_tile_type = (TileTypeBase) null;
    this.color_array = (ColorArray) null;
    this.tile_up = (WorldTile) null;
    this.tile_down = (WorldTile) null;
    this.tile_left = (WorldTile) null;
    this.tile_right = (WorldTile) null;
    this.neighbours = (WorldTile[]) null;
    this.neighboursAll = (WorldTile[]) null;
    this.road_island = (TileIsland) null;
    this.world_tile_zone_border = (WorldTileZoneBorder) null;
    this.region = (MapRegion) null;
    this.zone = (TileZone) null;
    this.chunk = (MapChunk) null;
    UnsafeCollectionPool<List<Actor>, Actor>.Release(this._units);
    this._units = (List<Actor>) null;
    this.data = (WorldTileData) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override int GetHashCode() => this.data.tile_id;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(WorldTile pTile) => this.data.tile_id == pTile.data.tile_id;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool reachableFrom(WorldTile pFromTile)
  {
    return this.isSameIsland(pFromTile) || this.region.island.reachableByCityFrom(pFromTile.region.island);
  }

  public bool freeze(int pDamage = 1)
  {
    if (!this.canBeFrozen() || this.building != null && this.building.isUsable() && this.building.asset.prevent_freeze)
      return false;
    this.data.frozen = true;
    if (this.Type.fast_freeze)
    {
      for (int index = 0; index < this.neighbours.Length; ++index)
      {
        WorldTile neighbour = this.neighbours[index];
        if (neighbour.Type.fast_freeze && neighbour.canBeFrozen() && Randy.randomChance(0.35f))
          neighbour.freeze(pDamage);
      }
    }
    this.health = 10;
    World.world.setTileDirty(this);
    if (this.zone.visible)
      World.world.flash_effects.flashPixel(this, 20);
    if (this.Type.chunk_dirty_when_temperature)
    {
      MapAction.checkTileState(this, (TileTypeBase) this.main_type, true);
      this.updateStats();
    }
    return true;
  }

  public void unfreeze(int pDamage = 1)
  {
    if (!this.canBeUnFrozen())
      return;
    if (this.health > 0)
    {
      this.health -= pDamage;
      if (this.health > 0)
        return;
    }
    this.data.frozen = false;
    this.health = 10;
    World.world.setTileDirty(this);
    if (this.zone.visible)
      World.world.flash_effects.flashPixel(this, 20);
    if (this.Type.chunk_dirty_when_temperature)
    {
      MapAction.checkTileState(this, (TileTypeBase) this.main_type, true);
      this.updateStats();
    }
    if (!this.Type.fast_freeze)
      return;
    for (int index = 0; index < this.neighbours.Length; ++index)
    {
      WorldTile neighbour = this.neighbours[index];
      if (neighbour.canBeUnFrozen() && Randy.randomChance(0.2f))
        neighbour.unfreeze(pDamage);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isSameLayer(WorldTile pTile1, WorldTile pTile2)
  {
    return pTile1.Type.layer_type == pTile2.Type.layer_type;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool canBeFrozen() => !this.isFrozen() && this.Type.can_be_frozen;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool canBeUnFrozen()
  {
    return this.data.frozen && this.Type.can_be_unfrozen && !this.Type.forever_frozen;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isTemporaryFrozen() => this.data.frozen;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isFrozen() => this.data.frozen || this.Type.forever_frozen;

  public TileRank getCreepTileRank() => this.main_type.creep_rank_type;

  public City zone_city => this.zone.city;

  public bool hasCity() => this.zone_city != null;

  public bool has_tile_up => this.tile_up != null;

  public bool has_tile_down => this.tile_down != null;

  public bool has_tile_left => this.tile_left != null;

  public bool has_tile_right => this.tile_right != null;

  public void tryToBreak()
  {
    this.health = 0;
    this.unfreeze(99);
  }

  public WorldTile getWalkableTileAround(WorldTile pFrom)
  {
    foreach (WorldTile walkableTileAround in this.neighboursAll.LoopRandom<WorldTile>())
    {
      if (walkableTileAround.isSameIsland(pFrom))
        return walkableTileAround;
    }
    return (WorldTile) null;
  }

  public IEnumerable<WorldTile> getTilesAround(int pRadius)
  {
    for (int iX = -pRadius; iX <= pRadius; ++iX)
    {
      for (int iY = -pRadius; iY <= pRadius; ++iY)
        yield return World.world.GetTile(this.x + iX, this.y + iY);
    }
  }

  public WorldTile getTileAroundThisOnSameIsland(WorldTile pTileFrom)
  {
    foreach (WorldTile thisOnSameIsland in this.neighboursAll.LoopRandom<WorldTile>())
    {
      if (thisOnSameIsland.isSameIsland(this))
        return thisOnSameIsland;
    }
    return (WorldTile) null;
  }

  public WorldTile getTileAroundThisOnSameIsland(WorldTile pTileFrom, bool pClosest)
  {
    if (!pClosest)
      return this.getTileAroundThisOnSameIsland(pTileFrom);
    int num1 = int.MaxValue;
    WorldTile thisOnSameIsland = (WorldTile) null;
    foreach (WorldTile pT2 in this.neighboursAll)
    {
      int num2 = Toolbox.SquaredDistTile(pTileFrom, pT2);
      if (num2 < num1 && pT2.isSameIsland(this))
      {
        num1 = num2;
        thisOnSameIsland = pT2;
      }
    }
    return thisOnSameIsland;
  }

  public bool isDiagonal(WorldTile pTile)
  {
    int num1 = Math.Abs(pTile.x - this.x);
    int num2 = Math.Abs(pTile.y - this.y);
    return num1 == 1 && num2 == 1;
  }

  public bool isSameCityHere(City pCity) => this.zone.isSameCityHere(pCity);

  public bool isWaterAround()
  {
    return !this.has_tile_down || !this.has_tile_up || !this.has_tile_left || !this.has_tile_right || this.tile_down.Type.liquid || this.tile_up.Type.liquid || this.tile_left.Type.liquid || this.tile_right.Type.liquid;
  }

  public int random_animation_seed => World.world.tile_manager.random_seeds[this.data.tile_id];

  public int tile_id => this.data.tile_id;

  public float distanceTo(WorldTile pTile) => Toolbox.DistTile(this, pTile);

  public WorldTile getNeighbourTileSameIsland()
  {
    foreach (WorldTile neighbourTileSameIsland in this.neighboursAll.LoopRandom<WorldTile>())
    {
      if (neighbourTileSameIsland.isSameIsland(this))
        return neighbourTileSameIsland;
    }
    return this;
  }
}
