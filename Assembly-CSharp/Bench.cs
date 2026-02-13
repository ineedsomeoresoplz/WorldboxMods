// Decompiled with JetBrains decompiler
// Type: Bench
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bench
{
  public static bool bench_enabled = false;
  public static bool bench_ai_enabled = false;
  private static Dictionary<string, BenchmarkGroup> dict = new Dictionary<string, BenchmarkGroup>();
  private static float _timer_flatten = 0.0f;

  public static void update()
  {
    if (!Bench.bench_enabled)
      return;
    Bench.finishSplitBenchmarkGroupAI();
    Bench.finishSplitBenchmarkGroup("effects_traits");
    Bench.finishSplitBenchmarkGroup("effects_items");
    if ((double) Bench._timer_flatten > 0.0)
    {
      Bench._timer_flatten -= Time.deltaTime;
    }
    else
    {
      Bench._timer_flatten = 0.05f;
      Bench.flatten("effects_traits");
      Bench.flatten("effects_items");
    }
  }

  private static void flatten(string pID)
  {
    BenchmarkGroup benchmarkGroup;
    if (!Bench.dict.TryGetValue(pID, out benchmarkGroup))
      return;
    benchmarkGroup.flatten();
  }

  private static void finishSplitBenchmarkGroupAI() => DebugConfig.isOn(DebugOption.BenchAiEnabled);

  private static void finishSplitBenchmarkGroup(string pID)
  {
    BenchmarkGroup benchmarkGroup;
    if (!Bench.dict.TryGetValue(pID, out benchmarkGroup))
      return;
    double pValue = 0.0;
    foreach (ToolBenchmarkData toolBenchmarkData in benchmarkGroup.dict_data.Values)
    {
      pValue += toolBenchmarkData.latest_result;
      toolBenchmarkData.saveAverageCounter();
    }
    Bench.benchSaveSplit(pID, pValue, 1, "game_total");
  }

  public static void saveAverageCounter(string pID, string pGroup)
  {
    Bench.get(pID, pGroup).saveAverageCounter();
  }

  public static BenchmarkGroup getGroup(string pID)
  {
    if (Bench.dict.ContainsKey(pID))
      return Bench.dict[pID];
    BenchmarkGroup group = new BenchmarkGroup();
    group.id = pID;
    Bench.dict.Add(pID, group);
    return group;
  }

  private static ToolBenchmarkData get(string pID, string pGroupID = "main", bool pNew = true)
  {
    BenchmarkGroup benchmarkGroup;
    if (!Bench.dict.TryGetValue(pGroupID, out benchmarkGroup))
    {
      benchmarkGroup = new BenchmarkGroup();
      benchmarkGroup.id = pGroupID;
      Bench.dict.Add(pGroupID, benchmarkGroup);
    }
    ToolBenchmarkData toolBenchmarkData;
    if (!benchmarkGroup.dict_data.TryGetValue(pID, out toolBenchmarkData) & pNew)
    {
      toolBenchmarkData = new ToolBenchmarkData();
      toolBenchmarkData.id = pID;
      benchmarkGroup.dict_data.Add(pID, toolBenchmarkData);
    }
    return toolBenchmarkData;
  }

  public static void clearBenchmarkEntrySkipMultiple(string pGroupID = "main", params string[] pEntries)
  {
    foreach (string pEntry in pEntries)
    {
      Bench.bench(pEntry, pGroupID);
      Bench.benchEnd(pEntry, pGroupID);
    }
  }

  public static void clearBenchmarkEntrySkip(string pID, string pGroupID = "main")
  {
    Bench.bench(pID, pGroupID);
    Bench.benchEnd(pID, pGroupID);
  }

  public static double bench(string pID, string pGroupID = "main", bool pForce = false)
  {
    if (!(Bench.bench_enabled | pForce))
      return 0.0;
    ToolBenchmarkData toolBenchmarkData = Bench.get(pID, pGroupID);
    double sinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
    double pTime = sinceStartupAsDouble;
    toolBenchmarkData.start(pTime);
    return sinceStartupAsDouble;
  }

  public static double benchEnd(
    string pID,
    string pGroupID = "main",
    bool pSaveCounter = false,
    long pCounter = 0,
    bool pForce = false)
  {
    if (!(Bench.bench_enabled | pForce))
      return 0.0;
    ToolBenchmarkData toolBenchmarkData = Bench.get(pID, pGroupID);
    double pTime = Time.realtimeSinceStartupAsDouble - toolBenchmarkData.latest_time;
    toolBenchmarkData.end(pTime);
    if (pSaveCounter)
    {
      toolBenchmarkData.newCount(pCounter);
      toolBenchmarkData.saveAverageCounter();
    }
    return pTime;
  }

  public static void benchSet(string pID, double pVal, int pCounter, string pGroupID = "main")
  {
    if (!Bench.bench_enabled)
      return;
    Bench.benchSave(pID, pVal, pCounter, pGroupID);
    Bench.saveAverageCounter(pID, pGroupID);
  }

  public static void benchSetValue(string pID, int pValue, string pGroupID = "main")
  {
    if (!Bench.bench_enabled)
      return;
    Bench.get(pID, pGroupID).newValue(pValue);
  }

  public static int getBenchValue(string pID, string pGroupID = "main")
  {
    return !Bench.bench_enabled ? 0 : (int) Bench.get(pID, pGroupID).debug_value;
  }

  public static double benchSave(string pID, double pValue, int pCounter, string pGroupID = "main")
  {
    if (!Bench.bench_enabled)
      return 0.0;
    ToolBenchmarkData toolBenchmarkData = Bench.get(pID, pGroupID);
    toolBenchmarkData.end(pValue);
    toolBenchmarkData.newCount((long) pCounter);
    return pValue;
  }

  public static double benchSaveSplit(string pID, double pValue, int pCounter, string pGroupID = "main")
  {
    if (!Bench.bench_enabled)
      return 0.0;
    ToolBenchmarkData toolBenchmarkData = Bench.get(pID, pGroupID);
    toolBenchmarkData.end(pValue);
    toolBenchmarkData.newCount((long) pCounter);
    return pValue;
  }

  public static string getBenchResult(string pID, string pGroupID = "main", bool pAverage = true)
  {
    return Bench.getBenchResultAsDouble(pID, pGroupID, pAverage).ToString("##,0.#######");
  }

  public static double getBenchResultAsDouble(string pID, string pGroupID = "main", bool pAverage = true)
  {
    ToolBenchmarkData toolBenchmarkData = Bench.get(pID, pGroupID, false);
    if (toolBenchmarkData == null)
      return -1.0;
    return pAverage ? toolBenchmarkData.getAverage() : toolBenchmarkData.latest_result;
  }

  public static string printableBenchResults(string pGroupID = "main", bool pAverage = false, params string[] pID)
  {
    double[] keys = new double[pID.Length];
    double num1 = 0.0;
    double maxValue = double.MaxValue;
    for (int index = 0; index < pID.Length; ++index)
    {
      keys[index] = Bench.getBenchResultAsDouble(pID[index], pGroupID, pAverage);
      if (keys[index] > num1)
        num1 = keys[index];
      if (keys[index] < maxValue)
        maxValue = keys[index];
    }
    Array.Sort<double, string>(keys, pID);
    using (ListPool<string[]> pRows = new ListPool<string[]>())
    {
      pRows.Add(new string[5]
      {
        "ID",
        "TIME",
        "PERCENT",
        "WINNER",
        "BAR"
      });
      pRows.Add(new string[0]);
      for (int index1 = 0; index1 < pID.Length; ++index1)
      {
        double num2 = keys[index1] / num1;
        bool flag1 = keys[index1].Equals(maxValue);
        bool flag2 = keys[index1].Equals(num1);
        string str1 = "";
        string str2 = "";
        string pString = "";
        int num3 = (int) (num2 * 10.0);
        for (int index2 = 0; index2 < num3; ++index2)
          pString += "■";
        string str3 = Toolbox.fillRight(pString, 10);
        if (flag1 | flag2)
        {
          if (flag1)
            str1 = "<color=green>";
          if (flag2)
            str1 = "<color=red>";
          str2 = "</color>";
        }
        string str4 = str1 + pID[index1] + str2;
        string str5 = str1 + keys[index1].ToString("F7") + str2;
        string str6 = str1 + num2.ToString("P0") + str2;
        string str7 = str1 + (flag1 ? "WINNER" : (flag2 ? "SLOWEST" : "")) + str2;
        string str8 = str1 + str3 + str2;
        pRows.Add(new string[5]
        {
          str4,
          str5,
          str6,
          str7,
          str8
        });
      }
      return Toolbox.printRows(pRows);
    }
  }

  public static void printBenchResult(string pID, string pGroupID = "main", bool pAverage = false)
  {
    double benchResultAsDouble = Bench.getBenchResultAsDouble(pID, pGroupID, pAverage);
    string str = benchResultAsDouble.ToString("##,0.##########");
    if (benchResultAsDouble > 0.3)
      str = $"<color=red>{str}</color>";
    else if (benchResultAsDouble > 0.1)
      str = $"<color=yellow>{str}</color>";
    Debug.Log((object) $"#benchmark: <color=white>{pID}</color>: {str}");
  }
}
