// Decompiled with JetBrains decompiler
// Type: BlacklistTest7
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest7
{
  private static readonly Dictionary<int, HashSet<int>> _profanity = new Dictionary<int, HashSet<int>>();
  private static int _min_length = int.MaxValue;
  private static int _max_length = int.MinValue;
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest7._initiated)
      return;
    BlacklistTest7._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest7._profanity, ref BlacklistTest7._min_length, ref BlacklistTest7._max_length);
  }

  private static int getCharHashCode(char[] pChar) => BlacklistTools.getCharHashCode(pChar);

  internal static bool checkBlackList(string pName)
  {
    ReadOnlySpan<char> pSpan = MemoryExtensions.AsSpan(pName.ToLower());
    ReadOnlySpan<char> readOnlySpan = BlacklistTools.cleanSpan(pSpan);
    bool flag = !ReadOnlySpan<char>.op_Equality(readOnlySpan, pSpan);
    for (int minLength = BlacklistTest7._min_length; minLength <= BlacklistTest7._max_length; ++minLength)
    {
      HashSet<int> intSet = BlacklistTest7._profanity[minLength];
      for (int index = 0; index < pSpan.Length - minLength + 1; ++index)
      {
        int charHashCode1 = BlacklistTest7.getCharHashCode(pSpan.Slice(index, minLength).ToArray());
        if (intSet.Contains(charHashCode1))
          return true;
        if (flag && readOnlySpan.Length >= index + minLength)
        {
          int charHashCode2 = BlacklistTest7.getCharHashCode(readOnlySpan.Slice(index, minLength).ToArray());
          if (intSet.Contains(charHashCode2))
            return true;
        }
      }
    }
    return false;
  }
}
