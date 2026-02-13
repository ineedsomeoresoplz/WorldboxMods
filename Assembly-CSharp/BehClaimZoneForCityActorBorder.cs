// Decompiled with JetBrains decompiler
// Type: BehClaimZoneForCityActorBorder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehClaimZoneForCityActorBorder : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    return BehClaimZoneForCityActorBorder.tryClaimZone(pActor);
  }

  public static BehResult tryClaimZone(Actor pActor)
  {
    TileZone zone = pActor.current_tile.zone;
    City city = pActor.city;
    WorldTile tile = city.getTile();
    if (tile == null || !city.isZoneToClaimStillGood(pActor, zone, tile))
      return BehResult.Stop;
    bool flag = pActor.hasCultureTrait("expansionists") || DebugConfig.isOn(DebugOption.CityFastZonesGrowth);
    int num = zone.city == null ? 0 : (zone.city != city ? 1 : 0);
    city.addZone(zone);
    if (num != 0)
      flag = false;
    if (flag)
    {
      foreach (TileZone pZone in zone.neighbours_all)
      {
        if (!pZone.hasCity() && pZone.centerTile.isSameIsland(tile) && city.isZoneToClaimStillGood(pActor, pZone, tile))
          city.addZone(pZone);
      }
    }
    pActor.addLoot(SimGlobals.m.coins_for_zone);
    return BehResult.Continue;
  }
}
