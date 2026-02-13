// Decompiled with JetBrains decompiler
// Type: WarTypeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class WarTypeAsset : Asset, IMultiLocalesAsset
{
  public string name_template = "war_conquest";
  public string localized_type = "war_type_conquest";
  public string localized_war_name = "war_name_conquest";
  public string path_icon = "war_conquest";
  public bool kingdom_for_name_attacker = true;
  public bool forced_war;
  public bool alliance_join;
  public bool total_war;
  public bool rebellion;
  public bool can_end_with_plot;

  public IEnumerable<string> getLocaleIDs()
  {
    yield return this.localized_type;
    yield return this.localized_war_name;
  }
}
