// Decompiled with JetBrains decompiler
// Type: CityBuildOrderAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityPools;

#nullable disable
[Serializable]
public class CityBuildOrderAsset : Asset
{
  [NonSerialized]
  public string[] list_for_generation;
  public List<BuildOrder> list = new List<BuildOrder>();

  public void addUpgrade(
    string pID,
    int pLimitType = 0,
    int pPop = 0,
    int pBuildings = 0,
    bool pCheckFullVillage = false,
    bool pZonesCheck = false,
    int pMinZones = 0)
  {
    this.addBuilding(pID, pLimitType, pPop, pBuildings, pCheckFullVillage, pZonesCheck, pMinZones).upgrade = true;
  }

  public BuildOrder addBuilding(
    string pID,
    int pLimitType = 0,
    int pPop = 0,
    int pBuildings = 0,
    bool pCheckFullVillage = false,
    bool pCheckHouseLimit = false,
    int pMinZones = 0)
  {
    BuildOrder buildOrder = new BuildOrder();
    buildOrder.id = pID;
    buildOrder.limit_type = pLimitType;
    buildOrder.required_pop = pPop;
    buildOrder.required_buildings = pBuildings;
    buildOrder.check_full_village = pCheckFullVillage;
    buildOrder.check_house_limit = pCheckHouseLimit;
    buildOrder.min_zones = pMinZones;
    this.list.Add(buildOrder);
    BuildOrderLibrary.b = buildOrder;
    return buildOrder;
  }

  public void prepareForAssetGeneration()
  {
    HashSet<string> pHashSet = UnsafeCollectionPool<HashSet<string>, string>.Get();
    foreach (BuildOrder buildOrder in this.list)
    {
      pHashSet.Add(buildOrder.id);
      if (buildOrder.requirements_orders != null)
        pHashSet.UnionWith((IEnumerable<string>) buildOrder.requirements_orders);
    }
    this.list_for_generation = pHashSet.ToArray<string>();
    UnsafeCollectionPool<HashSet<string>, string>.Release(pHashSet);
  }
}
