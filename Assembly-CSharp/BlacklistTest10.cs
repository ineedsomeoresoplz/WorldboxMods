// Decompiled with JetBrains decompiler
// Type: BlacklistTest10
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest10
{
  private static readonly Dictionary<char, string[]> _profanity = new Dictionary<char, string[]>();
  private static readonly HashSet<char> _unique = new HashSet<char>();
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest10._initiated)
      return;
    BlacklistTest10._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest10._profanity, BlacklistTest10._unique);
  }

  public static bool checkBlackList(string pName)
  {
    string lower = pName.ToLower();
    BlacklistTest10._unique.Clear();
    BlacklistTest10._unique.UnionWith((IEnumerable<char>) lower);
    BlacklistTest10._unique.RemoveWhere((Predicate<char>) (pChar => !char.IsLetter(pChar)));
    string str = BlacklistTools.cleanString(lower);
    bool flag = !(str == lower);
    Dictionary<char, string[]> profanity = BlacklistTest10._profanity;
    foreach (char key in BlacklistTest10._unique)
    {
      if (profanity.ContainsKey(key))
      {
        for (int index = 0; index < profanity[key].Length; ++index)
        {
          if (lower.Contains(profanity[key][index]) || flag && str.Contains(profanity[key][index]))
            return true;
        }
      }
    }
    return false;
  }
}
