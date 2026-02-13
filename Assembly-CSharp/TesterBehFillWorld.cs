// Decompiled with JetBrains decompiler
// Type: TesterBehFillWorld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterBehFillWorld : BehaviourActionTester
{
  private static List<TileType> tiles = new List<TileType>();
  private static List<TopTileType> top_tiles = new List<TopTileType>();
  private string type;

  public TesterBehFillWorld(string pType) => this.type = pType;

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (TesterBehFillWorld.tiles.Count == 0)
    {
      foreach (TileType tileType in AssetManager.tiles.list)
      {
        if (tileType.can_be_autotested)
          TesterBehFillWorld.tiles.Add(tileType);
      }
      foreach (TopTileType topTileType in AssetManager.top_tiles.list)
      {
        if (topTileType.can_be_autotested)
          TesterBehFillWorld.top_tiles.Add(topTileType);
      }
    }
    TopTileType pTopType = (TopTileType) null;
    TileType pType;
    if (this.type == "random")
    {
      pTopType = TesterBehFillWorld.top_tiles.GetRandom<TopTileType>();
      pType = !pTopType.is_biome ? TesterBehFillWorld.tiles.GetRandom<TileType>() : (Randy.randomBool() ? TileLibrary.soil_high : TileLibrary.soil_low);
    }
    else
      pType = AssetManager.tiles.get(this.type);
    for (int index1 = 0; index1 < 3; ++index1)
    {
      WorldTile[] tiles = BehaviourActionBase<AutoTesterBot>.world.map_chunk_manager.chunks.GetRandom<MapChunk>().tiles;
      int length = tiles.Length;
      for (int index2 = 0; index2 < length; ++index2)
      {
        WorldTile pTile = tiles[index2];
        MapAction.terraformMain(pTile, pType, TerraformLibrary.destroy_no_flash);
        if (pTopType != null)
          MapAction.terraformTop(pTile, pTopType, TerraformLibrary.destroy_no_flash);
      }
    }
    return base.execute(pObject);
  }
}
