// Decompiled with JetBrains decompiler
// Type: HashSetExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public static class HashSetExtensions
{
  [CanBeNull]
  public static T GetRandom<T>(this HashSet<T> pHashSet)
  {
    int num1 = Randy.randomInt(0, pHashSet.Count);
    int num2 = 0;
    foreach (T pHash in pHashSet)
    {
      if (num2++ == num1)
        return pHash;
    }
    return default (T);
  }

  public static T[] ToArray<T>(this HashSet<T> pHashSet)
  {
    T[] array = new T[pHashSet.Count];
    pHashSet.CopyTo(array);
    return array;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool Any<T>(this HashSet<T> pHashSet)
  {
    // ISSUE: explicit non-virtual call
    return pHashSet != null && __nonvirtual (pHashSet.Count) > 0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool RemoveAll<T>(this HashSet<T> pHashSet, ICollection<T> pToRemove)
  {
    if (pToRemove == null)
      throw new ArgumentNullException(nameof (pToRemove));
    if (pToRemove.Count == 0 || pHashSet.Count == 0)
      return false;
    int count1 = pHashSet.Count;
    pHashSet.ExceptWith((IEnumerable<T>) pToRemove);
    int count2 = pHashSet.Count;
    return count1 != count2;
  }

  public static T Pop<T>(this HashSet<T> pHashSet)
  {
    if (pHashSet == null)
      throw new ArgumentNullException(nameof (pHashSet));
    if (pHashSet.Count == 0)
      throw new InvalidOperationException("Cannot pop from an empty HashSet.");
    int num1 = pHashSet.Count - 1;
    int num2 = 0;
    foreach (T pHash in pHashSet)
    {
      if (num2++ == num1)
      {
        pHashSet.Remove(pHash);
        return pHash;
      }
    }
    throw new InvalidOperationException("Unexpected error: HashSet is empty after iteration.");
  }

  public static T Shift<T>(this HashSet<T> pHashSet)
  {
    if (pHashSet == null)
      throw new ArgumentNullException(nameof (pHashSet));
    if (pHashSet.Count == 0)
      throw new InvalidOperationException("Cannot shift from an empty HashSet.");
    using (HashSet<T>.Enumerator enumerator = pHashSet.GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        T current = enumerator.Current;
        pHashSet.Remove(current);
        return current;
      }
    }
    throw new InvalidOperationException("Unexpected error: HashSet is empty after iteration.");
  }
}
