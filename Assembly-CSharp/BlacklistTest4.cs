// Decompiled with JetBrains decompiler
// Type: BlacklistTest4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest4
{
  private static readonly Dictionary<char, char[][]> _profanity = new Dictionary<char, char[][]>();
  private static readonly HashSet<char> _unique = new HashSet<char>();
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest4._initiated)
      return;
    BlacklistTest4._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest4._profanity, BlacklistTest4._unique);
  }

  internal static bool checkBlackList(string pName)
  {
    string lower = pName.ToLower();
    ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(lower);
    BlacklistTest4._unique.Clear();
    BlacklistTest4._unique.UnionWith((IEnumerable<char>) lower);
    BlacklistTest4._unique.RemoveWhere((Predicate<char>) (pChar => !char.IsLetter(pChar)));
    ReadOnlySpan<char> pText = BlacklistTools.cleanSpan(readOnlySpan);
    bool flag = !ReadOnlySpan<char>.op_Equality(pText, readOnlySpan);
    Dictionary<char, char[][]> profanity = BlacklistTest4._profanity;
    foreach (char key in BlacklistTest4._unique)
    {
      char[][] chArray;
      if (profanity.TryGetValue(key, out chArray))
      {
        for (int index = 0; index < chArray.Length; ++index)
        {
          ReadOnlySpan<char> pSearchPattern = Span<char>.op_Implicit(MemoryExtensions.AsSpan<char>(chArray[index]));
          if (BlacklistTools.contains(readOnlySpan, pSearchPattern) || flag && BlacklistTools.contains(pText, pSearchPattern))
            return true;
        }
      }
    }
    return false;
  }
}
