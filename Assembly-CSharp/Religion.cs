// Decompiled with JetBrains decompiler
// Type: Religion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Religion : MetaObjectWithTraits<ReligionData, ReligionTrait>
{
  public readonly List<City> cities = new List<City>();
  public readonly List<Kingdom> kingdoms = new List<Kingdom>();
  public BooksHandler books = new BooksHandler();
  public List<PlotAsset> possible_rites = new List<PlotAsset>();
  private bool _has_bloodline_bond;

  protected override MetaType meta_type => MetaType.Religion;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.religions;

  public bool is_magic_only_clan_members => this._has_bloodline_bond;

  public void newReligion(Actor pActor, WorldTile pTile, bool pAddDefaultTraits)
  {
    this.data.creator_name = pActor.getName();
    this.data.creator_id = pActor.getID();
    this.data.creator_species_id = pActor.asset.id;
    this.data.creator_subspecies_name = pActor.subspecies.name;
    this.data.creator_subspecies_id = pActor.subspecies.getID();
    ReligionData data1 = this.data;
    Clan clan = pActor.clan;
    long num1 = clan != null ? clan.id : -1L;
    data1.creator_clan_id = num1;
    this.data.creator_clan_name = pActor.clan?.name;
    this.data.creator_kingdom_id = pActor.kingdom.getID();
    this.data.creator_kingdom_name = pActor.kingdom.name;
    this.data.creator_city_name = pActor.city?.name;
    ReligionData data2 = this.data;
    City city = pActor.city;
    long num2 = city != null ? city.getID() : -1L;
    data2.creator_city_id = num2;
    this.generateNewMetaObject(pAddDefaultTraits);
    this.generateName(pActor);
    this.books.setMeta(pReligion: this);
  }

  public override bool isReadyForRemoval() => !this.books.hasBooks() && base.isReadyForRemoval();

  public override void loadData(ReligionData pData)
  {
    base.loadData(pData);
    this.books.setDirty();
    this.books.setMeta(pReligion: this);
  }

  protected override void recalcBaseStats()
  {
    base.recalcBaseStats();
    this._has_bloodline_bond = this.hasTrait("bloodline_bond");
    this.possible_rites.Clear();
    foreach (ReligionTrait trait in (IEnumerable<ReligionTrait>) this.getTraits())
    {
      if (trait.hasPlotAsset())
        this.possible_rites.Add(trait.plot_asset);
    }
  }

  public void countConversion() => this.addRenown(1);

  protected sealed override void setDefaultValues() => base.setDefaultValues();

  protected override List<string> default_traits => this.getActorAsset().default_religion_traits;

  protected override AssetLibrary<ReligionTrait> trait_library
  {
    get => (AssetLibrary<ReligionTrait>) AssetManager.religion_traits;
  }

  protected override List<string> saved_traits => this.data.saved_traits;

  protected override string species_id => this.data.creator_species_id;

  public override void increaseBirths() => throw new NotImplementedException(this.GetType().Name);

  public override void save()
  {
    base.save();
    this.data.saved_traits = this.getTraitsAsStrings();
  }

  public override void generateBanner()
  {
    this.data.banner_background_id = AssetManager.religion_banners_library.getNewIndexBackground();
    this.data.banner_icon_id = AssetManager.religion_banners_library.getNewIndexIcon();
  }

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.religion_colors_library;
  }

  public void listCity(City pCity) => this.cities.Add(pCity);

  public void listKingdom(Kingdom pKingdom) => this.kingdoms.Add(pKingdom);

  public void clearListCities() => this.cities.Clear();

  public void clearListKingdoms() => this.kingdoms.Clear();

  private void generateName(Actor pActor)
  {
    this.setName(pActor.generateName(MetaType.Religion, this.getID()));
    ReligionData data = this.data;
    Culture culture = pActor.culture;
    long num = culture != null ? culture.getID() : -1L;
    data.name_culture_id = num;
  }

  public Sprite getBackgroundSprite()
  {
    return AssetManager.religion_banners_library.getSpriteBackground(this.data.banner_background_id);
  }

  public Sprite getIconSprite()
  {
    return AssetManager.religion_banners_library.getSpriteIcon(this.data.banner_icon_id);
  }

  public int countCities() => this.cities.Count;

  public int countKingdoms() => this.kingdoms.Count;

  public override void convertSameSpeciesAroundUnit(Actor pActorMain, bool pOverrideExisting = false)
  {
    foreach (Actor actor in this.getUnitFromChunkForConversion(pActorMain))
    {
      if (pOverrideExisting || !actor.hasReligion())
        actor.setReligion(this);
    }
  }

  public override void forceConvertSameSpeciesAroundUnit(Actor pActorMain)
  {
    this.convertSameSpeciesAroundUnit(pActorMain, true);
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "religion");
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
