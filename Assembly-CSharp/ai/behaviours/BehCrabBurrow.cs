// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCrabBurrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCrabBurrow : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.force_animation = true;
    this.force_animation_id = "burrow";
    this.special_prevent_can_be_attacked = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if (pActor.isHungry())
    {
      pActor.endJob();
      return BehResult.Stop;
    }
    if (!Toolbox.hasDifferentSpeciesInChunkAround(pActor.current_tile, pActor.asset.id))
    {
      pActor.endJob();
      return BehResult.Stop;
    }
    pActor.timer_action = Randy.randomFloat(10f, 20f);
    return BehResult.RepeatStep;
  }
}
