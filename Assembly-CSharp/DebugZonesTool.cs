// Decompiled with JetBrains decompiler
// Type: DebugZonesTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class DebugZonesTool
{
  public static void actionGrowBorder()
  {
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    if (mouseTilePos == null)
      return;
    TileZone zone = mouseTilePos.zone;
    if (!zone.hasCity())
      return;
    World.world.city_zone_helper.city_growth.getZoneToClaim((Actor) null, zone.city);
  }

  public static void actionAbandonZones()
  {
    WorldTile mouseTilePos = World.world.getMouseTilePos();
    if (mouseTilePos == null)
      return;
    TileZone zone = mouseTilePos.zone;
    if (!zone.hasCity())
      return;
    Bench.bench("abandon_stuff", "meh");
    World.world.city_zone_helper.city_abandon.check(zone.city, true);
    Debug.Log((object) ("bench abandon: " + Bench.benchEnd("abandon_stuff", "meh").ToString()));
  }
}
