// Decompiled with JetBrains decompiler
// Type: WorldAgeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldAgeManager
{
  private WorldAgeEffects _effects;
  private float _night_multiplier;
  private float _timer_special_action;
  private WorldAgeAsset _current_age;
  private float NIGHT_MAX = 0.6f;
  private float NIGHT_SPEED = 0.1f;

  private static MapStats _map_stats => World.world.map_stats;

  public WorldAgeAsset getCurrentAge() => this._current_age;

  public WorldAgeAsset getNextAge() => this.getAgeFromSlot(this.getNextSlotIndex());

  public int getCurrentSlotIndex() => WorldAgeManager._map_stats.world_age_slot_index;

  public int getNextSlotIndex()
  {
    return Toolbox.loopIndex(WorldAgeManager._map_stats.world_age_slot_index + 1, WorldAgeManager._map_stats.world_ages_slots.Length);
  }

  public WorldAgeAsset getAgeFromSlot(int pIndex)
  {
    string pID = WorldAgeManager._map_stats.world_ages_slots[pIndex];
    if (string.IsNullOrEmpty(pID))
      pID = "age_unknown";
    return AssetManager.era_library.get(pID);
  }

  public void setAgeToSlot(WorldAgeAsset pAsset, int pSlotIndex)
  {
    if (WorldAgeManager._map_stats.world_ages_slots[pSlotIndex] == pAsset.id)
      return;
    WorldAgeManager._map_stats.world_ages_slots[pSlotIndex] = pAsset.id;
    if (pSlotIndex != WorldAgeManager._map_stats.world_age_slot_index)
      return;
    this.setCurrentAge(pAsset, false);
  }

  public float getNightMod()
  {
    if (Object.op_Equality((Object) this._effects, (Object) null))
      return 0.0f;
    return this._effects.override_night ? this._effects.night_value_mat : this._night_multiplier;
  }

  public bool shouldShowLights() => (double) this.getNightMod() != 0.0;

  internal void loadAge()
  {
    bool pOverrideTime = false;
    if (string.IsNullOrEmpty(WorldAgeManager._map_stats.world_age_id) || WorldAgeManager._map_stats.world_age_id == "age_unknown")
    {
      WorldAgeManager._map_stats.world_age_id = "age_hope";
      pOverrideTime = true;
    }
    WorldAgeAsset hope = AssetManager.era_library.get(WorldAgeManager._map_stats.world_age_id);
    if (hope == null)
    {
      hope = WorldAgeLibrary.hope;
      pOverrideTime = true;
    }
    this.setCurrentAge(hope, pOverrideTime);
  }

  public void update(float pElapsed)
  {
    if (!World.world.isPaused() && !this.isPaused())
    {
      WorldAgeManager._map_stats.current_age_progress += pElapsed / (WorldAgeManager._map_stats.current_world_ages_duration / WorldAgeManager._map_stats.world_ages_speed_multiplier);
      if ((double) WorldAgeManager._map_stats.current_age_progress >= 1.0)
        this.startNextAge();
    }
    this.updateEffects(pElapsed);
  }

  public float getTimeTillNextAge()
  {
    return WorldAgeManager._map_stats.current_world_ages_duration * (1f - WorldAgeManager._map_stats.current_age_progress);
  }

  public bool isPaused() => WorldAgeManager._map_stats.is_world_ages_paused;

  public void togglePlay(bool pState) => WorldAgeManager._map_stats.is_world_ages_paused = !pState;

  public void setAgesSpeedMultiplier(float pMultiplier)
  {
    WorldAgeManager._map_stats.world_ages_speed_multiplier = pMultiplier;
  }

  public void debugEndAge() => WorldAgeManager._map_stats.current_world_ages_duration = 5f;

  public void startNextAge(float pStartProgress = 0.0f)
  {
    this.setCurrentSlotIndex(this.getNextSlotIndex(), pStartProgress);
  }

  public void setCurrentSlotIndex(int pIndex, float pStartProgress = 1f)
  {
    WorldAgeManager._map_stats.world_age_slot_index = pIndex;
    WorldAgeManager._map_stats.current_age_progress = pStartProgress;
    this.setCurrentAge(this.checkAge(this.getAgeFromSlot(pIndex)));
  }

  private void updateEffects(float pElapsed)
  {
    if (Object.op_Equality((Object) this._effects, (Object) null))
    {
      this._effects = WorldAgeEffects.instance;
    }
    else
    {
      this._effects.update(World.world.delta_time);
      if (World.world.isPaused() || this._current_age == null)
        return;
      if (this._current_age.overlay_darkness)
      {
        float num = this.NIGHT_MAX * ((float) PlayerConfig.getIntValue("age_night_effect") / 100f);
        this._night_multiplier += pElapsed * this.NIGHT_SPEED;
        if ((double) this._night_multiplier > (double) num)
          this._night_multiplier = num;
      }
      else
      {
        this._night_multiplier -= pElapsed * this.NIGHT_SPEED;
        if ((double) this._night_multiplier < 0.0)
          this._night_multiplier = 0.0f;
      }
      if (this._current_age.special_effect_action == null)
        return;
      this._timer_special_action -= pElapsed;
      if ((double) this._timer_special_action > 0.0)
        return;
      this._current_age.special_effect_action();
      this._timer_special_action = this._current_age.special_effect_interval;
    }
  }

  private void calcNextEraTime(WorldAgeAsset pAsset, bool pForceMax = false)
  {
    int num = (int) ((double) Randy.randomInt(pAsset.years_min, pAsset.years_max) * 12.0 * 5.0);
    if (pForceMax)
      num = (int) ((double) pAsset.years_max * 12.0 * 5.0);
    WorldAgeManager._map_stats.current_world_ages_duration = (float) num;
  }

  private void setCurrentAge(WorldAgeAsset pAsset, bool pOverrideTime = true)
  {
    pAsset = this.checkAge(pAsset);
    WorldAgeManager._map_stats.world_age_id = pAsset.id;
    WorldAgeManager._map_stats.world_age_started_at = World.world.getCurWorldTime();
    if (this._current_age != pAsset)
      WorldAgeManager._map_stats.same_world_age_started_at = World.world.getCurWorldTime();
    if (pOverrideTime)
      this.calcNextEraTime(pAsset);
    this._current_age = pAsset;
    WorldBehaviourClouds.setEra(pAsset);
    List<Actor> simpleList = World.world.units.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
      simpleList[index].setStatsDirty();
    this._timer_special_action = pAsset.special_effect_interval;
  }

  public void clear() => this._current_age = (WorldAgeAsset) null;

  public void prepare()
  {
    if (this._current_age != null)
      return;
    this.setCurrentAge(WorldAgeLibrary.hope);
  }

  public bool isWinter() => this._current_age != null && this._current_age.flag_winter;

  public bool isNight() => this._current_age != null && this._current_age.flag_night;

  public bool isChaosAge() => this._current_age != null && this._current_age.flag_chaos;

  public bool isLightAge() => this._current_age != null && this._current_age.flag_light_age;

  public bool isCurrentAge(WorldAgeAsset pAgeAsset) => this._current_age == pAgeAsset;

  private WorldAgeAsset checkAge(WorldAgeAsset pAsset)
  {
    if (pAsset.id == "age_unknown")
      pAsset = AssetManager.era_library.list_only_normal.GetRandom<WorldAgeAsset>();
    return pAsset;
  }

  public int calculateMoonsLeft()
  {
    return (int) (((double) WorldAgeManager._map_stats.current_world_ages_duration * (1.0 - (double) WorldAgeManager._map_stats.current_age_progress) / 5.0 + 1.0) / (double) WorldAgeManager._map_stats.world_ages_speed_multiplier);
  }

  public void setDefaultAges()
  {
    for (int key = 1; key < 9; ++key)
    {
      using (ListPool<WorldAgeAsset> list = new ListPool<WorldAgeAsset>((ICollection<WorldAgeAsset>) AssetManager.era_library.pool_by_slots[key]))
      {
        WorldAgeAsset random = list.GetRandom<WorldAgeAsset>();
        if (random.link_default_slots)
        {
          bool flag = false;
          foreach (int defaultSlot in random.default_slots)
          {
            if (defaultSlot != key && this.getAgeFromSlot(defaultSlot - 1) == random)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            list.Remove(random);
            random = list.GetRandom<WorldAgeAsset>();
          }
        }
        this.setAgeToSlot(random, key - 1);
      }
    }
    this.setCurrentSlotIndex(0, 0.0f);
  }
}
