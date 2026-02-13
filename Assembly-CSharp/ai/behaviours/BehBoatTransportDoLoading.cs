// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatTransportDoLoading
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;

#nullable disable
namespace ai.behaviours;

public class BehBoatTransportDoLoading : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    TaxiRequest taxiRequest = this.boat.taxi_request;
    if (taxiRequest == null)
    {
      this.boat.cancelWork(pActor);
      return BehResult.Stop;
    }
    bool flag = true;
    if (this.boat.passengerWaitCounter > 4 || this.boat.countPassengers() >= 100)
      flag = false;
    else if (taxiRequest.everyoneEmbarked())
      flag = false;
    if (flag)
    {
      foreach (Actor actor in taxiRequest.getActors())
      {
        if (!actor.is_inside_boat && !actor.isFighting() && (!actor.hasTask() || !actor.ai.task.flag_boat_related))
        {
          actor.stopSleeping();
          actor.cancelAllBeh();
          actor.setTask("force_into_a_boat");
        }
      }
      taxiRequest.setState(TaxiRequestState.Loading);
      pActor.timer_action = 12f;
      ++this.boat.passengerWaitCounter;
      return BehResult.RepeatStep;
    }
    if (!this.boat.hasPassengers())
    {
      this.boat.cancelWork(pActor);
      return BehResult.Stop;
    }
    taxiRequest.setState(TaxiRequestState.Transporting);
    taxiRequest.cancelForLatePassengers();
    this.boat.taxi_target = taxiRequest.getTileTarget();
    return BehResult.Continue;
  }
}
