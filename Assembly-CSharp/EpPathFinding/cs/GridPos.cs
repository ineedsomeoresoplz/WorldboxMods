// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.GridPos
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace EpPathFinding.cs;

public class GridPos : IEquatable<GridPos>
{
  public int x;
  public int y;

  public GridPos()
  {
    this.x = 0;
    this.y = 0;
  }

  public GridPos(int iX, int iY)
  {
    this.x = iX;
    this.y = iY;
  }

  public GridPos(GridPos b)
  {
    this.x = b.x;
    this.y = b.y;
  }

  public override int GetHashCode() => this.x ^ this.y;

  public override bool Equals(object obj)
  {
    GridPos gridPos = (GridPos) obj;
    return (object) gridPos != null && this.x == gridPos.x && this.y == gridPos.y;
  }

  public bool Equals(GridPos p) => (object) p != null && this.x == p.x && this.y == p.y;

  public static bool operator ==(GridPos a, GridPos b)
  {
    if ((object) a == (object) b)
      return true;
    return (object) a != null && (object) b != null && a.x == b.x && a.y == b.y;
  }

  public static bool operator !=(GridPos a, GridPos b) => !(a == b);

  public GridPos Set(int iX, int iY)
  {
    this.x = iX;
    this.y = iY;
    return this;
  }

  public override string ToString() => $"({this.x},{this.y})";
}
