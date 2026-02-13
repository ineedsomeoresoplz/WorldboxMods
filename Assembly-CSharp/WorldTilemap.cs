// Decompiled with JetBrains decompiler
// Type: WorldTilemap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

#nullable disable
public class WorldTilemap : BaseMapObject
{
  private const int EMPTY_Z = -1000;
  public static readonly Vector3Int EMPTY_TILE_POS = new Vector3Int(-1, -1, -1000);
  private Dictionary<int, TilemapExtended> _layers;
  [SerializeField]
  private TilemapExtended _prefab_tilemap_layer;
  [SerializeField]
  private Material _water_rims_material;
  private TileType _asset_border_water_outline;
  private TileType _asset_border_water_runup;
  private TileType _asset_border_pit;
  private TilemapExtended _layer_border_water_runup;
  private TilemapExtended _layer_water_outline;
  private readonly HashSet<TileZone> _dirty_zones = new HashSet<TileZone>();
  private readonly List<TileZone> _clear_list_zones = new List<TileZone>();
  private HashSet<WorldTile>[] _tiles_by_zone;
  private readonly Color _color_border_water_runup_default = Toolbox.makeColor("#DDFCFF", 0.7f);
  private float _color_water_runup_alpha_current = 0.4f;
  private float _color_water_runup_timer;
  private bool _color_water_runup_state_fade_in = true;
  private const float WATER_RUNUP_INTERVAL = 0.01f;
  private const float WATER_RUNUP_SPEED_CHANGE = 0.6f;
  private const float COLOR_WATER_RUNUP_ALPHA_BOUND_MIN = 0.02f;
  private const float COLOR_WATER_RUNUP_ALPHA_BOUND_M = 0.7f;

  internal override void create()
  {
    base.create();
    this._layers = new Dictionary<int, TilemapExtended>();
    this._asset_border_water_outline = AssetManager.tiles.get("border_water");
    this._asset_border_water_runup = AssetManager.tiles.get("border_water_runup");
    this._asset_border_pit = AssetManager.tiles.get("border_pit");
    for (int index = 0; index < AssetManager.tiles.list.Count; ++index)
      this.createTileMapFor((TileTypeBase) AssetManager.tiles.list[index]);
    for (int index = 0; index < AssetManager.top_tiles.list.Count; ++index)
      this.createTileMapFor((TileTypeBase) AssetManager.top_tiles.list[index]);
    this._layer_border_water_runup = this._layers[this._asset_border_water_runup.render_z];
    this._layer_water_outline = this._layers[this._asset_border_water_outline.render_z];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool needsRedraw(WorldTile pTile) => pTile.last_rendered_tile_type != pTile.Type;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void addToQueueToRedraw(WorldTile pTile)
  {
    TileZone zone = pTile.zone;
    this._dirty_zones.Add(zone);
    this._tiles_by_zone[zone.id].Add(pTile);
  }

  private void createTileMapFor(TileTypeBase pTileBase)
  {
    if (this._layers.ContainsKey(pTileBase.render_z))
      return;
    TilemapExtended tilemapExtended = Object.Instantiate<TilemapExtended>(this._prefab_tilemap_layer, ((Component) this).transform);
    tilemapExtended.create(pTileBase);
    if (pTileBase.id == "border_water_runup")
      ((Renderer) ((Component) tilemapExtended).GetComponent<TilemapRenderer>()).sharedMaterial = this._water_rims_material;
    this._layers.Add(pTileBase.render_z, tilemapExtended);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (World.world.isPaused())
      return;
    this.updateWaterRunup(Time.deltaTime);
  }

  private void updateWaterRunup(float pElapsed)
  {
    if ((double) this._color_water_runup_timer > 0.0)
    {
      this._color_water_runup_timer -= pElapsed;
    }
    else
    {
      this._color_water_runup_timer = 0.01f;
      if (this._color_water_runup_state_fade_in)
      {
        this._color_water_runup_alpha_current += pElapsed * 0.6f;
        if ((double) this._color_water_runup_alpha_current >= 0.699999988079071)
        {
          this._color_water_runup_alpha_current = 0.7f;
          this._color_water_runup_state_fade_in = false;
        }
      }
      else
      {
        this._color_water_runup_alpha_current -= pElapsed * 0.6f;
        if ((double) this._color_water_runup_alpha_current <= 0.019999999552965164)
        {
          this._color_water_runup_alpha_current = 0.02f;
          this._color_water_runup_state_fade_in = true;
        }
      }
      float nightMod = World.world.era_manager.getNightMod();
      Color color = Toolbox.blendColor(Color32.op_Implicit(Toolbox.color_night), this._color_border_water_runup_default, nightMod);
      color.a = this._color_water_runup_alpha_current;
      this._water_rims_material.color = color;
    }
  }

  internal void clear()
  {
    if (this._tiles_by_zone != null)
    {
      foreach (HashSet<WorldTile> worldTileSet in this._tiles_by_zone)
        worldTileSet.Clear();
    }
    this._dirty_zones.Clear();
    this._clear_list_zones.Clear();
    foreach (TilemapExtended tilemapExtended in this._layers.Values)
      tilemapExtended.clear();
  }

  internal void generate(int pCount)
  {
    this._tiles_by_zone = new HashSet<WorldTile>[pCount];
    for (int index = 0; index < pCount; ++index)
      this._tiles_by_zone[index] = new HashSet<WorldTile>(64 /*0x40*/);
  }

  private void prepareToDraw()
  {
    foreach (TilemapExtended tilemapExtended in this._layers.Values)
      tilemapExtended.prepareDraw();
  }

  internal void redrawTiles(bool pForceAll = false)
  {
    if (this._dirty_zones.Count == 0 || !(MapBox.isRenderGameplay() | pForceAll))
      return;
    this.prepareToDraw();
    if (pForceAll)
    {
      foreach (TileZone dirtyZone in this._dirty_zones)
        this.checkZoneToRender(dirtyZone);
    }
    else
    {
      List<TileZone> visibleZones = World.world.zone_camera.getVisibleZones();
      for (int index = 0; index < visibleZones.Count; ++index)
        this.checkZoneToRender(visibleZones[index]);
    }
    if (pForceAll)
    {
      this._clear_list_zones.Clear();
      this._dirty_zones.Clear();
    }
    this.redrawAllLayers();
    this.drawFinish();
  }

  private void drawFinish()
  {
    this._dirty_zones.ExceptWith((IEnumerable<TileZone>) this._clear_list_zones);
    this._clear_list_zones.Clear();
  }

  private void redrawAllLayers()
  {
    foreach (TilemapExtended tilemapExtended in this._layers.Values)
      tilemapExtended.redraw();
  }

  private void checkZoneToRender(TileZone pZone)
  {
    if (!this._dirty_zones.Contains(pZone))
      return;
    HashSet<WorldTile> worldTileSet = this._tiles_by_zone[pZone.id];
    foreach (WorldTile pTile in worldTileSet)
      this.renderTile(pTile);
    this._clear_list_zones.Add(pZone);
    worldTileSet.Clear();
  }

  private void renderTile(WorldTile pTile)
  {
    TileTypeBase tileTypeBase = (TileTypeBase) pTile.main_type;
    if (pTile.Type != null)
      tileTypeBase = pTile.Type;
    int renderZ = tileTypeBase.render_z;
    Vector3Int pPosition1;
    ref Vector3Int local1 = ref pPosition1;
    Vector2Int pos1 = pTile.pos;
    int x = ((Vector2Int) ref pos1).x;
    Vector2Int pos2 = pTile.pos;
    int y1 = ((Vector2Int) ref pos2).y;
    int num = renderZ;
    // ISSUE: explicit constructor call
    ((Vector3Int) ref local1).\u002Ector(x, y1, num);
    Vector3Int lastRenderedPosTile = pTile.last_rendered_pos_tile;
    int z1 = ((Vector3Int) ref lastRenderedPosTile).z;
    if (((Vector3Int) ref pPosition1).z != z1 || pTile.last_rendered_tile_type != tileTypeBase)
    {
      if (z1 != -1000)
      {
        this._layers[z1].addToQueueToRedraw(pTile, lastRenderedPosTile, (TileBase) null);
        pTile.last_rendered_pos_tile = WorldTilemap.EMPTY_TILE_POS;
      }
      pTile.last_rendered_tile_type = tileTypeBase;
      TilemapExtended layer = this._layers[((Vector3Int) ref pPosition1).z];
      Tile variation = this.getVariation(pTile);
      WorldTile pWorldTile = pTile;
      Vector3Int pPosition2 = pPosition1;
      Tile pTileGraphics = variation;
      layer.addToQueueToRedraw(pWorldTile, pPosition2, (TileBase) pTileGraphics);
      pTile.last_rendered_pos_tile = pPosition1;
    }
    Vector3Int renderedBorderPosOcean = pTile.last_rendered_border_pos_ocean;
    int z2 = ((Vector3Int) ref renderedBorderPosOcean).z;
    if (z2 != -1000)
    {
      this._layers[z2].addToQueueToRedraw(pTile, renderedBorderPosOcean, (TileBase) null);
      pTile.last_rendered_border_pos_ocean = WorldTilemap.EMPTY_TILE_POS;
      this._layer_border_water_runup.addToQueueToRedraw(pTile, lastRenderedPosTile, (TileBase) null, true);
    }
    if (!pTile.main_type.ground && !pTile.main_type.block || pTile.main_type.can_be_filled_with_ocean)
      return;
    TileType tileType = (TileType) null;
    bool flag = false;
    if (pTile.has_tile_down && pTile.tile_down.main_type.can_be_filled_with_ocean)
    {
      tileType = this._asset_border_pit;
      renderZ = tileType.render_z;
    }
    else if (pTile.isWaterAround())
    {
      tileType = this._asset_border_water_outline;
      renderZ = tileType.render_z;
      flag = true;
    }
    if (tileType == null)
      return;
    TilemapExtended layer1 = this._layers[renderZ];
    ref Vector3Int local2 = ref pPosition1;
    Vector2Int pos3 = pTile.pos;
    int y2 = ((Vector2Int) ref pos3).y;
    ((Vector3Int) ref local2).y = y2;
    ((Vector3Int) ref pPosition1).z = renderZ;
    WorldTile pWorldTile1 = pTile;
    Vector3Int pPosition3 = pPosition1;
    Tile main = tileType.sprites.main;
    layer1.addToQueueToRedraw(pWorldTile1, pPosition3, (TileBase) main);
    pTile.last_rendered_border_pos_ocean = pPosition1;
    if (!flag)
      return;
    this._layer_border_water_runup.addToQueueToRedraw(pTile, pPosition1, (TileBase) this._asset_border_water_runup.sprites.main, true);
  }

  internal void enableTiles(bool pValue)
  {
    if (((Component) this).gameObject.activeSelf == pValue)
      return;
    ((Component) this).gameObject.SetActive(pValue);
  }

  private Tile getVariation(WorldTile pTile)
  {
    TileSprites sprites = pTile.main_type.sprites;
    if (pTile.Type != null)
      sprites = pTile.Type.sprites;
    return pTile.Type.force_edge_variation && pTile.has_tile_up && pTile.tile_up.Type != pTile.Type ? pTile.Type.sprites.getVariation(pTile.Type.force_edge_variation_frame) : sprites.getRandom();
  }

  internal void debug(DebugTool pTool)
  {
    pTool.setText("_dirty_zones", (object) this._dirty_zones.Count);
    pTool.setText("_clear_list_zones", (object) this._clear_list_zones.Count);
  }

  public void checkEnableForWaterRunups(bool pIsLowRes)
  {
    if (pIsLowRes)
    {
      if (!((Component) this._layer_border_water_runup).gameObject.activeSelf)
        return;
      ((Component) this._layer_border_water_runup).gameObject.SetActive(false);
      ((Component) this._layer_water_outline).gameObject.SetActive(false);
    }
    else
    {
      if (((Component) this._layer_border_water_runup).gameObject.activeSelf)
        return;
      ((Component) this._layer_border_water_runup).gameObject.SetActive(true);
      ((Component) this._layer_water_outline).gameObject.SetActive(true);
    }
  }
}
