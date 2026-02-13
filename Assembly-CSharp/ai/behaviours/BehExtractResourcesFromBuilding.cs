// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehExtractResourcesFromBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehExtractResourcesFromBuilding : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_building_target = true;
    this.check_building_target_non_usable = true;
  }

  public override BehResult execute(Actor pActor)
  {
    BuildingAsset asset = pActor.beh_building_target.asset;
    pActor.beh_building_target.extractResources(pActor);
    if (asset.resources_given != null)
    {
      foreach (ResourceContainer resourceContainer in asset.resources_given)
      {
        int amount = resourceContainer.amount;
        if (asset.building_type == BuildingType.Building_Mineral && pActor.hasTrait("miner") && Randy.randomBool())
          ++amount;
        pActor.addToInventory(resourceContainer.id, amount);
      }
    }
    return BehResult.Continue;
  }
}
