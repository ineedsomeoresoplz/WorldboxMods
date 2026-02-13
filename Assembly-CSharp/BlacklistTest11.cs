// Decompiled with JetBrains decompiler
// Type: BlacklistTest11
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest11
{
  private static readonly Dictionary<string, string[]> _profanity = new Dictionary<string, string[]>();
  private const int INDEX_LENGTH = 3;
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest11._initiated)
      return;
    BlacklistTest11._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest11._profanity);
  }

  internal static bool checkBlackList(string pName)
  {
    ReadOnlySpan<char> pSpan = MemoryExtensions.AsSpan(pName.ToLower());
    int length1 = pSpan.Length;
    Dictionary<string, string[]> profanity = BlacklistTest11._profanity;
    for (int index1 = 0; index1 < length1 - 3 + 1; ++index1)
    {
      string key = pSpan.Slice(index1, 3).ToString();
      string[] strArray;
      if (profanity.TryGetValue(key, out strArray))
      {
        for (int index2 = 0; index2 < strArray.Length; ++index2)
        {
          ReadOnlySpan<char> pSearchPattern = MemoryExtensions.AsSpan(strArray[index2]);
          if (BlacklistTools.contains(pSpan.Slice(index1), pSearchPattern))
            return true;
        }
      }
    }
    ReadOnlySpan<char> readOnlySpan = BlacklistTools.cleanSpan(pSpan);
    int length2 = readOnlySpan.Length;
    if ((ReadOnlySpan<char>.op_Equality(readOnlySpan, pSpan) ? 0 : (length2 > 2 ? 1 : 0)) == 0)
      return false;
    for (int index3 = 0; index3 < length2 - 3 + 1; ++index3)
    {
      string key = readOnlySpan.Slice(index3, 3).ToString();
      string[] strArray;
      if (profanity.TryGetValue(key, out strArray))
      {
        for (int index4 = 0; index4 < strArray.Length; ++index4)
        {
          ReadOnlySpan<char> pSearchPattern = MemoryExtensions.AsSpan(strArray[index4]);
          if (BlacklistTools.contains(readOnlySpan.Slice(index3), pSearchPattern))
            return true;
        }
      }
    }
    return false;
  }
}
