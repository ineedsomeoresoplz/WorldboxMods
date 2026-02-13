// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatTransportFindTilePickUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using tools;

#nullable disable
namespace ai.behaviours;

public class BehBoatTransportFindTilePickUp : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if (this.boat.taxi_request == null || !this.boat.taxi_request.isStillLegit())
    {
      this.boat.cancelWork(pActor);
      return BehResult.Stop;
    }
    this.boat.pickup_near_dock = false;
    ActorTool.checkHomeDocks(pActor);
    Building homeBuilding = this.boat.actor.getHomeBuilding();
    if (homeBuilding != null)
    {
      WorldTile oceanTileInSameOcean = homeBuilding.component_docks.getOceanTileInSameOcean(pActor.current_tile);
      if (oceanTileInSameOcean != null && oceanTileInSameOcean.isSameIsland(this.boat.taxi_request.getTileStart()))
      {
        this.boat.pickup_near_dock = true;
        if (this.boat.isNearDock())
        {
          this.boat.passengerWaitCounter = 0;
          pActor.beh_tile_target = pActor.current_tile;
          return BehResult.Continue;
        }
        pActor.beh_tile_target = oceanTileInSameOcean;
        return BehResult.Continue;
      }
    }
    WorldTile tileForBoat = OceanHelper.findTileForBoat(pActor.current_tile, this.boat.taxi_request.getTileStart());
    if (tileForBoat == null)
    {
      this.boat.cancelWork(pActor);
      return BehResult.Stop;
    }
    this.boat.passengerWaitCounter = 0;
    pActor.beh_tile_target = tileForBoat;
    return BehResult.Continue;
  }
}
