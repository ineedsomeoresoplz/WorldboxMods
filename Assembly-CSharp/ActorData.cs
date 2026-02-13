// Decompiled with JetBrains decompiler
// Type: ActorData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
[Serializable]
public class ActorData : BaseObjectData
{
  public List<long> saved_items;
  [DefaultValue(null)]
  public ActorBag inventory;
  public int x;
  public int y;
  [DefaultValue(-1)]
  public long transportID = -1;
  [DefaultValue(-1)]
  public long homeBuildingID = -1;
  [DefaultValue(UnitProfession.Nothing)]
  public UnitProfession profession;
  [JsonProperty]
  public List<string> saved_traits;

  [DefaultValue(-1)]
  public long cityID { get; set; } = -1;

  [DefaultValue(-1)]
  public long civ_kingdom_id { get; set; } = -1;

  [Preserve]
  [Obsolete("use .id instead", true)]
  public long actorID
  {
    set
    {
      if (!value.hasValue() || this.id.hasValue())
        return;
      this.id = value;
    }
  }

  [Preserve]
  [Obsolete("use .name instead", true)]
  public string firstName
  {
    set
    {
      if (string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(this.name))
        return;
      this.name = value;
    }
  }

  [Preserve]
  [Obsolete("use .asset_id instead", true)]
  public string statsID
  {
    set
    {
      if (string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(this.asset_id))
        return;
      this.asset_id = value;
    }
  }

  [DefaultValue("")]
  public string favorite_food { get; set; } = "";

  [JsonProperty]
  [DefaultValue(ActorSex.Male)]
  public ActorSex sex { get; set; }

  [DefaultValue(-1)]
  public int head { get; set; } = -1;

  [DefaultValue(-1)]
  public long culture { get; set; } = -1;

  [DefaultValue(-1)]
  public long clan { get; set; } = -1;

  [DefaultValue(-1)]
  public long subspecies { get; set; } = -1;

  [DefaultValue(-1)]
  public long language { get; set; } = -1;

  [DefaultValue(-1)]
  public long plot { get; set; } = -1;

  [DefaultValue(-1)]
  public long religion { get; set; } = -1;

  [DefaultValue(-1)]
  public long family { get; set; } = -1;

  [DefaultValue(-1)]
  public long army { get; set; } = -1;

  [DefaultValue(-1)]
  public long lover { get; set; } = -1;

  [DefaultValue(-1)]
  public long best_friend_id { get; set; } = -1;

  [DefaultValue(0)]
  public int renown { get; set; }

  [JsonProperty]
  public string asset_id { get; set; }

  [DefaultValue(0)]
  public int kills { get; set; }

  [DefaultValue(0)]
  public int food_consumed { get; set; }

  [DefaultValue(1)]
  public int age_overgrowth { get; set; } = 1;

  [DefaultValue(0)]
  public int births { get; set; }

  [DefaultValue(-1)]
  public long parent_id_1 { get; set; } = -1;

  [DefaultValue(-1)]
  public long parent_id_2 { get; set; } = -1;

  [DefaultValue(-1)]
  public long ancestor_family { get; set; } = -1;

  [DefaultValue(1)]
  public int generation { get; set; } = 1;

  [DefaultValue(0)]
  public int pollen { get; set; }

  [DefaultValue(0)]
  public int loot { get; set; }

  [DefaultValue(0)]
  public int money { get; set; }

  [JsonProperty]
  [DefaultValue(0)]
  public int nutrition { get; set; }

  [DefaultValue(0)]
  public int happiness { get; set; }

  [DefaultValue(0)]
  public int stamina { get; set; }

  [DefaultValue(0)]
  public int mana { get; set; }

  [DefaultValue(1)]
  public int level { get; set; } = 1;

  [DefaultValue(0)]
  public int experience { get; set; }

  [JsonProperty]
  [DefaultValue(0)]
  public int phenotype_shade { get; set; }

  [JsonProperty]
  [DefaultValue(0)]
  public int phenotype_index { get; set; }

  [Preserve]
  [Obsolete("use .nutrition instead", true)]
  public int hunger
  {
    set
    {
      if (this.nutrition != 0 || value == 0)
        return;
      this.nutrition = value;
    }
  }

  [Preserve]
  [Obsolete("use .saved_traits instead", true)]
  public List<string> traits
  {
    set
    {
      if (value == null || value.Count == 0)
        return;
      List<string> savedTraits = this.saved_traits;
      if ((savedTraits != null ? (__nonvirtual (savedTraits.Count) > 0 ? 1 : 0) : 0) != 0)
        return;
      this.saved_traits = value;
    }
  }

  [Preserve]
  [Obsolete("use .sex instead", true)]
  public ActorGender gender
  {
    set
    {
      if (this.sex != ActorSex.Male)
        return;
      if (value != ActorGender.Male)
      {
        if (value == ActorGender.Female)
          this.sex = ActorSex.Female;
        else
          this.sex = ActorSex.Male;
      }
      else
        this.sex = ActorSex.Male;
    }
  }

  public int getAge() => Date.getYearsSince(this.created_time) + this.age_overgrowth;
}
