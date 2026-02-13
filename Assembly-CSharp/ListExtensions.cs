// Decompiled with JetBrains decompiler
// Type: ListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
public static class ListExtensions
{
  private static Random rnd => Randy.rnd;

  public static string ToJson(this List<string> list)
  {
    return list.Count == 0 ? "[]" : $"['{string.Join("','", (IEnumerable<string>) list)}']";
  }

  public static void ShuffleHalf<T>(this List<T> list)
  {
    if (list.Count < 2)
      return;
    int count = list.Count;
    int num = count / 2 + 1;
    for (int index = 0; index < num && index < count; index += 2)
      list.Swap<T>(index, ListExtensions.rnd.Next(index, count));
  }

  public static void ShuffleN<T>(this List<T> list, int pItems)
  {
    if (list.Count < 2)
      return;
    int maxValue = list.Count < pItems ? list.Count : pItems;
    for (int index = 0; index < maxValue; ++index)
      list.Swap<T>(index, ListExtensions.rnd.Next(index, maxValue));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Shuffle<T>(this List<T> list)
  {
    if (list.Count < 2)
      return;
    int count = list.Count;
    for (int index = 0; index < count; ++index)
      list.Swap<T>(index, ListExtensions.rnd.Next(index, count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ShuffleOne<T>(this List<T> list)
  {
    if (list.Count < 2)
      return;
    list.Swap<T>(0, ListExtensions.rnd.Next(0, list.Count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ShuffleOne<T>(this List<T> list, int nItem)
  {
    if (list.Count < 2 || list.Count < nItem + 1)
      return;
    list.Swap<T>(nItem, ListExtensions.rnd.Next(nItem, list.Count));
  }

  public static void ShuffleLast<T>(this List<T> list)
  {
    if (list.Count < 2)
      return;
    list.Swap<T>(list.Count - 1, ListExtensions.rnd.Next(0, list.Count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T Pop<T>(this List<T> list)
  {
    T obj = list[list.Count - 1];
    list.RemoveAt(list.Count - 1);
    return obj;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T Shift<T>(this List<T> list)
  {
    T obj = list[0];
    list.RemoveAt(0);
    return obj;
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T First<T>(this List<T> list) => list[0];

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T Last<T>(this List<T> list) => list[list.Count - 1];

  public static void ShuffleRandomOne<T>(this List<T> list)
  {
    if (list.Count < 2)
      return;
    int num = Randy.randomInt(0, list.Count - 1);
    list.Swap<T>(num, ListExtensions.rnd.Next(num, list.Count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Swap<T>(this List<T> list, int i, int j)
  {
    T obj = list[i];
    list[i] = list[j];
    list[j] = obj;
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T GetRandom<T>(this List<T> list) => list[ListExtensions.rnd.Next(0, list.Count)];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void RemoveAtSwapBack<T>(this List<T> list, T pObject)
  {
    int index1 = list.IndexOf(pObject);
    if (index1 == -1)
      return;
    int index2 = list.Count - 1;
    list[index1] = list[index2];
    list[index2] = pObject;
    list.RemoveAt(index2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool Any<T>(this List<T> list) => list != null && __nonvirtual (list.Count) > 0;

  [Pure]
  public static bool SetEquals<T>(this List<T> pList, IEnumerable<T> pOther)
  {
    if (pList == null || pOther == null)
      return false;
    HashSet<T> objSet = CollectionPool<HashSet<T>, T>.Get();
    HashSet<T> other = CollectionPool<HashSet<T>, T>.Get();
    objSet.UnionWith((IEnumerable<T>) pList);
    other.UnionWith(pOther);
    bool flag = objSet.SetEquals((IEnumerable<T>) other);
    other.Clear();
    objSet.Clear();
    CollectionPool<HashSet<T>, T>.Release(objSet);
    CollectionPool<HashSet<T>, T>.Release(other);
    return flag;
  }

  public static string ToLineString<T>(this List<T> pList, string pSeparator = ",")
  {
    return pList == null ? string.Empty : string.Join<T>(pSeparator, (IEnumerable<T>) pList);
  }

  public static void PrintToConsole<T>(this List<T> pList)
  {
    if (pList == null)
      return;
    Debug.Log((object) pList.ToLineString<T>());
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void AddTimes<T>(this List<T> pList, int pAmount, T pObject)
  {
    for (int index = 0; index < pAmount; ++index)
      pList.Add(pObject);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T LoopNext<T>(this List<T> pList, T pObject)
  {
    int num = pList.IndexOf(pObject);
    if (num == -1)
      return pObject;
    int index = num + 1;
    if (index >= pList.Count)
      index = 0;
    return pList[index];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Span<T> AsSpan<T>(this List<T> pList)
  {
    ListAccessHelper.ListDataHelper<T> listDataHelper = UnsafeUtility.As<List<T>, ListAccessHelper.ListDataHelper<T>>(ref pList);
    int size = listDataHelper._size;
    T[] items = listDataHelper._items;
    if ((uint) size > (uint) items.Length)
      throw new InvalidOperationException("Concurrent operations are not supported.");
    return new Span<T>(items, 0, size);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ReadOnlySpan<T> AsReadOnlySpan<T>(this List<T> pList)
  {
    ListAccessHelper.ListDataHelper<T> listDataHelper = UnsafeUtility.As<List<T>, ListAccessHelper.ListDataHelper<T>>(ref pList);
    int size = listDataHelper._size;
    T[] items = listDataHelper._items;
    if ((uint) size > (uint) items.Length)
      throw new InvalidOperationException("Concurrent operations are not supported.");
    return new ReadOnlySpan<T>(items, 0, size);
  }

  public static string AsString<T>(this List<T> pList) => pList.ToArray().AsString<T>();
}
