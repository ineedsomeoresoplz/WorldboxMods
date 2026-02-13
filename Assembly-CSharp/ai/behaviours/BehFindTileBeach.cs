// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTileBeach
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehFindTileBeach : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    BehaviourActionActor.possible_moves.Clear();
    this.findEdgesInRegion(pActor.current_tile.region);
    if (BehaviourActionActor.possible_moves.Count == 0)
    {
      for (int index = 0; index < pActor.current_tile.region.neighbours.Count; ++index)
        this.findEdgesInRegion(pActor.current_tile.region.neighbours[index]);
    }
    if (BehaviourActionActor.possible_moves.Count == 0)
      return BehResult.Stop;
    pActor.beh_tile_target = BehaviourActionActor.possible_moves.GetRandom<WorldTile>();
    BehaviourActionActor.possible_moves.Clear();
    return BehResult.Continue;
  }

  private void findEdgesInRegion(MapRegion pRegion)
  {
    List<WorldTile> edgeTiles = pRegion.getEdgeTiles();
    int count = edgeTiles.Count;
    int num = Randy.randomInt(0, count);
    for (int index1 = 0; index1 < count; ++index1)
    {
      int index2 = (index1 + num) % count;
      WorldTile worldTile = edgeTiles[index2];
      if (worldTile.Type.ocean)
      {
        BehaviourActionActor.possible_moves.Add(worldTile);
        break;
      }
    }
  }
}
