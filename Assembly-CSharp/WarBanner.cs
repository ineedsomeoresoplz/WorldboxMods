// Decompiled with JetBrains decompiler
// Type: WarBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WarBanner : BannerGeneric<War, WarData>
{
  public KingdomBanner banner_kingdom1;
  public KingdomBanner banner_kingdom2;
  public Image war_icon;
  public bool diplo_banner;
  public Image total_war_icon;
  private bool diplo_banner_initiated;
  public bool buttons_enabled;

  protected override MetaType meta_type => MetaType.War;

  protected override string tooltip_id => "war";

  protected override void setupBanner()
  {
    base.setupBanner();
    ((Component) this.banner_kingdom1).gameObject.SetActive(false);
    ((Component) this.banner_kingdom2).gameObject.SetActive(false);
    ((Component) this.total_war_icon).gameObject.SetActive(false);
    Kingdom mainAttacker = this.meta_object.getMainAttacker();
    if (!mainAttacker.isRekt())
    {
      ((Component) this.banner_kingdom1).gameObject.SetActive(true);
      this.banner_kingdom1.load((NanoObject) mainAttacker);
    }
    if (this.meta_object.isTotalWar())
    {
      ((Component) this.total_war_icon).gameObject.SetActive(true);
    }
    else
    {
      Kingdom mainDefender = this.meta_object.getMainDefender();
      if (!mainDefender.isRekt())
      {
        ((Component) this.banner_kingdom2).gameObject.SetActive(true);
        this.banner_kingdom2.load((NanoObject) mainDefender);
      }
    }
    switch (this.meta_object.data.winner)
    {
      case WarWinner.Attackers:
        this.banner_kingdom1.hasWon();
        if (!this.meta_object.isTotalWar())
        {
          this.banner_kingdom2.hasLost();
          break;
        }
        break;
      case WarWinner.Defenders:
        this.banner_kingdom1.hasLost();
        if (!this.meta_object.isTotalWar())
        {
          this.banner_kingdom2.hasWon();
          break;
        }
        break;
    }
    this.war_icon.sprite = SpriteTextureLoader.getSprite(this.meta_object.getAsset().path_icon);
    if (!this.buttons_enabled)
      return;
    this.initDiploBanner();
  }

  private void initDiploBanner()
  {
    if (this.diplo_banner_initiated)
      return;
    this.diplo_banner_initiated = true;
    this.diplo_banner = true;
    ((Behaviour) ((Component) this).GetComponent<Button>()).enabled = true;
    ((Behaviour) ((Component) this).GetComponent<TipButton>()).enabled = true;
    UiButtonHoverAnimation component = ((Component) this).GetComponent<UiButtonHoverAnimation>();
    ((Behaviour) component).enabled = true;
    component.scale_size = 1.1f;
    component.default_scale = new Vector3(0.8f, 0.8f, 0.8f);
    this.setupTooltip();
  }

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.war = this.meta_object;
    return tooltipData;
  }
}
