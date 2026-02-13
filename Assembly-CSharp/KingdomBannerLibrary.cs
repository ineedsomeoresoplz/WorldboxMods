// Decompiled with JetBrains decompiler
// Type: KingdomBannerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KingdomBannerLibrary : GenericBannerLibrary
{
  public const string PATH_BANNER_KINGDOMS = "banners_kingdoms/";
  public const string PATH_BACKGROUND = "/background";
  public const string PATH_ICON = "/icon";

  public override void init() => base.init();

  public override BannerAsset get(string pID)
  {
    if (this.dict.ContainsKey(pID))
      return base.get(pID);
    this.loadNewAssetRuntime(pID);
    return base.get(pID);
  }

  public static string getFullPathBackground(string pID) => $"banners_kingdoms/{pID}/background";

  public static string getFullPathIcon(string pID) => $"banners_kingdoms/{pID}/icon";

  private BannerAsset loadNewAssetRuntime(string pID)
  {
    string fullPathBackground = KingdomBannerLibrary.getFullPathBackground(pID);
    string fullPathIcon = KingdomBannerLibrary.getFullPathIcon(pID);
    Sprite[] spriteList1 = SpriteTextureLoader.getSpriteList(fullPathBackground);
    Sprite[] spriteList2 = SpriteTextureLoader.getSpriteList(fullPathIcon);
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    foreach (Sprite sprite in spriteList1)
    {
      string str = $"{fullPathBackground}/{((Object) sprite).name}";
      stringList1.Add(str);
    }
    foreach (Sprite sprite in spriteList2)
    {
      string str = $"{fullPathIcon}/{((Object) sprite).name}";
      stringList2.Add(str);
    }
    BannerAsset bannerAsset = new BannerAsset();
    bannerAsset.id = pID;
    bannerAsset.backgrounds = stringList1;
    bannerAsset.icons = stringList2;
    BannerAsset pAsset = bannerAsset;
    this.add(pAsset);
    return pAsset;
  }
}
