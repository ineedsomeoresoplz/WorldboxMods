// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSetNextTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehSetNextTask : BehaviourActionActor
{
  private bool _clean;
  private bool _force;
  private string _task_id;

  public BehSetNextTask(string taskID, bool pClean = true, bool pForce = false)
  {
    this._task_id = taskID;
    this._clean = pClean;
    this._force = pForce;
  }

  public override BehResult execute(Actor pActor)
  {
    return this.forceTask(pActor, this._task_id, this._clean, this._force);
  }
}
