// Decompiled with JetBrains decompiler
// Type: MusicBoxLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class MusicBoxLibrary : AssetLibrary<MusicAsset>
{
  internal readonly List<MusicBoxContainerTiles> c_list_params = new List<MusicBoxContainerTiles>();
  internal readonly Dictionary<string, MusicBoxContainerCivs> c_dict_civs = new Dictionary<string, MusicBoxContainerCivs>();
  internal readonly List<MusicBoxContainerTiles> c_list_environments = new List<MusicBoxContainerTiles>();
  internal readonly Dictionary<string, MusicBoxContainerUnits> c_dict_units = new Dictionary<string, MusicBoxContainerUnits>();
  public static MusicAsset New_World;
  public static MusicAsset Menu;
  public static MusicAsset Neutral_001;
  public static MusicAsset Locations_Desert;

  public override void init()
  {
    base.init();
    MusicAsset pAsset1 = new MusicAsset();
    pAsset1.id = "Menu";
    pAsset1.disable_param_after_start = true;
    MusicBoxLibrary.Menu = this.add(pAsset1);
    MusicAsset pAsset2 = new MusicAsset();
    pAsset2.id = "Neutral_001";
    pAsset2.disable_param_after_start = true;
    MusicBoxLibrary.Neutral_001 = this.add(pAsset2);
    MusicAsset pAsset3 = new MusicAsset();
    pAsset3.id = "New_World";
    pAsset3.disable_param_after_start = true;
    MusicBoxLibrary.New_World = this.add(pAsset3);
    MusicAsset pAsset4 = new MusicAsset();
    pAsset4.id = "wea_rain";
    this.add(pAsset4);
    MusicAsset pAsset5 = new MusicAsset();
    pAsset5.id = "wea_snow";
    this.add(pAsset5);
    this.addUnits();
    this.addUnique();
    this.addLoc();
    this.addEnv();
  }

  private void addEnv()
  {
    MusicAsset pAsset1 = new MusicAsset();
    pAsset1.id = "LavaEnvironment";
    pAsset1.fmod_path = "event:/SFX/ENVIRONMENT/LavaEnvironment";
    pAsset1.is_environment = true;
    pAsset1.is_param = true;
    pAsset1.min_tiles_to_play = 30;
    this.add(pAsset1);
    this.t.setTileTypes("lava0", "lava1", "lava2", "lava3");
    MusicAsset pAsset2 = new MusicAsset();
    pAsset2.id = "MapEnvironment";
    pAsset2.fmod_path = "event:/SFX/ENVIRONMENT/MapEnvironment";
    pAsset2.is_environment = true;
    pAsset2.is_param = true;
    pAsset2.mini_map_only = true;
    this.add(pAsset2);
    MusicAsset pAsset3 = new MusicAsset();
    pAsset3.id = "Ocean";
    pAsset3.fmod_path = "event:/SFX/ENVIRONMENT/Ocean";
    pAsset3.is_environment = true;
    pAsset3.is_param = true;
    pAsset3.min_tiles_to_play = 100;
    this.add(pAsset3);
    this.t.setTileTypes("deep_ocean");
    MusicAsset pAsset4 = new MusicAsset();
    pAsset4.id = "Sea";
    pAsset4.fmod_path = "event:/SFX/ENVIRONMENT/Sea";
    pAsset4.is_param = true;
    pAsset4.is_environment = true;
    pAsset4.min_tiles_to_play = 100;
    this.add(pAsset4);
    this.t.setTileTypes("close_ocean");
  }

  private void addLoc()
  {
    MusicAsset pAsset1 = new MusicAsset();
    pAsset1.id = "Locations_Forest";
    pAsset1.is_param = true;
    this.add(pAsset1);
    this.t.setTileTypes("grass_high", "grass_low", "enchanted_low", "enchanted_high");
    MusicAsset pAsset2 = new MusicAsset();
    pAsset2.id = "Locations_Desert";
    pAsset2.is_param = true;
    MusicBoxLibrary.Locations_Desert = this.add(pAsset2);
    MusicAsset pAsset3 = new MusicAsset();
    pAsset3.id = "Locations_Mountains";
    pAsset3.is_param = true;
    this.add(pAsset3);
    this.t.setTileTypes("hills", "mountains");
    MusicAsset pAsset4 = new MusicAsset();
    pAsset4.id = "Locations_Ocean";
    pAsset4.is_param = true;
    this.add(pAsset4);
    this.t.setTileTypes("deep_ocean", "close_ocean");
    MusicAsset pAsset5 = new MusicAsset();
    pAsset5.id = "Locations_Snow";
    pAsset5.is_param = true;
    this.add(pAsset5);
    this.t.setTileTypes("snow_block", "permafrost_high", "snow_hills", "permafrost_low", "snow_sand");
  }

  private void addUnique()
  {
    MusicAsset pAsset1 = new MusicAsset();
    pAsset1.id = "Crabzilla";
    pAsset1.is_unit_param = true;
    pAsset1.special_delegate_units = (MusicAssetDelegate) (pContainer =>
    {
      if (!ControllableUnit.isControllingCrabzilla())
        return;
      pContainer.units = 1;
    });
    pAsset1.priority = MusicLayerPriority.High;
    this.add(pAsset1);
    MusicAsset pAsset2 = new MusicAsset();
    pAsset2.id = "GreyGoo";
    pAsset2.is_unit_param = true;
    pAsset2.special_delegate_units = (MusicAssetDelegate) (pContainer =>
    {
      if (!World.world.grey_goo_layer.isActive())
        return;
      if (!WorldLawLibrary.world_law_gaias_covenant.isEnabled() || MapBox.isRenderMiniMap())
      {
        pContainer.units = 1;
      }
      else
      {
        List<TileZone> visibleZones = World.world.zone_camera.getVisibleZones();
        for (int index = 0; index < visibleZones.Count; ++index)
        {
          if (visibleZones[index].hasTilesOfType((TileTypeBase) TileLibrary.grey_goo))
          {
            pContainer.units = 1;
            break;
          }
        }
      }
    });
    pAsset2.priority = MusicLayerPriority.Medium;
    this.add(pAsset2);
    MusicAsset pAsset3 = new MusicAsset();
    pAsset3.id = "Unique_ZombieInfection";
    pAsset3.is_unit_param = true;
    pAsset3.special_delegate_units = (MusicAssetDelegate) (pContainer => { });
    pAsset3.priority = MusicLayerPriority.Medium;
    this.add(pAsset3);
  }

  private void addUnits()
  {
    MusicAsset pAsset1 = new MusicAsset();
    pAsset1.id = "_units_param";
    pAsset1.is_unit_param = true;
    this.add(pAsset1);
    this.clone("Units_Bandits", "_units_param");
    this.clone("Units_Bear", "_units_param");
    this.clone("Units_BeeHive", "_units_param");
    this.clone("Units_Cat", "_units_param");
    this.clone("Units_Chicken", "_units_param");
    this.clone("Units_ColdOne", "_units_param");
    this.clone("Units_Demon", "_units_param");
    this.clone("Units_Fairy", "_units_param");
    this.clone("Units_LivingPlants", "_units_param");
    this.clone("Units_Piranha", "_units_param");
    this.clone("Units_Rabbit", "_units_param");
    this.clone("Units_Rat", "_units_param");
    this.clone("Units_Skeleton", "_units_param");
    this.clone("Units_Sheep", "_units_param");
    this.clone("Units_Snowman", "_units_param");
    this.clone("Units_Wolf", "_units_param");
    this.clone("Units_Worm", "_units_param");
    this.clone("Units_Zombie", "_units_param");
    this.clone("Buildings_Tumor", "_units_param");
    this.clone("Humans_Neutral", "_units_param");
    this.clone("Elves_Neutral", "_units_param");
    this.clone("Orcs_Neutral", "_units_param");
    this.clone("Dwarves_Neutral", "_units_param");
    MusicAsset pAsset2 = new MusicAsset();
    pAsset2.id = "Units_GodFinger";
    pAsset2.is_unit_param = true;
    pAsset2.special_delegate_units = new MusicAssetDelegate(this.special_god_finger);
    this.add(pAsset2);
    MusicAsset pAsset3 = new MusicAsset();
    pAsset3.id = "Units_Dragon";
    pAsset3.is_unit_param = true;
    pAsset3.special_delegate_units = new MusicAssetDelegate(this.special_dragon);
    this.add(pAsset3);
    MusicAsset pAsset4 = new MusicAsset();
    pAsset4.id = "Units_Santa";
    pAsset4.is_unit_param = true;
    this.add(pAsset4);
    MusicAsset pAsset5 = new MusicAsset();
    pAsset5.id = "Units_UFO";
    pAsset5.is_unit_param = true;
    pAsset5.special_delegate_units = new MusicAssetDelegate(this.special_ufo);
    this.add(pAsset5);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (MusicAsset pAsset in this.list)
    {
      if (pAsset.civilization)
        this.addCivContainer(pAsset);
      if (pAsset.is_param)
        this.addTileContainer(pAsset);
      if (pAsset.is_unit_param)
        this.addUnitContainer(pAsset);
      string[] tileTypeStrings = pAsset.tile_type_strings;
      if ((tileTypeStrings != null ? (tileTypeStrings.Length != 0 ? 1 : 0) : 0) != 0)
      {
        using (ListPool<TileTypeBase> list = new ListPool<TileTypeBase>(pAsset.tile_type_strings.Length))
        {
          foreach (string tileTypeString in pAsset.tile_type_strings)
          {
            if (AssetManager.top_tiles.has(tileTypeString))
              list.Add((TileTypeBase) AssetManager.top_tiles.get(tileTypeString));
            else if (AssetManager.tiles.has(tileTypeString))
              list.Add((TileTypeBase) AssetManager.tiles.get(tileTypeString));
            else
              BaseAssetLibrary.logAssetError("MusicBoxLibrary: No matching Tile Type found for", tileTypeString);
          }
          pAsset.tile_types = list.ToArray<TileTypeBase>();
        }
      }
    }
    foreach (MusicAsset musicAsset in this.list)
    {
      if (musicAsset.tile_types != null)
      {
        foreach (TileTypeBase tileType in musicAsset.tile_types)
        {
          TileTypeBase tileTypeBase1;
          TileTypeBase tileTypeBase2 = tileTypeBase1 = tileType;
          if (tileTypeBase2.music_assets == null)
            tileTypeBase2.music_assets = new List<MusicAsset>();
          tileTypeBase1.music_assets.Add(musicAsset);
        }
      }
    }
  }

  private MusicBoxContainerUnits addUnitContainer(MusicAsset pAsset)
  {
    MusicBoxContainerUnits boxContainerUnits = new MusicBoxContainerUnits();
    this.c_dict_units.Add(pAsset.id, boxContainerUnits);
    boxContainerUnits.asset = pAsset;
    return boxContainerUnits;
  }

  private MusicBoxContainerCivs addCivContainer(MusicAsset pAsset)
  {
    MusicBoxContainerCivs boxContainerCivs = new MusicBoxContainerCivs();
    this.c_dict_civs.Add(pAsset.id, boxContainerCivs);
    boxContainerCivs.asset = pAsset;
    return boxContainerCivs;
  }

  private MusicBoxContainerTiles addTileContainer(MusicAsset pAsset)
  {
    MusicBoxContainerTiles boxContainerTiles = new MusicBoxContainerTiles();
    boxContainerTiles.asset = pAsset;
    pAsset.container_tiles = boxContainerTiles;
    this.c_list_params.Add(boxContainerTiles);
    if (pAsset.is_environment)
      this.c_list_environments.Add(boxContainerTiles);
    return boxContainerTiles;
  }

  private void special_god_finger(MusicBoxContainerUnits pContainer)
  {
    Kingdom kingdom = World.world.kingdoms_wild.get("godfinger");
    pContainer.units += kingdom.units.Count;
  }

  private void special_dragon(MusicBoxContainerUnits pContainer)
  {
    Kingdom kingdom = World.world.kingdoms_wild.get("dragons");
    pContainer.units += kingdom.units.Count;
  }

  private void special_ufo(MusicBoxContainerUnits pContainer)
  {
    ActorAsset actorAsset = AssetManager.actor_library.get("UFO");
    pContainer.units += actorAsset.units.Count;
  }
}
