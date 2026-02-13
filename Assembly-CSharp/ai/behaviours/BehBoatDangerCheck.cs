// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatDangerCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatDangerCheck : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.attackedBy == null)
      return BehResult.Stop;
    if ((double) pActor.getHealthRatio() < 0.25)
    {
      this.checkHomeDocks(pActor);
      if (this.boat.actor.getHomeBuilding() != null)
      {
        pActor.cancelAllBeh();
        return this.forceTask(pActor, "boat_return_to_dock");
      }
    }
    return BehResult.Continue;
  }
}
