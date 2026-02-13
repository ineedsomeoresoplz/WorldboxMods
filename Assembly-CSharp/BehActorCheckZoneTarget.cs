// Decompiled with JetBrains decompiler
// Type: BehActorCheckZoneTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehActorCheckZoneTarget : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    City city = pActor.city;
    TileZone zoneToClaim = BehaviourActionBase<Actor>.world.city_zone_helper.city_growth.getZoneToClaim(pActor, pActor.city);
    if (zoneToClaim == null || zoneToClaim.city == city)
      return BehResult.Stop;
    WorldTile worldTile1 = (WorldTile) null;
    if (zoneToClaim.centerTile.isSameIsland(pActor.current_tile))
    {
      worldTile1 = zoneToClaim.centerTile;
    }
    else
    {
      foreach (WorldTile worldTile2 in zoneToClaim.tiles.LoopRandom<WorldTile>())
      {
        if (worldTile2.isSameIsland(pActor.current_tile))
        {
          worldTile1 = worldTile2;
          break;
        }
      }
    }
    if (worldTile1 == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile1;
    return BehResult.Continue;
  }
}
