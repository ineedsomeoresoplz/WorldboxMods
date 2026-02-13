// Decompiled with JetBrains decompiler
// Type: ArmiesColorsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ArmiesColorsLibrary : ColorLibrary
{
  public ArmiesColorsLibrary() => this.file_path = "colors/colors_general";

  public override void init()
  {
    base.init();
    this.loadFromFile<ArmiesColorsLibrary>();
  }

  public override bool isColorUsedInWorld(ColorAsset pAsset)
  {
    foreach (Army army in (CoreSystemManager<Army, ArmyData>) World.world.armies)
    {
      if (this.checkColor(pAsset, army.data.color_id))
        return true;
    }
    return base.isColorUsedInWorld(pAsset);
  }
}
