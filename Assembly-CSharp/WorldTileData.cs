// Decompiled with JetBrains decompiler
// Type: WorldTileData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class WorldTileData
{
  public string type;
  public int height;
  [DefaultValue(ConwayType.None)]
  public ConwayType conwayType = ConwayType.None;
  [NonSerialized]
  public double fire_timestamp;
  [NonSerialized]
  public bool frozen;
  public readonly int tile_id;

  public WorldTileData(int pTileID)
  {
    this.tile_id = pTileID;
    this.clear();
  }

  internal void clear()
  {
    this.type = (string) null;
    this.height = 0;
    this.conwayType = ConwayType.None;
    this.fire_timestamp = 0.0;
    this.frozen = false;
  }
}
