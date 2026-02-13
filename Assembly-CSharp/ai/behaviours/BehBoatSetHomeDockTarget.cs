// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatSetHomeDockTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatSetHomeDockTarget : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    this.checkHomeDocks(pActor);
    Building homeBuilding = this.boat.actor.getHomeBuilding();
    if (homeBuilding == null)
      return BehResult.Stop;
    pActor.beh_building_target = homeBuilding;
    return BehResult.Continue;
  }
}
