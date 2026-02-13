// Decompiled with JetBrains decompiler
// Type: MapBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using ai.behaviours;
using db;
using DG.Tweening;
using EpPathFinding.cs;
using life.taxi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using tools.debug;
using UnityEngine;

#nullable disable
public class MapBox : MonoBehaviour
{
  internal GameObject joys;
  public const float TRANSITION_EFFECT_ALPHA = 0.1f;
  public const float TRANSITION_EFFECT_ALPHA_SPEED = 0.1f;
  internal SaveManager save_manager;
  internal ResourceThrowManager resource_throw_manager;
  internal GameStats game_stats;
  internal WorldObject world_object = new WorldObject();
  internal MapStats map_stats = new MapStats();
  internal WorldLaws world_laws;
  internal HotkeyTabsData hotkey_tabs_data;
  internal Canvas canvas;
  public static MapBox instance;
  internal PowerButtonSelector selected_buttons;
  internal ParallelOptions parallel_options;
  public Transform drag_parent;
  public static int width;
  public static int height;
  public static int current_world_seed_id;
  internal WorldTile[,] tiles_map;
  internal WorldTile[] tiles_list;
  public readonly List<BaseSystemManager> list_all_sim_managers = new List<BaseSystemManager>();
  private readonly List<BaseSystemManager> _list_meta_other_managers = new List<BaseSystemManager>();
  private readonly List<BaseSystemManager> _list_meta_main_managers = new List<BaseSystemManager>();
  private readonly List<BaseSystemManager> _list_sim_objects_managers = new List<BaseSystemManager>();
  public ProjectileManager projectiles;
  public StatusManager statuses;
  public CityManager cities;
  public WarManager wars;
  public PlotManager plots;
  public AllianceManager alliances;
  public ClanManager clans;
  public KingdomManager kingdoms;
  public WildKingdomsManager kingdoms_wild;
  public CultureManager cultures;
  public BookManager books;
  public SubspeciesManager subspecies;
  public ReligionManager religions;
  public LanguageManager languages;
  public FamilyManager families;
  public ArmyManager armies;
  public ItemManager items;
  public DiplomacyManager diplomacy;
  public BuildingManager buildings;
  public ActorManager units;
  public TileManager tile_manager;
  internal WorldTilemap tilemap;
  private float _redraw_timer;
  private bool _initiated;
  private DebugLayer _debug_layer;
  internal readonly RegionPathFinder region_path_finder = new RegionPathFinder();
  internal LoadingScreen transition_screen;
  internal StackEffects stack_effects;
  public DropManager drop_manager;
  internal PathFindingVisualiser path_finding_visualiser;
  internal WorldLayer world_layer;
  internal SpriteRenderer _world_layer_switch_effect;
  internal WorldLayerEdges world_layer_edges;
  internal UnitLayer unit_layer;
  internal GreyGooLayer grey_goo_layer;
  internal FireLayer fire_layer;
  private LavaLayer _lava_layer;
  internal PixelFlashEffects flash_effects;
  internal IslandsCalculator islands_calculator;
  internal ZoneCalculator zone_calculator;
  internal RoadsCalculator roads_calculator;
  internal BurnedTilesLayer burned_layer;
  internal ExplosionsEffects explosion_layer;
  internal ConwayLife conway_layer;
  internal MapChunkManager map_chunk_manager;
  internal AutoCivilization civilization_maker;
  private List<MapLayer> _map_layers;
  private List<BaseModule> _map_modules;
  internal Earthquake earthquake_manager;
  public Vector2 wind_direction;
  private StaticGrid _search_grid_ground;
  internal HashSet<WorldTile> tiles_dirty;
  internal GlowParticles particles_fire;
  internal GlowParticles particles_smoke;
  internal NameplateManager nameplate_manager;
  internal WorldBoxConsole.Console console;
  internal bool has_focus = true;
  internal Heat heat;
  internal HeatRayEffect heat_ray_fx;
  internal EffectDivineLight fx_divine_light;
  private MapBorder _map_border;
  internal QualityChanger quality_changer;
  internal Transform transform_units;
  internal SimObjectsZones sim_object_zones;
  internal Tutorial tutorial;
  private WorldLog _world_log;
  internal Magnet magnet;
  internal float timer_nutrition_decay;
  internal AutoTesterBot auto_tester;
  private UnitSelectionEffect _unit_select_effect;
  private readonly List<SpriteGroupSystem<GroupSpriteObject>> _list_systems = new List<SpriteGroupSystem<GroupSpriteObject>>();
  public static CursorSpeed cursor_speed;
  private DebugTextGroupSystem _debug_text_group_system;
  private SignalManager _signal_manager;
  internal ExplosionChecker explosion_checker;
  internal WorldAgeManager era_manager;
  public DelayedActionsManager delayed_actions_manager;
  public PlayerControl player_control;
  private bool _first_gen = true;
  private int _load_counter;
  private float _shake_timer;
  private float _shake_interval_timer;
  private float _shake_intensity = 1f;
  private float _shake_interval = 0.1f;
  private bool _shake_x = true;
  private bool _shake_y = true;
  private Transform _shake_camera;
  internal float elapsed;
  internal float delta_time;
  internal float fixed_delta_time;
  public readonly CityZoneHelper city_zone_helper = new CityZoneHelper();
  internal static Action on_world_loaded;
  private static int _tile_id;
  internal readonly AStarParam pathfinding_param = new AStarParam();
  internal int dirty_tiles_last;
  private bool _is_paused;
  private int _render_skip;
  private bool _meta_skip = true;
  private MetaTypeAsset _cached_map_meta_asset;
  private ArchitectMood _cached_architect_mood;

  internal LibraryMaterials library_materials => LibraryMaterials.instance;

  internal Camera camera { get; private set; }

  internal MoveCamera move_camera { get; private set; }

  internal ZoneCamera zone_camera { get; private set; }

  private void Awake()
  {
    MapBox.instance = this;
    this.player_control = new PlayerControl();
    this.parallel_options = new ParallelOptions()
    {
      CancellationToken = this.destroyCancellationToken
    };
    this.auto_tester = Object.FindFirstObjectByType<AutoTesterBot>((FindObjectsInactive) 1);
    this.save_manager = ((Component) this).GetComponentInChildren<SaveManager>();
    this.game_stats = ((Component) this).GetComponentInChildren<GameStats>();
    this.tilemap = ((Component) this).GetComponentInChildren<WorldTilemap>();
    this._map_border = ((Component) this).GetComponentInChildren<MapBorder>();
    this.stack_effects = ((Component) this).GetComponentInChildren<StackEffects>();
    this.resource_throw_manager = new ResourceThrowManager();
    this.heat_ray_fx = ((Component) this).GetComponentInChildren<HeatRayEffect>();
    this.fx_divine_light = ((Component) this).GetComponentInChildren<EffectDivineLight>();
    this.particles_fire = ((Component) ((Component) this).transform.Find("Particles Fire")).GetComponent<GlowParticles>();
    this.particles_smoke = ((Component) ((Component) this).transform.Find("Particles Smoke")).GetComponent<GlowParticles>();
    this._shake_camera = GameObject.Find("CameraShake").transform;
    Transform transform = GameObject.Find("Canvas Container Main").transform;
    this.canvas = ((Component) transform.FindRecursive("Canvas - UI/General")).GetComponent<Canvas>();
    this.transition_screen = ((Component) transform).GetComponentInChildren<LoadingScreen>(true);
    this.console = ((Component) transform).GetComponentInChildren<WorldBoxConsole.Console>(true);
    this.nameplate_manager = ((Component) transform).GetComponentInChildren<NameplateManager>(true);
    this.tutorial = ((Component) transform).GetComponentInChildren<Tutorial>(true);
    this.selected_buttons = ((Component) transform).GetComponentInChildren<PowerButtonSelector>();
    MapBox.cursor_speed = new CursorSpeed();
    this._signal_manager = new SignalManager();
    this.joys = GameObject.Find("Joys");
    this.joys.gameObject.SetActive(false);
    this.magnet = new Magnet();
    this.islands_calculator = new IslandsCalculator();
    this.sim_object_zones = new SimObjectsZones();
    this._world_log = new WorldLog();
    this.quality_changer = ((Component) this).GetComponent<QualityChanger>();
    this.transform_units = ((Component) this).transform.FindRecursive("Units");
    this.stack_effects.create();
    this.tiles_dirty = new HashSet<WorldTile>();
    this.tiles_list = new WorldTile[0];
    this.tile_manager = new TileManager();
    this.drop_manager = new DropManager(((Component) this).transform.Find("Drops"));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.subspecies = new SubspeciesManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.families = new FamilyManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.armies = new ArmyManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.languages = new LanguageManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.religions = new ReligionManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.cities = new CityManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.clans = new ClanManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.alliances = new AllianceManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.kingdoms = new KingdomManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.kingdoms_wild = new WildKingdomsManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.cultures = new CultureManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.plots = new PlotManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.wars = new WarManager()));
    this._list_meta_main_managers.Add((BaseSystemManager) (this.items = new ItemManager()));
    this._list_meta_other_managers.Add((BaseSystemManager) (this.books = new BookManager()));
    this._list_meta_other_managers.Add((BaseSystemManager) (this.diplomacy = new DiplomacyManager()));
    this._list_meta_other_managers.Add((BaseSystemManager) (this.projectiles = new ProjectileManager()));
    this._list_meta_other_managers.Add((BaseSystemManager) (this.statuses = new StatusManager()));
    this._list_sim_objects_managers.Add((BaseSystemManager) (this.units = new ActorManager()));
    this._list_sim_objects_managers.Add((BaseSystemManager) (this.buildings = new BuildingManager()));
    this.list_all_sim_managers.AddRange((IEnumerable<BaseSystemManager>) this._list_sim_objects_managers);
    this.list_all_sim_managers.AddRange((IEnumerable<BaseSystemManager>) this._list_meta_main_managers);
    this.list_all_sim_managers.AddRange((IEnumerable<BaseSystemManager>) this._list_meta_other_managers);
    this.heat = new Heat();
    this.wind_direction = new Vector2(-0.1f, 0.2f);
    this.era_manager = new WorldAgeManager();
    this.delayed_actions_manager = new DelayedActionsManager();
    AssetManager.world_behaviours.createManagers();
    ((Component) this).gameObject.AddOrGetComponent<MusicBox>();
    DOTween.SetTweensCapacity(1000, 100);
  }

  private void Start()
  {
    Application.lowMemory += new Application.LowMemoryCallback(AutoSaveManager.OnLowMemory);
    Application.lowMemory += new Application.LowMemoryCallback(PlayerConfig.turnOffAssetsPreloading);
    PlayerConfig.instance.start();
    this.explosion_checker = new ExplosionChecker();
    this.camera = Camera.main;
    this.move_camera = ((Component) this.camera).GetComponent<MoveCamera>();
    this._initiated = true;
    this._map_layers = new List<MapLayer>();
    this._map_modules = new List<BaseModule>();
    this._map_layers.Add((MapLayer) (this.world_layer = ((Component) this).GetComponentInChildren<WorldLayer>()));
    this._map_layers.Add((MapLayer) (this.world_layer_edges = ((Component) this).GetComponentInChildren<WorldLayerEdges>()));
    this._world_layer_switch_effect = ((Component) ((Component) this).gameObject.transform.Find("world_layer_back")).GetComponent<SpriteRenderer>();
    this._map_layers.Add((MapLayer) (this.unit_layer = ((Component) this).GetComponentInChildren<UnitLayer>()));
    this._map_layers.Add((MapLayer) (this.zone_calculator = ((Component) this).GetComponentInChildren<ZoneCalculator>()));
    this._map_layers.Add((MapLayer) (this.burned_layer = ((Component) this).GetComponentInChildren<BurnedTilesLayer>()));
    this._map_layers.Add((MapLayer) (this.explosion_layer = ((Component) this).GetComponentInChildren<ExplosionsEffects>()));
    this._map_layers.Add((MapLayer) (this.conway_layer = ((Component) this).GetComponentInChildren<ConwayLife>()));
    this._map_layers.Add((MapLayer) (this.fire_layer = ((Component) this).GetComponentInChildren<FireLayer>()));
    this._map_layers.Add((MapLayer) (this._lava_layer = ((Component) this).GetComponentInChildren<LavaLayer>()));
    this._map_layers.Add((MapLayer) (this._debug_layer = ((Component) this).GetComponentInChildren<DebugLayer>()));
    this._map_layers.Add((MapLayer) ((Component) this).GetComponentInChildren<DebugLayerCursor>());
    this._map_layers.Add((MapLayer) (this.path_finding_visualiser = ((Component) this).GetComponentInChildren<PathFindingVisualiser>()));
    this._map_layers.Add((MapLayer) (this.flash_effects = ((Component) this).GetComponentInChildren<PixelFlashEffects>()));
    this._map_modules.Add((BaseModule) (this.roads_calculator = ((Component) this).GetComponentInChildren<RoadsCalculator>()));
    this._map_modules.Add((BaseModule) (this.grey_goo_layer = ((Component) this).GetComponentInChildren<GreyGooLayer>()));
    this.map_chunk_manager = new MapChunkManager();
    this.zone_camera = new ZoneCamera();
    if (Config.isComputer || Config.isEditor)
    {
      this._unit_select_effect = Object.Instantiate<GameObject>((GameObject) Resources.Load("effects/PrefabUnitSelectionEffect"), ((Component) this).gameObject.transform).GetComponent<UnitSelectionEffect>();
      this._unit_select_effect.create();
    }
    this.addNewSystem((SpriteGroupSystem<GroupSpriteObject>) (this._debug_text_group_system = new GameObject().AddComponent<DebugTextGroupSystem>()));
    foreach (SpriteGroupSystem<GroupSpriteObject> listSystem in this._list_systems)
      listSystem.create();
  }

  private void addNewSystem(SpriteGroupSystem<GroupSpriteObject> pSystem)
  {
    this._list_systems.Add(pSystem);
    ((Component) pSystem).transform.parent = ((Component) this).transform;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isGameplayControlsLocked()
  {
    return ScrollWindow.isWindowActive() || ScrollWindow.isAnimationActive() || RewardedAds.isShowing();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isWindowOnScreen()
  {
    return ScrollWindow.isWindowActive() || ScrollWindow.isAnimationActive();
  }

  internal bool calcPath(Actor pActor, WorldTile pFrom, WorldTile pTargetTile, List<WorldTile> pSavePath)
  {
    pSavePath.Clear();
    StaticGrid searchGridGround = this._search_grid_ground;
    HeuristicMode iMode = HeuristicMode.MANHATTAN;
    float num1 = 2f;
    int num2;
    DiagonalMovement diagonalMovement;
    if (this.pathfinding_param.ocean)
    {
      num2 = 50;
      diagonalMovement = DiagonalMovement.OnlyWhenNoObstacles;
    }
    else
    {
      num2 = !this.pathfinding_param.limit ? -1 : 500;
      diagonalMovement = DiagonalMovement.Always;
    }
    int num3 = -1;
    if (this.pathfinding_param.roads)
    {
      num1 = 1f;
      diagonalMovement = DiagonalMovement.Never;
      iMode = HeuristicMode.EUCLIDEAN;
    }
    searchGridGround.Reset();
    if (!pFrom.isSameIsland(pTargetTile) && !this.pathfinding_param.ocean)
    {
      pSavePath.Add(pFrom);
      pSavePath.Add(pTargetTile);
      this.path_finding_visualiser.showPath(searchGridGround, pSavePath);
      return true;
    }
    // Check for army path restrictions if world law is enabled
    if (WorldLawLibrary.world_law_army_no_cross_foreign.isEnabled() && pActor != null && pActor.army != null)
    {
      Kingdom actorKingdom = pActor.kingdom;
      if (actorKingdom != null)
      {
        Alliance actorAlliance = actorKingdom.getAlliance();
        // Check if target tile is in foreign territory
        if (pTargetTile.zone != null && pTargetTile.zone.city != null)
        {
          Kingdom tileKingdom = pTargetTile.zone.city.kingdom;
          if (tileKingdom != null && tileKingdom != actorKingdom)
          {
            // Allow if allied or direct enemy in war
            bool isAllied = actorAlliance != null && actorAlliance.kingdoms_hashset.Contains(tileKingdom);
            bool isDirectEnemy = actorKingdom.isInWarWith(tileKingdom);
            if (!isAllied && !isDirectEnemy)
            {
              // Block path to foreign territory
              return false;
            }
          }
        }
      }
    }
    Vector2Int pos1 = pFrom.pos;
    int x1 = ((Vector2Int) ref pos1).x;
    Vector2Int pos2 = pFrom.pos;
    int y1 = ((Vector2Int) ref pos2).y;
    GridPos iStartPos = new GridPos(x1, y1);
    pos2 = pTargetTile.pos;
    int x2 = ((Vector2Int) ref pos2).x;
    pos2 = pTargetTile.pos;
    int y2 = ((Vector2Int) ref pos2).y;
    GridPos iEndPos = new GridPos(x2, y2);
    this.pathfinding_param.setGrid((BaseGrid) searchGridGround, iStartPos, iEndPos);
    this.pathfinding_param.DiagonalMovement = diagonalMovement;
    this.pathfinding_param.SetHeuristic(iMode);
    this.pathfinding_param.max_open_list = num3;
    this.pathfinding_param.weight = num1;
    AStarFinder.FindPath(this.pathfinding_param, pSavePath);
    this.path_finding_visualiser.showPath(searchGridGround, pSavePath);
    return pSavePath.Count != 0;
  }

  public void startTheGame(bool pForceGenerate = false)
  {
    LogText.log(nameof (MapBox), nameof (startTheGame), "st");
    Randy.fullReset();
    Config.game_loaded = true;
    Config.current_brush = "circ_5";
    if (Config.isMobile)
      PlayInterstitialAd.setActive(true);
    Config.LOAD_TIME_CREATE = Time.realtimeSinceStartup;
    if (pForceGenerate || Config.load_new_map)
      this.generateNewMap();
    else if (Config.load_random_test_map)
      TestMaps.loadNextMap();
    else if (Config.load_dragon)
      SaveManager.loadMapFromResources("mapTemplates/map_dragon");
    else if (Config.load_save_on_start)
    {
      this._first_gen = false;
      this.save_manager.loadWorld(SaveManager.getSlotSavePath(Config.load_save_on_start_slot));
    }
    else if (Config.load_save_from_path)
      SaveManager.loadMapFromResources(Config.load_test_save_path);
    else if (Config.load_test_map)
    {
      DebugMap.makeDebugMap();
    }
    else
    {
      string str;
      try
      {
        str = DateTime.Now.ToString("MM/dd");
      }
      catch (Exception ex)
      {
        str = "";
      }
      if (str == "04/01")
        SaveManager.loadMapFromResources("mapTemplates/map_april_fools");
      else if (FavoriteWorld.hasFavoriteWorldSet())
      {
        int favoriteWorld = PlayerConfig.instance.data.favorite_world;
        FavoriteWorld.cacheSaveSlotID(favoriteWorld);
        FavoriteWorld.clearFavoriteWorld();
        this._first_gen = false;
        this.save_manager.loadWorld(SaveManager.getSlotSavePath(favoriteWorld));
      }
      else if (this.game_stats.data.gameLaunches <= 3L)
      {
        SaveManager.loadMapFromResources("mapTemplates/map_dragon");
      }
      else
      {
        this.generateNewMap();
        SmoothLoader.add((MapLoaderAction) (() => this.buildings.addBuilding("volcano", this.GetTile(MapBox.width / 2, MapBox.height / 2))), "add_volcano");
        SmoothLoader.add((MapLoaderAction) (() =>
        {
          WorldTile tile1 = this.GetTile(0, MapBox.height - 1);
          WorldTile tile2 = this.GetTile(MapBox.width - 1, MapBox.height - 1);
          WorldTile tile3 = this.GetTile(0, 0);
          WorldTile tile4 = this.GetTile(MapBox.width - 1, 0);
          this.units.spawnNewUnit("angle", tile1, pMiracleSpawn: true).setName("DAB", false);
          this.units.spawnNewUnit("angle", tile2, pMiracleSpawn: true).setName("ABC", false);
          this.units.spawnNewUnit("angle", tile3, pMiracleSpawn: true).setName("CDA", false);
          this.units.spawnNewUnit("angle", tile4, pMiracleSpawn: true).setName("BCD", false);
        }), "spawn_angles");
      }
    }
    SmoothLoader.add(new MapLoaderAction(this.addLastStep), "Prepare Game Launch");
  }

  private void addLastStep()
  {
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      Config.LOAD_TIME_GENERATE = Time.realtimeSinceStartup;
      ((Renderer) ((Component) this).GetComponent<SpriteRenderer>()).enabled = true;
      ((Component) this.nameplate_manager).gameObject.SetActive(true);
      FavoriteWorld.restoreCachedFavoriteWorldOnSuccess();
      if (!Config.disable_startup_window)
      {
        if (PlayerConfig.instance.data.tutorialFinished || Config.disable_tutorial)
          ScrollWindow.get("welcome").forceShow();
        else
          this.tutorial.startTutorial();
      }
      PremiumElementsChecker.checkElements();
      MonoBehaviour.print((object) ("LOAD TIME INIT: " + Config.LOAD_TIME_INIT.ToString()));
      float num = Config.LOAD_TIME_CREATE - Config.LOAD_TIME_INIT;
      MonoBehaviour.print((object) ("LOAD TIME CREATE: " + num.ToString()));
      num = Config.LOAD_TIME_GENERATE - Config.LOAD_TIME_CREATE;
      MonoBehaviour.print((object) ("LOAD TIME GENERATE: " + num.ToString()));
      LogText.log(nameof (MapBox), "startTheGame", "en");
    }), "Start the Game", pToEnd: true);
  }

  private void afterLoadEvent()
  {
    Debug.Log((object) "afterLoadEvent--------------------------");
    PremiumElementsChecker.checkElements();
  }

  internal void centerCamera()
  {
    Vector3 position = ((Component) this.camera).transform.position;
    position.x = (float) (MapBox.width / 2);
    position.y = (float) (MapBox.height / 2);
    ((Component) this.camera).transform.position = position;
    this.move_camera.resetZoom();
  }

  private void resetTiles()
  {
    this._search_grid_ground?.Reset();
    foreach (WorldTile tiles in this.tiles_list)
      tiles.clear();
    this.tiles_dirty.Clear();
    this.tilemap.clear();
  }

  private void clearTiles()
  {
    this._search_grid_ground?.Dispose();
    this._search_grid_ground = (StaticGrid) null;
    this.zone_calculator.clean();
    this.map_chunk_manager.clean();
    foreach (WorldTile tiles in this.tiles_list)
      tiles.Dispose();
    this.tiles_list = new WorldTile[0];
    for (int index1 = 0; index1 < MapBox.width; ++index1)
    {
      for (int index2 = 0; index2 < MapBox.height; ++index2)
        this.tiles_map[index1, index2] = (WorldTile) null;
    }
    this.tiles_map = (WorldTile[,]) null;
    this.tiles_dirty.Clear();
    this.tilemap.clear();
  }

  private void createTiles()
  {
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      this.tiles_list = new WorldTile[MapBox.width * MapBox.height];
      this.tiles_map = new WorldTile[MapBox.width, MapBox.height];
      GeneratorTool.Setup(this.tiles_map);
    }), "Prepare Tiles");
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      MapBox._tile_id = 0;
      for (int index1 = 0; index1 < MapBox.height; ++index1)
      {
        for (int index2 = 0; index2 < MapBox.width; ++index2)
        {
          WorldTile pTile = new WorldTile(index2, index1, MapBox._tile_id);
          this._search_grid_ground.SetTileNode(index2, index1, pTile);
          this.tiles_map[index2, index1] = pTile;
          this.tiles_list[MapBox._tile_id] = pTile;
          ++MapBox._tile_id;
        }
      }
    }), $"Create Tiles ({(MapBox.height * MapBox.width).ToString()})", true);
    int length1;
    SmoothLoader.add((MapLoaderAction) (() => length1 = this.tiles_list.Length), $"Create Neighbours [{(MapBox.height * MapBox.width).ToString()}] (1/3)", true);
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      int length2 = this.tiles_list.Length;
      for (int index = 0; index < length2; ++index)
        this.tiles_list[index].resetNeighbourLists();
    }), $"Create Neighbours [{(MapBox.height * MapBox.width).ToString()}] (2/3)", true);
    SmoothLoader.add((MapLoaderAction) (() => GeneratorTool.GenerateTileNeighbours(this.tiles_list)), $"Create Neighbours [{(MapBox.height * MapBox.width).ToString()}] (3/3)", true);
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      this.zone_calculator.generate();
      WorldBehaviourActionFire.prepare();
    }), "Create Zones", true);
    SmoothLoader.add((MapLoaderAction) (() => this.map_chunk_manager.prepare()), "Create Chunks", true);
  }

  public static AttackDataResult newAttack(AttackData pData)
  {
    if (pData.hit_tile == null)
      return new AttackDataResult(ApplyAttackState.Continue);
    int targets = pData.targets;
    AttackDataResult attackDataResult = new AttackDataResult(ApplyAttackState.Continue);
    if (pData.target != null)
    {
      attackDataResult = MapBox.checkAttackFor(pData, pData.target);
      switch (attackDataResult.state)
      {
        case ApplyAttackState.Hit:
          --targets;
          break;
        case ApplyAttackState.Block:
        case ApplyAttackState.Deflect:
          return attackDataResult;
      }
      if (targets == 0)
        return attackDataResult;
    }
    if (targets == 0)
      return attackDataResult;
    List<BaseSimObject> list = EnemiesFinder.findEnemiesFrom(pData.hit_tile, pData.kingdom, 0).list;
    if (list == null)
      return attackDataResult;
    foreach (BaseSimObject pTargetToCheck in list.LoopRandom<BaseSimObject>())
    {
      if (pTargetToCheck != pData.target)
      {
        if (targets != 0)
        {
          attackDataResult = MapBox.checkAttackFor(pData, pTargetToCheck);
          switch (attackDataResult.state)
          {
            case ApplyAttackState.Hit:
              --targets;
              continue;
            case ApplyAttackState.Block:
            case ApplyAttackState.Deflect:
              return attackDataResult;
            default:
              continue;
          }
        }
        else
          break;
      }
    }
    return attackDataResult;
  }

  public static AttackDataResult checkAttackFor(AttackData pData, BaseSimObject pTargetToCheck)
  {
    if (pTargetToCheck.isRekt() || pData.initiator.isRekt() || pTargetToCheck == pData.initiator || !pData.initiator.canAttackTarget(pTargetToCheck) || pTargetToCheck.isActor() && pTargetToCheck.hasStatus("dodge"))
      return AttackDataResult.Continue;
    Vector3 vector3 = Vector2.op_Implicit(pTargetToCheck.current_position);
    double num1 = (double) Toolbox.SquaredDist(vector3.x, vector3.y + pTargetToCheck.getHeight(), pData.hit_position.x, pData.hit_position.y + pData.hit_position.z);
    float num2 = pData.area_of_effect + pTargetToCheck.stats["size"];
    double num3 = (double) (num2 * num2);
    if (num1 >= num3)
      return AttackDataResult.Miss;
    Vector3.MoveTowards(pData.hit_position, vector3, pTargetToCheck.stats["size"] * 0.9f).y += pTargetToCheck.getHeight();
    AttackDataResult attackDataResult = MapBox.applyAttack(pData, pTargetToCheck);
    if (attackDataResult.state != ApplyAttackState.Hit)
      return attackDataResult;
    Vector3 hitPosition = pData.hit_position;
    hitPosition.y += hitPosition.z;
    if (pData.critical)
    {
      EffectsLibrary.spawnAt("fx_hit_critical", hitPosition, 0.1f);
      return attackDataResult;
    }
    EffectsLibrary.spawnAt("fx_hit", hitPosition, 0.1f);
    return attackDataResult;
  }

  private static AttackDataResult applyAttack(AttackData pData, BaseSimObject pTargetToCheck)
  {
    bool flag1 = pTargetToCheck.isActor();
    Actor a = pTargetToCheck.a;
    ProjectileAsset projectileAsset = (ProjectileAsset) null;
    if (pData.is_projectile)
      projectileAsset = AssetManager.projectiles.get(pData.projectile_id);
    if (flag1 && ControllableUnit.isControllingUnit(a) && a.hasMeleeAttack() && a.isJustAttacked())
    {
      int num = CombatActionLibrary.combat_action_deflect.action_actor(pTargetToCheck.a, pData) ? 1 : 0;
      return new AttackDataResult(ApplyAttackState.Deflect, pTargetToCheck.a.data.id);
    }
    CombatActionAsset pResultCombatAsset;
    if (flag1 && a.tryToUseAdvancedCombatAction(a.getCombatActionPool(CombatActionPool.BEFORE_HIT_DEFLECT), (BaseSimObject) null, out pResultCombatAsset))
    {
      int num = pResultCombatAsset.action_actor(pTargetToCheck.a, pData) ? 1 : 0;
      return new AttackDataResult(ApplyAttackState.Deflect, pTargetToCheck.a.data.id);
    }
    bool flag2 = false;
    if (projectileAsset != null && projectileAsset.can_be_blocked)
      flag2 = true;
    if (flag2 && flag1 && a.tryToUseAdvancedCombatAction(a.getCombatActionPool(CombatActionPool.BEFORE_HIT_BLOCK), (BaseSimObject) null, out pResultCombatAsset))
    {
      int num = pResultCombatAsset.action_actor(pTargetToCheck.a, pData) ? 1 : 0;
      return AttackDataResult.Block;
    }
    if (flag1 && a.tryToUseAdvancedCombatAction(a.getCombatActionPool(CombatActionPool.BEFORE_HIT), (BaseSimObject) null, out pResultCombatAsset))
    {
      int num = pResultCombatAsset.action_actor(a, pData) ? 1 : 0;
      return AttackDataResult.Continue;
    }
    int num1 = (int) Randy.randomFloat(pData.damage_range * (float) pData.damage, (float) pData.damage);
    if (pData.critical)
      num1 *= pData.critical_damage_multiplier;
    if (pData.initiator.isActor() && pTargetToCheck.isAlive())
      pData.initiator.a.addExperience(2);
    BaseSimObject baseSimObject = pTargetToCheck;
    double pDamage = (double) num1;
    int attackType = (int) pData.attack_type;
    BaseSimObject initiator = pData.initiator;
    bool metallicWeapon = pData.metallic_weapon;
    int num2 = pData.skip_shake ? 1 : 0;
    int num3 = metallicWeapon ? 1 : 0;
    baseSimObject.getHit((float) pDamage, pAttackType: (AttackType) attackType, pAttacker: initiator, pSkipIfShake: num2 != 0, pMetallicWeapon: num3 != 0);
    if (!pTargetToCheck.hasHealth())
      ActorTool.applyForceToUnit(pData, pTargetToCheck);
    else
      ActorTool.applyForceToUnit(pData, pTargetToCheck, 0.5f, true);
    if (pData.initiator.isActor())
      pData.initiator.a.attackTargetActions(pTargetToCheck, pData.hit_tile);
    if (flag1 && pData.initiator.isActor() && !pTargetToCheck.hasHealth() && pData.initiator.a.needsFood() && pData.initiator.a.subspecies.diet_meat && a.asset.source_meat)
    {
      pData.initiator.a.addNutritionFromEating(70, true);
      pData.initiator.a.countConsumed();
    }
    return AttackDataResult.Hit;
  }

  public void clearArchitectMood() => this._cached_architect_mood = (ArchitectMood) null;

  public void clearWorld()
  {
    this.clearArchitectMood();
    ++MapBox.current_world_seed_id;
    DBInserter.Lock();
    this.tile_manager.clear();
    CursedSacrifice.reset();
    LogText.log(nameof (MapBox), nameof (clearWorld), "st");
    this.auto_tester?.clearWorld();
    Analytics.worldLoading();
    SelectedUnit.clear();
    ControllableUnit.clear();
    DBManager.clearAndClose();
    this.explosion_checker.clear();
    BattleKeeperManager.clear();
    ZoneMetaDataVisualizer.clearAll();
    Finder.clear();
    this._debug_layer.clear();
    this.selected_buttons.unselectAll();
    this.player_control.clear();
    MusicBox.clearAllSounds();
    this.clearFrameCaches();
    EnemiesFinder.disposeAll();
    TaxiManager.clear();
    this.islands_calculator.clear();
    RegionLinkHashes.clear();
    this.nameplate_manager.clearAll();
    this.map_chunk_manager.clearAll();
    this.islands_calculator.clear();
    this.quality_changer.reset();
    this.tilemap.clear();
    this.zone_camera.clear();
    Config.paused = false;
    if (DebugConfig.isOn(DebugOption.PauseOnStart))
      Config.paused = true;
    this.selected_buttons.checkToggleIcons();
    this.heat.clear();
    this.era_manager.clear();
    this.delayed_actions_manager.clear();
    foreach (TileTypeBase tileTypeBase in AssetManager.tiles.list)
      tileTypeBase.hashsetClear();
    foreach (TileTypeBase tileTypeBase in AssetManager.top_tiles.list)
      tileTypeBase.hashsetClear();
    foreach (WorldBehaviourAsset worldBehaviourAsset in AssetManager.world_behaviours.list)
      worldBehaviourAsset.manager.clear();
    WildKingdomsManager.neutral.clearListCities();
    AutoSaveManager.resetAutoSaveTimer();
    AssetManager.actor_library.clear();
    AssetManager.buildings.clear();
    BehaviourActionActor.clear();
    this.city_zone_helper.clear();
    this.region_path_finder.clear();
    Toolbox.clearAll();
    this.drop_manager.clear();
    this.armies.clear();
    foreach (BaseSystemManager listAllSimManager in this.list_all_sim_managers)
      listAllSimManager.clear();
    this.particles_fire.clear();
    this.particles_smoke.clear();
    this.stack_effects.clear();
    TornadoEffect.Clear();
    this.resource_throw_manager.clear();
    foreach (MapLayer mapLayer in this._map_layers)
      mapLayer.clear();
    foreach (BaseModule mapModule in this._map_modules)
      mapModule.clear();
    this.sim_object_zones.fullClear();
    this.resetTiles();
    this.zone_camera.fullClear();
    this.world_layer_edges.clear();
    WorldBehaviourActionFire.clearFires();
    HistoryHud.instance.Clear();
    DBInserter.Unlock();
    DBManager.clearAndClose();
    LogText.log(nameof (MapBox), nameof (clearWorld), "en");
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public WorldTile GetTile(int pX, int pY)
  {
    return pX < 0 || pX >= MapBox.width || pY < 0 || pY >= MapBox.height ? (WorldTile) null : this.tiles_map[pX, pY];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public WorldTile GetTileSimple(int pX, int pY) => this.tiles_map[pX, pY];

  public void setMapSize(int pWidth, int pHeight)
  {
    Config.ZONE_AMOUNT_X = pWidth;
    Config.ZONE_AMOUNT_Y = pHeight;
    MapBox.width = Config.ZONE_AMOUNT_X * 64 /*0x40*/;
    MapBox.height = Config.ZONE_AMOUNT_Y * 64 /*0x40*/;
    if (this.tiles_list.Length == MapBox.width * MapBox.height)
      return;
    this.recreateSizes();
  }

  private void afterTransitionGeneration() => this.generateNewMap();

  public void clickGenerateNewMap()
  {
    this.transition_screen.startTransition(new LoadingScreen.TransitionAction(this.afterTransitionGeneration));
  }

  public void generateNewMap()
  {
    if (!this._initiated)
      return;
    if (Config.show_console_on_start)
      this.console.Toggle();
    SmoothLoader.prepare();
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      LogText.log(nameof (MapBox), nameof (generateNewMap), "st");
      Analytics.worldLoading();
      if (this._first_gen)
        Config.customMapSize = Config.customMapSizeDefault;
      if (!this._first_gen)
        AchievementLibrary.custom_world.check();
      this._first_gen = false;
      int size = MapSizeLibrary.getSize(Config.customMapSize);
      this.addClearWorld(size, size);
    }), "Generate New Map (1/3)");
    SmoothLoader.add((MapLoaderAction) (() => Config.ZONE_AMOUNT_Y = Config.ZONE_AMOUNT_X = MapSizeLibrary.getSize(Config.customMapSize)), "Generate New Map (2/3)");
    SmoothLoader.add((MapLoaderAction) (() => this.setMapSize(Config.ZONE_AMOUNT_X, Config.ZONE_AMOUNT_Y)), "Generate New Map (3/3)");
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      LogText.log(nameof (MapBox), "GenerateMap", "st");
      AssetManager.tiles.setListTo(DepthGeneratorType.Generator);
      this.world_laws = new WorldLaws();
      this.world_laws.init();
      this.hotkey_tabs_data = new HotkeyTabsData();
    }), "gen: World Laws");
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      this.map_stats = new MapStats();
      this.map_stats.initNewWorld();
      Randy.resetSeed(Randy.randomInt(1, 555555555));
    }), "gen: Generating Name");
    if (!Config.disable_db)
    {
      SmoothLoader.add((MapLoaderAction) (() => DBManager.createDB()), "Creating Stats DB");
      DBTables.createOrMigrateTablesLoader();
    }
    WindowPreloader.addWaitForWindowResources();
    SmoothLoader.add((MapLoaderAction) (() => this.era_manager.setDefaultAges()), "gen: World Ages");
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      MapGenerator.prepare();
      LogText.log(nameof (MapBox), "GenerateMap", "en");
    }), "Preparing Map", true);
    SmoothLoader.add((MapLoaderAction) (() => this.cleanUpWorld()), "Cleaning Up The World", true);
    SmoothLoader.add((MapLoaderAction) (() => this.redrawTiles()), "Drawing Up The World", true);
    SmoothLoader.add((MapLoaderAction) (() => this.preloadRenderedSprites()), "Preload rendered sprites...", pNewWaitTimerValue: 0.2f);
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      this.finishMakingWorld();
      LogText.log(nameof (MapBox), nameof (generateNewMap), "en");
    }), "Tidying Up The World", true);
    SmoothLoader.add((MapLoaderAction) (() => this.lastGC()), "Rewriting The World", true);
    this.addLoadAutoTester();
    this.addKillAllUnits();
    this.addLoadWorldCallbacks();
    SmoothLoader.add((MapLoaderAction) (() => this.finishingUpLoading()), "Finishing up...", pNewWaitTimerValue: 0.2f);
  }

  public void finishingUpLoading() => CanvasMain.instance.setMainUiEnabled(true);

  public void preloadRenderedSprites()
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) this.units)
      unit.checkSpriteToRender();
    foreach (Building building in (SimSystemManager<Building, BuildingData>) this.buildings)
      building.checkSpriteToRender();
  }

  public void addUnloadResources()
  {
    ++this._load_counter;
    if (this._load_counter <= 5)
      return;
    this._load_counter = 0;
    SmoothLoader.add((MapLoaderAction) (() => Resources.UnloadUnusedAssets()), "UnloadUnusedAssets", true);
  }

  public void addClearWorld(int pNextWidth, int pNextHeight)
  {
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      LogText.log(nameof (MapBox), "clearWorld", "st");
      this.clearWorld();
      LogText.log(nameof (MapBox), "clearWorld", "en");
    }), "Clearing World", true);
    DebugMemory.addMemorySnapshot("clearWorld");
    if (this.tiles_list.Length == pNextWidth * 64 /*0x40*/ * (pNextHeight * 64 /*0x40*/))
      return;
    SmoothLoader.add((MapLoaderAction) (() => this.clearTiles()), "Clean old Tiles");
    DebugMemory.addMemorySnapshot("clearTiles");
  }

  public void addKillAllUnits()
  {
    if (!DebugConfig.isOn(DebugOption.KillAllUnitsOnLoad))
      return;
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) this.units)
        unit.dieAndDestroy(AttackType.None);
    }), "Killing All Units", true);
  }

  public void addLoadAutoTester()
  {
    if (!DebugConfig.isOn(DebugOption.TesterLibs))
      return;
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      if (string.IsNullOrEmpty(Config.auto_test_on_start))
        return;
      this.auto_tester.create(Config.auto_test_on_start);
      ((Component) this.auto_tester).gameObject.SetActive(true);
    }), "Loading Auto Tester", true);
  }

  public void addLoadWorldCallbacks()
  {
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      ++Config.debug_worlds_loaded;
      Action onWorldLoaded = MapBox.on_world_loaded;
      if (onWorldLoaded == null)
        return;
      onWorldLoaded();
    }), "World Loaded", true);
  }

  private void generateMap(string pType = "islands")
  {
  }

  public void cleanUpWorld(bool pSetChunksDirty = true)
  {
    MapGenerator.clear();
    this.updateDirtyMetaContainersAndCleanup();
    this.era_manager.prepare();
    if (pSetChunksDirty)
    {
      this.map_chunk_manager.allDirty();
      this.map_chunk_manager.update(0.0f, true);
    }
    foreach (City city in (CoreSystemManager<City, CityData>) this.cities)
      city.forceDoChecks();
    foreach (City city in (CoreSystemManager<City, CityData>) this.cities)
      city.executeAllActionsForCity();
    this.allTilesDirty();
    this.centerCamera();
  }

  public void redrawTiles()
  {
    this._meta_skip = true;
    if (MusicBox.new_world_on_start_played)
      MusicBox.reserveFlag("new_world");
    this.tilemap.redrawTiles(true);
  }

  public void finishMakingWorld()
  {
    ToolbarButtons.instance?.resetBar();
    ++this.game_stats.data.mapsCreated;
    AchievementLibrary.gen_5_worlds.check();
    AchievementLibrary.gen_50_worlds.check();
    AchievementLibrary.gen_100_worlds.check();
    Analytics.worldLoaded();
    Config.LAST_LOAD_TIME = Time.realtimeSinceStartup;
  }

  public void lastGC() => Config.forceGC("finish making world");

  private void recreateSizes()
  {
    SmoothLoader.add((MapLoaderAction) (() => this._search_grid_ground = new StaticGrid(MapBox.width, MapBox.height)), "Recreate Sizes (1/4)");
    SmoothLoader.add((MapLoaderAction) (() => this.createTiles()), "Recreate Sizes (2/4)");
    SmoothLoader.add((MapLoaderAction) (() => this.tile_manager.setup(MapBox.width, MapBox.height, this.tiles_map)), "Tile Manager", true);
    for (int index = 0; index < this._map_layers.Count; ++index)
    {
      int j = index;
      SmoothLoader.add((MapLoaderAction) (() => this._map_layers[j].createTextureNew()), $"Recreate Sizes (3/4) ({(index + 1).ToString()}/{this._map_layers.Count.ToString()})");
    }
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      if (Globals.TRAILER_MODE)
        Object.Destroy((Object) ((Component) this._map_border).gameObject);
      else
        this._map_border.generateTexture();
    }), "Recreate Sizes (4/4)");
  }

  public Actor getActorNearCursor() => ActionLibrary.getActorNearPos(MapBox.instance.getMousePos());

  public WorldTile getMouseTilePosCachedFrame() => this.player_control.getMouseTilePosCachedFrame();

  public Vector2 getMousePos() => this.player_control.getMousePos();

  public WorldTile getMouseTilePos() => this.player_control.getMouseTilePos();

  public bool isPointerInGame() => this.player_control.isPointerInGame();

  public bool isPointerInsideMapBounds() => this.getMouseTilePos() != null;

  public bool isOverUI() => this.player_control.isOverUI();

  public bool isTouchOverUI(Touch pTouch) => this.player_control.isTouchOverUI(pTouch);

  public static bool controlsLocked() => PlayerControl.controlsLocked();

  public static bool isControllingUnit() => PlayerControl.isControllingUnit();

  public bool isBusyWithUI() => this.player_control.isBusyWithUI();

  public bool isActionHappening() => this.player_control.isActionHappening();

  public bool isOverUiButton()
  {
    PlayerControl playerControl = this.player_control;
    return playerControl != null && playerControl.isPointerOverUIButton();
  }

  public void loopWithBrush(
    WorldTile pCenterTile,
    BrushData pBrush,
    PowerAction pAction,
    GodPower pPower = null)
  {
    BrushPixelData[] pos = pBrush.pos;
    int length = pos.Length;
    for (int index = 0; index < length; ++index)
    {
      BrushPixelData brushPixelData = pos[index];
      int pX = pCenterTile.x + brushPixelData.x;
      int pY = pCenterTile.y + brushPixelData.y;
      if (pX >= 0 && pX < MapBox.width && pY >= 0 && pY < MapBox.height)
      {
        WorldTile tileSimple = MapBox.instance.GetTileSimple(pX, pY);
        int num = pAction(tileSimple, pPower) ? 1 : 0;
      }
    }
  }

  public void highlightTilesBrush(
    WorldTile pCenterTile,
    BrushData pBrush,
    PowerAction pAction,
    GodPower pPower = null)
  {
    this.loopWithBrush(pCenterTile, pBrush, pAction, pPower);
  }

  public void loopWithBrushPowerForDropsFull(
    WorldTile pCenterTile,
    BrushData pBrush,
    PowerAction pAction,
    GodPower pPower = null)
  {
    this.loopWithBrush(pCenterTile, pBrush, pAction, pPower);
  }

  public void loopWithBrushPowerForDropsRandom(
    WorldTile pCenterTile,
    BrushData pBrush,
    PowerAction pAction,
    GodPower pPower = null)
  {
    BrushPixelData[] pos = pBrush.pos;
    int length = pos.Length;
    using (ListPool<WorldTile> list = new ListPool<WorldTile>())
    {
      for (int index = 0; index < length; ++index)
      {
        BrushPixelData brushPixelData = pos[index];
        int pX = pCenterTile.x + brushPixelData.x;
        int pY = pCenterTile.y + brushPixelData.y;
        if (pX >= 0 && pX < MapBox.width && pY >= 0 && pY < MapBox.height)
        {
          WorldTile tileSimple = MapBox.instance.GetTileSimple(pX, pY);
          list.Add(tileSimple);
        }
      }
      int drops = pBrush.drops;
      list.Shuffle<WorldTile>();
      for (int index = 0; index < drops && list.Count != 0; ++index)
      {
        WorldTile pTile = list.Pop<WorldTile>();
        int num = pAction(pTile, pPower) ? 1 : 0;
      }
    }
  }

  public void loopWithBrush(
    WorldTile pCenterTile,
    BrushData pBrush,
    PowerActionWithID pAction,
    string pPowerID = null)
  {
    BrushPixelData[] pos = pBrush.pos;
    int length = pos.Length;
    for (int index = 0; index < length; ++index)
    {
      BrushPixelData brushPixelData = pos[index];
      int pX = pCenterTile.x + brushPixelData.x;
      int pY = pCenterTile.y + brushPixelData.y;
      if (pX >= 0 && pX < MapBox.width && pY >= 0 && pY < MapBox.height)
      {
        WorldTile tileSimple = MapBox.instance.GetTileSimple(pX, pY);
        int num = pAction(tileSimple, pPowerID) ? 1 : 0;
      }
    }
  }

  public void checkCityZone(WorldTile pTile)
  {
    if (pTile.zone.city == null)
      return;
    bool flag = false;
    HashSet<Building> hashset = pTile.zone.getHashset(BuildingList.Civs);
    if (hashset != null)
    {
      foreach (Building building in hashset)
      {
        if (building.city == pTile.zone.city)
        {
          flag = true;
          break;
        }
      }
    }
    if (flag)
      return;
    pTile.zone.city.removeZone(pTile.zone);
  }

  public static void spawnLightningBig(WorldTile pTile, float pScale = 0.25f, Actor pActor = null)
  {
    BaseEffect baseEffect = EffectsLibrary.spawnAtTile("fx_lightning_big", pTile, pScale);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return;
    int pRad = (int) ((double) pScale * 25.0);
    MapAction.checkLightningAction(pTile.pos, pRad, pActor, true, true);
    MapAction.damageWorld(pTile, pRad, AssetManager.terraform.get("lightning_power"), (BaseSimObject) pActor);
    baseEffect.sprite_renderer.flipX = Randy.randomBool();
    MapAction.checkSantaHit(pTile.pos, pRad);
    MapAction.checkUFOHit(pTile.pos, pRad, pActor);
    MapAction.checkTornadoHit(pTile.pos, pRad);
  }

  public static void spawnLightningMedium(WorldTile pTile, float pScale = 0.25f, Actor pActor = null)
  {
    BaseEffect baseEffect = EffectsLibrary.spawnAtTile("fx_lightning_medium", pTile, pScale);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return;
    int pRad = (int) ((double) pScale * 15.0);
    MapAction.checkLightningAction(pTile.pos, pRad, pActor);
    MapAction.damageWorld(pTile, pRad, AssetManager.terraform.get("lightning_normal"), (BaseSimObject) pActor);
    baseEffect.sprite_renderer.flipX = Randy.randomBool();
    MapAction.checkTornadoHit(pTile.pos, pRad);
  }

  public static void spawnLightningSmall(WorldTile pTile, float pScale = 0.25f, Actor pActor = null)
  {
    BaseEffect baseEffect = EffectsLibrary.spawnAtTile("fx_lightning_small", pTile, pScale);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return;
    int pRad = (int) ((double) pScale * 10.0);
    MapAction.checkLightningAction(pTile.pos, pRad, pActor);
    MapAction.damageWorld(pTile, pRad, AssetManager.terraform.get("lightning_normal"), (BaseSimObject) pActor);
    baseEffect.sprite_renderer.flipX = Randy.randomBool();
    MapAction.checkTornadoHit(pTile.pos, pRad);
  }

  public void applyForceOnTile(
    WorldTile pTile,
    int pRad = 10,
    float pForceAmount = 1.5f,
    bool pForceOut = true,
    int pDamage = 0,
    string[] pIgnoreKingdoms = null,
    BaseSimObject pByWho = null,
    TerraformOptions pOptions = null,
    bool pChangeHappiness = false)
  {
    int num = pRad * pRad;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1))
    {
      if (actor != pByWho?.a && (double) Toolbox.SquaredDistTile(actor.current_tile, pTile) <= (double) num && (pOptions == null || !actor.asset.very_high_flyer || pOptions.applies_to_high_flyers))
      {
        if (pIgnoreKingdoms != null)
        {
          bool flag = false;
          for (int index = 0; index < pIgnoreKingdoms.Length; ++index)
          {
            Kingdom kingdom = this.kingdoms_wild.get(pIgnoreKingdoms[index]);
            if (actor.kingdom == kingdom)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            continue;
        }
        actor.makeStunned(4f);
        if (pChangeHappiness)
          actor.changeHappiness("just_forced_power");
        if (actor.asset.can_be_hurt_by_powers && pDamage > 0)
        {
          AttackType pAttackType = AttackType.Other;
          if (pOptions != null)
            pAttackType = pOptions.attack_type;
          actor.getHit((float) pDamage, pAttackType: pAttackType, pAttacker: pByWho);
        }
        if ((double) pForceAmount > 0.0)
        {
          if (pForceOut)
            actor.calculateForce((float) actor.current_tile.x, (float) actor.current_tile.y, (float) pTile.x, (float) pTile.y, pForceAmount, pCheckCancelJobOnLand: true);
          else
            actor.calculateForce((float) pTile.x, (float) pTile.y, (float) actor.current_tile.x, (float) actor.current_tile.y, pForceAmount, pCheckCancelJobOnLand: true);
        }
      }
    }
  }

  internal void stopAttacksFor(bool pMonsters)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) this.units)
    {
      if (unit.has_attack_target && unit.isEnemyTargetAlive() && (unit.kingdom.asset.mobs ? 1 : (unit.attack_target.kingdom.asset.mobs ? 1 : 0)) == (pMonsters ? 1 : 0))
        unit.cancelAllBeh();
    }
  }

  public void allDirty()
  {
    for (int index = 0; index < this.tiles_list.Length; ++index)
    {
      WorldTile tiles = this.tiles_list[index];
      this.tiles_dirty.Add(tiles);
      this.tilemap.addToQueueToRedraw(tiles);
    }
  }

  private void OnApplicationFocus(bool pFocus) => this.has_focus = pFocus;

  private void OnApplicationPause(bool pPause) => this.has_focus = !pPause;

  private void OnApplicationQuit() => DOTween.KillAll(false);

  private void updateShake(float pElapsed)
  {
    if ((double) this._shake_timer == 0.0)
      return;
    if ((double) this._shake_timer > 0.0)
      this._shake_timer -= pElapsed;
    if ((double) this._shake_timer <= 0.0)
    {
      this._shake_timer = 0.0f;
      this._shake_camera.position = new Vector3(0.0f, 0.0f);
    }
    else if ((double) this._shake_interval_timer > 0.0)
    {
      this._shake_interval_timer -= pElapsed;
    }
    else
    {
      this._shake_interval_timer = this._shake_interval;
      Vector3 vector3 = new Vector3();
      if (this._shake_x)
        vector3.x = Randy.randomFloat(-this._shake_intensity, this._shake_intensity);
      if (this._shake_y)
        vector3.y = Randy.randomFloat(-this._shake_intensity, this._shake_intensity);
      this._shake_camera.position = vector3;
    }
  }

  public void startShake(
    float pDuration = 0.3f,
    float pInterval = 0.01f,
    float pIntensity = 2f,
    bool pShakeX = false,
    bool pShakeY = true)
  {
    this._shake_timer = pDuration;
    this._shake_interval = pInterval;
    this._shake_intensity = pIntensity;
    this._shake_x = pShakeX;
    this._shake_y = pShakeY;
  }

  private void updateMapLayers(float pElapsed)
  {
    Bench.bench("heat", "game_total");
    this.heat.update(pElapsed);
    Bench.benchEnd("heat", "game_total");
    Bench.bench("map_chunk_manager", "game_total");
    this.map_chunk_manager.update(pElapsed);
    Bench.benchEnd("map_chunk_manager", "game_total");
    Bench.bench("map_layers", "game_total");
    for (int index = 0; index < this._map_layers.Count; ++index)
      this._map_layers[index].update(pElapsed);
    Bench.benchEnd("map_layers", "game_total");
    Bench.bench("map_layers_draw", "game_total");
    for (int index = 0; index < this._map_layers.Count; ++index)
      this._map_layers[index].draw(pElapsed);
    Bench.benchEnd("map_layers_draw", "game_total");
    Bench.bench("map_modules", "game_total");
    for (int index = 0; index < this._map_modules.Count; ++index)
      this._map_modules[index].update(pElapsed);
    Bench.benchEnd("map_modules", "game_total");
  }

  public float calculateCurElapsed() => Time.fixedDeltaTime * Config.time_scale_asset.multiplier;

  private void clearFrameCaches()
  {
  }

  private void LateUpdate() => this.player_control.clearLateUpdate();

  private void Update()
  {
    FPS.update();
    if (!Config.game_loaded)
      return;
    Config.parallel_jobs_updater = DebugConfig.isOn(DebugOption.ParallelJobsUpdater);
    Bench.bench_ai_enabled = DebugConfig.isOn(DebugOption.BenchAiEnabled);
    if (SmoothLoader.isLoading())
    {
      if (DebugConfig.isOn(DebugOption.GenerateNewMapOnMapLoadingError))
      {
        try
        {
          SmoothLoader.update(Time.deltaTime);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ex);
          this.generateNewMap();
        }
      }
      else
        SmoothLoader.update(Time.deltaTime);
    }
    else
    {
      Randy.nextSeed();
      Bench.bench("game_total");
      ScrollingHelper.update();
      Bench.bench("move_camera", "game_total");
      this.move_camera.update();
      Bench.benchEnd("move_camera", "game_total");
      Bench.bench("mapbox_update_1", "game_total");
      this.stack_effects.light_blobs.Clear();
      this._signal_manager.update();
      this.clearFrameCaches();
      Config.updateCrashMetadata();
      PlayerConfig.instance.update();
      Tooltip.checkClearAll();
      CursorTooltipHelper.update();
      this.delta_time = Time.fixedDeltaTime;
      this.fixed_delta_time = Time.fixedDeltaTime;
      this.game_stats.updateStats(Time.deltaTime);
      this._is_paused = Config.paused || ScrollWindow.isWindowActive() || RewardedAds.isShowing();
      this._cached_map_meta_asset = Zones.getMapMetaAsset();
      Bench.benchEnd("mapbox_update_1", "game_total");
      Bench.bench("battle_keeper", "game_total");
      BattleKeeperManager.update(this.delta_time);
      Bench.benchEnd("battle_keeper", "game_total");
      this.elapsed = this.calculateCurElapsed();
      if (Config.fps_lock_30)
        this.elapsed *= 2f;
      MapBox.cursor_speed.update();
      Bench.bench("music_box", "game_total");
      MusicBox.inst.update(this.delta_time);
      Bench.benchEnd("music_box", "game_total");
      Bench.bench("auto_tester", "game_total");
      this.auto_tester.update(this.elapsed);
      Bench.benchEnd("auto_tester", "game_total");
      if (Config.isMobile && (RewardedAds.isShowing() || PlayInterstitialAd.isShowing()))
        return;
      Bench.bench("auto_save", "game_total");
      AutoSaveManager.update();
      Bench.benchEnd("auto_save", "game_total");
      Bench.bench("send_to_sql", "game_total");
      DBInserter.executeCommandsAsync();
      Bench.benchEnd("send_to_sql", "game_total");
      this.checkMainSimulationUpdate();
      this.delayed_actions_manager.update(this.elapsed, this.delta_time);
      this.tilemap.update(this.elapsed);
      Bench.bench("update_shake", "game_total");
      this.updateShake(this.elapsed);
      Bench.benchEnd("update_shake", "game_total");
      Bench.bench("update_panel", "game_total");
      this.map_stats.updateStatsForPanel(this.delta_time);
      Bench.benchEnd("update_panel", "game_total");
      Bench.bench("quality_changer", "game_total");
      this.quality_changer.update();
      Bench.benchEnd("quality_changer", "game_total");
      this.updateTransitionEffect();
      Bench.bench("update_controls", "game_total");
      this.player_control.updateControls();
      Bench.benchEnd("update_controls", "game_total");
      Bench.bench("zone_camera", "game_total");
      this.zone_camera.update();
      Bench.benchEnd("zone_camera", "game_total");
      Bench.bench("unit_select_effect", "game_total");
      if (Object.op_Inequality((Object) this._unit_select_effect, (Object) null))
        this._unit_select_effect.update(this.elapsed);
      Bench.benchEnd("unit_select_effect", "game_total");
      Bench.bench("zone_selection_effect", "game_total");
      this.zone_calculator.updateAnimationsAndSelections();
      Bench.benchEnd("zone_selection_effect", "game_total");
      Bench.bench("nameplates", "game_total");
      this.nameplate_manager.update();
      Bench.benchEnd("nameplates", "game_total");
      if (Config.time_scale_asset.render_skip)
      {
        if (this._render_skip < 2)
        {
          ++this._render_skip;
        }
        else
        {
          this._render_skip = 0;
          this.calculateVisibleObjects();
          this.renderStuff();
        }
      }
      else
      {
        this.calculateVisibleObjects();
        this.renderStuff();
      }
      Bench.bench("update_sprite_constructor", "game_total");
      this.updateDynamicSprites();
      Bench.benchEnd("update_sprite_constructor", "game_total");
      Bench.bench("light_renderer", "game_total");
      LightRenderer.instance.update(this.delta_time);
      Bench.benchEnd("light_renderer", "game_total");
      Bench.bench("update_finish", "game_total");
      this.updateFinish();
      Bench.benchEnd("update_finish", "game_total");
      Bench.bench("end_checks", "game_total");
      this.checkMinWindowSize();
      this.checkVersionCallbacks();
      Bench.update();
      Bench.benchEnd("end_checks", "game_total");
      Bench.benchEnd("game_total");
    }
  }

  private void checkMainSimulationUpdate()
  {
    int num = ScrollWindow.isWindowActive() ? 1 : Config.time_scale_asset.ticks;
    for (int index = 0; index < num; ++index)
      this.updateSimulation(this.elapsed);
  }

  private void updateTransitionEffect()
  {
    if ((double) this._world_layer_switch_effect.color.a == 0.0)
      return;
    Color color = this._world_layer_switch_effect.color;
    color.a -= this.delta_time * 0.1f;
    if ((double) color.a < 0.0)
      color.a = 0.0f;
    this._world_layer_switch_effect.color = color;
  }

  private void updateSimulation(float pElapsed)
  {
    this.updateDirtyMetaContainersAndCleanup();
    this.explosion_checker.update(pElapsed);
    this.city_zone_helper.update(pElapsed);
    if (!this.isPaused())
    {
      this.updateTimerNutrition(pElapsed);
      Bench.bench("update_age", "game_total");
      this.map_stats.updateWorldTime(pElapsed);
      Bench.benchEnd("update_age", "game_total");
      Bench.bench("taxi", "game_total");
      TaxiManager.update(pElapsed);
      Bench.benchEnd("taxi", "game_total");
      Bench.bench("update_meta_history", "game_total");
      this.updateMetaHistory();
      Bench.benchEnd("update_meta_history", "game_total");
    }
    AnimationHelper.updateTime(pElapsed, this.delta_time);
    EnemiesFinder.clear();
    ControllableUnit.updateControllableUnit();
    this.updateMapLayers(pElapsed);
    this.updateCities(pElapsed);
    this.updateActors(pElapsed);
    this.updateBuildings(pElapsed);
    this.drop_manager.update(pElapsed);
    this.cultures.update(pElapsed);
    this.stack_effects.update(pElapsed);
    this.resource_throw_manager.update(pElapsed);
    this.updateWorldBehaviours(pElapsed);
    Bench.bench("army_manager", "game_total");
    this.armies.update(pElapsed);
    Bench.benchEnd("army_manager", "game_total");
    Bench.bench("kingdoms", "game_total");
    this.kingdoms.update(pElapsed);
    Bench.benchEnd("kingdoms", "game_total");
    Bench.bench("diplomacy", "game_total");
    this.diplomacy.update(pElapsed);
    Bench.benchEnd("diplomacy", "game_total");
    Bench.bench("subspecies", "game_total");
    this.subspecies.update(pElapsed);
    Bench.benchEnd("subspecies", "game_total");
    Bench.bench("plots", "game_total");
    this.plots.update(pElapsed);
    Bench.benchEnd("plots", "game_total");
    Bench.bench("clans", "game_total");
    this.clans.update(pElapsed);
    Bench.benchEnd("clans", "game_total");
    Bench.bench("alliances", "game_total");
    this.alliances.update(pElapsed);
    Bench.benchEnd("alliances", "game_total");
    Bench.bench("wars", "game_total");
    this.wars.update(pElapsed);
    Bench.benchEnd("wars", "game_total");
    Bench.bench("languages", "game_total");
    this.languages.update(pElapsed);
    Bench.benchEnd("languages", "game_total");
    Bench.bench("religions", "game_total");
    this.religions.update(pElapsed);
    Bench.benchEnd("religions", "game_total");
    Bench.bench("projectiles", "game_total");
    this.projectiles.update(pElapsed);
    Bench.benchEnd("projectiles", "game_total");
    Bench.bench("stasuses", "game_total");
    this.statuses.update(pElapsed);
    Bench.benchEnd("stasuses", "game_total");
    Bench.bench("era_manager", "game_total");
    this.era_manager.update(pElapsed);
    Bench.benchEnd("era_manager", "game_total");
  }

  private void updateMetaHistory()
  {
    if (!Config.graphs || Config.disable_db || Date.getCurrentMonth() != 12)
      return;
    int currentYear = Date.getCurrentYear();
    if (currentYear == this.map_stats.history_current_year)
      return;
    this.map_stats.history_current_year = currentYear;
    foreach (BaseSystemManager listAllSimManager in this.list_all_sim_managers)
      listAllSimManager.startCollectHistoryData();
    this.world_object.startCollectHistoryData();
    foreach (BaseSystemManager listAllSimManager in this.list_all_sim_managers)
      listAllSimManager.clearLastYearStats();
    this.world_object.clearLastYearStats();
  }

  private void updateDirtyMetaContainersAndCleanup()
  {
    BuildingZonesSystem.update();
    this.checkSimManagerLists();
    this.units.checkContainer();
    this.buildings.checkContainer();
    this.sim_object_zones.update();
    Bench.bench("prepare_for_meta_checks", "game_total");
    this.units.prepareForMetaChecks();
    Bench.benchEnd("prepare_for_meta_checks", "game_total");
    Bench.bench("check_dirty_meta_units", "game_total");
    this.checkDirtyUnits();
    Bench.benchEnd("check_dirty_meta_units", "game_total");
    Bench.bench("check_dirty_meta_objects", "game_total");
    this.checkDirtyMetaObjects();
    Bench.benchEnd("check_dirty_meta_objects", "game_total");
    if (!this.isWindowOnScreen())
    {
      Bench.bench("check_meta_obj_destroy", "game_total");
      this.checkMetaObjectsDestroy();
      Bench.benchEnd("check_meta_obj_destroy", "game_total");
      Bench.bench("check_obj_destroy", "game_total");
      this.checkObjectsToDestroy();
      Bench.benchEnd("check_obj_destroy", "game_total");
    }
    this.checkSimManagerLists();
    Bench.bench("check_references_units", "game_total");
    this.checkEventUnitsDestroy();
    Bench.benchEnd("check_references_units", "game_total");
    Bench.bench("check_references_buildings", "game_total");
    this.checkEventBuildingsDestroy();
    Bench.benchEnd("check_references_buildings", "game_total");
    Bench.bench("check_references_houses", "game_total");
    this.checkEventHouses();
    Bench.benchEnd("check_references_houses", "game_total");
    Bench.bench("check_dirty_meta_objects_2", "game_total");
    this.checkDirtyMetaObjects();
    Bench.benchEnd("check_dirty_meta_objects_2", "game_total");
    Bench.bench("check_anything_changed", "game_total");
    this.checkAnyMetaAddedRemoved();
    Bench.benchEnd("check_anything_changed", "game_total");
  }

  private void checkEventUnitsDestroy()
  {
    if (!this.units.event_destroy)
      return;
    this.units.event_destroy = false;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) this.units)
    {
      if (unit.beh_actor_target != null && !unit.beh_actor_target.isAlive())
        unit.beh_actor_target = (BaseSimObject) null;
      if (unit.attackedBy != null && !unit.attackedBy.isAlive())
        unit.attackedBy = (BaseSimObject) null;
      if (unit.hasLover() && !unit.lover.isAlive())
      {
        unit.lover.lover = (Actor) null;
        unit.lover = (Actor) null;
      }
    }
    TaxiManager.removeDeadUnits();
  }

  private void checkEventBuildingsDestroy()
  {
    if (!this.buildings.event_destroy)
      return;
    List<Actor> simpleList = this.units.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
    {
      Actor actor = simpleList[index];
      if (actor.beh_building_target != null && !actor.beh_building_target.isAlive())
        actor.beh_building_target = (Building) null;
      if (actor.attackedBy != null && !actor.attackedBy.isAlive())
        actor.attackedBy = (BaseSimObject) null;
    }
    this.buildings.event_destroy = false;
  }

  private void checkEventHouses()
  {
    if (!this.buildings.event_houses)
      return;
    foreach (Building occupiedBuilding in this.buildings.occupied_buildings)
    {
      occupiedBuilding.residents.Clear();
      if (occupiedBuilding.asset.docks)
        occupiedBuilding.component_docks.clearBoatCounter();
    }
    List<Actor> simpleList = this.units.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
    {
      Actor pActor = simpleList[index];
      pActor.checkHomeBuilding();
      Building homeBuilding = pActor.home_building;
      if (homeBuilding != null)
      {
        if (homeBuilding.asset.docks)
          homeBuilding.component_docks.increaseBoatCounter(pActor);
        else
          homeBuilding.residents.Add(pActor.data.id);
      }
      Building insideBuilding = pActor.inside_building;
      if (insideBuilding != null && (!insideBuilding.isUsable() || insideBuilding.isAbandoned()))
      {
        pActor.exitBuilding();
        pActor.cancelAllBeh();
      }
    }
    this.buildings.event_houses = false;
  }

  private void debugHouses()
  {
    foreach (Building building in (SimSystemManager<Building, BuildingData>) this.buildings)
    {
      if (!building.isUsable() && building.countResidents() > 0)
        Debug.LogError((object) $"Building {building.data.id.ToString()} has residents but is not usable");
      if (!building.asset.docks && building.countResidents() > building.asset.housing_slots)
        Debug.LogError((object) $"{building.asset.id} has more residents than housing {building.countResidents().ToString()}/{building.asset.housing_slots.ToString()}");
    }
  }

  public void checkSimManagerLists()
  {
    for (int index = 0; index < this.list_all_sim_managers.Count; ++index)
      this.list_all_sim_managers[index].checkLists();
  }

  private void checkDirtyUnits()
  {
    bool flag = false;
    int num = 0;
    for (int index = 0; index < this._list_meta_main_managers.Count; ++index)
    {
      if (this._list_meta_main_managers[index].isUnitsDirty())
      {
        flag = true;
        ++num;
      }
    }
    if (!flag)
      return;
    if (num < 3)
    {
      this.subspecies.beginChecksUnits();
      this.families.beginChecksUnits();
      this.armies.beginChecksUnits();
      this.clans.beginChecksUnits();
      this.plots.beginChecksUnits();
      this.languages.beginChecksUnits();
      this.cultures.beginChecksUnits();
      this.religions.beginChecksUnits();
      this.cities.beginChecksUnits();
      this.kingdoms.beginChecksUnits();
      this.kingdoms_wild.beginChecksUnits();
    }
    else
      Parallel.ForEach<BaseSystemManager>((IEnumerable<BaseSystemManager>) this._list_meta_main_managers, this.parallel_options, (Action<BaseSystemManager>) (pSystem => pSystem.parallelDirtyUnitsCheck()));
  }

  private void checkDirtyMetaObjects()
  {
    this.kingdoms_wild.beginChecksBuildings();
    this.kingdoms.beginChecksBuildings();
    this.cities.beginChecksBuildings();
    this.kingdoms.beginChecksCities();
    this.religions.beginChecksKingdoms();
    this.religions.beginChecksCities();
    this.cultures.beginChecksKingdoms();
    this.cultures.beginChecksCities();
    this.languages.beginChecksKingdoms();
    this.languages.beginChecksCities();
  }

  private void checkAnyMetaAddedRemoved()
  {
    if (!BaseSystemManager.anything_changed)
      return;
    Config.selected_objects_graph.RemoveWhere((Func<NanoObject, bool>) (pNanoObject => pNanoObject.isRekt()));
    if (ScrollWindow.isWindowActive())
    {
      ScrollWindow.checkElements();
      if (!MetaSwitchManager.isAnimationActive())
        MetaSwitchManager.checkAndRefresh();
    }
    SpriteSwitcher.checkAllStates();
    BaseSystemManager.anything_changed = false;
  }

  private void checkMetaObjectsDestroy()
  {
    if (this._meta_skip)
    {
      this._meta_skip = false;
    }
    else
    {
      foreach (BaseSystemManager listMetaMainManager in this._list_meta_main_managers)
        listMetaMainManager.checkDeadObjects();
    }
  }

  private void calculateVisibleObjects()
  {
    this.buildings.calculateVisibleBuildings();
    this.units.calculateVisibleActors();
  }

  public void resetRedrawTimer() => this._redraw_timer = -1f;

  private void renderStuff()
  {
    QuantumSpriteManager.update();
    Bench.bench("redraw_mini_map", "game_total");
    if ((double) this._redraw_timer > 0.0)
    {
      this._redraw_timer -= Time.deltaTime;
    }
    else
    {
      this._redraw_timer = 1f / 1000f;
      if (this.tiles_dirty.Count > 0)
        this.redrawMiniMap();
    }
    Bench.benchEnd("redraw_mini_map", "game_total");
    Bench.bench("redraw_tiles", "game_total");
    this.tilemap.redrawTiles();
    Bench.benchEnd("redraw_tiles", "game_total");
    Bench.bench("update_debug_texts", "game_total");
    this.updateDebugGroupSystem();
    Bench.benchEnd("update_debug_texts", "game_total");
  }

  private void updateFinish()
  {
    if ((double) this.timer_nutrition_decay > 0.0)
      return;
    this.timer_nutrition_decay = SimGlobals.m.interval_nutrition_decay;
  }

  private void checkVersionCallbacks()
  {
    if ((double) VersionCallbacks.timer > 0.0)
      VersionCallbacks.updateVC(this.elapsed);
    if (!Config.EVERYTHING_FIREWORKS)
      return;
    this.spawnForcedFireworks();
  }

  private void checkMinWindowSize()
  {
    if (Screen.fullScreen)
      return;
    if (Screen.width < 720)
    {
      Screen.SetResolution(720, Screen.height, false);
    }
    else
    {
      if (Screen.height >= 480)
        return;
      Screen.SetResolution(Screen.width, 480, false);
    }
  }

  private void checkObjectsToDestroy()
  {
    this.buildings.checkObjectsToDestroy();
    this.units.checkObjectsToDestroy();
  }

  private void updateWorldBehaviours(float pElapsed)
  {
    if (!DebugConfig.isOn(DebugOption.SystemWorldBehaviours))
      return;
    Bench.bench("world_beh", "game_total");
    List<WorldBehaviourAsset> list = AssetManager.world_behaviours.list;
    for (int index = 0; index < list.Count; ++index)
    {
      WorldBehaviourAsset worldBehaviourAsset = list[index];
      if (worldBehaviourAsset.enabled)
      {
        Bench.bench(worldBehaviourAsset.id, "world_beh");
        worldBehaviourAsset.manager.update(pElapsed);
        Bench.benchEnd(worldBehaviourAsset.id, "world_beh");
      }
    }
    Bench.benchEnd("world_beh", "game_total");
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float getWorldTimeElapsedSince(double pTime)
  {
    return (float) (this.map_stats.world_time - pTime);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float getRealTimeElapsedSince(double pTime) => (float) (this.getCurSessionTime() - pTime);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double getCurWorldTime() => this.map_stats.world_time;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double getCurSessionTime() => this.game_stats.data.gameTime;

  public void updateDynamicSprites() => AssetManager.dynamic_sprites_library.checkDirty();

  public void updateDebugGroupSystem() => this._debug_text_group_system.update(this.elapsed);

  internal void updateTimerNutrition(float pElapsed)
  {
    if ((double) this.timer_nutrition_decay <= 0.0)
      return;
    this.timer_nutrition_decay -= pElapsed;
  }

  internal void updateObjectAge()
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) this.units)
      unit.updateAge();
    this.cities.updateAge();
    this.kingdoms.updateAge();
  }

  private void updateCities(float pElapsed)
  {
    if (!DebugConfig.isOn(DebugOption.SystemUpdateCities))
      return;
    Bench.bench("cities", "game_total");
    this.cities.update(pElapsed);
    Bench.benchEnd("cities", "game_total");
  }

  private void updateBuildings(float pElapsed)
  {
    if (!DebugConfig.isOn(DebugOption.SystemUpdateBuildings))
      return;
    this.buildings.update(pElapsed);
  }

  private void updateActors(float pElapsed)
  {
    if (!DebugConfig.isOn(DebugOption.SystemUpdateUnits))
      return;
    this.units.update(pElapsed);
  }

  private void allTilesDirty()
  {
    this.tiles_dirty.Clear();
    this.tilemap.clear();
    for (int index = 0; index < this.tiles_list.Length; ++index)
      this.setTileDirty(this.tiles_list[index]);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void redrawRenderedTile(WorldTile pTile)
  {
    pTile.last_rendered_tile_type = (TileTypeBase) null;
    this.setTileDirty(pTile);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void setTileDirty(WorldTile pTile)
  {
    pTile.updateStats();
    this.tiles_dirty.Add(pTile);
    if (this.tilemap.needsRedraw(pTile))
    {
      this.tilemap.addToQueueToRedraw(pTile);
      if (pTile.has_tile_up)
        this.tilemap.addToQueueToRedraw(pTile.tile_up);
      if (pTile.has_tile_down)
        this.tilemap.addToQueueToRedraw(pTile.tile_down);
      if (pTile.has_tile_left)
        this.tilemap.addToQueueToRedraw(pTile.tile_left);
      if (pTile.has_tile_right)
        this.tilemap.addToQueueToRedraw(pTile.tile_right);
    }
    this.world_layer_edges.addDirtyChunk(pTile.chunk);
    this.checkBehaviours(pTile);
  }

  internal void setZoomOrthographic(float pZoom) => this.quality_changer.setZoomOrthographic(pZoom);

  public void redrawMiniMap(bool pForce = false)
  {
    if (!DebugConfig.isOn(DebugOption.SystemRedrawMap) || !(MapBox.isRenderMiniMap() | pForce))
      return;
    this.dirty_tiles_last = this.tiles_dirty.Count;
    foreach (WorldTile pTile in this.tiles_dirty)
      this.updateDirtyTile(pTile);
    this.world_layer_edges.redraw();
    this.tiles_dirty.Clear();
    this.world_layer.updatePixels();
  }

  internal void checkBehaviours(WorldTile pTile)
  {
    if (pTile.Type.explodable_timed)
      this.explosion_layer.addTimedTnt(pTile);
    if (pTile.Type.can_be_filled_with_ocean)
      WorldBehaviourOcean.tiles.Add(pTile);
    else
      WorldBehaviourOcean.tiles.Remove(pTile);
  }

  private void updateDirtyTile(WorldTile pTile)
  {
    if (pTile.hasBuilding())
    {
      Color grey = Color.grey;
      if (!Color.op_Equality(Color32.op_Implicit(pTile.building.getColorForMinimap(pTile)), Color32.op_Implicit(Toolbox.clear)))
      {
        this.world_layer.pixels[pTile.data.tile_id] = pTile.building.getColorForMinimap(pTile);
        return;
      }
    }
    this.world_layer.pixels[pTile.data.tile_id] = pTile.getColor();
  }

  public void followUnit(Actor pActor)
  {
    SelectedUnit.clear();
    this.move_camera.focusOnAndFollow(pActor, (Action) null, (Action) null);
  }

  public void locateSelectedVillage()
  {
    City selectedCity = SelectedMetas.selected_city;
    ScrollWindow.hideAllEvent();
    this.move_camera.focusOn(Vector2.op_Implicit(selectedCity.city_center));
  }

  public void locatePosition(Vector3 pVector)
  {
    if (this.isGameplayControlsLocked())
      ScrollWindow.hideAllEvent();
    this.move_camera.focusOn(pVector);
  }

  public void locatePosition(
    Vector3 pVector,
    Action pFocusReachedCallback,
    Action pFocusCancelCallback)
  {
    if (this.isGameplayControlsLocked())
      ScrollWindow.hideAllEvent();
    this.move_camera.focusOn(pVector, pFocusReachedCallback, pFocusCancelCallback);
  }

  public void locateAndFollow(
    Actor pActor,
    Action pFocusReachedCallback,
    Action pFocusCancelCallback)
  {
    if (this.isGameplayControlsLocked())
      ScrollWindow.hideAllEvent();
    this.move_camera.focusOnAndFollow(pActor, pFocusReachedCallback, pFocusCancelCallback);
  }

  public bool isSelectedPower(string pPower)
  {
    return this.isAnyPowerSelected() && this.selected_power.id == pPower;
  }

  public string getSelectedPowerID()
  {
    return !this.isAnyPowerSelected() ? string.Empty : this.selected_power.id;
  }

  public GodPower selected_power => this.selected_buttons.selectedButton?.godPower;

  public MouseHoldAnimation getSelectedPowerHoldAnimation()
  {
    return !this.isAnyPowerSelected() ? MouseHoldAnimation.Default : this.getSelectedPowerAsset().mouse_hold_animation;
  }

  public bool canDragMap()
  {
    return !this.isAnyPowerSelected() || this.getSelectedPowerAsset().can_drag_map;
  }

  public GodPower getSelectedPowerAsset()
  {
    return !this.isAnyPowerSelected() ? (GodPower) null : AssetManager.powers.get(this.getSelectedPowerID());
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isAnyPowerSelected()
  {
    return Object.op_Inequality((Object) this.selected_buttons.selectedButton, (Object) null);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal bool isPaused() => this._is_paused;

  internal void spawnCongratulationFireworks()
  {
    City random1 = this.cities.getRandom();
    if (random1 == null)
      return;
    Building random2 = Randy.getRandom<Building>(random1.buildings);
    if (random2 == null || random2.isUnderConstruction())
      return;
    EffectsLibrary.spawn("fx_fireworks", random2.current_tile);
  }

  internal void spawnForcedFireworks()
  {
    WorldTile random = Randy.getRandom<WorldTile>(this.tiles_list);
    PlayerConfig.dict["sound"].boolVal = true;
    EffectsLibrary.spawn("fx_fireworks", random);
  }

  public int getCivWorldPopulation()
  {
    int civWorldPopulation = 0;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) this.units)
    {
      if (unit.isSapient())
        ++civWorldPopulation;
      if (unit.asset.is_boat)
        ++civWorldPopulation;
    }
    return civWorldPopulation;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isRenderMiniMap() => MapBox.instance.quality_changer.isLowRes();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isRenderGameplay() => !MapBox.instance.quality_changer.isLowRes();

  internal static void aye() => CornerAye.instance.startAye();

  public MetaTypeAsset getCachedMapMetaAsset() => this._cached_map_meta_asset;

  public ArchitectMood getArchitectMood()
  {
    if (this._cached_architect_mood == null)
      this._cached_architect_mood = this.map_stats.getArchitectMood();
    return this._cached_architect_mood;
  }

  public Color getArchitectColor() => this.getArchitectMood().getColor();
}
