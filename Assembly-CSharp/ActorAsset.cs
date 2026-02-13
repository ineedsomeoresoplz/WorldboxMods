// Decompiled with JetBrains decompiler
// Type: ActorAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[Serializable]
public class ActorAsset : BaseUnlockableAsset, IDescriptionAsset, ILocalizedAsset, IAnimationFrames
{
  [DefaultValue(true)]
  public bool split_ai_update = true;
  [DefaultValue(true)]
  public bool has_ai_system = true;
  [DefaultValue(1)]
  public int item_making_skill = 1;
  public bool affected_by_dust;
  public string sound_idle;
  public string sound_idle_loop;
  public string sound_spawn;
  public string sound_death;
  public string sound_attack;
  [DefaultValue("event:/SFX/HIT/HitGeneric")]
  public string sound_hit = "event:/SFX/HIT/HitGeneric";
  public bool show_controllable_tip = true;
  public bool show_task_icon = true;
  public string music_theme;
  public string music_theme_civ;
  [DefaultValue(UnitTextureAtlasID.Units)]
  public UnitTextureAtlasID texture_atlas;
  public bool ignored_by_infinity_coin;
  [DefaultValue("")]
  public string name_taxonomic_kingdom = "";
  [DefaultValue("")]
  public string name_taxonomic_phylum = "";
  [DefaultValue("")]
  public string name_taxonomic_class = "";
  [DefaultValue("")]
  public string name_taxonomic_order = "";
  [DefaultValue("")]
  public string name_taxonomic_family = "";
  [DefaultValue("")]
  public string name_taxonomic_genus = "";
  [DefaultValue("")]
  public string name_taxonomic_species = "";
  [DefaultValue(true)]
  public bool name_subspecies_add_biome_suffix = true;
  public bool auto_civ;
  [DefaultValue("")]
  public string name_locale = "";
  [DefaultValue(StatusTier.Advanced)]
  public StatusTier allowed_status_tiers = StatusTier.Advanced;
  [DefaultValue(true)]
  public bool render_status_effects = true;
  [DefaultValue(ActorSize.S13_Human)]
  public ActorSize actor_size = ActorSize.S13_Human;
  public string[] animation_walk;
  [DefaultValue(10f)]
  public float animation_walk_speed = 10f;
  public string[] animation_swim;
  [DefaultValue(8f)]
  public float animation_swim_speed = 8f;
  public string[] animation_idle = ActorAnimationSequences.walk_0;
  [DefaultValue(10f)]
  public float animation_idle_speed = 10f;
  [DefaultValue(10f)]
  public float max_shake_timer = 10f;
  [DefaultValue(true)]
  public bool animation_speed_based_on_walk_speed = true;
  [DefaultValue("base_attack")]
  public string default_attack = "base_attack";
  public bool immune_to_tumor;
  public bool immune_to_slowness;
  public int aggression;
  [DefaultValue(true)]
  public bool shadow = true;
  [DefaultValue("unitShadow_5")]
  public string shadow_texture = "unitShadow_5";
  [DefaultValue("unitShadow_2")]
  public string shadow_texture_egg = "unitShadow_2";
  [DefaultValue("unitShadow_4")]
  public string shadow_texture_baby = "unitShadow_4";
  [DefaultValue(true)]
  public bool hit_fx_alternative_offset = true;
  [DefaultValue(true)]
  public bool can_level_up = true;
  [DefaultValue(true)]
  public bool can_talk_with = true;
  public float base_throwing_range;
  [DefaultValue(true)]
  public bool use_tool_items = true;
  public bool use_items;
  public bool take_items;
  public bool control_can_jump = true;
  public bool control_can_talk = true;
  public bool control_can_dash = true;
  public bool control_can_backstep = true;
  public bool control_can_steal = true;
  public bool control_can_swear = true;
  public bool control_can_kick = true;
  public bool use_phenotypes;
  [JsonIgnore]
  public Dictionary<string, List<string>> phenotypes_dict;
  public List<string> phenotypes_list;
  public List<string> generated_subspecies_names_prefixes;
  public bool can_be_killed_by_stuff;
  public bool can_be_killed_by_life_eraser;
  public bool can_be_killed_by_divine_light;
  [DefaultValue(true)]
  public bool show_on_meta_layer = true;
  public bool ignore_tile_speed_multiplier;
  public bool skip_fight_logic;
  public bool can_attack_buildings;
  public bool can_attack_brains;
  [DefaultValue(true)]
  public bool count_as_unit = true;
  public bool only_melee_attack;
  public bool flag_ufo;
  public bool flag_finger;
  public bool flag_turtle;
  public bool default_animal;
  public bool civ;
  public bool unit_other;
  [DefaultValue("")]
  public string kingdom_id_wild = "";
  [DefaultValue("")]
  public string kingdom_id_civilization = "";
  public bool special;
  [DefaultValue(true)]
  public bool show_in_taxonomy_tooltip = true;
  [DefaultValue(true)]
  public bool render_budding = true;
  public string family_banner_frame_generation_exclusion = "families/frame_11";
  public string family_banner_frame_generation_inclusion;
  public bool family_banner_frame_only_inclusion;
  [DefaultValue("")]
  public string texture_id = "";
  [DefaultValue("")]
  public string architecture_id = "";
  public string texture_path_zombie_for_auto_loader_main;
  public string texture_path_zombie_for_auto_loader_heads;
  public ActorTextureSubAsset texture_asset;
  public bool prevent_unconscious_rotation;
  public bool render_heads_for_babies;
  private string _debug_phenotype_color = "";
  public bool body_separate_part_hands;
  public bool has_baby_form;
  public bool has_advanced_textures;
  public List<string> decision_ids;
  private DecisionAsset[] _cached_assets_decisions;
  private int _cached_assets_decisions_counter;
  private HashSet<SubspeciesTrait> _cached_assets_subspecies_traits;
  private Sprite _cached_sprite;
  private BaseStats _cached_overview_stats;
  [DefaultValue(0.5f)]
  public float hovering_min = 0.5f;
  [DefaultValue(1.2f)]
  public float hovering_max = 1.2f;
  public bool hovering;
  public bool flying;
  public bool very_high_flyer;
  public bool disable_jump_animation;
  public bool rotating_animation;
  [DefaultValue(true)]
  public bool die_on_blocks = true;
  public bool ignore_blocks;
  [DefaultValue(true)]
  public bool move_from_block = true;
  [DefaultValue(true)]
  public bool run_to_water_when_on_fire = true;
  public bool damaged_by_ocean;
  [DefaultValue(true)]
  public bool cancel_beh_on_land = true;
  public bool force_ocean_creature;
  public bool force_land_creature;
  public bool is_humanoid;
  public bool is_boat;
  public bool is_boat_transport;
  public bool draw_boat_mark;
  public bool draw_boat_mark_big;
  [DefaultValue("")]
  public string boat_type = "";
  [DefaultValue(6)]
  public int animal_breeding_close_units_limit = 6;
  [DefaultValue("")]
  public string avatar_prefab = "";
  [NonSerialized]
  public bool has_avatar_prefab;
  public bool ignore_generic_render;
  public bool need_colored_sprite;
  public bool die_from_dispel;
  [DefaultValue(true)]
  public bool die_in_lava = true;
  public bool can_be_moved_by_powers;
  public bool can_be_hurt_by_powers;
  public bool can_turn_into_ice_one;
  public bool can_turn_into_mush;
  public bool can_turn_into_tumor;
  public bool can_evolve_into_new_species;
  public bool has_soul;
  [DefaultValue(true)]
  public bool can_receive_traits = true;
  public string base_asset_id;
  public string power_id;
  public bool zombie_auto_asset;
  public bool can_turn_into_zombie;
  [DefaultValue("")]
  public string zombie_id_internal = "";
  public string zombie_color_hex = "#3B8130";
  public bool unit_zombie;
  public bool dynamic_sprite_zombie;
  [DefaultValue("")]
  public string skeleton_id = "";
  [DefaultValue("")]
  public string mush_id = "";
  [DefaultValue("")]
  public string tumor_id = "";
  [DefaultValue("")]
  public string evolution_id = "";
  public bool can_turn_into_demon_in_age_of_chaos;
  public bool show_icon_inspect_window;
  [DefaultValue("")]
  public string show_icon_inspect_window_id = "";
  public bool hide_favorite_icon;
  [DefaultValue(true)]
  public bool can_be_favorited = true;
  public bool can_be_inspected;
  [DefaultValue(2.5f)]
  public float inspect_avatar_scale = 2.5f;
  public float inspect_avatar_offset_x;
  public float inspect_avatar_offset_y;
  [DefaultValue(100)]
  public int nutrition_max = 100;
  [DefaultValue(3)]
  public int months_breeding_timeout = 3;
  [DefaultValue(18)]
  public int age_spawn = 18;
  [DefaultValue(true)]
  public bool can_edit_traits = true;
  [DefaultValue(false)]
  public bool can_edit_equipment;
  [DefaultValue(true)]
  public bool finish_scale_on_creation = true;
  [DefaultValue(2f)]
  public float path_movement_timeout = 2f;
  public bool source_meat;
  public bool source_meat_insect;
  [DefaultValue(0.3f)]
  public float default_height = 0.3f;
  public bool update_z;
  public bool visible_on_minimap;
  public bool follow_herd;
  [DefaultValue(true)]
  public bool inspect_stats = true;
  [DefaultValue(true)]
  public bool inspect_children = true;
  [DefaultValue(true)]
  public bool inspect_generation = true;
  [DefaultValue(true)]
  public bool inspect_sex = true;
  [DefaultValue(true)]
  public bool inspect_kills = true;
  [DefaultValue(true)]
  public bool inspect_experience = true;
  [DefaultValue(true)]
  public bool inspect_show_species = true;
  [DefaultValue(true)]
  public bool inspect_mind = true;
  [DefaultValue(true)]
  public bool inspect_genealogy = true;
  [DefaultValue(true)]
  public bool allow_possession = true;
  [DefaultValue(true)]
  public bool allow_strange_urge_movement = true;
  public bool inspect_home;
  public bool immune_to_injuries;
  [DefaultValue(true)]
  public bool can_be_cloned = true;
  [DefaultValue(10)]
  public int experience_given = 10;
  public string[] job;
  public string[] job_citizen;
  public string[] job_kingdom;
  public string[] job_baby;
  public string[] job_attacker;
  public string effect_cast_top = "fx_cast_top_blue";
  public string effect_cast_ground = "fx_cast_ground_blue";
  public string effect_teleport = "fx_teleport_blue";
  public List<string> spell_ids;
  public bool effect_damage;
  public bool can_flip;
  public bool special_dead_animation;
  public bool death_animation_angle;
  [DefaultValue(StatusTier.Advanced)]
  public StatusTier status_tiers = StatusTier.Advanced;
  [DefaultValue(true)]
  public bool has_sprite_renderer = true;
  public bool die_by_lightning;
  [DefaultValue(true)]
  public bool has_skin = true;
  [DefaultValue("")]
  public string grow_into_id = "";
  [DefaultValue("iconQuestionMark")]
  public string icon = "iconQuestionMark";
  public bool skip_save;
  public string color_hex;
  [NonSerialized]
  public Color32? color;
  public ConstructionCost cost;
  [DefaultValue(40)]
  public int species_spawn_radius = 40;
  public bool can_have_subspecies;
  public int genome_size;
  [DefaultValue(30)]
  public int family_spawn_radius = 30;
  [DefaultValue(20)]
  public int family_limit = 20;
  public bool create_family_at_spawn;
  [DefaultValue(FamilyParentsMode.Normal)]
  public FamilyParentsMode family_show_parents;
  [DefaultValue("COLLECTIVE_NAME")]
  public string collective_term = "COLLECTIVE_NAME";
  [DefaultValue(50)]
  public int language_spawn_radius = 50;
  public List<string> traits;
  public HashSet<string> traits_ignore;
  public List<string> preferred_attribute;
  [DefaultValue(null)]
  public HashSet<string> preferred_colors;
  public string[] production;
  [DefaultValue(null)]
  public string[] name_template_sets;
  [DefaultValue("default_name")]
  public string name_template_unit = "default_name";
  [DefaultValue("")]
  public string banner_id = "";
  [DefaultValue("")]
  public string build_order_template_id = "";
  [DefaultValue(4)]
  public int civ_base_cities = 4;
  [DefaultValue(0.35f)]
  public float civ_base_army_multiplier = 0.35f;
  public List<string> default_subspecies_traits;
  public List<string> default_clan_traits;
  public List<string> default_culture_traits;
  public List<string> default_language_traits;
  public List<string> default_kingdom_traits;
  public List<string> default_religion_traits;
  public List<string> trait_filter_subspecies;
  public List<string> trait_group_filter_subspecies;
  public List<ResourceContainer> resources_given;
  public string[] skin_citizen_male;
  public string[] skin_citizen_female;
  public string[] skin_warrior;
  public string[] default_weapons;
  public ActorGetSprite get_override_sprite;
  [NonSerialized]
  public bool has_override_sprite;
  public ActorGetSprites get_override_avatar_frames;
  [NonSerialized]
  public bool has_override_avatar_frames;
  public List<string> chromosomes_first;
  public HashSet<GenomePart> genome_parts = new HashSet<GenomePart>();
  [DefaultValue(3)]
  public int max_random_amount = 3;
  [DefaultValue(true)]
  public bool can_be_surprised = true;
  [NonSerialized]
  public HashSet<Actor> units = new HashSet<Actor>();
  [NonSerialized]
  public ArchitectureAsset architecture_asset;
  [NonSerialized]
  public SpellHolder spells;
  public BaseActionActor action_on_load;
  public WorldAction action_click;
  public WorldAction action_death;
  public DeadAnimation action_dead_animation;
  public GetHitAction action_get_hit;
  public WorldAction check_flip;
  public bool force_hide_stamina;
  public bool force_hide_mana;

  protected override HashSet<string> progress_elements => this._progress_data?.unlocked_actors;

  [JsonIgnore]
  public bool has_sound_idle => this.sound_idle != null;

  [JsonIgnore]
  public bool has_sound_idle_loop => this.sound_idle_loop != null;

  [JsonIgnore]
  public bool has_sound_spawn => this.sound_spawn != null;

  [JsonIgnore]
  public bool has_sound_death => this.sound_death != null;

  [JsonIgnore]
  public bool has_sound_attack => this.sound_attack != null;

  [JsonIgnore]
  public bool has_sound_hit => this.sound_hit != null;

  [JsonIgnore]
  public bool has_music_theme => this.music_theme != null;

  [JsonIgnore]
  public bool has_music_theme_civ => this.music_theme_civ != null;

  public void addSubspeciesNamePrefix(string pName)
  {
    if (this.generated_subspecies_names_prefixes == null)
      this.generated_subspecies_names_prefixes = new List<string>();
    this.generated_subspecies_names_prefixes.Add(pName);
  }

  public bool hasDefaultEggForm()
  {
    List<string> subspeciesTraits = this.default_subspecies_traits;
    // ISSUE: explicit non-virtual call
    return subspeciesTraits != null && __nonvirtual (subspeciesTraits.Contains("reproduction_strategy_oviparity"));
  }

  public string getDefaultEggID()
  {
    string defaultEggId = "egg_shell_plain";
    foreach (string defaultSubspeciesTrait in this.default_subspecies_traits)
    {
      SubspeciesTrait subspeciesTrait = AssetManager.subspecies_traits.get(defaultSubspeciesTrait);
      if (subspeciesTrait.phenotype_egg)
      {
        defaultEggId = subspeciesTrait.id_egg;
        break;
      }
    }
    return defaultEggId;
  }

  [JsonIgnore]
  public string debug_phenotype_colors
  {
    get
    {
      if (string.IsNullOrEmpty(this._debug_phenotype_color))
      {
        List<string> phenotypesList = this.phenotypes_list;
        this._debug_phenotype_color = phenotypesList != null ? phenotypesList.GetRandom<string>() : (string) null;
      }
      return this._debug_phenotype_color;
    }
    set => this._debug_phenotype_color = value;
  }

  [DefaultValue("male_1")]
  public string skin_civ_default_male => "male_1";

  [DefaultValue("female_1")]
  public string skin_civ_default_female => "female_1";

  public void setZombie(bool pAnimal = true)
  {
    if (this.id != "zombie")
      this.base_asset_id = "zombie";
    this.needs_to_be_explored = false;
    this.use_phenotypes = false;
    this.name_locale = "Zombie";
    this.can_attack_brains = true;
    this.kingdom_id_wild = "undead";
    this.collective_term = "group_swarm";
    this.kingdom_id_civilization = string.Empty;
    this.architecture_id = string.Empty;
    this.build_order_template_id = string.Empty;
    this.take_items = false;
    this.follow_herd = false;
    this.can_be_killed_by_divine_light = true;
    this.unit_zombie = true;
    this.has_baby_form = false;
    this.has_advanced_textures = false;
    this.unlocked_with_achievement = false;
    this.achievement_id = (string) null;
    this.addTrait("zombie");
    this.addTrait("stupid");
    this.can_turn_into_zombie = false;
    this.can_turn_into_mush = false;
    this.base_stats["lifespan"] = 0.0f;
    this.zombie_id_internal = string.Empty;
    this.job = Toolbox.a<string>("decision");
    this.can_evolve_into_new_species = false;
    this.addDecision("attack_golden_brain");
    if (this.traits != null)
      this.traits = this.traits.FindAll((Predicate<string>) (pTrait => !AssetManager.traits.get(pTrait).remove_for_zombie_actor_asset));
    if (this.default_subspecies_traits != null)
      this.default_subspecies_traits = this.default_subspecies_traits.FindAll((Predicate<string>) (pTrait => !AssetManager.subspecies_traits.get(pTrait).remove_for_zombies));
    this.default_kingdom_traits = (List<string>) null;
    this.default_culture_traits = (List<string>) null;
    this.default_clan_traits = (List<string>) null;
    this.default_language_traits = (List<string>) null;
    this.default_religion_traits = (List<string>) null;
    this.addTraitGroupFilter("advanced_brain");
    this.addTraitGroupFilter("reproduction_strategy");
    this.addTraitGroupFilter("reproductive_methods");
    this.addTraitGroupFilter("eggs");
    this.addTraitGroupFilter("harmony");
    if (pAnimal)
      this.generateFmodPaths("zombie_animal");
    else
      this.generateFmodPaths("zombie");
    this.music_theme = "Units_Zombie";
    this.sound_hit = "event:/SFX/HIT/HitFlesh";
  }

  public void setCanTurnIntoZombieAsset(string pZombieID, bool pAutoZombieAsset)
  {
    this.can_turn_into_zombie = true;
    this.zombie_auto_asset = pAutoZombieAsset;
    this.zombie_id_internal = pZombieID;
  }

  public string getZombieID()
  {
    return !this.zombie_auto_asset ? this.zombie_id_internal : $"{this.zombie_id_internal}_{this.id}";
  }

  public void cloneTaxonomyFromForSapiens(string pFrom)
  {
    ActorAsset actorAsset = AssetManager.actor_library.get(pFrom);
    this.name_taxonomic_kingdom = actorAsset.name_taxonomic_kingdom;
    this.name_taxonomic_phylum = actorAsset.name_taxonomic_phylum;
    this.name_taxonomic_class = actorAsset.name_taxonomic_class;
    this.name_taxonomic_order = actorAsset.name_taxonomic_order;
    this.name_taxonomic_family = actorAsset.name_taxonomic_family;
    this.name_taxonomic_genus = actorAsset.name_taxonomic_genus;
    this.name_taxonomic_species = "sapiens";
  }

  public string getTaxonomyRank(string pType)
  {
    switch (pType)
    {
      case "taxonomy_class":
        return this.name_taxonomic_class;
      case "taxonomy_family":
        return this.name_taxonomic_family;
      case "taxonomy_genus":
        return this.name_taxonomic_genus;
      case "taxonomy_kingdom":
        return this.name_taxonomic_kingdom;
      case "taxonomy_order":
        return this.name_taxonomic_order;
      case "taxonomy_phylum":
        return this.name_taxonomic_phylum;
      case "taxonomy_species":
        return this.name_taxonomic_species;
      default:
        return string.Empty;
    }
  }

  public bool isTaxonomyRank(string pType, string pID) => this.getTaxonomyRank(pType) == pID;

  public void setSocialStructure(
    string pTerm,
    int pLimit,
    bool pCreateOnSpawn = true,
    bool pFollowHerd = true,
    FamilyParentsMode pShowParents = FamilyParentsMode.Alpha)
  {
    this.collective_term = pTerm;
    this.family_limit = pLimit;
    this.create_family_at_spawn = pCreateOnSpawn;
    this.family_show_parents = pShowParents;
    this.follow_herd = pFollowHerd;
  }

  public void generateFmodPaths(string pID)
  {
    string str = "event:/SFX/UNITS/" + pID;
    if (!this.has_sound_attack)
      this.sound_attack = str + "/attack";
    if (!this.has_sound_death)
      this.sound_death = str + "/death";
    if (!this.has_sound_idle)
      this.sound_idle = str + "/idle";
    if (this.has_sound_spawn)
      return;
    this.sound_spawn = str + "/spawn";
  }

  public void clonePhenotype(string pFrom)
  {
    ActorAsset actorAsset = AssetManager.actor_library.get(pFrom);
    if (actorAsset.phenotypes_dict == null)
      return;
    this.phenotypes_dict = new Dictionary<string, List<string>>((IDictionary<string, List<string>>) actorAsset.phenotypes_dict);
    this.phenotypes_list = new List<string>((IEnumerable<string>) actorAsset.phenotypes_list);
  }

  public PhenotypeAsset getDefaultPhenotypeAsset()
  {
    string phenotypes = this.phenotypes_list[0];
    return AssetManager.phenotype_library.get(phenotypes);
  }

  public void clearTraits()
  {
    if (this.traits == null)
      return;
    this.traits.Clear();
  }

  public string getCollectiveTermID() => this.collective_term;

  public override string getLocaleID() => this.name_locale.Underscore();

  public string getDescriptionID() => this.getGodPower()?.getDescriptionID();

  public string getLocalizedName()
  {
    return this.isAvailable() ? LocalizedTextManager.getText(this.getLocaleID()) : LocalizedTextManager.getText("achievement_tip_hidden");
  }

  public string getLocalizedDescription()
  {
    string localizedDescription;
    if (!this.isAvailable())
    {
      if (this.unlocked_with_achievement)
        localizedDescription = LocalizedTextManager.getText("actor_locked_tooltip_text_achievement").Replace("$achievement_id$", $"<color=#00ffffff>{this.getAchievementLocaleID().Localize()}</color>");
      else
        localizedDescription = LocalizedTextManager.getText("actor_locked_tooltip_text_exploration");
    }
    else
      localizedDescription = LocalizedTextManager.getText(this.getDescriptionID());
    return localizedDescription;
  }

  public void addPreferredColors(params string[] pColors)
  {
    this.preferred_colors = new HashSet<string>((IEnumerable<string>) pColors);
  }

  public string getTranslatedName() => this.getLocaleID().Localize();

  public void addGenome(params (string, float)[] pListGenomePartsIDs)
  {
    for (int index = 0; index < pListGenomePartsIDs.Length; ++index)
    {
      (string id, float pValue) = pListGenomePartsIDs[index];
      GenomePart equalValue = new GenomePart(id, pValue);
      if (!this.genome_parts.Add(equalValue))
      {
        GenomePart actualValue;
        this.genome_parts.TryGetValue(equalValue, out actualValue);
        GenomePart genomePart = new GenomePart(id, actualValue.value + pValue);
        this.genome_parts.Remove(actualValue);
        this.genome_parts.Add(genomePart);
      }
    }
  }

  public string getIconPath() => "ui/Icons/" + this.icon;

  public Sprite getSpriteIcon()
  {
    if (this._cached_sprite == null)
      this._cached_sprite = SpriteTextureLoader.getSprite(this.getIconPath());
    return this._cached_sprite;
  }

  public override Sprite getSprite() => this.getSpriteIcon();

  public bool hasBiomePhenotype(string pBiomeID)
  {
    return this.phenotypes_dict != null && this.phenotypes_dict.Count != 0 && this.phenotypes_dict.ContainsKey(pBiomeID);
  }

  public BaseStats getStatsForOverview()
  {
    if (this._cached_overview_stats == null)
    {
      this._cached_overview_stats = new BaseStats();
      this._cached_overview_stats["health"] = this.base_stats["health"];
      this._cached_overview_stats["lifespan"] = this.base_stats["lifespan"];
      this._cached_overview_stats["damage"] = this.base_stats["damage"];
      this._cached_overview_stats["speed"] = this.base_stats["speed"];
      foreach (GenomePart genomePart in this.genome_parts)
      {
        string id = genomePart.id;
        if (id == "health" || id == "lifespan" || id == "damage" || id == "speed")
          this._cached_overview_stats[genomePart.id] += genomePart.value;
      }
    }
    return this._cached_overview_stats;
  }

  public bool hasDecisions()
  {
    List<string> decisionIds = this.decision_ids;
    // ISSUE: explicit non-virtual call
    return decisionIds != null && __nonvirtual (decisionIds.Count) > 0;
  }

  public DecisionAsset[] getDecisions()
  {
    if (this.hasDecisions() && this._cached_assets_decisions == null)
    {
      this._cached_assets_decisions = new DecisionAsset[64 /*0x40*/];
      foreach (string decisionId in this.decision_ids)
      {
        DecisionAsset decisionAsset = AssetManager.decisions_library.get(decisionId);
        if (decisionAsset != null)
          this._cached_assets_decisions[this._cached_assets_decisions_counter++] = decisionAsset;
      }
    }
    return this._cached_assets_decisions;
  }

  public int decisions_counter => this._cached_assets_decisions_counter;

  public string getDefaultKingdom() => this.kingdom_id_wild;

  public HashSet<SubspeciesTrait> getDefaultSubspeciesTraits()
  {
    if (this.default_subspecies_traits == null)
      return (HashSet<SubspeciesTrait>) null;
    if (this._cached_assets_subspecies_traits == null)
    {
      this._cached_assets_subspecies_traits = new HashSet<SubspeciesTrait>();
      this.default_subspecies_traits.Sort((Comparison<string>) ((pS1, pS2) => string.Compare(pS1, pS2, StringComparison.Ordinal)));
      foreach (string defaultSubspeciesTrait in this.default_subspecies_traits)
      {
        SubspeciesTrait subspeciesTrait = AssetManager.subspecies_traits.get(defaultSubspeciesTrait);
        if (subspeciesTrait != null)
          this._cached_assets_subspecies_traits.Add(subspeciesTrait);
      }
    }
    return this._cached_assets_subspecies_traits;
  }

  public int countPopulation() => this.units.Count;

  public int countSubspecies()
  {
    int num = 0;
    foreach (MetaObject<SubspeciesData> metaObject in (CoreSystemManager<Subspecies, SubspeciesData>) World.world.subspecies)
    {
      if (metaObject.getActorAsset() == this)
        ++num;
    }
    return num;
  }

  public int countFamilies()
  {
    int num = 0;
    foreach (MetaObject<FamilyData> family in (CoreSystemManager<Family, FamilyData>) World.world.families)
    {
      if (family.getActorAsset() == this)
        ++num;
    }
    return num;
  }

  public void addSpell(string pID)
  {
    if (this.spell_ids == null)
      this.spell_ids = new List<string>();
    this.spell_ids.Add(pID);
  }

  public void addTraitGroupFilter(string pTrait)
  {
    if (this.trait_group_filter_subspecies == null)
      this.trait_group_filter_subspecies = new List<string>();
    if (this.trait_group_filter_subspecies.Contains(pTrait))
      return;
    this.trait_group_filter_subspecies.Add(pTrait);
  }

  public void addTrait(string pTraitID)
  {
    if (this.traits == null)
      this.traits = new List<string>();
    if (this.traits.Contains(pTraitID))
      return;
    this.traits.Add(pTraitID);
  }

  public void addTraitIgnore(string pTraitID)
  {
    if (this.traits_ignore == null)
      this.traits_ignore = new HashSet<string>();
    this.traits_ignore.Add(pTraitID);
  }

  public void removeTrait(string pTrait)
  {
    this.traits?.Remove(pTrait);
    List<string> traits = this.traits;
    // ISSUE: explicit non-virtual call
    if ((traits != null ? (__nonvirtual (traits.Count) == 0 ? 1 : 0) : 0) == 0)
      return;
    this.traits = (List<string>) null;
  }

  public void addSubspeciesTrait(string pTrait)
  {
    if (this.default_subspecies_traits == null)
      this.default_subspecies_traits = new List<string>();
    if (this.default_subspecies_traits.Contains(pTrait))
      return;
    this.default_subspecies_traits.Add(pTrait);
  }

  public void addCultureTrait(string pTrait)
  {
    if (this.default_culture_traits == null)
      this.default_culture_traits = new List<string>();
    if (this.default_culture_traits.Contains(pTrait))
      return;
    this.default_culture_traits.Add(pTrait);
  }

  public void addLanguageTrait(string pTrait)
  {
    if (this.default_language_traits == null)
      this.default_language_traits = new List<string>();
    if (this.default_language_traits.Contains(pTrait))
      return;
    this.default_language_traits.Add(pTrait);
  }

  public void addKingdomTrait(string pTrait)
  {
    if (this.default_kingdom_traits == null)
      this.default_kingdom_traits = new List<string>();
    if (this.default_kingdom_traits.Contains(pTrait))
      return;
    this.default_kingdom_traits.Add(pTrait);
  }

  public void addClanTrait(string pTrait)
  {
    if (this.default_clan_traits == null)
      this.default_clan_traits = new List<string>();
    if (this.default_clan_traits.Contains(pTrait))
      return;
    this.default_clan_traits.Add(pTrait);
  }

  public void addReligionTrait(string pTrait)
  {
    if (this.default_religion_traits == null)
      this.default_religion_traits = new List<string>();
    if (this.default_religion_traits.Contains(pTrait))
      return;
    this.default_religion_traits.Add(pTrait);
  }

  public void addDecision(string pDecision)
  {
    if (this.decision_ids == null)
      this.decision_ids = new List<string>();
    if (this.decision_ids.Contains(pDecision))
      return;
    this.decision_ids.Add(pDecision);
  }

  public override bool unlock(bool pSaveData = true)
  {
    if (!base.unlock(pSaveData))
      return false;
    string pID = string.IsNullOrEmpty(this.base_asset_id) ? this.id : this.base_asset_id;
    ActorAsset key;
    if (pID != this.id)
    {
      key = AssetManager.actor_library.get(pID);
      if (!key.unlock(true))
        return false;
    }
    else
      key = this;
    PowerButton powerButton;
    if (PowerButton.actor_spawn_buttons.TryGetValue(key, out powerButton))
      ((Graphic) powerButton.icon).color = Toolbox.color_white;
    return true;
  }

  protected override bool isDebugUnlockedAll() => DebugConfig.isOn(DebugOption.UnlockAllActors);

  public bool canEditItem(EquipmentAsset pItem)
  {
    return !pItem.is_pool_weapon && this.can_edit_equipment || pItem.is_pool_weapon && this.can_edit_equipment;
  }

  public void addResource(string pID, int pAmount, bool pNewList = false)
  {
    if (this.resources_given == null | pNewList)
      this.resources_given = new List<ResourceContainer>();
    if (this.resources_given.Count > 0)
    {
      for (int index = 0; index < this.resources_given.Count; ++index)
      {
        ResourceContainer resourceContainer = this.resources_given[index];
        if (resourceContainer.id == pID)
        {
          resourceContainer.amount += pAmount;
          this.resources_given[index] = resourceContainer;
          return;
        }
      }
    }
    this.resources_given.Add(new ResourceContainer(pID, pAmount));
  }

  public BuildingAsset getBuildingDockAsset()
  {
    string pID = "docks_" + this.architecture_id;
    return AssetManager.buildings.get(pID);
  }

  public void setSimpleCivSettings()
  {
    this.skin_citizen_male = Toolbox.a<string>("male_1");
    this.skin_citizen_female = Toolbox.a<string>("female_1");
    this.skin_warrior = Toolbox.a<string>("warrior_1");
    this.production = new string[1]{ "bread" };
    this.build_order_template_id = "build_order_basic";
    this.name_template_sets = Toolbox.a<string>("default_name");
    this.job = Toolbox.a<string>("decision");
    this.job_citizen = Toolbox.a<string>("unit_citizen");
    this.job_kingdom = Toolbox.a<string>("decision");
    this.job_baby = Toolbox.a<string>("decision");
    this.job_attacker = Toolbox.a<string>("attacker");
    this.kingdom_id_wild = "neutral_animals";
  }

  public bool canBecomeSapient() => !string.IsNullOrEmpty(this.kingdom_id_civilization);

  public bool hasDefaultSpells() => this.spells != null && this.spells.hasAny();

  public TooltipData getTooltip()
  {
    GodPower godPower = this.getGodPower();
    return new TooltipData()
    {
      tip_name = this.getLocaleID(),
      tip_description = this.getDescriptionID(),
      power = godPower
    };
  }

  public GodPower getGodPower()
  {
    string pID = this.power_id ?? this.base_asset_id ?? this.id;
    return !AssetManager.powers.has(pID) ? (GodPower) null : AssetManager.powers.get(pID);
  }

  public string getNameTemplate(MetaType pType)
  {
    string[] nameTemplateSets = this.name_template_sets;
    string random = nameTemplateSets != null ? nameTemplateSets.GetRandom<string>() : (string) null;
    if (!string.IsNullOrEmpty(random))
      return AssetManager.name_sets.get(random).get(pType);
    if (pType != MetaType.Unit)
      return this.name_template_unit;
    string.IsNullOrEmpty(this.name_template_unit);
    return this.name_template_unit;
  }

  [JsonIgnore]
  public string boat_texture_id => this.id;

  public string[] getWalk() => this.animation_walk;

  public string[] getIdle() => this.animation_idle;

  public string[] getSwim() => this.animation_swim;
}
