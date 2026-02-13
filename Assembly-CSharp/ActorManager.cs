// Decompiled with JetBrains decompiler
// Type: ActorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using ai.behaviours;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public class ActorManager : SimSystemManager<Actor, ActorData>
{
  private JobManagerActors _job_manager;
  public readonly ActorRenderData render_data = new ActorRenderData(4096 /*0x1000*/);
  public readonly ActorVisibleDataArray visible_units_avatars = new ActorVisibleDataArray();
  public readonly ActorVisibleDataArray visible_units = new ActorVisibleDataArray();
  public readonly ActorVisibleDataArray visible_units_alive = new ActorVisibleDataArray();
  public readonly ActorVisibleDataArray visible_units_with_status = new ActorVisibleDataArray();
  public readonly ActorVisibleDataArray visible_units_with_favorite = new ActorVisibleDataArray();
  public readonly ActorVisibleDataArray visible_units_with_banner = new ActorVisibleDataArray();
  public readonly ActorVisibleDataArray visible_units_just_ate = new ActorVisibleDataArray();
  public readonly ActorVisibleDataArray visible_units_socialize = new ActorVisibleDataArray();
  private double _timestamp_sleeping_units;
  public readonly List<Actor> cached_sleeping_units = new List<Actor>();
  private readonly List<ActorVisibleDataArray> _unit_visible_lists = new List<ActorVisibleDataArray>();
  public bool have_dying_units;
  public readonly List<Actor> units_only_wild = new List<Actor>();
  public readonly List<Actor> units_only_civ = new List<Actor>();
  public readonly List<Actor> units_only_alive = new List<Actor>();
  public readonly List<Actor> units_only_dying = new List<Actor>();

  public ActorManager()
  {
    this.type_id = "unit";
    this._job_manager = new JobManagerActors("actors");
    this._unit_visible_lists.Add(this.visible_units);
    this._unit_visible_lists.Add(this.visible_units_avatars);
    this._unit_visible_lists.Add(this.visible_units_alive);
    this._unit_visible_lists.Add(this.visible_units_with_status);
    this._unit_visible_lists.Add(this.visible_units_with_favorite);
    this._unit_visible_lists.Add(this.visible_units_with_banner);
    this._unit_visible_lists.Add(this.visible_units_just_ate);
    this._unit_visible_lists.Add(this.visible_units_socialize);
  }

  public void prepareForMetaChecks()
  {
    this.units_only_wild.Clear();
    this.units_only_alive.Clear();
    this.units_only_dying.Clear();
    this.units_only_civ.Clear();
    this.have_dying_units = false;
    List<Actor> simpleList = this.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
    {
      Actor actor = simpleList[index];
      if (actor.isAlive())
      {
        if (actor.kingdom.wild)
          this.units_only_wild.Add(actor);
        else
          this.units_only_civ.Add(actor);
        this.units_only_alive.Add(actor);
      }
      else
      {
        this.units_only_dying.Add(actor);
        this.have_dying_units = true;
      }
    }
  }

  public void calculateVisibleActors()
  {
    Bench.bench("actors_prepare_lists", "game_total");
    this.clearLists();
    this.prepareLists();
    Bench.benchEnd("actors_prepare_lists", "game_total");
    Bench.bench("actors_fill_visible", "game_total");
    this.fillVisibleObjects();
    Bench.benchEnd("actors_fill_visible", "game_total");
    Bench.bench("actors_precalc_render_data_parallel", "game_total");
    this.precalculateRenderDataParallel();
    Bench.benchEnd("actors_precalc_render_data_parallel", "game_total");
    Bench.bench("actors_precalc_render_data_normal", "game_total");
    this.precalculateRenderDataNormal();
    Bench.benchEnd("actors_precalc_render_data_normal", "game_total");
  }

  private void precalculateRenderDataParallel()
  {
    int tDebugItemScale = DebugConfig.isOn(DebugOption.RenderBigItems) ? 10 : 1;
    bool tShouldRenderUnitShadows = World.world.quality_changer.shouldRenderUnitShadows();
    int tTotalVisibleObjects = this.visible_units.count;
    Actor[] tArray = this.visible_units.array;
    int tDynamicBatchSize = 256 /*0x0100*/;
    Parallel.For(0, ParallelHelper.calcTotalBatches(tTotalVisibleObjects, tDynamicBatchSize), World.world.parallel_options, (Action<int>) (pBatchIndex =>
    {
      int batchBeg = ParallelHelper.calculateBatchBeg(pBatchIndex, tDynamicBatchSize);
      int batchEnd = ParallelHelper.calculateBatchEnd(batchBeg, tDynamicBatchSize, tTotalVisibleObjects);
      for (int index = batchBeg; index < batchEnd; ++index)
      {
        Actor actor = tArray[index];
        Vector3 currentScale = actor.current_scale;
        Vector3 vector3_1 = actor.updateRotation();
        Vector3 vector3_2 = actor.updatePos();
        bool flag1 = actor.checkHasRenderedItem();
        bool flag2 = !actor.asset.ignore_generic_render;
        Sprite sprite;
        if (flag1)
        {
          Sprite renderedItemSprite = actor.getRenderedItemSprite();
          IHandRenderer handRendererAsset = actor.getCachedHandRendererAsset();
          int pColorID = -900000;
          if (handRendererAsset.is_colored)
            pColorID = actor.kingdom.getColor().GetHashCode();
          sprite = DynamicSprites.getCachedAtlasItemSprite(DynamicSprites.getItemSpriteID(renderedItemSprite, pColorID), renderedItemSprite);
        }
        else
          sprite = (Sprite) null;
        this.render_data.positions[index] = vector3_2;
        this.render_data.scales[index] = currentScale;
        this.render_data.rotations[index] = vector3_1;
        this.render_data.flip_x_states[index] = actor.flip;
        this.render_data.colors[index] = actor.color;
        this.render_data.has_normal_render[index] = flag2;
        this.render_data.has_item[index] = flag1;
        this.render_data.item_sprites[index] = sprite;
        AnimationFrameData animationFrameData = actor.getAnimationFrameData();
        bool flag3 = false;
        if (tShouldRenderUnitShadows && actor.show_shadow)
        {
          ActorTextureSubAsset actorTextureSubAsset = !actor.hasSubspecies() || !actor.subspecies.has_mutation_reskin ? actor.asset.texture_asset : actor.subspecies.mutation_skin_asset.texture_asset;
          flag3 = actorTextureSubAsset.shadow;
          if (actorTextureSubAsset.shadow)
          {
            Vector2 vector2_1;
            if (actor.isEgg())
            {
              this.render_data.shadow_sprites[index] = actorTextureSubAsset.shadow_sprite_egg;
              vector2_1 = actorTextureSubAsset.shadow_size_egg;
            }
            else if (actor.isBaby())
            {
              this.render_data.shadow_sprites[index] = actorTextureSubAsset.shadow_sprite_baby;
              vector2_1 = actorTextureSubAsset.shadow_size_baby;
            }
            else
            {
              this.render_data.shadow_sprites[index] = actorTextureSubAsset.shadow_sprite;
              vector2_1 = actorTextureSubAsset.shadow_size;
            }
            Vector2 vector2_2 = Vector2.op_Multiply(vector2_1, Vector2.op_Implicit(currentScale));
            int num1 = actor.flip ? 1 : -1;
            float num2 = vector2_2.x / 2f;
            float num3 = vector2_2.y * 0.6f;
            float num4 = Mathf.Abs(vector3_1.z);
            Vector2 currentShadowPosition = actor.current_shadow_position;
            currentShadowPosition.x += (float) ((double) num2 * ((double) vector3_1.z * (double) num1) / 90.0);
            currentShadowPosition.y -= (float) ((double) num3 * (double) num4 / 90.0);
            this.render_data.shadow_position[index] = Vector2.op_Implicit(currentShadowPosition);
            if (animationFrameData != null && Vector2.op_Inequality(animationFrameData.size_unit, new Vector2()))
            {
              float num5 = Vector2.op_Multiply(animationFrameData.size_unit, Vector2.op_Implicit(currentScale)).y / vector2_2.x * currentScale.x;
              float num6 = Mathf.Lerp(currentScale.x, num5, num4 / 90f);
              this.render_data.shadow_scales[index] = Vector2.op_Implicit(new Vector2(num6, currentScale.y));
            }
            else
              this.render_data.shadow_scales[index] = currentScale;
          }
        }
        this.render_data.shadows[index] = flag3;
        if (flag2)
        {
          if (actor.canParallelSetColoredSprite())
          {
            Sprite mainSprite = actor.calculateMainSprite();
            this.render_data.main_sprites[index] = mainSprite;
            this.render_data.main_sprite_colored[index] = !actor.hasColoredSprite() ? mainSprite : (actor.isColoredSpriteNeedsCheck(mainSprite) ? (Sprite) null : actor.getLastColoredSprite());
          }
          else
          {
            this.render_data.main_sprites[index] = (Sprite) null;
            this.render_data.main_sprite_colored[index] = (Sprite) null;
          }
          if (flag1)
          {
            this.render_data.item_scale[index] = Vector3.op_Multiply(currentScale, (float) tDebugItemScale);
            float num7 = 0.0f;
            float num8 = 0.0f;
            if (animationFrameData != null)
            {
              num7 = animationFrameData.pos_item.x;
              num8 = animationFrameData.pos_item.y;
            }
            float num9 = vector3_2.x + num7 * currentScale.x;
            float num10 = vector3_2.y + num8 * currentScale.y;
            float num11 = (float) ((double) num8 * (double) currentScale.y - 0.0099999997764825821);
            Vector3 point;
            // ISSUE: explicit constructor call
            ((Vector3) ref point).\u002Ector(num9, num10);
            Vector3 angles = vector3_1;
            if ((double) angles.y != 0.0 || (double) angles.z != 0.0)
            {
              Vector3 pivot;
              // ISSUE: explicit constructor call
              ((Vector3) ref pivot).\u002Ector(vector3_2.x, vector3_2.y, 0.0f);
              point = Toolbox.RotatePointAroundPivot(ref point, ref pivot, ref angles);
            }
            point.z = num11;
            this.render_data.item_pos[index] = point;
          }
        }
      }
    }));
  }

  private void precalculateRenderDataNormal()
  {
    ActorRenderData renderData = this.render_data;
    int count = this.visible_units.count;
    Actor[] array = this.visible_units.array;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      if (renderData.has_normal_render[index] && renderData.main_sprite_colored[index] == null)
      {
        Sprite pMainSprite = renderData.main_sprites[index] ?? actor.calculateMainSprite();
        renderData.main_sprite_colored[index] = actor.calculateColoredSprite(pMainSprite);
      }
    }
  }

  private void fillVisibleObjects()
  {
    this.prepareArray();
    Actor[] simpleArray = this.getSimpleArray();
    int count = this.Count;
    bool flag = MapBox.isRenderGameplay();
    int num1 = 0;
    int num2 = 0;
    Actor[] array1 = this.visible_units.array;
    Actor[] array2 = this.visible_units_alive.array;
    for (int index = 0; index < count; ++index)
    {
      Actor pUnit = simpleArray[index];
      ActorAsset asset = pUnit.asset;
      TileZone zone = pUnit.current_tile.zone;
      if (asset.has_avatar_prefab)
        this.visible_units_avatars.array[this.visible_units_avatars.count++] = pUnit;
      if (pUnit.isFavorite() && !asset.hide_favorite_icon && zone.visible && !ControllableUnit.isControllingUnit(pUnit))
        this.visible_units_with_favorite.array[this.visible_units_with_favorite.count++] = pUnit;
      if (zone.visible && flag && pUnit.is_visible)
      {
        array1[num1++] = pUnit;
        if (pUnit.isAlive())
        {
          array2[num2++] = pUnit;
          if (pUnit.is_army_captain)
            this.visible_units_with_banner.array[this.visible_units_with_banner.count++] = pUnit;
          if (asset.render_status_effects && pUnit.hasAnyStatusEffectToRender())
            this.visible_units_with_status.array[this.visible_units_with_status.count++] = pUnit;
          if (pUnit.timestamp_session_ate_food > 0.0)
            this.visible_units_just_ate.array[this.visible_units_just_ate.count++] = pUnit;
          BehaviourActionActor action = pUnit.ai.action;
          if (action != null && action.socialize)
            this.visible_units_socialize.array[this.visible_units_socialize.count++] = pUnit;
          else if (pUnit.is_forced_socialize_icon && !pUnit.is_moving && !pUnit.isLying() && pUnit.isAttackReady() && Date.getMonthsSince(pUnit.is_forced_socialize_timestamp) < 1)
            this.visible_units_socialize.array[this.visible_units_socialize.count++] = pUnit;
        }
      }
    }
    this.visible_units.count = num1;
    this.visible_units_alive.count = num2;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    Bench.bench("actors", "game_total");
    this.checkContainer();
    this._job_manager.updateBase(pElapsed);
    this.checkContainer();
    Bench.benchEnd("actors", "game_total");
  }

  private void checkOverrideUnitShooting()
  {
    if (!DebugConfig.isOn(DebugOption.OverrideUnitShooting) || !Input.GetMouseButtonDown(0))
      return;
    Vector2 mousePos = World.world.getMousePos();
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    Actor actorNearCursor = World.world.getActorNearCursor();
    if (mouseTilePos == null)
      return;
    foreach (Actor actor in (SimSystemManager<Actor, ActorData>) this)
    {
      if (actor != actorNearCursor && actor.isAlive() && actor.hasRangeAttack())
      {
        Actor pInitiator = actor;
        Kingdom kingdom = actor.kingdom;
        Vector3 vector3 = Vector2.op_Implicit(actor.current_position);
        WorldTile pHitTile = mouseTilePos;
        Vector3 pHitPosition = Vector2.op_Implicit(mousePos);
        Vector3 pInitiatorPosition = vector3;
        Kingdom pKingdom = kingdom;
        string projectile = actor.getWeaponAsset().projectile;
        AttackData pData = new AttackData((BaseSimObject) pInitiator, pHitTile, pHitPosition, pInitiatorPosition, (BaseSimObject) null, pKingdom, AttackType.Weapon, pProjectileID: projectile);
        int num = CombatActionLibrary.combat_attack_range.action(pData) ? 1 : 0;
      }
    }
  }

  protected override void destroyObject(Actor pActor)
  {
    base.destroyObject(pActor);
    if (pActor.hasKingdom())
      pActor.setKingdom((Kingdom) null);
    if (pActor.hasSubspecies())
      pActor.setSubspecies((Subspecies) null);
    if (pActor.tile_target != null)
      pActor.clearTileTarget();
    pActor.asset.units.Remove(pActor);
    this.removeObject(pActor);
    this._job_manager.removeObject(pActor, pActor.batch);
    if (Object.op_Inequality((Object) pActor.avatar, (Object) null))
    {
      Object.Destroy((Object) pActor.avatar);
      pActor.avatar = (GameObject) null;
    }
    if (pActor.idle_loop_sound == null)
      return;
    pActor.idle_loop_sound.stop();
  }

  internal override void scheduleDestroyOnPlay(Actor pObject)
  {
    this.triggerActionsOnRemove(pObject);
    base.scheduleDestroyOnPlay(pObject);
  }

  private void triggerActionsOnRemove(Actor pActor)
  {
    foreach (ActorTrait trait in pActor.traits)
    {
      WorldActionTrait actionOnObjectRemove = trait.action_on_object_remove;
      if (actionOnObjectRemove != null)
      {
        int num = actionOnObjectRemove((NanoObject) pActor, (BaseAugmentationAsset) trait) ? 1 : 0;
      }
    }
  }

  public override void loadFromSave(List<ActorData> pList)
  {
    base.loadFromSave(pList);
    this.checkContainer();
  }

  public Actor evolutionEvent(Actor pTargetActor, bool pWithBiomeEffect, bool pAscension)
  {
    Subspecies subspecies1 = pTargetActor.subspecies;
    bool flag1 = false;
    Subspecies subspecies2 = (Subspecies) null;
    string pID = pTargetActor.asset.id;
    if (subspecies1.hasEvolvedIntoForm() && !pAscension)
    {
      subspecies2 = subspecies1.getEvolvedInto();
      if (subspecies2 != null)
        pID = subspecies2.getActorAsset().id;
    }
    if (subspecies2 == null)
    {
      bool flag2 = false;
      if (pTargetActor.asset.can_evolve_into_new_species)
      {
        flag2 = subspecies1.isSapient() || Randy.randomBool();
        if (flag2)
          pID = pTargetActor.asset.evolution_id;
      }
      if (!flag2)
      {
        Subspecies subspecies3 = World.world.subspecies.newSpecies(pTargetActor.asset, pTargetActor.current_tile, true);
        flag1 = true;
        subspecies3.mutateFrom(subspecies1);
        subspecies2 = subspecies3;
      }
    }
    if (subspecies2 == null)
    {
      subspecies2 = World.world.subspecies.newSpecies(AssetManager.actor_library.get(pID), pTargetActor.current_tile);
      flag1 = true;
      subspecies2.mutateFrom(subspecies1);
    }
    if (flag1)
    {
      subspecies2.addTrait("uplifted");
      subspecies2.makeSapient();
      subspecies2.data.biome_variant = subspecies1.data.biome_variant;
    }
    ActorAsset pAsset = AssetManager.actor_library.get(pID);
    pTargetActor.setAsset(pAsset);
    pTargetActor.setSubspecies(subspecies2);
    subspecies2.data.parent_subspecies = subspecies1.id;
    if (pAscension)
    {
      string name = subspecies2.name;
      if (!name.Contains("Ascentus"))
      {
        string pName = name + " Ascentus";
        subspecies2.setName(pName);
      }
    }
    else
      subspecies1.setEvolutionSubspecies(subspecies2);
    if (pWithBiomeEffect && Randy.randomChance(0.1f))
    {
      BiomeAsset biome = pTargetActor.current_tile.getBiome();
      if (biome != null && biome.evolution_trait_subspecies != null && biome.evolution_trait_subspecies.Count > 0)
      {
        SubspeciesTrait pTrait = AssetManager.subspecies_traits.get(biome.evolution_trait_subspecies.GetRandom<string>());
        if (pTrait != null)
          subspecies2.addTrait(pTrait, false);
      }
    }
    pTargetActor.afterEvolutionEvents();
    return pTargetActor;
  }

  public bool cloneUnit(Actor pCloneFrom, WorldTile pTileTarget = null)
  {
    if (pCloneFrom == null || !pCloneFrom.asset.can_be_cloned)
      return false;
    pCloneFrom.prepareForSave();
    ActorData data = pCloneFrom.data;
    string name = pCloneFrom.getName();
    ActorData actorData = new ActorData();
    ActorTool.copyImportantData(data, actorData, true);
    actorData.created_time = World.world.getCurWorldTime();
    actorData.id = World.world.map_stats.getNextId("unit");
    actorData.name = name;
    actorData.custom_name = data.custom_name;
    actorData.age_overgrowth = data.getAge();
    actorData.parent_id_1 = data.id;
    pCloneFrom.increaseBirths();
    if (pTileTarget == null)
      pTileTarget = pCloneFrom.current_tile.neighboursAll.GetRandom<WorldTile>();
    actorData.x = pTileTarget.x;
    actorData.y = pTileTarget.y;
    Actor pActor = World.world.units.loadObject(actorData);
    pActor.created_time_unscaled = (double) Time.time;
    pActor.addTrait("fragile_health");
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pCloneFrom.getTraits())
      pActor.addTrait(trait);
    pActor.addTrait("miracle_born");
    if (!pCloneFrom.hasFamily() && pCloneFrom.asset.create_family_at_spawn)
      World.world.families.newFamily(pCloneFrom, pCloneFrom.current_tile, (Actor) null);
    pActor.data.cloneCustomDataFrom((BaseSystemData) pCloneFrom.data);
    pActor.setReligion(pCloneFrom.religion);
    pActor.setClan(pCloneFrom.clan);
    pActor.setCulture(pCloneFrom.culture);
    pActor.setSubspecies(pCloneFrom.subspecies);
    pActor.joinLanguage(pCloneFrom.language);
    pActor.setFamily(pCloneFrom.family);
    pActor.setHealth(data.health, false);
    pActor.setMana(data.mana, false);
    pActor.setStamina(data.stamina, false);
    pActor.setHappiness(data.happiness, false);
    pActor.setNutrition(data.nutrition, false);
    pActor.addTrait("clone");
    if (data.saved_items != null)
    {
      foreach (long savedItem in data.saved_items)
      {
        Item obj = World.world.items.get(savedItem);
        if (obj != null)
        {
          Item pItem = World.world.items.generateItem(obj.getAsset(), pActor: pCloneFrom);
          pItem.data.modifiers.Clear();
          pItem.data.modifiers.AddRange((IEnumerable<string>) obj.data.modifiers);
          pItem.data.modifiers.Remove("eternal");
          pItem.initItem();
          pActor.equipment.setItem(pItem, pActor);
        }
      }
    }
    pActor.applyRandomForce();
    if (pActor.isRendered())
      EffectsLibrary.spawn("fx_spawn", pTileTarget);
    if (pActor.asset.has_sound_spawn)
      MusicBox.playSound(pActor.asset.sound_spawn, pTileTarget);
    return true;
  }

  public Actor createNewUnit(
    string pStatsID,
    WorldTile pTile,
    bool pMiracleSpawn = false,
    float pSpawnHeight = 0.0f,
    Subspecies pSubspecies = null,
    Subspecies pSubspeciesMutateFrom = null,
    bool pSpawnWithItems = true,
    bool pAdultAge = false,
    bool pGiveOwnerlessItems = false,
    bool pSapientSubspecies = false)
  {
    ActorAsset pAsset1 = AssetManager.actor_library.get(pStatsID);
    if (pAsset1 == null)
      return (Actor) null;
    Actor pActor1 = this.newObject();
    pActor1.setAsset(pAsset1);
    if (!pSubspecies.isRekt())
    {
      pActor1.setSubspecies(pSubspecies);
    }
    else
    {
      ActorAsset pAsset2 = pAsset1;
      WorldTile pTile1 = pTile;
      Actor pActor2 = pActor1;
      Actor actor;
      ref Actor local = ref actor;
      Subspecies subspecies = pSubspeciesMutateFrom;
      int num = pSapientSubspecies ? 1 : 0;
      Subspecies pSubspeciesMutateFrom1 = subspecies;
      this.checkNewSpecies(pAsset2, pTile1, pActor2, out local, pLookForSapientSubspecies: num != 0, pSubspeciesMutateFrom: pSubspeciesMutateFrom1);
      if (pMiracleSpawn && actor != null)
      {
        if (actor.hasCulture())
          pActor1.setCulture(actor.culture);
        if (actor.hasReligion())
          pActor1.setReligion(actor.religion);
        if (actor.hasLanguage())
          pActor1.setLanguage(actor.language);
      }
    }
    this.addRandomTraitFromBiomeToActor(pActor1, pTile);
    this.finalizeActor(pStatsID, pActor1, pTile, pSpawnHeight);
    if (pMiracleSpawn | pAdultAge)
    {
      if (pMiracleSpawn)
        pActor1.addTrait("miracle_born");
      pActor1.data.age_overgrowth = !pActor1.hasSubspecies() ? pAsset1.age_spawn : (int) Math.Ceiling((double) pActor1.subspecies.age_breeding);
      if (HotkeyLibrary.isHoldingAlt())
        pActor1.data.age_overgrowth = 0;
    }
    pActor1.newCreature();
    if (pSpawnWithItems)
      pActor1.generateDefaultSpawnWeapons(pGiveOwnerlessItems);
    pActor1.clearSprites();
    return pActor1;
  }

  private void finalizeActor(string pStats, Actor pActor, WorldTile pTile, float pZHeight = 0.0f)
  {
    ActorAsset pAsset = AssetManager.actor_library.get(pStats);
    pActor.setAsset(pAsset);
    ActorData data = pActor.data;
    pActor.spawnOn(pTile, pZHeight);
    if (data.subspecies.hasValue())
      pActor.setSubspecies(World.world.subspecies.get(data.subspecies));
    if (data.family.hasValue())
      pActor.setFamily(World.world.families.get(data.family));
    if (data.language.hasValue())
      pActor.setLanguage(World.world.languages.get(data.language));
    if (data.plot.hasValue())
      pActor.setPlot(World.world.plots.get(data.plot));
    if (data.religion.hasValue())
      pActor.setReligion(World.world.religions.get(data.religion));
    if (data.clan.hasValue())
      pActor.setClan(World.world.clans.get(data.clan));
    if (data.culture.hasValue())
      pActor.setCulture(World.world.cultures.get(data.culture));
    if (data.army.hasValue())
      pActor.setArmy(World.world.armies.get(data.army));
    pActor.create();
    pActor.checkDefaultKingdom();
    pActor.checkDefaultProfession();
    pActor.updateStats();
    if (!pActor.asset.can_be_killed_by_stuff)
      return;
    pActor.batch.c_main_tile_action.Add(pActor);
  }

  public Actor createBabyActorFromData(ActorData pData, WorldTile pTile, City pCity)
  {
    ActorAsset actorAsset = AssetManager.actor_library.get(pData.asset_id);
    Actor pActor = base.loadObject(pData);
    pActor.setData(pData);
    pActor.created_time_unscaled = (double) Time.time;
    this.finalizeActor(actorAsset.id, pActor, pTile);
    return pActor;
  }

  public Actor spawnNewUnitByPlayer(
    string pStatsID,
    WorldTile pTile,
    bool pSpawnSound = false,
    bool pMiracleSpawn = false,
    float pSpawnHeight = 6f,
    Subspecies pSubspecies = null)
  {
    Actor pActor = this.spawnNewUnit(pStatsID, pTile, pSpawnSound, pMiracleSpawn, pSpawnHeight, pSubspecies, true);
    if (pActor.current_zone.hasCity() && pActor.isSapient())
    {
      City city = pActor.current_zone.city;
      if (!city.isNeutral() && city.isPossibleToJoin(pActor))
        pActor.joinCity(city);
    }
    return pActor;
  }

  public Actor spawnNewUnit(
    string pActorAssetID,
    WorldTile pTile,
    bool pSpawnSound = false,
    bool pMiracleSpawn = false,
    float pSpawnHeight = 6f,
    Subspecies pSubspecies = null,
    bool pGiveOwnerlessItems = false,
    bool pAdultAge = false)
  {
    string pStatsID = pActorAssetID;
    WorldTile pTile1 = pTile;
    int num1 = pMiracleSpawn ? 1 : 0;
    double pSpawnHeight1 = (double) pSpawnHeight;
    Subspecies pSubspecies1 = pSubspecies;
    bool flag = pGiveOwnerlessItems;
    int num2 = pAdultAge ? 1 : 0;
    int num3 = flag ? 1 : 0;
    Actor newUnit = this.createNewUnit(pStatsID, pTile1, num1 != 0, (float) pSpawnHeight1, pSubspecies1, pAdultAge: num2 != 0, pGiveOwnerlessItems: num3 != 0);
    if (pSpawnSound && newUnit.asset.has_sound_spawn)
    {
      string soundSpawn = newUnit.asset.sound_spawn;
      Vector2Int pos = pTile.pos;
      double x = (double) ((Vector2Int) ref pos).x;
      pos = pTile.pos;
      double y = (double) ((Vector2Int) ref pos).y;
      MusicBox.playSound(soundSpawn, (float) x, (float) y);
    }
    if (newUnit.kingdom == null)
    {
      Kingdom pKingdomToSet = World.world.kingdoms_wild.get(newUnit.asset.kingdom_id_wild);
      newUnit.setKingdom(pKingdomToSet);
    }
    newUnit.setStatsDirty();
    newUnit.setNutrition(SimGlobals.m.nutrition_level_on_spawn);
    return newUnit;
  }

  private void checkNewSpecies(
    ActorAsset pAsset,
    WorldTile pTile,
    Actor pActor,
    out Actor pClosestActor,
    bool pGlobalSearch = false,
    bool pLookForSapientSubspecies = false,
    Subspecies pSubspeciesMutateFrom = null)
  {
    pClosestActor = (Actor) null;
    if (!pAsset.can_have_subspecies)
      return;
    Subspecies pObject = (Subspecies) null;
    if (pGlobalSearch)
    {
      foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) World.world.subspecies)
      {
        if (subspecies.isSpecies(pAsset.id))
        {
          pObject = subspecies;
          break;
        }
      }
    }
    if (pObject == null)
    {
      Actor pSubspeciesActor;
      pObject = World.world.subspecies.getNearbySpecies(pAsset, pTile, out pSubspeciesActor, pLookForSapientSubspecies, true);
      pClosestActor = pSubspeciesActor;
    }
    if (pObject == null)
    {
      pObject = World.world.subspecies.newSpecies(pAsset, pTile);
      if (pSubspeciesMutateFrom != null)
        pObject.mutateFrom(pSubspeciesMutateFrom);
      pObject.forceRecalcBaseStats();
    }
    pActor.setSubspecies(pObject);
    pActor.event_full_stats = true;
    pActor.setStatsDirty();
  }

  public ActorTrait addRandomTraitFromBiomeToActor(Actor pActor, WorldTile pTile)
  {
    if (!pTile.Type.is_biome)
      return (ActorTrait) null;
    BiomeAsset biomeAsset = pTile.Type.biome_asset;
    List<string> spawnTraitActor = biomeAsset.spawn_trait_actor;
    // ISSUE: explicit non-virtual call
    if ((spawnTraitActor != null ? (__nonvirtual (spawnTraitActor.Count) > 0 ? 1 : 0) : 0) == 0 || !Randy.randomBool())
      return (ActorTrait) null;
    string random = biomeAsset.spawn_trait_actor.GetRandom<string>();
    ActorTrait pTrait = AssetManager.traits.get(random);
    pActor.addTrait(pTrait);
    return pTrait;
  }

  public override Actor loadObject(ActorData pData)
  {
    if (this.dict.ContainsKey(pData.id))
    {
      Debug.Log((object) ("Trying to load unit with same ID, that already is loaded. " + pData.id.ToString()));
      return (Actor) null;
    }
    WorldTile tile = World.world.GetTile(pData.x, pData.y);
    if (tile == null)
      return (Actor) null;
    ActorAsset actorAsset = AssetManager.actor_library.get(pData.asset_id);
    if (actorAsset == null)
      return (Actor) null;
    int health = pData.health;
    int nutrition = pData.nutrition;
    int stamina = pData.stamina;
    int mana = pData.mana;
    Actor pActor = base.loadObject(pData);
    pActor.setData(pData);
    this.finalizeActor(actorAsset.id, pActor, tile);
    if (pActor.canUseItems())
      pActor.equipment.load(pData.saved_items, pActor);
    if (pActor.isSapient())
      pActor.reloadInventory();
    pActor.loadFromSave();
    pActor.updateStats();
    pActor.setHealth(health);
    pActor.setNutrition(nutrition);
    pActor.setStamina(stamina);
    pActor.setMana(mana);
    if (pActor.asset.can_have_subspecies && !pActor.hasSubspecies())
      this.checkNewSpecies(pActor.asset, pActor.current_tile, pActor, out Actor _, true);
    pActor.makeWait(Randy.randomFloat(0.1f, 2f));
    return pActor;
  }

  protected override void addObject(Actor pObject)
  {
    base.addObject(pObject);
    this._job_manager.addNewObject(pObject);
  }

  private void clearLists()
  {
    for (int index = 0; index < this._unit_visible_lists.Count; ++index)
      this._unit_visible_lists[index].count = 0;
  }

  private void prepareLists()
  {
    for (int index = 0; index < this._unit_visible_lists.Count; ++index)
      this._unit_visible_lists[index].prepare(this.Count);
    this.render_data.checkSize(this.Count);
    this.checkContainer();
  }

  public override void clear()
  {
    this._job_manager.clear();
    this.cached_sleeping_units.Clear();
    this.clearLists();
    this.checkContainer();
    this.scheduleDestroyAllOnWorldClear();
    this.checkObjectsToDestroy();
    base.clear();
  }

  public void debugJobManager(DebugTool pTool) => this._job_manager.debug(pTool);

  public JobManagerActors getJobManager() => this._job_manager;

  public void checkSleepingUnits()
  {
    if ((double) World.world.getWorldTimeElapsedSince(this._timestamp_sleeping_units) < 10.0)
      return;
    this.cached_sleeping_units.Clear();
    this._timestamp_sleeping_units = World.world.getCurWorldTime();
    foreach (Status status in World.world.statuses.list.LoopRandom<Status>())
    {
      if (!status.is_finished && !(status.asset.id != "sleeping"))
      {
        Actor a = status.sim_object.a;
        if (a.isAlive())
        {
          this.cached_sleeping_units.Add(a);
          if (this.cached_sleeping_units.Count > 10)
            break;
        }
      }
    }
  }
}
