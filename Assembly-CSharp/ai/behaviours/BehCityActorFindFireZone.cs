// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorFindFireZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehCityActorFindFireZone : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.hasStatus("burning"))
      return BehResult.Stop;
    TileZone cityZoneNearFire = BehCityActorFindFireZone.getCityZoneNearFire(pActor);
    if (cityZoneNearFire == null)
      return BehResult.Stop;
    WorldTile closestTileNotOnFire = BehCityActorFindFireZone.getClosestTileNotOnFire(cityZoneNearFire.tiles, pActor.current_tile);
    if (closestTileNotOnFire == null)
      return BehResult.Stop;
    pActor.beh_tile_target = closestTileNotOnFire;
    return BehResult.Continue;
  }

  private static TileZone getCityZoneNearFire(Actor pActor)
  {
    using (ListPool<TileZone> listPool = new ListPool<TileZone>((ICollection<TileZone>) pActor.city.zones))
    {
      int num1 = int.MaxValue;
      TileZone cityZoneNearFire = (TileZone) null;
      foreach (TileZone neighbourZone in pActor.city.neighbour_zones)
      {
        if (!neighbourZone.hasCity())
          listPool.Add(neighbourZone);
      }
      Vector2Int pos1 = pActor.current_tile.pos;
      for (int index = 0; index < listPool.Count; ++index)
      {
        TileZone tileZone = listPool[index];
        if (tileZone.isZoneOnFire())
        {
          Vector2Int pos2 = tileZone.centerTile.pos;
          int num2 = Toolbox.SquaredDist(((Vector2Int) ref pos1).x, ((Vector2Int) ref pos1).y, ((Vector2Int) ref pos2).x, ((Vector2Int) ref pos2).y);
          if (num2 < num1)
          {
            cityZoneNearFire = tileZone;
            num1 = num2;
          }
        }
      }
      listPool.Clear();
      return cityZoneNearFire;
    }
  }

  public static WorldTile getClosestTileNotOnFire(WorldTile[] pArray, WorldTile pTarget)
  {
    WorldTile closestTileNotOnFire = (WorldTile) null;
    WorldTile[] worldTileArray = pArray;
    int length = worldTileArray.Length;
    int num1 = int.MaxValue;
    for (int index = 0; index < length; ++index)
    {
      WorldTile worldTile = worldTileArray[index];
      int num2 = Toolbox.SquaredDist(pTarget.x, pTarget.y, worldTile.x, worldTile.y);
      if (num2 < num1 && !worldTile.hasBuilding() && !worldTile.isOnFire())
      {
        num1 = num2;
        closestTileNotOnFire = worldTile;
      }
    }
    return closestTileNotOnFire;
  }
}
