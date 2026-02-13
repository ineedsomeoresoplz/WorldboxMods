// Decompiled with JetBrains decompiler
// Type: Culture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Culture : MetaObjectWithTraits<CultureData, CultureTrait>
{
  public const int MAX_LEVEL = 5;
  public readonly List<City> cities = new List<City>();
  public readonly List<Kingdom> kingdoms = new List<Kingdom>();
  public readonly BooksHandler books = new BooksHandler();
  private readonly List<string> _preferred_weapons_craft_subtypes = new List<string>();
  private readonly List<EquipmentAsset> _preferred_weapons_craft_assets = new List<EquipmentAsset>();
  private NameSetAsset _name_set_asset;
  private readonly List<CultureTrait> _traits_town_plan_zones = new List<CultureTrait>();
  private readonly Dictionary<MetaType, OnomasticsData> _onomastics_data = new Dictionary<MetaType, OnomasticsData>();

  protected override MetaType meta_type => MetaType.Culture;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.cultures;

  public void createCulture(Actor pActor, bool pAddDefaultTraits)
  {
    this.setName(pActor.generateName(MetaType.Culture, this.getID()));
    CultureData data1 = this.data;
    Culture culture = pActor.culture;
    long num1 = culture != null ? culture.getID() : -1L;
    data1.name_culture_id = num1;
    this.data.original_actor_asset = pActor.asset.id;
    this.data.creator_city_name = pActor.city?.name;
    CultureData data2 = this.data;
    City city = pActor.city;
    long num2 = city != null ? city.getID() : -1L;
    data2.creator_city_id = num2;
    this.data.creator_name = pActor.getName();
    this.data.creator_id = pActor.getID();
    this.data.creator_species_id = pActor.asset.id;
    this.data.creator_subspecies_name = pActor.subspecies.name;
    this.data.creator_subspecies_id = pActor.subspecies.getID();
    this.data.creator_clan_name = pActor.clan?.name;
    CultureData data3 = this.data;
    Clan clan = pActor.clan;
    long num3 = clan != null ? clan.getID() : -1L;
    data3.creator_clan_id = num3;
    CultureData data4 = this.data;
    Kingdom kingdom = pActor.kingdom;
    long num4 = kingdom != null ? kingdom.getID() : -1L;
    data4.creator_kingdom_id = num4;
    this.data.creator_kingdom_name = pActor.kingdom?.name;
    this.data.name_template_set = pActor.asset.name_template_sets.GetRandom<string>();
    this.books.setMeta(this);
    this.generateNewMetaObject(pAddDefaultTraits);
    this.setDirty();
  }

  public void cloneAndEvolveOnomastics(Culture pFrom)
  {
    CultureData data = pFrom.data;
    this.data.parent_culture_id = pFrom.id;
    this.data.name_template_set = data.name_template_set;
    pFrom.loadAllOnomasticsData();
    this._onomastics_data.Clear();
    foreach (KeyValuePair<MetaType, OnomasticsData> keyValuePair in pFrom._onomastics_data)
    {
      OnomasticsData pData = new OnomasticsData();
      pData.loadFromShortTemplate(keyValuePair.Value.getShortTemplate());
      OnomasticsEvolver.scramble(pData);
      this._onomastics_data.Add(keyValuePair.Key, pData);
    }
  }

  protected override void recalcBaseStats()
  {
    base.recalcBaseStats();
    this.recalcPreferredWeapons();
    this.recalcTownPlanTraits();
  }

  private void recalcTownPlanTraits()
  {
    this._traits_town_plan_zones.Clear();
    foreach (CultureTrait trait in (IEnumerable<CultureTrait>) this.getTraits())
    {
      if (trait.town_layout_plan)
        this._traits_town_plan_zones.Add(trait);
    }
  }

  public void countConversion() => this.addRenown(1);

  protected override AssetLibrary<CultureTrait> trait_library
  {
    get => (AssetLibrary<CultureTrait>) AssetManager.culture_traits;
  }

  protected override List<string> default_traits => this.getActorAsset().default_culture_traits;

  protected override List<string> saved_traits => this.data.saved_traits;

  protected override string species_id => this.data.original_actor_asset;

  protected sealed override void setDefaultValues() => base.setDefaultValues();

  public override void increaseBirths() => throw new NotImplementedException(this.GetType().Name);

  public int countCities() => this.cities.Count;

  public int countKingdoms() => this.kingdoms.Count;

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.culture_colors_library;
  }

  public bool canUseRoads() => this.hasTrait("roads");

  public void testDebugNewBook()
  {
    if (this.units.Count == 0)
      return;
    Actor random = this.units.GetRandom<Actor>();
    if (random.getCity() == null || !random.city.hasBookSlots())
      return;
    World.world.books.generateNewBook(random);
  }

  private void recalcPreferredWeapons()
  {
    this._preferred_weapons_craft_subtypes.Clear();
    this._preferred_weapons_craft_assets.Clear();
    foreach (CultureTrait trait in (IEnumerable<CultureTrait>) this.getTraits())
    {
      if (trait.is_weapon_trait)
      {
        if (trait.related_weapon_subtype_ids != null)
        {
          foreach (string relatedWeaponSubtypeId in trait.related_weapon_subtype_ids)
            this._preferred_weapons_craft_subtypes.Add(relatedWeaponSubtypeId);
        }
        if (trait.related_weapons_ids != null)
        {
          foreach (string relatedWeaponsId in trait.related_weapons_ids)
          {
            EquipmentAsset equipmentAsset = AssetManager.items.get(relatedWeaponsId);
            if (equipmentAsset != null)
              this._preferred_weapons_craft_assets.Add(equipmentAsset);
          }
        }
      }
    }
  }

  public bool hasPreferredWeaponsToCraft() => this._preferred_weapons_craft_assets.Count > 0;

  public List<EquipmentAsset> getPreferredWeaponAssets() => this._preferred_weapons_craft_assets;

  public string getPreferredWeaponSubtypeIDs()
  {
    return this._preferred_weapons_craft_subtypes.Count == 0 ? (string) null : this._preferred_weapons_craft_subtypes.GetRandom<string>();
  }

  public float chanceToGiveTraits() => 0.5f;

  public void clearListCities() => this.cities.Clear();

  public void clearListKingdoms() => this.kingdoms.Clear();

  public void listCity(City pCity) => this.cities.Add(pCity);

  public void listKingdom(Kingdom pKingdom) => this.kingdoms.Add(pKingdom);

  public override void generateBanner()
  {
    this.data.banner_decor_id = AssetManager.culture_banners_library.getNewIndexBackground();
    this.data.banner_element_id = AssetManager.culture_banners_library.getNewIndexIcon();
  }

  public override void save()
  {
    base.save();
    this.data.saved_traits = this.getTraitsAsStrings();
    if (this._onomastics_data.Count > 0)
    {
      this.data.onomastics = new Dictionary<MetaType, string>(this._onomastics_data.Count);
      foreach (MetaType key in this._onomastics_data.Keys)
      {
        OnomasticsData onomasticsData = this._onomastics_data[key];
        this.data.onomastics[key] = onomasticsData.getShortTemplate();
      }
    }
    else
    {
      this.data.onomastics?.Clear();
      this.data.onomastics = (Dictionary<MetaType, string>) null;
    }
  }

  public override void loadData(CultureData pData)
  {
    base.loadData(pData);
    if (!string.IsNullOrEmpty(this.data.name_template_set) && !AssetManager.name_sets.has(this.data.name_template_set))
    {
      this.data.name_template_set = (string) null;
      this._name_set_asset = (NameSetAsset) null;
    }
    this.books.setDirty();
    this.books.setMeta(this);
    this._onomastics_data.Clear();
    if (pData.onomastics == null)
      return;
    foreach (KeyValuePair<MetaType, string> onomastic in pData.onomastics)
    {
      OnomasticsData onomasticsData = new OnomasticsData();
      onomasticsData.loadFromShortTemplate(onomastic.Value);
      this._onomastics_data.Add(onomastic.Key, onomasticsData);
    }
  }

  public override void updateDirty()
  {
    foreach (City city in this.cities)
      city.setStatusDirty();
  }

  public void debug(DebugTool pTool)
  {
    pTool.setText("id:", (object) this.id);
    pTool.setText("name:", (object) this.name);
    pTool.setText("followers:", (object) this.countUnits());
    pTool.setText("cities:", (object) this.countCities());
  }

  internal void updateTitleCenter()
  {
  }

  public Sprite getElementSprite()
  {
    return AssetManager.culture_banners_library.getSpriteIcon(this.data.banner_element_id);
  }

  public Sprite getDecorSprite()
  {
    return AssetManager.culture_banners_library.getSpriteBackground(this.data.banner_decor_id);
  }

  public List<long> getBooks() => this.books.getList();

  public override bool isReadyForRemoval() => !this.books.hasBooks() && base.isReadyForRemoval();

  public override void convertSameSpeciesAroundUnit(Actor pActorMain, bool pOverrideExisting = false)
  {
    foreach (Actor actor in this.getUnitFromChunkForConversion(pActorMain))
    {
      if (pOverrideExisting || !actor.hasCulture())
        actor.setCulture(this);
    }
  }

  public override void forceConvertSameSpeciesAroundUnit(Actor pActorMain)
  {
    this.convertSameSpeciesAroundUnit(pActorMain, true);
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "culture");
    this.books.clear();
    this._preferred_weapons_craft_subtypes.Clear();
    this._preferred_weapons_craft_assets.Clear();
    this.cities.Clear();
    this.kingdoms.Clear();
    this._traits_town_plan_zones.Clear();
    this._name_set_asset = (NameSetAsset) null;
    this._onomastics_data.Clear();
    base.Dispose();
  }

  public string getNameTemplate(MetaType pType)
  {
    if (this._name_set_asset == null)
    {
      if (string.IsNullOrEmpty(this.data.name_template_set))
        this.data.name_template_set = this.getActorAsset().name_template_sets.GetRandom<string>();
      if (string.IsNullOrEmpty(this.data.name_template_set))
      {
        this._name_set_asset = (NameSetAsset) null;
        return AssetManager.name_generator.get(this.getActorAsset().getNameTemplate(pType)).id;
      }
    }
    this._name_set_asset = AssetManager.name_sets.get(this.data.name_template_set);
    return this._name_set_asset.get(pType);
  }

  public void loadAllOnomasticsData()
  {
    foreach (MetaType type in NameSetAsset.getTypes())
      this.getOnomasticData(type);
  }

  public OnomasticsData getOnomasticData(MetaType pType, bool pReset = false)
  {
    OnomasticsData onomasticData;
    if (!this._onomastics_data.TryGetValue(pType, out onomasticData) | pReset)
    {
      if (onomasticData == null)
        onomasticData = new OnomasticsData();
      else
        onomasticData.clearTemplateData();
      string nameTemplate1 = this.getNameTemplate(pType);
      if (!string.IsNullOrEmpty(nameTemplate1))
      {
        NameGeneratorAsset nameGeneratorAsset = AssetManager.name_generator.get(nameTemplate1);
        if (nameGeneratorAsset.hasOnomastics())
        {
          OnomasticsData originalData = OnomasticsCache.getOriginalData(nameGeneratorAsset.onomastics_templates.GetRandom<string>());
          onomasticData.cloneFrom(originalData);
        }
        else
          Debug.Log((object) $"no onomastics data found for {nameGeneratorAsset.id} for {pType}");
      }
      if (onomasticData.isEmpty())
      {
        Debug.Log((object) $"name set asset {this._name_set_asset.id} doesn't have {pType.ToString()}");
        Debug.Log((object) "loading from actor");
        string nameTemplate2 = this.getActorAsset().getNameTemplate(pType);
        NameGeneratorAsset nameGeneratorAsset = AssetManager.name_generator.get(nameTemplate2);
        if (nameGeneratorAsset.hasOnomastics())
        {
          OnomasticsData originalData = OnomasticsCache.getOriginalData(nameGeneratorAsset.onomastics_templates.GetRandom<string>());
          onomasticData.cloneFrom(originalData);
        }
        else
          Debug.Log((object) $"no onomastics data found for {nameGeneratorAsset.id} for {pType}");
      }
      if (onomasticData.isEmpty())
      {
        onomasticData.setDebugTest();
        Debug.Log((object) $"no onomastics data found for {this._name_set_asset.id} for {pType}, defaulting");
      }
      this._onomastics_data[pType] = onomasticData;
    }
    return onomasticData;
  }

  public bool planAllowsToPlaceBuildingInZone(TileZone pZone, TileZone pCenterZone)
  {
    foreach (CultureTrait traitsTownPlanZone in this._traits_town_plan_zones)
    {
      if (!traitsTownPlanZone.passable_zone_checker(pZone, pCenterZone))
        return false;
    }
    return true;
  }

  public bool hasSpecialTownPlans() => this._traits_town_plan_zones.Count > 0;

  public bool hasTrueRoots() => this.hasTrait("true_roots");

  public bool isPossibleToConvertToOtherMeta() => !this.hasTrueRoots();

  public override bool hasCities() => this.cities.Count > 0;

  public override IEnumerable<City> getCities() => (IEnumerable<City>) this.cities;

  public override bool hasKingdoms() => this.kingdoms.Count > 0;

  public override IEnumerable<Kingdom> getKingdoms() => (IEnumerable<Kingdom>) this.kingdoms;
}
