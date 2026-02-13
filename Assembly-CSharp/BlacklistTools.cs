// Decompiled with JetBrains decompiler
// Type: BlacklistTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
public static class BlacklistTools
{
  private static string[] _profanities;

  public static string[] getProfanities()
  {
    if (BlacklistTools._profanities != null)
      return BlacklistTools._profanities;
    TextAsset textAsset = Resources.Load("blacklisted_names") as TextAsset;
    string text = textAsset.text;
    Resources.UnloadAsset((Object) textAsset);
    string[] strArray = Regex.Split(text, "\r\n?|\n", RegexOptions.Singleline);
    using (ListPool<string> list = new ListPool<string>(strArray.Length))
    {
      for (int index = 0; index < strArray.Length; ++index)
      {
        string lower = strArray[index].Trim().ToLower();
        if (lower.Length != 0)
          list.Add(lower);
      }
      BlacklistTools._profanities = list.ToArray<string>();
      return BlacklistTools._profanities;
    }
  }

  public static void loadProfanityFilter(
    Dictionary<char, string[]> pProfanity,
    HashSet<char> pUnique)
  {
    // ISSUE: explicit non-virtual call
    if (pProfanity != null && __nonvirtual (pProfanity.Count) > 0)
      return;
    try
    {
      Dictionary<char, List<string>> dictionary = new Dictionary<char, List<string>>();
      foreach (string profanity in BlacklistTools.getProfanities())
      {
        pUnique.Clear();
        pUnique.UnionWith((IEnumerable<char>) profanity);
        pUnique.RemoveWhere((Predicate<char>) (pChar => !char.IsLetter(pChar)));
        foreach (char key in pUnique)
        {
          if (!dictionary.ContainsKey(key))
            dictionary[key] = new List<string>();
          dictionary[key].Add(profanity);
        }
      }
      foreach (char key in dictionary.Keys)
        pProfanity[key] = dictionary[key].ToArray();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Error when loading blacklist");
      Debug.LogError((object) ex);
    }
  }

  public static void loadProfanityFilter(
    Dictionary<char, char[][]> pProfanity,
    HashSet<char> pUnique)
  {
    // ISSUE: explicit non-virtual call
    if (pProfanity != null && __nonvirtual (pProfanity.Count) > 0)
      return;
    try
    {
      Dictionary<char, List<char[]>> dictionary = new Dictionary<char, List<char[]>>();
      foreach (string profanity in BlacklistTools.getProfanities())
      {
        pUnique.Clear();
        pUnique.UnionWith((IEnumerable<char>) profanity);
        pUnique.RemoveWhere((Predicate<char>) (pChar => !char.IsLetter(pChar)));
        char[] charArray = profanity.ToCharArray();
        foreach (char key in pUnique)
        {
          if (!dictionary.ContainsKey(key))
            dictionary[key] = new List<char[]>();
          dictionary[key].Add(charArray);
        }
      }
      foreach (char key in dictionary.Keys)
        pProfanity[key] = dictionary[key].ToArray();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Error when loading blacklist");
      Debug.LogError((object) ex);
    }
  }

  public static void loadProfanityFilter(
    Dictionary<int, HashSet<int>> pProfanity,
    ref int pMinLength,
    ref int pMaxLength)
  {
    // ISSUE: explicit non-virtual call
    if (pProfanity != null && __nonvirtual (pProfanity.Count) > 0)
      return;
    try
    {
      foreach (string profanity in BlacklistTools.getProfanities())
      {
        if (profanity.Length < pMinLength)
          pMinLength = profanity.Length;
        if (profanity.Length > pMaxLength)
          pMaxLength = profanity.Length;
        if (!pProfanity.ContainsKey(profanity.Length))
          pProfanity.Add(profanity.Length, new HashSet<int>());
        if (!pProfanity[profanity.Length].Add(BlacklistTools.getCharHashCode(profanity.ToCharArray())))
          Debug.Log((object) ("Duplicate profanity: " + profanity));
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Error when loading blacklist");
      Debug.LogError((object) ex);
    }
  }

  public static void loadProfanityFilter(Dictionary<string, string[]> pProfanity, int pIndexLength = 3)
  {
    // ISSUE: explicit non-virtual call
    if (pProfanity != null && __nonvirtual (pProfanity.Count) > 0)
      return;
    try
    {
      Dictionary<string, HashSet<string>> dictionary = new Dictionary<string, HashSet<string>>();
      foreach (string profanity in BlacklistTools.getProfanities())
      {
        string key = profanity.Substring(0, pIndexLength);
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, new HashSet<string>());
        if (!dictionary[key].Add(profanity))
          Debug.Log((object) ("Duplicate profanity: " + profanity));
      }
      foreach (KeyValuePair<string, HashSet<string>> keyValuePair in dictionary)
        pProfanity.Add(keyValuePair.Key, keyValuePair.Value.ToArray<string>());
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Error when loading blacklist");
      Debug.LogError((object) ex);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getCharHashCode(char[] pChar)
  {
    return ((IStructuralEquatable) pChar).GetHashCode((IEqualityComparer) EqualityComparer<char>.Default);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string cleanString(string pString)
  {
    if (string.IsNullOrEmpty(pString))
      return pString;
    char ch = pString[0];
    string str1 = ch.ToString();
    for (int index = 0; index < pString.Length - 1; ++index)
    {
      ch = pString[index];
      if (!ch.Equals(pString[index + 1]))
      {
        string str2 = str1;
        ch = pString[index + 1];
        string str3 = ch.ToString();
        str1 = str2 + str3;
      }
    }
    return str1;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string cleanStringAsSpan(string pString)
  {
    if (string.IsNullOrEmpty(pString))
      return pString;
    ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(pString);
    Span<char> span = stackalloc char[readOnlySpan.Length];
    int num1 = 0;
    ref Span<char> local = ref span;
    int num2 = num1;
    int num3 = num2 + 1;
    local[num2] = readOnlySpan[0];
    for (int index = 1; index < readOnlySpan.Length; ++index)
    {
      if ((int) readOnlySpan[index] != (int) readOnlySpan[index - 1])
        span[num3++] = readOnlySpan[index];
    }
    return new string(Span<char>.op_Implicit(span.Slice(0, num3)));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ReadOnlySpan<char> cleanSpan(ReadOnlySpan<char> pSpan)
  {
    if (pSpan.Length == 0)
      return pSpan;
    Span<char> span = Span<char>.op_Implicit(new char[pSpan.Length]);
    int num1 = 0;
    ref Span<char> local = ref span;
    int num2 = num1;
    int num3 = num2 + 1;
    local[num2] = pSpan[0];
    for (int index = 1; index < pSpan.Length; ++index)
    {
      if ((int) pSpan[index] != (int) pSpan[index - 1])
        span[num3++] = pSpan[index];
    }
    return Span<char>.op_Implicit(span.Slice(0, num3));
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool contains(ReadOnlySpan<char> pText, ReadOnlySpan<char> pSearchPattern)
  {
    if (pSearchPattern.Length == 0)
      return true;
    if (pSearchPattern.Length > pText.Length)
      return false;
    char ch = pSearchPattern[0];
    for (int index1 = 0; index1 <= pText.Length - pSearchPattern.Length; ++index1)
    {
      if ((int) pText[index1] == (int) ch)
      {
        bool flag = true;
        for (int index2 = 1; index2 < pSearchPattern.Length; ++index2)
        {
          if ((int) pText[index1 + index2] != (int) pSearchPattern[index2])
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return true;
      }
    }
    return false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool contains(
    ReadOnlySpan<char> pText,
    ReadOnlySpan<char> pSearchPattern,
    int pStartIndex)
  {
    if (pSearchPattern.Length == 0)
      return true;
    if (pSearchPattern.Length > pText.Length)
      return false;
    char ch = pSearchPattern[0];
    for (int index1 = pStartIndex; index1 <= pText.Length - pSearchPattern.Length; ++index1)
    {
      if ((int) pText[index1] == (int) ch)
      {
        bool flag = true;
        for (int index2 = 1; index2 < pSearchPattern.Length; ++index2)
        {
          if ((int) pText[index1 + index2] != (int) pSearchPattern[index2])
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return true;
      }
    }
    return false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool contains(ref ReadOnlySpan<char> pText, ref ReadOnlySpan<char> pSearchPattern)
  {
    if (pSearchPattern.Length == 0)
      return true;
    if (pSearchPattern.Length > pText.Length)
      return false;
    char ch = pSearchPattern[0];
    for (int index1 = 0; index1 <= pText.Length - pSearchPattern.Length; ++index1)
    {
      if ((int) pText[index1] == (int) ch)
      {
        bool flag = true;
        for (int index2 = 1; index2 < pSearchPattern.Length; ++index2)
        {
          if ((int) pText[index1 + index2] != (int) pSearchPattern[index2])
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return true;
      }
    }
    return false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool contains2(ref ReadOnlySpan<char> pText, ref ReadOnlySpan<char> pSearchPattern)
  {
    int length1 = pSearchPattern.Length;
    if (length1 == 0)
      return true;
    int length2 = pText.Length;
    if (length1 > length2)
      return false;
    int num = 0;
    for (int index = length2 - length1; num <= index; ++num)
    {
      if (MemoryExtensions.SequenceEqual<char>(pText.Slice(num, length1), pSearchPattern))
        return true;
    }
    return false;
  }
}
