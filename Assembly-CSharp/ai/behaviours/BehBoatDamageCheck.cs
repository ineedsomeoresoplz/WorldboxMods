// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatDamageCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatDamageCheck : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if ((double) pActor.getHealthRatio() < 0.800000011920929)
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
