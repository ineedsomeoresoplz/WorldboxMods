// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehUFOFindTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehUFOFindTarget : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    pActor.beh_tile_target = (WorldTile) null;
    int pResult1;
    pActor.data.get("attacksForCity", out pResult1);
    long pResult2;
    pActor.data.get("cityToAttack", out pResult2, -1L);
    City city = pResult2.hasValue() ? BehaviourActionBase<Actor>.world.cities.get(pResult2) : (City) null;
    if (pResult1 > 0 && city != null)
    {
      if (!city.isAlive() || city.buildings.Count == 0)
      {
        pActor.beh_tile_target = (WorldTile) null;
        pResult1 = 0;
        pActor.data.removeLong("cityToAttack");
      }
      else
      {
        Building random = city.buildings.GetRandom<Building>();
        pActor.beh_tile_target = random.current_tile.zone.tiles.GetRandom<WorldTile>();
        --pResult1;
      }
    }
    else if (pResult1 <= 0)
    {
      pActor.beh_tile_target = (WorldTile) null;
      pActor.data.removeLong("cityToAttack");
    }
    if (pResult1 > 0)
      pActor.data.set("attacksForCity", pResult1);
    else
      pActor.data.removeInt("attacksForCity");
    if (pActor.beh_tile_target == null)
    {
      WorldTile worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
      if (!BehaviourActionBase<Actor>.world.islands_calculator.hasGround())
      {
        pActor.beh_tile_target = worldTile;
        return BehResult.Continue;
      }
      if (!worldTile.Type.ground)
        worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
      if (!worldTile.Type.ground)
        worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
      if (!worldTile.Type.ground)
        worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
      if (!worldTile.Type.ground)
        worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
      if (!worldTile.Type.ground)
        worldTile = Toolbox.getRandomTileWithinDistance(pActor.current_tile, 100);
      if (!worldTile.Type.ground && BehaviourActionBase<Actor>.world.islands_calculator.getRandomIslandGround() != null)
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
    }
    return BehResult.Continue;
  }
}
