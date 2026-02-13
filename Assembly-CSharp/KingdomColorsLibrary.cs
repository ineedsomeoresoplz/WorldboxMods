// Decompiled with JetBrains decompiler
// Type: KingdomColorsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class KingdomColorsLibrary : ColorLibrary
{
  public KingdomColorsLibrary()
  {
    this.file_path = "colors/colors_general";
    this.must_be_global = true;
  }

  public override void init()
  {
    base.init();
    this.loadFromFile<KingdomColorsLibrary>();
  }

  public override bool isColorUsedInWorld(ColorAsset pAsset)
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (this.checkColor(pAsset, kingdom.data.color_id))
        return true;
    }
    foreach (Alliance alliance in (CoreSystemManager<Alliance, AllianceData>) World.world.alliances)
    {
      if (this.checkColor(pAsset, alliance.data.color_id))
        return true;
    }
    return base.isColorUsedInWorld(pAsset);
  }
}
