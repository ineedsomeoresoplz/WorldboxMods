// Decompiled with JetBrains decompiler
// Type: ArrayExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public static class ArrayExtensions
{
  private static Random rnd => Randy.rnd;

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T First<T>(this T[] pArray) => pArray[0];

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T Last<T>(this T[] pArray) => pArray[pArray.Length - 1];

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int IndexOf<T>(this T[] pArray, T pValue) => Array.IndexOf<T>(pArray, pValue);

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool Contains<T>(this T[] pArray, T pValue)
  {
    return Array.IndexOf<T>(pArray, pValue) > -1;
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int FreeIndex<T>(this T[] pArray) => Array.IndexOf((Array) pArray, (object) null);

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T GetRandom<T>(this T[] pArray)
  {
    return pArray[ArrayExtensions.rnd.Next(0, pArray.Length)];
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T GetRandom<T>(this T[] pArray, int pLength)
  {
    return pArray[ArrayExtensions.rnd.Next(0, pLength)];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Swap<T>(this T[] pArray, int pIndex1, int pIndex2)
  {
    T p = pArray[pIndex1];
    pArray[pIndex1] = pArray[pIndex2];
    pArray[pIndex2] = p;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Shuffle<T>(this T[] pArray)
  {
    if (pArray.Length < 2)
      return;
    int length = pArray.Length;
    for (int index = 0; index < length; ++index)
      pArray.Swap<T>(index, ArrayExtensions.rnd.Next(index, length));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Shuffle<T>(this T[] pArray, int pCount)
  {
    if (pCount < 2)
      return;
    for (int index = 0; index < pCount; ++index)
      pArray.Swap<T>(index, ArrayExtensions.rnd.Next(index, pCount));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ShuffleOne<T>(this T[] pArray)
  {
    if (pArray.Length < 2)
      return;
    pArray.Swap<T>(0, ArrayExtensions.rnd.Next(0, pArray.Length));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ShuffleOne<T>(this T[] pArray, int pItem)
  {
    if (pArray.Length < 2 || pArray.Length < pItem + 1)
      return;
    pArray.Swap<T>(pItem, ArrayExtensions.rnd.Next(pItem, pArray.Length));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ShuffleOne<T>(this T[] pArray, int pItem, int pCount)
  {
    if (pCount < 2 || pCount < pItem + 1)
      return;
    pArray.Swap<T>(pItem, ArrayExtensions.rnd.Next(pItem, pCount));
  }

  public static void Clear<T>(this T[] pArray) => Array.Clear((Array) pArray, 0, pArray.Length);

  public static void Clear<T>(this T[] pArray, int pCount)
  {
    Array.Clear((Array) pArray, 0, pCount);
  }

  [Pure]
  public static bool AnyTrue(this bool[] pArray)
  {
    foreach (bool p in pArray)
    {
      if (p)
        return true;
    }
    return false;
  }

  [Pure]
  public static bool AnyFalse(this bool[] pArray)
  {
    foreach (bool p in pArray)
    {
      if (!p)
        return true;
    }
    return false;
  }

  public static string AsString<T>(this T[] pArray)
  {
    if (pArray == null)
      return "";
    using (ListPool<string> list = new ListPool<string>(pArray.Length))
    {
      foreach (T p in pArray)
        list.Add(p?.ToString() ?? "null");
      return string.Join(", ", list.ToArray<string>());
    }
  }

  public static void PrintToConsole<T>(this T[] pArray, string pMessage = null)
  {
    if (pArray == null)
      return;
    string str = "";
    foreach (T p in pArray)
      str = $"{str}{p.ToString()},";
    if (str.Length > 0)
      str = str.TrimEnd(',');
    if (pMessage != null)
      Debug.Log((object) $"{pMessage}: [{str}]");
    else
      Debug.Log((object) str);
  }

  public static bool AllTrue(this bool[] pArray) => !pArray.AnyFalse();

  public static bool AllFalse(this bool[] pArray) => !pArray.AnyTrue();
}
