// Decompiled with JetBrains decompiler
// Type: LoyaltyCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class LoyaltyCalculator
{
  public static Dictionary<LoyaltyAsset, int> results = new Dictionary<LoyaltyAsset, int>();
  public static int total = 0;

  public static int calculate(City pCity)
  {
    LoyaltyCalculator.clear();
    foreach (LoyaltyAsset key in AssetManager.loyalty_library.list)
    {
      int num = key.calc(pCity);
      LoyaltyCalculator.total += num;
      if (num != 0)
        LoyaltyCalculator.results.Add(key, num);
    }
    return LoyaltyCalculator.total;
  }

  private static void clear()
  {
    LoyaltyCalculator.total = 0;
    LoyaltyCalculator.results.Clear();
  }
}
