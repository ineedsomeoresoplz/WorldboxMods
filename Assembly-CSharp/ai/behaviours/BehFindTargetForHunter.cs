// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTargetForHunter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindTargetForHunter : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_actor_target != null && pActor.isTargetOkToAttack(pActor.beh_actor_target.a))
      return BehResult.Continue;
    pActor.beh_actor_target = (BaseSimObject) this.getClosestMeatActor(pActor, 3);
    return pActor.beh_actor_target != null ? BehResult.Continue : BehResult.Stop;
  }

  private Actor getClosestMeatActor(Actor pActor, int pMinAge = 0, bool pCheckSame = false)
  {
    BehaviourActionActor.temp_actors.Clear();
    foreach (Actor pTarget in Finder.getUnitsFromChunk(pActor.current_tile, 3))
    {
      if (!pTarget.isSameKingdom((BaseSimObject) pActor) && pActor.isTargetOkToAttack(pTarget) && pTarget.asset.source_meat && (pMinAge <= 0 || pTarget.getAge() >= pMinAge))
        BehaviourActionActor.temp_actors.Add(pTarget);
    }
    return Toolbox.getClosestActor(BehaviourActionActor.temp_actors, pActor.current_tile);
  }
}
