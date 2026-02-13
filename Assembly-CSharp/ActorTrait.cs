// Decompiled with JetBrains decompiler
// Type: ActorTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class ActorTrait : BaseTrait<ActorTrait>
{
  public int rate_birth;
  public int rate_acquire_grow_up;
  public bool acquire_grow_up_sapient_only;
  public int rate_inherit;
  public bool is_mutation_box_allowed = true;
  public int same_trait_mod;
  public int opposite_trait_mod;
  public bool only_active_on_era_flag;
  public bool era_active_moon;
  public bool era_active_night;
  [DefaultValue(TraitType.Other)]
  public TraitType type = TraitType.Other;
  public bool remove_for_zombie_actor_asset;
  public bool can_be_cured;
  public bool affects_mind;
  public bool is_kingdom_affected;
  [DefaultValue("")]
  public string forced_kingdom = string.Empty;
  public bool can_be_removed_by_divine_light;
  public bool can_be_removed_by_accelerated_healing;
  public float likeability;
  public bool in_training_dummy_combat_pot;

  protected override HashSet<string> progress_elements
  {
    get => this._progress_data?.unlocked_traits_actor;
  }

  public override string typed_id => "trait";

  public override BaseCategoryAsset getGroup()
  {
    return (BaseCategoryAsset) AssetManager.trait_groups.get(this.group_id);
  }

  public int getRate(string pGroup)
  {
    return pGroup == "body" ? this.rate_birth : this.rate_acquire_grow_up;
  }

  public Kingdom getForcedKingdom()
  {
    if (!(this.forced_kingdom == string.Empty))
      return World.world.kingdoms_wild.get(this.forced_kingdom);
    Debug.LogError((object) ("Shouldn't call this from a trait that doesn't have a forced kingdom! " + this.id));
    return (Kingdom) null;
  }

  protected override IEnumerable<ITraitsOwner<ActorTrait>> getRelatedMetaList()
  {
    return (IEnumerable<ITraitsOwner<ActorTrait>>) World.world.units;
  }

  public override string getCountRows() => this.getCountRowsByCategories();

  protected override bool isSapient(ITraitsOwner<ActorTrait> pObject)
  {
    return ((Actor) pObject).isSapient();
  }
}
