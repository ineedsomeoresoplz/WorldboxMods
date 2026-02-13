// Decompiled with JetBrains decompiler
// Type: CityEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class CityEquipment : IDisposable
{
  internal Dictionary<EquipmentType, List<long>> items_dicts;
  public List<long> item_storage_weapons = new List<long>();
  public List<long> item_storage_helmets = new List<long>();
  public List<long> item_storage_armor = new List<long>();
  public List<long> item_storage_boots = new List<long>();
  public List<long> item_storage_rings = new List<long>();
  public List<long> item_storage_amulets = new List<long>();

  public CityEquipment() => this.init();

  internal void init()
  {
    if (this.items_dicts == null)
      this.items_dicts = new Dictionary<EquipmentType, List<long>>();
    else
      this.items_dicts.Clear();
    this.items_dicts.Add(EquipmentType.Weapon, this.item_storage_weapons);
    this.items_dicts.Add(EquipmentType.Helmet, this.item_storage_helmets);
    this.items_dicts.Add(EquipmentType.Armor, this.item_storage_armor);
    this.items_dicts.Add(EquipmentType.Boots, this.item_storage_boots);
    this.items_dicts.Add(EquipmentType.Ring, this.item_storage_rings);
    this.items_dicts.Add(EquipmentType.Amulet, this.item_storage_amulets);
  }

  public void clearItems()
  {
    foreach (List<long> allEquipmentList in this.getAllEquipmentLists())
    {
      foreach (long pID in allEquipmentList)
        World.world.items.get(pID)?.clearCity();
    }
    this.clearCollections();
  }

  public int countItems()
  {
    int num = 0;
    foreach (List<long> allEquipmentList in this.getAllEquipmentLists())
      num += allEquipmentList.Count;
    return num;
  }

  public bool hasAnyItem()
  {
    foreach (List<long> allEquipmentList in this.getAllEquipmentLists())
    {
      if (allEquipmentList.Count > 0)
        return true;
    }
    return false;
  }

  public void addItem(City pCity, Item pItem, List<long> pList = null)
  {
    EquipmentAsset asset = pItem.getAsset();
    if (pList == null)
      pList = this.getEquipmentList(asset.equipment_type);
    pItem.setInCityStorage(pCity);
    pList.Add(pItem.id);
  }

  public List<long> getEquipmentList(EquipmentType pType) => this.items_dicts[pType];

  public IEnumerable<List<long>> getAllEquipmentLists()
  {
    foreach (List<long> allEquipmentList in this.items_dicts.Values)
      yield return allEquipmentList;
  }

  public void loadFromSave(City pCity)
  {
    this.init();
    foreach (List<long> allEquipmentList in this.getAllEquipmentLists())
      allEquipmentList.RemoveAll((Predicate<long>) (pID =>
      {
        Item obj = World.world.items.get(pID);
        return obj == null || obj.getAsset() == null;
      }));
    foreach (List<long> allEquipmentList in this.getAllEquipmentLists())
    {
      for (int index = 0; index < allEquipmentList.Count; ++index)
        World.world.items.get(allEquipmentList[index]).setInCityStorage(pCity);
    }
  }

  public void Dispose()
  {
    this.items_dicts?.Clear();
    this.clearCollections();
  }

  private void clearCollections()
  {
    this.item_storage_weapons.Clear();
    this.item_storage_helmets.Clear();
    this.item_storage_armor.Clear();
    this.item_storage_boots.Clear();
    this.item_storage_rings.Clear();
    this.item_storage_amulets.Clear();
  }
}
