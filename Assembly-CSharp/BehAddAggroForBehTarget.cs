// Decompiled with JetBrains decompiler
// Type: BehAddAggroForBehTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehAddAggroForBehTarget : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_actor_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    pActor.addAggro(pActor.beh_actor_target.a);
    return BehResult.Continue;
  }
}
