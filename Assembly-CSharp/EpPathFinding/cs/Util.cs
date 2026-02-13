// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.Util
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace EpPathFinding.cs;

public class Util
{
  public static DiagonalMovement GetDiagonalMovement(bool iCrossCorners, bool iCrossAdjacentPoint)
  {
    if (iCrossCorners & iCrossAdjacentPoint)
      return DiagonalMovement.Always;
    return iCrossCorners ? DiagonalMovement.IfAtLeastOneWalkable : DiagonalMovement.OnlyWhenNoObstacles;
  }
}
