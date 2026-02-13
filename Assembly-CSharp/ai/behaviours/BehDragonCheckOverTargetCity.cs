// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonCheckOverTargetCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonCheckOverTargetCity : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    if (WorldLawLibrary.world_law_peaceful_monsters.isEnabled())
      return BehResult.Continue;
    int pResult1;
    pActor.data.get("attacksForCity", out pResult1);
    if (pResult1 == 0)
      return BehResult.Continue;
    long pResult2;
    pActor.data.get("cityToAttack", out pResult2, -1L);
    if ((pResult2.hasValue() ? BehaviourActionBase<Actor>.world.cities.get(pResult2) : (City) null) == null || Randy.randomChance(0.8f))
      return BehResult.Continue;
    int num;
    if (pActor.isFlying() && !Dragon.canLand(pActor) && this.dragon.hasTargetsForSlide() && Randy.randomBool())
    {
      pActor.data.set("attacksForCity", num = pResult1 - 1);
      return this.forceTask(pActor, "dragon_slide");
    }
    if (pActor.isFlying() || !Dragon.canLand(pActor))
      return BehResult.Continue;
    pActor.data.set("attacksForCity", num = pResult1 - 1);
    return this.forceTask(pActor, "dragon_land_attack");
  }
}
