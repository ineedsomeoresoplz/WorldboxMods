// Decompiled with JetBrains decompiler
// Type: FamilysBannerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class FamilysBannerLibrary : GenericBannerLibrary
{
  public override void init()
  {
    base.init();
    BannerAsset pAsset = new BannerAsset();
    pAsset.id = "main";
    pAsset.backgrounds = new List<string>()
    {
      "families/background_00",
      "families/background_01",
      "families/background_02",
      "families/background_03",
      "families/background_04",
      "families/background_05",
      "families/background_06",
      "families/background_07",
      "families/background_08",
      "families/background_09",
      "families/background_10",
      "families/background_11",
      "families/background_12",
      "families/background_13",
      "families/background_14",
      "families/background_15",
      "families/background_16"
    };
    pAsset.frames = new List<string>()
    {
      "families/frame_00",
      "families/frame_01",
      "families/frame_02",
      "families/frame_03",
      "families/frame_04",
      "families/frame_05",
      "families/frame_06",
      "families/frame_07",
      "families/frame_08",
      "families/frame_09",
      "families/frame_10",
      "families/frame_11",
      "families/frame_12",
      "families/frame_13",
      "families/frame_14",
      "families/frame_15",
      "families/frame_16",
      "families/frame_17",
      "families/frame_18",
      "families/frame_19",
      "families/frame_20",
      "families/frame_21",
      "families/frame_22"
    };
    this.main = this.add(pAsset);
  }
}
