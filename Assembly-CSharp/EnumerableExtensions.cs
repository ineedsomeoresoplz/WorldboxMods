// Decompiled with JetBrains decompiler
// Type: EnumerableExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class EnumerableExtensions
{
  public static T GetRandom<T>(this IEnumerable<T> pEnumerable)
  {
    switch (pEnumerable)
    {
      case List<T> list1:
        return list1.GetRandom<T>();
      case ListPool<T> list2:
        return list2.GetRandom<T>();
      case T[] pArray:
        return pArray.GetRandom<T>();
      case HashSet<T> pHashSet:
        return pHashSet.GetRandom<T>();
      default:
        using (ListPool<T> list = new ListPool<T>(pEnumerable))
          return list.GetRandom<T>();
    }
  }
}
