// Decompiled with JetBrains decompiler
// Type: ReligionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ReligionManager : MetaSystemManager<Religion, ReligionData>
{
  private bool _dirty_kingdoms = true;
  private bool _dirty_cities = true;

  public ReligionManager() => this.type_id = "religion";

  public Religion newReligion(Actor pFounder, bool pAddDefaultTraits)
  {
    ++World.world.game_stats.data.religionsCreated;
    ++World.world.map_stats.religionsCreated;
    Religion religion = this.newObject();
    religion.newReligion(pFounder, pFounder.current_tile, pAddDefaultTraits);
    MetaHelper.addRandomTrait<ReligionTrait>((ITraitsOwner<ReligionTrait>) religion, (BaseTraitLibrary<ReligionTrait>) AssetManager.religion_traits);
    this.addRandomTraitFromBiomeToReligion(religion, pFounder.current_tile);
    return religion;
  }

  private void addRandomTraitFromBiomeToReligion(Religion pReligion, WorldTile pTile)
  {
    pReligion.addRandomTraitFromBiome<ReligionTrait>(pTile, pTile.Type.biome_asset?.spawn_trait_religion, (AssetLibrary<ReligionTrait>) AssetManager.religion_traits);
  }

  public Religion getMainReligion(List<Actor> pUnitList)
  {
    for (int index = 0; index < pUnitList.Count; ++index)
    {
      Actor pUnit = pUnitList[index];
      if (pUnit.hasReligion())
        this.countMetaObject(pUnit.religion);
    }
    return this.getMostUsedMetaObject();
  }

  public override void removeObject(Religion pObject)
  {
    ++World.world.game_stats.data.religionsForgotten;
    ++World.world.map_stats.religionsForgotten;
    base.removeObject(pObject);
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Religion religion = pActor.religion;
      if (religion != null && religion.isDirtyUnits())
        religion.listUnit(pActor);
    }
  }

  public void beginChecksKingdoms()
  {
    if (this._dirty_kingdoms)
      this.updateDirtyKingdoms();
    this._dirty_kingdoms = false;
  }

  private void updateDirtyKingdoms()
  {
    this.clearAllKingdomLists();
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.hasReligion())
        kingdom.religion.listKingdom(kingdom);
    }
  }

  private void clearAllKingdomLists()
  {
    foreach (Religion religion in (CoreSystemManager<Religion, ReligionData>) this)
      religion.clearListKingdoms();
  }

  public void beginChecksCities()
  {
    if (this._dirty_cities)
      this.updateDirtyCities();
    this._dirty_cities = false;
  }

  private void updateDirtyCities()
  {
    this.clearAllCitiesLists();
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city.hasReligion())
        city.religion.listCity(city);
    }
  }

  private void clearAllCitiesLists()
  {
    foreach (Religion religion in (CoreSystemManager<Religion, ReligionData>) this)
      religion.clearListCities();
  }

  public void setDirtyKingdoms() => this._dirty_kingdoms = true;

  public void setDirtyCities() => this._dirty_cities = true;

  public override bool isLocked()
  {
    return this.isUnitsDirty() || this._dirty_cities || this._dirty_kingdoms;
  }
}
