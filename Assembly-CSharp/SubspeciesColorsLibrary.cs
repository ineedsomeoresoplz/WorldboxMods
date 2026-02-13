// Decompiled with JetBrains decompiler
// Type: SubspeciesColorsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SubspeciesColorsLibrary : ColorLibrary
{
  public SubspeciesColorsLibrary() => this.file_path = "colors/colors_general";

  public override void init()
  {
    base.init();
    this.useSameColorsFrom((ColorLibrary) AssetManager.kingdom_colors_library);
  }

  public override bool isColorUsedInWorld(ColorAsset pAsset)
  {
    foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) World.world.subspecies)
    {
      if (this.checkColor(pAsset, subspecies.data.color_id))
        return true;
    }
    return base.isColorUsedInWorld(pAsset);
  }
}
