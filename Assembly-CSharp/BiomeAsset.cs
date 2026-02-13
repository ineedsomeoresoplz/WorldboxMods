// Decompiled with JetBrains decompiler
// Type: BiomeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable
[Serializable]
public class BiomeAsset : Asset, IDescriptionAsset, ILocalizedAsset, IMultiLocalesAsset
{
  public GrowTypeSelector grow_type_selector_trees;
  public GrowTypeSelector grow_type_selector_plants;
  public GrowTypeSelector grow_type_selector_bushes;
  public GrowTypeSelector grow_type_selector_minerals;
  public List<string> pot_sapient_units_spawn;
  public List<string> pot_units_spawn;
  public List<string> pot_trees_spawn;
  public List<string> pot_plants_spawn;
  public List<string> pot_bushes_spawn;
  public List<string> pot_minerals_spawn;
  public bool grow_vegetation_auto;
  public bool grow_minerals_auto;
  public bool pot_spawn_units_auto;
  public bool cold_biome;
  public bool dark_biome;
  public bool spread_ignore_burned_stages;
  public bool spread_biome;
  public bool spread_by_drops_water;
  public bool spread_by_drops_fire;
  public bool spread_by_drops_curse;
  public bool spread_by_drops_blessing;
  public bool spread_by_drops_powerup;
  public bool spread_by_drops_acid;
  public bool spread_by_drops_coffee;
  public bool special_biome;
  [DefaultValue(6)]
  public int grow_strength = 6;
  public string spread_only_in_era;
  public string tile_low;
  public string tile_high;
  [NonSerialized]
  private TopTileType _cached_tile_high;
  [NonSerialized]
  private TopTileType _cached_tile_low;
  public int generator_pot_amount;
  public int generator_max_size;
  public string localized_key;
  public int loot_generation;
  public string[] subspecies_name_suffix;
  public List<string> spawn_trait_actor;
  public List<string> spawn_trait_subspecies;
  public List<string> spawn_trait_subspecies_always;
  public List<string> spawn_trait_culture;
  public List<string> spawn_trait_clan;
  public List<string> spawn_trait_language;
  public List<string> spawn_trait_religion;
  public List<string> evolution_trait_subspecies;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public TopTileType getTile(WorldTile pTile)
  {
    switch (pTile.main_type.rank_type)
    {
      case TileRank.Low:
        return this.getTileLow();
      case TileRank.High:
        return this.getTileHigh();
      default:
        return (TopTileType) null;
    }
  }

  public int getTileCount()
  {
    TopTileType tileHigh = this.getTileHigh();
    int num = 0 + (tileHigh != null ? tileHigh.getCurrentTiles().Count : 0);
    TopTileType tileLow = this.getTileLow();
    int count = tileLow != null ? tileLow.getCurrentTiles().Count : 0;
    return num + count;
  }

  [CanBeNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public TopTileType getTileHigh()
  {
    if (this._cached_tile_high == null)
      this._cached_tile_high = AssetManager.top_tiles.get(this.tile_high);
    return this._cached_tile_high;
  }

  [CanBeNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public TopTileType getTileLow()
  {
    if (this._cached_tile_low == null)
      this._cached_tile_low = AssetManager.top_tiles.get(this.tile_low);
    return this._cached_tile_low;
  }

  public void addTree(string pID, int pRateAmount = 1)
  {
    this.grow_vegetation_auto = true;
    if (this.pot_trees_spawn == null)
      this.pot_trees_spawn = new List<string>();
    for (int index = 0; index < pRateAmount; ++index)
      this.pot_trees_spawn.Add(pID);
  }

  public void addPlant(string pID, int pRateAmount = 1)
  {
    this.grow_vegetation_auto = true;
    if (this.pot_plants_spawn == null)
      this.pot_plants_spawn = new List<string>();
    for (int index = 0; index < pRateAmount; ++index)
      this.pot_plants_spawn.Add(pID);
  }

  public void addBush(string pID, int pRateAmount = 1)
  {
    this.grow_vegetation_auto = true;
    if (this.pot_bushes_spawn == null)
      this.pot_bushes_spawn = new List<string>();
    for (int index = 0; index < pRateAmount; ++index)
      this.pot_bushes_spawn.Add(pID);
  }

  public void addMineral(string pID, int pRateAmount = 1)
  {
    this.grow_minerals_auto = true;
    if (this.pot_minerals_spawn == null)
      this.pot_minerals_spawn = new List<string>();
    for (int index = 0; index < pRateAmount; ++index)
      this.pot_minerals_spawn.Add(pID);
  }

  public void addUnit(string pID, int pRateAmount = 1)
  {
    this.pot_spawn_units_auto = true;
    if (this.pot_units_spawn == null)
      this.pot_units_spawn = new List<string>();
    for (int index = 0; index < pRateAmount; ++index)
      this.pot_units_spawn.Add(pID);
  }

  public void addSapientUnit(string pID, int pRateAmount = 1)
  {
    this.pot_spawn_units_auto = true;
    if (this.pot_sapient_units_spawn == null)
      this.pot_sapient_units_spawn = new List<string>();
    for (int index = 0; index < pRateAmount; ++index)
      this.pot_sapient_units_spawn.Add(pID);
  }

  internal void addActorTrait(string pTrait)
  {
    if (this.spawn_trait_actor == null)
      this.spawn_trait_actor = new List<string>();
    if (this.spawn_trait_actor.Contains(pTrait))
      return;
    this.spawn_trait_actor.Add(pTrait);
  }

  internal void addSubspeciesTrait(string pTrait)
  {
    if (this.spawn_trait_subspecies == null)
      this.spawn_trait_subspecies = new List<string>();
    if (this.spawn_trait_subspecies.Contains(pTrait))
      return;
    this.spawn_trait_subspecies.Add(pTrait);
  }

  internal void addSubspeciesTraitAlways(string pTrait)
  {
    if (this.spawn_trait_subspecies_always == null)
      this.spawn_trait_subspecies_always = new List<string>();
    if (this.spawn_trait_subspecies_always.Contains(pTrait))
      return;
    this.spawn_trait_subspecies_always.Add(pTrait);
  }

  internal void addSubspeciesTraitEvolution(string pTrait)
  {
    if (this.evolution_trait_subspecies == null)
      this.evolution_trait_subspecies = new List<string>();
    if (this.evolution_trait_subspecies.Contains(pTrait))
      return;
    this.evolution_trait_subspecies.Add(pTrait);
  }

  internal void addCultureTrait(string pTrait)
  {
    if (this.spawn_trait_culture == null)
      this.spawn_trait_culture = new List<string>();
    if (this.spawn_trait_culture.Contains(pTrait))
      return;
    this.spawn_trait_culture.Add(pTrait);
  }

  internal void addLanguageTrait(string pTrait)
  {
    if (this.spawn_trait_language == null)
      this.spawn_trait_language = new List<string>();
    if (this.spawn_trait_language.Contains(pTrait))
      return;
    this.spawn_trait_language.Add(pTrait);
  }

  internal void addClanTrait(string pTrait)
  {
    if (this.spawn_trait_clan == null)
      this.spawn_trait_clan = new List<string>();
    if (this.spawn_trait_clan.Contains(pTrait))
      return;
    this.spawn_trait_clan.Add(pTrait);
  }

  internal void addReligionTrait(string pTrait)
  {
    if (this.spawn_trait_religion == null)
      this.spawn_trait_religion = new List<string>();
    if (this.spawn_trait_religion.Contains(pTrait))
      return;
    this.spawn_trait_religion.Add(pTrait);
  }

  public string getLocaleID() => this.localized_key.Underscore();

  public string getDescriptionID() => this.getLocaleID() + "_description";

  public IEnumerable<string> getLocaleIDs()
  {
    if (LocalizedTextManager.stringExists(this.getLocaleID() + "_seeds"))
      yield return this.getLocaleID() + "_seeds";
  }
}
