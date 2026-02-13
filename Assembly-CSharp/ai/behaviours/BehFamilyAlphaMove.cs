// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFamilyAlphaMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFamilyAlphaMove : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    WorldTile worldTile = (WorldTile) null;
    if (pActor.isHerbivore())
      worldTile = this.findTileForHerbivore(pActor);
    else if (pActor.isCarnivore())
      worldTile = this.findTileForCarnivore(pActor);
    if (worldTile != null)
      worldTile = worldTile.region.tiles.GetRandom<WorldTile>();
    if (worldTile == null)
      return this.forceTask(pActor, "random_move");
    pActor.beh_tile_target = worldTile.region.tiles.GetRandom<WorldTile>();
    return BehResult.Continue;
  }

  private Building getNearbyBuildings(WorldTile pTile)
  {
    float num1 = float.MaxValue;
    Building nearbyBuildings = (Building) null;
    foreach (Building building in Finder.getBuildingsFromChunk(pTile, 3, pRandom: true))
    {
      float num2 = (float) Toolbox.SquaredDistTile(building.current_tile, pTile);
      if ((double) num2 < (double) num1 && building.asset.flora && building.current_tile.isSameIsland(pTile))
      {
        nearbyBuildings = building;
        num1 = num2;
        if ((double) num1 < 25.0)
          return nearbyBuildings;
      }
    }
    return nearbyBuildings;
  }

  private Actor getNearbyActor(Actor pActor, WorldTile pTile)
  {
    float num1 = float.MaxValue;
    Actor nearbyActor = (Actor) null;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 3, pRandom: true))
    {
      float num2 = (float) Toolbox.SquaredDistTile(actor.current_tile, pTile);
      if ((double) num2 < (double) num1 && actor.family != pActor.family && !actor.isSameSpecies(pActor) && actor.current_tile.isSameIsland(pTile) && actor.asset.source_meat)
      {
        nearbyActor = actor;
        num1 = num2;
        if ((double) num1 < 5.0)
          return nearbyActor;
      }
    }
    return nearbyActor;
  }

  private WorldTile findTileForHerbivore(Actor pActor)
  {
    Building nearbyBuildings = this.getNearbyBuildings(pActor.current_tile);
    return nearbyBuildings != null ? nearbyBuildings.current_tile.region.tiles.GetRandom<WorldTile>() : (WorldTile) null;
  }

  private WorldTile findTileForCarnivore(Actor pActor)
  {
    WorldTile currentTile = pActor.current_tile;
    Actor nearbyActor = this.getNearbyActor(pActor, currentTile);
    if (nearbyActor != null)
      return nearbyActor.current_tile.region.tiles.GetRandom<WorldTile>();
    return nearbyActor == null ? currentTile.region.island.getRandomTile() : (WorldTile) null;
  }
}
