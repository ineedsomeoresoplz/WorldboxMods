// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSpawnHeartsFromBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehSpawnHeartsFromBuilding : BehaviourActionActor
{
  private float _amount;

  public BehSpawnHeartsFromBuilding(float pAmount = 1f) => this._amount = pAmount;

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasLover())
      return BehResult.Stop;
    pActor.addAfterglowStatus();
    pActor.lover.addAfterglowStatus();
    this.spawnHearts(pActor);
    return BehResult.Continue;
  }

  private void spawnHearts(Actor pActor)
  {
    Building behBuildingTarget = pActor.beh_building_target;
    for (int index = 0; (double) index < (double) this._amount; ++index)
    {
      float num1 = (float) behBuildingTarget.current_tile.x + Randy.randomFloat(-1f, 1f);
      float num2 = (float) ((double) behBuildingTarget.current_tile.y + (double) Randy.randomFloat(0.0f, 1f) + 2.0);
      Vector3 pPos;
      // ISSUE: explicit constructor call
      ((Vector3) ref pPos).\u002Ector(num1, num2);
      EffectsLibrary.spawnAt("fx_hearts", pPos, 0.15f);
    }
  }
}
