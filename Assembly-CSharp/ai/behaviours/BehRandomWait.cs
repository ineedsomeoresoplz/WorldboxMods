// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehRandomWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehRandomWait : BehaviourActionActor
{
  private float min;
  private float max;

  public BehRandomWait(float pMin = 0.0f, float pMax = 1f, bool pLand = false)
  {
    this.min = pMin;
    this.max = pMax;
    this.land_if_hovering = pLand;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.timer_action = Randy.randomFloat(this.min, this.max);
    return BehResult.Continue;
  }
}
