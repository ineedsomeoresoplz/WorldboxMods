// Decompiled with JetBrains decompiler
// Type: BehFindRandomFarTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehFindRandomFarTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    MapRegion mapRegion = pActor.current_tile.region;
    for (int index = 0; index < 5 && mapRegion.neighbours.Count != 0; ++index)
      mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
    if (mapRegion.tiles.Count <= 0)
      return BehResult.Stop;
    pActor.beh_tile_target = mapRegion.tiles.GetRandom<WorldTile>();
    return BehResult.Continue;
  }
}
