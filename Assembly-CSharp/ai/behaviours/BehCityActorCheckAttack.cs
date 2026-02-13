// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorCheckAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorCheckAttack : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    TileZone targetAttackZone = pActor.city.target_attack_zone;
    City city = pActor.city.target_attack_zone.city;
    if (!this.isAttackingZoneAvailable(pActor, targetAttackZone, city))
      return BehResult.Stop;
    if (pActor.current_tile.zone.city != targetAttackZone.city)
    {
      pActor.beh_tile_target = targetAttackZone.tiles.GetRandom<WorldTile>();
      return BehResult.Continue;
    }
    Building buildingOfType = city.getBuildingOfType("type_watch_tower", false, pLimitIsland: pActor.current_island);
    if (buildingOfType != null)
    {
      pActor.beh_tile_target = buildingOfType.current_tile.region.tiles.GetRandom<WorldTile>();
      return BehResult.Continue;
    }
    foreach (TileZone tileZone in pActor.current_tile.zone.neighbours_all)
    {
      if (tileZone.city == city)
      {
        WorldTile random = tileZone.tiles.GetRandom<WorldTile>();
        if (random.isSameIsland(pActor.current_tile))
        {
          pActor.beh_tile_target = random;
          return BehResult.Continue;
        }
      }
    }
    foreach (TileZone zone in city.zones)
    {
      WorldTile random = zone.tiles.GetRandom<WorldTile>();
      if (random.isSameIsland(pActor.current_tile))
      {
        pActor.beh_tile_target = random;
        return BehResult.Continue;
      }
    }
    return BehResult.Stop;
  }

  private bool isAttackingZoneAvailable(Actor pActor, TileZone pAttackZone, City pAttackCity)
  {
    return (!pActor.army.isGroupInCityAndHaveLeader() || pActor.city.isOkToSendArmy()) && pAttackCity != null && pAttackZone != null && pAttackZone.centerTile.isSameIsland(pActor.current_tile);
  }
}
