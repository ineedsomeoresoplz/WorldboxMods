// Decompiled with JetBrains decompiler
// Type: NameEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;

#nullable disable
public readonly struct NameEntry
{
  [JsonProperty]
  public readonly int color_id;
  [JsonProperty]
  public readonly string name;
  [JsonProperty]
  public readonly double timestamp;
  [JsonProperty]
  public readonly bool custom;

  public NameEntry(string pName, bool pCustom)
  {
    this.name = pName;
    this.color_id = -1;
    this.timestamp = (double) (int) World.world.getCurWorldTime();
    this.custom = pCustom;
  }

  public NameEntry(string pName, bool pCustom, int pColorId)
  {
    this.name = pName;
    this.color_id = pColorId;
    this.timestamp = (double) (int) World.world.getCurWorldTime();
    this.custom = pCustom;
  }

  public NameEntry(string pName, bool pCustom, double pTimestamp)
  {
    this.name = pName;
    this.color_id = -1;
    this.timestamp = (double) (int) pTimestamp;
    this.custom = pCustom;
  }

  public NameEntry(string pName, bool pCustom, int pColorId, double pTimestamp)
  {
    this.name = pName;
    this.color_id = pColorId;
    this.timestamp = (double) (int) pTimestamp;
    this.custom = pCustom;
  }
}
