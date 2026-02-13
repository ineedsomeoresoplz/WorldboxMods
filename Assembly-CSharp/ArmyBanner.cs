// Decompiled with JetBrains decompiler
// Type: ArmyBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ArmyBanner : BannerGeneric<Army, ArmyData>
{
  [SerializeField]
  private Image _species_icon;

  protected override MetaType meta_type => MetaType.Army;

  protected override string tooltip_id => "army";

  protected override void setupBanner()
  {
    base.setupBanner();
    Kingdom kingdom = this.meta_object.getKingdom();
    this.part_background.sprite = kingdom.getElementBackground();
    this.part_icon.sprite = kingdom.getElementIcon();
    ColorAsset color1 = kingdom.getColor();
    Color colorMainSecond = color1.getColorMainSecond();
    Color colorBanner = color1.getColorBanner();
    Color color2 = Color.Lerp(colorMainSecond, Color.black, 0.05f);
    Color color3 = Color.Lerp(colorBanner, Color.black, 0.05f);
    ((Graphic) this.part_background).color = color2;
    ((Graphic) this.part_icon).color = color3;
    ((Component) this._species_icon).gameObject.SetActive(false);
  }

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.army = this.meta_object;
    return tooltipData;
  }
}
