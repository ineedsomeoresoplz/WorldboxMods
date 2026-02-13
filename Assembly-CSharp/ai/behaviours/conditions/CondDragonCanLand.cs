// Decompiled with JetBrains decompiler
// Type: ai.behaviours.conditions.CondDragonCanLand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours.conditions;

public class CondDragonCanLand : BehaviourActorCondition
{
  public override bool check(Actor pActor)
  {
    if (!Dragon.canLand(pActor) || pActor.getActorComponent<Dragon>().lastLanded == pActor.current_tile)
      return false;
    bool pResult;
    pActor.data.get("justUp", out pResult);
    if (!pResult)
      return true;
    pActor.data.removeBool("justUp");
    return false;
  }
}
