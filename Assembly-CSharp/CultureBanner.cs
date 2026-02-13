// Decompiled with JetBrains decompiler
// Type: CultureBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CultureBanner : BannerGeneric<Culture, CultureData>
{
  protected override MetaType meta_type => MetaType.Culture;

  protected override string tooltip_id => "culture";

  protected override void loadPartBackground()
  {
    this.part_background = ((Component) ((Component) this).transform.FindRecursive("Decor")).GetComponent<Image>();
  }

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.culture = this.meta_object;
    return tooltipData;
  }

  protected override void setupBanner()
  {
    base.setupBanner();
    this.part_icon.sprite = this.meta_object.getElementSprite();
    this.part_background.sprite = this.meta_object.getDecorSprite();
    ColorAsset color = this.meta_object.getColor();
    ((Graphic) this.part_icon).color = color.getColorBanner();
    ((Graphic) this.part_background).color = color.getColorMainSecond();
    ((Graphic) this.part_frame).color = color.getColorMainSecond();
  }
}
