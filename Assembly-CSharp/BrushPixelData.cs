// Decompiled with JetBrains decompiler
// Type: BrushPixelData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;

#nullable disable
[JsonConverter(typeof (BrushPixelDataConverter))]
[Serializable]
public readonly struct BrushPixelData(int pX, int pY, int pDist) : IEquatable<BrushPixelData>
{
  public readonly int x = pX;
  public readonly int y = pY;
  public readonly int dist = pDist;

  public bool Equals(BrushPixelData pOther) => this.x == pOther.x && this.y == pOther.y;

  public override int GetHashCode() => this.x * 100000 + this.y;
}
