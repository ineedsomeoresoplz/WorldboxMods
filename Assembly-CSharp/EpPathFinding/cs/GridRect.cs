// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.GridRect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace EpPathFinding.cs;

public class GridRect
{
  public int minX;
  public int minY;
  public int maxX;
  public int maxY;

  public GridRect()
  {
    this.minX = 0;
    this.minY = 0;
    this.maxX = 0;
    this.maxY = 0;
  }

  public GridRect(int iMinX, int iMinY, int iMaxX, int iMaxY)
  {
    this.minX = iMinX;
    this.minY = iMinY;
    this.maxX = iMaxX;
    this.maxY = iMaxY;
  }

  public GridRect(GridRect b)
  {
    this.minX = b.minX;
    this.minY = b.minY;
    this.maxX = b.maxX;
    this.maxY = b.maxY;
  }

  public override int GetHashCode() => this.minX ^ this.minY ^ this.maxX ^ this.maxY;

  public override bool Equals(object obj)
  {
    GridRect gridRect = (GridRect) obj;
    return (object) gridRect != null && this.minX == gridRect.minX && this.minY == gridRect.minY && this.maxX == gridRect.maxX && this.maxY == gridRect.maxY;
  }

  public bool Equals(GridRect p)
  {
    return (object) p != null && this.minX == p.minX && this.minY == p.minY && this.maxX == p.maxX && this.maxY == p.maxY;
  }

  public static bool operator ==(GridRect a, GridRect b)
  {
    if ((object) a == (object) b)
      return true;
    return (object) a != null && (object) b != null && a.minX == b.minX && a.minY == b.minY && a.maxX == b.maxX && a.maxY == b.maxY;
  }

  public static bool operator !=(GridRect a, GridRect b) => !(a == b);

  public GridRect Set(int iMinX, int iMinY, int iMaxX, int iMaxY)
  {
    this.minX = iMinX;
    this.minY = iMinY;
    this.maxX = iMaxX;
    this.maxY = iMaxY;
    return this;
  }
}
