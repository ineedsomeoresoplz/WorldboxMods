// Decompiled with JetBrains decompiler
// Type: SubspeciesBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SubspeciesBanner : BannerGeneric<Subspecies, SubspeciesData>
{
  private Image _part_bookmark_1;
  private Image _part_bookmark_2;
  public Image unit_sprite;

  protected override MetaType meta_type => MetaType.Subspecies;

  protected override string tooltip_id => "subspecies";

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.subspecies = this.meta_object;
    return tooltipData;
  }

  protected override void setupParts()
  {
    base.setupParts();
    this._part_bookmark_1 = ((Component) ((Component) this).transform.FindRecursive("Bookmark 1"))?.GetComponent<Image>();
    this._part_bookmark_2 = ((Component) ((Component) this).transform.FindRecursive("Bookmark 2"))?.GetComponent<Image>();
  }

  protected override void setupBanner()
  {
    base.setupBanner();
    this.part_background.sprite = this.meta_object.getSpriteBackground();
    this.part_icon.sprite = this.meta_object.getSpriteIcon();
    ColorAsset color = this.meta_object.getColor();
    ((Graphic) this._part_bookmark_1).color = color.getColorMainSecond();
    ((Graphic) this._part_bookmark_2).color = color.getColorMain();
    this.unit_sprite.sprite = this.meta_object.getUnitSpriteForBanner();
  }
}
