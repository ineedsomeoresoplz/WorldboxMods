// Decompiled with JetBrains decompiler
// Type: BlacklistTest8
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest8
{
  private static readonly Dictionary<int, HashSet<int>> _profanity = new Dictionary<int, HashSet<int>>();
  private static int _min_length = int.MaxValue;
  private static int _max_length = int.MinValue;
  private static readonly Dictionary<int, char[]> _char_arrays = new Dictionary<int, char[]>();
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest8._initiated)
      return;
    BlacklistTest8._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest8._profanity, ref BlacklistTest8._min_length, ref BlacklistTest8._max_length);
    for (int minLength = BlacklistTest8._min_length; minLength <= BlacklistTest8._max_length; ++minLength)
      BlacklistTest8._char_arrays[minLength] = new char[minLength];
  }

  private static int getCharHashCode(char[] pChar) => BlacklistTools.getCharHashCode(pChar);

  internal static bool checkBlackList(string pName)
  {
    ReadOnlySpan<char> pSpan = MemoryExtensions.AsSpan(pName.ToLower());
    ReadOnlySpan<char> readOnlySpan = BlacklistTools.cleanSpan(pSpan);
    bool flag = !ReadOnlySpan<char>.op_Equality(readOnlySpan, pSpan);
    for (int minLength = BlacklistTest8._min_length; minLength <= BlacklistTest8._max_length; ++minLength)
    {
      char[] charArray = BlacklistTest8._char_arrays[minLength];
      HashSet<int> intSet = BlacklistTest8._profanity[minLength];
      for (int index = 0; index < pSpan.Length - minLength + 1; ++index)
      {
        pSpan.Slice(index, minLength).CopyTo(Span<char>.op_Implicit(charArray));
        int charHashCode1 = BlacklistTest8.getCharHashCode(charArray);
        if (intSet.Contains(charHashCode1))
          return true;
        if (flag && readOnlySpan.Length >= index + minLength)
        {
          readOnlySpan.Slice(index, minLength).CopyTo(Span<char>.op_Implicit(charArray));
          int charHashCode2 = BlacklistTest8.getCharHashCode(charArray);
          if (intSet.Contains(charHashCode2))
            return true;
        }
      }
    }
    return false;
  }
}
