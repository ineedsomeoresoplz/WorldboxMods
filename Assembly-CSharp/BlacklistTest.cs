// Decompiled with JetBrains decompiler
// Type: BlacklistTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest
{
  private static readonly Dictionary<char, string[]> _profanity = new Dictionary<char, string[]>();
  private static readonly HashSet<char> _unique = new HashSet<char>();
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest._initiated)
      return;
    BlacklistTest._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest._profanity, BlacklistTest._unique);
  }

  public static bool checkBlackList(string pName)
  {
    string lower = pName.ToLower();
    BlacklistTest._unique.Clear();
    BlacklistTest._unique.UnionWith((IEnumerable<char>) lower);
    BlacklistTest._unique.RemoveWhere((Predicate<char>) (pChar => !char.IsLetter(pChar)));
    Dictionary<char, string[]> profanity = BlacklistTest._profanity;
    ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(lower);
    ReadOnlySpan<char> pText = BlacklistTools.cleanSpan(readOnlySpan);
    bool flag = !ReadOnlySpan<char>.op_Equality(pText, readOnlySpan);
    foreach (char key in BlacklistTest._unique)
    {
      string[] strArray;
      if (profanity.TryGetValue(key, out strArray))
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          ReadOnlySpan<char> pSearchPattern = MemoryExtensions.AsSpan(strArray[index]);
          if (BlacklistTools.contains(readOnlySpan, pSearchPattern) || flag && BlacklistTools.contains(pText, pSearchPattern))
            return true;
        }
      }
    }
    return false;
  }
}
