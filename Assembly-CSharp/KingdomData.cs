// Decompiled with JetBrains decompiler
// Type: KingdomData
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
public class KingdomData : MetaObjectData
{
  [DefaultValue(-1.0)]
  public double timestamp_alliance = -1.0;
  public List<string> saved_traits;
  [DefaultValue(-1.0)]
  public double timestamp_last_war = -1.0;
  [DefaultValue(-1.0)]
  public double timestamp_new_conquest = -1.0;
  [DefaultValue(-1.0)]
  public double timestamp_king_rule = -1.0;
  public float timer_new_king = 10f;
  public List<LeaderEntry> past_rulers;
  [DefaultValue(0)]
  public int total_kings;

  [DefaultValue(-1)]
  public long allianceID { get; set; } = -1;

  [JsonProperty]
  public string original_actor_asset { get; set; }

  [DefaultValue(-1)]
  public long royal_clan_id { get; set; } = -1;

  public string motto { get; set; }

  [DefaultValue(-1)]
  public long id_culture { get; set; } = -1;

  [DefaultValue(-1)]
  public long id_language { get; set; } = -1;

  [DefaultValue(-1)]
  public long id_religion { get; set; } = -1;

  [DefaultValue(-1)]
  public long kingID { get; set; } = -1;

  [DefaultValue(-1)]
  public long capitalID { get; set; } = -1;

  [DefaultValue(-1)]
  public long last_capital_id { get; set; } = -1;

  [DefaultValue(1)]
  public int last_army_id { get; set; } = 1;

  [Preserve]
  [Obsolete("use .color_id instead", true)]
  public int colorId
  {
    set
    {
      if (value == -1 || this.color_id != 0)
        return;
      this.setColorID(value);
    }
  }

  [Preserve]
  [Obsolete("use .original_actor_asset instead", true)]
  public string raceId
  {
    set
    {
      if (string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(this.original_actor_asset))
        return;
      this.original_actor_asset = value;
    }
  }

  [DefaultValue(0)]
  public int banner_background_id { get; set; }

  [DefaultValue(0)]
  public int banner_icon_id { get; set; }

  public long left { get; set; }

  public long joined { get; set; }

  public long moved { get; set; }

  public long migrated { get; set; }

  public override void Dispose()
  {
    base.Dispose();
    this.saved_traits?.Clear();
    this.saved_traits = (List<string>) null;
    this.past_rulers?.Clear();
    this.past_rulers = (List<LeaderEntry>) null;
  }
}
