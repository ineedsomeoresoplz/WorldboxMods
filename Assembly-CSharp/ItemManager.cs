// Decompiled with JetBrains decompiler
// Type: ItemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityPools;

#nullable disable
public class ItemManager : CoreSystemManager<Item, ItemData>
{
  private HashSet<string> unique_legendary_names = new HashSet<string>();
  private List<Item> _to_remove = new List<Item>();
  private bool _dirty;

  public ItemManager()
  {
    this.type_id = "item";
    MapBox.on_world_loaded += (Action) (() => this.diagnostic());
  }

  public bool isDirty() => this._dirty;

  public void setDirty() => this._dirty = true;

  public Item newItem(EquipmentAsset pAsset)
  {
    Item obj = this.newObject();
    obj.newItem(pAsset);
    return obj;
  }

  public void diagnostic()
  {
    Dictionary<Item, int> dictionary1 = UnsafeCollectionPool<Dictionary<Item, int>, KeyValuePair<Item, int>>.Get();
    Dictionary<Item, int> dictionary2 = UnsafeCollectionPool<Dictionary<Item, int>, KeyValuePair<Item, int>>.Get();
    foreach (CoreSystemObject<CityData> city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      foreach (List<long> allEquipmentList in city.data.equipment.getAllEquipmentLists())
      {
        foreach (long pID in allEquipmentList)
        {
          Item key = this.get(pID);
          if (key != null)
          {
            if (!dictionary1.ContainsKey(key))
              dictionary1.Add(key, 0);
            dictionary1[key]++;
          }
        }
      }
    }
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.hasEquipment())
      {
        foreach (ActorEquipmentSlot actorEquipmentSlot in unit.equipment)
        {
          Item key = actorEquipmentSlot.getItem();
          if (key != null)
          {
            if (!dictionary2.ContainsKey(key))
              dictionary2.Add(key, 0);
            dictionary2[key]++;
          }
        }
      }
    }
    foreach (Item key in this.list)
    {
      if (dictionary1.ContainsKey(key) && dictionary2.ContainsKey(key))
        Debug.LogError((object) ("Item Error. Item in city and in unit " + key.id.ToString()));
    }
    UnsafeCollectionPool<Dictionary<Item, int>, KeyValuePair<Item, int>>.Release(dictionary1);
    UnsafeCollectionPool<Dictionary<Item, int>, KeyValuePair<Item, int>>.Release(dictionary2);
  }

  public override Item loadObject(ItemData pData)
  {
    return AssetManager.items.get(pData.asset_id) == null ? (Item) null : base.loadObject(pData);
  }

  private List<ItemModAsset> getModPool(EquipmentType pType)
  {
    List<ItemModAsset> pool;
    switch (pType)
    {
      case EquipmentType.Weapon:
        pool = AssetManager.items_modifiers.pools["weapon"];
        break;
      case EquipmentType.Ring:
      case EquipmentType.Amulet:
        pool = AssetManager.items_modifiers.pools["accessory"];
        break;
      default:
        pool = AssetManager.items_modifiers.pools["armor"];
        break;
    }
    return pool;
  }

  private ItemModAsset getRandomModFromPool(EquipmentType pType)
  {
    return this.getModPool(pType).GetRandom<ItemModAsset>();
  }

  public void generateModsFor(Item pItem, int pTries = 1, Actor pActor = null, bool pAddName = true)
  {
    EquipmentAsset asset = pItem.getAsset();
    using (ListPool<string> pList = new ListPool<string>())
    {
      for (int index = 0; index < pTries; ++index)
      {
        if (!Randy.randomBool())
        {
          ItemModAsset randomModFromPool = this.getRandomModFromPool(asset.equipment_type);
          if (randomModFromPool.mod_can_be_given)
          {
            bool addMod = this.tryToAddMod(pItem, randomModFromPool);
            string pName;
            if (pAddName & addMod && this.checkModName(pItem, randomModFromPool, asset, pActor, out pName))
              pList.Add(pName);
          }
        }
      }
      if (asset.item_modifiers != null)
      {
        for (int index = 0; index < asset.item_modifiers.Length; ++index)
        {
          ItemModAsset itemModifier = asset.item_modifiers[index];
          bool addMod = this.tryToAddMod(pItem, itemModifier);
          string pName;
          if (pAddName & addMod && this.checkModName(pItem, itemModifier, asset, pActor, out pName))
            pList.Add(pName);
        }
      }
      pList.RemoveAll((Predicate<string>) (tName => string.IsNullOrEmpty(tName)));
      if (pList.Count <= 0)
        return;
      pItem.setName(Randy.getRandom<string>(pList));
    }
  }

  public Item generateItem(
    EquipmentAsset pItemAsset,
    Kingdom pKingdom = null,
    string pWho = null,
    int pTries = 1,
    Actor pActor = null,
    int pFakeCreationYear = 0,
    bool pByPlayer = false)
  {
    Item pItem = this.newItem(pItemAsset);
    this.generateModsFor(pItem, pTries, pActor);
    pItem.data.asset_id = pItemAsset.id;
    pItem.data.by = pWho;
    if (!pByPlayer && !pActor.isRekt() && pActor.name == pWho)
      pItem.data.creator_id = pActor.getID();
    else
      pItem.data.creator_id = -1L;
    pItem.created_time_unscaled = (double) Time.time;
    pItem.data.created_time -= (double) pFakeCreationYear * 60.0;
    pItem.data.created_by_player = pByPlayer;
    if (pKingdom != null)
    {
      pItem.data.byColor = pKingdom.getColor().color_text;
      pItem.data.creator_kingdom_id = pKingdom.id;
      pItem.data.from = pKingdom.name;
      pItem.data.fromColor = pKingdom.getColor().color_text;
    }
    pItem.initItem();
    return pItem;
  }

  public override void removeObject(Item pObject)
  {
    base.removeObject(pObject);
    pObject.setShouldBeRemoved();
  }

  public override void clear()
  {
    base.clear();
    this.unique_legendary_names.Clear();
  }

  private bool tryToAddMod(Item pItem, ItemModAsset pModAsset) => pItem.addMod(pModAsset);

  private bool checkModName(
    Item pItem,
    ItemModAsset pModAsset,
    EquipmentAsset pItemAsset,
    Actor pActor,
    out string pName)
  {
    pName = (string) null;
    if (pModAsset.quality != Rarity.R3_Legendary)
      return false;
    int num = 0;
    while (string.IsNullOrEmpty(pName) || this.unique_legendary_names.Contains(pName))
    {
      string randomNameTemplate = pItemAsset.getRandomNameTemplate(pActor);
      NameGeneratorAsset pAsset = AssetManager.name_generator.get(randomNameTemplate);
      pName = NameGenerator.generateNameFromTemplate(pAsset, pActor);
      if (++num > 100)
        this.unique_legendary_names.Clear();
    }
    return true;
  }

  public override void checkDeadObjects()
  {
    base.checkDeadObjects();
    if (!this.isDirty())
      return;
    foreach (Item obj in (CoreSystemManager<Item, ItemData>) this)
    {
      if (obj.isReadyForRemoval())
        this._to_remove.Add(obj);
    }
    foreach (Item pObject in this._to_remove)
      this.removeObject(pObject);
    this._to_remove.Clear();
    this._dirty = false;
  }
}
