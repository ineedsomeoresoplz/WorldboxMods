// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehRepairInDock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace ai.behaviours;

public class BehRepairInDock : BehCityActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.check_building_target_non_usable = true;
    this.null_check_building_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (pActor.hasMaxHealth())
      return BehResult.Continue;
    int num = pActor.getMaxHealth() - pActor.getHealth();
    int pVal = num > 100 ? 100 : num;
    pActor.restoreHealth(pVal);
    float a = (float) (pVal / 25);
    pActor.timer_action = (float) Math.Ceiling((double) a);
    pActor.stayInBuilding(pActor.beh_building_target);
    pActor.beh_tile_target = (WorldTile) null;
    pActor.beh_building_target.startShake(0.5f);
    return !pActor.hasMaxHealth() ? BehResult.RepeatStep : BehResult.Continue;
  }
}
