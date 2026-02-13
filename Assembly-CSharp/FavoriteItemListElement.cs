// Decompiled with JetBrains decompiler
// Type: FavoriteItemListElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FavoriteItemListElement : WindowListElementBase<Item, ItemData>
{
  public Text name_text;
  public CountUpOnClick kills_text;
  public CountUpOnClick age_text;
  public CountUpOnClick owners_text;
  public CountUpOnClick damage_text;
  public CountUpOnClick armor_text;
  public CountUpOnClick durability_text;
  [SerializeField]
  private UiUnitAvatarElement _unit_avatar_element;
  [SerializeField]
  private CityBanner _banner_city;
  [SerializeField]
  private GameObject _ownerless;
  private IconOutline _outline;

  internal override void show(Item pItem)
  {
    base.show(pItem);
    this.clear();
    this.name_text.text = pItem.getName();
    ((Graphic) this.name_text).color = Toolbox.makeColor(pItem.getQualityColor());
    this.kills_text.setValue(pItem.data.kills);
    this.age_text.setValue(pItem.getAge());
    this.damage_text.setValue((int) pItem.getFullStats()["damage"]);
    this.armor_text.setValue((int) pItem.getFullStats()["armor"]);
    this.durability_text.setValue(pItem.getDurabilityCurrent());
    if (pItem.hasActor())
    {
      ((Component) this._unit_avatar_element).gameObject.SetActive(true);
      this._unit_avatar_element.show(pItem.getActor());
    }
    else if (pItem.hasCity())
    {
      ((Component) this._banner_city).gameObject.SetActive(true);
      this._banner_city.load((NanoObject) pItem.getCity());
    }
    else
      this._ownerless.SetActive(true);
  }

  protected override void tooltipAction()
  {
    Tooltip.show((object) this, "equipment", new TooltipData()
    {
      item = this.meta_object
    });
  }

  private void clear()
  {
    ((Component) this._unit_avatar_element).gameObject.SetActive(false);
    ((Component) this._banner_city).gameObject.SetActive(false);
    this._ownerless.SetActive(false);
  }
}
