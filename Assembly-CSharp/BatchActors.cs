// Decompiled with JetBrains decompiler
// Type: BatchActors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using Unity.Mathematics;

#nullable disable
public class BatchActors : Batch<Actor>
{
  public ObjectContainer<Actor> c_main;
  public ObjectContainer<Actor> c_check_attack_target;
  public ObjectContainer<Actor> c_update_movement;
  public ObjectContainer<Actor> c_main_tile_action;
  public ObjectContainer<Actor> c_shake;
  public ObjectContainer<Actor> c_stats_dirty;
  public ObjectContainer<Actor> c_action_landed;
  public ObjectContainer<Actor> c_make_decision;
  public ObjectContainer<Actor> c_sprite_animations;
  public ObjectContainer<Actor> c_update_children;
  public ObjectContainer<Actor> c_check_enemy_target;
  public ObjectContainer<Actor> c_augmentation_effects;
  public ObjectContainer<Actor> c_events_become_adult;
  public ObjectContainer<Actor> c_events_hatched;
  public ObjectContainer<Actor> c_hovering;
  public ObjectContainer<Actor> c_pollinating;
  public ObjectContainer<Actor> c_check_deaths;
  internal List<Actor> l_parallel_update_sprites = new List<Actor>();
  public Random rnd = new Random(10U);

  protected override void createJobs()
  {
    this.addJob((ObjectContainer<Actor>) null, new JobUpdater(((Batch<Actor>) this).prepare), JobType.Parallel, "prepare");
    this.createJob(out this.c_main, new JobUpdater(this.updateParallelChecks), JobType.Parallel, "update_timers");
    this.addJob(this.c_main, new JobUpdater(this.updateVisibility), JobType.Parallel, "update_visibility");
    this.createJob(out this.c_stats_dirty, new JobUpdater(this.updateStats), JobType.Parallel, "update_stats");
    this.createJob(out this.c_events_become_adult, new JobUpdater(this.updateEventsBecomeAdult), JobType.Post, "update_events_become_adult");
    this.createJob(out this.c_events_hatched, new JobUpdater(this.updateEventsEggHatched), JobType.Post, "update_events_hatched");
    this.createJob(out this.c_action_landed, new JobUpdater(this.updateActionLanded), JobType.Post, "update_action_landed");
    this.addJob(this.c_main, new JobUpdater(this.updateNutritionDecay), JobType.Post, "update_hunger");
    this.addJob(this.c_main, new JobUpdater(this.u1_checkInside), JobType.Post, "u1_checkInside");
    this.createJob(out this.c_update_children, new JobUpdater(this.u2_updateChildren), JobType.Post, "u2_updateChildren");
    this.createJob(out this.c_sprite_animations, new JobUpdater(this.u3_spriteAnimation), JobType.Post, "u3_spriteAnimation");
    this.addJob(this.c_main, new JobUpdater(this.u4_deadCheck), JobType.Post, "u4_deadCheck");
    this.createJob(out this.c_main_tile_action, new JobUpdater(this.u5_curTileAction), JobType.Post, "u5_curTileAction");
    this.addJob(this.c_main, new JobUpdater(this.u6_checkFrozen), JobType.Post, "u6_checkFrozen");
    this.createJob(out this.c_augmentation_effects, new JobUpdater(this.u7_checkAugmentationEffects), JobType.Post, "u7_checkAugmentationEffects", 20);
    this.addJob(this.c_main, new JobUpdater(this.u8_checkUpdateTimers), JobType.Post, "u8_checkUpdateTimers");
    this.addJob(this.c_main, new JobUpdater(this.b1_checkUnderForce), JobType.Post, "b1_checkUnderForce");
    this.createJob(out this.c_check_attack_target, new JobUpdater(this.b2_checkCurrentEnemyTarget), JobType.Post, "b2_checkCurrentEnemyTarget");
    this.addJob(this.c_main, new JobUpdater(this.b3_findEnemyTarget), JobType.Post, "b3_findEnemyTarget", 5);
    this.addJob(this.c_main, new JobUpdater(this.b4_checkTaskVerifier), JobType.Post, "b4_checkTaskVerifier");
    this.addJob(this.c_main, new JobUpdater(this.b5_checkPathMovement), JobType.Post, "b5_checkPathMovement");
    this.createJob(out this.c_make_decision, new JobUpdater(this.b6_0_updateDecision), JobType.Post, "b6_0_update_decision");
    this.addJob(this.c_main, new JobUpdater(this.b55_updateNaturalDeaths), JobType.Post, "b55_update_natural_death", 20);
    this.addJob(this.c_main, new JobUpdater(this.b6_updateAI), JobType.Post, "b6_update_ai");
    this.createJob(out this.c_update_movement, new JobUpdater(this.u10_checkSmoothMovement), JobType.Post, "u10_checkSmoothMovement");
    this.createJob(out this.c_shake, new JobUpdater(this.updateShake), JobType.Post, "update_shake");
    this.createJob(out this.c_hovering, new JobUpdater(this.updateHovering), JobType.Post, "update_hovering");
    this.createJob(out this.c_pollinating, new JobUpdater(this.updatePollinating), JobType.Post, "update_pollinating");
    this.createJob(out this.c_check_deaths, new JobUpdater(this.updateDeathCheck), JobType.Post, "update_death");
    this.main = this.c_main;
    this.clearParallelResults = this.clearParallelResults + new JobUpdater(this.clearParallelSprites);
  }

  private void updateParallelChecks()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateParallelChecks(this._elapsed);
  }

  private void updateVisibility()
  {
    if (!this.check(this._cur_container))
      return;
    bool flag = MapBox.isRenderGameplay();
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      if (actor.asset.has_sprite_renderer)
        actor.is_visible = !actor.isInMagnet() && !actor.isInsideSomething() && (!flag ? actor.asset.visible_on_minimap : actor.current_tile.zone.visible);
    }
  }

  private void updateNutritionDecay()
  {
    if ((double) World.world.timer_nutrition_decay > 0.0 || World.world.isPaused() || !this.check(this._cur_container))
      return;
    bool pDoStarvationDamage = false;
    if (WorldLawLibrary.world_law_hunger.isEnabled())
      pDoStarvationDamage = true;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      if (actor.needsFood() && !actor.isEgg() && (!actor.hasSubspecies() || !actor.subspecies.has_trait_energy_preserver || !actor.hasStatus("sleeping")))
        actor.updateNutritionDecay(pDoStarvationDamage);
    }
  }

  private void updateEventsBecomeAdult()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].eventBecomeAdult();
  }

  private void updateEventsEggHatched()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].eventHatchFromEgg();
  }

  private void updateActionLanded()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].actionLanded();
  }

  private void updateStats()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateStats();
  }

  private void u1_checkInside()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u1_checkInside(this._elapsed);
  }

  private void u2_updateChildren()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u2_updateChildren(this._elapsed);
  }

  private void u3_spriteAnimation()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u3_spriteAnimation(this._elapsed);
  }

  private void u4_deadCheck()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u4_deadCheck(this._elapsed);
  }

  private void u5_curTileAction()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u5_curTileAction();
  }

  private void u5_checkTileDeath()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].checkDieOnGroundBoat();
  }

  private void u6_checkFrozen()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u6_checkFrozen(this._elapsed);
  }

  private void u7_checkAugmentationEffects()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u7_checkAugmentationEffects();
  }

  private void u8_checkUpdateTimers()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u8_checkUpdateTimers(this._elapsed);
  }

  private void b1_checkUnderForce()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b1_checkUnderForce(this._elapsed);
  }

  private void b2_checkCurrentEnemyTarget()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b2_checkCurrentEnemyTarget(this._elapsed);
  }

  private void b3_findEnemyTarget()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b3_findEnemyTarget(this._elapsed);
  }

  private void b4_checkTaskVerifier()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b4_checkTaskVerifier(this._elapsed);
  }

  private void b5_checkPathMovement()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b5_checkPathMovement(this._elapsed);
  }

  private void b6_0_updateDecision()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b6_0_updateDecision(this._elapsed);
    this._cur_container.Clear();
  }

  private void b55_updateNaturalDeaths()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b55_updateNaturalDeaths(this._elapsed);
  }

  private void b6_updateAI()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].b6_updateAI(this._elapsed);
  }

  private void u10_checkSmoothMovement()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].u10_checkSmoothMovement(this._elapsed);
  }

  private void updateShake()
  {
    if (!this.check(this._cur_container))
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateShake(this._elapsed);
    this._cur_container.checkAddRemove();
  }

  private void updateHovering()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updateHover(this._elapsed);
  }

  private void updatePollinating()
  {
    if (!this.check(this._cur_container) || World.world.isPaused())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].updatePollinate(this._elapsed);
  }

  private void updateDeathCheck()
  {
    if (!this.check(this._cur_container) || World.world.isWindowOnScreen())
      return;
    Actor[] array = this._array;
    int count = this._count;
    for (int index = 0; index < count; ++index)
      array[index].checkDeath();
    this._cur_container.Clear();
  }

  internal override void clear()
  {
    base.clear();
    JobUpdater clearParallelResults = this.clearParallelResults;
    if (clearParallelResults == null)
      return;
    clearParallelResults();
  }

  private void clearParallelSprites() => this.l_parallel_update_sprites.Clear();

  internal override void add(Actor pActor)
  {
    base.add(pActor);
    pActor.batch = this;
  }

  internal override void remove(Actor pObject)
  {
    base.remove(pObject);
    pObject.batch = (BatchActors) null;
  }
}
