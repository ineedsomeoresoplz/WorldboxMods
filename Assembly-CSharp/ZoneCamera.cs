// Decompiled with JetBrains decompiler
// Type: ZoneCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ZoneCamera
{
  private readonly List<TileZone> _visible_zones = new List<TileZone>();
  private readonly HashSet<MapChunk> _set_visible_chunks = new HashSet<MapChunk>();
  private readonly List<MapChunk> _list_visible_chunks = new List<MapChunk>();
  private readonly ZoneCalculator _zone_manager;
  private int _last_start_x = -1;
  private int _last_start_y = -1;
  private int _last_width = -1;
  private int _last_height = -1;
  private int _last_main_x = -1;

  public ZoneCamera() => this._zone_manager = World.world.zone_calculator;

  private void calculateBounds(
    out int pResultStartX,
    out int pResultStartY,
    out int pResultWidth,
    out int pResultHeight,
    out int pResultMainX)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = num1 + 1;
    int num5 = num3 + 1;
    TileZone zoneWithinCamera1 = this.getZoneWithinCamera(0, 0);
    TileZone zoneWithinCamera2 = this.getZoneWithinCamera(1, 0);
    TileZone zoneWithinCamera3 = this.getZoneWithinCamera(0, 1);
    TileZone zoneWithinCamera4 = this.getZoneWithinCamera(1, 1);
    int num6 = zoneWithinCamera3.x <= zoneWithinCamera1.x ? zoneWithinCamera1.x : zoneWithinCamera3.x;
    int num7 = zoneWithinCamera2.y <= zoneWithinCamera1.y ? zoneWithinCamera1.y : zoneWithinCamera2.y;
    int num8 = num6 - num4;
    int num9 = num7 - num2;
    int num10 = zoneWithinCamera4.x - num8 + num4;
    int num11 = zoneWithinCamera4.y - num9 + num5;
    if (zoneWithinCamera2.x < zoneWithinCamera4.x)
      num10 = zoneWithinCamera2.x - num8;
    if (zoneWithinCamera3.y < zoneWithinCamera4.y)
      num11 = zoneWithinCamera3.y - num9;
    int num12 = num8;
    if (num12 > 0)
      ++num12;
    if (num8 < 0)
      num8 = 0;
    if (num9 < 0)
      num9 = 0;
    if (num10 > this._zone_manager.zones_total_x)
      num10 = this._zone_manager.zones_total_x;
    if (num11 > this._zone_manager.zones_total_y)
      num11 = this._zone_manager.zones_total_y;
    pResultStartX = num8;
    pResultStartY = num9;
    pResultWidth = num10;
    pResultHeight = num11;
    pResultMainX = num12;
  }

  internal void update()
  {
    Bench.bench("zone_camera", "zone_camera_total");
    Bench.bench("calc_bounds", "zone_camera");
    int pResultStartX;
    int pResultStartY;
    int pResultWidth;
    int pResultHeight;
    int pResultMainX;
    this.calculateBounds(out pResultStartX, out pResultStartY, out pResultWidth, out pResultHeight, out pResultMainX);
    Bench.benchEnd("calc_bounds", "zone_camera");
    if (pResultStartX == this._last_start_x && pResultStartY == this._last_start_y && pResultWidth == this._last_width && pResultHeight == this._last_height && pResultMainX == this._last_main_x)
      return;
    this._last_start_x = pResultStartX;
    this._last_start_y = pResultStartY;
    this._last_width = pResultWidth;
    this._last_height = pResultHeight;
    this._last_main_x = pResultMainX;
    Bench.bench("clear", "zone_camera");
    this.clear();
    Bench.benchEnd("clear", "zone_camera");
    Bench.bench("fill", "zone_camera");
    this.fillVisibleZones(pResultStartX, pResultStartY, pResultWidth, pResultHeight, pResultMainX);
    Bench.benchEnd("fill", "zone_camera");
    Bench.benchEnd("zone_camera", "zone_camera_total");
  }

  private void fillVisibleZones(int pStartX, int pStartY, int pWidth, int pHeight, int pMainX)
  {
    int zonesTotalX = this._zone_manager.zones_total_x;
    int zonesTotalY = this._zone_manager.zones_total_y;
    HashSet<MapChunk> setVisibleChunks = this._set_visible_chunks;
    List<TileZone> visibleZones = this._visible_zones;
    float powerBarPositionY = World.world.move_camera.power_bar_position_y;
    if (pStartX == 0 && pStartY == 0 && pWidth == zonesTotalX && pHeight == zonesTotalY)
    {
      visibleZones.AddRange((IEnumerable<TileZone>) this._zone_manager.zones);
      foreach (TileZone tileZone in visibleZones)
      {
        tileZone.visible = true;
        tileZone.visible_main_centered = true;
      }
      this._list_visible_chunks.AddRange((IEnumerable<MapChunk>) World.world.map_chunk_manager.chunks);
      setVisibleChunks.UnionWith((IEnumerable<MapChunk>) this._list_visible_chunks);
    }
    else
    {
      for (int index1 = 0; index1 <= pWidth; ++index1)
      {
        for (int index2 = 0; index2 <= pHeight; ++index2)
        {
          int pX = pStartX + index1;
          if (pX >= 0 && pX < zonesTotalX)
          {
            int pY = pStartY + index2;
            if (pY >= 0 && pY < zonesTotalY)
            {
              TileZone zoneUnsafe = this._zone_manager.getZoneUnsafe(pX, pY);
              visibleZones.Add(zoneUnsafe);
              setVisibleChunks.Add(zoneUnsafe.chunk);
              zoneUnsafe.visible = true;
              if (pX >= pMainX && index1 < pWidth && index2 < pHeight && (double) zoneUnsafe.top_left_corner_tile.y >= (double) powerBarPositionY)
                zoneUnsafe.visible_main_centered = true;
            }
          }
        }
      }
      this._list_visible_chunks.AddRange((IEnumerable<MapChunk>) setVisibleChunks);
    }
  }

  public List<TileZone> getVisibleZones() => this._visible_zones;

  public List<MapChunk> getVisibleChunks() => this._list_visible_chunks;

  public bool hasVisibleZones() => this._visible_zones.Count > 0;

  public int countVisibleZones() => this._visible_zones.Count;

  private TileZone getZoneWithinCamera(int pX, int pY, float pBonusY = 0.0f)
  {
    Vector3 worldPoint = World.world.camera.ViewportToWorldPoint(new Vector3((float) pX, (float) pY, World.world.camera.nearClipPlane));
    int x = (int) worldPoint.x;
    int pY1 = (int) worldPoint.y + (int) ((double) pBonusY * 8.0);
    int pX1 = this._zone_manager.zones_total_x - 1;
    int pY2 = this._zone_manager.zones_total_y - 1;
    WorldTile tile = World.world.GetTile(x, pY1);
    if (tile != null)
      return tile.zone;
    if (pX == 0 && pY == 0)
      return this._zone_manager.getZone(0, 0);
    if (pX == 1 && pY == 1)
      return this._zone_manager.getZone(pX1, pY2);
    if (pX == 0 && pY == 1)
      return this._zone_manager.getZone(0, pY2);
    return pX == 1 && pY == 0 ? this._zone_manager.getZone(pX1, 0) : (TileZone) null;
  }

  public void clear()
  {
    List<TileZone> visibleZones = this._visible_zones;
    foreach (TileZone tileZone in visibleZones)
    {
      tileZone.visible = false;
      tileZone.visible_main_centered = false;
    }
    visibleZones.Clear();
    this._list_visible_chunks.Clear();
    this._set_visible_chunks.Clear();
  }

  public void fullClear()
  {
    this._visible_zones.Clear();
    this._last_start_x = -1;
    this._last_start_y = -1;
    this._last_width = -1;
    this._last_height = -1;
    this._last_main_x = -1;
  }
}
