// Decompiled with JetBrains decompiler
// Type: BehRandomSocializeTopic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehRandomSocializeTopic : BehaviourActionActor
{
  private float _timer_min;
  private float _timer_max;
  private float _chance;

  public BehRandomSocializeTopic(float pMinTimer, float pMaxTimer, float pChance)
  {
    this.socialize = true;
    this._timer_min = pMinTimer;
    this._timer_max = pMaxTimer;
    this._chance = pChance;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.clearLastTopicSprite();
    if (!pActor.hasSubspecies() || !pActor.subspecies.has_advanced_communication || !Randy.randomChance(this._chance))
      return BehResult.Stop;
    float pValue = Randy.randomFloat(this._timer_min, this._timer_max);
    pActor.makeWait(pValue);
    return BehResult.Continue;
  }
}
