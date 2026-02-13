// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehUFOSelectTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehUFOSelectTarget : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    UFO actorComponent = pActor.getActorComponent<UFO>();
    if (actorComponent.aggroTargets.Count > 0)
    {
      BehaviourActionActor.temp_actors.Clear();
      foreach (Actor aggroTarget in actorComponent.aggroTargets)
      {
        if (aggroTarget != null && aggroTarget.isAlive())
          BehaviourActionActor.temp_actors.Add(aggroTarget);
      }
      actorComponent.aggroTargets.Clear();
      Actor closestActor = Toolbox.getClosestActor(BehaviourActionActor.temp_actors, pActor.current_tile);
      if (closestActor != null)
      {
        if (closestActor.city != null)
        {
          long pResult;
          pActor.data.get("cityToAttack", out pResult, -1L);
          if (!pResult.hasValue())
          {
            pActor.data.set("cityToAttack", closestActor.city.data.id);
            pActor.data.set("attacksForCity", Randy.randomInt(3, 10));
          }
        }
        pActor.beh_tile_target = closestActor.current_tile;
        return this.forceTask(pActor, "ufo_chase", false);
      }
    }
    return BehResult.Continue;
  }
}
