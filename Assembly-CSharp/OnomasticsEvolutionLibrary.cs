// Decompiled with JetBrains decompiler
// Type: OnomasticsEvolutionLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class OnomasticsEvolutionLibrary : AssetLibrary<OnomasticsEvolutionAsset>
{
  private static readonly char[] _vowels = AssetLibrary<OnomasticsEvolutionAsset>.a<char>('a', 'e', 'i', 'o', 'u', 'y');
  private static readonly char[] _vowels_h = AssetLibrary<OnomasticsEvolutionAsset>.a<char>('a', 'e', 'i', 'o', 'u', 'y', 'h');
  private static readonly char[] _consonants = AssetLibrary<OnomasticsEvolutionAsset>.a<char>('b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z');

  public override void init()
  {
    base.init();
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "k",
      to = "c",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "g",
      to = "k",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "f",
      to = "v",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "g",
      to = "gh",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "f",
      to = "gh",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "v",
      to = "b",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ci",
      to = "z",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_not_in_start)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "s",
      to = "z",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "t",
      to = "d",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ks",
      to = "x",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_not_in_start)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "th",
      to = "f",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "th",
      to = "d",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "thi",
      to = "ti",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "er",
      to = "ar",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "sh",
      to = "sch",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "sh",
      to = "sz",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ch",
      to = "cz",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "mm",
      to = "m",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "mn",
      to = "m",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "gg",
      to = "g",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_not_in_start)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "nn",
      to = "n",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "mm",
      to = "hm",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ph",
      to = "f",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ph",
      to = "pp",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "p",
      to = "pp",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ck",
      to = "k",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_not_in_start)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ck",
      to = "gg",
      not_surrounded_by = OnomasticsEvolutionLibrary._consonants,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_not_in_start)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "e",
      to = "ai",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "e",
      to = "a",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "o",
      to = "a",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "u",
      to = "y",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ie",
      to = "y",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace_in_end)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "i",
      to = "y",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "y",
      to = "oe",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "e",
      to = "ae",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "a",
      to = "au",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "o",
      to = "au",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oo",
      to = "ou",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oo",
      to = "ue",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oo",
      to = "oa",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oo",
      to = "u",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ee",
      to = "i",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ie",
      to = "e",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ei",
      to = "ee",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ea",
      to = "ee",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ea",
      to = "ei",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ear",
      to = "ere",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "re",
      to = "ru",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "sp",
      to = "shp",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "sht",
      to = "st",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "u",
      to = "oe",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "o",
      to = "oe",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oh",
      to = "oe",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ah",
      to = "ae",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oe",
      to = "u",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oe",
      to = "oh",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ae",
      to = "ah",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ah",
      to = "oh",
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "a",
      to = "ah",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "o",
      to = "oh",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "u",
      to = "uh",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels_h,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "ah",
      to = "a",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "oh",
      to = "o",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
    this.add(new OnomasticsEvolutionAsset()
    {
      from = "uh",
      to = "u",
      not_surrounded_by = OnomasticsEvolutionLibrary._vowels,
      reverse = false,
      replacer = new OnomasticsReplacerDelegate(OnomasticsEvolutionLibrary.replace)
    });
  }

  public static bool replace(OnomasticsEvolutionAsset pAsset, ref string pReplace)
  {
    if (!pReplace.Contains(pAsset.from))
      return false;
    int random = pReplace.AllIndexesOf(pAsset.from).GetRandom<int>();
    char pValue1 = random > 0 ? pReplace[random - 1] : ' ';
    char pValue2 = random + pAsset.from.Length < pReplace.Length ? pReplace[random + pAsset.from.Length] : ' ';
    char ch1 = pAsset.to.First();
    char ch2 = pAsset.to.Last();
    if ((int) pValue1 == (int) ch2 || (int) pValue1 == (int) ch1 || (int) pValue2 == (int) ch1 || (int) pValue2 == (int) ch2)
      return false;
    char[] notSurroundedBy1 = pAsset.not_surrounded_by;
    if ((notSurroundedBy1 != null ? (notSurroundedBy1.Contains<char>(pValue1) ? 1 : 0) : 0) != 0)
      return false;
    char[] notSurroundedBy2 = pAsset.not_surrounded_by;
    if ((notSurroundedBy2 != null ? (notSurroundedBy2.Contains<char>(pValue2) ? 1 : 0) : 0) != 0)
      return false;
    pReplace = pReplace.Remove(random, pAsset.from.Length).Insert(random, pAsset.to);
    return true;
  }

  public static bool replace_in_end(OnomasticsEvolutionAsset pAsset, ref string pReplace)
  {
    if (!pReplace.Contains(pAsset.from))
      return false;
    int startIndex = pReplace.LastIndexOf(pAsset.from);
    if (startIndex + pAsset.from.Length != pReplace.Length)
      return false;
    char pValue1 = startIndex > 0 ? pReplace[startIndex - 1] : ' ';
    char pValue2 = startIndex + pAsset.from.Length < pReplace.Length ? pReplace[startIndex + pAsset.from.Length] : ' ';
    char ch1 = pAsset.to.First();
    char ch2 = pAsset.to.Last();
    if ((int) pValue1 == (int) ch2 || (int) pValue1 == (int) ch1 || (int) pValue2 == (int) ch1 || (int) pValue2 == (int) ch2)
      return false;
    char[] notSurroundedBy1 = pAsset.not_surrounded_by;
    if ((notSurroundedBy1 != null ? (notSurroundedBy1.Contains<char>(pValue1) ? 1 : 0) : 0) != 0)
      return false;
    char[] notSurroundedBy2 = pAsset.not_surrounded_by;
    if ((notSurroundedBy2 != null ? (notSurroundedBy2.Contains<char>(pValue2) ? 1 : 0) : 0) != 0)
      return false;
    pReplace = pReplace.Remove(startIndex, pAsset.from.Length).Insert(startIndex, pAsset.to);
    return true;
  }

  public static bool replace_not_in_start(OnomasticsEvolutionAsset pAsset, ref string pReplace)
  {
    return pReplace.Contains(pAsset.from) && pReplace.IndexOf(pAsset.from) != 0 && OnomasticsEvolutionLibrary.replace(pAsset, ref pReplace);
  }

  public override OnomasticsEvolutionAsset add(OnomasticsEvolutionAsset pAsset)
  {
    pAsset.id = $"{pAsset.from}_{pAsset.to}";
    this.t = base.add(pAsset);
    if (!this.t.reverse)
      return this.t;
    OnomasticsEvolutionAsset pAsset1 = new OnomasticsEvolutionAsset();
    pAsset1.id = $"{this.t.to}_{this.t.from}";
    pAsset1.from = this.t.to;
    pAsset1.to = this.t.from;
    pAsset1.not_surrounded_by = this.t.not_surrounded_by;
    pAsset1.replacer = this.t.replacer;
    pAsset1.reverse = false;
    return this.add(pAsset1);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (OnomasticsEvolutionAsset pEvolution in this.list)
    {
      OnomasticsEvolver.add(pEvolution);
      if (pEvolution.from.Length >= pEvolution.to.Length)
      {
        OnomasticsEvolver.add(pEvolution);
        OnomasticsEvolver.add(pEvolution);
      }
      if (pEvolution.from.Length > pEvolution.to.Length)
      {
        OnomasticsEvolver.add(pEvolution);
        OnomasticsEvolver.add(pEvolution);
      }
    }
    OnomasticsEvolver.shuffle();
  }
}
