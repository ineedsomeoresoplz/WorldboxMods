// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonCheckOverTargetActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonCheckOverTargetActor : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    return WorldLawLibrary.world_law_peaceful_monsters.isEnabled() || this.dragon.aggroTargets.Count == 0 || !Dragon.canLand(pActor) || !this.dragon.targetsWithinLandAttackRange() ? BehResult.Continue : this.forceTask(pActor, "dragon_land_attack");
  }
}
