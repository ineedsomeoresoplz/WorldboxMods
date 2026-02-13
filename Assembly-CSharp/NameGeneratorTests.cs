// Decompiled with JetBrains decompiler
// Type: NameGeneratorTests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityPools;

#nullable disable
public static class NameGeneratorTests
{
  private static string _test_string;

  public static void runTests()
  {
  }

  public static string testAllNamesForUniqueness()
  {
    string pResults = "";
    foreach (NameGeneratorAsset pAsset in AssetManager.name_generator.list)
    {
      HashSet<string> stringSet = new HashSet<string>();
      for (int index = 0; index < 1000; ++index)
      {
        string nameFromTemplate = NameGenerator.generateNameFromTemplate(pAsset);
        if (!stringSet.Contains(nameFromTemplate))
          stringSet.Add(nameFromTemplate);
      }
      pResults = $"{pResults}Unique names for asset {pAsset.id}: {stringSet.Count.ToString()}\n";
    }
    return NameGeneratorTests.writeResults("name_test3_uniq", pResults);
  }

  public static string testAllNamesOutput()
  {
    string pResults = "";
    foreach (NameGeneratorAsset pAsset in AssetManager.name_generator.list)
    {
      pResults = $"{pResults}\n--- asset name: {pAsset.id} ---\n";
      pResults = $"{pResults}{NameGenerator.generateNamesFromTemplate(20, pAsset, pTestReplacer: true)}\n";
    }
    return NameGeneratorTests.writeResults("name_test3", pResults);
  }

  public static string testNamesAlliances()
  {
    NameGeneratorTests.testNameStart();
    NameGeneratorTests.testName("alliance_name");
    return NameGeneratorTests.testNameEnd();
  }

  public static string testNamesWars()
  {
    NameGeneratorTests.testNameStart();
    NameGeneratorTests.testName("war_conquest");
    NameGeneratorTests.testName("war_rebellion");
    NameGeneratorTests.testName("war_spite");
    NameGeneratorTests.testName("war_inspire");
    NameGeneratorTests.testName("war_whisper");
    return NameGeneratorTests.testNameEnd();
  }

  public static string testNamesItems()
  {
    NameGeneratorTests.testNameStart();
    NameGeneratorTests.testName("boots_name");
    NameGeneratorTests.testName("armor_name");
    NameGeneratorTests.testName("helmet_name");
    NameGeneratorTests.testName("ring_name");
    NameGeneratorTests.testName("amulet_name");
    return NameGeneratorTests.testNameEnd();
  }

  public static string testNamesWeapons()
  {
    NameGeneratorTests.testNameStart();
    NameGeneratorTests.testName("sword_name");
    NameGeneratorTests.testName("axe_name");
    NameGeneratorTests.testName("hammer_name");
    NameGeneratorTests.testName("stick_name");
    NameGeneratorTests.testName("blaster_name");
    NameGeneratorTests.testName("spear_name");
    NameGeneratorTests.testName("bow_name");
    NameGeneratorTests.testName("flame_sword_name");
    NameGeneratorTests.testName("necromancer_staff_name");
    NameGeneratorTests.testName("evil_staff_name");
    NameGeneratorTests.testName("white_staff_name");
    NameGeneratorTests.testName("plague_doctor_staff_name");
    NameGeneratorTests.testName("druid_staff_name");
    return NameGeneratorTests.testNameEnd();
  }

  public static void testNameStart() => NameGeneratorTests._test_string = "";

  public static string testNameEnd()
  {
    return NameGeneratorTests.writeResults("name_test2", NameGeneratorTests._test_string);
  }

  public static void testName(string pID, int pAmount = 20)
  {
    NameGeneratorTests._test_string = $"{NameGeneratorTests._test_string}\n--- {pID}:\n";
    NameGeneratorAsset pAsset = AssetManager.name_generator.get(pID);
    NameGeneratorTests._test_string = $"{NameGeneratorTests._test_string}{NameGenerator.generateNamesFromTemplate(100, pAsset, pTestReplacer: true)}\n";
  }

  public static string testNamesBooks()
  {
    // ISSUE: unable to decompile the method.
  }

  public static string testNamesDefault()
  {
    string str = "" + "\n--- default - legacy:\n";
    for (int index = 0; index < 100; ++index)
      str = $"{str}{NameGenerator.getName("orc_unit", pForceLegacy: true)}\n";
    string pResults = str + "\n--- default_name - onomastics:\n";
    for (int index = 0; index < 100; ++index)
      pResults = $"{pResults}{NameGenerator.getName("orc_unit")}\n";
    return NameGeneratorTests.writeResults("name_test_default", pResults);
  }

  public static string testNamesClans()
  {
    return NameGeneratorTests.writeResults("name_test2", $"{$"{$"{$"{"" + "\n--- human_clan name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_clan"))}\n" + "\n--- elf_clan name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_clan"))}\n" + "\n--- dwarf_clan name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_clan"))}\n" + "\n--- orc_clan name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_clan"))}\n");
  }

  public static string testNamesKingdoms()
  {
    return NameGeneratorTests.writeResults("name_test2", $"{$"{$"{$"{"" + "\n--- human_kingdom name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_kingdom"))}\n" + "\n--- elf_kingdom name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_kingdom"))}\n" + "\n--- dwarf_kingdom name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_kingdom"))}\n" + "\n--- orc_kingdom name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_kingdom"))}\n");
  }

  public static string testNamesCities()
  {
    return NameGeneratorTests.writeResults("name_test2", $"{$"{$"{$"{$"{$"{$"{$"{"" + "\n--- human_city name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_city"))}\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_city"), pForceLegacy: true)}\n" + "\n--- elf_city name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_city"))}\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_city"), pForceLegacy: true)}\n" + "\n--- dwarf_city name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_city"))}\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_city"), pForceLegacy: true)}\n" + "\n--- orc_city name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_city"))}\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_city"), pForceLegacy: true)}\n");
  }

  public static string testNamesCulture()
  {
    return NameGeneratorTests.writeResults("name_test2", $"{$"{$"{$"{"" + "\n--- elf_culture name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_culture"))}\n" + "\n--- dwarf_culture name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_culture"))}\n" + "\n--- orc_culture name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_culture"))}\n" + "\n--- human_culture name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_culture"))}\n");
  }

  public static string testMottos()
  {
    return NameGeneratorTests.writeResults("name_test_mottos", $"{"" + "\n--- Mottos:\n"}{NameGenerator.generateNamesFromTemplate(100, AssetManager.name_generator.get("clan_mottos"))}\n");
  }

  public static string testNames()
  {
    return NameGeneratorTests.writeResults("name_test2", $"{$"{$"{$"{$"{$"{$"{$"{$"{$"{$"{$"{"" + "\n--- elf name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_unit"))}\n" + "\n--- elf City:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_city"))}\n" + "\n--- elf Kingdom:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("elf_kingdom"))}\n" + "\n--- dwarf name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_unit"))}\n" + "\n--- dwarf City:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_city"))}\n" + "\n--- dwarf Kingdom:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("dwarf_kingdom"))}\n" + "\n--- orc name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_unit"))}\n" + "\n--- orc City:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_city"))}\n" + "\n--- orc Kingdom:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("orc_kingdom"))}\n" + "\n--- Human name:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_unit"))}\n" + "\n--- Human City:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_city"))}\n" + "\n--- Human Kingdom:\n"}{NameGenerator.generateNamesFromTemplate(20, AssetManager.name_generator.get("human_kingdom"))}\n");
  }

  public static string testShowOnomasticsVsLegacy()
  {
    string pResults = "";
    string str1 = "[<color=green>ONO</color>]";
    string str2 = "[<color=orange>LEG</color>]";
    string str3 = "[<color=red>---</color>]";
    string str4 = "[<color=yellow>DIC</color>]";
    foreach (NameGeneratorAsset nameGeneratorAsset in AssetManager.name_generator.list)
    {
      if ((string.IsNullOrEmpty("") || nameGeneratorAsset.id.Contains("")) && !"".Contains(nameGeneratorAsset.id))
      {
        string str5 = str3;
        string str6 = str3;
        string str7 = str3;
        string str8 = " ";
        string str9 = " ";
        if (nameGeneratorAsset.hasOnomastics())
        {
          str8 = "+";
          str6 = str1;
        }
        if (nameGeneratorAsset.use_dictionary)
          str5 = str4;
        List<string[]> templates1 = nameGeneratorAsset.templates;
        // ISSUE: explicit non-virtual call
        if ((templates1 != null ? (__nonvirtual (templates1.Count) > 0 ? 1 : 0) : 0) != 0)
        {
          str9 = "-";
          str7 = str2;
        }
        pResults = $"{pResults}{str8}{str9} {str5} {str6} {str7} {nameGeneratorAsset.id}\n";
        if (nameGeneratorAsset.hasOnomastics())
        {
          List<string[]> templates2 = nameGeneratorAsset.templates;
          // ISSUE: explicit non-virtual call
          if ((templates2 != null ? (__nonvirtual (templates2.Count) > 0 ? 1 : 0) : 0) != 0)
            pResults += NameGeneratorTests.compareOnomasticsVsLegacy(nameGeneratorAsset.id, 15000);
        }
      }
    }
    return NameGeneratorTests.writeResults("name_test_ono", pResults);
  }

  public static string compareOnomasticsVsLegacy(string pNameAssetID, int pRuns)
  {
    string str1 = "";
    Randy.resetSeed(Randy.randomInt(1, 500));
    NameGeneratorAsset pAsset = AssetManager.name_generator.get(pNameAssetID);
    HashSet<string> other1 = UnsafeCollectionPool<HashSet<string>, string>.Get();
    HashSet<string> other2 = UnsafeCollectionPool<HashSet<string>, string>.Get();
    HashSet<string> collection1 = UnsafeCollectionPool<HashSet<string>, string>.Get();
    float realtimeSinceStartup1 = Time.realtimeSinceStartup;
    for (int index = 0; index < pRuns; ++index)
      other1.Add(NameGenerator.generateNameFromTemplate(pAsset, pForceLegacy: true).ToLowerInvariant());
    float realtimeSinceStartup2 = Time.realtimeSinceStartup;
    float realtimeSinceStartup3 = Time.realtimeSinceStartup;
    for (int index = 0; index < pRuns; ++index)
      other2.Add(NameGenerator.generateNameFromTemplate(pAsset).ToLowerInvariant());
    float realtimeSinceStartup4 = Time.realtimeSinceStartup;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    foreach (string str2 in other1)
    {
      if (other2.Contains(str2))
      {
        ++num3;
        collection1.Add(str2);
      }
      else
        ++num1;
    }
    foreach (string str3 in other2)
    {
      if (!other1.Contains(str3))
        ++num2;
    }
    Dictionary<int, int> source1 = new Dictionary<int, int>();
    Dictionary<int, int> source2 = new Dictionary<int, int>();
    int num4 = int.MaxValue;
    int num5 = 0;
    foreach (string str4 in other1)
    {
      int length = str4.Length;
      if (length < num4)
        num4 = length;
      if (length > num5)
        num5 = length;
    }
    int num6 = int.MaxValue;
    int num7 = 0;
    foreach (string str5 in other2)
    {
      int length = str5.Length;
      if (length < num6)
        num6 = length;
      if (length > num7)
        num7 = length;
    }
    int num8 = Mathf.Min(num4, num6);
    int num9 = Mathf.Max(num5, num7);
    for (int key = num8; key <= num9; ++key)
    {
      source1[key] = 0;
      source2[key] = 0;
    }
    foreach (string str6 in other1)
    {
      int length = str6.Length;
      source1[length]++;
    }
    foreach (string str7 in other2)
    {
      int length = str7.Length;
      source2[length]++;
    }
    float num10 = 100f * (float) num1 / (float) other1.Count;
    float num11 = 100f * (float) num2 / (float) other2.Count;
    float num12 = 100f * (float) num3 / (float) other1.Count;
    string str8 = (double) num10 < 25.0 ? $"<color=green>{num10.ToString("F2")}%</color>" : ((double) num10 < 70.0 ? $"<color=orange>{num10.ToString("F2")}%</color>" : $"<color=red>{num10.ToString("F2")}%</color>");
    string str9 = (double) num11 < 25.0 ? $"<color=green>{num11.ToString("F2")}%</color>" : ((double) num11 < 70.0 ? $"<color=orange>{num11.ToString("F2")}%</color>" : $"<color=red>{num11.ToString("F2")}%</color>");
    string str10 = (double) num12 < 25.0 ? $"<color=red>{num12.ToString("F2")}%</color>" : ((double) num12 < 70.0 ? $"<color=orange>{num12.ToString("F2")}%</color>" : $"<color=green>{num12.ToString("F2")}%</color>");
    using (ListPool<string[]> pRows = new ListPool<string[]>())
    {
      pRows.Add(new string[2]
      {
        $"Unique {pNameAssetID} :",
        pRuns.ToString() + " runs"
      });
      ListPool<string[]> listPool1 = pRows;
      string[] strArray1 = new string[4]
      {
        "Legacy :",
        other1.Count.ToString() ?? "",
        null,
        null
      };
      int num13 = 100 * other1.Count / other2.Count;
      strArray1[2] = num13.ToString() + "%";
      strArray1[3] = (realtimeSinceStartup2 - realtimeSinceStartup1).ToString("F2") + "s";
      listPool1.Add(strArray1);
      ListPool<string[]> listPool2 = pRows;
      string[] strArray2 = new string[4]
      {
        "Ono :",
        null,
        null,
        null
      };
      num13 = other2.Count;
      strArray2[1] = num13.ToString() ?? "";
      num13 = 100 * other2.Count / other1.Count;
      strArray2[2] = num13.ToString() + "%";
      strArray2[3] = (realtimeSinceStartup4 - realtimeSinceStartup3).ToString("F2") + "s";
      listPool2.Add(strArray2);
      pRows.Add(new string[3]
      {
        "names only in legacy :",
        num1.ToString() ?? "",
        str8
      });
      pRows.Add(new string[3]
      {
        "names only in ono :",
        num2.ToString() ?? "",
        str9
      });
      pRows.Add(new string[3]
      {
        "names in both :",
        num3.ToString() ?? "",
        str10
      });
      string str11 = num4 < num6 ? $"<color=red>{num4.ToString()}</color>" : num4.ToString();
      string str12 = num6 < num4 ? $"<color=red>{num6.ToString()}</color>" : num6.ToString();
      string str13 = num5 > num7 ? $"<color=red>{num5.ToString()}</color>" : num5.ToString();
      string str14 = num7 > num5 ? $"<color=red>{num7.ToString()}</color>" : num7.ToString();
      pRows.Add(new string[2]
      {
        "min/max len legacy :",
        $"{str11}-{str13}"
      });
      pRows.Add(new string[2]
      {
        "min/max len ono :",
        $"{str12}-{str14}"
      });
      string str15 = $"{str1}\n{Toolbox.printRows(pRows)}";
      pRows.Clear();
      string[] array1 = source1.Select<KeyValuePair<int, int>, string>((Func<KeyValuePair<int, int>, string>) (p => p.Key.ToString())).ToArray<string>();
      string[] array2 = source1.Select<KeyValuePair<int, int>, string>((Func<KeyValuePair<int, int>, string>) (p => p.Value.ToString())).ToArray<string>();
      string[] array3 = source2.Select<KeyValuePair<int, int>, string>((Func<KeyValuePair<int, int>, string>) (p => p.Value.ToString())).ToArray<string>();
      string[] array4 = ((IEnumerable<string>) new string[1]
      {
        "len dist"
      }).Concat<string>((IEnumerable<string>) array1).ToArray<string>();
      string[] array5 = ((IEnumerable<string>) new string[1]
      {
        "legacy :"
      }).Concat<string>((IEnumerable<string>) array2).ToArray<string>();
      string[] array6 = ((IEnumerable<string>) new string[1]
      {
        "ono :"
      }).Concat<string>((IEnumerable<string>) array3).ToArray<string>();
      pRows.Add(array4);
      pRows.Add(array5);
      pRows.Add(array6);
      string str16 = $"{str15}\n{Toolbox.printRows(pRows)}";
      HashSet<string> collection2 = UnsafeCollectionPool<HashSet<string>, string>.Get();
      collection2.UnionWith((IEnumerable<string>) other1);
      collection2.ExceptWith((IEnumerable<string>) other2);
      using (ListPool<string> listPool3 = new ListPool<string>((ICollection<string>) collection2))
      {
        listPool3.Sort();
        using (ListPool<string> listPool4 = new ListPool<string>(91))
        {
          using (ListPool<string> listPool5 = new ListPool<string>(91))
          {
            using (ListPool<string> listPool6 = new ListPool<string>(91))
            {
              if (listPool3.Count > 0)
              {
                listPool4.Add("Legacy");
                (string str17, string str18) = NameGeneratorTests.findShortestLongest(listPool3);
                for (int index = 0; index < Mathf.Min(listPool3.Count, 30); ++index)
                  listPool4.Add(listPool3.Shift<string>());
                for (int index = 0; index < Mathf.Min(listPool3.Count, 30); ++index)
                  listPool4.Insert(Mathf.Min(31 /*0x1F*/, listPool4.Count), listPool3.Pop<string>());
                int num14 = Mathf.Max(listPool3.Count / 2 - 15, 0);
                for (int index = 0; index < Mathf.Min(listPool3.Count, 30); ++index)
                  listPool4.Insert(Mathf.Min(30 + index + 1, listPool4.Count), listPool3[index + num14]);
                listPool4.Add(Toolbox.fillLeft("", str18.Length, '='));
                listPool4.Add("Min/Max");
                listPool4.Add(Toolbox.fillLeft("", str18.Length, '='));
                listPool4.Add(str18);
                listPool4.Add(str17);
              }
              HashSet<string> collection3 = UnsafeCollectionPool<HashSet<string>, string>.Get();
              collection3.UnionWith((IEnumerable<string>) other2);
              collection3.ExceptWith((IEnumerable<string>) other1);
              using (ListPool<string> listPool7 = new ListPool<string>((ICollection<string>) collection3))
              {
                listPool7.Sort();
                if (listPool7.Count > 0)
                {
                  listPool5.Add("Ono");
                  (string str19, string str20) = NameGeneratorTests.findShortestLongest(listPool7);
                  for (int index = 0; index < Mathf.Min(listPool7.Count, 30); ++index)
                    listPool5.Add(listPool7.Shift<string>());
                  for (int index = 0; index < Mathf.Min(listPool7.Count, 30); ++index)
                    listPool5.Insert(Mathf.Min(31 /*0x1F*/, listPool5.Count), listPool7.Pop<string>());
                  int num15 = Mathf.Max(listPool7.Count / 2 - 15, 0);
                  for (int index = 0; index < Mathf.Min(listPool7.Count, 30); ++index)
                    listPool5.Insert(Mathf.Min(30 + index + 1, listPool5.Count), listPool7[index + num15]);
                  listPool5.Add(Toolbox.fillLeft("", str20.Length, '='));
                  listPool5.Add("Min/Max");
                  listPool5.Add(Toolbox.fillLeft("", str20.Length, '='));
                  listPool5.Add(str20);
                  listPool5.Add(str19);
                }
                using (ListPool<string> listPool8 = new ListPool<string>((ICollection<string>) collection1))
                {
                  listPool8.Sort();
                  if (listPool8.Count > 0)
                  {
                    listPool6.Add("Both");
                    (string str21, string str22) = NameGeneratorTests.findShortestLongest(listPool8);
                    for (int index = 0; index < Mathf.Min(listPool8.Count, 30); ++index)
                      listPool6.Add(listPool8.Shift<string>());
                    for (int index = 0; index < Mathf.Min(listPool8.Count, 30); ++index)
                      listPool6.Insert(Mathf.Min(31 /*0x1F*/, listPool8.Count), listPool8.Pop<string>());
                    int num16 = Mathf.Max(listPool8.Count / 2 - 15, 0);
                    for (int index = 0; index < Mathf.Min(listPool8.Count, 30); ++index)
                      listPool6.Insert(Mathf.Min(30 + index + 1, listPool8.Count), listPool8[index + num16]);
                    listPool6.Add(Toolbox.fillLeft("", str22.Length, '='));
                    listPool6.Add("Min/Max");
                    listPool6.Add(Toolbox.fillLeft("", str22.Length, '='));
                    listPool6.Add(str22);
                    listPool6.Add(str21);
                  }
                  string str23 = $"{str16}\n{Toolbox.printColumns(listPool4, listPool5, listPool6)}";
                  UnsafeCollectionPool<HashSet<string>, string>.Release(collection2);
                  UnsafeCollectionPool<HashSet<string>, string>.Release(collection3);
                  UnsafeCollectionPool<HashSet<string>, string>.Release(other1);
                  UnsafeCollectionPool<HashSet<string>, string>.Release(other2);
                  UnsafeCollectionPool<HashSet<string>, string>.Release(collection1);
                  return str23;
                }
              }
            }
          }
        }
      }
    }
  }

  private static (string, string) findShortestLongest(ListPool<string> pHashSet)
  {
    // ISSUE: unable to decompile the method.
  }

  public static string writeResults(string pFilename, string pResults)
  {
    File.WriteAllText($"{Application.persistentDataPath}/{pFilename}", pResults);
    Debug.Log((object) $"Written result to {pFilename} in {Application.persistentDataPath}");
    return pResults;
  }
}
