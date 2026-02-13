// Decompiled with JetBrains decompiler
// Type: Actor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using ai.behaviours;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
public class Actor : 
  BaseSimObject,
  ILoadable<ActorData>,
  ITraitsOwner<ActorTrait>,
  IEquatable<Actor>,
  IComparable<Actor>,
  IFavoriteable
{
  internal ActorIdleLoopSound idle_loop_sound;
  internal bool is_forced_socialize_icon;
  internal double is_forced_socialize_timestamp;
  internal string ate_last_item_id;
  internal double timestamp_session_ate_food;
  internal double timestamp_tween_session_social;
  private double _last_color_effect_timestamp;
  private double _last_stamina_reduce_timestamp;
  internal double timestamp_profession_set;
  internal List<BaseActorComponent> children_special;
  private Dictionary<Type, BaseActorComponent> _dict_special;
  private List<ActorSimpleComponent> children_pre_behaviour;
  private Dictionary<Type, ActorSimpleComponent> dict_pre_behaviour;
  private UnitProfession _profession;
  public GameObject avatar;
  private double _timestamp_augmentation_effects;
  internal bool show_shadow;
  internal Vector2 current_shadow_position = Vector2.op_Implicit(Globals.POINT_IN_VOID);
  private double[] _decision_cooldowns;
  private bool[] _decision_disabled;
  public DecisionAsset[] decisions = new DecisionAsset[64 /*0x40*/];
  public int decisions_counter;
  private int _current_children;
  private readonly Queue<HappinessHistory> _last_happiness_history = new Queue<HappinessHistory>();
  private HashSet<long> _aggression_targets = new HashSet<long>();
  private HoverState _hover_state;
  private float _hover_timer;
  public BatchActors batch;
  internal WorldTile beh_tile_target;
  internal Building beh_building_target;
  internal BaseSimObject beh_actor_target;
  internal Book beh_book_target;
  internal Building inside_building;
  internal bool is_inside_building;
  internal Boat inside_boat;
  internal bool is_inside_boat;
  internal BaseSimObject attackedBy;
  public Actor lover;
  public readonly HashSet<ActorTrait> traits = new HashSet<ActorTrait>();
  private readonly CombatActionHolder _combat_actions = new CombatActionHolder();
  private readonly SpellHolder _spells = new SpellHolder();
  private readonly Dictionary<string, bool> _traits_cache = new Dictionary<string, bool>();
  internal ActorData data;
  internal ProfessionAsset profession_asset;
  private bool _state_adult;
  private bool _state_baby;
  private bool _state_egg;
  public ActorAsset asset;
  public Vector2 next_step_position;
  public Vector2 next_step_position_possession;
  internal Vector2 shake_offset;
  public static readonly Vector2 sprite_offset = new Vector2(0.5f, 0.5f);
  public Vector2 move_jump_offset;
  private bool _shake_horizontal;
  private bool _shake_vertical;
  private float _shake_timer;
  private bool _shake_active;
  private float _shake_volume;
  private bool _is_moving;
  private bool _possessed_movement;
  private bool _is_in_liquid;
  internal bool is_visible;
  internal bool last_sprite_renderer_enabled;
  internal AnimationFrameData frame_data;
  internal bool dirty_current_tile;
  internal WorldTile tile_target;
  private WorldTile _next_step_tile;
  public SplitPathStatus split_path;
  public int current_path_index;
  public readonly List<WorldTile> current_path = new List<WorldTile>();
  public List<MapRegion> current_path_global;
  public BaseActionActor callbacks_on_death;
  public BaseActionActor callbacks_landed;
  public BaseActionActor callbacks_cancel_path_movement;
  public BaseActionActor callbacks_magnet_update;
  internal float actor_scale;
  internal float target_scale;
  internal BaseSimObject attack_target;
  internal bool has_attack_target;
  internal float timer_action;
  internal float timer_jump_animation;
  internal float hitbox_bonus_height;
  internal Vector3 velocity;
  internal float velocity_speed;
  internal bool under_forces;
  protected WorldTimer targets_to_ignore_timer;
  private bool _flying;
  internal bool is_in_magnet;
  internal float attack_timer;
  internal double last_attack_timestamp;
  internal EquipmentAsset _attack_asset;
  internal PersonalityAsset s_personality;
  private readonly List<BaseAugmentationAsset> _s_special_effect_augmentations = new List<BaseAugmentationAsset>();
  private readonly Dictionary<BaseAugmentationAsset, double> _s_special_effect_augmentations_timers = new Dictionary<BaseAugmentationAsset, double>();
  internal AttackAction s_action_attack_target;
  internal GetHitAction s_get_hit_action;
  protected static readonly List<BaseAugmentationAsset> _tempAugmentationList = new List<BaseAugmentationAsset>();
  private bool _has_emotions;
  private bool _has_tag_unconscious;
  public bool has_tag_immunity_cold;
  private bool _has_status_strange_urge;
  private bool _has_status_possessed;
  private bool _has_status_sleeping;
  private bool _has_status_tantrum;
  private bool _has_status_drowning;
  private bool _has_status_invincible;
  private bool _cache_check_has_status_removed_on_damage;
  private bool _has_trait_weightless;
  private bool _has_trait_peaceful;
  private bool _has_trait_clone;
  internal bool has_tag_generate_light;
  private bool _has_any_sick_trait;
  internal bool is_immovable;
  internal bool is_ai_frozen;
  private bool _has_stop_idle_animation;
  private bool _ignore_fights;
  protected bool should_check_land_cancel;
  internal WorldTile scheduled_tile_target;
  internal bool _action_wait_after_land;
  internal float _action_wait_after_land_timer;
  internal AiSystemActor ai;
  public CitizenJobAsset citizen_job;
  protected Building _home_building;
  private float _death_timer_color_stage_1;
  private float _death_timer_alpha_stage_2;
  private float _jump_time;
  private float lastX;
  private float lastY;
  public float flip_angle;
  internal bool flip;
  private int _precalc_movement_speed_skips;
  private float _current_combined_movement_speed;
  internal float _timeout_targets;
  internal Vector3 target_angle;
  internal float rotation_cooldown;
  private RotationDirection _rotation_direction;
  private Sprite _last_topic_sprite;
  public Color color;
  internal bool dirty_sprite_main;
  private Sprite _cached_sprite_item;
  private IHandRenderer _cached_hand_renderer_asset;
  internal Sprite cached_sprite_head;
  internal bool dirty_sprite_head;
  internal AnimationContainerUnit animation_container;
  private Sprite _last_main_sprite;
  private Sprite _last_colored_sprite;
  private ColorAsset _last_color_asset;
  private bool _dirty_sprite_item;
  private bool _has_animated_item;
  public SpriteAnimation sprite_animation;
  private const float POSSESSION_ATTACK_SECONDS = 0.5f;
  private double _possession_attack_happened_frame;
  private AttackType _last_attack_type;
  public ActorEquipment equipment;
  public Army army;
  public City city;
  public Clan clan;
  public Culture culture;
  public Family family;
  public Language language;
  public Plot plot;
  public Religion religion;
  public Subspecies subspecies;
  private const float FIND_TILE_SQ_DIST = 4f;
  private const float CUR_SQ_DIST = 0.160000011f;
  private const float NEW_SQ_DIST = 0.09f;
  private bool _beh_skip;
  private bool _update_done;
  private string _last_decision_id;

  public Queue<HappinessHistory> happiness_change_history => this._last_happiness_history;

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this._last_decision_id = string.Empty;
    this.is_inside_building = false;
    this.inside_building = (Building) null;
    this._state_adult = false;
    this._state_baby = false;
    this._state_egg = false;
    this.next_step_position = Vector2.op_Implicit(Globals.emptyVector);
    this.shake_offset = new Vector2(0.0f, 0.0f);
    this.move_jump_offset = new Vector2(0.0f, 0.0f);
    this._shake_horizontal = true;
    this._shake_vertical = true;
    this._shake_timer = 0.0f;
    this._shake_active = false;
    this._shake_volume = 0.1f;
    this._is_moving = false;
    this.is_visible = false;
    this.last_sprite_renderer_enabled = false;
    this.dirty_current_tile = true;
    this.split_path = SplitPathStatus.Normal;
    this.current_path_index = 0;
    this.current_path_global = (List<MapRegion>) null;
    this.actor_scale = 0.0f;
    this.target_scale = 0.0f;
    this.timer_action = 0.0f;
    this.timer_jump_animation = 0.0f;
    this.hitbox_bonus_height = 0.0f;
    this.velocity = new Vector3();
    this.velocity_speed = 0.0f;
    this.under_forces = false;
    this._flying = false;
    this.is_in_magnet = false;
    this.attack_timer = 0.0f;
    this._attack_asset = ItemLibrary.base_attack;
    this.dirty_sprite_main = true;
    this.cached_sprite_head = (Sprite) null;
    this.dirty_sprite_head = true;
    this._cached_sprite_item = (Sprite) null;
    this._cached_hand_renderer_asset = (IHandRenderer) null;
    this.s_personality = (PersonalityAsset) null;
    this._action_wait_after_land = false;
    this.rotation_cooldown = 0.0f;
    this.target_angle = new Vector3();
    this._timeout_targets = 0.0f;
    this._precalc_movement_speed_skips = 0;
    this.flip_angle = 0.0f;
    this.lastX = -10f;
    this.lastY = -10f;
    this._jump_time = 0.0f;
    this._death_timer_color_stage_1 = 1f;
    this._death_timer_alpha_stage_2 = 1f;
    this.color = Color.white;
    this.ate_last_item_id = string.Empty;
    this.timestamp_session_ate_food = 0.0;
    this.timestamp_tween_session_social = 0.0;
    this.timestamp_profession_set = 0.0;
    this._timestamp_augmentation_effects = 0.0;
    this.show_shadow = false;
    this._decision_cooldowns = Toolbox.checkArraySize<double>(this._decision_cooldowns, AssetManager.decisions_library.list.Count);
    this._decision_disabled = Toolbox.checkArraySize<bool>(this._decision_disabled, AssetManager.decisions_library.list.Count);
    this.decisions = Toolbox.checkArraySize<DecisionAsset>(this.decisions, AssetManager.decisions_library.list.Count);
  }

  public void setShowShadow(bool pShadow) => this.show_shadow = pShadow;

  public string coloredName
  {
    get
    {
      if (this.data == null)
        return "";
      if (this.kingdom?.getColor() == null)
        return this.getName();
      return $"<color={this.kingdom.getColor().color_text}>{this.getName()}</color>";
    }
  }

  private void updateChildrenList(List<BaseActorComponent> pList, float pElapsed)
  {
    if (pList == null)
      return;
    for (int index = 0; index < pList.Count; ++index)
      pList[index].update(pElapsed);
  }

  private void updateChildrenListSimple(List<ActorSimpleComponent> pList, float pElapsed)
  {
    if (pList == null)
      return;
    for (int index = 0; index < pList.Count; ++index)
      pList[index].update(pElapsed);
  }

  public void setAsset(ActorAsset pAsset)
  {
    if (this.asset != null)
      this.asset.units.Remove(this);
    this.asset = pAsset;
    this.asset.units.Add(this);
    this.setStatsDirty();
    if (!this.canUseItems() || this.hasEquipment())
      return;
    this.equipment = new ActorEquipment();
  }

  internal override void create()
  {
    base.create();
    if (this.ai == null)
      this.ai = new AiSystemActor(this);
    this.ai.jobs_library = (AssetLibrary<ActorJob>) AssetManager.job_actor;
    this.ai.task_library = (AssetLibrary<BehaviourTaskActor>) AssetManager.tasks_actor;
    this.ai.next_job_delegate = new GetNextJobID(this.getNextJob);
    this.ai.clear_action_delegate = new JobAction(this.clearBeh);
    this.ai.subscribeToTaskSwitch(new TaskSwitchAction(this.setItemSpriteRenderDirty));
    if (this.targets_to_ignore_timer == null)
      this.targets_to_ignore_timer = new WorldTimer(3f, new Action(((BaseSimObject) this).clearIgnoreTargets));
    this._flying = this.asset.flying;
    this.setActorScale(this.asset.base_stats["scale"] * 0.6f);
    if (this.asset.finish_scale_on_creation)
    {
      this.target_scale = this.asset.base_stats["scale"];
      this.finishScale();
    }
    this.setObjectType(MapObjectType.Actor);
    this.setShowShadow(this.asset.shadow);
    if (this.asset.has_sound_idle_loop)
      this.idle_loop_sound = new ActorIdleLoopSound(this.asset, this);
    if (this.isHovering())
      this.move_jump_offset.y = this.asset.hovering_min;
    this.addChildren();
    if (this.asset.kingdom_id_wild.Contains("ants"))
      AchievementLibrary.ant_world.check();
    if (this.asset.kingdom_id_wild.Contains("monkey"))
      AchievementLibrary.planet_of_apes.check();
    if (this.asset.cancel_beh_on_land)
      this.callbacks_landed += new BaseActionActor(this.checkLand);
    this.callbacks_landed += new BaseActionActor(this.checkDeathOutsideMap);
    this.callbacks_on_death += new BaseActionActor(this.playDeathSound);
    this.callbacks_magnet_update += new BaseActionActor(this.actionMagnetAnimation);
  }

  public bool canSeeTileBasedOnDirection(WorldTile pTile)
  {
    return this.is_looking_left == this.isTileOnTheLeft(pTile);
  }

  public void setParent1(Actor pParentActor, bool pIncreaseChildren = true)
  {
    this.data.parent_id_1 = pParentActor.data.id;
    if (!pIncreaseChildren)
      return;
    pParentActor.increaseChildren();
  }

  public void setParent2(Actor pActor, bool pIncreaseChildren = true)
  {
    this.data.parent_id_2 = pActor.data.id;
    if (!pIncreaseChildren)
      return;
    pActor.increaseChildren();
  }

  internal void setProfession(UnitProfession pType, bool pCancelBeh = true)
  {
    this._profession = pType;
    this.profession_asset = AssetManager.professions.get(pType);
    this.setStatsDirty();
    if (this.hasCity())
      this.city.setCitizensDirty();
    if (pCancelBeh)
      this.cancelAllBeh();
    this.timestamp_profession_set = World.world.getCurWorldTime();
    this.clearGraphicsFully();
  }

  private void addChildren()
  {
    if (this.asset.avatar_prefab != string.Empty)
    {
      this.avatar = Object.Instantiate<GameObject>(Resources.Load<GameObject>("actors/" + this.asset.avatar_prefab), World.world.transform_units);
      if (this.avatar.HasComponent<SpriteAnimation>())
      {
        this.sprite_animation = this.avatar.GetComponent<SpriteAnimation>();
        this.batch.c_sprite_animations.Add(this);
      }
      if (this.avatar.HasComponent<Crabzilla>())
        this.addChild((BaseActorComponent) this.avatar.GetComponent<Crabzilla>());
      if (this.avatar.HasComponent<GodFinger>())
        this.addChild((BaseActorComponent) this.avatar.GetComponent<GodFinger>());
      if (this.avatar.HasComponent<Dragon>())
        this.addChild((BaseActorComponent) this.avatar.GetComponent<Dragon>());
      if (this.avatar.HasComponent<UFO>())
        this.addChild((BaseActorComponent) this.avatar.GetComponent<UFO>());
    }
    if (this.asset.is_boat)
      this.addChildSimple((ActorSimpleComponent) new Boat());
    if (this.children_pre_behaviour == null && this.children_special == null)
      return;
    this.batch.c_update_children.Add(this);
  }

  private void addChild(BaseActorComponent pObject)
  {
    if (this.children_special == null)
    {
      this.children_special = new List<BaseActorComponent>();
      this._dict_special = new Dictionary<Type, BaseActorComponent>();
    }
    Type type = ((object) pObject).GetType();
    this.children_special.Add(pObject);
    this._dict_special.Add(type, pObject);
    pObject.create(this);
  }

  private void addChildSimple(ActorSimpleComponent pObject)
  {
    if (this.children_pre_behaviour == null)
    {
      this.children_pre_behaviour = new List<ActorSimpleComponent>();
      this.dict_pre_behaviour = new Dictionary<Type, ActorSimpleComponent>();
    }
    Type type = pObject.GetType();
    this.children_pre_behaviour.Add(pObject);
    this.dict_pre_behaviour.Add(type, pObject);
    pObject.create(this);
  }

  public T getActorComponent<T>() where T : BaseActorComponent
  {
    if (this._dict_special == null)
      return default (T);
    BaseActorComponent baseActorComponent;
    return this._dict_special.TryGetValue(typeof (T), out baseActorComponent) ? baseActorComponent as T : default (T);
  }

  public T getSimpleComponent<T>() where T : ActorSimpleComponent
  {
    ActorSimpleComponent actorSimpleComponent;
    return this.dict_pre_behaviour.TryGetValue(typeof (T), out actorSimpleComponent) ? actorSimpleComponent as T : default (T);
  }

  private void playDeathSound(Actor pActor)
  {
    if (!this.asset.has_sound_death)
      return;
    MusicBox.playSound(this.asset.sound_death, this.current_tile, true, true);
  }

  public void playIdleSound()
  {
    if (!this.asset.has_sound_idle)
      return;
    MusicBox.playIdleSoundVisibleOnly(this.asset.sound_idle, this.current_tile);
  }

  public void startShake(float pTimer = 0.3f, float pVol = 0.1f, bool pHorizontal = true, bool pVertical = true)
  {
    this._shake_horizontal = pHorizontal;
    this._shake_vertical = pVertical;
    this._shake_timer = Mathf.Min(pTimer, this.asset.max_shake_timer);
    this._shake_volume = pVol;
    this._shake_active = true;
    this.batch.c_shake.Add(this);
  }

  public Vector3 getThrowStartPosition()
  {
    Vector3 transformPosition = this.cur_transform_position;
    Vector3 currentScale = this.current_scale;
    Vector3 currentRotation = this.current_rotation;
    AnimationFrameData animationFrameData = this.getAnimationFrameData();
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (animationFrameData != null)
    {
      num1 = animationFrameData.pos_item.x;
      num2 = animationFrameData.pos_item.y;
    }
    float num3 = transformPosition.x + num1 * currentScale.x;
    float num4 = transformPosition.y + num2 * currentScale.y;
    Vector3 point;
    // ISSUE: explicit constructor call
    ((Vector3) ref point).\u002Ector(num3, num4, -0.01f);
    Vector3 angles = currentRotation;
    if ((double) angles.y != 0.0 || (double) angles.z != 0.0)
    {
      Vector3 pivot;
      // ISSUE: explicit constructor call
      ((Vector3) ref pivot).\u002Ector(transformPosition.x, transformPosition.y, 0.0f);
      point = Toolbox.RotatePointAroundPivot(ref point, ref pivot, ref angles);
      point.z = -0.01f;
    }
    return point;
  }

  public void checkDefaultProfession() => this.setProfession(UnitProfession.Unit, false);

  public void addAfterglowStatus()
  {
    this.addStatusEffect("afterglow", (float) this.asset.months_breeding_timeout * 5f);
  }

  public void updateHover(float pElapsed)
  {
    if (!this.isAlive())
    {
      this.changeMoveJumpOffset((float) (-(double) pElapsed * 10.0));
    }
    else
    {
      if (this.isOnGround())
        this.changeMoveJumpOffset((float) (-(double) pElapsed * 3.0));
      else if ((double) this.move_jump_offset.y < (double) this.asset.hovering_min)
      {
        this.changeMoveJumpOffset(pElapsed * 3f);
        return;
      }
      if ((double) this._hover_timer > 0.0)
      {
        this._hover_timer -= pElapsed;
      }
      else
      {
        this._hover_timer = 1f + Randy.randomFloat(0.0f, 4f);
        if (World.world.isPaused())
          return;
        switch (this._hover_state)
        {
          case HoverState.Hover:
            if (Randy.randomBool())
            {
              this._hover_state = HoverState.Down;
              break;
            }
            this._hover_state = HoverState.Up;
            break;
          case HoverState.Up:
            this._hover_state = HoverState.Hover;
            if ((double) this.move_jump_offset.y >= (double) this.asset.hovering_max)
              break;
            this.changeMoveJumpOffset(pElapsed * 3f);
            break;
          case HoverState.Down:
            this._hover_state = HoverState.Hover;
            if ((double) this.move_jump_offset.y <= (double) this.asset.hovering_min)
              break;
            this.changeMoveJumpOffset((float) (-(double) pElapsed * 3.0));
            break;
        }
      }
    }
  }

  public void updatePollinate(float pElapsed)
  {
    if (!this.isAlive())
      return;
    if (!this.is_moving && this.ai.task?.id == "pollinate")
    {
      this.setHoverState(HoverState.Down);
      this.changeMoveJumpOffset((float) (-(double) pElapsed * 3.0));
    }
    else
    {
      this.setHoverState(HoverState.Up);
      if ((double) this.move_jump_offset.y >= (double) this.asset.hovering_max)
        return;
      this.changeMoveJumpOffset(pElapsed * 3f);
    }
  }

  private void checkCalibrateTargetPosition()
  {
    if (this.hasRangeAttack() || this.beh_actor_target == null)
      return;
    BaseSimObject behActorTarget = this.beh_actor_target;
    if (!this.hasTask() || this.ai.action == null || !this.ai.action.calibrate_target_position || behActorTarget == null || !behActorTarget.isActor())
      return;
    Actor a = this.beh_actor_target.a;
    if ((double) Toolbox.SquaredDist(a.current_tile.x, a.current_tile.y, this.tile_target.x, this.tile_target.y) <= (double) (this.ai.action.check_actor_target_position_distance * this.ai.action.check_actor_target_position_distance))
      return;
    this.clearPathForCalibration();
    int num = (int) this.ai.action.startExecute(this);
  }

  internal override bool addStatusEffect(
    StatusAsset pStatusAsset,
    float pOverrideTimer = 0.0f,
    bool pColorEffect = true)
  {
    if (pStatusAsset.affects_mind && this.hasTag("strong_mind"))
      return false;
    int num = base.addStatusEffect(pStatusAsset, pOverrideTimer, pColorEffect) ? 1 : 0;
    if ((num & (pColorEffect ? 1 : 0)) == 0)
      return num != 0;
    this.startColorEffect();
    return num != 0;
  }

  public void setTargetAngleZ(float pValue) => this.target_angle.z = pValue;

  public void lookTowardsPosition(Vector2 pDirection)
  {
    if (!this.asset.can_flip)
      return;
    if ((double) this.current_position.x < (double) pDirection.x)
      this.setFlip(true);
    else
      this.setFlip(false);
  }

  public override void setStatsDirty()
  {
    if (this.isAlive())
      this.batch.c_stats_dirty.Add(this);
    base.setStatsDirty();
    this.setItemSpriteRenderDirty();
  }

  private void checkRageDemon()
  {
    if (!WorldLawLibrary.world_law_disasters_other.isEnabled() || !this.canTurnIntoDemon() || !World.world_era.era_disaster_rage_brings_demons || this.hasTrait("blessed") || this.isFavorite() || !this.hasStatus("rage") || !Randy.randomChance(0.1f))
      return;
    ActionLibrary.turnIntoDemon((BaseSimObject) this);
  }

  internal void updateChangeScale(float pElapsed)
  {
    if ((double) this.actor_scale == (double) this.target_scale)
      return;
    if ((double) this.actor_scale > (double) this.target_scale)
    {
      this.setActorScale(this.actor_scale - 0.2f * pElapsed);
      if ((double) this.actor_scale >= (double) this.target_scale)
        return;
      this.setActorScale(this.target_scale);
    }
    else
    {
      this.setActorScale(this.actor_scale + 0.2f * pElapsed);
      if ((double) this.actor_scale <= (double) this.target_scale)
        return;
      this.setActorScale(this.target_scale);
    }
  }

  internal void newCreature()
  {
    this.changeHappiness("just_born");
    ++World.world.game_stats.data.creaturesCreated;
    ++World.world.map_stats.creaturesCreated;
    AchievementLibrary.ten_thousands_creatures.check();
    this.generatePersonality();
    this.checkShouldBeEgg();
    this.event_full_stats = true;
    this.updateStats();
    this.event_full_stats = true;
    if (!this.needsFood())
      return;
    this.setNutrition(this.getMaxNutrition());
  }

  public void clearTraits()
  {
    this.clearTraitCache();
    this.traits.Clear();
  }

  public override void Dispose()
  {
    WorldBehaviourUnitTemperatures.removeUnit(this);
    this.clearTraits();
    this.idle_loop_sound = (ActorIdleLoopSound) null;
    this.checkSimpleComponentListDispose(this.children_pre_behaviour);
    this.checkComponentListDispose(this.children_special);
    this._profession = UnitProfession.Nothing;
    this.sprite_animation = (SpriteAnimation) null;
    this.lover = (Actor) null;
    this.idle_loop_sound = (ActorIdleLoopSound) null;
    this.scheduled_tile_target = (WorldTile) null;
    this._last_main_sprite = (Sprite) null;
    this._last_colored_sprite = (Sprite) null;
    this._last_color_asset = (ColorAsset) null;
    this._last_topic_sprite = (Sprite) null;
    this.children_special = (List<BaseActorComponent>) null;
    this._dict_special = (Dictionary<Type, BaseActorComponent>) null;
    this.children_pre_behaviour = (List<ActorSimpleComponent>) null;
    this.dict_pre_behaviour = (Dictionary<Type, ActorSimpleComponent>) null;
    this.avatar = (GameObject) null;
    this.clearDecisions();
    if (this.hasSubspecies())
    {
      World.world.subspecies.unitDied(this.subspecies);
      this.subspecies = (Subspecies) null;
    }
    if (this.hasCulture())
    {
      World.world.cultures.unitDied(this.culture);
      this.culture = (Culture) null;
    }
    this.ai.reset();
    this._last_happiness_history.Clear();
    this.citizen_job = (CitizenJobAsset) null;
    if (this.hasCity())
    {
      World.world.cities.unitDied(this.city);
      this.city = (City) null;
    }
    if (this.hasKingdom())
    {
      if (this.isKing())
        this.kingdom.removeKing();
      World.world.kingdoms.unitDied(this.kingdom);
      this.kingdom = (Kingdom) null;
    }
    this.callbacks_on_death = (BaseActionActor) null;
    this.callbacks_landed = (BaseActionActor) null;
    this.callbacks_cancel_path_movement = (BaseActionActor) null;
    this.callbacks_magnet_update = (BaseActionActor) null;
    this.s_personality = (PersonalityAsset) null;
    this._s_special_effect_augmentations.Clear();
    this._s_special_effect_augmentations_timers.Clear();
    this.s_action_attack_target = (AttackAction) null;
    this.targets_to_ignore_timer = (WorldTimer) null;
    this.clearOldPath();
    this.data = (ActorData) null;
    this.attackedBy = (BaseSimObject) null;
    this.attack_target = (BaseSimObject) null;
    this.has_attack_target = false;
    this.army = (Army) null;
    this.clan = (Clan) null;
    this.culture = (Culture) null;
    this.family = (Family) null;
    this.language = (Language) null;
    this.plot = (Plot) null;
    this.religion = (Religion) null;
    this.subspecies = (Subspecies) null;
    this.beh_tile_target = (WorldTile) null;
    this.beh_building_target = (Building) null;
    this.beh_actor_target = (BaseSimObject) null;
    this.beh_book_target = (Book) null;
    this.exitBuilding();
    this.inside_boat = (Boat) null;
    this.is_inside_boat = false;
    this.army = (Army) null;
    this.batch = (BatchActors) null;
    this.equipment = (ActorEquipment) null;
    this.tile_target = (WorldTile) null;
    this.profession_asset = (ProfessionAsset) null;
    this._next_step_tile = (WorldTile) null;
    this.asset = (ActorAsset) null;
    this.frame_data = (AnimationFrameData) null;
    this.animation_container = (AnimationContainerUnit) null;
    this._home_building = (Building) null;
    this.cached_sprite_head = (Sprite) null;
    this._cached_sprite_item = (Sprite) null;
    this._cached_hand_renderer_asset = (IHandRenderer) null;
    this._aggression_targets.Clear();
    this._current_children = 0;
    this.is_forced_socialize_icon = false;
    this.is_forced_socialize_timestamp = 0.0;
    base.Dispose();
  }

  private void checkComponentListDispose(List<BaseActorComponent> pList)
  {
    if (pList == null)
      return;
    for (int index = 0; index < pList.Count; ++index)
      pList[index].Dispose();
    pList.Clear();
  }

  private void checkSimpleComponentListDispose(List<ActorSimpleComponent> pList)
  {
    if (pList == null)
      return;
    for (int index = 0; index < pList.Count; ++index)
      pList[index].Dispose();
    pList.Clear();
  }

  public void showTooltip(object pUiObject)
  {
    string pType = !this.isKing() ? (!this.isCityLeader() ? "actor" : "actor_leader") : "actor_king";
    Tooltip.show(pUiObject, pType, new TooltipData()
    {
      actor = this
    });
  }

  public override ColorAsset getColor() => this.kingdom.getColor();

  public void setHoverState(HoverState pState) => this._hover_state = pState;

  public override string ToString()
  {
    if (this.data == null)
      return "[Actor is null]";
    using (StringBuilderPool stringBuilderPool1 = new StringBuilderPool())
    {
      stringBuilderPool1.Append($"[Actor:{this.id} ");
      if (!this.isAlive())
        stringBuilderPool1.Append("[DEAD] ");
      if (!string.IsNullOrEmpty(this.data.name))
        stringBuilderPool1.Append(this.data.name + " ");
      if (this.hasCity())
      {
        stringBuilderPool1.Append($"City:{this.city.getID()} ");
        if (this.city.kingdom != this.kingdom)
        {
          StringBuilderPool stringBuilderPool2 = stringBuilderPool1;
          Kingdom kingdom = this.city.kingdom;
          string str = $"CityKingdom:{(kingdom != null ? kingdom.getID() : -1L)} ";
          stringBuilderPool2.Append(str);
        }
        if (this.city.hasArmy())
          stringBuilderPool1.Append($"CityArmy:{this.city.army.id} ");
      }
      if (this.hasKingdom())
        stringBuilderPool1.Append($"Kingdom:{this.kingdom.getID()} ");
      if (this.isKing())
        stringBuilderPool1.Append("isKing ");
      if (this.isCityLeader())
        stringBuilderPool1.Append("isCityLeader ");
      if (this.hasArmy())
      {
        stringBuilderPool1.Append($"Army:{this.army.id} ");
        if (this.isArmyGroupLeader())
          stringBuilderPool1.Append("isArmyGroupLeader ");
        if (this.isArmyGroupWarrior())
          stringBuilderPool1.Append("isArmyGroupWarrior ");
      }
      return stringBuilderPool1.ToString().Trim() + "]";
    }
  }

  private int getMaxPossibleLevel() => 9999;

  internal void addExperience(int pValue)
  {
    if (pValue == 0 || !this.asset.can_level_up || !this.isAlive())
      return;
    if (this.hasCulture() && this.culture.hasTrait("fast_learners"))
      pValue *= CultureTraitLibrary.getValue("fast_learners");
    int maxPossibleLevel = this.getMaxPossibleLevel();
    if (this.data.level >= maxPossibleLevel)
      return;
    this.data.experience += pValue;
    if (this.data.experience >= this.getExpToLevelup())
      this.levelUp();
    if (this.data.level < maxPossibleLevel)
      return;
    this.data.experience = this.getExpToLevelup();
  }

  public void addRenown(int pValue) => this.data.renown += pValue;

  public void addRenown(int pAmount, float pPercent)
  {
    this.addRenown((int) ((double) pAmount * (double) pPercent));
  }

  internal void updateAge()
  {
    this.checkGrowthEvent();
    float age = (float) this.getAge();
    if (this.hasSubspecies())
    {
      WorldAction actionsActorGrowth = this.subspecies.all_actions_actor_growth;
      if (actionsActorGrowth != null)
      {
        int num = actionsActorGrowth((BaseSimObject) this, this.current_tile) ? 1 : 0;
      }
      this.updateAttributes();
    }
    if (this.hasCity())
    {
      if (this.isKing())
        this.addExperience(20);
      if (this.isCityLeader())
        this.addExperience(10);
    }
    if (this.isSapient() && (double) age > 300.0 && this.hasTrait("immortal") && Randy.randomBool())
      this.addTrait("evil");
    if ((double) age <= 40.0 || !Randy.randomChance(0.3f))
      return;
    this.addTrait("wise");
  }

  private void updateAttributes()
  {
    if (!Randy.randomChance(0.3f))
      return;
    string possibleAttribute = this.subspecies.getPossibleAttribute();
    if (string.IsNullOrEmpty(possibleAttribute))
      return;
    this.data[possibleAttribute]++;
  }

  public void setMaxHappiness() => this.setHappiness(this.getMaxHappiness());

  public void setHappiness(int pValue, bool pClamp = true)
  {
    if (pClamp)
      pValue = Math.Clamp(pValue, this.getMinHappiness(), this.getMaxHappiness());
    this.data.happiness = pValue;
  }

  public void restoreHealthPercent(float pVal)
  {
    if ((double) pVal <= 0.0 || this.hasMaxHealth())
      return;
    this.restoreHealth(this.getMaxHealthPercent(pVal));
  }

  public void restoreHealth(int pVal)
  {
    if (this.hasMaxHealth())
      return;
    this.changeHealth(pVal);
  }

  public bool changeHappiness(string pID, int pValue = 0)
  {
    if (!this.hasEmotions() || this.isEgg())
      return false;
    HappinessAsset happinessAsset = AssetManager.happiness_library.get(pID);
    if (happinessAsset.ignored_by_psychopaths && this.hasTrait("psychopath"))
      return false;
    int num = pValue + happinessAsset.value;
    this.setHappiness(Mathf.Clamp(this.getHappiness() + num, this.getMinHappiness(), this.getMaxHappiness()));
    if (happinessAsset.show_change_happiness_effect)
    {
      if (num > 0)
        EffectsLibrary.showMetaEventEffect("fx_change_happiness_positive", this);
      else if (num < 0)
        EffectsLibrary.showMetaEventEffect("fx_change_happiness_negative", this);
    }
    this._last_happiness_history.Enqueue(new HappinessHistory()
    {
      index = happinessAsset.index,
      timestamp = World.world.getCurWorldTime(),
      bonus = pValue
    });
    if (this._last_happiness_history.Count > 20)
      this._last_happiness_history.Dequeue();
    return true;
  }

  public void spendNutritionOnBirth()
  {
    this.decreaseNutrition(SimGlobals.m.nutrition_cost_new_baby);
  }

  public void addNutritionFromEating(int pVal = 100, bool pSetMaxNutrition = false, bool pSetJustAte = false)
  {
    if (pSetMaxNutrition)
      this.setNutrition(this.getMaxNutrition());
    else
      this.setNutrition(Math.Min(this.getMaxNutrition(), this.data.nutrition + pVal));
    if (!pSetJustAte)
      return;
    this.justAte();
  }

  public void updateNutritionDecay(bool pDoStarvationDamage = true)
  {
    this.decreaseNutrition(this.subspecies.getMetabolicRate());
    if (this.isStarving())
    {
      this.setNutrition(0);
      if (!pDoStarvationDamage)
        return;
      this.getHit((float) this.getMaxHealthPercent(SimGlobals.m.starvation_damage_multiplier), pAttackType: AttackType.Starvation);
      if (!this.isAlive())
        return;
      this.addStatusEffect("starving", pColorEffect: false);
    }
    else
    {
      this.updateStamina();
      this.updateMana();
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void decreaseNutrition(int pValue = -1) => this.setNutrition(this.getNutrition() - pValue);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void setNutrition(int pVal, bool pClamp = true)
  {
    if (pClamp)
      pVal = Math.Clamp(pVal, 0, this.getMaxNutrition());
    this.data.nutrition = pVal;
  }

  public void updateMana()
  {
    if (this.isManaFull())
      return;
    this.addMana(SimGlobals.m.mana_change);
  }

  public void addMana(int pValue)
  {
    int maxMana = this.getMaxMana();
    int mana = this.getMana();
    if (mana < maxMana)
      mana += pValue;
    this.setMana(Math.Clamp(mana, 0, maxMana));
  }

  public int getMaxManaPercent(float pPercent)
  {
    return Mathf.Max(1, (int) ((double) this.getMaxMana() * (double) pPercent));
  }

  public void restoreManaPercent(float pVal)
  {
    if ((double) pVal <= 0.0 || this.hasMaxMana())
      return;
    this.restoreMana(this.getMaxManaPercent(pVal));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void changeMana(int pValue)
  {
    this.data.mana = Mathf.Clamp(this.data.mana + pValue, 0, this.getMaxMana());
  }

  public void restoreMana(int pVal)
  {
    if (this.hasMaxMana())
      return;
    this.changeMana(pVal);
  }

  public void setMana(int pValue, bool pClamp = true)
  {
    if (pClamp)
      pValue = Math.Clamp(pValue, 0, this.getMaxMana());
    this.data.mana = pValue;
  }

  public void spendMana(int pValueSpend)
  {
    if (pValueSpend == 0)
      return;
    this.setMana(this.getMana() - pValueSpend);
  }

  public int getMaxStaminaPercent(float pPercent)
  {
    return Mathf.Max(1, (int) ((double) this.getMaxStamina() * (double) pPercent));
  }

  public void restoreStaminaPercent(float pVal)
  {
    if ((double) pVal <= 0.0 || this.isStaminaFull())
      return;
    this.restoreStamina(this.getMaxStaminaPercent(pVal));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void changeStamina(int pValue)
  {
    this.data.stamina = Mathf.Clamp(this.data.stamina + pValue, 0, this.getMaxStamina());
  }

  public void restoreStamina(int pVal)
  {
    if (this.isStaminaFull())
      return;
    this.changeStamina(pVal);
  }

  public void updateStamina()
  {
    if (this.isStaminaFull())
      return;
    this.addStamina(SimGlobals.m.stamina_change);
  }

  public void addStamina(int pValue)
  {
    int maxStamina = this.getMaxStamina();
    int stamina = this.getStamina();
    if (stamina < maxStamina)
      stamina += pValue;
    this.setStamina(Math.Clamp(stamina, 0, maxStamina));
  }

  public void setStamina(int pValue, bool pClamp = true)
  {
    if (pClamp)
      pValue = Math.Clamp(pValue, 0, this.getMaxStamina());
    this.data.stamina = pValue;
  }

  public void spendStamina(int pValueSpend)
  {
    if (pValueSpend == 0)
      return;
    this.setStamina(this.getStamina() - pValueSpend);
  }

  public void spendStaminaWithCooldown(int pValueSpend)
  {
    if (pValueSpend == 0 || this.isUnderStaminaCooldown())
      return;
    this._last_stamina_reduce_timestamp = World.world.getCurSessionTime();
    this.setStamina(this.getStamina() - pValueSpend);
  }

  public bool hasHappinessEntry(string pID, float pTime = 0.0f)
  {
    if (!this.hasHappinessHistory())
      return false;
    foreach (HappinessHistory happinessHistory in this.happiness_change_history)
    {
      if (!(happinessHistory.asset.id != pID) && ((double) pTime == 0.0 || happinessHistory.elapsedSince() < (double) pTime))
        return true;
    }
    return false;
  }

  public bool is_invincible => this._has_status_invincible;

  public void finishScale() => this.setActorScale(this.target_scale);

  public void setActorScale(float pVal)
  {
    this.actor_scale = pVal;
    ((Vector3) ref this.current_scale).Set(this.actor_scale, this.actor_scale, 1f);
  }

  public void setData(ActorData pData) => this.data = pData;

  public void loadData(ActorData pData)
  {
    this.setData(pData);
    pData.load();
  }

  public void generateSex()
  {
    if (Randy.randomBool())
      this.data.sex = ActorSex.Male;
    else
      this.data.sex = ActorSex.Female;
  }

  protected void generatePersonality()
  {
    if (this.hasSubspecies())
    {
      foreach (ActorTrait trait in (IEnumerable<ActorTrait>) this.subspecies.getActorBirthTraits().getTraits())
        this.addTrait(trait);
      if (this.subspecies.hasPhenotype())
        this.generatePhenotypeAndShade();
    }
    else
      this.generateRandomSpawnTraits(this.asset);
    if (this.isAdult())
      this.checkTraitMutationGrowUp();
    this.checkTraitMutationOnBirth();
    this.generateSex();
    this.setStatsDirty();
  }

  public void calcIsEgg()
  {
    if (!this.hasSubspecies() || !this.subspecies.has_egg_form)
      return;
    this._state_egg = this.hasStatus("egg");
  }

  public void calcIsBaby()
  {
    if (!this.hasSubspecies() || !this.asset.has_baby_form || (double) this.getAge() >= (double) this.subspecies.age_adult)
      return;
    this._state_baby = true;
    this.clearSprites();
  }

  public void setCheckLanding() => this.should_check_land_cancel = true;

  public void addForce(
    float pX,
    float pY,
    float pHeight,
    bool pCheckLandCancelAllActions = false,
    bool pIgnorePosHeight = false)
  {
    if (!this.asset.can_be_moved_by_powers)
      return;
    if (pCheckLandCancelAllActions)
      this.setCheckLanding();
    if (!pIgnorePosHeight && (double) this.position_height > 0.0)
      return;
    this.velocity.x = pX;
    this.velocity.y = pY;
    this.velocity.z = pHeight;
    this.velocity_speed = pHeight;
    this.under_forces = true;
  }

  public void setFlying(bool pVal)
  {
    this._flying = pVal;
    if (pVal)
      this.hitbox_bonus_height = 8f;
    else
      this.hitbox_bonus_height = 2f;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal void checkIsInLiquid()
  {
    this._is_in_liquid = this.current_tile.is_liquid && (double) this.move_jump_offset.y == 0.0 && (double) this.position_height <= 0.0 && this.isAlive();
  }

  private void addDefaultItemAttackActions(ItemAsset pItemAsset)
  {
    this.addItemActions(pItemAsset);
    if (pItemAsset.action_attack_target == null)
      return;
    this.s_action_attack_target += pItemAsset.action_attack_target;
  }

  private void addItemActions(ItemAsset pItemAsset)
  {
    if (pItemAsset.action_special_effect == null)
      return;
    this._s_special_effect_augmentations.Add((BaseAugmentationAsset) pItemAsset);
  }

  internal void attackTargetActions(BaseSimObject pTarget, WorldTile pTile)
  {
    AttackAction actionAttackTarget = this.s_action_attack_target;
    if (actionAttackTarget == null)
      return;
    int num = actionAttackTarget((BaseSimObject) this, pTarget, pTile) ? 1 : 0;
  }

  protected void calcAgeStates()
  {
    this._state_egg = false;
    this._state_baby = false;
    this._state_adult = false;
    this.calcIsEgg();
    if (!this.isEgg())
    {
      this.calcIsBaby();
      if (this.isBaby())
        return;
      this._state_adult = true;
      this.clearSprites();
    }
    else
    {
      this._state_baby = true;
      this.clearSprites();
    }
  }

  internal override void updateStats()
  {
    // ISSUE: unable to decompile the method.
  }

  public void resetAttackTimeout() => this.attack_timer = 0.0f;

  public void setActionTimeout(float pTimeout) => this.attack_timer = pTimeout;

  private void addSpecialEffectAugmentations(IEnumerable<BaseAugmentationAsset> pAssets)
  {
    foreach (BaseAugmentationAsset pAsset in pAssets)
    {
      if (pAsset.action_special_effect != null)
        this._s_special_effect_augmentations.Add(pAsset);
      if (pAsset.action_attack_target != null)
        this.s_action_attack_target += pAsset.action_attack_target;
    }
  }

  private void addSpecialEffectsFromMetas(List<BaseAugmentationAsset> pAugmentations)
  {
    if (pAugmentations == null || pAugmentations.Count == 0)
      return;
    this._s_special_effect_augmentations.AddRange((IEnumerable<BaseAugmentationAsset>) pAugmentations);
  }

  private void calculateOffspringBasedOnAge()
  {
    if (this.hasTrait("immortal"))
      return;
    int stat = (int) this.stats["offspring"];
    float ageRatio = this.getAgeRatio();
    float num = 1f;
    if ((double) ageRatio > 0.89999997615814209)
      num = 0.1f;
    else if ((double) ageRatio > 0.699999988079071)
      num = 0.2f;
    else if ((double) ageRatio > 0.5)
      num = 0.5f;
    else if ((double) ageRatio > 0.30000001192092896)
      num = 0.8f;
    this.stats["offspring"] = (float) (int) Math.Ceiling((double) stat * (double) num);
  }

  internal virtual void updateFall()
  {
    if ((double) this.position_height < 0.0)
      return;
    float elapsed = World.world.elapsed;
    this.position_height -= SimGlobals.m.gravity * this.stats.get("mass") * elapsed;
    if ((double) this.position_height > 0.0)
      return;
    this.position_height = 0.0f;
    if (this.under_forces)
      return;
    this.stopForce();
  }

  private void stopForce()
  {
    this.position_height = 0.0f;
    this.velocity = Vector3.zero;
    this.under_forces = false;
    this.batch.c_action_landed.Add(this);
  }

  internal virtual void actionLanded()
  {
    this.batch.c_action_landed.Remove(this);
    this.dirty_current_tile = true;
    BaseActionActor callbacksLanded = this.callbacks_landed;
    if (callbacksLanded != null)
      callbacksLanded(this);
    if (this._action_wait_after_land)
    {
      this._action_wait_after_land = false;
      this.makeWait(this._action_wait_after_land_timer);
    }
    this.checkStepActionForTile(this.current_tile);
  }

  public void updateShake(float pElapsed)
  {
    if (!this._shake_active)
      return;
    this._shake_timer -= pElapsed;
    if ((double) this._shake_timer <= 0.0)
    {
      ((Vector2) ref this.shake_offset).Set(0.0f, 0.0f);
      this._shake_active = false;
      this.batch.c_shake.Remove(this);
    }
    else
    {
      if (this._shake_vertical)
        this.shake_offset.y = ((Random) ref this.batch.rnd).NextFloat(-this._shake_volume, this._shake_volume);
      if (!this._shake_horizontal)
        return;
      this.shake_offset.x = ((Random) ref this.batch.rnd).NextFloat(-this._shake_volume, this._shake_volume);
    }
  }

  internal void updateFlipRotation(float pElapsed)
  {
    if (!this.asset.can_flip)
      return;
    if (this.flip)
    {
      this.flip_angle += pElapsed * 600f;
      if ((double) this.flip_angle > 180.0)
        this.flip_angle = 180f;
    }
    else
    {
      this.flip_angle -= pElapsed * 600f;
      if ((double) this.flip_angle < 0.0)
        this.flip_angle = 0.0f;
    }
    this.target_angle.y = this.flip_angle;
  }

  internal bool flipAnimationActive()
  {
    if (!this.asset.can_flip)
      return false;
    return this.flip ? (double) this.flip_angle != 180.0 : (double) this.flip_angle != 0.0;
  }

  private void updateRotations(float pElapsed)
  {
    if ((double) this.rotation_cooldown > 0.0)
      this.rotation_cooldown -= pElapsed;
    else if (this.is_unconscious)
      this.updateRotationFall(pElapsed);
    else
      this.updateRotationBack(pElapsed);
  }

  private void updateRotationFall(float pElapsed)
  {
    if (this.getTextureAsset().prevent_unconscious_rotation)
      return;
    if (this.current_tile.is_liquid && this._is_in_liquid)
    {
      this.target_angle.z = 0.0f;
    }
    else
    {
      if (this._rotation_direction == RotationDirection.Left && (double) this.target_angle.z != -90.0)
      {
        this.target_angle.z -= 230f * pElapsed;
        if ((double) this.target_angle.z < -90.0)
          this.target_angle.z = -90f;
      }
      if (this._rotation_direction != RotationDirection.Right || (double) this.target_angle.z == 90.0)
        return;
      this.target_angle.z += 300f * pElapsed;
      if ((double) this.target_angle.z <= 90.0)
        return;
      this.target_angle.z = 90f;
    }
  }

  private void updateRotationBack(float pElapsed)
  {
    if ((double) this.target_angle.z == 0.0)
      return;
    if ((double) this.target_angle.z < 0.0)
    {
      this.target_angle.z += 300f * pElapsed;
      if ((double) this.target_angle.z > 0.0)
        this.target_angle.z = 0.0f;
    }
    if ((double) this.target_angle.z <= 0.0)
      return;
    this.target_angle.z -= 300f * pElapsed;
    if ((double) this.target_angle.z >= 0.0)
      return;
    this.target_angle.z = 0.0f;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Vector3 updateRotation()
  {
    if ((double) this.current_rotation.y == (double) this.target_angle.y && (double) this.current_rotation.z == (double) this.target_angle.z)
      return this.current_rotation;
    ((Vector3) ref this.current_rotation).Set(this.target_angle.x, this.target_angle.y, this.target_angle.z);
    return this.current_rotation;
  }

  internal void updateDeadBlackAnimation(float pElapsed)
  {
    if ((double) this._death_timer_color_stage_1 > 0.0)
    {
      this._death_timer_color_stage_1 -= pElapsed;
      if ((double) this._death_timer_color_stage_1 <= 0.0)
        this._death_timer_color_stage_1 = 0.0f;
    }
    if ((double) this._death_timer_color_stage_1 > 0.0)
    {
      Color color;
      // ISSUE: explicit constructor call
      ((Color) ref color).\u002Ector(this._death_timer_color_stage_1, this._death_timer_color_stage_1, this._death_timer_color_stage_1, 1f);
      this.color = color;
    }
    else
    {
      if ((double) this._death_timer_alpha_stage_2 <= 0.0)
        return;
      this._death_timer_alpha_stage_2 -= 1f * pElapsed;
      if ((double) this._death_timer_alpha_stage_2 <= 0.0)
      {
        this.die(true, AttackType.None, false);
      }
      else
      {
        Color color;
        // ISSUE: explicit constructor call
        ((Color) ref color).\u002Ector(this._death_timer_color_stage_1, this._death_timer_color_stage_1, this._death_timer_color_stage_1, this._death_timer_alpha_stage_2);
        this.color = color;
      }
    }
  }

  internal virtual void spawnOn(WorldTile pTile, float pZHeight = 0.0f)
  {
    this.setCurrentTilePosition(pTile);
    this.position_height = pZHeight;
    this.hitbox_bonus_height = this.asset.default_height;
  }

  public string getName()
  {
    if (string.IsNullOrEmpty(this.data.name))
    {
      this.generateNewName();
      AchievementLibrary.child_named_toto.checkBySignal((object) this.data.name);
    }
    return this.data.name;
  }

  public string generateName(MetaType pType, long pSeed, ActorSex pSex = ActorSex.None)
  {
    return NameGenerator.generateName(this, pType, pSeed + World.world.map_stats.life_dna, pSex);
  }

  public override string name
  {
    get => this.getName();
    protected set => this.data.name = value;
  }

  private void generateNewName()
  {
    ActorSex pSex = this.isSapient() ? this.data.sex : ActorSex.None;
    this.setName(NameGenerator.generateName(this, MetaType.Unit, World.world.map_stats.life_dna + this.getID() * 543L, pSex));
    ActorData data = this.data;
    Culture culture = this.culture;
    long num = culture != null ? culture.id : -1L;
    data.name_culture_id = num;
  }

  public override void trackName(bool pPostChange = false)
  {
    if (string.IsNullOrEmpty(this.data.name) || pPostChange && (this.data.past_names == null || this.data.past_names.Count == 0))
      return;
    ActorData data = this.data;
    if (data.past_names == null)
      data.past_names = new List<NameEntry>();
    if (this.data.past_names.Count == 0)
    {
      this.data.past_names.Add(new NameEntry(this.data.name, false, this.data.created_time));
    }
    else
    {
      if (this.data.past_names.Last<NameEntry>().name == this.data.name)
        return;
      this.data.past_names.Add(new NameEntry(this.data.name, this.data.custom_name));
    }
  }

  public void setHomeBuilding(Building pBuilding)
  {
    if (this._home_building != null)
      this.clearHomeBuilding();
    this._home_building = pBuilding;
    this._home_building.residents.Add(this.data.id);
    World.world.buildings.event_houses = true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasHomeBuilding() => this.getHomeBuilding() != null;

  public Building home_building => this._home_building;

  public Building getHomeBuilding()
  {
    this.checkHomeBuilding();
    return this._home_building;
  }

  public void checkHomeBuilding()
  {
    if (this._home_building == null)
      return;
    if (!this._home_building.isUsable() || this._home_building.isAbandoned())
    {
      this.clearHomeBuilding();
      this.changeHappiness("just_lost_house");
    }
    else
    {
      if (!this._home_building.asset.city_building || this._home_building.city == this.city)
        return;
      this.clearHomeBuilding();
      this.changeHappiness("just_lost_house");
    }
  }

  public void cloneTopicSprite(Sprite pSprite) => this._last_topic_sprite = pSprite;

  public void clearLastTopicSprite() => this._last_topic_sprite = (Sprite) null;

  public Sprite getTopicSpriteTrait()
  {
    return this.traits.Count == 0 ? (Sprite) null : this.traits.GetRandom<ActorTrait>().getSprite();
  }

  public Sprite getSocializeTopic()
  {
    if (Object.op_Equality((Object) this._last_topic_sprite, (Object) null))
      this._last_topic_sprite = AssetManager.communication_topic_library.getTopicSprite(this);
    return this._last_topic_sprite;
  }

  public void forceSocializeTopic(string pPath)
  {
    this._last_topic_sprite = SpriteTextureLoader.getSprite(pPath);
    this.is_forced_socialize_timestamp = World.world.getCurWorldTime();
  }

  public void clearHomeBuilding()
  {
    this._home_building = (Building) null;
    World.world.buildings.event_houses = true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override void setAlive(bool pValue)
  {
    this._alive = pValue;
    if (!pValue && this.data.died_time == 0.0)
      this.data.died_time = World.world.getCurWorldTime();
    if (pValue)
      return;
    World.world.units.somethingChanged();
  }

  internal bool isProfession(UnitProfession pType) => this._profession == pType;

  public bool isAnimal()
  {
    return !this.isSapient() && !this.asset.unit_other && this.asset.default_animal;
  }

  public bool isNomad() => !this.isKingdomCiv();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isSapient() => this.hasSubspecies() && this.subspecies.isSapient();

  public bool isPrettyOld()
  {
    int age = this.getAge();
    return age > 1 && (double) age >= (double) this.subspecies.age_adult && (double) this.getAgeRatio() > 0.699999988079071;
  }

  public bool isBaby() => this._state_baby;

  public bool isAdult() => this._state_adult;

  public bool isBreedingAge()
  {
    return this.hasSubspecies() && (double) this.getAge() >= (double) this.subspecies.age_breeding;
  }

  public bool isEgg() => this._state_egg;

  public int getAge() => this.data.getAge();

  public int age => this.getAge();

  public string getBirthday() => Date.getDate(this.data.created_time);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isKing() => this.hasKingdom() && this.kingdom.king == this;

  public float getMaturationTimeSeconds() => this.getMaturationTimeMonths() * 5f;

  public float getMaturationTimeMonths()
  {
    float maturationTimeMonths = 0.0f;
    if (this.hasSubspecies())
      maturationTimeMonths += this.subspecies.getMaturationTimeMonths();
    return maturationTimeMonths;
  }

  public bool is_army_captain => this.hasArmy() && this.army.getCaptain() == this;

  public bool isFavorite() => this.data.favorite;

  public void switchFavorite() => this.data.favorite = !this.data.favorite;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override City getCity() => this.city;

  public bool canBuildNewCity()
  {
    return !this.current_zone.hasCity() && !this.hasCity() && this.current_zone.isGoodForNewCity(this);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isCityLeader() => this.hasCity() && this.city.leader == this;

  public override bool hasDied() => this.data.died_time > 0.0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isPollinator()
  {
    Subspecies subspecies = this.subspecies;
    return subspecies != null && subspecies.has_trait_pollinating;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isAffectedByLiquid() => !this.isInAir() && this._is_in_liquid;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal override bool isInAir() => this._flying || this.isHovering() || this.isInMagnet();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal override bool isFlying() => this._flying;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool ignoresBlocks() => this.asset.ignore_blocks || this.isFlying() || this.isInMagnet();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isInMagnet() => this.is_in_magnet;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isHovering()
  {
    Subspecies subspecies = this.subspecies;
    return subspecies != null && subspecies.has_trait_hovering;
  }

  public ActorAsset getActorAsset() => this.asset;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IReadOnlyCollection<ActorTrait> getTraits()
  {
    return (IReadOnlyCollection<ActorTrait>) this.traits;
  }

  public bool isWaterCreature()
  {
    if (this.asset.force_ocean_creature)
      return true;
    Subspecies subspecies = this.subspecies;
    return subspecies != null && subspecies.has_trait_water_creature;
  }

  public bool mustAvoidGround() => this.isWaterCreature() && !this.asset.force_land_creature;

  public bool isInStablePlace()
  {
    if (this.mustAvoidGround())
    {
      if (this.current_tile.Type.ground)
        return false;
    }
    else if (this.current_tile.Type.ocean && !this.isWaterCreature() || this.current_tile.Type.lava && this.asset.die_in_lava)
      return false;
    return true;
  }

  internal bool hasWeapon() => this.canUseItems() && !this.equipment.weapon.isEmpty();

  internal Item getWeapon() => this.hasWeapon() ? this.equipment.weapon.getItem() : (Item) null;

  internal EquipmentAsset getWeaponAsset()
  {
    return this.hasWeapon() ? this.equipment.weapon.getItem().getAsset() : AssetManager.items.get(this.asset.default_attack);
  }

  public bool isWeaponFirearm() => this.getWeapon()?.getAsset().group_id == "firearm";

  public bool isArmyGroupLeader() => this.hasArmy() && this.army.getCaptain() == this;

  public bool isArmyGroupWarrior() => this.hasArmy() && this.army.getCaptain() != this;

  public bool hasTraits() => this.traits.Count > 0;

  public bool is_profession_nothing => this._profession == UnitProfession.Nothing;

  public bool is_profession_king => this._profession == UnitProfession.King;

  public bool is_profession_leader => this._profession == UnitProfession.Leader;

  public bool is_profession_warrior => this._profession == UnitProfession.Warrior;

  public bool is_profession_citizen => this._profession == UnitProfession.Unit;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isSexMale() => this.data.sex == ActorSex.Male;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isSexFemale() => this.data.sex == ActorSex.Female;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasEquipment() => this.equipment != null;

  public bool hasHouse() => this.getHomeBuilding() != null;

  public bool hasLover() => this.lover != null;

  public bool hasBestFriend() => this.getBestFriend() != null;

  public Actor getBestFriend()
  {
    return this.data.best_friend_id.hasValue() ? World.world.units.get(this.data.best_friend_id) : (Actor) null;
  }

  public bool isChildOf(Actor pActor) => this.isChildOf(pActor.data.id);

  public bool isChildOf(long pID) => this.data.parent_id_1 == pID || this.data.parent_id_2 == pID;

  public bool isParentOf(long pID, Actor pActor)
  {
    return pID == pActor.data.parent_id_1 || pID == pActor.data.parent_id_2;
  }

  public bool isParentOf(Actor pActor) => this.isParentOf(this.data.id, pActor);

  public IEnumerable<Actor> getParents()
  {
    Actor parent1 = World.world.units.get(this.data.parent_id_1);
    if (parent1 != null && parent1.isAlive())
      yield return parent1;
    Actor parent2 = World.world.units.get(this.data.parent_id_2);
    if (parent2 != null && parent2.isAlive())
      yield return parent2;
  }

  public IEnumerable<Actor> getChildren(bool pOnlyCurrentFamily = true)
  {
    Actor pActor = this;
    if (pOnlyCurrentFamily)
    {
      if (pActor.hasFamily())
      {
        foreach (Actor unit in pActor.family.units)
        {
          if (unit != pActor && unit.isChildOf(pActor))
            yield return unit;
        }
      }
    }
    else
    {
      int tCurrentLivingChildren = pActor.current_children_count;
      if (tCurrentLivingChildren != 0)
      {
        long tParentID = pActor.data.id;
        if (pActor.hasSubspecies() && !pActor.subspecies.isRekt())
        {
          foreach (Actor unit in pActor.subspecies.units)
          {
            if (!unit.isRekt() && unit != pActor && unit.isChildOf(tParentID))
            {
              --tCurrentLivingChildren;
              yield return unit;
              if (tCurrentLivingChildren == 0)
                break;
            }
          }
        }
      }
    }
  }

  public bool hasSuitableBookTraits()
  {
    foreach (BaseAugmentationAsset trait in (IEnumerable<ActorTrait>) this.getTraits())
    {
      if (trait.group_id == "mind")
        return true;
    }
    return false;
  }

  public bool canBeSurprised(WorldTile pFromTile = null)
  {
    return this._has_emotions && this.asset.can_be_surprised && !this.isFighting() && !this.isInsideSomething() && !this.is_unconscious && !this.isEgg() && (!this.hasTask() || !this.ai.task.ignore_fight_check);
  }

  public bool is_looking_left => !this.flip;

  public bool isTileOnTheLeft(WorldTile pTile) => this.current_tile.x > pTile.x;

  public bool isFighting() => this.has_attack_target;

  public UnitProfession getProfession() => this._profession;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int getNutrition() => this.data.nutrition;

  public bool isHungry()
  {
    return this.needsFood() && (double) this.getNutritionRatio() <= (double) SimGlobals.m.nutrition_level_hungry;
  }

  public float getNutritionRatio() => (float) this.getNutrition() / (float) this.getMaxNutrition();

  public float getHealthRatio() => (float) this.getHealth() / (float) this.getMaxHealth();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasMaxHealth() => this.getHealth() >= this.getMaxHealth();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasMaxMana() => this.getMana() >= this.getMaxMana();

  public bool isStarving() => this.getNutrition() == 0;

  public bool hasFavoriteFood() => !string.IsNullOrEmpty(this.data.favorite_food);

  public ResourceAsset favorite_food_asset => AssetManager.resources.get(this.data.favorite_food);

  public bool hasEmotions() => this._has_emotions;

  public bool canHavePrejudice() => this.hasEmotions();

  public bool hasHappinessHistory() => this._last_happiness_history.Count > 0;

  public bool isUnhappy()
  {
    return this.hasEmotions() && (double) this.getHappinessRatio() < 0.30000001192092896;
  }

  public int getHappiness() => this.data.happiness;

  public bool isHappy()
  {
    return !this.hasEmotions() || (double) this.getHappinessRatio() >= 0.60000002384185791;
  }

  public int getMinHappiness() => -100;

  public int getMaxHappiness() => 100;

  public float getHappinessRatio() => (float) (((double) this.getHappiness() + 100.0) / 200.0);

  internal bool isSameSpecies(string pID) => this.asset.id == pID;

  internal bool isSameSpecies(Actor pActor) => this.isSameSpecies(pActor.asset.id);

  internal bool isSameSubspecies(Subspecies pSubspecies) => this.subspecies == pSubspecies;

  public bool isAllowedToLookForEnemies()
  {
    return !this.shouldSkipFightCheck() && (!this.hasTask() || !this.ai.task.ignore_fight_check) && !this._has_trait_peaceful && !this.isInsideSomething() && (this.kingdom.asset.units_always_looking_for_enemies || !this.isBaby());
  }

  private bool shouldSkipFightCheck()
  {
    return this.asset.skip_fight_logic || this._ignore_fights || this.asset.is_boat && this.getSimpleComponent<Boat>().hasPassengers();
  }

  public bool isInWaterAndCantAttack() => !this.isWaterCreature() && this.isAffectedByLiquid();

  public bool hasReachedOffspringLimit() => this.current_children_count >= this.getMaxOffspring();

  public int getMaxOffspring() => (int) Math.Ceiling((double) this.stats["offspring"]);

  public bool haveNutritionForNewBaby()
  {
    return !this.needsFood() || this.getNutrition() >= SimGlobals.m.nutrition_cost_new_baby;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isInsideSomething() => this.is_inside_building || this.is_inside_boat;

  public bool isOnSameIsland(Actor pActor) => this.current_tile.isSameIsland(pActor.current_tile);

  public bool hasSameCity(Actor pActorTarget) => this.hasCity() && this.city == pActorTarget.city;

  public bool canBreed()
  {
    return this.isAlive() && this.isBreedingAge() && this.haveNutritionForNewBaby() && !this.hasStatus("pregnant") && !this.hasStatus("afterglow");
  }

  public bool canProduceBabies() => !this.hasTrait("infertile");

  public bool isPlacePrivateForBreeding()
  {
    int num1 = Toolbox.countUnitsInChunk(this.current_tile);
    if (!this.hasCity())
      return this.asset.animal_breeding_close_units_limit > num1;
    int num2 = this.city.getPopulationMaximum() * 2 + 10;
    return this.city.countUnits() < num2;
  }

  public bool isOnGround()
  {
    if (this.is_immovable || this.is_unconscious)
      return true;
    if (!this.hasTask())
      return false;
    BehaviourActionActor action = this.ai.action;
    return action != null && action.land_if_hovering;
  }

  internal bool isInAttackRange(BaseSimObject pObject)
  {
    float num1 = this.getAttackRange() + pObject.stats["size"];
    float num2 = num1 * num1;
    return (double) Toolbox.SquaredDistVec2Float(this.current_position, pObject.current_position) < (double) num2;
  }

  internal bool isAttackReady() => (double) this.attack_timer <= 0.0;

  public float getAttackCooldownRatio()
  {
    float attackCooldown = this.getAttackCooldown();
    return (double) attackCooldown == 0.0 ? 1f : this.attack_timer / attackCooldown;
  }

  internal bool isAttackPossible()
  {
    return this.isAttackReady() && (double) this.current_rotation.z == 0.0;
  }

  public bool canUseSpells()
  {
    return !this.hasStatus("spell_silence") && !this.hasSpellCastCooldownStatus();
  }

  public bool hasSpells()
  {
    return this._spells.hasAny() || this.hasSubspecies() && this.subspecies.spells.hasAny() || this.canUseReligionSpells() && this.religion.spells.hasAny() || this.asset.hasDefaultSpells();
  }

  public bool canUseReligionSpells()
  {
    if (!this.hasReligion() || !this.religion.spells.hasAny() || this.hasTrait("mute"))
      return false;
    return this.hasClan() ? !this.clan.hasTrait("void_ban") : !this.religion.is_magic_only_clan_members;
  }

  public SpellAsset getRandomSpell()
  {
    using (ListPool<SpellAsset> list = new ListPool<SpellAsset>())
    {
      if (this._spells.hasAny())
        list.Add(this._spells.getRandomSpell());
      if (this.hasSubspecies() && this.subspecies.spells.hasAny())
        list.Add(this.subspecies.spells.getRandomSpell());
      if (this.canUseReligionSpells())
        list.Add(this.religion.spells.getRandomSpell());
      if (this.asset.hasDefaultSpells())
        list.Add(this.asset.spells.getRandomSpell());
      return list.Count == 0 ? (SpellAsset) null : list.GetRandom<SpellAsset>();
    }
  }

  internal override float getHeight() => this.position_height + this.hitbox_bonus_height;

  public float getScaleMod() => this.actor_scale / 0.1f;

  public bool isCameraFollowingUnit() => MoveCamera.isCameraFollowingUnit(this);

  internal bool isTargetOkToAttack(Actor pTarget)
  {
    return pTarget != this && this.canAttackTarget((BaseSimObject) pTarget) && this.isSameIslandAs((BaseSimObject) pTarget);
  }

  private float getLastColorEffectTime()
  {
    return World.world.getRealTimeElapsedSince(this._last_color_effect_timestamp);
  }

  private float getLastStaminaReduceTime()
  {
    return World.world.getRealTimeElapsedSince(this._last_stamina_reduce_timestamp);
  }

  public bool isUnderDamageCooldown()
  {
    return (double) this.getLastColorEffectTime() < 0.30000001192092896;
  }

  private bool isUnderStaminaCooldown()
  {
    return (double) this.getLastStaminaReduceTime() < 0.30000001192092896;
  }

  private bool haveMetallicArmor() => false;

  private bool haveMetallicWeapon()
  {
    return this.hasEquipment() && !this.equipment.getSlot(EquipmentType.Weapon).isEmpty() && this.equipment.getSlot(EquipmentType.Weapon).getItem().getAsset().metallic;
  }

  public bool isSameKingdomAndAlmostDead(Actor pActor, float pDamage)
  {
    return this.isSameKingdom((BaseSimObject) pActor) && (double) this.getHealth() - (double) pDamage <= 0.0;
  }

  public bool isSameKingdom(BaseSimObject pSimObject) => this.kingdom == pSimObject.kingdom;

  public bool isInCityIsland()
  {
    if (this.city.isRekt())
      return false;
    WorldTile tile = this.city.getTile();
    return tile != null && this.current_tile.isSameIsland(tile);
  }

  public bool isClone() => this._has_trait_clone;

  public bool isClonedFrom(Actor pActor)
  {
    return this.isClone() && this.data.parent_id_1 == pActor.data.id;
  }

  public bool isSameClones(Actor pActor)
  {
    return this.isClone() && pActor.isClone() && this.data.parent_id_1 == pActor.data.parent_id_1;
  }

  public bool isUnitFitToRule() => this.isAlive() && this.isKingdomCiv();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(Actor pObject) => this.GetHashCode() == pObject.GetHashCode();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int CompareTo(Actor pTarget) => this.GetHashCode().CompareTo(pTarget.GetHashCode());

  public bool canTalkWith(Actor pTarget)
  {
    return this != pTarget && pTarget.isReadyToTalk() && this.isSameIslandAs((BaseSimObject) pTarget) && !this.areFoes((BaseSimObject) pTarget) && !this.isInsideSomething() && !pTarget.asset.special;
  }

  public bool canFallInLoveWith(Actor pTarget)
  {
    return !this.hasLover() && this.isAdult() && this.isBreedingAge() && this.subspecies.needs_mate && pTarget.subspecies.needs_mate && this.isSameSpecies(pTarget) && this.subspecies.isPartnerSuitableForReproduction(this, pTarget) && !pTarget.hasLover() && pTarget.isAdult() && pTarget.isBreedingAge() && !this.isRelatedTo(pTarget);
  }

  public bool hasHouseCityInBordersAndSameIsland()
  {
    return this.hasCity() && this.hasHouse() && this.inOwnCityBorders() && this.inOwnHouseIsland();
  }

  public bool inOwnHouseIsland()
  {
    Building homeBuilding = this.getHomeBuilding();
    return !homeBuilding.isRekt() && this.current_tile.isSameIsland(homeBuilding.current_tile);
  }

  public bool inOwnCityBorders() => this.hasCity() && this.current_zone.isSameCityHere(this.city);

  public bool inOwnCityIsland()
  {
    if (!this.hasCity())
      return false;
    WorldTile tile = this.city.getTile();
    return tile != null && this.current_tile.isSameIsland(tile);
  }

  public bool isReadyToTalk()
  {
    return this.isAlive() && this.canSocialize() && (!this.hasTask() || this.ai.task.cancellable_by_socialize);
  }

  public bool canSocialize()
  {
    return !this.asset.unit_zombie && !this.isEgg() && !this.isFighting() && !this.hasStatus("recovery_social") && this.hasSubspecies();
  }

  public int getConstructionSpeed()
  {
    int constructionSpeed = 2;
    if (this.hasSubspecies())
      constructionSpeed += (int) this.subspecies.base_stats_meta["construction_speed"];
    return constructionSpeed;
  }

  private bool combatActionOnTimeout() => this.hasStatus("recovery_combat_action");

  private bool hasSpellCastCooldownStatus() => this.hasStatus("recovery_spell");

  public bool hasEnoughMana(int pCostMana) => pCostMana == 0 || this.getMana() >= pCostMana;

  public int getMana() => this.data.mana;

  public int getMaxMana() => (int) this.stats["mana"];

  public void setMaxMana() => this.setMana(this.getMaxMana());

  public bool isManaFull() => this.getMana() == this.getMaxMana();

  public bool hasEnoughStamina(int pCostStamina)
  {
    return pCostStamina == 0 || this.getStamina() >= pCostStamina;
  }

  public int getStamina() => this.data.stamina;

  public int getMaxStamina() => (int) this.stats["stamina"];

  public void setMaxStamina() => this.setStamina(this.getMaxStamina());

  public bool isStaminaFull() => this.getStamina() == this.getMaxStamina();

  public int current_children_count => this._current_children;

  public bool isWarrior() => this.profession_asset.profession_id == UnitProfession.Warrior;

  public bool isCarnivore() => this.hasSubspecies() && this.subspecies.diet_meat;

  public bool isHerbivore() => this.hasSubspecies() && this.subspecies.diet_vegetation;

  public bool hasStatusStunned() => this.hasStatus("stunned");

  public bool is_unconscious => this._has_tag_unconscious;

  public bool isLying() => this.is_unconscious || this._has_status_sleeping;

  public override bool hasStatusTantrum() => this._has_status_tantrum;

  public bool hasAnyCash() => this.money > 0 || this.loot > 0;

  public int loot => this.data.loot;

  public int money => this.data.money;

  public int renown => this.data.renown;

  public int level => this.data.level;

  public bool hasEnoughMoney(int pCost) => this.money >= pCost;

  public int getHappinessPercent()
  {
    int maxHappiness = this.getMaxHappiness();
    int minHappiness = this.getMinHappiness();
    return Mathf.Clamp(Mathf.Clamp(this.getHappiness() - minHappiness, 0, maxHappiness - minHappiness) * 100 / (maxHappiness - minHappiness), 0, 100);
  }

  public float distanceToObjectTarget(BaseSimObject pBaseSimObject)
  {
    return Toolbox.DistVec2Float(this.current_position, pBaseSimObject.current_position);
  }

  public float distanceToActorTile(Actor pActor) => this.distanceToActorTile(pActor.current_tile);

  public float distanceToActorTile(WorldTile pTile) => this.current_tile.distanceTo(pTile);

  public bool isRelatedTo(Actor pTarget)
  {
    return this.hasFamily() && pTarget.hasFamily() && this.isSapient() && this.family == pTarget.family || this.isChildOf(pTarget) || this.isParentOf(pTarget);
  }

  public bool isImportantTo(Actor pTarget)
  {
    return this.hasLover() && this.lover == pTarget || this.hasBestFriend() && this.getBestFriend() == pTarget;
  }

  public bool canWork()
  {
    if (this.isAdult())
      return true;
    if (this.hasCulture())
    {
      Culture culture = this.culture;
      if (culture.hasTrait("tiny_legends"))
        return true;
      if (culture.hasTrait("youth_reverence"))
        return false;
    }
    return this.getAge() >= SimGlobals.m.child_work_age;
  }

  public int intelligence => (int) this.stats[nameof (intelligence)];

  public int diplomacy => (int) this.stats[nameof (diplomacy)];

  public int warfare => (int) this.stats[nameof (warfare)];

  public int stewardship => (int) this.stats[nameof (stewardship)];

  public bool hasCultureTrait(string pTraitID)
  {
    return this.hasCulture() && this.culture.hasTrait(pTraitID);
  }

  public bool canBePossessed() => this.asset.allow_possession;

  public float getAttackRange() => this.stats["range"];

  public float getAttackRangeSquared() => this.stats["range"] * this.stats["range"];

  protected override MetaType meta_type => MetaType.Unit;

  public float getStaminaRatio()
  {
    float maxStamina = (float) this.getMaxStamina();
    return (double) maxStamina == 0.0 ? 0.0f : (float) this.getStamina() / maxStamina;
  }

  public float getManaRatio()
  {
    float maxMana = (float) this.getMaxMana();
    return (double) maxMana == 0.0 ? 0.0f : (float) this.getMana() / maxMana;
  }

  public bool canGetFoodFromCity()
  {
    return this.isFoodFreeForThisPerson() || this.money > SimGlobals.m.min_coins_before_city_food;
  }

  public bool isFoodFreeForThisPerson()
  {
    return this.isKing() || this.isCityLeader() || this.isBaby() || this.isStarving();
  }

  public int getMaxNutrition()
  {
    float nutritionMax = (float) this.asset.nutrition_max;
    if (this.hasSubspecies())
      nutritionMax += this.subspecies.base_stats_meta["max_nutrition"];
    return (int) nutritionMax;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int getExpToLevelup() => 100 + (this.data.level - 1) * 20;

  private bool calculateIsSick()
  {
    return this.hasTrait("infected") || this.hasTrait("plague") || this.hasTrait("mush_spores") && this.asset.can_turn_into_mush || this.hasTrait("tumor_infection") && this.asset.can_turn_into_tumor;
  }

  public bool isSick() => this._has_any_sick_trait;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool canTakeItems() => this.asset.take_items;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool understandsHowToUseItems() => this.canUseItems() && this.isSapient();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool canUseItems() => this.asset.use_items;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool canEditEquipment() => this.asset.use_items;

  public bool canTurnIntoColdOne()
  {
    return !this.isAdult() && this.asset.can_turn_into_ice_one && this.asset.has_soul;
  }

  public bool canTurnIntoDemon()
  {
    return !this.isBaby() && this.asset.can_turn_into_demon_in_age_of_chaos;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override BaseObjectData getData() => (BaseObjectData) this.data;

  public bool is_moving => this._is_moving || this._possessed_movement;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isCarryingResources() => this.inventory.hasResources();

  public bool needsFood() => this.hasSubspecies() && this.subspecies.needs_food;

  public bool isDamagedByRain() => this.hasSubspecies() && this.subspecies.is_damaged_by_water;

  public bool isDamagedByOcean()
  {
    return this.hasSubspecies() ? this.subspecies.is_damaged_by_water : this.asset.damaged_by_ocean;
  }

  public int getWaterDamage()
  {
    int waterDamage = (int) ((double) this.getMaxHealth() * (double) SimGlobals.m.water_damage_multiplier);
    if (waterDamage < 1)
      waterDamage = 1;
    return waterDamage;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasSubspeciesTrait(string pTraitID)
  {
    return this.hasSubspecies() && this.subspecies.hasTrait(pTraitID);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasSubspeciesMetaTag(string pTagID)
  {
    return this.hasSubspecies() && this.subspecies.hasMetaTag(pTagID);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasTag(string pTag) => this.stats.hasTag(pTag);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isImmuneToFire() => this.hasTag("immunity_fire");

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isImmuneToCold() => this.hasTag("immunity_cold");

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isImmovable() => this.hasTag("immovable");

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isAiFrozen() => this.hasTag("frozen_ai");

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isIgnoreFights() => this.hasTag("ignore_fights");

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasStopIdleAnimation()
  {
    return (!this.hasSubspecies() || !this.subspecies.hasMetaTag("always_idle_animation")) && this.hasTag("stop_idle_animation");
  }

  public bool hasDivineScar() => this.hasTrait("scar_of_divinity");

  public bool hasTelepathicLink()
  {
    return this.hasSubspecies() && this.subspecies.hasTrait("telepathic_link");
  }

  public float getResourceThrowDistance()
  {
    return this.asset.base_throwing_range + this.stats["throwing_range"];
  }

  internal bool isFalling()
  {
    return (double) this.position_height != 0.0 || (double) this.move_jump_offset.y != 0.0;
  }

  public float getAgeRatio()
  {
    float stat = this.stats["lifespan"];
    return (float) this.getAge() / stat;
  }

  public int getMassKG()
  {
    int massKg = (int) ((double) this.stats["mass_2"] * (double) (this.target_scale / 0.1f));
    if (this.isBaby())
      massKg = (int) ((double) massKg * (double) SimGlobals.m.baby_mass_multiplier);
    return massKg;
  }

  public IEnumerable<ResourceContainer> getResourcesFromActor()
  {
    if (this.asset.resources_given != null)
    {
      int tMass = this.getMassKG();
      foreach (ResourceContainer resourceContainer in this.asset.resources_given)
      {
        ResourceAsset asset = resourceContainer.asset;
        int num = tMass / asset.drop_per_mass + 1;
        int pAmount = Mathf.Clamp(resourceContainer.amount * num, 1, asset.drop_max);
        if (pAmount > 0)
          yield return new ResourceContainer(resourceContainer.id, pAmount);
      }
    }
  }

  public bool hasXenophobic() => this.hasCulture() && this.culture.hasTrait("xenophobic");

  public bool hasXenophiles() => this.hasCulture() && this.culture.hasTrait("xenophiles");

  public bool hasCannibalism() => this.hasSubspecies() && this.subspecies.hasCannibalism();

  public bool isOneCityKingdom() => this.hasCity() && this.city.kingdom.countCities() == 1;

  public bool isImportantPerson()
  {
    return this.isKing() || this.isCityLeader() || this.isArmyGroupLeader() || this.isFavorite();
  }

  public bool canCurrentTaskBeCancelledByReproduction()
  {
    return !this.hasTask() || this.ai.task.cancellable_by_reproduction;
  }

  public bool isAbleToSkipPriorityLevels()
  {
    return !this.isWarrior() || !this.hasCity() || !this.city.hasAttackZoneOrder();
  }

  public void makeSpawnSound(bool pFromUI)
  {
    if (!this.asset.has_sound_spawn)
      return;
    if (pFromUI)
      MusicBox.playSoundUI(this.asset.sound_spawn);
    else
      MusicBox.playSound(this.asset.sound_spawn, this.current_tile);
  }

  public void makeSoundAttack()
  {
    if (!this.asset.has_sound_attack)
      return;
    MusicBox.playSound(this.asset.sound_attack, this.current_tile, true, true);
  }

  public string getTaskText()
  {
    if (!this.hasTask())
      return "???";
    return $"{this.ai.task.getLocalizedText()} {this.ai.getTaskTime().ColorHex(ColorStyleLibrary.m.color_text_grey_dark)}";
  }

  public void afterEvolutionEvents()
  {
    this.clearGraphicsFully();
    this.makeConfused();
    this.applyRandomForce();
    this.increaseEvolutions();
  }

  public void generatePhenotypeAndShade()
  {
    this.data.phenotype_index = this.subspecies.getRandomPhenotypeIndex();
    this.data.phenotype_shade = Actor.getRandomPhenotypeShade();
  }

  public static int getRandomPhenotypeShade() => Randy.randomInt(0, 4);

  public bool isRendered() => this.current_tile.zone.visible;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool checkHasRenderedItem()
  {
    return this.canUseItems() && !this._is_in_liquid && !this.isEgg() && (!this.equipment.weapon.isEmpty() || this.hasTask() && this.ai.task.force_hand_tool != string.Empty || this.isCarryingResources());
  }

  internal Sprite getSpriteToRender() => this.checkSpriteToRender();

  public bool hasColoredSprite() => this.asset.need_colored_sprite;

  public bool isColoredSpriteNeedsCheck(Sprite pMainSprite)
  {
    return Object.op_Inequality((Object) this._last_main_sprite, (Object) pMainSprite) || this._last_color_asset != this.kingdom.getColor();
  }

  public Sprite calculateColoredSprite(Sprite pMainSprite, bool pUpdateFrameData = true)
  {
    if (this.isColoredSpriteNeedsCheck(pMainSprite))
    {
      if (this.animation_container != null & pUpdateFrameData)
        this.animation_container.dict_frame_data.TryGetValue(((Object) pMainSprite).name, out this.frame_data);
      this.checkSpriteHead();
      int phenotypeIndex = this.data.phenotype_index;
      int phenotypeShade = this.data.phenotype_shade;
      this._last_colored_sprite = DynamicSpriteCreator.getSpriteUnit(this.frame_data, pMainSprite, this, this.kingdom.getColor(), phenotypeIndex, phenotypeShade, this.asset.texture_atlas);
      this._last_main_sprite = pMainSprite;
      this._last_color_asset = this.kingdom.getColor();
    }
    return this._last_colored_sprite;
  }

  public Sprite getLastColoredSprite() => this._last_colored_sprite;

  public bool canParallelSetColoredSprite()
  {
    return this.asset.has_avatar_prefab || !this.dirty_sprite_main;
  }

  public Sprite calculateMainSprite()
  {
    if (this.asset.has_override_sprite)
      return this.asset.get_override_sprite(this);
    this.checkAnimationContainer();
    if (this.ai.action != null && this.ai.action.force_animation)
      return this.animation_container.sprites[this.ai.action.force_animation_id];
    if (!this.isAlive() || this._has_stop_idle_animation)
      return !this.animation_container.has_swimming || !this._has_status_drowning ? this.animation_container.idle.frames[0] : this.animation_container.swimming.frames[0];
    float pAnimationSpeed = this.asset.animation_walk_speed;
    bool flag = false;
    ActorAnimation actorAnimation;
    if (this.is_moving || (double) this.timer_jump_animation > 0.0 || (double) this.move_jump_offset.y > 0.0 || this.is_in_magnet)
    {
      if (this.animation_container.has_swimming && this.isAffectedByLiquid())
      {
        actorAnimation = this.animation_container.swimming;
        pAnimationSpeed = this.asset.animation_swim_speed;
      }
      else
        actorAnimation = this.animation_container.walking;
      flag = true;
    }
    else if ((double) this.position_height > 0.0)
      actorAnimation = this.animation_container.idle;
    else if (this.animation_container.has_swimming && this.isAffectedByLiquid())
    {
      actorAnimation = this.animation_container.swimming;
      pAnimationSpeed = this.asset.animation_swim_speed;
      flag = true;
    }
    else
    {
      actorAnimation = this.animation_container.idle;
      pAnimationSpeed = this.asset.animation_idle_speed;
    }
    if (this.asset.animation_speed_based_on_walk_speed & flag)
    {
      float num = pAnimationSpeed * (this.stats["speed"] / 10f);
      pAnimationSpeed = Mathf.Clamp(num, 4f, num);
    }
    return actorAnimation.frames.Length <= 1 ? actorAnimation.frames[0] : AnimationHelper.getSpriteFromList(this.GetHashCode(), (IList<Sprite>) actorAnimation.frames, pAnimationSpeed);
  }

  internal Sprite checkSpriteToRender()
  {
    Sprite mainSprite = this.calculateMainSprite();
    return !this.asset.need_colored_sprite ? mainSprite : this.calculateColoredSprite(mainSprite);
  }

  protected void setItemSpriteRenderDirty() => this._dirty_sprite_item = true;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Sprite getRenderedItemSprite()
  {
    if (this._dirty_sprite_item || this._has_animated_item)
    {
      this._cached_hand_renderer_asset = this.getHandRendererAsset();
      this._cached_sprite_item = ItemRendering.getItemMainSpriteFrame(this._cached_hand_renderer_asset);
      this._dirty_sprite_item = false;
    }
    return this._cached_sprite_item;
  }

  public IHandRenderer getCachedHandRendererAsset() => this._cached_hand_renderer_asset;

  public IHandRenderer getHandRendererAsset()
  {
    IHandRenderer renderedToolOrItem = this.getRenderedToolOrItem();
    if (renderedToolOrItem != null)
      return renderedToolOrItem;
    return this.hasWeapon() ? this.getWeaponTextureId() : (IHandRenderer) null;
  }

  private IHandRenderer getRenderedToolOrItem()
  {
    if (!this.asset.use_tool_items)
      return (IHandRenderer) null;
    this._has_animated_item = false;
    if (this.has_attack_target && this.hasWeapon())
      return (IHandRenderer) null;
    if (this.isCarryingResources())
      return (IHandRenderer) AssetManager.resources.get(this.inventory.getItemIDToRender());
    if (this.hasTask())
    {
      UnitHandToolAsset cachedHandToolAsset = this.ai.task.cached_hand_tool_asset;
      if (cachedHandToolAsset != null)
      {
        this._has_animated_item = cachedHandToolAsset.animated;
        return (IHandRenderer) cachedHandToolAsset;
      }
    }
    return (IHandRenderer) null;
  }

  public bool isItemInHandAnimated()
  {
    if (this.isCarryingResources())
      return false;
    if (this.hasTask())
    {
      UnitHandToolAsset cachedHandToolAsset = this.ai.task.cached_hand_tool_asset;
      if (cachedHandToolAsset != null)
        return cachedHandToolAsset.animated;
    }
    return this.hasWeapon() && this.getWeapon().getAsset().animated;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void clearSprites()
  {
    this.dirty_sprite_head = true;
    this.dirty_sprite_main = true;
  }

  public void clearGraphicsFully()
  {
    this.clearSprites();
    this.clearLastColorCache();
    this.animation_container = (AnimationContainerUnit) null;
    this.frame_data = (AnimationFrameData) null;
    this.animation_container = (AnimationContainerUnit) null;
    this._last_main_sprite = (Sprite) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public AnimationFrameData getAnimationFrameData() => this.frame_data;

  public Vector3 getHeadOffsetPositionForFunRendering()
  {
    Vector3 positionForFunRendering;
    // ISSUE: explicit constructor call
    ((Vector3) ref positionForFunRendering).\u002Ector(this.cur_transform_position.x, this.cur_transform_position.y, 0.0f);
    AnimationFrameData animationFrameData = this.getAnimationFrameData();
    if (animationFrameData != null)
    {
      positionForFunRendering.x += animationFrameData.pos_head.x * this.current_scale.x;
      positionForFunRendering.y += animationFrameData.pos_head.y * this.current_scale.y;
    }
    return positionForFunRendering;
  }

  public IHandRenderer getWeaponTextureId()
  {
    Item weapon = this.getWeapon();
    this._has_animated_item = weapon.getAsset().animated;
    return (IHandRenderer) weapon.asset;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private ActorTextureSubAsset getTextureAsset()
  {
    return !this.hasSubspecies() || !this.subspecies.has_mutation_reskin ? this.asset.texture_asset : this.subspecies.mutation_skin_asset.texture_asset;
  }

  public string getUnitTexturePath() => this.getTextureAsset().getUnitTexturePath(this);

  internal void checkAnimationContainer()
  {
    if (!this.dirty_sprite_main)
      return;
    this.dirty_sprite_main = false;
    this.animation_container = ActorAnimationLoader.getAnimationContainer(this.getUnitTexturePath(), this.asset, this.subspecies?.egg_asset, this.subspecies?.mutation_skin_asset);
  }

  public SpriteAnimation getSpriteAnimation() => this.sprite_animation;

  public Vector2 getRenderedItemPosition()
  {
    AnimationFrameData animationFrameData = this.getAnimationFrameData();
    return animationFrameData == null ? Vector2.one : animationFrameData.pos_item;
  }

  public void clearLastColorCache()
  {
    this._last_colored_sprite = (Sprite) null;
    this._last_color_asset = (ColorAsset) null;
    this.cached_sprite_head = (Sprite) null;
  }

  public void startColorEffect(ActorColorEffect pColorType = ActorColorEffect.White)
  {
    if (!this.asset.effect_damage || !this.is_visible || this.isUnderDamageCooldown())
      return;
    this._last_color_effect_timestamp = World.world.getCurSessionTime();
    if (World.world.stack_effects.actor_effect_hit.Count > 1000)
      return;
    if (pColorType == ActorColorEffect.Red)
      World.world.stack_effects.actor_effect_hit.Add(new ActorDamageEffectData()
      {
        actor = this,
        timestamp = this._last_color_effect_timestamp
      });
    else
      World.world.stack_effects.actor_effect_highlight.Add(new ActorHighlightEffectData()
      {
        actor = this,
        timestamp = this._last_color_effect_timestamp
      });
  }

  protected void checkSpriteHead()
  {
    if (!this.dirty_sprite_head)
      return;
    this.dirty_sprite_head = false;
    if (this.frame_data == null || !this.frame_data.show_head || this.animation_container.heads.Length == 0 || this.isEgg() || this.isBaby() && !this.animation_container.render_heads_for_children)
      return;
    ActorTextureSubAsset textureAsset = this.getTextureAsset();
    if (!textureAsset.has_advanced_textures)
    {
      this.checkHeadID(this.animation_container.heads);
      this.setHeadSprite(this.animation_container.heads[this.data.head]);
    }
    else
    {
      bool flag = false;
      string pPath;
      Sprite[] pListHeads;
      if (this.isSexMale())
      {
        pPath = textureAsset.texture_heads_male;
        pListHeads = this.animation_container.heads_male;
      }
      else
      {
        pPath = textureAsset.texture_heads_female;
        pListHeads = this.animation_container.heads_female;
      }
      if (this.isSapient())
      {
        if (this.is_profession_warrior && !this.equipment.helmet.isEmpty())
        {
          pPath = textureAsset.texture_head_warrior;
          flag = true;
        }
        else if (this.is_profession_king)
        {
          pPath = textureAsset.texture_head_king;
          flag = true;
        }
        else if (textureAsset.has_old_heads && this.hasTrait("wise"))
        {
          pPath = !this.isSexMale() ? textureAsset.texture_heads_old_female : textureAsset.texture_heads_old_male;
          flag = true;
        }
        else if (this.isSexMale())
        {
          pPath = textureAsset.texture_heads_male;
          pListHeads = this.animation_container.heads_male;
        }
        else
        {
          pPath = textureAsset.texture_heads_female;
          pListHeads = this.animation_container.heads_female;
        }
      }
      if (flag)
      {
        this.setHeadSprite(ActorAnimationLoader.getHeadSpecial(pPath));
      }
      else
      {
        this.checkHeadID(pListHeads);
        this.setHeadSprite(ActorAnimationLoader.getHead(pPath, this.data.head));
      }
    }
  }

  internal void checkHeadID(Sprite[] pListHeads, bool pCheckSavedHead = true)
  {
    if (pCheckSavedHead && this.data.head > pListHeads.Length - 1)
      this.data.head = 0;
    if (this.data.head != -1)
      return;
    this.data.head = AnimationHelper.getSpriteIndex(this.data.id, pListHeads.Length);
  }

  private void setHeadSprite(Sprite pSprite) => this.cached_sprite_head = pSprite;

  protected void updateDeadAnimation(float pElapsed)
  {
    if (this.asset.special_dead_animation && this.asset.action_dead_animation((BaseSimObject) this, this.current_tile, pElapsed))
      return;
    if (World.world.quality_changer.isFullLowRes())
    {
      this.die(true, AttackType.None, false);
    }
    else
    {
      if (this.asset.death_animation_angle && !this._has_status_drowning && (double) this.target_angle.z < 90.0)
      {
        this.target_angle.z = Mathf.Lerp(this.target_angle.z, 90f, pElapsed * 4f);
        if ((double) this.target_angle.z > 90.0)
          this.target_angle.z = 90f;
        if (this.is_visible && (double) Mathf.Abs(this.current_rotation.z) < 45.0)
          return;
      }
      this.changeMoveJumpOffset(-0.05f);
      if (this.isFalling())
        return;
      this.updateDeadBlackAnimation(pElapsed);
    }
  }

  public bool has_rendered_sprite_head => this.cached_sprite_head != null;

  public double[] getDecisionsCooldowns() => this._decision_cooldowns;

  public bool[] getDecisionsDisabled() => this._decision_disabled;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isDecisionOnCooldown(int pIndex, double pCooldown)
  {
    double decisionCooldown = this._decision_cooldowns[pIndex];
    if (decisionCooldown == 0.0)
      return false;
    if ((double) World.world.getWorldTimeElapsedSince(decisionCooldown) <= pCooldown)
      return true;
    this._decision_cooldowns[pIndex] = 0.0;
    return false;
  }

  public void setupRandomDecisionCooldowns()
  {
    double curWorldTime = World.world.getCurWorldTime();
    for (int index = 0; index < this.decisions_counter; ++index)
    {
      DecisionAsset decision = this.decisions[index];
      if (decision.cooldown != 0)
      {
        double num = curWorldTime - (double) Randy.randomFloat(0.0f, (float) decision.cooldown * 0.5f);
        this._decision_cooldowns[decision.decision_index] = num;
      }
    }
    this.timer_action = Randy.randomFloat(1f, 5f);
  }

  public void setDecisionCooldown(DecisionAsset pAsset)
  {
    if (pAsset.cooldown == 0)
      return;
    this._decision_cooldowns[pAsset.decision_index] = World.world.getCurWorldTime();
  }

  public bool isDecisionEnabled(int pIndex) => !this._decision_disabled[pIndex];

  public bool switchDecisionState(int pIndex)
  {
    this._decision_disabled[pIndex] = !this._decision_disabled[pIndex];
    return this._decision_disabled[pIndex];
  }

  public void setDecisionState(int pIndex, bool pState)
  {
    this._decision_disabled[pIndex] = !pState;
  }

  public void setTask(string pTaskId, bool pClean = true, bool pCleanJob = false, bool pForceAction = false)
  {
    this.ai.setTask(pTaskId, pClean, pCleanJob, pForceAction);
  }

  public void cancelAllBeh()
  {
    this.ai.clearBeh();
    this.ai.setTaskBehFinished();
    this.endJob();
    this.clearTasks();
  }

  public void endJob()
  {
    this.ai.clearJob();
    this.citizen_job = (CitizenJobAsset) null;
  }

  protected virtual void clearTasks()
  {
    this.exitBuilding();
    this.clearAttackTarget();
    this.timer_action = 0.0f;
    this.clearTileTarget();
    this.stopMovement();
  }

  public void setCitizenJob(CitizenJobAsset pJobAsset)
  {
    this.citizen_job = pJobAsset;
    this.ai.setJob(pJobAsset.unit_job_default);
  }

  internal void clearBeh()
  {
    this.clearTasks();
    this.beh_tile_target = (WorldTile) null;
    this.beh_building_target = (Building) null;
    this.beh_actor_target = (BaseSimObject) null;
    this.beh_book_target = (Book) null;
  }

  public string getNextJob() => Actor.nextJobActor(this.a);

  public static string nextJobActor(Actor pActor)
  {
    if (pActor.isEgg())
      return "egg";
    string str = (string) null;
    if (pActor.isSapient())
    {
      if (pActor.isBaby())
        str = pActor.asset.job_baby.GetRandom<string>();
      else if (pActor.hasCity())
        str = !pActor.isProfession(UnitProfession.Warrior) ? pActor.asset.job_citizen.GetRandom<string>() : pActor.asset.job_attacker.GetRandom<string>();
      else if (pActor.isKingdomCiv())
        str = pActor.asset.job_kingdom.GetRandom<string>();
      else if (pActor.asset.job.Length != 0)
        str = pActor.asset.job.GetRandom<string>();
    }
    else if (pActor.asset.job.Length != 0)
      str = pActor.asset.job.GetRandom<string>();
    return str;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isTask(string pID) => this.ai.task?.id == pID;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasTask() => this.ai.hasTask();

  public void clearDecisions()
  {
    this._decision_cooldowns.Clear<double>();
    this._decision_disabled.Clear<bool>();
    this.decisions.Clear<DecisionAsset>();
    this.decisions_counter = 0;
    this._last_decision_id = string.Empty;
  }

  public void scheduleTask(string pTask, WorldTile pTile)
  {
    this.ai.scheduleTask(pTask);
    this.scheduled_tile_target = pTile;
  }

  private void registerDecisions()
  {
    foreach (BaseAugmentationAsset trait in this.traits)
    {
      DecisionAsset[] decisionsAssets = trait.decisions_assets;
      if (decisionsAssets != null)
      {
        for (int index = 0; index < decisionsAssets.Length; ++index)
          this.decisions[this.decisions_counter++] = decisionsAssets[index];
      }
    }
    Clan clan = this.clan;
    if ((clan != null ? (clan.decisions_assets.Count > 0 ? 1 : 0) : 0) != 0)
    {
      List<DecisionAsset> decisionsAssets = this.clan.decisions_assets;
      for (int index = 0; index < decisionsAssets.Count; ++index)
        this.decisions[this.decisions_counter++] = decisionsAssets[index];
    }
    Culture culture = this.culture;
    if ((culture != null ? (culture.decisions_assets.Count > 0 ? 1 : 0) : 0) != 0)
    {
      List<DecisionAsset> decisionsAssets = this.culture.decisions_assets;
      for (int index = 0; index < decisionsAssets.Count; ++index)
        this.decisions[this.decisions_counter++] = decisionsAssets[index];
    }
    Language language = this.language;
    if ((language != null ? (language.decisions_assets.Count > 0 ? 1 : 0) : 0) != 0)
    {
      List<DecisionAsset> decisionsAssets = this.language.decisions_assets;
      for (int index = 0; index < decisionsAssets.Count; ++index)
        this.decisions[this.decisions_counter++] = decisionsAssets[index];
    }
    Religion religion = this.religion;
    if ((religion != null ? (religion.decisions_assets.Count > 0 ? 1 : 0) : 0) != 0 && this.canUseReligionSpells())
    {
      List<DecisionAsset> decisionsAssets = this.religion.decisions_assets;
      for (int index = 0; index < decisionsAssets.Count; ++index)
        this.decisions[this.decisions_counter++] = decisionsAssets[index];
    }
    Subspecies subspecies = this.subspecies;
    if ((subspecies != null ? (subspecies.decisions_assets.Count > 0 ? 1 : 0) : 0) != 0)
    {
      List<DecisionAsset> decisionsAssets = this.subspecies.decisions_assets;
      for (int index = 0; index < decisionsAssets.Count; ++index)
        this.decisions[this.decisions_counter++] = decisionsAssets[index];
    }
    if (this.profession_asset != null && this.profession_asset.hasDecisions())
    {
      foreach (DecisionAsset decisionsAsset in this.profession_asset.decisions_assets)
        this.decisions[this.decisions_counter++] = decisionsAsset;
    }
    if (this._spells.hasAny())
    {
      foreach (SpellAsset spell in (IEnumerable<SpellAsset>) this._spells.spells)
      {
        if (spell.hasDecisions())
        {
          foreach (DecisionAsset decisionsAsset in spell.decisions_assets)
            this.decisions[this.decisions_counter++] = decisionsAsset;
        }
      }
    }
    if (this.hasWeapon() && this.getWeapon().getAsset().hasDecisions())
    {
      foreach (DecisionAsset decisionsAsset in this.getWeapon().getAsset().decisions_assets)
        this.decisions[this.decisions_counter++] = decisionsAsset;
    }
    if (this.hasFamily())
    {
      foreach (DecisionAsset decisionsAsset in MetaTypeLibrary.family.decisions_assets)
        this.decisions[this.decisions_counter++] = decisionsAsset;
    }
    if (this.hasCity() && !this.asset.is_boat)
    {
      foreach (DecisionAsset decisionsAsset in MetaTypeLibrary.city.decisions_assets)
        this.decisions[this.decisions_counter++] = decisionsAsset;
    }
    if (this.hasPlot())
    {
      foreach (DecisionAsset decisionsAsset in MetaTypeLibrary.plot.decisions_assets)
        this.decisions[this.decisions_counter++] = decisionsAsset;
    }
    if (!this.hasClan())
      return;
    foreach (DecisionAsset decisionsAsset in MetaTypeLibrary.clan.decisions_assets)
      this.decisions[this.decisions_counter++] = decisionsAsset;
  }

  public void debugFav()
  {
  }

  public WorldTile debug_next_step_tile => this._next_step_tile;

  public void clearWait() => this.timer_action = 0.0f;

  public void makeWait(float pValue = 10f)
  {
    this.stopMovement();
    this.timer_action = pValue;
  }

  public void stopSleeping() => this.finishStatusEffect("sleeping");

  private void checkStepActionForTile(WorldTile pTile)
  {
    if (pTile.Type.step_action != null && Randy.randomChance(pTile.Type.step_action_chance))
    {
      int num = pTile.Type.step_action(pTile, this.a) ? 1 : 0;
    }
    Building building = pTile.building;
    if (building == null || !building.asset.flora)
      return;
    BuildingAsset asset = building.asset;
    switch (asset.flora_type)
    {
      case FloraType.Fungi:
        if (!WorldLawLibrary.world_law_exploding_mushrooms.isEnabled())
          break;
        MapAction.damageWorld(pTile, 5, AssetManager.terraform.get("grenade"));
        EffectsLibrary.spawnAtTileRandomScale("fx_explosion_small", pTile, 0.1f, 0.15f);
        break;
      case FloraType.Plant:
        if (asset.type == "type_flower" && WorldLawLibrary.world_law_nectar_nap.isEnabled() && Randy.randomChance(0.1f))
        {
          this.makeSleep(10f);
          break;
        }
        if (WorldLawLibrary.world_law_plants_tickles.isEnabled() && Randy.randomChance(0.3f))
          this.tryToGetSurprised(pTile);
        if (!WorldLawLibrary.world_law_root_pranks.isEnabled() || !Randy.randomChance(0.2f))
          break;
        this.makeStunned();
        break;
    }
  }

  public void setLover(Actor pActor) => this.lover = pActor;

  public void setBestFriend(Actor pActor, bool pNew)
  {
    this.data.best_friend_id = pActor.data.id;
    if (!pNew)
      return;
    this.changeHappiness("just_made_friend");
  }

  public void becomeLoversWith(Actor pTarget)
  {
    this.setLover(pTarget);
    pTarget.setLover(this);
    this.addStatusEffect("fell_in_love", pColorEffect: false);
    pTarget.addStatusEffect("fell_in_love", pColorEffect: false);
  }

  public void resetSocialize()
  {
    this.data.removeInt("socialize");
    this.timestamp_tween_session_social = 0.0;
  }

  public void addActionWaitAfterLand(float pTimer)
  {
    this._action_wait_after_land = true;
    this._action_wait_after_land_timer = pTimer;
  }

  private void actionMagnetAnimation(Actor pActor) => this.position_height = 0.0f;

  private bool isSurprisedJump(WorldTile pTile)
  {
    int num = this.canSeeTileBasedOnDirection(pTile) ? 1 : 0;
    bool flag = false;
    if (num == 0 && this.hasSubspecies() && this.subspecies.can_process_emotions && (this.subspecies.has_trait_timid || !this.hasStatus("on_guard")))
      flag = true;
    return flag;
  }

  private void checkLand(Actor pActor)
  {
    if (!this.should_check_land_cancel)
      return;
    this.should_check_land_cancel = false;
    if (this.has_attack_target && this.isEnemyTargetAlive() && this._has_emotions && !this.hasStatusTantrum())
    {
      if ((double) this.getHealthRatio() < 0.15000000596046448)
      {
        this.cancelAllBeh();
        this.setTask("run_away", pForceAction: true);
        return;
      }
      if ((double) Toolbox.DistVec2Float(this.current_position, this.attack_target.current_position) < 10.0)
        return;
    }
    this.cancelAllBeh();
  }

  private void checkDeathOutsideMap(Actor pActor)
  {
    if (this.inMapBorder())
      return;
    this.getHitFullHealth(AttackType.Gravity);
  }

  public void tryToGetSurprised(WorldTile pTile, bool pForceJump = false)
  {
    if (!this.canBeSurprised(pTile))
      return;
    this.getSurprised(pTile, pForceJump);
  }

  public void getSurprised(WorldTile pTile, bool pForceJump = false)
  {
    if (!this._has_emotions)
      return;
    float num1 = 1f + Randy.randomFloat(0.0f, 2f);
    int num2 = !this.hasStatus("surprised") ? 1 : 0;
    bool flag = pForceJump || this.isSurprisedJump(pTile);
    if (num2 != 0)
    {
      this.addStatusEffect("surprised", num1, false);
      if (this.hasStatus("just_ate"))
      {
        this.poop(false);
        flag = true;
      }
    }
    else
      num1 = 0.1f;
    if (flag)
    {
      this.addActionWaitAfterLand(num1);
      this.applyRandomForce();
    }
    this.addStatusEffect("on_guard", pColorEffect: false);
    if (flag || !this.isTask("investigate_curiosity") || !this.is_moving)
    {
      this.lookTowardsPosition(Vector2.op_Implicit(pTile.posV3));
      this.stopMovement();
      this.cancelAllBeh();
      if (!flag)
        this.makeWait(num1);
      this.scheduleTask("investigate_curiosity", pTile);
    }
    float pVal = 0.3f;
    if (this.hasSubspecies() && this.subspecies.has_trait_timid)
      pVal += 0.3f;
    if (!Randy.randomChance(pVal))
      return;
    this.cancelAllBeh();
    this.scheduleTask("run_away", (WorldTile) null);
  }

  public bool makeSleep(float pTime)
  {
    int num = this.addStatusEffect("sleeping", pTime) ? 1 : 0;
    if (num == 0)
      return num != 0;
    this.makeWait(pTime);
    return num != 0;
  }

  public void makeStunned(float pTime = 5f)
  {
    pTime += Randy.randomFloat(0.0f, pTime * 0.1f);
    this.cancelAllBeh();
    this.makeWait(pTime);
    if (!this.addStatusEffect("stunned", pTime))
      return;
    this.finishAngryStatus();
  }

  public void makeStunnedFromUI()
  {
    this.makeStunned();
    this.updateStats();
  }

  public void justAte() => this.addStatusEffect("just_ate");

  public void poop(bool pApplyForce)
  {
    this.donePooping();
    float pVal = 1f;
    string pAssetID;
    if (this.hasSubspecies())
    {
      pAssetID = this.subspecies.getRandomBioProduct();
      pVal = 0.2f;
    }
    else
      pAssetID = nameof (poop);
    if ((double) pVal >= 1.0 || Randy.randomChance(pVal))
      BuildingHelper.tryToBuildNear(this.current_tile, pAssetID);
    if (!pApplyForce)
      return;
    this.applyRandomForce();
  }

  public void donePooping()
  {
    this.finishStatusEffect("just_ate");
    this.changeHappiness("just_pooped");
  }

  public void birthEvent(string pAddSpecialTrait = null, string pAddSpecialStatus = null)
  {
    this.changeHappiness("just_had_child");
    this.makeStunned(4f);
    this.spendNutritionOnBirth();
    if (!string.IsNullOrEmpty(pAddSpecialTrait))
      this.addTrait(pAddSpecialTrait);
    if (string.IsNullOrEmpty(pAddSpecialStatus))
      return;
    this.addStatusEffect(pAddSpecialStatus);
  }

  public void consumeTopTile(WorldTile pTile)
  {
    if (Randy.randomChance(0.3f))
      World.world.units.addRandomTraitFromBiomeToActor(this, pTile);
    this.addNutritionFromEating(pTile.Type.nutrition, pSetJustAte: true);
    this.countConsumed();
    pTile.topTileEaten();
    pTile.setBurned();
  }

  public void countConsumed() => ++this.data.food_consumed;

  public void consumeFoodResource(ResourceAsset pAsset)
  {
    this.ate_last_item_id = pAsset.id;
    this.timestamp_session_ate_food = World.world.getCurSessionTime();
    if (pAsset.give_experience != 0)
      this.addExperience(pAsset.give_experience);
    if (pAsset.restore_happiness != 0)
      this.changeHappiness("just_ate", pAsset.restore_happiness);
    int restoreNutrition = pAsset.restore_nutrition;
    float restoreHealth = pAsset.restore_health;
    if (this.hasFavoriteFood())
    {
      if (pAsset.id != this.data.favorite_food)
      {
        ResourceAsset favoriteFoodAsset = this.favorite_food_asset;
        if ((double) pAsset.tastiness > (double) favoriteFoodAsset.tastiness && Randy.randomChance(pAsset.favorite_food_chance))
          this.data.favorite_food = pAsset.id;
      }
    }
    else if (Randy.randomChance(pAsset.favorite_food_chance))
      this.data.favorite_food = pAsset.id;
    if (pAsset.id == this.data.favorite_food)
    {
      restoreNutrition *= 2;
      restoreHealth *= 2f;
    }
    this.addNutritionFromEating(restoreNutrition, pSetJustAte: true);
    this.restoreHealthPercent(restoreHealth);
    this.countConsumed();
    if (!Randy.randomChance(pAsset.give_chance))
      return;
    ActorTrait[] giveTrait = pAsset.give_trait;
    if ((giveTrait != null ? (giveTrait.Length != 0 ? 1 : 0) : 0) != 0 && Randy.randomBool())
    {
      ActorTrait random = pAsset.give_trait.GetRandom<ActorTrait>();
      if (random != null)
        this.addTrait(random);
    }
    StatusAsset[] giveStatus = pAsset.give_status;
    if ((giveStatus != null ? (giveStatus.Length != 0 ? 1 : 0) : 0) != 0 && Randy.randomBool())
    {
      StatusAsset random = pAsset.give_status.GetRandom<StatusAsset>();
      if (random != null)
        this.addStatusEffect(random, 0.0f, true);
    }
    if (pAsset.give_action == null || !Randy.randomBool())
      return;
    pAsset.give_action(pAsset);
  }

  internal void justBorn() => this.setActorScale(0.02f);

  public void stopBeingWarrior()
  {
    if (this.isProfession(UnitProfession.Warrior))
    {
      this.setProfession(UnitProfession.Unit);
      if (this.hasCity())
        --this.city.status.warriors_current;
    }
    this.removeFromArmy();
  }

  public void pokeFromAvatarUI()
  {
    if (this.getHealth() > 1)
      this.getHit(1f, pAttackType: AttackType.Divine);
    if (Randy.randomChance(0.15f))
    {
      this.makeStunnedFromUI();
      this.changeHappiness("got_poked");
    }
    this.addStatusEffect("motivated");
    this.applyRandomForce();
    this.makeSoundAttack();
  }

  public void finishPossessionStatus()
  {
    this.finishStatusEffect("possessed");
    this._has_status_possessed = false;
  }

  public void madePeace(War pWar)
  {
    this.changeHappiness("just_made_peace");
    if (this.isKing())
      this.addRenown(pWar.getRenown(), 0.2f);
    if (this.isCityLeader())
      this.addRenown(pWar.getRenown(), 0.05f);
    if (this.is_army_captain)
      this.army.addRenown(pWar.getRenown(), 0.05f);
    if (!this.hasTag("love_peace"))
      return;
    this.addStatusEffect("festive_spirit");
  }

  public void warWon(War pWar)
  {
    if (!this.hasHappinessEntry("was_conquered", 300f))
    {
      if (this.isKing())
        this.addRenown(pWar.getRenown());
      if (this.isCityLeader())
        this.addRenown(pWar.getRenown(), 0.2f);
      if (this.isWarrior())
        this.addRenown(pWar.getRenown(), 0.05f);
      if (this.is_army_captain)
        this.army.addRenown(pWar.getRenown(), 0.05f);
      this.changeHappiness("just_won_war");
    }
    if (!this.hasTag("love_peace"))
      return;
    this.addStatusEffect("festive_spirit");
  }

  public void warLost(War pWar)
  {
    this.changeHappiness("just_lost_war");
    if (this.isKing())
      this.addRenown(pWar.getRenown(), 0.05f);
    if (!this.is_army_captain)
      return;
    this.army.addRenown(pWar.getRenown(), 0.01f);
  }

  public void setTransformed() => this.data.set("transformation_done", true);

  public bool isAlreadyTransformed()
  {
    bool pResult;
    this.data.get("transformation_done", out pResult);
    return pResult;
  }

  public void makeConfused(float pConfusedTimer = -1f, bool pColorEffect = false)
  {
    this.cancelAllBeh();
    if (pColorEffect)
      this.startColorEffect();
    this.addStatusEffect("confused", pConfusedTimer, pColorEffect);
    this.makeStunned(3f);
  }

  public void checkShouldBeEgg()
  {
    if (!this.hasSubspecies() || !this.subspecies.has_egg_form || (double) this.age >= (double) this.subspecies.age_adult)
      return;
    this.addStatusEffect("egg", this.getMaturationTimeSeconds());
  }

  public void leavePlot() => this.setPlot((Plot) null);

  private void levelUp()
  {
    int expToLevelup = this.getExpToLevelup();
    int maxPossibleLevel = this.getMaxPossibleLevel();
    this.data.experience = 0;
    ++this.data.level;
    if (this.hasCulture() && this.culture.hasTrait("training_potential"))
      ++this.data.level;
    if (this.data.level == maxPossibleLevel)
      this.data.experience = expToLevelup;
    this.setStatsDirty();
    EffectsLibrary.showMetaEventEffect("fx_experience_gain", this);
  }

  private void checkGrowthEvent()
  {
    bool flag = this.isBaby();
    int num = this.isEgg() ? 1 : 0;
    this.calcAgeStates();
    if (this.animation_container != null && this.animation_container.child != this.isBaby())
      this.clearSprites();
    if (num != 0 && !this.isEgg())
    {
      this.batch.c_events_hatched.Add(this);
    }
    else
    {
      if (!flag || this.isBaby())
        return;
      this.batch.c_events_become_adult.Add(this);
    }
  }

  internal void eventHatchFromEgg()
  {
    this.growthStateEvent();
    this.triggerHatchFromEggAction();
    this.applyRandomForce();
    this.changeHappiness("just_got_out_of_egg");
    this.batch.c_events_hatched.Remove(this);
  }

  internal void eventBecomeAdult()
  {
    this.growthStateEvent();
    this.changeHappiness("just_became_adult");
    this.checkTraitMutationGrowUp();
    this.batch.c_events_become_adult.Remove(this);
    this.subspecies.counter_new_adults?.registerEvent();
  }

  private void growthStateEvent()
  {
    this.setStatsDirty();
    this.event_full_stats = true;
    if (!this.hasCity())
      return;
    this.city.setCitizensDirty();
    this.city.setStatusDirty();
  }

  private void triggerHatchFromEggAction()
  {
    SubspeciesTrait eggAsset = this.subspecies.egg_asset;
    if (eggAsset == null || !eggAsset.has_after_hatch_from_egg_action)
      return;
    eggAsset.after_hatch_from_egg_action(this);
  }

  public bool checkNaturalDeath()
  {
    if (!WorldLawLibrary.world_law_old_age.isEnabled() || this.hasTrait("immortal"))
      return false;
    float age = (float) this.getAge();
    float stat = this.stats["lifespan"];
    if ((double) stat == 0.0 || (double) age <= (double) stat || !Randy.randomChance(Mathf.Clamp((float) (1.0 / (1.0 + (double) Mathf.Exp((float) (-5.0 * ((double) (age - stat) / (double) stat - 0.5))))), 0.0f, 0.9f)))
      return false;
    this.getHitFullHealth(AttackType.Age);
    return true;
  }

  public void spawnParticle(Color pColor)
  {
    if (Randy.randomBool() || !MapBox.isRenderGameplay())
      return;
    Vector3 pVector = Vector2.op_Implicit(this.current_position);
    pVector.y += (float) (0.5 * (double) this.current_scale.y / 2.0);
    pVector.x += Randy.randomFloat(-0.2f, 0.2f);
    pVector.y += Randy.randomFloat(-0.2f, 0.2f);
    BaseEffect baseEffect = EffectsLibrary.spawn("fx_status_particle");
    if (!Object.op_Inequality((Object) baseEffect, (Object) null))
      return;
    ((StatusParticle) baseEffect).spawnParticle(pVector, pColor);
  }

  private void checkActionsFromAllMetas()
  {
    if (this.hasSubspecies())
    {
      this.addSpecialEffectsFromMetas(this.subspecies.all_actions_actor_special_effect);
      this.s_action_attack_target += this.subspecies.all_actions_actor_attack_target;
      this.s_get_hit_action += this.subspecies.all_actions_actor_get_hit;
    }
    if (this.hasClan())
    {
      this.addSpecialEffectsFromMetas(this.clan.all_actions_actor_special_effect);
      this.s_action_attack_target += this.clan.all_actions_actor_attack_target;
      this.s_get_hit_action += this.clan.all_actions_actor_get_hit;
    }
    if (this.hasLanguage())
    {
      this.addSpecialEffectsFromMetas(this.language.all_actions_actor_special_effect);
      this.s_action_attack_target += this.language.all_actions_actor_attack_target;
      this.s_get_hit_action += this.language.all_actions_actor_get_hit;
    }
    if (this.hasCulture())
    {
      this.addSpecialEffectsFromMetas(this.culture.all_actions_actor_special_effect);
      this.s_action_attack_target += this.culture.all_actions_actor_attack_target;
      this.s_get_hit_action += this.culture.all_actions_actor_get_hit;
    }
    if (!this.hasReligion())
      return;
    this.addSpecialEffectsFromMetas(this.religion.all_actions_actor_special_effect);
    this.s_action_attack_target += this.religion.all_actions_actor_attack_target;
    this.s_get_hit_action += this.religion.all_actions_actor_get_hit;
  }

  private void recalcCombatActions()
  {
    foreach (ActorTrait trait in this.traits)
    {
      if (trait.hasCombatActions())
        this._combat_actions.mergeWith(trait.combat_actions);
    }
    this.checkCombatActions(this.subspecies?.combat_actions);
    this.checkCombatActions(this.clan?.combat_actions);
    this.checkCombatActions(this.religion?.combat_actions);
  }

  private void recalcSpells()
  {
    foreach (ActorTrait trait in this.traits)
    {
      if (trait.hasSpells())
        this._spells.mergeWith((IReadOnlyList<SpellAsset>) trait.spells);
    }
    if (!this.hasEquipment())
      return;
    foreach (ActorEquipmentSlot actorEquipmentSlot in this.equipment)
    {
      if (!actorEquipmentSlot.isEmpty())
      {
        Item obj = actorEquipmentSlot.getItem();
        if (obj.asset.hasSpells())
          this._spells.mergeWith((IReadOnlyList<SpellAsset>) obj.asset.spells);
      }
    }
  }

  private void checkSpells(SpellHolder pSpellsHolder)
  {
    if (pSpellsHolder == null || !pSpellsHolder.hasAny())
      return;
    this._spells.mergeWith(pSpellsHolder);
  }

  private void checkCombatActions(CombatActionHolder pHolder)
  {
    if (pHolder == null || pHolder.isEmpty())
      return;
    this._combat_actions.mergeWith(pHolder);
  }

  public List<CombatActionAsset> getCombatActionPool(CombatActionPool pPool)
  {
    return !this._combat_actions.hasAny() ? (List<CombatActionAsset>) null : this._combat_actions.getPool(pPool);
  }

  private void clearCombatActions() => this._combat_actions.reset();

  private void clearSpells() => this._spells.reset();

  private bool checkCurrentEnemyTarget()
  {
    if (this.shouldSkipFightCheck() || !this.has_attack_target || !this.isEnemyTargetAlive())
      return false;
    BaseSimObject attackTarget = this.attack_target;
    Actor a = this.attack_target.a;
    if (this.isKingdomCiv() && attackTarget.isKingdomCiv() && !this.shouldContinueToAttackTarget())
    {
      this.clearAttackTarget();
      return false;
    }
    if (attackTarget.isActor() && !this.hasStatusTantrum() && !attackTarget.areFoes((BaseSimObject) this) && attackTarget.a.is_unconscious)
    {
      this.clearAttackTarget();
      return false;
    }
    if (this.canAttackTarget(attackTarget, pAttackBuildings: this.asset.can_attack_buildings))
    {
      bool flag1 = this.isAttackPossible();
      bool flag2 = this.isInAttackRange(attackTarget);
      if (!flag2)
      {
        float objectTarget = this.distanceToObjectTarget(attackTarget);
        if ((double) objectTarget > 20.0 && a != null && a.isTask("run_away"))
        {
          this.clearAttackTarget();
          return false;
        }
        if ((double) objectTarget > 50.0)
        {
          this.clearAttackTarget();
          return false;
        }
        CombatActionAsset pResultCombatAsset;
        if ((double) objectTarget > 3.0 && this.tryToUseAdvancedCombatAction(this.getCombatActionPool(CombatActionPool.BEFORE_ATTACK_MELEE), attackTarget, out pResultCombatAsset))
        {
          int num = pResultCombatAsset.action_actor_target_position(this, attackTarget.current_position, attackTarget.current_tile) ? 1 : 0;
          return false;
        }
      }
      if ((double) this.attack_timer > 0.0 || !flag1 & flag2)
      {
        this.stopMovement();
        CombatActionAsset pResultCombatAsset;
        if (this.hasRangeAttack() && this.tryToUseAdvancedCombatAction(this.getCombatActionPool(CombatActionPool.BEFORE_ATTACK_RANGE), attackTarget, out pResultCombatAsset))
        {
          int num = pResultCombatAsset.action_actor_target_position(this, attackTarget.current_position, attackTarget.current_tile) ? 1 : 0;
        }
        return true;
      }
      if (flag2 && this.tryToAttack(attackTarget, false, pAttackPosition: new Vector3()))
      {
        this.stopMovement();
        return true;
      }
    }
    return false;
  }

  private bool checkEnemyTargets()
  {
    if (!this.isAllowedToLookForEnemies() || this.isInWaterAndCantAttack() || this._has_status_strange_urge)
      return false;
    if (this.has_attack_target)
    {
      if (!this.hasTask() || !this.ai.task.in_combat)
        this.setTask("fighting", pCleanJob: true);
      return false;
    }
    if ((double) this._timeout_targets > 0.0)
      return false;
    this._timeout_targets = 0.1f + Randy.randomFloat(0.0f, 1f);
    BaseSimObject pSimObject = this.findEnemyObjectTarget(this.asset.can_attack_buildings);
    if (pSimObject == null && this._aggression_targets.Count > 0)
    {
      using (ListPool<Actor> pList = new ListPool<Actor>(this._aggression_targets.Count))
      {
        foreach (long aggressionTarget in this._aggression_targets)
        {
          Actor pObject = World.world.units.get(aggressionTarget);
          if (!pObject.isRekt())
            pList.Add(pObject);
        }
        if (pList.Count > 0)
          pSimObject = this.checkObjectList((IEnumerable<BaseSimObject>) pList, this.asset.can_attack_buildings, true, true, 30);
        else
          this._aggression_targets.Clear();
      }
    }
    if (pSimObject == null)
      return false;
    this.startFightingWith(pSimObject);
    return true;
  }

  public void startFightingWith(BaseSimObject pSimObject)
  {
    this.setAttackTarget(pSimObject);
    this.setTask("fighting", false, true);
    this.beh_actor_target = pSimObject;
  }

  internal void startAttackCooldown()
  {
    this.attack_timer = this.getAttackCooldown();
    this.last_attack_timestamp = World.world.getCurWorldTime();
  }

  internal bool isJustAttacked()
  {
    return (double) World.world.getWorldTimeElapsedSince(this.last_attack_timestamp) < 0.25;
  }

  internal bool tryToAttack(
    BaseSimObject pTarget,
    bool pDoChecks = true,
    Action pKillAction = null,
    Vector3 pAttackPosition = default (Vector3),
    Kingdom pForceKingdom = null,
    WorldTile pTileTarget = null,
    float pBonusAreOfEffect = 0.0f)
  {
    if (pDoChecks && (this.hasMeleeAttack() && pTarget != null && (double) pTarget.position_height > 0.0 || this.isInWaterAndCantAttack() || !this.isAttackPossible() || pTarget != null && !this.isInAttackRange(pTarget)))
      return false;
    float num1 = 0.0f;
    float num2 = 0.0f;
    Vector3 pDirection;
    if (pTarget != null)
    {
      pDirection = Vector2.op_Implicit(pTarget.current_position);
      num1 = pTarget.getHeight();
      num2 = pTarget.stats["size"];
    }
    else
      pDirection = pAttackPosition;
    bool hasStatusPossessed = this._has_status_possessed;
    this.startAttackCooldown();
    this.punchTargetAnimation(pDirection, pReverse: this.hasRangeAttack());
    Vector3 vector3_1;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3_1).\u002Ector(pDirection.x, pDirection.y);
    if (pTarget != null && pTarget.isActor() && pTarget.a.is_moving && pTarget.isFlying())
      vector3_1 = Vector3.MoveTowards(vector3_1, Vector2.op_Implicit(pTarget.a.next_step_position), num2 * 3f);
    Vector3 vector3_2 = Vector2.op_Implicit(this.current_position);
    float num3 = Vector2.Distance(Vector2.op_Implicit(vector3_2), Vector2.op_Implicit(pDirection)) + num1;
    Vector3 newPoint = Toolbox.getNewPoint(vector3_2.x, vector3_2.y, vector3_1.x, vector3_1.y, num3 - num2);
    string projectile = this.getWeaponAsset().projectile;
    bool pProjectile = this.hasRangeAttack();
    Kingdom kingdom = pForceKingdom ?? this.kingdom;
    WorldTile pHitTile = pTileTarget ?? pTarget?.current_tile;
    Kingdom pKingdom = kingdom;
    Vector3 pInitiatorPosition = vector3_2;
    AttackData pData = new AttackData((BaseSimObject) this, pHitTile, newPoint, pInitiatorPosition, pTarget, pKingdom, AttackType.Weapon, this.haveMetallicWeapon(), pProjectile: pProjectile, pProjectileID: projectile, pKillAction: pKillAction, pBonusAreOfEffect: pBonusAreOfEffect);
    using (ListPool<CombatActionAsset> listPool = new ListPool<CombatActionAsset>())
    {
      if (this.hasSpells() && this.canUseSpells() && !hasStatusPossessed)
        this.addToAttackPool(CombatActionLibrary.combat_cast_spell, listPool);
      CombatActionAsset combatActionAsset;
      bool flag;
      if (listPool.Count > 0)
      {
        if (this.hasMeleeAttack())
          this.addToAttackPool(CombatActionLibrary.combat_attack_melee, listPool);
        else
          this.addToAttackPool(CombatActionLibrary.combat_attack_range, listPool);
        combatActionAsset = listPool.GetRandom<CombatActionAsset>();
        flag = combatActionAsset.action(pData);
        if (!flag && !combatActionAsset.basic)
          flag = !this.hasMeleeAttack() ? CombatActionLibrary.combat_attack_range.action(pData) : CombatActionLibrary.combat_attack_melee.action(pData);
      }
      else
      {
        combatActionAsset = !this.hasMeleeAttack() ? CombatActionLibrary.combat_attack_range : CombatActionLibrary.combat_attack_melee;
        flag = combatActionAsset.action(pData);
      }
      if (flag)
      {
        this.spendStamina(combatActionAsset.cost_stamina);
        this.spendMana(combatActionAsset.cost_mana);
      }
      if (combatActionAsset.play_unit_attack_sounds)
        this.makeSoundAttack();
      if (this.needsFood() && Randy.randomBool())
        this.decreaseNutrition();
      float pForceAmountDirection = this.stats.get("recoil");
      if ((double) pForceAmountDirection > 0.0)
        this.calculateForce(this.current_position.x, this.current_position.y, vector3_1.x, vector3_1.y, pForceAmountDirection);
      return true;
    }
  }

  internal override void getHitFullHealth(AttackType pAttackType)
  {
    this.getHit((float) this.getHealth(), false, pAttackType, (BaseSimObject) null, false, false, false);
  }

  internal override void getHit(
    float pDamage,
    bool pFlash,
    AttackType pAttackType,
    BaseSimObject pAttacker = null,
    bool pSkipIfShake = true,
    bool pMetallicWeapon = false,
    bool pCheckDamageReduction = true)
  {
    this._last_attack_type = pAttackType;
    if (this._cache_check_has_status_removed_on_damage)
    {
      foreach (Status statuse in this.getStatuses())
      {
        if (!statuse.is_finished && statuse.asset.removed_on_damage)
          this.finishStatusEffect(statuse.asset.id);
      }
    }
    if (DebugConfig.isOn(DebugOption.IgnoreDamage) || pSkipIfShake && this._shake_active)
      return;
    this.attackedBy = (BaseSimObject) null;
    if (pAttacker.isRekt())
      pAttacker = (BaseSimObject) null;
    if (pAttacker != this)
      this.attackedBy = pAttacker;
    if (!this.hasHealth() || this.is_invincible)
      return;
    Actor a = pAttacker?.a;
    if (pAttackType == AttackType.Weapon)
    {
      bool flag = false;
      if (pMetallicWeapon && this.haveMetallicWeapon())
        flag = true;
      if (flag)
        MusicBox.playSound("event:/SFX/HIT/HitSwordSword", this.current_tile, pVisibleOnly: true);
      else if (this.asset.has_sound_hit)
        MusicBox.playSound(this.asset.sound_hit, this.current_tile, pVisibleOnly: true);
      if (a != null && !this.hasStatus("shield"))
        this.damageEquipmentOnGetHit(a);
    }
    if (pCheckDamageReduction)
    {
      if (pAttackType == AttackType.Other || pAttackType == AttackType.Weapon)
      {
        float num = (float) (1.0 - (double) this.stats["armor"] / 100.0);
        pDamage *= num;
      }
      if ((double) pDamage < 1.0)
        pDamage = 1f;
      if (a != null)
      {
        float pDamageFinal;
        this.checkSpecialAttackLogic(a, pAttackType, pDamage, out pDamageFinal);
        pDamage = pDamageFinal;
        AchievementLibrary.clone_wars.checkBySignal((object) (this, a));
      }
    }
    this.changeHealth((int) -(double) pDamage);
    this.timer_action = 1f / 500f;
    GetHitAction getHitAction = this.s_get_hit_action;
    if (getHitAction != null)
    {
      int num1 = getHitAction((BaseSimObject) this, pAttacker, this.current_tile) ? 1 : 0;
    }
    if (pFlash)
      this.startColorEffect(ActorColorEffect.Red);
    if (!this.hasHealth())
      this.batch.c_check_deaths.Add(this);
    if (pAttackType == AttackType.Weapon && !this.asset.immune_to_injuries && !this.hasStatus("shield"))
    {
      if (Randy.randomChance(0.02f))
        this.addInjuryTrait("crippled");
      if (Randy.randomChance(0.02f))
        this.addInjuryTrait("eyepatch");
    }
    this.startShake();
    if (!this.has_attack_target)
    {
      if (this.attackedBy != null && !this.shouldIgnoreTarget(this.attackedBy) && this.canAttackTarget(this.attackedBy, false))
        this.setAttackTarget(this.attackedBy);
    }
    else if (this.hasMeleeAttack() && this.attackedBy != null && this.canAttackTarget(this.attackedBy, false))
    {
      float num2 = Toolbox.SquaredDistVec2Float(this.current_position, this.attack_target.current_position);
      float num3 = Toolbox.SquaredDistVec2Float(this.current_position, pAttacker.current_position);
      if ((double) num2 > (double) this.getAttackRangeSquared() && (double) num3 < (double) num2)
        this.setAttackTarget(pAttacker);
    }
    if (this.hasAnyStatusEffect())
    {
      foreach (Status statuse in this.getStatuses())
      {
        GetHitAction actionGetHit = statuse.asset.action_get_hit;
        if (actionGetHit != null)
        {
          int num4 = actionGetHit((BaseSimObject) this, pAttacker, this.current_tile) ? 1 : 0;
        }
      }
    }
    GetHitAction actionGetHit1 = this.asset.action_get_hit;
    if (actionGetHit1 != null)
    {
      int num5 = actionGetHit1((BaseSimObject) this, pAttacker, this.current_tile) ? 1 : 0;
    }
    if (this.hasHealth())
      return;
    this.checkCallbacksOnDeath();
  }

  private void pickupResourcesFromKill(Actor pAttacker)
  {
    if (!pAttacker.hasCity())
      return;
    foreach (ResourceContainer pResourceContainer in this.getResourcesFromActor())
    {
      if (!this.isSameSpecies(pAttacker) || pAttacker.hasTrait("savage"))
        pAttacker.addToInventory(pResourceContainer);
    }
  }

  private void checkSpecialAttackLogic(
    Actor pAttacker,
    AttackType pAttackType,
    float pInitialDamage,
    out float pDamageFinal)
  {
    pDamageFinal = pInitialDamage;
    bool flag1 = this.isSameKingdom((BaseSimObject) pAttacker);
    bool flag2 = false;
    bool flag3 = pAttacker.hasStatus("tantrum") && !flag1;
    bool pCheckAllLists = pAttacker.hasStatus("possessed");
    int num = this.kingdom.isEnemy(pAttacker.kingdom) ? 1 : 0;
    float pVal = 0.1f;
    if (this._has_status_possessed | pCheckAllLists)
      pVal = 0.7f;
    else if (flag1)
      pVal = 0.5f;
    if (num != 0)
      pVal = 0.0f;
    bool flag4 = Randy.randomChance(pVal);
    if (flag3)
      flag4 = true;
    if ((double) this.getHealthRatio() < 0.5 & flag4)
    {
      pDamageFinal = 1f;
      this.makeStunned();
      this.changeHappiness("lost_fight");
      this.finishAngryStatus();
      flag2 = true;
      if (flag3)
        pAttacker.finishStatusEffect("tantrum");
      if (Randy.randomChance(0.4f))
        pAttacker.finishAngryStatus();
    }
    bool flag5 = false;
    if (flag1 && pAttackType != AttackType.Eaten)
    {
      if (Randy.randomChance(0.3f) | pCheckAllLists || pAttacker.hasStatus("angry"))
      {
        this.checkAggroAgainst(pAttacker, pCheckAllLists);
        flag5 = true;
      }
      if (flag2)
      {
        pDamageFinal = 0.0f;
        pAttacker.clearAttackTarget();
        pAttacker.makeWait(0.3f);
        if (pAttacker.hasStatus("angry"))
          pAttacker.finishAngryStatus();
      }
    }
    if (!(!flag5 & pCheckAllLists))
      return;
    this.checkAggroAgainst(pAttacker);
  }

  private void damageEquipmentOnGetHit(Actor pAttacker)
  {
    if (!pAttacker.hasWeapon() || !this.hasEquipment())
      return;
    int num1 = 4;
    float pVal = 0.35f;
    Item weapon = pAttacker.getWeapon();
    EquipmentAsset asset1 = weapon.getAsset();
    int rigidityRating1 = asset1.rigidity_rating;
    int num2 = 0;
    bool flag = false;
    foreach (ActorEquipmentSlot actorEquipmentSlot in this.equipment)
    {
      if (!Randy.randomBool())
      {
        Item obj = actorEquipmentSlot.getItem();
        if (!obj.isBroken())
        {
          EquipmentAsset asset2 = obj.getAsset();
          int rigidityRating2 = asset2.rigidity_rating;
          if (!asset2.is_pool_weapon)
            num2 += rigidityRating2;
          int pDamage = rigidityRating1 / rigidityRating2 * num1;
          obj.getDamaged(pDamage);
          if (obj.isBroken())
            flag = true;
        }
      }
    }
    if (flag)
      this.setStatsDirty();
    if (weapon.isBroken() || Randy.randomBool())
      return;
    if (asset1.attack_type == WeaponType.Melee)
    {
      int pDamage = num2 / 5 / rigidityRating1 * num1;
      weapon.getDamaged(pDamage);
    }
    else if (asset1.attack_type == WeaponType.Range && Randy.randomChance(pVal))
      weapon.getDamaged(1);
    if (!weapon.isBroken())
      return;
    pAttacker.setStatsDirty();
  }

  public void addInjuryTrait(string pTraitID)
  {
    if (!this.addTrait(pTraitID))
      return;
    this.changeHappiness("just_injured");
  }

  private void checkAggroAgainst(Actor pAttackedBy, bool pCheckAllLists = false)
  {
    this.addAggro(pAttackedBy);
    if (!pCheckAllLists)
      return;
    if (this.hasFamily())
      this.family.allAngryAt(pAttackedBy, 10f);
    if (this.hasClan())
      this.clan.allAngryAt(pAttackedBy, 10f);
    if (this.hasCity() && this.isBaby())
      this.city.allAngryAt(pAttackedBy, 10f);
    if (this.hasLover())
      this.lover.addAggro(pAttackedBy);
    if (this.hasBestFriend())
      this.getBestFriend().addAggro(pAttackedBy);
    if (!this.isKing() && !this.isWarrior() && !this.isCityLeader() && !this.isBaby() || pAttackedBy.isKing())
      return;
    foreach (City city in this.kingdom.getCities())
    {
      if (city.hasArmy())
        city.army.allAngryAt(pAttackedBy, 10f);
    }
  }

  internal void newKillAction(Actor pDeadUnit, Kingdom pPrevKingdom, AttackType pAttackType)
  {
    this.increaseKills();
    if (this.hasWeapon())
      this.getWeapon().countKill();
    if (this.isKingdomCiv() && pPrevKingdom.isCiv())
    {
      War war = World.world.wars.getWar(this.kingdom, pPrevKingdom, false);
      if (war != null)
      {
        if (war.isAttacker(this.kingdom))
          war.increaseDeathsDefenders(pAttackType);
        else
          war.increaseDeathsAttackers(pAttackType);
      }
    }
    if (!this.isAlive())
      return;
    if ((double) this.timer_action <= 0.0)
      this.makeWait(Randy.randomFloat(0.1f, 1f));
    if (this.hasTrait("bloodlust"))
      this.changeHappiness("just_killed");
    this.addLoot(pDeadUnit.giveAllLootAndMoney());
    this.takeAllResources(pDeadUnit);
    if (this.data.kills > 10)
      this.addTrait("veteran");
    if (pDeadUnit.isKing())
      this.addTrait("kingslayer");
    this.addExperience(pDeadUnit.asset.experience_given);
    this.addRenown(pDeadUnit.asset.experience_given);
    if (this.hasTrait("madness"))
      this.restoreHealth(this.getMaxHealthPercent(0.05f));
    if (this.understandsHowToUseItems() && !pDeadUnit.hasTrait("infected") && this.canTakeItems())
      this.takeItems(pDeadUnit);
    this.checkRageDemon();
  }

  internal void applyRandomForce(float pMinHeight = 1.5f, float pMaxHeight = 2f)
  {
    float pForceAmountDirection = Randy.randomFloat(1.5f, 2f);
    float pForceHeight = Randy.randomFloat(pMinHeight, pMaxHeight);
    WorldTile random = this.current_tile.neighboursAll.GetRandom<WorldTile>();
    this.calculateForce(this.current_tile.posV3.x, this.current_tile.posV3.y, random.posV3.x, random.posV3.y, pForceAmountDirection, pForceHeight, true);
  }

  internal void calculateForce(
    float pStartX,
    float pStartY,
    float pTargetX,
    float pTargetY,
    float pForceAmountDirection,
    float pForceHeight = 0.0f,
    bool pCheckCancelJobOnLand = false)
  {
    if ((double) pForceHeight == 0.0)
      pForceHeight = pForceAmountDirection;
    pForceAmountDirection *= SimGlobals.m.unit_force_multiplier;
    pForceHeight *= SimGlobals.m.unit_force_multiplier;
    if ((double) pForceAmountDirection <= 0.0)
      return;
    double angle = (double) Toolbox.getAngle(pStartX, pStartY, pTargetX, pTargetY);
    float pX = -Mathf.Cos((float) angle) * pForceAmountDirection;
    float pY = -Mathf.Sin((float) angle) * pForceAmountDirection;
    if ((double) pStartX == (double) pTargetX && (double) pStartY == (double) pTargetY)
    {
      pX = 0.0f;
      pY = 0.0f;
    }
    this.addForce(pX, pY, pForceHeight, pCheckCancelJobOnLand);
  }

  public bool tryToUseAdvancedCombatAction(
    List<CombatActionAsset> pCombatActionAssetsCategory,
    BaseSimObject pAttackTarget,
    out CombatActionAsset pResultCombatAsset)
  {
    pResultCombatAsset = (CombatActionAsset) null;
    if (pCombatActionAssetsCategory == null || pCombatActionAssetsCategory.Count == 0 || this.hasTrait("slow") || this.combatActionOnTimeout())
      return false;
    using (ListPool<CombatActionAsset> list = new ListPool<CombatActionAsset>(pCombatActionAssetsCategory.Count))
    {
      foreach (CombatActionAsset combatActionAsset in pCombatActionAssetsCategory)
      {
        if (this.hasEnoughStamina(combatActionAsset.cost_stamina) && this.hasEnoughMana(combatActionAsset.cost_mana))
        {
          if (pAttackTarget != null)
          {
            CombatActionCheckStart canDoAction = combatActionAsset.can_do_action;
            if ((canDoAction != null ? (!canDoAction(this, pAttackTarget) ? 1 : 0) : 0) != 0)
              continue;
          }
          list.Add(combatActionAsset);
        }
      }
      if (list.Count == 0)
        return false;
      CombatActionAsset random = list.GetRandom<CombatActionAsset>();
      if (!Randy.randomChance(random.chance + random.chance * this.stats["skill_combat"]))
        return false;
      this.spendStamina(random.cost_stamina);
      this.spendMana(random.cost_mana);
      pResultCombatAsset = random;
      this.addStatusEffect("recovery_combat_action", pResultCombatAsset.cooldown, false);
      return true;
    }
  }

  public void addAggro(long pActorID)
  {
    Actor actor = World.world.units.get(pActorID);
    if (actor.isRekt())
      return;
    this.addAggro(actor);
  }

  public void addAggro(Actor pActor)
  {
    if (pActor.isRekt() || pActor == this)
      return;
    this.addStatusEffect("angry", pColorEffect: false);
    this._aggression_targets.Add(pActor.getID());
  }

  public void finishAngryStatus()
  {
    this._aggression_targets.Clear();
    this.finishStatusEffect("angry");
  }

  public void spawnSlashPunch(Vector2 pTowardsPosition)
  {
    this.spawnSlash(pTowardsPosition, "effects/slashes/slash_punch");
  }

  public void spawnSlashSteal(Vector2 pTowardsPosition)
  {
    this.spawnSlash(pTowardsPosition, "effects/slashes/slash_steal");
  }

  public void spawnSlashYell(Vector2 pTowardsPosition)
  {
    this.spawnSlash(pTowardsPosition, "effects/slashes/slash_swear");
  }

  public void spawnSlashTalk(Vector2 pTowardsPosition)
  {
    this.spawnSlash(pTowardsPosition, "effects/slashes/slash_talk");
  }

  public void spawnSlashKick(Vector2 pTowardsPosition)
  {
    this.spawnSlash(pTowardsPosition, "effects/slashes/slash_kick", pStartZ: (float) (-(double) this.actor_scale * 8.0));
  }

  public void spawnSlash(
    Vector2 pTowardsPosition,
    string pSlashType = null,
    float pDistMod = 2f,
    float pTargetZ = 0.0f,
    float pStartZ = 0.0f,
    float? pAngle = null)
  {
    if (!this.is_visible || !EffectsLibrary.canShowSlashEffect())
      return;
    if (string.IsNullOrEmpty(pSlashType))
      pSlashType = this._attack_asset.path_slash_animation;
    Vector2 slashPosition = this.getSlashPosition(this, pTowardsPosition, pDistMod, pTargetZ, pStartZ);
    float pAngle1 = pAngle.HasValue ? pAngle.Value : this.getSlashAngle(slashPosition, pTowardsPosition);
    EffectsLibrary.spawnSlash(slashPosition, pSlashType, pAngle1, this.actor_scale);
  }

  public float getSlashAngle(Vector2 pSlashVector, Vector2 pAttackPosition)
  {
    return Toolbox.getAngleDegrees(pSlashVector.x, pSlashVector.y, pAttackPosition.x, pAttackPosition.y);
  }

  public Vector2 getSlashPosition(
    Actor pActor,
    Vector2 pAttackPosition,
    float pDistMod,
    float pTargetZ = 0.0f,
    float pStartZ = 0.0f)
  {
    float scaleMod = pActor.getScaleMod();
    double stat = (double) pActor.stats["size"];
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(pActor.current_position.x, pActor.current_position.y);
    vector2.y += pActor.getHeight();
    vector2.y += 0.5f * scaleMod;
    vector2.y += pStartZ;
    double num = (double) scaleMod;
    float pDist = (float) (stat * num) * pDistMod;
    return Toolbox.getNewPointVec2(vector2.x, vector2.y, pAttackPosition.x, pAttackPosition.y + pTargetZ, pDist);
  }

  public void doCastAnimation()
  {
    if (!this.is_visible)
      return;
    Vector2 renderedItemPosition = this.getRenderedItemPosition();
    Vector3 transformPosition = this.cur_transform_position;
    EffectsLibrary.spawnAt(this.asset.effect_cast_ground, transformPosition, this.stats["scale"]);
    transformPosition.y += renderedItemPosition.y * 6f * this.current_scale.y;
    EffectsLibrary.spawnAt(this.asset.effect_cast_top, transformPosition, this.stats["scale"]);
  }

  internal void punchTargetAnimation(Vector3 pDirection, bool pFlip = true, bool pReverse = false, float pAngle = 40f)
  {
    if (!this.asset.can_flip)
      return;
    if (pFlip && this.checkFlip())
    {
      if ((double) this.current_position.x < (double) pDirection.x)
        this.setFlip(true);
      else
        this.setFlip(false);
    }
    if (pReverse)
      this.target_angle.z = -pAngle;
    else
      this.target_angle.z = pAngle;
  }

  private void addToAttackPool(CombatActionAsset pAsset, ListPool<CombatActionAsset> pPool)
  {
    for (int index = 0; index < pAsset.rate; ++index)
      pPool.Add(pAsset);
  }

  private void checkHappinessChangeFromDeathEvent()
  {
    foreach (Actor parent in this.getParents())
      parent.changeHappiness("death_child");
    this.getBestFriend()?.changeHappiness("death_best_friend");
    if (this.hasLover())
    {
      this.lover.changeHappiness("death_lover");
      this.lover.finishStatusEffect("fell_in_love");
    }
    if (!this.hasFamily())
      return;
    foreach (Actor unit in this.family.units)
    {
      if (unit != this && !unit.isParentOf(this))
        unit.changeHappiness("death_family_member");
    }
  }

  private void checkCallbacksOnDeath()
  {
    WorldAction unitDeathAction = this.current_tile.Type.unit_death_action;
    if (unitDeathAction != null)
    {
      int num1 = unitDeathAction((BaseSimObject) this, this.current_tile) ? 1 : 0;
    }
    WorldAction actionDeath1 = this.asset.action_death;
    if (actionDeath1 != null)
    {
      int num2 = actionDeath1((BaseSimObject) this, this.current_tile) ? 1 : 0;
    }
    using (ListPool<ActorTrait> listPool = new ListPool<ActorTrait>((IEnumerable<ActorTrait>) this.getTraits()))
    {
      for (int index = 0; index < listPool.Count; ++index)
      {
        WorldAction actionDeath2 = listPool[index].action_death;
        if (actionDeath2 != null)
        {
          int num3 = actionDeath2((BaseSimObject) this, this.current_tile) ? 1 : 0;
        }
      }
      if (this.hasAnyStatusEffect())
      {
        foreach (Status statuse in this.getStatuses())
        {
          if (statuse.asset.action_death != null)
          {
            int num4 = statuse.asset.action_death((BaseSimObject) this, this.current_tile) ? 1 : 0;
          }
        }
      }
      if (this.hasClan())
      {
        WorldAction actionsActorDeath = this.clan.all_actions_actor_death;
        if (actionsActorDeath != null)
        {
          int num5 = actionsActorDeath((BaseSimObject) this, this.current_tile) ? 1 : 0;
        }
      }
      if (this.hasSubspecies())
      {
        WorldAction actionsActorDeath = this.subspecies.all_actions_actor_death;
        if (actionsActorDeath != null)
        {
          int num6 = actionsActorDeath((BaseSimObject) this, this.current_tile) ? 1 : 0;
        }
      }
      if (this.hasReligion())
      {
        WorldAction actionsActorDeath = this.religion.all_actions_actor_death;
        if (actionsActorDeath != null)
        {
          int num7 = actionsActorDeath((BaseSimObject) this, this.current_tile) ? 1 : 0;
        }
      }
      BaseActionActor callbacksOnDeath = this.callbacks_on_death;
      if (callbacksOnDeath == null)
        return;
      callbacksOnDeath(this);
    }
  }

  public void checkDeath()
  {
    if (this.hasHealth() || !this.isAlive())
      return;
    Kingdom kingdom = this.kingdom;
    Actor actor = (Actor) null;
    if (!this.attackedBy.isRekt() && this.attackedBy.isActor() && this.attackedBy != this)
      actor = this.attackedBy.a;
    if (this._last_attack_type == AttackType.Weapon && (this.isKingdomCiv() || !actor.isRekt() && actor.isKingdomCiv()))
      BattleKeeperManager.addUnitKilled(this);
    if (actor != null)
    {
      actor.newKillAction(this, kingdom, this._last_attack_type);
      this.pickupResourcesFromKill(actor);
    }
    this.die(pType: this._last_attack_type);
  }

  public void dieSimpleNone() => this.die(pType: AttackType.None, pCountDeath: false);

  public void dieAndDestroy(AttackType pType) => this.die(true, pType, false);

  public void removeByMetamorphosis() => this.die(true, AttackType.Metamorphosis, false, false);

  private void die(bool pDestroy = false, AttackType pType = AttackType.Other, bool pCountDeath = true, bool pLogFavorite = true)
  {
    if (!this.isAlive() && !pDestroy)
      return;
    switch (pType)
    {
      case AttackType.Plague:
      case AttackType.Infection:
      case AttackType.Tumor:
      case AttackType.Divine:
      case AttackType.AshFever:
      case AttackType.Metamorphosis:
      case AttackType.Starvation:
      case AttackType.Age:
      case AttackType.None:
      case AttackType.Poison:
      case AttackType.Gravity:
      case AttackType.Drowning:
        this.attackedBy = (BaseSimObject) null;
        break;
    }
    SelectedUnit.removeSelected(this);
    if (ControllableUnit.isControllingUnit(this))
    {
      ControllableUnit.remove(this);
      if (this.asset.id == "crabzilla")
        pDestroy = true;
    }
    if (this.isAlive())
    {
      this.setAlive(false);
      this.skipUpdates();
      if (this.is_inside_boat)
      {
        this.inside_boat.removePassenger(this);
        this.exitBoat();
      }
      if (pCountDeath)
      {
        this.countDeath(pType);
        this.checkHappinessChangeFromDeathEvent();
      }
      if (this.isFavorite() & pLogFavorite)
      {
        if (!this.attackedBy.isRekt() && this.attackedBy.isActor())
          WorldLog.logFavMurder(this, this.attackedBy.a);
        else
          WorldLog.logFavDead(this);
      }
      this.clearTasks();
    }
    this.exitBuilding();
    this.clearHomeBuilding();
    using (ListPool<Item> pItems = new ListPool<Item>())
    {
      if (this.hasEquipment())
      {
        pItems.AddRange(this.equipment.getItems());
        this.takeAwayItems();
      }
      if (this.current_tile.zone.hasCity())
      {
        this.current_tile.zone.city.tryToPutItems((IEnumerable<Item>) pItems);
        pItems.Clear();
      }
      if (pDestroy)
        World.world.units.scheduleDestroyOnPlay(this);
      if (this.isKing())
      {
        this.kingdom.removeKing();
        this.kingdom.logKingDead(this);
      }
      if (this.hasCity())
      {
        this.stopBeingWarrior();
        if (pType == AttackType.Age)
        {
          this.city.tryToPutItems((IEnumerable<Item>) pItems);
          pItems.Clear();
        }
        this.setCity((City) null);
      }
      if (this.isKing())
        this.kingdom.removeKing();
      this.clearManagers();
      if (this.hasEquipment())
        this.equipment.destroyAllEquipment();
      this.clearAttackTarget();
      this.clearTileTarget();
    }
  }

  public void checkDieOnGroundBoat()
  {
    if (!this.asset.is_boat || this.current_tile.Type.liquid || !this.isAlive() || this.isInMagnet())
      return;
    this.getHitFullHealth(AttackType.Gravity);
    this.skipBehaviour();
    if (!this.hasStatus("magnetized"))
      return;
    ++World.world.game_stats.data.boatsDestroyedByMagnet;
    AchievementLibrary.boats_disposal.checkBySignal();
  }

  public void copyAggroFrom(Actor pTarget)
  {
    if (!pTarget.hasStatus("angry"))
      return;
    foreach (long aggressionTarget in pTarget._aggression_targets)
      this.addAggro(aggressionTarget);
    if (!pTarget.attackedBy.isRekt() && pTarget.attackedBy.isActor())
      this.addAggro(pTarget.attackedBy.a);
    if (pTarget.attack_target.isRekt() || !pTarget.attack_target.isActor())
      return;
    this.addAggro(pTarget.attack_target.a);
  }

  public bool isInAggroList(Actor pActor) => this._aggression_targets.Contains(pActor.getID());

  public bool shouldContinueToAttackTarget()
  {
    BaseSimObject attackTarget = this.attack_target;
    return this.areFoes(attackTarget) || attackTarget.isActor() && (attackTarget.a.hasStatusTantrum() || this.isInAggroList(attackTarget.a));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void clearAttackTarget()
  {
    if (!this.has_attack_target)
      return;
    this.attack_target = (BaseSimObject) null;
    this.has_attack_target = false;
    this.batch.c_check_attack_target.Remove(this.a);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isEnemyTargetAlive()
  {
    if (this.has_attack_target)
    {
      if (this.attack_target.isRekt())
      {
        this.clearAttackTarget();
        return false;
      }
      if (this.attack_target.isBuilding() && !this.attack_target.b.isUsable())
      {
        this.clearAttackTarget();
        return false;
      }
    }
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void setAttackTarget(BaseSimObject pAttackTarget)
  {
    this.attack_target = pAttackTarget;
    if (this.has_attack_target)
      return;
    this.has_attack_target = true;
    this.batch.c_check_attack_target.Add(this.a);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasRangeAttack() => this._attack_asset.attack_type == WeaponType.Range;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasMeleeAttack() => this._attack_asset.attack_type == WeaponType.Melee;

  private void checkAttackTypes() => this._attack_asset = this.getWeaponAsset();

  private bool isEquipmentMeleeAttack()
  {
    EquipmentAsset weaponAsset = this.getWeaponAsset();
    return this.asset.only_melee_attack || weaponAsset.attack_type == WeaponType.Melee;
  }

  public float getAttackCooldown() => 1f / this.stats["attack_speed"];

  private void takeAwayItems()
  {
    if (!this.hasEquipment())
      return;
    foreach (ActorEquipmentSlot actorEquipmentSlot in this.equipment)
    {
      if (!actorEquipmentSlot.isEmpty())
        actorEquipmentSlot.takeAwayItem();
    }
  }

  public bool isInDangerZone()
  {
    return this.hasCity() && this.city.danger_zones.Contains(this.current_zone);
  }

  public void setPossessionAttackHappened()
  {
    this._possession_attack_happened_frame = World.world.getCurWorldTime();
  }

  public bool isPossessionAttackJustHappened()
  {
    return World.world.getCurWorldTime() - this._possession_attack_happened_frame <= 0.5;
  }

  public ActorBag inventory
  {
    get => this.data.inventory;
    set => this.data.inventory = value;
  }

  public void addLoot(int pLootValue)
  {
    if (pLootValue == 0)
      return;
    this.data.loot += pLootValue;
    this.data.loot = Mathf.Clamp(this.data.loot, 0, 99999);
    EffectsLibrary.showMoneyEffect("fx_money_got_loot", this.current_position, this.current_zone, this.actor_scale);
  }

  public void addMoney(int pValue)
  {
    if (pValue == 0)
      return;
    this.data.money += pValue;
    this.data.money = Mathf.Clamp(this.data.money, 0, 99999);
    EffectsLibrary.showMoneyEffect("fx_money_got_money", this.current_position, this.current_zone, this.actor_scale);
  }

  public int giveAllLoot()
  {
    int loot = this.data.loot;
    this.lootEmpty();
    return loot;
  }

  public int giveAllMoney()
  {
    int money = this.data.money;
    this.data.money = 0;
    return money;
  }

  public void spendMoney(int pCost)
  {
    if (pCost == 0)
      return;
    this.data.money -= pCost;
    EffectsLibrary.showMoneyEffect("fx_money_spend_money", this.current_position, this.current_zone, this.actor_scale);
  }

  public int getMoneyForGift()
  {
    if (this.money < 10)
      return 0;
    int moneyForGift = Mathf.RoundToInt((float) this.money * Randy.randomFloat(0.05f, 0.1f));
    if (moneyForGift == 0)
      return 0;
    EffectsLibrary.showMoneyEffect("fx_money_spend_money", this.current_position, this.current_zone, this.actor_scale);
    return moneyForGift;
  }

  public void takeAllOwnLoot() => this.addMoney(this.giveAllLoot());

  public int giveAllLootAndMoney() => this.giveAllLoot() + this.giveAllMoney();

  public void paidTax(float pTaxRate, string pEffect)
  {
    this.lootEmpty();
    EffectsLibrary.showMoneyEffect(pEffect, this.current_position, this.current_zone, this.actor_scale);
    int pValue = -5;
    if ((double) pTaxRate > 0.7)
      pValue = -10;
    this.changeHappiness("paid_tax", pValue);
  }

  public void lootEmpty() => this.data.loot = 0;

  public void giveInventoryResourcesToCity()
  {
    if (this.isCarryingResources() && this.hasCity() && this.city.isAlive())
    {
      foreach (ResourceContainer resourceContainer in this.inventory.getResources().Values)
        this.city.addResourcesToRandomStockpile(resourceContainer.id, resourceContainer.amount);
    }
    this.inventory.empty();
    this.setItemSpriteRenderDirty();
  }

  public void generateDefaultSpawnWeapons(bool pUseOwnerless)
  {
    if (pUseOwnerless && this.canUseItems())
    {
      foreach (Item pItem in (CoreSystemManager<Item, ItemData>) World.world.items)
      {
        if (!pItem.isDestroyable() && !pItem.hasCity() && !pItem.hasActor())
        {
          this.equipment.setItem(pItem, this);
          return;
        }
      }
    }
    string[] defaultWeapons = this.asset.default_weapons;
    if ((defaultWeapons != null ? (defaultWeapons.Length != 0 ? 1 : 0) : 0) == 0)
      return;
    this.createNewWeapon(this.asset.default_weapons.GetRandom<string>());
  }

  public bool createNewWeapon(string pItemId)
  {
    this.equipment.weapon.setItem(World.world.items.generateItem(AssetManager.items.get(pItemId), pActor: this.a, pFakeCreationYear: 10), this.a);
    return true;
  }

  internal void reloadInventory() => this.setStatsDirty();

  public void stealActionFrom(
    Actor pTarget,
    float pTargetStunnedTimer = 5f,
    float pWaitTimerForThief = 1f,
    bool pAddAggro = true,
    bool pPossessedSteal = false)
  {
    bool flag = false;
    int pLootValue = pTarget.giveAllLootAndMoney();
    if (pLootValue > 0)
      flag = true;
    this.addLoot(pLootValue);
    pTarget.cancelAllBeh();
    pTarget.makeStunned(pTargetStunnedTimer);
    this.makeWait(pWaitTimerForThief);
    this.addStatusEffect("being_suspicious");
    if (pAddAggro)
      pTarget.addAggro(this);
    this.punchTargetAnimation(Vector2.op_Implicit(this.current_position), false, pAngle: -40f);
    if (((this.hasSubspeciesMetaTag("steal_items") ? 1 : (this.hasTag("steal_items") ? 1 : 0)) | (pPossessedSteal ? 1 : 0)) != 0 && this.tryToStealItems(pTarget, pPossessedSteal))
      flag = true;
    if (!flag)
      return;
    pTarget.changeHappiness("got_robbed");
  }

  public bool tryToStealItems(Actor pActorTarget, bool pPossessedSteal = false)
  {
    if (!this.understandsHowToUseItems() || !this.hasMeleeAttack())
      return false;
    float pChance = 0.5f;
    if (pPossessedSteal)
      pChance = 1f;
    if (!this.takeItems(pActorTarget, pChance, 1))
      return false;
    pActorTarget.makeStunned(1f);
    this.checkAttackTypes();
    pActorTarget.checkAttackTypes();
    return true;
  }

  public bool tryToAcceptGift(Actor pActorTarget)
  {
    if (!this.understandsHowToUseItems() || !this.takeItems(pActorTarget, 0.5f, 1))
      return false;
    this.checkAttackTypes();
    pActorTarget.checkAttackTypes();
    return true;
  }

  public void takeAllResources(Actor pActorTarget)
  {
    if (!pActorTarget.isCarryingResources())
      return;
    foreach (KeyValuePair<string, ResourceContainer> resource in pActorTarget.inventory.getResources())
      this.inventory.add(resource.Value);
    pActorTarget.inventory.empty();
  }

  public bool takeItems(Actor pActorTarget, float pChance = 1f, int pMaxItems = 0)
  {
    if (!this.understandsHowToUseItems() || !pActorTarget.hasEquipment())
      return false;
    using (ListPool<ActorEquipmentSlot> list = new ListPool<ActorEquipmentSlot>((IEnumerable<ActorEquipmentSlot>) pActorTarget.equipment))
    {
      bool items = false;
      if (pMaxItems == 0)
        pMaxItems = list.Count;
      foreach (ActorEquipmentSlot actorEquipmentSlot in list.LoopRandom<ActorEquipmentSlot>(pMaxItems))
      {
        if (!actorEquipmentSlot.isEmpty())
        {
          ActorEquipmentSlot slot = this.equipment.getSlot(actorEquipmentSlot.type);
          Item obj = slot.getItem();
          Item pItem = actorEquipmentSlot.getItem();
          if (!pItem.isCursed() && (slot.isEmpty() || !obj.isCursed() && pItem.getValue() > obj.getValue()))
          {
            items = true;
            actorEquipmentSlot.takeAwayItem();
            slot.setItem(pItem, this);
            this.setStatsDirty();
            pActorTarget.setStatsDirty();
          }
        }
      }
      return items;
    }
  }

  public void addToInventory(string pResourceID, int pAmount)
  {
    this.inventory = this.inventory.add(pResourceID, pAmount);
    this.setItemSpriteRenderDirty();
  }

  public void addToInventory(ResourceContainer pResourceContainer)
  {
    this.inventory = this.inventory.add(pResourceContainer);
    this.setItemSpriteRenderDirty();
  }

  public void takeFromInventory(string pID, int pAmount)
  {
    this.inventory = this.inventory.remove(pID, pAmount);
    this.setItemSpriteRenderDirty();
  }

  public void setSubspecies(Subspecies pObject)
  {
    World.world.subspecies.setDirtyUnits(this.subspecies);
    this.subspecies = pObject;
    World.world.subspecies.unitAdded(pObject);
    this.setStatsDirty();
  }

  public void joinLanguage(Language pLanguage)
  {
    if (this.language != pLanguage)
    {
      bool flag = false;
      if (this.hasLanguage())
      {
        this.language.increaseSpeakersLost();
        flag = true;
      }
      if (pLanguage != null)
      {
        if (!flag)
          pLanguage.countNewSpeaker();
        else
          pLanguage.countConversion();
      }
    }
    this.setLanguage(pLanguage);
  }

  public void setLanguage(Language pObject)
  {
    World.world.languages.setDirtyUnits(this.language);
    this.language = pObject;
    World.world.languages.unitAdded(pObject);
    this.setStatsDirty();
  }

  public void setPlot(Plot pObject)
  {
    World.world.plots.setDirtyUnits(this.plot);
    this.plot = pObject;
    World.world.plots.unitAdded(pObject);
    this.setStatsDirty();
  }

  public void setReligion(Religion pObject)
  {
    World.world.religions.setDirtyUnits(this.religion);
    this.religion = pObject;
    World.world.religions.unitAdded(pObject);
    this.setStatsDirty();
  }

  public void setFamily(Family pObject)
  {
    World.world.families.setDirtyUnits(this.family);
    this.family = pObject;
    World.world.families.unitAdded(pObject);
    this.setStatsDirty();
  }

  public void setClan(Clan pObject)
  {
    World.world.clans.setDirtyUnits(this.clan);
    this.clan = pObject;
    World.world.clans.unitAdded(pObject);
    this.setStatsDirty();
  }

  public void setCulture(Culture pCulture)
  {
    World.world.cultures.setDirtyUnits(this.culture);
    this.culture = pCulture;
    World.world.cultures.unitAdded(pCulture);
    this.setStatsDirty();
  }

  internal void removeFromArmy()
  {
    if (!this.hasArmy())
      return;
    Army army = this.army;
    this.setArmy((Army) null);
    army.checkCaptainRemoval(this);
  }

  public void setArmy(Army pObject)
  {
    World.world.armies.setDirtyUnits(this.army);
    this.army = pObject;
    World.world.armies.unitAdded(pObject);
    this.setStatsDirty();
  }

  internal void setCity(City pCity)
  {
    if (this.city == pCity)
      return;
    if (this.city != null)
      this.city.eventUnitRemoved(this.a);
    World.world.cities.setDirtyUnits(this.city);
    this.city = pCity;
    if (this.city != null)
    {
      this.city.eventUnitAdded(this.a);
      this.setKingdom(this.city.kingdom);
    }
    World.world.cities.unitAdded(this.city);
    this.setStatsDirty();
  }

  public void setMetasFromCity(City pCity)
  {
    if (pCity.hasCulture() && !this.hasCulture())
      this.setCulture(pCity.culture);
    if (pCity.hasLanguage() && !this.hasLanguage())
      this.joinLanguage(pCity.language);
    if (!pCity.hasReligion() || this.hasReligion())
      return;
    this.setReligion(pCity.religion);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasClan() => this.clan != null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasSubspecies() => this.subspecies != null;

  public bool hasArmy() => this.army != null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasFamily() => this.family != null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasLanguage() => this.language != null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasPlot() => this.plot != null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasReligion() => this.religion != null;

  public bool tryToConvertToReligion(Religion pReligion)
  {
    if (!this.hasSubspecies() || !this.subspecies.has_advanced_memory || this.hasReligion() && this.religion == pReligion || this.hasCulture() && !this.culture.isPossibleToConvertToOtherMeta())
      return false;
    this.setReligion(pReligion);
    pReligion.countConversion();
    EffectsLibrary.showMetaEventEffectConversion("fx_conversion_religion", this);
    return true;
  }

  public bool tryToConvertToCulture(Culture pCulture)
  {
    if (!this.hasSubspecies() || !this.subspecies.has_advanced_memory || this.hasCulture() && this.culture == pCulture || this.hasCulture() && !this.culture.isPossibleToConvertToOtherMeta())
      return false;
    int num = this.hasCulture() ? 1 : 0;
    Culture culture = this.culture;
    this.setCulture(pCulture);
    if (num != 0)
      pCulture.countConversion();
    EffectsLibrary.showMetaEventEffectConversion("fx_conversion_culture", this);
    return true;
  }

  public bool tryToConvertToLanguage(Language pLanguage)
  {
    if (!this.hasSubspecies() || !this.subspecies.has_advanced_communication || this.hasLanguage() && this.language == pLanguage || this.hasCulture() && !this.culture.isPossibleToConvertToOtherMeta())
      return false;
    this.joinLanguage(pLanguage);
    EffectsLibrary.showMetaEventEffectConversion("fx_conversion_language", this);
    return true;
  }

  public void saveOriginFamily(long pID) => this.data.ancestor_family = pID;

  private void clearManagers()
  {
    if (this.hasClan())
    {
      World.world.clans.unitDied(this.clan);
      this.clan = (Clan) null;
    }
    if (this.hasArmy())
    {
      World.world.armies.unitDied(this.army);
      this.army = (Army) null;
    }
    if (this.hasCulture())
    {
      World.world.cultures.unitDied(this.culture);
      this.culture = (Culture) null;
    }
    if (this.hasFamily())
    {
      World.world.families.unitDied(this.family);
      this.family = (Family) null;
    }
    if (this.hasLanguage())
    {
      World.world.languages.unitDied(this.language);
      this.language = (Language) null;
    }
    if (this.hasPlot())
    {
      World.world.plots.unitDied(this.plot);
      this.plot = (Plot) null;
    }
    if (!this.hasReligion())
      return;
    World.world.religions.unitDied(this.religion);
    this.religion = (Religion) null;
  }

  internal bool isCitizenJob(string pJob)
  {
    return this.citizen_job != null && this.citizen_job.id == pJob;
  }

  public void forgetCulture()
  {
    this.makeConfused();
    if (!this.hasCulture())
      return;
    this.setCulture((Culture) null);
  }

  public void forgetReligion()
  {
    this.makeConfused();
    if (!this.hasReligion())
      return;
    this.setReligion((Religion) null);
  }

  public void forgetLanguage()
  {
    this.makeConfused(10f);
    if (!this.hasLanguage())
      return;
    this.joinLanguage((Language) null);
  }

  public void forgetClan()
  {
    this.makeConfused();
    if (!this.hasClan())
      return;
    this.clan.tryForgetChief(this);
    this.setClan((Clan) null);
  }

  public void forgetKingdomAndCity()
  {
    this.makeConfused();
    this.removeFromPreviousFaction();
    if (!this.isKingdomCiv())
      return;
    this.setDefaultKingdom();
  }

  public void tryToConvertActorToMetaFromActor(Actor pActor, bool pStunOnSuccess = true)
  {
    int num = 0;
    if (pActor.hasCulture() && Randy.randomBool() && this.tryToConvertToCulture(pActor.culture))
      ++num;
    if (pActor.hasLanguage() && Randy.randomBool() && this.tryToConvertToLanguage(pActor.language))
      ++num;
    if (pActor.hasReligion() && Randy.randomBool() && this.tryToConvertToReligion(pActor.religion))
      ++num;
    if (!pStunOnSuccess)
      return;
    if (num > 0)
    {
      this.makeStunned();
      this.applyRandomForce();
      this.addStatusEffect("voices_in_my_head");
    }
    else
    {
      if (!Randy.randomChance(0.1f))
        return;
      this.makeConfused(Randy.randomFloat(0.8f, 5f));
    }
  }

  public void joinCity(City pCity)
  {
    bool flag1 = !this.asset.is_boat;
    if (this.city != pCity)
    {
      bool flag2 = this.hasCity();
      if (flag2 && flag1)
        this.city.increaseLeft();
      if (pCity != null)
      {
        if (pCity.kingdom != this.kingdom)
          this.joinKingdom(pCity.kingdom);
        if (flag1)
        {
          if (flag2)
            pCity.increaseMoved();
          else
            pCity.increaseJoined();
        }
      }
    }
    this.setCity(pCity);
  }

  public void joinKingdom(Kingdom pKingdom)
  {
    if (!this.asset.is_boat && this.kingdom != pKingdom)
    {
      bool flag = this.hasKingdom();
      if (flag && this.kingdom.isCiv())
        this.kingdom.increaseLeft();
      if (pKingdom != null && pKingdom.isCiv())
      {
        if (flag)
          pKingdom.increaseMoved();
        else
          pKingdom.increaseJoined();
      }
    }
    this.setKingdom(pKingdom);
  }

  internal void setKingdom(Kingdom pKingdomToSet)
  {
    if (this.kingdom == pKingdomToSet)
      return;
    this.checkKingdom();
    this.kingdom = pKingdomToSet;
    this.checkKingdom();
    this.setStatsDirty();
  }

  private void checkKingdom()
  {
    if (!this.hasKingdom())
      return;
    if (this.kingdom.wild)
      World.world.kingdoms_wild.setDirtyUnits(this.kingdom);
    else
      World.world.kingdoms.setDirtyUnits(this.kingdom);
  }

  public void setForcedKingdom(Kingdom pForcedKingdom)
  {
    if (this.kingdom.asset.id == pForcedKingdom.asset.id)
      return;
    this.joinKingdom(pForcedKingdom);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasCulture() => this.culture != null;

  public bool buildCityAndStartCivilization()
  {
    if (!World.world.cities.canStartNewCityCivilizationHere(this))
      return false;
    Kingdom kingdom = World.world.kingdoms.makeNewCivKingdom(this);
    City city = World.world.cities.buildFirstCivilizationCity(this);
    this.createDefaultCultureAndLanguageAndClan(city.name);
    kingdom.setUnitMetas(this);
    city.setUnitMetas(this);
    return true;
  }

  public void createDefaultCultureAndLanguageAndClan(string pCultureName = null)
  {
    if (!this.hasClan())
      World.world.clans.newClan(this, true);
    if (!this.hasLanguage() && this.subspecies.has_advanced_communication)
    {
      Language pLanguage = World.world.languages.newLanguage(this, true);
      this.joinLanguage(pLanguage);
      pLanguage.convertSameSpeciesAroundUnit(this, false);
    }
    if (this.hasCulture() || !this.subspecies.has_advanced_memory)
      return;
    Culture pCulture = World.world.cultures.newCulture(this, true);
    if (pCultureName != null)
      pCulture.setName(pCultureName, false);
    this.setCulture(pCulture);
    pCulture.convertSameSpeciesAroundUnit(this, false);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void checkDefaultKingdom()
  {
    if (this.hasKingdom())
      return;
    this.setDefaultKingdom();
  }

  public void setDefaultKingdom()
  {
    this.setKingdom(World.world.kingdoms_wild.get(this.asset.kingdom_id_wild));
  }

  public void removeFromPreviousFaction()
  {
    this.stopBeingWarrior();
    if (this.isKing())
      this.kingdom.kingLeftEvent();
    this.joinCity((City) null);
  }

  public bool wantsToSplitMeta()
  {
    return (!this.hasKingdom() || !this.isKingdomCiv() || !this.hasSubspecies() || this.kingdom.getMainSubspecies() != this.subspecies) && (this.hasTrait("ambitious") || this.hasStatus("inspired"));
  }

  public NanoObject getMetaObjectOfType(MetaType pType)
  {
    switch (pType)
    {
      case MetaType.Subspecies:
        return (NanoObject) this.subspecies;
      case MetaType.Family:
        return (NanoObject) this.family;
      case MetaType.Language:
        return (NanoObject) this.language;
      case MetaType.Culture:
        return (NanoObject) this.culture;
      case MetaType.Religion:
        return (NanoObject) this.religion;
      case MetaType.Clan:
        return (NanoObject) this.clan;
      case MetaType.City:
        return (NanoObject) this.city;
      case MetaType.Kingdom:
        return (NanoObject) this.kingdom;
      case MetaType.Alliance:
        return (NanoObject) this.kingdom.getAlliance();
      case MetaType.Army:
        return (NanoObject) this.army;
      default:
        return (NanoObject) null;
    }
  }

  internal void setFlip(bool pFlip) => this.flip = pFlip;

  public void precalcMovementSpeed(bool pForce = false)
  {
    if (!pForce)
    {
      if (!this.is_moving)
        return;
      if (this._precalc_movement_speed_skips > 0)
      {
        --this._precalc_movement_speed_skips;
        return;
      }
      this._precalc_movement_speed_skips = 5;
    }
    bool flag1 = this.isInAir();
    bool flag2 = this.isWaterCreature();
    float num1 = 1f;
    if (this.asset.ignore_tile_speed_multiplier | flag1 | flag2)
      num1 = 1f;
    else if (this.current_tile.is_liquid)
    {
      if (this.getStamina() <= 0 && !this.hasTag("fast_swimming"))
        num1 *= 0.4f;
    }
    else if (!string.IsNullOrEmpty(this.current_tile.Type.ignore_walk_multiplier_if_tag) && !this.stats.hasTag(this.current_tile.Type.ignore_walk_multiplier_if_tag))
      num1 = this.current_tile.Type.walk_multiplier;
    if (!this.asset.ignore_tile_speed_multiplier && this._is_in_liquid && this.hasTag("fast_swimming"))
      num1 *= 5f;
    if (this.hasTask() && (double) this.ai.task.speed_multiplier != 1.0)
      num1 *= this.ai.task.speed_multiplier;
    float num2 = this.stats["speed"] * num1;
    if (!flag1 && WorldLawLibrary.world_law_entanglewood.isEnabled())
    {
      Building building = this.current_tile.building;
      if ((building != null ? (building.asset.flora_type == FloraType.Tree ? 1 : 0) : 0) != 0)
        num2 *= 0.8f;
    }
    if ((double) num2 < 1.0)
      num2 = 1f;
    if (DebugConfig.isOn(DebugOption.UnitsAlwaysFast))
      num2 *= 20f;
    this._current_combined_movement_speed = num2 * 0.4f * SimGlobals.m.unit_speed_multiplier;
    if (this.tile_target == null)
      return;
    float num3 = Toolbox.DistVec2Float(this.current_position, Vector2.op_Implicit(this.tile_target.posV3));
    if ((double) num3 >= 1.0 || (double) this._current_combined_movement_speed <= 3.0)
      return;
    this._current_combined_movement_speed *= Mathf.Lerp(1f, 0.3f, 1f - num3);
  }

  internal bool checkFlip() => this.asset.check_flip((BaseSimObject) this);

  protected void updateMovement(float pElapsed, float pWalkedDistance = 0.0f)
  {
    float num1 = Toolbox.DistVec2Float(this.current_position, this.next_step_position);
    if (this.asset.can_flip && this.checkFlip())
    {
      if ((double) this.current_position.x < (double) this.next_step_position.x)
        this.setFlip(true);
      else
        this.setFlip(false);
    }
    float movementDelta = this.getMovementDelta(pElapsed, pWalkedDistance);
    if ((double) num1 < (double) movementDelta)
    {
      float num2 = num1;
      this.current_position = this.next_step_position;
      if (this.isUsingPath())
        this.updatePathMovement();
      else
        this.stopMovement();
      if (!this.is_moving)
        return;
      this.updateMovement(pElapsed, pWalkedDistance + num2);
    }
    else
      this.current_position = Vector2.MoveTowards(this.current_position, this.next_step_position, movementDelta);
  }

  private float getMovementDelta(float pElapsed, float pWalkedDistance = 0.0f)
  {
    float movementDelta = this._current_combined_movement_speed * pElapsed - pWalkedDistance;
    if ((double) movementDelta < 0.0)
      movementDelta = 0.0f;
    return movementDelta;
  }

  internal void updateMovementPossessedFlip()
  {
    if (InputHelpers.mouseSupported)
    {
      this.checkFlipAgainstTargetPosition(World.world.getMousePos());
    }
    else
    {
      if (!ControllableUnit.isMovementActionActive() || this.isPossessionAttackJustHappened())
        return;
      this.checkFlipAgainstTargetPosition(Vector2.op_Addition(ControllableUnit.getMovementVector(), this.current_position));
    }
  }

  public void checkFlipAgainstTargetPosition(Vector2 pPosition)
  {
    if (!this.asset.can_flip)
      return;
    if ((double) this.current_position.x < (double) pPosition.x)
      this.setFlip(true);
    else
      this.setFlip(false);
  }

  internal float updatePossessedMovementTowards(float pElapsed, Vector2 pMovementPoint)
  {
    this.precalcMovementSpeed(true);
    if (this.asset.can_flip && this.checkFlip())
    {
      float factorForSideMovement = this.getMismatchFactorForSideMovement(pMovementPoint);
      if ((double) factorForSideMovement > 0.20000000298023224)
        pElapsed *= Mathf.Lerp(1f, 0.8f, factorForSideMovement);
    }
    float movementDelta = this.getMovementDelta(pElapsed);
    Vector2 pPoint = this.checkVelocityAgainstBlock(Vector2.MoveTowards(this.current_position, pMovementPoint, movementDelta));
    if (!Toolbox.inMapBorder(ref pPoint))
      return 0.0f;
    this.current_position = pPoint;
    return movementDelta;
  }

  public Vector2 getPossessionControlTargetPosition() => ControllableUnit.getClickVector();

  public Vector2 getPossessionControlTargetPositionMovementVector()
  {
    return InputHelpers.mouseSupported ? ControllableUnit.getClickVector() : Vector2.op_Addition(ControllableUnit.getMovementVector(), this.current_position);
  }

  private float getMismatchFactorForSideMovement(Vector2 pMovementPoint)
  {
    Vector2 mousePos = World.world.getMousePos();
    bool flag1 = (double) this.current_position.x < (double) mousePos.x;
    bool flag2 = (double) this.current_position.x < (double) pMovementPoint.x;
    int num1 = (double) this.current_position.y < (double) mousePos.y ? 1 : 0;
    bool flag3 = (double) this.current_position.y < (double) pMovementPoint.y;
    float num2 = Mathf.Abs(pMovementPoint.x - this.current_position.x);
    float num3 = Mathf.Abs(pMovementPoint.y - this.current_position.y);
    float num4 = 0.0f;
    if (flag1 != flag2)
      num4 += num2;
    int num5 = flag3 ? 1 : 0;
    if (num1 != num5)
      num4 += num3;
    return Mathf.Clamp01(num4 / (float) ((double) num2 + (double) num3 + 1.0 / 1000.0));
  }

  internal void findCurrentTile(bool pCheckNeighbours = true)
  {
    Vector3 vector3 = Vector2.op_Implicit(this.current_position);
    if (!this.dirty_current_tile && (double) vector3.x == (double) this.lastX && (double) vector3.y == (double) this.lastY)
      return;
    this.dirty_current_tile = false;
    this.lastX = this.current_position.x;
    this.lastY = this.current_position.y;
    if (this.current_tile != null && (double) Toolbox.SquaredDist(this.current_tile.posV3.x, this.current_tile.posV3.y, vector3.x, vector3.y) < 0.16000001132488251)
      return;
    WorldTile tileAt = Toolbox.getTileAt(vector3.x, vector3.y);
    this.setCurrentTile(tileAt);
    if ((double) Toolbox.SquaredDist(tileAt.posV3.x, tileAt.posV3.y, vector3.x, vector3.y) < 0.090000003576278687 || !pCheckNeighbours || this.isFlying())
      return;
    bool flag = this.mustAvoidGround();
    if (tileAt.Type.lava && this.asset.die_in_lava)
    {
      foreach (WorldTile pTile in tileAt.neighboursAll)
      {
        if (pTile.Type.ground)
        {
          this.setCurrentTile(pTile);
          break;
        }
      }
    }
    if (tileAt.Type.ocean && this.isDamagedByOcean())
    {
      foreach (WorldTile pTile in tileAt.neighboursAll)
      {
        if (!pTile.is_liquid)
        {
          this.setCurrentTile(pTile);
          break;
        }
      }
    }
    if (tileAt.Type.block && !this.isFlying() && !flag)
    {
      foreach (WorldTile pTile in tileAt.neighboursAll)
      {
        if (pTile.Type.ground)
        {
          this.setCurrentTile(pTile);
          break;
        }
      }
    }
    if (!tileAt.is_liquid & flag)
    {
      foreach (WorldTile pTile in tileAt.neighboursAll)
      {
        if (pTile.is_liquid)
        {
          this.setCurrentTile(pTile);
          break;
        }
      }
    }
    if (!tileAt.isOnFire() || this.isImmuneToFire())
      return;
    foreach (WorldTile pTile in tileAt.neighboursAll)
    {
      if (!pTile.isOnFire())
      {
        this.setCurrentTile(pTile);
        break;
      }
    }
  }

  internal void checkFindCurrentTile()
  {
    if (!this.dirty_current_tile && (this._next_step_tile == null || (double) Toolbox.SquaredDistTile(this.current_tile, this._next_step_tile) <= 4.0))
      return;
    this.findCurrentTile();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal void setTileTarget(WorldTile pTile)
  {
    this.clearTileTarget();
    this.tile_target = pTile;
    this.tile_target.setTargetedBy(this.a);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal void clearTileTarget()
  {
    if (this.tile_target == null)
      return;
    if (this.tile_target.isTargetedBy(this))
      this.tile_target.cleanTargetedBy();
    this.tile_target = (WorldTile) null;
    this.scheduled_tile_target = (WorldTile) null;
  }

  internal void clearOldPath()
  {
    this.current_path.Clear();
    this.current_path_global = (List<MapRegion>) null;
    this.current_path_index = 0;
  }

  public virtual void updatePathMovement()
  {
    if (!this.isFollowingLocalPath())
    {
      this.setNotMoving();
      if (this.split_path != SplitPathStatus.Split)
      {
        this.split_path = SplitPathStatus.Split;
        this.timer_action = Randy.randomFloat(0.0f, this.asset.path_movement_timeout);
      }
      else
      {
        this.split_path = SplitPathStatus.Normal;
        if (this.tile_target == null)
          return;
        int num = (int) this.goTo(this.tile_target);
      }
    }
    else
    {
      WorldTile pTileTarget = this.current_path[this.current_path_index];
      TileTypeBase type = pTileTarget.Type;
      ++this.current_path_index;
      if (type.damaged_when_walked)
        this.current_tile.tryToBreak();
      bool flag = true;
      if (this._has_status_strange_urge)
        flag = false;
      if (flag)
      {
        if (this.asset.is_boat && !pTileTarget.isGoodForBoat())
        {
          BaseActionActor cancelPathMovement = this.callbacks_cancel_path_movement;
          if (cancelPathMovement != null)
            cancelPathMovement(this);
          this.cancelAllBeh();
          return;
        }
        if (type.block && !this.ignoresBlocks())
        {
          if (!this.hasTask() || !this.ai.task.move_from_block)
          {
            this.cancelAllBeh();
            return;
          }
        }
        else
        {
          if (this.asset.die_in_lava && type.lava)
          {
            this.cancelAllBeh();
            return;
          }
          if (this.isDamagedByOcean() && type.ocean && !this._is_in_liquid)
          {
            this.cancelAllBeh();
            return;
          }
        }
      }
      if (pTileTarget.isOnFire() && !this.isImmuneToFire() && !this.hasStatus("burning") && !this.current_tile.isOnFire())
      {
        if (this.hasTask() && this.ai.task.is_fireman)
        {
          this.stopMovement();
        }
        else
        {
          this.cancelAllBeh();
          this.makeWait(0.3f);
        }
      }
      else
        this.moveTo(pTileTarget);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isFollowingLocalPath()
  {
    return this.current_path.Count > 0 && this.current_path_index < this.current_path.Count;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isUsingPath() => this.isFollowingLocalPath() || this.current_path_global != null;

  public ExecuteEvent goTo(
    WorldTile pTile,
    bool pPathOnWater = false,
    bool pWalkOnBlocks = false,
    bool pWalkOnLava = false,
    int pLimitPathfindingRegions = 0)
  {
    this.setTileTarget(pTile);
    return ActorMove.goTo(this, pTile, pPathOnWater, pWalkOnBlocks, pWalkOnLava, pLimitPathfindingRegions);
  }

  public void clearPathForCalibration()
  {
    this.clearOldPath();
    this.next_step_position = this.current_position;
  }

  private void finishStrangeUrgeMovement()
  {
    this._has_status_strange_urge = false;
    this.finishStatusEffect("strange_urge");
    this.setTask("strange_urge_finish");
  }

  public void stopMovement()
  {
    this.split_path = SplitPathStatus.Normal;
    this._next_step_tile = (WorldTile) null;
    this.clearOldPath();
    this.clearTileTarget();
    this.setNotMoving();
    this.next_step_position = Vector2.op_Implicit(Globals.emptyVector);
    this.dirty_current_tile = true;
    if (!this._has_status_strange_urge)
      return;
    this.finishStrangeUrgeMovement();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void setIsMoving()
  {
    if (this.is_moving)
      return;
    this._is_moving = true;
    this.batch.c_update_movement.Add(this.a);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void setNotMoving()
  {
    if (!this.is_moving)
      return;
    this._is_moving = false;
    this.batch.c_update_movement.Remove(this.a);
  }

  public void setPossessedMovement(bool pValue) => this._possessed_movement = pValue;

  public void moveTo(WorldTile pTileTarget)
  {
    this.setIsMoving();
    if (!this.has_attack_target && this.current_tile != null && pTileTarget.isOnFire() && !this.current_tile.isOnFire() && !this.isImmuneToFire())
    {
      this.cancelAllBeh();
    }
    else
    {
      this._next_step_tile = pTileTarget;
      if ((double) Toolbox.SquaredDistTile(this.current_tile, pTileTarget) > 4.0)
        this.dirty_current_tile = true;
      else
        this.setCurrentTile(this._next_step_tile);
      this.checkStepActionForTile(this.current_tile);
      Vector3 vector3;
      // ISSUE: explicit constructor call
      ((Vector3) ref vector3).\u002Ector(pTileTarget.posV3.x, pTileTarget.posV3.y);
      this.next_step_position = Vector2.op_Implicit(vector3);
    }
  }

  public Vector3 updatePos()
  {
    Vector3 vector3 = Vector2.op_Implicit(this.current_position);
    Vector2 shakeOffset = this.shake_offset;
    Vector2 moveJumpOffset = this.move_jump_offset;
    float num1 = vector3.x + moveJumpOffset.x + shakeOffset.x;
    float num2 = vector3.y + moveJumpOffset.y + shakeOffset.y + this.position_height;
    ((Vector2) ref this.current_shadow_position).Set(vector3.x + shakeOffset.x, vector3.y + shakeOffset.y);
    float positionHeight = this.position_height;
    ((Vector3) ref this.cur_transform_position).Set(num1, num2, positionHeight);
    return this.cur_transform_position;
  }

  public void stayInBuilding(Building pBuilding)
  {
    this.is_inside_building = true;
    this.inside_building = pBuilding;
  }

  internal void exitBuilding()
  {
    if (!this.is_inside_building)
      return;
    this.timer_action = 0.0f;
    this.is_inside_building = false;
    this.inside_building = (Building) null;
  }

  internal void embarkInto(Boat pBoat)
  {
    this.stopMovement();
    this.data.transportID = pBoat.actor.data.id;
    this.is_inside_boat = true;
    this.inside_boat = pBoat;
    this.inside_boat.addPassenger(this);
    this.setTask("sit_inside_boat");
    this.ai.update();
  }

  internal void disembarkTo(Boat pBoat, WorldTile pTile)
  {
    this.spawnOn(pTile);
    this.data.transportID = -1L;
    this.exitBoat();
    this.setTask("short_move");
  }

  internal void exitBoat()
  {
    this.inside_boat = (Boat) null;
    this.is_inside_boat = false;
    this.dirty_current_tile = true;
  }

  internal void changeMoveJumpOffset(float pValue)
  {
    this.move_jump_offset.y += pValue;
    if ((double) this.move_jump_offset.y >= 0.0)
      return;
    this.move_jump_offset.y = 0.0f;
  }

  internal void setCurrentTile(WorldTile pTile) => this.current_tile = pTile;

  internal void setCurrentTilePosition(WorldTile pTile)
  {
    this.setCurrentTile(pTile);
    ((Vector2) ref this.current_position).Set(pTile.posV3.x, pTile.posV3.y);
  }

  protected void updateWalkJump(float pElapsed)
  {
    if (!this.is_visible && (double) this.move_jump_offset.y == 0.0 || (double) this.position_height > 0.0 || this.asset.disable_jump_animation)
      return;
    if (!this.is_moving)
    {
      if ((double) this.move_jump_offset.y == 0.0 && ((double) this._jump_time == 0.0 || this.isAffectedByLiquid()))
        return;
    }
    else if (!this.is_moving && (double) this._jump_time == 0.0 || this.isAffectedByLiquid())
      return;
    this._jump_time += World.world.elapsed * 6f;
    if ((double) this._jump_time >= 1.0)
      this.changeMoveJumpOffset(-3f * pElapsed);
    else
      this.changeMoveJumpOffset(3f * pElapsed);
    if ((double) this._jump_time >= 2.0)
    {
      this._jump_time = 0.0f;
      this.changeMoveJumpOffset(0.0f);
    }
    if (!this.asset.rotating_animation)
      return;
    this.target_angle.z += (float) (-(double) this.move_jump_offset.y * 200.0) * World.world.elapsed;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool inMapBorder() => Toolbox.inMapBorder(ref this.current_position);

  protected virtual void updateVelocity()
  {
    if (!this.under_forces)
      return;
    this.dirty_current_tile = true;
    float fixedDeltaTime = World.world.fixed_delta_time;
    float num1 = fixedDeltaTime * this.velocity_speed;
    float num2 = Mathf.Min(this.stats["mass"] * SimGlobals.m.gravity, 20f);
    this.position_height += this.velocity.z * fixedDeltaTime * num2;
    this.velocity.z -= (float) ((double) fixedDeltaTime * (double) num2 * 0.30000001192092896);
    Vector3 vector3 = Vector2.op_Implicit(this.current_position);
    Vector2 pNewPos;
    // ISSUE: explicit constructor call
    ((Vector2) ref pNewPos).\u002Ector(vector3.x + this.velocity.x * num1, vector3.y + this.velocity.y * num1);
    Vector2 vector2 = this.checkVelocityAgainstBlock(pNewPos);
    ((Vector2) ref this.current_position).Set(vector2.x, vector2.y);
    if ((double) this.position_height < 0.0)
    {
      this.position_height = 0.0f;
      this.velocity.z = 0.0f;
    }
    if ((double) this.position_height > 0.0)
      return;
    this.stopForce();
  }

  private Vector2 checkVelocityAgainstBlock(Vector2 pNewPos)
  {
    WorldTile tileAt = Toolbox.getTileAt(pNewPos.x, pNewPos.y);
    if (this.current_tile.Type.block && (!this.current_tile.Type.mountains || tileAt.Type.mountains) || tileAt == this.current_tile)
      return pNewPos;
    if (this.asset.is_boat)
    {
      if (tileAt.Type.liquid)
        return pNewPos;
    }
    else if (!tileAt.Type.block || (double) this.getHeight() > (double) tileAt.Type.block_height)
      return pNewPos;
    Vector2 wallNormal = this.getWallNormal(pNewPos, Vector2.op_Implicit(tileAt.posV3));
    float num1 = 0.8f;
    float num2 = (float) ((double) this.velocity.x * (double) wallNormal.x + (double) this.velocity.y * (double) wallNormal.y);
    float num3 = this.velocity.x - 2f * num2 * wallNormal.x;
    float num4 = this.velocity.y - 2f * num2 * wallNormal.y;
    this.velocity.x = num3 * num1;
    this.velocity.y = num4 * num1;
    pNewPos.x = this.current_position.x;
    pNewPos.y = this.current_position.y;
    return pNewPos;
  }

  private Vector2 getWallNormal(Vector2 pPosition, Vector2 pBlockPosition)
  {
    Vector2 vector2 = Vector2.op_Subtraction(pPosition, pBlockPosition);
    Vector2 normalized = ((Vector2) ref vector2).normalized;
    return (double) Mathf.Abs(normalized.x) > (double) Mathf.Abs(normalized.y) ? new Vector2(Mathf.Sign(normalized.x), 0.0f) : new Vector2(0.0f, Mathf.Sign(normalized.y));
  }

  public void prepareForSave()
  {
    this.saveCoordinates();
    this.saveAssetID();
    this.saveProfession();
    this.saveHomeBuilding();
    this.saveEquipment();
    this.saveLover();
    this.saveCity();
    this.saveKingdomCiv();
    this.saveCulture();
    this.saveClan();
    this.saveSubspecies();
    this.saveFamily();
    this.saveArmy();
    this.saveLanguage();
    this.savePlot();
    this.saveReligion();
    this.saveTraits();
    this.finishSaving();
  }

  private void saveCoordinates()
  {
    ActorData data1 = this.data;
    Vector2Int pos1 = this.current_tile.pos;
    int x = ((Vector2Int) ref pos1).x;
    data1.x = x;
    ActorData data2 = this.data;
    Vector2Int pos2 = this.current_tile.pos;
    int y = ((Vector2Int) ref pos2).y;
    data2.y = y;
  }

  private void saveAssetID() => this.data.asset_id = this.asset.id;

  private void saveProfession() => this.data.profession = this._profession;

  private void saveHomeBuilding()
  {
    if (this._home_building != null && this._home_building.isUsable() && !this._home_building.isAbandoned())
      this.data.homeBuildingID = this._home_building.data.id;
    else
      this.data.homeBuildingID = -1L;
  }

  private void saveEquipment()
  {
    if (!this.hasEquipment())
      return;
    this.data.saved_items = this.equipment.getDataForSave();
  }

  private void saveLover()
  {
    if (this.hasLover())
      this.data.lover = this.lover.data.id;
    else
      this.data.lover = -1L;
  }

  private void saveCity()
  {
    if (this.hasCity() && this.city.isAlive())
      this.data.cityID = this.city.id;
    else
      this.data.cityID = -1L;
  }

  private void saveKingdomCiv()
  {
    if (this.isKingdomCiv())
      this.data.civ_kingdom_id = this.kingdom.id;
    else
      this.data.civ_kingdom_id = -1L;
  }

  private void saveCulture()
  {
    if (this.hasCulture())
      this.data.culture = this.culture.id;
    else
      this.data.culture = -1L;
  }

  private void saveClan()
  {
    if (this.hasClan())
      this.data.clan = this.clan.id;
    else
      this.data.clan = -1L;
  }

  private void saveSubspecies()
  {
    if (this.hasSubspecies())
      this.data.subspecies = this.subspecies.id;
    else
      this.data.subspecies = -1L;
  }

  private void saveFamily()
  {
    if (this.hasFamily())
      this.data.family = this.family.id;
    else
      this.data.family = -1L;
  }

  private void saveArmy()
  {
    if (this.hasArmy())
      this.data.army = this.army.id;
    else
      this.data.army = -1L;
  }

  private void saveLanguage()
  {
    if (this.hasLanguage())
      this.data.language = this.language.id;
    else
      this.data.language = -1L;
  }

  private void savePlot()
  {
    if (this.hasPlot())
      this.data.plot = this.plot.id;
    else
      this.data.plot = -1L;
  }

  private void saveReligion()
  {
    if (this.hasReligion())
      this.data.religion = this.religion.id;
    else
      this.data.religion = -1L;
  }

  private void saveTraits()
  {
    this.data.saved_traits = Toolbox.getListForSave<ActorTrait>(this.getTraits());
  }

  private void finishSaving() => this.data.save();

  public void loadFromSave()
  {
    this.setStatsDirty();
    TraitTools.loadTraits(this, this.data.saved_traits);
    foreach (ActorTrait trait in this.traits)
    {
      WorldActionTrait augmentationLoad = trait.action_on_augmentation_load;
      if (augmentationLoad != null)
      {
        int num = augmentationLoad((NanoObject) this, (BaseAugmentationAsset) trait) ? 1 : 0;
      }
    }
    if (this.isSapient() && this.is_profession_nothing)
      this.data.profession = UnitProfession.Unit;
    this.setProfession(this.data.profession, false);
    City pCity = World.world.cities.get(this.data.cityID);
    Kingdom pKingdomToSet = World.world.kingdoms.get(this.data.civ_kingdom_id);
    if (pCity != null && !pCity.isNeutral())
      this.setCity(pCity);
    if (pKingdomToSet != null)
      this.setKingdom(pKingdomToSet);
    if (this.hasEquipment())
    {
      foreach (ActorEquipmentSlot actorEquipmentSlot in this.equipment)
      {
        if (!actorEquipmentSlot.isEmpty())
        {
          Item obj = actorEquipmentSlot.getItem();
          int index = 0;
          while (index < obj.data.modifiers.Count)
          {
            if (AssetManager.items_modifiers.get(obj.data.modifiers[index]) == null)
              obj.data.modifiers.RemoveAt(index);
            else
              ++index;
          }
        }
      }
    }
    if (this.data.inventory.isEmpty())
      this.data.inventory.empty();
    foreach (Actor parent in this.getParents())
      parent.increaseChildren();
    BaseActionActor actionOnLoad = this.asset.action_on_load;
    if (actionOnLoad == null)
      return;
    actionOnLoad(this);
  }

  private void countDeath(AttackType pType)
  {
    ++World.world.game_stats.data.creaturesDied;
    ++World.world.map_stats.deaths;
    switch (pType)
    {
      case AttackType.Acid:
        ++World.world.map_stats.deaths_acid;
        goto case AttackType.Other;
      case AttackType.Fire:
        ++World.world.map_stats.deaths_fire;
        goto case AttackType.Other;
      case AttackType.Plague:
        ++World.world.map_stats.deaths_plague;
        goto case AttackType.Other;
      case AttackType.Infection:
        ++World.world.map_stats.deaths_infection;
        goto case AttackType.Other;
      case AttackType.Tumor:
        ++World.world.map_stats.deaths_tumor;
        goto case AttackType.Other;
      case AttackType.Other:
      case AttackType.AshFever:
      case AttackType.None:
        if (this.hasArmy())
          this.army.increaseDeaths(pType);
        if (this.hasCity())
          this.city.increaseDeaths(pType);
        if (this.hasClan())
          this.clan.increaseDeaths(pType);
        if (this.hasCulture())
          this.culture.increaseDeaths(pType);
        if (this.hasFamily())
          this.family.increaseDeaths(pType);
        if (this.hasLanguage())
          this.language.increaseDeaths(pType);
        if (this.hasReligion())
          this.religion.increaseDeaths(pType);
        if (this.hasSubspecies())
          this.subspecies.increaseDeaths(pType);
        if (this.isKingdomCiv())
          this.kingdom.increaseDeaths(pType);
        using (IEnumerator<Actor> enumerator = this.getParents().GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.decreaseChildren();
          break;
        }
      case AttackType.Divine:
        ++World.world.map_stats.deaths_divine;
        goto case AttackType.Other;
      case AttackType.Metamorphosis:
        ++World.world.map_stats.metamorphosis;
        goto case AttackType.Other;
      case AttackType.Starvation:
        ++World.world.map_stats.deaths_hunger;
        goto case AttackType.Other;
      case AttackType.Eaten:
        ++World.world.map_stats.deaths_eaten;
        goto case AttackType.Other;
      case AttackType.Age:
        ++World.world.map_stats.deaths_age;
        goto case AttackType.Other;
      case AttackType.Weapon:
        ++World.world.map_stats.deaths_weapon;
        goto case AttackType.Other;
      case AttackType.Poison:
        ++World.world.map_stats.deaths_poison;
        goto case AttackType.Other;
      case AttackType.Gravity:
        ++World.world.map_stats.deaths_gravity;
        goto case AttackType.Other;
      case AttackType.Drowning:
        ++World.world.map_stats.deaths_drowning;
        goto case AttackType.Other;
      case AttackType.Water:
        ++World.world.map_stats.deaths_water;
        goto case AttackType.Other;
      case AttackType.Explosion:
        ++World.world.map_stats.deaths_explosion;
        goto case AttackType.Other;
      case AttackType.Smile:
        ++World.world.map_stats.deaths_smile;
        goto case AttackType.Other;
      default:
        throw new ArgumentOutOfRangeException($"Unknown attack type: {pType}");
    }
  }

  public void increaseEvolutions()
  {
    ++World.world.map_stats.evolutions;
    if (this.hasCity())
      this.city.increaseEvolutions();
    if (this.hasClan())
      this.clan.increaseEvolutions();
    if (this.hasReligion())
      this.religion.increaseEvolutions();
    if (this.hasSubspecies())
      this.subspecies.increaseEvolutions();
    if (!this.isKingdomCiv())
      return;
    this.kingdom.increaseEvolutions();
  }

  private void increaseKills()
  {
    ++this.data.kills;
    if (this.hasArmy())
      this.army.increaseKills();
    if (this.hasCity())
      this.city.increaseKills();
    if (this.hasClan())
      this.clan.increaseKills();
    if (this.hasCulture())
      this.culture.increaseKills();
    if (this.hasFamily())
      this.family.increaseKills();
    if (this.hasLanguage())
      this.language.increaseKills();
    if (this.hasReligion())
      this.religion.increaseKills();
    if (this.hasSubspecies())
      this.subspecies.increaseKills();
    if (!this.isKingdomCiv())
      return;
    this.kingdom.increaseKills();
  }

  public void increaseChildren() => ++this._current_children;

  public void decreaseChildren() => --this._current_children;

  public void increaseBirths() => ++this.data.births;

  public void applyForcedKingdomTrait()
  {
    this.removeFromPreviousFaction();
    this.removeTrait("peaceful");
    this.startShake(pVol: 0.2f);
    this.startColorEffect();
    this.cancelAllBeh();
  }

  public string getTraitsAsLocalizedString()
  {
    string asLocalizedString = "";
    foreach (ActorTrait trait in this.traits)
      asLocalizedString = $"{asLocalizedString}{trait.getTranslatedName()}, ";
    return asLocalizedString;
  }

  public void sortTraits(IReadOnlyCollection<ActorTrait> pTraits)
  {
    if (!this.traits.SetEquals((IEnumerable<ActorTrait>) pTraits))
      return;
    this.traits.Clear();
    foreach (ActorTrait pTrait in (IEnumerable<ActorTrait>) pTraits)
      this.traits.Add(pTrait);
  }

  public void traitModifiedEvent()
  {
  }

  public void removeTrait(string pTraitID) => this.removeTrait(AssetManager.traits.get(pTraitID));

  public bool removeTrait(ActorTrait pTrait)
  {
    int num1 = this.traits.Remove(pTrait) ? 1 : 0;
    if (num1 == 0)
      return num1 != 0;
    WorldActionTrait augmentationRemove = pTrait.action_on_augmentation_remove;
    if (augmentationRemove != null)
    {
      int num2 = augmentationRemove((NanoObject) this, (BaseAugmentationAsset) pTrait) ? 1 : 0;
    }
    this.setStatsDirty();
    this.clearTraitCache();
    return num1 != 0;
  }

  public void removeTraits(ICollection<ActorTrait> pTraits)
  {
    bool flag = false;
    foreach (ActorTrait pTrait in (IEnumerable<ActorTrait>) pTraits)
    {
      if (this.traits.Remove(pTrait))
      {
        WorldActionTrait augmentationRemove = pTrait.action_on_augmentation_remove;
        if (augmentationRemove != null)
        {
          int num = augmentationRemove((NanoObject) this, (BaseAugmentationAsset) pTrait) ? 1 : 0;
        }
        flag = true;
      }
    }
    if (!flag)
      return;
    this.setStatsDirty();
    this.clearTraitCache();
  }

  public void clearTraitCache() => this._traits_cache.Clear();

  private void removeOppositeTraits(ActorTrait pTrait)
  {
    if (!pTrait.hasOppositeTraits<ActorTrait>())
      return;
    this.removeTraits((ICollection<ActorTrait>) pTrait.opposite_traits);
  }

  public bool addTrait(string pTraitID, bool pRemoveOpposites = false)
  {
    ActorTrait pTrait = AssetManager.traits.get(pTraitID);
    return pTrait != null && this.addTrait(pTrait, pRemoveOpposites);
  }

  public bool addTrait(ActorTrait pTrait, bool pRemoveOpposites = false)
  {
    if (this.hasTrait(pTrait) || pTrait.affects_mind && this.hasTag("strong_mind"))
      return false;
    if (pTrait.traits_to_remove != null)
      this.removeTraits((ICollection<ActorTrait>) pTrait.traits_to_remove);
    if (pRemoveOpposites)
      this.removeOppositeTraits(pTrait);
    else if (this.hasOppositeTrait(pTrait))
      return false;
    this.traits.Add(pTrait);
    WorldActionTrait onAugmentationAdd = pTrait.action_on_augmentation_add;
    if (onAugmentationAdd != null)
    {
      int num = onAugmentationAdd((NanoObject) this, (BaseAugmentationAsset) pTrait) ? 1 : 0;
    }
    this.setStatsDirty();
    this.clearTraitCache();
    return true;
  }

  internal bool hasOppositeTrait(string pTraitID)
  {
    return TraitTools.hasOppositeTrait(pTraitID, this.traits);
  }

  internal bool hasOppositeTrait(ActorTrait pTrait) => pTrait.hasOppositeTrait(this.traits);

  public void generateRandomSpawnTraits(ActorAsset pAsset)
  {
    if (pAsset.traits == null)
      return;
    for (int index = 0; index < pAsset.traits.Count; ++index)
      this.addTrait(pAsset.traits[index]);
  }

  public void checkTraitMutationOnBirth()
  {
    if (!this.hasSubspecies())
      return;
    int mutationsActorTraits = this.subspecies.getAmountOfRandomMutationsActorTraits();
    if (mutationsActorTraits == 0)
      return;
    for (int index = 0; index < mutationsActorTraits; ++index)
    {
      ActorTrait random = AssetManager.traits.pot_traits_birth.GetRandom<ActorTrait>();
      if (this.asset.traits_ignore == null || !this.asset.traits_ignore.Contains(random.id))
        this.addTrait(random);
    }
  }

  public void checkTraitMutationGrowUp()
  {
    if (!this.hasSubspecies())
      return;
    int num = Randy.randomInt(0, 3);
    for (int index = 0; index < num; ++index)
    {
      ActorTrait random = AssetManager.traits.pot_traits_growup.GetRandom<ActorTrait>();
      if ((this.asset.traits_ignore == null || !this.asset.traits_ignore.Contains(random.id)) && (!random.acquire_grow_up_sapient_only || this.isSapient()))
        this.addTrait(random);
    }
  }

  public int countTraits() => this.traits.Count;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasTrait(string pTraitID)
  {
    bool flag;
    if (!this._traits_cache.TryGetValue(pTraitID, out flag))
    {
      flag = this.hasTrait(AssetManager.traits.get(pTraitID));
      this._traits_cache[pTraitID] = flag;
    }
    return flag;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasTrait(ActorTrait pTrait) => this.traits.Contains(pTrait);

  public void updateParallelChecks(float pElapsed)
  {
    this._update_done = false;
    this._beh_skip = false;
    if ((double) this.timer_jump_animation > 0.0)
      this.timer_jump_animation -= pElapsed;
    this.checkFindCurrentTile();
    this.checkIsInLiquid();
    if (this.asset.update_z && (double) this.position_height != 0.0)
      this.updateFall();
    if (this.attackedBy != null && !this.attackedBy.isAlive())
      this.attackedBy = (BaseSimObject) null;
    if (this.is_inside_boat)
      return;
    this.updateFlipRotation(pElapsed);
    if (this.under_forces)
    {
      for (int index = 0; (double) index < (double) Config.time_scale_asset.multiplier; ++index)
        this.updateVelocity();
    }
    if (World.world.isPaused() || !this.isAlive())
      return;
    this.updateRotations(pElapsed);
    if ((double) this.attack_timer >= 0.0)
      this.attack_timer -= pElapsed;
    this.updateWalkJump(World.world.delta_time);
    if ((double) this._timeout_targets >= 0.0)
      this._timeout_targets -= World.world.delta_time;
    if ((double) this.timer_action >= 0.0)
      this.timer_action -= pElapsed;
    if (this.isAllowedToLookForEnemies())
      this.targets_to_ignore_timer.update(pElapsed);
    this.updateChangeScale(pElapsed);
    if (this.is_immovable)
      return;
    this.precalcMovementSpeed();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void skipUpdates() => this._update_done = true;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void skipBehaviour() => this._beh_skip = true;

  public void u1_checkInside(float pElapsed)
  {
    if (!this.isInsideSomething() || !this.is_inside_boat)
      return;
    this.setCurrentTilePosition(this.inside_boat.actor.current_tile);
    this.skipUpdates();
  }

  public void u2_updateChildren(float pElapsed)
  {
    if (this._update_done)
      return;
    this.updateChildrenList(this.children_special, pElapsed);
    this.updateChildrenListSimple(this.children_pre_behaviour, pElapsed);
  }

  public void u3_spriteAnimation(float pElapsed)
  {
    if (this._update_done || !this.is_visible)
      return;
    this.sprite_animation.update(pElapsed);
  }

  public void u4_deadCheck(float pElapsed)
  {
    if (this._update_done)
      return;
    if (!this.isAlive())
    {
      this.updateDeadAnimation(pElapsed);
      this.skipUpdates();
    }
    else
    {
      if (!this.isInMagnet() && !this.under_forces)
        return;
      this.skipUpdates();
    }
  }

  public void u5_curTileAction()
  {
    if (this._update_done || (double) this.position_height > 0.0)
      return;
    WorldTile currentTile = this.current_tile;
    TileTypeBase type = currentTile.Type;
    if (this.isFlying())
      return;
    if (type.block && !this.ignoresBlocks())
    {
      if (this.asset.move_from_block && !this.is_moving && (!this.hasTask() || !this.ai.task.move_from_block))
        this.setTask("move_from_block", pCleanJob: true);
      if (this.asset.die_on_blocks && !this.isUnderDamageCooldown() && !this._shake_active && this.getHealth() > 1)
        this.getHit(1f, pAttackType: AttackType.Gravity);
      if (!this.isInAir() || this.isHovering())
      {
        this.applyRandomForce(pMaxHeight: 3f);
        if (Randy.randomChance(0.02f))
          this.makeStunned();
      }
      if (!type.mountains && !type.wall)
        return;
      this.checkDieOnGroundBoat();
    }
    else
    {
      if (type.ground)
      {
        if (currentTile.isOnFire() && !this.isImmuneToFire())
        {
          ActionLibrary.addBurningEffectOnTarget((BaseSimObject) null, (BaseSimObject) this);
          if (!this.isAlive())
          {
            if (!this._update_done)
              Debug.LogError((object) "If you ever see me, remove this line");
            this.skipUpdates();
            return;
          }
        }
        if (this.isWaterCreature() && !this.asset.force_land_creature)
        {
          this.spendStaminaWithCooldown(Randy.randomInt(1, 6));
          if (!this.isUnderDamageCooldown() && !this._shake_active)
            this.getHit(1f);
        }
        this.checkDieOnGroundBoat();
      }
      else if (type.liquid)
      {
        if (type.damaged_when_walked)
          currentTile.tryToBreak();
        if (!type.lava)
          this.finishStatusEffect("burning");
        if (this.isDamagedByOcean() && currentTile.Type.ocean && !this.isUnderDamageCooldown() && !this._shake_active)
          this.getHit((float) this.getWaterDamage(), pAttackType: AttackType.Water);
        if (!this.hasTag("fast_swimming") && !this.isWaterCreature() && !this.isInAir())
        {
          this.spendStaminaWithCooldown(Randy.randomInt(1, 6));
          if (this.getStamina() <= 0 && !this.isUnderDamageCooldown())
            this.addStatusEffect("drowning", pColorEffect: false);
        }
      }
      if (type.damage_units && !this.isUnderDamageCooldown() && (!type.lava || this.asset.die_in_lava && !this.isImmuneToFire()))
      {
        this.getHit((float) type.damage, pAttackType: AttackType.Fire);
        if (!this.hasHealth())
        {
          if (type.lava)
            CursedSacrifice.checkGoodForSacrifice(this);
          this.skipUpdates();
        }
      }
      if (!currentTile.hasBuilding() || !currentTile.building.asset.has_step_action)
        return;
      currentTile.building.asset.step_action(this, currentTile.building);
      if (this.hasHealth())
        return;
      this.skipUpdates();
    }
  }

  public void u6_checkFrozen(float pElapsed)
  {
    if (this._update_done || !this.is_ai_frozen && !this.is_unconscious)
      return;
    this.skipUpdates();
  }

  public void u8_checkUpdateTimers(float pElapsed)
  {
    if (this._update_done)
      return;
    if ((double) this.timer_action >= 0.0)
    {
      this.skipUpdates();
    }
    else
    {
      if (this.isAlive())
        return;
      if (!this._update_done)
        Debug.LogError((object) "If you ever see me, remove this line");
      this.skipUpdates();
    }
  }

  public void u7_checkAugmentationEffects()
  {
    if (this._update_done || (double) World.world.getWorldTimeElapsedSince(this._timestamp_augmentation_effects) < 1.0)
      return;
    List<BaseAugmentationAsset> augmentationList = Actor._tempAugmentationList;
    Dictionary<BaseAugmentationAsset, double> augmentationsTimers = this._s_special_effect_augmentations_timers;
    double curWorldTime = World.world.getCurWorldTime();
    this._timestamp_augmentation_effects = curWorldTime;
    int index1 = 0;
    for (int count = this._s_special_effect_augmentations.Count; index1 < count; ++index1)
    {
      BaseAugmentationAsset effectAugmentation = this._s_special_effect_augmentations[index1];
      double pTime;
      if (augmentationsTimers.TryGetValue(effectAugmentation, out pTime))
      {
        if ((double) World.world.getWorldTimeElapsedSince(pTime) >= (double) effectAugmentation.special_effect_interval)
          augmentationList.Add(effectAugmentation);
        else
          continue;
      }
      augmentationsTimers[effectAugmentation] = curWorldTime;
    }
    if (augmentationList.Count == 0)
      return;
    int index2 = 0;
    for (int count = augmentationList.Count; index2 < count; ++index2)
    {
      BaseAugmentationAsset augmentationAsset = augmentationList[index2];
      WorldAction actionSpecialEffect = augmentationAsset.action_special_effect;
      if (Bench.bench_enabled)
      {
        double sinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
        int num = actionSpecialEffect((BaseSimObject) this, this.current_tile) ? 1 : 0;
        double pValue = Time.realtimeSinceStartupAsDouble - sinceStartupAsDouble;
        Bench.benchSaveSplit(augmentationAsset.id, pValue, 1, "effects_traits");
      }
      else
      {
        int num1 = actionSpecialEffect((BaseSimObject) this, this.current_tile) ? 1 : 0;
      }
    }
    Actor._tempAugmentationList.Clear();
  }

  public void b1_checkUnderForce(float pElapsed)
  {
    if (this._update_done)
      return;
    if (this.under_forces)
    {
      this.skipBehaviour();
    }
    else
    {
      if (!this.asset.update_z || (double) this.position_height == 0.0)
        return;
      this.skipBehaviour();
    }
  }

  public void b2_checkCurrentEnemyTarget(float pElapsed)
  {
    if (this._update_done || this._beh_skip || !this.checkCurrentEnemyTarget())
      return;
    this.skipBehaviour();
  }

  public void b3_findEnemyTarget(float pElapsed)
  {
    if (this._update_done || this._beh_skip || !this.checkEnemyTargets())
      return;
    this.stopMovement();
    this.skipBehaviour();
  }

  public void b4_checkTaskVerifier(float pElapsed)
  {
    if (this._update_done || this._beh_skip)
      return;
    if (this.hasTask() && this.ai.task.has_verifier && this.ai.task.task_verifier.execute(this) == BehResult.Stop)
    {
      this.cancelAllBeh();
      this.skipBehaviour();
    }
    else
    {
      if (!this.is_moving)
        return;
      this.skipBehaviour();
    }
  }

  public void b5_checkPathMovement(float pElapsed)
  {
    if (this._update_done || this._beh_skip || !this.isUsingPath())
      return;
    this.updatePathMovement();
    this.skipBehaviour();
  }

  public void b6_0_updateDecision(float pElapsed)
  {
    if (this._update_done || this._beh_skip || this.is_unconscious || this._has_status_possessed || !this.asset.has_ai_system)
      return;
    DecisionHelper.makeDecisionFor(this, out this._last_decision_id);
  }

  public string getLastDecisionForMindOverview() => this._last_decision_id;

  public void b6_updateAI(float pElapsed)
  {
    if (this._update_done || this._beh_skip || this.is_unconscious || this._has_status_possessed || !this.asset.has_ai_system)
      return;
    this.ai.update();
  }

  public void b55_updateNaturalDeaths(float pElapsed)
  {
    if (this._update_done || this._beh_skip || this.is_unconscious || this._has_status_possessed || !this.asset.has_ai_system || this.ai.action_index != 0 || !this.checkNaturalDeath())
      return;
    this.skipBehaviour();
    this.skipUpdates();
  }

  public void u10_checkSmoothMovement(float pElapsed)
  {
    if (this._update_done || this.is_immovable)
      return;
    if (!Config.time_scale_asset.sonic)
      this.checkCalibrateTargetPosition();
    this.updateMovement(pElapsed);
  }
}
