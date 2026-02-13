// Decompiled with JetBrains decompiler
// Type: TesterMutateActorTraits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterMutateActorTraits : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    using (ListPool<ActorTrait> list = new ListPool<ActorTrait>())
    {
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) BehaviourActionBase<AutoTesterBot>.world.units)
      {
        if (!Randy.randomChance(0.9f))
        {
          list.Clear();
          list.AddRange((IEnumerable<ActorTrait>) unit.getTraits());
          if (list.Count > 0)
          {
            ActorTrait random = list.GetRandom<ActorTrait>();
            if (random.can_be_removed)
              unit.removeTrait(random);
          }
          int num = 10;
          while (num-- > 0)
          {
            ActorTrait random = AssetManager.traits.list.GetRandom<ActorTrait>();
            if (random.can_be_given && !random.id.Contains("zombie") && !random.id.Contains("plague") && unit.addTrait(random))
              break;
          }
        }
      }
      return base.execute(pObject);
    }
  }
}
