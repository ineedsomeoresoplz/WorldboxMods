// Decompiled with JetBrains decompiler
// Type: BuildingZonesSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BuildingZonesSystem
{
  private static bool _dirty;

  public static void setDirty() => BuildingZonesSystem._dirty = true;

  public static void update()
  {
    if (!BuildingZonesSystem._dirty)
      return;
    BuildingZonesSystem._dirty = false;
    List<TileZone> zones = World.world.zone_calculator.zones;
    using (ListPool<TileZone> listPool = new ListPool<TileZone>())
    {
      for (int index = 0; index < zones.Count; ++index)
      {
        TileZone tileZone = zones[index];
        if (tileZone.isDirty())
          listPool.Add(tileZone);
      }
      for (int index = 0; index < listPool.Count; ++index)
      {
        TileZone tileZone = listPool[index];
        tileZone.clearBuildingLists();
        tileZone.setDirty(false);
        foreach (Building pBuilding in tileZone.buildings_all)
        {
          if (!pBuilding.isOnRemove() && !pBuilding.isRemoved())
          {
            if (pBuilding.current_tile.zone == tileZone)
              tileZone.buildings_render_list.Add(pBuilding);
            tileZone.addBuildingToSet(pBuilding);
            if (pBuilding.asset.city_building && !tileZone.hasCity())
            {
              if (pBuilding.isCiv())
                pBuilding.makeAbandoned();
              else
                pBuilding.makeAbandoned();
            }
          }
        }
      }
    }
  }
}
