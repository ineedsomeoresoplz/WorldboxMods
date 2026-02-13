// Decompiled with JetBrains decompiler
// Type: WorldTileDataStruct
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public readonly struct WorldTileDataStruct : IEquatable<WorldTileDataStruct>
{
  public readonly string type;
  public readonly int height;
  public readonly ConwayType conwayType;
  public readonly double fire_timestamp;
  public readonly bool frozen;
  public readonly int tile_id;
  public readonly int x;
  public readonly int y;

  public WorldTileDataStruct(
    string pType,
    int pHeight,
    ConwayType pConwayType,
    bool pFire,
    double pFireTimestamp,
    bool pFrozen,
    int pTileID,
    int pX,
    int pY)
  {
    this.type = pType;
    this.height = pHeight;
    this.conwayType = pConwayType;
    this.fire_timestamp = pFireTimestamp;
    this.frozen = pFrozen;
    this.tile_id = pTileID;
    this.x = pX;
    this.y = pY;
  }

  public WorldTileDataStruct(WorldTile pTile, int pTileID)
  {
    WorldTileData data = pTile.data;
    this.type = data.type;
    this.height = data.height;
    this.conwayType = data.conwayType;
    this.fire_timestamp = data.fire_timestamp;
    this.frozen = data.frozen;
    this.tile_id = pTileID;
    this.x = pTile.x;
    this.y = pTile.y;
  }

  public bool Equals(WorldTileDataStruct pOther) => this.tile_id == pOther.tile_id;

  public override bool Equals(object pObject)
  {
    return pObject is WorldTileDataStruct pOther && this.Equals(pOther);
  }

  public override int GetHashCode() => this.tile_id;
}
