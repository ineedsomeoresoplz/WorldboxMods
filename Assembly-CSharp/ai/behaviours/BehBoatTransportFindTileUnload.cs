// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatTransportFindTileUnload
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using tools;

#nullable disable
namespace ai.behaviours;

public class BehBoatTransportFindTileUnload : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if (this.boat.taxi_target == null)
      return BehResult.Stop;
    WorldTile tileForBoat = OceanHelper.findTileForBoat(pActor.current_tile, this.boat.taxi_target);
    if (tileForBoat == null)
    {
      this.boat.cancelWork(pActor);
      return BehResult.Stop;
    }
    pActor.beh_tile_target = tileForBoat;
    return BehResult.Continue;
  }
}
