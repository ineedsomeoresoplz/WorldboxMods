// Decompiled with JetBrains decompiler
// Type: BenchmarkBlacklist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BenchmarkBlacklist
{
  private static List<WorldTile> _test_world_tiles = new List<WorldTile>();
  private static HashSet<WorldTile> _test_hashset = new HashSet<WorldTile>();
  private static WorldTile[] _test_world_tiles_arr;
  private static List<string> _names = new List<string>();
  private static HashSet<string> _names_set = new HashSet<string>();
  private static int _runs = 0;
  private static int _max = 250;
  private static HashSet<string> _good_words = new HashSet<string>();
  private static HashSet<string> _bad_words = new HashSet<string>();
  private static HashSet<string> _result_good_words = new HashSet<string>();
  private static HashSet<string> _result_bad_words = new HashSet<string>();

  public static void start()
  {
    if (BenchmarkBlacklist._runs-- <= 0)
    {
      BenchmarkBlacklist._names.Clear();
      BenchmarkBlacklist._names_set.Clear();
      BenchmarkBlacklist._max = Randy.randomInt(50, (int) byte.MaxValue);
      BenchmarkBlacklist._runs = 5;
    }
    if (BenchmarkBlacklist._names.Count == 0)
    {
      BenchmarkBlacklist._good_words.Clear();
      BenchmarkBlacklist._bad_words.Clear();
      BenchmarkBlacklist._names_set.Clear();
      Blacklist.init();
      BlacklistTest.init();
      BlacklistTest2.init();
      BlacklistTest3.init();
      BlacklistTest4.init();
      BlacklistTest5.init();
      BlacklistTest6.init();
      BlacklistTest7.init();
      BlacklistTest8.init();
      BlacklistTest9.init();
      BlacklistTest10.init();
      BlacklistTest11.init();
      BlacklistTest12.init();
      BlacklistTest13.init();
      AssetManager.name_generator.list.Shuffle<NameGeneratorAsset>();
      foreach (NameGeneratorAsset pAsset in AssetManager.name_generator.list)
      {
        if (BenchmarkBlacklist._names_set.Count <= BenchmarkBlacklist._max)
        {
          for (int index = 0; index < 150; ++index)
          {
            string nameFromTemplate = NameGenerator.generateNameFromTemplate(pAsset);
            if (string.IsNullOrEmpty(nameFromTemplate))
            {
              Debug.LogError((object) ("name generator returned null or empty string " + pAsset.id));
            }
            else
            {
              BenchmarkBlacklist._names_set.Add(nameFromTemplate);
              if (BenchmarkBlacklist._names_set.Count > BenchmarkBlacklist._max)
                break;
            }
          }
        }
        else
          break;
      }
      BenchmarkBlacklist._names.AddRange((IEnumerable<string>) BenchmarkBlacklist._names_set);
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
      {
        if (!Blacklist.checkBlackList(BenchmarkBlacklist._names[index]))
        {
          flag1 = true;
          BenchmarkBlacklist._good_words.Add(BenchmarkBlacklist._names[index]);
        }
        else
        {
          flag2 = true;
          BenchmarkBlacklist._bad_words.Add(BenchmarkBlacklist._names[index]);
        }
      }
      if (!flag1 || !flag2)
      {
        BenchmarkBlacklist._runs = 0;
        BenchmarkBlacklist.start();
      }
      string[] strArray = new string[8]
      {
        "Unique names for test ",
        BenchmarkBlacklist._names.Count.ToString(),
        " / ",
        BenchmarkBlacklist._max.ToString(),
        " => ",
        null,
        null,
        null
      };
      int count = BenchmarkBlacklist._good_words.Count;
      strArray[5] = count.ToString();
      strArray[6] = " / ";
      count = BenchmarkBlacklist._bad_words.Count;
      strArray[7] = count.ToString();
      Debug.Log((object) string.Concat(strArray));
    }
    BenchmarkBlacklist._result_good_words.Clear();
    BenchmarkBlacklist._result_bad_words.Clear();
    Bench.bench("blacklist_test", "blacklist_test_total");
    Bench.bench("current_blacklist_good", "blacklist_test");
    int pCounter1 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (!Blacklist.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter1;
        BenchmarkBlacklist._result_good_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("current_blacklist_good", "blacklist_test", true, (long) pCounter1);
    Bench.bench("current_blacklist_bad", "blacklist_test");
    int pCounter2 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (Blacklist.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter2;
        BenchmarkBlacklist._result_bad_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("current_blacklist_bad", "blacklist_test", true, (long) pCounter2);
    BenchmarkBlacklist.checkResult("current_blacklist_bad");
    Bench.bench("three_blacklist_good_9", "blacklist_test");
    int pCounter3 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (!BlacklistTest9.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter3;
        BenchmarkBlacklist._result_good_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("three_blacklist_good_9", "blacklist_test", true, (long) pCounter3);
    Bench.bench("three_blacklist_bad_9", "blacklist_test");
    int pCounter4 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (BlacklistTest9.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter4;
        BenchmarkBlacklist._result_bad_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("three_blacklist_bad_9", "blacklist_test", true, (long) pCounter4);
    BenchmarkBlacklist.checkResult("three_blacklist_bad_9");
    Bench.bench("old_blacklist_good_10", "blacklist_test");
    int pCounter5 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (!BlacklistTest10.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter5;
        BenchmarkBlacklist._result_good_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("old_blacklist_good_10", "blacklist_test", true, (long) pCounter5);
    Bench.bench("old_blacklist_bad_10", "blacklist_test");
    int pCounter6 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (BlacklistTest10.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter6;
        BenchmarkBlacklist._result_bad_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("old_blacklist_bad_10", "blacklist_test", true, (long) pCounter6);
    BenchmarkBlacklist.checkResult("old_blacklist_bad_10");
    Bench.bench("slice_blacklist_good_11", "blacklist_test");
    int pCounter7 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (!BlacklistTest11.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter7;
        BenchmarkBlacklist._result_good_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("slice_blacklist_good_11", "blacklist_test", true, (long) pCounter7);
    Bench.bench("slice_blacklist_bad_11", "blacklist_test");
    int pCounter8 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (BlacklistTest11.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter8;
        BenchmarkBlacklist._result_bad_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("slice_blacklist_bad_11", "blacklist_test", true, (long) pCounter8);
    BenchmarkBlacklist.checkResult("slice_blacklist_bad_11");
    Bench.bench("ref_blacklist_good_12", "blacklist_test");
    int pCounter9 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (!BlacklistTest12.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter9;
        BenchmarkBlacklist._result_good_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("ref_blacklist_good_12", "blacklist_test", true, (long) pCounter9);
    Bench.bench("ref_blacklist_bad_12", "blacklist_test");
    int pCounter10 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (BlacklistTest12.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter10;
        BenchmarkBlacklist._result_bad_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("ref_blacklist_bad_12", "blacklist_test", true, (long) pCounter10);
    BenchmarkBlacklist.checkResult("ref_blacklist_bad_12");
    Bench.bench("idx_blacklist_good_13", "blacklist_test");
    int pCounter11 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (!BlacklistTest13.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter11;
        BenchmarkBlacklist._result_good_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("idx_blacklist_good_13", "blacklist_test", true, (long) pCounter11);
    Bench.bench("idx_blacklist_bad_13", "blacklist_test");
    int pCounter12 = 0;
    for (int index = 0; index < BenchmarkBlacklist._names.Count; ++index)
    {
      if (BlacklistTest13.checkBlackList(BenchmarkBlacklist._names[index]))
      {
        ++pCounter12;
        BenchmarkBlacklist._result_bad_words.Add(BenchmarkBlacklist._names[index]);
      }
    }
    Bench.benchEnd("idx_blacklist_bad_13", "blacklist_test", true, (long) pCounter12);
    BenchmarkBlacklist.checkResult("idx_blacklist_bad_13");
    Bench.benchEnd("blacklist_test", "blacklist_test_total");
  }

  public static void checkResult(string pBenchmarkName)
  {
    if (BenchmarkBlacklist._result_good_words.Count != BenchmarkBlacklist._good_words.Count || BenchmarkBlacklist._result_bad_words.Count != BenchmarkBlacklist._bad_words.Count)
    {
      string[] strArray = new string[9];
      strArray[0] = pBenchmarkName;
      strArray[1] = ": Blacklist check failed ";
      int count = BenchmarkBlacklist._result_good_words.Count;
      strArray[2] = count.ToString();
      strArray[3] = " ";
      count = BenchmarkBlacklist._good_words.Count;
      strArray[4] = count.ToString();
      strArray[5] = " ";
      count = BenchmarkBlacklist._result_bad_words.Count;
      strArray[6] = count.ToString();
      strArray[7] = " ";
      count = BenchmarkBlacklist._bad_words.Count;
      strArray[8] = count.ToString();
      Debug.LogError((object) string.Concat(strArray));
      foreach (string resultGoodWord in BenchmarkBlacklist._result_good_words)
      {
        if (!BenchmarkBlacklist._good_words.Contains(resultGoodWord))
          Debug.LogError((object) $"{pBenchmarkName}: Missing good word: {resultGoodWord}");
      }
      foreach (string resultBadWord in BenchmarkBlacklist._result_bad_words)
      {
        if (!BenchmarkBlacklist._bad_words.Contains(resultBadWord))
          Debug.LogError((object) $"{pBenchmarkName}: Missing bad word: {resultBadWord}");
      }
      foreach (string goodWord in BenchmarkBlacklist._good_words)
      {
        if (!BenchmarkBlacklist._result_good_words.Contains(goodWord))
          Debug.LogError((object) $"{pBenchmarkName}: Extra good word: {goodWord}");
      }
      foreach (string badWord in BenchmarkBlacklist._bad_words)
      {
        if (!BenchmarkBlacklist._result_bad_words.Contains(badWord))
          Debug.LogError((object) $"{pBenchmarkName}: Extra bad word: {badWord}");
      }
    }
    BenchmarkBlacklist._result_good_words.Clear();
    BenchmarkBlacklist._result_bad_words.Clear();
  }
}
