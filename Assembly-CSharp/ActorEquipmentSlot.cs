// Decompiled with JetBrains decompiler
// Type: ActorEquipmentSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ActorEquipmentSlot
{
  private Item _item;
  public EquipmentType type;

  public ActorEquipmentSlot(EquipmentType pType = EquipmentType.Armor) => this.type = pType;

  public Item getItem() => this._item;

  public bool isEmpty()
  {
    if (this._item == null)
      return true;
    if (!this._item.shouldbe_removed)
      return false;
    Debug.LogError((object) "Item should be removed but it's still in the slot!");
    return true;
  }

  public bool is_empty => this.isEmpty();

  public void takeAwayItem()
  {
    if (this.isEmpty())
      return;
    this._item.clearUnit();
    this._item = (Item) null;
  }

  public void setEmptyDebug() => this._item = (Item) null;

  internal void setItem(Item pItem, Actor pActor)
  {
    if (!this.isEmpty())
      this.takeAwayItem();
    this._item = pItem;
    this._item.setUnitHasIt(pActor);
    pActor.setStatsDirty();
  }

  public bool canChangeSlot() => this.isEmpty() || !this.getItem().isCursed();
}
