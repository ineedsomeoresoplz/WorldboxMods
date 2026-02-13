// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonIdle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonIdle : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    if (this.dragon.aggroTargets.Count > 0)
      return BehResult.Continue;
    if ((double) this.dragon.idle_time == -1.0)
      this.dragon.idle_time = Randy.randomFloat(1f, 3f);
    this.dragon.idle_time -= BehaviourActionBase<Actor>.world.elapsed;
    if ((double) this.dragon.idle_time > 0.0)
      return BehResult.RepeatStep;
    this.dragon.idle_time = -1f;
    return BehResult.Continue;
  }
}
