// Decompiled with JetBrains decompiler
// Type: LanguagesColorsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class LanguagesColorsLibrary : ColorLibrary
{
  public LanguagesColorsLibrary() => this.file_path = "colors/colors_general";

  public override void init()
  {
    base.init();
    this.useSameColorsFrom((ColorLibrary) AssetManager.kingdom_colors_library);
  }

  public override bool isColorUsedInWorld(ColorAsset pAsset)
  {
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) World.world.languages)
    {
      if (this.checkColor(pAsset, language.data.color_id))
        return true;
    }
    return base.isColorUsedInWorld(pAsset);
  }
}
