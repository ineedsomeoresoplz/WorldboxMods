// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindRandomCivBuildingTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindRandomCivBuildingTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    MapRegion region = pActor.current_tile.region;
    if (Randy.randomChance(0.65f) && region.tiles.Count > 0)
    {
      pActor.beh_tile_target = region.getRandomTile();
      return BehResult.Continue;
    }
    Building building1 = (Building) null;
    foreach (Building building2 in Finder.getBuildingsFromChunk(pActor.current_tile, 1, pRandom: true))
    {
      if (building2.asset.city_building && building2.current_tile.isSameIsland(pActor.current_tile) && building2.isCiv())
      {
        building1 = building2;
        break;
      }
    }
    if (building1 == null)
    {
      if (region.tiles.Count <= 0)
        return BehResult.Stop;
      pActor.beh_tile_target = region.getRandomTile();
      return BehResult.Continue;
    }
    pActor.beh_tile_target = building1.current_tile.region.getRandomTile();
    return BehResult.Continue;
  }
}
