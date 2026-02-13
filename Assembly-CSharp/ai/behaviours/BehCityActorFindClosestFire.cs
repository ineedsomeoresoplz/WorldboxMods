// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorFindClosestFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehCityActorFindClosestFire : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    WorldTile currentTile = pActor.current_tile;
    WorldTile tileOnFireFromZones = BehCityActorFindClosestFire.getClosestTileOnFireFromZones(pActor);
    if (tileOnFireFromZones == null)
      return BehResult.Stop;
    if (pActor.is_visible)
      pActor.spawnSlashYell(Vector2Int.op_Implicit(tileOnFireFromZones.pos));
    WorldTile fightFire = BehCityActorFindClosestFire.raycastTileForUnitToFightFire(currentTile, tileOnFireFromZones);
    if (fightFire == null)
      return BehResult.Stop;
    pActor.beh_tile_target = fightFire;
    return BehResult.Continue;
  }

  private static WorldTile getClosestTileOnFireFromZones(Actor pActor)
  {
    WorldTile currentTile = pActor.current_tile;
    TileZone currentZone = pActor.current_zone;
    WorldTile closestTileOnFire = BehCityActorFindClosestFire.getClosestTileOnFire(currentZone.tiles, currentTile);
    if (closestTileOnFire == null)
    {
      foreach (TileZone tileZone in currentZone.neighbours_all.LoopRandom<TileZone>())
      {
        if (tileZone.isZoneOnFire())
        {
          closestTileOnFire = BehCityActorFindClosestFire.getClosestTileOnFire(tileZone.tiles, currentTile);
          if (closestTileOnFire != null)
            return closestTileOnFire;
        }
      }
    }
    return closestTileOnFire;
  }

  private static WorldTile getClosestTileOnFire(WorldTile[] pArray, WorldTile pTarget)
  {
    WorldTile closestTileOnFire = (WorldTile) null;
    WorldTile[] worldTileArray = pArray;
    int length = worldTileArray.Length;
    int num1 = int.MaxValue;
    for (int index = 0; index < length; ++index)
    {
      WorldTile worldTile = worldTileArray[index];
      int num2 = Toolbox.SquaredDist(pTarget.x, pTarget.y, worldTile.x, worldTile.y);
      if (num2 < num1 && worldTile.isOnFire())
      {
        num1 = num2;
        closestTileOnFire = worldTile;
      }
    }
    return closestTileOnFire;
  }

  public static WorldTile raycastTileForUnitToFightFire(WorldTile pActorTile, WorldTile pTargetFire)
  {
    if (pActorTile == pTargetFire)
      return pActorTile;
    List<WorldTile> worldTileList = PathfinderTools.raycast(pTargetFire, pActorTile);
    WorldTile fightFire = (WorldTile) null;
    float num1 = float.MaxValue;
    for (int index = 0; index < worldTileList.Count; ++index)
    {
      WorldTile worldTile = worldTileList[index];
      float num2 = (float) Toolbox.SquaredDist(worldTile.x, worldTile.y, pTargetFire.x, pTargetFire.y);
      if ((double) num2 >= 4.0 && (double) num2 < (double) num1)
      {
        num1 = num2;
        fightFire = worldTile;
      }
    }
    worldTileList.Clear();
    return fightFire;
  }
}
