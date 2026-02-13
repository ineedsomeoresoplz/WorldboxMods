// Decompiled with JetBrains decompiler
// Type: ListPoolExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Pool;

#nullable disable
public static class ListPoolExtensions
{
  private static Random rnd => Randy.rnd;

  public static string ToJson(this ListPool<string> list)
  {
    return list.Count == 0 ? "[]" : $"['{string.Join("','", (IEnumerable<string>) list)}']";
  }

  public static void ShuffleHalf<T>(this ListPool<T> list)
  {
    if (list.Count < 2)
      return;
    int count = list.Count;
    int num = count / 2 + 1;
    for (int index = 0; index < num && index < count; index += 2)
      list.Swap<T>(index, ListPoolExtensions.rnd.Next(index, count));
  }

  public static void ShuffleN<T>(this ListPool<T> list, int pItems)
  {
    if (list.Count < 2)
      return;
    int maxValue = list.Count < pItems ? list.Count : pItems;
    for (int index = 0; index < maxValue; ++index)
      list.Swap<T>(index, ListPoolExtensions.rnd.Next(index, maxValue));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Shuffle<T>(this ListPool<T> list)
  {
    if (list.Count < 2)
      return;
    int count = list.Count;
    for (int index = 0; index < count; ++index)
      list.Swap<T>(index, ListPoolExtensions.rnd.Next(index, count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ShuffleOne<T>(this ListPool<T> list)
  {
    if (list.Count < 2)
      return;
    list.Swap<T>(0, ListPoolExtensions.rnd.Next(0, list.Count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ShuffleOne<T>(this ListPool<T> list, int nItem)
  {
    if (list.Count < 2 || list.Count < nItem + 1)
      return;
    list.Swap<T>(nItem, ListPoolExtensions.rnd.Next(nItem, list.Count));
  }

  public static void ShuffleLast<T>(this ListPool<T> list)
  {
    if (list.Count < 2)
      return;
    list.Swap<T>(list.Count - 1, ListPoolExtensions.rnd.Next(0, list.Count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T Pop<T>(this ListPool<T> list)
  {
    T obj = list[list.Count - 1];
    list.RemoveAt(list.Count - 1);
    return obj;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T Shift<T>(this ListPool<T> list)
  {
    T obj = list[0];
    list.RemoveAt(0);
    return obj;
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T First<T>(this ListPool<T> list) => list[0];

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T Last<T>(this ListPool<T> list) => list[list.Count - 1];

  public static void ShuffleRandomOne<T>(this ListPool<T> list)
  {
    if (list.Count < 2)
      return;
    int num = Randy.randomInt(0, list.Count - 1);
    list.Swap<T>(num, ListPoolExtensions.rnd.Next(num, list.Count));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void Swap<T>(this ListPool<T> list, int i, int j)
  {
    T[] rawBuffer = list.GetRawBuffer();
    T obj = rawBuffer[i];
    rawBuffer[i] = rawBuffer[j];
    rawBuffer[j] = obj;
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T GetRandom<T>(this ListPool<T> list)
  {
    return list[ListPoolExtensions.rnd.Next(0, list.Count)];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void RemoveAtSwapBack<T>(this ListPool<T> list, T pObject)
  {
    int index1 = list.IndexOf(pObject);
    if (index1 == -1)
      return;
    int index2 = list.Count - 1;
    list[index1] = list[index2];
    list[index2] = pObject;
    list.RemoveAt(index2);
  }

  [Pure]
  public static T[] ToArray<T>(this ListPool<T> list)
  {
    T[] array = new T[list.Count];
    list.CopyTo(array, 0);
    return array;
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool Any<T>(this ListPool<T> list) => list != null && list.Count > 0;

  [Pure]
  public static bool SetEquals<T>(this ListPool<T> pList, IEnumerable<T> pOther)
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

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void AddTimes<T>(this ListPool<T> pList, int pAmount, T pObject)
  {
    for (int index = 0; index < pAmount; ++index)
      pList.Add(pObject);
  }

  public static int CountAll<T>(this ListPool<T> pList, Predicate<T> pMatch)
  {
    int num = 0;
    for (int index = 0; index < pList.Count; ++index)
    {
      if (pMatch(pList[index]))
        ++num;
    }
    return num;
  }

  public static IEnumerable<T> Where<T>(this ListPool<T> pList, Func<T, bool> pPredicate)
  {
    for (int i = 0; i < pList.Count; ++i)
    {
      if (pPredicate(pList[i]))
        yield return pList[i];
    }
  }

  [Pure]
  public static bool ValuesEqual<T>(this ListPool<T> pList, ListPool<T> pOther)
  {
    return pList.Count == pOther.Count && pList.GetLongHashCode<T>() == pOther.GetLongHashCode<T>();
  }

  [Pure]
  public static long GetLongHashCode<T>(this ListPool<T> pList)
  {
    // ISSUE: unable to decompile the method.
  }

  public static string AsString<T>(this ListPool<T> pListPool)
  {
    return pListPool.ToArray<T>().AsString<T>();
  }
}
