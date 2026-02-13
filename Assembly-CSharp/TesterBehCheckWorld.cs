// Decompiled with JetBrains decompiler
// Type: TesterBehCheckWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using UnityEngine;

#nullable disable
public class TesterBehCheckWorld : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    string str = TesterBehCheckWorld.checkTestData();
    if (string.IsNullOrEmpty(str))
      return BehResult.Continue;
    Debug.Log((object) ("Errors:\n" + str));
    pObject.ai.setTask("shutdown", pCleanJob: true);
    return BehResult.Skip;
  }

  private static string checkTestData()
  {
    string empty = string.Empty;
    if (BehaviourActionBase<AutoTesterBot>.world.cities.Count == 0)
      empty += "cities list is empty - load a map with cities present\n";
    if (BehaviourActionBase<AutoTesterBot>.world.clans.Count == 0)
      empty += "clans list is empty - load a map with clans present\n";
    if (BehaviourActionBase<AutoTesterBot>.world.plots.Count == 0)
      empty += "plots list is empty - load a map with plots present\n";
    if (BehaviourActionBase<AutoTesterBot>.world.alliances.Count == 0)
      empty += "alliances list is empty - load a map with alliances present\n";
    if (BehaviourActionBase<AutoTesterBot>.world.wars.Count == 0)
      empty += "wars list is empty - load a map with wars present\n";
    if (BehaviourActionBase<AutoTesterBot>.world.kingdoms.Count == 0)
      empty += "kingdoms list is empty - load a map with cultures present\n";
    if (BehaviourActionBase<AutoTesterBot>.world.cultures.Count == 0)
      empty += "cultures list is empty - load a map with cultures present\n";
    if (BehaviourActionBase<AutoTesterBot>.world.units.Count == 0)
      empty += "units list is empty - load a map with world present\n";
    return empty + "You can only test this in the editor\n";
  }
}
