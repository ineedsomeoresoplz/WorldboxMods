// Decompiled with JetBrains decompiler
// Type: ai.behaviours.CityBehCheckAttackZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class CityBehCheckAttackZone : BehaviourActionCity
{
  public override BehResult execute(City pCity)
  {
    City city = pCity.target_attack_city;
    bool flag = true;
    if (city == null)
      flag = false;
    else if (!city.isAlive() || !pCity.hasAnyWarriors() || !city.kingdom.isEnemy(pCity.kingdom) || !city.reachableFrom(pCity))
      flag = false;
    if (!flag)
    {
      pCity.target_attack_city = (City) null;
      pCity.target_attack_zone = (TileZone) null;
      city = (City) null;
    }
    if (pCity.target_attack_city != null && pCity.target_attack_zone.city != pCity.target_attack_city)
      pCity.target_attack_zone = (TileZone) null;
    if (pCity.hasAttackZoneOrder())
      return BehResult.Continue;
    if (city == null)
    {
      if (!pCity.isOkToSendArmy())
        return BehResult.Continue;
      city = this.findTargetCity(pCity);
    }
    if (city == null)
      return BehResult.Continue;
    pCity.target_attack_city = city;
    if (city.buildings.Count > 0)
    {
      Building random = city.buildings.GetRandom<Building>();
      pCity.target_attack_zone = random.current_tile.zone;
    }
    else if (pCity.hasZones())
      pCity.target_attack_zone = pCity.zones.GetRandom<TileZone>();
    return BehResult.Continue;
  }

  private City findTargetCity(City pOurCity)
  {
    // ISSUE: unable to decompile the method.
  }
}
