// Decompiled with JetBrains decompiler
// Type: DebugLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DebugLayer : MapLayer
{
  internal static List<TileZone> fmod_zones_to_draw = new List<TileZone>();
  private HashSet<WorldTile> _tiles = new HashSet<WorldTile>();
  public Color color1 = Color.gray;
  public Color color2 = Color.white;
  public Color color_red = Color.red;
  public Color color_active_path;
  private bool used;
  private List<MapRegion> _forced_global_path = new List<MapRegion>();

  protected override void UpdateDirty(float pElapsed)
  {
    if (!DebugConfig.instance.debugButton.gameObject.activeSelf)
    {
      this.clear();
    }
    else
    {
      this.color_active_path = new Color(1f, 1f, 1f, 0.5f);
      this.used = false;
      this.clear();
      if (this._forced_global_path != null && this._forced_global_path.Count > 0)
        this.drawRegionPath(this._forced_global_path);
      if (DebugConfig.isOn(DebugOption.CityZones))
        this.drawZones();
      else if (DebugConfig.isOn(DebugOption.Chunks))
        this.drawChunks();
      if (DebugConfig.isOn(DebugOption.PathRegions))
        this.drawPathRegions();
      if (DebugConfig.isOn(DebugOption.ActivePaths))
        this.drawActivePaths();
      if (DebugConfig.isOn(DebugOption.CityPlaces))
        this.drawCityPlaces();
      if (DebugConfig.isOn(DebugOption.RenderCityDangerZones))
        this.drawCityDangerZones();
      if (DebugConfig.isOn(DebugOption.RenderVisibleZones))
        this.drawVisibleZones();
      if (DebugConfig.isOn(DebugOption.RenderCityCenterZones))
        this.drawCityCenterZones();
      if (DebugConfig.isOn(DebugOption.RenderCityFarmPlaces))
        this.drawCityFarmZones();
      if (DebugConfig.isOn(DebugOption.Buildings))
        this.drawBuildings();
      if (DebugConfig.isOn(DebugOption.FmodZones))
        this.drawFmodZones();
      if (DebugConfig.isOn(DebugOption.ConstructionTiles))
        this.drawConstructionTiles();
      if (DebugConfig.isOn(DebugOption.UnitsInside))
        this.drawUnitsInside();
      if (DebugConfig.isOn(DebugOption.TargetedBy))
        this.drawTargetedBy();
      if (DebugConfig.isOn(DebugOption.UnitKingdoms))
        this.drawUnitKingdoms();
      if (DebugConfig.isOn(DebugOption.DisplayUnitTiles))
        this.drawUnitTiles();
      if (DebugConfig.isOn(DebugOption.ProKing))
        this.drawProfession(UnitProfession.King);
      if (DebugConfig.isOn(DebugOption.ProLeader))
        this.drawProfession(UnitProfession.Leader);
      if (DebugConfig.isOn(DebugOption.ProUnit))
        this.drawProfession(UnitProfession.Unit);
      if (DebugConfig.isOn(DebugOption.ProWarrior))
        this.drawProfession(UnitProfession.Warrior);
      if (this.used)
      {
        if (!((Component) this).gameObject.activeSelf)
          ((Component) this).gameObject.SetActive(true);
        this.updatePixels();
      }
      else
      {
        if (!((Component) this).gameObject.activeSelf)
          return;
        ((Component) this).gameObject.SetActive(false);
      }
    }
  }

  private void drawUnitKingdoms()
  {
    this.used = true;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.kingdom != null && unit.kingdom.getColor() != null)
      {
        Color color = Color32.op_Implicit(unit.kingdom.getColor().getColorMain32());
        this.pixels[unit.current_tile.data.tile_id] = Color32.op_Implicit(color);
        this._tiles.Add(unit.current_tile);
      }
    }
  }

  private void drawUnitTiles()
  {
    this.used = true;
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.hasUnits())
      {
        this.pixels[tiles.data.tile_id] = Color32.op_Implicit(Color.blue);
        this._tiles.Add(tiles);
      }
    }
  }

  private void drawTargetedBy()
  {
    this.used = true;
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.isTargeted())
      {
        this.pixels[tiles.data.tile_id] = Color32.op_Implicit(Color.blue);
        this._tiles.Add(tiles);
      }
    }
  }

  private void drawProfession(UnitProfession pPro)
  {
    this.used = true;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.isProfession(pPro))
      {
        Color blue = Color.blue;
        this.pixels[unit.current_tile.data.tile_id] = Color32.op_Implicit(blue);
        this._tiles.Add(unit.current_tile);
      }
    }
  }

  private void drawCitizenJobs(string pID)
  {
    this.used = true;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.ai.job != null && !(pID != unit.ai.job.id))
      {
        Color red = Color.red;
        this.pixels[unit.current_tile.data.tile_id] = Color32.op_Implicit(red);
        this._tiles.Add(unit.current_tile);
      }
    }
  }

  private void drawUnitsInside()
  {
    this.used = true;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.is_inside_building)
      {
        this.pixels[unit.current_tile.data.tile_id] = Color32.op_Implicit(Color.green);
        this._tiles.Add(unit.current_tile);
      }
    }
  }

  private void drawConstructionTiles()
  {
    this.used = true;
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.hasBuilding() && tiles.building.asset.docks)
      {
        (TileZone[] tileZoneArray, int num) = Toolbox.getAllZonesFromTile(tiles);
        for (int index = 0; index < num; ++index)
        {
          TileZone pZone = tileZoneArray[index];
          foreach (WorldTile worldTile in tiles.building.checkZoneForDockConstruction(pZone))
          {
            this.pixels[worldTile.data.tile_id] = Color32.op_Implicit(Color.red);
            this._tiles.Add(worldTile);
          }
        }
      }
    }
  }

  private void drawFmodZones()
  {
    this.used = true;
    foreach (TileZone tileZone in DebugLayer.fmod_zones_to_draw)
      this.fill(tileZone.tiles, Color.yellow);
  }

  private void drawBuildings()
  {
    this.used = true;
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      if (tiles.hasBuilding())
      {
        if (tiles.building.kingdom != null && tiles.building.isKingdomCiv())
          this.pixels[tiles.data.tile_id] = tiles.building.kingdom.getColor().getColorMain32();
        else
          this.pixels[tiles.data.tile_id] = Color32.op_Implicit(Color.red);
        this.pixels[tiles.building.current_tile.data.tile_id] = Color32.op_Implicit(Color.magenta);
        this.pixels[tiles.building.door_tile.data.tile_id] = Color32.op_Implicit(Color.yellow);
        this._tiles.Add(tiles.building.current_tile);
        this._tiles.Add(tiles.building.door_tile);
        this._tiles.Add(tiles);
      }
    }
  }

  private void drawCityCenterZones()
  {
    this.used = true;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      WorldTile tile = city.getTile();
      if (tile != null)
        this.fill(tile.zone.tiles, Color.red);
    }
  }

  private void drawCityFarmZones()
  {
    this.used = true;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      this.fill(city.calculated_place_for_farms.getSimpleList(), Color.blue);
      this.fill(city.calculated_farm_fields.getSimpleList(), Color.cyan);
      this.fill(city.calculated_crops.getSimpleList(), Color.green);
      this.fill(city.calculated_grown_wheat.getSimpleList(), Color.yellow);
    }
  }

  private void drawVisibleZones()
  {
    this.used = true;
    List<TileZone> visibleZones = World.world.zone_camera.getVisibleZones();
    for (int index = 0; index < visibleZones.Count; ++index)
    {
      TileZone tileZone = visibleZones[index];
      if (tileZone.visible_main_centered)
        this.fill(tileZone.tiles, Color.green);
      else if (tileZone.visible)
        this.fill(tileZone.tiles, Color.blue);
    }
  }

  private void drawCityDangerZones()
  {
    this.used = true;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      foreach (TileZone dangerZone in city.danger_zones)
        this.fill(dangerZone.tiles, Color.red);
    }
  }

  private void drawCityPlaces()
  {
    this.used = true;
    foreach (TileZone zone in World.world.zone_calculator.zones)
    {
      if (zone.city != null)
        this.fill(zone.tiles, Color.yellow);
      else if (zone.isGoodForNewCity())
        this.fill(zone.tiles, Color.blue);
    }
  }

  private void drawActivePaths()
  {
    this.used = true;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      if (unit.current_path_global != null)
      {
        this.drawRegionPath(unit.current_path_global);
        this.fill(unit.current_path, Color.blue);
      }
    }
  }

  public void drawRegionPath(List<MapRegion> pRegions)
  {
    this.used = true;
    foreach (MapRegion pRegion in pRegions)
      this.fill(pRegion.tiles, this.color_active_path);
  }

  public void forceDrawRegionPath(List<MapRegion> pRegions)
  {
    this._forced_global_path.Clear();
    this._forced_global_path.AddRange((IEnumerable<MapRegion>) pRegions);
  }

  private void drawPathRegions()
  {
    this.used = true;
    foreach (MapChunk chunk in World.world.map_chunk_manager.chunks)
    {
      foreach (MapRegion region in chunk.regions)
      {
        if (region.path_wave_id != -1)
          this.fill(region.tiles, new Color(1f, 1f, 0.0f, 0.9f));
      }
    }
    List<MapRegion> lastGlobalPath = World.world.region_path_finder.last_globalPath;
    // ISSUE: explicit non-virtual call
    if ((lastGlobalPath != null ? (__nonvirtual (lastGlobalPath.Count) > 0 ? 1 : 0) : 0) == 0 || World.world.region_path_finder?.tileStart?.region == null || World.world.region_path_finder?.tileTarget?.region == null)
      return;
    foreach (MapRegion mapRegion in World.world.region_path_finder.last_globalPath)
      this.fill(mapRegion.tiles, Color.blue);
    this.fill(World.world.region_path_finder.tileStart.region.tiles, Color.green);
    this.fill(World.world.region_path_finder.tileTarget.region.tiles, new Color(1f, 0.0f, 0.0f, 0.3f));
  }

  private void fill(List<WorldTile> pTiles, Color pColor, bool pEdge = false)
  {
    this.createTextureNew();
    for (int index = 0; index < pTiles.Count; ++index)
    {
      WorldTile pTile = pTiles[index];
      if (!pEdge || pTile.region != null)
      {
        this._tiles.Add(pTile);
        this.pixels[pTile.data.tile_id] = Color32.op_Implicit(pColor);
      }
    }
  }

  private void fill(WorldTile[] pTiles, Color pColor, bool pEdge = false)
  {
    this.createTextureNew();
    for (int index = 0; index < pTiles.Length; ++index)
    {
      WorldTile pTile = pTiles[index];
      if (!pEdge || pTile.region != null)
      {
        this._tiles.Add(pTile);
        this.pixels[pTile.data.tile_id] = Color32.op_Implicit(pColor);
      }
    }
  }

  private void drawZones()
  {
    this.used = true;
    foreach (TileZone zone in World.world.zone_calculator.zones)
    {
      zone.debug_zone_color = (zone.x + zone.y) % 2 != 0 ? this.color2 : this.color1;
      this.fill(zone.tiles, zone.debug_zone_color);
    }
  }

  private void testCityLayout()
  {
    DebugVariables instance = DebugVariables.instance;
    if ((instance != null ? (!instance.layout_city_test ? 1 : 0) : 0) != 0)
      return;
    this.used = true;
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    if (mouseTilePos == null)
      return;
    TileZone zone1 = mouseTilePos?.zone;
    foreach (TileZone zone2 in World.world.zone_calculator.zones)
    {
      bool flag = true;
      if (!TownPlans.debugVisualizeZone(zone2, zone1))
        flag = false;
      zone2.debug_zone_color = !flag ? this.color_red : this.color1;
      this.fill(zone2.tiles, zone2.debug_zone_color);
    }
  }

  private void drawChunks()
  {
    this.used = true;
    foreach (MapChunk chunk in World.world.map_chunk_manager.chunks)
      this.fill(chunk.tiles, chunk.color);
  }

  internal override void clear()
  {
    HashSet<WorldTile> tiles = this._tiles;
    if (tiles.Count == 0)
      return;
    foreach (WorldTile worldTile in tiles)
    {
      if (worldTile.data.tile_id <= this.pixels.Length - 1)
        this.pixels[worldTile.data.tile_id] = Color32.op_Implicit(Color.clear);
    }
    this._tiles.Clear();
    this.createTextureNew();
  }
}
