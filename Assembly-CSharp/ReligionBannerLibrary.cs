// Decompiled with JetBrains decompiler
// Type: ReligionBannerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ReligionBannerLibrary : GenericBannerLibrary
{
  public override void init()
  {
    base.init();
    BannerAsset pAsset = new BannerAsset();
    pAsset.id = "main";
    pAsset.backgrounds = new List<string>()
    {
      "religions/background_00",
      "religions/background_01",
      "religions/background_02",
      "religions/background_03",
      "religions/background_04"
    };
    pAsset.icons = new List<string>()
    {
      "religions/icon_00",
      "religions/icon_01",
      "religions/icon_02",
      "religions/icon_03",
      "religions/icon_04",
      "religions/icon_05",
      "religions/icon_06",
      "religions/icon_07",
      "religions/icon_08",
      "religions/icon_09",
      "religions/icon_10",
      "religions/icon_11",
      "religions/icon_12",
      "religions/icon_13",
      "religions/icon_14",
      "religions/icon_15",
      "religions/icon_16",
      "religions/icon_17",
      "religions/icon_18",
      "religions/icon_19",
      "religions/icon_20",
      "religions/icon_21",
      "religions/icon_22",
      "religions/icon_23",
      "religions/icon_24"
    };
    this.main = this.add(pAsset);
  }
}
