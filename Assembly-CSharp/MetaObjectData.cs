// Decompiled with JetBrains decompiler
// Type: MetaObjectData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System.ComponentModel;

#nullable disable
public class MetaObjectData : BaseSystemData
{
  [JsonProperty]
  public int color_id { get; set; }

  [DefaultValue(-1)]
  public int original_color_id { get; set; } = -1;

  public long total_deaths { get; set; }

  public long total_births { get; set; }

  public long total_kills { get; set; }

  public long deaths_natural { get; set; }

  public long deaths_hunger { get; set; }

  public long deaths_eaten { get; set; }

  public long deaths_plague { get; set; }

  public long deaths_poison { get; set; }

  public long deaths_infection { get; set; }

  public long deaths_tumor { get; set; }

  public long deaths_acid { get; set; }

  public long deaths_fire { get; set; }

  public long deaths_divine { get; set; }

  public long deaths_weapon { get; set; }

  public long deaths_gravity { get; set; }

  public long deaths_drowning { get; set; }

  public long deaths_water { get; set; }

  public long deaths_explosion { get; set; }

  public long deaths_other { get; set; }

  public long metamorphosis { get; set; }

  public long evolutions { get; set; }

  public int renown { get; set; }

  public void setColorID(int pColorID)
  {
    this.color_id = pColorID;
    if (this.original_color_id != -1)
      return;
    this.original_color_id = this.color_id;
  }
}
