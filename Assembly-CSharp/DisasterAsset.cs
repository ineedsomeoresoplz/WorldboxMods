// Decompiled with JetBrains decompiler
// Type: DisasterAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class DisasterAsset : Asset
{
  public DisasterAction action;
  public int rate = 1;
  public float chance = 1f;
  public string world_log;
  public int min_world_population;
  public int min_world_cities;
  public bool premium_only;
  public DisasterType type = DisasterType.Other;
  public string spawn_asset_unit = string.Empty;
  public string spawn_asset_building = string.Empty;
  public int max_existing_units = 20;
  public int units_min = 1;
  public int units_max = 10;
  public HashSet<string> ages_allow = new HashSet<string>();
  public HashSet<string> ages_forbid = new HashSet<string>();
}
