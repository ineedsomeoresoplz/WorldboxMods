// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindRandomTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindRandomTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    MapRegion mapRegion = pActor.current_tile.region;
    if (Randy.randomChance(0.65f) && mapRegion.tiles.Count > 0)
    {
      pActor.beh_tile_target = mapRegion.tiles.GetRandom<WorldTile>();
      return BehResult.Continue;
    }
    if (mapRegion.neighbours.Count > 0 && Randy.randomBool())
      mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
    if (mapRegion.tiles.Count <= 0)
      return BehResult.Stop;
    pActor.beh_tile_target = mapRegion.tiles.GetRandom<WorldTile>();
    return BehResult.Continue;
  }
}
