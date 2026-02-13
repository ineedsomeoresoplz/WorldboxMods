// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBeeReturnHome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBeeReturnHome : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    Building homeBuilding = pActor.getHomeBuilding();
    if (homeBuilding.isRekt() || (double) Toolbox.DistTile(pActor.current_tile, homeBuilding.current_tile) > 3.0)
      return BehResult.Stop;
    if (pActor.data.pollen == 3 && pActor.current_tile.building == homeBuilding)
    {
      pActor.data.pollen = 0;
      if (pActor.isKingdomCiv())
        pActor.addToInventory("honey", 1);
      else
        homeBuilding.component_beehive.addHoney();
      pActor.timer_action = 3f;
    }
    return BehResult.Continue;
  }
}
