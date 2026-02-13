// Decompiled with JetBrains decompiler
// Type: SuffixesSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class SuffixesSettings : StructureSettings
{
  public override void create(LanguageStructure pStructure, int pSizeMin, int pSizeMax)
  {
    foreach (WordType wordType in LanguageStructureHelpers.word_types)
      this.generate(pStructure, wordType, pSizeMin, pSizeMax);
  }

  public void generate(LanguageStructure pStructure, WordType pWord, int pSizeMin, int pSizeMax)
  {
    int pAmount = Randy.randomInt(pSizeMin, pSizeMax);
    int index = (int) pWord;
    bool flag = pAmount != 0 && Randy.randomBool();
    this.enabled[index] = flag;
    if (!flag)
      return;
    this.sets[index] = this.generateSets(pStructure, pAmount);
    this.separator[index] = "";
  }

  private string[] generateSets(LanguageStructure pStructure, int pAmount)
  {
    string[] sets = new string[pAmount];
    for (int index = 0; index < pAmount; ++index)
    {
      string str;
      switch (Randy.randomInt(0, 5))
      {
        case 0:
          str = pStructure.sets_consonants.GetRandom<string>() + pStructure.sets_vowels.GetRandom<string>();
          break;
        case 1:
          str = pStructure.sets_onset_2.GetRandom<string>() + pStructure.sets_vowels.GetRandom<string>();
          break;
        case 2:
          str = pStructure.sets_consonants.GetRandom<string>() + pStructure.sets_vowels.GetRandom<string>();
          break;
        default:
          str = pStructure.sets_vowels.GetRandom<string>() + pStructure.sets_consonants.GetRandom<string>();
          break;
      }
      sets[index] = str;
    }
    return sets;
  }
}
