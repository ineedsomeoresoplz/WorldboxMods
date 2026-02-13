// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerGoToCircleTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace ai.behaviours;

public class BehFingerGoToCircleTarget : BehFinger
{
  private int _min_range;
  private int _max_range;

  public BehFingerGoToCircleTarget(int pMinRange = 20, int pMaxRange = 25)
  {
    this._min_range = pMinRange;
    this._max_range = pMaxRange;
  }

  public override BehResult execute(Actor pActor)
  {
    WorldTile currentTile = pActor.current_tile;
    int num = Randy.randomInt(this._min_range, this._max_range);
    using (ListPool<WorldTile> listPool1 = new ListPool<WorldTile>()
    {
      BehaviourActionBase<Actor>.world.GetTile(currentTile.x - num / 2, currentTile.y + num / 2),
      BehaviourActionBase<Actor>.world.GetTile(currentTile.x - num, currentTile.y),
      BehaviourActionBase<Actor>.world.GetTile(currentTile.x - num / 2, currentTile.y - num / 2)
    })
    {
      using (ListPool<WorldTile> listPool2 = new ListPool<WorldTile>()
      {
        BehaviourActionBase<Actor>.world.GetTile(currentTile.x + num / 2, currentTile.y + num / 2),
        BehaviourActionBase<Actor>.world.GetTile(currentTile.x + num, currentTile.y),
        BehaviourActionBase<Actor>.world.GetTile(currentTile.x + num / 2, currentTile.y - num / 2)
      })
      {
        using (ListPool<WorldTile> listPool3 = new ListPool<WorldTile>()
        {
          BehaviourActionBase<Actor>.world.GetTile(currentTile.x - num / 2, currentTile.y + num / 2),
          BehaviourActionBase<Actor>.world.GetTile(currentTile.x, currentTile.y + num),
          BehaviourActionBase<Actor>.world.GetTile(currentTile.x + num / 2, currentTile.y + num / 2)
        })
        {
          using (ListPool<WorldTile> listPool4 = new ListPool<WorldTile>()
          {
            BehaviourActionBase<Actor>.world.GetTile(currentTile.x - num / 2, currentTile.y - num / 2),
            BehaviourActionBase<Actor>.world.GetTile(currentTile.x, currentTile.y - num),
            BehaviourActionBase<Actor>.world.GetTile(currentTile.x + num / 2, currentTile.y - num / 2)
          })
          {
            using (ListPool<ListPool<WorldTile>> list = new ListPool<ListPool<WorldTile>>()
            {
              listPool1,
              listPool2,
              listPool3,
              listPool4
            })
            {
              list.RemoveAll((Predicate<ListPool<WorldTile>>) (tList => tList.Contains((WorldTile) null)));
              if (list.Count == 0)
                return BehResult.Stop;
              ListPool<WorldTile> random = list.GetRandom<ListPool<WorldTile>>();
              return ActorMove.goToCurved(pActor, pActor.current_tile, random[0], random[1], random[2], pActor.current_tile) == ExecuteEvent.False ? BehResult.Stop : BehResult.Continue;
            }
          }
        }
      }
    }
  }
}
