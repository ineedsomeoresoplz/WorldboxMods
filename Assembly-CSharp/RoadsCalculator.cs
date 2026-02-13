// Decompiled with JetBrains decompiler
// Type: RoadsCalculator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class RoadsCalculator : BaseModule
{
  public List<TileIsland> islands;
  private int current_island;
  private bool dirty = true;

  internal override void create()
  {
    base.create();
    this.islands = new List<TileIsland>();
    this.hashset = new HashSet<WorldTile>();
  }

  public void setDirty(WorldTile pTile)
  {
    if (pTile.road_island != null)
      pTile.road_island.dirty = true;
    if (pTile.Type.road)
    {
      this.hashset.Add(pTile);
    }
    else
    {
      this.hashset.Remove(pTile);
      pTile.road_island = (TileIsland) null;
    }
    this.dirty = true;
  }

  internal override void clear()
  {
    base.clear();
    this.hashset.Clear();
    this.islands.Clear();
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (!this.dirty)
      return;
    this.dirty = false;
    this.RecalculateIslands();
  }

  private void RecalculateIslands()
  {
    int index = 0;
    while (index < this.islands.Count)
    {
      TileIsland island = this.islands[index];
      if (island.dirty)
      {
        this.islands.RemoveAt(index);
        foreach (WorldTile tilesRoad in (HashSet<WorldTile>) island.tiles_roads)
          tilesRoad.road_island = (TileIsland) null;
      }
      else
        ++index;
    }
    foreach (WorldTile pTile in this.hashset)
    {
      if (pTile.road_island == null)
      {
        TileIsland pIsland = new TileIsland(this.current_island++);
        this.islands.Add(pIsland);
        this.CalcIsland(pTile, pIsland);
      }
    }
  }

  private void CalcIsland(WorldTile pTile, TileIsland pIsland)
  {
    List<WorldTile> worldTileList = new List<WorldTile>()
    {
      pTile
    };
    while (worldTileList.Count > 0)
    {
      WorldTile worldTile1 = worldTileList[0];
      worldTileList.RemoveAt(0);
      worldTile1.road_island = pIsland;
      pIsland.tiles_roads.Add(worldTile1);
      for (int index = 0; index < worldTile1.neighboursAll.Length; ++index)
      {
        WorldTile worldTile2 = worldTile1.neighboursAll[index];
        if (worldTile2.road_island == null && worldTile2.Type.road)
        {
          worldTile2.road_island = pIsland;
          worldTileList.Add(worldTile2);
        }
      }
    }
  }
}
