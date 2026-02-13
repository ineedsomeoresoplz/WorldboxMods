// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehAnimalBreedingTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehAnimalBreedingTime : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_actor_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    if ((double) Toolbox.DistTile(pActor.current_tile, pActor.beh_actor_target.current_tile) > 1.0)
      return BehResult.Stop;
    pActor.beh_actor_target.a.startShake();
    pActor.startShake();
    pActor.beh_actor_target.a.timer_action = 2f;
    EffectsLibrary.spawnAt("fx_hearts", pActor.current_position, 0.15f);
    return BehResult.Continue;
  }
}
