// Decompiled with JetBrains decompiler
// Type: WorldLogMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using SQLite;
using System;
using UnityEngine;

#nullable disable
[Preserve]
[Skip]
[Serializable]
public class WorldLogMessage
{
  [NotNull]
  public string asset_id { get; set; }

  [NotNull]
  public int timestamp { get; set; }

  public string special1 { get; set; }

  public string special2 { get; set; }

  public string special3 { get; set; }

  public string color_special_1 { get; set; }

  public string color_special_2 { get; set; }

  public string color_special_3 { get; set; }

  public long unit_id { get; set; } = -1;

  public long kingdom_id { get; set; } = -1;

  public int? x { get; set; }

  public int? y { get; set; }

  [Ignore]
  public Vector2 location
  {
    set
    {
      this.x = new int?((int) value.x);
      this.y = new int?((int) value.y);
    }
    get
    {
      return !this.x.HasValue || !this.y.HasValue ? new Vector2(-1f, -1f) : new Vector2((float) this.x.Value, (float) this.y.Value);
    }
  }

  [Ignore]
  public Actor unit
  {
    set => this.unit_id = value != null ? value.getID() : -1L;
    get => World.world.units.get(this.unit_id);
  }

  [Ignore]
  public Kingdom kingdom
  {
    set => this.kingdom_id = value != null ? value.getID() : -1L;
    get => World.world.kingdoms.get(this.kingdom_id);
  }

  [Ignore]
  public Color color_special1
  {
    set => this.color_special_1 = Toolbox.colorToHex(Color32.op_Implicit(value), false);
  }

  [Ignore]
  public Color color_special2
  {
    set => this.color_special_2 = Toolbox.colorToHex(Color32.op_Implicit(value), false);
  }

  [Ignore]
  public Color color_special3
  {
    set => this.color_special_3 = Toolbox.colorToHex(Color32.op_Implicit(value), false);
  }

  public WorldLogMessage()
  {
  }

  public WorldLogMessage(
    WorldLogAsset pAsset,
    string pSpecial1 = null,
    string pSpecial2 = null,
    string pSpecial3 = null)
  {
    this.asset_id = pAsset.id;
    this.special1 = pSpecial1;
    this.special2 = pSpecial2;
    this.special3 = pSpecial3;
    this.x = new int?();
    this.y = new int?();
    this.unit_id = -1L;
    this.color_special_1 = (string) null;
    this.color_special_2 = (string) null;
    this.color_special_3 = (string) null;
    this.timestamp = (int) World.world.getCurWorldTime();
    this.unit = (Actor) null;
  }
}
