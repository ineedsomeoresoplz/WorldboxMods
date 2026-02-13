// Decompiled with JetBrains decompiler
// Type: BlacklistTest12
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class BlacklistTest12
{
  private static readonly Dictionary<string, string[]> _profanity = new Dictionary<string, string[]>();
  private const int INDEX_LENGTH = 3;
  private static bool _initiated = false;

  public static void init()
  {
    if (BlacklistTest12._initiated)
      return;
    BlacklistTest12._initiated = true;
    BlacklistTools.loadProfanityFilter(BlacklistTest12._profanity);
  }

  internal static bool checkBlackList(string pName)
  {
    ReadOnlySpan<char> pText1 = MemoryExtensions.AsSpan(pName.ToLower());
    Dictionary<string, string[]> profanity = BlacklistTest12._profanity;
    ReadOnlySpan<char> readOnlySpan;
    for (int index1 = 0; index1 < pText1.Length - 3 + 1; ++index1)
    {
      readOnlySpan = pText1.Slice(index1, 3);
      string key = readOnlySpan.ToString();
      string[] strArray;
      if (profanity.TryGetValue(key, out strArray))
      {
        for (int index2 = 0; index2 < strArray.Length; ++index2)
        {
          ReadOnlySpan<char> pSearchPattern = MemoryExtensions.AsSpan(strArray[index2]);
          if (BlacklistTools.contains(ref pText1, ref pSearchPattern))
            return true;
        }
      }
    }
    ReadOnlySpan<char> pText2 = BlacklistTools.cleanSpan(pText1);
    if ((ReadOnlySpan<char>.op_Equality(pText2, pText1) ? 0 : (pText2.Length > 2 ? 1 : 0)) == 0)
      return false;
    for (int index3 = 0; index3 < pText2.Length - 3 + 1; ++index3)
    {
      readOnlySpan = pText2.Slice(index3, 3);
      string key = readOnlySpan.ToString();
      string[] strArray;
      if (profanity.TryGetValue(key, out strArray))
      {
        for (int index4 = 0; index4 < strArray.Length; ++index4)
        {
          ReadOnlySpan<char> pSearchPattern = MemoryExtensions.AsSpan(strArray[index4]);
          if (BlacklistTools.contains(ref pText2, ref pSearchPattern))
            return true;
        }
      }
    }
    return false;
  }
}
