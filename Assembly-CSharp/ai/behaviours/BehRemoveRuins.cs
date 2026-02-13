// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehRemoveRuins
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehRemoveRuins : BehActorBuildingTarget
{
  public override BehResult execute(Actor pActor)
  {
    BuildingAsset asset = pActor.beh_building_target.asset;
    switch (asset.building_type)
    {
      case BuildingType.Building_Tree:
        if (asset.hasResourceGiven("wood"))
        {
          pActor.addToInventory("wood", 1);
          break;
        }
        break;
      case BuildingType.Building_Civ:
        if (asset.cost.wood > 0)
          pActor.addToInventory("wood", 1);
        if (asset.cost.stone > 0)
        {
          pActor.addToInventory("stone", 1);
          break;
        }
        break;
    }
    pActor.beh_building_target.startDestroyBuilding();
    pActor.addLoot(SimGlobals.m.coins_for_cleaning);
    return BehResult.Continue;
  }
}
