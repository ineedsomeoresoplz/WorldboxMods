// Decompiled with JetBrains decompiler
// Type: CityData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Scripting;

#nullable disable
[Serializable]
public class CityData : MetaObjectData
{
  public CityEquipment equipment;
  public List<ZoneData> zones = new List<ZoneData>();
  public int total_food_consumed;
  public float timer_supply;
  public float timer_trade;
  public List<LeaderEntry> past_rulers;
  [DefaultValue(0)]
  public int total_leaders;
  public double timestamp_kingdom;

  [JsonProperty]
  public string original_actor_asset { get; set; }

  [DefaultValue(-1)]
  public long kingdomID { get; set; } = -1;

  [DefaultValue(-1)]
  public long leaderID { get; set; } = -1;

  [DefaultValue(-1)]
  public long founder_id { get; set; } = -1;

  public string founder_name { get; set; }

  [DefaultValue(-1)]
  public long last_leader_id { get; set; } = -1;

  [DefaultValue(-1)]
  public long last_kingdom_id { get; set; } = -1;

  [DefaultValue(-1)]
  public long id_culture { get; set; } = -1;

  [DefaultValue(-1)]
  public long id_language { get; set; } = -1;

  [DefaultValue(-1)]
  public long id_religion { get; set; } = -1;

  public long left { get; set; }

  public long joined { get; set; }

  public long moved { get; set; }

  public long migrated { get; set; }

  [Preserve]
  [Obsolete("use .name instead", true)]
  public string cityName
  {
    set
    {
      if (!string.IsNullOrEmpty(this.name) || string.IsNullOrEmpty(value))
        return;
      this.name = value;
    }
  }

  [Preserve]
  [Obsolete("use .id instead", true)]
  public long cityID
  {
    set
    {
      if (!value.hasValue() || this.id.hasValue())
        return;
      this.id = value;
    }
  }

  [Preserve]
  [Obsolete("use .original_actor_asset instead", true)]
  public string race
  {
    set
    {
      if (string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(this.original_actor_asset))
        return;
      this.original_actor_asset = value;
    }
  }

  public override void Dispose()
  {
    base.Dispose();
    this.zones.Clear();
    this.past_rulers?.Clear();
    this.past_rulers = (List<LeaderEntry>) null;
    this.equipment?.Dispose();
    this.equipment = (CityEquipment) null;
  }
}
