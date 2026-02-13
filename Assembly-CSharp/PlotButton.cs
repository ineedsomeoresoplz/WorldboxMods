// Decompiled with JetBrains decompiler
// Type: PlotButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlotButton : AugmentationButton<PlotAsset>
{
  public override void load(PlotAsset pElement)
  {
    this.create();
    this.augmentation_asset = pElement;
    this.image.sprite = this.augmentation_asset.getSprite();
    ((Object) ((Component) this).gameObject).name = $"{this.getElementType()}_{this.augmentation_asset.id}";
    this.loadLegendaryOutline();
  }

  protected override void Update()
  {
    if (this.is_editor_button)
      return;
    if (this.augmentation_asset.unlocked_with_achievement)
      ((Component) this.locked_bg).gameObject.SetActive(false);
    else
      ((Component) this.locked_bg).gameObject.SetActive(!this.augmentation_asset.isAvailable());
  }

  public override void updateIconColor(bool pSelected)
  {
    if (!this.is_editor_button)
      return;
    if (!this.getElementAsset().isAvailable())
      ((Graphic) this.image).color = Toolbox.color_black;
    else if (pSelected)
      ((Graphic) this.image).color = Toolbox.color_augmentation_selected;
    else if (this.augmentation_asset.canBeDoneByRole(SelectedUnit.unit))
    {
      if ((this.augmentation_asset.check_can_be_forced == null ? 1 : (this.augmentation_asset.check_can_be_forced(SelectedUnit.unit) ? 1 : 0)) == 0)
        ((Graphic) this.image).color = Toolbox.color_gray;
      else
        ((Graphic) this.image).color = Toolbox.color_white;
    }
    else
      ((Graphic) this.image).color = Toolbox.color_gray;
  }

  protected override bool unlockElement() => this.augmentation_asset.unlock();

  protected override void startSignal() => AchievementLibrary.plots_explorer.checkBySignal();

  protected override void fillTooltipData(PlotAsset pElement)
  {
    Tooltip.show((object) this, this.tooltip_type, this.tooltipDataBuilder());
  }

  protected override string tooltip_type => "plot_in_editor";

  protected override TooltipData tooltipDataBuilder()
  {
    return new TooltipData()
    {
      plot_asset = this.augmentation_asset
    };
  }

  protected override string getElementType() => "plot";

  public override string getElementId() => this.augmentation_asset.id;

  protected override Rarity getRarity() => this.augmentation_asset.rarity;
}
