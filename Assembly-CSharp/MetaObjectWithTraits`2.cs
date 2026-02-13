// Decompiled with JetBrains decompiler
// Type: MetaObjectWithTraits`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MetaObjectWithTraits<TData, TBaseTrait> : MetaObject<TData>, ITraitsOwner<TBaseTrait>
  where TData : MetaObjectData
  where TBaseTrait : BaseTrait<TBaseTrait>
{
  private readonly HashSet<TBaseTrait> _traits = new HashSet<TBaseTrait>();
  public readonly BaseStats base_stats = new BaseStats();
  public readonly BaseStats base_stats_meta = new BaseStats();
  private ActorAsset _species_asset;
  public readonly List<BaseAugmentationAsset> all_actions_actor_special_effect = new List<BaseAugmentationAsset>();
  public AttackAction all_actions_actor_attack_target;
  public GetHitAction all_actions_actor_get_hit;
  public WorldAction all_actions_actor_death;
  public WorldAction all_actions_actor_growth;
  public WorldAction all_actions_actor_birth;
  public readonly List<DecisionAsset> decisions_assets = new List<DecisionAsset>();
  public readonly CombatActionHolder combat_actions = new CombatActionHolder();
  public readonly SpellHolder spells = new SpellHolder();

  public override void loadData(TData pData)
  {
    base.loadData(pData);
    this.loadTraits();
  }

  private void resetStatsAndCallbacks()
  {
    this.all_actions_actor_death = (WorldAction) null;
    this.all_actions_actor_growth = (WorldAction) null;
    this.all_actions_actor_birth = (WorldAction) null;
    this.all_actions_actor_attack_target = (AttackAction) null;
    this.all_actions_actor_get_hit = (GetHitAction) null;
    this.all_actions_actor_special_effect.Clear();
    this.base_stats.clear();
    this.base_stats_meta.clear();
    this.decisions_assets.Clear();
    this.combat_actions.reset();
    this.spells.reset();
  }

  public void forceRecalcBaseStats() => this.recalcBaseStats();

  protected virtual void recalcBaseStats()
  {
    this.resetStatsAndCallbacks();
    foreach (TBaseTrait trait in this._traits)
    {
      this.base_stats.mergeStats(trait.base_stats);
      this.base_stats_meta.mergeStats(trait.base_stats_meta);
      this.all_actions_actor_death += trait.action_death;
      this.all_actions_actor_growth += trait.action_growth;
      this.all_actions_actor_birth += trait.action_birth;
      this.all_actions_actor_attack_target += trait.action_attack_target;
      this.all_actions_actor_get_hit += trait.action_get_hit;
      if (trait.action_special_effect != null)
        this.all_actions_actor_special_effect.Add((BaseAugmentationAsset) trait);
      if (trait.hasDecisions())
        this.decisions_assets.AddRange((IEnumerable<DecisionAsset>) trait.decisions_assets);
      if (trait.hasCombatActions())
        this.combat_actions.mergeWith(trait.combat_actions);
      if (trait.hasSpells())
      {
        this.spells.mergeWith((IReadOnlyList<SpellAsset>) trait.spells);
        foreach (SpellAsset spell in trait.spells)
        {
          if (spell.hasDecisions())
            this.decisions_assets.AddRange((IEnumerable<DecisionAsset>) spell.decisions_assets);
        }
      }
    }
    this.setUnitStatsDirty();
  }

  private void setUnitStatsDirty()
  {
    List<Actor> units = this.units;
    int count = units.Count;
    for (int index = 0; index < count; ++index)
      units[index].setStatsDirty();
  }

  private void loadTraits()
  {
    if (this.saved_traits == null)
      return;
    this.fillTraitAssetsFromStringList(this.saved_traits);
    foreach (TBaseTrait trait in this._traits)
    {
      WorldActionTrait augmentationLoad = trait.action_on_augmentation_load;
      if (augmentationLoad != null)
      {
        int num = augmentationLoad((NanoObject) this, (BaseAugmentationAsset) trait) ? 1 : 0;
      }
    }
  }

  protected void fillTraitAssetsFromStringList(List<string> pList)
  {
    foreach (string pID in pList.LoopRandom<string>())
    {
      TBaseTrait pTrait = this.trait_library.get(pID);
      if ((object) pTrait != null && !this.hasOppositeTrait(pTrait))
        this._traits.Add(pTrait);
    }
    this.recalcBaseStats();
  }

  protected override void generateNewMetaObject()
  {
    base.generateNewMetaObject();
    if (this.default_traits == null)
      return;
    this.fillTraitAssetsFromStringList(this.default_traits);
  }

  protected override void generateNewMetaObject(bool pAddDefaultTraits)
  {
    base.generateNewMetaObject();
    if (!(this.default_traits != null & pAddDefaultTraits))
      return;
    this.fillTraitAssetsFromStringList(this.default_traits);
  }

  public List<string> getTraitsAsStrings()
  {
    return Toolbox.getListForSave<TBaseTrait>((IReadOnlyCollection<TBaseTrait>) this._traits);
  }

  public string getTraitsAsLocalizedString()
  {
    string asLocalizedString = "";
    foreach (TBaseTrait trait in this._traits)
      asLocalizedString = $"{asLocalizedString}{trait.getTranslatedName()}, ";
    return asLocalizedString;
  }

  public void copyTraits(IReadOnlyCollection<TBaseTrait> pTraitsToCopy)
  {
    foreach (TBaseTrait pTrait in (IEnumerable<TBaseTrait>) pTraitsToCopy)
    {
      if (!this.hasOppositeTrait(pTrait))
        this._traits.Add(pTrait);
    }
    this.recalcBaseStats();
  }

  protected void clearTraits()
  {
    if (this._traits.Count == 0)
      return;
    this._traits.Clear();
  }

  public IReadOnlyCollection<TBaseTrait> getTraits()
  {
    return (IReadOnlyCollection<TBaseTrait>) this._traits;
  }

  public void sortTraits(IReadOnlyCollection<TBaseTrait> pTraits)
  {
    if (!this._traits.SetEquals((IEnumerable<TBaseTrait>) pTraits))
      return;
    this._traits.Clear();
    foreach (TBaseTrait pTrait in (IEnumerable<TBaseTrait>) pTraits)
      this._traits.Add(pTrait);
  }

  public virtual void traitModifiedEvent()
  {
  }

  public override void triggerOnRemoveObject()
  {
    base.triggerOnRemoveObject();
    foreach (TBaseTrait trait in this._traits)
    {
      WorldActionTrait actionOnObjectRemove = trait.action_on_object_remove;
      if (actionOnObjectRemove != null)
      {
        int num = actionOnObjectRemove((NanoObject) this, (BaseAugmentationAsset) trait) ? 1 : 0;
      }
    }
  }

  public void removeTrait(string pTraitID) => this.removeTrait(this.trait_library.get(pTraitID));

  public bool hasTrait(string pTrait) => this.hasTrait(this.trait_library.get(pTrait));

  public bool hasMetaTag(string pTag) => this.base_stats_meta.hasTag(pTag);

  public bool hasTraits() => this._traits.Count > 0;

  public bool hasTrait(TBaseTrait pTrait) => this._traits.Contains(pTrait);

  public void removeTraits(ListPool<string> pTraits)
  {
    // ISSUE: unable to decompile the method.
  }

  public virtual bool removeTrait(TBaseTrait pTrait)
  {
    int num1 = this._traits.Remove(pTrait) ? 1 : 0;
    if (num1 == 0)
      return num1 != 0;
    WorldActionTrait augmentationRemove = pTrait.action_on_augmentation_remove;
    if (augmentationRemove != null)
    {
      int num2 = augmentationRemove((NanoObject) this, (BaseAugmentationAsset) pTrait) ? 1 : 0;
    }
    this.recalcBaseStats();
    return num1 != 0;
  }

  private void removeOppositeTraits(TBaseTrait pTrait)
  {
    if (!pTrait.hasOppositeTraits<TBaseTrait>())
      return;
    foreach (TBaseTrait oppositeTrait in pTrait.opposite_traits)
      this.removeTrait(oppositeTrait);
  }

  public virtual bool addTrait(string pTraitID, bool pRemoveOpposites = false)
  {
    TBaseTrait pTrait = this.trait_library.get(pTraitID);
    return (object) pTrait != null && this.addTrait(pTrait, pRemoveOpposites);
  }

  public virtual bool addTrait(TBaseTrait pTrait, bool pRemoveOpposites = false)
  {
    if (this.hasTrait(pTrait))
      return false;
    if (pRemoveOpposites)
      this.removeOppositeTraits(pTrait);
    if (this.hasOppositeTrait(pTrait))
      return false;
    this._traits.Add(pTrait);
    WorldActionTrait onAugmentationAdd = pTrait.action_on_augmentation_add;
    if (onAugmentationAdd != null)
    {
      int num = onAugmentationAdd((NanoObject) this, (BaseAugmentationAsset) pTrait) ? 1 : 0;
    }
    this.recalcBaseStats();
    return true;
  }

  public override Sprite getTopicSprite()
  {
    return this._traits.Count == 0 ? (Sprite) null : this._traits.GetRandom<TBaseTrait>().getSprite();
  }

  internal bool hasOppositeTrait(TBaseTrait pTrait)
  {
    return pTrait.hasOppositeTrait<TBaseTrait>(this._traits);
  }

  protected virtual AssetLibrary<TBaseTrait> trait_library
  {
    get => throw new NotImplementedException(this.GetType().Name);
  }

  protected virtual List<string> default_traits => (List<string>) null;

  protected virtual List<string> saved_traits => (List<string>) null;

  protected virtual string species_id => "human";

  public override ActorAsset getActorAsset()
  {
    if (this._species_asset == null)
    {
      string speciesId = this.species_id;
      this._species_asset = AssetManager.actor_library.get(speciesId);
    }
    return this._species_asset;
  }

  public bool isSameActorAsset(ActorAsset pAsset) => this.getActorAsset() == pAsset;

  public void addRandomTraitFromBiome<T>(
    WorldTile pTile,
    List<string> pTraitList,
    AssetLibrary<T> pTraitLibrary)
    where T : BaseTrait<TBaseTrait>
  {
    if (!pTile.Type.is_biome || pTraitList == null || pTraitList.Count == 0)
      return;
    int count = pTraitList.Count;
    for (int index = 0; index < count; ++index)
    {
      if (!Randy.randomBool())
      {
        string random = pTraitList.GetRandom<string>();
        this.addTrait((TBaseTrait) pTraitLibrary.get(random), true);
      }
    }
  }

  public void addTraitFromBiome<T>(
    WorldTile pTile,
    List<string> pTraitList,
    AssetLibrary<T> pTraitLibrary)
    where T : BaseTrait<TBaseTrait>
  {
    if (!pTile.Type.is_biome || pTraitList == null || pTraitList.Count == 0)
      return;
    for (int index = 0; index < pTraitList.Count; ++index)
      this.addTrait((TBaseTrait) pTraitLibrary.get(pTraitList[index]), true);
  }

  public TBaseTrait getTraitForBook()
  {
    IReadOnlyCollection<TBaseTrait> traits = this.getTraits();
    using (ListPool<TBaseTrait> list = new ListPool<TBaseTrait>(traits.Count))
    {
      foreach (TBaseTrait baseTrait in (IEnumerable<TBaseTrait>) traits)
      {
        if (baseTrait.can_be_in_book)
          list.Add(baseTrait);
      }
      return list.Count == 0 ? default (TBaseTrait) : list.GetRandom<TBaseTrait>();
    }
  }

  public override void Dispose()
  {
    this._species_asset = (ActorAsset) null;
    this.clearTraits();
    this.resetStatsAndCallbacks();
    base.Dispose();
  }
}
