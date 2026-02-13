// Decompiled with JetBrains decompiler
// Type: ConsonantSeparator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class ConsonantSeparator
{
  private static HashSet<char> _consonants = new HashSet<char>()
  {
    'b',
    'c',
    'd',
    'f',
    'g',
    'h',
    'j',
    'k',
    'l',
    'm',
    'n',
    'p',
    'q',
    'r',
    's',
    't',
    'v',
    'w',
    'x',
    'z'
  };

  public static void addRandomVowels(StringBuilderPool pString, string[] pPartsToInsert)
  {
    if (pString.Length < 2)
      return;
    pString.ToLowerInvariant();
    int num = pString.LastIndexOfAny(' ', ',') + 2;
    using (ListPool<int> list = new ListPool<int>(pString.Length))
    {
      for (int index = num; index < pString.Length; ++index)
      {
        if (ConsonantSeparator.isConsonant(pString[index - 1]) && ConsonantSeparator.isConsonant(pString[index]))
          list.Add(index);
      }
      if (list.Count == 0)
        return;
      int random1 = OnomasticsLibrary.GetRandom<int>(list);
      string random2 = OnomasticsLibrary.GetRandom<string>(pPartsToInsert);
      pString.Insert(random1, random2);
    }
  }

  public static ListPool<int> findAllConsonants(StringBuilderPool pString, int pStart, int pLength)
  {
    ListPool<int> allConsonants = new ListPool<int>(pLength);
    for (int index = pStart; index < pStart + pLength; ++index)
    {
      if (ConsonantSeparator.isConsonant(pString[index]))
        allConsonants.Add(index);
    }
    return allConsonants;
  }

  public static ListPool<int> findAllSingleConsonants(
    StringBuilderPool pString,
    int pStart,
    int pLength)
  {
    ListPool<int> singleConsonants = new ListPool<int>(pLength);
    for (int index = pStart; index < pStart + pLength; ++index)
    {
      if (ConsonantSeparator.isConsonant(pString[index]) && (index <= 0 || !ConsonantSeparator.isConsonant(pString[index - 1])) && (index >= pString.Length - 1 || !ConsonantSeparator.isConsonant(pString[index + 1])))
        singleConsonants.Add(index);
    }
    return singleConsonants;
  }

  public static bool isConsonant(char pChar)
  {
    pChar = char.ToLowerInvariant(pChar);
    if (ConsonantSeparator._consonants.Contains(pChar))
      return true;
    return char.IsLetter(pChar) && !VowelSeparator.isVowel(pChar);
  }
}
