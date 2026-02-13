// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDealDamageToTargetBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehDealDamageToTargetBuilding : BehaviourActionActor
{
  private const float DAMAGE_MULTIPLIER = 0.1f;
  private float _min;
  private float _max;

  public BehDealDamageToTargetBuilding(float pMinMultiplier, float pMaxMultiplier)
  {
    this._min = pMinMultiplier;
    this._max = pMaxMultiplier;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Building behBuildingTarget = pActor.beh_building_target;
    float num = Randy.randomFloat(this._min, this._max);
    if ((double) num <= 0.0)
      return BehResult.Continue;
    int pDamage = (int) Mathf.Max(pActor.stats["damage"] * num, 1f);
    behBuildingTarget.getHit((float) pDamage, true, AttackType.Other, (BaseSimObject) null, true, false, true);
    pActor.spawnSlash(behBuildingTarget.current_position);
    return BehResult.Continue;
  }
}
