// Decompiled with JetBrains decompiler
// Type: BehTrySleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehTrySleep : BehaviourActionActor
{
  private bool _sleep_outside;

  public BehTrySleep(bool pSleepOutside = false) => this._sleep_outside = pSleepOutside;

  public override BehResult execute(Actor pActor)
  {
    float waitTimer = this.getWaitTimer(pActor);
    pActor.makeSleep(waitTimer);
    if (pActor.hasCity() && !pActor.hasHouse() && pActor.isSapient())
      pActor.changeHappiness("slept_outside");
    return BehResult.Continue;
  }

  private float getWaitTimer(Actor pActor)
  {
    if (!pActor.hasSubspecies())
      return 20f;
    WorldAgeAsset currentAge = BehaviourActionBase<Actor>.world.era_manager.getCurrentAge();
    Subspecies subspecies = pActor.subspecies;
    bool flag = false;
    if (currentAge.flag_winter && subspecies.hasTrait("winter_slumberers"))
      flag = true;
    else if (currentAge.flag_night && subspecies.hasTrait("nocturnal_dormancy"))
      flag = true;
    else if (!currentAge.flag_chaos && subspecies.hasTrait("chaos_driven"))
      flag = true;
    else if (currentAge.flag_light_age && subspecies.hasTrait("circadian_drift"))
      flag = true;
    float waitTimer;
    if (flag)
    {
      waitTimer = 100f;
    }
    else
    {
      float pMinInclusive = 20f;
      float pMaxExclusive = 60f;
      if (subspecies.hasTrait("monophasic_sleep"))
      {
        pMinInclusive = 40f;
        pMaxExclusive = 90f;
      }
      waitTimer = Randy.randomFloat(pMinInclusive, pMaxExclusive);
      if (subspecies.hasTrait("prolonged_rest"))
        waitTimer += Randy.randomFloat(pMinInclusive, pMaxExclusive);
    }
    return waitTimer;
  }
}
