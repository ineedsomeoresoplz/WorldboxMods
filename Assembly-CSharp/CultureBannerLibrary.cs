// Decompiled with JetBrains decompiler
// Type: CultureBannerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CultureBannerLibrary : GenericBannerLibrary
{
  public override void init()
  {
    base.init();
    BannerAsset pAsset = new BannerAsset();
    pAsset.id = "main";
    pAsset.icons = new List<string>()
    {
      "cultures/culture_element_0",
      "cultures/culture_element_1",
      "cultures/culture_element_2",
      "cultures/culture_element_3",
      "cultures/culture_element_4",
      "cultures/culture_element_5",
      "cultures/culture_element_6",
      "cultures/culture_element_7"
    };
    pAsset.backgrounds = new List<string>()
    {
      "cultures/culture_decor_0",
      "cultures/culture_decor_1",
      "cultures/culture_decor_2",
      "cultures/culture_decor_3",
      "cultures/culture_decor_4",
      "cultures/culture_decor_5",
      "cultures/culture_decor_6",
      "cultures/culture_decor_7",
      "cultures/culture_decor_8"
    };
    this.main = this.add(pAsset);
  }
}
