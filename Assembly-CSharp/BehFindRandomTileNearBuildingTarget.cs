// Decompiled with JetBrains decompiler
// Type: BehFindRandomTileNearBuildingTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehFindRandomTileNearBuildingTarget : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_building_target == null || !pActor.beh_building_target.current_tile.isSameIsland(pActor.current_tile))
      return BehResult.Stop;
    MapRegion mapRegion = pActor.beh_building_target.current_tile.region;
    if (Randy.randomChance(0.2f) && mapRegion.neighbours.Count > 0)
      mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
    WorldTile random = mapRegion.tiles.GetRandom<WorldTile>();
    pActor.beh_tile_target = random;
    return BehResult.Continue;
  }
}
