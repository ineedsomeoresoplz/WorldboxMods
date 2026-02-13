// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehActorCheckBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehActorCheckBool : BehaviourActionActor
{
  private string actionIfBool;
  private string boolCheck;

  public BehActorCheckBool(string pBool, string pActionIfHit)
  {
    this.actionIfBool = pActionIfHit;
    this.boolCheck = pBool;
  }

  public override BehResult execute(Actor pActor)
  {
    bool pResult;
    pActor.data.get(this.boolCheck, out pResult);
    if (!pResult)
      return BehResult.Continue;
    pActor.data.removeBool(this.boolCheck);
    return this.forceTask(pActor, this.actionIfBool);
  }
}
