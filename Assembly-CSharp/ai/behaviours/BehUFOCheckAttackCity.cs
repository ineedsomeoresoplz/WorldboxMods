// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehUFOCheckAttackCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehUFOCheckAttackCity : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    long pResult1;
    pActor.data.get("cityToAttack", out pResult1, -1L);
    if (pResult1.hasValue() && pActor.current_tile.hasBuilding() && !WorldLawLibrary.world_law_peaceful_monsters.isEnabled() && pActor.current_tile.building.isUsable())
      return this.forceTask(pActor, "ufo_attack");
    int pResult2;
    pActor.data.get("attacksForCity", out pResult2);
    return pResult2 > 0 ? BehResult.RestartTask : BehResult.Continue;
  }
}
