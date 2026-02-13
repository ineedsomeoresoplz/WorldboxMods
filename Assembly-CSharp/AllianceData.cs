// Decompiled with JetBrains decompiler
// Type: AllianceData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
public class AllianceData : MetaObjectData
{
  public List<long> kingdoms;
  public double timestamp_member_joined;
  [DefaultValue(AllianceType.Normal)]
  public AllianceType alliance_type;

  public string motto { get; set; }

  public int banner_background_id { get; set; }

  public int banner_icon_id { get; set; }

  public string founder_actor_name { get; set; }

  [DefaultValue(-1)]
  public long founder_actor_id { get; set; } = -1;

  public string founder_kingdom_name { get; set; }

  [DefaultValue(-1)]
  public long founder_kingdom_id { get; set; } = -1;
}
