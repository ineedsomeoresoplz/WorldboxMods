// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehTaxiEmbark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;

#nullable disable
namespace ai.behaviours;

public class BehTaxiEmbark : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    TaxiRequest requestForActor = TaxiManager.getRequestForActor(pActor);
    if (requestForActor == null || !requestForActor.hasAssignedBoat() || requestForActor.state != TaxiRequestState.Loading)
      return BehResult.Stop;
    Boat boat = requestForActor.getBoat();
    bool flag = requestForActor.isBoatNearDock();
    if (!((double) Toolbox.SquaredDistTile(boat.actor.current_tile, pActor.current_tile) < 25.0 | flag))
      return BehResult.Stop;
    pActor.beh_tile_target = (WorldTile) null;
    pActor.embarkInto(requestForActor.getBoat());
    return BehResult.Continue;
  }
}
