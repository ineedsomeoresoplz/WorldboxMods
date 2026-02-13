// Decompiled with JetBrains decompiler
// Type: ClanBannerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ClanBannerLibrary : GenericBannerLibrary
{
  public override void init()
  {
    base.init();
    BannerAsset pAsset = new BannerAsset();
    pAsset.id = "main";
    pAsset.backgrounds = new List<string>()
    {
      "clans/clan_background_00",
      "clans/clan_background_01",
      "clans/clan_background_02",
      "clans/clan_background_03",
      "clans/clan_background_04",
      "clans/clan_background_05",
      "clans/clan_background_06",
      "clans/clan_background_07",
      "clans/clan_background_08",
      "clans/clan_background_09",
      "clans/clan_background_10",
      "clans/clan_background_11",
      "clans/clan_background_12",
      "clans/clan_background_13",
      "clans/clan_background_14",
      "clans/clan_background_15",
      "clans/clan_background_16"
    };
    pAsset.icons = new List<string>()
    {
      "clans/clan_icon_00",
      "clans/clan_icon_01",
      "clans/clan_icon_02",
      "clans/clan_icon_03",
      "clans/clan_icon_04",
      "clans/clan_icon_05",
      "clans/clan_icon_06",
      "clans/clan_icon_07",
      "clans/clan_icon_08",
      "clans/clan_icon_09",
      "clans/clan_icon_10",
      "clans/clan_icon_11",
      "clans/clan_icon_12",
      "clans/clan_icon_13",
      "clans/clan_icon_14",
      "clans/clan_icon_15",
      "clans/clan_icon_16",
      "clans/clan_icon_17",
      "clans/clan_icon_18",
      "clans/clan_icon_19",
      "clans/clan_icon_20",
      "clans/clan_icon_21"
    };
    this.main = this.add(pAsset);
  }
}
