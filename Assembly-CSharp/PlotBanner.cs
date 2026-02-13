// Decompiled with JetBrains decompiler
// Type: PlotBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class PlotBanner : BannerGeneric<Plot, PlotData>
{
  protected override MetaType meta_type => MetaType.Plot;

  protected override string tooltip_id => "plot";

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.plot = this.meta_object;
    return tooltipData;
  }

  protected override void setupBanner()
  {
    base.setupBanner();
    PlotAsset asset = this.meta_object.getAsset();
    string pPath = "plots/backgrounds/plot_background_0";
    string pathIcon = asset.path_icon;
    this.part_background.sprite = SpriteTextureLoader.getSprite(pPath);
    this.part_icon.sprite = SpriteTextureLoader.getSprite(pathIcon);
  }
}
