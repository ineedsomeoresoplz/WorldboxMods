// Decompiled with JetBrains decompiler
// Type: ZoneMetaDataVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class ZoneMetaDataVisualizer
{
  public const float FADE_TIME = 5f;
  public static readonly Dictionary<TileZone, ZoneMetaData> zone_data_dict = new Dictionary<TileZone, ZoneMetaData>();
  private static readonly List<TileZone> _to_remove = new List<TileZone>();
  private static MetaType _last_meta_type = MetaType.None;

  public static bool hasZoneData(TileZone pZone)
  {
    return ZoneMetaDataVisualizer.zone_data_dict.ContainsKey(pZone);
  }

  public static ZoneMetaData getZoneMetaData(TileZone pZone)
  {
    ZoneMetaData zoneMetaData;
    ZoneMetaDataVisualizer.zone_data_dict.TryGetValue(pZone, out zoneMetaData);
    return zoneMetaData;
  }

  public static ListPool<TileZone> getZonesWithMeta(IMetaObject pMeta)
  {
    ListPool<TileZone> zonesWithMeta = new ListPool<TileZone>();
    foreach (ZoneMetaData zoneMetaData in ZoneMetaDataVisualizer.zone_data_dict.Values)
    {
      if (zoneMetaData.meta_object == pMeta)
        zonesWithMeta.Add(zoneMetaData.zone);
    }
    return zonesWithMeta;
  }

  private static bool shouldUpdateEntry(ZoneMetaData pData, IMetaObject pNewMetaObject)
  {
    IMetaObject metaObject = pData.meta_object;
    return metaObject == null || metaObject.getMetaTypeAsset().map_mode != pNewMetaObject.getMetaTypeAsset().map_mode || pData.previous_priority_amount < pNewMetaObject.countUnits() || metaObject == pNewMetaObject;
  }

  public static void countMetaZone(TileZone pZone, IMetaObject pMetaObject, double pTimestamp)
  {
    ZoneMetaData pData;
    if (ZoneMetaDataVisualizer.zone_data_dict.TryGetValue(pZone, out pData))
    {
      if (!ZoneMetaDataVisualizer.shouldUpdateEntry(pData, pMetaObject))
        return;
      pData.meta_object = pMetaObject;
      pData.timestamp = pTimestamp;
      pData.previous_priority_amount = pMetaObject.countUnits();
      ZoneMetaDataVisualizer.zone_data_dict[pZone] = pData;
    }
    else
    {
      ZoneMetaData zoneMetaData = new ZoneMetaData()
      {
        meta_object = pMetaObject,
        zone = pZone,
        timestamp = pTimestamp,
        timestamp_new = pTimestamp,
        previous_priority_amount = pMetaObject.countUnits()
      };
      ZoneMetaDataVisualizer.zone_data_dict.Add(pZone, zoneMetaData);
    }
  }

  private static void start() => ZoneMetaDataVisualizer._to_remove.Clear();

  private static void checkDynamicZones()
  {
    MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
    if (cachedMapMetaAsset != null && cachedMapMetaAsset.map_mode != ZoneMetaDataVisualizer._last_meta_type)
    {
      ZoneMetaDataVisualizer.clearAll();
      ZoneMetaDataVisualizer._last_meta_type = cachedMapMetaAsset.map_mode;
    }
    if (cachedMapMetaAsset == null || !cachedMapMetaAsset.has_dynamic_zones || !cachedMapMetaAsset.isMetaZoneOptionSelectedFluid())
      return;
    cachedMapMetaAsset.dynamic_zones();
  }

  private static void clearOldAndDeadZones()
  {
    double curWorldTime = World.world.getCurWorldTime();
    List<TileZone> toRemove = ZoneMetaDataVisualizer._to_remove;
    foreach (KeyValuePair<TileZone, ZoneMetaData> keyValuePair in ZoneMetaDataVisualizer.zone_data_dict)
    {
      ZoneMetaData zoneMetaData = keyValuePair.Value;
      if (zoneMetaData.meta_object == null || !zoneMetaData.meta_object.isAlive())
        toRemove.Add(keyValuePair.Key);
      else if ((double) zoneMetaData.getDiffTime(curWorldTime) > 5.0)
        toRemove.Add(keyValuePair.Key);
    }
    foreach (TileZone key in toRemove)
      ZoneMetaDataVisualizer.zone_data_dict.Remove(key);
    ZoneMetaDataVisualizer._to_remove.Clear();
  }

  public static void updateMetaZones()
  {
    Bench.bench("fluid_zones_data", "fluid_zones_data_total");
    Bench.bench("start", "fluid_zones_data");
    ZoneMetaDataVisualizer.start();
    Bench.benchEnd("start", "fluid_zones_data");
    Bench.bench("checkDynamicZones", "fluid_zones_data");
    ZoneMetaDataVisualizer.checkDynamicZones();
    Bench.benchEnd("checkDynamicZones", "fluid_zones_data");
    Bench.bench("clearOldAndDeadZones", "fluid_zones_data");
    ZoneMetaDataVisualizer.clearOldAndDeadZones();
    Bench.benchEnd("clearOldAndDeadZones", "fluid_zones_data");
    Bench.bench("checkCenterTitles", "fluid_zones_data");
    ZoneMetaDataVisualizer.checkCenterTitles();
    Bench.benchEnd("checkCenterTitles", "fluid_zones_data");
    Bench.benchEnd("fluid_zones_data", "fluid_zones_data_total");
  }

  private static void checkCenterTitles()
  {
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) World.world.cultures)
      culture.updateTitleCenter();
  }

  public static void clearAll() => ZoneMetaDataVisualizer.zone_data_dict.Clear();
}
