// Decompiled with JetBrains decompiler
// Type: JobAsset`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public abstract class JobAsset<TCondition, TSimObject> : Asset where TCondition : BehaviourBaseCondition<TSimObject>
{
  public bool random;
  public List<TaskContainer<TCondition, TSimObject>> tasks = new List<TaskContainer<TCondition, TSimObject>>();

  public void addCondition(TCondition pCondition, bool pExpectedResult = true)
  {
    this.tasks[this.tasks.Count - 1].addCondition(pCondition, pExpectedResult);
  }

  public void addTask(string pTask)
  {
    this.tasks.Add(new TaskContainer<TCondition, TSimObject>()
    {
      id = pTask
    });
  }
}
