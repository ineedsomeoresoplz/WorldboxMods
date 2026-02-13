// Decompiled with JetBrains decompiler
// Type: AiSystem`5
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class AiSystem<TSimObject, TJob, TTask, TAction, TCondition>
  where TJob : JobAsset<TCondition, TSimObject>
  where TTask : BehaviourTaskBase<TAction>
  where TAction : BehaviourActionBase<TSimObject>
  where TCondition : BehaviourBaseCondition<TSimObject>
{
  public AssetLibrary<TJob> jobs_library;
  public AssetLibrary<TTask> task_library;
  private List<SingleAction<TTask, TAction>> _single_actions;
  internal int action_index;
  internal int restarts;
  internal int task_index;
  private int[] _random_tasks = new int[0];
  public TJob job;
  internal TTask task;
  internal TAction action;
  private double _timestamp_task_start;
  protected readonly TSimObject ai_object;
  public GetNextJobID next_job_delegate;
  public JobAction clear_action_delegate;
  private TaskSwitchAction _task_switch_action;
  private string _scheduled_task_id;

  public AiSystem(TSimObject pObject)
  {
    this.ai_object = pObject;
    this.next_job_delegate = new GetNextJobID(AiSystem<TSimObject, TJob, TTask, TAction, TCondition>.nextJobDefault);
  }

  public void scheduleTask(string pTaskID) => this._scheduled_task_id = pTaskID;

  public void addSingleTask(string pID)
  {
    TTask pTask = this.task_library.get(pID);
    if (this._single_actions == null)
      this._single_actions = new List<SingleAction<TTask, TAction>>();
    SingleAction<TTask, TAction> singleAction = new SingleAction<TTask, TAction>(pTask);
    this._single_actions.Add(singleAction);
    singleAction.reset();
  }

  private void updateNewBehJob()
  {
    if (this._scheduled_task_id != null)
    {
      this.setTask(this._scheduled_task_id);
      this._scheduled_task_id = (string) null;
    }
    else
    {
      if ((object) this.job == null)
        this.setJob(this.next_job_delegate());
      if (this.task_index >= this.job.tasks.Count)
        this.task_index = 0;
      TaskContainer<TCondition, TSimObject> nextTask = this.getNextTask(this.job);
      if (nextTask.has_conditions)
      {
        if (this.checkConditionsForTask(nextTask))
          this.setTask(nextTask.id);
        else
          this.setTask("nothing");
      }
      else
        this.setTask(nextTask.id);
    }
  }

  private TaskContainer<TCondition, TSimObject> getNextTask(TJob pJob)
  {
    List<TaskContainer<TCondition, TSimObject>> tasks = pJob.tasks;
    if (!pJob.random)
      return tasks[this.task_index++];
    if (this.task_index == 0 && this._random_tasks.Length != tasks.Count)
    {
      this._random_tasks = new int[tasks.Count];
      for (int index = 0; index < this._random_tasks.Length; ++index)
        this._random_tasks[index] = index;
      this._random_tasks.Shuffle<int>();
    }
    return tasks[this._random_tasks[this.task_index++]];
  }

  private bool checkConditionsForTask(
    TaskContainer<TCondition, TSimObject> pTaskContainer)
  {
    if (pTaskContainer.conditions.Count == 0)
      Debug.LogError((object) "TOO MANY COOKS");
    foreach (KeyValuePair<TCondition, bool> condition1 in pTaskContainer.conditions)
    {
      TCondition condition2;
      bool flag1;
      condition1.Deconstruct(ref condition2, ref flag1);
      TCondition condition3 = condition2;
      bool flag2 = flag1;
      if (condition3.check(this.ai_object) != flag2)
      {
        flag1 = false;
        return flag1;
      }
    }
    return true;
  }

  public void subscribeToTaskSwitch(TaskSwitchAction pAction)
  {
    this._task_switch_action += pAction;
  }

  public virtual void setTask(string pTaskId, bool pClean = true, bool pCleanJob = false, bool pForceAction = false)
  {
    if (pClean)
      this.clearBeh();
    if (pCleanJob)
    {
      this.job = default (TJob);
      this.task_index = 0;
      this.clearAction();
    }
    this.task = this.task_library.get(pTaskId);
    this.action_index = 0;
    this.restarts = 0;
    this._timestamp_task_start = World.world.getCurWorldTime();
    if (pForceAction)
      this.setAction(this.task.get(this.action_index));
    TaskSwitchAction taskSwitchAction = this._task_switch_action;
    if (taskSwitchAction == null)
      return;
    taskSwitchAction();
  }

  protected virtual void setAction(TAction pAction) => this.action = pAction;

  private void clearAction() => this.action = default (TAction);

  public void restartJob()
  {
    this.action_index = 0;
    this.task_index = 0;
    this.clearAction();
  }

  internal void clearBeh()
  {
    if (this.clear_action_delegate == null)
      return;
    this.clear_action_delegate();
  }

  public void clearJob()
  {
    this.job = default (TJob);
    this.task_index = 0;
  }

  public virtual void setJob(string pJobID)
  {
    this.job = this.jobs_library.get(pJobID);
    this.task_index = 0;
  }

  public void updateSingleTasks(float pElapsed)
  {
    if (this._single_actions == null)
      return;
    for (int index = 0; index < this._single_actions.Count; ++index)
    {
      SingleAction<TTask, TAction> singleAction = this._single_actions[index];
      singleAction.timer -= pElapsed;
      if ((double) singleAction.timer <= 0.0)
      {
        int num = (int) singleAction.task.list[0].startExecute(this.ai_object);
        singleAction.reset();
      }
    }
  }

  internal void update()
  {
    if (Bench.bench_ai_enabled)
    {
      if ((object) this.task != null)
      {
        string id = this.task.id;
      }
      double sinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
      this.run();
      double pValue = Time.realtimeSinceStartupAsDouble - sinceStartupAsDouble;
      if ((object) this.task == null)
        return;
      this.task.rate_counter_calls.registerEvent();
      this.task.rate_counter_performance.registerEvent(pValue);
    }
    else
      this.run();
  }

  public void decisionRun() => this.run();

  private void run()
  {
    if ((object) this.task == null)
    {
      this.updateNewBehJob();
      if ((object) this.task == null)
        return;
    }
    if (this.action_index >= this.task.list.Count)
    {
      this.setTaskBehFinished();
    }
    else
    {
      this.setAction(this.task.get(this.action_index));
      BehResult behResult;
      if (Bench.bench_ai_enabled)
      {
        string id = this.action.id;
        double sinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
        behResult = this.action.startExecute(this.ai_object);
        double pValue = Time.realtimeSinceStartupAsDouble - sinceStartupAsDouble;
        if ((object) this.action != null)
        {
          this.action.rate_counter_calls.registerEvent();
          this.action.rate_counter_performance.registerEvent(pValue);
        }
      }
      else
        behResult = this.action.startExecute(this.ai_object);
      if ((object) this.task == null)
        return;
      switch (behResult)
      {
        case BehResult.Continue:
          ++this.action_index;
          break;
        case BehResult.Stop:
          this.setTaskBehFinished();
          break;
        case BehResult.StepBack:
          --this.action_index;
          if (this.action_index >= 0)
            break;
          this.action_index = 0;
          break;
        case BehResult.RestartTask:
          this.action_index = 0;
          ++this.restarts;
          break;
        case BehResult.ImmediateRun:
          this.run();
          break;
      }
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool hasTask() => (object) this.task != null;

  public void setTaskBehFinished()
  {
    this.task = default (TTask);
    this.action_index = -1;
    this.clearAction();
  }

  protected virtual void debugLogAction()
  {
  }

  protected virtual void debugLogActionResult(BehResult pResult)
  {
  }

  protected string getActionID(TAction pAction)
  {
    string actionId = pAction?.GetType().ToString();
    if (actionId != null)
      actionId = actionId.Replace("ai.behaviours.", "");
    return actionId;
  }

  public void debug(DebugTool pTool)
  {
    string pT2_1 = this.getActionID(this.action);
    if (pT2_1 != null)
      pT2_1 = pT2_1.Replace("ai.behaviours.", "");
    pTool.setText("job:", (object) this.job == null ? (object) "-" : (object) this.job.id);
    int num = this.task_index + 1;
    int? nullable1 = this.job?.tasks.Count;
    int valueOrDefault = nullable1.GetValueOrDefault();
    string pT2_2 = !(num < valueOrDefault & nullable1.HasValue) ? "-" : (!this.job.random ? this.job?.tasks[this.task_index + 1].id + " (S)" : this.job?.tasks[this._random_tasks[this.task_index + 1]].id + " (R)");
    pTool.setText("next task:", (object) pT2_2);
    pTool.setSeparator();
    pTool.setText(": task:", (object) this.task?.id);
    DebugTool debugTool1 = pTool;
    string str1 = this.task_index.ToString();
    // ISSUE: variable of a boxed type
    __Boxed<TJob> job = (object) this.job;
    int? nullable2;
    if (job == null)
    {
      nullable1 = new int?();
      nullable2 = nullable1;
    }
    else
      nullable2 = new int?(job.tasks.Count);
    nullable1 = nullable2;
    string str2 = nullable1.ToString();
    string pT2_3 = $"{str1}/{str2}";
    debugTool1.setText(": task index:", (object) pT2_3);
    pTool.setSeparator();
    pTool.setText(":: action:", (object) pT2_1);
    DebugTool debugTool2 = pTool;
    string str3 = this.action_index.ToString();
    // ISSUE: variable of a boxed type
    __Boxed<TTask> task = (object) this.task;
    int? nullable3;
    if (task == null)
    {
      nullable1 = new int?();
      nullable3 = nullable1;
    }
    else
      nullable3 = new int?(task.list.Count);
    nullable1 = nullable3;
    string str4 = nullable1.ToString();
    string pT2_4 = $"{str3}/{str4}";
    debugTool2.setText(":: action index:", (object) pT2_4);
    pTool.setSeparator();
  }

  public static string nextJobDefault() => (string) null;

  internal virtual void reset()
  {
    this.jobs_library = (AssetLibrary<TJob>) null;
    this.task_library = (AssetLibrary<TTask>) null;
    this._single_actions = (List<SingleAction<TTask, TAction>>) null;
    this.action_index = 0;
    this.task_index = 0;
    this.restarts = 0;
    this.job = default (TJob);
    this.task = default (TTask);
    this.action = default (TAction);
    this.next_job_delegate = (GetNextJobID) null;
    this.clear_action_delegate = (JobAction) null;
    this._task_switch_action = (TaskSwitchAction) null;
  }

  public string getTaskTime()
  {
    return Date.formatSeconds(World.world.getWorldTimeElapsedSince(this._timestamp_task_start));
  }
}
