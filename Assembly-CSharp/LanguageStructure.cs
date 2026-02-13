// Decompiled with JetBrains decompiler
// Type: LanguageStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
[Serializable]
public class LanguageStructure
{
  public string[] sets_vowels;
  public string[] sets_consonants;
  public string[] sets_onset_1;
  public string[] sets_onset_2;
  public string[] sets_codas_1;
  public string[] sets_codas_2;
  public string[] sets_diphthongs;
  public string[] syllables_start;
  public string[] syllables_mid;
  public string[] syllables_ends;
  public string[] word_patterns;
  public float[] word_weights;
  public ArticleSettings settings_articles;
  public PrefixesSettings settings_prefixes;
  public SuffixesSettings settings_suffixes;

  public LanguageStructure() => this.generateSyllableSets();

  public void generateSyllableSets()
  {
    if (this.syllables_start != null)
      return;
    this.generateMainParts();
    this.generatePatterns();
    int pSizeMin = Randy.randomInt(1, 2);
    int pSizeMax = Randy.randomInt(1, 3);
    this.settings_articles = new ArticleSettings();
    this.settings_articles.create(this, pSizeMin, pSizeMax);
    this.settings_prefixes = new PrefixesSettings();
    this.settings_prefixes.create(this, 0, 4);
    this.settings_suffixes = new SuffixesSettings();
    this.settings_suffixes.create(this, 0, 4);
    this.syllables_start = this.generateSyllables("syllable_starts", Randy.randomInt(2, 10));
    this.syllables_mid = this.generateSyllables("syllable_mids", Randy.randomInt(2, 10));
    this.syllables_ends = this.generateSyllables("syllable_ends", Randy.randomInt(2, 10));
  }

  private void generatePatterns()
  {
    int length = Randy.randomInt(3, 10);
    this.word_patterns = new string[length];
    this.word_weights = new float[length];
    for (int index = 0; index < length; ++index)
    {
      this.word_patterns[index] = LanguageStructureHelpers.possible_word_patterns.GetRandom<string>();
      this.word_weights[index] = Randy.randomFloat(0.05f, 1f);
    }
  }

  private void generateMainParts()
  {
    this.sets_consonants = this.generateParts("consonant", 5);
    this.sets_vowels = this.generateParts("vowel", 5);
    this.sets_onset_1 = this.generateParts("onset1", 5);
    this.sets_onset_2 = this.generateParts("onset2", 5);
    this.sets_codas_1 = this.generateParts("coda1", 5);
    this.sets_codas_2 = this.generateParts("coda2", 5);
    this.sets_diphthongs = this.generateParts("diphthongs", 5);
  }

  private string[] generateParts(string pID, int pAmount)
  {
    LinguisticsAsset linguisticsAsset = AssetManager.linguistics_library.get(pID);
    string[] parts = new string[pAmount];
    for (int index = 0; index < pAmount; ++index)
      parts[index] = linguisticsAsset.getRandom();
    return parts;
  }

  private string[] generateSyllables(string pID, int pAmount)
  {
    int length = pAmount;
    string[] syllables = new string[length];
    LinguisticsAsset linguisticsAsset = AssetManager.linguistics_library.get(pID);
    for (int index = 0; index < length; ++index)
    {
      string str1 = string.Join("", linguisticsAsset.getRandomPattern());
      using (new StringBuilderPool())
      {
        string str2 = string.Empty;
        string empty = string.Empty;
        string str3 = string.Empty;
        if (str1.StartsWith("CC"))
          str2 = this.sets_onset_2.GetRandom<string>();
        else if (str1.StartsWith("C"))
          str2 = this.sets_onset_1.GetRandom<string>();
        string str4 = (str1.Contains("VV") ? 1 : (Randy.randomChance(0.2f) ? 1 : 0)) != 0 ? this.sets_diphthongs.GetRandom<string>() : this.sets_vowels.GetRandom<string>();
        if (str1.EndsWith("CC"))
          str3 = this.sets_codas_2.GetRandom<string>();
        else if (str1.EndsWith("C"))
          str3 = this.sets_codas_1.GetRandom<string>();
        string str5 = str2 + str4 + str3;
        syllables[index] = str5;
      }
    }
    return syllables;
  }

  private string fixOrthography(string pSyllable)
  {
    if (string.IsNullOrEmpty(pSyllable))
      return pSyllable;
    string input = Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(pSyllable, "([bcdfghjklmnpqrstvwxyz])\\1{2,}", "$1$1").Replace("ck", "ck").Replace("kk", "ck").Replace("cc", "ck"), "qw|qv", "qu").Replace("q", "qu"), "aa+", "a"), "ii+", "i"), "uu+", "u");
    if (input.StartsWith("x"))
      input = "z" + input.Substring(1);
    string str1 = Regex.Replace(input, "([bcdfghjklmnpqrstvwxyz])\\1\\1+", "$1$1").Replace("tch", "ch").Replace("dge", "ge");
    if (str1.Length > 2)
    {
      string lower = str1.Substring(0, 2).ToLower();
      string[] strArray = new string[7]
      {
        "kg",
        "pn",
        "gn",
        "kn",
        "wr",
        "mn",
        "ps"
      };
      foreach (string str2 in strArray)
      {
        if (lower == str2)
        {
          str1 = str1.Substring(1);
          break;
        }
      }
    }
    return str1;
  }
}
