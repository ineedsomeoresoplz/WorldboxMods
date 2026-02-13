// Decompiled with JetBrains decompiler
// Type: LanguageData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
public class LanguageData : MetaObjectData
{
  public int banner_background_id;
  public int banner_icon_id;
  public int books_written;
  public string creator_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_id = -1;
  public string creator_subspecies_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_subspecies_id = -1;
  public string creator_species_id = string.Empty;
  [DefaultValue(-1)]
  public long creator_kingdom_id = -1;
  public string creator_kingdom_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_clan_id = -1;
  public string creator_clan_name = string.Empty;
  [DefaultValue(-1)]
  public long creator_city_id = -1;
  public string creator_city_name = string.Empty;
  public List<string> saved_traits;
  public int speakers_new;
  public int speakers_converted;
  public int speakers_lost;
  public LanguageStructure structure;
}
