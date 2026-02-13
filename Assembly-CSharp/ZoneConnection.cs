// Decompiled with JetBrains decompiler
// Type: ZoneConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
public readonly struct ZoneConnection(TileZone pZone, MapRegion pRegion) : IEquatable<ZoneConnection>
{
  public readonly TileZone zone = pZone;
  public readonly MapRegion region = pRegion;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(ZoneConnection pObject)
  {
    return this.zone.Equals((object) pObject.zone) && this.region.Equals(pObject.region);
  }

  public override int GetHashCode() => this.zone.GetHashCode() + this.region.GetHashCode() * 100000;
}
