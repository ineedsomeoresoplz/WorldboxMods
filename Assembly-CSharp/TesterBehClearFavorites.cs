// Decompiled with JetBrains decompiler
// Type: TesterBehClearFavorites
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehClearFavorites : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pActor)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) BehaviourActionBase<AutoTesterBot>.world.units)
    {
      if (!unit.isRekt())
        unit.data.favorite = false;
    }
    return base.execute(pActor);
  }
}
