// Decompiled with JetBrains decompiler
// Type: WordAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
[Serializable]
public class WordAsset : Asset
{
  public string getLocaleID() => throw new NotImplementedException();

  public string getDescriptionID() => throw new NotImplementedException();

  public string getDescriptionID2() => throw new NotImplementedException();

  public string getWordInLanguage(
    LanguageStructure pStructure,
    LinguisticsAsset pLinguisticsAsset,
    int pSeed)
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      foreach (char ch in this.getWordPattern(pStructure, pSeed))
      {
        string str1;
        switch (ch)
        {
          case 'E':
            str1 = pStructure.syllables_ends.GetRandom<string>();
            break;
          case 'M':
            str1 = pStructure.syllables_mid.GetRandom<string>();
            break;
          case 'S':
            str1 = pStructure.syllables_start.GetRandom<string>();
            break;
          default:
            str1 = "";
            break;
        }
        string str2 = str1;
        stringBuilderPool.Append(str2);
      }
      string wordInLanguage = stringBuilderPool.ToString();
      if (pLinguisticsAsset.word_type != WordType.None)
      {
        int wordType = (int) pLinguisticsAsset.word_type;
        PrefixesSettings settingsPrefixes = pStructure.settings_prefixes;
        SuffixesSettings settingsSuffixes = pStructure.settings_suffixes;
        if (settingsPrefixes.enabled[wordType])
          wordInLanguage = settingsPrefixes.sets[wordType].GetRandom<string>() + settingsPrefixes.separator[wordType] + wordInLanguage;
        if (settingsSuffixes.enabled[wordType])
          wordInLanguage = wordInLanguage + settingsSuffixes.separator[wordType] + settingsSuffixes.sets[wordType].GetRandom<string>();
      }
      return wordInLanguage;
    }
  }

  private string getWordPattern(LanguageStructure pStructure, int pSeed)
  {
    return this.selectWeightedPattern(pStructure.word_patterns, pStructure.word_weights);
  }

  private string selectWeightedPattern(string[] pPattern, float[] pWeight)
  {
    float num1 = Randy.random();
    float num2 = 0.0f;
    for (int index = 0; index < pPattern.Length; ++index)
    {
      num2 += pWeight[index];
      if ((double) num1 < (double) num2)
        return pPattern[index];
    }
    return pPattern.Last<string>();
  }

  private string fixWordBoundaries(string pWord)
  {
    return string.IsNullOrEmpty(pWord) ? pWord : Regex.Replace(Regex.Replace(Regex.Replace(pWord, "([bcdfghjklmnpqrstvwxyz])\\1{2,}", "$1$1"), "([aeiou])\\1+", (MatchEvaluator) (m =>
    {
      string str = m.Value;
      return str == "ee" || str == "oo" || str == "aa" ? str.Substring(0, 2) : m.Groups[1].Value;
    })), "([bdgkpt])([bdgkpt])", "$1").Replace("tst", "st").Replace("ndn", "nd").Replace("ckc", "ck");
  }
}
