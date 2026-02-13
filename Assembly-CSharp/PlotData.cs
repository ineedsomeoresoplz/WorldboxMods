// Decompiled with JetBrains decompiler
// Type: PlotData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.ComponentModel;

#nullable disable
public class PlotData : MetaObjectData
{
  public string plot_type_id;
  public string founder_name;
  [DefaultValue(-1)]
  public long founder_id = -1;
  [DefaultValue(-1)]
  public long id_initiator_actor = -1;
  [DefaultValue(-1)]
  public long id_initiator_city = -1;
  [DefaultValue(-1)]
  public long id_initiator_kingdom = -1;
  [DefaultValue(-1)]
  public long id_target_actor = -1;
  [DefaultValue(-1)]
  public long id_target_city = -1;
  [DefaultValue(-1)]
  public long id_target_kingdom = -1;
  [DefaultValue(-1)]
  public long id_target_alliance = -1;
  [DefaultValue(-1)]
  public long id_target_war = -1;
  public bool forced;
  public float progress_current;

  public override void Dispose() => base.Dispose();
}
