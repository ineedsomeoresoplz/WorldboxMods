// Decompiled with JetBrains decompiler
// Type: DecisionHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class DecisionHelper
{
  internal static UtilityBasedDecisionSystem decision_system = new UtilityBasedDecisionSystem();

  public static bool makeDecisionFor(Actor pActor, out string pLastDecisionID)
  {
    pLastDecisionID = string.Empty;
    if (pActor.isStatsDirty())
    {
      pActor.setTask("wait");
      return false;
    }
    DecisionAsset decisionAsset = DecisionHelper.decision_system.useOn(pActor);
    if (decisionAsset == null)
      return false;
    pLastDecisionID = decisionAsset.id;
    string pTaskId = decisionAsset.id;
    if (!string.IsNullOrEmpty(decisionAsset.task_id))
      pTaskId = decisionAsset.task_id;
    pActor.setTask(pTaskId);
    return true;
  }

  public static void runSimulation(Actor pActor)
  {
    DecisionHelper.decision_system.useOn(pActor, false);
  }

  public static void runSimulationForMindTab(Actor pActor)
  {
    DecisionHelper.decision_system.useOn(pActor, false);
  }
}
