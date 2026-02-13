// Decompiled with JetBrains decompiler
// Type: ZoneCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ZoneCalculator : MapLayer
{
  public readonly List<TileZone> zones = new List<TileZone>();
  private readonly Dictionary<int, TileZone> _zones_dict_id = new Dictionary<int, TileZone>();
  internal TileZone[,] map;
  private bool _dirty;
  private int _last_zone_state = -1;
  public float outline_animation;
  private bool _outline_animation_in;
  private float _cached_map_opacity;
  private bool _cached_ony_favorited_meta;
  private bool _cached_check_animation;
  private bool _cached_should_be_clear_color;
  private bool _last_should_be_clear_color;
  private Kingdom _last_selected_kingdom;
  public int zones_total_x;
  public int zones_total_y;
  private const float ALPHA_NON_FAVORITED_META = 0.5f;
  private const float ALPHA_NON_SELECTED_META = 0.6f;
  public Color color1 = Color.gray;
  public Color color2 = Color.white;
  private readonly Color32 _color_clear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0);
  private readonly HashSetTileZone _to_clean_up = new HashSetTileZone();
  private float _night_multiplier = 1f;
  private int _debug_redrawn_last;
  private float _redraw_timer;
  private bool _dirty_draw_zones;
  public float minimap_opacity = 1f;
  public float border_brightness = 1f;
  private MetaTypeAsset _mode_asset;
  private bool _selection_changed_dirty;
  private NanoObject _cursor_nano_object;
  private NanoObject _selected_nano_object;
  private readonly HashSetTileZone _current_drawn_zones = new HashSetTileZone();
  private int _debug_redrawn_last_amount;

  internal override void create()
  {
    base.create();
    this.colorValues = new Color(1f, 0.46f, 0.19f, 1f);
    Color color = this.sprRnd.color;
    color.a = 0.78f;
    this.sprRnd.color = color;
  }

  public void clearDebug()
  {
    if (!DebugConfig.isOn(DebugOption.DebugZones))
      return;
    for (int index = 0; index < this.zones.Count; ++index)
      this.zones[index].clearDebug();
  }

  internal override void clear()
  {
    base.clear();
    for (int index = 0; index < this.zones.Count; ++index)
      this.zones[index].clear();
    this._current_drawn_zones.Clear();
    this._to_clean_up.Clear();
    this._last_selected_kingdom = (Kingdom) null;
  }

  public void clean()
  {
    foreach (TileZone zone in this.zones)
      zone.Dispose();
    this.zones.Clear();
    this._zones_dict_id.Clear();
    this.map = (TileZone[,]) null;
  }

  public void generate()
  {
    this.zones.Clear();
    this._zones_dict_id.Clear();
    int num1 = 8;
    this.zones_total_x = Config.ZONE_AMOUNT_X * num1;
    this.zones_total_y = Config.ZONE_AMOUNT_Y * num1;
    this.map = new TileZone[this.zones_total_x, this.zones_total_y];
    int num2 = 0;
    for (int index1 = 0; index1 < this.zones_total_y; ++index1)
    {
      for (int index2 = 0; index2 < this.zones_total_x; ++index2)
      {
        TileZone pZone = new TileZone()
        {
          x = index2,
          y = index1,
          id = num2++
        };
        pZone.debug_zone_color = (index2 + index1) % 2 != 0 ? this.color2 : this.color1;
        this.map[index2, index1] = pZone;
        this.zones.Add(pZone);
        this._zones_dict_id.Add(pZone.id, pZone);
        this.fillZone(pZone);
      }
    }
    World.world.tilemap.generate(this.zones.Count);
    this.generateNeighbours();
    this.zones.Shuffle<TileZone>();
  }

  public TileZone getZoneByID(int pID) => this._zones_dict_id[pID];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public TileZone getZone(int pX, int pY)
  {
    return pX < 0 || pX >= this.zones_total_x || pY < 0 || pY >= this.zones_total_y ? (TileZone) null : this.map[pX, pY];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public TileZone getZoneUnsafe(int pX, int pY) => this.map[pX, pY];

  private void generateNeighbours()
  {
    using (ListPool<TileZone> listPool1 = new ListPool<TileZone>(4))
    {
      using (ListPool<TileZone> listPool2 = new ListPool<TileZone>(8))
      {
        List<TileZone> zones = this.zones;
        int count = zones.Count;
        for (int index = 0; index < count; ++index)
        {
          TileZone tileZone = zones[index];
          TileZone zone1 = this.getZone(tileZone.x - 1, tileZone.y);
          tileZone.addNeighbour(zone1, TileDirection.Left, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2);
          TileZone zone2 = this.getZone(tileZone.x + 1, tileZone.y);
          tileZone.addNeighbour(zone2, TileDirection.Right, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2);
          TileZone zone3 = this.getZone(tileZone.x, tileZone.y - 1);
          tileZone.addNeighbour(zone3, TileDirection.Down, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2);
          TileZone zone4 = this.getZone(tileZone.x, tileZone.y + 1);
          tileZone.addNeighbour(zone4, TileDirection.Up, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2);
          TileZone zone5 = this.getZone(tileZone.x - 1, tileZone.y - 1);
          tileZone.addNeighbour(zone5, TileDirection.Null, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2, true);
          TileZone zone6 = this.getZone(tileZone.x - 1, tileZone.y + 1);
          tileZone.addNeighbour(zone6, TileDirection.Null, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2, true);
          TileZone zone7 = this.getZone(tileZone.x + 1, tileZone.y - 1);
          tileZone.addNeighbour(zone7, TileDirection.Null, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2, true);
          TileZone zone8 = this.getZone(tileZone.x + 1, tileZone.y + 1);
          tileZone.addNeighbour(zone8, TileDirection.Null, (IList<TileZone>) listPool1, (IList<TileZone>) listPool2, true);
          tileZone.neighbours = listPool1.ToArray<TileZone>();
          tileZone.neighbours_all = listPool2.ToArray<TileZone>();
          listPool1.Clear();
          listPool2.Clear();
        }
      }
    }
  }

  private void fillZone(TileZone pZone)
  {
    int num1 = pZone.x * 8;
    int num2 = pZone.y * 8;
    int num3 = 4;
    for (int pX = 0; pX < 8; ++pX)
    {
      for (int pY = 0; pY < 8; ++pY)
      {
        WorldTile tile = World.world.GetTile(pX + num1, pY + num2);
        if (tile != null)
        {
          tile.zone = pZone;
          pZone.addTile(tile, pX, pY);
          if (pX == num3 && pY == num3)
            pZone.centerTile = tile;
        }
      }
    }
  }

  private void updateOutlineAnimation(float pElapsed)
  {
    if (this._selected_nano_object == null && this._cursor_nano_object == null)
    {
      this._outline_animation_in = true;
      this.outline_animation = 0.0f;
    }
    else if (this._outline_animation_in)
    {
      this.outline_animation += pElapsed * 2f;
      if ((double) this.outline_animation < 1.0)
        return;
      this.outline_animation = 1f;
      this._outline_animation_in = false;
    }
    else
    {
      this.outline_animation -= pElapsed * 2f;
      if ((double) this.outline_animation > 0.0)
        return;
      this.outline_animation = 0.0f;
      this._outline_animation_in = true;
    }
  }

  public void updateAnimationsAndSelections()
  {
    this._cached_map_opacity = this.getMapOpacity();
    this._cached_ony_favorited_meta = PlayerConfig.optionBoolEnabled("only_favorited_meta");
    this._cached_check_animation = !ScrollWindow.isWindowActive() && (!this._cursor_nano_object.isRekt() || !this._selected_nano_object.isRekt());
    this._cached_should_be_clear_color = this.shouldBeClearColor();
    this.checkCursorNanoObject();
    this.checkSelectedNanoObject();
    this.updateOutlineAnimation(World.world.delta_time);
    MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
    int num = cachedMapMetaAsset != null ? cachedMapMetaAsset.getZoneOptionState() : -1;
    this.checkClearAllZonesToRedraw();
    this.checkDrawnZonesDirty();
    this._last_should_be_clear_color = this._cached_should_be_clear_color;
    bool flag = Zones.showMapBorders();
    if (cachedMapMetaAsset != null & flag)
    {
      ((Renderer) this.sprRnd).enabled = true;
      this.setMode(cachedMapMetaAsset);
      this._last_zone_state = num;
    }
    else
    {
      this.setMode((MetaTypeAsset) null);
      ((Renderer) this.sprRnd).enabled = false;
    }
    Color white = Color.white;
    white.r = this.border_brightness;
    white.g = this.border_brightness;
    white.b = this.border_brightness;
    this._night_multiplier = !World.world.era_manager.getCurrentAge().overlay_darkness ? Mathf.Lerp(this._night_multiplier, 1f, World.world.delta_time * 0.5f) : Mathf.Lerp(this._night_multiplier, 0.6f, World.world.delta_time * 0.5f);
    white.a = this._cached_map_opacity;
    this.sprRnd.color = white;
  }

  public override void update(float pElapsed) => base.update(pElapsed);

  private void checkClearAllZonesToRedraw()
  {
    MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
    int num = cachedMapMetaAsset != null ? cachedMapMetaAsset.getZoneOptionState() : -1;
    if ((num == -1 || num == this._last_zone_state) && !this._selection_changed_dirty && this._last_should_be_clear_color == this._cached_should_be_clear_color)
      return;
    this.clearCurrentDrawnZones();
    this._selection_changed_dirty = false;
  }

  public override void draw(float pElapsed) => this.redrawZones();

  private static float getCameraScaleZoom()
  {
    return Mathf.Clamp(MoveCamera.instance.main_camera.orthographicSize / 20f, 1f, 30f);
  }

  private void setDirty() => this._dirty = true;

  private void setMode(MetaTypeAsset pAsset)
  {
    if (this._mode_asset != pAsset)
      this.clearAllRedrawTimers();
    this._mode_asset = pAsset;
  }

  public void clearAllRedrawTimers()
  {
    this.clearTimer();
    this.clearWorldBehaviourTimer();
  }

  public void clearTimer() => this._redraw_timer = 0.0f;

  public void clearWorldBehaviourTimer()
  {
    AssetManager.world_behaviours.get("zones_meta_data_visualizer").manager.timerClear();
  }

  public bool isModeNone() => this._mode_asset == null;

  private bool isBorderColor_relations(TileZone pZone, City pCity, bool pCheckFriendly = false)
  {
    if (pZone != null && pZone.city != pCity && pZone.city != null && pZone.city.kingdom == pCity.kingdom)
      return false;
    return pZone == null || pZone.city == null || pZone.city.kingdom != pCity.kingdom;
  }

  public void debug(DebugTool pTool)
  {
    if (this._debug_redrawn_last_amount != 0)
      this._debug_redrawn_last = this._debug_redrawn_last_amount;
    pTool.setText("_toCleanUp", (object) this._to_clean_up.Count);
    pTool.setText("_lastDrawnZones", (object) this._current_drawn_zones.Count);
    pTool.setText("redrawn_last", (object) this._debug_redrawn_last);
    pTool.setSeparator();
  }

  public TileZone getMapCenterZone() => this.map[this.zones_total_x / 2, this.zones_total_y / 2];

  public void drawZoneMeta(
    TileZone pZone,
    MetaTypeAsset pMetaTypeAsset,
    MetaZoneGetMetaSimple pZoneGetDelegate)
  {
    IMetaObject metaObject = pZoneGetDelegate(pZone);
    bool pUp = this.isBorderColorSameNanoObject(pZone.zone_up, pZoneGetDelegate, metaObject);
    bool pDown = this.isBorderColorSameNanoObject(pZone.zone_down, pZoneGetDelegate, metaObject);
    bool pLeft = this.isBorderColorSameNanoObject(pZone.zone_left, pZoneGetDelegate, metaObject);
    bool pRight = this.isBorderColorSameNanoObject(pZone.zone_right, pZoneGetDelegate, metaObject);
    MetaObjectData pMetaData = (MetaObjectData) null;
    if (metaObject != null)
      pMetaData = metaObject.getMetaData();
    this.drawZoneMeta(metaObject, pZone, pUp, pDown, pLeft, pRight, pMetaData, pMetaTypeAsset);
  }

  public void drawZoneAlliance(TileZone pZone, int pZoneOption)
  {
    Alliance allianceOnZone = pZone.getAllianceOnZone(pZoneOption);
    bool pUp = this.isBorderColor_alliance(pZone.zone_up, allianceOnZone, pZoneOption);
    bool pDown = this.isBorderColor_alliance(pZone.zone_down, allianceOnZone, pZoneOption);
    bool pLeft = this.isBorderColor_alliance(pZone.zone_left, allianceOnZone, pZoneOption);
    bool pRight = this.isBorderColor_alliance(pZone.zone_right, allianceOnZone, pZoneOption);
    this.drawZoneMeta((IMetaObject) allianceOnZone, pZone, pUp, pDown, pLeft, pRight, (MetaObjectData) allianceOnZone.data, MetaTypeLibrary.alliance);
  }

  private bool isBorderColor_alliance(
    TileZone pZone,
    Alliance pAlliance,
    int pZoneOption,
    bool pCheckFriendly = false)
  {
    if (pZone == null)
      return true;
    NanoObject allianceOnZone = (NanoObject) pZone.getAllianceOnZone(pZoneOption);
    return allianceOnZone == null || allianceOnZone != pAlliance;
  }

  private void drawZoneMeta(
    IMetaObject pMeta,
    TileZone pZone,
    bool pUp,
    bool pDown,
    bool pLeft,
    bool pRight,
    MetaObjectData pMetaData,
    MetaTypeAsset pMetaTypeAsset)
  {
    int pHashCode = -1;
    if (pMeta != null)
      pHashCode = pMeta.GetHashCode();
    int idForDraw = this.generateIdForDraw(this._mode_asset, pHashCode, pUp, pDown, pLeft, pRight);
    bool pLastAnimated = false;
    int pColorAssetID = 0;
    Color32 color32_1;
    Color32 color32_2;
    if (pMeta != null && pMeta.isAlive())
    {
      ColorAsset color = pMeta.getColor();
      pColorAssetID = color.index_id;
      color32_1 = color.getColorMainSecond32();
      color32_2 = !this._cached_should_be_clear_color ? color.getColorBorderInsideAlpha32() : this._color_clear;
      pLastAnimated = this.checkFadeAndSelectionColors(pZone, ref color32_2, ref color32_1, 0.0f, pMeta, pMetaTypeAsset, pMetaData.favorite);
    }
    else
    {
      color32_2 = this._color_clear;
      color32_1 = this._color_clear;
    }
    if (!pZone.checkShouldReRender(pHashCode, idForDraw, pColorAssetID, pLastAnimated))
      return;
    this.applyMetaColorsToZone(pZone, ref color32_2, ref color32_1, pUp, pDown, pLeft, pRight);
  }

  public void drawZoneCity(TileZone pZone)
  {
    City city = pZone.city;
    Kingdom kingdom = city.kingdom;
    ColorAsset color = city.getColor();
    Color32 color32 = color.getColorBorderInsideAlpha32();
    Color32 colorMainSecond32 = color.getColorMainSecond32();
    if (this._cached_should_be_clear_color)
      color32 = this._color_clear;
    bool pUp = this.isBorderColor_cities(pZone.zone_up, city, true);
    bool pDown = this.isBorderColor_cities(pZone.zone_down, city);
    bool pLeft = this.isBorderColor_cities(pZone.zone_left, city);
    bool pRight = this.isBorderColor_cities(pZone.zone_right, city, true);
    int hashCode = city.GetHashCode();
    int pID = this.generateIdForDraw(this._mode_asset, hashCode, pUp, pDown, pLeft, pRight) + kingdom.GetHashCode();
    bool pLastAnimated = this.checkFadeAndSelectionColors(pZone, ref color32, ref colorMainSecond32, 0.0f, (IMetaObject) city, MetaTypeLibrary.city, city.data.favorite);
    if (!pZone.checkShouldReRender(hashCode, pID, 0, pLastAnimated))
      return;
    this.applyMetaColorsToZone(pZone, ref color32, ref colorMainSecond32, pUp, pDown, pLeft, pRight);
  }

  private bool isBorderColorSameNanoObject(
    TileZone pZone,
    MetaZoneGetMetaSimple pZoneGetMetaDelegate,
    IMetaObject pNanoObjectMain)
  {
    if (pZone == null)
      return true;
    IMetaObject metaObject = pZoneGetMetaDelegate(pZone);
    return metaObject == null || metaObject != pNanoObjectMain;
  }

  private bool isBorderColor_cities(TileZone pZone, City pCityMain, bool pCheckFriendly = false)
  {
    if (pZone == null)
      return true;
    City city = pZone.city;
    Kingdom kingdom = city?.kingdom;
    if (pCheckFriendly && city != pCityMain && city != null && kingdom == pCityMain.kingdom)
      return false;
    return city == null || kingdom != pCityMain.kingdom || city != pCityMain;
  }

  private bool checkShouldDrawObject(bool pMetaFavorite)
  {
    return !this._cached_ony_favorited_meta || pMetaFavorite;
  }

  private float getMapOpacity()
  {
    return (!MapBox.isRenderMiniMap() ? Mathf.Clamp(ZoneCalculator.getCameraScaleZoom() * 0.3f, 0.0f, 0.78f) : this.minimap_opacity) * this._night_multiplier;
  }

  private bool shouldBeClearColor() => MapBox.isRenderGameplay();

  public void drawEnd(TileZone pZone)
  {
    this._current_drawn_zones.Add(pZone);
    this._to_clean_up.Remove(pZone);
  }

  private void checkCursorNanoObject()
  {
    NanoObject nanoObject = (NanoObject) null;
    if (MapBox.isRenderMiniMap() && !World.world.isOverUI())
    {
      WorldTile tilePosCachedFrame = World.world.getMouseTilePosCachedFrame();
      MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
      if (tilePosCachedFrame != null && cachedMapMetaAsset != null)
      {
        MetaZoneGetMeta tileGetMetaobject = cachedMapMetaAsset.tile_get_metaobject;
        nanoObject = (tileGetMetaobject != null ? tileGetMetaobject(tilePosCachedFrame.zone, cachedMapMetaAsset.getZoneOptionState()) : (IMetaObject) null) as NanoObject;
      }
    }
    if (this._cursor_nano_object != nanoObject)
      this._selection_changed_dirty = true;
    this._cursor_nano_object = nanoObject;
  }

  private void checkSelectedNanoObject()
  {
    NanoObject nanoObject = (NanoObject) null;
    if (MapBox.isRenderMiniMap())
    {
      MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
      if (cachedMapMetaAsset != null)
        nanoObject = cachedMapMetaAsset.get_selected();
    }
    if (nanoObject != this._selected_nano_object)
      this._selection_changed_dirty = true;
    this._selected_nano_object = nanoObject;
  }

  private void redrawZones()
  {
    Bench.bench("borders_renderer", "borders_renderer_total");
    Bench.bench("clearAllRedrawTimers", "borders_renderer");
    if (this._last_selected_kingdom != SelectedMetas.selected_kingdom)
    {
      this._last_selected_kingdom = SelectedMetas.selected_kingdom;
      this.clearAllRedrawTimers();
    }
    Bench.benchEnd("clearAllRedrawTimers", "borders_renderer");
    if ((double) this._redraw_timer > 0.0)
    {
      this._redraw_timer -= Time.deltaTime;
      Bench.clearBenchmarkEntrySkipMultiple("borders_renderer_total", "_to_clean_up.union", "draw_zones.Invoke", "clearDrawnZones", "updatePixels");
    }
    else
    {
      this._redraw_timer = 0.01f;
      Bench.bench("_to_clean_up.union", "borders_renderer");
      this._debug_redrawn_last_amount = 0;
      if (this._current_drawn_zones.Any<TileZone>())
        this._to_clean_up.UnionWith((IEnumerable<TileZone>) this._current_drawn_zones);
      Bench.benchEnd("_to_clean_up.union", "borders_renderer");
      Bench.bench("draw_zones.Invoke", "borders_renderer");
      if (this._mode_asset != null)
        this._mode_asset.draw_zones(this._mode_asset);
      Bench.benchEnd("draw_zones.Invoke", "borders_renderer");
      Bench.bench("clearDrawnZones", "borders_renderer");
      if (this._to_clean_up.Any<TileZone>())
        this.clearDrawnZones();
      Bench.benchEnd("clearDrawnZones", "borders_renderer");
      Bench.bench("updatePixels", "borders_renderer");
      if (this._dirty)
      {
        this._dirty = false;
        this.updatePixels();
      }
      Bench.benchEnd("updatePixels", "borders_renderer");
      Bench.benchEnd("borders_renderer", "borders_renderer_total");
    }
  }

  private void clearDrawnZones()
  {
    foreach (TileZone pZone in (HashSet<TileZone>) this._to_clean_up)
    {
      this.drawZoneClear(pZone);
      pZone.resetRenderHelpers();
      this._current_drawn_zones.Remove(pZone);
    }
    this._to_clean_up.Clear();
  }

  public void dirtyAndClear()
  {
    this.setDrawnZonesDirty();
    this.clearCurrentDrawnZones();
  }

  internal void clearCurrentDrawnZones(bool pCleanTimer = true)
  {
    foreach (TileZone currentDrawnZone in (HashSet<TileZone>) this._current_drawn_zones)
    {
      this.drawZoneClear(currentDrawnZone);
      currentDrawnZone.resetRenderHelpers();
    }
    this._current_drawn_zones.Clear();
    if (!pCleanTimer)
      return;
    this.clearAllRedrawTimers();
  }

  private int generateIdForDraw(
    MetaTypeAsset pModeAsset,
    int pHashCode,
    bool pUp,
    bool pDown,
    bool pLeft,
    bool pRight)
  {
    int idForDraw = (pModeAsset.GetHashCode() + 1) * 10000000;
    if (pUp)
      idForDraw += 100000;
    if (pDown)
      idForDraw += 10000;
    if (pLeft)
      idForDraw += 1000;
    if (pRight)
      idForDraw += 100;
    return idForDraw;
  }

  public MetaType getCurrentModeDebug()
  {
    MetaType pType = Zones.getForcedMapMode();
    if (pType.isNone())
    {
      MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
      if (cachedMapMetaAsset != null)
        pType = cachedMapMetaAsset.map_mode;
    }
    return pType;
  }

  private void applyAlphaFadeToColor(
    ref Color32 pColorBorderInside,
    ref Color32 pColorBorderOutside,
    MetaTypeAsset pMetaTypeAsset,
    float pDiff,
    int pUnits,
    double pTimestampNew)
  {
    float cachedMapOpacity = this._cached_map_opacity;
    float num1 = pDiff / 5f;
    float num2 = 0.6f;
    float num3 = (float) pColorBorderInside.a / (float) byte.MaxValue;
    float num4 = (float) pColorBorderOutside.a / (float) byte.MaxValue;
    float num5 = 1f;
    double timeElapsedSince = (double) World.world.getWorldTimeElapsedSince(pTimestampNew);
    float num6 = 1f - num1;
    float num7 = Mathf.Clamp01((float) (((double) num1 - (double) num2) / (1.0 - (double) num2)));
    double num8 = (1.0 - (double) num7 * (double) num7 * (double) num7) * ((double) cachedMapOpacity * (double) num5);
    float num9 = num6 * (cachedMapOpacity * num5);
    double num10 = (double) num3;
    byte num11 = (byte) (num8 * num10 * (double) byte.MaxValue);
    byte num12 = (byte) ((double) num9 * (double) num4 * (double) byte.MaxValue);
    pColorBorderInside.a = num11;
    pColorBorderOutside.a = num12;
  }

  private bool shouldShowSelectionFor(IMetaObject pNanoObject)
  {
    return pNanoObject == this._cursor_nano_object || pNanoObject == this._selected_nano_object;
  }

  private bool checkFadeAndSelectionColors(
    TileZone pZone,
    ref Color32 pColorBorderInside,
    ref Color32 pColorBorderOut,
    float pDiff,
    IMetaObject pMetaObjectToDraw,
    MetaTypeAsset pMetaTypeAsset,
    bool pFavorite)
  {
    bool flag1 = false;
    if (this._cached_check_animation)
    {
      if (this.shouldShowSelectionFor(pMetaObjectToDraw))
        flag1 = true;
      else if (!this._selected_nano_object.isRekt())
      {
        pColorBorderInside.a = (byte) ((double) pColorBorderInside.a * 0.60000002384185791);
        pColorBorderOut.a = (byte) ((double) pColorBorderOut.a * 0.60000002384185791);
      }
    }
    bool flag2 = flag1 || (!SelectedUnit.isSet() ? this.checkShouldDrawObject(pFavorite) : SelectedUnit.unit.getMetaObjectOfType(pMetaTypeAsset.map_mode) == pMetaObjectToDraw);
    if (!flag2)
    {
      pColorBorderInside.a = (byte) ((double) pColorBorderInside.a * 0.5);
      pColorBorderOut.a = (byte) ((double) pColorBorderOut.a * 0.5);
    }
    if (!flag2 && this._cached_should_be_clear_color)
      pColorBorderInside = this._color_clear;
    if (flag1)
    {
      pZone.resetRenderHelpers();
      float outlineAnimation = this.outline_animation;
      pColorBorderOut = Color32.Lerp(pColorBorderOut, Toolbox.color_black_32, outlineAnimation);
    }
    return flag1;
  }

  public void drawBegin()
  {
  }

  internal void setDrawnZonesDirty() => this._dirty_draw_zones = true;

  private void checkDrawnZonesDirty()
  {
    if (!this._dirty_draw_zones)
      return;
    this._dirty_draw_zones = false;
    this.clearAllRedrawTimers();
  }

  public void drawGenericFluid(ZoneMetaData pData, MetaTypeAsset pMetaTypeAsset)
  {
    TileZone zone = pData.zone;
    IMetaObject metaObject = pData.meta_object;
    bool pFavorite = metaObject.isFavorite();
    double curWorldTime = World.world.getCurWorldTime();
    float diffTime = pData.getDiffTime(curWorldTime);
    if ((double) diffTime > 5.0)
      return;
    ColorAsset color = metaObject.getColor();
    Color32 color32_1 = Color32.op_Implicit(color.getColorText());
    Color32 color32_2 = Color32.op_Implicit(color.getColorText());
    if ((double) diffTime != 0.0)
      this.applyAlphaFadeToColor(ref color32_1, ref color32_2, pMetaTypeAsset, diffTime, metaObject.countUnits(), pData.timestamp_new);
    bool pUp = false;
    bool pDown = false;
    bool pLeft = false;
    bool pRight = false;
    int num = this.shouldShowSelectionFor(metaObject) ? 1 : 0;
    if (num != 0)
    {
      pUp = this.isBorderNanoMetaFluid(zone.zone_up, metaObject, curWorldTime);
      pDown = this.isBorderNanoMetaFluid(zone.zone_down, metaObject, curWorldTime);
      pLeft = this.isBorderNanoMetaFluid(zone.zone_left, metaObject, curWorldTime);
      pRight = this.isBorderNanoMetaFluid(zone.zone_right, metaObject, curWorldTime);
    }
    int hashCode = metaObject.GetHashCode();
    int idForDraw = this.generateIdForDraw(this._mode_asset, hashCode, pUp, pDown, pLeft, pRight);
    zone.last_drawn_id = idForDraw;
    zone.last_drawn_hashcode = hashCode;
    this.checkFadeAndSelectionColors(zone, ref color32_1, ref color32_2, diffTime, metaObject, pMetaTypeAsset, pFavorite);
    if (num != 0)
      this.applyMetaColorsToZone(zone, ref color32_1, ref color32_2, pUp, pDown, pLeft, pRight);
    else
      this.applyMetaColorsToZoneFull(zone, ref color32_1);
  }

  private bool isBorderNanoMetaFluid(TileZone pZone, IMetaObject pMetaMain, double pCurTime)
  {
    if (pZone == null || !ZoneMetaDataVisualizer.hasZoneData(pZone))
      return true;
    ZoneMetaData zoneMetaData = ZoneMetaDataVisualizer.getZoneMetaData(pZone);
    return (double) zoneMetaData.getDiffTime(pCurTime) > 5.0 || zoneMetaData.meta_object != pMetaMain;
  }

  private void applyMetaColorsToZoneFull(TileZone pZone, ref Color32 pColor)
  {
    this.setDirty();
    WorldTile[] tiles = pZone.tiles;
    Color32[] pixels = this.pixels;
    int length = tiles.Length;
    Color32 color32 = pixels[tiles[0].data.tile_id];
    if ((int) color32.r == (int) pColor.r && (int) color32.g == (int) pColor.g && (int) color32.b == (int) pColor.b && (int) color32.a == (int) pColor.a)
      return;
    for (int index = 0; index < length; ++index)
    {
      int tileId = tiles[index].data.tile_id;
      pixels[tileId] = pColor;
    }
    ++this._debug_redrawn_last_amount;
  }

  private void applyMetaColorsToZone(
    TileZone pZone,
    ref Color32 pColorInside,
    ref Color32 pColorOutside,
    bool pUp,
    bool pDown,
    bool pLeft,
    bool pRight)
  {
    this.setDirty();
    WorldTile[] tiles = pZone.tiles;
    Color32[] pixels = this.pixels;
    int length = tiles.Length;
    for (int index = 0; index < length; ++index)
    {
      WorldTile worldTile = tiles[index];
      int tileId = worldTile.data.tile_id;
      WorldTileZoneBorder worldTileZoneBorder = worldTile.world_tile_zone_border;
      pixels[tileId] = worldTileZoneBorder.border ? (!pUp || !worldTileZoneBorder.border_up ? (!pDown || !worldTileZoneBorder.border_down ? (!pLeft || !worldTileZoneBorder.border_left ? (!pRight || !worldTileZoneBorder.border_right ? pColorInside : pColorOutside) : pColorOutside) : pColorOutside) : pColorOutside) : pColorInside;
    }
    ++this._debug_redrawn_last_amount;
  }

  private void drawZoneClear(TileZone pZone) => this.colorZone(pZone, Toolbox.clear);

  private void colorZone(TileZone pZone, Color32 pColor)
  {
    this.setDirty();
    Color32[] pixels = this.pixels;
    WorldTile[] tiles = pZone.tiles;
    int length = tiles.Length;
    Color32 color32 = pixels[tiles[0].data.tile_id];
    if ((int) color32.r == (int) pColor.r && (int) color32.g == (int) pColor.g && (int) color32.b == (int) pColor.b && (int) color32.a == (int) pColor.a)
      return;
    for (int index = 0; index < length; ++index)
    {
      int tileId = tiles[index].data.tile_id;
      pixels[tileId] = pColor;
    }
  }
}
