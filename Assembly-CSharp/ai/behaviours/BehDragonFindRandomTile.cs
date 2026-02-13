// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonFindRandomTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehDragonFindRandomTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_tile_target != null)
      return BehResult.Continue;
    WorldTile worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
    if (!BehaviourActionBase<Actor>.world.islands_calculator.hasGround())
    {
      pActor.beh_tile_target = worldTile;
      return BehResult.Continue;
    }
    if (!worldTile.Type.ground && !worldTile.Type.lava)
      worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
    if (!worldTile.Type.ground && !worldTile.Type.lava)
      worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
    if (!worldTile.Type.ground && !worldTile.Type.lava)
      worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
    if (!worldTile.Type.ground && !worldTile.Type.lava)
      worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
    if (!worldTile.Type.ground && !worldTile.Type.lava)
      worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
    if (!worldTile.Type.ground && !worldTile.Type.lava && BehaviourActionBase<Actor>.world.islands_calculator.getRandomIslandGround() != null)
      worldTile = Toolbox.getClosestTile(new List<WorldTile>()
      {
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround(),
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround(),
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround(),
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround(),
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround(),
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround(),
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround(),
        BehaviourActionBase<Actor>.world.islands_calculator.tryGetRandomGround()
      }, pActor.current_tile);
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }
}
