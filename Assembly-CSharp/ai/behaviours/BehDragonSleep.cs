// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonSleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonSleep : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    pActor.setFlip(false);
    pActor.data.set("sleepy", 0);
    if ((double) this.dragon.sleep_time == -1.0)
      this.dragon.sleep_time = Randy.randomFloat(10f, 80f);
    this.dragon.sleep_time -= BehaviourActionBase<Actor>.world.elapsed;
    if (!pActor.hasMaxHealth() && Randy.randomChance(0.1f))
      pActor.restoreHealth(1);
    if ((double) this.dragon.sleep_time > 0.0)
      return BehResult.RepeatStep;
    this.dragon.sleep_time = -1f;
    return BehResult.Continue;
  }
}
