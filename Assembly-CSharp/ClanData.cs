// Decompiled with JetBrains decompiler
// Type: ClanData
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
public class ClanData : MetaObjectData
{
  public string motto;
  [DefaultValue(-1)]
  public long chief_id = -1;
  [DefaultValue(-1)]
  public long culture_id = -1;
  public List<LeaderEntry> past_chiefs;
  public List<string> saved_traits;
  public int books_written;
  public int banner_background_id;
  public int banner_icon_id;
  public string founder_actor_name;
  [DefaultValue(-1)]
  public long founder_actor_id = -1;
  public string founder_kingdom_name;
  [DefaultValue(-1)]
  public long founder_kingdom_id = -1;
  public string founder_city_name;
  [DefaultValue(-1)]
  public long founder_city_id = -1;
  public string creator_species_id = string.Empty;
  public string creator_subspecies_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_subspecies_id = -1;

  [JsonProperty]
  public string original_actor_asset { get; set; }

  [Preserve]
  [Obsolete("use .original_actor_asset instead", true)]
  public string race_id
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
    this.saved_traits?.Clear();
    this.saved_traits = (List<string>) null;
    this.past_chiefs?.Clear();
    this.past_chiefs = (List<LeaderEntry>) null;
  }
}
