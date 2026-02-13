// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckCityDestroyed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckCityDestroyed : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.city != null)
      return BehResult.Continue;
    if (pActor.profession_asset.cancel_when_no_city)
      pActor.stopBeingWarrior();
    pActor.endJob();
    return BehResult.Stop;
  }
}
