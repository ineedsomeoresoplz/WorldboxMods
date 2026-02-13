// Decompiled with JetBrains decompiler
// Type: ActorEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
public class ActorEquipment : IEnumerable<ActorEquipmentSlot>, IEnumerable
{
  public const int SLOTS_AMOUNT = 6;
  public const string NONE = "none";
  public ActorEquipmentSlot helmet = new ActorEquipmentSlot(EquipmentType.Helmet);
  public ActorEquipmentSlot armor = new ActorEquipmentSlot();
  public ActorEquipmentSlot weapon = new ActorEquipmentSlot(EquipmentType.Weapon);
  public ActorEquipmentSlot boots = new ActorEquipmentSlot(EquipmentType.Boots);
  public ActorEquipmentSlot ring = new ActorEquipmentSlot(EquipmentType.Ring);
  public ActorEquipmentSlot amulet = new ActorEquipmentSlot(EquipmentType.Amulet);
  private Dictionary<EquipmentType, ActorEquipmentSlot> _dictionary;

  public ActorEquipment() => this.initDictionary();

  public void destroyAllEquipment()
  {
    foreach (ActorEquipmentSlot actorEquipmentSlot in this._dictionary.Values)
    {
      if (!actorEquipmentSlot.isEmpty())
        actorEquipmentSlot.takeAwayItem();
    }
  }

  private void initDictionary()
  {
    this._dictionary = new Dictionary<EquipmentType, ActorEquipmentSlot>();
    this._dictionary.Add(EquipmentType.Helmet, this.helmet);
    this._dictionary.Add(EquipmentType.Armor, this.armor);
    this._dictionary.Add(EquipmentType.Weapon, this.weapon);
    this._dictionary.Add(EquipmentType.Boots, this.boots);
    this._dictionary.Add(EquipmentType.Ring, this.ring);
    this._dictionary.Add(EquipmentType.Amulet, this.amulet);
  }

  public bool hasItems()
  {
    foreach (ActorEquipmentSlot actorEquipmentSlot in this._dictionary.Values)
    {
      if (!actorEquipmentSlot.isEmpty())
        return true;
    }
    return false;
  }

  public IEnumerable<Item> getItems()
  {
    foreach (ActorEquipmentSlot actorEquipmentSlot in this._dictionary.Values)
    {
      if (!actorEquipmentSlot.isEmpty())
        yield return actorEquipmentSlot.getItem();
    }
  }

  public List<long> getDataForSave()
  {
    List<long> longList = new List<long>();
    foreach (ActorEquipmentSlot actorEquipmentSlot in this)
      longList.Add(actorEquipmentSlot.getItem().data.id);
    return longList.Count == 0 ? (List<long>) null : longList;
  }

  public void load(List<long> pList, Actor pActor)
  {
    if (pList == null || pList.Count == 0)
      return;
    foreach (long p in pList)
    {
      Item pItem = World.world.items.get(p);
      if (pItem != null)
      {
        EquipmentAsset asset = pItem.getAsset();
        if (asset != null)
          this.getSlot(asset.equipment_type).setItem(pItem, pActor);
      }
    }
    this.initDictionary();
  }

  public void setItem(Item pItem, Actor pActor)
  {
    this.getSlot(pItem.getAsset().equipment_type).setItem(pItem, pActor);
  }

  public ActorEquipmentSlot getSlot(EquipmentType pType) => this._dictionary[pType];

  public IEnumerator<ActorEquipmentSlot> GetEnumerator()
  {
    foreach (ActorEquipmentSlot actorEquipmentSlot in this._dictionary.Values)
    {
      if (!actorEquipmentSlot.isEmpty())
        yield return actorEquipmentSlot;
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
