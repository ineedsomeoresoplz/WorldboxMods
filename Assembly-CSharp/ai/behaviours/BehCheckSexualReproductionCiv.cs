// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckSexualReproductionCiv
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckSexualReproductionCiv : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.isKingdomCiv() && (!pActor.hasHouse() || !pActor.hasLover()))
      return BehResult.Stop;
    Actor lover = pActor.lover;
    if (!lover.canCurrentTaskBeCancelledByReproduction())
      return BehResult.Stop;
    lover.setTask("sexual_reproduction_civ_go", pCleanJob: true);
    lover.timer_action = 0.0f;
    return this.forceTask(pActor, "sexual_reproduction_civ_go");
  }
}
