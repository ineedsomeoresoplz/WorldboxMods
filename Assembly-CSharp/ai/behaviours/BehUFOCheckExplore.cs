// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehUFOCheckExplore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehUFOCheckExplore : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int pResult;
    pActor.data.get("exploringTicks", out pResult);
    if (pResult <= 0)
      return BehResult.Continue;
    int pData = pResult - 1;
    pActor.data.set("exploringTicks", pData);
    if (pActor.current_tile.zone.city != null)
    {
      pActor.data.set("cityToAttack", pActor.current_tile.zone.city.data.id);
      pActor.data.set("attacksForCity", Randy.randomInt(3, 10));
      return this.forceTask(pActor, "ufo_fly", false);
    }
    return pActor.ai.task?.id == "ufo_explore" ? BehResult.RestartTask : this.forceTask(pActor, "ufo_explore", false);
  }
}
