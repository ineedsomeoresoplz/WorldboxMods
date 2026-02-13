// Decompiled with JetBrains decompiler
// Type: BatchBuildings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Unity.Mathematics;

#nullable disable
public class BatchBuildings : Batch<Building>
{
  public ObjectContainer<Building> c_main;
  public ObjectContainer<Building> c_scale;
  public ObjectContainer<Building> c_angle;
  public ObjectContainer<Building> c_components;
  public ObjectContainer<Building> c_spread_trees;
  public ObjectContainer<Building> c_spread_plants;
  public ObjectContainer<Building> c_spread_fungi;
  public ObjectContainer<Building> c_poop;
  public ObjectContainer<Building> c_resource_shaker;
  public ObjectContainer<Building> c_shake;
  public ObjectContainer<Building> c_position_dirty;
  public ObjectContainer<Building> c_tiles_dirty;
  public ObjectContainer<Building> c_stats_dirty;
  public ObjectContainer<Building> c_auto_remove;
  public Random rnd = new Random(10U);
  private float _timer_spread_trees;
  private float _timer_spread_plants;
  private float _timer_poop_flora;
  private float _timer_spread_fungi;
  public List<Action> actions_to_run = new List<Action>();

  protected override void createJobs()
  {
    this.addJob((ObjectContainer<Building>) null, new JobUpdater(((Batch<Building>) this).prepare), JobType.Parallel, "prepare");
    this.createJob(out this.c_scale, new JobUpdater(this.updateScale), JobType.Parallel, "update_scale");
    this.createJob(out this.c_angle, new JobUpdater(this.updateAngle), JobType.Parallel, "update_angle");
    this.createJob(out this.c_resource_shaker, new JobUpdater(this.updateResourceShaker), JobType.Parallel, "update_resource_shaker");
    this.createJob(out this.c_stats_dirty, new JobUpdater(this.updateStatsDirty), JobType.Parallel, "update_dirty_stats");
    this.createJob(out this.c_shake, new JobUpdater(this.updateShake), JobType.Parallel, "update_shake");
    this.createJob(out this.c_main, new JobUpdater(this.updateVisibility), JobType.Parallel, "update_visibility");
    this.createJob(out this.c_tiles_dirty, new JobUpdater(this.updateTilesDirty), JobType.Post, "update_dirty_tiles");
    this.createJob(out this.c_auto_remove, new JobUpdater(this.updateAutoRemove), JobType.Post, "update_auto_remove");
    this.createJob(out this.c_components, new JobUpdater(this.updateComponents), JobType.Post, "update_components");
    this.createJob(out this.c_spread_trees, new JobUpdater(this.updateSpreadTrees), JobType.Post, "update_spread_trees");
    this.createJob(out this.c_spread_plants, new JobUpdater(this.updateSpreadPlants), JobType.Post, "update_spread_plants");
    this.createJob(out this.c_spread_fungi, new JobUpdater(this.updateSpreadFungi), JobType.Post, "update_spread_fungi");
    this.createJob(out this.c_poop, new JobUpdater(this.updatePoopTurningIntoFlora), JobType.Post, "update_poop_turning_into_flora");
    this.createJob(out this.c_position_dirty, new JobUpdater(this.updatePositionsDirty), JobType.Post, "update_dirty_positions");
    this.main = this.c_main;
    this.applyParallelResults = this.applyParallelResults + new JobUpdater(this.applyTweenActions);
  }

  public void applyTweenActions()
  {
    if (this.actions_to_run.Count == 0)
      return;
    for (int index = 0; index < this.actions_to_run.Count; ++index)
      this.actions_to_run[index]();
    this.actions_to_run.Clear();
  }

  internal override void clear()
  {
    base.clear();
    JobUpdater clearParallelResults = this.clearParallelResults;
    if (clearParallelResults != null)
      clearParallelResults();
    this.actions_to_run.Clear();
  }

  private void updateScale()
  {
    if (!this.check(this._cur_container))
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateScale();
  }

  private void updateAngle()
  {
    if (!this.check(this._cur_container))
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateAngle(this._elapsed);
  }

  private void updateVisibility()
  {
    if (!this.check(this._cur_container))
      return;
    bool flag1 = MapBox.isRenderGameplay();
    bool flag2 = World.world.quality_changer.shouldRenderBuildings();
    if (!DebugConfig.isOn(DebugOption.ScaleEffectEnabled) && flag2 && !flag1)
      flag2 = false;
    Building[] array = this._array;
    int count = this._count;
    if (flag1)
    {
      for (int index = 0; index < count; ++index)
      {
        Building building = array[index];
        building.is_visible = building.current_tile.zone.visible;
      }
    }
    else
    {
      for (int index = 0; index < count; ++index)
        array[index].is_visible = flag2;
    }
  }

  private void updateTilesDirty()
  {
    if (!this.check(this._cur_container))
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].checkDirtyTiles();
  }

  private void updateAutoRemove()
  {
    if (!this.check(this._cur_container))
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateAutoRemove(this._elapsed);
  }

  private void updateStatsDirty()
  {
    if (!this.check(this._cur_container))
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateStats();
  }

  private void updateComponents()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Building building = array[index];
      if (building.isUsable())
        building.updateComponents(this._elapsed);
    }
  }

  private void updateSpreadTrees()
  {
    if (!this.check(this._cur_container) || World.world.isPaused() || !WorldLawLibrary.world_law_spread_trees.isEnabled())
      return;
    if ((double) this._timer_spread_trees >= 0.0)
    {
      this._timer_spread_trees -= this._elapsed;
      if ((double) this._timer_spread_trees > 0.0)
        return;
      this._timer_spread_trees = WorldLawLibrary.getIntervalSpreadTrees();
    }
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Building building = array[index];
      if (building.isUsable())
        building.checkVegetationSpread(this._elapsed);
    }
  }

  private void updateSpreadPlants()
  {
    if (!this.check(this._cur_container) || World.world.isPaused() || !WorldLawLibrary.world_law_spread_plants.isEnabled())
      return;
    if ((double) this._timer_spread_plants >= 0.0)
    {
      this._timer_spread_plants -= this._elapsed;
      if ((double) this._timer_spread_plants > 0.0)
        return;
      this._timer_spread_plants = WorldLawLibrary.getIntervalSpreadPlants();
    }
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Building building = array[index];
      if (building.isUsable())
        building.checkVegetationSpread(this._elapsed);
    }
  }

  private void updatePoopTurningIntoFlora()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    if ((double) this._timer_poop_flora >= 0.0)
    {
      this._timer_poop_flora -= this._elapsed;
      if ((double) this._timer_poop_flora > 0.0)
        return;
      this._timer_poop_flora = 5f;
    }
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Building building = array[index];
      if (building.isUsable() && (double) building.getExistenceMonths() >= (double) SimGlobals.m.months_till_pool_turns_into_flora && !Randy.randomChance(0.7f))
      {
        WorldTile currentTile = building.current_tile;
        BiomeAsset biomeAsset = currentTile.Type.biome_asset;
        if (biomeAsset != null && biomeAsset.grow_type_selector_plants != null)
        {
          building.startDestroyBuilding();
          BuildingActions.tryGrowVegetationRandom(currentTile, VegetationType.Plants, pCheckLimit: false, pCheckRandom: false);
        }
      }
    }
  }

  private void updateSpreadFungi()
  {
    if (!this.check(this._cur_container) || World.world.isPaused() || !WorldLawLibrary.world_law_spread_fungi.isEnabled())
      return;
    if ((double) this._timer_spread_fungi >= 0.0)
    {
      this._timer_spread_fungi -= this._elapsed;
      if ((double) this._timer_spread_fungi > 0.0)
        return;
      this._timer_spread_fungi = WorldLawLibrary.getIntervalSpreadFungi();
    }
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Building building = array[index];
      if (building.isUsable())
        building.checkVegetationSpread(this._elapsed);
    }
  }

  private void updateResourceShaker()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Building building = array[index];
      if (building.isUsable())
        building.updateTimerShakeResources(this._elapsed);
    }
  }

  private void updateShake()
  {
    if (!this.check(this._cur_container))
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateShake(this._elapsed);
  }

  private void updatePositionsDirty()
  {
    if (!this.check(this._cur_container))
      return;
    Building[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updatePosition();
  }

  internal override void add(Building pBuilding)
  {
    base.add(pBuilding);
    pBuilding.batch = this;
  }

  internal override void remove(Building pObject)
  {
    base.remove(pObject);
    pObject.batch = (BatchBuildings) null;
  }
}
