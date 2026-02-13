// Decompiled with JetBrains decompiler
// Type: BookData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class BookData : BaseSystemData
{
  public string book_type;
  public string path_cover;
  public string path_icon;
  public string author_name;
  [DefaultValue(-1)]
  public long author_id = -1;
  public string author_clan_name;
  [DefaultValue(-1)]
  public long author_clan_id = -1;
  public string author_kingdom_name;
  [DefaultValue(-1)]
  public long author_kingdom_id = -1;
  public string author_city_name;
  [DefaultValue(-1)]
  public long author_city_id = -1;
  [DefaultValue(-1)]
  public long language_id = -1;
  public string language_name;
  [DefaultValue(-1)]
  public long culture_id = -1;
  public string culture_name;
  [DefaultValue(-1)]
  public long religion_id = -1;
  public string religion_name;
  public int times_read;
  public double timestamp_read_last_time;
  public string trait_id_actor = string.Empty;
  public string trait_id_language = string.Empty;
  public string trait_id_culture = string.Empty;
  public string trait_id_religion = string.Empty;
  [DefaultValue(-1)]
  public long building_id = -1;
}
