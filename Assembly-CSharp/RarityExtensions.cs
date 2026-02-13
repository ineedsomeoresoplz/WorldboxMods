// Decompiled with JetBrains decompiler
// Type: RarityExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class RarityExtensions
{
  public static string getRarityColorHex(this Rarity pRarity) => pRarity.getAsset().color_hex;

  public static RarityAsset getAsset(this Rarity pRarity)
  {
    string stringId = pRarity.getStringID();
    return AssetManager.rarity_library.get(stringId);
  }

  public static Color getRarityColor(this Rarity pRarity)
  {
    return pRarity.getAsset().color_container.color;
  }

  public static string getStringID(this Rarity pRarity) => pRarity.ToString().ToLower();

  public static int GetRate(this Rarity pRarity)
  {
    switch (pRarity)
    {
      case Rarity.R0_Normal:
        return 10;
      case Rarity.R1_Rare:
        return 6;
      case Rarity.R2_Epic:
        return 3;
      case Rarity.R3_Legendary:
        return 1;
      default:
        return 0;
    }
  }
}
