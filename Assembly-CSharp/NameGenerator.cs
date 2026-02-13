// Decompiled with JetBrains decompiler
// Type: NameGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityPools;

#nullable disable
public class NameGenerator
{
  private static int _current_consonants = 0;
  private static int _current_vowels = 0;
  private static readonly char[] vowels_all = new char[6]
  {
    'a',
    'e',
    'i',
    'o',
    'u',
    'y'
  };
  private static readonly char[] consonants_all = new char[20]
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
  private static bool _initiated = false;
  [ThreadStatic]
  private static Dictionary<string, ListPool<string>> _dict_splitted_items;

  public static void init()
  {
    if (NameGenerator._initiated)
      return;
    NameGenerator._initiated = true;
    Blacklist.init();
  }

  public static string generateName(Actor pActor, MetaType pType, long pSeed, ActorSex pSex = ActorSex.None)
  {
    string pAssetID = (string) null;
    int hashCode = pType.GetHashCode();
    pSeed += (long) hashCode;
    if (pActor.hasCulture())
    {
      OnomasticsData onomasticData = pActor.culture.getOnomasticData(pType);
      if (onomasticData != null)
        return onomasticData.generateName(pSex, pSeed: new long?(pSeed));
      pAssetID = pActor.culture.getNameTemplate(pType);
    }
    else
    {
      foreach (Actor parent in pActor.getParents())
      {
        if (parent.hasCulture())
        {
          OnomasticsData onomasticData = parent.culture.getOnomasticData(pType);
          if (onomasticData != null)
            return onomasticData.generateName(pSex, pSeed: new long?(pSeed));
          pAssetID = parent.culture.getNameTemplate(pType);
          break;
        }
      }
    }
    if (string.IsNullOrEmpty(pAssetID))
      pAssetID = pActor.asset.getNameTemplate(pType);
    return NameGenerator.getName(pAssetID, pSex, pSeed: new long?(pSeed));
  }

  public static string getName(
    string pAssetID,
    ActorSex pSex = ActorSex.Male,
    bool pForceLegacy = false,
    string pTemplate = null,
    long? pSeed = null,
    bool pIgnoreBlackList = false)
  {
    NameGenerator.init();
    NameGeneratorAsset pAsset = AssetManager.name_generator.get(pAssetID);
    NameGenerator._current_consonants = 0;
    NameGenerator._current_vowels = 0;
    string nameFromTemplate = NameGenerator.generateNameFromTemplate(pAsset, pForceLegacy: pForceLegacy, pOnomasticsTemplate: pTemplate, pSeed: pSeed, pSex: pSex, pIgnoreBlacklist: pIgnoreBlackList);
    if (!pAsset.hasOnomastics() && pSex == ActorSex.Female)
    {
      string strB = nameFromTemplate.Substring(nameFromTemplate.Length - 1, 1);
      bool flag = false;
      foreach (string vowel in pAsset.vowels)
      {
        if (vowel.CompareTo(strB) == 0)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        nameFromTemplate += Randy.getRandom<string>(pAsset.vowels);
    }
    return nameFromTemplate;
  }

  private static string firstToUpper(string pString) => pString.FirstToUpper();

  private static string addVowel(string[] pList, bool pUppercase = false)
  {
    NameGenerator._current_consonants = 0;
    ++NameGenerator._current_vowels;
    return pUppercase ? NameGenerator.firstToUpper(Randy.getRandom<string>(pList)) : Randy.getRandom<string>(pList);
  }

  private static string addEnding(NameGeneratorAsset pTemplate, string pName)
  {
    string str = Randy.getRandom<string>(pTemplate.parts);
    if (NameGenerator.isConsonant(str[0]) && NameGenerator.isConsonant(pName[pName.Length - 1]))
      str = NameGenerator.addVowel(pTemplate.vowels) + str;
    else if (!NameGenerator.isConsonant(str[0]) && !NameGenerator.isConsonant(pName[pName.Length - 1]))
      str = NameGenerator.addConsonant(pTemplate.consonants) + str;
    return str;
  }

  private static string addConsonant(string[] pList, bool pUppercase = false)
  {
    ++NameGenerator._current_consonants;
    NameGenerator._current_vowels = 0;
    return pUppercase ? NameGenerator.firstToUpper(Randy.getRandom<string>(pList)) : Randy.getRandom<string>(pList);
  }

  private static string addPart(string[] pArray, bool pUppercase = false)
  {
    string pString = Randy.getRandom<string>(pArray);
    if (NameGenerator.isConsonant(pString[pString.Length - 1]))
    {
      ++NameGenerator._current_consonants;
      NameGenerator._current_vowels = 0;
    }
    else
    {
      NameGenerator._current_consonants = 0;
      ++NameGenerator._current_vowels;
    }
    if (pUppercase)
      pString = NameGenerator.firstToUpper(pString);
    return pString;
  }

  private static bool isConsonant(char pChar)
  {
    return NameGenerator.consonants_all.IndexOf<char>(pChar) > -1;
  }

  private static bool isVowel(char pChar) => NameGenerator.vowels_all.IndexOf<char>(pChar) > -1;

  public static string generateNameFromTemplate(
    string pAssetID,
    Actor pActor = null,
    Kingdom pKingdom = null,
    bool pForceLegacy = false)
  {
    return NameGenerator.generateNameFromTemplate(AssetManager.name_generator.get(pAssetID), pActor, pKingdom, pForceLegacy);
  }

  public static string generateNameFromOnomastics(
    NameGeneratorAsset pAsset,
    string pTemplate = null,
    Actor pActor = null,
    long? pSeed = null,
    ActorSex pSex = ActorSex.None)
  {
    OnomasticsData onomasticsData = string.IsNullOrEmpty(pTemplate) ? OnomasticsCache.getOriginalData(pAsset.onomastics_templates.GetRandom<string>()) : OnomasticsCache.getOriginalData(pTemplate);
    ActorSex pSex1 = pSex;
    if (pActor != null)
      pSex1 = pActor.data.sex;
    return onomasticsData.generateName(pSex1, pSeed: pSeed);
  }

  public static string generateNamesFromTemplate(
    int pAmount,
    NameGeneratorAsset pAsset,
    Actor pActor = null,
    Kingdom pKingdom = null,
    bool pForceLegacy = false,
    bool pTestReplacer = false)
  {
    string namesFromTemplate = "";
    HashSet<string> collection = new HashSet<string>();
    List<string> list = new List<string>();
    if (pAsset.hasOnomastics() && !pForceLegacy)
    {
      foreach (string onomasticsTemplate in pAsset.onomastics_templates)
      {
        collection.Clear();
        list.Clear();
        for (int index = 0; index < 100; ++index)
          collection.Add(NameGenerator.generateNameFromTemplate(pAsset, pActor, pKingdom, pOnomasticsTemplate: onomasticsTemplate, pTestReplacer: pTestReplacer));
        namesFromTemplate = $"{namesFromTemplate}\n--- {onomasticsTemplate}";
        string[] strArray = new string[6]
        {
          namesFromTemplate,
          "\n -- (",
          null,
          null,
          null,
          null
        };
        int num = collection.Count;
        strArray[2] = num.ToString();
        strArray[3] = " / ";
        num = 100;
        strArray[4] = num.ToString();
        strArray[5] = ") \n";
        namesFromTemplate = string.Concat(strArray);
        list.AddRange((IEnumerable<string>) collection);
        list.Shuffle<string>();
        if (list.Count - pAmount > 0)
          list.RemoveRange(pAmount, list.Count - pAmount);
        list.Sort();
        foreach (string str in list)
          namesFromTemplate = $"{namesFromTemplate}{str}\n";
      }
    }
    else
    {
      for (int index = 0; index < 100; ++index)
        collection.Add(NameGenerator.generateNameFromTemplate(pAsset, pActor, pKingdom, pForceLegacy, pTestReplacer: pTestReplacer));
      namesFromTemplate = $"{namesFromTemplate + "\n--- Legacy"}\n -- ({collection.Count.ToString()} / {100.ToString()}) \n";
      list.AddRange((IEnumerable<string>) collection);
      list.Shuffle<string>();
      if (list.Count - pAmount > 0)
        list.RemoveRange(pAmount, list.Count - pAmount);
      list.Sort();
      foreach (string str in list)
        namesFromTemplate = $"{namesFromTemplate}{str}\n";
    }
    return namesFromTemplate;
  }

  public static string generateNameFromTemplate(
    NameGeneratorAsset pAsset,
    Actor pActor = null,
    Kingdom pKingdom = null,
    bool pForceLegacy = false,
    int pCalls = 0,
    string pOnomasticsTemplate = null,
    string[] pClassicTemplate = null,
    bool pTestReplacer = false,
    long? pSeed = null,
    ActorSex pSex = ActorSex.None,
    bool pIgnoreBlacklist = false)
  {
    if (pCalls > 50)
      return string.Empty;
    if (pAsset.hasOnomastics() && !pForceLegacy)
      return NameGenerator.generateNameFromOnomastics(pAsset, pOnomasticsTemplate, pActor, pSeed, pSex);
    NameGenerator._current_consonants = 0;
    NameGenerator._current_vowels = 0;
    string pName = "";
    string[] strArray1 = pClassicTemplate ?? pAsset.templates.GetRandom<string[]>();
    bool flag1 = false;
    foreach (string str1 in strArray1)
    {
      string key;
      string str2;
      if (str1.Contains('#'))
      {
        string[] strArray2 = str1.Split('#', StringSplitOptions.None);
        key = strArray2[0];
        str2 = strArray2[1];
      }
      else
      {
        key = str1;
        str2 = "";
      }
      if (pAsset.use_dictionary)
      {
        if (key == "$comma$")
        {
          pName += ", ";
        }
        else
        {
          if (key.Contains(';'))
            key = key.Split(';', StringSplitOptions.None).GetRandom<string>();
          Dictionary<string, ListPool<string>> dictSplittedItems = NameGenerator._dict_splitted_items;
          // ISSUE: explicit non-virtual call
          if ((dictSplittedItems != null ? (__nonvirtual (dictSplittedItems.ContainsKey(key)) ? 1 : 0) : 0) == 0)
          {
            if (NameGenerator._dict_splitted_items == null)
              NameGenerator._dict_splitted_items = UnsafeCollectionPool<Dictionary<string, ListPool<string>>, KeyValuePair<string, ListPool<string>>>.Get();
            ListPool<string> listPool = new ListPool<string>(pAsset.dict_parts[key].Split(',', StringSplitOptions.None));
            NameGenerator._dict_splitted_items.Add(key, listPool);
          }
          NameGenerator._dict_splitted_items[key].ShuffleLast<string>();
          string str3 = NameGenerator._dict_splitted_items[key].Last<string>();
          if (NameGenerator._dict_splitted_items[key].Count > 1)
            NameGenerator._dict_splitted_items[key].Pop<string>();
          pName += str3;
        }
      }
      else
      {
        switch (key)
        {
          case " ":
          case "space":
            pName += " ";
            continue;
          case "CONSONANT":
            pName += NameGenerator.addConsonant(pAsset.consonants, true);
            continue;
          case "Letters":
            string[] strArray3 = str2.Split('-', StringSplitOptions.None);
            pName += NameGenerator.addWord(pAsset, int.Parse(strArray3[0]), int.Parse(strArray3[1]), true);
            continue;
          case "Part":
            string upper = NameGenerator.firstToUpper(pAsset.parts.GetRandom<string>());
            pName += upper;
            continue;
          case "Part_group":
            bool flag2 = true;
            using (List<string>.Enumerator enumerator = pAsset.part_groups.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string[] pArray = enumerator.Current.Split(',', StringSplitOptions.None);
                if (flag2)
                {
                  pName += NameGenerator.firstToUpper(pArray.GetRandom<string>());
                  flag2 = false;
                }
                else
                  pName += pArray.GetRandom<string>();
              }
              continue;
            }
          case "Part_group2":
            bool flag3 = true;
            using (List<string>.Enumerator enumerator = pAsset.part_groups2.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string[] pArray = enumerator.Current.Split(',', StringSplitOptions.None);
                if (flag3)
                {
                  pName += NameGenerator.firstToUpper(pArray.GetRandom<string>());
                  flag3 = false;
                }
                else
                  pName += pArray.GetRandom<string>();
              }
              continue;
            }
          case "Part_group3":
            bool flag4 = true;
            using (List<string>.Enumerator enumerator = pAsset.part_groups3.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string[] pArray = enumerator.Current.Split(',', StringSplitOptions.None);
                if (flag4)
                {
                  pName += NameGenerator.firstToUpper(pArray.GetRandom<string>());
                  flag4 = false;
                }
                else
                  pName += pArray.GetRandom<string>();
              }
              continue;
            }
          case "RANDOM_LETTER":
            pName = !Randy.randomBool() ? pName + NameGenerator.addConsonant(pAsset.consonants, true) : pName + NameGenerator.addVowel(pAsset.vowels, true);
            continue;
          case "VOWEL":
            pName += NameGenerator.addVowel(pAsset.vowels, true);
            continue;
          case "addition_ending":
            if (!flag1 && Randy.randomChance(pAsset.add_addition_chance))
            {
              pName = $"{pName} {pAsset.addition_ending.GetRandom<string>()}";
              flag1 = true;
              continue;
            }
            continue;
          case "addition_start":
            if (!flag1 && Randy.randomChance(pAsset.add_addition_chance))
            {
              pName = $"{pName}{pAsset.addition_start.GetRandom<string>()} ";
              flag1 = true;
              continue;
            }
            continue;
          case "consonant":
            pName += NameGenerator.addConsonant(pAsset.consonants);
            continue;
          case "letters":
            string[] strArray4 = str2.Split('-', StringSplitOptions.None);
            pName += NameGenerator.addWord(pAsset, int.Parse(strArray4[0]), int.Parse(strArray4[1]));
            continue;
          case "number":
            pName += Randy.randomInt(0, 10).ToString();
            continue;
          case "part":
            pName += pAsset.parts.GetRandom<string>();
            continue;
          case "part_group":
            using (List<string>.Enumerator enumerator = pAsset.part_groups.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string[] pArray = enumerator.Current.Split(',', StringSplitOptions.None);
                pName += pArray.GetRandom<string>();
              }
              continue;
            }
          case "part_group2":
            using (List<string>.Enumerator enumerator = pAsset.part_groups2.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string[] pArray = enumerator.Current.Split(',', StringSplitOptions.None);
                pName += pArray.GetRandom<string>();
              }
              continue;
            }
          case "part_group3":
            using (List<string>.Enumerator enumerator = pAsset.part_groups3.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string[] pArray = enumerator.Current.Split(',', StringSplitOptions.None);
                pName += pArray.GetRandom<string>();
              }
              continue;
            }
          case "removalchance":
            if (Randy.randomBool())
            {
              pName.Remove(pName.Length - 1);
              continue;
            }
            continue;
          case "special1":
            pName += pAsset.special1.GetRandom<string>();
            continue;
          case "special2":
            pName += pAsset.special2.GetRandom<string>();
            continue;
          case "vowel":
            pName += NameGenerator.addVowel(pAsset.vowels);
            continue;
          case "vowelchance":
            if (Randy.randomBool())
            {
              pName += NameGenerator.addVowel(pAsset.vowels);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    if (pName.Contains('$'))
    {
      if (pTestReplacer)
      {
        NameGeneratorReplacers.replacer_debug(ref pName);
      }
      else
      {
        if (pAsset.replacer != null)
          pAsset.replacer(ref pName, pActor);
        if (pAsset.replacer_kingdom != null)
          pAsset.replacer_kingdom(ref pName, pKingdom);
      }
    }
    bool flag5 = false;
    if (string.IsNullOrEmpty(pName))
      flag5 = true;
    else if (!pAsset.use_dictionary && !pIgnoreBlacklist && Blacklist.checkBlackList(pName))
      flag5 = true;
    if (flag5)
      return NameGenerator.generateNameFromTemplate(pAsset, pActor, pKingdom, pForceLegacy, ++pCalls, pOnomasticsTemplate, pClassicTemplate, pTestReplacer);
    pName = NameGenerator.firstToUpper(pName);
    if (pAsset.finalizer != null)
      pName = pAsset.finalizer(pName);
    if (NameGenerator._dict_splitted_items != null)
    {
      foreach (ListPool<string> listPool in NameGenerator._dict_splitted_items.Values)
        listPool.Dispose();
      NameGenerator._dict_splitted_items.Clear();
      UnsafeCollectionPool<Dictionary<string, ListPool<string>>, KeyValuePair<string, ListPool<string>>>.Release(NameGenerator._dict_splitted_items);
      NameGenerator._dict_splitted_items = (Dictionary<string, ListPool<string>>) null;
    }
    return pName;
  }

  private static string addWord(NameGeneratorAsset pAsset, int pMin, int pMax, bool pToUpperFirst = false)
  {
    string str = "";
    int num = Randy.randomInt(pMin, pMax);
    for (int index = 0; index < num; ++index)
    {
      if (NameGenerator._current_consonants >= pAsset.max_consonants_in_row)
      {
        str += NameGenerator.addVowel(pAsset.vowels, pToUpperFirst);
        pToUpperFirst = false;
      }
      else if (NameGenerator._current_vowels >= pAsset.max_vowels_in_row)
      {
        str += NameGenerator.addConsonant(pAsset.consonants, pToUpperFirst);
        pToUpperFirst = false;
      }
      else if (Randy.randomBool())
      {
        str += NameGenerator.addVowel(pAsset.vowels, pToUpperFirst);
        pToUpperFirst = false;
      }
      else
      {
        str += NameGenerator.addConsonant(pAsset.consonants, pToUpperFirst);
        pToUpperFirst = false;
      }
    }
    return str;
  }
}
