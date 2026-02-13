// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindTile : BehaviourActionActor
{
  private TileFinderType _type;

  public BehFindTile(TileFinderType pType) => this._type = pType;

  public override BehResult execute(Actor pActor)
  {
    if (this._type == TileFinderType.NewRoad)
    {
      using (ListPool<WorldTile> pList = new ListPool<WorldTile>(5))
      {
        WorldTile roadTileToBuild = pActor.city.getRoadTileToBuild(pActor);
        if (roadTileToBuild != null)
          pList.Add(roadTileToBuild);
        if (pList.Count == 0)
          return BehResult.Stop;
        pActor.beh_tile_target = Randy.getRandom<WorldTile>(pList);
        return BehResult.Continue;
      }
    }
    WorldTile tileInChunk = Finder.findTileInChunk(pActor.current_tile, this._type);
    if (tileInChunk == null)
      return BehResult.Stop;
    pActor.beh_tile_target = tileInChunk;
    return BehResult.Continue;
  }
}
