// Decompiled with JetBrains decompiler
// Type: BehLaunchFireworks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehLaunchFireworks : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasEnoughMoney(SimGlobals.m.festive_fireworks_cost))
      return BehResult.Stop;
    pActor.finishStatusEffect("festive_spirit");
    this.spawnFireworksByUnit(pActor);
    pActor.spendMoney(SimGlobals.m.festive_fireworks_cost);
    return BehResult.Continue;
  }

  internal void spawnFireworksByUnit(Actor pActor)
  {
    EffectsLibrary.spawn("fx_fireworks", pActor.current_tile);
  }
}
