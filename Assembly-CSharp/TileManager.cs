// Decompiled with JetBrains decompiler
// Type: TileManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TileManager
{
  public int[] random_seeds;
  public int[] fire_animation_set;
  public bool[] fires;
  public Vector3[] positions_vector3;
  public int tiles_count;

  public void setup(int pWidth, int pHeight, WorldTile[,] pTilesMap)
  {
    this.tiles_count = pWidth * pHeight;
    this.setupAnimationSeeds();
    this.setupVector3Positions();
    this.setupFires();
  }

  private void setupFires()
  {
    if (this.fires != null && this.fires.Length == this.tiles_count)
      return;
    this.fires = new bool[this.tiles_count];
  }

  private void setupVector3Positions()
  {
    if (this.positions_vector3 == null || this.positions_vector3.Length != this.tiles_count)
      this.positions_vector3 = new Vector3[this.tiles_count];
    foreach (WorldTile tiles in World.world.tiles_list)
      this.positions_vector3[tiles.data.tile_id] = tiles.posV3;
  }

  private void setupAnimationSeeds()
  {
    if (this.random_seeds != null && this.random_seeds.Length == this.tiles_count)
      return;
    this.random_seeds = new int[this.tiles_count];
    this.fire_animation_set = new int[this.tiles_count];
    for (int index = 0; index < this.random_seeds.Length; ++index)
    {
      this.random_seeds[index] = Randy.randomInt(0, 10000);
      this.fire_animation_set[index] = this.random_seeds[index] % 6 != 0 ? (this.random_seeds[index] % 3 != 0 ? 0 : 1) : 2;
    }
  }

  public void clear()
  {
    bool[] fires = this.fires;
    if (fires == null)
      return;
    fires.Clear<bool>();
  }
}
