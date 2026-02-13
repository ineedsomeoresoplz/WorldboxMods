// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBurnTumorTiles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace ai.behaviours;

public class BehBurnTumorTiles : BehaviourActionActor
{
  private static List<WorldTile> tiles = new List<WorldTile>();

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.current_tile.Type.ground)
      return BehResult.Stop;
    WorldTile pTile = (WorldTile) null;
    List<WorldTile> tiles = BehBurnTumorTiles.tiles;
    this.checkRegion(pActor.current_tile.region, tiles);
    if (tiles.Count != 0)
    {
      pTile = tiles.GetRandom<WorldTile>();
    }
    else
    {
      List<MapRegion> neighbours = pActor.current_tile.region.neighbours;
      for (int index = 0; index < neighbours.Count; ++index)
      {
        this.checkRegion(neighbours[index], tiles);
        if (tiles.Count != 0)
        {
          pTile = tiles.GetRandom<WorldTile>();
          break;
        }
      }
    }
    tiles.Clear();
    if (pTile == null)
      return BehResult.Stop;
    AttackAction action = AssetManager.spells.get("cast_fire").action;
    if (action != null)
    {
      int num = action((BaseSimObject) pActor, (BaseSimObject) null, pTile) ? 1 : 0;
    }
    pActor.doCastAnimation();
    return BehResult.Continue;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void checkRegion(MapRegion pRegion, List<WorldTile> pTiles)
  {
    List<WorldTile> tiles = pRegion.tiles;
    for (int index = 0; index < tiles.Count; ++index)
    {
      WorldTile worldTile = tiles[index];
      if (worldTile.Type.creep)
        pTiles.Add(worldTile);
    }
  }
}
