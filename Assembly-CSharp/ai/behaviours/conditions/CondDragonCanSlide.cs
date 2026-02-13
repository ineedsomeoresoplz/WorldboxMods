// Decompiled with JetBrains decompiler
// Type: ai.behaviours.conditions.CondDragonCanSlide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours.conditions;

public class CondDragonCanSlide : BehaviourActorCondition
{
  public override bool check(Actor pActor)
  {
    if (!pActor.getActorComponent<Dragon>().hasTargetsForSlide())
      return false;
    bool pResult;
    pActor.data.get("justSlid", out pResult);
    pActor.data.removeBool("justSlid");
    return !pResult;
  }
}
