// Decompiled with JetBrains decompiler
// Type: SubspeciesData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
public class SubspeciesData : MetaObjectData
{
  public List<ChromosomeData> saved_chromosome_data;
  public List<string> saved_traits;
  public List<string> saved_actor_birth_traits;
  public string biome_variant = "default_color";
  public int banner_background_id;
  public string species_id = "human";
  [DefaultValue(-1)]
  public long parent_subspecies = -1;
  [DefaultValue(-1)]
  public long evolved_into_subspecies = -1;
  [DefaultValue(0)]
  public int evolved_into_subspecies_next_spawn;
  public double last_evolution_timestamp;
  public int skin_id;
  public int mutation;
  [DefaultValue(1)]
  public int generation = 1;
  [DefaultValue(-1)]
  public long skeleton_form_id = -1;

  public override void Dispose()
  {
    base.Dispose();
    this.saved_chromosome_data?.Clear();
    this.saved_chromosome_data = (List<ChromosomeData>) null;
    this.saved_traits?.Clear();
    this.saved_traits = (List<string>) null;
    this.saved_actor_birth_traits?.Clear();
    this.saved_actor_birth_traits = (List<string>) null;
  }
}
