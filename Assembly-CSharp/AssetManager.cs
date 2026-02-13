// Decompiled with JetBrains decompiler
// Type: AssetManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using db;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#nullable disable
public class AssetManager
{
  public static TileEffectsLibrary tile_tile_effects;
  public static KingdomBannerLibrary kingdom_banners_library;
  public static CultureBannerLibrary culture_banners_library;
  public static ClanBannerLibrary clan_banners_library;
  public static ReligionBannerLibrary religion_banners_library;
  public static LanguageBannerLibrary language_banners_library;
  public static SubspeciesBannerLibrary subspecies_banners_library;
  public static FamilysBannerLibrary family_banners_library;
  public static WorldTimeScaleLibrary time_scales;
  public static OptionsLibrary options_library;
  public static WorldLawLibrary world_laws_library;
  public static WorldLawGroupLibrary world_law_groups;
  public static OnomasticsLibrary onomastics_library;
  public static OnomasticsEvolutionLibrary onomastics_evolution_library;
  public static LinguisticsLibrary linguistics_library;
  public static WordsLibrary words_library;
  public static SentencesLibrary sentences_library;
  public static StoryLibrary story_library;
  public static RarityLibrary rarity_library;
  public static GraphTimeLibrary graph_time_library;
  public static HistoryDataLibrary history_data_library;
  public static HistoryMetaDataLibrary history_meta_data_library;
  public static BaseStatsLibrary base_stats_library;
  public static ChromosomeTypeLibrary chromosome_type_library;
  public static GeneLibrary gene_library;
  public static NameplateLibrary nameplates_library;
  public static MetaTypeLibrary meta_type_library;
  public static MetaCustomizationLibrary meta_customization_library;
  public static MetaRepresentationLibrary meta_representation_library;
  public static DecisionsLibrary decisions_library;
  public static NeuralLayerLibrary neural_layers;
  public static LoyaltyLibrary loyalty_library;
  public static OpinionLibrary opinion_library;
  public static HappinessLibrary happiness_library;
  public static KingdomJobLibrary job_kingdom;
  public static BehaviourTaskKingdomLibrary tasks_kingdom;
  public static WorldLogLibrary world_log_library;
  public static HistoryGroupLibrary history_groups;
  public static CityJobLibrary job_city;
  public static BehaviourTaskCityLibrary tasks_city;
  public static ActorJobLibrary job_actor;
  public static BehaviourTaskActorLibrary tasks_actor;
  public static CitizenJobLibrary citizen_job_library;
  public static CultureTraitLibrary culture_traits;
  public static CultureTraitGroupLibrary culture_trait_groups;
  public static LanguageTraitLibrary language_traits;
  public static LanguageTraitGroupLibrary language_trait_groups;
  public static SubspeciesTraitLibrary subspecies_traits;
  public static SubspeciesTraitGroupLibrary subspecies_trait_groups;
  public static ClanTraitLibrary clan_traits;
  public static ClanTraitGroupLibrary clan_trait_groups;
  public static ReligionTraitLibrary religion_traits;
  public static ReligionTraitGroupLibrary religion_trait_groups;
  public static TraitRainLibrary trait_rains;
  public static CommunicationLibrary communication_library;
  public static CommunicationTopicLibrary communication_topic_library;
  public static BookTypeLibrary book_types;
  public static PersonalityLibrary personalities;
  public static ProfessionLibrary professions;
  public static DropsLibrary drops;
  public static BuildingLibrary buildings;
  public static ActorAssetLibrary actor_library;
  public static ActorTraitLibrary traits;
  public static ActorTraitGroupLibrary trait_groups;
  public static KingdomLibrary kingdoms;
  public static KingdomTraitLibrary kingdoms_traits;
  public static KingdomTraitGroupLibrary kingdoms_traits_groups;
  public static NameGeneratorLibrary name_generator;
  public static NameSetsLibrary name_sets;
  public static DisasterLibrary disasters;
  public static PhenotypeLibrary phenotype_library;
  public static BiomeLibrary biome_library;
  public static ResourceLibrary resources;
  public static ItemLibrary items;
  public static ItemModifierLibrary items_modifiers;
  public static ItemGroupLibrary item_groups;
  public static UnitHandToolLibrary unit_hand_tools;
  public static ProjectileLibrary projectiles;
  public static BuildOrderLibrary city_build_orders;
  public static ArchitectureLibrary architecture_library;
  public static CloudLibrary clouds;
  public static MonthLibrary months;
  public static TileLibrary tiles;
  public static TopTileLibrary top_tiles;
  public static TerraformLibrary terraform;
  public static PowerLibrary powers;
  public static SpellLibrary spells;
  public static StatusLibrary status;
  public static TesterJobLibrary tester_jobs;
  public static TesterBehTaskLibrary tester_tasks;
  public static MusicBoxLibrary music_box;
  public static AchievementLibrary achievements;
  public static AchievementGroupLibrary achievement_groups;
  public static SignalLibrary signals;
  public static MapGenSettingsLibrary map_gen_settings;
  public static MapGenTemplateLibrary map_gen_templates;
  public static QuantumSpriteLibrary quantum_sprites;
  public static WorldBehaviourLibrary world_behaviours;
  public static MapSizeLibrary map_sizes;
  public static WorldAgeLibrary era_library;
  public static EffectsLibrary effects_library;
  public static SimGlobals sim_globals_library;
  public static ColorStyleLibrary color_style_library;
  public static ClanColorsLibrary clan_colors_library;
  public static SubspeciesColorsLibrary subspecies_colors_library;
  public static FamiliesColorsLibrary families_colors_library;
  public static ArmiesColorsLibrary armies_colors_library;
  public static LanguagesColorsLibrary languages_colors_library;
  public static KingdomColorsLibrary kingdom_colors_library;
  public static CultureColorsLibrary culture_colors_library;
  public static ReligionColorsLibrary religion_colors_library;
  public static ArchitectMoodLibrary architect_mood_library;
  public static PlotsLibrary plots_library;
  public static PlotCategoryLibrary plot_category_library;
  public static TooltipLibrary tooltips;
  public static WarTypeLibrary war_types_library;
  public static HotkeyLibrary hotkey_library;
  public static StatisticsLibrary statistics_library;
  public static BrushLibrary brush_library;
  public static DebugToolLibrary debug_tool_library;
  public static CombatActionLibrary combat_action_library;
  public static DynamicSpritesLibrary dynamic_sprites_library;
  public static KnowledgeLibrary knowledge_library;
  public static WindowLibrary window_library;
  public static MetaTextReportLibrary meta_text_report_library;
  public static ListWindowLibrary list_window_library;
  public static PowerTabLibrary power_tab_library;
  public static LocaleGroupLibrary locale_groups_library;
  public static GameLanguageLibrary game_language_library;
  private static AssetManager _instance;
  private readonly List<BaseAssetLibrary> _list = new List<BaseAssetLibrary>();
  private readonly Dictionary<string, BaseAssetLibrary> _dict = new Dictionary<string, BaseAssetLibrary>();
  private string _assetgv;
  public static HashSet<string> missing_locale_keys = new HashSet<string>();

  public static void clear() => AssetManager._instance = (AssetManager) null;

  public static void initMain()
  {
    if (AssetManager._instance != null)
      return;
    AssetManager._instance = new AssetManager();
  }

  public static void init() => AssetManager._instance.initLibs();

  public AssetManager()
  {
    this._assetgv = Config.gv;
    this.add((BaseAssetLibrary) (AssetManager.game_language_library = new GameLanguageLibrary()), "game_languages");
    this.add((BaseAssetLibrary) (AssetManager.options_library = new OptionsLibrary()), nameof (options_library));
  }

  private void initLibs()
  {
    this.add((BaseAssetLibrary) (AssetManager.tile_tile_effects = new TileEffectsLibrary()), "tile_effects");
    this.add((BaseAssetLibrary) (AssetManager.base_stats_library = new BaseStatsLibrary()), "base_stats_library");
    this.add((BaseAssetLibrary) (AssetManager.world_log_library = new WorldLogLibrary()), "world_log_library");
    this.add((BaseAssetLibrary) (AssetManager.history_groups = new HistoryGroupLibrary()), "history_groups");
    this.add((BaseAssetLibrary) (AssetManager.decisions_library = new DecisionsLibrary()), "decisions_library");
    this.add((BaseAssetLibrary) (AssetManager.neural_layers = new NeuralLayerLibrary()), "neural_layer_library");
    this.add((BaseAssetLibrary) (AssetManager.graph_time_library = new GraphTimeLibrary()), "graph_time_library");
    this.add((BaseAssetLibrary) (AssetManager.history_data_library = new HistoryDataLibrary()), "history_data_library");
    this.add((BaseAssetLibrary) (AssetManager.history_meta_data_library = new HistoryMetaDataLibrary()), "history_meta_data_library");
    this.add((BaseAssetLibrary) (AssetManager.world_laws_library = new WorldLawLibrary()), "world_laws_library");
    this.add((BaseAssetLibrary) (AssetManager.world_law_groups = new WorldLawGroupLibrary()), "world_law_groups");
    this.add((BaseAssetLibrary) (AssetManager.meta_type_library = new MetaTypeLibrary()), "meta_type_library");
    this.add((BaseAssetLibrary) (AssetManager.meta_text_report_library = new MetaTextReportLibrary()), "meta_text_report_library");
    this.add((BaseAssetLibrary) (AssetManager.meta_customization_library = new MetaCustomizationLibrary()), "meta_customization_library");
    this.add((BaseAssetLibrary) (AssetManager.meta_representation_library = new MetaRepresentationLibrary()), "meta_representation_library");
    this.add((BaseAssetLibrary) (AssetManager.culture_banners_library = new CultureBannerLibrary()), "culture_banners_library");
    this.add((BaseAssetLibrary) (AssetManager.kingdom_banners_library = new KingdomBannerLibrary()), "kingdom_banners_library");
    this.add((BaseAssetLibrary) (AssetManager.clan_banners_library = new ClanBannerLibrary()), "clan_banners_library");
    this.add((BaseAssetLibrary) (AssetManager.religion_banners_library = new ReligionBannerLibrary()), "religion_banners_library");
    this.add((BaseAssetLibrary) (AssetManager.language_banners_library = new LanguageBannerLibrary()), "language_banners_library");
    this.add((BaseAssetLibrary) (AssetManager.subspecies_banners_library = new SubspeciesBannerLibrary()), "subspecies_banners_library");
    this.add((BaseAssetLibrary) (AssetManager.family_banners_library = new FamilysBannerLibrary()), "family_banners_library");
    this.add((BaseAssetLibrary) (AssetManager.time_scales = new WorldTimeScaleLibrary()), "world_time_scales_library");
    this.add((BaseAssetLibrary) (AssetManager.communication_library = new CommunicationLibrary()), "communication_library");
    this.add((BaseAssetLibrary) (AssetManager.communication_topic_library = new CommunicationTopicLibrary()), "communication_topic_library");
    this.add((BaseAssetLibrary) (AssetManager.city_build_orders = new BuildOrderLibrary()), "city_build_orders");
    this.add((BaseAssetLibrary) (AssetManager.architecture_library = new ArchitectureLibrary()), "architecture_library");
    this.add((BaseAssetLibrary) (AssetManager.book_types = new BookTypeLibrary()), "book_types_library");
    this.add((BaseAssetLibrary) (AssetManager.nameplates_library = new NameplateLibrary()), "nameplates_library");
    this.add((BaseAssetLibrary) (AssetManager.combat_action_library = new CombatActionLibrary()), "combat_action_library");
    this.add((BaseAssetLibrary) (AssetManager.biome_library = new BiomeLibrary()), "biome_library");
    this.add((BaseAssetLibrary) (AssetManager.phenotype_library = new PhenotypeLibrary()), "phenotype_library");
    this.add((BaseAssetLibrary) (AssetManager.dynamic_sprites_library = new DynamicSpritesLibrary()), "dynamic_sprites_library");
    this.add((BaseAssetLibrary) (AssetManager.debug_tool_library = new DebugToolLibrary()), "debug_tool_library");
    this.add((BaseAssetLibrary) (AssetManager.brush_library = new BrushLibrary()), "brush_library");
    this.add((BaseAssetLibrary) (AssetManager.chromosome_type_library = new ChromosomeTypeLibrary()), "chromosome_type_library");
    this.add((BaseAssetLibrary) (AssetManager.gene_library = new GeneLibrary()), "gene_library");
    this.add((BaseAssetLibrary) (AssetManager.loyalty_library = new LoyaltyLibrary()), "loyalty_library");
    this.add((BaseAssetLibrary) (AssetManager.opinion_library = new OpinionLibrary()), "opinion_library");
    this.add((BaseAssetLibrary) (AssetManager.happiness_library = new HappinessLibrary()), "happiness_library");
    this.add((BaseAssetLibrary) (AssetManager.hotkey_library = new HotkeyLibrary()), "hotkey_library");
    this.add((BaseAssetLibrary) (AssetManager.tooltips = new TooltipLibrary()), "tooltips");
    this.add((BaseAssetLibrary) (AssetManager.war_types_library = new WarTypeLibrary()), "war_types_library");
    this.add((BaseAssetLibrary) (AssetManager.sim_globals_library = new SimGlobals()), "sim_globals_library");
    this.add((BaseAssetLibrary) (AssetManager.color_style_library = new ColorStyleLibrary()), "color_style_library");
    this.add((BaseAssetLibrary) (AssetManager.effects_library = new EffectsLibrary()), "effects_library");
    this.add((BaseAssetLibrary) (AssetManager.kingdom_colors_library = new KingdomColorsLibrary()), "kingdom_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.clan_colors_library = new ClanColorsLibrary()), "clan_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.subspecies_colors_library = new SubspeciesColorsLibrary()), "species_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.languages_colors_library = new LanguagesColorsLibrary()), "language_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.families_colors_library = new FamiliesColorsLibrary()), "families_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.armies_colors_library = new ArmiesColorsLibrary()), "armies_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.culture_colors_library = new CultureColorsLibrary()), "culture_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.religion_colors_library = new ReligionColorsLibrary()), "religion_colors_library");
    this.add((BaseAssetLibrary) (AssetManager.months = new MonthLibrary()), "month_library");
    this.add((BaseAssetLibrary) (AssetManager.era_library = new WorldAgeLibrary()), "era_library");
    this.add((BaseAssetLibrary) (AssetManager.clouds = new CloudLibrary()), "cloud_library");
    this.add((BaseAssetLibrary) (AssetManager.map_sizes = new MapSizeLibrary()), "map_sizes");
    this.add((BaseAssetLibrary) (AssetManager.music_box = new MusicBoxLibrary()), "music_box");
    this.add((BaseAssetLibrary) (AssetManager.tiles = new TileLibrary()), "tiles");
    this.add((BaseAssetLibrary) (AssetManager.top_tiles = new TopTileLibrary()), "top_tiles");
    this.add((BaseAssetLibrary) (AssetManager.culture_traits = new CultureTraitLibrary()), "culture_traits");
    this.add((BaseAssetLibrary) (AssetManager.culture_trait_groups = new CultureTraitGroupLibrary()), "culture_trait_groups");
    this.add((BaseAssetLibrary) (AssetManager.language_traits = new LanguageTraitLibrary()), "language_traits");
    this.add((BaseAssetLibrary) (AssetManager.language_trait_groups = new LanguageTraitGroupLibrary()), "language_trait_groups");
    this.add((BaseAssetLibrary) (AssetManager.clan_traits = new ClanTraitLibrary()), "clan_traits");
    this.add((BaseAssetLibrary) (AssetManager.clan_trait_groups = new ClanTraitGroupLibrary()), "clan_trait_groups");
    this.add((BaseAssetLibrary) (AssetManager.subspecies_traits = new SubspeciesTraitLibrary()), "subspecies_traits");
    this.add((BaseAssetLibrary) (AssetManager.subspecies_trait_groups = new SubspeciesTraitGroupLibrary()), "subspecies_trait_groups");
    this.add((BaseAssetLibrary) (AssetManager.religion_traits = new ReligionTraitLibrary()), "religion_traits");
    this.add((BaseAssetLibrary) (AssetManager.religion_trait_groups = new ReligionTraitGroupLibrary()), "religion_trait_groups");
    this.add((BaseAssetLibrary) (AssetManager.trait_rains = new TraitRainLibrary()), "trait_rains");
    this.add((BaseAssetLibrary) (AssetManager.professions = new ProfessionLibrary()), "professions");
    this.add((BaseAssetLibrary) (AssetManager.quantum_sprites = new QuantumSpriteLibrary()), "quantum_sprites");
    this.add((BaseAssetLibrary) (AssetManager.world_behaviours = new WorldBehaviourLibrary()), "world_behaviours");
    this.add((BaseAssetLibrary) (AssetManager.personalities = new PersonalityLibrary()), "personalities");
    this.add((BaseAssetLibrary) (AssetManager.drops = new DropsLibrary()), "drops");
    this.add((BaseAssetLibrary) (AssetManager.status = new StatusLibrary()), "status");
    this.add((BaseAssetLibrary) (AssetManager.spells = new SpellLibrary()), "spells");
    this.add((BaseAssetLibrary) (AssetManager.citizen_job_library = new CitizenJobLibrary()), "citizen_job_library");
    this.add((BaseAssetLibrary) (AssetManager.tasks_actor = new BehaviourTaskActorLibrary()), "beh_actor");
    this.add((BaseAssetLibrary) (AssetManager.tasks_city = new BehaviourTaskCityLibrary()), "beh_city");
    this.add((BaseAssetLibrary) (AssetManager.tasks_kingdom = new BehaviourTaskKingdomLibrary()), "beh_kingdom");
    this.add((BaseAssetLibrary) (AssetManager.traits = new ActorTraitLibrary()), "traits");
    this.add((BaseAssetLibrary) (AssetManager.trait_groups = new ActorTraitGroupLibrary()), "trait_groups");
    this.add((BaseAssetLibrary) (AssetManager.plots_library = new PlotsLibrary()), "plots");
    this.add((BaseAssetLibrary) (AssetManager.plot_category_library = new PlotCategoryLibrary()), "plot_group");
    this.add((BaseAssetLibrary) (AssetManager.kingdoms = new KingdomLibrary()), "kingdoms");
    this.add((BaseAssetLibrary) (AssetManager.kingdoms_traits_groups = new KingdomTraitGroupLibrary()), "kingdom_trait_group");
    this.add((BaseAssetLibrary) (AssetManager.kingdoms_traits = new KingdomTraitLibrary()), "kingdom_traits");
    this.add((BaseAssetLibrary) (AssetManager.actor_library = new ActorAssetLibrary()), "units");
    this.add((BaseAssetLibrary) (AssetManager.buildings = new BuildingLibrary()), "buildings");
    this.add((BaseAssetLibrary) (AssetManager.name_generator = new NameGeneratorLibrary()), "name_generator");
    this.add((BaseAssetLibrary) (AssetManager.name_sets = new NameSetsLibrary()), "name_sets");
    this.add((BaseAssetLibrary) (AssetManager.disasters = new DisasterLibrary()), "disasters");
    this.add((BaseAssetLibrary) (AssetManager.job_actor = new ActorJobLibrary()), "job_actor");
    this.add((BaseAssetLibrary) (AssetManager.job_city = new CityJobLibrary()), "job_city");
    this.add((BaseAssetLibrary) (AssetManager.job_kingdom = new KingdomJobLibrary()), "job_kingdom");
    this.add((BaseAssetLibrary) (AssetManager.powers = new PowerLibrary()), "powers");
    this.add((BaseAssetLibrary) (AssetManager.items = new ItemLibrary()), "items");
    this.add((BaseAssetLibrary) (AssetManager.items_modifiers = new ItemModifierLibrary()), "items_modifiers");
    this.add((BaseAssetLibrary) (AssetManager.item_groups = new ItemGroupLibrary()), "item_groups");
    this.add((BaseAssetLibrary) (AssetManager.unit_hand_tools = new UnitHandToolLibrary()), "tools");
    this.add((BaseAssetLibrary) (AssetManager.resources = new ResourceLibrary()), "resources");
    this.add((BaseAssetLibrary) (AssetManager.terraform = new TerraformLibrary()), "terraform");
    this.add((BaseAssetLibrary) (AssetManager.projectiles = new ProjectileLibrary()), "projectiles");
    this.add((BaseAssetLibrary) (AssetManager.signals = new SignalLibrary()), "signals");
    this.add((BaseAssetLibrary) (AssetManager.achievement_groups = new AchievementGroupLibrary()), "achievement_groups");
    this.add((BaseAssetLibrary) (AssetManager.map_gen_templates = new MapGenTemplateLibrary()), "map_gen_templates");
    this.add((BaseAssetLibrary) (AssetManager.map_gen_settings = new MapGenSettingsLibrary()), "map_gen_settings");
    this.add((BaseAssetLibrary) (AssetManager.statistics_library = new StatisticsLibrary()), "statistics_library");
    this.add((BaseAssetLibrary) (AssetManager.linguistics_library = new LinguisticsLibrary()), "linguistics_library");
    this.add((BaseAssetLibrary) (AssetManager.words_library = new WordsLibrary()), "words_library");
    this.add((BaseAssetLibrary) (AssetManager.sentences_library = new SentencesLibrary()), "sentences_library");
    this.add((BaseAssetLibrary) (AssetManager.story_library = new StoryLibrary()), "story_library");
    this.add((BaseAssetLibrary) (AssetManager.onomastics_library = new OnomasticsLibrary()), "onomastics_library");
    this.add((BaseAssetLibrary) (AssetManager.onomastics_evolution_library = new OnomasticsEvolutionLibrary()), "onomastics_evolution_library");
    this.add((BaseAssetLibrary) (AssetManager.rarity_library = new RarityLibrary()), "rarity_library");
    this.add((BaseAssetLibrary) (AssetManager.knowledge_library = new KnowledgeLibrary()), "knowledge_library");
    this.add((BaseAssetLibrary) (AssetManager.window_library = new WindowLibrary()), "window_library");
    this.add((BaseAssetLibrary) (AssetManager.list_window_library = new ListWindowLibrary()), "list_window_library");
    this.add((BaseAssetLibrary) (AssetManager.power_tab_library = new PowerTabLibrary()), "power_tab_library");
    this.add((BaseAssetLibrary) (AssetManager.architect_mood_library = new ArchitectMoodLibrary()), "architect_mood_library");
    this.add((BaseAssetLibrary) (AssetManager.achievements = new AchievementLibrary()), "achievements");
    if (DebugConfig.isOn(DebugOption.TesterLibs))
      AssetManager.loadAutoTester();
    this.add((BaseAssetLibrary) (AssetManager.locale_groups_library = new LocaleGroupLibrary()), "locale_groups");
    foreach (BaseAssetLibrary baseAssetLibrary in this._list)
      baseAssetLibrary.post_init();
    foreach (BaseAssetLibrary baseAssetLibrary in this._list)
      baseAssetLibrary.linkAssets();
  }

  public static void loadAutoTester()
  {
    if (AssetManager.tester_jobs != null)
      return;
    AssetManager._instance.add((BaseAssetLibrary) (AssetManager.tester_jobs = new TesterJobLibrary()), "tester_jobs");
    AssetManager._instance.add((BaseAssetLibrary) (AssetManager.tester_tasks = new TesterBehTaskLibrary()), "tester_tasks");
  }

  internal static void generateMissingLocalesFile()
  {
  }

  public void exportAssets()
  {
    if (!DebugConfig.isOn(DebugOption.ExportAssetLibraries))
      return;
    MiniBench miniBench = new MiniBench(nameof (exportAssets), 25L);
    string path = "GenAssets/wbassets";
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    Parallel.ForEach<BaseAssetLibrary>((IEnumerable<BaseAssetLibrary>) this._list, new ParallelOptions()
    {
      MaxDegreeOfParallelism = 3
    }, (Action<BaseAssetLibrary>) (pLib => pLib.exportAssets()));
    miniBench.Dispose();
  }

  private void add(BaseAssetLibrary pLibrary, string pId)
  {
    if (this._assetgv[0] != '0')
      return;
    pLibrary.init();
    this._list.Add(pLibrary);
    this._dict.Add(pId, pLibrary);
    pLibrary.id = pId;
  }

  public static IEnumerable<BaseAssetLibrary> getList()
  {
    return (IEnumerable<BaseAssetLibrary>) AssetManager._instance._list;
  }

  public static bool has(string pLibraryID) => AssetManager._instance._dict.ContainsKey(pLibraryID);

  public static BaseAssetLibrary get(string pLibraryID) => AssetManager._instance._dict[pLibraryID];
}
