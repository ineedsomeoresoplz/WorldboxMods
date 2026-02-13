// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTileWhenOnFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindTileWhenOnFire : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    WorldTile waterIn = this.findWaterIn(pActor.chunk);
    if (waterIn == null)
    {
      foreach (MapChunk pChunk in pActor.chunk.neighbours_all)
      {
        waterIn = this.findWaterIn(pChunk);
        if (waterIn != null)
          break;
      }
    }
    if (waterIn == null)
      return BehResult.Stop;
    pActor.beh_tile_target = waterIn;
    return BehResult.Continue;
  }

  private WorldTile findWaterIn(MapChunk pChunk)
  {
    foreach (MapRegion mapRegion in pChunk.regions.LoopRandom<MapRegion>())
    {
      if (mapRegion.type == TileLayerType.Ocean)
        return mapRegion.tiles.GetRandom<WorldTile>();
    }
    return (WorldTile) null;
  }
}
