// Decompiled with JetBrains decompiler
// Type: OnomasticsEvolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public static class OnomasticsEvolver
{
  private static List<OnomasticsEvolutionAsset> _pool = new List<OnomasticsEvolutionAsset>();
  private static Dictionary<int, bool> _replaced = new Dictionary<int, bool>();

  public static void add(OnomasticsEvolutionAsset pEvolution)
  {
    OnomasticsEvolver._pool.Add(pEvolution);
  }

  public static void shuffle() => OnomasticsEvolver._pool.Shuffle<OnomasticsEvolutionAsset>();

  public static bool scramble(OnomasticsData pData)
  {
    int num1 = 0;
    foreach (KeyValuePair<string, OnomasticsDataGroup> group in pData.groups)
    {
      OnomasticsDataGroup onomasticsDataGroup = group.Value;
      if (!onomasticsDataGroup.isEmpty())
      {
        OnomasticsEvolver._replaced.Clear();
        string[] pArray = onomasticsDataGroup.characters_string.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int key = 0; key < pArray.Length; ++key)
          OnomasticsEvolver._replaced.Add(key, false);
        int num2 = 0;
        for (int index = 0; index < OnomasticsEvolver._pool.Count; ++index)
        {
          if (num2 < 7)
          {
            OnomasticsEvolutionAsset pAsset = OnomasticsEvolver._pool[index];
            bool flag = false;
            if (!pArray.Contains<string>(pAsset.to))
            {
              for (int key = 0; key < pArray.Length; ++key)
              {
                if (!OnomasticsEvolver._replaced[key])
                {
                  string pReplace = pArray[key];
                  if (pAsset.replacer(pAsset, ref pReplace))
                  {
                    flag = true;
                    OnomasticsEvolver._replaced[key] = true;
                  }
                  pArray[key] = pReplace;
                }
              }
              if (flag)
                ++num2;
            }
          }
        }
        onomasticsDataGroup.characters_string = string.Join(' ', pArray);
        onomasticsDataGroup.characters = pArray;
        if (num2 > 0)
          ++num1;
      }
    }
    return num1 > 0;
  }
}
