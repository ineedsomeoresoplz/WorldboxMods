// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehBorderSteal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class CityBehBorderSteal : BehaviourActionCity
{
  private static List<TileZone> _zones = new List<TileZone>();

  public override BehResult execute(City pCity)
  {
    if (!DebugConfig.isOn(DebugOption.SystemZoneGrowth) || !WorldLawLibrary.world_law_border_stealing.isEnabled() || pCity.status.population == 0 || pCity.buildings.Count == 0)
      return BehResult.Stop;
    int num = 0;
    while (num < 3 && !this.tryStealZone(pCity))
      ++num;
    return BehResult.Continue;
  }

  private bool tryStealZone(City pCity)
  {
    CityBehBorderSteal._zones.Clear();
    foreach (TileZone neighbour in pCity.buildings.GetRandom<Building>().current_tile.zone.neighbours)
    {
      if (neighbour.city != pCity && neighbour.city != null && !neighbour.hasAnyBuildingsInSet(BuildingList.Civs) && neighbour.city.kingdom.isEnemy(pCity.kingdom))
      {
        this.stealZone(neighbour, pCity);
        return true;
      }
    }
    return false;
  }

  private void stealZone(TileZone pZone, City pCity)
  {
    if (pZone.city != null)
      pZone.city.removeZone(pZone);
    pCity.addZone(pZone);
  }
}
