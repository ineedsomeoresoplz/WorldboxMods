// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehRepeatTaskChance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehRepeatTaskChance : BehaviourActionActor
{
  private float chance;

  public BehRepeatTaskChance(float pChance = 0.5f) => this.chance = pChance;

  public override BehResult execute(Actor pActor)
  {
    return Randy.randomChance(this.chance) ? BehResult.RestartTask : BehResult.Continue;
  }
}
