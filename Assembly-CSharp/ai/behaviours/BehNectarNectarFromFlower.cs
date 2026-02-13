// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehNectarNectarFromFlower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehNectarNectarFromFlower : BehActorUsableBuildingTarget
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_building_target.asset.type != "type_flower")
      return BehResult.Stop;
    if (pActor.beh_building_target.isAlive())
    {
      int pVal = (int) ((double) pActor.beh_building_target.asset.nutrition_restore * 0.5);
      pActor.addNutritionFromEating(pVal, pSetJustAte: true);
      pActor.countConsumed();
    }
    WorldTile currentTile = pActor.beh_building_target.current_tile;
    pActor.punchTargetAnimation(currentTile.posV3, false);
    return BehResult.Continue;
  }
}
