// Decompiled with JetBrains decompiler
// Type: CityZoneGrowth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CityZoneGrowth : CityZoneWorkerBase
{
  private const float MOD_RADIUS = 0.75f;

  public TileZone getZoneToClaim(
    Actor pActor,
    City pCity,
    bool pDebug = false,
    HashSet<TileZone> pSetToFill = null,
    int pBonusRange = 0)
  {
    this.clearAll();
    WorldTile tile = pCity.getTile();
    if (tile == null)
      return (TileZone) null;
    bool pStopWaveWhenEmptyZoneFound = !pDebug;
    this.startWaveFromTile(pActor, tile, pCity, pStopWaveWhenEmptyZoneFound, pBonusRange);
    if (!pDebug || pSetToFill == null)
      return this.checkGrowBorder(pCity);
    foreach (ZoneConnection zoneConnection in this._zones_checked)
      pSetToFill.Add(zoneConnection.zone);
    return (TileZone) null;
  }

  private TileZone checkGrowBorder(City pCity)
  {
    int num = Randy.randomChance(0.7f) ? 1 : 0;
    TileZone tileZone = (TileZone) null;
    if (num != 0)
    {
      TileZone randomZone = this.getRandomZone(pCity);
      if (randomZone != null)
        tileZone = randomZone;
    }
    else
      tileZone = this.getRandomCheckedZone(pCity);
    return tileZone;
  }

  private TileZone getRandomZone(City pCity)
  {
    using (ListPool<TileZone> list = new ListPool<TileZone>((ICollection<TileZone>) pCity.border_zones))
    {
      WorldTile tile = pCity.getTile();
      if (tile == null)
        return (TileZone) null;
      TileZone zone = tile.zone;
      float num1 = (float) pCity.getZoneRange() * 0.75f;
      float num2 = num1 * num1;
      foreach (TileZone tileZone in list.LoopRandom<TileZone>())
      {
        foreach (TileZone randomZone in tileZone.neighbours.LoopRandom<TileZone>())
        {
          if (randomZone.canBeClaimedByCity(pCity) && randomZone.centerTile.isSameIsland(tile) && (double) Toolbox.SquaredDist(zone.x, zone.y, randomZone.x, randomZone.y) <= (double) num2)
            return randomZone;
        }
      }
      return (TileZone) null;
    }
  }

  private TileZone getBestZoneFromList(City pCity, List<TileZone> pList)
  {
    TileZone bestZoneFromList = (TileZone) null;
    TileZone zone = pCity.getTile().zone;
    int num1 = int.MaxValue;
    for (int index = 0; index < pList.Count; ++index)
    {
      TileZone p = pList[index];
      int num2 = Toolbox.SquaredDist(p.x, p.y, zone.x, zone.y);
      if (num2 < num1)
      {
        bestZoneFromList = p;
        num1 = num2;
      }
    }
    return bestZoneFromList;
  }

  private TileZone getRandomCheckedZone(City pCity)
  {
    using (ListPool<TileZone> list = new ListPool<TileZone>(this._zones_checked.Count))
    {
      foreach (ZoneConnection zoneConnection in this._zones_checked)
      {
        TileZone zone = zoneConnection.zone;
        if (zone.canBeClaimedByCity(pCity))
          list.Add(zone);
      }
      return list.Count > 0 ? list.GetRandom<TileZone>() : (TileZone) null;
    }
  }

  private void startWaveFromTile(
    Actor pActor,
    WorldTile pTile,
    City pCity,
    bool pStopWaveWhenEmptyZoneFound = true,
    int pBonusRange = 0)
  {
    this.prepareWave();
    if (pActor == null)
      pActor = pCity.leader;
    Queue<ZoneConnection> pWave1 = this._wave;
    Queue<ZoneConnection> pWave2 = this._next_wave;
    this.queueConnection(new ZoneConnection(pTile.zone, pTile.region), pWave1, true);
    using (ListPool<MapRegion> pListToFill = new ListPool<MapRegion>())
    {
      int num1 = pCity.getZoneRange() + pBonusRange;
      float num2 = (float) num1 * 0.75f;
      float num3 = num2 * num2;
      int num4 = 0;
      bool flag = false;
      while ((pWave2.Count > 0 || pWave1.Count > 0) && !(pStopWaveWhenEmptyZoneFound & flag))
      {
        if (pWave1.Count == 0)
        {
          Queue<ZoneConnection> zoneConnectionQueue = pWave1;
          pWave1 = pWave2;
          pWave2 = zoneConnectionQueue;
          ++num4;
          if (num4 > num1)
            break;
        }
        ZoneConnection zoneConnection = pWave1.Dequeue();
        TileZone zone = zoneConnection.zone;
        MapRegion region = zoneConnection.region;
        for (int index1 = 0; index1 < zone.neighbours.Length; ++index1)
        {
          TileZone neighbour = zone.neighbours[index1];
          if ((!pStopWaveWhenEmptyZoneFound || !neighbour.hasCity() || neighbour.isSameCityHere(pCity)) && neighbour.tiles_with_ground != 0 && (pActor == null || !pActor.hasSubspecies() || neighbour.checkCanSettleInThisBiomes(pActor.subspecies)))
          {
            pListToFill.Clear();
            if (TileZone.hasZonesConnectedViaRegions(zone, neighbour, region, pListToFill))
            {
              for (int index2 = 0; index2 < pListToFill.Count; ++index2)
              {
                MapRegion pRegion = pListToFill[index2];
                ZoneConnection pConnection = new ZoneConnection(neighbour, pRegion);
                if (this._zones_checked.Add(pConnection))
                {
                  if (neighbour.canBeClaimedByCity(pCity))
                    flag = true;
                  if ((double) Toolbox.SquaredDist(pTile.zone.x, pTile.zone.y, neighbour.x, neighbour.y) <= (double) num3)
                  {
                    if (!(pStopWaveWhenEmptyZoneFound & flag))
                      this.queueConnection(pConnection, pWave2);
                    else
                      break;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
