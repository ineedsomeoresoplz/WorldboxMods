// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorGetRandomZoneTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorGetRandomZoneTile : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.city.hasZones())
      return BehResult.Stop;
    WorldTile worldTile;
    if (pActor.current_tile.zone.city != pActor.city || Randy.randomChance(0.2f))
    {
      worldTile = pActor.city.zones.GetRandom<TileZone>().getRandomTile();
      if (!worldTile.isSameIsland(pActor.current_tile))
        return BehResult.Stop;
    }
    else
      worldTile = pActor.current_tile.region.tiles.GetRandom<WorldTile>();
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }
}
