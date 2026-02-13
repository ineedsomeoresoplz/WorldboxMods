// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehActorTryToAddRandomCombatSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehActorTryToAddRandomCombatSkill : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!Randy.randomChance(0.15f))
      return BehResult.Stop;
    ActorTrait random = AssetManager.traits.pot_traits_combat.GetRandom<ActorTrait>();
    pActor.addTrait(random);
    return BehResult.Continue;
  }
}
