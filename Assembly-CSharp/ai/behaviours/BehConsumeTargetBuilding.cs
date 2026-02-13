// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehConsumeTargetBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehConsumeTargetBuilding : BehActorUsableBuildingTarget
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_building_target.asset.type == "type_fruits")
    {
      if (pActor.beh_building_target.hasResourcesToCollect())
      {
        pActor.beh_building_target.extractResources(pActor);
        pActor.addNutritionFromEating(pActor.beh_building_target.asset.nutrition_restore, pSetJustAte: true);
        pActor.countConsumed();
      }
    }
    else if (pActor.beh_building_target.isAlive())
    {
      pActor.beh_building_target.startDestroyBuilding();
      pActor.addNutritionFromEating(pActor.beh_building_target.asset.nutrition_restore, pSetJustAte: true);
      pActor.countConsumed();
    }
    WorldTile currentTile = pActor.beh_building_target.current_tile;
    pActor.punchTargetAnimation(currentTile.posV3, false);
    return BehResult.Continue;
  }
}
