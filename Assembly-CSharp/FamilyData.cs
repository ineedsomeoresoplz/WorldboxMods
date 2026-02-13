// Decompiled with JetBrains decompiler
// Type: FamilyData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.ComponentModel;

#nullable disable
public class FamilyData : MetaObjectData
{
  public int banner_background_id;
  public int banner_frame_id;
  [DefaultValue(-1)]
  public long alpha_id = -1;
  public string founder_actor_name_1;
  public string founder_actor_name_2;
  [DefaultValue(-1)]
  public long main_founder_id_1 = -1;
  [DefaultValue(-1)]
  public long main_founder_id_2 = -1;
  [DefaultValue(-1)]
  public long subspecies_id = -1;
  public string subspecies_name = string.Empty;
  public string species_id = string.Empty;
  [DefaultValue(-1)]
  public long founder_city_id = -1;
  public string founder_city_name = string.Empty;
  [DefaultValue(-1)]
  public long founder_kingdom_id = -1;
  public string founder_kingdom_name = string.Empty;
  [DefaultValue(-1)]
  public long original_family_1 = -1;
  [DefaultValue(-1)]
  public long original_family_2 = -1;
}
