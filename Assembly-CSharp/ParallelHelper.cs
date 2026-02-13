// Decompiled with JetBrains decompiler
// Type: ParallelHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public static class ParallelHelper
{
  public static int DEBUG_BATCH_SIZE = 128 /*0x80*/;
  private const int MAX_BATCH_SIZE = 256 /*0x0100*/;
  private static readonly int[] _batch_sizes = new int[11]
  {
    4,
    8,
    16 /*0x10*/,
    32 /*0x20*/,
    64 /*0x40*/,
    128 /*0x80*/,
    256 /*0x0100*/,
    512 /*0x0200*/,
    1024 /*0x0400*/,
    2048 /*0x0800*/,
    4096 /*0x1000*/
  };

  public static int getDynamicBatchSize(int pCount)
  {
    if (pCount <= 32 /*0x20*/)
      return 4;
    if (pCount <= 64 /*0x40*/)
      return 8;
    if (pCount <= 128 /*0x80*/)
      return 16 /*0x10*/;
    if (pCount <= 256 /*0x0100*/)
      return 32 /*0x20*/;
    if (pCount <= 512 /*0x0200*/)
      return 64 /*0x40*/;
    return pCount <= 2048 /*0x0800*/ ? 128 /*0x80*/ : 256 /*0x0100*/;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int calcTotalBatches(int pItemsTotal, int pBatchSize)
  {
    return Mathf.CeilToInt((float) pItemsTotal / (float) pBatchSize);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int calculateBatchEnd(int pItemsIndexStart, int pBatchSize, int pItemsTotal)
  {
    return Mathf.Min(pItemsIndexStart + pBatchSize, pItemsTotal);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int calculateBatchBeg(int pBatchIndex, int pBatchSize) => pBatchIndex * pBatchSize;

  public static void moveDebugBatchSize()
  {
    int num = Array.IndexOf<int>(ParallelHelper._batch_sizes, ParallelHelper.DEBUG_BATCH_SIZE);
    if (num == -1)
      num = 0;
    int index = (num + 1) % ParallelHelper._batch_sizes.Length;
    ParallelHelper.DEBUG_BATCH_SIZE = ParallelHelper._batch_sizes[index];
  }
}
