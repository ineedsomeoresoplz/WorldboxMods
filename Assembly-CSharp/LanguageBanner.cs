// Decompiled with JetBrains decompiler
// Type: LanguageBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class LanguageBanner : BannerGeneric<Language, LanguageData>
{
  protected override MetaType meta_type => MetaType.Language;

  protected override string tooltip_id => "language";

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.language = this.meta_object;
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
  }
}
