// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorFertilizeCrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCityActorFertilizeCrop : BehActorUsableBuildingTarget
{
  public override BehResult execute(Actor pActor)
  {
    Building behBuildingTarget = pActor.beh_building_target;
    if (behBuildingTarget.component_wheat.isMaxLevel() || pActor.inventory.getResource("fertilizer") == 0)
      return BehResult.Stop;
    pActor.takeFromInventory("fertilizer", 1);
    behBuildingTarget.component_wheat.growFull();
    pActor.addLoot(SimGlobals.m.coins_for_fertilize);
    return BehResult.Continue;
  }
}
