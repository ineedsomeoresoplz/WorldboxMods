// Decompiled with JetBrains decompiler
// Type: TrailerMonolith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrailerMonolith : MonoBehaviour
{
  public static readonly bool enable_trailer_stuff;
  public bool enabled_auto = true;
  public AudioSource audio_source;
  private GameObject camera_object;
  private string[] _biomes = new string[21]
  {
    "biome_savanna",
    "biome_grass",
    "biome_infernal",
    "biome_crystal",
    "biome_lemon",
    "biome_singularity",
    "biome_garlic",
    "biome_clover",
    "biome_candy",
    "biome_permafrost",
    "biome_desert",
    "biome_swamp",
    "biome_maple",
    "biome_birch",
    "biome_flower",
    "biome_paradox",
    "biome_mushroom",
    "biome_rocklands",
    "biome_enchanted",
    "biome_corrupted",
    "biome_jungle"
  };
  private string[] _unit_assets_to_spawn = new string[9]
  {
    "demon",
    "cold_one",
    "sheep",
    "angle",
    "skeleton",
    "evil_mage",
    "white_mage",
    "alien",
    "necromancer"
  };
  private int[] _keys = new int[16 /*0x10*/]
  {
    0,
    4,
    8,
    10,
    11,
    13,
    16 /*0x10*/,
    24,
    32 /*0x20*/,
    36,
    40,
    42,
    43,
    45,
    48 /*0x30*/,
    56
  };
  private double[] _keys_timings;
  private double[] _drums;
  private int _current_biome;
  private const double INTERVAL_DANCING_TREES = 0.46153125166893005;
  private double _timer_dancing_trees;
  private const double INTERVAL_LOOP = 29.538000106811523;
  private double _timer_song;
  private double offset_timings = -0.15000000596046448;
  private double _last_offset;
  public double track_time;
  private HashSet<int> _processed_keys = new HashSet<int>();
  private HashSet<int> _processed_drums = new HashSet<int>();
  private HashSet<Building> _processed_buildings = new HashSet<Building>();
  public bool reset;
  public bool transition;
  public static int harp_frame_index;
  private const int HARP_MAX_FRAMES = 19;
  private bool _camera_go_zoom = true;
  private float _camera_switch_timer = 10f;
  private const int MAX_WAVE = 5;
  private int _tree_wave;

  public void Start()
  {
    this.camera_object = ((Component) Camera.main).gameObject;
    this.calculateTimings();
    this.camera_object.AddComponent<AudioListener>();
    DebugConfig.setOption(DebugOption.ArrowsUnitsAttackTargets, false);
    this._drums = new double[64 /*0x40*/];
    for (int index = 0; index < this._drums.Length; ++index)
      this._drums[index] = (double) index * 29.538000106811523 / 64.0;
  }

  private void newLoop()
  {
    this._processed_keys.Clear();
    this._processed_drums.Clear();
    this._processed_buildings.Clear();
    this.resetDancingTrees();
    double num = 0.0;
    if (this._timer_song > 29.538000106811523)
      num = this._timer_song - 29.538000106811523;
    this._timer_song = 0.0 + num;
  }

  private void resetTrack()
  {
    this.audio_source.Stop();
    this.audio_source.time = 0.0f;
    this.audio_source.Play();
  }

  private void calculateTimings()
  {
    this._last_offset = this.offset_timings;
    this._keys_timings = new double[this._keys.Length];
    for (int index = 0; index < this._keys.Length; ++index)
      this._keys_timings[index] = (double) this._keys[index] * 29.538000106811523 / 64.0 + this.offset_timings;
  }

  private void resetDancingTrees()
  {
    this._timer_dancing_trees = 0.0 + (this._timer_dancing_trees <= 0.46153125166893005 ? 0.0 : this._timer_dancing_trees - 0.46153125166893005);
  }

  public void Update()
  {
    if (Config.worldLoading || !this.enabled_auto)
      return;
    if (Input.GetKeyDown((KeyCode) 114))
      this.reset = true;
    if (World.world.isPaused())
      return;
    if (this._last_offset != this.offset_timings)
      this.calculateTimings();
    if (Time.frameCount % 5 == 0 && TrailerMonolith.harp_frame_index < 19)
      ++TrailerMonolith.harp_frame_index;
    this.track_time = this._timer_song;
    if (this.reset)
    {
      this.reset = false;
      this.resetTrack();
      this.newLoop();
      World.world.move_camera.forceZoom(30f);
      this._camera_switch_timer = 2f;
    }
    this.updateCamera();
    if (this._timer_song < 29.538000106811523)
    {
      for (int index = 0; index < this._drums.Length; ++index)
      {
        if (!this._processed_drums.Contains(index) && this._timer_song >= this._drums[index])
        {
          this._processed_drums.Add(index);
          this.dancingTrees();
        }
      }
      for (int pIndex = 0; pIndex < this._keys_timings.Length; ++pIndex)
      {
        if (!this._processed_keys.Contains(pIndex) && this._timer_song >= this._keys_timings[pIndex])
        {
          this.glowMonolith(pIndex);
          this.spawnRandomUnit();
          this.spawnRandomUnit();
          this.spawnRandomUnit();
          this.spawnRandomLightning();
          this.spawnRandomLightning();
          this.spawnRandomLightning();
          this.spawnRandomLightning();
          this._processed_keys.Add(pIndex);
          if (pIndex == 8 || pIndex == 14 || pIndex == 6)
          {
            this.doMonolithAction();
            this.switchBiome();
            this.spawnRandoMTornado();
          }
        }
      }
    }
    if (this._timer_song < 29.538000106811523 && !this.transition)
    {
      this._timer_song += (double) Time.deltaTime;
    }
    else
    {
      this.newLoop();
      this.doMonolithAction();
      this.switchBiome();
    }
    if (!this.transition)
      return;
    this.transition = false;
  }

  private void updateCamera()
  {
    World.world.move_camera.camera_zoom_speed = 0.2f;
    if ((double) this._camera_switch_timer > 0.0)
    {
      this._camera_switch_timer -= Time.deltaTime;
    }
    else
    {
      this._camera_switch_timer = 10f;
      this._camera_go_zoom = !this._camera_go_zoom;
    }
    if (this._camera_go_zoom)
      World.world.move_camera.setTargetZoom(30f);
    else
      World.world.move_camera.setTargetZoom(60f);
  }

  private void spawnRandoMTornado()
  {
    EffectsLibrary.spawnAtTile("fx_tornado", TopTileLibrary.wall_ancient.getCurrentTiles().GetRandom<WorldTile>(), 0.125f);
  }

  private void spawnRandomLightning()
  {
    MapBox.spawnLightningSmall(World.world.islands_calculator.tryGetRandomGround());
  }

  private void doMonolithAction()
  {
    Building monolith = this.findMonolith();
    EffectsLibrary.spawnAt("fx_monolith_launch_bottom", monolith.current_tile.posV3, monolith.current_scale.y);
    EffectsLibrary.spawnAt("fx_monolith_launch", monolith.current_tile.posV3, monolith.current_scale.y);
  }

  private void spawnRandomUnit()
  {
    string random1 = this._unit_assets_to_spawn.GetRandom<string>();
    WorldTile random2 = TileLibrary.hills.getCurrentTiles().GetRandom<WorldTile>();
    bool pMiracleSpawn = Randy.randomChance(0.8f);
    World.world.units.spawnNewUnit(random1, random2, pMiracleSpawn: pMiracleSpawn);
    EffectsLibrary.spawn("fx_spawn", random2);
  }

  private void glowMonolith(int pIndex)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      unit.makeStunned(1f);
      unit.applyRandomForce();
    }
    Building monolith = this.findMonolith();
    if (monolith == null)
      return;
    if (pIndex == 5 || pIndex == 9 || pIndex == 13)
      EffectsLibrary.spawnAt("fx_monolith_glow_1", monolith.current_tile.posV3, monolith.current_scale.y);
    else
      EffectsLibrary.spawnAt("fx_monolith_glow_2", monolith.current_tile.posV3, monolith.current_scale.y);
    TrailerMonolith.harp_frame_index = 11;
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (!(building.asset.id == "monolith") && !(building.asset.id == "waypoint_harp"))
        building.startShake(0.5f);
    }
  }

  private Building findMonolith()
  {
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (!(building.asset.id != "monolith"))
      {
        building.setMaxHealth();
        return building;
      }
    }
    return (Building) null;
  }

  private void dancingTrees()
  {
    Building monolith = this.findMonolith();
    if (monolith == null)
      return;
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (!(building.asset.id == "monolith") && !(building.asset.id == "waypoint_harp"))
        building.setScaleTween(0.9f);
    }
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (!(building.asset.id != "waypoint_harp"))
        building.setScaleTween(0.8f);
    }
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (!(building.asset.id == "monolith") && !(building.asset.id == "waypoint_harp") && building.asset.building_type == BuildingType.Building_Tree)
        building.setScaleTween(0.9f);
    }
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (!(building.asset.id == "monolith") && !(building.asset.id == "waypoint_harp") && building.asset.building_type == BuildingType.Building_Tree && !this._processed_buildings.Contains(building))
      {
        float num1 = Vector3.Distance(building.current_tile.posV3, Vector2.op_Implicit(monolith.current_position));
        float num2 = 1f;
        switch (this._tree_wave)
        {
          case 0:
            num2 = 10f;
            break;
          case 1:
            num2 = 15f;
            break;
          case 2:
            num2 = 25f;
            break;
          case 3:
            num2 = 35f;
            break;
          case 4:
            num2 = 50f;
            break;
        }
        if ((double) num1 <= (double) num2)
        {
          this._processed_buildings.Add(building);
          float pDuration = 0.3f * (float) (5 - this._tree_wave) + Randy.randomFloat(0.0f, 0.1f);
          building.setScaleTween(0.3f, pDuration);
        }
      }
    }
    ++this._tree_wave;
    if (this._tree_wave < 5)
      return;
    this._tree_wave = 0;
    this._processed_buildings.Clear();
  }

  private void switchBiome()
  {
    ++this._current_biome;
    if (this._current_biome >= this._biomes.Length)
      this._current_biome = 0;
    this._tree_wave = 0;
    World.world.era_manager.startNextAge();
    BiomeAsset biomeAsset = AssetManager.biome_library.get(this._biomes[this._current_biome]);
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.main_type.soil && tiles.top_type != null && tiles.top_type.is_biome)
      {
        if (tiles.main_type.rank_type == TileRank.High)
          tiles.setTopTileType(biomeAsset.getTileHigh());
        else
          tiles.setTopTileType(biomeAsset.getTileLow());
      }
    }
    foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
    {
      if (building.asset.flora)
      {
        if (building.asset.building_type == BuildingType.Building_Tree)
        {
          if (!(building.asset.id == "palm_tree"))
          {
            string random = biomeAsset.pot_trees_spawn.GetRandom<string>();
            BuildingAsset buildingAsset = AssetManager.buildings.get(random);
            building.asset = buildingAsset;
            building.clearSprites();
          }
        }
        else if (building.asset.building_type == BuildingType.Building_Plant)
        {
          string random = biomeAsset.pot_plants_spawn.GetRandom<string>();
          if (random == "fruit_bush")
          {
            for (int index = 0; index < biomeAsset.pot_plants_spawn.Count; ++index)
            {
              if (!(biomeAsset.pot_plants_spawn[index] == "fruit_bush"))
              {
                random = biomeAsset.pot_plants_spawn[index];
                break;
              }
            }
          }
          BuildingAsset buildingAsset = AssetManager.buildings.get(random);
          building.asset = buildingAsset;
          building.clearSprites();
        }
      }
    }
  }
}
