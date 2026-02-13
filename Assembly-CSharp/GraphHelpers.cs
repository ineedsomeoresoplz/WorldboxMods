// Decompiled with JetBrains decompiler
// Type: GraphHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityPools;

#nullable disable
public static class GraphHelpers
{
  public static string getCategoryName(string pCategory)
  {
    return !pCategory.Contains('|') ? pCategory : pCategory.Split('|', StringSplitOptions.None)[0];
  }

  public static ListPool<string> bestCategories(Dictionary<string, MinMax> pCategoryStats)
  {
    Dictionary<string, AvgStats> dictionary = UnsafeCollectionPool<Dictionary<string, AvgStats>, KeyValuePair<string, AvgStats>>.Get();
    foreach (KeyValuePair<string, MinMax> pCategoryStat in pCategoryStats)
    {
      string categoryName = GraphHelpers.getCategoryName(pCategoryStat.Key);
      MinMax minMax = pCategoryStat.Value;
      AvgStats avgStats;
      if (!dictionary.TryGetValue(categoryName, out avgStats))
        avgStats = new AvgStats(0.0, 0, categoryName);
      dictionary[categoryName] = avgStats.add((double) minMax.max);
    }
    using (ListPool<AvgStats> listPool1 = new ListPool<AvgStats>((ICollection<AvgStats>) dictionary.Values))
    {
      UnsafeCollectionPool<Dictionary<string, AvgStats>, KeyValuePair<string, AvgStats>>.Release(dictionary);
      listPool1.Sort((Comparison<AvgStats>) ((a, b) =>
      {
        int num = b.count.CompareTo(a.count);
        return num == 0 ? b.avg.CompareTo(a.avg) : num;
      }));
      int capacity = Math.Min(3, listPool1.Count);
      ListPool<string> listPool2 = new ListPool<string>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        if (index <= 0 || listPool1[index].avg > 3.0 && listPool1[index].count >= listPool1[0].count)
          listPool2.Add(listPool1[index].name);
      }
      return listPool2;
    }
  }

  public static string horizontalFormatYears(double pValue, int pDigits)
  {
    return $"{Toolbox.formatNumber((long) (pValue - (double) Date.getCurrentYear()) * -1L)}\n{pValue.ToText()}";
  }

  public static string verticalFormat(double pValue, int pDigits)
  {
    MinMax minMax = GraphController.min_max;
    string pText = Math.Abs(pValue) >= 1000.0 ? Toolbox.formatNumber((long) pValue) : pValue.ToString("N" + pDigits.ToString());
    if (pValue == 0.0)
      return Toolbox.coloredText(pText, "#FFBC66");
    if (pValue < 0.0)
    {
      string pColor = Toolbox.colorBetween(pValue, (double) minMax.min, 0.0, "#FF637D", "#FFBC66");
      return Toolbox.coloredText(pText, pColor);
    }
    string pColor1 = Toolbox.colorBetween(pValue, 0.0, (double) minMax.max, "#FFBC66", "#F3961F");
    return Toolbox.coloredText(pText, pColor1);
  }

  public static long calculateNiceMaxAxisSize(double pLargestValue)
  {
    if (pLargestValue < 5.0)
      return 5;
    if (pLargestValue < 8.0)
      return 8;
    if (pLargestValue < 10.0)
      return 10;
    if (pLargestValue < 20.0)
      return 20;
    if (pLargestValue < 30.0)
      return 30;
    if (pLargestValue < 40.0)
      return 40;
    if (pLargestValue < 50.0)
      return 50;
    if (pLargestValue < 60.0)
      return 60;
    if (pLargestValue < 80.0)
      return 80 /*0x50*/;
    if (pLargestValue < 100.0)
      return 100;
    if (pLargestValue < 120.0)
      return 120;
    if (pLargestValue < 140.0)
      return 140;
    if (pLargestValue < 160.0)
      return 160 /*0xA0*/;
    if (pLargestValue < 180.0)
      return 180;
    if (pLargestValue < 200.0)
      return 200;
    if (pLargestValue < 240.0)
      return 240 /*0xF0*/;
    if (pLargestValue < 280.0)
      return 280;
    if (pLargestValue < 300.0)
      return 300;
    if (pLargestValue < 340.0)
      return 340;
    if (pLargestValue < 380.0)
      return 380;
    if (pLargestValue < 400.0)
      return 400;
    if (pLargestValue < 500.0)
      return 500;
    if (pLargestValue < 600.0)
      return 600;
    if (pLargestValue < 700.0)
      return 700;
    if (pLargestValue < 800.0)
      return 800;
    if (pLargestValue < 900.0)
      return 900;
    if (pLargestValue < 1000.0)
      return 1000;
    double num1 = (double) Mathf.Pow(10f, Mathf.Floor(Mathf.Log10((float) pLargestValue)));
    double num2 = pLargestValue / num1;
    return (long) ((num2 > 1.5 ? (num2 > 2.0 ? (num2 > 3.0 ? (num2 > 5.0 ? 10.0 : 5.0) : 3.0) : 2.0) : 1.5) * num1);
  }

  public static int findVerticalDivision(long pValue)
  {
    if (GraphHelpers.canDivideIntoWholeNumbers(pValue, 4))
      return 4;
    if (GraphHelpers.canDivideIntoWholeNumbers(pValue, 5))
      return 5;
    if (GraphHelpers.canDivideIntoWholeNumbers(pValue, 3))
      return 3;
    if (GraphHelpers.canDivideIntoWholeNumbers(pValue, 6))
      return 6;
    return GraphHelpers.canDivideIntoWholeNumbers(pValue, 2) ? 2 : 4;
  }

  private static bool canDivideIntoWholeNumbers(long pTotalValue, int pSegments)
  {
    for (int index = 1; index <= pSegments; ++index)
    {
      if ((double) pTotalValue / (double) pSegments * (double) index % 1.0 > 0.0)
        return false;
    }
    return true;
  }
}
