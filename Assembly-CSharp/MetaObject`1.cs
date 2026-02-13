// Decompiled with JetBrains decompiler
// Type: MetaObject`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class MetaObject<TData> : CoreSystemObject<TData>, IMetaObject, ICoreObject where TData : MetaObjectData
{
  private bool _units_dirty;
  protected static readonly HashSet<Family> _family_counter = new HashSet<Family>();
  private ColorAsset _cached_color;
  private bool _force_preserve_alive;
  private int _cursor_over;
  private double _timestamp_last_visible = -1.0;
  private Actor _cached_visible_unit;
  private long _cached_visible_unit_id;

  protected virtual bool track_death_types => false;

  public List<Actor> units { get; } = new List<Actor>();

  public void preserveAlive() => this._force_preserve_alive = true;

  protected override void setDefaultValues()
  {
    base.setDefaultValues();
    this._units_dirty = true;
    this._force_preserve_alive = true;
  }

  public virtual bool isReadyForRemoval() => !this._force_preserve_alive && this.units.Count <= 0;

  internal virtual void clearListUnits()
  {
    this._force_preserve_alive = false;
    this.units.Clear();
  }

  public virtual void listUnit(Actor pActor) => this.units.Add(pActor);

  public bool isLocked() => this.isDirtyUnits();

  public bool isDirtyUnits() => this._units_dirty;

  public void unDirty()
  {
    ++this.stats_dirty_version;
    this._units_dirty = false;
  }

  public void setDirty() => this._units_dirty = true;

  public virtual void updateDirty()
  {
  }

  public override void Dispose()
  {
    if (!Config.disable_dispose_logs)
      Debug.Log((object) $"MetaObject::Dispose {this.data.id.ToString()} {this.data.name}");
    this.clearListUnits();
    this._cached_color = (ColorAsset) null;
    this.clearCachedVisibleUnit();
    base.Dispose();
  }

  protected virtual ColorLibrary getColorLibrary()
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public override bool updateColor(ColorAsset pColor)
  {
    if (this.getColor() == pColor)
      return false;
    this.data.setColorID(this.getColorLibrary().list.IndexOf(pColor));
    this._cached_color = (ColorAsset) null;
    return true;
  }

  public bool isCursorOver() => this._cursor_over > 0;

  public void setCursorOver() => this._cursor_over = 3;

  public void clearCursorOver()
  {
    if (this._cursor_over <= 0)
      return;
    --this._cursor_over;
  }

  public override ColorAsset getColor()
  {
    if (this._cached_color == null)
      this._cached_color = this.getColorLibrary().list[this.data.color_id];
    return this._cached_color;
  }

  public override void trackName(bool pPostChange = false)
  {
    if (string.IsNullOrEmpty(this.data.name) || pPostChange && (this.data.past_names == null || this.data.past_names.Count == 0))
      return;
    BaseSystemData data = (BaseSystemData) this.data;
    if (data.past_names == null)
      data.past_names = new List<NameEntry>();
    if (this.data.past_names.Count == 0)
    {
      this.data.past_names.Add(new NameEntry(this.data.name, false, this.data.original_color_id, this.data.created_time));
    }
    else
    {
      if (this.data.past_names.Last<NameEntry>().name == this.data.name)
        return;
      this.data.past_names.Add(new NameEntry(this.data.name, this.data.custom_name, this.data.color_id));
    }
  }

  protected virtual void generateNewMetaObject(bool pAddDefaultTraits)
  {
    this.generateNewMetaObject();
  }

  protected virtual void generateNewMetaObject()
  {
    this.generateColor();
    this.generateBanner();
  }

  public virtual void generateBanner() => throw new NotImplementedException(this.GetType().Name);

  protected virtual void generateColor()
  {
    ActorAsset actorAsset = this.getActorAsset();
    this.data.setColorID(this.getColorLibrary().getNextColorIndex(actorAsset));
  }

  public bool isSelected() => SelectedObjects.isNanoObjectSelected((NanoObject) this);

  public virtual int countUnits() => this.units.Count;

  public virtual IEnumerable<Actor> getUnits() => (IEnumerable<Actor>) this.units;

  public virtual Actor getRandomUnit() => Randy.getRandom<Actor>(this.units);

  public Actor getRandomActorForReaper()
  {
    foreach (Actor randomActorForReaper in this.units.LoopRandom<Actor>())
    {
      if (randomActorForReaper.isAlive())
        return randomActorForReaper;
    }
    return (Actor) null;
  }

  public virtual int countHappyUnits()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.asset.is_boat && unit.isHappy())
        ++num;
    }
    return num;
  }

  public virtual int countUnhappyUnits()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.asset.is_boat && unit.isUnhappy())
        ++num;
    }
    return num;
  }

  public virtual int countSingleMales()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isBreedingAge() && unit.isSexMale() && !unit.hasLover())
        ++num;
    }
    return num;
  }

  public virtual int countCouples()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.hasLover())
        ++num;
    }
    return num / 2;
  }

  public virtual int countSingleFemales()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isBreedingAge() && unit.isSexFemale() && !unit.hasLover())
        ++num;
    }
    return num;
  }

  public virtual int countHoused()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.asset.is_boat && unit.hasHouse())
        ++num;
    }
    return num;
  }

  public virtual int countHomeless()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.asset.is_boat && !unit.hasHouse())
        ++num;
    }
    return num;
  }

  public virtual int countStarving()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isStarving())
        ++num;
    }
    return num;
  }

  public virtual int countHungry()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isHungry())
        ++num;
    }
    return num;
  }

  public virtual int countSick()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isSick())
        ++num;
    }
    return num;
  }

  public virtual int countAdults()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isAlive() && !unit.asset.is_boat && unit.isAdult())
        ++num;
    }
    return num;
  }

  public virtual int countTotalMoney()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isAlive())
        num += unit.money;
    }
    return num;
  }

  public int countPotentialParents(ActorSex pSex)
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isAlive() && !unit.asset.is_boat && unit.data.sex == pSex && unit.canBreed() && !unit.hasReachedOffspringLimit())
        ++num;
    }
    return num;
  }

  public int countUnitsWithStatus(string pStatusID)
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isAlive() && unit.hasStatus(pStatusID))
        ++num;
    }
    return num;
  }

  public virtual int countChildren()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.asset.is_boat && unit.isAlive() && unit.isBaby())
        ++num;
    }
    return num;
  }

  public virtual IEnumerable<Family> getFamilies()
  {
    MetaObject<TData>._family_counter.Clear();
    try
    {
      foreach (Actor unit in this.getUnits())
      {
        if (unit.hasFamily() && MetaObject<TData>._family_counter.Add(unit.family))
          yield return unit.family;
      }
    }
    finally
    {
      MetaObject<TData>._family_counter.Clear();
    }
  }

  public virtual bool hasFamilies()
  {
    foreach (Actor unit in this.getUnits())
    {
      if (unit.hasFamily())
        return true;
    }
    return false;
  }

  public virtual int countFamilies()
  {
    int num = 0;
    foreach (Family family in this.getFamilies())
      ++num;
    return num;
  }

  public int countKings()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isKing())
        ++num;
    }
    return num;
  }

  public int countLeaders()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isCityLeader())
        ++num;
    }
    return num;
  }

  public virtual int countMales()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isAlive() && unit.isSexMale())
        ++num;
    }
    return num;
  }

  public virtual int countFemales()
  {
    int num = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (unit.isAlive() && unit.isSexFemale())
        ++num;
    }
    return num;
  }

  public virtual int countPopulationPercentage()
  {
    return (int) ((double) this.countUnits() / (double) World.world.units.Count * 100.0);
  }

  public virtual void increaseDeaths(AttackType pType)
  {
    if (!this.isAlive())
      return;
    ++this.data.total_deaths;
    if (!this.track_death_types)
      return;
    switch (pType)
    {
      case AttackType.Acid:
        ++this.data.deaths_acid;
        break;
      case AttackType.Fire:
        ++this.data.deaths_fire;
        break;
      case AttackType.Plague:
        ++this.data.deaths_plague;
        break;
      case AttackType.Infection:
        ++this.data.deaths_infection;
        break;
      case AttackType.Tumor:
        ++this.data.deaths_tumor;
        break;
      case AttackType.Divine:
        ++this.data.deaths_divine;
        break;
      case AttackType.Metamorphosis:
        ++this.data.metamorphosis;
        break;
      case AttackType.Starvation:
        ++this.data.deaths_hunger;
        break;
      case AttackType.Eaten:
        ++this.data.deaths_eaten;
        break;
      case AttackType.Age:
        ++this.data.deaths_natural;
        break;
      case AttackType.Weapon:
        ++this.data.deaths_weapon;
        break;
      case AttackType.Poison:
        ++this.data.deaths_poison;
        break;
      case AttackType.Gravity:
        ++this.data.deaths_gravity;
        break;
      case AttackType.Drowning:
        ++this.data.deaths_drowning;
        break;
      case AttackType.Water:
        ++this.data.deaths_water;
        break;
      case AttackType.Explosion:
        ++this.data.deaths_explosion;
        break;
      default:
        ++this.data.deaths_other;
        break;
    }
  }

  public virtual void increaseBirths()
  {
    if (!this.isAlive())
      return;
    ++this.data.total_births;
  }

  public virtual void increaseKills()
  {
    if (!this.isAlive())
      return;
    ++this.data.total_kills;
  }

  private void clearCachedVisibleUnit()
  {
    this._cached_visible_unit = (Actor) null;
    this._cached_visible_unit_id = -1L;
    this._timestamp_last_visible = -1.0;
  }

  public Actor getOldestVisibleUnitForNameplatesCached()
  {
    if ((double) World.world.getWorldTimeElapsedSince(this._timestamp_last_visible) > 5.0)
      this._cached_visible_unit = (Actor) null;
    if (!this._cached_visible_unit.isRekt() && (!this._cached_visible_unit.current_zone.visible_main_centered || this._cached_visible_unit.id != this._cached_visible_unit_id))
      this.clearCachedVisibleUnit();
    if (this._cached_visible_unit != null)
      return this._cached_visible_unit;
    this._timestamp_last_visible = World.world.getCurWorldTime();
    this._cached_visible_unit = this.getOldestVisibleUnit();
    if (this._cached_visible_unit != null)
      this._cached_visible_unit_id = this._cached_visible_unit.data.id;
    else
      this.clearCachedVisibleUnit();
    return this._cached_visible_unit;
  }

  public Actor getOldestVisibleUnit()
  {
    Actor oldestVisibleUnit = (Actor) null;
    foreach (Actor unit in this.units)
    {
      if (!unit.asset.is_boat && unit.isAlive() && unit.current_zone.visible_main_centered && (oldestVisibleUnit == null || unit.data.created_time < oldestVisibleUnit.data.created_time))
        oldestVisibleUnit = unit;
    }
    return oldestVisibleUnit;
  }

  public virtual Sprite getTopicSprite() => throw new NotImplementedException();

  public long getTotalDeaths() => this.data.total_deaths;

  public long getTotalBirths() => this.data.total_births;

  public long getTotalKills() => this.data.total_kills;

  public long getEvolutions() => this.data.evolutions;

  public void increaseEvolutions() => ++this.data.evolutions;

  public long getDeaths(AttackType pType)
  {
    switch (pType)
    {
      case AttackType.Acid:
        return this.data.deaths_acid;
      case AttackType.Fire:
        return this.data.deaths_fire;
      case AttackType.Plague:
        return this.data.deaths_plague;
      case AttackType.Infection:
        return this.data.deaths_infection;
      case AttackType.Tumor:
        return this.data.deaths_tumor;
      case AttackType.Other:
      case AttackType.AshFever:
      case AttackType.None:
        return this.data.deaths_other;
      case AttackType.Divine:
        return this.data.deaths_divine;
      case AttackType.Metamorphosis:
        return this.data.metamorphosis;
      case AttackType.Starvation:
        return this.data.deaths_hunger;
      case AttackType.Eaten:
        return this.data.deaths_eaten;
      case AttackType.Age:
        return this.data.deaths_natural;
      case AttackType.Weapon:
        return this.data.deaths_weapon;
      case AttackType.Poison:
        return this.data.deaths_poison;
      case AttackType.Gravity:
        return this.data.deaths_gravity;
      case AttackType.Drowning:
        return this.data.deaths_drowning;
      case AttackType.Water:
        return this.data.deaths_water;
      case AttackType.Explosion:
        return this.data.deaths_explosion;
      default:
        throw new ArgumentOutOfRangeException($"Unknown attack type: {pType}");
    }
  }

  public void addRenown(int pAmount) => this.data.renown += pAmount;

  public void addRenown(int pAmount, float pPercent)
  {
    this.addRenown((int) ((double) pAmount * (double) pPercent));
  }

  public MetaTypeAsset meta_type_asset => AssetManager.meta_type_library.getAsset(this.meta_type);

  public virtual void clearLastYearStats()
  {
  }

  public virtual void convertSameSpeciesAroundUnit(Actor pActorMain, bool pOverrideExisting = false)
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public virtual void forceConvertSameSpeciesAroundUnit(Actor pActorMain)
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public virtual ActorAsset getActorAsset() => (ActorAsset) null;

  public IEnumerable<Actor> getUnitFromChunkForConversion(Actor pActorMain)
  {
    foreach (Actor actor in Finder.getUnitsFromChunk(pActorMain.current_tile, 1))
    {
      if (actor.isSameSpecies(pActorMain) && (!actor.hasCity() || actor.hasSameCity(pActorMain)))
        yield return actor;
    }
  }

  public Sprite getSpriteIcon() => this.getActorAsset().getSpriteIcon();

  public void allAngryAt(Actor pActorTarget, float pDistance)
  {
    float num = pDistance * pDistance;
    WorldTile currentTile = pActorTarget.current_tile;
    bool flag = pActorTarget.hasStatus("possessed");
    foreach (Actor unit in this.getUnits())
    {
      if (unit != pActorTarget && !unit.isRekt() && (double) Toolbox.SquaredDistTile(unit.current_tile, currentTile) <= (double) num && (!flag || !unit.hasStatus("possessed_follower")))
        unit.addAggro(pActorTarget);
    }
  }

  public virtual bool hasUnits()
  {
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.isRekt() && !unit.asset.is_boat)
        return true;
    }
    return false;
  }

  public virtual void triggerOnRemoveObject()
  {
  }

  public MetaObjectData getMetaData() => (MetaObjectData) this.data;

  public int getRenown() => this.data.renown;

  public virtual int getPopulationPeople() => this.units.Count;

  public virtual bool hasCities() => throw new NotImplementedException();

  public virtual IEnumerable<City> getCities() => throw new NotImplementedException();

  public virtual bool hasKingdoms() => throw new NotImplementedException();

  public virtual IEnumerable<Kingdom> getKingdoms() => throw new NotImplementedException();
}
