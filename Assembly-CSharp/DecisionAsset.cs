// Decompiled with JetBrains decompiler
// Type: DecisionAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[Serializable]
public class DecisionAsset : Asset, ILocalizedAsset
{
  public DecisionAction action_check_launch;
  public float weight = 1f;
  [NonSerialized]
  public bool has_weight_custom;
  public DecisionActionWeight weight_calculate_custom;
  public string task_id;
  public int decision_index;
  public int cooldown;
  public string path_icon;
  public NeuroLayer priority;
  [NonSerialized]
  public int priority_int_cached;
  public bool cooldown_on_launch_failure;
  public bool only_special;
  public bool unique;
  public bool list_civ;
  public bool list_baby;
  public bool list_animal;
  public bool only_adult;
  public bool only_mob;
  public bool only_herd;
  public bool only_sapient;
  public bool only_safe;
  public bool only_hungry;
  public bool city_must_be_safe;
  [NonSerialized]
  public Sprite cached_sprite;

  public virtual Sprite getSprite()
  {
    if (this.cached_sprite == null)
      this.cached_sprite = SpriteTextureLoader.getSprite(this.path_icon);
    return this.cached_sprite;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isPossible(Actor pActor)
  {
    if (this.only_hungry && !pActor.isHungry() || this.only_safe && pActor.isFighting() || this.only_herd && !pActor.asset.follow_herd || this.only_adult && !pActor.isAdult() || this.only_mob && pActor.isKingdomCiv() || this.only_sapient && !pActor.isSapient())
      return false;
    if (this.city_must_be_safe && pActor.inOwnCityBorders())
    {
      ProfessionAsset professionAsset = pActor.profession_asset;
      if ((professionAsset != null ? (professionAsset.can_capture ? 1 : 0) : 0) != 0)
      {
        City city = pActor.current_zone.city;
        if ((city != null ? (city.isInDanger() ? 1 : 0) : 0) != 0)
          return false;
      }
    }
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isPossible(ref DecisionChecks pChecks)
  {
    return (!this.only_hungry || pChecks.is_hungry) && (!this.only_safe || !pChecks.is_fighting) && (!this.only_herd || pChecks.is_herd) && (!this.only_adult || pChecks.is_adult) && (!this.only_mob || !pChecks.is_civ) && (!this.only_sapient || pChecks.is_sapient) && (!this.city_must_be_safe || !pChecks.can_capture_city || !pChecks.city_is_in_danger);
  }

  public string getLocalizedText() => this.getLocaleID().Localize();

  public string getLocaleID()
  {
    string pID = !string.IsNullOrEmpty(this.task_id) ? this.task_id : this.id;
    return AssetManager.tasks_actor.get(pID).getLocaleID();
  }

  public string getFiringRate()
  {
    if (this.cooldown <= 0)
      return "N/A";
    float num = 1f / (float) this.cooldown;
    return (double) num < 0.10000000149011612 ? $"{num:0.000} Hz" : $"{num:0.00} Hz";
  }
}
