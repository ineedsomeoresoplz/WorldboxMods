// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehPollinate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehPollinate : BehaviourActionActor
{
  public BehPollinate() => this.land_if_hovering = true;

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.current_tile.hasBuilding())
      return BehResult.Stop;
    if (pActor.current_tile.building.asset.type == "type_flower")
    {
      ++pActor.data.pollen;
      pActor.current_tile.pollinate();
      if (pActor.asset.id != "bee" && pActor.data.pollen >= 10)
      {
        pActor.data.pollen -= 10;
        if (pActor.isKingdomCiv())
          pActor.addToInventory("honey", 1);
      }
    }
    pActor.timer_action = Randy.randomFloat(4f, 10f);
    return BehResult.Continue;
  }
}
