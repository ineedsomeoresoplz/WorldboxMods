// Decompiled with JetBrains decompiler
// Type: CitizenJobAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class CitizenJobAsset : Asset
{
  public string path_icon;
  public int priority;
  public int priority_no_food;
  [DefaultValue(true)]
  public bool common_job = true;
  [DefaultValue(true)]
  public bool ok_for_king = true;
  [DefaultValue(true)]
  public bool ok_for_leader = true;
  public bool only_leaders;
  public CitizenJobCondition should_be_assigned;
  public string unit_job_default;
  public DebugOption debug_option;
}
