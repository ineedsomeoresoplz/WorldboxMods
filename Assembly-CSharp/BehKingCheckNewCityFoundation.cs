// Decompiled with JetBrains decompiler
// Type: BehKingCheckNewCityFoundation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BehKingCheckNewCityFoundation : BehaviourActionActor
{
  private const int MAX_MOVED = 6;
  private List<TileZone> _next_wave = new List<TileZone>();
  private List<TileZone> _wave = new List<TileZone>();
  private HashSet<TileZone> _checked_zones = new HashSet<TileZone>();
  private static Color _color1 = new Color(1f, 0.0f, 0.0f, 0.3f);
  private static Color _color2 = new Color(0.0f, 0.0f, 1f, 0.3f);
  private static Color _color3 = new Color(1f, 0.92f, 0.016f, 0.3f);
  private static Color _color4 = new Color(0.0f, 1f, 0.0f, 0.3f);

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.uses_families = true;
    this.uses_cities = true;
    this.uses_kingdoms = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Kingdom kingdom = pActor.kingdom;
    if (!kingdom.hasCapital() || this.hasCitiesWithoutPopulation(kingdom))
      return BehResult.Stop;
    BehaviourActionBase<Actor>.world.city_zone_helper.city_place_finder.recalc();
    if (!BehaviourActionBase<Actor>.world.city_zone_helper.city_place_finder.hasPossibleZones())
      return BehResult.Stop;
    using (ListPool<City> listForExpansion = this.getCityListForExpansion(kingdom))
    {
      if (listForExpansion.Count == 0)
        return BehResult.Stop;
      City pCityExpandFrom;
      TileZone zoneForExpansion = this.findZoneForExpansion(pActor, listForExpansion, out pCityExpandFrom);
      if (zoneForExpansion == null)
        return BehResult.Stop;
      this.moveSomeUnitsToNewCity(BehaviourActionBase<Actor>.world.cities.buildNewCity(pActor, zoneForExpansion), pCityExpandFrom);
      return BehResult.Continue;
    }
  }

  private bool hasCitiesWithoutPopulation(Kingdom pKingdom)
  {
    WorldTile tile1 = pKingdom.capital.getTile();
    if (tile1 == null)
      return false;
    foreach (City city in pKingdom.getCities())
    {
      if (city.countUnits() <= 30)
      {
        WorldTile tile2 = city.getTile();
        if (tile2 != null && tile2.reachableFrom(tile1))
          return true;
      }
    }
    return false;
  }

  private void moveSomeUnitsToNewCity(City pNewCity, City pFromCity)
  {
    int num1 = Mathf.Min(pFromCity.units.Count, 6);
    int num2 = 0;
    foreach (Actor actor in pFromCity.units.LoopRandom<Actor>())
    {
      if (this.isPossibleToMoveUnitToCity(actor, pNewCity))
      {
        this.moveToCity(actor, pNewCity);
        int pMovedAlready = num2 + 1;
        int num3 = this.checkUnitFamilyAndLovers(actor, pNewCity, pMovedAlready);
        num2 = pMovedAlready + num3;
        if (num2 >= num1)
          break;
      }
    }
  }

  private int checkUnitFamilyAndLovers(Actor pActor, City pCity, int pMovedAlready)
  {
    int num = 0;
    if (pActor.hasLover())
    {
      Actor lover = pActor.lover;
      if (this.isPossibleToMoveUnitToCity(lover, pCity))
      {
        this.moveToCity(lover, pCity);
        ++num;
      }
    }
    if (pActor.hasFamily())
    {
      foreach (Actor unit in pActor.family.units)
      {
        if (this.isPossibleToMoveUnitToCity(unit, pCity))
        {
          this.moveToCity(unit, pCity);
          ++num;
        }
        if (num + pMovedAlready >= 6)
          break;
      }
    }
    return num;
  }

  private void moveToCity(Actor pActor, City pCity)
  {
    pActor.stopBeingWarrior();
    pActor.joinCity(pCity);
    pActor.cancelAllBeh();
  }

  private bool isPossibleToMoveUnitToCity(Actor pUnit, City pCity)
  {
    return !pUnit.isRekt() && pUnit.isAdult() && !pUnit.isCityLeader() && !pUnit.isKing() && !pUnit.isArmyGroupLeader() && pUnit.army == null && (!pUnit.hasLover() || !pUnit.lover.isKing() && !pUnit.lover.isCityLeader()) && pUnit.city != pCity;
  }

  private TileZone findZoneForExpansion(
    Actor pActor,
    ListPool<City> pPossibleCitiesToExpandFrom,
    out City pCityExpandFrom)
  {
    // ISSUE: unable to decompile the method.
  }

  private TileZone findZoneForCityOnFarIsland(Actor pActor, City pCity)
  {
    TileZone forCityOnFarIsland = (TileZone) null;
    int num1 = int.MaxValue;
    WorldTile tile = pCity.getTile();
    if (tile == null)
      return (TileZone) null;
    Vector2Int pos = tile.pos;
    foreach (TileZone zone in BehaviourActionBase<Actor>.world.city_zone_helper.city_place_finder.zones)
    {
      int num2 = Toolbox.SquaredDistVec2(zone.centerTile.pos, pos);
      if (num2 <= num1 && tile.reachableFrom(zone.centerTile) && zone.checkCanSettleInThisBiomes(pActor.subspecies))
      {
        num1 = num2;
        forCityOnFarIsland = zone;
      }
    }
    return forCityOnFarIsland;
  }

  private TileZone findZoneForCityOnTheSameIsland2(Actor pActor, City pMainCity)
  {
    WorldTile tile1 = pMainCity.getTile();
    if (tile1 == null)
      return (TileZone) null;
    using (ListPool<TileZone> listPool = new ListPool<TileZone>())
    {
      bool flag = DebugConfig.isOn(DebugOption.CitySettleCalc);
      if (flag)
        DebugHighlight.clear();
      foreach (City city in pMainCity.kingdom.getCities())
      {
        if (city == pMainCity)
        {
          WorldTile tile2 = city.getTile();
          if (tile2 != null && tile1.isSameIsland(tile2))
          {
            foreach (TileZone neighbourZone in city.neighbour_zones)
            {
              if (!neighbourZone.hasCity())
              {
                this._wave.Add(neighbourZone);
                this._checked_zones.Add(neighbourZone);
              }
            }
          }
        }
      }
      int pWave = 0;
      while (this._wave.Count > 0)
      {
        if (flag)
        {
          switch (pWave)
          {
            case 0:
              DebugHighlight.newHighlightList(BehKingCheckNewCityFoundation._color1, this._wave);
              break;
            case 1:
              DebugHighlight.newHighlightList(BehKingCheckNewCityFoundation._color2, this._wave);
              break;
            case 2:
              DebugHighlight.newHighlightList(BehKingCheckNewCityFoundation._color3, this._wave);
              break;
            case 3:
              DebugHighlight.newHighlightList(BehKingCheckNewCityFoundation._color4, this._wave);
              break;
          }
        }
        this.startWave(pWave, tile1, listPool, pActor);
        if (this._next_wave.Count > 0)
        {
          this._wave.AddRange((IEnumerable<TileZone>) this._next_wave);
          this._next_wave.Clear();
          ++pWave;
          if (pWave >= 4)
            break;
        }
      }
      this._wave.Clear();
      this._checked_zones.Clear();
      return listPool.Count == 0 ? (TileZone) null : listPool.GetRandom<TileZone>();
    }
  }

  private void startWave(
    int pWave,
    WorldTile pCityTile,
    ListPool<TileZone> pGoodZones,
    Actor pActor)
  {
    List<TileZone> wave = this._wave;
    HashSet<TileZone> checkedZones = this._checked_zones;
    while (wave.Count > 0)
    {
      TileZone tileZone = wave.Pop<TileZone>();
      checkedZones.Add(tileZone);
      if (pWave > 2 && tileZone.isGoodForNewCity(pActor) && tileZone.centerTile.isSameIsland(pCityTile))
        pGoodZones.Add(tileZone);
      foreach (TileZone neighbour in tileZone.neighbours)
      {
        if (checkedZones.Add(neighbour) && !neighbour.hasCity())
          this._next_wave.Add(neighbour);
      }
    }
  }

  private ListPool<City> getCityListForExpansion(Kingdom pKingdom)
  {
    ListPool<City> list = new ListPool<City>(pKingdom.countCities());
    foreach (City city in pKingdom.getCities())
    {
      if (city.getTile() != null && city.status.population_adults >= 30 && !city.needSettlers())
        list.Add(city);
    }
    list.Shuffle<City>();
    return list;
  }

  private TileZone findCityForMigration(City pCity)
  {
    WorldTile tile1 = pCity.getTile();
    if (tile1 == null)
      return (TileZone) null;
    foreach (City city in pCity.kingdom.getCities().LoopRandom<City>())
    {
      if (city != pCity)
      {
        WorldTile tile2 = city.getTile();
        if (tile2 != null && tile1.reachableFrom(tile2) && city.needSettlers())
        {
          TileZone zone = city.getTile()?.zone;
          if (zone != null)
            return zone;
        }
      }
    }
    return (TileZone) null;
  }
}
