// Decompiled with JetBrains decompiler
// Type: TooltipActorEquipmentsRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TooltipActorEquipmentsRow : TooltipItemsRow<TooltipOutlineItem>
{
  protected override void loadItems()
  {
    this.items_pool.clear();
    Actor actor = this.tooltip_data.actor;
    if (!actor.canUseItems() || actor.equipment == null || !actor.equipment.hasItems())
    {
      ((Component) this).gameObject.SetActive(false);
    }
    else
    {
      bool flag = false;
      foreach (ActorEquipmentSlot actorEquipmentSlot in actor.equipment)
      {
        Item obj = actorEquipmentSlot.getItem();
        if (obj != null)
        {
          flag = true;
          TooltipOutlineItem next = this.items_pool.getNext();
          next.image.sprite = obj.getSprite();
          if (obj.getQuality() == Rarity.R3_Legendary)
            next.outline.show(RarityLibrary.legendary.color_container);
          else
            ((Component) next.outline).gameObject.SetActive(false);
        }
      }
      ((Component) this).gameObject.SetActive(flag);
    }
  }
}
