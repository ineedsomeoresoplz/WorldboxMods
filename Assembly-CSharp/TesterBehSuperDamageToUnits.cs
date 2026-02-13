// Decompiled with JetBrains decompiler
// Type: TesterBehSuperDamageToUnits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehSuperDamageToUnits : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) BehaviourActionBase<AutoTesterBot>.world.units)
    {
      if (unit.asset.can_be_killed_by_stuff)
        unit.getHit(1E+17f, pAttackType: AttackType.Divine);
    }
    return base.execute(pObject);
  }
}
