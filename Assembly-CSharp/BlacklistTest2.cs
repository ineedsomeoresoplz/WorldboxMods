// Decompiled with JetBrains decompiler
// Type: BlacklistTest2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest2
{
  private static readonly Dictionary<char, string[]> _profanity = new Dictionary<char, string[]>();
  private static readonly HashSet<char> _unique = new HashSet<char>();
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest2._initiated)
      return;
    BlacklistTest2._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest2._profanity, BlacklistTest2._unique);
  }

  internal static bool checkBlackList(string pName)
  {
    string lower = pName.ToLower();
    BlacklistTest2._unique.Clear();
    BlacklistTest2._unique.UnionWith((IEnumerable<char>) lower);
    BlacklistTest2._unique.RemoveWhere((Predicate<char>) (pChar => !char.IsLetter(pChar)));
    Dictionary<char, string[]> profanity = BlacklistTest2._profanity;
    string str = BlacklistTools.cleanStringAsSpan(lower);
    bool flag = !(str == lower);
    ReadOnlySpan<char> pText1 = MemoryExtensions.AsSpan(lower);
    ReadOnlySpan<char> pText2 = flag ? MemoryExtensions.AsSpan(str) : ReadOnlySpan<char>.op_Implicit((char[]) null);
    foreach (char key in BlacklistTest2._unique)
    {
      string[] strArray;
      if (profanity.TryGetValue(key, out strArray))
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          ReadOnlySpan<char> pSearchPattern = MemoryExtensions.AsSpan(strArray[index]);
          if (BlacklistTools.contains(pText1, pSearchPattern) || flag && BlacklistTools.contains(pText2, pSearchPattern))
            return true;
        }
      }
    }
    return false;
  }
}
