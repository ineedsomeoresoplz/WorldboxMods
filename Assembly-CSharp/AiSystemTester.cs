// Decompiled with JetBrains decompiler
// Type: AiSystemTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class AiSystemTester : 
  AiSystem<AutoTesterBot, JobTesterAsset, BehaviourTaskTester, BehaviourActionTester, BehaviourTesterCondition>
{
  public AiSystemTester(AutoTesterBot pObject)
    : base(pObject)
  {
    this.jobs_library = (AssetLibrary<JobTesterAsset>) AssetManager.tester_jobs;
    this.task_library = (AssetLibrary<BehaviourTaskTester>) AssetManager.tester_tasks;
  }
}
