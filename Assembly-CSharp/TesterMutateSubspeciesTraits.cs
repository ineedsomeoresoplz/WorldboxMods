// Decompiled with JetBrains decompiler
// Type: TesterMutateSubspeciesTraits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterMutateSubspeciesTraits : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    using (ListPool<SubspeciesTrait> list = new ListPool<SubspeciesTrait>())
    {
      foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) BehaviourActionBase<AutoTesterBot>.world.subspecies)
      {
        if (!Randy.randomChance(0.9f))
        {
          list.Clear();
          list.AddRange((IEnumerable<SubspeciesTrait>) subspecies.getTraits());
          if (list.Count > 0)
            subspecies.removeTrait(list.GetRandom<SubspeciesTrait>());
          int num = 10;
          for (int index = 0; index < num; ++index)
          {
            SubspeciesTrait random = AssetManager.subspecies_traits.list.GetRandom<SubspeciesTrait>();
            if (random.can_be_given && subspecies.addTrait(random, false))
              break;
          }
        }
      }
      return base.execute(pObject);
    }
  }
}
