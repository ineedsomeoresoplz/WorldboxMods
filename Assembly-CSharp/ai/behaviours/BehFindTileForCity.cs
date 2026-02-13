// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTileForCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehFindTileForCity : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!BehaviourActionBase<Actor>.world.city_zone_helper.city_place_finder.hasPossibleZones())
      return BehResult.Stop;
    TileZone tileZone1 = (TileZone) null;
    float num1 = float.MaxValue;
    List<TileZone> zones = BehaviourActionBase<Actor>.world.city_zone_helper.city_place_finder.zones;
    Vector3 posV3 = pActor.current_tile.posV3;
    TileZone tileZone2 = (TileZone) null;
    for (int index = 0; index < zones.Count; ++index)
    {
      TileZone tileZone3 = zones[index];
      float num2 = Toolbox.SquaredDistVec3(posV3, tileZone3.centerTile.posV3);
      if ((double) num1 > (double) num2 && tileZone3.tiles[0].isSameIsland(pActor.current_tile) && tileZone3.isGoodForNewCity())
      {
        num1 = num2;
        tileZone1 = tileZone3;
      }
    }
    if (tileZone1 != null)
    {
      pActor.beh_tile_target = tileZone1.tiles.GetRandom<WorldTile>();
      return BehResult.Continue;
    }
    if (tileZone2 != null)
    {
      pActor.beh_tile_target = tileZone2.tiles.GetRandom<WorldTile>();
      return BehResult.Continue;
    }
    TileIsland randomIslandGround = BehaviourActionBase<Actor>.world.islands_calculator.getRandomIslandGround();
    if (randomIslandGround == null)
      return BehResult.Stop;
    pActor.beh_tile_target = randomIslandGround.getRandomTile();
    return BehResult.Continue;
  }
}
