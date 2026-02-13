// Decompiled with JetBrains decompiler
// Type: BehBoatCheckExistence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehBoatCheckExistence : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    if (this.boat.actor.getHomeBuilding() == null)
    {
      int pResult;
      pActor.data.get("existence_check", out pResult);
      if (pResult == 0)
        pActor.data.set("existence_check", (int) BehaviourActionBase<Actor>.world.getCurWorldTime());
      else if (Date.getMonthsSince((double) pResult) > 2)
        pActor.getHitFullHealth(AttackType.Explosion);
    }
    else
      pActor.data.removeInt("existence_check");
    return BehResult.Continue;
  }
}
