// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehTaxiFindShipTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;

#nullable disable
namespace ai.behaviours;

public class BehTaxiFindShipTile : BehCityActor
{
  public override BehResult execute(Actor pActor)
  {
    TaxiRequest requestForActor = TaxiManager.getRequestForActor(pActor);
    if (requestForActor == null || !requestForActor.hasAssignedBoat() || requestForActor.state != TaxiRequestState.Loading)
      return BehResult.Stop;
    Boat boat = requestForActor.getBoat();
    WorldTile worldTile = (WorldTile) null;
    if (boat.pickup_near_dock)
    {
      Building homeBuilding = boat.actor.getHomeBuilding();
      if (homeBuilding != null)
      {
        WorldTile constructionTile = homeBuilding.getConstructionTile();
        if (constructionTile != null)
          worldTile = constructionTile.region.tiles.GetRandom<WorldTile>();
      }
    }
    if (worldTile == null)
      worldTile = PathfinderTools.raycastTileForUnitToEmbark(pActor.current_tile, boat.actor.current_tile);
    if (worldTile == null)
      return BehResult.Stop;
    pActor.beh_tile_target = worldTile;
    return BehResult.Continue;
  }
}
