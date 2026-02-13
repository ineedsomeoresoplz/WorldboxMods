// Decompiled with JetBrains decompiler
// Type: BaseSimObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BaseSimObject : NanoObject, IEquatable<BaseSimObject>
{
  public float position_height;
  public WorldTile current_tile;
  public Vector2 current_position;
  public Vector3 current_scale;
  internal Vector3 current_rotation;
  private HashSet<long> _targets_to_ignore;
  [NonSerialized]
  public Kingdom kingdom;
  private bool _stats_dirty;
  internal bool event_full_stats;
  internal readonly BaseStats stats = new BaseStats();
  internal Actor a;
  internal Building b;
  private MapObjectType _object_type;
  private readonly Dictionary<string, Status> _active_status_dict = new Dictionary<string, Status>();
  private bool _has_any_status_cached;
  private bool _has_any_status_to_render;
  internal Vector3 cur_transform_position;

  public TileIsland current_island => this.current_tile.region.island;

  public TileZone current_zone => this.current_tile.zone;

  public MapChunk current_chunk => this.current_tile.chunk;

  public MapRegion current_region => this.current_tile.region;

  internal virtual void create()
  {
  }

  public int countStatusEffects() => this._active_status_dict.Count;

  public Dictionary<string, Status>.ValueCollection getStatuses()
  {
    return this._active_status_dict.Values;
  }

  public Dictionary<string, Status>.KeyCollection getStatusesIds() => this._active_status_dict.Keys;

  public IReadOnlyDictionary<string, Status> getStatusesDict()
  {
    return (IReadOnlyDictionary<string, Status>) this._active_status_dict;
  }

  protected override void setDefaultValues()
  {
    base.setDefaultValues();
    this._stats_dirty = true;
    this.event_full_stats = false;
    this.current_rotation = new Vector3();
    this.position_height = 0.0f;
    this._has_any_status_cached = false;
    this._has_any_status_to_render = false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasCity() => this.getCity() != null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual City getCity() => (City) null;

  internal bool addStatusEffect(string pID, float pOverrideTimer = 0.0f, bool pColorEffect = true)
  {
    StatusAsset pStatusAsset = AssetManager.status.get(pID);
    return pStatusAsset != null && this.addStatusEffect(pStatusAsset, pOverrideTimer, pColorEffect);
  }

  internal virtual bool addStatusEffect(
    StatusAsset pStatusAsset,
    float pOverrideTimer = 0.0f,
    bool pColorEffect = true)
  {
    if (!this.isAlive())
      return false;
    bool pIsActor = this.isActor();
    if (pIsActor && this.a.asset.allowed_status_tiers < pStatusAsset.tier)
      return false;
    bool pHasAnyStatus = this.hasAnyStatusEffectRaw();
    if (pHasAnyStatus && this.hasStatus(pStatusAsset.id))
    {
      if (!pStatusAsset.allow_timer_reset && (double) pOverrideTimer == 0.0)
        return false;
      Status status = this._active_status_dict[pStatusAsset.id];
      float pDuration = pStatusAsset.duration;
      if ((double) pOverrideTimer != 0.0)
        pDuration = pOverrideTimer;
      if (status.getRemainingTime() < (double) pDuration)
        status.setDuration(pDuration);
      return true;
    }
    if (!this.canAddStatus(pStatusAsset, pIsActor, pColorEffect))
      return false;
    this.addNewStatusEffect(pStatusAsset, pOverrideTimer, pColorEffect, pIsActor, pHasAnyStatus);
    return true;
  }

  private bool canAddStatus(StatusAsset pStatusAsset, bool pIsActor, bool pHasAnyStatus)
  {
    if (pIsActor)
    {
      if (pStatusAsset.opposite_traits != null)
      {
        for (int index = 0; index < pStatusAsset.opposite_traits.Length; ++index)
        {
          if (this.a.hasTrait(pStatusAsset.opposite_traits[index]))
            return false;
        }
      }
      if (pStatusAsset.opposite_tags != null && this.a.stats.hasTags() && this.a.stats.hasTags(pStatusAsset.opposite_tags))
        return false;
    }
    if (pStatusAsset.opposite_status != null & pHasAnyStatus)
    {
      for (int index = 0; index < pStatusAsset.opposite_status.Length; ++index)
      {
        if (this.hasStatus(pStatusAsset.opposite_status[index]))
          return false;
      }
    }
    return true;
  }

  private void addNewStatusEffect(
    StatusAsset pStatusAsset,
    float pOverrideTimer,
    bool pColorEffect,
    bool pIsActor,
    bool pHasAnyStatus)
  {
    Status status = World.world.statuses.newStatus(this, pStatusAsset, pOverrideTimer);
    this.setStatsDirty();
    this._active_status_dict.Add(pStatusAsset.id, status);
    this._has_any_status_cached = true;
    if (((!pIsActor ? 0 : (pStatusAsset.cancel_actor_job ? 1 : 0)) & (pColorEffect ? 1 : 0)) != 0)
    {
      this.a.cancelAllBeh();
      this.a.startColorEffect();
    }
    if (pStatusAsset.remove_status != null & pHasAnyStatus)
    {
      for (int index = 0; index < pStatusAsset.remove_status.Length; ++index)
        this.finishStatusEffect(pStatusAsset.remove_status[index]);
    }
    if (!pIsActor)
      return;
    WorldAction actionOnReceive = pStatusAsset.action_on_receive;
    if (actionOnReceive == null)
      return;
    int num = actionOnReceive(this) ? 1 : 0;
  }

  internal void finishAllStatusEffects()
  {
    foreach (Status status in this._active_status_dict.Values)
    {
      status.finish();
      this.setStatsDirty();
    }
    this._active_status_dict.Clear();
    this._has_any_status_cached = false;
    this._has_any_status_to_render = false;
  }

  public void finishStatusEffect(string pID)
  {
    Status status;
    if (!this.hasAnyStatusEffect() || !this._active_status_dict.TryGetValue(pID, out status))
      return;
    status.finish();
    this.setStatsDirty();
  }

  public virtual void setStatsDirty() => this._stats_dirty = true;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isActor() => this._object_type == MapObjectType.Actor;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isBuilding() => this._object_type == MapObjectType.Building;

  public void setObjectType(MapObjectType pType)
  {
    this._object_type = pType;
    if (this._object_type == MapObjectType.Actor)
      this.a = (Actor) this;
    else
      this.b = (Building) this;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool hasStatus(string pID) => this._active_status_dict.ContainsKey(pID);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool hasAnyStatusEffect() => this._has_any_status_cached;

  internal bool hasAnyStatusEffectRaw() => this._active_status_dict.Count > 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool hasAnyStatusEffectToRender() => this._has_any_status_to_render;

  public void removeFinishedStatusEffect(Status pStatusData)
  {
    this._active_status_dict.Remove(pStatusData.asset.id);
    this._has_any_status_cached = this.hasAnyStatusEffectRaw();
    this.setStatsDirty();
  }

  internal virtual void updateStats()
  {
    this._stats_dirty = false;
    ++this.stats_dirty_version;
    this.updateCachedStatusEffects();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isStatsDirty() => this._stats_dirty;

  private void updateCachedStatusEffects()
  {
    this._has_any_status_cached = this.hasAnyStatusEffectRaw();
    this._has_any_status_to_render = false;
    if (!this._has_any_status_cached)
      return;
    foreach (Status status in this._active_status_dict.Values)
    {
      if (!status.is_finished && status.asset.need_visual_render)
      {
        this._has_any_status_to_render = true;
        break;
      }
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isInLiquid() => this.current_tile.Type.liquid;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isInWater() => this.current_tile.Type.ocean;

  public bool isTouchingLiquid() => this.isInLiquid() && !this.isInAir();

  internal virtual bool isInAir() => false;

  internal virtual bool isFlying() => false;

  internal virtual float getHeight() => 0.0f;

  internal virtual void getHit(
    float pDamage,
    bool pFlash = true,
    AttackType pAttackType = AttackType.Other,
    BaseSimObject pAttacker = null,
    bool pSkipIfShake = true,
    bool pMetallicWeapon = false,
    bool pCheckDamageReduction = true)
  {
  }

  internal virtual void getHitFullHealth(AttackType pAttackType)
  {
  }

  internal BaseSimObject findEnemyObjectTarget(bool pAttackBuildings)
  {
    EnemyFinderData enemiesFrom = EnemiesFinder.findEnemiesFrom(this.current_tile, this.kingdom);
    if (enemiesFrom.isEmpty())
      return (BaseSimObject) null;
    bool pFindClosest = true;
    if (enemiesFrom.list.Count > 50)
      pFindClosest = Randy.randomChance(0.6f);
    return this.checkObjectList(pFindClosest ? (IEnumerable<BaseSimObject>) enemiesFrom.list : enemiesFrom.list.LoopRandom<BaseSimObject>(), pAttackBuildings, pFindClosest, false);
  }

  protected BaseSimObject checkObjectList(
    IEnumerable<BaseSimObject> pList,
    bool pAttackBuildings,
    bool pFindClosest,
    bool pIgnoreStunned,
    int pMaxDist = 2147483647 /*0x7FFFFFFF*/)
  {
    int num1 = int.MaxValue;
    BaseSimObject baseSimObject = (BaseSimObject) null;
    long num2 = pMaxDist == int.MaxValue ? (long) pMaxDist : (long) (pMaxDist * pMaxDist + 1);
    bool flag = this.isActor() && this.a.hasMeleeAttack();
    WorldTile currentTile1 = this.current_tile;
    Vector2Int pos = currentTile1.pos;
    foreach (BaseSimObject p in pList)
    {
      if (p.isAlive() && p != this)
      {
        WorldTile currentTile2 = p.current_tile;
        if (pFindClosest)
        {
          num1 = Toolbox.SquaredDistVec2(currentTile2.pos, pos);
          if ((long) num1 >= num2)
            continue;
        }
        if ((!pIgnoreStunned || !p.isActor() || !p.a.hasStatusStunned()) && this.canAttackTarget(p, pAttackBuildings: pAttackBuildings) && (!flag || currentTile2.isSameIsland(currentTile1) || !currentTile2.Type.block && currentTile1.region.island.isConnectedWith(currentTile2.region.island)) && (!p.isBuilding() || !this.isKingdomCiv() || !p.b.asset.city_building || p.b.asset.tower || !(p.kingdom.getSpecies() == this.kingdom.getSpecies())) && !this.shouldIgnoreTarget(p))
        {
          if (!pFindClosest || num1 <= 4)
            return p;
          baseSimObject = p;
          num2 = (long) num1;
        }
      }
    }
    return baseSimObject;
  }

  internal void ignoreTarget(BaseSimObject pTarget)
  {
    if (this._targets_to_ignore == null)
      this._targets_to_ignore = new HashSet<long>();
    this._targets_to_ignore.Add(pTarget.getID());
  }

  internal bool shouldIgnoreTarget(BaseSimObject pTarget)
  {
    HashSet<long> targetsToIgnore = this._targets_to_ignore;
    // ISSUE: explicit non-virtual call
    return targetsToIgnore != null && __nonvirtual (targetsToIgnore.Contains(pTarget.getID()));
  }

  internal void clearIgnoreTargets() => this._targets_to_ignore?.Clear();

  internal int countTargetsToIgnore()
  {
    HashSet<long> targetsToIgnore = this._targets_to_ignore;
    // ISSUE: explicit non-virtual call
    return targetsToIgnore == null ? 0 : __nonvirtual (targetsToIgnore.Count);
  }

  internal bool canAttackTarget(
    BaseSimObject pTarget,
    bool pCheckForFactions = true,
    bool pAttackBuildings = true)
  {
    if (!this.isAlive() || !pTarget.isAlive())
      return false;
    bool flag1 = this.isActor();
    if (pTarget.isBuilding() && !pAttackBuildings && (!flag1 || !this.a.asset.unit_zombie || !pTarget.kingdom.asset.brain))
      return false;
    string str;
    WeaponType weaponType;
    if (flag1)
    {
      if (this.a.asset.skip_fight_logic)
        return false;
      str = this.a.asset.id;
      weaponType = this.a._attack_asset.attack_type;
    }
    else
    {
      str = this.b.kingdom.getSpecies();
      weaponType = WeaponType.Range;
    }
    if (pTarget.isActor())
    {
      Actor a = pTarget.a;
      if (!a.asset.can_be_killed_by_stuff || a.isInsideSomething() || a.isFlying() && weaponType == WeaponType.Melee || a.ai.action != null && a.ai.action.special_prevent_can_be_attacked || a.isInMagnet())
        return false;
      if (pCheckForFactions && this.areFoes(pTarget) && a.isKingdomCiv() && this.isKingdomCiv() && !this.hasStatusTantrum() && !a.hasStatusTantrum())
      {
        bool flag2 = flag1 && this.a.hasXenophobic() || a.hasXenophobic();
        bool flag3 = flag1 && this.a.hasXenophiles() || a.hasXenophiles();
        bool flag4 = flag1 && this.a.culture == a.culture;
        bool flag5 = str == a.asset.id;
        bool flag6 = flag5 | flag3 && !flag2 || flag4 & flag5;
        if (!WorldLawLibrary.world_law_angry_civilians.isEnabled() && (a.profession_asset.is_civilian & flag6 || ((!flag1 ? 0 : (this.a.profession_asset.is_civilian ? 1 : 0)) & (flag6 ? 1 : 0)) != 0))
          return false;
      }
      if (pCheckForFactions & flag1 && this.a.hasCannibalism() && this.a.isSameSpecies(a))
      {
        Family family1 = this.a.family;
        Family family2 = a.family;
        if (family2 == null || family1 == null || this.a.hasFamily() && (family2 == family1 || !family2.areMostUnitsHungry() && !family1.areMostUnitsHungry()))
          return false;
      }
    }
    else
    {
      Building b = pTarget.b;
      if (this.isKingdomCiv() && b.asset.city_building && b.asset.tower && !b.isCiv() && flag1 && this.a.profession_asset.is_civilian && !WorldLawLibrary.world_law_angry_civilians.isEnabled() && b.kingdom.getSpecies() == this.kingdom.getSpecies())
        return false;
    }
    if (flag1)
    {
      ActorAsset asset = this.a.asset;
      if (!this.a.isWaterCreature() || !this.a.hasRangeAttack())
      {
        if (this.a.isWaterCreature() && !asset.force_land_creature)
        {
          if (!pTarget.isInLiquid() || !pTarget.current_tile.isSameIsland(this.current_tile))
            return false;
        }
        else if (weaponType == WeaponType.Melee && pTarget.isInLiquid() && !this.a.isWaterCreature())
          return false;
      }
    }
    return true;
  }

  public bool areFoes(BaseSimObject pTarget) => this.kingdom.isEnemy(pTarget.kingdom);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void setHealth(int pValue, bool pClamp = true)
  {
    BaseObjectData data = this.getData();
    if (pClamp)
      pValue = Mathf.Clamp(pValue, 1, this.getMaxHealth());
    int num = pValue;
    data.health = num;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void setMaxHealth() => this.setHealth(this.getMaxHealth());

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void changeHealth(int pValue)
  {
    BaseObjectData data = this.getData();
    data.health = Mathf.Clamp(data.health + pValue, 0, this.getMaxHealth());
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int getHealth() => this.getData().health;

  public int getMaxHealthPercent(float pPercent)
  {
    return Mathf.Max(1, (int) ((double) this.getMaxHealth() * (double) pPercent));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasHealth() => this.getHealth() > 0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(BaseSimObject pObject) => this._hashcode == pObject.GetHashCode();

  public int getMaxHealth() => (int) this.stats["health"];

  public override void Dispose()
  {
    this.current_tile = (WorldTile) null;
    this.kingdom = (Kingdom) null;
    this.stats.reset();
    this.clearIgnoreTargets();
    this._targets_to_ignore = (HashSet<long>) null;
    this.disposeStatusEffects();
    this.current_tile = (WorldTile) null;
    base.Dispose();
  }

  private void disposeStatusEffects()
  {
    foreach (Status status in this._active_status_dict.Values)
      status.finish();
    this._active_status_dict.Clear();
    this._has_any_status_cached = false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isKingdomCiv() => this.kingdom.isCiv();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isKingdomMob() => this.kingdom.isMobs();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasKingdom() => this.kingdom != null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual BaseObjectData getData() => (BaseObjectData) null;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public sealed override long getID() => this.getData().id;

  public override double getFoundedTimestamp() => this.getData().created_time;

  public virtual bool hasStatusTantrum() => false;

  public bool isSameIsland(WorldTile pTile) => this.current_tile.isSameIsland(pTile);

  public bool isSameIslandAs(BaseSimObject pTarget)
  {
    return this.current_tile.isSameIsland(pTarget.current_tile);
  }

  public MapChunk chunk => this.current_tile.chunk;
}
