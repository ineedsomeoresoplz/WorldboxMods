// Decompiled with JetBrains decompiler
// Type: FamilyBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class FamilyBanner : BannerGeneric<Family, FamilyData>
{
  protected override MetaType meta_type => MetaType.Family;

  protected override string tooltip_id => "family";

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.family = this.meta_object;
    return tooltipData;
  }

  protected override void setupBanner()
  {
    base.setupBanner();
    this.part_background.sprite = this.meta_object.getSpriteBackground();
    this.part_icon.sprite = this.meta_object.getSpriteIcon();
    this.part_frame.sprite = this.meta_object.getSpriteFrame();
    ((Graphic) this.part_background).color = this.meta_object.getColor().getColorMainSecond();
  }
}
