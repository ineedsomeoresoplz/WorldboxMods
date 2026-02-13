// Decompiled with JetBrains decompiler
// Type: WarData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class WarData : MetaObjectData
{
  public List<long> list_attackers = new List<long>();
  public List<long> list_defenders = new List<long>();
  public List<long> past_attackers = new List<long>();
  public List<long> past_defenders = new List<long>();
  public List<long> died_attackers = new List<long>();
  public List<long> died_defenders = new List<long>();
  [DefaultValue(WarWinner.Nobody)]
  public WarWinner winner;

  [DefaultValue(-1)]
  public long main_attacker { get; set; } = -1;

  [DefaultValue(-1)]
  public long main_defender { get; set; } = -1;

  public int dead_attackers { get; set; }

  public int dead_defenders { get; set; }

  public string started_by_actor_name { get; set; } = string.Empty;

  [DefaultValue(-1)]
  public long started_by_actor_id { get; set; } = -1;

  public string started_by_kingdom_name { get; set; } = string.Empty;

  [DefaultValue(-1)]
  public long started_by_kingdom_id { get; set; } = -1;

  [DefaultValue("normal")]
  public string war_type { get; set; } = "normal";

  public override void Dispose()
  {
    this.list_attackers.Clear();
    this.list_defenders.Clear();
    base.Dispose();
  }
}
