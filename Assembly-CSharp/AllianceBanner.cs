// Decompiled with JetBrains decompiler
// Type: AllianceBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AllianceBanner : BannerGeneric<Alliance, AllianceData>
{
  public Sprite frame_normal;
  public Sprite frame_forced;

  protected override MetaType meta_type => MetaType.Alliance;

  protected override string tooltip_id => "alliance";

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.alliance = this.meta_object;
    return tooltipData;
  }

  protected override void setupBanner()
  {
    base.setupBanner();
    this.part_background.sprite = this.meta_object.getBackgroundSprite();
    this.part_icon.sprite = this.meta_object.getIconSprite();
    ColorAsset color = this.meta_object.getColor();
    ((Graphic) this.part_background).color = color.getColorMainSecond();
    ((Graphic) this.part_icon).color = color.getColorBanner();
    if (this.meta_object.isNormalType())
      this.part_frame.sprite = this.frame_normal;
    else
      this.part_frame.sprite = this.frame_forced;
  }
}
