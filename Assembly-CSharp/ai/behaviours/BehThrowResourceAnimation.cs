// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehThrowResourceAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehThrowResourceAnimation : BehCityActor
{
  private string _resource_id;

  public BehThrowResourceAnimation(string pResourceId) => this._resource_id = pResourceId;

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Building behBuildingTarget = pActor.beh_building_target;
    float num = Mathf.Max(Toolbox.DistTile(pActor.current_tile, behBuildingTarget.current_tile), 1f);
    if ((double) num > 1.5)
      num = 1.5f;
    if (pActor.is_visible)
    {
      float pDuration = num;
      Vector2 pStart = Vector2.op_Implicit(pActor.getThrowStartPosition());
      Vector2 pEnd = Vector2.op_Addition(behBuildingTarget.current_position, behBuildingTarget.asset.stockpile_center_offset);
      pEnd.x += Randy.randomFloat(-0.1f, 0.1f);
      pEnd.y += Randy.randomFloat(-0.1f, 0.1f);
      BehaviourActionBase<Actor>.world.resource_throw_manager.addNew(pStart, pEnd, pDuration, this._resource_id, 1, 2f, behBuildingTarget);
    }
    return BehResult.Continue;
  }
}
