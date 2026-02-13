// Decompiled with JetBrains decompiler
// Type: Language
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Language : MetaObjectWithTraits<LanguageData, LanguageTrait>
{
  public readonly List<City> cities = new List<City>();
  public readonly List<Kingdom> kingdoms = new List<Kingdom>();
  public readonly BooksHandler books = new BooksHandler();

  protected override MetaType meta_type => MetaType.Language;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.languages;

  public void newLanguage(Actor pActor, bool pAddDefaultTraits)
  {
    this.generateName(pActor);
    this.data.creator_name = pActor.getName();
    this.data.creator_id = pActor.getID();
    this.data.creator_species_id = pActor.asset.id;
    this.data.creator_subspecies_name = pActor.subspecies.name;
    this.data.creator_subspecies_id = pActor.subspecies.getID();
    this.data.creator_kingdom_id = pActor.kingdom.getID();
    this.data.creator_kingdom_name = pActor.kingdom.data.name;
    LanguageData data1 = this.data;
    Clan clan = pActor.clan;
    long num1 = clan != null ? clan.getID() : -1L;
    data1.creator_clan_id = num1;
    this.data.creator_clan_name = pActor.clan?.data.name;
    LanguageData data2 = this.data;
    City city = pActor.city;
    long num2 = city != null ? city.getID() : -1L;
    data2.creator_city_id = num2;
    this.data.creator_city_name = pActor.city?.name;
    this.generateNewMetaObject(pAddDefaultTraits);
    this.books.setMeta(pLanguage: this);
  }

  public void countNewSpeaker()
  {
    this.increaseNewSpeakers();
    this.addRenown(1);
  }

  public void countConversion()
  {
    this.increaseConvertedSpeakers();
    this.addRenown(2);
  }

  public override void save()
  {
    base.save();
    this.data.saved_traits = this.getTraitsAsStrings();
  }

  protected override List<string> default_traits => this.getActorAsset().default_language_traits;

  protected override AssetLibrary<LanguageTrait> trait_library
  {
    get => (AssetLibrary<LanguageTrait>) AssetManager.language_traits;
  }

  protected override List<string> saved_traits => this.data.saved_traits;

  protected override string species_id => this.data.creator_species_id;

  protected sealed override void setDefaultValues() => base.setDefaultValues();

  public override void increaseBirths()
  {
    NotImplementedException implementedException = new NotImplementedException(this.GetType().Name);
  }

  public override void generateBanner()
  {
    this.data.banner_icon_id = AssetManager.language_banners_library.getNewIndexIcon();
    this.data.banner_background_id = AssetManager.language_banners_library.getNewIndexBackground();
  }

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.languages_colors_library;
  }

  public void listCity(City pCity) => this.cities.Add(pCity);

  public void listKingdom(Kingdom pKingdom) => this.kingdoms.Add(pKingdom);

  public void clearListCities() => this.cities.Clear();

  public void clearListKingdoms() => this.kingdoms.Clear();

  private void generateName(Actor pActor)
  {
    this.setName(pActor.generateName(MetaType.Language, this.getID()));
    LanguageData data = this.data;
    Culture culture = pActor.culture;
    long num = culture != null ? culture.getID() : -1L;
    data.name_culture_id = num;
  }

  public Sprite getBackgroundSprite()
  {
    return AssetManager.language_banners_library.getSpriteBackground(this.data.banner_background_id);
  }

  public Sprite getIconSprite()
  {
    return AssetManager.language_banners_library.getSpriteIcon(this.data.banner_icon_id);
  }

  public override void loadData(LanguageData pData)
  {
    base.loadData(pData);
    this.books.setMeta(pLanguage: this);
  }

  public int countWrittenBooks() => this.data.books_written;

  public int countCities() => this.cities.Count;

  public int countKingdoms() => this.kingdoms.Count;

  public int getSpeakersNew() => this.data.speakers_new;

  public int getSpeakersLost() => this.data.speakers_lost;

  public int getSpeakersConverted() => this.data.speakers_converted;

  public void increaseConvertedSpeakers() => ++this.data.speakers_converted;

  public void increaseNewSpeakers() => ++this.data.speakers_new;

  public void increaseSpeakersLost() => ++this.data.speakers_lost;

  public override bool isReadyForRemoval() => !this.books.hasBooks() && base.isReadyForRemoval();

  public override void convertSameSpeciesAroundUnit(Actor pActorMain, bool pOverrideExisting = false)
  {
    foreach (Actor actor in this.getUnitFromChunkForConversion(pActorMain))
    {
      if (pOverrideExisting || !actor.hasLanguage())
        actor.joinLanguage(this);
    }
  }

  public override void forceConvertSameSpeciesAroundUnit(Actor pActorMain)
  {
    this.convertSameSpeciesAroundUnit(pActorMain, true);
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "language");
    this.books.clear();
    this.cities.Clear();
    this.kingdoms.Clear();
    base.Dispose();
  }

  public override bool hasCities() => this.cities.Count > 0;

  public override IEnumerable<City> getCities() => (IEnumerable<City>) this.cities;

  public override bool hasKingdoms() => this.kingdoms.Count > 0;

  public override IEnumerable<Kingdom> getKingdoms() => (IEnumerable<Kingdom>) this.kingdoms;
}
