// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorGetRandomDangerZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorGetRandomDangerZone : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    City city = pActor.city;
    if (!city.hasZones() || !city.isInDanger())
      return BehResult.Stop;
    if (Randy.randomChance(0.2f))
    {
      foreach (TileZone dangerZone in city.danger_zones)
      {
        WorldTile random = dangerZone.tiles.GetRandom<WorldTile>();
        if (random.isSameIsland(pActor.current_tile))
        {
          pActor.beh_tile_target = random;
          return BehResult.Continue;
        }
      }
    }
    int num1 = int.MaxValue;
    WorldTile worldTile = (WorldTile) null;
    foreach (TileZone dangerZone in city.danger_zones)
    {
      WorldTile centerTile = dangerZone.centerTile;
      int num2 = Toolbox.SquaredDistTile(pActor.current_tile, centerTile);
      if (num2 <= num1 && centerTile.isSameIsland(pActor.current_tile) && (num2 != num1 || !Randy.randomBool()))
      {
        num1 = num2;
        worldTile = centerTile;
      }
    }
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }
}
