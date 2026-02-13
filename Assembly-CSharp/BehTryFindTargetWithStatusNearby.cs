// Decompiled with JetBrains decompiler
// Type: BehTryFindTargetWithStatusNearby
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehTryFindTargetWithStatusNearby : BehaviourActionActor
{
  private string[] _status_ids;

  public BehTryFindTargetWithStatusNearby(params string[] pStatusIDs)
  {
    this._status_ids = pStatusIDs;
  }

  public override BehResult execute(Actor pActor)
  {
    Actor closestActorWithStatus = this.getClosestActorWithStatus(pActor, this._status_ids);
    if (closestActorWithStatus == null)
    {
      WorldTile tileInChunk = Finder.findTileInChunk(pActor.current_tile, TileFinderType.FreeTile);
      if (tileInChunk == null)
        return BehResult.Stop;
      pActor.beh_tile_target = tileInChunk;
      return BehResult.Continue;
    }
    pActor.beh_tile_target = closestActorWithStatus.current_tile.getTileAroundThisOnSameIsland(pActor.current_tile);
    pActor.beh_actor_target = (BaseSimObject) closestActorWithStatus;
    return BehResult.Continue;
  }

  private Actor getClosestActorWithStatus(Actor pSelf, string[] pStatusIDs)
  {
    bool pRandom = Randy.randomBool();
    int num1 = int.MaxValue;
    Actor closestActorWithStatus = (Actor) null;
    foreach (Actor pTarget in Finder.getUnitsFromChunk(pSelf.current_tile, 1, pRandom: pRandom))
    {
      if (pTarget != pSelf)
      {
        int num2 = Toolbox.SquaredDistTile(pTarget.current_tile, pSelf.current_tile);
        if (num2 < num1 && pSelf.isSameIslandAs((BaseSimObject) pTarget) && pTarget.hasAnyStatusEffect())
        {
          bool flag = false;
          foreach (string pStatusId in pStatusIDs)
          {
            if (pTarget.hasStatus(pStatusId))
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            num1 = num2;
            closestActorWithStatus = pTarget;
            if (!pRandom)
            {
              if (Randy.randomBool())
                break;
            }
            else
              break;
          }
        }
      }
    }
    return closestActorWithStatus;
  }
}
