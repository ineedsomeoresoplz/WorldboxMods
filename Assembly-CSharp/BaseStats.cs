// Decompiled with JetBrains decompiler
// Type: BaseStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[Serializable]
public class BaseStats : ICloneable
{
  [JsonProperty]
  private List<BaseStatsContainer> _stats_list = new List<BaseStatsContainer>();
  private Dictionary<string, BaseStatsContainer> _stats_dict = new Dictionary<string, BaseStatsContainer>();
  private List<BaseStatsContainer> _multipliers_list;
  [JsonProperty]
  private HashSet<string> _tags;

  private void set(string pID, float pAmount)
  {
    BaseStatAsset baseStatAsset = AssetManager.base_stats_library.get(pID);
    if (baseStatAsset.ignore)
      return;
    if ((double) pAmount == 0.0 && !baseStatAsset.normalize)
    {
      BaseStatsContainer baseStatsContainer;
      if (!this._stats_dict.TryGetValue(pID, out baseStatsContainer))
        return;
      if (baseStatAsset.multiplier)
        this._multipliers_list?.Remove(baseStatsContainer);
      this._stats_list.Remove(baseStatsContainer);
      this._stats_dict.Remove(pID);
    }
    else
    {
      BaseStatsContainer baseStatsContainer;
      if (!this._stats_dict.TryGetValue(pID, out baseStatsContainer))
      {
        baseStatsContainer = new BaseStatsContainer();
        baseStatsContainer.value = pAmount;
        baseStatsContainer.id = pID;
        this._stats_dict[pID] = baseStatsContainer;
        this._stats_list.Add(baseStatsContainer);
        if (!baseStatAsset.multiplier)
          return;
        if (this._multipliers_list == null)
          this._multipliers_list = new List<BaseStatsContainer>();
        this._multipliers_list.Add(baseStatsContainer);
      }
      else
        this._stats_dict[pID].value = pAmount;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public List<BaseStatsContainer> getList() => this._stats_list;

  public void checkStatName(string pID)
  {
    if (!Config.game_loaded || AssetManager.base_stats_library.get(pID) != null)
      return;
    Debug.LogError((object) ("base_stats_library.get() - no asset with id: " + pID));
    Debug.LogError((object) "Only call stats with S.id, never with pure strings to avoid typos");
  }

  public float get(string pID)
  {
    BaseStatsContainer baseStatsContainer;
    return this._stats_dict.TryGetValue(pID, out baseStatsContainer) ? baseStatsContainer.value : 0.0f;
  }

  public bool hasStat(string pID) => this._stats_dict.ContainsKey(pID);

  public BaseStatsContainer getContainer(string pID)
  {
    BaseStatsContainer container = (BaseStatsContainer) null;
    this._stats_dict.TryGetValue(pID, out container);
    return container;
  }

  internal void mergeStats(BaseStats pStats, float pMultiplier = 1f)
  {
    for (int index = 0; index < pStats._stats_list.Count; ++index)
    {
      BaseStatsContainer stats = pStats._stats_list[index];
      this[stats.id] += stats.value * pMultiplier;
    }
    if (pStats._tags == null)
      return;
    if (this._tags == null)
      this._tags = new HashSet<string>((IEnumerable<string>) pStats._tags);
    else
      this._tags.UnionWith((IEnumerable<string>) pStats._tags);
  }

  public void checkMultipliers()
  {
    if (this._multipliers_list == null)
      return;
    for (int index = 0; index < this._multipliers_list.Count; ++index)
    {
      BaseStatsContainer multipliers = this._multipliers_list[index];
      BaseStatsContainer container = this.getContainer(multipliers.asset.main_stat_to_multiply);
      if (container != null)
        container.value += container.value * multipliers.value;
    }
  }

  public bool hasTag(string pTag)
  {
    HashSet<string> tags = this._tags;
    // ISSUE: explicit non-virtual call
    return tags != null && __nonvirtual (tags.Contains(pTag));
  }

  public bool hasTags(string[] pTags)
  {
    HashSet<string> tags = this._tags;
    // ISSUE: explicit non-virtual call
    return tags != null && __nonvirtual (tags.Overlaps((IEnumerable<string>) pTags));
  }

  public void normalize()
  {
    for (int index = 0; index < this._stats_list.Count; ++index)
      this._stats_list[index].normalize();
  }

  internal void clear()
  {
    this._multipliers_list?.Clear();
    this._stats_list.Clear();
    this._stats_dict.Clear();
    this._tags?.Clear();
  }

  public void reset() => this.clear();

  public float this[string pKey]
  {
    get => this.get(pKey);
    set => this.set(pKey, value);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void addTag(string pTag)
  {
    if (this._tags == null)
      this._tags = new HashSet<string>();
    this._tags.Add(pTag);
  }

  public bool hasTags()
  {
    HashSet<string> tags = this._tags;
    // ISSUE: explicit non-virtual call
    return tags != null && __nonvirtual (tags.Count) > 0;
  }

  public bool hasStats()
  {
    List<BaseStatsContainer> statsList = this._stats_list;
    // ISSUE: explicit non-virtual call
    return statsList != null && __nonvirtual (statsList.Count) > 0;
  }

  public bool ShouldSerialize_tags() => this.hasTags();

  public bool ShouldSerialize_stats_list() => this.hasStats();

  public void addCombatAction(string pCombatAction)
  {
  }

  public object Clone()
  {
    BaseStats baseStats = new BaseStats();
    baseStats.mergeStats(this);
    return (object) baseStats;
  }
}
