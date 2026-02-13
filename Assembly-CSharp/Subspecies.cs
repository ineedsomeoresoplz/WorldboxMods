// Decompiled with JetBrains decompiler
// Type: Subspecies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Subspecies : MetaObjectWithTraits<SubspeciesData, SubspeciesTrait>, ISapient
{
  private const float AGE_THRESHOLD_ADULT = 30f;
  private const float AGE_AGE_BREEDING_CIV = 18f;
  private const float AGE_THRESHOLD_ADULT_CIV = 16f;
  private const float AGE_MAX_ADULT = 16f;
  private const float AGE_EXPONENTIAL_ADULT = 0.55f;
  private const float AGE_MULTIPLIER_ADULT = 1.1f;
  public readonly Nucleus nucleus = new Nucleus();
  private int _cached_phenotype_index_for_banner;
  private List<PhenotypeAsset> _phenotype_list_assets = new List<PhenotypeAsset>();
  private HashSet<int> _phenotypes_set_indexes = new HashSet<int>();
  private bool _has_egg_form;
  private bool _has_mutation_reskin;
  private bool _needs_food;
  private bool _needs_mate;
  private bool _can_process_emotions;
  private bool _is_sapient;
  private bool _has_advanced_memory;
  private bool _has_advanced_communication;
  private bool _damaged_by_water;
  private bool _timid;
  private bool _curious;
  private bool _water_creature;
  private bool _hovering;
  private bool _pollinating;
  private bool _magic;
  private bool _diet_flowers;
  private bool _diet_fruits;
  private bool _diet_crops;
  private bool _diet_vegetation;
  private bool _diet_meat;
  private bool _diet_blood;
  private bool _diet_minerals;
  private bool _diet_wood;
  private bool _diet_cannibalism;
  private int _cached_metabolic_rate;
  private bool _cached_energy_preserver;
  private int _cached_males;
  private int _cached_females;
  private string _egg_id;
  private SubspeciesTrait _egg_asset;
  private string _mutation_skin_id;
  private SubspeciesTrait _mutation_skin_asset;
  private string _cached_skin_male;
  private string _cached_skin_female;
  private string _cached_skin_warrior;
  private readonly HashSet<string> _allowed_food_by_diet = new HashSet<string>();
  private readonly SubspeciesActorBirthTraits _actor_birth_traits = new SubspeciesActorBirthTraits();
  private bool _trait_changed_event;
  private Sprite _cached_unit_sprite_for_banner;
  private const string reproduction_neuron = "reproduction_neuron";
  private const string reproduction_basics_1 = "reproduction_basics_1";
  private const string reproduction_basics_2 = "reproduction_basics_2";
  private const string reproduction_basics_3 = "reproduction_basics_3";
  private const string reproduction_basics_4 = "reproduction_basics_4";
  private const string reproduction_sexual_try = "reproduction_sexual_try";
  private const string reproduction_acts = "reproduction_acts";
  private const string reproduction = "reproduction";
  private const string births = "births";
  private const string new_adults = "new_adults";
  public static string[] ALL_REPRODUCTION_COUNTERS = new string[10]
  {
    nameof (reproduction_neuron),
    nameof (reproduction_basics_1),
    nameof (reproduction_basics_2),
    nameof (reproduction_basics_3),
    nameof (reproduction_basics_4),
    nameof (reproduction_sexual_try),
    nameof (reproduction_acts),
    nameof (reproduction),
    nameof (births),
    nameof (new_adults)
  };
  public RateCounter counter_reproduction_neuron;
  public RateCounter counter_reproduction_basics_1;
  public RateCounter counter_reproduction_basics_2;
  public RateCounter counter_reproduction_basics_3;
  public RateCounter counter_reproduction_basics_4;
  public RateCounter counter_reproduction_sexual_try;
  public RateCounter counter_reproduction;
  public RateCounter counter_reproduction_acts;
  public RateCounter counter_births;
  public RateCounter counter_new_adults;
  public List<RateCounter> list_counters = new List<RateCounter>();

  protected override MetaType meta_type => MetaType.Subspecies;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.subspecies;

  protected override bool track_death_types => true;

  protected override void setDefaultValues()
  {
    base.setDefaultValues();
    this.initReproductionCounters();
  }

  public void newSpecies(ActorAsset pAsset, WorldTile pTile, bool pMutation = false)
  {
    this.data.species_id = pAsset.id;
    this.generateNewMetaObject();
    if (pMutation)
      this.addDNAMutationToSeed();
    this.generateNucleus();
    this.generateActorBirthTraits();
    this.generatePhenotype(pAsset, pTile);
    this.generateName(pAsset, pTile);
    this.createSkins();
    this._trait_changed_event = false;
    this.recalcBaseStats();
  }

  protected override void generateNewMetaObject()
  {
    base.generateNewMetaObject();
    if (!WorldLawLibrary.world_law_mutant_box.isEnabled())
      return;
    int num = Randy.randomInt(1, 4);
    for (int index = 0; index < num; ++index)
    {
      SubspeciesTrait randomSpawnTrait = AssetManager.subspecies_traits.getRandomSpawnTrait();
      if (randomSpawnTrait.isAvailable())
        this.addTrait(randomSpawnTrait, true);
    }
  }

  private void createSkins()
  {
    this.data.skin_id = Randy.randomInt(0, this.getActorAsset().skin_citizen_female.Length);
  }

  public string getSkinFemale() => this._cached_skin_female;

  public string getSkinMale() => this._cached_skin_male;

  public string getSkinWarrior() => this._cached_skin_warrior;

  public bool hasEvolvedIntoForm() => this.data.evolved_into_subspecies.hasValue();

  public Subspecies getEvolvedInto()
  {
    Subspecies subspecies = World.world.subspecies.get(this.data.evolved_into_subspecies);
    if (subspecies == null)
      return (Subspecies) null;
    return !subspecies.isAlive() ? (Subspecies) null : subspecies;
  }

  public void setEvolutionSubspecies(Subspecies pSubspecies)
  {
    if (this.data.evolved_into_subspecies.hasValue() && (double) World.world.getWorldTimeElapsedSince(this.data.last_evolution_timestamp) < 60.0)
      return;
    this.data.last_evolution_timestamp = World.world.getCurWorldTime();
    this.data.evolved_into_subspecies = pSubspecies.getID();
  }

  public int getMaxRandomMutations() => (int) this.base_stats_meta["mutation"];

  public int getAmountOfRandomMutationsSubspecies()
  {
    int maxRandomMutations = this.getMaxRandomMutations();
    return maxRandomMutations == 0 ? 0 : Randy.randomInt(0, maxRandomMutations + 1);
  }

  public int getAmountOfRandomMutationsActorTraits()
  {
    return Randy.randomInt(0, this.getMaxRandomMutations() + 1);
  }

  public void mutateFrom(Subspecies pParentsSubspecies)
  {
    int mutationsSubspecies = pParentsSubspecies.getAmountOfRandomMutationsSubspecies();
    this.cloneSubspeciesTraits(pParentsSubspecies);
    this.nucleus.cloneFrom(pParentsSubspecies.nucleus);
    this.nucleus.doRandomGeneMutations(mutationsSubspecies + 1);
    this.mutateTraits(mutationsSubspecies);
    this.genesChangedEvent();
    this.increaseGeneration(pParentsSubspecies.getGeneration());
  }

  private void increaseGeneration(int pFromGeneration) => this.setGeneration(pFromGeneration + 1);

  private void setGeneration(int pValue) => this.data.generation = pValue;

  public int getGeneration() => this.data.generation;

  private void cloneSubspeciesTraits(Subspecies pParentsSubspecies)
  {
    bool unitZombie = this.getActorAsset().unit_zombie;
    this.clearTraits();
    foreach (SubspeciesTrait trait in (IEnumerable<SubspeciesTrait>) pParentsSubspecies.getTraits())
    {
      if (!unitZombie || !trait.remove_for_zombies)
        this.addTrait(trait, false);
    }
  }

  internal void mutateTraits(int pMutations)
  {
    int num1 = 0;
    for (int index = 0; index < pMutations; ++index)
    {
      if (this.addTrait(AssetManager.subspecies_traits.getRandomMutationTraitToAdd(), true))
        ++num1;
    }
    if (num1 <= 0)
      return;
    int num2 = 0;
    for (int index = 0; index < num1; ++index)
    {
      if (this.removeTrait(AssetManager.subspecies_traits.getRandomMutationTraitToRemove()))
        ++num2;
    }
  }

  public override void increaseBirths()
  {
    base.increaseBirths();
    this.addRenown(1);
    this.counter_births?.registerEvent();
  }

  public bool needOppositeSexTypeForReproduction() => this.hasTraitReproductionSexual();

  public bool isPartnerSuitableForReproduction(Actor pActor, Actor pTarget)
  {
    return !this.needOppositeSexTypeForReproduction() || pActor.data.sex != pTarget.data.sex;
  }

  public int getRandomPhenotypeIndex()
  {
    PhenotypeAsset randomPhenotypeAsset = this.getRandomPhenotypeAsset();
    return randomPhenotypeAsset == null ? 0 : randomPhenotypeAsset.phenotype_index;
  }

  public PhenotypeAsset getRandomPhenotypeAsset()
  {
    return this._phenotype_list_assets.Count == 0 ? (PhenotypeAsset) null : this._phenotype_list_assets.GetRandom<PhenotypeAsset>();
  }

  public int getMainPhenotypeIndexForBanner() => this._cached_phenotype_index_for_banner;

  protected override AssetLibrary<SubspeciesTrait> trait_library
  {
    get => (AssetLibrary<SubspeciesTrait>) AssetManager.subspecies_traits;
  }

  protected override List<string> default_traits => this.getActorAsset().default_subspecies_traits;

  protected override List<string> saved_traits => this.data.saved_traits;

  protected override string species_id => this.data.species_id;

  public void generateActorBirthTraits()
  {
    this._actor_birth_traits.init(this.getActorAsset(), this);
  }

  public void makeSapient()
  {
    this.addTrait("amygdala");
    this.addTrait("advanced_hippocampus");
    this.addTrait("prefrontal_cortex");
    this.addTrait("wernicke_area");
  }

  public void generateNucleus()
  {
    ActorAsset actorAsset = this.getActorAsset();
    Randy.resetSeed(World.world.map_stats.life_dna + (long) actorAsset.getIndexID() + (long) actorAsset.countSubspecies() + (long) this.data.mutation);
    this.nucleus.createFrom(actorAsset);
  }

  public void addDNAMutationToSeed() => this.data.mutation = Randy.randomInt(0, 55555);

  public void genesChangedEvent()
  {
    this.nucleus.setDirty();
    this.recalcBaseStats();
    this.makeAllUnitsDirtyAndConfused();
  }

  private void makeAllUnitsDirtyAndConfused()
  {
    foreach (Actor unit in this.units)
    {
      if (!unit.isRekt())
      {
        unit.event_full_stats = true;
        unit.setStatsDirty();
        unit.cancelAllBeh();
        unit.makeConfused();
      }
    }
  }

  public bool isBiomeSpecific() => !(this.data.biome_variant == "default_color");

  public bool hasPhenotype() => this.getActorAsset().use_phenotypes;

  public override void generateBanner()
  {
    this.data.banner_background_id = AssetManager.subspecies_banners_library.getNewIndexBackground();
  }

  public int getMetabolicRate() => this._cached_metabolic_rate;

  protected override void recalcBaseStats()
  {
    base.recalcBaseStats();
    this.clearVisualCache();
    if (this._trait_changed_event)
    {
      this._trait_changed_event = false;
      this.makeAllUnitsDirtyAndConfused();
    }
    this.base_stats.mergeStats(this.getActorAsset().base_stats);
    this.base_stats.mergeStats(this.nucleus.getStats());
    this.base_stats_meta.mergeStats(this.nucleus.getStatsMeta());
    this.base_stats["health"] = Mathf.Max(this.base_stats["health"], 1f);
    this.base_stats["damage"] = Mathf.Max(this.base_stats["damage"], 1f);
    this.base_stats["lifespan"] = Mathf.Max(this.base_stats["lifespan"], 1f);
    this.base_stats["speed"] = Mathf.Max(this.base_stats["speed"], 1f);
    this._cached_metabolic_rate = (int) Mathf.Max((float) SimGlobals.m.base_metabolic_rate, this.base_stats["metabolic_rate"]);
    this._cached_energy_preserver = this.hasTrait("energy_preserver");
    this._timid = this.hasTrait("cautious_instincts");
    this._curious = this.hasTrait("inquisitive_nature");
    this._water_creature = this.hasTrait("aquatic");
    this._hovering = this.hasTrait("hovering");
    this.checkForgetMetas();
    this.cacheTags();
    this.calcAllowedFoodByDiet();
    this.checkMutationSkin();
    this.cacheSkins();
    this.checkReproductionStrategy();
    this.calculateAgeRelatedStats();
    this.checkCurrentColor();
  }

  public int cached_males => this._cached_males;

  public int cached_females => this._cached_females;

  public bool has_trait_energy_preserver => this._cached_energy_preserver;

  public bool has_trait_timid => this._timid;

  public bool has_trait_curious => this._curious;

  public bool has_trait_water_creature => this._water_creature;

  public bool has_trait_hovering => this._hovering;

  public bool has_trait_pollinating => this._pollinating;

  private void checkForgetMetas()
  {
    int num = this._is_sapient ? 1 : 0;
    bool hasAdvancedMemory = this._has_advanced_memory;
    bool advancedCommunication = this._has_advanced_communication;
    bool flag1 = this.hasMetaTag("has_sapience");
    bool flag2 = this.hasMetaTag("has_advanced_memory");
    bool flag3 = this.hasMetaTag("has_advanced_communication");
    if (num != 0 && !flag1)
    {
      foreach (Actor unit in this.units)
      {
        if (!unit.isRekt() && unit.isKingdomCiv())
          unit.forgetKingdomAndCity();
      }
    }
    if (hasAdvancedMemory != flag2)
    {
      foreach (Actor unit in this.units)
      {
        if (!unit.isRekt())
        {
          if (unit.hasCulture())
            unit.forgetCulture();
          if (unit.hasReligion())
            unit.forgetReligion();
        }
      }
    }
    if (advancedCommunication == flag3)
      return;
    foreach (Actor unit in this.units)
    {
      if (!unit.isRekt() && unit.hasLanguage())
        unit.forgetLanguage();
    }
  }

  private void calculateAgeRelatedStats()
  {
    this.getActorAsset();
    int baseStat = (int) this.base_stats["lifespan"];
    float num1;
    float num2;
    if ((double) baseStat > 30.0 && this.isSapient())
    {
      num1 = 16f;
      num2 = 18f;
    }
    else
    {
      num1 = Mathf.Pow((float) baseStat, 0.55f) * 1.1f;
      num2 = num1;
    }
    if ((double) num1 > 16.0)
      num1 = 16f;
    if (this.isSapient() && (double) num2 > 18.0)
      num2 = 18f;
    this.base_stats_meta["age_adult"] = num1;
    this.base_stats_meta["age_breeding"] = num2;
  }

  private void cacheTags()
  {
    this._is_sapient = this.hasMetaTag("has_sapience");
    this._needs_food = this.hasMetaTag("needs_food");
    this._needs_mate = this.hasMetaTag("needs_mate");
    this._can_process_emotions = this.hasMetaTag("has_emotions");
    this._has_advanced_memory = this.hasMetaTag("has_advanced_memory");
    this._has_advanced_communication = this.hasMetaTag("has_advanced_communication");
    this._damaged_by_water = this.hasMetaTag("damaged_by_water");
    this._diet_vegetation = this.hasMetaTag("diet_vegetation");
    this._diet_meat = this.hasMetaTag("diet_meat");
    this._diet_blood = this.hasMetaTag("diet_blood");
    this._diet_minerals = this.hasMetaTag("diet_minerals");
    this._diet_wood = this.hasMetaTag("diet_wood");
    this._diet_cannibalism = this.hasMetaTag("diet_same_species");
    this._magic = this.hasMetaTag("magic");
  }

  public bool hasCannibalism() => this._diet_cannibalism;

  public bool isSapient() => this._is_sapient;

  public bool isMagic() => this._magic;

  public ReproductiveStrategy getReproductionStrategy()
  {
    if (this.hasTraitOviparity())
      return ReproductiveStrategy.Egg;
    return this.hasTraitViviparity() ? ReproductiveStrategy.Pregnancy : ReproductiveStrategy.SpawnUnitImmediate;
  }

  public bool isReproductionSexual() => this.hasMetaTag("reproduction_sexual");

  public bool hasTraitReproductionSexual() => this.hasTrait("reproduction_sexual");

  public bool hasTraitReproductionSexualHermaphroditic()
  {
    return this.hasTrait("reproduction_hermaphroditic");
  }

  public bool hasTraitOviparity() => this.hasTrait("reproduction_strategy_oviparity");

  public bool hasTraitViviparity() => this.hasTrait("reproduction_strategy_viviparity");

  private void checkReproductionStrategy()
  {
    bool hasEggForm = this._has_egg_form;
    if (this.hasTrait("reproduction_strategy_oviparity"))
    {
      this._has_egg_form = true;
      this._egg_id = "egg_shell_plain";
      foreach (SubspeciesTrait trait in (IEnumerable<SubspeciesTrait>) this.getTraits())
      {
        if (trait.phenotype_egg)
        {
          this._egg_id = trait.id_egg;
          break;
        }
      }
      this._egg_asset = AssetManager.subspecies_traits.get(this._egg_id);
    }
    else
      this._has_egg_form = false;
    if (hasEggForm == this._has_egg_form)
      return;
    this.resetUnitSprites();
    foreach (Actor unit in this.units)
    {
      if (!unit.isRekt())
        unit.cancelAllBeh();
    }
  }

  private void checkMutationSkin()
  {
    this._mutation_skin_asset = (SubspeciesTrait) null;
    bool hasMutationReskin = this._has_mutation_reskin;
    this._has_mutation_reskin = false;
    foreach (SubspeciesTrait trait in (IEnumerable<SubspeciesTrait>) this.getTraits())
    {
      if (trait.is_mutation_skin)
      {
        this._mutation_skin_id = trait.id;
        this._mutation_skin_asset = AssetManager.subspecies_traits.get(this._mutation_skin_id);
        this._has_mutation_reskin = true;
        break;
      }
    }
    if (hasMutationReskin == this._has_mutation_reskin)
      return;
    this.resetUnitSprites();
  }

  private void cacheSkins()
  {
    int skinId = this.data.skin_id;
    if (this._has_mutation_reskin)
    {
      int count = this._mutation_skin_asset.skin_citizen_male.Count;
      int index = Toolbox.loopIndex(skinId, count);
      this._cached_skin_male = this._mutation_skin_asset.skin_citizen_male[index];
      this._cached_skin_female = this._mutation_skin_asset.skin_citizen_female[index];
      this._cached_skin_warrior = this._mutation_skin_asset.skin_warrior[index];
    }
    else
    {
      ActorAsset actorAsset = this.getActorAsset();
      this._cached_skin_male = actorAsset.skin_citizen_male[skinId];
      this._cached_skin_female = actorAsset.skin_citizen_female[skinId];
      this._cached_skin_warrior = actorAsset.skin_warrior[skinId];
    }
  }

  private void checkCurrentColor()
  {
    if (!this.getActorAsset().use_phenotypes)
      return;
    ListPool<PhenotypeAsset> pList1 = new ListPool<PhenotypeAsset>((ICollection<PhenotypeAsset>) this._phenotype_list_assets);
    this.clearPhenotypeCache();
    this.fillPhenotypeCache();
    List<PhenotypeAsset> phenotypeListAssets = this._phenotype_list_assets;
    if (Toolbox.areListsEqual<PhenotypeAsset>((IList<PhenotypeAsset>) pList1, (IList<PhenotypeAsset>) phenotypeListAssets))
      return;
    this.resetUnitSprites();
    this._cached_phenotype_index_for_banner = this._phenotype_list_assets.GetRandom<PhenotypeAsset>().phenotype_index;
  }

  private void fillPhenotypeCache()
  {
    ActorAsset actorAsset = this.getActorAsset();
    if (!actorAsset.use_phenotypes)
      return;
    foreach (SubspeciesTrait trait in (IEnumerable<SubspeciesTrait>) this.getTraits())
    {
      if (trait.phenotype_skin)
        this.cachePhenotype(trait.getPhenotypeAsset());
    }
    if (this._phenotypes_set_indexes.Count != 0)
      return;
    this.cachePhenotype(actorAsset.getDefaultPhenotypeAsset());
  }

  private void clearPhenotypeCache()
  {
    this._phenotype_list_assets.Clear();
    this._phenotypes_set_indexes.Clear();
  }

  private void cachePhenotype(PhenotypeAsset pPhenotypeAsset)
  {
    this._phenotype_list_assets.Add(pPhenotypeAsset);
    this._phenotypes_set_indexes.Add(pPhenotypeAsset.phenotype_index);
  }

  public void checkPhenotypeColor()
  {
    foreach (Actor unit in this.units)
    {
      if (!unit.isRekt())
        this.checkIfPhenotypeIsLegit(unit);
    }
  }

  private void checkIfPhenotypeIsLegit(Actor pActor)
  {
    int phenotypeIndex = pActor.data.phenotype_index;
    if (phenotypeIndex != 0 && this._phenotypes_set_indexes.Contains(phenotypeIndex))
      return;
    pActor.generatePhenotypeAndShade();
  }

  private void resetUnitSprites()
  {
    foreach (Actor unit in this.units)
    {
      if (!unit.isRekt())
      {
        this.checkIfPhenotypeIsLegit(unit);
        unit.setStatsDirty();
        unit.clearSprites();
        unit.clearLastColorCache();
      }
    }
  }

  public int countCurrentFamilies()
  {
    int num = 0;
    foreach (CoreSystemObject<FamilyData> family in (CoreSystemManager<Family, FamilyData>) World.world.families)
    {
      if (family.data.subspecies_id == this.data.id)
        ++num;
    }
    return num;
  }

  public Sprite getSpriteBackground()
  {
    return AssetManager.subspecies_banners_library.getSpriteBackground(this.data.banner_background_id);
  }

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.subspecies_colors_library;
  }

  public bool isSpecies(string pSpeciesCheck) => this.species_id == pSpeciesCheck;

  private void generateName(ActorAsset pAsset, WorldTile pTile)
  {
    using (StringBuilderPool pName = new StringBuilderPool())
    {
      pName.Append(pAsset.name_taxonomic_genus);
      if (!string.IsNullOrEmpty(pAsset.name_taxonomic_species))
      {
        pName.Append(" ");
        pName.Append(pAsset.name_taxonomic_species);
      }
      if (pAsset.name_subspecies_add_biome_suffix && pTile.Type.is_biome && pAsset.hasBiomePhenotype(pTile.Type.biome_asset.id))
      {
        string random = pTile.Type.biome_asset.subspecies_name_suffix.GetRandom<string>();
        pName.Append(" ");
        pName.Append(random);
      }
      for (int index = 0; index < 5 && this.hasNameInWorld(pName); ++index)
        pName.Append(SubspeciesManager.NAME_ENDINGS.GetRandom<string>());
      pName.ToTitleCase();
      this.setName(pName.ToString());
    }
  }

  private bool hasNameInWorld(StringBuilderPool pName)
  {
    ReadOnlySpan<char> readOnlySpan = Span<char>.op_Implicit(pName.AsSpan());
    Span<char> span1 = Span<char>.op_Implicit(new char[readOnlySpan.Length]);
    MemoryExtensions.ToLowerInvariant(readOnlySpan, span1);
    Span<char> span2 = Span<char>.op_Implicit(new char[pName.Length]);
    foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) World.world.subspecies)
    {
      if (subspecies != this)
      {
        string name = subspecies.name;
        if (name.Length == span1.Length)
        {
          MemoryExtensions.ToLowerInvariant(MemoryExtensions.AsSpan(name), span2);
          if (MemoryExtensions.SequenceEqual<char>(span1, Span<char>.op_Implicit(span2)))
            return true;
        }
      }
    }
    return false;
  }

  private void calcAllowedFoodByDiet()
  {
    this._allowed_food_by_diet.Clear();
    foreach (KeyValuePair<string, List<string>> dietFoodPool in AssetManager.resources.diet_food_pools)
    {
      if (this.hasTrait(dietFoodPool.Key))
        this._allowed_food_by_diet.UnionWith((IEnumerable<string>) dietFoodPool.Value);
    }
  }

  public HashSet<string> getAllowedFoodByDiet() => this._allowed_food_by_diet;

  private void generatePhenotype(ActorAsset pAsset, WorldTile pTile)
  {
    if (!pAsset.use_phenotypes)
      return;
    this.data.biome_variant = pTile.Type.biome_id;
    if (string.IsNullOrEmpty(this.data.biome_variant))
      this.data.biome_variant = "default_color";
    this.generatePhenotype(pAsset, this.data.biome_variant);
  }

  private void generatePhenotype(ActorAsset pAsset, string pColorVariationForBiome = "default_color")
  {
    if (!pAsset.use_phenotypes)
      return;
    if (pAsset.phenotypes_dict == null || pAsset.phenotypes_dict.Count == 0)
    {
      Debug.LogError((object) ("No phenotypes. Check assets " + pAsset.id));
    }
    else
    {
      if (!pAsset.hasBiomePhenotype(pColorVariationForBiome))
        pColorVariationForBiome = "default_color";
      List<string> list = pAsset.phenotypes_dict[pColorVariationForBiome];
      if (list.Count == 0)
        return;
      string random = list.GetRandom<string>();
      PhenotypeAsset phenotypeAsset = AssetManager.phenotype_library.get(random);
      this.addTrait(AssetManager.subspecies_traits.get(phenotypeAsset.subspecies_trait_id), false);
    }
  }

  public override void loadData(SubspeciesData pData)
  {
    base.loadData(pData);
    this.nucleus.reset();
    List<ChromosomeData> savedChromosomeData = this.data.saved_chromosome_data;
    // ISSUE: explicit non-virtual call
    if ((savedChromosomeData != null ? (__nonvirtual (savedChromosomeData.Count) > 0 ? 1 : 0) : 0) != 0)
    {
      foreach (ChromosomeData pData1 in this.data.saved_chromosome_data)
      {
        Chromosome pChromosome = new Chromosome(pData1.chromosome_type, false);
        pChromosome.load(pData1);
        this.nucleus.addChromosome(pChromosome);
      }
    }
    this._actor_birth_traits.setSubspecies(this);
    this._actor_birth_traits.reset();
    this._actor_birth_traits.fillTraitAssetsFromStringList((IEnumerable<string>) this.data.saved_actor_birth_traits);
    this.recalcBaseStats();
  }

  public override void save()
  {
    base.save();
    this.data.saved_chromosome_data = this.nucleus.getListForSave();
    this.data.saved_traits = this.getTraitsAsStrings();
    this.data.saved_actor_birth_traits = this._actor_birth_traits.getTraitsAsStrings();
  }

  public void debugClear() => this.loadData(this.data);

  public float age_adult => this.base_stats_meta[nameof (age_adult)];

  public float age_breeding => this.base_stats_meta[nameof (age_breeding)];

  public bool diet_vegetation => this._diet_vegetation;

  public bool diet_meat => this._diet_meat;

  public BaseStats base_stats_male => this.nucleus.base_stats_male;

  public BaseStats base_stats_female => this.nucleus.base_stats_female;

  public bool needs_food => this._needs_food;

  public bool needs_mate => this._needs_mate;

  public bool can_process_emotions => this._can_process_emotions;

  public bool has_advanced_memory => this._has_advanced_memory;

  public bool has_advanced_communication => this._has_advanced_communication;

  public bool is_damaged_by_water => this._damaged_by_water;

  public bool has_egg_form => this._has_egg_form;

  public string egg_id => this._egg_id;

  public string egg_sprite_path => this._egg_asset.sprite_path;

  public SubspeciesTrait egg_asset => this._egg_asset;

  public Sprite egg_sprite => this._egg_asset.getSprite();

  public bool has_mutation_reskin => this._has_mutation_reskin;

  public SubspeciesTrait mutation_skin_asset => this._mutation_skin_asset;

  public string getRandomBioProduct()
  {
    using (ListPool<string> list = new ListPool<string>())
    {
      if (this.hasTrait("bioproduct_gems"))
        list.Add("mineral_gems");
      if (this.hasTrait("bioproduct_stone"))
        list.Add("mineral_stone");
      if (this.hasTrait("bioproduct_mushrooms"))
      {
        list.Add("mushroom_red");
        list.Add("mushroom_green");
        list.Add("mushroom_teal");
        list.Add("mushroom_white");
        list.Add("mushroom_yellow");
      }
      if (this.hasTrait("bioproduct_gold"))
        list.Add("mineral_gold");
      return list.Count == 0 ? "poop" : list.GetRandom<string>();
    }
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "subspecies");
    this._mutation_skin_asset = (SubspeciesTrait) null;
    this._cached_phenotype_index_for_banner = 0;
    this._phenotype_list_assets.Clear();
    this._phenotypes_set_indexes.Clear();
    this.base_stats.reset();
    this.nucleus.reset();
    this._actor_birth_traits.reset();
    this.spells.reset();
    this._egg_asset = (SubspeciesTrait) null;
    base.Dispose();
  }

  public bool hasParentSubspecies() => this.data.parent_subspecies.hasValue();

  public void unstableGenomeEvent()
  {
    this.nucleus.unstableGenomeEvent();
    this.genesChangedEvent();
  }

  public void cacheCounters()
  {
    this._cached_females = this.countFemales();
    this._cached_males = this.countMales();
  }

  public void eventGMO()
  {
    this.addTrait("gmo");
    this._trait_changed_event = true;
  }

  public float getMaturationTimeMonths() => 0.0f + this.base_stats_meta["maturation"];

  public override bool addTrait(SubspeciesTrait pTrait, bool pRemoveOpposites = false)
  {
    return this.canAddTrait(pTrait) && base.addTrait(pTrait, pRemoveOpposites);
  }

  public bool canAddTrait(SubspeciesTrait pTrait)
  {
    ActorAsset actorAsset = this.getActorAsset();
    return (actorAsset.trait_filter_subspecies == null || !actorAsset.trait_filter_subspecies.Contains(pTrait.id)) && (actorAsset.trait_group_filter_subspecies == null || !actorAsset.trait_group_filter_subspecies.Contains(pTrait.group_id));
  }

  public string getPossibleAttribute()
  {
    return this.nucleus.pot_possible_attributes.Count == 0 ? (string) null : this.nucleus.pot_possible_attributes.GetRandom<string>();
  }

  public bool addBirthTrait(string pActorTraitID)
  {
    ActorTrait pTrait = AssetManager.traits.get(pActorTraitID);
    return pTrait != null && this._actor_birth_traits.addTrait(pTrait);
  }

  public SubspeciesActorBirthTraits getActorBirthTraits() => this._actor_birth_traits;

  public int countMainKingdoms()
  {
    int num = 0;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.getMainSubspecies() == this)
        ++num;
    }
    return num;
  }

  public bool hasPopulationLimit() => (double) this.base_stats_meta["limit_population"] > 0.0;

  public bool hasReachedPopulationLimit()
  {
    int num = (int) this.base_stats_meta["limit_population"];
    return num != 0 && this.countUnits() >= num;
  }

  public int countMainCities()
  {
    int num = 0;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city.getMainSubspecies() == this)
        ++num;
    }
    return num;
  }

  public Subspecies getSkeletonForm()
  {
    Subspecies pObject = World.world.subspecies.get(this.data.skeleton_form_id);
    return pObject.isRekt() ? (Subspecies) null : pObject;
  }

  public override void traitModifiedEvent()
  {
    this._trait_changed_event = true;
    base.traitModifiedEvent();
  }

  public void setSkeletonForm(Subspecies pSkeletonForm)
  {
    this.data.skeleton_form_id = pSkeletonForm.id;
    ActorAsset actorAsset = pSkeletonForm.getActorAsset();
    string pString = "";
    if (actorAsset.generated_subspecies_names_prefixes != null)
      pString = actorAsset.generated_subspecies_names_prefixes.GetRandom<string>();
    if (string.IsNullOrEmpty(pString))
      return;
    string pName = $"{pString.FirstToUpper()} {this.name}";
    pSkeletonForm.setName(pName, false);
  }

  private void clearVisualCache() => this._cached_unit_sprite_for_banner = (Sprite) null;

  public Sprite getUnitSpriteForBanner()
  {
    if (Object.op_Inequality((Object) this._cached_unit_sprite_for_banner, (Object) null))
      return this._cached_unit_sprite_for_banner;
    ActorAsset actorAsset = this.getActorAsset();
    SubspeciesTrait pMutationAsset = (SubspeciesTrait) null;
    ActorTextureSubAsset textureAsset;
    if (this.has_mutation_reskin)
    {
      pMutationAsset = this.mutation_skin_asset;
      textureAsset = pMutationAsset.texture_asset;
    }
    else
      textureAsset = actorAsset.texture_asset;
    AnimationContainerUnit containerForUi = DynamicActorSpriteCreatorUI.getContainerForUI(actorAsset, true, textureAsset, pMutationAsset);
    ColorAsset defaultKingdomColor = AssetManager.kingdoms.get(actorAsset.kingdom_id_wild).default_kingdom_color;
    int phenotypeIndexForBanner = this.getMainPhenotypeIndexForBanner();
    int pPhenotypeShade = 0;
    Sprite unitSpriteForUi = DynamicActorSpriteCreatorUI.getUnitSpriteForUI(actorAsset, containerForUi.walking.frames[0], containerForUi, true, ActorSex.Male, phenotypeIndexForBanner, pPhenotypeShade, defaultKingdomColor, 0L, 0);
    this._cached_unit_sprite_for_banner = unitSpriteForUi;
    return unitSpriteForUi;
  }

  public override bool hasCities()
  {
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city.getMainSubspecies() == this)
        return true;
    }
    return false;
  }

  public override IEnumerable<City> getCities()
  {
    Subspecies subspecies = this;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city.getMainSubspecies() == subspecies)
        yield return city;
    }
  }

  public override bool hasKingdoms()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.getMainSubspecies() == this)
        return true;
    }
    return false;
  }

  public override IEnumerable<Kingdom> getKingdoms()
  {
    Subspecies subspecies = this;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.getMainSubspecies() == subspecies)
        yield return kingdom;
    }
  }

  public void initReproductionCounters()
  {
  }

  private RateCounter checkNewCounter(RateCounter pCounter, string pID)
  {
    if (pCounter == null)
    {
      pCounter = new RateCounter(pID);
      this.list_counters.Add(pCounter);
    }
    pCounter.reset();
    return pCounter;
  }

  public void debugReproductionEvents(DebugTool pTool)
  {
  }

  public void counterReproduction() => this.counter_reproduction?.registerEvent();

  public void countReproductionNeuron() => this.counter_reproduction_neuron?.registerEvent();
}
