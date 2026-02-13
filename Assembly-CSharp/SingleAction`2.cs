// Decompiled with JetBrains decompiler
// Type: SingleAction`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SingleAction<TTask, TAction>
  where TTask : BehaviourTaskBase<TAction>
  where TAction : BehaviourElementAI
{
  public float timer;
  public float interval = 1f;
  public float interval_random = 1f;
  public TTask task;

  public SingleAction(TTask pTask)
  {
    this.task = pTask;
    this.interval = pTask.single_interval;
    this.interval_random = pTask.single_interval_random;
  }

  public void reset() => this.timer = this.interval + Randy.randomFloat(0.0f, this.interval_random);
}
