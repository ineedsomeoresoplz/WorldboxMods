// Decompiled with JetBrains decompiler
// Type: CityBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CityBanner : BannerGeneric<City, CityData>
{
  [SerializeField]
  private Sprite _city_sprite;
  [SerializeField]
  private Sprite _capital_sprite;
  private Image _part_city_icon;

  protected override MetaType meta_type => MetaType.City;

  protected override string tooltip_id => "city";

  protected override void setupBanner()
  {
    base.setupBanner();
    ColorAsset color1 = this.meta_object.kingdom.getColor();
    this.part_background.sprite = this.meta_object.kingdom.getElementBackground();
    this.part_icon.sprite = this.meta_object.kingdom.getElementIcon();
    this._part_city_icon.sprite = DynamicSprites.getIconWithColors(this.meta_object.isCapitalCity() ? this._capital_sprite : this._city_sprite, (PhenotypeAsset) null, color1);
    Color colorMainSecond = color1.getColorMainSecond();
    Color colorBanner = color1.getColorBanner();
    Color color2 = Color.Lerp(colorMainSecond, Color.black, 0.05f);
    Color color3 = Color.Lerp(colorBanner, Color.black, 0.05f);
    ((Graphic) this.part_background).color = color2;
    ((Graphic) this.part_icon).color = color3;
  }

  protected override void setupParts()
  {
    base.setupParts();
    this._part_city_icon = ((Component) ((Component) this).transform.FindRecursive("Foundation")).GetComponent<Image>();
  }

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.city = this.meta_object;
    return tooltipData;
  }
}
