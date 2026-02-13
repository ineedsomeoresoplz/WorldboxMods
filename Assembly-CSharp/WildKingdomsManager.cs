// Decompiled with JetBrains decompiler
// Type: WildKingdomsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class WildKingdomsManager : MetaSystemManager<Kingdom, KingdomData>
{
  public static Kingdom abandoned;
  public static Kingdom ruins;
  public static Kingdom neutral;
  public static Kingdom nature;
  protected readonly Dictionary<string, Kingdom> _dict = new Dictionary<string, Kingdom>();
  private bool _dirty_buildings = true;
  private long _latest;

  public WildKingdomsManager()
  {
    this.type_id = "kingdom_wild";
    foreach (KingdomAsset pAsset in AssetManager.kingdoms.list)
      this.newWildKingdom(pAsset);
    WildKingdomsManager.abandoned = this.get(nameof (abandoned));
    WildKingdomsManager.ruins = this.get(nameof (ruins));
    WildKingdomsManager.nature = this.get(nameof (nature));
    WildKingdomsManager.neutral = this.get(nameof (neutral));
    WildKingdomsManager.neutral.data.original_actor_asset = "druid";
  }

  public override void startCollectHistoryData()
  {
  }

  public override void clearLastYearStats()
  {
  }

  private Kingdom newWildKingdom(KingdomAsset pAsset)
  {
    long pSpecialID = this._latest--;
    if (pSpecialID == -1L)
      pSpecialID = this._latest--;
    Kingdom kingdom = this.newObject(pSpecialID);
    kingdom.asset = pAsset;
    this._dict.Add(kingdom.asset.id, kingdom);
    kingdom.createWildKingdom();
    if (pAsset.default_civ_color_index != -1)
      kingdom.data.setColorID(pAsset.default_civ_color_index);
    kingdom.data.name = pAsset.id;
    return kingdom;
  }

  public override void clear()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) this)
    {
      kingdom.clearListUnits();
      kingdom.clearBuildingList();
    }
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyWild = World.world.units.units_only_wild;
    for (int index = 0; index < unitsOnlyWild.Count; ++index)
    {
      Actor pActor = unitsOnlyWild[index];
      Kingdom kingdom = pActor.kingdom;
      if (kingdom != null && kingdom.isDirtyUnits())
        kingdom.listUnit(pActor);
    }
  }

  public void beginChecksBuildings()
  {
    if (this._dirty_buildings)
      this.updateDirtyBuildings();
    this._dirty_buildings = false;
  }

  private void updateDirtyBuildings()
  {
    this.clearAllBuildingLists();
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (building.isAlive() && building.kingdom.wild)
        building.kingdom.listBuilding(building);
    }
  }

  public void setDirtyBuildings() => this._dirty_buildings = true;

  private void clearAllBuildingLists()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) this)
      kingdom.clearBuildingList();
  }

  public override void checkDeadObjects()
  {
  }

  public Kingdom get(string pID)
  {
    if (string.IsNullOrEmpty(pID))
      return (Kingdom) null;
    Kingdom kingdom;
    this._dict.TryGetValue(pID, out kingdom);
    return kingdom;
  }

  public override void removeObject(Kingdom pObject)
  {
    this._dict.Remove(pObject.asset.id);
    base.removeObject(pObject);
  }
}
