// Decompiled with JetBrains decompiler
// Type: SubspeciesBannerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SubspeciesBannerLibrary : GenericBannerLibrary
{
  public override void init()
  {
    base.init();
    BannerAsset pAsset = new BannerAsset();
    pAsset.id = "main";
    pAsset.backgrounds = new List<string>()
    {
      "subspecies/background_00",
      "subspecies/background_01",
      "subspecies/background_02",
      "subspecies/background_03",
      "subspecies/background_04",
      "subspecies/background_05",
      "subspecies/background_06",
      "subspecies/background_07",
      "subspecies/background_08",
      "subspecies/background_09",
      "subspecies/background_10",
      "subspecies/background_11"
    };
    this.main = this.add(pAsset);
  }
}
