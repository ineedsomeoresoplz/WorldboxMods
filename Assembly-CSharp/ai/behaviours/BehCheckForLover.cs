// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckForLover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckForLover : BehCitizenActionCity
{
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
    Actor lover = pActor.lover;
    if (lover.isTask("sexual_reproduction_civ_go") && lover.beh_building_target == pActor.beh_building_target && lover.ai.action_index > 3)
    {
      pActor.stayInBuilding(pActor.beh_building_target);
      lover.stayInBuilding(lover.beh_building_target);
      lover.setTask("sexual_reproduction_civ_wait", false, pForceAction: true);
      return this.forceTask(pActor, "sexual_reproduction_civ_action", false, true);
    }
    return !lover.isTask("sexual_reproduction_civ_go") ? BehResult.Stop : BehResult.Continue;
  }
}
