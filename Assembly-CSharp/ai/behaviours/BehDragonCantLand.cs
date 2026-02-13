// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonCantLand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonCantLand : BehaviourActionActor
{
  private string task_id;

  public BehDragonCantLand(string pNextAction) => this.task_id = pNextAction;

  public override BehResult execute(Actor pActor)
  {
    return !Dragon.canLand(pActor) ? this.forceTask(pActor, this.task_id) : BehResult.Continue;
  }
}
