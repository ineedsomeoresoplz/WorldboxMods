// Decompiled with JetBrains decompiler
// Type: ArmyData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
public class ArmyData : MetaObjectData
{
  [DefaultValue(-1)]
  public long id_city = -1;
  [DefaultValue(-1)]
  public long id_captain = -1;
  [DefaultValue(-1)]
  public long id_kingdom = -1;
  public List<LeaderEntry> past_captains;

  public override void Dispose()
  {
    base.Dispose();
    this.past_captains?.Clear();
    this.past_captains = (List<LeaderEntry>) null;
  }
}
