// Decompiled with JetBrains decompiler
// Type: BehFindRandomFrontTileNearHouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehFindRandomFrontTileNearHouse : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    Building homeBuilding = pActor.getHomeBuilding();
    if (homeBuilding == null)
      return BehResult.Stop;
    WorldTile doorTile = homeBuilding.door_tile;
    if (!doorTile.isSameIsland(pActor.current_tile))
    {
      if (!homeBuilding.current_tile.isSameIsland(pActor.current_tile))
        return BehResult.Stop;
      pActor.beh_tile_target = homeBuilding.current_tile;
      return BehResult.Continue;
    }
    using (ListPool<WorldTile> list = new ListPool<WorldTile>())
    {
      for (int index = 0; index < 3; ++index)
      {
        WorldTile tile = BehaviourActionBase<Actor>.world.GetTile(doorTile.x + index, doorTile.y);
        if (tile != null && doorTile.isSameIsland(tile))
          list.Add(tile);
      }
      for (int index = 0; index < 3; ++index)
      {
        WorldTile tile = BehaviourActionBase<Actor>.world.GetTile(doorTile.x - index, doorTile.y);
        if (tile != null && doorTile.isSameIsland(tile))
          list.Add(tile);
      }
      if (list.Count == 0)
        return BehResult.Stop;
      WorldTile random = list.GetRandom<WorldTile>();
      pActor.beh_tile_target = random;
      return BehResult.Continue;
    }
  }
}
