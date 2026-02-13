// Decompiled with JetBrains decompiler
// Type: LanguageManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class LanguageManager : MetaSystemManager<Language, LanguageData>
{
  private bool _dirty_kingdoms = true;
  private bool _dirty_cities = true;

  public LanguageManager() => this.type_id = "language";

  public Language newLanguage(Actor pActor, bool pAddDefaultTraits)
  {
    ++World.world.game_stats.data.languagesCreated;
    ++World.world.map_stats.languagesCreated;
    Language language = this.newObject();
    language.newLanguage(pActor, pAddDefaultTraits);
    MetaHelper.addRandomTrait<LanguageTrait>((ITraitsOwner<LanguageTrait>) language, (BaseTraitLibrary<LanguageTrait>) AssetManager.language_traits);
    this.addRandomTraitFromBiomeToLanguage(language, pActor.current_tile);
    return language;
  }

  public void addRandomTraitFromBiomeToLanguage(Language pLanguage, WorldTile pTile)
  {
    pLanguage.addRandomTraitFromBiome<LanguageTrait>(pTile, pTile.Type.biome_asset?.spawn_trait_language, (AssetLibrary<LanguageTrait>) AssetManager.language_traits);
  }

  public Language getMainLanguage(List<Actor> pUnitList)
  {
    for (int index = 0; index < pUnitList.Count; ++index)
    {
      Actor pUnit = pUnitList[index];
      if (pUnit.hasLanguage())
        this.countMetaObject(pUnit.language);
    }
    return this.getMostUsedMetaObject();
  }

  public override void removeObject(Language pObject)
  {
    ++World.world.game_stats.data.languagesForgotten;
    ++World.world.map_stats.languagesForgotten;
    base.removeObject(pObject);
  }

  protected override void updateDirtyUnits()
  {
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Language language = pActor.language;
      if (language != null && language.isDirtyUnits())
        language.listUnit(pActor);
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
    this.clearAllKingdomListst();
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      kingdom.getLanguage()?.listKingdom(kingdom);
  }

  private void clearAllKingdomListst()
  {
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) this)
      language.clearListKingdoms();
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
      if (city.hasLanguage())
        city.language.listCity(city);
    }
  }

  private void clearAllCitiesListst()
  {
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) this)
      language.clearListCities();
  }

  public void setDirtyKingdoms() => this._dirty_kingdoms = true;

  public void setDirtyCities() => this._dirty_cities = true;

  public override bool isLocked()
  {
    return this.isUnitsDirty() || this._dirty_cities || this._dirty_kingdoms;
  }
}
