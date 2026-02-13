// Decompiled with JetBrains decompiler
// Type: TesterMutateSubspeciesGenes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterMutateSubspeciesGenes : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) BehaviourActionBase<AutoTesterBot>.world.subspecies)
    {
      if (!Randy.randomChance(0.9f))
      {
        subspecies.nucleus.doRandomGeneMutations(2);
        subspecies.mutateTraits(1);
        subspecies.unstableGenomeEvent();
      }
    }
    return base.execute(pObject);
  }
}
