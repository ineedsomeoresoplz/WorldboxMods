// Decompiled with JetBrains decompiler
// Type: TesterMutateActorStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterMutateActorStatus : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    using (ListPool<Status> list = new ListPool<Status>())
    {
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) BehaviourActionBase<AutoTesterBot>.world.units)
      {
        if (unit.hasSubspecies() && !Randy.randomChance(0.95f))
        {
          if (unit.hasAnyStatusEffectRaw())
          {
            list.Clear();
            list.AddRange((IEnumerable<Status>) unit.getStatuses());
            if (list.Count > 0)
              unit.finishStatusEffect(list.GetRandom<Status>().asset.id);
          }
          else
          {
            int num = 10;
            do
              ;
            while (!unit.addStatusEffect(AssetManager.status.list.GetRandom<StatusAsset>(), 0.0f, true) && num-- > 0);
          }
        }
      }
      return base.execute(pObject);
    }
  }
}
