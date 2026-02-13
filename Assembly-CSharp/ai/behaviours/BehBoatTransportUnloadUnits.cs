// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatTransportUnloadUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;

#nullable disable
namespace ai.behaviours;

public class BehBoatTransportUnloadUnits : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if (this.boat.taxi_target == null)
      return BehResult.Stop;
    WorldTile pTile = PathfinderTools.raycastTileForUnitLandingFromOcean(pActor.current_tile, this.boat.taxi_target);
    if (pTile.Type.ocean)
    {
      foreach (WorldTile worldTile in pTile.neighboursAll)
      {
        if (worldTile.Type.ground)
        {
          pTile = worldTile;
          break;
        }
      }
    }
    this.boat.unloadPassengers(pTile);
    if (this.boat.taxi_request != null)
    {
      TaxiManager.finish(this.boat.taxi_request);
      this.boat.taxi_request = (TaxiRequest) null;
      this.boat.cancelWork(pActor);
    }
    return BehResult.Continue;
  }
}
