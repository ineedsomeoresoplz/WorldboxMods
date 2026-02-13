// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorGetRandomIdleTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorGetRandomIdleTile : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.city.hasZones())
      return BehResult.Stop;
    WorldTile worldTile = this.tryToGetBonfireTile(pActor) ?? this.getRandomCityZoneTile(pActor);
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }

  private WorldTile getRandomCityZoneTile(Actor pActor)
  {
    WorldTile random;
    if (!pActor.current_tile.isSameCityHere(pActor.city) || Randy.randomChance(0.2f))
    {
      random = pActor.city.zones.GetRandom<TileZone>().tiles.GetRandom<WorldTile>();
      if (!random.isSameIsland(pActor.current_tile))
        return (WorldTile) null;
    }
    else
      random = pActor.current_tile.region.tiles.GetRandom<WorldTile>();
    return random;
  }

  private WorldTile tryToGetBonfireTile(Actor pActor)
  {
    Building buildingOfType = pActor.city.getBuildingOfType("type_bonfire", pRandom: true, pLimitIsland: pActor.current_island);
    if (buildingOfType == null)
      return (WorldTile) null;
    WorldTile random = buildingOfType.tiles.GetRandom<WorldTile>();
    if (Randy.randomChance(0.3f))
    {
      MapRegion mapRegion = buildingOfType.current_tile.region;
      if (mapRegion.neighbours.Count > 0 && Randy.randomChance(0.3f))
        mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
      random = mapRegion.tiles.GetRandom<WorldTile>();
    }
    return random;
  }
}
