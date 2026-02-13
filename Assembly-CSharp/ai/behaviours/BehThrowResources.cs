// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehThrowResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehThrowResources : BehCityActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.isCarryingResources())
      return BehResult.Continue;
    Building behBuildingTarget = pActor.beh_building_target;
    float num = Mathf.Max(Toolbox.DistTile(pActor.current_tile, behBuildingTarget.current_tile), 1f);
    if ((double) num > 1.5)
      num = 1.5f;
    using (Dictionary<string, ResourceContainer>.Enumerator enumerator = pActor.inventory.getResources().GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        KeyValuePair<string, ResourceContainer> current = enumerator.Current;
        string key = current.Key;
        ResourceAsset resourceAsset = AssetManager.resources.get(key);
        if (pActor.is_visible)
        {
          int amount = current.Value.amount;
          float pDuration = num;
          Vector2 pStart = Vector2.op_Implicit(pActor.getThrowStartPosition());
          Vector2 pEnd = Vector2.op_Addition(behBuildingTarget.current_position, behBuildingTarget.asset.stockpile_center_offset);
          pEnd.x += Randy.randomFloat(-0.1f, 0.1f);
          pEnd.y += Randy.randomFloat(-0.1f, 0.1f);
          BehaviourActionBase<Actor>.world.resource_throw_manager.addNew(pStart, pEnd, pDuration, key, amount, 4f, behBuildingTarget);
        }
        pActor.takeFromInventory(key, 1);
        behBuildingTarget.addResources(key, 1);
        pActor.addLoot(resourceAsset.loot_value);
        pActor.makeWait(0.2f);
        if (pActor.isCarryingResources())
          return BehResult.StepBack;
      }
    }
    return BehResult.Continue;
  }
}
