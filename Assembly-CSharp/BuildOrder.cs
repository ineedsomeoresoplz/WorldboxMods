// Decompiled with JetBrains decompiler
// Type: BuildOrder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class BuildOrder : Asset
{
  public int required_pop;
  public int required_buildings;
  public int limit_type;
  public bool check_full_village;
  public bool check_house_limit;
  public int min_zones;
  public bool upgrade;
  public string[] requirements_orders;
  public string[] requirements_types;

  public BuildingAsset getBuildingAsset(City pCity, string pOrderID = null)
  {
    if (string.IsNullOrEmpty(pOrderID))
      pOrderID = this.id;
    return pCity.getActorAsset().architecture_asset.getBuilding(pOrderID);
  }
}
