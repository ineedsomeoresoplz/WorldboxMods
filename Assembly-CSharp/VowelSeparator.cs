// Decompiled with JetBrains decompiler
// Type: VowelSeparator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class VowelSeparator
{
  private const string VOWELS = "aeiouy";
  private const string VOWELS_SPECIAL = "àáâãäåæèéêëìíîïòóôõöøùúûüýÿāăąēĕėęěĩīĭįĳōŏőœũūŭůűųŷǎǐǒǔǖǘǚǜǟǡǣǫǭǻǽǿȁȃȅȇȉȋȍȏȕȗȧȩȫȭȯȱȳеийоуыэюяѐёєіїѝў";
  private static HashSet<char> _vowels = new HashSet<char>((IEnumerable<char>) "aeiouy".ToCharArray());
  private static HashSet<char> _special_vowels = new HashSet<char>((IEnumerable<char>) "àáâãäåæèéêëìíîïòóôõöøùúûüýÿāăąēĕėęěĩīĭįĳōŏőœũūŭůűųŷǎǐǒǔǖǘǚǜǟǡǣǫǭǻǽǿȁȃȅȇȉȋȍȏȕȗȧȩȫȭȯȱȳеийоуыэюяѐёєіїѝў".ToCharArray());

  public static void addRandomConsonants(StringBuilderPool pString, string[] pPartsToInsert)
  {
    if (pString.Length < 2)
      return;
    pString.ToLowerInvariant();
    int num = pString.LastIndexOfAny(' ', ',') + 2;
    using (ListPool<int> list = new ListPool<int>(pString.Length))
    {
      for (int index = num; index < pString.Length; ++index)
      {
        if (VowelSeparator.isVowel(pString[index - 1]) && VowelSeparator.isVowel(pString[index]))
          list.Add(index);
      }
      if (list.Count == 0)
        return;
      int random1 = OnomasticsLibrary.GetRandom<int>(list);
      string random2 = OnomasticsLibrary.GetRandom<string>(pPartsToInsert);
      pString.Insert(random1, random2);
    }
  }

  public static ListPool<int> findAllVowels(StringBuilderPool pString, int pStart, int pLength)
  {
    ListPool<int> allVowels = new ListPool<int>(pLength);
    for (int index = pStart; index < pStart + pLength; ++index)
    {
      if (VowelSeparator.isVowel(pString[index]))
        allVowels.Add(index);
    }
    return allVowels;
  }

  public static ListPool<int> findAllSingleVowels(
    StringBuilderPool pString,
    int pStart,
    int pLength)
  {
    pString.ToLowerInvariant();
    ListPool<int> allSingleVowels = new ListPool<int>(pLength);
    for (int index = pStart; index < pStart + pLength; ++index)
    {
      if (VowelSeparator.isVowel(pString[index]) && (index <= 0 || !VowelSeparator.isVowel(pString[index - 1])) && (index >= pString.Length - 1 || !VowelSeparator.isVowel(pString[index + 1])))
        allSingleVowels.Add(index);
    }
    return allSingleVowels;
  }

  public static bool isVowel(char pChar)
  {
    pChar = char.ToLowerInvariant(pChar);
    if (VowelSeparator._vowels.Contains(pChar))
      return true;
    return char.IsLetter(pChar) && VowelSeparator._special_vowels.Contains(pChar);
  }
}
