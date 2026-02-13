// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatFindRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;

#nullable disable
namespace ai.behaviours;

public class BehBoatFindRequest : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if (this.boat.taxi_request != null && !this.boat.taxi_request.isAlreadyUsedByBoat(pActor))
    {
      this.boat.taxi_request.cancel();
      this.boat.taxi_request = (TaxiRequest) null;
    }
    this.boat.taxi_request = TaxiManager.getNewRequestForBoat(pActor);
    if (this.boat.taxi_request == null)
      return BehResult.Stop;
    this.boat.taxi_request.assign(this.boat);
    return this.forceTask(pActor, "boat_transport_go_load");
  }
}
