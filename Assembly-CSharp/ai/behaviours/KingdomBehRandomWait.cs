// Decompiled with JetBrains decompiler
// Type: ai.behaviours.KingdomBehRandomWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class KingdomBehRandomWait : BehaviourActionKingdom
{
  private float min;
  private float max;

  public override bool shouldRetry(Kingdom pObject) => false;

  public KingdomBehRandomWait(float pMin = 0.0f, float pMax = 1f)
  {
    this.min = pMin;
    this.max = pMax;
  }

  public override BehResult execute(Kingdom pKingdom)
  {
    pKingdom.timer_action = Randy.randomFloat(this.min, this.max);
    return BehResult.Continue;
  }
}
