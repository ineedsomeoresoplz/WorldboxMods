// Decompiled with JetBrains decompiler
// Type: EpPathFinding.cs.Heuristic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace EpPathFinding.cs;

public class Heuristic
{
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float Manhattan(int iDx, int iDy) => (float) (iDx + iDy);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float Euclidean(int iDx, int iDy)
  {
    double num1 = (double) iDx;
    float num2 = (float) iDy;
    return (float) Math.Sqrt(num1 * num1 + (double) num2 * (double) num2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float Chebyshev(int iDx, int iDy) => (float) Mathf.Max(iDx, iDy);
}
