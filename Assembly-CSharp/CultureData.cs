// Decompiled with JetBrains decompiler
// Type: CultureData
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
public class CultureData : MetaObjectData
{
  public int banner_decor_id;
  public int banner_element_id;
  public string creator_city_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_city_id = -1;
  [DefaultValue(-1)]
  public long creator_id = -1;
  public string creator_name = string.Empty;
  public string creator_species_id = string.Empty;
  public string creator_subspecies_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_subspecies_id = -1;
  [DefaultValue(-1)]
  public long creator_kingdom_id = -1;
  public string creator_kingdom_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_clan_id = -1;
  public string creator_clan_name = string.Empty;
  public List<string> saved_traits;
  public double timestamp_last_written_book;
  public Dictionary<MetaType, string> onomastics;
  [JsonProperty("year")]
  [Preserve]
  [Obsolete("not used anymore", false)]
  public int year_obsolete;
  [DefaultValue("")]
  public string name_template_set = "";
  [DefaultValue(-1)]
  public long parent_culture_id = -1;

  [JsonProperty]
  public string original_actor_asset { get; set; }

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
    this.saved_traits?.Clear();
    this.saved_traits = (List<string>) null;
    this.onomastics?.Clear();
    this.onomastics = (Dictionary<MetaType, string>) null;
    base.Dispose();
  }
}
