// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatTransportCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatTransportCheck : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    this.checkHomeDocks(pActor);
    if (!this.boat.hasPassengers())
      return this.forceTask(pActor, "boat_transport_check_taxi");
    WorldTile worldTile = (WorldTile) null;
    if (this.boat.countPassengers() > 5 && pActor != null)
    {
      bool? nullable = pActor.city?.hasAttackZoneOrder();
      bool flag = true;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        worldTile = pActor.city.target_attack_zone.centerTile;
    }
    if (worldTile == null)
      worldTile = pActor?.city?.getTile();
    if (worldTile == null)
      return BehResult.Stop;
    this.boat.taxi_target = worldTile;
    pActor.beh_tile_target = worldTile;
    return this.forceTask(pActor, "boat_transport_go_unload", false);
  }
}
