// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTileForEating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehFindTileForEating : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    WorldTile worldTile = this.findTileAround((IEnumerable<WorldTile>) pActor.current_tile.neighboursAll) ?? this.findTileAround((IEnumerable<WorldTile>) pActor.current_tile.region.tiles);
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }

  private WorldTile findTileAround(IEnumerable<WorldTile> pList)
  {
    WorldTile tileAround = (WorldTile) null;
    foreach (WorldTile p in pList)
    {
      if (p.Type.canBeEatenByGeophag())
      {
        if (tileAround == null)
          tileAround = p;
        else if (Randy.randomBool())
        {
          tileAround = p;
          break;
        }
      }
    }
    return tileAround;
  }
}
