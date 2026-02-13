// Decompiled with JetBrains decompiler
// Type: CityBehBorderShrink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class CityBehBorderShrink : BehaviourActionCity
{
  public override bool errorsFound(City pCity) => false;

  public override bool shouldRetry(City pCity) => false;

  public override BehResult execute(City pCity)
  {
    if ((double) BehaviourActionBase<City>.world.getWorldTimeElapsedSince(pCity.timestamp_shrink) < (double) SimGlobals.m.empty_city_borders_shrink_time || pCity.hasUnits())
      return BehResult.Stop;
    TileZone zoneToRemove = this.getZoneToRemove(pCity);
    if (zoneToRemove == null)
      return BehResult.Stop;
    pCity.removeZone(zoneToRemove);
    pCity.timestamp_shrink = BehaviourActionBase<City>.world.getCurWorldTime();
    return BehResult.Continue;
  }

  private TileZone getZoneToRemove(City pCity)
  {
    TileZone zoneToRemove = (TileZone) null;
    if (pCity.border_zones.Count > 0)
      zoneToRemove = this.getRandomZoneFromList((IReadOnlyCollection<TileZone>) pCity.border_zones);
    else if (pCity.zones.Count > 0)
      zoneToRemove = this.getRandomZoneFromList((IReadOnlyCollection<TileZone>) pCity.zones);
    return zoneToRemove;
  }

  private TileZone getRandomZoneFromList(IReadOnlyCollection<TileZone> pList)
  {
    if (pList.Count == 0)
      return (TileZone) null;
    using (ListPool<TileZone> list = new ListPool<TileZone>((IEnumerable<TileZone>) pList))
      return list.GetRandom<TileZone>();
  }
}
