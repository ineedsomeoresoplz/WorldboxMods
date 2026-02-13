// Decompiled with JetBrains decompiler
// Type: KingdomBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KingdomBanner : BannerGeneric<Kingdom, KingdomData>
{
  public bool diplo_banner;
  [SerializeField]
  protected Image background;
  [SerializeField]
  protected Image icon;
  [SerializeField]
  protected Image dead_image;
  [SerializeField]
  protected Image left_image;
  [SerializeField]
  protected Image winner_image;
  [SerializeField]
  protected Image loser_image;
  private Color _bgcolor;
  private Color _iconcolor;

  protected override MetaType meta_type => MetaType.Kingdom;

  protected override string tooltip_id => "kingdom";

  protected override void setupTooltip() => base.setupTooltip();

  protected override void setupBanner()
  {
    base.setupBanner();
    ((Component) this.dead_image).gameObject.SetActive(false);
    ((Component) this.left_image).gameObject.SetActive(false);
    ((Component) this.winner_image).gameObject.SetActive(false);
    ((Component) this.loser_image).gameObject.SetActive(false);
    this.part_background.sprite = this.meta_object.getElementBackground();
    this.part_icon.sprite = this.meta_object.getElementIcon();
    ColorAsset color = this.meta_object.getColor();
    ((Graphic) this.part_background).color = color.getColorMainSecond();
    ((Graphic) this.part_icon).color = color.getColorBanner();
  }

  public override void load(NanoObject pObject)
  {
    base.load(pObject);
    if (!this.meta_object.hasDied())
      return;
    this.showAsDead();
  }

  private void showAsDead() => ((Component) this.dead_image).gameObject.SetActive(true);

  public void hasLeftWar() => ((Component) this.left_image).gameObject.SetActive(true);

  public void hasWon() => ((Component) this.winner_image).gameObject.SetActive(true);

  public void hasLost() => ((Component) this.loser_image).gameObject.SetActive(true);

  protected override void tooltipAction()
  {
    if (this.meta_object == null)
      return;
    string pType = this.meta_object.hasDied() ? "kingdom_dead" : "kingdom";
    string str = string.Empty;
    if (this.diplo_banner)
    {
      pType = "kingdom_diplo";
      str = "kingdom_diplo";
    }
    TooltipData tooltipData = this.getTooltipData();
    tooltipData.tip_name = str;
    Tooltip.show((object) this, pType, tooltipData);
  }

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.kingdom = this.meta_object;
    return tooltipData;
  }
}
