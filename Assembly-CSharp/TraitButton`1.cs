// Decompiled with JetBrains decompiler
// Type: TraitButton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TraitButton<TTrait> : AugmentationButton<TTrait> where TTrait : BaseTrait<TTrait>
{
  public override void load(TTrait pElement)
  {
    this.create();
    this.augmentation_asset = pElement;
    this.image.sprite = this.augmentation_asset.getSprite();
    ((Object) ((Component) this).gameObject).name = $"{this.getElementType()}_{this.augmentation_asset.id}";
    this.loadLegendaryOutline();
  }

  internal virtual void load(string pElementID) => throw new NotImplementedException();

  protected override void Update()
  {
    if (this.is_editor_button)
      return;
    if (this.augmentation_asset.unlocked_with_achievement)
      ((Component) this.locked_bg).gameObject.SetActive(false);
    else
      ((Component) this.locked_bg).gameObject.SetActive(!this.augmentation_asset.isAvailable());
  }

  protected override void fillTooltipData(TTrait pTrait)
  {
    TooltipData pData = this.tooltipDataBuilder();
    pData.is_editor_augmentation_button = this.is_editor_button;
    Tooltip.show((object) this, this.tooltip_type, pData);
  }

  protected override bool unlockElement() => this.augmentation_asset.unlock(true);

  protected override string getElementType() => "trait";

  protected override void startSignal()
  {
    AchievementLibrary.traits_explorer_40.checkBySignal();
    AchievementLibrary.traits_explorer_60.checkBySignal();
    AchievementLibrary.traits_explorer_90.checkBySignal();
  }

  public override string getElementId() => this.augmentation_asset.id;

  protected override Rarity getRarity() => this.augmentation_asset.rarity;
}
