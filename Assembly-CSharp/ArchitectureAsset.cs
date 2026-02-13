// Decompiled with JetBrains decompiler
// Type: ArchitectureAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class ArchitectureAsset : Asset
{
  public bool generate_buildings;
  public string generation_target;
  public bool spread_biome;
  public string spread_biome_id;
  [DefaultValue(true)]
  public bool burnable_buildings = true;
  [DefaultValue(true)]
  public bool acid_affected_buildings = true;
  [DefaultValue(true)]
  public bool has_shadows = true;
  [DefaultValue("building")]
  public string material = "building";
  public Dictionary<string, string> building_ids_for_construction;
  public string[] styled_building_orders;
  public (string, string)[] shared_building_orders;
  [DefaultValue("boat_fishing")]
  public string actor_asset_id_boat_fishing = "boat_fishing";
  public string actor_asset_id_trading;
  public string actor_asset_id_transport;

  public void addBuildingOrderKey(string pKey, string pID)
  {
    if (this.building_ids_for_construction == null)
      this.building_ids_for_construction = new Dictionary<string, string>();
    this.building_ids_for_construction[pKey] = pID;
  }

  public void replaceSharedID(string pID, string pNewID)
  {
    for (int index = 0; index < this.shared_building_orders.Length; ++index)
    {
      if (this.shared_building_orders[index].Item1 == pID)
      {
        this.shared_building_orders[index].Item2 = pNewID;
        break;
      }
    }
  }

  public BuildingAsset getBuilding(string pOrderID)
  {
    string buildingId = this.getBuildingID(pOrderID);
    return AssetManager.buildings.get(buildingId);
  }

  public string getBuildingID(string pOrderID) => this.building_ids_for_construction[pOrderID];
}
