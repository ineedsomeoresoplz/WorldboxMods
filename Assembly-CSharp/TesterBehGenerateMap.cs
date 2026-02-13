// Decompiled with JetBrains decompiler
// Type: TesterBehGenerateMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehGenerateMap : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    Config.customZoneX = 7;
    Config.customZoneY = 7;
    BehaviourActionBase<AutoTesterBot>.world.generateNewMap();
    return base.execute(pObject);
  }
}
