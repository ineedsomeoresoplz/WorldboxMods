// Decompiled with JetBrains decompiler
// Type: TestActorSimpleObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TestActorSimpleObject
{
  public Sprite sprite;
  public float pos_x;
  public float pos_y;
  public float scale_x = 1f;
  public float scale_y = 1f;
  private List<Sprite> sprites = new List<Sprite>();

  public void create(List<Sprite> pSprites)
  {
    this.sprites = pSprites;
    this.randomRespawn();
    this.setRandomSprite();
  }

  public void randomRespawn()
  {
    WorldTile random = World.world.tiles_list.GetRandom<WorldTile>();
    this.pos_x = (float) random.x;
    this.pos_y = (float) random.y;
  }

  public void update(float pElapsed) => this.randomMove(pElapsed);

  private void randomMove(float pElapsed)
  {
    this.pos_x += (float) ((double) Randy.randomFloat(-1f, 1f) * (double) pElapsed * 6.0);
    this.pos_y += (float) ((double) Randy.randomFloat(-1f, 1f) * (double) pElapsed * 6.0);
  }

  private void setRandomSprite() => this.sprite = this.sprites.GetRandom<Sprite>();
}
