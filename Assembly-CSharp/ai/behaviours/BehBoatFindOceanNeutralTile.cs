// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoatFindOceanNeutralTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoatFindOceanNeutralTile : BehBoat
{
  public override BehResult execute(Actor pActor)
  {
    this.checkHomeDocks(pActor);
    Building homeBuilding = this.boat.actor.getHomeBuilding();
    if (homeBuilding != null)
    {
      if (pActor.getSimpleComponent<Boat>().isNearDock())
        return BehResult.Stop;
      WorldTile oceanTileInSameOcean = homeBuilding.component_docks.getOceanTileInSameOcean(pActor.current_tile);
      if (oceanTileInSameOcean != null)
      {
        pActor.beh_tile_target = oceanTileInSameOcean;
        return BehResult.Continue;
      }
    }
    WorldTile randomTileForBoat = ActorTool.getRandomTileForBoat(pActor);
    if (randomTileForBoat == null || randomTileForBoat.zone.city != null && randomTileForBoat.zone.city.kingdom.isEnemy(pActor.kingdom))
      return BehResult.Stop;
    pActor.beh_tile_target = randomTileForBoat;
    return BehResult.Continue;
  }
}
