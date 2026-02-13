// Decompiled with JetBrains decompiler
// Type: CultureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CultureManager : MetaSystemManager<Culture, CultureData>
{
  private bool _dirty_kingdoms = true;
  private bool _dirty_cities = true;

  public CultureManager() => this.type_id = "culture";

  public Culture newCulture(Actor pFounder, bool pAddDefaultTraits)
  {
    ++World.world.game_stats.data.culturesCreated;
    ++World.world.map_stats.culturesCreated;
    Culture culture = this.newObject();
    culture.createCulture(pFounder, pAddDefaultTraits);
    this.addRandomTraitFromBiomeToCulture(culture, pFounder.current_tile);
    MetaHelper.addRandomTrait<CultureTrait>((ITraitsOwner<CultureTrait>) culture, (BaseTraitLibrary<CultureTrait>) AssetManager.culture_traits);
    return culture;
  }

  public void addRandomTraitFromBiomeToCulture(Culture pCulture, WorldTile pTile)
  {
    pCulture.addRandomTraitFromBiome<CultureTrait>(pTile, pTile.Type.biome_asset?.spawn_trait_culture, (AssetLibrary<CultureTrait>) AssetManager.culture_traits);
  }

  public override void removeObject(Culture pObject)
  {
    ++World.world.game_stats.data.culturesForgotten;
    ++World.world.map_stats.culturesForgotten;
    base.removeObject(pObject);
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Culture culture = pActor.culture;
      if (culture != null && culture.isDirtyUnits())
        culture.listUnit(pActor);
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
      if (kingdom.hasCulture())
        kingdom.culture.listKingdom(kingdom);
    }
  }

  private void clearAllKingdomLists()
  {
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) this)
      culture.clearListKingdoms();
  }

  public void beginChecksCities()
  {
    if (this._dirty_cities)
      this.updateDirtyCities();
    this._dirty_cities = false;
  }

  private void updateDirtyCities()
  {
    this.clearAllCitiesListst();
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city.hasCulture())
        city.culture.listCity(city);
    }
  }

  private void clearAllCitiesListst()
  {
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) this)
      culture.clearListCities();
  }

  public void setDirtyCities() => this._dirty_cities = true;

  public void setDirtyKingdoms() => this._dirty_kingdoms = true;

  public override bool isLocked()
  {
    return this.isUnitsDirty() || this._dirty_cities || this._dirty_kingdoms;
  }

  public Culture getMainCulture(List<Actor> pUnitList)
  {
    for (int index = 0; index < pUnitList.Count; ++index)
    {
      Actor pUnit = pUnitList[index];
      if (pUnit.hasCulture())
        this.countMetaObject(pUnit.culture);
    }
    return this.getMostUsedMetaObject();
  }

  public override void clear() => base.clear();
}
