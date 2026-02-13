// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehActorAddExperience
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehActorAddExperience : BehaviourActionActor
{
  private int _min;
  private int _max;

  public BehActorAddExperience(int pMin, int pMax)
  {
    this._min = pMin;
    this._max = pMax;
  }

  public override BehResult execute(Actor pActor)
  {
    int pValue = Randy.randomInt(this._min, this._max + 1);
    pActor.addExperience(pValue);
    return BehResult.Continue;
  }
}
