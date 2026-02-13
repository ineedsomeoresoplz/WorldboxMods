// Decompiled with JetBrains decompiler
// Type: CityResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class CityResources : IDisposable
{
  [NonSerialized]
  private Dictionary<string, CityStorageSlot> _resources = new Dictionary<string, CityStorageSlot>();
  [NonSerialized]
  private List<CityStorageSlot> _list_food = new List<CityStorageSlot>();
  [NonSerialized]
  private List<CityStorageSlot> _list_other = new List<CityStorageSlot>();
  public List<CityStorageSlot> saved_resources;

  public void loadFromSave()
  {
    if (this.saved_resources == null)
      return;
    foreach (CityStorageSlot savedResource in this.saved_resources)
    {
      if (AssetManager.resources.get(savedResource.id) != null && savedResource.amount >= 0)
      {
        savedResource.create(savedResource.id);
        this.putToDict(savedResource);
      }
    }
  }

  public int get(string pRes)
  {
    CityStorageSlot cityStorageSlot;
    return this._resources.TryGetValue(pRes, out cityStorageSlot) ? cityStorageSlot.amount : 0;
  }

  public int change(string pRes, int pAmount = 1)
  {
    if (DebugConfig.isOn(DebugOption.CityInfiniteResources))
      pAmount = 999;
    CityStorageSlot cityStorageSlot;
    int num;
    if (this._resources.TryGetValue(pRes, out cityStorageSlot))
    {
      cityStorageSlot.amount += pAmount;
      if (cityStorageSlot.amount > cityStorageSlot.asset.maximum)
        cityStorageSlot.amount = cityStorageSlot.asset.maximum;
      num = cityStorageSlot.amount;
    }
    else
      num = this.addNew(pRes, pAmount);
    return num;
  }

  private int addNew(string pResID, int pAmount)
  {
    CityStorageSlot pRes = new CityStorageSlot(pResID);
    pRes.amount = pAmount;
    this.putToDict(pRes);
    return pRes.amount;
  }

  public bool hasSpaceForResource(ResourceAsset pAsset) => this.get(pAsset.id) < pAsset.storage_max;

  public bool hasResourcesForNewItems()
  {
    foreach (Asset strategicResourceAsset in AssetManager.resources.strategic_resource_assets)
    {
      if (this.get(strategicResourceAsset.id) > 10)
        return true;
    }
    return false;
  }

  public void set(string pRes, int pAmount)
  {
    CityStorageSlot cityStorageSlot;
    if (this._resources.TryGetValue(pRes, out cityStorageSlot))
      cityStorageSlot.amount = pAmount;
    else
      this.addNew(pRes, pAmount);
  }

  private void putToDict(CityStorageSlot pRes)
  {
    if (this._resources.ContainsKey(pRes.id))
      return;
    this._resources.Add(pRes.id, pRes);
    if (pRes.asset.food)
      this._list_food.Add(pRes);
    else
      this._list_other.Add(pRes);
  }

  public ResourceAsset getRandomSuitableFood(Subspecies pSubspecies, string pSpecificFood = null)
  {
    if (this._list_food.Count == 0)
      return (ResourceAsset) null;
    if (!string.IsNullOrEmpty(pSpecificFood) && this.get(pSpecificFood) > 0)
      return AssetManager.resources.get(pSpecificFood);
    HashSet<string> allowedFoodByDiet = pSubspecies.getAllowedFoodByDiet();
    return this.getAvailableFoodAsset(this._list_food, allowedFoodByDiet, true) ?? this.getAvailableFoodAsset(this._list_other, allowedFoodByDiet, false);
  }

  private ResourceAsset getAvailableFoodAsset(
    List<CityStorageSlot> pList,
    HashSet<string> pAllowedFood,
    bool pSort)
  {
    if (pSort)
      pList.Sort(new Comparison<CityStorageSlot>(this.foodSorter));
    for (int index = 0; index < pList.Count; ++index)
    {
      CityStorageSlot p = pList[index];
      if (p.amount != 0 && pAllowedFood.Contains(p.id))
        return p.asset;
    }
    return (ResourceAsset) null;
  }

  public int foodSorter(CityStorageSlot o1, CityStorageSlot o2) => o2.amount.CompareTo(o1.amount);

  public int countFood()
  {
    int num = 0;
    foreach (CityStorageSlot cityStorageSlot in this._list_food)
      num += cityStorageSlot.amount;
    return num;
  }

  public ResourceAsset getRandomFoodAsset()
  {
    return this._list_food.Count == 0 ? (ResourceAsset) null : this._list_food.GetRandom<CityStorageSlot>().asset;
  }

  public void save()
  {
    this.saved_resources = new List<CityStorageSlot>();
    foreach (CityStorageSlot slot in this.getSlots())
    {
      if (slot.amount != 0)
        this.saved_resources.Add(slot);
    }
  }

  public IEnumerable<string> getKeys() => (IEnumerable<string>) this._resources.Keys;

  public IEnumerable<CityStorageSlot> getSlots()
  {
    return (IEnumerable<CityStorageSlot>) this._resources.Values;
  }

  public void Dispose()
  {
    this._resources.Clear();
    this._list_food.Clear();
    this._list_other.Clear();
    this.saved_resources?.Clear();
  }
}
