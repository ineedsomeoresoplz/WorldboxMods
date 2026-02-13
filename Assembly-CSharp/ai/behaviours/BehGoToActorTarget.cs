// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehGoToActorTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehGoToActorTarget : BehaviourActionActor
{
  private GoToActorTargetType _type;
  private bool _path_on_water;
  private bool _check_can_attack_target;
  private bool _check_same_island;
  private bool _check_inside_something;

  public BehGoToActorTarget(
    GoToActorTargetType pType = GoToActorTargetType.SameTile,
    bool pPathOnWater = false,
    bool pCheckCanAttackTarget = false,
    bool pCalibrateTargetPosition = false,
    float pCheckDistance = 2f,
    bool pCheckSameIsland = true,
    bool pCheckInsideSomething = true)
  {
    this._path_on_water = pPathOnWater;
    this._type = pType;
    this._check_can_attack_target = pCheckCanAttackTarget;
    this._check_same_island = pCheckSameIsland;
    this._check_inside_something = pCheckInsideSomething;
    this.calibrate_target_position = pCalibrateTargetPosition;
    this.check_actor_target_position_distance = pCheckDistance;
  }

  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_actor_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    BaseSimObject behActorTarget = pActor.beh_actor_target;
    WorldTile pTile = behActorTarget.current_tile;
    if (behActorTarget.isActor())
    {
      Actor a = behActorTarget.a;
      if (this._check_can_attack_target && !pActor.isTargetOkToAttack(a) || this._check_same_island && !pActor.isSameIslandAs((BaseSimObject) a) || this._check_inside_something && a.isInsideSomething())
        return BehResult.Stop;
    }
    switch (this._type)
    {
      case GoToActorTargetType.SameTile:
        pTile = behActorTarget.current_tile;
        break;
      case GoToActorTargetType.SameRegion:
        pTile = behActorTarget.current_tile.region.tiles.GetRandom<WorldTile>();
        break;
      case GoToActorTargetType.NearbyTile:
        pTile = behActorTarget.current_tile.getTileAroundThisOnSameIsland(pActor.current_tile);
        break;
      case GoToActorTargetType.NearbyTileClosest:
        pTile = behActorTarget.current_tile.getTileAroundThisOnSameIsland(pActor.current_tile, true);
        break;
      case GoToActorTargetType.RaycastWithAttackRange:
        pTile = this.raycastToTarget(pActor, behActorTarget);
        if (pTile == pActor.current_tile)
          return BehResult.Continue;
        break;
    }
    if (pTile == null)
    {
      pActor.ignoreTarget(behActorTarget);
      return BehResult.Stop;
    }
    if (pActor.goTo(pTile, this._path_on_water) == ExecuteEvent.True)
      return BehResult.Continue;
    pActor.ignoreTarget(behActorTarget);
    return BehResult.Stop;
  }

  private WorldTile raycastToTarget(Actor pSelf, BaseSimObject pTarget)
  {
    WorldTile currentTile1 = pSelf.current_tile;
    WorldTile currentTile2 = pTarget.current_tile;
    List<WorldTile> worldTileList = PathfinderTools.raycast(currentTile1, currentTile2);
    WorldTile target = (WorldTile) null;
    float attackRangeSquared = pSelf.getAttackRangeSquared();
    for (int index = 0; index < worldTileList.Count; ++index)
    {
      WorldTile pT1 = worldTileList[index];
      if (pT1.isSameIsland(currentTile1) && (double) Toolbox.SquaredDistTile(pT1, currentTile2) < (double) attackRangeSquared)
      {
        target = pT1;
        break;
      }
    }
    if (target == null)
      target = currentTile2;
    return target;
  }
}
