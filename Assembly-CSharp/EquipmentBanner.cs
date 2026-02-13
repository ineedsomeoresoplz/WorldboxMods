// Decompiled with JetBrains decompiler
// Type: EquipmentBanner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EquipmentBanner : BannerGeneric<Item, ItemData>
{
  [SerializeField]
  private IconOutline _outline;
  [SerializeField]
  private Sprite _frame_sprite_legendary;
  [SerializeField]
  private Sprite _frame_sprite_epic;

  protected override MetaType meta_type => MetaType.Item;

  protected override string tooltip_id => "equipment";

  protected override TooltipData getTooltipData()
  {
    TooltipData tooltipData = base.getTooltipData();
    tooltipData.item = this.meta_object;
    return tooltipData;
  }

  protected override void setupBanner()
  {
    base.setupBanner();
    Item metaObject = this.meta_object;
    Rarity quality = metaObject.getQuality();
    this.part_icon.sprite = metaObject.getSprite();
    bool flag = true;
    switch (quality)
    {
      case Rarity.R2_Epic:
        this.part_frame.sprite = this._frame_sprite_epic;
        break;
      case Rarity.R3_Legendary:
        this.part_frame.sprite = this._frame_sprite_legendary;
        break;
      default:
        flag = false;
        break;
    }
    ((Component) this.part_frame).gameObject.SetActive(flag);
    if (quality == Rarity.R3_Legendary)
      this.showOutline();
    else
      ((Component) this._outline).gameObject.SetActive(false);
  }

  private void showOutline() => this._outline.show(RarityLibrary.legendary.color_container);
}
