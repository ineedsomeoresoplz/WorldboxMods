// Decompiled with JetBrains decompiler
// Type: BuildingAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class BuildingAsset : Asset
{
  [NonSerialized]
  public bool sprites_are_initiated;
  public Vector3 scale_base = new Vector3(0.25f, 0.25f, 0.25f);
  [DefaultValue("")]
  public string kingdom = string.Empty;
  [DefaultValue("")]
  public string civ_kingdom = string.Empty;
  public BuildingFundament fundament;
  [DefaultValue("building")]
  public string material = "building";
  [DefaultValue("buildings")]
  public string atlas_id = nameof (buildings);
  [DefaultValue("buildings")]
  public string atlas_id_fallback_when_not_wobbly = nameof (buildings);
  [NonSerialized]
  public DynamicSpritesAsset atlas_asset;
  public bool prevent_freeze;
  public float bonus_z;
  public bool removed_by_sponge;
  [DefaultValue("")]
  public string sprite_path = string.Empty;
  [DefaultValue("buildings/")]
  public string main_path = "buildings/";
  public bool grow_creep;
  [DefaultValue(CreepWorkerMovementType.RandomNeighbourAll)]
  public CreepWorkerMovementType grow_creep_movement_type;
  [DefaultValue("")]
  public string grow_creep_type = string.Empty;
  public bool draw_light_area;
  public float draw_light_area_offset_x;
  public float draw_light_area_offset_y;
  [DefaultValue(0.5f)]
  public float draw_light_size = 0.5f;
  public int grow_creep_steps_max;
  public float grow_creep_step_interval;
  [DefaultValue(1)]
  public int grow_creep_workers = 1;
  public bool grow_creep_direction_random_position;
  public bool grow_creep_random_new_direction;
  public bool grow_creep_flash;
  public int construction_progress_needed;
  public bool grow_creep_redraw_tile;
  [DefaultValue(7)]
  public int grow_creep_steps_before_new_direction = 7;
  [DefaultValue(true)]
  public bool has_ruins_graphics = true;
  public bool has_special_animation_state;
  [DefaultValue(6f)]
  public float animation_speed = 6f;
  [DefaultValue(BuildingType.Building_None)]
  public BuildingType building_type;
  public bool sparkle_effect;
  public List<ResourceContainer> resources_given;
  public bool can_be_grown;
  [DefaultValue(0.5f)]
  public float vegetation_random_chance = 0.5f;
  public bool has_kingdom_color;
  public bool city_building;
  public bool can_be_abandoned;
  public bool mini_civ_auto_load;
  public bool destroy_on_liquid;
  public bool can_be_upgraded;
  [DefaultValue("")]
  public string upgrade_to = string.Empty;
  [DefaultValue("")]
  public string upgraded_from = string.Empty;
  public int upgrade_level;
  [DefaultValue("")]
  public string type = string.Empty;
  public bool gatherable;
  public bool wheat;
  public bool produce_biome_food;
  public float growth_time;
  public int loot_generation;
  public string[] boat_types;
  public string boat_type_fishing;
  public string boat_type_trading;
  public string boat_type_transport;
  public bool waypoint;
  public int priority;
  public BuildingStepAction step_action;
  [NonSerialized]
  public bool has_step_action;
  [DefaultValue(true)]
  public bool shadow = true;
  public Vector2 shadow_bound = new Vector2(0.5f, 0.8f);
  [DefaultValue(0.2f)]
  public float shadow_distortion = 0.2f;
  public bool auto_remove_ruin;
  public bool ice_tower;
  public bool spawn_units;
  public bool beehive;
  [DefaultValue("-")]
  public string spawn_units_asset = "-";
  public bool tower;
  [DefaultValue("")]
  public string tower_projectile = string.Empty;
  public float tower_projectile_offset;
  [DefaultValue(false)]
  public bool tower_attack_buildings;
  [DefaultValue(3f)]
  public float tower_projectile_reload = 3f;
  [DefaultValue(1)]
  public int tower_projectile_amount = 1;
  public bool ignore_other_buildings_for_upgrade;
  public bool random_flip;
  public ConstructionCost cost;
  public BaseStats base_stats;
  public bool ignored_by_cities;
  public bool remove_buildings_when_dropped;
  public bool remove_civ_buildings;
  public bool ignore_same_building_id;
  public bool build_road_to;
  public bool can_be_damaged_by_tornado;
  public bool can_be_placed_on_liquid;
  public bool can_be_placed_on_blocks;
  public bool damaged_by_rain;
  public bool only_build_tiles;
  public bool build_place_borders;
  public bool build_place_single;
  public bool build_place_center;
  public bool needs_farms_ground;
  public bool build_place_batch;
  public bool build_prefer_replace_house;
  public bool check_for_close_building;
  public bool ignore_buildings;
  public bool can_be_demolished;
  public bool burnable;
  public bool affected_by_lava;
  public bool affected_by_acid;
  public bool can_units_live_here;
  public int housing_slots;
  public int housing_happiness;
  public int max_houses;
  public bool storage;
  public bool storage_only_food;
  [DefaultValue(true)]
  public bool can_be_living_house = true;
  [DefaultValue(true)]
  public bool can_be_living_plant = true;
  [DefaultValue(true)]
  public bool remove_ruins = true;
  [DefaultValue(true)]
  public bool has_ruin_state = true;
  public bool has_resources_to_collect;
  public bool has_resources_grown_to_collect;
  public bool has_resources_grown_to_collect_on_spawn;
  public bool can_be_chopped_down;
  public int book_slots;
  public BuildingOverrideMainSprites get_override_sprites_main;
  public BuildingOverrideMainSprite get_override_sprite_main;
  public bool is_vegetation;
  public bool is_stockpile;
  public Vector2 stockpile_top_left_offset;
  public Vector2 stockpile_center_offset;
  public int limit_per_zone;
  public bool become_alive_when_chopped;
  public int limit_in_radius;
  public int limit_global;
  public bool docks;
  [NonSerialized]
  public bool has_biome_tags;
  public HashSet<BiomeTag> biome_tags_growth;
  [NonSerialized]
  public bool has_biome_tags_spread;
  public HashSet<BiomeTag> biome_tags_spread;
  public bool spread_biome;
  public string spread_biome_id;
  [DefaultValue("")]
  public string group = string.Empty;
  public bool affected_by_drought;
  public bool affected_by_cold_temperature;
  public bool smoke;
  [DefaultValue(0.5f)]
  public float smoke_interval = 0.5f;
  public Vector2Int smoke_offset;
  public bool spawn_drops;
  [DefaultValue("")]
  public string spawn_drop_id = "";
  public float spawn_drop_interval;
  public float spawn_drop_start_height;
  public float spawn_drop_min_height;
  public float spawn_drop_max_height;
  public float spawn_drop_min_radius;
  public float spawn_drop_max_radius;
  public string transform_tiles_to_tile_type;
  public string transform_tiles_to_top_tiles;
  public string sound_spawn;
  public string sound_idle;
  public string sound_hit;
  public string sound_built;
  public string sound_destroyed;
  public int nutrition_restore;
  public bool spawn_rats;
  public bool flora;
  public FloraSize flora_size;
  public bool spread;
  public float spread_chance;
  public float spread_steps;
  public FloraType flora_type;
  public string[] spread_ids;
  public bool has_sprites_spawn;
  public bool has_sprites_main;
  public bool has_sprites_main_disabled;
  public bool has_sprites_ruin;
  public bool has_sprites_special;
  public bool has_sprite_construction;
  public bool check_for_adaptation_tags;
  public GetColorForMapIcon get_map_icon_color;
  public bool has_get_map_icon_color;
  [NonSerialized]
  public BuildingSprites building_sprites;
  [NonSerialized]
  public HashSet<Building> buildings = new HashSet<Building>();

  [JsonIgnore]
  public bool has_sound_spawn => this.sound_spawn != null;

  [JsonIgnore]
  public bool has_sound_idle => this.sound_idle != null;

  [JsonIgnore]
  public bool has_sound_hit => this.sound_hit != null;

  [JsonIgnore]
  public bool has_sound_built => this.sound_built != null;

  [JsonIgnore]
  public bool has_sound_destroyed => this.sound_destroyed != null;

  public bool setSpread(FloraType pType, int pSpreadSteps = 1, float pSpreadChance = 1f)
  {
    if (pType == FloraType.None)
    {
      this.spread = false;
      return false;
    }
    this.spread = true;
    this.flora_type = pType;
    this.spread_steps = (float) pSpreadSteps;
    this.spread_chance = pSpreadChance;
    return true;
  }

  public void setAtlasID(string pAtlasID, string pFallbackID = null)
  {
    if (pFallbackID == null)
      pFallbackID = pAtlasID;
    this.atlas_id = pAtlasID;
    this.atlas_id_fallback_when_not_wobbly = pFallbackID;
  }

  public void setShadow(float pBoundX, float pBoundY, float pDistortion)
  {
    this.shadow = true;
    this.shadow_bound.x = pBoundX;
    this.shadow_bound.y = pBoundY;
    this.shadow_distortion = pDistortion;
  }

  public bool isOverlaysBiomeTags(TileTypeBase pTileType)
  {
    return !this.has_biome_tags || pTileType.overlapsBiomeTags(this.biome_tags_growth);
  }

  public bool isOverlaysBiomeSpreadTags(TileTypeBase pTileType)
  {
    return this.has_biome_tags_spread && pTileType.overlapsBiomeTags(this.biome_tags_spread);
  }

  public void checkLimits(Building pBuildingToIgnore = null)
  {
    if (this.limit_global == 0 || this.buildings.Count < this.limit_global)
      return;
    int num = this.buildings.Count - this.limit_global;
    foreach (Building building in this.buildings)
    {
      if (num == 0)
        break;
      if ((pBuildingToIgnore == null || pBuildingToIgnore != building) && building.isAlive())
      {
        building.startDestroyBuilding();
        --num;
      }
    }
  }

  public bool canBeOccupied() => this.hasHousingSlots() || this.docks || this.spawn_units;

  public void addResource(string pID, int pAmount, bool pNewList = false)
  {
    if (this.resources_given == null | pNewList)
      this.resources_given = new List<ResourceContainer>();
    this.resources_given.Add(new ResourceContainer(pID, pAmount));
  }

  public bool hasResourceGiven(string pID)
  {
    if (this.resources_given == null)
      return false;
    foreach (ResourceContainer resourceContainer in this.resources_given)
    {
      if (resourceContainer.id == pID)
        return true;
    }
    return false;
  }

  public ActorAsset getRandomBoatAssetToBuild(City pCity)
  {
    string boatAssetIdFromType = this.getBoatAssetIDFromType(this.boat_types.GetRandom<string>(), pCity);
    return string.IsNullOrEmpty(boatAssetIdFromType) ? (ActorAsset) null : AssetManager.actor_library.get(boatAssetIdFromType);
  }

  public void setHousingSlots(int pValue)
  {
    this.can_units_live_here = true;
    this.housing_slots = pValue;
  }

  public bool hasHousingSlots() => this.housing_slots > 0;

  private string getBoatAssetIDFromType(string pSpeciesBoat, City pCity)
  {
    if (pCity == null)
      return "boat_fishing";
    ArchitectureAsset architectureAsset = pCity.getActorAsset().architecture_asset;
    switch (pSpeciesBoat)
    {
      case "boat_type_fishing":
        return architectureAsset.actor_asset_id_boat_fishing;
      case "boat_type_trading":
        return architectureAsset.actor_asset_id_trading;
      case "boat_type_transport":
        return architectureAsset.actor_asset_id_transport;
      default:
        return architectureAsset.actor_asset_id_boat_fishing;
    }
  }

  public void checkSpritesAreLoaded()
  {
    if (this.sprites_are_initiated)
      return;
    this.sprites_are_initiated = true;
    this.loadBuildingSprites();
  }

  public void loadBuildingSprites()
  {
    Sprite[] collection = this.loadBuildingSpriteList();
    PreloadHelpers.total_building_sprites += collection.Length;
    PreloadHelpers.all_preloaded_sprites_buildings.AddRange((IEnumerable<Sprite>) collection);
    BuildingSprites buildingSprites = new BuildingSprites();
    this.building_sprites = buildingSprites;
    ++PreloadHelpers.total_building_sprite_containers;
    for (int index1 = 0; index1 < collection.Length; ++index1)
    {
      Sprite sprite = collection[index1];
      string[] strArray = ((Object) sprite).name.Split('_', StringSplitOptions.None);
      string str = strArray[0];
      int index2 = int.Parse(strArray[1]);
      while (buildingSprites.animation_data.Count < index2 + 1)
        buildingSprites.animation_data.Add((BuildingAnimationData) null);
      if (buildingSprites.animation_data[index2] == null)
        buildingSprites.animation_data[index2] = new BuildingAnimationData();
      BuildingAnimationData buildingAnimationData1 = this.building_sprites.animation_data[index2];
      bool pIsContructionSprite = false;
      switch (str)
      {
        case "construction":
          this.building_sprites.construction = sprite;
          pIsContructionSprite = true;
          break;
        case "disabled":
          BuildingAnimationData buildingAnimationData2 = buildingAnimationData1;
          if (buildingAnimationData2.list_main_disabled == null)
            buildingAnimationData2.list_main_disabled = new ListPool<Sprite>();
          buildingAnimationData1.list_main_disabled.Add(sprite);
          if (buildingAnimationData1.list_main_disabled.Count > 1)
          {
            buildingAnimationData1.animated = true;
            break;
          }
          break;
        case "main":
          BuildingAnimationData buildingAnimationData3 = buildingAnimationData1;
          if (buildingAnimationData3.list_main == null)
            buildingAnimationData3.list_main = new ListPool<Sprite>();
          buildingAnimationData1.list_main.Add(sprite);
          if (buildingAnimationData1.list_main.Count > 1)
          {
            buildingAnimationData1.animated = true;
            break;
          }
          break;
        case "mini":
          this.building_sprites.map_icon = new BuildingMapIcon(sprite);
          break;
        case "ruin":
          BuildingAnimationData buildingAnimationData4 = buildingAnimationData1;
          if (buildingAnimationData4.list_ruins == null)
            buildingAnimationData4.list_ruins = new ListPool<Sprite>();
          buildingAnimationData1.list_ruins.Add(sprite);
          break;
        case "spawn":
          BuildingAnimationData buildingAnimationData5 = buildingAnimationData1;
          if (buildingAnimationData5.list_spawn == null)
            buildingAnimationData5.list_spawn = new ListPool<Sprite>();
          buildingAnimationData1.list_spawn.Add(sprite);
          if (buildingAnimationData1.list_spawn.Count > 1)
          {
            buildingAnimationData1.animated = true;
            break;
          }
          break;
        case "special":
          BuildingAnimationData buildingAnimationData6 = buildingAnimationData1;
          if (buildingAnimationData6.list_special == null)
            buildingAnimationData6.list_special = new ListPool<Sprite>();
          buildingAnimationData1.list_special.Add(sprite);
          break;
      }
      if (this.shadow)
        DynamicSpriteCreator.createBuildingShadow(this, sprite, pIsContructionSprite);
    }
    foreach (BuildingAnimationData buildingAnimationData in this.building_sprites.animation_data)
    {
      ListPool<Sprite> listMain = buildingAnimationData.list_main;
      buildingAnimationData.main = listMain != null ? listMain.ToArray<Sprite>() : (Sprite[]) null;
      ListPool<Sprite> listSpawn = buildingAnimationData.list_spawn;
      buildingAnimationData.spawn = listSpawn != null ? listSpawn.ToArray<Sprite>() : (Sprite[]) null;
      ListPool<Sprite> listMainDisabled = buildingAnimationData.list_main_disabled;
      buildingAnimationData.main_disabled = listMainDisabled != null ? listMainDisabled.ToArray<Sprite>() : (Sprite[]) null;
      ListPool<Sprite> listRuins = buildingAnimationData.list_ruins;
      buildingAnimationData.ruins = listRuins != null ? listRuins.ToArray<Sprite>() : (Sprite[]) null;
      ListPool<Sprite> listSpecial = buildingAnimationData.list_special;
      buildingAnimationData.special = listSpecial != null ? listSpecial.ToArray<Sprite>() : (Sprite[]) null;
      buildingAnimationData.list_main?.Dispose();
      buildingAnimationData.list_spawn?.Dispose();
      buildingAnimationData.list_main_disabled?.Dispose();
      buildingAnimationData.list_ruins?.Dispose();
      buildingAnimationData.list_special?.Dispose();
      buildingAnimationData.list_main = (ListPool<Sprite>) null;
      buildingAnimationData.list_spawn = (ListPool<Sprite>) null;
      buildingAnimationData.list_main_disabled = (ListPool<Sprite>) null;
      buildingAnimationData.list_ruins = (ListPool<Sprite>) null;
      buildingAnimationData.list_special = (ListPool<Sprite>) null;
    }
  }

  public Sprite[] loadBuildingSpriteList()
  {
    string pPath = this.sprite_path;
    if (string.IsNullOrEmpty(pPath))
      pPath = this.main_path + this.id;
    return SpriteTextureLoader.getSpriteList(pPath);
  }
}
